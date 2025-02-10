using System.Runtime.InteropServices;

namespace jellybins.Core.Sections;

[StructLayout(LayoutKind.Sequential)]
public struct Data {
    public uint VirtualAddress;
    public uint Size;
}