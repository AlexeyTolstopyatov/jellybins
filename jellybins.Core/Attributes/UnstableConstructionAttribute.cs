namespace jellybins.Core.Attributes;

/// <summary>
/// Method works with unexpected details or has detected bugs
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class UnstableConstructionAttribute : Attribute
{
    private string _shortenMessage;
    
    public UnstableConstructionAttribute(string shortenMessage)
    {
        _shortenMessage = shortenMessage;
    }

    public UnstableConstructionAttribute()
    {
        _shortenMessage = "Be careful when using this object";
    }
}