using jellybins.File.Headers;
using jellybins.File.Modeling.Base;
using jellybins.File.Modeling.Controls;
using jellybins.File.Modeling.Information;
using PE = jellybins.Report.Common.PortableExecutable;
using HEAD = jellybins.Report.Sections;

namespace jellybins.File.Modeling.Analysers;

public class NewExecutableAnalyser : IExecutableAnalyser
{
    private NeHeader _header;
    private ushort? _subEnvironment;
    private readonly NewExecutableInformation _information = new();
    private readonly PortableExecutableInformation _environment = new();
    public NewExecutableAnalyser(NeHeader header)
    {
        _header = header;
    }
    
    public void ProcessChars(ref FileChars chars)
    {
        chars.Cpu = _information.ProcessorFlagToString(_header.pflags);
        chars.Os = _information.OperatingSystemFlagToString(_header.os);
        chars.Type = FileType.New;
        ushort subEnv = _header.os switch
        {
            1 => 5,     // 
            3 => 102,   // DOS16 (NT-VDM  required)
            2 => 101,   // Win16 (NT-VDM? required)
            4 => 101,   // Win16 (NT-VDM? required)
            _ => 1      // No environment (or unknown)
        };
        _subEnvironment = subEnv;
        chars.Environment = (PE.Environment)subEnv;
        chars.EnvironmentString = _environment.EnvironmentFlagToString(subEnv);
        chars.MajorVersion = _header.major;
        chars.MinorVersion = _header.minor;
        chars.MinimumMajorVersion = 5;
        chars.MinimumMinorVersion = 0;
    }

    public void ProcessFlags(ref FileView view)
    {
        Dictionary<string, string[]> flags = new()
        {
            {
                "Основные",
                new []
                {
                    "16-разрядный",
                    "Требует NT-VDM"
                }
            },
            {
                "Программные флаги",
                _information.FlagsToStrings(_header.pflags)
            },
            {
                "Флаги приложения",
                _information.ApplicationFlagsToStrings(_header.aflags)
            },
            {
                "Флаги OS/2",
                _information.OtherFlagsToStrings(_header.flagsothers)
            },
            {
                "Подсистема",
                new []
                {
                    _environment.EnvironmentFlagToString(_subEnvironment!.Value)
                }
            }
        };
        view.Flags = flags;
    }

    /// <summary>
    /// Создает словарь параметров/значений заголовка
    /// </summary>
    /// <param name="view"></param>
    public void ProcessHeaderSections(ref FileView view)
    {
        Dictionary<string, string[]> neSection =
            HEAD.Information.SectionsToStrings(ref _header);
        
        view.PushSection(ref neSection);
    }

    public void ProcessHeaderSectionNames(ref FileView view)
    {
        // Нужен HEAD.Information.NamesAndSectionsToStrings();
        throw new NotImplementedException("Нет метода NamesAndSectionsToStrings");
    }
}