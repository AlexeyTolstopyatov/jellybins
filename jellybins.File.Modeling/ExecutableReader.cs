using System.Runtime.InteropServices;
using System.Reflection.PortableExecutable;
using jellybins.File.Modeling.Base;

namespace jellybins.File.Modeling;

public class ExecutableReader : IExecutableReader
{
    private readonly string _fileName;
    
    public ExecutableReader(string fileName)
    {
        _fileName = fileName;
    }
    
    /// <summary>
    /// Читает файл и записывает в структуру
    /// </summary>
    /// <param name="head">Куда писать</param>
    /// <typeparam name="TStruct">Структура с неуправляемыми типами данных</typeparam>
    public void Fill<TStruct>(ref TStruct head) where TStruct : struct
    {
        Fill(ref head, 0);
    }

    /// <summary>
    /// Возвращает СЛОВО сравниваемого типа из файла
    /// </summary>
    /// <param name="offset">Откуда начать читать</param>
    public ushort GetUInt16(int offset)
    {
        byte[] destination = new byte[2]; // WORD
        using (var file = System.IO.File.Open(_fileName, FileMode.Open, FileAccess.Read))
        {
            file.Seek(offset, SeekOrigin.Begin);
            _ = file.Read(destination, 0, destination.Length);
        }
        // 0x3212 byte[] = 32, 12 => 0x32 * 0x100 => 0x3200 + 0x12 => 0x3212
        return (ushort)(destination[0] * 0x100 + destination[1]);
    }

    /// <summary>
    /// Возвращает 32 разрядное слово
    /// </summary>
    /// <param name="offset">Откуда начать читать</param>
    /// <returns></returns>
    public uint GetUInt32(int offset)
    {
        byte[] destination = new byte[4]; // DWORD
        using (var file = System.IO.File.Open(_fileName, FileMode.Open, FileAccess.Read))
        {
            file.Seek(offset, SeekOrigin.Begin); // mov ptr, offset
            file.Read(destination, 0, destination.Length); // read 4 bytes
        }
        // 0xFFAAEE11 => [FF AA EE 11] => 0xFF * 0x10000 => 0xFF0000 + 0xAA00 => 0xFFAA00 => 0xFFAA00 + 0x11 => 0xFFAAEE11 
        return (uint)(destination[0] * 0x10000 + destination[1] + (destination[2] * 0x100) + destination[3]);
    }

    public byte GetUInt8(int offset)
    {
        byte[] destination = new byte[1]; // DWORD
        using (var file = System.IO.File.Open(_fileName, FileMode.Open, FileAccess.Read))
        {
            file.Seek(offset, SeekOrigin.Begin); // mov ptr, offset
            file.Read(destination, 0, destination.Length); // read 4 bytes
        }
        // 0xFFAAEE11 => [FF AA EE 11] => 0xFF * 0x10000 => 0xFF0000 + 0xAA00 => 0xFFAA00 => 0xFFAA00 + 0x11 => 0xFFAAEE11 
        return destination[0];
    }
    
    /// <summary>
    /// Заполняет структуру данными из файла по указанным параметрам
    /// </summary>
    /// <param name="head">Куда писать</param>
    /// <param name="offset">С какого байта читать</param>
    /// <typeparam name="TStruct">Тип нетипобезопасной структуры</typeparam>
    public void Fill<TStruct>(ref TStruct head, int offset) where TStruct : struct
    {
        using FileStream stream = new(_fileName, FileMode.Open, FileAccess.Read);
        byte[] buffer = new byte[Marshal.SizeOf<TStruct>()];
        GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);

        stream.Seek(offset, SeekOrigin.Begin);
        stream.Read(buffer, 0, buffer.Length);

        head = (TStruct)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(TStruct))!;
        handle.Free();
    }

}