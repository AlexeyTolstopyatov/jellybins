using JellyBins.Abstractions;
using JellyBins.PortableExecutable.Headers;

namespace JellyBins.PortableExecutable.Models;

public class PeImportsDump : BaseDump<List<PeImportDescriptor32>>
{
    public List<ImportDll> FoundImports { get; set; } = [];
}