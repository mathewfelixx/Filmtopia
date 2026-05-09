Imports System.Data.OleDb

Public Class frmLogin

    Dim attempts As Integer = 0

    Private Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        Dim retrievedPassword As String = GetUNPW(txtUsername.Text)

        If retrievedPassword = "" Or retrievedPassword = "Empty" Then
            Exit Sub
        End If

        If txtPassword.Text = retrievedPassword Then
            attempts = 0
            LogedIn = True
            WriteLog("AUTH", "User '" & txtUsername.Text & "' logged in successfully")
            ClearLoginFields()
            Me.Hide()
            frmMainFormOLD.ShowDialog()
        Else
            attempts += 1
            txtPassword.Text = ""
            txtPassword.Focus()

            If attempts >= 3 Then
                MessageBox.Show("Too many failed attempts. The application will now close.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Error)
                WriteLog("AUTH", "User '" & txtUsername.Text & "' failed password authentication.")
                Application.Exit()
            Else
                MessageBox.Show("Incorrect password. Attempt " & attempts & " of 3.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
        End If
    End Sub

    Private Function GetUNPW(username As String) As String
        Dim plainTextPW As String = ""

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT Password, AccessLevel FROM tblLogin WHERE Username = @Username"
            SQLCmd.Parameters.AddWithValue("@Username", username)

            Dim rs As OleDbDataReader = SQLCmd.ExecuteReader()

            If rs.Read() Then
                Dim strPW = rs("Password")
                plainTextPW = Decrypt(strPW)
                UserAccessLevel = rs("AccessLevel")
            Else
                If username = "" Then
                    MessageBox.Show("Username cannot be empty", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Else
                    MessageBox.Show("Username '" & username & "' not found.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                End If

            End If

            rs.Close()
            cn.Close()
        End If

        Return plainTextPW
    End Function

    Private Sub ClearLoginFields()
        txtUsername.Text = ""
        txtPassword.Text = ""
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Application.Exit()
    End Sub

    Private Sub frmLogin_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CommonFormStartup()
        ClearLoginFields()

        attempts = 0
    End Sub

    Private Sub frmLogin_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Application.Exit()
    End Sub




End Class