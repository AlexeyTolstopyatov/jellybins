using JellyBins.PortableExecutable.Models;

namespace HeadersTest;

public class SectionsOpeningTest
{
    [SetUp] 
    public void Setup()
    {
    }

    [Test]
    public void RichOpened()
    {
        ProgramHeaders programHeaders = new(@"D:\Анализ файлов\inst\PE\acpi.sys");

        if (programHeaders.RichHeaderFound)
            Assert.Pass(); // Tests done
        else
            Assert.Fail(); // Tests fail
    }
    [Test]
    public void Win32ValueZero()
    {
        ProgramHeaders headers = new(@"D:\Анализ файлов\inst\PE\acpi.sys");
        if (headers.Is64Bit)
        {
            if (headers.PeHeader.PeOptionalHeader.Win32VersionValue == 0)
                Assert.Pass();
            else
                Assert.Fail();
        }
        else
        {
            if (headers.PeHeader32.PeOptionalHeader.Win32VersionValue == 0)
                Assert.Pass();
            else
                Assert.Fail();
        }
        
    }
}