Imports System.Data.OleDb
Imports System.Text
Module modMain
    Private Const DatabasePath As String = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source='Filmtopia.mdb';
                                           Persist Security Info=false;"
    Public cn As OleDbConnection
    Public LogedIn As Boolean = False
    Public UserAccessLevel As Integer = 99
    Public CurrentLoginID As Long = 0
    Public SessionStarted As Date = Date.MinValue

    Public Const ScreenInService As String = "In service"
    Public Const ScreenOutOfService As String = "Out of service"

    Public Function ScreenIsInService(screenID As Long) As Boolean
        Dim status As String = ScreenInService

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT ScreenStatus FROM tblScreen WHERE ScreenID = @ScreenID"
            SQLCmd.Parameters.AddWithValue("@ScreenID", CInt(screenID))
            Dim answer As Object = SQLCmd.ExecuteScalar()

            If answer IsNot Nothing AndAlso Not IsDBNull(answer) Then
                status = answer.ToString()
            End If

            cn.Close()
        End If

        Return status <> ScreenOutOfService
    End Function

    Public Const ScreeningScheduled As String = "Scheduled"
    Public Const ScreeningCancelled As String = "Cancelled"

    Public Function ScreeningIsOn(screeningID As Long) As Boolean
        Dim status As String = ScreeningScheduled

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT ScreeningStatus FROM tblScreening WHERE ScreeningID = @ScreeningID"
            SQLCmd.Parameters.AddWithValue("@ScreeningID", CInt(screeningID))
            Dim answer As Object = SQLCmd.ExecuteScalar()

            If answer IsNot Nothing AndAlso Not IsDBNull(answer) Then
                status = answer.ToString()
            End If

            cn.Close()
        End If

        Return status <> ScreeningCancelled
    End Function

    Public Const FoodOnSale As String = "On sale"
    Public Const FoodWithdrawn As String = "Withdrawn"

    Public Function FoodItemIsOnSale(foodItemID As Long) As Boolean
        Dim status As String = FoodOnSale

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT FoodItemStatus FROM tblFoodItem WHERE FoodItemID = @FoodItemID"
            SQLCmd.Parameters.AddWithValue("@FoodItemID", CInt(foodItemID))
            Dim answer As Object = SQLCmd.ExecuteScalar()

            If answer IsNot Nothing AndAlso Not IsDBNull(answer) Then
                status = answer.ToString()
            End If

            cn.Close()
        End If

        Return status <> FoodWithdrawn
    End Function

    Public Function DbConnect() As Boolean
        Try
            cn = New OleDbConnection(DatabasePath)
            cn.Open()
            Return True
        Catch ex As Exception
            MessageBox.Show("Unable to open the database. " & ex.Message, "Database", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function
    Public Function DbConnectQuiet() As Boolean
        Try
            cn = New OleDbConnection(DatabasePath)
            cn.Open()
            cn.Close()
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function Encrypt(PlainText As String) As String

        Dim CipherText As String = PlainText

        If PlainText.Length = 0 Then
            MessageBox.Show("There is nothing to encrypt", "Encryption", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Else
            Dim dat As Byte() = New Byte(PlainText.Length - 1) {}
            dat = System.Text.Encoding.UTF8.GetBytes(PlainText)
            CipherText = Convert.ToBase64String(dat)
        End If
        Return CipherText
    End Function

    Public Function Decrypt(CipherText As String) As String
        Dim PlainText As String = CipherText

        If CipherText.Length = 0 Then
            MessageBox.Show("There is nothing to decrypt", "Encryption", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Else

            Try
                Dim strDecrypt As New System.Text.UTF8Encoding
                Dim UTF_Decrypt As System.Text.Decoder = strDecrypt.GetDecoder
                Dim uData As Byte() = Convert.FromBase64String(CipherText)
                Dim CharNum As Integer = UTF_Decrypt.GetCharCount(uData, 0, uData.Length)
                Dim Decrypt_Char As Char() = New Char(CharNum - 1) {}
                UTF_Decrypt.GetChars(uData, 0, uData.Length, Decrypt_Char, 0)
                PlainText = New String(Decrypt_Char)
            Catch ex As Exception
                PlainText = CipherText
            End Try
        End If

        Return PlainText
    End Function

    Public Sub CommonFormStartup(frm As Form)
        LoadVersion()
        UpdateAllVersionLabels()
        ColourScheme(frm)

    End Sub

    Public Function CsvField(value As String) As String
        Return """" & value.Replace("""", """""") & """"
    End Function

    Public Sub ClearPanel(pnl As Panel)
        For i As Integer = pnl.Controls.Count - 1 To 0 Step -1
            Dim ctrl As Control = pnl.Controls(i)
            pnl.Controls.RemoveAt(i)
            ctrl.Dispose()
        Next
    End Sub

    Public Sub SayDone(lbl As Label, message As String)
        lbl.Text = message
        lbl.ForeColor = AccentFore
    End Sub

    Public Function SafeInt(text As String) As Integer
        Dim number As Double = Val(text)

        If number > Integer.MaxValue Or number < Integer.MinValue Then
            Return 0
        End If

        Return CInt(number)
    End Function

    Public Function CellAsText(row As DataGridViewRow, columnIndex As Integer) As String
        If row.Cells(columnIndex).Value Is Nothing Then
            Return ""
        End If

        Return row.Cells(columnIndex).Value.ToString()
    End Function

    Public Function ExportGridToCsv(dgv As DataGridView, defaultFileName As String, title As String) As Boolean
        If dgv.Rows.Count = 0 Then
            MessageBox.Show("There is nothing on screen to export", title, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If

        Dim saveDialog As New SaveFileDialog
        saveDialog.Filter = "CSV files (*.csv)|*.csv"
        saveDialog.FileName = defaultFileName
        saveDialog.RestoreDirectory = True

        If saveDialog.ShowDialog() <> DialogResult.OK Then
            Return False
        End If

        Dim writer As System.IO.StreamWriter

        Try
            writer = New System.IO.StreamWriter(saveDialog.FileName)
        Catch ex As Exception
            MessageBox.Show("That file could not be written to. If it is open in another program, close it and try again." & vbCrLf & vbCrLf & ex.Message,
                            title, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try

        Dim line As String = ""

        For Each col As DataGridViewColumn In dgv.Columns
            If col.Visible AndAlso Not TypeOf col Is DataGridViewImageColumn Then
                If line <> "" Then
                    line = line & ","
                End If
                line = line & CsvField(col.HeaderText)
            End If
        Next
        writer.WriteLine(line)

        For Each row As DataGridViewRow In dgv.Rows
            line = ""
            For Each col As DataGridViewColumn In dgv.Columns
                If col.Visible AndAlso Not TypeOf col Is DataGridViewImageColumn Then
                    If line <> "" Then
                        line = line & ","
                    End If
                    line = line & CsvField(CellAsText(row, col.Index))
                End If
            Next
            writer.WriteLine(line)
        Next

        writer.Close()
        Return True
    End Function

    Public Function PictureFolder(folderName As String) As String
        Dim folder As String = Application.StartupPath & "\" & folderName

        If Not System.IO.Directory.Exists(folder) Then
            Try
                System.IO.Directory.CreateDirectory(folder)
            Catch ex As Exception
            End Try
        End If

        Return folder
    End Function

    Public Function PosterFolder() As String
        Return PictureFolder("Posters")
    End Function

    Public Function FoodPictureFolder() As String
        Return PictureFolder("FoodPictures")
    End Function

    Public Function PictureFromFile(fullPath As String) As Image
        If Not System.IO.File.Exists(fullPath) Then
            Return Nothing
        End If

        Try
            Dim stream As New System.IO.MemoryStream(System.IO.File.ReadAllBytes(fullPath))
            Dim loaded As Image = Image.FromStream(stream)
            Dim copy As Image = New Bitmap(loaded)
            loaded.Dispose()
            stream.Close()
            Return copy
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Function PictureByName(folderName As String, fileName As String) As Image
        If fileName Is Nothing OrElse fileName.Trim() = "" Then
            Return Nothing
        End If

        Return PictureFromFile(PictureFolder(folderName) & "\" & fileName)
    End Function

    Public Function PosterImage(fileName As String) As Image
        Return PictureByName("Posters", fileName)
    End Function

    Public Function FoodImage(fileName As String) As Image
        Return PictureByName("FoodPictures", fileName)
    End Function

    Public Function SmallPicture(folderName As String, fileName As String, width As Integer, height As Integer) As Image
        Dim big As Image = PictureByName(folderName, fileName)

        If big Is Nothing Then
            Return Nothing
        End If

        Dim small As New Bitmap(width, height)
        Dim g As Graphics = Graphics.FromImage(small)

        g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic

        Dim scale As Double = Math.Min(width / big.Width, height / big.Height)
        Dim drawWidth As Integer = CInt(big.Width * scale)
        Dim drawHeight As Integer = CInt(big.Height * scale)

        g.DrawImage(big, (width - drawWidth) \ 2, (height - drawHeight) \ 2, drawWidth, drawHeight)

        g.Dispose()
        big.Dispose()

        Return small
    End Function

    Public Function SavePictureFile(sourceFile As String, folderName As String, id As Long, title As String) As String
        If Not System.IO.File.Exists(sourceFile) Then
            MessageBox.Show("That picture could not be found.", title, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return ""
        End If

        Dim newName As String = id & System.IO.Path.GetExtension(sourceFile).ToLower()

        Try
            System.IO.File.Copy(sourceFile, PictureFolder(folderName) & "\" & newName, True)
            Return newName
        Catch ex As Exception
            MessageBox.Show("Could not save the picture. " & ex.Message, title, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return ""
        End Try
    End Function

    Public Function SavePosterFile(sourceFile As String, filmID As Long) As String
        Return SavePictureFile(sourceFile, "Posters", filmID, "Poster")
    End Function

    Public Function SaveFoodImageFile(sourceFile As String, foodItemID As Long) As String
        Return SavePictureFile(sourceFile, "FoodPictures", foodItemID, "Picture")
    End Function

    Public Sub DeletePictureFile(folderName As String, fileName As String)
        If fileName Is Nothing OrElse fileName.Trim() = "" Then
            Exit Sub
        End If

        Try
            System.IO.File.Delete(PictureFolder(folderName) & "\" & fileName)
        Catch ex As Exception
        End Try
    End Sub

    Public Sub DeletePosterFile(fileName As String)
        DeletePictureFile("Posters", fileName)
    End Sub

    Public Sub DeleteFoodImageFile(fileName As String)
        DeletePictureFile("FoodPictures", fileName)
    End Sub

    Public Function SystemSetting(settingName As String) As String
        Dim value As String = ""

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT SettingValue FROM tblSystemSetting WHERE SettingName = @SettingName"
            SQLCmd.Parameters.AddWithValue("@SettingName", settingName)
            Dim found As Object = SQLCmd.ExecuteScalar()

            If found IsNot Nothing AndAlso Not IsDBNull(found) Then
                value = found.ToString()
            End If

            cn.Close()
        End If

        Return value
    End Function

    Public Function TmdbKey() As String
        Return SystemSetting("TmdbApiKey")
    End Function

    Private Const TmdbSearchUrl As String = "https://api.themoviedb.org/3/search/movie"
    Private Const TmdbImageUrl As String = "https://image.tmdb.org/t/p/w500"

    Public Function JsonTextValue(json As String, fieldName As String, startAt As Integer) As String
        Dim marker As String = """" & fieldName & """:"""
        Dim atField As Integer = InStr(startAt, json, marker)

        If atField = 0 Then
            Return ""
        End If

        Dim valueStart As Integer = atField + Len(marker)
        Dim valueEnd As Integer = InStr(valueStart, json, """")

        If valueEnd = 0 Then
            Return ""
        End If

        Return Mid(json, valueStart, valueEnd - valueStart)
    End Function

    Public Function FetchPosterFromTmdb(filmTitle As String, filmYear As String, ByRef matchedAs As String) As String
        matchedAs = ""

        Dim apiKey As String = TmdbKey()

        If apiKey = "" Then
            MessageBox.Show("Fetching posters has not been set up yet, so there is no key to ask TMDB with." & vbCrLf &
                            "A poster can still be chosen off this computer with Choose picture.",
                            "Fetch poster", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return ""
        End If

        Dim url As String = TmdbSearchUrl & "?api_key=" & apiKey &
                            "&query=" & Uri.EscapeDataString(filmTitle)

        If filmYear.Trim() <> "" Then
            url = url & "&year=" & Uri.EscapeDataString(filmYear.Trim())
        End If

        Dim json As String = ""

        Try
            Dim client As New System.Net.WebClient
            client.Encoding = System.Text.Encoding.UTF8
            json = client.DownloadString(url)
            client.Dispose()
        Catch ex As Exception
            MessageBox.Show("Could not reach TMDB to look the film up. " & ex.Message, "Fetch poster",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return ""
        End Try

        Dim atPoster As Integer = InStr(json, """poster_path"":""")

        If atPoster = 0 Then
            MessageBox.Show("TMDB has no poster for a film matching that title and year.", "Fetch poster",
                            MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return ""
        End If

        Dim posterPath As String = JsonTextValue(json, "poster_path", atPoster)

        Dim atTitle As Integer = InStrRev(json, """title"":""", atPoster)

        If atTitle = 0 Then
            atTitle = 1
        End If

        Dim foundTitle As String = JsonTextValue(json, "title", atTitle)
        Dim foundDate As String = JsonTextValue(json, "release_date", atPoster)

        If posterPath = "" Then
            MessageBox.Show("TMDB answered but the reply could not be read.", "Fetch poster",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return ""
        End If

        If foundDate.Length >= 4 Then
            matchedAs = foundTitle & " (" & Mid(foundDate, 1, 4) & ")"
        Else
            matchedAs = foundTitle
        End If

        Dim tempFile As String = System.IO.Path.GetTempPath() & "filmtopia_poster.jpg"

        Try
            Dim client As New System.Net.WebClient
            client.DownloadFile(TmdbImageUrl & posterPath, tempFile)
            client.Dispose()
        Catch ex As Exception
            MessageBox.Show("Found the film but could not download the picture. " & ex.Message, "Fetch poster",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return ""
        End Try

        Return tempFile
    End Function

End Module
