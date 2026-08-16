Imports System.Data.OleDb

Public Class frmImportFilms

    Private Const MaxResults As Integer = 200

    Private filmsOnSystem As DataTable

    Private Sub frmImportFilms_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CommonFormStartup(Me)

        SetUpGrid()

        txtYearFrom.Text = CStr(Year(Date.Today) - 3)

        lblMatchCount.Text = "Nothing searched for yet"
        lblSearchInfo.Text = "The file is very big, so only films matching the search are read out of it"

        WriteLog("FILM", "Import films form opened")
    End Sub

    Private Sub SetUpGrid()
        dgvMatches.Columns.Clear()

        Dim colTick As New DataGridViewCheckBoxColumn
        colTick.Name = "colTick"
        colTick.HeaderText = "Import"
        colTick.Width = 55
        dgvMatches.Columns.Add(colTick)

        AddTextColumn("colTconst", "IMDb id", 90)
        dgvMatches.Columns("colTconst").Visible = False

        AddTextColumn("colTitle", "Title", 330)
        AddTextColumn("colYear", "Year", 60)
        AddTextColumn("colDuration", "Mins", 60)
        AddTextColumn("colGenres", "Genres", 220)

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

        dgvMatches.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

        dgvMatches.Columns("colTick").FillWeight = 5
        dgvMatches.Columns("colTitle").FillWeight = 30
        dgvMatches.Columns("colYear").FillWeight = 6
        dgvMatches.Columns("colDuration").FillWeight = 6
        dgvMatches.Columns("colGenres").FillWeight = 22
        dgvMatches.Columns("colRating").FillWeight = 11
        dgvMatches.Columns("colOnSystem").FillWeight = 20

        dgvMatches.Columns("colTick").MinimumWidth = 50
        dgvMatches.Columns("colYear").MinimumWidth = 55
        dgvMatches.Columns("colDuration").MinimumWidth = 55
        dgvMatches.Columns("colRating").MinimumWidth = 85

        dgvMatches.Columns("colYear").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        dgvMatches.Columns("colDuration").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
    End Sub

    Private Sub AddTextColumn(columnName As String, heading As String, width As Integer)
        Dim col As New DataGridViewTextBoxColumn
        col.Name = columnName
        col.HeaderText = heading
        col.Width = width
        col.ReadOnly = True
        dgvMatches.Columns.Add(col)
    End Sub

    Private Sub btnBrowse_Click(sender As Object, e As EventArgs) Handles btnBrowse.Click
        Dim openDialog As New OpenFileDialog
        openDialog.Filter = "IMDb data file (*.tsv)|*.tsv|All files (*.*)|*.*"
        openDialog.Title = "Choose the IMDb title.basics file"

        If openDialog.ShowDialog() = DialogResult.OK Then
            txtFilePath.Text = openDialog.FileName

            Dim info As New System.IO.FileInfo(openDialog.FileName)
            lblFileInfo.Text = "File is " & FileSizeAsText(info.Length)

            dgvMatches.Rows.Clear()
            lblMatchCount.Text = "Nothing searched for yet"
        End If
    End Sub

    Private Sub btnBrowseDesc_Click(sender As Object, e As EventArgs) Handles btnBrowseDesc.Click
        Dim openDialog As New OpenFileDialog
        openDialog.Filter = "Descriptions file (*.tsv)|*.tsv|All files (*.*)|*.*"
        openDialog.Title = "Choose the descriptions file"

        If openDialog.ShowDialog() = DialogResult.OK Then
            txtDescFilePath.Text = openDialog.FileName
        End If
    End Sub

    Private Function FileSizeAsText(bytes As Long) As String
        If bytes > 1073741824 Then
            Return Math.Round(bytes / 1073741824, 1) & " GB"
        End If

        If bytes > 1048576 Then
            Return Math.Round(bytes / 1048576, 1) & " MB"
        End If

        Return Math.Round(bytes / 1024, 1) & " KB"
    End Function

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

        btnSearch.Enabled = False
        btnBrowse.Enabled = False

        Dim reader As System.IO.StreamReader

        Try
            reader = New System.IO.StreamReader(txtFilePath.Text)
        Catch ex As Exception
            btnSearch.Enabled = True
            btnBrowse.Enabled = True
            Me.Cursor = Cursors.Default
            MessageBox.Show("That file could not be opened. If it is still downloading or is open in " &
                            "another program, wait for it to finish and try again." & vbCrLf & vbCrLf & ex.Message,
                            "Import Films", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End Try

        Dim line As String = reader.ReadLine()
        Dim linesRead As Long = 0
        Dim found As Integer = 0

        Try
            line = reader.ReadLine()

            Do While line IsNot Nothing AndAlso found < MaxResults
                linesRead = linesRead + 1

                Dim bits() As String = Split(line, vbTab)

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

                If linesRead Mod 100000 = 0 Then
                    lblSearchInfo.Text = "Searching... " & Format(linesRead, "#,##0") & " lines read, " & found & " found so far"
                    Application.DoEvents()
                End If

                line = reader.ReadLine()
            Loop

        Catch ex As Exception
            MessageBox.Show("Something went wrong part way through reading the file, so the list is only " &
                            "as far as it got." & vbCrLf & vbCrLf & ex.Message,
                            "Import Films", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Finally
            reader.Close()
            btnSearch.Enabled = True
            btnBrowse.Enabled = True
            Me.Cursor = Cursors.Default
        End Try


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

    Private Function YearIsOk(startYear As String, earliestYear As Integer) As Boolean
        If earliestYear = 0 Then
            Return True
        End If

        If startYear = "\N" Or Not IsNumeric(startYear) Then
            Return False
        End If

        Return CInt(startYear) >= earliestYear
    End Function

    Private Sub AddMatchToGrid(tconst As String, filmTitle As String, startYear As String, runtime As String, genres As String)
        Dim rowNumber As Integer = dgvMatches.Rows.Add()
        Dim row As DataGridViewRow = dgvMatches.Rows(rowNumber)

        row.Cells("colTick").Value = False
        row.Cells("colTconst").Value = tconst
        row.Cells("colTitle").Value = filmTitle
        row.Cells("colYear").Value = BlankIfMissing(startYear)
        row.Cells("colDuration").Value = BlankIfMissing(runtime)
        row.Cells("colGenres").Value = Replace(BlankIfMissing(genres), ",", ", ")

        If AlreadyOnSystem(filmTitle, startYear) Then
            row.Cells("colOnSystem").Value = "Already on system"
            row.DefaultCellStyle.BackColor = Color.Gainsboro
            row.DefaultCellStyle.ForeColor = Color.DimGray
            row.Cells("colTick").ReadOnly = True
            row.Cells("colRating").ReadOnly = True
        End If
    End Sub

    Private Function BlankIfMissing(value As String) As String
        If value = "\N" Then
            Return ""
        End If

        Return value
    End Function

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

    Private Sub btnImport_Click(sender As Object, e As EventArgs) Handles btnImport.Click
        dgvMatches.EndEdit()

        Dim ticked As Integer = 0

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

        Dim reader As System.IO.StreamReader

        Try
            reader = New System.IO.StreamReader(txtDescFilePath.Text)
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            MessageBox.Show("The descriptions file could not be opened, so the films will come in without " &
                            "descriptions." & vbCrLf & vbCrLf & ex.Message,
                            "Import Films", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End Try

        Dim line As String = reader.ReadLine()
        Dim linesRead As Long = 0
        Dim stillLooking As Integer = wantedIDs.Length

        Try
            line = reader.ReadLine()

            Do While line IsNot Nothing AndAlso stillLooking > 0
                linesRead = linesRead + 1

                Dim bits() As String = Split(line, vbTab)

                If bits.Length >= 2 Then
                    Dim atFilm As Integer
                    For atFilm = 0 To wantedIDs.Length - 1
                        If foundDescriptions(atFilm) = "" And bits(0) = wantedIDs(atFilm) Then
                            foundDescriptions(atFilm) = BlankIfMissing(bits(1).Trim())
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

        Catch ex As Exception
            MessageBox.Show("Something went wrong part way through the descriptions file, so some films may " &
                            "come in without one." & vbCrLf & vbCrLf & ex.Message,
                            "Import Films", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Finally
            reader.Close()
        End Try
        Me.Cursor = Cursors.Default

        lblSearchInfo.Text = "Descriptions looked up, " & Format(linesRead, "#,##0") & " lines read"
    End Sub

    Private Function IsTicked(row As DataGridViewRow) As Boolean
        If row.Cells("colTick").Value Is Nothing Then
            Return False
        End If

        Return CBool(row.Cells("colTick").Value)
    End Function

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
