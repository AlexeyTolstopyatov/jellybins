﻿using System.Runtime.InteropServices;

namespace jellybins.Core.Sections;

[StructLayout(LayoutKind.Explicit, Pack = 1)]
public struct Section {
    [FieldOffset(0)]
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)] public char[] Name;
    [FieldOffset(8)]  public uint VirtualSize;
    [FieldOffset(12)] public uint VirtualAddress;
    [FieldOffset(16)] public uint SizeOfRawData;
    [FieldOffset(20)] public uint PointerToRawData;
    [FieldOffset(24)] public uint PointerToRelocations;
    [FieldOffset(28)] public uint PointerToLineNumbers;
    [FieldOffset(32)] public ushort NumberOfRelocations;
    [FieldOffset(34)] public ushort NumberOfLineNumbers;
    [FieldOffset(36)] public DataSectionFlags Characteristics;

    public string SectionNameToString => new(Name);
    
}