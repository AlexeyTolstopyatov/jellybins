namespace JellyBins.Abstractions;

public class BaseDump<TStruct>
{
    public String? Name { get; set; }
    public UInt64? Address { get; set; }
    public Int32? Size { get; set; }
    public String[]? Characteristics { get; set; }
    public TStruct? Segmentation { get; set; }
}