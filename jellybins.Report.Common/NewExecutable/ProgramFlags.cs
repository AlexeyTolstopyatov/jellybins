namespace jellybins.Report.Common.NewExecutable;

public enum ProgramFlags : byte
{
    DataGroupType = 0,
    DataNone = 0x01,
    DataSingleShared = 0x02,
    DataMultipleShared = 0x03,
    GlobalInit = 0x2,
    ProtectedMode = 0x08,
    I8086Instructions = 0x4,
    I286Instructions = 0x5,
    I386Instructions = 0x6,
    I8087Instructions = 0x7
}