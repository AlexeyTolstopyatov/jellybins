using System.Runtime.InteropServices;

namespace JellyBins.NewExecutable.Headers;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct NeSegmentsHeader
{
    public UInt32 LocalSectorOffset;
    public UInt32 SegmentLength;
    public UInt32 Flag;
    public UInt32 MinimumAlloc;
}