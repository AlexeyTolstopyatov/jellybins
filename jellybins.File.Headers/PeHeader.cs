using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace jellybins.File.Headers
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct NtHeader32
    {
        public uint Signature; // PE\x00\x00
        [MarshalAs(UnmanagedType.Struct)] public PortableFileHeader WinNtMain;
        [MarshalAs(UnmanagedType.Struct)] public OptionalPortableHeader32 WinNtOptional;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct NtHeader64
    {
        public uint Signature;
        [MarshalAs(UnmanagedType.Struct)] public PortableFileHeader WinNtMain;
        [MarshalAs(UnmanagedType.Struct)] public OptionalPortableHeader64 WinNtOptional;
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
    public struct OptionalPortableHeader32
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
        
        [MarshalAs(UnmanagedType.Struct)] public Data ExportTable;
        [MarshalAs(UnmanagedType.Struct)] public Data ImportTable;
        [MarshalAs(UnmanagedType.Struct)] public Data ResourceTable;
        [MarshalAs(UnmanagedType.Struct)] public Data ExceptionTable;
        [MarshalAs(UnmanagedType.Struct)] public Data CertificateTable;
        [MarshalAs(UnmanagedType.Struct)] public Data BaseRelocationTable;
        [MarshalAs(UnmanagedType.Struct)] public Data Debug;
        [MarshalAs(UnmanagedType.Struct)] public Data Architecture;
        [MarshalAs(UnmanagedType.Struct)] public Data GlobalPtr;
        [MarshalAs(UnmanagedType.Struct)] public Data TLSTable;
        [MarshalAs(UnmanagedType.Struct)] public Data LoadConfigTable;
        [MarshalAs(UnmanagedType.Struct)] public Data BoundImport;
        [MarshalAs(UnmanagedType.Struct)] public Data IAT;
        [MarshalAs(UnmanagedType.Struct)] public Data DelayImportDescriptor;
        [MarshalAs(UnmanagedType.Struct)] public Data CLRRuntimeHeader;
        [MarshalAs(UnmanagedType.Struct)] public Data Reserved;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct OptionalPortableHeader64
    {
        public ushort Magic;
        public byte MajorLinkerVersion;
        public byte MinorLinkerVersion;
        public uint SizeOfCode;
        public uint SizeOfInitializedData;
        public uint SizeOfUninitializedData;
        public uint AddressOfEntryPoint;
        public uint BaseOfCode;
        public ulong ImageBase;
        public uint SectionAlignment;
        public uint FileAlignment;
        public ushort MajorOperatingSystemVersion;
        public ushort MinorOperatingSystemVersion;
        public ushort MajorImageVersion;
        public ushort MinorImageVersion;
        public ushort MajorSubsystemVersion;
        public ushort MinorSubsystemVersion;
        public uint Win32VersionValue;
        public uint SizeOfImage;
        public uint SizeOfHeaders;
        public uint CheckSum;
        public ushort Subsystem;
        public ushort DllCharacteristics;
        public ulong SizeOfStackReserve;
        public ulong SizeOfStackCommit;
        public ulong SizeOfHeapReserve;
        public ulong SizeOfHeapCommit;
        public uint LoaderFlags;
        public uint NumberOfRvaAndSizes;
        
        [MarshalAs(UnmanagedType.Struct)] public Data ExportTable;
        [MarshalAs(UnmanagedType.Struct)] public Data ImportTable;
        [MarshalAs(UnmanagedType.Struct)] public Data ResourceTable;
        [MarshalAs(UnmanagedType.Struct)] public Data ExceptionTable;
        [MarshalAs(UnmanagedType.Struct)] public Data CertificateTable;
        [MarshalAs(UnmanagedType.Struct)] public Data BaseRelocationTable;
        [MarshalAs(UnmanagedType.Struct)] public Data Debug;
        [MarshalAs(UnmanagedType.Struct)] public Data Architecture;
        [MarshalAs(UnmanagedType.Struct)] public Data GlobalPtr;
        [MarshalAs(UnmanagedType.Struct)] public Data TlsTable;
        [MarshalAs(UnmanagedType.Struct)] public Data LoadConfigTable;
        [MarshalAs(UnmanagedType.Struct)] public Data BoundImport;
        [MarshalAs(UnmanagedType.Struct)] public Data IAT;
        [MarshalAs(UnmanagedType.Struct)] public Data DelayImportDescriptor;
        [MarshalAs(UnmanagedType.Struct)] public Data Cor20Table;
        [MarshalAs(UnmanagedType.Struct)] public Data Reserved;
    }
}
