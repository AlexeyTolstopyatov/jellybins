using System.Runtime.InteropServices;
using System.Xml.XPath;
using JellyBins.Abstractions;
using JellyBins.LinearExecutable.Headers;

namespace JellyBins.LinearExecutable.Models;

public class LeFileDumper : IFileDumper
{
    public LeFileDumper(String path)
    {
        Info = new()
        {
            Name = new FileInfo(path).Name,
            Path = path,
            CpuWordLength = 16,
        };
        MzHeaderDump = new();
        LeHeaderDump = new();
    }
    public LeFileInfo Info { get; private set; }
    public MzHeaderDump? MzHeaderDump { get; private set; }
    public LeHeaderDump? LeHeaderDump { get; private set; }
    
    public void Dump()
    {
        using FileStream stream = new(Info.Path!, FileMode.Open, FileAccess.Read);
        using BinaryReader reader = new(stream);
        
        // IMAGE_DOS_HEADER construct
        MzHeaderDump!.Segmentation = Fill<MzHeader>(reader);
        MzHeaderDump.Address = 0;
        MzHeaderDump.Size = SizeOf(MzHeaderDump.Segmentation);
        MzHeaderDump.Name = "MZ Header (WinAPI: IMAGE_DOS_HEADER)";

        // LX_HEADER construct (sources: os2.h emm386.h vmm386.h)
        stream.Seek(MzHeaderDump.Segmentation.e_lfanew, SeekOrigin.Begin);
        LeHeaderDump!.Segmentation = Fill<LeHeader>(reader);
        LeHeaderDump.Address = MzHeaderDump.Segmentation.e_lfanew;
        LeHeaderDump.Size = SizeOf(LeHeaderDump.Segmentation);
        LeHeaderDump.Name = "LX Header (OS/2 API: LX_HEADER)";

        
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