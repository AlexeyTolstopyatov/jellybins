namespace jellybins.Report.Common.AssemblerOutExecutable;

public enum Flags
{
    DynamicExecutable = 0x20,
    Pic = 0x10,
    DpMask = 0x30
}

// Интерпретации флага (a_flags & DP_MASK):
//  00 -- Традиционный исполняемый двоичный файл
//  01 -- Объектный файл (содержит PIC код)
//  10 -- Динамический исполняемый файл
//  11 -- Независимый