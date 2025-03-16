using System.Runtime.InteropServices;
using jellybins.Core.Attributes;
using jellybins.Core.Headers;
using jellybins.Core.Interfaces;
using jellybins.Core.Models;
using jellybins.Core.Readers.MarkZbykowski;
using FileStream = System.IO.FileStream;

namespace jellybins.Core.Readers.PortableExecutable;

/// <summary>
/// My first try to read imports table from
/// Microsoft Object (Portable) files
/// </summary>
public class PortableExecutableSectionsReader : ISectionsReader
{
    private readonly string _path;
    private MarkZbikowski _dosHeader;
    private PortableExecutable32 _portableHeader32;
    private PortableExecutable64 _portableHeader64;
    private SectionsProperties[]? _sections;
    private PortableExecutableDataDirectory[]? _directories;
    
    #region ISectionReader
    public void ProcessImports()
    {
        throw new NotImplementedException();
    }

    public void ProcessRuntime()
    {
        throw new NotImplementedException();
    }

    public void ProcessExports()
    {
        throw new NotImplementedException();
    }
    #endregion
    
    public SectionsProperties[] Sections => _sections!;
    public PortableExecutableDataDirectory[]? Directories => _directories;
    
    /// <summary>
    /// Calls <see cref="ReadAsync"/> to fill common
    /// Sections table information.
    /// </summary>
    /// <param name="path"></param>
    public PortableExecutableSectionsReader(string path)
    {
        _path = path;
        _dosHeader = new MarkZbikowski();
        _directories = _portableHeader32.WinNtOptional.Directories;
        ReadAsync();
    }
    
    /// <summary>
    /// Reads information about SectionTable and saves it
    /// </summary>
    /// <returns></returns>
    [UnderConstruction("Attempts to implement")]
    private void ReadAsync()
    {
        string filePath = _path;

        using FileStream fs = new(filePath, FileMode.Open, FileAccess.Read);
        using BinaryReader reader = new(fs);
        
        // IMAGE_DOS_HEADER
        MarkZbikowski dosHeader = ReadStruct<MarkZbikowski>(reader);
            
        // MZ check
        if (dosHeader.e_sign != 0x5A4D)
        {
            Console.WriteLine("Неверный DOS заголовок");
            return;
        }

        // move to PE00
        fs.Position = dosHeader.e_lfanew;
            
        // Чтение сигнатуры PE
        uint peSignature = reader.ReadUInt32();
        if (peSignature != 0x4550 && peSignature != 0x5045)
        {
            Console.WriteLine("Неверный PE заголовок");
            return;
        }
        
        // IMAGE_FILE_HEADER
        PortableFileHeader fileHeader = ReadStruct<PortableFileHeader>(reader);
            
        // IMAGE_OPTIONAL_HEADER_32
        OptionalPortableHeader32 optionalHeader = ReadStruct<OptionalPortableHeader32>(reader);
        if (optionalHeader.Magic == 0x20b) // PE32+
        {
            reader.BaseStream.Seek(dosHeader.e_lfanew, SeekOrigin.Begin);
            _portableHeader64 = ReadStruct<PortableExecutable64>(reader);
        }
        else
        {
            reader.BaseStream.Seek(dosHeader.e_lfanew, SeekOrigin.Begin);
            _portableHeader32 = ReadStruct<PortableExecutable32>(reader);
        }
        
        // IMAGE_SECTION_HEADER sections[16?]
        var sections = new List<SectionsProperties>();
        
        for (int i = 0; i < fileHeader.NumberOfSections; i++)
        {
            PortableExecutableDataSection section = ReadStruct<PortableExecutableDataSection>(reader);
            SectionsProperties model = new();
                
            string sectionName = System.Text.Encoding.ASCII
                .GetString(section.Name)
                .TrimEnd('\0')
                .Where(c => char.IsLetterOrDigit(c) || char.IsPunctuation(c) || char.IsSymbol(c))
                .Aggregate("", (current, c) => current + c);
                
            model.Name = sectionName;
            model.VirtualAddress = section.VirtualAddress;
            model.VirtualSize = section.VirtualSize;
            model.NumberOfRelocations = section.NumberOfRelocations;
            model.NumberOfLineNumbers = section.NumberOfLineNumbers;
            model.PointerToLineNumbers = section.PointerToLineNumbers;
            model.PointerToRelocations = section.PointerToRelocations;
            model.PointerToRawData = section.PointerToRawData;
            model.SizeOfRawData = section.SizeOfRawData;
                
            sections.Add(model);
        }
            
        _sections = sections.ToArray();
    }

    static T ReadStruct<T>(BinaryReader reader)
    {
        byte[] bytes = reader.ReadBytes(Marshal.SizeOf(typeof(T)));
        GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
        T structure = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T))!;
        handle.Free();
        return structure;
    }
    
    /// <summary>
    /// Calculates first (ExportSection offset)
    /// in Portable Executable
    /// </summary>
    /// <returns></returns>
    public uint GetFirstSectionOffset32()
    {
        // export section offset //
        uint offset = Convert.ToUInt32(
            _dosHeader.e_lfanew +
            _portableHeader32.WinNtMain.SizeOfOptionalHeader +
            sizeof(UInt32) +
            Marshal.SizeOf<PortableExecutable32>()
        );
        return offset;
    }
    /// <summary>
    /// Calculates first (ExportSection offset)
    /// in Portable Executable
    /// </summary>
    /// <returns></returns>
    public uint GetFirstSectionOffset64()
    {
        // export section offset //
        uint offset = Convert.ToUInt32(
            _dosHeader.e_lfanew +
            _portableHeader64.WinNtMain.SizeOfOptionalHeader +
            sizeof(uint) +
            Marshal.SizeOf<PortableExecutable32>()
        );
        return offset;
    }
    
}