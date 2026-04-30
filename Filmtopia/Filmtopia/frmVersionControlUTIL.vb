Public Class frmVersionControlUTIL
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        appversion = txtVersionControl.Text
        SaveVersion()
        UpdateAllVersionLabels()
    End Sub

    Private Sub frmVersionControlUTIL_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CommonFormStartup()
    End Sub
End Class