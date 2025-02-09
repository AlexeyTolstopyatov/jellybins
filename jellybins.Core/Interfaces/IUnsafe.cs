namespace jellybins.Core.Interfaces;

public interface IUnsafe
{
    void Fill<TStruct>(int offset, ref TStruct head) where TStruct : struct;
    void Fill<TStruct>(ref TStruct head) where TStruct : struct;
    byte GetUInt8(int offset);
    ushort GetUInt16(int offset);
    uint GetUInt32(int offset);
    ulong GetUInt64(int offset);
}