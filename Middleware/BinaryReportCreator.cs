using System.Windows;
using System.Windows.Automation;
using System.Windows.Media;
using jellybins.Binary;
using jellybins.Models;
using jellybins.Views;
using Wpf.Ui.Controls;
using ListView = Wpf.Ui.Controls.ListView;
using TextBlock = Wpf.Ui.Controls.TextBlock;

/*
 * Jelly Bins (C) Толстопятов Алексей 2024
 *      Binary Report Creator
 * Взаимодействует с элементами окна, создает подробный отчет
 * о анализе заголовка двоичного файла
 * Члены класса (Можно описать одной процедурой)
 *              TryParseXXHeader(ref BinaryHeaderPage, ref XxHeader): Создает элементы страницы
 */
namespace jellybins.Middleware
{
    /// <summary>
    /// Здесь содержится весь хлам по созданию страниц
    /// </summary>
    internal static class BinaryReportCreator
    {
        public static bool TryParseNetComponentMethods(
            string assembly, 
            ref BinaryProceduresPage netp)
        {
            netp.libmethods.Items.Clear();
            NetComponentInternals netl = new NetComponentInternals(assembly);
            
            if (!netl.Loaded)
            {
                ExceptionsController.ShowException(
                    new InvalidOperationException("Двоичный файл не был загружен")
                );

                return netl.Loaded; // пока что будет так.
            }

            netp.libadd.Text = netl.Description;
            netp.libname.Text = netl.Name;
            netp.libtype.Text = netl.Title;

            foreach (string item in netl.Types)
            {
                netp.libmethods.Items.Add(new TextBlock()
                {
                    TextWrapping = TextWrapping.Wrap,
                    Foreground = Brushes.Cyan,
                    FontFamily = new FontFamily("Consolas"),
                    Text = item
                });
            }

            foreach (string item in netl.Methods)
            {
                netp.libmethods.Items.Add(new TextBlock()
                {
                    TextWrapping = TextWrapping.Wrap,
                    Foreground = Brushes.Violet,
                    FontFamily = new FontFamily("Consolas"),
                    FontSize = 16,
                    Text = item
                });
            }

            return true;
        }

        public static void TryParseUnknown(
            ref BinaryHeaderPage bin,
            ushort head)
        {
            bin.binprops.Text = $"Начало файла: 0x{head:x}";
            bin.bintype.Text = BinaryInformation.GetInformation(BinaryInformationPrinter.Other);
            bin.ThisArchLabel.Text =
                bin.ThisOsVersionLabel.Text =
                    bin.ThisOsLabel.Text =
                        bin.OsVerLabel.Text =
                            bin.OsRequiredLabel.Text =
                                bin.ArchRequiredLabel.Text = "Не рассчитано";
            
        }
        public static void TryParsePortableHeader(
            ref BinaryHeaderPage bin,
            ref NtHeader nt)
        {
            bin.binprops.Text = BinaryInformation.GetInformation(BinaryInformationPrinter.Portable);
            bin.bintype.Text = BinaryInformation.GetType(BinaryInformationPrinter.Portable);
            bin.bintable.Children.Add(new CardExpander()
            {
                IsExpanded = true,
                Header = "PE",
                Content = new ListView()
                {
                    Items =
                    {
                        $"Подпись 0x{nt.Signature:x}",
                        $"Архитектура 0x{nt.WinNtMain.Machine:x}",
                        $"Количество секций 0x{nt.WinNtMain.NumberOfSections:x}",
                        $"Количество символов 0x{nt.WinNtMain.NumberOfSymbols}",
                        $"Смещение таблицы символов 0x{nt.WinNtMain.PointerToSymbolTable:x}",
                        $"Размер доп. заголовка 0x{nt.WinNtMain.SizeOfOptionalHeader:x}",
                        $"Характеристики 0x{nt.WinNtMain.Characteristics:x}"
                    },
                    MaxHeight = 300,
                    MaxWidth = 300,
                },
                MinWidth = 300,
                MaxWidth = 300,
                VerticalAlignment = VerticalAlignment.Top
            });
            bin.bintable.Children.Add(new CardExpander()
            {
                Header = "PE32",
                IsExpanded = true,
                Content = new ListView()
                {
                    Items =
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
                    },
                    MaxWidth = 300,
                    MaxHeight = 300,
                },
                MinWidth = 300,
                MaxWidth = 300,
                VerticalAlignment = VerticalAlignment.Top
            });
        }

        public static void TryParseNeHeader(
            ref BinaryHeaderPage bin,
            ref NeHeader ne)
        {
            bin.binprops.Text = BinaryInformation.GetInformation(BinaryInformationPrinter.New);
            bin.bintype.Text = BinaryInformation.GetType(BinaryInformationPrinter.New);
            bin.bintable.Children.Add(
                new CardExpander()
                {
                    Header = "NE",
                    IsExpanded = true,
                    Content = new ListView()
                    {
                        Items =
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
                            $"Ожидаемая версия Операционной системы 0x{ne.minor:x}{ne.major}"
                        },
                        MaxHeight = 300,
                        MinWidth = 300,
                    },
                    MinWidth = 300,
                    MaxWidth = 300,
                    VerticalAlignment = VerticalAlignment.Top
                });
            
            bin.OsRequiredLabel.Text = NeInformationBlock.OperatingSystemFlagToString(ne.os);
            bin.OsVerLabel.Text = NeInformationBlock.WindowsVersionToString(ne.major, ne.minor);
            bin.ArchRequiredLabel.Text = NeInformationBlock.ProcessorFlagToString(ne.pflags);

            if (NeInformationBlock.OperatingSystemFlagToString(ne.os) == String.Empty &
                NeInformationBlock.WindowsVersionToString(ne.major, ne.minor) != String.Empty)
                bin.OsRequiredLabel.Text = NeInformationBlock.WindowsVersionToString(ne.major, ne.minor);
            
            // Loader Flags Enumeration
            foreach (var programFlag in NeInformationBlock.ProgramFlagsToStrings(ne.pflags))
            {
                if (programFlag != string.Empty)
                    bin.LoaderFlags.Items.Add(new TextBlock()
                    {
                        Foreground = Brushes.Yellow,
                        Text = programFlag
                    });
            }

            foreach (var appFlag in NeInformationBlock.ApplicationFlagsToStrings(ne.aflags))
            {
                if (appFlag != String.Empty)
                    bin.LoaderFlags.Items.Add(new TextBlock()
                    {
                        Foreground = Brushes.SpringGreen,
                        Text = appFlag
                    });
            }

            foreach (var oFlag in NeInformationBlock.OtherFlagsToStrings(ne.flagsothers))
            {
                if (oFlag != string.Empty)
                    bin.LoaderFlags.Items.Add(new TextBlock()
                    {
                        Foreground = Brushes.CornflowerBlue,
                        Text = oFlag
                    });
            }
        }
        
        public static void TryParseLeHeader(
            ref BinaryHeaderPage binp,
            ref LeHeader le)
        {
            binp.binprops.Text = BinaryInformation.GetInformation(BinaryInformationPrinter.Linear);
            binp.bintype.Text = BinaryInformation.GetType(BinaryInformationPrinter.Linear);
            binp.bintable.Children.Add(
                new CardExpander()
                {
                    IsExpanded = true,
                    Header = "LE",
                    Content = new ListView()
                    {
                        Items =
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
                            $"Размер кучи {le.HeapSize:x}"
                        },
                        MaxHeight = 300,
                        MinWidth = 300,
                    },
                    MinWidth = 300,
                    MaxWidth = 300,
                    VerticalAlignment = VerticalAlignment.Top
                });

            binp.OsRequiredLabel.Text = LeInformationBlock.OsFlagToString(le.TargetOperatingSystem);
            binp.ArchRequiredLabel.Text = LeInformationBlock.ArchFlagToString(le.CPUType);

            foreach (var mtFlag in LeInformationBlock.ModuleFlagsToStrings(le.ModuleTypeFlags))
            {
                if (mtFlag != string.Empty)
                    binp.LoaderFlags.Items.Add(
                        new TextBlock()
                        {
                            Foreground = Brushes.Cyan,
                            Text = mtFlag
                        });
            }
            
            if (le.WindowsVXDDeviceID == 0) return;
            
            binp.LoaderFlags.Items.Add(new TextBlock()
            {
                Text = "Драйвер устройства (VxD)",
                Foreground = Brushes.CornflowerBlue,
            });
            TryParseVxdHeader(ref binp, ref le);
        }
        private static void TryParseVxdHeader(
            ref BinaryHeaderPage bin,
            ref LeHeader vdd
        )
        {
            bin.bintable.Children.Add(
                new CardExpander()
                {
                    IsExpanded = true,
                    Header = "VxD",
                    Content = new ListView()
                    {
                        Items =
                        {
                            $"ID Драйвера Windows 0x{vdd.WindowsVXDDeviceID:x}",
                            $"Версия DDK 0x{vdd.WindowsDDKVersion:x}",
                            $"Смещение инфо-ресурсов 0x{vdd.WindowsVXDVersionInfoResourceOffset:x}",
                            $"Длина инфо-ресурсов 0x{vdd.WindowsVXDVersionInfoResourceLength:x}"
                        },
                        MaxHeight = 300,
                        MinWidth = 300,
                    },
                    MinWidth = 300,
                    MaxWidth = 300,
                    VerticalAlignment = VerticalAlignment.Top
                });
        }
        
        
        public static void TryParseMzHeader(
            ref BinaryHeaderPage binp, 
            ref MzHeader mz)
        {
            binp.bintable.Children.Add(new CardExpander()
            {
                IsExpanded = true,
                Header = "MZ",
                Content = new ListView()
                {
                    Items = 
                    {
                        "Подпись 0x" + mz.e_sign.ToString("x"),
                        "Размер последнего блока 0x"+ mz.e_lastb.ToString("x"),
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
                    },
                    MaxHeight = 300,
                    MinWidth = 300,
                },
                MinWidth = 300,
                MaxWidth = 300,
                VerticalAlignment = VerticalAlignment.Top
            });
            //(string, string, string) machine = DeterminantEngine.GetThisMachineFlags();
            binp.bintype.Text = BinaryInformation.GetType(BinaryInformationPrinter.MarkZbykowski);
            binp.binprops.Text = BinaryInformation.GetInformation(BinaryInformationPrinter.MarkZbykowski);
            binp.OsRequiredLabel.Text = "Microsoft DOS";
            binp.OsVerLabel.Text = "2.0";
            binp.ArchRequiredLabel.Text = "Intel x86 (i286 IA32)";
            // binp.ThisArchLabel.Text = machine.Item2;
            // binp.ThisOsVersionLabel.Text = machine.Item3;
            // binp.ThisOsLabel.Text = machine.Item1;
        }
    }
}
