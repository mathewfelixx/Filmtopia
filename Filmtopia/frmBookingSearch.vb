Imports System.Data.OleDb

Public Class frmBookingSearch
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
    End Sub

    Private Sub dgvBookings_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs) Handles dgvBookings.DataBindingComplete
        dgvBookings.ClearSelection()
    End Sub
End Class
