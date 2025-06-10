VERSION 2.00
Begin Form frmCallDlls 
   BorderStyle     =   1  'Fixed Single
   Caption         =   "Calling DLL Procedures"
   ClipControls    =   0   'False
   Height          =   2310
   Left            =   900
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   ScaleHeight     =   1905
   ScaleWidth      =   5505
   Top             =   1080
   Width           =   5625
   Begin PictureBox picSprite 
      AutoRedraw      =   -1  'True
      AutoSize        =   -1  'True
      Height          =   510
      Left            =   960
      Picture         =   CALLDLLS.FRX:0000
      ScaleHeight     =   32
      ScaleMode       =   3  'Pixel
      ScaleWidth      =   32
      TabIndex        =   25
      Top             =   1920
      Visible         =   0   'False
      Width           =   510
   End
   Begin PictureBox picCopy 
      AutoRedraw      =   -1  'True
      BorderStyle     =   0  'None
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
   Begin PictureBox picMask 
      AutoRedraw      =   -1  'True
      AutoSize        =   -1  'True
      Height          =   510
      Left            =   240
      Picture         =   CALLDLLS.FRX:0302
      ScaleHeight     =   32
      ScaleMode       =   3  'Pixel
      ScaleWidth      =   32
      TabIndex        =   23
      Top             =   1920
      Visible         =   0   'False
      Width           =   510
   End
   Begin CommandButton cmdBitBlt 
      Caption         =   "BitBlt"
      Height          =   495
      Left            =   4680
      TabIndex        =   22
      Top             =   1320
      Width           =   735
   End
   Begin Frame fraInfo 
      Caption         =   "Instructions"
      ClipControls    =   0   'False
      Height          =   1695
      Index           =   0
      Left            =   120
      TabIndex        =   0
      Top             =   120
      Width           =   4455
      Begin Label lblInfo 
         Caption         =   "Click the right mouse button on the icons to the right."
         Height          =   495
         Index           =   1
         Left            =   840
         TabIndex        =   1
         Top             =   480
         Width           =   2415
      End
   End
   Begin Timer tmrBounce 
      Enabled         =   0   'False
      Interval        =   1
      Left            =   4080
      Top             =   1920
   End
   Begin Frame fraInfo 
      Caption         =   "General Info"
      ClipControls    =   0   'False
      Height          =   1695
      Index           =   4
      Left            =   120
      TabIndex        =   13
      Top             =   120
      Visible         =   0   'False
      Width           =   4455
      Begin Label lblInfo 
         Caption         =   "Keyboard:"
         Height          =   435
         Index           =   14
         Left            =   120
         TabIndex        =   17
         Top             =   1080
         Width           =   4230
      End
      Begin Label lblInfo 
         AutoSize        =   -1  'True
         Caption         =   "Language:"
         Height          =   195
         Index           =   13
         Left            =   120
         TabIndex        =   16
         Top             =   840
         Width           =   915
      End
      Begin Label lblInfo 
         AutoSize        =   -1  'True
         Caption         =   "Mouse:"
         Height          =   195
         Index           =   12
         Left            =   120
         TabIndex        =   15
         Top             =   360
         Width           =   630
      End
      Begin Label lblInfo 
         AutoSize        =   -1  'True
         Caption         =   "Network:"
         Height          =   195
         Index           =   11
         Left            =   120
         TabIndex        =   14
         Top             =   600
         Width           =   780
      End
   End
   Begin Frame fraInfo 
      Caption         =   "Operating System"
      ClipControls    =   0   'False
      Height          =   1695
      Index           =   1
      Left            =   120
      TabIndex        =   2
      Top             =   120
      Visible         =   0   'False
      Width           =   4455
      Begin Label lblInfo 
         AutoSize        =   -1  'True
         Caption         =   "(Enhanced mode)"
         Height          =   195
         Index           =   3
         Left            =   360
         TabIndex        =   8
         Top             =   600
         Width           =   1500
      End
      Begin Label lblInfo 
         AutoSize        =   -1  'True
         Caption         =   "Disk Operating System 5.0"
         Height          =   195
         Index           =   4
         Left            =   240
         TabIndex        =   4
         Top             =   960
         Width           =   2265
      End
      Begin Label lblInfo 
         AutoSize        =   -1  'True
         Caption         =   "Microsoft Windows Version 3.1"
         Height          =   195
         Index           =   2
         Left            =   240
         TabIndex        =   3
         Top             =   360
         Width           =   2640
      End
   End
   Begin Frame fraInfo 
      Caption         =   "Processor, Memory, and System Resources"
      ClipControls    =   0   'False
      Height          =   1695
      Index           =   2
      Left            =   120
      TabIndex        =   5
      Top             =   120
      Visible         =   0   'False
      Width           =   4455
      Begin Timer tmrSysInfo 
         Interval        =   1
         Left            =   3840
         Top             =   240
      End
      Begin Shape shpFrame 
         Height          =   255
         Index           =   1
         Left            =   1080
         Top             =   960
         Width           =   3135
      End
      Begin Shape shpBar 
         BackStyle       =   1  'Opaque
         DrawMode        =   7  'Xor Pen
         Height          =   255
         Index           =   1
         Left            =   1080
         Top             =   960
         Width           =   1695
      End
      Begin Shape shpFrame 
         Height          =   255
         Index           =   2
         Left            =   1080
         Top             =   1320
         Width           =   3135
      End
      Begin Shape shpBar 
         BackStyle       =   1  'Opaque
         DrawMode        =   7  'Xor Pen
         Height          =   255
         Index           =   2
         Left            =   1080
         Top             =   1320
         Width           =   1695
      End
      Begin Label lblResInfo 
         Alignment       =   2  'Center
         Caption         =   "user"
         Height          =   255
         Index           =   2
         Left            =   1080
         TabIndex        =   21
         Top             =   1320
         Width           =   3135
      End
      Begin Label lblResInfo 
         Alignment       =   2  'Center
         Caption         =   "gdi"
         Height          =   255
         Index           =   1
         Left            =   1080
         TabIndex        =   20
         Top             =   960
         Width           =   3135
      End
      Begin Label lblR 
         Caption         =   "GDI"
         Height          =   255
         Index           =   1
         Left            =   240
         TabIndex        =   19
         Top             =   960
         Width           =   855
      End
      Begin Label lblR 
         Caption         =   "User"
         Height          =   255
         Index           =   2
         Left            =   240
         TabIndex        =   18
         Top             =   1320
         Width           =   855
      End
      Begin Label lblInfo 
         AutoSize        =   -1  'True
         Caption         =   "CPU: 486 (with Math Coprocessor)"
         Height          =   195
         Index           =   5
         Left            =   240
         TabIndex        =   7
         Top             =   360
         Width           =   2940
      End
      Begin Label lblInfo 
         AutoSize        =   -1  'True
         Caption         =   "Memory Free"
         Height          =   195
         Index           =   6
         Left            =   240
         TabIndex        =   6
         Top             =   600
         Width           =   1095
      End
   End
   Begin Frame fraInfo 
      Caption         =   "Video"
      ClipControls    =   0   'False
      Height          =   1695
      Index           =   3
      Left            =   120
      TabIndex        =   9
      Top             =   120
      Visible         =   0   'False
      Width           =   4455
      Begin Label lblInfo 
         AutoSize        =   -1  'True
         Caption         =   "Colors:"
         Height          =   195
         Index           =   10
         Left            =   240
         TabIndex        =   12
         Top             =   1320
         Width           =   600
      End
      Begin Label lblInfo 
         AutoSize        =   -1  'True
         Caption         =   "Resolution"
         Height          =   195
         Index           =   9
         Left            =   240
         TabIndex        =   11
         Top             =   960
         Width           =   915
      End
      Begin Label lblInfo 
         Caption         =   "Video Driver:"
         Height          =   495
         Index           =   8
         Left            =   240
         TabIndex        =   10
         Top             =   360
         Width           =   3975
      End
   End
   Begin Image ImgIcon 
      Height          =   480
      Index           =   1
      Left            =   4800
      Picture         =   CALLDLLS.FRX:0604
      Top             =   720
      Width           =   480
   End
   Begin Image ImgIcon 
      Height          =   480
      Index           =   0
      Left            =   4800
      Picture         =   CALLDLLS.FRX:0906
      Top             =   120
      Width           =   480
   End
End
Option Explicit
Dim dx As Integer, dy As Integer, X As Integer, Y As Integer
Dim PicWidth As Integer, PicHeight As Integer
Dim RightEdge As Integer, BottomEdge As Integer

Sub cmdBitBlt_Click ()
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

Sub FillSysInfo ()
Dim WinFlags As Long, FreeSpace As Currency, FreeBlock As Currency, temp

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

Sub Form_Load ()

    Show                ' Make sure this form has an hWnd, etc.
    Load frmMenus
    Icon = imgIcon(1).Picture

    FillSysInfo
End Sub

Sub Form_QueryUnload (Cancel As Integer, UnloadMode As Integer)
    End
End Sub

Sub Form_Resize ()
Dim t As Integer, hDC As Integer
    If WindowState = MINIMIZED Then
	RightEdge = Screen.Width \ Screen.TwipsPerPixelX
	BottomEdge = Screen.Height \ Screen.TwipsPerPixelY
	If tmrBounce.Enabled Then
	    hDC = GetDC(GetDeskTopWindow())
	    t = BitBlt(picCopy.hDC, 0, 0, PicWidth, PicHeight, hDC, X, Y, SRCCOPY)
	    ReleaseDC GetDeskTopWindow(), hDC
	End If
    Else
	ScaleMode = PIXELS
	RightEdge = ScaleWidth
	BottomEdge = ScaleHeight
	If tmrBounce.Enabled Then
	    hDC = GetDC(GetDeskTopWindow())
	    t = BitBlt(hDC, X, Y, PicWidth, PicHeight, picCopy.hDC, 0, 0, SRCCOPY)
	    ReleaseDC GetDeskTopWindow(), hDC
	End If

    End If
End Sub

Sub ImgIcon_MouseDown (Index As Integer, Button As Integer, Shift As Integer, X As Single, Y As Single)
Dim IX  As Integer, IY As Integer
Dim hMenu As Integer, hSubMenu As Integer, R As Integer
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

Sub tmrBounce_Timer ()
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
	hDC = GetDC(GetDeskTopWindow())
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
    If releaseit Then ReleaseDC GetDeskTopWindow(), hDC
End Sub

Sub tmrSysInfo_Timer ()
Static Res(1 To 2) As Integer, OldFreeSpace As Currency
Dim i As Integer, newVal As Integer, temp, FreeSpace As Currency

    ' Update resource info if visible.
    If fraInfo(RES_INFO).Visible Then
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
	    FreeSpace = CLng(temp + 1&) Xor &HFFFFFFFF
	Else
	    FreeSpace = temp
	End If
	If FreeSpace <> OldFreeSpace Then
	    lblinfo(6).Caption = "Free memory space: " & Format(FreeSpace, "#,# \b\y\t\e\s")
	    OldFreeSpace = FreeSpace
	End If
    End If
End Sub

