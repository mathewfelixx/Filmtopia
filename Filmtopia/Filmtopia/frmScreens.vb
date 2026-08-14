Imports System.Data.OleDb

Public Class frmScreens

    'tracks the ScreenID of the row currently selected in the grid, 0 means nothing selected
    Private selectedScreenID As Long = 0

    'the layout the selected screen had when it was clicked on. it is kept so that saving can tell
    'whether the layout has actually been changed, because the seats only need making again if it has
    Private rowsWhenPicked As Integer = 0
    Private perRowWhenPicked As Integer = 0

    Private Sub frmScreens_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CommonFormStartup(Me)
        LoadScreens()
        ClearFields()
        WriteLog("SCREEN", "Screens form opened")
    End Sub

    'loads the screens into the grid along with how many seats have actually been made for each
    'one and how many screenings it has, so a screen that is in use is obvious before anybody
    'starts changing it
    Private Sub LoadScreens()
        Dim dt As New DataTable

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT ScreenID, ScreenName, ScreenCapacity, ScreenRows, SeatsPerRow " &
                                 "FROM tblScreen ORDER BY ScreenName"
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
            dgvScreens.Columns("ScreenCapacity").HeaderText = "Capacity"
            dgvScreens.Columns("Rows").HeaderText = "Layout"
            dgvScreens.Columns("Seats").HeaderText = "Seats made"
            dgvScreens.Columns("Screenings").HeaderText = "Screenings"

            'the two layout numbers are what the boxes underneath get filled from, they are not
            'worth a column of their own when the Layout column already says it in words
            dgvScreens.Columns("ScreenRows").Visible = False
            dgvScreens.Columns("SeatsPerRow").Visible = False

            dgvScreens.Columns("ScreenID").Width = 50
            dgvScreens.Columns("ScreenName").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            dgvScreens.Columns("ScreenCapacity").Width = 90
            dgvScreens.Columns("Rows").Width = 140
            dgvScreens.Columns("Seats").Width = 100
            dgvScreens.Columns("Screenings").Width = 100

            dgvScreens.Columns("ScreenCapacity").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            dgvScreens.Columns("Seats").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            dgvScreens.Columns("Screenings").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        End If

        MarkScreensThatDoNotAddUp()
        ShowCount(dt)
        dgvScreens.ClearSelection()

        WriteLog("SCREEN", "Screen list loaded")
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

    'says how many screens there are and how many seats that is altogether
    Private Sub ShowCount(dt As DataTable)
        Dim seats As Integer = 0

        For Each row As DataRow In dt.Rows
            seats = seats + CInt(row("Seats"))
        Next

        lblGridCount.Text = dt.Rows.Count & " screen(s), " & seats & " seats in the building"
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

        'count how the rows split between the three sorts so the mix can be shown
        Dim standardRows As Integer = 0
        Dim premiumRows As Integer = 0
        Dim accessibleRows As Integer = 0

        For rowIndex As Integer = 0 To numRows - 1
            Dim thisType As String = SeatTypeForRow(rowIndex, numRows)
            If thisType = SeatPremium Then
                premiumRows = premiumRows + 1
            ElseIf thisType = SeatAccessible Then
                accessibleRows = accessibleRows + 1
            Else
                standardRows = standardRows + 1
            End If
        Next

        Dim mix As String = standardRows & " standard"
        If premiumRows > 0 Then
            mix = mix & ", " & premiumRows & " premium"
        End If
        If accessibleRows > 0 Then
            mix = mix & ", " & accessibleRows & " accessible"
        End If

        lblLayout.Text = "That makes " & (numRows * perRow) & " seats, numbered A1 to " &
                         Chr(64 + numRows) & perRow & "." & vbCrLf &
                         "Rows: " & mix & "."
    End Sub

    'adds a new screen and makes its seats
    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        If txtName.Text.Trim() = "" Then
            MessageBox.Show("Enter a screen name")
            txtName.Focus()
            Exit Sub
        End If

        If Not CapacityIsValid() Then
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

        'logging waits until the connection is shut, WriteLog opens its own
        If newScreenID > 0 Then
            WriteLog("SCREEN", "Screen added: " & txtName.Text.Trim(), LogChange)
            WriteLog("SCREEN", "Seats generated for ScreenID " & newScreenID & ", " & numRows & " row(s) of " &
                               perRow & ", " & (numRows * perRow) & " seats", LogChange)
        End If

        LoadScreens()
        ClearFields()
    End Sub

    'saves the changes made to the screen selected in the grid
    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        If selectedScreenID = 0 Then
            MessageBox.Show("Select a screen in the grid first")
            Exit Sub
        End If

        If txtName.Text.Trim() = "" Then
            MessageBox.Show("Enter a screen name")
            txtName.Focus()
            Exit Sub
        End If

        If Not CapacityIsValid() Then
            Exit Sub
        End If

        Dim newRows As Integer = CInt(Val(txtRows.Text))
        Dim newPerRow As Integer = CInt(Val(txtPerRow.Text))
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
                               "Carry on?", "Confirm", MessageBoxButtons.YesNo) = DialogResult.No Then
                Exit Sub
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

                'only remake the seats if the layout actually changed, not if just the name did
                If capacityChanged Then
                    DeleteSeats(SQLCmd, selectedScreenID)
                    GenerateSeats(SQLCmd, selectedScreenID, newRows, newPerRow)
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

        If saved Then
            WriteLog("SCREEN", "Screen updated: " & txtName.Text.Trim(), LogChange)

            If capacityChanged Then
                WriteLog("SCREEN", "Seats generated for ScreenID " & selectedScreenID & ", " & newRows & " row(s) of " &
                                   newPerRow & ", " & (newRows * newPerRow) & " seats", LogChange)
            End If
        End If

        LoadScreens()
        ClearFields()
    End Sub

    'deletes the screen selected in the grid
    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If selectedScreenID = 0 Then
            MessageBox.Show("Select a screen in the grid first")
            Exit Sub
        End If

        'a screen with something scheduled in it cannot go, those screenings would be left in a
        'room that does not exist
        Dim screenings As Integer = ScreeningsOnScreen(selectedScreenID)

        If screenings > 0 Then
            MessageBox.Show("'" & txtName.Text & "' has " & screenings & " screening(s) scheduled in it." & vbCrLf &
                            "Delete those screenings first, then the screen can be removed.",
                            "Cannot delete", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Dim booked As Integer = BookedSeatsOnScreen(selectedScreenID)

        If booked > 0 Then
            MessageBox.Show("'" & txtName.Text & "' has " & booked & " seat(s) that are booked." & vbCrLf &
                            "Cancel those bookings first, then the screen can be removed.",
                            "Cannot delete", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If MessageBox.Show("Delete '" & txtName.Text & "' and all of its seats?", "Confirm", MessageBoxButtons.YesNo) = DialogResult.No Then
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

        If deleted Then
            WriteLog("SCREEN", "Screen deleted: " & txtName.Text, LogChange)
        End If

        LoadScreens()
        ClearFields()
    End Sub

    'checks the two layout boxes. there is no multiple of ten rule any more, because the rows and
    'the seats in them are given separately, so any size of screen works out exactly
    Private Function CapacityIsValid() As Boolean
        If Not IsNumeric(txtRows.Text) Then
            MessageBox.Show("How many rows has to be a number")
            txtRows.Focus()
            Return False
        End If

        If Not IsNumeric(txtPerRow.Text) Then
            MessageBox.Show("How many seats in a row has to be a number")
            txtPerRow.Focus()
            Return False
        End If

        Dim numRows As Integer = SafeInt(txtRows.Text)
        Dim perRow As Integer = SafeInt(txtPerRow.Text)

        If numRows <= 0 Then
            MessageBox.Show("A screen needs at least one row")
            txtRows.Focus()
            Return False
        End If

        If perRow <= 0 Then
            MessageBox.Show("A row needs at least one seat in it")
            txtPerRow.Focus()
            Return False
        End If

        'twenty six rows is as far as the letters go
        If numRows > 26 Then
            MessageBox.Show("The rows are lettered A to Z, so a screen cannot have more than 26 rows")
            txtRows.Focus()
            Return False
        End If

        'the seat map draws a button per seat across the panel, so a silly wide row would run off
        'the side of it
        If perRow > 20 Then
            MessageBox.Show("A row cannot have more than 20 seats in it, they would not fit on the seat map")
            txtPerRow.Focus()
            Return False
        End If

        Return True
    End Function

    'clears the boxes and the selection
    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        ClearFields()
        WriteLog("SCREEN", "Screen fields cleared")
    End Sub

    Private Sub ClearFields()
        selectedScreenID = 0
        rowsWhenPicked = 0
        perRowWhenPicked = 0
        txtName.Text = ""
        txtRows.Text = ""
        txtPerRow.Text = ""
        dgvScreens.ClearSelection()
        ShowWhatIsBeingEdited()
        ShowLayoutPreview()
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

        For rowIndex As Integer = 0 To numRows - 1
            Dim rowLetter As String = Chr(65 + rowIndex)
            Dim typeID As Long = TypeIDFromTable(dtTypes, SeatTypeForRow(rowIndex, numRows))

            For seatNum As Integer = 1 To perRow
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

        Dim row As DataGridViewRow = dgvScreens.Rows(e.RowIndex)
        selectedScreenID = CLng(row.Cells("ScreenID").Value)
        txtName.Text = row.Cells("ScreenName").Value.ToString()
        txtRows.Text = row.Cells("ScreenRows").Value.ToString()
        txtPerRow.Text = row.Cells("SeatsPerRow").Value.ToString()

        'remembered so saving can tell whether the layout has been changed or only the name
        rowsWhenPicked = CInt(row.Cells("ScreenRows").Value)
        perRowWhenPicked = CInt(row.Cells("SeatsPerRow").Value)

        ShowWhatIsBeingEdited()
        WriteLog("SCREEN", "Screen selected: " & txtName.Text)
    End Sub

End Class
