namespace JellyBins.Abstractions;

public interface IFileDumper
{
    void Dump();
    UInt16 GetExtensionTypeId();
    UInt16 GetBinaryTypeId();
    FileSegmentationType SegmentationType { get; }
}