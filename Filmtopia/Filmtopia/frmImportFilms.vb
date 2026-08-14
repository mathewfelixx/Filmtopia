Imports System.Data.OleDb

Public Class frmImportFilms

    'the most films the search will put in the grid. the IMDb file has millions of rows in it, so
    'without a cap a search for something common would fill the grid with thousands of results
    'that nobody is going to scroll through
    Private Const MaxResults As Integer = 200

    'the films that are already on the system, loaded once at the start of a search so each match
    'can be checked against it without going back to the database every time
    Private filmsOnSystem As DataTable

    Private Sub frmImportFilms_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CommonFormStartup(Me)

        SetUpGrid()

        'most of the time a cinema is looking for something recent, so the year box starts a few
        'years back rather than empty
        txtYearFrom.Text = CStr(Year(Date.Today) - 3)

        'the same window gets used again if the screen is opened a second time, so the messages from
        'last time are put back to how they started. the two file boxes are left alone on purpose,
        'so the files only have to be picked once in a session
        lblMatchCount.Text = "Nothing searched for yet"
        lblSearchInfo.Text = "The file is very big, so only films matching the search are read out of it"

        WriteLog("FILM", "Import films form opened")
    End Sub

    'builds the columns for the results grid. they are made here rather than in the designer because
    'two of them are not plain text, one is a tick box and one is a drop down
    Private Sub SetUpGrid()
        'this screen is opened with ShowDialog, and closing a window opened that way only hides it
        'instead of getting rid of it. so opening the screen a second time runs the form's Load
        'again on the same window, and without this the columns would be built on top of the ones
        'that were already there. eight would become sixteen, then twenty four, and the grid would
        'scroll further and further sideways every time the screen was opened
        dgvMatches.Columns.Clear()

        Dim colTick As New DataGridViewCheckBoxColumn
        colTick.Name = "colTick"
        colTick.HeaderText = "Import"
        colTick.Width = 55
        dgvMatches.Columns.Add(colTick)

        'IMDb's own id for the film, the tt number at the front of every row. it is not worth showing
        'but it is what the descriptions file is matched on, so it has to be carried along
        AddTextColumn("colTconst", "IMDb id", 90)
        dgvMatches.Columns("colTconst").Visible = False

        AddTextColumn("colTitle", "Title", 330)
        AddTextColumn("colYear", "Year", 60)
        AddTextColumn("colDuration", "Mins", 60)
        AddTextColumn("colGenres", "Genres", 220)

        'the age rating is not in the IMDb file at all, so it has to be picked here before a film
        'can be brought in
        Dim colRating As New DataGridViewComboBoxColumn
        colRating.Name = "colRating"
        colRating.HeaderText = "Age rating"
        colRating.Width = 90
        colRating.Items.Add("U")
        colRating.Items.Add("PG")
        colRating.Items.Add("12A")
        colRating.Items.Add("12")
        colRating.Items.Add("15")
        colRating.Items.Add("18")
        dgvMatches.Columns.Add(colRating)

        AddTextColumn("colOnSystem", "", 160)

        'the widths above are a starting point, but they are a plain number of pixels while the grid
        'itself gets bigger or smaller depending on the screen the program is run on. on a smaller
        'screen the columns would add up to more than the grid and it would scroll sideways. telling
        'the columns to share out whatever width there is instead means they always fit
        dgvMatches.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

        dgvMatches.Columns("colTick").FillWeight = 5
        dgvMatches.Columns("colTitle").FillWeight = 30
        dgvMatches.Columns("colYear").FillWeight = 6
        dgvMatches.Columns("colDuration").FillWeight = 6
        dgvMatches.Columns("colGenres").FillWeight = 22
        dgvMatches.Columns("colRating").FillWeight = 11
        dgvMatches.Columns("colOnSystem").FillWeight = 20

        'the narrow ones are not allowed to be squashed down to nothing
        dgvMatches.Columns("colTick").MinimumWidth = 50
        dgvMatches.Columns("colYear").MinimumWidth = 55
        dgvMatches.Columns("colDuration").MinimumWidth = 55
        dgvMatches.Columns("colRating").MinimumWidth = 85

        dgvMatches.Columns("colYear").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        dgvMatches.Columns("colDuration").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
    End Sub

    'adds one plain read only column to the results grid
    Private Sub AddTextColumn(columnName As String, heading As String, width As Integer)
        Dim col As New DataGridViewTextBoxColumn
        col.Name = columnName
        col.HeaderText = heading
        col.Width = width
        col.ReadOnly = True
        dgvMatches.Columns.Add(col)
    End Sub

    'lets the manager pick the IMDb file off their machine
    Private Sub btnBrowse_Click(sender As Object, e As EventArgs) Handles btnBrowse.Click
        Dim openDialog As New OpenFileDialog
        openDialog.Filter = "IMDb data file (*.tsv)|*.tsv|All files (*.*)|*.*"
        openDialog.Title = "Choose the IMDb title.basics file"

        If openDialog.ShowDialog() = DialogResult.OK Then
            txtFilePath.Text = openDialog.FileName

            'the real file is over a gigabyte, so saying how big it is makes it obvious why the
            'search takes a moment
            Dim info As New System.IO.FileInfo(openDialog.FileName)
            lblFileInfo.Text = "File is " & FileSizeAsText(info.Length)

            dgvMatches.Rows.Clear()
            lblMatchCount.Text = "Nothing searched for yet"
        End If
    End Sub

    'lets the manager pick the descriptions file. this one is optional, without it the films still
    'import, they just come in with nothing written about them
    Private Sub btnBrowseDesc_Click(sender As Object, e As EventArgs) Handles btnBrowseDesc.Click
        Dim openDialog As New OpenFileDialog
        openDialog.Filter = "Descriptions file (*.tsv)|*.tsv|All files (*.*)|*.*"
        openDialog.Title = "Choose the descriptions file"

        If openDialog.ShowDialog() = DialogResult.OK Then
            txtDescFilePath.Text = openDialog.FileName
        End If
    End Sub

    'turns a number of bytes into something readable like 1.4 GB
    Private Function FileSizeAsText(bytes As Long) As String
        If bytes > 1073741824 Then
            Return Math.Round(bytes / 1073741824, 1) & " GB"
        End If

        If bytes > 1048576 Then
            Return Math.Round(bytes / 1048576, 1) & " MB"
        End If

        Return Math.Round(bytes / 1024, 1) & " KB"
    End Function

    'reads the file looking for films that match what has been typed in. the file is far too big to
    'load in one go, so it is read a line at a time and only the matches are kept
    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        If txtFilePath.Text.Trim() = "" Then
            MessageBox.Show("Choose the IMDb data file first", "Import Films", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If Not System.IO.File.Exists(txtFilePath.Text) Then
            MessageBox.Show("That file is not there any more, choose it again", "Import Films", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If txtSearchTitle.Text.Trim() = "" Then
            MessageBox.Show("Type part of a film title to search for", "Import Films", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtSearchTitle.Focus()
            Exit Sub
        End If

        If txtYearFrom.Text.Trim() <> "" And Not IsNumeric(txtYearFrom.Text) Then
            MessageBox.Show("The year has to be a number, or leave it empty for every year", "Import Films", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtYearFrom.Focus()
            Exit Sub
        End If

        LoadFilmsAlreadyOnSystem()

        dgvMatches.Rows.Clear()
        Me.Cursor = Cursors.WaitCursor

        Dim wanted As String = LCase(txtSearchTitle.Text.Trim())
        Dim earliestYear As Integer = 0

        If txtYearFrom.Text.Trim() <> "" Then
            earliestYear = SafeInt(txtYearFrom.Text)
        End If

        Dim reader As New System.IO.StreamReader(txtFilePath.Text)
        Dim line As String = reader.ReadLine()   'the first line is the column headings, thrown away
        Dim linesRead As Long = 0
        Dim found As Integer = 0

        line = reader.ReadLine()

        Do While line IsNot Nothing And found < MaxResults
            linesRead = linesRead + 1

            Dim bits() As String = Split(line, vbTab)

            'a proper row has nine fields on it. anything shorter is a damaged line and is skipped
            'rather than being allowed to crash the loop
            If bits.Length >= 9 Then
                Dim tconst As String = bits(0)
                Dim titleType As String = bits(1)
                Dim filmTitle As String = bits(2)
                Dim isAdult As String = bits(4)
                Dim startYear As String = bits(5)
                Dim runtime As String = bits(7)
                Dim genres As String = bits(8)

                If titleType = "movie" And isAdult = "0" Then
                    If InStr(LCase(filmTitle), wanted) > 0 Then
                        If YearIsOk(startYear, earliestYear) Then
                            AddMatchToGrid(tconst, filmTitle, startYear, runtime, genres)
                            found = found + 1
                        End If
                    End If
                End If
            End If

            'the loop is holding the form up while it runs, so every so often it says how far it has
            'got and gives the form a chance to redraw itself. without this the window would just sit
            'there looking frozen on a file this size
            If linesRead Mod 100000 = 0 Then
                lblSearchInfo.Text = "Searching... " & Format(linesRead, "#,##0") & " lines read, " & found & " found so far"
                Application.DoEvents()
            End If

            line = reader.ReadLine()
        Loop

        reader.Close()
        Me.Cursor = Cursors.Default

        lblSearchInfo.Text = Format(linesRead, "#,##0") & " lines read from the file"

        If found = 0 Then
            lblMatchCount.Text = "No films found matching '" & txtSearchTitle.Text.Trim() & "'"
        ElseIf found = MaxResults Then
            lblMatchCount.Text = "Showing the first " & MaxResults & " films found, narrow the search down if the one you want is not here"
        Else
            lblMatchCount.Text = found & " film(s) found"
        End If

        WriteLog("FILM", "IMDb file searched for '" & txtSearchTitle.Text.Trim() & "', " & found & " found")
    End Sub

    'checks a year out of the file against the year box. IMDb puts \N where it does not know a
    'value, so that has to be checked for before trying to treat it as a number
    Private Function YearIsOk(startYear As String, earliestYear As Integer) As Boolean
        If earliestYear = 0 Then
            Return True
        End If

        If startYear = "\N" Or Not IsNumeric(startYear) Then
            Return False
        End If

        Return CInt(startYear) >= earliestYear
    End Function

    'puts one film from the file into the results grid
    Private Sub AddMatchToGrid(tconst As String, filmTitle As String, startYear As String, runtime As String, genres As String)
        Dim rowNumber As Integer = dgvMatches.Rows.Add()
        Dim row As DataGridViewRow = dgvMatches.Rows(rowNumber)

        row.Cells("colTick").Value = False
        row.Cells("colTconst").Value = tconst
        row.Cells("colTitle").Value = filmTitle
        row.Cells("colYear").Value = BlankIfMissing(startYear)
        row.Cells("colDuration").Value = BlankIfMissing(runtime)
        row.Cells("colGenres").Value = Replace(BlankIfMissing(genres), ",", ", ")

        'a film that is already on the system is greyed out and cannot be ticked, so the same film
        'does not get added to tblFilm twice
        If AlreadyOnSystem(filmTitle, startYear) Then
            row.Cells("colOnSystem").Value = "Already on system"
            row.DefaultCellStyle.BackColor = Color.Gainsboro
            row.DefaultCellStyle.ForeColor = Color.DimGray
            row.Cells("colTick").ReadOnly = True
            row.Cells("colRating").ReadOnly = True
        End If
    End Sub

    'IMDb writes \N when it does not have a value, which would look wrong on screen
    Private Function BlankIfMissing(value As String) As String
        If value = "\N" Then
            Return ""
        End If

        Return value
    End Function

    'reads the films that are already on the system into a table, so the search can mark the ones
    'that do not need importing again
    Private Sub LoadFilmsAlreadyOnSystem()
        filmsOnSystem = New DataTable

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT FilmTitle, FilmYear FROM tblFilm"
            Dim da As New OleDbDataAdapter(SQLCmd)
            da.Fill(filmsOnSystem)
            cn.Close()
        End If
    End Sub

    'true if a film with the same title and year is on the system already. the year is part of the
    'check because a remake shares its name with the original but is a different film
    Private Function AlreadyOnSystem(filmTitle As String, startYear As String) As Boolean
        If filmsOnSystem Is Nothing Then
            Return False
        End If

        For Each row As DataRow In filmsOnSystem.Rows
            If LCase(row("FilmTitle").ToString().Trim()) = LCase(filmTitle.Trim()) Then
                If row("FilmYear").ToString() = BlankIfMissing(startYear) Then
                    Return True
                End If
            End If
        Next

        Return False
    End Function

    'writes the ticked films into tblFilm
    Private Sub btnImport_Click(sender As Object, e As EventArgs) Handles btnImport.Click
        'a tick box or a drop down in a grid does not actually hold its new value until the cursor
        'moves off it, so ticking the last row and pressing import straight away would read the old
        'value. this makes the grid finish what it is doing first
        dgvMatches.EndEdit()

        Dim ticked As Integer = 0

        'nothing is written until every ticked row has been checked, so the import either all goes
        'in or none of it does, rather than stopping half way through
        For Each row As DataGridViewRow In dgvMatches.Rows
            If IsTicked(row) Then
                ticked = ticked + 1

                If row.Cells("colRating").Value Is Nothing Then
                    MessageBox.Show("'" & row.Cells("colTitle").Value.ToString() & "' needs an age rating before it can be imported." & vbCrLf &
                                    "The age rating is not in the IMDb file, so it has to be picked here.",
                                    "Age rating missing", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    dgvMatches.CurrentCell = row.Cells("colRating")
                    WriteLog("FILM", "Import stopped, no age rating picked for " & row.Cells("colTitle").Value.ToString(), LogWarning)
                    Exit Sub
                End If
            End If
        Next

        If ticked = 0 Then
            MessageBox.Show("Tick the films you want to import first", "Import Films", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        'the ids of the films being brought in, and the description found for each one. they are two
        'arrays lined up with each other, so wantedIDs(2) and foundDescriptions(2) are the same film
        Dim wantedIDs(ticked - 1) As String
        Dim foundDescriptions(ticked - 1) As String
        Dim atFilm As Integer = 0

        For Each row As DataGridViewRow In dgvMatches.Rows
            If IsTicked(row) Then
                wantedIDs(atFilm) = row.Cells("colTconst").Value.ToString()
                foundDescriptions(atFilm) = ""
                atFilm = atFilm + 1
            End If
        Next

        LookUpDescriptions(wantedIDs, foundDescriptions)

        Dim imported As Integer = 0
        Dim withDescription As Integer = 0
        atFilm = 0

        For Each row As DataGridViewRow In dgvMatches.Rows
            If IsTicked(row) Then
                ImportOneFilm(row, foundDescriptions(atFilm))

                If foundDescriptions(atFilm) <> "" Then
                    withDescription = withDescription + 1
                End If

                imported = imported + 1
                atFilm = atFilm + 1
            End If
        Next

        WriteLog("FILM", "Imported " & imported & " film(s) from IMDb file, " & withDescription & " with descriptions", LogChange)

        Dim message As String = imported & " film(s) imported."

        If txtDescFilePath.Text.Trim() = "" Then
            message = message & vbCrLf & "No descriptions file was chosen, so add what they are about on the films screen."
        ElseIf withDescription < imported Then
            message = message & vbCrLf & withDescription & " of them got a description. The rest were not in the descriptions file, so they need one writing on the films screen."
        Else
            message = message & vbCrLf & "All of them got a description."
        End If

        MessageBox.Show(message, "Import finished", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Me.Close()
    End Sub

    'reads the descriptions file once, picking out the ones belonging to the films being imported.
    'it is done this way round on purpose. the file has about a million lines in it, so going through
    'it once looking for all of them is a second or two, where looking it up film by film would mean
    'reading the whole thing again for every single one
    Private Sub LookUpDescriptions(wantedIDs() As String, foundDescriptions() As String)
        If txtDescFilePath.Text.Trim() = "" Then
            Exit Sub
        End If

        If Not System.IO.File.Exists(txtDescFilePath.Text) Then
            MessageBox.Show("The descriptions file is not there any more, so the films will come in without descriptions", "Import Films", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            WriteLog("FILM", "Descriptions file missing at import", LogWarning)
            Exit Sub
        End If

        Me.Cursor = Cursors.WaitCursor

        Dim reader As New System.IO.StreamReader(txtDescFilePath.Text)
        Dim line As String = reader.ReadLine()   'the headings line, thrown away
        Dim linesRead As Long = 0
        Dim stillLooking As Integer = wantedIDs.Length

        line = reader.ReadLine()

        Do While line IsNot Nothing And stillLooking > 0
            linesRead = linesRead + 1

            Dim bits() As String = Split(line, vbTab)

            If bits.Length >= 2 Then
                'check this line against every film still waiting for a description
                Dim atFilm As Integer
                For atFilm = 0 To wantedIDs.Length - 1
                    If foundDescriptions(atFilm) = "" And bits(0) = wantedIDs(atFilm) Then
                        foundDescriptions(atFilm) = bits(1)
                        stillLooking = stillLooking - 1
                    End If
                Next
            End If

            If linesRead Mod 100000 = 0 Then
                lblSearchInfo.Text = "Looking up descriptions... " & Format(linesRead, "#,##0") & " lines read"
                Application.DoEvents()
            End If

            line = reader.ReadLine()
        Loop

        reader.Close()
        Me.Cursor = Cursors.Default

        lblSearchInfo.Text = "Descriptions looked up, " & Format(linesRead, "#,##0") & " lines read"
    End Sub

    'true if the tick box on a row is ticked. an untouched tick box holds nothing rather than False,
    'so it has to be checked before being read as a boolean
    Private Function IsTicked(row As DataGridViewRow) As Boolean
        If row.Cells("colTick").Value Is Nothing Then
            Return False
        End If

        Return CBool(row.Cells("colTick").Value)
    End Function

    'writes one row of the grid into tblFilm
    Private Sub ImportOneFilm(row As DataGridViewRow, description As String)
        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "INSERT INTO tblFilm (FilmTitle, FilmYear, FilmAgeRating, FilmDuration, FilmGenres, FilmDescription) " &
                                 "VALUES (@FilmTitle, @FilmYear, @FilmAgeRating, @FilmDuration, @FilmGenres, @FilmDescription)"
            SQLCmd.Parameters.AddWithValue("@FilmTitle", row.Cells("colTitle").Value.ToString())
            SQLCmd.Parameters.AddWithValue("@FilmYear", NumberOrNull(row.Cells("colYear").Value.ToString()))
            SQLCmd.Parameters.AddWithValue("@FilmAgeRating", row.Cells("colRating").Value.ToString())
            SQLCmd.Parameters.AddWithValue("@FilmDuration", NumberOrNull(row.Cells("colDuration").Value.ToString()))
            SQLCmd.Parameters.AddWithValue("@FilmGenres", row.Cells("colGenres").Value.ToString())
            SQLCmd.Parameters.AddWithValue("@FilmDescription", description)
            SQLCmd.ExecuteNonQuery()
            cn.Close()
        End If
    End Sub

    'the year and the running time are not always in the file, and an empty one has to go into the
    'database as a proper null instead of a zero
    Private Function NumberOrNull(value As String) As Object
        If value.Trim() = "" Or Not IsNumeric(value) Then
            Return DBNull.Value
        End If

        Return CInt(value)
    End Function

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

End Class
