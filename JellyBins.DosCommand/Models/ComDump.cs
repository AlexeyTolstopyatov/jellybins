using JellyBins.Abstractions;

namespace JellyBins.DosCommand.Models;

public class ComDump : BaseDump<ComSection[]>
{
    public ComSection[]? Sections { get; set; }
}