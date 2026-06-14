Imports System.Data.OleDb

Public Class frmScreens

    'tracks the ScreenID of the row currently selected in the grid, 0 means nothing selected
    Private selectedScreenID As Long = 0

    Private Sub frmScreens_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CommonFormStartup()
        LoadScreens()
        WriteLog("SCREEN", "Screens form opened")
    End Sub

    'loads all screens from tblScreen into the grid
    Private Sub LoadScreens()
        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT ScreenID, ScreenName, ScreenCapacity FROM tblScreen"
            Dim da As New OleDbDataAdapter(SQLCmd)
            Dim dt As New DataTable
            da.Fill(dt)
            dgvScreens.DataSource = dt
            cn.Close()
        End If

        'let the name column stretch out and wrap so its all readable
        dgvScreens.Columns("ScreenName").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        dgvScreens.DefaultCellStyle.WrapMode = DataGridViewTriState.True
        dgvScreens.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells

        WriteLog("SCREEN", "Screen list loaded")
    End Sub

    'adds a new screen using the values typed into the textboxes
    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        If txtName.Text = "" Then
            MessageBox.Show("Enter a screen name")
            Exit Sub
        End If

        If Not CapacityIsValid() Then Exit Sub

        Dim newScreenID As Long = 0

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "INSERT INTO tblScreen (ScreenName, ScreenCapacity) VALUES (@ScreenName, @ScreenCapacity)"
            SQLCmd.Parameters.AddWithValue("@ScreenName", txtName.Text)
            SQLCmd.Parameters.AddWithValue("@ScreenCapacity", Val(txtCapacity.Text))
            SQLCmd.ExecuteNonQuery()

            'grab the ID just given to the new screen so we can generate its seats
            SQLCmd.CommandText = "SELECT @@IDENTITY"
            newScreenID = CLng(SQLCmd.ExecuteScalar())
            cn.Close()
        End If

        GenerateSeats(newScreenID, Val(txtCapacity.Text))

        WriteLog("SCREEN", "Screen added: " & txtName.Text)
        LoadScreens()
        ClearFields()
    End Sub

    'updates the currently selected screen with the values in the textboxes
    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        If selectedScreenID = 0 Then
            MessageBox.Show("Select a screen in the grid first")
            Exit Sub
        End If

        If Not CapacityIsValid() Then Exit Sub

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "UPDATE tblScreen SET ScreenName = @ScreenName, ScreenCapacity = @ScreenCapacity WHERE ScreenID = @ScreenID"
            SQLCmd.Parameters.AddWithValue("@ScreenName", txtName.Text)
            SQLCmd.Parameters.AddWithValue("@ScreenCapacity", Val(txtCapacity.Text))
            SQLCmd.Parameters.AddWithValue("@ScreenID", CInt(selectedScreenID))
            SQLCmd.ExecuteNonQuery()
            cn.Close()
        End If

        'capacity may have changed so wipe the old seats and generate fresh ones
        DeleteSeats(selectedScreenID)
        GenerateSeats(selectedScreenID, Val(txtCapacity.Text))

        WriteLog("SCREEN", "Screen updated: " & txtName.Text)
        LoadScreens()
        ClearFields()
    End Sub

    'deletes the currently selected screen
    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If selectedScreenID = 0 Then
            MessageBox.Show("Select a screen in the grid first")
            Exit Sub
        End If

        If MessageBox.Show("Delete this screen?", "Confirm", MessageBoxButtons.YesNo) = DialogResult.No Then
            Exit Sub
        End If

        'remove the seats that belong to this screen first
        DeleteSeats(selectedScreenID)

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "DELETE FROM tblScreen WHERE ScreenID = @ScreenID"
            SQLCmd.Parameters.AddWithValue("@ScreenID", CInt(selectedScreenID))
            SQLCmd.ExecuteNonQuery()
            cn.Close()
        End If

        WriteLog("SCREEN", "Screen deleted: " & txtName.Text)
        LoadScreens()
        ClearFields()
    End Sub

    'checks the capacity box is a whole multiple of 10, since seats are generated in rows of 10
    Private Function CapacityIsValid() As Boolean
        Dim capacity As Integer = Val(txtCapacity.Text)

        If capacity <= 0 Or capacity Mod 10 <> 0 Then
            MessageBox.Show("Screen capacity must be a multiple of 10 (e.g. 10, 20, 30...)")
            Return False
        End If

        Return True
    End Function

    'clears the textboxes and the selection
    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        ClearFields()
        WriteLog("SCREEN", "Screen fields cleared")
    End Sub

    Private Sub ClearFields()
        selectedScreenID = 0
        txtName.Text = ""
        txtCapacity.Text = ""
        dgvScreens.ClearSelection()
    End Sub

    'makes a row of 10 seats for every 10 seats of capacity, rows go A, B, C...
    Private Sub GenerateSeats(screenID As Long, capacity As Integer)
        Dim numRows As Integer = capacity \ 10

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "INSERT INTO tblSeat (ScreenID, SeatRow, SeatNumber) VALUES (@ScreenID, @SeatRow, @SeatNumber)"
            SQLCmd.Parameters.AddWithValue("@ScreenID", CInt(screenID))
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

    'removes every seat that belongs to a screen
    Private Sub DeleteSeats(screenID As Long)
        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "DELETE FROM tblSeat WHERE ScreenID = @ScreenID"
            SQLCmd.Parameters.AddWithValue("@ScreenID", CInt(screenID))
            SQLCmd.ExecuteNonQuery()
            cn.Close()
        End If
    End Sub

    'when a row is clicked, load its values into the textboxes for editing
    Private Sub dgvScreens_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvScreens.CellClick
        If e.RowIndex < 0 Then Exit Sub

        Dim row As DataGridViewRow = dgvScreens.Rows(e.RowIndex)
        selectedScreenID = CLng(row.Cells("ScreenID").Value)
        txtName.Text = row.Cells("ScreenName").Value.ToString()
        txtCapacity.Text = row.Cells("ScreenCapacity").Value.ToString()
        WriteLog("SCREEN", "Screen selected: " & txtName.Text)
    End Sub

End Class
