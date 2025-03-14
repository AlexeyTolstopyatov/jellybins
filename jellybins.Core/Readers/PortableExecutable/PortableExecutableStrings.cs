using System.Data;
using jellybins.Core.Attributes;
using jellybins.Core.Exceptions;
using jellybins.Core.Interfaces;
using jellybins.Core.Strings;

namespace jellybins.Core.Readers.PortableExecutable;

public class PortableExecutableStrings : IStrings
{
    public string OperatingSystemFlagToString<T>(T os = default(T)) where T : IComparable
    {
        return "Microsoft Windows";
    }
    
    public string CpuArchitectureFlagToString<T>(T cpu) where T : IComparable
    {
        return Convert.ToUInt32(cpu) switch
        {
            0x014c => "Intel i386",
            0x8664 => "Intel x86-64",
            0x0200 => "Intel Itanium",
            0x1c0 => "ARM",
            0xaa64 => "ARM 64",
            0xebc => "EFI",
            _ => throw new UndefinedArgumentException(Convert.ToInt32(cpu))
        };
    }

    public string OperatingSystemVersionToString<T>(T major, T minor) where T : IComparable
    {
        return $"{major}.{minor}";
    }

    public string ImageVersionFlagsToString<T>(T major, T minor)
    {
        return $"{major}.{minor}";
    }

    [UnderConstruction("Can't work with EFI PE binaries!")]
    public string CpuWordLengthFlagToString<T>(T word = default(T))
    {
        // get magic from Optional PE header
        return Convert.ToInt32(word) switch
        {
            0x10b => "32",
            0x20b => "64",
            _ => throw new UndefinedArgumentException(Convert.ToInt32(word))
        };
    }

    public string SubsystemFromOperatingSystemFlagToString<T>(T word = default(T))
    {
        ushort ss = Convert.ToUInt16(word);
        return ss switch
        {
            (ushort)PortableExecutableSubsystem.Native => "Native",
            (ushort)PortableExecutableSubsystem.Win32Cui => "Win32 Console Interface",
            (ushort)PortableExecutableSubsystem.Win32Gui => "Win32 Graphic User Interface",
            (ushort)PortableExecutableSubsystem.Os2SsCui => "OS/2 Console Interface",
            (ushort)PortableExecutableSubsystem.PosixSs => "POSIX",
            (ushort)PortableExecutableSubsystem.WindowsNative => "Windows Native",
            (ushort)PortableExecutableSubsystem.WinCeGui => "Windows CE Graphic User Interface",
            (ushort)PortableExecutableSubsystem.EfiApplication => "EFI Application",
            (ushort)PortableExecutableSubsystem.EfiBootDriver => "EFI Bootable Driver",
            (ushort)PortableExecutableSubsystem.EfiRomImage => "EFI ROM",
            (ushort)PortableExecutableSubsystem.XboxApplication => "XBOX Application",
            (ushort)PortableExecutableSubsystem.WinBootApplication => "Windows Bootable Application",
            _ => ss.ToString()
        };
    }
    
    public string ImageTypeFlagToString<T>(T word)
    {
        ushort table = Convert.ToUInt16(word);
        
        if ((table & 0x0002) != 0) return ImageType.Application.ToString();
        if ((table & 0x2000) != 0) return ImageType.DynamicLinkedLibrary.ToString();
        
        throw new UndefinedArgumentException(table);
    }

    public bool ImageTypeFlagIsWindowsDriver(ushort word)
    {
        return (word & 0x2000) != 0;
    }
}