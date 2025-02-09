using System.Runtime.InteropServices;

namespace jellybins.Core.Headers.Runtime;

// 
// (C) Bilbo Backends 2024-2025
// 
//     Part of this information taken from other
// Visual Basic Decompilers and forums.
//     If values of Virtual Machine not equal "VB5!",
// may be not N-Code Visual Basic application. It is one of three ways to
// determine runtime usage. 
//
/// <summary>
/// Represents Virtual machine header.
/// Visual Basic Signature must be equals "VB5!" just because
/// after Visual Basic 5.0, applications become compiled as
/// Native-code contained.
/// </summary>
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct VbRuntimeHeader
{
    [MarshalAs(UnmanagedType.LPArray, SizeConst = 4)] 
    public char[] Signature; // VB5!
    [MarshalAs(UnmanagedType.LPArray, SizeConst = 12)]
    public char[] VirtualMachine; // msvbvm60.dll or 50. Other earlier versions dont know.
}

/// <summary>
/// Represents parts of two instructions, where find
/// EntryPoint of VB Virtual machine entry point
/// <code>
/// push (VB virtual machine signature)
/// jmp (entry-point offset)
/// </code>
/// </summary>
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct VbStartRuntimeHeader
{
    [MarshalAs(UnmanagedType.I1)]
    public byte PushStartOpCode;
    [MarshalAs(UnmanagedType.U4)]
    public uint PushVbSignatureOffset;
    [MarshalAs(UnmanagedType.I1)]
    public byte CallStartOpCode;
    [MarshalAs(UnmanagedType.U4)]
    public uint CallVbVmOffset;
}