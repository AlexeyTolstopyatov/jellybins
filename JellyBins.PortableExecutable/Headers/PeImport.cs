using System.Runtime.InteropServices;
using JellyBins.PortableExecutable.Headers;

namespace JellyBins.PortableExecutable;

public struct PeImport
{
    [MarshalAs(UnmanagedType.ByValArray)]
    public Char[] Name;
    [MarshalAs(UnmanagedType.Struct)]
    public PeImportDescriptor32 Descriptor32;
}