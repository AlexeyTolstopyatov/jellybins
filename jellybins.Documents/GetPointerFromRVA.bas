'
' Got from Visual Basic 6 Decompiler
' Needs for exploring Portable Executabe's sections
' 

Public Function GetPtrFromRVA(ByVal iRVA As Integer) As Long
'*****************************
'Purpose: To get the real entrypoint used for VB5
'*****************************
      Dim num2 As Integer
      Dim num3 As Integer
      num3 = PEHeader.NumSections - 1
      num2 = 0
      Do While (num2 <= num3)
            If ((iRVA >= SecHeader(num2).Address) And (iRVA < (SecHeader(num2).Address + SecHeader(num2).SizeRawData))) Then
                  
                  GetPtrFromRVA = (iRVA - (SecHeader(num2).Address - SecHeader(num2).RawDataPointer))
            End If
            num2 = num2 + 1
      Loop
      GetPtrFromRVA = iRVA
End Function