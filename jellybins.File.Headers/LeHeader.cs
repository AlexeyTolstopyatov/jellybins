using System.Runtime.InteropServices;
/*
 * Jelly Bins (C) Толстопятов Алексей 2024
 *      LE (Linear Executable) Header
 * Файл со структурой и определениями линейного заголовка
 * Смешанный 16-32 разрядный заголовок, разработанный IBM для OS/2
 * Используется в OS/2 2.x версиях для библиотек и приложений, имеет подпись LX
 * Используется в Windows 3x Windows 9x в качестве библиотек/драйверов .VxD имеет подпись LE
 */
namespace jellybins.File.Headers
{
    [StructLayout(LayoutKind.Sequential)]
    public struct LeHeader
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public char[] SignatureWord;

        [MarshalAs(UnmanagedType.I1)] public byte ByteOrder;
        [MarshalAs(UnmanagedType.U2)] public ushort WordOrder;
        [MarshalAs(UnmanagedType.U4)] public uint ExecutableFormatLevel;
        [MarshalAs(UnmanagedType.U2)] public ushort CPUType;
        [MarshalAs(UnmanagedType.U2)] public ushort TargetOperatingSystem;
        [MarshalAs(UnmanagedType.U4)] public uint ModuleVersion;
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

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] Reserved;

        [MarshalAs(UnmanagedType.U4)] public uint WindowsVXDVersionInfoResourceOffset;
        [MarshalAs(UnmanagedType.U4)] public uint WindowsVXDVersionInfoResourceLength;
        [MarshalAs(UnmanagedType.U2)] public ushort WindowsVXDDeviceID;
        [MarshalAs(UnmanagedType.U2)] public ushort WindowsDDKVersion;
    }
}
