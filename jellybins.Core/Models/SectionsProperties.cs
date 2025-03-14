namespace jellybins.Core.Models;

public class SectionsProperties
{
    public string? Name { get; set; }
    public uint VirtualAddress { get; set; }
    public uint VirtualSize { get; set; }
    public uint SizeOfRawData { get; set; }
    public uint PointerToRawData { get; set; }
    public uint PointerToRelocations { get; set; }
    public uint PointerToLineNumbers { get; set; }
    public ushort NumberOfRelocations { get; set; }
    public ushort NumberOfLineNumbers { get; set; }
    public string[]? Characteristics { get; set; }
}