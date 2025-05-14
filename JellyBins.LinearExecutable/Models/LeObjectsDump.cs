using JellyBins.Abstractions;
using JellyBins.LinearExecutable.Headers;

namespace JellyBins.LinearExecutable.Models;

public class LeObjectDump : BaseDump<LeObjectHeader>
{
    public String? SuggestedObjectName { get; set; }
}