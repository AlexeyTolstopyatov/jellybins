using System.Runtime.InteropServices;

namespace JellyBins.PortableExecutable.Headers;

[StructLayout(LayoutKind.Explicit)]
public struct PeSection 
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)] 
    [FieldOffset(0)] public Byte[] Name;
    [FieldOffset(8)] public UInt32 VirtualSize;
    [FieldOffset(12)] public UInt32 VirtualAddress;
    [FieldOffset(16)] public UInt32 SizeOfRawData;
    [FieldOffset(20)] public UInt32 PointerToRawData;
    [FieldOffset(24)] public UInt32 PointerToRelocations;
    [FieldOffset(28)] public UInt32 PointerToLineNumbers;
    [FieldOffset(32)] public UInt16 NumberOfRelocations;
    [FieldOffset(34)] public UInt16 NumberOfLineNumbers;
    [FieldOffset(36)] public UInt64 Characteristics;
}