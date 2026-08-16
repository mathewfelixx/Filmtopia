<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmScreens
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
        Me.lblHeading = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.lblHint = New System.Windows.Forms.Label()
        Me.lblGridCount = New System.Windows.Forms.Label()
        Me.dgvScreens = New System.Windows.Forms.DataGridView()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.lblName = New System.Windows.Forms.Label()
        Me.txtName = New System.Windows.Forms.TextBox()
        Me.lblRows = New System.Windows.Forms.Label()
        Me.txtRows = New System.Windows.Forms.TextBox()
        Me.lblPerRow = New System.Windows.Forms.Label()
        Me.txtPerRow = New System.Windows.Forms.TextBox()
        Me.lblLayout = New System.Windows.Forms.Label()
        Me.btnAdd = New System.Windows.Forms.Button()
        Me.btnUpdate = New System.Windows.Forms.Button()
        Me.btnDelete = New System.Windows.Forms.Button()
        Me.btnClear = New System.Windows.Forms.Button()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.lblPickedScreen = New System.Windows.Forms.Label()
        Me.tabScreen = New System.Windows.Forms.TabControl()
        Me.tabOverview = New System.Windows.Forms.TabPage()
        Me.lblOverview = New System.Windows.Forms.Label()
        Me.lblScreenState = New System.Windows.Forms.Label()
        Me.lblReason = New System.Windows.Forms.Label()
        Me.txtReason = New System.Windows.Forms.TextBox()
        Me.btnOutOfService = New System.Windows.Forms.Button()
        Me.btnBackInService = New System.Windows.Forms.Button()
        Me.lblStatusHint = New System.Windows.Forms.Label()
        Me.tabHeatmap = New System.Windows.Forms.TabPage()
        Me.lblHeatmapInfo = New System.Windows.Forms.Label()
        Me.pnlHeatmap = New System.Windows.Forms.Panel()
        Me.lblHeatmapKey = New System.Windows.Forms.Label()
        Me.tabSeatPlan = New System.Windows.Forms.TabPage()
        Me.lblSeatPlanInfo = New System.Windows.Forms.Label()
        Me.pnlSeatPlan = New System.Windows.Forms.Panel()
        Me.btnPlanDefault = New System.Windows.Forms.Button()
        Me.btnPlanAllStandard = New System.Windows.Forms.Button()
        Me.lblSeatPlanKey = New System.Windows.Forms.Label()
        Me.lblPlanPreset = New System.Windows.Forms.Label()
        Me.cboPlanPreset = New System.Windows.Forms.ComboBox()
        Me.btnApplyPlan = New System.Windows.Forms.Button()
        Me.tabScreenings = New System.Windows.Forms.TabPage()
        Me.dgvScreenings = New System.Windows.Forms.DataGridView()
        Me.lblScreeningsInfo = New System.Windows.Forms.Label()
        Me.lblSaved = New System.Windows.Forms.Label()
        Me.lblVersion = New System.Windows.Forms.Label()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.tabScreen.SuspendLayout()
        Me.tabOverview.SuspendLayout()
        Me.tabHeatmap.SuspendLayout()
        Me.tabSeatPlan.SuspendLayout()
        Me.tabScreenings.SuspendLayout()
        CType(Me.dgvScreenings, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvScreens, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblHeading
        '
        Me.lblHeading.AutoSize = True
        Me.lblHeading.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Bold)
        Me.lblHeading.Location = New System.Drawing.Point(16, 12)
        Me.lblHeading.Name = "lblHeading"
        Me.lblHeading.Size = New System.Drawing.Size(90, 25)
        Me.lblHeading.TabIndex = 0
        Me.lblHeading.Text = "Screens"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.lblHint)
        Me.GroupBox1.Controls.Add(Me.lblGridCount)
        Me.GroupBox1.Controls.Add(Me.dgvScreens)
        Me.GroupBox1.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.GroupBox1.Location = New System.Drawing.Point(16, 46)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(868, 470)
        Me.GroupBox1.TabIndex = 1
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "The screens in the building"
        '
        'lblHint
        '
        Me.lblHint.AutoSize = True
        Me.lblHint.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lblHint.Location = New System.Drawing.Point(16, 28)
        Me.lblHint.Name = "lblHint"
        Me.lblHint.Size = New System.Drawing.Size(380, 15)
        Me.lblHint.TabIndex = 0
        Me.lblHint.Text = "Seats are made in rows of ten, lettered A, B, C and so on"
        '
        'lblGridCount
        '
        Me.lblGridCount.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lblGridCount.Location = New System.Drawing.Point(440, 28)
        Me.lblGridCount.Name = "lblGridCount"
        Me.lblGridCount.Size = New System.Drawing.Size(412, 17)
        Me.lblGridCount.TabIndex = 1
        Me.lblGridCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'dgvScreens
        '
        Me.dgvScreens.AllowUserToAddRows = False
        Me.dgvScreens.AllowUserToDeleteRows = False
        Me.dgvScreens.AllowUserToResizeRows = False
        Me.dgvScreens.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvScreens.Location = New System.Drawing.Point(16, 58)
        Me.dgvScreens.MultiSelect = False
        Me.dgvScreens.Name = "dgvScreens"
        Me.dgvScreens.ReadOnly = True
        Me.dgvScreens.RowHeadersVisible = False
        Me.dgvScreens.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvScreens.Size = New System.Drawing.Size(836, 400)
        Me.dgvScreens.TabIndex = 2
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.lblStatus)
        Me.GroupBox2.Controls.Add(Me.lblName)
        Me.GroupBox2.Controls.Add(Me.txtName)
        Me.GroupBox2.Controls.Add(Me.lblRows)
        Me.GroupBox2.Controls.Add(Me.txtRows)
        Me.GroupBox2.Controls.Add(Me.lblPerRow)
        Me.GroupBox2.Controls.Add(Me.txtPerRow)
        Me.GroupBox2.Controls.Add(Me.lblLayout)
        Me.GroupBox2.Controls.Add(Me.btnAdd)
        Me.GroupBox2.Controls.Add(Me.btnUpdate)
        Me.GroupBox2.Controls.Add(Me.btnDelete)
        Me.GroupBox2.Controls.Add(Me.btnClear)
        Me.GroupBox2.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.GroupBox2.Location = New System.Drawing.Point(16, 526)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(868, 180)
        Me.GroupBox2.TabIndex = 2
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Add a screen or change one"
        '
        'lblStatus
        '
        Me.lblStatus.AutoSize = True
        Me.lblStatus.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lblStatus.Location = New System.Drawing.Point(16, 24)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(150, 19)
        Me.lblStatus.TabIndex = 0
        Me.lblStatus.Text = "Adding a new screen"
        '
        'lblName
        '
        Me.lblName.AutoSize = True
        Me.lblName.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lblName.Location = New System.Drawing.Point(16, 61)
        Me.lblName.Name = "lblName"
        Me.lblName.Size = New System.Drawing.Size(39, 15)
        Me.lblName.TabIndex = 1
        Me.lblName.Text = "Name"
        '
        'txtName
        '
        Me.txtName.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.txtName.MaxLength = 255
        Me.txtName.Location = New System.Drawing.Point(90, 58)
        Me.txtName.Name = "txtName"
        Me.txtName.Size = New System.Drawing.Size(200, 23)
        Me.txtName.TabIndex = 2
        '
        'lblRows
        '
        Me.lblRows.AutoSize = True
        Me.lblRows.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lblRows.Location = New System.Drawing.Point(312, 61)
        Me.lblRows.Name = "lblRows"
        Me.lblRows.Size = New System.Drawing.Size(38, 15)
        Me.lblRows.TabIndex = 3
        Me.lblRows.Text = "Rows"
        '
        'txtRows
        '
        Me.txtRows.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.txtRows.Location = New System.Drawing.Point(370, 58)
        Me.txtRows.Name = "txtRows"
        Me.txtRows.Size = New System.Drawing.Size(50, 23)
        Me.txtRows.TabIndex = 4
        Me.txtRows.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lblPerRow
        '
        Me.lblPerRow.AutoSize = True
        Me.lblPerRow.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lblPerRow.Location = New System.Drawing.Point(434, 61)
        Me.lblPerRow.Name = "lblPerRow"
        Me.lblPerRow.Size = New System.Drawing.Size(56, 15)
        Me.lblPerRow.TabIndex = 5
        Me.lblPerRow.Text = "Per row"
        '
        'txtPerRow
        '
        Me.txtPerRow.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.txtPerRow.Location = New System.Drawing.Point(518, 58)
        Me.txtPerRow.Name = "txtPerRow"
        Me.txtPerRow.Size = New System.Drawing.Size(50, 23)
        Me.txtPerRow.TabIndex = 6
        Me.txtPerRow.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lblLayout
        '
        Me.lblLayout.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lblLayout.Location = New System.Drawing.Point(16, 95)
        Me.lblLayout.Name = "lblLayout"
        Me.lblLayout.Size = New System.Drawing.Size(560, 60)
        Me.lblLayout.TabIndex = 5
        Me.lblLayout.Text = "Type how many seats to see the layout"
        '
        'btnAdd
        '
        Me.btnAdd.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me.btnAdd.Location = New System.Drawing.Point(600, 55)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(252, 42)
        Me.btnAdd.TabIndex = 6
        Me.btnAdd.Text = "ADD THIS SCREEN"
        Me.btnAdd.UseVisualStyleBackColor = True
        '
        'btnUpdate
        '
        Me.btnUpdate.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnUpdate.Location = New System.Drawing.Point(600, 103)
        Me.btnUpdate.Name = "btnUpdate"
        Me.btnUpdate.Size = New System.Drawing.Size(121, 32)
        Me.btnUpdate.TabIndex = 7
        Me.btnUpdate.Text = "Save changes"
        Me.btnUpdate.UseVisualStyleBackColor = True
        '
        'btnDelete
        '
        Me.btnDelete.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnDelete.Location = New System.Drawing.Point(731, 103)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(121, 32)
        Me.btnDelete.TabIndex = 8
        Me.btnDelete.Text = "Delete"
        Me.btnDelete.UseVisualStyleBackColor = True
        '
        'btnClear
        '
        Me.btnClear.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnClear.Location = New System.Drawing.Point(600, 141)
        Me.btnClear.Name = "btnClear"
        Me.btnClear.Size = New System.Drawing.Size(252, 28)
        Me.btnClear.TabIndex = 9
        Me.btnClear.Text = "Clear the boxes"
        Me.btnClear.UseVisualStyleBackColor = True
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.lblPickedScreen)
        Me.GroupBox3.Controls.Add(Me.tabScreen)
        Me.GroupBox3.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.GroupBox3.Location = New System.Drawing.Point(900, 46)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(480, 660)
        Me.GroupBox3.TabIndex = 4
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Looking after a screen"
        '
        'lblPickedScreen
        '
        Me.lblPickedScreen.AutoSize = False
        Me.lblPickedScreen.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lblPickedScreen.Location = New System.Drawing.Point(14, 26)
        Me.lblPickedScreen.Name = "lblPickedScreen"
        Me.lblPickedScreen.Size = New System.Drawing.Size(452, 20)
        Me.lblPickedScreen.Text = "Pick a screen in the grid"
        '
        'tabScreen
        '
        Me.tabScreen.Controls.Add(Me.tabOverview)
        Me.tabScreen.Controls.Add(Me.tabHeatmap)
        Me.tabScreen.Controls.Add(Me.tabSeatPlan)
        Me.tabScreen.Controls.Add(Me.tabScreenings)
        Me.tabScreen.Location = New System.Drawing.Point(12, 52)
        Me.tabScreen.Name = "tabScreen"
        Me.tabScreen.SelectedIndex = 0
        Me.tabScreen.Size = New System.Drawing.Size(456, 596)
        Me.tabScreen.TabIndex = 0
        '
        'tabOverview
        '
        Me.tabOverview.Controls.Add(Me.lblOverview)
        Me.tabOverview.Controls.Add(Me.lblScreenState)
        Me.tabOverview.Controls.Add(Me.lblReason)
        Me.tabOverview.Controls.Add(Me.txtReason)
        Me.tabOverview.Controls.Add(Me.btnOutOfService)
        Me.tabOverview.Controls.Add(Me.btnBackInService)
        Me.tabOverview.Controls.Add(Me.lblStatusHint)
        Me.tabOverview.Location = New System.Drawing.Point(4, 25)
        Me.tabOverview.Name = "tabOverview"
        Me.tabOverview.Padding = New System.Windows.Forms.Padding(3)
        Me.tabOverview.Size = New System.Drawing.Size(448, 567)
        Me.tabOverview.TabIndex = 0
        Me.tabOverview.Text = "Overview"
        Me.tabOverview.UseVisualStyleBackColor = True
        '
        'lblOverview
        '
        Me.lblOverview.AutoSize = False
        Me.lblOverview.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lblOverview.Location = New System.Drawing.Point(12, 10)
        Me.lblOverview.Name = "lblOverview"
        Me.lblOverview.Size = New System.Drawing.Size(424, 228)
        Me.lblOverview.Text = ""
        '
        'lblScreenState
        '
        Me.lblScreenState.AutoSize = False
        Me.lblScreenState.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me.lblScreenState.Location = New System.Drawing.Point(12, 244)
        Me.lblScreenState.Name = "lblScreenState"
        Me.lblScreenState.Size = New System.Drawing.Size(424, 60)
        Me.lblScreenState.Text = ""
        '
        'lblReason
        '
        Me.lblReason.AutoSize = False
        Me.lblReason.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lblReason.Location = New System.Drawing.Point(12, 310)
        Me.lblReason.Name = "lblReason"
        Me.lblReason.Size = New System.Drawing.Size(424, 22)
        Me.lblReason.Text = "Why is it coming out of service?"
        '
        'txtReason
        '
        Me.txtReason.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.txtReason.Location = New System.Drawing.Point(12, 334)
        Me.txtReason.MaxLength = 100
        Me.txtReason.Name = "txtReason"
        Me.txtReason.Size = New System.Drawing.Size(424, 23)
        Me.txtReason.TabIndex = 1
        '
        'btnOutOfService
        '
        Me.btnOutOfService.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnOutOfService.Location = New System.Drawing.Point(12, 366)
        Me.btnOutOfService.Name = "btnOutOfService"
        Me.btnOutOfService.Size = New System.Drawing.Size(204, 34)
        Me.btnOutOfService.TabIndex = 2
        Me.btnOutOfService.Text = "Take out of service"
        Me.btnOutOfService.UseVisualStyleBackColor = True
        '
        'btnBackInService
        '
        Me.btnBackInService.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnBackInService.Location = New System.Drawing.Point(230, 366)
        Me.btnBackInService.Name = "btnBackInService"
        Me.btnBackInService.Size = New System.Drawing.Size(204, 34)
        Me.btnBackInService.TabIndex = 3
        Me.btnBackInService.Text = "Put back in service"
        Me.btnBackInService.UseVisualStyleBackColor = True
        '
        'lblStatusHint
        '
        Me.lblStatusHint.AutoSize = False
        Me.lblStatusHint.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lblStatusHint.Location = New System.Drawing.Point(12, 408)
        Me.lblStatusHint.Name = "lblStatusHint"
        Me.lblStatusHint.Size = New System.Drawing.Size(424, 64)
        Me.lblStatusHint.Text = ""
        '
        'tabHeatmap
        '
        Me.tabHeatmap.Controls.Add(Me.lblHeatmapInfo)
        Me.tabHeatmap.Controls.Add(Me.pnlHeatmap)
        Me.tabHeatmap.Controls.Add(Me.lblHeatmapKey)
        Me.tabHeatmap.Location = New System.Drawing.Point(4, 25)
        Me.tabHeatmap.Name = "tabHeatmap"
        Me.tabHeatmap.Padding = New System.Windows.Forms.Padding(3)
        Me.tabHeatmap.Size = New System.Drawing.Size(448, 567)
        Me.tabHeatmap.TabIndex = 1
        Me.tabHeatmap.Text = "Seat popularity"
        Me.tabHeatmap.UseVisualStyleBackColor = True
        '
        'lblHeatmapInfo
        '
        Me.lblHeatmapInfo.AutoSize = False
        Me.lblHeatmapInfo.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lblHeatmapInfo.Location = New System.Drawing.Point(12, 8)
        Me.lblHeatmapInfo.Name = "lblHeatmapInfo"
        Me.lblHeatmapInfo.Size = New System.Drawing.Size(424, 46)
        Me.lblHeatmapInfo.Text = ""
        '
        'pnlHeatmap
        '
        Me.pnlHeatmap.Location = New System.Drawing.Point(12, 58)
        Me.pnlHeatmap.Name = "pnlHeatmap"
        Me.pnlHeatmap.Size = New System.Drawing.Size(424, 382)
        Me.pnlHeatmap.TabIndex = 0
        '
        'lblHeatmapKey
        '
        Me.lblHeatmapKey.AutoSize = False
        Me.lblHeatmapKey.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lblHeatmapKey.Location = New System.Drawing.Point(12, 448)
        Me.lblHeatmapKey.Name = "lblHeatmapKey"
        Me.lblHeatmapKey.Size = New System.Drawing.Size(424, 104)
        Me.lblHeatmapKey.Text = ""
        '
        'tabSeatPlan
        '
        Me.tabSeatPlan.Controls.Add(Me.lblSeatPlanInfo)
        Me.tabSeatPlan.Controls.Add(Me.pnlSeatPlan)
        Me.tabSeatPlan.Controls.Add(Me.btnPlanDefault)
        Me.tabSeatPlan.Controls.Add(Me.btnPlanAllStandard)
        Me.tabSeatPlan.Controls.Add(Me.lblSeatPlanKey)
        Me.tabSeatPlan.Controls.Add(Me.lblPlanPreset)
        Me.tabSeatPlan.Controls.Add(Me.cboPlanPreset)
        Me.tabSeatPlan.Controls.Add(Me.btnApplyPlan)
        Me.tabSeatPlan.Location = New System.Drawing.Point(4, 25)
        Me.tabSeatPlan.Name = "tabSeatPlan"
        Me.tabSeatPlan.Padding = New System.Windows.Forms.Padding(3)
        Me.tabSeatPlan.Size = New System.Drawing.Size(448, 567)
        Me.tabSeatPlan.TabIndex = 3
        Me.tabSeatPlan.Text = "Seat plan"
        Me.tabSeatPlan.UseVisualStyleBackColor = True
        '
        'lblSeatPlanInfo
        '
        Me.lblSeatPlanInfo.AutoSize = False
        Me.lblSeatPlanInfo.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lblSeatPlanInfo.Location = New System.Drawing.Point(12, 8)
        Me.lblSeatPlanInfo.Name = "lblSeatPlanInfo"
        Me.lblSeatPlanInfo.Size = New System.Drawing.Size(424, 62)
        Me.lblSeatPlanInfo.Text = ""
        '
        'pnlSeatPlan
        '
        Me.pnlSeatPlan.Cursor = System.Windows.Forms.Cursors.Hand
        Me.pnlSeatPlan.Location = New System.Drawing.Point(12, 74)
        Me.pnlSeatPlan.Name = "pnlSeatPlan"
        Me.pnlSeatPlan.Size = New System.Drawing.Size(424, 284)
        Me.pnlSeatPlan.TabIndex = 0
        '
        'btnPlanDefault
        '
        Me.btnPlanDefault.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnPlanDefault.Location = New System.Drawing.Point(12, 438)
        Me.btnPlanDefault.Name = "btnPlanDefault"
        Me.btnPlanDefault.Size = New System.Drawing.Size(204, 30)
        Me.btnPlanDefault.TabIndex = 1
        Me.btnPlanDefault.Text = "Use the usual layout"
        Me.btnPlanDefault.UseVisualStyleBackColor = True
        '
        'btnPlanAllStandard
        '
        Me.btnPlanAllStandard.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnPlanAllStandard.Location = New System.Drawing.Point(232, 438)
        Me.btnPlanAllStandard.Name = "btnPlanAllStandard"
        Me.btnPlanAllStandard.Size = New System.Drawing.Size(204, 30)
        Me.btnPlanAllStandard.TabIndex = 2
        Me.btnPlanAllStandard.Text = "Every seat standard"
        Me.btnPlanAllStandard.UseVisualStyleBackColor = True
        '
        'lblSeatPlanKey
        '
        Me.lblPlanPreset.AutoSize = True
        Me.lblPlanPreset.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lblPlanPreset.Location = New System.Drawing.Point(12, 372)
        Me.lblPlanPreset.Name = "lblPlanPreset"
        Me.lblPlanPreset.Size = New System.Drawing.Size(110, 15)
        Me.lblPlanPreset.Text = "Ready-made plan"
        '
        'cboPlanPreset
        '
        Me.cboPlanPreset.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboPlanPreset.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.cboPlanPreset.Location = New System.Drawing.Point(160, 368)
        Me.cboPlanPreset.Name = "cboPlanPreset"
        Me.cboPlanPreset.Size = New System.Drawing.Size(276, 23)
        '
        'btnApplyPlan
        '
        Me.btnApplyPlan.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnApplyPlan.Location = New System.Drawing.Point(12, 400)
        Me.btnApplyPlan.Name = "btnApplyPlan"
        Me.btnApplyPlan.Size = New System.Drawing.Size(424, 30)
        Me.btnApplyPlan.Text = "Use this ready-made plan"
        Me.btnApplyPlan.UseVisualStyleBackColor = True
        '
        'lblSeatPlanKey
        '
        Me.lblSeatPlanKey.AutoSize = False
        Me.lblSeatPlanKey.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lblSeatPlanKey.Location = New System.Drawing.Point(12, 476)
        Me.lblSeatPlanKey.Name = "lblSeatPlanKey"
        Me.lblSeatPlanKey.Size = New System.Drawing.Size(424, 86)
        Me.lblSeatPlanKey.Text = ""
        '
        'tabScreenings
        '
        Me.tabScreenings.Controls.Add(Me.dgvScreenings)
        Me.tabScreenings.Controls.Add(Me.lblScreeningsInfo)
        Me.tabScreenings.Location = New System.Drawing.Point(4, 25)
        Me.tabScreenings.Name = "tabScreenings"
        Me.tabScreenings.Padding = New System.Windows.Forms.Padding(3)
        Me.tabScreenings.Size = New System.Drawing.Size(448, 567)
        Me.tabScreenings.TabIndex = 2
        Me.tabScreenings.Text = "What is on"
        Me.tabScreenings.UseVisualStyleBackColor = True
        '
        'dgvScreenings
        '
        Me.dgvScreenings.AllowUserToAddRows = False
        Me.dgvScreenings.AllowUserToDeleteRows = False
        Me.dgvScreenings.AllowUserToResizeRows = False
        Me.dgvScreenings.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvScreenings.Location = New System.Drawing.Point(12, 10)
        Me.dgvScreenings.MultiSelect = False
        Me.dgvScreenings.Name = "dgvScreenings"
        Me.dgvScreenings.ReadOnly = True
        Me.dgvScreenings.RowHeadersVisible = False
        Me.dgvScreenings.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvScreenings.Size = New System.Drawing.Size(424, 474)
        Me.dgvScreenings.TabIndex = 0
        '
        'lblScreeningsInfo
        '
        Me.lblScreeningsInfo.AutoSize = False
        Me.lblScreeningsInfo.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lblScreeningsInfo.Location = New System.Drawing.Point(12, 490)
        Me.lblScreeningsInfo.Name = "lblScreeningsInfo"
        Me.lblScreeningsInfo.Size = New System.Drawing.Size(424, 70)
        Me.lblScreeningsInfo.Text = ""
        '
        'lblSaved
        '
        Me.lblSaved.AutoSize = False
        Me.lblSaved.Location = New System.Drawing.Point(360, 716)
        Me.lblSaved.Name = "lblSaved"
        Me.lblSaved.Size = New System.Drawing.Size(524, 16)
        Me.lblSaved.Text = ""
        '
        'lblVersion
        '
        Me.lblVersion.AutoSize = True
        Me.lblVersion.ForeColor = System.Drawing.Color.Gray
        Me.lblVersion.Location = New System.Drawing.Point(16, 716)
        Me.lblVersion.Name = "lblVersion"
        Me.lblVersion.Size = New System.Drawing.Size(271, 16)
        Me.lblVersion.TabIndex = 3
        Me.lblVersion.Text = "Filmtopia Cinema Management System  v1.0"
        '
        'frmScreens
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1396, 745)
        Me.Controls.Add(Me.lblSaved)
        Me.Controls.Add(Me.lblVersion)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.lblHeading)
        Me.Name = "frmScreens"
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Text = "Screens"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.tabScreen.ResumeLayout(False)
        Me.tabOverview.ResumeLayout(False)
        Me.tabOverview.PerformLayout()
        Me.tabHeatmap.ResumeLayout(False)
        Me.tabSeatPlan.ResumeLayout(False)
        Me.tabScreenings.ResumeLayout(False)
        CType(Me.dgvScreenings, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvScreens, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblHeading As Label
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents lblHint As Label
    Friend WithEvents lblGridCount As Label
    Friend WithEvents dgvScreens As DataGridView
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents lblStatus As Label
    Friend WithEvents lblName As Label
    Friend WithEvents txtName As TextBox
    Friend WithEvents lblRows As Label
    Friend WithEvents txtRows As TextBox
    Friend WithEvents lblPerRow As Label
    Friend WithEvents txtPerRow As TextBox
    Friend WithEvents lblLayout As Label
    Friend WithEvents btnAdd As Button
    Friend WithEvents btnUpdate As Button
    Friend WithEvents btnDelete As Button
    Friend WithEvents btnClear As Button
    Friend WithEvents GroupBox3 As GroupBox
    Friend WithEvents lblPickedScreen As Label
    Friend WithEvents tabScreen As TabControl
    Friend WithEvents tabOverview As TabPage
    Friend WithEvents lblOverview As Label
    Friend WithEvents lblScreenState As Label
    Friend WithEvents lblReason As Label
    Friend WithEvents txtReason As TextBox
    Friend WithEvents btnOutOfService As Button
    Friend WithEvents btnBackInService As Button
    Friend WithEvents lblStatusHint As Label
    Friend WithEvents tabHeatmap As TabPage
    Friend WithEvents lblHeatmapInfo As Label
    Friend WithEvents pnlHeatmap As Panel
    Friend WithEvents lblHeatmapKey As Label
    Friend WithEvents tabSeatPlan As TabPage
    Friend WithEvents lblSeatPlanInfo As Label
    Friend WithEvents pnlSeatPlan As Panel
    Friend WithEvents btnPlanDefault As Button
    Friend WithEvents btnPlanAllStandard As Button
    Friend WithEvents lblSeatPlanKey As Label
    Friend WithEvents lblPlanPreset As Label
    Friend WithEvents cboPlanPreset As ComboBox
    Friend WithEvents btnApplyPlan As Button
    Friend WithEvents tabScreenings As TabPage
    Friend WithEvents dgvScreenings As DataGridView
    Friend WithEvents lblScreeningsInfo As Label
    Friend WithEvents lblSaved As Label
    Friend WithEvents lblVersion As Label
End Class
