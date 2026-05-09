Public Class frmTesting
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        frmVersionControlUTIL.Show()

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        frmLogin.Show()
        Me.Hide()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles btnMain.Click
        frmMainFormOLD.Show()
        Me.Hide()
    End Sub

    Private Sub Button3_Click_1(sender As Object, e As EventArgs) Handles Button3.Click
        frmSettings.Show()
        'Me.Hide()

    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        frmLogs.Show()
    End Sub
End Class