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
        switch (dumper.SegmentationType)
        {
            case FileSegmentationType.PortableExecutable:
                var pDebug = (dumper as PeFileDumper)!.ImportsDump.FoundImports.Select(names => names.DllName).ToArray();
                return pDebug;
            case FileSegmentationType.LinearExecutable:
                var lDebug = (dumper as LeFileDumper)!.ImportModulesTable;
                return lDebug;
            case FileSegmentationType.NewExecutable:
                var nDebug = (dumper as NeFileDumper)!.ImportsTableDump.Select(x => x.Segmentation.Name).ToArray();
                return nDebug;
            default:
                throw new ArgumentOutOfRangeException(nameof(dumper.SegmentationType));
        }
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

        Dictionary<String, String[]>? list = JsonSerializer.Deserialize<Dictionary<String, String[]>>(File.ReadAllText(dictionaryPath))!;
        
        foreach (String dumpedDllName in ExtractDllNamesFromDump())
        {
            List<String> x = list
                .Where(x => x.Value.Any(s => String.Equals(
                    s, 
                    dumpedDllName, 
                    StringComparison.OrdinalIgnoreCase)))
                .Select(x => x.Key + $"({dumpedDllName})")
                .ToList();
            
            categories.AddRange(x);
        }
        
        return categories.ToArray();
    } 
}