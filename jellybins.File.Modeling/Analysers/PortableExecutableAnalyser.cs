using jellybins.File.Headers;
using jellybins.File.Modeling.Base;
using jellybins.File.Modeling.Controls;
using jellybins.File.Modeling.Information;
using HEAD = jellybins.Report.Sections;
using PE = jellybins.Report.Common.PortableExecutable;

namespace jellybins.File.Modeling.Analysers;

public class PortableExecutableAnalyser : IExecutableAnalyser
{
    private readonly PortableExecutableInformation _information = new();
    private NtHeader32 _header32;
    private NtHeader64 _header64;
    
    public PortableExecutableAnalyser(NtHeader64 header64)
    {
        _header64 = header64;
    }
    
    public PortableExecutableAnalyser(NtHeader32 header32)
    {
        _header32 = header32;
    }


    public void ProcessChars(ref FileChars chars)
    {
        if (_header64.Signature == 0)
        {
            chars.Cpu = _information.ProcessorFlagToString(_header32.WinNtMain.Machine);
            chars.Os  = _information.OperatingSystemFlagToString(1);
            chars.Type = FileType.Portable;
            chars.MajorVersion = _header32.WinNtOptional.MajorOsVersion;
            chars.MinorVersion = _header32.WinNtOptional.MinorOsVersion;
            chars.MinimumMajorVersion = _header32.WinNtOptional.MajorSubSystemVersion;
            chars.MinimumMinorVersion = _header32.WinNtOptional.MinorSubSystemVersion;
            chars.Environment = (PE.Environment)_header32.WinNtOptional.Subsystem;
            chars.EnvironmentString = _information.EnvironmentFlagToString(_header32.WinNtOptional.Subsystem);
            return;
        }

        if (_header32.Signature == 0)
        {
            chars.Cpu = _information.ProcessorFlagToString(_header64.WinNtMain.Machine);
            chars.Os = _information.OperatingSystemFlagToString(1);
            chars.Type = FileType.Portable;
            chars.MajorVersion = _header64.WinNtOptional.MajorOperatingSystemVersion;
            chars.MinorVersion = _header64.WinNtOptional.MinorOperatingSystemVersion;
            chars.MinimumMajorVersion = _header64.WinNtOptional.MajorSubsystemVersion;
            chars.MinimumMinorVersion = _header64.WinNtOptional.MinorSubsystemVersion;
            chars.Environment = (PE.Environment)_header64.WinNtOptional.Subsystem;
            chars.EnvironmentString = _information.EnvironmentFlagToString(_header64.WinNtOptional.Subsystem);
        }
    }

    public void ProcessFlags(ref FileView view)
    {
        if (_header32.Signature != 0)
        {
            Dictionary<string, string[]> peFlags;
            peFlags = new Dictionary<string, string[]>()
            {
                {
                    "Основные",
                    new []
                    {
                        _information.MagicFlagToString(_header32.WinNtOptional.Magic),
                    }
                },
                {
                    "Характеристики",
                    _information.FlagsToStrings(_header32.WinNtMain.Characteristics)
                },
                {
                    "Характеристики DLL",
                    _information.DllCharacteristicsToStrings(_header32.WinNtOptional.DllCharacteristics)
                },
                {
                    "Подсистема",
                    new[]
                    {
                        _information.EnvironmentFlagToString(_header32.WinNtOptional.Subsystem)
                    }
                }
            };
            view.PushFlags(flags: peFlags);
        }
        if (_header64.Signature != 0)
        {
            Dictionary<string, string[]> peFlags;
            peFlags = new Dictionary<string, string[]>()
            {
                {
                    "Основные",
                    new []
                    {
                        _information.MagicFlagToString(_header64.WinNtOptional.Magic),
                    }
                },
                {
                    "Характеристики",
                    _information.FlagsToStrings(_header64.WinNtMain.Characteristics)
                },
                {
                    "Характеристики DLL",
                    _information.DllCharacteristicsToStrings(_header64.WinNtOptional.DllCharacteristics)
                },
                {
                    "Подсистема",
                    new[]
                    {
                        _information.EnvironmentFlagToString(_header64.WinNtOptional.Subsystem)
                    }
                }
            };
            view.PushFlags(flags: peFlags);
        }
    }

    public void ProcessHeaderSections(ref FileView view)
    {
        if (_header32.Signature != 0)
        {
            Dictionary<string, string[]> peSections = HEAD.Information.SectionsToStrings(ref _header32);
            view.PushSection(ref peSections);
        }
        else
        {
            Dictionary<string, string[]> peSections64 = HEAD.Information.SectionsToStrings(ref _header64);
            view.PushSection(ref peSections64);
        }
    }

    public void ProcessHeaderSectionNames(ref FileView view)
    {
        throw new NotImplementedException();
    }
}