Imports System.Data.OleDb

Public Class frmScreenings
    Private selectedScreeningID As Long = 0

    Private Sub frmScreenings_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadFilmList()
        LoadScreenList()
        LoadScreenings()
    End Sub

    Private Sub LoadFilmList()
        Dim SQLCmd As New OleDbCommand("SELECT FilmID, FilmTitle FROM tblFilm ORDER BY FilmTitle")
        Dim dt As New DataTable
        If FillTable(SQLCmd, dt) Then
            cboFilm.DisplayMember = "FilmTitle"
            cboFilm.ValueMember = "FilmID"
            cboFilm.DataSource = dt
            cboFilm.SelectedIndex = -1
        End If
    End Sub

    Private Sub LoadScreenList()
        Dim SQLCmd As New OleDbCommand("SELECT ScreenID, ScreenName FROM tblScreen ORDER BY ScreenName")
        Dim dt As New DataTable
        If FillTable(SQLCmd, dt) Then
            cboScreen.DisplayMember = "ScreenName"
            cboScreen.ValueMember = "ScreenID"
            cboScreen.DataSource = dt
            cboScreen.SelectedIndex = -1
        End If
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

    Private Function DetailsAreValid() As Boolean
        Dim valid As Boolean = False
        Dim price As Decimal

        If cboFilm.SelectedIndex = -1 Then
            MessageBox.Show("Please choose a film.", "Missing Details", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            cboFilm.Focus()

        ElseIf cboScreen.SelectedIndex = -1 Then
            MessageBox.Show("Please choose a screen.", "Missing Details", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            cboScreen.Focus()

        ElseIf Not IsValidTime(txtTime.Text.Trim()) Then
            MessageBox.Show("The time must be in HH:MM format, for example 19:30.", "Invalid Time", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtTime.Focus()

        ElseIf Not Decimal.TryParse(txtTicketPrice.Text.Trim(), price) Then
            MessageBox.Show("The ticket price must be a number, for example 8.50.", "Invalid Ticket Price", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtTicketPrice.Focus()

        Else
            valid = True
        End If

        Return valid
    End Function

    Private Function IsValidTime(timeText As String) As Boolean
        Dim valid As Boolean = False
        If timeText Like "##:##" Then
            Dim hours As Integer = CInt(timeText.Substring(0, 2))
            Dim minutes As Integer = CInt(timeText.Substring(3, 2))
            If hours <= 23 And minutes <= 59 Then
                valid = True
            End If
        End If
        Return valid
    End Function

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        If DetailsAreValid() Then
            Dim SQLCmd As New OleDbCommand("INSERT INTO tblScreening (FilmID, ScreenID, ScreeningDate, ScreeningTime, TicketPrice) " &
                                           "VALUES (@FilmID, @ScreenID, @ScreeningDate, @ScreeningTime, @TicketPrice)")
            SQLCmd.Parameters.AddWithValue("@FilmID", cboFilm.SelectedValue)
            SQLCmd.Parameters.AddWithValue("@ScreenID", cboScreen.SelectedValue)
            SQLCmd.Parameters.AddWithValue("@ScreeningDate", dtpDate.Value.Date)
            SQLCmd.Parameters.AddWithValue("@ScreeningTime", txtTime.Text.Trim())
            SQLCmd.Parameters.AddWithValue("@TicketPrice", CDec(txtTicketPrice.Text.Trim()))
            If RunCommand(SQLCmd) Then
                WriteLog(LogScreening, "Added screening of " & cboFilm.Text & " in " & cboScreen.Text & " on " & dtpDate.Value.ToString("dd/MM/yyyy") & " at " & txtTime.Text.Trim())
                LoadScreenings()
                ClearFields()
            End If
        End If
    End Sub

    Private Sub frmScreenings_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        dgvScreenings.ClearSelection()
    End Sub

    Private Sub dgvScreenings_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs) Handles dgvScreenings.DataBindingComplete
        dgvScreenings.ClearSelection()
    End Sub

    Private Sub dgvScreenings_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvScreenings.CellClick
        If e.RowIndex >= 0 Then
            Dim row As DataGridViewRow = dgvScreenings.Rows(e.RowIndex)
            selectedScreeningID = row.Cells("ScreeningID").Value
            cboFilm.SelectedValue = row.Cells("FilmID").Value
            cboScreen.SelectedValue = row.Cells("ScreenID").Value
            dtpDate.Value = row.Cells("ScreeningDate").Value
            txtTime.Text = row.Cells("ScreeningTime").Value
            txtTicketPrice.Text = CDec(row.Cells("TicketPrice").Value).ToString("0.00")
        End If
    End Sub

    Private Sub ClearFields()
        selectedScreeningID = 0
        dgvScreenings.ClearSelection()
        cboFilm.SelectedIndex = -1
        cboScreen.SelectedIndex = -1
        dtpDate.Value = Date.Today
        txtTime.Text = ""
        txtTicketPrice.Text = ""
        cboFilm.Focus()
    End Sub
End Class
