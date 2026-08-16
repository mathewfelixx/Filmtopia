Imports System.Data.OleDb

Module modBookings

    Public Const BookingActive As String = "Active"
    Public Const BookingCancelled As String = "Cancelled"

    Public Const RefundSeatLine As String = "Seat"
    Public Const RefundFoodLine As String = "Food"

    Public Const SeatStandard As String = "Standard"
    Public Const SeatPremium As String = "Premium"
    Public Const SeatAccessible As String = "Accessible"
    Public Const SeatSaver As String = "Saver"

    Public Const WalkInForename As String = "Walk-in"
    Public Const WalkInSurname As String = "Customer"

    Public Const PlanAllStandard As String = "Every seat the same"
    Public Const PlanCentreBlock As String = "Premium block in the middle"
    Public Const PlanPremiumBack As String = "Premium at the back"
    Public Const PlanBudget As String = "Budget screen"

    Public Function SeatPrice(basePrice As Double, multiplier As Double) As Double
        Return Math.Round(basePrice * multiplier, 2)
    End Function

    Public Function TypeIDFromTable(dtTypes As DataTable, typeName As String) As Long
        For Each row As DataRow In dtTypes.Rows
            If row("SeatTypeName").ToString() = typeName Then
                Return CLng(row("SeatTypeID"))
            End If
        Next

        Return 0
    End Function

    Public Function SaverRowCount(numRows As Integer) As Integer
        If numRows <= 2 Then
            Return 0
        End If

        If numRows <= 5 Then
            Return 1
        End If

        Return 2
    End Function

    Public Function PlanSeatType(planName As String, rowIndex As Integer, seatIndex As Integer,
                                 numRows As Integer, perRow As Integer) As String

        If perRow >= 4 AndAlso rowIndex = numRows \ 2 Then
            If seatIndex = 0 OrElse seatIndex = perRow - 1 Then
                Return SeatAccessible
            End If
        End If

        If planName = PlanAllStandard Then
            Return SeatStandard
        End If

        Dim saverRows As Integer = SaverRowCount(numRows)

        If planName = PlanBudget Then
            If rowIndex < numRows \ 2 Then
                Return SeatSaver
            End If

            Return SeatStandard
        End If

        If rowIndex < saverRows Then
            Return SeatSaver
        End If

        If planName = PlanCentreBlock AndAlso numRows >= 5 Then
            Dim blockRows As Integer = numRows \ 3
            If blockRows < 2 Then
                blockRows = 2
            End If

            Dim firstBlockRow As Integer = (numRows - blockRows) \ 2
            If firstBlockRow < saverRows Then
                firstBlockRow = saverRows
            End If

            Dim lastBlockRow As Integer = firstBlockRow + blockRows - 1
            Dim firstBlockSeat As Integer = perRow \ 4
            Dim lastBlockSeat As Integer = perRow - 1 - (perRow \ 4)

            If rowIndex >= firstBlockRow AndAlso rowIndex <= lastBlockRow Then
                If seatIndex >= firstBlockSeat AndAlso seatIndex <= lastBlockSeat Then
                    Return SeatPremium
                End If
            End If

            Return SeatStandard
        End If

        If numRows >= 4 AndAlso rowIndex >= numRows - 2 Then
            Return SeatPremium
        End If

        Return SeatStandard
    End Function

    Public Function CompleteSale(customerID As Long, isWalkIn As Boolean, screeningID As Long,
                                 seatIDs() As Long, foodOrder As DataTable, totalCost As Double,
                                 soldByLoginID As Long) As Long
        Dim newID As Long = 0

        If DbConnect() Then
            Dim trans As OleDbTransaction = cn.BeginTransaction()

            Try
                Dim SQLCmd As New OleDbCommand
                SQLCmd.Connection = cn
                SQLCmd.Transaction = trans

                Dim seatGone As Boolean = False
                Dim s As Integer

                For s = 0 To seatIDs.Length - 1
                    SQLCmd.CommandText = "SELECT COUNT(*) FROM tblBookingSeat " &
                                         "WHERE ScreeningID = @ScreeningID AND SeatID = @SeatID"
                    SQLCmd.Parameters.Clear()
                    SQLCmd.Parameters.AddWithValue("@ScreeningID", CInt(screeningID))
                    SQLCmd.Parameters.AddWithValue("@SeatID", CInt(seatIDs(s)))

                    If CInt(SQLCmd.ExecuteScalar()) > 0 Then
                        seatGone = True
                    End If
                Next

                If seatGone Then
                    trans.Rollback()
                    cn.Close()
                    MessageBox.Show("One of those seats has just been booked on another till." & vbNewLine &
                                    "Nothing has been saved, so pick the seats again and take the sale once more.",
                                    "Seat gone", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return 0
                End If

                Dim saleCustomerID As Long = customerID

                If isWalkIn Then
                    SQLCmd.CommandText = "SELECT MIN(CustomerID) FROM tblCustomer " &
                                         "WHERE CustomerForename = @CustomerForename AND CustomerSurname = @CustomerSurname"
                    SQLCmd.Parameters.Clear()
                    SQLCmd.Parameters.AddWithValue("@CustomerForename", WalkInForename)
                    SQLCmd.Parameters.AddWithValue("@CustomerSurname", WalkInSurname)
                    Dim walkInResult As Object = SQLCmd.ExecuteScalar()

                    If walkInResult IsNot Nothing AndAlso Not IsDBNull(walkInResult) Then
                        saleCustomerID = CLng(walkInResult)
                    Else
                        SQLCmd.CommandText = "INSERT INTO tblCustomer (CustomerForename, CustomerSurname, CustomerEmail, CustomerPhone) " &
                                             "VALUES (@CustomerForename, @CustomerSurname, @CustomerEmail, @CustomerPhone)"
                        SQLCmd.Parameters.Clear()
                        SQLCmd.Parameters.AddWithValue("@CustomerForename", WalkInForename)
                        SQLCmd.Parameters.AddWithValue("@CustomerSurname", WalkInSurname)
                        SQLCmd.Parameters.AddWithValue("@CustomerEmail", "")
                        SQLCmd.Parameters.AddWithValue("@CustomerPhone", "")
                        SQLCmd.ExecuteNonQuery()

                        SQLCmd.CommandText = "SELECT @@IDENTITY"
                        SQLCmd.Parameters.Clear()
                        saleCustomerID = CLng(SQLCmd.ExecuteScalar())
                    End If
                End If

                SQLCmd.CommandText = "INSERT INTO tblBooking (CustomerID, ScreeningID, BookingDate, TotalCost, BookingStatus, LoginID) " &
                                     "VALUES (@CustomerID, @ScreeningID, @BookingDate, @TotalCost, @BookingStatus, @LoginID)"
                SQLCmd.Parameters.Clear()
                SQLCmd.Parameters.AddWithValue("@CustomerID", CInt(saleCustomerID))

                If screeningID = 0 Then
                    SQLCmd.Parameters.AddWithValue("@ScreeningID", DBNull.Value)
                Else
                    SQLCmd.Parameters.AddWithValue("@ScreeningID", CInt(screeningID))
                End If

                SQLCmd.Parameters.AddWithValue("@BookingDate", Date.Now.Date)
                SQLCmd.Parameters.AddWithValue("@TotalCost", totalCost)
                SQLCmd.Parameters.AddWithValue("@BookingStatus", BookingActive)

                If soldByLoginID = 0 Then
                    SQLCmd.Parameters.AddWithValue("@LoginID", DBNull.Value)
                Else
                    SQLCmd.Parameters.AddWithValue("@LoginID", CInt(soldByLoginID))
                End If

                SQLCmd.ExecuteNonQuery()

                SQLCmd.CommandText = "SELECT @@IDENTITY"
                SQLCmd.Parameters.Clear()
                newID = CLng(SQLCmd.ExecuteScalar())

                Dim ticketPrice As Double = 0

                If screeningID <> 0 Then
                    SQLCmd.CommandText = "SELECT TicketPrice FROM tblScreening WHERE ScreeningID = @ScreeningID"
                    SQLCmd.Parameters.Clear()
                    SQLCmd.Parameters.AddWithValue("@ScreeningID", CInt(screeningID))
                    Dim priceResult = SQLCmd.ExecuteScalar()
                    If priceResult IsNot Nothing AndAlso Not IsDBNull(priceResult) Then
                        ticketPrice = CDbl(priceResult)
                    End If
                End If

                Dim i As Integer
                For i = 0 To seatIDs.Length - 1
                    SQLCmd.CommandText = "SELECT tblSeatType.PriceMultiplier " &
                                         "FROM tblSeat INNER JOIN tblSeatType ON tblSeat.SeatTypeID = tblSeatType.SeatTypeID " &
                                         "WHERE tblSeat.SeatID = @SeatID"
                    SQLCmd.Parameters.Clear()
                    SQLCmd.Parameters.AddWithValue("@SeatID", CInt(seatIDs(i)))
                    Dim multiplier As Double = 1
                    Dim multiplierResult = SQLCmd.ExecuteScalar()
                    If multiplierResult IsNot Nothing AndAlso Not IsDBNull(multiplierResult) Then
                        multiplier = CDbl(multiplierResult)
                    End If

                    SQLCmd.CommandText = "INSERT INTO tblBookingSeat (BookingID, SeatID, ScreeningID, SeatPricePaid) " &
                                         "VALUES (@BookingID, @SeatID, @ScreeningID, @SeatPricePaid)"
                    SQLCmd.Parameters.Clear()
                    SQLCmd.Parameters.AddWithValue("@BookingID", CInt(newID))
                    SQLCmd.Parameters.AddWithValue("@SeatID", CInt(seatIDs(i)))
                    SQLCmd.Parameters.AddWithValue("@ScreeningID", CInt(screeningID))
                    SQLCmd.Parameters.AddWithValue("@SeatPricePaid", SeatPrice(ticketPrice, multiplier))
                    SQLCmd.ExecuteNonQuery()
                Next

                Dim f As Integer
                For f = 0 To foodOrder.Rows.Count - 1
                    SQLCmd.CommandText = "INSERT INTO tblOrderItem (BookingID, FoodItemID, Quantity, ItemPricePaid) " &
                                         "VALUES (@BookingID, @FoodItemID, @Quantity, @ItemPricePaid)"
                    SQLCmd.Parameters.Clear()
                    SQLCmd.Parameters.AddWithValue("@BookingID", CInt(newID))
                    SQLCmd.Parameters.AddWithValue("@FoodItemID", CInt(foodOrder.Rows(f)("FoodItemID")))
                    SQLCmd.Parameters.AddWithValue("@Quantity", CInt(foodOrder.Rows(f)("Quantity")))
                    SQLCmd.Parameters.AddWithValue("@ItemPricePaid", CDbl(foodOrder.Rows(f)("Price")))
                    SQLCmd.ExecuteNonQuery()
                Next

                trans.Commit()

            Catch ex As Exception
                trans.Rollback()
                newID = 0
                MessageBox.Show("The sale could not be saved, so nothing at all was written. " & ex.Message,
                                "Sale", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try

            cn.Close()
        End If

        If newID > 0 Then
            RecalculateBookingTotal(newID)
        End If

        Return newID
    End Function

    Public Sub RecalculateBookingTotal(bookingID As Long)
        Dim ticketTotal As Double = 0
        Dim foodTotal As Double = 0
        Dim dtItems As New DataTable
        Dim dtBack As New DataTable

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn

            SQLCmd.CommandText = "SELECT SUM(SeatPricePaid) FROM tblBookingSeat WHERE BookingID = @BookingID"
            SQLCmd.Parameters.AddWithValue("@BookingID", CInt(bookingID))
            Dim ticketResult = SQLCmd.ExecuteScalar()
            If ticketResult IsNot Nothing AndAlso Not IsDBNull(ticketResult) Then
                ticketTotal = CDbl(ticketResult)
            End If

            SQLCmd.CommandText = "SELECT OrderItemID, Quantity, ItemPricePaid " &
                                 "FROM tblOrderItem " &
                                 "WHERE BookingID = @BookingID"
            SQLCmd.Parameters.Clear()
            SQLCmd.Parameters.AddWithValue("@BookingID", CInt(bookingID))
            Dim daItems As New OleDbDataAdapter(SQLCmd)
            daItems.Fill(dtItems)

            SQLCmd.CommandText = "SELECT OrderItemID, SUM(QtyRefunded) AS QtyBack " &
                                 "FROM tblRefundLine " &
                                 "WHERE OrderItemID IN (SELECT OrderItemID FROM tblOrderItem WHERE BookingID = @BookingID) " &
                                 "GROUP BY OrderItemID"
            SQLCmd.Parameters.Clear()
            SQLCmd.Parameters.AddWithValue("@BookingID", CInt(bookingID))
            Dim daBack As New OleDbDataAdapter(SQLCmd)
            daBack.Fill(dtBack)

            cn.Close()
        End If

        Dim i As Integer
        For i = 0 To dtItems.Rows.Count - 1
            Dim stillOwed As Integer = CInt(dtItems.Rows(i)("Quantity")) -
                                       RefundedQtyFromTable(dtBack, CLng(dtItems.Rows(i)("OrderItemID")))

            If stillOwed > 0 Then
                foodTotal = foodTotal + stillOwed * CDbl(dtItems.Rows(i)("ItemPricePaid"))
            End If
        Next

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "UPDATE tblBooking " &
                                 "SET TotalCost = @TotalCost " &
                                 "WHERE BookingID = @BookingID"
            SQLCmd.Parameters.AddWithValue("@TotalCost", ticketTotal + foodTotal)
            SQLCmd.Parameters.AddWithValue("@BookingID", CInt(bookingID))
            SQLCmd.ExecuteNonQuery()
            cn.Close()
        End If
    End Sub

    Public Function RefundedQtyFromTable(dtBack As DataTable, orderItemID As Long) As Integer
        Dim r As Integer

        For r = 0 To dtBack.Rows.Count - 1
            If CLng(dtBack.Rows(r)("OrderItemID")) = orderItemID Then
                If IsDBNull(dtBack.Rows(r)("QtyBack")) Then
                    Return 0
                End If

                Return CInt(dtBack.Rows(r)("QtyBack"))
            End If
        Next

        Return 0
    End Function

    Public Function RecalculateAllBookingTotals() As Integer
        Dim bookingIDs(-1) As Long
        Dim howMany As Integer = 0

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT COUNT(*) FROM tblBooking"
            howMany = CInt(SQLCmd.ExecuteScalar())

            ReDim bookingIDs(howMany - 1)

            SQLCmd.CommandText = "SELECT BookingID FROM tblBooking ORDER BY BookingID"
            Dim rs As OleDbDataReader = SQLCmd.ExecuteReader()
            Dim i As Integer = 0
            While rs.Read()
                bookingIDs(i) = CLng(rs("BookingID"))
                i = i + 1
            End While
            rs.Close()
            cn.Close()
        End If

        Dim n As Integer
        For n = 0 To bookingIDs.Length - 1
            RecalculateBookingTotal(bookingIDs(n))
        Next

        Return howMany
    End Function

    Public Function TakeRefund(bookingID As Long, seatLines As DataTable, foodLines As DataTable,
                               reason As String, byLoginID As Long) As Long
        Dim newID As Long = 0
        Dim refundTotal As Double = 0
        Dim wholeSale As Boolean = False
        Dim s As Integer
        Dim f As Integer

        If seatLines.Rows.Count = 0 AndAlso foodLines.Rows.Count = 0 Then
            Return 0
        End If

        For s = 0 To seatLines.Rows.Count - 1
            refundTotal = refundTotal + CDbl(seatLines.Rows(s)("AmountRefunded"))
        Next

        For f = 0 To foodLines.Rows.Count - 1
            refundTotal = refundTotal + CDbl(foodLines.Rows(f)("AmountRefunded"))
        Next

        If DbConnect() Then
            Dim trans As OleDbTransaction = cn.BeginTransaction()

            Try
                Dim SQLCmd As New OleDbCommand
                SQLCmd.Connection = cn
                SQLCmd.Transaction = trans

                Dim alreadyDone As Boolean = False

                For s = 0 To seatLines.Rows.Count - 1
                    SQLCmd.CommandText = "SELECT COUNT(*) FROM tblBookingSeat " &
                                         "WHERE BookingID = @BookingID AND SeatID = @SeatID"
                    SQLCmd.Parameters.Clear()
                    SQLCmd.Parameters.AddWithValue("@BookingID", CInt(bookingID))
                    SQLCmd.Parameters.AddWithValue("@SeatID", CInt(seatLines.Rows(s)("SeatID")))

                    If CInt(SQLCmd.ExecuteScalar()) = 0 Then
                        alreadyDone = True
                    End If
                Next

                For f = 0 To foodLines.Rows.Count - 1
                    Dim soldQty As Integer = 0
                    Dim backQty As Integer = 0

                    SQLCmd.CommandText = "SELECT Quantity FROM tblOrderItem WHERE OrderItemID = @OrderItemID"
                    SQLCmd.Parameters.Clear()
                    SQLCmd.Parameters.AddWithValue("@OrderItemID", CInt(foodLines.Rows(f)("OrderItemID")))
                    Dim soldResult = SQLCmd.ExecuteScalar()

                    If soldResult IsNot Nothing AndAlso Not IsDBNull(soldResult) Then
                        soldQty = CInt(soldResult)
                    End If

                    SQLCmd.CommandText = "SELECT SUM(QtyRefunded) FROM tblRefundLine WHERE OrderItemID = @OrderItemID"
                    SQLCmd.Parameters.Clear()
                    SQLCmd.Parameters.AddWithValue("@OrderItemID", CInt(foodLines.Rows(f)("OrderItemID")))
                    Dim backResult = SQLCmd.ExecuteScalar()

                    If backResult IsNot Nothing AndAlso Not IsDBNull(backResult) Then
                        backQty = CInt(backResult)
                    End If

                    If CInt(foodLines.Rows(f)("QtyRefunded")) > soldQty - backQty Then
                        alreadyDone = True
                    End If
                Next

                If alreadyDone Then
                    trans.Rollback()
                    cn.Close()
                    MessageBox.Show("Some of that has already been refunded on another till." & vbNewLine &
                                    "Nothing has been paid back, so open the booking again and check what is left.",
                                    "Already refunded", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return 0
                End If

                SQLCmd.CommandText = "INSERT INTO tblRefund (BookingID, RefundDate, RefundAmount, RefundReason, LoginID) " &
                                     "VALUES (@BookingID, Now(), @RefundAmount, @RefundReason, @LoginID)"
                SQLCmd.Parameters.Clear()
                SQLCmd.Parameters.AddWithValue("@BookingID", CInt(bookingID))
                SQLCmd.Parameters.AddWithValue("@RefundAmount", refundTotal)
                SQLCmd.Parameters.AddWithValue("@RefundReason", reason)

                If byLoginID = 0 Then
                    SQLCmd.Parameters.AddWithValue("@LoginID", DBNull.Value)
                Else
                    SQLCmd.Parameters.AddWithValue("@LoginID", CInt(byLoginID))
                End If

                SQLCmd.ExecuteNonQuery()

                SQLCmd.CommandText = "SELECT @@IDENTITY"
                SQLCmd.Parameters.Clear()
                newID = CLng(SQLCmd.ExecuteScalar())

                For s = 0 To seatLines.Rows.Count - 1
                    SQLCmd.CommandText = "INSERT INTO tblRefundLine (RefundID, LineType, SeatID, QtyRefunded, AmountRefunded) " &
                                         "VALUES (@RefundID, @LineType, @SeatID, @QtyRefunded, @AmountRefunded)"
                    SQLCmd.Parameters.Clear()
                    SQLCmd.Parameters.AddWithValue("@RefundID", CInt(newID))
                    SQLCmd.Parameters.AddWithValue("@LineType", RefundSeatLine)
                    SQLCmd.Parameters.AddWithValue("@SeatID", CInt(seatLines.Rows(s)("SeatID")))
                    SQLCmd.Parameters.AddWithValue("@QtyRefunded", 1)
                    SQLCmd.Parameters.AddWithValue("@AmountRefunded", CDbl(seatLines.Rows(s)("AmountRefunded")))
                    SQLCmd.ExecuteNonQuery()

                    SQLCmd.CommandText = "DELETE FROM tblBookingSeat " &
                                         "WHERE BookingID = @BookingID AND SeatID = @SeatID"
                    SQLCmd.Parameters.Clear()
                    SQLCmd.Parameters.AddWithValue("@BookingID", CInt(bookingID))
                    SQLCmd.Parameters.AddWithValue("@SeatID", CInt(seatLines.Rows(s)("SeatID")))
                    SQLCmd.ExecuteNonQuery()
                Next

                For f = 0 To foodLines.Rows.Count - 1
                    SQLCmd.CommandText = "INSERT INTO tblRefundLine (RefundID, LineType, OrderItemID, QtyRefunded, AmountRefunded) " &
                                         "VALUES (@RefundID, @LineType, @OrderItemID, @QtyRefunded, @AmountRefunded)"
                    SQLCmd.Parameters.Clear()
                    SQLCmd.Parameters.AddWithValue("@RefundID", CInt(newID))
                    SQLCmd.Parameters.AddWithValue("@LineType", RefundFoodLine)
                    SQLCmd.Parameters.AddWithValue("@OrderItemID", CInt(foodLines.Rows(f)("OrderItemID")))
                    SQLCmd.Parameters.AddWithValue("@QtyRefunded", CInt(foodLines.Rows(f)("QtyRefunded")))
                    SQLCmd.Parameters.AddWithValue("@AmountRefunded", CDbl(foodLines.Rows(f)("AmountRefunded")))
                    SQLCmd.ExecuteNonQuery()
                Next

                Dim seatsLeft As Integer = 0
                Dim soldLeft As Integer = 0
                Dim backLeft As Integer = 0

                SQLCmd.CommandText = "SELECT COUNT(*) FROM tblBookingSeat WHERE BookingID = @BookingID"
                SQLCmd.Parameters.Clear()
                SQLCmd.Parameters.AddWithValue("@BookingID", CInt(bookingID))
                seatsLeft = CInt(SQLCmd.ExecuteScalar())

                SQLCmd.CommandText = "SELECT SUM(Quantity) FROM tblOrderItem WHERE BookingID = @BookingID"
                SQLCmd.Parameters.Clear()
                SQLCmd.Parameters.AddWithValue("@BookingID", CInt(bookingID))
                Dim soldLeftResult = SQLCmd.ExecuteScalar()

                If soldLeftResult IsNot Nothing AndAlso Not IsDBNull(soldLeftResult) Then
                    soldLeft = CInt(soldLeftResult)
                End If

                SQLCmd.CommandText = "SELECT SUM(QtyRefunded) FROM tblRefundLine " &
                                     "WHERE OrderItemID IN (SELECT OrderItemID FROM tblOrderItem WHERE BookingID = @BookingID)"
                SQLCmd.Parameters.Clear()
                SQLCmd.Parameters.AddWithValue("@BookingID", CInt(bookingID))
                Dim backLeftResult = SQLCmd.ExecuteScalar()

                If backLeftResult IsNot Nothing AndAlso Not IsDBNull(backLeftResult) Then
                    backLeft = CInt(backLeftResult)
                End If

                If seatsLeft = 0 AndAlso soldLeft - backLeft = 0 Then
                    SQLCmd.CommandText = "UPDATE tblBooking " &
                                         "SET BookingStatus = @BookingStatus, CancelledDate = Now() " &
                                         "WHERE BookingID = @BookingID AND BookingStatus <> @AlreadyCancelled"
                    SQLCmd.Parameters.Clear()
                    SQLCmd.Parameters.AddWithValue("@BookingStatus", BookingCancelled)
                    SQLCmd.Parameters.AddWithValue("@BookingID", CInt(bookingID))
                    SQLCmd.Parameters.AddWithValue("@AlreadyCancelled", BookingCancelled)
                    SQLCmd.ExecuteNonQuery()
                    wholeSale = True
                End If

                trans.Commit()

            Catch ex As Exception
                trans.Rollback()
                newID = 0
                MessageBox.Show("The refund could not be saved, so nothing at all was paid back. " & ex.Message,
                                "Refund", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try

            cn.Close()
        End If

        If newID > 0 Then
            RecalculateBookingTotal(bookingID)

            Dim whatHappened As String = "Refund " & newID & " of " & FormatCurrency(refundTotal) &
                                         " on booking " & bookingID & " - " &
                                         seatLines.Rows.Count & " seat(s), " &
                                         foodLines.Rows.Count & " food line(s) - " & reason

            If wholeSale Then
                whatHappened = whatHappened & " - nothing left, booking now cancelled"
            End If

            WriteLog("REFUND", whatHappened, LogChange)
        End If

        Return newID
    End Function

    Public Function BookingRefundTotal(bookingID As Long) As Double
        Dim total As Double = 0

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT SUM(RefundAmount) FROM tblRefund WHERE BookingID = @BookingID"
            SQLCmd.Parameters.AddWithValue("@BookingID", CInt(bookingID))
            Dim result = SQLCmd.ExecuteScalar()

            If result IsNot Nothing AndAlso Not IsDBNull(result) Then
                total = CDbl(result)
            End If

            cn.Close()
        End If

        Return total
    End Function

    Public Function OrderItemRefundedQty(orderItemID As Long) As Integer
        Dim qty As Integer = 0

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT SUM(QtyRefunded) FROM tblRefundLine WHERE OrderItemID = @OrderItemID"
            SQLCmd.Parameters.AddWithValue("@OrderItemID", CInt(orderItemID))
            Dim result = SQLCmd.ExecuteScalar()

            If result IsNot Nothing AndAlso Not IsDBNull(result) Then
                qty = CInt(result)
            End If

            cn.Close()
        End If

        Return qty
    End Function

    Public Function RefundWholeBooking(bookingID As Long, reason As String, byLoginID As Long) As Long
        Dim seatLines As New DataTable
        seatLines.Columns.Add("SeatID", GetType(Long))
        seatLines.Columns.Add("AmountRefunded", GetType(Double))

        Dim foodLines As New DataTable
        foodLines.Columns.Add("OrderItemID", GetType(Long))
        foodLines.Columns.Add("QtyRefunded", GetType(Integer))
        foodLines.Columns.Add("AmountRefunded", GetType(Double))

        Dim dtItems As New DataTable
        Dim dtBack As New DataTable

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT SeatID, SeatPricePaid FROM tblBookingSeat WHERE BookingID = @BookingID"
            SQLCmd.Parameters.AddWithValue("@BookingID", CInt(bookingID))
            Dim rs As OleDbDataReader = SQLCmd.ExecuteReader()

            While rs.Read()
                Dim seatRow As DataRow = seatLines.NewRow()
                seatRow("SeatID") = CLng(rs("SeatID"))
                seatRow("AmountRefunded") = CDbl(rs("SeatPricePaid"))
                seatLines.Rows.Add(seatRow)
            End While

            rs.Close()

            SQLCmd.CommandText = "SELECT OrderItemID, Quantity, ItemPricePaid " &
                                 "FROM tblOrderItem " &
                                 "WHERE BookingID = @BookingID"
            SQLCmd.Parameters.Clear()
            SQLCmd.Parameters.AddWithValue("@BookingID", CInt(bookingID))
            Dim daItems As New OleDbDataAdapter(SQLCmd)
            daItems.Fill(dtItems)

            SQLCmd.CommandText = "SELECT OrderItemID, SUM(QtyRefunded) AS QtyBack " &
                                 "FROM tblRefundLine " &
                                 "WHERE OrderItemID IN (SELECT OrderItemID FROM tblOrderItem WHERE BookingID = @BookingID) " &
                                 "GROUP BY OrderItemID"
            SQLCmd.Parameters.Clear()
            SQLCmd.Parameters.AddWithValue("@BookingID", CInt(bookingID))
            Dim daBack As New OleDbDataAdapter(SQLCmd)
            daBack.Fill(dtBack)

            cn.Close()
        End If

        Dim i As Integer

        For i = 0 To dtItems.Rows.Count - 1
            Dim stillOwed As Integer = CInt(dtItems.Rows(i)("Quantity")) -
                                       RefundedQtyFromTable(dtBack, CLng(dtItems.Rows(i)("OrderItemID")))

            If stillOwed > 0 Then
                Dim foodRow As DataRow = foodLines.NewRow()
                foodRow("OrderItemID") = CLng(dtItems.Rows(i)("OrderItemID"))
                foodRow("QtyRefunded") = stillOwed
                foodRow("AmountRefunded") = stillOwed * CDbl(dtItems.Rows(i)("ItemPricePaid"))
                foodLines.Rows.Add(foodRow)
            End If
        Next

        Return TakeRefund(bookingID, seatLines, foodLines, reason, byLoginID)
    End Function

End Module
