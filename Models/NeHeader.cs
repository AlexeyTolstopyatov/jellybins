using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
/*
 * Jelly Bins (C) Толстопятов Алексей 2024
 *      NEHeader (New Executable) Header
 * Файл со структурой и определениями Нового Microsoft заголовка
 * если я помню правильно, это было во времена i286, приложения для OS/2, Windows
 * были 16разрядными. Внутри Windows 2.11 Windows 3.0 была среда выполнения Win16
 */
namespace jellybins.Models
{
    public enum NewRequiredOperatingSystem
    {
        Unknown = 0x0,                       // Based
        OperatingSystem2 = 0x1,              // Based
        Windows16 = 0x2,                     // Windows 286 (Win16 Environment)
        EuropeanDos = 0x3,                   // European DOS 4.x
        Windows386 = 0x4,                    // Windows 386 (Win32 Environment Include) 
        BorlandOperatingSystemServices = 0x5 // BOSS
    }
    
    public enum NewOtherLoaderFlags
    {
        LongNamesSupport = 0x0,
        Os22xProtectedMode = 0x1,
        Os22xProportionalFonts = 0x2,
        ExecHasGangloadArea = 0x3
    }
    
    public enum NewApplicationType : byte
    {
        None = 0,                   // 
        FullScreen = 0x01,          // Флаг полноэкранного режима 
        WindowsPmCompatible = 0x02, // Совместимость с OS/2 Presentation Manager
        WindowsPmUsage = 0x03,      // Использование OS/2 Presentation Manager
        LoadCodeSegment = 0x08,     // Есть ли у двоичного файла .code, который будет сначала загружен
        ImageLinkErrors = 0x20,     // В образе возникли ошибки при линковкe
        NonConforming = 0x40,       // Несоответствующая программа
        LibraryModule = 0x80,       // Модуль библиотеки или приложения
    }

    public enum NewProgramType : byte
    {
        DataGroupType = 0,
        DataNone = 0x01,
        DataSingleShared = 0x02,
        DataMultipleShared = 0x03,
        GlobalInit = 0x2,
        ProtectedMode = 0x08,
        I8086Instructions = 0x4,
        I286Instructions = 0x5,
        I386Instructions = 0x6,
        I8087Instructions = 0x7
    }
    
    [StructLayout(LayoutKind.Sequential)]
    internal struct NeHeader
    {
        public NeHeader()
        {
            
        }
        [MarshalAs(UnmanagedType.U2)] public ushort magic = 0; // "Магическое число"
        [MarshalAs(UnmanagedType.U1)] public byte ver = 0; // "Номер версии"
        [MarshalAs(UnmanagedType.U1)] public byte rev = 0; // "Номер ревизии"
        [MarshalAs(UnmanagedType.U2)] public ushort enttab = 0; // "Смещение таблицы входа"
        [MarshalAs(UnmanagedType.U2)] public ushort cbenttab = 0; // "Количество байт в таблице входа"
        [MarshalAs(UnmanagedType.U4)] public uint crc = 0; // "Контрольная сумма всего файла"
        [MarshalAs(UnmanagedType.U1)] public byte pflags = 0; // Тип программы. (архитектура, требования к загрузчику)
        [MarshalAs(UnmanagedType.U1)] public byte aflags = 0; // Тип приложения. (приложение/драйвер/библиотека)
        [MarshalAs(UnmanagedType.U2)] public ushort autodata = 0; // "Автоматический сегмент данных"
        [MarshalAs(UnmanagedType.U2)] public ushort heap = 0; // "Начальное выделение памяти для кучи"
        [MarshalAs(UnmanagedType.U2)] public ushort stack = 0; // "Начальное распределение стека"
        [MarshalAs(UnmanagedType.U4)] public uint csip = 0; // "Начальные установки CS:IP"
        [MarshalAs(UnmanagedType.U4)] public uint sssp = 0; // "Начальные установки SS:SP"
        [MarshalAs(UnmanagedType.U2)] public ushort cseg = 0; // "Количество сегментов в файле"
        [MarshalAs(UnmanagedType.U2)] public ushort cmod = 0; // "Записи в таблице модулей"
        [MarshalAs(UnmanagedType.U2)] public ushort cbnrestab = 0; // "Размер таблицы имен несуществующих сегментов"
        [MarshalAs(UnmanagedType.U2)] public ushort segtab = 0; // "Смещение таблицы сегментов"
        [MarshalAs(UnmanagedType.U2)] public ushort rsrctab = 0; // "Смещение таблицы ресурсов"
        [MarshalAs(UnmanagedType.U2)] public ushort restab = 0; // "Смещение таблицы имен несуществующих сегментов"
        [MarshalAs(UnmanagedType.U2)] public ushort modtab = 0; // "Смещение таблицы ссылок на модули"
        [MarshalAs(UnmanagedType.U2)] public ushort imptab = 0; // "Смещение таблицы импортированных имен"
        [MarshalAs(UnmanagedType.U4)] public uint nrestab = 0; // "Смещение таблицы несуществующих имен"
        [MarshalAs(UnmanagedType.U2)] public ushort cmovent = 0; // "Количество перемещаемых записей"
        [MarshalAs(UnmanagedType.U2)] public ushort align = 0; // "Сдвиг смещения сегмента"
        [MarshalAs(UnmanagedType.U2)] public ushort cres = 0; // "Сдвиг смещения сегмента"
        [MarshalAs(UnmanagedType.U1)] public byte os = 0; // "Целевая операционная система"
        [MarshalAs(UnmanagedType.U1)] public byte flagsothers = 0; // "Другие флаги .EXE"
        [MarshalAs(UnmanagedType.U2)] public ushort pretthunks = 0; // "Смещение возвратных фреймов"
        [MarshalAs(UnmanagedType.U2)] public ushort psegrefbytes = 0; // "Смещение сегментных ссылочных байтов"
        [MarshalAs(UnmanagedType.U2)] public ushort swaparea = 0; // "Минимальный размер области свопа кода"
        [MarshalAs(UnmanagedType.U1)] public byte minor = 0; // "Ожидаемая версия Windows [min.maj]"
        [MarshalAs(UnmanagedType.U1)] public byte major = 0;
    }
}
