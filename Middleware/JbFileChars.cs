using System.Windows.Navigation;
using jellybins.Models;

namespace jellybins.Binary;

/*
 * Jelly Bins (C) Толстопятов Алексей 2024
 *      Binary Characteristics
 * Отвечает за хранение основных характеристик двоичных файлов
 * 
 */
public class JbFileChars
{
    public JbFileType Type { get; private set; }
    public int MajorVersion { get; private set; }
    public int MinorVersion { get; private set; }
    public JbFileOs Os { get; private set; }
    
    public PortableEnvironment Environment { get; private set; }

    public JbFileChars(
        JbFileOs os,
        JbFileType type,
        int major,
        int minor)
    {
        MajorVersion = major;
        MinorVersion = minor;
        Type = type;
        Os = os;
    }
}

public enum JbFileOs : int
{
    Unknown = -1,
    Dos = 0,
    Windows = 1,
    Os2 = 2,
    Unix = 3
}