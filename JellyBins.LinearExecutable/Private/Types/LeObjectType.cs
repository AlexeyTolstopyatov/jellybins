namespace JellyBins.LinearExecutable.Private.Types;

public enum LeObjectType
{
    Readable = 0x0001,
    Writable = 0x0002,
    Executable = 0x0004,
    Resource = 0x0008,
    DiscardPriority = 0x0010,
    Shared = 0x0020,
    PreloadPages = 0x0040,
    InvalidPages = 0x0080,
    ZeroPages = 0x0100,
    Resident = 0x0200,
    RequiredAlias16X16 = 0x1000,
    BigOrDefaultBitSetting = 0x2000,
    IaConforming = 0x4000,
    IoPortsPriority = 0x8000,
}