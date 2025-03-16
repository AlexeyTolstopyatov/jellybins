using jellybins.Core.Interfaces;
using jellybins.Core.Models;

namespace jellybins.Core.Readers.COM;

public class DosCommandSectionsReader : ISectionsReader
{
    public DosCommandSectionsReader(string path)
    {
        byte[] fileData = File.ReadAllBytes(path);
        
        int i = 0;
        int codeEnd = 0;

        while (i < fileData.Length)
        {
            byte opcode = fileData[i];
            switch (opcode)
            {
                case 0xB8: // MOV AX
                    i += 3;
                    break;
                case 0xCD: // INT
                    i += 2;
                    break;
                case 0xC3: // RET
                    codeEnd = i + 1;
                    i += 1;
                    break;
                default:
                    i += 1;
                    break;
            }

            if (opcode == 0xC3)
                break;
        }

        // результаты
        List<SectionsProperties> sections = new List<SectionsProperties>()
        {
            new()
            {
                Name = ".psp",
                PointerToRawData = 0,
                SizeOfRawData = 0x100
            },
            new() 
            {
                Name = ".code",
                PointerToRawData = 0x100, 
                SizeOfRawData = (uint)Math.Abs(codeEnd - 0x100)
            },
            new()
            {
                Name = ".data",
                PointerToRawData = (uint)codeEnd, 
                SizeOfRawData = (uint)Math.Abs(codeEnd - fileData.Length)
            }
        };

        Sections = sections.ToArray();
    }
    public void ProcessImports()
    {
        Console.Error.WriteLine($"Exited {nameof(ProcessImports)}");
    }

    public void ProcessRuntime()
    {
        Console.Error.WriteLine($"Exited {nameof(ProcessRuntime)}");
    }

    public void ProcessExports()
    {
        Console.Error.WriteLine($"Exited {nameof(ProcessExports)}");
    }

    public SectionsProperties[] Sections { get; }
}