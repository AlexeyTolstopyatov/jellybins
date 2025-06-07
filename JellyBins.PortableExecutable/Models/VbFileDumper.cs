using JellyBins.Abstractions;
using JellyBins.PortableExecutable.Headers;

namespace JellyBins.PortableExecutable.Models;

/// <summary>
/// Part file's Static Analyser. Main goal: determine
/// specific VB machine structures and other metadata
/// (return filled structures and extracted API)
/// </summary>
public class VbFileDumper : IFileDumper
{
    private PeSection[] Sections { get; set; } = [];
    private PeDirectory[] Directories { get; set; } = [];

    public Boolean IsOldVb4Linked { get; private set; }
    public Boolean IsVb4Linked { get; private set; }
    public Boolean IsVb3Linked { get; private set; }
    public Boolean IsVb5Linked { get; private set; }
    public Boolean IsVb6Linked { get; private set; }
    
    /// <summary> Runs automatically when constructor calls </summary>
    public void Dump()
    {
        throw new NotImplementedException();
    }

    /// <exception cref="NotImplementedException"> Don't need in this context </exception>
    public UInt16 GetExtensionTypeId()
    {
        throw new NotImplementedException();
    }
    
    /// <exception cref="NotImplementedException"> Don't need in this context </exception>
    public UInt16 GetBinaryTypeId()
    {
        throw new NotImplementedException();
    }

    public FileSegmentationType SegmentationType { get; } = 
        FileSegmentationType.PortableExecutable;
}