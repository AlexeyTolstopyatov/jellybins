namespace JellyBins.LinearExecutable.Headers;

public class LeImportEntry
{
    public String? ModuleName { get; set; }
    public String? ProcedureName { get; set; }
    public UInt32 Ordinal { get; set; }
    public Boolean IsByOrdinal { get; set; }
}