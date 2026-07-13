Imports System.Data.OleDb

Public Class frmFoodItems
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
    End Sub

    Private Sub frmFoodItems_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        dgvFood.ClearSelection()
    End Sub

    Private Sub dgvFood_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs) Handles dgvFood.DataBindingComplete
        dgvFood.ClearSelection()
    End Sub
End Class
