namespace jellybins.File.Modeling.Base;

public interface IExecutableReader
{
    uint GetUInt32(int offset);
    ushort GetUInt16(int offset);
    byte GetUInt8(int offset);
    void Fill<TStruct>(ref TStruct head, int offset) where TStruct : struct;
    void Fill<TStruct>(ref TStruct head) where TStruct : struct;
}