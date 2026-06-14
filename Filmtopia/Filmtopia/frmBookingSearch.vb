Imports System.Data.OleDb

Public Class frmBookingSearch

    'the booking currently selected in the grid, 0 means nothing selected
    Private selectedBookingID As Integer = 0

    Private Sub frmBookingSearch_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CommonFormStartup()
        LoadBookings("")
        WriteLog("BOOKING", "Booking search form opened")
    End Sub

    'searches by booking id if a number was typed, otherwise by customer name
    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        LoadBookings(txtSearch.Text.Trim())
        WriteLog("BOOKING", "Booking search run for '" & txtSearch.Text.Trim() & "'")
    End Sub

    'clears the search box and shows every booking
    Private Sub btnShowAll_Click(sender As Object, e As EventArgs) Handles btnShowAll.Click
        txtSearch.Text = ""
        LoadBookings("")
    End Sub

    'loads the bookings grid, filtered by booking id or customer name if given
    Private Sub LoadBookings(searchText As String)
        Dim dt As New DataTable

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn

            Dim baseQuery As String = "SELECT tblBooking.BookingID, CustomerForename & ' ' & CustomerSurname AS CustomerName, FilmTitle, ScreeningDate, ScreeningTime, TotalCost FROM ((tblBooking INNER JOIN tblCustomer ON tblBooking.CustomerID = tblCustomer.CustomerID) INNER JOIN tblScreening ON tblBooking.ScreeningID = tblScreening.ScreeningID) INNER JOIN tblFilm ON tblScreening.FilmID = tblFilm.FilmID"

            If searchText = "" Then
                SQLCmd.CommandText = baseQuery
            ElseIf IsNumeric(searchText) Then
                SQLCmd.CommandText = baseQuery & " WHERE tblBooking.BookingID = @SearchID"
                SQLCmd.Parameters.AddWithValue("@SearchID", CInt(searchText))
            Else
                SQLCmd.CommandText = baseQuery & " WHERE CustomerForename & ' ' & CustomerSurname LIKE @SearchName"
                SQLCmd.Parameters.AddWithValue("@SearchName", "%" & searchText & "%")
            End If

            Dim da As New OleDbDataAdapter(SQLCmd)
            da.Fill(dt)
            cn.Close()
        End If

        dgvBookings.DataSource = dt

        If dgvBookings.Columns.Count > 0 Then
            dgvBookings.Columns("FilmTitle").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            dgvBookings.Columns("TotalCost").DefaultCellStyle.Format = "C"
        End If

        selectedBookingID = 0
    End Sub

    'remembers which booking was clicked so it can be cancelled
    Private Sub dgvBookings_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvBookings.CellClick
        If e.RowIndex < 0 Then Exit Sub

        Dim row As DataGridViewRow = dgvBookings.Rows(e.RowIndex)
        selectedBookingID = CInt(row.Cells("BookingID").Value)
    End Sub

    'cancels the selected booking, removing its seats, food order and the booking itself
    Private Sub btnCancelBooking_Click(sender As Object, e As EventArgs) Handles btnCancelBooking.Click
        If selectedBookingID = 0 Then
            MessageBox.Show("Select a booking in the grid first")
            Exit Sub
        End If

        If MessageBox.Show("Cancel this booking? This will free its seats and remove any food order.", "Confirm", MessageBoxButtons.YesNo) = DialogResult.No Then
            Exit Sub
        End If

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn

            'remove the seats held for this booking
            SQLCmd.CommandText = "DELETE FROM tblBookingSeat WHERE BookingID = @BookingID"
            SQLCmd.Parameters.AddWithValue("@BookingID", selectedBookingID)
            SQLCmd.ExecuteNonQuery()

            'remove any food and drink ordered for this booking
            SQLCmd.CommandText = "DELETE FROM tblOrderItem WHERE BookingID = @BookingID"
            SQLCmd.Parameters.Clear()
            SQLCmd.Parameters.AddWithValue("@BookingID", selectedBookingID)
            SQLCmd.ExecuteNonQuery()

            'finally remove the booking itself
            SQLCmd.CommandText = "DELETE FROM tblBooking WHERE BookingID = @BookingID"
            SQLCmd.Parameters.Clear()
            SQLCmd.Parameters.AddWithValue("@BookingID", selectedBookingID)
            SQLCmd.ExecuteNonQuery()

            cn.Close()
        End If

        WriteLog("BOOKING", "Booking " & selectedBookingID & " cancelled")
        MessageBox.Show("Booking cancelled")
        LoadBookings(txtSearch.Text.Trim())
    End Sub

End Class
