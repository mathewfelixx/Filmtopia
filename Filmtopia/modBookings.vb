Imports System.Data.OleDb

Module modBookings
    Public ReadOnly FreeSeatColour As Color = Color.FromArgb(220, 220, 220)
    Public ReadOnly ChosenSeatColour As Color = Color.Fuchsia

    Public Function GetSeats(screenID As Long) As DataTable
        Dim SQLCmd As New OleDbCommand("SELECT SeatID, SeatRow, SeatNumber FROM tblSeat WHERE ScreenID = @ScreenID ORDER BY SeatRow, SeatNumber")
        SQLCmd.Parameters.AddWithValue("@ScreenID", screenID)
        Dim dt As New DataTable
        FillTable(SQLCmd, dt)
        Return dt
    End Function
End Module
