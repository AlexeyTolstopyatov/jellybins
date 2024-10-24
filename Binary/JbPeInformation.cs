using jellybins.Models;
namespace jellybins.Binary;
/*
 * Jelly Bins (C) Толстопятов Алексей 2024
 *      Portable Executable Information Block
 * Класс предоставляющий возможность распознавания флагов
 * из линейного заголовка заголовка.
 * Члены класса:    OperatingSystemFlagToString(ushort):    Возвращает ОС для которой собран двоичный файл
 *                  ProcessorFlagToString(ushort):          Возвращает флаг архитектуры процессора, для которого был собран файл
 *                  CharacteristicsToStrings(uint):         Возвращает список расшифрованных флагов из таблицы определения файла
 *                  MagicFlagToString(ushort):              Возвращает разрядность двоичного файла
 *                  VersionFlagToString(ushort, ushort)     Возвращает версию в виде строки
 *                  EnvironmentFlagToString(ushort):        Возвращает тип подсистемы (окружения)
 */
public static class JbPeInformation
{
    /// <summary>
    /// Определяет архитектуру процессора для двоичного файла
    /// </summary>
    /// <param name="machine"></param>
    /// <returns></returns>
    public static string ProcessorFlagToString(ushort machine)
    {
        return (PortableArchitecture)machine switch
        {
            PortableArchitecture.IA32 => "Intel x86 (i386 или IA32)",
            PortableArchitecture.IA32e => "AMD64 (x86-64 или IA32e)",
            PortableArchitecture.IA64 => "Intel Itanium (IA64)",
            _ => ""
        };
    }

    /// <summary>
    /// PE исполняемые файлы, используются только в Microsoft Windows
    /// </summary>
    /// <returns></returns>
    public static string OperatingSystemToString() => 
        "Microsoft Windows";

    /// <summary>
    /// Определяет разрядность двоичного файла
    /// </summary>
    /// <param name="magic"></param>
    /// <returns></returns>
    public static string MagicFlagToString(ushort magic)
    {
        return (PortableMagic)magic switch
        {
            (PortableMagic.Hdr32Magic) => "32-разрядный",
            (PortableMagic.Hdr64Magic) => "64-разрядный",
            (PortableMagic.RomMagic) => "EFI Образ",
            _ => ""
        };
    }

    /// <summary>
    /// Возвращает требуемое окружение (подсистему) для запуска
    /// приложения
    /// </summary>
    /// <param name="subenv"></param>
    /// <returns></returns>
    public static string EnvironmentFlagToString(ushort subenv)
    {
        return (PortableEnvironment)subenv switch
        {
            PortableEnvironment.Native => "Отсутствует подсистема",
            PortableEnvironment.ConsoleInterface => "Консольное приложение Windows",
            PortableEnvironment.GraphicalInterface => "Оконное приложение Windows",
            PortableEnvironment.PosixConsoleInterface => "Консольное приложение POSIX",
            PortableEnvironment.Os2ConsoleInterface => "Консольное Приложение OS/2",
            PortableEnvironment.WindowsNative => "Использует Windows Runtime (WinRT)",
            PortableEnvironment.WindowsCeGraphicalInterface => "Оконное приложение Windows CE",
            _ => ""
        };
    }

    /// <summary>
    /// Возвращает версию для исполняемого файла
    /// </summary>
    /// <param name="maj">Основная версия</param>
    /// <param name="min">Дополнительная версия</param>
    /// <returns></returns>
    public static string VersionToString(ushort maj, ushort min)
    {
        return $"{maj}.{min}";
    }
    
    /*
     * Характеристики: Атрибуты файла могут быть вычислены
     * выборочно с помощью нескольких значений.
     * (Допустимым значением этих флагов является значение IMAGE_FILE_xxx,
     * определенное в winnt.h.
     * Значение этого поля в обычных EXE-файлах обычно равно 0100h,
     * а значение этого поля в DLL-файлах обычно равно 210Eh.)
     * Маленький совет:
     * несколькими атрибутами можно владеть одновременно с помощью
     * "операции или"!
     */
    
    /// <summary>
    /// Возвращает список характеристик библиотеки (для Characteristics)
    /// </summary>
    /// <param name="chars"></param>
    /// <returns></returns>
    public static string[] CharacteristicsToStrings(uint chars)
    {
        return new[]
        {
            ((chars & (uint)PortableCharacteristics.Executable) != 0)? "Запускается напрямую" : string.Empty,
            ((chars & (uint)PortableCharacteristics.RelocationsStripped) != 0)? "Сведения о перемещениях в другом файле" : string.Empty,
            ((chars & (uint)PortableCharacteristics.DynamicLibrary) != 0)? "Динамическая библиотека" : string.Empty,
            ((chars & (uint)PortableCharacteristics.CoffLinesNumbersRemoved) != 0)? "Номера строк COFF в другом файле" :string.Empty,
            ((chars & (uint)PortableCharacteristics.CoffSymbolTableRemoved) != 0)? "Таблица символов COFF в другом файле" : string.Empty,
            ((chars & (uint)PortableCharacteristics.AggressiveTrimWorkSet) != 0)? "Агрессивно урезает рабочий набор" : string.Empty,
            ((chars & (uint)PortableCharacteristics.LargeAddressAware) != 0)? "Поддержка адресов больше 2Гб" : string.Empty,
            ((chars & (uint)PortableCharacteristics.ByteWordReverseLow) != 0)? "Порядок байт 0x0001" : string.Empty,
            ((chars & (uint)PortableCharacteristics.ByteWordReverseHigh) != 0)? "Порядок байт 0x1000" : string.Empty,
            ((chars & (uint)PortableCharacteristics.Support32Words) != 0)? "Машина поддерживает 32битные слова" : string.Empty,
            ((chars & (uint)PortableCharacteristics.DebugInfoStripped) != 0)? "Информация о отладке в другом файле" : string.Empty,
            ((chars & (uint)PortableCharacteristics.ImageBaseOnRemovableMedia) != 0)? "Копировать в SWAP раздел" : string.Empty,
            ((chars & (uint)PortableCharacteristics.ImageBaseInNetworkResource) != 0)? "Копировать из сети в SWAP раздел" : string.Empty,
            ((chars & (uint)PortableCharacteristics.SystemFile) != 0)? "Системный файл" : string.Empty,
            ((chars & (uint)PortableCharacteristics.SingleCpuRequired) != 0)? "Только для одного ЦП" : string.Empty
        };
    }
}