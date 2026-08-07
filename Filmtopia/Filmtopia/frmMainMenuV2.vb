Imports System.Data.OleDb

Public Class frmMainMenuV2

    'remembers which nav button is the selected one so the hover colour does not fight with it
    Private activeNav As Button = Nothing

    'turns all the nav buttons back to see through so only the selected one is highlighted
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

    'highlights the button that was just clicked
    Private Sub SetActive(btn As Button)
        SetAllButtonsTransp()
        activeNav = btn
        btn.BackColor = HighlightBack
    End Sub

    'lights a button up a bit when the mouse goes over it, unless it is already the selected one
    Private Sub NavButton_MouseEnter(sender As Object, e As EventArgs) Handles btnBookings.MouseEnter,
        btnFindBooking.MouseEnter, btnScreenings.MouseEnter, btnCustomers.MouseEnter, btnFilms.MouseEnter,
        btnScreens.MouseEnter, btnFood.MouseEnter, btnReports.MouseEnter, btnLogs.MouseEnter, btnSettings.MouseEnter

        Dim btn As Button = CType(sender, Button)
        If Not btn Is activeNav Then
            btn.BackColor = SidebarHover
        End If
    End Sub

    'puts it back to normal when the mouse moves off it again
    Private Sub NavButton_MouseLeave(sender As Object, e As EventArgs) Handles btnBookings.MouseLeave,
        btnFindBooking.MouseLeave, btnScreenings.MouseLeave, btnCustomers.MouseLeave, btnFilms.MouseLeave,
        btnScreens.MouseLeave, btnFood.MouseLeave, btnReports.MouseLeave, btnLogs.MouseLeave, btnSettings.MouseLeave

        Dim btn As Button = CType(sender, Button)
        If Not btn Is activeNav Then
            btn.BackColor = Color.Transparent
        End If
    End Sub

    Private Sub btnBookings_Click(sender As Object, e As EventArgs) Handles btnBookings.Click
        SetActive(btnBookings)
        frmBookings.Show()
    End Sub

    Private Sub btnFindBooking_Click(sender As Object, e As EventArgs) Handles btnFindBooking.Click
        SetActive(btnFindBooking)
        frmBookingSearch.Show()
    End Sub

    Private Sub btnScreenings_Click(sender As Object, e As EventArgs) Handles btnScreenings.Click
        SetActive(btnScreenings)
        frmScreenings.Show()
    End Sub

    Private Sub btnCustomers_Click(sender As Object, e As EventArgs) Handles btnCustomers.Click
        SetActive(btnCustomers)
        frmCustomers.Show()
    End Sub

    Private Sub btnFilms_Click(sender As Object, e As EventArgs) Handles btnFilms.Click
        SetActive(btnFilms)
        frmFilms.Show()
    End Sub

    Private Sub btnScreens_Click(sender As Object, e As EventArgs) Handles btnScreens.Click
        SetActive(btnScreens)
        frmScreens.Show()
    End Sub

    Private Sub btnFood_Click(sender As Object, e As EventArgs) Handles btnFood.Click
        SetActive(btnFood)
        frmFoodItems.Show()
    End Sub

    Private Sub btnReports_Click(sender As Object, e As EventArgs) Handles btnReports.Click
        SetActive(btnReports)
        frmSalesReport.Show()
    End Sub

    Private Sub btnLogs_Click(sender As Object, e As EventArgs) Handles btnLogs.Click
        SetActive(btnLogs)
        frmLogs.Show()
    End Sub

    Private Sub btnSettings_Click(sender As Object, e As EventArgs) Handles btnSettings.Click
        SetActive(btnSettings)
        frmSettings.Show()
    End Sub

    Private Sub btnLogout_Click(sender As Object, e As EventArgs) Handles btnLogout.Click
        LogedIn = False
        UserAccessLevel = 99
        ClearUserSettings()
        Me.Close()
        frmLogin.Show()
        ApplyThemeToAllForms()
    End Sub

    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        LoadStats()
        LoadWhatsOn()

        If UserAccessLevel <> 1 Then
            LoadTopFilm()
        End If
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
            SQLCmd.CommandText = "SELECT f.FilmTitle, sc.ScreenName, s.ScreeningDate, s.ScreeningTime, " &
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
            dgvWhatsOn.DataSource = dt
            cn.Close()

            TidyGrid()
        End If
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
        dgvWhatsOn.Columns("ScreenName").Width = 130
        dgvWhatsOn.Columns("ScreeningDate").Width = 120
        dgvWhatsOn.Columns("ScreeningTime").Width = 90
        dgvWhatsOn.Columns("ScreenCapacity").Width = 90
        dgvWhatsOn.Columns("SeatsBooked").Width = 90
        dgvWhatsOn.Columns("SeatsLeft").Width = 90

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
            lblWhatsOn.Text = "What is on and how full it is"
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
        lblClock.Text = Format(Now, "HH:mm:ss")
    End Sub

    Private Sub frmMainMenuV2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CommonFormStartup(Me)
        SetAllButtonsTransp()
        ConfigureAccessLevel()
        LoadStats()
        LoadWhatsOn()

        'only staff get the most popular film line so there is no point looking it up for a manager
        If UserAccessLevel <> 1 Then
            LoadTopFilm()
        End If

        lblClock.Text = Format(Now, "HH:mm:ss")
        timerClock.Start()
    End Sub

End Class
