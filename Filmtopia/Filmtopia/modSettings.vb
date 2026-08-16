Imports System.IO
Imports System.Data.OleDb

Module modSettings

    Public DarkModeOn As Boolean = False

    Public LastGenreFilter As String = "All genres"
    Public LastScreeningsShow As String = "Still to come"
    Public LastScreeningsScreen As String = "All screens"

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
    Public SeatAvailable As Color
    Public SeatSelected As Color
    Public SeatTaken As Color
    Public SeatFore As Color
    Public SeatTakenFore As Color

    Public SeatPremiumEdge As Color
    Public SeatAccessibleEdge As Color
    Public SeatSaverEdge As Color
    Public OccupancyHigh As Color
    Public OccupancyMed As Color
    Public AltRowBack As Color
    Public PastFore As Color
    Public CardHover As Color

    Public Sub SetThemeColours()
        PanelBack = Color.FromArgb(64, 0, 64)
        PanelFore = Color.White
        HighlightBack = Color.FromArgb(216, 27, 96)
        HighlightFore = Color.White
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
            SeatAvailable = Color.FromArgb(69, 69, 75)
            SeatSelected = Color.FromArgb(216, 27, 96)
            SeatTaken = Color.FromArgb(92, 43, 62)
            SeatFore = Color.FromArgb(232, 232, 232)
            SeatTakenFore = Color.FromArgb(168, 128, 143)
            SeatPremiumEdge = Color.FromArgb(255, 202, 40)
            SeatAccessibleEdge = Color.FromArgb(100, 181, 246)
            SeatSaverEdge = Color.FromArgb(90, 200, 130)
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
            SeatAvailable = Color.FromArgb(220, 220, 220)
            SeatSelected = Color.Fuchsia
            SeatTaken = Color.FromArgb(255, 192, 255)
            SeatFore = Color.Black
            SeatTakenFore = Color.Black
            SeatPremiumEdge = Color.FromArgb(184, 134, 11)
            SeatAccessibleEdge = Color.FromArgb(21, 101, 192)
            SeatSaverEdge = Color.FromArgb(30, 140, 70)
            OccupancyHigh = Color.FromArgb(198, 40, 40)
            OccupancyMed = Color.FromArgb(239, 108, 0)
            AltRowBack = Color.FromArgb(245, 245, 248)
            PastFore = Color.FromArgb(140, 140, 140)
            CardHover = Color.FromArgb(238, 238, 245)
        End If
    End Sub

    Public Sub DarkMode()
        DarkModeOn = True
        SetThemeColours()
        SaveUserSettings()
        ApplyThemeToAllForms()
    End Sub

    Public Sub LightMode()
        DarkModeOn = False
        SetThemeColours()
        SaveUserSettings()
        ApplyThemeToAllForms()
    End Sub

    Public Sub ColourScheme(frm As Form)
        SetThemeColours()
        frm.BackColor = FormBack
        frm.ForeColor = TextFore
        ColourControls(frm)
    End Sub

    Public Sub ApplyThemeToAllForms()
        For Each openForm As Form In Application.OpenForms
            ColourScheme(openForm)
        Next
    End Sub

    Private Function IsBrandPanel(ctrl As Control) As Boolean
        If ctrl.Name = "FlowLayoutPanel1" Or ctrl.Name = "pnlHeader" Or ctrl.Name = "pnlSidebar" Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub ColourControls(parent As Control)
        For Each ctrl As Control In parent.Controls
            If Not IsBrandPanel(ctrl) Then
                ColourOneControl(ctrl)

                If ctrl.HasChildren And ctrl.Name <> "pnlSeatMap" Then
                    ColourControls(ctrl)
                End If
            End If
        Next
    End Sub

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
            If ctrl.Name = "lblScreen" Then
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
            If ctrl.Name.StartsWith("pnlAccent") Then
            ElseIf ctrl.Name.StartsWith("pnlCard") Then
                ctrl.BackColor = CardBack
                ctrl.ForeColor = TextFore
            Else
                ctrl.BackColor = FormBack
                ctrl.ForeColor = TextFore
            End If

        ElseIf TypeOf ctrl Is PictureBox Then
            If ctrl.Name.StartsWith("pic") Then
                ctrl.BackColor = InputBack
            End If

        ElseIf TypeOf ctrl Is DateTimePicker Then

        Else
            ctrl.BackColor = FormBack
            ctrl.ForeColor = TextFore
        End If
    End Sub

    Private Sub ColourButton(btn As Button)
        If DarkModeOn Then
            btn.FlatStyle = FlatStyle.Flat
            btn.FlatAppearance.BorderColor = BorderCol
            btn.BackColor = ButtonBack
            btn.ForeColor = ButtonFore
        Else
            btn.FlatStyle = FlatStyle.Standard
            btn.BackColor = ButtonBack
            btn.ForeColor = ButtonFore
            btn.UseVisualStyleBackColor = True
        End If
    End Sub

    Private Sub ColourGrid(dgv As DataGridView)
        dgv.BackgroundColor = GridBack
        dgv.GridColor = GridLineCol
        dgv.EnableHeadersVisualStyles = False

        dgv.DefaultCellStyle.BackColor = InputBack
        dgv.DefaultCellStyle.ForeColor = InputFore
        dgv.DefaultCellStyle.SelectionBackColor = HighlightBack
        dgv.DefaultCellStyle.SelectionForeColor = HighlightFore

        dgv.AlternatingRowsDefaultCellStyle.BackColor = AltRowBack

        dgv.ColumnHeadersDefaultCellStyle.BackColor = PanelBack
        dgv.ColumnHeadersDefaultCellStyle.ForeColor = PanelFore
        dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = PanelBack
        dgv.ColumnHeadersDefaultCellStyle.SelectionForeColor = PanelFore

        dgv.RowHeadersDefaultCellStyle.BackColor = FormBack
        dgv.RowHeadersDefaultCellStyle.ForeColor = TextFore
    End Sub

    Private Sub UseDefaultSettings()
        DarkModeOn = False
        LastGenreFilter = "All genres"
        LastScreeningsShow = "Still to come"
        LastScreeningsScreen = "All screens"
    End Sub

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
    End Sub

    Public Sub LoadUserSettings(loginID As Long)
        CurrentLoginID = loginID

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

    Private Sub SaveOneSetting(settingName As String, settingValue As String)
        Dim SQLCmd As New OleDbCommand
        SQLCmd.Connection = cn
        SQLCmd.CommandText = "UPDATE tblUserSettings " &
                             "SET SettingValue = @SettingValue " &
                             "WHERE LoginID = @LoginID AND SettingName = @SettingName"
        SQLCmd.Parameters.AddWithValue("@SettingValue", settingValue)
        SQLCmd.Parameters.AddWithValue("@LoginID", CurrentLoginID)
        SQLCmd.Parameters.AddWithValue("@SettingName", settingName)

        Dim rowsChanged As Integer = SQLCmd.ExecuteNonQuery()

        If rowsChanged = 0 Then
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

    Public Sub SaveUserSettings()
        If CurrentLoginID = 0 Then
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

            cn.Close()
        End If
    End Sub

    Public Sub ClearUserSettings()
        CurrentLoginID = 0
        UseDefaultSettings()
        SetThemeColours()
    End Sub

    Public Function ChangePassword(Username As String, OldPassword As String, NewPassword As String) As Boolean
        Dim changed As Boolean = False
        Dim wrongPassword As Boolean = False

        If DbConnect() Then
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

        If changed Then
            WriteLog("AUTH", "User '" & Username & "' changed their password", LogSecurity)
        ElseIf wrongPassword Then
            WriteLog("AUTH", "User '" & Username & "' failed to change password, wrong current password", LogWarning)
        End If

        Return changed
    End Function

    Public Function CreateBackup(destinationFolder As String) As String
        Dim sourceFile As String = Application.StartupPath & "\Filmtopia.mdb"

        If Not File.Exists(sourceFile) Then
            MessageBox.Show("Could not find the database to back up.", "Backup Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return ""
        End If

        If Not Directory.Exists(destinationFolder) Then
            MessageBox.Show("That folder no longer exists.", "Backup Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return ""
        End If

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
