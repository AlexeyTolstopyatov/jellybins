using System.Runtime.InteropServices;

namespace jellybins.Core.Readers.PortableExecutable;

/// <summary>
/// PortableExecutableDataDirectory is Directory structure (logical partitions of Microsoft COFF)
/// </summary>
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct PortableExecutableDataDirectory {
    [MarshalAs(UnmanagedType.U4)]
    public uint VirtualAddress;
    [MarshalAs(UnmanagedType.U4)]
    public uint Size;
}