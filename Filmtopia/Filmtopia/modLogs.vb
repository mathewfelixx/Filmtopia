Imports System.Data.OleDb

Module modLogs

    'the four levels a log entry can be. they are only ever used as text so they are kept here as
    'constants, that way a typo is a compile error instead of a row that never shows up in a filter.
    'INFO is the routine stuff, CHANGE is anything that altered the data, WARNING is something that
    'failed or was refused, SECURITY is logging in and out and anything to do with accounts
    Public Const LogInfo As String = "INFO"
    Public Const LogChange As String = "CHANGE"
    Public Const LogWarning As String = "WARNING"
    Public Const LogSecurity As String = "SECURITY"

    'writes one line into tblLogs. the severity is optional so all the older calls still work and
    'just come out as INFO. the user is not passed in, it is picked up automatically, because an
    'audit trail is no use if whoever wrote the call can decide to leave the name off
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

    'whoever is signed in at the moment. things like the login screen itself and the startup form
    'run before anybody has signed in, so those come out as system
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
