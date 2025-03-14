using jellybins.Core.Strings;

namespace jellybins.Core.Models;

/// <summary>
/// Represents important fields of exploring executable
/// that must be filled for everyone
/// </summary>
public struct CommonProperties
{
    /// <summary>
    /// OperatingSystemVersion of Operating System
    /// </summary>
    public string OperatingSystemVersion;
    /// <summary>
    /// Version of binary, builtin header
    /// </summary>
    public string LinkerVersion;
    /// <summary>
    /// Targeted CPU Architecture name
    /// </summary>
    public string CpuArchitecture;
    /// <summary>
    /// Required OS for Running
    /// </summary>
    public string OperatingSystem;
    /// <summary>
    /// Name of Microsoft Windows <see cref="PortableExecutableSubsystem"/>
    /// Microsoft binaries contain information
    /// about using layer of Operating System (Win32, POSIX compat, OS/2 compat etc...)
    /// </summary>
    public string Subsystem;
    /// <summary>
    /// Type of exploring binary file (e.g. DynamicLinkedLibrary, Executable, Archive, etc.) 
    /// </summary>
    public string ImageType;
    /// <summary>
    /// Maximum CPU word length support. (output format is "16" or "64").
    /// </summary>
    public string CpuWordLength;
    /// <summary>
    /// Suggested runtime complex usage. (CLR/VB5/GO/... etc)
    /// </summary>
    public string? RuntimeWord;
}
