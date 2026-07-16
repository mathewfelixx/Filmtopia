Imports System.Data.OleDb

Public Class frmFoodItems
    Private selectedFoodItemID As Long = 0
    Private selectedFoodName As String = ""
    Private selectedWithdrawn As Boolean = False

    Private Sub frmFoodItems_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadFood()
    End Sub

    Private Sub LoadFood()
        Dim SQLCmd As New OleDbCommand("SELECT FoodItemID, FoodCategory, FoodName, FoodPrice, FoodWithdrawn " &
                                       "FROM tblFoodItem ORDER BY FoodCategory, FoodName")
        Dim dt As New DataTable
        If FillTable(SQLCmd, dt) Then
            dgvFood.DataSource = dt
            dgvFood.Columns("FoodItemID").Visible = False
            dgvFood.Columns("FoodCategory").HeaderText = "Category"
            dgvFood.Columns("FoodName").HeaderText = "Item"
            dgvFood.Columns("FoodPrice").HeaderText = "Price"
            dgvFood.Columns("FoodWithdrawn").HeaderText = "Withdrawn"
            dgvFood.Columns("FoodName").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            dgvFood.Columns("FoodPrice").DefaultCellStyle.Format = "C2"
        End If
        selectedFoodItemID = 0
    End Sub

    Private Sub frmFoodItems_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        dgvFood.ClearSelection()
    End Sub

    Private Sub dgvFood_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs) Handles dgvFood.DataBindingComplete
        dgvFood.ClearSelection()
    End Sub

    Private Sub dgvFood_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvFood.CellClick
        If e.RowIndex >= 0 Then
            Dim row As DataGridViewRow = dgvFood.Rows(e.RowIndex)
            selectedFoodItemID = row.Cells("FoodItemID").Value
            selectedFoodName = row.Cells("FoodName").Value
            selectedWithdrawn = row.Cells("FoodWithdrawn").Value
        End If
    End Sub

    Private Sub btnWithdraw_Click(sender As Object, e As EventArgs) Handles btnWithdraw.Click
        If selectedFoodItemID = 0 Then
            MessageBox.Show("Please pick an item from the list first.", "No Item Picked", MessageBoxButtons.OK, MessageBoxIcon.Information)
        ElseIf selectedWithdrawn Then
            MessageBox.Show(selectedFoodName & " has already been withdrawn from sale.", "Already Withdrawn", MessageBoxButtons.OK, MessageBoxIcon.Information)
        ElseIf SetWithdrawn(True) Then
            WriteLog(LogFood, "Withdrew " & selectedFoodName & " from sale")
            LoadFood()
        End If
    End Sub

    Private Sub btnPutBack_Click(sender As Object, e As EventArgs) Handles btnPutBack.Click
        If selectedFoodItemID = 0 Then
            MessageBox.Show("Please pick an item from the list first.", "No Item Picked", MessageBoxButtons.OK, MessageBoxIcon.Information)
        ElseIf Not selectedWithdrawn Then
            MessageBox.Show(selectedFoodName & " is already on sale.", "Already on Sale", MessageBoxButtons.OK, MessageBoxIcon.Information)
        ElseIf SetWithdrawn(False) Then
            WriteLog(LogFood, "Put " & selectedFoodName & " back on sale")
            LoadFood()
        End If
    End Sub

    Private Function SetWithdrawn(withdrawn As Boolean) As Boolean
        Dim SQLCmd As New OleDbCommand("UPDATE tblFoodItem SET FoodWithdrawn = @FoodWithdrawn WHERE FoodItemID = @FoodItemID")
        SQLCmd.Parameters.AddWithValue("@FoodWithdrawn", withdrawn)
        SQLCmd.Parameters.AddWithValue("@FoodItemID", selectedFoodItemID)
        Return RunCommand(SQLCmd)
    End Function
End Class
