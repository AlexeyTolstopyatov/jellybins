using JellyBins.Abstractions;

namespace JellyBins.PortableExecutable.Models;

public class PeFileInfo : BaseFileInfo
{
    public String? RuntimeWord { get; set; }
    public String? DateTimeStamp { get; set; }
    
    public String? MinimumSystemVersion { get; set; }
}