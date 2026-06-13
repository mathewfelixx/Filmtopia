'this form is just for jumping straight to other forms while testing, not part of the real app
Public Class frmTesting
    Private Sub btnOpenLogin_Click(sender As Object, e As EventArgs) Handles btnOpenLogin.Click
        frmLogin.Show()
        Me.Hide()
    End Sub

    Private Sub btnMainStaff_Click(sender As Object, e As EventArgs) Handles btnMainStaff.Click
        frmLogin.globalusername = "Staff"
        UserAccessLevel = 2
        frmMainForm.Show()
        Me.Hide()
    End Sub

    Private Sub btnMainAdmin_Click(sender As Object, e As EventArgs) Handles btnMainAdmin.Click
        frmLogin.globalusername = "Admin"
        UserAccessLevel = 1
        frmMainForm.Show()
        Me.Hide()
    End Sub

    Private Sub btnOpenLogs_Click(sender As Object, e As EventArgs) Handles btnOpenLogs.Click
        frmLogs.Show()
    End Sub

    Private Sub btnOpenVersionControl_Click(sender As Object, e As EventArgs) Handles btnOpenVersionControl.Click
        frmVersionControlUTIL.Show()
    End Sub

    Private Sub frmTesting_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CommonFormStartup()
    End Sub
End Class
