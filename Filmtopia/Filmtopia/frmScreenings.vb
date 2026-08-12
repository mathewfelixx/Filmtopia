Imports System.Data.OleDb

Public Class frmScreenings

    'tracks the ScreeningID of the row currently selected in the grid, 0 means nothing selected
    Private selectedScreeningID As Integer = 0

    'how long the screen is left empty after a film before the next one can start, for getting
    'everybody out and cleaning up
    Private Const TurnaroundMinutes As Integer = 15

    'the adverts and trailers that run before the film does. the time on a ticket is when the
    'adverts start, not when the film does, so a screening ties the screen up for longer than the
    'running time of the film. without this the schedule was too optimistic by twenty minutes and
    'two showings could be put on that would really have run into each other
    Private Const TrailerMinutes As Integer = 20

    'the earliest and the latest a film is allowed to start, used when suggesting a time
    Private Const FirstShowMinutes As Integer = 10 * 60
    Private Const LastShowMinutes As Integer = 23 * 60

    'how tall one screen's lane is on the timeline, and the strip across the top the hours are
    'written in. the hours are drawn in that strip without the scrolling taken off, so they stay
    'put while the lanes underneath them move
    Private Const TimelineLaneHeight As Integer = 32
    Private Const TimelineHeaderHeight As Integer = 22

    'true while the form is setting itself up, so filling the show box does not load the grid
    'before everything is ready
    Private stillLoading As Boolean = True

    'true once something has been typed into the boxes that has not been saved yet. it is what
    'the warning before another row replaces it is based on
    Private boxesChanged As Boolean = False

    'true while a row is being copied into the boxes, so filling them in does not count as typing
    Private fillingBoxes As Boolean = False

    'made once and reused for the nearly full rows. making a new font for every row every time the
    'grid is coloured in would be throwing fonts away by the hundred
    Private rowBoldFont As New Font("Segoe UI", 9, FontStyle.Bold)

    'one lane down the timeline for each screen. these two are the same length and line up with
    'each other, so laneName(2) is the name of the screen whose id is in laneScreenID(2)
    Private laneScreenID() As Integer
    Private laneName() As String
    Private laneCount As Integer = 0

    'everything on the day the timeline is showing. all of these line up with each other the same
    'way, one position per screening, and timelineCount says how many of them are filled in
    Private timelineID() As Integer
    Private timelineLane() As Integer
    Private timelineStart() As Integer
    Private timelineDuration() As Integer
    Private timelineTitle() As String
    Private timelineSold() As Integer
    Private timelineCapacity() As Integer
    Private timelineCount As Integer = 0

    'the tooltip shown when the mouse is over a showing on the timeline. one is made for the whole
    'form rather than one per block, the same way the seat map on the booking screen does it
    Private timelineTips As New ToolTip

    'which showing the tooltip is currently describing, so it is not set again on every single
    'mouse move, which makes it flicker
    Private tipShowingFor As Integer = -1

    Private Sub frmScreenings_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CommonFormStartup(Me)

        LoadFilmsCombo()
        LoadScreensCombo()

        LoadScreenFilterCombo()

        cboShow.Items.Add("Today")
        cboShow.Items.Add("This week")
        cboShow.Items.Add("Still to come")
        cboShow.Items.Add("Already been on")
        cboShow.Items.Add("Everything")

        'start on whichever the user last left it on. if the saved one is not in the list any more
        'IndexOf comes back as -1, so it falls back to still to come rather than an empty box
        cboShow.SelectedIndex = cboShow.Items.IndexOf(LastScreeningsShow)
        If cboShow.SelectedIndex = -1 Then
            cboShow.SelectedIndex = cboShow.Items.IndexOf("Still to come")
        End If

        'the empty grid guard still wins over what was remembered. opening on a filter that looks
        'forwards when there is nothing coming up looks broken, so in that case it starts on the
        'lot. already been on is left alone, it is not looking forwards
        If (cboShow.Text = "Today" Or cboShow.Text = "This week" Or cboShow.Text = "Still to come") And UpcomingCount() = 0 Then
            cboShow.SelectedIndex = cboShow.Items.IndexOf("Everything")
        End If

        'and the same for the screen that was last looked at, in case it has since been deleted
        cboScreenFilter.SelectedIndex = cboScreenFilter.FindStringExact(LastScreeningsScreen)
        If cboScreenFilter.SelectedIndex = -1 Then
            cboScreenFilter.SelectedIndex = 0
        End If

        stillLoading = False

        'lets the form see escape before the box that has focus does
        Me.KeyPreview = True

        LoadScreenings()
        ClearFields()
        cboFilm.Focus()
        WriteLog("SCREENING", "Screenings form opened")
    End Sub

    'remembers the show filter for next time. saved on close rather than on every change so that
    'flicking between the three is not a write to the database each time
    Private Sub frmScreenings_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If cboShow.Text = "" Then
            Exit Sub
        End If

        LastScreeningsShow = cboShow.Text
        LastScreeningsScreen = cboScreenFilter.Text
        SaveUserSettings()
    End Sub

    'saves the schedule as it is on screen, which is what the show filter has left showing
    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        If ExportGridToCsv(dgvScreenings, "Screenings.csv", "Screenings") Then
            WriteLog("SCREENING", "Screening list exported, " & dgvScreenings.Rows.Count & " screenings")
        End If
    End Sub

    'escape clears the search if there is anything in it, and shuts the form if there is not.
    'that is the way round the films form does it, and it means escape never loses a search
    'and closes the whole screen in one press
    Private Sub frmScreenings_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.F5 Then
            LoadScreenings()
        ElseIf e.KeyCode = Keys.Escape Then
            If txtSearch.Text <> "" Then
                txtSearch.Clear()
            Else
                Me.Close()
            End If
        End If
    End Sub

    'typing in the search box does not go to the database on every letter. each key press starts
    'the little timer off again, so the grid is only reloaded once somebody has stopped typing
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

    'picking a screen to look at reloads the grid straight away, there is nothing to wait for
    Private Sub cboScreenFilter_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboScreenFilter.SelectedIndexChanged
        If stillLoading Then
            Exit Sub
        End If

        LoadScreenings()
    End Sub

    'how many screenings are today or later, used to decide what the form opens on
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

    'fills the film combo with every film title
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

    'fills the screen combo with every screen name
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

    'fills the screen box above the grid, which narrows the list to one room. it is a separate box
    'from the one in the editor underneath because they are doing different jobs, one picks which
    'screen to look at and the other picks which screen to put a film in
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

            'the all screens choice goes in the table rather than being added to the box, because
            'a box that has been given a data source will not take items added to it by hand
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

    'loads the screenings into the grid, narrowed down by the date filter, the screen box and
    'whatever has been typed into the search box
    Private Sub LoadScreenings()
        Dim dt As New DataTable

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn

            'join screening to film (for the title and how long it runs) and to screen (for the
            'name and how many seats it holds). the seats sold come back in the same query as a
            'subquery instead of a count per row, which used to be one trip to the database for
            'every screening on the grid. it only looks at tblBookingSeat, no join inside it, which
            'is exactly what the ScreeningID on the seat row is there for
            Dim baseQuery As String = "SELECT tblScreening.ScreeningID, FilmTitle, ScreenName, ScreeningDate, ScreeningTime, TicketPrice, " &
                                      "FilmDuration, ScreenCapacity, tblScreening.FilmID, tblScreening.ScreenID, " &
                                      "(SELECT COUNT(*) FROM tblBookingSeat AS bs WHERE bs.ScreeningID = tblScreening.ScreeningID) AS SeatsBooked " &
                                      "FROM (tblScreening INNER JOIN tblFilm ON tblScreening.FilmID = tblFilm.FilmID) " &
                                      "INNER JOIN tblScreen ON tblScreening.ScreenID = tblScreen.ScreenID"

            'the where clause is built up a piece at a time from whatever the three boxes are set
            'to. the parameters have to be added in the same order the @names appear in the
            'finished query, because oledb matches them by position and not by name
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
                'the most recent one that has been on is the one somebody is most likely after
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

            'same order as above
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
                'the same thing twice, because a positional parameter cannot be used twice over
                SQLCmd.Parameters.AddWithValue("@Search", "%" & txtSearch.Text.Trim() & "%")
                SQLCmd.Parameters.AddWithValue("@Search2", "%" & txtSearch.Text.Trim() & "%")
            End If

            Dim da As New OleDbDataAdapter(SQLCmd)
            da.Fill(dt)
            cn.Close()
        End If

        'how full each screening is. it is the most useful thing on the whole screen, it says at a
        'glance which showings are selling and which are not. the numbers come from the query now,
        'this only turns them into something to read
        dt.Columns.Add("SoldText", GetType(String))
        dt.Columns.Add("PercentFull", GetType(Integer))
        dt.Columns.Add("EndsAt", GetType(String))

        For Each row As DataRow In dt.Rows
            Dim sold As Integer = CInt(row("SeatsBooked"))
            Dim capacity As Integer = CInt(row("ScreenCapacity"))

            row("SoldText") = sold & " of " & capacity

            'a screen with no seats in it would be a divide by zero, so it is checked first
            If capacity > 0 Then
                row("PercentFull") = CInt(sold * 100 / capacity)
            Else
                row("PercentFull") = 0
            End If

            row("EndsAt") = EndTimeText(row("ScreeningTime").ToString(), CInt(row("FilmDuration")))
        Next

        dgvScreenings.DataSource = dt

        If dgvScreenings.Columns.Contains("ScreeningID") Then
            'the raw IDs and the working out columns are kept for the code but not put on show
            dgvScreenings.Columns("FilmID").Visible = False
            dgvScreenings.Columns("ScreenID").Visible = False
            dgvScreenings.Columns("FilmDuration").Visible = False
            dgvScreenings.Columns("ScreenCapacity").Visible = False
            dgvScreenings.Columns("SeatsBooked").Visible = False

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

            'the working out columns are tacked on the end of the table, so without this the ends
            'time would sit after the seats sold instead of next to the time it belongs with
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
            'the quotes round the percent sign stop it being treated as multiply by a hundred
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

    'everything that colours the grid in, in one place. it is separate from loading because
    'clicking a column header re-orders the rows underneath the colours, so it all has to be
    'done again afterwards
    Private Sub MarkTheGrid()
        MarkSoldOutScreenings()
        ColourOccupancy()
    End Sub

    'sorting moves the rows about but leaves the colouring where it was, so the wrong rows end up
    'looking sold out. doing it again once the sort has finished puts it back on the right ones
    Private Sub dgvScreenings_Sorted(sender As Object, e As EventArgs) Handles dgvScreenings.Sorted
        MarkTheGrid()
    End Sub

    'makes a screening that is filling up stand out, red for nearly full and amber for half full
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
                'an empty colour means use the grids normal one. it has to be set back like this
                'in case this cell was coloured in before the user sorted a column
                cell.Style.ForeColor = Color.Empty
                cell.Style.SelectionForeColor = Color.Empty
                cell.Style.Font = Nothing
            End If
        Next
    End Sub

    'a screening with every seat gone is worth seeing straight away, and one that nobody has
    'booked at all is worth knowing about too
    Private Sub MarkSoldOutScreenings()
        For Each row As DataGridViewRow In dgvScreenings.Rows
            Dim capacity As Integer = CInt(row.Cells("ScreenCapacity").Value)
            'read off the hidden number rather than picking it back out of the 12 of 80 text,
            'which stopped working the moment the text had been replaced with SOLD OUT
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
                'sorting moves rows around underneath the colours, so a row that is not sold out
                'has to be put back to the grids normal colours in case it used to be one that was
                row.DefaultCellStyle.BackColor = Color.Empty
                row.DefaultCellStyle.ForeColor = Color.Empty
            End If
        Next
    End Sub

    'counts how many seats have been booked on a screening
    Private Function SeatsSold(screeningID As Integer) As Integer
        Dim total As Integer = 0

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            'no join needed, the screening is on the seat row itself
            SQLCmd.CommandText = "SELECT COUNT(*) FROM tblBookingSeat " &
                                 "WHERE ScreeningID = @ScreeningID"
            SQLCmd.Parameters.AddWithValue("@ScreeningID", screeningID)
            total = CInt(SQLCmd.ExecuteScalar())
            cn.Close()
        End If

        Return total
    End Function

    'counts how many bookings are on a screening, seats or not. this is what stops a screening
    'being deleted while somebody is still booked onto it
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

    'says how many screenings are showing and how many seats they have sold between them
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

    '=== the day timeline ===================================================================
    'the grid says what is on. the timeline says what the day actually looks like, one lane per
    'screen with the showings drawn along it in time order, so a gap big enough to put another
    'film in is something you can see rather than something you have to work out

    'reads the screens into the lanes, and everything on the chosen day into the arrays that get
    'drawn. it is all read up front rather than during the paint, because a paint can happen many
    'times over and must never be waiting on the database
    Private Sub LoadTimelineDay()
        laneCount = 0
        timelineCount = 0

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn

            'the lanes first. every screen gets one whether anything is on in it or not, an empty
            'room is exactly the thing somebody looking for space wants to see
            SQLCmd.CommandText = "SELECT ScreenID, ScreenName FROM tblScreen ORDER BY ScreenName"
            Dim daScreens As New OleDbDataAdapter(SQLCmd)
            Dim dtScreens As New DataTable
            daScreens.Fill(dtScreens)

            ReDim laneScreenID(dtScreens.Rows.Count)
            ReDim laneName(dtScreens.Rows.Count)

            For Each screenRow As DataRow In dtScreens.Rows
                laneScreenID(laneCount) = CInt(screenRow("ScreenID"))
                laneName(laneCount) = screenRow("ScreenName").ToString()
                laneCount = laneCount + 1
            Next

            'then everything on that day, with how full each one is, so a busy showing can be
            'told from an empty one at a glance
            SQLCmd.CommandText = "SELECT tblScreening.ScreeningID, tblScreening.ScreenID, FilmTitle, ScreeningTime, " &
                                 "FilmDuration, ScreenCapacity, " &
                                 "(SELECT COUNT(*) FROM tblBookingSeat AS bs WHERE bs.ScreeningID = tblScreening.ScreeningID) AS SeatsBooked " &
                                 "FROM (tblScreening INNER JOIN tblFilm ON tblScreening.FilmID = tblFilm.FilmID) " &
                                 "INNER JOIN tblScreen ON tblScreening.ScreenID = tblScreen.ScreenID " &
                                 "WHERE ScreeningDate = @ScreeningDate " &
                                 "ORDER BY ScreeningTime, tblScreening.ScreeningID"
            SQLCmd.Parameters.AddWithValue("@ScreeningDate", dtpTimelineDate.Value.Date)
            Dim daDay As New OleDbDataAdapter(SQLCmd)
            Dim dtDay As New DataTable
            daDay.Fill(dtDay)
            cn.Close()

            ReDim timelineID(dtDay.Rows.Count)
            ReDim timelineLane(dtDay.Rows.Count)
            ReDim timelineStart(dtDay.Rows.Count)
            ReDim timelineDuration(dtDay.Rows.Count)
            ReDim timelineTitle(dtDay.Rows.Count)
            ReDim timelineSold(dtDay.Rows.Count)
            ReDim timelineCapacity(dtDay.Rows.Count)

            For Each dayRow As DataRow In dtDay.Rows
                Dim startsAt As Integer = TimeAsMinutes(dayRow("ScreeningTime").ToString())
                Dim lane As Integer = LaneForScreen(CInt(dayRow("ScreenID")))

                'a time that does not read as HH:MM cannot be placed on the day, and a screening
                'in a screen that is not in the list has nowhere to go. both are skipped rather
                'than drawn somewhere wrong
                If startsAt >= 0 And lane >= 0 Then
                    timelineID(timelineCount) = CInt(dayRow("ScreeningID"))
                    timelineLane(timelineCount) = lane
                    timelineStart(timelineCount) = startsAt
                    timelineDuration(timelineCount) = CInt(dayRow("FilmDuration"))
                    timelineTitle(timelineCount) = dayRow("FilmTitle").ToString()
                    timelineSold(timelineCount) = CInt(dayRow("SeatsBooked"))
                    timelineCapacity(timelineCount) = CInt(dayRow("ScreenCapacity"))
                    timelineCount = timelineCount + 1
                End If
            Next
        End If

        SetTimelineScrollSize()
        pnlTimeline.Invalidate()
    End Sub

    'which lane a screen is drawn in, or -1 if that screen is not in the list at all
    Private Function LaneForScreen(screenID As Integer) As Integer
        For i As Integer = 0 To laneCount - 1
            If laneScreenID(i) = screenID Then
                Return i
            End If
        Next

        Return -1
    End Function

    'the last minute of the day that has to fit on the picture. normally midnight, but a late
    'showing of a long film runs past it and drawing it half missing would be a lie
    Private Function TimelineLastMinute() As Integer
        Dim latest As Integer = 24 * 60

        For i As Integer = 0 To timelineCount - 1
            Dim finish As Integer = timelineStart(i) + ScreenTimeNeeded(timelineDuration(i))

            If finish > latest Then
                latest = finish
            End If
        Next

        'round it up to the next whole hour so the last hour line is not sat right on the edge
        If latest Mod 60 <> 0 Then
            latest = latest + (60 - (latest Mod 60))
        End If

        Return latest
    End Function

    'works out where everything goes. the drawing and the clicking both call this rather than
    'each working it out for themselves, because if they ever disagreed a click would land on a
    'different showing from the one drawn under the mouse
    Private Sub TimelineGeometry(ByRef leftEdge As Integer, ByRef topEdge As Integer,
                                 ByRef laneHeight As Integer, ByRef minuteWidth As Double,
                                 ByRef firstMinute As Integer, ByRef lastMinute As Integer)
        'room down the left for the screen names, and a strip across the top for the hours
        leftEdge = 92
        laneHeight = TimelineLaneHeight
        firstMinute = FirstShowMinutes
        lastMinute = TimelineLastMinute()

        'the lanes are a fixed height and the panel scrolls when there are more of them than fit.
        'squeezing every screen into the space instead sounds tidier but with eight rooms it left
        'the lanes too thin to put a film title in at all. AutoScrollPosition is zero or negative,
        'so adding it here moves the lanes up by however far they have been scrolled, and because
        'the clicking asks the same question it stays pointing at the right showing
        topEdge = TimelineHeaderHeight + pnlTimeline.AutoScrollPosition.Y

        'the right hand margin is wide enough for the last hour to be written under it, since
        'that label is drawn centred on the line and would otherwise run off the edge
        Dim usableWidth As Integer = pnlTimeline.ClientSize.Width - leftEdge - 26
        minuteWidth = usableWidth / (lastMinute - firstMinute)
    End Sub

    'how tall the whole picture wants to be, which is what the panel scrolls against
    Private Sub SetTimelineScrollSize()
        pnlTimeline.AutoScroll = True
        'no width given, so it only ever scrolls up and down and the day always fits across
        pnlTimeline.AutoScrollMinSize = New Size(0, TimelineHeaderHeight + (laneCount * TimelineLaneHeight) + 6)
    End Sub

    Private Sub pnlTimeline_Paint(sender As Object, e As PaintEventArgs) Handles pnlTimeline.Paint
        Dim g As Graphics = e.Graphics
        g.Clear(FormBack)

        If laneCount = 0 Then
            g.DrawString("There are no screens set up yet", pnlTimeline.Font, New SolidBrush(SubtleFore), 10, 10)
            Exit Sub
        End If

        Dim leftEdge, topEdge, laneHeight As Integer
        Dim firstMinute, lastMinute As Integer
        Dim minuteWidth As Double
        TimelineGeometry(leftEdge, topEdge, laneHeight, minuteWidth, firstMinute, lastMinute)

        Dim linePen As New Pen(BorderCol)
        Dim subtleBrush As New SolidBrush(SubtleFore)
        Dim textBrush As New SolidBrush(TextFore)
        Dim bandBrush As New SolidBrush(AltRowBack)

        Dim dayWidth As Integer = CInt((lastMinute - firstMinute) * minuteWidth)
        Dim lanesBottom As Integer = topEdge + (laneHeight * laneCount)

        'the lanes, one per screen, with every other one shaded so the eye can follow a long row
        For lane As Integer = 0 To laneCount - 1
            Dim laneY As Integer = topEdge + (lane * laneHeight)

            If lane Mod 2 = 1 Then
                g.FillRectangle(bandBrush, leftEdge, laneY, dayWidth, laneHeight)
            End If

            g.DrawLine(linePen, leftEdge, laneY, leftEdge + dayWidth, laneY)
            g.DrawString(laneName(lane), pnlTimeline.Font, textBrush, 4, laneY + (laneHeight \ 2) - 8)
        Next

        'and the line along the very bottom, which the loop above stops short of
        g.DrawLine(linePen, leftEdge, lanesBottom, leftEdge + dayWidth, lanesBottom)

        'the hour lines, drawn over the shading but under the showings
        Dim hour As Integer = firstMinute
        While hour <= lastMinute
            Dim hourX As Integer = leftEdge + CInt((hour - firstMinute) * minuteWidth)
            g.DrawLine(linePen, hourX, topEdge, hourX, lanesBottom)
            hour = hour + 60
        End While

        For i As Integer = 0 To timelineCount - 1
            DrawOneShowing(g, i, leftEdge, topEdge, laneHeight, minuteWidth, firstMinute)
        Next

        'a line down where we are now, but only when the day being looked at is actually today
        If dtpTimelineDate.Value.Date = Date.Today Then
            Dim nowMinutes As Integer = (Date.Now.Hour * 60) + Date.Now.Minute

            If nowMinutes >= firstMinute And nowMinutes <= lastMinute Then
                Dim nowPen As New Pen(OccupancyHigh, 2)
                Dim nowX As Integer = leftEdge + CInt((nowMinutes - firstMinute) * minuteWidth)
                g.DrawLine(nowPen, nowX, topEdge, nowX, lanesBottom)
                nowPen.Dispose()
            End If
        End If

        'the hours go on last, in a strip across the top that is painted over whatever has been
        'scrolled up underneath it, so the times stay readable however far down the screens go
        Dim headerBrush As New SolidBrush(FormBack)
        g.FillRectangle(headerBrush, 0, 0, pnlTimeline.ClientSize.Width, TimelineHeaderHeight - 1)
        g.DrawLine(linePen, 0, TimelineHeaderHeight - 1, pnlTimeline.ClientSize.Width, TimelineHeaderHeight - 1)

        hour = firstMinute
        While hour <= lastMinute
            Dim hourX As Integer = leftEdge + CInt((hour - firstMinute) * minuteWidth)
            g.DrawString(MinutesAsTime(hour), pnlTimeline.Font, subtleBrush, hourX - 14, 3)
            hour = hour + 60
        End While

        headerBrush.Dispose()
        linePen.Dispose()
        subtleBrush.Dispose()
        textBrush.Dispose()
        bandBrush.Dispose()
    End Sub

    'draws one showing as three strips joined together, the trailers, the film itself, and the
    'turnaround afterwards. drawing it that way is the whole point of the picture, it shows that a
    'screening ties the room up for a good deal longer than the film runs for
    Private Sub DrawOneShowing(g As Graphics, i As Integer, leftEdge As Integer, topEdge As Integer,
                               laneHeight As Integer, minuteWidth As Double, firstMinute As Integer)
        Dim blockY As Integer = topEdge + (timelineLane(i) * laneHeight) + 3
        Dim blockHeight As Integer = laneHeight - 7

        Dim trailerX As Integer = leftEdge + CInt((timelineStart(i) - firstMinute) * minuteWidth)
        Dim filmX As Integer = leftEdge + CInt((timelineStart(i) + TrailerMinutes - firstMinute) * minuteWidth)
        Dim cleanX As Integer = leftEdge + CInt((timelineStart(i) + TrailerMinutes + timelineDuration(i) - firstMinute) * minuteWidth)
        Dim endX As Integer = leftEdge + CInt((timelineStart(i) + ScreenTimeNeeded(timelineDuration(i)) - firstMinute) * minuteWidth)

        'the trailers and the turnaround are hatched rather than filled in, because the room is
        'taken but the film is not on, and they should not be mistaken for the showing itself
        Dim trailerBrush As New Drawing2D.HatchBrush(Drawing2D.HatchStyle.LightUpwardDiagonal, SubtleFore, FormBack)
        Dim cleanBrush As New Drawing2D.HatchBrush(Drawing2D.HatchStyle.LightDownwardDiagonal, SubtleFore, FormBack)
        Dim filmBrush As New SolidBrush(TimelineBlockColour(timelineSold(i), timelineCapacity(i)))
        Dim edgePen As New Pen(BorderCol)

        If filmX > trailerX Then
            g.FillRectangle(trailerBrush, trailerX, blockY, filmX - trailerX, blockHeight)
            g.DrawRectangle(edgePen, trailerX, blockY, filmX - trailerX, blockHeight)
        End If

        If endX > cleanX Then
            g.FillRectangle(cleanBrush, cleanX, blockY, endX - cleanX, blockHeight)
            g.DrawRectangle(edgePen, cleanX, blockY, endX - cleanX, blockHeight)
        End If

        If cleanX > filmX Then
            g.FillRectangle(filmBrush, filmX, blockY, cleanX - filmX, blockHeight)
            g.DrawRectangle(edgePen, filmX, blockY, cleanX - filmX, blockHeight)

            'only worth writing in if there is room for it to be read. a title cut off after two
            'letters tells nobody anything, the tooltip is there for the narrow ones
            If cleanX - filmX > 45 Then
                Dim caption As String = MinutesAsTime(timelineStart(i)) & " " & timelineTitle(i)
                Dim clip As New Rectangle(filmX + 3, blockY + 2, cleanX - filmX - 6, blockHeight - 4)
                Dim oldClip As Region = g.Clip
                g.SetClip(clip)
                g.DrawString(caption, pnlTimeline.Font, New SolidBrush(SeatFore), filmX + 4, blockY + (blockHeight \ 2) - 8)
                g.Clip = oldClip
            End If
        End If

        trailerBrush.Dispose()
        cleanBrush.Dispose()
        filmBrush.Dispose()
        edgePen.Dispose()
    End Sub

    'the colour a showing is filled in with, going by how full it is. it is the same eighty and
    'fifty percent rule the grid and the main menu use, so all three agree
    Private Function TimelineBlockColour(sold As Integer, capacity As Integer) As Color
        If capacity <= 0 Then
            Return SeatAvailable
        End If

        Dim percent As Integer = CInt(sold * 100 / capacity)

        If percent >= 80 Then
            Return OccupancyHigh
        ElseIf percent >= 50 Then
            Return OccupancyMed
        End If

        Return SeatAvailable
    End Function

    'which showing is under a point on the picture, or -1 for an empty part of a lane. it uses
    'exactly the same geometry the drawing does, which is why that is worked out in one place
    Private Function ShowingAt(mouseX As Integer, mouseY As Integer) As Integer
        Dim leftEdge, topEdge, laneHeight As Integer
        Dim firstMinute, lastMinute As Integer
        Dim minuteWidth As Double
        TimelineGeometry(leftEdge, topEdge, laneHeight, minuteWidth, firstMinute, lastMinute)

        'the hours strip across the top belongs to no lane at all
        If mouseY < TimelineHeaderHeight Then
            Return -1
        End If

        'checked before the divide, because a negative divided by a positive comes out as 0 in
        'whole number division and would say the click was in the top lane when it was above it
        If mouseY < topEdge Or mouseX < leftEdge Then
            Return -1
        End If

        Dim lane As Integer = (mouseY - topEdge) \ laneHeight

        If lane < 0 Or lane > laneCount - 1 Then
            Return -1
        End If

        Dim minute As Integer = firstMinute + CInt((mouseX - leftEdge) / minuteWidth)

        For i As Integer = 0 To timelineCount - 1
            If timelineLane(i) = lane Then
                If minute >= timelineStart(i) And minute < timelineStart(i) + ScreenTimeNeeded(timelineDuration(i)) Then
                    Return i
                End If
            End If
        Next

        Return -1
    End Function

    'clicking a showing opens it in the boxes underneath. clicking an empty part of a lane fills
    'the boxes in with that screen, that day and roughly that time, which is the quickest way
    'there is of putting a film into a gap somebody has just spotted
    Private Sub pnlTimeline_MouseDown(sender As Object, e As MouseEventArgs) Handles pnlTimeline.MouseDown
        Dim hit As Integer = ShowingAt(e.X, e.Y)

        If Not ChangesCanBeLost() Then
            Exit Sub
        End If

        If hit >= 0 Then
            LoadScreeningIntoBoxes(timelineID(hit))
            Exit Sub
        End If

        StartNewScreeningAt(e.X, e.Y)
    End Sub

    'fills the boxes in ready for a new screening in the lane and at the time that was clicked
    Private Sub StartNewScreeningAt(mouseX As Integer, mouseY As Integer)
        Dim leftEdge, topEdge, laneHeight As Integer
        Dim firstMinute, lastMinute As Integer
        Dim minuteWidth As Double
        TimelineGeometry(leftEdge, topEdge, laneHeight, minuteWidth, firstMinute, lastMinute)

        If mouseY < TimelineHeaderHeight Or mouseY < topEdge Or mouseX < leftEdge Then
            Exit Sub
        End If

        Dim lane As Integer = (mouseY - topEdge) \ laneHeight

        If lane < 0 Or lane > laneCount - 1 Then
            Exit Sub
        End If

        Dim minute As Integer = firstMinute + CInt((mouseX - leftEdge) / minuteWidth)

        'nobody schedules a film at 14:23, so it is dropped back to the five minutes before it
        minute = minute - (minute Mod 5)

        If minute > LastShowMinutes Then
            minute = LastShowMinutes
        End If

        fillingBoxes = True
        selectedScreeningID = 0
        cboScreen.SelectedValue = laneScreenID(lane)
        dtpScreeningDate.Value = dtpTimelineDate.Value.Date
        txtScreeningTime.Text = MinutesAsTime(minute)
        fillingBoxes = False

        'this one counts as typing, because it really is the start of a new screening being made
        boxesChanged = True

        ShowWhatIsBeingEdited()
        ShowEndTime()
        SayDone(lblSaved, "Started a new screening in " & laneName(lane) & " at " & MinutesAsTime(minute))
    End Sub

    'the tooltip over a showing, which is where the detail lives for the blocks too narrow to
    'write in. it is only set when the mouse moves onto a different one, otherwise setting it
    'over and over makes it flicker
    Private Sub pnlTimeline_MouseMove(sender As Object, e As MouseEventArgs) Handles pnlTimeline.MouseMove
        Dim hit As Integer = ShowingAt(e.X, e.Y)

        If hit = tipShowingFor Then
            Exit Sub
        End If

        tipShowingFor = hit

        If hit < 0 Then
            timelineTips.SetToolTip(pnlTimeline, "")
            Exit Sub
        End If

        Dim finish As Integer = timelineStart(hit) + TrailerMinutes + timelineDuration(hit)
        Dim clear As Integer = timelineStart(hit) + ScreenTimeNeeded(timelineDuration(hit))

        timelineTips.SetToolTip(pnlTimeline,
            timelineTitle(hit) & vbCrLf &
            "Starts " & MinutesAsTime(timelineStart(hit)) & ", film ends " & MinutesAsTime(finish) & vbCrLf &
            "Screen free at " & MinutesAsTime(clear) & vbCrLf &
            timelineSold(hit) & " of " & timelineCapacity(hit) & " seats sold")
    End Sub

    'scrolling only repaints the part that has come into view, which would drag the hours strip
    'down the picture with it, so the whole thing is drawn again
    Private Sub pnlTimeline_Scroll(sender As Object, e As ScrollEventArgs) Handles pnlTimeline.Scroll
        pnlTimeline.Invalidate()
    End Sub

    Private Sub dtpTimelineDate_ValueChanged(sender As Object, e As EventArgs) Handles dtpTimelineDate.ValueChanged
        If stillLoading Then
            Exit Sub
        End If

        LoadTimelineDay()
    End Sub

    'the timeline is only worth reading again when it is the tab being looked at
    Private Sub tabView_SelectedIndexChanged(sender As Object, e As EventArgs) Handles tabView.SelectedIndexChanged
        If stillLoading Then
            Exit Sub
        End If

        If tabView.SelectedTab Is tabTimeline Then
            LoadTimelineDay()
        End If
    End Sub

    'called after anything is saved or deleted. the grid is always reloaded, the timeline only
    'when it is on show, so a change made from the list does not cost a query nobody will look at
    Private Sub RefreshTimelineIfShowing()
        If tabView.SelectedTab Is tabTimeline Then
            LoadTimelineDay()
        End If
    End Sub

    'turns HH:MM into the number of minutes since midnight, which makes the times easy to compare.
    'comparing them as text would say 09:00 is later than 10:00 in some cases
    Private Function TimeAsMinutes(timeText As String) As Integer
        If Not IsValidScreeningTime(timeText) Then
            Return -1
        End If

        Return (CInt(timeText.Substring(0, 2)) * 60) + CInt(timeText.Substring(3, 2))
    End Function

    'turns minutes since midnight back into HH:MM
    Private Function MinutesAsTime(minutes As Integer) As String
        'anything past midnight rolls round onto the next day
        Dim wrapped As Integer = minutes Mod (24 * 60)

        Return Format(wrapped \ 60, "00") & ":" & Format(wrapped Mod 60, "00")
    End Function

    'how long a screening ties the screen up for altogether: the adverts and trailers first, then
    'the film, then the clean up afterwards. everything that works out whether two screenings fit
    'goes through here, so they cannot disagree about it
    Private Function ScreenTimeNeeded(duration As Integer) As Integer
        Return TrailerMinutes + duration + TurnaroundMinutes
    End Function

    'when the audience actually gets out, which is the trailers plus the film. the clean up after
    'that is the cinema's problem and not something to put on a listing
    Private Function EndTimeText(startTime As String, duration As Integer) As String
        Dim startMinutes As Integer = TimeAsMinutes(startTime)

        If startMinutes < 0 Then
            Return ""
        End If

        Return MinutesAsTime(startMinutes + TrailerMinutes + duration)
    End Function

    'checks the screening time is in HH:MM format, e.g. 14:30
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

    'how long the picked film runs for
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

    'as the film and the start time are picked, it works out when the film would finish and when
    'the screen would be free for the next one
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

    'changing the show box loads the grid again
    Private Sub cboShow_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboShow.SelectedIndexChanged
        If stillLoading Then
            Exit Sub
        End If

        LoadScreenings()
    End Sub

    'finds the earliest time the picked film could go on the picked screen on the picked day without
    'running into anything already booked in. it reads everything already on that screen that day,
    'sorts it into time order, then walks along the day from opening time keeping track of how far
    'through it has got, and stops at the first gap the film actually fits in.
    'gives back the time in minutes, or -1 if there is nowhere left to put it
    Private Function NextFreeSlot() As Integer
        'worked out before the connection is opened, because it opens one of its own
        Dim needed As Integer = ScreenTimeNeeded(DurationOfPickedFilm())

        Dim starts(-1) As Integer
        Dim finishes(-1) As Integer
        Dim howMany As Integer = 0

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn

            'how many there are first, so the arrays can be made the right size
            SQLCmd.CommandText = "SELECT COUNT(*) FROM tblScreening " &
                                 "WHERE ScreenID = @ScreenID AND ScreeningDate = @ScreeningDate"
            SQLCmd.Parameters.AddWithValue("@ScreenID", CInt(cboScreen.SelectedValue))
            SQLCmd.Parameters.AddWithValue("@ScreeningDate", dtpScreeningDate.Value.Date)
            Dim onThatDay As Integer = CInt(SQLCmd.ExecuteScalar())

            If onThatDay > 0 Then
                ReDim starts(onThatDay - 1)
                ReDim finishes(onThatDay - 1)

                SQLCmd.CommandText = "SELECT tblScreening.ScreeningID, ScreeningTime, FilmDuration " &
                                     "FROM tblScreening INNER JOIN tblFilm ON tblScreening.FilmID = tblFilm.FilmID " &
                                     "WHERE tblScreening.ScreenID = @ScreenID AND ScreeningDate = @ScreeningDate"
                SQLCmd.Parameters.Clear()
                SQLCmd.Parameters.AddWithValue("@ScreenID", CInt(cboScreen.SelectedValue))
                SQLCmd.Parameters.AddWithValue("@ScreeningDate", dtpScreeningDate.Value.Date)

                Dim rs As OleDbDataReader = SQLCmd.ExecuteReader()

                While rs.Read()
                    'the one being edited is left out, otherwise it would be treated as being in
                    'its own way and the same time it already has would never be offered back
                    If CInt(rs("ScreeningID")) <> selectedScreeningID Then
                        Dim thisStart As Integer = TimeAsMinutes(rs("ScreeningTime").ToString())

                        If thisStart >= 0 Then
                            starts(howMany) = thisStart
                            finishes(howMany) = thisStart + ScreenTimeNeeded(CInt(rs("FilmDuration")))
                            howMany = howMany + 1
                        End If
                    End If
                End While

                rs.Close()
            End If

            cn.Close()
        End If

        'put them in start time order. an insertion sort is plenty here because a screen only has a
        'handful of showings in a day, and doing the sort here rather than in the query means the
        'walk below does not quietly depend on what order the database hands the rows back in
        Dim i As Integer
        For i = 1 To howMany - 1
            Dim keyStart As Integer = starts(i)
            Dim keyFinish As Integer = finishes(i)
            Dim j As Integer = i - 1

            While j >= 0 AndAlso starts(j) > keyStart
                starts(j + 1) = starts(j)
                finishes(j + 1) = finishes(j)
                j = j - 1
            End While

            starts(j + 1) = keyStart
            finishes(j + 1) = keyFinish
        Next

        'walk the day. earliest is how far through the day we have got so far, and each showing
        'either leaves a big enough gap in front of it or pushes earliest along past itself
        Dim earliest As Integer = FirstShowMinutes

        For i = 0 To howMany - 1
            'the gap has to be big enough AND the start has to be one we are allowed to use.
            'without the second half a gap late on could be offered after the last start time
            If earliest + needed <= starts(i) And earliest <= LastShowMinutes Then
                Return earliest
            End If

            If finishes(i) > earliest Then
                earliest = finishes(i)
            End If
        Next

        'nothing in the way after the last showing, so anything up to the latest start will do
        If earliest <= LastShowMinutes Then
            Return earliest
        End If

        Return -1
    End Function

    'fills the time box in with the first time the film would actually fit
    Private Sub btnSuggest_Click(sender As Object, e As EventArgs) Handles btnSuggest.Click
        If cboFilm.SelectedIndex = -1 Then
            MessageBox.Show("Pick a film first, otherwise there is no way to know how long it needs", "Screenings", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If cboScreen.SelectedIndex = -1 Then
            MessageBox.Show("Pick a screen first", "Screenings", MessageBoxButtons.OK, MessageBoxIcon.Warning)
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

    'looks for anything already on in the same screen that this screening would run into. it is
    'the same check both ways round, two screenings clash if one starts before the other has
    'finished and been cleaned up
    Private Function ClashingScreening() As String
        Dim startMinutes As Integer = TimeAsMinutes(txtScreeningTime.Text)
        Dim endMinutes As Integer = startMinutes + ScreenTimeNeeded(DurationOfPickedFilm())
        Dim clash As String = ""

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            'everything else on in that screen on that day, and how long each one runs for
            SQLCmd.CommandText = "SELECT tblScreening.ScreeningID, FilmTitle, ScreeningTime, FilmDuration " &
                                 "FROM tblScreening INNER JOIN tblFilm ON tblScreening.FilmID = tblFilm.FilmID " &
                                 "WHERE tblScreening.ScreenID = @ScreenID AND ScreeningDate = @ScreeningDate"
            SQLCmd.Parameters.AddWithValue("@ScreenID", CInt(cboScreen.SelectedValue))
            SQLCmd.Parameters.AddWithValue("@ScreeningDate", dtpScreeningDate.Value.Date)

            Dim rs As OleDbDataReader = SQLCmd.ExecuteReader()

            While rs.Read()
                'a screening never clashes with itself when it is being changed
                If CInt(rs("ScreeningID")) <> selectedScreeningID Then
                    Dim otherStart As Integer = TimeAsMinutes(rs("ScreeningTime").ToString())

                    If otherStart >= 0 Then
                        Dim otherEnd As Integer = otherStart + ScreenTimeNeeded(CInt(rs("FilmDuration")))

                        'they overlap if this one starts before the other finishes and the other
                        'starts before this one finishes
                        If startMinutes < otherEnd And otherStart < endMinutes Then
                            clash = rs("FilmTitle").ToString() & " at " & rs("ScreeningTime").ToString() &
                                    ", which is not finished and cleared until " & MinutesAsTime(otherEnd)
                        End If
                    End If
                End If
            End While

            rs.Close()
            cn.Close()
        End If

        Return clash
    End Function

    'the ticket price as it should be stored. money only goes to two decimal places, and without
    'the rounding something typed in as 6.999 was being saved exactly as it was typed
    Private Function PriceFromBox() As Double
        Return Math.Round(Val(txtTicketPrice.Text), 2)
    End Function

    'true if the very same film is already on in the very same screen at the very same time on the
    'same day. the one being edited is left out, otherwise changing the price on a screening would
    'be reported as a duplicate of itself
    Private Function SameScreeningExists() As Boolean
        Dim found As Integer = 0

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT COUNT(*) FROM tblScreening " &
                                 "WHERE FilmID = @FilmID AND ScreenID = @ScreenID " &
                                 "AND ScreeningDate = @ScreeningDate AND ScreeningTime = @ScreeningTime " &
                                 "AND ScreeningID <> @ScreeningID"
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

    'checks what has been picked and typed in before it goes anywhere near the database. it is in
    'one place because adding a screening and changing one both need the same checks doing
    Private Function DetailsAreOk(isNew As Boolean) As Boolean
        If cboFilm.SelectedIndex = -1 Or cboScreen.SelectedIndex = -1 Then
            MessageBox.Show("Pick a film and a screen", "Screenings", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If

        'a screen that has been taken out of service on the screens form is not available. the
        'combo still lists every screen on purpose, otherwise an old screening already sitting in
        'a shut room could not be opened and corrected. it is stopped here, at the save, instead
        If Not ScreenIsInService(CLng(cboScreen.SelectedValue)) Then
            MessageBox.Show("'" & cboScreen.Text & "' is out of service at the moment, so nothing can be " &
                            "scheduled in it." & vbCrLf & vbCrLf &
                            "Put it back in service on the screens form, or pick a different screen.",
                            "Screen is out of service", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            WriteLog("SCREENING", "Refused, " & cboScreen.Text & " is out of service", LogWarning)
            Return False
        End If

        'a new screening in the past is a mistake, but an old one that is being corrected has to
        'be allowed to stay where it is
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

        'the exact same showing twice is really a clash, but the clash message talks about the
        'screen already showing something else, which reads oddly when it is the same film at the
        'same time. it is worth catching first and saying plainly what has happened
        If SameScreeningExists() Then
            MessageBox.Show("That screening is already on the system." & vbCrLf & vbCrLf &
                            cboFilm.Text & " is already showing in " & cboScreen.Text & " at " &
                            txtScreeningTime.Text & " on " & dtpScreeningDate.Value.ToString("dd/MM/yyyy") & ".",
                            "Already scheduled", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If

        'two films cannot be on in the same room at the same time
        Dim clash As String = ClashingScreening()

        If clash <> "" Then
            'saying no is not much help on its own, so it works out where the film would actually
            'go and offers that instead
            Dim slot As Integer = NextFreeSlot()
            Dim advice As String = "Pick a different time or a different screen."

            If slot >= 0 Then
                advice = "The first time it would fit on that screen is " & MinutesAsTime(slot) & "." & vbCrLf &
                         "Find me a free time will put that in for you."
            End If

            MessageBox.Show("That screen is already showing " & clash & "." & vbCrLf & vbCrLf & advice,
                            "Two films at once", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            'a clash being caught is the scheduler doing its job, so it is worth having on record
            WriteLog("SCREENING", "Clash refused, " & cboScreen.Text & " is already showing " & clash & " at " & txtScreeningTime.Text, LogWarning)
            Return False
        End If

        Return True
    End Function

    'adds a new screening using the values picked and typed in
    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        If Not DetailsAreOk(True) Then
            Exit Sub
        End If

        Dim saved As Boolean = False

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "INSERT INTO tblScreening (FilmID, ScreenID, ScreeningDate, ScreeningTime, TicketPrice) " &
                                 "VALUES (@FilmID, @ScreenID, @ScreeningDate, @ScreeningTime, @TicketPrice)"
            SQLCmd.Parameters.AddWithValue("@FilmID", CInt(cboFilm.SelectedValue))
            SQLCmd.Parameters.AddWithValue("@ScreenID", CInt(cboScreen.SelectedValue))
            SQLCmd.Parameters.AddWithValue("@ScreeningDate", dtpScreeningDate.Value.Date)
            SQLCmd.Parameters.AddWithValue("@ScreeningTime", txtScreeningTime.Text)
            SQLCmd.Parameters.AddWithValue("@TicketPrice", PriceFromBox())
            SQLCmd.ExecuteNonQuery()
            cn.Close()
            saved = True
        End If

        'nothing was written if the database could not be opened, so it must not be
        'logged or announced as though it had been
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

    'saves the changes made to the screening selected in the grid
    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        'this cannot normally happen, the button is switched off until a row is picked.
        'it stays in so the sub can never run without an id, whatever calls it
        If selectedScreeningID = 0 Then
            MessageBox.Show("Select a screening in the grid first", "Screenings", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If Not DetailsAreOk(False) Then
            Exit Sub
        End If

        'moving a screening that people have already booked onto means they turn up at the wrong
        'time, so it is worth stopping to think about
        Dim sold As Integer = SeatsSold(selectedScreeningID)

        If sold > 0 Then
            If MessageBox.Show(sold & " seat(s) are already booked on this screening." & vbCrLf &
                               "Changing it will not tell those customers." & vbCrLf & vbCrLf &
                               "Carry on?", "Already booked", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) = DialogResult.No Then
                Exit Sub
            End If
        End If

        'nothing is worked out again for the bookings that already exist on this screening, and that
        'is on purpose. a booking is the price it was agreed at. if somebody paid 6.99 the cinema
        'cannot turn round later and say it was 11.99, so a new price only applies to sales made
        'after it. what each seat was charged is kept on the booking itself so this cannot change it
        Dim saved As Boolean = False

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "UPDATE tblScreening " &
                                 "SET FilmID = @FilmID, ScreenID = @ScreenID, ScreeningDate = @ScreeningDate, ScreeningTime = @ScreeningTime, TicketPrice = @TicketPrice " &
                                 "WHERE ScreeningID = @ScreeningID"
            SQLCmd.Parameters.AddWithValue("@FilmID", CInt(cboFilm.SelectedValue))
            SQLCmd.Parameters.AddWithValue("@ScreenID", CInt(cboScreen.SelectedValue))
            SQLCmd.Parameters.AddWithValue("@ScreeningDate", dtpScreeningDate.Value.Date)
            SQLCmd.Parameters.AddWithValue("@ScreeningTime", txtScreeningTime.Text)
            SQLCmd.Parameters.AddWithValue("@TicketPrice", PriceFromBox())
            SQLCmd.Parameters.AddWithValue("@ScreeningID", selectedScreeningID)
            SQLCmd.ExecuteNonQuery()
            cn.Close()
            saved = True
        End If

        'nothing was written if the database could not be opened, so it must not be
        'logged or announced as though it had been
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

    'deletes the screening selected in the grid
    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        'this cannot normally happen, the button is switched off until a row is picked.
        'it stays in so the sub can never run without an id, whatever calls it
        If selectedScreeningID = 0 Then
            MessageBox.Show("Select a screening in the grid first", "Screenings", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        'a screening people have booked onto cannot just disappear, their bookings would be left
        'pointing at a showing that is not there any more.
        'this counts the bookings and not the seats. a food only sale is still a booking on this
        'screening even though it has no seats against it, and checking the seats missed those
        Dim bookings As Integer = BookingsOnScreening(selectedScreeningID)

        If bookings > 0 Then
            Dim sold As Integer = SeatsSold(selectedScreeningID)
            MessageBox.Show("This screening has " & bookings & " booking(s) on it, " &
                            sold & " seat(s) in total." & vbCrLf &
                            "Cancel those bookings first, then the screening can be removed.",
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

        'nothing was written if the database could not be opened, so it must not be
        'logged or announced as though it had been
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

    'clears the fields and the selection
    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        ClearFields()
    End Sub

    'anything changed in the boxes by hand counts as an unsaved change
    Private Sub Details_Changed(sender As Object, e As EventArgs) Handles cboFilm.SelectedIndexChanged, cboScreen.SelectedIndexChanged,
        dtpScreeningDate.ValueChanged, txtScreeningTime.TextChanged, txtTicketPrice.TextChanged
        If fillingBoxes Then
            Exit Sub
        End If

        boxesChanged = True
    End Sub

    'asks before typing that has not been saved gets thrown away. it only asks when something has
    'actually been changed, so clicking down a list of rows to read them never interrupts
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

        'the confirmation only lasts until the next thing is started
        lblSaved.Text = ""
        selectedScreeningID = 0
        cboFilm.SelectedIndex = -1
        cboScreen.SelectedIndex = -1
        dtpScreeningDate.Value = Date.Now
        txtScreeningTime.Text = ""
        txtTicketPrice.Text = ""
        fillingBoxes = False
        boxesChanged = False

        dgvScreenings.ClearSelection()
        ShowWhatIsBeingEdited()
        ShowEndTime()
    End Sub

    'the heading over the boxes says whether a new screening is being put on or an existing one is
    'being changed. save and delete are switched off until something is picked, rather than
    'letting them be pressed and then telling the user off with a message box
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

    'when a row is clicked, load its values into the fields for editing
    Private Sub dgvScreenings_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvScreenings.CellClick
        If e.RowIndex < 0 Then Exit Sub

        'clicking a row replaces whatever is in the boxes, so anything typed and not saved
        'would have gone without a word. the selection is left alone if the answer is no
        If Not ChangesCanBeLost() Then
            Exit Sub
        End If

        fillingBoxes = True

        Dim row As DataGridViewRow = dgvScreenings.Rows(e.RowIndex)
        selectedScreeningID = CInt(row.Cells("ScreeningID").Value)
        cboFilm.SelectedValue = CInt(row.Cells("FilmID").Value)
        cboScreen.SelectedValue = CInt(row.Cells("ScreenID").Value)
        dtpScreeningDate.Value = CDate(row.Cells("ScreeningDate").Value)
        txtScreeningTime.Text = row.Cells("ScreeningTime").Value.ToString()

        'the grid shows the price with a pound sign in front of it, the box wants the plain number
        txtTicketPrice.Text = Format(CDbl(row.Cells("TicketPrice").Value), "0.00")

        fillingBoxes = False
        boxesChanged = False

        ShowWhatIsBeingEdited()
        ShowEndTime()
    End Sub

    'loads one screening into the boxes by its id. the grid can fill them in from the row that was
    'clicked because it already holds everything, but the timeline only keeps what it needs to
    'draw with, so it reads the screening back rather than carrying the price around to draw a
    'picture that never shows it
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

    'double clicking a row is the same as clicking it, except it puts the cursor straight in the
    'time box, since changing the time is far and away the most likely reason for opening one
    Private Sub dgvScreenings_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvScreenings.CellDoubleClick
        If e.RowIndex < 0 Then Exit Sub

        'the single click has already loaded the row by the time this runs, so there is nothing
        'to load again. if it did not load, because the unsaved changes question was answered no,
        'the boxes must be left exactly where they are
        If selectedScreeningID = 0 Then
            Exit Sub
        End If

        txtScreeningTime.Focus()
        txtScreeningTime.SelectAll()
    End Sub

End Class
