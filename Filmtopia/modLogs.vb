Imports System.Data.OleDb

Module modLogs
    Public Const LogAuth As String = "AUTH"
    Public Const LogFilm As String = "FILM"
    Public Const LogScreen As String = "SCREEN"
    Public Const LogScreening As String = "SCREENING"
    Public Const LogBooking As String = "BOOKING"

    Public Sub WriteLog(logType As String, message As String)
        Dim SQLCmd As New OleDbCommand("INSERT INTO tblLogs (LogDateTime, LoginID, LogType, LogMessage) VALUES (Now(), @LoginID, @LogType, @LogMessage)")
        If CurrentLoginID = 0 Then
            SQLCmd.Parameters.AddWithValue("@LoginID", DBNull.Value)
        Else
            SQLCmd.Parameters.AddWithValue("@LoginID", CurrentLoginID)
        End If
        SQLCmd.Parameters.AddWithValue("@LogType", logType)
        SQLCmd.Parameters.AddWithValue("@LogMessage", Left(message, 255))
        RunCommand(SQLCmd)
    End Sub
End Module
