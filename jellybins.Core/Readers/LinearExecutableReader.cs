using jellybins.Core.Attributes;
using jellybins.Core.Headers;
using jellybins.Core.Models;
using jellybins.Core.Models.Flags;
using jellybins.Core.Interfaces;
using jellybins.Core.Sections;
using jellybins.Core.Strings;
using jellybins.Core.Utilities;

namespace jellybins.Core.Readers;

public class LinearExecutableReader : IReader
{
    private readonly Data _imports;
    private readonly LinearExecutable _head;
    private readonly LinearExecutableStrings _stringsService;

    private CommonProperties _result;

    public CommonProperties Properties => _result;
    
    public LinearExecutableReader(LinearExecutable head)
    {
        _stringsService = new LinearExecutableStrings();
        _head = head;
    }

    public LinearExecutableReader(Data imports)
    {
        _stringsService = new LinearExecutableStrings();
        _imports = imports;
    }

    public Dictionary<string, string> GetHeader()
    {
        return new Dictionary<string, string>()
        {
            { nameof(_head.SignatureWord), $"0x{_head.SignatureWord:X}" },
            { nameof(_head.ByteOrder), $"0x{_head.ByteOrder:X}" },
            { nameof(_head.WordOrder), $"0x{_head.ExecutableFormatLevel:X}" },
            { nameof(_head.CPUType), $"0x{_head.CPUType:X}" },
            { nameof(_head.TargetOperatingSystem), $"0x{_head.TargetOperatingSystem:X}" },
            { nameof(_head.ModuleVersionMajor), $"0x{_head.ModuleVersionMajor:X}" },
            { nameof(_head.ModuleVersionMinor), $"0x{_head.ModuleVersionMinor:X}" },
            { nameof(_head.ModuleTypeFlags), $"0x{_head.ModuleTypeFlags:X}" },
            { nameof(_head.NumberOfMemoryPages), $"0x{_head.NumberOfMemoryPages:X}" },
            { nameof(_head.InitialObjectCSNumber), $"0x{_head.InitialObjectCSNumber:X}" },
            { nameof(_head.InitialEIP), $"0x{_head.InitialEIP:X}" },
            { nameof(_head.InitialSSObjectNumber), $"0x{_head.InitialSSObjectNumber:X}" },
            { nameof(_head.InitialESP), $"0x{_head.InitialESP:X}" },
            { nameof(_head.MemoryPageSize), $"0x{_head.MemoryPageSize:X}" },
            { nameof(_head.BytesOnLastPage), $"0x{_head.BytesOnLastPage:X}" },
            { nameof(_head.FixupSectionSize), $"0x{_head.FixupSectionSize:X}" },
            { nameof(_head.FixupSectionChecksum), $"0x{_head.FixupSectionChecksum:X}" },
            { nameof(_head.LoaderSectionSize), $"0x{_head.LoaderSectionSize:X}" },
            { nameof(_head.LoaderSectionChecksum), $"0x{_head.LoaderSectionSize:X}" },
            { nameof(_head.ObjectTableOffset), $"0x{_head.ObjectTableOffset:X}" },
            { nameof(_head.ObjectTableEntries), $"0x{_head.ObjectTableEntries:X}" },
            { nameof(_head.ObjectPageMapOffset), $"0x{_head.ObjectPageMapOffset:X}" },
            { nameof(_head.ObjectIterateDataMapOffset), $"0x{_head.ObjectIterateDataMapOffset:X}" },
            { nameof(_head.ResourceTableOffset), $"0x{_head.ResourceTableOffset:X}" },
            { nameof(_head.ResourceTableEntries), $"0x{_head.ResourceTableEntries:X}" },
            { nameof(_head.ResidentNamesTableOffset), $"0x{_head.ResidentNamesTableOffset:X}" },
            { nameof(_head.EntryTableOffset), $"0x{_head.EntryTableOffset:X}" },
            { nameof(_head.ModuleDirectivesTableOffset), $"0x{_head.ModuleDirectivesTableOffset:X}" },
            { nameof(_head.ModuleDirectivesTableEntries), $"0x{_head.ModuleDirectivesTableEntries:x}" },
            { nameof(_head.FixupPageTableOffset), $"0x{_head.FixupPageTableOffset:X}" },
            { nameof(_head.FixupRecordTableOffset), $"0x{_head.FixupRecordTableOffset:X}" },
            { nameof(_head.ImportedModulesNameTableOffset), $"0x{_head.ImportedModulesNameTableOffset:X}" },
            { nameof(_head.ImportedModulesCount), $"0x{_head.ImportedModulesCount:X}" },
            { nameof(_head.ImportedProcedureNameTableOffset), $"0x{_head.ImportedProcedureNameTableOffset:X}" },
            { nameof(_head.PerPageChecksumTableOffset), $"0x{_head.PerPageChecksumTableOffset:X}" },
            { nameof(_head.DataPagesOffsetFromTopOfFile), $"0x{_head.DataPagesOffsetFromTopOfFile:X}" },
            { nameof(_head.PreloadPagesCount), $"0x{_head.PreloadPagesCount:X}" }, { nameof(_head.NonResidentNamesTableOffsetFromTopOfFile), $"0x{_head.NonResidentNamesTableOffsetFromTopOfFile:X}" },
            { nameof(_head.NonResidentNamesTableLength), $"0x{_head.NonResidentNamesTableLength:X}" },
            { nameof(_head.NonResidentNamesTableChecksum), $"0x{_head.NonResidentNamesTableChecksum:X}" },
            { nameof(_head.AutomaticDataObject), $"0x{_head.AutomaticDataObject:X}" },
            { nameof(_head.DebugInformationOffset), $"0x{_head.DebugInformationOffset:X}" },
            { nameof(_head.DebugInformationLength), $"0x{_head.DebugInformationLength:X}" },
            { nameof(_head.PreloadInstancePagesNumber), $"0x{_head.PreloadInstancePagesNumber:X}" },
            { nameof(_head.DemandInstancePagesNumber), $"0x{_head.DemandInstancePagesNumber:X}" },
            { nameof(_head.HeapSize), $"0x{_head.HeapSize:X}" },
            { nameof(_head.StackSize), $"0x{_head.StackSize:X}" },
        };
    }

    public Dictionary<string, string[]> GetFlags()
    {
        var iterates = from item in new[] {
            (_head.ModuleTypeFlags & 0x00000000) != 0 ? "Executable" : "",
            (_head.ModuleTypeFlags & 0x00008000) != 0 ? "Library" : "",
            (_head.ModuleTypeFlags & 0x00000004) != 0 ? "PerProcessInit" : "StandardInit",
            (_head.ModuleTypeFlags & 0x00000010) != 0 ? "InternalFixes" : "",
            (_head.ModuleTypeFlags & 0x00000020) != 0 ? "ExternalFixes" : "",
            (_head.ModuleTypeFlags & 0x00000100) != 0 ? "OS/2 PM Incompatible" : "",
            (_head.ModuleTypeFlags & 0x00000200) != 0 ? "OS/2 PM Compatible" : "",
            (_head.ModuleTypeFlags & 0x00000300) != 0 ? "OS/2 PM Needed" : "",
            (_head.ModuleTypeFlags & 0x00002000) != 0 ? "NotLoadable" : "Loadable",
            (_head.ModuleTypeFlags & 0x00020000) != 0 ? "PhysicalDriver" : "",
            (_head.ModuleTypeFlags & 0x00028000) != 0 ? "VirtualDriver" : "",
            (_head.ModuleTypeFlags & 0x00040000) != 0 ? "PerProcessTerm" : "",
            (_head.ModuleTypeFlags & 0x00080000) != 0 ? "MultiCpuUnsafe" : ""
        }
            .Where(i => !string.IsNullOrEmpty(i))
            .Distinct()
            .ToList()
            select item;
        var pair = new Dictionary<string, string[]> { { "_module", iterates.ToArray() } };
        return pair;
    }

    [UnderConstruction("Image's version not shows")]
    public CommonProperties GetProperties()
    {
        // path, name
        _result.CpuArchitecture = _stringsService.CpuArchitectureFlagToString(_head.CPUType);
        _result.CpuWordLength = _stringsService.CpuWordLengthFlagToString(_head.WordOrder);
        _result.OperatingSystem = _stringsService.OperatingSystemFlagToString(_head.TargetOperatingSystem);
        _result.OperatingSystemVersion = _stringsService.OperatingSystemVersionToString(_head.TargetOperatingSystem, 0);
        _result.Subsystem = _stringsService.SubsystemFromOperatingSystemFlagToString(_head.TargetOperatingSystem);
        _result.ImageType = _stringsService.ImageTypeFlagToString(_head.ModuleTypeFlags);
        _result.LinkerVersion = _stringsService.ImageVersionFlagsToString(_head.ModuleVersionMajor, _head.ModuleVersionMinor);
        return _result;
    }

    public void GetImports()
    {
        throw new NotImplementedException();
    }
}