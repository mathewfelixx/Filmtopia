Imports System.Data.OleDb

Module modLogs

    Public Const LogInfo As String = "INFO"
    Public Const LogChange As String = "CHANGE"
    Public Const LogWarning As String = "WARNING"
    Public Const LogSecurity As String = "SECURITY"

    Public Sub WriteLog(logType As String, message As String, Optional severity As String = LogInfo)
        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "INSERT INTO tblLogs (LogDateTime, LogType, LogMessage, LogUser, LogSeverity) " &
                                 "VALUES (Now(), @Type, @Message, @User, @Severity)"
            SQLCmd.Parameters.AddWithValue("@Type", logType)
            SQLCmd.Parameters.AddWithValue("@Message", message)
            SQLCmd.Parameters.AddWithValue("@User", CurrentLogUser())
            SQLCmd.Parameters.AddWithValue("@Severity", severity)
            SQLCmd.ExecuteNonQuery()
            cn.Close()
        End If
    End Sub

    Public Function CurrentLogUser() As String
        If Not LogedIn Then
            Return "system"
        End If

        If frmLogin.globalusername = "" Then
            Return "system"
        End If

        Return frmLogin.globalusername
    End Function

End Module
