Public Class frmStartup
    Private Sub frmStartup_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        timerStartup.Start()
        Dim animation As String = Application.StartupPath & "\Anim1.gif"

        If System.IO.File.Exists(animation) Then
            PictureBox1.Image = Image.FromFile(animation)
            PictureBox1.SizeMode = PictureBoxSizeMode.StretchImage
        End If


    End Sub

    Private Sub timerStartup_Tick(sender As Object, e As EventArgs) Handles timerStartup.Tick

        timerStartup.Stop()
        Me.Hide()
        frmLogin.Show()
    End Sub

    Private Sub frmStartup_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Me.KeyPress
        timerStartup.Stop()
        Me.Hide()
        frmLogin.Show()
    End Sub
End Class