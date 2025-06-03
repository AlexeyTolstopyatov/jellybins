using System.Data;
using JellyBins.Abstractions;

namespace JellyBins.Client.Models;

public class FileDumpModel(IDrawer drawer)
{
    public String Name { get; private set; } = drawer.InfoDictionary["FileName"];
    public String Path { get; private set; } = drawer.InfoDictionary["FilePath"];
    public String CpuArchitecture { get; private set; } = drawer.InfoDictionary["Target CPU"];
    public String CpuMaxWord { get; private set; } = drawer.InfoDictionary["Max. WORD"];
    public DataTable[] Headers { get; private set; } = drawer.HeadersTables;
    public DataTable[] Sections { get; private set; } = drawer.SectionTables;
    public DataTable[] Imports { get; private set; } = drawer.ImportsTables;
    public DataTable[] Exports { get; private set; } = drawer.ExportsTables;
    public String[] ExternToolChain { get; private set; } = drawer.ExternToolChain;
    public String[][] HeadersCharacteristics { get; private set; } = drawer.HeadersCharacteristics;
    public String[][] SectionsCharacteristics { get; private set; } = drawer.SectionsCharacteristics;
}