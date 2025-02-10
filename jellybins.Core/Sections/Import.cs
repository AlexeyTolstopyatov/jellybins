using System.Runtime.InteropServices;

namespace jellybins.Core.Sections;

[StructLayout(LayoutKind.Sequential)]
public struct ImportDescriptor
{
    public uint OriginalFirstThunk;
    public uint TimeDateStamp;
    public uint ForwarderChain;
    public uint Name;
    public uint FirstThunk;
}
// public static void ParsePEFile(string filePath)
// {
//  using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
//  using (var reader = new BinaryReader(stream))
//  {
//     var dosHeader = ReadStruct<IMAGE_DOS_HEADER>(reader);
//     if (dosHeader.e_magic != 0x5A4D)
//     {
//         throw new InvalidOperationException("Invalid DOS signature");
//     }
//
//     stream.Seek(dosHeader.e_lfanew, SeekOrigin.Begin);
//     var ntHeaders = ReadStruct<IMAGE_NT_HEADERS32>(reader);
//     if (ntHeaders.Signature != 0x00004550)
//     {
//         throw new InvalidOperationException("Invalid NT signature");
//     }
//
//     if (ntHeaders.OptionalHeader.Magic == 0x10b)
//     {
//         // PE32
//         ParseImports32(reader, ntHeaders.OptionalHeader.DataDirectory[1]);
//     }
//     else if (ntHeaders.OptionalHeader.Magic == 0x20b)
//     {
//         // PE64
//         var ntHeaders64 = ReadStruct<IMAGE_NT_HEADERS64>(reader);
//         ParseImports64(reader, ntHeaders64.OptionalHeader.DataDirectory[1]);
//     }
//     else
//     {
//         throw new InvalidOperationException("Unknown PE format");
//     }
// }
// }
//