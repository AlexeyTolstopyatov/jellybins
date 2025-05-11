using System.Runtime.InteropServices;
using JellyBins.PortableExecutable.Headers;

namespace JellyBins.PortableExecutable.Models;

/// <summary>
/// Структура представляет собой
/// компот из таблиц внутри
/// исполняемого файла PE. И будет ведущей структурой
/// для анализа данных.
/// </summary>
public class ProgramHeaders
{
    public String RuntimeWord { get; init; } = String.Empty;
    public MzHeader MzHeader { get; private set; }
    public RichHeader RichHeader { get; private set; }
    public PeHeader PeHeader { get; private set; }
    public PeHeader32 PeHeader32 { get; private set; }
    public PeHeaderRom PeHeaderRom { get; private set; }
    public PeSection[] Sections { get; private set; } = [];
    public Cor20Header Cor20Header { get; private set; }
    public VbHeader VbHeader { get; private set; }
    
    public Boolean RichHeaderFound { get; private set; }
    public Boolean Cor20HeaderFound { get; private set; }
    public Boolean ExportsFound { get; private set; }
    public Boolean ImportsFound { get; private set; }
    public Boolean RuntimeFound { get; private set; }
    public Boolean Is64Bit { get; private set; }
    public Boolean IsWin32Zero { get; private set; }
    public Boolean IsRomImage { get; private set; }

    public ProgramHeaders(String path)
    {
        
    }
    
    private void ReadPeHeader(BinaryReader reader)
    {
        if (Is64Bit) PeHeader = Fill<PeHeader>(reader);
        else PeHeader32 = Fill<PeHeader32>(reader);
    }
    
    private TStruct Fill<TStruct>(BinaryReader reader) where TStruct : struct
    {
        Byte[] bytes = reader.ReadBytes(Marshal.SizeOf(typeof(TStruct)));
        GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
        TStruct result = Marshal.PtrToStructure<TStruct>(handle.AddrOfPinnedObject());
        handle.Free();
        return result;
    }

    /// <summary>
    /// Узнает только разрядность машины (х64 или х32)
    /// </summary>
    /// <param name="characteristics">Требуемая архитектура</param>
    private Boolean Machine64Bit(UInt16 characteristics)
    {
        if ((characteristics & 0x0100) == 0)
            return true;

        return false;
    }
}

