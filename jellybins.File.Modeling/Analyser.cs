using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using jellybins.File.Headers;
using jellybins.File.Modeling.Base;
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
    public static Analyser Instance() => new Analyser();
    
    private IExecutableAnalyser? _analyser;
    /// <summary>
    /// Требования файла для работы
    /// </summary>
    public FileChars Chars { get; private set; } =
        new (
            "Неизвестно",
            "Неизвестно",
            FileType.Other, 
            0, 
            0);

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
    public Analyser Get(string path)
    {
        ushort word = Searcher.GetUInt16(path, 0);

        // Пока что нет
        if (FileTypeInformation.DetectType(word) == FileType.Other)
            return GetUnknownSegmentedExecutable(word, path);

        if (FileTypeInformation.DetectType(word) == FileType.MarkZbykowski)
        {
            MzHeader mz = new();
            FileView view = new(new FileInfo(path));
            FileChars chars = new("", "", FileType.Other, 0, 0);
            Searcher.Fill(ref path, ref mz);
            Dictionary<string, string[]> mzSection = Section.Information.SectionsToStrings(ref mz);

            view.PushSection(ref mzSection);
            word = Searcher.GetUInt16(path, mz.e_lfanew);

            switch (FileTypeInformation.DetectType(word))
            {
                case FileType.New:
                {
                    NeHeader neHeader = new();
                    Searcher.Fill(ref path, ref neHeader, (int)mz.e_lfanew);
                    _analyser = new NewExecutableAnalyser(neHeader);
                    _analyser.ProcessChars(ref chars);
                    _analyser.ProcessFlags(ref view);
                    _analyser.ProcessHeaderSections(ref view);
                    break;
                }
                case FileType.Linear:
                {
                    LeHeader leHeader = new();
                    Searcher.Fill(ref path, ref leHeader, (int)mz.e_lfanew);
                    _analyser = new LinearExecutableAnalyser(leHeader);
                    _analyser.ProcessChars(ref chars);
                    _analyser.ProcessFlags(ref view);
                    _analyser.ProcessHeaderSections(ref view);
                    break;
                }
                case FileType.Portable:
                {
                    NtHeader32 ntHeader = new();
                    NtHeader64 ntHeader64 = new();
                    Searcher.Fill(ref path, ref ntHeader, (int)mz.e_lfanew);
                    
                    switch (ntHeader.WinNtOptional.Magic)
                    {
                        case (ushort)PE.Magic.Application64Bit:
                            Searcher.Fill(ref path, ref ntHeader64, (int)mz.e_lfanew);
                            _analyser = new PortableExecutableAnalyser(ntHeader64);
                            _analyser.ProcessChars(ref chars);
                            _analyser.ProcessFlags(ref view);
                            _analyser.ProcessHeaderSections(ref view);
                            break;
                        
                        case (ushort)PE.Magic.Application32Bit:
                            _analyser = new PortableExecutableAnalyser(ntHeader);
                            _analyser.ProcessChars(ref chars);
                            _analyser.ProcessFlags(ref view);
                            _analyser.ProcessHeaderSections(ref view);
                            break;
                    }

                    break;
                }
                default:
                    return GetMarkSegmentedExecutable(path);
            }
            return new Analyser()
            {
                Chars = chars,
                View = view
            };
        }

        return new Analyser();
    }


    private Analyser GetMarkSegmentedExecutable(string path)
    {
        MzHeader  dosHeader = new();
        FileView  dosExecView = new(new FileInfo(path));
        
        Searcher.Fill(ref path, ref dosHeader);
        // BUG: Rider понимает PushFlags как ссылку на структуру
        // Что с этим делать, я не знаю.
        // В свернутом виде PushFlags понимает как самостоятельный вызов функции.
        dosExecView.PushFlags(new Dictionary<string, string[]>() 
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
            Chars = new FileChars(
                "Microsoft DOS",
                "Intel x86",
                FileType.MarkZbykowski,
                2,
                0),
            View = dosExecView
        };
    }

    private static Analyser GetUnknownSegmentedExecutable(ushort address, string path)
    {
        FileType fType = FileTypeInformation.DetectType(address);
        return new Analyser()
        {
            Chars = new FileChars("?", "?", FileType.Other, 0, 0),
            View = new FileView(new FileInfo(path), new Dictionary<string, string[]>()
            {
                   
            }, new Dictionary<string, string[]>()
            {
                {
                    "Что известно",
                    new []
                    {
                        $"Начало файла: 0x{address:x}",
                        FileTypeInformation.GetType(fType),
                        FileTypeInformation.GetTitle(fType),
                    }
                }
            })
        };
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
    public Analyser Set(string path)
    {
        MzHeader dos = new();
        NtHeader32 winnt = new();
        
        Searcher.Fill(ref path, ref dos);
        Searcher.GetUInt16(path, dos.e_lfanew);
        Searcher.Fill(ref path, ref winnt, (int)dos.e_lfanew);

        return new Analyser()
        {
            Chars = new FileChars(
                PE.Information.OperatingSystemToString(),
                PE.Information.ProcessorFlagToString(winnt.WinNtMain.Machine),
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