Imports System.Data.OleDb

Public Class frmUserOverview

    Private Sub frmUserOverview_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CommonFormStartup(Me)
        Me.KeyPreview = True

        ShowWhoIsSignedIn()
        ConfigureAccessLevel()
        ShowAppearance()

        dtpActFrom.Value = Date.Today.AddDays(-7)
        dtpActTo.Value = Date.Today
        LoadActivityTypeFilter()
        LoadActivitySeverityFilter()
        stillLoadingActivity = False
        LoadActivity()

        dtpSalesFrom.Value = Date.Today.AddMonths(-1)
        dtpSalesTo.Value = Date.Today
        stillLoadingSales = False
        LoadMySales()

        LoadMySettings()

        WriteLog("ACCOUNT", "My account screen opened")
    End Sub

    Private Sub frmUserOverview_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Escape Then
            If txtActSearch.Text <> "" Then
                txtActSearch.Text = ""
            Else
                Me.Close()
            End If
        End If
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

    Private Sub ShowWhoIsSignedIn()
        lblWelcome.Text = GetGreeting() & ", " & frmLogin.globalusername

        lblSubSession.Text = "On since " & Format(SessionStarted, "HH:mm") & ", " &
                             HowLongAgo(SessionStarted)

        Dim lastTime As Date = LastSignedInBefore()

        If lastTime = Date.MinValue Then
            lblSubLastLogin.Text = "This is the first time this account has signed in"
        Else
            lblSubLastLogin.Text = "Last signed in " & Format(lastTime, "dd/MM/yyyy") & " at " &
                                   Format(lastTime, "HH:mm")
        End If

        ShowSecurityLine()
    End Sub

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
        ElseIf minutes < 1440 Then
            Return "about " & (minutes \ 60) & " hours"
        End If

        Dim days As Integer = minutes \ 1440

        If days = 1 Then
            Return "a day"
        ElseIf days < 14 Then
            Return days & " days"
        ElseIf days < 60 Then
            Return (days \ 7) & " weeks"
        ElseIf days < 365 Then
            Return (days \ 30) & " months"
        Else
            Return "over a year"
        End If
    End Function

    Private Function SignedInPattern() As String
        Return "User '" & frmLogin.globalusername & "' logged in successfully%"
    End Function

    Private Function FailedPattern() As String
        Return "User '" & frmLogin.globalusername & "' failed%"
    End Function

    Private Function ScalarOrZero(SQLCmd As OleDbCommand) As Double
        Dim answer As Object = SQLCmd.ExecuteScalar()

        If answer Is Nothing OrElse IsDBNull(answer) Then
            Return 0
        End If

        Return CDbl(answer)
    End Function

    Private Function Plural(howMany As Integer, oneWord As String, manyWord As String) As String
        If howMany = 1 Then
            Return "1 " & oneWord
        Else
            Return howMany & " " & manyWord
        End If
    End Function

    Private Function WhenItHappened(moment As Date) As String
        If moment.Date = Date.Today Then
            Return "at " & Format(moment, "HH:mm") & " today"
        ElseIf moment.Date = Date.Today.AddDays(-1) Then
            Return "at " & Format(moment, "HH:mm") & " yesterday"
        Else
            Return "on " & Format(moment, "dd/MM/yyyy") & " at " & Format(moment, "HH:mm")
        End If
    End Function

    Private Function SignInTimes(howMany As Integer) As Date()
        Dim found(howMany - 1) As Date
        Dim i As Integer

        For i = 0 To howMany - 1
            found(i) = Date.MinValue
        Next

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT TOP 10 LogDateTime FROM tblLogs " &
                                 "WHERE LogType = @Auth AND LogMessage LIKE @SignedIn " &
                                 "ORDER BY LogDateTime DESC, LogID DESC"
            SQLCmd.Parameters.AddWithValue("@Auth", "AUTH")
            SQLCmd.Parameters.AddWithValue("@SignedIn", SignedInPattern())

            Dim rs As OleDbDataReader = SQLCmd.ExecuteReader()
            Dim got As Integer = 0

            While rs.Read() And got < howMany
                found(got) = CDate(rs("LogDateTime"))
                got = got + 1
            End While

            rs.Close()
            cn.Close()
        End If

        Return found
    End Function

    Private Function LastSignedInBefore() As Date
        Dim times() As Date = SignInTimes(2)

        Return times(1)
    End Function

    Private Function FailedSince(since As Date) As Integer
        Dim answer As Integer = 0

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT COUNT(*) FROM tblLogs " &
                                 "WHERE LogType = @Auth AND LogMessage LIKE @Failed " &
                                 "AND LogDateTime > @Since"
            SQLCmd.Parameters.AddWithValue("@Auth", "AUTH")
            SQLCmd.Parameters.AddWithValue("@Failed", FailedPattern())
            SQLCmd.Parameters.AddWithValue("@Since", since)
            answer = CInt(ScalarOrZero(SQLCmd))
            cn.Close()
        End If

        Return answer
    End Function

    Private Function LastFailedSince(since As Date) As Date
        Dim answer As Date = Date.MinValue

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT TOP 1 LogDateTime FROM tblLogs " &
                                 "WHERE LogType = @Auth AND LogMessage LIKE @Failed " &
                                 "AND LogDateTime > @Since " &
                                 "ORDER BY LogDateTime DESC, LogID DESC"
            SQLCmd.Parameters.AddWithValue("@Auth", "AUTH")
            SQLCmd.Parameters.AddWithValue("@Failed", FailedPattern())
            SQLCmd.Parameters.AddWithValue("@Since", since)

            Dim rs As OleDbDataReader = SQLCmd.ExecuteReader()
            If rs.Read() Then
                answer = CDate(rs("LogDateTime"))
            End If
            rs.Close()
            cn.Close()
        End If

        Return answer
    End Function

    Private Sub ShowSecurityLine()
        Dim previous As Date = LastSignedInBefore()
        Dim failed As Integer = FailedSince(previous)

        If failed = 0 Then
            If previous = Date.MinValue Then
                lblSubSecurity.Text = "No failed attempts on your account."
            Else
                lblSubSecurity.Text = "No failed attempts since you last signed in."
            End If

            lblSubSecurity.ForeColor = SubtleFore
        Else
            Dim lastFail As Date = LastFailedSince(previous)
            Dim wording As String = Plural(failed, "failed attempt", "failed attempts")

            If previous = Date.MinValue Then
                lblSubSecurity.Text = wording & " on your account, the last " & WhenItHappened(lastFail)
            Else
                lblSubSecurity.Text = wording & " since you last signed in, the last " & WhenItHappened(lastFail)
            End If

            lblSubSecurity.ForeColor = HighlightBack
        End If

        pnlAccentDivider.BackColor = BorderCol
    End Sub

    Private Sub ConfigureAccessLevel()
        If UserAccessLevel = 1 Then
            lblRole.Text = "Manager, full access to everything"
        Else
            lblRole.Text = "Staff, the tills and the booking screens"
        End If
    End Sub

    Private Sub ShowAppearance()
        settingAppearance = True

        If DarkModeOn Then
            rdoDark.Checked = True
        Else
            rdoLight.Checked = True
        End If

        settingAppearance = False
    End Sub

    Private settingAppearance As Boolean = False

    Private Sub rdoLight_Click(sender As Object, e As EventArgs) Handles rdoLight.Click
        If settingAppearance Then
            Exit Sub
        End If

        LightMode()
        AfterThemeChanged()
    End Sub

    Private Sub rdoDark_Click(sender As Object, e As EventArgs) Handles rdoDark.Click
        If settingAppearance Then
            Exit Sub
        End If

        DarkMode()
        AfterThemeChanged()
    End Sub

    Private Sub AfterThemeChanged()
        ShowSecurityLine()
        LoadMySettings()
    End Sub

    Private Sub btnGoPassword_Click(sender As Object, e As EventArgs) Handles btnGoPassword.Click
        frmSettings.Show()
        frmSettings.BringToFront()
    End Sub

    Private Const MaxActivityRows As Integer = 500

    Private stillLoadingActivity As Boolean = True

    Private matchingActivity As Integer = 0

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

    Private Sub AddActivityParams(SQLCmd As OleDbCommand)
        SQLCmd.Parameters.AddWithValue("@Me", frmLogin.globalusername)
        SQLCmd.Parameters.AddWithValue("@FromDate", dtpActFrom.Value.Date)
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

    Private Sub LoadActivity()
        Dim dt As New DataTable
        matchingActivity = 0

        If DbConnect() Then
            Dim where As String = BuildActivityWhere()

            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT COUNT(*) FROM tblLogs " & where
            AddActivityParams(SQLCmd)
            matchingActivity = CInt(SQLCmd.ExecuteScalar())

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

    Private Sub ActivityFilter_Changed(sender As Object, e As EventArgs) Handles cboActType.SelectedIndexChanged,
        cboActSeverity.SelectedIndexChanged, dtpActFrom.ValueChanged, dtpActTo.ValueChanged

        If stillLoadingActivity Then
            Exit Sub
        End If

        LoadActivity()
    End Sub

    Private Sub txtActSearch_KeyDown(sender As Object, e As KeyEventArgs) Handles txtActSearch.KeyDown
        If e.KeyCode = Keys.Enter Then
            LoadActivity()
            e.SuppressKeyPress = True
        End If
    End Sub

    Private Sub btnActExport_Click(sender As Object, e As EventArgs) Handles btnActExport.Click
        If ExportGridToCsv(dgvActivity, "MyActivity.csv", "My Account") Then
            WriteLog("ACCOUNT", "Own activity exported, " & dgvActivity.Rows.Count & " entries")
            MessageBox.Show("Your activity has been exported.", "My Account", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub tabMe_SelectedIndexChanged(sender As Object, e As EventArgs) Handles tabMe.SelectedIndexChanged
        dgvActivity.ClearSelection()
        dgvMySales.ClearSelection()
        dgvMySettings.ClearSelection()
    End Sub

    Private stillLoadingSales As Boolean = True

    Private Sub LoadMySales()
        Dim dt As New DataTable

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn

            SQLCmd.CommandText = "SELECT tblBooking.BookingID, " &
                                 "CustomerForename & ' ' & CustomerSurname AS CustomerName, " &
                                 "IIf(IsNull(tblFilm.FilmTitle), 'Counter sale', tblFilm.FilmTitle) AS FilmTitle, ScreeningDate, ScreeningTime, " &
                                 "(SELECT COUNT(*) FROM tblBookingSeat AS bs WHERE bs.BookingID = tblBooking.BookingID) AS Seats, " &
                                 "BookingDate, TotalCost, BookingStatus " &
                                 "FROM ((tblBooking LEFT JOIN tblCustomer ON tblBooking.CustomerID = tblCustomer.CustomerID) " &
                                 "LEFT JOIN tblScreening ON tblBooking.ScreeningID = tblScreening.ScreeningID) " &
                                 "LEFT JOIN tblFilm ON tblScreening.FilmID = tblFilm.FilmID " &
                                 "WHERE tblBooking.LoginID = @Me AND BookingDate >= @From AND BookingDate < @To " &
                                 "ORDER BY tblBooking.BookingID DESC"
            SQLCmd.Parameters.AddWithValue("@Me", CInt(CurrentLoginID))
            SQLCmd.Parameters.AddWithValue("@From", dtpSalesFrom.Value.Date)
            SQLCmd.Parameters.AddWithValue("@To", dtpSalesTo.Value.Date.AddDays(1))

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

            dgvMySales.Columns("TotalCost").Visible = (UserAccessLevel = 1)
        End If

        GreyOutCancelled()
        ShowSalesCount(dt)
        dgvMySales.ClearSelection()
    End Sub

    Private Sub GreyOutCancelled()
        For Each row As DataGridViewRow In dgvMySales.Rows
            If CellAsText(row, dgvMySales.Columns("BookingStatus").Index) = BookingCancelled Then
                row.DefaultCellStyle.ForeColor = PastFore
            End If
        Next
    End Sub

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

            If dt.Rows(i)("BookingStatus").ToString() = BookingCancelled Then
                cancelled = cancelled + 1
            Else
                money = money + CDbl(dt.Rows(i)("TotalCost"))
            End If
        Next

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

    Private Sub SalesDate_Changed(sender As Object, e As EventArgs) Handles dtpSalesFrom.ValueChanged, dtpSalesTo.ValueChanged
        If stillLoadingSales Then
            Exit Sub
        End If

        LoadMySales()
    End Sub

    Private Sub dgvMySales_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvMySales.CellDoubleClick
        If e.RowIndex < 0 Then
            Exit Sub
        End If

        Dim bookingID As Long = CLng(dgvMySales.Rows(e.RowIndex).Cells("BookingID").Value)

        frmBookingSearch.Show()
        frmBookingSearch.SelectBooking(bookingID)
    End Sub

    Private Sub btnSalesExport_Click(sender As Object, e As EventArgs) Handles btnSalesExport.Click
        If ExportGridToCsv(dgvMySales, "MySales.csv", "My Account") Then
            WriteLog("ACCOUNT", "Own sales exported, " & dgvMySales.Rows.Count & " sales")
            MessageBox.Show("Your sales have been exported.", "My Account", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

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

        LoadUserSettings(CurrentLoginID)
        ApplyThemeToAllForms()
        ShowAppearance()
        LoadMySettings()
        ShowSecurityLine()

        WriteLog("SETTINGS", "Own settings reset to default, " & howMany & " removed", LogChange)
        MessageBox.Show("Your settings have been put back to default.", "My Settings",
                        MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

End Class
