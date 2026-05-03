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
        frmLogs.Close()
    End Sub
End Class
