using System.Runtime.InteropServices;
using jellybins.Core.Attributes;
using jellybins.Core.Exceptions;
using jellybins.Core.Interfaces;
using jellybins.Core.Strings;

namespace jellybins.Core.Readers.LinearExecutable;

public class LinearExecutableStrings : IStrings
{
    public string OperatingSystemFlagToString<T>([Optional] T os) where T : IComparable
    {
        return Convert.ToInt32(os) switch
        {
            1 => "IBM OS/2",
            2 => "Microsoft Windows/286",
            3 => "Microsoft DOS",
            4 => "Microsoft Windows/386",
            _ => throw new UndefinedArgumentException(Convert.ToInt32(os))
        };
    }

    public string CpuArchitectureFlagToString<T>(T cpu) where T : IComparable
    {
        return Convert.ToInt32(cpu) switch
        {
            1 => "Intel i286",
            2 => "Intel i386",
            3 => "Intel i486",
            _ => throw new UndefinedArgumentException(Convert.ToInt32(cpu))
        };
    }

    /// <summary>
    /// Detecting version by 2 parameters (not version parts)
    /// </summary>
    /// <param name="major">OS flag</param>
    /// <param name="minor">unused)</param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public string OperatingSystemVersionToString<T>(T major, T minor) where T : IComparable
    {
        int os = Convert.ToInt32(major);
        return os switch
        {
            1 => "2.0",
            3 => "4.0",
            2 or 4 => "3.0", // minimum 
            _ => throw new UndefinedArgumentException(os)
        };
    }
    
    public string ImageVersionFlagsToString<T>(T major, T minor)
    {
        return $"{major}.{minor}";
    }
    [UnderConstruction]
    public string CpuWordLengthFlagToString<T>(T word = default(T))
    {
        return Convert.ToInt16(word) == 0 ? "16" : "32";
    }

    public string SubsystemFromOperatingSystemFlagToString<T>([Optional] T word)
    {
        // OS/2 2.x applications not supported already.
        // WxD model drivers Uses Native 16-bit ware. No way.
        return PortableExecutableSubsystem.Native.ToString();
    }

    public string ImageTypeFlagToString<T>(T word)
    {
        uint module = Convert.ToUInt32(word);
        ImageType result;
        // Get module type from ModuleFlags table. 

        if ((module & 0x00008000) != 0) result = ImageType.DynamicLinkedLibrary;
        else if ((module & 0x00028000) != 0) result = ImageType.VirtualDriver;
        else if ((module & 0x00020000) != 0) result = ImageType.PhysicalDriver;
        else result = ImageType.Application; // OS/2 .EXE
        
        return result.ToString();
    }

    public string HeaderSignatureToString(ushort sig)
    {
        char[] sigc = new char[2];
        sigc[0] = (char)(sig & 0xFF);
        sigc[1] = (char)((sig >> 8) & 0xFF);

        return new string(sigc);
    }
}