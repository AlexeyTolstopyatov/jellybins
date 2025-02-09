namespace jellybins.Core.Models.Flags;

public struct NewExecutableFlags
{
    /// <summary>
    /// Contain important flags which helps loader
    /// to recognize binary specific
    /// (.DATA behaviour, required CPU, Memory management)
    /// </summary>
    public string[] Program;
    /// <summary>
    /// Contain flags for Operating System, which define
    /// GUI behaviour (Window mode, Line behaviour).
    /// It creates part of compatible ware between OS/2 and Windows
    /// </summary>
    public string[] Application;
    /// <summary>
    /// Contain flags for IBM OS/2, which helps loader and other OS modules
    /// to build living conditions for binary (e.g. File system differences, Fonts behaviour, etc.)
    /// </summary>
    public string[] Os2;
}