namespace jellybins.Core.Strings;

internal enum Processor
{
    /// <summary>
    /// Unknown
    /// </summary>
    Zero = 0,
    /// <summary>
    /// Sun 68010/68020
    /// </summary>
    Sun68010 = 0x01,
    /// <summary>
    /// Only Sun 68020
    /// </summary>
    Sun68020 = 0x02,
    /// <summary>
    /// I don't know what is it.
    /// </summary>
    Pc386 = 0x64,
    /// <summary>
    /// База
    /// </summary>
    Ia32 = 0x86, // Intel x86
    /// <summary>
    /// Motorola 68K Memory page 8Kib
    /// </summary>
    Motorola68K = 0x87, // Motorola 68.000 8KB memory page
    /// <summary>
    /// Motorola 68K Memory page 4Kib
    /// </summary>
    Motorola68K4K = 0x88, // Motorola 68.000 4KB memory page
    /// <summary>
    /// Motorola 68K Memory page 2Kib
    /// </summary>
    Motorola68K2K = 0x90, // Motorola 68.000 2KB memory page
    /// <summary>
    /// NS 325 (32-bit)
    /// </summary>
    Ns32532 = 0x89,
    /// <summary>
    /// Sun Spark
    /// </summary>
    SunSpark = 0x8a,
    /// <summary>
    /// P-MAX
    /// </summary>
    PMax = 0x8b,
    /// <summary>
    /// Vax 1Kб Размер страницы памяти
    /// </summary>
    Vax1K = 0x8c,
    /// <summary>
    /// Alpha BSD
    /// </summary>
    Alpha = 0x8d,
    /// <summary>
    /// MIPS (32-bit)
    /// </summary>
    BigEndianMips = 0x8e,
    /// <summary>
    /// Advanced RISC Machine 6 (32-bit)
    /// </summary>
    Arm6 = 0x8f,
    /// <summary>
    /// SH3
    /// </summary>
    Sh3 = 0x91,
    /// <summary>
    /// PowerPC
    /// Big Endian
    /// </summary>
    BigEndianPowerPc = 0x95,
    /// <summary>
    /// VAX
    /// </summary>
    Vax = 0x96,
    /// <summary>
    /// MIPS 1
    /// </summary>
    Mips1 = 0x97,
    /// <summary>
    /// MIPS 2
    /// </summary>
    Mips2 = 0x98,
    /// <summary>
    /// Motorola 88K
    /// Memory page 8Kib
    /// </summary>
    Motorola88K = 0x99,
    /// <summary>
    /// HP PA-RISC
    /// </summary>
    Hppa = 0x9a,
    /// <summary>
    /// SH5 (64-bit)
    /// </summary>
    Sh5 = 0x9b,
    /// <summary>
    /// Sun Spark (64-bit)
    /// </summary>
    Spark64 = 0x9c,
    /// <summary>
    /// Intel x86-64
    /// или AMD-64
    /// </summary>
    Ia32E = 0x9d,
    /// <summary>
    /// SH5 (32-bit)
    /// </summary>
    Sh532 = 0x9e,
    /// <summary>
    /// Intel Itanium (64-bit)
    /// </summary>
    Ia64 = 0x9f,
    /// <summary>
    /// AARCH (64-bit)
    /// </summary>
    Aarch64 = 0xb7,
    /// <summary>
    /// Open RISC 1000
    /// </summary>
    OpenRisc1K = 0xb8,
    /// <summary>
    /// RISC-V
    /// </summary>
    RiscV = 0xb9,
    /// <summary>
    /// HP-200 BSD 
    /// </summary>
    Hp200 = 0xc8,
    /// <summary>
    /// HP-300 BSD
    /// 68020 + 68881
    /// </summary>
    Hp300 = 0x12c,
    /// <summary>
    /// HP-UX 200-300 
    /// </summary>
    HpUx = 0x2c
}