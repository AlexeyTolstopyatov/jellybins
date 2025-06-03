using System.Data;

namespace JellyBins.Abstractions;

public interface IDrawer
{
    public DataTable[] HeadersTables { get; }
    public DataTable[] SectionTables { get; }
    public DataTable[] ImportsTables { get; }
    public DataTable[] ExportsTables { get; }
    public Dictionary<String, String> InfoDictionary { get; }
    public void MakeHeadersTables();
    public void MakeSectionsTables();
    public void MakeInfo();
    public String[] Characteristics { get; }
    public String[] ExternToolChain { get; }
}