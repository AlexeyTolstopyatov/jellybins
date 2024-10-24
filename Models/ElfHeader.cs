using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace jellybins.Models
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct ExecutableLinkableHeader32
    {
        public ExecutableLinkableHeader32()
        {
            p_sign = new char[16];
            p_offest = 0;
            p_type = 0;
            p_vaddress = 0;
            p_paddress = 0;
            p_memsz = 0;
            p_flags = 0;
            p_align = 0;
            p_filesz = 0;
        }

        char[] p_sign;
        public uint p_type;
        public uint p_offest;
        public uint p_vaddress;
        public uint p_paddress;
        public uint p_filesz;
        public uint p_memsz;
        public uint p_flags;
        public uint p_align;
    }
}
