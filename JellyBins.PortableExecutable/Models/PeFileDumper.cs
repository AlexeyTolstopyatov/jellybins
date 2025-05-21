using System.Runtime.InteropServices;
using JellyBins.Abstractions;
using JellyBins.PortableExecutable.Headers;
using System.Reflection;
using System.Text.RegularExpressions;
using static System.TimeZone;

namespace JellyBins.PortableExecutable.Models;

public class PeFileDumper(String path) : IFileDumper
{
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
    public PeDirectoryDump[] DirectoryDumps { get; private set; } = [];
    public PeCor20HeaderDump Cor20HeaderDump { get; private set; } = new();
    public PeVb5HeaderDump Vb5HeaderDump { get; private set; } = new();
    
    private UInt16 _extensionTypeId = 0;
    private UInt16 _binaryTypeId = 0;
    private UInt32 _numberOfRvaAndSizes;
    private UInt32 _numberOfSections;
    private Boolean _machine64Bit;
    
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
        if (!Machine64Bit(FileHeaderDump.Segmentation.Characteristics))
        {
            OptionalHeader32Dump.Segmentation = Fill<PeOptionalHeader32>(reader);
            OptionalHeader32Dump.Address = (UInt64)stream.Position;
            OptionalHeader32Dump.Size = SizeOf(OptionalHeader32Dump.Segmentation);
            OptionalHeader32Dump.Name = "PE Optional Header (WinAPI: IMAGE_OPTIONAL32_HEADER)";
            _numberOfRvaAndSizes = OptionalHeader32Dump.Segmentation.NumberOfRvaAndSizes;
            _machine64Bit = false;
        }
        else
        {
            OptionalHeaderDump.Segmentation = Fill<PeOptionalHeader>(reader);
            OptionalHeaderDump.Address = MzHeaderDump.Segmentation.e_lfanew;
            OptionalHeaderDump.Size = SizeOf(FileHeaderDump.Segmentation);
            OptionalHeaderDump.Name = "PE Optional Header (WinAPI: IMAGE_OPTIONAL_HEADER)";
            _numberOfRvaAndSizes = OptionalHeaderDump.Segmentation.NumberOfRvaAndSizes;
            _machine64Bit = true;
        }
        
        List<PeSectionDump> sects = [];
        // checking sections by offset
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
            ExtractSectionsFromDump()
        );

        ImportsDump = dumper.ImportsDump(reader);
        
        reader.Close();
    }

    private PeDirectory[] ExtractDirectoriesFromDump()
    {
        return DirectoryDumps.Select(x => x.Segmentation).ToArray();
    }
    private PeSection[] ExtractSectionsFromDump()
    {
        return SectionDumps.Select(x => x.Segmentation).ToArray();
    }
    private TStruct Fill<TStruct>(BinaryReader reader) where TStruct : struct
    {
        Byte[] bytes = reader.ReadBytes(Marshal.SizeOf(typeof(TStruct)));
        GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
        TStruct result = Marshal.PtrToStructure<TStruct>(handle.AddrOfPinnedObject());
        handle.Free();
        return result;
    }
    private Int32 SizeOf<TStruct>(TStruct structure) where TStruct : struct
    {
        return Marshal.SizeOf(structure);
    }

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
        PeDirectory[] directories = _machine64Bit
            ? OptionalHeaderDump.Segmentation.Directories
            : OptionalHeader32Dump.Segmentation.Directories;
        
        List<PeDirectoryDump> directoryDumps = [];
        for (Int32 i = 0; i < _numberOfRvaAndSizes; ++i)
        {
            String name = $"PE Data Directory (JellyBins: {names[i]})";
            Int32 size = 8;
            PeDirectoryDump dump = new()
            {
                Name = name,
                Characteristics = ["DATA_OFFSET_RVA"],
                Size = size,
                Segmentation = directories[i]
            };
            directoryDumps.Add(dump);
        }

        DirectoryDumps = directoryDumps.ToArray();
    }

    private void FindCharacteristics()
    {
        // make FileHeader characteristics array
    }

    private void FindDllCharacteristics()
    {
        // make OptionalHeader characteristics array
    }

    private void FindSectionCharacteristics(ref PeSectionDump dump)
    {
        // change instance. make characteristics
        
    }
    private void FindTimeStamp()
    {
        DateTime stamp = new(1970, 1, 1, 0, 0, 0);

        stamp = stamp.AddSeconds(FileHeaderDump.Segmentation.TimeDateStamp);
        stamp += CurrentTimeZone.GetUtcOffset(stamp);
        
        FileHeaderDump.TimeStamp = stamp;
    }
    private Boolean Machine64Bit(UInt16 characteristics)
    {
        return (characteristics & 0x0100) == 0; // machine 32 bit flag
    }
    
    public UInt16 GetExtensionTypeId()
    {
        return _extensionTypeId;
    }

    public UInt16 GetBinaryTypeId()
    {
        return _binaryTypeId;
    }
}