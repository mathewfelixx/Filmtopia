<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMainForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMainForm))
        Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.btnBookings = New System.Windows.Forms.Button()
        Me.btnScreenings = New System.Windows.Forms.Button()
        Me.btnFilms = New System.Windows.Forms.Button()
        Me.btnReports = New System.Windows.Forms.Button()
        Me.btnScreens = New System.Windows.Forms.Button()
        Me.pnlContent = New System.Windows.Forms.Panel()
        Me.btnFood = New System.Windows.Forms.Button()
        Me.btnLogout = New System.Windows.Forms.Button()
        Me.lblVersion = New System.Windows.Forms.Label()
        Me.btnCustomers = New System.Windows.Forms.Button()
        Me.btnSettings = New System.Windows.Forms.Button()
        Me.btnLogs = New System.Windows.Forms.Button()
        Me.FlowLayoutPanel1.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlContent.SuspendLayout()
        Me.SuspendLayout()
        '
        'FlowLayoutPanel1
        '
        Me.FlowLayoutPanel1.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.FlowLayoutPanel1.Controls.Add(Me.GroupBox1)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnBookings)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnScreenings)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnCustomers)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnFilms)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnScreens)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnFood)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnReports)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnLogs)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnSettings)
        Me.FlowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Left
        Me.FlowLayoutPanel1.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FlowLayoutPanel1.ForeColor = System.Drawing.Color.White
        Me.FlowLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
        Me.FlowLayoutPanel1.Size = New System.Drawing.Size(200, 603)
        Me.FlowLayoutPanel1.TabIndex = 0
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.PictureBox1)
        Me.GroupBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.ForeColor = System.Drawing.Color.White
        Me.GroupBox1.Location = New System.Drawing.Point(3, 3)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(197, 177)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Filmtopia Staff"
        '
        'PictureBox1
        '
        Me.PictureBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.InitialImage = Nothing
        Me.PictureBox1.Location = New System.Drawing.Point(3, 24)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(191, 150)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 0
        Me.PictureBox1.TabStop = False
        '
        'btnBookings
        '
        Me.btnBookings.BackColor = System.Drawing.Color.FromArgb(CType(CType(173, Byte), Integer), CType(CType(20, Byte), Integer), CType(CType(87, Byte), Integer))
        Me.btnBookings.Location = New System.Drawing.Point(3, 186)
        Me.btnBookings.Name = "btnBookings"
        Me.btnBookings.Size = New System.Drawing.Size(194, 41)
        Me.btnBookings.TabIndex = 1
        Me.btnBookings.Text = "Bookings"
        Me.btnBookings.UseVisualStyleBackColor = False
        '
        'btnScreenings
        '
        Me.btnScreenings.BackColor = System.Drawing.Color.FromArgb(CType(CType(173, Byte), Integer), CType(CType(20, Byte), Integer), CType(CType(87, Byte), Integer))
        Me.btnScreenings.Location = New System.Drawing.Point(3, 233)
        Me.btnScreenings.Name = "btnScreenings"
        Me.btnScreenings.Size = New System.Drawing.Size(194, 41)
        Me.btnScreenings.TabIndex = 2
        Me.btnScreenings.Text = "Screenings"
        Me.btnScreenings.UseVisualStyleBackColor = False
        '
        'btnFilms
        '
        Me.btnFilms.BackColor = System.Drawing.Color.FromArgb(CType(CType(173, Byte), Integer), CType(CType(20, Byte), Integer), CType(CType(87, Byte), Integer))
        Me.btnFilms.Location = New System.Drawing.Point(3, 327)
        Me.btnFilms.Name = "btnFilms"
        Me.btnFilms.Size = New System.Drawing.Size(194, 41)
        Me.btnFilms.TabIndex = 3
        Me.btnFilms.Text = "Films"
        Me.btnFilms.UseVisualStyleBackColor = False
        '
        'btnReports
        '
        Me.btnReports.BackColor = System.Drawing.Color.FromArgb(CType(CType(173, Byte), Integer), CType(CType(20, Byte), Integer), CType(CType(87, Byte), Integer))
        Me.btnReports.Location = New System.Drawing.Point(3, 468)
        Me.btnReports.Name = "btnReports"
        Me.btnReports.Size = New System.Drawing.Size(194, 41)
        Me.btnReports.TabIndex = 4
        Me.btnReports.Text = "Reports"
        Me.btnReports.UseVisualStyleBackColor = False
        '
        'btnScreens
        '
        Me.btnScreens.BackColor = System.Drawing.Color.FromArgb(CType(CType(173, Byte), Integer), CType(CType(20, Byte), Integer), CType(CType(87, Byte), Integer))
        Me.btnScreens.Location = New System.Drawing.Point(3, 374)
        Me.btnScreens.Name = "btnScreens"
        Me.btnScreens.Size = New System.Drawing.Size(194, 41)
        Me.btnScreens.TabIndex = 5
        Me.btnScreens.Text = "Screens"
        Me.btnScreens.UseVisualStyleBackColor = False
        '
        'pnlContent
        '
        Me.pnlContent.Controls.Add(Me.lblVersion)
        Me.pnlContent.Controls.Add(Me.btnLogout)
        Me.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlContent.Location = New System.Drawing.Point(200, 0)
        Me.pnlContent.Name = "pnlContent"
        Me.pnlContent.Size = New System.Drawing.Size(882, 603)
        Me.pnlContent.TabIndex = 1
        '
        'btnFood
        '
        Me.btnFood.BackColor = System.Drawing.Color.FromArgb(CType(CType(173, Byte), Integer), CType(CType(20, Byte), Integer), CType(CType(87, Byte), Integer))
        Me.btnFood.Location = New System.Drawing.Point(3, 421)
        Me.btnFood.Name = "btnFood"
        Me.btnFood.Size = New System.Drawing.Size(194, 41)
        Me.btnFood.TabIndex = 6
        Me.btnFood.Text = "Food"
        Me.btnFood.UseVisualStyleBackColor = False
        '
        'btnLogout
        '
        Me.btnLogout.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.btnLogout.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnLogout.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(180, Byte), Integer), CType(CType(210, Byte), Integer))
        Me.btnLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnLogout.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnLogout.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(180, Byte), Integer), CType(CType(210, Byte), Integer))
        Me.btnLogout.Location = New System.Drawing.Point(787, 11)
        Me.btnLogout.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.btnLogout.Name = "btnLogout"
        Me.btnLogout.Size = New System.Drawing.Size(83, 34)
        Me.btnLogout.TabIndex = 1
        Me.btnLogout.Text = "Log out"
        Me.btnLogout.UseVisualStyleBackColor = False
        '
        'lblVersion
        '
        Me.lblVersion.AutoSize = True
        Me.lblVersion.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lblVersion.ForeColor = System.Drawing.Color.Gray
        Me.lblVersion.Location = New System.Drawing.Point(6, 574)
        Me.lblVersion.Name = "lblVersion"
        Me.lblVersion.Size = New System.Drawing.Size(303, 20)
        Me.lblVersion.TabIndex = 6
        Me.lblVersion.Text = "Filmtopia Cinema Management System  v1.0"
        '
        'btnCustomers
        '
        Me.btnCustomers.BackColor = System.Drawing.Color.FromArgb(CType(CType(173, Byte), Integer), CType(CType(20, Byte), Integer), CType(CType(87, Byte), Integer))
        Me.btnCustomers.Location = New System.Drawing.Point(3, 280)
        Me.btnCustomers.Name = "btnCustomers"
        Me.btnCustomers.Size = New System.Drawing.Size(194, 41)
        Me.btnCustomers.TabIndex = 7
        Me.btnCustomers.Text = "Customers"
        Me.btnCustomers.UseVisualStyleBackColor = False
        '
        'btnSettings
        '
        Me.btnSettings.BackColor = System.Drawing.Color.FromArgb(CType(CType(173, Byte), Integer), CType(CType(20, Byte), Integer), CType(CType(87, Byte), Integer))
        Me.btnSettings.Location = New System.Drawing.Point(3, 562)
        Me.btnSettings.Name = "btnSettings"
        Me.btnSettings.Size = New System.Drawing.Size(194, 41)
        Me.btnSettings.TabIndex = 8
        Me.btnSettings.Text = "Settings"
        Me.btnSettings.UseVisualStyleBackColor = False
        '
        'btnLogs
        '
        Me.btnLogs.BackColor = System.Drawing.Color.FromArgb(CType(CType(173, Byte), Integer), CType(CType(20, Byte), Integer), CType(CType(87, Byte), Integer))
        Me.btnLogs.Location = New System.Drawing.Point(3, 515)
        Me.btnLogs.Name = "btnLogs"
        Me.btnLogs.Size = New System.Drawing.Size(194, 41)
        Me.btnLogs.TabIndex = 9
        Me.btnLogs.Text = "Logs"
        Me.btnLogs.UseVisualStyleBackColor = False
        '
        'frmMainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1082, 603)
        Me.Controls.Add(Me.pnlContent)
        Me.Controls.Add(Me.FlowLayoutPanel1)
        Me.Name = "frmMainForm"
        Me.Text = "Filmtopia Management System"
        Me.FlowLayoutPanel1.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlContent.ResumeLayout(False)
        Me.pnlContent.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents FlowLayoutPanel1 As FlowLayoutPanel
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents btnBookings As Button
    Friend WithEvents btnScreenings As Button
    Friend WithEvents pnlContent As Panel
    Friend WithEvents btnFilms As Button
    Friend WithEvents btnReports As Button
    Friend WithEvents btnScreens As Button
    Friend WithEvents btnFood As Button
    Friend WithEvents btnLogout As Button
    Friend WithEvents lblVersion As Label
    Friend WithEvents btnCustomers As Button
    Friend WithEvents btnSettings As Button
    Friend WithEvents btnLogs As Button
End Class
