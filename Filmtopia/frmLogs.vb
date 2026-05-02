Imports System.Data.OleDb

Public Class frmLogs
    Private Sub frmLogs_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadLogs()
    End Sub

    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        LoadLogs()
    End Sub

    Private Sub LoadLogs()
        Dim SQLCmd As New OleDbCommand("SELECT LogDateTime, Username, LogType, LogMessage " &
                                       "FROM tblLogs LEFT JOIN tblLogin ON tblLogs.LoginID = tblLogin.LoginID " &
                                       "ORDER BY LogID DESC")
        Dim dt As New DataTable
        If FillTable(SQLCmd, dt) Then
            dgvLogs.DataSource = dt
            dgvLogs.Columns("LogDateTime").HeaderText = "Date and Time"
            dgvLogs.Columns("Username").HeaderText = "User"
            dgvLogs.Columns("LogType").HeaderText = "Type"
            dgvLogs.Columns("LogMessage").HeaderText = "Message"
            dgvLogs.Columns("LogDateTime").DefaultCellStyle.Format = "dd/MM/yyyy HH:mm:ss"
            dgvLogs.Columns("LogMessage").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        End If
    End Sub
End Class
