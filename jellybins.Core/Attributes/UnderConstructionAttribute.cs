namespace jellybins.Core.Attributes;

[AttributeUsage(AttributeTargets.All)]
public sealed class UnderConstructionAttribute : Attribute
{
    private string _warning;

    public UnderConstructionAttribute(string warning)
    {
        _warning = warning;
    }

    public UnderConstructionAttribute()
    {
        _warning = "The object may be unfinished or not implemented at all.";
    }
}