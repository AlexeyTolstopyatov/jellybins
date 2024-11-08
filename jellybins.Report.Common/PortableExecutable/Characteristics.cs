namespace jellybins.Report.Common.PortableExecutable;

public enum Characteristics
{
    /// <summary>
    /// Сведения о перемещениях секций
    /// находятся в другом файле
    /// </summary>
    RelocationsStripped = 0x0001,       // .reloc Stripped
    /// <summary>
    /// Исполняемый образ (EXE|DLL...)
    /// </summary>
    Executable = 0x0002,                // Binary is Executable
    /// <summary>
    /// Номера строк COFF находятся в другом файле
    /// </summary>
    CoffLinesNumbersRemoved = 0x0004,   // COFF Line's numbers Stripped to another file
    /// <summary>
    /// Таблица символов COFF находится в другом файле
    /// </summary>
    CoffSymbolTableRemoved = 0x0008,    // COFF Char's table Stripped to another fil
    /// <summary>
    /// Агрессивно обосрать рабочее место 
    /// </summary>
    AggressiveTrimWorkSet = 0x0010,     // Aggressively trim WorkSet (Obsolete as Windows NT 5.0)
    /// <summary>
    /// Поддержка адресов памяти больше 2Gb
    /// </summary>
    LargeAddressAware = 0x0020,         // Memory Pages support > 2 GB
    /// <summary>
    /// Порядок байт от младшего к старшему (IA32 момент)
    /// </summary>
    ByteWordReverseLow = 0x0080,        // Low Endian flag (Obsolete)
    /// <summary>
    /// Порядок байт от старшего к младшему (IA64 момент)
    /// </summary>
    ByteWordReverseHigh = 0x8000,       // Big Endian flag (Obsolete)
    /// <summary>
    /// Требуются 32 разрядные регистры
    /// </summary>
    Support32Words = 0x0100,            // Machine must support 32-bit WORDs (DWORDs?)
    /// <summary>
    /// Информация отладки в другом файле
    /// </summary>
    DebugInfoStripped = 0x0200,         // Debug information stripped to another file
    /// <summary>
    /// Образ находится на извлекаемом устройстве
    /// </summary>
    ImageBaseOnRemovableMedia = 0x0400, // Image bases on Removable Media => Transport to SWAP
    /// <summary>
    /// Образ находится на сетевом устройстве
    /// </summary>
    ImageBaseInNetworkResource = 0x0800,// Image bases in Network Media => Transport to SWAP
    /// <summary>
    /// Системный файл!!!
    /// </summary>
    SystemFile = 0x1000,                // Image is System's file
    /// <summary>
    /// Динамическая библиотека
    /// (Обычно говорит, будет ли характеристика DLL ниже или нет)
    /// </summary>
    DynamicLibrary = 0x2000,            // Binary is DLL
    /// <summary>
    /// Только для одного ядра (или процессора)
    /// </summary>
    SingleCpuRequired = 0x4000,
}