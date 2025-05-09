using JellyBins.Abstractions;

namespace JellyBins.DosCommand.Models;

public class ComReportMaker : IReportMaker
{
    public ComReport? Report { get; private set; }

    public Task MakeAsync(String fullName)
    {
        ComReport instance = new();
        Byte[] bytes = File.ReadAllBytes(fullName);
        
        instance.Flags =
        [
            "image_file_no_sections".ToUpper(),
            "image_mem_tiny".ToUpper(),
            "image_file_limit".ToUpper(),
            "image_mem_psp".ToUpper(),
            "image_mem_stack".ToUpper()
        ];

        String os = "MS-DOS", arch = "I8086";

        if (FindCPmSyscall(bytes))
        {
            os = "CP/M";
            arch = "I8080";
        }
        
        instance.Data = new ComData()
        {
            CpuArchitecture = arch,
            CpuWordLength = 16,
            Name = new FileInfo(fullName).Name,
            OperatingSystem = os,
            OperatingSystemVersion = "1.0",
            Path = fullName,
        };

        UInt32 addr = FindDataAddress(bytes);
        
        instance.Sections =
        [
            new ComSection {Address = 0, Size = 0x0FF, Name = ".psp"},
            new ComSection {Address = 0x100, Size = (addr - 0x100), Name = ".text"},
            new ComSection {Address = addr, Size = ((UInt32)bytes.Length - addr), Name = ".data"},
            new ComSection {Address = (UInt32)bytes.Length, Size = 0, Name = ".overlay"}
        ];
        
        Report = instance;
        return Task.CompletedTask;
    }
    
    private static Boolean FindCPmSyscall(Byte[] bytes)
    {
        Int32 i = 0;
        while (i < bytes.Length)
        {
            Byte opcode = bytes[i];

            if (opcode == 0xCD)
            {
                if ((i + 2) < bytes.Length && bytes[i + 1] == 0x05 && bytes[i + 2] == 0x00)
                {
                    return true;
                }
            }
            else
            {
                i += 1;
            }

        }
        return false;
    }
    
    private static UInt32 FindDataAddress(Byte[] bytes)
    {
        UInt32 i = 0;
        UInt32 end = 0;

        while (i < bytes.Length)
        {
            Byte opcode = bytes[i];
            switch (opcode)
            {
                case 0xB8: // MOV AX
                    i += 3;
                    break;
                case 0xCD: // INT
                    i += 2;
                    break;
                case 0xC3: // RET
                    end = i + 1;
                    i += 1;
                    break;
                default:
                    i += 1;
                    break;
            }

            if (opcode == 0xC3)
                break;
        }

        return end;
    }
}