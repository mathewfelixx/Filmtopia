Imports System.Data.OleDb

'this screen is the only one in the program that is about the person using it rather than about
'the cinema. everything on it is filtered down to whoever is signed in, so there is no picking a
'user anywhere on it, and there is no access level check either, because nobody should be stopped
'from looking at their own work. what a manager gets extra is the money
Public Class frmUserOverview

    Private Sub frmUserOverview_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CommonFormStartup(Me)
        Me.KeyPreview = True

        ShowWhoIsSignedIn()
        ConfigureAccessLevel()
        LoadMyStats()

        'a week is a sensible amount to be looking at to begin with
        dtpActFrom.Value = Date.Today.AddDays(-7)
        dtpActTo.Value = Date.Today
        LoadActivityTypeFilter()
        LoadActivitySeverityFilter()
        stillLoadingActivity = False
        LoadActivity()

        'a month back for the sales, a week is too short to see much of what you have sold
        dtpSalesFrom.Value = Date.Today.AddMonths(-1)
        dtpSalesTo.Value = Date.Today
        stillLoadingSales = False
        LoadMySales()

        LoadMySettings()

        WriteLog("ACCOUNT", "My account screen opened")
    End Sub

    'escape closes the screen, same as everywhere else
    Private Sub frmUserOverview_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Escape Then
            If txtActSearch.Text <> "" Then
                txtActSearch.Text = ""
            Else
                Me.Close()
            End If
        End If
    End Sub

    'works out a greeting from the time of day. the main menu does the same thing, it is only a
    'few lines and copying it keeps this form from having to reach into another form for it
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

    'fills in the four lines at the top saying who is signed in and when
    Private Sub ShowWhoIsSignedIn()
        lblWelcome.Text = GetGreeting() & ", " & frmLogin.globalusername

        If UserAccessLevel = 1 Then
            lblRole.Text = "Manager"
        Else
            lblRole.Text = "Staff"
        End If

        lblSubSession.Text = "Signed in at " & Format(SessionStarted, "HH:mm") & ", " &
                             HowLongAgo(SessionStarted) & " ago"

        Dim lastTime As Date = LastSignedInBefore()

        'a brand new account, or one that has only ever signed in once, has nothing to show here
        If lastTime = Date.MinValue Then
            lblSubLastLogin.Text = "This is the first time this account has signed in"
        Else
            lblSubLastLogin.Text = "Last signed in " & Format(lastTime, "dd/MM/yyyy") & " at " &
                                   Format(lastTime, "HH:mm")
        End If
    End Sub

    'turns a length of time into something readable. saying 143 minutes is no use to anybody
    Private Function HowLongAgo(since As Date) As String
        Dim minutes As Integer = CInt(DateDiff(DateInterval.Minute, since, Date.Now))

        If minutes < 1 Then
            Return "less than a minute"
        ElseIf minutes = 1 Then
            Return "a minute"
        ElseIf minutes < 60 Then
            Return minutes & " minutes"
        ElseIf minutes < 120 Then
            Return "about an hour"
        Else
            Return "about " & (minutes \ 60) & " hours"
        End If
    End Function

    'when this account signed in before the time it is signed in now. the audit log already
    'records every login, so it is read back out of there rather than a new column being put on
    'tblLogin. keeping it in one place means the two can never end up disagreeing.
    'the top two are asked for and the second one is taken, because the newest one is this session
    Private Function LastSignedInBefore() As Date
        Dim answer As Date = Date.MinValue

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT TOP 2 LogDateTime " &
                                 "FROM tblLogs " &
                                 "WHERE LogUser = @Me AND LogType = @Auth AND LogMessage LIKE @LoggedIn " &
                                 "ORDER BY LogDateTime DESC, LogID DESC"
            SQLCmd.Parameters.AddWithValue("@Me", frmLogin.globalusername)
            SQLCmd.Parameters.AddWithValue("@Auth", "AUTH")
            SQLCmd.Parameters.AddWithValue("@LoggedIn", "%logged in successfully%")

            Dim rs As OleDbDataReader = SQLCmd.ExecuteReader()
            Dim howMany As Integer = 0

            While rs.Read()
                howMany = howMany + 1
                If howMany = 2 Then
                    answer = CDate(rs("LogDateTime"))
                End If
            End While

            rs.Close()
            cn.Close()
        End If

        Return answer
    End Function

    'the last card is money for a manager and snacks for everybody else, the same rule the
    'dashboard already follows. a member of staff sees what they have done, not what the cinema
    'is worth
    Private Sub ConfigureAccessLevel()
        If UserAccessLevel = 1 Then
            lblCardTitle4.Text = "What I have taken"
        Else
            lblCardTitle4.Text = "Snacks I have handed over"
        End If
    End Sub

    'reads a single number back off a command, coping with SUM coming back empty when there is
    'nothing at all to add up. every card below would need these four lines otherwise
    Private Function ScalarOrZero(SQLCmd As OleDbCommand) As Double
        Dim answer As Object = SQLCmd.ExecuteScalar()

        If answer Is Nothing OrElse IsDBNull(answer) Then
            Return 0
        End If

        Return CDbl(answer)
    End Function

    'fills the four cards along the top. all of it is narrowed to this login, so an empty screen
    'here means this person has not done anything yet rather than that the cinema is empty
    Private Sub LoadMyStats()
        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn

            'sales taken. a cancelled one was refunded so it is not a sale any more, same rule as
            'the dashboard and the sales report
            SQLCmd.CommandText = "SELECT COUNT(*) FROM tblBooking " &
                                 "WHERE LoginID = @Me AND BookingStatus <> @Cancelled"
            SQLCmd.Parameters.AddWithValue("@Me", CInt(CurrentLoginID))
            SQLCmd.Parameters.AddWithValue("@Cancelled", BookingCancelled)
            lblStat1.Text = CInt(ScalarOrZero(SQLCmd)).ToString()

            SQLCmd.CommandText = "SELECT COUNT(*) FROM tblBooking " &
                                 "WHERE LoginID = @Me AND BookingStatus = @Cancelled"
            SQLCmd.Parameters.Clear()
            SQLCmd.Parameters.AddWithValue("@Me", CInt(CurrentLoginID))
            SQLCmd.Parameters.AddWithValue("@Cancelled", BookingCancelled)
            lblCardSub1.Text = CInt(ScalarOrZero(SQLCmd)) & " of mine were cancelled"

            'seats sold. this reads tblBookingSeat on its own and picks the bookings out with a
            'subquery on one table, rather than joining the two, because a join inside a subquery
            'is the thing that broke two queries on the main menu.
            'there is no cancelled check needed, cancelling deletes the seat rows outright
            SQLCmd.CommandText = "SELECT COUNT(*) FROM tblBookingSeat " &
                                 "WHERE BookingID IN (SELECT BookingID FROM tblBooking WHERE LoginID = @Me)"
            SQLCmd.Parameters.Clear()
            SQLCmd.Parameters.AddWithValue("@Me", CInt(CurrentLoginID))
            lblStat2.Text = CInt(ScalarOrZero(SQLCmd)).ToString()

            SQLCmd.CommandText = "SELECT COUNT(*) FROM (SELECT DISTINCT ScreeningID FROM tblBookingSeat " &
                                 "WHERE BookingID IN (SELECT BookingID FROM tblBooking WHERE LoginID = @Me)) AS mine"
            SQLCmd.Parameters.Clear()
            SQLCmd.Parameters.AddWithValue("@Me", CInt(CurrentLoginID))
            lblCardSub2.Text = "across " & CInt(ScalarOrZero(SQLCmd)) & " screenings"

            'things done, which is this person's own share of the audit log
            SQLCmd.CommandText = "SELECT COUNT(*) FROM tblLogs WHERE LogUser = @Me"
            SQLCmd.Parameters.Clear()
            SQLCmd.Parameters.AddWithValue("@Me", frmLogin.globalusername)
            lblStat3.Text = CInt(ScalarOrZero(SQLCmd)).ToString()

            SQLCmd.CommandText = "SELECT COUNT(*) FROM tblLogs " &
                                 "WHERE LogUser = @Me AND LogDateTime >= @Since"
            SQLCmd.Parameters.Clear()
            SQLCmd.Parameters.AddWithValue("@Me", frmLogin.globalusername)
            SQLCmd.Parameters.AddWithValue("@Since", Date.Today.AddDays(-7))
            lblCardSub3.Text = CInt(ScalarOrZero(SQLCmd)) & " in the last 7 days"

            If UserAccessLevel = 1 Then
                LoadMoneyCard(SQLCmd)
            Else
                LoadSnacksCard(SQLCmd)
            End If

            cn.Close()
        End If
    End Sub

    'the manager version of the last card, what this person's sales came to
    Private Sub LoadMoneyCard(SQLCmd As OleDbCommand)
        SQLCmd.CommandText = "SELECT SUM(TotalCost) FROM tblBooking " &
                             "WHERE LoginID = @Me AND BookingStatus <> @Cancelled"
        SQLCmd.Parameters.Clear()
        SQLCmd.Parameters.AddWithValue("@Me", CInt(CurrentLoginID))
        SQLCmd.Parameters.AddWithValue("@Cancelled", BookingCancelled)
        Dim taken As Double = ScalarOrZero(SQLCmd)

        lblStat4.Text = FormatCurrency(taken)

        Dim sales As Integer = SafeInt(lblStat1.Text)

        If sales = 0 Then
            lblCardSub4.Text = "nothing taken yet"
        Else
            lblCardSub4.Text = FormatCurrency(taken / sales) & " a sale on average"
        End If
    End Sub

    'the staff version, how much food this person has actually handed over. a cancelled booking
    'was refunded so its food is not counted, and that is why this has to go back to tblBooking
    Private Sub LoadSnacksCard(SQLCmd As OleDbCommand)
        SQLCmd.CommandText = "SELECT SUM(Quantity) " &
                             "FROM tblOrderItem INNER JOIN tblBooking ON tblOrderItem.BookingID = tblBooking.BookingID " &
                             "WHERE tblBooking.LoginID = @Me AND tblBooking.BookingStatus <> @Cancelled"
        SQLCmd.Parameters.Clear()
        SQLCmd.Parameters.AddWithValue("@Me", CInt(CurrentLoginID))
        SQLCmd.Parameters.AddWithValue("@Cancelled", BookingCancelled)
        lblStat4.Text = CInt(ScalarOrZero(SQLCmd)).ToString()

        SQLCmd.CommandText = "SELECT COUNT(*) FROM (SELECT DISTINCT tblOrderItem.BookingID " &
                             "FROM tblOrderItem INNER JOIN tblBooking ON tblOrderItem.BookingID = tblBooking.BookingID " &
                             "WHERE tblBooking.LoginID = @Me AND tblBooking.BookingStatus <> @Cancelled) AS mine"
        SQLCmd.Parameters.Clear()
        SQLCmd.Parameters.AddWithValue("@Me", CInt(CurrentLoginID))
        SQLCmd.Parameters.AddWithValue("@Cancelled", BookingCancelled)
        lblCardSub4.Text = "on " & CInt(ScalarOrZero(SQLCmd)) & " of my sales"
    End Sub

    'ACTIVITY TAB ------------------------------------------------------------------------------
    'this is the audit log narrowed to one person. frmLogs is manager only and refuses staff, which
    'is right, they should not be reading everybody elses trail. there is no reason they cannot read
    'their own though, and that is what this is. there is no user box on it anywhere on purpose,
    'the whole point is that it can only ever be you

    'same cap frmLogs uses. the log only grows and nobody reads five hundred rows anyway
    Private Const MaxActivityRows As Integer = 500

    'true while the filter boxes are being filled, so adding an item does not run a search
    Private stillLoadingActivity As Boolean = True

    'how many entries matched altogether, which is not the same as how many fit on screen
    Private matchingActivity As Integer = 0

    'fills the area box with the log types this person has actually used, so it can never offer
    'an area they have never touched
    Private Sub LoadActivityTypeFilter()
        cboActType.Items.Clear()
        cboActType.Items.Add("All areas")

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT DISTINCT LogType FROM tblLogs WHERE LogUser = @Me ORDER BY LogType"
            SQLCmd.Parameters.AddWithValue("@Me", frmLogin.globalusername)

            Dim rs As OleDbDataReader = SQLCmd.ExecuteReader()
            While rs.Read()
                cboActType.Items.Add(rs("LogType").ToString())
            End While
            rs.Close()
            cn.Close()
        End If

        cboActType.SelectedIndex = 0
    End Sub

    'the levels are fixed so this one is just typed in, same as frmLogs does it
    Private Sub LoadActivitySeverityFilter()
        cboActSeverity.Items.Clear()
        cboActSeverity.Items.Add("All levels")
        cboActSeverity.Items.Add("Anything but routine")
        cboActSeverity.Items.Add(LogInfo)
        cboActSeverity.Items.Add(LogChange)
        cboActSeverity.Items.Add(LogWarning)
        cboActSeverity.Items.Add(LogSecurity)
        cboActSeverity.SelectedIndex = 0
    End Sub

    'builds the WHERE out of whichever filters are set. it is a string on its own because the same
    'conditions are wanted twice, once for the rows and once to count how many there were.
    'the user is not a filter here, it is welded into the front of every one
    Private Function BuildActivityWhere() As String
        Dim where As String = "WHERE LogUser = @Me AND LogDateTime >= @FromDate AND LogDateTime < @ToDate"

        If cboActType.SelectedIndex > 0 Then
            where = where & " AND LogType = @Type"
        End If

        If cboActSeverity.Text = "Anything but routine" Then
            where = where & " AND LogSeverity <> @Severity"
        ElseIf cboActSeverity.SelectedIndex > 0 Then
            where = where & " AND LogSeverity = @Severity"
        End If

        If txtActSearch.Text.Trim() <> "" Then
            where = where & " AND LogMessage LIKE @Search"
        End If

        Return where
    End Function

    'puts the values in for whatever the sub above added. OleDb does not go by the name, it goes by
    'the order they were added, so these have to go in in exactly the same order as up there
    Private Sub AddActivityParams(SQLCmd As OleDbCommand)
        SQLCmd.Parameters.AddWithValue("@Me", frmLogin.globalusername)
        SQLCmd.Parameters.AddWithValue("@FromDate", dtpActFrom.Value.Date)
        'the to date is moved on a day and the query uses less than, otherwise anything logged
        'during the last day would be missed because its time of day is after midnight
        SQLCmd.Parameters.AddWithValue("@ToDate", dtpActTo.Value.Date.AddDays(1))

        If cboActType.SelectedIndex > 0 Then
            SQLCmd.Parameters.AddWithValue("@Type", cboActType.Text)
        End If

        If cboActSeverity.Text = "Anything but routine" Then
            SQLCmd.Parameters.AddWithValue("@Severity", LogInfo)
        ElseIf cboActSeverity.SelectedIndex > 0 Then
            SQLCmd.Parameters.AddWithValue("@Severity", cboActSeverity.Text)
        End If

        If txtActSearch.Text.Trim() <> "" Then
            SQLCmd.Parameters.AddWithValue("@Search", "%" & txtActSearch.Text.Trim() & "%")
        End If
    End Sub

    'loads this person's own entries, newest first
    Private Sub LoadActivity()
        Dim dt As New DataTable
        matchingActivity = 0

        If DbConnect() Then
            Dim where As String = BuildActivityWhere()

            'how many there are altogether, before the cap is put on
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT COUNT(*) FROM tblLogs " & where
            AddActivityParams(SQLCmd)
            matchingActivity = CInt(SQLCmd.ExecuteScalar())

            'then the rows, newest first so the cap keeps the most recent ones
            Dim SQLCmd2 As New OleDbCommand
            SQLCmd2.Connection = cn
            SQLCmd2.CommandText = "SELECT TOP " & MaxActivityRows & " LogDateTime, LogSeverity, LogType, LogMessage " &
                                  "FROM tblLogs " & where & " " &
                                  "ORDER BY LogDateTime DESC, LogID DESC"
            AddActivityParams(SQLCmd2)
            Dim da As New OleDbDataAdapter(SQLCmd2)
            da.Fill(dt)
            cn.Close()
        End If

        dgvActivity.DataSource = dt

        'the user column is not on the grid at all. every row is this person, so a column saying
        'so on every line would only be taking room off the message
        If dgvActivity.Columns.Count > 0 Then
            dgvActivity.Columns("LogDateTime").HeaderText = "When"
            dgvActivity.Columns("LogSeverity").HeaderText = "Level"
            dgvActivity.Columns("LogType").HeaderText = "Area"
            dgvActivity.Columns("LogMessage").HeaderText = "What happened"

            dgvActivity.Columns("LogDateTime").Width = 150
            dgvActivity.Columns("LogSeverity").Width = 90
            dgvActivity.Columns("LogType").Width = 100
            dgvActivity.Columns("LogMessage").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill

            dgvActivity.Columns("LogDateTime").DefaultCellStyle.Format = "dd/MM/yyyy HH:mm:ss"
        End If

        ShowActivityCount()
        dgvActivity.ClearSelection()
    End Sub

    'says how much is on screen, and owns up when the cap has hidden some of it
    Private Sub ShowActivityCount()
        Dim showing As Integer = dgvActivity.Rows.Count

        If matchingActivity = 0 Then
            lblGridCount.Text = "Nothing of yours matches those filters"
        ElseIf showing < matchingActivity Then
            lblGridCount.Text = "Showing the newest " & showing & " of " & matchingActivity &
                                " entries - narrow the dates or the filters to see the rest"
        ElseIf matchingActivity = 1 Then
            lblGridCount.Text = "1 entry"
        Else
            lblGridCount.Text = showing & " entries"
        End If
    End Sub

    Private Sub btnActApply_Click(sender As Object, e As EventArgs) Handles btnActApply.Click
        LoadActivity()
    End Sub

    'puts every filter back to how it starts and shows the last week again
    Private Sub btnActClear_Click(sender As Object, e As EventArgs) Handles btnActClear.Click
        stillLoadingActivity = True
        dtpActFrom.Value = Date.Today.AddDays(-7)
        dtpActTo.Value = Date.Today
        cboActType.SelectedIndex = 0
        cboActSeverity.SelectedIndex = 0
        txtActSearch.Text = ""
        stillLoadingActivity = False

        LoadActivity()
    End Sub

    'changing any of the boxes searches again straight away
    Private Sub ActivityFilter_Changed(sender As Object, e As EventArgs) Handles cboActType.SelectedIndexChanged,
        cboActSeverity.SelectedIndexChanged, dtpActFrom.ValueChanged, dtpActTo.ValueChanged

        If stillLoadingActivity Then
            Exit Sub
        End If

        LoadActivity()
    End Sub

    'enter in the search box searches, rather than having to reach for the button
    Private Sub txtActSearch_KeyDown(sender As Object, e As KeyEventArgs) Handles txtActSearch.KeyDown
        If e.KeyCode = Keys.Enter Then
            LoadActivity()
            e.SuppressKeyPress = True
        End If
    End Sub

    'saves whats on screen to a csv. this goes through the shared one in modMain rather than being
    'written out again, because that walks the grids own columns, so if this grid ever gains one
    'the file gains it too instead of quietly leaving it off
    Private Sub btnActExport_Click(sender As Object, e As EventArgs) Handles btnActExport.Click
        If ExportGridToCsv(dgvActivity, "MyActivity.csv", "My Account") Then
            WriteLog("ACCOUNT", "Own activity exported, " & dgvActivity.Rows.Count & " entries")
            MessageBox.Show("Your activity has been exported.", "My Account", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    'a grid that was filled while its tab was hidden picks its first row when the tab is
    'finally shown, so the screen opens with a row highlighted that nobody clicked on
    Private Sub tabMe_SelectedIndexChanged(sender As Object, e As EventArgs) Handles tabMe.SelectedIndexChanged
        dgvActivity.ClearSelection()
        dgvMySales.ClearSelection()
        dgvMySettings.ClearSelection()
    End Sub

    'SALES TAB ---------------------------------------------------------------------------------
    'the sales this person took, which is a thing the program could not answer at all until
    'tblBooking started carrying a LoginID

    'true while the date boxes are being set up, so setting them does not run the query twice
    Private stillLoadingSales As Boolean = True

    'loads the bookings this login took between the two dates, newest first
    Private Sub LoadMySales()
        Dim dt As New DataTable

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn

            'the seat count is a subquery on tblBookingSeat on its own. it reads one table and
            'joins nothing, which is the shape that is safe here. the four table join stays out in
            'the FROM where it belongs, a join inside a subquery is what broke the main menu.
            'BookingDate has no time of day on it, so the to date can be used as it is. the log
            'range on the other tab has to add a day to its end date, they look inconsistent side
            'by side and they are not, the two columns are simply different sorts of date
            SQLCmd.CommandText = "SELECT tblBooking.BookingID, " &
                                 "CustomerForename & ' ' & CustomerSurname AS CustomerName, " &
                                 "FilmTitle, ScreeningDate, ScreeningTime, " &
                                 "(SELECT COUNT(*) FROM tblBookingSeat AS bs WHERE bs.BookingID = tblBooking.BookingID) AS Seats, " &
                                 "BookingDate, TotalCost, BookingStatus " &
                                 "FROM ((tblBooking INNER JOIN tblCustomer ON tblBooking.CustomerID = tblCustomer.CustomerID) " &
                                 "INNER JOIN tblScreening ON tblBooking.ScreeningID = tblScreening.ScreeningID) " &
                                 "INNER JOIN tblFilm ON tblScreening.FilmID = tblFilm.FilmID " &
                                 "WHERE tblBooking.LoginID = @Me AND BookingDate >= @From AND BookingDate <= @To " &
                                 "ORDER BY tblBooking.BookingID DESC"
            SQLCmd.Parameters.AddWithValue("@Me", CInt(CurrentLoginID))
            SQLCmd.Parameters.AddWithValue("@From", dtpSalesFrom.Value.Date)
            SQLCmd.Parameters.AddWithValue("@To", dtpSalesTo.Value.Date)

            Dim da As New OleDbDataAdapter(SQLCmd)
            da.Fill(dt)
            cn.Close()
        End If

        dgvMySales.DataSource = dt

        If dgvMySales.Columns.Count > 0 Then
            dgvMySales.Columns("BookingID").HeaderText = "Booking"
            dgvMySales.Columns("CustomerName").HeaderText = "Customer"
            dgvMySales.Columns("FilmTitle").HeaderText = "Film"
            dgvMySales.Columns("ScreeningDate").HeaderText = "Showing"
            dgvMySales.Columns("ScreeningTime").HeaderText = "Time"
            dgvMySales.Columns("Seats").HeaderText = "Seats"
            dgvMySales.Columns("BookingDate").HeaderText = "Sold on"
            dgvMySales.Columns("TotalCost").HeaderText = "Total"
            dgvMySales.Columns("BookingStatus").HeaderText = "Status"

            dgvMySales.Columns("BookingID").Width = 70
            dgvMySales.Columns("CustomerName").Width = 170
            dgvMySales.Columns("ScreeningDate").Width = 100
            dgvMySales.Columns("ScreeningTime").Width = 70
            dgvMySales.Columns("Seats").Width = 60
            dgvMySales.Columns("BookingDate").Width = 100
            dgvMySales.Columns("TotalCost").Width = 90
            dgvMySales.Columns("BookingStatus").Width = 90
            dgvMySales.Columns("FilmTitle").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill

            dgvMySales.Columns("ScreeningDate").DefaultCellStyle.Format = "dd/MM/yyyy"
            dgvMySales.Columns("BookingDate").DefaultCellStyle.Format = "dd/MM/yyyy"
            dgvMySales.Columns("TotalCost").DefaultCellStyle.Format = "C"
            dgvMySales.Columns("Seats").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            dgvMySales.Columns("TotalCost").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

            'staff are not shown money anywhere, so the column comes off rather than being blanked
            dgvMySales.Columns("TotalCost").Visible = (UserAccessLevel = 1)
        End If

        GreyOutCancelled()
        ShowSalesCount(dt)
        dgvMySales.ClearSelection()
    End Sub

    'a cancelled sale is greyed rather than hidden, it still happened and it is still yours.
    'the colour comes off the theme so it is still readable in dark mode
    Private Sub GreyOutCancelled()
        For Each row As DataGridViewRow In dgvMySales.Rows
            If CellAsText(row, dgvMySales.Columns("BookingStatus").Index) = BookingCancelled Then
                row.DefaultCellStyle.ForeColor = PastFore
            End If
        Next
    End Sub

    'the line under the grid. it adds the seats and the money up off the table that was just read
    'rather than asking the database again for something it has already been told
    Private Sub ShowSalesCount(dt As DataTable)
        If dt.Rows.Count = 0 Then
            lblSalesCount.Text = "You did not take any sales between those dates"
            Exit Sub
        End If

        Dim seats As Integer = 0
        Dim money As Double = 0
        Dim cancelled As Integer = 0
        Dim i As Integer

        For i = 0 To dt.Rows.Count - 1
            seats = seats + CInt(dt.Rows(i)("Seats"))

            'a cancelled sale was refunded so it is not money taken, same rule as everywhere else
            If dt.Rows(i)("BookingStatus").ToString() = BookingCancelled Then
                cancelled = cancelled + 1
            Else
                money = money + CDbl(dt.Rows(i)("TotalCost"))
            End If
        Next

        'one sale and one seat read wrong with an s on the end
        Dim text As String = dt.Rows.Count & " sales"
        If dt.Rows.Count = 1 Then
            text = "1 sale"
        End If

        If seats = 1 Then
            text = text & ", 1 seat"
        Else
            text = text & ", " & seats & " seats"
        End If

        If UserAccessLevel = 1 Then
            text = text & ", " & FormatCurrency(money)
        End If

        If cancelled > 0 Then
            text = text & " (" & cancelled & " cancelled, not counted)"
        End If

        lblSalesCount.Text = text
    End Sub

    'changing either date reloads the list
    Private Sub SalesDate_Changed(sender As Object, e As EventArgs) Handles dtpSalesFrom.ValueChanged, dtpSalesTo.ValueChanged
        If stillLoadingSales Then
            Exit Sub
        End If

        LoadMySales()
    End Sub

    'double clicking a sale opens the booking search on it, which is where a booking gets looked
    'at properly and where it would be cancelled. this screen deliberately cannot change anything
    Private Sub dgvMySales_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvMySales.CellDoubleClick
        If e.RowIndex < 0 Then
            Exit Sub
        End If

        Dim bookingID As Long = CLng(dgvMySales.Rows(e.RowIndex).Cells("BookingID").Value)

        frmBookingSearch.Show()
        'after Show on purpose, the form fills its grid as it loads
        frmBookingSearch.SelectBooking(bookingID)
    End Sub

    Private Sub btnSalesExport_Click(sender As Object, e As EventArgs) Handles btnSalesExport.Click
        If ExportGridToCsv(dgvMySales, "MySales.csv", "My Account") Then
            WriteLog("ACCOUNT", "Own sales exported, " & dgvMySales.Rows.Count & " sales")
            MessageBox.Show("Your sales have been exported.", "My Account", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    'MY SETTINGS TAB ---------------------------------------------------------------------------
    'what Filmtopia is remembering for this person. the settings table holds them under short
    'names meant for the code, so they are turned into something readable on the way out

    'turns the stored name into something a person would recognise. anything not listed comes
    'through as it is, so a setting added later still shows up rather than vanishing off the list
    Private Function FriendlySettingName(settingName As String) As String
        If settingName.ToUpper() = "THEME" Then
            Return "Colour scheme"
        ElseIf settingName.ToUpper() = "GENREFILTER" Then
            Return "Films screen, genre filter"
        ElseIf settingName.ToUpper() = "SCREENINGSSHOW" Then
            Return "Screenings screen, which showings"
        ElseIf settingName.ToUpper() = "SCREENINGSSCREEN" Then
            Return "Screenings screen, which screen"
        Else
            Return settingName
        End If
    End Function

    'same again for the value. only the theme is stored as a shouted word
    Private Function FriendlySettingValue(settingName As String, settingValue As String) As String
        If settingName.ToUpper() = "THEME" Then
            If settingValue.ToUpper() = "DARK" Then
                Return "Dark"
            Else
                Return "Light"
            End If
        End If

        Return settingValue
    End Function

    'reads this person's settings rows and puts them on the grid. the table is built by hand
    'rather than being bound straight to the query, because the names have to be swapped for
    'readable ones on the way past
    Private Sub LoadMySettings()
        Dim dt As New DataTable
        dt.Columns.Add("Setting")
        dt.Columns.Add("Value")

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT SettingName, SettingValue FROM tblUserSettings " &
                                 "WHERE LoginID = @LoginID ORDER BY SettingName"
            SQLCmd.Parameters.AddWithValue("@LoginID", CInt(CurrentLoginID))

            Dim rs As OleDbDataReader = SQLCmd.ExecuteReader()
            While rs.Read()
                Dim storedName As String = rs("SettingName").ToString()
                Dim storedValue As String = rs("SettingValue").ToString()
                dt.Rows.Add(FriendlySettingName(storedName), FriendlySettingValue(storedName, storedValue))
            End While
            rs.Close()
            cn.Close()
        End If

        dgvMySettings.DataSource = dt

        If dgvMySettings.Columns.Count > 0 Then
            dgvMySettings.Columns("Setting").Width = 300
            dgvMySettings.Columns("Value").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        End If

        If dt.Rows.Count = 0 Then
            lblSubResetHint.Text = "Nothing is being remembered for you yet, so there is nothing to put back."
            btnResetMySettings.Enabled = False
        Else
            lblSubResetHint.Text = "This only affects you. Nobody elses settings are touched."
            btnResetMySettings.Enabled = True
        End If

        dgvMySettings.ClearSelection()
    End Sub

    'throws this person's saved settings away and goes back to the defaults. it asks first, and it
    'opens on No like the rest of the yes/no boxes that destroy something
    Private Sub btnResetMySettings_Click(sender As Object, e As EventArgs) Handles btnResetMySettings.Click
        Dim answer As DialogResult = MessageBox.Show(
            "Put all of your settings back to default?" & vbNewLine & vbNewLine &
            "Your colour scheme and the filters the screens remember for you will be forgotten. " &
            "This only affects your account.",
            "My Settings", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

        If answer = DialogResult.No Then
            Exit Sub
        End If

        Dim howMany As Integer = 0

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "DELETE FROM tblUserSettings WHERE LoginID = @LoginID"
            SQLCmd.Parameters.AddWithValue("@LoginID", CInt(CurrentLoginID))
            howMany = SQLCmd.ExecuteNonQuery()
            cn.Close()
        End If

        'reading them back in is what puts the variables to their defaults, because there is
        'nothing left in the table to read. ClearUserSettings is not used here on purpose, that
        'one sets CurrentLoginID to 0, which is for logging out rather than for this
        LoadUserSettings(CurrentLoginID)
        ApplyThemeToAllForms()
        LoadMySettings()

        WriteLog("SETTINGS", "Own settings reset to default, " & howMany & " removed", LogChange)
        MessageBox.Show("Your settings have been put back to default.", "My Settings",
                        MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

End Class
