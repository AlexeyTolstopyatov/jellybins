using System.Reflection;
using jellybins.Core.Attributes;
using jellybins.Core.Models;
using jellybins.Core.Interfaces;


namespace jellybins.Core.Readers.PortableExecutable;

public class PortableExecutableReader : IReader
{
    private readonly PortableExecutable32 _head32;
    private readonly PortableExecutable64 _head64;
    private readonly PortableExecutableStrings _strings;
    private readonly string _fileName;
    
    public PortableExecutableReader(PortableExecutable32 head32, string path)
    {
        _fileName = path;
        _head32 = head32;
        _head64 = new PortableExecutable64();
        _strings = new PortableExecutableStrings();
    }

    public PortableExecutableReader(PortableExecutable64 head64, string path)
    {
        _fileName = path;
        _head32 = new PortableExecutable32();
        _head64 = head64;
        _strings = new PortableExecutableStrings();
    }
    
    public Dictionary<string, string> GetHeader() =>
        _head32.Signature == 0 
            ? GetQWordHeader() 
            : GetDWordHeader();

    private Dictionary<string, string> GetDWordHeader()
    {
        var header = new Dictionary<string, string>
        {
            { nameof(_head32.WinNtMain.Machine), $"0x{_head32.WinNtMain.Machine:X}" },
            { nameof(_head32.WinNtMain.NumberOfSections), $"0x{_head32.WinNtMain.NumberOfSections:X}" },
            { nameof(_head32.WinNtMain.TimeDateStamp), $"0x{_head32.WinNtMain.TimeDateStamp:X}" },
            { nameof(_head32.WinNtMain.PointerToSymbolTable), $"0x{_head32.WinNtMain.PointerToSymbolTable:X}" },
            { nameof(_head32.WinNtMain.NumberOfSymbols), $"0x{_head32.WinNtMain.NumberOfSymbols:X}" },
            { nameof(_head32.WinNtMain.SizeOfOptionalHeader), $"0x{_head32.WinNtMain.SizeOfOptionalHeader:X}" },
            { nameof(_head32.WinNtMain.Characteristics), $"0x{_head32.WinNtMain.Characteristics:X}" },
            { nameof(_head32.WinNtOptional.Magic), $"0x{_head32.WinNtOptional.Magic:X}" },
            { nameof(_head32.WinNtOptional.MajorLinkerVersion), $"0x{_head32.WinNtOptional.MajorLinkerVersion:X}" },
            { nameof(_head32.WinNtOptional.MinorLinkerVersion), $"0x{_head32.WinNtOptional.MinorLinkerVersion:X}" },
            { nameof(_head32.WinNtOptional.SizeOfCode), $"0x{_head32.WinNtOptional.SizeOfCode:X}" },
            { nameof(_head32.WinNtOptional.SizeOfInitializedData), $"0x{_head32.WinNtOptional.SizeOfInitializedData:X}" },
            { nameof(_head32.WinNtOptional.SizeOfUninitializedData), $"0x{_head32.WinNtOptional.SizeOfUninitializedData:X}" },
            { nameof(_head32.WinNtOptional.AddressOfEntryPoint), $"0x{_head32.WinNtOptional.AddressOfEntryPoint:X}" },
            { nameof(_head32.WinNtOptional.BaseOfCode), $"0x{_head32.WinNtOptional.BaseOfCode:X}" },
            { nameof(_head32.WinNtOptional.BaseOfData), $"0x{_head32.WinNtOptional.BaseOfData:X}" },
            { nameof(_head32.WinNtOptional.ImageBase), $"0x{_head32.WinNtOptional.ImageBase:X}" },
            { nameof(_head32.WinNtOptional.SectionAlignment), $"0x{_head32.WinNtOptional.SectionAlignment:X}" },
            { nameof(_head32.WinNtOptional.FileAlignment), $"0x{_head32.WinNtOptional.FileAlignment:X}" },
            { nameof(_head32.WinNtOptional.MajorOperatingSystemVersion), $"0x{_head32.WinNtOptional.MajorOperatingSystemVersion:X}" },
            { nameof(_head32.WinNtOptional.MinorOperatingSystemVersion), $"0x{_head32.WinNtOptional.MinorOperatingSystemVersion:X}" },
            { nameof(_head32.WinNtOptional.MajorImageVersion), $"0x{_head32.WinNtOptional.MajorImageVersion:X}" },
            { nameof(_head32.WinNtOptional.MinorImageVersion), $"0x{_head32.WinNtOptional.MinorImageVersion:X}" },
            { nameof(_head32.WinNtOptional.MajorSubsystemVersion), $"0x{_head32.WinNtOptional.MajorSubsystemVersion:X}" },
            { nameof(_head32.WinNtOptional.MinorSubsystemVersion), $"0x{_head32.WinNtOptional.MinorSubsystemVersion:X}" },
            { nameof(_head32.WinNtOptional.Win32VersionValue), $"0x{_head32.WinNtOptional.Win32VersionValue:X}" },
            { nameof(_head32.WinNtOptional.SizeOfImage), $"0x{_head32.WinNtOptional.SizeOfImage:X}" },
            { nameof(_head32.WinNtOptional.SizeOfHeaders), $"0x{_head32.WinNtOptional.SizeOfHeaders:X}" },
            { nameof(_head32.WinNtOptional.CheckSum), $"0x{_head32.WinNtOptional.CheckSum:X}" },
            { nameof(_head32.WinNtOptional.Subsystem), $"0x{_head32.WinNtOptional.Subsystem:X}" },
            { nameof(_head32.WinNtOptional.DllCharacteristics), $"0x{_head32.WinNtOptional.DllCharacteristics:X}" },
            { nameof(_head32.WinNtOptional.SizeOfStackReserve), $"0x{_head32.WinNtOptional.SizeOfStackReserve:X}" },
            { nameof(_head32.WinNtOptional.SizeOfStackCommit), $"0x{_head32.WinNtOptional.SizeOfStackCommit:X}" },
            { nameof(_head32.WinNtOptional.SizeOfHeapReserve), $"0x{_head32.WinNtOptional.SizeOfHeapReserve:X}" },
            { nameof(_head32.WinNtOptional.SizeOfHeapCommit), $"0x{_head32.WinNtOptional.SizeOfHeapCommit:X}" },
            { nameof(_head32.WinNtOptional.LoaderFlags), $"0x{_head32.WinNtOptional.LoaderFlags:X}" },
            { nameof(_head32.WinNtOptional.NumberOfRvaAndSizes), $"0x{_head32.WinNtOptional.NumberOfRvaAndSizes:X}" }
        };
        return header;
    }

    private Dictionary<string, string> GetQWordHeader()
    {
        var header = new Dictionary<string, string>
        {
            { nameof(_head64.WinNtMain.Machine), $"0x{_head64.WinNtMain.Machine:X}" },
            { nameof(_head64.WinNtMain.NumberOfSections), $"0x{_head64.WinNtMain.NumberOfSections:X}" },
            { nameof(_head64.WinNtMain.TimeDateStamp), $"0x{_head64.WinNtMain.TimeDateStamp:X}" },
            { nameof(_head64.WinNtMain.PointerToSymbolTable), $"0x{_head64.WinNtMain.PointerToSymbolTable:X}" },
            { nameof(_head64.WinNtMain.NumberOfSymbols), $"0x{_head64.WinNtMain.NumberOfSymbols:X}" },
            { nameof(_head64.WinNtMain.SizeOfOptionalHeader), $"0x{_head64.WinNtMain.SizeOfOptionalHeader:X}" },
            { nameof(_head64.WinNtMain.Characteristics), $"0x{_head64.WinNtMain.Characteristics:X}" },
            { nameof(_head64.WinNtOptional.Magic), $"0x{_head64.WinNtOptional.Magic:X}" },
            { nameof(_head64.WinNtOptional.MajorLinkerVersion), $"0x{_head64.WinNtOptional.MajorLinkerVersion:X}" },
            { nameof(_head64.WinNtOptional.MinorLinkerVersion), $"0x{_head64.WinNtOptional.MinorLinkerVersion:X}" },
            { nameof(_head64.WinNtOptional.SizeOfCode), $"0x{_head64.WinNtOptional.SizeOfCode:X}" }, // Размер кода
            { nameof(_head64.WinNtOptional.SizeOfInitializedData), $"0x{_head64.WinNtOptional.SizeOfInitializedData:X}" },
            { nameof(_head64.WinNtOptional.SizeOfUninitializedData), $"0x{_head64.WinNtOptional.SizeOfUninitializedData:X}" },
            { nameof(_head64.WinNtOptional.AddressOfEntryPoint), $"0x{_head64.WinNtOptional.AddressOfEntryPoint:X}" },
            { nameof(_head64.WinNtOptional.BaseOfCode), $"0x{_head64.WinNtOptional.BaseOfCode:X}" },
            { nameof(_head64.WinNtOptional.BaseOfData), $"0x{_head64.WinNtOptional.BaseOfData:X}" },
            { nameof(_head64.WinNtOptional.ImageBase), $"0x{_head64.WinNtOptional.ImageBase:X}" },
            { nameof(_head64.WinNtOptional.SectionAlignment), $"0x{_head64.WinNtOptional.SectionAlignment:X}" },
            { nameof(_head64.WinNtOptional.FileAlignment), $"0x{_head64.WinNtOptional.FileAlignment:X}" },
            { nameof(_head64.WinNtOptional.MajorOperatingSystemVersion), $"0x{_head64.WinNtOptional.MajorOperatingSystemVersion:X}" },
            { nameof(_head64.WinNtOptional.MinorOperatingSystemVersion), $"0x{_head64.WinNtOptional.MinorOperatingSystemVersion:X}" },
            { nameof(_head64.WinNtOptional.MajorImageVersion), $"0x{_head64.WinNtOptional.MajorImageVersion:X}" },
            { nameof(_head64.WinNtOptional.MinorImageVersion), $"0x{_head64.WinNtOptional.MinorImageVersion:X}" },
            { nameof(_head64.WinNtOptional.MajorSubsystemVersion), $"0x{_head64.WinNtOptional.MajorSubsystemVersion:X}" },
            { nameof(_head64.WinNtOptional.MinorSubsystemVersion), $"0x{_head64.WinNtOptional.MinorSubsystemVersion:X}" },
            { nameof(_head64.WinNtOptional.Win32VersionValue), $"0x{_head64.WinNtOptional.Win32VersionValue:X}" },
            { nameof(_head64.WinNtOptional.SizeOfImage), $"0x{_head64.WinNtOptional.SizeOfImage:X}" },
            { nameof(_head64.WinNtOptional.SizeOfHeaders), $"0x{_head64.WinNtOptional.SizeOfHeaders:X}" },
            { nameof(_head64.WinNtOptional.CheckSum), $"0x{_head64.WinNtOptional.CheckSum:X}" }, // Контрольная сумма
            { nameof(_head64.WinNtOptional.Subsystem), $"0x{_head64.WinNtOptional.Subsystem:X}" },
            { nameof(_head64.WinNtOptional.DllCharacteristics), $"0x{_head64.WinNtOptional.DllCharacteristics:X}" },
            { nameof(_head64.WinNtOptional.SizeOfStackReserve), $"0x{_head64.WinNtOptional.SizeOfStackReserve:X}" },
            { nameof(_head64.WinNtOptional.SizeOfStackCommit), $"0x{_head64.WinNtOptional.SizeOfStackCommit:X}" },
            { nameof(_head64.WinNtOptional.SizeOfHeapReserve), $"0x{_head64.WinNtOptional.SizeOfHeapReserve:X}" },
            { nameof(_head64.WinNtOptional.SizeOfHeapCommit), $"0x{_head64.WinNtOptional.SizeOfHeapCommit:X}" },
            { nameof(_head64.WinNtOptional.LoaderFlags), $"0x{_head64.WinNtOptional.LoaderFlags:X}" },
            { nameof(_head64.WinNtOptional.NumberOfRvaAndSizes), $"0x{_head64.WinNtOptional.NumberOfRvaAndSizes:X}" }
        };
        return header;
    }

    [UnderConstruction]
    public Dictionary<string, string[]> GetFlags()
    {
        Dictionary<string, string[]> result = new();
        ushort characteristics = (_head32.Signature == 0)
            ? _head64.WinNtMain.Characteristics
            : _head32.WinNtMain.Characteristics;
        
        string[] charsSegment = new[]
        {
            ((characteristics & 0x0001) != 0) ? "RELOCS_STRIPPED" : "",
            ((characteristics & 0x0002) != 0) ? "EXECUTABLE_IMAGE" : "",
            ((characteristics & 0x0004) != 0) ? "LINE_NUMS_STRIPPED" : "",
            ((characteristics & 0x0008) != 0) ? "LOCAL_SYMS_STRIPPED" : "",
            ((characteristics & 0x0010) != 0) ? "AGGRESIVE_WS_TRIM" : "",
            ((characteristics & 0x0020) != 0) ? "LARGE_ADDRESS_AWARE" : "",
            ((characteristics & 0x0040) != 0) ? "BYTES_REVERSED_LO" : "",
            ((characteristics & 0x0080) != 0) ? "32BIT_MACHINE" : "",
            ((characteristics & 0x0100) != 0) ? "DEBUG_STRIPPED" : "",
            ((characteristics & 0x0200) != 0) ? "REMOVABLE_RUN_FROM_SW" : "",
            ((characteristics & 0x0400) != 0) ? "NET_RUN_FROM_SWAP" : "",
            ((characteristics & 0x0800) != 0) ? "SYSTEM_IMAGE" : "",
            ((characteristics & 0x1000) != 0) ? "DLL" : "",
            ((characteristics & 0x2000) != 0) ? "UP_SYSTEM_ONLY" : "",
            ((characteristics & 0x4000) != 0) ? "BYTES_REVERSED_HI" : "",
        }
            .Where(c => !string.IsNullOrEmpty(c))
            .Distinct()
            .ToArray();
        
        characteristics = (_head32.Signature == 0)
            ? _head64.WinNtOptional.DllCharacteristics
            : _head32.WinNtOptional.DllCharacteristics;
        
        string[] dllCharacteristics = new[]
        {
            ((characteristics & 0x0001) != 0) ? "DLL_HIGH_ENTROPY_VA" : "",
            ((characteristics & 0x0020) != 0) ? "DLL_DYNAMIC_BASE" : "",
            ((characteristics & 0x0040) != 0) ? "DLL_FORCE_INTEGRITY" : "",
            ((characteristics & 0x0080) != 0) ? "DLL_NX_COMPAT" : "",
            ((characteristics & 0x0100) != 0) ? "DLL_NO_ISOLATION" : "",
            ((characteristics & 0x0200) != 0) ? "DLL_NO_SEH" : "",
            ((characteristics & 0x0400) != 0) ? "DLL_NO_BIND" : "",
            ((characteristics & 0x0800) != 0) ? "DLL_APP_CONTAINER" : "",
            ((characteristics & 0x1000) != 0) ? "DLL_WDM_DRIVER" : "",
            ((characteristics & 0x2000) != 0) ? "DLL_GUARD_CF" : "",
            ((characteristics & 0x4000) != 0) ? "DLL_TERMINAL_SERVER_AWARE" : "",
        }
            .Where(d => !string.IsNullOrEmpty(d))
            .Distinct()
            .ToArray();
        
        result.Add("_characteristics", charsSegment);
        result.Add("_dllCharacteristics", dllCharacteristics);

        return result;
    }

    public CommonProperties GetProperties()
        => _head32.Signature == 0 
            ? GetPropertiesForQWordLengthHeader() 
            : GetPropertiesForDWordLengthHeader();
    
    [UnderConstruction("Image's type detects incorrect.")]
    private CommonProperties GetPropertiesForDWordLengthHeader() => new() 
    {
        OperatingSystem = _strings.OperatingSystemFlagToString(0),
        OperatingSystemVersion = _strings.OperatingSystemVersionToString(_head32.WinNtOptional.MajorOperatingSystemVersion, _head32.WinNtOptional.MinorOperatingSystemVersion),
        CpuArchitecture = _strings.CpuArchitectureFlagToString(_head32.WinNtMain.Machine),
        CpuWordLength = _strings.CpuWordLengthFlagToString(_head32.WinNtOptional.Magic),
        ImageType = _strings.ImageTypeFlagToString(_head32.WinNtMain.Characteristics),
        LinkerVersion = _strings.ImageVersionFlagsToString(_head32.WinNtOptional.MajorLinkerVersion, _head32.WinNtOptional.MinorLinkerVersion),
        Subsystem = _strings.SubsystemFromOperatingSystemFlagToString(_head32.WinNtOptional.Subsystem),
    };

    [UnderConstruction("Image's type detects incorrect.")]
    private CommonProperties GetPropertiesForQWordLengthHeader() => new() 
    {
        OperatingSystem = _strings.OperatingSystemFlagToString(0),
        OperatingSystemVersion = _strings.OperatingSystemVersionToString(_head64.WinNtOptional.MajorOperatingSystemVersion, _head64.WinNtOptional.MinorOperatingSystemVersion),
        CpuArchitecture = _strings.CpuArchitectureFlagToString(_head64.WinNtMain.Machine),
        CpuWordLength = _strings.CpuWordLengthFlagToString(_head64.WinNtOptional.Magic),
        ImageType = _strings.ImageTypeFlagToString(_head64.WinNtMain.Characteristics),
        LinkerVersion = _strings.ImageVersionFlagsToString(_head64.WinNtOptional.MajorLinkerVersion, _head64.WinNtOptional.MinorLinkerVersion),
        Subsystem = _strings.SubsystemFromOperatingSystemFlagToString(_head64.WinNtOptional.Subsystem),
        RuntimeWord = GetRuntimeWord(_fileName)
    };
    
    [UnderConstruction("May not work. Fuck you")]
    public Dictionary<string, string> GetImports()
    {
        PortableExecutableSectionsReader reader = new(_fileName);
        return new Dictionary<string, string>();
    }

    [UnderConstruction("Fuck it")]
    public string GetRuntimeWord(string path)
    {
        try
        {
            Assembly assembly = Assembly.LoadFile(path);

            return (assembly.ImageRuntimeVersion == string.Empty)
                ? "OS ABI"
                : $"Common IL ({assembly.ImageRuntimeVersion})";
        }
        catch
        {
            // ignored
        }

        return "OS ABI";
    }

    public static uint RvaToFileOffset()
    {
        return 0;
    }
}