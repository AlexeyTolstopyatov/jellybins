using System.Runtime.InteropServices;

namespace jellybins.Core.Headers.Runtime;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
public struct CommonLanguageRuntime
{
    public uint SizeofHead;
    public ushort MajorRuntimeVersion;
    public ushort MinorRuntimeVersion;
    public ulong MetaDataOffset;
    public uint LinkerFlags;
    public uint EntryPointToken;
}