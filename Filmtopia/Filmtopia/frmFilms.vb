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

        'the genres to filter the list by. this is a set list rather than a SELECT DISTINCT because
        'a film keeps all its genres in the one field, so a distinct query would come back with
        'Action,Adventure,Drama as a single option instead of three separate ones
        cboGenreFilter.Items.Add("All genres")
        cboGenreFilter.Items.Add("Action")
        cboGenreFilter.Items.Add("Adventure")
        cboGenreFilter.Items.Add("Animation")
        cboGenreFilter.Items.Add("Comedy")
        cboGenreFilter.Items.Add("Crime")
        cboGenreFilter.Items.Add("Documentary")
        cboGenreFilter.Items.Add("Drama")
        cboGenreFilter.Items.Add("Family")
        cboGenreFilter.Items.Add("Fantasy")
        cboGenreFilter.Items.Add("Horror")
        cboGenreFilter.Items.Add("Mystery")
        cboGenreFilter.Items.Add("Romance")
        cboGenreFilter.Items.Add("Sci-Fi")
        cboGenreFilter.Items.Add("Thriller")
        cboGenreFilter.Items.Add("War")
        cboGenreFilter.Items.Add("Western")
        cboGenreFilter.SelectedIndex = 0

        'the shortest a row in the grid is allowed to be. the rows grow to fit whatever is written
        'about a film, and this stops one with hardly anything in it ending up as a thin line next
        'to a tall one. it is set here rather than in the designer because opening the form in the
        'designer wipes it, and then every row goes thin again with nothing to say why
        dgvFilms.RowTemplate.MinimumHeight = 44

        stillLoading = False

        'lets the form see escape before the box that has focus does
        Me.KeyPreview = True

        LoadFilms()
        ClearFields()

        'the first thing somebody usually wants is to find a film, so the cursor starts there
        txtSearch.Focus()
        WriteLog("FILM", "Films form opened")
    End Sub

    'saves the list as it is on screen, so whatever the search box and the genre filter have
    'narrowed it down to is what comes out
    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        If ExportGridToCsv(dgvFilms, "Films.csv", "Films") Then
            WriteLog("FILM", "Film list exported, " & dgvFilms.Rows.Count & " films")
        End If
    End Sub

    'escape empties the search box, or shuts the form if there is nothing to empty. doing both off
    'the one key means it never has to be explained, you press it until you are out
    Private Sub frmFilms_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        'f5 reloads the list, which is what most programs use that key for and what the main menu
        'already does. before this the only way to pick up somebody elses change was to close it
        If e.KeyCode = Keys.F5 Then
            LoadFilms()
        ElseIf e.KeyCode = Keys.Escape Then
            If txtSearch.Text <> "" Then
                txtSearch.Text = ""
            Else
                Me.Close()
            End If
        End If
    End Sub

    'loads the films into the grid, only the ones matching the search box if anything is typed in it
    Private Sub LoadFilms()
        Dim dt As New DataTable

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn

            Dim baseQuery As String = "SELECT FilmID, FilmTitle, FilmYear, FilmAgeRating, FilmDuration, FilmGenres, FilmDescription " &
                                      "FROM tblFilm"

            'there are two things that can narrow the list now, so the conditions are built up into
            'a string and only put on the end if there is anything in it. the values are added in
            'the same order as the conditions, because OleDb goes by the order the parameters were
            'added and not by their names
            Dim conditions As String = ""

            If txtSearch.Text.Trim() <> "" Then
                'searching the description as well means half remembering what a film is about
                'is enough to find it
                conditions = "(FilmTitle LIKE @Search OR FilmDescription LIKE @Search2)"
            End If

            If GenrePicked() <> "" Then
                If conditions <> "" Then
                    conditions = conditions & " AND "
                End If
                conditions = conditions & "FilmGenres LIKE @Genre"
            End If

            'films that came in from the IMDb file with nothing written about them. an empty box and
            'a box that was never filled in are not the same thing to Access, so both have to be
            'asked for
            If chkNeedsDescription.Checked Then
                If conditions <> "" Then
                    conditions = conditions & " AND "
                End If
                conditions = conditions & "(FilmDescription IS NULL OR FilmDescription = '')"
            End If

            If conditions = "" Then
                SQLCmd.CommandText = baseQuery & " ORDER BY FilmTitle"
            Else
                SQLCmd.CommandText = baseQuery & " WHERE " & conditions & " ORDER BY FilmTitle"
            End If

            If txtSearch.Text.Trim() <> "" Then
                SQLCmd.Parameters.AddWithValue("@Search", "%" & txtSearch.Text.Trim() & "%")
                SQLCmd.Parameters.AddWithValue("@Search2", "%" & txtSearch.Text.Trim() & "%")
            End If

            If GenrePicked() <> "" Then
                SQLCmd.Parameters.AddWithValue("@Genre", "%" & GenrePicked() & "%")
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
            dgvFilms.Columns("FilmYear").HeaderText = "Year"
            dgvFilms.Columns("FilmAgeRating").HeaderText = "Rating"
            dgvFilms.Columns("RunsFor").HeaderText = "Runs for"
            dgvFilms.Columns("FilmGenres").HeaderText = "Genres"
            dgvFilms.Columns("FilmDescription").HeaderText = "Description"

            'the minutes are still there to be read back when a row is clicked, just not shown
            dgvFilms.Columns("FilmDuration").Visible = False

            'the running time column is worked out after the others so it comes back off the end of
            'the table, which would put it on the right hand side of the grid after the description.
            'setting the order here puts the columns in the order they make sense to read in
            dgvFilms.Columns("FilmID").DisplayIndex = 0
            dgvFilms.Columns("FilmTitle").DisplayIndex = 1
            dgvFilms.Columns("FilmYear").DisplayIndex = 2
            dgvFilms.Columns("FilmAgeRating").DisplayIndex = 3
            dgvFilms.Columns("RunsFor").DisplayIndex = 4
            dgvFilms.Columns("FilmGenres").DisplayIndex = 5
            dgvFilms.Columns("FilmDescription").DisplayIndex = 6

            'the columns share out the width of the grid between them instead of each being set to a
            'number of pixels. setting them by hand meant the widths never added up to the width of
            'the grid, so there was either an empty strip down the right hand side or a scroll bar
            'along the bottom. the weights are out of a hundred, so the title gets a quarter of
            'whatever room there is and the year gets a fifteenth of it
            dgvFilms.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

            dgvFilms.Columns("FilmID").FillWeight = 4
            dgvFilms.Columns("FilmTitle").FillWeight = 24
            dgvFilms.Columns("FilmYear").FillWeight = 6
            dgvFilms.Columns("FilmAgeRating").FillWeight = 6
            dgvFilms.Columns("RunsFor").FillWeight = 8
            dgvFilms.Columns("FilmGenres").FillWeight = 16
            dgvFilms.Columns("FilmDescription").FillWeight = 36

            'the narrow columns are not allowed to be squashed smaller than their heading
            dgvFilms.Columns("FilmID").MinimumWidth = 40
            dgvFilms.Columns("FilmYear").MinimumWidth = 55
            dgvFilms.Columns("FilmAgeRating").MinimumWidth = 60
            dgvFilms.Columns("RunsFor").MinimumWidth = 75

            dgvFilms.Columns("FilmYear").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            dgvFilms.Columns("FilmAgeRating").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            dgvFilms.Columns("RunsFor").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            dgvFilms.Columns("FilmID").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            'the three columns that hold proper text are allowed to wrap onto another line, so a long
            'title or a long list of genres can still be read instead of being cut off with dots
            dgvFilms.Columns("FilmTitle").DefaultCellStyle.WrapMode = DataGridViewTriState.True
            dgvFilms.Columns("FilmGenres").DefaultCellStyle.WrapMode = DataGridViewTriState.True
            dgvFilms.Columns("FilmDescription").DefaultCellStyle.WrapMode = DataGridViewTriState.True

            'each row is then made tall enough to show everything that is in it. the rows have a
            'minimum height set in the designer as well, so a film with hardly anything written
            'about it still gets a decent sized row instead of a thin one
            dgvFilms.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells

            'a bit of breathing room around the text so it is not right up against the lines, and
            'the text sits in the middle of the row rather than stuck to the top of it
            dgvFilms.DefaultCellStyle.Padding = New Padding(6, 4, 6, 4)
            dgvFilms.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft

            'the header height is only allowed to be set once it has been told to stop working it
            'out for itself
            dgvFilms.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing
            dgvFilms.ColumnHeadersHeight = 32
        End If

        ShowCount(dt.Rows.Count)
        dgvFilms.ClearSelection()

        WriteLog("FILM", "Film list loaded")
    End Sub

    'the genre that has been picked to filter by, or an empty string if the list is not being
    'filtered. it is a function because both the query and the count label need to know
    Private Function GenrePicked() As String
        If cboGenreFilter.SelectedIndex <= 0 Then
            Return ""
        End If

        Return cboGenreFilter.Text
    End Function

    'the list narrows when a different genre is picked, same as typing in the search box does
    Private Sub cboGenreFilter_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboGenreFilter.SelectedIndexChanged
        If stillLoading Then
            Exit Sub
        End If

        LoadFilms()
    End Sub

    'shows just the films still waiting for somebody to write what they are about
    Private Sub chkNeedsDescription_CheckedChanged(sender As Object, e As EventArgs) Handles chkNeedsDescription.CheckedChanged
        If stillLoading Then
            Exit Sub
        End If

        LoadFilms()
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

    'says how many films are showing, and whether the search box or the genre box is hiding any
    Private Sub ShowCount(shown As Integer)
        'what the list is being narrowed by, written out so it can go on the end of the message
        Dim narrowedBy As String = ""

        If txtSearch.Text.Trim() <> "" Then
            narrowedBy = "matching '" & txtSearch.Text.Trim() & "'"
        End If

        If GenrePicked() <> "" Then
            If narrowedBy <> "" Then
                narrowedBy = narrowedBy & " and"
            End If
            narrowedBy = narrowedBy & " in " & GenrePicked()
        End If

        If chkNeedsDescription.Checked Then
            If narrowedBy <> "" Then
                narrowedBy = narrowedBy & " and"
            End If
            narrowedBy = narrowedBy & " still needing a description"
        End If

        If narrowedBy = "" Then
            If shown = 1 Then
                lblGridCount.Text = "1 film"
            Else
                lblGridCount.Text = shown & " films"
            End If
        ElseIf shown = 0 Then
            lblGridCount.Text = "No films " & narrowedBy.Trim()
        Else
            lblGridCount.Text = shown & " film(s) " & narrowedBy.Trim()
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
            MessageBox.Show("Enter a film title", "Films", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtTitle.Focus()
            Return False
        End If

        If cboAgeRating.Text.Trim() = "" Then
            MessageBox.Show("Pick an age rating", "Films", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            cboAgeRating.Focus()
            Return False
        End If

        If txtDuration.Text.Trim() = "" Then
            MessageBox.Show("Enter a duration in minutes", "Films", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtDuration.Focus()
            Return False
        End If

        If Not IsNumeric(txtDuration.Text) Then
            MessageBox.Show("The duration has to be a number of minutes", "Films", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtDuration.Focus()
            Return False
        End If

        If Val(txtDuration.Text) <= 0 Then
            MessageBox.Show("The duration has to be more than zero", "Films", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtDuration.Focus()
            Return False
        End If

        'nothing runs for longer than about five hours, so a number bigger than that is somebody
        'typing the running time in seconds or slipping on the keyboard
        If Val(txtDuration.Text) > 300 Then
            MessageBox.Show("That duration looks too long, it should be in minutes", "Films", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtDuration.Focus()
            Return False
        End If

        'the year is allowed to be left empty, because the films that were on the system before
        'there was a year field do not have one. if something has been typed in though it still
        'has to make sense
        If txtYear.Text.Trim() <> "" Then
            If Not IsNumeric(txtYear.Text) Then
                MessageBox.Show("The year has to be a number, like 2021", "Films", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtYear.Focus()
                Return False
            End If

            'the first films were made in the 1880s, and a cinema might have next year's blockbuster
            'on the system early, but not one from twenty years time
            If Val(txtYear.Text) < 1888 Or Val(txtYear.Text) > Year(Date.Today) + 5 Then
                MessageBox.Show("That year does not look right, it should be between 1888 and " & (Year(Date.Today) + 5),
                                "Films", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtYear.Focus()
                Return False
            End If
        End If

        If TitleAlreadyUsed() Then
            MessageBox.Show("'" & txtTitle.Text.Trim() & "' is already on the system for that year." & vbCrLf &
                            "Edit the one that is there rather than adding it twice.",
                            "Already on the system", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtTitle.Focus()
            Return False
        End If

        Return True
    End Function

    'says whether another film already has this title and year. the year is part of it because the
    'same title genuinely does come round again, a remake is not a duplicate. the selected film is
    'left out of the count so saving a film without changing its title does not trip over itself,
    'and when nothing is selected selectedFilmID is 0, which no real film has
    Private Function TitleAlreadyUsed() As Boolean
        Dim total As Integer = 0

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn

            'a film with no year has to be matched on the title on its own, because in SQL a null
            'is never equal to anything, not even another null
            If txtYear.Text.Trim() = "" Then
                SQLCmd.CommandText = "SELECT COUNT(*) FROM tblFilm " &
                                     "WHERE FilmTitle = @FilmTitle AND FilmYear IS NULL AND FilmID <> @FilmID"
            Else
                SQLCmd.CommandText = "SELECT COUNT(*) FROM tblFilm " &
                                     "WHERE FilmTitle = @FilmTitle AND FilmYear = @FilmYear AND FilmID <> @FilmID"
            End If

            SQLCmd.Parameters.AddWithValue("@FilmTitle", txtTitle.Text.Trim())

            If txtYear.Text.Trim() <> "" Then
                SQLCmd.Parameters.AddWithValue("@FilmYear", SafeInt(txtYear.Text))
            End If

            SQLCmd.Parameters.AddWithValue("@FilmID", CInt(selectedFilmID))
            total = CInt(SQLCmd.ExecuteScalar())
            cn.Close()
        End If

        Return total > 0
    End Function

    'the year box as something the database will take. an empty box has to go in as a proper null
    'rather than a zero, otherwise every film that has not been given a year shows 0 in the grid
    Private Function YearForDatabase() As Object
        If txtYear.Text.Trim() = "" Then
            Return DBNull.Value
        End If

        Return SafeInt(txtYear.Text)
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
            MessageBox.Show("Select a film in the grid first", "Films", MessageBoxButtons.OK, MessageBoxIcon.Warning)
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
            MessageBox.Show("Select a film in the grid first", "Films", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        'a film that is on the schedule cannot just be removed, its screenings would be left
        'pointing at a film that is not there any more and the whats on list would break
        Dim screenings As Integer = ScreeningsForFilm(selectedFilmID)

        If screenings > 0 Then
            MessageBox.Show("'" & txtTitle.Text & "' has " & screenings & " screening(s) scheduled." & vbCrLf &
                            "Delete those screenings first, then the film can be removed.",
                            "Cannot delete", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            'the refusals are worth recording as well as the deletions. a log that only ever says
            'what worked makes it look like nothing is ever attempted and stopped
            WriteLog("FILM", "Delete refused for '" & txtTitle.Text & "', it has " & screenings & " screening(s)", LogWarning)
            Exit Sub
        End If

        If MessageBox.Show("Delete '" & txtTitle.Text & "'?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
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

    'opens the screen that pulls film details out of an IMDb data file. the list is reloaded when
    'that screen closes so anything imported shows up straight away
    Private Sub btnImportFromFile_Click(sender As Object, e As EventArgs) Handles btnImportFromFile.Click
        If UserAccessLevel <> 1 Then
            MessageBox.Show("Only a manager can import films", "Films", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        frmImportFilms.ShowDialog()

        LoadFilms()
        ClearFields()
    End Sub

End Class
