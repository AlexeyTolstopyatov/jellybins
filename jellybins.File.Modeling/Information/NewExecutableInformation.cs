using jellybins.File.Modeling.Base;
using jellybins.Report.Common.NewExecutable;
using OperatingSystem = jellybins.Report.Common.NewExecutable.OperatingSystem;

namespace jellybins.File.Modeling.Information;

public class NewExecutableInformation : IExecutableInformation
{
    public string ProcessorFlagToString<T>(T flag) where T : IComparable
    {
        byte f = Convert.ToByte(flag);
        if ((f & (byte)ProgramFlags.I8086Instructions) != 0) return "Intel x86 (8086)";
        if ((f & (byte)ProgramFlags.I286Instructions) != 0) return "Intel x86 (i286)";
        if ((f & (byte)ProgramFlags.I386Instructions) != 0) return "Intel x86 (i386)";
        if ((f & (byte)ProgramFlags.I8087Instructions) != 0) return "Intel x87 (8087)";
        
        return "Неизвестно";
    }

    public string OperatingSystemFlagToString<T>(T? flag) where T : IComparable
    {
        return Convert.ToByte(flag) switch
        {
            (byte)OperatingSystem.Windows16 => "Microsoft Windows/286",
            (byte)OperatingSystem.Windows386 => "Microsoft Windows",
            (byte)OperatingSystem.OperatingSystem2 => "IBM OS/2",
            (byte)OperatingSystem.BorlandOperatingSystemServices => "Borland OS",
            _ => "Неизвестно"
        };
    }

    public string VersionFlagsToString<TVersion>(TVersion major, TVersion minor) where TVersion : IComparable
    {
        return $"{major:x}.{minor:x}";
    }

    public string[] FlagsToStrings(object flags)
    {
        return new[]
        {
            (((byte)flags & (byte)ProgramFlags.ProtectedMode) != 0) ? "Защищенный режим" : string.Empty,
            (((byte)flags & (byte)ProgramFlags.DataSingleShared) != 0) ? "Одна .data секция" : string.Empty,
            (((byte)flags & (byte)ProgramFlags.DataMultipleShared) != 0) ? "Много .data секций" : string.Empty
        }
        .Where(item => !string.IsNullOrEmpty(item))
        .Distinct()
        .ToArray();
    }
    
    public string[] ApplicationFlagsToStrings(byte aFlags)
    {
        List<string> flags = new()
        {
            ((aFlags & (byte)ApplicationFlags.FullScreen) != 0) ? "Во весь экран" : string.Empty,
            ((aFlags & (byte)ApplicationFlags.WindowsPmCompatible) != 0) ? "Совместимо с OS/2 PM" : string.Empty,
            ((aFlags & (byte)ApplicationFlags.WindowsPmUsage) != 0) ? "Использует OS/2 PM" : string.Empty,
            ((aFlags & (byte)ApplicationFlags.NonConforming) != 0) ? "Не загружается" : string.Empty,
            ((aFlags & (byte)ApplicationFlags.ImageLinkErrors) != 0) ? "Ошибка сборки" : string.Empty,
            ((aFlags & (byte)ApplicationFlags.LoadCodeSegment) != 0) ? "Сначала Загружает .text" : string.Empty,
            ((aFlags & (byte)ApplicationFlags.LibraryModule) != 0) ? "Библиотека" : string.Empty
        };
        return flags
            .Where(x => !string.IsNullOrEmpty(x))
            .Distinct()
            .ToArray();
    }
    
    public string[] OtherFlagsToStrings(ushort otherFlag)
    {
        return new[]
            {
                ((otherFlag & (ushort)OtherFlags.LongNamesSupport) != 0) ? "Большие имена (HPFS)" : "Правило 8.3 (FAT)",
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
}