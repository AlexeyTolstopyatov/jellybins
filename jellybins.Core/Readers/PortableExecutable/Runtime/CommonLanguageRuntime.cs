using System.Runtime.InteropServices;

namespace jellybins.Core.Readers.PortableExecutable.Runtime;
/// <summary>
/// Cor20 header which stores in .cormeta section If I'M ReAlLY kNoW It
/// </summary>
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
public struct CommonLanguageRuntime
{
    public uint SizeOfHead;
    public ushort MajorRuntimeVersion;
    public ushort MinorRuntimeVersion;
    public ulong MetaDataOffset;
    public uint LinkerFlags;
    public uint EntryPointToken;
}