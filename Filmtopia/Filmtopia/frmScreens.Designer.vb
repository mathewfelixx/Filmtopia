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
        Me.tabScreens = New System.Windows.Forms.TabControl()
        Me.tabManage = New System.Windows.Forms.TabPage()
        Me.tabSeatTypes = New System.Windows.Forms.TabPage()
        Me.dgvScreens = New System.Windows.Forms.DataGridView()
        Me.lblName = New System.Windows.Forms.Label()
        Me.txtName = New System.Windows.Forms.TextBox()
        Me.lblCapacity = New System.Windows.Forms.Label()
        Me.txtCapacity = New System.Windows.Forms.TextBox()
        Me.btnAdd = New System.Windows.Forms.Button()
        Me.btnUpdate = New System.Windows.Forms.Button()
        Me.btnDelete = New System.Windows.Forms.Button()
        Me.btnClear = New System.Windows.Forms.Button()
        Me.chkShowInactive = New System.Windows.Forms.CheckBox()
        Me.lblSeatScreen = New System.Windows.Forms.Label()
        Me.cboSeatTypeScreen = New System.Windows.Forms.ComboBox()
        Me.lblSeatHint = New System.Windows.Forms.Label()
        Me.lblScreenIndicator = New System.Windows.Forms.Label()
        Me.pnlSeatTypeMap = New System.Windows.Forms.Panel()
        Me.lblKeyTitle = New System.Windows.Forms.Label()
        Me.lblStdKey = New System.Windows.Forms.Label()
        Me.lblPremKey = New System.Windows.Forms.Label()
        Me.lblAccKey = New System.Windows.Forms.Label()
        Me.lblSetTo = New System.Windows.Forms.Label()
        Me.cboTargetType = New System.Windows.Forms.ComboBox()
        Me.btnApplyType = New System.Windows.Forms.Button()
        Me.btnClearSel = New System.Windows.Forms.Button()
        Me.lblVersion = New System.Windows.Forms.Label()
        CType(Me.dgvScreens, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabScreens.SuspendLayout()
        Me.tabManage.SuspendLayout()
        Me.tabSeatTypes.SuspendLayout()
        Me.SuspendLayout()
        '
        'tabScreens
        '
        Me.tabScreens.Controls.Add(Me.tabManage)
        Me.tabScreens.Controls.Add(Me.tabSeatTypes)
        Me.tabScreens.Location = New System.Drawing.Point(8, 8)
        Me.tabScreens.Name = "tabScreens"
        Me.tabScreens.SelectedIndex = 0
        Me.tabScreens.Size = New System.Drawing.Size(884, 560)
        Me.tabScreens.TabIndex = 0
        '
        'tabManage
        '
        Me.tabManage.Controls.Add(Me.dgvScreens)
        Me.tabManage.Controls.Add(Me.lblName)
        Me.tabManage.Controls.Add(Me.txtName)
        Me.tabManage.Controls.Add(Me.lblCapacity)
        Me.tabManage.Controls.Add(Me.txtCapacity)
        Me.tabManage.Controls.Add(Me.btnAdd)
        Me.tabManage.Controls.Add(Me.btnUpdate)
        Me.tabManage.Controls.Add(Me.btnClear)
        Me.tabManage.Controls.Add(Me.btnDelete)
        Me.tabManage.Controls.Add(Me.chkShowInactive)
        Me.tabManage.Location = New System.Drawing.Point(4, 22)
        Me.tabManage.Name = "tabManage"
        Me.tabManage.Padding = New System.Windows.Forms.Padding(3)
        Me.tabManage.Size = New System.Drawing.Size(876, 534)
        Me.tabManage.TabIndex = 0
        Me.tabManage.Text = "Manage screens"
        Me.tabManage.UseVisualStyleBackColor = True
        '
        'tabSeatTypes
        '
        Me.tabSeatTypes.Controls.Add(Me.lblSeatScreen)
        Me.tabSeatTypes.Controls.Add(Me.cboSeatTypeScreen)
        Me.tabSeatTypes.Controls.Add(Me.lblSeatHint)
        Me.tabSeatTypes.Controls.Add(Me.lblScreenIndicator)
        Me.tabSeatTypes.Controls.Add(Me.pnlSeatTypeMap)
        Me.tabSeatTypes.Controls.Add(Me.lblKeyTitle)
        Me.tabSeatTypes.Controls.Add(Me.lblStdKey)
        Me.tabSeatTypes.Controls.Add(Me.lblPremKey)
        Me.tabSeatTypes.Controls.Add(Me.lblAccKey)
        Me.tabSeatTypes.Controls.Add(Me.lblSetTo)
        Me.tabSeatTypes.Controls.Add(Me.cboTargetType)
        Me.tabSeatTypes.Controls.Add(Me.btnApplyType)
        Me.tabSeatTypes.Controls.Add(Me.btnClearSel)
        Me.tabSeatTypes.Location = New System.Drawing.Point(4, 22)
        Me.tabSeatTypes.Name = "tabSeatTypes"
        Me.tabSeatTypes.Padding = New System.Windows.Forms.Padding(3)
        Me.tabSeatTypes.Size = New System.Drawing.Size(876, 534)
        Me.tabSeatTypes.TabIndex = 1
        Me.tabSeatTypes.Text = "Seat types"
        Me.tabSeatTypes.UseVisualStyleBackColor = True
        '
        'dgvScreens
        '
        Me.dgvScreens.AllowUserToAddRows = False
        Me.dgvScreens.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvScreens.Location = New System.Drawing.Point(12, 12)
        Me.dgvScreens.MultiSelect = False
        Me.dgvScreens.Name = "dgvScreens"
        Me.dgvScreens.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvScreens.Size = New System.Drawing.Size(660, 300)
        Me.dgvScreens.TabIndex = 0
        '
        'lblName
        '
        Me.lblName.AutoSize = True
        Me.lblName.Location = New System.Drawing.Point(12, 330)
        Me.lblName.Name = "lblName"
        Me.lblName.Size = New System.Drawing.Size(35, 13)
        Me.lblName.TabIndex = 1
        Me.lblName.Text = "Name"
        '
        'txtName
        '
        Me.txtName.Location = New System.Drawing.Point(120, 327)
        Me.txtName.Name = "txtName"
        Me.txtName.Size = New System.Drawing.Size(250, 20)
        Me.txtName.TabIndex = 2
        '
        'lblCapacity
        '
        Me.lblCapacity.AutoSize = True
        Me.lblCapacity.Location = New System.Drawing.Point(12, 360)
        Me.lblCapacity.Name = "lblCapacity"
        Me.lblCapacity.Size = New System.Drawing.Size(50, 13)
        Me.lblCapacity.TabIndex = 3
        Me.lblCapacity.Text = "Capacity"
        '
        'txtCapacity
        '
        Me.txtCapacity.Location = New System.Drawing.Point(120, 357)
        Me.txtCapacity.Name = "txtCapacity"
        Me.txtCapacity.Size = New System.Drawing.Size(100, 20)
        Me.txtCapacity.TabIndex = 4
        '
        'btnAdd
        '
        Me.btnAdd.Location = New System.Drawing.Point(486, 327)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(150, 30)
        Me.btnAdd.TabIndex = 5
        Me.btnAdd.Text = "Add"
        Me.btnAdd.UseVisualStyleBackColor = True
        '
        'btnUpdate
        '
        Me.btnUpdate.Location = New System.Drawing.Point(486, 363)
        Me.btnUpdate.Name = "btnUpdate"
        Me.btnUpdate.Size = New System.Drawing.Size(150, 30)
        Me.btnUpdate.TabIndex = 6
        Me.btnUpdate.Text = "Update"
        Me.btnUpdate.UseVisualStyleBackColor = True
        '
        'btnClear
        '
        Me.btnClear.Location = New System.Drawing.Point(486, 399)
        Me.btnClear.Name = "btnClear"
        Me.btnClear.Size = New System.Drawing.Size(150, 30)
        Me.btnClear.TabIndex = 7
        Me.btnClear.Text = "Clear"
        Me.btnClear.UseVisualStyleBackColor = True
        '
        'btnDelete
        '
        Me.btnDelete.Location = New System.Drawing.Point(486, 435)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(150, 30)
        Me.btnDelete.TabIndex = 8
        Me.btnDelete.Text = "Take out of service"
        Me.btnDelete.UseVisualStyleBackColor = True
        '
        'chkShowInactive
        '
        Me.chkShowInactive.AutoSize = True
        Me.chkShowInactive.Location = New System.Drawing.Point(12, 395)
        Me.chkShowInactive.Name = "chkShowInactive"
        Me.chkShowInactive.Size = New System.Drawing.Size(172, 17)
        Me.chkShowInactive.TabIndex = 9
        Me.chkShowInactive.Text = "Show out-of-service screens"
        Me.chkShowInactive.UseVisualStyleBackColor = True
        '
        'lblSeatScreen
        '
        Me.lblSeatScreen.AutoSize = True
        Me.lblSeatScreen.Location = New System.Drawing.Point(16, 18)
        Me.lblSeatScreen.Name = "lblSeatScreen"
        Me.lblSeatScreen.Size = New System.Drawing.Size(43, 13)
        Me.lblSeatScreen.TabIndex = 0
        Me.lblSeatScreen.Text = "Screen"
        '
        'cboSeatTypeScreen
        '
        Me.cboSeatTypeScreen.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSeatTypeScreen.Location = New System.Drawing.Point(90, 15)
        Me.cboSeatTypeScreen.Name = "cboSeatTypeScreen"
        Me.cboSeatTypeScreen.Size = New System.Drawing.Size(260, 21)
        Me.cboSeatTypeScreen.TabIndex = 1
        '
        'lblSeatHint
        '
        Me.lblSeatHint.AutoSize = True
        Me.lblSeatHint.Location = New System.Drawing.Point(16, 50)
        Me.lblSeatHint.Name = "lblSeatHint"
        Me.lblSeatHint.Size = New System.Drawing.Size(320, 13)
        Me.lblSeatHint.TabIndex = 2
        Me.lblSeatHint.Text = "Click seats to select them, then choose a type and press Apply."
        '
        'lblScreenIndicator
        '
        Me.lblScreenIndicator.Location = New System.Drawing.Point(16, 74)
        Me.lblScreenIndicator.Name = "lblScreenIndicator"
        Me.lblScreenIndicator.Size = New System.Drawing.Size(600, 22)
        Me.lblScreenIndicator.TabIndex = 3
        Me.lblScreenIndicator.Text = "Screen"
        Me.lblScreenIndicator.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'pnlSeatTypeMap
        '
        Me.pnlSeatTypeMap.AutoScroll = True
        Me.pnlSeatTypeMap.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlSeatTypeMap.Location = New System.Drawing.Point(16, 100)
        Me.pnlSeatTypeMap.Name = "pnlSeatTypeMap"
        Me.pnlSeatTypeMap.Size = New System.Drawing.Size(600, 410)
        Me.pnlSeatTypeMap.TabIndex = 4
        '
        'lblKeyTitle
        '
        Me.lblKeyTitle.AutoSize = True
        Me.lblKeyTitle.Location = New System.Drawing.Point(640, 80)
        Me.lblKeyTitle.Name = "lblKeyTitle"
        Me.lblKeyTitle.Size = New System.Drawing.Size(25, 13)
        Me.lblKeyTitle.TabIndex = 4
        Me.lblKeyTitle.Text = "Key"
        '
        'lblStdKey
        '
        Me.lblStdKey.Location = New System.Drawing.Point(640, 103)
        Me.lblStdKey.Name = "lblStdKey"
        Me.lblStdKey.Size = New System.Drawing.Size(160, 22)
        Me.lblStdKey.TabIndex = 5
        Me.lblStdKey.Text = "  Standard"
        Me.lblStdKey.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblPremKey
        '
        Me.lblPremKey.Location = New System.Drawing.Point(640, 130)
        Me.lblPremKey.Name = "lblPremKey"
        Me.lblPremKey.Size = New System.Drawing.Size(160, 22)
        Me.lblPremKey.TabIndex = 6
        Me.lblPremKey.Text = "  Premium x1.5"
        Me.lblPremKey.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblAccKey
        '
        Me.lblAccKey.Location = New System.Drawing.Point(640, 157)
        Me.lblAccKey.Name = "lblAccKey"
        Me.lblAccKey.Size = New System.Drawing.Size(160, 22)
        Me.lblAccKey.TabIndex = 7
        Me.lblAccKey.Text = "  Accessible"
        Me.lblAccKey.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblSetTo
        '
        Me.lblSetTo.AutoSize = True
        Me.lblSetTo.Location = New System.Drawing.Point(640, 205)
        Me.lblSetTo.Name = "lblSetTo"
        Me.lblSetTo.Size = New System.Drawing.Size(115, 13)
        Me.lblSetTo.TabIndex = 8
        Me.lblSetTo.Text = "Set selected seats to"
        '
        'cboTargetType
        '
        Me.cboTargetType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboTargetType.Location = New System.Drawing.Point(640, 228)
        Me.cboTargetType.Name = "cboTargetType"
        Me.cboTargetType.Size = New System.Drawing.Size(180, 21)
        Me.cboTargetType.TabIndex = 9
        '
        'btnApplyType
        '
        Me.btnApplyType.Location = New System.Drawing.Point(640, 262)
        Me.btnApplyType.Name = "btnApplyType"
        Me.btnApplyType.Size = New System.Drawing.Size(180, 32)
        Me.btnApplyType.TabIndex = 10
        Me.btnApplyType.Text = "Apply to selected"
        Me.btnApplyType.UseVisualStyleBackColor = True
        '
        'btnClearSel
        '
        Me.btnClearSel.Location = New System.Drawing.Point(640, 300)
        Me.btnClearSel.Name = "btnClearSel"
        Me.btnClearSel.Size = New System.Drawing.Size(180, 30)
        Me.btnClearSel.TabIndex = 11
        Me.btnClearSel.Text = "Clear selection"
        Me.btnClearSel.UseVisualStyleBackColor = True
        '
        'lblVersion
        '
        Me.lblVersion.AutoSize = True
        Me.lblVersion.ForeColor = System.Drawing.Color.Gray
        Me.lblVersion.Location = New System.Drawing.Point(330, 575)
        Me.lblVersion.Name = "lblVersion"
        Me.lblVersion.Size = New System.Drawing.Size(215, 13)
        Me.lblVersion.TabIndex = 1
        Me.lblVersion.Text = "Filmtopia Cinema Management System  v1.0"
        '
        'frmScreens
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(900, 600)
        Me.Controls.Add(Me.tabScreens)
        Me.Controls.Add(Me.lblVersion)
        Me.Name = "frmScreens"
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Text = "Screens"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        CType(Me.dgvScreens, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabScreens.ResumeLayout(False)
        Me.tabManage.ResumeLayout(False)
        Me.tabManage.PerformLayout()
        Me.tabSeatTypes.ResumeLayout(False)
        Me.tabSeatTypes.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents tabScreens As TabControl
    Friend WithEvents tabManage As TabPage
    Friend WithEvents tabSeatTypes As TabPage
    Friend WithEvents dgvScreens As DataGridView
    Friend WithEvents lblName As Label
    Friend WithEvents txtName As TextBox
    Friend WithEvents lblCapacity As Label
    Friend WithEvents txtCapacity As TextBox
    Friend WithEvents btnAdd As Button
    Friend WithEvents btnUpdate As Button
    Friend WithEvents btnDelete As Button
    Friend WithEvents btnClear As Button
    Friend WithEvents chkShowInactive As CheckBox
    Friend WithEvents lblSeatScreen As Label
    Friend WithEvents cboSeatTypeScreen As ComboBox
    Friend WithEvents lblSeatHint As Label
    Friend WithEvents lblScreenIndicator As Label
    Friend WithEvents pnlSeatTypeMap As Panel
    Friend WithEvents lblKeyTitle As Label
    Friend WithEvents lblStdKey As Label
    Friend WithEvents lblPremKey As Label
    Friend WithEvents lblAccKey As Label
    Friend WithEvents lblSetTo As Label
    Friend WithEvents cboTargetType As ComboBox
    Friend WithEvents btnApplyType As Button
    Friend WithEvents btnClearSel As Button
    Friend WithEvents lblVersion As Label
End Class
