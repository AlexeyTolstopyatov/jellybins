using jellybins.File.Headers;
using jellybins.File.Modeling.Base;
using jellybins.File.Modeling.Controls;
using jellybins.File.Modeling.Information;
using Environment = jellybins.Report.Common.PortableExecutable.Environment;

namespace jellybins.File.Modeling.Analysers;

public class AssemblerOutExecutableAnalyser : IExecutableAnalyser
{
    private AoutHeader _header;
    private readonly PortableExecutableInformation _environment = new();
    private readonly AssemblerOutExecutableInformation _information = new();
    public AssemblerOutExecutableAnalyser(AoutHeader header)
    {
        _header = header;
    }
    
    public void ProcessChars(ref FileChars chars)
    {
        chars.Cpu = _information.ProcessorFlagToString(_header.a_cpu);
        
        if ((_header.a_mag & 0407) != 0)
        {
            // Нежелательно, но я сделаю вывод
            // о том, что любой другой двоичный файл собранный в a-out
            // не имеет O-MAGIC, тогда
            // явно не будет из ранних версий ядра Linux (1.x)
            // 
            chars.Os = "Linux";
            chars.MajorVersion = 1;
            chars.MinorVersion = 2;
        }
        
        else if ((_header.a_mag & 0410) != 0)
        {
            chars.Os = "BSD Unix";
            chars.MajorVersion = 2;
            chars.MinorVersion = 0;
        }

        else
        {
            chars.Os = "Unix";
            chars.MinorVersion = 0;
            chars.MajorVersion = 0;
        }
        // Prepare char[] to ulong for CPU
        
        chars.Environment = Environment.Native;
        chars.EnvironmentString = _environment.EnvironmentFlagToString(1);
    }

    public void ProcessFlags(ref FileView view)
    {
        view.Flags = new Dictionary<string, string[]>()
        {
            {
                "Магическое число",
                new[]
                {
                    _information.MagicFlagToString(_header.a_mag),
                }
            },
            {
                "Параметры загрузчика",
                _information.MemoryFlagsToStrings()
            },
            {
                "Разрядность",
                _information.FlagsToStrings(_header.a_cpu)
            },
            {
                "Характеристики",
                _information.ApplicationFlagToString(_header.a_flags)
            }
        };
    }

    public void ProcessHeaderSections(ref FileView view)
    {
        throw new NotImplementedException();
    }

    public void ProcessHeaderSectionNames(ref FileView view)
    {
        throw new NotImplementedException();
    }
}