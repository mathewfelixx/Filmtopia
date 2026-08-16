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
        Me.lblSubSecurity = New System.Windows.Forms.Label()
        Me.btnGoPassword = New System.Windows.Forms.Button()
        Me.lblAppearance = New System.Windows.Forms.Label()
        Me.rdoLight = New System.Windows.Forms.RadioButton()
        Me.rdoDark = New System.Windows.Forms.RadioButton()
        Me.pnlAccentDivider = New System.Windows.Forms.Panel()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.lblCurrent = New System.Windows.Forms.Label()
        Me.txtCurrentPW = New System.Windows.Forms.TextBox()
        Me.lblNew = New System.Windows.Forms.Label()
        Me.txtNewPW = New System.Windows.Forms.TextBox()
        Me.lblConfirm = New System.Windows.Forms.Label()
        Me.txtConfirmPW = New System.Windows.Forms.TextBox()
        Me.btnChangePassword = New System.Windows.Forms.Button()
        Me.lblSubPWHelp = New System.Windows.Forms.Label()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
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
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
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
        Me.lblRole.Location = New System.Drawing.Point(16, 76)
        Me.lblRole.Name = "lblRole"
        Me.lblRole.Size = New System.Drawing.Size(0, 17)
        Me.lblRole.TabIndex = 2
        '
        'lblSubLastLogin
        '
        Me.lblSubLastLogin.AutoSize = True
        Me.lblSubLastLogin.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.lblSubLastLogin.Location = New System.Drawing.Point(16, 118)
        Me.lblSubLastLogin.Name = "lblSubLastLogin"
        Me.lblSubLastLogin.Size = New System.Drawing.Size(0, 13)
        Me.lblSubLastLogin.TabIndex = 3
        '
        'lblSubSession
        '
        Me.lblSubSession.AutoSize = True
        Me.lblSubSession.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.lblSubSession.Location = New System.Drawing.Point(16, 100)
        Me.lblSubSession.Name = "lblSubSession"
        Me.lblSubSession.Size = New System.Drawing.Size(0, 13)
        Me.lblSubSession.TabIndex = 4
        '
        'lblSubSecurity
        '
        Me.lblSubSecurity.AutoSize = True
        Me.lblSubSecurity.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.lblSubSecurity.Location = New System.Drawing.Point(16, 136)
        Me.lblSubSecurity.Name = "lblSubSecurity"
        Me.lblSubSecurity.Size = New System.Drawing.Size(0, 13)
        Me.lblSubSecurity.TabIndex = 5
        '
        'btnGoPassword
        '
        Me.btnGoPassword.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnGoPassword.Location = New System.Drawing.Point(949, 48)
        Me.btnGoPassword.Name = "btnGoPassword"
        Me.btnGoPassword.Size = New System.Drawing.Size(170, 30)
        Me.btnGoPassword.TabIndex = 6
        Me.btnGoPassword.Text = "Change my password"
        Me.btnGoPassword.UseVisualStyleBackColor = True
        '
        'lblAppearance
        '
        Me.lblAppearance.AutoSize = True
        Me.lblAppearance.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lblAppearance.Location = New System.Drawing.Point(846, 92)
        Me.lblAppearance.Name = "lblAppearance"
        Me.lblAppearance.Size = New System.Drawing.Size(75, 15)
        Me.lblAppearance.TabIndex = 7
        Me.lblAppearance.Text = "Appearance"
        '
        'rdoLight
        '
        Me.rdoLight.AutoSize = True
        Me.rdoLight.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.rdoLight.Location = New System.Drawing.Point(933, 90)
        Me.rdoLight.Name = "rdoLight"
        Me.rdoLight.Size = New System.Drawing.Size(53, 19)
        Me.rdoLight.TabIndex = 8
        Me.rdoLight.TabStop = True
        Me.rdoLight.Text = "Light"
        Me.rdoLight.UseVisualStyleBackColor = True
        '
        'rdoDark
        '
        Me.rdoDark.AutoSize = True
        Me.rdoDark.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.rdoDark.Location = New System.Drawing.Point(1010, 90)
        Me.rdoDark.Name = "rdoDark"
        Me.rdoDark.Size = New System.Drawing.Size(52, 19)
        Me.rdoDark.TabIndex = 9
        Me.rdoDark.TabStop = True
        Me.rdoDark.Text = "Dark"
        Me.rdoDark.UseVisualStyleBackColor = True
        '
        'pnlAccentDivider
        '
        Me.pnlAccentDivider.Location = New System.Drawing.Point(16, 156)
        Me.pnlAccentDivider.Name = "pnlAccentDivider"
        Me.pnlAccentDivider.Size = New System.Drawing.Size(1103, 1)
        Me.pnlAccentDivider.TabIndex = 10
        '
        'tabMe
        '
        Me.tabMe.Controls.Add(Me.tabActivity)
        Me.tabMe.Controls.Add(Me.tabSales)
        Me.tabMe.Controls.Add(Me.tabSettings)
        Me.tabMe.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.tabMe.Location = New System.Drawing.Point(16, 168)
        Me.tabMe.Name = "tabMe"
        Me.tabMe.SelectedIndex = 0
        Me.tabMe.Size = New System.Drawing.Size(1103, 572)
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
        Me.tabActivity.Size = New System.Drawing.Size(1095, 544)
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
        Me.tabSales.Size = New System.Drawing.Size(1095, 544)
        Me.tabSales.TabIndex = 1
        Me.tabSales.Text = "Sales I have taken"
        Me.tabSales.UseVisualStyleBackColor = True
        '
        'tabSettings
        '
        Me.tabSettings.Controls.Add(Me.GroupBox1)
        Me.tabSettings.Controls.Add(Me.GroupBox2)
        Me.tabSettings.Location = New System.Drawing.Point(4, 24)
        Me.tabSettings.Name = "tabSettings"
        Me.tabSettings.Padding = New System.Windows.Forms.Padding(3)
        Me.tabSettings.Size = New System.Drawing.Size(1095, 544)
        Me.tabSettings.TabIndex = 2
        Me.tabSettings.Text = "My password and settings"
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
        Me.dgvActivity.Size = New System.Drawing.Size(1069, 430)
        Me.dgvActivity.TabIndex = 9
        '
        'lblGridCount
        '
        Me.lblGridCount.AutoSize = True
        Me.lblGridCount.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.lblGridCount.Location = New System.Drawing.Point(12, 512)
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
        Me.dgvMySales.Size = New System.Drawing.Size(1069, 430)
        Me.dgvMySales.TabIndex = 6
        '
        'lblSalesCount
        '
        Me.lblSalesCount.AutoSize = True
        Me.lblSalesCount.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.lblSalesCount.Location = New System.Drawing.Point(12, 512)
        Me.lblSalesCount.Name = "lblSalesCount"
        Me.lblSalesCount.Size = New System.Drawing.Size(0, 17)
        Me.lblSalesCount.TabIndex = 7
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.lblCurrent)
        Me.GroupBox1.Controls.Add(Me.txtCurrentPW)
        Me.GroupBox1.Controls.Add(Me.lblNew)
        Me.GroupBox1.Controls.Add(Me.txtNewPW)
        Me.GroupBox1.Controls.Add(Me.lblConfirm)
        Me.GroupBox1.Controls.Add(Me.txtConfirmPW)
        Me.GroupBox1.Controls.Add(Me.btnChangePassword)
        Me.GroupBox1.Controls.Add(Me.lblSubPWHelp)
        Me.GroupBox1.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(520, 170)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Change my password"
        '
        'lblCurrent
        '
        Me.lblCurrent.AutoSize = True
        Me.lblCurrent.Location = New System.Drawing.Point(14, 34)
        Me.lblCurrent.Name = "lblCurrent"
        Me.lblCurrent.Size = New System.Drawing.Size(107, 15)
        Me.lblCurrent.TabIndex = 0
        Me.lblCurrent.Text = "Current password"
        '
        'txtCurrentPW
        '
        Me.txtCurrentPW.Location = New System.Drawing.Point(166, 31)
        Me.txtCurrentPW.Name = "txtCurrentPW"
        Me.txtCurrentPW.Size = New System.Drawing.Size(200, 23)
        Me.txtCurrentPW.TabIndex = 1
        Me.txtCurrentPW.UseSystemPasswordChar = True
        '
        'lblNew
        '
        Me.lblNew.AutoSize = True
        Me.lblNew.Location = New System.Drawing.Point(14, 66)
        Me.lblNew.Name = "lblNew"
        Me.lblNew.Size = New System.Drawing.Size(88, 15)
        Me.lblNew.TabIndex = 2
        Me.lblNew.Text = "New password"
        '
        'txtNewPW
        '
        Me.txtNewPW.Location = New System.Drawing.Point(166, 63)
        Me.txtNewPW.Name = "txtNewPW"
        Me.txtNewPW.Size = New System.Drawing.Size(200, 23)
        Me.txtNewPW.TabIndex = 3
        Me.txtNewPW.UseSystemPasswordChar = True
        '
        'lblConfirm
        '
        Me.lblConfirm.AutoSize = True
        Me.lblConfirm.Location = New System.Drawing.Point(14, 98)
        Me.lblConfirm.Name = "lblConfirm"
        Me.lblConfirm.Size = New System.Drawing.Size(137, 15)
        Me.lblConfirm.TabIndex = 4
        Me.lblConfirm.Text = "Confirm new password"
        '
        'txtConfirmPW
        '
        Me.txtConfirmPW.Location = New System.Drawing.Point(166, 95)
        Me.txtConfirmPW.Name = "txtConfirmPW"
        Me.txtConfirmPW.Size = New System.Drawing.Size(200, 23)
        Me.txtConfirmPW.TabIndex = 5
        Me.txtConfirmPW.UseSystemPasswordChar = True
        '
        'btnChangePassword
        '
        Me.btnChangePassword.Location = New System.Drawing.Point(166, 128)
        Me.btnChangePassword.Name = "btnChangePassword"
        Me.btnChangePassword.Size = New System.Drawing.Size(140, 30)
        Me.btnChangePassword.TabIndex = 6
        Me.btnChangePassword.Text = "Change password"
        Me.btnChangePassword.UseVisualStyleBackColor = True
        '
        'lblSubPWHelp
        '
        Me.lblSubPWHelp.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.lblSubPWHelp.Location = New System.Drawing.Point(378, 31)
        Me.lblSubPWHelp.Name = "lblSubPWHelp"
        Me.lblSubPWHelp.Size = New System.Drawing.Size(130, 90)
        Me.lblSubPWHelp.TabIndex = 7
        Me.lblSubPWHelp.Text = "At least 6 characters, and not the one you are using now."
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.dgvMySettings)
        Me.GroupBox2.Controls.Add(Me.btnResetMySettings)
        Me.GroupBox2.Controls.Add(Me.lblSubResetHint)
        Me.GroupBox2.Controls.Add(Me.lblSubSettingsHint)
        Me.GroupBox2.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.GroupBox2.Location = New System.Drawing.Point(12, 194)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(520, 338)
        Me.GroupBox2.TabIndex = 1
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "What Filmtopia remembers about me"
        '
        'lblSubSettingsHint
        '
        Me.lblSubSettingsHint.AutoSize = True
        Me.lblSubSettingsHint.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.lblSubSettingsHint.Location = New System.Drawing.Point(14, 26)
        Me.lblSubSettingsHint.Name = "lblSubSettingsHint"
        Me.lblSubSettingsHint.Size = New System.Drawing.Size(0, 13)
        Me.lblSubSettingsHint.TabIndex = 0
        Me.lblSubSettingsHint.Text = "These are the choices the screens are keeping for you."
        '
        'dgvMySettings
        '
        Me.dgvMySettings.AllowUserToAddRows = False
        Me.dgvMySettings.AllowUserToDeleteRows = False
        Me.dgvMySettings.AllowUserToResizeRows = False
        Me.dgvMySettings.BackgroundColor = System.Drawing.Color.White
        Me.dgvMySettings.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.dgvMySettings.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvMySettings.Location = New System.Drawing.Point(14, 48)
        Me.dgvMySettings.MultiSelect = False
        Me.dgvMySettings.Name = "dgvMySettings"
        Me.dgvMySettings.ReadOnly = True
        Me.dgvMySettings.RowHeadersVisible = False
        Me.dgvMySettings.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvMySettings.Size = New System.Drawing.Size(492, 200)
        Me.dgvMySettings.TabIndex = 1
        '
        'btnResetMySettings
        '
        Me.btnResetMySettings.Location = New System.Drawing.Point(14, 262)
        Me.btnResetMySettings.Name = "btnResetMySettings"
        Me.btnResetMySettings.Size = New System.Drawing.Size(230, 30)
        Me.btnResetMySettings.TabIndex = 2
        Me.btnResetMySettings.Text = "Put my settings back to default"
        Me.btnResetMySettings.UseVisualStyleBackColor = True
        '
        'lblSubResetHint
        '
        Me.lblSubResetHint.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.lblSubResetHint.Location = New System.Drawing.Point(14, 298)
        Me.lblSubResetHint.Name = "lblSubResetHint"
        Me.lblSubResetHint.Size = New System.Drawing.Size(492, 26)
        Me.lblSubResetHint.TabIndex = 3
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
        Me.Controls.Add(Me.pnlAccentDivider)
        Me.Controls.Add(Me.rdoDark)
        Me.Controls.Add(Me.rdoLight)
        Me.Controls.Add(Me.lblAppearance)
        Me.Controls.Add(Me.btnGoPassword)
        Me.Controls.Add(Me.lblSubSecurity)
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
        Me.tabSettings.ResumeLayout(False)
        Me.tabSettings.PerformLayout()
        CType(Me.dgvMySettings, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabSales.ResumeLayout(False)
        Me.tabSales.PerformLayout()
        CType(Me.dgvMySales, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabActivity.ResumeLayout(False)
        Me.tabActivity.PerformLayout()
        CType(Me.dgvActivity, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.tabMe.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblHeading As Label
    Friend WithEvents lblWelcome As Label
    Friend WithEvents lblRole As Label
    Friend WithEvents lblSubLastLogin As Label
    Friend WithEvents lblSubSession As Label
    Friend WithEvents lblSubSecurity As Label
    Friend WithEvents btnGoPassword As Button
    Friend WithEvents lblAppearance As Label
    Friend WithEvents rdoLight As RadioButton
    Friend WithEvents rdoDark As RadioButton
    Friend WithEvents pnlAccentDivider As Panel
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents lblCurrent As Label
    Friend WithEvents txtCurrentPW As TextBox
    Friend WithEvents lblNew As Label
    Friend WithEvents txtNewPW As TextBox
    Friend WithEvents lblConfirm As Label
    Friend WithEvents txtConfirmPW As TextBox
    Friend WithEvents btnChangePassword As Button
    Friend WithEvents lblSubPWHelp As Label
    Friend WithEvents GroupBox2 As GroupBox
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
