Imports System.IO
Imports System.Data.OleDb

Module modSettings

    'the settings for whoever is logged in at the moment
    'when a new setting is added it needs a variable here, a line in ReadOneSetting and a line
    'in SaveUserSettings, and that is all
    Public DarkModeOn As Boolean = False

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

    'picks which set of colours to use depending on the mode
    Public Sub SetThemeColours()
        'the sidebar purple and the pink highlight stay the same in both modes so the program
        'still looks like Filmtopia either way
        PanelBack = Color.FromArgb(64, 0, 64)
        PanelFore = Color.White
        HighlightBack = Color.FromArgb(216, 27, 96)
        HighlightFore = Color.White

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
        If ctrl.Name = "FlowLayoutPanel1" Or ctrl.Name = "pnlHeader" Then
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

                If ctrl.HasChildren Then
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
            'the version label is meant to be faint and the welcome message uses the accent colour
            If ctrl.Name = "lblVersion" Then
                ctrl.ForeColor = SubtleFore
            ElseIf ctrl.Name = "lblWelcome" Then
                ctrl.ForeColor = AccentFore
            Else
                ctrl.ForeColor = TextFore
            End If

        ElseIf TypeOf ctrl Is PictureBox Then
            'left alone because these hold the logo

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
    End Sub

    'takes one row out of tblUserSettings and puts it in the right variable
    Private Sub ReadOneSetting(settingName As String, settingValue As String)
        If settingName.ToUpper() = "THEME" Then
            DarkModeOn = (settingValue.ToUpper() = "DARK")
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
            WriteLog("AUTH", "User '" & Username & "' changed their password")
        ElseIf wrongPassword Then
            WriteLog("AUTH", "User '" & Username & "' failed to change password, wrong current password")
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
            WriteLog("BACKUP", "Database backed up to " & backupFile)
            Return backupFile
        Catch ex As Exception
            MessageBox.Show("Could not create the backup. " & ex.Message, "Backup Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return ""
        End Try
    End Function

End Module
