using System.Runtime.InteropServices;

using jellybins.File.Headers;
using jellybins.File.Modeling.Base;
using jellybins.File.Modeling.Controls;
using jellybins.File.Modeling.Information;
using Environment = jellybins.Report.Common.PortableExecutable.Environment;
using HEAD = jellybins.Report.Sections;

namespace jellybins.File.Modeling.Analysers;

public class LinearExecutableAnalyser : IExecutableAnalyser
{
    private LeHeader _header;
    private readonly LinearExecutableInformation _information;
    public LinearExecutableAnalyser(LeHeader header)
    {
        _header = header;
        _information = new LinearExecutableInformation();
    }


    /// <summary>
    /// Заполняет структуру характеристик для
    /// исследуемого исполняемого файла
    /// </summary>
    /// <param name="chars">Модель характеристик</param>
    public void ProcessChars(ref FileChars chars)
    {
        chars.Cpu = _information.ProcessorFlagToString(_header.CPUType);
        chars.Os = _information.OperatingSystemFlagToString(_header.TargetOperatingSystem);
        chars.Environment = Environment.Native;
        chars.Type = FileType.Linear;
        chars.EnvironmentString = new PortableExecutableInformation()
            .EnvironmentFlagToString(1);
        
        // Линейные исполняемые файла (если это не модель виртуального драйвера)
        // в Microsoft Windows не используются... Что же делать.
        // Модель виртуальных драйверов в Windows существовала до NT 5.0
        // (Новая модель драйверов - WDM)
        chars.MinimumMajorVersion = 4; 
        chars.MinimumMinorVersion = 0;
        
        // FIXME: Версия ОС рассчитана не верно
        chars.MajorVersion = (_header.WindowsDDKVersion != 0) ? _header.WindowsDDKVersion >> 4 : 2;
        chars.MinorVersion = (_header.WindowsDDKVersion != 0) ? (_header.WindowsDDKVersion - chars.MajorVersion) : 0;
    }

    /// <summary>
    /// Заполняет флаги процессора, ОС, архитектуры
    /// и требования к запуску исследуемого файла
    /// </summary>
    /// <param name="view">Модель данных</param>
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
                _information.FlagsToStrings(_header.ModuleTypeFlags)
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