Public Class frmTesting
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnVersionControl.Click
        frmVersionControlUTIL.Show()

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        frmLogin.Show()
        Me.Hide()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles btnMainOLD.Click
        frmMainFormOLD.Show()
        Me.Hide()
    End Sub

    Private Sub Button3_Click_1(sender As Object, e As EventArgs) Handles btnSettings.Click
        frmSettings.Show()
        'Me.Hide()

    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles btnLogs.Click
        frmLogs.Show()
    End Sub

    Private Sub btnMainStaff_Click(sender As Object, e As EventArgs) Handles btnMainStaff.Click
        frmMainForm.Show()

    End Sub

    Private Sub frmTesting_Load(sender As Object, e As EventArgs)

    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles btnAdminMode.Click
        frmLogin.globalusername = "Admin"
        UserAccessLevel = 2
        frmMainForm.Show()
    End Sub
End Class