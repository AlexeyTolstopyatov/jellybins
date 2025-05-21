using JellyBins.Abstractions;
using JellyBins.PortableExecutable.Headers;

namespace JellyBins.PortableExecutable.Models;

public class PeImportsDump : BaseDump<List<PeImportDescriptor>>
{
    public List<ImportDll> FoundImports { get; set; } = [];
}