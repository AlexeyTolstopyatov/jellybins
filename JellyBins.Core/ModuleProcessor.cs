using System.Linq.Expressions;
using System.Text.Json;
using JellyBins.Abstractions;
using JellyBins.LinearExecutable.Models;
using JellyBins.NewExecutable.Models;
using JellyBins.PortableExecutable.Models;

namespace JellyBins.Core;

public class ModuleProcessor(IFileDumper dumper) : IModuleProcessor
{
    /// <param name="dict">path to JSON dictionary</param>
    /// <returns> array of DLL arrays from dictionary </returns>
    private String[][] ExtractDllNamesFromDictionary(JsonDictionary[] dict)
    {
        return dict.Select(n => n.DllNames).ToArray();
    }
    /// <returns> only import DLL array</returns>
    private String[] ExtractDllNamesFromDump()
    {

        return dumper.SegmentationType switch
        {
            FileSegmentationType.PortableExecutable => (dumper as PeFileDumper)!
                .ImportsDump
                .FoundImports
                .Select(names => names.DllName)
                .ToArray(),
            FileSegmentationType.LinearExecutable => (dumper as LeFileDumper)!
                .ImportModulesTable,
            FileSegmentationType.NewExecutable => (dumper as NeFileDumper)!
                .ImportsTableDump
                .Select(x => x.Segmentation.Name)
                .ToArray(),
            _ => []
        };
    }
    
    /// <returns> Keys array of dictionary (or nothing) </returns>
    public String[] TryToKnowUsedModules()
    {
        String bin = dumper.SegmentationType switch
        {
            FileSegmentationType.PortableExecutable => nameof(PortableExecutable),
            FileSegmentationType.LinearExecutable => nameof(LinearExecutable),
            FileSegmentationType.NewExecutable => nameof(NewExecutable),
            _ => "Executable"
        };
        String dictionaryPath =
            AppDomain.CurrentDomain.BaseDirectory +
            nameof(JellyBins) + "." +
            bin + "." +
            "Dictionary.json";
        List<String> categories = [];
        
        if (!File.Exists(dictionaryPath))
            return [];

        JsonDictionary[] list = JsonSerializer.Deserialize<JsonDictionary[]>(dictionaryPath)!;
        
        foreach (String dumpedDllName in ExtractDllNamesFromDump())
        {
            List<String> x = list
                .Where(x => x.DllNames.Contains(dumpedDllName))
                .Select(x => x.Category)
                .ToList();
            
            categories.AddRange(x);
        }
        
        return categories.ToArray();
    } 
}