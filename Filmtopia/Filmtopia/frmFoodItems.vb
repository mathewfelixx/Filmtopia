Imports System.Data.OleDb

Public Class frmFoodItems

    Private selectedFoodItemID As Integer = 0
    Private selectedFoodItemIsActive As Boolean = True

    Private Sub frmFoodItems_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CommonFormStartup()
        LoadFoodItems()
        WriteLog("FOOD", "Food items form opened")
    End Sub

    Private Sub LoadFoodItems()
        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT FoodItemID, FoodItemName, FoodItemPrice, FoodItemCategory, IsActive, " &
                                 "IIF(IsActive, 'On sale', 'Withdrawn') AS Status " &
                                 "FROM tblFoodItem WHERE (@ShowAll = True OR IsActive = True)"
            SQLCmd.Parameters.AddWithValue("@ShowAll", chkShowInactive.Checked)
            Dim da As New OleDbDataAdapter(SQLCmd)
            Dim dt As New DataTable
            da.Fill(dt)
            dgvFoodItems.DataSource = dt
            cn.Close()
        End If

        dgvFoodItems.Columns("IsActive").Visible = False
        dgvFoodItems.Columns("FoodItemName").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        dgvFoodItems.DefaultCellStyle.WrapMode = DataGridViewTriState.True
        dgvFoodItems.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells

        WriteLog("FOOD", "Food item list loaded")
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        If txtName.Text = "" Then
            MessageBox.Show("Enter a food item name")
            Exit Sub
        End If
        If txtPrice.Text = "" Then
            MessageBox.Show("Enter a price")
            Exit Sub
        End If
        If Not IsNumeric(txtPrice.Text) Then
            MessageBox.Show("Price must be a number")
            Exit Sub
        End If
        If Val(txtPrice.Text) <= 0 Then
            MessageBox.Show("Price must be greater than 0")
            Exit Sub
        End If
        If txtCategory.Text = "" Then
            MessageBox.Show("Enter a category")
            Exit Sub
        End If

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "INSERT INTO tblFoodItem (FoodItemName, FoodItemPrice, FoodItemCategory, IsActive) " &
                                 "VALUES (@FoodItemName, @FoodItemPrice, @FoodItemCategory, @IsActive)"
            SQLCmd.Parameters.AddWithValue("@FoodItemName", txtName.Text)
            SQLCmd.Parameters.AddWithValue("@FoodItemPrice", Val(txtPrice.Text))
            SQLCmd.Parameters.AddWithValue("@FoodItemCategory", txtCategory.Text)
            SQLCmd.Parameters.AddWithValue("@IsActive", True)
            SQLCmd.ExecuteNonQuery()
            cn.Close()
        End If

        WriteLog("FOOD", "Food item added: " & txtName.Text)
        LoadFoodItems()
        ClearFields()
    End Sub

    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        If selectedFoodItemID = 0 Then
            MessageBox.Show("Select a food item in the grid first")
            Exit Sub
        End If
        If txtName.Text = "" Then
            MessageBox.Show("Enter a food item name")
            Exit Sub
        End If
        If txtPrice.Text = "" Then
            MessageBox.Show("Enter a price")
            Exit Sub
        End If
        If Not IsNumeric(txtPrice.Text) Then
            MessageBox.Show("Price must be a number")
            Exit Sub
        End If
        If Val(txtPrice.Text) <= 0 Then
            MessageBox.Show("Price must be greater than 0")
            Exit Sub
        End If
        If txtCategory.Text = "" Then
            MessageBox.Show("Enter a category")
            Exit Sub
        End If

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "UPDATE tblFoodItem " &
                                 "SET FoodItemName = @FoodItemName, FoodItemPrice = @FoodItemPrice, FoodItemCategory = @FoodItemCategory " &
                                 "WHERE FoodItemID = @FoodItemID"
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

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If selectedFoodItemID = 0 Then
            MessageBox.Show("Select a food item in the grid first")
            Exit Sub
        End If

        Dim withdrawing As Boolean = selectedFoodItemIsActive
        Dim confirmMsg As String
        If withdrawing Then
            confirmMsg = "Withdraw this item from sale? Its record and past orders are kept, but it will not be offered on new orders."
        Else
            confirmMsg = "Return this item to sale?"
        End If

        If MessageBox.Show(confirmMsg, "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = DialogResult.No Then
            Exit Sub
        End If

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "UPDATE tblFoodItem SET IsActive = @IsActive WHERE FoodItemID = @FoodItemID"
            SQLCmd.Parameters.AddWithValue("@IsActive", Not withdrawing)
            SQLCmd.Parameters.AddWithValue("@FoodItemID", selectedFoodItemID)
            SQLCmd.ExecuteNonQuery()
            cn.Close()
        End If

        If withdrawing Then
            WriteLog("FOOD", "Food item withdrawn from sale: " & txtName.Text)
        Else
            WriteLog("FOOD", "Food item returned to sale: " & txtName.Text)
        End If

        LoadFoodItems()
        ClearFields()
    End Sub

    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        ClearFields()
        WriteLog("FOOD", "Food item fields cleared")
    End Sub

    Private Sub ClearFields()
        selectedFoodItemID = 0
        selectedFoodItemIsActive = True
        txtName.Text = ""
        txtPrice.Text = ""
        txtCategory.Text = ""
        btnDelete.Text = "Withdraw from sale"
        dgvFoodItems.ClearSelection()
    End Sub

    Private Sub chkShowInactive_CheckedChanged(sender As Object, e As EventArgs) Handles chkShowInactive.CheckedChanged
        LoadFoodItems()
        ClearFields()
    End Sub

    Private Sub dgvFoodItems_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvFoodItems.CellClick
        If e.RowIndex < 0 Then Exit Sub

        Dim row As DataGridViewRow = dgvFoodItems.Rows(e.RowIndex)
        selectedFoodItemID = CInt(row.Cells("FoodItemID").Value)
        txtName.Text = row.Cells("FoodItemName").Value.ToString()
        txtPrice.Text = row.Cells("FoodItemPrice").Value.ToString()
        txtCategory.Text = row.Cells("FoodItemCategory").Value.ToString()
        selectedFoodItemIsActive = CBool(row.Cells("IsActive").Value)

        If selectedFoodItemIsActive Then
            btnDelete.Text = "Withdraw from sale"
        Else
            btnDelete.Text = "Return to sale"
        End If

        WriteLog("FOOD", "Food item selected: " & txtName.Text)
    End Sub

End Class
