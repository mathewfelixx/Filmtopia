Imports System.Data.OleDb

Public Class frmMainMenuV2

    'counts the timer ticks so the figures can be refreshed once a minute
    Private secondsCounter As Integer = 0

    'when the figures were last reloaded, used so coming back to the menu does not reload it
    'over and over again
    Private lastRefresh As Date = Date.MinValue

    'which row was right clicked, -1 means none
    Private rightClickedRow As Integer = -1

    'true while the form is still setting itself up, so filling the filter box does not try to
    'load the grid before everything is ready
    Private stillLoading As Boolean = True

    'set just before the menu closes itself for a log out, so the are you sure message does not
    'appear as well
    Private loggingOut As Boolean = False

    'set once the user has said yes to quitting, because Application.Exit makes the closing event
    'run a second time and the message was appearing twice
    Private shuttingDown As Boolean = False

    'how many screenings there are altogether, used to say how many the grid is hiding
    Private totalScreenings As Integer = 0

    'made once and reused, otherwise a new font would be made for every row every single refresh
    Private rowBoldFont As New Font("Segoe UI", 9.75!, FontStyle.Bold)

    'turns all the nav buttons back to see through, used when the menu first opens
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
        btnKiosk.BackColor = Color.Transparent
    End Sub

    'a button is pink the whole time its form is open, so more than one can be pink at once
    Private Sub SetActive(btn As Button)
        btn.BackColor = HighlightBack
    End Sub

    'lights a button up a bit when the mouse goes over it, unless its form is already open
    Private Sub NavButton_MouseEnter(sender As Object, e As EventArgs) Handles btnBookings.MouseEnter,
        btnFindBooking.MouseEnter, btnScreenings.MouseEnter, btnCustomers.MouseEnter, btnFilms.MouseEnter,
        btnScreens.MouseEnter, btnFood.MouseEnter, btnReports.MouseEnter, btnLogs.MouseEnter, btnSettings.MouseEnter,
        btnKiosk.MouseEnter

        Dim btn As Button = CType(sender, Button)
        If btn.BackColor <> HighlightBack Then
            btn.BackColor = SidebarHover
        End If
    End Sub

    'puts it back to normal when the mouse moves off it again
    Private Sub NavButton_MouseLeave(sender As Object, e As EventArgs) Handles btnBookings.MouseLeave,
        btnFindBooking.MouseLeave, btnScreenings.MouseLeave, btnCustomers.MouseLeave, btnFilms.MouseLeave,
        btnScreens.MouseLeave, btnFood.MouseLeave, btnReports.MouseLeave, btnLogs.MouseLeave, btnSettings.MouseLeave,
        btnKiosk.MouseLeave

        Dim btn As Button = CType(sender, Button)
        If btn.BackColor <> HighlightBack Then
            btn.BackColor = Color.Transparent
        End If
    End Sub

    'opens a form from the menu, highlights the button that opened it, and remembers which button
    'belongs to that form so the highlight can be taken off again when the form is closed
    Private Sub OpenForm(frm As Form, btn As Button)
        SetActive(btn)

        'the form holds on to its button in Tag, the same way the seat map holds a SeatID
        frm.Tag = btn

        'take the handler off first, otherwise opening the same form twice would add a second one
        RemoveHandler frm.FormClosed, AddressOf OpenedForm_Closed
        AddHandler frm.FormClosed, AddressOf OpenedForm_Closed

        frm.Show()

        'if it was already open but minimised, bringing it to the front on its own does nothing,
        'so it has to be put back to a normal window first
        If frm.WindowState = FormWindowState.Minimized Then
            frm.WindowState = FormWindowState.Normal
        End If

        frm.BringToFront()
    End Sub

    'runs when a form opened from the menu is closed, however it was closed, including the X in
    'the corner, and puts its button back to normal
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

    'puts the machine into self service mode. the kiosk fills the whole screen and there is no way
    'out of it for a customer, so it is worth saying what is about to happen before it opens
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

    Private Sub btnSettings_Click(sender As Object, e As EventArgs) Handles btnSettings.Click
        OpenForm(frmSettings, btnSettings)
    End Sub

    'closes anything the user opened from the menu, so logging out does not leave somebody elses
    'bookings or customer details sitting on the screen
    Private Sub CloseOpenedForms()
        'counted backwards because closing a form takes it out of the list, which would make a
        'forwards loop skip every other one
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

        'tells the closing event this is a log out and not somebody quitting the program
        loggingOut = True
        Me.Close()

        frmLogin.Show()
        ApplyThemeToAllForms()
    End Sub

    'closing the menu with the X used to leave the program running in the background with no
    'window on screen, so now it asks and then shuts everything down properly
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

    'the cards can be clicked to jump to the screen they are about, but only if that person is
    'allowed there, which is the same as whether that sidebar button is showing
    Private Sub OpenFromCard(frm As Form, btn As Button)
        If btn.Visible Then
            OpenForm(frm, btn)
        End If
    End Sub

    'the labels sit on top of the panel so they need the click handler as well as the panel
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
        'the last card is money for a manager and snacks for staff, so it opens a different screen
        If UserAccessLevel = 1 Then
            OpenFromCard(frmSalesReport, btnReports)
        Else
            OpenFromCard(frmFoodItems, btnFood)
        End If
    End Sub

    'gives one card and everything sitting on it the hand pointer, so it looks clickable
    Private Sub HandCard(card As Panel, title As Label, value As Label, sub1 As Label)
        card.Cursor = Cursors.Hand
        title.Cursor = Cursors.Hand
        value.Cursor = Cursors.Hand
        sub1.Cursor = Cursors.Hand
    End Sub

    'makes the cards that actually go somewhere show the hand pointer
    Private Sub SetCardCursors()
        HandCard(pnlCard2, lblCardTitle2, lblStat2, lblCardSub2)
        HandCard(pnlCard3, lblCardTitle3, lblStat3, lblCardSub3)

        If UserAccessLevel = 1 Then
            HandCard(pnlCard1, lblCardTitle1, lblStat1, lblCardSub1)
            HandCard(pnlCard4, lblCardTitle4, lblStat4, lblCardSub4)
        End If
    End Sub

    'lights a card up when the mouse is over it, but only the ones that actually go somewhere,
    'which are the ones that were given the hand pointer
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

    'the labels sit on top of the card so they have to report the mouse moving too
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

    'puts a little bit of help on a card and all of its labels in one go
    Private Sub TipCard(tips As ToolTip, card As Panel, title As Label, value As Label, sub1 As Label, message As String)
        tips.SetToolTip(card, message)
        tips.SetToolTip(title, message)
        tips.SetToolTip(value, message)
        tips.SetToolTip(sub1, message)
    End Sub

    'explains what everything on the menu means when the mouse rests on it
    Private Sub SetToolTips()
        Dim tips As New ToolTip
        tips.AutoPopDelay = 8000
        tips.InitialDelay = 500

        TipCard(tips, pnlCard1, lblCardTitle1, lblStat1, lblCardSub1, "How many films are on the system. Click to manage them.")
        TipCard(tips, pnlCard2, lblCardTitle2, lblStat2, lblCardSub2, "How many screenings are scheduled. Click to manage them.")

        If UserAccessLevel = 1 Then
            TipCard(tips, pnlCard3, lblCardTitle3, lblStat3, lblCardSub3, "How many bookings have been made. Click to make one.")
            TipCard(tips, pnlCard4, lblCardTitle4, lblStat4, lblCardSub4, "Everything taken, split into tickets and concessions. Click for the sales report.")
        Else
            TipCard(tips, pnlCard3, lblCardTitle3, lblStat3, lblCardSub3, "How many seats have been sold. Click to make a booking.")
            TipCard(tips, pnlCard4, lblCardTitle4, lblStat4, lblCardSub4, "How many food and drink items have been sold.")
        End If

        tips.SetToolTip(txtSearch, "Type part of a film or screen name to narrow the list down")
        tips.SetToolTip(cboShow, "Choose which screenings the list shows")
        tips.SetToolTip(btnRefresh, "Update the figures now. F5 does the same.")
        tips.SetToolTip(dgvWhatsOn, "Double click a screening to sell tickets for it")
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
        tips.SetToolTip(btnSettings, "Backups, password and appearance")
        tips.SetToolTip(btnLogout, "Log out and go back to the login screen")
    End Sub

    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        secondsCounter = 0
        RefreshDashboard()
    End Sub

    'F5 refreshes the dashboard, which is what most programs use that key for, and escape empties
    'the search box because that is quicker than deleting it
    Private Sub frmMainMenuV2_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.F5 Then
            secondsCounter = 0
            RefreshDashboard()
        ElseIf e.KeyCode = Keys.Escape And txtSearch.Text <> "" Then
            txtSearch.Text = ""
        End If
    End Sub

    'typing in the search box or changing the filter reloads the list straight away
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

    'fills the little drop down that says which screenings to show
    Private Sub FillShowFilter()
        cboShow.Items.Add("All screenings")
        cboShow.Items.Add("Today only")
        cboShow.Items.Add("Today and after")
        cboShow.Items.Add("Already been")
        cboShow.SelectedIndex = 0
    End Sub

    'turns the drop down choice into the bit of SQL that goes on the end of the WHERE
    'this is not user input so it is safe to join on to the query, the search box is still a
    'parameter because that is something somebody types
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

    'opens the booking screen already showing the screening on the row that was picked
    Private Sub OpenBookingForRow(rowIndex As Integer)
        If rowIndex < 0 Or rowIndex >= dgvWhatsOn.Rows.Count Then
            Exit Sub
        End If

        Dim screeningID As Long = CLng(dgvWhatsOn.Rows(rowIndex).Cells("ScreeningID").Value)

        OpenForm(frmBookings, btnBookings)
        'has to be after the form is shown, because its combo is filled in when it loads
        frmBookings.SelectScreening(screeningID)
    End Sub

    Private Sub dgvWhatsOn_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvWhatsOn.CellDoubleClick
        OpenBookingForRow(e.RowIndex)
    End Sub

    'clicking a column heading sorts the list, which moves the rows about, so the bold and the
    'colouring have to be worked out again for their new positions
    Private Sub dgvWhatsOn_Sorted(sender As Object, e As EventArgs) Handles dgvWhatsOn.Sorted
        ColourOccupancy()
        MarkUpcomingScreenings()
    End Sub

    'right clicking remembers and highlights the row under the mouse, otherwise the menu would
    'work on whichever row happened to be selected before
    Private Sub dgvWhatsOn_CellMouseDown(sender As Object, e As DataGridViewCellMouseEventArgs) Handles dgvWhatsOn.CellMouseDown
        If e.Button = MouseButtons.Right And e.RowIndex >= 0 Then
            rightClickedRow = e.RowIndex

            'the row is selected rather than moving the current cell, because the first column is
            'the hidden ScreeningID and the current cell is not allowed to be on a hidden column
            dgvWhatsOn.ClearSelection()
            dgvWhatsOn.Rows(e.RowIndex).Selected = True
        End If
    End Sub

    Private Sub GridMenuBook_Click(sender As Object, e As EventArgs)
        OpenBookingForRow(rightClickedRow)
    End Sub

    'copies the details of the right clicked screening so they can be pasted into an email or a
    'message without typing them out again
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

    'builds the little menu that appears when a screening is right clicked
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

    'works out a greeting based on the current time of day
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

    'reads the totals for the four cards along the top, and the smaller line underneath each one
    'the last two cards are different for staff because they should not be looking at the takings
    Private Sub LoadStats()
        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn

            'the first two cards are the same whoever is logged in
            SQLCmd.CommandText = "SELECT COUNT(*) FROM tblFilm"
            lblStat1.Text = SQLCmd.ExecuteScalar().ToString()

            'a film nobody has scheduled is not earning anything, so it is worth pointing out
            SQLCmd.CommandText = "SELECT COUNT(*) FROM tblFilm WHERE FilmID IN (SELECT FilmID FROM tblScreening)"
            lblCardSub1.Text = SQLCmd.ExecuteScalar().ToString() & " have screenings"

            SQLCmd.CommandText = "SELECT COUNT(*) FROM tblScreening"
            totalScreenings = CInt(SQLCmd.ExecuteScalar())
            lblStat2.Text = totalScreenings.ToString()

            SQLCmd.CommandText = "SELECT COUNT(*) FROM tblScreening WHERE ScreeningDate >= @Today " &
                                 "AND (ScreeningStatus IS NULL OR ScreeningStatus <> 'Cancelled')"
            SQLCmd.Parameters.AddWithValue("@Today", Date.Today)
            lblCardSub2.Text = SQLCmd.ExecuteScalar().ToString() & " still to come"
            'the parameter has to come off again because the queries after this one do not use it
            SQLCmd.Parameters.Clear()

            'a cancelled booking has been refunded, so none of it counts as money taken. the sales
            'report already left them out but this screen did not, so the two disagreed about the
            'takings. the seat counts below need no filter because cancelling deletes the seat rows
            If UserAccessLevel = 1 Then
                'managers get the business figures
                SQLCmd.CommandText = "SELECT COUNT(*) FROM tblBooking WHERE BookingStatus <> @Cancelled"
                SQLCmd.Parameters.AddWithValue("@Cancelled", BookingCancelled)
                Dim bookings As Integer = CInt(SQLCmd.ExecuteScalar())
                lblStat3.Text = bookings.ToString()

                SQLCmd.CommandText = "SELECT COUNT(*) FROM tblBookingSeat"
                SQLCmd.Parameters.Clear()
                lblCardSub3.Text = SQLCmd.ExecuteScalar().ToString() & " seats sold"

                'SUM comes back empty if there are no bookings at all so that has to be checked
                SQLCmd.CommandText = "SELECT SUM(TotalCost) FROM tblBooking WHERE BookingStatus <> @Cancelled"
                SQLCmd.Parameters.AddWithValue("@Cancelled", BookingCancelled)
                Dim takingsResult = SQLCmd.ExecuteScalar()
                Dim takings As Double = 0
                If takingsResult IsNot Nothing AndAlso Not IsDBNull(takingsResult) Then
                    takings = CDbl(takingsResult)
                End If

                'the total on a booking is the tickets plus any food, so the concessions side has to
                'be worked out on its own and taken off to leave what the tickets brought in.
                'the food rows are deliberately kept on a cancelled booking, so this has to go back
                'to tblBooking to leave the refunded ones out
                SQLCmd.CommandText = "SELECT SUM(Quantity * FoodItemPrice) " &
                                     "FROM (tblOrderItem INNER JOIN tblFoodItem ON tblOrderItem.FoodItemID = tblFoodItem.FoodItemID) " &
                                     "INNER JOIN tblBooking ON tblOrderItem.BookingID = tblBooking.BookingID " &
                                     "WHERE tblBooking.BookingStatus <> @Cancelled"
                SQLCmd.Parameters.Clear()
                SQLCmd.Parameters.AddWithValue("@Cancelled", BookingCancelled)
                Dim foodResult = SQLCmd.ExecuteScalar()
                Dim concessions As Double = 0
                If foodResult IsNot Nothing AndAlso Not IsDBNull(foodResult) Then
                    concessions = CDbl(foodResult)
                End If

                Dim tickets As Double = takings - concessions

                lblStat4.Text = FormatCurrency(takings)
                If takings = 0 Then
                    lblCardSub4.Text = "nothing taken yet"
                Else
                    'split it so it is obvious how much of the money is tickets and how much is snacks
                    lblCardSub4.Text = FormatCurrency(tickets) & " tickets | " & FormatCurrency(concessions) & " concessions"
                End If
            Else
                'staff get numbers that are useful on a shift instead of anything about money
                SQLCmd.CommandText = "SELECT COUNT(*) FROM tblBookingSeat"
                lblStat3.Text = SQLCmd.ExecuteScalar().ToString()

                SQLCmd.CommandText = "SELECT COUNT(*) FROM tblBooking WHERE BookingStatus <> @Cancelled"
                SQLCmd.Parameters.AddWithValue("@Cancelled", BookingCancelled)
                lblCardSub3.Text = "across " & SQLCmd.ExecuteScalar().ToString() & " bookings"

                'a cancelled booking's snacks were refunded so they are not still to be handed over
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

    'finds whichever film has sold the most seats so far
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

    'works out how full the whole cinema is across every screening on the system
    Private Function GetOverallText() As String
        Dim SQLCmd As New OleDbCommand
        SQLCmd.Connection = cn

        'every seat there is to sell, counting a screen once for each of its screenings
        SQLCmd.CommandText = "SELECT SUM(sc.ScreenCapacity) " &
                             "FROM tblScreening AS s INNER JOIN tblScreen AS sc ON s.ScreenID = sc.ScreenID " &
                             "WHERE (s.ScreeningStatus IS NULL OR s.ScreeningStatus <> 'Cancelled')"
        Dim capacity = SQLCmd.ExecuteScalar()

        If capacity Is Nothing OrElse IsDBNull(capacity) OrElse CInt(capacity) = 0 Then
            Return "No screenings to report on yet"
        End If

        'only seats that belong to a screening still on the system are counted
        SQLCmd.CommandText = "SELECT COUNT(*) " &
                             "FROM (tblBookingSeat AS bs INNER JOIN tblBooking AS b ON bs.BookingID = b.BookingID) " &
                             "INNER JOIN tblScreening AS s ON b.ScreeningID = s.ScreeningID"
        Dim sold As Integer = CInt(SQLCmd.ExecuteScalar())

        Dim percent As Integer = CInt(sold * 100 / CInt(capacity))

        Return sold & " of " & capacity & " seats sold across all screenings (" & percent & "%)"
    End Function

    'the line under the cards, it says what is showing next and warns about anything that is
    'nearly sold out so staff can put more seats on or tell a manager
    Private Function GetNextUpText() As String
        Dim result As String = "Nothing is scheduled from today onwards."

        Dim SQLCmd As New OleDbCommand
        SQLCmd.Connection = cn
        'the time is text in the form HH:MM so it can be compared as text and still come out in
        'the right order
        SQLCmd.CommandText = "SELECT TOP 1 f.FilmTitle, sc.ScreenName, s.ScreeningDate, s.ScreeningTime " &
                             "FROM (tblScreening AS s INNER JOIN tblFilm AS f ON s.FilmID = f.FilmID) " &
                             "INNER JOIN tblScreen AS sc ON s.ScreenID = sc.ScreenID " &
                             "WHERE (s.ScreeningDate > @Today " &
                             "OR (s.ScreeningDate = @Today2 AND s.ScreeningTime >= @Now)) " &
                             "AND (s.ScreeningStatus IS NULL OR s.ScreeningStatus <> 'Cancelled') " &
                             "ORDER BY s.ScreeningDate, s.ScreeningTime, s.ScreeningID"
        'the parameters go on in the order they appear in the query above, and today has to go on
        'twice because it is used twice
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

    'counts the screenings still to come that are at least 80 percent sold
    'the bit in brackets counts the seats sold for each screening straight off tblBookingSeat.
    'it used to join tblBookingSeat to tblBooking in there to find out which screening each seat
    'was for, but the screening is written on the seat row itself now so the join is not needed
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

    'fills in the two summary lines that sit underneath the cards
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

            'a manager wants to know how the whole place is doing, staff would rather know what
            'everybody is coming in to see
            If UserAccessLevel = 1 Then
                lblTopFilm.Text = GetOverallText()
            Else
                lblTopFilm.Text = GetTopFilmText()
            End If

            cn.Close()
        End If
    End Sub

    'fills the grid with the screenings the user has asked for, what film each one is and how
    'many seats have gone
    Private Sub LoadWhatsOn()
        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            'the bits in brackets count the booked seats for each screening as the query goes along,
            'the second one takes that away from the capacity to get how many are still free.
            'both of them read the screening straight off the seat row rather than joining back to
            'tblBooking to find it, which is what they used to do
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

            'the same search goes in twice because each question mark in the query needs its own
            'parameter, and they have to be added in the order they appear
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

            'remember how far down the grid was scrolled, otherwise the once a minute refresh
            'would jump the user back to the top while they were reading it
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

    'puts screenings that are still to come in bold so they stand out from ones already shown
    'it is done this way round because greying out the past ones made the whole grid look faded
    Private Sub MarkUpcomingScreenings()
        dgvWhatsOn.AlternatingRowsDefaultCellStyle.BackColor = AltRowBack

        For i As Integer = 0 To dgvWhatsOn.Rows.Count - 1
            Dim showDate As Date = CDate(dgvWhatsOn.Rows(i).Cells("ScreeningDate").Value)

            If showDate >= Date.Today Then
                dgvWhatsOn.Rows(i).DefaultCellStyle.Font = rowBoldFont
            Else
                'setting it back to nothing makes the row use the grids own font again, which
                'matters after the user has sorted a column and the rows have swapped round
                dgvWhatsOn.Rows(i).DefaultCellStyle.Font = Nothing
            End If

            'hovering over a row explains the numbers in words
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

    'says underneath the grid how many screenings are being shown, and whether any are hidden
    'because of the search box or the filter
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

    'an empty grid on its own looks like something has gone wrong, so when there is nothing to
    'list the grid is hidden and a message is put in exactly the same space explaining why
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

        'the label takes the grids place so it is painted the same colour as the grid would be
        lblNoRows.BackColor = InputBack
        dgvWhatsOn.Visible = False
        lblNoRows.Visible = True
    End Sub

    'adds a column working out what percentage of each screening has been sold, and a column
    'saying when it is in words rather than making the user work it out from the date
    Private Sub AddPercentFull(dt As DataTable)
        dt.Columns.Add("PercentFull", GetType(Integer))
        dt.Columns.Add("WhenText", GetType(String))

        For i As Integer = 0 To dt.Rows.Count - 1
            Dim capacity As Integer = CInt(dt.Rows(i)("ScreenCapacity"))
            Dim booked As Integer = CInt(dt.Rows(i)("SeatsBooked"))

            'a screen with no seats would cause a divide by zero so it is checked first
            If capacity > 0 Then
                dt.Rows(i)("PercentFull") = CInt(booked * 100 / capacity)
            Else
                dt.Rows(i)("PercentFull") = 0
            End If

            'a screening with nothing left is worth saying in words, a number on its own is easy
            'to miss when somebody is busy
            If capacity - booked <= 0 Then
                dt.Rows(i)("WhenText") = DescribeWhen(CDate(dt.Rows(i)("ScreeningDate"))) & " - full"
            Else
                dt.Rows(i)("WhenText") = DescribeWhen(CDate(dt.Rows(i)("ScreeningDate")))
            End If
        Next
    End Sub

    'turns a screening date into something readable like Today or In 3 days
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

    'makes a screening that is filling up stand out, red for nearly full and orange for half full
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
                'an empty colour means use the grids normal one, which is needed in case this
                'cell was coloured in before the user sorted a column
                cell.Style.ForeColor = Color.Empty
                cell.Style.SelectionForeColor = Color.Empty
                cell.Style.Font = Nothing
            End If

            'the sold out wording is put on in AddPercentFull, this just makes it red as well
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

    'gives the grid columns proper headings and sensible widths
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
        'the ID is only there so double clicking knows which screening was picked
        dgvWhatsOn.Columns("ScreeningID").Visible = False

        'a manager wants to see how many have sold, staff at the till want to know how many are
        'still going, so each of them only gets the column that is useful to them
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
        'put the When column next to the time rather than stuck on the end
        dgvWhatsOn.Columns("WhenText").DisplayIndex = 4

        'just the date is wanted, not the 00:00:00 on the end of it
        dgvWhatsOn.Columns("ScreeningDate").DefaultCellStyle.Format = "dd/MM/yyyy"

        'the numbers line up better on the right, the way they would on a receipt
        dgvWhatsOn.Columns("ScreenCapacity").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dgvWhatsOn.Columns("SeatsBooked").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dgvWhatsOn.Columns("SeatsLeft").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dgvWhatsOn.Columns("PercentFull").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

        'match the font the rest of the form uses
        dgvWhatsOn.DefaultCellStyle.Font = New Font("Segoe UI", 9.75!)
        dgvWhatsOn.ColumnHeadersDefaultCellStyle.Font = rowBoldFont

        'nothing is picked when the form opens so the first row should not look selected
        dgvWhatsOn.ClearSelection()
    End Sub

    'sets the menu up for whoever is logged in, managers and staff see different things
    Private Sub ConfigureAccessLevel()
        lblWelcome.Text = GetGreeting() & ", " & frmLogin.globalusername

        If UserAccessLevel = 1 Then
            lblSubtitle.Text = "Signed in as a manager. Here is how the cinema is doing."
            lblCardTitle3.Text = "Bookings taken"
            lblCardTitle4.Text = "Money taken"

            btnFilms.Visible = True
            btnScreens.Visible = True
            btnFood.Visible = True
            btnReports.Visible = True
            btnLogs.Visible = True
            lblNavManage.Visible = True
        Else
            'staff get a reminder instead of the manager blurb, and the two cards on the right
            'swap to things they can actually use instead of anything about money
            lblSubtitle.Text = GetStaffMessage()
            lblCardTitle3.Text = "Seats sold"
            lblCardTitle4.Text = "Snacks sold"

            btnFilms.Visible = False
            btnScreens.Visible = False
            btnFood.Visible = False
            btnReports.Visible = False
            btnLogs.Visible = False
            'the heading would be sat above nothing if the buttons under it are all hidden
            lblNavManage.Visible = False
        End If
    End Sub

    'picks one of the little staff messages at random so the menu is not the same every shift
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

        'the timer ticks once a second, so counting sixty of them refreshes the figures each minute
        secondsCounter = secondsCounter + 1

        If secondsCounter >= 60 Then
            secondsCounter = 0

            'only refresh if the database can actually be reached. without this check a database
            'that had been moved or was open in Access would put an error message on the screen
            'every single minute, and the user could not get rid of them
            If DbConnectQuiet() Then
                RefreshDashboard()
            End If
        End If
    End Sub

    'puts the date and time in the top corner and keeps it tucked against the right hand edge,
    'which it has to be worked out for because the text gets wider and narrower
    Private Sub ShowClock()
        lblClock.Text = Format(Now, "ddd d MMM   HH:mm:ss")
        lblClock.Left = pnlHeader.Width - lblClock.Width - 30
    End Sub

    'works out where everything that has to stretch goes. it is done in code rather than being
    'left to the designer because the window can be resized, and because windows makes the form
    'a bit shorter than it was drawn on a smaller screen, which used to push the bottom line off
    Private Sub LayoutDashboard()
        Dim edge As Integer = 23
        Dim gap As Integer = 17

        'the purple bars go right across the top and all the way down the side
        pnlHeader.Width = Me.ClientSize.Width
        pnlSidebar.Height = Me.ClientSize.Height - pnlSidebar.Top

        'log out sits at the bottom of the sidebar and the nav list fills what is above it
        btnLogout.Top = pnlSidebar.Height - btnLogout.Height - 22
        flpNav.Height = btnLogout.Top - flpNav.Top - 10

        'the grid fills everything between its heading and the bottom of the window, leaving
        'room for the two small lines underneath it
        dgvWhatsOn.Width = Me.ClientSize.Width - dgvWhatsOn.Left - edge
        dgvWhatsOn.Height = Me.ClientSize.Height - dgvWhatsOn.Top - 46

        'the four cards line up with the two ends of the grid
        Dim cardWidth As Integer = (dgvWhatsOn.Width - gap * 3) \ 4
        pnlCard1.Left = dgvWhatsOn.Left
        pnlCard2.Left = dgvWhatsOn.Left + cardWidth + gap
        pnlCard3.Left = dgvWhatsOn.Left + (cardWidth + gap) * 2
        'the last one is pinned to the right hand end instead of being worked out, so the rounding
        'off in the division does not leave a gap down the side
        pnlCard4.Left = dgvWhatsOn.Right - cardWidth

        pnlCard1.Width = cardWidth
        pnlCard2.Width = cardWidth
        pnlCard3.Width = cardWidth
        pnlCard4.Width = cardWidth

        'the search box, the filter and the refresh button are laid out from the right hand end
        'of the grid backwards, so they finish exactly where the grid finishes
        btnRefresh.Left = dgvWhatsOn.Right - btnRefresh.Width
        cboShow.Left = btnRefresh.Left - cboShow.Width - 10
        txtSearch.Left = cboShow.Left - txtSearch.Width - 12
        lblSearch.Left = txtSearch.Left - lblSearch.Width - 8

        'the two summary lines share the row under the cards
        lblTopFilm.Left = dgvWhatsOn.Right - lblTopFilm.Width
        lblAlerts.Width = lblTopFilm.Left - lblAlerts.Left - 12

        'and the two small lines go just underneath the grid
        lblVersion.Top = dgvWhatsOn.Bottom + 12
        lblGridCount.Top = dgvWhatsOn.Bottom + 13
        lblGridCount.Left = dgvWhatsOn.Right - lblGridCount.Width

        'the no screenings message covers exactly the same space the grid would have taken
        lblNoRows.Left = dgvWhatsOn.Left
        lblNoRows.Top = dgvWhatsOn.Top
        lblNoRows.Width = dgvWhatsOn.Width
        lblNoRows.Height = dgvWhatsOn.Height
    End Sub

    Private Sub frmMainMenuV2_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize
        'resizing happens once while the form is still being built, before the controls have all
        'been put on it, so nothing is moved until they are there
        If dgvWhatsOn Is Nothing Then
            Exit Sub
        End If

        LayoutDashboard()
        ShowClock()
    End Sub

    'coming back to the menu after making a booking is the moment the figures are most likely to
    'be out of date, so they get reloaded, but only if it has been a couple of seconds since the
    'last time or clicking about would reload it constantly
    Private Sub frmMainMenuV2_Activated(sender As Object, e As EventArgs) Handles MyBase.Activated
        If stillLoading Then
            Exit Sub
        End If

        If DateDiff(DateInterval.Second, lastRefresh, Now) < 2 Then
            Exit Sub
        End If

        'checked quietly first, because a message box here would pop up again the moment it was
        'closed and the form became active again
        If DbConnectQuiet() Then
            RefreshDashboard()
        End If
    End Sub

    'reloads everything on the dashboard
    Private Sub RefreshDashboard()
        lastRefresh = Now
        LoadStats()
        LoadHeadlines()
        LoadWhatsOn()
    End Sub

    Private Sub frmMainMenuV2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CommonFormStartup(Me)
        'lets the form see F5 before the control that has focus does
        Me.KeyPreview = True
        'the window can be made bigger but not smaller than it was designed
        Me.MinimumSize = Me.Size

        SetAllButtonsTransp()
        ConfigureAccessLevel()
        SetCardCursors()
        SetToolTips()
        SetGridMenu()
        FillShowFilter()
        LayoutDashboard()

        'everything is set up now so the search box and the filter are allowed to reload the grid
        stillLoading = False
        RefreshDashboard()

        ShowClock()
        timerClock.Start()

        'the search box gets the cursor so the user can just start typing, and so the first nav
        'button does not sit there with a focus box round it
        txtSearch.Select()
    End Sub


End Class
