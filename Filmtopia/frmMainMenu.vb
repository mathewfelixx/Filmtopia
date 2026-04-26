Public Class frmMainMenu
    Private Sub frmMainMenu_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lblWelcome.Text = GetGreeting() & ", " & CurrentUsername & "!"
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

    Private Sub btnLogout_Click(sender As Object, e As EventArgs) Handles btnLogout.Click
        Me.Close()
    End Sub

    Private Sub frmMainMenu_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        CurrentLoginID = 0
        CurrentUsername = ""
        UserAccessLevel = 0
        frmLogin.Show()
    End Sub
End Class
