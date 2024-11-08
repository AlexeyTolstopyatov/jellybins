namespace jellybins.Report.Common.PortableExecutable;

public enum Processor
{
    /// <summary>
    /// Intel i386 и позже
    /// </summary>
    Ia32 = 0x014c,
    /// <summary>
    /// Intel x86-64 или amd64 или IA32 Extended
    /// </summary>
    Ia32E = 0x8664,
    /// <summary>
    /// Intel Itanium
    /// </summary>
    Ia64 = 0x0200
}