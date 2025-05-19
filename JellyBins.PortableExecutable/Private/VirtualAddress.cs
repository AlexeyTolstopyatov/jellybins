using JellyBins.Abstractions;
using JellyBins.PortableExecutable.Headers;

namespace JellyBins.PortableExecutable.Private;

public class VirtualAddress(UInt64 imageBase) : IVirtualAddress
{
    /// <param name="offset">Raw file pointer</param>
    /// <returns> Virtual Address <c>VA</c> </returns>
    /// <exception cref="NotImplementedException"></exception>
    public Int64 Offset(UInt32 offset)
    {
        throw new NotImplementedException();
    }
    /// <param name="offset">Raw file pointer</param>
    /// <returns> Virtual Address <c>VA</c> </returns>
    /// <exception cref="NotImplementedException"></exception>
    public Int64 Offset(UInt16 offset)
    {
        throw new NotImplementedException();
    }
    /// <param name="section"> <c>IMAGE_SECTION_HEADER</c> <see cref="PeSection"/></param>
    /// <param name="rva"> relative virtual address </param>
    /// <returns>Raw file offset of section's data</returns>
    public Int64 RawOffset(Int64 rva, PeSection section)
    {
        return rva - section.VirtualAddress + section.PointerToRawData;
    }
}