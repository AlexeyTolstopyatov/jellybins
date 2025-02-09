using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace jellybins.Core.Headers
{
    /// <summary>
    /// I'm so sorry. MINIX format specification != FreeBSD format specification
    /// A-Out files in FreeBSD are 64 bit!!!
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct AssemblerOutput
    {
        [MarshalAs(UnmanagedType.U2)] public ushort a_mag;  // sizeof=2
        [MarshalAs(UnmanagedType.U1)] public byte a_flags;  // sizeof=2
        [MarshalAs(UnmanagedType.U1)] public byte a_cpu;    // sizeof=1 (maximum=0xFF) почему тогда есть определения 0x12c
        [MarshalAs(UnmanagedType.U1)] public char a_hdrlen; // sizeof=1
        [MarshalAs(UnmanagedType.U1)] public char a_unused; // sizeof=1
        [MarshalAs(UnmanagedType.U1)] public char a_version;// sizeof=1
        [MarshalAs(UnmanagedType.U8)] public ulong a_text;
        [MarshalAs(UnmanagedType.U8)] public ulong a_data;
        [MarshalAs(UnmanagedType.U8)] public ulong a_bss;
        [MarshalAs(UnmanagedType.U8)] public ulong a_syms;
        [MarshalAs(UnmanagedType.U8)] public ulong a_trsize;
        [MarshalAs(UnmanagedType.U8)] public ulong a_drsize;
    }
}
