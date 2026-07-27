Imports System.Data.OleDb

Public Class frmFilms

    'tracks the FilmID of the row currently selected in the grid, 0 means nothing selected
    Private selectedFilmID As Long = 0

    'true while the form is setting itself up, so filling the search box does not load the grid
    'before everything is ready
    Private stillLoading As Boolean = True

    Private Sub frmFilms_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CommonFormStartup(Me)

        'the ratings a film can be given. it is a drop down rather than a box to type in because
        'the rating has to be one of these, and typing it by hand meant 15 and 15A both turning up
        cboAgeRating.Items.Add("U")
        cboAgeRating.Items.Add("PG")
        cboAgeRating.Items.Add("12A")
        cboAgeRating.Items.Add("12")
        cboAgeRating.Items.Add("15")
        cboAgeRating.Items.Add("18")

        stillLoading = False

        LoadFilms()
        ClearFields()
        WriteLog("FILM", "Films form opened")
    End Sub

    'loads the films into the grid, only the ones matching the search box if anything is typed in it
    Private Sub LoadFilms()
        Dim dt As New DataTable

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn

            Dim baseQuery As String = "SELECT FilmID, FilmTitle, FilmAgeRating, FilmDuration, FilmDescription " &
                                      "FROM tblFilm"

            If txtSearch.Text.Trim() = "" Then
                SQLCmd.CommandText = baseQuery & " ORDER BY FilmTitle"
            Else
                'searching the description as well means half remembering what a film is about
                'is enough to find it
                SQLCmd.CommandText = baseQuery & " WHERE FilmTitle LIKE @Search OR FilmDescription LIKE @Search2 " &
                                     "ORDER BY FilmTitle"
                SQLCmd.Parameters.AddWithValue("@Search", "%" & txtSearch.Text.Trim() & "%")
                SQLCmd.Parameters.AddWithValue("@Search2", "%" & txtSearch.Text.Trim() & "%")
            End If

            Dim da As New OleDbDataAdapter(SQLCmd)
            da.Fill(dt)
            cn.Close()
        End If

        'the duration is stored as a number of minutes, which is right for working things out but
        'hard to read. an extra column is added holding it as hours and minutes, and the real one
        'is hidden. it has to be a separate column because the minutes column only holds numbers,
        'so putting 2h 15m into it would fall over
        dt.Columns.Add("RunsFor", GetType(String))
        For Each row As DataRow In dt.Rows
            If Not IsDBNull(row("FilmDuration")) Then
                row("RunsFor") = MinutesAsText(CInt(row("FilmDuration")))
            End If
        Next

        dgvFilms.DataSource = dt

        If dgvFilms.Columns.Contains("FilmID") Then
            dgvFilms.Columns("FilmID").HeaderText = "ID"
            dgvFilms.Columns("FilmTitle").HeaderText = "Title"
            dgvFilms.Columns("FilmAgeRating").HeaderText = "Rating"
            dgvFilms.Columns("RunsFor").HeaderText = "Runs for"
            dgvFilms.Columns("FilmDescription").HeaderText = "Description"

            'the minutes are still there to be read back when a row is clicked, just not shown
            dgvFilms.Columns("FilmDuration").Visible = False

            dgvFilms.Columns("FilmID").Width = 50
            dgvFilms.Columns("FilmTitle").Width = 260
            dgvFilms.Columns("FilmAgeRating").Width = 70
            dgvFilms.Columns("RunsFor").Width = 100
            dgvFilms.Columns("FilmDescription").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill

            dgvFilms.Columns("FilmAgeRating").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            dgvFilms.Columns("RunsFor").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            'let the description wrap so all of it can be read
            dgvFilms.Columns("FilmDescription").DefaultCellStyle.WrapMode = DataGridViewTriState.True
            dgvFilms.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
        End If

        ShowCount(dt.Rows.Count)
        dgvFilms.ClearSelection()

        WriteLog("FILM", "Film list loaded")
    End Sub

    'turns a number of minutes into something like 2h 15m
    Private Function MinutesAsText(minutes As Integer) As String
        Dim hours As Integer = minutes \ 60
        Dim left As Integer = minutes Mod 60

        If hours = 0 Then
            Return left & "m"
        End If

        Return hours & "h " & left & "m"
    End Function

    'says how many films are showing, and whether the search is hiding any
    Private Sub ShowCount(shown As Integer)
        If txtSearch.Text.Trim() = "" Then
            If shown = 1 Then
                lblGridCount.Text = "1 film"
            Else
                lblGridCount.Text = shown & " films"
            End If
        ElseIf shown = 0 Then
            lblGridCount.Text = "No films match '" & txtSearch.Text.Trim() & "'"
        Else
            lblGridCount.Text = shown & " film(s) matching '" & txtSearch.Text.Trim() & "'"
        End If
    End Sub

    'the list narrows as it is typed in, there is no need for a search button
    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        If stillLoading Then
            Exit Sub
        End If

        LoadFilms()
    End Sub

    'checks what has been typed in before it goes anywhere near the database. it is in one place
    'because adding and changing a film both need exactly the same checks doing
    Private Function DetailsAreOk() As Boolean
        If txtTitle.Text.Trim() = "" Then
            MessageBox.Show("Enter a film title")
            txtTitle.Focus()
            Return False
        End If

        If cboAgeRating.Text.Trim() = "" Then
            MessageBox.Show("Pick an age rating")
            cboAgeRating.Focus()
            Return False
        End If

        If txtDuration.Text.Trim() = "" Then
            MessageBox.Show("Enter a duration in minutes")
            txtDuration.Focus()
            Return False
        End If

        If Not IsNumeric(txtDuration.Text) Then
            MessageBox.Show("The duration has to be a number of minutes")
            txtDuration.Focus()
            Return False
        End If

        If Val(txtDuration.Text) <= 0 Then
            MessageBox.Show("The duration has to be more than zero")
            txtDuration.Focus()
            Return False
        End If

        'nothing runs for longer than about five hours, so a number bigger than that is somebody
        'typing the running time in seconds or slipping on the keyboard
        If Val(txtDuration.Text) > 300 Then
            MessageBox.Show("That duration looks too long, it should be in minutes")
            txtDuration.Focus()
            Return False
        End If

        'the year is allowed to be left empty, because the films that were on the system before
        'there was a year field do not have one. if something has been typed in though it still
        'has to make sense
        If txtYear.Text.Trim() <> "" Then
            If Not IsNumeric(txtYear.Text) Then
                MessageBox.Show("The year has to be a number, like 2021")
                txtYear.Focus()
                Return False
            End If

            'the first films were made in the 1880s, and a cinema might have next year's blockbuster
            'on the system early, but not one from twenty years time
            If Val(txtYear.Text) < 1888 Or Val(txtYear.Text) > Year(Date.Today) + 5 Then
                MessageBox.Show("That year does not look right, it should be between 1888 and " & (Year(Date.Today) + 5))
                txtYear.Focus()
                Return False
            End If
        End If

        Return True
    End Function

    'the year box as something the database will take. an empty box has to go in as a proper null
    'rather than a zero, otherwise every film that has not been given a year shows 0 in the grid
    Private Function YearForDatabase() As Object
        If txtYear.Text.Trim() = "" Then
            Return DBNull.Value
        End If

        Return CInt(Val(txtYear.Text))
    End Function

    'adds a new film using the values typed into the boxes
    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        If Not DetailsAreOk() Then
            Exit Sub
        End If

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "INSERT INTO tblFilm (FilmTitle, FilmYear, FilmAgeRating, FilmDuration, FilmGenres, FilmDescription) " &
                                 "VALUES (@FilmTitle, @FilmYear, @FilmAgeRating, @FilmDuration, @FilmGenres, @FilmDescription)"
            SQLCmd.Parameters.AddWithValue("@FilmTitle", txtTitle.Text.Trim())
            SQLCmd.Parameters.AddWithValue("@FilmYear", YearForDatabase())
            SQLCmd.Parameters.AddWithValue("@FilmAgeRating", cboAgeRating.Text.Trim())
            SQLCmd.Parameters.AddWithValue("@FilmDuration", Val(txtDuration.Text))
            SQLCmd.Parameters.AddWithValue("@FilmGenres", txtGenres.Text.Trim())
            SQLCmd.Parameters.AddWithValue("@FilmDescription", txtDescription.Text.Trim())
            SQLCmd.ExecuteNonQuery()
            cn.Close()
        End If

        WriteLog("FILM", "Film added: " & txtTitle.Text.Trim(), LogChange)
        LoadFilms()
        ClearFields()
    End Sub

    'saves the changes made to the film that is selected in the grid
    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        If selectedFilmID = 0 Then
            MessageBox.Show("Select a film in the grid first")
            Exit Sub
        End If

        If Not DetailsAreOk() Then
            Exit Sub
        End If

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "UPDATE tblFilm " &
                                 "SET FilmTitle = @FilmTitle, FilmYear = @FilmYear, FilmAgeRating = @FilmAgeRating, FilmDuration = @FilmDuration, FilmGenres = @FilmGenres, FilmDescription = @FilmDescription " &
                                 "WHERE FilmID = @FilmID"
            SQLCmd.Parameters.AddWithValue("@FilmTitle", txtTitle.Text.Trim())
            SQLCmd.Parameters.AddWithValue("@FilmYear", YearForDatabase())
            SQLCmd.Parameters.AddWithValue("@FilmAgeRating", cboAgeRating.Text.Trim())
            SQLCmd.Parameters.AddWithValue("@FilmDuration", Val(txtDuration.Text))
            SQLCmd.Parameters.AddWithValue("@FilmGenres", txtGenres.Text.Trim())
            SQLCmd.Parameters.AddWithValue("@FilmDescription", txtDescription.Text.Trim())
            SQLCmd.Parameters.AddWithValue("@FilmID", CInt(selectedFilmID))
            SQLCmd.ExecuteNonQuery()
            cn.Close()
        End If

        WriteLog("FILM", "Film updated: " & txtTitle.Text.Trim(), LogChange)
        LoadFilms()
        ClearFields()
    End Sub

    'deletes the film that is selected in the grid
    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If selectedFilmID = 0 Then
            MessageBox.Show("Select a film in the grid first")
            Exit Sub
        End If

        'a film that is on the schedule cannot just be removed, its screenings would be left
        'pointing at a film that is not there any more and the whats on list would break
        Dim screenings As Integer = ScreeningsForFilm(selectedFilmID)

        If screenings > 0 Then
            MessageBox.Show("'" & txtTitle.Text & "' has " & screenings & " screening(s) scheduled." & vbCrLf &
                            "Delete those screenings first, then the film can be removed.",
                            "Cannot delete", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If MessageBox.Show("Delete '" & txtTitle.Text & "'?", "Confirm", MessageBoxButtons.YesNo) = DialogResult.No Then
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

        WriteLog("FILM", "Film deleted: " & txtTitle.Text, LogChange)
        LoadFilms()
        ClearFields()
    End Sub

    'counts how many screenings a film has, used to stop it being deleted while it is scheduled
    Private Function ScreeningsForFilm(filmID As Long) As Integer
        Dim total As Integer = 0

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT COUNT(*) FROM tblScreening WHERE FilmID = @FilmID"
            SQLCmd.Parameters.AddWithValue("@FilmID", CInt(filmID))
            total = CInt(SQLCmd.ExecuteScalar())
            cn.Close()
        End If

        Return total
    End Function

    'clears the boxes and the selection
    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        ClearFields()
        WriteLog("FILM", "Film fields cleared")
    End Sub

    Private Sub ClearFields()
        selectedFilmID = 0
        txtTitle.Text = ""
        txtYear.Text = ""
        cboAgeRating.SelectedIndex = -1
        cboAgeRating.Text = ""
        txtDuration.Text = ""
        txtGenres.Text = ""
        txtDescription.Text = ""
        dgvFilms.ClearSelection()
        ShowWhatIsBeingEdited()
    End Sub

    'the heading over the boxes says whether a new film is being typed in or an existing one is
    'being changed, so it is never a guess as to what the buttons are about to do. save and delete
    'are switched off until something is picked, rather than letting them be pressed and then
    'telling the user off with a message box
    Private Sub ShowWhatIsBeingEdited()
        If selectedFilmID = 0 Then
            lblStatus.Text = "Adding a new film"
            btnUpdate.Enabled = False
            btnDelete.Enabled = False
            btnAdd.Enabled = True
        Else
            lblStatus.Text = "Editing: " & txtTitle.Text
            btnUpdate.Enabled = True
            btnDelete.Enabled = True
            btnAdd.Enabled = False
        End If
    End Sub

    'when a row is clicked, load its values into the boxes for editing
    Private Sub dgvFilms_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvFilms.CellClick
        If e.RowIndex < 0 Then Exit Sub

        Dim row As DataGridViewRow = dgvFilms.Rows(e.RowIndex)
        selectedFilmID = CLng(row.Cells("FilmID").Value)
        txtTitle.Text = row.Cells("FilmTitle").Value.ToString()
        txtYear.Text = row.Cells("FilmYear").Value.ToString()
        cboAgeRating.Text = row.Cells("FilmAgeRating").Value.ToString()
        txtGenres.Text = row.Cells("FilmGenres").Value.ToString()
        txtDescription.Text = row.Cells("FilmDescription").Value.ToString()

        'the box wants the plain number of minutes, which is the hidden column, not the 2h 15m
        'version that is on show
        txtDuration.Text = row.Cells("FilmDuration").Value.ToString()

        ShowWhatIsBeingEdited()
        WriteLog("FILM", "Film selected: " & txtTitle.Text)
    End Sub

End Class
