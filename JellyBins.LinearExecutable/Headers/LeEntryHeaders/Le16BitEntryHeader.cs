using System.Runtime.InteropServices;

namespace JellyBins.LinearExecutable.Headers.LeEntryHeaders;

[StructLayout(LayoutKind.Sequential)]
public struct Le16BitEntryHeader
{
    //public Byte Count;
    //public Byte Type;
    public UInt16 Object;
    public Byte Flags;
    public UInt16 Offset; // @OrdinalNumber
}