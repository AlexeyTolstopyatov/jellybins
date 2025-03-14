using jellybins.Core.Headers;
using jellybins.Core.Models;
using jellybins.Core.Interfaces;

namespace jellybins.Core.Readers.MarkZbykowski;

public class MarkZbykowskiExecutableReader : IReader
{
    private readonly MarkZbikowski _head;
    private readonly MarkZbykowskiExecutableStrings _strings;
    private ushort _longFileAddressValue;

    public MarkZbykowskiExecutableReader(MarkZbikowski head, ushort e_lfanew)
    {
        _strings = new MarkZbykowskiExecutableStrings();
        _head = head;
        _longFileAddressValue = e_lfanew;
    }
    
    public Dictionary<string, string> GetHeader()
    {
        return new Dictionary<string, string>()
        {
            {nameof(_head.e_sign), $"0x{_head.e_sign:X}"},
            {nameof(_head.e_lastb), $"0x{_head.e_fbl:X}"},
            {nameof(_head.e_relc), $"0x{_head.e_relc:X}"},
            {nameof(_head.e_pars), $"0x{_head.e_pars:X}"},
            {nameof(_head.e_minep), $"0x{_head.e_minep:X}"},
            {nameof(_head.e_maxep), $"0x{_head.e_maxep:X}"},
            {nameof(_head.ss), $"0x{_head.ss:X}"},
            {nameof(_head.cs), $"0x{_head.cs:X}"},
            {nameof(_head.sp), $"0x{_head.sp:X}"},
            {nameof(_head.ip), $"0x{_head.ip:X}"},
            {nameof(_head.e_check), $"0x{_head.e_check:X}"},
            {nameof(_head.e_reltableoff), $"0x{_head.e_reltableoff:X}"},
            {nameof(_head.e_overnum), $"0x{_head.e_overnum:X}"},
            {nameof(_head.e_res0x1c), $"[0x{_head.e_res0x1c[0]}, 0x{_head.e_res0x1c[1]:X}, 0x{_head.e_res0x1c[2]:X}, 0x{_head.e_res0x1c[3]:X}]"},
            {nameof(_head.e_oemid), $"0x{_head.e_oemid:X}"},
            {nameof(_head.e_oeminfo), $"{_head.e_oeminfo:X}"},
            {nameof(_head.e_lfanew), $"0x{_head.e_lfanew:X}"}
        };
    }

    public Dictionary<string, string[]> GetFlags()
    {
        return new Dictionary<string, string[]>()
        {
            {
                "_flags", new[] 
                    { 
                        $"e_lfanew: 0x{_longFileAddressValue:x}",
                        $"e_lfanew chars: {(char)(_longFileAddressValue & 0x00FF)} {(char)(_longFileAddressValue & 0xFF00)}",
                        $"OEM ID {_head.e_oemid}",
                        $"OEM Info {_head.e_oeminfo}",
                    }
            }
        };
    }

    public CommonProperties GetProperties()
    {
        return new CommonProperties()
        {
            OperatingSystem = _strings.OperatingSystemFlagToString(0),
            OperatingSystemVersion = _strings.OperatingSystemVersionToString(2, 0),
            CpuArchitecture = _strings.CpuArchitectureFlagToString(_head.e_oemid),
            CpuWordLength = _strings.CpuWordLengthFlagToString(0),
            ImageType = _strings.ImageTypeFlagToString(0),
            Subsystem = _strings.SubsystemFromOperatingSystemFlagToString(0)
        };
    }

    public Dictionary<string, string> GetImports()
    {
        Console.WriteLine("Not Implemented!");
        return new Dictionary<string, string>();
    }
}