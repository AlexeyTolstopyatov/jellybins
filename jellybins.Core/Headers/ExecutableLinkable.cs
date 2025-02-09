using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace jellybins.Core.Headers
{
    [Flags]
    public enum ElfData : ushort
    {
        LittleEndian = 1,
        BigEndian = 2  
    }
    
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ExecutableLinkable32
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] e_ident; // ELF magic number (16 bytes)
        [MarshalAs(UnmanagedType.Struct)]
        public ElfData e_type;    // Object file type
        [MarshalAs(UnmanagedType.Struct)]
        public ElfData e_machine; // CpuArchitecture
        public UInt32 e_version;  // ELF file version
        public UInt32 e_entry;    // Entry point address
        public UInt32 e_phoff;    // Program header table file offset
        public UInt32 e_shoff;    // Section header table file offset
        public UInt32 e_flags;    // Processor-specific flags
        [MarshalAs(UnmanagedType.Struct)]
        public ElfData e_ehsize;  // ELF header size
        [MarshalAs(UnmanagedType.Struct)]
        public ElfData e_phentsize; // Program header entry size
        [MarshalAs(UnmanagedType.Struct)]
        public ElfData e_phnum;    // Number of program headers
        [MarshalAs(UnmanagedType.Struct)]
        public ElfData e_shentsize; // Section header entry size
        [MarshalAs(UnmanagedType.Struct)]
        public ElfData e_shnum;    // Number of section headers
        [MarshalAs(UnmanagedType.Struct)]
        public ElfData e_shstrndx; // Section header string table index
    }


    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ExecutableLinkable64
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] e_ident; // ELF magic number (16 bytes)
        [MarshalAs(UnmanagedType.Struct)]
        public ElfData e_type;    // Object file type
        [MarshalAs(UnmanagedType.Struct)]
        public ElfData e_machine; // CpuArchitecture
        public UInt32 e_version;  // ELF file version
        public UInt64 e_entry;    // Entry point address
        public UInt64 e_phoff;    // Program header table file offset
        public UInt64 e_shoff;    // Section header table file offset
        public UInt32 e_flags;    // Processor-specific flags
        [MarshalAs(UnmanagedType.Struct)]
        public ElfData e_ehsize;  // ELF header size
        [MarshalAs(UnmanagedType.Struct)]
        public ElfData e_phentsize; // Program header entry size
        [MarshalAs(UnmanagedType.Struct)]
        public ElfData e_phnum;    // Number of program headers
        [MarshalAs(UnmanagedType.Struct)]
        public ElfData e_shentsize; // Section header entry size
        [MarshalAs(UnmanagedType.Struct)]
        public ElfData e_shnum;    // Number of section headers
        [MarshalAs(UnmanagedType.Struct)]
        public ElfData e_shstrndx; // Section header string table index
    }
}
