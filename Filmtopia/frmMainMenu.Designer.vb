<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmMainMenu
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.flpSidebar = New System.Windows.Forms.FlowLayoutPanel()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.btnFilms = New System.Windows.Forms.Button()
        Me.btnScreens = New System.Windows.Forms.Button()
        Me.btnLogs = New System.Windows.Forms.Button()
        Me.btnLogout = New System.Windows.Forms.Button()
        Me.pnlContent = New System.Windows.Forms.Panel()
        Me.lblWelcome = New System.Windows.Forms.Label()
        Me.flpSidebar.SuspendLayout()
        Me.pnlContent.SuspendLayout()
        Me.SuspendLayout()
        '
        'flpSidebar
        '
        Me.flpSidebar.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.flpSidebar.Controls.Add(Me.GroupBox1)
        Me.flpSidebar.Controls.Add(Me.btnFilms)
        Me.flpSidebar.Controls.Add(Me.btnScreens)
        Me.flpSidebar.Controls.Add(Me.btnLogs)
        Me.flpSidebar.Controls.Add(Me.btnLogout)
        Me.flpSidebar.Dock = System.Windows.Forms.DockStyle.Left
        Me.flpSidebar.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        Me.flpSidebar.ForeColor = System.Drawing.Color.White
        Me.flpSidebar.Location = New System.Drawing.Point(0, 0)
        Me.flpSidebar.Name = "flpSidebar"
        Me.flpSidebar.Size = New System.Drawing.Size(200, 603)
        Me.flpSidebar.TabIndex = 0
        Me.flpSidebar.WrapContents = False
        '
        'GroupBox1
        '
        Me.GroupBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.ForeColor = System.Drawing.Color.White
        Me.GroupBox1.Location = New System.Drawing.Point(3, 3)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(194, 80)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Filmtopia"
        '
        'btnFilms
        '
        Me.btnFilms.BackColor = System.Drawing.Color.Transparent
        Me.btnFilms.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnFilms.Location = New System.Drawing.Point(3, 89)
        Me.btnFilms.Name = "btnFilms"
        Me.btnFilms.Padding = New System.Windows.Forms.Padding(12, 0, 0, 0)
        Me.btnFilms.Size = New System.Drawing.Size(194, 32)
        Me.btnFilms.TabIndex = 1
        Me.btnFilms.Text = "Films"
        Me.btnFilms.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnFilms.UseVisualStyleBackColor = False
        '
        'btnScreens
        '
        Me.btnScreens.BackColor = System.Drawing.Color.Transparent
        Me.btnScreens.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnScreens.Location = New System.Drawing.Point(3, 127)
        Me.btnScreens.Name = "btnScreens"
        Me.btnScreens.Padding = New System.Windows.Forms.Padding(12, 0, 0, 0)
        Me.btnScreens.Size = New System.Drawing.Size(194, 32)
        Me.btnScreens.TabIndex = 2
        Me.btnScreens.Text = "Screens"
        Me.btnScreens.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnScreens.UseVisualStyleBackColor = False
        '
        'btnLogs
        '
        Me.btnLogs.BackColor = System.Drawing.Color.Transparent
        Me.btnLogs.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnLogs.Location = New System.Drawing.Point(3, 165)
        Me.btnLogs.Name = "btnLogs"
        Me.btnLogs.Padding = New System.Windows.Forms.Padding(12, 0, 0, 0)
        Me.btnLogs.Size = New System.Drawing.Size(194, 32)
        Me.btnLogs.TabIndex = 3
        Me.btnLogs.Text = "Logs"
        Me.btnLogs.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnLogs.UseVisualStyleBackColor = False
        '
        'btnLogout
        '
        Me.btnLogout.BackColor = System.Drawing.Color.Purple
        Me.btnLogout.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(180, Byte), Integer), CType(CType(210, Byte), Integer))
        Me.btnLogout.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnLogout.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnLogout.ForeColor = System.Drawing.Color.White
        Me.btnLogout.Location = New System.Drawing.Point(3, 203)
        Me.btnLogout.Name = "btnLogout"
        Me.btnLogout.Padding = New System.Windows.Forms.Padding(5, 0, 0, 0)
        Me.btnLogout.Size = New System.Drawing.Size(194, 34)
        Me.btnLogout.TabIndex = 4
        Me.btnLogout.Text = "Log out"
        Me.btnLogout.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnLogout.UseVisualStyleBackColor = False
        '
        'pnlContent
        '
        Me.pnlContent.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(248, Byte), Integer))
        Me.pnlContent.Controls.Add(Me.lblWelcome)
        Me.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlContent.Location = New System.Drawing.Point(200, 0)
        Me.pnlContent.Name = "pnlContent"
        Me.pnlContent.Size = New System.Drawing.Size(882, 603)
        Me.pnlContent.TabIndex = 1
        '
        'lblWelcome
        '
        Me.lblWelcome.AutoSize = True
        Me.lblWelcome.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWelcome.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.lblWelcome.Location = New System.Drawing.Point(30, 30)
        Me.lblWelcome.Name = "lblWelcome"
        Me.lblWelcome.Size = New System.Drawing.Size(145, 36)
        Me.lblWelcome.TabIndex = 0
        Me.lblWelcome.Text = "Welcome"
        '
        'frmMainMenu
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1082, 603)
        Me.Controls.Add(Me.pnlContent)
        Me.Controls.Add(Me.flpSidebar)
        Me.Name = "frmMainMenu"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Filmtopia - Main Menu"
        Me.flpSidebar.ResumeLayout(False)
        Me.pnlContent.ResumeLayout(False)
        Me.pnlContent.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents flpSidebar As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents btnFilms As System.Windows.Forms.Button
    Friend WithEvents btnScreens As System.Windows.Forms.Button
    Friend WithEvents btnLogs As System.Windows.Forms.Button
    Friend WithEvents btnLogout As System.Windows.Forms.Button
    Friend WithEvents pnlContent As System.Windows.Forms.Panel
    Friend WithEvents lblWelcome As System.Windows.Forms.Label
End Class
