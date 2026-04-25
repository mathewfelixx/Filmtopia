Imports System.Data.OleDb
Imports System.Text

Module modMain
    Private Const DatabasePath As String = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=Filmtopia.mdb;Persist Security Info=False;"

    Public cn As OleDbConnection

    Public Function DbConnect() As Boolean
        Try
            cn = New OleDbConnection(DatabasePath)
            cn.Open()
            Return True
        Catch ex As Exception
            MessageBox.Show("Unable to open the database. " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function

    Public Function Encode(plainText As String) As String
        Return Convert.ToBase64String(Encoding.UTF8.GetBytes(plainText))
    End Function

    Public Function Decode(encodedText As String) As String
        Return Encoding.UTF8.GetString(Convert.FromBase64String(encodedText))
    End Function
End Module
