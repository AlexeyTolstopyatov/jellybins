using System.Runtime.InteropServices;
using jellybins.Core.Exceptions;
using jellybins.Core.Interfaces;
using jellybins.Core.Strings;

namespace jellybins.Core.Readers.LinearExecutable;

public class WindowsDeviceDriverStrings : IStrings
{
    public string OperatingSystemFlagToString<T>([Optional]T os) where T : IComparable
    {
        return "Microsoft Windows";
    } 

    public string CpuArchitectureFlagToString<T>(T cpu) where T : IComparable
    {
        return Convert.ToInt32(cpu) switch
        {
            1 => "Intel i286",
            2 => "Intel i386",
            3 => "Intel i486",
            _ => throw new UndefinedArgumentException(Convert.ToInt32(cpu))
        };
    }
    public string OperatingSystemVersionToString<T>(T major, T minor) where T : IComparable
    {
        ushort mj = Convert.ToUInt16(major);
        ushort mi = Convert.ToUInt16(minor);
        return $"{mi}.{mj}";
    }

    public string ImageVersionFlagsToString<T>(T major, T minor)
    {
        ushort mj = Convert.ToUInt16(major);
        ushort mi = Convert.ToUInt16(minor);
        return $"{mj}.{mi}";
    }

    public string CpuWordLengthFlagToString<T>(T word = default(T))
    {
        return "16";
    }

    public string SubsystemFromOperatingSystemFlagToString<T>(T word = default(T))
    {
        return PortableExecutableSubsystem.Native.ToString();
    }

    public string ImageTypeFlagToString<T>(T word)
    {
        return ImageType.VirtualDriver.ToString();
    }
}