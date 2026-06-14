Imports System.Data.OleDb

Public Class frmFoodItems

    'tracks the FoodItemID of the row currently selected in the grid, 0 means nothing selected
    Private selectedFoodItemID As Long = 0

    Private Sub frmFoodItems_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CommonFormStartup()
        LoadFoodItems()
        WriteLog("FOOD", "Food items form opened")
    End Sub

    'loads all food items from tblFoodItem into the grid
    Private Sub LoadFoodItems()
        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT FoodItemID, FoodItemName, FoodItemPrice, FoodItemCategory FROM tblFoodItem"
            Dim da As New OleDbDataAdapter(SQLCmd)
            Dim dt As New DataTable
            da.Fill(dt)
            dgvFoodItems.DataSource = dt
            cn.Close()
        End If

        'let the name column stretch out and wrap so its all readable
        dgvFoodItems.Columns("FoodItemName").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        dgvFoodItems.DefaultCellStyle.WrapMode = DataGridViewTriState.True
        dgvFoodItems.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells

        WriteLog("FOOD", "Food item list loaded")
    End Sub

    'adds a new food item using the values typed into the textboxes
    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        If txtName.Text = "" Then
            MessageBox.Show("Enter a food item name")
            Exit Sub
        End If

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "INSERT INTO tblFoodItem (FoodItemName, FoodItemPrice, FoodItemCategory) VALUES (@FoodItemName, @FoodItemPrice, @FoodItemCategory)"
            SQLCmd.Parameters.AddWithValue("@FoodItemName", txtName.Text)
            SQLCmd.Parameters.AddWithValue("@FoodItemPrice", Val(txtPrice.Text))
            SQLCmd.Parameters.AddWithValue("@FoodItemCategory", txtCategory.Text)
            SQLCmd.ExecuteNonQuery()
            cn.Close()
        End If

        WriteLog("FOOD", "Food item added: " & txtName.Text)
        LoadFoodItems()
        ClearFields()
    End Sub

    'updates the currently selected food item with the values in the textboxes
    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        If selectedFoodItemID = 0 Then
            MessageBox.Show("Select a food item in the grid first")
            Exit Sub
        End If

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "UPDATE tblFoodItem SET FoodItemName = @FoodItemName, FoodItemPrice = @FoodItemPrice, FoodItemCategory = @FoodItemCategory WHERE FoodItemID = @FoodItemID"
            SQLCmd.Parameters.AddWithValue("@FoodItemName", txtName.Text)
            SQLCmd.Parameters.AddWithValue("@FoodItemPrice", Val(txtPrice.Text))
            SQLCmd.Parameters.AddWithValue("@FoodItemCategory", txtCategory.Text)
            SQLCmd.Parameters.AddWithValue("@FoodItemID", selectedFoodItemID)
            SQLCmd.ExecuteNonQuery()
            cn.Close()
        End If

        WriteLog("FOOD", "Food item updated: " & txtName.Text)
        LoadFoodItems()
        ClearFields()
    End Sub

    'deletes the currently selected food item
    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If selectedFoodItemID = 0 Then
            MessageBox.Show("Select a food item in the grid first")
            Exit Sub
        End If

        If MessageBox.Show("Delete this food item?", "Confirm", MessageBoxButtons.YesNo) = DialogResult.No Then
            Exit Sub
        End If

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "DELETE FROM tblFoodItem WHERE FoodItemID = @FoodItemID"
            SQLCmd.Parameters.AddWithValue("@FoodItemID", selectedFoodItemID)
            SQLCmd.ExecuteNonQuery()
            cn.Close()
        End If

        WriteLog("FOOD", "Food item deleted: " & txtName.Text)
        LoadFoodItems()
        ClearFields()
    End Sub

    'clears the textboxes and the selection
    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        ClearFields()
        WriteLog("FOOD", "Food item fields cleared")
    End Sub

    Private Sub ClearFields()
        selectedFoodItemID = 0
        txtName.Text = ""
        txtPrice.Text = ""
        txtCategory.Text = ""
        dgvFoodItems.ClearSelection()
    End Sub

    'when a row is clicked, load its values into the textboxes for editing
    Private Sub dgvFoodItems_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvFoodItems.CellClick
        If e.RowIndex < 0 Then Exit Sub

        Dim row As DataGridViewRow = dgvFoodItems.Rows(e.RowIndex)
        selectedFoodItemID = CLng(row.Cells("FoodItemID").Value)
        txtName.Text = row.Cells("FoodItemName").Value.ToString()
        txtPrice.Text = row.Cells("FoodItemPrice").Value.ToString()
        txtCategory.Text = row.Cells("FoodItemCategory").Value.ToString()
        WriteLog("FOOD", "Food item selected: " & txtName.Text)
    End Sub

End Class
