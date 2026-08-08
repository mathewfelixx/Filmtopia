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
        Me.lblNavFront = New System.Windows.Forms.Label()
        Me.btnBookings = New System.Windows.Forms.Button()
        Me.btnFindBooking = New System.Windows.Forms.Button()
        Me.btnScreenings = New System.Windows.Forms.Button()
        Me.btnCustomers = New System.Windows.Forms.Button()
        Me.lblNavManage = New System.Windows.Forms.Label()
        Me.btnFilms = New System.Windows.Forms.Button()
        Me.btnScreens = New System.Windows.Forms.Button()
        Me.btnFood = New System.Windows.Forms.Button()
        Me.btnReports = New System.Windows.Forms.Button()
        Me.btnLogs = New System.Windows.Forms.Button()
        Me.lblNavSystem = New System.Windows.Forms.Label()
        Me.btnSettings = New System.Windows.Forms.Button()
        Me.btnLogout = New System.Windows.Forms.Button()
        Me.lblWelcome = New System.Windows.Forms.Label()
        Me.lblSubtitle = New System.Windows.Forms.Label()
        Me.pnlCard1 = New System.Windows.Forms.Panel()
        Me.pnlAccent1 = New System.Windows.Forms.Panel()
        Me.lblCardTitle1 = New System.Windows.Forms.Label()
        Me.lblStat1 = New System.Windows.Forms.Label()
        Me.lblCardSub1 = New System.Windows.Forms.Label()
        Me.pnlCard2 = New System.Windows.Forms.Panel()
        Me.pnlAccent2 = New System.Windows.Forms.Panel()
        Me.lblCardTitle2 = New System.Windows.Forms.Label()
        Me.lblStat2 = New System.Windows.Forms.Label()
        Me.lblCardSub2 = New System.Windows.Forms.Label()
        Me.pnlCard3 = New System.Windows.Forms.Panel()
        Me.pnlAccent3 = New System.Windows.Forms.Panel()
        Me.lblCardTitle3 = New System.Windows.Forms.Label()
        Me.lblStat3 = New System.Windows.Forms.Label()
        Me.lblCardSub3 = New System.Windows.Forms.Label()
        Me.pnlCard4 = New System.Windows.Forms.Panel()
        Me.pnlAccent4 = New System.Windows.Forms.Panel()
        Me.lblCardTitle4 = New System.Windows.Forms.Label()
        Me.lblStat4 = New System.Windows.Forms.Label()
        Me.lblCardSub4 = New System.Windows.Forms.Label()
        Me.lblAlerts = New System.Windows.Forms.Label()
        Me.lblTopFilm = New System.Windows.Forms.Label()
        Me.lblWhatsOn = New System.Windows.Forms.Label()
        Me.lblSearch = New System.Windows.Forms.Label()
        Me.txtSearch = New System.Windows.Forms.TextBox()
        Me.cboShow = New System.Windows.Forms.ComboBox()
        Me.btnRefresh = New System.Windows.Forms.Button()
        Me.lblNoRows = New System.Windows.Forms.Label()
        Me.dgvWhatsOn = New System.Windows.Forms.DataGridView()
        Me.lblGridCount = New System.Windows.Forms.Label()
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
        Me.pnlHeader.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right)), System.Windows.Forms.AnchorStyles)
        Me.pnlHeader.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.pnlHeader.Controls.Add(Me.lblBrand)
        Me.pnlHeader.Controls.Add(Me.lblClock)
        Me.pnlHeader.Location = New System.Drawing.Point(0, 0)
        Me.pnlHeader.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.pnlHeader.Name = "pnlHeader"
        Me.pnlHeader.Size = New System.Drawing.Size(1493, 79)
        Me.pnlHeader.TabIndex = 0
        '
        'lblBrand
        '
        Me.lblBrand.AutoSize = True
        Me.lblBrand.Font = New System.Drawing.Font("Segoe UI", 17.0!, System.Drawing.FontStyle.Bold)
        Me.lblBrand.ForeColor = System.Drawing.Color.White
        Me.lblBrand.Location = New System.Drawing.Point(29, 18)
        Me.lblBrand.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblBrand.Name = "lblBrand"
        Me.lblBrand.Size = New System.Drawing.Size(169, 40)
        Me.lblBrand.TabIndex = 0
        Me.lblBrand.Text = "FILMTOPIA"
        '
        'lblClock
        '
        Me.lblClock.AutoSize = True
        Me.lblClock.Font = New System.Drawing.Font("Segoe UI", 11.0!)
        Me.lblClock.ForeColor = System.Drawing.Color.FromArgb(CType(CType(231, Byte), Integer), CType(CType(169, Byte), Integer), CType(CType(231, Byte), Integer))
        Me.lblClock.Location = New System.Drawing.Point(1253, 27)
        Me.lblClock.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblClock.Name = "lblClock"
        Me.lblClock.Size = New System.Drawing.Size(80, 25)
        Me.lblClock.TabIndex = 1
        Me.lblClock.Text = "00:00:00"
        '
        'pnlSidebar
        '
        Me.pnlSidebar.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left)), System.Windows.Forms.AnchorStyles)
        Me.pnlSidebar.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.pnlSidebar.Controls.Add(Me.flpNav)
        Me.pnlSidebar.Controls.Add(Me.btnLogout)
        Me.pnlSidebar.Location = New System.Drawing.Point(0, 79)
        Me.pnlSidebar.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.pnlSidebar.Name = "pnlSidebar"
        Me.pnlSidebar.Size = New System.Drawing.Size(280, 807)
        Me.pnlSidebar.TabIndex = 1
        '
        'flpNav
        '
        Me.flpNav.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.flpNav.Controls.Add(Me.lblNavFront)
        Me.flpNav.Controls.Add(Me.btnBookings)
        Me.flpNav.Controls.Add(Me.btnFindBooking)
        Me.flpNav.Controls.Add(Me.btnScreenings)
        Me.flpNav.Controls.Add(Me.btnCustomers)
        Me.flpNav.Controls.Add(Me.lblNavManage)
        Me.flpNav.Controls.Add(Me.btnFilms)
        Me.flpNav.Controls.Add(Me.btnScreens)
        Me.flpNav.Controls.Add(Me.btnFood)
        Me.flpNav.Controls.Add(Me.btnReports)
        Me.flpNav.Controls.Add(Me.btnLogs)
        Me.flpNav.Controls.Add(Me.lblNavSystem)
        Me.flpNav.Controls.Add(Me.btnSettings)
        Me.flpNav.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        Me.flpNav.Location = New System.Drawing.Point(0, 14)
        Me.flpNav.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.flpNav.Name = "flpNav"
        Me.flpNav.Size = New System.Drawing.Size(280, 700)
        Me.flpNav.TabIndex = 0
        Me.flpNav.WrapContents = False
        '
        'lblNavFront
        '
        Me.lblNavFront.AutoSize = True
        Me.lblNavFront.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold)
        Me.lblNavFront.ForeColor = System.Drawing.Color.FromArgb(CType(CType(196, Byte), Integer), CType(CType(140, Byte), Integer), CType(CType(196, Byte), Integer))
        Me.lblNavFront.Location = New System.Drawing.Point(37, 8)
        Me.lblNavFront.Margin = New System.Windows.Forms.Padding(37, 8, 0, 6)
        Me.lblNavFront.Name = "lblNavFront"
        Me.lblNavFront.Size = New System.Drawing.Size(112, 19)
        Me.lblNavFront.TabIndex = 0
        Me.lblNavFront.Text = "FRONT OF HOUSE"
        '
        'btnBookings
        '
        Me.btnBookings.BackColor = System.Drawing.Color.Transparent
        Me.btnBookings.FlatAppearance.BorderSize = 0
        Me.btnBookings.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnBookings.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.btnBookings.ForeColor = System.Drawing.Color.White
        Me.btnBookings.Location = New System.Drawing.Point(21, 33)
        Me.btnBookings.Margin = New System.Windows.Forms.Padding(21, 0, 0, 7)
        Me.btnBookings.Name = "btnBookings"
        Me.btnBookings.Padding = New System.Windows.Forms.Padding(16, 0, 0, 0)
        Me.btnBookings.Size = New System.Drawing.Size(237, 49)
        Me.btnBookings.TabIndex = 1
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
        Me.btnFindBooking.Location = New System.Drawing.Point(21, 89)
        Me.btnFindBooking.Margin = New System.Windows.Forms.Padding(21, 0, 0, 7)
        Me.btnFindBooking.Name = "btnFindBooking"
        Me.btnFindBooking.Padding = New System.Windows.Forms.Padding(16, 0, 0, 0)
        Me.btnFindBooking.Size = New System.Drawing.Size(237, 49)
        Me.btnFindBooking.TabIndex = 2
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
        Me.btnScreenings.Location = New System.Drawing.Point(21, 145)
        Me.btnScreenings.Margin = New System.Windows.Forms.Padding(21, 0, 0, 7)
        Me.btnScreenings.Name = "btnScreenings"
        Me.btnScreenings.Padding = New System.Windows.Forms.Padding(16, 0, 0, 0)
        Me.btnScreenings.Size = New System.Drawing.Size(237, 49)
        Me.btnScreenings.TabIndex = 3
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
        Me.btnCustomers.Location = New System.Drawing.Point(21, 201)
        Me.btnCustomers.Margin = New System.Windows.Forms.Padding(21, 0, 0, 7)
        Me.btnCustomers.Name = "btnCustomers"
        Me.btnCustomers.Padding = New System.Windows.Forms.Padding(16, 0, 0, 0)
        Me.btnCustomers.Size = New System.Drawing.Size(237, 49)
        Me.btnCustomers.TabIndex = 4
        Me.btnCustomers.Text = "Customers"
        Me.btnCustomers.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCustomers.UseVisualStyleBackColor = False
        '
        'lblNavManage
        '
        Me.lblNavManage.AutoSize = True
        Me.lblNavManage.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold)
        Me.lblNavManage.ForeColor = System.Drawing.Color.FromArgb(CType(CType(196, Byte), Integer), CType(CType(140, Byte), Integer), CType(CType(196, Byte), Integer))
        Me.lblNavManage.Location = New System.Drawing.Point(37, 265)
        Me.lblNavManage.Margin = New System.Windows.Forms.Padding(37, 8, 0, 6)
        Me.lblNavManage.Name = "lblNavManage"
        Me.lblNavManage.Size = New System.Drawing.Size(93, 19)
        Me.lblNavManage.TabIndex = 5
        Me.lblNavManage.Text = "MANAGEMENT"
        '
        'btnFilms
        '
        Me.btnFilms.BackColor = System.Drawing.Color.Transparent
        Me.btnFilms.FlatAppearance.BorderSize = 0
        Me.btnFilms.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnFilms.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.btnFilms.ForeColor = System.Drawing.Color.White
        Me.btnFilms.Location = New System.Drawing.Point(21, 290)
        Me.btnFilms.Margin = New System.Windows.Forms.Padding(21, 0, 0, 7)
        Me.btnFilms.Name = "btnFilms"
        Me.btnFilms.Padding = New System.Windows.Forms.Padding(16, 0, 0, 0)
        Me.btnFilms.Size = New System.Drawing.Size(237, 49)
        Me.btnFilms.TabIndex = 6
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
        Me.btnScreens.Location = New System.Drawing.Point(21, 346)
        Me.btnScreens.Margin = New System.Windows.Forms.Padding(21, 0, 0, 7)
        Me.btnScreens.Name = "btnScreens"
        Me.btnScreens.Padding = New System.Windows.Forms.Padding(16, 0, 0, 0)
        Me.btnScreens.Size = New System.Drawing.Size(237, 49)
        Me.btnScreens.TabIndex = 7
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
        Me.btnFood.Location = New System.Drawing.Point(21, 402)
        Me.btnFood.Margin = New System.Windows.Forms.Padding(21, 0, 0, 7)
        Me.btnFood.Name = "btnFood"
        Me.btnFood.Padding = New System.Windows.Forms.Padding(16, 0, 0, 0)
        Me.btnFood.Size = New System.Drawing.Size(237, 49)
        Me.btnFood.TabIndex = 8
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
        Me.btnReports.Location = New System.Drawing.Point(21, 458)
        Me.btnReports.Margin = New System.Windows.Forms.Padding(21, 0, 0, 7)
        Me.btnReports.Name = "btnReports"
        Me.btnReports.Padding = New System.Windows.Forms.Padding(16, 0, 0, 0)
        Me.btnReports.Size = New System.Drawing.Size(237, 49)
        Me.btnReports.TabIndex = 9
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
        Me.btnLogs.Location = New System.Drawing.Point(21, 514)
        Me.btnLogs.Margin = New System.Windows.Forms.Padding(21, 0, 0, 7)
        Me.btnLogs.Name = "btnLogs"
        Me.btnLogs.Padding = New System.Windows.Forms.Padding(16, 0, 0, 0)
        Me.btnLogs.Size = New System.Drawing.Size(237, 49)
        Me.btnLogs.TabIndex = 10
        Me.btnLogs.Text = "Logs"
        Me.btnLogs.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnLogs.UseVisualStyleBackColor = False
        '
        'lblNavSystem
        '
        Me.lblNavSystem.AutoSize = True
        Me.lblNavSystem.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold)
        Me.lblNavSystem.ForeColor = System.Drawing.Color.FromArgb(CType(CType(196, Byte), Integer), CType(CType(140, Byte), Integer), CType(CType(196, Byte), Integer))
        Me.lblNavSystem.Location = New System.Drawing.Point(37, 578)
        Me.lblNavSystem.Margin = New System.Windows.Forms.Padding(37, 8, 0, 6)
        Me.lblNavSystem.Name = "lblNavSystem"
        Me.lblNavSystem.Size = New System.Drawing.Size(56, 19)
        Me.lblNavSystem.TabIndex = 11
        Me.lblNavSystem.Text = "SYSTEM"
        '
        'btnSettings
        '
        Me.btnSettings.BackColor = System.Drawing.Color.Transparent
        Me.btnSettings.FlatAppearance.BorderSize = 0
        Me.btnSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSettings.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.btnSettings.ForeColor = System.Drawing.Color.White
        Me.btnSettings.Location = New System.Drawing.Point(21, 603)
        Me.btnSettings.Margin = New System.Windows.Forms.Padding(21, 0, 0, 7)
        Me.btnSettings.Name = "btnSettings"
        Me.btnSettings.Padding = New System.Windows.Forms.Padding(16, 0, 0, 0)
        Me.btnSettings.Size = New System.Drawing.Size(237, 49)
        Me.btnSettings.TabIndex = 12
        Me.btnSettings.Text = "Settings"
        Me.btnSettings.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSettings.UseVisualStyleBackColor = False
        '
        'btnLogout
        '
        Me.btnLogout.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left)), System.Windows.Forms.AnchorStyles)
        Me.btnLogout.BackColor = System.Drawing.Color.FromArgb(CType(CType(216, Byte), Integer), CType(CType(27, Byte), Integer), CType(CType(96, Byte), Integer))
        Me.btnLogout.FlatAppearance.BorderSize = 0
        Me.btnLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnLogout.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me.btnLogout.ForeColor = System.Drawing.Color.White
        Me.btnLogout.Location = New System.Drawing.Point(21, 731)
        Me.btnLogout.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnLogout.Name = "btnLogout"
        Me.btnLogout.Size = New System.Drawing.Size(237, 49)
        Me.btnLogout.TabIndex = 13
        Me.btnLogout.Text = "Log out"
        Me.btnLogout.UseVisualStyleBackColor = False
        '
        'lblWelcome
        '
        Me.lblWelcome.AutoSize = True
        Me.lblWelcome.Font = New System.Drawing.Font("Segoe UI", 19.0!, System.Drawing.FontStyle.Bold)
        Me.lblWelcome.Location = New System.Drawing.Point(315, 98)
        Me.lblWelcome.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblWelcome.Name = "lblWelcome"
        Me.lblWelcome.Size = New System.Drawing.Size(348, 45)
        Me.lblWelcome.TabIndex = 2
        Me.lblWelcome.Text = "Good evening, Admin"
        '
        'lblSubtitle
        '
        Me.lblSubtitle.AutoSize = True
        Me.lblSubtitle.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.lblSubtitle.Location = New System.Drawing.Point(317, 145)
        Me.lblSubtitle.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblSubtitle.Name = "lblSubtitle"
        Me.lblSubtitle.Size = New System.Drawing.Size(258, 23)
        Me.lblSubtitle.TabIndex = 3
        Me.lblSubtitle.Text = "Here is how the cinema is doing."
        '
        'pnlCard1
        '
        Me.pnlCard1.Controls.Add(Me.lblCardTitle1)
        Me.pnlCard1.Controls.Add(Me.lblStat1)
        Me.pnlCard1.Controls.Add(Me.lblCardSub1)
        Me.pnlCard1.Controls.Add(Me.pnlAccent1)
        Me.pnlCard1.Location = New System.Drawing.Point(320, 183)
        Me.pnlCard1.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.pnlCard1.Name = "pnlCard1"
        Me.pnlCard1.Size = New System.Drawing.Size(273, 126)
        Me.pnlCard1.TabIndex = 4
        '
        'pnlAccent1
        '
        Me.pnlAccent1.BackColor = System.Drawing.Color.FromArgb(CType(CType(216, Byte), Integer), CType(CType(27, Byte), Integer), CType(CType(96, Byte), Integer))
        Me.pnlAccent1.Dock = System.Windows.Forms.DockStyle.Left
        Me.pnlAccent1.Location = New System.Drawing.Point(0, 0)
        Me.pnlAccent1.Name = "pnlAccent1"
        Me.pnlAccent1.Size = New System.Drawing.Size(7, 126)
        Me.pnlAccent1.TabIndex = 3
        '
        'lblCardTitle1
        '
        Me.lblCardTitle1.AutoSize = True
        Me.lblCardTitle1.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.lblCardTitle1.Location = New System.Drawing.Point(25, 14)
        Me.lblCardTitle1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblCardTitle1.Name = "lblCardTitle1"
        Me.lblCardTitle1.Size = New System.Drawing.Size(48, 23)
        Me.lblCardTitle1.TabIndex = 0
        Me.lblCardTitle1.Text = "Films"
        '
        'lblStat1
        '
        Me.lblStat1.AutoSize = True
        Me.lblStat1.Font = New System.Drawing.Font("Segoe UI", 24.0!, System.Drawing.FontStyle.Bold)
        Me.lblStat1.Location = New System.Drawing.Point(21, 38)
        Me.lblStat1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblStat1.Name = "lblStat1"
        Me.lblStat1.Size = New System.Drawing.Size(46, 54)
        Me.lblStat1.TabIndex = 1
        Me.lblStat1.Text = "0"
        '
        'lblCardSub1
        '
        Me.lblCardSub1.AutoSize = True
        Me.lblCardSub1.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.lblCardSub1.Location = New System.Drawing.Point(25, 95)
        Me.lblCardSub1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblCardSub1.Name = "lblCardSub1"
        Me.lblCardSub1.Size = New System.Drawing.Size(10, 19)
        Me.lblCardSub1.TabIndex = 2
        Me.lblCardSub1.Text = ""
        '
        'pnlCard2
        '
        Me.pnlCard2.Controls.Add(Me.lblCardTitle2)
        Me.pnlCard2.Controls.Add(Me.lblStat2)
        Me.pnlCard2.Controls.Add(Me.lblCardSub2)
        Me.pnlCard2.Controls.Add(Me.pnlAccent2)
        Me.pnlCard2.Location = New System.Drawing.Point(610, 183)
        Me.pnlCard2.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.pnlCard2.Name = "pnlCard2"
        Me.pnlCard2.Size = New System.Drawing.Size(273, 126)
        Me.pnlCard2.TabIndex = 5
        '
        'pnlAccent2
        '
        Me.pnlAccent2.BackColor = System.Drawing.Color.FromArgb(CType(CType(142, Byte), Integer), CType(CType(36, Byte), Integer), CType(CType(170, Byte), Integer))
        Me.pnlAccent2.Dock = System.Windows.Forms.DockStyle.Left
        Me.pnlAccent2.Location = New System.Drawing.Point(0, 0)
        Me.pnlAccent2.Name = "pnlAccent2"
        Me.pnlAccent2.Size = New System.Drawing.Size(7, 126)
        Me.pnlAccent2.TabIndex = 3
        '
        'lblCardTitle2
        '
        Me.lblCardTitle2.AutoSize = True
        Me.lblCardTitle2.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.lblCardTitle2.Location = New System.Drawing.Point(25, 14)
        Me.lblCardTitle2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblCardTitle2.Name = "lblCardTitle2"
        Me.lblCardTitle2.Size = New System.Drawing.Size(92, 23)
        Me.lblCardTitle2.TabIndex = 0
        Me.lblCardTitle2.Text = "Screenings"
        '
        'lblStat2
        '
        Me.lblStat2.AutoSize = True
        Me.lblStat2.Font = New System.Drawing.Font("Segoe UI", 24.0!, System.Drawing.FontStyle.Bold)
        Me.lblStat2.Location = New System.Drawing.Point(21, 38)
        Me.lblStat2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblStat2.Name = "lblStat2"
        Me.lblStat2.Size = New System.Drawing.Size(46, 54)
        Me.lblStat2.TabIndex = 1
        Me.lblStat2.Text = "0"
        '
        'lblCardSub2
        '
        Me.lblCardSub2.AutoSize = True
        Me.lblCardSub2.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.lblCardSub2.Location = New System.Drawing.Point(25, 95)
        Me.lblCardSub2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblCardSub2.Name = "lblCardSub2"
        Me.lblCardSub2.Size = New System.Drawing.Size(10, 19)
        Me.lblCardSub2.TabIndex = 2
        Me.lblCardSub2.Text = ""
        '
        'pnlCard3
        '
        Me.pnlCard3.Controls.Add(Me.lblCardTitle3)
        Me.pnlCard3.Controls.Add(Me.lblStat3)
        Me.pnlCard3.Controls.Add(Me.lblCardSub3)
        Me.pnlCard3.Controls.Add(Me.pnlAccent3)
        Me.pnlCard3.Location = New System.Drawing.Point(900, 183)
        Me.pnlCard3.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.pnlCard3.Name = "pnlCard3"
        Me.pnlCard3.Size = New System.Drawing.Size(273, 126)
        Me.pnlCard3.TabIndex = 6
        '
        'pnlAccent3
        '
        Me.pnlAccent3.BackColor = System.Drawing.Color.FromArgb(CType(CType(2, Byte), Integer), CType(CType(136, Byte), Integer), CType(CType(209, Byte), Integer))
        Me.pnlAccent3.Dock = System.Windows.Forms.DockStyle.Left
        Me.pnlAccent3.Location = New System.Drawing.Point(0, 0)
        Me.pnlAccent3.Name = "pnlAccent3"
        Me.pnlAccent3.Size = New System.Drawing.Size(7, 126)
        Me.pnlAccent3.TabIndex = 3
        '
        'lblCardTitle3
        '
        Me.lblCardTitle3.AutoSize = True
        Me.lblCardTitle3.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.lblCardTitle3.Location = New System.Drawing.Point(25, 14)
        Me.lblCardTitle3.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblCardTitle3.Name = "lblCardTitle3"
        Me.lblCardTitle3.Size = New System.Drawing.Size(126, 23)
        Me.lblCardTitle3.TabIndex = 0
        Me.lblCardTitle3.Text = "Bookings taken"
        '
        'lblStat3
        '
        Me.lblStat3.AutoSize = True
        Me.lblStat3.Font = New System.Drawing.Font("Segoe UI", 24.0!, System.Drawing.FontStyle.Bold)
        Me.lblStat3.Location = New System.Drawing.Point(21, 38)
        Me.lblStat3.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblStat3.Name = "lblStat3"
        Me.lblStat3.Size = New System.Drawing.Size(46, 54)
        Me.lblStat3.TabIndex = 1
        Me.lblStat3.Text = "0"
        '
        'lblCardSub3
        '
        Me.lblCardSub3.AutoSize = True
        Me.lblCardSub3.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.lblCardSub3.Location = New System.Drawing.Point(25, 95)
        Me.lblCardSub3.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblCardSub3.Name = "lblCardSub3"
        Me.lblCardSub3.Size = New System.Drawing.Size(10, 19)
        Me.lblCardSub3.TabIndex = 2
        Me.lblCardSub3.Text = ""
        '
        'pnlCard4
        '
        Me.pnlCard4.Controls.Add(Me.lblCardTitle4)
        Me.pnlCard4.Controls.Add(Me.lblStat4)
        Me.pnlCard4.Controls.Add(Me.lblCardSub4)
        Me.pnlCard4.Controls.Add(Me.pnlAccent4)
        Me.pnlCard4.Location = New System.Drawing.Point(1190, 183)
        Me.pnlCard4.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.pnlCard4.Name = "pnlCard4"
        Me.pnlCard4.Size = New System.Drawing.Size(273, 126)
        Me.pnlCard4.TabIndex = 7
        '
        'pnlAccent4
        '
        Me.pnlAccent4.BackColor = System.Drawing.Color.FromArgb(CType(CType(56, Byte), Integer), CType(CType(142, Byte), Integer), CType(CType(60, Byte), Integer))
        Me.pnlAccent4.Dock = System.Windows.Forms.DockStyle.Left
        Me.pnlAccent4.Location = New System.Drawing.Point(0, 0)
        Me.pnlAccent4.Name = "pnlAccent4"
        Me.pnlAccent4.Size = New System.Drawing.Size(7, 126)
        Me.pnlAccent4.TabIndex = 3
        '
        'lblCardTitle4
        '
        Me.lblCardTitle4.AutoSize = True
        Me.lblCardTitle4.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.lblCardTitle4.Location = New System.Drawing.Point(25, 14)
        Me.lblCardTitle4.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblCardTitle4.Name = "lblCardTitle4"
        Me.lblCardTitle4.Size = New System.Drawing.Size(109, 23)
        Me.lblCardTitle4.TabIndex = 0
        Me.lblCardTitle4.Text = "Money taken"
        '
        'lblStat4
        '
        Me.lblStat4.AutoSize = True
        Me.lblStat4.Font = New System.Drawing.Font("Segoe UI", 24.0!, System.Drawing.FontStyle.Bold)
        Me.lblStat4.Location = New System.Drawing.Point(21, 38)
        Me.lblStat4.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblStat4.Name = "lblStat4"
        Me.lblStat4.Size = New System.Drawing.Size(46, 54)
        Me.lblStat4.TabIndex = 1
        Me.lblStat4.Text = "0"
        '
        'lblCardSub4
        '
        Me.lblCardSub4.AutoSize = True
        Me.lblCardSub4.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.lblCardSub4.Location = New System.Drawing.Point(25, 95)
        Me.lblCardSub4.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblCardSub4.Name = "lblCardSub4"
        Me.lblCardSub4.Size = New System.Drawing.Size(10, 19)
        Me.lblCardSub4.TabIndex = 2
        Me.lblCardSub4.Text = ""
        '
        'lblAlerts
        '
        Me.lblAlerts.AutoEllipsis = True
        Me.lblAlerts.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.lblAlerts.Location = New System.Drawing.Point(320, 320)
        Me.lblAlerts.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblAlerts.Name = "lblAlerts"
        Me.lblAlerts.Size = New System.Drawing.Size(700, 26)
        Me.lblAlerts.TabIndex = 8
        Me.lblAlerts.Text = "Next up:"
        Me.lblAlerts.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblTopFilm
        '
        Me.lblTopFilm.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right)), System.Windows.Forms.AnchorStyles)
        Me.lblTopFilm.AutoEllipsis = True
        Me.lblTopFilm.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Italic)
        Me.lblTopFilm.Location = New System.Drawing.Point(1030, 320)
        Me.lblTopFilm.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblTopFilm.Name = "lblTopFilm"
        Me.lblTopFilm.Size = New System.Drawing.Size(433, 26)
        Me.lblTopFilm.TabIndex = 9
        Me.lblTopFilm.Text = "Most popular right now:"
        Me.lblTopFilm.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblWhatsOn
        '
        Me.lblWhatsOn.AutoSize = True
        Me.lblWhatsOn.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold)
        Me.lblWhatsOn.Location = New System.Drawing.Point(317, 362)
        Me.lblWhatsOn.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblWhatsOn.Name = "lblWhatsOn"
        Me.lblWhatsOn.Size = New System.Drawing.Size(280, 28)
        Me.lblWhatsOn.TabIndex = 10
        Me.lblWhatsOn.Text = "What is on"
        '
        'lblSearch
        '
        Me.lblSearch.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right)), System.Windows.Forms.AnchorStyles)
        Me.lblSearch.AutoSize = True
        Me.lblSearch.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.lblSearch.Location = New System.Drawing.Point(872, 366)
        Me.lblSearch.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblSearch.Name = "lblSearch"
        Me.lblSearch.Size = New System.Drawing.Size(60, 23)
        Me.lblSearch.TabIndex = 11
        Me.lblSearch.Text = "Search"
        '
        'txtSearch
        '
        Me.txtSearch.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right)), System.Windows.Forms.AnchorStyles)
        Me.txtSearch.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.txtSearch.Location = New System.Drawing.Point(940, 362)
        Me.txtSearch.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.txtSearch.Name = "txtSearch"
        Me.txtSearch.Size = New System.Drawing.Size(200, 30)
        Me.txtSearch.TabIndex = 12
        '
        'cboShow
        '
        Me.cboShow.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right)), System.Windows.Forms.AnchorStyles)
        Me.cboShow.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboShow.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.cboShow.FormattingEnabled = True
        Me.cboShow.Location = New System.Drawing.Point(1152, 362)
        Me.cboShow.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.cboShow.Name = "cboShow"
        Me.cboShow.Size = New System.Drawing.Size(178, 31)
        Me.cboShow.TabIndex = 13
        '
        'btnRefresh
        '
        Me.btnRefresh.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right)), System.Windows.Forms.AnchorStyles)
        Me.btnRefresh.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnRefresh.Location = New System.Drawing.Point(1340, 361)
        Me.btnRefresh.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnRefresh.Name = "btnRefresh"
        Me.btnRefresh.Size = New System.Drawing.Size(123, 33)
        Me.btnRefresh.TabIndex = 14
        Me.btnRefresh.Text = "Refresh"
        Me.btnRefresh.UseVisualStyleBackColor = True
        '
        'lblNoRows
        '
        Me.lblNoRows.Font = New System.Drawing.Font("Segoe UI", 11.0!)
        Me.lblNoRows.Location = New System.Drawing.Point(320, 480)
        Me.lblNoRows.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblNoRows.Name = "lblNoRows"
        Me.lblNoRows.Size = New System.Drawing.Size(1143, 40)
        Me.lblNoRows.TabIndex = 15
        Me.lblNoRows.Text = "Nothing to show"
        Me.lblNoRows.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.lblNoRows.Visible = False
        '
        'dgvWhatsOn
        '
        Me.dgvWhatsOn.AllowUserToAddRows = False
        Me.dgvWhatsOn.AllowUserToDeleteRows = False
        Me.dgvWhatsOn.AllowUserToResizeRows = False
        Me.dgvWhatsOn.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvWhatsOn.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dgvWhatsOn.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvWhatsOn.Location = New System.Drawing.Point(320, 402)
        Me.dgvWhatsOn.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.dgvWhatsOn.Name = "dgvWhatsOn"
        Me.dgvWhatsOn.ReadOnly = True
        Me.dgvWhatsOn.RowHeadersVisible = False
        Me.dgvWhatsOn.RowHeadersWidth = 51
        Me.dgvWhatsOn.RowTemplate.Height = 30
        Me.dgvWhatsOn.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvWhatsOn.Size = New System.Drawing.Size(1143, 432)
        Me.dgvWhatsOn.TabIndex = 16
        '
        'lblGridCount
        '
        Me.lblGridCount.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right)), System.Windows.Forms.AnchorStyles)
        Me.lblGridCount.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.lblGridCount.Location = New System.Drawing.Point(1113, 843)
        Me.lblGridCount.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblGridCount.Name = "lblGridCount"
        Me.lblGridCount.Size = New System.Drawing.Size(350, 21)
        Me.lblGridCount.TabIndex = 17
        Me.lblGridCount.Text = ""
        Me.lblGridCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblVersion
        '
        Me.lblVersion.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left)), System.Windows.Forms.AnchorStyles)
        Me.lblVersion.AutoSize = True
        Me.lblVersion.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.lblVersion.Location = New System.Drawing.Point(317, 844)
        Me.lblVersion.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblVersion.Name = "lblVersion"
        Me.lblVersion.Size = New System.Drawing.Size(283, 19)
        Me.lblVersion.TabIndex = 18
        Me.lblVersion.Text = "Filmtopia Cinema Management System  v1.0"
        '
        'timerClock
        '
        Me.timerClock.Interval = 1000
        '
        'frmMainMenuV2
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1493, 886)
        Me.Controls.Add(Me.lblVersion)
        Me.Controls.Add(Me.lblGridCount)
        Me.Controls.Add(Me.lblNoRows)
        Me.Controls.Add(Me.dgvWhatsOn)
        Me.Controls.Add(Me.btnRefresh)
        Me.Controls.Add(Me.cboShow)
        Me.Controls.Add(Me.txtSearch)
        Me.Controls.Add(Me.lblSearch)
        Me.Controls.Add(Me.lblWhatsOn)
        Me.Controls.Add(Me.lblTopFilm)
        Me.Controls.Add(Me.lblAlerts)
        Me.Controls.Add(Me.pnlCard4)
        Me.Controls.Add(Me.pnlCard3)
        Me.Controls.Add(Me.pnlCard2)
        Me.Controls.Add(Me.pnlCard1)
        Me.Controls.Add(Me.lblSubtitle)
        Me.Controls.Add(Me.lblWelcome)
        Me.Controls.Add(Me.pnlSidebar)
        Me.Controls.Add(Me.pnlHeader)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
        Me.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.MaximizeBox = True
        Me.Name = "frmMainMenuV2"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Filmtopia Management System"
        Me.pnlHeader.ResumeLayout(False)
        Me.pnlHeader.PerformLayout()
        Me.pnlSidebar.ResumeLayout(False)
        Me.flpNav.ResumeLayout(False)
        Me.flpNav.PerformLayout()
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
    Friend WithEvents lblNavFront As Label
    Friend WithEvents btnBookings As Button
    Friend WithEvents btnFindBooking As Button
    Friend WithEvents btnScreenings As Button
    Friend WithEvents btnCustomers As Button
    Friend WithEvents lblNavManage As Label
    Friend WithEvents btnFilms As Button
    Friend WithEvents btnScreens As Button
    Friend WithEvents btnFood As Button
    Friend WithEvents btnReports As Button
    Friend WithEvents btnLogs As Button
    Friend WithEvents lblNavSystem As Label
    Friend WithEvents btnSettings As Button
    Friend WithEvents btnLogout As Button
    Friend WithEvents lblWelcome As Label
    Friend WithEvents lblSubtitle As Label
    Friend WithEvents pnlCard1 As Panel
    Friend WithEvents pnlAccent1 As Panel
    Friend WithEvents lblCardTitle1 As Label
    Friend WithEvents lblStat1 As Label
    Friend WithEvents lblCardSub1 As Label
    Friend WithEvents pnlCard2 As Panel
    Friend WithEvents pnlAccent2 As Panel
    Friend WithEvents lblCardTitle2 As Label
    Friend WithEvents lblStat2 As Label
    Friend WithEvents lblCardSub2 As Label
    Friend WithEvents pnlCard3 As Panel
    Friend WithEvents pnlAccent3 As Panel
    Friend WithEvents lblCardTitle3 As Label
    Friend WithEvents lblStat3 As Label
    Friend WithEvents lblCardSub3 As Label
    Friend WithEvents pnlCard4 As Panel
    Friend WithEvents pnlAccent4 As Panel
    Friend WithEvents lblCardTitle4 As Label
    Friend WithEvents lblStat4 As Label
    Friend WithEvents lblCardSub4 As Label
    Friend WithEvents lblAlerts As Label
    Friend WithEvents lblTopFilm As Label
    Friend WithEvents lblWhatsOn As Label
    Friend WithEvents lblSearch As Label
    Friend WithEvents txtSearch As TextBox
    Friend WithEvents cboShow As ComboBox
    Friend WithEvents btnRefresh As Button
    Friend WithEvents lblNoRows As Label
    Friend WithEvents dgvWhatsOn As DataGridView
    Friend WithEvents lblGridCount As Label
    Friend WithEvents lblVersion As Label
    Friend WithEvents timerClock As Timer
End Class
