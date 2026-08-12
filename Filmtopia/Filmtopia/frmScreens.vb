Imports System.Data.OleDb

Public Class frmScreens

    'tracks the ScreenID of the row currently selected in the grid, 0 means nothing selected
    Private selectedScreenID As Long = 0

    'the layout the selected screen had when it was clicked on. it is kept so that saving can tell
    'whether the layout has actually been changed, because the seats only need making again if it has
    Private rowsWhenPicked As Integer = 0
    Private perRowWhenPicked As Integer = 0

    'true once something has been typed into the boxes that has not been saved yet. it is what
    'the warning before another row replaces it is based on
    Private boxesChanged As Boolean = False

    'true while a row is being copied into the boxes, so filling them in does not count as typing
    Private fillingBoxes As Boolean = False

    'whether the screen showing in the panel on the right is open for business, and why it was
    'taken out of service if it is not. both come off the grid row that was clicked
    Private selectedStatus As String = ScreenInService
    Private selectedReason As String = ""

    'how many times each seat in the selected screen has been sold, laid out the same way the
    'room is. heatCounts(rowIndex, seatIndex) so heatCounts(0, 0) is seat A1. it is filled in
    'when a screen is picked and the panel just draws whatever is in it
    Private heatCounts(,) As Integer
    Private heatRows As Integer = 0
    Private heatPerRow As Integer = 0
    Private heatBusiest As Integer = 0

    'what sort each seat in the room being edited is meant to be, laid out the same way the room
    'is. planTypes(rowIndex, seatIndex) holds "Standard", "Premium" or "Accessible", so
    'planTypes(0, 0) is seat A1. it is what the seat plan tab draws and what the seats get made
    'from, and it means the premium and accessible seats no longer have to be whole fixed rows
    Private planTypes(,) As String
    Private planRows As Integer = 0
    Private planPerRow As Integer = 0

    'true once a seat on the plan has been changed and not saved yet. it is kept separate from
    'boxesChanged because saving uses it to decide whether the seats need writing to at all
    Private planChanged As Boolean = False

    Private Sub frmScreens_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If UserAccessLevel <> 1 Then
            MessageBox.Show("Only a manager can open the screens screen.", "Screens", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            WriteLog("SCREEN", "Screens screen refused, access level " & UserAccessLevel, LogSecurity)
            Me.Close()
            Exit Sub
        End If

        CommonFormStartup(Me)

        'lets the form see escape before the box that has focus does
        Me.KeyPreview = True

        'the seat popularity map is drawn rather than made out of buttons. it cannot be clicked
        'on, so five hundred buttons would be five hundred controls doing nothing
        pnlHeatmap.BackColor = Color.White

        'the seat plan is drawn the same way, but this one does get clicked on. it is one panel
        'with the seats painted into it, and the click is worked out from where it landed
        pnlSeatPlan.BackColor = Color.White

        LoadScreens()
        ClearFields()
        txtName.Focus()
        WriteLog("SCREEN", "Screens form opened")
    End Sub

    'escape shuts the form, same as the close button on the ones that have one
    Private Sub frmScreens_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.F5 Then
            LoadScreens()
        ElseIf e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub

    'loads the screens into the grid along with how many seats have actually been made for each
    'one and how many screenings it has, so a screen that is in use is obvious before anybody
    'starts changing it
    Private Sub LoadScreens()
        Dim dt As New DataTable

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT ScreenID, ScreenName, ScreenCapacity, ScreenRows, SeatsPerRow, " &
                                 "ScreenStatus, ScreenStatusReason " &
                                 "FROM tblScreen ORDER BY ScreenName, ScreenID"
            Dim da As New OleDbDataAdapter(SQLCmd)
            da.Fill(dt)
            cn.Close()
        End If

        'the extra columns are worked out a screen at a time. there are only ever a handful of
        'screens so this is quick enough, and it is far easier to follow than one big query
        dt.Columns.Add("Rows", GetType(String))
        dt.Columns.Add("Seats", GetType(Integer))
        dt.Columns.Add("Screenings", GetType(Integer))

        For Each row As DataRow In dt.Rows
            Dim screenID As Long = CLng(row("ScreenID"))

            row("Rows") = RowsAsText(CInt(row("ScreenRows")), CInt(row("SeatsPerRow")))
            row("Seats") = SeatsOnScreen(screenID)
            row("Screenings") = ScreeningsOnScreen(screenID)
        Next

        dgvScreens.DataSource = dt

        If dgvScreens.Columns.Contains("ScreenID") Then
            dgvScreens.Columns("ScreenID").HeaderText = "ID"
            dgvScreens.Columns("ScreenName").HeaderText = "Screen"
            dgvScreens.Columns("ScreenStatus").HeaderText = "Status"
            dgvScreens.Columns("ScreenCapacity").HeaderText = "Capacity"
            dgvScreens.Columns("Rows").HeaderText = "Layout"
            dgvScreens.Columns("Seats").HeaderText = "Seats made"
            dgvScreens.Columns("Screenings").HeaderText = "Screenings"

            'the two layout numbers are what the boxes underneath get filled from, they are not
            'worth a column of their own when the Layout column already says it in words
            dgvScreens.Columns("ScreenRows").Visible = False
            dgvScreens.Columns("SeatsPerRow").Visible = False

            'the reason is only worth reading one screen at a time, it is far too long for a column
            dgvScreens.Columns("ScreenStatusReason").Visible = False

            dgvScreens.Columns("ScreenID").Width = 42
            dgvScreens.Columns("ScreenName").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            dgvScreens.Columns("ScreenStatus").Width = 96
            dgvScreens.Columns("ScreenCapacity").Width = 72
            dgvScreens.Columns("Rows").Width = 116
            dgvScreens.Columns("Seats").Width = 84
            dgvScreens.Columns("Screenings").Width = 82

            dgvScreens.Columns("ScreenCapacity").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            dgvScreens.Columns("Seats").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            dgvScreens.Columns("Screenings").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        End If

        MarkScreensThatDoNotAddUp()

        'done second on purpose. a screen can be the wrong size and shut at the same time, and
        'being shut is the more important of the two to see, so it paints over the other one
        MarkScreensOutOfService()

        ShowCount(dt)
        dgvScreens.ClearSelection()
    End Sub

    'a screen whose capacity does not match the seats that were actually made is a sign something
    'went wrong when it was set up, so it is coloured in rather than left to be spotted by eye
    Private Sub MarkScreensThatDoNotAddUp()
        For Each row As DataGridViewRow In dgvScreens.Rows
            If CInt(row.Cells("ScreenCapacity").Value) <> CInt(row.Cells("Seats").Value) Then
                If DarkModeOn Then
                    row.DefaultCellStyle.BackColor = Color.FromArgb(82, 62, 12)
                    row.DefaultCellStyle.ForeColor = Color.White
                Else
                    row.DefaultCellStyle.BackColor = Color.FromArgb(255, 244, 205)
                    row.DefaultCellStyle.ForeColor = Color.FromArgb(110, 80, 0)
                End If
            End If
        Next
    End Sub

    'a screen that has been taken out of service is coloured so it stands out in the list.
    'without this the only sign would be one word in a column, and somebody scheduling a film
    'would not notice it until the save was refused
    Private Sub MarkScreensOutOfService()
        For Each row As DataGridViewRow In dgvScreens.Rows
            If StatusOfRow(row) = ScreenOutOfService Then
                If DarkModeOn Then
                    row.DefaultCellStyle.BackColor = Color.FromArgb(74, 40, 40)
                    row.DefaultCellStyle.ForeColor = Color.White
                Else
                    row.DefaultCellStyle.BackColor = Color.FromArgb(250, 226, 226)
                    row.DefaultCellStyle.ForeColor = Color.FromArgb(140, 40, 40)
                End If
            End If
        Next
    End Sub

    'reads the status off a grid row. a screen that was made before the status column existed has
    'nothing in that cell, and an empty one counts as open so nothing that used to work stops
    Private Function StatusOfRow(row As DataGridViewRow) As String
        If row.Cells("ScreenStatus").Value Is Nothing OrElse IsDBNull(row.Cells("ScreenStatus").Value) Then
            Return ScreenInService
        End If

        If row.Cells("ScreenStatus").Value.ToString().Trim() = "" Then
            Return ScreenInService
        End If

        Return row.Cells("ScreenStatus").Value.ToString()
    End Function

    'says how many screens there are and how many seats that is altogether
    Private Sub ShowCount(dt As DataTable)
        Dim seats As Integer = 0

        For Each row As DataRow In dt.Rows
            seats = seats + CInt(row("Seats"))
        Next

        'the shut ones are counted separately, because "300 seats in the building" is not true
        'if one of the rooms is closed
        Dim shut As Integer = 0

        For Each row As DataGridViewRow In dgvScreens.Rows
            If StatusOfRow(row) = ScreenOutOfService Then
                shut = shut + 1
            End If
        Next

        lblGridCount.Text = dt.Rows.Count & " screen(s), " & seats & " seats in the building"

        If shut > 0 Then
            lblGridCount.Text = lblGridCount.Text & ", " & shut & " shut"
        End If
    End Sub

    'describes a screen's layout in words, e.g. 4 rows of 12, A to D
    Private Function RowsAsText(numRows As Integer, perRow As Integer) As String
        If numRows <= 0 Or perRow <= 0 Then
            Return "none"
        End If

        If numRows = 1 Then
            Return "1 row of " & perRow & ", A"
        End If

        Return numRows & " rows of " & perRow & ", A to " & Chr(64 + numRows)
    End Function

    'counts the seats that have actually been made for a screen
    Private Function SeatsOnScreen(screenID As Long) As Integer
        Dim total As Integer = 0

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT COUNT(*) FROM tblSeat WHERE ScreenID = @ScreenID"
            SQLCmd.Parameters.AddWithValue("@ScreenID", CInt(screenID))
            total = CInt(SQLCmd.ExecuteScalar())
            cn.Close()
        End If

        Return total
    End Function

    'counts the screenings scheduled in a screen
    Private Function ScreeningsOnScreen(screenID As Long) As Integer
        Dim total As Integer = 0

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT COUNT(*) FROM tblScreening WHERE ScreenID = @ScreenID"
            SQLCmd.Parameters.AddWithValue("@ScreenID", CInt(screenID))
            total = CInt(SQLCmd.ExecuteScalar())
            cn.Close()
        End If

        Return total
    End Function

    'counts how many seats in a screen have been booked by somebody. this is what makes changing
    'the size of a screen dangerous, because making the seats again would leave those bookings
    'pointing at seats that no longer exist
    Private Function BookedSeatsOnScreen(screenID As Long) As Integer
        Dim total As Integer = 0

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT COUNT(*) FROM tblBookingSeat " &
                                 "INNER JOIN tblSeat ON tblBookingSeat.SeatID = tblSeat.SeatID " &
                                 "WHERE tblSeat.ScreenID = @ScreenID"
            SQLCmd.Parameters.AddWithValue("@ScreenID", CInt(screenID))
            total = CInt(SQLCmd.ExecuteScalar())
            cn.Close()
        End If

        Return total
    End Function

    'as either box is typed in it says what the screen will come out as, so the size and the mix of
    'seats can be seen before anything is saved
    Private Sub Layout_TextChanged(sender As Object, e As EventArgs) Handles txtRows.TextChanged, txtPerRow.TextChanged
        'the plan is rebuilt first, because the line under the boxes counts the seats up off it
        BuildSeatPlan()
        ShowLayoutPreview()
    End Sub

    Private Sub ShowLayoutPreview()
        If txtRows.Text.Trim() = "" Or txtPerRow.Text.Trim() = "" Then
            lblLayout.Text = "Type how many rows and how many seats in each row"
            Exit Sub
        End If

        If Not IsNumeric(txtRows.Text) Or Not IsNumeric(txtPerRow.Text) Then
            lblLayout.Text = "Both of those have to be numbers"
            Exit Sub
        End If

        Dim numRows As Integer = SafeInt(txtRows.Text)
        Dim perRow As Integer = SafeInt(txtPerRow.Text)

        If numRows <= 0 Or perRow <= 0 Then
            lblLayout.Text = "A screen needs at least one row with at least one seat in it"
            Exit Sub
        End If

        If numRows > 26 Then
            'the rows are lettered A to Z, so there is nowhere to go after 26
            lblLayout.Text = "The rows are lettered A to Z, so 26 rows is the most there can be"
            Exit Sub
        End If

        'count how the seats split between the three sorts so the mix can be shown. it is counted
        'off the plan rather than off the row rule, because the plan is what actually gets made
        Dim standardSeats As Integer = 0
        Dim premiumSeats As Integer = 0
        Dim accessibleSeats As Integer = 0
        CountPlannedSeats(standardSeats, premiumSeats, accessibleSeats)

        Dim mix As String = standardSeats & " standard"
        If premiumSeats > 0 Then
            mix = mix & ", " & premiumSeats & " premium"
        End If
        If accessibleSeats > 0 Then
            mix = mix & ", " & accessibleSeats & " accessible"
        End If

        lblLayout.Text = "That makes " & (numRows * perRow) & " seats, numbered A1 to " &
                         Chr(64 + numRows) & perRow & "." & vbCrLf &
                         "Seats: " & mix & "." & vbCrLf &
                         "Use the seat plan tab to move the premium and accessible ones about."
    End Sub

    'adds a new screen and makes its seats
    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        If txtName.Text.Trim() = "" Then
            MessageBox.Show("Enter a screen name", "Screens", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtName.Focus()
            Exit Sub
        End If

        If Not CapacityIsValid() Then
            Exit Sub
        End If

        If NameAlreadyUsed() Then
            SayNameIsTaken()
            Exit Sub
        End If

        Dim newScreenID As Long = 0
        Dim numRows As Integer = SafeInt(txtRows.Text)
        Dim perRow As Integer = SafeInt(txtPerRow.Text)

        'the screen and its seats go in together inside one transaction. they used to be two
        'separate connections, so if the seats failed there was a screen sitting there with nothing
        'in it and the seat map came up empty with nothing to say why
        If DbConnect() Then
            Dim trans As OleDbTransaction = cn.BeginTransaction()

            Try
                Dim SQLCmd As New OleDbCommand
                SQLCmd.Connection = cn
                SQLCmd.Transaction = trans
                'the capacity is still stored, but it is worked out from the layout rather than typed,
                'so it can no longer disagree with the number of seats that actually get made
                SQLCmd.CommandText = "INSERT INTO tblScreen (ScreenName, ScreenCapacity, ScreenRows, SeatsPerRow) " &
                                     "VALUES (@ScreenName, @ScreenCapacity, @ScreenRows, @SeatsPerRow)"
                SQLCmd.Parameters.AddWithValue("@ScreenName", txtName.Text.Trim())
                SQLCmd.Parameters.AddWithValue("@ScreenCapacity", numRows * perRow)
                SQLCmd.Parameters.AddWithValue("@ScreenRows", numRows)
                SQLCmd.Parameters.AddWithValue("@SeatsPerRow", perRow)
                SQLCmd.ExecuteNonQuery()

                'grab the ID just given to the new screen so we can generate its seats.
                'the parameters have to come off first, this query does not take any
                SQLCmd.CommandText = "SELECT @@IDENTITY"
                SQLCmd.Parameters.Clear()
                newScreenID = CLng(SQLCmd.ExecuteScalar())

                GenerateSeats(SQLCmd, newScreenID, numRows, perRow)

                trans.Commit()

            Catch ex As Exception
                trans.Rollback()
                newScreenID = 0
                MessageBox.Show("The screen could not be saved, so nothing at all was written. " & ex.Message,
                                "Screen", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try

            cn.Close()
        End If

        'kept because ClearFields empties the box before there is a chance to say what was saved
        Dim savedName As String = txtName.Text.Trim()

        'logging waits until the connection is shut, WriteLog opens its own
        If newScreenID > 0 Then
            Dim standardSeats As Integer = 0
            Dim premiumSeats As Integer = 0
            Dim accessibleSeats As Integer = 0
            CountPlannedSeats(standardSeats, premiumSeats, accessibleSeats)

            WriteLog("SCREEN", "Screen added: " & savedName, LogChange)
            WriteLog("SCREEN", "Seats generated for ScreenID " & newScreenID & ", " & numRows & " row(s) of " &
                               perRow & ", " & (numRows * perRow) & " seats (" & standardSeats & " standard, " &
                               premiumSeats & " premium, " & accessibleSeats & " accessible)", LogChange)
        End If

        LoadScreens()
        ClearFields()

        If newScreenID > 0 Then
            SayDone(lblSaved, "Added '" & savedName & "' with " & (numRows * perRow) & " seats")
        End If
    End Sub

    'saves the changes made to the screen selected in the grid
    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        'this cannot normally happen, the button is switched off until a row is picked.
        'it stays in so the sub can never run without an id, whatever calls it
        If selectedScreenID = 0 Then
            MessageBox.Show("Select a screen in the grid first", "Screens", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If txtName.Text.Trim() = "" Then
            MessageBox.Show("Enter a screen name", "Screens", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtName.Focus()
            Exit Sub
        End If

        If Not CapacityIsValid() Then
            Exit Sub
        End If

        If NameAlreadyUsed() Then
            SayNameIsTaken()
            Exit Sub
        End If

        Dim newRows As Integer = SafeInt(txtRows.Text)
        Dim newPerRow As Integer = SafeInt(txtPerRow.Text)
        Dim newCapacity As Integer = newRows * newPerRow

        'the seats are only worth making again if the layout has actually changed. before, renaming
        'a screen wiped all of its seats and made them again for no reason.
        'the rows and the seats per row are both checked, not just the total, because 6 rows of 10
        'and 10 rows of 6 are the same number of seats but a completely different room
        Dim capacityChanged As Boolean = (newRows <> rowsWhenPicked Or newPerRow <> perRowWhenPicked)

        If capacityChanged Then
            'making the seats again means deleting the old ones, and anything already booked in
            'this screen is booked against one of those seats, so it has to be stopped
            Dim booked As Integer = BookedSeatsOnScreen(selectedScreenID)

            If booked > 0 Then
                MessageBox.Show("This screen has " & booked & " seat(s) already booked." & vbCrLf &
                                "Changing how many seats it has would mean making them all again, and those " &
                                "bookings would be left pointing at seats that no longer exist." & vbCrLf & vbCrLf &
                                "Cancel those bookings first, or leave the number of seats as it is.",
                                "Cannot resize this screen", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            If MessageBox.Show("Changing the number of seats will make all of this screen's seats again." & vbCrLf &
                               "Carry on?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = DialogResult.No Then
                Exit Sub
            End If
        End If

        'changing the plan on a room that is already selling is allowed, but it is worth saying
        'what it does. tickets already sold keep the price they were sold at, that is stored on
        'the booking, so this only changes what the seat costs from now on
        If planChanged And Not capacityChanged Then
            If BookedSeatsOnScreen(selectedScreenID) > 0 Then
                If MessageBox.Show("This screen has seats already booked." & vbCrLf &
                                   "Changing what sort a seat is changes what it costs from now on. Tickets " &
                                   "already sold keep the price they were sold at." & vbCrLf & vbCrLf &
                                   "Carry on?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                                   MessageBoxDefaultButton.Button2) = DialogResult.No Then
                    Exit Sub
                End If
            End If
        End If

        'this is the most damaging thing the program can do. resizing a screen throws all of its
        'seats away and makes them again, and that used to happen on three separate connections, so
        'a failure after the delete left the room with no seats at all and no way of getting them
        'back. all of it is one transaction now, so either the whole resize happens or none of it
        Dim saved As Boolean = False

        If DbConnect() Then
            Dim trans As OleDbTransaction = cn.BeginTransaction()

            Try
                Dim SQLCmd As New OleDbCommand
                SQLCmd.Connection = cn
                SQLCmd.Transaction = trans
                SQLCmd.CommandText = "UPDATE tblScreen " &
                                     "SET ScreenName = @ScreenName, ScreenCapacity = @ScreenCapacity, " &
                                     "ScreenRows = @ScreenRows, SeatsPerRow = @SeatsPerRow " &
                                     "WHERE ScreenID = @ScreenID"
                SQLCmd.Parameters.AddWithValue("@ScreenName", txtName.Text.Trim())
                SQLCmd.Parameters.AddWithValue("@ScreenCapacity", newCapacity)
                SQLCmd.Parameters.AddWithValue("@ScreenRows", newRows)
                SQLCmd.Parameters.AddWithValue("@SeatsPerRow", newPerRow)
                SQLCmd.Parameters.AddWithValue("@ScreenID", CInt(selectedScreenID))
                SQLCmd.ExecuteNonQuery()

                'only remake the seats if the layout actually changed, not if just the name did.
                'if the room is the same size but the plan has been marked out differently, the
                'seats stay exactly where they are and only the sort on each one is written
                If capacityChanged Then
                    DeleteSeats(SQLCmd, selectedScreenID)
                    GenerateSeats(SQLCmd, selectedScreenID, newRows, newPerRow)
                ElseIf planChanged Then
                    SaveSeatTypes(SQLCmd, selectedScreenID, newRows, newPerRow)
                End If

                trans.Commit()
                saved = True

            Catch ex As Exception
                trans.Rollback()
                MessageBox.Show("The screen could not be saved, so it has been left exactly as it was. " & ex.Message,
                                "Screen", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try

            cn.Close()
        End If

        Dim savedName As String = txtName.Text.Trim()

        If saved Then
            WriteLog("SCREEN", "Screen updated: " & savedName, LogChange)

            If capacityChanged Then
                WriteLog("SCREEN", "Seats generated for ScreenID " & selectedScreenID & ", " & newRows & " row(s) of " &
                                   newPerRow & ", " & (newRows * newPerRow) & " seats", LogChange)
            ElseIf planChanged Then
                Dim standardSeats As Integer = 0
                Dim premiumSeats As Integer = 0
                Dim accessibleSeats As Integer = 0
                CountPlannedSeats(standardSeats, premiumSeats, accessibleSeats)
                WriteLog("SCREEN", "Seat plan changed for ScreenID " & selectedScreenID & ", now " &
                                   standardSeats & " standard, " & premiumSeats & " premium, " &
                                   accessibleSeats & " accessible", LogChange)
            End If
        End If

        LoadScreens()
        ClearFields()

        If saved Then
            SayDone(lblSaved, "Saved changes to '" & savedName & "'")
        End If
    End Sub

    'deletes the screen selected in the grid
    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        'this cannot normally happen, the button is switched off until a row is picked.
        'it stays in so the sub can never run without an id, whatever calls it
        If selectedScreenID = 0 Then
            MessageBox.Show("Select a screen in the grid first", "Screens", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        'a screen with something scheduled in it cannot go, those screenings would be left in a
        'room that does not exist
        Dim screenings As Integer = ScreeningsOnScreen(selectedScreenID)

        If screenings > 0 Then
            MessageBox.Show("'" & txtName.Text & "' has " & screenings & " screening(s) scheduled in it." & vbCrLf &
                            "Delete those screenings first, then the screen can be removed.",
                            "Cannot delete", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            WriteLog("SCREEN", "Delete refused for '" & txtName.Text & "', it has " & screenings & " screening(s)", LogWarning)
            Exit Sub
        End If

        Dim booked As Integer = BookedSeatsOnScreen(selectedScreenID)

        If booked > 0 Then
            MessageBox.Show("'" & txtName.Text & "' has " & booked & " seat(s) that are booked." & vbCrLf &
                            "Cancel those bookings first, then the screen can be removed.",
                            "Cannot delete", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            WriteLog("SCREEN", "Delete refused for '" & txtName.Text & "', it has " & booked & " booked seat(s)", LogWarning)
            Exit Sub
        End If

        If MessageBox.Show("Delete '" & txtName.Text & "' and all of its seats?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = DialogResult.No Then
            Exit Sub
        End If

        'the seats have to go before the screen does, the database will not allow it the other way
        'round. both together in one transaction so a screen can never be left with orphan seats
        Dim deleted As Boolean = False

        If DbConnect() Then
            Dim trans As OleDbTransaction = cn.BeginTransaction()

            Try
                Dim SQLCmd As New OleDbCommand
                SQLCmd.Connection = cn
                SQLCmd.Transaction = trans

                DeleteSeats(SQLCmd, selectedScreenID)

                SQLCmd.CommandText = "DELETE FROM tblScreen " &
                                     "WHERE ScreenID = @ScreenID"
                SQLCmd.Parameters.Clear()
                SQLCmd.Parameters.AddWithValue("@ScreenID", CInt(selectedScreenID))
                SQLCmd.ExecuteNonQuery()

                trans.Commit()
                deleted = True

            Catch ex As Exception
                trans.Rollback()
                MessageBox.Show("The screen could not be deleted, so nothing has been removed. " & ex.Message,
                                "Screen", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try

            cn.Close()
        End If

        Dim savedName As String = txtName.Text.Trim()

        If deleted Then
            WriteLog("SCREEN", "Screen deleted: " & savedName, LogChange)
        End If

        LoadScreens()
        ClearFields()

        If deleted Then
            SayDone(lblSaved, "Deleted '" & savedName & "'")
        End If
    End Sub

    'checks the two layout boxes. there is no multiple of ten rule any more, because the rows and
    'the seats in them are given separately, so any size of screen works out exactly
    Private Function CapacityIsValid() As Boolean
        If Not IsNumeric(txtRows.Text) Then
            MessageBox.Show("How many rows has to be a number", "Screens", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtRows.Focus()
            Return False
        End If

        If Not IsNumeric(txtPerRow.Text) Then
            MessageBox.Show("How many seats in a row has to be a number", "Screens", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtPerRow.Focus()
            Return False
        End If

        Dim numRows As Integer = SafeInt(txtRows.Text)
        Dim perRow As Integer = SafeInt(txtPerRow.Text)

        If numRows <= 0 Then
            MessageBox.Show("A screen needs at least one row", "Screens", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtRows.Focus()
            Return False
        End If

        If perRow <= 0 Then
            MessageBox.Show("A row needs at least one seat in it", "Screens", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtPerRow.Focus()
            Return False
        End If

        'twenty six rows is as far as the letters go
        If numRows > 26 Then
            MessageBox.Show("The rows are lettered A to Z, so a screen cannot have more than 26 rows", "Screens", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtRows.Focus()
            Return False
        End If

        'the seat map draws a button per seat across the panel, so a silly wide row would run off
        'the side of it
        If perRow > 20 Then
            MessageBox.Show("A row cannot have more than 20 seats in it, they would not fit on the seat map", "Screens", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtPerRow.Focus()
            Return False
        End If

        Return True
    End Function

    'says whether another screen already has this name. screens are picked by name all over the
    'program, on the screenings form and the door list, so two called Screen 2 would be guesswork.
    'the screen being edited is left out of the count so renaming nothing does not trip over itself
    Private Function NameAlreadyUsed() As Boolean
        Dim total As Integer = 0

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT COUNT(*) FROM tblScreen " &
                                 "WHERE ScreenName = @ScreenName AND ScreenID <> @ScreenID"
            SQLCmd.Parameters.AddWithValue("@ScreenName", txtName.Text.Trim())
            SQLCmd.Parameters.AddWithValue("@ScreenID", CInt(selectedScreenID))
            total = CInt(SQLCmd.ExecuteScalar())
            cn.Close()
        End If

        Return total > 0
    End Function

    'the message for when the name is taken, said the same way whether adding or changing
    Private Sub SayNameIsTaken()
        MessageBox.Show("There is already a screen called '" & txtName.Text.Trim() & "'." & vbCrLf &
                        "Screens are picked by name everywhere else, so two the same cannot be told apart.",
                        "Name already used", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        txtName.Focus()
    End Sub

    'clears the boxes and the selection
    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        ClearFields()
    End Sub

    'anything changed in the boxes by hand counts as an unsaved change
    Private Sub Details_Changed(sender As Object, e As EventArgs) Handles txtName.TextChanged, txtRows.TextChanged,
        txtPerRow.TextChanged
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
        selectedScreenID = 0
        rowsWhenPicked = 0
        perRowWhenPicked = 0
        txtName.Text = ""
        txtRows.Text = ""
        txtPerRow.Text = ""
        fillingBoxes = False
        boxesChanged = False
        planChanged = False

        selectedStatus = ScreenInService
        selectedReason = ""

        dgvScreens.ClearSelection()
        ShowWhatIsBeingEdited()
        BuildSeatPlan()
        ShowLayoutPreview()
        ClearScreenDetail()
    End Sub

    'the heading over the boxes says whether a new screen is being typed in or an existing one is
    'being changed. save and delete are switched off until something is picked, rather than
    'letting them be pressed and then telling the user off with a message box
    Private Sub ShowWhatIsBeingEdited()
        If selectedScreenID = 0 Then
            lblStatus.Text = "Adding a new screen"
            btnUpdate.Enabled = False
            btnDelete.Enabled = False
            btnAdd.Enabled = True
        Else
            lblStatus.Text = "Editing: " & txtName.Text
            btnUpdate.Enabled = True
            btnDelete.Enabled = True
            btnAdd.Enabled = False
        End If
    End Sub

    'makes a row of 10 seats for every 10 seats of capacity, rows go A, B, C...
    'makes the seats for a screen from how many rows it has and how many seats are in each row.
    'it used to work the rows out as capacity \ 10, which threw away the remainder, so asking for
    '95 seats quietly made 90. the number of rows and the seats in each are now both given, so
    'the seats made always come to exactly rows times seats per row
    'the command is passed in already connected and inside a transaction, because making the seats
    'has to succeed or fail together with whatever is being done to the screen itself
    Private Sub GenerateSeats(SQLCmd As OleDbCommand, screenID As Long, numRows As Integer, perRow As Integer)
        'read the seat types once at the start rather than looking one up for every seat
        SQLCmd.CommandText = "SELECT SeatTypeID, SeatTypeName FROM tblSeatType"
        SQLCmd.Parameters.Clear()
        Dim dtTypes As New DataTable
        Dim rs As OleDbDataReader = SQLCmd.ExecuteReader()
        dtTypes.Load(rs)
        rs.Close()

        SQLCmd.CommandText = "INSERT INTO tblSeat (ScreenID, SeatRow, SeatNumber, SeatTypeID) " &
                             "VALUES (@ScreenID, @SeatRow, @SeatNumber, @SeatTypeID)"
        SQLCmd.Parameters.Clear()
        SQLCmd.Parameters.AddWithValue("@ScreenID", CInt(screenID))
        SQLCmd.Parameters.AddWithValue("@SeatRow", "")
        SQLCmd.Parameters.AddWithValue("@SeatNumber", 0)
        SQLCmd.Parameters.AddWithValue("@SeatTypeID", 0)

        'the sort is now looked up a seat at a time rather than a row at a time, because the plan
        'on the seat plan tab can have premium and accessible seats anywhere in the room
        For rowIndex As Integer = 0 To numRows - 1
            Dim rowLetter As String = Chr(65 + rowIndex)

            For seatNum As Integer = 1 To perRow
                Dim typeID As Long = TypeIDFromTable(dtTypes, PlannedTypeFor(rowIndex, seatNum - 1, numRows))
                SQLCmd.Parameters("@SeatRow").Value = rowLetter
                SQLCmd.Parameters("@SeatNumber").Value = seatNum
                SQLCmd.Parameters("@SeatTypeID").Value = CInt(typeID)
                SQLCmd.ExecuteNonQuery()
            Next
        Next
    End Sub

    'finds the id of a seat type in the little table that was read at the start
    Private Function TypeIDFromTable(dtTypes As DataTable, typeName As String) As Long
        For Each row As DataRow In dtTypes.Rows
            If row("SeatTypeName").ToString() = typeName Then
                Return CLng(row("SeatTypeID"))
            End If
        Next

        Return 0
    End Function

    'works out what sort of seat a row is. the back two rows of a screen are the premium ones,
    'which is how most cinemas do it because the view from the back is better, and the front row
    'is the accessible one because it is the easiest to get to. everything else is standard.
    'the rule lives in one function so the seat map, the preview and the seat making all agree
    Private Function SeatTypeForRow(rowIndex As Integer, numRows As Integer) As String
        'a really small screen has no room to set rows aside, so it is all standard
        If numRows < 4 Then
            Return SeatStandard
        End If

        If rowIndex = 0 Then
            Return SeatAccessible
        End If

        If rowIndex >= numRows - 2 Then
            Return SeatPremium
        End If

        Return SeatStandard
    End Function

    'removes every seat that belongs to a screen. same as GenerateSeats, the command comes in
    'already inside a transaction so the delete can be undone if what follows it goes wrong
    Private Sub DeleteSeats(SQLCmd As OleDbCommand, screenID As Long)
        SQLCmd.CommandText = "DELETE FROM tblSeat " &
                             "WHERE ScreenID = @ScreenID"
        SQLCmd.Parameters.Clear()
        SQLCmd.Parameters.AddWithValue("@ScreenID", CInt(screenID))
        SQLCmd.ExecuteNonQuery()
    End Sub

    'when a row is clicked, load its values into the boxes for editing
    Private Sub dgvScreens_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvScreens.CellClick
        If e.RowIndex < 0 Then Exit Sub

        'clicking a row replaces whatever is in the boxes, so anything typed and not saved
        'would have gone without a word. the selection is left alone if the answer is no
        If Not ChangesCanBeLost() Then
            Exit Sub
        End If

        fillingBoxes = True

        Dim row As DataGridViewRow = dgvScreens.Rows(e.RowIndex)
        selectedScreenID = CLng(row.Cells("ScreenID").Value)
        txtName.Text = row.Cells("ScreenName").Value.ToString()
        txtRows.Text = row.Cells("ScreenRows").Value.ToString()
        txtPerRow.Text = row.Cells("SeatsPerRow").Value.ToString()

        'remembered so saving can tell whether the layout has been changed or only the name
        rowsWhenPicked = CInt(row.Cells("ScreenRows").Value)
        perRowWhenPicked = CInt(row.Cells("SeatsPerRow").Value)

        selectedStatus = StatusOfRow(row)
        selectedReason = ReasonOfRow(row)

        fillingBoxes = False
        boxesChanged = False

        ShowWhatIsBeingEdited()
        ShowScreenDetail()
    End Sub

    'reads the out of service reason off a grid row, empty if there is not one
    Private Function ReasonOfRow(row As DataGridViewRow) As String
        If row.Cells("ScreenStatusReason").Value Is Nothing OrElse IsDBNull(row.Cells("ScreenStatusReason").Value) Then
            Return ""
        End If

        Return row.Cells("ScreenStatusReason").Value.ToString()
    End Function

    'puts the grid selection back on a screen after the grid has been reloaded, so changing a
    'screen's status does not throw away what is showing in the panel on the right
    Private Sub SelectScreenInGrid(screenID As Long)
        For Each row As DataGridViewRow In dgvScreens.Rows
            If CLng(row.Cells("ScreenID").Value) = screenID Then
                row.Selected = True
                selectedStatus = StatusOfRow(row)
                selectedReason = ReasonOfRow(row)
                Exit For
            End If
        Next
    End Sub

    '=============================================================================
    'everything below here is the panel on the right, which is about looking after a screen that
    'already exists rather than making a new one. the boxes on the left change what a screen is,
    'this side says how it is doing and whether it is open
    '=============================================================================

    'fills the whole right hand panel in for whichever screen is picked in the grid
    Private Sub ShowScreenDetail()
        If selectedScreenID = 0 Then
            ClearScreenDetail()
            Exit Sub
        End If

        lblPickedScreen.Text = txtName.Text

        LoadOverview()
        LoadHeatmap()
        LoadSeatPlanFromScreen()
        LoadScreeningsForScreen()
        ShowStatusButtons()
    End Sub

    'empties the right hand panel when nothing is picked, so it never shows numbers belonging to
    'a screen that is no longer selected
    Private Sub ClearScreenDetail()
        lblPickedScreen.Text = "Pick a screen in the grid"
        lblOverview.Text = ""
        lblScreenState.Text = ""
        lblStatusHint.Text = ""
        lblHeatmapInfo.Text = ""
        lblHeatmapKey.Text = ""
        lblScreeningsInfo.Text = ""
        txtReason.Text = ""
        dgvScreenings.DataSource = Nothing

        btnOutOfService.Enabled = False
        btnBackInService.Enabled = False
        txtReason.Enabled = False

        heatRows = 0
        heatPerRow = 0
        heatBusiest = 0
        pnlHeatmap.Invalidate()
    End Sub

    'the block of numbers at the top of the overview tab
    Private Sub LoadOverview()
        Dim seatsMade As Integer = SeatsOnScreen(selectedScreenID)
        Dim screenings As Integer = ScreeningsOnScreen(selectedScreenID)
        Dim upcoming As Integer = UpcomingScreeningsOnScreen(selectedScreenID)
        Dim sold As Integer = BookedSeatsOnScreen(selectedScreenID)
        Dim takings As Double = TakingsOnScreen(selectedScreenID)

        'how full the room usually gets. every screening put the whole room on sale, so the seats
        'that could have been sold is the number of screenings times the size of the room
        Dim couldHaveSold As Integer = screenings * seatsMade
        Dim howFull As String = "no screenings yet"

        If couldHaveSold > 0 Then
            howFull = Math.Round(sold * 100.0 / couldHaveSold, 1) & "% full on average"
        End If

        'a tab character does not line up in a label the way it does in a text box, so each line
        'names the thing it is showing instead of trying to make two columns out of it
        Dim lines As String = ""
        lines = lines & "Layout:  " & RowsAsText(SafeInt(txtRows.Text), SafeInt(txtPerRow.Text)) & vbCrLf
        lines = lines & "Seats made:  " & seatsMade & vbCrLf
        lines = lines & "Screenings:  " & screenings & " altogether, " & upcoming & " to come" & vbCrLf & vbCrLf
        lines = lines & "Tickets sold:  " & sold & vbCrLf
        lines = lines & "Ticket takings:  " & Format(takings, "Currency") & vbCrLf
        lines = lines & "How full:  " & howFull & vbCrLf
        lines = lines & "Busiest time:  " & BusiestTimeSlot(selectedScreenID) & vbCrLf & vbCrLf
        lines = lines & "Sold and takings count live bookings only." & vbCrLf
        lines = lines & "A cancelled booking gives its seats back."

        lblOverview.Text = lines
    End Sub

    'counts the screenings in a screen that have not been on yet
    Private Function UpcomingScreeningsOnScreen(screenID As Long) As Integer
        Dim total As Integer = 0

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT COUNT(*) FROM tblScreening " &
                                 "WHERE ScreenID = @ScreenID AND ScreeningDate >= @Today"
            SQLCmd.Parameters.AddWithValue("@ScreenID", CInt(screenID))
            SQLCmd.Parameters.AddWithValue("@Today", Date.Today)
            total = CInt(SQLCmd.ExecuteScalar())
            cn.Close()
        End If

        Return total
    End Function

    'adds up what the tickets sold in a screen actually took. it sums SeatPricePaid off the seat
    'rows rather than working the price out again, because a ticket is the price it was sold at
    'and a later price change must not reach back and rewrite what a screen took last month
    Private Function TakingsOnScreen(screenID As Long) As Double
        Dim total As Double = 0

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT SUM(SeatPricePaid) FROM tblBookingSeat " &
                                 "INNER JOIN tblSeat ON tblBookingSeat.SeatID = tblSeat.SeatID " &
                                 "WHERE tblSeat.ScreenID = @ScreenID"
            SQLCmd.Parameters.AddWithValue("@ScreenID", CInt(screenID))
            Dim answer As Object = SQLCmd.ExecuteScalar()

            'SUM comes back empty rather than zero when there is nothing to add up
            If answer IsNot Nothing AndAlso Not IsDBNull(answer) Then
                total = CDbl(answer)
            End If

            cn.Close()
        End If

        Return total
    End Function

    'works out which time of day fills this screen up the best. the screenings and the seats sold
    'are read in two goes and then matched up by hand, because what is wanted is an average per
    'time of day and there is no one query that gives that without getting clever
    Private Function BusiestTimeSlot(screenID As Long) As String
        Dim dtScreenings As New DataTable
        Dim dtSold As New DataTable

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT ScreeningID, ScreeningTime FROM tblScreening " &
                                 "WHERE ScreenID = @ScreenID"
            SQLCmd.Parameters.AddWithValue("@ScreenID", CInt(screenID))
            Dim da As New OleDbDataAdapter(SQLCmd)
            da.Fill(dtScreenings)
            cn.Close()
        End If

        If dtScreenings.Rows.Count = 0 Then
            Return "nothing has been on yet"
        End If

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            'tblBookingSeat carries the ScreeningID itself, so the seats sold can be counted
            'without joining back through the booking
            SQLCmd.CommandText = "SELECT tblBookingSeat.ScreeningID FROM tblBookingSeat " &
                                 "INNER JOIN tblScreening ON tblBookingSeat.ScreeningID = tblScreening.ScreeningID " &
                                 "WHERE tblScreening.ScreenID = @ScreenID"
            SQLCmd.Parameters.AddWithValue("@ScreenID", CInt(screenID))
            Dim da As New OleDbDataAdapter(SQLCmd)
            da.Fill(dtSold)
            cn.Close()
        End If

        'one entry per time of day. slotTimes holds the time, slotSold how many seats went at that
        'time altogether and slotShows how many screenings there were, so an average can be worked
        'out at the end. three arrays kept side by side, the same position in each is the same slot
        Dim slotTimes(0) As String
        Dim slotSold(0) As Integer
        Dim slotShows(0) As Integer
        Dim slotCount As Integer = 0

        For Each screening As DataRow In dtScreenings.Rows
            Dim thisTime As String = screening("ScreeningTime").ToString()
            Dim thisID As Long = CLng(screening("ScreeningID"))

            'count the seats sold for this screening by going through the sold rows
            Dim soldHere As Integer = 0

            For Each seat As DataRow In dtSold.Rows
                If CLng(seat("ScreeningID")) = thisID Then
                    soldHere = soldHere + 1
                End If
            Next

            'find the slot this time already has, or start a new one for it
            Dim slot As Integer = -1

            For i As Integer = 0 To slotCount - 1
                If slotTimes(i) = thisTime Then
                    slot = i
                    Exit For
                End If
            Next

            If slot = -1 Then
                slot = slotCount
                slotCount = slotCount + 1
                ReDim Preserve slotTimes(slotCount)
                ReDim Preserve slotSold(slotCount)
                ReDim Preserve slotShows(slotCount)
                slotTimes(slot) = thisTime
                slotSold(slot) = 0
                slotShows(slot) = 0
            End If

            slotSold(slot) = slotSold(slot) + soldHere
            slotShows(slot) = slotShows(slot) + 1
        Next

        'now pick whichever slot sold the most seats per screening
        Dim bestSlot As Integer = -1
        Dim bestAverage As Double = -1

        For i As Integer = 0 To slotCount - 1
            Dim average As Double = slotSold(i) / slotShows(i)

            If average > bestAverage Then
                bestAverage = average
                bestSlot = i
            End If
        Next

        If bestSlot = -1 Or bestAverage <= 0 Then
            Return "nothing sold yet"
        End If

        Return slotTimes(bestSlot) & ", " & Math.Round(bestAverage, 1) & " seats a showing"
    End Function

    '=============================================================================
    'the seat popularity map
    '=============================================================================

    'counts how many times every seat in the screen has been sold and puts the answers into
    'heatCounts, laid out the same way the room is. the seats and the sold rows are read in two
    'goes and matched up in a loop rather than being counted with a GROUP BY, because every seat
    'has to end up in the grid, including the ones nobody has ever picked
    Private Sub LoadHeatmap()
        Dim dtSeats As New DataTable
        Dim dtSold As New DataTable

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT SeatID, SeatRow, SeatNumber FROM tblSeat " &
                                 "WHERE ScreenID = @ScreenID ORDER BY SeatRow, SeatNumber"
            SQLCmd.Parameters.AddWithValue("@ScreenID", CInt(selectedScreenID))
            Dim da As New OleDbDataAdapter(SQLCmd)
            da.Fill(dtSeats)
            cn.Close()
        End If

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT tblBookingSeat.SeatID FROM tblBookingSeat " &
                                 "INNER JOIN tblSeat ON tblBookingSeat.SeatID = tblSeat.SeatID " &
                                 "WHERE tblSeat.ScreenID = @ScreenID"
            SQLCmd.Parameters.AddWithValue("@ScreenID", CInt(selectedScreenID))
            Dim da As New OleDbDataAdapter(SQLCmd)
            da.Fill(dtSold)
            cn.Close()
        End If

        heatRows = SafeInt(txtRows.Text)
        heatPerRow = SafeInt(txtPerRow.Text)
        heatBusiest = 0

        If heatRows <= 0 Or heatPerRow <= 0 Or dtSeats.Rows.Count = 0 Then
            heatRows = 0
            heatPerRow = 0
            lblHeatmapInfo.Text = "This screen has no seats made for it yet."
            lblHeatmapKey.Text = ""
            pnlHeatmap.Invalidate()
            Exit Sub
        End If

        ReDim heatCounts(heatRows - 1, heatPerRow - 1)

        'go through every seat in the room and count how many of the sold rows point at it.
        'it is a loop inside a loop, which is slower than letting the database group it, but a
        'screen is a few hundred seats at most and this way the seats nobody booked are still there
        Dim totalSold As Integer = 0

        For Each seat As DataRow In dtSeats.Rows
            Dim seatID As Long = CLng(seat("SeatID"))
            Dim rowIndex As Integer = Asc(seat("SeatRow").ToString().ToUpper()) - 65
            Dim seatIndex As Integer = CInt(seat("SeatNumber")) - 1

            'a seat left over from an older, bigger layout would fall outside the grid
            If rowIndex >= 0 And rowIndex < heatRows And seatIndex >= 0 And seatIndex < heatPerRow Then
                Dim timesSold As Integer = 0

                For Each soldRow As DataRow In dtSold.Rows
                    If CLng(soldRow("SeatID")) = seatID Then
                        timesSold = timesSold + 1
                    End If
                Next

                heatCounts(rowIndex, seatIndex) = timesSold
                totalSold = totalSold + timesSold

                If timesSold > heatBusiest Then
                    heatBusiest = timesSold
                End If
            End If
        Next

        lblHeatmapInfo.Text = "How often each seat has been sold, across" & vbCrLf &
                              "every screening. " & totalSold & " ticket(s) altogether."

        If heatBusiest = 0 Then
            lblHeatmapKey.Text = "Nothing has been sold in this screen yet," & vbCrLf &
                                 "so every seat is grey." & vbCrLf &
                                 "Cancelled tickets are not counted, their" & vbCrLf &
                                 "seats went back on sale."
        Else
            lblHeatmapKey.Text = "Grey means never sold. The colour warms" & vbCrLf &
                                 "towards red the more often a seat has gone." & vbCrLf &
                                 "The busiest seat here has gone " & heatBusiest & " time(s)." & vbCrLf &
                                 "Cancelled tickets are not counted."
        End If

        pnlHeatmap.Invalidate()
    End Sub

    'draws the seat popularity map. it is painted rather than made out of buttons because nothing
    'on it can be clicked, so there is no reason for it to be hundreds of controls
    Private Sub pnlHeatmap_Paint(sender As Object, e As PaintEventArgs) Handles pnlHeatmap.Paint
        Dim g As Graphics = e.Graphics
        g.Clear(Color.White)

        If heatRows <= 0 Or heatPerRow <= 0 Then
            g.DrawString("No seats to show", New Font("Segoe UI", 9), Brushes.Gray, 10, 10)
            Exit Sub
        End If

        'room is left down the side for the row letters and along the top for the screen
        Dim letterWidth As Integer = 22
        Dim screenBar As Integer = 22
        Dim usableWidth As Integer = pnlHeatmap.Width - letterWidth - 6
        Dim usableHeight As Integer = pnlHeatmap.Height - screenBar - 6

        'every seat is the same size, so the size is whichever of the two directions runs out first
        Dim cellSize As Integer = usableWidth \ heatPerRow

        If usableHeight \ heatRows < cellSize Then
            cellSize = usableHeight \ heatRows
        End If

        If cellSize < 4 Then
            g.DrawString("This screen is too big to draw here", New Font("Segoe UI", 9), Brushes.Gray, 10, 10)
            Exit Sub
        End If

        'a gap between the seats, but not on the tiny ones or there would be nothing left
        Dim gap As Integer = 2

        If cellSize < 14 Then
            gap = 1
        End If

        'the block of seats is centred both ways, otherwise a small screen sits in the corner
        'with a lot of empty white underneath it
        Dim startX As Integer = letterWidth + (usableWidth - (heatPerRow * cellSize)) \ 2
        Dim startY As Integer = screenBar + 4 + (usableHeight - (heatRows * cellSize)) \ 2

        'the screen itself goes along the top, because row A is the front row
        Dim barWidth As Integer = heatPerRow * cellSize - gap
        g.FillRectangle(New SolidBrush(Color.FromArgb(70, 70, 78)), startX, startY - 20, barWidth, 14)
        g.DrawString("SCREEN", New Font("Segoe UI", 7.5), Brushes.White,
                     startX + (barWidth \ 2) - 24, startY - 19)

        Dim letterFont As New Font("Segoe UI", 7.5)
        Dim countFont As New Font("Segoe UI", 7.5)

        For rowIndex As Integer = 0 To heatRows - 1
            Dim y As Integer = startY + (rowIndex * cellSize)

            'the row letter down the left so a hot patch can be pointed at
            g.DrawString(Chr(65 + rowIndex), letterFont, Brushes.Gray, 4, y + 1)

            For seatIndex As Integer = 0 To heatPerRow - 1
                Dim x As Integer = startX + (seatIndex * cellSize)
                Dim timesSold As Integer = heatCounts(rowIndex, seatIndex)

                g.FillRectangle(New SolidBrush(HeatColour(timesSold)), x, y, cellSize - gap, cellSize - gap)

                'the number only goes on when the seats are drawn big enough to read it
                If cellSize >= 26 And timesSold > 0 Then
                    Dim fore As Brush = Brushes.Black

                    If heatBusiest > 0 AndAlso timesSold > heatBusiest \ 2 Then
                        fore = Brushes.White
                    End If

                    g.DrawString(timesSold.ToString(), countFont, fore, x + 3, y + 2)
                End If
            Next
        Next
    End Sub

    'picks the colour for a seat from how many times it has been sold. a seat nobody has ever
    'picked is grey, and everything else fades from pale yellow up to deep red depending on how
    'it compares with the busiest seat in the room. working from the busiest seat rather than a
    'fixed number means a quiet screen still shows which of its seats people prefer
    Private Function HeatColour(timesSold As Integer) As Color
        If timesSold <= 0 Or heatBusiest <= 0 Then
            Return Color.FromArgb(224, 224, 228)
        End If

        Dim howHot As Double = timesSold / heatBusiest

        'the pale end is 255,236,160 and the hot end is 190,30,45, so each part of the colour is
        'moved that far along depending on how hot the seat is
        Dim red As Integer = CInt(255 + ((190 - 255) * howHot))
        Dim green As Integer = CInt(236 + ((30 - 236) * howHot))
        Dim blue As Integer = CInt(160 + ((45 - 160) * howHot))

        Return Color.FromArgb(red, green, blue)
    End Function

    '=============================================================================
    'the seat plan, where the premium and accessible seats get put
    '=============================================================================

    'builds the plan to match whatever is in the two boxes. anything already picked is kept if it
    'still fits, so nudging the seats per row up by one does not throw away all the marking out
    'that has already been done. new squares start on whatever the usual layout would give them
    Private Sub BuildSeatPlan()
        Dim numRows As Integer = SafeInt(txtRows.Text)
        Dim perRow As Integer = SafeInt(txtPerRow.Text)

        If numRows <= 0 Or perRow <= 0 Or numRows > 26 Then
            planRows = 0
            planPerRow = 0
            ShowSeatPlanKey()
            pnlSeatPlan.Invalidate()
            Exit Sub
        End If

        'hold on to what is there now, because the ReDim below empties the array
        Dim oldTypes(,) As String = planTypes
        Dim oldRows As Integer = planRows
        Dim oldPerRow As Integer = planPerRow

        ReDim planTypes(numRows - 1, perRow - 1)
        planRows = numRows
        planPerRow = perRow

        For rowIndex As Integer = 0 To planRows - 1
            For seatIndex As Integer = 0 To planPerRow - 1
                If rowIndex < oldRows And seatIndex < oldPerRow Then
                    planTypes(rowIndex, seatIndex) = oldTypes(rowIndex, seatIndex)
                Else
                    planTypes(rowIndex, seatIndex) = SeatTypeForRow(rowIndex, planRows)
                End If
            Next
        Next

        ShowSeatPlanKey()
        pnlSeatPlan.Invalidate()
    End Sub

    'what sort of seat the plan says a square is. anything the plan does not cover falls back on
    'the usual layout, so seat making still works even if the plan was never drawn
    Private Function PlannedTypeFor(rowIndex As Integer, seatIndex As Integer, numRows As Integer) As String
        If rowIndex < 0 Or seatIndex < 0 Or rowIndex >= planRows Or seatIndex >= planPerRow Then
            Return SeatTypeForRow(rowIndex, numRows)
        End If

        If planTypes(rowIndex, seatIndex) = "" Then
            Return SeatTypeForRow(rowIndex, numRows)
        End If

        Return planTypes(rowIndex, seatIndex)
    End Function

    'clicking a seat moves it on to the next sort, and round again from the end
    Private Function NextSeatType(thisType As String) As String
        If thisType = SeatStandard Then
            Return SeatPremium
        End If

        If thisType = SeatPremium Then
            Return SeatAccessible
        End If

        Return SeatStandard
    End Function

    'the colour each sort of seat is drawn in on the plan
    Private Function PlanColour(seatType As String) As Color
        If seatType = SeatPremium Then
            Return Color.FromArgb(212, 175, 55)
        End If

        If seatType = SeatAccessible Then
            Return Color.FromArgb(60, 120, 200)
        End If

        Return Color.FromArgb(190, 195, 205)
    End Function

    'works out how big to draw the seats and where the block of them starts. the drawing and the
    'clicking both need these numbers, so they are worked out in one place rather than twice,
    'which is what would let a click land on a different seat from the one drawn there
    Private Sub SeatPlanGeometry(ByRef cellSize As Integer, ByRef startX As Integer, ByRef startY As Integer)
        Dim letterWidth As Integer = 22
        Dim screenBar As Integer = 22
        Dim usableWidth As Integer = pnlSeatPlan.Width - letterWidth - 6
        Dim usableHeight As Integer = pnlSeatPlan.Height - screenBar - 6

        cellSize = usableWidth \ planPerRow

        If usableHeight \ planRows < cellSize Then
            cellSize = usableHeight \ planRows
        End If

        startX = letterWidth + (usableWidth - (planPerRow * cellSize)) \ 2
        startY = screenBar + 4 + (usableHeight - (planRows * cellSize)) \ 2
    End Sub

    'draws the plan of the room. it is painted rather than made out of buttons because a big
    'screen would be several hundred controls, and all it has to do is show a colour per seat
    Private Sub pnlSeatPlan_Paint(sender As Object, e As PaintEventArgs) Handles pnlSeatPlan.Paint
        Dim g As Graphics = e.Graphics
        g.Clear(Color.White)

        If planRows <= 0 Or planPerRow <= 0 Then
            g.DrawString("Type how many rows and seats first", New Font("Segoe UI", 9), Brushes.Gray, 10, 10)
            Exit Sub
        End If

        Dim cellSize As Integer = 0
        Dim startX As Integer = 0
        Dim startY As Integer = 0
        SeatPlanGeometry(cellSize, startX, startY)

        If cellSize < 6 Then
            g.DrawString("This screen is too big to draw here", New Font("Segoe UI", 9), Brushes.Gray, 10, 10)
            Exit Sub
        End If

        'a gap between the seats, but a smaller one on the little ones or there is nothing left
        Dim gap As Integer = 2

        If cellSize < 14 Then
            gap = 1
        End If

        'the screen itself goes along the top, because row A is the front row
        Dim barWidth As Integer = planPerRow * cellSize - gap
        g.FillRectangle(New SolidBrush(Color.FromArgb(70, 70, 78)), startX, startY - 20, barWidth, 14)
        g.DrawString("SCREEN", New Font("Segoe UI", 7.5), Brushes.White,
                     startX + (barWidth \ 2) - 24, startY - 19)

        Dim letterFont As New Font("Segoe UI", 7.5)
        Dim markFont As New Font("Segoe UI", 7.5, FontStyle.Bold)

        For rowIndex As Integer = 0 To planRows - 1
            Dim y As Integer = startY + (rowIndex * cellSize)

            'the row letter down the left. clicking it changes the whole row at once
            g.DrawString(Chr(65 + rowIndex), letterFont, Brushes.Gray, 4, y + 1)

            For seatIndex As Integer = 0 To planPerRow - 1
                Dim x As Integer = startX + (seatIndex * cellSize)
                Dim thisType As String = planTypes(rowIndex, seatIndex)

                g.FillRectangle(New SolidBrush(PlanColour(thisType)), x, y, cellSize - gap, cellSize - gap)

                'a letter on the seat as well as the colour, so the plan can still be read when
                'it is printed in black and white for the write up
                If cellSize >= 14 And thisType <> SeatStandard Then
                    Dim mark As String = "P"

                    If thisType = SeatAccessible Then
                        mark = "A"
                    End If

                    g.DrawString(mark, markFont, Brushes.White, x + 2, y + 1)
                End If
            Next
        Next
    End Sub

    'clicking a seat changes what sort it is, and clicking the row letter changes the whole row
    Private Sub pnlSeatPlan_MouseDown(sender As Object, e As MouseEventArgs) Handles pnlSeatPlan.MouseDown
        If planRows <= 0 Or planPerRow <= 0 Then
            Exit Sub
        End If

        Dim cellSize As Integer = 0
        Dim startX As Integer = 0
        Dim startY As Integer = 0
        SeatPlanGeometry(cellSize, startX, startY)

        If cellSize < 6 Then
            Exit Sub
        End If

        'above the first row is not a click on a seat at all. it has to be checked before the
        'divide, because a negative divided down still comes out as row 0
        If e.Y < startY Then
            Exit Sub
        End If

        Dim rowIndex As Integer = (e.Y - startY) \ cellSize

        If rowIndex >= planRows Then
            Exit Sub
        End If

        If e.X < startX Then
            'the row letter, so the whole row goes to whatever the first seat in it would become
            Dim newType As String = NextSeatType(planTypes(rowIndex, 0))

            For seatIndex As Integer = 0 To planPerRow - 1
                planTypes(rowIndex, seatIndex) = newType
            Next
        Else
            Dim seatIndex As Integer = (e.X - startX) \ cellSize

            If seatIndex >= planPerRow Then
                Exit Sub
            End If

            planTypes(rowIndex, seatIndex) = NextSeatType(planTypes(rowIndex, seatIndex))
        End If

        'marking seats out counts as an unsaved change the same as typing in the boxes does
        planChanged = True
        boxesChanged = True

        ShowSeatPlanKey()
        ShowLayoutPreview()
        pnlSeatPlan.Invalidate()
    End Sub

    'puts the plan back to the usual layout, front row accessible and the back two premium
    Private Sub btnPlanDefault_Click(sender As Object, e As EventArgs) Handles btnPlanDefault.Click
        If planRows <= 0 Then
            Exit Sub
        End If

        For rowIndex As Integer = 0 To planRows - 1
            For seatIndex As Integer = 0 To planPerRow - 1
                planTypes(rowIndex, seatIndex) = SeatTypeForRow(rowIndex, planRows)
            Next
        Next

        planChanged = True
        boxesChanged = True
        ShowSeatPlanKey()
        ShowLayoutPreview()
        pnlSeatPlan.Invalidate()
    End Sub

    'wipes the plan back to a room with nothing special in it
    Private Sub btnPlanAllStandard_Click(sender As Object, e As EventArgs) Handles btnPlanAllStandard.Click
        If planRows <= 0 Then
            Exit Sub
        End If

        For rowIndex As Integer = 0 To planRows - 1
            For seatIndex As Integer = 0 To planPerRow - 1
                planTypes(rowIndex, seatIndex) = SeatStandard
            Next
        Next

        planChanged = True
        boxesChanged = True
        ShowSeatPlanKey()
        ShowLayoutPreview()
        pnlSeatPlan.Invalidate()
    End Sub

    'the wording above and below the plan, including how many of each sort there are
    Private Sub ShowSeatPlanKey()
        If planRows <= 0 Or planPerRow <= 0 Then
            lblSeatPlanInfo.Text = "Type how many rows and how many seats in each row first," & vbCrLf &
                                   "then the plan of the room can be marked out here."
            lblSeatPlanKey.Text = ""
            Exit Sub
        End If

        lblSeatPlanInfo.Text = "Click a seat to change what sort it is. Standard, then" & vbCrLf &
                               "premium, then accessible, then round again." & vbCrLf &
                               "Clicking a row letter changes that whole row."

        Dim standardSeats As Integer = 0
        Dim premiumSeats As Integer = 0
        Dim accessibleSeats As Integer = 0
        CountPlannedSeats(standardSeats, premiumSeats, accessibleSeats)

        lblSeatPlanKey.Text = "Grey is standard, gold is premium (P), blue is" & vbCrLf &
                              "accessible (A). What a seat costs comes from its sort." & vbCrLf &
                              standardSeats & " standard, " & premiumSeats & " premium, " &
                              accessibleSeats & " accessible."
    End Sub

    'counts the plan up into the three sorts. the line under the boxes and the key under the
    'plan both want the same three numbers, so they get counted once here
    Private Sub CountPlannedSeats(ByRef standardSeats As Integer, ByRef premiumSeats As Integer, ByRef accessibleSeats As Integer)
        standardSeats = 0
        premiumSeats = 0
        accessibleSeats = 0

        For rowIndex As Integer = 0 To planRows - 1
            For seatIndex As Integer = 0 To planPerRow - 1
                Dim thisType As String = planTypes(rowIndex, seatIndex)

                If thisType = SeatPremium Then
                    premiumSeats = premiumSeats + 1
                ElseIf thisType = SeatAccessible Then
                    accessibleSeats = accessibleSeats + 1
                Else
                    standardSeats = standardSeats + 1
                End If
            Next
        Next
    End Sub

    'reads the sort of every seat in the selected screen back into the plan, so an existing room
    'comes up marked out the way it really is rather than the way the usual layout would have it
    Private Sub LoadSeatPlanFromScreen()
        BuildSeatPlan()

        If planRows <= 0 Or selectedScreenID = 0 Then
            Exit Sub
        End If

        Dim dtSeats As New DataTable

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT tblSeat.SeatRow, tblSeat.SeatNumber, tblSeatType.SeatTypeName " &
                                 "FROM tblSeat INNER JOIN tblSeatType ON tblSeat.SeatTypeID = tblSeatType.SeatTypeID " &
                                 "WHERE tblSeat.ScreenID = @ScreenID"
            SQLCmd.Parameters.AddWithValue("@ScreenID", CInt(selectedScreenID))
            Dim da As New OleDbDataAdapter(SQLCmd)
            da.Fill(dtSeats)
            cn.Close()
        End If

        For Each seat As DataRow In dtSeats.Rows
            Dim rowIndex As Integer = Asc(seat("SeatRow").ToString().ToUpper()) - 65
            Dim seatIndex As Integer = CInt(seat("SeatNumber")) - 1

            'a seat left over from an older, bigger layout would fall outside the plan
            If rowIndex >= 0 And rowIndex < planRows And seatIndex >= 0 And seatIndex < planPerRow Then
                planTypes(rowIndex, seatIndex) = seat("SeatTypeName").ToString()
            End If
        Next

        'what has just been read is what is already saved, so there is nothing to write back yet
        planChanged = False

        ShowSeatPlanKey()
        ShowLayoutPreview()
        pnlSeatPlan.Invalidate()
    End Sub

    'writes the plan onto seats that already exist. this is the path taken when the room is still
    'the same size, so the seats must not be thrown away and made again - anything already booked
    'is pointing at them. only the sort on each seat changes, the seats themselves stay put.
    'the command comes in inside a transaction like the other seat routines do
    Private Sub SaveSeatTypes(SQLCmd As OleDbCommand, screenID As Long, numRows As Integer, perRow As Integer)
        SQLCmd.CommandText = "SELECT SeatTypeID, SeatTypeName FROM tblSeatType"
        SQLCmd.Parameters.Clear()
        Dim dtTypes As New DataTable
        Dim rs As OleDbDataReader = SQLCmd.ExecuteReader()
        dtTypes.Load(rs)
        rs.Close()

        SQLCmd.CommandText = "UPDATE tblSeat SET SeatTypeID = @SeatTypeID " &
                             "WHERE ScreenID = @ScreenID AND SeatRow = @SeatRow AND SeatNumber = @SeatNumber"
        SQLCmd.Parameters.Clear()
        SQLCmd.Parameters.AddWithValue("@SeatTypeID", 0)
        SQLCmd.Parameters.AddWithValue("@ScreenID", CInt(screenID))
        SQLCmd.Parameters.AddWithValue("@SeatRow", "")
        SQLCmd.Parameters.AddWithValue("@SeatNumber", 0)

        For rowIndex As Integer = 0 To numRows - 1
            Dim rowLetter As String = Chr(65 + rowIndex)

            For seatNum As Integer = 1 To perRow
                Dim typeID As Long = TypeIDFromTable(dtTypes, PlannedTypeFor(rowIndex, seatNum - 1, numRows))
                SQLCmd.Parameters("@SeatTypeID").Value = CInt(typeID)
                SQLCmd.Parameters("@SeatRow").Value = rowLetter
                SQLCmd.Parameters("@SeatNumber").Value = seatNum
                SQLCmd.ExecuteNonQuery()
            Next
        Next
    End Sub

    '=============================================================================
    'what is on in this screen
    '=============================================================================

    'lists the screenings still to come in this screen, with how full each one is. it is read only
    'on purpose, the screenings form is still the place they get changed. it is here so that
    'whoever is about to shut a screen can see what they would be disrupting
    Private Sub LoadScreeningsForScreen()
        Dim dt As New DataTable

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT tblScreening.ScreeningID, ScreeningDate, ScreeningTime, FilmTitle " &
                                 "FROM tblScreening INNER JOIN tblFilm ON tblScreening.FilmID = tblFilm.FilmID " &
                                 "WHERE tblScreening.ScreenID = @ScreenID AND ScreeningDate >= @Today " &
                                 "AND (ScreeningStatus IS NULL OR ScreeningStatus <> 'Cancelled') " &
                                 "ORDER BY ScreeningDate, ScreeningTime"
            SQLCmd.Parameters.AddWithValue("@ScreenID", CInt(selectedScreenID))
            SQLCmd.Parameters.AddWithValue("@Today", Date.Today)
            Dim da As New OleDbDataAdapter(SQLCmd)
            da.Fill(dt)
            cn.Close()
        End If

        Dim capacity As Integer = SeatsOnScreen(selectedScreenID)

        'a second table is built to show, with its columns already in the order they should read.
        'putting the made up columns on the end of the first one and then shuffling them about
        'afterwards came out in the wrong order, because the hidden columns are still in the way
        Dim dtShow As New DataTable
        dtShow.Columns.Add("When", GetType(String))
        dtShow.Columns.Add("Film", GetType(String))
        dtShow.Columns.Add("Sold", GetType(String))

        Dim withBookings As Integer = 0

        For Each row As DataRow In dt.Rows
            Dim soldHere As Integer = SeatsSoldOnScreening(CLng(row("ScreeningID")))
            Dim showRow As DataRow = dtShow.NewRow()

            showRow("When") = CDate(row("ScreeningDate")).ToString("ddd dd MMM") & "  " & row("ScreeningTime").ToString()
            showRow("Film") = row("FilmTitle").ToString()

            If capacity > 0 Then
                showRow("Sold") = soldHere & "/" & capacity & " (" & CInt(soldHere * 100.0 / capacity) & "%)"
            Else
                showRow("Sold") = soldHere.ToString()
            End If

            dtShow.Rows.Add(showRow)

            If soldHere > 0 Then
                withBookings = withBookings + 1
            End If
        Next

        dgvScreenings.DataSource = dtShow

        If dgvScreenings.Columns.Contains("When") Then
            dgvScreenings.Columns("When").Width = 116
            dgvScreenings.Columns("Film").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            dgvScreenings.Columns("Sold").Width = 88
        End If

        If dtShow.Rows.Count = 0 Then
            lblScreeningsInfo.Text = "Nothing is booked into this screen from" & vbCrLf &
                                     "today onwards, so taking it out of service" & vbCrLf &
                                     "would not disrupt anybody."
        Else
            lblScreeningsInfo.Text = dtShow.Rows.Count & " screening(s) still to come, " & withBookings & " with" & vbCrLf &
                                     "tickets sold. Screenings are changed on the" & vbCrLf &
                                     "screenings form, this list is only context."
        End If
    End Sub

    'counts the seats sold for one screening. tblBookingSeat carries the ScreeningID itself, so
    'this does not need to go back through tblBooking, and cancelling deletes these rows so there
    'is nothing to filter out
    Private Function SeatsSoldOnScreening(screeningID As Long) As Integer
        Dim total As Integer = 0

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT COUNT(*) FROM tblBookingSeat WHERE ScreeningID = @ScreeningID"
            SQLCmd.Parameters.AddWithValue("@ScreeningID", CInt(screeningID))
            total = CInt(SQLCmd.ExecuteScalar())
            cn.Close()
        End If

        Return total
    End Function

    '=============================================================================
    'taking a screen out of service and putting it back
    '=============================================================================

    'sets up the two buttons and the words above them for whichever screen is picked
    Private Sub ShowStatusButtons()
        If selectedStatus = ScreenOutOfService Then
            lblScreenState.Text = "This screen is OUT OF SERVICE" & vbCrLf & "Reason: " & selectedReason
            lblScreenState.ForeColor = Color.FromArgb(170, 40, 40)

            btnOutOfService.Enabled = False
            btnBackInService.Enabled = True
            txtReason.Enabled = False
            txtReason.Text = selectedReason

            lblStatusHint.Text = "Nothing new can be scheduled here until it" & vbCrLf &
                                 "is put back. Screenings already in it have" & vbCrLf &
                                 "been left alone."
        Else
            lblScreenState.Text = "This screen is in service"
            lblScreenState.ForeColor = AccentFore

            btnOutOfService.Enabled = True
            btnBackInService.Enabled = False
            txtReason.Enabled = True
            txtReason.Text = ""

            lblStatusHint.Text = "Taking a screen out of service stops new" & vbCrLf &
                                 "screenings going in it. The screen, its seats" & vbCrLf &
                                 "and its history all stay as they are."
        End If
    End Sub

    'takes a screen out of service, which is a repair, a refit or anything else that means it
    'cannot be used for a while. it is deliberately not a delete, because deleting would take the
    'seats and everything that has ever been sold in the room with it
    Private Sub btnOutOfService_Click(sender As Object, e As EventArgs) Handles btnOutOfService.Click
        If selectedScreenID = 0 Then
            MessageBox.Show("Select a screen in the grid first", "Screens", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If txtReason.Text.Trim() = "" Then
            MessageBox.Show("Say why the screen is coming out of service." & vbCrLf &
                            "Somebody looking at it next week needs to know whether it is a broken projector " &
                            "or a refit that is going to take a month.",
                            "Screens", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtReason.Focus()
            Exit Sub
        End If

        'a screening that people have already bought tickets for cannot just be left in a room that
        'is shut. those bookings have to be dealt with first, so the screen will not go out
        Dim bookedAhead As Integer = BookedSeatsAhead(selectedScreenID)

        If bookedAhead > 0 Then
            MessageBox.Show("There are " & bookedAhead & " ticket(s) already sold for screenings still to come " &
                            "in this screen." & vbCrLf & vbCrLf &
                            "Shutting the room would leave those customers with seats in a screen that is not " &
                            "open. Cancel or move those bookings first, then it can go out of service.",
                            "Cannot take this screen out of service", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            WriteLog("SCREEN", "Out of service refused for '" & txtName.Text & "', " & bookedAhead & " ticket(s) sold ahead", LogWarning)
            Exit Sub
        End If

        'screenings with nothing sold are not a reason to stop, but they are worth saying out loud
        Dim upcoming As Integer = UpcomingScreeningsOnScreen(selectedScreenID)
        Dim question As String = "Take '" & txtName.Text & "' out of service?"

        If upcoming > 0 Then
            question = question & vbCrLf & vbCrLf &
                       "It still has " & upcoming & " screening(s) scheduled in it. Nothing has been sold for " &
                       "them, but they will be sitting in a screen that is shut until they are moved or removed."
        End If

        If MessageBox.Show(question, "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                           MessageBoxDefaultButton.Button2) = DialogResult.No Then
            Exit Sub
        End If

        SetScreenStatus(ScreenOutOfService, txtReason.Text.Trim())
    End Sub

    'puts a screen back on after whatever was wrong with it has been sorted out
    Private Sub btnBackInService_Click(sender As Object, e As EventArgs) Handles btnBackInService.Click
        If selectedScreenID = 0 Then
            MessageBox.Show("Select a screen in the grid first", "Screens", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        'a room with no seats in it cannot take a booking, so putting it back would only mean the
        'seat map came up empty with nothing to explain why
        Dim seatsMade As Integer = SeatsOnScreen(selectedScreenID)

        If seatsMade = 0 Then
            MessageBox.Show("'" & txtName.Text & "' has no seats made for it, so nothing could be sold in it " &
                            "anyway." & vbCrLf & vbCrLf &
                            "Set its rows and seats per row in the boxes on the left and save, which makes the " &
                            "seats, then put it back in service.",
                            "Cannot put this screen back", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If MessageBox.Show("Put '" & txtName.Text & "' back in service?" & vbCrLf &
                           "Films can be scheduled in it again straight away.",
                           "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
            Exit Sub
        End If

        SetScreenStatus(ScreenInService, "")
    End Sub

    'writes the new status onto the screen. the reason and the date go with it so the list is not
    'just a word, it says why and since when
    Private Sub SetScreenStatus(newStatus As String, reason As String)
        Dim saved As Boolean = False

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            'the date goes in with Now() in the query rather than as a parameter, the same way the
            'cancel on the booking search does it. a date with a time on it through AddWithValue
            'carries the milliseconds with it, and an Access date field has no room for those, so
            'the whole update comes back as a data type mismatch. every other date in the program
            'is a midnight one, which is why this is the only place it bit
            SQLCmd.CommandText = "UPDATE tblScreen " &
                                 "SET ScreenStatus = @ScreenStatus, ScreenStatusReason = @ScreenStatusReason, " &
                                 "ScreenStatusDate = Now() " &
                                 "WHERE ScreenID = @ScreenID"
            SQLCmd.Parameters.AddWithValue("@ScreenStatus", newStatus)
            SQLCmd.Parameters.AddWithValue("@ScreenStatusReason", reason)
            SQLCmd.Parameters.AddWithValue("@ScreenID", CInt(selectedScreenID))
            SQLCmd.ExecuteNonQuery()
            saved = True
            cn.Close()
        End If

        Dim screenName As String = txtName.Text.Trim()

        'a room being shut or opened changes what the whole program will let people do, so it is
        'worth more than an ordinary change entry in the log
        If saved Then
            If newStatus = ScreenOutOfService Then
                WriteLog("SCREEN", "Screen taken out of service: " & screenName & " (" & reason & ")", LogWarning)
            Else
                WriteLog("SCREEN", "Screen put back in service: " & screenName, LogChange)
            End If
        End If

        Dim keepOnThisScreen As Long = selectedScreenID

        LoadScreens()
        SelectScreenInGrid(keepOnThisScreen)
        ShowStatusButtons()

        If saved Then
            If newStatus = ScreenOutOfService Then
                SayDone(lblSaved, "'" & screenName & "' is now out of service")
            Else
                SayDone(lblSaved, "'" & screenName & "' is back in service")
            End If
        End If
    End Sub

    'counts the tickets already sold for screenings in this screen that have not been on yet.
    'this is the thing that decides whether a screen is allowed to be shut
    Private Function BookedSeatsAhead(screenID As Long) As Integer
        Dim total As Integer = 0

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT COUNT(*) FROM tblBookingSeat " &
                                 "INNER JOIN tblScreening ON tblBookingSeat.ScreeningID = tblScreening.ScreeningID " &
                                 "WHERE tblScreening.ScreenID = @ScreenID AND tblScreening.ScreeningDate >= @Today"
            SQLCmd.Parameters.AddWithValue("@ScreenID", CInt(screenID))
            SQLCmd.Parameters.AddWithValue("@Today", Date.Today)
            total = CInt(SQLCmd.ExecuteScalar())
            cn.Close()
        End If

        Return total
    End Function

End Class
