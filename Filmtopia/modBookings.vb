Imports System.Data.OleDb

Module modBookings
    Public ReadOnly FreeSeatColour As Color = Color.FromArgb(220, 220, 220)
    Public ReadOnly ChosenSeatColour As Color = Color.Fuchsia
    Public ReadOnly BookedSeatColour As Color = Color.FromArgb(255, 192, 255)

    Public Function GetSeats(screenID As Long) As DataTable
        Dim SQLCmd As New OleDbCommand("SELECT SeatID, SeatRow, SeatNumber FROM tblSeat WHERE ScreenID = @ScreenID ORDER BY SeatRow, SeatNumber")
        SQLCmd.Parameters.AddWithValue("@ScreenID", screenID)
        Dim dt As New DataTable
        FillTable(SQLCmd, dt)
        Return dt
    End Function

    Public Function GetBookedSeats(screeningID As Long) As DataTable
        Dim SQLCmd As New OleDbCommand("SELECT SeatID FROM tblBookingSeat INNER JOIN tblBooking ON tblBookingSeat.BookingID = tblBooking.BookingID " &
                                       "WHERE ScreeningID = @ScreeningID")
        SQLCmd.Parameters.AddWithValue("@ScreeningID", screeningID)
        Dim dt As New DataTable
        FillTable(SQLCmd, dt)
        Return dt
    End Function

    Public Function IsBooked(seatID As Long, dtBooked As DataTable) As Boolean
        Dim booked As Boolean = False
        For Each bookedSeat As DataRow In dtBooked.Rows
            If bookedSeat("SeatID") = seatID Then
                booked = True
            End If
        Next
        Return booked
    End Function
End Module
