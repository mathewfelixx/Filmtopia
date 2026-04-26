Imports System.Data.OleDb

Public Class frmLogin
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
                txtUsername.Text = ""
                txtPassword.Text = ""
                txtUsername.Focus()
                Me.Hide()
                frmMainMenu.Show()
            Else
                txtPassword.Text = ""
                MessageBox.Show("Incorrect username or password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtPassword.Focus()
            End If
        End If
    End Sub
End Class
