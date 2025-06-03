using System.Data;
using System.Diagnostics.Eventing.Reader;
using JellyBins.Abstractions;
using JellyBins.Core;

namespace JellyBins.Client.Models;

public class FileInfoModel(IDrawer drawer)
{
    public String Name { get; private set; } = drawer.InfoDictionary["FileName"];
    public String Path { get; private set; } = drawer.InfoDictionary["FilePath"];
    public String CpuArchitecture { get; private set; } = drawer.InfoDictionary["Target CPU"];
    public String CpuMaxWord { get; private set; } = drawer.InfoDictionary["Max. WORD"];
    public String OsName { get; private set; } = drawer.InfoDictionary.Values.ElementAt(5);
    public String OsVersion { get; private set; } = drawer.InfoDictionary["Target OS ver."];
    
    public String[] Characteristics { get; private set; } = drawer.Characteristics;
    
    public String DateStamp => 
        drawer.InfoDictionary.TryGetValue("BirthDay", out String? s) ? s : String.Empty;
    public String Title =>
        drawer.InfoDictionary.TryGetValue("FileTitle", out String? s) ? s : String.Empty;
    public String MinimumOsVersion =>
        drawer.InfoDictionary.TryGetValue("Minimum OS ver.", out String? s) ? s : String.Empty;
    public String[] ExternToolChain { get; private set; } = drawer.ExternToolChain;
}