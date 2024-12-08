using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace jellybins.File.Headers
{
    
    /// <summary>
    /// A-Out Структура заголовка двоичного файла
    /// Важно скорее всего здесь узнать только заголовок
    /// А для полного анализа по-хорошему вообще это все дизассемблировать, ибо
    /// выравнивание секций в каждом Unix-like ребенке вообще разное может быть
    /// А (Net)'BSD (скорее всего) еще долго сидела с исполняемыми файлами A-Out, поэтому вариантов
    /// Магического три, а не 1 (0407)
    /// В добавок к разнице от Unix до Unix подобной ОС (очень старых версий)
    /// системные вызовы разные, библиотеки разные, входные точки разные.
    /// И Assembler-Output нереально сильно зависит от архитектуры... Поэтому как-то так.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct AoutHeader
    {
        [MarshalAs(UnmanagedType.U2)] public ushort a_mag;  // sizeof=2
        [MarshalAs(UnmanagedType.U2)] public char a_flags;  // sizeof=2
        [MarshalAs(UnmanagedType.U1)] public char a_cpu;    // sizeof=1 (maximum=0xFF) почему тогда есть определения 0x12c
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
    // Где использовался скелет такого зверя
    // Unix System IV
    // Solaris
    // Linux 1.x
    // ...-BSD
    // Iris
    // XeroxOS (полагаю тоже)
    // 
    
    
    // struct  exec {                  /* a.out header */
    // 7   unsigned char a_magic[2];     /* magic number */
    // 8   unsigned char a_flags;        /* flags, see below */
    // 9   unsigned char a_cpu;          /* cpu id */
    // 10   unsigned char a_hdrlen;       /* length of header */
    // 11   unsigned char a_unused;       /* reserved for future use */
    // 12   unsigned short a_version;     /* version stamp (not used at present) */
    // ...
    // }
}
