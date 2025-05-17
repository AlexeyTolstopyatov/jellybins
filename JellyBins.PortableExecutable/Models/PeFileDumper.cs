using System.Runtime.InteropServices;
using JellyBins.Abstractions;
using JellyBins.PortableExecutable.Headers;

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

    public void Dump()
    {
        
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