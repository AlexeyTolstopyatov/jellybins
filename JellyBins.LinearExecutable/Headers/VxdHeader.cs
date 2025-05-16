using System.Runtime.InteropServices;

namespace JellyBins.LinearExecutable.Headers;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct VxdHeader
{
    [MarshalAs(UnmanagedType.U4)] public UInt32 WindowsVXDVersionInfoResourceOffset;
    [MarshalAs(UnmanagedType.U4)] public UInt32 WindowsVXDVersionInfoResourceLength;
    [MarshalAs(UnmanagedType.U2)] public UInt16 WindowsVXDDeviceID;
    [MarshalAs(UnmanagedType.U1)] public Byte WindowsDDKVersionMinor; // char => byte
    [MarshalAs(UnmanagedType.U1)] public Byte WindowsDDKVersionMajor;
}