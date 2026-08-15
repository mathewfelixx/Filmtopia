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

    Private Sub btnChangePassword_Click(sender As Object, e As EventArgs) Handles btnChangePassword.Click
        If txtCurrentPW.Text = "" Or txtNewPW.Text = "" Or txtConfirmPW.Text = "" Then
            MessageBox.Show("Please fill in all three password boxes.", "Change Password", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If txtNewPW.Text.Length < 6 Then
            MessageBox.Show("Your new password must be at least 6 characters.", "Change Password", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If txtNewPW.Text <> txtConfirmPW.Text Then
            MessageBox.Show("The new passwords do not match.", "Change Password", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtNewPW.Text = ""
            txtConfirmPW.Text = ""
            txtNewPW.Focus()
            Exit Sub
        End If

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

    Private Sub ConfigureAccessLevel()
        If UserAccessLevel = 1 Then
            btnChooseFolder.Enabled = True
        Else
            btnChooseFolder.Enabled = False
            btnCreateBackup.Enabled = False
            lblHelp.Text = "Only a manager can back up the database."
        End If
    End Sub

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
        WriteLog("SETTINGS", "Settings form opened")

        If DarkModeOn Then
            rdoDark.Checked = True
        Else
            rdoLight.Checked = True
        End If
    End Sub

End Class
