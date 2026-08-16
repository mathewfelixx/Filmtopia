Imports System.Data.OleDb

Public Class frmScreens

    Private selectedScreenID As Long = 0

    Private rowsWhenPicked As Integer = 0
    Private perRowWhenPicked As Integer = 0

    Private boxesChanged As Boolean = False

    Private fillingBoxes As Boolean = False

    Private selectedStatus As String = ScreenInService
    Private selectedReason As String = ""

    Private heatCounts(,) As Integer
    Private heatRows As Integer = 0
    Private heatPerRow As Integer = 0
    Private heatBusiest As Integer = 0

    Private planTypes(,) As String
    Private planRows As Integer = 0
    Private planPerRow As Integer = 0

    Private planChanged As Boolean = False

    Private Sub frmScreens_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If UserAccessLevel <> 1 Then
            MessageBox.Show("Only a manager can open the screens screen.", "Screens", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            WriteLog("SCREEN", "Screens screen refused, access level " & UserAccessLevel, LogSecurity)
            Me.Close()
            Exit Sub
        End If

        CommonFormStartup(Me)

        Me.KeyPreview = True

        pnlHeatmap.BackColor = Color.White

        pnlSeatPlan.BackColor = Color.White

        LoadPlanPresetCombo()

        LoadScreens()
        ClearFields()
        txtName.Focus()
        WriteLog("SCREEN", "Screens form opened")
    End Sub

    Private Sub frmScreens_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.F5 Then
            LoadScreens()
        ElseIf e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub

    Private Sub LoadScreens()
        Dim dt As New DataTable

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT ScreenID, ScreenName, ScreenCapacity, ScreenRows, SeatsPerRow, " &
                                 "ScreenStatus, ScreenStatusReason, " &
                                 "(SELECT COUNT(*) FROM tblSeat " &
                                 "WHERE tblSeat.ScreenID = tblScreen.ScreenID) AS Seats, " &
                                 "(SELECT COUNT(*) FROM tblScreening " &
                                 "WHERE tblScreening.ScreenID = tblScreen.ScreenID) AS Screenings " &
                                 "FROM tblScreen ORDER BY ScreenName, ScreenID"
            Dim da As New OleDbDataAdapter(SQLCmd)
            da.Fill(dt)
            cn.Close()
        End If

        dt.Columns.Add("Rows", GetType(String))

        For Each row As DataRow In dt.Rows
            row("Rows") = RowsAsText(CInt(row("ScreenRows")), CInt(row("SeatsPerRow")))
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

            dgvScreens.Columns("ScreenRows").Visible = False
            dgvScreens.Columns("SeatsPerRow").Visible = False

            dgvScreens.Columns("ScreenStatusReason").Visible = False

            dgvScreens.Columns("ScreenID").Width = 42
            dgvScreens.Columns("ScreenName").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            dgvScreens.Columns("ScreenStatus").Width = 96
            dgvScreens.Columns("ScreenCapacity").Width = 72
            dgvScreens.Columns("Rows").Width = 116
            dgvScreens.Columns("Seats").Width = 84
            dgvScreens.Columns("Screenings").Width = 82

            dgvScreens.Columns("Rows").DisplayIndex = dgvScreens.Columns("Seats").DisplayIndex

            dgvScreens.Columns("ScreenCapacity").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            dgvScreens.Columns("Seats").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            dgvScreens.Columns("Screenings").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        End If

        MarkScreensThatDoNotAddUp()

        MarkScreensOutOfService()

        ShowCount(dt)
        dgvScreens.ClearSelection()
    End Sub

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

    Private Function StatusOfRow(row As DataGridViewRow) As String
        If row.Cells("ScreenStatus").Value Is Nothing OrElse IsDBNull(row.Cells("ScreenStatus").Value) Then
            Return ScreenInService
        End If

        If row.Cells("ScreenStatus").Value.ToString().Trim() = "" Then
            Return ScreenInService
        End If

        Return row.Cells("ScreenStatus").Value.ToString()
    End Function

    Private Sub ShowCount(dt As DataTable)
        Dim seats As Integer = 0

        For Each row As DataRow In dt.Rows
            seats = seats + CInt(row("Seats"))
        Next

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

    Private Function RowsAsText(numRows As Integer, perRow As Integer) As String
        If numRows <= 0 Or perRow <= 0 Then
            Return "none"
        End If

        If numRows = 1 Then
            Return "1 row of " & perRow & ", A"
        End If

        Return numRows & " rows of " & perRow & ", A to " & Chr(64 + numRows)
    End Function

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

    Private Sub Layout_TextChanged(sender As Object, e As EventArgs) Handles txtRows.TextChanged, txtPerRow.TextChanged
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
            lblLayout.Text = "The rows are lettered A to Z, so 26 rows is the most there can be"
            Exit Sub
        End If

        Dim standardSeats As Integer = 0
        Dim premiumSeats As Integer = 0
        Dim accessibleSeats As Integer = 0
        Dim saverSeats As Integer = 0
        CountPlannedSeats(standardSeats, premiumSeats, accessibleSeats, saverSeats)

        Dim mix As String = standardSeats & " standard"
        If saverSeats > 0 Then
            mix = mix & ", " & saverSeats & " saver"
        End If
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

        If DbConnect() Then
            Dim trans As OleDbTransaction = cn.BeginTransaction()

            Try
                Dim SQLCmd As New OleDbCommand
                SQLCmd.Connection = cn
                SQLCmd.Transaction = trans
                SQLCmd.CommandText = "INSERT INTO tblScreen (ScreenName, ScreenCapacity, ScreenRows, SeatsPerRow) " &
                                     "VALUES (@ScreenName, @ScreenCapacity, @ScreenRows, @SeatsPerRow)"
                SQLCmd.Parameters.AddWithValue("@ScreenName", txtName.Text.Trim())
                SQLCmd.Parameters.AddWithValue("@ScreenCapacity", numRows * perRow)
                SQLCmd.Parameters.AddWithValue("@ScreenRows", numRows)
                SQLCmd.Parameters.AddWithValue("@SeatsPerRow", perRow)
                SQLCmd.ExecuteNonQuery()

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

        Dim savedName As String = txtName.Text.Trim()

        If newScreenID > 0 Then
            Dim standardSeats As Integer = 0
            Dim premiumSeats As Integer = 0
            Dim accessibleSeats As Integer = 0
            Dim saverSeats As Integer = 0
            CountPlannedSeats(standardSeats, premiumSeats, accessibleSeats, saverSeats)

            WriteLog("SCREEN", "Screen added: " & savedName, LogChange)
            WriteLog("SCREEN", "Seats generated for ScreenID " & newScreenID & ", " & numRows & " row(s) of " &
                               perRow & ", " & (numRows * perRow) & " seats (" & standardSeats & " standard, " &
                               saverSeats & " saver, " & premiumSeats & " premium, " &
                               accessibleSeats & " accessible)", LogChange)
        End If

        LoadScreens()
        ClearFields()

        If newScreenID > 0 Then
            SayDone(lblSaved, "Added '" & savedName & "' with " & (numRows * perRow) & " seats")
        End If
    End Sub

    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
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

        Dim capacityChanged As Boolean = (newRows <> rowsWhenPicked Or newPerRow <> perRowWhenPicked)

        If capacityChanged Then
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
                Dim saverSeats As Integer = 0
                CountPlannedSeats(standardSeats, premiumSeats, accessibleSeats, saverSeats)
                WriteLog("SCREEN", "Seat plan changed for ScreenID " & selectedScreenID & ", now " &
                                   standardSeats & " standard, " & saverSeats & " saver, " &
                                   premiumSeats & " premium, " & accessibleSeats & " accessible", LogChange)
            End If
        End If

        LoadScreens()
        ClearFields()

        If saved Then
            SayDone(lblSaved, "Saved changes to '" & savedName & "'")
        End If
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If selectedScreenID = 0 Then
            MessageBox.Show("Select a screen in the grid first", "Screens", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

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

        If numRows > 26 Then
            MessageBox.Show("The rows are lettered A to Z, so a screen cannot have more than 26 rows", "Screens", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtRows.Focus()
            Return False
        End If

        If perRow > 20 Then
            MessageBox.Show("A row cannot have more than 20 seats in it, they would not fit on the seat map", "Screens", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtPerRow.Focus()
            Return False
        End If

        Return True
    End Function

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

    Private Sub SayNameIsTaken()
        MessageBox.Show("There is already a screen called '" & txtName.Text.Trim() & "'." & vbCrLf &
                        "Screens are picked by name everywhere else, so two the same cannot be told apart.",
                        "Name already used", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        txtName.Focus()
    End Sub

    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        ClearFields()
    End Sub

    Private Sub Details_Changed(sender As Object, e As EventArgs) Handles txtName.TextChanged, txtRows.TextChanged,
        txtPerRow.TextChanged
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

    Private Sub GenerateSeats(SQLCmd As OleDbCommand, screenID As Long, numRows As Integer, perRow As Integer)
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

            For seatNum As Integer = 1 To perRow
                Dim typeID As Long = TypeIDFromTable(dtTypes, PlannedTypeFor(rowIndex, seatNum - 1, numRows))
                SQLCmd.Parameters("@SeatRow").Value = rowLetter
                SQLCmd.Parameters("@SeatNumber").Value = seatNum
                SQLCmd.Parameters("@SeatTypeID").Value = CInt(typeID)
                SQLCmd.ExecuteNonQuery()
            Next
        Next
    End Sub

    Private Function SeatTypeForRow(rowIndex As Integer, numRows As Integer) As String
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

    Private Sub DeleteSeats(SQLCmd As OleDbCommand, screenID As Long)
        SQLCmd.CommandText = "DELETE FROM tblSeat " &
                             "WHERE ScreenID = @ScreenID"
        SQLCmd.Parameters.Clear()
        SQLCmd.Parameters.AddWithValue("@ScreenID", CInt(screenID))
        SQLCmd.ExecuteNonQuery()
    End Sub

    Private Sub dgvScreens_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvScreens.CellClick
        If e.RowIndex < 0 Then Exit Sub

        If Not ChangesCanBeLost() Then
            Exit Sub
        End If

        fillingBoxes = True

        Dim row As DataGridViewRow = dgvScreens.Rows(e.RowIndex)
        selectedScreenID = CLng(row.Cells("ScreenID").Value)
        txtName.Text = row.Cells("ScreenName").Value.ToString()
        txtRows.Text = row.Cells("ScreenRows").Value.ToString()
        txtPerRow.Text = row.Cells("SeatsPerRow").Value.ToString()

        rowsWhenPicked = CInt(row.Cells("ScreenRows").Value)
        perRowWhenPicked = CInt(row.Cells("SeatsPerRow").Value)

        selectedStatus = StatusOfRow(row)
        selectedReason = ReasonOfRow(row)

        fillingBoxes = False
        boxesChanged = False

        ShowWhatIsBeingEdited()
        ShowScreenDetail()
    End Sub

    Private Function ReasonOfRow(row As DataGridViewRow) As String
        If row.Cells("ScreenStatusReason").Value Is Nothing OrElse IsDBNull(row.Cells("ScreenStatusReason").Value) Then
            Return ""
        End If

        Return row.Cells("ScreenStatusReason").Value.ToString()
    End Function

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

    Private Sub LoadOverview()
        Dim seatsMade As Integer = 0
        Dim screenings As Integer = 0
        Dim upcoming As Integer = 0
        Dim sold As Integer = 0
        Dim takings As Double = 0

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT " &
                                 "(SELECT COUNT(*) FROM tblSeat " &
                                 "WHERE tblSeat.ScreenID = @ScreenID) AS SeatsMade, " &
                                 "(SELECT COUNT(*) FROM tblScreening " &
                                 "WHERE tblScreening.ScreenID = @ScreenID2) AS Screenings, " &
                                 "(SELECT COUNT(*) FROM tblScreening " &
                                 "WHERE tblScreening.ScreenID = @ScreenID3 " &
                                 "AND tblScreening.ScreeningDate >= @Today) AS Upcoming, " &
                                 "(SELECT COUNT(*) FROM tblBookingSeat " &
                                 "WHERE tblBookingSeat.ScreeningID IN " &
                                 "(SELECT ScreeningID FROM tblScreening WHERE ScreenID = @ScreenID4)) AS Sold, " &
                                 "(SELECT SUM(SeatPricePaid) FROM tblBookingSeat " &
                                 "WHERE tblBookingSeat.ScreeningID IN " &
                                 "(SELECT ScreeningID FROM tblScreening WHERE ScreenID = @ScreenID5)) AS Takings " &
                                 "FROM tblScreen WHERE ScreenID = @ScreenID6"
            SQLCmd.Parameters.AddWithValue("@ScreenID", CInt(selectedScreenID))
            SQLCmd.Parameters.AddWithValue("@ScreenID2", CInt(selectedScreenID))
            SQLCmd.Parameters.AddWithValue("@ScreenID3", CInt(selectedScreenID))
            SQLCmd.Parameters.AddWithValue("@Today", Date.Today)
            SQLCmd.Parameters.AddWithValue("@ScreenID4", CInt(selectedScreenID))
            SQLCmd.Parameters.AddWithValue("@ScreenID5", CInt(selectedScreenID))
            SQLCmd.Parameters.AddWithValue("@ScreenID6", CInt(selectedScreenID))

            Dim rs As OleDbDataReader = SQLCmd.ExecuteReader()

            If rs.Read() Then
                seatsMade = CInt(rs("SeatsMade"))
                screenings = CInt(rs("Screenings"))
                upcoming = CInt(rs("Upcoming"))
                sold = CInt(rs("Sold"))

                If Not IsDBNull(rs("Takings")) Then
                    takings = CDbl(rs("Takings"))
                End If
            End If

            rs.Close()
            cn.Close()
        End If

        Dim couldHaveSold As Integer = screenings * seatsMade
        Dim howFull As String = "no screenings yet"

        If couldHaveSold > 0 Then
            howFull = Math.Round(sold * 100.0 / couldHaveSold, 1) & "% full on average"
        End If

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
            SQLCmd.CommandText = "SELECT tblBookingSeat.ScreeningID FROM tblBookingSeat " &
                                 "INNER JOIN tblScreening ON tblBookingSeat.ScreeningID = tblScreening.ScreeningID " &
                                 "WHERE tblScreening.ScreenID = @ScreenID"
            SQLCmd.Parameters.AddWithValue("@ScreenID", CInt(screenID))
            Dim da As New OleDbDataAdapter(SQLCmd)
            da.Fill(dtSold)
            cn.Close()
        End If

        Dim slotTimes(0) As String
        Dim slotSold(0) As Integer
        Dim slotShows(0) As Integer
        Dim slotCount As Integer = 0

        For Each screening As DataRow In dtScreenings.Rows
            Dim thisTime As String = screening("ScreeningTime").ToString()
            Dim thisID As Long = CLng(screening("ScreeningID"))

            Dim soldHere As Integer = 0

            For Each seat As DataRow In dtSold.Rows
                If CLng(seat("ScreeningID")) = thisID Then
                    soldHere = soldHere + 1
                End If
            Next

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

        Dim totalSold As Integer = 0

        For Each seat As DataRow In dtSeats.Rows
            Dim seatID As Long = CLng(seat("SeatID"))
            Dim rowIndex As Integer = Asc(seat("SeatRow").ToString().ToUpper()) - 65
            Dim seatIndex As Integer = CInt(seat("SeatNumber")) - 1

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

    Private Sub pnlHeatmap_Paint(sender As Object, e As PaintEventArgs) Handles pnlHeatmap.Paint
        Dim g As Graphics = e.Graphics
        g.Clear(Color.White)

        If heatRows <= 0 Or heatPerRow <= 0 Then
            g.DrawString("No seats to show", New Font("Segoe UI", 9), Brushes.Gray, 10, 10)
            Exit Sub
        End If

        Dim letterWidth As Integer = 22
        Dim screenBar As Integer = 22
        Dim usableWidth As Integer = pnlHeatmap.Width - letterWidth - 6
        Dim usableHeight As Integer = pnlHeatmap.Height - screenBar - 6

        Dim cellSize As Integer = usableWidth \ heatPerRow

        If usableHeight \ heatRows < cellSize Then
            cellSize = usableHeight \ heatRows
        End If

        If cellSize < 4 Then
            g.DrawString("This screen is too big to draw here", New Font("Segoe UI", 9), Brushes.Gray, 10, 10)
            Exit Sub
        End If

        Dim gap As Integer = 2

        If cellSize < 14 Then
            gap = 1
        End If

        Dim startX As Integer = letterWidth + (usableWidth - (heatPerRow * cellSize)) \ 2
        Dim startY As Integer = screenBar + 4 + (usableHeight - (heatRows * cellSize)) \ 2

        Dim barWidth As Integer = heatPerRow * cellSize - gap
        g.FillRectangle(New SolidBrush(Color.FromArgb(70, 70, 78)), startX, startY - 20, barWidth, 14)
        g.DrawString("SCREEN", New Font("Segoe UI", 7.5), Brushes.White,
                     startX + (barWidth \ 2) - 24, startY - 19)

        Dim letterFont As New Font("Segoe UI", 7.5)
        Dim countFont As New Font("Segoe UI", 7.5)

        For rowIndex As Integer = 0 To heatRows - 1
            Dim y As Integer = startY + (rowIndex * cellSize)

            g.DrawString(Chr(65 + rowIndex), letterFont, Brushes.Gray, 4, y + 1)

            For seatIndex As Integer = 0 To heatPerRow - 1
                Dim x As Integer = startX + (seatIndex * cellSize)
                Dim timesSold As Integer = heatCounts(rowIndex, seatIndex)

                g.FillRectangle(New SolidBrush(HeatColour(timesSold)), x, y, cellSize - gap, cellSize - gap)

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

    Private Function HeatColour(timesSold As Integer) As Color
        If timesSold <= 0 Or heatBusiest <= 0 Then
            Return Color.FromArgb(224, 224, 228)
        End If

        Dim howHot As Double = timesSold / heatBusiest

        Dim red As Integer = CInt(255 + ((190 - 255) * howHot))
        Dim green As Integer = CInt(236 + ((30 - 236) * howHot))
        Dim blue As Integer = CInt(160 + ((45 - 160) * howHot))

        Return Color.FromArgb(red, green, blue)
    End Function

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

    Private Function PlannedTypeFor(rowIndex As Integer, seatIndex As Integer, numRows As Integer) As String
        If rowIndex < 0 Or seatIndex < 0 Or rowIndex >= planRows Or seatIndex >= planPerRow Then
            Return SeatTypeForRow(rowIndex, numRows)
        End If

        If planTypes(rowIndex, seatIndex) = "" Then
            Return SeatTypeForRow(rowIndex, numRows)
        End If

        Return planTypes(rowIndex, seatIndex)
    End Function

    Private Function NextSeatType(thisType As String) As String
        If thisType = SeatStandard Then
            Return SeatSaver
        End If

        If thisType = SeatSaver Then
            Return SeatPremium
        End If

        If thisType = SeatPremium Then
            Return SeatAccessible
        End If

        Return SeatStandard
    End Function

    Private Function PlanColour(seatType As String) As Color
        If seatType = SeatPremium Then
            Return Color.FromArgb(212, 175, 55)
        End If

        If seatType = SeatAccessible Then
            Return Color.FromArgb(60, 120, 200)
        End If

        If seatType = SeatSaver Then
            Return Color.FromArgb(70, 160, 100)
        End If

        Return Color.FromArgb(190, 195, 205)
    End Function

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

        Dim gap As Integer = 2

        If cellSize < 14 Then
            gap = 1
        End If

        Dim barWidth As Integer = planPerRow * cellSize - gap
        g.FillRectangle(New SolidBrush(Color.FromArgb(70, 70, 78)), startX, startY - 20, barWidth, 14)
        g.DrawString("SCREEN", New Font("Segoe UI", 7.5), Brushes.White,
                     startX + (barWidth \ 2) - 24, startY - 19)

        Dim letterFont As New Font("Segoe UI", 7.5)
        Dim markFont As New Font("Segoe UI", 7.5, FontStyle.Bold)

        For rowIndex As Integer = 0 To planRows - 1
            Dim y As Integer = startY + (rowIndex * cellSize)

            g.DrawString(Chr(65 + rowIndex), letterFont, Brushes.Gray, 4, y + 1)

            For seatIndex As Integer = 0 To planPerRow - 1
                Dim x As Integer = startX + (seatIndex * cellSize)
                Dim thisType As String = planTypes(rowIndex, seatIndex)

                g.FillRectangle(New SolidBrush(PlanColour(thisType)), x, y, cellSize - gap, cellSize - gap)

                If cellSize >= 14 And thisType <> SeatStandard Then
                    Dim mark As String = "P"

                    If thisType = SeatAccessible Then
                        mark = "A"
                    End If

                    If thisType = SeatSaver Then
                        mark = "S"
                    End If

                    g.DrawString(mark, markFont, Brushes.White, x + 2, y + 1)
                End If
            Next
        Next
    End Sub

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

        If e.Y < startY Then
            Exit Sub
        End If

        Dim rowIndex As Integer = (e.Y - startY) \ cellSize

        If rowIndex >= planRows Then
            Exit Sub
        End If

        If e.X < startX Then
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

        planChanged = True
        boxesChanged = True

        ShowSeatPlanKey()
        ShowLayoutPreview()
        pnlSeatPlan.Invalidate()
    End Sub

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

    Private Sub LoadPlanPresetCombo()
        cboPlanPreset.Items.Add(PlanAllStandard)
        cboPlanPreset.Items.Add(PlanCentreBlock)
        cboPlanPreset.Items.Add(PlanPremiumBack)
        cboPlanPreset.Items.Add(PlanBudget)
        cboPlanPreset.SelectedIndex = 1
    End Sub

    Private Sub btnApplyPlan_Click(sender As Object, e As EventArgs) Handles btnApplyPlan.Click
        If planRows <= 0 Then
            MessageBox.Show("Type how many rows and how many seats in each row first.", "No plan yet", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        Dim planName As String = cboPlanPreset.Text

        For rowIndex As Integer = 0 To planRows - 1
            For seatIndex As Integer = 0 To planPerRow - 1
                planTypes(rowIndex, seatIndex) = PlanSeatType(planName, rowIndex, seatIndex, planRows, planPerRow)
            Next
        Next

        planChanged = True
        boxesChanged = True
        ShowSeatPlanKey()
        ShowLayoutPreview()
        pnlSeatPlan.Invalidate()
        SayDone(lblSaved, "Plan set to '" & planName & "'. Press Save changes to keep it.")
    End Sub

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

    Private Sub ShowSeatPlanKey()
        If planRows <= 0 Or planPerRow <= 0 Then
            lblSeatPlanInfo.Text = "Type how many rows and how many seats in each row first," & vbCrLf &
                                   "then the plan of the room can be marked out here."
            lblSeatPlanKey.Text = ""
            Exit Sub
        End If

        lblSeatPlanInfo.Text = "Click a seat to change what sort it is. Standard, then" & vbCrLf &
                               "saver, then premium, then accessible, then round again." & vbCrLf &
                               "Clicking a row letter changes that whole row."

        Dim standardSeats As Integer = 0
        Dim premiumSeats As Integer = 0
        Dim accessibleSeats As Integer = 0
        Dim saverSeats As Integer = 0
        CountPlannedSeats(standardSeats, premiumSeats, accessibleSeats, saverSeats)

        lblSeatPlanKey.Text = "Grey is standard, green is saver (S), gold is premium (P)," & vbCrLf &
                              "blue is accessible (A). What a seat costs comes from its sort." & vbCrLf &
                              standardSeats & " standard, " & saverSeats & " saver, " &
                              premiumSeats & " premium, " & accessibleSeats & " accessible."
    End Sub

    Private Sub CountPlannedSeats(ByRef standardSeats As Integer, ByRef premiumSeats As Integer, ByRef accessibleSeats As Integer, ByRef saverSeats As Integer)
        standardSeats = 0
        premiumSeats = 0
        accessibleSeats = 0
        saverSeats = 0

        For rowIndex As Integer = 0 To planRows - 1
            For seatIndex As Integer = 0 To planPerRow - 1
                Dim thisType As String = planTypes(rowIndex, seatIndex)

                If thisType = SeatPremium Then
                    premiumSeats = premiumSeats + 1
                ElseIf thisType = SeatAccessible Then
                    accessibleSeats = accessibleSeats + 1
                ElseIf thisType = SeatSaver Then
                    saverSeats = saverSeats + 1
                Else
                    standardSeats = standardSeats + 1
                End If
            Next
        Next
    End Sub

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

            If rowIndex >= 0 And rowIndex < planRows And seatIndex >= 0 And seatIndex < planPerRow Then
                planTypes(rowIndex, seatIndex) = seat("SeatTypeName").ToString()
            End If
        Next

        planChanged = False

        ShowSeatPlanKey()
        ShowLayoutPreview()
        pnlSeatPlan.Invalidate()
    End Sub

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

    Private Sub LoadScreeningsForScreen()
        Dim dt As New DataTable

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT tblScreening.ScreeningID, ScreeningDate, ScreeningTime, FilmTitle, " &
                                 "(SELECT COUNT(*) FROM tblBookingSeat " &
                                 "WHERE tblBookingSeat.ScreeningID = tblScreening.ScreeningID) AS SeatsSold " &
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

        Dim dtShow As New DataTable
        dtShow.Columns.Add("When", GetType(String))
        dtShow.Columns.Add("Film", GetType(String))
        dtShow.Columns.Add("Sold", GetType(String))

        Dim withBookings As Integer = 0

        For Each row As DataRow In dt.Rows
            Dim soldHere As Integer = CInt(row("SeatsSold"))
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

    Private Sub ShowStatusButtons()
        If selectedStatus = ScreenOutOfService Then
            lblScreenState.Text = "This screen is OUT OF SERVICE" & vbCrLf & "Reason: " & selectedReason
            lblScreenState.ForeColor = Color.FromArgb(170, 40, 40)

            btnOutOfService.Enabled = False
            btnBackInService.Enabled = True
            txtReason.Enabled = False
            txtReason.Text = selectedReason

            lblStatusHint.Text = "Nothing new can be scheduled here and no" & vbCrLf &
                                 "more tickets can be sold for what is already" & vbCrLf &
                                 "in it, until it is put back."
        Else
            lblScreenState.Text = "This screen is in service"
            lblScreenState.ForeColor = AccentFore

            btnOutOfService.Enabled = True
            btnBackInService.Enabled = False
            txtReason.Enabled = True
            txtReason.Text = ""

            lblStatusHint.Text = "Taking a screen out of service stops new" & vbCrLf &
                                 "screenings going in it and stops tickets being" & vbCrLf &
                                 "sold for it. Its seats and history stay as they are."
        End If
    End Sub

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

    Private Sub btnBackInService_Click(sender As Object, e As EventArgs) Handles btnBackInService.Click
        If selectedScreenID = 0 Then
            MessageBox.Show("Select a screen in the grid first", "Screens", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

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

    Private Sub SetScreenStatus(newStatus As String, reason As String)
        Dim saved As Boolean = False

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
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
