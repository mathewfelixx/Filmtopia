Imports System.Data.OleDb

Public Class frmScreens
    Private Const SeatsPerRow As Integer = 10

    Private selectedScreenID As Long = 0
    Private selectedScreenName As String = ""

    Private Sub frmScreens_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadScreens()
    End Sub

    Private Sub LoadScreens()
        Dim SQLCmd As New OleDbCommand("SELECT ScreenID, ScreenName, ScreenCapacity " &
                                       "FROM tblScreen ORDER BY ScreenName")
        Dim dt As New DataTable
        If FillTable(SQLCmd, dt) Then
            dgvScreens.DataSource = dt
            dgvScreens.Columns("ScreenID").Visible = False
            dgvScreens.Columns("ScreenName").HeaderText = "Name"
            dgvScreens.Columns("ScreenCapacity").HeaderText = "Seats"
            dgvScreens.Columns("ScreenName").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        End If
    End Sub

    Private Function DetailsAreValid(screenID As Long) As Boolean
        Dim valid As Boolean = False
        Dim seats As Integer

        If txtName.Text.Trim() = "" Then
            MessageBox.Show("Please enter the screen's name.", "Missing Details", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtName.Focus()

        ElseIf Not Integer.TryParse(txtSeats.Text.Trim(), seats) Then
            MessageBox.Show("The number of seats must be a whole number.", "Invalid Number of Seats", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtSeats.Focus()

        ElseIf seats < 10 Or seats > 260 Or seats Mod SeatsPerRow <> 0 Then
            MessageBox.Show("Seats are set out in rows of 10 lettered A to Z, so the number of seats must be a multiple of 10 from 10 to 260.", "Invalid Number of Seats", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtSeats.Focus()

        ElseIf NameIsTaken(txtName.Text.Trim(), screenID) Then
            MessageBox.Show("There is already a screen called " & txtName.Text.Trim() & ".", "Duplicate Screen", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtName.Focus()

        Else
            valid = True
        End If

        Return valid
    End Function

    Private Function NameIsTaken(screenName As String, screenID As Long) As Boolean
        Dim SQLCmd As New OleDbCommand("SELECT COUNT(*) FROM tblScreen WHERE ScreenName = @ScreenName AND ScreenID <> @ScreenID")
        SQLCmd.Parameters.AddWithValue("@ScreenName", screenName)
        SQLCmd.Parameters.AddWithValue("@ScreenID", screenID)
        Dim matches As Integer = GetValue(SQLCmd)
        Return matches > 0
    End Function

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        If DetailsAreValid(0) Then
            Dim screenName As String = txtName.Text.Trim()
            Dim seats As Integer = CInt(txtSeats.Text.Trim())
            If SaveNewScreen(screenName, seats) Then
                WriteLog(LogScreen, "Added screen " & screenName & " with " & seats & " seats")
                LoadScreens()
                ClearFields()
            End If
        End If
    End Sub

    Private Function SaveNewScreen(screenName As String, seats As Integer) As Boolean
        Dim saved As Boolean = False

        If DbConnect() Then
            Dim trans As OleDbTransaction = cn.BeginTransaction()
            Try
                Dim SQLCmd As New OleDbCommand
                SQLCmd.Connection = cn
                SQLCmd.Transaction = trans
                SQLCmd.CommandText = "INSERT INTO tblScreen (ScreenName, ScreenCapacity) VALUES (@ScreenName, @ScreenCapacity)"
                SQLCmd.Parameters.AddWithValue("@ScreenName", screenName)
                SQLCmd.Parameters.AddWithValue("@ScreenCapacity", seats)
                SQLCmd.ExecuteNonQuery()

                SQLCmd.Parameters.Clear()
                SQLCmd.CommandText = "SELECT @@IDENTITY"
                Dim newScreenID As Long = SQLCmd.ExecuteScalar()

                SQLCmd.CommandText = "INSERT INTO tblSeat (ScreenID, SeatRow, SeatNumber) VALUES (@ScreenID, @SeatRow, @SeatNumber)"
                For rowNumber As Integer = 0 To seats \ SeatsPerRow - 1
                    For seatNumber As Integer = 1 To SeatsPerRow
                        SQLCmd.Parameters.Clear()
                        SQLCmd.Parameters.AddWithValue("@ScreenID", newScreenID)
                        SQLCmd.Parameters.AddWithValue("@SeatRow", Chr(Asc("A") + rowNumber))
                        SQLCmd.Parameters.AddWithValue("@SeatNumber", seatNumber)
                        SQLCmd.ExecuteNonQuery()
                    Next
                Next

                trans.Commit()
                saved = True
            Catch ex As Exception
                trans.Rollback()
                MessageBox.Show("The screen could not be saved, so nothing was added. " & ex.Message, "Save Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
            cn.Close()
        End If

        Return saved
    End Function

    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        If selectedScreenID = 0 Then
            MessageBox.Show("Please pick a screen from the list first.", "No Screen Picked", MessageBoxButtons.OK, MessageBoxIcon.Information)
        ElseIf DetailsAreValid(selectedScreenID) Then
            Dim newName As String = txtName.Text.Trim()
            If newName = selectedScreenName Then
                MessageBox.Show("That is already the screen's name.", "Nothing to Change", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                Dim SQLCmd As New OleDbCommand("UPDATE tblScreen SET ScreenName = @ScreenName WHERE ScreenID = @ScreenID")
                SQLCmd.Parameters.AddWithValue("@ScreenName", newName)
                SQLCmd.Parameters.AddWithValue("@ScreenID", selectedScreenID)
                If RunCommand(SQLCmd) Then
                    WriteLog(LogScreen, "Renamed screen " & selectedScreenName & " to " & newName & " (screen number " & selectedScreenID & ")")
                    LoadScreens()
                    ClearFields()
                End If
            End If
        End If
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If selectedScreenID = 0 Then
            MessageBox.Show("Please pick a screen from the list first.", "No Screen Picked", MessageBoxButtons.OK, MessageBoxIcon.Information)
        ElseIf MessageBox.Show("Delete " & selectedScreenName & " and all of its seats? This cannot be undone.", "Delete Screen", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) = DialogResult.Yes Then
            Dim deleteSeats As New OleDbCommand("DELETE FROM tblSeat WHERE ScreenID = @ScreenID")
            deleteSeats.Parameters.AddWithValue("@ScreenID", selectedScreenID)
            Dim deleteScreen As New OleDbCommand("DELETE FROM tblScreen WHERE ScreenID = @ScreenID")
            deleteScreen.Parameters.AddWithValue("@ScreenID", selectedScreenID)
            Dim commands() As OleDbCommand = {deleteSeats, deleteScreen}
            If RunCommands(commands) Then
                WriteLog(LogScreen, "Deleted screen " & selectedScreenName & " and its seats (screen number " & selectedScreenID & ")")
                LoadScreens()
                ClearFields()
            End If
        End If
    End Sub

    Private Sub frmScreens_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        dgvScreens.ClearSelection()
    End Sub

    Private Sub dgvScreens_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs) Handles dgvScreens.DataBindingComplete
        dgvScreens.ClearSelection()
    End Sub

    Private Sub dgvScreens_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvScreens.CellClick
        If e.RowIndex >= 0 Then
            Dim row As DataGridViewRow = dgvScreens.Rows(e.RowIndex)
            selectedScreenID = row.Cells("ScreenID").Value
            selectedScreenName = row.Cells("ScreenName").Value
            txtName.Text = row.Cells("ScreenName").Value
            txtSeats.Text = row.Cells("ScreenCapacity").Value
            txtSeats.ReadOnly = True
        End If
    End Sub

    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        ClearFields()
    End Sub

    Private Sub ClearFields()
        selectedScreenID = 0
        selectedScreenName = ""
        dgvScreens.ClearSelection()
        txtName.Text = ""
        txtSeats.Text = ""
        txtSeats.ReadOnly = False
        txtName.Focus()
    End Sub
End Class
