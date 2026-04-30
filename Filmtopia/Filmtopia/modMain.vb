Imports System.Data.OleDb
Imports System.Text
Module modMain
    Private Const DatabasePath As String = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source='Filmtopia.mdb';
                                           Persist Security Info=false;"
    Public cn As OleDbConnection
    Public LogedIn As Boolean = False
    Public UserAccessLevel As Integer = 99  ' indicates the prvilage level the user has 99 for customers
    Public Function DbConnect() As Boolean
        Try
            cn = New OleDbConnection(DatabasePath)
            cn.Open()
            '   MessageBox.Show("Opend Successfully ")
            Return True
        Catch ex As Exception
            MessageBox.Show("unable to open database " & ex.Message)
            Return False
        End Try
    End Function
    Public Function Encrypt(PlainText As String) As String
        'Function takes plain text concerts it into Unitext8 (UTF8) and uses VB built encrption function (basically it
        'XORS with key called UTF8) 
        ' it will return encrypted string called cipertext 

        Dim CipherText As String = PlainText

        If PlainText.Length = 0 Then
            MessageBox.Show("Empty PlainText can be encrypted")
        Else
            Dim dat As Byte() = New Byte(PlainText.Length - 1) {}
            dat = System.Text.Encoding.UTF8.GetBytes(PlainText)
            CipherText = Convert.ToBase64String(dat)
        End If
        Return CipherText
    End Function

    Public Function Decrypt(CipherText As String) As String
        Dim PlainText As String = CipherText

        If CipherText.Length = 0 Then
            MessageBox.Show("text to decrypt is empty")
        Else

            Dim strDecrypt As New System.Text.UTF8Encoding
            Dim UTF_Decrypt As System.Text.Decoder = strDecrypt.GetDecoder
            Dim uData As Byte() = Convert.FromBase64String(CipherText)
            Dim CharNum As Integer = UTF_Decrypt.GetCharCount(uData, 0, uData.Length) ' number of char in ciphertext
            Dim Decrypt_Char As Char() = New Char(CharNum - 1) {}
            UTF_Decrypt.GetChars(uData, 0, uData.Length, Decrypt_Char, 0)
            PlainText = New String(Decrypt_Char)
        End If

        Return PlainText
    End Function

    Public Sub CommonFormStartup()
        LoadVersion()
        UpdateAllVersionLabels()

    End Sub
End Module
