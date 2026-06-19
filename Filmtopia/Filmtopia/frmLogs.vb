Imports System.Data.OleDb

Public Class frmLogs

    Private Sub frmLogs_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadLogs()
    End Sub

    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        LoadLogs()
    End Sub

    Private Sub LoadLogs()
        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT LogID, LogDateTime, LogType, LogMessage " &
                                 "FROM tblLogs " &
                                 "ORDER BY LogDateTime DESC"
            Dim rs As OleDbDataReader = SQLCmd.ExecuteReader()
            dgvLogs.Rows.Clear()
            dgvLogs.Columns.Clear()
            dgvLogs.Columns.Add("LogID", "ID")
            dgvLogs.Columns.Add("LogDateTime", "Date & Time")
            dgvLogs.Columns.Add("LogType", "Type")
            dgvLogs.Columns.Add("LogMessage", "Message")
            While rs.Read()
                dgvLogs.Rows.Add(rs("LogID"), rs("LogDateTime"), rs("LogType"), rs("LogMessage"))
            End While
            rs.Close()
            dgvLogs.Columns("LogMessage").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            dgvLogs.DefaultCellStyle.WrapMode = DataGridViewTriState.True
            dgvLogs.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
            cn.Close()
        End If
    End Sub

End Class