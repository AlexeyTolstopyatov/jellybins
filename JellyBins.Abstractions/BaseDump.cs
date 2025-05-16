namespace JellyBins.Abstractions;

public class BaseDump<T>
{
    public String? Name { get; set; }
    public UInt64? Address { get; set; }
    public Int32? Size { get; set; }
    public String[]? Characteristics { get; set; }
    public T? Segmentation { get; set; }
}