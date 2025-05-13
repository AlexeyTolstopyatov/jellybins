using System.Runtime.InteropServices;

namespace JellyBins.NewExecutable.Headers;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct NeSegment
{
    [MarshalAs(UnmanagedType.U4)]
    public UInt32 LocalSectorOffset;
    [MarshalAs(UnmanagedType.U4)]
    public UInt32 SegmentLength;
    [MarshalAs(UnmanagedType.U4)]
    public UInt32 Flag;
    [MarshalAs(UnmanagedType.U4)]
    public UInt32 MinimumAlloc;
}