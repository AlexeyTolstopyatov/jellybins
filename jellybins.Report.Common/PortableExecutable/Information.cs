
namespace jellybins.Report.Common.PortableExecutable;
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
public static class Information
{
    /// <summary>
    /// Определяет архитектуру процессора для двоичного файла
    /// </summary>
    /// <param name="machine"></param>
    /// <returns></returns>
    public static string ProcessorFlagToString(ushort machine)
    {
        return (Processor)machine switch
        {
            Processor.Ia32 => "Intel x86 (i386 или IA32)",
            Processor.Ia32E => "AMD64 (x86-64 или IA32e)",
            Processor.Ia64 => "Intel Itanium (IA64)",
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
        return (Magic)magic switch
        {
            (Magic.Application32Bit) => "32-разрядный",
            (Magic.Application64Bit) => "64-разрядный",
            (Magic.ApplicationRom) => "EFI Образ",
            _ => ""
        };
    }

    /// <summary>
    /// Возвращает требуемое окружение (подсистему) для запуска
    /// приложения
    /// </summary>
    /// <param name="subEnv"></param>
    /// <returns></returns>
    public static string EnvironmentFlagToString(ushort subEnv)
    {
        return (Environment)subEnv switch
        {
            Environment.Native => "Отсутствует подсистема",
            Environment.ConsoleInterface => "Консольное приложение Win32",
            Environment.GraphicalInterface => "Оконное приложение Win32",
            Environment.PosixConsoleInterface => "Консольное приложение POSIX",
            Environment.Os2ConsoleInterface => "Консольное Приложение OS/2",
            Environment.WindowsNative => "Приложение WinRT",
            Environment.WindowsCeGraphicalInterface => "Оконное приложение Windows CE",
            Environment.Win16GraphicalInterface => "Оконное приложение Win16",
            Environment.DosConsoleInterface => "Использует NT-VDM",
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
                ((chars & (uint)Characteristics.Executable) != 0) 
                    ? "Запускается напрямую" : 
                    string.Empty,
                ((chars & (uint)Characteristics.RelocationsStripped) != 0)
                    ? "Сведения о перемещениях в другом файле"
                    : string.Empty,
                ((chars & (uint)Characteristics.DynamicLibrary) != 0) 
                    ? "Динамическая библиотека" 
                    : string.Empty,
                ((chars & (uint)Characteristics.CoffLinesNumbersRemoved) != 0)
                    ? "Номера строк COFF в другом файле"
                    : string.Empty,
                ((chars & (uint)Characteristics.CoffSymbolTableRemoved) != 0)
                    ? "Таблица символов COFF в другом файле"
                    : string.Empty,
                ((chars & (uint)Characteristics.AggressiveTrimWorkSet) != 0)
                    ? "Агрессивно урезает рабочий набор"
                    : string.Empty,
                ((chars & (uint)Characteristics.LargeAddressAware) != 0)
                    ? "Поддержка адресов больше 2Гб"
                    : string.Empty,
                ((chars & (uint)Characteristics.ByteWordReverseLow) != 0) 
                    ? "Порядок байт 0x0001" 
                    : string.Empty,
                ((chars & (uint)Characteristics.ByteWordReverseHigh) != 0) 
                    ? "Порядок байт 0x1000" 
                    : string.Empty,
                ((chars & (uint)Characteristics.Support32Words) != 0)
                    ? "Машина поддерживает 32битные слова"
                    : string.Empty,
                ((chars & (uint)Characteristics.DebugInfoStripped) != 0)
                    ? "Информация о отладке в другом файле"
                    : string.Empty,
                ((chars & (uint)Characteristics.ImageBaseOnRemovableMedia) != 0)
                    ? "Копировать в SWAP раздел"
                    : string.Empty,
                ((chars & (uint)Characteristics.ImageBaseInNetworkResource) != 0)
                    ? "Копировать из сети в SWAP раздел"
                    : string.Empty,
                ((chars & (uint)Characteristics.SystemFile) != 0) 
                    ? "Системный файл" 
                    : string.Empty,
                ((chars & (uint)Characteristics.SingleCpuRequired) != 0) 
                    ? "Только для одного ЦП" 
                    : string.Empty
            }
            .Where(flag => !string.IsNullOrEmpty(flag))
            .Distinct()
            .ToArray();
    }

    public static string[] DllCharacteristicsToStrings(ushort dllc)
    {
        return new[]
        {
            //
            // То, чего не было в документации Microsoft
            //
            ((dllc & (ushort)System.Reflection.PortableExecutable.DllCharacteristics.ProcessInit) != 0)
                ? "По-процессная инициализацияя"
                : "",
            ((dllc & (ushort)System.Reflection.PortableExecutable.DllCharacteristics.ProcessTerm) != 0)
                ? "По-процессное уничтожение"
                : "",
            ((dllc & (ushort)System.Reflection.PortableExecutable.DllCharacteristics.ThreadInit) != 0)
                ? "Создает потоки"
                : "",
            ((dllc & (ushort)System.Reflection.PortableExecutable.DllCharacteristics.ThreadTerm) != 0)
                ? "Удаляет потоки"
                : "",

            //
            // То что указано в документации Microsoft
            //
            ((dllc & (ushort)DllCharacteristics.HighEntropyOfVirtualAddresses) != 0)
                ? "Высокая энтрапия виртуальных адресов"
                : "Стандартное распределение виртуальных адресов",
            ((dllc & (ushort)DllCharacteristics.DynamicBase) != 0)
                ? "Перемещаемая база"
                : "Статическая база",
            ((dllc & (ushort)DllCharacteristics.ForceIntegrity) != 0)
                ? "Проверка целостности"
                : "",
            ((dllc & (ushort)DllCharacteristics.ImageNxCompat) != 0)
                ? "NX-совместимый"
                : "NX-несовместим",
            ((dllc & (ushort)DllCharacteristics.NoIsolation) != 0)
                ? "Не изолируется"
                : "Изолируется",
            ((dllc & (ushort)DllCharacteristics.NoBind) != 0)
                ? "Не связывается"
                : "Связывается",
            ((dllc & (ushort)DllCharacteristics.NoSeh) != 0)
                ? "Не использует SEH"
                : "Использует SEH",
            ((dllc & (ushort)DllCharacteristics.ApplicationContainer) != 0)
                ? "Должен быть в контейнере"
                : "",
            ((dllc & (ushort)DllCharacteristics.WindowsDriverModel) != 0)
                ? "Драйвер устройства"
                : "",
            ((dllc & (ushort)DllCharacteristics.ControlFlowGuard) != 0)
                ? "Использует CF-Guard"
                : "",
            ((dllc & (ushort)DllCharacteristics.TerminalServerAware) != 0)
                ? "Осведомляет Terminal Server"
                : "Не работает с Terminal Server"
        }.Where(flag => !string.IsNullOrEmpty(flag)).Distinct().ToArray();
    }
}