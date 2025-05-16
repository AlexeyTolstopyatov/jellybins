using System.Runtime.InteropServices;

namespace JellyBins.LinearExecutable.Headers.LeEntryHeaders;

[StructLayout(LayoutKind.Sequential)]
public struct LeCallGateEntryHeader
{
    //public Byte Count;
    //public Byte Type;
    public UInt16 Object;
    public UInt16 Flags;
    public UInt16 CallGate;
}