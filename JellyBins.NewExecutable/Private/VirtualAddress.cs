using JellyBins.Abstractions;

namespace JellyBins.NewExecutable.Private;

public class VirtualAddress(UInt32 headerPointer) : IVirtualAddress
{
    public Int64 Offset(UInt32 offset)
    {
        return headerPointer + offset;
    }

    public Int64 Offset(UInt16 offset)
    {
        return headerPointer + offset;
    }
}