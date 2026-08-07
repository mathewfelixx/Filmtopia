Imports System.IO
Imports System.Data.OleDb

Module modSettings
    Public Sub DarkMode()

    End Sub
    Public Sub LightMode()

    End Sub
    Public Sub ColourScheme()

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
