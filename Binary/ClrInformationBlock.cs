namespace jellybins.Binary;

public static class ClrInformationBlock
{
    public static string[] LinkerFlagsToStrings(uint flags)
    {
        return new[]
        {
            ((flags & 0x02) != 0) ? "32-разрядный" : string.Empty,
            ((flags & 0x08) != 0) ? "Есть Строгие наименования" : string.Empty,
            ((flags & 0x01) != 0) ? "IL ONLY" : string.Empty,
            ((flags & 0x01000) != 0) ? "Есть Данные отладки" : string.Empty
        };
    }
}