using JellyBins.Abstractions;
using JellyBins.PortableExecutable.Headers;

namespace JellyBins.PortableExecutable.Models;

public class PeDirectoryDump : BaseDump<PeDirectory>
{
    public String Win32ApiName { get; set; } = "image_data_directory".ToUpper();
}