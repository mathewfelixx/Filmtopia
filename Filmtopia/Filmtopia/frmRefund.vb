Imports System.Data.OleDb

Public Class frmRefund

    Public currentBookingID As Integer = 0

    Private Sub frmRefund_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CommonFormStartup()
        LoadBookingInfo()
        LoadSeats()
        LoadFood()
        WriteLog("REFUND", "Refund form opened for booking " & currentBookingID)
    End Sub

    Private Sub LoadBookingInfo()
        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT CustomerForename & ' ' & CustomerSurname AS CustomerName, FilmTitle, ScreeningDate, ScreeningTime " &
                                 "FROM ((tblBooking INNER JOIN tblCustomer ON tblBooking.CustomerID = tblCustomer.CustomerID) " &
                                 "INNER JOIN tblScreening ON tblBooking.ScreeningID = tblScreening.ScreeningID) " &
                                 "INNER JOIN tblFilm ON tblScreening.FilmID = tblFilm.FilmID " &
                                 "WHERE tblBooking.BookingID = @BookingID"
            SQLCmd.Parameters.AddWithValue("@BookingID", currentBookingID)
            Dim rs As OleDbDataReader = SQLCmd.ExecuteReader()
            If rs.Read() Then
                lblBookingInfo.Text = "Booking #" & currentBookingID & " - " & rs("CustomerName").ToString() & " - " & rs("FilmTitle").ToString() & " (" & rs("ScreeningDate").ToString() & " " & rs("ScreeningTime").ToString() & ")"
            End If
            rs.Close()
            cn.Close()
        End If
    End Sub

    Private Sub LoadSeats()
        Dim dt As New DataTable
        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT tblBookingSeat.SeatID, tblSeat.SeatRow & tblSeat.SeatNumber AS Seat, tblBookingSeat.SeatPricePaid AS Price " &
                                 "FROM tblBookingSeat INNER JOIN tblSeat ON tblBookingSeat.SeatID = tblSeat.SeatID " &
                                 "WHERE tblBookingSeat.BookingID = @BookingID " &
                                 "ORDER BY tblSeat.SeatRow, tblSeat.SeatNumber"
            SQLCmd.Parameters.AddWithValue("@BookingID", currentBookingID)
            Dim da As New OleDbDataAdapter(SQLCmd)
            da.Fill(dt)
            cn.Close()
        End If

        dgvRefundSeats.DataSource = Nothing
        dgvRefundSeats.Columns.Clear()
        dgvRefundSeats.DataSource = dt

        Dim colRefund As New DataGridViewCheckBoxColumn()
        colRefund.Name = "colSeatRefund"
        colRefund.HeaderText = "Refund"
        colRefund.FalseValue = False
        colRefund.TrueValue = True
        dgvRefundSeats.Columns.Add(colRefund)

        If dgvRefundSeats.Columns.Contains("SeatID") Then
            dgvRefundSeats.Columns("SeatID").Visible = False
        End If
        If dgvRefundSeats.Columns.Contains("Seat") Then
            dgvRefundSeats.Columns("Seat").ReadOnly = True
        End If
        If dgvRefundSeats.Columns.Contains("Price") Then
            dgvRefundSeats.Columns("Price").ReadOnly = True
            dgvRefundSeats.Columns("Price").DefaultCellStyle.Format = "C"
        End If
        dgvRefundSeats.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
    End Sub

    Private Sub LoadFood()
        Dim dt As New DataTable
        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT tblOrderItem.OrderItemID, FoodItemName AS Item, FoodItemPrice AS UnitPrice, Quantity AS Ordered " &
                                 "FROM tblOrderItem INNER JOIN tblFoodItem ON tblOrderItem.FoodItemID = tblFoodItem.FoodItemID " &
                                 "WHERE tblOrderItem.BookingID = @BookingID"
            SQLCmd.Parameters.AddWithValue("@BookingID", currentBookingID)
            Dim da As New OleDbDataAdapter(SQLCmd)
            da.Fill(dt)
            cn.Close()
        End If

        dgvRefundFood.DataSource = Nothing
        dgvRefundFood.Columns.Clear()
        dgvRefundFood.DataSource = dt

        Dim colQty As New DataGridViewTextBoxColumn()
        colQty.Name = "colRefundQty"
        colQty.HeaderText = "Refund qty"
        dgvRefundFood.Columns.Add(colQty)

        If dgvRefundFood.Columns.Contains("OrderItemID") Then
            dgvRefundFood.Columns("OrderItemID").Visible = False
        End If
        If dgvRefundFood.Columns.Contains("UnitPrice") Then
            dgvRefundFood.Columns("UnitPrice").DefaultCellStyle.Format = "C"
        End If
        For Each col As DataGridViewColumn In dgvRefundFood.Columns
            If col.Name <> "colRefundQty" Then
                col.ReadOnly = True
            End If
        Next
        dgvRefundFood.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub btnConfirmRefund_Click(sender As Object, e As EventArgs) Handles btnConfirmRefund.Click
        dgvRefundSeats.EndEdit()
        dgvRefundFood.EndEdit()

        If txtReason.Text.Trim() = "" Then
            MessageBox.Show("Enter a reason for the refund")
            Exit Sub
        End If

        Dim total As Double = 0
        Dim itemCount As Integer = 0

        For Each row As DataGridViewRow In dgvRefundSeats.Rows
            Dim v As Object = row.Cells("colSeatRefund").Value
            If v IsNot Nothing AndAlso CBool(v) Then
                total = total + CDbl(row.Cells("Price").Value)
                itemCount = itemCount + 1
            End If
        Next

        For Each row As DataGridViewRow In dgvRefundFood.Rows
            Dim qty As Integer = RefundQtyOnRow(row)
            If qty > 0 Then
                Dim ordered As Integer = CInt(row.Cells("Ordered").Value)
                If qty > ordered Then
                    MessageBox.Show("You cannot refund more " & row.Cells("Item").Value.ToString() & " than were ordered")
                    Exit Sub
                End If
                total = total + CDbl(row.Cells("UnitPrice").Value) * qty
                itemCount = itemCount + 1
            End If
        Next

        If itemCount = 0 Then
            MessageBox.Show("Tick a seat or enter a food quantity to refund")
            Exit Sub
        End If

        If MessageBox.Show("Refund " & FormatCurrency(total) & " across " & itemCount & " item(s)?", "Confirm refund", MessageBoxButtons.YesNo) = DialogResult.No Then
            Exit Sub
        End If

        Dim refundID As Integer = 0

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "INSERT INTO tblRefund (BookingID, RefundDate, TotalRefunded, Reason, AuthorisedBy) " &
                                 "VALUES (@BookingID, @RefundDate, @TotalRefunded, @Reason, @AuthorisedBy)"
            SQLCmd.Parameters.AddWithValue("@BookingID", currentBookingID)
            SQLCmd.Parameters.AddWithValue("@RefundDate", Date.Now.Date)
            SQLCmd.Parameters.AddWithValue("@TotalRefunded", total)
            SQLCmd.Parameters.AddWithValue("@Reason", txtReason.Text.Trim())
            SQLCmd.Parameters.AddWithValue("@AuthorisedBy", CurrentLoginID)
            SQLCmd.ExecuteNonQuery()

            SQLCmd.CommandText = "SELECT @@IDENTITY"
            SQLCmd.Parameters.Clear()
            refundID = CInt(SQLCmd.ExecuteScalar())

            For Each row As DataGridViewRow In dgvRefundSeats.Rows
                Dim v As Object = row.Cells("colSeatRefund").Value
                If v IsNot Nothing AndAlso CBool(v) Then
                    Dim seatID As Integer = CInt(row.Cells("SeatID").Value)
                    Dim price As Double = CDbl(row.Cells("Price").Value)
                    Dim seatText As String = row.Cells("Seat").Value.ToString()

                    SQLCmd.CommandText = "DELETE FROM tblBookingSeat WHERE BookingID = @BookingID AND SeatID = @SeatID"
                    SQLCmd.Parameters.Clear()
                    SQLCmd.Parameters.AddWithValue("@BookingID", currentBookingID)
                    SQLCmd.Parameters.AddWithValue("@SeatID", seatID)
                    SQLCmd.ExecuteNonQuery()

                    SQLCmd.CommandText = "INSERT INTO tblRefundLine (RefundID, ItemDescription, Amount) VALUES (@RefundID, @Description, @Amount)"
                    SQLCmd.Parameters.Clear()
                    SQLCmd.Parameters.AddWithValue("@RefundID", refundID)
                    SQLCmd.Parameters.AddWithValue("@Description", "Seat " & seatText)
                    SQLCmd.Parameters.AddWithValue("@Amount", price)
                    SQLCmd.ExecuteNonQuery()
                End If
            Next

            For Each row As DataGridViewRow In dgvRefundFood.Rows
                Dim qty As Integer = RefundQtyOnRow(row)
                If qty > 0 Then
                    Dim orderItemID As Integer = CInt(row.Cells("OrderItemID").Value)
                    Dim ordered As Integer = CInt(row.Cells("Ordered").Value)
                    Dim unit As Double = CDbl(row.Cells("UnitPrice").Value)
                    Dim itemName As String = row.Cells("Item").Value.ToString()

                    If qty >= ordered Then
                        SQLCmd.CommandText = "DELETE FROM tblOrderItem WHERE OrderItemID = @OrderItemID"
                        SQLCmd.Parameters.Clear()
                        SQLCmd.Parameters.AddWithValue("@OrderItemID", orderItemID)
                        SQLCmd.ExecuteNonQuery()
                    Else
                        SQLCmd.CommandText = "UPDATE tblOrderItem SET Quantity = Quantity - @Qty WHERE OrderItemID = @OrderItemID"
                        SQLCmd.Parameters.Clear()
                        SQLCmd.Parameters.AddWithValue("@Qty", qty)
                        SQLCmd.Parameters.AddWithValue("@OrderItemID", orderItemID)
                        SQLCmd.ExecuteNonQuery()
                    End If

                    SQLCmd.CommandText = "INSERT INTO tblRefundLine (RefundID, ItemDescription, Amount) VALUES (@RefundID, @Description, @Amount)"
                    SQLCmd.Parameters.Clear()
                    SQLCmd.Parameters.AddWithValue("@RefundID", refundID)
                    SQLCmd.Parameters.AddWithValue("@Description", itemName & " x" & qty)
                    SQLCmd.Parameters.AddWithValue("@Amount", unit * qty)
                    SQLCmd.ExecuteNonQuery()
                End If
            Next

            cn.Close()
        End If

        RecalculateBookingTotal()
        Dim wasCancelled As Boolean = AutoCancelIfEmpty()

        If wasCancelled Then
            WriteLog("REFUND", "Refund " & refundID & " of " & FormatCurrency(total) & " left nothing, booking " & currentBookingID & " cancelled")
            MessageBox.Show("Refunded " & FormatCurrency(total) & ". Nothing was left, so the booking was cancelled.")
        Else
            WriteLog("REFUND", "Refund " & refundID & " of " & FormatCurrency(total) & " on booking " & currentBookingID)
            MessageBox.Show("Refunded " & FormatCurrency(total))
        End If

        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Function RefundQtyOnRow(row As DataGridViewRow) As Integer
        Dim cell As Object = row.Cells("colRefundQty").Value
        If cell Is Nothing Then
            Return 0
        End If
        Return CInt(Val(cell.ToString()))
    End Function

    Private Sub RecalculateBookingTotal()
        Dim newTotal As Double = 0
        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT SUM(SeatPricePaid) FROM tblBookingSeat WHERE BookingID = @BookingID"
            SQLCmd.Parameters.AddWithValue("@BookingID", currentBookingID)
            Dim result As Object = SQLCmd.ExecuteScalar()
            If result IsNot Nothing AndAlso Not IsDBNull(result) Then
                newTotal = CDbl(result)
            End If

            SQLCmd.CommandText = "UPDATE tblBooking SET TotalCost = @TotalCost WHERE BookingID = @BookingID"
            SQLCmd.Parameters.Clear()
            SQLCmd.Parameters.AddWithValue("@TotalCost", newTotal)
            SQLCmd.Parameters.AddWithValue("@BookingID", currentBookingID)
            SQLCmd.ExecuteNonQuery()
            cn.Close()
        End If
    End Sub

    Private Function AutoCancelIfEmpty() As Boolean
        Dim seatsLeft As Integer = 0
        Dim foodLeft As Integer = 0
        Dim cancelled As Boolean = False

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT COUNT(*) FROM tblBookingSeat WHERE BookingID = @BookingID"
            SQLCmd.Parameters.AddWithValue("@BookingID", currentBookingID)
            seatsLeft = CInt(SQLCmd.ExecuteScalar())

            SQLCmd.CommandText = "SELECT COUNT(*) FROM tblOrderItem WHERE BookingID = @BookingID"
            SQLCmd.Parameters.Clear()
            SQLCmd.Parameters.AddWithValue("@BookingID", currentBookingID)
            foodLeft = CInt(SQLCmd.ExecuteScalar())

            If seatsLeft = 0 And foodLeft = 0 Then
                SQLCmd.CommandText = "DELETE FROM tblBooking WHERE BookingID = @BookingID"
                SQLCmd.Parameters.Clear()
                SQLCmd.Parameters.AddWithValue("@BookingID", currentBookingID)
                SQLCmd.ExecuteNonQuery()
                cancelled = True
            End If
            cn.Close()
        End If

        Return cancelled
    End Function

End Class
