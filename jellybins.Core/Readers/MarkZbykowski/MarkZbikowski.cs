using System.Runtime.InteropServices;

namespace jellybins.Core.Readers.MarkZbykowski
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct MarkZbikowski
    {
        [MarshalAs(UnmanagedType.U2)] public ushort e_sign;
        [MarshalAs(UnmanagedType.U2)] public ushort e_lastb;
        [MarshalAs(UnmanagedType.U2)] public ushort e_fbl;
        [MarshalAs(UnmanagedType.U2)] public ushort e_relc;
        [MarshalAs(UnmanagedType.U2)] public ushort e_pars;
        [MarshalAs(UnmanagedType.U2)] public ushort e_minep;
        [MarshalAs(UnmanagedType.U2)] public ushort e_maxep;
        [MarshalAs(UnmanagedType.U2)] public ushort ss;
        [MarshalAs(UnmanagedType.U2)] public ushort sp;
        [MarshalAs(UnmanagedType.U2)] public ushort e_check;
        [MarshalAs(UnmanagedType.U2)] public ushort ip;
        [MarshalAs(UnmanagedType.U2)] public ushort cs;
        [MarshalAs(UnmanagedType.U2)] public ushort e_reltableoff;
        [MarshalAs(UnmanagedType.U2)] public ushort e_overnum;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)] public ushort[] e_res0x1c;
        [MarshalAs(UnmanagedType.U2)] public ushort e_oemid;
        [MarshalAs(UnmanagedType.U2)] public ushort e_oeminfo;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)] public ushort[] e_res_0x28;
        [MarshalAs(UnmanagedType.U4)] public uint e_lfanew;
    }
}
