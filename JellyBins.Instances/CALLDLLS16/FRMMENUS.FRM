VERSION 2.00
Begin Form frmMenus 
   Caption         =   "Form2"
   Height          =   3450
   Left            =   1290
   LinkTopic       =   "Form2"
   ScaleHeight     =   2760
   ScaleWidth      =   5145
   Top             =   3720
   Width           =   5265
   Begin Menu mnuBar 
      Caption         =   "Sounds"
      Index           =   1
      Begin Menu mnuSounds 
         Caption         =   "(no sounds)"
         Enabled         =   0   'False
         Index           =   0
      End
   End
   Begin Menu mnuBar 
      Caption         =   "System Info"
      Index           =   2
      Begin Menu mnuSysInfo 
         Caption         =   "&Windows"
         Index           =   0
      End
      Begin Menu mnuSysInfo 
         Caption         =   "&CPU"
         Index           =   1
      End
      Begin Menu mnuSysInfo 
         Caption         =   "&Video"
         Index           =   2
      End
      Begin Menu mnuSysInfo 
         Caption         =   "&General"
         Index           =   3
      End
      Begin Menu mnuSysInfo 
         Caption         =   "-"
         Index           =   4
      End
      Begin Menu mnuSysInfo 
         Caption         =   "Always on top"
         Index           =   5
      End
   End
End
Option Explicit

Sub Form_Load ()
Dim WinPath As String, SoundFile As String, i As Integer
    WinPath = WindowsDirectory()
    SoundFile = Dir(WinPath & "\" & "*.wav")
    If WindowsVersion() > 3# Then
        If waveOutGetNumDevs() = 0 Then
            ' No wave output devices available.
            mnuSounds(0).Caption = "No Wave audio device available"
        ElseIf SoundFile = "" Then
            ' No sound files in Windows directory
            Exit Sub
        Else
            mnuSounds(0).Caption = Left(SoundFile, InStr(1, SoundFile, ".") - 1)
            mnuSounds(0).Enabled = True
            i = 1
            Do
                SoundFile = Dir
                If SoundFile = "" Then Exit Do
                Load mnuSounds(i)
                mnuSounds(i).Caption = Left(SoundFile, InStr(1, SoundFile, ".") - 1)
                i = i + 1
            Loop
        End If
    Else
        ' Wave audio and "always on top" only available in Windows 3.1+
        Unload mnuSysInfo(5)
        Unload mnuSysInfo(4)
    End If
End Sub

Sub mnuSounds_Click (Index As Integer)
Dim R As Integer
Const SYNC = 1
    R = sndPlaySound(ByVal CStr(WindowsDirectory() & "\" & mnuSounds(Index).Caption & ".wav"), SYNC)
End Sub

Sub mnuSysInfo_Click (Index As Integer)
    If Index <> 5 Then
        If VisibleFrame Is Nothing Then
            frmCallDlls!fraInfo(0).Visible = False
        Else
            VisibleFrame.Visible = False
        End If
        frmCallDlls!fraInfo(Index + 1).Visible = True
        Set VisibleFrame = frmCallDlls!fraInfo(Index + 1)
    Else
        mnuSysInfo(Index).Checked = Not mnuSysInfo(Index).Checked
        If mnuSysInfo(Index).Checked Then
            SetWindowPos frmCallDlls.hWnd, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOACTIVATE Or SWP_SHOWWINDOW
        Else
            SetWindowPos frmCallDlls.hWnd, HWND_NOTOPMOST, 0, 0, 0, 0, SWP_NOACTIVATE Or SWP_SHOWWINDOW
        End If
    End If
End Sub

