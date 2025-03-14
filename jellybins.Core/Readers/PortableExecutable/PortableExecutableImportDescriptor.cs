using System.Runtime.InteropServices;

namespace jellybins.Core.Readers.PortableExecutable;

[StructLayout(LayoutKind.Sequential)]
public struct ImportDescriptor
{
    public uint OriginalFirstThunk;
    public uint TimeDateStamp;
    public uint ForwarderChain;
    public uint Name;
    public uint FirstThunk;
}