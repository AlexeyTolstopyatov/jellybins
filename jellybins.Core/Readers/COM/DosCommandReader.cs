using System.Security.Cryptography;
using jellybins.Core.Interfaces;
using jellybins.Core.Models;
using jellybins.Core.Strings;

namespace jellybins.Core.Readers.COM;

/// <summary>
/// Represents Reader for xx-DOS .COM files.
/// NOT Component Object Model assemblies.
/// </summary>
public class DosCommandReader : IReader
{
    private byte[] _code;
    public DosCommandReader(string path)
    {
        // save bytes.
        _code = File.ReadAllBytes(path);
    }
    
    public Dictionary<string, string> GetHeader()
    {
        Console.Error.WriteLine("No header for .COM files!");
        return new Dictionary<string, string>();
    }

    public Dictionary<string, string[]> GetFlags()
    {
        return new()
        {
            { 
                "_allknown", 
                new[]
                {
                    "image_file_no_sections".ToUpper(), 
                    "image_mem_tiny".ToUpper(), 
                    "image_file_limit".ToUpper(), 
                    "image_mem_psp".ToUpper(), 
                    "image_mem_stack".ToUpper()
                } 
            }
        };
    }

    public CommonProperties GetProperties()
    {
        // detect required Architecture
        DosCommandDisassembler asm = new();

        (string CPU, string OS) vector = asm
            .DetermineSystemCalls(_code);
        
        // гадание на кофейной гуще...
        // fixme: я где-то здесь проебался. Это надо исправить срочно. OS != OS
        return new CommonProperties()
        {
            CpuArchitecture = vector.CPU,
            OperatingSystem = vector.OS,
            OperatingSystemVersion = (vector.OS == "Microsoft DOS") ? "1.0" : "2.2",
            Subsystem = PortableExecutableSubsystem.NtVirtualDosMachine.ToString(),
            CpuWordLength = (vector.CPU == "Intel 8080") ? "8" : "16",
            ImageType = "Application",
            LinkerVersion = "0",
            RuntimeWord = (vector.OS == "Microsoft DOS" ? "DOS API" : "CP/M API")
        };
    }

    public Dictionary<string, string> GetImports()
    {
        return new Dictionary<string, string>();
    }
}