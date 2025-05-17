using System.Collections.Specialized;
using System.Drawing;
using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices;
using JellyBins.Abstractions;
using JellyBins.PortableExecutable.Headers;
using Microsoft.VisualBasic;

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
    public PeDirectoryDump[] DirectoryDumps { get; private set; } = [];
    public PeCor20HeaderDump Cor20HeaderDump { get; private set; } = new();
    public PeVb5HeaderDump Vb5HeaderDump { get; private set; } = new();
    
    private UInt16 _extensionTypeId = 0;
    private UInt16 _binaryTypeId = 0;
    private UInt32 _numberOfRvaAndSizes;
    private UInt32 _numberOfSections;

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
        }
        else
        {
            OptionalHeaderDump.Segmentation = Fill<PeOptionalHeader>(reader);
            OptionalHeaderDump.Address = MzHeaderDump.Segmentation.e_lfanew;
            OptionalHeaderDump.Size = SizeOf(FileHeaderDump.Segmentation);
            OptionalHeaderDump.Name = "PE Optional Header (WinAPI: IMAGE_OPTIONAL_HEADER)";
            _numberOfRvaAndSizes = OptionalHeaderDump.Segmentation.NumberOfRvaAndSizes;
        }
        // checking directories by offset
        List<PeDirectoryDump> dirs = [];
        for (Int32 i = 0; i < 16; ++i)
        {
            PeDirectoryDump dump = new()
            {
                Address = (UInt64)stream.Position,
                Segmentation = Fill<PeDirectory>(reader)
            };
            dump.Size = SizeOf(dump.Segmentation);
            dump.Name = "PE Directory (WinAPI: IMAGE_DIRECTORY)";
            
            dirs.Add(dump);
        }

        DirectoryDumps = dirs.ToArray();
        List<PeSectionDump> sects = [];
        // checking sections by offset
        for (Int32 i = 0; i < _numberOfSections; ++i)
        {
            PeSectionDump dump = new()
            {
                Address = (UInt64)stream.Position,
                Segmentation = Fill<PeSection>(reader),
            };
            dump.Size = SizeOf(dump.Segmentation);
            dump.Name = "PE Section (WinAPI: IMAGE_SECTION)";
            sects.Add(dump);
        }

        SectionDumps = sects.ToArray();
        
        reader.Close();
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
    private Boolean Machine64Bit(UInt16 characteristics)
    {
        return (characteristics & 0x0100) == 0;
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