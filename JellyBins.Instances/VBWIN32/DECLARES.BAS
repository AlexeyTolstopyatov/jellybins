Attribute VB_Name = "Module1"
Option Explicit
Dim WinVersion As Integer, SoundAvailable As Integer
Global VisibleFrame As Frame
 
Global Const TWIPS = 1
Global Const PIXELS = 3
Global Const RES_INFO = 2
Global Const MINIMIZED = 1

Type Rect
    Left As Integer
    Top As Integer
    Right As Integer
    Bottom As Integer
End Type


'begin conditional compile

#If Win32 Then              '32-bit declares

    Public Type SystemInfo
        dwOemId As Long
        dwPageSize As Long
        lpMinimumApplicationAddress As Long
        lpMaximumApplicationAddress As Long
        dwActiveProcessorMask As Long
        dwNumberOfProcessors As Long
        dwProcessorType As Long
        dwAllocationGranularity As Long
        dwReserved As Long
    End Type
    
    Public Type MemoryStatus
        dwLength As Long
        dwMemoryLoad As Long
        dwTotalPhys As Long
        dwAvailPhys As Long
        dwTotalPageFile As Long
        dwAvailPageFile As Long
        dwTotalVirtual As Long
        dwAvailVirtual As Long
    End Type
    
    Declare Function GetFreeSpace Lib "kernel32" Alias "GetFreeSpaceA" (ByVal flag As Integer) As Long
    Declare Function GetWindowsDirectory Lib "kernel32" Alias "GetWindowsDirectoryA" (ByVal lpBuffer As String, ByVal nSize As Long) As Long
    
    'the GetSystemInfo sub (following) replaces the GetWinFlags function in the Win16 API
    Declare Sub GetSystemInfo Lib "kernel32" (lpSystemInfo As SystemInfo)
        
    'the GlobalMemoryStatus sub (following) substitutes for GetFreeSystemResources in the Win16 API
    Declare Sub GlobalMemoryStatus Lib "kernel32" (lpBuffer As MemoryStatus)
    
    Declare Function GetVersion Lib "kernel32" () As Long
    Declare Function GetSystemMetrics Lib "User32" (ByVal nIndex As Long) As Long
    Declare Function GetDeviceCaps Lib "GDI32" (ByVal hDC As Long, ByVal nIndex As Long) As Long
    Declare Function GlobalCompact Lib "kernel32" (ByVal dwMinFree As Long) As Long
    Declare Function TrackPopupMenu Lib "User32" (ByVal hMenu As Long, ByVal wFlags As Long, ByVal X As Long, ByVal Y As Long, ByVal nReserved As Long, ByVal hWnd As Long, lpReserved As Any) As Long
    Declare Function GetMenu Lib "User32" (ByVal hWnd As Long) As Long
    Declare Function GetSubMenu Lib "User32" (ByVal hMenu As Long, ByVal nPos As Long) As Long
    Declare Function InsertMenu Lib "User32" Alias "InsertMenuA" (ByVal hMenu As Long, ByVal nPosition As Long, ByVal wFlags As Long, ByVal wIDNewItem As Long, ByVal lpNewItem As Any) As Long
    Declare Function GetSystemMenu Lib "User32" (ByVal hWnd As Long, ByVal bRevert As Long) As Long
    Declare Function GetDesktopWindow Lib "User32" () As Long
    Declare Function GetDC Lib "User32" (ByVal hWnd As Long) As Long
    Declare Function ReleaseDC Lib "User32" (ByVal hWnd As Long, ByVal hDC As Long) As Long
    Declare Function BitBlt Lib "GDI32" (ByVal hDestDC As Long, ByVal X As Long, ByVal Y As Long, ByVal nWidth As Long, ByVal nHeight As Long, ByVal hSrcDC As Long, ByVal XSrc As Long, ByVal YSrc As Long, ByVal dwRop As Long) As Long
    Declare Sub SetWindowPos Lib "User32" (ByVal hWnd As Long, ByVal hWndInsertAfter As Long, ByVal X As Long, ByVal Y As Long, ByVal cx As Long, ByVal cy As Long, ByVal wFlags As Long)
    Declare Function GetPrivateProfileString Lib "kernel32" Alias "GetPrivateProfileStringA" (ByVal lpApplicationName As String, lpKeyName As Any, ByVal lpDefault As String, ByVal lpRetunedString As String, ByVal nSize As Long, ByVal lpFileName As String) As Long
    Declare Function GetProfileString Lib "kernel32" Alias "GetProfileStringA" (ByVal lpAppName As String, lpKeyName As Any, ByVal lpDefault As String, ByVal lpReturnedString As String, ByVal nSize As Long) As Long
    Declare Function waveOutGetNumDevs Lib "winmm" () As Long
    Declare Function GetSystemDirectory Lib "kernel32" Alias "GetSystemDirectoryA" (ByVal lpBuffer As String, ByVal nSize As Long) As Long
    Declare Function sndPlaySound Lib "winmm" Alias "sndPlaySoundA" (ByVal lpszSoundName As String, ByVal uFlags As Long) As Long

#Else               '16-bit declares
    
    Declare Function GetPrivateProfileString Lib "Kernel" (ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal lpDefault As String, ByVal lpReturnedString As String, ByVal nSize As Integer, ByVal lpFileName As String) As Integer
    Declare Function GetProfileString Lib "Kernel" (ByVal lpAppName As String, ByVal lpKeyName As String, ByVal lpDefault As String, ByVal lpReturnedString As String, ByVal nSize As Integer) As Integer
    Declare Sub SetWindowPos Lib "User" (ByVal h1%, ByVal h2%, ByVal X%, ByVal Y%, ByVal cx%, ByVal cy%, ByVal f%)
    Declare Function GetDesktopWindow Lib "User" Alias "GetDeskTopWindow" () As Integer
    Declare Function GetDC Lib "User" (ByVal hWnd%) As Integer
    Declare Sub ReleaseDC Lib "User" (ByVal hWnd%, ByVal hDC%)
    Declare Function BitBlt Lib "GDI" (ByVal destDC%, ByVal X%, ByVal Y%, ByVal w%, ByVal h%, ByVal srchDC%, ByVal srcX%, ByVal srcY%, ByVal rop&) As Integer
    Declare Function TrackPopupMenu Lib "User" (ByVal hMenu%, ByVal wFlags%, ByVal X%, ByVal Y%, ByVal r2%, ByVal hWd%, R As Rect) As Integer
    Declare Function GetMenu Lib "User" (ByVal hWd%) As Integer
    Declare Function GetSubMenu Lib "User" (ByVal hMenu%, ByVal nPos%) As Integer
    Declare Function InsertMenu Lib "User" (ByVal hMenu As Integer, ByVal nPosition As Integer, ByVal wFlags As Integer, ByVal wIDNewItem As Integer, ByVal lpNewItem As Any) As Integer
    Declare Function GetSystemMenu Lib "User" (ByVal hWnd As Integer, ByVal bRevert As Integer) As Integer
    Declare Function sndPlaySound Lib "MMSystem" (lpsound As Any, ByVal flag As Integer) As Integer
    Declare Function waveOutGetNumDevs Lib "MMSystem" () As Integer
    Declare Function GlobalCompact Lib "Kernel" (ByVal flag&) As Long
    Declare Function GetFreeSpace Lib "Kernel" (ByVal flag%) As Long
    
    'there is no equivalent to GetFreeSystemResources (following) in the Win32 API
    Declare Function GetFreeSystemResources Lib "User" (ByVal fuSysResource As Integer) As Integer
    Declare Function GetDeviceCaps Lib "GDI" (ByVal hDC%, ByVal nIndex%) As Integer
    Declare Function GetVersion Lib "Kernel" () As Long
    Declare Function GetSystemMetrics Lib "User" (ByVal n As Integer) As Integer
    Declare Function GetWindowsDirectory Lib "Kernel" (ByVal P$, ByVal S%) As Integer
    Declare Function GetSystemDirectory Lib "Kernel" (ByVal P$, ByVal S%) As Integer
    Declare Function GetWinFlags Lib "Kernel" () As Long
#End If
'end conditional compile

Global Const WF_CPU286 = &H2&
Global Const WF_CPU386 = &H4&
Global Const WF_CPU486 = &H8&
Global Const WF_STANDARD = &H10&
Global Const WF_ENHANCED = &H20&
Global Const WF_80x87 = &H400&

Global Const SM_MOUSEPRESENT = 19

Global Const GFSR_SYSTEMRESOURCES = &H0
Global Const GFSR_GDIRESOURCES = &H1
Global Const GFSR_USERRESOURCES = &H2

Global Const MF_POPUP = &H10
Global Const MF_BYPOSITION = &H400
Global Const MF_SEPARATOR = &H800

Global Const SRCCOPY = &HCC0020
Global Const SRCERASE = &H440328
Global Const SRCINVERT = &H660046
Global Const SRCAND = &H8800C6

Global Const HWND_TOPMOST = -1
Global Const HWND_NOTOPMOST = -2
Global Const SWP_NOACTIVATE = &H10
Global Const SWP_SHOWWINDOW = &H40


Function DeviceColors(hDC As Integer) As Long
Const PLANES = 14
Const BITSPIXEL = 12
    DeviceColors = GetDeviceCaps(hDC, PLANES) * 2 ^ GetDeviceCaps(hDC, BITSPIXEL)
End Function

Function DosVersion()
Dim Ver As Long, DosVer As Long
    Ver = GetVersion()
    DosVer = Ver \ &H10000
    DosVersion = Format((DosVer \ 256) + ((DosVer Mod 256) / 100), "Fixed")
End Function

Function GetSysIni(section, key)
Dim retVal As String, AppName As String, worked As Integer
    retVal = String$(255, 0)
    worked = GetPrivateProfileString(section, key, "", retVal, Len(retVal), "System.ini")
    If worked = 0 Then
        GetSysIni = "unknown"
    Else
        GetSysIni = Left(retVal, InStr(retVal, Chr(0)) - 1)
    End If
End Function

Function GetWinIni(section, key)
Dim retVal As String, AppName As String, worked As Integer
    retVal = String$(255, 0)
    worked = GetProfileString(section, key, "", retVal, Len(retVal))
    If worked = 0 Then
        GetWinIni = "unknown"
    Else
        GetWinIni = Left(retVal, InStr(retVal, Chr(0)) - 1)
    End If
End Function

Function SystemDirectory() As String
Dim WinPath As String
    WinPath = String(145, Chr(0))
    SystemDirectory = Left(WinPath, GetSystemDirectory(WinPath, InStr(WinPath, Chr(0)) - 1))
End Function

Function WindowsDirectory() As String
Dim WinPath As String
Dim temp
    WinPath = String(145, Chr(0))
    temp = GetWindowsDirectory(WinPath, 145)
    WindowsDirectory = Left(WinPath, InStr(WinPath, Chr(0)) - 1)
End Function

Function WindowsVersion()
Dim Ver As Long, WinVer As Long
    Ver = GetVersion()
    WinVer = Ver And &HFFFF&
    WindowsVersion = Format((WinVer Mod 256) + ((WinVer \ 256) / 100), "Fixed")
End Function

