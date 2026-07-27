Imports System.IO

Module modSettings
    Public Sub DarkMode()

    End Sub
    Public Sub LightMode()

    End Sub
    Public Sub ColourScheme()

    End Sub
    Public Sub ChangePassword(Username As String, OldPassword As String)

    End Sub

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
