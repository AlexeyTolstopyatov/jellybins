using System.Runtime.InteropServices;
using jellybins.Core.Attributes;
using jellybins.Core.Exceptions;
using jellybins.Core.Headers;
using jellybins.Core.Interfaces;
using jellybins.Core.Readers.MarkZbykowski;
using jellybins.Core.Readers.PortableExecutable;

namespace jellybins.Core.Readers.Factory;

/// <summary>
/// Represents factory method for
/// detecting segmentation of image.
/// </summary>
public class SectionsFactory
{
    [UnderConstruction]
    public static ISectionsReader CreateReader<T>(string fileName, T signature)
    {
        // detect signs
        if (Marshal.SizeOf(signature) == sizeof(ulong))  throw new ImageTypeException();
        if (Marshal.SizeOf(signature) != sizeof(ushort)) throw new ImageTypeException();
        
        ushort convertedSignature = Convert.ToUInt16(signature);
        
        return convertedSignature switch
        {
            0x5a4d or 0x4d5a => new MarkZbikowskiSectionsReader(fileName),
            0x5045 or 0x4550 => new PortableExecutableSectionsReader(fileName),
            _ => throw new ImageTypeException()
        };
    }
}