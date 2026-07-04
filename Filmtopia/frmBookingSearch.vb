Imports System.Data.OleDb

Public Class frmBookingSearch
    Private selectedBookingID As Long = 0

    Private Sub frmBookingSearch_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadBookings("")
    End Sub

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        Dim searchText As String = txtSearch.Text.Trim()
        LoadBookings(searchText)
        If searchText <> "" And dgvBookings.Rows.Count = 0 Then
            MessageBox.Show("No bookings were found for " & searchText & ".", "No Bookings Found", MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtSearch.Focus()
        End If
    End Sub

    Private Sub btnShowAll_Click(sender As Object, e As EventArgs) Handles btnShowAll.Click
        txtSearch.Text = ""
        LoadBookings("")
        txtSearch.Focus()
    End Sub

    Private Sub LoadBookings(searchText As String)
        Dim bookingNumber As Long = 0
        Long.TryParse(searchText, bookingNumber)

        Dim SQLCmd As New OleDbCommand("SELECT BookingID, CustomerName, BookingDateTime FROM tblBooking " &
                                       "WHERE InStr(CustomerName, @SearchText) > 0 OR BookingID = @BookingID " &
                                       "ORDER BY BookingID DESC")
        SQLCmd.Parameters.AddWithValue("@SearchText", searchText)
        SQLCmd.Parameters.AddWithValue("@BookingID", bookingNumber)
        Dim dt As New DataTable
        If FillTable(SQLCmd, dt) Then
            dgvBookings.DataSource = dt
            dgvBookings.Columns("BookingID").HeaderText = "Booking No"
            dgvBookings.Columns("CustomerName").HeaderText = "Customer"
            dgvBookings.Columns("BookingDateTime").HeaderText = "Booked"
            dgvBookings.Columns("CustomerName").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            dgvBookings.Columns("BookingDateTime").DefaultCellStyle.Format = "dd/MM/yyyy HH:mm"
        End If
        selectedBookingID = 0
        lstDetails.Items.Clear()
    End Sub

    Private Sub dgvBookings_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs) Handles dgvBookings.DataBindingComplete
        dgvBookings.ClearSelection()
    End Sub

    Private Sub dgvBookings_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvBookings.CellClick
        If e.RowIndex >= 0 Then
            Dim row As DataGridViewRow = dgvBookings.Rows(e.RowIndex)
            selectedBookingID = row.Cells("BookingID").Value
            ShowDetails(row)
        End If
    End Sub

    Private Sub ShowDetails(row As DataGridViewRow)
        Dim booked As Date = row.Cells("BookingDateTime").Value
        lstDetails.Items.Clear()
        lstDetails.Items.Add("Booking number " & selectedBookingID & " for " & row.Cells("CustomerName").Value)
        lstDetails.Items.Add("Booked on " & booked.ToString("dd/MM/yyyy") & " at " & booked.ToString("HH:mm"))

        Dim SQLCmd As New OleDbCommand("SELECT FilmTitle, ScreenName, ScreeningDate, ScreeningTime " &
                                       "FROM ((tblBooking INNER JOIN tblScreening ON tblBooking.ScreeningID = tblScreening.ScreeningID) " &
                                       "INNER JOIN tblFilm ON tblScreening.FilmID = tblFilm.FilmID) " &
                                       "INNER JOIN tblScreen ON tblScreening.ScreenID = tblScreen.ScreenID " &
                                       "WHERE BookingID = @BookingID")
        SQLCmd.Parameters.AddWithValue("@BookingID", selectedBookingID)
        Dim dtScreening As New DataTable
        FillTable(SQLCmd, dtScreening)
        If dtScreening.Rows.Count = 0 Then
            lstDetails.Items.Add("Food only sale, no film or seats")
        Else
            Dim screening As DataRow = dtScreening.Rows(0)
            lstDetails.Items.Add("Film: " & screening("FilmTitle"))
            lstDetails.Items.Add("Screening: " & screening("ScreenName") & " on " & CDate(screening("ScreeningDate")).ToString("dd/MM/yyyy") & " at " & screening("ScreeningTime"))
        End If

        Dim total As Decimal = 0

        SQLCmd = New OleDbCommand("SELECT SeatRow, SeatNumber, SeatPricePaid " &
                                  "FROM tblBookingSeat INNER JOIN tblSeat ON tblBookingSeat.SeatID = tblSeat.SeatID " &
                                  "WHERE BookingID = @BookingID ORDER BY SeatRow, SeatNumber")
        SQLCmd.Parameters.AddWithValue("@BookingID", selectedBookingID)
        Dim dtSeats As New DataTable
        FillTable(SQLCmd, dtSeats)
        For Each seat As DataRow In dtSeats.Rows
            Dim seatPrice As Decimal = seat("SeatPricePaid")
            lstDetails.Items.Add("Seat " & seat("SeatRow") & seat("SeatNumber") & "   " & seatPrice.ToString("C2"))
            total = total + seatPrice
        Next

        SQLCmd = New OleDbCommand("SELECT Quantity, FoodName, ItemPricePaid " &
                                  "FROM tblOrderItem INNER JOIN tblFoodItem ON tblOrderItem.FoodItemID = tblFoodItem.FoodItemID " &
                                  "WHERE BookingID = @BookingID ORDER BY OrderItemID")
        SQLCmd.Parameters.AddWithValue("@BookingID", selectedBookingID)
        Dim dtFood As New DataTable
        FillTable(SQLCmd, dtFood)
        If dtFood.Rows.Count = 0 Then
            lstDetails.Items.Add("No food or drink")
        Else
            For Each food As DataRow In dtFood.Rows
                Dim lineTotal As Decimal = food("Quantity") * food("ItemPricePaid")
                lstDetails.Items.Add(food("Quantity") & " x " & food("FoodName") & "   " & lineTotal.ToString("C2"))
                total = total + lineTotal
            Next
        End If

        lstDetails.Items.Add("Total: " & total.ToString("C2"))
    End Sub
End Class
