namespace jellybins.Core.Utilities;

/// <summary>
/// Describes functions for not-fatal messages.
/// It was needed for me for binaries exploring and making notes
/// of binary structures and services behaviour.
/// Works if Compiled builds Debug-configured image.
/// </summary>
public static class InternalDebugger
{
    public static void PrintInformation(string str)
    {
        Print("[i]: " + str, ConsoleColor.Cyan);
    }

    public static void PrintWarning(string str)
    {
        Print("[!]: " + str, ConsoleColor.Yellow);   
    }

    private static void Print(string str, ConsoleColor color)
    {
        #if DEBUG
        Console.ForegroundColor = color;
        Console.WriteLine(str);
        Console.ResetColor();
        #endif
    }
}