using System.Runtime.InteropServices;
using jellybins.Core.Exceptions;
using jellybins.Core.Interfaces;

namespace jellybins.Core.Strings;

public class NewExecutableStrings : IStrings
{
    public string OperatingSystemFlagToString<T>(T os = default!) where T : IComparable
    {
        return Convert.ToByte(os) switch
        {
            0x0 => "Unknown",
            0x1 => "IBM OS/2",
            0x2 => "Microsoft Windows/286",
            0x3 => "Microsoft DOS",
            0x4 => "Microsoft Windows/386",
            0x5 => "Borland OS",
            _ => throw new UndefinedArgumentException(Convert.ToInt32(os))
        };
    }

    public string CpuArchitectureFlagToString<T>(T cpu) where T : IComparable
    {
        ushort pflags = Convert.ToUInt16(cpu);

        if ((pflags & 0x4) != 0) return "Intel 8086";
        if ((pflags & 0x5) != 0) return "Intel i286";
        if ((pflags & 0x6) != 0) return "Intel i386";
        if ((pflags & 0x7) != 0) return "Intel x87";

        throw new UndefinedArgumentException(pflags);
    }

    public string OperatingSystemVersionToString<T>(T major, T minor) where T : IComparable
    {
        return $"{major}.{minor}";
    }

    public string ImageVersionFlagsToString<T>(T major, T minor)
    {
        return $"{major}.{minor}";
    }

    public string CpuWordLengthFlagToString<T>([Optional] T word)
    {
        return "16";
    }

    public string SubsystemFromOperatingSystemFlagToString<T>(T word = default(T))
    {
        byte os = Convert.ToByte(word);
        return os switch
        {
            0x2 or 0x4 => Subsystem.Win16Gui.ToString(),
            0x1 => Subsystem.Os2SsCui.ToString(),
            _ => Subsystem.NtVirtualDosMachine.ToString()
        };
    }

    public string ImageTypeFlagToString<T>(T word)
    {
        ushort appFlags = Convert.ToUInt16(word);
        if ((appFlags & 0x80) != 0) return ImageType.DynamicLinkedLibrary.ToString();
        if ((appFlags & 0x40) != 0) return ImageType.WithLinkerErrors.ToString();
        
        return ImageType.Application.ToString();
    }
}