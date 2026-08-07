<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmMainMenuV2
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
        Me.pnlHeader = New System.Windows.Forms.Panel()
        Me.lblBrand = New System.Windows.Forms.Label()
        Me.lblClock = New System.Windows.Forms.Label()
        Me.pnlSidebar = New System.Windows.Forms.Panel()
        Me.flpNav = New System.Windows.Forms.FlowLayoutPanel()
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
        Me.lblWelcome = New System.Windows.Forms.Label()
        Me.lblSubtitle = New System.Windows.Forms.Label()
        Me.pnlCard1 = New System.Windows.Forms.Panel()
        Me.lblCardTitle1 = New System.Windows.Forms.Label()
        Me.lblStat1 = New System.Windows.Forms.Label()
        Me.pnlCard2 = New System.Windows.Forms.Panel()
        Me.lblCardTitle2 = New System.Windows.Forms.Label()
        Me.lblStat2 = New System.Windows.Forms.Label()
        Me.pnlCard3 = New System.Windows.Forms.Panel()
        Me.lblCardTitle3 = New System.Windows.Forms.Label()
        Me.lblStat3 = New System.Windows.Forms.Label()
        Me.pnlCard4 = New System.Windows.Forms.Panel()
        Me.lblCardTitle4 = New System.Windows.Forms.Label()
        Me.lblStat4 = New System.Windows.Forms.Label()
        Me.lblWhatsOn = New System.Windows.Forms.Label()
        Me.lblTopFilm = New System.Windows.Forms.Label()
        Me.dgvWhatsOn = New System.Windows.Forms.DataGridView()
        Me.btnRefresh = New System.Windows.Forms.Button()
        Me.lblVersion = New System.Windows.Forms.Label()
        Me.timerClock = New System.Windows.Forms.Timer(Me.components)
        Me.pnlHeader.SuspendLayout()
        Me.pnlSidebar.SuspendLayout()
        Me.flpNav.SuspendLayout()
        Me.pnlCard1.SuspendLayout()
        Me.pnlCard2.SuspendLayout()
        Me.pnlCard3.SuspendLayout()
        Me.pnlCard4.SuspendLayout()
        CType(Me.dgvWhatsOn, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pnlHeader
        '
        Me.pnlHeader.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.pnlHeader.Controls.Add(Me.lblBrand)
        Me.pnlHeader.Controls.Add(Me.lblClock)
        Me.pnlHeader.Location = New System.Drawing.Point(0, 0)
        Me.pnlHeader.Name = "pnlHeader"
        Me.pnlHeader.Size = New System.Drawing.Size(1120, 64)
        Me.pnlHeader.TabIndex = 0
        '
        'lblBrand
        '
        Me.lblBrand.AutoSize = True
        Me.lblBrand.Font = New System.Drawing.Font("Segoe UI", 17.0!, System.Drawing.FontStyle.Bold)
        Me.lblBrand.ForeColor = System.Drawing.Color.White
        Me.lblBrand.Location = New System.Drawing.Point(22, 15)
        Me.lblBrand.Name = "lblBrand"
        Me.lblBrand.Size = New System.Drawing.Size(148, 31)
        Me.lblBrand.TabIndex = 0
        Me.lblBrand.Text = "FILMTOPIA"
        '
        'lblClock
        '
        Me.lblClock.AutoSize = True
        Me.lblClock.Font = New System.Drawing.Font("Segoe UI", 11.0!)
        Me.lblClock.ForeColor = System.Drawing.Color.FromArgb(CType(CType(231, Byte), Integer), CType(CType(169, Byte), Integer), CType(CType(231, Byte), Integer))
        Me.lblClock.Location = New System.Drawing.Point(940, 22)
        Me.lblClock.Name = "lblClock"
        Me.lblClock.Size = New System.Drawing.Size(60, 20)
        Me.lblClock.TabIndex = 1
        Me.lblClock.Text = "00:00:00"
        '
        'pnlSidebar
        '
        Me.pnlSidebar.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.pnlSidebar.Controls.Add(Me.flpNav)
        Me.pnlSidebar.Controls.Add(Me.btnLogout)
        Me.pnlSidebar.Location = New System.Drawing.Point(0, 64)
        Me.pnlSidebar.Name = "pnlSidebar"
        Me.pnlSidebar.Size = New System.Drawing.Size(210, 656)
        Me.pnlSidebar.TabIndex = 1
        '
        'flpNav
        '
        Me.flpNav.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.flpNav.Controls.Add(Me.btnBookings)
        Me.flpNav.Controls.Add(Me.btnFindBooking)
        Me.flpNav.Controls.Add(Me.btnScreenings)
        Me.flpNav.Controls.Add(Me.btnCustomers)
        Me.flpNav.Controls.Add(Me.btnFilms)
        Me.flpNav.Controls.Add(Me.btnScreens)
        Me.flpNav.Controls.Add(Me.btnFood)
        Me.flpNav.Controls.Add(Me.btnReports)
        Me.flpNav.Controls.Add(Me.btnLogs)
        Me.flpNav.Controls.Add(Me.btnSettings)
        Me.flpNav.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        Me.flpNav.Location = New System.Drawing.Point(0, 14)
        Me.flpNav.Name = "flpNav"
        Me.flpNav.Size = New System.Drawing.Size(210, 500)
        Me.flpNav.TabIndex = 0
        Me.flpNav.WrapContents = False
        '
        'btnBookings
        '
        Me.btnBookings.BackColor = System.Drawing.Color.Transparent
        Me.btnBookings.FlatAppearance.BorderSize = 0
        Me.btnBookings.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnBookings.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.btnBookings.ForeColor = System.Drawing.Color.White
        Me.btnBookings.Margin = New System.Windows.Forms.Padding(16, 0, 0, 6)
        Me.btnBookings.Name = "btnBookings"
        Me.btnBookings.Padding = New System.Windows.Forms.Padding(12, 0, 0, 0)
        Me.btnBookings.Size = New System.Drawing.Size(178, 40)
        Me.btnBookings.TabIndex = 0
        Me.btnBookings.Text = "Bookings"
        Me.btnBookings.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnBookings.UseVisualStyleBackColor = False
        '
        'btnFindBooking
        '
        Me.btnFindBooking.BackColor = System.Drawing.Color.Transparent
        Me.btnFindBooking.FlatAppearance.BorderSize = 0
        Me.btnFindBooking.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnFindBooking.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.btnFindBooking.ForeColor = System.Drawing.Color.White
        Me.btnFindBooking.Margin = New System.Windows.Forms.Padding(16, 0, 0, 6)
        Me.btnFindBooking.Name = "btnFindBooking"
        Me.btnFindBooking.Padding = New System.Windows.Forms.Padding(12, 0, 0, 0)
        Me.btnFindBooking.Size = New System.Drawing.Size(178, 40)
        Me.btnFindBooking.TabIndex = 1
        Me.btnFindBooking.Text = "Find Booking"
        Me.btnFindBooking.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnFindBooking.UseVisualStyleBackColor = False
        '
        'btnScreenings
        '
        Me.btnScreenings.BackColor = System.Drawing.Color.Transparent
        Me.btnScreenings.FlatAppearance.BorderSize = 0
        Me.btnScreenings.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnScreenings.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.btnScreenings.ForeColor = System.Drawing.Color.White
        Me.btnScreenings.Margin = New System.Windows.Forms.Padding(16, 0, 0, 6)
        Me.btnScreenings.Name = "btnScreenings"
        Me.btnScreenings.Padding = New System.Windows.Forms.Padding(12, 0, 0, 0)
        Me.btnScreenings.Size = New System.Drawing.Size(178, 40)
        Me.btnScreenings.TabIndex = 2
        Me.btnScreenings.Text = "Screenings"
        Me.btnScreenings.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnScreenings.UseVisualStyleBackColor = False
        '
        'btnCustomers
        '
        Me.btnCustomers.BackColor = System.Drawing.Color.Transparent
        Me.btnCustomers.FlatAppearance.BorderSize = 0
        Me.btnCustomers.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCustomers.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.btnCustomers.ForeColor = System.Drawing.Color.White
        Me.btnCustomers.Margin = New System.Windows.Forms.Padding(16, 0, 0, 6)
        Me.btnCustomers.Name = "btnCustomers"
        Me.btnCustomers.Padding = New System.Windows.Forms.Padding(12, 0, 0, 0)
        Me.btnCustomers.Size = New System.Drawing.Size(178, 40)
        Me.btnCustomers.TabIndex = 3
        Me.btnCustomers.Text = "Customers"
        Me.btnCustomers.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCustomers.UseVisualStyleBackColor = False
        '
        'btnFilms
        '
        Me.btnFilms.BackColor = System.Drawing.Color.Transparent
        Me.btnFilms.FlatAppearance.BorderSize = 0
        Me.btnFilms.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnFilms.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.btnFilms.ForeColor = System.Drawing.Color.White
        Me.btnFilms.Margin = New System.Windows.Forms.Padding(16, 0, 0, 6)
        Me.btnFilms.Name = "btnFilms"
        Me.btnFilms.Padding = New System.Windows.Forms.Padding(12, 0, 0, 0)
        Me.btnFilms.Size = New System.Drawing.Size(178, 40)
        Me.btnFilms.TabIndex = 4
        Me.btnFilms.Text = "Films"
        Me.btnFilms.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnFilms.UseVisualStyleBackColor = False
        '
        'btnScreens
        '
        Me.btnScreens.BackColor = System.Drawing.Color.Transparent
        Me.btnScreens.FlatAppearance.BorderSize = 0
        Me.btnScreens.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnScreens.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.btnScreens.ForeColor = System.Drawing.Color.White
        Me.btnScreens.Margin = New System.Windows.Forms.Padding(16, 0, 0, 6)
        Me.btnScreens.Name = "btnScreens"
        Me.btnScreens.Padding = New System.Windows.Forms.Padding(12, 0, 0, 0)
        Me.btnScreens.Size = New System.Drawing.Size(178, 40)
        Me.btnScreens.TabIndex = 5
        Me.btnScreens.Text = "Screens"
        Me.btnScreens.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnScreens.UseVisualStyleBackColor = False
        '
        'btnFood
        '
        Me.btnFood.BackColor = System.Drawing.Color.Transparent
        Me.btnFood.FlatAppearance.BorderSize = 0
        Me.btnFood.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnFood.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.btnFood.ForeColor = System.Drawing.Color.White
        Me.btnFood.Margin = New System.Windows.Forms.Padding(16, 0, 0, 6)
        Me.btnFood.Name = "btnFood"
        Me.btnFood.Padding = New System.Windows.Forms.Padding(12, 0, 0, 0)
        Me.btnFood.Size = New System.Drawing.Size(178, 40)
        Me.btnFood.TabIndex = 6
        Me.btnFood.Text = "Food and Drink"
        Me.btnFood.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnFood.UseVisualStyleBackColor = False
        '
        'btnReports
        '
        Me.btnReports.BackColor = System.Drawing.Color.Transparent
        Me.btnReports.FlatAppearance.BorderSize = 0
        Me.btnReports.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnReports.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.btnReports.ForeColor = System.Drawing.Color.White
        Me.btnReports.Margin = New System.Windows.Forms.Padding(16, 0, 0, 6)
        Me.btnReports.Name = "btnReports"
        Me.btnReports.Padding = New System.Windows.Forms.Padding(12, 0, 0, 0)
        Me.btnReports.Size = New System.Drawing.Size(178, 40)
        Me.btnReports.TabIndex = 7
        Me.btnReports.Text = "Sales Report"
        Me.btnReports.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnReports.UseVisualStyleBackColor = False
        '
        'btnLogs
        '
        Me.btnLogs.BackColor = System.Drawing.Color.Transparent
        Me.btnLogs.FlatAppearance.BorderSize = 0
        Me.btnLogs.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnLogs.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.btnLogs.ForeColor = System.Drawing.Color.White
        Me.btnLogs.Margin = New System.Windows.Forms.Padding(16, 0, 0, 6)
        Me.btnLogs.Name = "btnLogs"
        Me.btnLogs.Padding = New System.Windows.Forms.Padding(12, 0, 0, 0)
        Me.btnLogs.Size = New System.Drawing.Size(178, 40)
        Me.btnLogs.TabIndex = 8
        Me.btnLogs.Text = "Logs"
        Me.btnLogs.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnLogs.UseVisualStyleBackColor = False
        '
        'btnSettings
        '
        Me.btnSettings.BackColor = System.Drawing.Color.Transparent
        Me.btnSettings.FlatAppearance.BorderSize = 0
        Me.btnSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSettings.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.btnSettings.ForeColor = System.Drawing.Color.White
        Me.btnSettings.Margin = New System.Windows.Forms.Padding(16, 0, 0, 6)
        Me.btnSettings.Name = "btnSettings"
        Me.btnSettings.Padding = New System.Windows.Forms.Padding(12, 0, 0, 0)
        Me.btnSettings.Size = New System.Drawing.Size(178, 40)
        Me.btnSettings.TabIndex = 9
        Me.btnSettings.Text = "Settings"
        Me.btnSettings.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSettings.UseVisualStyleBackColor = False
        '
        'btnLogout
        '
        Me.btnLogout.BackColor = System.Drawing.Color.FromArgb(CType(CType(216, Byte), Integer), CType(CType(27, Byte), Integer), CType(CType(96, Byte), Integer))
        Me.btnLogout.FlatAppearance.BorderSize = 0
        Me.btnLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnLogout.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me.btnLogout.ForeColor = System.Drawing.Color.White
        Me.btnLogout.Location = New System.Drawing.Point(16, 594)
        Me.btnLogout.Name = "btnLogout"
        Me.btnLogout.Size = New System.Drawing.Size(178, 40)
        Me.btnLogout.TabIndex = 10
        Me.btnLogout.Text = "Log out"
        Me.btnLogout.UseVisualStyleBackColor = False
        '
        'lblWelcome
        '
        Me.lblWelcome.AutoSize = True
        Me.lblWelcome.Font = New System.Drawing.Font("Segoe UI", 19.0!, System.Drawing.FontStyle.Bold)
        Me.lblWelcome.Location = New System.Drawing.Point(236, 84)
        Me.lblWelcome.Name = "lblWelcome"
        Me.lblWelcome.Size = New System.Drawing.Size(260, 34)
        Me.lblWelcome.TabIndex = 2
        Me.lblWelcome.Text = "Good evening, Admin"
        '
        'lblSubtitle
        '
        Me.lblSubtitle.AutoSize = True
        Me.lblSubtitle.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.lblSubtitle.Location = New System.Drawing.Point(238, 122)
        Me.lblSubtitle.Name = "lblSubtitle"
        Me.lblSubtitle.Size = New System.Drawing.Size(200, 17)
        Me.lblSubtitle.TabIndex = 3
        Me.lblSubtitle.Text = "Here is how the cinema is doing."
        '
        'pnlCard1
        '
        Me.pnlCard1.Controls.Add(Me.lblCardTitle1)
        Me.pnlCard1.Controls.Add(Me.lblStat1)
        Me.pnlCard1.Location = New System.Drawing.Point(240, 156)
        Me.pnlCard1.Name = "pnlCard1"
        Me.pnlCard1.Size = New System.Drawing.Size(205, 96)
        Me.pnlCard1.TabIndex = 4
        '
        'lblCardTitle1
        '
        Me.lblCardTitle1.AutoSize = True
        Me.lblCardTitle1.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.lblCardTitle1.Location = New System.Drawing.Point(16, 14)
        Me.lblCardTitle1.Name = "lblCardTitle1"
        Me.lblCardTitle1.Size = New System.Drawing.Size(100, 17)
        Me.lblCardTitle1.TabIndex = 0
        Me.lblCardTitle1.Text = "Films"
        '
        'lblStat1
        '
        Me.lblStat1.AutoSize = True
        Me.lblStat1.Font = New System.Drawing.Font("Segoe UI", 24.0!, System.Drawing.FontStyle.Bold)
        Me.lblStat1.Location = New System.Drawing.Point(13, 36)
        Me.lblStat1.Name = "lblStat1"
        Me.lblStat1.Size = New System.Drawing.Size(50, 45)
        Me.lblStat1.TabIndex = 1
        Me.lblStat1.Text = "0"
        '
        'pnlCard2
        '
        Me.pnlCard2.Controls.Add(Me.lblCardTitle2)
        Me.pnlCard2.Controls.Add(Me.lblStat2)
        Me.pnlCard2.Location = New System.Drawing.Point(458, 156)
        Me.pnlCard2.Name = "pnlCard2"
        Me.pnlCard2.Size = New System.Drawing.Size(205, 96)
        Me.pnlCard2.TabIndex = 5
        '
        'lblCardTitle2
        '
        Me.lblCardTitle2.AutoSize = True
        Me.lblCardTitle2.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.lblCardTitle2.Location = New System.Drawing.Point(16, 14)
        Me.lblCardTitle2.Name = "lblCardTitle2"
        Me.lblCardTitle2.Size = New System.Drawing.Size(100, 17)
        Me.lblCardTitle2.TabIndex = 0
        Me.lblCardTitle2.Text = "Screenings"
        '
        'lblStat2
        '
        Me.lblStat2.AutoSize = True
        Me.lblStat2.Font = New System.Drawing.Font("Segoe UI", 24.0!, System.Drawing.FontStyle.Bold)
        Me.lblStat2.Location = New System.Drawing.Point(13, 36)
        Me.lblStat2.Name = "lblStat2"
        Me.lblStat2.Size = New System.Drawing.Size(50, 45)
        Me.lblStat2.TabIndex = 1
        Me.lblStat2.Text = "0"
        '
        'pnlCard3
        '
        Me.pnlCard3.Controls.Add(Me.lblCardTitle3)
        Me.pnlCard3.Controls.Add(Me.lblStat3)
        Me.pnlCard3.Location = New System.Drawing.Point(676, 156)
        Me.pnlCard3.Name = "pnlCard3"
        Me.pnlCard3.Size = New System.Drawing.Size(205, 96)
        Me.pnlCard3.TabIndex = 6
        '
        'lblCardTitle3
        '
        Me.lblCardTitle3.AutoSize = True
        Me.lblCardTitle3.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.lblCardTitle3.Location = New System.Drawing.Point(16, 14)
        Me.lblCardTitle3.Name = "lblCardTitle3"
        Me.lblCardTitle3.Size = New System.Drawing.Size(100, 17)
        Me.lblCardTitle3.TabIndex = 0
        Me.lblCardTitle3.Text = "Bookings taken"
        '
        'lblStat3
        '
        Me.lblStat3.AutoSize = True
        Me.lblStat3.Font = New System.Drawing.Font("Segoe UI", 24.0!, System.Drawing.FontStyle.Bold)
        Me.lblStat3.Location = New System.Drawing.Point(13, 36)
        Me.lblStat3.Name = "lblStat3"
        Me.lblStat3.Size = New System.Drawing.Size(50, 45)
        Me.lblStat3.TabIndex = 1
        Me.lblStat3.Text = "0"
        '
        'pnlCard4
        '
        Me.pnlCard4.Controls.Add(Me.lblCardTitle4)
        Me.pnlCard4.Controls.Add(Me.lblStat4)
        Me.pnlCard4.Location = New System.Drawing.Point(894, 156)
        Me.pnlCard4.Name = "pnlCard4"
        Me.pnlCard4.Size = New System.Drawing.Size(205, 96)
        Me.pnlCard4.TabIndex = 7
        '
        'lblCardTitle4
        '
        Me.lblCardTitle4.AutoSize = True
        Me.lblCardTitle4.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.lblCardTitle4.Location = New System.Drawing.Point(16, 14)
        Me.lblCardTitle4.Name = "lblCardTitle4"
        Me.lblCardTitle4.Size = New System.Drawing.Size(100, 17)
        Me.lblCardTitle4.TabIndex = 0
        Me.lblCardTitle4.Text = "Money taken"
        '
        'lblStat4
        '
        Me.lblStat4.AutoSize = True
        Me.lblStat4.Font = New System.Drawing.Font("Segoe UI", 24.0!, System.Drawing.FontStyle.Bold)
        Me.lblStat4.Location = New System.Drawing.Point(13, 36)
        Me.lblStat4.Name = "lblStat4"
        Me.lblStat4.Size = New System.Drawing.Size(50, 45)
        Me.lblStat4.TabIndex = 1
        Me.lblStat4.Text = "0"
        '
        'lblWhatsOn
        '
        Me.lblWhatsOn.AutoSize = True
        Me.lblWhatsOn.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold)
        Me.lblWhatsOn.Location = New System.Drawing.Point(238, 272)
        Me.lblWhatsOn.Name = "lblWhatsOn"
        Me.lblWhatsOn.Size = New System.Drawing.Size(180, 21)
        Me.lblWhatsOn.TabIndex = 8
        Me.lblWhatsOn.Text = "What is on and how full it is"
        '
        'lblTopFilm
        '
        Me.lblTopFilm.AutoSize = True
        Me.lblTopFilm.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Italic)
        Me.lblTopFilm.Location = New System.Drawing.Point(390, 276)
        Me.lblTopFilm.Name = "lblTopFilm"
        Me.lblTopFilm.Size = New System.Drawing.Size(220, 17)
        Me.lblTopFilm.TabIndex = 12
        Me.lblTopFilm.Text = "Most popular right now:"
        Me.lblTopFilm.Visible = False
        '
        'dgvWhatsOn
        '
        Me.dgvWhatsOn.AllowUserToAddRows = False
        Me.dgvWhatsOn.AllowUserToDeleteRows = False
        Me.dgvWhatsOn.AllowUserToResizeRows = False
        Me.dgvWhatsOn.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dgvWhatsOn.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvWhatsOn.Location = New System.Drawing.Point(240, 300)
        Me.dgvWhatsOn.Name = "dgvWhatsOn"
        Me.dgvWhatsOn.ReadOnly = True
        Me.dgvWhatsOn.RowHeadersVisible = False
        Me.dgvWhatsOn.RowTemplate.Height = 30
        Me.dgvWhatsOn.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvWhatsOn.Size = New System.Drawing.Size(859, 320)
        Me.dgvWhatsOn.TabIndex = 9
        '
        'btnRefresh
        '
        Me.btnRefresh.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnRefresh.Location = New System.Drawing.Point(1005, 268)
        Me.btnRefresh.Name = "btnRefresh"
        Me.btnRefresh.Size = New System.Drawing.Size(94, 28)
        Me.btnRefresh.TabIndex = 10
        Me.btnRefresh.Text = "Refresh"
        Me.btnRefresh.UseVisualStyleBackColor = True
        '
        'lblVersion
        '
        Me.lblVersion.AutoSize = True
        Me.lblVersion.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.lblVersion.Location = New System.Drawing.Point(238, 632)
        Me.lblVersion.Name = "lblVersion"
        Me.lblVersion.Size = New System.Drawing.Size(220, 13)
        Me.lblVersion.TabIndex = 11
        Me.lblVersion.Text = "Filmtopia Cinema Management System  v1.0"
        '
        'timerClock
        '
        Me.timerClock.Interval = 1000
        '
        'frmMainMenuV2
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1120, 720)
        Me.Controls.Add(Me.lblVersion)
        Me.Controls.Add(Me.btnRefresh)
        Me.Controls.Add(Me.dgvWhatsOn)
        Me.Controls.Add(Me.lblTopFilm)
        Me.Controls.Add(Me.lblWhatsOn)
        Me.Controls.Add(Me.pnlCard4)
        Me.Controls.Add(Me.pnlCard3)
        Me.Controls.Add(Me.pnlCard2)
        Me.Controls.Add(Me.pnlCard1)
        Me.Controls.Add(Me.lblSubtitle)
        Me.Controls.Add(Me.lblWelcome)
        Me.Controls.Add(Me.pnlSidebar)
        Me.Controls.Add(Me.pnlHeader)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "frmMainMenuV2"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Filmtopia"
        Me.pnlHeader.ResumeLayout(False)
        Me.pnlHeader.PerformLayout()
        Me.pnlSidebar.ResumeLayout(False)
        Me.flpNav.ResumeLayout(False)
        Me.pnlCard1.ResumeLayout(False)
        Me.pnlCard1.PerformLayout()
        Me.pnlCard2.ResumeLayout(False)
        Me.pnlCard2.PerformLayout()
        Me.pnlCard3.ResumeLayout(False)
        Me.pnlCard3.PerformLayout()
        Me.pnlCard4.ResumeLayout(False)
        Me.pnlCard4.PerformLayout()
        CType(Me.dgvWhatsOn, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents pnlHeader As Panel
    Friend WithEvents lblBrand As Label
    Friend WithEvents lblClock As Label
    Friend WithEvents pnlSidebar As Panel
    Friend WithEvents flpNav As FlowLayoutPanel
    Friend WithEvents btnBookings As Button
    Friend WithEvents btnFindBooking As Button
    Friend WithEvents btnScreenings As Button
    Friend WithEvents btnCustomers As Button
    Friend WithEvents btnFilms As Button
    Friend WithEvents btnScreens As Button
    Friend WithEvents btnFood As Button
    Friend WithEvents btnReports As Button
    Friend WithEvents btnLogs As Button
    Friend WithEvents btnSettings As Button
    Friend WithEvents btnLogout As Button
    Friend WithEvents lblWelcome As Label
    Friend WithEvents lblSubtitle As Label
    Friend WithEvents pnlCard1 As Panel
    Friend WithEvents lblCardTitle1 As Label
    Friend WithEvents lblStat1 As Label
    Friend WithEvents pnlCard2 As Panel
    Friend WithEvents lblCardTitle2 As Label
    Friend WithEvents lblStat2 As Label
    Friend WithEvents pnlCard3 As Panel
    Friend WithEvents lblCardTitle3 As Label
    Friend WithEvents lblStat3 As Label
    Friend WithEvents pnlCard4 As Panel
    Friend WithEvents lblCardTitle4 As Label
    Friend WithEvents lblStat4 As Label
    Friend WithEvents lblWhatsOn As Label
    Friend WithEvents lblTopFilm As Label
    Friend WithEvents dgvWhatsOn As DataGridView
    Friend WithEvents btnRefresh As Button
    Friend WithEvents lblVersion As Label
    Friend WithEvents timerClock As Timer
End Class
