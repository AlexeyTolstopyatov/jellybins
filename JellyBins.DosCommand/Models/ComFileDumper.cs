using JellyBins.Abstractions;

namespace JellyBins.DosCommand.Models;

public class ComFileDumper(String path) : IFileDumper
{
    public FileSegmentationType SegmentationType { get; } = FileSegmentationType.Dos1Command;
    public ComFileInfo Info { get; private set; } = new()
    {
        Name = new FileInfo(path).Name,
        Path = path
    };
    public ComSectionDump[]? Sections { get; private set; }
    
    private UInt16 _extensionTypeId = (UInt16)FileType.Application;
    private UInt16 _binaryTypeId = (UInt16)FileType.Application;
    
    /// <summary>
    /// Automatically (from constructor) makes structure
    /// with file characteristics and information about.
    /// </summary>
    /// <returns></returns>
    public void Dump()
    {
        ComDump dump = new();
        Byte[] bytes = File.ReadAllBytes(Info.Path!);
        
        dump.Characteristics =
        [
            "image_file_no_sections".ToUpper(),
            "image_mem_tiny".ToUpper(),
            "image_file_limit".ToUpper(),
            "image_mem_psp".ToUpper(),
            "image_mem_stack".ToUpper()
        ];

        String os = "MS-DOS", arch = "Intel 8086";

        // if (FindCPmSyscall(bytes))
        // {
        //     os = "CP/M";
        //     arch = "Intel 8080";
        // }

        Info.CpuArchitecture = arch;
        Info.CpuWordLength = 16;
        Info.OperatingSystem = os;
        Info.OperatingSystemVersion = "1.0";
        
        UInt32 addr = FindDataAddress(bytes);

        Sections =
        [
            new ComSectionDump()
            {
                Name = "COM Section",
                Address = 0,
                Characteristics = ["SEC_PSP", "SEC_RESERVED"],
                Segmentation = new ComSection()
                {
                    Address = 0, Size = 0x0FF, Name = ".psp"
                }
            },
            new ComSectionDump()
            {
                Name = "COM Section",
                Address = 0x100,
                Characteristics = ["SEC_TEXT"],
                Segmentation = new ComSection()
                {
                    Address = 0x100, Size = (addr - 0x100), Name = ".text"
                }
            },
            new ComSectionDump()
            {
                Name = "COM Section",
                Address = addr - 0x100,
                Characteristics = ["SEC_DATA"],
                Segmentation = new ComSection()
                {
                    Address = addr, Size = ((UInt32)bytes.Length - addr), Name = ".data"
                }
            }
        ];
    }

    public UInt16 GetExtensionTypeId()
    {
        return _extensionTypeId;
    }

    public UInt16 GetBinaryTypeId()
    {
        return _binaryTypeId;
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