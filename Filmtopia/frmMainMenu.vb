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
End Class
