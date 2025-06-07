using System.Runtime.InteropServices;
using JellyBins.Abstractions;
using JellyBins.PortableExecutable.Exceptions;
using JellyBins.PortableExecutable.Headers;

namespace JellyBins.PortableExecutable.Models;

/// <summary>
/// Part file's Static Analyser. Main goal: determine
/// specific VB machine structures and other metadata
/// (return filled structures and extracted API)
/// </summary>
public class VbFileDumper(PeSection[] sections, PeDirectory[] directories, ImportDll[] imports)
{
    public struct ImageData
    {
        public Int64 BaseAlignment;
        public Int64 VbStartOffset;
        public Int64 VbStart;
        public Int64 VbVersionRawOffset;
        public Int64 VbVersionMasked;
        public Int64 VbInterpreterOffset;
        public Int64 PushStartAddress;
        public Int64 ProjDataAppReference;
    }

    private ImageData _appData = new();
    private PeSection[] Sections { get; set; } = sections;
    private PeDirectory[] Directories { get; set; } = directories;
    private ImportDll[] Imports { get; set; } = imports;

    public Boolean IsOldVb4Linked { get; private set; }
    public Boolean IsVb4Linked { get; private set; }
    public Boolean IsVb5Linked { get; private set; }
    public Boolean IsVb6Linked { get; private set; }

    public void FindMetadata<T>(BinaryReader reader) where T : struct
    {
        if (String.Equals(Imports[0].DllName, "VB40032.dll", StringComparison.InvariantCultureIgnoreCase))
        {
            // get VB 4.0 mark >> send to specified method
            IsVb4Linked = true;
        }
        if (String.Equals(Imports[0].DllName, "msvbvm50.dll", StringComparison.InvariantCultureIgnoreCase))
        {
            IsVb5Linked = true;
        }

        if (String.Equals(Imports[0].DllName, "msvbvm60.dll", StringComparison.InvariantCultureIgnoreCase))
        {
            IsVb6Linked = true;
        }
    }
    /// <returns> true if API seems like ActiveX object :3</returns>
    public Boolean IsActiveXObject(ExportFunction[] exports)
    {
        Boolean activex = exports.Any(x => 
            String.Equals("DllCanUnloadNow", x.Name, StringComparison.OrdinalIgnoreCase));
        IsVb5Linked = true;
        IsVb6Linked = true;
        
        return activex;
    }
    
    public PeVb5HeaderDump GetVb56ImageDump(BinaryReader reader, UInt32 addressOfEntryPoint, UInt32 imageBase)
    {
        // This code took from Semi VB Decompiler (see modPeSkeleton.bas!CheckHeader(void))
        UInt32 decLoadOffset = imageBase;
        Byte vbVersion;
        try
        {
            _appData.BaseAlignment = ((imageBase + addressOfEntryPoint) - Offset(addressOfEntryPoint));
            reader.BaseStream.Seek(Offset(addressOfEntryPoint), SeekOrigin.Begin);
            vbVersion = reader.ReadByte();
            
            // VB5
            if (vbVersion == 104)
            {
                // offset +1?
                reader.BaseStream.Seek(Offset(addressOfEntryPoint), SeekOrigin.Begin);

                _appData.VbStart = reader.ReadInt32();

                // remember the start of VB Header
                // VBStartHeader.PushStartAddress = vbStart;
                
                return new PeVb5HeaderDump();
            }
        }
        catch (Exception e)
        {
            // NormalVB:
            // Get the APP data VB app start location = OPTHeader.EntryPoint
            _appData.VbStart = addressOfEntryPoint;
            _appData.BaseAlignment = imageBase;
            
            // Point file at the VB code start position
            reader.BaseStream.Seek(_appData.VbStart, SeekOrigin.Begin);
            
            // Get the VBStartHeader, check error
            // 
            // --- Find this method ---
            // GetVBStartHeader();
            //
            // if (ErrorFlag == true)
            // {
            //     CheckHeader = false;     The plan fell off
            //     return CheckHeader;  <-- Exit Sub
            // }
                  
            // **************************************
            // The VB start vector holds the compiler signature
            // **************************************
            
            // Get the APP data VB signature offset
            _appData.VbVersionRawOffset = _appData.PushStartAddress;
            
            // Calculate the APP offset
            _appData.VbVersionMasked = _appData.VbVersionRawOffset - decLoadOffset;

            // Point file at the VB signature position
            try
            {
                reader.BaseStream.Seek(_appData.VbVersionMasked, SeekOrigin.Begin);
                
                // Check for VB version (compiler) of this file, check error
                GetVBVer();
              
                if (ErrorFlag == true)
                {
                    CheckHeader = false;
                    return CheckHeader;
                }
                
                // Assign this location to our reference
                AppData.ProjDataAppReference = _appData.VbInterpreterOffset;
                       
                // *****************************
                // Check if the interpreter name exists
                // *****************************
                
                // Point file at the Data Directory #1 position
                reader.BaseStream.Seek(Directories[1].VirtualAddress, SeekOrigin.Begin);
                
                // Move ahead 12 bytes
                reader.BaseStream.Seek(reader.BaseStream.Position + 12, SeekOrigin.Begin);
                
                // Get the APP data interpreter address offset
                _appData.VbInterpreterOffset = reader.ReadUInt32();
                
                // Move to the interpreter signature
                reader.BaseStream.Seek(AppData.VBIntrptrOffset, SeekOrigin.Begin);
                
                // Get the interpreter
                GetVBIntrptr();
                IsVb6Linked = true;
                if (ErrorFlag == true)
                {
                    // CheckHeader = false;
                    return CheckHeader;
                }

                // If we got here, this is definitely a valid VB6 app
                return CheckHeader;
            }
            catch
            {
                // Handle error from On Error Resume Next
            }
        }
        return new PeVb5HeaderDump();
    }

    private Boolean TryGetVbHeading()
    {
        
    }
    
    public Vb4HeaderDump GetVb4HeaderDump(BinaryReader reader)
    {
        return new();
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
    {   // rva = {uint} 2019914798 
        // rva = {long} 2019914798 
        // RVA в PE всегда 32-битное, преобразуем к uint
        UInt32 rva32 = Convert.ToUInt32(rva); // instead casting
        foreach (PeSection section in Sections.OrderBy(s => s.VirtualAddress))
        {
            if (rva32 >= section.VirtualAddress && 
                rva32 < section.VirtualAddress + section.VirtualSize)
            {
                return section;
            }
        }
        throw new SectionNotFoundException();
    }
}