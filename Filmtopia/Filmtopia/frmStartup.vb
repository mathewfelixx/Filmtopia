Public Class frmStartup
    Private Sub frmStartup_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        timerStartup.Start()
        PictureBox1.Image = Image.FromFile("Anim1.gif")
        PictureBox1.SizeMode = PictureBoxSizeMode.StretchImage


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