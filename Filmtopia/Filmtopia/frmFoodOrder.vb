Imports System.Data.OleDb

Public Class frmFoodOrder

    'the booking this food order belongs to, set by frmBookings before showing this form
    Public currentBookingID As Long = 0

    'the order item currently selected in the grid, 0 means nothing selected
    Private selectedOrderItemID As Integer = 0

    Private Sub frmFoodOrder_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CommonFormStartup()
        LoadBookingInfo()
        LoadFoodItemsCombo()
        LoadOrderItems()
        WriteLog("FOODORDER", "Food order form opened for booking " & currentBookingID)
    End Sub

    'shows the customer, film and screening for this booking at the top of the form
    Private Sub LoadBookingInfo()
        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            'join booking to customer (name), then to screening, then to film (title)
            SQLCmd.CommandText = "SELECT CustomerForename & ' ' & CustomerSurname AS CustomerName, FilmTitle, ScreeningDate, ScreeningTime FROM ((tblBooking INNER JOIN tblCustomer ON tblBooking.CustomerID = tblCustomer.CustomerID) INNER JOIN tblScreening ON tblBooking.ScreeningID = tblScreening.ScreeningID) INNER JOIN tblFilm ON tblScreening.FilmID = tblFilm.FilmID WHERE tblBooking.BookingID = @BookingID"
            SQLCmd.Parameters.AddWithValue("@BookingID", CInt(currentBookingID))
            Dim rs As OleDbDataReader = SQLCmd.ExecuteReader()
            If rs.Read() Then
                lblBookingInfo.Text = "Booking #" & currentBookingID & " - " & rs("CustomerName").ToString() & " - " & rs("FilmTitle").ToString() & " (" & rs("ScreeningDate").ToString() & " " & rs("ScreeningTime").ToString() & ")"
            End If
            rs.Close()
            cn.Close()
        End If
    End Sub

    'fills the food item combo with every food item
    Private Sub LoadFoodItemsCombo()
        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT FoodItemID, FoodItemName, FoodItemPrice FROM tblFoodItem"
            Dim da As New OleDbDataAdapter(SQLCmd)
            Dim dt As New DataTable
            da.Fill(dt)
            cboFoodItem.DataSource = dt
            cboFoodItem.DisplayMember = "FoodItemName"
            cboFoodItem.ValueMember = "FoodItemID"
            cboFoodItem.SelectedIndex = -1
            cn.Close()
        End If
    End Sub

    'shows the price of the food item picked in the combo
    Private Sub cboFoodItem_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboFoodItem.SelectedIndexChanged
        If cboFoodItem.SelectedIndex = -1 Then
            lblPrice.Text = ""
            Exit Sub
        End If

        Dim row As DataRowView = CType(cboFoodItem.SelectedItem, DataRowView)
        lblPrice.Text = FormatCurrency(row("FoodItemPrice"))
    End Sub

    'loads the food items already ordered for this booking, with subtotals and a running total
    Private Sub LoadOrderItems()
        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            'join order item to food item so we can show the name and price
            SQLCmd.CommandText = "SELECT tblOrderItem.OrderItemID, FoodItemName, FoodItemPrice, Quantity, FoodItemPrice * Quantity AS Subtotal FROM tblOrderItem INNER JOIN tblFoodItem ON tblOrderItem.FoodItemID = tblFoodItem.FoodItemID WHERE BookingID = @BookingID"
            SQLCmd.Parameters.AddWithValue("@BookingID", CInt(currentBookingID))
            Dim da As New OleDbDataAdapter(SQLCmd)
            Dim dt As New DataTable
            da.Fill(dt)
            dgvOrderItems.DataSource = dt
            cn.Close()
        End If

        'hide the raw id column, its only there for selecting a row to remove
        dgvOrderItems.Columns("OrderItemID").Visible = False

        'work out the running total by adding up the subtotal column
        Dim total As Double = 0
        For Each row As DataGridViewRow In dgvOrderItems.Rows
            total = total + CDbl(row.Cells("Subtotal").Value)
        Next
        lblTotal.Text = "Total: " & FormatCurrency(total)
    End Sub

    'adds the picked food item and quantity to the order
    Private Sub btnAddItem_Click(sender As Object, e As EventArgs) Handles btnAddItem.Click
        If cboFoodItem.SelectedIndex = -1 Then
            MessageBox.Show("Pick a food item first")
            Exit Sub
        End If

        Dim quantity As Integer = CInt(Val(txtQuantity.Text))
        If quantity < 1 Then
            MessageBox.Show("Enter a quantity of 1 or more")
            Exit Sub
        End If

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "INSERT INTO tblOrderItem (BookingID, FoodItemID, Quantity) VALUES (@BookingID, @FoodItemID, @Quantity)"
            SQLCmd.Parameters.AddWithValue("@BookingID", CInt(currentBookingID))
            SQLCmd.Parameters.AddWithValue("@FoodItemID", CInt(cboFoodItem.SelectedValue))
            SQLCmd.Parameters.AddWithValue("@Quantity", quantity)
            SQLCmd.ExecuteNonQuery()
            cn.Close()
        End If

        WriteLog("FOODORDER", "Added " & quantity & " x " & cboFoodItem.Text & " to booking " & currentBookingID)
        LoadOrderItems()
        txtQuantity.Text = "1"
    End Sub

    'remembers which order item row was clicked so it can be removed
    Private Sub dgvOrderItems_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvOrderItems.CellClick
        If e.RowIndex < 0 Then Exit Sub

        Dim row As DataGridViewRow = dgvOrderItems.Rows(e.RowIndex)
        selectedOrderItemID = CInt(row.Cells("OrderItemID").Value)
    End Sub

    'removes the selected item from the order
    Private Sub btnRemoveItem_Click(sender As Object, e As EventArgs) Handles btnRemoveItem.Click
        If selectedOrderItemID = 0 Then
            MessageBox.Show("Select an item in the grid first")
            Exit Sub
        End If

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "DELETE FROM tblOrderItem WHERE OrderItemID = @OrderItemID"
            SQLCmd.Parameters.AddWithValue("@OrderItemID", selectedOrderItemID)
            SQLCmd.ExecuteNonQuery()
            cn.Close()
        End If

        WriteLog("FOODORDER", "Removed order item " & selectedOrderItemID & " from booking " & currentBookingID)
        selectedOrderItemID = 0
        LoadOrderItems()
    End Sub

    'closes the food order form
    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

End Class
