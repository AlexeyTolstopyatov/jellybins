namespace jellybins.Core.Models.Flags;

/// <summary>
/// Represents PE specific flags
/// </summary>
public struct PortableExecutableFlags
{
    /// <summary>
    /// All common characteristics of PE file.
    /// (contains Loader behaviour, binary type, etc.)
    /// </summary>
    public string[] Characteristics;
    /// <summary>
    /// Specific executable characteristics
    /// for Windows. (e.g. binding with Terminal Server, Compatible layer, memory behaviour)
    /// </summary>
    public string[] DllCharacteristics;
    /// <summary>
    /// Subsystem ware for applications live.
    /// (Win32/WinRT/POSIX/WinCE/POSIX/EFI/XBOX...)
    /// </summary>
    public string Subsystem;
}