﻿/*
 * JellyBins (C) Толстопятов Алексей А. 2024
 *      File Type
 * Здесь содержатся определения для файлов
 * (Страницы общих свойств).
 * 
 */
namespace jellybins.File.Modeling;

public enum FileType
{
    MarkZbykowski,
    NetObject,          // Managed .NET Assembly
    Portable,           // Windows NT PE32/+
    Linear,             // MS-DOS/Windows/OS-2 LE/LX
    New,                // MS-DOS/Windows/OS-2 NE
    AOut,               // a-out Linux/Solaris executable
    ExecutableLinkable, // ELF32/64
    MachObject,         // Binary inside .app/.kext/.framework
    Java,               // Jar files
    Other               // Other binary file (unknown)
}

public static class FileTypeInformation
{
    public static string GetTitle(FileType type)
    {
        string title = type switch
        {
            FileType.MarkZbykowski => "MZ",
            FileType.NetObject => ".NET COM",
            FileType.MachObject => "MachO",
            FileType.Java => "JAR",
            FileType.ExecutableLinkable => "ELF",
            FileType.AOut => "a-out",
            FileType.New => "NE",
            FileType.Portable => "PE",
            FileType.Linear => "LE",
            FileType.Other => "Другой двоичный файл",
            _ => "Неизвестный двоичный файл"
        };

        return title;
    }

    public static string GetType(FileType type) => type.ToString();
    public static string GetInformation(FileType type) 
    {
        string info = type switch
        {
            FileType.New => "New Executable (NE) — «Новый исполняемый» — формат EXE-файлов, используемый в 16-битных операционных системах, таких, как Windows (до версий 3.x включительно), OS/2 1.x и MS-DOS 4.0.\n\nПри запуске из Windows NT или OS/2 2.x NE-программы запускаются под Virtual DOS Machine Начиная с Windows NT 6.0 (Vista) оболочка Windows не поддерживает извлечение ресурсов из New Executable",
            FileType.Linear => "Linear Executable (LE) — смешанный 16/32-битный формат исполняемого файла, появившийся в OS/2 2.0. Может быть идентифицирован по сигнатуре «LE» (ASCII). Не используется в настоящее время в OS/2, ранее использовался для драйверов VxD под Windows 3.x и Windows 9x.\n\nДля открытия файла LE необходимо использовать подходящее программное обеспечение, например DOS/4GW PM 32X DOS Extender.",
            FileType.NetObject => ".NET Компонент — это динамическая библиотека или исполняемый файл, которые используют платофрму .NET и IL для работы.",
            FileType.Portable => "Portable Executable (PE) — «переносимый исполняемый» — формат исполняемых файлов, объектного кода и динамических библиотек (DLL), используемый в 32- и 64-разрядных версиях операционной системы Microsoft Windows",
            FileType.MarkZbykowski => "MZ — стандартный формат 16-битных исполняемых файлов с расширением .EXE для DOS.\n\nНазван так по сигнатуре — ASCII-символам MZ (4D 5A) в первых двух байтах. Эта сигнатура — инициалы Марка Збиковски, одного из создателей MS-DOS.\n\nФормат был разработан как замена устаревшему формату .COM.",
            FileType.AOut => "a.out — это формат файла, используемый в старых версиях Unix-подобных операционных систем для исполняемых файлов, объектного кода и, в более поздних системах, разделяемых библиотек. Это сокращенная форма \"Assembler Output\", ",
            FileType.ExecutableLinkable => "ELF — формат исполняемых двоичных файлов, используемый во многих современных UNIX-подобных операционных системах, таких как FreeBSD, Linux, Solaris.",
            _ => "Информация отсутствует."
        };
        return info;
    }

    /// <summary>
    /// Возвращает тип двоичного файла, на основе начала структуры
    /// </summary>
    /// <param name="word">Cловo (8/16/32/64 битного)</param>
    /// <returns></returns>
    public static FileType DetectType(uint word)
    {
        return word switch
        {
            // Microsoft/IBM
            0x4d5a or 0x5a4d => FileType.MarkZbykowski,
            // LE / LX
            0x4c58 or 0x584c or 0x4c45 or 0x454c => FileType.Linear,
            // NE
            0x454e or 0x4e45 => FileType.New,
            // PE/COFF
            0x4550 or 0x5045 => FileType.Portable,
                
            // Неформалы (Unix)
            // Assembler Output format
            0x103 or 0x301 or 
                0x107 or 0x701 or
                0x109 or 0x901 or 
                0xcc or 0x10b or 0xb01 or
                0x108 or 0x801 => FileType.AOut,
                
            // Unix
            // ELF
            0x464c457f => FileType.ExecutableLinkable,
            
            _ => FileType.Other
        };
    }
}