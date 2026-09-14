Imports System.Data.OleDb

Public Class frmScreens

    Private selectedScreenID As Integer = 0
    Private selectedScreenIsActive As Boolean = True

    Private Sub frmScreens_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CommonFormStartup()
        LoadScreens()
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

End Class
