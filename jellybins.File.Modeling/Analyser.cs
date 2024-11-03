using System.Reflection.Metadata;
using jellybins.File.Headers;
using jellybins.File.Modeling.Controls;
using Section = jellybins.Report.Sections;
using LE = jellybins.Report.Common.LinearExecutable;
using NE = jellybins.Report.Common.NewExecutable;
using PE = jellybins.Report.Common.PortableExecutable;
namespace jellybins.File.Modeling;
/*
 * JellyBins (C) Толстопятов Алексей А. 2024
 *      Binary Analyser
 * Здесь содержится начало операции сравнения характеристик
 * Вероятно прийдется переименовывать это, как JbAnalyser
 */
public class Analyser
{
    /// <summary>
    /// Требования файла для работы
    /// </summary>
    public FileChars Result { get; private set; } =
        new ("", FileType.Other, 0, 0);

    /// <summary>
    /// Внутренности файла, которые распознал анализатор
    /// </summary>
    public FileView? View { get; private set; }
    public static string EqualsToString (FileChars analysing, FileChars thisPc)
    {
        // MIND: Сравнение флагов анализатора
        // Я же понимаю, что GUI часть я только под Windows
        // могу осилить. Походу речи и быть не может, что ОС
        // сравниваемого файла может не быть MS Windows
        if (analysing.Os is "Microsoft Windows" && analysing.MajorVersion <= thisPc.MajorVersion)
            return "Запускается на вашем устройстве";
        
        // MIND: О Подсистемах Windows NT
        // Начиная с Windows 2000 (NT 5.0) убрали поддержку подсистемы
        // OS/2 (os2ss) для приложений собранных для 1.1 версии Поэтому наверное
        // будет лучше сразу проверить версию ОС
        //
        // Поддержку NT-VDM (NT Virtual DOS Machine) отключили чуть позже,
        // на моей памяти уже в NT 6.0 (Windows Vista) нельзя было
        // запускать приложения собранные под DOS.
        
        // MIND: Для всего ценного старья 
        // Желательно это переписать более внятно и просто...
        // Вопрос: как? Докажи что ты работаешь тех-лидом, все-таки.
        if (analysing is
            {
                Os: not "Microsoft Windows",
                Type: FileType.New or FileType.MarkZbykowski or FileType.Linear
            } && thisPc.MajorVersion <= 5)
            return "Запускается на вашем устройстве";
            
        // FIXME: Сравнение по флагу версии
        // Если приложение работает под Windows NT 5.Х,
        // Windows NT 5.0 = Windows 2000
        // Windows NT 5.1 = Windows XP
        // Windows NT 5.2 = Windows XP / 2003 Server
        
        // Если все-таки заголовок уже все-таки COFF/PE
        if (analysing is
            {
                Os: "Microsoft Windows", 
                Type: FileType.Portable
            } 
            && thisPc.MajorVersion >= analysing.MinimumMajorVersion)
            return "Запускается на вашем устройстве";
        return "Не совместимо";
    }
    
    /// <summary>
    /// Получает информацию о исследуемом файле
    /// Возвращает характеристики исследуемого файла.
    /// </summary>
    /// <param name="path">
    /// Путь в (файловой системе) исследуемого файла
    /// </param>
    /// <returns>
    /// Требования исследуемого файла для его загрузки. И
    /// Модель данных файла (распознанных анализатором)
    /// </returns>
    public static Analyser Get(string path)
    {
        ushort mzSign = Searcher.GetUInt16(path, 0);
        if (FileTypeInformation.DetectType(mzSign) == FileType.Other)
        {
            return new Analyser();
        }

        MzHeader mz = new();
        FileView view = new(new FileInfo(path));
        Searcher.Fill(ref path, ref mz);
        Dictionary<string, string[]> mzSection = Section.Information.SectionsToStrings(ref mz);
        
        ushort word = Searcher.GetUInt16(path, mz.e_lfanew);

        switch (FileTypeInformation.DetectType(word))
        {
            case FileType.Linear:
                LeHeader le = new();
                Searcher.Fill(ref path, ref le, (int)mz.e_lfanew);
                int maj = (le.WindowsDDKVersion != 0) ? le.WindowsDDKVersion >> 4 : 2;
                int min = (le.WindowsDDKVersion != 0) ? le.WindowsDDKVersion - ((le.WindowsDDKVersion >> 4) * 10) : 0;
                Dictionary<string, string[]> lineSection = Section.Information.SectionsToStrings(ref le);
                view.PushFlags(new Dictionary<string, string[]>()
                {
                    {
                        "Основные",
                        new []
                        {
                            // Информации по поводу таких тонкостей в сети вообще
                            // не найти, похоже. Из всей вычитанной IBM документации,
                            // могу предположить, что разрядность определяется по слову (WordOrder).
                            (le.WordOrder > 0) ? "32-разрядный" : "16-разрядный"
                        }
                    }
                });
                view.PushFlags(new Dictionary<string, string[]>()
                {
                    {
                        "LE",
                        LE.Information.ModuleFlagsToStrings(le.ModuleTypeFlags)
                    }
                });
                view.PushSection(ref mzSection);
                view.PushSection(ref lineSection);
                return new Analyser()
                {
                    Result = new FileChars(
                        LE.Information.OperatingSystemFlagToString(le.TargetOperatingSystem),
                        FileType.Linear,
                        maj,
                        min)
                    {
                        Environment = PE.Environment.Native,
                    },
                    View = view
                };
            case FileType.New:
                NeHeader ne = new();
                Searcher.Fill(ref path, ref ne, (int)mz.e_lfanew);
                Dictionary<string, string[]> flags = new()
                {
                    {
                        "Программные флаги",
                        NE.Information.ProgramFlagsToStrings(ne.pflags)
                    },
                    {
                        "Флаги приложения",
                        NE.Information.ApplicationFlagsToStrings(ne.aflags)
                    },
                    {
                        "Флаги OS/2",
                        NE.Information.OtherFlagsToStrings(ne.flagsothers)
                    }
                };
                Dictionary<string, string[]> neSection = Section.Information.SectionsToStrings(ref ne);
                view.Flags = flags;
                view.PushSection(ref mzSection);
                view.PushSection(ref neSection);
                return new Analyser()
                {
                    Result = new FileChars(
                        NE.Information.OperatingSystemFlagToString(ne.os),
                        FileType.New,
                        ne.major,
                        ne.minor
                        )
                    {
                        // FIXME: Флаги анализатора
                        // Это немного плохое решение: запихивать
                        // Выдуманные флаги среды и существующие в одно.
                        // Но для определения подсистемы (для чужих) приложений
                        // надо как-то сделать.
                        Environment = (NE.OperatingSystem)ne.os switch
                        {
                            NE.OperatingSystem.OperatingSystem2 => PE.Environment.Os2ConsoleInterface,
                            NE.OperatingSystem.EuropeanDos => PE.Environment.DosConsoleInterface,
                            NE.OperatingSystem.Windows16 => PE.Environment.Win16GraphicalInterface,
                            NE.OperatingSystem.Windows386 => PE.Environment.Win16GraphicalInterface,
                            _ => PE.Environment.Native
                        }
                    },
                    View = view
                };
            case FileType.Portable:
                NtHeader nt = new();
                Searcher.Fill(ref path, ref nt, (int)mz.e_lfanew);
                Dictionary<string, string[]> peFlags = new Dictionary<string, string[]>()
                {
                    {
                        "Основные",
                        new []
                        {
                            PE.Information.MagicFlagToString(nt.WinNtOptional.Magic),
                        }
                    },
                    {
                        "Характеристики",
                        PE.Information.CharacteristicsToStrings(nt.WinNtMain.Characteristics)
                    }
                };
                Dictionary<string, string[]> winntSection = Section.Information.SectionsToStrings(ref nt);
                view.PushSection(ref mzSection);
                view.PushSection(ref winntSection);
                view.PushFlags(peFlags);
                
                // TODO: Распознавание CLR заголовка
                // Смещение по реальному адресу работает неправильно
                // Смещение по виртуальному адресу работает неправильно
                // Разрешить вопрос надо.
                return new Analyser()
                {
                    Result = new FileChars(
                        PE.Information.OperatingSystemToString(),
                        FileType.Portable,
                        nt.WinNtOptional.MajorOsVersion,
                        nt.WinNtOptional.MinorOsVersion
                        ),
                    View = view
                };
            default:
                // BUG: Rider понимает PushFlags как ссылку на структуру
                // Что с этим делать, я не знаю.
                // В свернутом виде PushFlags понимает как самостоятельный вызов функции.
                view.PushSection(ref mzSection);
                view.PushFlags(new Dictionary<string, string[]>() 
                {
                    {
                        "Основные",
                        new []
                        {
                            "16-разрядный",
                            "Требуется NT-VDM"
                        }
                    }
                });
                return new Analyser()
                {
                    Result = new FileChars(
                        "Microsoft DOS",
                        FileType.MarkZbykowski,
                        2,
                        0),
                    View = view
                };
        }
    }

    /// <summary>
    /// Устанавливает файл-образец с которого будут получены
    /// сведения о системе
    /// </summary>
    /// <param name="path"></param>
    /// <returns>
    /// Требования файла к загрузке
    /// Пустую модель данных файла (потому что это образец)
    /// </returns>
    public static Analyser Set(string path)
    {
        MzHeader dos = new();
        NtHeader winnt = new();
        
        Searcher.Fill(ref path, ref dos);
        Searcher.GetUInt16(path, dos.e_lfanew);
        Searcher.Fill(ref path, ref winnt, (int)dos.e_lfanew);

        return new Analyser()
        {
            Result = new FileChars(
                PE.Information.OperatingSystemToString(),
                FileType.Portable,
                winnt.WinNtOptional.MajorSubSystemVersion,
                winnt.WinNtOptional.MinorSubSystemVersion
                )
            {
                Environment = (PE.Environment)winnt.WinNtOptional.Subsystem,
                MinimumMajorVersion = winnt.WinNtOptional.MajorSubSystemVersion,
                MinimumMinorVersion = winnt.WinNtOptional.MinorSubSystemVersion
            }
        };
    }
}