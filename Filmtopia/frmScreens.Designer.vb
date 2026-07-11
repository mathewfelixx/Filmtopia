<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmScreens
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
        Me.dgvScreens = New System.Windows.Forms.DataGridView()
        Me.lblName = New System.Windows.Forms.Label()
        Me.txtName = New System.Windows.Forms.TextBox()
        Me.lblSeats = New System.Windows.Forms.Label()
        Me.txtSeats = New System.Windows.Forms.TextBox()
        Me.btnAdd = New System.Windows.Forms.Button()
        Me.btnUpdate = New System.Windows.Forms.Button()
        Me.btnDelete = New System.Windows.Forms.Button()
        Me.btnClear = New System.Windows.Forms.Button()
        Me.lblReason = New System.Windows.Forms.Label()
        Me.txtReason = New System.Windows.Forms.TextBox()
        Me.btnOutOfService = New System.Windows.Forms.Button()
        CType(Me.dgvScreens, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'dgvScreens
        '
        Me.dgvScreens.AllowUserToAddRows = False
        Me.dgvScreens.AllowUserToDeleteRows = False
        Me.dgvScreens.AllowUserToResizeRows = False
        Me.dgvScreens.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.dgvScreens.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvScreens.Location = New System.Drawing.Point(16, 15)
        Me.dgvScreens.MultiSelect = False
        Me.dgvScreens.Name = "dgvScreens"
        Me.dgvScreens.ReadOnly = True
        Me.dgvScreens.RowHeadersVisible = False
        Me.dgvScreens.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvScreens.Size = New System.Drawing.Size(880, 369)
        Me.dgvScreens.TabIndex = 0
        '
        'lblName
        '
        Me.lblName.AutoSize = True
        Me.lblName.Location = New System.Drawing.Point(16, 406)
        Me.lblName.Name = "lblName"
        Me.lblName.Size = New System.Drawing.Size(43, 16)
        Me.lblName.TabIndex = 1
        Me.lblName.Text = "Name"
        '
        'txtName
        '
        Me.txtName.Location = New System.Drawing.Point(160, 402)
        Me.txtName.Name = "txtName"
        Me.txtName.Size = New System.Drawing.Size(332, 22)
        Me.txtName.TabIndex = 2
        '
        'lblSeats
        '
        Me.lblSeats.AutoSize = True
        Me.lblSeats.Location = New System.Drawing.Point(16, 443)
        Me.lblSeats.Name = "lblSeats"
        Me.lblSeats.Size = New System.Drawing.Size(111, 16)
        Me.lblSeats.TabIndex = 3
        Me.lblSeats.Text = "Number of seats"
        '
        'txtSeats
        '
        Me.txtSeats.Location = New System.Drawing.Point(160, 439)
        Me.txtSeats.Name = "txtSeats"
        Me.txtSeats.Size = New System.Drawing.Size(132, 22)
        Me.txtSeats.TabIndex = 4
        '
        'btnAdd
        '
        Me.btnAdd.Location = New System.Drawing.Point(600, 402)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(120, 37)
        Me.btnAdd.TabIndex = 5
        Me.btnAdd.Text = "Add"
        Me.btnAdd.UseVisualStyleBackColor = True
        '
        'btnUpdate
        '
        Me.btnUpdate.Location = New System.Drawing.Point(728, 402)
        Me.btnUpdate.Name = "btnUpdate"
        Me.btnUpdate.Size = New System.Drawing.Size(120, 37)
        Me.btnUpdate.TabIndex = 6
        Me.btnUpdate.Text = "Update"
        Me.btnUpdate.UseVisualStyleBackColor = True
        '
        'btnDelete
        '
        Me.btnDelete.Location = New System.Drawing.Point(600, 447)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(120, 37)
        Me.btnDelete.TabIndex = 7
        Me.btnDelete.Text = "Delete"
        Me.btnDelete.UseVisualStyleBackColor = True
        '
        'btnClear
        '
        Me.btnClear.Location = New System.Drawing.Point(728, 447)
        Me.btnClear.Name = "btnClear"
        Me.btnClear.Size = New System.Drawing.Size(120, 37)
        Me.btnClear.TabIndex = 8
        Me.btnClear.Text = "Clear"
        Me.btnClear.UseVisualStyleBackColor = True
        '
        'lblReason
        '
        Me.lblReason.AutoSize = True
        Me.lblReason.Location = New System.Drawing.Point(16, 496)
        Me.lblReason.Name = "lblReason"
        Me.lblReason.Size = New System.Drawing.Size(55, 16)
        Me.lblReason.TabIndex = 9
        Me.lblReason.Text = "Reason"
        '
        'txtReason
        '
        Me.txtReason.Location = New System.Drawing.Point(160, 492)
        Me.txtReason.Name = "txtReason"
        Me.txtReason.Size = New System.Drawing.Size(332, 22)
        Me.txtReason.TabIndex = 10
        '
        'btnOutOfService
        '
        Me.btnOutOfService.Location = New System.Drawing.Point(600, 492)
        Me.btnOutOfService.Name = "btnOutOfService"
        Me.btnOutOfService.Size = New System.Drawing.Size(120, 37)
        Me.btnOutOfService.TabIndex = 11
        Me.btnOutOfService.Text = "Out of Service"
        Me.btnOutOfService.UseVisualStyleBackColor = True
        '
        'frmScreens
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(912, 545)
        Me.Controls.Add(Me.btnOutOfService)
        Me.Controls.Add(Me.txtReason)
        Me.Controls.Add(Me.lblReason)
        Me.Controls.Add(Me.btnClear)
        Me.Controls.Add(Me.btnDelete)
        Me.Controls.Add(Me.btnUpdate)
        Me.Controls.Add(Me.btnAdd)
        Me.Controls.Add(Me.txtSeats)
        Me.Controls.Add(Me.lblSeats)
        Me.Controls.Add(Me.txtName)
        Me.Controls.Add(Me.lblName)
        Me.Controls.Add(Me.dgvScreens)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "frmScreens"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Filmtopia - Screens"
        CType(Me.dgvScreens, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents dgvScreens As System.Windows.Forms.DataGridView
    Friend WithEvents lblName As System.Windows.Forms.Label
    Friend WithEvents txtName As System.Windows.Forms.TextBox
    Friend WithEvents lblSeats As System.Windows.Forms.Label
    Friend WithEvents txtSeats As System.Windows.Forms.TextBox
    Friend WithEvents btnAdd As System.Windows.Forms.Button
    Friend WithEvents btnUpdate As System.Windows.Forms.Button
    Friend WithEvents btnDelete As System.Windows.Forms.Button
    Friend WithEvents btnClear As System.Windows.Forms.Button
    Friend WithEvents lblReason As System.Windows.Forms.Label
    Friend WithEvents txtReason As System.Windows.Forms.TextBox
    Friend WithEvents btnOutOfService As System.Windows.Forms.Button
End Class
