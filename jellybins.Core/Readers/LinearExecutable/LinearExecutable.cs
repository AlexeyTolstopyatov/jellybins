using System.Runtime.InteropServices;

namespace jellybins.Core.Readers.LinearExecutable
{
    [StructLayout(LayoutKind.Sequential)]
    public struct LinearExecutable
    {
        [MarshalAs(UnmanagedType.U2)] public ushort SignatureWord;
        [MarshalAs(UnmanagedType.I1)] public byte ByteOrder;
        [MarshalAs(UnmanagedType.U1)] public byte WordOrder;
        [MarshalAs(UnmanagedType.U4)] public uint ExecutableFormatLevel;
        [MarshalAs(UnmanagedType.U2)] public ushort CPUType;
        [MarshalAs(UnmanagedType.U2)] public ushort TargetOperatingSystem;
        [MarshalAs(UnmanagedType.U2)] public ushort ModuleVersionMajor;
        [MarshalAs(UnmanagedType.U2)] public ushort ModuleVersionMinor;
        [MarshalAs(UnmanagedType.U4)] public uint ModuleTypeFlags;
        [MarshalAs(UnmanagedType.U4)] public uint NumberOfMemoryPages;
        [MarshalAs(UnmanagedType.U4)] public uint InitialObjectCSNumber;
        [MarshalAs(UnmanagedType.U4)] public uint InitialEIP;
        [MarshalAs(UnmanagedType.U4)] public uint InitialSSObjectNumber;
        [MarshalAs(UnmanagedType.U4)] public uint InitialESP;
        [MarshalAs(UnmanagedType.U4)] public uint MemoryPageSize;
        [MarshalAs(UnmanagedType.U4)] public uint BytesOnLastPage;
        [MarshalAs(UnmanagedType.U4)] public uint FixupSectionSize;
        [MarshalAs(UnmanagedType.U4)] public uint FixupSectionChecksum;
        [MarshalAs(UnmanagedType.U4)] public uint LoaderSectionSize;
        [MarshalAs(UnmanagedType.U4)] public uint LoaderSectionChecksum;
        [MarshalAs(UnmanagedType.U4)] public uint ObjectTableOffset;
        [MarshalAs(UnmanagedType.U2)] public ushort ObjectTableEntries;
        [MarshalAs(UnmanagedType.U4)] public uint ObjectPageMapOffset;
        [MarshalAs(UnmanagedType.U4)] public uint ObjectIterateDataMapOffset;
        [MarshalAs(UnmanagedType.U4)] public uint ResourceTableOffset;
        [MarshalAs(UnmanagedType.U2)] public ushort ResourceTableEntries;
        [MarshalAs(UnmanagedType.U4)] public uint ResidentNamesTableOffset;
        [MarshalAs(UnmanagedType.U4)] public uint EntryTableOffset;
        [MarshalAs(UnmanagedType.U4)] public uint ModuleDirectivesTableOffset;
        [MarshalAs(UnmanagedType.U2)] public ushort ModuleDirectivesTableEntries;
        [MarshalAs(UnmanagedType.U4)] public uint FixupPageTableOffset;
        [MarshalAs(UnmanagedType.U4)] public uint FixupRecordTableOffset;
        [MarshalAs(UnmanagedType.U4)] public uint ImportedModulesNameTableOffset;
        [MarshalAs(UnmanagedType.U2)] public ushort ImportedModulesCount;
        [MarshalAs(UnmanagedType.U4)] public uint ImportedProcedureNameTableOffset;
        [MarshalAs(UnmanagedType.U4)] public uint PerPageChecksumTableOffset;
        [MarshalAs(UnmanagedType.U4)] public uint DataPagesOffsetFromTopOfFile;
        [MarshalAs(UnmanagedType.U4)] public uint PreloadPagesCount;
        [MarshalAs(UnmanagedType.U4)] public uint NonResidentNamesTableOffsetFromTopOfFile;
        [MarshalAs(UnmanagedType.U4)] public uint NonResidentNamesTableLength;
        [MarshalAs(UnmanagedType.U4)] public uint NonResidentNamesTableChecksum;
        [MarshalAs(UnmanagedType.U4)] public uint AutomaticDataObject;
        [MarshalAs(UnmanagedType.U4)] public uint DebugInformationOffset;
        [MarshalAs(UnmanagedType.U4)] public uint DebugInformationLength;
        [MarshalAs(UnmanagedType.U4)] public uint PreloadInstancePagesNumber;
        [MarshalAs(UnmanagedType.U4)] public uint DemandInstancePagesNumber;
        [MarshalAs(UnmanagedType.U4)] public uint HeapSize;
        [MarshalAs(UnmanagedType.U4)] public uint StackSize;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)] public byte[] Reserved;

        [MarshalAs(UnmanagedType.U4)] public uint WindowsVXDVersionInfoResourceOffset;
        [MarshalAs(UnmanagedType.U4)] public uint WindowsVXDVersionInfoResourceLength;
        [MarshalAs(UnmanagedType.U2)] public ushort WindowsVXDDeviceID;
        [MarshalAs(UnmanagedType.U1)] public byte WindowsDDKVersionMinor; // char => byte
        [MarshalAs(UnmanagedType.U1)] public byte WindowsDDKVersionMajor;
    }
}
