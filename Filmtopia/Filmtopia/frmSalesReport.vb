Imports System.Data.OleDb

Public Class frmSalesReport

    Private Sub frmSalesReport_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CommonFormStartup()

        'default the date range to the start of this month through to today
        dtpFrom.Value = New Date(Date.Now.Year, Date.Now.Month, 1)
        dtpTo.Value = Date.Now.Date

        RunReport()
        WriteLog("REPORT", "Sales report form opened")
    End Sub

    'runs the whole report for the date range picked
    Private Sub btnRunReport_Click(sender As Object, e As EventArgs) Handles btnRunReport.Click
        RunReport()
        WriteLog("REPORT", "Sales report run for " & dtpFrom.Value.ToShortDateString() & " to " & dtpTo.Value.ToShortDateString())
    End Sub

    'loads the ticket sales grid and the revenue totals for the picked date range
    Private Sub RunReport()
        Dim fromDate As Date = dtpFrom.Value.Date
        Dim toDate As Date = dtpTo.Value.Date

        Dim ticketRevenue As Double = LoadSalesByFilm(fromDate, toDate)
        Dim foodRevenue As Double = GetFoodRevenue(fromDate, toDate)

        lblTicketRevenue.Text = "Ticket revenue: " & FormatCurrency(ticketRevenue)
        lblFoodRevenue.Text = "Food revenue: " & FormatCurrency(foodRevenue)
        lblGrandTotal.Text = "Grand total: " & FormatCurrency(ticketRevenue + foodRevenue)
    End Sub

    'fills the grid with bookings and ticket revenue per film, returns the total ticket revenue
    Private Function LoadSalesByFilm(fromDate As Date, toDate As Date) As Double
        Dim dt As New DataTable

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            'join booking to screening, then to film (for the title), and group by film to total up revenue
            SQLCmd.CommandText = "SELECT FilmTitle, COUNT(tblBooking.BookingID) AS Bookings, SUM(TotalCost) AS TicketRevenue FROM (tblBooking INNER JOIN tblScreening ON tblBooking.ScreeningID = tblScreening.ScreeningID) INNER JOIN tblFilm ON tblScreening.FilmID = tblFilm.FilmID WHERE tblBooking.BookingDate BETWEEN @FromDate AND @ToDate GROUP BY FilmTitle"
            SQLCmd.Parameters.AddWithValue("@FromDate", fromDate)
            SQLCmd.Parameters.AddWithValue("@ToDate", toDate)
            Dim da As New OleDbDataAdapter(SQLCmd)
            da.Fill(dt)
            cn.Close()
        End If

        dgvSalesByFilm.DataSource = dt

        If dgvSalesByFilm.Columns.Count > 0 Then
            dgvSalesByFilm.Columns("FilmTitle").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            dgvSalesByFilm.Columns("TicketRevenue").DefaultCellStyle.Format = "C"
        End If

        'add up the ticket revenue column for the overall total
        Dim total As Double = 0
        For Each row As DataRow In dt.Rows
            If Not IsDBNull(row("TicketRevenue")) Then
                total = total + CDbl(row("TicketRevenue"))
            End If
        Next
        Return total
    End Function

    'works out the total food and drink revenue for the picked date range
    Private Function GetFoodRevenue(fromDate As Date, toDate As Date) As Double
        Dim total As Double = 0

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            'join order item to food item (for the price), then to booking (for the date), and total it up
            SQLCmd.CommandText = "SELECT SUM(FoodItemPrice * Quantity) FROM (tblOrderItem INNER JOIN tblFoodItem ON tblOrderItem.FoodItemID = tblFoodItem.FoodItemID) INNER JOIN tblBooking ON tblOrderItem.BookingID = tblBooking.BookingID WHERE tblBooking.BookingDate BETWEEN @FromDate AND @ToDate"
            SQLCmd.Parameters.AddWithValue("@FromDate", fromDate)
            SQLCmd.Parameters.AddWithValue("@ToDate", toDate)
            Dim result = SQLCmd.ExecuteScalar()
            If Not IsDBNull(result) Then
                total = CDbl(result)
            End If
            cn.Close()
        End If

        Return total
    End Function

End Class
