Imports System.Data.OleDb

Module modVersionControl
    Public appversion As String
    'function that returns the current appversion concat with the string before it
    Public Function GetVersion() As String
        Dim fullstring As String = "Filmtopia Cinema Management System " & appversion
        Return fullstring
    End Function

    'subroutine that loads the current appversion from the database
    Public Sub LoadVersion()
        If DbConnect() Then
            Dim query As String = "SELECT Version FROM tblVersionControl"
            Dim cmd As New OleDbCommand(query, cn)
            Dim result = cmd.ExecuteScalar()

            If result IsNot Nothing Then
                appversion = CStr(result)
            Else
                appversion = "v1.0.0 [VC ERROR]"
            End If

            cn.Close()
        End If
    End Sub

    'subroutine that saves the new appversion into the Filmtopia.mdb
    Public Sub SaveVersion()
        If DbConnect() Then
            Dim query As String = "UPDATE tblVersionControl SET Version = @version"
            Dim cmd As New OleDbCommand(query, cn)
            cmd.Parameters.AddWithValue("@version", appversion)
            cmd.ExecuteNonQuery()
            cn.Close()
        End If
    End Sub


    'subroutine that will contain all the different lblVersions from different forms.
    Public Sub UpdateAllVersionLabels()
        frmLogin.lblVersion.Text = GetVersion()
        frmMainForm.lblVersion.Text = GetVersion()
        frmVersionControlUTIL.lblVersion.Text = GetVersion()
    End Sub

End Module
