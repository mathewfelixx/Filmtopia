Imports System.Data.OleDb

Public Class frmBookings
    Private Sub frmBookings_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadScreenings()
    End Sub

    Private Sub LoadScreenings()
        Dim SQLCmd As New OleDbCommand("SELECT ScreeningID, tblScreening.ScreenID, FilmDuration, FilmTitle, ScreenName, ScreeningDate, ScreeningTime, TicketPrice " &
                                       "FROM (tblScreening INNER JOIN tblFilm ON tblScreening.FilmID = tblFilm.FilmID) " &
                                       "INNER JOIN tblScreen ON tblScreening.ScreenID = tblScreen.ScreenID " &
                                       "WHERE ScreeningDate >= @Today " &
                                       "ORDER BY ScreeningDate, ScreeningTime, ScreenName")
        SQLCmd.Parameters.AddWithValue("@Today", Date.Today)
        Dim dt As New DataTable
        If FillTable(SQLCmd, dt) Then
            Dim timeNow As Integer = TimeToMinutes(Date.Now.ToString("HH:mm"))
            For i As Integer = dt.Rows.Count - 1 To 0 Step -1
                Dim screening As DataRow = dt.Rows(i)
                Dim filmEndTime As Integer = TimeToMinutes(screening("ScreeningTime")) + screening("FilmDuration")
                If screening("ScreeningDate") = Date.Today And filmEndTime <= timeNow Then
                    dt.Rows.Remove(screening)
                End If
            Next

            dgvScreenings.DataSource = dt
            dgvScreenings.Columns("ScreeningID").Visible = False
            dgvScreenings.Columns("ScreenID").Visible = False
            dgvScreenings.Columns("FilmDuration").Visible = False
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

    Private Sub frmBookings_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        dgvScreenings.ClearSelection()
    End Sub

    Private Sub dgvScreenings_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs) Handles dgvScreenings.DataBindingComplete
        dgvScreenings.ClearSelection()
    End Sub
End Class
