Imports System.Data.OleDb

Public Class frmFilms
    Private selectedFilmID As Long = 0
    Private selectedFilmTitle As String = ""

    Private Sub frmFilms_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If UserAccessLevel = AccessManager Then
            btnDelete.Visible = True
        Else
            btnDelete.Visible = False
        End If
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

    Private Function DetailsAreValid(filmID As Long) As Boolean
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

        ElseIf TitleIsTaken(txtTitle.Text.Trim(), filmID) Then
            MessageBox.Show("There is already a film called " & txtTitle.Text.Trim() & ".", "Duplicate Film", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtTitle.Focus()

        Else
            valid = True
        End If

        Return valid
    End Function

    Private Function TitleIsTaken(title As String, filmID As Long) As Boolean
        Dim SQLCmd As New OleDbCommand("SELECT COUNT(*) FROM tblFilm WHERE FilmTitle = @FilmTitle AND FilmID <> @FilmID")
        SQLCmd.Parameters.AddWithValue("@FilmTitle", title)
        SQLCmd.Parameters.AddWithValue("@FilmID", filmID)
        Dim matches As Integer = GetValue(SQLCmd)
        Return matches > 0
    End Function

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        If DetailsAreValid(0) Then
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

    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        If selectedFilmID = 0 Then
            MessageBox.Show("Please pick a film from the list first.", "No Film Picked", MessageBoxButtons.OK, MessageBoxIcon.Information)
        ElseIf DetailsAreValid(selectedFilmID) Then
            Dim SQLCmd As New OleDbCommand("UPDATE tblFilm SET FilmTitle = @FilmTitle, FilmAgeRating = @FilmAgeRating, FilmDuration = @FilmDuration, FilmDescription = @FilmDescription " &
                                           "WHERE FilmID = @FilmID")
            SQLCmd.Parameters.AddWithValue("@FilmTitle", txtTitle.Text.Trim())
            SQLCmd.Parameters.AddWithValue("@FilmAgeRating", cboAgeRating.Text)
            SQLCmd.Parameters.AddWithValue("@FilmDuration", CInt(txtDuration.Text.Trim()))
            SQLCmd.Parameters.AddWithValue("@FilmDescription", txtDescription.Text.Trim())
            SQLCmd.Parameters.AddWithValue("@FilmID", selectedFilmID)
            If RunCommand(SQLCmd) Then
                WriteLog(LogFilm, "Updated film " & txtTitle.Text.Trim() & " (film number " & selectedFilmID & ")")
                LoadFilms()
                ClearFields()
            End If
        End If
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If selectedFilmID = 0 Then
            MessageBox.Show("Please pick a film from the list first.", "No Film Picked", MessageBoxButtons.OK, MessageBoxIcon.Information)
        ElseIf MessageBox.Show("Delete " & selectedFilmTitle & "? This cannot be undone.", "Delete Film", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) = DialogResult.Yes Then
            Dim SQLCmd As New OleDbCommand("DELETE FROM tblFilm WHERE FilmID = @FilmID")
            SQLCmd.Parameters.AddWithValue("@FilmID", selectedFilmID)
            If RunCommand(SQLCmd) Then
                WriteLog(LogFilm, "Deleted film " & selectedFilmTitle & " (film number " & selectedFilmID & ")")
                LoadFilms()
                ClearFields()
            End If
        End If
    End Sub

    Private Sub frmFilms_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        dgvFilms.ClearSelection()
    End Sub

    Private Sub dgvFilms_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs) Handles dgvFilms.DataBindingComplete
        dgvFilms.ClearSelection()
    End Sub

    Private Sub dgvFilms_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvFilms.CellClick
        If e.RowIndex >= 0 Then
            Dim row As DataGridViewRow = dgvFilms.Rows(e.RowIndex)
            selectedFilmID = row.Cells("FilmID").Value
            selectedFilmTitle = row.Cells("FilmTitle").Value
            txtTitle.Text = row.Cells("FilmTitle").Value
            cboAgeRating.SelectedItem = row.Cells("FilmAgeRating").Value
            txtDuration.Text = row.Cells("FilmDuration").Value
            txtDescription.Text = row.Cells("FilmDescription").Value.ToString()
        End If
    End Sub

    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        ClearFields()
    End Sub

    Private Sub ClearFields()
        selectedFilmID = 0
        selectedFilmTitle = ""
        dgvFilms.ClearSelection()
        txtTitle.Text = ""
        cboAgeRating.SelectedIndex = -1
        txtDuration.Text = ""
        txtDescription.Text = ""
        txtTitle.Focus()
    End Sub
End Class
