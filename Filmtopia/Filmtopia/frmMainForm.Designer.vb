<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmMainForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMainForm))
        Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.btnBookings = New System.Windows.Forms.Button()
        Me.btnFindBooking = New System.Windows.Forms.Button()
        Me.btnScreenings = New System.Windows.Forms.Button()
        Me.btnCustomers = New System.Windows.Forms.Button()
        Me.btnFilms = New System.Windows.Forms.Button()
        Me.btnScreens = New System.Windows.Forms.Button()
        Me.btnFood = New System.Windows.Forms.Button()
        Me.btnReports = New System.Windows.Forms.Button()
        Me.btnLogs = New System.Windows.Forms.Button()
        Me.btnSettings = New System.Windows.Forms.Button()
        Me.btnLogout = New System.Windows.Forms.Button()
        Me.pnlContent = New System.Windows.Forms.Panel()
        Me.lblWelcome = New System.Windows.Forms.Label()
        Me.lblInstructions = New System.Windows.Forms.Label()
        Me.lblVersion = New System.Windows.Forms.Label()
        Me.pnlHeader = New System.Windows.Forms.Panel()
        Me.FlowLayoutPanel1.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlContent.SuspendLayout()
        Me.pnlHeader.SuspendLayout()
        Me.SuspendLayout()
        '
        'FlowLayoutPanel1
        '
        Me.FlowLayoutPanel1.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.FlowLayoutPanel1.Controls.Add(Me.GroupBox1)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnBookings)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnFindBooking)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnScreenings)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnCustomers)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnFilms)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnScreens)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnFood)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnReports)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnLogs)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnSettings)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnLogout)
        Me.FlowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Left
        Me.FlowLayoutPanel1.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FlowLayoutPanel1.ForeColor = System.Drawing.Color.White
        Me.FlowLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
        Me.FlowLayoutPanel1.Size = New System.Drawing.Size(200, 603)
        Me.FlowLayoutPanel1.TabIndex = 0
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.PictureBox1)
        Me.GroupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.GroupBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.ForeColor = System.Drawing.Color.White
        Me.GroupBox1.Location = New System.Drawing.Point(3, 3)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(197, 177)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Filmtopia Manager"
        '
        'PictureBox1
        '
        Me.PictureBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.InitialImage = Nothing
        Me.PictureBox1.Location = New System.Drawing.Point(3, 23)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(191, 151)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PictureBox1.TabIndex = 0
        Me.PictureBox1.TabStop = False
        '
        'btnBookings
        '
        Me.btnBookings.BackColor = System.Drawing.Color.Transparent
        Me.btnBookings.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnBookings.Location = New System.Drawing.Point(3, 186)
        Me.btnBookings.Name = "btnBookings"
        Me.btnBookings.Padding = New System.Windows.Forms.Padding(12, 0, 0, 0)
        Me.btnBookings.Size = New System.Drawing.Size(194, 32)
        Me.btnBookings.TabIndex = 1
        Me.btnBookings.Text = "Bookings"
        Me.btnBookings.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnBookings.UseVisualStyleBackColor = False
        '
        'btnFindBooking
        '
        Me.btnFindBooking.BackColor = System.Drawing.Color.Transparent
        Me.btnFindBooking.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnFindBooking.Location = New System.Drawing.Point(3, 224)
        Me.btnFindBooking.Name = "btnFindBooking"
        Me.btnFindBooking.Padding = New System.Windows.Forms.Padding(12, 0, 0, 0)
        Me.btnFindBooking.Size = New System.Drawing.Size(194, 32)
        Me.btnFindBooking.TabIndex = 10
        Me.btnFindBooking.Text = "Find Booking"
        Me.btnFindBooking.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnFindBooking.UseVisualStyleBackColor = False
        '
        'btnScreenings
        '
        Me.btnScreenings.BackColor = System.Drawing.Color.Transparent
        Me.btnScreenings.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnScreenings.Location = New System.Drawing.Point(3, 262)
        Me.btnScreenings.Name = "btnScreenings"
        Me.btnScreenings.Padding = New System.Windows.Forms.Padding(12, 0, 0, 0)
        Me.btnScreenings.Size = New System.Drawing.Size(194, 32)
        Me.btnScreenings.TabIndex = 2
        Me.btnScreenings.Text = "Screenings"
        Me.btnScreenings.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnScreenings.UseVisualStyleBackColor = False
        '
        'btnCustomers
        '
        Me.btnCustomers.BackColor = System.Drawing.Color.Transparent
        Me.btnCustomers.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnCustomers.Location = New System.Drawing.Point(3, 300)
        Me.btnCustomers.Name = "btnCustomers"
        Me.btnCustomers.Padding = New System.Windows.Forms.Padding(12, 0, 0, 0)
        Me.btnCustomers.Size = New System.Drawing.Size(194, 32)
        Me.btnCustomers.TabIndex = 7
        Me.btnCustomers.Text = "Customers"
        Me.btnCustomers.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCustomers.UseVisualStyleBackColor = False
        '
        'btnFilms
        '
        Me.btnFilms.BackColor = System.Drawing.Color.FromArgb(CType(CType(173, Byte), Integer), CType(CType(20, Byte), Integer), CType(CType(87, Byte), Integer))
        Me.btnFilms.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnFilms.Location = New System.Drawing.Point(3, 338)
        Me.btnFilms.Name = "btnFilms"
        Me.btnFilms.Padding = New System.Windows.Forms.Padding(12, 0, 0, 0)
        Me.btnFilms.Size = New System.Drawing.Size(194, 32)
        Me.btnFilms.TabIndex = 3
        Me.btnFilms.Text = "Films"
        Me.btnFilms.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnFilms.UseVisualStyleBackColor = False
        '
        'btnScreens
        '
        Me.btnScreens.BackColor = System.Drawing.Color.Transparent
        Me.btnScreens.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnScreens.Location = New System.Drawing.Point(3, 376)
        Me.btnScreens.Name = "btnScreens"
        Me.btnScreens.Padding = New System.Windows.Forms.Padding(12, 0, 0, 0)
        Me.btnScreens.Size = New System.Drawing.Size(194, 32)
        Me.btnScreens.TabIndex = 5
        Me.btnScreens.Text = "Screens"
        Me.btnScreens.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnScreens.UseVisualStyleBackColor = False
        '
        'btnFood
        '
        Me.btnFood.BackColor = System.Drawing.Color.Transparent
        Me.btnFood.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnFood.Location = New System.Drawing.Point(3, 414)
        Me.btnFood.Name = "btnFood"
        Me.btnFood.Padding = New System.Windows.Forms.Padding(12, 0, 0, 0)
        Me.btnFood.Size = New System.Drawing.Size(194, 32)
        Me.btnFood.TabIndex = 6
        Me.btnFood.Text = "Food"
        Me.btnFood.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnFood.UseVisualStyleBackColor = False
        '
        'btnReports
        '
        Me.btnReports.BackColor = System.Drawing.Color.Transparent
        Me.btnReports.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnReports.Location = New System.Drawing.Point(3, 452)
        Me.btnReports.Name = "btnReports"
        Me.btnReports.Padding = New System.Windows.Forms.Padding(12, 0, 0, 0)
        Me.btnReports.Size = New System.Drawing.Size(194, 32)
        Me.btnReports.TabIndex = 4
        Me.btnReports.Text = "Reports"
        Me.btnReports.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnReports.UseVisualStyleBackColor = False
        '
        'btnLogs
        '
        Me.btnLogs.BackColor = System.Drawing.Color.Transparent
        Me.btnLogs.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnLogs.Location = New System.Drawing.Point(3, 490)
        Me.btnLogs.Name = "btnLogs"
        Me.btnLogs.Padding = New System.Windows.Forms.Padding(12, 0, 0, 0)
        Me.btnLogs.Size = New System.Drawing.Size(194, 32)
        Me.btnLogs.TabIndex = 9
        Me.btnLogs.Text = "Logs"
        Me.btnLogs.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnLogs.UseVisualStyleBackColor = False
        '
        'btnSettings
        '
        Me.btnSettings.BackColor = System.Drawing.Color.Transparent
        Me.btnSettings.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnSettings.Location = New System.Drawing.Point(3, 528)
        Me.btnSettings.Name = "btnSettings"
        Me.btnSettings.Padding = New System.Windows.Forms.Padding(12, 0, 0, 0)
        Me.btnSettings.Size = New System.Drawing.Size(194, 32)
        Me.btnSettings.TabIndex = 8
        Me.btnSettings.Text = "Settings"
        Me.btnSettings.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSettings.UseVisualStyleBackColor = False
        '
        'btnLogout
        '
        Me.btnLogout.BackColor = System.Drawing.Color.Purple
        Me.btnLogout.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnLogout.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(180, Byte), Integer), CType(CType(210, Byte), Integer))
        Me.btnLogout.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnLogout.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnLogout.ForeColor = System.Drawing.Color.White
        Me.btnLogout.Location = New System.Drawing.Point(3, 565)
        Me.btnLogout.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.btnLogout.Name = "btnLogout"
        Me.btnLogout.Padding = New System.Windows.Forms.Padding(5, 0, 0, 0)
        Me.btnLogout.Size = New System.Drawing.Size(191, 34)
        Me.btnLogout.TabIndex = 1
        Me.btnLogout.Text = "-Log out"
        Me.btnLogout.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnLogout.UseVisualStyleBackColor = False
        '
        'pnlContent
        '
        Me.pnlContent.Controls.Add(Me.lblWelcome)
        Me.pnlContent.Controls.Add(Me.lblInstructions)
        Me.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlContent.Location = New System.Drawing.Point(200, 109)
        Me.pnlContent.Name = "pnlContent"
        Me.pnlContent.Size = New System.Drawing.Size(882, 494)
        Me.pnlContent.TabIndex = 1
        '
        'lblWelcome
        '
        Me.lblWelcome.AutoSize = True
        Me.lblWelcome.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWelcome.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.lblWelcome.Location = New System.Drawing.Point(40, 40)
        Me.lblWelcome.Name = "lblWelcome"
        Me.lblWelcome.Size = New System.Drawing.Size(154, 36)
        Me.lblWelcome.TabIndex = 0
        Me.lblWelcome.Text = "Welcome!"
        '
        'lblInstructions
        '
        Me.lblInstructions.AutoSize = True
        Me.lblInstructions.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInstructions.Location = New System.Drawing.Point(40, 90)
        Me.lblInstructions.Name = "lblInstructions"
        Me.lblInstructions.Size = New System.Drawing.Size(523, 240)
        Me.lblInstructions.TabIndex = 1
        Me.lblInstructions.Text = resources.GetString("lblInstructions.Text")
        '
        'lblVersion
        '
        Me.lblVersion.AutoSize = True
        Me.lblVersion.Font = New System.Drawing.Font("Microsoft Yi Baiti", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblVersion.ForeColor = System.Drawing.Color.Gray
        Me.lblVersion.Location = New System.Drawing.Point(620, 79)
        Me.lblVersion.Name = "lblVersion"
        Me.lblVersion.Size = New System.Drawing.Size(215, 13)
        Me.lblVersion.TabIndex = 6
        Me.lblVersion.Text = "Filmtopia Cinema Management System  v1.0"
        '
        'pnlHeader
        '
        Me.pnlHeader.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.pnlHeader.Controls.Add(Me.lblVersion)
        Me.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlHeader.Location = New System.Drawing.Point(200, 0)
        Me.pnlHeader.Name = "pnlHeader"
        Me.pnlHeader.Size = New System.Drawing.Size(882, 109)
        Me.pnlHeader.TabIndex = 2
        '
        'frmMainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1082, 603)
        Me.Controls.Add(Me.pnlContent)
        Me.Controls.Add(Me.pnlHeader)
        Me.Controls.Add(Me.FlowLayoutPanel1)
        Me.Name = "frmMainForm"
        Me.Text = "Filmtopia Management System"
        Me.FlowLayoutPanel1.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlContent.ResumeLayout(False)
        Me.pnlContent.PerformLayout()
        Me.pnlHeader.ResumeLayout(False)
        Me.pnlHeader.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents FlowLayoutPanel1 As FlowLayoutPanel
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents btnBookings As Button
    Friend WithEvents btnFindBooking As Button
    Friend WithEvents btnScreenings As Button
    Friend WithEvents pnlContent As Panel
    Friend WithEvents lblWelcome As Label
    Friend WithEvents lblInstructions As Label
    Friend WithEvents btnFilms As Button
    Friend WithEvents btnReports As Button
    Friend WithEvents btnScreens As Button
    Friend WithEvents btnFood As Button
    Friend WithEvents btnLogout As Button
    Friend WithEvents lblVersion As Label
    Friend WithEvents btnCustomers As Button
    Friend WithEvents btnSettings As Button
    Friend WithEvents btnLogs As Button
    Friend WithEvents pnlHeader As Panel
End Class
