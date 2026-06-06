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

    Private Function DetailsAreValid(timeNeeded As Boolean) As Boolean
        Dim valid As Boolean = False
        Dim price As Decimal

        If cboFilm.SelectedIndex = -1 Then
            MessageBox.Show("Please choose a film.", "Missing Details", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            cboFilm.Focus()

        ElseIf cboScreen.SelectedIndex = -1 Then
            MessageBox.Show("Please choose a screen.", "Missing Details", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            cboScreen.Focus()

        ElseIf timeNeeded And Not IsValidTime(txtTime.Text.Trim()) Then
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

    Private Function SaveScreenings(screeningDates() As Date, screeningTimes() As String) As Boolean
        Dim commands(screeningDates.Length - 1) As OleDbCommand
        For i As Integer = 0 To screeningDates.Length - 1
            commands(i) = New OleDbCommand("INSERT INTO tblScreening (FilmID, ScreenID, ScreeningDate, ScreeningTime, TicketPrice) " &
                                           "VALUES (@FilmID, @ScreenID, @ScreeningDate, @ScreeningTime, @TicketPrice)")
            commands(i).Parameters.AddWithValue("@FilmID", cboFilm.SelectedValue)
            commands(i).Parameters.AddWithValue("@ScreenID", cboScreen.SelectedValue)
            commands(i).Parameters.AddWithValue("@ScreeningDate", screeningDates(i))
            commands(i).Parameters.AddWithValue("@ScreeningTime", screeningTimes(i))
            commands(i).Parameters.AddWithValue("@TicketPrice", CDec(txtTicketPrice.Text.Trim()))
        Next
        Return RunCommands(commands)
    End Function

    Private Sub btnSuggestTime_Click(sender As Object, e As EventArgs) Handles btnSuggestTime.Click
        If cboFilm.SelectedIndex = -1 Then
            MessageBox.Show("Please choose a film first.", "Missing Details", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            cboFilm.Focus()
        ElseIf cboScreen.SelectedIndex = -1 Then
            MessageBox.Show("Please choose a screen first.", "Missing Details", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            cboScreen.Focus()
        Else
            LoadOtherScreenings(dtpDate.Value.Date, selectedScreeningID)
            Dim screeningLength As Integer = GetScreeningLength()
            Dim startTime As Integer = RoundUpTime(TimeToMinutes(OpeningTime))
            Dim found As Boolean = False

            While Not found And startTime + screeningLength <= TimeToMinutes(ClosingTime)
                If FindClash(startTime, screeningLength) = "" Then
                    found = True
                Else
                    startTime = startTime + RoundingMinutes
                End If
            End While

            If found Then
                txtTime.Text = MinutesToTime(startTime)
            Else
                MessageBox.Show(cboFilm.Text & " does not fit in " & cboScreen.Text & " on " & dtpDate.Value.ToString("dd/MM/yyyy") & " before closing at " & ClosingTime & ".", "No Time Free", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        End If
    End Sub

    Private Sub btnFillDay_Click(sender As Object, e As EventArgs) Handles btnFillDay.Click
        If DetailsAreValid(False) Then
            FillDay()
        End If
    End Sub

    Private Sub FillDay()
        Dim screeningDate As Date = dtpDate.Value.Date
        Dim screeningLength As Integer = GetScreeningLength()
        Dim fillDates(-1) As Date
        Dim fillTimes(-1) As String
        Dim fillCount As Integer = 0
        Dim plannedTimes As String = ""

        LoadOtherScreenings(screeningDate, 0)
        Dim startTime As Integer = RoundUpTime(TimeToMinutes(OpeningTime))
        While startTime + screeningLength <= TimeToMinutes(ClosingTime)
            If FindClash(startTime, screeningLength) = "" Then
                ReDim Preserve fillDates(fillCount)
                ReDim Preserve fillTimes(fillCount)
                fillDates(fillCount) = screeningDate
                fillTimes(fillCount) = MinutesToTime(startTime)
                fillCount = fillCount + 1
                plannedTimes = plannedTimes & MinutesToTime(startTime) & " (screen free at " & MinutesToTime(startTime + screeningLength) & ")" & vbCrLf
                startTime = RoundUpTime(startTime + screeningLength)
            Else
                startTime = startTime + RoundingMinutes
            End If
        End While

        Dim question As String = "Add these " & fillCount & " screening(s) of " & cboFilm.Text & " in " & cboScreen.Text & " on " & screeningDate.ToString("dd/MM/yyyy") & "?" & vbCrLf & vbCrLf & plannedTimes

        If fillCount = 0 Then
            MessageBox.Show(cboFilm.Text & " does not fit in " & cboScreen.Text & " on " & screeningDate.ToString("dd/MM/yyyy") & " before closing at " & ClosingTime & ".", "No Time Free", MessageBoxButtons.OK, MessageBoxIcon.Information)
        ElseIf MessageBox.Show(question, "Fill Day", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            If SaveScreenings(fillDates, fillTimes) Then
                WriteLog(LogScreening, "Filled " & cboScreen.Text & " on " & screeningDate.ToString("dd/MM/yyyy") & " with " & fillCount & " screening(s) of " & cboFilm.Text)
                LoadScreenings()
                ClearFields()
            End If
        End If
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        If DetailsAreValid(True) Then
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

    Private Sub btnScheduleRun_Click(sender As Object, e As EventArgs) Handles btnScheduleRun.Click
        If DetailsAreValid(True) Then
            Dim dayCount As Integer = DateDiff(DateInterval.Day, dtpDate.Value.Date, dtpUntil.Value.Date) + 1
            If dtpUntil.Value.Date < dtpDate.Value.Date Then
                MessageBox.Show("The until date cannot be before the first date.", "Invalid Dates", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                dtpUntil.Focus()
            Else
                ScheduleRun(dayCount)
            End If
        End If
    End Sub

    Private Sub ScheduleRun(dayCount As Integer)
        Dim firstDate As Date = dtpDate.Value.Date
        Dim lastDate As Date = dtpUntil.Value.Date
        Dim screeningTime As String = txtTime.Text.Trim()
        Dim screeningLength As Integer = GetScreeningLength()
        Dim runDates(-1) As Date
        Dim runTimes(-1) As String
        Dim runCount As Integer = 0
        Dim plannedDays As String = ""
        Dim skippedDays As String = ""

        For dayNumber As Integer = 0 To dayCount - 1
            Dim screeningDate As Date = firstDate.AddDays(dayNumber)
            LoadOtherScreenings(screeningDate, 0)
            Dim clash As String = FindClash(TimeToMinutes(screeningTime), screeningLength)
            If clash = "" Then
                ReDim Preserve runDates(runCount)
                ReDim Preserve runTimes(runCount)
                runDates(runCount) = screeningDate
                runTimes(runCount) = screeningTime
                runCount = runCount + 1
                plannedDays = plannedDays & screeningDate.ToString("dd/MM/yyyy") & " at " & screeningTime & vbCrLf
            Else
                skippedDays = skippedDays & screeningDate.ToString("dd/MM/yyyy") & " (" & clash & ")" & vbCrLf
            End If
        Next

        Dim question As String = "Add these " & runCount & " screening(s) of " & cboFilm.Text & " in " & cboScreen.Text & "?" & vbCrLf & vbCrLf & plannedDays
        If skippedDays <> "" Then
            question = question & vbCrLf & "These days will be skipped because the screen is busy:" & vbCrLf & skippedDays
        End If

        If runCount = 0 Then
            MessageBox.Show("The screen is busy at " & screeningTime & " on every one of those days, so nothing can be added." & vbCrLf & vbCrLf & skippedDays, "Screen Busy", MessageBoxButtons.OK, MessageBoxIcon.Information)
        ElseIf MessageBox.Show(question, "Schedule Run", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            If SaveScreenings(runDates, runTimes) Then
                WriteLog(LogScreening, "Scheduled " & runCount & " screening(s) of " & cboFilm.Text & " in " & cboScreen.Text & " at " & screeningTime & " between " & firstDate.ToString("dd/MM/yyyy") & " and " & lastDate.ToString("dd/MM/yyyy"))
                LoadScreenings()
                ClearFields()
            End If
        End If
    End Sub

    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        If selectedScreeningID = 0 Then
            MessageBox.Show("Please pick a screening from the list first.", "No Screening Picked", MessageBoxButtons.OK, MessageBoxIcon.Information)
        ElseIf DetailsAreValid(True) Then
            If ScreenIsFree(selectedScreeningID) Then
                UpdateScreening()
            End If
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
        dtpUntil.Value = Date.Today
        txtTime.Text = ""
        txtTicketPrice.Text = ""
        cboFilm.Focus()
    End Sub
End Class
