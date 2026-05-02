Imports System.Data.OleDb

Public Class frmLogin
    Private Const MaxAttempts As Integer = 3
    Private failedAttempts As Integer = 0

    Private Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        Dim username As String = txtUsername.Text.Trim()

        If username = "" Or txtPassword.Text = "" Then
            MessageBox.Show("Please enter a username and password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Else
            Dim SQLCmd As New OleDbCommand("SELECT LoginID, Username, [Password], AccessLevel FROM tblLogin WHERE Username = @Username")
            SQLCmd.Parameters.AddWithValue("@Username", username)
            Dim dt As New DataTable
            FillTable(SQLCmd, dt)

            Dim passwordRight As Boolean = False
            If dt.Rows.Count = 1 Then
                If Decode(dt.Rows(0)("Password")) = txtPassword.Text Then
                    passwordRight = True
                End If
            End If

            If passwordRight Then
                CurrentLoginID = dt.Rows(0)("LoginID")
                CurrentUsername = dt.Rows(0)("Username")
                UserAccessLevel = dt.Rows(0)("AccessLevel")
                WriteLog(LogAuth, CurrentUsername & " logged in")
                failedAttempts = 0
                txtUsername.Text = ""
                txtPassword.Text = ""
                txtUsername.Focus()
                Me.Hide()
                frmMainMenu.Show()
            Else
                failedAttempts = failedAttempts + 1
                txtPassword.Text = ""
                If failedAttempts >= MaxAttempts Then
                    MessageBox.Show("Too many failed attempts. Filmtopia will now close.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Application.Exit()
                Else
                    MessageBox.Show("Incorrect username or password. Attempt " & failedAttempts & " of " & MaxAttempts & ".", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    txtPassword.Focus()
                End If
            End If
        End If
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Application.Exit()
    End Sub
End Class
