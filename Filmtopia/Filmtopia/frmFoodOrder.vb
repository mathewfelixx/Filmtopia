Imports System.Data.OleDb

Public Class frmFoodOrder

    Public currentBookingID As Long = 0

    Private selectedOrderItemID As Integer = 0

    Private Sub frmFoodOrder_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CommonFormStartup(Me)
        LoadBookingInfo()
        LoadFoodItemsCombo()
        LoadOrderItems()

        If BookingIsCancelled() Then
            lblBookingInfo.Text = lblBookingInfo.Text & "  -  CANCELLED, nothing can be added"
            btnAddItem.Enabled = False
            btnRemoveItem.Enabled = False
        End If

        WriteLog("FOODORDER", "Food order form opened for booking " & currentBookingID)
    End Sub

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

    Private Sub LoadBookingInfo()
        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
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

    Private Sub LoadFoodItemsCombo()
        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT FoodItemID, FoodItemName, FoodItemPrice, FoodItemCategory, FoodItemImage " &
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

    Private Sub cboFoodItem_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboFoodItem.SelectedIndexChanged
        If cboFoodItem.SelectedIndex = -1 Then
            lblPrice.Text = ""
            lblItemCategory.Text = ""
            ShowItemPicture("")
            Exit Sub
        End If

        Dim row As DataRowView = CType(cboFoodItem.SelectedItem, DataRowView)
        lblPrice.Text = FormatCurrency(row("FoodItemPrice"))
        lblItemCategory.Text = row("FoodItemCategory").ToString()
        ShowItemPicture(row("FoodItemImage").ToString())
    End Sub

    Private Sub ShowItemPicture(fileName As String)
        If picFoodItem.Image IsNot Nothing Then
            picFoodItem.Image.Dispose()
            picFoodItem.Image = Nothing
        End If

        picFoodItem.Image = FoodImage(fileName)

        lblNoPicture.Visible = (picFoodItem.Image Is Nothing)
    End Sub

    Private Sub frmFoodOrder_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If picFoodItem.Image IsNot Nothing Then
            picFoodItem.Image.Dispose()
            picFoodItem.Image = Nothing
        End If
    End Sub

    Private Sub LoadOrderItems()
        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
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

        dgvOrderItems.Columns("OrderItemID").Visible = False

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

        Dim total As Double = 0
        Dim things As Integer = 0
        For Each row As DataGridViewRow In dgvOrderItems.Rows
            total = total + CDbl(row.Cells("Subtotal").Value)
            things = things + CInt(row.Cells("Quantity").Value)
        Next
        lblTotal.Text = "Total: " & FormatCurrency(total)

        If dgvOrderItems.Rows.Count = 0 Then
            lblGridCount.Text = "Nothing ordered yet"
        Else
            lblGridCount.Text = dgvOrderItems.Rows.Count & " line(s), " & things & " item(s) to hand over"
        End If
    End Sub

    Private Sub btnAddItem_Click(sender As Object, e As EventArgs) Handles btnAddItem.Click
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

        RecalculateBookingTotal(currentBookingID)
        WriteLog("FOODORDER", "Added " & quantity & " x " & cboFoodItem.Text & " to booking " & currentBookingID, LogChange)
        LoadOrderItems()
        txtQuantity.Text = "1"
    End Sub

    Private Sub dgvOrderItems_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvOrderItems.CellClick
        If e.RowIndex < 0 Then Exit Sub

        Dim row As DataGridViewRow = dgvOrderItems.Rows(e.RowIndex)
        selectedOrderItemID = CInt(row.Cells("OrderItemID").Value)
    End Sub

    Private Sub btnRemoveItem_Click(sender As Object, e As EventArgs) Handles btnRemoveItem.Click
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

        RecalculateBookingTotal(currentBookingID)
        WriteLog("FOODORDER", "Removed order item " & selectedOrderItemID & " from booking " & currentBookingID, LogChange)
        selectedOrderItemID = 0
        LoadOrderItems()
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

End Class
