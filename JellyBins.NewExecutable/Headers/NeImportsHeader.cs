using System.Runtime.InteropServices;

namespace JellyBins.NewExecutable.Headers;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct NeImportsHeader
{
    public UInt16 NameLength;
    [MarshalAs(UnmanagedType.LPStr)] public String Name;
}