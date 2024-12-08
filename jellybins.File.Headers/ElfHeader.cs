using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace jellybins.Models
{
    [Flags]
    public enum ElfData : ushort
    {
        LittleEndian = 1,
        BigEndian = 2  
    }


    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Elf32Hdr
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] e_ident; // ELF magic number (16 bytes)
        public ElfData e_type;    // Object file type
        public ElfData e_machine; // Architecture
        public UInt32 e_version;  // ELF file version
        public UInt32 e_entry;    // Entry point address
        public UInt32 e_phoff;    // Program header table file offset
        public UInt32 e_shoff;    // Section header table file offset
        public UInt32 e_flags;    // Processor-specific flags
        public ElfData e_ehsize;  // ELF header size
        public ElfData e_phentsize; // Program header entry size
        public ElfData e_phnum;    // Number of program headers
        public ElfData e_shentsize; // Section header entry size
        public ElfData e_shnum;    // Number of section headers
        public ElfData e_shstrndx; // Section header string table index
    }


    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Elf64Hdr
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] e_ident; // ELF magic number (16 bytes)
        public ElfData e_type;    // Object file type
        public ElfData e_machine; // Architecture
        public UInt32 e_version;  // ELF file version
        public UInt64 e_entry;    // Entry point address
        public UInt64 e_phoff;    // Program header table file offset
        public UInt64 e_shoff;    // Section header table file offset
        public UInt32 e_flags;    // Processor-specific flags
        public ElfData e_ehsize;  // ELF header size
        public ElfData e_phentsize; // Program header entry size
        public ElfData e_phnum;    // Number of program headers
        public ElfData e_shentsize; // Section header entry size
        public ElfData e_shnum;    // Number of section headers
        public ElfData e_shstrndx; // Section header string table index
    }
}
