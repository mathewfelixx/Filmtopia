Public Class frmMainMenu
    Private Sub frmMainMenu_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lblWelcome.Text = GetGreeting() & ", " & CurrentUsername & "!"
        ConfigureAccessLevel()
    End Sub

    Private Sub ConfigureAccessLevel()
        Dim isManager As Boolean = False
        If UserAccessLevel = AccessManager Then
            isManager = True
        End If

        btnScreens.Visible = isManager
        btnFoodMenu.Visible = isManager
        btnLogs.Visible = isManager

        If isManager Then
            GroupBox1.Text = "Filmtopia Manager"
        Else
            GroupBox1.Text = "Filmtopia Staff"
        End If
    End Sub

    Private Function GetGreeting() As String
        Dim greeting As String
        If Date.Now.Hour < 12 Then
            greeting = "Good morning"
        ElseIf Date.Now.Hour < 18 Then
            greeting = "Good afternoon"
        Else
            greeting = "Good evening"
        End If
        Return greeting
    End Function

    Private Sub btnFilms_Click(sender As Object, e As EventArgs) Handles btnFilms.Click
        frmFilms.Show()
        frmFilms.BringToFront()
    End Sub

    Private Sub btnScreenings_Click(sender As Object, e As EventArgs) Handles btnScreenings.Click
        frmScreenings.Show()
        frmScreenings.BringToFront()
    End Sub

    Private Sub btnBookings_Click(sender As Object, e As EventArgs) Handles btnBookings.Click
        frmBookings.Show()
        frmBookings.BringToFront()
    End Sub

    Private Sub btnFindBooking_Click(sender As Object, e As EventArgs) Handles btnFindBooking.Click
        frmBookingSearch.Show()
        frmBookingSearch.BringToFront()
    End Sub

    Private Sub btnScreens_Click(sender As Object, e As EventArgs) Handles btnScreens.Click
        frmScreens.Show()
        frmScreens.BringToFront()
    End Sub

    Private Sub btnFoodMenu_Click(sender As Object, e As EventArgs) Handles btnFoodMenu.Click
        frmFoodItems.Show()
        frmFoodItems.BringToFront()
    End Sub

    Private Sub btnLogs_Click(sender As Object, e As EventArgs) Handles btnLogs.Click
        frmLogs.Show()
        frmLogs.BringToFront()
    End Sub

    Private Sub btnLogout_Click(sender As Object, e As EventArgs) Handles btnLogout.Click
        Me.Close()
    End Sub

    Private Sub frmMainMenu_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        WriteLog(LogAuth, CurrentUsername & " logged out")
        CurrentLoginID = 0
        CurrentUsername = ""
        UserAccessLevel = 0
        CloseOtherForms()
        frmLogin.Show()
    End Sub

    Private Sub CloseOtherForms()
        frmFilms.Close()
        frmScreenings.Close()
        frmBookings.Close()
        frmBookingSearch.Close()
        frmScreens.Close()
        frmFoodItems.Close()
        frmLogs.Close()
    End Sub
End Class
