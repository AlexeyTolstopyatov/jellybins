using System.Runtime.InteropServices;

namespace jellybins.Core.Sections;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
public struct Data
{
    public Data(){}
    
    public uint VirtualAddress = 0;
    public uint Size = 0;
}