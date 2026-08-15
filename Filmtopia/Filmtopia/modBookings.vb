Imports System.Data.OleDb

Module modBookings

    Public Const BookingActive As String = "Active"
    Public Const BookingCancelled As String = "Cancelled"

    Public Const SeatStandard As String = "Standard"
    Public Const SeatPremium As String = "Premium"
    Public Const SeatAccessible As String = "Accessible"

    Public Function SeatPrice(basePrice As Double, multiplier As Double) As Double
        Return Math.Round(basePrice * multiplier, 2)
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
                    SQLCmd.CommandText = "INSERT INTO tblCustomer (CustomerForename, CustomerSurname, CustomerEmail, CustomerPhone) " &
                                         "VALUES (@CustomerForename, @CustomerSurname, @CustomerEmail, @CustomerPhone)"
                    SQLCmd.Parameters.Clear()
                    SQLCmd.Parameters.AddWithValue("@CustomerForename", "Walk-in")
                    SQLCmd.Parameters.AddWithValue("@CustomerSurname", "Customer")
                    SQLCmd.Parameters.AddWithValue("@CustomerEmail", "")
                    SQLCmd.Parameters.AddWithValue("@CustomerPhone", "")
                    SQLCmd.ExecuteNonQuery()

                    SQLCmd.CommandText = "SELECT @@IDENTITY"
                    SQLCmd.Parameters.Clear()
                    saleCustomerID = CLng(SQLCmd.ExecuteScalar())
                End If

                SQLCmd.CommandText = "INSERT INTO tblBooking (CustomerID, ScreeningID, BookingDate, TotalCost, BookingStatus, LoginID) " &
                                     "VALUES (@CustomerID, @ScreeningID, @BookingDate, @TotalCost, @BookingStatus, @LoginID)"
                SQLCmd.Parameters.Clear()
                SQLCmd.Parameters.AddWithValue("@CustomerID", CInt(saleCustomerID))
                SQLCmd.Parameters.AddWithValue("@ScreeningID", CInt(screeningID))
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
                SQLCmd.CommandText = "SELECT TicketPrice FROM tblScreening WHERE ScreeningID = @ScreeningID"
                SQLCmd.Parameters.Clear()
                SQLCmd.Parameters.AddWithValue("@ScreeningID", CInt(screeningID))
                Dim priceResult = SQLCmd.ExecuteScalar()
                If priceResult IsNot Nothing AndAlso Not IsDBNull(priceResult) Then
                    ticketPrice = CDbl(priceResult)
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

        Return newID
    End Function

    Public Sub RecalculateBookingTotal(bookingID As Long)
        Dim ticketTotal As Double = 0
        Dim foodTotal As Double = 0

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn

            SQLCmd.CommandText = "SELECT SUM(SeatPricePaid) FROM tblBookingSeat WHERE BookingID = @BookingID"
            SQLCmd.Parameters.AddWithValue("@BookingID", CInt(bookingID))
            Dim ticketResult = SQLCmd.ExecuteScalar()
            If ticketResult IsNot Nothing AndAlso Not IsDBNull(ticketResult) Then
                ticketTotal = CDbl(ticketResult)
            End If

            SQLCmd.CommandText = "SELECT SUM(Quantity * ItemPricePaid) " &
                                 "FROM tblOrderItem " &
                                 "WHERE BookingID = @BookingID"
            SQLCmd.Parameters.Clear()
            SQLCmd.Parameters.AddWithValue("@BookingID", CInt(bookingID))
            Dim foodResult = SQLCmd.ExecuteScalar()
            If foodResult IsNot Nothing AndAlso Not IsDBNull(foodResult) Then
                foodTotal = CDbl(foodResult)
            End If

            SQLCmd.CommandText = "UPDATE tblBooking " &
                                 "SET TotalCost = @TotalCost " &
                                 "WHERE BookingID = @BookingID AND BookingStatus <> @Cancelled"
            SQLCmd.Parameters.Clear()
            SQLCmd.Parameters.AddWithValue("@TotalCost", ticketTotal + foodTotal)
            SQLCmd.Parameters.AddWithValue("@BookingID", CInt(bookingID))
            SQLCmd.Parameters.AddWithValue("@Cancelled", BookingCancelled)
            SQLCmd.ExecuteNonQuery()

            cn.Close()
        End If
    End Sub

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

End Module
