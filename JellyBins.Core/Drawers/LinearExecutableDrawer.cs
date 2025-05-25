using System.Data;
using JellyBins.Abstractions;
using JellyBins.LinearExecutable.Headers;
using JellyBins.LinearExecutable.Models;

namespace JellyBins.Core.Drawers;

public class LinearExecutableDrawer : IDrawer
{
    private readonly LeFileDumper _dumper;
    public LinearExecutableDrawer(LeFileDumper dumper)
    {
        _dumper = dumper;
        MakeInfo();
        MakeHeadersTables();
        MakeSectionsTables();
        MakeExportTables();
        //MakeImportTables();
    }
    
    public DataTable[] HeadersTables { get; private set; } = [];
    public DataTable[] SectionTables { get; private set; } = [];
    public DataTable[] ImportsTables { get; private set; } = [];
    public DataTable[] ExportsTables { get; private set; } = [];
    public Dictionary<String, String> InfoDictionary { get; private set; } = [];
    
    public void MakeHeadersTables()
    {
        DataTable meta = new()
        {
            Columns = { "Name", "Address", "Size" }
        };
        meta.Rows.Add(
            _dumper.MzHeaderDump.Name,
            _dumper.MzHeaderDump.Address,
            _dumper.MzHeaderDump.Size
        );
        meta.Rows.Add(
            _dumper.LeHeaderDump.Name,
            _dumper.LeHeaderDump.Address,
            _dumper.LeHeaderDump.Size
        );
        DataTable dosHeader = MakeDosHeader();
        DataTable os2WinHeader = MakeOs2WindowsHeader();

        HeadersTables = [meta, dosHeader, os2WinHeader];
    }
    private DataTable MakeDosHeader()
    {
        MzHeader mz = _dumper.MzHeaderDump.Segmentation;
        DataTable table = new();
        table.Columns.AddRange([new DataColumn("Segment"), new DataColumn("Value")]);

        table.Rows.Add(nameof(mz.e_sign), mz.e_sign.ToString("X"));
        table.Rows.Add(nameof(mz.e_lastb), mz.e_lastb.ToString("X"));
        table.Rows.Add(nameof(mz.e_fbl), mz.e_fbl.ToString("X"));
        table.Rows.Add(nameof(mz.e_relc), mz.e_relc.ToString("X"));
        table.Rows.Add(nameof(mz.e_pars), mz.e_pars.ToString("X"));
        table.Rows.Add(nameof(mz.e_minep), mz.e_minep.ToString("X"));
        table.Rows.Add(nameof(mz.e_maxep), mz.e_maxep.ToString("X"));
        table.Rows.Add(nameof(mz.ss), mz.ss.ToString("X"));
        table.Rows.Add(nameof(mz.sp), mz.sp.ToString("X"));
        table.Rows.Add(nameof(mz.e_check), mz.e_check.ToString("X"));
        table.Rows.Add(nameof(mz.ip), mz.ip.ToString("X"));
        table.Rows.Add(nameof(mz.cs), mz.cs.ToString("X"));
        table.Rows.Add(nameof(mz.e_reltableoff), mz.e_reltableoff.ToString("X"));
        table.Rows.Add(nameof(mz.e_overnum), mz.e_overnum.ToString("X"));
        table.Rows.Add(nameof(mz.e_oemid), mz.e_oemid.ToString("X"));
        table.Rows.Add(nameof(mz.e_oeminfo), mz.e_oeminfo.ToString("X"));
        table.Rows.Add(nameof(mz.e_lfanew), mz.e_lfanew.ToString("X"));
        
        return table;
    }
    private DataTable MakeOs2WindowsHeader()
    {
        LeHeader linear = _dumper.LeHeaderDump.Segmentation;
        
        DataTable table = new();
        table.Columns.AddRange([new DataColumn("Segment"), new DataColumn("Value")]);

        table.Rows.Add(nameof(linear.SignatureWord), linear.SignatureWord.ToString("X"));
        table.Rows.Add(nameof(linear.ByteOrder), linear.ByteOrder.ToString("X"));
        table.Rows.Add(nameof(linear.WordOrder), linear.WordOrder.ToString("X"));
        table.Rows.Add(nameof(linear.ExecutableFormatLevel), linear.ExecutableFormatLevel.ToString("X"));
        table.Rows.Add(nameof(linear.CPUType), linear.CPUType.ToString("X"));
        table.Rows.Add(nameof(linear.OSType), linear.OSType.ToString("X"));
        table.Rows.Add(nameof(linear.ModuleVersionMajor), linear.ModuleVersionMajor.ToString("X"));
        table.Rows.Add(nameof(linear.ModuleVersionMinor), linear.ModuleVersionMinor.ToString("X"));
        table.Rows.Add(nameof(linear.ModuleTypeFlags), linear.ModuleTypeFlags.ToString("X"));
        table.Rows.Add(nameof(linear.NumberOfMemoryPages), linear.NumberOfMemoryPages.ToString("X"));
        table.Rows.Add(nameof(linear.InitialObjectCSNumber), linear.InitialObjectCSNumber.ToString("X"));
        table.Rows.Add(nameof(linear.InitialEIP), linear.InitialEIP.ToString("X"));
        table.Rows.Add(nameof(linear.InitialSSObjectNumber), linear.InitialSSObjectNumber.ToString("X"));
        table.Rows.Add(nameof(linear.InitialESP), linear.InitialESP.ToString("X"));
        table.Rows.Add(nameof(linear.MemoryPageSize), linear.MemoryPageSize.ToString("X"));
        table.Rows.Add(nameof(linear.BytesOnLastPage), linear.BytesOnLastPage.ToString("X"));
        table.Rows.Add(nameof(linear.FixupSectionSize), linear.FixupSectionSize.ToString("X"));
        table.Rows.Add(nameof(linear.FixupSectionChecksum), linear.FixupSectionChecksum.ToString("X"));
        table.Rows.Add(nameof(linear.ObjectTableOffset), linear.ObjectTableOffset.ToString("X"));
        table.Rows.Add(nameof(linear.ObjectTableEntries), linear.ObjectTableEntries.ToString("X"));
        table.Rows.Add(nameof(linear.ObjectPageMapOffset), linear.ObjectPageMapOffset.ToString("X"));
        table.Rows.Add(nameof(linear.ObjectIterateDataMapOffset), linear.ObjectIterateDataMapOffset.ToString("X"));
        table.Rows.Add(nameof(linear.ResourceTableOffset), linear.ResourceTableOffset.ToString("X"));
        table.Rows.Add(nameof(linear.ResourceTableEntries), linear.ResourceTableEntries.ToString("X"));
        table.Rows.Add(nameof(linear.ResidentNamesTableOffset), linear.ResidentNamesTableOffset.ToString("X"));
        table.Rows.Add(nameof(linear.EntryTableOffset), linear.EntryTableOffset.ToString("X"));
        table.Rows.Add(nameof(linear.ModuleDirectivesTableOffset), linear.ModuleDirectivesTableOffset.ToString("X"));
        table.Rows.Add(nameof(linear.ModuleDirectivesTableEntries), linear.ModuleDirectivesTableEntries.ToString("X"));
        table.Rows.Add(nameof(linear.FixupPageTableOffset), linear.FixupPageTableOffset.ToString("X"));
        table.Rows.Add(nameof(linear.FixupRecordTableOffset), linear.FixupRecordTableOffset.ToString("X"));
        table.Rows.Add(nameof(linear.ImportedModulesNameTableOffset), linear.ImportedModulesNameTableOffset.ToString("X"));
        table.Rows.Add(nameof(linear.ImportedModulesCount), linear.ImportedModulesCount.ToString("X"));
        table.Rows.Add(nameof(linear.ImportedProcedureNameTableOffset), linear.ImportedProcedureNameTableOffset.ToString("X"));
        table.Rows.Add(nameof(linear.PerPageChecksumTableOffset), linear.PerPageChecksumTableOffset.ToString("X"));
        table.Rows.Add(nameof(linear.DataPagesOffsetFromTopOfFile), linear.DataPagesOffsetFromTopOfFile.ToString("X"));
        table.Rows.Add(nameof(linear.PreloadPagesCount), linear.PreloadPagesCount.ToString("X"));
        table.Rows.Add(nameof(linear.NonResidentNamesTableOffsetFromTopOfFile), linear.NonResidentNamesTableOffsetFromTopOfFile.ToString("X"));
        table.Rows.Add(nameof(linear.NonResidentNamesTableLength), linear.NonResidentNamesTableLength.ToString("X"));
        table.Rows.Add(nameof(linear.NonResidentNamesTableChecksum), linear.NonResidentNamesTableChecksum.ToString("X"));
        table.Rows.Add(nameof(linear.AutomaticDataObject), linear.AutomaticDataObject.ToString("X"));
        table.Rows.Add(nameof(linear.DebugInformationOffset), linear.DebugInformationOffset.ToString("X"));
        table.Rows.Add(nameof(linear.DebugInformationLength), linear.DebugInformationLength.ToString("X"));
        table.Rows.Add(nameof(linear.PreloadInstancePagesNumber), linear.PreloadInstancePagesNumber.ToString("X"));
        table.Rows.Add(nameof(linear.DemandInstancePagesNumber), linear.DemandInstancePagesNumber.ToString("X"));
        table.Rows.Add(nameof(linear.ExtraHeapAlloc), linear.ExtraHeapAlloc.ToString("X"));
        
        return table;
    }
    public void MakeSectionsTables()
    {
        DataTable meta = new() { Columns = { "Name", "Address", "Size" } };
        DataTable dirs = new() { Columns = { "VirtualAddress", "#PageTable", "#PageTableEntries", "BaseRelocAddress", "ObjectFlags" }};
        foreach (LeObjectDump objectDump in _dumper.ObjectsTableDump)
        {
            LeObjectHeader obj = objectDump.Segmentation;
            meta.Rows.Add(
                objectDump.Name,
                objectDump.Address!.Value.ToString("X"),
                objectDump.Size!.Value.ToString("X")
            );
            dirs.Rows.Add(
                obj.VirtualSize,
                obj.PageTableIndex.ToString("X"),
                obj.PageTableEntries.ToString("X"),
                obj.RelocBaseAddr.ToString("X"),
                obj.ObjectFlags.ToString("X")
            );
        }

        SectionTables = [meta, dirs];
    }

    private void MakeExportTables()
    {
        DataTable meta = new() { Columns = { "Name", "Address", "Size" } };
        meta.Rows.Add(
            _dumper.ExportsDump.Name,
            _dumper.ExportsDump.Address,
            _dumper.ExportsDump.Size
        );
        DataTable exportsTable = new();

        exportsTable.Columns.AddRange([
            new DataColumn("Name"),
            new DataColumn("Ordinal"),
            new DataColumn("ObjectIndex"),
            new DataColumn("Offset")
        ]);
        
        foreach (LeExportEntry leExportDump in _dumper.ExportsDump.Segmentation!)
        {
            exportsTable.Rows.Add(
                leExportDump.Name,
                "@" + leExportDump.Ordinal,
                leExportDump.ObjectIndex,
                leExportDump.Offset
            );
        }

        ExportsTables = [exportsTable];
    }

    private void MakeImportTables()
    {
        DataTable meta = new() { Columns = { "Name", "Address", "Size" } };
        meta.Rows.Add(
            _dumper.ImportsDump.Name,
            _dumper.ImportsDump.Address,
            _dumper.ImportsDump.Size
        );
        DataTable imports = new(){ Columns = { "Module", "Procedure", "ByOrdinal", "Ordinal" } };
        
        _dumper.ImportsDump.Segmentation ??= [];
        
        foreach (LeImportEntry importEntry in _dumper.ImportsDump.Segmentation!)
        {
            imports.Rows.Add(
                importEntry.ModuleName,
                importEntry.ProcedureName,
                importEntry.IsByOrdinal,
                "@" + importEntry.Ordinal
            );
        }

        ImportsTables = [meta, imports];
    }
    public void MakeInfo()
    {
        Dictionary<String, String> infoDictionary = [];
        infoDictionary.Add("FileName", _dumper.Info.Name!);
        infoDictionary.Add("FilePath", _dumper.Info.Path!);
        infoDictionary.Add("FileTitle", _dumper.Info.ProjectDescription!);
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