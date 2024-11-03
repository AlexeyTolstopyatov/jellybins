using Environment = jellybins.Report.Common.PortableExecutable.Environment;

namespace jellybins.File.Modeling;

/**
 * Jelly Bins (C) Толстопятов Алексей 2024
 *      File Characteristics
 * Заполняет поля
 * (подробнее)
 * Description.FileChars.md
 */
public class FileChars
{
    public FileType Type { get; private set; }
    public int MajorVersion { get; private set; }
    public int MinorVersion { get; private set; }
    public int? MinimumMajorVersion { get; set; }
    public int? MinimumMinorVersion { get; set; }
    public string Os { get; private set; }
    public int Cpu { get; set; }
    public Environment Environment { get; set; }
    
    public FileChars(
        string os,
        FileType type,
        int major,
        int minor)
    {
        MajorVersion = major;
        MinorVersion = minor;
        Type = type;
        Os = os;
    }
}