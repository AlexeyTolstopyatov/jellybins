using jellybins.Core.Attributes;
using jellybins.Core.Headers;
using jellybins.Core.Models;
using jellybins.Core.Interfaces;
using jellybins.Core.Strings;

namespace jellybins.Core.Readers;

public class NewExecutableReader : IReader
{
    private readonly NewExecutable _head;
    private readonly NewExecutableStrings _strings;
    private CommonProperties _result;

    public CommonProperties Properties => _result;

    public NewExecutableReader(NewExecutable head)
    {
        _head = head;
        _strings = new NewExecutableStrings();
    }

    public Dictionary<string, string> GetHeader()
    {
        return new()
        {
            { nameof(_head.magic), $"0x{_head.magic:X}" },
            { nameof(_head.ver), $"0x{_head.ver:X}" },
            { nameof(_head.rev), $"0x{_head.rev:X}" },
            { nameof(_head.enttab), $"0x{_head.enttab:X}" },
            { nameof(_head.cbenttab), $"0x{_head.cbenttab:X}" },
            { nameof(_head.crc), $"0x{_head.crc:X}" },
            { nameof(_head.pflags), $"0x{_head.pflags:X}" },
            { nameof(_head.aflags), $"0x{_head.aflags:X}" },
            { nameof(_head.autodata), $"0x{_head.autodata:X}" },
            { nameof(_head.heap), $"0x{_head.heap:X}" },
            { nameof(_head.stack), $"0x{_head.stack:X}" },
            { nameof(_head.csip), $"0x{_head.csip:X}" },
            { nameof(_head.sssp), $"0x{_head.sssp:X}" },
            { nameof(_head.cseg), $"0x{_head.cseg:X}" },
            { nameof(_head.cmod), $"0x{_head.cmod:X}" },
            { nameof(_head.rsrctab), $"0x{_head.rsrctab:X}" },
            { nameof(_head.restab), $"0x{_head.restab:X}" },
            { nameof(_head.modtab), $"0x{_head.modtab:X}" },
            { nameof(_head.imptab), $"0x{_head.imptab:X}" },
            { nameof(_head.nrestab), $"0x{_head.nrestab:X}" },
            { nameof(_head.cmovent), $"0x{_head.cmovent:X}" },
            { nameof(_head.align), $"0x{_head.align:X}" },
            { nameof(_head.cres), $"0x{_head.cres:X}" },
            { nameof(_head.os), $"0x{_head.os:X}" },
            { nameof(_head.flagsothers), $"0x{_head.flagsothers:X}" },
            { nameof(_head.pretthunks), $"0x{_head.pretthunks:X}" },
            { nameof(_head.swaparea), $"0x{_head.swaparea:X}" },
            { nameof(_head.minor), $"0x{_head.minor:X}" },
            { nameof(_head.major), $"0x{_head.major:X}" }
        };
    }
    
    private static readonly Dictionary<ushort, string> ProgramFlags = new()
    {
        { 0x01, "Single-task application" },
        { 0x02, "Multi-task application" },
        { 0x04, "Global initialization" },
        { 0x08, "Protected mode only" },
        { 0x80, "Fast-load area" }
    };

    private static readonly Dictionary<ushort, string> ApplicationFlags = new Dictionary<ushort, string>()
    {
        { 0x01, "Full-screen" },
        { 0x02, "Compatible with Windows PM" },
        { 0x04, "Uses Windows PM API" },
        { 0x08, "Supports long filenames" },
        { 0x10, "Supports real mode" },
        { 0x20, "Supports protected mode" },
    };

    private static readonly Dictionary<ushort, string> Os2Flags = new Dictionary<ushort, string>()
    {
        { 0x01, "OS/2 application" },
        { 0x02, "Bound as family app" },
        { 0x04, "No 8.3 rule" },
        { 0x08, "Supports real mode" },
        { 0x10, "Supports OS/2 2.x protected mode" },
    };
    
    public Dictionary<string, string[]> GetFlags()
    {
        Dictionary<string, string[]> characteristics = new Dictionary<string, string[]>();

        var programFlags =
            ProgramFlags
                .Where(f => (_head.pflags & f.Key) == f.Key)
                .Select(f => f.Value);
        characteristics.Add("_program", programFlags.ToArray());


        var applicationFlags =
            ApplicationFlags
                .Where(a => (_head.aflags & a.Key) == a.Key)
                .Select(a => a.Value);
        characteristics.Add("_application", applicationFlags.ToArray());

        var ibmOs2Flags =
            Os2Flags
                .Where(o => (_head.flagsothers & o.Key) == o.Key)
                .Select(o => o.Value);
        characteristics.Add("_others", ibmOs2Flags.ToArray());
        
        return characteristics;
    }

    public CommonProperties GetProperties()
    {
        _result.CpuArchitecture = _strings.CpuArchitectureFlagToString(_head.pflags);
        _result.CpuWordLength = _strings.CpuWordLengthFlagToString(0);
        _result.OperatingSystem = _strings.OperatingSystemFlagToString(_head.os);
        
        // гадание на кофейной гуще начинается здесь.
        if (_head is { os: 0, major: > 0 })
            _result.OperatingSystem = _strings.OperatingSystemFlagToString(2);
        
        if (_head.major > 0)
            _result.OperatingSystemVersion = _strings.OperatingSystemVersionToString(_head.major, _head.minor);
        else
        {
            _result.OperatingSystemVersion = _head.os switch
            {
                0x1 => _strings.OperatingSystemVersionToString(1, 1),
                0x3 => _strings.OperatingSystemVersionToString(4, 0),
                _ => _result.OperatingSystemVersion
            };
        }
        _result.Subsystem = _strings.SubsystemFromOperatingSystemFlagToString(_head.os);
        _result.ImageType = _strings.ImageTypeFlagToString(_head.os);
        _result.LinkerVersion = _strings.ImageVersionFlagsToString(_head.ver, _head.rev);
        
        return _result;
    }
    
    [UnderConstruction]
    public void GetImports()
    {
        throw new NotImplementedException();
    }
}