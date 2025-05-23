using System.Runtime.InteropServices;
using System.Text;
using JellyBins.Abstractions;
using JellyBins.NewExecutable.Headers;
using JellyBins.NewExecutable.Private;


namespace JellyBins.NewExecutable.Models;

public class NeFileDumper : IFileDumper
{
    public FileSegmentationType SegmentationType { get; } = FileSegmentationType.NewExecutable;
    public NeInfo? Info { get; private set; }
    public BaseDump<MzHeader> MzHeaderDump { get; private set; }
    public NeDump NeHeaderDump { get; private set; }
    public NeSegmentDump[] SegmentsTableDump { get; private set; }
    public NeImportDump[] ImportsTableDump { get; private set; }
    public NeExportDump[] ExportsTableDump { get; private set; }
    public NeEntryDump[] EntriesDump { get; private set; }

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
        
        FindBinaryTypeId();
        FindExtensionTypeId();
        FindCharacteristics();
        
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
                Name = "NE Import (JellyBins: IMAGE_IMPORT_MODULE)",
                Characteristics = ["IMAGE_IMPORT_MODULE"]
            };
            
            imports.Add(module);

        }
        
        ImportsTableDump = imports.ToArray();
        
        stream.Seek((Int64)(NeHeaderDump.Address + NeHeaderDump.Segmentation.enttab), SeekOrigin.Begin);
        
        // FIXME: raw bytes instead ASCII FFI
        FindBinaryEntries(reader);
        FindNonResidentNames(reader);

        reader.Close();
    }
    private void FindBinaryEntries(BinaryReader reader)
    {
        List<NeEntryDump> entries = [];
        
        Int64 modOffset = (Int64)(NeHeaderDump.Address + NeHeaderDump.Segmentation.modtab)!;
        reader.BaseStream.Seek(modOffset, SeekOrigin.Begin);
        
        UInt16[] moduleOffsets = new UInt16[NeHeaderDump.Segmentation.cmod];
        for (Int32 i = 0; i < moduleOffsets.Length; i++)
        {
            moduleOffsets[i] = reader.ReadUInt16();
        }

        // Обработка каждого модуля
        foreach (UInt16 offset in moduleOffsets)
        {
            Int64 importOffset = (Int64)(NeHeaderDump.Address + NeHeaderDump.Segmentation.imptab + offset)!;
            reader.BaseStream.Seek(importOffset, SeekOrigin.Begin);

            Byte modNameLength = reader.ReadByte();
            String modName = Encoding.ASCII.GetString(reader.ReadBytes(modNameLength));

            NeEntryDump entryDump = new()
            {
                Name = "NE Entry (JellyBins: IMAGE_MODULE_ENTRIES)",
                Address = (UInt64)reader.BaseStream.Position,
                Size = 0,
                Segmentation = new List<NeEntry>()
            };
            
            while (true)
            {
                if (reader.BaseStream.Position >= reader.BaseStream.Length)
                    break;

                Byte funcDescriptor = reader.ReadByte();
                if (funcDescriptor == 0)
                    break;

                Boolean isOrdinal = (funcDescriptor & 0x80) != 0;
                if (isOrdinal)
                {
                    Byte lowByte = reader.ReadByte();
                    UInt16 ordinal = (UInt16)((funcDescriptor & 0x7F) << 8 | lowByte);

                    entryDump.Segmentation!.Add(new NeEntry()
                    {
                        ModuleName = modName,
                        IsByOrdinal = true,
                        Ordinal = ordinal,
                    });
                    entryDump.Characteristics = ["MOD_IMPORT_ORDINAL"];
                }
                else
                {
                    Byte funcNameLength = funcDescriptor;
                    String funcName = Encoding.ASCII.GetString(reader.ReadBytes(funcNameLength));
                    
                    entryDump.Segmentation!.Add(new NeEntry
                    {
                        IsByOrdinal = false,
                        ModuleName = modName,
                        Name = funcName,
                    });
                    entryDump.Characteristics = ["MOD_IMPORT_FUNCTION"];
                }
            }

            entries.Add(entryDump);
        }

        EntriesDump = entries.ToArray();
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
        if ((NeHeaderDump.Segmentation.pflags & 0x4) != 0)
        {
            chars.Add("PROGRAM_IA_8086");
            Info!.CpuArchitecture = "Intel 8086";
        }

        if ((NeHeaderDump.Segmentation.pflags & 0x5) != 0)
        {
            chars.Add("PROGRAM_IA_80286");
            Info!.CpuArchitecture = "Intel i286";
        }
        if ((NeHeaderDump.Segmentation.pflags & 0x6) != 0)
        {
            chars.Add("PROGRAM_IA_80386");
            Info!.CpuArchitecture = "Intel i386";
        }
        if ((NeHeaderDump.Segmentation.pflags & 0x7) != 0)
        {
            chars.Add("PROGRAM_IA_8087");
            Info!.CpuArchitecture = "Intel 8087";
        }
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

        String osVersion = $"{NeHeaderDump.Segmentation.major}.{NeHeaderDump.Segmentation.minor}";
        // String binVersion = $"{NeHeaderDump.Segmentation.ver}.{NeHeaderDump.Segmentation.rev}";
        Info!.OperatingSystemVersion = osVersion;
        Info.BinaryType = _binaryTypeId;
        Info.ExtensionType = _extensionTypeId;
        Info.OperatingSystem = NeHeaderDump.Segmentation.os switch
        {
            0x1 => "IBM OS/2",
            0x2 => "Microsoft Windows/286",
            0x3 => "Microsoft DOS",
            0x4 => "Microsoft Windows/386",
            0x5 => "Borland OSServices",
            _ => "Microsoft Windows/286"
        };
        if (NeHeaderDump.Segmentation is { major: 0, minor: 0 })
        {
            // мае любимае гадание на гуще кафея
            Info.OperatingSystemVersion = NeHeaderDump.Segmentation.os switch
            {
                0x1 => "1.1",
                0x2 => "2.1",
                0x3 => "4.0",
                0x4 => "3.1",
                0x5 => "5.0",
                _ => "1.0"
            };
        }
        
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
    private void FindNonResidentNames(BinaryReader reader)
    {
        List<NeExportDump> exports = new();
        
        if (NeHeaderDump.Segmentation.cbnrestab == 0)
            return;
        
        reader.BaseStream.Seek(NeHeaderDump.Segmentation.nrestab, SeekOrigin.Begin);

        Byte i;
        while ((i = reader.ReadByte()) != 0)
        {
            String name = Encoding.ASCII.GetString(reader.ReadBytes(i));
            UInt16 ordinal = reader.ReadUInt16();
            exports.Add(new NeExportDump()
            {
                Name = "IMAGE_NONRESIDENT_TABLE_ENTRY",
                Segmentation = new NeExport()
                {
                    Count = i,
                    Name = name,
                    Ordinal = ordinal
                }
            });
        }

        ExportsTableDump = exports.ToArray();
        Info!.ProjectDescription = exports[0].Name;
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