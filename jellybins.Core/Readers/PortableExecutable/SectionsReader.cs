using System.Runtime.InteropServices;
using jellybins.Core.Exceptions;
using jellybins.Core.Headers;
using jellybins.Core.Sections;

namespace jellybins.Core.Readers.PortableExecutable;

public class SectionsReader
{
    private string _path;
    
    public SectionsReader(string path)
    {
        _path = path;
    }

    public Task ReadAsync()
    {
        using (FileStream stream = new(_path, FileMode.Open, FileAccess.Read))
        using (BinaryReader reader = new(stream))
        {
            MarkZbikowski dosHeader = ReadStruct<MarkZbikowski>(reader);
            if (dosHeader.e_lfanew != 0x5A4D)
            {
                throw new InvalidOperationException("Invalid DOS signature");
            }

            stream.Seek(dosHeader.e_lfanew, SeekOrigin.Begin);
            PortableExecutable32 ntHeaders = ReadStruct<PortableExecutable32>(reader);
            if (ntHeaders.Signature != 0x00004550)
            {
                throw new InvalidOperationException("Invalid NT signature");
            }

            switch (ntHeaders.WinNtOptional.Magic)
            {
                case 0x10b:
                    // ok, going deeper
                    ParseImports32(reader, ntHeaders.WinNtOptional.ImportTable);
                    break;
                case 0x20b:
                {
                    // reinit struct.
                    PortableExecutable64 ntHeaders64 = ReadStruct<PortableExecutable64>(reader);
                    ParseImports64(reader, ntHeaders64.WinNtOptional.ImportTable);
                    break;
                }
                default: throw new ImageTypeException();
            }
        }
        return Task.CompletedTask;
    }
    
    private static T ReadStruct<T>(BinaryReader reader) where T : struct
    {
        byte[] bytes = reader.ReadBytes(Marshal.SizeOf(typeof(T)));
        GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
        T result = Marshal.PtrToStructure<T>(handle.AddrOfPinnedObject());
        handle.Free();
        return result;
    }

    private static string ReadNullTerminatedString(BinaryReader reader)
    {
        var bytes = new List<byte>();
        byte b;
        while ((b = reader.ReadByte()) != 0)
        {
            bytes.Add(b);
        }
        return System.Text.Encoding.ASCII.GetString(bytes.ToArray());
    }

    private static uint ReadUInt32(BinaryReader reader)
    {
        return reader.ReadUInt32();
    }

    private static ulong ReadUInt64(BinaryReader reader)
    {
        return reader.ReadUInt64();
    }
    
    private static void ParseImports32(BinaryReader reader, Data importDirectory)
    {
        if (importDirectory.VirtualAddress == 0 || importDirectory.Size == 0)
        {
            // return value
            return;
        }

        reader.BaseStream.Seek(importDirectory.VirtualAddress, SeekOrigin.Begin);
        ImportDescriptor importDescriptor = ReadStruct<ImportDescriptor>(reader);

        while (importDescriptor.Name != 0)
        {
            reader.BaseStream.Seek(importDescriptor.Name, SeekOrigin.Begin);
            string dllName = ReadNullTerminatedString(reader);

            Console.WriteLine($"DLL: {dllName}");

            reader.BaseStream.Seek(importDescriptor.OriginalFirstThunk, SeekOrigin.Begin);
            uint thunk = ReadUInt32(reader);

            while (thunk != 0)
            {
                if ((thunk & 0x80000000) != 0)
                {
                    // Ordinal import
                    ushort ordinal = (ushort)(thunk & 0xFFFF);
                    Console.WriteLine($"  Ordinal: {ordinal}");
                }
                else
                {
                    // Name import
                    reader.BaseStream.Seek(thunk, SeekOrigin.Begin);
                    string functionName = ReadNullTerminatedString(reader);
                    Console.WriteLine($"  Function: {functionName}");
                }

                thunk = ReadUInt32(reader);
            }

            importDescriptor = ReadStruct<ImportDescriptor>(reader);
        }
    }

    private static void ParseImports64(BinaryReader reader, Data importDirectory)
    {
        if (importDirectory.VirtualAddress == 0 || importDirectory.Size == 0)
        {
            Console.WriteLine("No import table found.");
            return;
        }

        reader.BaseStream.Seek(importDirectory.VirtualAddress, SeekOrigin.Begin);
        var importDescriptor = ReadStruct<ImportDescriptor>(reader);

        while (importDescriptor.Name != 0)
        {
            reader.BaseStream.Seek(importDescriptor.Name, SeekOrigin.Begin);
            string dllName = ReadNullTerminatedString(reader);

            Console.WriteLine($"DLL: {dllName}");

            reader.BaseStream.Seek(importDescriptor.OriginalFirstThunk, SeekOrigin.Begin);
            ulong thunk = ReadUInt64(reader);

            while (thunk != 0)
            {
                if ((thunk & 0x8000000000000000) != 0)
                {
                    // Ordinal import
                    ushort ordinal = (ushort)(thunk & 0xFFFF);
                    Console.WriteLine($"  Ordinal: {ordinal}");
                }
                else
                {
                    // Name import
                    reader.BaseStream.Seek((long)thunk, SeekOrigin.Begin);
                    string functionName = ReadNullTerminatedString(reader);
                    Console.WriteLine($"  Function: {functionName}");
                }

                thunk = ReadUInt64(reader);
            }

            importDescriptor = ReadStruct<ImportDescriptor>(reader);
        }
    }

}