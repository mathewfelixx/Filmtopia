<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmLogin
    Inherits System.Windows.Forms.Form

    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.pnlHeader = New System.Windows.Forms.Panel()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.lblSubtitle = New System.Windows.Forms.Label()
        Me.pnlContent = New System.Windows.Forms.Panel()
        Me.lblUsername = New System.Windows.Forms.Label()
        Me.txtUsername = New System.Windows.Forms.TextBox()
        Me.lblPassword = New System.Windows.Forms.Label()
        Me.txtPassword = New System.Windows.Forms.TextBox()
        Me.btnLogin = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.lblFooter = New System.Windows.Forms.Label()
        Me.pnlHeader.SuspendLayout()
        Me.pnlContent.SuspendLayout()
        Me.SuspendLayout()

        ' pnlHeader
        Me.pnlHeader.BackColor = System.Drawing.Color.FromArgb(173, 20, 87)
        Me.pnlHeader.Controls.Add(Me.lblTitle)
        Me.pnlHeader.Controls.Add(Me.lblSubtitle)
        Me.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlHeader.Location = New System.Drawing.Point(0, 0)
        Me.pnlHeader.Name = "pnlHeader"
        Me.pnlHeader.Size = New System.Drawing.Size(400, 80)
        Me.pnlHeader.TabIndex = 0

        ' lblTitle
        Me.lblTitle.AutoSize = True
        Me.lblTitle.Font = New System.Drawing.Font("Segoe UI", 16, System.Drawing.FontStyle.Bold)
        Me.lblTitle.ForeColor = System.Drawing.Color.White
        Me.lblTitle.Location = New System.Drawing.Point(20, 14)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Text = "Filmtopia"

        ' lblSubtitle
        Me.lblSubtitle.AutoSize = True
        Me.lblSubtitle.Font = New System.Drawing.Font("Segoe UI", 9)
        Me.lblSubtitle.ForeColor = System.Drawing.Color.FromArgb(255, 180, 210)
        Me.lblSubtitle.Location = New System.Drawing.Point(22, 50)
        Me.lblSubtitle.Name = "lblSubtitle"
        Me.lblSubtitle.Text = "Cinema Management System"

        ' pnlContent
        Me.pnlContent.BackColor = System.Drawing.Color.FromArgb(245, 245, 248)
        Me.pnlContent.Controls.Add(Me.lblUsername)
        Me.pnlContent.Controls.Add(Me.txtUsername)
        Me.pnlContent.Controls.Add(Me.lblPassword)
        Me.pnlContent.Controls.Add(Me.txtPassword)
        Me.pnlContent.Controls.Add(Me.btnLogin)
        Me.pnlContent.Controls.Add(Me.btnClose)
        Me.pnlContent.Controls.Add(Me.lblFooter)
        Me.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlContent.Location = New System.Drawing.Point(0, 80)
        Me.pnlContent.Name = "pnlContent"
        Me.pnlContent.Padding = New System.Windows.Forms.Padding(30)
        Me.pnlContent.Size = New System.Drawing.Size(400, 270)
        Me.pnlContent.TabIndex = 1

        ' lblUsername
        Me.lblUsername.AutoSize = True
        Me.lblUsername.Font = New System.Drawing.Font("Segoe UI", 9)
        Me.lblUsername.ForeColor = System.Drawing.Color.FromArgb(80, 80, 80)
        Me.lblUsername.Location = New System.Drawing.Point(30, 30)
        Me.lblUsername.Name = "lblUsername"
        Me.lblUsername.Text = "Username"

        ' txtUsername
        Me.txtUsername.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtUsername.Font = New System.Drawing.Font("Segoe UI", 10)
        Me.txtUsername.Location = New System.Drawing.Point(30, 50)
        Me.txtUsername.Name = "txtUsername"
        Me.txtUsername.Size = New System.Drawing.Size(340, 25)
        Me.txtUsername.TabIndex = 1

        ' lblPassword
        Me.lblPassword.AutoSize = True
        Me.lblPassword.Font = New System.Drawing.Font("Segoe UI", 9)
        Me.lblPassword.ForeColor = System.Drawing.Color.FromArgb(80, 80, 80)
        Me.lblPassword.Location = New System.Drawing.Point(30, 95)
        Me.lblPassword.Name = "lblPassword"
        Me.lblPassword.Text = "Password"

        ' txtPassword
        Me.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtPassword.Font = New System.Drawing.Font("Segoe UI", 10)
        Me.txtPassword.Location = New System.Drawing.Point(30, 115)
        Me.txtPassword.Name = "txtPassword"
        Me.txtPassword.PasswordChar = "*"c
        Me.txtPassword.Size = New System.Drawing.Size(340, 25)
        Me.txtPassword.TabIndex = 2

        ' btnLogin
        Me.btnLogin.BackColor = System.Drawing.Color.FromArgb(173, 20, 87)
        Me.btnLogin.FlatAppearance.BorderSize = 0
        Me.btnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnLogin.Font = New System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold)
        Me.btnLogin.ForeColor = System.Drawing.Color.White
        Me.btnLogin.Location = New System.Drawing.Point(30, 165)
        Me.btnLogin.Name = "btnLogin"
        Me.btnLogin.Size = New System.Drawing.Size(340, 38)
        Me.btnLogin.TabIndex = 3
        Me.btnLogin.Text = "Log In"
        Me.btnLogin.Cursor = System.Windows.Forms.Cursors.Hand

        ' btnClose
        Me.btnClose.BackColor = System.Drawing.Color.FromArgb(245, 245, 248)
        Me.btnClose.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(220, 220, 225)
        Me.btnClose.FlatAppearance.BorderSize = 1
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Segoe UI", 9)
        Me.btnClose.ForeColor = System.Drawing.Color.FromArgb(150, 150, 150)
        Me.btnClose.Location = New System.Drawing.Point(30, 215)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(340, 30)
        Me.btnClose.TabIndex = 4
        Me.btnClose.Text = "Cancel"
        Me.btnClose.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel

        ' lblFooter
        Me.lblFooter.AutoSize = True
        Me.lblFooter.Font = New System.Drawing.Font("Segoe UI", 8)
        Me.lblFooter.ForeColor = System.Drawing.Color.Gray
        Me.lblFooter.Location = New System.Drawing.Point(30, 255)
        Me.lblFooter.Name = "lblFooter"
        Me.lblFooter.Text = "Filmtopia Cinema Management System  v1.0"

        ' frmLogin
        Me.AcceptButton = Me.btnLogin
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(245, 245, 248)
        Me.CancelButton = Me.btnClose
        Me.ClientSize = New System.Drawing.Size(400, 350)
        Me.Controls.Add(Me.pnlContent)
        Me.Controls.Add(Me.pnlHeader)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = New System.Drawing.Icon("C:\Users\mathe\OneDrive - Holy Cross College\Computer Science\NEA\FILMTOPIA\FILMTOPIA\Backup for project resources\filmtopia-logo.ico")
        Me.MaximizeBox = False
        Me.Name = "frmLogin"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Filmtopia"
        Me.pnlHeader.ResumeLayout(False)
        Me.pnlHeader.PerformLayout()
        Me.pnlContent.ResumeLayout(False)
        Me.pnlContent.PerformLayout()
        Me.ResumeLayout(False)
    End Sub

    Friend WithEvents pnlHeader As System.Windows.Forms.Panel
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents lblSubtitle As System.Windows.Forms.Label
    Friend WithEvents pnlContent As System.Windows.Forms.Panel
    Friend WithEvents lblUsername As System.Windows.Forms.Label
    Friend WithEvents txtUsername As System.Windows.Forms.TextBox
    Friend WithEvents lblPassword As System.Windows.Forms.Label
    Friend WithEvents txtPassword As System.Windows.Forms.TextBox
    Friend WithEvents btnLogin As System.Windows.Forms.Button
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents lblFooter As System.Windows.Forms.Label

End Class