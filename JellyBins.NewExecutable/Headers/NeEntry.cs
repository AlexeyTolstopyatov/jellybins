namespace JellyBins.NewExecutable.Headers;

public class NeEntry
{
    public Boolean IsByOrdinal { get; set; }
    public UInt16 Ordinal { get; set; }
    public String? Name { get; set; }
    public String? ModuleName { get; set; }
}