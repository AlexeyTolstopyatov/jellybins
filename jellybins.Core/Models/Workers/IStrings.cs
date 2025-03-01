using System.Runtime.InteropServices;
using jellybins.Core.Strings;

namespace jellybins.Core.Models.Workers;

public interface IStrings
{
    /// <summary>
    /// Detecting Operating System flag
    /// </summary>
    /// <param name="os">Value of required Operating system</param>
    /// <typeparam name="T">Numeric type (<see cref="Int16"/> or <see cref="Byte"/>)</typeparam>
    /// <returns></returns>
    string OperatingSystemFlagToString<T>([Optional] T os) where T : IComparable;
    /// <summary>
    /// Detecting required CPU architecture
    /// </summary>
    /// <param name="cpu">Value of required CPU</param>
    /// <typeparam name="T">Numeric type</typeparam>
    /// <returns></returns>
    string CpuArchitectureFlagToString<T>(T cpu) where T : IComparable;
    /// <summary>
    /// Returns Version string based on two integers
    /// </summary>
    /// <param name="major">Main version value</param>
    /// <param name="minor">Big update version value</param>
    /// <typeparam name="T">Numeric type</typeparam>
    /// <returns></returns>
    string OperatingSystemVersionToString<T>(T major, T minor) where T : IComparable;
    /// <summary>
    /// Returns bitness of exploring executable
    /// </summary>
    /// <returns></returns>
    string CpuWordLengthFlagToString<T>([Optional] T word);

    /// <summary>
    /// Returns subsystem name for target binary
    /// </summary>
    /// <param name="word"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Subsystem SubsystemFromOperatingSystemFlagToString<T>([Optional] T word);
    /// <summary>
    /// Returns type of exploring image (binary)
    /// </summary>
    /// <param name="word"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    string ImageTypeFlagToString<T>(T word);
}