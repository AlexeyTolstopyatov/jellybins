namespace jellybins.Report.Common.NewExecutable;
/*
 * Jelly Bins (C) Толстопятов Алексей 2024
 *      New Executable Information Block
 * Класс предоставляющий возможность распознавания флагов
 * из линейного заголовка заголовка.
 * Члены класса:    ProgramFlagsToStrings(ushort):      Возвращает список расшифрованных программных флагов (требования к процессору)
 *                  ProcessorFlagToStrings(ushort):     Возвращает флаг архитектуры процессора, для которого был собран файл
 *                  ApplicationFlagsToStrings(uint):    Возвращает список расшифрованных флагов из таблицы определения файла
 *                  OperatingSystemFlagToString(byte):  Возвращает ОС для которой был собран файл
 *                  OtherFlagsToStrings(ushort):        Возвращает список расшифрованных флагов загрузчика для OS/2
 *                  WindowsVersionToString(byte, byte): Возвращает требуемую версию Windows для двичного файла
 */
public static class Information
{
    /// <summary>
    /// Читает флаги модуля
    /// Ищет сопоставления, переводит натуральный язык
    /// </summary>
    /// <param name="mFlags"></param>
    /// <returns>Результат сопоставлений с определениями</returns>
    public static string[] ProgramFlagsToStrings(byte mFlags)
    {
        return new[]
            {
                ((mFlags & (byte)ProgramFlags.ProtectedMode) != 0) ? "Защищенный режим" : string.Empty,
                ((mFlags & (byte)ProgramFlags.DataSingleShared) != 0) ? "Одна .data секция" : string.Empty,
                ((mFlags & (byte)ProgramFlags.DataMultipleShared) != 0) ? "Много .data секций" : string.Empty
            }
            .Where(item => !string.IsNullOrEmpty(item))
            .Distinct()
            .ToArray();
    }

    /// <summary>
    /// Сопоставляет флаг с определениями процессоров
    /// </summary>
    /// <param name="cpuFlag"></param>
    /// <returns>Название архитектуры</returns>
    public static string ProcessorFlagToString(byte cpuFlag)
    {
        if ((cpuFlag & (byte)ProgramFlags.I8086Instructions) != 0) return "Intel x86 (8086)";
        else if ((cpuFlag & (byte)ProgramFlags.I8087Instructions) != 0) return "Intel x87 (8087)";
        else if ((cpuFlag & (byte)ProgramFlags.I286Instructions) != 0) return "Intel x86 (i286)";
        else if ((cpuFlag & (byte)ProgramFlags.I386Instructions) != 0) return "Intel x86 (i386)";
        else return "Неизвестно";
    }
    
    /// <summary>
    /// Сопоставляет флаги с определениями
    /// </summary>
    /// <param name="aFlags"></param>
    /// <returns>Расшифровку Флагов приложения</returns>
    public static string[] ApplicationFlagsToStrings(byte aFlags)
    {
        List<string> flags = new()
        {
            ((aFlags & (byte)ApplicationFlags.FullScreen) != 0) ? "Во весь экран" : string.Empty,
            ((aFlags & (byte)ApplicationFlags.WindowsPmCompatible) != 0) ? "Совместимо с OS/2 PM" : string.Empty,
            ((aFlags & (byte)ApplicationFlags.WindowsPmUsage) != 0) ? "Использует OS/2 PM" : string.Empty,
            ((aFlags & (byte)ApplicationFlags.NonConforming) != 0) ? "Неформал с района" : string.Empty,
            ((aFlags & (byte)ApplicationFlags.ImageLinkErrors) != 0) ? "Ошибка сборки" : string.Empty,
            ((aFlags & (byte)ApplicationFlags.LoadCodeSegment) != 0) ? "Сначала Загружает .code" : string.Empty,
            ((aFlags & (byte)ApplicationFlags.LibraryModule) != 0) ? "Библиотека" : string.Empty
        };
        return flags
            .Where(x => !string.IsNullOrEmpty(x))
            .Distinct()
            .ToArray();
    }

    /// <summary>
    /// Сопоставляет флаги ОС с определениями
    /// </summary>
    /// <param name="osFlag"></param>
    /// <returns></returns>
    public static string OperatingSystemFlagToString(byte osFlag)
    {
        return (OperatingSystem)osFlag switch
        {
            OperatingSystem.Windows16 => "Microsoft Windows",
            OperatingSystem.Windows386 => "Microsoft Windows",
            OperatingSystem.OperatingSystem2 => "IBM OS/2",
            OperatingSystem.BorlandOperatingSystemServices => "Borland OS Services",
            _ => "Неизвестно"
        };
    }
    /// <summary>
    /// Распознает другие флаги исполняемого файла
    /// (в основном используются OS/2)
    /// </summary>
    /// <param name="otherFlag"></param>
    /// <returns></returns>
    public static string[] OtherFlagsToStrings(ushort otherFlag)
    {
        return new[]
            {
                ((otherFlag & (ushort)OtherFlags.LongNamesSupport) != 0) ? "Большие имена" : "Правило 8.3",
                ((otherFlag & (ushort)OtherFlags.Os22xProtectedMode) != 0) ? "OS/2 2.0 Защищенный режим" : string.Empty,
                ((otherFlag & (ushort)OtherFlags.Os22xProportionalFonts) != 0) ? "Пропорционалый шрифт" : string.Empty,
                ((otherFlag & (ushort)OtherFlags.ExecHasGangloadArea) != 0)
                    ? "Есть область групповой загрузки"
                    : string.Empty
            }
            .Where(item => !string.IsNullOrEmpty(item))
            .Distinct()
            .ToArray();
    }
    
    /// <summary>
    /// Возвращает версию Windows
    /// </summary>
    /// <param name="maj"></param>
    /// <param name="minor"></param>
    /// <returns></returns>
    public static string WindowsVersionToString(byte maj, byte minor)
    {
        return (maj > 0) ? $"{maj:x}.{minor:x}" : string.Empty;
    }
}