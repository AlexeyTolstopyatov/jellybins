using JellyBins.LinearExecutable.Models;
using JellyBins.PortableExecutable.Models;
using JellyBins.NewExecutable.Models;
using JellyBins.DosCommand.Models;

namespace HeadersTest;

public class SectionsOpeningTest
{
    [SetUp] 
    public void Setup()
    {
    }
    
    /// <summary>
    /// ImportProcedureNames [not supported]
    /// Exports/NonResident names [done]
    /// Sections [done]
    /// Info [done]
    /// ResidentNames [not implemented]
    /// Resources [not implemented]
    /// Characteristics [done]
    /// </summary>
    [Test]
    public void ParseNeBinary()
    {
        // debugger running
        NeFileDumper dumper = new(@"D:\Анализ файлов\inst\NE\PROGMAN.EXe");

        dumper.Dump();
        dumper.GetBinaryTypeId();
        dumper.GetExtensionTypeId();
        Assert.Pass();
    }
    /// <summary>
    /// EntryTable [not supported].
    /// ImportProcedureNamesTable [supported?]
    /// ImportModules [done]
    /// Info [done]
    /// ObjectSections [done]
    /// ResidentNames [done as Dictionary not ...Dump]
    /// Exports/NonResidentNames [done]
    /// Resources [not implemented]
    /// Characteristics [done]
    /// </summary>
    [Test]
    public void ParseLxBinary()
    {
        LeFileDumper dumper = new(@"D:\Анализ файлов\inst\LE\ecsshell.exe");
        dumper.Dump();
        Assert.Pass();
    }

    /// <summary>
    /// Characteristics [not implemented]
    /// Sections [done]
    /// Directories [done]
    /// Imports/Exports [not implemented]
    /// Runtime Word [not implemented]
    /// </summary>
    [Test]
    public void ParsePeBinary()
    {
        PeFileDumper dumper = new(@"D:\Анализ файлов\inst\PE\explorer.exe");
        // 32 bit imports reads ok
        // 64 bit imports problem
        dumper.Dump();
        Assert.Pass();
    }
    [Test]
    public void ParseCommand()
    {
        ComFileDumper dumper = new(@"D:\Анализ файлов\inst\COM\EDIT.COM");
        
        dumper.Dump();
        Assert.Pass();
    }
}