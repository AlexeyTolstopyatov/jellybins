using jellybins.File.Headers;
using jellybins.File.Modeling.Analysers;
using jellybins.File.Modeling.Base;
using jellybins.File.Modeling.Controls;
using jellybins.File.Modeling.Information;
using Section = jellybins.Report.Sections;
using PE = jellybins.Report.Common.PortableExecutable;

namespace jellybins.File.Modeling;
/*
 * JellyBins (C) Толстопятов Алексей А. 2024
 *      Binary ExecutableAnalyser
 * Здесь содержится начало операции сравнения характеристик
 * Вероятно прийдется переименовывать это, как JbAnalyser
 */
public class ExecutableAnalyser
{
    public static ExecutableAnalyser Instance() => new ExecutableAnalyser();
    
    /// <summary>
    /// API анализатора
    /// </summary>
    private IExecutableAnalyser? _analyser;
    
    /// <summary>
    /// API Службы чтения файла
    /// </summary>
    private IExecutableReader? _reader;
    
    /// <summary>
    /// Требования файла для работы
    /// (распознаются здесь и в анализаторе)
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
    
    /// <summary>
    /// Сравнивает файлы
    /// </summary>
    /// <param name="analysing">Исследуемый файл</param>
    /// <param name="thisPc">Файл-образец</param>
    /// <returns>Строку (прогноз) запуска</returns>
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

        switch (analysing)
        {
            // MIND: Для всего ценного старья 
            // Желательно это переписать более внятно и просто...
            // Вопрос: как? Докажи что ты работаешь тех-лидом, все-таки.
            case { 
                Type: 
                FileType.New or 
                FileType.MarkZbykowski or 
                FileType.Linear
            } when thisPc.MajorVersion <= 5:
            // FIXME: Сравнение по флагу версии
            // Если приложение работает под Windows NT 5.Х,
            // Windows NT 5.0 = Windows 2000
            // Windows NT 5.1 = Windows XP
            // Windows NT 5.2 = Windows XP / 2003 Server
            // Если все-таки заголовок уже все-таки COFF/PE
            case
            {
                Os: "Microsoft Windows", 
                Type: FileType.Portable
            } 
            when thisPc.MajorVersion >= analysing.MinimumMajorVersion:
                return "Запускается на вашем устройстве";
            default:
                return "Не совместимо";
        }
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
    public ExecutableAnalyser Get(string path)
    {
        _reader = new ExecutableReader(path);
        
        ushort word = _reader.GetUInt16(0);
        
        // Если IBM / Microsoft ~ moment ~
        if (FileTypeInformation.DetectType(word) == FileType.MarkZbykowski) 
            return GetFormatOsSegmentedExecutable(path);

        // Если двоичный файл из ОС-неформала (старые Unix-подобные ОС/РТОС)
        if (FileTypeInformation.DetectType(word) == FileType.AOut) // false
            return GetAssemblerOutputExecutable(path);

        return GetUnknownSegmentedExecutable(word, path);
    }
    
    #region Microsoft / IBM
    private ExecutableAnalyser GetFormatOsSegmentedExecutable(string path)
    {
        _reader = new ExecutableReader(path);
        
        MzHeader mz = new();
        FileView view = new(new FileInfo(path));
        FileChars chars = new("", "", FileType.Other, 0, 0);
        
        _reader.Fill(ref mz);
        Dictionary<string, string[]> mzSection = Section.Information.SectionsToStrings(ref mz);

        view.PushSection(ref mzSection);
        ushort word = _reader.GetUInt16((int)mz.e_lfanew);

        switch (FileTypeInformation.DetectType(word))
        {
            case FileType.New:
            {
                NeHeader neHeader = new();
                _reader.Fill(ref neHeader, (int)mz.e_lfanew);
                _analyser = new NewExecutableAnalyser(neHeader);
                _analyser.ProcessChars(ref chars);
                _analyser.ProcessFlags(ref view);
                _analyser.ProcessHeaderSections(ref view);
                break;
            }
            case FileType.Linear:
            {
                LeHeader leHeader = new();
                _reader.Fill(ref leHeader, (int)mz.e_lfanew);
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
                _reader.Fill(ref ntHeader, (int)mz.e_lfanew);
                    
                switch (ntHeader.WinNtOptional.Magic)
                {
                    case (ushort)PE.Magic.Application64Bit:
                        _reader.Fill(ref ntHeader64, (int)mz.e_lfanew);
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
        return new ExecutableAnalyser()
        {
            Chars = chars,
            View = view
        };

    }

    private ExecutableAnalyser GetMarkSegmentedExecutable(string path)
    {
        MzHeader  dosHeader = new();
        FileView  dosExecView = new(new FileInfo(path));
        
        _reader = new ExecutableReader(path);
        _reader.Fill(ref dosHeader);
        
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
        return new ExecutableAnalyser()
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
    #endregion
    
    #region Unix
    /// <summary>
    /// Возвращает всю информацию о исполняемом двоичном файле, если это A-OUT
    /// Сегментный двоичный файл
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    private ExecutableAnalyser GetAssemblerOutputExecutable(string path)
    {
        AoutHeader header = new();
        FileChars chars = new(path, "", FileType.AOut, 0, 0);
        FileView view = new(new FileInfo(path));
        
        _reader = new ExecutableReader(path);
        _reader.Fill(ref header);
        _analyser = new AssemblerOutExecutableAnalyser(header);
        
        _analyser.ProcessChars(ref chars);
        _analyser.ProcessFlags(ref view);
        
        // TODO: ProcessSections or ProcessSectionNames
        // need to implement too

        Chars = chars;
        View = view;
        return new ExecutableAnalyser()
        {
            Chars = chars,
            View = view
        };
    }
    
    #endregion
    
    /// <summary>
    /// Если ни один из вариантов начала файла не подходит под условие
    /// Вызывается этот метод, как заглушка, чтобы заполнить страницу необходимыми
    /// данными для дальнейшего анализа и поиска информации.
    /// </summary>
    /// <param name="address"></param>
    /// <param name="path"></param>
    /// <returns>
    /// Полузаполненный объект, который содержит начало файла (по основанию 16)
    /// Тип файла внутри jellybins (FileType) и описание к неизвестному типу файла.
    /// </returns>
    private static ExecutableAnalyser GetUnknownSegmentedExecutable(ushort address, string path)
    {
        FileType fType = FileTypeInformation.DetectType(address);
        return new ExecutableAnalyser()
        {
            Chars = new FileChars("Неизвестно", "Неизвестно", FileType.Other, 0, 0),
            View = new FileView(
                new FileInfo(path), 
                new Dictionary<string, string[]>(), 
                new Dictionary<string, string[]>
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

    private ExecutableAnalyser GetExecutableLinkableSegmentedExecutable(string path)
    {
        return new();
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
    public ExecutableAnalyser Set(string path)
    {
        MzHeader dos = new();
        NtHeader32 winnt = new();
        PortableExecutableInformation information = new();
        
        _reader = new ExecutableReader(path);
        _reader.Fill(ref dos);
        _reader.GetUInt32((int)dos.e_lfanew);
        _reader.Fill(ref winnt, (int)dos.e_lfanew);
        
        return new ExecutableAnalyser()
        {
            Chars = new FileChars(
                information.OperatingSystemFlagToString(1),
                information.ProcessorFlagToString(winnt.WinNtMain.Machine),
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