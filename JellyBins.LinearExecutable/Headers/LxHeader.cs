using System.Runtime.InteropServices;

namespace JellyBins.LinearExecutable.Headers;

[StructLayout(LayoutKind.Sequential)]
public struct LxHeader
{
    [MarshalAs(UnmanagedType.U2)] public UInt16 SignatureWord;
    [MarshalAs(UnmanagedType.I1)] public Byte ByteOrder;
    [MarshalAs(UnmanagedType.U1)] public Byte WordOrder;
    [MarshalAs(UnmanagedType.U4)] public UInt32 ExecutableFormatLevel;
    [MarshalAs(UnmanagedType.U2)] public UInt16 CPUType;
    [MarshalAs(UnmanagedType.U2)] public UInt16 OSType;
    [MarshalAs(UnmanagedType.U2)] public UInt16 ModuleVersionMajor;
    [MarshalAs(UnmanagedType.U2)] public UInt16 ModuleVersionMinor;
    [MarshalAs(UnmanagedType.U4)] public UInt32 ModuleTypeFlags;
    [MarshalAs(UnmanagedType.U4)] public UInt32 ModuleNumberOfPages;
    [MarshalAs(UnmanagedType.U4)] public UInt32 InitialEIPObjectNumber;
    [MarshalAs(UnmanagedType.U4)] public UInt32 InitialEIP;
    [MarshalAs(UnmanagedType.U4)] public UInt32 InitialESPObjectNumber;
    [MarshalAs(UnmanagedType.U4)] public UInt32 InitialESP;
    [MarshalAs(UnmanagedType.U4)] public UInt32 MemoryPageSize;
    [MarshalAs(UnmanagedType.U4)] public UInt32 MemoryPageOffsetShift;
    //[MarshalAs(UnmanagedType.U4)] public UInt32 BytesOnLastPage;
    [MarshalAs(UnmanagedType.U4)] public UInt32 FixupSectionSize;
    [MarshalAs(UnmanagedType.U4)] public UInt32 FixupSectionChecksum;
    [MarshalAs(UnmanagedType.U4)] public UInt32 LoaderSectionSize;
    [MarshalAs(UnmanagedType.U4)] public UInt32 LoaderSectionChecksum;
    [MarshalAs(UnmanagedType.U4)] public UInt32 ObjectTableOffset;
    [MarshalAs(UnmanagedType.U2)] public UInt16 ObjectTableEntries;
    [MarshalAs(UnmanagedType.U4)] public UInt32 ObjectPageMapOffset;
    [MarshalAs(UnmanagedType.U4)] public UInt32 ObjectIterateDataMapOffset;
    [MarshalAs(UnmanagedType.U4)] public UInt32 ResourceTableOffset;
    [MarshalAs(UnmanagedType.U2)] public UInt16 ResourceTableEntries;
    [MarshalAs(UnmanagedType.U4)] public UInt32 ResidentNamesTableOffset;
    [MarshalAs(UnmanagedType.U4)] public UInt32 EntryTableOffset;
    [MarshalAs(UnmanagedType.U4)] public UInt32 ModuleDirectivesTableOffset;
    [MarshalAs(UnmanagedType.U2)] public UInt16 ModuleDirectivesTableEntries;
    [MarshalAs(UnmanagedType.U4)] public UInt32 FixupPageTableOffset;
    [MarshalAs(UnmanagedType.U4)] public UInt32 FixupRecordTableOffset;
    [MarshalAs(UnmanagedType.U4)] public UInt32 ImportedModulesNameTableOffset;
    [MarshalAs(UnmanagedType.U2)] public UInt16 ImportedModulesCount;
    [MarshalAs(UnmanagedType.U4)] public UInt32 ImportedProcedureNameTableOffset;
    [MarshalAs(UnmanagedType.U4)] public UInt32 PerPageChecksumTableOffset;
    [MarshalAs(UnmanagedType.U4)] public UInt32 DataPagesOffsetFromTopOfFile;
    [MarshalAs(UnmanagedType.U4)] public UInt32 PreloadPagesCount;
    [MarshalAs(UnmanagedType.U4)] public UInt32 NonResidentNamesTableOffsetFromTopOfFile;
    [MarshalAs(UnmanagedType.U4)] public UInt32 NonResidentNamesTableLength;
    [MarshalAs(UnmanagedType.U4)] public UInt32 NonResidentNamesTableChecksum;
    [MarshalAs(UnmanagedType.U4)] public UInt32 AutomaticDataObject;
    [MarshalAs(UnmanagedType.U4)] public UInt32 DebugInformationOffset;
    [MarshalAs(UnmanagedType.U4)] public UInt32 DebugInformationLength;
    [MarshalAs(UnmanagedType.U4)] public UInt32 PreloadInstancePagesNumber;
    [MarshalAs(UnmanagedType.U4)] public UInt32 DemandInstancePagesNumber;
    [MarshalAs(UnmanagedType.U4)] public UInt32 HeapSize;
    [MarshalAs(UnmanagedType.U4)] public UInt32 StackSize;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)] public Byte[] Reserved;
}