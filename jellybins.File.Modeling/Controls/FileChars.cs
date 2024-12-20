﻿using jellybins.Report.Common.PortableExecutable;
using Environment = jellybins.Report.Common.PortableExecutable.Environment;

namespace jellybins.File.Modeling.Controls;

/**
 * Jelly Bins (C) Толстопятов Алексей 2024
 *      File Characteristics
 * Заполняет поля
 * (подробнее)
 * Description.FileChars.md
 */
public class FileChars
{
    public FileType Type { get; set; }
    public int MajorVersion { get; set; }
    public int MinorVersion { get; set; }
    public int? MinimumMajorVersion { get; set; }
    public int? MinimumMinorVersion { get; set; }
    public string Os { get; set; }
    public string Cpu { get; set; }
    public Environment Environment { get; set; }
    public string? EnvironmentString { get; set; }
    public FileChars(
        string os,
        string cpu,
        FileType type,
        int major,
        int minor)
    {
        MajorVersion = major;
        MinorVersion = minor;
        Type = type;
        Os = os;
        Cpu = cpu;
    }
}