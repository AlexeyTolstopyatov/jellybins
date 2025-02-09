using jellybins.Console.Interfaces;
using jellybins.Console.Utilities;
using jellybins.Core.Attributes;
using jellybins.Core.Interfaces;
using jellybins.Core.Models;
using jellybins.Core.Readers.Factory;
using static System.Console;

namespace jellybins.Console.Handlers;

/// <summary>
/// Represents shell-mode handler.
/// Calls when application starts without command-line arguments
///
/// This entity must hide Core API...
/// </summary>
public class ActiveHandler : IHandler
{
    private IReader _reader;
    
    public ActiveHandler()
    {
        _reader = null!;
        UpdateReader();
        CreateDialog(); 
    }
    
    public ActiveHandler(string path)
    {
        _reader = new ReaderFactory(path).CreateReader();
        CreateDialog();
    }

    private void CreateDialog()
    {
        try
        {
            _reader.GetHeader();
        }
        catch
        {
            WriteLine("Unable to read entity");
            return;
        }
        WriteLine("JellyBins (C) Tolstopyatov Alexey 2024-2025");
        
        while (true)
        {
            Write("binary_part> ");
            string query = ReadLine()!;
            switch (query)
            {
                // catching requests
                case "plist":
                    BuildPropertiesTable();
                    break;
                case "head":
                    BuildHeaderTable();
                    break;
                case "flags":
                    BuildFlagsTable();
                    break;
                case "exit":
                    return;
                case "new":
                    UpdateReader();
                    break; // terminate shell mode
                default:
                    WriteLine("Unknown part");
                    break;
            }
        }
    }
    private void UpdateReader()
    {
        Write("binary_path> ");
        string path = ReadLine()!;
        if (!File.Exists(path))
        {
            WriteLine("Unable to read entity");
            return;
        }

        _reader = new ReaderFactory(path).CreateReader();
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
        TablePainter.Paint(_reader.GetHeader());
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