using jellybins.Core.Attributes;
using jellybins.Core.Exceptions;
using jellybins.Core.Interfaces;
using static jellybins.Core.Strings.Processor;

namespace jellybins.Core.Strings;

/// <summary>
/// Represents definition logic for Assembler-Output format of executables
/// All definitions taken from MINIX 1 sources.
/// </summary>
public class AssemblerOutExecutableStrings : IStrings
{
    [UnderConstruction]
    public string OperatingSystemFlagToString<T>(T os = default(T)) where T : IComparable
    {
        return "Minix";
    }

    public string CpuArchitectureFlagToString<T>(T cpu) where T : IComparable
    {
        // taken from FreeBSD / MINIX 1 loader's behaviour
        uint cpuf = Convert.ToUInt32(cpu);
        return cpuf switch
        {
            (uint)Alpha => "Alpha BSD",
            (uint)Arm6 => "ARM 6",
            (uint)Hp200 => "HP 200",
            (uint)Hp300 => "HP 300",
            (uint)Hppa => "HP PA-RISC",
            (uint)Ia32 => "Intel i386",
            (uint)Ia64 => "Intel Itanuim",
            (uint)Ia32E => "Intel x86-64",
            (uint)Mips1 => "MIPS 1",
            (uint)Mips2 => "MIPS 2",
            (uint)Ns32532 => "NS 32500",
            (uint)Pc386 => "PC 386",
            (uint)Sh3 or (uint)Sh532 => "SH3",
            (uint)Sh5 => "SH5",
            (uint)Spark64 => "Sun SPARK",
            (uint)Sun68010 => "Sun SPARK 68010",
            (uint)Sun68020 => "Sun SPARK 68020",
            (uint)Vax => "VAX",
            (uint)HpUx => "HP-UX 200-300",
            (uint)Motorola68K 
                or (uint)Motorola68K2K 
                or (uint)Motorola68K4K => "Motorola 68000",
            (uint)Motorola88K => "Motorola 88000",
            (uint)PMax => "PMAX",
            (uint)RiscV => "RISC-V",
            (uint)Vax1K => "VAX",
            (uint)OpenRisc1K => "Open RISC",
            (uint)BigEndianPowerPc => "Power PC",
            _ => "Unknown"
        };
    }
    
    [UnderConstruction("Not working with A-Out files.")]
    public string OperatingSystemVersionToString<T>(T major, T minor) where T : IComparable
    {
        return "1.1.2";
    }

    public string ImageVersionFlagsToString<T>(T major, T minor)
    {
        return $"{major}.{minor}";
    }

    public string CpuWordLengthFlagToString<T>(T word = default(T))
    {
        // taken from MINIX 1 loader behaviour
        uint flag = Convert.ToUInt32(word);
        if ((flag & (uint)Alpha) != 0) return "32";
        if ((flag & (uint)Arm6) != 0) return "32";
        if ((flag & (uint)Hp200) != 0) return "16";
        if ((flag & (uint)Hp300) != 0) return "16";
        if ((flag & (uint)Hppa) != 0) return "32";
        if ((flag & (uint)Ia32) != 0) return "32";
        if ((flag & (uint)Ia64) != 0) return "64";
        if ((flag & (uint)Ia32E) != 0) return "64";
        if ((flag & (uint)Mips1) != 0) return "40";
        if ((flag & (uint)Mips2) != 0) return "32";
        if ((flag & (uint)Ns32532) != 0) return "32";
        if ((flag & (uint)Pc386) != 0) return "32";
        if ((flag & (uint)Sh3) != 0) return "64";
        if ((flag & (uint)Sh532) != 0) return "32";
        if ((flag & (uint)Sh5) != 0) return "64";
        if ((flag & (uint)Spark64) != 0) return "64";
        if ((flag & (uint)Sun68010) != 0) return "32";
        if ((flag & (uint)Sun68020) != 0) return "32";
        if ((flag & (uint)Vax) != 0) return "32";
        if ((flag & (uint)HpUx) != 0) return "32";
        if ((flag & (uint)Motorola68K) != 0) return "32";
        if ((flag & (uint)Motorola68K2K) != 0) return "32";
        if ((flag & (uint)Motorola68K4K) != 0) return "32";
        if ((flag & (uint)Motorola88K) != 0) return "32";
        if ((flag & (uint)PMax) != 0) return "32";
        if ((flag & (uint)RiscV) != 0) return "64";
        if ((flag & (uint)Vax1K) != 0) return "32";
        if ((flag & (uint)OpenRisc1K) != 0) return "64";
        if ((flag & (uint)BigEndianPowerPc) != 0) return "32";
        return "16";
    }

    public string SubsystemFromOperatingSystemFlagToString<T>(T word = default(T))
    {
        return Subsystem.Native.ToString();
    }

    public string ImageTypeFlagToString<T>(T word)
    {
        ushort flags = Convert.ToUInt16(word);
        return (flags & 0x30) switch
        {
            0x01 => ImageType.ObjectFile.ToString(),
            0x10 => ImageType.DynamicLinkedLibrary.ToString(), 
            0x11 => ImageType.StaticLinkedLibrary.ToString(), // may be context in someone API/ABI. I'm stupid
            _ => ImageType.Application.ToString()
        };
    }

    /// <summary>
    /// Returns Magic WORD type
    /// </summary>
    /// <param name="magic">magic WORD</param>
    public string MagicFlagToString(ushort magic)
    {
        return magic switch
        {
            0x129 or 0x921 or 0xcc => "Quick",
            0x10b or 0xb01 => "Zero filled",
            0x108 or 0x801 => "New",
            _ => "Old"
        };
    }
}