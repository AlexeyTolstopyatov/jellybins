using System.Text;

namespace JellyBins.PortableExecutable.Models;

public class PeImportsDumper
{
    // very long implementation members. Idea to make
    // self-hosted models (Classes) to making dumps of .sections
    
}

public class PeDump
{
    public static void DumpImports(String pePath, String outputPath)
    {
        using FileStream fs = new(pePath, FileMode.Open, FileAccess.Read);
        using BinaryReader reader = new(fs);
        using StreamWriter writer = new(outputPath);
        
        #region DOS/NT objects registration/skip
        if (reader.ReadUInt16() != 0x5A4D) // "MZ"
            throw new InvalidDataException("Not a valid PE file");

        fs.Position = 0x3C;
        UInt32 peHeaderOffset = reader.ReadUInt32();
        fs.Position = peHeaderOffset;
        if (reader.ReadUInt32() != 0x4550) // "PE\0\0"
            throw new InvalidDataException("PE signature not found");

        // COFF-заголовок
        fs.Position += 20; // Пропускаем Machine, NumberOfSections и другие поля
        UInt16 sizeOfOptionalHeader = reader.ReadUInt16();
        fs.Position += 112; // Переходим к Optional Header
        #endregion
        
        // Data Directories
        (UInt32 VirtualAddress, UInt32 Size)[] dataDirectories = new (UInt32 VirtualAddress, UInt32 Size)[16];
        for (Int32 i = 0; i < 16; i++)
        {
            dataDirectories[i] = (reader.ReadUInt32(), reader.ReadUInt32());
        }

        (UInt32 VirtualAddress, UInt32 Size) importTable = dataDirectories[1];
        if (importTable.VirtualAddress == 0)
        {
            writer.WriteLine("No import table found");
            return;
        }

        // Переходим к таблице импортов
        UInt32 importRva = importTable.VirtualAddress;
        Int64 importSectionOffset = RvaToFileOffset(fs, reader, importRva);

        fs.Position = importSectionOffset;
        List<ImportDll> imports = new();

        // Читаем дескрипторы импорта
        while (true)
        {
            ImageImportDescriptor descriptor = new()
            {
                OriginalFirstThunk = reader.ReadUInt32(),
                TimeDateStamp = reader.ReadUInt32(),
                ForwarderChain = reader.ReadUInt32(),
                Name = reader.ReadUInt32(),
                FirstThunk = reader.ReadUInt32()
            };

            if (descriptor.Name == 0) break;

            imports.Add(ReadImportDll(fs, reader, descriptor));
        }

        // Записываем результаты
        foreach (ImportDll import in imports)
        {
            writer.WriteLine($"{import.DllName}:");
            foreach (ImportedFunction func in import.Functions)
            {
                writer.WriteLine($"  {(func.Ordinal != null ? $"@{func.Ordinal}" : func.Name)}");
            }
        }
    }

    private static ImportDll ReadImportDll(FileStream fs, BinaryReader reader, ImageImportDescriptor descriptor)
    {
        Int64 nameOffset = RvaToFileOffset(fs, reader, descriptor.Name);
        fs.Position = nameOffset;
        String dllName = ReadNullTerminatedString(reader);

        List<ImportedFunction> functions = new();
        UInt32 thunkRva = descriptor.OriginalFirstThunk != 0 ? descriptor.OriginalFirstThunk : descriptor.FirstThunk;
        Int64 thunkOffset = RvaToFileOffset(fs, reader, thunkRva);

        while (true)
        {
            fs.Position = thunkOffset;
            UInt32 thunkData = reader.ReadUInt32();
            if (thunkData == 0) break;

            if ((thunkData & 0x80000000) != 0)
            {
                functions.Add(new ImportedFunction { Ordinal = thunkData & 0xFFFF });
            }
            else
            {
                Int64 nameAddr = RvaToFileOffset(fs, reader, thunkData + 2);
                fs.Position = nameAddr;
                functions.Add(new ImportedFunction { Name = ReadNullTerminatedString(reader) });
            }

            thunkOffset += 4;
        }

        return new ImportDll { DllName = dllName, Functions = functions };
    }

    private static Int64 RvaToFileOffset(FileStream fs, BinaryReader reader, UInt32 rva)
    {
        Int64 originalPosition = fs.Position;
        fs.Position = 0xD8; // Начало секций в Optional Header

        for (Int32 i = 0; i < 96; i++) // Максимальное количество секций для примера
        {
            UInt32 virtualAddress = reader.ReadUInt32();
            UInt32 virtualSize = reader.ReadUInt32();
            UInt32 rawOffset = reader.ReadUInt32();
            UInt32 rawSize = reader.ReadUInt32();
            fs.Position += 16; // Пропускаем остальные поля

            if (rva >= virtualAddress && rva < virtualAddress + virtualSize)
            {
                return rawOffset + (rva - virtualAddress);
            }
        }

        throw new InvalidDataException("RVA not found in any section");
    }

    private static String ReadNullTerminatedString(BinaryReader reader)
    {
        List<Byte> bytes = new();
        Byte b;
        while ((b = reader.ReadByte()) != 0)
            bytes.Add(b);
        
        return Encoding.ASCII.GetString(bytes.ToArray());
    }
}

public struct ImageImportDescriptor
{
    public UInt32 OriginalFirstThunk;
    public UInt32 TimeDateStamp;
    public UInt32 ForwarderChain;
    public UInt32 Name;
    public UInt32 FirstThunk;
}

public class ImportDll
{
    public String DllName { get; set; }
    public List<ImportedFunction> Functions { get; set; }
}

public class ImportedFunction
{
    public String Name { get; set; }
    public UInt32? Ordinal { get; set; }
}