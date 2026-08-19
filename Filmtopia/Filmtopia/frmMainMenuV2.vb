Imports System.Data.OleDb

Public Class frmMainMenuV2

    Private secondsCounter As Integer = 0

    Private lastRefresh As Date = Date.MinValue

    Private rightClickedRow As Integer = -1

    Private stillLoading As Boolean = True

    Private loggingOut As Boolean = False

    Private shuttingDown As Boolean = False

    Private totalScreenings As Integer = 0

    Private rowBoldFont As New Font("Segoe UI", 9.75!, FontStyle.Bold)

    Private tips As New ToolTip

    Private Sub SetAllButtonsTransp()
        btnBookings.BackColor = Color.Transparent
        btnFindBooking.BackColor = Color.Transparent
        btnScreenings.BackColor = Color.Transparent
        btnCustomers.BackColor = Color.Transparent
        btnFilms.BackColor = Color.Transparent
        btnScreens.BackColor = Color.Transparent
        btnFood.BackColor = Color.Transparent
        btnReports.BackColor = Color.Transparent
        btnLogs.BackColor = Color.Transparent
        btnSettings.BackColor = Color.Transparent
        btnMyAccount.BackColor = Color.Transparent
        btnKiosk.BackColor = Color.Transparent
    End Sub

    Private Sub SetActive(btn As Button)
        btn.BackColor = HighlightBack
    End Sub

    Private Sub NavButton_MouseEnter(sender As Object, e As EventArgs) Handles btnBookings.MouseEnter,
        btnFindBooking.MouseEnter, btnScreenings.MouseEnter, btnCustomers.MouseEnter, btnFilms.MouseEnter,
        btnScreens.MouseEnter, btnFood.MouseEnter, btnReports.MouseEnter, btnLogs.MouseEnter, btnSettings.MouseEnter,
        btnKiosk.MouseEnter, btnMyAccount.MouseEnter

        Dim btn As Button = CType(sender, Button)
        If btn.BackColor <> HighlightBack Then
            btn.BackColor = SidebarHover
        End If
    End Sub

    Private Sub NavButton_MouseLeave(sender As Object, e As EventArgs) Handles btnBookings.MouseLeave,
        btnFindBooking.MouseLeave, btnScreenings.MouseLeave, btnCustomers.MouseLeave, btnFilms.MouseLeave,
        btnScreens.MouseLeave, btnFood.MouseLeave, btnReports.MouseLeave, btnLogs.MouseLeave, btnSettings.MouseLeave,
        btnKiosk.MouseLeave, btnMyAccount.MouseLeave

        Dim btn As Button = CType(sender, Button)
        If btn.BackColor <> HighlightBack Then
            btn.BackColor = Color.Transparent
        End If
    End Sub

    Private Sub OpenForm(frm As Form, btn As Button)
        SetActive(btn)

        frm.Tag = btn

        RemoveHandler frm.FormClosed, AddressOf OpenedForm_Closed
        AddHandler frm.FormClosed, AddressOf OpenedForm_Closed

        frm.Show()

        If frm.WindowState = FormWindowState.Minimized Then
            frm.WindowState = FormWindowState.Normal
        End If

        frm.BringToFront()
    End Sub

    Private Sub OpenedForm_Closed(sender As Object, e As FormClosedEventArgs)
        Dim frm As Form = CType(sender, Form)

        If TypeOf frm.Tag Is Button Then
            Dim btn As Button = CType(frm.Tag, Button)
            btn.BackColor = Color.Transparent
        End If
    End Sub

    Private Sub btnBookings_Click(sender As Object, e As EventArgs) Handles btnBookings.Click
        OpenForm(frmBookings, btnBookings)
    End Sub

    Private Sub btnKiosk_Click(sender As Object, e As EventArgs) Handles btnKiosk.Click
        Dim answer As DialogResult = MessageBox.Show("Put this machine into kiosk mode?" & vbNewLine & vbNewLine &
                                                     "The screen will be taken over by the customer self service " &
                                                     "screen. Use Staff Exit in the top corner to come back.",
                                                     "Kiosk Mode", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        If answer = DialogResult.Yes Then
            OpenForm(frmKiosk, btnKiosk)
        End If
    End Sub

    Private Sub btnFindBooking_Click(sender As Object, e As EventArgs) Handles btnFindBooking.Click
        OpenForm(frmBookingSearch, btnFindBooking)
    End Sub

    Private Sub btnScreenings_Click(sender As Object, e As EventArgs) Handles btnScreenings.Click
        OpenForm(frmScreenings, btnScreenings)
    End Sub

    Private Sub btnCustomers_Click(sender As Object, e As EventArgs) Handles btnCustomers.Click
        OpenForm(frmCustomers, btnCustomers)
    End Sub

    Private Sub btnFilms_Click(sender As Object, e As EventArgs) Handles btnFilms.Click
        OpenForm(frmFilms, btnFilms)
    End Sub

    Private Sub btnScreens_Click(sender As Object, e As EventArgs) Handles btnScreens.Click
        OpenForm(frmScreens, btnScreens)
    End Sub

    Private Sub btnFood_Click(sender As Object, e As EventArgs) Handles btnFood.Click
        OpenForm(frmFoodItems, btnFood)
    End Sub

    Private Sub btnReports_Click(sender As Object, e As EventArgs) Handles btnReports.Click
        OpenForm(frmSalesReport, btnReports)
    End Sub

    Private Sub btnLogs_Click(sender As Object, e As EventArgs) Handles btnLogs.Click
        OpenForm(frmLogs, btnLogs)
    End Sub

    Private Sub btnMyAccount_Click(sender As Object, e As EventArgs) Handles btnMyAccount.Click
        OpenForm(frmUserOverview, btnMyAccount)
    End Sub

    Private Sub btnSettings_Click(sender As Object, e As EventArgs) Handles btnSettings.Click
        OpenForm(frmSettings, btnSettings)
    End Sub

    Private Sub CloseOpenedForms()
        For i As Integer = Application.OpenForms.Count - 1 To 0 Step -1
            Dim openForm As Form = Application.OpenForms(i)

            If openForm IsNot Me And openForm IsNot frmLogin And openForm IsNot frmStartup Then
                openForm.Close()
            End If
        Next
    End Sub

    Private Sub btnLogout_Click(sender As Object, e As EventArgs) Handles btnLogout.Click
        Dim answer As DialogResult = MessageBox.Show("Are you sure you want to log out?", "Log Out",
                                                     MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If answer = DialogResult.No Then
            Exit Sub
        End If

        WriteLog("AUTH", "User '" & frmLogin.globalusername & "' logged out", LogSecurity)

        CloseOpenedForms()
        timerClock.Stop()

        LogedIn = False
        UserAccessLevel = 99
        ClearUserSettings()

        loggingOut = True
        Me.Close()

        frmLogin.Show()
        ApplyThemeToAllForms()
    End Sub

    Private Sub frmMainMenuV2_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If loggingOut Or shuttingDown Then
            Exit Sub
        End If

        Dim answer As DialogResult = MessageBox.Show("Are you sure you want to close Filmtopia?", "Close",
                                                     MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If answer = DialogResult.No Then
            e.Cancel = True
        Else
            WriteLog("AUTH", "User '" & frmLogin.globalusername & "' closed the program", LogSecurity)
            shuttingDown = True
            Application.Exit()
        End If
    End Sub

    Private Sub OpenFromCard(frm As Form, btn As Button)
        If btn.Visible Then
            OpenForm(frm, btn)
        End If
    End Sub

    Private Sub Card1_Click(sender As Object, e As EventArgs) Handles pnlCard1.Click, lblCardTitle1.Click, lblStat1.Click, lblCardSub1.Click
        OpenFromCard(frmFilms, btnFilms)
    End Sub

    Private Sub Card2_Click(sender As Object, e As EventArgs) Handles pnlCard2.Click, lblCardTitle2.Click, lblStat2.Click, lblCardSub2.Click
        OpenFromCard(frmScreenings, btnScreenings)
    End Sub

    Private Sub Card3_Click(sender As Object, e As EventArgs) Handles pnlCard3.Click, lblCardTitle3.Click, lblStat3.Click, lblCardSub3.Click
        OpenFromCard(frmBookings, btnBookings)
    End Sub

    Private Sub Card4_Click(sender As Object, e As EventArgs) Handles pnlCard4.Click, lblCardTitle4.Click, lblStat4.Click, lblCardSub4.Click
        If UserAccessLevel = 1 Then
            OpenFromCard(frmSalesReport, btnReports)
        Else
            OpenFromCard(frmFoodItems, btnFood)
        End If
    End Sub

    Private Sub HandCard(card As Panel, title As Label, value As Label, sub1 As Label)
        card.Cursor = Cursors.Hand
        title.Cursor = Cursors.Hand
        value.Cursor = Cursors.Hand
        sub1.Cursor = Cursors.Hand
    End Sub

    Private Sub SetCardCursors()
        lblWelcome.Cursor = Cursors.Hand
        HandCard(pnlCard3, lblCardTitle3, lblStat3, lblCardSub3)

        If UserAccessLevel = 1 Then
            HandCard(pnlCard1, lblCardTitle1, lblStat1, lblCardSub1)
            HandCard(pnlCard2, lblCardTitle2, lblStat2, lblCardSub2)
            HandCard(pnlCard4, lblCardTitle4, lblStat4, lblCardSub4)
        End If
    End Sub

    Private Sub HoverCard(card As Panel, mouseIsOver As Boolean)
        If card.Cursor <> Cursors.Hand Then
            Exit Sub
        End If

        If mouseIsOver Then
            card.BackColor = CardHover
        Else
            card.BackColor = CardBack
        End If
    End Sub

    Private Sub Card1_MouseEnter(sender As Object, e As EventArgs) Handles pnlCard1.MouseEnter, lblCardTitle1.MouseEnter, lblStat1.MouseEnter, lblCardSub1.MouseEnter
        HoverCard(pnlCard1, True)
    End Sub

    Private Sub Card1_MouseLeave(sender As Object, e As EventArgs) Handles pnlCard1.MouseLeave, lblCardTitle1.MouseLeave, lblStat1.MouseLeave, lblCardSub1.MouseLeave
        HoverCard(pnlCard1, False)
    End Sub

    Private Sub Card2_MouseEnter(sender As Object, e As EventArgs) Handles pnlCard2.MouseEnter, lblCardTitle2.MouseEnter, lblStat2.MouseEnter, lblCardSub2.MouseEnter
        HoverCard(pnlCard2, True)
    End Sub

    Private Sub Card2_MouseLeave(sender As Object, e As EventArgs) Handles pnlCard2.MouseLeave, lblCardTitle2.MouseLeave, lblStat2.MouseLeave, lblCardSub2.MouseLeave
        HoverCard(pnlCard2, False)
    End Sub

    Private Sub Card3_MouseEnter(sender As Object, e As EventArgs) Handles pnlCard3.MouseEnter, lblCardTitle3.MouseEnter, lblStat3.MouseEnter, lblCardSub3.MouseEnter
        HoverCard(pnlCard3, True)
    End Sub

    Private Sub Card3_MouseLeave(sender As Object, e As EventArgs) Handles pnlCard3.MouseLeave, lblCardTitle3.MouseLeave, lblStat3.MouseLeave, lblCardSub3.MouseLeave
        HoverCard(pnlCard3, False)
    End Sub

    Private Sub Card4_MouseEnter(sender As Object, e As EventArgs) Handles pnlCard4.MouseEnter, lblCardTitle4.MouseEnter, lblStat4.MouseEnter, lblCardSub4.MouseEnter
        HoverCard(pnlCard4, True)
    End Sub

    Private Sub Card4_MouseLeave(sender As Object, e As EventArgs) Handles pnlCard4.MouseLeave, lblCardTitle4.MouseLeave, lblStat4.MouseLeave, lblCardSub4.MouseLeave
        HoverCard(pnlCard4, False)
    End Sub

    Private Sub TipCard(tips As ToolTip, card As Panel, title As Label, value As Label, sub1 As Label, message As String)
        tips.SetToolTip(card, message)
        tips.SetToolTip(title, message)
        tips.SetToolTip(value, message)
        tips.SetToolTip(sub1, message)
    End Sub

    Private Sub SetToolTips()
        tips.AutoPopDelay = 8000
        tips.InitialDelay = 500

        TipCard(tips, pnlCard1, lblCardTitle1, lblStat1, lblCardSub1, "How many films are on the system. Click to manage them.")

        If UserAccessLevel = 1 Then
            TipCard(tips, pnlCard2, lblCardTitle2, lblStat2, lblCardSub2, "How many screenings are scheduled. Click to manage them.")
            TipCard(tips, pnlCard3, lblCardTitle3, lblStat3, lblCardSub3, "How many bookings have been made. Click to make one.")
            TipCard(tips, pnlCard4, lblCardTitle4, lblStat4, lblCardSub4, "Everything taken, split into tickets and concessions. Click for the sales report.")
        Else
            TipCard(tips, pnlCard2, lblCardTitle2, lblStat2, lblCardSub2, "How many screenings are scheduled today.")
            TipCard(tips, pnlCard3, lblCardTitle3, lblStat3, lblCardSub3, "How many seats have been sold. Click to make a booking.")
            TipCard(tips, pnlCard4, lblCardTitle4, lblStat4, lblCardSub4, "How many food and drink items have been sold.")
        End If

        tips.SetToolTip(txtSearch, "Type part of a film or screen name to narrow the list down")
        tips.SetToolTip(cboShow, "Choose which screenings the list shows")
        tips.SetToolTip(btnRefresh, "Update the figures now. F5 does the same.")
        tips.SetToolTip(btnBookings, "Make a new booking and pick seats")
        tips.SetToolTip(btnFindBooking, "Look up or cancel a booking")
        tips.SetToolTip(btnScreenings, "See and set up what is showing")
        tips.SetToolTip(btnCustomers, "Look up customer details")
        tips.SetToolTip(btnFilms, "Add and edit films")
        tips.SetToolTip(btnScreens, "Set up screens and their seating")
        tips.SetToolTip(btnFood, "Manage food and drink items")
        tips.SetToolTip(btnReports, "View the sales report")
        tips.SetToolTip(btnKiosk, "Hand the screen over to a customer to serve themselves")
        tips.SetToolTip(btnLogs, "See a history of what has been done")
        tips.SetToolTip(btnMyAccount, "What you have sold and done, and when you tend to be on")
        tips.SetToolTip(lblWelcome, "Your own sales, your own history and your settings")
        tips.SetToolTip(btnSettings, "Settings for the whole cinema, and database backup")
        tips.SetToolTip(btnLogout, "Log out and go back to the login screen")
    End Sub

    Private Sub lblWelcome_Click(sender As Object, e As EventArgs) Handles lblWelcome.Click
        OpenForm(frmUserOverview, btnMyAccount)
    End Sub

    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        secondsCounter = 0
        RefreshDashboard()
    End Sub

    Private Sub frmMainMenuV2_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.F5 Then
            secondsCounter = 0
            RefreshDashboard()
        ElseIf e.KeyCode = Keys.Escape And txtSearch.Text <> "" Then
            txtSearch.Text = ""
        End If
    End Sub

    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        If stillLoading Then
            Exit Sub
        End If
        LoadWhatsOn()
    End Sub

    Private Sub cboShow_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboShow.SelectedIndexChanged
        If stillLoading Then
            Exit Sub
        End If
        LoadWhatsOn()
    End Sub

    Private Sub FillShowFilter()
        cboShow.Items.Add("All screenings")
        cboShow.Items.Add("Today only")
        cboShow.Items.Add("Today and after")
        cboShow.Items.Add("Already been")
        cboShow.SelectedIndex = 0
    End Sub

    Private Function DateFilterSQL() As String
        If cboShow.SelectedIndex = 1 Then
            Return " AND s.ScreeningDate = @Today"
        ElseIf cboShow.SelectedIndex = 2 Then
            Return " AND s.ScreeningDate >= @Today"
        ElseIf cboShow.SelectedIndex = 3 Then
            Return " AND s.ScreeningDate < @Today"
        Else
            Return ""
        End If
    End Function

    Private Sub OpenBookingForRow(rowIndex As Integer)
        If rowIndex < 0 Or rowIndex >= dgvWhatsOn.Rows.Count Then
            Exit Sub
        End If

        Dim screeningID As Long = CLng(dgvWhatsOn.Rows(rowIndex).Cells("ScreeningID").Value)

        OpenForm(frmBookings, btnBookings)
        frmBookings.SelectScreening(screeningID)
    End Sub

    Private Sub dgvWhatsOn_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvWhatsOn.CellDoubleClick
        OpenBookingForRow(e.RowIndex)
    End Sub

    Private Sub dgvWhatsOn_Sorted(sender As Object, e As EventArgs) Handles dgvWhatsOn.Sorted
        ColourOccupancy()
        MarkUpcomingScreenings()
    End Sub

    Private Sub dgvWhatsOn_CellMouseDown(sender As Object, e As DataGridViewCellMouseEventArgs) Handles dgvWhatsOn.CellMouseDown
        If e.Button = MouseButtons.Right And e.RowIndex >= 0 Then
            rightClickedRow = e.RowIndex

            dgvWhatsOn.ClearSelection()
            dgvWhatsOn.Rows(e.RowIndex).Selected = True
        End If
    End Sub

    Private Sub GridMenuBook_Click(sender As Object, e As EventArgs)
        OpenBookingForRow(rightClickedRow)
    End Sub

    Private Sub GridMenuCopy_Click(sender As Object, e As EventArgs)
        If rightClickedRow < 0 Or rightClickedRow >= dgvWhatsOn.Rows.Count Then
            Exit Sub
        End If

        Dim row As DataGridViewRow = dgvWhatsOn.Rows(rightClickedRow)
        Dim details As String = row.Cells("FilmTitle").Value.ToString() & " - " &
                                row.Cells("ScreenName").Value.ToString() & " - " &
                                Format(CDate(row.Cells("ScreeningDate").Value), "dd/MM/yyyy") & " at " &
                                row.Cells("ScreeningTime").Value.ToString()

        Clipboard.SetText(details)
        MessageBox.Show("Copied: " & details, "Screening Details", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub GridMenuRefresh_Click(sender As Object, e As EventArgs)
        secondsCounter = 0
        RefreshDashboard()
    End Sub

    Private Sub SetGridMenu()
        Dim gridMenu As New ContextMenuStrip
        Dim bookItem As ToolStripMenuItem = New ToolStripMenuItem("Make a booking for this screening")
        Dim copyItem As ToolStripMenuItem = New ToolStripMenuItem("Copy the screening details")
        Dim refreshItem As ToolStripMenuItem = New ToolStripMenuItem("Refresh the list")

        AddHandler bookItem.Click, AddressOf GridMenuBook_Click
        AddHandler copyItem.Click, AddressOf GridMenuCopy_Click
        AddHandler refreshItem.Click, AddressOf GridMenuRefresh_Click

        gridMenu.Items.Add(bookItem)
        gridMenu.Items.Add(copyItem)
        gridMenu.Items.Add(New ToolStripSeparator)
        gridMenu.Items.Add(refreshItem)

        dgvWhatsOn.ContextMenuStrip = gridMenu
    End Sub

    Private Function GetGreeting() As String
        Dim hour As Integer = Date.Now.Hour

        If hour < 12 Then
            Return "Good morning"
        ElseIf hour < 18 Then
            Return "Good afternoon"
        Else
            Return "Good evening"
        End If
    End Function

    Private Sub LoadStats()
        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn

            SQLCmd.CommandText = "SELECT COUNT(*) FROM tblFilm"
            lblStat1.Text = SQLCmd.ExecuteScalar().ToString()

            SQLCmd.CommandText = "SELECT COUNT(*) FROM tblFilm WHERE FilmID IN (SELECT FilmID FROM tblScreening)"
            lblCardSub1.Text = SQLCmd.ExecuteScalar().ToString() & " have screenings"

            SQLCmd.CommandText = "SELECT COUNT(*) FROM tblScreening " &
                                 "WHERE (ScreeningStatus IS NULL OR ScreeningStatus <> 'Cancelled')"
            totalScreenings = CInt(SQLCmd.ExecuteScalar())
            lblStat2.Text = totalScreenings.ToString()

            SQLCmd.CommandText = "SELECT COUNT(*) FROM tblScreening WHERE ScreeningDate >= @Today " &
                                 "AND (ScreeningStatus IS NULL OR ScreeningStatus <> 'Cancelled')"
            SQLCmd.Parameters.AddWithValue("@Today", Date.Today)
            lblCardSub2.Text = SQLCmd.ExecuteScalar().ToString() & " still to come"
            SQLCmd.Parameters.Clear()

            If UserAccessLevel = 1 Then
                SQLCmd.CommandText = "SELECT COUNT(*) FROM tblBooking WHERE BookingStatus <> @Cancelled"
                SQLCmd.Parameters.AddWithValue("@Cancelled", BookingCancelled)
                Dim bookings As Integer = CInt(SQLCmd.ExecuteScalar())
                lblStat3.Text = bookings.ToString()

                SQLCmd.CommandText = "SELECT COUNT(*) FROM tblBookingSeat"
                SQLCmd.Parameters.Clear()
                lblCardSub3.Text = SQLCmd.ExecuteScalar().ToString() & " seats sold"

                SQLCmd.CommandText = "SELECT SUM(SeatPricePaid) FROM tblBookingSeat"
                SQLCmd.Parameters.Clear()
                Dim ticketResult = SQLCmd.ExecuteScalar()
                Dim tickets As Double = 0
                If ticketResult IsNot Nothing AndAlso Not IsDBNull(ticketResult) Then
                    tickets = CDbl(ticketResult)
                End If

                SQLCmd.CommandText = "SELECT SUM(Quantity * ItemPricePaid) FROM tblOrderItem"
                SQLCmd.Parameters.Clear()
                Dim foodResult = SQLCmd.ExecuteScalar()
                Dim concessions As Double = 0
                If foodResult IsNot Nothing AndAlso Not IsDBNull(foodResult) Then
                    concessions = CDbl(foodResult)
                End If

                SQLCmd.CommandText = "SELECT SUM(AmountRefunded) FROM tblRefundLine WHERE LineType = @LineType"
                SQLCmd.Parameters.Clear()
                SQLCmd.Parameters.AddWithValue("@LineType", RefundFoodLine)
                Dim foodBackResult = SQLCmd.ExecuteScalar()
                If foodBackResult IsNot Nothing AndAlso Not IsDBNull(foodBackResult) Then
                    concessions = concessions - CDbl(foodBackResult)
                End If

                Dim takings As Double = tickets + concessions

                lblStat4.Text = FormatCurrency(takings)
                If takings = 0 Then
                    lblCardSub4.Text = "nothing taken yet"
                Else
                    lblCardSub4.Text = FormatCurrency(tickets) & " tickets | " & FormatCurrency(concessions) & " concessions"
                End If
            Else
                SQLCmd.CommandText = "SELECT COUNT(*) FROM tblBookingSeat"
                lblStat3.Text = SQLCmd.ExecuteScalar().ToString()

                SQLCmd.CommandText = "SELECT COUNT(*) FROM tblBooking WHERE BookingStatus <> @Cancelled"
                SQLCmd.Parameters.AddWithValue("@Cancelled", BookingCancelled)
                lblCardSub3.Text = "across " & SQLCmd.ExecuteScalar().ToString() & " bookings"

                SQLCmd.CommandText = "SELECT SUM(Quantity) " &
                                     "FROM tblOrderItem INNER JOIN tblBooking ON tblOrderItem.BookingID = tblBooking.BookingID " &
                                     "WHERE tblBooking.BookingStatus <> @Cancelled"
                Dim snacks = SQLCmd.ExecuteScalar()
                If snacks Is Nothing OrElse IsDBNull(snacks) Then
                    lblStat4.Text = "0"
                Else
                    lblStat4.Text = snacks.ToString()
                End If

                SQLCmd.CommandText = "SELECT COUNT(*) FROM tblFoodItem"
                SQLCmd.Parameters.Clear()
                lblCardSub4.Text = SQLCmd.ExecuteScalar().ToString() & " items on the menu"
            End If

            cn.Close()
        End If
    End Sub

    Private Function GetTopFilmText() As String
        Dim result As String = "No seats have been booked yet"

        Dim SQLCmd As New OleDbCommand
        SQLCmd.Connection = cn
        SQLCmd.CommandText = "SELECT TOP 1 f.FilmTitle, COUNT(*) AS SeatsSold " &
                             "FROM ((tblBookingSeat AS bs " &
                             "INNER JOIN tblBooking AS b ON bs.BookingID = b.BookingID) " &
                             "INNER JOIN tblScreening AS s ON b.ScreeningID = s.ScreeningID) " &
                             "INNER JOIN tblFilm AS f ON s.FilmID = f.FilmID " &
                             "GROUP BY f.FilmTitle " &
                             "ORDER BY COUNT(*) DESC, f.FilmTitle"

        Dim rs As OleDbDataReader = SQLCmd.ExecuteReader()
        If rs.Read() Then
            result = "Everyone is watching " & rs("FilmTitle") & " - " & rs("SeatsSold") & " seats gone"
        End If
        rs.Close()

        Return result
    End Function

    Private Function GetOverallText() As String
        Dim SQLCmd As New OleDbCommand
        SQLCmd.Connection = cn

        SQLCmd.CommandText = "SELECT SUM(sc.ScreenCapacity) " &
                             "FROM tblScreening AS s INNER JOIN tblScreen AS sc ON s.ScreenID = sc.ScreenID " &
                             "WHERE (s.ScreeningStatus IS NULL OR s.ScreeningStatus <> 'Cancelled')"
        Dim capacity = SQLCmd.ExecuteScalar()

        If capacity Is Nothing OrElse IsDBNull(capacity) OrElse CInt(capacity) = 0 Then
            Return "No screenings to report on yet"
        End If

        SQLCmd.CommandText = "SELECT COUNT(*) " &
                             "FROM (tblBookingSeat AS bs INNER JOIN tblBooking AS b ON bs.BookingID = b.BookingID) " &
                             "INNER JOIN tblScreening AS s ON b.ScreeningID = s.ScreeningID"
        Dim sold As Integer = CInt(SQLCmd.ExecuteScalar())

        Dim percent As Integer = CInt(sold * 100 / CInt(capacity))

        Return sold & " of " & capacity & " seats sold across all screenings (" & percent & "%)"
    End Function

    Private Function GetNextUpText() As String
        Dim result As String = "Nothing is scheduled from today onwards."

        Dim SQLCmd As New OleDbCommand
        SQLCmd.Connection = cn
        SQLCmd.CommandText = "SELECT TOP 1 f.FilmTitle, sc.ScreenName, s.ScreeningDate, s.ScreeningTime " &
                             "FROM (tblScreening AS s INNER JOIN tblFilm AS f ON s.FilmID = f.FilmID) " &
                             "INNER JOIN tblScreen AS sc ON s.ScreenID = sc.ScreenID " &
                             "WHERE (s.ScreeningDate > @Today " &
                             "OR (s.ScreeningDate = @Today2 AND s.ScreeningTime >= @Now)) " &
                             "AND (s.ScreeningStatus IS NULL OR s.ScreeningStatus <> 'Cancelled') " &
                             "ORDER BY s.ScreeningDate, s.ScreeningTime, s.ScreeningID"
        SQLCmd.Parameters.AddWithValue("@Today", Date.Today)
        SQLCmd.Parameters.AddWithValue("@Today2", Date.Today)
        SQLCmd.Parameters.AddWithValue("@Now", Format(Now, "HH:mm"))

        Dim rs As OleDbDataReader = SQLCmd.ExecuteReader()
        If rs.Read() Then
            result = "Next up: " & rs("FilmTitle") & " in " & rs("ScreenName") & ", " &
                     LCase(DescribeWhen(CDate(rs("ScreeningDate")))) & " at " & rs("ScreeningTime") & "."
        End If
        rs.Close()

        Return result
    End Function

    Private Function CountNearlySoldOut() As Integer
        Dim SQLCmd As New OleDbCommand
        SQLCmd.Connection = cn
        SQLCmd.CommandText = "SELECT COUNT(*) " &
                             "FROM tblScreening AS s INNER JOIN tblScreen AS sc ON s.ScreenID = sc.ScreenID " &
                             "WHERE s.ScreeningDate >= @Today AND sc.ScreenCapacity > 0 " &
                             "AND (s.ScreeningStatus IS NULL OR s.ScreeningStatus <> 'Cancelled') " &
                             "AND (SELECT COUNT(*) FROM tblBookingSeat AS bs " &
                             "WHERE bs.ScreeningID = s.ScreeningID) >= sc.ScreenCapacity * 0.8"
        SQLCmd.Parameters.AddWithValue("@Today", Date.Today)

        Return CInt(SQLCmd.ExecuteScalar())
    End Function

    Private Sub LoadHeadlines()
        If DbConnect() Then
            Dim message As String = GetNextUpText()
            Dim nearly As Integer = CountNearlySoldOut()

            If nearly = 1 Then
                message = message & "  1 screening is nearly sold out."
            ElseIf nearly > 1 Then
                message = message & "  " & nearly & " screenings are nearly sold out."
            End If

            lblAlerts.Text = message

            If UserAccessLevel = 1 Then
                lblTopFilm.Text = GetOverallText()
            Else
                lblTopFilm.Text = GetTopFilmText()
            End If

            cn.Close()
        End If
    End Sub

    Private Sub LoadWhatsOn()
        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT s.ScreeningID, f.FilmTitle, sc.ScreenName, s.ScreeningDate, s.ScreeningTime, " &
                                 "sc.ScreenCapacity, " &
                                 "(SELECT COUNT(*) FROM tblBookingSeat AS bs " &
                                 "WHERE bs.ScreeningID = s.ScreeningID) AS SeatsBooked, " &
                                 "sc.ScreenCapacity - (SELECT COUNT(*) FROM tblBookingSeat AS bs2 " &
                                 "WHERE bs2.ScreeningID = s.ScreeningID) AS SeatsLeft " &
                                 "FROM (tblScreening AS s INNER JOIN tblFilm AS f ON s.FilmID = f.FilmID) " &
                                 "INNER JOIN tblScreen AS sc ON s.ScreenID = sc.ScreenID " &
                                 "WHERE (f.FilmTitle LIKE @Search OR sc.ScreenName LIKE @Search2) " &
                                 "AND (s.ScreeningStatus IS NULL OR s.ScreeningStatus <> 'Cancelled')" &
                                 DateFilterSQL() &
                                 " ORDER BY s.ScreeningDate, s.ScreeningTime, s.ScreeningID"

            Dim wanted As String = "%" & txtSearch.Text.Trim() & "%"
            SQLCmd.Parameters.AddWithValue("@Search", wanted)
            SQLCmd.Parameters.AddWithValue("@Search2", wanted)

            If cboShow.SelectedIndex > 0 Then
                SQLCmd.Parameters.AddWithValue("@Today", Date.Today)
            End If

            Dim da As New OleDbDataAdapter(SQLCmd)
            Dim dt As New DataTable
            da.Fill(dt)
            AddPercentFull(dt)

            Dim scrolledTo As Integer = 0
            If dgvWhatsOn.FirstDisplayedScrollingRowIndex > 0 Then
                scrolledTo = dgvWhatsOn.FirstDisplayedScrollingRowIndex
            End If

            dgvWhatsOn.DataSource = dt

            If scrolledTo > 0 And scrolledTo < dgvWhatsOn.Rows.Count Then
                dgvWhatsOn.FirstDisplayedScrollingRowIndex = scrolledTo
            End If

            cn.Close()

            TidyGrid()
            ColourOccupancy()
            MarkUpcomingScreenings()
            ShowGridCount(dt)
            ShowEmptyMessage(dt)
        End If
    End Sub

    Private Sub MarkUpcomingScreenings()
        dgvWhatsOn.AlternatingRowsDefaultCellStyle.BackColor = AltRowBack

        For i As Integer = 0 To dgvWhatsOn.Rows.Count - 1
            Dim showDate As Date = CDate(dgvWhatsOn.Rows(i).Cells("ScreeningDate").Value)

            If showDate >= Date.Today Then
                dgvWhatsOn.Rows(i).DefaultCellStyle.Font = rowBoldFont
            Else
                dgvWhatsOn.Rows(i).DefaultCellStyle.Font = Nothing
            End If

            Dim booked As Integer = CInt(dgvWhatsOn.Rows(i).Cells("SeatsBooked").Value)
            Dim capacity As Integer = CInt(dgvWhatsOn.Rows(i).Cells("ScreenCapacity").Value)
            Dim film As String = dgvWhatsOn.Rows(i).Cells("FilmTitle").Value.ToString()
            Dim tip As String = film & " - " & booked & " of " & capacity & " seats sold, "

            If capacity - booked = 0 Then
                tip = tip & "it is sold out. Double click to look at the booking screen."
            Else
                tip = tip & (capacity - booked) & " still free. Double click to make a booking."
            End If

            For Each cell As DataGridViewCell In dgvWhatsOn.Rows(i).Cells
                cell.ToolTipText = tip
            Next
        Next
    End Sub

    Private Sub ShowGridCount(dt As DataTable)
        If dt.Rows.Count = totalScreenings Then
            If totalScreenings = 1 Then
                lblGridCount.Text = "1 screening"
            Else
                lblGridCount.Text = totalScreenings & " screenings"
            End If
        Else
            lblGridCount.Text = "Showing " & dt.Rows.Count & " of " & totalScreenings & " screenings"
        End If
    End Sub

    Private Sub ShowEmptyMessage(dt As DataTable)
        If dt.Rows.Count > 0 Then
            lblNoRows.Visible = False
            dgvWhatsOn.Visible = True
            Exit Sub
        End If

        If txtSearch.Text.Trim() <> "" Then
            lblNoRows.Text = "Nothing matches " & Chr(34) & txtSearch.Text.Trim() & Chr(34) & "."
        ElseIf cboShow.SelectedIndex = 1 Then
            lblNoRows.Text = "There is nothing on today."
        ElseIf cboShow.SelectedIndex = 2 Then
            lblNoRows.Text = "There is nothing scheduled from today onwards."
        ElseIf cboShow.SelectedIndex = 3 Then
            lblNoRows.Text = "No screenings have been and gone yet."
        Else
            lblNoRows.Text = "There are no screenings on the system yet."
        End If

        lblNoRows.BackColor = InputBack
        dgvWhatsOn.Visible = False
        lblNoRows.Visible = True
    End Sub

    Private Sub AddPercentFull(dt As DataTable)
        dt.Columns.Add("PercentFull", GetType(Integer))
        dt.Columns.Add("WhenText", GetType(String))

        For i As Integer = 0 To dt.Rows.Count - 1
            Dim capacity As Integer = CInt(dt.Rows(i)("ScreenCapacity"))
            Dim booked As Integer = CInt(dt.Rows(i)("SeatsBooked"))

            If capacity > 0 Then
                dt.Rows(i)("PercentFull") = CInt(booked * 100 / capacity)
            Else
                dt.Rows(i)("PercentFull") = 0
            End If

            If capacity - booked <= 0 Then
                dt.Rows(i)("WhenText") = DescribeWhen(CDate(dt.Rows(i)("ScreeningDate"))) & " - full"
            Else
                dt.Rows(i)("WhenText") = DescribeWhen(CDate(dt.Rows(i)("ScreeningDate")))
            End If
        Next
    End Sub

    Private Function DescribeWhen(showDate As Date) As String
        Dim days As Integer = DateDiff(DateInterval.Day, Date.Today, showDate)

        If days < 0 Then
            Return "Past"
        ElseIf days = 0 Then
            Return "Today"
        ElseIf days = 1 Then
            Return "Tomorrow"
        Else
            Return "In " & days & " days"
        End If
    End Function

    Private Sub ColourOccupancy()
        For i As Integer = 0 To dgvWhatsOn.Rows.Count - 1
            Dim cell As DataGridViewCell = dgvWhatsOn.Rows(i).Cells("PercentFull")
            Dim percent As Integer = CInt(cell.Value)

            If percent >= 80 Then
                cell.Style.ForeColor = OccupancyHigh
                cell.Style.SelectionForeColor = OccupancyHigh
                cell.Style.Font = rowBoldFont
            ElseIf percent >= 50 Then
                cell.Style.ForeColor = OccupancyMed
                cell.Style.SelectionForeColor = OccupancyMed
                cell.Style.Font = Nothing
            Else
                cell.Style.ForeColor = Color.Empty
                cell.Style.SelectionForeColor = Color.Empty
                cell.Style.Font = Nothing
            End If

            Dim whenCell As DataGridViewCell = dgvWhatsOn.Rows(i).Cells("WhenText")
            If CInt(dgvWhatsOn.Rows(i).Cells("SeatsLeft").Value) <= 0 Then
                whenCell.Style.ForeColor = OccupancyHigh
                whenCell.Style.SelectionForeColor = OccupancyHigh
            Else
                whenCell.Style.ForeColor = Color.Empty
                whenCell.Style.SelectionForeColor = Color.Empty
            End If
        Next
    End Sub

    Private Sub TidyGrid()
        If dgvWhatsOn.Columns.Count = 0 Then
            Exit Sub
        End If

        dgvWhatsOn.Columns("FilmTitle").HeaderText = "Film"
        dgvWhatsOn.Columns("ScreenName").HeaderText = "Screen"
        dgvWhatsOn.Columns("ScreeningDate").HeaderText = "Date"
        dgvWhatsOn.Columns("ScreeningTime").HeaderText = "Time"
        dgvWhatsOn.Columns("ScreenCapacity").HeaderText = "Seats"
        dgvWhatsOn.Columns("SeatsBooked").HeaderText = "Booked"
        dgvWhatsOn.Columns("SeatsLeft").HeaderText = "Left"
        dgvWhatsOn.Columns("PercentFull").HeaderText = "% Full"
        dgvWhatsOn.Columns("ScreeningID").Visible = False

        If UserAccessLevel = 1 Then
            dgvWhatsOn.Columns("SeatsBooked").Visible = True
            dgvWhatsOn.Columns("SeatsLeft").Visible = False
        Else
            dgvWhatsOn.Columns("SeatsBooked").Visible = False
            dgvWhatsOn.Columns("SeatsLeft").Visible = True
        End If

        dgvWhatsOn.Columns("FilmTitle").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        dgvWhatsOn.Columns("ScreenName").Width = 110
        dgvWhatsOn.Columns("ScreeningDate").Width = 100
        dgvWhatsOn.Columns("ScreeningTime").Width = 90
        dgvWhatsOn.Columns("ScreenCapacity").Width = 90
        dgvWhatsOn.Columns("SeatsBooked").Width = 90
        dgvWhatsOn.Columns("SeatsLeft").Width = 90
        dgvWhatsOn.Columns("PercentFull").Width = 80
        dgvWhatsOn.Columns("WhenText").HeaderText = "When"
        dgvWhatsOn.Columns("WhenText").Width = 110
        dgvWhatsOn.Columns("WhenText").DisplayIndex = 4

        dgvWhatsOn.Columns("ScreeningDate").DefaultCellStyle.Format = "dd/MM/yyyy"

        dgvWhatsOn.Columns("ScreenCapacity").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dgvWhatsOn.Columns("SeatsBooked").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dgvWhatsOn.Columns("SeatsLeft").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dgvWhatsOn.Columns("PercentFull").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

        dgvWhatsOn.DefaultCellStyle.Font = New Font("Segoe UI", 9.75!)
        dgvWhatsOn.ColumnHeadersDefaultCellStyle.Font = rowBoldFont

        dgvWhatsOn.ClearSelection()
    End Sub

    Private Sub ConfigureAccessLevel()
        lblWelcome.Text = GetGreeting() & ", " & frmLogin.globalusername

        If UserAccessLevel = 1 Then
            lblSubtitle.Text = "Signed in as a manager. Here is how the cinema is doing."
            lblCardTitle3.Text = "Bookings taken"
            lblCardTitle4.Text = "Money taken"

            btnFilms.Visible = True
            btnScreenings.Visible = True
            btnScreens.Visible = True
            btnFood.Visible = True
            btnReports.Visible = True
            btnLogs.Visible = True
            btnSettings.Visible = True
            lblNavManage.Visible = True
            btnMyAccount.Visible = True
        Else
            lblSubtitle.Text = GetStaffMessage()
            lblCardTitle3.Text = "Seats sold"
            lblCardTitle4.Text = "Snacks sold"

            btnFilms.Visible = False
            btnScreenings.Visible = False
            btnScreens.Visible = False
            btnFood.Visible = False
            btnReports.Visible = False
            btnLogs.Visible = False
            btnSettings.Visible = False
            lblNavManage.Visible = False
            btnMyAccount.Visible = True
        End If
    End Sub

    Private Function GetStaffMessage() As String
        Dim messages(5) As String

        messages(0) = "Check the age rating before you sell a ticket."
        messages(1) = "The popcorn machine wants ten minutes to warm up."
        messages(2) = "If a screening is nearly full, tell a manager."
        messages(3) = "Tickets get checked at the screen door, not at the till."
        messages(4) = "You are the first person the customer sees, so smile."
        messages(5) = "Nobody has ever asked for less butter."

        Randomize()
        Dim pick As Integer = Int(Rnd() * 6)

        Return messages(pick)
    End Function

    Private Sub timerClock_Tick(sender As Object, e As EventArgs) Handles timerClock.Tick
        ShowClock()

        secondsCounter = secondsCounter + 1

        If secondsCounter >= 60 Then
            secondsCounter = 0

            If DbConnectQuiet() Then
                RefreshDashboard()
            End If
        End If
    End Sub

    Private Sub ShowClock()
        lblClock.Text = Format(Now, "ddd d MMM   HH:mm:ss")
        lblClock.Left = pnlHeader.Width - lblClock.Width - 30
    End Sub

    Private Sub LayoutDashboard()
        Dim edge As Integer = 23
        Dim gap As Integer = 17

        pnlHeader.Width = Me.ClientSize.Width
        pnlSidebar.Height = Me.ClientSize.Height - pnlSidebar.Top

        btnLogout.Top = pnlSidebar.Height - btnLogout.Height - 22
        flpNav.Height = btnLogout.Top - flpNav.Top - 10

        dgvWhatsOn.Width = Me.ClientSize.Width - dgvWhatsOn.Left - edge
        dgvWhatsOn.Height = Me.ClientSize.Height - dgvWhatsOn.Top - 46

        Dim cardWidth As Integer = (dgvWhatsOn.Width - gap * 3) \ 4
        pnlCard1.Left = dgvWhatsOn.Left
        pnlCard2.Left = dgvWhatsOn.Left + cardWidth + gap
        pnlCard3.Left = dgvWhatsOn.Left + (cardWidth + gap) * 2
        pnlCard4.Left = dgvWhatsOn.Right - cardWidth

        pnlCard1.Width = cardWidth
        pnlCard2.Width = cardWidth
        pnlCard3.Width = cardWidth
        pnlCard4.Width = cardWidth

        btnRefresh.Left = dgvWhatsOn.Right - btnRefresh.Width
        cboShow.Left = btnRefresh.Left - cboShow.Width - 10
        txtSearch.Left = cboShow.Left - txtSearch.Width - 12
        lblSearch.Left = txtSearch.Left - lblSearch.Width - 8

        lblTopFilm.Left = dgvWhatsOn.Right - lblTopFilm.Width
        lblAlerts.Width = lblTopFilm.Left - lblAlerts.Left - 12

        lblVersion.Top = dgvWhatsOn.Bottom + 12
        lblGridCount.Top = dgvWhatsOn.Bottom + 13
        lblGridCount.Left = dgvWhatsOn.Right - lblGridCount.Width

        lblNoRows.Left = dgvWhatsOn.Left
        lblNoRows.Top = dgvWhatsOn.Top
        lblNoRows.Width = dgvWhatsOn.Width
        lblNoRows.Height = dgvWhatsOn.Height
    End Sub

    Private Sub frmMainMenuV2_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize
        If dgvWhatsOn Is Nothing Then
            Exit Sub
        End If

        LayoutDashboard()
        ShowClock()
    End Sub

    Private Sub frmMainMenuV2_Activated(sender As Object, e As EventArgs) Handles MyBase.Activated
        If stillLoading Then
            Exit Sub
        End If

        If DateDiff(DateInterval.Second, lastRefresh, Now) < 2 Then
            Exit Sub
        End If

        If DbConnectQuiet() Then
            RefreshDashboard()
        End If
    End Sub

    Private Sub RefreshDashboard()
        lastRefresh = Now
        LoadStats()
        LoadHeadlines()
        LoadWhatsOn()
    End Sub

    Private Sub frmMainMenuV2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CommonFormStartup(Me)
        Me.KeyPreview = True
        Me.MinimumSize = Me.Size

        SetAllButtonsTransp()
        ConfigureAccessLevel()
        SetCardCursors()
        SetToolTips()
        SetGridMenu()
        FillShowFilter()
        LayoutDashboard()

        stillLoading = False
        RefreshDashboard()

        ShowClock()
        timerClock.Start()

        txtSearch.Select()
    End Sub


End Class
