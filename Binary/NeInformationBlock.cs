using jellybins.Models;

namespace jellybins.Binary;
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
public static class NeInformationBlock
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
            ((mFlags & (byte)NewProgramType.ProtectedMode) != 0) ? "Защищенный режим" : string.Empty,
            ((mFlags & (byte)NewProgramType.DataSingleShared) != 0) ? "Одна .data секция" : string.Empty,
            ((mFlags & (byte)NewProgramType.DataMultipleShared) != 0) ? "Много .data секций" : string.Empty
        };
    }

    /// <summary>
    /// Сопоставляет флаг с определениями процессоров
    /// </summary>
    /// <param name="cpuFlag"></param>
    /// <returns>Название архитектуры</returns>
    public static string ProcessorFlagToString(byte cpuFlag)
    {
        if ((cpuFlag & (byte)NewProgramType.I8086Instructions) != 0) return "Intel x86 (8086)";
        else if ((cpuFlag & (byte)NewProgramType.I8087Instructions) != 0) return "Intel x87 (8087)";
        else if ((cpuFlag & (byte)NewProgramType.I286Instructions) != 0) return "Intel x86 (i286)";
        else if ((cpuFlag & (byte)NewProgramType.I386Instructions) != 0) return "Intel x86 (i386)";
        else return "Неизвестно";
    }
    
    /// <summary>
    /// Сопоставляет флаги с определениями
    /// </summary>
    /// <param name="aFlags"></param>
    /// <returns>Расшифровку Флагов приложения</returns>
    public static string[] ApplicationFlagsToStrings(byte aFlags)
    {
        return new[]
        {
            ((aFlags & (byte)NewApplicationType.FullScreen) != 0) ? "Во весь экран" : string.Empty,
            ((aFlags & (byte)NewApplicationType.WindowsPmCompatible) != 0) ? "Совместимо с OS/2 PM" : string.Empty,
            ((aFlags & (byte)NewApplicationType.WindowsPmUsage) != 0) ? "Использует OS/2 PM" : string.Empty,
            ((aFlags & (byte)NewApplicationType.NonConforming) != 0) ? "Неформал с района" : string.Empty,
            ((aFlags & (byte)NewApplicationType.ImageLinkErrors) != 0) ? "Ошибка сборки" : string.Empty,
            ((aFlags & (byte)NewApplicationType.LoadCodeSegment) != 0) ? "Сначала Загружает .code" : string.Empty,
            ((aFlags & (byte)NewApplicationType.LibraryModule) != 0) ? "Библиотека" : string.Empty
        };
    }

    /// <summary>
    /// Сопоставляет флаги ОС с определениями
    /// </summary>
    /// <param name="osFlag"></param>
    /// <returns></returns>
    public static string OperatingSystemFlagToString(byte osFlag)
    {
        return (NewRequiredOperatingSystem)osFlag switch
        {
            NewRequiredOperatingSystem.Windows16 => "Microsoft Windows",
            NewRequiredOperatingSystem.Windows386 => "Microsoft Windows",
            NewRequiredOperatingSystem.OperatingSystem2 => "IBM OS/2",
            NewRequiredOperatingSystem.BorlandOperatingSystemServices => "Borland OS Services",
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
            ((otherFlag & (ushort)NewOtherLoaderFlags.LongNamesSupport) != 0) ? "Большие имена" : "Правило 8.3",
            ((otherFlag & (ushort)NewOtherLoaderFlags.Os22xProtectedMode) != 0) ? "OS/2 2.0 Защищенный режим" : string.Empty,
            ((otherFlag & (ushort)NewOtherLoaderFlags.Os22xProportionalFonts) != 0) ? "Пропорционалый шрифт" : string.Empty,
            ((otherFlag & (ushort)NewOtherLoaderFlags.ExecHasGangloadArea) != 0)? "Есть область групповой загрузки" : string.Empty
        };
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