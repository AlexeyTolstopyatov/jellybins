namespace jellybins.Report.Common.PortableExecutable;

public enum Magic
{
    /// <summary>
    /// Говорит, что использовать выше 2Гб нельзя
    /// </summary>
    Application32Bit = 0x10b,
    /// <summary>
    /// А здесь можно
    /// </summary>
    Application64Bit = 0x20b,
    /// <summary>
    /// Тут вообще все можно.
    /// </summary>
    ApplicationRom = 0x107
}