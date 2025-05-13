using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using JellyBins.Abstractions;
using JellyBins.NewExecutable.Headers;

namespace JellyBins.NewExecutable.Models;

public class NeFileDumper : IFileDumper
{ 
    public NeInfo? Info { get; private set; }
    public BaseDump<MzHeader> MzHeaderDump { get; private set; }
    public BaseDump<NeHeader> NeHeaderDump { get; private set; }
    public BaseDump<NeSegmentInfo>[] SegmentsTableDump { get; private set; }

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

        // NE binary segmentation construct
        Int64 segmentsOffset = (Int64)(NeHeaderDump.Address + NeHeaderDump.Segmentation.segtab);

        stream.Seek(segmentsOffset, 0);

        List<BaseDump<NeSegmentInfo>> segTable = [];

        for (Int32 i = 0; i < NeHeaderDump.Segmentation.cseg; i++)
        {
            BaseDump<NeSegmentInfo> segment = new();
            segment.Address = (UInt64)stream.Position;
            segment.Segmentation = Fill<NeSegmentInfo>(reader);
            segment.Size = SizeOf(segment.Segmentation);
            
            segTable.Add(segment);
        }

        SegmentsTableDump = segTable.ToArray();
        
        
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