Imports System.Data.OleDb

Public Class frmScreenings
    Private selectedScreeningID As Long = 0
    Private selectedScreeningDetails As String = ""

    Private otherStarts(-1) As Integer
    Private otherEnds(-1) As Integer
    Private otherNames(-1) As String

    Private Sub frmScreenings_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If UserAccessLevel = AccessManager Then
            btnDelete.Visible = True
        Else
            btnDelete.Visible = False
        End If
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

    Private Function GetScreeningLength() As Integer
        Dim SQLCmd As New OleDbCommand("SELECT FilmDuration FROM tblFilm WHERE FilmID = @FilmID")
        SQLCmd.Parameters.AddWithValue("@FilmID", cboFilm.SelectedValue)
        Return GetValue(SQLCmd)
    End Function

    Private Sub LoadOtherScreenings(screeningDate As Date, screeningBeingChanged As Long)
        Dim SQLCmd As New OleDbCommand("SELECT ScreeningDate, ScreeningTime, FilmTitle, FilmDuration " &
                                       "FROM tblScreening INNER JOIN tblFilm ON tblScreening.FilmID = tblFilm.FilmID " &
                                       "WHERE ScreenID = @ScreenID AND ScreeningDate >= @DayBefore AND ScreeningDate <= @DayAfter " &
                                       "AND ScreeningID <> @ScreeningID")
        SQLCmd.Parameters.AddWithValue("@ScreenID", cboScreen.SelectedValue)
        SQLCmd.Parameters.AddWithValue("@DayBefore", screeningDate.AddDays(-1))
        SQLCmd.Parameters.AddWithValue("@DayAfter", screeningDate.AddDays(1))
        SQLCmd.Parameters.AddWithValue("@ScreeningID", screeningBeingChanged)
        Dim dt As New DataTable
        FillTable(SQLCmd, dt)

        ReDim otherStarts(dt.Rows.Count - 1)
        ReDim otherEnds(dt.Rows.Count - 1)
        ReDim otherNames(dt.Rows.Count - 1)
        For i As Integer = 0 To dt.Rows.Count - 1
            Dim otherDate As Date = dt.Rows(i)("ScreeningDate")
            Dim otherTime As String = dt.Rows(i)("ScreeningTime")
            Dim daysAway As Integer = DateDiff(DateInterval.Day, screeningDate, otherDate)

            otherStarts(i) = daysAway * MinutesInADay + TimeToMinutes(otherTime)
            otherEnds(i) = otherStarts(i) + dt.Rows(i)("FilmDuration")
            otherNames(i) = dt.Rows(i)("FilmTitle") & " on " & otherDate.ToString("dd/MM/yyyy") & " at " & otherTime & ", which has " & cboScreen.Text & " until " & MinutesToTime(otherEnds(i))
        Next
    End Sub

    Private Function FindClash(startTime As Integer, screeningLength As Integer) As String
        Dim clash As String = ""
        Dim endTime As Integer = startTime + screeningLength
        For i As Integer = 0 To otherStarts.Length - 1
            If clash = "" And startTime < otherEnds(i) And endTime > otherStarts(i) Then
                clash = otherNames(i)
            End If
        Next
        Return clash
    End Function

    Private Function ScreenIsFree(screeningBeingChanged As Long) As Boolean
        LoadOtherScreenings(dtpDate.Value.Date, screeningBeingChanged)
        Dim clash As String = FindClash(TimeToMinutes(txtTime.Text.Trim()), GetScreeningLength())

        Dim free As Boolean = True
        If clash <> "" Then
            MessageBox.Show("This screening would overlap the screening of " & clash & ".", "Screen Busy", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtTime.Focus()
            free = False
        End If
        Return free
    End Function

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        If DetailsAreValid() Then
            If ScreenIsFree(0) Then
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
        End If
    End Sub

    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        If selectedScreeningID = 0 Then
            MessageBox.Show("Please pick a screening from the list first.", "No Screening Picked", MessageBoxButtons.OK, MessageBoxIcon.Information)
        ElseIf DetailsAreValid() Then
            UpdateScreening()
        End If
    End Sub

    Private Sub UpdateScreening()
        Dim SQLCmd As New OleDbCommand("UPDATE tblScreening SET FilmID = @FilmID, ScreenID = @ScreenID, ScreeningDate = @ScreeningDate, ScreeningTime = @ScreeningTime, TicketPrice = @TicketPrice " &
                                       "WHERE ScreeningID = @ScreeningID")
        SQLCmd.Parameters.AddWithValue("@FilmID", cboFilm.SelectedValue)
        SQLCmd.Parameters.AddWithValue("@ScreenID", cboScreen.SelectedValue)
        SQLCmd.Parameters.AddWithValue("@ScreeningDate", dtpDate.Value.Date)
        SQLCmd.Parameters.AddWithValue("@ScreeningTime", txtTime.Text.Trim())
        SQLCmd.Parameters.AddWithValue("@TicketPrice", CDec(txtTicketPrice.Text.Trim()))
        SQLCmd.Parameters.AddWithValue("@ScreeningID", selectedScreeningID)
        If RunCommand(SQLCmd) Then
            WriteLog(LogScreening, "Updated screening number " & selectedScreeningID & " to " & cboFilm.Text & " in " & cboScreen.Text & " on " & dtpDate.Value.ToString("dd/MM/yyyy") & " at " & txtTime.Text.Trim())
            LoadScreenings()
            ClearFields()
        End If
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If selectedScreeningID = 0 Then
            MessageBox.Show("Please pick a screening from the list first.", "No Screening Picked", MessageBoxButtons.OK, MessageBoxIcon.Information)
        ElseIf MessageBox.Show("Delete the screening of " & selectedScreeningDetails & "? This cannot be undone.", "Delete Screening", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) = DialogResult.Yes Then
            Dim SQLCmd As New OleDbCommand("DELETE FROM tblScreening WHERE ScreeningID = @ScreeningID")
            SQLCmd.Parameters.AddWithValue("@ScreeningID", selectedScreeningID)
            If RunCommand(SQLCmd) Then
                WriteLog(LogScreening, "Deleted the screening of " & selectedScreeningDetails & " (screening number " & selectedScreeningID & ")")
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
            selectedScreeningDetails = row.Cells("FilmTitle").Value & " in " & row.Cells("ScreenName").Value & " on " & CDate(row.Cells("ScreeningDate").Value).ToString("dd/MM/yyyy") & " at " & row.Cells("ScreeningTime").Value
            cboFilm.SelectedValue = row.Cells("FilmID").Value
            cboScreen.SelectedValue = row.Cells("ScreenID").Value
            dtpDate.Value = row.Cells("ScreeningDate").Value
            txtTime.Text = row.Cells("ScreeningTime").Value
            txtTicketPrice.Text = CDec(row.Cells("TicketPrice").Value).ToString("0.00")
        End If
    End Sub

    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        ClearFields()
    End Sub

    Private Sub ClearFields()
        selectedScreeningID = 0
        selectedScreeningDetails = ""
        dgvScreenings.ClearSelection()
        cboFilm.SelectedIndex = -1
        cboScreen.SelectedIndex = -1
        dtpDate.Value = Date.Today
        txtTime.Text = ""
        txtTicketPrice.Text = ""
        cboFilm.Focus()
    End Sub
End Class
