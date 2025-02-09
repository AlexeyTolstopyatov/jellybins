using jellybins.Core.Headers;
using jellybins.Core.Models;
using jellybins.Core.Interfaces;
using jellybins.Core.Strings;

namespace jellybins.Core.Readers;

public class AssemblerOutExecutableReader : IReader
{
    private CommonProperties _result;
    private readonly AssemblerOutExecutableStrings _strings;
    private readonly AssemblerOutput _head;

    public CommonProperties Properties => _result;

    public AssemblerOutExecutableReader(AssemblerOutput head)
    {
        _head = head;
        _strings = new AssemblerOutExecutableStrings();
    }

    public Dictionary<string, string> GetHeader()
    {
        return new Dictionary<string, string>
        {
            {nameof(_head.a_mag), $"0x{_head.a_mag:X}"},
            {nameof(_head.a_flags), $"0x{_head.a_flags:X}"},
            {nameof(_head.a_cpu), $"0x{_head.a_cpu:X}"},
            {nameof(_head.a_hdrlen), $"0x{_head.a_hdrlen:X}"},
            {nameof(_head.a_unused), $"0x{_head.a_unused:X}" },
            {nameof(_head.a_version), $"$0x{_head.a_version}"},
            {nameof(_head.a_text), $"0x{_head.a_text:X}"},
            {nameof(_head.a_data), $"0x{_head.a_data:X}"},
            {nameof(_head.a_bss), $"0x{_head.a_bss:X}"},
            {nameof(_head.a_syms), $"0x{_head.a_syms:X}"},
            {nameof(_head.a_trsize), $"0x{_head.a_trsize:X}"},
            {nameof(_head.a_drsize), $"0x{_head.a_drsize:X}"}
        };
    }

    public Dictionary<string, string[]> GetFlags()
    {
        List<string> flags = new()
        {
            ((_head.a_flags & 0x10) != 0) ? "ObjectCodeImage" : "",
            ((_head.a_flags & 0x20) != 0) ? "DynamicImage" : "",
        };
        flags = flags
            .Where(x => !string.IsNullOrEmpty(x))
            .Distinct()
            .ToList();

        var iterates = 
            from item in flags
            select item;

        return new Dictionary<string, string[]>(){{"_flags", iterates.ToArray()}};
    }

    public CommonProperties GetProperties()
    {
        _result.ImageType = _strings.ImageTypeFlagToString(_head.a_flags);
        _result.Subsystem = _strings.SubsystemFromOperatingSystemFlagToString(_head.a_hdrlen);
        _result.CpuArchitecture = _strings.CpuArchitectureFlagToString(_head.a_cpu);
        _result.CpuWordLength = _strings.CpuWordLengthFlagToString(_head.a_cpu);
        _result.OperatingSystemVersion = _strings.OperatingSystemVersionToString(1, 2);
        _result.OperatingSystem = _strings.OperatingSystemFlagToString(0);
        _result.LinkerVersion = _strings.ImageVersionFlagsToString(_head.a_version, 0);
        return _result;
    }

    public void GetImports()
    {
        throw new NotImplementedException();
    }
}