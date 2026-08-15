Imports System.Data.OleDb

Public Class frmSalesReport

    'true while the form is setting itself up, so filling the show box does not run the report
    'before the dates have been put in
    Private stillLoading As Boolean = True

    'true while a quick range is filling the two date boxes in, so that does not get mistaken
    'for somebody changing a date by hand and knock the quick range back to Custom
    Private settingDates As Boolean = False

    'the little bits of help that show when the mouse rests on a card. they say the things that
    'will not fit on a card this narrow
    Private cardTips As New ToolTip

    'where each bar ended up on the chart, and what it was. they are filled in while it is being
    'drawn rather than being worked out again, so what is drawn and what the mouse finds cannot
    'disagree with each other
    Private barRects() As Rectangle
    Private barLabels() As String
    Private barValues() As Double
    Private barCount As Integer = 0

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

        'the ranges somebody would actually ask for, so the dates do not have to be picked out
        'by hand every time. Custom is what it says when they have been
        cboQuickRange.Items.Add("Today")
        cboQuickRange.Items.Add("Yesterday")
        cboQuickRange.Items.Add("This week")
        cboQuickRange.Items.Add("This month")
        cboQuickRange.Items.Add("Last month")
        cboQuickRange.Items.Add("This year")
        cboQuickRange.Items.Add("All time")
        cboQuickRange.Items.Add("Custom")

        'the dates above were already set to this month, so the box is put on the one that matches
        cboQuickRange.SelectedIndex = cboQuickRange.Items.IndexOf("This month")

        stillLoading = False

        LayoutReport()
        SetCardTips()

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

    'arranges everything that is not part of the filter bar. the grid and the labels under it are
    'worked out from how big the window is now, so the form can be resized and maximised instead
    'of being stuck at one size. the filter bar itself stays where the designer put it, apart
    'from the two buttons that are pinned to the right hand end
    Private Sub LayoutReport()
        'this fires once while the form is still being built, before there is anything to move
        If dgvSalesByFilm Is Nothing Then
            Exit Sub
        End If

        Dim edge As Integer = 16
        Dim gap As Integer = 12

        'run and export are lined up against the right hand end, one per row
        btnRunReport.Left = Me.ClientSize.Width - edge - btnRunReport.Width
        btnExport.Left = Me.ClientSize.Width - edge - btnExport.Width

        'the five cards share the width between them. the last one is pinned to the right hand
        'end rather than worked out, so the rounding off in the divide does not leave a gap
        Dim cardWidth As Integer = (Me.ClientSize.Width - edge * 2 - gap * 4) \ 5

        pnlCard1.Left = edge
        pnlCard2.Left = edge + (cardWidth + gap)
        pnlCard3.Left = edge + (cardWidth + gap) * 2
        pnlCard4.Left = edge + (cardWidth + gap) * 3
        pnlCard5.Left = Me.ClientSize.Width - edge - cardWidth

        pnlCard1.Width = cardWidth
        pnlCard2.Width = cardWidth
        pnlCard3.Width = cardWidth
        pnlCard4.Width = cardWidth
        pnlCard5.Width = cardWidth

        'the chart takes a bit over a third of the width and the grid gets the rest
        Dim chartWidth As Integer = CInt((Me.ClientSize.Width - edge * 2 - gap) * 0.34)

        dgvSalesByFilm.Left = edge
        dgvSalesByFilm.Top = pnlCard1.Bottom + gap
        dgvSalesByFilm.Width = Me.ClientSize.Width - edge * 2 - gap - chartWidth

        'what is left under the grid has to hold the row count, the three totals and the version
        dgvSalesByFilm.Height = Me.ClientSize.Height - dgvSalesByFilm.Top - 128

        pnlChart.Left = dgvSalesByFilm.Right + gap
        pnlChart.Top = dgvSalesByFilm.Top
        pnlChart.Width = chartWidth
        pnlChart.Height = dgvSalesByFilm.Height

        lblGridCount.Left = edge
        lblGridCount.Top = dgvSalesByFilm.Bottom + 6

        lblTicketRevenue.Left = edge
        lblTicketRevenue.Top = lblGridCount.Bottom + 8

        lblFoodRevenue.Left = edge
        lblFoodRevenue.Top = lblTicketRevenue.Bottom + 4

        'the big total is put over on the right so it is not buried under the other two
        lblGrandTotal.Top = lblGridCount.Bottom + 12
        lblGrandTotal.Left = Me.ClientSize.Width - edge - lblGrandTotal.Width

        lblVersion.Left = edge
        lblVersion.Top = Me.ClientSize.Height - lblVersion.Height - 8
    End Sub

    'resizing the window lays it out again
    Private Sub frmSalesReport_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize
        LayoutReport()
    End Sub

    'picking a quick range fills the two date boxes in and runs the report
    Private Sub cboQuickRange_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboQuickRange.SelectedIndexChanged
        If stillLoading Then
            Exit Sub
        End If

        'Custom is not a range, it is just what the box says once the dates have been touched
        If cboQuickRange.Text = "Custom" Then
            Exit Sub
        End If

        Dim firstDay As Date = Date.Today
        Dim lastDay As Date = Date.Today

        If cboQuickRange.Text = "Yesterday" Then
            firstDay = Date.Today.AddDays(-1)
            lastDay = firstDay

        ElseIf cboQuickRange.Text = "This week" Then
            'the week is taken as starting on a monday. windows counts sunday as 0, so taking one
            'off puts monday at 0 and leaves sunday at -1, which has to be turned into 6
            Dim daysBack As Integer = CInt(Date.Today.DayOfWeek) - 1

            If daysBack < 0 Then
                daysBack = 6
            End If

            firstDay = Date.Today.AddDays(-daysBack)

        ElseIf cboQuickRange.Text = "This month" Then
            firstDay = New Date(Date.Today.Year, Date.Today.Month, 1)

        ElseIf cboQuickRange.Text = "Last month" Then
            'the day before the first of this month is the last day of last month, whatever
            'length that month was
            Dim monthStart As Date = New Date(Date.Today.Year, Date.Today.Month, 1)
            firstDay = monthStart.AddMonths(-1)
            lastDay = monthStart.AddDays(-1)

        ElseIf cboQuickRange.Text = "This year" Then
            firstDay = New Date(Date.Today.Year, 1, 1)

        ElseIf cboQuickRange.Text = "All time" Then
            firstDay = EarliestOnRecord()
        End If

        settingDates = True
        dtpFrom.Value = firstDay
        dtpTo.Value = lastDay
        settingDates = False

        RunReport()
    End Sub

    'changing either date by hand means the range is not one of the quick ones any more
    Private Sub DatePickedByHand(sender As Object, e As EventArgs) Handles dtpFrom.ValueChanged, dtpTo.ValueChanged
        If stillLoading Or settingDates Then
            Exit Sub
        End If

        cboQuickRange.SelectedIndex = cboQuickRange.Items.IndexOf("Custom")
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

        LoadHeadline(fromDate, toDate)

        Me.Cursor = Cursors.Default
        ShowCount()

        'the totals are autosize labels, so they are a different width once they have been
        'written into and the right hand one has to be lined up again
        LayoutReport()

        Return True
    End Function

    'fills in the five cards along the top. these are always the same five whichever report is
    'showing, because they are about the date range and not about the way it is broken down.
    'they all go through one connection, the way the main menu does its dashboard
    Private Sub LoadHeadline(fromDate As Date, toDate As Date)
        Dim tickets As Integer = 0
        Dim ticketMoney As Double = 0
        Dim foodMoney As Double = 0
        Dim bookings As Integer = 0
        Dim seatsOnOffer As Integer = 0
        Dim seatsSold As Integer = 0

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn

            'tickets and what they came to
            SQLCmd.CommandText = "SELECT COUNT(*) AS Tickets, SUM(SeatPricePaid) AS TicketRevenue " &
                                 "FROM (tblBookingSeat INNER JOIN tblBooking ON tblBookingSeat.BookingID = tblBooking.BookingID) " &
                                 "INNER JOIN tblScreening ON tblBooking.ScreeningID = tblScreening.ScreeningID " &
                                 "WHERE IIf(@ByScreening, tblScreening.ScreeningDate, tblBooking.BookingDate) >= @FromDate " &
                                 "AND IIf(@ByScreening2, tblScreening.ScreeningDate, tblBooking.BookingDate) < @ToDate " &
                                 "AND tblBooking.BookingStatus <> @Cancelled"
            AddRangeParameters(SQLCmd, fromDate, toDate)

            Dim rs As OleDbDataReader = SQLCmd.ExecuteReader()

            If rs.Read() Then
                tickets = CInt(rs("Tickets"))

                'SUM comes back empty rather than zero when there are no rows at all
                If Not IsDBNull(rs("TicketRevenue")) Then
                    ticketMoney = CDbl(rs("TicketRevenue"))
                End If
            End If

            rs.Close()

            'concessions
            SQLCmd.Parameters.Clear()
            SQLCmd.CommandText = "SELECT SUM(Quantity * ItemPricePaid) " &
                                 "FROM (tblOrderItem INNER JOIN tblBooking ON tblOrderItem.BookingID = tblBooking.BookingID) " &
                                 "INNER JOIN tblScreening ON tblBooking.ScreeningID = tblScreening.ScreeningID " &
                                 "WHERE IIf(@ByScreening, tblScreening.ScreeningDate, tblBooking.BookingDate) >= @FromDate " &
                                 "AND IIf(@ByScreening2, tblScreening.ScreeningDate, tblBooking.BookingDate) < @ToDate " &
                                 "AND tblBooking.BookingStatus <> @Cancelled"
            AddRangeParameters(SQLCmd, fromDate, toDate)
            Dim foodResult As Object = SQLCmd.ExecuteScalar()

            If foodResult IsNot Nothing AndAlso Not IsDBNull(foodResult) Then
                foodMoney = CDbl(foodResult)
            End If

            'how many sales that was
            SQLCmd.Parameters.Clear()
            SQLCmd.CommandText = "SELECT COUNT(*) " &
                                 "FROM tblBooking INNER JOIN tblScreening ON tblBooking.ScreeningID = tblScreening.ScreeningID " &
                                 "WHERE IIf(@ByScreening, tblScreening.ScreeningDate, tblBooking.BookingDate) >= @FromDate " &
                                 "AND IIf(@ByScreening2, tblScreening.ScreeningDate, tblBooking.BookingDate) < @ToDate " &
                                 "AND tblBooking.BookingStatus <> @Cancelled"
            AddRangeParameters(SQLCmd, fromDate, toDate)
            bookings = CInt(SQLCmd.ExecuteScalar())

            'how full is worked out against the screenings that played in the range, whichever
            'way the measure by box is set. seats sold on a booking taken last month for a film
            'showing this month belong to this month as far as how full the room was goes
            SQLCmd.Parameters.Clear()
            SQLCmd.CommandText = "SELECT SUM(tblScreen.ScreenCapacity) " &
                                 "FROM tblScreening INNER JOIN tblScreen ON tblScreening.ScreenID = tblScreen.ScreenID " &
                                 "WHERE tblScreening.ScreeningDate >= @FromDate AND tblScreening.ScreeningDate < @ToDate"
            SQLCmd.Parameters.AddWithValue("@FromDate", fromDate)
            SQLCmd.Parameters.AddWithValue("@ToDate", PeriodEnd(toDate))
            Dim capacityResult As Object = SQLCmd.ExecuteScalar()

            If capacityResult IsNot Nothing AndAlso Not IsDBNull(capacityResult) Then
                seatsOnOffer = CInt(capacityResult)
            End If

            'the seat rows carry the screening themselves, so this needs no join back to the
            'booking, and no cancelled filter either because cancelling deletes these rows
            SQLCmd.Parameters.Clear()
            SQLCmd.CommandText = "SELECT COUNT(*) " &
                                 "FROM tblBookingSeat INNER JOIN tblScreening ON tblBookingSeat.ScreeningID = tblScreening.ScreeningID " &
                                 "WHERE tblScreening.ScreeningDate >= @FromDate AND tblScreening.ScreeningDate < @ToDate"
            SQLCmd.Parameters.AddWithValue("@FromDate", fromDate)
            SQLCmd.Parameters.AddWithValue("@ToDate", PeriodEnd(toDate))
            seatsSold = CInt(SQLCmd.ExecuteScalar())

            cn.Close()
        End If

        lblStat1.Text = FormatCurrency(ticketMoney + foodMoney)
        'just the two figures, the card is not wide enough to say which is which. the tip does
        lblCardSub1.Text = FormatCurrency(ticketMoney) & " + " & FormatCurrency(foodMoney)

        lblStat2.Text = tickets.ToString()

        If bookings = 1 Then
            lblCardSub2.Text = "on 1 booking"
        Else
            lblCardSub2.Text = "on " & bookings & " bookings"
        End If

        'nothing sold means there is nothing to average, and dividing by it would fall over
        If tickets > 0 Then
            lblStat3.Text = FormatCurrency(ticketMoney / tickets)
            lblStat5.Text = FormatCurrency(foodMoney / tickets)
        Else
            lblStat3.Text = "-"
            lblStat5.Text = "-"
        End If

        lblCardSub3.Text = "per seat sold"
        lblCardSub5.Text = "for every ticket"

        If seatsOnOffer > 0 Then
            lblStat4.Text = CInt((seatsSold / seatsOnOffer) * 100) & "%"
            lblCardSub4.Text = seatsSold & " of " & seatsOnOffer & " seats"
        Else
            lblStat4.Text = "-"
            lblCardSub4.Text = "nothing was on"
        End If
    End Sub

    'puts the same bit of help on a card and all three of its labels, so it shows wherever the
    'mouse happens to be sitting on the card
    Private Sub TipCard(card As Panel, title As Label, value As Label, sub1 As Label, message As String)
        cardTips.SetToolTip(card, message)
        cardTips.SetToolTip(title, message)
        cardTips.SetToolTip(value, message)
        cardTips.SetToolTip(sub1, message)
    End Sub

    'explains what the five figures along the top actually mean
    Private Sub SetCardTips()
        cardTips.AutoPopDelay = 8000
        cardTips.InitialDelay = 500

        TipCard(pnlCard1, lblCardTitle1, lblStat1, lblCardSub1, "Everything taken in this range, tickets plus concessions. The small line is tickets first, then concessions.")
        TipCard(pnlCard2, lblCardTitle2, lblStat2, lblCardSub2, "How many seats were sold in this range.")
        TipCard(pnlCard3, lblCardTitle3, lblStat3, lblCardSub3, "What the average seat sold for. Premium and accessible seats cost a different amount, so this sits somewhere between them.")
        TipCard(pnlCard4, lblCardTitle4, lblStat4, lblCardSub4, "How full the screenings that played in this range were. This one always goes on the screening date, even when the report is measured on the booking date, because a seat sold last month for a film showing this month still filled a seat this month. That is why this count can differ from the tickets sold card.")
        TipCard(pnlCard5, lblCardTitle5, lblStat5, lblCardSub5, "How much was spent on food and drink for every ticket sold.")
    End Sub

    'the three parameters every one of the range queries wants, put on in the order the query
    'mentions them. it is written once here because five queries were all doing the same thing
    Private Sub AddRangeParameters(SQLCmd As OleDbCommand, fromDate As Date, toDate As Date)
        SQLCmd.Parameters.AddWithValue("@ByScreening", MeasuringByScreening())
        SQLCmd.Parameters.AddWithValue("@FromDate", fromDate)
        SQLCmd.Parameters.AddWithValue("@ByScreening2", MeasuringByScreening())
        SQLCmd.Parameters.AddWithValue("@ToDate", PeriodEnd(toDate))
        SQLCmd.Parameters.AddWithValue("@Cancelled", BookingCancelled)
    End Sub

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

    'the number columns are given a width so their headings do not wrap onto two lines. only the
    'one text column is left filling whatever space is left over
    Private Sub SetColumnWidths()
        SetWidth("BookingID", 90)
        SetWidth("CustomerName", 190)
        SetWidth("ScreeningDate", 100)
        SetWidth("ScreeningTime", 70)
        SetWidth("ScreenName", 110)
        SetWidth("ReportDay", 110)
        SetWidth("Sold", 70)
        SetWidth("Tickets", 95)
        SetWidth("TicketRevenue", 110)
        SetWidth("FoodRevenue", 110)
        SetWidth("TotalCost", 100)
        SetWidth("Total", 100)
    End Sub

    'sets one column width, but only if this report has that column and it is not the one that
    'has been left to fill the space. a filling column works its own width out
    Private Sub SetWidth(columnName As String, wide As Integer)
        If dgvSalesByFilm.Columns.Contains(columnName) Then
            If dgvSalesByFilm.Columns(columnName).AutoSizeMode <> DataGridViewAutoSizeColumnMode.Fill Then
                dgvSalesByFilm.Columns(columnName).Width = wide
            End If
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
        'the old columns are thrown away first. if the report being swapped to has a column with
        'the same name as one the last report had, the grid keeps the old column, and it keeps
        'where that column was sitting as well. going from tickets and concessions to by day left
        'ticket revenue in front of tickets sold, because that is where it had been before
        dgvSalesByFilm.DataSource = Nothing
        dgvSalesByFilm.Columns.Clear()

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

        SetColumnWidths()

        'the grid is told not to sort itself. the sorting is done in code further down, and if the
        'grid also sorted then clicking a heading would run both and they would fight over the order
        For Each col As DataGridViewColumn In dgvSalesByFilm.Columns
            col.SortMode = DataGridViewColumnSortMode.Programmatic
        Next

        ShowSortArrow()
        dgvSalesByFilm.ClearSelection()

        'the chart is drawn off the same table, so it has to be redrawn whenever it changes
        pnlChart.Invalidate()
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

    'the panel asks for this whenever it needs repainting, which is after every report and
    'whenever the colour scheme changes
    Private Sub pnlChart_Paint(sender As Object, e As PaintEventArgs) Handles pnlChart.Paint
        DrawChart(e.Graphics, pnlChart.Width, pnlChart.Height)
    End Sub

    'draws the chart.
    '
    'everything is worked out from the width and height it is handed rather than from the panel,
    'so the same routine can draw onto the screen, onto a printed page or into a picture file.
    'three copies of this that each drew it slightly differently would be a lot worse
    Private Sub DrawChart(g As Graphics, wide As Integer, high As Integer)
        'this can be asked to draw before the form has finished loading, so the colours are
        'filled in first rather than being left as nothing
        SetThemeColours()

        g.Clear(GridBack)
        g.DrawRectangle(New Pen(BorderCol), 0, 0, wide - 1, high - 1)

        Dim titleFont As New Font("Segoe UI", 9.75, FontStyle.Bold)
        Dim smallFont As New Font("Segoe UI", 8.25)
        Dim faded As New SolidBrush(SubtleFore)

        barCount = 0

        If reportTable Is Nothing OrElse reportTable.Rows.Count = 0 Then
            g.DrawString("Nothing to chart", smallFont, faded, 12, 12)
            Exit Sub
        End If

        Dim labelColumn As String = ChartLabelColumn()
        Dim valueColumn As String = ChartValueColumn()

        If labelColumn = "" Or valueColumn = "" Then
            g.DrawString("This report has nothing to chart", smallFont, faded, 12, 12)
            Exit Sub
        End If

        g.DrawString(ChartTitle(), titleFont, New SolidBrush(TextFore), 10, 8)

        If cboReportType.Text = "By day" Then
            DrawDayChart(g, wide, high, labelColumn, valueColumn, smallFont, faded)
        Else
            DrawBarChart(g, wide, high, labelColumn, valueColumn, smallFont, faded)
        End If
    End Sub

    'a bar per row, lying on its side so the names have somewhere to go. the rows are taken in
    'whatever order the grid is showing them in, so sorting the grid reorders the chart as well
    Private Sub DrawBarChart(g As Graphics, wide As Integer, high As Integer, labelColumn As String, valueColumn As String, smallFont As Font, faded As Brush)
        Dim top As Integer = 32
        Dim room As Integer = high - top - 10

        'a bar thinner than this cannot be read, so only as many are drawn as will fit
        Dim barHeight As Integer = 22
        Dim howMany As Integer = room \ barHeight

        If howMany > reportTable.Rows.Count Then
            howMany = reportTable.Rows.Count
        End If

        If howMany < 1 Then
            g.DrawString("Not enough room to draw this", smallFont, faded, 12, top)
            Exit Sub
        End If

        Dim biggest As Double = BiggestValue(valueColumn, howMany)

        If biggest <= 0 Then
            g.DrawString("Nothing was taken in this range", smallFont, faded, 12, top)
            Exit Sub
        End If

        'room down the left for the names, but never more than about half the panel
        Dim nameRoom As Integer = CInt(wide * 0.42)

        If nameRoom > 140 Then
            nameRoom = 140
        End If

        Dim barLeft As Integer = nameRoom + 6
        Dim barRoom As Integer = wide - barLeft - 10

        ReDim barRects(howMany - 1)
        ReDim barLabels(howMany - 1)
        ReDim barValues(howMany - 1)
        barCount = howMany

        For i As Integer = 0 To howMany - 1
            Dim value As Double = RowValue(i, valueColumn)
            Dim name As String = RowLabel(i, labelColumn)
            Dim y As Integer = top + (i * barHeight)

            'the longest bar fills the room, everything else is drawn against that
            Dim length As Integer = CInt((value / biggest) * barRoom)

            'something that sold anything at all still gets a sliver, so it does not look like
            'it sold nothing
            If length < 1 And value > 0 Then
                length = 1
            End If

            Dim paint As Color = AccentFore

            'the best one is picked out in the brand pink so it is obvious at a glance
            If value >= biggest Then
                paint = HighlightBack
            End If

            g.FillRectangle(New SolidBrush(paint), barLeft, y + 3, length, barHeight - 8)

            g.DrawString(FitText(g, name, smallFont, nameRoom - 12), smallFont, New SolidBrush(TextFore), 10, y + 3)

            'the amount goes just past the end of the bar, unless there is no room left, in
            'which case it goes inside it
            Dim amount As String = ChartValueText(value)
            Dim amountWidth As Integer = CInt(g.MeasureString(amount, smallFont).Width)

            If barLeft + length + amountWidth + 4 < wide Then
                g.DrawString(amount, smallFont, faded, barLeft + length + 4, y + 3)
            ElseIf length > amountWidth + 8 Then
                'the longest bar reaches the edge, so its amount goes inside it instead. the
                'writing is done in the grid background colour, which is the one colour that is
                'always the opposite of the bar, whichever way round the theme is
                g.DrawString(amount, smallFont, New SolidBrush(GridBack), barLeft + length - amountWidth - 4, y + 3)
            End If

            barRects(i) = New Rectangle(barLeft, y, barRoom, barHeight)
            barLabels(i) = name
            barValues(i) = value
        Next

        'if there was not room for all of them, say so rather than quietly leaving them off
        If howMany < reportTable.Rows.Count Then
            g.DrawString("+ " & (reportTable.Rows.Count - howMany) & " more, make the window taller", smallFont, faded, 10, top + (howMany * barHeight) + 2)
        End If
    End Sub

    'standing up rather than lying down, because a day chart reads left to right
    Private Sub DrawDayChart(g As Graphics, wide As Integer, high As Integer, labelColumn As String, valueColumn As String, smallFont As Font, faded As Brush)
        Dim top As Integer = 32
        Dim bottom As Integer = high - 24
        Dim left As Integer = 10
        Dim across As Integer = wide - left - 10
        Dim howMany As Integer = reportTable.Rows.Count

        Dim columnWidth As Integer = across \ howMany

        If columnWidth < 2 Or bottom - top < 20 Then
            g.DrawString("Too many days to draw here", smallFont, faded, 12, top)
            Exit Sub
        End If

        Dim biggest As Double = BiggestValue(valueColumn, howMany)

        If biggest <= 0 Then
            g.DrawString("Nothing was taken in this range", smallFont, faded, 12, top)
            Exit Sub
        End If

        'the line the columns stand on
        g.DrawLine(New Pen(BorderCol), left, bottom, left + (howMany * columnWidth), bottom)

        ReDim barRects(howMany - 1)
        ReDim barLabels(howMany - 1)
        ReDim barValues(howMany - 1)
        barCount = howMany

        'a gap between the columns, but not on the thin ones or there would be nothing left
        Dim gap As Integer = 2

        If columnWidth < 8 Then
            gap = 1
        End If

        For i As Integer = 0 To howMany - 1
            Dim value As Double = RowValue(i, valueColumn)
            Dim x As Integer = left + (i * columnWidth)
            Dim tall As Integer = CInt((value / biggest) * (bottom - top))

            If tall < 1 And value > 0 Then
                tall = 1
            End If

            Dim paint As Color = AccentFore

            If value >= biggest Then
                paint = HighlightBack
            End If

            If tall > 0 Then
                g.FillRectangle(New SolidBrush(paint), x, bottom - tall, columnWidth - gap, tall)
            End If

            barRects(i) = New Rectangle(x, top, columnWidth, bottom - top)
            barLabels(i) = RowLabel(i, labelColumn)
            barValues(i) = value
        Next

        'the first and last day are written under the ends, the rest would not fit
        g.DrawString(ShortDay(0), smallFont, faded, left, bottom + 4)

        Dim lastText As String = ShortDay(howMany - 1)
        Dim lastWidth As Integer = CInt(g.MeasureString(lastText, smallFont).Width)
        g.DrawString(lastText, smallFont, faded, wide - 10 - lastWidth, bottom + 4)
    End Sub

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

    'the biggest value in the first so many rows, which is what the bars are scaled against.
    'only the rows being drawn are looked at, otherwise a big one further down the list that is
    'not on the chart would squash everything that is
    Private Function BiggestValue(valueColumn As String, howMany As Integer) As Double
        Dim biggest As Double = 0

        For i As Integer = 0 To howMany - 1
            If RowValue(i, valueColumn) > biggest Then
                biggest = RowValue(i, valueColumn)
            End If
        Next

        Return biggest
    End Function

    'one value off the report, with an empty cell counting as nothing
    Private Function RowValue(rowIndex As Integer, valueColumn As String) As Double
        If IsDBNull(reportTable.Rows(rowIndex)(valueColumn)) Then
            Return 0
        End If

        Return CDbl(reportTable.Rows(rowIndex)(valueColumn))
    End Function

    'what to write beside a bar
    Private Function RowLabel(rowIndex As Integer, labelColumn As String) As String
        If IsDBNull(reportTable.Rows(rowIndex)(labelColumn)) Then
            Return ""
        End If

        If labelColumn = "ReportDay" Then
            Return CDate(reportTable.Rows(rowIndex)(labelColumn)).ToString("dd/MM/yyyy")
        End If

        Return reportTable.Rows(rowIndex)(labelColumn).ToString()
    End Function

    'the short form of a day, for the two ends of the day chart
    Private Function ShortDay(rowIndex As Integer) As String
        Return CDate(reportTable.Rows(rowIndex)("ReportDay")).ToString("dd/MM")
    End Function

    'the cancellations report counts bookings, everything else counts money
    Private Function ChartValueText(value As Double) As String
        Return FormatCurrency(value)
    End Function

    'the column the bars are sized from. the reports that show both put the two together in a
    'Total column, so that one is looked for first
    Private Function ChartValueColumn() As String
        If reportTable Is Nothing Then
            Return ""
        End If

        If reportTable.Columns.Contains("Total") Then
            Return "Total"
        End If

        If reportTable.Columns.Contains("TicketRevenue") Then
            Return "TicketRevenue"
        End If

        If reportTable.Columns.Contains("FoodRevenue") Then
            Return "FoodRevenue"
        End If

        If reportTable.Columns.Contains("TotalCost") Then
            Return "TotalCost"
        End If

        Return ""
    End Function

    'what goes along the bottom or down the side of the chart
    Private Function ChartLabelColumn() As String
        If reportTable Is Nothing Then
            Return ""
        End If

        If reportTable.Columns.Contains("ReportDay") Then
            Return "ReportDay"
        End If

        Return NameColumn()
    End Function

    'what the chart is showing, written across the top of it
    Private Function ChartTitle() As String
        If cboReportType.Text = "By day" Then
            Return "Ticket revenue by day"
        ElseIf cboReportType.Text = "Concessions only" Then
            Return "Revenue by item"
        ElseIf cboReportType.Text = "By screen" Then
            Return "Ticket revenue by screen"
        ElseIf cboReportType.Text = "By screening" Then
            Return "Ticket revenue by screening"
        ElseIf cboReportType.Text = "By staff member" Then
            Return "Ticket revenue by person"
        ElseIf cboReportType.Text = "Cancellations" Then
            Return "Refunded"
        ElseIf cboReportType.Text = "Tickets only" Then
            Return "Ticket revenue by film"
        Else
            Return "Takings by film"
        End If
    End Function

    'cuts a name down with dots on the end until it fits the room there is for it
    Private Function FitText(g As Graphics, text As String, useFont As Font, room As Integer) As String
        If g.MeasureString(text, useFont).Width <= room Then
            Return text
        End If

        Dim shorter As String = text

        Do While shorter.Length > 1 AndAlso g.MeasureString(shorter & "...", useFont).Width > room
            shorter = shorter.Substring(0, shorter.Length - 1)
        Loop

        Return shorter & "..."
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

    'the oldest date there is anything on record for, so All time really does cover all of it.
    'both tables are asked, because the report can be measured against either date and the
    'oldest screening is not necessarily on the same day as the oldest booking
    Private Function EarliestOnRecord() As Date
        Dim earliest As Date = New Date(Date.Today.Year, 1, 1)

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT MIN(BookingDate) FROM tblBooking"
            Dim oldestBooking As Object = SQLCmd.ExecuteScalar()

            SQLCmd.CommandText = "SELECT MIN(ScreeningDate) FROM tblScreening"
            Dim oldestScreening As Object = SQLCmd.ExecuteScalar()
            cn.Close()

            If oldestBooking IsNot Nothing AndAlso Not IsDBNull(oldestBooking) Then
                earliest = CDate(oldestBooking)
            End If

            If oldestScreening IsNot Nothing AndAlso Not IsDBNull(oldestScreening) Then
                If CDate(oldestScreening) < earliest Then
                    earliest = CDate(oldestScreening)
                End If
            End If
        End If

        Return earliest.Date
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
