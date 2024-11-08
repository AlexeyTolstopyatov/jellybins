using System.Runtime.InteropServices;
using jellybins.File.Headers;
using jellybins.File.Modeling.Base;
using jellybins.File.Modeling.Controls;
using Environment = jellybins.Report.Common.PortableExecutable.Environment;
using LE = jellybins.Report.Common.LinearExecutable;
using PE = jellybins.Report.Common.PortableExecutable;
using HEAD = jellybins.Report.Sections;

namespace jellybins.File.Modeling;

public class LinearExecutableAnalyser : IExecutableAnalyser
{
    private LeHeader _header;
    
    public LinearExecutableAnalyser(LeHeader header)
    {
        _header = header;
    }


    public void ProcessChars(ref FileChars chars)
    {
        chars.Cpu = LE.Information.ProcessorFlagToString(_header.CPUType);
        chars.Os = LE.Information.OperatingSystemFlagToString(_header.TargetOperatingSystem);
        chars.Environment = Environment.Native;
        chars.Type = FileType.Linear;
        chars.EnvironmentString = PE.Information.EnvironmentFlagToString(1);
        chars.MinimumMajorVersion = 4; 
        chars.MinimumMinorVersion = 0;
        chars.MajorVersion = (_header.WindowsDDKVersion != 0) ? _header.WindowsDDKVersion >> 4 : 2;
        chars.MinorVersion = (_header.WindowsDDKVersion != 0) ? (_header.WindowsDDKVersion - chars.MajorVersion) : 0;
    }

    public void ProcessFlags(ref FileView view)
    {
        view.PushFlags(new Dictionary<string, string[]>()
        {
            {
                "Основные",
                new []
                {
                    // Информации по поводу таких тонкостей в сети вообще
                    // не найти, похоже. Из всей вычитанной IBM документации,
                    // могу предположить, что разрядность определяется по слову (WordOrder).
                    (_header.WordOrder > 0) ? "32-разрядный" : "16-разрядный"
                }
            },
            {
                "Флаги модуля",
                LE.Information.ModuleFlagsToStrings(_header.ModuleTypeFlags)
            },
            {
                "Подсистема",
                new []
                {
                    "Нет подсистемы"
                }
            }

        });
    }

    public void ProcessHeaderSections(ref FileView view)
    {
        Dictionary<string, string[]> leSection = HEAD.Information.SectionsToStrings(ref _header);
        view.PushSection(ref leSection);
    }

    public void ProcessHeaderSectionNames(ref FileView view)
    {
        throw new NotImplementedException();
    }
}