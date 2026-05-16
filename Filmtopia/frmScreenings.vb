Imports System.Data.OleDb

Public Class frmScreenings
    Private Sub frmScreenings_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadScreenings()
    End Sub

    Private Sub LoadScreenings()
        Dim SQLCmd As New OleDbCommand("SELECT ScreeningID, tblScreening.FilmID, tblScreening.ScreenID, FilmTitle, ScreenName, ScreeningDate, ScreeningTime, TicketPrice " &
                                       "FROM (tblScreening INNER JOIN tblFilm ON tblScreening.FilmID = tblFilm.FilmID) " &
                                       "INNER JOIN tblScreen ON tblScreening.ScreenID = tblScreen.ScreenID " &
                                       "ORDER BY ScreeningDate, ScreeningTime, ScreenName")
        Dim dt As New DataTable
        If FillTable(SQLCmd, dt) Then
            dgvScreenings.DataSource = dt
            dgvScreenings.Columns("ScreeningID").Visible = False
            dgvScreenings.Columns("FilmID").Visible = False
            dgvScreenings.Columns("ScreenID").Visible = False
            dgvScreenings.Columns("FilmTitle").HeaderText = "Film"
            dgvScreenings.Columns("ScreenName").HeaderText = "Screen"
            dgvScreenings.Columns("ScreeningDate").HeaderText = "Date"
            dgvScreenings.Columns("ScreeningTime").HeaderText = "Time"
            dgvScreenings.Columns("TicketPrice").HeaderText = "Ticket Price"
            dgvScreenings.Columns("FilmTitle").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            dgvScreenings.Columns("ScreeningDate").DefaultCellStyle.Format = "dd/MM/yyyy"
            dgvScreenings.Columns("TicketPrice").DefaultCellStyle.Format = "C2"
        End If
    End Sub
End Class
