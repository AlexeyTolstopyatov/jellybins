using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;
using System.Xml;
using jellybins.Core.Sections;

//
// (C) Bilbo Backends 2024-2025
//
// NextRuntimeHeader contains Relative Virtual address
// not to CIL Runtime! Before .NET framework,
// 16th .DATA PortableExecutableDataSection was used in Visual Basic Classic (native-code) compiled
// applications, and NextRuntime-header was absolutely different.
//
// May be Visual Basic header's struct is a Microsoft's secret,
// my goal is know it anyway. Using VB Virtual machine header
// I can try to recognize and disassemble old Applications
// and know little more about VB 5.0/6.0 internals.
//

namespace jellybins.Core.Readers.PortableExecutable
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct PortableExecutable32
    {
        public uint Signature;
        [MarshalAs(UnmanagedType.Struct)] public PortableFileHeader WinNtMain;
        [MarshalAs(UnmanagedType.Struct)] public OptionalPortableHeader32 WinNtOptional;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct PortableExecutable64
    {
        public uint Signature;
        [MarshalAs(UnmanagedType.Struct)] public PortableFileHeader WinNtMain;
        [MarshalAs(UnmanagedType.Struct)] public OptionalPortableHeader64 WinNtOptional;
    }
    
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct PortableFileHeader
    {
        public UInt16 Machine;
        public UInt16 NumberOfSections;
        public UInt32 TimeDateStamp;
        public UInt32 PointerToSymbolTable;
        public UInt32 NumberOfSymbols;
        public UInt16 SizeOfOptionalHeader;
        public UInt16 Characteristics;
    }


    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet=CharSet.Ansi)]
    public struct OptionalPortableHeader32
    {
        public UInt16 Magic;
        public Byte MajorLinkerVersion;
        public Byte MinorLinkerVersion;
        public UInt32 SizeOfCode;
        public UInt32 SizeOfInitializedData;
        public UInt32 SizeOfUninitializedData;
        public UInt32 AddressOfEntryPoint;
        public UInt32 BaseOfCode;
        public UInt32 BaseOfData;
        public UInt32 ImageBase;
        public UInt32 SectionAlignment;
        public UInt32 FileAlignment;
        public UInt16 MajorOperatingSystemVersion;
        public UInt16 MinorOperatingSystemVersion;
        public UInt16 MajorImageVersion;
        public UInt16 MinorImageVersion;
        public UInt16 MajorSubsystemVersion;
        public UInt16 MinorSubsystemVersion;
        public UInt32 Win32VersionValue;
        public UInt32 SizeOfImage;
        public UInt32 SizeOfHeaders;
        public UInt32 CheckSum;
        public UInt16 Subsystem;
        public UInt16 DllCharacteristics;
        public UInt32 SizeOfStackReserve;
        public UInt32 SizeOfStackCommit;
        public UInt32 SizeOfHeapReserve;
        public UInt32 SizeOfHeapCommit;
        public UInt32 LoaderFlags;
        public UInt32 NumberOfRvaAndSizes;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public PortableExecutableDataDirectory[] Directories;
        // [PortableExecutableDataDirectory] ExportTable;
        // [PortableExecutableDataDirectory] ImportTable;
        // [PortableExecutableDataDirectory] ResourceTable;
        // [PortableExecutableDataDirectory] ExceptionTable;
        // [PortableExecutableDataDirectory] CertificateTable;
        // [PortableExecutableDataDirectory] BaseRelocationTable;
        // [PortableExecutableDataDirectory] Debug;
        // [PortableExecutableDataDirectory] Architecture;
        // [PortableExecutableDataDirectory] GlobalPtr;
        // [PortableExecutableDataDirectory] TlsTable;
        // [PortableExecutableDataDirectory] LoadConfigTable;
        // [PortableExecutableDataDirectory] BoundImport;
        // [PortableExecutableDataDirectory] IAT;
        // [PortableExecutableDataDirectory] DelayImportDescriptor;
        // [PortableExecutableDataDirectory] NextRuntimeHeader;
        // [PortableExecutableDataDirectory] Reserved;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct OptionalPortableHeader64
    {
        public UInt16 Magic;
        public Byte MajorLinkerVersion;
        public Byte MinorLinkerVersion;
        public UInt32 SizeOfCode;
        public UInt32 SizeOfInitializedData;
        public UInt32 SizeOfUninitializedData;
        public UInt32 AddressOfEntryPoint;
        public UInt32 BaseOfCode;
        public UInt32 BaseOfData;
        public UInt32 ImageBase;
        public UInt32 SectionAlignment;
        public UInt32 FileAlignment;
        public UInt16 MajorOperatingSystemVersion;
        public UInt16 MinorOperatingSystemVersion;
        public UInt16 MajorImageVersion;
        public UInt16 MinorImageVersion;
        public UInt16 MajorSubsystemVersion;
        public UInt16 MinorSubsystemVersion;
        public UInt32 Win32VersionValue;
        public UInt32 SizeOfImage;
        public UInt32 SizeOfHeaders;
        public UInt32 CheckSum;
        public UInt16 Subsystem;
        public UInt16 DllCharacteristics;
        public UInt64 SizeOfStackReserve;
        public UInt64 SizeOfStackCommit;
        public UInt64 SizeOfHeapReserve;
        public UInt64 SizeOfHeapCommit;
        public UInt32 LoaderFlags;
        public UInt32 NumberOfRvaAndSizes;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public PortableExecutableDataDirectory[] Directories;
        // [PortableExecutableDataDirectory] ExportTable;
        // [PortableExecutableDataDirectory] ImportTable;
        // [PortableExecutableDataDirectory] ResourceTable;
        // [PortableExecutableDataDirectory] ExceptionTable;
        // [PortableExecutableDataDirectory] CertificateTable;
        // [PortableExecutableDataDirectory] BaseRelocationTable;
        // [PortableExecutableDataDirectory] Debug;
        // [PortableExecutableDataDirectory] Architecture;
        // [PortableExecutableDataDirectory] GlobalPtr;
        // [PortableExecutableDataDirectory] TlsTable;
        // [PortableExecutableDataDirectory] LoadConfigTable;
        // [PortableExecutableDataDirectory] BoundImport;
        // [PortableExecutableDataDirectory] IAT;
        // [PortableExecutableDataDirectory] DelayImportDescriptor;
        // [PortableExecutableDataDirectory] NextRuntimeHeader;
        // [PortableExecutableDataDirectory] Reserved;
    }
}
