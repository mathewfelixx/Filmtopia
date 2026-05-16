Imports System.Data.OleDb
Imports System.Text

Module modMain
    Private Const DatabasePath As String = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=Filmtopia.mdb;Persist Security Info=False;"
    Public Const AccessManager As Integer = 1

    Public cn As OleDbConnection
    Public UserAccessLevel As Integer = 0
    Public CurrentLoginID As Long = 0
    Public CurrentUsername As String = ""

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

    Public Function FillTable(SQLCmd As OleDbCommand, dt As DataTable) As Boolean
        Dim success As Boolean = False
        If DbConnect() Then
            SQLCmd.Connection = cn
            Dim da As New OleDbDataAdapter(SQLCmd)
            da.Fill(dt)
            success = True
            cn.Close()
        End If
        Return success
    End Function

    Public Function GetValue(SQLCmd As OleDbCommand) As Object
        Dim value As Object = Nothing
        If DbConnect() Then
            SQLCmd.Connection = cn
            value = SQLCmd.ExecuteScalar()
            cn.Close()
        End If
        Return value
    End Function

    Public Function RunCommand(SQLCmd As OleDbCommand) As Boolean
        Dim success As Boolean = False
        If DbConnect() Then
            SQLCmd.Connection = cn
            SQLCmd.ExecuteNonQuery()
            success = True
            cn.Close()
        End If
        Return success
    End Function

    Public Function RunCommands(commands() As OleDbCommand) As Boolean
        Dim success As Boolean = False
        If DbConnect() Then
            Dim trans As OleDbTransaction = cn.BeginTransaction()
            Try
                For i As Integer = 0 To commands.Length - 1
                    commands(i).Connection = cn
                    commands(i).Transaction = trans
                    commands(i).ExecuteNonQuery()
                Next
                trans.Commit()
                success = True
            Catch ex As Exception
                trans.Rollback()
                MessageBox.Show("This could not be saved to the database. " & ex.Message, "Save Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
            cn.Close()
        End If
        Return success
    End Function

    Public Function Encode(plainText As String) As String
        Return Convert.ToBase64String(Encoding.UTF8.GetBytes(plainText))
    End Function

    Public Function Decode(encodedText As String) As String
        Return Encoding.UTF8.GetString(Convert.FromBase64String(encodedText))
    End Function
End Module
