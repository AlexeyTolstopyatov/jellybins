using System.Runtime.InteropServices;
using System.Text;
using JellyBins.Abstractions;
using JellyBins.LinearExecutable.Headers;
using JellyBins.LinearExecutable.Private;
using JellyBins.LinearExecutable.Private.Types;

namespace JellyBins.LinearExecutable.Models;

public class LeFileDumper(String path) : IFileDumper
{
    public FileSegmentationType SegmentationType { get; init; } = FileSegmentationType.LinearExecutable;
    private UInt16 _extensionTypeId = 0, _binaryTypeId = 0;
    public Dictionary<UInt16, String> ResidentTable { get; private set; } = [];
    public String[] ImportModulesTable { get; private set; } = [];
    public String[] ImportProceduresTable { get; private set; } = [];
    public LeFileInfo Info { get; private set; } = new()
    {
        Name = new FileInfo(path).Name,
        Path = path,
        CpuWordLength = 16,
    };
    public MzHeaderDump MzHeaderDump { get; private set; } = new();
    public LeHeaderDump LeHeaderDump { get; private set; } = new();
    public LeObjectDump[] ObjectsTableDump { get; private set; } = [];
    public LeDirectiveDump[] DirectiveTableDumps { get; private set; } = [];
    public LeExportsDump ExportsDump { get; private set; } = new();
    public LeImportsDump ImportsDump { get; private set; } = new();
    private VirtualAddress Lva { get; set; } = new(0);
    public LeEntriesDumps EntriesDumps { get; private set; } = new()
    {
        Entries16Bit = new() { Segmentation = new() },
        EntriesCallGate = new() { Segmentation = new() },
        Entries32Bit = new() { Segmentation = new() },
        EntriesForwarder = new(){ Segmentation = new()}
    };

    /// <summary>
    /// Makes dumps of required binary (DOESN'T CHECK start of file).
    /// So if you try to make dumps of PE binary - this function gives you memory garbage
    /// and it all.
    /// </summary>
    public void Dump()
    {
        using FileStream stream = new(Info.Path!, FileMode.Open, FileAccess.Read);
        using BinaryReader reader = new(stream);
        
        // IMAGE_DOS_HEADER construct
        MzHeaderDump.Segmentation = Fill<MzHeader>(reader);
        MzHeaderDump.Address = 0;
        MzHeaderDump.Size = SizeOf(MzHeaderDump.Segmentation);
        MzHeaderDump.Name = "MZ Header (WinAPI: IMAGE_DOS_HEADER)";

        // LX_HEADER construct (sources: os2.h emm386.h vmm386.h)
        stream.Seek(MzHeaderDump.Segmentation.e_lfanew, SeekOrigin.Begin);
        LeHeaderDump.Segmentation = Fill<LeHeader>(reader);
        LeHeaderDump.Address = MzHeaderDump.Segmentation.e_lfanew;
        LeHeaderDump.Size = SizeOf(LeHeaderDump.Segmentation);
        LeHeaderDump.Name = "LE Header (OS/2 API: LE_HEADER)";

        Lva = new VirtualAddress((UInt64)LeHeaderDump.Address);
        
        // write addition module information
        FindCharacteristics();
        FindExtensionType();
        FindBinaryType();
        // other details iteration (sources: os2.h)
        FindIterateObjectEntries(reader);
        FindIterateDirectives(reader);
        FindImports(reader);
        FindNonResidentNames(reader);
        FindResidentNames(reader);
        FindEntries(reader);
        
        reader.Close();
    }

    #region Private methods
    private void FindCharacteristics()
    {
        UInt32 moduleFlags = LeHeaderDump.Segmentation.ModuleTypeFlags;
        List<String> chars = [];
        if ((moduleFlags & 0x00000000) != 0)
            chars.Add("MODULE_EXECUTABLE");
        if ((moduleFlags & 0x00008000) != 0)
            chars.Add("MODULE_DLL");
        if ((moduleFlags & 0x00010000) != 0)
            chars.Add("MODULE_MEM_PROTECTED");
        if ((moduleFlags & 0x00020000) != 0)
            chars.Add("MODULE_DRIVER");
        if ((moduleFlags & 0x00028000) != 0)
            chars.Add("MODULE_DRIVER_VIRTUAL");
        if ((moduleFlags & 0x00038000) != 0)
            chars.Add("MODULE_MODMASK");
        if ((moduleFlags & 0x00002000) != 0)
            chars.Add("MODULE_LNKERR");
        if ((moduleFlags & 0x00000010) != 0)
            chars.Add("MODULE_FIXUPS_INTERNAL_STRIPPED");
        if ((moduleFlags & 0x00000008) != 0)
            chars.Add("MODULE_DLL_SYS");
        if ((moduleFlags & 0x00000020) != 0)
            chars.Add("MODULE_FIXUPS_EXTERNAL_STRIPPED");
        if ((moduleFlags & 0x00000004) != 0)
            chars.Add("MODULE_LIB_INIT");
        if ((moduleFlags & 0x40000000) != 0)
            chars.Add("MODULE_LIB_TERM");
        if ((moduleFlags & 0x00008000) != 0)
            chars.Add("MODULE_MULTICPU_UNSAFE");
        if ((moduleFlags & 0x00000100) != 0)
            chars.Add("MODULE_WND_FULLSCREEN");
        if ((moduleFlags & 0x00000200) != 0)
            chars.Add("MODULE_WND_OS2PM_COMPAT");
        if ((moduleFlags & 0x00000300) != 0)
            chars.Add("MODULE_WMD_OS2PM_REQUIRED");

        LeHeaderDump.Characteristics = chars.ToArray();
        
        Info.OperatingSystem = LeHeaderDump.Segmentation.OSType switch
        {
            0x1 => "IBM OS/2",
            0x2 => "Microsoft Windows/286",
            0x3 => "DOS/4",
            0x4 => "Microsoft Windows/386",
            0x5 => "IBM Personality",
            _ => "?"
        };

        Info.CpuArchitecture = LeHeaderDump.Segmentation.CPUType switch
        {
            0x01 => "Intel i286",
            0x02 => "Intel i386",
            0x03 => "Intel i486",
            0x04 => "Intel i568",
            0x20 => "Intel i860",
            0x40 => "MIPS Mark I",
            0x41 => "MIPS Mark II",
            0x42 => "MIPS Mark III",
            _ => "?"
        };
    }
    private void FindExtensionType()
    {
        String ext = Path.GetExtension(Info!.Path!);
        if (String.Equals(ext, "dll", StringComparison.OrdinalIgnoreCase))
            _extensionTypeId = (UInt16)FileType.DynamicLibrary;
        else if (String.Equals(ext, ".exe", StringComparison.OrdinalIgnoreCase))
            _extensionTypeId = (UInt16)FileType.Application;
        else if (String.Equals(ext, ".drv", StringComparison.OrdinalIgnoreCase))
            _extensionTypeId = (UInt16)FileType.Driver;
    }
    private void FindBinaryType()
    {
        if ((LeHeaderDump.Segmentation.ModuleTypeFlags & 0x00000000) != 0)
            _binaryTypeId = (UInt16)FileType.Application;
        if ((LeHeaderDump.Segmentation.ModuleTypeFlags & 0x00008000) != 0)
            _binaryTypeId = (UInt16)FileType.DynamicLibrary;
        if ((LeHeaderDump.Segmentation.ModuleTypeFlags & 0x00020000) != 0)
            _binaryTypeId = (UInt16)FileType.Driver;
        if ((LeHeaderDump.Segmentation.ModuleTypeFlags & 0x00028000) != 0)
            _binaryTypeId = (UInt16)FileType.Driver;
    }
    private void FindResidentNames(BinaryReader reader)
    {
        reader.BaseStream.Seek(
            Lva.Offset(LeHeaderDump.Segmentation.ResidentNamesTableOffset), 
            SeekOrigin.Begin);

        while (true)
        {
            Byte len = reader.ReadByte();
            if (len == 0) 
                break;
            
            Byte[] nameBytes = reader.ReadBytes(len);
            UInt16 ordinal = reader.ReadUInt16();
            
            ResidentTable[ordinal] = Encoding.ASCII.GetString(nameBytes);
        }
    }
    private void FindEntries(BinaryReader reader)
    {
        ExportsDump.Address = (UInt64)reader.BaseStream.Position;
        ExportsDump.Name = "LE ObjectEntries (JellyBins: IMAGE_OBJ_ENTRIES)";
        
        reader.BaseStream.Seek(Lva.Offset(LeHeaderDump.Segmentation.EntryTableOffset), SeekOrigin.Begin);
        
        Byte count;
        while ((count = reader.ReadByte()) != 0)
        {
            // count of objects in bungle
            Byte type = reader.ReadByte();
            // UInt16 index = reader.ReadUInt16();
            switch (type)
            {
                case (Byte)LeEntryType.Entry16Bit:
                {
                    WriteEntriesInBungle(count, EntriesDumps.Entries16Bit.Segmentation!, reader);
                    break;
                }
                case (Byte)LeEntryType.Entry32Bit:
                {
                    WriteEntriesInBungle(count, EntriesDumps.Entries32Bit.Segmentation!, reader);
                    break;
                }
                case (Byte)LeEntryType.Entry286CallGate:
                {
                    WriteEntriesInBungle(count, EntriesDumps.EntriesCallGate.Segmentation!, reader);
                    break;
                }
                case (Byte)LeEntryType.EntryForwarder:
                {
                    WriteEntriesInBungle(count, EntriesDumps.EntriesForwarder.Segmentation!, reader);
                    break;
                }
            }
        }
    }
    private void WriteEntriesInBungle<T>(Byte count, List<T> to, BinaryReader reader) where T : struct
    {
        for (Byte i = 0; i < count; ++i)
        {
            if (reader.ReadByte() != 0)
                to.Add(Fill<T>(reader));
        }
        
    }
    private void FindImports(BinaryReader reader)
    {
        ImportsDump.Name = "LE Imports (JellyBins: IMAGE_IMPORT_ENTRIES)";
        ImportsDump.Address = (UInt64)reader.BaseStream.Position;

        ImportModulesTable = ReadImportModuleNames(reader).ToArray();
        //ImportProceduresTable = ReadImportProcedureNames(reader).ToArray();
    }
    private String[] FindNames(BinaryReader reader, Int64 offset)
    {
        List<String> names = [];
        reader.BaseStream.Seek(offset, SeekOrigin.Begin);
        
        while (true)
        {
            Byte len = reader.ReadByte();
            if (len == 0) 
                break;
            
            Byte[] bytes = reader.ReadBytes(len);
            names.Add(Encoding.ASCII.GetString(bytes));
        }
        
        return names.ToArray();
    }
    private void FindNonResidentNames(BinaryReader reader)
    {
        ExportsDump.Segmentation = new();

        if (LeHeaderDump.Segmentation.NonResidentNamesTableLength == 0 ||
            LeHeaderDump.Segmentation.NonResidentNamesTableOffsetFromTopOfFile == 0)
        {
            // no names.
            return;
        }
        
        reader.BaseStream.Seek(LeHeaderDump.Segmentation.NonResidentNamesTableOffsetFromTopOfFile,
            SeekOrigin.Begin);

        Byte count;
        while ((count = reader.ReadByte()) != 0)
        {
            String name = Encoding.ASCII.GetString(reader.ReadBytes(count));
            UInt16 ordinal = reader.ReadUInt16();
            
            ExportsDump.Segmentation!.Add(new LeExportEntry()
            {
                Name = name,
                Ordinal = ordinal
            });
        }

        Info.ProjectDescription = ExportsDump.Segmentation[0].Name;
    }
    private void FindIterateObjectEntries(BinaryReader reader)
    {
        reader.BaseStream.Seek(Lva.Offset(LeHeaderDump.Segmentation.ObjectTableOffset), SeekOrigin.Begin);
        List<LeObjectDump> dumps = [];
        
        for (Int32 i = 0; i < LeHeaderDump.Segmentation.ObjectTableEntries; i++)
        {
            List<String> chars = [];
            LeObjectDump dump = new()
            {
                Name = "LE Entry (JellyBins: IMAGE_SEC)",
                Address = (UInt64)reader.BaseStream.Position,
                Segmentation = Fill<LeObjectHeader>(reader)
            };
            if ((dump.Segmentation.ObjectFlags & (UInt16)LeObjectType.Readable) != 0)
                chars.Add("SEC_READABLE_PAGE");
            if ((dump.Segmentation.ObjectFlags & (UInt16)LeObjectType.Executable) != 0)
                chars.Add("SEC_EXECUTABLE_PAGE");
            if ((dump.Segmentation.ObjectFlags & (UInt16)LeObjectType.Writable) != 0)
                chars.Add("SEC_WRITABLE_PAGE");
            if ((dump.Segmentation.ObjectFlags & (UInt16)LeObjectType.Shared) != 0)
                chars.Add("SEC_SHARED_PAGE");
            if ((dump.Segmentation.ObjectFlags & (UInt16)LeObjectType.Resource) != 0)
                chars.Add("SEC_RESOURCE_PAGE");
            if ((dump.Segmentation.ObjectFlags & (UInt16)LeObjectType.Resident) != 0)
                chars.Add("SEC_DATASYS_PAGE");
            if ((dump.Segmentation.ObjectFlags & (UInt16)LeObjectType.DiscardPriority) != 0)
                chars.Add("SEC_MEMFREE_PAGE");
            if ((dump.Segmentation.ObjectFlags & (UInt16)LeObjectType.IaConforming) != 0)
                chars.Add("SEC_X86CONFORMING_PAGE");
            if ((dump.Segmentation.ObjectFlags & (UInt16)LeObjectType.PreloadPages) != 0)
                chars.Add("SEC_DATAPRELOAD_PAGE");
            if ((dump.Segmentation.ObjectFlags & (UInt16)LeObjectType.ZeroPages) != 0)
                chars.Add("SEC_ZERO_FILLED");
            if ((dump.Segmentation.ObjectFlags & (UInt16)LeObjectType.IoPortsPriority) != 0)
                chars.Add("SEC_IODEVICES_ACCESS");
            if ((dump.Segmentation.ObjectFlags & (UInt16)LeObjectType.RequiredAlias16X16) != 0)
                chars.Add("SEC_X86_ALIAS_REQUIRED");
            if ((dump.Segmentation.ObjectFlags & (UInt16)LeObjectType.BigOrDefaultBitSetting) != 0)
                chars.Add("SEC_X86_BIGDEFAULT_BITSET");
            
            // common characteristics check (needs to make suggested name)
            // those times section names were just calling conventions between
            // linker and compiler, so after linking this information was only in .debug
            // records (may be). 
            //
            // Object Section names have taken from those times IBM toolchain
            // (not .text .data... from Unix convention) 
            if ((dump.Segmentation.ObjectFlags & (UInt16)LeObjectType.Executable) != 0)
                dump.SuggestedObjectName = "CODE32";
            else if ((dump.Segmentation.ObjectFlags & (UInt16)LeObjectType.Writable) != 0)
                dump.SuggestedObjectName = "DATA32";
            else if ((dump.Segmentation.ObjectFlags & (UInt16)LeObjectType.ZeroPages) != 0)
                dump.SuggestedObjectName = "BSS32";
            
            dump.Size = SizeOf(dump.Segmentation);
            dump.Characteristics = chars.ToArray();
            dumps.Add(dump);
        }
        
        ObjectsTableDump = dumps.ToArray();
    }
    private void FindIterateDirectives(BinaryReader reader)
    {
        reader.BaseStream.Seek(Lva.Offset(LeHeaderDump.Segmentation.ModuleDirectivesTableOffset), SeekOrigin.Begin);
        
        List<LeDirectiveDump> entries = new();
        for (Int32 i = 0; i < LeHeaderDump.Segmentation.ModuleDirectivesTableEntries; i++)
        {
            LeDirective dir = Fill<LeDirective>(reader);
            LeDirectiveDump entry = new()
            {
                Name = "LE Module Directive (JellyBins: IMAGE_MODDIR)",
                Address = (UInt64)reader.BaseStream.Position,
                Size = SizeOf(dir),
                Segmentation = dir
            };

            List<String> chars =
            [
                (entry.Segmentation.DirectiveNumber & 0x8000) != 0 
                    ? "MODDIR_DATA_RESIDENT" 
                    : "MODDIR_DATA_NONRESIDENT",
                (entry.Segmentation.DirectiveNumber & 0x8000) != 0 
                    ? "MODDR_DATA_FROM_LXHDR_OFFSET"
                    : "MODDIR_DATA_FROM_FILETOP_OFFSET",
            ];
            if ((entry.Segmentation.DirectiveNumber & 0x0001) != 0)
                chars.Add("MODDIR_VERIFY_REC");
            if ((entry.Segmentation.DirectiveNumber & 0x0002) != 0)
                chars.Add("MODDIR_CULTURE_REC");
            if ((entry.Segmentation.DirectiveNumber & 0x0004) != 0)
                chars.Add("MODDIR_THREADSTATE_INIT_REC");
            if ((entry.Segmentation.DirectiveNumber & 0x0005) != 0)
                chars.Add("MODDIR_BROWSE_INFO_REC");
            
            entry.Characteristics = chars.ToArray();
            entries.Add(entry);
        }

        DirectiveTableDumps = entries.ToArray();
    }
    private List<String> ReadImportModuleNames(BinaryReader reader)
    {
        List<String> imports = [];
        for (Int32 i = 0; i < LeHeaderDump.Segmentation.ImportedModulesCount; i++)
        {
            Byte len = reader.ReadByte();
            reader.ReadBytes(len);
            // garbage
            String[] modNames = FindNames(reader, Lva.Offset(LeHeaderDump.Segmentation.ImportedModulesNameTableOffset));
            
            imports.Add(modNames[i]);
        }

        return imports;
    }
    private List<String> ReadImportProcedureNames(BinaryReader reader)
    {
        reader.BaseStream.Seek(Lva.Offset(LeHeaderDump.Segmentation.ImportedProcedureNameTableOffset), SeekOrigin.Begin);
        UInt32 importedProcedureNamesTableEnd =
            LeHeaderDump.Segmentation.FixupPageTableOffset +
            LeHeaderDump.Segmentation.FixupSectionSize - 
            LeHeaderDump.Segmentation.ImportedProcedureNameTableOffset;
        
        List<String> procs = [];
        
        for (Int32 i = 0; i < importedProcedureNamesTableEnd; ++i)
        {
            String[] names = FindNames(reader, Lva.Offset(LeHeaderDump.Segmentation.ImportedProcedureNameTableOffset));
            procs.Add(names[i]);
        }

        return procs;
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
    #endregion

    public UInt16 GetExtensionTypeId()
    {
        return _extensionTypeId;
    }

    public UInt16 GetBinaryTypeId()
    {
        return _binaryTypeId;
    }
}