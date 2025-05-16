namespace JellyBins.LinearExecutable.Private.Types;

public enum LeEntryType
{
    EntryUnused = 0x00,
    Entry16Bit = 0x01,
    Entry286CallGate = 0x02,
    Entry32Bit = 0x03,
    EntryForwarder = 0x04,
    ParameterTypingBit = 0x80
}