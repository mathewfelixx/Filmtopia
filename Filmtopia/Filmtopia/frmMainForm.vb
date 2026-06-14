Public Class frmMainForm
    'turns all the nav buttons back to transparent so only the active one is highlighted
    Private Sub SetAllButtonsTransp()
        btnBookings.BackColor = Color.Transparent
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
        btnBookings.BackColor = Color.FromArgb(173, 20, 87)
        frmBookings.Show()
    End Sub

    Private Sub btnScreenings_Click(sender As Object, e As EventArgs) Handles btnScreenings.Click
        SetAllButtonsTransp()
        btnScreenings.BackColor = Color.FromArgb(173, 20, 87)
        frmScreenings.Show()
    End Sub

    Private Sub btnCustomers_Click(sender As Object, e As EventArgs) Handles btnCustomers.Click
        SetAllButtonsTransp()
        btnCustomers.BackColor = Color.FromArgb(173, 20, 87)
        frmCustomers.Show()
    End Sub

    Private Sub btnFilms_Click(sender As Object, e As EventArgs) Handles btnFilms.Click
        SetAllButtonsTransp()
        btnFilms.BackColor = Color.FromArgb(173, 20, 87)
        frmFilms.Show()
    End Sub

    Private Sub btnScreens_Click(sender As Object, e As EventArgs) Handles btnScreens.Click
        SetAllButtonsTransp()
        btnScreens.BackColor = Color.FromArgb(173, 20, 87)
        frmScreens.Show()
    End Sub

    Private Sub btnFood_Click(sender As Object, e As EventArgs) Handles btnFood.Click
        SetAllButtonsTransp()
        btnFood.BackColor = Color.FromArgb(173, 20, 87)
        frmFoodItems.Show()
    End Sub

    Private Sub btnReports_Click(sender As Object, e As EventArgs) Handles btnReports.Click
        SetAllButtonsTransp()
        btnReports.BackColor = Color.FromArgb(173, 20, 87)
        MessageBox.Show("Reports coming soon")
    End Sub

    Private Sub btnLogs_Click(sender As Object, e As EventArgs) Handles btnLogs.Click
        SetAllButtonsTransp()
        btnLogs.BackColor = Color.FromArgb(173, 20, 87)
        frmLogs.Show()
    End Sub

    Private Sub btnSettings_Click(sender As Object, e As EventArgs) Handles btnSettings.Click
        SetAllButtonsTransp()
        btnSettings.BackColor = Color.FromArgb(173, 20, 87)
        MessageBox.Show("Settings coming soon")
    End Sub

    Private Sub btnLogout_Click(sender As Object, e As EventArgs) Handles btnLogout.Click
        LogedIn = False
        UserAccessLevel = 99
        Me.Close()
        frmLogin.Show()
    End Sub

    'shows or hides the management buttons depending on who is logged in
    Private Sub ConfigureAccessLevel()
        GroupBox1.Text = "Filmtopia " & frmLogin.globalusername
        lblWelcome.Text = "Welcome, " & frmLogin.globalusername & "!"

        If UserAccessLevel = 1 Then
            btnFilms.Visible = True
            btnScreens.Visible = True
            btnFood.Visible = True
            btnReports.Visible = True
            btnLogs.Visible = True
        Else
            btnFilms.Visible = False
            btnScreens.Visible = False
            btnFood.Visible = False
            btnReports.Visible = False
            btnLogs.Visible = False
        End If
    End Sub

    Private Sub frmMainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetAllButtonsTransp()
        CommonFormStartup()
        ConfigureAccessLevel()
    End Sub
End Class
