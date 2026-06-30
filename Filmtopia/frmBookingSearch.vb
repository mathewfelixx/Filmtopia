Imports System.Data.OleDb

Public Class frmBookingSearch
    Private Sub frmBookingSearch_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadBookings()
    End Sub

    Private Sub LoadBookings()
        Dim SQLCmd As New OleDbCommand("SELECT BookingID, CustomerName, BookingDateTime FROM tblBooking " &
                                       "ORDER BY BookingID DESC")
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
