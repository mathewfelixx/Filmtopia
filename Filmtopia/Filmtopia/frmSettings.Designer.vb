<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmSettings
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
        Me.components = New System.ComponentModel.Container()
        Me.lblHeading = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.lblFolderCaption = New System.Windows.Forms.Label()
        Me.txtFolder = New System.Windows.Forms.TextBox()
        Me.btnChooseFolder = New System.Windows.Forms.Button()
        Me.btnCreateBackup = New System.Windows.Forms.Button()
        Me.lblHelp = New System.Windows.Forms.Label()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.lblCurrent = New System.Windows.Forms.Label()
        Me.txtCurrentPW = New System.Windows.Forms.TextBox()
        Me.lblNew = New System.Windows.Forms.Label()
        Me.txtNewPW = New System.Windows.Forms.TextBox()
        Me.lblConfirm = New System.Windows.Forms.Label()
        Me.txtConfirmPW = New System.Windows.Forms.TextBox()
        Me.btnChangePassword = New System.Windows.Forms.Button()
        Me.lblPWHelp = New System.Windows.Forms.Label()
        Me.lblVersion = New System.Windows.Forms.Label()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.FolderBrowserDialog1 = New System.Windows.Forms.FolderBrowserDialog()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblHeading
        '
        Me.lblHeading.AutoSize = True
        Me.lblHeading.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.lblHeading.Location = New System.Drawing.Point(12, 9)
        Me.lblHeading.Name = "lblHeading"
        Me.lblHeading.Size = New System.Drawing.Size(88, 24)
        Me.lblHeading.TabIndex = 0
        Me.lblHeading.Text = "Settings"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.lblFolderCaption)
        Me.GroupBox1.Controls.Add(Me.txtFolder)
        Me.GroupBox1.Controls.Add(Me.btnChooseFolder)
        Me.GroupBox1.Controls.Add(Me.btnCreateBackup)
        Me.GroupBox1.Controls.Add(Me.lblHelp)
        Me.GroupBox1.Location = New System.Drawing.Point(16, 45)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(516, 155)
        Me.GroupBox1.TabIndex = 1
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Database Backup"
        '
        'lblFolderCaption
        '
        Me.lblFolderCaption.AutoSize = True
        Me.lblFolderCaption.Location = New System.Drawing.Point(12, 30)
        Me.lblFolderCaption.Name = "lblFolderCaption"
        Me.lblFolderCaption.Size = New System.Drawing.Size(79, 13)
        Me.lblFolderCaption.TabIndex = 0
        Me.lblFolderCaption.Text = "Backup folder:"
        '
        'txtFolder
        '
        Me.txtFolder.Location = New System.Drawing.Point(15, 48)
        Me.txtFolder.Name = "txtFolder"
        Me.txtFolder.ReadOnly = True
        Me.txtFolder.Size = New System.Drawing.Size(371, 20)
        Me.txtFolder.TabIndex = 1
        '
        'btnChooseFolder
        '
        Me.btnChooseFolder.Location = New System.Drawing.Point(392, 46)
        Me.btnChooseFolder.Name = "btnChooseFolder"
        Me.btnChooseFolder.Size = New System.Drawing.Size(108, 24)
        Me.btnChooseFolder.TabIndex = 2
        Me.btnChooseFolder.Text = "Choose Folder..."
        Me.btnChooseFolder.UseVisualStyleBackColor = True
        '
        'btnCreateBackup
        '
        Me.btnCreateBackup.Enabled = False
        Me.btnCreateBackup.Location = New System.Drawing.Point(15, 105)
        Me.btnCreateBackup.Name = "btnCreateBackup"
        Me.btnCreateBackup.Size = New System.Drawing.Size(128, 30)
        Me.btnCreateBackup.TabIndex = 4
        Me.btnCreateBackup.Text = "Create Backup"
        Me.btnCreateBackup.UseVisualStyleBackColor = True
        '
        'lblHelp
        '
        Me.lblHelp.AutoSize = True
        Me.lblHelp.Location = New System.Drawing.Point(12, 80)
        Me.lblHelp.Name = "lblHelp"
        Me.lblHelp.Size = New System.Drawing.Size(330, 13)
        Me.lblHelp.TabIndex = 3
        Me.lblHelp.Text = "Choose a folder first, then create a backup of the database."
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.lblCurrent)
        Me.GroupBox2.Controls.Add(Me.txtCurrentPW)
        Me.GroupBox2.Controls.Add(Me.lblNew)
        Me.GroupBox2.Controls.Add(Me.txtNewPW)
        Me.GroupBox2.Controls.Add(Me.lblConfirm)
        Me.GroupBox2.Controls.Add(Me.txtConfirmPW)
        Me.GroupBox2.Controls.Add(Me.btnChangePassword)
        Me.GroupBox2.Controls.Add(Me.lblPWHelp)
        Me.GroupBox2.Location = New System.Drawing.Point(16, 210)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(516, 175)
        Me.GroupBox2.TabIndex = 2
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Change Password"
        '
        'lblCurrent
        '
        Me.lblCurrent.AutoSize = True
        Me.lblCurrent.Location = New System.Drawing.Point(12, 31)
        Me.lblCurrent.Name = "lblCurrent"
        Me.lblCurrent.Size = New System.Drawing.Size(94, 13)
        Me.lblCurrent.TabIndex = 0
        Me.lblCurrent.Text = "Current password:"
        '
        'txtCurrentPW
        '
        Me.txtCurrentPW.Location = New System.Drawing.Point(152, 28)
        Me.txtCurrentPW.Name = "txtCurrentPW"
        Me.txtCurrentPW.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtCurrentPW.Size = New System.Drawing.Size(200, 20)
        Me.txtCurrentPW.TabIndex = 1
        '
        'lblNew
        '
        Me.lblNew.AutoSize = True
        Me.lblNew.Location = New System.Drawing.Point(12, 61)
        Me.lblNew.Name = "lblNew"
        Me.lblNew.Size = New System.Drawing.Size(81, 13)
        Me.lblNew.TabIndex = 2
        Me.lblNew.Text = "New password:"
        '
        'txtNewPW
        '
        Me.txtNewPW.Location = New System.Drawing.Point(152, 58)
        Me.txtNewPW.Name = "txtNewPW"
        Me.txtNewPW.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtNewPW.Size = New System.Drawing.Size(200, 20)
        Me.txtNewPW.TabIndex = 3
        '
        'lblConfirm
        '
        Me.lblConfirm.AutoSize = True
        Me.lblConfirm.Location = New System.Drawing.Point(12, 91)
        Me.lblConfirm.Name = "lblConfirm"
        Me.lblConfirm.Size = New System.Drawing.Size(122, 13)
        Me.lblConfirm.TabIndex = 4
        Me.lblConfirm.Text = "Confirm new password:"
        '
        'txtConfirmPW
        '
        Me.txtConfirmPW.Location = New System.Drawing.Point(152, 88)
        Me.txtConfirmPW.Name = "txtConfirmPW"
        Me.txtConfirmPW.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtConfirmPW.Size = New System.Drawing.Size(200, 20)
        Me.txtConfirmPW.TabIndex = 5
        '
        'btnChangePassword
        '
        Me.btnChangePassword.Location = New System.Drawing.Point(152, 122)
        Me.btnChangePassword.Name = "btnChangePassword"
        Me.btnChangePassword.Size = New System.Drawing.Size(140, 30)
        Me.btnChangePassword.TabIndex = 6
        Me.btnChangePassword.Text = "Change Password"
        Me.btnChangePassword.UseVisualStyleBackColor = True
        '
        'lblPWHelp
        '
        Me.lblPWHelp.AutoSize = True
        Me.lblPWHelp.ForeColor = System.Drawing.Color.Gray
        Me.lblPWHelp.Location = New System.Drawing.Point(358, 31)
        Me.lblPWHelp.Name = "lblPWHelp"
        Me.lblPWHelp.Size = New System.Drawing.Size(140, 13)
        Me.lblPWHelp.TabIndex = 7
        Me.lblPWHelp.Text = "At least 6 characters."
        '
        'lblVersion
        '
        Me.lblVersion.AutoSize = True
        Me.lblVersion.ForeColor = System.Drawing.Color.Gray
        Me.lblVersion.Location = New System.Drawing.Point(13, 400)
        Me.lblVersion.Name = "lblVersion"
        Me.lblVersion.Size = New System.Drawing.Size(45, 13)
        Me.lblVersion.TabIndex = 3
        Me.lblVersion.Text = "lblVersion"
        '
        'btnClose
        '
        Me.btnClose.Location = New System.Drawing.Point(444, 395)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(88, 26)
        Me.btnClose.TabIndex = 4
        Me.btnClose.Text = "Close"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'frmSettings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(548, 435)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.lblVersion)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.lblHeading)
        Me.Name = "frmSettings"
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Text = "Settings"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblHeading As Label
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents lblFolderCaption As Label
    Friend WithEvents txtFolder As TextBox
    Friend WithEvents btnChooseFolder As Button
    Friend WithEvents btnCreateBackup As Button
    Friend WithEvents lblHelp As Label
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents lblCurrent As Label
    Friend WithEvents txtCurrentPW As TextBox
    Friend WithEvents lblNew As Label
    Friend WithEvents txtNewPW As TextBox
    Friend WithEvents lblConfirm As Label
    Friend WithEvents txtConfirmPW As TextBox
    Friend WithEvents btnChangePassword As Button
    Friend WithEvents lblPWHelp As Label
    Friend WithEvents lblVersion As Label
    Friend WithEvents btnClose As Button
    Friend WithEvents FolderBrowserDialog1 As FolderBrowserDialog
End Class
