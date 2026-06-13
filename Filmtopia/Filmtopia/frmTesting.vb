Public Class frmTesting
    Private Sub Button1_Click(sender As Object, e As EventArgs) 
        frmVersionControlUTIL.Show()

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) 
        frmLogin.Show()
        Me.Hide()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) 
        frmMainFormOLD.Show()
        Me.Hide()
    End Sub

    Private Sub Button3_Click_1(sender As Object, e As EventArgs) 
        frmSettings.Show()
        'Me.Hide()

    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) 
        frmLogs.Show()
    End Sub

    Private Sub btnMainStaff_Click(sender As Object, e As EventArgs) 
        frmMainForm.Show()

    End Sub

    Private Sub frmTesting_Load(sender As Object, e As EventArgs)

    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) 
        frmLogin.globalusername = "Admin"
        UserAccessLevel = 2
        frmMainForm.Show()
    End Sub
End Class