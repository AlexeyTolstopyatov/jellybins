using System.Runtime.InteropServices;

namespace JellyBins.PortableExecutable.Headers;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct VbHeader
{
    [MarshalAs(UnmanagedType.U1, SizeConst = 4)]
    public Byte[] VbMagic;
    public UInt16 RuntimeBuild;
    public UInt64 LanguageDll;
    public UInt64 SecondLanguageDll;
    public UInt16 RuntimeRevision;
    public UInt32 LanguageId;
    public UInt32 SecondLanguageId;
    public UInt32 SubMainPointer;
    public UInt32 ProjectDataPointer;
    public UInt32 IntegerCtlsFlagLow;
    public UInt32 IntegerCtlsFlagHigh;
    public UInt32 ThreadFlags;
    public UInt32 ThreadCount;
    public UInt16 FormCtlsCount;
    public UInt16 ExternalCtlsCount;
    public UInt32 ThunkCount;
    public UInt32 GuiTablePointer;
    public UInt32 ExternalTablePointer;
    public UInt32 ComRegisterDataPointer;
    public UInt32 ProjectDescriptionOffset;
    public UInt32 ProjectExeNameOffset;
    public UInt32 ProjectHelpOffset;
    public UInt32 ProjectNameOffset;
}