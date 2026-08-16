Imports System.Data.OleDb

Public Class frmRefund

    Public currentBookingID As Long = 0

    Private bookingIsCancelled As Boolean = False
    Private canRefund As Boolean = False
    Private fillingGrids As Boolean = False

    Private Sub frmRefund_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CommonFormStartup(Me)
    End Sub

    Private Sub frmRefund_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        RefreshBooking()
        WriteLog("REFUND", "Booking " & currentBookingID & " opened for refund")
    End Sub

    Private Sub RefreshBooking()
        Me.Text = "Booking " & currentBookingID & " - detail and refund"

        dgvSeats.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False
        dgvFood.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False

        txtReason.Text = ""

        LoadBookingHeader()
        LoadSeats()
        LoadFood()
        ApplyAccessLevel()
        UpdateRefundTotal()
    End Sub

    Private Sub LoadBookingHeader()
        Dim soldOn As String = ""
        Dim soldBy As String = "unknown"
        Dim status As String = ""

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT CustomerForename & ' ' & CustomerSurname AS CustomerName, FilmTitle, " &
                                 "ScreeningDate, ScreeningTime, BookingDate, BookingStatus, Username " &
                                 "FROM (((tblBooking LEFT JOIN tblCustomer ON tblBooking.CustomerID = tblCustomer.CustomerID) " &
                                 "LEFT JOIN tblScreening ON tblBooking.ScreeningID = tblScreening.ScreeningID) " &
                                 "LEFT JOIN tblFilm ON tblScreening.FilmID = tblFilm.FilmID) " &
                                 "LEFT JOIN tblLogin ON tblBooking.LoginID = tblLogin.LoginID " &
                                 "WHERE tblBooking.BookingID = @BookingID"
            SQLCmd.Parameters.AddWithValue("@BookingID", CInt(currentBookingID))
            Dim rs As OleDbDataReader = SQLCmd.ExecuteReader()

            If rs.Read() Then
                If IsDBNull(rs("ScreeningDate")) Then
                    lblBookingInfo.Text = "Booking " & currentBookingID & "  -  " & rs("CustomerName").ToString() & "  -  Counter sale"
                Else
                    lblBookingInfo.Text = "Booking " & currentBookingID & "  -  " & rs("CustomerName").ToString() & "  -  " &
                                          rs("FilmTitle").ToString() & "  -  " &
                                          Format(CDate(rs("ScreeningDate")), "dd/MM/yyyy") & " " & rs("ScreeningTime").ToString()
                End If

                If Not IsDBNull(rs("BookingDate")) Then
                    soldOn = Format(CDate(rs("BookingDate")), "dd/MM/yyyy")
                End If

                If Not IsDBNull(rs("Username")) Then
                    soldBy = rs("Username").ToString()
                End If

                status = rs("BookingStatus").ToString()
            End If

            rs.Close()
            cn.Close()
        End If

        lblSoldBy.Text = "Sold on " & soldOn & " by " & soldBy
        bookingIsCancelled = (status = BookingCancelled)

        Dim paidBack As Double = BookingRefundTotal(currentBookingID)

        If bookingIsCancelled Then
            lblStatus.Text = "Status: cancelled - everything on this sale has been refunded"
            lblStatus.ForeColor = PastFore
        Else
            lblStatus.Text = "Status: active"
        End If

        If paidBack > 0 Then
            lblAlreadyRefunded.Text = "Already refunded: " & FormatCurrency(paidBack)
        Else
            lblAlreadyRefunded.Text = ""
        End If
    End Sub

    Private Sub LoadSeats()
        Dim dt As New DataTable
        dt.Columns.Add("Refund", GetType(Boolean))
        dt.Columns.Add("SeatID", GetType(Long))
        dt.Columns.Add("SeatName", GetType(String))
        dt.Columns.Add("SeatTypeName", GetType(String))
        dt.Columns.Add("PricePaid", GetType(Double))
        dt.Columns.Add("Status", GetType(String))

        Dim stillSold As Integer = 0

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT tblSeat.SeatID, tblSeat.SeatRow & tblSeat.SeatNumber AS SeatName, " &
                                 "tblSeatType.SeatTypeName, tblBookingSeat.SeatPricePaid " &
                                 "FROM (tblBookingSeat INNER JOIN tblSeat ON tblBookingSeat.SeatID = tblSeat.SeatID) " &
                                 "INNER JOIN tblSeatType ON tblSeat.SeatTypeID = tblSeatType.SeatTypeID " &
                                 "WHERE tblBookingSeat.BookingID = @BookingID " &
                                 "ORDER BY tblSeat.SeatRow, tblSeat.SeatNumber"
            SQLCmd.Parameters.AddWithValue("@BookingID", CInt(currentBookingID))
            Dim rs As OleDbDataReader = SQLCmd.ExecuteReader()

            While rs.Read()
                Dim newRow As DataRow = dt.NewRow()
                newRow("Refund") = False
                newRow("SeatID") = CLng(rs("SeatID"))
                newRow("SeatName") = rs("SeatName").ToString()
                newRow("SeatTypeName") = rs("SeatTypeName").ToString()
                newRow("PricePaid") = CDbl(rs("SeatPricePaid"))
                newRow("Status") = "Sold"
                dt.Rows.Add(newRow)
                stillSold = stillSold + 1
            End While

            rs.Close()

            SQLCmd.CommandText = "SELECT tblSeat.SeatRow & tblSeat.SeatNumber AS SeatName, tblSeatType.SeatTypeName, " &
                                 "tblRefundLine.AmountRefunded, tblRefund.RefundDate " &
                                 "FROM ((tblRefundLine INNER JOIN tblRefund ON tblRefundLine.RefundID = tblRefund.RefundID) " &
                                 "INNER JOIN tblSeat ON tblRefundLine.SeatID = tblSeat.SeatID) " &
                                 "INNER JOIN tblSeatType ON tblSeat.SeatTypeID = tblSeatType.SeatTypeID " &
                                 "WHERE tblRefund.BookingID = @BookingID AND tblRefundLine.LineType = @LineType " &
                                 "ORDER BY tblSeat.SeatRow, tblSeat.SeatNumber"
            SQLCmd.Parameters.Clear()
            SQLCmd.Parameters.AddWithValue("@BookingID", CInt(currentBookingID))
            SQLCmd.Parameters.AddWithValue("@LineType", RefundSeatLine)
            Dim rsBack As OleDbDataReader = SQLCmd.ExecuteReader()

            While rsBack.Read()
                Dim backRow As DataRow = dt.NewRow()
                backRow("Refund") = False
                backRow("SeatID") = 0
                backRow("SeatName") = rsBack("SeatName").ToString()
                backRow("SeatTypeName") = rsBack("SeatTypeName").ToString()
                backRow("PricePaid") = CDbl(rsBack("AmountRefunded"))
                backRow("Status") = "Refunded " & Format(CDate(rsBack("RefundDate")), "dd/MM/yyyy")
                dt.Rows.Add(backRow)
            End While

            rsBack.Close()
            cn.Close()
        End If

        fillingGrids = True
        dgvSeats.DataSource = Nothing
        dgvSeats.Columns.Clear()
        dgvSeats.DataSource = dt
        fillingGrids = False

        If dgvSeats.Columns.Count > 0 Then
            dgvSeats.Columns("SeatID").Visible = False

            dgvSeats.Columns("Refund").HeaderText = "Refund"
            dgvSeats.Columns("SeatName").HeaderText = "Seat"
            dgvSeats.Columns("SeatTypeName").HeaderText = "Type"
            dgvSeats.Columns("PricePaid").HeaderText = "Price paid"
            dgvSeats.Columns("Status").HeaderText = "Status"

            dgvSeats.Columns("Refund").Width = 70
            dgvSeats.Columns("SeatName").Width = 100
            dgvSeats.Columns("SeatTypeName").Width = 140
            dgvSeats.Columns("PricePaid").Width = 120
            dgvSeats.Columns("Status").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill

            dgvSeats.Columns("PricePaid").DefaultCellStyle.Format = "C"

            dgvSeats.Columns("SeatName").ReadOnly = True
            dgvSeats.Columns("SeatTypeName").ReadOnly = True
            dgvSeats.Columns("PricePaid").ReadOnly = True
            dgvSeats.Columns("Status").ReadOnly = True

            GreyOutRefundedSeats()
            dgvSeats.ClearSelection()
        End If

        If stillSold = 0 Then
            lblSeatCount.Text = "No seats left on sale against this booking"
        ElseIf stillSold = 1 Then
            lblSeatCount.Text = "1 seat still sold"
        Else
            lblSeatCount.Text = stillSold & " seats still sold"
        End If
    End Sub

    Private Sub GreyOutRefundedSeats()
        For Each row As DataGridViewRow In dgvSeats.Rows
            If row.Cells("Status").Value IsNot Nothing Then
                If row.Cells("Status").Value.ToString() <> "Sold" Then
                    row.DefaultCellStyle.ForeColor = PastFore
                    row.Cells("Refund").ReadOnly = True
                End If
            End If
        Next
    End Sub

    Private Sub LoadFood()
        Dim dt As New DataTable
        dt.Columns.Add("Refund", GetType(Boolean))
        dt.Columns.Add("OrderItemID", GetType(Long))
        dt.Columns.Add("FoodItemName", GetType(String))
        dt.Columns.Add("ItemPricePaid", GetType(Double))
        dt.Columns.Add("Quantity", GetType(Integer))
        dt.Columns.Add("QtyRefunded", GetType(Integer))
        dt.Columns.Add("QtyToRefund", GetType(Integer))
        dt.Columns.Add("StillOwed", GetType(Double))

        Dim dtItems As New DataTable
        Dim dtBack As New DataTable

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT tblOrderItem.OrderItemID, tblFoodItem.FoodItemName, " &
                                 "tblOrderItem.ItemPricePaid, tblOrderItem.Quantity " &
                                 "FROM tblOrderItem INNER JOIN tblFoodItem ON tblOrderItem.FoodItemID = tblFoodItem.FoodItemID " &
                                 "WHERE tblOrderItem.BookingID = @BookingID " &
                                 "ORDER BY tblFoodItem.FoodItemName"
            SQLCmd.Parameters.AddWithValue("@BookingID", CInt(currentBookingID))
            Dim daItems As New OleDbDataAdapter(SQLCmd)
            daItems.Fill(dtItems)

            SQLCmd.CommandText = "SELECT OrderItemID, SUM(QtyRefunded) AS QtyBack " &
                                 "FROM tblRefundLine " &
                                 "WHERE OrderItemID IN (SELECT OrderItemID FROM tblOrderItem WHERE BookingID = @BookingID) " &
                                 "GROUP BY OrderItemID"
            SQLCmd.Parameters.Clear()
            SQLCmd.Parameters.AddWithValue("@BookingID", CInt(currentBookingID))
            Dim daBack As New OleDbDataAdapter(SQLCmd)
            daBack.Fill(dtBack)

            cn.Close()
        End If

        Dim itemsLeft As Integer = 0
        Dim i As Integer

        For i = 0 To dtItems.Rows.Count - 1
            Dim sold As Integer = CInt(dtItems.Rows(i)("Quantity"))
            Dim back As Integer = RefundedQtyFromTable(dtBack, CLng(dtItems.Rows(i)("OrderItemID")))

            Dim newRow As DataRow = dt.NewRow()
            newRow("Refund") = False
            newRow("OrderItemID") = CLng(dtItems.Rows(i)("OrderItemID"))
            newRow("FoodItemName") = dtItems.Rows(i)("FoodItemName").ToString()
            newRow("ItemPricePaid") = CDbl(dtItems.Rows(i)("ItemPricePaid"))
            newRow("Quantity") = sold
            newRow("QtyRefunded") = back
            newRow("QtyToRefund") = 0
            newRow("StillOwed") = (sold - back) * CDbl(dtItems.Rows(i)("ItemPricePaid"))
            dt.Rows.Add(newRow)

            itemsLeft = itemsLeft + (sold - back)
        Next

        fillingGrids = True
        dgvFood.DataSource = Nothing
        dgvFood.Columns.Clear()
        dgvFood.DataSource = dt
        fillingGrids = False

        If dgvFood.Columns.Count > 0 Then
            dgvFood.Columns("OrderItemID").Visible = False

            dgvFood.Columns("Refund").HeaderText = "Refund"
            dgvFood.Columns("FoodItemName").HeaderText = "Item"
            dgvFood.Columns("ItemPricePaid").HeaderText = "Each"
            dgvFood.Columns("Quantity").HeaderText = "Sold"
            dgvFood.Columns("QtyRefunded").HeaderText = "Refunded"
            dgvFood.Columns("QtyToRefund").HeaderText = "Refund now"
            dgvFood.Columns("StillOwed").HeaderText = "Still owed"

            dgvFood.Columns("Refund").Width = 62
            dgvFood.Columns("FoodItemName").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            dgvFood.Columns("ItemPricePaid").Width = 70
            dgvFood.Columns("Quantity").Width = 55
            dgvFood.Columns("QtyRefunded").Width = 80
            dgvFood.Columns("QtyToRefund").Width = 105
            dgvFood.Columns("StillOwed").Width = 100

            dgvFood.Columns("ItemPricePaid").DefaultCellStyle.Format = "C"
            dgvFood.Columns("StillOwed").DefaultCellStyle.Format = "C"
            dgvFood.Columns("Quantity").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            dgvFood.Columns("QtyRefunded").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            dgvFood.Columns("QtyToRefund").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            dgvFood.Columns("FoodItemName").ReadOnly = True
            dgvFood.Columns("ItemPricePaid").ReadOnly = True
            dgvFood.Columns("Quantity").ReadOnly = True
            dgvFood.Columns("QtyRefunded").ReadOnly = True
            dgvFood.Columns("StillOwed").ReadOnly = True

            GreyOutFinishedFood()
            dgvFood.ClearSelection()
        End If

        If dt.Rows.Count = 0 Then
            lblFoodCount.Text = "No food or drink was sold on this booking"
        Else
            lblFoodCount.Text = dt.Rows.Count & " line(s), " & itemsLeft & " item(s) not yet refunded"
        End If
    End Sub

    Private Sub GreyOutFinishedFood()
        For Each row As DataGridViewRow In dgvFood.Rows
            If LeftOnLine(row) <= 0 Then
                row.DefaultCellStyle.ForeColor = PastFore
                row.Cells("QtyToRefund").ReadOnly = True
                row.Cells("Refund").ReadOnly = True
            End If
        Next
    End Sub

    Private Function QtyIn(row As DataGridViewRow, columnName As String) As Integer
        If row.Cells(columnName).Value Is Nothing Then
            Return 0
        End If

        If IsDBNull(row.Cells(columnName).Value) Then
            Return 0
        End If

        Return SafeInt(row.Cells(columnName).Value.ToString())
    End Function

    Private Function LeftOnLine(row As DataGridViewRow) As Integer
        Return QtyIn(row, "Quantity") - QtyIn(row, "QtyRefunded")
    End Function

    Private Function SeatIsSold(row As DataGridViewRow) As Boolean
        If row.Cells("Status").Value Is Nothing Then
            Return False
        End If

        Return row.Cells("Status").Value.ToString() = "Sold"
    End Function

    Private Function IsTicked(row As DataGridViewRow) As Boolean
        If row.Cells("Refund").Value Is Nothing Then
            Return False
        End If

        If IsDBNull(row.Cells("Refund").Value) Then
            Return False
        End If

        Return CBool(row.Cells("Refund").Value)
    End Function

    Private Sub ApplyAccessLevel()
        canRefund = (UserAccessLevel = 1)

        btnRefundSelected.Enabled = canRefund
        btnRefundEverything.Enabled = canRefund
        txtReason.Enabled = canRefund

        If canRefund Then
            lblNotAllowed.Text = ""
        Else
            lblNotAllowed.Text = "A manager has to authorise a refund"
            lblNotAllowed.ForeColor = PastFore
            dgvSeats.ReadOnly = True
            dgvFood.ReadOnly = True
        End If

        If bookingIsCancelled Then
            btnRefundSelected.Enabled = False
            btnRefundEverything.Enabled = False
        End If
    End Sub

    Private Function SelectedRefundTotal() As Double
        Dim total As Double = 0

        For Each row As DataGridViewRow In dgvSeats.Rows
            If SeatIsSold(row) AndAlso IsTicked(row) Then
                total = total + CDbl(row.Cells("PricePaid").Value)
            End If
        Next

        For Each row As DataGridViewRow In dgvFood.Rows
            Dim wanted As Integer = QtyIn(row, "QtyToRefund")

            If wanted > 0 Then
                total = total + wanted * CDbl(row.Cells("ItemPricePaid").Value)
            End If
        Next

        Return total
    End Function

    Private Sub UpdateRefundTotal()
        lblRefundTotal.Text = "Refund total: " & FormatCurrency(SelectedRefundTotal())
    End Sub

    Private Sub dgvSeats_CurrentCellDirtyStateChanged(sender As Object, e As EventArgs) Handles dgvSeats.CurrentCellDirtyStateChanged
        If dgvSeats.IsCurrentCellDirty Then
            dgvSeats.CommitEdit(DataGridViewDataErrorContexts.Commit)
        End If
    End Sub

    Private Sub dgvSeats_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles dgvSeats.CellValueChanged
        If fillingGrids Then Exit Sub
        If e.RowIndex < 0 Then Exit Sub

        UpdateRefundTotal()
    End Sub

    Private Sub dgvFood_CurrentCellDirtyStateChanged(sender As Object, e As EventArgs) Handles dgvFood.CurrentCellDirtyStateChanged
        If dgvFood.CurrentCell Is Nothing Then Exit Sub
        If dgvFood.Columns(dgvFood.CurrentCell.ColumnIndex).Name <> "Refund" Then Exit Sub

        If dgvFood.IsCurrentCellDirty Then
            dgvFood.CommitEdit(DataGridViewDataErrorContexts.Commit)
        End If
    End Sub

    Private Sub dgvFood_EditingControlShowing(sender As Object, e As DataGridViewEditingControlShowingEventArgs) Handles dgvFood.EditingControlShowing
        RemoveHandler e.Control.KeyPress, AddressOf QuantityKeyPress

        If dgvFood.CurrentCell Is Nothing Then Exit Sub

        If dgvFood.Columns(dgvFood.CurrentCell.ColumnIndex).Name = "QtyToRefund" Then
            AddHandler e.Control.KeyPress, AddressOf QuantityKeyPress
        End If
    End Sub

    Private Sub QuantityKeyPress(sender As Object, e As KeyPressEventArgs)
        If Not Char.IsDigit(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    Private Sub dgvFood_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles dgvFood.CellValueChanged
        If fillingGrids Then Exit Sub
        If e.RowIndex < 0 Then Exit Sub

        Dim columnName As String = dgvFood.Columns(e.ColumnIndex).Name

        If columnName <> "Refund" AndAlso columnName <> "QtyToRefund" Then Exit Sub

        Dim row As DataGridViewRow = dgvFood.Rows(e.RowIndex)
        Dim canGoBack As Integer = LeftOnLine(row)
        Dim wanted As Integer

        If columnName = "Refund" Then
            If IsTicked(row) Then
                wanted = canGoBack
            Else
                wanted = 0
            End If
        Else
            wanted = QtyIn(row, "QtyToRefund")

            If wanted < 0 Then
                wanted = 0
            End If

            If wanted > canGoBack Then
                wanted = canGoBack
            End If
        End If

        fillingGrids = True
        row.Cells("QtyToRefund").Value = wanted
        row.Cells("Refund").Value = (wanted > 0)
        fillingGrids = False

        UpdateRefundTotal()
    End Sub

    Private Sub dgvFood_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles dgvFood.DataError
        e.ThrowException = False
        e.Cancel = False
    End Sub

    Private Sub btnRefundSelected_Click(sender As Object, e As EventArgs) Handles btnRefundSelected.Click
        DoRefund(False)
    End Sub

    Private Sub btnRefundEverything_Click(sender As Object, e As EventArgs) Handles btnRefundEverything.Click
        If MessageBox.Show("Refund the whole of booking " & currentBookingID & "?" & vbCrLf &
                           "Every seat still sold goes back on sale and everything not already refunded is paid back.",
                           "Refund everything", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
            Exit Sub
        End If

        fillingGrids = True

        For Each row As DataGridViewRow In dgvSeats.Rows
            If SeatIsSold(row) Then
                row.Cells("Refund").Value = True
            End If
        Next

        For Each row As DataGridViewRow In dgvFood.Rows
            row.Cells("QtyToRefund").Value = LeftOnLine(row)
            row.Cells("Refund").Value = (LeftOnLine(row) > 0)
        Next

        fillingGrids = False
        UpdateRefundTotal()

        DoRefund(True)
    End Sub

    Private Sub DoRefund(skipConfirm As Boolean)
        If Not canRefund Then
            MessageBox.Show("Only a manager can give money back.", "Refund", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Dim seatLines As New DataTable
        seatLines.Columns.Add("SeatID", GetType(Long))
        seatLines.Columns.Add("AmountRefunded", GetType(Double))

        Dim foodLines As New DataTable
        foodLines.Columns.Add("OrderItemID", GetType(Long))
        foodLines.Columns.Add("QtyRefunded", GetType(Integer))
        foodLines.Columns.Add("AmountRefunded", GetType(Double))

        For Each row As DataGridViewRow In dgvSeats.Rows
            If SeatIsSold(row) Then
                If IsTicked(row) Then
                    Dim seatRow As DataRow = seatLines.NewRow()
                    seatRow("SeatID") = CLng(row.Cells("SeatID").Value)
                    seatRow("AmountRefunded") = CDbl(row.Cells("PricePaid").Value)
                    seatLines.Rows.Add(seatRow)
                End If
            End If
        Next

        For Each row As DataGridViewRow In dgvFood.Rows
            Dim wanted As Integer = QtyIn(row, "QtyToRefund")

            If wanted > 0 Then
                Dim foodRow As DataRow = foodLines.NewRow()
                foodRow("OrderItemID") = CLng(row.Cells("OrderItemID").Value)
                foodRow("QtyRefunded") = wanted
                foodRow("AmountRefunded") = wanted * CDbl(row.Cells("ItemPricePaid").Value)
                foodLines.Rows.Add(foodRow)
            End If
        Next

        If seatLines.Rows.Count = 0 AndAlso foodLines.Rows.Count = 0 Then
            MessageBox.Show("Tick a seat or type a quantity against a food line first", "Refund", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If txtReason.Text.Trim() = "" Then
            MessageBox.Show("Say why the money is going back before refunding it", "Refund", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtReason.Focus()
            Exit Sub
        End If

        Dim amount As Double = SelectedRefundTotal()

        If Not skipConfirm Then
            If MessageBox.Show("Refund " & FormatCurrency(amount) & " on booking " & currentBookingID & "?" & vbCrLf &
                               seatLines.Rows.Count & " seat(s) and " & foodLines.Rows.Count & " food line(s)." & vbCrLf &
                               "Refunded seats go straight back on sale.",
                               "Confirm refund", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
                Exit Sub
            End If
        End If

        Dim refundID As Long = TakeRefund(currentBookingID, seatLines, foodLines, txtReason.Text.Trim(), CurrentLoginID)

        If refundID > 0 Then
            MessageBox.Show(FormatCurrency(amount) & " refunded and written down as refund " & refundID & ".",
                            "Refund", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If

        RefreshBooking()
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

End Class
