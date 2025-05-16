using System.Runtime.InteropServices;
using JellyBins.Abstractions;
using JellyBins.LinearExecutable.Headers;
using JellyBins.LinearExecutable.Private;

namespace JellyBins.LinearExecutable.Models;

public class LxFileDumper : IFileDumper
{
    public MzHeaderDump MzHeaderDump { get; private set; } = new();
    public LxHeaderDump LxHeaderDump { get; private set; } = new();
    public LxFileInfo Info { get; private set; } = new();
    public VirtualAddress Lva { get; private set; } = new(0);

    public void Dump()
    {
        using FileStream stream = new(Info.Path!, FileMode.Open, FileAccess.Read);
        using BinaryReader reader = new(stream);
        
        // IMAGE_DOS_HEADER construct
        MzHeaderDump.Segmentation = Fill<MzHeader>(reader);
        MzHeaderDump.Address = 0;
        MzHeaderDump.Size = SizeOf(MzHeaderDump.Segmentation);
        MzHeaderDump.Name = "MZ Header (WinAPI: IMAGE_DOS_HEADER)";

        // LX_HEADER construct (sources: os2.h emm386.h vmm386.h)
        stream.Seek(MzHeaderDump.Segmentation.e_lfanew, SeekOrigin.Begin);
        LxHeaderDump.Segmentation = Fill<LxHeader>(reader);
        LxHeaderDump.Address = MzHeaderDump.Segmentation.e_lfanew;
        LxHeaderDump.Size = SizeOf(LxHeaderDump.Segmentation);
        LxHeaderDump.Name = "LX Header (OS/2 API: LX_HEADER)";

        Lva = new VirtualAddress((UInt64)LxHeaderDump.Address);
        
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
    
    public UInt16 GetExtensionTypeId()
    {
        throw new NotImplementedException();
    }

    public UInt16 GetBinaryTypeId()
    {
        throw new NotImplementedException();
    }
}