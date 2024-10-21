using jellybins.Models;

namespace jellybins.Binary;

/*
 * Jelly Bins (C) Толстопятов Алексей 2024
 *      Linear Executable Information Block
 * Класс предоставляющий возможность распознавания флагов 
 * из линейного заголовка заголовка.
 * Члены класса:    OperatingSystemFlagToString(ushort):     Возвращает ОС для которой собран двоичный файл
 *                  ProcessorFlagToString(ushort):   Возвращает флаг архитектуры процессора, для которого был собран файл
 *                  ModuleFlagsToStrings(uint): Возвращает список расшифрованных флагов из таблицы определения файла
 *
 */
public static class LeInformationBlock
{
    /// <summary>
    /// Читает флаг ОС для двоичного файла из заголовка
    /// Ищет сопоставления, переводит натуральный язык
    /// </summary>
    /// <param name="mFlags"></param>
    /// <returns>Результат сопоставлений с определениями</returns>
    public static string OperatingSystemFlagToString(ushort os)
    {
        return (LinearOperatingSystemFlag)os switch
        {
            LinearOperatingSystemFlag.OS2 => "IBM OS/2",
            LinearOperatingSystemFlag.Windows => "Microsoft Windows",
            LinearOperatingSystemFlag.Windows386 => "Microsoft Windows/386",
            LinearOperatingSystemFlag.DOS => "Microsoft DOS",
            _ => string.Empty
        };
    }

    /// <summary>
    /// Читает флаг архитектуры модуля
    /// Ищет сопоставления, переводит натуральный язык
    /// </summary>
    /// <param name="mFlags"></param>
    /// <returns>Результат сопоставлений с определениями</returns>
    public static string ProcessorFlagToString(ushort cpu)
    {
        return (LinearArchitecture)cpu switch
        {
            LinearArchitecture.I286Instructions => "Intel x86 (i286)",
            LinearArchitecture.I386Instructions => "Intel x86 (i386)",
            LinearArchitecture.I486Instructions => "Intel x86 (i486)",
            _ => string.Empty
        };
    }

    /// <summary>
    /// Читает флаги модуля
    /// Ищет сопоставления, переводит натуральный язык
    /// </summary>
    /// <param name="mFlags"></param>
    /// <returns>Результат сопоставлений с определениями</returns>
    public static string[] ModuleFlagsToStrings(uint mFlags)
    {
        return new[]
        {
            ((mFlags & (uint)LinearModuleFlags.PerProcessInit) != 0)? "Запуск по-процессно" : string.Empty,
            ((mFlags & (uint)LinearModuleFlags.ProgramModule) != 0)? "Приложение" : string.Empty,
            ((mFlags & (uint)LinearModuleFlags.LibraryModule) != 0)? "Библиотека" : string.Empty,
            ((mFlags & (uint)LinearModuleFlags.CompatiblePm) != 0)? "Совместим с OS/2 PM" : string.Empty,
            ((mFlags & (uint)LinearModuleFlags.InCompatiblePm) != 0)? "Несовместим с OS/2 PM" : string.Empty,
            ((mFlags & (uint)LinearModuleFlags.UsesPm) != 0)? "Использует OS/2 PM" : string.Empty,
            ((mFlags & (uint)LinearModuleFlags.PerProcessTermination) != 0)? "Уничтожение по-процессно" : string.Empty,
            ((mFlags & (uint)LinearModuleFlags.NotLoadable) != 0)? "Не загружается" : string.Empty,
            ((mFlags & (uint)LinearModuleFlags.PhysicalDriver) != 0)? "Драйвер физического устройства" : string.Empty,
            ((mFlags & (uint)LinearModuleFlags.VirtualDriver) != 0)? "Драйвер виртуального устройства" : string.Empty,
            ((mFlags & (uint)LinearModuleFlags.MultiCpuUnsafe) != 0)? "Только для 1 ядра" : string.Empty
        };
    }
}