using System.Diagnostics;
using jellybins.File.Modeling.Base;
using jellybins.Report.Common.LinearExecutable;
using OperatingSystem = jellybins.Report.Common.LinearExecutable.OperatingSystem;

namespace jellybins.File.Modeling.Descriptions;

public class LinearExecutableInformation : IExecutableInformation
{
    public string ProcessorFlagToString<T>(T flag) where T : IComparable
    {
        return flag switch
        {
            Processor.I286Instructions => "Intel x86 (i286)",
            Processor.I386Instructions => "Intel x86 (i386)",
            Processor.I486Instructions => "Intel x86 (i486)",
            _ => string.Empty
        };
    }

    public string OperatingSystemFlagToString<T>(T? flag) where T : IComparable
    {
        return flag switch
        {
            OperatingSystem.OS2 => "IBM OS/2",
            OperatingSystem.Windows => "Microsoft Windows",
            OperatingSystem.Windows386 => "Microsoft Windows/386",
            OperatingSystem.DOS => "Microsoft DOS",
            _ => string.Empty
        };
    }

    public string VersionFlagsToString<TVersion>(TVersion major, TVersion minor) where TVersion : IComparable
    {
        return $"{major:x}.{minor:x}";
    }
    
    public string[] FlagsToStrings(object mFlags)
    {
        return new[]
        {
            (((uint)mFlags & (uint)ModuleFlag.PerProcessInit) != 0) ? "Запуск по-процессно" : string.Empty,
            (((uint)mFlags & (uint)ModuleFlag.ProgramModule) != 0) ? "Приложение" : string.Empty,
            (((uint)mFlags & (uint)ModuleFlag.LibraryModule) != 0) ? "Библиотека" : string.Empty,
            (((uint)mFlags & (uint)ModuleFlag.CompatiblePm) != 0) ? "Совместим с OS/2 PM" : string.Empty,
            (((uint)mFlags & (uint)ModuleFlag.InCompatiblePm) != 0) ? "Несовместим с OS/2 PM" : string.Empty,
            (((uint)mFlags & (uint)ModuleFlag.UsesPm) != 0) ? "Использует OS/2 PM" : string.Empty,
            (((uint)mFlags & (uint)ModuleFlag.PerProcessTermination) != 0) ? "Уничтожение по-процессно" : string.Empty,
            (((uint)mFlags & (uint)ModuleFlag.NotLoadable) != 0) ? "Не загружается" : string.Empty,
            (((uint)mFlags & (uint)ModuleFlag.PhysicalDriver) != 0) ? "Драйвер физического устройства" : string.Empty,
            (((uint)mFlags & (uint)ModuleFlag.VirtualDriver) != 0) ? "Драйвер виртуального устройства" : string.Empty,
            (((uint)mFlags & (uint)ModuleFlag.MultiCpuUnsafe) != 0) ? "Только для 1 ЦП" : string.Empty
        }.Where(x => !string.IsNullOrEmpty(x))
        .Distinct()
        .ToArray();
    }
}