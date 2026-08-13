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

        WriteLog("ACCOUNT", "My account screen opened")
    End Sub

    'escape closes the screen, same as everywhere else
    Private Sub frmUserOverview_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.Close()
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

End Class
