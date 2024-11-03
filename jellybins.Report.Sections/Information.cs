using jellybins.File.Headers;
namespace jellybins.Report.Sections;

public class Information
{
    /// <summary>
    /// Переводит структуру в таблицу
    /// </summary>
    public static Dictionary<string, string[]> SectionsToStrings(ref NtHeader nt)
    {
        return new()
        {
            {
                "PE", new[] 
                {
                    $"Подпись 0x{nt.Signature:x}",
                    $"Архитектура 0x{nt.WinNtMain.Machine:x}",
                    $"Количество секций 0x{nt.WinNtMain.NumberOfSections:x}",
                    $"Количество символов 0x{nt.WinNtMain.NumberOfSymbols}",
                    $"Смещение таблицы символов 0x{nt.WinNtMain.PointerToSymbolTable:x}",
                    $"Размер доп. заголовка 0x{nt.WinNtMain.SizeOfOptionalHeader:x}",
                    $"Характеристики 0x{nt.WinNtMain.Characteristics:x}"
                }
            },
            {
                "PE32", new[]
                {
                    $"Разрядность 0х{nt.WinNtOptional.Magic:x}",
                    $"Основная версия сборщика 0x{nt.WinNtOptional.MajorLinkVersion:x}",
                    $"Дополнительная версия 0x{nt.WinNtOptional.MinorLinkVersion:x}",
                    $"Размер .code секции 0x{nt.WinNtOptional.CodeSectionSize:x}",
                    $"Размер создающихся объектов 0x{nt.WinNtOptional.InitObjectsSize:x}",
                    $"Размер уничтожающихся объектов 0x{nt.WinNtOptional.UninitObjectsSize:x}",
                    $"Смещение Точки входа 0x{nt.WinNtOptional.EntryPoint:x}",
                    $"Смещение .code секции 0x{nt.WinNtOptional.CodeSectionOffset}",
                    $"Смещение .data секции 0x{nt.WinNtOptional.DataSectionOffset:x}",
                    $"Выравнивание секций 0x{nt.WinNtOptional.SectionAlignment:x}",
                    $"Выравнивание файла 0x{nt.WinNtOptional.FileAlignment:x}",
                    $"Основная версия ОС 0x{nt.WinNtOptional.MajorOsVersion:x}",
                    $"Дополнительная версия ОС 0x{nt.WinNtOptional.MinorOsVersion:x}",
                    $"Основная версия подсистемы 0x{nt.WinNtOptional.MajorSubSystemVersion:x}",
                    $"Дополнительная версия подсистемы 0x{nt.WinNtOptional.MinorSubSystemVersion:x}",
                    $"Версия Win32 0x{nt.WinNtOptional.Win32Version:x}",
                    $"Размер образа 0x{nt.WinNtOptional.SizeofImage:x}",
                    $"Размер заголовков 0x{nt.WinNtOptional.SizeofHeaders:x}",
                    $"Контрольная сумма 0x{nt.WinNtOptional.Checksum:x}",
                    $"Характеристики 0x{nt.WinNtOptional.DllCharacteristics:x}",
                    $"Стэк 0x{nt.WinNtOptional.SizeofStackReserve:x}",
                    $"Куча {nt.WinNtOptional.SizeofHeapReserve:x}",
                    $"Размер коммита в стэк 0x{nt.WinNtOptional.SizeofStackCommit:x}",
                    $"Размер коммита в кучу 0x{nt.WinNtOptional.SizeofHeapCommit:x}",
                    $"Флаги загрузчика {nt.WinNtOptional.LoaderFlags:x}",
                    $"Количество RVA и Размеры 0x{nt.WinNtOptional.NumberOfRVAAndSizes:x}"
                }
            }
        };
    }
    /// <summary>
    /// Переводит структуру в таблицу
    /// </summary>
    public static Dictionary<string, string[]> SectionsToStrings(ref ClrHeader clr)
    {
        return new Dictionary<string, string[]>()
        {
            {
                "CLR",
                new []
                {
                    $"Размер заголовка 0x{clr.SizeofHead:x}",
                    $"Основная версия CLR {clr.MajorRuntimeVersion}",
                    $"Дополнительная версия CLR {clr.MinorRuntimeVersion}",
                    $"Смещение метаданных 0x{clr.MetaDataOffset:x}",
                    $"Флаги сборщика 0x{clr.LinkerFlags:x}",
                    $"Токен точки входа 0x{clr.EntryPointToken:x}"
                }
            }
        };
    }
    /// <summary>
    /// Переводит структуру в таблицу
    /// </summary>
    public static Dictionary<string, string[]> SectionsToStrings(ref LeHeader le)
    {
        return new()
        {
            {
                "LE",
                new[]
                {
                    $"Подпись 0x{(le.SignatureWord[0] * 0x100 + le.SignatureWord[1]):x}",
                    $"Порядок: {le.ByteOrder:x} {le.WordOrder:x}",
                    $"Формат исполняемого уровня {le.ExecutableFormatLevel:x}",
                    $"Тип Процессора 0x{le.CPUType:x}",
                    $"Тип Операционной системы 0x{le.TargetOperatingSystem:x}",
                    $"Версия модуля 0x{le.ModuleVersion:x}",
                    $"Флаги модуля 0x{le.ModuleTypeFlags:x}",
                    $"Количество Страниц памяти 0x{le.NumberOfMemoryPages:x}",
                    $"Номер CS для начального объекта 0x{le.InitialObjectCSNumber:x}",
                    $"EIP 0x{le.InitialEIP:x}",
                    $"SS 0x{le.InitialSSObjectNumber:x}",
                    $"ESP 0x{le.InitialESP:x}",
                    $"Номер SS начального объекта 0x{le.InitialSSObjectNumber:x}",
                    $"Размер страницы памяти 0x{le.MemoryPageSize:x}",
                    $"Байты в последней странице 0x{le.BytesOnLastPage:x}",
                    $"Фиксированный размер секции 0x{le.FixupSectionSize:x}",
                    $"Фиксированная рассчетная сумма секции 0x{le.FixupSectionChecksum:x}",
                    $"Размер секции загрузчика 0x{le.LoaderSectionSize:x}",
                    $"Рассчетная сумма секции загрузчика 0x{le.LoaderSectionChecksum:x}",
                    $"Смещение таблицы объектов 0x{le.ObjectTableOffset:x}",
                    $"Вводные данные таблицы объектов 0x{le.ObjectTableEntries:x}",
                    $"Смещение таблицы страниц памяти 0x{le.ObjectPageMapOffset:x}",
                    $"Смещение таблицы перечисляемых данных 0x{le.ObjectIterateDataMapOffset:x}",
                    $"Смещение таблицы ресурсов 0x{le.ResourceTableOffset:x}",
                    $"Входные данные таблицы ресурсов 0x{le.ResourceTableEntries:x}",
                    $"Таблица имен резидентов 0x{le.ResidentNamesTableOffset:x}",
                    $"Смещение входной таблицы 0x{le.EntryTableOffset:x}",
                    $"Смещение Таблицы модулей указаний 0x{le.ModuleDirectivesTableOffset:x}",
                    $"Входные данные таблицы указаний 0x{le.ModuleDirectivesTableEntries:x}",
                    $"Смещение таблицы фиксированных страниц памяти 0x{le.FixupPageTableOffset:x}",
                    $"Смещение таблицы фиксированных записей памяти 0x{le.FixupRecordTableOffset:x}",
                    $"Колличество импортированных модулей 0x{le.ImportedModulesCount:x}",
                    $"Смещение таблицы импортируемых модулей 0x{le.ImportedModulesNameTableOffset:x}",
                    $"Смещение таблицы импортируемых функций 0x{le.ImportedProcedureNameTableOffset:x}",
                    $"Смещение таблицы контрольной суммы на страницу 0x{le.PerPageChecksumTableOffset:x}",
                    $"Смещение данных страниц с начала файла 0x{le.DataPagesOffsetFromTopOfFile:x}",
                    $"Предварительное количество страниц 0x{le.PreloadPagesCount:x}",
                    $"Смещение таблицы имен Не-резидентов относительно начала файла 0x{le.NonResidentNamesTableOffsetFromTopOfFile:x}",
                    $"Рассчетная сумма таблицы не-резидентных имен 0x{le.NonResidentNamesTableChecksum:x}",
                    $"Длинна таблицы не-резидентных имен 0x{le.NonResidentNamesTableLength:x}",
                    $"Автоматический объект данных 0x{le.AutomaticDataObject:x}",
                    $"Смещение информации отладки 0x{le.DebugInformationOffset:x}",
                    $"Длинна информации отладки 0x{le.DebugInformationLength:x}",
                    $"Предварительный экземпляр количества страниц 0x{le.PreloadInstancePagesNumber:x}",
                    $"Требуемый экземпляр количества страниц 0x{le.DemandInstancePagesNumber:x}",
                    $"Размер стэка {le.StackSize:x}",
                    $"Размер кучи {le.HeapSize:x}",
                    (le.WindowsDDKVersion != 0) ? $"ID Драйвера Windows 0x{le.WindowsVXDDeviceID:x}" : "",
                    (le.WindowsDDKVersion != 0) ? $"Версия DDK 0x{le.WindowsDDKVersion:x}" : "",
                    (le.WindowsDDKVersion != 0) ? $"Смещение инфо-ресурсов 0x{le.WindowsVXDVersionInfoResourceOffset:x}" : "",
                    (le.WindowsDDKVersion != 0) ? $"Длина инфо-ресурсов 0x{le.WindowsVXDVersionInfoResourceLength:x}" : ""
                }
            }
        };
    }
    /// <summary>
    /// Переводит структуру в таблицу
    /// </summary>
    public static Dictionary<string, string[]> SectionsToStrings(ref NeHeader ne)
    {
        return new Dictionary<string, string[]>()
        {
            {
                "NE",
                new[]
                {
                    $"Магическое число 0x{ne.magic:x}",
                    $"Номер версии 0x{ne.ver:x}",
                    $"Номер ревизии 0x{ne.rev:x}",
                    $"Смещение таблицы входа 0x{ne.enttab:x}",
                    $"Количество байт в таблице входа 0x{ne.cbenttab}",
                    $"Контрольная сумма всего файла 0x{ne.crc:x}",
                    $"Флаг процессора 0x{ne.pflags:x}",
                    $"Автоматический сегмент данных 0x{ne.autodata:x}",
                    $"Начальное выделение памяти для кучи 0x{ne.heap:x}",
                    $"Начальное распределение стека 0x{ne.stack:x}",
                    $"Начальные установки CS:IP 0x{ne.csip:x}",
                    $"Начальные установки SS:SP 0x{ne.sssp:x}",
                    $"Количество сегментов в файле 0x{ne.cseg}",
                    $"Записи в таблице модулей 0x{ne.cmod:x}",
                    $"Размер таблицы имен несуществующих сегментов 0x{ne.cbnrestab:x}",
                    $"Смещение таблицы сегментов 0x{ne.segtab:x}",
                    $"Смещение таблицы ресурсов 0x{ne.rsrctab:x}",
                    $"Смещение таблицы имен несуществующих сегментов 0x{ne.restab:x}",
                    $"Смещение таблицы ссылок на модули 0x{ne.modtab:x}",
                    $"Смещение таблицы импортированных имен 0x{ne.imptab:x}",
                    $"Смещение таблицы несуществующих имен 0x{ne.nrestab:x}",
                    $"Количество перемещаемых записей 0x{ne.cmovent:x}",
                    $"Сдвиг смещения сегмента 0x{ne.align:x}",
                    $"Целевая операционная система 0x{ne.os:x}",
                    $"Другие флаги .EXE 0x{ne.flagsothers:x}",
                    $"Смещение возвратных фреймов 0x{ne.pretthunks:x}",
                    $"Смещение сегментных ссылочных байтов 0x{ne.psegrefbytes:x}",
                    $"Минимальный размер области SWAP-раздела 0x{ne.swaparea:x}",
                    $"Ожидаемая версия Операционной системы 0x{ne.minor:x}{ne.major}",
                }
            }
        };
    }
    /// <summary>
    /// Переводит структуру в таблицу
    /// </summary>
    public static Dictionary<string, string[]> SectionsToStrings(ref MzHeader mz)
    {
        return new Dictionary<string, string[]>()
        {
            {
                "MZ",
                new[]
                {
                    "Подпись 0x" + mz.e_sign.ToString("x"),
                    "Размер последнего блока 0x" + mz.e_lastb.ToString("x"),
                    "Количество блоков 0x" + mz.e_fbl.ToString("x"),
                    "Количество перемещений 0x" + mz.e_relc.ToString("x"),
                    "Смещение таблицы перемещений 0x" + mz.e_reltableoff.ToString("x"),
                    "Минимальное количество отступов 0x" + mz.e_minep.ToString("x"),
                    "Максимальное количество отсутпов 0x" + mz.e_maxep.ToString("x"),
                    "Указатель стэка (SP) 0x" + mz.sp.ToString("x"),
                    "Указатель инструкции (IP) 0x" + mz.ip.ToString("x"),
                    "Сегмент стэка (SS) 0x" + mz.ss.ToString("x"),
                    "Сегмент кода (CS) 0x" + mz.cs.ToString("x"),
                    "Указатель следующего заголовка 0x" + mz.e_lfanew.ToString("x"),
                }
            }
        };
    }
}
