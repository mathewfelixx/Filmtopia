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
        Me.lblVersion = New System.Windows.Forms.Label()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.FolderBrowserDialog1 = New System.Windows.Forms.FolderBrowserDialog()
        Me.GroupBox1.SuspendLayout()
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
        Me.lblHeading.Text = "Backup"
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
        'lblVersion
        '
        Me.lblVersion.AutoSize = True
        Me.lblVersion.ForeColor = System.Drawing.Color.Gray
        Me.lblVersion.Location = New System.Drawing.Point(13, 220)
        Me.lblVersion.Name = "lblVersion"
        Me.lblVersion.Size = New System.Drawing.Size(45, 13)
        Me.lblVersion.TabIndex = 4
        Me.lblVersion.Text = "Filmtopia Cinema Management System  v1.0"
        '
        'btnClose
        '
        Me.btnClose.Location = New System.Drawing.Point(444, 215)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(88, 26)
        Me.btnClose.TabIndex = 5
        Me.btnClose.Text = "Close"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'frmSettings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(548, 255)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.lblVersion)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.lblHeading)
        Me.CancelButton = Me.btnClose
        Me.Name = "frmSettings"
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Text = "Backup"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
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
    Friend WithEvents lblVersion As Label
    Friend WithEvents btnClose As Button
    Friend WithEvents FolderBrowserDialog1 As FolderBrowserDialog
End Class
