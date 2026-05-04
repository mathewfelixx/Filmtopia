Imports System.Data.OleDb

Public Class frmFilms
    Private Sub frmFilms_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadFilms()
    End Sub

    Private Sub LoadFilms()
        Dim SQLCmd As New OleDbCommand("SELECT FilmID, FilmTitle, FilmAgeRating, FilmDuration, FilmDescription " &
                                       "FROM tblFilm ORDER BY FilmTitle")
        Dim dt As New DataTable
        If FillTable(SQLCmd, dt) Then
            dgvFilms.DataSource = dt
            dgvFilms.Columns("FilmID").Visible = False
            dgvFilms.Columns("FilmTitle").HeaderText = "Title"
            dgvFilms.Columns("FilmAgeRating").HeaderText = "Age Rating"
            dgvFilms.Columns("FilmDuration").HeaderText = "Duration (mins)"
            dgvFilms.Columns("FilmDescription").HeaderText = "Description"
            dgvFilms.Columns("FilmDescription").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            dgvFilms.DefaultCellStyle.WrapMode = DataGridViewTriState.True
        End If
    End Sub
End Class
