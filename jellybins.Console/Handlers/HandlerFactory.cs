using jellybins.Console.Interfaces;
using static System.Console;
namespace jellybins.Console.Handlers;

/// <summary>
/// Represents IHandler-maker method.
/// Make Automatic object.
/// </summary>
public static class HandlerFactory
{
    // I feel myself lost inside those structures and possibilities
    // All people can make mistakes... I have no person who I can
    // ask about it. I must make all modules by myself... I'm drowning in it.
    // They're only joking, I'm silly, I don't know .NET, I don't know object-oriented idea...
    // I really dont understand many concepts, but I need to know that and use that.
    /// <summary>
    /// Makes Core Readers API connect via IHandler instance.
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static IHandler CreateHandler(string path) =>
        new ActiveHandler(path);

    /// <summary>
    /// Makes Core readers API connect via IHandler instance
    /// </summary>
    /// <returns></returns>
    public static IHandler CreateHandler() => new ActiveHandler();
    /// <summary>
    /// Makes Core readers API connect via IHandler instance
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    public static IHandler CreateHandler(ref string[] args)
    {
        var reqs = ParseArguments(args);
        
        if (reqs.Where(k => k.Key == "bin").Distinct().Count()! == 0)
            throw new Exception();
        
        return new InteractiveHandler(reqs);
    }
    
    /// <summary>
    /// Transforms array of strings to key-value pairs
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    private static Dictionary<string, string> ParseArguments(string[] args)
    {
        var result = new Dictionary<string, string>();

        for (int i = 0; i < args.Length; i++)
        {
            // value must go after key expression
            if (!args[i].StartsWith("--")) continue;
            
            string key = args[i].Substring(2);
            string? value = null;

            if (i + 1 < args.Length && !args[i + 1].StartsWith("--"))
            {
                value = args[i + 1];
                i++; // Skip next entity. (--key value)
            }

            if (value != null) result[key] = value;
            else result[key] = "";
        }

        return result;
    }
}