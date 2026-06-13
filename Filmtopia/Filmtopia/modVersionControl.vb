Imports System.Data.OleDb



'##############################################################
'this is a finished feature (theres nothing much more to add)
'##############################################################











Module modVersionControl
    Public appversion As String
    'function that returns the current appversion concat with the string before it
    Public Function GetVersion() As String
        Dim fullstring As String = "Filmtopia Cinema Management System v" & appversion
        Return fullstring
    End Function

    'subroutine that loads the current appversion from the database
    Public Sub LoadVersion()
        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT Version FROM tblVersionControl"
            Dim result = SQLCmd.ExecuteScalar()
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
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "UPDATE tblVersionControl SET Version = @Version"
            SQLCmd.Parameters.AddWithValue("@Version", appversion)
            SQLCmd.ExecuteNonQuery()
            cn.Close()
        End If
    End Sub

    'subroutine that will contain all the different lblVersions from different forms.

    Public Sub UpdateAllVersionLabels()
        frmLogin.lblVersion.Text = GetVersion()
        frmVersionControlUTIL.lblVersion.Text = GetVersion()
        frmMainForm.lblVersion.Text = GetVersion()
        frmFilms.lblVersion.Text = GetVersion()

    End Sub


End Module
