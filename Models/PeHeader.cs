using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace jellybins.Models
{
    public enum PortableCharacteristics
    {
        RelocationsStripped = 0x0001,       // .reloc Stripped
        Executable = 0x0002,                // Binary is Executable
        CoffLinesNumbersRemoved = 0x0004,   // COFF Line's numbers Stripped to another file
        CoffSymbolTableRemoved = 0x0008,    // COFF Char's table Stripped to another file
        AggressiveTrimWorkSet = 0x0010,     // Aggressively trim WorkSet (Obsolete as Windows NT 5.0)
        LargeAddressAware = 0x0020,         // Memory Pages support > 2 GB
        ByteWordReverseLow = 0x0080,        // Low Endian flag (Obsolete)
        ByteWordReverseHigh = 0x8000,       // Big Endian flag (Obsolete)
        Support32Words = 0x0100,            // Machine must support 32-bit WORDs (DWORDs?)
        DebugInfoStripped = 0x0200,         // Debug information stripped to another file
        ImageBaseOnRemovableMedia = 0x0400, // Image bases on Removable Media => Transport to SWAP
        ImageBaseInNetworkResource = 0x0800,// Image bases in Network Media => Transport to SWAP
        SystemFile = 0x1000,                // Image is System's file
        DynamicLibrary = 0x2000,            // Binary is DLL
        SingleCpuRequired = 0x4000,         // File requires only 1 CPU
    }
    
    public enum PortableArchitecture
    {
        IA32 = 0x014c,
        IA64 = 0x0200,
        IA32e = 0x8664
    }

    public enum PortableMagic
    {
        Hdr32Magic = 0x10b,
        Hdr64Magic = 0x20b,
        RomMagic = 0x107
    }
    
    public enum PortableEnvironment
    {
        Native = 1,
        ConsoleInterface = 3,
        GraphicalInterface = 2,
        Os2ConsoleInterface = 5,
        PosixConsoleInterface = 7,
        WindowsNative = 8,
        WindowsCeGraphicalInterface = 9
    }

    
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct NtHeader
    {
        public uint Signature; // PE\x00\x00
        [MarshalAs(UnmanagedType.Struct)] public PortableFileHeader WinNtMain;
        [MarshalAs(UnmanagedType.Struct)] public OptionalPortableHeader WinNtOptional;
    }
    
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct PortableFileHeader
    {
        public ushort Machine;
        public ushort NumberOfSections;
        public uint TimeDateStamp;
        public uint PointerToSymbolTable;
        public uint NumberOfSymbols;
        public ushort SizeOfOptionalHeader;
        public ushort Characteristics;
    }


    [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
    public struct OptionalPortableHeader
    {
        public ushort Magic; // 32/64bit 
        public byte MajorLinkVersion;
        public byte MinorLinkVersion;
        public uint CodeSectionSize;
        public uint InitObjectsSize;
        public uint UninitObjectsSize;
        public uint EntryPoint;
        public uint CodeSectionOffset;
        public uint DataSectionOffset;
        public uint ImageBase;
        public uint SectionAlignment;
        public uint FileAlignment;
        public ushort MajorOsVersion;
        public ushort MinorOsVersion;
        public ushort MajorImageVersion;
        public ushort MinorImageVersion;
        public ushort MajorSubSystemVersion;
        public ushort MinorSubSystemVersion;
        public uint Win32Version;
        public uint SizeofImage;
        public uint SizeofHeaders;
        public uint Checksum;
        public ushort Subsystem;
        public ushort DllCharacteristics;
        public uint SizeofStackReserve;
        public uint SizeofStackCommit;
        public uint SizeofHeapReserve;
        public uint SizeofHeapCommit;
        public uint LoaderFlags;
        public uint NumberOfRVAAndSizes;
    }
}
