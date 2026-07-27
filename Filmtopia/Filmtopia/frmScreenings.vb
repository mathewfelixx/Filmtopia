Imports System.Data.OleDb

Public Class frmScreenings

    'tracks the ScreeningID of the row currently selected in the grid, 0 means nothing selected
    Private selectedScreeningID As Integer = 0

    'how long the screen is left empty after a film before the next one can start, for getting
    'everybody out and cleaning up
    Private Const TurnaroundMinutes As Integer = 15

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

        'what is still to come is what somebody usually wants, but if there is nothing coming up
        'the form would open on an empty grid looking broken, so in that case it starts on the lot
        If UpcomingCount() > 0 Then
            cboShow.SelectedIndex = 0
        Else
            cboShow.SelectedIndex = 2
        End If

        stillLoading = False

        LoadScreenings()
        ClearFields()
        WriteLog("SCREENING", "Screenings form opened")
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
                SQLCmd.CommandText = baseQuery & " WHERE ScreeningDate >= @Today ORDER BY ScreeningDate, ScreeningTime"
                SQLCmd.Parameters.AddWithValue("@Today", Date.Today)
            ElseIf cboShow.Text = "Already been on" Then
                SQLCmd.CommandText = baseQuery & " WHERE ScreeningDate < @Today ORDER BY ScreeningDate DESC, ScreeningTime"
                SQLCmd.Parameters.AddWithValue("@Today", Date.Today)
            Else
                SQLCmd.CommandText = baseQuery & " ORDER BY ScreeningDate, ScreeningTime"
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

        WriteLog("SCREENING", "Screening list loaded")
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
            SQLCmd.CommandText = "SELECT COUNT(*) FROM tblBookingSeat " &
                                 "INNER JOIN tblBooking ON tblBookingSeat.BookingID = tblBooking.BookingID " &
                                 "WHERE tblBooking.ScreeningID = @ScreeningID"
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

    'when a screening finishes, not counting the clean-up afterwards
    Private Function EndTimeText(startTime As String, duration As Integer) As String
        Dim startMinutes As Integer = TimeAsMinutes(startTime)

        If startMinutes < 0 Then
            Return ""
        End If

        Return MinutesAsTime(startMinutes + duration)
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

        lblEndsAt.Text = cboFilm.Text & " runs for " & duration & " minutes, so it would finish at " &
                         MinutesAsTime(startMinutes + duration) & "." & vbCrLf &
                         "With " & TurnaroundMinutes & " minutes to clear up, the screen is free again at " &
                         MinutesAsTime(startMinutes + duration + TurnaroundMinutes) & "."
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

    'looks for anything already on in the same screen that this screening would run into. it is
    'the same check both ways round, two screenings clash if one starts before the other has
    'finished and been cleaned up
    Private Function ClashingScreening() As String
        Dim startMinutes As Integer = TimeAsMinutes(txtScreeningTime.Text)
        Dim endMinutes As Integer = startMinutes + DurationOfPickedFilm() + TurnaroundMinutes
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
                        Dim otherEnd As Integer = otherStart + CInt(rs("FilmDuration")) + TurnaroundMinutes

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
            MessageBox.Show("Pick a film and a screen")
            Return False
        End If

        'a new screening in the past is a mistake, but an old one that is being corrected has to
        'be allowed to stay where it is
        If isNew And dtpScreeningDate.Value.Date < Date.Today Then
            MessageBox.Show("Screening date cant be in the past")
            Return False
        End If

        If txtScreeningTime.Text.Trim() = "" Then
            MessageBox.Show("Enter a screening time (HH:MM)")
            txtScreeningTime.Focus()
            Return False
        End If

        If Not IsValidScreeningTime(txtScreeningTime.Text) Then
            MessageBox.Show("Screening time must be in HH:MM format, e.g. 14:30")
            txtScreeningTime.Focus()
            Return False
        End If

        If txtTicketPrice.Text.Trim() = "" Then
            MessageBox.Show("Enter a ticket price")
            txtTicketPrice.Focus()
            Return False
        End If

        If Not IsNumeric(txtTicketPrice.Text) Then
            MessageBox.Show("Ticket price must be a number")
            txtTicketPrice.Focus()
            Return False
        End If

        If Val(txtTicketPrice.Text) <= 0 Then
            MessageBox.Show("Ticket price must be greater than 0")
            txtTicketPrice.Focus()
            Return False
        End If

        If Val(txtTicketPrice.Text) > 50 Then
            MessageBox.Show("That ticket price looks too high, it should be in pounds")
            txtTicketPrice.Focus()
            Return False
        End If

        'two films cannot be on in the same room at the same time
        Dim clash As String = ClashingScreening()

        If clash <> "" Then
            MessageBox.Show("That screen is already showing " & clash & "." & vbCrLf & vbCrLf &
                            "Pick a different time or a different screen.",
                            "Two films at once", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If

        Return True
    End Function

    'adds a new screening using the values picked and typed in
    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        If Not DetailsAreOk(True) Then
            Exit Sub
        End If

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
        End If

        WriteLog("SCREENING", "Screening added: " & cboFilm.Text & " on " & cboScreen.Text, LogChange)
        LoadScreenings()
        ClearFields()
    End Sub

    'saves the changes made to the screening selected in the grid
    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        If selectedScreeningID = 0 Then
            MessageBox.Show("Select a screening in the grid first")
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
                               "Carry on?", "Already booked", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.No Then
                Exit Sub
            End If
        End If

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
        End If

        WriteLog("SCREENING", "Screening updated: " & cboFilm.Text & " on " & cboScreen.Text, LogChange)
        LoadScreenings()
        ClearFields()
    End Sub

    'deletes the screening selected in the grid
    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If selectedScreeningID = 0 Then
            MessageBox.Show("Select a screening in the grid first")
            Exit Sub
        End If

        'a screening people have booked onto cannot just disappear, their bookings would be left
        'pointing at a showing that is not there any more
        Dim sold As Integer = SeatsSold(selectedScreeningID)

        If sold > 0 Then
            MessageBox.Show("This screening has " & sold & " seat(s) booked on it." & vbCrLf &
                            "Cancel those bookings first, then the screening can be removed.",
                            "Cannot delete", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If MessageBox.Show("Delete this screening?", "Confirm", MessageBoxButtons.YesNo) = DialogResult.No Then
            Exit Sub
        End If

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "DELETE FROM tblScreening " &
                                 "WHERE ScreeningID = @ScreeningID"
            SQLCmd.Parameters.AddWithValue("@ScreeningID", selectedScreeningID)
            SQLCmd.ExecuteNonQuery()
            cn.Close()
        End If

        WriteLog("SCREENING", "Screening deleted: ScreeningID " & selectedScreeningID, LogChange)
        LoadScreenings()
        ClearFields()
    End Sub

    'clears the fields and the selection
    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        ClearFields()
        WriteLog("SCREENING", "Screening fields cleared")
    End Sub

    Private Sub ClearFields()
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
        WriteLog("SCREENING", "Screening selected: " & cboFilm.Text & " on " & cboScreen.Text)
    End Sub

End Class
