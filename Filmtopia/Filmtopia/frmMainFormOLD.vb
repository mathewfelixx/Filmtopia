Public Class frmMainFormOLD
    Private Sub frmMainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CommonFormStartup()
        ConfigureAccessLevel()
    End Sub

    Private Sub ConfigureAccessLevel()
        If UserAccessLevel = 2 Then
            lblAccessLevel.Text = "Manager"
            btnFilms.Visible = True
            btnReports.Visible = True
        Else
            lblAccessLevel.Text = "Staff"
            btnFilms.Visible = False
            btnReports.Visible = False
        End If
    End Sub

    'Private Sub btnBookings_Click(sender As Object, e As EventArgs) Handles btnBookings.Click
    '    frmBooking.Show()
    'End Sub

    'Private Sub btnScreenings_Click(sender As Object, e As EventArgs) Handles btnScreenings.Click
    '    frmScreenings.Show()
    'End Sub

    Private Sub btnFilms_Click(sender As Object, e As EventArgs) Handles btnFilms.Click
        frmFilms.ShowDialog()
    End Sub

    Private Sub btnReports_Click(sender As Object, e As EventArgs) Handles btnReports.Click
        MessageBox.Show("Reports coming soon")
    End Sub

    Private Sub btnLogout_Click(sender As Object, e As EventArgs) Handles btnLogout.Click
        LogedIn = False
        UserAccessLevel = 99
        Me.Close()
        frmLogin.Show()
    End Sub

    Private Sub frmMainForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing

        e.Cancel = True
        LogedIn = False
        UserAccessLevel = 99
        Me.Hide()
        frmLogin.Show()

    End Sub


End Class