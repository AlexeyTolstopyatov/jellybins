using System.Runtime.InteropServices;
using System.Text;
using JellyBins.Abstractions;
using JellyBins.LinearExecutable.Headers;
using JellyBins.LinearExecutable.Private;
using JellyBins.LinearExecutable.Private.Types;

namespace JellyBins.LinearExecutable.Models;

public class LeFileDumper(String path) : IFileDumper
{
    public Dictionary<UInt16, String> ResidentTable { get; private set; } = [];
    public String[] ImportedModules { get; private set; } = [];
    public String[] ImportedProcedures { get; private set; } = [];
    public LeFileInfo Info { get; private set; } = new()
    {
        Name = new FileInfo(path).Name,
        Path = path,
        CpuWordLength = 16,
    };
    public MzHeaderDump MzHeaderDump { get; private set; } = new();
    public LeHeaderDump LeHeaderDump { get; private set; } = new();
    public LeObjectDump[] LeObjectsDump { get; private set; } = [];
    public LeDirectiveDump[] LeDirectiveDumps { get; private set; } = [];
    public LeExportsDump LeExportsDump { get; private set; } = new();
    public LeImportsDump LeImportsDump { get; private set; } = new();
    private LeVirtualAddress Lva { get; set; } = new(0);

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
        LeHeaderDump.Name = "LX Header (OS/2 API: LX_HEADER)";

        Lva = new LeVirtualAddress((UInt64)LeHeaderDump.Address);
        
        // other details iteration (sources: os2.h)
        FindIterateObjectEntries(reader);
        FindIterateDirectives(reader);
        FindImports(reader);
        FindExports(reader);
        FindResidentObjects(reader);
        
        reader.Close();
    }

    #region Private methods
    
    private void FindResidentObjects(BinaryReader reader)
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

    private void FindImports(BinaryReader reader)
    {
        List<LeImportEntry> imports = [];
        
        Int64 fixupRecordTableOffset = 
            Lva.Offset(LeHeaderDump.Segmentation.FixupPageTableOffset);
        
        List<String> moduleNames = ReadImportModuleNames(reader);
        List<String> procNames = ReadImportProcedureNames(reader);
        ImportedModules = moduleNames.ToArray();
        ImportedProcedures = procNames.ToArray();
        
        Int64 fixupPageTableOffset = Lva.Offset(LeHeaderDump.Segmentation.FixupPageTableOffset);
        
        reader.BaseStream.Seek(fixupPageTableOffset, SeekOrigin.Begin);
        LeImportsDump.Name = "LX Imports (JellyBins: IMAGE_IMPORT_ENTRIES)";
        LeImportsDump.Address = (UInt64)reader.BaseStream.Position;
        
        List<UInt32> fixupPageOffsets = [];
        while (true)
        {
            UInt32 offset = reader.ReadUInt32();
            if (offset == 0) 
                break;
            fixupPageOffsets.Add(offset);
        }
        
        // incorrect offset. May be prev time was the move +1 byte
        foreach (UInt32 offset in fixupPageOffsets)
        {
            reader.BaseStream.Seek(fixupRecordTableOffset + offset, SeekOrigin.Begin);
            
            // Чтение Fixup Record (пока что упрощенно, реальные записи я не понимаю)
            Byte srcType = reader.ReadByte(); // Это пиздец а не характеристики. Надо тоже учитывать
            Byte flags = reader.ReadByte(); // Это характеристики

            // Проверка типа импорта (0x01 = по ординалу)
            Boolean isByOrdinal = (flags & 0x01) != 0;
            UInt16 moduleOrdinal = reader.ReadUInt16();
            
            if (isByOrdinal)
            {
                UInt32 procOrdinal = reader.ReadUInt32();
                imports.Add(new LeImportEntry
                {
                    ModuleName = moduleNames[moduleOrdinal - 1],
                    Ordinal = procOrdinal,
                    IsByOrdinal = true
                });
            }
            else
            {
                UInt32 procNameOffset = reader.ReadUInt32();
                String procName = procNames[(Int32)procNameOffset];
                
                imports.Add(new LeImportEntry
                {
                    ModuleName = moduleNames[moduleOrdinal - 1],
                    ProcedureName = procName,
                    IsByOrdinal = false
                });
            }
        }

        LeImportsDump.Segmentation = imports;
    }
    private String[] FindNames(BinaryReader reader)
    {
        List<String> names = [];
        reader.BaseStream.Seek(Lva.Offset(LeHeaderDump.Segmentation.ImportedModulesNameTableOffset), SeekOrigin.Begin);
        
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
    private void FindExports(BinaryReader reader)
    {
        List<LeExportEntry> exports = [];
        LeExportsDump.Address = (UInt64)reader.BaseStream.Position;
        LeExportsDump.Name = "LX Exports (JellyBins: IMAGE_EXPORT_ENTRIES)";
        
        reader.BaseStream.Seek(Lva.Offset(LeHeaderDump.Segmentation.EntryTableOffset), SeekOrigin.Begin);
        Byte bundleType;
        
        while ((bundleType = reader.ReadByte()) != 0)
        {
            Byte count = reader.ReadByte();
            // Пример для 32-битных записей (тип 0x03)
            // if bundleType & 0x03 != 0
            for (Int32 i = 0; i < count; i++)
            {
                UInt16 ordinal = reader.ReadUInt16();
                UInt32 offset = reader.ReadUInt32();
                
                exports.Add(new LeExportEntry
                {
                    Name = ResidentTable.GetValueOrDefault(ordinal),
                    Ordinal = ordinal,
                    Offset = offset
                });
            }
        }
        LeExportsDump.Segmentation = exports;
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
                Name = "LX Entry (JellyBins: IMAGE_SEC)",
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
        
        LeObjectsDump = dumps.ToArray();
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
                Name = "LX Module Directive (JellyBins: IMAGE_MODDIR)",
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

        LeDirectiveDumps = entries.ToArray();
    }
    
    /// <summary>
    /// Reads and fills Imported library modules collection
    /// </summary>
    /// <param name="reader"><see cref="BinaryReader"/> instance</param>
    private List<String> ReadImportModuleNames(BinaryReader reader)
    {
        List<String> imports = [];
        for (Int32 i = 0; i < LeHeaderDump.Segmentation.ImportedModulesCount; i++)
        {
            Byte len = reader.ReadByte();
            reader.ReadBytes(len);
            // garbage
            String[] modNames = FindNames(reader);
            
            imports.Add(modNames[i]);
        }

        return imports;
    }

    private List<String> ReadImportProcedureNames(BinaryReader reader)
    {
        List<String> imports = [];
        Int64 procedureTableStart = Lva.Offset(LeHeaderDump.Segmentation.ImportedProcedureNameTableOffset);

        Int64 procedureTableLength = Math.Abs(Lva.Offset(LeHeaderDump.Segmentation.FixupPageTableOffset) 
                                     - Lva.Offset(LeHeaderDump.Segmentation.ImportedProcedureNameTableOffset));

        reader.BaseStream.Seek(procedureTableStart, SeekOrigin.Begin);
    
        Byte[] procedureTableBytes = reader.ReadBytes((Int32)procedureTableLength); // ArgumentException
        
        Int32 pos = 0;
        while (pos < procedureTableBytes.Length)
        {
            Byte len = procedureTableBytes[pos];
            if (len == 0) break;
        
            String name = Encoding.ASCII.GetString(procedureTableBytes, pos + 1, len);
            imports.Add(name);
            pos += len + 1;
        }

        return imports;
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
        throw new NotImplementedException();
    }

    public UInt16 GetBinaryTypeId()
    {
        throw new NotImplementedException();
    }
}