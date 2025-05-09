namespace JellyBins.PortableExecutable.Private.Types;

public enum RichProductType 
{
    Import = 0x0001,
    Export = 0x0002,
    Resource = 0x0003,
    Exception = 0x0004,
    Secure = 0x0005,
    Relocation = 0x0006,
    Debug = 0x0007,
    Architecture = 0x0008,
    GlobalPointer = 0x0009,
    Tls = 0x000A,
    LoadConfiguration = 0x000B,
    BoundImport = 0x000C,
    Iat = 0x000D,
    DelayedImport = 0x000E,
    ComRuntime = 0x000F,
    
    CCompiler = 0x0081, // cl.exe
    CxxCompiler = 0x0083, // msvc?
    ResourceCompiler = 0x0084, // rc.exe
    Linker = 0x008A, // ld.exe link.exe
    NetCompiler = 0x007A, // csc.exe fsc.exe
    Assembler = 0x0094, // ml.exe ml64.exe
    CCxxOptimizingCompiler = 0x0095,
}