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

                'the seats that were picked on the map.
                'the screening is written on each seat row as well as on the booking. it is the same
                'value twice, which is normally something to avoid, but Access can only make a rule
                'unique within one table. having it here lets the database itself refuse to sell the
                'same seat twice on the same screening instead of that only being a rule in the code
                Dim i As Integer
                For i = 0 To seatIDs.Length - 1
                    SQLCmd.CommandText = "INSERT INTO tblBookingSeat (BookingID, SeatID, ScreeningID) " &
                                         "VALUES (@BookingID, @SeatID, @ScreeningID)"
                    SQLCmd.Parameters.Clear()
                    SQLCmd.Parameters.AddWithValue("@BookingID", CInt(newID))
                    SQLCmd.Parameters.AddWithValue("@SeatID", CInt(seatIDs(i)))
                    SQLCmd.Parameters.AddWithValue("@ScreeningID", CInt(screeningID))
                    SQLCmd.ExecuteNonQuery()
                Next

                'the food that was added to the order while the sale was being built up
                Dim f As Integer
                For f = 0 To foodOrder.Rows.Count - 1
                    SQLCmd.CommandText = "INSERT INTO tblOrderItem (BookingID, FoodItemID, Quantity) " &
                                         "VALUES (@BookingID, @FoodItemID, @Quantity)"
                    SQLCmd.Parameters.Clear()
                    SQLCmd.Parameters.AddWithValue("@BookingID", CInt(newID))
                    SQLCmd.Parameters.AddWithValue("@FoodItemID", CInt(foodOrder.Rows(f)("FoodItemID")))
                    SQLCmd.Parameters.AddWithValue("@Quantity", CInt(foodOrder.Rows(f)("Quantity")))
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

    'makes a booking and its seats together. either both go in or neither does, so the database
    'can never end up with a booking that has no seats attached to it.
    'pass an empty array of seats for a food only sale.
    'returns the new BookingID, or 0 if it did not work
    Public Function CreateBookingWithSeats(customerID As Long, screeningID As Long, seatIDs() As Long) As Long
        Dim newID As Long = 0

        If DbConnect() Then
            Dim trans As OleDbTransaction = cn.BeginTransaction()

            Try
                Dim SQLCmd As New OleDbCommand
                SQLCmd.Connection = cn
                SQLCmd.Transaction = trans

                'the cost goes in as zero to start with, RecalculateBookingTotal sets it properly
                'once the seats are actually saved
                SQLCmd.CommandText = "INSERT INTO tblBooking (CustomerID, ScreeningID, BookingDate, TotalCost, BookingStatus) " &
                                     "VALUES (@CustomerID, @ScreeningID, @BookingDate, @TotalCost, @BookingStatus)"
                SQLCmd.Parameters.AddWithValue("@CustomerID", CInt(customerID))
                SQLCmd.Parameters.AddWithValue("@ScreeningID", CInt(screeningID))
                SQLCmd.Parameters.AddWithValue("@BookingDate", Date.Now.Date)
                SQLCmd.Parameters.AddWithValue("@TotalCost", 0)
                SQLCmd.Parameters.AddWithValue("@BookingStatus", BookingActive)
                SQLCmd.ExecuteNonQuery()

                'get the id the new booking was given so its seats can be linked to it
                SQLCmd.CommandText = "SELECT @@IDENTITY"
                SQLCmd.Parameters.Clear()
                newID = CLng(SQLCmd.ExecuteScalar())

                Dim i As Integer
                For i = 0 To seatIDs.Length - 1
                    SQLCmd.CommandText = "INSERT INTO tblBookingSeat (BookingID, SeatID, ScreeningID) " &
                                         "VALUES (@BookingID, @SeatID, @ScreeningID)"
                    SQLCmd.Parameters.Clear()
                    SQLCmd.Parameters.AddWithValue("@BookingID", CInt(newID))
                    SQLCmd.Parameters.AddWithValue("@SeatID", CInt(seatIDs(i)))
                    SQLCmd.Parameters.AddWithValue("@ScreeningID", CInt(screeningID))
                    SQLCmd.ExecuteNonQuery()
                Next

                trans.Commit()

            Catch ex As Exception
                trans.Rollback()
                newID = 0
                MessageBox.Show("The booking could not be made, so nothing was saved. " & ex.Message,
                                "Booking", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try

            cn.Close()
        End If

        'work the money out from what actually got saved, not from what was on screen
        If newID > 0 Then
            RecalculateBookingTotal(newID)
        End If

        Return newID
    End Function

    'works out what a booking costs and saves it onto the booking.
    'the total is always the seats times the ticket price, plus whatever food has been ordered.
    'this is the only place TotalCost is ever set, so it cannot drift out of step any more
    Public Sub RecalculateBookingTotal(bookingID As Long)
        Dim ticketPrice As Double = 0
        Dim ticketTotal As Double = 0
        Dim foodTotal As Double = 0

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn

            'what one standard ticket costs for the screening this booking is for
            SQLCmd.CommandText = "SELECT TicketPrice " &
                                 "FROM tblScreening INNER JOIN tblBooking ON tblScreening.ScreeningID = tblBooking.ScreeningID " &
                                 "WHERE tblBooking.BookingID = @BookingID"
            SQLCmd.Parameters.AddWithValue("@BookingID", CInt(bookingID))
            Dim priceResult = SQLCmd.ExecuteScalar()
            If priceResult IsNot Nothing AndAlso Not IsDBNull(priceResult) Then
                ticketPrice = CDbl(priceResult)
            End If

            'the seats are added up one at a time rather than being counted, because they are not
            'all worth the same any more. each seat costs the screening price times the multiplier
            'for whatever sort of seat it is
            SQLCmd.CommandText = "SELECT tblSeatType.PriceMultiplier " &
                                 "FROM (tblBookingSeat INNER JOIN tblSeat ON tblBookingSeat.SeatID = tblSeat.SeatID) " &
                                 "INNER JOIN tblSeatType ON tblSeat.SeatTypeID = tblSeatType.SeatTypeID " &
                                 "WHERE tblBookingSeat.BookingID = @BookingID"
            SQLCmd.Parameters.Clear()
            SQLCmd.Parameters.AddWithValue("@BookingID", CInt(bookingID))
            Dim rs As OleDbDataReader = SQLCmd.ExecuteReader()
            While rs.Read()
                ticketTotal = ticketTotal + SeatPrice(ticketPrice, CDbl(rs("PriceMultiplier")))
            End While
            rs.Close()

            'what the food comes to, this comes back empty if nothing has been ordered
            SQLCmd.CommandText = "SELECT SUM(Quantity * FoodItemPrice) " &
                                 "FROM tblOrderItem INNER JOIN tblFoodItem ON tblOrderItem.FoodItemID = tblFoodItem.FoodItemID " &
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
