namespace JellyBins.Abstractions;

public interface IFileDumper
{
    /// <summary>
    /// Makes dump of required (set in constructor method)
    /// binary
    /// </summary>
    void Dump();
    UInt16 GetExtensionTypeId();
    UInt16 GetBinaryTypeId();
    /// <summary>
    /// JellyBins internal type flag of determined binary
    /// </summary>
    FileSegmentationType SegmentationType { get; }
}