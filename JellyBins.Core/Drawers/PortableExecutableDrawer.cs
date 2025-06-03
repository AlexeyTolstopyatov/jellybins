using System.Data;
using System.Threading.Channels;
using JellyBins.Abstractions;
using JellyBins.PortableExecutable.Headers;
using JellyBins.PortableExecutable.Models;

namespace JellyBins.Core.Drawers;

public class PortableExecutableDrawer : IDrawer
{
    private PeFileDumper _dumper;
    public PortableExecutableDrawer(PeFileDumper dumper)
    {
        _dumper = dumper;
        MakeInfo();
        MakeHeadersTables();
        MakeSectionsTables();
        MakeExports();
        MakeImports();
        ExtractCharacteristics();
        MakeExternToolChain();
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
            _dumper.FileHeaderDump.Name,
            _dumper.FileHeaderDump.Address,
            _dumper.FileHeaderDump.Size
        );
        meta.Rows.Add(
            _dumper.Machine64Bit
                ? _dumper.OptionalHeaderDump.Name
                : _dumper.OptionalHeader32Dump.Name,
            _dumper.Machine64Bit
                ? _dumper.OptionalHeaderDump.Address
                : _dumper.OptionalHeader32Dump.Address,
            _dumper.Machine64Bit
                ? _dumper.OptionalHeaderDump.Size
                : _dumper.OptionalHeader32Dump.Size
        );
        DataTable dosHeader = MakeDosHeader();
        DataTable fileHeader = MakeFileHeader();
        DataTable optionalHeader = _dumper.Machine64Bit
            ? MakeOptionalHeader()
            : MakeOptional32Header();

        HeadersTables = [meta, dosHeader, fileHeader, optionalHeader];
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

    private DataTable MakeFileHeader()
    {
        PeFileHeader mz = _dumper.FileHeaderDump.Segmentation;
        DataTable table = new();
        table.Columns.AddRange([new DataColumn("Segment"), new DataColumn("Value")]);

        table.Rows.Add(nameof(mz.Machine), mz.Machine.ToString("X"));
        table.Rows.Add(nameof(mz.NumberOfSections), mz.NumberOfSections.ToString("X"));
        table.Rows.Add(nameof(mz.TimeDateStamp), mz.TimeDateStamp.ToString("X"));
        table.Rows.Add(nameof(mz.PointerToSymbolTable), mz.PointerToSymbolTable.ToString("X"));
        table.Rows.Add(nameof(mz.NumberOfSymbols), mz.NumberOfSymbols.ToString("X"));
        table.Rows.Add(nameof(mz.SizeOfOptionalHeader), mz.SizeOfOptionalHeader.ToString("X"));
        table.Rows.Add(nameof(mz.Characteristics), mz.Characteristics.ToString("X"));
        
        return table;
    }

    private DataTable MakeOptionalHeader()
    {
        PeOptionalHeader optional = _dumper.OptionalHeaderDump.Segmentation;
        DataTable table = new();
        table.Columns.AddRange([new DataColumn("Segment"), new DataColumn("Value")]);

        table.Rows.Add(nameof(optional.Magic), optional.Magic.ToString("X"));
        table.Rows.Add(nameof(optional.MajorLinkerVersion), optional.MajorLinkerVersion.ToString("X"));
        table.Rows.Add(nameof(optional.MinorLinkerVersion), optional.MinorLinkerVersion.ToString("X"));
        table.Rows.Add(nameof(optional.SizeOfCode), optional.SizeOfCode.ToString("X"));
        table.Rows.Add(nameof(optional.SizeOfInitializedData), optional.SizeOfInitializedData.ToString("X"));
        table.Rows.Add(nameof(optional.SizeOfUninitializedData), optional.SizeOfUninitializedData.ToString("X"));
        table.Rows.Add(nameof(optional.AddressOfEntryPoint), optional.AddressOfEntryPoint.ToString("X"));
        table.Rows.Add(nameof(optional.BaseOfCode), optional.BaseOfCode.ToString("X"));
        table.Rows.Add(nameof(optional.BaseOfData), optional.BaseOfData.ToString("X"));
        table.Rows.Add(nameof(optional.ImageBase), optional.ImageBase.ToString("X"));
        table.Rows.Add(nameof(optional.SectionAlignment), optional.SectionAlignment.ToString("X"));
        table.Rows.Add(nameof(optional.FileAlignment), optional.FileAlignment.ToString("X"));
        table.Rows.Add(nameof(optional.MajorOperatingSystemVersion), optional.MajorOperatingSystemVersion.ToString("X"));
        table.Rows.Add(nameof(optional.MinorOperatingSystemVersion), optional.MinorOperatingSystemVersion.ToString("X"));
        table.Rows.Add(nameof(optional.MajorSubsystemVersion), optional.MajorSubsystemVersion.ToString("X"));
        table.Rows.Add(nameof(optional.MinorSubsystemVersion), optional.MinorSubsystemVersion.ToString("X"));
        table.Rows.Add(nameof(optional.MajorImageVersion), optional.MajorImageVersion.ToString("X"));
        table.Rows.Add(nameof(optional.MinorImageVersion), optional.MinorImageVersion.ToString("X"));
        table.Rows.Add(nameof(optional.Win32VersionValue), optional.Win32VersionValue.ToString("X"));
        table.Rows.Add(nameof(optional.SizeOfImage), optional.SizeOfImage.ToString("X"));
        table.Rows.Add(nameof(optional.SizeOfHeaders), optional.SizeOfHeaders.ToString("X"));
        table.Rows.Add(nameof(optional.CheckSum), optional.CheckSum.ToString("X"));
        table.Rows.Add(nameof(optional.Subsystem), optional.Subsystem.ToString("X"));
        table.Rows.Add(nameof(optional.DllCharacteristics), optional.DllCharacteristics.ToString("X"));
        table.Rows.Add(nameof(optional.SizeOfStackReserve), optional.SizeOfStackReserve.ToString("X"));
        table.Rows.Add(nameof(optional.SizeOfStackCommit), optional.SizeOfStackCommit.ToString("X"));
        table.Rows.Add(nameof(optional.SizeOfHeapReserve), optional.SizeOfHeapReserve.ToString("X"));
        table.Rows.Add(nameof(optional.SizeOfHeapCommit), optional.SizeOfHeapCommit.ToString("X"));
        table.Rows.Add(nameof(optional.LoaderFlags), optional.LoaderFlags.ToString("X"));
        table.Rows.Add(nameof(optional.NumberOfRvaAndSizes), optional.NumberOfRvaAndSizes.ToString("X"));
        
        return table;
    }

    private DataTable MakeOptional32Header()
    {
        PeOptionalHeader32 optional = _dumper.OptionalHeader32Dump.Segmentation;
        DataTable table = new();
        table.Columns.AddRange([new DataColumn("Segment"), new DataColumn("Value")]);

        table.Rows.Add(nameof(optional.Magic), optional.Magic.ToString("X"));
        table.Rows.Add(nameof(optional.MajorLinkerVersion), optional.MajorLinkerVersion.ToString("X"));
        table.Rows.Add(nameof(optional.MinorLinkerVersion), optional.MinorLinkerVersion.ToString("X"));
        table.Rows.Add(nameof(optional.SizeOfCode), optional.SizeOfCode.ToString("X"));
        table.Rows.Add(nameof(optional.SizeOfInitializedData), optional.SizeOfInitializedData.ToString("X"));
        table.Rows.Add(nameof(optional.SizeOfUninitializedData), optional.SizeOfUninitializedData.ToString("X"));
        table.Rows.Add(nameof(optional.AddressOfEntryPoint), optional.AddressOfEntryPoint.ToString("X"));
        table.Rows.Add(nameof(optional.BaseOfCode), optional.BaseOfCode.ToString("X"));
        table.Rows.Add(nameof(optional.BaseOfData), optional.BaseOfData.ToString("X"));
        table.Rows.Add(nameof(optional.ImageBase), optional.ImageBase.ToString("X"));
        table.Rows.Add(nameof(optional.SectionAlignment), optional.SectionAlignment.ToString("X"));
        table.Rows.Add(nameof(optional.FileAlignment), optional.FileAlignment.ToString("X"));
        table.Rows.Add(nameof(optional.MajorOperatingSystemVersion), optional.MajorOperatingSystemVersion.ToString("X"));
        table.Rows.Add(nameof(optional.MinorOperatingSystemVersion), optional.MinorOperatingSystemVersion.ToString("X"));
        table.Rows.Add(nameof(optional.MajorSubsystemVersion), optional.MajorSubsystemVersion.ToString("X"));
        table.Rows.Add(nameof(optional.MinorSubsystemVersion), optional.MinorSubsystemVersion.ToString("X"));
        table.Rows.Add(nameof(optional.MajorImageVersion), optional.MajorImageVersion.ToString("X"));
        table.Rows.Add(nameof(optional.MinorImageVersion), optional.MinorImageVersion.ToString("X"));
        table.Rows.Add(nameof(optional.Win32VersionValue), optional.Win32VersionValue.ToString("X"));
        table.Rows.Add(nameof(optional.SizeOfImage), optional.SizeOfImage.ToString("X"));
        table.Rows.Add(nameof(optional.SizeOfHeaders), optional.SizeOfHeaders.ToString("X"));
        table.Rows.Add(nameof(optional.CheckSum), optional.CheckSum.ToString("X"));
        table.Rows.Add(nameof(optional.Subsystem), optional.Subsystem.ToString("X"));
        table.Rows.Add(nameof(optional.DllCharacteristics), optional.DllCharacteristics.ToString("X"));
        table.Rows.Add(nameof(optional.SizeOfStackReserve), optional.SizeOfStackReserve.ToString("X"));
        table.Rows.Add(nameof(optional.SizeOfStackCommit), optional.SizeOfStackCommit.ToString("X"));
        table.Rows.Add(nameof(optional.SizeOfHeapReserve), optional.SizeOfHeapReserve.ToString("X"));
        table.Rows.Add(nameof(optional.SizeOfHeapCommit), optional.SizeOfHeapCommit.ToString("X"));
        table.Rows.Add(nameof(optional.LoaderFlags), optional.LoaderFlags.ToString("X"));
        table.Rows.Add(nameof(optional.NumberOfRvaAndSizes), optional.NumberOfRvaAndSizes.ToString("X"));
        
        return table;
    }
    public void MakeSectionsTables()
    {
        DataTable meta = new()
        {
            Columns = { "Name", "Address", "Size" }
        };
        DataTable dirs = new()
        {
            Columns =
            {
                "VirtualAddress",
                "Size",
            }
        };
        DataTable sections = new()
        {
            Columns = 
            {
                "Name", 
                "VirtualAddress",
                "VirtualSize",
                "SizeOfRawData",
                "PointerToRawData",
                "PointerToRelocs",
                "PointerToLine#",
                "#Relocs",
                "#LineNumbers",
                "Characteristics" 
            }
        };
        
        foreach (PeDirectoryDump dump in _dumper.DirectoryDumps)
        {
            meta.Rows.Add(
                dump.Name,
                dump.Address,
                dump.Size
            );
            dirs.Rows.Add(
                dump.Segmentation.VirtualAddress.ToString("X"),
                dump.Segmentation.Size.ToString("X")
            );
        }
        foreach (PeSectionDump dump in _dumper.SectionDumps)
        {
            meta.Rows.Add(
                dump.Name,
                dump.Address,
                dump.Size
            );
            sections.Rows.Add(
                new String(dump.Segmentation.Name.Where(x => x != '\0').ToArray()),
                dump.Segmentation.VirtualAddress.ToString("X"),
                dump.Segmentation.VirtualSize.ToString("X"),
                dump.Segmentation.SizeOfRawData.ToString("X"),
                dump.Segmentation.PointerToRawData.ToString("X"),
                dump.Segmentation.PointerToRelocations.ToString("X"),
                dump.Segmentation.PointerToLinenumbers.ToString("X"),
                dump.Segmentation.NumberOfRelocations.ToString("X"),
                dump.Segmentation.NumberOfLinenumbers.ToString("X"),
                dump.Segmentation.Characteristics.ToString("X")
            );
        }

        SectionTables = [meta, dirs, sections];
    }

    public void MakeInfo()
    {
        Dictionary<String, String> infoDictionary = [];
        infoDictionary.Add("FileName", _dumper.Info.Name!);
        infoDictionary.Add("FilePath", _dumper.Info.Path!);
        infoDictionary.Add("BirthDay", _dumper.Info.DateTimeStamp!);
        infoDictionary.Add("Target CPU", _dumper.Info.CpuArchitecture!);
        infoDictionary.Add("Max. WORD", $"{_dumper.Info.CpuWordLength} BIT");
        infoDictionary.Add("Target OS ", _dumper.Info.OperatingSystem!);
        infoDictionary.Add("Target OS ver.", _dumper.Info.OperatingSystemVersion!);
        infoDictionary.Add("Minimum OS ver.", _dumper.Info.MinimumSystemVersion!);
        infoDictionary.Add("FileType", FileTypeToString((FileType)_dumper.GetBinaryTypeId()));
        infoDictionary.Add("ExtType ", FileTypeToString((FileType)_dumper.GetExtensionTypeId()));
        
        InfoDictionary = infoDictionary;
    }

    public String[] Characteristics { get; private set; } = [];
    public String[] ExternToolChain { get; private set; } = [];
    public String[][] SectionsCharacteristics { get; private set; }
    public String[][] HeadersCharacteristics { get; private set; }

    private void MakeExternToolChain()
    {
        ExternToolChain = new ModuleProcessor(_dumper)
            .TryToKnowUsedModules()
            //.Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();
    }
    
    private void ExtractCharacteristics()
    {
        List<String> all = [];
        List<String[]> headers = [];
        if (_dumper.Machine64Bit)
        {
            all.AddRange(_dumper.FileHeaderDump.Characteristics!);
            all.AddRange(_dumper.OptionalHeaderDump.Characteristics!);
            headers.Add(_dumper.FileHeaderDump.Characteristics!);
            headers.Add(_dumper.OptionalHeaderDump.Characteristics!);
        }
        else
        {
            all.AddRange(_dumper.FileHeaderDump.Characteristics!);
            all.AddRange(_dumper.OptionalHeader32Dump.Characteristics!);
            headers.Add(_dumper.FileHeaderDump.Characteristics!);
            headers.Add(_dumper.OptionalHeaderDump.Characteristics!);
        }

        Characteristics = all.ToArray();
        
        
        List<String[]> sections = [];
        foreach (var dump in _dumper.DirectoryDumps)
        {
            sections.Add(dump.Characteristics!);
        }

        foreach (PeSectionDump dump in _dumper.SectionDumps)
        {
            sections.Add(dump.Characteristics!);
        }

        SectionsCharacteristics = sections.ToArray();
        HeadersCharacteristics = headers.ToArray();
        
    }

    private void MakeExports()
    {
        DataTable meta = new()
        {
            Columns = { "Name", "Address", "Size" }
        };
        meta.Rows.Add(
            _dumper.ExportsDump.Name,
            _dumper.ExportsDump.Address,
            _dumper.ExportsDump.Size
        );
        
        DataTable exports = new()
        {
            Columns = { "Name", "Ordinal", "Address" }
        };

        if (_dumper.ExportsDump.Segmentation != null)
            foreach (ExportFunction function in _dumper.ExportsDump.Segmentation)
            {
                exports.Rows.Add(
                    function.Name,
                    "@" + function.Ordinal,
                    function.Address.ToString("X")
                );
            }

        ExportsTables = [meta, exports];
    }

    private void MakeImports()
    {
        DataTable meta = new()
        {
            Columns = { "Name", "Address", "Size" }
        };
        meta.Rows.Add(
            _dumper.ImportsDump.Name,
            _dumper.ImportsDump.Address,
            _dumper.ImportsDump.Size
        );

        DataTable imports = new()
        {
            Columns =
            {
                "From",
                "Name",
                "Ordinal",
                "Hint",
                "Address"
            }
        };

        foreach (ImportDll import in _dumper.ImportsDump.FoundImports)
        {
            foreach (ImportedFunction function in import.Functions)
            {
                imports.Rows.Add(
                    import.DllName,
                    function.Name,
                    "@" + function.Ordinal,
                    function.Hint.ToString("X"),
                    function.Address.ToString("X")
                );
            }
        }

        ImportsTables = [meta, imports];
    }
    private static String FileTypeToString(FileType type)
    {
        return type.ToString();
    }
}