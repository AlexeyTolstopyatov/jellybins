using System.Runtime.InteropServices;
/*
 * Jelly Bins (C) Толстопятов Алексей 2024
 *      LE (Linear Executable) Header
 * Файл со структурой и определениями линейного заголовка
 * Смешанный 16-32 разрядный заголовок, разработанный IBM для OS/2
 * Используется в OS/2 2.x версиях для библиотек и приложений, имеет подпись LX
 * Используется в Windows 3x Windows 9x в качестве библиотек/драйверов .VxD имеет подпись LE
 */
namespace jellybins.Models
{
    /// <summary>
    /// Определения флага Требуемой операционной системы (byte TargetOperatingSystem)
    /// </summary>
    internal enum LinearOperatingSystemFlag
    {
        OS2 = 1,
        Windows = 2,
        DOS = 3,
        Windows386 = 4
    }
    /// <summary>
    /// Определения флага Типа процессора (ushort CPUType)
    /// </summary>
    internal enum LinearArchitecture
    {
        I286Instructions = 0x1,
        I386Instructions = 0x2,
        I486Instructions = 0x3
    }

    /// <summary>
    /// Определения флагов в таблице флагов
    /// (uint ModuleTypeFlags)
    /// </summary>
    internal enum LinearModuleFlags : uint
    {
        ProgramModule  = 0,
        LibraryModule  = 0x00008000,
        PerProcessInit = 0x00000004,
        InternalFixes  = 0x00000010,
        ExternalFixes  = 0x00000020,
        InCompatiblePm = 0x00000100,
        CompatiblePm   = 0x00000200,
        UsesPm         = 0x00000300,
        NotLoadable    = 0x00002000,
        PhysicalDriver = 0x00020000,
        VirtualDriver  = 0x00028000,
        PerProcessTermination = 0x0004000,
        MultiCpuUnsafe = 0x00080000,
    }
    
    [StructLayout(LayoutKind.Sequential)]
    internal struct LeHeader
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public char[] SignatureWord;

        [MarshalAs(UnmanagedType.I1)] public byte ByteOrder;
        [MarshalAs(UnmanagedType.I1)] public byte WordOrder;
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
