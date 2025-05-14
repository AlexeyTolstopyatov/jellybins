using System.Runtime.InteropServices;
using JellyBins.Abstractions;
using JellyBins.LinearExecutable.Headers;
using JellyBins.LinearExecutable.Private.Types;

namespace JellyBins.LinearExecutable.Models;

public class LeFileDumper : IFileDumper
{
    public LeFileDumper(String path)
    {
        Info = new()
        {
            Name = new FileInfo(path).Name,
            Path = path,
            CpuWordLength = 16,
        };
        MzHeaderDump = new();
        LeHeaderDump = new();
        LeExportsDump = new();
        LeImportsDump = new();
        LeObjectsDump = [];
        LeDirectiveDumps = [];
        ResidentTable = [];
    }

    public Dictionary<UInt16, String> ResidentTable { get; private set; }
    public LeFileInfo Info { get; private set; }
    public MzHeaderDump? MzHeaderDump { get; private set; }
    public LeHeaderDump? LeHeaderDump { get; private set; }
    public LeObjectDump[] LeObjectsDump { get; private set; }
    public LeDirectiveDump[] LeDirectiveDumps { get; private set; }
    public LeExportsDump LeExportsDump { get; private set; }
    public LeImportsDump LeImportsDump { get; private set; }

    public void Dump()
    {
        using FileStream stream = new(Info.Path!, FileMode.Open, FileAccess.Read);
        using BinaryReader reader = new(stream);
        
        // IMAGE_DOS_HEADER construct
        MzHeaderDump!.Segmentation = Fill<MzHeader>(reader);
        MzHeaderDump.Address = 0;
        MzHeaderDump.Size = SizeOf(MzHeaderDump.Segmentation);
        MzHeaderDump.Name = "MZ Header (WinAPI: IMAGE_DOS_HEADER)";

        // LX_HEADER construct (sources: os2.h emm386.h vmm386.h)
        stream.Seek(MzHeaderDump.Segmentation.e_lfanew, SeekOrigin.Begin);
        LeHeaderDump!.Segmentation = Fill<LeHeader>(reader);
        LeHeaderDump.Address = MzHeaderDump.Segmentation.e_lfanew;
        LeHeaderDump.Size = SizeOf(LeHeaderDump.Segmentation);
        LeHeaderDump.Name = "LX Header (OS/2 API: LX_HEADER)";

        // other details iteration (sources: os2.h)
        FindIterateObjectEntries(reader);
        FindIterateDirectives(reader);
        FindResidentObjects(reader);
        FindImports(reader);
        FindExports(reader);
        
        reader.Close();
    }

    private void FindResidentObjects(BinaryReader reader)
    {
        reader.BaseStream.Seek((Int64)(LeHeaderDump.Segmentation.ResidentNamesTableOffset + LeHeaderDump.Address), SeekOrigin.Begin);

        while (true)
        {
            Byte len = reader.ReadByte();
            if (len == 0) 
                break;
            
            Byte[] nameBytes = reader.ReadBytes(len);
            UInt16 ordinal = reader.ReadUInt16();
            ResidentTable[ordinal] = System.Text.Encoding.ASCII.GetString(nameBytes);
        }
    }

    private void FindImports(BinaryReader reader)
    {
        reader.BaseStream.Seek((Int64)(LeHeaderDump.Address + LeHeaderDump.Segmentation.ImportedProcedureNameTableOffset), SeekOrigin.Begin);

        List<LeImportEntry> imports = [];
        LeImportsDump.Name = "LX Imports (JellyBins: IMAGE_IMPORT_ENTRIES)";
        LeImportsDump.Address = (UInt64)reader.BaseStream.Position;
        
        for (Int32 i = 0; i < LeHeaderDump.Segmentation.ImportedModulesCount; i++)
        {
            Byte len = reader.ReadByte();
            Byte[] nameBytes = reader.ReadBytes(len);
            String[] modNames = FindNames(reader);
            String procName = System.Text.Encoding.ASCII.GetString(nameBytes);
            
            imports.Add(new LeImportEntry
            {
                ModuleName = modNames[i],
                ProcedureName = procName,
                IsByOrdinal = false // Упрощенно !!!!
            });
        }

        LeImportsDump.Segmentation = imports;
    }

    private String[] FindNames(BinaryReader reader)
    {
        List<String> names = [];
        reader.BaseStream.Seek((Int64)(LeHeaderDump.Segmentation.ImportedModulesNameTableOffset + LeHeaderDump.Address), SeekOrigin.Begin);
        
        while (true)
        {
            Byte len = reader.ReadByte();
            if (len == 0) 
                break;
            
            Byte[] bytes = reader.ReadBytes(len);
            names.Add(System.Text.Encoding.ASCII.GetString(bytes));
        }
        
        return names.ToArray();
    }
    private void FindExports(BinaryReader reader)
    {
        List<LeExportEntry> exports = [];
        LeExportsDump.Address = (UInt64)reader.BaseStream.Position;
        LeExportsDump.Name = "LX Exports (JellyBins: IMAGE_EXPORT_ENTRIES)";
        
        // Чтение Entry Table
        reader.BaseStream.Seek((Int64)(LeHeaderDump.Address + LeHeaderDump.Segmentation.EntryTableOffset), SeekOrigin.Begin);
        Byte bundleType;
        
        while ((bundleType = reader.ReadByte()) != 0)
        {
            Byte count = reader.ReadByte();
            // Пример для 32-битных записей (тип 0x03)
            if (bundleType == 0x03)
            {
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
        }
        LeExportsDump.Segmentation = exports;
    }
    private void FindIterateObjectEntries(BinaryReader reader)
    {
        reader.BaseStream.Seek((Int64)(LeHeaderDump!.Segmentation.ObjectPageMapOffset + LeHeaderDump.Address)!, SeekOrigin.Begin);
        List<LeObjectDump> dumps = [];
        // Важный момент: Или ObjectPages или ObjectTables блять.
        // Учитывая то, что и от начала файла сдвиг давал нормальные результаты.
        // Чёрт только и знает что является чем и почему. Отладчиков для OS/2 я не нашел
        for (Int32 i = 0; i < LeHeaderDump.Segmentation.ObjectTableEntries; i++)
        {
            List<String> chars = [];
            LeObjectDump dump = new()
            {
                Name = "LX Entry (JellyBins: IMAGE_OBJECT_ENTRY)",
                Address = (UInt64)reader.BaseStream.Position,
                Segmentation = Fill<LeObjectHeader>(reader)
            };
            if ((dump.Segmentation.ObjectFlags & (UInt16)LeObjectType.Readable) != 0)
            {
                chars.Add("SEC_READABLE_PAGE");
                if ((dump.Segmentation.ObjectFlags & (UInt16)LeObjectType.Executable) != 0)
                    dump.SuggestedObjectName = "CODE32";
                else if ((dump.Segmentation.ObjectFlags & (UInt16)LeObjectType.Writable) != 0)
                    dump.SuggestedObjectName = "DATA32";
                else if ((dump.Segmentation.ObjectFlags & (UInt16)LeObjectType.ZeroPages) != 0)
                    dump.SuggestedObjectName = "BSS32";
            }
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
                chars.Add("SEC_BIGDEFAULT_BITSET");
            
            dump.Size = SizeOf(dump.Segmentation);
            dump.Characteristics = chars.ToArray();
            dumps.Add(dump);
        }
        
        LeObjectsDump = dumps.ToArray();
    }
    private void FindIterateDirectives(BinaryReader reader)
    {
        reader.BaseStream.Seek((Int64)(LeHeaderDump!.Address + LeHeaderDump!.Segmentation.ModuleDirectivesTableOffset)!, SeekOrigin.Begin);
        
        List<LeDirectiveDump> entries = new();
        //reader.BaseStream.Seek(directivesOffset, SeekOrigin.Begin);

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
        throw new NotImplementedException();
    }

    public UInt16 GetBinaryTypeId()
    {
        throw new NotImplementedException();
    }
}