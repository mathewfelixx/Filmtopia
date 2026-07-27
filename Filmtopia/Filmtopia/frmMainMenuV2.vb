Imports System.Data.OleDb

Public Class frmMainMenuV2

    'counts the timer ticks so the figures can be refreshed once a minute
    Private secondsCounter As Integer = 0

    'which row was right clicked, -1 means none
    Private rightClickedRow As Integer = -1

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
    End Sub

    'a button is pink the whole time its form is open, so more than one can be pink at once
    Private Sub SetActive(btn As Button)
        btn.BackColor = HighlightBack
    End Sub

    'lights a button up a bit when the mouse goes over it, unless its form is already open
    Private Sub NavButton_MouseEnter(sender As Object, e As EventArgs) Handles btnBookings.MouseEnter,
        btnFindBooking.MouseEnter, btnScreenings.MouseEnter, btnCustomers.MouseEnter, btnFilms.MouseEnter,
        btnScreens.MouseEnter, btnFood.MouseEnter, btnReports.MouseEnter, btnLogs.MouseEnter, btnSettings.MouseEnter

        Dim btn As Button = CType(sender, Button)
        If btn.BackColor <> HighlightBack Then
            btn.BackColor = SidebarHover
        End If
    End Sub

    'puts it back to normal when the mouse moves off it again
    Private Sub NavButton_MouseLeave(sender As Object, e As EventArgs) Handles btnBookings.MouseLeave,
        btnFindBooking.MouseLeave, btnScreenings.MouseLeave, btnCustomers.MouseLeave, btnFilms.MouseLeave,
        btnScreens.MouseLeave, btnFood.MouseLeave, btnReports.MouseLeave, btnLogs.MouseLeave, btnSettings.MouseLeave

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

    Private Sub btnLogout_Click(sender As Object, e As EventArgs) Handles btnLogout.Click
        LogedIn = False
        UserAccessLevel = 99
        ClearUserSettings()
        Me.Close()
        frmLogin.Show()
        ApplyThemeToAllForms()
    End Sub

    'the cards can be clicked to jump to the screen they are about, but only if that person is
    'allowed there, which is the same as whether that sidebar button is showing
    Private Sub OpenFromCard(frm As Form, btn As Button)
        If btn.Visible Then
            OpenForm(frm, btn)
        End If
    End Sub

    'the labels sit on top of the panel so they need the click handler as well as the panel
    Private Sub Card1_Click(sender As Object, e As EventArgs) Handles pnlCard1.Click, lblCardTitle1.Click, lblStat1.Click
        OpenFromCard(frmFilms, btnFilms)
    End Sub

    Private Sub Card2_Click(sender As Object, e As EventArgs) Handles pnlCard2.Click, lblCardTitle2.Click, lblStat2.Click
        OpenFromCard(frmScreenings, btnScreenings)
    End Sub

    Private Sub Card3_Click(sender As Object, e As EventArgs) Handles pnlCard3.Click, lblCardTitle3.Click, lblStat3.Click
        OpenFromCard(frmBookings, btnBookings)
    End Sub

    Private Sub Card4_Click(sender As Object, e As EventArgs) Handles pnlCard4.Click, lblCardTitle4.Click, lblStat4.Click
        'the last card is money for a manager and snacks for staff, so it opens a different screen
        If UserAccessLevel = 1 Then
            OpenFromCard(frmSalesReport, btnReports)
        Else
            OpenFromCard(frmFoodItems, btnFood)
        End If
    End Sub

    'makes the cards that actually go somewhere show the hand pointer
    Private Sub SetCardCursors()
        pnlCard2.Cursor = Cursors.Hand
        lblCardTitle2.Cursor = Cursors.Hand
        lblStat2.Cursor = Cursors.Hand
        pnlCard3.Cursor = Cursors.Hand
        lblCardTitle3.Cursor = Cursors.Hand
        lblStat3.Cursor = Cursors.Hand

        If UserAccessLevel = 1 Then
            pnlCard1.Cursor = Cursors.Hand
            lblCardTitle1.Cursor = Cursors.Hand
            lblStat1.Cursor = Cursors.Hand
            pnlCard4.Cursor = Cursors.Hand
            lblCardTitle4.Cursor = Cursors.Hand
            lblStat4.Cursor = Cursors.Hand
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
    Private Sub Card1_MouseEnter(sender As Object, e As EventArgs) Handles pnlCard1.MouseEnter, lblCardTitle1.MouseEnter, lblStat1.MouseEnter
        HoverCard(pnlCard1, True)
    End Sub

    Private Sub Card1_MouseLeave(sender As Object, e As EventArgs) Handles pnlCard1.MouseLeave, lblCardTitle1.MouseLeave, lblStat1.MouseLeave
        HoverCard(pnlCard1, False)
    End Sub

    Private Sub Card2_MouseEnter(sender As Object, e As EventArgs) Handles pnlCard2.MouseEnter, lblCardTitle2.MouseEnter, lblStat2.MouseEnter
        HoverCard(pnlCard2, True)
    End Sub

    Private Sub Card2_MouseLeave(sender As Object, e As EventArgs) Handles pnlCard2.MouseLeave, lblCardTitle2.MouseLeave, lblStat2.MouseLeave
        HoverCard(pnlCard2, False)
    End Sub

    Private Sub Card3_MouseEnter(sender As Object, e As EventArgs) Handles pnlCard3.MouseEnter, lblCardTitle3.MouseEnter, lblStat3.MouseEnter
        HoverCard(pnlCard3, True)
    End Sub

    Private Sub Card3_MouseLeave(sender As Object, e As EventArgs) Handles pnlCard3.MouseLeave, lblCardTitle3.MouseLeave, lblStat3.MouseLeave
        HoverCard(pnlCard3, False)
    End Sub

    Private Sub Card4_MouseEnter(sender As Object, e As EventArgs) Handles pnlCard4.MouseEnter, lblCardTitle4.MouseEnter, lblStat4.MouseEnter
        HoverCard(pnlCard4, True)
    End Sub

    Private Sub Card4_MouseLeave(sender As Object, e As EventArgs) Handles pnlCard4.MouseLeave, lblCardTitle4.MouseLeave, lblStat4.MouseLeave
        HoverCard(pnlCard4, False)
    End Sub

    'puts a little bit of help on a card and both of its labels in one go
    Private Sub TipCard(tips As ToolTip, card As Panel, title As Label, value As Label, message As String)
        tips.SetToolTip(card, message)
        tips.SetToolTip(title, message)
        tips.SetToolTip(value, message)
    End Sub

    'explains what everything on the menu means when the mouse rests on it
    Private Sub SetToolTips()
        Dim tips As New ToolTip
        tips.AutoPopDelay = 8000
        tips.InitialDelay = 500

        TipCard(tips, pnlCard1, lblCardTitle1, lblStat1, "How many films are on the system. Click to manage them.")
        TipCard(tips, pnlCard2, lblCardTitle2, lblStat2, "How many screenings are scheduled. Click to manage them.")

        If UserAccessLevel = 1 Then
            TipCard(tips, pnlCard3, lblCardTitle3, lblStat3, "How many bookings have been made. Click to make one.")
            TipCard(tips, pnlCard4, lblCardTitle4, lblStat4, "Everything taken from ticket sales. Click for the sales report.")
        Else
            TipCard(tips, pnlCard3, lblCardTitle3, lblStat3, "How many seats have been sold. Click to make a booking.")
            TipCard(tips, pnlCard4, lblCardTitle4, lblStat4, "How many food and drink items have been sold.")
        End If

        tips.SetToolTip(btnRefresh, "Update the figures now. F5 does the same.")
        tips.SetToolTip(btnBookings, "Make a new booking and pick seats")
        tips.SetToolTip(btnFindBooking, "Look up or cancel a booking")
        tips.SetToolTip(btnScreenings, "See and set up what is showing")
        tips.SetToolTip(btnCustomers, "Look up customer details")
        tips.SetToolTip(btnFilms, "Add and edit films")
        tips.SetToolTip(btnScreens, "Set up screens and their seating")
        tips.SetToolTip(btnFood, "Manage food and drink items")
        tips.SetToolTip(btnReports, "View the sales report")
        tips.SetToolTip(btnLogs, "See a history of what has been done")
        tips.SetToolTip(btnSettings, "Backups, password and appearance")
        tips.SetToolTip(btnLogout, "Log out and go back to the login screen")
    End Sub

    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        secondsCounter = 0
        RefreshDashboard()
    End Sub

    'F5 refreshes the dashboard, which is what most programs use that key for
    Private Sub frmMainMenuV2_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.F5 Then
            secondsCounter = 0
            RefreshDashboard()
        End If
    End Sub

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

    'builds the little menu that appears when a screening is right clicked
    Private Sub SetGridMenu()
        Dim gridMenu As New ContextMenuStrip
        Dim bookItem As ToolStripMenuItem = New ToolStripMenuItem("Make a booking for this screening")

        AddHandler bookItem.Click, AddressOf GridMenuBook_Click
        gridMenu.Items.Add(bookItem)

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

    'reads the totals for the four cards along the top
    'the last two cards are different for staff because they should not be looking at the takings
    Private Sub LoadStats()
        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn

            'the first two cards are the same whoever is logged in
            SQLCmd.CommandText = "SELECT COUNT(*) FROM tblFilm"
            lblStat1.Text = SQLCmd.ExecuteScalar().ToString()

            SQLCmd.CommandText = "SELECT COUNT(*) FROM tblScreening"
            lblStat2.Text = SQLCmd.ExecuteScalar().ToString()

            If UserAccessLevel = 1 Then
                'managers get the business figures
                SQLCmd.CommandText = "SELECT COUNT(*) FROM tblBooking"
                lblStat3.Text = SQLCmd.ExecuteScalar().ToString()

                'SUM comes back empty if there are no bookings at all so that has to be checked
                SQLCmd.CommandText = "SELECT SUM(TotalCost) FROM tblBooking"
                Dim takings = SQLCmd.ExecuteScalar()
                If takings Is Nothing OrElse IsDBNull(takings) Then
                    lblStat4.Text = FormatCurrency(0)
                Else
                    lblStat4.Text = FormatCurrency(takings)
                End If
            Else
                'staff get numbers that are useful on a shift instead of anything about money
                SQLCmd.CommandText = "SELECT COUNT(*) FROM tblBookingSeat"
                lblStat3.Text = SQLCmd.ExecuteScalar().ToString()

                SQLCmd.CommandText = "SELECT SUM(Quantity) FROM tblOrderItem"
                Dim snacks = SQLCmd.ExecuteScalar()
                If snacks Is Nothing OrElse IsDBNull(snacks) Then
                    lblStat4.Text = "0"
                Else
                    lblStat4.Text = snacks.ToString()
                End If
            End If

            cn.Close()
        End If
    End Sub

    'finds whichever film has sold the most seats so far
    Private Sub LoadTopFilm()
        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT TOP 1 f.FilmTitle, COUNT(*) AS SeatsSold " &
                                 "FROM ((tblBookingSeat AS bs " &
                                 "INNER JOIN tblBooking AS b ON bs.BookingID = b.BookingID) " &
                                 "INNER JOIN tblScreening AS s ON b.ScreeningID = s.ScreeningID) " &
                                 "INNER JOIN tblFilm AS f ON s.FilmID = f.FilmID " &
                                 "GROUP BY f.FilmTitle " &
                                 "ORDER BY COUNT(*) DESC"

            Dim rs As OleDbDataReader = SQLCmd.ExecuteReader()
            If rs.Read() Then
                lblTopFilm.Text = "Everyone is watching " & rs("FilmTitle") & " - " & rs("SeatsSold") & " seats gone"
            Else
                lblTopFilm.Text = "No seats have been booked yet"
            End If
            rs.Close()
            cn.Close()
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

    'fills the grid with every screening, what film it is and how many seats have gone
    Private Sub LoadWhatsOn()
        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            'the bits in brackets count the booked seats for each screening as the query goes along,
            'the second one takes that away from the capacity to get how many are still free
            SQLCmd.CommandText = "SELECT s.ScreeningID, f.FilmTitle, sc.ScreenName, s.ScreeningDate, s.ScreeningTime, " &
                                 "sc.ScreenCapacity, " &
                                 "(SELECT COUNT(*) FROM tblBookingSeat AS bs " &
                                 "INNER JOIN tblBooking AS b ON bs.BookingID = b.BookingID " &
                                 "WHERE b.ScreeningID = s.ScreeningID) AS SeatsBooked, " &
                                 "sc.ScreenCapacity - (SELECT COUNT(*) FROM tblBookingSeat AS bs2 " &
                                 "INNER JOIN tblBooking AS b2 ON bs2.BookingID = b2.BookingID " &
                                 "WHERE b2.ScreeningID = s.ScreeningID) AS SeatsLeft " &
                                 "FROM (tblScreening AS s INNER JOIN tblFilm AS f ON s.FilmID = f.FilmID) " &
                                 "INNER JOIN tblScreen AS sc ON s.ScreenID = sc.ScreenID " &
                                 "ORDER BY s.ScreeningDate DESC, s.ScreeningTime"

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
            ShowGridSummary(dt)
        End If
    End Sub

    'puts screenings that are still to come in bold so they stand out from ones already shown
    'it is done this way round because greying out the past ones made the whole grid look faded
    Private Sub MarkUpcomingScreenings()
        dgvWhatsOn.AlternatingRowsDefaultCellStyle.BackColor = AltRowBack

        For i As Integer = 0 To dgvWhatsOn.Rows.Count - 1
            Dim showDate As Date = CDate(dgvWhatsOn.Rows(i).Cells("ScreeningDate").Value)

            If showDate >= Date.Today Then
                dgvWhatsOn.Rows(i).DefaultCellStyle.Font = New Font("Segoe UI", 9.75!, FontStyle.Bold)
            End If

            'hovering over a row explains the numbers in words
            Dim booked As Integer = CInt(dgvWhatsOn.Rows(i).Cells("SeatsBooked").Value)
            Dim capacity As Integer = CInt(dgvWhatsOn.Rows(i).Cells("ScreenCapacity").Value)
            Dim film As String = dgvWhatsOn.Rows(i).Cells("FilmTitle").Value.ToString()
            Dim tip As String = film & " - " & booked & " of " & capacity & " seats sold, " &
                                (capacity - booked) & " still free. Double click to make a booking."

            For Each cell As DataGridViewCell In dgvWhatsOn.Rows(i).Cells
                cell.ToolTipText = tip
            Next
        Next
    End Sub

    'says so plainly if there is nothing in the grid, and gives a manager a one line summary of
    'how the whole cinema is doing next to the heading
    Private Sub ShowGridSummary(dt As DataTable)
        If dt.Rows.Count = 0 Then
            lblTopFilm.Text = "There are no screenings on the system yet"
            lblTopFilm.Visible = True
            Exit Sub
        End If

        If UserAccessLevel = 1 Then
            Dim totalSeats As Integer = 0
            Dim totalBooked As Integer = 0

            For i As Integer = 0 To dt.Rows.Count - 1
                totalSeats = totalSeats + CInt(dt.Rows(i)("ScreenCapacity"))
                totalBooked = totalBooked + CInt(dt.Rows(i)("SeatsBooked"))
            Next

            Dim overall As Integer = 0
            If totalSeats > 0 Then
                overall = CInt(totalBooked * 100 / totalSeats)
            End If

            lblTopFilm.Text = totalBooked & " of " & totalSeats & " seats sold across all screenings (" & overall & "%)"
            lblTopFilm.Visible = True
        End If
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

            dt.Rows(i)("WhenText") = DescribeWhen(CDate(dt.Rows(i)("ScreeningDate")))
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
                cell.Style.Font = New Font("Segoe UI", 9.75!, FontStyle.Bold)
            ElseIf percent >= 50 Then
                cell.Style.ForeColor = OccupancyMed
                cell.Style.SelectionForeColor = OccupancyMed
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
        dgvWhatsOn.Columns("WhenText").Width = 90
        'put the When column next to the time rather than stuck on the end
        dgvWhatsOn.Columns("WhenText").DisplayIndex = 4

        'just the date is wanted, not the 00:00:00 on the end of it
        dgvWhatsOn.Columns("ScreeningDate").DefaultCellStyle.Format = "dd/MM/yyyy"

        'match the font the rest of the form uses
        dgvWhatsOn.DefaultCellStyle.Font = New Font("Segoe UI", 9.75!)
        dgvWhatsOn.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 9.75!, FontStyle.Bold)

        'nothing is picked when the form opens so the first row should not look selected
        dgvWhatsOn.ClearSelection()
    End Sub

    'sets the menu up for whoever is logged in, managers and staff see different things
    Private Sub ConfigureAccessLevel()
        lblWelcome.Text = GetGreeting() & ", " & frmLogin.globalusername

        If UserAccessLevel = 1 Then
            lblSubtitle.Text = "Signed in as a manager. Here is how the cinema is doing."
            'kept short so the summary line next to it has room
            lblWhatsOn.Text = "What is on"
            lblCardTitle3.Text = "Bookings taken"
            lblCardTitle4.Text = "Money taken"
            lblTopFilm.Visible = False

            btnFilms.Visible = True
            btnScreens.Visible = True
            btnFood.Visible = True
            btnReports.Visible = True
            btnLogs.Visible = True
        Else
            'staff get a random message instead of the business summary, and the two cards on the
            'right swap to things they can actually use instead of anything about money
            lblSubtitle.Text = GetStaffMessage()
            'kept short so it does not run into the most popular film line next to it
            lblWhatsOn.Text = "What is on"
            lblCardTitle3.Text = "Seats sold"
            lblCardTitle4.Text = "Snacks sold"
            lblTopFilm.Visible = True

            btnFilms.Visible = False
            btnScreens.Visible = False
            btnFood.Visible = False
            btnReports.Visible = False
            btnLogs.Visible = False
        End If
    End Sub

    Private Sub timerClock_Tick(sender As Object, e As EventArgs) Handles timerClock.Tick
        lblClock.Text = Format(Now, "ddd d MMM   HH:mm:ss")

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

    'reloads everything on the dashboard
    Private Sub RefreshDashboard()
        LoadStats()
        LoadWhatsOn()

        If UserAccessLevel <> 1 Then
            LoadTopFilm()
        End If
    End Sub

    Private Sub frmMainMenuV2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CommonFormStartup(Me)
        'lets the form see F5 before the control that has focus does
        Me.KeyPreview = True
        'the clock shows the date as well now so it needs starting further left to fit
        lblClock.Location = New Point(880, 22)
        SetAllButtonsTransp()
        ConfigureAccessLevel()
        SetCardCursors()
        SetToolTips()
        SetGridMenu()
        RefreshDashboard()

        lblClock.Text = Format(Now, "ddd d MMM   HH:mm:ss")
        timerClock.Start()
    End Sub

End Class
