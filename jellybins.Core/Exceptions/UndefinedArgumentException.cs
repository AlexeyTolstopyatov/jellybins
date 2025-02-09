namespace jellybins.Core.Exceptions;

public class UndefinedArgumentException : Exception
{
    public UndefinedArgumentException(int value) : 
        base($"Unknown flag {value} (0x{value:x}).")
    {
    }
}