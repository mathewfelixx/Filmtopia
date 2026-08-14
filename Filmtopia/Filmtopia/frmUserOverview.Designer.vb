<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmUserOverview
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
        Me.lblHeading = New System.Windows.Forms.Label()
        Me.lblWelcome = New System.Windows.Forms.Label()
        Me.lblRole = New System.Windows.Forms.Label()
        Me.lblSubLastLogin = New System.Windows.Forms.Label()
        Me.lblSubSession = New System.Windows.Forms.Label()
        Me.pnlCard1 = New System.Windows.Forms.Panel()
        Me.lblCardTitle1 = New System.Windows.Forms.Label()
        Me.lblStat1 = New System.Windows.Forms.Label()
        Me.lblCardSub1 = New System.Windows.Forms.Label()
        Me.pnlAccent1 = New System.Windows.Forms.Panel()
        Me.pnlCard2 = New System.Windows.Forms.Panel()
        Me.lblCardTitle2 = New System.Windows.Forms.Label()
        Me.lblStat2 = New System.Windows.Forms.Label()
        Me.lblCardSub2 = New System.Windows.Forms.Label()
        Me.pnlAccent2 = New System.Windows.Forms.Panel()
        Me.pnlCard3 = New System.Windows.Forms.Panel()
        Me.lblCardTitle3 = New System.Windows.Forms.Label()
        Me.lblStat3 = New System.Windows.Forms.Label()
        Me.lblCardSub3 = New System.Windows.Forms.Label()
        Me.pnlAccent3 = New System.Windows.Forms.Panel()
        Me.pnlCard4 = New System.Windows.Forms.Panel()
        Me.lblCardTitle4 = New System.Windows.Forms.Label()
        Me.lblStat4 = New System.Windows.Forms.Label()
        Me.lblCardSub4 = New System.Windows.Forms.Label()
        Me.pnlAccent4 = New System.Windows.Forms.Panel()
        Me.tabMe = New System.Windows.Forms.TabControl()
        Me.tabActivity = New System.Windows.Forms.TabPage()
        Me.tabSales = New System.Windows.Forms.TabPage()
        Me.tabSettings = New System.Windows.Forms.TabPage()
        Me.lblActFrom = New System.Windows.Forms.Label()
        Me.dtpActFrom = New System.Windows.Forms.DateTimePicker()
        Me.lblActTo = New System.Windows.Forms.Label()
        Me.dtpActTo = New System.Windows.Forms.DateTimePicker()
        Me.lblActArea = New System.Windows.Forms.Label()
        Me.cboActType = New System.Windows.Forms.ComboBox()
        Me.lblActLevel = New System.Windows.Forms.Label()
        Me.cboActSeverity = New System.Windows.Forms.ComboBox()
        Me.lblActSearch = New System.Windows.Forms.Label()
        Me.txtActSearch = New System.Windows.Forms.TextBox()
        Me.btnActApply = New System.Windows.Forms.Button()
        Me.btnActClear = New System.Windows.Forms.Button()
        Me.btnActExport = New System.Windows.Forms.Button()
        Me.dgvActivity = New System.Windows.Forms.DataGridView()
        Me.lblGridCount = New System.Windows.Forms.Label()
        Me.lblSalesFrom = New System.Windows.Forms.Label()
        Me.dtpSalesFrom = New System.Windows.Forms.DateTimePicker()
        Me.lblSalesTo = New System.Windows.Forms.Label()
        Me.dtpSalesTo = New System.Windows.Forms.DateTimePicker()
        Me.lblSubSalesHint = New System.Windows.Forms.Label()
        Me.btnSalesExport = New System.Windows.Forms.Button()
        Me.dgvMySales = New System.Windows.Forms.DataGridView()
        Me.lblSalesCount = New System.Windows.Forms.Label()
        Me.lblSubSettingsHint = New System.Windows.Forms.Label()
        Me.dgvMySettings = New System.Windows.Forms.DataGridView()
        Me.btnResetMySettings = New System.Windows.Forms.Button()
        Me.lblSubResetHint = New System.Windows.Forms.Label()
        Me.lblVersion = New System.Windows.Forms.Label()
        Me.pnlCard1.SuspendLayout()
        Me.pnlCard2.SuspendLayout()
        Me.pnlCard3.SuspendLayout()
        Me.pnlCard4.SuspendLayout()
        Me.tabMe.SuspendLayout()
        Me.tabActivity.SuspendLayout()
        Me.tabSales.SuspendLayout()
        Me.tabSettings.SuspendLayout()
        CType(Me.dgvMySettings, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvMySales, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvActivity, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblHeading
        '
        Me.lblHeading.AutoSize = True
        Me.lblHeading.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Bold)
        Me.lblHeading.Location = New System.Drawing.Point(16, 12)
        Me.lblHeading.Name = "lblHeading"
        Me.lblHeading.Size = New System.Drawing.Size(126, 25)
        Me.lblHeading.TabIndex = 0
        Me.lblHeading.Text = "My Account"
        '
        'lblWelcome
        '
        Me.lblWelcome.AutoSize = True
        Me.lblWelcome.Font = New System.Drawing.Font("Segoe UI", 12.0!)
        Me.lblWelcome.Location = New System.Drawing.Point(16, 48)
        Me.lblWelcome.Name = "lblWelcome"
        Me.lblWelcome.Size = New System.Drawing.Size(70, 21)
        Me.lblWelcome.TabIndex = 1
        Me.lblWelcome.Text = "Welcome"
        '
        'lblRole
        '
        Me.lblRole.AutoSize = True
        Me.lblRole.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.lblRole.Location = New System.Drawing.Point(16, 80)
        Me.lblRole.Name = "lblRole"
        Me.lblRole.Size = New System.Drawing.Size(0, 17)
        Me.lblRole.TabIndex = 2
        '
        'lblSubLastLogin
        '
        Me.lblSubLastLogin.AutoSize = True
        Me.lblSubLastLogin.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.lblSubLastLogin.Location = New System.Drawing.Point(16, 106)
        Me.lblSubLastLogin.Name = "lblSubLastLogin"
        Me.lblSubLastLogin.Size = New System.Drawing.Size(0, 13)
        Me.lblSubLastLogin.TabIndex = 3
        '
        'lblSubSession
        '
        Me.lblSubSession.AutoSize = True
        Me.lblSubSession.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.lblSubSession.Location = New System.Drawing.Point(16, 126)
        Me.lblSubSession.Name = "lblSubSession"
        Me.lblSubSession.Size = New System.Drawing.Size(0, 13)
        Me.lblSubSession.TabIndex = 4
        '
        'pnlCard1
        '
        Me.pnlCard1.Controls.Add(Me.lblCardTitle1)
        Me.pnlCard1.Controls.Add(Me.lblStat1)
        Me.pnlCard1.Controls.Add(Me.lblCardSub1)
        Me.pnlCard1.Controls.Add(Me.pnlAccent1)
        Me.pnlCard1.Location = New System.Drawing.Point(16, 158)
        Me.pnlCard1.Name = "pnlCard1"
        Me.pnlCard1.Size = New System.Drawing.Size(263, 126)
        Me.pnlCard1.TabIndex = 5
        '
        'lblCardTitle1
        '
        Me.lblCardTitle1.AutoSize = True
        Me.lblCardTitle1.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.lblCardTitle1.Location = New System.Drawing.Point(25, 14)
        Me.lblCardTitle1.Name = "lblCardTitle1"
        Me.lblCardTitle1.Size = New System.Drawing.Size(80, 17)
        Me.lblCardTitle1.TabIndex = 0
        Me.lblCardTitle1.Text = "Sales taken"
        '
        'lblStat1
        '
        Me.lblStat1.AutoSize = True
        Me.lblStat1.Font = New System.Drawing.Font("Segoe UI", 24.0!, System.Drawing.FontStyle.Bold)
        Me.lblStat1.Location = New System.Drawing.Point(21, 38)
        Me.lblStat1.Name = "lblStat1"
        Me.lblStat1.Size = New System.Drawing.Size(37, 45)
        Me.lblStat1.TabIndex = 1
        Me.lblStat1.Text = "0"
        '
        'lblCardSub1
        '
        Me.lblCardSub1.AutoSize = True
        Me.lblCardSub1.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.lblCardSub1.Location = New System.Drawing.Point(25, 95)
        Me.lblCardSub1.Name = "lblCardSub1"
        Me.lblCardSub1.Size = New System.Drawing.Size(0, 13)
        Me.lblCardSub1.TabIndex = 2
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
        'pnlCard2
        '
        Me.pnlCard2.Controls.Add(Me.lblCardTitle2)
        Me.pnlCard2.Controls.Add(Me.lblStat2)
        Me.pnlCard2.Controls.Add(Me.lblCardSub2)
        Me.pnlCard2.Controls.Add(Me.pnlAccent2)
        Me.pnlCard2.Location = New System.Drawing.Point(296, 158)
        Me.pnlCard2.Name = "pnlCard2"
        Me.pnlCard2.Size = New System.Drawing.Size(263, 126)
        Me.pnlCard2.TabIndex = 6
        '
        'lblCardTitle2
        '
        Me.lblCardTitle2.AutoSize = True
        Me.lblCardTitle2.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.lblCardTitle2.Location = New System.Drawing.Point(25, 14)
        Me.lblCardTitle2.Name = "lblCardTitle2"
        Me.lblCardTitle2.Size = New System.Drawing.Size(80, 17)
        Me.lblCardTitle2.TabIndex = 0
        Me.lblCardTitle2.Text = "Seats sold"
        '
        'lblStat2
        '
        Me.lblStat2.AutoSize = True
        Me.lblStat2.Font = New System.Drawing.Font("Segoe UI", 24.0!, System.Drawing.FontStyle.Bold)
        Me.lblStat2.Location = New System.Drawing.Point(21, 38)
        Me.lblStat2.Name = "lblStat2"
        Me.lblStat2.Size = New System.Drawing.Size(37, 45)
        Me.lblStat2.TabIndex = 1
        Me.lblStat2.Text = "0"
        '
        'lblCardSub2
        '
        Me.lblCardSub2.AutoSize = True
        Me.lblCardSub2.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.lblCardSub2.Location = New System.Drawing.Point(25, 95)
        Me.lblCardSub2.Name = "lblCardSub2"
        Me.lblCardSub2.Size = New System.Drawing.Size(0, 13)
        Me.lblCardSub2.TabIndex = 2
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
        'pnlCard3
        '
        Me.pnlCard3.Controls.Add(Me.lblCardTitle3)
        Me.pnlCard3.Controls.Add(Me.lblStat3)
        Me.pnlCard3.Controls.Add(Me.lblCardSub3)
        Me.pnlCard3.Controls.Add(Me.pnlAccent3)
        Me.pnlCard3.Location = New System.Drawing.Point(576, 158)
        Me.pnlCard3.Name = "pnlCard3"
        Me.pnlCard3.Size = New System.Drawing.Size(263, 126)
        Me.pnlCard3.TabIndex = 7
        '
        'lblCardTitle3
        '
        Me.lblCardTitle3.AutoSize = True
        Me.lblCardTitle3.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.lblCardTitle3.Location = New System.Drawing.Point(25, 14)
        Me.lblCardTitle3.Name = "lblCardTitle3"
        Me.lblCardTitle3.Size = New System.Drawing.Size(80, 17)
        Me.lblCardTitle3.TabIndex = 0
        Me.lblCardTitle3.Text = "Things done"
        '
        'lblStat3
        '
        Me.lblStat3.AutoSize = True
        Me.lblStat3.Font = New System.Drawing.Font("Segoe UI", 24.0!, System.Drawing.FontStyle.Bold)
        Me.lblStat3.Location = New System.Drawing.Point(21, 38)
        Me.lblStat3.Name = "lblStat3"
        Me.lblStat3.Size = New System.Drawing.Size(37, 45)
        Me.lblStat3.TabIndex = 1
        Me.lblStat3.Text = "0"
        '
        'lblCardSub3
        '
        Me.lblCardSub3.AutoSize = True
        Me.lblCardSub3.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.lblCardSub3.Location = New System.Drawing.Point(25, 95)
        Me.lblCardSub3.Name = "lblCardSub3"
        Me.lblCardSub3.Size = New System.Drawing.Size(0, 13)
        Me.lblCardSub3.TabIndex = 2
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
        'pnlCard4
        '
        Me.pnlCard4.Controls.Add(Me.lblCardTitle4)
        Me.pnlCard4.Controls.Add(Me.lblStat4)
        Me.pnlCard4.Controls.Add(Me.lblCardSub4)
        Me.pnlCard4.Controls.Add(Me.pnlAccent4)
        Me.pnlCard4.Location = New System.Drawing.Point(856, 158)
        Me.pnlCard4.Name = "pnlCard4"
        Me.pnlCard4.Size = New System.Drawing.Size(263, 126)
        Me.pnlCard4.TabIndex = 8
        '
        'lblCardTitle4
        '
        Me.lblCardTitle4.AutoSize = True
        Me.lblCardTitle4.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.lblCardTitle4.Location = New System.Drawing.Point(25, 14)
        Me.lblCardTitle4.Name = "lblCardTitle4"
        Me.lblCardTitle4.Size = New System.Drawing.Size(80, 17)
        Me.lblCardTitle4.TabIndex = 0
        Me.lblCardTitle4.Text = "Takings"
        '
        'lblStat4
        '
        Me.lblStat4.AutoSize = True
        Me.lblStat4.Font = New System.Drawing.Font("Segoe UI", 24.0!, System.Drawing.FontStyle.Bold)
        Me.lblStat4.Location = New System.Drawing.Point(21, 38)
        Me.lblStat4.Name = "lblStat4"
        Me.lblStat4.Size = New System.Drawing.Size(37, 45)
        Me.lblStat4.TabIndex = 1
        Me.lblStat4.Text = "0"
        '
        'lblCardSub4
        '
        Me.lblCardSub4.AutoSize = True
        Me.lblCardSub4.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.lblCardSub4.Location = New System.Drawing.Point(25, 95)
        Me.lblCardSub4.Name = "lblCardSub4"
        Me.lblCardSub4.Size = New System.Drawing.Size(0, 13)
        Me.lblCardSub4.TabIndex = 2
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
        'tabMe
        '
        Me.tabMe.Controls.Add(Me.tabActivity)
        Me.tabMe.Controls.Add(Me.tabSales)
        Me.tabMe.Controls.Add(Me.tabSettings)
        Me.tabMe.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.tabMe.Location = New System.Drawing.Point(16, 298)
        Me.tabMe.Name = "tabMe"
        Me.tabMe.SelectedIndex = 0
        Me.tabMe.Size = New System.Drawing.Size(1103, 442)
        Me.tabMe.TabIndex = 9
        '
        'tabActivity
        '
        Me.tabActivity.Controls.Add(Me.lblGridCount)
        Me.tabActivity.Controls.Add(Me.dgvActivity)
        Me.tabActivity.Controls.Add(Me.btnActExport)
        Me.tabActivity.Controls.Add(Me.btnActClear)
        Me.tabActivity.Controls.Add(Me.btnActApply)
        Me.tabActivity.Controls.Add(Me.txtActSearch)
        Me.tabActivity.Controls.Add(Me.lblActSearch)
        Me.tabActivity.Controls.Add(Me.cboActSeverity)
        Me.tabActivity.Controls.Add(Me.lblActLevel)
        Me.tabActivity.Controls.Add(Me.cboActType)
        Me.tabActivity.Controls.Add(Me.lblActArea)
        Me.tabActivity.Controls.Add(Me.dtpActTo)
        Me.tabActivity.Controls.Add(Me.lblActTo)
        Me.tabActivity.Controls.Add(Me.dtpActFrom)
        Me.tabActivity.Controls.Add(Me.lblActFrom)
        Me.tabActivity.Location = New System.Drawing.Point(4, 24)
        Me.tabActivity.Name = "tabActivity"
        Me.tabActivity.Padding = New System.Windows.Forms.Padding(3)
        Me.tabActivity.Size = New System.Drawing.Size(1095, 414)
        Me.tabActivity.TabIndex = 0
        Me.tabActivity.Text = "What I have done"
        Me.tabActivity.UseVisualStyleBackColor = True
        '
        'tabSales
        '
        Me.tabSales.Controls.Add(Me.lblSalesCount)
        Me.tabSales.Controls.Add(Me.dgvMySales)
        Me.tabSales.Controls.Add(Me.btnSalesExport)
        Me.tabSales.Controls.Add(Me.lblSubSalesHint)
        Me.tabSales.Controls.Add(Me.dtpSalesTo)
        Me.tabSales.Controls.Add(Me.lblSalesTo)
        Me.tabSales.Controls.Add(Me.dtpSalesFrom)
        Me.tabSales.Controls.Add(Me.lblSalesFrom)
        Me.tabSales.Location = New System.Drawing.Point(4, 24)
        Me.tabSales.Name = "tabSales"
        Me.tabSales.Padding = New System.Windows.Forms.Padding(3)
        Me.tabSales.Size = New System.Drawing.Size(1095, 414)
        Me.tabSales.TabIndex = 1
        Me.tabSales.Text = "Sales I have taken"
        Me.tabSales.UseVisualStyleBackColor = True
        '
        'tabSettings
        '
        Me.tabSettings.Controls.Add(Me.lblSubResetHint)
        Me.tabSettings.Controls.Add(Me.btnResetMySettings)
        Me.tabSettings.Controls.Add(Me.dgvMySettings)
        Me.tabSettings.Controls.Add(Me.lblSubSettingsHint)
        Me.tabSettings.Location = New System.Drawing.Point(4, 24)
        Me.tabSettings.Name = "tabSettings"
        Me.tabSettings.Padding = New System.Windows.Forms.Padding(3)
        Me.tabSettings.Size = New System.Drawing.Size(1095, 414)
        Me.tabSettings.TabIndex = 2
        Me.tabSettings.Text = "My settings"
        Me.tabSettings.UseVisualStyleBackColor = True
        '
        'lblActFrom
        '
        Me.lblActFrom.AutoSize = True
        Me.lblActFrom.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.lblActFrom.Location = New System.Drawing.Point(12, 12)
        Me.lblActFrom.Name = "lblActFrom"
        Me.lblActFrom.Size = New System.Drawing.Size(37, 17)
        Me.lblActFrom.TabIndex = 0
        Me.lblActFrom.Text = "From"
        '
        'dtpActFrom
        '
        Me.dtpActFrom.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.dtpActFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short
        Me.dtpActFrom.Location = New System.Drawing.Point(12, 36)
        Me.dtpActFrom.Name = "dtpActFrom"
        Me.dtpActFrom.Size = New System.Drawing.Size(120, 25)
        Me.dtpActFrom.TabIndex = 1
        '
        'lblActTo
        '
        Me.lblActTo.AutoSize = True
        Me.lblActTo.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.lblActTo.Location = New System.Drawing.Point(144, 12)
        Me.lblActTo.Name = "lblActTo"
        Me.lblActTo.Size = New System.Drawing.Size(22, 17)
        Me.lblActTo.TabIndex = 0
        Me.lblActTo.Text = "To"
        '
        'dtpActTo
        '
        Me.dtpActTo.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.dtpActTo.Format = System.Windows.Forms.DateTimePickerFormat.Short
        Me.dtpActTo.Location = New System.Drawing.Point(144, 36)
        Me.dtpActTo.Name = "dtpActTo"
        Me.dtpActTo.Size = New System.Drawing.Size(120, 25)
        Me.dtpActTo.TabIndex = 2
        '
        'lblActArea
        '
        Me.lblActArea.AutoSize = True
        Me.lblActArea.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.lblActArea.Location = New System.Drawing.Point(276, 12)
        Me.lblActArea.Name = "lblActArea"
        Me.lblActArea.Size = New System.Drawing.Size(33, 17)
        Me.lblActArea.TabIndex = 0
        Me.lblActArea.Text = "Area"
        '
        'cboActType
        '
        Me.cboActType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboActType.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.cboActType.FormattingEnabled = True
        Me.cboActType.Location = New System.Drawing.Point(276, 36)
        Me.cboActType.Name = "cboActType"
        Me.cboActType.Size = New System.Drawing.Size(140, 25)
        Me.cboActType.TabIndex = 3
        '
        'lblActLevel
        '
        Me.lblActLevel.AutoSize = True
        Me.lblActLevel.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.lblActLevel.Location = New System.Drawing.Point(428, 12)
        Me.lblActLevel.Name = "lblActLevel"
        Me.lblActLevel.Size = New System.Drawing.Size(38, 17)
        Me.lblActLevel.TabIndex = 0
        Me.lblActLevel.Text = "Level"
        '
        'cboActSeverity
        '
        Me.cboActSeverity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboActSeverity.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.cboActSeverity.FormattingEnabled = True
        Me.cboActSeverity.Location = New System.Drawing.Point(428, 36)
        Me.cboActSeverity.Name = "cboActSeverity"
        Me.cboActSeverity.Size = New System.Drawing.Size(170, 25)
        Me.cboActSeverity.TabIndex = 4
        '
        'lblActSearch
        '
        Me.lblActSearch.AutoSize = True
        Me.lblActSearch.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.lblActSearch.Location = New System.Drawing.Point(610, 12)
        Me.lblActSearch.Name = "lblActSearch"
        Me.lblActSearch.Size = New System.Drawing.Size(45, 17)
        Me.lblActSearch.TabIndex = 0
        Me.lblActSearch.Text = "Search"
        '
        'txtActSearch
        '
        Me.txtActSearch.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.txtActSearch.Location = New System.Drawing.Point(610, 36)
        Me.txtActSearch.MaxLength = 100
        Me.txtActSearch.Name = "txtActSearch"
        Me.txtActSearch.Size = New System.Drawing.Size(180, 25)
        Me.txtActSearch.TabIndex = 5
        '
        'btnActApply
        '
        Me.btnActApply.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnActApply.Location = New System.Drawing.Point(800, 35)
        Me.btnActApply.Name = "btnActApply"
        Me.btnActApply.Size = New System.Drawing.Size(80, 27)
        Me.btnActApply.TabIndex = 6
        Me.btnActApply.Text = "Apply"
        Me.btnActApply.UseVisualStyleBackColor = True
        '
        'btnActClear
        '
        Me.btnActClear.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnActClear.Location = New System.Drawing.Point(888, 35)
        Me.btnActClear.Name = "btnActClear"
        Me.btnActClear.Size = New System.Drawing.Size(95, 27)
        Me.btnActClear.TabIndex = 7
        Me.btnActClear.Text = "Clear filters"
        Me.btnActClear.UseVisualStyleBackColor = True
        '
        'btnActExport
        '
        Me.btnActExport.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnActExport.Location = New System.Drawing.Point(991, 35)
        Me.btnActExport.Name = "btnActExport"
        Me.btnActExport.Size = New System.Drawing.Size(90, 27)
        Me.btnActExport.TabIndex = 8
        Me.btnActExport.Text = "Export"
        Me.btnActExport.UseVisualStyleBackColor = True
        '
        'dgvActivity
        '
        Me.dgvActivity.AllowUserToAddRows = False
        Me.dgvActivity.AllowUserToDeleteRows = False
        Me.dgvActivity.AllowUserToResizeRows = False
        Me.dgvActivity.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dgvActivity.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvActivity.Location = New System.Drawing.Point(12, 74)
        Me.dgvActivity.MultiSelect = False
        Me.dgvActivity.Name = "dgvActivity"
        Me.dgvActivity.ReadOnly = True
        Me.dgvActivity.RowHeadersVisible = False
        Me.dgvActivity.RowTemplate.Height = 28
        Me.dgvActivity.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvActivity.Size = New System.Drawing.Size(1069, 300)
        Me.dgvActivity.TabIndex = 9
        '
        'lblGridCount
        '
        Me.lblGridCount.AutoSize = True
        Me.lblGridCount.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.lblGridCount.Location = New System.Drawing.Point(12, 382)
        Me.lblGridCount.Name = "lblGridCount"
        Me.lblGridCount.Size = New System.Drawing.Size(0, 17)
        Me.lblGridCount.TabIndex = 0
        Me.lblGridCount.Text = ""
        '
        'lblSalesFrom
        '
        Me.lblSalesFrom.AutoSize = True
        Me.lblSalesFrom.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.lblSalesFrom.Location = New System.Drawing.Point(12, 12)
        Me.lblSalesFrom.Name = "lblSalesFrom"
        Me.lblSalesFrom.Size = New System.Drawing.Size(37, 17)
        Me.lblSalesFrom.TabIndex = 0
        Me.lblSalesFrom.Text = "From"
        '
        'dtpSalesFrom
        '
        Me.dtpSalesFrom.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.dtpSalesFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short
        Me.dtpSalesFrom.Location = New System.Drawing.Point(12, 36)
        Me.dtpSalesFrom.Name = "dtpSalesFrom"
        Me.dtpSalesFrom.Size = New System.Drawing.Size(120, 25)
        Me.dtpSalesFrom.TabIndex = 1
        '
        'lblSalesTo
        '
        Me.lblSalesTo.AutoSize = True
        Me.lblSalesTo.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.lblSalesTo.Location = New System.Drawing.Point(144, 12)
        Me.lblSalesTo.Name = "lblSalesTo"
        Me.lblSalesTo.Size = New System.Drawing.Size(22, 17)
        Me.lblSalesTo.TabIndex = 2
        Me.lblSalesTo.Text = "To"
        '
        'dtpSalesTo
        '
        Me.dtpSalesTo.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.dtpSalesTo.Format = System.Windows.Forms.DateTimePickerFormat.Short
        Me.dtpSalesTo.Location = New System.Drawing.Point(144, 36)
        Me.dtpSalesTo.Name = "dtpSalesTo"
        Me.dtpSalesTo.Size = New System.Drawing.Size(120, 25)
        Me.dtpSalesTo.TabIndex = 3
        '
        'lblSubSalesHint
        '
        Me.lblSubSalesHint.AutoSize = True
        Me.lblSubSalesHint.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.lblSubSalesHint.Location = New System.Drawing.Point(288, 42)
        Me.lblSubSalesHint.Name = "lblSubSalesHint"
        Me.lblSubSalesHint.Size = New System.Drawing.Size(0, 13)
        Me.lblSubSalesHint.TabIndex = 4
        Me.lblSubSalesHint.Text = "Double click a sale to open it in the booking search"
        '
        'btnSalesExport
        '
        Me.btnSalesExport.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnSalesExport.Location = New System.Drawing.Point(991, 35)
        Me.btnSalesExport.Name = "btnSalesExport"
        Me.btnSalesExport.Size = New System.Drawing.Size(90, 27)
        Me.btnSalesExport.TabIndex = 5
        Me.btnSalesExport.Text = "Export"
        Me.btnSalesExport.UseVisualStyleBackColor = True
        '
        'dgvMySales
        '
        Me.dgvMySales.AllowUserToAddRows = False
        Me.dgvMySales.AllowUserToDeleteRows = False
        Me.dgvMySales.AllowUserToResizeRows = False
        Me.dgvMySales.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dgvMySales.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvMySales.Location = New System.Drawing.Point(12, 74)
        Me.dgvMySales.MultiSelect = False
        Me.dgvMySales.Name = "dgvMySales"
        Me.dgvMySales.ReadOnly = True
        Me.dgvMySales.RowHeadersVisible = False
        Me.dgvMySales.RowTemplate.Height = 28
        Me.dgvMySales.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvMySales.Size = New System.Drawing.Size(1069, 300)
        Me.dgvMySales.TabIndex = 6
        '
        'lblSalesCount
        '
        Me.lblSalesCount.AutoSize = True
        Me.lblSalesCount.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.lblSalesCount.Location = New System.Drawing.Point(12, 382)
        Me.lblSalesCount.Name = "lblSalesCount"
        Me.lblSalesCount.Size = New System.Drawing.Size(0, 17)
        Me.lblSalesCount.TabIndex = 7
        '
        'lblSubSettingsHint
        '
        Me.lblSubSettingsHint.AutoSize = True
        Me.lblSubSettingsHint.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.lblSubSettingsHint.Location = New System.Drawing.Point(12, 14)
        Me.lblSubSettingsHint.Name = "lblSubSettingsHint"
        Me.lblSubSettingsHint.Size = New System.Drawing.Size(0, 17)
        Me.lblSubSettingsHint.TabIndex = 0
        Me.lblSubSettingsHint.Text = "These are the settings Filmtopia is remembering for you. Your password and colour scheme are changed on the Settings screen."
        '
        'dgvMySettings
        '
        Me.dgvMySettings.AllowUserToAddRows = False
        Me.dgvMySettings.AllowUserToDeleteRows = False
        Me.dgvMySettings.AllowUserToResizeRows = False
        Me.dgvMySettings.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dgvMySettings.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvMySettings.Location = New System.Drawing.Point(12, 46)
        Me.dgvMySettings.MultiSelect = False
        Me.dgvMySettings.Name = "dgvMySettings"
        Me.dgvMySettings.ReadOnly = True
        Me.dgvMySettings.RowHeadersVisible = False
        Me.dgvMySettings.RowTemplate.Height = 28
        Me.dgvMySettings.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvMySettings.Size = New System.Drawing.Size(640, 260)
        Me.dgvMySettings.TabIndex = 1
        '
        'btnResetMySettings
        '
        Me.btnResetMySettings.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnResetMySettings.Location = New System.Drawing.Point(12, 320)
        Me.btnResetMySettings.Name = "btnResetMySettings"
        Me.btnResetMySettings.Size = New System.Drawing.Size(210, 30)
        Me.btnResetMySettings.TabIndex = 2
        Me.btnResetMySettings.Text = "Put my settings back to default"
        Me.btnResetMySettings.UseVisualStyleBackColor = True
        '
        'lblSubResetHint
        '
        Me.lblSubResetHint.AutoSize = True
        Me.lblSubResetHint.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.lblSubResetHint.Location = New System.Drawing.Point(232, 328)
        Me.lblSubResetHint.Name = "lblSubResetHint"
        Me.lblSubResetHint.Size = New System.Drawing.Size(0, 13)
        Me.lblSubResetHint.TabIndex = 3
        Me.lblSubResetHint.Text = "This only affects you. Nobody elses settings are touched."
        '
        'lblVersion
        '
        Me.lblVersion.AutoSize = True
        Me.lblVersion.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.lblVersion.Location = New System.Drawing.Point(16, 748)
        Me.lblVersion.Name = "lblVersion"
        Me.lblVersion.Size = New System.Drawing.Size(240, 13)
        Me.lblVersion.TabIndex = 10
        Me.lblVersion.Text = "Filmtopia Cinema Management System  v1.0"
        '
        'frmUserOverview
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1135, 775)
        Me.Controls.Add(Me.lblVersion)
        Me.Controls.Add(Me.tabMe)
        Me.Controls.Add(Me.pnlCard4)
        Me.Controls.Add(Me.pnlCard3)
        Me.Controls.Add(Me.pnlCard2)
        Me.Controls.Add(Me.pnlCard1)
        Me.Controls.Add(Me.lblSubSession)
        Me.Controls.Add(Me.lblSubLastLogin)
        Me.Controls.Add(Me.lblRole)
        Me.Controls.Add(Me.lblWelcome)
        Me.Controls.Add(Me.lblHeading)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.MaximizeBox = False
        Me.Name = "frmUserOverview"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "My Account"
        Me.pnlCard1.ResumeLayout(False)
        Me.pnlCard1.PerformLayout()
        Me.pnlCard2.ResumeLayout(False)
        Me.pnlCard2.PerformLayout()
        Me.pnlCard3.ResumeLayout(False)
        Me.pnlCard3.PerformLayout()
        Me.pnlCard4.ResumeLayout(False)
        Me.pnlCard4.PerformLayout()
        Me.tabSettings.ResumeLayout(False)
        Me.tabSettings.PerformLayout()
        CType(Me.dgvMySettings, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabSales.ResumeLayout(False)
        Me.tabSales.PerformLayout()
        CType(Me.dgvMySales, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabActivity.ResumeLayout(False)
        Me.tabActivity.PerformLayout()
        CType(Me.dgvActivity, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabMe.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblHeading As Label
    Friend WithEvents lblWelcome As Label
    Friend WithEvents lblRole As Label
    Friend WithEvents lblSubLastLogin As Label
    Friend WithEvents lblSubSession As Label
    Friend WithEvents pnlCard1 As Panel
    Friend WithEvents lblCardTitle1 As Label
    Friend WithEvents lblStat1 As Label
    Friend WithEvents lblCardSub1 As Label
    Friend WithEvents pnlAccent1 As Panel
    Friend WithEvents pnlCard2 As Panel
    Friend WithEvents lblCardTitle2 As Label
    Friend WithEvents lblStat2 As Label
    Friend WithEvents lblCardSub2 As Label
    Friend WithEvents pnlAccent2 As Panel
    Friend WithEvents pnlCard3 As Panel
    Friend WithEvents lblCardTitle3 As Label
    Friend WithEvents lblStat3 As Label
    Friend WithEvents lblCardSub3 As Label
    Friend WithEvents pnlAccent3 As Panel
    Friend WithEvents pnlCard4 As Panel
    Friend WithEvents lblCardTitle4 As Label
    Friend WithEvents lblStat4 As Label
    Friend WithEvents lblCardSub4 As Label
    Friend WithEvents pnlAccent4 As Panel
    Friend WithEvents tabMe As TabControl
    Friend WithEvents tabActivity As TabPage
    Friend WithEvents tabSales As TabPage
    Friend WithEvents tabSettings As TabPage
    Friend WithEvents lblActFrom As Label
    Friend WithEvents dtpActFrom As DateTimePicker
    Friend WithEvents lblActTo As Label
    Friend WithEvents dtpActTo As DateTimePicker
    Friend WithEvents lblActArea As Label
    Friend WithEvents cboActType As ComboBox
    Friend WithEvents lblActLevel As Label
    Friend WithEvents cboActSeverity As ComboBox
    Friend WithEvents lblActSearch As Label
    Friend WithEvents txtActSearch As TextBox
    Friend WithEvents btnActApply As Button
    Friend WithEvents btnActClear As Button
    Friend WithEvents btnActExport As Button
    Friend WithEvents dgvActivity As DataGridView
    Friend WithEvents lblGridCount As Label
    Friend WithEvents lblSalesFrom As Label
    Friend WithEvents dtpSalesFrom As DateTimePicker
    Friend WithEvents lblSalesTo As Label
    Friend WithEvents dtpSalesTo As DateTimePicker
    Friend WithEvents lblSubSalesHint As Label
    Friend WithEvents btnSalesExport As Button
    Friend WithEvents dgvMySales As DataGridView
    Friend WithEvents lblSalesCount As Label
    Friend WithEvents lblSubSettingsHint As Label
    Friend WithEvents dgvMySettings As DataGridView
    Friend WithEvents btnResetMySettings As Button
    Friend WithEvents lblSubResetHint As Label
    Friend WithEvents lblVersion As Label
End Class
