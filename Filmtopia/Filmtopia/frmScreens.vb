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

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "INSERT INTO tblScreen (ScreenName, ScreenCapacity) VALUES (@ScreenName, @ScreenCapacity)"
            SQLCmd.Parameters.AddWithValue("@ScreenName", txtName.Text)
            SQLCmd.Parameters.AddWithValue("@ScreenCapacity", Val(txtCapacity.Text))
            SQLCmd.ExecuteNonQuery()
            cn.Close()
        End If

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

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "UPDATE tblScreen SET ScreenName = @ScreenName, ScreenCapacity = @ScreenCapacity WHERE ScreenID = @ScreenID"
            SQLCmd.Parameters.AddWithValue("@ScreenName", txtName.Text)
            SQLCmd.Parameters.AddWithValue("@ScreenCapacity", Val(txtCapacity.Text))
            SQLCmd.Parameters.AddWithValue("@ScreenID", selectedScreenID)
            SQLCmd.ExecuteNonQuery()
            cn.Close()
        End If

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

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "DELETE FROM tblScreen WHERE ScreenID = @ScreenID"
            SQLCmd.Parameters.AddWithValue("@ScreenID", selectedScreenID)
            SQLCmd.ExecuteNonQuery()
            cn.Close()
        End If

        WriteLog("SCREEN", "Screen deleted: " & txtName.Text)
        LoadScreens()
        ClearFields()
    End Sub

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
