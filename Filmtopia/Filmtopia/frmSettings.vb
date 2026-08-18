Imports System.Data.OleDb

Public Class frmSettings

    Private Sub btnChooseFolder_Click(sender As Object, e As EventArgs) Handles btnChooseFolder.Click
        FolderBrowserDialog1.Description = "Choose a folder to save the backup in"

        If FolderBrowserDialog1.ShowDialog() = DialogResult.OK Then
            txtFolder.Text = FolderBrowserDialog1.SelectedPath
            btnCreateBackup.Enabled = True
            lblHelp.Text = "Ready to back up. Click Create Backup."
        End If
    End Sub

    Private Sub btnCreateBackup_Click(sender As Object, e As EventArgs) Handles btnCreateBackup.Click
        If txtFolder.Text = "" Then
            MessageBox.Show("Please choose a folder first.", "Backup", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Dim savedTo As String = CreateBackup(txtFolder.Text)

        If savedTo <> "" Then
            MessageBox.Show("Backup created successfully:" & vbNewLine & vbNewLine & savedTo, "Backup", MessageBoxButtons.OK, MessageBoxIcon.Information)
            lblHelp.Text = "Last backup saved at " & Format(Now, "HH:mm") & "."
        End If
    End Sub

    Private Sub ShowScreeningTimes()
        txtTrailerMinutes.Text = TrailerMinutes.ToString()
        txtTurnaroundMinutes.Text = TurnaroundMinutes.ToString()
        txtFirstShow.Text = MinutesAsTimeText(FirstShowMinutes)
        txtLastShow.Text = MinutesAsTimeText(LastShowMinutes)
        txtRoundToMinutes.Text = RoundToMinutes.ToString()
    End Sub

    Private Function NumberTypedIn(box As TextBox, caption As String, unit As String, lowest As Integer, highest As Integer, ByRef value As Integer) As Boolean
        Dim typed As String = box.Text.Trim()

        If Not IsNumeric(typed) Then
            MessageBox.Show(caption & " must be a whole number of " & unit & ".", "Cinema Settings", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            box.Focus()
            Return False
        End If

        value = CInt(typed)

        If value < lowest Or value > highest Then
            MessageBox.Show(caption & " must be between " & lowest & " and " & highest & " " & unit & ".", "Cinema Settings", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            box.Focus()
            Return False
        End If

        Return True
    End Function

    Private Sub btnSaveTimes_Click(sender As Object, e As EventArgs) Handles btnSaveTimes.Click
        Dim newTrailer As Integer = 0
        Dim newTurnaround As Integer = 0
        Dim newRoundTo As Integer = 0

        If Not NumberTypedIn(txtTrailerMinutes, "The adverts and trailers", "minutes", 0, 120, newTrailer) Then
            Exit Sub
        End If

        If Not NumberTypedIn(txtTurnaroundMinutes, "The clearing up time", "minutes", 0, 120, newTurnaround) Then
            Exit Sub
        End If

        If Not NumberTypedIn(txtRoundToMinutes, "Rounding the start times", "minutes", 1, 60, newRoundTo) Then
            Exit Sub
        End If

        If 60 Mod newRoundTo <> 0 Then
            MessageBox.Show("Round start times to a number of minutes that divides into an hour, like 5, 10, 15 or 30." & vbCrLf & vbCrLf &
                            "Anything else and the times drift through the hour instead of looking tidy.",
                            "Cinema Settings", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtRoundToMinutes.Focus()
            Exit Sub
        End If

        Dim firstTyped As String = txtFirstShow.Text.Trim()
        Dim lastTyped As String = txtLastShow.Text.Trim()

        If Not IsValidTimeText(firstTyped) Then
            MessageBox.Show("The earliest start must be a time like 10:00.", "Cinema Settings", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtFirstShow.Focus()
            Exit Sub
        End If

        If Not IsValidTimeText(lastTyped) Then
            MessageBox.Show("The latest start must be a time like 23:00.", "Cinema Settings", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtLastShow.Focus()
            Exit Sub
        End If

        If TimeTextAsMinutes(lastTyped, 0) <= TimeTextAsMinutes(firstTyped, 0) Then
            MessageBox.Show("The latest start has to be after the earliest start.", "Cinema Settings", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtLastShow.Focus()
            Exit Sub
        End If

        SaveSystemSetting("TrailerMinutes", newTrailer.ToString())
        SaveSystemSetting("TurnaroundMinutes", newTurnaround.ToString())
        SaveSystemSetting("FirstShowTime", firstTyped)
        SaveSystemSetting("LastShowTime", lastTyped)
        SaveSystemSetting("RoundToMinutes", newRoundTo.ToString())

        LoadSystemSettings()
        ShowScreeningTimes()

        WriteLog("SETTINGS", "Screening times changed to " & newTrailer & " minutes of adverts, " &
                             newTurnaround & " minutes clearing up, showing between " &
                             firstTyped & " and " & lastTyped & ", start times rounded up to " &
                             newRoundTo & " minutes", LogChange)

        MessageBox.Show("Screening times saved.", "Cinema Settings", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub ShowSelling()
        txtDefaultPrice.Text = Format(DefaultTicketPrice, "0.00")
    End Sub

    Private Sub ShowKiosk()
        txtMaxSeats.Text = MaxSeatsPerSale.ToString()
        txtIdleSeconds.Text = IdleSecondsAllowed.ToString()
        txtThankYouSeconds.Text = IdleSecondsOnThankYou.ToString()
    End Sub

    Private Sub ShowSecurity()
        txtLoginTries.Text = LoginTriesAllowed.ToString()
        txtMinPassword.Text = MinPasswordLength.ToString()
    End Sub

    Private Sub btnSaveSelling_Click(sender As Object, e As EventArgs) Handles btnSaveSelling.Click
        Dim typed As String = txtDefaultPrice.Text.Trim()

        If Not IsNumeric(typed) Then
            MessageBox.Show("The ticket price must be a number.", "Cinema Settings", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtDefaultPrice.Focus()
            Exit Sub
        End If

        Dim newPrice As Double = Val(typed)

        If newPrice <= 0 Then
            MessageBox.Show("The ticket price must be greater than 0.", "Cinema Settings", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtDefaultPrice.Focus()
            Exit Sub
        End If

        If newPrice > 50 Then
            MessageBox.Show("That ticket price looks too high, it should be in pounds.", "Cinema Settings", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtDefaultPrice.Focus()
            Exit Sub
        End If

        SaveSystemSetting("DefaultTicketPrice", Format(newPrice, "0.00"))

        LoadSystemSettings()
        ShowSelling()

        WriteLog("SETTINGS", "Ticket price a new screening starts at changed to " & Format(newPrice, "0.00"), LogChange)

        MessageBox.Show("Selling settings saved.", "Cinema Settings", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub btnSaveKiosk_Click(sender As Object, e As EventArgs) Handles btnSaveKiosk.Click
        Dim newMaxSeats As Integer = 0
        Dim newIdle As Integer = 0
        Dim newThankYou As Integer = 0

        If Not NumberTypedIn(txtMaxSeats, "The most tickets in one sale", "tickets", 1, 20, newMaxSeats) Then
            Exit Sub
        End If

        If Not NumberTypedIn(txtIdleSeconds, "The kiosk idle time", "seconds", 15, 600, newIdle) Then
            Exit Sub
        End If

        If Not NumberTypedIn(txtThankYouSeconds, "The thank you screen time", "seconds", 5, 120, newThankYou) Then
            Exit Sub
        End If

        SaveSystemSetting("MaxSeatsPerSale", newMaxSeats.ToString())
        SaveSystemSetting("KioskIdleSeconds", newIdle.ToString())
        SaveSystemSetting("KioskThankYouSeconds", newThankYou.ToString())

        LoadSystemSettings()
        ShowKiosk()

        WriteLog("SETTINGS", "Kiosk changed to " & newMaxSeats & " tickets a sale, resetting after " &
                             newIdle & " seconds and " & newThankYou & " seconds on the thank you screen", LogChange)

        MessageBox.Show("Kiosk settings saved.", "Cinema Settings", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub btnSaveSecurity_Click(sender As Object, e As EventArgs) Handles btnSaveSecurity.Click
        Dim newTries As Integer = 0
        Dim newLength As Integer = 0

        If Not NumberTypedIn(txtLoginTries, "The number of password tries", "tries", 1, 10, newTries) Then
            Exit Sub
        End If

        If Not NumberTypedIn(txtMinPassword, "The shortest password", "characters", 4, 20, newLength) Then
            Exit Sub
        End If

        SaveSystemSetting("LoginTriesAllowed", newTries.ToString())
        SaveSystemSetting("MinPasswordLength", newLength.ToString())

        LoadSystemSettings()
        ShowSecurity()

        WriteLog("SETTINGS", "Security changed to " & newTries & " password tries and a shortest password of " &
                             newLength & " characters", LogSecurity)

        MessageBox.Show("Security settings saved.", "Cinema Settings", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub ShowAccessLevels()
        cboNewAccessLevel.Items.Clear()
        cboNewAccessLevel.Items.Add("Manager, full access to everything")
        cboNewAccessLevel.Items.Add("Staff, the tills and the booking screens")
        cboNewAccessLevel.SelectedIndex = 1
    End Sub

    Private Function ChosenAccessLevel() As Integer
        If cboNewAccessLevel.SelectedIndex = 0 Then
            Return 1
        Else
            Return 2
        End If
    End Function

    Private Function RoleName(accessLevel As Integer) As String
        If accessLevel = 1 Then
            Return "a manager"
        Else
            Return "staff"
        End If
    End Function

    Private Function UsernameLooksOk(username As String) As Boolean
        Dim position As Integer

        For position = 1 To Len(username)
            Dim letter As Char = CChar(Mid(username, position, 1))

            If Not Char.IsLetterOrDigit(letter) Then
                Return False
            End If
        Next

        Return True
    End Function

    Private Function UsernameTaken(username As String) As Boolean
        Dim taken As Boolean = False

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT COUNT(*) FROM tblLogin WHERE Username = @Username"
            SQLCmd.Parameters.AddWithValue("@Username", username)

            If Val(SQLCmd.ExecuteScalar()) > 0 Then
                taken = True
            End If

            cn.Close()
        End If

        Return taken
    End Function

    Private Sub ClearNewAccount()
        txtNewUsername.Text = ""
        txtNewPassword.Text = ""
        cboNewAccessLevel.SelectedIndex = 1
        txtNewUsername.Focus()
    End Sub

    Private Sub btnCreateUser_Click(sender As Object, e As EventArgs) Handles btnCreateUser.Click
        Dim newUsername As String = txtNewUsername.Text.Trim()
        Dim newPassword As String = txtNewPassword.Text

        If newUsername.Length < 3 Then
            MessageBox.Show("The username must be at least 3 characters.", "Cinema Settings", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtNewUsername.Focus()
            Exit Sub
        End If

        If Not UsernameLooksOk(newUsername) Then
            MessageBox.Show("The username can only have letters and numbers in it.", "Cinema Settings", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtNewUsername.Focus()
            Exit Sub
        End If

        If newPassword.Length < MinPasswordLength Then
            MessageBox.Show("The password must be at least " & MinPasswordLength & " characters.", "Cinema Settings", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtNewPassword.Focus()
            Exit Sub
        End If

        If cboNewAccessLevel.SelectedIndex < 0 Then
            MessageBox.Show("Choose what the new user is allowed to do.", "Cinema Settings", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            cboNewAccessLevel.Focus()
            Exit Sub
        End If

        If UsernameTaken(newUsername) Then
            MessageBox.Show("There is already an account called '" & newUsername & "'.", "Cinema Settings", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtNewUsername.Focus()
            Exit Sub
        End If

        Dim newLevel As Integer = ChosenAccessLevel()

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "INSERT INTO tblLogin (Username, [Password], AccessLevel) " &
                                 "VALUES (@Username, @Password, @AccessLevel)"
            SQLCmd.Parameters.AddWithValue("@Username", newUsername)
            SQLCmd.Parameters.AddWithValue("@Password", Encrypt(newPassword))
            SQLCmd.Parameters.AddWithValue("@AccessLevel", newLevel)
            SQLCmd.ExecuteNonQuery()
            cn.Close()
        End If

        WriteLog("AUTH", "Account '" & newUsername & "' created as " & RoleName(newLevel), LogSecurity)

        MessageBox.Show("Account '" & newUsername & "' created. Tell them the password so they can sign in.", "Cinema Settings", MessageBoxButtons.OK, MessageBoxIcon.Information)

        ClearNewAccount()
    End Sub

    Private Sub ConfigureAccessLevel()
        If UserAccessLevel = 1 Then
            btnChooseFolder.Enabled = True
        Else
            btnChooseFolder.Enabled = False
            btnCreateBackup.Enabled = False
            lblHelp.Text = "Only a manager can back up the database."

            txtTrailerMinutes.ReadOnly = True
            txtTurnaroundMinutes.ReadOnly = True
            txtFirstShow.ReadOnly = True
            txtLastShow.ReadOnly = True
            txtRoundToMinutes.ReadOnly = True
            btnSaveTimes.Enabled = False
            lblTimesHelp.Text = "Only a manager can change the screening times."

            txtDefaultPrice.ReadOnly = True
            btnSaveSelling.Enabled = False
            lblSellingHelp.Text = "Only a manager can change the starting price."

            txtMaxSeats.ReadOnly = True
            txtIdleSeconds.ReadOnly = True
            txtThankYouSeconds.ReadOnly = True
            btnSaveKiosk.Enabled = False
            lblKioskHelp.Text = "Only a manager can change the kiosk."

            txtLoginTries.ReadOnly = True
            txtMinPassword.ReadOnly = True
            btnSaveSecurity.Enabled = False
            lblSecurityHelp.Text = "Only a manager can change these."
            txtNewUsername.ReadOnly = True
            txtNewPassword.ReadOnly = True
            cboNewAccessLevel.Enabled = False
            btnCreateUser.Enabled = False
            lblAccountsHelp.Text = "Only a manager can create accounts."
        End If
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub frmSettings_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CommonFormStartup(Me)
        txtFolder.Text = ""
        btnCreateBackup.Enabled = False
        ShowScreeningTimes()
        ShowSelling()
        ShowKiosk()
        ShowSecurity()
        ShowAccessLevels()
        ConfigureAccessLevel()
        WriteLog("SETTINGS", "Cinema settings opened")
    End Sub

End Class
