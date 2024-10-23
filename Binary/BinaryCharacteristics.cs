namespace jellybins.Binary;

/*
 * Jelly Bins (C) Толстопятов Алексей 2024
 *      Binary Characteristics
 * Отвечает за хранение основных характеристик двоичных файлов
 * 
 */
public class BinaryCharacteristics
{
    private int _majorVersion = 0;
    private int _minorVersion = 0;
    private RequiredOperatingSystem _os = RequiredOperatingSystem.Unknown;
    private BinaryType _type = BinaryType.Other;
    
    public BinaryType Type { get; private set; }
    public int MajorVersion { get; private set; }
    public int MinorVersion { get; private set; }
    public RequiredOperatingSystem TargetOs { get; private set; }

    public BinaryCharacteristics()
    {
        MajorVersion = _majorVersion;
        MinorVersion = _minorVersion;
        Type = _type;
        TargetOs = _os;
    }
    
    public static operator ==(BinaryCharacteristics a, BinaryCharacteristics b)
    {
        
    }
}

public enum RequiredOperatingSystem : int
{
    Unknown = -1,
    Dos = 0,
    Windows = 1,
    Os2 = 2,
    Unix = 3
}