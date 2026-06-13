Imports System.Data.OleDb

Public Class frmScreens

    Private selectedScreenID As Long = 0

    Private Sub frmScreens_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CommonFormStartup()
        LoadScreens()
    End Sub

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

        dgvScreens.Columns("ScreenName").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        dgvScreens.DefaultCellStyle.WrapMode = DataGridViewTriState.True
        dgvScreens.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
    End Sub

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

        LoadScreens()
        ClearFields()
    End Sub

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

        LoadScreens()
        ClearFields()
    End Sub

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

        LoadScreens()
        ClearFields()
    End Sub

    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        ClearFields()
    End Sub

    Private Sub ClearFields()
        selectedScreenID = 0
        txtName.Text = ""
        txtCapacity.Text = ""
        dgvScreens.ClearSelection()
    End Sub

    Private Sub dgvScreens_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvScreens.CellClick
        If e.RowIndex < 0 Then Exit Sub

        Dim row As DataGridViewRow = dgvScreens.Rows(e.RowIndex)
        selectedScreenID = CLng(row.Cells("ScreenID").Value)
        txtName.Text = row.Cells("ScreenName").Value.ToString()
        txtCapacity.Text = row.Cells("ScreenCapacity").Value.ToString()
    End Sub

End Class
