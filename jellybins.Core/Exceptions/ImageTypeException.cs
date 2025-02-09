namespace jellybins.Core.Exceptions;

public class ImageTypeException : Exception
{
    public ImageTypeException() : base("Unknown file's signature!") {}
    
    public ImageTypeException(int value) : 
        base($"Unknown file's signature - 0x{value:x}") {}

    public ImageTypeException(ulong value) : 
        base($"Unknown file's signature - 0x{value:x}") {}
}