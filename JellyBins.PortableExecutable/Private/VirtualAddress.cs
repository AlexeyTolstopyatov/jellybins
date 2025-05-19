using JellyBins.Abstractions;

namespace JellyBins.PortableExecutable.Private;

public class VirtualAddress(UInt64 imageBase) : IVirtualAddress
{
    public Int64 Offset(UInt32 offset)
    {
        throw new NotImplementedException();
    }

    public Int64 Offset(UInt16 offset)
    {
        throw new NotImplementedException();
    }

    public Int64 RelativeOffset(UInt64 offset)
    {
        return Convert.ToInt64(offset - imageBase);
    }
}