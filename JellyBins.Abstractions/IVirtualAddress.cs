namespace JellyBins.Abstractions;

public interface IVirtualAddress
{
    Int64 Offset(UInt32 offset);
    Int64 Offset(UInt16 offset);
}