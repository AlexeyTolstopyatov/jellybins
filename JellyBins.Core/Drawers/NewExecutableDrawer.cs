using System.Data;
using JellyBins.Abstractions;
using JellyBins.NewExecutable.Headers;
using JellyBins.NewExecutable.Models;

namespace JellyBins.Core.Drawers;

public class NewExecutableDrawer : IDrawer
{
    private NeFileDumper _dumper;
    
    public NewExecutableDrawer(NeFileDumper dumper)
    {
        _dumper = dumper;
        MakeInfo();
        MakeHeadersTables();
        MakeSectionsTables();
        MakeExports();
        MakeImports();
    }

    public DataTable[] HeadersTables { get; private set; } = [];
    public DataTable[] SectionTables { get; private set; } = [];
    public DataTable[] ImportsTables { get; private set; } = [];
    public DataTable[] ExportsTables { get; private set; } = [];
    public Dictionary<String, String> InfoDictionary { get; private set; } = [];
    
    public void MakeHeadersTables()
    {
        // MZ header \/ NE header.
        DataTable dosHeader = MakeDosHeader();
        DataTable windowsHeader = MakeWindowsHeader();
        DataTable meta = new();
        
        meta.Columns.AddRange([
            new DataColumn("Name"),
            new DataColumn("Address"),
            new DataColumn("Size")
        ]);
        meta.Rows.Add(
            _dumper.MzHeaderDump.Name,
            _dumper.MzHeaderDump.Address,
            _dumper.MzHeaderDump.Size
        );
        meta.Rows.Add(
            _dumper.NeHeaderDump.Name,
            _dumper.NeHeaderDump.Address!.Value.ToString("X"),
            _dumper.NeHeaderDump.Size!.Value.ToString("X")
        );
        
        HeadersTables = [
            meta,
            dosHeader, 
            windowsHeader
        ];
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
    
    private DataTable MakeWindowsHeader()
    {
        NeHeader ne = _dumper.NeHeaderDump.Segmentation;
        DataTable table = new();
        
        table.Columns.AddRange([new DataColumn("Segment"), new DataColumn("Value")]);
        table.Rows.Add(nameof(ne.magic), ne.magic.ToString("X"));
        table.Rows.Add(nameof(ne.ver), ne.ver.ToString("X"));
        table.Rows.Add(nameof(ne.rev), ne.rev.ToString("X"));
        table.Rows.Add(nameof(ne.enttab), ne.enttab.ToString("X"));
        table.Rows.Add(nameof(ne.cbenttab), ne.cbenttab.ToString("X"));
        table.Rows.Add(nameof(ne.crc), ne.crc.ToString("X"));
        table.Rows.Add(nameof(ne.pflags), ne.pflags.ToString("X"));
        table.Rows.Add(nameof(ne.aflags), ne.aflags.ToString("X"));
        table.Rows.Add(nameof(ne.autodata), ne.autodata.ToString("X"));
        table.Rows.Add(nameof(ne.heap), ne.heap.ToString("X"));
        table.Rows.Add(nameof(ne.stack), ne.stack.ToString("X"));
        table.Rows.Add(nameof(ne.csip), ne.csip.ToString("X"));
        table.Rows.Add(nameof(ne.sssp), ne.sssp.ToString("X"));
        table.Rows.Add(nameof(ne.cseg), ne.cseg.ToString("X"));
        table.Rows.Add(nameof(ne.cmod), ne.cmod.ToString("X"));
        table.Rows.Add(nameof(ne.segtab), ne.segtab.ToString("X"));
        table.Rows.Add(nameof(ne.rsrctab), ne.rsrctab.ToString("X"));
        table.Rows.Add(nameof(ne.restab), ne.restab.ToString("X"));
        table.Rows.Add(nameof(ne.cmovent), ne.cmovent.ToString("X"));
        table.Rows.Add(nameof(ne.align), ne.align.ToString("X"));
        table.Rows.Add(nameof(ne.os), ne.os.ToString("X"));
        table.Rows.Add(nameof(ne.flagsothers), ne.flagsothers.ToString("X"));
        table.Rows.Add(nameof(ne.pretthunks), ne.pretthunks.ToString("X"));
        table.Rows.Add(nameof(ne.psegrefbytes), ne.psegrefbytes.ToString("X"));
        table.Rows.Add(nameof(ne.swaparea), ne.swaparea.ToString("X"));
        table.Rows.Add(nameof(ne.minor), ne.minor.ToString("X"));
        table.Rows.Add(nameof(ne.magic), ne.major.ToString("X"));

        return table;
    }
    public void MakeSectionsTables()
    {
        DataTable meta = new();
        DataTable segs = new();
        
        meta.Columns.AddRange([
            new DataColumn("Name"),
            new DataColumn("Address"),
            new DataColumn("Size")
        ]);
        segs.Columns.AddRange([
            new DataColumn("Type"), 
            new DataColumn("#Segment"), 
            new DataColumn("FileOffset"),
            new DataColumn("FileLength"),
            new DataColumn("Flags"),
            new DataColumn("MinAllocation")
        ]);

        foreach (NeSegmentDump segmentDump in _dumper.SegmentsTableDump)
        {
            NeSegmentInfo seg = segmentDump.Segmentation;
            meta.Rows.Add(
                segmentDump.Name,
                segmentDump.Address!.Value.ToString("X"),
                segmentDump.Size!.Value.ToString("X")
            );
            segs.Rows.Add(
                seg.Type,
                seg.SegmentNumber.ToString("X"),
                seg.FileOffset.ToString("X"),
                seg.FileLength.ToString("X"),
                seg.Flags.ToString("X"),
                seg.MinAllocation.ToString("X")
            );
        }

        SectionTables = [meta, segs];
    }

    public void MakeInfo()
    {
        Dictionary<String, String> infoDictionary = [];
        infoDictionary.Add("FileName", _dumper.Info!.Name!);
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

    public void MakeImports()
    {
        DataTable meta = new();
        DataTable imps = new();
        
        meta.Columns.AddRange([
            new DataColumn("Name"),
            new DataColumn("Address"),
            new DataColumn("Size")
        ]);
        imps.Columns.AddRange([
            new DataColumn("#Name"),
            new DataColumn("Name")
        ]);
        foreach (NeImportDump neImportDump in _dumper.ImportsTableDump)
        {
            meta.Rows.Add(
                neImportDump.Name,
                neImportDump.Address,
                neImportDump.Size
            );
            imps.Rows.Add(
                neImportDump.Segmentation.NameLength,
                neImportDump.Segmentation.Name
            );
        }

        ImportsTables = [meta, imps];
    }

    private void MakeExports()
    {
        DataTable exportsTable = new();

        exportsTable.Columns.AddRange([
            new DataColumn("#"),
            new DataColumn("Name"),
            new DataColumn("Ordinal")
        ]);
        
        foreach (NeExportDump neExportDump in _dumper.ExportsTableDump)
        {
            exportsTable.Rows.Add(
                neExportDump.Segmentation.Count,
                neExportDump.Segmentation.Name,
                "@" + neExportDump.Segmentation.Ordinal
            );
        }

        ExportsTables = [exportsTable];
    }
    
    private static String FileTypeToString(FileType type)
    {
        return type.ToString();
    }
}