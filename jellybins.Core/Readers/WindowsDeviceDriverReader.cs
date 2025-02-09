using jellybins.Core.Headers;
using jellybins.Core.Models;
using jellybins.Core.Interfaces;
using jellybins.Core.Sections;
using jellybins.Core.Strings;

namespace jellybins.Core.Readers;

public class WindowsDeviceDriverReader : IReader
{
    private readonly Data _imports;
    private readonly LinearExecutable _head;
    private readonly WindowsDeviceDriverStrings _stringsService;

    private CommonProperties _result;
    
    public WindowsDeviceDriverReader(LinearExecutable head)
    {
        _stringsService = new WindowsDeviceDriverStrings();
        _head = head;
    }

    public WindowsDeviceDriverReader(Data imports)
    {
        _stringsService = new WindowsDeviceDriverStrings();
        _imports = imports;
    }

    public Dictionary<string, string> GetHeader()
    {
        return new Dictionary<string, string>
        {
            { nameof(_head.WindowsVXDDeviceID), $"0x{_head.WindowsVXDDeviceID:X}" },
            { nameof(_head.WindowsDDKVersionMajor), $"0x{_head.WindowsDDKVersionMajor:X}" },
            { nameof(_head.WindowsDDKVersionMinor), $"0x{_head.WindowsDDKVersionMinor:X}" },
            { nameof(_head.WindowsVXDVersionInfoResourceOffset), $"0x{_head.WindowsVXDVersionInfoResourceOffset:X}" },
            { nameof(_head.WindowsVXDVersionInfoResourceLength), $"0x{_head.WindowsVXDVersionInfoResourceLength:X}" }
        };
    }

    public Dictionary<string, string[]> GetFlags()
    {
        var iterates = from item in new[]
                {
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
                    (_head.ModuleTypeFlags & 0x00080000) != 0 ? "MultiCpuUnsafe" : "",
                    "WindowsDeviceDriver"
                }
                .Where(i => !string.IsNullOrEmpty(i))
                .Distinct()
                .ToList()
            select item;
        
        return new Dictionary<string, string[]>(){{"_module", iterates.ToArray()}};
    }

    public CommonProperties GetProperties()
    {
        // path, name
        _result.CpuArchitecture = _stringsService.CpuArchitectureFlagToString(_head.CPUType);
        _result.CpuWordLength = _stringsService.CpuWordLengthFlagToString(_head.WordOrder);
        _result.OperatingSystem = _stringsService.OperatingSystemFlagToString(_head.TargetOperatingSystem);
        _result.OperatingSystemVersion = _stringsService.OperatingSystemVersionToString(_head.WindowsDDKVersionMinor, _head.WindowsDDKVersionMajor);
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