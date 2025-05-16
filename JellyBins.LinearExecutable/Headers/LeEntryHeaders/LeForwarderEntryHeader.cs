using System.Runtime.InteropServices;

namespace JellyBins.LinearExecutable.Headers.LeEntryHeaders;

[StructLayout(LayoutKind.Sequential)]
public struct LeForwarderEntryHeader
{
    //public Byte Count;
    //public Byte Type;
    public UInt16 Unknown;
    public Byte Flags;
    public UInt16 Ordinal;
    public UInt16 Offset;
}