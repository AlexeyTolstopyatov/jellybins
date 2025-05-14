using System.Runtime.InteropServices;

namespace JellyBins.LinearExecutable.Headers;

[StructLayout(LayoutKind.Sequential)]
public struct LeDirective
{
    public UInt16 DirectiveNumber;
    public UInt16 DataLength;
    public UInt32 DataOffset;
}