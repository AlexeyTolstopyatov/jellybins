namespace JellyBins.Abstractions;

public interface IFileDumper
{
    Task DumpAsync();
    UInt16 GetExtensionTypeId();
    UInt16 GetBinaryTypeId();
}