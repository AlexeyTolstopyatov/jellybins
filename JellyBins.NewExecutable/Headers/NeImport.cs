using System.Runtime.InteropServices;

namespace JellyBins.NewExecutable.Headers;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct NeImport
{
    public UInt16 NameLength;
    public String Name;
}