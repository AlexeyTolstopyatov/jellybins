using System.Runtime.InteropServices;

namespace jellybins.File.Headers;
/*
 * Jelly Bins (C) Толстопятов Алексей 2024
 *         Common Language Runtime Header
 * Структура заголовка CLR 
 */
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
public struct ClrHeader
{
    public uint SizeofHead;
    public ushort MajorRuntimeVersion;
    public ushort MinorRuntimeVersion;
    public ulong MetaDataOffset;
    public uint LinkerFlags;
    public uint EntryPointToken;
}