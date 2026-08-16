Imports System.Data.OleDb

Public Class frmFilms

    Private selectedFilmID As Long = 0

    Private stillLoading As Boolean = True

    Private boxesChanged As Boolean = False

    Private fillingBoxes As Boolean = False

    Private posterFileName As String = ""

    Private posterOriginalName As String = ""

    Private posterSourceFile As String = ""

    Private Sub frmFilms_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If UserAccessLevel <> 1 Then
            MessageBox.Show("Only a manager can open the films screen.", "Films", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            WriteLog("FILM", "Films screen refused, access level " & UserAccessLevel, LogSecurity)
            Me.Close()
            Exit Sub
        End If

        CommonFormStartup(Me)

        cboAgeRating.Items.Add("U")
        cboAgeRating.Items.Add("PG")
        cboAgeRating.Items.Add("12A")
        cboAgeRating.Items.Add("12")
        cboAgeRating.Items.Add("15")
        cboAgeRating.Items.Add("18")

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
        cboGenreFilter.SelectedIndex = cboGenreFilter.Items.IndexOf(LastGenreFilter)
        If cboGenreFilter.SelectedIndex = -1 Then
            cboGenreFilter.SelectedIndex = 0
        End If

        dgvFilms.RowTemplate.MinimumHeight = 44

        stillLoading = False

        Me.KeyPreview = True

        LoadFilms()
        ClearFields()

        txtSearch.Focus()
        WriteLog("FILM", "Films form opened")
    End Sub

    Private Sub frmFilms_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If picPoster.Image IsNot Nothing Then
            picPoster.Image.Dispose()
            picPoster.Image = Nothing
        End If

        If cboGenreFilter.Text = "" Then
            Exit Sub
        End If

        LastGenreFilter = cboGenreFilter.Text
        SaveUserSettings()
    End Sub

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        If ExportGridToCsv(dgvFilms, "Films.csv", "Films") Then
            WriteLog("FILM", "Film list exported, " & dgvFilms.Rows.Count & " films")
        End If
    End Sub

    Private Sub frmFilms_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
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

    Private Sub LoadFilms()
        Dim dt As New DataTable

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn

            Dim baseQuery As String = "SELECT FilmID, FilmTitle, FilmYear, FilmAgeRating, FilmDuration, FilmGenres, FilmDescription, FilmPoster " &
                                      "FROM tblFilm"

            Dim conditions As String = ""

            If txtSearch.Text.Trim() <> "" Then
                conditions = "(FilmTitle LIKE @Search OR FilmDescription LIKE @Search2)"
            End If

            If GenrePicked() <> "" Then
                If conditions <> "" Then
                    conditions = conditions & " AND "
                End If
                conditions = conditions & "FilmGenres LIKE @Genre"
            End If

            If chkNeedsDescription.Checked Then
                If conditions <> "" Then
                    conditions = conditions & " AND "
                End If
                conditions = conditions & "(FilmDescription IS NULL OR FilmDescription = '')"
            End If

            If conditions = "" Then
                SQLCmd.CommandText = baseQuery & " ORDER BY FilmTitle, FilmYear, FilmID"
            Else
                SQLCmd.CommandText = baseQuery & " WHERE " & conditions & " ORDER BY FilmTitle, FilmYear, FilmID"
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

            dgvFilms.Columns("FilmDuration").Visible = False

            dgvFilms.Columns("FilmPoster").Visible = False

            dgvFilms.Columns("FilmID").DisplayIndex = 0
            dgvFilms.Columns("FilmTitle").DisplayIndex = 1
            dgvFilms.Columns("FilmYear").DisplayIndex = 2
            dgvFilms.Columns("FilmAgeRating").DisplayIndex = 3
            dgvFilms.Columns("RunsFor").DisplayIndex = 4
            dgvFilms.Columns("FilmGenres").DisplayIndex = 5
            dgvFilms.Columns("FilmDescription").DisplayIndex = 6

            dgvFilms.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

            dgvFilms.Columns("FilmID").FillWeight = 4
            dgvFilms.Columns("FilmTitle").FillWeight = 24
            dgvFilms.Columns("FilmYear").FillWeight = 6
            dgvFilms.Columns("FilmAgeRating").FillWeight = 6
            dgvFilms.Columns("RunsFor").FillWeight = 8
            dgvFilms.Columns("FilmGenres").FillWeight = 16
            dgvFilms.Columns("FilmDescription").FillWeight = 36

            dgvFilms.Columns("FilmID").MinimumWidth = 40
            dgvFilms.Columns("FilmYear").MinimumWidth = 55
            dgvFilms.Columns("FilmAgeRating").MinimumWidth = 60
            dgvFilms.Columns("RunsFor").MinimumWidth = 75

            dgvFilms.Columns("FilmYear").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            dgvFilms.Columns("FilmAgeRating").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            dgvFilms.Columns("RunsFor").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            dgvFilms.Columns("FilmID").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            dgvFilms.Columns("FilmTitle").DefaultCellStyle.WrapMode = DataGridViewTriState.True
            dgvFilms.Columns("FilmGenres").DefaultCellStyle.WrapMode = DataGridViewTriState.True
            dgvFilms.Columns("FilmDescription").DefaultCellStyle.WrapMode = DataGridViewTriState.True

            dgvFilms.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells

            dgvFilms.DefaultCellStyle.Padding = New Padding(6, 4, 6, 4)
            dgvFilms.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft

            dgvFilms.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing
            dgvFilms.ColumnHeadersHeight = 32
        End If

        ShowCount(dt.Rows.Count)
        dgvFilms.ClearSelection()
    End Sub

    Private Function GenrePicked() As String
        If cboGenreFilter.SelectedIndex <= 0 Then
            Return ""
        End If

        Return cboGenreFilter.Text
    End Function

    Private Sub cboGenreFilter_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboGenreFilter.SelectedIndexChanged
        If stillLoading Then
            Exit Sub
        End If

        LoadFilms()
    End Sub

    Private Sub chkNeedsDescription_CheckedChanged(sender As Object, e As EventArgs) Handles chkNeedsDescription.CheckedChanged
        If stillLoading Then
            Exit Sub
        End If

        LoadFilms()
    End Sub

    Private Function MinutesAsText(minutes As Integer) As String
        Dim hours As Integer = minutes \ 60
        Dim left As Integer = minutes Mod 60

        If hours = 0 Then
            Return left & "m"
        End If

        Return hours & "h " & left & "m"
    End Function

    Private Sub ShowCount(shown As Integer)
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

    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        If stillLoading Then
            Exit Sub
        End If

        timerSearch.Stop()
        timerSearch.Start()
    End Sub

    Private Sub timerSearch_Tick(sender As Object, e As EventArgs) Handles timerSearch.Tick
        timerSearch.Stop()
        LoadFilms()
    End Sub

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

        If Val(txtDuration.Text) > 300 Then
            MessageBox.Show("That duration looks too long, it should be in minutes", "Films", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtDuration.Focus()
            Return False
        End If

        If txtYear.Text.Trim() <> "" Then
            If Not IsNumeric(txtYear.Text) Then
                MessageBox.Show("The year has to be a number, like 2021", "Films", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtYear.Focus()
                Return False
            End If

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

    Private Function TitleAlreadyUsed() As Boolean
        Dim total As Integer = 0

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn

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

    Private Function YearForDatabase() As Object
        If txtYear.Text.Trim() = "" Then
            Return DBNull.Value
        End If

        Return SafeInt(txtYear.Text)
    End Function

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        If Not DetailsAreOk() Then
            Exit Sub
        End If

        Dim saved As Boolean = False

        Dim newFilmID As Long = 0

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

            SQLCmd.CommandText = "SELECT @@IDENTITY"
            SQLCmd.Parameters.Clear()
            newFilmID = CLng(SQLCmd.ExecuteScalar())

            cn.Close()
            saved = True
        End If

        If Not saved Then
            Exit Sub
        End If

        If posterSourceFile <> "" AndAlso newFilmID > 0 Then
            Dim addedPoster As String = SavePosterFile(posterSourceFile, newFilmID)
            If addedPoster <> "" Then
                SavePosterName(newFilmID, addedPoster)
            End If
        End If

        Dim savedName As String = txtTitle.Text.Trim()
        WriteLog("FILM", "Film added: " & txtTitle.Text.Trim(), LogChange)
        LoadFilms()
        ClearFields()
        SayDone(lblSaved, "Added '" & savedName & "'")
    End Sub

    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        If selectedFilmID = 0 Then
            MessageBox.Show("Select a film in the grid first", "Films", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If Not DetailsAreOk() Then
            Exit Sub
        End If

        If posterSourceFile <> "" Then
            Dim newPoster As String = SavePosterFile(posterSourceFile, selectedFilmID)
            If newPoster <> "" Then
                posterFileName = newPoster
                posterSourceFile = ""
            End If
        End If

        Dim saved As Boolean = False

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "UPDATE tblFilm " &
                                 "SET FilmTitle = @FilmTitle, FilmYear = @FilmYear, FilmAgeRating = @FilmAgeRating, FilmDuration = @FilmDuration, FilmGenres = @FilmGenres, FilmDescription = @FilmDescription, FilmPoster = @FilmPoster " &
                                 "WHERE FilmID = @FilmID"
            SQLCmd.Parameters.AddWithValue("@FilmTitle", txtTitle.Text.Trim())
            SQLCmd.Parameters.AddWithValue("@FilmYear", YearForDatabase())
            SQLCmd.Parameters.AddWithValue("@FilmAgeRating", cboAgeRating.Text.Trim())
            SQLCmd.Parameters.AddWithValue("@FilmDuration", Val(txtDuration.Text))
            SQLCmd.Parameters.AddWithValue("@FilmGenres", txtGenres.Text.Trim())
            SQLCmd.Parameters.AddWithValue("@FilmDescription", txtDescription.Text.Trim())
            SQLCmd.Parameters.AddWithValue("@FilmPoster", posterFileName)
            SQLCmd.Parameters.AddWithValue("@FilmID", CInt(selectedFilmID))
            SQLCmd.ExecuteNonQuery()
            cn.Close()
            saved = True
        End If

        If Not saved Then
            Exit Sub
        End If

        If posterOriginalName <> "" AndAlso posterOriginalName <> posterFileName Then
            DeletePosterFile(posterOriginalName)
        End If

        Dim savedName As String = txtTitle.Text.Trim()
        WriteLog("FILM", "Film updated: " & txtTitle.Text.Trim(), LogChange)
        LoadFilms()
        ClearFields()
        SayDone(lblSaved, "Saved changes to '" & savedName & "'")
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If selectedFilmID = 0 Then
            MessageBox.Show("Select a film in the grid first", "Films", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Dim screenings As Integer = ScreeningsForFilm(selectedFilmID)

        If screenings > 0 Then
            MessageBox.Show("'" & txtTitle.Text & "' has " & screenings & " screening(s) scheduled." & vbCrLf &
                            "Delete those screenings first, then the film can be removed.",
                            "Cannot delete", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            WriteLog("FILM", "Delete refused for '" & txtTitle.Text & "', it has " & screenings & " screening(s)", LogWarning)
            Exit Sub
        End If

        If MessageBox.Show("Delete '" & txtTitle.Text & "'?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = DialogResult.No Then
            Exit Sub
        End If

        Dim saved As Boolean = False

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "DELETE FROM tblFilm " &
                                 "WHERE FilmID = @FilmID"
            SQLCmd.Parameters.AddWithValue("@FilmID", CInt(selectedFilmID))
            SQLCmd.ExecuteNonQuery()
            cn.Close()
            saved = True
        End If

        If Not saved Then
            Exit Sub
        End If

        DeletePosterFile(posterOriginalName)

        Dim savedName As String = txtTitle.Text.Trim()
        WriteLog("FILM", "Film deleted: " & txtTitle.Text, LogChange)
        LoadFilms()
        ClearFields()
        SayDone(lblSaved, "Deleted '" & savedName & "'")
    End Sub

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

    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        ClearFields()
    End Sub

    Private Sub Details_Changed(sender As Object, e As EventArgs) Handles txtTitle.TextChanged, txtYear.TextChanged,
        cboAgeRating.TextChanged, txtDuration.TextChanged, txtGenres.TextChanged, txtDescription.TextChanged
        If fillingBoxes Then
            Exit Sub
        End If

        boxesChanged = True
    End Sub

    Private Function ChangesCanBeLost() As Boolean
        If Not boxesChanged Then
            Return True
        End If

        Return MessageBox.Show("There are changes in the boxes that have not been saved." & vbCrLf &
                               "Throw them away?", "Unsaved changes",
                               MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                               MessageBoxDefaultButton.Button2) = DialogResult.Yes
    End Function

    Private Sub ClearFields()
        fillingBoxes = True

        lblSaved.Text = ""
        selectedFilmID = 0
        txtTitle.Text = ""
        txtYear.Text = ""
        cboAgeRating.SelectedIndex = -1
        cboAgeRating.Text = ""
        txtDuration.Text = ""
        txtGenres.Text = ""
        txtDescription.Text = ""

        posterFileName = ""
        posterOriginalName = ""
        posterSourceFile = ""
        ShowPoster()

        fillingBoxes = False
        boxesChanged = False

        dgvFilms.ClearSelection()
        ShowWhatIsBeingEdited()
    End Sub

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

    Private Sub dgvFilms_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvFilms.CellClick
        If e.RowIndex < 0 Then Exit Sub

        If Not ChangesCanBeLost() Then
            Exit Sub
        End If

        fillingBoxes = True

        Dim row As DataGridViewRow = dgvFilms.Rows(e.RowIndex)
        selectedFilmID = CLng(row.Cells("FilmID").Value)
        txtTitle.Text = row.Cells("FilmTitle").Value.ToString()
        txtYear.Text = row.Cells("FilmYear").Value.ToString()
        cboAgeRating.Text = row.Cells("FilmAgeRating").Value.ToString()
        txtGenres.Text = row.Cells("FilmGenres").Value.ToString()
        txtDescription.Text = row.Cells("FilmDescription").Value.ToString()

        txtDuration.Text = row.Cells("FilmDuration").Value.ToString()

        posterFileName = row.Cells("FilmPoster").Value.ToString()
        posterOriginalName = posterFileName
        posterSourceFile = ""
        ShowPoster()

        fillingBoxes = False
        boxesChanged = False

        ShowWhatIsBeingEdited()
    End Sub

    Private Sub ShowPoster()
        If picPoster.Image IsNot Nothing Then
            picPoster.Image.Dispose()
            picPoster.Image = Nothing
        End If

        If posterSourceFile <> "" Then
            picPoster.Image = PictureFromFile(posterSourceFile)
        Else
            picPoster.Image = PosterImage(posterFileName)
        End If

        lblNoPoster.Visible = (picPoster.Image Is Nothing)
    End Sub

    Private Sub btnChoosePoster_Click(sender As Object, e As EventArgs) Handles btnChoosePoster.Click
        Dim openDialog As New OpenFileDialog
        openDialog.Filter = "Pictures (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png"
        openDialog.Title = "Choose a poster"
        openDialog.RestoreDirectory = True

        If openDialog.ShowDialog() <> DialogResult.OK Then
            Exit Sub
        End If

        Dim check As Image = PictureFromFile(openDialog.FileName)
        If check Is Nothing Then
            MessageBox.Show("That file could not be read as a picture.", "Poster", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If
        check.Dispose()

        posterSourceFile = openDialog.FileName
        boxesChanged = True
        ShowPoster()
    End Sub

    Private Sub btnFetchPoster_Click(sender As Object, e As EventArgs) Handles btnFetchPoster.Click
        If txtTitle.Text.Trim() = "" Then
            MessageBox.Show("Type the film title in first, that is what TMDB is asked for.", "Fetch poster",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Me.Cursor = Cursors.WaitCursor
        Dim matchedAs As String = ""
        Dim fetched As String = FetchPosterFromTmdb(txtTitle.Text.Trim(), txtYear.Text.Trim(), matchedAs)
        Me.Cursor = Cursors.Default

        If fetched = "" Then
            Exit Sub
        End If

        posterSourceFile = fetched
        boxesChanged = True
        ShowPoster()

        SayDone(lblSaved, "Found " & matchedAs & " - check it is right, then save")
        WriteLog("FILM", "Poster fetched from TMDB for '" & txtTitle.Text.Trim() & "', matched as " & matchedAs)
    End Sub

    Private Sub btnRemovePoster_Click(sender As Object, e As EventArgs) Handles btnRemovePoster.Click
        If posterSourceFile = "" AndAlso posterFileName = "" Then
            Exit Sub
        End If

        posterSourceFile = ""
        posterFileName = ""
        boxesChanged = True
        ShowPoster()
    End Sub

    Private Sub SavePosterName(filmID As Long, fileName As String)
        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "UPDATE tblFilm SET FilmPoster = @FilmPoster WHERE FilmID = @FilmID"
            SQLCmd.Parameters.AddWithValue("@FilmPoster", fileName)
            SQLCmd.Parameters.AddWithValue("@FilmID", CInt(filmID))
            SQLCmd.ExecuteNonQuery()
            cn.Close()
        End If
    End Sub

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
