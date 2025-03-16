using System.Runtime.InteropServices;
using jellybins.Core.Headers;
using jellybins.Core.Interfaces;
using jellybins.Core.Models;

namespace jellybins.Core.Readers.MarkZbykowski;

public class MarkZbikowskiSectionsReader : ISectionsReader
{
    private byte[]? _code;
    public MarkZbikowskiSectionsReader(string fileName)
    {
        using FileStream stream = new(fileName, FileMode.Open, FileAccess.Read);
        using BinaryReader reader = new(stream);
        byte[] headerBytes = reader.ReadBytes(Marshal.SizeOf(typeof(MarkZbikowski)));
        MarkZbikowski header = ByteArrayToStructure<MarkZbikowski>(headerBytes);
        
        // determine logical sections
        int headerSize = header.e_pars * 16; // 1 paragraph = 16 bytes
        int codeSize = (header.e_fbl * 512) - headerSize; // .code size + .data
        int overlayOffset = header.e_fbl * 512; // .overlay
        List<SectionsProperties> propertiesList = new();
        
        // .CODE 
        Console.WriteLine("Логическая секция CODE:");
        Console.WriteLine($"  Начало: 0x{headerSize:x}");
        Console.WriteLine($"  Размер: 0x{codeSize:x} байт");
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
        // Предположительно, начинается после кода (эвристика!)
        // В реальности это зависит от программы!
        Console.WriteLine("\nЛогическая секция DATA (приблизительно):");
        Console.WriteLine($"  Начало: 0x{headerSize + codeSize:x}");
        Console.WriteLine($"  Размер: определяется во время выполнения (нет в файле)");
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

        // STACK задан в заголовке, но не хранится в файле
        Console.WriteLine("\nЛогическая секция STACK (в памяти):");
        Console.WriteLine($"  SS:SP = 0x{header.ss:x}:0x{header.sp:x}");
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
        // .OVERLAY (если нет)
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
        
        Console.WriteLine("\nOverlay:");
        Console.WriteLine($"  Начало: 0x{overlayOffset:x}");
        Console.WriteLine($"  Размер: 0x{new FileInfo(fileName).Length - overlayOffset:x} байт");
        
        _saveChanges:
        Sections = propertiesList.ToArray();
        _code = File.ReadAllBytes(fileName);
    }
    private static T ByteArrayToStructure<T>(byte[] bytes) where T : struct
    {
        var handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
        var structure = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T))!;
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
                Console.WriteLine($"Найден вызов DOS API по смещению 0x{i:x}");
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
                Console.WriteLine($"Найден вызов функции по смещению 0x{i:x}");
            }
        }
    }
    public SectionsProperties[] Sections { get; private set; }
}