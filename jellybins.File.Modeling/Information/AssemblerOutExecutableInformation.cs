using System.Net;
using jellybins.File.Modeling.Base;
using jellybins.Report.Common.AssemblerOutExecutable;

namespace jellybins.File.Modeling.Information;

internal struct AssemblerOutDifferences
{
    public string[] Memory;
    public string Os;
    public string Bit;
}

public class AssemblerOutExecutableInformation : IExecutableInformation
{
    private AssemblerOutDifferences _preprocessor = new();
    public string ProcessorFlagToString<T>(T flag) where T : IComparable
    {
        // Я офигел малек когда узнал как вычитается значение процессора...
        // Попробую сделать это проще.
        uint flag32 = Convert.ToUInt32(flag);
        //if (((flag32) & (uint)Processor.Aarch64) != 0) return "AARCH 64";
        if (((flag32) & (uint)Processor.Alpha) != 0) return "Alpha BSD";
        if (((flag32) & (uint)Processor.Arm6) != 0) return "ARM 6";
        if (((flag32) & (uint)Processor.Hp200) != 0) return "HP 200";
        if (((flag32) & (uint)Processor.Hp300) != 0) return "HP 300";
        if (((flag32) & (uint)Processor.Hppa) != 0) return "HP PA-RISC";
        if (((flag32) & (uint)Processor.Ia32) != 0) return "Intel x86";
        if (((flag32) & (uint)Processor.Ia64) != 0) return "Intel Itanuim";
        if (((flag32) & (uint)Processor.Ia32E) != 0) return "Intel x86-64";
        if (((flag32) & (uint)Processor.Mips1) != 0) return "MIPS 1";
        if (((flag32) & (uint)Processor.Mips2) != 0) return "MIPS 2";
        if (((flag32) & (uint)Processor.Ns32532) != 0) return "NS 32500";
        if (((flag32) & (uint)Processor.Pc386) != 0) return "PC 386";
        if (((flag32) & (uint)Processor.Sh3) != 0) return "SH3";
        if (((flag32) & (uint)Processor.Sh532) != 0) return "SH3";
        if (((flag32) & (uint)Processor.Sh5) != 0) return "SH5";
        if (((flag32) & (uint)Processor.Spark64) != 0) return "Sun SPARK";
        if (((flag32) & (uint)Processor.Sun68010) != 0) return "Sun SPARK 68010";
        if (((flag32) & (uint)Processor.Sun68020) != 0) return "Sun SPARK 68020";
        if (((flag32) & (uint)Processor.Vax) != 0) return "VAX";
        if (((flag32) & (uint)Processor.HpUx) != 0) return "HP-UX 200-300";
        if (((flag32) & (uint)Processor.Motorola68K) != 0) return "Motorola 68000";
        if (((flag32) & (uint)Processor.Motorola68K2K) != 0) return "Motorola 68000";
        if (((flag32) & (uint)Processor.Motorola68K4K) != 0) return "Motorola 68000";
        if (((flag32) & (uint)Processor.Motorola88K) != 0) return "Motorola 88000";
        if (((flag32) & (uint)Processor.PMax) != 0) return "PMAX";
        if (((flag32) & (uint)Processor.RiscV) != 0) return "RISC-V";
        if (((flag32) & (uint)Processor.Vax1K) != 0) return "VAX";
        if (((flag32) & (uint)Processor.OpenRisc1K) != 0) return "Open RISC";
        if (((flag32) & (uint)Processor.BigEndianPowerPc) != 0) return "Power PC";
            
        return "Неизвестно";
    }

    public string OperatingSystemFlagToString<T>(T? flag) where T : IComparable
    {
        throw new NotImplementedException();
    }

    public string VersionFlagsToString<TVersion>(TVersion major, TVersion minor) where TVersion : IComparable
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Определяет флаги процессора на основе флага архитектуры процессора в двоичном файле
    /// </summary>
    /// <param name="flags"></param>
    /// <returns></returns>
    public string[] FlagsToStrings(object flags)
    {
        uint flag = Convert.ToUInt32(flags);
        
        //if ((flag & (uint)Processor.Aarch64) != 0) return new[] { "64-разрядный" };
        if ((flag & (uint)Processor.Alpha) != 0) return new[] { "32-разрядный" };
        if ((flag & (uint)Processor.Arm6) != 0) return new[] { "32-разрядный" };
        if ((flag & (uint)Processor.Hp200) != 0) return new [] { "16-разрядный" };
        if ((flag & (uint)Processor.Hp300) != 0) return new [] { "16-разрядный" };
        if ((flag & (uint)Processor.Hppa) != 0) return new [] { "32-разрядный" };
        if ((flag & (uint)Processor.Ia32) != 0) return new [] { "32-разрядный", ".386 инструкции" };
        if ((flag & (uint)Processor.Ia64) != 0) return new [] { "64-разрядный" };
        if ((flag & (uint)Processor.Ia32E) != 0) return new [] { "64-разрядный", ".386 инструкции" };
        if ((flag & (uint)Processor.Mips1) != 0) return new [] { "40-разрядный" };
        if ((flag & (uint)Processor.Mips2) != 0) return new [] { "32-разрядный" };
        if ((flag & (uint)Processor.Ns32532) != 0) return new [] { "32-разрядный" };
        if ((flag & (uint)Processor.Pc386) != 0) return new[] { "32-разрядный" };
        if ((flag & (uint)Processor.Sh3) != 0) return new [] { "64-разрядный" };
        if ((flag & (uint)Processor.Sh532) != 0) return new[] { "32-разрядный" };
        if ((flag & (uint)Processor.Sh5) != 0) return new []{ "64-разрядный" };
        if ((flag & (uint)Processor.Spark64) != 0) return new [] { "64-разрядный" };
        if ((flag & (uint)Processor.Sun68010) != 0) return new [] { "32-разрядный" };
        if ((flag & (uint)Processor.Sun68020) != 0) return new [] { "32-разрядный" };
        if ((flag & (uint)Processor.Vax) != 0) return new [] { "32-разрядный" };
        if ((flag & (uint)Processor.HpUx) != 0) return new [] { "32-разрядный" };
        if ((flag & (uint)Processor.Motorola68K) != 0) return new [] { "32-разрядный" };
        if ((flag & (uint)Processor.Motorola68K2K) != 0) return new [] { "32-разрядный" };
        if ((flag & (uint)Processor.Motorola68K4K) != 0) return new [] { "32-разрядный" };
        if ((flag & (uint)Processor.Motorola88K) != 0) return new[] { "32-разрядный" };
        if ((flag & (uint)Processor.PMax) != 0) return new [] { "32-разрядный" };
        if ((flag & (uint)Processor.RiscV) != 0) return new [] { "64-разрядный" };
        if ((flag & (uint)Processor.Vax1K) != 0) return new [] { "32-разрядный" };
        if ((flag & (uint)Processor.OpenRisc1K) != 0) return new [] { "64-разрядный" };
        if ((flag & (uint)Processor.BigEndianPowerPc) != 0) return new [] { "32-разрядный" };
        
        return new[]{ "Неопределено" };
    }

    /// <summary>
    /// Определяет тип магического числа двоичного файла
    /// Записывает параметры для загрузчика двоичного файла
    /// Чтобы получить данные загрузчика <seealso cref="MemoryFlagsToStrings"/>
    /// используйте <c>MemoryFlagsToStrings</c>
    /// </summary>
    /// <param name="magic"></param>
    /// <returns></returns>
    public string MagicFlagToString(ushort magic)
    {
        switch (magic)
        {
            case 0x129 or 0x921:
            case 0xcc:
                _preprocessor.Memory =new [] { "16-разрядные команды.", "Только чтение", "Компактная модель" };
                return "Q-MAGIC";
            case 0x10b or 0xb01:
                _preprocessor.Memory = new [] {"Пространство между блоками", "Только чтение"};
                return "Z-MAGIC";
            case 0x108 or 0x801:
                _preprocessor.Memory = new[] { "Чтение и запись", "Поддержка больших данных" };
                return "N-MAGIC";
            default:
                _preprocessor.Memory = new[] { "Стандартная модель", "Только чтение", "Ограничения размера страниц" };
                return "O-MAGIC";
        }
    }

    /// <summary>
    /// Работает только после вызова <c>MagicFlagToString</c>
    /// </summary>
    /// <returns></returns>
    public string[] MemoryFlagsToStrings() => _preprocessor.Memory;

    /// <summary>
    /// Определяет каким типом в системе двоичный файл является
    /// </summary>
    /// <returns></returns>
    public string[] ApplicationFlagToString(ushort flags)
    {
        return new[]
        {
            ((flags & 0x10) != 0) ? "Исполняемый файл" : string.Empty,
            ((flags & 0x01) != 0) ? "Неотмечена нулевая страница" : string.Empty,
            ((flags & 0x02) != 0) ? "Вывравненные секции" : string.Empty,
            ((flags & 0x04) != 0) ? "Новый стиль таблицы символов" : string.Empty,
            ((flags & 0x08) != 0) ? "Образ внутри файла" : string.Empty,
            ((flags & 0x20) != 0) ? ".data и .text разделяются" : string.Empty,
            ((flags & 0x40) != 0) ? "Только .text" : string.Empty,
            ((flags & 0x80) != 0) ? "Наложение .text" : string.Empty
        }.ToList()
            .Where(x => !string.IsNullOrEmpty(x))
            .ToArray();
    }
}