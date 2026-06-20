<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmFoodItems
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
        Me.dgvFoodItems = New System.Windows.Forms.DataGridView()
        Me.lblName = New System.Windows.Forms.Label()
        Me.txtName = New System.Windows.Forms.TextBox()
        Me.lblPrice = New System.Windows.Forms.Label()
        Me.txtPrice = New System.Windows.Forms.TextBox()
        Me.lblCategory = New System.Windows.Forms.Label()
        Me.txtCategory = New System.Windows.Forms.TextBox()
        Me.btnAdd = New System.Windows.Forms.Button()
        Me.btnUpdate = New System.Windows.Forms.Button()
        Me.btnDelete = New System.Windows.Forms.Button()
        Me.btnClear = New System.Windows.Forms.Button()
        Me.lblVersion = New System.Windows.Forms.Label()
        CType(Me.dgvFoodItems, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'dgvFoodItems
        '
        Me.dgvFoodItems.AllowUserToAddRows = False
        Me.dgvFoodItems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvFoodItems.Location = New System.Drawing.Point(16, 15)
        Me.dgvFoodItems.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.dgvFoodItems.MultiSelect = False
        Me.dgvFoodItems.Name = "dgvFoodItems"
        Me.dgvFoodItems.RowHeadersWidth = 51
        Me.dgvFoodItems.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvFoodItems.Size = New System.Drawing.Size(1038, 369)
        Me.dgvFoodItems.TabIndex = 0
        '
        'lblName
        '
        Me.lblName.AutoSize = True
        Me.lblName.Location = New System.Drawing.Point(16, 406)
        Me.lblName.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblName.Name = "lblName"
        Me.lblName.Size = New System.Drawing.Size(42, 16)
        Me.lblName.TabIndex = 1
        Me.lblName.Text = "Name"
        '
        'txtName
        '
        Me.txtName.Location = New System.Drawing.Point(160, 402)
        Me.txtName.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.txtName.Name = "txtName"
        Me.txtName.Size = New System.Drawing.Size(332, 22)
        Me.txtName.TabIndex = 2
        '
        'lblPrice
        '
        Me.lblPrice.AutoSize = True
        Me.lblPrice.Location = New System.Drawing.Point(16, 443)
        Me.lblPrice.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblPrice.Name = "lblPrice"
        Me.lblPrice.Size = New System.Drawing.Size(40, 16)
        Me.lblPrice.TabIndex = 3
        Me.lblPrice.Text = "Price"
        '
        'txtPrice
        '
        Me.txtPrice.Location = New System.Drawing.Point(160, 439)
        Me.txtPrice.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.txtPrice.Name = "txtPrice"
        Me.txtPrice.Size = New System.Drawing.Size(132, 22)
        Me.txtPrice.TabIndex = 4
        '
        'lblCategory
        '
        Me.lblCategory.AutoSize = True
        Me.lblCategory.Location = New System.Drawing.Point(16, 480)
        Me.lblCategory.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblCategory.Name = "lblCategory"
        Me.lblCategory.Size = New System.Drawing.Size(63, 16)
        Me.lblCategory.TabIndex = 5
        Me.lblCategory.Text = "Category"
        '
        'txtCategory
        '
        Me.txtCategory.Location = New System.Drawing.Point(160, 476)
        Me.txtCategory.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.txtCategory.Name = "txtCategory"
        Me.txtCategory.Size = New System.Drawing.Size(132, 22)
        Me.txtCategory.TabIndex = 6
        '
        'btnAdd
        '
        Me.btnAdd.Location = New System.Drawing.Point(600, 402)
        Me.btnAdd.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(120, 37)
        Me.btnAdd.TabIndex = 7
        Me.btnAdd.Text = "Add"
        Me.btnAdd.UseVisualStyleBackColor = True
        '
        'btnUpdate
        '
        Me.btnUpdate.Location = New System.Drawing.Point(728, 402)
        Me.btnUpdate.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnUpdate.Name = "btnUpdate"
        Me.btnUpdate.Size = New System.Drawing.Size(120, 37)
        Me.btnUpdate.TabIndex = 8
        Me.btnUpdate.Text = "Update"
        Me.btnUpdate.UseVisualStyleBackColor = True
        '
        'btnDelete
        '
        Me.btnDelete.Location = New System.Drawing.Point(600, 447)
        Me.btnDelete.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(120, 37)
        Me.btnDelete.TabIndex = 9
        Me.btnDelete.Text = "Delete"
        Me.btnDelete.UseVisualStyleBackColor = True
        '
        'btnClear
        '
        Me.btnClear.Location = New System.Drawing.Point(728, 447)
        Me.btnClear.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnClear.Name = "btnClear"
        Me.btnClear.Size = New System.Drawing.Size(120, 37)
        Me.btnClear.TabIndex = 10
        Me.btnClear.Text = "Clear"
        Me.btnClear.UseVisualStyleBackColor = True
        '
        'lblVersion
        '
        Me.lblVersion.AutoSize = True
        Me.lblVersion.ForeColor = System.Drawing.Color.Gray
        Me.lblVersion.Location = New System.Drawing.Point(733, 603)
        Me.lblVersion.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblVersion.Name = "lblVersion"
        Me.lblVersion.Size = New System.Drawing.Size(271, 16)
        Me.lblVersion.TabIndex = 11
        Me.lblVersion.Text = "Filmtopia Cinema Management System  v1.0"
        '
        'frmFoodItems
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1067, 640)
        Me.Controls.Add(Me.lblVersion)
        Me.Controls.Add(Me.btnClear)
        Me.Controls.Add(Me.btnDelete)
        Me.Controls.Add(Me.btnUpdate)
        Me.Controls.Add(Me.btnAdd)
        Me.Controls.Add(Me.txtCategory)
        Me.Controls.Add(Me.lblCategory)
        Me.Controls.Add(Me.txtPrice)
        Me.Controls.Add(Me.lblPrice)
        Me.Controls.Add(Me.txtName)
        Me.Controls.Add(Me.lblName)
        Me.Controls.Add(Me.dgvFoodItems)
        Me.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.Name = "frmFoodItems"
        Me.Text = "Food Items"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        CType(Me.dgvFoodItems, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents dgvFoodItems As DataGridView
    Friend WithEvents lblName As Label
    Friend WithEvents txtName As TextBox
    Friend WithEvents lblPrice As Label
    Friend WithEvents txtPrice As TextBox
    Friend WithEvents lblCategory As Label
    Friend WithEvents txtCategory As TextBox
    Friend WithEvents btnAdd As Button
    Friend WithEvents btnUpdate As Button
    Friend WithEvents btnDelete As Button
    Friend WithEvents btnClear As Button
    Friend WithEvents lblVersion As Label
End Class
