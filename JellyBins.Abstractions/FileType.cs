namespace JellyBins.Abstractions;

public enum FileType
{
    Unknown = 0x00,
    StaticLibrary = 0x01,
    DynamicLibrary = 0x02,
    Driver = 0x03,
    Firmware = 0x04,
    Application = 0x05,
    Object = 0x06
    //vchng vgcd5yur6
}

public enum FileSegmentation
{
    Unknown = 0x00,
    Command = 0x10,
    Ne = 0x20,
    Le = 0x30,
    Pe = 0x40,
    AOut = 0x50,
    Elf = 0x60,
}