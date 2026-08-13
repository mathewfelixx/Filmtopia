Imports System.Data.OleDb

Public Class frmFoodOrder

    'the booking this food order belongs to, set by frmBookings before showing this form
    Public currentBookingID As Long = 0

    'the order item currently selected in the grid, 0 means nothing selected
    Private selectedOrderItemID As Integer = 0

    Private Sub frmFoodOrder_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CommonFormStartup(Me)
        LoadBookingInfo()
        LoadFoodItemsCombo()
        LoadOrderItems()

        'a cancelled sale has been refunded, so nothing more can be put on it. the till screen
        'already leaves cancelled bookings out of its list, but this form can be reached with any
        'booking, and adding food to one would sit in the database without ever being charged for
        If BookingIsCancelled() Then
            lblBookingInfo.Text = lblBookingInfo.Text & "  -  CANCELLED, nothing can be added"
            btnAddItem.Enabled = False
            btnRemoveItem.Enabled = False
        End If

        WriteLog("FOODORDER", "Food order form opened for booking " & currentBookingID)
    End Sub

    'says whether this booking has been cancelled
    Private Function BookingIsCancelled() As Boolean
        Dim cancelled As Boolean = False

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT COUNT(*) FROM tblBooking " &
                                 "WHERE BookingID = @BookingID AND BookingStatus = @Cancelled"
            SQLCmd.Parameters.AddWithValue("@BookingID", CInt(currentBookingID))
            SQLCmd.Parameters.AddWithValue("@Cancelled", BookingCancelled)
            cancelled = CInt(SQLCmd.ExecuteScalar()) > 0
            cn.Close()
        End If

        Return cancelled
    End Function

    'shows the customer, film and screening for this booking at the top of the form
    Private Sub LoadBookingInfo()
        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            'join booking to customer (name), then to screening, then to film (title)
            SQLCmd.CommandText = "SELECT CustomerForename & ' ' & CustomerSurname AS CustomerName, FilmTitle, ScreeningDate, ScreeningTime " &
                                 "FROM ((tblBooking INNER JOIN tblCustomer ON tblBooking.CustomerID = tblCustomer.CustomerID) " &
                                 "INNER JOIN tblScreening ON tblBooking.ScreeningID = tblScreening.ScreeningID) " &
                                 "INNER JOIN tblFilm ON tblScreening.FilmID = tblFilm.FilmID " &
                                 "WHERE tblBooking.BookingID = @BookingID"
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
            'an item taken off the menu must not be sellable, but it is only hidden from what is
            'being offered now. the lines already on an order still read back fine, they join to
            'this table for the name and the row is still there
            SQLCmd.CommandText = "SELECT FoodItemID, FoodItemName, FoodItemPrice " &
                                 "FROM tblFoodItem " &
                                 "WHERE (FoodItemStatus IS NULL OR FoodItemStatus <> @Withdrawn) " &
                                 "ORDER BY FoodItemCategory, FoodItemName"
            SQLCmd.Parameters.AddWithValue("@Withdrawn", FoodWithdrawn)
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
            'the price and the subtotal come off the order line, because that is what this order was
            'actually charged. the join is only still here to fetch the name to show, since the line
            'itself only keeps the id
            SQLCmd.CommandText = "SELECT tblOrderItem.OrderItemID, FoodItemName, ItemPricePaid, Quantity, ItemPricePaid * Quantity AS Subtotal " &
                                 "FROM tblOrderItem INNER JOIN tblFoodItem ON tblOrderItem.FoodItemID = tblFoodItem.FoodItemID " &
                                 "WHERE BookingID = @BookingID"
            SQLCmd.Parameters.AddWithValue("@BookingID", CInt(currentBookingID))
            Dim da As New OleDbDataAdapter(SQLCmd)
            Dim dt As New DataTable
            da.Fill(dt)
            dgvOrderItems.DataSource = dt
            cn.Close()
        End If

        'hide the raw id column, its only there for selecting a row to remove
        dgvOrderItems.Columns("OrderItemID").Visible = False

        'the two money columns were coming out as plain numbers, so 4.5 next to a total that said
        'four pounds fifty. same currency format the rest of the grids use
        dgvOrderItems.Columns("FoodItemName").HeaderText = "Item"
        dgvOrderItems.Columns("ItemPricePaid").HeaderText = "Price"
        dgvOrderItems.Columns("Quantity").HeaderText = "Qty"
        dgvOrderItems.Columns("Subtotal").HeaderText = "Subtotal"

        dgvOrderItems.Columns("FoodItemName").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        dgvOrderItems.Columns("ItemPricePaid").Width = 90
        dgvOrderItems.Columns("Quantity").Width = 60
        dgvOrderItems.Columns("Subtotal").Width = 100

        dgvOrderItems.Columns("ItemPricePaid").DefaultCellStyle.Format = "C"
        dgvOrderItems.Columns("Subtotal").DefaultCellStyle.Format = "C"
        dgvOrderItems.Columns("Quantity").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

        'work out the running total by adding up the subtotal column
        Dim total As Double = 0
        For Each row As DataGridViewRow In dgvOrderItems.Rows
            total = total + CDbl(row.Cells("Subtotal").Value)
        Next
        lblTotal.Text = "Total: " & FormatCurrency(total)
    End Sub

    'adds the picked food item and quantity to the order
    Private Sub btnAddItem_Click(sender As Object, e As EventArgs) Handles btnAddItem.Click
        'checked again here rather than trusting the button being switched off on load
        If BookingIsCancelled() Then
            MessageBox.Show("Booking " & currentBookingID & " has been cancelled and refunded, so nothing can be added to it.",
                            "Cancelled booking", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If cboFoodItem.SelectedIndex = -1 Then
            MessageBox.Show("Pick a food item first", "Food Order", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Dim quantity As Integer = SafeInt(txtQuantity.Text)
        If quantity < 1 Then
            MessageBox.Show("Enter a quantity of 1 or more", "Food Order", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        'what it is being sold for right now. the price goes onto the line and is never worked out
        'again, so a line added today keeps today's price even if the menu changes tomorrow
        Dim chosen As DataRowView = CType(cboFoodItem.SelectedItem, DataRowView)
        Dim pricePaid As Double = CDbl(chosen("FoodItemPrice"))

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "INSERT INTO tblOrderItem (BookingID, FoodItemID, Quantity, ItemPricePaid) " &
                                 "VALUES (@BookingID, @FoodItemID, @Quantity, @ItemPricePaid)"
            SQLCmd.Parameters.AddWithValue("@BookingID", CInt(currentBookingID))
            SQLCmd.Parameters.AddWithValue("@FoodItemID", CInt(cboFoodItem.SelectedValue))
            SQLCmd.Parameters.AddWithValue("@Quantity", quantity)
            SQLCmd.Parameters.AddWithValue("@ItemPricePaid", pricePaid)
            SQLCmd.ExecuteNonQuery()
            cn.Close()
        End If

        'modBookings owns the money, this form just tells it something changed
        RecalculateBookingTotal(currentBookingID)
        WriteLog("FOODORDER", "Added " & quantity & " x " & cboFoodItem.Text & " to booking " & currentBookingID, LogChange)
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
        'the food on a cancelled sale is kept on purpose so the report can show what was refunded,
        'so it must not be taken off either
        If BookingIsCancelled() Then
            MessageBox.Show("Booking " & currentBookingID & " has been cancelled, so its order cannot be changed.",
                            "Cancelled booking", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If selectedOrderItemID = 0 Then
            MessageBox.Show("Select an item in the grid first", "Food Order", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "DELETE FROM tblOrderItem " &
                                 "WHERE OrderItemID = @OrderItemID"
            SQLCmd.Parameters.AddWithValue("@OrderItemID", selectedOrderItemID)
            SQLCmd.ExecuteNonQuery()
            cn.Close()
        End If

        'modBookings owns the money, this form just tells it something changed
        RecalculateBookingTotal(currentBookingID)
        WriteLog("FOODORDER", "Removed order item " & selectedOrderItemID & " from booking " & currentBookingID, LogChange)
        selectedOrderItemID = 0
        LoadOrderItems()
    End Sub

    'closes the food order form
    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

End Class
