namespace JellyBins.DosCommand.Models;

public class ComReport
{
    public ComSection[]? Sections { get; set; }
    public ComData? Data { get; set; }
    public String[]? Flags { get; set; }
}