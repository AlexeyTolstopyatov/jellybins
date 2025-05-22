using JellyBins.Abstractions;
using JellyBins.PortableExecutable.Headers;

namespace JellyBins.PortableExecutable.Models;

public class PeExportsDump : BaseDump<List<ExportFunction>>
{
    public PeImageExportDirectory ImageExportDirectory { get; set; }
}