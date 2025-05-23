namespace JellyBins.Abstractions;

/// <summary>
/// Supported segmentations for JellyBins
/// internals
/// </summary>
public enum FileSegmentationType
{
    Unknown = 0,
    Dos1Command = 1,
    Dos2MarkZbikowski = 2,
    NewExecutable = 3,
    LinearExecutable = 4,
    PortableExecutable = 5,
    AssemblerOutput = 6,
    ExecutableLinkable = 7,
    MachObject = 8,
    FatMachObject = 9
}