Imports System.IO
Imports System.Data.OleDb

Module modSettings

    'the settings for whoever is logged in at the moment
    'when a new setting is added it needs a variable here, a line in ReadOneSetting and a line
    'in SaveUserSettings, and that is all
    Public DarkModeOn As Boolean = False

    'the filters the user last left the films and screenings screens on. they are kept as the text
    'that was showing in the box rather than the position in the list, because the genre list could
    'be added to later and a saved position would then point at the wrong genre
    Public LastGenreFilter As String = "All genres"
    Public LastScreeningsShow As String = "Still to come"
    Public LastScreeningsScreen As String = "All screens"

    'the colours the rest of the program uses, these get filled in by SetThemeColours
    Public FormBack As Color
    Public TextFore As Color
    Public InputBack As Color
    Public InputFore As Color
    Public ReadOnlyBack As Color
    Public ButtonBack As Color
    Public ButtonFore As Color
    Public PanelBack As Color
    Public PanelFore As Color
    Public HighlightBack As Color
    Public HighlightFore As Color
    Public SubtleFore As Color
    Public AccentFore As Color
    Public GridBack As Color
    Public GridLineCol As Color
    Public BorderCol As Color
    Public CardBack As Color
    Public SidebarHover As Color
    'the seat map colours on the bookings form
    Public SeatAvailable As Color
    Public SeatSelected As Color
    Public SeatTaken As Color
    Public SeatFore As Color
    Public SeatTakenFore As Color

    'the border round a seat on the map, which is how the sort of seat is shown. the background
    'is already saying whether it is free, picked or gone, so the sort has to go somewhere else
    Public SeatPremiumEdge As Color
    Public SeatAccessibleEdge As Color
    'used on the dashboard to show how full a screening is
    Public OccupancyHigh As Color
    Public OccupancyMed As Color
    'every other row in a grid, and the colour for a screening that has already been and gone
    Public AltRowBack As Color
    Public PastFore As Color
    'a dashboard card with the mouse over it
    Public CardHover As Color

    'picks which set of colours to use depending on the mode
    Public Sub SetThemeColours()
        'the sidebar purple and the pink highlight stay the same in both modes so the program
        'still looks like Filmtopia either way
        PanelBack = Color.FromArgb(64, 0, 64)
        PanelFore = Color.White
        HighlightBack = Color.FromArgb(216, 27, 96)
        HighlightFore = Color.White
        'a slightly lighter purple used when the mouse goes over a sidebar button
        SidebarHover = Color.FromArgb(92, 16, 90)

        If DarkModeOn Then
            FormBack = Color.FromArgb(43, 43, 46)
            TextFore = Color.FromArgb(232, 232, 232)
            InputBack = Color.FromArgb(60, 60, 65)
            InputFore = Color.FromArgb(236, 236, 236)
            ReadOnlyBack = Color.FromArgb(50, 50, 54)
            ButtonBack = Color.FromArgb(69, 69, 73)
            ButtonFore = Color.FromArgb(236, 236, 236)
            SubtleFore = Color.FromArgb(154, 154, 154)
            AccentFore = Color.FromArgb(231, 169, 231)
            GridBack = Color.FromArgb(50, 50, 54)
            GridLineCol = Color.FromArgb(74, 74, 80)
            BorderCol = Color.FromArgb(90, 90, 96)
            CardBack = Color.FromArgb(58, 58, 64)
            'a free seat is grey, a picked one is the Filmtopia pink, a taken one is a dull
            'maroon so it fades into the background and does not look clickable
            SeatAvailable = Color.FromArgb(69, 69, 75)
            SeatSelected = Color.FromArgb(216, 27, 96)
            SeatTaken = Color.FromArgb(92, 43, 62)
            SeatFore = Color.FromArgb(232, 232, 232)
            SeatTakenFore = Color.FromArgb(168, 128, 143)
            SeatPremiumEdge = Color.FromArgb(255, 202, 40)
            SeatAccessibleEdge = Color.FromArgb(100, 181, 246)
            OccupancyHigh = Color.FromArgb(255, 107, 107)
            OccupancyMed = Color.FromArgb(255, 183, 77)
            AltRowBack = Color.FromArgb(56, 56, 61)
            PastFore = Color.FromArgb(130, 130, 138)
            CardHover = Color.FromArgb(74, 74, 82)
        Else
            FormBack = SystemColors.Control
            TextFore = Color.Black
            InputBack = Color.White
            InputFore = Color.Black
            ReadOnlyBack = Color.FromArgb(230, 230, 230)
            ButtonBack = SystemColors.Control
            ButtonFore = Color.Black
            SubtleFore = Color.Gray
            AccentFore = Color.FromArgb(64, 0, 64)
            GridBack = Color.White
            GridLineCol = Color.FromArgb(210, 210, 210)
            BorderCol = Color.FromArgb(180, 180, 180)
            CardBack = Color.White
            'the same seat colours the form already used, kept so light mode looks unchanged
            SeatAvailable = Color.FromArgb(220, 220, 220)
            SeatSelected = Color.Fuchsia
            SeatTaken = Color.FromArgb(255, 192, 255)
            SeatFore = Color.Black
            SeatTakenFore = Color.Black
            SeatPremiumEdge = Color.FromArgb(184, 134, 11)
            SeatAccessibleEdge = Color.FromArgb(21, 101, 192)
            OccupancyHigh = Color.FromArgb(198, 40, 40)
            OccupancyMed = Color.FromArgb(239, 108, 0)
            AltRowBack = Color.FromArgb(245, 245, 248)
            PastFore = Color.FromArgb(140, 140, 140)
            CardHover = Color.FromArgb(238, 238, 245)
        End If
    End Sub

    'switches the program to dark mode
    Public Sub DarkMode()
        DarkModeOn = True
        SetThemeColours()
        SaveUserSettings()
        ApplyThemeToAllForms()
    End Sub

    'switches the program back to light mode
    Public Sub LightMode()
        DarkModeOn = False
        SetThemeColours()
        SaveUserSettings()
        ApplyThemeToAllForms()
    End Sub

    'colours a whole form and everything sitting on it
    Public Sub ColourScheme(frm As Form)
        SetThemeColours()
        frm.BackColor = FormBack
        frm.ForeColor = TextFore
        ColourControls(frm)
    End Sub

    'recolours every form that is open at the moment so the change happens straight away
    Public Sub ApplyThemeToAllForms()
        For Each openForm As Form In Application.OpenForms
            ColourScheme(openForm)
        Next
    End Sub

    'the sidebar and the header bar are purple in both modes so they are left alone
    Private Function IsBrandPanel(ctrl As Control) As Boolean
        If ctrl.Name = "FlowLayoutPanel1" Or ctrl.Name = "pnlHeader" Or ctrl.Name = "pnlSidebar" Then
            Return True
        Else
            Return False
        End If
    End Function

    'goes through the controls on a container and colours them, it calls itself again for
    'anything that has controls inside it like a group box or a panel
    Private Sub ColourControls(parent As Control)
        For Each ctrl As Control In parent.Controls
            If Not IsBrandPanel(ctrl) Then
                ColourOneControl(ctrl)

                'the seat map buttons use their own colours to show which seats are free, taken
                'or picked, so the theme must not go inside it and paint over them
                If ctrl.HasChildren And ctrl.Name <> "pnlSeatMap" Then
                    ColourControls(ctrl)
                End If
            End If
        Next
    End Sub

    'works out what a single control should look like from what type it is
    Private Sub ColourOneControl(ctrl As Control)
        If TypeOf ctrl Is DataGridView Then
            ColourGrid(CType(ctrl, DataGridView))

        ElseIf TypeOf ctrl Is TextBox Then
            Dim box As TextBox = CType(ctrl, TextBox)
            If box.ReadOnly Then
                box.BackColor = ReadOnlyBack
            Else
                box.BackColor = InputBack
            End If
            box.ForeColor = InputFore

        ElseIf TypeOf ctrl Is ComboBox Or TypeOf ctrl Is ListBox Then
            ctrl.BackColor = InputBack
            ctrl.ForeColor = InputFore

        ElseIf TypeOf ctrl Is Button Then
            ColourButton(CType(ctrl, Button))

        ElseIf TypeOf ctrl Is Label Then
            'the version label is meant to be faint, and the welcome message and the big numbers
            'on the dashboard cards use the accent colour so they stand out.
            'anything named lblSub something is a faint line under a heading, its a prefix rather
            'than one name now so a new screen can have as many of them as it needs
            If ctrl.Name = "lblScreen" Then
                'the screen bar on the booking form is purple in both modes so it keeps white text
                ctrl.ForeColor = PanelFore
            ElseIf ctrl.Name = "lblVersion" Or ctrl.Name.StartsWith("lblSub") Or
                   ctrl.Name = "lblGridCount" Or ctrl.Name = "lblNoRows" Or
                   ctrl.Name = "lblNoPoster" Or ctrl.Name = "lblNoPicture" Or
                   ctrl.Name.StartsWith("lblCardSub") Then
                ctrl.ForeColor = SubtleFore
            ElseIf ctrl.Name = "lblWelcome" Or ctrl.Name.StartsWith("lblStat") Then
                ctrl.ForeColor = AccentFore
            Else
                ctrl.ForeColor = TextFore
            End If

        ElseIf TypeOf ctrl Is Panel Then
            'the dashboard cards sit on top of the form so they use the lighter surface colour,
            'and the little coloured strip down the side of each card is left as it is
            If ctrl.Name.StartsWith("pnlAccent") Then
                'left alone on purpose
            ElseIf ctrl.Name.StartsWith("pnlCard") Then
                ctrl.BackColor = CardBack
                ctrl.ForeColor = TextFore
            Else
                ctrl.BackColor = FormBack
                ctrl.ForeColor = TextFore
            End If

        ElseIf TypeOf ctrl Is PictureBox Then
            'the two called PictureBox1 hold the logo on the startup and login screens and are
            'left alone. the ones that were given a name are the poster on the films form and the
            'picture on the food form, and those are empty boxes most of the time, so in dark mode
            'they were sat there as white squares until something was put in them
            If ctrl.Name.StartsWith("pic") Then
                ctrl.BackColor = InputBack
            End If

        ElseIf TypeOf ctrl Is DateTimePicker Then
            'windows draws these itself so changing the colours does nothing, they are left alone

        Else
            'group boxes, panels, check boxes and radio buttons all just take the form colours
            ctrl.BackColor = FormBack
            ctrl.ForeColor = TextFore
        End If
    End Sub

    'buttons need a bit more doing to them because windows normally draws them itself
    Private Sub ColourButton(btn As Button)
        If DarkModeOn Then
            btn.FlatStyle = FlatStyle.Flat
            btn.FlatAppearance.BorderColor = BorderCol
            btn.BackColor = ButtonBack
            btn.ForeColor = ButtonFore
        Else
            'putting it back to the normal windows button look
            btn.FlatStyle = FlatStyle.Standard
            btn.BackColor = ButtonBack
            btn.ForeColor = ButtonFore
            btn.UseVisualStyleBackColor = True
        End If
    End Sub

    'colours a grid, the headers need EnableHeadersVisualStyles turning off or they stay grey
    Private Sub ColourGrid(dgv As DataGridView)
        dgv.BackgroundColor = GridBack
        dgv.GridColor = GridLineCol
        dgv.EnableHeadersVisualStyles = False

        dgv.DefaultCellStyle.BackColor = InputBack
        dgv.DefaultCellStyle.ForeColor = InputFore
        dgv.DefaultCellStyle.SelectionBackColor = HighlightBack
        dgv.DefaultCellStyle.SelectionForeColor = HighlightFore

        'this was being set on the main menu grid and nowhere else, so switching to dark mode left
        'that grid with its light stripe until the next time it was filled in
        dgv.AlternatingRowsDefaultCellStyle.BackColor = AltRowBack

        dgv.ColumnHeadersDefaultCellStyle.BackColor = PanelBack
        dgv.ColumnHeadersDefaultCellStyle.ForeColor = PanelFore
        dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = PanelBack
        dgv.ColumnHeadersDefaultCellStyle.SelectionForeColor = PanelFore

        dgv.RowHeadersDefaultCellStyle.BackColor = FormBack
        dgv.RowHeadersDefaultCellStyle.ForeColor = TextFore
    End Sub

    'puts every setting back to what a brand new user would get
    Private Sub UseDefaultSettings()
        DarkModeOn = False
        LastGenreFilter = "All genres"
        LastScreeningsShow = "Still to come"
        LastScreeningsScreen = "All screens"
    End Sub

    'takes one row out of tblUserSettings and puts it in the right variable
    Private Sub ReadOneSetting(settingName As String, settingValue As String)
        If settingName.ToUpper() = "THEME" Then
            DarkModeOn = (settingValue.ToUpper() = "DARK")
        End If

        If settingName.ToUpper() = "GENREFILTER" Then
            LastGenreFilter = settingValue
        End If

        If settingName.ToUpper() = "SCREENINGSSHOW" Then
            LastScreeningsShow = settingValue
        End If

        If settingName.ToUpper() = "SCREENINGSSCREEN" Then
            LastScreeningsScreen = settingValue
        End If
        'any new settings get read here
    End Sub

    'loads the settings belonging to whoever has just logged in
    Public Sub LoadUserSettings(loginID As Long)
        CurrentLoginID = loginID

        'start from the defaults so a user with no settings saved yet still gets something sensible
        UseDefaultSettings()

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT SettingName, SettingValue " &
                                 "FROM tblUserSettings " &
                                 "WHERE LoginID = @LoginID"
            SQLCmd.Parameters.AddWithValue("@LoginID", loginID)

            Dim rs As OleDbDataReader = SQLCmd.ExecuteReader()
            While rs.Read()
                ReadOneSetting(rs("SettingName"), rs("SettingValue"))
            End While
            rs.Close()
            cn.Close()
        End If

        SetThemeColours()
    End Sub

    'saves one setting, it updates the row if the user already has one and adds it if they do not
    'the connection has to be open before this is called
    Private Sub SaveOneSetting(settingName As String, settingValue As String)
        Dim SQLCmd As New OleDbCommand
        SQLCmd.Connection = cn
        SQLCmd.CommandText = "UPDATE tblUserSettings " &
                             "SET SettingValue = @SettingValue " &
                             "WHERE LoginID = @LoginID AND SettingName = @SettingName"
        'the parameters have to be added in the same order they appear in the SQL above
        SQLCmd.Parameters.AddWithValue("@SettingValue", settingValue)
        SQLCmd.Parameters.AddWithValue("@LoginID", CurrentLoginID)
        SQLCmd.Parameters.AddWithValue("@SettingName", settingName)

        Dim rowsChanged As Integer = SQLCmd.ExecuteNonQuery()

        If rowsChanged = 0 Then
            'this user has never saved this setting before so a new row is needed
            Dim SQLCmd2 As New OleDbCommand
            SQLCmd2.Connection = cn
            SQLCmd2.CommandText = "INSERT INTO tblUserSettings (LoginID, SettingName, SettingValue) " &
                                  "VALUES (@LoginID, @SettingName, @SettingValue)"
            SQLCmd2.Parameters.AddWithValue("@LoginID", CurrentLoginID)
            SQLCmd2.Parameters.AddWithValue("@SettingName", settingName)
            SQLCmd2.Parameters.AddWithValue("@SettingValue", settingValue)
            SQLCmd2.ExecuteNonQuery()
        End If
    End Sub

    'saves the settings of the user who is logged in into the database
    Public Sub SaveUserSettings()
        If CurrentLoginID = 0 Then
            'nobody has logged in so there is nobody to save them for
            Exit Sub
        End If

        If DbConnect() Then
            If DarkModeOn Then
                SaveOneSetting("THEME", "DARK")
            Else
                SaveOneSetting("THEME", "LIGHT")
            End If
            SaveOneSetting("GENREFILTER", LastGenreFilter)
            SaveOneSetting("SCREENINGSSHOW", LastScreeningsShow)
            SaveOneSetting("SCREENINGSSCREEN", LastScreeningsScreen)
            'any new settings get saved here

            cn.Close()
        End If
    End Sub

    'called when someone logs out so the next person does not get the last persons settings
    Public Sub ClearUserSettings()
        CurrentLoginID = 0
        UseDefaultSettings()
        SetThemeColours()
    End Sub

    'checks the old password is right and if it is saves the new one, returns True if it worked
    Public Function ChangePassword(Username As String, OldPassword As String, NewPassword As String) As Boolean
        Dim changed As Boolean = False
        Dim wrongPassword As Boolean = False

        If DbConnect() Then
            'get the password currently stored for this user
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT Password " &
                                 "FROM tblLogin " &
                                 "WHERE Username = @Username"
            SQLCmd.Parameters.AddWithValue("@Username", Username)

            Dim storedPW As String = ""
            Dim rs As OleDbDataReader = SQLCmd.ExecuteReader()
            If rs.Read() Then
                storedPW = Decrypt(rs("Password"))
            End If
            rs.Close()

            If storedPW = "" Then
                MessageBox.Show("Could not find that user account.", "Change Password", MessageBoxButtons.OK, MessageBoxIcon.Error)
            ElseIf storedPW <> OldPassword Then
                MessageBox.Show("Your current password is not correct.", "Change Password", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                wrongPassword = True
            Else
                'old password matched so save the new one encrypted
                Dim SQLCmd2 As New OleDbCommand
                SQLCmd2.Connection = cn
                SQLCmd2.CommandText = "UPDATE tblLogin " &
                                      "SET Password = @Password " &
                                      "WHERE Username = @Username"
                SQLCmd2.Parameters.AddWithValue("@Password", Encrypt(NewPassword))
                SQLCmd2.Parameters.AddWithValue("@Username", Username)
                SQLCmd2.ExecuteNonQuery()
                changed = True
            End If

            cn.Close()
        End If

        'the log has to be written after the connection is closed because WriteLog opens it again
        If changed Then
            WriteLog("AUTH", "User '" & Username & "' changed their password", LogSecurity)
        ElseIf wrongPassword Then
            WriteLog("AUTH", "User '" & Username & "' failed to change password, wrong current password", LogWarning)
        End If

        Return changed
    End Function

    'copies Filmtopia.mdb into the folder the user picked, with the date and time in the file name
    'returns the full path it saved to, or an empty string if it did not work
    Public Function CreateBackup(destinationFolder As String) As String
        'the live database sits next to the exe
        Dim sourceFile As String = Application.StartupPath & "\Filmtopia.mdb"

        If Not File.Exists(sourceFile) Then
            MessageBox.Show("Could not find the database to back up.", "Backup Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return ""
        End If

        If Not Directory.Exists(destinationFolder) Then
            MessageBox.Show("That folder no longer exists.", "Backup Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return ""
        End If

        'colons are not allowed in file names so the time uses dashes
        Dim stamp As String = Format(Now, "dd-MM-yyyy HH-mm-ss")
        Dim backupFile As String = destinationFolder & "\Filmtopia Backup " & stamp & ".mdb"

        Try
            File.Copy(sourceFile, backupFile)
            WriteLog("BACKUP", "Database backed up to " & backupFile, LogSecurity)
            Return backupFile
        Catch ex As Exception
            MessageBox.Show("Could not create the backup. " & ex.Message, "Backup Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return ""
        End Try
    End Function

End Module
