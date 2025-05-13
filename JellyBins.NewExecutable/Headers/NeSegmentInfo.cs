using System.Runtime.InteropServices;

namespace JellyBins.NewExecutable.Headers;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct NeSegmentInfo
{
    public UInt16 SegmentNumber { get; set; }
    public UInt32 FileOffset { get; set; }
    public UInt16 FileLength { get; set; }
    public UInt16 Flags { get; set; }
    public UInt16 MinAllocation { get; set; }
    public String Type => (Flags & 0x0007) switch
    {
        0x0000 => "CODE",
        0x0001 => "DATA",
        _ => "UNKNOWN"
    };
}