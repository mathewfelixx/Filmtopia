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

    Private Function DetailsAreValid() As Boolean
        Dim valid As Boolean = False
        Dim duration As Integer

        If txtTitle.Text.Trim() = "" Then
            MessageBox.Show("Please enter the film's title.", "Missing Details", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtTitle.Focus()

        ElseIf cboAgeRating.SelectedIndex = -1 Then
            MessageBox.Show("Please choose an age rating.", "Missing Details", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            cboAgeRating.Focus()

        ElseIf Not Integer.TryParse(txtDuration.Text.Trim(), duration) Then
            MessageBox.Show("The duration must be a whole number of minutes.", "Invalid Duration", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtDuration.Focus()

        Else
            valid = True
        End If

        Return valid
    End Function

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        If DetailsAreValid() Then
            Dim SQLCmd As New OleDbCommand("INSERT INTO tblFilm (FilmTitle, FilmAgeRating, FilmDuration, FilmDescription) " &
                                           "VALUES (@FilmTitle, @FilmAgeRating, @FilmDuration, @FilmDescription)")
            SQLCmd.Parameters.AddWithValue("@FilmTitle", txtTitle.Text.Trim())
            SQLCmd.Parameters.AddWithValue("@FilmAgeRating", cboAgeRating.Text)
            SQLCmd.Parameters.AddWithValue("@FilmDuration", CInt(txtDuration.Text.Trim()))
            SQLCmd.Parameters.AddWithValue("@FilmDescription", txtDescription.Text.Trim())
            If RunCommand(SQLCmd) Then
                WriteLog(LogFilm, "Added film " & txtTitle.Text.Trim())
                LoadFilms()
                ClearFields()
            End If
        End If
    End Sub

    Private Sub ClearFields()
        txtTitle.Text = ""
        cboAgeRating.SelectedIndex = -1
        txtDuration.Text = ""
        txtDescription.Text = ""
        txtTitle.Focus()
    End Sub
End Class
