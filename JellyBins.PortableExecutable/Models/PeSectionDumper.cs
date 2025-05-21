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
    public PeImportsDump ImportsDump(BinaryReader reader)
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
            PeSection importTableSection = Section(directories[1].VirtualAddress); // IMAGE_DATA_DIRECTORY->VA

            reader.BaseStream.Seek(Offset(directories[1].VirtualAddress), SeekOrigin.Begin); // all sections instead IMPORTS
            List<PeImportDescriptor> items = [];
            while (true)
            {
                PeImportDescriptor item = Fill<PeImportDescriptor>(reader);
                if (item.OriginalFirstThunk == 0) break;

                items.Add(item);
            }

            foreach (PeImportDescriptor item in items)
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
            foreach (PeImportDescriptor descriptor in items)
            {
                modules.Add(ReadImportDll(reader, descriptor)); // читает правильно но функция везде только одна
            }

            dump.FoundImports = modules;
        }
        catch
        {
            // ignoring
        }
        
        dump.Size = Marshal.SizeOf<PeImportDescriptor>() * dump.Segmentation.Count;
        
        return dump;
    }
    /// <param name="reader"> Current instance of <see cref="BinaryReader"/> </param>
    /// <param name="descriptor"> Seeking <see cref="PeImportDescriptor"/> table </param>
    /// <returns> Filled <see cref="ImportDll"/> instance full of module information </returns>
    private ImportDll ReadImportDll(BinaryReader reader, PeImportDescriptor descriptor)
    {
        Int64 nameOffset = Offset(descriptor.Name);
        reader.BaseStream.Seek(nameOffset, SeekOrigin.Begin);
        String dllName = ReadTStr(reader);
        Debug.WriteLine($"Processing DLL: {dllName}");

        List<ImportedFunction> functions = ReadThunk(reader, descriptor.OriginalFirstThunk, "OFT");

        if (functions.Count == 0)
        {
            functions = ReadThunk(reader, descriptor.FirstThunk, "FirstThunk");
        }

        return new ImportDll { DllName = dllName, Functions = functions };
    }
    /// <param name="reader"></param>
    /// <param name="thunkRva"></param>
    /// <param name="tag"></param>
    /// <returns></returns>
    private List<ImportedFunction> ReadThunk(BinaryReader reader, UInt32 thunkRva, String tag)
    {
        List<ImportedFunction> result = [];
        if (thunkRva == 0) 
            return result;

        try
        {
            Int64 thunkOffset = Offset(thunkRva);
            while (true)
            {
                reader.BaseStream.Seek(thunkOffset, SeekOrigin.Begin);
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
                        Name = $"@{reader.ReadUInt32()}"  // ?
                    });
                }
                else
                {
                    result.Add(new ImportedFunction
                    {
                        Name = ReadTStr(reader)
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
        foreach (PeSection section in sections)
        {
            if (rva >= section.VirtualAddress && rva < section.VirtualAddress + section.VirtualSize) 
                return section;
        }

        throw new SectionNotFoundException();
    }
    
    private String ReadTStr(BinaryReader reader)
    {
        List<Byte> bytes = [];
        Byte b;
        while ((b = reader.ReadByte()) != 0)
            bytes.Add(b);
        
        return Encoding.ASCII.GetString(bytes.ToArray());
    }
    
    private TStruct Fill<TStruct>(BinaryReader reader) where TStruct : struct
    {
        Byte[] bytes = reader.ReadBytes(Marshal.SizeOf(typeof(TStruct)));
        GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
        TStruct result = Marshal.PtrToStructure<TStruct>(handle.AddrOfPinnedObject());
        handle.Free();
        return result;
    }
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
    public UInt32? Ordinal { get; set; } = 0;
}