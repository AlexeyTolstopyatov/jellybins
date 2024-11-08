namespace jellybins.Report.Common.PortableExecutable.Dotnet;

public static class Information
{
    public static string[] LinkerFlagsToStrings(uint flags)
    {
        return new[]
        {
            ((flags & 0x02) != 0) ? "32-разрядный" : string.Empty,
            ((flags & 0x08) != 0) ? "Подписано сильным именем" : string.Empty,
            ((flags & 0x01) != 0) ? "Только IL" : string.Empty,
            ((flags & 0x01000) != 0) ? "Есть Данные отладки" : string.Empty
        };
    }
}