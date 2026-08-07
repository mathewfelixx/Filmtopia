Public Class frmSettings

    'lets the user pick where the backup should be saved
    Private Sub btnChooseFolder_Click(sender As Object, e As EventArgs) Handles btnChooseFolder.Click
        FolderBrowserDialog1.Description = "Choose a folder to save the backup in"

        If FolderBrowserDialog1.ShowDialog() = DialogResult.OK Then
            txtFolder.Text = FolderBrowserDialog1.SelectedPath
            'only allow a backup once a folder has actually been chosen
            btnCreateBackup.Enabled = True
            lblHelp.Text = "Ready to back up. Click Create Backup."
        End If
    End Sub

    'copies the database into the chosen folder with the date and time in the name
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

    'checks what the user typed then changes their password
    Private Sub btnChangePassword_Click(sender As Object, e As EventArgs) Handles btnChangePassword.Click
        'presence check on all three boxes
        If txtCurrentPW.Text = "" Or txtNewPW.Text = "" Or txtConfirmPW.Text = "" Then
            MessageBox.Show("Please fill in all three password boxes.", "Change Password", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        'length check so people cannot set something too short
        If txtNewPW.Text.Length < 6 Then
            MessageBox.Show("Your new password must be at least 6 characters.", "Change Password", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        'the two new password boxes have to match
        If txtNewPW.Text <> txtConfirmPW.Text Then
            MessageBox.Show("The new passwords do not match.", "Change Password", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtNewPW.Text = ""
            txtConfirmPW.Text = ""
            txtNewPW.Focus()
            Exit Sub
        End If

        'no point changing it to the same thing
        If txtNewPW.Text = txtCurrentPW.Text Then
            MessageBox.Show("Your new password must be different from your current one.", "Change Password", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If ChangePassword(frmLogin.globalusername, txtCurrentPW.Text, txtNewPW.Text) Then
            MessageBox.Show("Your password has been changed.", "Change Password", MessageBoxButtons.OK, MessageBoxIcon.Information)
            ClearPasswordFields()
        Else
            txtCurrentPW.Text = ""
            txtCurrentPW.Focus()
        End If
    End Sub

    Private Sub ClearPasswordFields()
        txtCurrentPW.Text = ""
        txtNewPW.Text = ""
        txtConfirmPW.Text = ""
    End Sub

    'only managers are allowed to take a backup of the database
    'the buttons get turned off rather than the whole group box, because greying out the group box
    'greys out the writing on it as well and that is very hard to read in dark mode
    Private Sub ConfigureAccessLevel()
        If UserAccessLevel = 1 Then
            btnChooseFolder.Enabled = True
        Else
            btnChooseFolder.Enabled = False
            btnCreateBackup.Enabled = False
            lblHelp.Text = "Only a manager can back up the database."
        End If
    End Sub

    'the theme buttons use Click rather than CheckedChanged so that ticking one in code on load
    'does not set the theme off by itself
    Private Sub rdoLight_Click(sender As Object, e As EventArgs) Handles rdoLight.Click
        LightMode()
    End Sub

    Private Sub rdoDark_Click(sender As Object, e As EventArgs) Handles rdoDark.Click
        DarkMode()
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub frmSettings_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CommonFormStartup(Me)
        txtFolder.Text = ""
        btnCreateBackup.Enabled = False
        ClearPasswordFields()
        ConfigureAccessLevel()

        'tick whichever theme is already being used
        If DarkModeOn Then
            rdoDark.Checked = True
        Else
            rdoLight.Checked = True
        End If
    End Sub

End Class
