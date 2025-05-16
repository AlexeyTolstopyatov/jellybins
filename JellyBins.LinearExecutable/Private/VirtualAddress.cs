using JellyBins.Abstractions;

namespace JellyBins.LinearExecutable.Private;

public class VirtualAddress(UInt64 headerPointer) : IVirtualAddress
{
    /// <summary>
    /// Counts Offset from Linear header
    /// beginning till required field
    /// </summary>
    /// <param name="offset">field's offset</param>
    /// <returns></returns>
    public Int64 Offset(UInt32 offset)
    {
        return Convert.ToInt64(offset + headerPointer);
    }
    /// <summary>
    /// Counts Offset from Linear header
    /// beginning till required field
    /// </summary>
    /// <param name="offset"></param>
    /// <returns></returns>
    public Int64 Offset(UInt16 offset)
    {
        return Convert.ToInt64(offset + headerPointer);
    }
}