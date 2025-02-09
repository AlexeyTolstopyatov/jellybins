namespace jellybins.Core.Models.Flags;

public struct AssemblerOutFlags
{
    /// <summary>
    /// Important flag, which determines loader behaviour
    /// </summary>
    public string Magic;
    /// <summary>
    /// Based on <see cref="Magic"/> suggestions for every Unix-like
    /// Operating system. <para/> (Every old Unix-like OS has unique definitions
    /// for Magic word. It makes many troubles in Loader behaviour recognition)
    /// </summary>
    public string[] Loader;
    /// <summary>
    /// Binary characteristics
    /// </summary>
    public string[] Characteristics;
}