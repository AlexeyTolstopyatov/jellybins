namespace jellybins.Java.Exceptions;

public class JavaAppletTypeException : Exception
{
    public JavaAppletTypeException() : 
        base($"Unrecognized Java binary type.")
    {
    }
}