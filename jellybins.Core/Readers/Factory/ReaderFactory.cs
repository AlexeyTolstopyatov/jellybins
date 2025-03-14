using jellybins.Core.Exceptions;
using jellybins.Core.Headers;
using jellybins.Core.Interfaces;
using jellybins.Core.Readers.AssemblerOutput;
using jellybins.Core.Readers.LinearExecutable;
using jellybins.Core.Readers.MarkZbykowski;
using jellybins.Core.Readers.NewExecutable;
using jellybins.Core.Readers.PortableExecutable;
using jellybins.Core.Utilities;

namespace jellybins.Core.Readers.Factory;

/// <summary>
/// Represents all instrumental for exploring binaries.
/// Includes tools for unsafe structures, 
/// </summary>
public partial class ReaderFactory
{
    private ushort _signatureWord;
    private ulong  _signatureQWord;
    private readonly string _fileName;
    /// <summary>
    /// Prepares instance to next work
    /// </summary>
    /// <param name="path"></param>
    public ReaderFactory(string path)
    {
        _fileName = path;
    }
    /// <summary>
    /// Creates ready IReader instance for your FILE's name
    /// and determines type of your binary.
    /// </summary>
    /// <exception cref="ImageTypeException">
    /// If binary contains unknown signature, this exception throws.
    /// Make sure, you are not exploring your own binaries format.
    /// </exception>
    public IReader CreateReader()
    {
        // 1) fill unsafe structure.
        // 2) make ...ExecutableReader() instance.
        //    IReader contains necessary methods for next analysis
        //    Not need to return results now.
        BinaryByRequestReader reader = new(_fileName);
        ushort firstWord = reader.GetUInt16(0);
        switch (firstWord)
        {
            // seek for MZ (ZM)
            case 0x4d5a:
            case 0x5a4d:
                MarkZbikowski mz = new();
                reader.Fill(ref mz);
                // what if not MS-DOS 2.0 executable?
                // mov ptr, sign
                _signatureWord = reader.GetUInt16((int)mz.e_lfanew);
                switch (_signatureWord)
                {
                    case 0x454c:
                    case 0x4c45:
                    case 0x4c58:
                    case 0x584c:
                    {
                        LinearExecutable.LinearExecutable le = new();
                        reader.Fill(ref le, (int)mz.e_lfanew);
                        if (le.WindowsDDKVersionMajor == 0) 
                            return new LinearExecutableReader(le);
                        return new WindowsDeviceDriverReader(le);
                    }
                    case 0x454e:
                    case 0x4e45:
                    {
                        NewExecutable.NewExecutable ne = new();
                        reader.Fill(ref ne, (int)mz.e_lfanew);
                        return new NewExecutableReader(ne);
                    }
                    case 0x4550:
                    case 0x5045:
                    {
                        PortableExecutable32 pe32 = new();
                        reader.Fill(ref pe32, (int)mz.e_lfanew);
                        if (pe32.WinNtOptional.Magic == 0x10b)
                            return new PortableExecutableReader(pe32, _fileName);
                        
                        PortableExecutable64 pe64 = new();
                        reader.Fill(ref pe64, (int)mz.e_lfanew);
                        return new PortableExecutableReader(pe64, _fileName);
                    }
                    default:
                        return new MarkZbykowskiExecutableReader(mz, _signatureWord);
                }
            // seek for Unix binaries
            case 
                0x103 or 0x301 or 
                0x107 or 0x701 or
                0x109 or 0x901 or 
                0xcc or 0x10b or 0xb01 or
                0x108 or 0x801:
                _signatureWord = firstWord;
                AssemblerOutput.AssemblerOutput unix = new();
                reader.Fill(ref unix);
                return new AssemblerOutExecutableReader(unix);
            default:
                InternalDebugger.PrintInformation("seeking segment resize: WORD -> QWORD");
                _signatureWord = firstWord;
                break;
        }
        
        ulong lastWord = reader.GetUInt64(0);
        _signatureWord = 0;                 // make analyser flag
        if ((lastWord & 0x464c457f) != 0)   // non-strong comparison
        {
            // ELF binary.
            _signatureQWord = lastWord;
        }
        throw new ImageTypeException(_signatureQWord);
    }
}