Imports System.Data.OleDb
Imports System.Text
Module modMain
    Private Const DatabasePath As String = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source='Filmtopia.mdb';
                                           Persist Security Info=false;"
    Public cn As OleDbConnection
    Public LogedIn As Boolean = False
    Public UserAccessLevel As Integer = 99  ' indicates the prvilage level the user has 99 for customers
    Public CurrentLoginID As Long = 0  ' the LoginID of whoever is logged in, 0 means nobody is

    'the two states a screen can be in. a screen that is having work done to it is not deleted,
    'it is marked, the same way a cancelled booking is marked instead of being thrown away. that
    'matters because a deleted screen would take its seats and its history with it, and the
    'screenings that have already been on in it still have to add up in the sales report
    Public Const ScreenInService As String = "In service"
    Public Const ScreenOutOfService As String = "Out of service"

    'says whether a screen is open for business. it is asked before a screening is scheduled, so
    'the marking on the screens form actually stops something rather than just being a colour
    Public Function ScreenIsInService(screenID As Long) As Boolean
        Dim status As String = ScreenInService

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT ScreenStatus FROM tblScreen WHERE ScreenID = @ScreenID"
            SQLCmd.Parameters.AddWithValue("@ScreenID", CInt(screenID))
            Dim answer As Object = SQLCmd.ExecuteScalar()

            'a screen made before the status column existed has nothing in it, and an empty one
            'is treated as open rather than shut so nothing that used to work suddenly stops
            If answer IsNot Nothing AndAlso Not IsDBNull(answer) Then
                status = answer.ToString()
            End If

            cn.Close()
        End If

        Return status <> ScreenOutOfService
    End Function

    'the two states a screening can be in. a screening that is pulled is marked rather than
    'deleted, for the same reason a cancelled booking is. deleting it would take the sale that
    'was made on it with it, and the refund on record still has to add up in the sales report.
    'it also means the reason it was pulled is kept, which a deleted row could not do
    Public Const ScreeningScheduled As String = "Scheduled"
    Public Const ScreeningCancelled As String = "Cancelled"

    'every query that lists what is on has to leave the cancelled ones out, and they all spell
    'the condition out in full rather than building it from the constant above. that is on
    'purpose. the script that checks the queries reads them straight out of the code as text, so
    'anything glued on by a function is invisible to it and the query would go unchecked. the
    'IS NULL half is there so a screening written before the column existed still counts as on:
    'AND (ScreeningStatus IS NULL OR ScreeningStatus <> 'Cancelled')

    'says whether a screening is still going ahead. a blank status is one written before the
    'column existed, and those are all going ahead
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

    'the two states a food item can be in. an item that is taken off the menu is marked rather
    'than deleted, for the same reason a screening is. it cannot be deleted once somebody has
    'bought one, the order lines would be left pointing at nothing, so without this there was no
    'way at all of retiring something and a discontinued item stayed on sale forever
    Public Const FoodOnSale As String = "On sale"
    Public Const FoodWithdrawn As String = "Withdrawn"

    'the places that offer food for sale leave the withdrawn ones out, and they spell the
    'condition out in full for the same reason the screening one does, so the checking script can
    'still see it. the IS NULL half covers an item written before the column existed:
    'WHERE (FoodItemStatus IS NULL OR FoodItemStatus <> 'Withdrawn')

    'says whether a food item can still be sold. a blank status is one written before the column
    'existed, and those are all still on sale
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
            '   MessageBox.Show("Opend Successfully ")
            Return True
        Catch ex As Exception
            MessageBox.Show("Unable to open the database. " & ex.Message, "Database", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function
    'same idea as DbConnect but it does not show a message if it cannot get in, and it closes
    'the connection again straight away. it is for things that run on a timer, where a popup
    'appearing every minute would make the program impossible to use
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
        'Function takes plain text concerts it into Unitext8 (UTF8) and uses VB built encrption function (basically it
        'XORS with key called UTF8) 
        ' it will return encrypted string called cipertext 

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

            Dim strDecrypt As New System.Text.UTF8Encoding
            Dim UTF_Decrypt As System.Text.Decoder = strDecrypt.GetDecoder
            Dim uData As Byte() = Convert.FromBase64String(CipherText)
            Dim CharNum As Integer = UTF_Decrypt.GetCharCount(uData, 0, uData.Length) ' number of char in ciphertext
            Dim Decrypt_Char As Char() = New Char(CharNum - 1) {}
            UTF_Decrypt.GetChars(uData, 0, uData.Length, Decrypt_Char, 0)
            PlainText = New String(Decrypt_Char)
        End If

        Return PlainText
    End Function

    Public Sub CommonFormStartup(frm As Form)
        LoadVersion()
        UpdateAllVersionLabels()
        ColourScheme(frm)

    End Sub

    'wraps a value in quotes for a csv file, and doubles up any quotes already in it.
    'without this a name or a film title containing a comma would be split into two columns
    'when the file was opened in Excel. it lives here because more than one form exports now
    Public Function CsvField(value As String) As String
        Return """" & value.Replace("""", """""") & """"
    End Function

    'puts a short line on the form saying what just happened. adding, changing and deleting a record
    'only ever wrote a log entry before, so on screen nothing moved except the grid and there was no
    'telling a save that worked from a button that had not been pressed properly. the colour comes
    'from the theme so it still reads in dark mode. the label is emptied again by the form's
    'ClearFields, so the message only lasts until the next thing is started
    Public Sub SayDone(lbl As Label, message As String)
        lbl.Text = message
        lbl.ForeColor = AccentFore
    End Sub

    'turns what somebody typed into a whole number. Val is used rather than CInt on its own because
    'it copes with the box being empty or having letters in it, but Val hands back a Double, and
    'CInt on a Double too big for an Integer throws instead of coming back with anything. pasting a
    'long number into the quantity box was enough to bring the program down, so anything out of
    'range comes back as 0 and gets refused by the validation like any other silly value would
    Public Function SafeInt(text As String) As Integer
        Dim number As Double = Val(text)

        If number > Integer.MaxValue Or number < Integer.MinValue Then
            Return 0
        End If

        Return CInt(number)
    End Function

    'gets a cell as text. an empty cell holds Nothing rather than an empty string, so calling
    'ToString on it straight off falls over
    Public Function CellAsText(row As DataGridViewRow, columnIndex As Integer) As String
        If row.Cells(columnIndex).Value Is Nothing Then
            Return ""
        End If

        Return row.Cells(columnIndex).Value.ToString()
    End Function

    'saves whatever is showing in a grid out to a csv file. it walks the grid's own columns rather
    'than having the column names typed in, so a screen that gains a column gains it in the export
    'too instead of quietly leaving it out. hidden columns are left out because they are not part
    'of what the user is looking at. returns True only if a file was actually written, so the form
    'that called it knows whether there is anything worth logging
    Public Function ExportGridToCsv(dgv As DataGridView, defaultFileName As String, title As String) As Boolean
        If dgv.Rows.Count = 0 Then
            MessageBox.Show("There is nothing on screen to export", title, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If

        Dim saveDialog As New SaveFileDialog
        saveDialog.Filter = "CSV files (*.csv)|*.csv"
        saveDialog.FileName = defaultFileName
        'without this the whole program is left sat in whatever folder was last saved to
        saveDialog.RestoreDirectory = True

        If saveDialog.ShowDialog() <> DialogResult.OK Then
            Return False
        End If

        Dim writer As New System.IO.StreamWriter(saveDialog.FileName)
        Dim line As String = ""

        'the headings come off the grid so the file says the same as the screen does. a column of
        'pictures is skipped, there is nothing sensible to write in a text file for one and asking
        'the cell for its text gives back the name of the class instead of anything readable
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

    'where a set of pictures is kept. they sit in a folder next to the program the same way the
    'database does, so all that is stored in the database is the name of the file to look for. the
    'folder is made the first time it is asked for, so a fresh copy of the program does not fall
    'over the first time somebody picks a picture.
    'there are two sorts now, posters and food, so the folder name is passed in rather than there
    'being a second copy of all of this with one word changed
    Public Function PictureFolder(folderName As String) As String
        Dim folder As String = Application.StartupPath & "\" & folderName

        If Not System.IO.Directory.Exists(folder) Then
            System.IO.Directory.CreateDirectory(folder)
        End If

        Return folder
    End Function

    'the folder the film posters are in
    Public Function PosterFolder() As String
        Return PictureFolder("Posters")
    End Function

    'the folder the food pictures are in
    Public Function FoodPictureFolder() As String
        Return PictureFolder("FoodPictures")
    End Function

    'reads a picture off disk ready to be put on screen. nothing comes back if the file is not
    'there or is not really a picture, and whatever asked for it decides what to show instead
    '
    'the file is read into memory and the picture copied out of it rather than using
    'Image.FromFile. FromFile keeps the file open for as long as the picture is on screen, so
    'picking one film and then trying to give an earlier one a different poster failed saying the
    'file was in use by another process. it looked like a permissions problem and was not one
    Public Function PictureFromFile(fullPath As String) As Image
        If Not System.IO.File.Exists(fullPath) Then
            Return Nothing
        End If

        'a file that is not a picture, or one that was only half copied, throws when it is read.
        'that is treated the same as having no picture at all rather than bringing the form down
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

    'loads a picture by name out of one of the picture folders. an empty name just means whatever
    'it belongs to has not been given a picture yet, which is not a problem, it comes back as
    'nothing and whatever asked for it decides what to show instead
    Public Function PictureByName(folderName As String, fileName As String) As Image
        If fileName Is Nothing OrElse fileName.Trim() = "" Then
            Return Nothing
        End If

        Return PictureFromFile(PictureFolder(folderName) & "\" & fileName)
    End Function

    'loads the poster for a film. the name comes from the film's FilmPoster column
    Public Function PosterImage(fileName As String) As Image
        Return PictureByName("Posters", fileName)
    End Function

    'loads the picture for a food item. the name comes from its FoodItemImage column
    Public Function FoodImage(fileName As String) As Image
        Return PictureByName("FoodPictures", fileName)
    End Function

    'makes a small copy of a picture at the size it is going to be drawn at. the grid on the food
    'screen shows one of these on every row, and a menu of twenty items each holding a quarter of
    'a megabyte of jpg is a lot of memory for pictures drawn forty pixels wide, so the big one is
    'thrown away as soon as the small one has been drawn out of it
    Public Function SmallPicture(folderName As String, fileName As String, width As Integer, height As Integer) As Image
        Dim big As Image = PictureByName(folderName, fileName)

        If big Is Nothing Then
            Return Nothing
        End If

        Dim small As New Bitmap(width, height)
        Dim g As Graphics = Graphics.FromImage(small)

        'without this the shrunk picture comes out blocky and looks worse than no picture at all
        g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic

        'the picture is fitted inside the box keeping its shape rather than being squashed to fill
        'it, which is what a picture box set to zoom does
        Dim scale As Double = Math.Min(width / big.Width, height / big.Height)
        Dim drawWidth As Integer = CInt(big.Width * scale)
        Dim drawHeight As Integer = CInt(big.Height * scale)

        g.DrawImage(big, (width - drawWidth) \ 2, (height - drawHeight) \ 2, drawWidth, drawHeight)

        g.Dispose()
        big.Dispose()

        Return small
    End Function

    'copies a picture somebody picked into one of the picture folders and hands back the name it
    'was saved under, or an empty string if it did not work. the file is named after the record it
    'belongs to, so two of them can never end up sharing a picture, and giving one a new picture
    'writes over its old one. the title is only what the message box is called if it goes wrong
    Public Function SavePictureFile(sourceFile As String, folderName As String, id As Long, title As String) As String
        If Not System.IO.File.Exists(sourceFile) Then
            MessageBox.Show("That picture could not be found.", title, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return ""
        End If

        'the extension is kept so the file is still a jpg or a png afterwards rather than being
        'renamed into something the picture box cannot read
        Dim newName As String = id & System.IO.Path.GetExtension(sourceFile).ToLower()

        Try
            System.IO.File.Copy(sourceFile, PictureFolder(folderName) & "\" & newName, True)
            Return newName
        Catch ex As Exception
            MessageBox.Show("Could not save the picture. " & ex.Message, title, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return ""
        End Try
    End Function

    'saves a picked picture as a film's poster
    Public Function SavePosterFile(sourceFile As String, filmID As Long) As String
        Return SavePictureFile(sourceFile, "Posters", filmID, "Poster")
    End Function

    'saves a picked picture as a food item's picture
    Public Function SaveFoodImageFile(sourceFile As String, foodItemID As Long) As String
        Return SavePictureFile(sourceFile, "FoodPictures", foodItemID, "Picture")
    End Function

    'removes a picture file. it does not complain if the file has already gone, because the only
    'thing that matters is that it is not there afterwards
    Public Sub DeletePictureFile(folderName As String, fileName As String)
        If fileName Is Nothing OrElse fileName.Trim() = "" Then
            Exit Sub
        End If

        Try
            System.IO.File.Delete(PictureFolder(folderName) & "\" & fileName)
        Catch ex As Exception
            'a picture that will not delete is not worth stopping the save for. it is left where
            'it is and nothing points at it any more
        End Try
    End Sub

    'removes a film's poster file
    Public Sub DeletePosterFile(fileName As String)
        DeletePictureFile("Posters", fileName)
    End Sub

    'removes a food item's picture file
    Public Sub DeleteFoodImageFile(fileName As String)
        DeletePictureFile("FoodPictures", fileName)
    End Sub

    'reads a setting that belongs to the whole program rather than to one person. tblUserSettings
    'already holds the things each user picks for themselves, like the theme, and every row in it
    'hangs off a LoginID. this is for the settings that are the same whoever is signed in, so it is
    'its own table with no user on it. an empty string comes back if it has never been written
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

    'the key that lets the program ask TMDB for a poster. it is deliberately not typed into the code.
    'the source of this program is kept in version control and pushed somewhere public, so a key
    'written in here would be published along with it for anybody to pick up and use. it goes in the
    'database instead, which is not something that gets shared. left unset the fetching just says it
    'has not been set up, and a poster can still be chosen off the computer by hand
    Public Function TmdbKey() As String
        Return SystemSetting("TmdbApiKey")
    End Function

    'where TMDB is asked about a film, and where the pictures themselves come from. w500 is the
    'middle size they offer, plenty for a poster drawn 92 wide on the kiosk without pulling down a
    'file far bigger than anything here needs
    Private Const TmdbSearchUrl As String = "https://api.themoviedb.org/3/search/movie"
    Private Const TmdbImageUrl As String = "https://image.tmdb.org/t/p/w500"

    'pulls one piece of text out of a lump of JSON. TMDB answers in JSON and the only way to read it
    'properly would be to bring in an outside library, which is a lot to add when three pieces of
    'text are wanted out of the whole reply. so it is done by hand instead. it looks for the field
    'name in quotes followed by a colon and a quote, and takes everything up to the next quote
    '
    'this is not a real JSON reader and does not pretend to be one. it would not cope with a value
    'that had a quote mark inside it, and it does not understand numbers or true and false at all.
    'the three fields it is used on are a file path, a date and a title, and none of those come back
    'with a quote in them, so for what it is being asked to do it is enough
    Public Function JsonTextValue(json As String, fieldName As String, startAt As Integer) As String
        'the quote on the front matters. without it, looking for title would also find the
        'original_title that sits a few fields earlier and the wrong one would come back
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

    'asks TMDB for a film and downloads the poster of the best match into a temporary file, handing
    'back where it was put. an empty string means no poster was got, and the reason has already been
    'said on screen by then. matchedAs comes back with what TMDB thought the film was, so the person
    'pressing the button can see whether it has found the right one
    '
    'nothing about this is needed for the program to work. it is a quicker way of doing something
    'that can already be done by choosing a file, so every way it can fail ends in a message and the
    'film is left exactly as it was
    Public Function FetchPosterFromTmdb(filmTitle As String, filmYear As String, ByRef matchedAs As String) As String
        matchedAs = ""

        Dim apiKey As String = TmdbKey()

        If apiKey = "" Then
            MessageBox.Show("Fetching posters has not been set up yet, so there is no key to ask TMDB with." & vbCrLf &
                            "A poster can still be chosen off this computer with Choose picture.",
                            "Fetch poster", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return ""
        End If

        'the title goes into a web address, so anything in it that means something in an address has
        'to be written the safe way. film titles are full of spaces, colons and the odd ampersand
        Dim url As String = TmdbSearchUrl & "?api_key=" & apiKey &
                            "&query=" & Uri.EscapeDataString(filmTitle)

        'the year narrows it down a lot. there are three films called The Italian Job and the year
        'is the only thing in the boxes that tells them apart
        If filmYear.Trim() <> "" Then
            url = url & "&year=" & Uri.EscapeDataString(filmYear.Trim())
        End If

        Dim json As String = ""

        'no internet, a blocked address, a key that has been turned off, TMDB being down. all of it
        'comes out here and none of it is worth bringing the program down over
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

        'the results come back best match first, so the first poster in the reply is the one wanted.
        'everything is read from where that poster was found rather than from the start of the reply,
        'because the title and the date are wanted off the same result and not off a later one
        Dim atPoster As Integer = InStr(json, """poster_path"":""")

        If atPoster = 0 Then
            MessageBox.Show("TMDB has no poster for a film matching that title and year.", "Fetch poster",
                            MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return ""
        End If

        Dim posterPath As String = JsonTextValue(json, "poster_path", atPoster)

        'the title and the date are both wanted off the same result as the poster, but TMDB put the
        'title before the poster in their reply and the date after it, so they have to be looked for
        'in opposite directions. searching forwards for the title steps straight over the one
        'belonging to this film and picks up the next result's instead, which is how the wrong film
        'ended up being named on screen the first time this was tried
        Dim atTitle As Integer = InStrRev(json, """title"":""", atPoster)

        'InStr will not start from nothing, and a reply with no title in it at all is not worth
        'falling over for
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

        'the date comes back as yyyy-mm-dd and only the year is any use here
        If foundDate.Length >= 4 Then
            matchedAs = foundTitle & " (" & Mid(foundDate, 1, 4) & ")"
        Else
            matchedAs = foundTitle
        End If

        'it goes to a temporary file rather than straight into the posters folder, because at this
        'point nothing has been saved and the film might not be kept. it is copied into the posters
        'folder by the same code that copies a picture chosen off the computer, once the film is saved
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
