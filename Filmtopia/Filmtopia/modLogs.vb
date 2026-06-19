Imports System.Data.OleDb

Module modLogs
    Public Sub WriteLog(logType As String, message As String)
        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "INSERT INTO tblLogs (LogDateTime, LogType, LogMessage) " &
                                 "VALUES (Now(), @Type, @Message)"
            SQLCmd.Parameters.AddWithValue("@Type", logType)
            SQLCmd.Parameters.AddWithValue("@Message", message)
            SQLCmd.ExecuteNonQuery()
            cn.Close()
        End If
    End Sub
End Module
