using JellyBins.Abstractions;

namespace JellyBins.NewExecutable.Models;

public class NeInfo : BaseFileInfo
{
    public String? ProjectDescription { get; set; }
    public UInt32 BinaryType { get; set; }
    public UInt32 ExtensionType { get; set; }
}