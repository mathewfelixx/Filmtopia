Imports System.Data.OleDb

Public Class frmScreenings

    Private selectedScreeningID As Integer = 0

    Private screenWhenPicked As Integer = 0

    Private stillLoading As Boolean = True

    Private boxesChanged As Boolean = False

    Private fillingBoxes As Boolean = False

    Private rowBoldFont As New Font("Segoe UI", 9, FontStyle.Bold)

    Private Sub frmScreenings_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CommonFormStartup(Me)

        LoadFilmsCombo()
        LoadScreensCombo()

        LoadScreenFilterCombo()
        LoadTimelineScreens()

        cboShow.Items.Add("Today")
        cboShow.Items.Add("This week")
        cboShow.Items.Add("Still to come")
        cboShow.Items.Add("Already been on")
        cboShow.Items.Add("Everything")

        cboShow.SelectedIndex = cboShow.Items.IndexOf(LastScreeningsShow)
        If cboShow.SelectedIndex = -1 Then
            cboShow.SelectedIndex = cboShow.Items.IndexOf("Still to come")
        End If

        If (cboShow.Text = "Today" Or cboShow.Text = "This week" Or cboShow.Text = "Still to come") And UpcomingCount() = 0 Then
            cboShow.SelectedIndex = cboShow.Items.IndexOf("Everything")
        End If

        cboScreenFilter.SelectedIndex = cboScreenFilter.FindStringExact(LastScreeningsScreen)
        If cboScreenFilter.SelectedIndex = -1 Then
            cboScreenFilter.SelectedIndex = 0
        End If

        stillLoading = False

        Me.KeyPreview = True

        LoadScreenings()
        ClearFields()
        cboFilm.Focus()
        WriteLog("SCREENING", "Screenings form opened")
    End Sub

    Private Sub frmScreenings_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If cboShow.Text = "" Then
            Exit Sub
        End If

        LastScreeningsShow = cboShow.Text
        LastScreeningsScreen = cboScreenFilter.Text
        SaveUserSettings()
    End Sub

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        If ExportGridToCsv(dgvScreenings, "Screenings.csv", "Screenings") Then
            WriteLog("SCREENING", "Screening list exported, " & dgvScreenings.Rows.Count & " screenings")
        End If
    End Sub

    Private Sub frmScreenings_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.F5 Then
            LoadScreenings()
            RefreshTimelineIfShowing()
        ElseIf e.KeyCode = Keys.Escape Then
            If txtSearch.Text <> "" Then
                txtSearch.Clear()
            Else
                Me.Close()
            End If
        End If
    End Sub

    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        If stillLoading Then
            Exit Sub
        End If

        timerSearch.Stop()
        timerSearch.Start()
    End Sub

    Private Sub timerSearch_Tick(sender As Object, e As EventArgs) Handles timerSearch.Tick
        timerSearch.Stop()
        LoadScreenings()
    End Sub

    Private Sub cboScreenFilter_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboScreenFilter.SelectedIndexChanged
        If stillLoading Then
            Exit Sub
        End If

        LoadScreenings()
    End Sub

    Private Function UpcomingCount() As Integer
        Dim total As Integer = 0

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT COUNT(*) FROM tblScreening WHERE ScreeningDate >= @Today"
            SQLCmd.Parameters.AddWithValue("@Today", Date.Today)
            total = CInt(SQLCmd.ExecuteScalar())
            cn.Close()
        End If

        Return total
    End Function

    Private Sub LoadFilmsCombo()
        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT FilmID, FilmTitle " &
                                 "FROM tblFilm ORDER BY FilmTitle"
            Dim da As New OleDbDataAdapter(SQLCmd)
            Dim dt As New DataTable
            da.Fill(dt)
            cboFilm.DataSource = dt
            cboFilm.DisplayMember = "FilmTitle"
            cboFilm.ValueMember = "FilmID"
            cboFilm.SelectedIndex = -1
            cn.Close()
        End If
    End Sub

    Private Sub LoadScreensCombo()
        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT ScreenID, ScreenName " &
                                 "FROM tblScreen ORDER BY ScreenName"
            Dim da As New OleDbDataAdapter(SQLCmd)
            Dim dt As New DataTable
            da.Fill(dt)
            cboScreen.DataSource = dt
            cboScreen.DisplayMember = "ScreenName"
            cboScreen.ValueMember = "ScreenID"
            cboScreen.SelectedIndex = -1
            cn.Close()
        End If
    End Sub

    Private Sub LoadScreenFilterCombo()
        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT ScreenID, ScreenName " &
                                 "FROM tblScreen ORDER BY ScreenName"
            Dim da As New OleDbDataAdapter(SQLCmd)
            Dim dt As New DataTable
            da.Fill(dt)
            cn.Close()

            Dim allRow As DataRow = dt.NewRow()
            allRow("ScreenID") = 0
            allRow("ScreenName") = "All screens"
            dt.Rows.InsertAt(allRow, 0)

            cboScreenFilter.DataSource = dt
            cboScreenFilter.DisplayMember = "ScreenName"
            cboScreenFilter.ValueMember = "ScreenID"
            cboScreenFilter.SelectedIndex = 0
        End If
    End Sub

    Private Sub LoadScreenings()
        Dim dt As New DataTable

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn

            Dim baseQuery As String = "SELECT tblScreening.ScreeningID, FilmTitle, ScreenName, ScreeningDate, ScreeningTime, TicketPrice, " &
                                      "FilmDuration, ScreenCapacity, tblScreening.FilmID, tblScreening.ScreenID, ScreeningStatus, " &
                                      "tblScreening.TrailerMinutes, tblScreening.TurnaroundMinutes, " &
                                      "(SELECT COUNT(*) FROM tblBookingSeat AS bs WHERE bs.ScreeningID = tblScreening.ScreeningID) AS SeatsBooked " &
                                      "FROM (tblScreening INNER JOIN tblFilm ON tblScreening.FilmID = tblFilm.FilmID) " &
                                      "INNER JOIN tblScreen ON tblScreening.ScreenID = tblScreen.ScreenID"

            Dim conditions As String = ""
            Dim newestFirst As Boolean = False

            If cboShow.Text = "Today" Then
                conditions = "ScreeningDate = @Today"
            ElseIf cboShow.Text = "This week" Then
                conditions = "ScreeningDate >= @FromDate AND ScreeningDate <= @ToDate"
            ElseIf cboShow.Text = "Still to come" Then
                conditions = "ScreeningDate >= @Today"
            ElseIf cboShow.Text = "Already been on" Then
                conditions = "ScreeningDate < @Today"
                newestFirst = True
            End If

            If cboScreenFilter.SelectedIndex > 0 Then
                If conditions <> "" Then
                    conditions = conditions & " AND "
                End If
                conditions = conditions & "tblScreening.ScreenID = @ScreenID"
            End If

            If txtSearch.Text.Trim() <> "" Then
                If conditions <> "" Then
                    conditions = conditions & " AND "
                End If
                conditions = conditions & "(FilmTitle LIKE @Search OR ScreenName LIKE @Search2)"
            End If

            Dim ordering As String = " ORDER BY ScreeningDate, ScreeningTime, ScreeningID"
            If newestFirst Then
                ordering = " ORDER BY ScreeningDate DESC, ScreeningTime, ScreeningID"
            End If

            If conditions = "" Then
                SQLCmd.CommandText = baseQuery & ordering
            Else
                SQLCmd.CommandText = baseQuery & " WHERE " & conditions & ordering
            End If

            If cboShow.Text = "This week" Then
                SQLCmd.Parameters.AddWithValue("@FromDate", Date.Today)
                SQLCmd.Parameters.AddWithValue("@ToDate", Date.Today.AddDays(6))
            ElseIf cboShow.Text <> "Everything" Then
                SQLCmd.Parameters.AddWithValue("@Today", Date.Today)
            End If

            If cboScreenFilter.SelectedIndex > 0 Then
                SQLCmd.Parameters.AddWithValue("@ScreenID", CInt(cboScreenFilter.SelectedValue))
            End If

            If txtSearch.Text.Trim() <> "" Then
                SQLCmd.Parameters.AddWithValue("@Search", "%" & txtSearch.Text.Trim() & "%")
                SQLCmd.Parameters.AddWithValue("@Search2", "%" & txtSearch.Text.Trim() & "%")
            End If

            Dim da As New OleDbDataAdapter(SQLCmd)
            da.Fill(dt)
            cn.Close()
        End If

        dt.Columns.Add("SoldText", GetType(String))
        dt.Columns.Add("PercentFull", GetType(Integer))
        dt.Columns.Add("EndsAt", GetType(String))

        For Each row As DataRow In dt.Rows
            Dim sold As Integer = CInt(row("SeatsBooked"))
            Dim capacity As Integer = CInt(row("ScreenCapacity"))

            row("SoldText") = sold & " of " & capacity

            If capacity > 0 Then
                row("PercentFull") = CInt(sold * 100 / capacity)
            Else
                row("PercentFull") = 0
            End If

            row("EndsAt") = EndTimeText(row("ScreeningTime").ToString(), CInt(row("FilmDuration")),
                                        StoredMinutesOr(row("TrailerMinutes"), TrailerMinutes))
        Next

        dgvScreenings.DataSource = dt

        If dgvScreenings.Columns.Contains("ScreeningID") Then
            dgvScreenings.Columns("FilmID").Visible = False
            dgvScreenings.Columns("ScreenID").Visible = False
            dgvScreenings.Columns("FilmDuration").Visible = False
            dgvScreenings.Columns("ScreenCapacity").Visible = False
            dgvScreenings.Columns("SeatsBooked").Visible = False
            dgvScreenings.Columns("ScreeningStatus").Visible = False

            dgvScreenings.Columns("ScreeningID").HeaderText = "ID"
            dgvScreenings.Columns("FilmTitle").HeaderText = "Film"
            dgvScreenings.Columns("ScreenName").HeaderText = "Screen"
            dgvScreenings.Columns("ScreeningDate").HeaderText = "Date"
            dgvScreenings.Columns("ScreeningTime").HeaderText = "Starts"
            dgvScreenings.Columns("EndsAt").HeaderText = "Ends"
            dgvScreenings.Columns("TicketPrice").HeaderText = "Ticket"
            dgvScreenings.Columns("SoldText").HeaderText = "Seats sold"
            dgvScreenings.Columns("PercentFull").HeaderText = "Full"

            dgvScreenings.Columns("ScreeningID").Width = 50
            dgvScreenings.Columns("FilmTitle").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            dgvScreenings.Columns("ScreenName").Width = 110
            dgvScreenings.Columns("ScreeningDate").Width = 110
            dgvScreenings.Columns("ScreeningTime").Width = 70
            dgvScreenings.Columns("EndsAt").Width = 70
            dgvScreenings.Columns("TicketPrice").Width = 80
            dgvScreenings.Columns("SoldText").Width = 100
            dgvScreenings.Columns("PercentFull").Width = 60

            dgvScreenings.Columns("ScreeningID").DisplayIndex = 0
            dgvScreenings.Columns("FilmTitle").DisplayIndex = 1
            dgvScreenings.Columns("ScreenName").DisplayIndex = 2
            dgvScreenings.Columns("ScreeningDate").DisplayIndex = 3
            dgvScreenings.Columns("ScreeningTime").DisplayIndex = 4
            dgvScreenings.Columns("EndsAt").DisplayIndex = 5
            dgvScreenings.Columns("TicketPrice").DisplayIndex = 6
            dgvScreenings.Columns("SoldText").DisplayIndex = 7
            dgvScreenings.Columns("PercentFull").DisplayIndex = 8

            dgvScreenings.Columns("ScreeningDate").DefaultCellStyle.Format = "dd/MM/yyyy"
            dgvScreenings.Columns("TicketPrice").DefaultCellStyle.Format = "C"
            dgvScreenings.Columns("PercentFull").DefaultCellStyle.Format = "0'%'"
            dgvScreenings.Columns("ScreeningTime").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            dgvScreenings.Columns("EndsAt").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            dgvScreenings.Columns("SoldText").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            dgvScreenings.Columns("PercentFull").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        End If

        ShowCount(dt)
        MarkTheGrid()
        dgvScreenings.ClearSelection()
    End Sub

    Private Sub MarkTheGrid()
        MarkSoldOutScreenings()
        ColourOccupancy()
        GreyOutCancelled()
    End Sub

    Private Sub GreyOutCancelled()
        For Each row As DataGridViewRow In dgvScreenings.Rows
            If row.Cells("ScreeningStatus").Value IsNot Nothing AndAlso
               Not IsDBNull(row.Cells("ScreeningStatus").Value) AndAlso
               row.Cells("ScreeningStatus").Value.ToString() = ScreeningCancelled Then

                row.DefaultCellStyle.BackColor = Color.Empty
                row.DefaultCellStyle.ForeColor = PastFore
                row.Cells("PercentFull").Style.ForeColor = PastFore
                row.Cells("PercentFull").Style.SelectionForeColor = PastFore
                row.Cells("PercentFull").Style.Font = Nothing
                row.Cells("SoldText").Value = "Cancelled"
            End If
        Next
    End Sub

    Private Sub dgvScreenings_Sorted(sender As Object, e As EventArgs) Handles dgvScreenings.Sorted
        MarkTheGrid()
    End Sub

    Private Sub ColourOccupancy()
        For Each row As DataGridViewRow In dgvScreenings.Rows
            Dim cell As DataGridViewCell = row.Cells("PercentFull")
            Dim percent As Integer = CInt(cell.Value)

            If percent >= 80 Then
                cell.Style.ForeColor = OccupancyHigh
                cell.Style.SelectionForeColor = OccupancyHigh
                cell.Style.Font = rowBoldFont
            ElseIf percent >= 50 Then
                cell.Style.ForeColor = OccupancyMed
                cell.Style.SelectionForeColor = OccupancyMed
                cell.Style.Font = Nothing
            Else
                cell.Style.ForeColor = Color.Empty
                cell.Style.SelectionForeColor = Color.Empty
                cell.Style.Font = Nothing
            End If
        Next
    End Sub

    Private Sub MarkSoldOutScreenings()
        For Each row As DataGridViewRow In dgvScreenings.Rows
            Dim capacity As Integer = CInt(row.Cells("ScreenCapacity").Value)
            Dim sold As Integer = CInt(row.Cells("SeatsBooked").Value)

            If capacity > 0 And sold >= capacity Then
                If DarkModeOn Then
                    row.DefaultCellStyle.BackColor = Color.FromArgb(24, 60, 34)
                    row.DefaultCellStyle.ForeColor = Color.White
                Else
                    row.DefaultCellStyle.BackColor = Color.FromArgb(232, 245, 233)
                    row.DefaultCellStyle.ForeColor = Color.FromArgb(27, 94, 32)
                End If
                row.Cells("SoldText").Value = "SOLD OUT"
            Else
                row.DefaultCellStyle.BackColor = Color.Empty
                row.DefaultCellStyle.ForeColor = Color.Empty
            End If
        Next
    End Sub

    Private Function SeatsSold(screeningID As Integer) As Integer
        Dim total As Integer = 0

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT COUNT(*) FROM tblBookingSeat " &
                                 "WHERE ScreeningID = @ScreeningID"
            SQLCmd.Parameters.AddWithValue("@ScreeningID", screeningID)
            total = CInt(SQLCmd.ExecuteScalar())
            cn.Close()
        End If

        Return total
    End Function

    Private Function BookingsOnScreening(screeningID As Integer) As Integer
        Dim total As Integer = 0

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT COUNT(*) FROM tblBooking " &
                                 "WHERE ScreeningID = @ScreeningID"
            SQLCmd.Parameters.AddWithValue("@ScreeningID", screeningID)
            total = CInt(SQLCmd.ExecuteScalar())
            cn.Close()
        End If

        Return total
    End Function

    Private Sub ShowCount(dt As DataTable)
        If dt.Rows.Count = 0 Then
            lblGridCount.Text = "Nothing to show"
            Exit Sub
        End If

        Dim sold As Integer = 0
        For Each row As DataRow In dt.Rows
            sold = sold + CInt(row("SeatsBooked"))
        Next

        lblGridCount.Text = dt.Rows.Count & " screening(s), " & sold & " seat(s) sold between them"
    End Sub

    Private Sub LoadTimelineScreens()
        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT ScreenID, ScreenName FROM tblScreen ORDER BY ScreenName"
            Dim da As New OleDbDataAdapter(SQLCmd)
            Dim dt As New DataTable
            da.Fill(dt)
            cn.Close()

            cboTimelineScreen.DisplayMember = "ScreenName"
            cboTimelineScreen.ValueMember = "ScreenID"
            cboTimelineScreen.DataSource = dt
        End If
    End Sub

    Private Sub LoadTimelineDay()
        If cboTimelineScreen.SelectedValue Is Nothing Then
            Exit Sub
        End If

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT tblScreening.ScreeningID, FilmTitle, ScreeningTime, FilmDuration, ScreeningStatus, " &
                                 "tblScreening.TrailerMinutes, tblScreening.TurnaroundMinutes, " &
                                 "(SELECT COUNT(*) FROM tblBookingSeat AS bs WHERE bs.ScreeningID = tblScreening.ScreeningID) AS SeatsBooked " &
                                 "FROM tblScreening INNER JOIN tblFilm ON tblScreening.FilmID = tblFilm.FilmID " &
                                 "WHERE ScreeningDate = @ScreeningDate AND tblScreening.ScreenID = @ScreenID " &
                                 "ORDER BY ScreeningTime, tblScreening.ScreeningID"
            SQLCmd.Parameters.AddWithValue("@ScreeningDate", dtpTimelineDate.Value.Date)
            SQLCmd.Parameters.AddWithValue("@ScreenID", CInt(cboTimelineScreen.SelectedValue))
            Dim da As New OleDbDataAdapter(SQLCmd)
            Dim dt As New DataTable
            da.Fill(dt)
            cn.Close()

            dt.Columns.Add("EndsAt", GetType(String))
            dt.Columns.Add("SoldText", GetType(String))

            For Each row As DataRow In dt.Rows
                Dim duration As Integer = 0

                If Not IsDBNull(row("FilmDuration")) Then
                    duration = CInt(row("FilmDuration"))
                End If

                row("EndsAt") = EndTimeText(row("ScreeningTime").ToString(), duration,
                                            StoredMinutesOr(row("TrailerMinutes"), TrailerMinutes))
                row("SoldText") = row("SeatsBooked").ToString()
            Next

            dgvTimeline.DataSource = Nothing
            dgvTimeline.Columns.Clear()
            dgvTimeline.DataSource = dt

            ShowTimelineColumns()
            GreyOutCancelledOnDay()
            dgvTimeline.ClearSelection()

            lblTimelineCount.Text = dt.Rows.Count & " showing(s) on " & Format(dtpTimelineDate.Value.Date, "dd/MM/yyyy")
        End If
    End Sub

    Private Sub ShowTimelineColumns()
        If Not dgvTimeline.Columns.Contains("ScreeningID") Then
            Exit Sub
        End If

        dgvTimeline.Columns("ScreeningID").Visible = False
        dgvTimeline.Columns("FilmDuration").Visible = False
        dgvTimeline.Columns("ScreeningStatus").Visible = False
        dgvTimeline.Columns("SeatsBooked").Visible = False

        dgvTimeline.Columns("ScreeningTime").HeaderText = "Starts"
        dgvTimeline.Columns("FilmTitle").HeaderText = "Film"
        dgvTimeline.Columns("EndsAt").HeaderText = "Ends"
        dgvTimeline.Columns("SoldText").HeaderText = "Seats sold"

        dgvTimeline.Columns("ScreeningTime").Width = 70
        dgvTimeline.Columns("FilmTitle").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        dgvTimeline.Columns("EndsAt").Width = 70
        dgvTimeline.Columns("SoldText").Width = 90

        dgvTimeline.Columns("ScreeningTime").DisplayIndex = 0
        dgvTimeline.Columns("FilmTitle").DisplayIndex = 1
        dgvTimeline.Columns("EndsAt").DisplayIndex = 2
        dgvTimeline.Columns("SoldText").DisplayIndex = 3

        dgvTimeline.Columns("ScreeningTime").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        dgvTimeline.Columns("EndsAt").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        dgvTimeline.Columns("SoldText").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
    End Sub

    Private Sub GreyOutCancelledOnDay()
        For Each row As DataGridViewRow In dgvTimeline.Rows
            If row.Cells("ScreeningStatus").Value IsNot Nothing AndAlso
               Not IsDBNull(row.Cells("ScreeningStatus").Value) AndAlso
               row.Cells("ScreeningStatus").Value.ToString() = ScreeningCancelled Then

                row.DefaultCellStyle.ForeColor = PastFore
                row.DefaultCellStyle.SelectionForeColor = PastFore
                row.Cells("SoldText").Value = "Cancelled"
            End If
        Next
    End Sub

    Private Sub dgvTimeline_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvTimeline.CellClick
        If e.RowIndex < 0 Then
            Exit Sub
        End If

        If Not ChangesCanBeLost() Then
            Exit Sub
        End If

        LoadScreeningIntoBoxes(CInt(dgvTimeline.Rows(e.RowIndex).Cells("ScreeningID").Value))
    End Sub

    Private Sub cboTimelineScreen_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboTimelineScreen.SelectedIndexChanged
        If stillLoading Then
            Exit Sub
        End If

        LoadTimelineDay()
    End Sub

    Private Sub dtpTimelineDate_ValueChanged(sender As Object, e As EventArgs) Handles dtpTimelineDate.ValueChanged
        If stillLoading Then
            Exit Sub
        End If

        LoadTimelineDay()
    End Sub

    Private Sub tabView_SelectedIndexChanged(sender As Object, e As EventArgs) Handles tabView.SelectedIndexChanged
        If stillLoading Then
            Exit Sub
        End If

        If tabView.SelectedTab Is tabTimeline Then
            LoadTimelineDay()
        End If
    End Sub

    Private Sub RefreshTimelineIfShowing()
        If tabView.SelectedTab Is tabTimeline Then
            LoadTimelineDay()
        End If
    End Sub

    Private Function TimeAsMinutes(timeText As String) As Integer
        If Not IsValidScreeningTime(timeText) Then
            Return -1
        End If

        Return (CInt(timeText.Substring(0, 2)) * 60) + CInt(timeText.Substring(3, 2))
    End Function

    Private Function MinutesAsTime(minutes As Integer) As String
        Dim wrapped As Integer = minutes Mod (24 * 60)

        Return Format(wrapped \ 60, "00") & ":" & Format(wrapped Mod 60, "00")
    End Function

    Private Function ScreenTimeNeeded(duration As Integer) As Integer
        Return TrailerMinutes + duration + TurnaroundMinutes
    End Function

    Private Function ScreenTimeUsedBy(duration As Integer, trailerMins As Integer, turnaroundMins As Integer) As Integer
        Return trailerMins + duration + turnaroundMins
    End Function

    Private Function StoredMinutesOr(value As Object, fallback As Integer) As Integer
        If IsDBNull(value) Then
            Return fallback
        End If

        Return CInt(value)
    End Function

    Private Function EndTimeText(startTime As String, duration As Integer, trailerMins As Integer) As String
        Dim startMinutes As Integer = TimeAsMinutes(startTime)

        If startMinutes < 0 Then
            Return ""
        End If

        Return MinutesAsTime(startMinutes + trailerMins + duration)
    End Function

    Private Function IsValidScreeningTime(timeText As String) As Boolean
        If timeText.Length <> 5 Then
            Return False
        End If
        If timeText.Chars(2) <> ":" Then
            Return False
        End If

        Dim hourPart As String = timeText.Substring(0, 2)
        Dim minutePart As String = timeText.Substring(3, 2)

        If Not IsNumeric(hourPart) Or Not IsNumeric(minutePart) Then
            Return False
        End If

        Dim hour As Integer = CInt(hourPart)
        Dim minute As Integer = CInt(minutePart)

        If hour < 0 Or hour > 23 Or minute < 0 Or minute > 59 Then
            Return False
        End If

        Return True
    End Function

    Private Function DurationOfPickedFilm() As Integer
        If cboFilm.SelectedIndex = -1 Then
            Return 0
        End If

        Dim minutes As Integer = 0

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT FilmDuration FROM tblFilm WHERE FilmID = @FilmID"
            SQLCmd.Parameters.AddWithValue("@FilmID", CInt(cboFilm.SelectedValue))
            Dim result = SQLCmd.ExecuteScalar()
            If result IsNot Nothing AndAlso Not IsDBNull(result) Then
                minutes = CInt(result)
            End If
            cn.Close()
        End If

        Return minutes
    End Function

    Private Sub ShowEndTime()
        If cboFilm.SelectedIndex = -1 Then
            lblEndsAt.Text = "Pick a film and a start time to see when the screen would be free again"
            Exit Sub
        End If

        Dim startMinutes As Integer = TimeAsMinutes(txtScreeningTime.Text)

        If startMinutes < 0 Then
            lblEndsAt.Text = "Start time needs to be in HH:MM, like 14:30"
            Exit Sub
        End If

        Dim duration As Integer = DurationOfPickedFilm()

        lblEndsAt.Text = TrailerMinutes & " minutes of adverts, then " & cboFilm.Text & " runs for " &
                         duration & " minutes, so the film starts at " &
                         MinutesAsTime(startMinutes + TrailerMinutes) & " and is out at " &
                         MinutesAsTime(startMinutes + TrailerMinutes + duration) & "." & vbCrLf &
                         "With " & TurnaroundMinutes & " minutes to clear up, the screen is free again at " &
                         MinutesAsTime(startMinutes + ScreenTimeNeeded(duration)) & "."
    End Sub

    Private Sub ScreeningDetails_Changed(sender As Object, e As EventArgs) Handles cboFilm.SelectedIndexChanged,
        txtScreeningTime.TextChanged

        If stillLoading Then
            Exit Sub
        End If

        ShowEndTime()
    End Sub

    Private Sub cboShow_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboShow.SelectedIndexChanged
        If stillLoading Then
            Exit Sub
        End If

        LoadScreenings()
    End Sub

    Private Sub LoadDayOccupancy(screenID As Integer, theDate As Date, excludeID As Integer,
                                 ByRef starts() As Integer, ByRef finishes() As Integer,
                                 ByRef titles() As String, ByRef howMany As Integer)
        howMany = 0
        ReDim starts(0)
        ReDim finishes(0)
        ReDim titles(0)

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT tblScreening.ScreeningID, FilmTitle, ScreeningTime, FilmDuration, " &
                                 "tblScreening.TrailerMinutes, tblScreening.TurnaroundMinutes " &
                                 "FROM tblScreening INNER JOIN tblFilm ON tblScreening.FilmID = tblFilm.FilmID " &
                                 "WHERE tblScreening.ScreenID = @ScreenID AND ScreeningDate = @ScreeningDate " &
                                 "AND (ScreeningStatus IS NULL OR ScreeningStatus <> 'Cancelled')"
            SQLCmd.Parameters.AddWithValue("@ScreenID", screenID)
            SQLCmd.Parameters.AddWithValue("@ScreeningDate", theDate)
            Dim da As New OleDbDataAdapter(SQLCmd)
            Dim dt As New DataTable
            da.Fill(dt)
            cn.Close()

            ReDim starts(dt.Rows.Count)
            ReDim finishes(dt.Rows.Count)
            ReDim titles(dt.Rows.Count)

            For Each row As DataRow In dt.Rows
                If CInt(row("ScreeningID")) <> excludeID Then
                    Dim thisStart As Integer = TimeAsMinutes(row("ScreeningTime").ToString())

                    If thisStart >= 0 Then
                        starts(howMany) = thisStart
                        finishes(howMany) = thisStart + ScreenTimeUsedBy(CInt(row("FilmDuration")),
                                                                        StoredMinutesOr(row("TrailerMinutes"), TrailerMinutes),
                                                                        StoredMinutesOr(row("TurnaroundMinutes"), TurnaroundMinutes))
                        titles(howMany) = row("FilmTitle").ToString()
                        howMany = howMany + 1
                    End If
                End If
            Next
        End If
    End Sub

    Private Sub SortDayByStart(ByRef starts() As Integer, ByRef finishes() As Integer,
                               ByRef titles() As String, howMany As Integer)
        For i As Integer = 1 To howMany - 1
            Dim keyStart As Integer = starts(i)
            Dim keyFinish As Integer = finishes(i)
            Dim keyTitle As String = titles(i)
            Dim j As Integer = i - 1

            While j >= 0 AndAlso starts(j) > keyStart
                starts(j + 1) = starts(j)
                finishes(j + 1) = finishes(j)
                titles(j + 1) = titles(j)
                j = j - 1
            End While

            starts(j + 1) = keyStart
            finishes(j + 1) = keyFinish
            titles(j + 1) = keyTitle
        Next
    End Sub

    Private Function FirstFitFrom(after As Integer, needed As Integer, starts() As Integer,
                                  finishes() As Integer, howMany As Integer) As Integer
        Dim earliest As Integer = after

        If earliest < FirstShowMinutes Then
            earliest = FirstShowMinutes
        End If

        For i As Integer = 0 To howMany - 1
            If earliest + needed <= starts(i) And earliest <= LastShowMinutes Then
                Return earliest
            End If

            If finishes(i) > earliest Then
                earliest = finishes(i)
            End If
        Next

        If earliest <= LastShowMinutes Then
            Return earliest
        End If

        Return -1
    End Function

    Private Function NextFreeSlot() As Integer
        Dim needed As Integer = ScreenTimeNeeded(DurationOfPickedFilm())

        Dim starts(0) As Integer
        Dim finishes(0) As Integer
        Dim titles(0) As String
        Dim howMany As Integer

        LoadDayOccupancy(CInt(cboScreen.SelectedValue), dtpScreeningDate.Value.Date, selectedScreeningID,
                         starts, finishes, titles, howMany)
        SortDayByStart(starts, finishes, titles, howMany)

        Return FirstFitFrom(FirstShowMinutes, needed, starts, finishes, howMany)
    End Function

    Private Function CanWorkOutTimes() As Boolean
        If cboFilm.SelectedIndex = -1 Then
            MessageBox.Show("Pick a film first, otherwise there is no way to know how long it needs", "Screenings", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If

        If cboScreen.SelectedIndex = -1 Then
            MessageBox.Show("Pick a screen first", "Screenings", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If

        If Not ScreenIsInService(CLng(cboScreen.SelectedValue)) Then
            MessageBox.Show("'" & cboScreen.Text & "' is out of service at the moment, so nothing can be " &
                            "scheduled in it." & vbCrLf & vbCrLf &
                            "Put it back in service on the screens form, or pick a different screen.",
                            "Screen is out of service", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If

        Return True
    End Function

    Private Sub btnSuggest_Click(sender As Object, e As EventArgs) Handles btnSuggest.Click
        If Not CanWorkOutTimes() Then
            Exit Sub
        End If

        Dim slot As Integer = NextFreeSlot()

        If slot < 0 Then
            MessageBox.Show("There is no room left for " & cboFilm.Text & " on that screen that day." & vbCrLf &
                            "Try another screen or another date.",
                            "Nothing free", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        txtScreeningTime.Text = MinutesAsTime(slot)
        ShowEndTime()
        WriteLog("SCREENING", "Suggested " & MinutesAsTime(slot) & " for " & cboFilm.Text &
                              " on ScreenID " & cboScreen.SelectedValue.ToString())
    End Sub

    Private Function ClashingScreening() As String
        Dim startMinutes As Integer = TimeAsMinutes(txtScreeningTime.Text)
        Dim endMinutes As Integer = startMinutes + ScreenTimeNeeded(DurationOfPickedFilm())

        Dim starts(0) As Integer
        Dim finishes(0) As Integer
        Dim titles(0) As String
        Dim howMany As Integer

        LoadDayOccupancy(CInt(cboScreen.SelectedValue), dtpScreeningDate.Value.Date, selectedScreeningID,
                         starts, finishes, titles, howMany)
        SortDayByStart(starts, finishes, titles, howMany)

        For i As Integer = 0 To howMany - 1
            If startMinutes < finishes(i) And starts(i) < endMinutes Then
                Return titles(i) & " at " & MinutesAsTime(starts(i)) &
                       ", which is not finished and cleared until " & MinutesAsTime(finishes(i))
            End If
        Next

        Return ""
    End Function

    Private Sub CountWhatCancellingHits(screeningID As Integer, ByRef bookings As Integer, ByRef seats As Integer)
        bookings = BookingsOnScreening(screeningID)
        seats = SeatsSold(screeningID)
    End Sub

    Private Sub btnCancelScreening_Click(sender As Object, e As EventArgs) Handles btnCancelScreening.Click
        If selectedScreeningID = 0 Then
            MessageBox.Show("Pick the screening to cancel from the list first", "Screenings", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If Not ScreeningIsOn(selectedScreeningID) Then
            MessageBox.Show("That screening has already been cancelled", "Screenings", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        If txtCancelReason.Text.Trim() = "" Then
            MessageBox.Show("Say why it is being pulled. It goes on the screening and into the log, and " &
                            "somebody will want to know later why a film that was advertised did not go on.",
                            "Give a reason", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtCancelReason.Focus()
            Exit Sub
        End If

        Dim bookings As Integer
        Dim seats As Integer
        CountWhatCancellingHits(selectedScreeningID, bookings, seats)

        Dim damage As String = "Nobody has booked onto it."

        If bookings > 0 Then
            damage = bookings & " booking(s) and " & seats & " seat(s) will be cancelled." & vbCrLf &
                     "Customers are not told automatically, somebody has to ring them."
        End If

        If MessageBox.Show("Cancel " & cboFilm.Text & " in " & cboScreen.Text & " on " &
                           dtpScreeningDate.Value.ToString("dd/MM/yyyy") & " at " & txtScreeningTime.Text & "?" & vbCrLf & vbCrLf &
                           damage & vbCrLf & vbCrLf & "This cannot be undone.",
                           "Cancel a screening", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                           MessageBoxDefaultButton.Button2) = DialogResult.No Then
            Exit Sub
        End If

        Dim cancelledIt As Boolean = False

        If DbConnect() Then
            Dim trans As OleDbTransaction = cn.BeginTransaction()
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.Transaction = trans

            Try
                SQLCmd.CommandText = "UPDATE tblScreening SET ScreeningStatus = @Status, " &
                                     "ScreeningCancelReason = @Reason, ScreeningCancelDate = Now() " &
                                     "WHERE ScreeningID = @ScreeningID"
                SQLCmd.Parameters.AddWithValue("@Status", ScreeningCancelled)
                SQLCmd.Parameters.AddWithValue("@Reason", txtCancelReason.Text.Trim())
                SQLCmd.Parameters.AddWithValue("@ScreeningID", selectedScreeningID)
                SQLCmd.ExecuteNonQuery()

                SQLCmd.Parameters.Clear()
                SQLCmd.CommandText = "UPDATE tblBooking SET BookingStatus = @BookingStatus, CancelledDate = Now() " &
                                     "WHERE ScreeningID = @ScreeningID AND BookingStatus <> @AlreadyCancelled"
                SQLCmd.Parameters.AddWithValue("@BookingStatus", BookingCancelled)
                SQLCmd.Parameters.AddWithValue("@ScreeningID", selectedScreeningID)
                SQLCmd.Parameters.AddWithValue("@AlreadyCancelled", BookingCancelled)
                SQLCmd.ExecuteNonQuery()

                SQLCmd.Parameters.Clear()
                SQLCmd.CommandText = "DELETE FROM tblBookingSeat WHERE ScreeningID = @ScreeningID"
                SQLCmd.Parameters.AddWithValue("@ScreeningID", selectedScreeningID)
                SQLCmd.ExecuteNonQuery()

                trans.Commit()
                cancelledIt = True
            Catch ex As Exception
                trans.Rollback()
                MessageBox.Show("Nothing was changed, it was all put back." & vbCrLf & vbCrLf & ex.Message,
                                "Could not cancel it", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try

            cn.Close()
        End If

        If Not cancelledIt Then
            Exit Sub
        End If

        Dim savedName As String = cboFilm.Text

        WriteLog("SCREENING", "Screening " & selectedScreeningID & " cancelled: " & txtCancelReason.Text.Trim() &
                              ", " & bookings & " booking(s) and " & seats & " seat(s) affected", LogWarning)
        txtCancelReason.Text = ""
        LoadScreenings()
        RefreshTimelineIfShowing()
        ClearFields()
        SayDone(lblSaved, "Cancelled the '" & savedName & "' screening")
    End Sub

    Private Sub InsertScreening(SQLCmd As OleDbCommand, filmID As Integer, screenID As Integer,
                                theDate As Date, timeText As String, price As Double)
        SQLCmd.Parameters.Clear()
        SQLCmd.CommandText = "INSERT INTO tblScreening (FilmID, ScreenID, ScreeningDate, ScreeningTime, TicketPrice, TrailerMinutes, TurnaroundMinutes) " &
                             "VALUES (@FilmID, @ScreenID, @ScreeningDate, @ScreeningTime, @TicketPrice, @TrailerMinutes, @TurnaroundMinutes)"
        SQLCmd.Parameters.AddWithValue("@FilmID", filmID)
        SQLCmd.Parameters.AddWithValue("@ScreenID", screenID)
        SQLCmd.Parameters.AddWithValue("@ScreeningDate", theDate)
        SQLCmd.Parameters.AddWithValue("@ScreeningTime", timeText)
        SQLCmd.Parameters.AddWithValue("@TicketPrice", price)
        SQLCmd.Parameters.AddWithValue("@TrailerMinutes", TrailerMinutes)
        SQLCmd.Parameters.AddWithValue("@TurnaroundMinutes", TurnaroundMinutes)
        SQLCmd.ExecuteNonQuery()
    End Sub

    Private Function TimeAndPriceAreOk(needTime As Boolean) As Boolean
        If needTime Then
            If txtScreeningTime.Text.Trim() = "" Then
                MessageBox.Show("Enter a screening time (HH:MM)", "Screenings", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtScreeningTime.Focus()
                Return False
            End If

            If Not IsValidScreeningTime(txtScreeningTime.Text) Then
                MessageBox.Show("Screening time must be in HH:MM format, e.g. 14:30", "Screenings", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtScreeningTime.Focus()
                Return False
            End If
        End If

        If Not IsNumeric(txtTicketPrice.Text) Or Val(txtTicketPrice.Text) <= 0 Or Val(txtTicketPrice.Text) > 50 Then
            MessageBox.Show("Enter a ticket price in pounds, more than 0 and no more than 50", "Screenings", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtTicketPrice.Focus()
            Return False
        End If

        Return True
    End Function

    Private Sub btnRepeat_Click(sender As Object, e As EventArgs) Handles btnRepeat.Click
        If Not CanWorkOutTimes() Then
            Exit Sub
        End If

        If Not TimeAndPriceAreOk(True) Then
            Exit Sub
        End If

        Dim firstDay As Date = dtpScreeningDate.Value.Date
        Dim lastDay As Date = dtpRepeatUntil.Value.Date

        If lastDay < firstDay Then
            MessageBox.Show("The repeat until date is before the date the run starts on", "Screenings", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If firstDay < Date.Today Then
            MessageBox.Show("A run cannot start in the past", "Screenings", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Dim howManyDays As Integer = DateDiff(DateInterval.Day, firstDay, lastDay) + 1

        If howManyDays > 31 Then
            MessageBox.Show("That is " & howManyDays & " days. A run can be up to 31 days at a time.", "Too long a run", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Dim startMinutes As Integer = TimeAsMinutes(txtScreeningTime.Text)
        Dim needed As Integer = ScreenTimeNeeded(DurationOfPickedFilm())
        Dim screenID As Integer = CInt(cboScreen.SelectedValue)

        Dim goodDays(howManyDays) As Date
        Dim goodCount As Integer = 0
        Dim skipped As String = ""
        Dim skippedCount As Integer = 0

        For dayNumber As Integer = 0 To howManyDays - 1
            Dim thisDay As Date = firstDay.AddDays(dayNumber)

            Dim starts(0) As Integer
            Dim finishes(0) As Integer
            Dim titles(0) As String
            Dim howMany As Integer

            LoadDayOccupancy(screenID, thisDay, 0, starts, finishes, titles, howMany)
            SortDayByStart(starts, finishes, titles, howMany)

            Dim clashes As Boolean = False

            For i As Integer = 0 To howMany - 1
                If startMinutes < finishes(i) And starts(i) < startMinutes + needed Then
                    clashes = True
                End If
            Next

            If clashes Then
                skippedCount = skippedCount + 1

                If skippedCount <= 4 Then
                    If skipped <> "" Then
                        skipped = skipped & ", "
                    End If
                    skipped = skipped & thisDay.ToString("ddd d MMM")
                End If
            Else
                goodDays(goodCount) = thisDay
                goodCount = goodCount + 1
            End If
        Next

        If goodCount = 0 Then
            MessageBox.Show("There is something already on at " & txtScreeningTime.Text & " in " & cboScreen.Text &
                            " on every one of those " & howManyDays & " days." & vbCrLf & vbCrLf &
                            "Try another time, another screen, or use Find me a free time.",
                            "Nothing could be added", MessageBoxButtons.OK, MessageBoxIcon.Information)
            WriteLog("SCREENING", "Run refused, " & cboFilm.Text & " clashes on all " & howManyDays & " days", LogWarning)
            Exit Sub
        End If

        Dim question As String = "Put " & cboFilm.Text & " on in " & cboScreen.Text & " at " &
                                 txtScreeningTime.Text & " on " & goodCount & " day(s)?"

        If skippedCount > 0 Then
            question = question & vbCrLf & vbCrLf & "Skipping " & skipped
            If skippedCount > 4 Then
                question = question & " and " & (skippedCount - 4) & " more"
            End If
            question = question & ", something else is already on."
        End If

        If MessageBox.Show(question, "Put a run on", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = DialogResult.No Then
            Exit Sub
        End If

        Dim added As Integer = SaveManyScreenings(goodDays, goodCount, txtScreeningTime.Text, screenID)

        If added = 0 Then
            Exit Sub
        End If

        WriteLog("SCREENING", "Run added: " & cboFilm.Text & " in " & cboScreen.Text & " at " &
                              txtScreeningTime.Text & ", " & added & " screenings", LogChange)
        LoadScreenings()
        RefreshTimelineIfShowing()
        SayDone(lblSaved, "Put " & cboFilm.Text & " on " & added & " time(s)")
    End Sub

    Private Sub btnFillDay_Click(sender As Object, e As EventArgs) Handles btnFillDay.Click
        If Not CanWorkOutTimes() Then
            Exit Sub
        End If

        If Not TimeAndPriceAreOk(False) Then
            Exit Sub
        End If

        Dim theDay As Date = dtpScreeningDate.Value.Date

        If theDay < Date.Today Then
            MessageBox.Show("That day has already been, there is no point filling it up", "Screenings", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Dim needed As Integer = ScreenTimeNeeded(DurationOfPickedFilm())
        Dim screenID As Integer = CInt(cboScreen.SelectedValue)

        Dim starts(0) As Integer
        Dim finishes(0) As Integer
        Dim titles(0) As String
        Dim howMany As Integer

        LoadDayOccupancy(screenID, theDay, 0, starts, finishes, titles, howMany)
        SortDayByStart(starts, finishes, titles, howMany)

        Const MostInOneDay As Integer = 8

        ReDim Preserve starts(howMany + MostInOneDay)
        ReDim Preserve finishes(howMany + MostInOneDay)
        ReDim Preserve titles(howMany + MostInOneDay)

        Dim newTimes(MostInOneDay) As Integer
        Dim newCount As Integer = 0
        Dim cursor As Integer = FirstShowMinutes

        While newCount < MostInOneDay
            Dim slot As Integer = FirstFitFrom(cursor, needed, starts, finishes, howMany)

            If slot < 0 Then
                Exit While
            End If

            newTimes(newCount) = slot
            newCount = newCount + 1

            starts(howMany) = slot
            finishes(howMany) = slot + needed
            titles(howMany) = cboFilm.Text
            howMany = howMany + 1
            SortDayByStart(starts, finishes, titles, howMany)

            cursor = slot + needed
        End While

        If newCount = 0 Then
            MessageBox.Show("There is no room left for " & cboFilm.Text & " in " & cboScreen.Text & " that day.",
                            "Nothing free", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        Dim timeList As String = ""
        For i As Integer = 0 To newCount - 1
            If timeList <> "" Then
                timeList = timeList & ", "
            End If
            timeList = timeList & MinutesAsTime(newTimes(i))
        Next

        Dim question As String = "Put " & cboFilm.Text & " on " & newCount & " time(s) in " & cboScreen.Text &
                                 " on " & theDay.ToString("ddd d MMM") & "?" & vbCrLf & vbCrLf & timeList

        If MessageBox.Show(question, "Fill the day", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = DialogResult.No Then
            Exit Sub
        End If

        Dim added As Integer = SaveDayOfScreenings(theDay, newTimes, newCount, screenID)

        If added = 0 Then
            Exit Sub
        End If

        WriteLog("SCREENING", "Day filled: " & cboFilm.Text & " in " & cboScreen.Text & " on " &
                              theDay.ToString("dd/MM/yyyy") & ", " & added & " screenings", LogChange)
        LoadScreenings()
        RefreshTimelineIfShowing()
        SayDone(lblSaved, "Put " & cboFilm.Text & " on " & added & " time(s) that day")
    End Sub

    Private Function SaveDayOfScreenings(theDay As Date, times() As Integer, howMany As Integer, screenID As Integer) As Integer
        Dim theDays(howMany) As Date
        Dim theTimes(howMany) As String

        For i As Integer = 0 To howMany - 1
            theDays(i) = theDay
            theTimes(i) = MinutesAsTime(times(i))
        Next

        Return SaveScreeningRun(theDays, theTimes, howMany, screenID)
    End Function

    Private Function SaveManyScreenings(theDays() As Date, howMany As Integer, timeText As String, screenID As Integer) As Integer
        Dim theTimes(howMany) As String

        For i As Integer = 0 To howMany - 1
            theTimes(i) = timeText
        Next

        Return SaveScreeningRun(theDays, theTimes, howMany, screenID)
    End Function

    Private Function SaveScreeningRun(theDays() As Date, theTimes() As String, howMany As Integer, screenID As Integer) As Integer
        Dim written As Integer = 0

        If DbConnect() Then
            Dim trans As OleDbTransaction = cn.BeginTransaction()
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.Transaction = trans

            Try
                Dim filmID As Integer = CInt(cboFilm.SelectedValue)
                Dim price As Double = PriceFromBox()

                For i As Integer = 0 To howMany - 1
                    InsertScreening(SQLCmd, filmID, screenID, theDays(i), theTimes(i), price)
                    written = written + 1
                Next

                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                written = 0
                MessageBox.Show("Nothing was saved, the whole run was put back." & vbCrLf & vbCrLf & ex.Message,
                                "Could not save the run", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try

            cn.Close()
        End If

        Return written
    End Function

    Private Function PriceFromBox() As Double
        Return Math.Round(Val(txtTicketPrice.Text), 2)
    End Function

    Private Function SameScreeningExists() As Boolean
        Dim found As Integer = 0

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT COUNT(*) FROM tblScreening " &
                                 "WHERE FilmID = @FilmID AND ScreenID = @ScreenID " &
                                 "AND ScreeningDate = @ScreeningDate AND ScreeningTime = @ScreeningTime " &
                                 "AND ScreeningID <> @ScreeningID " &
                                 "AND (ScreeningStatus IS NULL OR ScreeningStatus <> 'Cancelled')"
            SQLCmd.Parameters.AddWithValue("@FilmID", CInt(cboFilm.SelectedValue))
            SQLCmd.Parameters.AddWithValue("@ScreenID", CInt(cboScreen.SelectedValue))
            SQLCmd.Parameters.AddWithValue("@ScreeningDate", dtpScreeningDate.Value.Date)
            SQLCmd.Parameters.AddWithValue("@ScreeningTime", txtScreeningTime.Text)
            SQLCmd.Parameters.AddWithValue("@ScreeningID", selectedScreeningID)
            found = CInt(SQLCmd.ExecuteScalar())
            cn.Close()
        End If

        Return found > 0
    End Function

    Private Function DetailsAreOk(isNew As Boolean) As Boolean
        If cboFilm.SelectedIndex = -1 Or cboScreen.SelectedIndex = -1 Then
            MessageBox.Show("Pick a film and a screen", "Screenings", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If

        If Not ScreenIsInService(CLng(cboScreen.SelectedValue)) Then
            MessageBox.Show("'" & cboScreen.Text & "' is out of service at the moment, so nothing can be " &
                            "scheduled in it." & vbCrLf & vbCrLf &
                            "Put it back in service on the screens form, or pick a different screen.",
                            "Screen is out of service", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            WriteLog("SCREENING", "Refused, " & cboScreen.Text & " is out of service", LogWarning)
            Return False
        End If

        If isNew And dtpScreeningDate.Value.Date < Date.Today Then
            MessageBox.Show("Screening date cant be in the past", "Screenings", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If

        If txtScreeningTime.Text.Trim() = "" Then
            MessageBox.Show("Enter a screening time (HH:MM)", "Screenings", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtScreeningTime.Focus()
            Return False
        End If

        If Not IsValidScreeningTime(txtScreeningTime.Text) Then
            MessageBox.Show("Screening time must be in HH:MM format, e.g. 14:30", "Screenings", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtScreeningTime.Focus()
            Return False
        End If

        If txtTicketPrice.Text.Trim() = "" Then
            MessageBox.Show("Enter a ticket price", "Screenings", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtTicketPrice.Focus()
            Return False
        End If

        If Not IsNumeric(txtTicketPrice.Text) Then
            MessageBox.Show("Ticket price must be a number", "Screenings", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtTicketPrice.Focus()
            Return False
        End If

        If Val(txtTicketPrice.Text) <= 0 Then
            MessageBox.Show("Ticket price must be greater than 0", "Screenings", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtTicketPrice.Focus()
            Return False
        End If

        If Val(txtTicketPrice.Text) > 50 Then
            MessageBox.Show("That ticket price looks too high, it should be in pounds", "Screenings", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtTicketPrice.Focus()
            Return False
        End If

        If SameScreeningExists() Then
            MessageBox.Show("That screening is already on the system." & vbCrLf & vbCrLf &
                            cboFilm.Text & " is already showing in " & cboScreen.Text & " at " &
                            txtScreeningTime.Text & " on " & dtpScreeningDate.Value.ToString("dd/MM/yyyy") & ".",
                            "Already scheduled", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If

        Dim clash As String = ClashingScreening()

        If clash <> "" Then
            Dim slot As Integer = NextFreeSlot()
            Dim advice As String = "Pick a different time or a different screen."

            If slot >= 0 Then
                advice = "The first time it would fit on that screen is " & MinutesAsTime(slot) & "." & vbCrLf &
                         "Find me a free time will put that in for you."
            End If

            MessageBox.Show("That screen is already showing " & clash & "." & vbCrLf & vbCrLf & advice,
                            "Two films at once", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            WriteLog("SCREENING", "Clash refused, " & cboScreen.Text & " is already showing " & clash & " at " & txtScreeningTime.Text, LogWarning)
            Return False
        End If

        Return True
    End Function

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        If Not DetailsAreOk(True) Then
            Exit Sub
        End If

        Dim saved As Boolean = False

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "INSERT INTO tblScreening (FilmID, ScreenID, ScreeningDate, ScreeningTime, TicketPrice, TrailerMinutes, TurnaroundMinutes) " &
                                 "VALUES (@FilmID, @ScreenID, @ScreeningDate, @ScreeningTime, @TicketPrice, @TrailerMinutes, @TurnaroundMinutes)"
            SQLCmd.Parameters.AddWithValue("@FilmID", CInt(cboFilm.SelectedValue))
            SQLCmd.Parameters.AddWithValue("@ScreenID", CInt(cboScreen.SelectedValue))
            SQLCmd.Parameters.AddWithValue("@ScreeningDate", dtpScreeningDate.Value.Date)
            SQLCmd.Parameters.AddWithValue("@ScreeningTime", txtScreeningTime.Text)
            SQLCmd.Parameters.AddWithValue("@TicketPrice", PriceFromBox())
            SQLCmd.Parameters.AddWithValue("@TrailerMinutes", TrailerMinutes)
            SQLCmd.Parameters.AddWithValue("@TurnaroundMinutes", TurnaroundMinutes)
            SQLCmd.ExecuteNonQuery()
            cn.Close()
            saved = True
        End If

        If Not saved Then
            Exit Sub
        End If

        Dim savedName As String = cboFilm.Text & " on " & cboScreen.Text
        WriteLog("SCREENING", "Screening added: " & cboFilm.Text & " on " & cboScreen.Text, LogChange)
        LoadScreenings()
        RefreshTimelineIfShowing()
        ClearFields()
        SayDone(lblSaved, "Added '" & savedName & "'")
    End Sub

    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        If selectedScreeningID = 0 Then
            MessageBox.Show("Select a screening in the grid first", "Screenings", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If Not DetailsAreOk(False) Then
            Exit Sub
        End If

        Dim sold As Integer = SeatsSold(selectedScreeningID)

        If sold > 0 And CInt(cboScreen.SelectedValue) <> screenWhenPicked Then
            MessageBox.Show(sold & " seat(s) are already booked on this screening, and those seats belong to " &
                            "the screen it is in now." & vbCrLf &
                            "Moving it to another screen would leave those bookings holding seats that are not " &
                            "in the room the film is on in." & vbCrLf & vbCrLf &
                            "Cancel the bookings first, or put it back in the screen it was in.",
                            "Cannot move this screening", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            WriteLog("SCREENING", "Screen change refused for screening " & selectedScreeningID & ", " &
                                  sold & " seat(s) already sold", LogWarning)
            Exit Sub
        End If

        If sold > 0 Then
            If MessageBox.Show(sold & " seat(s) are already booked on this screening." & vbCrLf &
                               "Changing it will not tell those customers." & vbCrLf & vbCrLf &
                               "Carry on?", "Already booked", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) = DialogResult.No Then
                Exit Sub
            End If
        End If

        Dim saved As Boolean = False

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "UPDATE tblScreening " &
                                 "SET FilmID = @FilmID, ScreenID = @ScreenID, ScreeningDate = @ScreeningDate, ScreeningTime = @ScreeningTime, TicketPrice = @TicketPrice, " &
                                 "TrailerMinutes = @TrailerMinutes, TurnaroundMinutes = @TurnaroundMinutes " &
                                 "WHERE ScreeningID = @ScreeningID"
            SQLCmd.Parameters.AddWithValue("@FilmID", CInt(cboFilm.SelectedValue))
            SQLCmd.Parameters.AddWithValue("@ScreenID", CInt(cboScreen.SelectedValue))
            SQLCmd.Parameters.AddWithValue("@ScreeningDate", dtpScreeningDate.Value.Date)
            SQLCmd.Parameters.AddWithValue("@ScreeningTime", txtScreeningTime.Text)
            SQLCmd.Parameters.AddWithValue("@TicketPrice", PriceFromBox())
            SQLCmd.Parameters.AddWithValue("@TrailerMinutes", TrailerMinutes)
            SQLCmd.Parameters.AddWithValue("@TurnaroundMinutes", TurnaroundMinutes)
            SQLCmd.Parameters.AddWithValue("@ScreeningID", selectedScreeningID)
            SQLCmd.ExecuteNonQuery()
            cn.Close()
            saved = True
        End If

        If Not saved Then
            Exit Sub
        End If

        Dim savedName As String = cboFilm.Text & " on " & cboScreen.Text
        WriteLog("SCREENING", "Screening updated: " & cboFilm.Text & " on " & cboScreen.Text, LogChange)
        LoadScreenings()
        RefreshTimelineIfShowing()
        ClearFields()
        SayDone(lblSaved, "Saved changes to '" & savedName & "'")
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If selectedScreeningID = 0 Then
            MessageBox.Show("Select a screening in the grid first", "Screenings", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Dim bookings As Integer = BookingsOnScreening(selectedScreeningID)

        If bookings > 0 Then
            Dim sold As Integer = SeatsSold(selectedScreeningID)
            MessageBox.Show("This screening has " & bookings & " booking(s) on it, " &
                            sold & " seat(s) in total." & vbCrLf & vbCrLf &
                            "A booking is kept even after it is cancelled, so that the sale stays in the takings " &
                            "and the refund is on record. That means a screening that has ever been booked cannot " &
                            "be removed, and cancelling those bookings will not change that." & vbCrLf & vbCrLf &
                            "Use Cancel this screening instead. It stops the screening going ahead, refunds " &
                            "everyone on it and keeps the history.",
                            "Cannot delete", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            WriteLog("SCREENING", "Delete refused, the screening has " & bookings & " booking(s) on it", LogWarning)
            Exit Sub
        End If

        If MessageBox.Show("Delete this screening?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = DialogResult.No Then
            Exit Sub
        End If

        Dim saved As Boolean = False

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "DELETE FROM tblScreening " &
                                 "WHERE ScreeningID = @ScreeningID"
            SQLCmd.Parameters.AddWithValue("@ScreeningID", selectedScreeningID)
            SQLCmd.ExecuteNonQuery()
            cn.Close()
            saved = True
        End If

        If Not saved Then
            Exit Sub
        End If

        Dim savedName As String = cboFilm.Text

        WriteLog("SCREENING", "Screening deleted: ScreeningID " & selectedScreeningID, LogChange)
        LoadScreenings()
        RefreshTimelineIfShowing()
        ClearFields()
        SayDone(lblSaved, "Deleted the '" & savedName & "' screening")
    End Sub

    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        ClearFields()
    End Sub

    Private Sub Details_Changed(sender As Object, e As EventArgs) Handles cboFilm.SelectedIndexChanged, cboScreen.SelectedIndexChanged,
        dtpScreeningDate.ValueChanged, txtScreeningTime.TextChanged, txtTicketPrice.TextChanged
        If fillingBoxes Then
            Exit Sub
        End If

        boxesChanged = True
    End Sub

    Private Function ChangesCanBeLost() As Boolean
        If Not boxesChanged Then
            Return True
        End If

        Return MessageBox.Show("There are changes in the boxes that have not been saved." & vbCrLf &
                               "Throw them away?", "Unsaved changes",
                               MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                               MessageBoxDefaultButton.Button2) = DialogResult.Yes
    End Function

    Private Sub ClearFields()
        fillingBoxes = True

        lblSaved.Text = ""
        selectedScreeningID = 0
        screenWhenPicked = 0
        cboFilm.SelectedIndex = -1
        cboScreen.SelectedIndex = -1
        dtpScreeningDate.Value = Date.Now
        dtpRepeatUntil.Value = Date.Today.AddDays(6)
        txtScreeningTime.Text = ""
        txtTicketPrice.Text = Format(DefaultTicketPrice, "0.00")
        fillingBoxes = False
        boxesChanged = False

        dgvScreenings.ClearSelection()
        ShowWhatIsBeingEdited()
        ShowEndTime()
    End Sub

    Private Sub ShowWhatIsBeingEdited()
        If selectedScreeningID = 0 Then
            lblStatus.Text = "Adding a new screening"
            btnUpdate.Enabled = False
            btnDelete.Enabled = False
            btnAdd.Enabled = True
        Else
            lblStatus.Text = "Editing screening " & selectedScreeningID & ": " & cboFilm.Text & " in " & cboScreen.Text
            btnUpdate.Enabled = True
            btnDelete.Enabled = True
            btnAdd.Enabled = False
        End If
    End Sub

    Private Sub dgvScreenings_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvScreenings.CellClick
        If e.RowIndex < 0 Then Exit Sub

        If Not ChangesCanBeLost() Then
            Exit Sub
        End If

        fillingBoxes = True

        Dim row As DataGridViewRow = dgvScreenings.Rows(e.RowIndex)
        selectedScreeningID = CInt(row.Cells("ScreeningID").Value)
        cboFilm.SelectedValue = CInt(row.Cells("FilmID").Value)
        cboScreen.SelectedValue = CInt(row.Cells("ScreenID").Value)
        screenWhenPicked = CInt(row.Cells("ScreenID").Value)
        dtpScreeningDate.Value = CDate(row.Cells("ScreeningDate").Value)
        txtScreeningTime.Text = row.Cells("ScreeningTime").Value.ToString()

        txtTicketPrice.Text = Format(CDbl(row.Cells("TicketPrice").Value), "0.00")

        fillingBoxes = False
        boxesChanged = False

        ShowWhatIsBeingEdited()
        ShowEndTime()
    End Sub

    Private Sub LoadScreeningIntoBoxes(screeningID As Integer)
        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT FilmID, ScreenID, ScreeningDate, ScreeningTime, TicketPrice " &
                                 "FROM tblScreening WHERE ScreeningID = @ScreeningID"
            SQLCmd.Parameters.AddWithValue("@ScreeningID", screeningID)
            Dim rs As OleDbDataReader = SQLCmd.ExecuteReader()

            If rs.Read() Then
                fillingBoxes = True
                selectedScreeningID = screeningID
                screenWhenPicked = CInt(rs("ScreenID"))
                cboFilm.SelectedValue = CInt(rs("FilmID"))
                cboScreen.SelectedValue = CInt(rs("ScreenID"))
                dtpScreeningDate.Value = CDate(rs("ScreeningDate"))
                txtScreeningTime.Text = rs("ScreeningTime").ToString()
                txtTicketPrice.Text = Format(CDbl(rs("TicketPrice")), "0.00")
                fillingBoxes = False
                boxesChanged = False
            End If

            rs.Close()
            cn.Close()
        End If

        ShowWhatIsBeingEdited()
        ShowEndTime()
    End Sub

    Private Sub dgvScreenings_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvScreenings.CellDoubleClick
        If e.RowIndex < 0 Then Exit Sub

        If selectedScreeningID = 0 Then
            Exit Sub
        End If

        txtScreeningTime.Focus()
        txtScreeningTime.SelectAll()
    End Sub

End Class
