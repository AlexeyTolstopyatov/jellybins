using System.Runtime.InteropServices;
using jellybins.Core.Interfaces;

namespace jellybins.Core.Strings;

public class MarkZbykowskiExecutableStrings : IStrings
{
    public string OperatingSystemFlagToString<T>(T os = default(T)) where T : IComparable
    {
        return "Microsoft DOS";
    }

    public string CpuArchitectureFlagToString<T>(T cpu) where T : IComparable
    {
        return "Intel i8086";
    }

    public string OperatingSystemVersionToString<T>(T major, T minor) where T : IComparable
    {
        return "2.0";
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
        return Subsystem.NtVirtualDosMachine.ToString();
    }

    public string ImageTypeFlagToString<T>(T word)
    {
        return ImageType.Application.ToString();
    }
}