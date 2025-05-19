using JellyBins.Abstractions;
using JellyBins.PortableExecutable.Headers;

namespace JellyBins.PortableExecutable.Models;

public class PeFileHeaderDump : BaseDump<PeFileHeader>
{
    public DateTime TimeStamp { get; set; }
}