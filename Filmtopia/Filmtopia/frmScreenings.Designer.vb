<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmScreenings
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
        Me.lblShow = New System.Windows.Forms.Label()
        Me.cboShow = New System.Windows.Forms.ComboBox()
        Me.lblScreenFilter = New System.Windows.Forms.Label()
        Me.cboScreenFilter = New System.Windows.Forms.ComboBox()
        Me.lblSearch = New System.Windows.Forms.Label()
        Me.txtSearch = New System.Windows.Forms.TextBox()
        Me.timerSearch = New System.Windows.Forms.Timer(Me.components)
        Me.lblGridCount = New System.Windows.Forms.Label()
        Me.tabView = New System.Windows.Forms.TabControl()
        Me.tabGrid = New System.Windows.Forms.TabPage()
        Me.dgvScreenings = New System.Windows.Forms.DataGridView()
        Me.tabTimeline = New System.Windows.Forms.TabPage()
        Me.lblTimelineDate = New System.Windows.Forms.Label()
        Me.dtpTimelineDate = New System.Windows.Forms.DateTimePicker()
        Me.lblTimelineCount = New System.Windows.Forms.Label()
        Me.lblTimelineScreen = New System.Windows.Forms.Label()
        Me.cboTimelineScreen = New System.Windows.Forms.ComboBox()
        Me.dgvTimeline = New System.Windows.Forms.DataGridView()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.lblFilm = New System.Windows.Forms.Label()
        Me.cboFilm = New System.Windows.Forms.ComboBox()
        Me.lblScreen = New System.Windows.Forms.Label()
        Me.cboScreen = New System.Windows.Forms.ComboBox()
        Me.lblDate = New System.Windows.Forms.Label()
        Me.dtpScreeningDate = New System.Windows.Forms.DateTimePicker()
        Me.lblTime = New System.Windows.Forms.Label()
        Me.txtScreeningTime = New System.Windows.Forms.TextBox()
        Me.lblPrice = New System.Windows.Forms.Label()
        Me.txtTicketPrice = New System.Windows.Forms.TextBox()
        Me.lblEndsAt = New System.Windows.Forms.Label()
        Me.btnAdd = New System.Windows.Forms.Button()
        Me.btnUpdate = New System.Windows.Forms.Button()
        Me.btnDelete = New System.Windows.Forms.Button()
        Me.btnClear = New System.Windows.Forms.Button()
        Me.btnSuggest = New System.Windows.Forms.Button()
        Me.lblCancelReason = New System.Windows.Forms.Label()
        Me.txtCancelReason = New System.Windows.Forms.TextBox()
        Me.btnCancelScreening = New System.Windows.Forms.Button()
        Me.lblRepeatUntil = New System.Windows.Forms.Label()
        Me.dtpRepeatUntil = New System.Windows.Forms.DateTimePicker()
        Me.btnRepeat = New System.Windows.Forms.Button()
        Me.btnFillDay = New System.Windows.Forms.Button()
        Me.btnExport = New System.Windows.Forms.Button()
        Me.lblSaved = New System.Windows.Forms.Label()
        Me.lblVersion = New System.Windows.Forms.Label()
        Me.GroupBox1.SuspendLayout()
        Me.tabView.SuspendLayout()
        Me.tabGrid.SuspendLayout()
        Me.tabTimeline.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        CType(Me.dgvScreenings, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvTimeline, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblHeading
        '
        Me.lblHeading.AutoSize = True
        Me.lblHeading.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Bold)
        Me.lblHeading.Location = New System.Drawing.Point(16, 12)
        Me.lblHeading.Name = "lblHeading"
        Me.lblHeading.Size = New System.Drawing.Size(130, 25)
        Me.lblHeading.TabIndex = 0
        Me.lblHeading.Text = "Screenings"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.lblShow)
        Me.GroupBox1.Controls.Add(Me.cboShow)
        Me.GroupBox1.Controls.Add(Me.lblScreenFilter)
        Me.GroupBox1.Controls.Add(Me.cboScreenFilter)
        Me.GroupBox1.Controls.Add(Me.lblSearch)
        Me.GroupBox1.Controls.Add(Me.txtSearch)
        Me.GroupBox1.Controls.Add(Me.lblGridCount)
        Me.GroupBox1.Controls.Add(Me.tabView)
        Me.GroupBox1.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.GroupBox1.Location = New System.Drawing.Point(16, 46)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(1036, 400)
        Me.GroupBox1.TabIndex = 1
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "What is scheduled"
        '
        'lblShow
        '
        Me.lblShow.AutoSize = True
        Me.lblShow.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lblShow.Location = New System.Drawing.Point(16, 28)
        Me.lblShow.Name = "lblShow"
        Me.lblShow.Size = New System.Drawing.Size(35, 15)
        Me.lblShow.TabIndex = 0
        Me.lblShow.Text = "Show"
        '
        'cboShow
        '
        Me.cboShow.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboShow.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.cboShow.Location = New System.Drawing.Point(78, 25)
        Me.cboShow.Name = "cboShow"
        Me.cboShow.Size = New System.Drawing.Size(150, 23)
        Me.cboShow.TabIndex = 1
        '
        'lblScreenFilter
        '
        Me.lblScreenFilter.AutoSize = True
        Me.lblScreenFilter.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lblScreenFilter.Location = New System.Drawing.Point(244, 28)
        Me.lblScreenFilter.Name = "lblScreenFilter"
        Me.lblScreenFilter.Size = New System.Drawing.Size(55, 15)
        Me.lblScreenFilter.TabIndex = 2
        Me.lblScreenFilter.Text = "Screen"
        '
        'cboScreenFilter
        '
        Me.cboScreenFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboScreenFilter.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.cboScreenFilter.Location = New System.Drawing.Point(314, 25)
        Me.cboScreenFilter.Name = "cboScreenFilter"
        Me.cboScreenFilter.Size = New System.Drawing.Size(140, 23)
        Me.cboScreenFilter.TabIndex = 3
        '
        'lblSearch
        '
        Me.lblSearch.AutoSize = True
        Me.lblSearch.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lblSearch.Location = New System.Drawing.Point(470, 28)
        Me.lblSearch.Name = "lblSearch"
        Me.lblSearch.Size = New System.Drawing.Size(55, 15)
        Me.lblSearch.TabIndex = 4
        Me.lblSearch.Text = "Search"
        '
        'txtSearch
        '
        Me.txtSearch.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.txtSearch.Location = New System.Drawing.Point(540, 25)
        Me.txtSearch.Name = "txtSearch"
        Me.txtSearch.Size = New System.Drawing.Size(170, 23)
        Me.txtSearch.TabIndex = 5
        '
        'timerSearch
        '
        Me.timerSearch.Interval = 300
        '
        'lblGridCount
        '
        Me.lblGridCount.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lblGridCount.Location = New System.Drawing.Point(726, 28)
        Me.lblGridCount.Name = "lblGridCount"
        Me.lblGridCount.Size = New System.Drawing.Size(294, 17)
        Me.lblGridCount.TabIndex = 6
        Me.lblGridCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'dgvScreenings
        '
        Me.dgvScreenings.AllowUserToAddRows = False
        Me.dgvScreenings.AllowUserToDeleteRows = False
        Me.dgvScreenings.AllowUserToResizeRows = False
        Me.dgvScreenings.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvScreenings.Location = New System.Drawing.Point(6, 6)
        Me.dgvScreenings.MultiSelect = False
        Me.dgvScreenings.Name = "dgvScreenings"
        Me.dgvScreenings.ReadOnly = True
        Me.dgvScreenings.RowHeadersVisible = False
        Me.dgvScreenings.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvScreenings.Size = New System.Drawing.Size(984, 290)
        Me.dgvScreenings.TabIndex = 0
        '
        'tabTimeline
        '
        Me.tabTimeline.Controls.Add(Me.lblTimelineDate)
        Me.tabTimeline.Controls.Add(Me.dtpTimelineDate)
        Me.tabTimeline.Controls.Add(Me.lblTimelineScreen)
        Me.tabTimeline.Controls.Add(Me.cboTimelineScreen)
        Me.tabTimeline.Controls.Add(Me.lblTimelineCount)
        Me.tabTimeline.Controls.Add(Me.dgvTimeline)
        Me.tabTimeline.Location = New System.Drawing.Point(4, 24)
        Me.tabTimeline.Name = "tabTimeline"
        Me.tabTimeline.Padding = New System.Windows.Forms.Padding(3)
        Me.tabTimeline.Size = New System.Drawing.Size(996, 302)
        Me.tabTimeline.TabIndex = 1
        Me.tabTimeline.Text = "Day timeline"
        Me.tabTimeline.UseVisualStyleBackColor = True
        '
        'lblTimelineDate
        '
        Me.lblTimelineDate.AutoSize = True
        Me.lblTimelineDate.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lblTimelineDate.Location = New System.Drawing.Point(8, 12)
        Me.lblTimelineDate.Name = "lblTimelineDate"
        Me.lblTimelineDate.Size = New System.Drawing.Size(45, 15)
        Me.lblTimelineDate.TabIndex = 0
        Me.lblTimelineDate.Text = "Day"
        '
        'dtpTimelineDate
        '
        Me.dtpTimelineDate.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.dtpTimelineDate.Format = System.Windows.Forms.DateTimePickerFormat.Short
        Me.dtpTimelineDate.Location = New System.Drawing.Point(70, 8)
        Me.dtpTimelineDate.Name = "dtpTimelineDate"
        Me.dtpTimelineDate.Size = New System.Drawing.Size(140, 23)
        Me.dtpTimelineDate.TabIndex = 1
        '
        'lblTimelineScreen
        '
        Me.lblTimelineScreen.AutoSize = True
        Me.lblTimelineScreen.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lblTimelineScreen.Location = New System.Drawing.Point(230, 12)
        Me.lblTimelineScreen.Name = "lblTimelineScreen"
        Me.lblTimelineScreen.Size = New System.Drawing.Size(45, 15)
        Me.lblTimelineScreen.TabIndex = 2
        Me.lblTimelineScreen.Text = "Screen"
        '
        'cboTimelineScreen
        '
        Me.cboTimelineScreen.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboTimelineScreen.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.cboTimelineScreen.Location = New System.Drawing.Point(297, 8)
        Me.cboTimelineScreen.Name = "cboTimelineScreen"
        Me.cboTimelineScreen.Size = New System.Drawing.Size(160, 23)
        Me.cboTimelineScreen.TabIndex = 3
        '
        'lblTimelineCount
        '
        Me.lblTimelineCount.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblTimelineCount.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lblTimelineCount.Location = New System.Drawing.Point(736, 12)
        Me.lblTimelineCount.Name = "lblTimelineCount"
        Me.lblTimelineCount.Size = New System.Drawing.Size(250, 17)
        Me.lblTimelineCount.TabIndex = 4
        Me.lblTimelineCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'dgvTimeline
        '
        Me.dgvTimeline.AllowUserToAddRows = False
        Me.dgvTimeline.AllowUserToDeleteRows = False
        Me.dgvTimeline.AllowUserToResizeRows = False
        Me.dgvTimeline.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvTimeline.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvTimeline.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.dgvTimeline.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.dgvTimeline.Location = New System.Drawing.Point(6, 38)
        Me.dgvTimeline.MultiSelect = False
        Me.dgvTimeline.Name = "dgvTimeline"
        Me.dgvTimeline.ReadOnly = True
        Me.dgvTimeline.RowHeadersVisible = False
        Me.dgvTimeline.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvTimeline.Size = New System.Drawing.Size(984, 258)
        Me.dgvTimeline.TabIndex = 5
        '
        'tabView
        '
        Me.tabView.Controls.Add(Me.tabGrid)
        Me.tabView.Controls.Add(Me.tabTimeline)
        Me.tabView.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.tabView.Location = New System.Drawing.Point(16, 58)
        Me.tabView.Name = "tabView"
        Me.tabView.SelectedIndex = 0
        Me.tabView.Size = New System.Drawing.Size(1004, 330)
        Me.tabView.TabIndex = 7
        '
        'tabGrid
        '
        Me.tabGrid.Controls.Add(Me.dgvScreenings)
        Me.tabGrid.Location = New System.Drawing.Point(4, 24)
        Me.tabGrid.Name = "tabGrid"
        Me.tabGrid.Padding = New System.Windows.Forms.Padding(3)
        Me.tabGrid.Size = New System.Drawing.Size(996, 302)
        Me.tabGrid.TabIndex = 0
        Me.tabGrid.Text = "List"
        Me.tabGrid.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.lblStatus)
        Me.GroupBox2.Controls.Add(Me.lblFilm)
        Me.GroupBox2.Controls.Add(Me.cboFilm)
        Me.GroupBox2.Controls.Add(Me.lblScreen)
        Me.GroupBox2.Controls.Add(Me.cboScreen)
        Me.GroupBox2.Controls.Add(Me.lblDate)
        Me.GroupBox2.Controls.Add(Me.dtpScreeningDate)
        Me.GroupBox2.Controls.Add(Me.lblTime)
        Me.GroupBox2.Controls.Add(Me.txtScreeningTime)
        Me.GroupBox2.Controls.Add(Me.lblPrice)
        Me.GroupBox2.Controls.Add(Me.txtTicketPrice)
        Me.GroupBox2.Controls.Add(Me.lblEndsAt)
        Me.GroupBox2.Controls.Add(Me.btnSuggest)
        Me.GroupBox2.Controls.Add(Me.btnAdd)
        Me.GroupBox2.Controls.Add(Me.btnUpdate)
        Me.GroupBox2.Controls.Add(Me.btnDelete)
        Me.GroupBox2.Controls.Add(Me.btnClear)
        Me.GroupBox2.Controls.Add(Me.lblCancelReason)
        Me.GroupBox2.Controls.Add(Me.txtCancelReason)
        Me.GroupBox2.Controls.Add(Me.btnCancelScreening)
        Me.GroupBox2.Controls.Add(Me.lblRepeatUntil)
        Me.GroupBox2.Controls.Add(Me.dtpRepeatUntil)
        Me.GroupBox2.Controls.Add(Me.btnRepeat)
        Me.GroupBox2.Controls.Add(Me.btnFillDay)
        Me.GroupBox2.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.GroupBox2.Location = New System.Drawing.Point(16, 456)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(1036, 294)
        Me.GroupBox2.TabIndex = 2
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Put a film on, or change a screening that is already there"
        '
        'lblStatus
        '
        Me.lblStatus.AutoSize = True
        Me.lblStatus.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lblStatus.Location = New System.Drawing.Point(16, 24)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(170, 19)
        Me.lblStatus.TabIndex = 0
        Me.lblStatus.Text = "Adding a new screening"
        '
        'lblFilm
        '
        Me.lblFilm.AutoSize = True
        Me.lblFilm.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lblFilm.Location = New System.Drawing.Point(16, 61)
        Me.lblFilm.Name = "lblFilm"
        Me.lblFilm.Size = New System.Drawing.Size(28, 15)
        Me.lblFilm.TabIndex = 1
        Me.lblFilm.Text = "Film"
        '
        'cboFilm
        '
        Me.cboFilm.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboFilm.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.cboFilm.Location = New System.Drawing.Point(90, 58)
        Me.cboFilm.Name = "cboFilm"
        Me.cboFilm.Size = New System.Drawing.Size(300, 23)
        Me.cboFilm.TabIndex = 2
        '
        'lblScreen
        '
        Me.lblScreen.AutoSize = True
        Me.lblScreen.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lblScreen.Location = New System.Drawing.Point(410, 61)
        Me.lblScreen.Name = "lblScreen"
        Me.lblScreen.Size = New System.Drawing.Size(45, 15)
        Me.lblScreen.TabIndex = 3
        Me.lblScreen.Text = "Screen"
        '
        'cboScreen
        '
        Me.cboScreen.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboScreen.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.cboScreen.Location = New System.Drawing.Point(490, 58)
        Me.cboScreen.Name = "cboScreen"
        Me.cboScreen.Size = New System.Drawing.Size(180, 23)
        Me.cboScreen.TabIndex = 4
        '
        'lblDate
        '
        Me.lblDate.AutoSize = True
        Me.lblDate.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lblDate.Location = New System.Drawing.Point(16, 99)
        Me.lblDate.Name = "lblDate"
        Me.lblDate.Size = New System.Drawing.Size(31, 15)
        Me.lblDate.TabIndex = 5
        Me.lblDate.Text = "Date"
        '
        'dtpScreeningDate
        '
        Me.dtpScreeningDate.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.dtpScreeningDate.Format = System.Windows.Forms.DateTimePickerFormat.Short
        Me.dtpScreeningDate.Location = New System.Drawing.Point(90, 95)
        Me.dtpScreeningDate.Name = "dtpScreeningDate"
        Me.dtpScreeningDate.Size = New System.Drawing.Size(140, 23)
        Me.dtpScreeningDate.TabIndex = 6
        '
        'lblTime
        '
        Me.lblTime.AutoSize = True
        Me.lblTime.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lblTime.Location = New System.Drawing.Point(250, 99)
        Me.lblTime.Name = "lblTime"
        Me.lblTime.Size = New System.Drawing.Size(80, 15)
        Me.lblTime.TabIndex = 7
        Me.lblTime.Text = "Starts (HH:MM)"
        '
        'txtScreeningTime
        '
        Me.txtScreeningTime.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.txtScreeningTime.Location = New System.Drawing.Point(385, 95)
        Me.txtScreeningTime.MaxLength = 5
        Me.txtScreeningTime.Name = "txtScreeningTime"
        Me.txtScreeningTime.Size = New System.Drawing.Size(70, 23)
        Me.txtScreeningTime.TabIndex = 8
        Me.txtScreeningTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lblPrice
        '
        Me.lblPrice.AutoSize = True
        Me.lblPrice.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lblPrice.Location = New System.Drawing.Point(475, 99)
        Me.lblPrice.Name = "lblPrice"
        Me.lblPrice.Size = New System.Drawing.Size(75, 15)
        Me.lblPrice.TabIndex = 9
        Me.lblPrice.Text = "Ticket price (£)"
        '
        'txtTicketPrice
        '
        Me.txtTicketPrice.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.txtTicketPrice.Location = New System.Drawing.Point(605, 95)
        Me.txtTicketPrice.MaxLength = 6
        Me.txtTicketPrice.Name = "txtTicketPrice"
        Me.txtTicketPrice.Size = New System.Drawing.Size(75, 23)
        Me.txtTicketPrice.TabIndex = 10
        Me.txtTicketPrice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblEndsAt
        '
        Me.lblEndsAt.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lblEndsAt.Location = New System.Drawing.Point(16, 131)
        Me.lblEndsAt.Name = "lblEndsAt"
        Me.lblEndsAt.Size = New System.Drawing.Size(500, 62)
        Me.lblEndsAt.TabIndex = 11
        Me.lblEndsAt.Text = "Pick a film and a start time to see when the screen would be free again"
        '
        'btnSuggest
        '
        Me.btnSuggest.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnSuggest.Location = New System.Drawing.Point(540, 136)
        Me.btnSuggest.Name = "btnSuggest"
        Me.btnSuggest.Size = New System.Drawing.Size(140, 32)
        Me.btnSuggest.TabIndex = 12
        Me.btnSuggest.Text = "Find me a free time"
        Me.btnSuggest.UseVisualStyleBackColor = True
        '
        'btnAdd
        '
        Me.btnAdd.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me.btnAdd.Location = New System.Drawing.Point(700, 58)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(320, 42)
        Me.btnAdd.TabIndex = 13
        Me.btnAdd.Text = "PUT THIS FILM ON"
        Me.btnAdd.UseVisualStyleBackColor = True
        '
        'btnUpdate
        '
        Me.btnUpdate.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnUpdate.Location = New System.Drawing.Point(700, 106)
        Me.btnUpdate.Name = "btnUpdate"
        Me.btnUpdate.Size = New System.Drawing.Size(155, 34)
        Me.btnUpdate.TabIndex = 14
        Me.btnUpdate.Text = "Save changes"
        Me.btnUpdate.UseVisualStyleBackColor = True
        '
        'btnDelete
        '
        Me.btnDelete.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnDelete.Location = New System.Drawing.Point(865, 106)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(155, 34)
        Me.btnDelete.TabIndex = 15
        Me.btnDelete.Text = "Delete screening"
        Me.btnDelete.UseVisualStyleBackColor = True
        '
        'btnClear
        '
        Me.btnClear.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnClear.Location = New System.Drawing.Point(700, 146)
        Me.btnClear.Name = "btnClear"
        Me.btnClear.Size = New System.Drawing.Size(320, 30)
        Me.btnClear.TabIndex = 16
        Me.btnClear.Text = "Clear the boxes"
        Me.btnClear.UseVisualStyleBackColor = True
        '
        'lblCancelReason
        '
        Me.lblCancelReason.AutoSize = True
        Me.lblCancelReason.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lblCancelReason.Location = New System.Drawing.Point(16, 254)
        Me.lblCancelReason.Name = "lblCancelReason"
        Me.lblCancelReason.Size = New System.Drawing.Size(150, 15)
        Me.lblCancelReason.TabIndex = 21
        Me.lblCancelReason.Text = "Reason for pulling it"
        '
        'txtCancelReason
        '
        Me.txtCancelReason.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.txtCancelReason.Location = New System.Drawing.Point(180, 250)
        Me.txtCancelReason.MaxLength = 100
        Me.txtCancelReason.Name = "txtCancelReason"
        Me.txtCancelReason.Size = New System.Drawing.Size(500, 23)
        Me.txtCancelReason.TabIndex = 22
        '
        'btnCancelScreening
        '
        Me.btnCancelScreening.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnCancelScreening.Location = New System.Drawing.Point(700, 248)
        Me.btnCancelScreening.Name = "btnCancelScreening"
        Me.btnCancelScreening.Size = New System.Drawing.Size(320, 30)
        Me.btnCancelScreening.TabIndex = 23
        Me.btnCancelScreening.Text = "Cancel this screening"
        Me.btnCancelScreening.UseVisualStyleBackColor = True
        '
        'lblRepeatUntil
        '
        Me.lblRepeatUntil.AutoSize = True
        Me.lblRepeatUntil.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lblRepeatUntil.Location = New System.Drawing.Point(16, 210)
        Me.lblRepeatUntil.Name = "lblRepeatUntil"
        Me.lblRepeatUntil.Size = New System.Drawing.Size(100, 15)
        Me.lblRepeatUntil.TabIndex = 17
        Me.lblRepeatUntil.Text = "Repeat until"
        '
        'dtpRepeatUntil
        '
        Me.dtpRepeatUntil.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.dtpRepeatUntil.Format = System.Windows.Forms.DateTimePickerFormat.Short
        Me.dtpRepeatUntil.Location = New System.Drawing.Point(130, 206)
        Me.dtpRepeatUntil.Name = "dtpRepeatUntil"
        Me.dtpRepeatUntil.Size = New System.Drawing.Size(140, 23)
        Me.dtpRepeatUntil.TabIndex = 18
        '
        'btnRepeat
        '
        Me.btnRepeat.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnRepeat.Location = New System.Drawing.Point(290, 206)
        Me.btnRepeat.Name = "btnRepeat"
        Me.btnRepeat.Size = New System.Drawing.Size(230, 30)
        Me.btnRepeat.TabIndex = 19
        Me.btnRepeat.Text = "Put it on every day up to then"
        Me.btnRepeat.UseVisualStyleBackColor = True
        '
        'btnFillDay
        '
        Me.btnFillDay.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnFillDay.Location = New System.Drawing.Point(540, 206)
        Me.btnFillDay.Name = "btnFillDay"
        Me.btnFillDay.Size = New System.Drawing.Size(230, 30)
        Me.btnFillDay.TabIndex = 20
        Me.btnFillDay.Text = "Fill the rest of that day with it"
        Me.btnFillDay.UseVisualStyleBackColor = True
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(910, 10)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(140, 28)
        Me.btnExport.Text = "Export to CSV"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'lblSaved
        '
        Me.lblSaved.AutoSize = False
        Me.lblSaved.Location = New System.Drawing.Point(360, 763)
        Me.lblSaved.Name = "lblSaved"
        Me.lblSaved.Size = New System.Drawing.Size(692, 16)
        Me.lblSaved.Text = ""
        '
        'lblVersion
        '
        Me.lblVersion.AutoSize = True
        Me.lblVersion.ForeColor = System.Drawing.Color.Gray
        Me.lblVersion.Location = New System.Drawing.Point(16, 763)
        Me.lblVersion.Name = "lblVersion"
        Me.lblVersion.Size = New System.Drawing.Size(271, 16)
        Me.lblVersion.TabIndex = 3
        Me.lblVersion.Text = "Filmtopia Cinema Management System  v1.0"
        '
        'frmScreenings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1068, 792)
        Me.Controls.Add(Me.btnExport)
        Me.Controls.Add(Me.lblSaved)
        Me.Controls.Add(Me.lblVersion)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.lblHeading)
        Me.Name = "frmScreenings"
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Text = "Screenings"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.tabView.ResumeLayout(False)
        Me.tabGrid.ResumeLayout(False)
        Me.tabTimeline.ResumeLayout(False)
        Me.tabTimeline.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        CType(Me.dgvScreenings, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvTimeline, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblHeading As Label
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents lblShow As Label
    Friend WithEvents cboShow As ComboBox
    Friend WithEvents lblScreenFilter As Label
    Friend WithEvents cboScreenFilter As ComboBox
    Friend WithEvents lblSearch As Label
    Friend WithEvents txtSearch As TextBox
    Friend WithEvents timerSearch As Timer
    Friend WithEvents lblGridCount As Label
    Friend WithEvents tabView As TabControl
    Friend WithEvents tabGrid As TabPage
    Friend WithEvents dgvScreenings As DataGridView
    Friend WithEvents tabTimeline As TabPage
    Friend WithEvents lblTimelineDate As Label
    Friend WithEvents dtpTimelineDate As DateTimePicker
    Friend WithEvents lblTimelineScreen As Label
    Friend WithEvents cboTimelineScreen As ComboBox
    Friend WithEvents lblTimelineCount As Label
    Friend WithEvents dgvTimeline As DataGridView
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents lblStatus As Label
    Friend WithEvents lblFilm As Label
    Friend WithEvents cboFilm As ComboBox
    Friend WithEvents lblScreen As Label
    Friend WithEvents cboScreen As ComboBox
    Friend WithEvents lblDate As Label
    Friend WithEvents dtpScreeningDate As DateTimePicker
    Friend WithEvents lblTime As Label
    Friend WithEvents txtScreeningTime As TextBox
    Friend WithEvents lblPrice As Label
    Friend WithEvents txtTicketPrice As TextBox
    Friend WithEvents lblEndsAt As Label
    Friend WithEvents btnSuggest As Button
    Friend WithEvents btnAdd As Button
    Friend WithEvents btnUpdate As Button
    Friend WithEvents btnDelete As Button
    Friend WithEvents btnClear As Button
    Friend WithEvents lblCancelReason As Label
    Friend WithEvents txtCancelReason As TextBox
    Friend WithEvents btnCancelScreening As Button
    Friend WithEvents lblRepeatUntil As Label
    Friend WithEvents dtpRepeatUntil As DateTimePicker
    Friend WithEvents btnRepeat As Button
    Friend WithEvents btnFillDay As Button
    Friend WithEvents btnExport As Button
    Friend WithEvents lblSaved As Label
    Friend WithEvents lblVersion As Label
End Class
