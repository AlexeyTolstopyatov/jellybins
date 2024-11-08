namespace jellybins.File.Headers;

public struct RelocationRow
{
    public string Section { get; set; }
    public uint RVA { get; set; }
    public uint Size { get; set; }
    
    public RelocationRow(string name, uint rva, uint size)
    {
        Section = name;
        RVA = rva;
        Size = size;
    }
    
}