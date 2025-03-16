using System.Runtime.InteropServices;
using jellybins.Core.Headers;
using jellybins.Core.Interfaces;
using jellybins.Core.Models;

namespace jellybins.Core.Readers.MarkZbykowski;

public class MarkZbikowskiSectionsReader : ISectionsReader
{
    private readonly byte[]? _code;
    public MarkZbikowskiSectionsReader(string fileName)
    {
        using FileStream stream = new(fileName, FileMode.Open, FileAccess.Read);
        using BinaryReader reader = new(stream);
        
        byte[] headerBytes = reader.ReadBytes(Marshal.SizeOf(typeof(MarkZbikowski)));
        MarkZbikowski header = ByteArrayToStructure<MarkZbikowski>(headerBytes);
        
        // самому найти логические секции
        int headerSize = header.e_pars * 16; // 1 paragraph = 16 bytes
        int codeSize = (header.e_fbl * 512) - headerSize; // .code size + .data
        int overlayOffset = header.e_fbl * 512; // .overlay
        List<SectionsProperties> propertiesList = new();
        
        // .CODE 
        propertiesList.Add(new SectionsProperties()
        {
            Name = ".text",
            NumberOfRelocations = 0,
            PointerToRelocations = 0,
            Characteristics = new []{"null"},
            PointerToRawData = (uint)headerSize,
            SizeOfRawData = (uint)codeSize,
            VirtualAddress = 0,
            VirtualSize = 0
        });
        // .DATA
        // Предположительно, начинается после кода (ЭТО ТОЖЕ гадание на кофейной гуще!)
        // В реальности это зависит от программы.
        propertiesList.Add(new SectionsProperties()
        {
            Name = ".data",
            NumberOfRelocations = 0,
            PointerToRelocations = 0,
            Characteristics = new []{"null"},
            PointerToRawData = (uint)(headerSize + codeSize),
            SizeOfRawData = (uint)codeSize,
            VirtualAddress = 0,
            VirtualSize = 0
        });

        // .STACK
        propertiesList.Add(new SectionsProperties()
        {
            Name = ".stack",
            NumberOfRelocations = 0,
            PointerToRelocations = 0,
            Characteristics = new []{"null"},
            PointerToRawData = header.sp,
            SizeOfRawData = header.ss,
            VirtualAddress = 0,
            VirtualSize = 0
        });
        // .OVERLAY (если нет -> игнорировать)
        if (new FileInfo(fileName).Length <= overlayOffset) goto _saveChanges;
        
        propertiesList.Add(new SectionsProperties()
        {
            Name = ".overlay",
            NumberOfRelocations = 0,
            PointerToRelocations = 0,
            Characteristics = new []{"null"},
            PointerToRawData = (uint)overlayOffset,
            SizeOfRawData = (uint)(new FileInfo(fileName).Length - overlayOffset),
            VirtualAddress = 0,
            VirtualSize = 0
        });
        _saveChanges:
        Sections = propertiesList.ToArray();
        _code = File.ReadAllBytes(fileName);
    }
    private static T ByteArrayToStructure<T>(byte[] bytes) where T : struct
    {
        GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
        T structure = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T))!;
        handle.Free();
        return structure;
    }
    
    public void ProcessImports(/*byte[] code*/)
    {
        // Поиск вызовов DOS API (int 21h)
        for (int i = 0; i < _code!.Length - 1; i++)
        {
            if (_code[i] == 0xCD && _code[i + 1] == 0x21) // int 21h
            {
                Console.WriteLine($"DOS API call: 0x{i:x}");
            }
        }
    }

    public void ProcessRuntime()
    {
        Console.WriteLine($"{nameof(ProcessRuntime)}: Exited");
    }

    public void ProcessExports()
    {
        // Поиск точек входа (например, по сигнатурам или вызовам)
        for (int i = 0; i < _code!.Length - 2; i++)
        {
            if (_code[i] == 0xE8 && _code[i + 1] == 0x00 && _code[i + 2] == 0x00) // call near
            {
                Console.WriteLine($"exported API call: 0x{i:x}");
            }
        }
    }
    public SectionsProperties[] Sections { get; private set; }
}