namespace JellyBins.Abstractions;

public class BaseFileInfo
{
    public String? Name { get; set; }
    public String? Path { get; set; }
    public String? OperatingSystem { get; set; }
    public String? OperatingSystemVersion { get; set; }
    public String? CpuArchitecture { get; set; }
    public UInt32? CpuWordLength { get; set; } = 16;
}