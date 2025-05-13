using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using JellyBins.Abstractions;
using JellyBins.NewExecutable.Headers;
using JellyBins.NewExecutable.Private;
using JellyBins.NewExecutable.Private.Types;

namespace JellyBins.NewExecutable.Models;

public class NeFileDumper : IFileDumper
{ 
    public NeInfo? Info { get; private set; }
    public BaseDump<MzHeader> MzHeaderDump { get; private set; }
    public NeDump NeHeaderDump { get; private set; }
    public NeSegmentDump[] SegmentsTableDump { get; private set; }
    public NeImportDump[] ImportsTableDump { get; private set; }

    private UInt16 _extensionTypeId;
    private UInt16 _binaryTypeId;

    public NeFileDumper(String path)
    {
        Info = new()
        {
            Name = new FileInfo(path).Name,
            Path = path
        };
        MzHeaderDump = new();
        NeHeaderDump = new();
        SegmentsTableDump = [];
    }

    public void Dump()
    {
        using FileStream stream = new(Info!.Path!, FileMode.Open, FileAccess.Read);
        using BinaryReader reader = new(stream);

        // IMAGE_DOS_HEADER construct
        MzHeaderDump.Segmentation = Fill<MzHeader>(reader);
        MzHeaderDump.Address = 0;
        MzHeaderDump.Size = SizeOf(MzHeaderDump.Segmentation);
        MzHeaderDump.Name = "MZ Header (WinAPI: IMAGE_DOS_HEADER)";

        // IMAGE_OS2_HEADER construct
        stream.Seek(MzHeaderDump.Segmentation.e_lfanew, SeekOrigin.Begin);
        NeHeaderDump.Segmentation = Fill<NeHeader>(reader);
        NeHeaderDump.Address = MzHeaderDump.Segmentation.e_lfanew;
        NeHeaderDump.Size = SizeOf(NeHeaderDump.Segmentation);
        NeHeaderDump.Name = "NE Header (WinAPI: IMAGE_OS2_HEADER)";
        
        FindCharacteristics();
        FindBinaryTypeId();
        FindExtensionTypeId();
        
        // NE binary segmentation construct
        Int64 segmentsOffset = (Int64)(NeHeaderDump.Address + NeHeaderDump.Segmentation.segtab);

        stream.Seek(segmentsOffset, 0);

        List<NeSegmentDump> segTable = [];

        for (Int32 i = 0; i < NeHeaderDump.Segmentation.cseg; i++)
        {
            NeSegmentDump segment = new()
            {
                Address = (UInt64)stream.Position,
                Segmentation = Fill<NeSegmentInfo>(reader)
            };
            segment.Size = SizeOf(segment.Segmentation);
            segment.Name = segment.Segmentation.Type;

            FindSegmentCharacteristics(ref segment);
            
            segTable.Add(segment);
        }
        SegmentsTableDump = segTable.ToArray();

        
        List<NeImportDump> imports = [];
        UInt16[] modOffsets = new UInt16[NeHeaderDump.Segmentation.cmod];
        
        stream.Seek((Int64)(NeHeaderDump.Address + NeHeaderDump.Segmentation.modtab), SeekOrigin.Begin);
        
        for (Int32 i = 0; i < NeHeaderDump.Segmentation.cmod; i++)
        {
            modOffsets[i] = reader.ReadUInt16();
        }
        foreach (UInt16 offset in modOffsets)
        {
            stream.Seek((Int64)(NeHeaderDump.Address + NeHeaderDump.Segmentation.imptab + offset), SeekOrigin.Begin);
            Byte nameLength = reader.ReadByte();
            String moduleName = new(reader.ReadChars(nameLength));

            NeImportDump module = new()
            {
                Segmentation = new NeImport()
                {
                    Name = moduleName,
                    NameLength = nameLength
                },
                Address = (UInt64)stream.Position,
                Name = moduleName,
                Characteristics = ["IMAGE_IMPORTED_MOD"]
            };
            
            imports.Add(module);

        }

        ImportsTableDump = imports.ToArray();
        reader.Close();
    }

    private void FindSegmentCharacteristics(ref NeSegmentDump segment)
    {
        List<String> chars = [];
        
        if ((segment.Segmentation.Flags & (UInt16)NeSegmentType.WithinRelocations) != 0) chars.Add("SEG_WITHIN_RELOCS");
        if ((segment.Segmentation.Flags & (UInt16)NeSegmentType.Mask) != 0) chars.Add("SEG_HASMASK");
        if ((segment.Segmentation.Flags & (UInt16)NeSegmentType.DiscardPriority) != 0) chars.Add("SEG_DISCARDABLE");
        if ((segment.Segmentation.Flags & (UInt16)NeSegmentType.Movable) != 0) chars.Add("SEG_MOVABLE_BASE");
        if ((segment.Segmentation.Flags & (UInt16)NeSegmentType.Data) != 0) chars.Add("SEG_DATA");
        if ((segment.Segmentation.Flags & (UInt16)NeSegmentType.Code) != 0) chars.Add("SEG_CODE");

        segment.Characteristics = chars.ToArray();
    }
    private void FindCharacteristics()
    {
        List<String> chars = [];
        if ((NeHeaderDump.Segmentation.pflags & 0x4) != 0) chars.Add("PROGRAM_IA_8086");
        if ((NeHeaderDump.Segmentation.pflags & 0x5) != 0) chars.Add("PROGRAM_IA_80286");
        if ((NeHeaderDump.Segmentation.pflags & 0x6) != 0) chars.Add("PROGRAM_IA_80386");
        if ((NeHeaderDump.Segmentation.pflags & 0x7) != 0) chars.Add("PROGRAM_IA_8087");
        
        if ((NeHeaderDump.Segmentation.pflags & 0x01) != 0) chars.Add("PROGRAM_SINGLE_TASK");
        if ((NeHeaderDump.Segmentation.pflags & 0x02) != 0) chars.Add("PROGRAM_MULTI_TASK");
        if ((NeHeaderDump.Segmentation.pflags & 0x08) != 0) chars.Add("PROGRAM_PROTECTED_MODE");
        if ((NeHeaderDump.Segmentation.pflags & 0x80) != 0) chars.Add("PROGRAM_FASTLOAD_AREA");

        if ((NeHeaderDump.Segmentation.aflags & 0x01) != 0) chars.Add("APP_FULLSCREEN");
        if ((NeHeaderDump.Segmentation.aflags & 0x02) != 0) chars.Add("APP_WINPM_COMPAT");
        chars.Add((NeHeaderDump.Segmentation.aflags & 0x08) != 0 ? "APP_LONGNAMES" : "APP_RULE83");
        if ((NeHeaderDump.Segmentation.aflags & 0x10) != 0) chars.Add("APP_REAL_MODE");
        if ((NeHeaderDump.Segmentation.aflags & 0x20) != 0) chars.Add("APP_PROTECTED_MODE");
        
        if ((NeHeaderDump.Segmentation.flagsothers & 0x01) != 0) chars.Add("OS2_APPLICATION");
        if ((NeHeaderDump.Segmentation.flagsothers & 0x02) != 0) chars.Add("OS2_FAMILY_APPLICATION");
        if ((NeHeaderDump.Segmentation.flagsothers & 0x04) != 0) chars.Add("OS2_LONGNAMES");
        if ((NeHeaderDump.Segmentation.flagsothers & 0x08) != 0) chars.Add("OS2_REAL_MODE");
        if ((NeHeaderDump.Segmentation.flagsothers & 0x10) != 0) chars.Add("OS2_2X_PROTECTED_MODE");
        
        NeHeaderDump.Characteristics = chars.ToArray();
    }

    private void FindBinaryTypeId()
    {
        if (NeHeaderDump.Segmentation.cseg == 0)
            _binaryTypeId = (UInt16)FileType.DynamicLibrary;

        _binaryTypeId = (UInt16)FileType.Application;
    }

    private void FindExtensionTypeId()
    {
        String ext = Path.GetExtension(Info!.Path!);
        if (String.Equals(ext, "dll", StringComparison.OrdinalIgnoreCase))
            _extensionTypeId = (UInt16)FileType.DynamicLibrary;
        else if (String.Equals(ext, ".exe", StringComparison.OrdinalIgnoreCase))
            _extensionTypeId = (UInt16)FileType.Application;
        else if (String.Equals(ext, ".drv", StringComparison.OrdinalIgnoreCase))
            _extensionTypeId = (UInt16)FileType.Driver;
    }
    
    private TStruct Fill<TStruct>(BinaryReader reader) where TStruct : struct
    {
        Byte[] bytes = reader.ReadBytes(Marshal.SizeOf(typeof(TStruct)));
        GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
        TStruct result = Marshal.PtrToStructure<TStruct>(handle.AddrOfPinnedObject());
        handle.Free();
        return result;
    }

    private Int32 SizeOf<TStruct>(TStruct structure) where TStruct : struct
    {
        return Marshal.SizeOf(structure);
    }

    public UInt16 GetExtensionTypeId()
    {
        return _extensionTypeId;
    }

    public UInt16 GetBinaryTypeId()
    {
        return _binaryTypeId;
    }
}