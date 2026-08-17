Imports System.Data.OleDb

Public Class frmSalesReport

    Private stillLoading As Boolean = True

    Private settingDates As Boolean = False

    Private cardTips As New ToolTip

    Private reportTable As DataTable

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

        dtpFrom.Value = New Date(Date.Now.Year, Date.Now.Month, 1)
        dtpTo.Value = Date.Now.Date

        cboReportType.Items.Add("Tickets and concessions")
        cboReportType.Items.Add("Tickets only")
        cboReportType.Items.Add("Concessions only")
        cboReportType.Items.Add("By day")
        cboReportType.Items.Add("By screening")
        cboReportType.Items.Add("By screen")
        cboReportType.Items.Add("By staff member")
        cboReportType.Items.Add("Refunds")
        cboReportType.SelectedIndex = 0

        cboMeasureBy.Items.Add("Booking date")
        cboMeasureBy.Items.Add("Screening date")
        cboMeasureBy.SelectedIndex = 0

        cboQuickRange.Items.Add("Today")
        cboQuickRange.Items.Add("Yesterday")
        cboQuickRange.Items.Add("This week")
        cboQuickRange.Items.Add("This month")
        cboQuickRange.Items.Add("Last month")
        cboQuickRange.Items.Add("This year")
        cboQuickRange.Items.Add("All time")
        cboQuickRange.Items.Add("Custom")

        cboQuickRange.SelectedIndex = cboQuickRange.Items.IndexOf("This month")

        stillLoading = False

        LayoutReport()
        SetCardTips()

        RunReport()
        WriteLog("REPORT", "Sales report form opened")
    End Sub

    Private Sub cboReportType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboReportType.SelectedIndexChanged
        If stillLoading Then
            Exit Sub
        End If

        RunReport()
    End Sub

    Private Sub LayoutReport()
        If dgvSalesByFilm Is Nothing Then
            Exit Sub
        End If

        Dim edge As Integer = 16
        Dim gap As Integer = 12

        btnRunReport.Left = Me.ClientSize.Width - edge - btnRunReport.Width
        btnExport.Left = Me.ClientSize.Width - edge - btnExport.Width
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

        dgvSalesByFilm.Left = edge
        dgvSalesByFilm.Top = pnlCard1.Bottom + gap
        dgvSalesByFilm.Width = Me.ClientSize.Width - edge * 2

        dgvSalesByFilm.Height = Me.ClientSize.Height - dgvSalesByFilm.Top - 112

        lblGridCount.Left = edge
        lblGridCount.Top = dgvSalesByFilm.Bottom + 6

        lblTicketRevenue.Left = edge
        lblTicketRevenue.Top = lblGridCount.Bottom + 8

        lblFoodRevenue.Left = edge
        lblFoodRevenue.Top = lblTicketRevenue.Bottom + 4

        lblGrandTotal.Top = lblGridCount.Bottom + 12
        lblGrandTotal.Left = Me.ClientSize.Width - edge - lblGrandTotal.Width

        lblVersion.Left = edge
        lblVersion.Top = Me.ClientSize.Height - lblVersion.Height - 8
    End Sub

    Private Sub frmSalesReport_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize
        LayoutReport()
    End Sub

    Private Sub cboQuickRange_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboQuickRange.SelectedIndexChanged
        If stillLoading Then
            Exit Sub
        End If

        If cboQuickRange.Text = "Custom" Then
            Exit Sub
        End If

        Dim firstDay As Date = Date.Today
        Dim lastDay As Date = Date.Today

        If cboQuickRange.Text = "Yesterday" Then
            firstDay = Date.Today.AddDays(-1)
            lastDay = firstDay

        ElseIf cboQuickRange.Text = "This week" Then
            Dim daysBack As Integer = CInt(Date.Today.DayOfWeek) - 1

            If daysBack < 0 Then
                daysBack = 6
            End If

            firstDay = Date.Today.AddDays(-daysBack)

        ElseIf cboQuickRange.Text = "This month" Then
            firstDay = New Date(Date.Today.Year, Date.Today.Month, 1)

        ElseIf cboQuickRange.Text = "Last month" Then
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

    Private Sub DatePickedByHand(sender As Object, e As EventArgs) Handles dtpFrom.ValueChanged, dtpTo.ValueChanged
        If stillLoading Or settingDates Then
            Exit Sub
        End If

        cboQuickRange.SelectedIndex = cboQuickRange.Items.IndexOf("Custom")
    End Sub

    Private Sub cboMeasureBy_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboMeasureBy.SelectedIndexChanged
        If stillLoading Then
            Exit Sub
        End If

        RunReport()
    End Sub

    Private Sub btnRunReport_Click(sender As Object, e As EventArgs) Handles btnRunReport.Click
        If RunReport() Then
            WriteLog("REPORT", "Sales report run (" & cboReportType.Text & ") for " & dtpFrom.Value.ToShortDateString() & " to " & dtpTo.Value.ToShortDateString())
        End If
    End Sub

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        If reportTable Is Nothing OrElse reportTable.Rows.Count = 0 Then
            MessageBox.Show("There is nothing on screen to export.", "Export", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Dim saveBox As New SaveFileDialog
        saveBox.Filter = "CSV files (*.csv)|*.csv"
        saveBox.RestoreDirectory = True
        saveBox.FileName = ExportFileName()

        If saveBox.ShowDialog() <> DialogResult.OK Then
            Exit Sub
        End If

        If Not WriteCsv(saveBox.FileName) Then
            Exit Sub
        End If

        WriteLog("REPORT", "Sales report exported (" & cboReportType.Text & "), " & reportTable.Rows.Count & " rows")
        MessageBox.Show(reportTable.Rows.Count & " rows saved.", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Function WriteCsv(fileName As String) As Boolean
        Dim writer As IO.StreamWriter

        Try
            writer = New IO.StreamWriter(fileName, False, New System.Text.UTF8Encoding(True))
        Catch ex As Exception
            MessageBox.Show("That file could not be written to. If it is open in another program, close it and try again." & vbCrLf & vbCrLf & ex.Message,
                            "Export", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try

        writer.WriteLine(CsvField("Filmtopia sales report"))
        writer.WriteLine(CsvField("Report") & "," & CsvField(cboReportType.Text))
        writer.WriteLine(CsvField("Measured by") & "," & CsvField(cboMeasureBy.Text))
        writer.WriteLine(CsvField("From") & "," & CsvField(dtpFrom.Value.ToString("dd/MM/yyyy")))
        writer.WriteLine(CsvField("To") & "," & CsvField(dtpTo.Value.ToString("dd/MM/yyyy")))
        writer.WriteLine(CsvField("Taken out") & "," & CsvField(Now().ToString("dd/MM/yyyy HH:mm")))
        writer.WriteLine(CsvField("Taken out by") & "," & CsvField(CurrentLogUser()))
        writer.WriteLine()

        Dim line As String = ""

        For Each col As DataGridViewColumn In dgvSalesByFilm.Columns
            If line <> "" Then
                line = line & ","
            End If

            line = line & CsvField(col.HeaderText)
        Next

        writer.WriteLine(line)

        For i As Integer = 0 To reportTable.Rows.Count - 1
            line = ""

            For Each col As DataGridViewColumn In dgvSalesByFilm.Columns
                If line <> "" Then
                    line = line & ","
                End If

                line = line & CsvField(ExportValue(i, col.DataPropertyName))
            Next

            writer.WriteLine(line)
        Next

        writer.WriteLine()
        line = ""

        For Each col As DataGridViewColumn In dgvSalesByFilm.Columns
            If line <> "" Then
                line = line & ","
            End If

            If col.Index = 0 Then
                line = line & CsvField("Total")
            ElseIf IsNumberColumn(col.DataPropertyName) Then
                line = line & CsvField(TotalColumn(reportTable, col.DataPropertyName).ToString())
            Else
                line = line & CsvField("")
            End If
        Next

        writer.WriteLine(line)
        writer.Close()

        Return True
    End Function

    Private Function ExportFileName() As String
        Return "Sales " & cboReportType.Text.Replace(" ", "") & " " & dtpFrom.Value.ToString("yyyy-MM-dd") & " to " & dtpTo.Value.ToString("yyyy-MM-dd") & ".csv"
    End Function

    Private Function ExportValue(rowIndex As Integer, columnName As String) As String
        If columnName = "" OrElse Not reportTable.Columns.Contains(columnName) Then
            Return ""
        End If

        Dim cell As Object = reportTable.Rows(rowIndex)(columnName)

        If IsDBNull(cell) Then
            Return ""
        End If

        If TypeOf cell Is Date Then
            If columnName = "CancelledDate" Then
                Return CDate(cell).ToString("dd/MM/yyyy HH:mm")
            End If

            Return CDate(cell).ToString("dd/MM/yyyy")
        End If

        Return cell.ToString()
    End Function

    Private Function IsNumberColumn(columnName As String) As Boolean
        If columnName = "" OrElse Not reportTable.Columns.Contains(columnName) Then
            Return False
        End If

        If columnName = "BookingID" Then
            Return False
        End If

        Dim kind As Type = reportTable.Columns(columnName).DataType

        Return kind Is GetType(Double) Or kind Is GetType(Integer) Or kind Is GetType(Decimal)
    End Function

    Private Sub btnFind_Click(sender As Object, e As EventArgs) Handles btnFind.Click
        FindInReport()
    End Sub

    Private Sub txtFind_KeyDown(sender As Object, e As KeyEventArgs) Handles txtFind.KeyDown
        If e.KeyCode = Keys.Enter Then
            FindInReport()

            e.SuppressKeyPress = True
        End If
    End Sub

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

    Private Function RunReport() As Boolean
        If dtpFrom.Value.Date > dtpTo.Value.Date Then
            MessageBox.Show("From date cant be after the to date", "Sales Report", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If

        Dim fromDate As Date = dtpFrom.Value.Date
        Dim toDate As Date = dtpTo.Value.Date

        Me.Cursor = Cursors.WaitCursor

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

        ElseIf cboReportType.Text = "Refunds" Then
            Dim refunded As Double = LoadRefunds(fromDate, toDate)

            lblTicketRevenue.Visible = False
            lblFoodRevenue.Visible = True
            lblFoodRevenue.Text = "Money paid back, so it is out of the takings"
            lblGrandTotal.Text = "Refunded: " & FormatCurrency(refunded)

        Else
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

        LayoutReport()

        Return True
    End Function

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

                If Not IsDBNull(rs("TicketRevenue")) Then
                    ticketMoney = CDbl(rs("TicketRevenue"))
                End If
            End If

            rs.Close()

            SQLCmd.Parameters.Clear()
            SQLCmd.CommandText = "SELECT SUM(Quantity * ItemPricePaid) " &
                                 "FROM (tblOrderItem INNER JOIN tblBooking ON tblOrderItem.BookingID = tblBooking.BookingID) " &
                                 "LEFT JOIN tblScreening ON tblBooking.ScreeningID = tblScreening.ScreeningID " &
                                 "WHERE IIf(@ByScreening, IIf(IsNull(tblScreening.ScreeningDate), tblBooking.BookingDate, tblScreening.ScreeningDate), tblBooking.BookingDate) >= @FromDate " &
                                 "AND IIf(@ByScreening2, IIf(IsNull(tblScreening.ScreeningDate), tblBooking.BookingDate, tblScreening.ScreeningDate), tblBooking.BookingDate) < @ToDate"
            AddDateParameters(SQLCmd, fromDate, toDate)
            Dim foodResult As Object = SQLCmd.ExecuteScalar()

            If foodResult IsNot Nothing AndAlso Not IsDBNull(foodResult) Then
                foodMoney = CDbl(foodResult)
            End If

            SQLCmd.Parameters.Clear()
            SQLCmd.CommandText = "SELECT SUM(tblRefundLine.AmountRefunded) " &
                                 "FROM ((tblRefundLine INNER JOIN tblOrderItem ON tblRefundLine.OrderItemID = tblOrderItem.OrderItemID) " &
                                 "INNER JOIN tblBooking ON tblOrderItem.BookingID = tblBooking.BookingID) " &
                                 "LEFT JOIN tblScreening ON tblBooking.ScreeningID = tblScreening.ScreeningID " &
                                 "WHERE IIf(@ByScreening, IIf(IsNull(tblScreening.ScreeningDate), tblBooking.BookingDate, tblScreening.ScreeningDate), tblBooking.BookingDate) >= @FromDate " &
                                 "AND IIf(@ByScreening2, IIf(IsNull(tblScreening.ScreeningDate), tblBooking.BookingDate, tblScreening.ScreeningDate), tblBooking.BookingDate) < @ToDate"
            AddDateParameters(SQLCmd, fromDate, toDate)
            Dim foodBackResult As Object = SQLCmd.ExecuteScalar()

            If foodBackResult IsNot Nothing AndAlso Not IsDBNull(foodBackResult) Then
                foodMoney = foodMoney - CDbl(foodBackResult)
            End If

            SQLCmd.Parameters.Clear()
            SQLCmd.CommandText = "SELECT COUNT(*) " &
                                 "FROM tblBooking LEFT JOIN tblScreening ON tblBooking.ScreeningID = tblScreening.ScreeningID " &
                                 "WHERE IIf(@ByScreening, IIf(IsNull(tblScreening.ScreeningDate), tblBooking.BookingDate, tblScreening.ScreeningDate), tblBooking.BookingDate) >= @FromDate " &
                                 "AND IIf(@ByScreening2, IIf(IsNull(tblScreening.ScreeningDate), tblBooking.BookingDate, tblScreening.ScreeningDate), tblBooking.BookingDate) < @ToDate " &
                                 "AND tblBooking.BookingStatus <> @Cancelled"
            AddRangeParameters(SQLCmd, fromDate, toDate)
            bookings = CInt(SQLCmd.ExecuteScalar())

            SQLCmd.Parameters.Clear()
            SQLCmd.CommandText = "SELECT SUM(tblScreen.ScreenCapacity) " &
                                 "FROM tblScreening INNER JOIN tblScreen ON tblScreening.ScreenID = tblScreen.ScreenID " &
                                 "WHERE tblScreening.ScreeningDate >= @FromDate AND tblScreening.ScreeningDate < @ToDate " &
                                 "AND (tblScreening.ScreeningStatus IS NULL OR tblScreening.ScreeningStatus <> @Cancelled)"
            SQLCmd.Parameters.AddWithValue("@FromDate", fromDate)
            SQLCmd.Parameters.AddWithValue("@ToDate", PeriodEnd(toDate))
            SQLCmd.Parameters.AddWithValue("@Cancelled", ScreeningCancelled)
            Dim capacityResult As Object = SQLCmd.ExecuteScalar()

            If capacityResult IsNot Nothing AndAlso Not IsDBNull(capacityResult) Then
                seatsOnOffer = CInt(capacityResult)
            End If

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
        lblCardSub1.Text = FormatCurrency(ticketMoney) & " + " & FormatCurrency(foodMoney)

        lblStat2.Text = tickets.ToString()

        If bookings = 1 Then
            lblCardSub2.Text = "on 1 booking"
        Else
            lblCardSub2.Text = "on " & bookings & " bookings"
        End If

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

    Private Sub TipCard(card As Panel, title As Label, value As Label, sub1 As Label, message As String)
        cardTips.SetToolTip(card, message)
        cardTips.SetToolTip(title, message)
        cardTips.SetToolTip(value, message)
        cardTips.SetToolTip(sub1, message)
    End Sub

    Private Sub SetCardTips()
        cardTips.AutoPopDelay = 8000
        cardTips.InitialDelay = 500

        TipCard(pnlCard1, lblCardTitle1, lblStat1, lblCardSub1, "Everything taken in this range, tickets plus concessions. The small line is tickets first, then concessions.")
        TipCard(pnlCard2, lblCardTitle2, lblStat2, lblCardSub2, "How many seats were sold in this range.")
        TipCard(pnlCard3, lblCardTitle3, lblStat3, lblCardSub3, "What the average seat sold for. Premium and accessible seats cost a different amount, so this sits somewhere between them.")
        TipCard(pnlCard4, lblCardTitle4, lblStat4, lblCardSub4, "How full the screenings that played in this range were. This one always goes on the screening date, even when the report is measured on the booking date, because a seat sold last month for a film showing this month still filled a seat this month. That is why this count can differ from the tickets sold card.")
        TipCard(pnlCard5, lblCardTitle5, lblStat5, lblCardSub5, "How much was spent on food and drink for every ticket sold.")
    End Sub

    Private Sub AddDateParameters(SQLCmd As OleDbCommand, fromDate As Date, toDate As Date)
        SQLCmd.Parameters.AddWithValue("@ByScreening", MeasuringByScreening())
        SQLCmd.Parameters.AddWithValue("@FromDate", fromDate)
        SQLCmd.Parameters.AddWithValue("@ByScreening2", MeasuringByScreening())
        SQLCmd.Parameters.AddWithValue("@ToDate", PeriodEnd(toDate))
    End Sub

    Private Sub AddRangeParameters(SQLCmd As OleDbCommand, fromDate As Date, toDate As Date)
        SQLCmd.Parameters.AddWithValue("@ByScreening", MeasuringByScreening())
        SQLCmd.Parameters.AddWithValue("@FromDate", fromDate)
        SQLCmd.Parameters.AddWithValue("@ByScreening2", MeasuringByScreening())
        SQLCmd.Parameters.AddWithValue("@ToDate", PeriodEnd(toDate))
        SQLCmd.Parameters.AddWithValue("@Cancelled", BookingCancelled)
    End Sub

    Private Sub ShowCount()
        Dim shown As Integer = dgvSalesByFilm.Rows.Count

        Dim thing As String = "film"
        Dim things As String = "films"

        If cboReportType.Text = "Concessions only" Then
            thing = "item"
            things = "items"
        ElseIf cboReportType.Text = "Refunds" Then
            thing = "refund"
            things = "refunds"
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

    Private Sub SetColumnWidths()
        For Each col As DataGridViewColumn In dgvSalesByFilm.Columns
            If col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill Then
                col.MinimumWidth = 150
            End If
        Next

        SetWidth("BookingID", 90)
        SetWidth("CustomerName", 190)
        SetWidth("ScreeningDate", 100)
        SetWidth("ScreeningTime", 70)
        SetWidth("ScreenName", 110)
        SetWidth("ReportDay", 110)
        SetWidth("Sold", 90)
        SetWidth("Tickets", 110)
        SetWidth("TicketRevenue", 130)
        SetWidth("FoodRevenue", 130)
        SetWidth("TotalCost", 130)
        SetWidth("Total", 130)
    End Sub

    Private Sub SetWidth(columnName As String, wide As Integer)
        If dgvSalesByFilm.Columns.Contains(columnName) Then
            If dgvSalesByFilm.Columns(columnName).AutoSizeMode <> DataGridViewAutoSizeColumnMode.Fill Then
                dgvSalesByFilm.Columns(columnName).Width = wide
            End If
        End If
    End Sub

    Private Sub dgvSalesByFilm_ColumnHeaderMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles dgvSalesByFilm.ColumnHeaderMouseClick
        Dim columnName As String = dgvSalesByFilm.Columns(e.ColumnIndex).Name

        If columnName = sortedBy Then
            sortAscending = Not sortAscending
        Else
            sortedBy = columnName

            sortAscending = (dgvSalesByFilm.Columns(e.ColumnIndex).ValueType Is GetType(String))
        End If

        SortReport(sortedBy, sortAscending)
        ShowReport()
    End Sub

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

    Private Sub ShowReport()
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
        ElseIf cboReportType.Text = "Refunds" Then
            dgvSalesByFilm.Columns("RefundID").HeaderText = "Refund"
            dgvSalesByFilm.Columns("BookingID").HeaderText = "Booking"
            dgvSalesByFilm.Columns("CustomerName").HeaderText = "Customer"
            dgvSalesByFilm.Columns("FilmTitle").HeaderText = "Film"
            dgvSalesByFilm.Columns("RefundDate").HeaderText = "Refunded on"
            dgvSalesByFilm.Columns("RefundReason").HeaderText = "Reason"
            dgvSalesByFilm.Columns("RefundAmount").HeaderText = "Paid back"
            dgvSalesByFilm.Columns("RefundID").Width = 80
            dgvSalesByFilm.Columns("BookingID").Width = 80
            dgvSalesByFilm.Columns("RefundReason").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            dgvSalesByFilm.Columns("RefundDate").DefaultCellStyle.Format = "dd/MM/yyyy HH:mm"
            dgvSalesByFilm.Columns("RefundAmount").DefaultCellStyle.Format = "C"
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

        For Each col As DataGridViewColumn In dgvSalesByFilm.Columns
            col.SortMode = DataGridViewColumnSortMode.Programmatic
        Next

        ShowSortArrow()
        dgvSalesByFilm.ClearSelection()
    End Sub

    Private Function LoadTicketsByFilm(fromDate As Date, toDate As Date) As Double
        Dim dt As DataTable = GetTicketsByFilmTable(fromDate, toDate)

        reportTable = dt
        ShowReport()

        Return TotalColumn(dt, "TicketRevenue")
    End Function

    Private Function LoadTicketsByDay(fromDate As Date, toDate As Date) As Double
        Dim dtTickets As DataTable = GetTicketsByDayTable(fromDate, toDate)

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

            Dim slot As Integer = CInt((soldOn.Date - fromDate.Date).TotalDays)

            If slot >= 0 And slot < howManyDays Then
                dayTickets(slot) = dayTickets(slot) + 1

                If Not IsDBNull(row("SeatPricePaid")) Then
                    dayTakings(slot) = dayTakings(slot) + CDbl(row("SeatPricePaid"))
                End If
            End If
        Next

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

    Private Function LoadTicketsByScreening(fromDate As Date, toDate As Date) As Double
        Dim dt As New DataTable

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
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

    Private Function LoadRefunds(fromDate As Date, toDate As Date) As Double
        Dim dt As New DataTable

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT tblRefund.RefundID, tblRefund.BookingID, " &
                                 "CustomerForename & ' ' & CustomerSurname AS CustomerName, " &
                                 "IIf(IsNull(tblFilm.FilmTitle), 'Counter sale', tblFilm.FilmTitle) AS FilmTitle, " &
                                 "tblRefund.RefundDate, tblRefund.RefundReason, tblRefund.RefundAmount " &
                                 "FROM (((tblRefund INNER JOIN tblBooking ON tblRefund.BookingID = tblBooking.BookingID) " &
                                 "LEFT JOIN tblCustomer ON tblBooking.CustomerID = tblCustomer.CustomerID) " &
                                 "LEFT JOIN tblScreening ON tblBooking.ScreeningID = tblScreening.ScreeningID) " &
                                 "LEFT JOIN tblFilm ON tblScreening.FilmID = tblFilm.FilmID " &
                                 "WHERE tblRefund.RefundDate >= @FromDate AND tblRefund.RefundDate < @ToDate " &
                                 "ORDER BY tblRefund.RefundDate DESC, tblRefund.RefundID DESC"
            SQLCmd.Parameters.AddWithValue("@FromDate", fromDate)
            SQLCmd.Parameters.AddWithValue("@ToDate", PeriodEnd(toDate))
            Dim da As New OleDbDataAdapter(SQLCmd)
            da.Fill(dt)
            cn.Close()
        End If

        reportTable = dt
        ShowReport()

        Return TotalColumn(dt, "RefundAmount")
    End Function

    Private Function LoadConcessionsByItem(fromDate As Date, toDate As Date) As Double
        Dim dt As New DataTable
        Dim dtBack As New DataTable

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT FoodItemName, SUM(Quantity) AS Sold, SUM(Quantity * ItemPricePaid) AS FoodRevenue " &
                                 "FROM ((tblOrderItem INNER JOIN tblFoodItem ON tblOrderItem.FoodItemID = tblFoodItem.FoodItemID) " &
                                 "INNER JOIN tblBooking ON tblOrderItem.BookingID = tblBooking.BookingID) " &
                                 "LEFT JOIN tblScreening ON tblBooking.ScreeningID = tblScreening.ScreeningID " &
                                 "WHERE IIf(@ByScreening, IIf(IsNull(tblScreening.ScreeningDate), tblBooking.BookingDate, tblScreening.ScreeningDate), tblBooking.BookingDate) >= @FromDate " &
                                 "AND IIf(@ByScreening2, IIf(IsNull(tblScreening.ScreeningDate), tblBooking.BookingDate, tblScreening.ScreeningDate), tblBooking.BookingDate) < @ToDate " &
                                 "GROUP BY FoodItemName"
            SQLCmd.Parameters.AddWithValue("@ByScreening", MeasuringByScreening())
            SQLCmd.Parameters.AddWithValue("@FromDate", fromDate)
            SQLCmd.Parameters.AddWithValue("@ByScreening2", MeasuringByScreening())
            SQLCmd.Parameters.AddWithValue("@ToDate", PeriodEnd(toDate))
            Dim da As New OleDbDataAdapter(SQLCmd)
            da.Fill(dt)

            SQLCmd.CommandText = "SELECT FoodItemName, SUM(tblRefundLine.QtyRefunded) AS QtyBack, " &
                                 "SUM(tblRefundLine.AmountRefunded) AS MoneyBack " &
                                 "FROM (((tblRefundLine INNER JOIN tblOrderItem ON tblRefundLine.OrderItemID = tblOrderItem.OrderItemID) " &
                                 "INNER JOIN tblFoodItem ON tblOrderItem.FoodItemID = tblFoodItem.FoodItemID) " &
                                 "INNER JOIN tblBooking ON tblOrderItem.BookingID = tblBooking.BookingID) " &
                                 "LEFT JOIN tblScreening ON tblBooking.ScreeningID = tblScreening.ScreeningID " &
                                 "WHERE IIf(@ByScreening, IIf(IsNull(tblScreening.ScreeningDate), tblBooking.BookingDate, tblScreening.ScreeningDate), tblBooking.BookingDate) >= @FromDate " &
                                 "AND IIf(@ByScreening2, IIf(IsNull(tblScreening.ScreeningDate), tblBooking.BookingDate, tblScreening.ScreeningDate), tblBooking.BookingDate) < @ToDate " &
                                 "GROUP BY FoodItemName"
            SQLCmd.Parameters.Clear()
            SQLCmd.Parameters.AddWithValue("@ByScreening", MeasuringByScreening())
            SQLCmd.Parameters.AddWithValue("@FromDate", fromDate)
            SQLCmd.Parameters.AddWithValue("@ByScreening2", MeasuringByScreening())
            SQLCmd.Parameters.AddWithValue("@ToDate", PeriodEnd(toDate))
            Dim daBack As New OleDbDataAdapter(SQLCmd)
            daBack.Fill(dtBack)

            cn.Close()
        End If

        Dim i As Integer

        For i = dt.Rows.Count - 1 To 0 Step -1
            Dim itemName As String = dt.Rows(i)("FoodItemName").ToString()
            Dim soldLeft As Integer = CInt(dt.Rows(i)("Sold")) - FindItemRefundQty(dtBack, itemName)
            Dim moneyLeft As Double = CDbl(dt.Rows(i)("FoodRevenue")) - FindItemRefundMoney(dtBack, itemName)

            If soldLeft <= 0 Then
                dt.Rows.RemoveAt(i)
            Else
                dt.Rows(i)("Sold") = soldLeft
                dt.Rows(i)("FoodRevenue") = moneyLeft
            End If
        Next

        reportTable = dt
        ShowReport()

        Return TotalColumn(dt, "FoodRevenue")
    End Function

    Private Sub LoadCombinedByFilm(fromDate As Date, toDate As Date, ByRef ticketTotal As Double, ByRef foodTotal As Double)
        Dim dtTickets As DataTable = GetTicketsByFilmTable(fromDate, toDate)
        Dim dtFood As DataTable = GetFoodByFilmTable(fromDate, toDate)

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

        SortReport("Total", False)
        sortedBy = "Total"
        sortAscending = False

        ShowReport()
    End Sub

    Private Function GetTicketsByFilmTable(fromDate As Date, toDate As Date) As DataTable
        Dim dt As New DataTable

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT FilmTitle, COUNT(*) AS Tickets, SUM(SeatPricePaid) AS TicketRevenue " &
                                 "FROM ((tblBookingSeat INNER JOIN tblBooking ON tblBookingSeat.BookingID = tblBooking.BookingID) " &
                                 "INNER JOIN tblScreening ON tblBooking.ScreeningID = tblScreening.ScreeningID) " &
                                 "INNER JOIN tblFilm ON tblScreening.FilmID = tblFilm.FilmID " &
                                 "WHERE IIf(@ByScreening, tblScreening.ScreeningDate, tblBooking.BookingDate) >= @FromDate " &
                                 "AND IIf(@ByScreening2, tblScreening.ScreeningDate, tblBooking.BookingDate) < @ToDate " &
                                 "AND tblBooking.BookingStatus <> @Cancelled " &
                                 "GROUP BY FilmTitle"
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

    Private Function GetFoodByFilmTable(fromDate As Date, toDate As Date) As DataTable
        Dim dt As New DataTable
        Dim dtBack As New DataTable

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT IIf(IsNull(tblFilm.FilmTitle), 'Counter sale', tblFilm.FilmTitle) AS FilmTitle, SUM(Quantity * ItemPricePaid) AS FoodRevenue " &
                                 "FROM ((tblOrderItem " &
                                 "INNER JOIN tblBooking ON tblOrderItem.BookingID = tblBooking.BookingID) " &
                                 "LEFT JOIN tblScreening ON tblBooking.ScreeningID = tblScreening.ScreeningID) " &
                                 "LEFT JOIN tblFilm ON tblScreening.FilmID = tblFilm.FilmID " &
                                 "WHERE IIf(@ByScreening, IIf(IsNull(tblScreening.ScreeningDate), tblBooking.BookingDate, tblScreening.ScreeningDate), tblBooking.BookingDate) >= @FromDate " &
                                 "AND IIf(@ByScreening2, IIf(IsNull(tblScreening.ScreeningDate), tblBooking.BookingDate, tblScreening.ScreeningDate), tblBooking.BookingDate) < @ToDate " &
                                 "GROUP BY IIf(IsNull(tblFilm.FilmTitle), 'Counter sale', tblFilm.FilmTitle)"
            SQLCmd.Parameters.AddWithValue("@ByScreening", MeasuringByScreening())
            SQLCmd.Parameters.AddWithValue("@FromDate", fromDate)
            SQLCmd.Parameters.AddWithValue("@ByScreening2", MeasuringByScreening())
            SQLCmd.Parameters.AddWithValue("@ToDate", PeriodEnd(toDate))
            Dim da As New OleDbDataAdapter(SQLCmd)
            da.Fill(dt)

            SQLCmd.CommandText = "SELECT IIf(IsNull(tblFilm.FilmTitle), 'Counter sale', tblFilm.FilmTitle) AS FilmTitle, " &
                                 "SUM(tblRefundLine.AmountRefunded) AS MoneyBack " &
                                 "FROM (((tblRefundLine INNER JOIN tblOrderItem ON tblRefundLine.OrderItemID = tblOrderItem.OrderItemID) " &
                                 "INNER JOIN tblBooking ON tblOrderItem.BookingID = tblBooking.BookingID) " &
                                 "LEFT JOIN tblScreening ON tblBooking.ScreeningID = tblScreening.ScreeningID) " &
                                 "LEFT JOIN tblFilm ON tblScreening.FilmID = tblFilm.FilmID " &
                                 "WHERE IIf(@ByScreening, IIf(IsNull(tblScreening.ScreeningDate), tblBooking.BookingDate, tblScreening.ScreeningDate), tblBooking.BookingDate) >= @FromDate " &
                                 "AND IIf(@ByScreening2, IIf(IsNull(tblScreening.ScreeningDate), tblBooking.BookingDate, tblScreening.ScreeningDate), tblBooking.BookingDate) < @ToDate " &
                                 "GROUP BY IIf(IsNull(tblFilm.FilmTitle), 'Counter sale', tblFilm.FilmTitle)"
            SQLCmd.Parameters.Clear()
            SQLCmd.Parameters.AddWithValue("@ByScreening", MeasuringByScreening())
            SQLCmd.Parameters.AddWithValue("@FromDate", fromDate)
            SQLCmd.Parameters.AddWithValue("@ByScreening2", MeasuringByScreening())
            SQLCmd.Parameters.AddWithValue("@ToDate", PeriodEnd(toDate))
            Dim daBack As New OleDbDataAdapter(SQLCmd)
            daBack.Fill(dtBack)

            cn.Close()
        End If

        Dim i As Integer

        For i = dt.Rows.Count - 1 To 0 Step -1
            Dim title As String = dt.Rows(i)("FilmTitle").ToString()
            Dim moneyLeft As Double = CDbl(dt.Rows(i)("FoodRevenue")) - FindFilmRefundMoney(dtBack, title)

            If moneyLeft <= 0 Then
                dt.Rows.RemoveAt(i)
            Else
                dt.Rows(i)("FoodRevenue") = moneyLeft
            End If
        Next

        Return dt
    End Function

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

        Dim sorted As DataTable = reportTable.Clone()

        For i As Integer = 0 To howMany - 1
            sorted.ImportRow(reportTable.Rows(order(i)))
        Next

        reportTable = sorted
    End Sub

    Private Sub MergeSort(order() As Integer, low As Integer, high As Integer, columnName As String, ascending As Boolean)
        If low >= high Then
            Exit Sub
        End If

        Dim middle As Integer = (low + high) \ 2

        MergeSort(order, low, middle, columnName, ascending)
        MergeSort(order, middle + 1, high, columnName, ascending)
        Merge(order, low, middle, high, columnName, ascending)
    End Sub

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

    Private Function ComesFirst(a As Integer, b As Integer, columnName As String, ascending As Boolean) As Boolean
        Dim result As Integer = CompareCells(reportTable.Rows(a)(columnName), reportTable.Rows(b)(columnName))

        If result = 0 Then
            Return True
        End If

        If ascending Then
            Return result < 0
        Else
            Return result > 0
        End If
    End Function

    Private Function CompareCells(valueA As Object, valueB As Object) As Integer
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
            Return String.Compare(valueA.ToString(), valueB.ToString(), True)
        End If

        If TypeOf valueA Is Date Then
            Return Date.Compare(CDate(valueA), CDate(valueB))
        End If

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

    Private Function FindRow(columnName As String, target As String) As Integer
        Dim low As Integer = 0
        Dim high As Integer = reportTable.Rows.Count - 1

        Do While low <= high
            Dim middle As Integer = (low + high) \ 2
            Dim here As String = reportTable.Rows(middle)(columnName).ToString()

            If here.Length > target.Length Then
                here = here.Substring(0, target.Length)
            End If

            Dim result As Integer = String.Compare(here, target, True)

            If result = 0 Then
                Return middle
            ElseIf result < 0 Then
                low = middle + 1
            Else
                high = middle - 1
            End If
        Loop

        Return -1
    End Function

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

    Private Function FindItemRefundQty(dtBack As DataTable, itemName As String) As Integer
        For Each row As DataRow In dtBack.Rows
            If row("FoodItemName").ToString() = itemName Then
                If IsDBNull(row("QtyBack")) Then
                    Return 0
                Else
                    Return CInt(row("QtyBack"))
                End If
            End If
        Next

        Return 0
    End Function

    Private Function FindItemRefundMoney(dtBack As DataTable, itemName As String) As Double
        For Each row As DataRow In dtBack.Rows
            If row("FoodItemName").ToString() = itemName Then
                If IsDBNull(row("MoneyBack")) Then
                    Return 0
                Else
                    Return CDbl(row("MoneyBack"))
                End If
            End If
        Next

        Return 0
    End Function

    Private Function FindFilmRefundMoney(dtBack As DataTable, filmTitle As String) As Double
        For Each row As DataRow In dtBack.Rows
            If row("FilmTitle").ToString() = filmTitle Then
                If IsDBNull(row("MoneyBack")) Then
                    Return 0
                Else
                    Return CDbl(row("MoneyBack"))
                End If
            End If
        Next

        Return 0
    End Function

    Private Function FilmIsInTable(dt As DataTable, filmTitle As String) As Boolean
        For Each row As DataRow In dt.Rows
            If row("FilmTitle").ToString() = filmTitle Then
                Return True
            End If
        Next

        Return False
    End Function

    Private Function MeasuringByScreening() As Integer
        If cboMeasureBy.Text = "Screening date" Then
            Return 1
        Else
            Return 0
        End If
    End Function

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

    Private Function PeriodEnd(toDate As Date) As Date
        Return toDate.Date.AddDays(1)
    End Function

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
