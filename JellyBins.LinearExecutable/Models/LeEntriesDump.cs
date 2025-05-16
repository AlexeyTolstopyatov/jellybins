using System.ComponentModel.DataAnnotations;
using JellyBins.Abstractions;
using JellyBins.LinearExecutable.Headers.LeEntryHeaders;

namespace JellyBins.LinearExecutable.Models;

public class LeEntriesDumps
{
    public BaseDump<List<Le16BitEntryHeader>> Entries16Bit { get; init; } = new();
    public BaseDump<List<Le32BitEntryHeader>> Entries32Bit { get; init; } = new();
    public BaseDump<List<LeCallGateEntryHeader>> EntriesCallGate { get; init; } = new();
    public BaseDump<List<LeForwarderEntryHeader>> EntriesForwarder { get; init; } = new();
}