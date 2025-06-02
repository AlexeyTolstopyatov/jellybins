using JellyBins.Abstractions;

namespace JellyBins.Client.Models;

public class FileInfoModel(IDrawer drawer)
{
    public String Name { get; private set; } = drawer.InfoDictionary["FileName"];
    public String Path { get; private set; } = drawer.InfoDictionary["FilePath"];
    public String CpuArchitecture { get; private set; } = drawer.InfoDictionary["Target CPU"];
    public String CpuMaxWord { get; private set; } = drawer.InfoDictionary["Max. WORD"];
    public String OsName { get; private set; } = drawer.InfoDictionary["Target OS"];
    public String OsVersion { get; private set; } = drawer.InfoDictionary["Target OS ver."];
    public Dictionary<String, String> InfoDictionary { get; init; } = drawer.InfoDictionary;
}