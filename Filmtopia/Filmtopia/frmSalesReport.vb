Imports System.Data.OleDb

Public Class frmSalesReport

    'true while the form is setting itself up, so filling the show box does not run the report
    'before the dates have been put in
    Private stillLoading As Boolean = True

    'whatever the report is showing at the moment. it is held on the form rather than being a
    'local in each loader because the grid is not the only thing that reads it any more
    Private reportTable As DataTable

    'which column the report is sorted by at the moment and which way round. blank means it is in
    'whatever order the query gave it back in
    Private sortedBy As String = ""
    Private sortAscending As Boolean = True

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
        cboReportType.Items.Add("By day")
        cboReportType.Items.Add("By screening")
        cboReportType.Items.Add("By screen")
        cboReportType.Items.Add("By staff member")
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

    'looks for whatever has been typed in the find box
    Private Sub btnFind_Click(sender As Object, e As EventArgs) Handles btnFind.Click
        FindInReport()
    End Sub

    'enter in the find box does the same as pressing the button
    Private Sub txtFind_KeyDown(sender As Object, e As KeyEventArgs) Handles txtFind.KeyDown
        If e.KeyCode = Keys.Enter Then
            FindInReport()

            'stops the ding windows makes when enter is pressed in a text box
            e.SuppressKeyPress = True
        End If
    End Sub

    'sorts the report by its name column and then looks the typed name up in it, and picks out
    'the row it lands on. the sort has to happen first, a binary search on an unsorted list
    'would walk off in the wrong direction and miss things that are sitting right there
    Private Sub FindInReport()
        Dim target As String = txtFind.Text.Trim()

        If target = "" Then
            Exit Sub
        End If

        Dim columnName As String = NameColumn()

        If columnName = "" Then
            MessageBox.Show("This report has no names on it to look through.", "Find", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        SortReport(columnName, True)
        sortedBy = columnName
        sortAscending = True
        ShowReport()
        ShowSortArrow()

        Dim found As Integer = FindRow(columnName, target)

        If found = -1 Then
            MessageBox.Show("Nothing on this report starts with " & target & ".", "Find", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            dgvSalesByFilm.ClearSelection()
            dgvSalesByFilm.Rows(found).Selected = True
            dgvSalesByFilm.FirstDisplayedScrollingRowIndex = found
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

        'a new report has different columns, so any sort from the last one is forgotten
        sortedBy = ""
        sortAscending = True

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

        ElseIf cboReportType.Text = "By day" Then
            Dim ticketRevenue As Double = LoadTicketsByDay(fromDate, toDate)

            lblTicketRevenue.Visible = True
            lblFoodRevenue.Visible = False
            lblTicketRevenue.Text = "Ticket revenue: " & FormatCurrency(ticketRevenue)
            lblGrandTotal.Text = "Tickets total: " & FormatCurrency(ticketRevenue)

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

        ElseIf cboReportType.Text = "By staff member" Then
            Dim ticketRevenue As Double = LoadTicketsByStaff(fromDate, toDate)

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

        'the plural is kept as its own word rather than sticking an s on the end, because people
        'does not work that way
        Dim thing As String = "film"
        Dim things As String = "films"

        If cboReportType.Text = "Concessions only" Then
            thing = "item"
            things = "items"
        ElseIf cboReportType.Text = "Cancellations" Then
            thing = "cancelled booking"
            things = "cancelled bookings"
        ElseIf cboReportType.Text = "By day" Then
            thing = "day"
            things = "days"
        ElseIf cboReportType.Text = "By screening" Then
            thing = "screening"
            things = "screenings"
        ElseIf cboReportType.Text = "By screen" Then
            thing = "screen"
            things = "screens"
        ElseIf cboReportType.Text = "By staff member" Then
            thing = "person"
            things = "people"
        End If

        If shown = 0 Then
            lblGridCount.Text = "Nothing in this date range"
        ElseIf shown = 1 Then
            lblGridCount.Text = "1 " & thing
        Else
            lblGridCount.Text = shown & " " & things
        End If
    End Sub

    'clicking a heading sorts the report by that column. clicking the same one again turns the
    'order round
    Private Sub dgvSalesByFilm_ColumnHeaderMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles dgvSalesByFilm.ColumnHeaderMouseClick
        Dim columnName As String = dgvSalesByFilm.Columns(e.ColumnIndex).Name

        If columnName = sortedBy Then
            sortAscending = Not sortAscending
        Else
            sortedBy = columnName

            'a name is easier to find a to z, but money and counts are more use biggest first, so
            'the first click on a column starts it off whichever way round suits it
            sortAscending = (dgvSalesByFilm.Columns(e.ColumnIndex).ValueType Is GetType(String))
        End If

        SortReport(sortedBy, sortAscending)
        ShowReport()
        ShowSortArrow()
    End Sub

    'puts the little arrow on whichever heading is being sorted by. the grid draws that itself
    'normally, but not once its own sorting has been turned off
    Private Sub ShowSortArrow()
        For Each col As DataGridViewColumn In dgvSalesByFilm.Columns
            If col.Name = sortedBy Then
                If sortAscending Then
                    col.HeaderCell.SortGlyphDirection = SortOrder.Ascending
                Else
                    col.HeaderCell.SortGlyphDirection = SortOrder.Descending
                End If
            Else
                col.HeaderCell.SortGlyphDirection = SortOrder.None
            End If
        Next
    End Sub

    'puts the table that has just been built onto the grid, then tidies the columns for whichever
    'report it is. this used to be written out again at the bottom of every one of the loaders
    Private Sub ShowReport()
        dgvSalesByFilm.DataSource = reportTable

        If dgvSalesByFilm.Columns.Count = 0 Then
            Exit Sub
        End If

        If cboReportType.Text = "Tickets only" Then
            dgvSalesByFilm.Columns("FilmTitle").HeaderText = "Film"
            dgvSalesByFilm.Columns("Tickets").HeaderText = "Tickets sold"
            dgvSalesByFilm.Columns("TicketRevenue").HeaderText = "Ticket revenue"
            dgvSalesByFilm.Columns("FilmTitle").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            dgvSalesByFilm.Columns("TicketRevenue").DefaultCellStyle.Format = "C"
        ElseIf cboReportType.Text = "By day" Then
            dgvSalesByFilm.Columns("ReportDay").HeaderText = "Day"
            dgvSalesByFilm.Columns("Tickets").HeaderText = "Tickets sold"
            dgvSalesByFilm.Columns("TicketRevenue").HeaderText = "Ticket revenue"
            dgvSalesByFilm.Columns("ReportDay").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            dgvSalesByFilm.Columns("ReportDay").DefaultCellStyle.Format = "dd/MM/yyyy"
            dgvSalesByFilm.Columns("TicketRevenue").DefaultCellStyle.Format = "C"
        ElseIf cboReportType.Text = "By screening" Then
            dgvSalesByFilm.Columns("FilmTitle").HeaderText = "Film"
            dgvSalesByFilm.Columns("ScreeningDate").HeaderText = "Date"
            dgvSalesByFilm.Columns("ScreeningTime").HeaderText = "Time"
            dgvSalesByFilm.Columns("ScreenName").HeaderText = "Screen"
            dgvSalesByFilm.Columns("Tickets").HeaderText = "Tickets sold"
            dgvSalesByFilm.Columns("TicketRevenue").HeaderText = "Ticket revenue"
            dgvSalesByFilm.Columns("FilmTitle").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            dgvSalesByFilm.Columns("ScreeningDate").DefaultCellStyle.Format = "dd/MM/yyyy"
            dgvSalesByFilm.Columns("TicketRevenue").DefaultCellStyle.Format = "C"
        ElseIf cboReportType.Text = "By screen" Then
            dgvSalesByFilm.Columns("ScreenName").HeaderText = "Screen"
            dgvSalesByFilm.Columns("Tickets").HeaderText = "Tickets sold"
            dgvSalesByFilm.Columns("TicketRevenue").HeaderText = "Ticket revenue"
            dgvSalesByFilm.Columns("ScreenName").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            dgvSalesByFilm.Columns("TicketRevenue").DefaultCellStyle.Format = "C"
        ElseIf cboReportType.Text = "By staff member" Then
            dgvSalesByFilm.Columns("SoldBy").HeaderText = "Sold by"
            dgvSalesByFilm.Columns("Tickets").HeaderText = "Tickets sold"
            dgvSalesByFilm.Columns("TicketRevenue").HeaderText = "Ticket revenue"
            dgvSalesByFilm.Columns("SoldBy").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            dgvSalesByFilm.Columns("TicketRevenue").DefaultCellStyle.Format = "C"
        ElseIf cboReportType.Text = "Cancellations" Then
            dgvSalesByFilm.Columns("BookingID").HeaderText = "Booking"
            dgvSalesByFilm.Columns("CustomerName").HeaderText = "Customer"
            dgvSalesByFilm.Columns("FilmTitle").HeaderText = "Film"
            dgvSalesByFilm.Columns("CancelledDate").HeaderText = "Cancelled"
            dgvSalesByFilm.Columns("TotalCost").HeaderText = "Refunded"
            dgvSalesByFilm.Columns("FilmTitle").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            dgvSalesByFilm.Columns("CancelledDate").DefaultCellStyle.Format = "dd/MM/yyyy HH:mm"
            dgvSalesByFilm.Columns("TotalCost").DefaultCellStyle.Format = "C"
        ElseIf cboReportType.Text = "Concessions only" Then
            dgvSalesByFilm.Columns("FoodItemName").HeaderText = "Item"
            dgvSalesByFilm.Columns("Sold").HeaderText = "Sold"
            dgvSalesByFilm.Columns("FoodRevenue").HeaderText = "Revenue"
            dgvSalesByFilm.Columns("FoodItemName").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            dgvSalesByFilm.Columns("FoodRevenue").DefaultCellStyle.Format = "C"
        Else
            dgvSalesByFilm.Columns("FilmTitle").HeaderText = "Film"
            dgvSalesByFilm.Columns("TicketRevenue").HeaderText = "Tickets"
            dgvSalesByFilm.Columns("FoodRevenue").HeaderText = "Concessions"
            dgvSalesByFilm.Columns("Total").HeaderText = "Total"
            dgvSalesByFilm.Columns("FilmTitle").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            dgvSalesByFilm.Columns("TicketRevenue").DefaultCellStyle.Format = "C"
            dgvSalesByFilm.Columns("FoodRevenue").DefaultCellStyle.Format = "C"
            dgvSalesByFilm.Columns("Total").DefaultCellStyle.Format = "C"
        End If

        'the grid is told not to sort itself. the sorting is done in code further down, and if the
        'grid also sorted then clicking a heading would run both and they would fight over the order
        For Each col As DataGridViewColumn In dgvSalesByFilm.Columns
            col.SortMode = DataGridViewColumnSortMode.Programmatic
        Next

        dgvSalesByFilm.ClearSelection()
    End Sub

    'fills the grid with the tickets sold and what they came to for each film, and returns the total.
    'the money comes from the seats on the booking times the screening price, not from TotalCost,
    'because TotalCost has the food added onto it as well
    Private Function LoadTicketsByFilm(fromDate As Date, toDate As Date) As Double
        Dim dt As DataTable = GetTicketsByFilmTable(fromDate, toDate)

        reportTable = dt
        ShowReport()

        Return TotalColumn(dt, "TicketRevenue")
    End Function

    'one row per day in the range, with what was sold on it.
    '
    'the database is not asked to group this. it hands back one row per ticket and the counting up
    'is done below, in one walk through the list. that is partly because it is the bit worth
    'writing, and partly because a GROUP BY only gives back the days that had a sale on them. a day
    'the cinema took nothing simply would not appear, and a run of takings with the quiet days
    'silently missing out of the middle of it tells you the wrong story
    Private Function LoadTicketsByDay(fromDate As Date, toDate As Date) As Double
        Dim dtTickets As DataTable = GetTicketsByDayTable(fromDate, toDate)

        'one slot per day in the range, counting both ends, so nothing is left out
        Dim howManyDays As Integer = CInt((toDate.Date - fromDate.Date).TotalDays) + 1

        If howManyDays < 1 Then
            howManyDays = 1
        End If

        Dim dayTickets(howManyDays - 1) As Integer
        Dim dayTakings(howManyDays - 1) As Double

        Dim useScreeningDate As Boolean = (MeasuringByScreening() = 1)

        For Each row As DataRow In dtTickets.Rows
            Dim soldOn As Date

            If useScreeningDate Then
                soldOn = CDate(row("ScreeningDate"))
            Else
                soldOn = CDate(row("BookingDate"))
            End If

            'which slot a ticket belongs in is just how many days it is past the start of the range
            Dim slot As Integer = CInt((soldOn.Date - fromDate.Date).TotalDays)

            If slot >= 0 And slot < howManyDays Then
                dayTickets(slot) = dayTickets(slot) + 1

                If Not IsDBNull(row("SeatPricePaid")) Then
                    dayTakings(slot) = dayTakings(slot) + CDbl(row("SeatPricePaid"))
                End If
            End If
        Next

        'the two lists become a table so the grid can show it like any other report
        Dim dt As New DataTable
        dt.Columns.Add("ReportDay", GetType(Date))
        dt.Columns.Add("Tickets", GetType(Integer))
        dt.Columns.Add("TicketRevenue", GetType(Double))

        Dim total As Double = 0

        For i As Integer = 0 To howManyDays - 1
            dt.Rows.Add(fromDate.Date.AddDays(i), dayTickets(i), dayTakings(i))
            total = total + dayTakings(i)
        Next

        reportTable = dt
        ShowReport()

        Return total
    End Function

    'one row per ticket in the range, with both dates on it so the counting up can use whichever
    'one the measure by box is set to. no film or screen is joined on, nothing here needs them
    Private Function GetTicketsByDayTable(fromDate As Date, toDate As Date) As DataTable
        Dim dt As New DataTable

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT tblBooking.BookingDate, tblScreening.ScreeningDate, SeatPricePaid " &
                                 "FROM (tblBookingSeat INNER JOIN tblBooking ON tblBookingSeat.BookingID = tblBooking.BookingID) " &
                                 "INNER JOIN tblScreening ON tblBooking.ScreeningID = tblScreening.ScreeningID " &
                                 "WHERE IIf(@ByScreening, tblScreening.ScreeningDate, tblBooking.BookingDate) >= @FromDate " &
                                 "AND IIf(@ByScreening2, tblScreening.ScreeningDate, tblBooking.BookingDate) < @ToDate " &
                                 "AND tblBooking.BookingStatus <> @Cancelled"
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

        reportTable = dt
        ShowReport()

        Return TotalColumn(dt, "TicketRevenue")
    End Function

    'one row per member of staff, for who has been taking the money.
    '
    'the login is joined on with a LEFT JOIN and not an ordinary one. an ordinary join would only
    'keep the bookings that have somebody recorded against them, and almost none of them do yet,
    'because the column that records it is new and was deliberately not filled in backwards. those
    'sales would just have gone missing off the report and the total would not have matched the
    'other reports. they come through as Not recorded instead, which is the truth, and that row
    'will shrink on its own as sales get taken
    Private Function LoadTicketsByStaff(fromDate As Date, toDate As Date) As Double
        Dim dt As New DataTable

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT IIf(tblLogin.Username IS NULL, 'Not recorded', tblLogin.Username) AS SoldBy, " &
                                 "COUNT(*) AS Tickets, SUM(SeatPricePaid) AS TicketRevenue " &
                                 "FROM ((tblBookingSeat INNER JOIN tblBooking ON tblBookingSeat.BookingID = tblBooking.BookingID) " &
                                 "INNER JOIN tblScreening ON tblBooking.ScreeningID = tblScreening.ScreeningID) " &
                                 "LEFT JOIN tblLogin ON tblBooking.LoginID = tblLogin.LoginID " &
                                 "WHERE IIf(@ByScreening, tblScreening.ScreeningDate, tblBooking.BookingDate) >= @FromDate " &
                                 "AND IIf(@ByScreening2, tblScreening.ScreeningDate, tblBooking.BookingDate) < @ToDate " &
                                 "AND tblBooking.BookingStatus <> @Cancelled " &
                                 "GROUP BY IIf(tblLogin.Username IS NULL, 'Not recorded', tblLogin.Username) " &
                                 "ORDER BY IIf(tblLogin.Username IS NULL, 'Not recorded', tblLogin.Username)"
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

        reportTable = dt
        ShowReport()

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

        reportTable = dt
        ShowReport()

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

        reportTable = dt
        ShowReport()

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

        reportTable = dt
        ShowReport()

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

        reportTable = dt

        'this is the one report the database does not put in any order, because it is stitched
        'together here out of two queries. biggest takings first is what a manager wants to see
        SortReport("Total", False)
        sortedBy = "Total"
        sortAscending = False

        ShowReport()
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

    'sorts the report by one of its columns.
    '
    'what actually gets sorted is a list of row numbers, not the rows. that way nothing big is
    'being shuffled about while the sort runs, and the table is only rebuilt once at the end.
    'a merge sort is used rather than something simpler. at the size this report runs at it makes
    'no odds at all, but it does not get worse as the cinema takes more bookings, and it keeps rows
    'that tie in the order they were already in, which stops the list jumping about when a column
    'with a lot of equal values is sorted
    Private Sub SortReport(columnName As String, ascending As Boolean)
        If reportTable Is Nothing Then
            Exit Sub
        End If

        If reportTable.Rows.Count < 2 Then
            Exit Sub
        End If

        Dim howMany As Integer = reportTable.Rows.Count
        Dim order(howMany - 1) As Integer

        For i As Integer = 0 To howMany - 1
            order(i) = i
        Next

        MergeSort(order, 0, howMany - 1, columnName, ascending)

        'the table is built again with the rows in the order the sort worked out
        Dim sorted As DataTable = reportTable.Clone()

        For i As Integer = 0 To howMany - 1
            sorted.ImportRow(reportTable.Rows(order(i)))
        Next

        reportTable = sorted
    End Sub

    'splits the list down the middle, sorts each half by calling itself, then merges the two sorted
    'halves back together. one item on its own is already sorted, which is where it stops
    Private Sub MergeSort(order() As Integer, low As Integer, high As Integer, columnName As String, ascending As Boolean)
        If low >= high Then
            Exit Sub
        End If

        Dim middle As Integer = (low + high) \ 2

        MergeSort(order, low, middle, columnName, ascending)
        MergeSort(order, middle + 1, high, columnName, ascending)
        Merge(order, low, middle, high, columnName, ascending)
    End Sub

    'the two halves are each already in order, so this walks along both at once and keeps taking
    'whichever of the two front rows should come first
    Private Sub Merge(order() As Integer, low As Integer, middle As Integer, high As Integer, columnName As String, ascending As Boolean)
        Dim merged(high - low) As Integer
        Dim left As Integer = low
        Dim right As Integer = middle + 1
        Dim put As Integer = 0

        Do While left <= middle And right <= high
            If ComesFirst(order(left), order(right), columnName, ascending) Then
                merged(put) = order(left)
                left = left + 1
            Else
                merged(put) = order(right)
                right = right + 1
            End If

            put = put + 1
        Loop

        'one half runs out before the other. whatever is left in the other one is already in order
        'so it just goes on the end
        Do While left <= middle
            merged(put) = order(left)
            left = left + 1
            put = put + 1
        Loop

        Do While right <= high
            merged(put) = order(right)
            right = right + 1
            put = put + 1
        Loop

        For i As Integer = 0 To high - low
            order(low + i) = merged(i)
        Next
    End Sub

    'says whether the row at a should come before the row at b
    Private Function ComesFirst(a As Integer, b As Integer, columnName As String, ascending As Boolean) As Boolean
        Dim result As Integer = CompareCells(reportTable.Rows(a)(columnName), reportTable.Rows(b)(columnName))

        'a tie takes from the left hand half, which is the one that was already in front. that is
        'what keeps equal rows in the order they started in
        If result = 0 Then
            Return True
        End If

        If ascending Then
            Return result < 0
        Else
            Return result > 0
        End If
    End Function

    'compares two values out of the same column and gives back -1, 0 or 1. text, dates and numbers
    'all have to be compared in their own way or the order comes out wrong
    Private Function CompareCells(valueA As Object, valueB As Object) As Integer
        'an empty cell counts as the smallest thing there is, so those rows gather at one end
        If IsDBNull(valueA) And IsDBNull(valueB) Then
            Return 0
        End If

        If IsDBNull(valueA) Then
            Return -1
        End If

        If IsDBNull(valueB) Then
            Return 1
        End If

        If TypeOf valueA Is String Then
            'ignoring the case matters here, otherwise every title starting with a capital sorts
            'in front of every title that does not
            Return String.Compare(valueA.ToString(), valueB.ToString(), True)
        End If

        If TypeOf valueA Is Date Then
            Return Date.Compare(CDate(valueA), CDate(valueB))
        End If

        'everything else on this report is a number
        Dim numberA As Double = CDbl(valueA)
        Dim numberB As Double = CDbl(valueB)

        If numberA < numberB Then
            Return -1
        ElseIf numberA > numberB Then
            Return 1
        Else
            Return 0
        End If
    End Function

    'the column holding the name on whichever report is showing. by screening has two of them, so
    'the film wins, that is the one somebody would think to type
    Private Function NameColumn() As String
        If reportTable Is Nothing Then
            Return ""
        End If

        If reportTable.Columns.Contains("FilmTitle") Then
            Return "FilmTitle"
        End If

        If reportTable.Columns.Contains("ScreenName") Then
            Return "ScreenName"
        End If

        If reportTable.Columns.Contains("FoodItemName") Then
            Return "FoodItemName"
        End If

        If reportTable.Columns.Contains("SoldBy") Then
            Return "SoldBy"
        End If

        Return ""
    End Function

    'looks a name up in the report and gives back the row it is on, or -1 if it is not there.
    '
    'this is a binary search. it looks at the middle row, and because the report is in order it
    'knows straight away which half the name has to be in, so it throws the other half away and
    'does the same again. that halving is why it only takes about five goes on thirty rows and
    'still only about ten on a thousand. it only works at all on a sorted list, which is why
    'FindInReport sorts before calling it.
    '
    'only the front of each name is compared, so typing incep finds Inception. that is still a
    'proper binary search, because comparing the fronts puts things in the same order that
    'comparing the whole names does
    Private Function FindRow(columnName As String, target As String) As Integer
        Dim low As Integer = 0
        Dim high As Integer = reportTable.Rows.Count - 1

        Do While low <= high
            Dim middle As Integer = (low + high) \ 2
            Dim here As String = reportTable.Rows(middle)(columnName).ToString()

            'only the front of the name is compared, cut to the length of what was typed
            If here.Length > target.Length Then
                here = here.Substring(0, target.Length)
            End If

            Dim result As Integer = String.Compare(here, target, True)

            If result = 0 Then
                Return middle
            ElseIf result < 0 Then
                'the middle one comes before what is wanted, so it is in the back half
                low = middle + 1
            Else
                high = middle - 1
            End If
        Loop

        Return -1
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
