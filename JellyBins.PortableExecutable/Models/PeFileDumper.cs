using System.Globalization;
using System.Runtime.InteropServices;
using JellyBins.Abstractions;
using JellyBins.PortableExecutable.Headers;
using static System.TimeZone;

namespace JellyBins.PortableExecutable.Models;

public class PeFileDumper(String path) : IFileDumper
{
    public FileSegmentationType SegmentationType { get; } = FileSegmentationType.PortableExecutable;
    public PeFileInfo Info { get; private set; } = new()
    {
        Name = new FileInfo(path).Name,
        Path = path,
        CpuWordLength = 32,
    };
    public MzHeaderDump MzHeaderDump { get; private set; } = new();
    public PeRichHeaderDump RichHeaderDump { get; private set; } = new();
    public PeFileHeaderDump FileHeaderDump { get; private set; } = new();
    public PeOptionalHeaderDump OptionalHeaderDump { get; private set; } = new();
    public PeOptionalHeader32Dump OptionalHeader32Dump { get; private set; } = new();
    public PeOptionalHeaderRomDump OptionalHeaderRomDump { get; private set; } = new();
    public PeSectionDump[] SectionDumps { get; private set; } = [];
    public PeImportsDump ImportsDump { get; private set; } = new();
    public PeExportsDump ExportsDump { get; private set; } = new();
    public PeDirectoryDump[] DirectoryDumps { get; private set; } = [];
    public PeCor20HeaderDump Cor20HeaderDump { get; private set; } = new();
    public PeVb5HeaderDump Vb5HeaderDump { get; private set; } = new();
    
    private UInt16 _extensionTypeId = 0;
    private UInt16 _binaryTypeId = 0;
    private UInt32 _numberOfRvaAndSizes;
    private UInt32 _numberOfSections;
    public Boolean Machine64Bit { get; private set; }

    public Boolean HasVb4Data { get; private set; }
    public Boolean HasOldVb4Data { get; private set; }
    public Boolean HasVb5Data { get; private set; }
    public Boolean HasCorData { get; private set; }
    
    /// <summary>
    /// Makes full dump of required binary.
    /// Use all public fields of <see cref="PeFileDumper"/> instance
    /// after this method.
    /// </summary>
    public void Dump()
    {
        using FileStream stream = new(Info.Path!, FileMode.Open, FileAccess.Read);
        using BinaryReader reader = new(stream);
        
        // IMAGE_DOS_HEADER construct
        MzHeaderDump.Segmentation = Fill<MzHeader>(reader);
        MzHeaderDump.Address = 0;
        MzHeaderDump.Size = SizeOf(MzHeaderDump.Segmentation);
        MzHeaderDump.Name = "MZ Header (WinAPI: IMAGE_DOS_HEADER)";

        // PE Header/1 construct (sources: windows.h)
        stream.Seek(MzHeaderDump.Segmentation.e_lfanew, SeekOrigin.Begin);
        reader.ReadUInt32(); // skip signature.
        FileHeaderDump.Segmentation = Fill<PeFileHeader>(reader);
        FileHeaderDump.Address = MzHeaderDump.Segmentation.e_lfanew;
        FileHeaderDump.Size = SizeOf(FileHeaderDump.Segmentation);
        FileHeaderDump.Name = "PE File Header (WinAPI: IMAGE_FILE_HEADER)";
        
        _numberOfSections = FileHeaderDump.Segmentation.NumberOfSections;
        
        // PE header/2 construct
        if (!IsMachine64Bit(FileHeaderDump.Segmentation.Characteristics))
        {
            OptionalHeader32Dump.Segmentation = Fill<PeOptionalHeader32>(reader);
            OptionalHeader32Dump.Address = (UInt64)stream.Position;
            OptionalHeader32Dump.Size = SizeOf(OptionalHeader32Dump.Segmentation);
            OptionalHeader32Dump.Name = "PE Optional Header (WinAPI: IMAGE_OPTIONAL32_HEADER)";
            _numberOfRvaAndSizes = OptionalHeader32Dump.Segmentation.NumberOfRvaAndSizes;
            Machine64Bit = false;
        }
        else
        {
            OptionalHeaderDump.Segmentation = Fill<PeOptionalHeader>(reader);
            OptionalHeaderDump.Address = MzHeaderDump.Segmentation.e_lfanew;
            OptionalHeaderDump.Size = SizeOf(FileHeaderDump.Segmentation);
            OptionalHeaderDump.Name = "PE Optional Header (WinAPI: IMAGE_OPTIONAL_HEADER)";
            _numberOfRvaAndSizes = OptionalHeaderDump.Segmentation.NumberOfRvaAndSizes;
            Machine64Bit = true;
        }
        
        List<PeSectionDump> sects = [];
        for (Int32 i = 0; i < _numberOfSections; ++i)
        {
            UInt64 address = (UInt64)stream.Position;
            PeSection section = Fill<PeSection>(reader);
            
            PeSectionDump dump = new()
            {
                Address = address,
                Segmentation = section
            };
            dump.Size = SizeOf(dump.Segmentation);
            dump.Name = "PE Section (WinAPI: IMAGE_SECTION_HEADER)";
            FindSectionCharacteristics(ref dump);
            
            sects.Add(dump);
        }

        SectionDumps = sects.ToArray();
        
        FindDirectoriesCharacteristics();
        FindCharacteristics();
        FindDllCharacteristics();
        FindTimeStamp();
        
        PeSectionDumper dumper = new 
        (
            ExtractDirectoriesFromDump(),
            ExtractSectionsFromDump(),
            Machine64Bit
        );
        ImportsDump = dumper.ImportsDump(reader);
        ExportsDump = dumper.ExportsDump(reader);

        if (DirectoryDumps[14].Segmentation.Size != 0)
        {
            // Common Runtime
            Cor20HeaderDump = dumper.CorDump(reader);
            HasCorData = true;
        }
        
        reader.Close();
    }
    /// <returns> All structures of <see cref="PeDirectory"/>[] array </returns>
    private PeDirectory[] ExtractDirectoriesFromDump()
    {
        return DirectoryDumps.Select(x => x.Segmentation).ToArray();
    }
    /// <returns> All structures of <see cref="PeSection"/>[] array </returns>
    private PeSection[] ExtractSectionsFromDump()
    {
        return SectionDumps.Select(x => x.Segmentation).ToArray();
    }
    /// <param name="reader"><see cref="BinaryReader"/> instance</param>
    /// <typeparam name="TStruct">structure</typeparam>
    /// <returns></returns>
    private TStruct Fill<TStruct>(BinaryReader reader) where TStruct : struct
    {
        Byte[] bytes = reader.ReadBytes(Marshal.SizeOf(typeof(TStruct)));
        GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
        TStruct result = Marshal.PtrToStructure<TStruct>(handle.AddrOfPinnedObject());
        handle.Free();
        
        return result;
    }
    /// <param name="structure">required structure</param>
    /// <typeparam name="TStruct"></typeparam>
    /// <returns>Size of structure</returns>
    private Int32 SizeOf<TStruct>(TStruct structure) where TStruct : struct
    {
        return Marshal.SizeOf(structure);
    }
    /// <summary> Searches and writes all information about <see cref="PeDirectory"/>[] </summary>
    private void FindDirectoriesCharacteristics()
    {
        String[] names =
        [
            "image_export_directory".ToUpper(),
            "image_import_directory".ToUpper(),
            "image_resource_directory".ToUpper(),
            "image_exception_directory".ToUpper(),
            "image_certification_directory".ToUpper(),
            "image_baserelocs_directory".ToUpper(),
            "image_debug_directory".ToUpper(),
            "image_architecture_directory".ToUpper(),
            "image_globalptr_directory".ToUpper(),
            "image_tls_directory".ToUpper(),
            "image_loadconfig_directory".ToUpper(),
            "image_boundimport_directory".ToUpper(),
            "image_iat_directory".ToUpper(),
            "image_delay_import_descriptors_directory".ToUpper(),
            "image_com_directory".ToUpper(),
            "image_reserved_directory".ToUpper()
        ];
        PeDirectory[] directories = Machine64Bit
            ? OptionalHeaderDump.Segmentation.Directories
            : OptionalHeader32Dump.Segmentation.Directories;
        
        List<PeDirectoryDump> directoryDumps = [];
        for (Int32 i = 0; i < _numberOfRvaAndSizes; ++i)
        {
            String name = $"PE Directory (JellyBins: {names[i]})";
            Int32 size = 8;
            PeDirectoryDump dump = new()
            {
                Name = name,
                Size = size,
                Segmentation = directories[i]
            };
            directoryDumps.Add(dump);
        }

        DirectoryDumps = directoryDumps.ToArray();
    }
    /// <summary>
    /// Extracts all registered flags/characteristics from field
    /// </summary>
    private void FindCharacteristics()
    {
        List<String> chars = [];
        UInt16 c = FileHeaderDump.Segmentation.Characteristics;
        if ((c & 0x0001) != 0) chars.Add("image_file_relocs_stripped".ToUpper());
        if ((c & 0x0002) != 0) chars.Add("image_file_executable".ToUpper());
        if ((c & 0x0004) != 0) chars.Add("image_file_linenums_stripped".ToUpper());
        if ((c & 0x0008) != 0) chars.Add("image_file_local_syms_stripped".ToUpper());
        if ((c & 0x0010) != 0) chars.Add("image_file_aggressive_ws_trim".ToUpper());
        if ((c & 0x0020) != 0) chars.Add("image_file_large_address_aware".ToUpper());
        if ((c & 0x0040) != 0) chars.Add("image_file_reserved".ToUpper());
        if ((c & 0x0080) != 0) chars.Add("image_file_bytes_reverse_lo".ToUpper());
        if ((c & 0x0100) != 0) chars.Add("image_file_32bit_machine".ToUpper());
        if ((c & 0x0200) != 0) chars.Add("image_debug_stripped".ToUpper());
        if ((c & 0x0400) != 0) chars.Add("image_file_media_run_from_swap".ToUpper());
        if ((c & 0x0800) != 0) chars.Add("image_net_run_from_swap".ToUpper());
        if ((c & 0x1000) != 0) chars.Add("image_file_system".ToUpper());
        if ((c & 0x2000) != 0) chars.Add("image_file_dll".ToUpper());
        if ((c & 0x4000) != 0) chars.Add("image_file_up_system_only".ToUpper());
        if ((c & 0x8000) != 0) chars.Add("image_file_bytes_reverse_hi".ToUpper());

        FileHeaderDump.Characteristics = chars.ToArray();

        UInt16 machine = FileHeaderDump.Segmentation.Machine;
        Info.CpuArchitecture = machine switch
        {
            0x014c => "Intel i386",
            0x200  => "Intel Itanium",
            0x010c => "Intel i860",
            0x8664 => "Amd64",
            0xaa64 => "Arm64 (LE)",
            0x01c => "Arm (LE)",
            _ => "?"
        };
        Info.OperatingSystem = OptionalHeaderRomDump.Name == null 
            ? "Microsoft Windows" 
            : "EFI bytecode";
        
        Info.CpuWordLength = (UInt32?)(Machine64Bit ? 64 : 32);
        if ((c & 0x2000) != 0)
            _binaryTypeId = (UInt16)FileType.DynamicLibrary;
        else
            _binaryTypeId = (UInt16)FileType.Application;
        
    }
    /// <summary>
    /// Extracts all registered flags from field
    /// </summary>
    private void FindDllCharacteristics()
    {
        List<String> chars = [];
        UInt16 c = Machine64Bit
            ? OptionalHeaderDump.Segmentation.DllCharacteristics
            : OptionalHeader32Dump.Segmentation.DllCharacteristics;
        if ((c & 0x0020) != 0) chars.Add("image_dllcharacteristics_high_entropy_va".ToUpper());
        if ((c & 0x0040) != 0) chars.Add("image_dllcharacteristics_dynamic_base".ToUpper());
        if ((c & 0x0080) != 0) chars.Add("image_dllcharacteristics_force_integrity".ToUpper());
        if ((c & 0x0100) != 0) chars.Add("image_dllcharacteristics_nx_compat".ToUpper());
        if ((c & 0x0200) != 0) chars.Add("image_dllcharacteristics_no_isolation".ToUpper());
        if ((c & 0x0400) != 0) chars.Add("image_dllcharacteristics_no_seh".ToUpper());
        if ((c & 0x0800) != 0) chars.Add("image_dllcharacteristics_no_bind".ToUpper());
        if ((c & 0x1000) != 0) chars.Add("image_dllcharacteristics_appcontainer".ToUpper());
        if ((c & 0x2000) != 0) chars.Add("image_dllcharacteristics_wdm_driver".ToUpper());
        if ((c & 0x4000) != 0) chars.Add("image_dllcharacteristics_guard_cf".ToUpper());
        if ((c & 0x8000) != 0) chars.Add("image_dllcharacteristics_terminal_server_aware".ToUpper());

        if (Machine64Bit)
        {
            OptionalHeaderDump.Characteristics = chars.ToArray();
            Info.MinimumSystemVersion =
                $"{OptionalHeaderDump.Segmentation.MajorSubsystemVersion}.{OptionalHeaderDump.Segmentation.MinorSubsystemVersion}";
            Info.OperatingSystemVersion =
                $"{OptionalHeaderDump.Segmentation.MajorOperatingSystemVersion}.{OptionalHeaderDump.Segmentation.MinorOperatingSystemVersion}";
        }
        else
        {
            OptionalHeader32Dump.Characteristics = chars.ToArray();
            Info.MinimumSystemVersion =
                $"{OptionalHeader32Dump.Segmentation.MajorSubsystemVersion}.{OptionalHeader32Dump.Segmentation.MinorSubsystemVersion}";
            Info.OperatingSystemVersion =
                $"{OptionalHeader32Dump.Segmentation.MajorOperatingSystemVersion}.{OptionalHeader32Dump.Segmentation.MinorOperatingSystemVersion}";
        }
        if ((c & 0x2000) != 0)
            _binaryTypeId = (UInt16)FileType.Driver;
    }
    /// <summary>
    /// Translates byte values to registered section's characteristics
    /// </summary>
    /// <param name="dump">section's dump</param>
    private void FindSectionCharacteristics(ref PeSectionDump dump)
    {
        // change instance. make characteristics
        UInt64 s = dump.Segmentation.Characteristics;
        List<String> chars = [];
        
        if ((s & 0x00000008) != 0) chars.Add("image_scn_type_no_pad".ToUpper());
        if ((s & 0x00000010) != 0) chars.Add("image_scn_reserved".ToUpper());
        if ((s & 0x00000020) != 0) chars.Add("image_scn_code".ToUpper());
        if ((s & 0x00000040) != 0) chars.Add("image_scn_initialized_data".ToUpper());
        if ((s & 0x00000080) != 0) chars.Add("image_scn_uninitialized_data".ToUpper());
        if ((s & 0x00000100) != 0) chars.Add("image_scn_lnk_other".ToUpper());
        if ((s & 0x00000200) != 0) chars.Add("image_scn_lnk_info".ToUpper());
        if ((s & 0x00000800) != 0) chars.Add("image_scn_lnk_remove".ToUpper());
        if ((s & 0x00001000) != 0) chars.Add("image_scn_lnk_comdat".ToUpper());
        if ((s & 0x00008000) != 0) chars.Add("image_scn_gprel".ToUpper());
        if ((s & 0x00020000) != 0) chars.Add("image_scn_mem_16_bit".ToUpper());
        if ((s & 0x00040000) != 0) chars.Add("image_scn_mem_locked".ToUpper());
        if ((s & 0x00080000) != 0) chars.Add("image_scn_mem_preload".ToUpper());
        if ((s & 0x00100000) != 0) chars.Add("image_scn_align_1_byte".ToUpper());
        if ((s & 0x00200000) != 0) chars.Add("image_scn_align_2_bytes".ToUpper());
        if ((s & 0x00300000) != 0) chars.Add("image_scn_align_4_bytes".ToUpper());
        if ((s & 0x00400000) != 0) chars.Add("image_scn_align_8_bytes".ToUpper());
        if ((s & 0x00500000) != 0) chars.Add("image_scn_align_16_bytes".ToUpper());
        if ((s & 0x00600000) != 0) chars.Add("image_scn_align_32".ToUpper());
        if ((s & 0x00700000) != 0) chars.Add("image_scn_align_64_bytes".ToUpper());
        if ((s & 0x00800000) != 0) chars.Add("image_scn_align_128_bytes".ToUpper());
        if ((s & 0x00900000) != 0) chars.Add("image_scn_align_256_bytes".ToUpper());
        if ((s & 0x00A00000) != 0) chars.Add("image_scn_align_512_bytes".ToUpper());
        if ((s & 0x00B00000) != 0) chars.Add("image_scn_align_2014_bytes".ToUpper());
        if ((s & 0x00C00000) != 0) chars.Add("image_scn_align_2048_bytes".ToUpper());
        if ((s & 0x00D00000) != 0) chars.Add("image_scn_align_4096_bytes".ToUpper());
        if ((s & 0x00E00000) != 0) chars.Add("image_scn_align_8192_bytes".ToUpper());
        if ((s & 0x01000000) != 0) chars.Add("image_scn_nreloc_ovfl".ToUpper());
        if ((s & 0x02000000) != 0) chars.Add("image_scn_mem_discardable".ToUpper());
        if ((s & 0x04000000) != 0) chars.Add("image_scn_mem_not_cached".ToUpper());
        if ((s & 0x08000000) != 0) chars.Add("image_scn_mem_not_paged".ToUpper());
        if ((s & 0x10000000) != 0) chars.Add("image_scn_mem_shared".ToUpper());
        if ((s & 0x20000000) != 0) chars.Add("image_scn_mem_execute".ToUpper());
        if ((s & 0x40000000) != 0) chars.Add("image_scn_mem_read".ToUpper());
        if ((s & 0x80000000) != 0) chars.Add("image_scn_mem_write".ToUpper());

        dump.Characteristics = chars.ToArray();
    }
    /// <summary>
    /// Translates time and date of creation
    /// </summary>
    private void FindTimeStamp()
    {
        DateTime stamp = new(1970, 1, 1, 0, 0, 0);

        stamp = stamp.AddSeconds(FileHeaderDump.Segmentation.TimeDateStamp);
        stamp += CurrentTimeZone.GetUtcOffset(stamp);
        
        FileHeaderDump.TimeStamp = stamp;
        Info.DateTimeStamp = stamp.ToString(CultureInfo.InvariantCulture);
    }
    /// <param name="characteristics"><see cref="PeFileHeader"/> characteristics</param>
    /// <returns><c>True</c> if <c>IMAGE_MACHINE_32_BIT</c> not found</returns>
    private Boolean IsMachine64Bit(UInt16 characteristics)
    {
        return (characteristics & 0x0100) == 0; // machine 32 bit flag
    }
    /// <returns> Application's type based on extension </returns>
    public UInt16 GetExtensionTypeId()
    {
        return _extensionTypeId;
    }
    /// <returns> Application's type based on internal structure </returns>
    public UInt16 GetBinaryTypeId()
    {
        return _binaryTypeId;
    }
}