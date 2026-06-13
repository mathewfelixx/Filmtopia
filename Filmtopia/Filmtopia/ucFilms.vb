Public Class ucFilms
    Public Sub LoadView(view As UserControl)
        frmMainForm.pnlContent.Controls.Clear()
        view.Dock = DockStyle.Fill
        frmMainForm.pnlContent.Controls.Add(view)
    End Sub
    Private Sub ucFilms_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub btnAddFilm_Click(sender As Object, e As EventArgs) Handles btnAddFilm.Click
        LoadView(New ucAddFilms())
    End Sub
End Class
