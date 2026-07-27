Imports System.Data.OleDb

'all the rules about what a booking is are kept in here rather than being spread across the
'forms. before this, the booking form and the food form each worked the money out in their own
'way and they did not agree with each other. now there is one routine that decides what a
'booking costs, and everything else calls it
Module modBookings

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

                'the booking itself, with the total that was on the screen
                SQLCmd.CommandText = "INSERT INTO tblBooking (CustomerID, ScreeningID, BookingDate, TotalCost) " &
                                     "VALUES (@CustomerID, @ScreeningID, @BookingDate, @TotalCost)"
                SQLCmd.Parameters.Clear()
                SQLCmd.Parameters.AddWithValue("@CustomerID", CInt(saleCustomerID))
                SQLCmd.Parameters.AddWithValue("@ScreeningID", CInt(screeningID))
                SQLCmd.Parameters.AddWithValue("@BookingDate", Date.Now.Date)
                SQLCmd.Parameters.AddWithValue("@TotalCost", totalCost)
                SQLCmd.ExecuteNonQuery()

                SQLCmd.CommandText = "SELECT @@IDENTITY"
                SQLCmd.Parameters.Clear()
                newID = CLng(SQLCmd.ExecuteScalar())

                'the seats that were picked on the map
                Dim i As Integer
                For i = 0 To seatIDs.Length - 1
                    SQLCmd.CommandText = "INSERT INTO tblBookingSeat (BookingID, SeatID) " &
                                         "VALUES (@BookingID, @SeatID)"
                    SQLCmd.Parameters.Clear()
                    SQLCmd.Parameters.AddWithValue("@BookingID", CInt(newID))
                    SQLCmd.Parameters.AddWithValue("@SeatID", CInt(seatIDs(i)))
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
                SQLCmd.CommandText = "INSERT INTO tblBooking (CustomerID, ScreeningID, BookingDate, TotalCost) " &
                                     "VALUES (@CustomerID, @ScreeningID, @BookingDate, @TotalCost)"
                SQLCmd.Parameters.AddWithValue("@CustomerID", CInt(customerID))
                SQLCmd.Parameters.AddWithValue("@ScreeningID", CInt(screeningID))
                SQLCmd.Parameters.AddWithValue("@BookingDate", Date.Now.Date)
                SQLCmd.Parameters.AddWithValue("@TotalCost", 0)
                SQLCmd.ExecuteNonQuery()

                'get the id the new booking was given so its seats can be linked to it
                SQLCmd.CommandText = "SELECT @@IDENTITY"
                SQLCmd.Parameters.Clear()
                newID = CLng(SQLCmd.ExecuteScalar())

                Dim i As Integer
                For i = 0 To seatIDs.Length - 1
                    SQLCmd.CommandText = "INSERT INTO tblBookingSeat (BookingID, SeatID) " &
                                         "VALUES (@BookingID, @SeatID)"
                    SQLCmd.Parameters.Clear()
                    SQLCmd.Parameters.AddWithValue("@BookingID", CInt(newID))
                    SQLCmd.Parameters.AddWithValue("@SeatID", CInt(seatIDs(i)))
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
        Dim seats As Integer = 0
        Dim ticketPrice As Double = 0
        Dim foodTotal As Double = 0

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn

            'how many seats are on this booking
            SQLCmd.CommandText = "SELECT COUNT(*) FROM tblBookingSeat " &
                                 "WHERE BookingID = @BookingID"
            SQLCmd.Parameters.AddWithValue("@BookingID", CInt(bookingID))
            seats = CInt(SQLCmd.ExecuteScalar())

            'what one ticket costs for the screening this booking is for
            SQLCmd.CommandText = "SELECT TicketPrice " &
                                 "FROM tblScreening INNER JOIN tblBooking ON tblScreening.ScreeningID = tblBooking.ScreeningID " &
                                 "WHERE tblBooking.BookingID = @BookingID"
            SQLCmd.Parameters.Clear()
            SQLCmd.Parameters.AddWithValue("@BookingID", CInt(bookingID))
            Dim priceResult = SQLCmd.ExecuteScalar()
            If priceResult IsNot Nothing AndAlso Not IsDBNull(priceResult) Then
                ticketPrice = CDbl(priceResult)
            End If

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

            SQLCmd.CommandText = "UPDATE tblBooking " &
                                 "SET TotalCost = @TotalCost " &
                                 "WHERE BookingID = @BookingID"
            SQLCmd.Parameters.Clear()
            SQLCmd.Parameters.AddWithValue("@TotalCost", (seats * ticketPrice) + foodTotal)
            SQLCmd.Parameters.AddWithValue("@BookingID", CInt(bookingID))
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
