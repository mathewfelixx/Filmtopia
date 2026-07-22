Imports System.Data.OleDb

Public Class frmSalesReport
    Private Sub frmSalesReport_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        dtpFrom.Value = New Date(Date.Today.Year, Date.Today.Month, 1)
        dtpTo.Value = Date.Today
        ShowReport()
    End Sub

    Private Sub btnShowReport_Click(sender As Object, e As EventArgs) Handles btnShowReport.Click
        If dtpTo.Value.Date < dtpFrom.Value.Date Then
            MessageBox.Show("The To date cannot be before the From date.", "Invalid Dates", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            dtpTo.Focus()
        Else
            ShowReport()
        End If
    End Sub

    Private Sub ShowReport()
        Dim fromDate As Date = dtpFrom.Value.Date
        Dim dayAfterTo As Date = dtpTo.Value.Date.AddDays(1)

        Dim SQLCmd As New OleDbCommand("SELECT FilmTitle, COUNT(*) AS Tickets, SUM(SeatPricePaid) AS TicketRevenue " &
                                       "FROM ((tblBookingSeat INNER JOIN tblBooking ON tblBookingSeat.BookingID = tblBooking.BookingID) " &
                                       "INNER JOIN tblScreening ON tblBooking.ScreeningID = tblScreening.ScreeningID) " &
                                       "INNER JOIN tblFilm ON tblScreening.FilmID = tblFilm.FilmID " &
                                       "WHERE BookingCancelled = False AND BookingDateTime >= @FromDate AND BookingDateTime < @DayAfterTo " &
                                       "GROUP BY FilmTitle " &
                                       "ORDER BY SUM(SeatPricePaid) DESC, FilmTitle")
        SQLCmd.Parameters.AddWithValue("@FromDate", fromDate)
        SQLCmd.Parameters.AddWithValue("@DayAfterTo", dayAfterTo)
        Dim dt As New DataTable
        If FillTable(SQLCmd, dt) Then
            dgvReport.DataSource = dt
            dgvReport.Columns("FilmTitle").HeaderText = "Film"
            dgvReport.Columns("TicketRevenue").HeaderText = "Ticket Revenue"
            dgvReport.Columns("FilmTitle").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            dgvReport.Columns("TicketRevenue").DefaultCellStyle.Format = "C2"

            Dim tickets As Integer = 0
            Dim ticketRevenue As Decimal = 0
            For Each film As DataRow In dt.Rows
                tickets = tickets + film("Tickets")
                ticketRevenue = ticketRevenue + film("TicketRevenue")
            Next

            Dim foodRevenue As Decimal = GetFoodRevenue(fromDate, dayAfterTo)

            lblTickets.Text = "Tickets sold: " & tickets
            lblTicketRevenue.Text = "Ticket revenue: " & ticketRevenue.ToString("C2")
            lblFoodRevenue.Text = "Food and drink revenue: " & foodRevenue.ToString("C2")
            lblTotalRevenue.Text = "Total revenue: " & (ticketRevenue + foodRevenue).ToString("C2")
        End If
    End Sub

    Private Function GetFoodRevenue(fromDate As Date, dayAfterTo As Date) As Decimal
        Dim SQLCmd As New OleDbCommand("SELECT SUM(Quantity * ItemPricePaid) " &
                                       "FROM tblOrderItem INNER JOIN tblBooking ON tblOrderItem.BookingID = tblBooking.BookingID " &
                                       "WHERE BookingCancelled = False AND BookingDateTime >= @FromDate AND BookingDateTime < @DayAfterTo")
        SQLCmd.Parameters.AddWithValue("@FromDate", fromDate)
        SQLCmd.Parameters.AddWithValue("@DayAfterTo", dayAfterTo)

        Dim revenue As Decimal = 0
        Dim result As Object = GetValue(SQLCmd)
        If Not IsDBNull(result) Then
            revenue = result
        End If
        Return revenue
    End Function

    Private Sub dgvReport_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs) Handles dgvReport.DataBindingComplete
        dgvReport.ClearSelection()
    End Sub
End Class
