using JellyBins.Abstractions;

namespace JellyBins.NewExecutable.Models;

public class NeFileDumper : IFileDumper
{
    public String? Path { get; init; }
    public String? Name { get; init; }
    private UInt16 _extensionTypeId;
    private UInt16 _binaryTypeId;

    public NeFileDumper(String path)
    {
        _extensionTypeId = 0;
        _binaryTypeId = 0;
        Path = path;
        Name = new FileInfo(path).Name;
    }
    
    public Task DumpAsync()
    {
        throw new NotImplementedException();
    }

    public UInt16 GetExtensionTypeId()
    {
        return _extensionTypeId;
    }

    public UInt16 GetBinaryTypeId()
    {
        return _binaryTypeId;
    }
}