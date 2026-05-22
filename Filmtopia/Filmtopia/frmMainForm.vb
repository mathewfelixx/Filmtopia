Public Class frmMainForm
    Public Sub LoadView(view As UserControl)
        pnlContent.Controls.Clear()
        view.Dock = DockStyle.Fill
        pnlContent.Controls.Add(view)
    End Sub
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
        LoadView(New ucBookings())
    End Sub

    Private Sub btnScreenings_Click(sender As Object, e As EventArgs) Handles btnScreenings.Click
        SetAllButtonsTransp()
        btnScreenings.BackColor = Color.FromArgb(173, 20, 87)
        LoadView(New ucScreenings())
    End Sub
    Private Sub btnLogout_Click(sender As Object, e As EventArgs) Handles btnLogout.Click
        LogedIn = False
        UserAccessLevel = 99
        Me.Close()
        frmLogin.Show()
    End Sub

    Private Sub btnCustomers_Click(sender As Object, e As EventArgs) Handles btnCustomers.Click
        SetAllButtonsTransp()
        btnCustomers.BackColor = Color.FromArgb(173, 20, 87)
    End Sub

    Private Sub btnFilms_Click(sender As Object, e As EventArgs) Handles btnFilms.Click
        SetAllButtonsTransp()
        btnFilms.BackColor = Color.FromArgb(173, 20, 87)
        LoadView(New ucFilms())
    End Sub

    Private Sub btnScreens_Click(sender As Object, e As EventArgs) Handles btnScreens.Click
        SetAllButtonsTransp()
        btnScreens.BackColor = Color.FromArgb(173, 20, 87)
    End Sub

    Private Sub btnFood_Click(sender As Object, e As EventArgs) Handles btnFood.Click
        SetAllButtonsTransp()
        btnFood.BackColor = Color.FromArgb(173, 20, 87)
    End Sub

    Private Sub btnLogs_Click(sender As Object, e As EventArgs) Handles btnLogs.Click
        SetAllButtonsTransp()
        btnLogs.BackColor = Color.FromArgb(173, 20, 87)
    End Sub

    Private Sub btnSettings_Click(sender As Object, e As EventArgs) Handles btnSettings.Click
        SetAllButtonsTransp()
        btnSettings.BackColor = Color.FromArgb(173, 20, 87)
    End Sub
    Private Sub frmMainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetAllButtonsTransp()
        CommonFormStartup()
        ConfigureAccessLevel()
    End Sub
    Private Sub ConfigureAccessLevel()

        If UserAccessLevel = 2 Then
            GroupBox1.Text = "Filmtopia " & frmLogin.globalusername
            btnFilms.Visible = True
            btnReports.Visible = True
        Else
            GroupBox1.Text = "Filmtopia " & frmLogin.globalusername
            btnFilms.Visible = False
            btnScreens.Visible = False
            btnFood.Visible = False
            btnLogs.Visible = False
            btnReports.Visible = False
        End If
    End Sub
    Private Sub btnReports_Click(sender As Object, e As EventArgs) Handles btnReports.Click
        SetAllButtonsTransp()
        MessageBox.Show("Reports coming soon")
    End Sub


End Class