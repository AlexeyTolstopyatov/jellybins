using JellyBins.PortableExecutable.Models;
using JellyBins.NewExecutable.Models;

namespace HeadersTest;

public class SectionsOpeningTest
{
    [SetUp] 
    public void Setup()
    {
    }
    
    /// <summary>
    /// PASSED!!!!!!! OH GOD!!!! IVE FiNaLlY DONE IT
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
    [Test]
    public void ParseLxBinary()
    {
        
    }
}