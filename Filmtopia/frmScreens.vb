Imports System.Data.OleDb

Public Class frmScreens
    Private Sub frmScreens_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadScreens()
    End Sub

    Private Sub LoadScreens()
        Dim SQLCmd As New OleDbCommand("SELECT ScreenID, ScreenName, ScreenCapacity " &
                                       "FROM tblScreen ORDER BY ScreenName")
        Dim dt As New DataTable
        If FillTable(SQLCmd, dt) Then
            dgvScreens.DataSource = dt
            dgvScreens.Columns("ScreenID").Visible = False
            dgvScreens.Columns("ScreenName").HeaderText = "Name"
            dgvScreens.Columns("ScreenCapacity").HeaderText = "Seats"
            dgvScreens.Columns("ScreenName").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        End If
    End Sub
End Class
