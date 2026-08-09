<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmLogs
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
        Me.lblFrom = New System.Windows.Forms.Label()
        Me.dtpFrom = New System.Windows.Forms.DateTimePicker()
        Me.lblTo = New System.Windows.Forms.Label()
        Me.dtpTo = New System.Windows.Forms.DateTimePicker()
        Me.lblType = New System.Windows.Forms.Label()
        Me.cboType = New System.Windows.Forms.ComboBox()
        Me.lblSeverity = New System.Windows.Forms.Label()
        Me.cboSeverity = New System.Windows.Forms.ComboBox()
        Me.lblSearch = New System.Windows.Forms.Label()
        Me.txtSearch = New System.Windows.Forms.TextBox()
        Me.lblUser = New System.Windows.Forms.Label()
        Me.cboUser = New System.Windows.Forms.ComboBox()
        Me.btnApply = New System.Windows.Forms.Button()
        Me.btnClearFilters = New System.Windows.Forms.Button()
        Me.btnRefresh = New System.Windows.Forms.Button()
        Me.btnExport = New System.Windows.Forms.Button()
        Me.dgvLogs = New System.Windows.Forms.DataGridView()
        Me.lblCount = New System.Windows.Forms.Label()
        Me.lblVersion = New System.Windows.Forms.Label()
        CType(Me.dgvLogs, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblFrom
        '
        Me.lblFrom.AutoSize = True
        Me.lblFrom.Location = New System.Drawing.Point(12, 17)
        Me.lblFrom.Name = "lblFrom"
        Me.lblFrom.Size = New System.Drawing.Size(33, 17)
        Me.lblFrom.TabIndex = 0
        Me.lblFrom.Text = "From"
        '
        'dtpFrom
        '
        Me.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short
        Me.dtpFrom.Location = New System.Drawing.Point(55, 13)
        Me.dtpFrom.Name = "dtpFrom"
        Me.dtpFrom.Size = New System.Drawing.Size(110, 22)
        Me.dtpFrom.TabIndex = 1
        '
        'lblTo
        '
        Me.lblTo.AutoSize = True
        Me.lblTo.Location = New System.Drawing.Point(175, 17)
        Me.lblTo.Name = "lblTo"
        Me.lblTo.Size = New System.Drawing.Size(20, 17)
        Me.lblTo.TabIndex = 2
        Me.lblTo.Text = "To"
        '
        'dtpTo
        '
        Me.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Short
        Me.dtpTo.Location = New System.Drawing.Point(203, 13)
        Me.dtpTo.Name = "dtpTo"
        Me.dtpTo.Size = New System.Drawing.Size(110, 22)
        Me.dtpTo.TabIndex = 3
        '
        'lblType
        '
        Me.lblType.AutoSize = True
        Me.lblType.Location = New System.Drawing.Point(327, 17)
        Me.lblType.Name = "lblType"
        Me.lblType.Size = New System.Drawing.Size(33, 17)
        Me.lblType.TabIndex = 4
        Me.lblType.Text = "Area"
        '
        'cboType
        '
        Me.cboType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboType.Location = New System.Drawing.Point(368, 13)
        Me.cboType.Name = "cboType"
        Me.cboType.Size = New System.Drawing.Size(130, 24)
        Me.cboType.TabIndex = 5
        '
        'lblSeverity
        '
        Me.lblSeverity.AutoSize = True
        Me.lblSeverity.Location = New System.Drawing.Point(512, 17)
        Me.lblSeverity.Name = "lblSeverity"
        Me.lblSeverity.Size = New System.Drawing.Size(38, 17)
        Me.lblSeverity.TabIndex = 6
        Me.lblSeverity.Text = "Level"
        '
        'cboSeverity
        '
        Me.cboSeverity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSeverity.Location = New System.Drawing.Point(556, 13)
        Me.cboSeverity.Name = "cboSeverity"
        Me.cboSeverity.Size = New System.Drawing.Size(160, 24)
        Me.cboSeverity.TabIndex = 7
        '
        'lblSearch
        '
        Me.lblSearch.AutoSize = True
        Me.lblSearch.Location = New System.Drawing.Point(730, 17)
        Me.lblSearch.Name = "lblSearch"
        Me.lblSearch.Size = New System.Drawing.Size(48, 17)
        Me.lblSearch.TabIndex = 8
        Me.lblSearch.Text = "Search"
        '
        'txtSearch
        '
        Me.txtSearch.Location = New System.Drawing.Point(784, 13)
        Me.txtSearch.Name = "txtSearch"
        Me.txtSearch.Size = New System.Drawing.Size(188, 22)
        Me.txtSearch.TabIndex = 9
        '
        'lblUser
        '
        Me.lblUser.AutoSize = True
        Me.lblUser.Location = New System.Drawing.Point(12, 55)
        Me.lblUser.Name = "lblUser"
        Me.lblUser.Size = New System.Drawing.Size(33, 17)
        Me.lblUser.TabIndex = 10
        Me.lblUser.Text = "User"
        '
        'cboUser
        '
        Me.cboUser.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboUser.Location = New System.Drawing.Point(55, 51)
        Me.cboUser.Name = "cboUser"
        Me.cboUser.Size = New System.Drawing.Size(150, 24)
        Me.cboUser.TabIndex = 11
        '
        'btnApply
        '
        Me.btnApply.Location = New System.Drawing.Point(220, 50)
        Me.btnApply.Name = "btnApply"
        Me.btnApply.Size = New System.Drawing.Size(110, 28)
        Me.btnApply.TabIndex = 12
        Me.btnApply.Text = "Apply filters"
        Me.btnApply.UseVisualStyleBackColor = True
        '
        'btnClearFilters
        '
        Me.btnClearFilters.Location = New System.Drawing.Point(340, 50)
        Me.btnClearFilters.Name = "btnClearFilters"
        Me.btnClearFilters.Size = New System.Drawing.Size(110, 28)
        Me.btnClearFilters.TabIndex = 13
        Me.btnClearFilters.Text = "Clear filters"
        Me.btnClearFilters.UseVisualStyleBackColor = True
        '
        'btnRefresh
        '
        Me.btnRefresh.Location = New System.Drawing.Point(460, 50)
        Me.btnRefresh.Name = "btnRefresh"
        Me.btnRefresh.Size = New System.Drawing.Size(110, 28)
        Me.btnRefresh.TabIndex = 14
        Me.btnRefresh.Text = "Refresh"
        Me.btnRefresh.UseVisualStyleBackColor = True
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(580, 50)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(130, 28)
        Me.btnExport.TabIndex = 15
        Me.btnExport.Text = "Export CSV"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'dgvLogs
        '
        Me.dgvLogs.AllowUserToAddRows = False
        Me.dgvLogs.AllowUserToDeleteRows = False
        Me.dgvLogs.AllowUserToResizeRows = False
        Me.dgvLogs.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvLogs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvLogs.Location = New System.Drawing.Point(12, 90)
        Me.dgvLogs.Name = "dgvLogs"
        Me.dgvLogs.ReadOnly = True
        Me.dgvLogs.RowHeadersVisible = False
        Me.dgvLogs.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvLogs.Size = New System.Drawing.Size(960, 420)
        Me.dgvLogs.TabIndex = 16
        '
        'lblCount
        '
        Me.lblCount.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblCount.AutoSize = True
        Me.lblCount.Location = New System.Drawing.Point(12, 520)
        Me.lblCount.Name = "lblCount"
        Me.lblCount.Size = New System.Drawing.Size(0, 17)
        Me.lblCount.TabIndex = 17
        '
        'lblVersion
        '
        Me.lblVersion.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblVersion.AutoSize = True
        Me.lblVersion.ForeColor = System.Drawing.Color.Gray
        Me.lblVersion.Location = New System.Drawing.Point(12, 545)
        Me.lblVersion.Name = "lblVersion"
        Me.lblVersion.Size = New System.Drawing.Size(271, 17)
        Me.lblVersion.TabIndex = 18
        Me.lblVersion.Text = "Filmtopia Cinema Management System  v1.0"
        '
        'frmLogs
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(984, 575)
        Me.Controls.Add(Me.lblVersion)
        Me.Controls.Add(Me.lblCount)
        Me.Controls.Add(Me.dgvLogs)
        Me.Controls.Add(Me.btnExport)
        Me.Controls.Add(Me.btnRefresh)
        Me.Controls.Add(Me.btnClearFilters)
        Me.Controls.Add(Me.btnApply)
        Me.Controls.Add(Me.cboUser)
        Me.Controls.Add(Me.lblUser)
        Me.Controls.Add(Me.txtSearch)
        Me.Controls.Add(Me.lblSearch)
        Me.Controls.Add(Me.cboSeverity)
        Me.Controls.Add(Me.lblSeverity)
        Me.Controls.Add(Me.cboType)
        Me.Controls.Add(Me.lblType)
        Me.Controls.Add(Me.dtpTo)
        Me.Controls.Add(Me.lblTo)
        Me.Controls.Add(Me.dtpFrom)
        Me.Controls.Add(Me.lblFrom)
        Me.Name = "frmLogs"
        Me.MinimumSize = New System.Drawing.Size(1000, 620)
        Me.Text = "Audit Log"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        CType(Me.dgvLogs, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblFrom As Label
    Friend WithEvents dtpFrom As DateTimePicker
    Friend WithEvents lblTo As Label
    Friend WithEvents dtpTo As DateTimePicker
    Friend WithEvents lblType As Label
    Friend WithEvents cboType As ComboBox
    Friend WithEvents lblSeverity As Label
    Friend WithEvents cboSeverity As ComboBox
    Friend WithEvents lblSearch As Label
    Friend WithEvents txtSearch As TextBox
    Friend WithEvents lblUser As Label
    Friend WithEvents cboUser As ComboBox
    Friend WithEvents btnApply As Button
    Friend WithEvents btnClearFilters As Button
    Friend WithEvents btnRefresh As Button
    Friend WithEvents btnExport As Button
    Friend WithEvents dgvLogs As DataGridView
    Friend WithEvents lblCount As Label
    Friend WithEvents lblVersion As Label
End Class
