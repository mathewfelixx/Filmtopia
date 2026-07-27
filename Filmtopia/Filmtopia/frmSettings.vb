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

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub frmSettings_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CommonFormStartup()
        txtFolder.Text = ""
        btnCreateBackup.Enabled = False
    End Sub

End Class
