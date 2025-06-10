VERSION 4.00
Begin VB.Form frmMenus 
   Caption         =   "Form2"
   ClientHeight    =   2715
   ClientLeft      =   1350
   ClientTop       =   4410
   ClientWidth     =   5175
   BeginProperty Font 
      name            =   "MS Sans Serif"
      charset         =   0
      weight          =   700
      size            =   8.25
      underline       =   0   'False
      italic          =   0   'False
      strikethrough   =   0   'False
   EndProperty
   Height          =   3405
   Left            =   1290
   LinkTopic       =   "Form2"
   ScaleHeight     =   2715
   ScaleWidth      =   5175
   Top             =   3780
   Width           =   5295
   Begin VB.Menu mnuBar 
      Caption         =   "Sounds"
      Index           =   1
      Begin VB.Menu mnuSounds 
         Caption         =   "(no sounds)"
         Enabled         =   0   'False
         Index           =   0
      End
   End
   Begin VB.Menu mnuBar 
      Caption         =   "System Info"
      Index           =   2
      Begin VB.Menu mnuSysInfo 
         Caption         =   "&Windows"
         Index           =   0
      End
      Begin VB.Menu mnuSysInfo 
         Caption         =   "&CPU"
         Index           =   1
      End
      Begin VB.Menu mnuSysInfo 
         Caption         =   "&Video"
         Index           =   2
      End
      Begin VB.Menu mnuSysInfo 
         Caption         =   "&General"
         Index           =   3
      End
      Begin VB.Menu mnuSysInfo 
         Caption         =   "-"
         Index           =   4
      End
      Begin VB.Menu mnuSysInfo 
         Caption         =   "Always on top"
         Index           =   5
      End
   End
End
Attribute VB_Name = "frmMenus"
Attribute VB_Creatable = False
Attribute VB_Exposed = False
Option Explicit

Private Sub Form_Load()
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

Private Sub mnuSounds_Click(Index As Integer)
Dim R As Integer
Const SYNC = 1
    R = sndPlaySound(ByVal CStr(WindowsDirectory() & "\" & mnuSounds(Index).Caption & ".wav"), SYNC)
End Sub

'begin conditional compile

#If Win32 Then                  '32-bit version of sub
    Private Sub mnuSysInfo_Click(Index As Integer)
        If Index <> 5 Then
            If VisibleFrame Is Nothing Then
                frmCallDlls.frainfo(0).Visible = False
            Else
                VisibleFrame.Visible = False
            End If
            frmCallDlls.frainfo(Index + 1).Visible = True
            Set VisibleFrame = frmCallDlls.frainfo(Index + 1)
        Else
            mnuSysInfo(Index).Checked = Not mnuSysInfo(Index).Checked
            
            If mnuSysInfo(Index).Checked Then
               SetWindowPos frmCallDlls.hWnd, HWND_TOPMOST, frmCallDlls.Left / 15, _
                            frmCallDlls.Top / 15, frmCallDlls.Width / 15, _
                            frmCallDlls.Height / 15, SWP_NOACTIVATE Or SWP_SHOWWINDOW
            Else
               SetWindowPos frmCallDlls.hWnd, HWND_NOTOPMOST, frmCallDlls.Left / 15, _
                            frmCallDlls.Top / 15, frmCallDlls.Width / 15, _
                            frmCallDlls.Height / 15, SWP_NOACTIVATE Or SWP_SHOWWINDOW
            End If
        End If
        If Index = 3 Then
            frmCallDlls.frainfo(4).Visible = False
        End If
    End Sub

#Else                           '16-bit version of sub

    Private Sub mnuSysInfo_Click(Index As Integer)
        If Index <> 5 Then
            If VisibleFrame Is Nothing Then
                frmCallDlls.frainfo(0).Visible = False
            Else
                VisibleFrame.Visible = False
            End If
            frmCallDlls.frainfo(Index + 1).Visible = True
            Set VisibleFrame = frmCallDlls.frainfo(Index + 1)
        Else
            mnuSysInfo(Index).Checked = Not mnuSysInfo(Index).Checked
            
            If mnuSysInfo(Index).Checked Then
               SetWindowPos frmCallDlls.hWnd, HWND_TOPMOST, frmCallDlls.Left / 15, _
                            frmCallDlls.Top / 15, frmCallDlls.Width / 15, _
                            frmCallDlls.Height / 15, SWP_NOACTIVATE Or SWP_SHOWWINDOW
            Else
               SetWindowPos frmCallDlls.hWnd, HWND_NOTOPMOST, frmCallDlls.Left / 15, _
                            frmCallDlls.Top / 15, frmCallDlls.Width / 15, _
                            frmCallDlls.Height / 15, SWP_NOACTIVATE Or SWP_SHOWWINDOW
            End If
        End If
    End Sub
#End If
'end conditional compile
