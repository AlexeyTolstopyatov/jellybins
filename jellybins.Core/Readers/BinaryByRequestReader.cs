using System.Runtime.InteropServices;

namespace jellybins.Core.Readers;

// I hate myself
/// <summary>
/// This Entity represents operations which every time calls
/// File struct and reads required file like binary image.
///
/// In other side this is Old code base which exists like
/// very awful idea.
/// </summary>
public class BinaryByRequestReader
{
    private readonly string _fileName;
    
    public BinaryByRequestReader(string fileName)
    {
        _fileName = fileName;
    }
    /// <summary>
    /// Fills unsafe struct from start of file
    /// </summary>
    /// <param name="head">Куда писать</param>
    /// <typeparam name="TStruct">Структура с неуправляемыми типами данных</typeparam>
    public void Fill<TStruct>(ref TStruct head) where TStruct : struct
    {
        Fill(ref head, 0);
    }

    /// <summary>
    /// Returns WORD (uint16_t) from bytes
    /// </summary>
    /// <param name="offset">address of bytes</param>
    public ushort GetUInt16(int offset)
    {
        if (string.IsNullOrEmpty(_fileName))
            throw new ArgumentNullException(nameof(_fileName), "Where are you set this?!");
        byte[] destination = new byte[2]; // WORD
        using (var file = File.OpenRead(_fileName))
        {
            file.Seek(offset, SeekOrigin.Begin);
            _ = file.Read(destination, 0, destination.Length);
        }
        // 0x3212 byte[] = 32, 12 => 0x32 * 0x100 => 0x3200 + 0x12 => 0x3212
        return (ushort)(destination[0] * 0x100 + destination[1]);
    }

    /// <summary>
    /// Returns unsigned int (uint32_t) from bytes
    /// </summary>
    /// <param name="offset">address of byte</param>
    /// <returns></returns>
    public uint GetUInt32(int offset)
    {
        if (string.IsNullOrEmpty(_fileName))
            throw new ArgumentNullException(nameof(_fileName), "Where are you set this?!");

        byte[] destination = new byte[4]; // DWORD
        using (var file = File.Open(_fileName, FileMode.Open, FileAccess.Read))
        {
            file.Seek(offset, SeekOrigin.Begin); // mov ptr, offset
            file.Read(destination, 0, destination.Length); // read 4 bytes
        }
        // 0xFFAAEE11 => [FF AA EE 11]
        // => 0xFF * 0x10000 => 0xFF0000 + 0xAA00 => 0xFFAA00 => 0xFFAA00 + 0x11 => 0xFFAAEE11 
        return (uint)(destination[0] * 0x10000 + destination[1] + destination[2] * 0x100 + destination[3]);
    }

    public byte GetUInt8(int offset)
    {
        byte[] destination = new byte[1]; // DWORD
        using (var file = File.Open(_fileName, FileMode.Open, FileAccess.Read))
        {
            file.Seek(offset, SeekOrigin.Begin); // mov ptr, offset
            file.Read(destination, 0, destination.Length); // read 4 bytes
        }
        // 0xFFAAEE11 => [FF AA EE 11] =>
        // 0xFF * 0x10000 => 0xFF0000 + 0xAA00 =>
        // 0xFFAA00 => 0xFFAA00 + 0x11 => 0xFFAAEE11 
        return destination[0];
    }

    public ulong GetUInt64(int offset)
    {
        if (string.IsNullOrEmpty(_fileName))
            throw new ArgumentNullException(nameof(_fileName), "Where are you set this?!");

        byte[] destination = new byte[8]; // DWORD
        using (var file = File.Open(_fileName, FileMode.Open, FileAccess.Read))
        {
            file.Seek(offset, SeekOrigin.Begin); // mov ptr, offset
            file.Read(destination, 0, destination.Length); // read 4 bytes
        }
        
        return (ulong)(
            destination[0] * 0x10000000 + 
            destination[1] * 0x1000000 + 
            destination[2] * 0x100000 + 
            destination[3] * 0x10000 +
            destination[4] * 0x1000 +
            destination[5] * 0x100 +
            destination[6] * 0x10 +
            destination[7]); // i'm f_cking fine
    }
    
    /// <summary>
    /// Fills structure by pointer, beginning seek from currently offset
    /// </summary>
    /// <param name="head">Where write</param>
    /// <param name="offset">From which byte start</param>
    /// <typeparam name="TStruct">Unsafe struct</typeparam>
    public void Fill<TStruct>(ref TStruct head, int offset) where TStruct : struct
    {
        if (string.IsNullOrEmpty(_fileName))
            throw new ArgumentNullException(nameof(_fileName), $"Where are you set {nameof(_fileName)}?!");

        using FileStream stream = new(_fileName, FileMode.Open, FileAccess.Read);
        byte[] buffer = new byte[Marshal.SizeOf<TStruct>()];
        GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);

        stream.Seek(offset, SeekOrigin.Begin);
        stream.Read(buffer, 0, buffer.Length);

        head = (TStruct)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(TStruct))!;
        handle.Free();
    }
}