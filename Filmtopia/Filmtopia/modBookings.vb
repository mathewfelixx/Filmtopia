Imports System.Data.OleDb

'all the rules about what a booking is are kept in here rather than being spread across the
'forms. before this, the booking form and the food form each worked the money out in their own
'way and they did not agree with each other. now there is one routine that decides what a
'booking costs, and everything else calls it
Module modBookings

    'the two states a booking can be in. they are only ever used as text so they are kept as
    'constants the same way the log severities are, that way a typo is a compile error rather than
    'a booking that quietly does not match anything.
    'a cancelled booking is not deleted any more, it stays in the table with its total on it so the
    'money history is still right and a refund can be seen. what does get removed is its seats,
    'because those have to go back on sale for somebody else
    Public Const BookingActive As String = "Active"
    Public Const BookingCancelled As String = "Cancelled"

    'the three sorts of seat. the names match the SeatTypeName column in tblSeatType, and what each
    'one costs is not in here on purpose, it is a PriceMultiplier held on the row in that table.
    'that way the price of a premium seat can be changed in the data without touching any code
    Public Const SeatStandard As String = "Standard"
    Public Const SeatPremium As String = "Premium"
    Public Const SeatAccessible As String = "Accessible"

    'what one seat costs for a screening, which is the screening's base price times whatever the
    'multiplier is for that sort of seat. every place that works out a price goes through here so
    'the till, the total on the booking and the sales report cannot disagree with each other
    Public Function SeatPrice(basePrice As Double, multiplier As Double) As Double
        Return Math.Round(basePrice * multiplier, 2)
    End Function

    'saves a whole sale in one go: the walk-in customer if there is one, the booking, the seats
    'and the food. it is all inside one transaction, so either the entire sale is saved or none
    'of it is. nothing is written until the till operator finishes the sale, which is why there
    'can no longer be half finished bookings or customers left lying about in the database.
    'returns the new BookingID, or 0 if it did not work
    Public Function CompleteSale(customerID As Long, isWalkIn As Boolean, screeningID As Long,
                                 seatIDs() As Long, foodOrder As DataTable, totalCost As Double) As Long
        Dim newID As Long = 0

        If DbConnect() Then
            Dim trans As OleDbTransaction = cn.BeginTransaction()

            Try
                Dim SQLCmd As New OleDbCommand
                SQLCmd.Connection = cn
                SQLCmd.Transaction = trans

                'before anything at all is written, check every seat that was picked is still free
                'on this screening. the bookings form checks this too, but that check finishes and
                'closes its connection before this one starts, so on a second till a seat could be
                'taken in between the two. doing it in here, inside the transaction, means the sale
                'is either made on seats that were free or it is not made at all
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

                'a walk-in has no details, so a quick customer record is made for them here.
                'doing it inside the transaction means an abandoned sale cannot leave one behind
                Dim saleCustomerID As Long = customerID

                If isWalkIn Then
                    SQLCmd.CommandText = "INSERT INTO tblCustomer (CustomerForename, CustomerSurname, CustomerEmail, CustomerPhone) " &
                                         "VALUES (@CustomerForename, @CustomerSurname, @CustomerEmail, @CustomerPhone)"
                    'the seat check above leaves its two parameters on the command, and OleDb fills
                    'the placeholders in the order they were added rather than by name, so without
                    'this the screening id and the seat id went in as the name
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

                'the booking itself, with the total that was on the screen. a new sale always starts
                'off active, it only becomes cancelled if somebody cancels it later
                SQLCmd.CommandText = "INSERT INTO tblBooking (CustomerID, ScreeningID, BookingDate, TotalCost, BookingStatus) " &
                                     "VALUES (@CustomerID, @ScreeningID, @BookingDate, @TotalCost, @BookingStatus)"
                SQLCmd.Parameters.Clear()
                SQLCmd.Parameters.AddWithValue("@CustomerID", CInt(saleCustomerID))
                SQLCmd.Parameters.AddWithValue("@ScreeningID", CInt(screeningID))
                SQLCmd.Parameters.AddWithValue("@BookingDate", Date.Now.Date)
                SQLCmd.Parameters.AddWithValue("@TotalCost", totalCost)
                SQLCmd.Parameters.AddWithValue("@BookingStatus", BookingActive)
                SQLCmd.ExecuteNonQuery()

                SQLCmd.CommandText = "SELECT @@IDENTITY"
                SQLCmd.Parameters.Clear()
                newID = CLng(SQLCmd.ExecuteScalar())

                'what a standard ticket costs for this screening right now. this is read once, here,
                'because what matters is the price at the moment the sale is made
                Dim ticketPrice As Double = 0
                SQLCmd.CommandText = "SELECT TicketPrice FROM tblScreening WHERE ScreeningID = @ScreeningID"
                SQLCmd.Parameters.Clear()
                SQLCmd.Parameters.AddWithValue("@ScreeningID", CInt(screeningID))
                Dim priceResult = SQLCmd.ExecuteScalar()
                If priceResult IsNot Nothing AndAlso Not IsDBNull(priceResult) Then
                    ticketPrice = CDbl(priceResult)
                End If

                'the seats that were picked on the map.
                'the screening is written on each seat row as well as on the booking. it is the same
                'value twice, which is normally something to avoid, but Access can only make a rule
                'unique within one table. having it here lets the database itself refuse to sell the
                'same seat twice on the same screening instead of that only being a rule in the code.
                'what the seat was actually charged at goes on the row as well. a booking is the price
                'it was agreed at, so if the ticket price goes up next week this sale must not change
                Dim i As Integer
                For i = 0 To seatIDs.Length - 1
                    'the multiplier for this particular seat, so a premium one is charged as premium
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

                'the food that was added to the order while the sale was being built up. the price
                'is written onto the line the same way the seat price is written onto the seat, so
                'putting the price of a drink up next month cannot change what this customer paid
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

    'works out what a booking costs and saves it onto the booking.
    'both halves are whatever they were actually charged at when the sale was made, the tickets off
    'the seat rows and the food off the order lines, and neither is ever worked out again from
    'today's prices. the food total is still added up fresh every time because lines can be added to
    'an order or taken off it after the sale, but a price change no longer moves it.
    'this is the only place TotalCost is set
    Public Sub RecalculateBookingTotal(bookingID As Long)
        Dim ticketTotal As Double = 0
        Dim foodTotal As Double = 0

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn

            'what the seats on this booking were sold for. putting the price up on the screening
            'later must not change what a customer who already paid is down as having paid
            SQLCmd.CommandText = "SELECT SUM(SeatPricePaid) FROM tblBookingSeat WHERE BookingID = @BookingID"
            SQLCmd.Parameters.AddWithValue("@BookingID", CInt(bookingID))
            Dim ticketResult = SQLCmd.ExecuteScalar()
            If ticketResult IsNot Nothing AndAlso Not IsDBNull(ticketResult) Then
                ticketTotal = CDbl(ticketResult)
            End If

            'what the food comes to, this comes back empty if nothing has been ordered. the price is
            'on the order line itself now so this no longer has to go anywhere near tblFoodItem
            SQLCmd.CommandText = "SELECT SUM(Quantity * ItemPricePaid) " &
                                 "FROM tblOrderItem " &
                                 "WHERE BookingID = @BookingID"
            SQLCmd.Parameters.Clear()
            SQLCmd.Parameters.AddWithValue("@BookingID", CInt(bookingID))
            Dim foodResult = SQLCmd.ExecuteScalar()
            If foodResult IsNot Nothing AndAlso Not IsDBNull(foodResult) Then
                foodTotal = CDbl(foodResult)
            End If

            'a cancelled booking is left alone. its seats have been taken off it, so working the
            'total out again would drop it to just the food and the refund on record would be wrong
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

    'goes through every booking and works its total out again. this is a tidy up job for bookings
    'that were made before the food was being counted, so their totals were too low
    Public Function RecalculateAllBookingTotals() As Integer
        Dim bookingIDs(-1) As Long
        Dim howMany As Integer = 0

        'read all the ids first, because the connection is needed again for each one
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
