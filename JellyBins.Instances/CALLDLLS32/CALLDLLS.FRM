VERSION 4.00
Begin VB.Form frmCallDlls 
   BorderStyle     =   1  'Fixed Single
   Caption         =   "Calling DLL Procedures"
   ClientHeight    =   1890
   ClientLeft      =   2400
   ClientTop       =   3270
   ClientWidth     =   5535
   ClipControls    =   0   'False
   BeginProperty Font 
      name            =   "MS Sans Serif"
      charset         =   1
      weight          =   700
      size            =   8.25
      underline       =   0   'False
      italic          =   0   'False
      strikethrough   =   0   'False
   EndProperty
   Height          =   2295
   Left            =   2340
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   ScaleHeight     =   1890
   ScaleWidth      =   5535
   Top             =   2925
   Width           =   5655
   Begin VB.PictureBox picSprite 
      AutoRedraw      =   -1  'True
      AutoSize        =   -1  'True
      BeginProperty Font 
         name            =   "MS Sans Serif"
         charset         =   1
         weight          =   700
         size            =   8.25
         underline       =   0   'False
         italic          =   0   'False
         strikethrough   =   0   'False
      EndProperty
      Height          =   510
      Left            =   960
      Picture         =   "CALLDLLS.frx":0000
      ScaleHeight     =   32
      ScaleMode       =   3  'Pixel
      ScaleWidth      =   32
      TabIndex        =   25
      Top             =   1920
      Visible         =   0   'False
      Width           =   510
   End
   Begin VB.PictureBox picCopy 
      AutoRedraw      =   -1  'True
      BorderStyle     =   0  'None
      BeginProperty Font 
         name            =   "MS Sans Serif"
         charset         =   1
         weight          =   700
         size            =   8.25
         underline       =   0   'False
         italic          =   0   'False
         strikethrough   =   0   'False
      EndProperty
      Height          =   495
      Left            =   1680
      ScaleHeight     =   33
      ScaleMode       =   3  'Pixel
      ScaleWidth      =   33
      TabIndex        =   24
      Top             =   1920
      Visible         =   0   'False
      Width           =   495
   End
   Begin VB.PictureBox picMask 
      AutoRedraw      =   -1  'True
      AutoSize        =   -1  'True
      BeginProperty Font 
         name            =   "MS Sans Serif"
         charset         =   1
         weight          =   700
         size            =   8.25
         underline       =   0   'False
         italic          =   0   'False
         strikethrough   =   0   'False
      EndProperty
      Height          =   510
      Left            =   240
      Picture         =   "CALLDLLS.frx":030A
      ScaleHeight     =   32
      ScaleMode       =   3  'Pixel
      ScaleWidth      =   32
      TabIndex        =   23
      Top             =   1920
      Visible         =   0   'False
      Width           =   510
   End
   Begin VB.CommandButton cmdBitBlt 
      Caption         =   "BitBlt"
      BeginProperty Font 
         name            =   "MS Sans Serif"
         charset         =   1
         weight          =   700
         size            =   8.25
         underline       =   0   'False
         italic          =   0   'False
         strikethrough   =   0   'False
      EndProperty
      Height          =   495
      Left            =   4680
      TabIndex        =   22
      Top             =   1320
      Width           =   735
   End
   Begin VB.Frame fraInfo 
      Caption         =   "Instructions"
      ClipControls    =   0   'False
      BeginProperty Font 
         name            =   "MS Sans Serif"
         charset         =   1
         weight          =   700
         size            =   8.25
         underline       =   0   'False
         italic          =   0   'False
         strikethrough   =   0   'False
      EndProperty
      Height          =   1695
      Index           =   0
      Left            =   120
      TabIndex        =   0
      Top             =   120
      Width           =   4455
      Begin VB.Label lblInfo 
         Caption         =   "Click the right mouse button on the icons to the right."
         BeginProperty Font 
            name            =   "MS Sans Serif"
            charset         =   1
            weight          =   700
            size            =   8.25
            underline       =   0   'False
            italic          =   0   'False
            strikethrough   =   0   'False
         EndProperty
         Height          =   495
         Index           =   1
         Left            =   840
         TabIndex        =   1
         Top             =   480
         Width           =   2415
      End
   End
   Begin VB.Timer tmrBounce 
      Enabled         =   0   'False
      Interval        =   1
      Left            =   4080
      Top             =   1920
   End
   Begin VB.Frame fraInfo 
      Caption         =   "General Info"
      ClipControls    =   0   'False
      BeginProperty Font 
         name            =   "MS Sans Serif"
         charset         =   1
         weight          =   700
         size            =   8.25
         underline       =   0   'False
         italic          =   0   'False
         strikethrough   =   0   'False
      EndProperty
      Height          =   1695
      Index           =   4
      Left            =   120
      TabIndex        =   13
      Top             =   120
      Visible         =   0   'False
      Width           =   4455
      Begin VB.Label lblInfo 
         Caption         =   "Keyboard:"
         BeginProperty Font 
            name            =   "MS Sans Serif"
            charset         =   1
            weight          =   700
            size            =   8.25
            underline       =   0   'False
            italic          =   0   'False
            strikethrough   =   0   'False
         EndProperty
         Height          =   435
         Index           =   14
         Left            =   120
         TabIndex        =   17
         Top             =   1080
         Width           =   4230
      End
      Begin VB.Label lblInfo 
         AutoSize        =   -1  'True
         Caption         =   "Language:"
         BeginProperty Font 
            name            =   "MS Sans Serif"
            charset         =   1
            weight          =   700
            size            =   8.25
            underline       =   0   'False
            italic          =   0   'False
            strikethrough   =   0   'False
         EndProperty
         Height          =   195
         Index           =   13
         Left            =   120
         TabIndex        =   16
         Top             =   840
         Width           =   915
      End
      Begin VB.Label lblInfo 
         AutoSize        =   -1  'True
         Caption         =   "Mouse:"
         BeginProperty Font 
            name            =   "MS Sans Serif"
            charset         =   1
            weight          =   700
            size            =   8.25
            underline       =   0   'False
            italic          =   0   'False
            strikethrough   =   0   'False
         EndProperty
         Height          =   195
         Index           =   12
         Left            =   120
         TabIndex        =   15
         Top             =   360
         Width           =   630
      End
      Begin VB.Label lblInfo 
         AutoSize        =   -1  'True
         Caption         =   "Network:"
         BeginProperty Font 
            name            =   "MS Sans Serif"
            charset         =   1
            weight          =   700
            size            =   8.25
            underline       =   0   'False
            italic          =   0   'False
            strikethrough   =   0   'False
         EndProperty
         Height          =   195
         Index           =   11
         Left            =   120
         TabIndex        =   14
         Top             =   600
         Width           =   780
      End
   End
   Begin VB.Frame fraInfo 
      Caption         =   "Operating System"
      ClipControls    =   0   'False
      BeginProperty Font 
         name            =   "MS Sans Serif"
         charset         =   1
         weight          =   700
         size            =   8.25
         underline       =   0   'False
         italic          =   0   'False
         strikethrough   =   0   'False
      EndProperty
      Height          =   1695
      Index           =   1
      Left            =   120
      TabIndex        =   2
      Top             =   120
      Visible         =   0   'False
      Width           =   4455
      Begin VB.Label lblInfo 
         AutoSize        =   -1  'True
         Caption         =   "(Enhanced mode)"
         BeginProperty Font 
            name            =   "MS Sans Serif"
            charset         =   1
            weight          =   700
            size            =   8.25
            underline       =   0   'False
            italic          =   0   'False
            strikethrough   =   0   'False
         EndProperty
         Height          =   195
         Index           =   3
         Left            =   360
         TabIndex        =   8
         Top             =   600
         Width           =   1500
      End
      Begin VB.Label lblInfo 
         AutoSize        =   -1  'True
         Caption         =   "Disk Operating System 5.0"
         BeginProperty Font 
            name            =   "MS Sans Serif"
            charset         =   1
            weight          =   700
            size            =   8.25
            underline       =   0   'False
            italic          =   0   'False
            strikethrough   =   0   'False
         EndProperty
         Height          =   195
         Index           =   4
         Left            =   240
         TabIndex        =   4
         Top             =   960
         Width           =   2265
      End
      Begin VB.Label lblInfo 
         AutoSize        =   -1  'True
         Caption         =   "Microsoft Windows Version 3.1"
         BeginProperty Font 
            name            =   "MS Sans Serif"
            charset         =   1
            weight          =   700
            size            =   8.25
            underline       =   0   'False
            italic          =   0   'False
            strikethrough   =   0   'False
         EndProperty
         Height          =   195
         Index           =   2
         Left            =   240
         TabIndex        =   3
         Top             =   360
         Width           =   2640
      End
   End
   Begin VB.Frame fraInfo 
      Caption         =   "Processor, Memory, and System Resources"
      ClipControls    =   0   'False
      BeginProperty Font 
         name            =   "MS Sans Serif"
         charset         =   1
         weight          =   700
         size            =   8.25
         underline       =   0   'False
         italic          =   0   'False
         strikethrough   =   0   'False
      EndProperty
      Height          =   1695
      Index           =   2
      Left            =   120
      TabIndex        =   5
      Top             =   120
      Visible         =   0   'False
      Width           =   4455
      Begin VB.Timer tmrSysInfo 
         Interval        =   1
         Left            =   3840
         Top             =   240
      End
      Begin VB.Shape shpFrame 
         Height          =   255
         Index           =   1
         Left            =   1080
         Top             =   960
         Width           =   3135
      End
      Begin VB.Shape shpBar 
         BackStyle       =   1  'Opaque
         DrawMode        =   7  'Invert
         Height          =   255
         Index           =   1
         Left            =   1080
         Top             =   960
         Width           =   1695
      End
      Begin VB.Shape shpFrame 
         Height          =   255
         Index           =   2
         Left            =   1080
         Top             =   1320
         Width           =   3135
      End
      Begin VB.Shape shpBar 
         BackStyle       =   1  'Opaque
         DrawMode        =   7  'Invert
         Height          =   255
         Index           =   2
         Left            =   1080
         Top             =   1320
         Width           =   1695
      End
      Begin VB.Label lblResInfo 
         Alignment       =   2  'Center
         Caption         =   "user"
         BeginProperty Font 
            name            =   "MS Sans Serif"
            charset         =   1
            weight          =   700
            size            =   8.25
            underline       =   0   'False
            italic          =   0   'False
            strikethrough   =   0   'False
         EndProperty
         Height          =   255
         Index           =   2
         Left            =   1080
         TabIndex        =   21
         Top             =   1320
         Width           =   3135
      End
      Begin VB.Label lblResInfo 
         Alignment       =   2  'Center
         Caption         =   "gdi"
         BeginProperty Font 
            name            =   "MS Sans Serif"
            charset         =   1
            weight          =   700
            size            =   8.25
            underline       =   0   'False
            italic          =   0   'False
            strikethrough   =   0   'False
         EndProperty
         Height          =   255
         Index           =   1
         Left            =   1080
         TabIndex        =   20
         Top             =   960
         Width           =   3135
      End
      Begin VB.Label lblR 
         Caption         =   "GDI"
         BeginProperty Font 
            name            =   "MS Sans Serif"
            charset         =   1
            weight          =   700
            size            =   8.25
            underline       =   0   'False
            italic          =   0   'False
            strikethrough   =   0   'False
         EndProperty
         Height          =   255
         Index           =   1
         Left            =   240
         TabIndex        =   19
         Top             =   960
         Width           =   855
      End
      Begin VB.Label lblR 
         Caption         =   "User"
         BeginProperty Font 
            name            =   "MS Sans Serif"
            charset         =   1
            weight          =   700
            size            =   8.25
            underline       =   0   'False
            italic          =   0   'False
            strikethrough   =   0   'False
         EndProperty
         Height          =   255
         Index           =   2
         Left            =   240
         TabIndex        =   18
         Top             =   1320
         Width           =   855
      End
      Begin VB.Label lblInfo 
         AutoSize        =   -1  'True
         Caption         =   "CPU: 486 (with Math Coprocessor)"
         BeginProperty Font 
            name            =   "MS Sans Serif"
            charset         =   1
            weight          =   700
            size            =   8.25
            underline       =   0   'False
            italic          =   0   'False
            strikethrough   =   0   'False
         EndProperty
         Height          =   195
         Index           =   5
         Left            =   240
         TabIndex        =   7
         Top             =   360
         Width           =   2940
      End
      Begin VB.Label lblInfo 
         AutoSize        =   -1  'True
         Caption         =   "Memory Free"
         BeginProperty Font 
            name            =   "MS Sans Serif"
            charset         =   1
            weight          =   700
            size            =   8.25
            underline       =   0   'False
            italic          =   0   'False
            strikethrough   =   0   'False
         EndProperty
         Height          =   195
         Index           =   6
         Left            =   240
         TabIndex        =   6
         Top             =   600
         Width           =   1095
      End
   End
   Begin VB.Frame fraInfo 
      Caption         =   "Video"
      ClipControls    =   0   'False
      BeginProperty Font 
         name            =   "MS Sans Serif"
         charset         =   1
         weight          =   700
         size            =   8.25
         underline       =   0   'False
         italic          =   0   'False
         strikethrough   =   0   'False
      EndProperty
      Height          =   1695
      Index           =   3
      Left            =   120
      TabIndex        =   9
      Top             =   120
      Visible         =   0   'False
      Width           =   4455
      Begin VB.Label lblInfo 
         AutoSize        =   -1  'True
         Caption         =   "Colors:"
         BeginProperty Font 
            name            =   "MS Sans Serif"
            charset         =   1
            weight          =   700
            size            =   8.25
            underline       =   0   'False
            italic          =   0   'False
            strikethrough   =   0   'False
         EndProperty
         Height          =   195
         Index           =   10
         Left            =   240
         TabIndex        =   12
         Top             =   1320
         Width           =   600
      End
      Begin VB.Label lblInfo 
         AutoSize        =   -1  'True
         Caption         =   "Resolution"
         BeginProperty Font 
            name            =   "MS Sans Serif"
            charset         =   1
            weight          =   700
            size            =   8.25
            underline       =   0   'False
            italic          =   0   'False
            strikethrough   =   0   'False
         EndProperty
         Height          =   195
         Index           =   9
         Left            =   240
         TabIndex        =   11
         Top             =   960
         Width           =   915
      End
      Begin VB.Label lblInfo 
         Caption         =   "Video Driver:"
         BeginProperty Font 
            name            =   "MS Sans Serif"
            charset         =   1
            weight          =   700
            size            =   8.25
            underline       =   0   'False
            italic          =   0   'False
            strikethrough   =   0   'False
         EndProperty
         Height          =   495
         Index           =   8
         Left            =   240
         TabIndex        =   10
         Top             =   360
         Width           =   3975
      End
   End
   Begin VB.Image ImgIcon 
      Height          =   480
      Index           =   1
      Left            =   4800
      Picture         =   "CALLDLLS.frx":0614
      Top             =   720
      Width           =   480
   End
   Begin VB.Image ImgIcon 
      Height          =   480
      Index           =   0
      Left            =   4800
      Picture         =   "CALLDLLS.frx":091E
      Top             =   120
      Width           =   480
   End
End
Attribute VB_Name = "frmCallDlls"
Attribute VB_Creatable = False
Attribute VB_Exposed = False
Option Explicit
Dim dx As Integer, dy As Integer, X As Integer, Y As Integer
Dim PicWidth As Integer, PicHeight As Integer
Dim RightEdge As Integer, BottomEdge As Integer

Private Sub cmdBitBlt_Click()
Dim t As Integer
    If tmrBounce.Enabled Then
        tmrBounce.Enabled = False
        Refresh
    Else
        ScaleMode = PIXELS
        dx = 15
        dy = 15
        tmrBounce.Enabled = True
        PicWidth = picSprite.ScaleWidth
        PicHeight = picSprite.ScaleHeight
        picCopy.Width = PicWidth
        picCopy.Height = PicHeight
        t = BitBlt(picCopy.hDC, 0, 0, PicWidth, PicHeight, hDC, X, Y, SRCCOPY)
    End If
End Sub

'begin conditional compile
#If Win32 Then              '32-bit version of sub
    Private Sub FillSysInfo()
        Dim FreeSpace As Currency, FreeBlock As Currency, temp
        frainfo(4).Visible = False
       
        'Operating System Info.
            Dim YourSystem As SystemInfo
            GetSystemInfo YourSystem
            
            If WindowsVersion() = 4 Then
                lblinfo(2).Caption = "Microsoft Windows 95"
                lblinfo(3).Caption = ""
                lblinfo(4).Caption = ""
            Else
                lblinfo(2).Caption = "Microsoft Windows NT"
                lblinfo(3).Caption = ""
                lblinfo(4).Caption = ""
            End If
                    
        ' CPU Info.
            lblinfo(5).Caption = "CPU: " & YourSystem.dwProcessorType
            
        ' Video info.
            lblinfo(8).Visible = False
            lblinfo(9).Caption = "Resolution: " & Screen.Width \ Screen.TwipsPerPixelX & " x " & Screen.Height \ Screen.TwipsPerPixelY
            lblinfo(10).Caption = "Colors: " & DeviceColors((hDC))
        
        ' General info.
            If GetSystemMetrics(SM_MOUSEPRESENT) Then
                lblinfo(11).Caption = "Mouse: " & GetSysIni("boot.description", "mouse.drv")
            Else
                lblinfo(11).Caption = "No mouse"
            End If
            lblinfo(12).Caption = "Network: " & GetSysIni("boot.description", "network.drv")
            lblinfo(13).Caption = "Language: " & GetSysIni("boot.description", "language.dll")
            lblinfo(14).Caption = "Keyboard: " & GetSysIni("boot.description", "keyboard.typ")
    End Sub

#Else                       '16-bit version of sub

    Private Sub FillSysInfo()
        Dim FreeSpace As Currency, FreeBlock As Currency, temp
        Dim WinFlags As Long
        
        ' Operating System Info.
        WinFlags = GetWinFlags()
        
        lblinfo(2).Caption = "Microsoft Windows Version " & WindowsVersion()
        If WinFlags And WF_ENHANCED Then
            lblinfo(3).Caption = "(Enhanced Mode)"
        Else
            lblinfo(3).Caption = "(Standard Mode)"
        End If
        
        lblinfo(4).Caption = "Disk Operating System " & DosVersion()
        
        ' CPU Info.
           If WinFlags And WF_CPU486 Then
            lblinfo(5).Caption = "CPU: 486"
        ElseIf WinFlags And WF_CPU386 Then
            lblinfo(5).Caption = "CPU: 386"
        ElseIf WinFlags And WF_CPU286 Then
            lblinfo(5).Caption = "CPU: 286"
        End If
        If WinFlags And WF_80x87 Then
            lblinfo(5).Caption = lblinfo(5).Caption & " (with Math coprocessor)"
        End If
        
        ' Video info.
           lblinfo(8).Caption = "Video Driver: " & GetSysIni("boot.description", "display.drv")
        lblinfo(9).Caption = "Resolution: " & Screen.Width \ Screen.TwipsPerPixelX & " x " & Screen.Height \ Screen.TwipsPerPixelY
        lblinfo(10).Caption = "Colors: " & DeviceColors((hDC))
        
        ' General info.
           If GetSystemMetrics(SM_MOUSEPRESENT) Then
            lblinfo(11).Caption = "Mouse: " & GetSysIni("boot.description", "mouse.drv")
        Else
            lblinfo(11).Caption = "No mouse"
        End If
        lblinfo(12).Caption = "Network: " & GetSysIni("boot.description", "network.drv")
        lblinfo(13).Caption = "Language: " & GetSysIni("boot.description", "language.dll")
        lblinfo(14).Caption = "Keyboard: " & GetSysIni("boot.description", "keyboard.typ")
    End Sub
#End If
'end conditional compile

' begin conditional compile

#If Win32 Then                  '32-bit version of sub
    Private Sub Form_Load()
        Show                ' Make sure this form has an hWnd, etc.
        Load frmMenus
        Icon = imgIcon(1).Picture
        FillSysInfo
        frmMenus.mnuSysInfo(3).Visible = False
    End Sub

#Else                       '16-bit version of sub
    
    Private Sub Form_Load()
        Show                ' Make sure this form has an hWnd, etc.
        Load frmMenus
        Icon = imgIcon(1).Picture
        FillSysInfo
    End Sub
#End If
'end conditional compile

Private Sub Form_QueryUnload(Cancel As Integer, UnloadMode As Integer)
    End
End Sub

Private Sub Form_Resize()
Dim t As Integer, hDC As Integer
    If WindowState = MINIMIZED Then
        RightEdge = Screen.Width \ Screen.TwipsPerPixelX
        BottomEdge = Screen.Height \ Screen.TwipsPerPixelY
        If tmrBounce.Enabled Then
            hDC = GetDC(GetDesktopWindow())
            t = BitBlt(picCopy.hDC, 0, 0, PicWidth, PicHeight, hDC, X, Y, SRCCOPY)
            ReleaseDC GetDesktopWindow(), hDC
        End If
    Else
        ScaleMode = PIXELS
        RightEdge = ScaleWidth
        BottomEdge = ScaleHeight
        If tmrBounce.Enabled Then
            hDC = GetDC(GetDesktopWindow())
            t = BitBlt(hDC, X, Y, PicWidth, PicHeight, picCopy.hDC, 0, 0, SRCCOPY)
            ReleaseDC GetDesktopWindow(), hDC
        End If

    End If
End Sub

Private Sub ImgIcon_MouseDown(Index As Integer, Button As Integer, Shift As Integer, X As Single, Y As Single)
Dim IX  As Integer, IY As Integer
'hMenu and hSubMenu must be long to run on 32-bit
Dim hMenu As Long, hSubMenu As Long, R As Integer
Dim menRect As Rect
    If Button And 2 Then
        ScaleMode = TWIPS
        menRect.Left = 0
        menRect.Top = 0
        menRect.Right = Screen.Width / Screen.TwipsPerPixelX
        menRect.Bottom = Screen.Height / Screen.TwipsPerPixelY
        IX = (X + Left + imgIcon(Index).Left) \ Screen.TwipsPerPixelX
        IY = (Y + Top + imgIcon(Index).Top + imgIcon(Index).Height) \ Screen.TwipsPerPixelY
        hMenu = GetMenu(frmMenus.hWnd)
        hSubMenu = GetSubMenu(hMenu, Index)
        R = TrackPopupMenu(hSubMenu, 2, IX, IY, 0, frmMenus.hWnd, menRect)
    End If

    ' Refresh SysInfo
    If Index = 2 Then
        FillSysInfo
    End If
End Sub

Private Sub tmrBounce_Timer()
'Following are static only to improve speed
Static NewX As Integer, NewY As Integer, temp As Integer
Static hDC As Integer, releaseit As Integer

'Calculate new position
    ScaleMode = PIXELS
    temp = X + dx
    If temp + PicWidth \ 2 > RightEdge Then
        dx = -Abs(dx)
    ElseIf temp < 0 Then
        dx = Abs(dx)
    End If
    
    NewX = X + dx
    
    temp = Y + dy
    If temp + PicHeight \ 2 > BottomEdge Then
        dy = -Abs(dy)
    ElseIf temp < 0 Then
        dy = Abs(dy)
    End If
    
    NewY = Y + dy

    If WindowState = MINIMIZED Then
        hDC = GetDC(GetDesktopWindow())
        releaseit = True
    Else
        hDC = Me.hDC
        releaseit = False
    End If


'Now perform "transparent" BitBlts:
'1 Copy old background back over sprite's old position
'2 Copy the background where the sprite will go
'3 Draw the mask
'4 Draw the sprite
    temp = BitBlt(hDC, X, Y, PicWidth, PicHeight, picCopy.hDC, 0, 0, SRCCOPY)
    temp = BitBlt(picCopy.hDC, 0, 0, PicWidth, PicHeight, hDC, NewX, NewY, SRCCOPY)
    temp = BitBlt(hDC, NewX, NewY, PicWidth, PicHeight, picMask.hDC, 0, 0, SRCAND)
    temp = BitBlt(hDC, NewX, NewY, PicWidth, PicHeight, picSprite.hDC, 0, 0, SRCINVERT)
    X = NewX
    Y = NewY
    If releaseit Then ReleaseDC GetDesktopWindow(), hDC
End Sub

'begin conditional compile

#If Win32 Then      '32-bit version of sub

    Private Sub tmrSysInfo_Timer()
        If frainfo(RES_INFO).Visible Then
            Dim intX As Integer
            For intX = 1 To 2
                lblResInfo(intX).Visible = False
                shpBar(intX).Visible = False
                shpFrame(intX).Visible = False
            Next intX
        End If
        
        lblR(1).Visible = False
        lblR(2).Visible = False
        
        Dim YourMemory As MemoryStatus
        GlobalMemoryStatus YourMemory
        lblinfo(6).Caption = "Free Memory: " & YourMemory.dwMemoryLoad & "%"

    End Sub
    
#Else               'i6-bit version of sub

    Private Sub tmrSysInfo_Timer()
    Static Res(1 To 2) As Integer, OldFreeSpace As Currency
    Dim i As Integer, newVal As Integer, temp, FreeSpace As Currency
    
        ' Update resource info if visible.
        If frainfo(RES_INFO).Visible Then
            For i = 1 To 2
                newVal = GetFreeSystemResources(i)
                ' Reduce flashing by updating bar graphs and percentage
                ' display only if they've actually changed.
                If newVal <> Res(i) Then
                    Res(i) = newVal
                    lblResInfo(i).Caption = Res(i) & "%"
                    shpBar(i).Width = shpFrame(i).Width * Res(i) \ 100
                End If
            Next
            
            temp = GetFreeSpace(0)
            If Sgn(temp) = -1 Then
                ' Return of GetFreeSpace is an unsigned long
                ' so handle case when high bit is set (two's complement).
                FreeSpace = CLng(temp + 1&) Xor &HFFFF
            Else
                FreeSpace = temp
            End If
            If FreeSpace <> OldFreeSpace Then
                lblinfo(6).Caption = "Free memory space: " & Format(FreeSpace, "#,# \b\y\t\e\s")
                OldFreeSpace = FreeSpace
            End If
        End If
    End Sub

#End If
'end conditional compile

