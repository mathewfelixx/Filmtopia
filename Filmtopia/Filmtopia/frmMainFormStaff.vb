Public Class frmMainFormStaff
    Private Sub LoadView(view As UserControl)
        pnlContent.Controls.Clear()
        view.Dock = DockStyle.Fill
        pnlContent.Controls.Add(view)
    End Sub

    Private Sub btnBookings_Click(sender As Object, e As EventArgs) Handles btnBookings.Click
        LoadView(New ucBookings())
    End Sub

    Private Sub btnScreenings_Click(sender As Object, e As EventArgs) Handles btnScreenings.Click
        LoadView(New ucScreenings())
    End Sub
End Class