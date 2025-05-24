using System.Data;
using JellyBins.Abstractions;
using JellyBins.DosCommand.Models;

namespace JellyBins.Core.Drawers;

public class ComDrawer(ComFileDumper dumper) : IDrawer
{
    public DataTable[] HeadersTables { get; private set; } = [];
    public DataTable[] SectionTables { get; private set; } = [];
    public DataTable[] ImportsTables { get; private set; } = [];
    public DataTable[] ExportsTables { get; private set; } = [];
    public Dictionary<String, String> InfoDictionary { get; } = [];
    
    /// <summary>
    /// does nothing (specially)
    /// </summary>
    public void MakeHeadersTables()
    {
        // do nothing. Headers missing
    }
    /// <summary>
    /// Makes table for every sections
    /// </summary>
    public void MakeSectionsTables()
    {
        List<DataTable> sectionTables = [];
        foreach (ComSectionDump sectionDump in dumper.Sections!)
        {
            DataTable table = new();
            
            table.Columns.Add("Dump Name");
            table.Columns.Add("Dump Size");
            table.Columns.Add("Dump Address");
            table.Columns.Add("Name");
            table.Columns.Add("Address");
            table.Columns.Add("Size");
            table.Columns.Add("Characterisrics");

            String charSet = sectionDump
                .Characteristics!
                .Aggregate("", 
                    (current, characteristic) => current + characteristic + "\n");

            table.Rows.Add(
                sectionDump.Name,
                sectionDump.Size,
                sectionDump.Address,
                sectionDump.Segmentation.Name,
                sectionDump.Segmentation.Address,
                sectionDump.Segmentation.Size,
                charSet);
            
            sectionTables.Add(table);
        }

        SectionTables = sectionTables.ToArray();
    }
    
    public void MakeInfo()
    {
        if (dumper.Equals(null))
            return;
        
        InfoDictionary.Add("Name", dumper.Info.Name!);
        InfoDictionary.Add("Path", dumper.Info.Path!);
        InfoDictionary.Add("CPU", dumper.Info.CpuArchitecture!);
        InfoDictionary.Add("CPU [WORD]", $"{dumper.Info.CpuWordLength} BIT");
        InfoDictionary.Add("OS", dumper.Info.OperatingSystem!);
        InfoDictionary.Add("OS Version", dumper.Info.OperatingSystemVersion!);
    }
}