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

    'true while the form is setting itself up, so filling the show box does not load the grid
    'before everything is ready
    Private stillLoading As Boolean = True

    Private Sub frmScreenings_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CommonFormStartup(Me)

        LoadFilmsCombo()
        LoadScreensCombo()

        cboShow.Items.Add("Still to come")
        cboShow.Items.Add("Already been on")
        cboShow.Items.Add("Everything")

        'start on whichever the user last left it on. if the saved one is not in the list any more
        'IndexOf comes back as -1, so it falls back to still to come rather than an empty box
        cboShow.SelectedIndex = cboShow.Items.IndexOf(LastScreeningsShow)
        If cboShow.SelectedIndex = -1 Then
            cboShow.SelectedIndex = 0
        End If

        'the empty grid guard still wins over what was remembered. opening on still to come when
        'there is nothing coming up looks broken, so in that case it starts on the lot
        If cboShow.SelectedIndex = 0 And UpcomingCount() = 0 Then
            cboShow.SelectedIndex = 2
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
        SaveUserSettings()
    End Sub

    'saves the schedule as it is on screen, which is what the show filter has left showing
    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        If ExportGridToCsv(dgvScreenings, "Screenings.csv", "Screenings") Then
            WriteLog("SCREENING", "Screening list exported, " & dgvScreenings.Rows.Count & " screenings")
        End If
    End Sub

    'escape shuts the form, same as the close button on the ones that have one
    Private Sub frmScreenings_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.F5 Then
            LoadScreenings()
        ElseIf e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
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

    'loads the screenings into the grid, either the ones still to come, the ones that have been
    'and gone, or the lot
    Private Sub LoadScreenings()
        Dim dt As New DataTable

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn

            'join screening to film (for the title and how long it runs) and to screen (for the
            'name and how many seats it holds)
            Dim baseQuery As String = "SELECT tblScreening.ScreeningID, FilmTitle, ScreenName, ScreeningDate, ScreeningTime, TicketPrice, " &
                                      "FilmDuration, ScreenCapacity, tblScreening.FilmID, tblScreening.ScreenID " &
                                      "FROM (tblScreening INNER JOIN tblFilm ON tblScreening.FilmID = tblFilm.FilmID) " &
                                      "INNER JOIN tblScreen ON tblScreening.ScreenID = tblScreen.ScreenID"

            If cboShow.Text = "Still to come" Then
                SQLCmd.CommandText = baseQuery & " WHERE ScreeningDate >= @Today ORDER BY ScreeningDate, ScreeningTime, ScreeningID"
                SQLCmd.Parameters.AddWithValue("@Today", Date.Today)
            ElseIf cboShow.Text = "Already been on" Then
                SQLCmd.CommandText = baseQuery & " WHERE ScreeningDate < @Today ORDER BY ScreeningDate DESC, ScreeningTime, ScreeningID"
                SQLCmd.Parameters.AddWithValue("@Today", Date.Today)
            Else
                SQLCmd.CommandText = baseQuery & " ORDER BY ScreeningDate, ScreeningTime, ScreeningID"
            End If

            Dim da As New OleDbDataAdapter(SQLCmd)
            da.Fill(dt)
            cn.Close()
        End If

        'how full each screening is, worked out one at a time. it is the most useful thing on the
        'whole screen, it says at a glance which showings are selling and which are not
        dt.Columns.Add("SoldText", GetType(String))
        dt.Columns.Add("EndsAt", GetType(String))

        For Each row As DataRow In dt.Rows
            Dim sold As Integer = SeatsSold(CInt(row("ScreeningID")))
            Dim capacity As Integer = CInt(row("ScreenCapacity"))

            row("SoldText") = sold & " of " & capacity
            row("EndsAt") = EndTimeText(row("ScreeningTime").ToString(), CInt(row("FilmDuration")))
        Next

        dgvScreenings.DataSource = dt

        If dgvScreenings.Columns.Contains("ScreeningID") Then
            'the raw IDs and the working out columns are kept for the code but not put on show
            dgvScreenings.Columns("FilmID").Visible = False
            dgvScreenings.Columns("ScreenID").Visible = False
            dgvScreenings.Columns("FilmDuration").Visible = False
            dgvScreenings.Columns("ScreenCapacity").Visible = False

            dgvScreenings.Columns("ScreeningID").HeaderText = "ID"
            dgvScreenings.Columns("FilmTitle").HeaderText = "Film"
            dgvScreenings.Columns("ScreenName").HeaderText = "Screen"
            dgvScreenings.Columns("ScreeningDate").HeaderText = "Date"
            dgvScreenings.Columns("ScreeningTime").HeaderText = "Starts"
            dgvScreenings.Columns("EndsAt").HeaderText = "Ends"
            dgvScreenings.Columns("TicketPrice").HeaderText = "Ticket"
            dgvScreenings.Columns("SoldText").HeaderText = "Seats sold"

            dgvScreenings.Columns("ScreeningID").Width = 50
            dgvScreenings.Columns("FilmTitle").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            dgvScreenings.Columns("ScreenName").Width = 110
            dgvScreenings.Columns("ScreeningDate").Width = 110
            dgvScreenings.Columns("ScreeningTime").Width = 70
            dgvScreenings.Columns("EndsAt").Width = 70
            dgvScreenings.Columns("TicketPrice").Width = 80
            dgvScreenings.Columns("SoldText").Width = 100

            dgvScreenings.Columns("ScreeningDate").DefaultCellStyle.Format = "dd/MM/yyyy"
            dgvScreenings.Columns("TicketPrice").DefaultCellStyle.Format = "C"
            dgvScreenings.Columns("ScreeningTime").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            dgvScreenings.Columns("EndsAt").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            dgvScreenings.Columns("SoldText").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        End If

        'counting up has to happen before the sold out rows are relabelled, because relabelling
        'them replaces the numbers that are being counted
        ShowCount(dt)
        MarkSoldOutScreenings()
        dgvScreenings.ClearSelection()
    End Sub

    'a screening with every seat gone is worth seeing straight away, and one that nobody has
    'booked at all is worth knowing about too
    Private Sub MarkSoldOutScreenings()
        For Each row As DataGridViewRow In dgvScreenings.Rows
            Dim capacity As Integer = CInt(row.Cells("ScreenCapacity").Value)
            Dim sold As Integer = SoldFromText(row.Cells("SoldText").Value.ToString())

            If capacity > 0 And sold >= capacity Then
                If DarkModeOn Then
                    row.DefaultCellStyle.BackColor = Color.FromArgb(24, 60, 34)
                    row.DefaultCellStyle.ForeColor = Color.White
                Else
                    row.DefaultCellStyle.BackColor = Color.FromArgb(232, 245, 233)
                    row.DefaultCellStyle.ForeColor = Color.FromArgb(27, 94, 32)
                End If
                row.Cells("SoldText").Value = "SOLD OUT"
            End If
        Next
    End Sub

    'pulls the number sold back out of the 12 of 80 text
    Private Function SoldFromText(soldText As String) As Integer
        Dim spacePos As Integer = soldText.IndexOf(" ")

        If spacePos < 1 Then
            Return 0
        End If

        Dim firstBit As String = soldText.Substring(0, spacePos)

        'a row that has already been relabelled as SOLD OUT has no number on the front of it
        If Not IsNumeric(firstBit) Then
            Return 0
        End If

        Return CInt(firstBit)
    End Function

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
            sold = sold + SoldFromText(row("SoldText").ToString())
        Next

        lblGridCount.Text = dt.Rows.Count & " screening(s), " & sold & " seat(s) sold between them"
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

    'checks what has been picked and typed in before it goes anywhere near the database. it is in
    'one place because adding a screening and changing one both need the same checks doing
    Private Function DetailsAreOk(isNew As Boolean) As Boolean
        If cboFilm.SelectedIndex = -1 Or cboScreen.SelectedIndex = -1 Then
            MessageBox.Show("Pick a film and a screen", "Screenings", MessageBoxButtons.OK, MessageBoxIcon.Warning)
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
            SQLCmd.Parameters.AddWithValue("@TicketPrice", Val(txtTicketPrice.Text))
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
            SQLCmd.Parameters.AddWithValue("@TicketPrice", Val(txtTicketPrice.Text))
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
        ClearFields()
        SayDone(lblSaved, "Deleted the '" & savedName & "' screening")
    End Sub

    'clears the fields and the selection
    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        ClearFields()
    End Sub

    Private Sub ClearFields()
        'the confirmation only lasts until the next thing is started
        lblSaved.Text = ""
        selectedScreeningID = 0
        cboFilm.SelectedIndex = -1
        cboScreen.SelectedIndex = -1
        dtpScreeningDate.Value = Date.Now
        txtScreeningTime.Text = ""
        txtTicketPrice.Text = ""
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

        Dim row As DataGridViewRow = dgvScreenings.Rows(e.RowIndex)
        selectedScreeningID = CInt(row.Cells("ScreeningID").Value)
        cboFilm.SelectedValue = CInt(row.Cells("FilmID").Value)
        cboScreen.SelectedValue = CInt(row.Cells("ScreenID").Value)
        dtpScreeningDate.Value = CDate(row.Cells("ScreeningDate").Value)
        txtScreeningTime.Text = row.Cells("ScreeningTime").Value.ToString()

        'the grid shows the price with a pound sign in front of it, the box wants the plain number
        txtTicketPrice.Text = Format(CDbl(row.Cells("TicketPrice").Value), "0.00")

        ShowWhatIsBeingEdited()
        ShowEndTime()
    End Sub

End Class
