Imports System.Data.OleDb

Public Class frmFilms

    'tracks the FilmID of the row currently selected in the grid, 0 means nothing selected
    Private selectedFilmID As Long = 0

    Private Sub frmFilms_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CommonFormStartup()
        LoadFilms()
        WriteLog("FILM", "Films form opened")
    End Sub

    'loads all films from tblFilm into the grid
    Private Sub LoadFilms()
        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT FilmID, FilmTitle, FilmAgeRating, FilmDuration, FilmDescription " &
                                 "FROM tblFilm"
            Dim da As New OleDbDataAdapter(SQLCmd)
            Dim dt As New DataTable
            da.Fill(dt)
            dgvFilms.DataSource = dt
            cn.Close()
        End If

        'let the description column stretch out and wrap so its all readable
        dgvFilms.Columns("FilmDescription").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        dgvFilms.DefaultCellStyle.WrapMode = DataGridViewTriState.True
        dgvFilms.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells

        WriteLog("FILM", "Film list loaded")
    End Sub

    'adds a new film using the values typed into the textboxes
    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        If txtTitle.Text = "" Then
            MessageBox.Show("Enter a film title")
            Exit Sub
        End If
        If txtAgeRating.Text = "" Then
            MessageBox.Show("Enter an age rating")
            Exit Sub
        End If
        If txtDuration.Text = "" Then
            MessageBox.Show("Enter a duration")
            Exit Sub
        End If
        If Val(txtDuration.Text) < 0 Then
            MessageBox.Show("Duration cannot be negative")
            Exit Sub
        End If

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "INSERT INTO tblFilm (FilmTitle, FilmAgeRating, FilmDuration, FilmDescription) " &
                                 "VALUES (@FilmTitle, @FilmAgeRating, @FilmDuration, @FilmDescription)"
            SQLCmd.Parameters.AddWithValue("@FilmTitle", txtTitle.Text)
            SQLCmd.Parameters.AddWithValue("@FilmAgeRating", txtAgeRating.Text)
            SQLCmd.Parameters.AddWithValue("@FilmDuration", Val(txtDuration.Text))
            SQLCmd.Parameters.AddWithValue("@FilmDescription", txtDescription.Text)
            SQLCmd.ExecuteNonQuery()
            cn.Close()
        End If

        WriteLog("FILM", "Film added: " & txtTitle.Text)
        LoadFilms()
        ClearFields()
    End Sub

    'updates the currently selected film with the values in the textboxes
    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        If selectedFilmID = 0 Then
            MessageBox.Show("Select a film in the grid first")
            Exit Sub
        End If
        If txtTitle.Text = "" Then
            MessageBox.Show("Enter a film title")
            Exit Sub
        End If
        If txtAgeRating.Text = "" Then
            MessageBox.Show("Enter an age rating")
            Exit Sub
        End If
        If txtDuration.Text = "" Then
            MessageBox.Show("Enter a duration")
            Exit Sub
        End If
        If Val(txtDuration.Text) < 0 Then
            MessageBox.Show("Duration cannot be negative")
            Exit Sub
        End If

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "UPDATE tblFilm " &
                                 "SET FilmTitle = @FilmTitle, FilmAgeRating = @FilmAgeRating, FilmDuration = @FilmDuration, FilmDescription = @FilmDescription " &
                                 "WHERE FilmID = @FilmID"
            SQLCmd.Parameters.AddWithValue("@FilmTitle", txtTitle.Text)
            SQLCmd.Parameters.AddWithValue("@FilmAgeRating", txtAgeRating.Text)
            SQLCmd.Parameters.AddWithValue("@FilmDuration", Val(txtDuration.Text))
            SQLCmd.Parameters.AddWithValue("@FilmDescription", txtDescription.Text)
            SQLCmd.Parameters.AddWithValue("@FilmID", CInt(selectedFilmID))
            SQLCmd.ExecuteNonQuery()
            cn.Close()
        End If

        WriteLog("FILM", "Film updated: " & txtTitle.Text)
        LoadFilms()
        ClearFields()
    End Sub

    'deletes the currently selected film
    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If selectedFilmID = 0 Then
            MessageBox.Show("Select a film in the grid first")
            Exit Sub
        End If

        If MessageBox.Show("Delete this film?", "Confirm", MessageBoxButtons.YesNo) = DialogResult.No Then
            Exit Sub
        End If

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "DELETE FROM tblFilm " &
                                 "WHERE FilmID = @FilmID"
            SQLCmd.Parameters.AddWithValue("@FilmID", CInt(selectedFilmID))
            SQLCmd.ExecuteNonQuery()
            cn.Close()
        End If

        WriteLog("FILM", "Film deleted: " & txtTitle.Text)
        LoadFilms()
        ClearFields()
    End Sub

    'clears the textboxes and the selection
    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        ClearFields()
        WriteLog("FILM", "Film fields cleared")
    End Sub

    Private Sub ClearFields()
        selectedFilmID = 0
        txtTitle.Text = ""
        txtAgeRating.Text = ""
        txtDuration.Text = ""
        txtDescription.Text = ""
        dgvFilms.ClearSelection()
    End Sub

    'when a row is clicked, load its values into the textboxes for editing
    Private Sub dgvFilms_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvFilms.CellClick
        If e.RowIndex < 0 Then Exit Sub

        Dim row As DataGridViewRow = dgvFilms.Rows(e.RowIndex)
        selectedFilmID = CLng(row.Cells("FilmID").Value)
        txtTitle.Text = row.Cells("FilmTitle").Value.ToString()
        txtAgeRating.Text = row.Cells("FilmAgeRating").Value.ToString()
        txtDuration.Text = row.Cells("FilmDuration").Value.ToString()
        txtDescription.Text = row.Cells("FilmDescription").Value.ToString()
        WriteLog("FILM", "Film selected: " & txtTitle.Text)
    End Sub

End Class
