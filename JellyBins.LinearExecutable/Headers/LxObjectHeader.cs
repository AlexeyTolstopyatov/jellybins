using System.Runtime.InteropServices;

namespace JellyBins.LinearExecutable.Headers;

[StructLayout(LayoutKind.Explicit)]
public struct LxObjectHeader
{
    [FieldOffset(0x00)] public UInt32 VirtualSize;     
    [FieldOffset(0x04)] public UInt32 RelocBaseAddr;   
    [FieldOffset(0x08)] public UInt16 ObjectFlags;     
    [FieldOffset(0x10)] public UInt32 PageTableIndex;  
    [FieldOffset(0x14)] public UInt32 PageTableEntries;
    [FieldOffset(0x18)] public UInt32 Reserved;
}