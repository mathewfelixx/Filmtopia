Public Class frmMainForm
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

    Private Sub btnBookings_Click(sender As Object, e As EventArgs) Handles btnBookings.Click
        SetAllButtonsTransp()
        btnBookings.BackColor = HighlightBack
        frmBookings.Show()
    End Sub

    Private Sub btnFindBooking_Click(sender As Object, e As EventArgs) Handles btnFindBooking.Click
        SetAllButtonsTransp()
        btnFindBooking.BackColor = HighlightBack
        frmBookingSearch.Show()
    End Sub

    Private Sub btnScreenings_Click(sender As Object, e As EventArgs) Handles btnScreenings.Click
        SetAllButtonsTransp()
        btnScreenings.BackColor = HighlightBack
        frmScreenings.Show()
    End Sub

    Private Sub btnCustomers_Click(sender As Object, e As EventArgs) Handles btnCustomers.Click
        SetAllButtonsTransp()
        btnCustomers.BackColor = HighlightBack
        frmCustomers.Show()
    End Sub

    Private Sub btnFilms_Click(sender As Object, e As EventArgs) Handles btnFilms.Click
        SetAllButtonsTransp()
        btnFilms.BackColor = HighlightBack
        frmFilms.Show()
    End Sub

    Private Sub btnScreens_Click(sender As Object, e As EventArgs) Handles btnScreens.Click
        SetAllButtonsTransp()
        btnScreens.BackColor = HighlightBack
        frmScreens.Show()
    End Sub

    Private Sub btnFood_Click(sender As Object, e As EventArgs) Handles btnFood.Click
        SetAllButtonsTransp()
        btnFood.BackColor = HighlightBack
        frmFoodItems.Show()
    End Sub

    Private Sub btnReports_Click(sender As Object, e As EventArgs) Handles btnReports.Click
        SetAllButtonsTransp()
        btnReports.BackColor = HighlightBack
        frmSalesReport.Show()
    End Sub

    Private Sub btnLogs_Click(sender As Object, e As EventArgs) Handles btnLogs.Click
        SetAllButtonsTransp()
        btnLogs.BackColor = HighlightBack
        frmLogs.Show()
    End Sub

    Private Sub btnSettings_Click(sender As Object, e As EventArgs) Handles btnSettings.Click
        SetAllButtonsTransp()
        btnSettings.BackColor = HighlightBack
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

    Private Sub ConfigureAccessLevel()

        lblWelcome.Text = GetGreeting() & ", " & frmLogin.globalusername & "!"

        If UserAccessLevel = 1 Then
            GroupBox1.Text = "Filmtopia Admin"
            btnFilms.Visible = True
            btnScreens.Visible = True
            btnFood.Visible = True
            btnReports.Visible = True
            btnLogs.Visible = True
        Else
            GroupBox1.Text = "Filmtopia Staff"
            btnFilms.Visible = False
            btnScreens.Visible = False
            btnFood.Visible = False
            btnReports.Visible = False
            btnLogs.Visible = False
        End If
    End Sub

    Private Sub frmMainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetAllButtonsTransp()
        CommonFormStartup(Me)
        ConfigureAccessLevel()
    End Sub
End Class
