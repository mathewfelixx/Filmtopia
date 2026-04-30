<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmMainForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMainForm))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.btnLogout = New System.Windows.Forms.Button()
        Me.btnBookings = New System.Windows.Forms.Button()
        Me.btnScreenings = New System.Windows.Forms.Button()
        Me.btnFilms = New System.Windows.Forms.Button()
        Me.btnReports = New System.Windows.Forms.Button()
        Me.pnlHeader = New System.Windows.Forms.Panel()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.lblAccessLevel = New System.Windows.Forms.Label()
        Me.pnlContent = New System.Windows.Forms.Panel()
        Me.lblVersion = New System.Windows.Forms.Label()
        Me.pnlHeader.SuspendLayout()
        Me.pnlContent.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnLogout
        '
        Me.btnLogout.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.btnLogout.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnLogout.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(180, Byte), Integer), CType(CType(210, Byte), Integer))
        Me.btnLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnLogout.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnLogout.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(180, Byte), Integer), CType(CType(210, Byte), Integer))
        Me.btnLogout.Location = New System.Drawing.Point(697, 18)
        Me.btnLogout.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.btnLogout.Name = "btnLogout"
        Me.btnLogout.Size = New System.Drawing.Size(83, 34)
        Me.btnLogout.TabIndex = 0
        Me.btnLogout.Text = "Log out"
        Me.ToolTip1.SetToolTip(Me.btnLogout, "Log out and return to the login screen")
        Me.btnLogout.UseVisualStyleBackColor = False
        '
        'btnBookings
        '
        Me.btnBookings.BackColor = System.Drawing.Color.White
        Me.btnBookings.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnBookings.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(220, Byte), Integer), CType(CType(220, Byte), Integer), CType(CType(225, Byte), Integer))
        Me.btnBookings.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnBookings.Font = New System.Drawing.Font("Segoe UI", 11.0!, System.Drawing.FontStyle.Bold)
        Me.btnBookings.ForeColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnBookings.Location = New System.Drawing.Point(27, 26)
        Me.btnBookings.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.btnBookings.Name = "btnBookings"
        Me.btnBookings.Size = New System.Drawing.Size(343, 107)
        Me.btnBookings.TabIndex = 1
        Me.btnBookings.Text = "Manage Bookings" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        Me.ToolTip1.SetToolTip(Me.btnBookings, "Add, view, search and cancel customer bookings")
        Me.btnBookings.UseVisualStyleBackColor = False
        '
        'btnScreenings
        '
        Me.btnScreenings.BackColor = System.Drawing.Color.White
        Me.btnScreenings.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnScreenings.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(220, Byte), Integer), CType(CType(220, Byte), Integer), CType(CType(225, Byte), Integer))
        Me.btnScreenings.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnScreenings.Font = New System.Drawing.Font("Segoe UI", 11.0!, System.Drawing.FontStyle.Bold)
        Me.btnScreenings.ForeColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnScreenings.Location = New System.Drawing.Point(403, 26)
        Me.btnScreenings.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.btnScreenings.Name = "btnScreenings"
        Me.btnScreenings.Size = New System.Drawing.Size(343, 107)
        Me.btnScreenings.TabIndex = 2
        Me.btnScreenings.Text = "Manage Screenings" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        Me.ToolTip1.SetToolTip(Me.btnScreenings, "Schedule and manage film screenings")
        Me.btnScreenings.UseVisualStyleBackColor = False
        '
        'btnFilms
        '
        Me.btnFilms.BackColor = System.Drawing.Color.White
        Me.btnFilms.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnFilms.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(220, Byte), Integer), CType(CType(220, Byte), Integer), CType(CType(225, Byte), Integer))
        Me.btnFilms.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnFilms.Font = New System.Drawing.Font("Segoe UI", 11.0!, System.Drawing.FontStyle.Bold)
        Me.btnFilms.ForeColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnFilms.Location = New System.Drawing.Point(27, 158)
        Me.btnFilms.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.btnFilms.Name = "btnFilms"
        Me.btnFilms.Size = New System.Drawing.Size(343, 107)
        Me.btnFilms.TabIndex = 3
        Me.btnFilms.Text = "Manage Films" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        Me.ToolTip1.SetToolTip(Me.btnFilms, "Add and manage films in the system")
        Me.btnFilms.UseVisualStyleBackColor = False
        '
        'btnReports
        '
        Me.btnReports.BackColor = System.Drawing.Color.White
        Me.btnReports.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnReports.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(220, Byte), Integer), CType(CType(220, Byte), Integer), CType(CType(225, Byte), Integer))
        Me.btnReports.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnReports.Font = New System.Drawing.Font("Segoe UI", 11.0!, System.Drawing.FontStyle.Bold)
        Me.btnReports.ForeColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.btnReports.Location = New System.Drawing.Point(403, 158)
        Me.btnReports.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.btnReports.Name = "btnReports"
        Me.btnReports.Size = New System.Drawing.Size(343, 107)
        Me.btnReports.TabIndex = 4
        Me.btnReports.Text = "View Reports" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        Me.ToolTip1.SetToolTip(Me.btnReports, "View sales reports and booking summaries")
        Me.btnReports.UseVisualStyleBackColor = False
        '
        'pnlHeader
        '
        Me.pnlHeader.BackColor = System.Drawing.Color.FromArgb(CType(CType(173, Byte), Integer), CType(CType(20, Byte), Integer), CType(CType(87, Byte), Integer))
        Me.pnlHeader.Controls.Add(Me.lblTitle)
        Me.pnlHeader.Controls.Add(Me.lblAccessLevel)
        Me.pnlHeader.Controls.Add(Me.btnLogout)
        Me.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlHeader.Location = New System.Drawing.Point(0, 0)
        Me.pnlHeader.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.pnlHeader.Name = "pnlHeader"
        Me.pnlHeader.Size = New System.Drawing.Size(800, 64)
        Me.pnlHeader.TabIndex = 0
        '
        'lblTitle
        '
        Me.lblTitle.AutoSize = True
        Me.lblTitle.Font = New System.Drawing.Font("Segoe UI", 16.0!, System.Drawing.FontStyle.Bold)
        Me.lblTitle.ForeColor = System.Drawing.Color.White
        Me.lblTitle.Location = New System.Drawing.Point(23, 16)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(140, 37)
        Me.lblTitle.TabIndex = 0
        Me.lblTitle.Text = "Filmtopia"
        '
        'lblAccessLevel
        '
        Me.lblAccessLevel.AutoSize = True
        Me.lblAccessLevel.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.lblAccessLevel.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(180, Byte), Integer), CType(CType(210, Byte), Integer))
        Me.lblAccessLevel.Location = New System.Drawing.Point(607, 22)
        Me.lblAccessLevel.Name = "lblAccessLevel"
        Me.lblAccessLevel.Size = New System.Drawing.Size(78, 23)
        Me.lblAccessLevel.TabIndex = 1
        Me.lblAccessLevel.Text = "Manager"
        '
        'pnlContent
        '
        Me.pnlContent.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(248, Byte), Integer))
        Me.pnlContent.Controls.Add(Me.btnBookings)
        Me.pnlContent.Controls.Add(Me.btnScreenings)
        Me.pnlContent.Controls.Add(Me.btnFilms)
        Me.pnlContent.Controls.Add(Me.btnReports)
        Me.pnlContent.Controls.Add(Me.lblVersion)
        Me.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlContent.Location = New System.Drawing.Point(0, 64)
        Me.pnlContent.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.pnlContent.Name = "pnlContent"
        Me.pnlContent.Padding = New System.Windows.Forms.Padding(27, 26, 27, 26)
        Me.pnlContent.Size = New System.Drawing.Size(800, 363)
        Me.pnlContent.TabIndex = 1
        '
        'lblVersion
        '
        Me.lblVersion.AutoSize = True
        Me.lblVersion.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lblVersion.ForeColor = System.Drawing.Color.Gray
        Me.lblVersion.Location = New System.Drawing.Point(27, 293)
        Me.lblVersion.Name = "lblVersion"
        Me.lblVersion.Size = New System.Drawing.Size(303, 20)
        Me.lblVersion.TabIndex = 5
        Me.lblVersion.Text = "Filmtopia Cinema Management System  v1.0"
        '
        'frmMainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(248, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(800, 427)
        Me.Controls.Add(Me.pnlContent)
        Me.Controls.Add(Me.pnlHeader)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.MaximizeBox = False
        Me.Name = "frmMainForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Filmtopia"
        Me.pnlHeader.ResumeLayout(False)
        Me.pnlHeader.PerformLayout()
        Me.pnlContent.ResumeLayout(False)
        Me.pnlContent.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents pnlHeader As System.Windows.Forms.Panel
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents lblAccessLevel As System.Windows.Forms.Label
    Friend WithEvents btnLogout As System.Windows.Forms.Button
    Friend WithEvents pnlContent As System.Windows.Forms.Panel
    Friend WithEvents btnBookings As System.Windows.Forms.Button
    Friend WithEvents btnScreenings As System.Windows.Forms.Button
    Friend WithEvents btnFilms As System.Windows.Forms.Button
    Friend WithEvents btnReports As System.Windows.Forms.Button
    Friend WithEvents lblVersion As System.Windows.Forms.Label

End Class