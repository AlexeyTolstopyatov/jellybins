using jellybins.File.Headers;
using jellybins.File.Modeling.Base;
using jellybins.File.Modeling.Controls;
using NE = jellybins.Report.Common.NewExecutable;
using PE = jellybins.Report.Common.PortableExecutable;
using HEAD = jellybins.Report.Sections;

namespace jellybins.File.Modeling;

public class NewExecutableAnalyser : IExecutableAnalyser
{
    private NeHeader _header;
    private ushort? _subEnvironment;
    
    public NewExecutableAnalyser(NeHeader header)
    {
        _header = header;
    }
    
    public void ProcessChars(ref FileChars chars)
    {
        chars.Cpu = NE.Information.ProcessorFlagToString(_header.pflags);
        chars.Os = NE.Information.OperatingSystemFlagToString(_header.os);
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
        chars.EnvironmentString = PE.Information.EnvironmentFlagToString(subEnv);
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
                NE.Information.ProgramFlagsToStrings(_header.pflags)
            },
            {
                "Флаги приложения",
                NE.Information.ApplicationFlagsToStrings(_header.aflags)
            },
            {
                "Флаги OS/2",
                NE.Information.OtherFlagsToStrings(_header.flagsothers)
            },
            {
                "Подсистема",
                new []
                {
                    PE.Information.EnvironmentFlagToString(_subEnvironment!.Value)
                }
            }
        };
        view.Flags = flags;
    }

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