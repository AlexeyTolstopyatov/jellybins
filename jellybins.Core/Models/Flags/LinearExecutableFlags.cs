namespace jellybins.Core.Models.Flags;

public struct LinearExecutableFlags
{
    /// <summary>
    /// Contains all specific flags, including binary recognition flag
    /// (library, executable, driver). All information in LX format stores here.
    /// </summary>
    public string[] ModuleFlags;
}