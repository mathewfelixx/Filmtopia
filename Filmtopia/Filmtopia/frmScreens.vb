Imports System.Data.OleDb

Public Class frmScreens

    Private selectedScreenID As Integer = 0
    Private selectedScreenIsActive As Boolean = True

    Private seatTypeScreenID As Integer = 0
    Private seatMapIDs() As Integer
    Private seatMapTypeIDs() As Integer

    Private seatStandardColour As Color = Color.FromArgb(220, 220, 220)
    Private seatPremiumColour As Color = Color.FromArgb(255, 214, 100)
    Private seatAccessibleColour As Color = Color.FromArgb(150, 200, 240)
    Private seatSelectedColour As Color = Color.Fuchsia

    Private Sub frmScreens_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CommonFormStartup()
        LoadScreens()
        LoadSeatTypeScreens()
        LoadTargetTypes()
        ApplySeatTypeSwatches()
        If UserAccessLevel <> 1 Then
            tabScreens.TabPages.Remove(tabSeatTypes)
        End If
        WriteLog("SCREEN", "Screens form opened")
    End Sub

    Private Sub LoadScreens()
        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT ScreenID, ScreenName, ScreenCapacity, IsActive, " &
                                 "IIF(IsActive, 'In service', 'Out of service') AS Status " &
                                 "FROM tblScreen WHERE (@ShowAll = True OR IsActive = True)"
            SQLCmd.Parameters.AddWithValue("@ShowAll", chkShowInactive.Checked)
            Dim da As New OleDbDataAdapter(SQLCmd)
            Dim dt As New DataTable
            da.Fill(dt)
            dgvScreens.DataSource = dt
            cn.Close()
        End If

        dgvScreens.Columns("IsActive").Visible = False
        dgvScreens.Columns("ScreenName").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        dgvScreens.DefaultCellStyle.WrapMode = DataGridViewTriState.True
        dgvScreens.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells

        WriteLog("SCREEN", "Screen list loaded")
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        If txtName.Text = "" Then
            MessageBox.Show("Enter a screen name")
            Exit Sub
        End If

        If Not CapacityIsValid() Then Exit Sub

        Dim newScreenID As Integer = 0

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "INSERT INTO tblScreen (ScreenName, ScreenCapacity, IsActive) " &
                                 "VALUES (@ScreenName, @ScreenCapacity, @IsActive)"
            SQLCmd.Parameters.AddWithValue("@ScreenName", txtName.Text)
            SQLCmd.Parameters.AddWithValue("@ScreenCapacity", Val(txtCapacity.Text))
            SQLCmd.Parameters.AddWithValue("@IsActive", True)
            SQLCmd.ExecuteNonQuery()

            SQLCmd.CommandText = "SELECT @@IDENTITY"
            newScreenID = CInt(SQLCmd.ExecuteScalar())
            cn.Close()
        End If

        GenerateSeats(newScreenID, Val(txtCapacity.Text))

        WriteLog("SCREEN", "Screen added: " & txtName.Text)
        LoadScreens()
        LoadSeatTypeScreens()
        ClearFields()
    End Sub

    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        If selectedScreenID = 0 Then
            MessageBox.Show("Select a screen in the grid first")
            Exit Sub
        End If

        If txtName.Text = "" Then
            MessageBox.Show("Enter a screen name")
            Exit Sub
        End If
        If Not CapacityIsValid() Then Exit Sub

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "UPDATE tblScreen " &
                                 "SET ScreenName = @ScreenName, ScreenCapacity = @ScreenCapacity " &
                                 "WHERE ScreenID = @ScreenID"
            SQLCmd.Parameters.AddWithValue("@ScreenName", txtName.Text)
            SQLCmd.Parameters.AddWithValue("@ScreenCapacity", Val(txtCapacity.Text))
            SQLCmd.Parameters.AddWithValue("@ScreenID", selectedScreenID)
            SQLCmd.ExecuteNonQuery()
            cn.Close()
        End If

        DeleteSeats(selectedScreenID)
        GenerateSeats(selectedScreenID, Val(txtCapacity.Text))

        WriteLog("SCREEN", "Screen updated: " & txtName.Text)
        LoadScreens()
        LoadSeatTypeScreens()
        ClearFields()
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If selectedScreenID = 0 Then
            MessageBox.Show("Select a screen in the grid first")
            Exit Sub
        End If

        Dim takingOut As Boolean = selectedScreenIsActive
        Dim confirmMsg As String
        If takingOut Then
            confirmMsg = "Take this screen out of service? Its record and bookings are kept, but it will not be offered for new screenings."
        Else
            confirmMsg = "Return this screen to service?"
        End If

        If MessageBox.Show(confirmMsg, "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = DialogResult.No Then
            Exit Sub
        End If

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "UPDATE tblScreen SET IsActive = @IsActive WHERE ScreenID = @ScreenID"
            SQLCmd.Parameters.AddWithValue("@IsActive", Not takingOut)
            SQLCmd.Parameters.AddWithValue("@ScreenID", selectedScreenID)
            SQLCmd.ExecuteNonQuery()
            cn.Close()
        End If

        If takingOut Then
            WriteLog("SCREEN", "Screen taken out of service: " & txtName.Text)
        Else
            WriteLog("SCREEN", "Screen returned to service: " & txtName.Text)
        End If

        LoadScreens()
        LoadSeatTypeScreens()
        ClearFields()
    End Sub

    Private Function CapacityIsValid() As Boolean
        Dim capacity As Integer = Val(txtCapacity.Text)

        If capacity <= 0 Or capacity Mod 10 <> 0 Then
            MessageBox.Show("Screen capacity must be a multiple of 10 (e.g. 10, 20, 30...)")
            Return False
        End If

        Return True
    End Function

    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        ClearFields()
        WriteLog("SCREEN", "Screen fields cleared")
    End Sub

    Private Sub ClearFields()
        selectedScreenID = 0
        selectedScreenIsActive = True
        txtName.Text = ""
        txtCapacity.Text = ""
        btnDelete.Text = "Take out of service"
        dgvScreens.ClearSelection()
    End Sub

    Private Sub chkShowInactive_CheckedChanged(sender As Object, e As EventArgs) Handles chkShowInactive.CheckedChanged
        LoadScreens()
        ClearFields()
    End Sub

    Private Sub GenerateSeats(screenID As Integer, capacity As Integer)
        Dim numRows As Integer = capacity \ 10

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "INSERT INTO tblSeat (ScreenID, SeatRow, SeatNumber) " &
                                 "VALUES (@ScreenID, @SeatRow, @SeatNumber)"
            SQLCmd.Parameters.AddWithValue("@ScreenID", screenID)
            SQLCmd.Parameters.AddWithValue("@SeatRow", "")
            SQLCmd.Parameters.AddWithValue("@SeatNumber", 0)

            For rowIndex As Integer = 0 To numRows - 1
                Dim rowLetter As String = Chr(65 + rowIndex)
                For seatNum As Integer = 1 To 10
                    SQLCmd.Parameters("@SeatRow").Value = rowLetter
                    SQLCmd.Parameters("@SeatNumber").Value = seatNum
                    SQLCmd.ExecuteNonQuery()
                Next
            Next

            cn.Close()
        End If

        WriteLog("SCREEN", "Seats generated for ScreenID " & screenID)
    End Sub

    Private Sub DeleteSeats(screenID As Integer)
        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "DELETE FROM tblSeat " &
                                 "WHERE ScreenID = @ScreenID"
            SQLCmd.Parameters.AddWithValue("@ScreenID", screenID)
            SQLCmd.ExecuteNonQuery()
            cn.Close()
        End If
    End Sub

    Private Sub dgvScreens_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvScreens.CellClick
        If e.RowIndex < 0 Then Exit Sub

        Dim row As DataGridViewRow = dgvScreens.Rows(e.RowIndex)
        selectedScreenID = CInt(row.Cells("ScreenID").Value)
        txtName.Text = row.Cells("ScreenName").Value.ToString()
        txtCapacity.Text = row.Cells("ScreenCapacity").Value.ToString()
        selectedScreenIsActive = CBool(row.Cells("IsActive").Value)

        If selectedScreenIsActive Then
            btnDelete.Text = "Take out of service"
        Else
            btnDelete.Text = "Return to service"
        End If

        WriteLog("SCREEN", "Screen selected: " & txtName.Text)
    End Sub

    Private Sub LoadSeatTypeScreens()
        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT ScreenID, ScreenName " &
                                 "FROM tblScreen WHERE IsActive = True ORDER BY ScreenName"
            Dim da As New OleDbDataAdapter(SQLCmd)
            Dim dt As New DataTable
            da.Fill(dt)
            cboSeatTypeScreen.DataSource = dt
            cboSeatTypeScreen.DisplayMember = "ScreenName"
            cboSeatTypeScreen.ValueMember = "ScreenID"
            cboSeatTypeScreen.SelectedIndex = -1
            cn.Close()
        End If
    End Sub

    Private Sub LoadTargetTypes()
        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT SeatTypeID, TypeName " &
                                 "FROM tblSeatType ORDER BY SeatTypeID"
            Dim da As New OleDbDataAdapter(SQLCmd)
            Dim dt As New DataTable
            da.Fill(dt)
            cboTargetType.DataSource = dt
            cboTargetType.DisplayMember = "TypeName"
            cboTargetType.ValueMember = "SeatTypeID"
            cboTargetType.SelectedIndex = -1
            cn.Close()
        End If
    End Sub

    Private Sub ApplySeatTypeSwatches()
        lblStdKey.BackColor = seatStandardColour
        lblPremKey.BackColor = seatPremiumColour
        lblAccKey.BackColor = seatAccessibleColour
    End Sub

    Private Function ColourForType(typeID As Integer) As Color
        If typeID = 2 Then
            Return seatPremiumColour
        ElseIf typeID = 3 Then
            Return seatAccessibleColour
        Else
            Return seatStandardColour
        End If
    End Function

    Private Function TypeForSeat(seatID As Integer) As Integer
        For i As Integer = 0 To seatMapIDs.Length - 1
            If seatMapIDs(i) = seatID Then
                Return seatMapTypeIDs(i)
            End If
        Next
        Return 1
    End Function

    Private Sub cboSeatTypeScreen_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboSeatTypeScreen.SelectedIndexChanged
        If cboSeatTypeScreen.SelectedIndex = -1 Then
            Exit Sub
        End If
        If Not IsNumeric(cboSeatTypeScreen.SelectedValue) Then
            Exit Sub
        End If
        seatTypeScreenID = CInt(cboSeatTypeScreen.SelectedValue)
        BuildSeatTypeMap()
    End Sub

    Private Sub BuildSeatTypeMap()
        pnlSeatTypeMap.Controls.Clear()

        If seatTypeScreenID = 0 Then
            Exit Sub
        End If

        Dim dtSeats As New DataTable
        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT SeatID, SeatRow, SeatNumber, SeatTypeID " &
                                 "FROM tblSeat WHERE ScreenID = @ScreenID " &
                                 "ORDER BY SeatRow, SeatNumber"
            SQLCmd.Parameters.AddWithValue("@ScreenID", seatTypeScreenID)
            Dim da As New OleDbDataAdapter(SQLCmd)
            da.Fill(dtSeats)
            cn.Close()
        End If

        If dtSeats.Rows.Count > 0 Then
            ReDim seatMapIDs(dtSeats.Rows.Count - 1)
            ReDim seatMapTypeIDs(dtSeats.Rows.Count - 1)
        Else
            seatMapIDs = New Integer() {}
            seatMapTypeIDs = New Integer() {}
        End If

        For i As Integer = 0 To dtSeats.Rows.Count - 1
            Dim seatID As Integer = CInt(dtSeats.Rows(i)("SeatID"))
            Dim seatRow As String = dtSeats.Rows(i)("SeatRow").ToString()
            Dim seatNumber As Integer = CInt(dtSeats.Rows(i)("SeatNumber"))
            Dim typeID As Integer = CInt(dtSeats.Rows(i)("SeatTypeID"))
            seatMapIDs(i) = seatID
            seatMapTypeIDs(i) = typeID

            Dim b As New Button
            b.Tag = seatID
            b.Text = seatRow & seatNumber
            b.Size = New Size(40, 35)
            b.Font = New Font("Segoe UI", 7)
            Dim rowIndex As Integer = Asc(seatRow) - 65
            b.Location = New Point((seatNumber - 1) * 45 + 10, rowIndex * 45 + 10)
            b.BackColor = ColourForType(typeID)
            AddHandler b.Click, AddressOf SeatType_Click
            pnlSeatTypeMap.Controls.Add(b)
        Next
    End Sub

    Private Sub SeatType_Click(sender As Object, e As EventArgs)
        Dim b As Button = CType(sender, Button)
        If b.BackColor = seatSelectedColour Then
            b.BackColor = ColourForType(TypeForSeat(CInt(b.Tag)))
        Else
            b.BackColor = seatSelectedColour
        End If
    End Sub

    Private Sub btnClearSel_Click(sender As Object, e As EventArgs) Handles btnClearSel.Click
        For Each ctrl As Control In pnlSeatTypeMap.Controls
            If TypeOf ctrl Is Button Then
                Dim b As Button = CType(ctrl, Button)
                If b.BackColor = seatSelectedColour Then
                    b.BackColor = ColourForType(TypeForSeat(CInt(b.Tag)))
                End If
            End If
        Next
    End Sub

    Private Sub btnApplyType_Click(sender As Object, e As EventArgs) Handles btnApplyType.Click
        If UserAccessLevel <> 1 Then
            MessageBox.Show("Only a manager can change seat types")
            Exit Sub
        End If
        If seatTypeScreenID = 0 Then
            MessageBox.Show("Pick a screen first")
            Exit Sub
        End If
        If cboTargetType.SelectedIndex = -1 Then
            MessageBox.Show("Pick a seat type to set")
            Exit Sub
        End If

        Dim targetTypeID As Integer = CInt(cboTargetType.SelectedValue)
        Dim changed As Integer = 0

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            For Each ctrl As Control In pnlSeatTypeMap.Controls
                If TypeOf ctrl Is Button Then
                    Dim b As Button = CType(ctrl, Button)
                    If b.BackColor = seatSelectedColour Then
                        SQLCmd.CommandText = "UPDATE tblSeat SET SeatTypeID = @SeatTypeID " &
                                             "WHERE SeatID = @SeatID"
                        SQLCmd.Parameters.Clear()
                        SQLCmd.Parameters.AddWithValue("@SeatTypeID", targetTypeID)
                        SQLCmd.Parameters.AddWithValue("@SeatID", CInt(b.Tag))
                        SQLCmd.ExecuteNonQuery()
                        changed = changed + 1
                    End If
                End If
            Next
            cn.Close()
        End If

        If changed = 0 Then
            MessageBox.Show("Select at least one seat first")
            Exit Sub
        End If

        WriteLog("SCREEN", changed & " seats set to type " & targetTypeID & " on ScreenID " & seatTypeScreenID)
        BuildSeatTypeMap()
    End Sub

End Class
