namespace JellyBins.PortableExecutable.Private.Types;

public enum RichMachineType
{
    Ia32E = 0x8664,
    Ia32 = 0x14c,
    Ia64 = 0x200,
    Am33 = 0x1d3,
    ArmLowEndian = 0x1c0,
    ArmV7 = 0x1c4,
    ArmV8X64 = 0xaa64,
    EfiByteCode = 0xebc,
    MitsubishiLowEndian = 0x9041,
    MipsLowEndian = 0x166,
    Mips16 = 0x266,
    MipsFpu = 0x366,
    Mips16Fpu = 0x466,
    PowerPcLowEndian = 0x1f0,
    PowerPcFpu = 0x1f1,
}