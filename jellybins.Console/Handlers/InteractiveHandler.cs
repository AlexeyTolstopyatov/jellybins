using jellybins.Console.Interfaces;
using jellybins.Console.Utilities;
using jellybins.Core.Attributes;
using jellybins.Core.Interfaces;
using jellybins.Core.Models;
using jellybins.Core.Readers.Factory;
using static System.Console;

namespace jellybins.Console.Handlers;

/// <summary>
/// Represents handler which starts at once, if
/// application calls with extra command-line arguments
/// Application executes command, then terminates.
///
/// This entity must hide Core API usage (replace API bridge to Handler-bridge?)
/// </summary>
public class InteractiveHandler : IHandler
{
    private readonly IReader _reader;
    /// <summary>
    /// Starts new handler work.
    /// </summary>
    /// <param name="args"></param>
    public InteractiveHandler(Dictionary<string, string> args)
    {
        string path = 
            args
                .First(x => x.Key == "bin")
                .Value;
        
        // it calls outside. (uses IHandler API)
        _reader = new ReaderFactory(path).CreateReader();
        if (args.ContainsKey("head")) BuildHeaderTable();
        if (args.ContainsKey("list")) BuildPropertiesTable();
        if (args.ContainsKey("flags")) BuildFlagsTable();
        if (args.ContainsKey("imports")) BuildImportsTable();
    }
    
    public void BuildPropertiesTable()
    {
        CommonProperties properties = _reader.GetProperties();
        WriteLine(@$"
{nameof(properties.ImageType)}              {properties.ImageType}
{nameof(properties.Subsystem)}              {properties.Subsystem}
{nameof(properties.CpuArchitecture)}        {properties.CpuArchitecture}
{nameof(properties.OperatingSystem)}        {properties.OperatingSystem}
{nameof(properties.CpuWordLength)}          {properties.CpuWordLength}
{nameof(properties.LinkerVersion)}          {properties.LinkerVersion}
{nameof(properties.OperatingSystemVersion)} {properties.OperatingSystemVersion}");
    }
    [UnderConstruction]
    public void BuildHeaderTable()
    {
        var head = _reader.GetHeader();
        TablePainter.Paint(head);
    }
    [UnderConstruction]
    public void BuildImportsTable()
    {
        throw new NotImplementedException();
    }
    [UnderConstruction]
    public void BuildFlagsTable()
    {
        foreach (var category in _reader.GetFlags())
        foreach (var flag in category.Value)
            WriteLine(category.Key + "\t" + flag);
    }
}