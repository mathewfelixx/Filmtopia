Imports System.Data.OleDb

Public Class frmSalesReport

    'true while the form is setting itself up, so filling the show box does not run the report
    'before the dates have been put in
    Private stillLoading As Boolean = True

    Private Sub frmSalesReport_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If UserAccessLevel <> 1 Then
            MessageBox.Show("Only a manager can see the sales report.", "Sales Report", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            WriteLog("REPORT", "Sales report refused, access level " & UserAccessLevel, LogSecurity)
            Me.Close()
            Exit Sub
        End If

        CommonFormStartup(Me)

        'default the date range to the start of this month through to today
        dtpFrom.Value = New Date(Date.Now.Year, Date.Now.Month, 1)
        dtpTo.Value = Date.Now.Date

        'the ways the report can be run, both together is the one it starts on
        cboReportType.Items.Add("Tickets and concessions")
        cboReportType.Items.Add("Tickets only")
        cboReportType.Items.Add("Concessions only")
        cboReportType.Items.Add("By screening")
        cboReportType.Items.Add("By screen")
        cboReportType.Items.Add("Cancellations")
        cboReportType.SelectedIndex = 0

        'which date the range is matched against. booking date is when the money came in,
        'screening date is what actually played that week. they are different questions and the
        'same range can give two different answers, so it is worth being able to pick
        cboMeasureBy.Items.Add("Booking date")
        cboMeasureBy.Items.Add("Screening date")
        cboMeasureBy.SelectedIndex = 0

        stillLoading = False

        RunReport()
        WriteLog("REPORT", "Sales report form opened")
    End Sub

    'picking a different option runs the report again straight away, so nobody has to press run
    Private Sub cboReportType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboReportType.SelectedIndexChanged
        If stillLoading Then
            Exit Sub
        End If

        RunReport()
    End Sub

    'measuring by a different date runs the report again, same as changing the show box
    Private Sub cboMeasureBy_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboMeasureBy.SelectedIndexChanged
        If stillLoading Then
            Exit Sub
        End If

        RunReport()
    End Sub

    'runs the whole report for the date range picked
    Private Sub btnRunReport_Click(sender As Object, e As EventArgs) Handles btnRunReport.Click
        If RunReport() Then
            WriteLog("REPORT", "Sales report run (" & cboReportType.Text & ") for " & dtpFrom.Value.ToShortDateString() & " to " & dtpTo.Value.ToShortDateString())
        End If
    End Sub

    'saves whichever report is on screen. the file is named after the report type so a folder of
    'them does not end up as four files all called SalesReport
    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        Dim fileName As String = cboReportType.Text.Replace(" ", "") & ".csv"

        If ExportGridToCsv(dgvSalesByFilm, fileName, "Sales Report") Then
            WriteLog("REPORT", "Sales report exported (" & cboReportType.Text & "), " & dgvSalesByFilm.Rows.Count & " rows")
        End If
    End Sub

    'runs the report for the date range picked, showing tickets, concessions or both depending
    'on what the show box is set to. comes back false if it would not run, so the caller knows
    'not to log it. the check lives in here rather than on the run button because picking a
    'different option in the show box runs the report too and used to skip the check
    Private Function RunReport() As Boolean
        If dtpFrom.Value.Date > dtpTo.Value.Date Then
            MessageBox.Show("From date cant be after the to date", "Sales Report", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If

        Dim fromDate As Date = dtpFrom.Value.Date
        Dim toDate As Date = dtpTo.Value.Date

        'the combined report runs two queries and then walks both tables, so it is worth
        'showing the busy cursor rather than having the screen sit there looking stuck
        Me.Cursor = Cursors.WaitCursor

        If cboReportType.Text = "Tickets only" Then
            Dim ticketRevenue As Double = LoadTicketsByFilm(fromDate, toDate)

            lblTicketRevenue.Visible = True
            lblFoodRevenue.Visible = False
            lblTicketRevenue.Text = "Ticket revenue: " & FormatCurrency(ticketRevenue)
            lblGrandTotal.Text = "Tickets total: " & FormatCurrency(ticketRevenue)

        ElseIf cboReportType.Text = "Concessions only" Then
            Dim foodRevenue As Double = LoadConcessionsByItem(fromDate, toDate)

            lblTicketRevenue.Visible = False
            lblFoodRevenue.Visible = True
            lblFoodRevenue.Text = "Concessions revenue: " & FormatCurrency(foodRevenue)
            lblGrandTotal.Text = "Concessions total: " & FormatCurrency(foodRevenue)

        ElseIf cboReportType.Text = "By screening" Then
            Dim ticketRevenue As Double = LoadTicketsByScreening(fromDate, toDate)

            lblTicketRevenue.Visible = True
            lblFoodRevenue.Visible = False
            lblTicketRevenue.Text = "Ticket revenue: " & FormatCurrency(ticketRevenue)
            lblGrandTotal.Text = "Tickets total: " & FormatCurrency(ticketRevenue)

        ElseIf cboReportType.Text = "By screen" Then
            Dim ticketRevenue As Double = LoadTicketsByScreen(fromDate, toDate)

            lblTicketRevenue.Visible = True
            lblFoodRevenue.Visible = False
            lblTicketRevenue.Text = "Ticket revenue: " & FormatCurrency(ticketRevenue)
            lblGrandTotal.Text = "Tickets total: " & FormatCurrency(ticketRevenue)

        ElseIf cboReportType.Text = "Cancellations" Then
            Dim refunded As Double = LoadCancellations(fromDate, toDate)

            lblTicketRevenue.Visible = False
            lblFoodRevenue.Visible = True
            lblFoodRevenue.Text = "These sales are not counted in the takings"
            lblGrandTotal.Text = "Refunded: " & FormatCurrency(refunded)

        Else
            'both together, so the grid gets a column each and the totals show the split
            Dim ticketRevenue As Double = 0
            Dim foodRevenue As Double = 0

            LoadCombinedByFilm(fromDate, toDate, ticketRevenue, foodRevenue)

            lblTicketRevenue.Visible = True
            lblFoodRevenue.Visible = True
            lblTicketRevenue.Text = "Ticket revenue: " & FormatCurrency(ticketRevenue)
            lblFoodRevenue.Text = "Concessions revenue: " & FormatCurrency(foodRevenue)
            lblGrandTotal.Text = "Grand total: " & FormatCurrency(ticketRevenue + foodRevenue)
        End If

        Me.Cursor = Cursors.Default
        ShowCount()

        Return True
    End Function

    'says how many rows came back, worded for whichever report is on screen. the other forms all
    'have one of these under the grid and this one did not
    Private Sub ShowCount()
        Dim shown As Integer = dgvSalesByFilm.Rows.Count
        Dim thing As String = "film"

        If cboReportType.Text = "Concessions only" Then
            thing = "item"
        ElseIf cboReportType.Text = "Cancellations" Then
            thing = "cancelled booking"
        ElseIf cboReportType.Text = "By screening" Then
            thing = "screening"
        ElseIf cboReportType.Text = "By screen" Then
            thing = "screen"
        End If

        If shown = 0 Then
            lblGridCount.Text = "Nothing in this date range"
        ElseIf shown = 1 Then
            lblGridCount.Text = "1 " & thing
        Else
            lblGridCount.Text = shown & " " & thing & "s"
        End If
    End Sub

    'fills the grid with the tickets sold and what they came to for each film, and returns the total.
    'the money comes from the seats on the booking times the screening price, not from TotalCost,
    'because TotalCost has the food added onto it as well
    Private Function LoadTicketsByFilm(fromDate As Date, toDate As Date) As Double
        Dim dt As DataTable = GetTicketsByFilmTable(fromDate, toDate)

        dgvSalesByFilm.DataSource = dt

        If dgvSalesByFilm.Columns.Count > 0 Then
            dgvSalesByFilm.Columns("FilmTitle").HeaderText = "Film"
            dgvSalesByFilm.Columns("Tickets").HeaderText = "Tickets sold"
            dgvSalesByFilm.Columns("TicketRevenue").HeaderText = "Ticket revenue"
            dgvSalesByFilm.Columns("FilmTitle").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            dgvSalesByFilm.Columns("TicketRevenue").DefaultCellStyle.Format = "C"
        End If

        Return TotalColumn(dt, "TicketRevenue")
    End Function

    'one row per screening, so a manager can see which showings actually sold and which played to
    'an empty room. this is tickets only, the food is bought against the booking and not against
    'the seat, so splitting it per screening would say more than the data really knows
    Private Function LoadTicketsByScreening(fromDate As Date, toDate As Date) As Double
        Dim dt As New DataTable

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            'the screen is joined on as well so the row says where it was on, which makes the
            'quiet showings easier to place
            SQLCmd.CommandText = "SELECT FilmTitle, tblScreening.ScreeningDate, tblScreening.ScreeningTime, " &
                                 "ScreenName, COUNT(*) AS Tickets, SUM(SeatPricePaid) AS TicketRevenue " &
                                 "FROM (((tblBookingSeat INNER JOIN tblBooking ON tblBookingSeat.BookingID = tblBooking.BookingID) " &
                                 "INNER JOIN tblScreening ON tblBooking.ScreeningID = tblScreening.ScreeningID) " &
                                 "INNER JOIN tblFilm ON tblScreening.FilmID = tblFilm.FilmID) " &
                                 "INNER JOIN tblScreen ON tblScreening.ScreenID = tblScreen.ScreenID " &
                                 "WHERE IIf(@ByScreening, tblScreening.ScreeningDate, tblBooking.BookingDate) >= @FromDate " &
                                 "AND IIf(@ByScreening2, tblScreening.ScreeningDate, tblBooking.BookingDate) < @ToDate " &
                                 "AND tblBooking.BookingStatus <> @Cancelled " &
                                 "GROUP BY FilmTitle, tblScreening.ScreeningDate, tblScreening.ScreeningTime, ScreenName " &
                                 "ORDER BY tblScreening.ScreeningDate, tblScreening.ScreeningTime"
            SQLCmd.Parameters.AddWithValue("@ByScreening", MeasuringByScreening())
            SQLCmd.Parameters.AddWithValue("@FromDate", fromDate)
            SQLCmd.Parameters.AddWithValue("@ByScreening2", MeasuringByScreening())
            SQLCmd.Parameters.AddWithValue("@ToDate", PeriodEnd(toDate))
            SQLCmd.Parameters.AddWithValue("@Cancelled", BookingCancelled)
            Dim da As New OleDbDataAdapter(SQLCmd)
            da.Fill(dt)
            cn.Close()
        End If

        dgvSalesByFilm.DataSource = dt

        If dgvSalesByFilm.Columns.Count > 0 Then
            dgvSalesByFilm.Columns("FilmTitle").HeaderText = "Film"
            dgvSalesByFilm.Columns("ScreeningDate").HeaderText = "Date"
            dgvSalesByFilm.Columns("ScreeningTime").HeaderText = "Time"
            dgvSalesByFilm.Columns("ScreenName").HeaderText = "Screen"
            dgvSalesByFilm.Columns("Tickets").HeaderText = "Tickets sold"
            dgvSalesByFilm.Columns("TicketRevenue").HeaderText = "Ticket revenue"
            dgvSalesByFilm.Columns("FilmTitle").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            dgvSalesByFilm.Columns("ScreeningDate").DefaultCellStyle.Format = "dd/MM/yyyy"
            dgvSalesByFilm.Columns("TicketRevenue").DefaultCellStyle.Format = "C"
        End If

        Return TotalColumn(dt, "TicketRevenue")
    End Function

    'one row per screen, for comparing the rooms against each other rather than the films
    Private Function LoadTicketsByScreen(fromDate As Date, toDate As Date) As Double
        Dim dt As New DataTable

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT ScreenName, COUNT(*) AS Tickets, SUM(SeatPricePaid) AS TicketRevenue " &
                                 "FROM (((tblBookingSeat INNER JOIN tblBooking ON tblBookingSeat.BookingID = tblBooking.BookingID) " &
                                 "INNER JOIN tblScreening ON tblBooking.ScreeningID = tblScreening.ScreeningID) " &
                                 "INNER JOIN tblFilm ON tblScreening.FilmID = tblFilm.FilmID) " &
                                 "INNER JOIN tblScreen ON tblScreening.ScreenID = tblScreen.ScreenID " &
                                 "WHERE IIf(@ByScreening, tblScreening.ScreeningDate, tblBooking.BookingDate) >= @FromDate " &
                                 "AND IIf(@ByScreening2, tblScreening.ScreeningDate, tblBooking.BookingDate) < @ToDate " &
                                 "AND tblBooking.BookingStatus <> @Cancelled " &
                                 "GROUP BY ScreenName " &
                                 "ORDER BY ScreenName"
            SQLCmd.Parameters.AddWithValue("@ByScreening", MeasuringByScreening())
            SQLCmd.Parameters.AddWithValue("@FromDate", fromDate)
            SQLCmd.Parameters.AddWithValue("@ByScreening2", MeasuringByScreening())
            SQLCmd.Parameters.AddWithValue("@ToDate", PeriodEnd(toDate))
            SQLCmd.Parameters.AddWithValue("@Cancelled", BookingCancelled)
            Dim da As New OleDbDataAdapter(SQLCmd)
            da.Fill(dt)
            cn.Close()
        End If

        dgvSalesByFilm.DataSource = dt

        If dgvSalesByFilm.Columns.Count > 0 Then
            dgvSalesByFilm.Columns("ScreenName").HeaderText = "Screen"
            dgvSalesByFilm.Columns("Tickets").HeaderText = "Tickets sold"
            dgvSalesByFilm.Columns("TicketRevenue").HeaderText = "Ticket revenue"
            dgvSalesByFilm.Columns("ScreenName").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            dgvSalesByFilm.Columns("TicketRevenue").DefaultCellStyle.Format = "C"
        End If

        Return TotalColumn(dt, "TicketRevenue")
    End Function

    'fills the grid with the bookings that were cancelled in the date range and what they came to,
    'and gives back what that adds up to. this is only possible now that cancelling marks a booking
    'instead of deleting it, before this the sale was gone and there was nothing left to report on.
    'the date used is when it was cancelled, not when it was booked, because a manager looking at
    'refunds wants to know what went out that week. this is the one report the measure by box
    'does not change, for the same reason
    Private Function LoadCancellations(fromDate As Date, toDate As Date) As Double
        Dim dt As New DataTable

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT tblBooking.BookingID, CustomerForename & ' ' & CustomerSurname AS CustomerName, " &
                                 "FilmTitle, CancelledDate, TotalCost " &
                                 "FROM ((tblBooking INNER JOIN tblCustomer ON tblBooking.CustomerID = tblCustomer.CustomerID) " &
                                 "INNER JOIN tblScreening ON tblBooking.ScreeningID = tblScreening.ScreeningID) " &
                                 "INNER JOIN tblFilm ON tblScreening.FilmID = tblFilm.FilmID " &
                                 "WHERE tblBooking.BookingStatus = @Cancelled " &
                                 "AND tblBooking.CancelledDate >= @FromDate AND tblBooking.CancelledDate < @ToDate " &
                                 "ORDER BY tblBooking.CancelledDate DESC, tblBooking.BookingID DESC"
            SQLCmd.Parameters.AddWithValue("@Cancelled", BookingCancelled)
            SQLCmd.Parameters.AddWithValue("@FromDate", fromDate)
            SQLCmd.Parameters.AddWithValue("@ToDate", PeriodEnd(toDate))
            Dim da As New OleDbDataAdapter(SQLCmd)
            da.Fill(dt)
            cn.Close()
        End If

        dgvSalesByFilm.DataSource = dt

        If dgvSalesByFilm.Columns.Count > 0 Then
            dgvSalesByFilm.Columns("BookingID").HeaderText = "Booking"
            dgvSalesByFilm.Columns("CustomerName").HeaderText = "Customer"
            dgvSalesByFilm.Columns("FilmTitle").HeaderText = "Film"
            dgvSalesByFilm.Columns("CancelledDate").HeaderText = "Cancelled"
            dgvSalesByFilm.Columns("TotalCost").HeaderText = "Refunded"
            dgvSalesByFilm.Columns("FilmTitle").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            dgvSalesByFilm.Columns("CancelledDate").DefaultCellStyle.Format = "dd/MM/yyyy HH:mm"
            dgvSalesByFilm.Columns("TotalCost").DefaultCellStyle.Format = "C"
        End If

        Return TotalColumn(dt, "TotalCost")
    End Function

    'fills the grid with how many of each food item was sold and what it came to, returns the total
    Private Function LoadConcessionsByItem(fromDate As Date, toDate As Date) As Double
        Dim dt As New DataTable

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            'join order item to food item (for the name) and to booking (for the date). the money is
            'summed off ItemPricePaid, which is what each line was actually charged, so putting a
            'price up on the menu cannot reach back and rewrite last month's takings.
            'the food on a cancelled booking is left out, it was refunded so it is not takings
            SQLCmd.CommandText = "SELECT FoodItemName, SUM(Quantity) AS Sold, SUM(Quantity * ItemPricePaid) AS FoodRevenue " &
                                 "FROM ((tblOrderItem INNER JOIN tblFoodItem ON tblOrderItem.FoodItemID = tblFoodItem.FoodItemID) " &
                                 "INNER JOIN tblBooking ON tblOrderItem.BookingID = tblBooking.BookingID) " &
                                 "INNER JOIN tblScreening ON tblBooking.ScreeningID = tblScreening.ScreeningID " &
                                 "WHERE IIf(@ByScreening, tblScreening.ScreeningDate, tblBooking.BookingDate) >= @FromDate " &
                                 "AND IIf(@ByScreening2, tblScreening.ScreeningDate, tblBooking.BookingDate) < @ToDate " &
                                 "AND tblBooking.BookingStatus <> @Cancelled " &
                                 "GROUP BY FoodItemName"
            'the switch is in the query twice, once for each IIf, so it goes in twice here as well,
            'in the order the query mentions them. the two have to be named differently. giving
            'them the same name looks tidier but Jet then treats it as one parameter, the rest
            'shuffle up by one and the report quietly comes back with nothing in it
            SQLCmd.Parameters.AddWithValue("@ByScreening", MeasuringByScreening())
            SQLCmd.Parameters.AddWithValue("@FromDate", fromDate)
            SQLCmd.Parameters.AddWithValue("@ByScreening2", MeasuringByScreening())
            SQLCmd.Parameters.AddWithValue("@ToDate", PeriodEnd(toDate))
            SQLCmd.Parameters.AddWithValue("@Cancelled", BookingCancelled)
            Dim da As New OleDbDataAdapter(SQLCmd)
            da.Fill(dt)
            cn.Close()
        End If

        dgvSalesByFilm.DataSource = dt

        If dgvSalesByFilm.Columns.Count > 0 Then
            dgvSalesByFilm.Columns("FoodItemName").HeaderText = "Item"
            dgvSalesByFilm.Columns("Sold").HeaderText = "Sold"
            dgvSalesByFilm.Columns("FoodRevenue").HeaderText = "Revenue"
            dgvSalesByFilm.Columns("FoodItemName").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            dgvSalesByFilm.Columns("FoodRevenue").DefaultCellStyle.Format = "C"
        End If

        Return TotalColumn(dt, "FoodRevenue")
    End Function

    'fills the grid with a row per film showing the tickets and the concessions side by side.
    'the two totals are passed back out so the labels underneath can show the split
    Private Sub LoadCombinedByFilm(fromDate As Date, toDate As Date, ByRef ticketTotal As Double, ByRef foodTotal As Double)
        Dim dtTickets As DataTable = GetTicketsByFilmTable(fromDate, toDate)
        Dim dtFood As DataTable = GetFoodByFilmTable(fromDate, toDate)

        'a table is built by hand here because the two lots of figures come from different queries
        Dim dt As New DataTable
        dt.Columns.Add("FilmTitle", GetType(String))
        dt.Columns.Add("TicketRevenue", GetType(Double))
        dt.Columns.Add("FoodRevenue", GetType(Double))
        dt.Columns.Add("Total", GetType(Double))

        ticketTotal = 0
        foodTotal = 0

        For Each ticketRow As DataRow In dtTickets.Rows
            Dim title As String = ticketRow("FilmTitle").ToString()
            Dim tickets As Double = 0
            If Not IsDBNull(ticketRow("TicketRevenue")) Then
                tickets = CDbl(ticketRow("TicketRevenue"))
            End If

            Dim food As Double = FindFilmFood(dtFood, title)

            dt.Rows.Add(title, tickets, food, tickets + food)
            ticketTotal = ticketTotal + tickets
            foodTotal = foodTotal + food
        Next

        'a booking can have food on it without any seats, so a film could have sold snacks and no
        'tickets. those films are not in the tickets table at all so they get added on here
        For Each foodRow As DataRow In dtFood.Rows
            Dim title As String = foodRow("FilmTitle").ToString()
            If Not FilmIsInTable(dtTickets, title) Then
                Dim food As Double = 0
                If Not IsDBNull(foodRow("FoodRevenue")) Then
                    food = CDbl(foodRow("FoodRevenue"))
                End If

                dt.Rows.Add(title, 0, food, food)
                foodTotal = foodTotal + food
            End If
        Next

        dgvSalesByFilm.DataSource = dt

        If dgvSalesByFilm.Columns.Count > 0 Then
            dgvSalesByFilm.Columns("FilmTitle").HeaderText = "Film"
            dgvSalesByFilm.Columns("TicketRevenue").HeaderText = "Tickets"
            dgvSalesByFilm.Columns("FoodRevenue").HeaderText = "Concessions"
            dgvSalesByFilm.Columns("Total").HeaderText = "Total"
            dgvSalesByFilm.Columns("FilmTitle").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            dgvSalesByFilm.Columns("TicketRevenue").DefaultCellStyle.Format = "C"
            dgvSalesByFilm.Columns("FoodRevenue").DefaultCellStyle.Format = "C"
            dgvSalesByFilm.Columns("Total").DefaultCellStyle.Format = "C"
        End If
    End Sub

    'the tickets sold and what they came to for each film in the date range
    Private Function GetTicketsByFilmTable(fromDate As Date, toDate As Date) As DataTable
        Dim dt As New DataTable

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            'one row of tblBookingSeat is one ticket, so counting them gives the tickets sold.
            'the money is what those tickets were actually sold for, which is written on the seat
            'row at the time of the sale. it used to be worked out here from the price on the
            'screening as it is now, which meant putting a ticket up changed what last month had
            'taken. two of the joins have gone with it, since the seat type is no longer needed.
            'cancelling a booking deletes its seat rows, so there should be nothing in here to
            'leave out anyway, but the filter is on to match the other three queries and so this
            'still comes out right if cancelling ever stops deleting them
            SQLCmd.CommandText = "SELECT FilmTitle, COUNT(*) AS Tickets, SUM(SeatPricePaid) AS TicketRevenue " &
                                 "FROM ((tblBookingSeat INNER JOIN tblBooking ON tblBookingSeat.BookingID = tblBooking.BookingID) " &
                                 "INNER JOIN tblScreening ON tblBooking.ScreeningID = tblScreening.ScreeningID) " &
                                 "INNER JOIN tblFilm ON tblScreening.FilmID = tblFilm.FilmID " &
                                 "WHERE IIf(@ByScreening, tblScreening.ScreeningDate, tblBooking.BookingDate) >= @FromDate " &
                                 "AND IIf(@ByScreening2, tblScreening.ScreeningDate, tblBooking.BookingDate) < @ToDate " &
                                 "AND tblBooking.BookingStatus <> @Cancelled " &
                                 "GROUP BY FilmTitle"
            'the switch is in the query twice, once for each IIf, so it goes in twice here as well,
            'in the order the query mentions them. the two have to be named differently. giving
            'them the same name looks tidier but Jet then treats it as one parameter, the rest
            'shuffle up by one and the report quietly comes back with nothing in it
            SQLCmd.Parameters.AddWithValue("@ByScreening", MeasuringByScreening())
            SQLCmd.Parameters.AddWithValue("@FromDate", fromDate)
            SQLCmd.Parameters.AddWithValue("@ByScreening2", MeasuringByScreening())
            SQLCmd.Parameters.AddWithValue("@ToDate", PeriodEnd(toDate))
            SQLCmd.Parameters.AddWithValue("@Cancelled", BookingCancelled)
            Dim da As New OleDbDataAdapter(SQLCmd)
            da.Fill(dt)
            cn.Close()
        End If

        Return dt
    End Function

    'what the concessions came to for each film, counted as the food bought on bookings for that film
    Private Function GetFoodByFilmTable(fromDate As Date, toDate As Date) As DataTable
        Dim dt As New DataTable

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            'cancelled bookings are left out, the same as on the concessions report. this used to join
            'tblFoodItem as well, but that was only ever there to get the price, and the price is on
            'the order line itself now, so it is one table and one set of brackets lighter
            SQLCmd.CommandText = "SELECT FilmTitle, SUM(Quantity * ItemPricePaid) AS FoodRevenue " &
                                 "FROM ((tblOrderItem " &
                                 "INNER JOIN tblBooking ON tblOrderItem.BookingID = tblBooking.BookingID) " &
                                 "INNER JOIN tblScreening ON tblBooking.ScreeningID = tblScreening.ScreeningID) " &
                                 "INNER JOIN tblFilm ON tblScreening.FilmID = tblFilm.FilmID " &
                                 "WHERE IIf(@ByScreening, tblScreening.ScreeningDate, tblBooking.BookingDate) >= @FromDate " &
                                 "AND IIf(@ByScreening2, tblScreening.ScreeningDate, tblBooking.BookingDate) < @ToDate " &
                                 "AND tblBooking.BookingStatus <> @Cancelled " &
                                 "GROUP BY FilmTitle"
            'the switch is in the query twice, once for each IIf, so it goes in twice here as well,
            'in the order the query mentions them. the two have to be named differently. giving
            'them the same name looks tidier but Jet then treats it as one parameter, the rest
            'shuffle up by one and the report quietly comes back with nothing in it
            SQLCmd.Parameters.AddWithValue("@ByScreening", MeasuringByScreening())
            SQLCmd.Parameters.AddWithValue("@FromDate", fromDate)
            SQLCmd.Parameters.AddWithValue("@ByScreening2", MeasuringByScreening())
            SQLCmd.Parameters.AddWithValue("@ToDate", PeriodEnd(toDate))
            SQLCmd.Parameters.AddWithValue("@Cancelled", BookingCancelled)
            Dim da As New OleDbDataAdapter(SQLCmd)
            da.Fill(dt)
            cn.Close()
        End If

        Return dt
    End Function

    'looks through the food table for a film and gives back what it took, or 0 if it is not in there
    Private Function FindFilmFood(dtFood As DataTable, filmTitle As String) As Double
        For Each row As DataRow In dtFood.Rows
            If row("FilmTitle").ToString() = filmTitle Then
                If IsDBNull(row("FoodRevenue")) Then
                    Return 0
                Else
                    Return CDbl(row("FoodRevenue"))
                End If
            End If
        Next

        Return 0
    End Function

    'says whether a film is already in one of the tables, used so a film is not listed twice
    Private Function FilmIsInTable(dt As DataTable, filmTitle As String) As Boolean
        For Each row As DataRow In dt.Rows
            If row("FilmTitle").ToString() = filmTitle Then
                Return True
            End If
        Next

        Return False
    End Function

    '1 when the report is being measured against the screening date instead of the booking date.
    'it goes into the queries as a parameter and the IIf in the WHERE picks the column with it.
    'the column name could have been glued into the SQL instead, which reads better, but then
    'the query is not a plain string any more and check-sql cannot pull it out and try it
    Private Function MeasuringByScreening() As Integer
        If cboMeasureBy.Text = "Screening date" Then
            Return 1
        Else
            Return 0
        End If
    End Function

    'gives back midnight at the start of the day after the one picked. every query then asks for
    'dates on or after the from date and before this, which takes in the whole of the last day.
    'BETWEEN was doing the wrong thing here, because a to date of the 5th means midnight on the
    '5th, so anything with a time later on that day fell outside the range and went missing
    Private Function PeriodEnd(toDate As Date) As Date
        Return toDate.Date.AddDays(1)
    End Function

    'adds up one money column of a table for the total underneath the grid
    Private Function TotalColumn(dt As DataTable, columnName As String) As Double
        Dim total As Double = 0

        For Each row As DataRow In dt.Rows
            If Not IsDBNull(row(columnName)) Then
                total = total + CDbl(row(columnName))
            End If
        Next

        Return total
    End Function


End Class
