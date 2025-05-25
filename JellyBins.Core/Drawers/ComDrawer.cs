using System.Data;
using JellyBins.Abstractions;
using JellyBins.DosCommand.Models;

namespace JellyBins.Core.Drawers;

public class ComDrawer : IDrawer
{
    private ComFileDumper _dumper;
    public ComDrawer(ComFileDumper dumper)
    {
        _dumper = dumper;
        
        MakeInfo();
        MakeHeadersTables();
        MakeSectionsTables();
    }
    public DataTable[] HeadersTables { get; private set; } = [];
    public DataTable[] SectionTables { get; private set; } = [];
    public DataTable[] ImportsTables { get; private set; } = [];
    public DataTable[] ExportsTables { get; private set; } = [];
    public Dictionary<String, String> InfoDictionary { get; private set; } = [];
    
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
        foreach (ComSectionDump sectionDump in _dumper.Sections!)
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
        Dictionary<String, String> infoDictionary = [];
        infoDictionary.Add("FileName", _dumper.Info.Name!);
        infoDictionary.Add("FilePath", _dumper.Info.Path!);
        infoDictionary.Add("Target CPU", _dumper.Info.CpuArchitecture!);
        infoDictionary.Add("Max. WORD", $"{_dumper.Info.CpuWordLength} BIT");
        infoDictionary.Add("Target OS ", _dumper.Info.OperatingSystem!);
        infoDictionary.Add("Target OS ver.", _dumper.Info.OperatingSystemVersion!);
        infoDictionary.Add("FileType", FileTypeToString((FileType)_dumper.GetBinaryTypeId()));
        infoDictionary.Add("ExtType ", FileTypeToString((FileType)_dumper.GetExtensionTypeId()));
        InfoDictionary = infoDictionary;
    }

    private static String FileTypeToString(FileType type)
    {
        return type.ToString();
    }
}