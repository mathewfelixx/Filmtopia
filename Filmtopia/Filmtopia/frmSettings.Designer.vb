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
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.lblTrailer = New System.Windows.Forms.Label()
        Me.txtTrailerMinutes = New System.Windows.Forms.TextBox()
        Me.lblTrailerUnit = New System.Windows.Forms.Label()
        Me.lblTurnaround = New System.Windows.Forms.Label()
        Me.txtTurnaroundMinutes = New System.Windows.Forms.TextBox()
        Me.lblTurnaroundUnit = New System.Windows.Forms.Label()
        Me.lblFirstShow = New System.Windows.Forms.Label()
        Me.txtFirstShow = New System.Windows.Forms.TextBox()
        Me.lblFirstShowUnit = New System.Windows.Forms.Label()
        Me.lblLastShow = New System.Windows.Forms.Label()
        Me.txtLastShow = New System.Windows.Forms.TextBox()
        Me.lblLastShowUnit = New System.Windows.Forms.Label()
        Me.lblTimesHelp = New System.Windows.Forms.Label()
        Me.btnSaveTimes = New System.Windows.Forms.Button()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.lblDefaultPrice = New System.Windows.Forms.Label()
        Me.txtDefaultPrice = New System.Windows.Forms.TextBox()
        Me.lblDefaultPriceUnit = New System.Windows.Forms.Label()
        Me.lblSellingHelp = New System.Windows.Forms.Label()
        Me.btnSaveSelling = New System.Windows.Forms.Button()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.lblMaxSeats = New System.Windows.Forms.Label()
        Me.txtMaxSeats = New System.Windows.Forms.TextBox()
        Me.lblMaxSeatsUnit = New System.Windows.Forms.Label()
        Me.lblIdle = New System.Windows.Forms.Label()
        Me.txtIdleSeconds = New System.Windows.Forms.TextBox()
        Me.lblIdleUnit = New System.Windows.Forms.Label()
        Me.lblThankYou = New System.Windows.Forms.Label()
        Me.txtThankYouSeconds = New System.Windows.Forms.TextBox()
        Me.lblThankYouUnit = New System.Windows.Forms.Label()
        Me.lblKioskHelp = New System.Windows.Forms.Label()
        Me.btnSaveKiosk = New System.Windows.Forms.Button()
        Me.GroupBox5 = New System.Windows.Forms.GroupBox()
        Me.lblLoginTries = New System.Windows.Forms.Label()
        Me.txtLoginTries = New System.Windows.Forms.TextBox()
        Me.lblLoginTriesUnit = New System.Windows.Forms.Label()
        Me.lblMinPassword = New System.Windows.Forms.Label()
        Me.txtMinPassword = New System.Windows.Forms.TextBox()
        Me.lblMinPasswordUnit = New System.Windows.Forms.Label()
        Me.lblSecurityHelp = New System.Windows.Forms.Label()
        Me.btnSaveSecurity = New System.Windows.Forms.Button()
        Me.GroupBox6 = New System.Windows.Forms.GroupBox()
        Me.lblNewUsername = New System.Windows.Forms.Label()
        Me.txtNewUsername = New System.Windows.Forms.TextBox()
        Me.lblNewPassword = New System.Windows.Forms.Label()
        Me.txtNewPassword = New System.Windows.Forms.TextBox()
        Me.lblNewAccessLevel = New System.Windows.Forms.Label()
        Me.cboNewAccessLevel = New System.Windows.Forms.ComboBox()
        Me.lblAccountsHelp = New System.Windows.Forms.Label()
        Me.btnCreateUser = New System.Windows.Forms.Button()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.GroupBox5.SuspendLayout()
        Me.GroupBox6.SuspendLayout()
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
        Me.lblHeading.Text = "Cinema Settings"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.lblFolderCaption)
        Me.GroupBox1.Controls.Add(Me.txtFolder)
        Me.GroupBox1.Controls.Add(Me.btnChooseFolder)
        Me.GroupBox1.Controls.Add(Me.btnCreateBackup)
        Me.GroupBox1.Controls.Add(Me.lblHelp)
        Me.GroupBox1.Location = New System.Drawing.Point(544, 217)
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
        Me.lblVersion.Location = New System.Drawing.Point(13, 565)
        Me.lblVersion.Name = "lblVersion"
        Me.lblVersion.Size = New System.Drawing.Size(45, 13)
        Me.lblVersion.TabIndex = 4
        Me.lblVersion.Text = "Filmtopia Cinema Management System  v1.0"
        '
        'btnClose
        '
        Me.btnClose.Location = New System.Drawing.Point(972, 560)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(88, 26)
        Me.btnClose.TabIndex = 5
        Me.btnClose.Text = "Close"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.lblTrailer)
        Me.GroupBox2.Controls.Add(Me.txtTrailerMinutes)
        Me.GroupBox2.Controls.Add(Me.lblTrailerUnit)
        Me.GroupBox2.Controls.Add(Me.lblTurnaround)
        Me.GroupBox2.Controls.Add(Me.txtTurnaroundMinutes)
        Me.GroupBox2.Controls.Add(Me.lblTurnaroundUnit)
        Me.GroupBox2.Controls.Add(Me.lblFirstShow)
        Me.GroupBox2.Controls.Add(Me.txtFirstShow)
        Me.GroupBox2.Controls.Add(Me.lblFirstShowUnit)
        Me.GroupBox2.Controls.Add(Me.lblLastShow)
        Me.GroupBox2.Controls.Add(Me.txtLastShow)
        Me.GroupBox2.Controls.Add(Me.lblLastShowUnit)
        Me.GroupBox2.Controls.Add(Me.lblTimesHelp)
        Me.GroupBox2.Controls.Add(Me.btnSaveTimes)
        Me.GroupBox2.Location = New System.Drawing.Point(16, 45)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(516, 200)
        Me.GroupBox2.TabIndex = 6
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Screening Times"
        '
        'lblTrailer
        '
        Me.lblTrailer.AutoSize = True
        Me.lblTrailer.Location = New System.Drawing.Point(12, 31)
        Me.lblTrailer.Name = "lblTrailer"
        Me.lblTrailer.Size = New System.Drawing.Size(186, 13)
        Me.lblTrailer.TabIndex = 0
        Me.lblTrailer.Text = "Adverts and trailers before the film"
        '
        'txtTrailerMinutes
        '
        Me.txtTrailerMinutes.Location = New System.Drawing.Point(230, 28)
        Me.txtTrailerMinutes.MaxLength = 3
        Me.txtTrailerMinutes.Name = "txtTrailerMinutes"
        Me.txtTrailerMinutes.Size = New System.Drawing.Size(60, 20)
        Me.txtTrailerMinutes.TabIndex = 1
        '
        'lblTrailerUnit
        '
        Me.lblTrailerUnit.AutoSize = True
        Me.lblTrailerUnit.Location = New System.Drawing.Point(296, 31)
        Me.lblTrailerUnit.Name = "lblTrailerUnit"
        Me.lblTrailerUnit.Size = New System.Drawing.Size(44, 13)
        Me.lblTrailerUnit.TabIndex = 2
        Me.lblTrailerUnit.Text = "minutes"
        '
        'lblTurnaround
        '
        Me.lblTurnaround.AutoSize = True
        Me.lblTurnaround.Location = New System.Drawing.Point(12, 61)
        Me.lblTurnaround.Name = "lblTurnaround"
        Me.lblTurnaround.Size = New System.Drawing.Size(137, 13)
        Me.lblTurnaround.TabIndex = 3
        Me.lblTurnaround.Text = "Clearing up after the film"
        '
        'txtTurnaroundMinutes
        '
        Me.txtTurnaroundMinutes.Location = New System.Drawing.Point(230, 58)
        Me.txtTurnaroundMinutes.MaxLength = 3
        Me.txtTurnaroundMinutes.Name = "txtTurnaroundMinutes"
        Me.txtTurnaroundMinutes.Size = New System.Drawing.Size(60, 20)
        Me.txtTurnaroundMinutes.TabIndex = 4
        '
        'lblTurnaroundUnit
        '
        Me.lblTurnaroundUnit.AutoSize = True
        Me.lblTurnaroundUnit.Location = New System.Drawing.Point(296, 61)
        Me.lblTurnaroundUnit.Name = "lblTurnaroundUnit"
        Me.lblTurnaroundUnit.Size = New System.Drawing.Size(44, 13)
        Me.lblTurnaroundUnit.TabIndex = 5
        Me.lblTurnaroundUnit.Text = "minutes"
        '
        'lblFirstShow
        '
        Me.lblFirstShow.AutoSize = True
        Me.lblFirstShow.Location = New System.Drawing.Point(12, 91)
        Me.lblFirstShow.Name = "lblFirstShow"
        Me.lblFirstShow.Size = New System.Drawing.Size(159, 13)
        Me.lblFirstShow.TabIndex = 6
        Me.lblFirstShow.Text = "Earliest a screening may start"
        '
        'txtFirstShow
        '
        Me.txtFirstShow.Location = New System.Drawing.Point(230, 88)
        Me.txtFirstShow.MaxLength = 5
        Me.txtFirstShow.Name = "txtFirstShow"
        Me.txtFirstShow.Size = New System.Drawing.Size(60, 20)
        Me.txtFirstShow.TabIndex = 7
        '
        'lblFirstShowUnit
        '
        Me.lblFirstShowUnit.AutoSize = True
        Me.lblFirstShowUnit.Location = New System.Drawing.Point(296, 91)
        Me.lblFirstShowUnit.Name = "lblFirstShowUnit"
        Me.lblFirstShowUnit.Size = New System.Drawing.Size(50, 13)
        Me.lblFirstShowUnit.TabIndex = 8
        Me.lblFirstShowUnit.Text = "(HH:MM)"
        '
        'lblLastShow
        '
        Me.lblLastShow.AutoSize = True
        Me.lblLastShow.Location = New System.Drawing.Point(12, 121)
        Me.lblLastShow.Name = "lblLastShow"
        Me.lblLastShow.Size = New System.Drawing.Size(152, 13)
        Me.lblLastShow.TabIndex = 9
        Me.lblLastShow.Text = "Latest a screening may start"
        '
        'txtLastShow
        '
        Me.txtLastShow.Location = New System.Drawing.Point(230, 118)
        Me.txtLastShow.MaxLength = 5
        Me.txtLastShow.Name = "txtLastShow"
        Me.txtLastShow.Size = New System.Drawing.Size(60, 20)
        Me.txtLastShow.TabIndex = 10
        '
        'lblLastShowUnit
        '
        Me.lblLastShowUnit.AutoSize = True
        Me.lblLastShowUnit.Location = New System.Drawing.Point(296, 121)
        Me.lblLastShowUnit.Name = "lblLastShowUnit"
        Me.lblLastShowUnit.Size = New System.Drawing.Size(50, 13)
        Me.lblLastShowUnit.TabIndex = 11
        Me.lblLastShowUnit.Text = "(HH:MM)"
        '
        'lblTimesHelp
        '
        Me.lblTimesHelp.AutoSize = True
        Me.lblTimesHelp.Location = New System.Drawing.Point(12, 168)
        Me.lblTimesHelp.Name = "lblTimesHelp"
        Me.lblTimesHelp.Size = New System.Drawing.Size(330, 13)
        Me.lblTimesHelp.TabIndex = 12
        Me.lblTimesHelp.Text = "These apply to new screenings. Ones already made keep their own."
        '
        'btnSaveTimes
        '
        Me.btnSaveTimes.Location = New System.Drawing.Point(392, 162)
        Me.btnSaveTimes.Name = "btnSaveTimes"
        Me.btnSaveTimes.Size = New System.Drawing.Size(108, 26)
        Me.btnSaveTimes.TabIndex = 13
        Me.btnSaveTimes.Text = "Save times"
        Me.btnSaveTimes.UseVisualStyleBackColor = True
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.lblDefaultPrice)
        Me.GroupBox3.Controls.Add(Me.txtDefaultPrice)
        Me.GroupBox3.Controls.Add(Me.lblDefaultPriceUnit)
        Me.GroupBox3.Controls.Add(Me.lblSellingHelp)
        Me.GroupBox3.Controls.Add(Me.btnSaveSelling)
        Me.GroupBox3.Location = New System.Drawing.Point(16, 257)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(516, 95)
        Me.GroupBox3.TabIndex = 7
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Selling"
        '
        'lblDefaultPrice
        '
        Me.lblDefaultPrice.AutoSize = True
        Me.lblDefaultPrice.Location = New System.Drawing.Point(12, 31)
        Me.lblDefaultPrice.Name = "lblDefaultPrice"
        Me.lblDefaultPrice.Size = New System.Drawing.Size(195, 13)
        Me.lblDefaultPrice.TabIndex = 0
        Me.lblDefaultPrice.Text = "Ticket price a new screening starts at"
        '
        'txtDefaultPrice
        '
        Me.txtDefaultPrice.Location = New System.Drawing.Point(250, 28)
        Me.txtDefaultPrice.MaxLength = 6
        Me.txtDefaultPrice.Name = "txtDefaultPrice"
        Me.txtDefaultPrice.Size = New System.Drawing.Size(60, 20)
        Me.txtDefaultPrice.TabIndex = 1
        '
        'lblDefaultPriceUnit
        '
        Me.lblDefaultPriceUnit.AutoSize = True
        Me.lblDefaultPriceUnit.Location = New System.Drawing.Point(316, 31)
        Me.lblDefaultPriceUnit.Name = "lblDefaultPriceUnit"
        Me.lblDefaultPriceUnit.Size = New System.Drawing.Size(42, 13)
        Me.lblDefaultPriceUnit.TabIndex = 2
        Me.lblDefaultPriceUnit.Text = "pounds"
        '
        'lblSellingHelp
        '
        Me.lblSellingHelp.AutoSize = True
        Me.lblSellingHelp.Location = New System.Drawing.Point(12, 65)
        Me.lblSellingHelp.Name = "lblSellingHelp"
        Me.lblSellingHelp.Size = New System.Drawing.Size(250, 13)
        Me.lblSellingHelp.TabIndex = 3
        Me.lblSellingHelp.Text = "Staff can still change it on the screening itself."
        '
        'btnSaveSelling
        '
        Me.btnSaveSelling.Location = New System.Drawing.Point(392, 58)
        Me.btnSaveSelling.Name = "btnSaveSelling"
        Me.btnSaveSelling.Size = New System.Drawing.Size(108, 26)
        Me.btnSaveSelling.TabIndex = 4
        Me.btnSaveSelling.Text = "Save selling"
        Me.btnSaveSelling.UseVisualStyleBackColor = True
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.lblMaxSeats)
        Me.GroupBox4.Controls.Add(Me.txtMaxSeats)
        Me.GroupBox4.Controls.Add(Me.lblMaxSeatsUnit)
        Me.GroupBox4.Controls.Add(Me.lblIdle)
        Me.GroupBox4.Controls.Add(Me.txtIdleSeconds)
        Me.GroupBox4.Controls.Add(Me.lblIdleUnit)
        Me.GroupBox4.Controls.Add(Me.lblThankYou)
        Me.GroupBox4.Controls.Add(Me.txtThankYouSeconds)
        Me.GroupBox4.Controls.Add(Me.lblThankYouUnit)
        Me.GroupBox4.Controls.Add(Me.lblKioskHelp)
        Me.GroupBox4.Controls.Add(Me.btnSaveKiosk)
        Me.GroupBox4.Location = New System.Drawing.Point(544, 45)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(516, 160)
        Me.GroupBox4.TabIndex = 8
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Kiosk"
        '
        'lblMaxSeats
        '
        Me.lblMaxSeats.AutoSize = True
        Me.lblMaxSeats.Location = New System.Drawing.Point(12, 31)
        Me.lblMaxSeats.Name = "lblMaxSeats"
        Me.lblMaxSeats.Size = New System.Drawing.Size(130, 13)
        Me.lblMaxSeats.TabIndex = 0
        Me.lblMaxSeats.Text = "Most tickets in one sale"
        '
        'txtMaxSeats
        '
        Me.txtMaxSeats.Location = New System.Drawing.Point(250, 28)
        Me.txtMaxSeats.MaxLength = 3
        Me.txtMaxSeats.Name = "txtMaxSeats"
        Me.txtMaxSeats.Size = New System.Drawing.Size(60, 20)
        Me.txtMaxSeats.TabIndex = 1
        '
        'lblMaxSeatsUnit
        '
        Me.lblMaxSeatsUnit.AutoSize = True
        Me.lblMaxSeatsUnit.Location = New System.Drawing.Point(316, 31)
        Me.lblMaxSeatsUnit.Name = "lblMaxSeatsUnit"
        Me.lblMaxSeatsUnit.Size = New System.Drawing.Size(40, 13)
        Me.lblMaxSeatsUnit.TabIndex = 2
        Me.lblMaxSeatsUnit.Text = "tickets"
        '
        'lblIdle
        '
        Me.lblIdle.AutoSize = True
        Me.lblIdle.Location = New System.Drawing.Point(12, 61)
        Me.lblIdle.Name = "lblIdle"
        Me.lblIdle.Size = New System.Drawing.Size(155, 13)
        Me.lblIdle.TabIndex = 3
        Me.lblIdle.Text = "Reset if nobody touches it for"
        '
        'txtIdleSeconds
        '
        Me.txtIdleSeconds.Location = New System.Drawing.Point(250, 58)
        Me.txtIdleSeconds.MaxLength = 4
        Me.txtIdleSeconds.Name = "txtIdleSeconds"
        Me.txtIdleSeconds.Size = New System.Drawing.Size(60, 20)
        Me.txtIdleSeconds.TabIndex = 4
        '
        'lblIdleUnit
        '
        Me.lblIdleUnit.AutoSize = True
        Me.lblIdleUnit.Location = New System.Drawing.Point(316, 61)
        Me.lblIdleUnit.Name = "lblIdleUnit"
        Me.lblIdleUnit.Size = New System.Drawing.Size(46, 13)
        Me.lblIdleUnit.TabIndex = 5
        Me.lblIdleUnit.Text = "seconds"
        '
        'lblThankYou
        '
        Me.lblThankYou.AutoSize = True
        Me.lblThankYou.Location = New System.Drawing.Point(12, 91)
        Me.lblThankYou.Name = "lblThankYou"
        Me.lblThankYou.Size = New System.Drawing.Size(158, 13)
        Me.lblThankYou.TabIndex = 6
        Me.lblThankYou.Text = "Thank you screen stays up for"
        '
        'txtThankYouSeconds
        '
        Me.txtThankYouSeconds.Location = New System.Drawing.Point(250, 88)
        Me.txtThankYouSeconds.MaxLength = 4
        Me.txtThankYouSeconds.Name = "txtThankYouSeconds"
        Me.txtThankYouSeconds.Size = New System.Drawing.Size(60, 20)
        Me.txtThankYouSeconds.TabIndex = 7
        '
        'lblThankYouUnit
        '
        Me.lblThankYouUnit.AutoSize = True
        Me.lblThankYouUnit.Location = New System.Drawing.Point(316, 91)
        Me.lblThankYouUnit.Name = "lblThankYouUnit"
        Me.lblThankYouUnit.Size = New System.Drawing.Size(46, 13)
        Me.lblThankYouUnit.TabIndex = 8
        Me.lblThankYouUnit.Text = "seconds"
        '
        'lblKioskHelp
        '
        Me.lblKioskHelp.AutoSize = True
        Me.lblKioskHelp.Location = New System.Drawing.Point(12, 131)
        Me.lblKioskHelp.Name = "lblKioskHelp"
        Me.lblKioskHelp.Size = New System.Drawing.Size(220, 13)
        Me.lblKioskHelp.TabIndex = 9
        Me.lblKioskHelp.Text = "These only affect the self service machine."
        '
        'btnSaveKiosk
        '
        Me.btnSaveKiosk.Location = New System.Drawing.Point(392, 124)
        Me.btnSaveKiosk.Name = "btnSaveKiosk"
        Me.btnSaveKiosk.Size = New System.Drawing.Size(108, 26)
        Me.btnSaveKiosk.TabIndex = 10
        Me.btnSaveKiosk.Text = "Save kiosk"
        Me.btnSaveKiosk.UseVisualStyleBackColor = True
        '
        'GroupBox5
        '
        Me.GroupBox5.Controls.Add(Me.lblLoginTries)
        Me.GroupBox5.Controls.Add(Me.txtLoginTries)
        Me.GroupBox5.Controls.Add(Me.lblLoginTriesUnit)
        Me.GroupBox5.Controls.Add(Me.lblMinPassword)
        Me.GroupBox5.Controls.Add(Me.txtMinPassword)
        Me.GroupBox5.Controls.Add(Me.lblMinPasswordUnit)
        Me.GroupBox5.Controls.Add(Me.lblSecurityHelp)
        Me.GroupBox5.Controls.Add(Me.btnSaveSecurity)
        Me.GroupBox5.Location = New System.Drawing.Point(16, 364)
        Me.GroupBox5.Name = "GroupBox5"
        Me.GroupBox5.Size = New System.Drawing.Size(516, 120)
        Me.GroupBox5.TabIndex = 9
        Me.GroupBox5.TabStop = False
        Me.GroupBox5.Text = "Security"
        '
        'lblLoginTries
        '
        Me.lblLoginTries.AutoSize = True
        Me.lblLoginTries.Location = New System.Drawing.Point(12, 31)
        Me.lblLoginTries.Name = "lblLoginTries"
        Me.lblLoginTries.Size = New System.Drawing.Size(200, 13)
        Me.lblLoginTries.TabIndex = 0
        Me.lblLoginTries.Text = "Wrong passwords before the app closes"
        '
        'txtLoginTries
        '
        Me.txtLoginTries.Location = New System.Drawing.Point(250, 28)
        Me.txtLoginTries.MaxLength = 3
        Me.txtLoginTries.Name = "txtLoginTries"
        Me.txtLoginTries.Size = New System.Drawing.Size(60, 20)
        Me.txtLoginTries.TabIndex = 1
        '
        'lblLoginTriesUnit
        '
        Me.lblLoginTriesUnit.AutoSize = True
        Me.lblLoginTriesUnit.Location = New System.Drawing.Point(316, 31)
        Me.lblLoginTriesUnit.Name = "lblLoginTriesUnit"
        Me.lblLoginTriesUnit.Size = New System.Drawing.Size(30, 13)
        Me.lblLoginTriesUnit.TabIndex = 2
        Me.lblLoginTriesUnit.Text = "tries"
        '
        'lblMinPassword
        '
        Me.lblMinPassword.AutoSize = True
        Me.lblMinPassword.Location = New System.Drawing.Point(12, 61)
        Me.lblMinPassword.Name = "lblMinPassword"
        Me.lblMinPassword.Size = New System.Drawing.Size(137, 13)
        Me.lblMinPassword.TabIndex = 3
        Me.lblMinPassword.Text = "Shortest password allowed"
        '
        'txtMinPassword
        '
        Me.txtMinPassword.Location = New System.Drawing.Point(250, 58)
        Me.txtMinPassword.MaxLength = 3
        Me.txtMinPassword.Name = "txtMinPassword"
        Me.txtMinPassword.Size = New System.Drawing.Size(60, 20)
        Me.txtMinPassword.TabIndex = 4
        '
        'lblMinPasswordUnit
        '
        Me.lblMinPasswordUnit.AutoSize = True
        Me.lblMinPasswordUnit.Location = New System.Drawing.Point(316, 61)
        Me.lblMinPasswordUnit.Name = "lblMinPasswordUnit"
        Me.lblMinPasswordUnit.Size = New System.Drawing.Size(58, 13)
        Me.lblMinPasswordUnit.TabIndex = 5
        Me.lblMinPasswordUnit.Text = "characters"
        '
        'lblSecurityHelp
        '
        Me.lblSecurityHelp.AutoSize = True
        Me.lblSecurityHelp.Location = New System.Drawing.Point(12, 95)
        Me.lblSecurityHelp.Name = "lblSecurityHelp"
        Me.lblSecurityHelp.Size = New System.Drawing.Size(205, 13)
        Me.lblSecurityHelp.TabIndex = 6
        Me.lblSecurityHelp.Text = "The login screen counts down from this."
        '
        'btnSaveSecurity
        '
        Me.btnSaveSecurity.Location = New System.Drawing.Point(392, 88)
        Me.btnSaveSecurity.Name = "btnSaveSecurity"
        Me.btnSaveSecurity.Size = New System.Drawing.Size(108, 26)
        Me.btnSaveSecurity.TabIndex = 7
        Me.btnSaveSecurity.Text = "Save security"
        Me.btnSaveSecurity.UseVisualStyleBackColor = True
        '
        'GroupBox6
        '
        Me.GroupBox6.Controls.Add(Me.lblNewUsername)
        Me.GroupBox6.Controls.Add(Me.txtNewUsername)
        Me.GroupBox6.Controls.Add(Me.lblNewPassword)
        Me.GroupBox6.Controls.Add(Me.txtNewPassword)
        Me.GroupBox6.Controls.Add(Me.lblNewAccessLevel)
        Me.GroupBox6.Controls.Add(Me.cboNewAccessLevel)
        Me.GroupBox6.Controls.Add(Me.lblAccountsHelp)
        Me.GroupBox6.Controls.Add(Me.btnCreateUser)
        Me.GroupBox6.Location = New System.Drawing.Point(544, 384)
        Me.GroupBox6.Name = "GroupBox6"
        Me.GroupBox6.Size = New System.Drawing.Size(516, 155)
        Me.GroupBox6.TabIndex = 10
        Me.GroupBox6.TabStop = False
        Me.GroupBox6.Text = "Staff Accounts"
        '
        'lblNewUsername
        '
        Me.lblNewUsername.AutoSize = True
        Me.lblNewUsername.Location = New System.Drawing.Point(12, 31)
        Me.lblNewUsername.Name = "lblNewUsername"
        Me.lblNewUsername.Size = New System.Drawing.Size(157, 13)
        Me.lblNewUsername.TabIndex = 0
        Me.lblNewUsername.Text = "Username for the new account"
        '
        'txtNewUsername
        '
        Me.txtNewUsername.Location = New System.Drawing.Point(250, 28)
        Me.txtNewUsername.MaxLength = 20
        Me.txtNewUsername.Name = "txtNewUsername"
        Me.txtNewUsername.Size = New System.Drawing.Size(180, 20)
        Me.txtNewUsername.TabIndex = 1
        '
        'lblNewPassword
        '
        Me.lblNewPassword.AutoSize = True
        Me.lblNewPassword.Location = New System.Drawing.Point(12, 61)
        Me.lblNewPassword.Name = "lblNewPassword"
        Me.lblNewPassword.Size = New System.Drawing.Size(151, 13)
        Me.lblNewPassword.TabIndex = 2
        Me.lblNewPassword.Text = "Password to start them with"
        '
        'txtNewPassword
        '
        Me.txtNewPassword.Location = New System.Drawing.Point(250, 58)
        Me.txtNewPassword.MaxLength = 20
        Me.txtNewPassword.Name = "txtNewPassword"
        Me.txtNewPassword.Size = New System.Drawing.Size(180, 20)
        Me.txtNewPassword.TabIndex = 3
        '
        'lblNewAccessLevel
        '
        Me.lblNewAccessLevel.AutoSize = True
        Me.lblNewAccessLevel.Location = New System.Drawing.Point(12, 91)
        Me.lblNewAccessLevel.Name = "lblNewAccessLevel"
        Me.lblNewAccessLevel.Size = New System.Drawing.Size(148, 13)
        Me.lblNewAccessLevel.TabIndex = 4
        Me.lblNewAccessLevel.Text = "What they are allowed to do"
        '
        'cboNewAccessLevel
        '
        Me.cboNewAccessLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboNewAccessLevel.FormattingEnabled = True
        Me.cboNewAccessLevel.Location = New System.Drawing.Point(250, 88)
        Me.cboNewAccessLevel.Name = "cboNewAccessLevel"
        Me.cboNewAccessLevel.Size = New System.Drawing.Size(240, 21)
        Me.cboNewAccessLevel.TabIndex = 5
        '
        'lblAccountsHelp
        '
        Me.lblAccountsHelp.AutoSize = True
        Me.lblAccountsHelp.Location = New System.Drawing.Point(12, 128)
        Me.lblAccountsHelp.Name = "lblAccountsHelp"
        Me.lblAccountsHelp.Size = New System.Drawing.Size(233, 13)
        Me.lblAccountsHelp.TabIndex = 6
        Me.lblAccountsHelp.Text = "Tell them the password, they can change it later."
        '
        'btnCreateUser
        '
        Me.btnCreateUser.Location = New System.Drawing.Point(392, 121)
        Me.btnCreateUser.Name = "btnCreateUser"
        Me.btnCreateUser.Size = New System.Drawing.Size(108, 26)
        Me.btnCreateUser.TabIndex = 7
        Me.btnCreateUser.Text = "Create account"
        Me.btnCreateUser.UseVisualStyleBackColor = True
        '
        'frmSettings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1076, 600)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.lblVersion)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.GroupBox4)
        Me.Controls.Add(Me.GroupBox5)
        Me.Controls.Add(Me.GroupBox6)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.lblHeading)
        Me.CancelButton = Me.btnClose
        Me.Name = "frmSettings"
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Text = "Cinema Settings"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        Me.GroupBox5.ResumeLayout(False)
        Me.GroupBox5.PerformLayout()
        Me.GroupBox6.ResumeLayout(False)
        Me.GroupBox6.PerformLayout()
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
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents lblTrailer As Label
    Friend WithEvents txtTrailerMinutes As TextBox
    Friend WithEvents lblTrailerUnit As Label
    Friend WithEvents lblTurnaround As Label
    Friend WithEvents txtTurnaroundMinutes As TextBox
    Friend WithEvents lblTurnaroundUnit As Label
    Friend WithEvents lblFirstShow As Label
    Friend WithEvents txtFirstShow As TextBox
    Friend WithEvents lblFirstShowUnit As Label
    Friend WithEvents lblLastShow As Label
    Friend WithEvents txtLastShow As TextBox
    Friend WithEvents lblLastShowUnit As Label
    Friend WithEvents lblTimesHelp As Label
    Friend WithEvents btnSaveTimes As Button
    Friend WithEvents GroupBox3 As GroupBox
    Friend WithEvents lblDefaultPrice As Label
    Friend WithEvents txtDefaultPrice As TextBox
    Friend WithEvents lblDefaultPriceUnit As Label
    Friend WithEvents lblSellingHelp As Label
    Friend WithEvents btnSaveSelling As Button
    Friend WithEvents GroupBox4 As GroupBox
    Friend WithEvents lblMaxSeats As Label
    Friend WithEvents txtMaxSeats As TextBox
    Friend WithEvents lblMaxSeatsUnit As Label
    Friend WithEvents lblIdle As Label
    Friend WithEvents txtIdleSeconds As TextBox
    Friend WithEvents lblIdleUnit As Label
    Friend WithEvents lblThankYou As Label
    Friend WithEvents txtThankYouSeconds As TextBox
    Friend WithEvents lblThankYouUnit As Label
    Friend WithEvents lblKioskHelp As Label
    Friend WithEvents btnSaveKiosk As Button
    Friend WithEvents GroupBox5 As GroupBox
    Friend WithEvents lblLoginTries As Label
    Friend WithEvents txtLoginTries As TextBox
    Friend WithEvents lblLoginTriesUnit As Label
    Friend WithEvents lblMinPassword As Label
    Friend WithEvents txtMinPassword As TextBox
    Friend WithEvents lblMinPasswordUnit As Label
    Friend WithEvents lblSecurityHelp As Label
    Friend WithEvents btnSaveSecurity As Button
    Friend WithEvents GroupBox6 As GroupBox
    Friend WithEvents lblNewUsername As Label
    Friend WithEvents txtNewUsername As TextBox
    Friend WithEvents lblNewPassword As Label
    Friend WithEvents txtNewPassword As TextBox
    Friend WithEvents lblNewAccessLevel As Label
    Friend WithEvents cboNewAccessLevel As ComboBox
    Friend WithEvents lblAccountsHelp As Label
    Friend WithEvents btnCreateUser As Button
End Class
