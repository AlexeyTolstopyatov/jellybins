using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using JellyBins.PortableExecutable.Exceptions;
using JellyBins.PortableExecutable.Headers;

namespace JellyBins.PortableExecutable.Models;

public class PeSectionDumper(PeDirectory[] directories, PeSection[] sections)
{
    /// <summary> Deserializes bytes segment to import entries table </summary>
    /// <param name="reader">your content reader instance</param>
    /// <returns> Done <see cref="PeImportsDump"/> structure </returns>
    public PeImportsDump Imports32Dump(BinaryReader reader)
    {
        PeImportsDump dump = new() 
        {
            Name = "PE Imports (JellyBins: IMAGE_IMPORT_DIRECTORY_TABLE)",
            Size = 0,
            Address = 0,
            Segmentation = []
        };

        try
        {
            reader.BaseStream.Seek(Offset(directories[1].VirtualAddress), SeekOrigin.Begin); // all sections instead IMPORTS
            List<PeImportDescriptor32> items = [];
            while (true)
            {
                PeImportDescriptor32 item = Fill<PeImportDescriptor32>(reader);
                if (item.OriginalFirstThunk == 0) break;

                items.Add(item);
            }

            foreach (PeImportDescriptor32 item in items)
            {
                reader.BaseStream.Seek(Offset(item.Name), SeekOrigin.Begin);
                Byte[] name = [];
                while (true)
                {
                    Byte b = Fill<Byte>(reader);
                    if (b == 0) break;

                    Byte[] dllName = new Byte[name.Length + 1];
                    name.CopyTo(dllName, 0);
                    dllName[name.Length] = b;
                    name = dllName;
                } // читает правильно

                Debug.WriteLine(Encoding.ASCII.GetString(name));
            }

            List<ImportDll> modules = [];
            foreach (PeImportDescriptor32 descriptor in items)
            {
                modules.Add(ReadImportDll32(reader, descriptor));
            }

            dump.FoundImports = modules;
        }
        catch
        {
            // ignoring
        }
        
        dump.Size = Marshal.SizeOf<PeImportDescriptor32>() * dump.Segmentation.Count;
        
        return dump;
    }
    /// <summary> Deserializes bytes segment to import entries table </summary>
    /// <param name="reader">your content reader instance</param>
    /// <returns> Done <see cref="PeImportsDump"/> structure </returns>
    public PeImportsDump Imports64Dump(BinaryReader reader)
    {
        PeImportsDump dump = new() 
        {
            Name = "PE Imports (JellyBins: IMAGE_IMPORT_DIRECTORY_TABLE)",
            Size = 0,
            Address = 0,
            Segmentation = []
        };
        
        if (directories[1].Size == 0 || directories[1].VirtualAddress == 0)
            return dump;
        
        try
        {
            reader.BaseStream.Seek(Offset(directories[1].VirtualAddress),
                SeekOrigin.Begin); // all sections instead IMPORTS
            List<PeImportDescriptor64> items = [];
            while (true)
            {
                PeImportDescriptor64 item = Fill<PeImportDescriptor64>(reader);
                if (item.OriginalFirstThunk == 0) 
                    break;

                items.Add(item);
            }

            foreach (PeImportDescriptor64 item in items)
            {
                reader.BaseStream.Seek(Offset((Int32)(item.Name)), SeekOrigin.Begin);
                Byte[] name = [];
                while (true)
                {
                    Byte b = Fill<Byte>(reader);
                    if (b == 0) break;

                    Byte[] dllName = new Byte[name.Length + 1];
                    name.CopyTo(dllName, 0);
                    dllName[name.Length] = b;
                    name = dllName;
                }

                Debug.WriteLine(Encoding.ASCII.GetString(name));
            }

            List<ImportDll> modules = [];
            foreach (PeImportDescriptor64 descriptor in items)
            {
                modules.Add(ReadImportDll64(reader, descriptor));
            }

            dump.FoundImports = modules;
        }
        catch
        {
            // ignoring
        }
        
        dump.Size = Marshal.SizeOf<PeImportDescriptor32>() * dump.Segmentation.Count;

        return dump;
    }
    /// <param name="reader"> Current instance of <see cref="BinaryReader"/> </param>
    /// <param name="descriptor32"> Seeking <see cref="PeImportDescriptor32"/> table </param>
    /// <returns> Filled <see cref="ImportDll"/> instance full of module information </returns>
    private ImportDll ReadImportDll32(BinaryReader reader, PeImportDescriptor32 descriptor32)
    {
        Int64 nameOffset = Offset(descriptor32.Name);
        reader.BaseStream.Seek(nameOffset, SeekOrigin.Begin);
        String dllName = ReadTStr(reader);
        Debug.WriteLine($"Processing DLL: {dllName}");

        List<ImportedFunction> oft = 
            ReadThunk32(reader, descriptor32.OriginalFirstThunk, "OriginalFirstThunk");

        List<ImportedFunction> ft = 
            ReadThunk32(reader, descriptor32.FirstThunk, "FirstThunk");

        List<ImportedFunction> functions = [];
        functions.AddRange(oft);
        functions.AddRange(ft);

        return new ImportDll { DllName = dllName, Functions = functions };
    }
    /// <param name="reader"> Current instance of <see cref="BinaryReader"/> </param>
    /// <param name="descriptor64"> Seeking <see cref="PeImportDescriptor32"/> table </param>
    /// <returns> Filled <see cref="ImportDll"/> instance full of module information </returns>
    private ImportDll ReadImportDll64(BinaryReader reader, PeImportDescriptor64 descriptor64)
    {
        Int64 nameOffset = Offset((Int64)descriptor64.Name);
        reader.BaseStream.Seek(nameOffset, SeekOrigin.Begin);
        String dllName = ReadTStr(reader);
        Debug.WriteLine($"Processing DLL: {dllName}");

        List<ImportedFunction> oft = 
            ReadThunk64(reader, descriptor64.OriginalFirstThunk, "OriginalFirstThunk");

        List<ImportedFunction> ft = 
            ReadThunk64(reader, descriptor64.FirstThunk, "FirstThunk");

        List<ImportedFunction> functions = [];
        functions.AddRange(oft);
        functions.AddRange(ft);

        return new ImportDll { DllName = dllName, Functions = functions };
    }
    /// <summary> Use it when application requires 32bit machine WORD </summary>
    /// <param name="reader"><see cref="BinaryReader"/> instance</param>
    /// <param name="thunkRva">RVA of procedures block</param>
    /// <param name="tag">debug information (#debug only)</param>
    /// <returns>List of imported functions</returns>
    private List<ImportedFunction> ReadThunk32(BinaryReader reader, UInt32 thunkRva, String tag)
    {
        List<ImportedFunction> result = [];
        if (thunkRva == 0) 
            return result;
        try
        {
            Int64 thunkOffset = Offset(thunkRva); // chunk = 0
            while (true)
            {
                reader.BaseStream.Position = thunkOffset;
                UInt32 thunkData = reader.ReadUInt32();
                if (thunkData == 0) 
                    break;

                Int64 nameAddr = Offset(thunkData);
                reader.BaseStream.Position = nameAddr;
                
                UInt16 hint = reader.ReadUInt16();

                if ((thunkData & 0x80000000) != 0)
                {
                    result.Add(new ImportedFunction()
                    {
                        Name = $"@{thunkData & 0xFFFF}",
                        Ordinal = thunkData & 0xFFFF,
                        Hint = hint
                    });
                }
                else
                {
                    result.Add(new ImportedFunction
                    {
                        Name = ReadTStr(reader),
                        Hint = hint
                    });
                }
                thunkOffset += 4;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"{tag} error: {ex.Message}");
        }
        
        return result;
    }
    /// <summary> Use it when Application requires 64bit machine WORD </summary>
    /// <param name="reader"><see cref="BinaryReader"/> instance</param>
    /// <param name="thunkRva">RVA of procedures block</param>
    /// <param name="tag">debug information (#debug only)</param>
    /// <returns>List of imported functions</returns>
    private List<ImportedFunction> ReadThunk64(BinaryReader reader, UInt64 thunkRva, String tag)
    {
        List<ImportedFunction> result = [];
        if (thunkRva == 0) 
            return result;
        try
        {
            Int64 thunkOffset = Offset((Int64)thunkRva);
            while (true)
            {
                reader.BaseStream.Position = thunkOffset;
                UInt64 thunkData = reader.ReadUInt64();
                if (thunkData == 0) 
                    break;

                Int64 nameAddr = Offset((Int64)thunkData);
                reader.BaseStream.Position = nameAddr;
                
                UInt16 hint = reader.ReadUInt16();

                if ((thunkData & 0x8000000000000000) != 0)
                {
                    result.Add(new ImportedFunction()
                    {
                        Name = $"@{thunkData & 0xFFFF}",
                        Ordinal = thunkData & 0xFFFF,
                        Hint = hint
                    });
                }
                else
                {
                    result.Add(new ImportedFunction
                    {
                        Name = ReadTStr(reader),
                        Hint = hint
                    });
                }
                
                thunkOffset += 8;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"{tag} error: {ex.Message}");
        }
        
        return result;
    }
    /// <param name="rva"> Required RVA </param>
    /// <returns> File offset from RVA of selected section </returns>
    /// <exception cref="SectionNotFoundException"> If RVA not belongs to any section </exception>
    private Int64 Offset(Int64 rva)
    {
        PeSection section = Section(rva);
        
        return 0 + section.PointerToRawData + (rva - section.VirtualAddress);
    }
    /// <param name="rva"> Required relative address </param>
    /// <returns> <see cref="PeSection"/> Which RVA belongs </returns>
    /// <exception cref="SectionNotFoundException"> If RVA not belongs to any section </exception>
    private PeSection Section(Int64 rva)
    {
        // RVA в PE всегда 32-битное, преобразуем к uint
        UInt32 rva32 = Convert.ToUInt32(rva); // instead casting
        foreach (PeSection section in sections.OrderBy(s => s.VirtualAddress))
        {
            if (rva32 >= section.VirtualAddress && 
                rva32 < section.VirtualAddress + section.VirtualSize)
            {
                return section;
            }
        }
        throw new SectionNotFoundException();
    }
    /// <param name="reader"> <see cref="BinaryReader"/> instance </param>
    /// <returns> ASCIIZ typed string <c>TSTR</c> </returns>
    private String ReadTStr(BinaryReader reader)
    {
        List<Byte> bytes = [];
        Byte b;
        while ((b = reader.ReadByte()) != 0)
            bytes.Add(b);
        
        return Encoding.ASCII.GetString(bytes.ToArray());
    }
    /// <param name="reader"> <see cref="BinaryReader"/> instance </param>
    /// <typeparam name="TStruct"> Structure type </typeparam>
    /// <returns> Deserialized structure by pointer </returns>
    private TStruct Fill<TStruct>(BinaryReader reader) where TStruct : struct
    {
        Byte[] bytes = reader.ReadBytes(Marshal.SizeOf(typeof(TStruct)));
        GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
        TStruct result = Marshal.PtrToStructure<TStruct>(handle.AddrOfPinnedObject());
        handle.Free();
        return result;
    }
    /// <param name="structure">Structure</param>
    /// <typeparam name="TStruct">Structure type</typeparam>
    /// <returns>Size of required structure</returns>
    private Int32 SizeOf<TStruct>(TStruct structure) where TStruct : struct
    {
        return Marshal.SizeOf(structure);
    }
}

public class ImportDll
{
    public String DllName { get; set; } = String.Empty;
    public List<ImportedFunction> Functions { get; set; } = [];
}

public class ImportedFunction
{
    public String Name { get; set; } = String.Empty;
    public UInt64 Ordinal { get; set; }
    public UInt16 Hint { get; set; }
}