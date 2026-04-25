Imports System.Data.OleDb

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
End Module
