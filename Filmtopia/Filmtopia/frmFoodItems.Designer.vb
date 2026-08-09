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
        Me.lblHeading = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.lblFilter = New System.Windows.Forms.Label()
        Me.cboFilter = New System.Windows.Forms.ComboBox()
        Me.lblGridCount = New System.Windows.Forms.Label()
        Me.dgvFoodItems = New System.Windows.Forms.DataGridView()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.lblName = New System.Windows.Forms.Label()
        Me.txtName = New System.Windows.Forms.TextBox()
        Me.lblPrice = New System.Windows.Forms.Label()
        Me.txtPrice = New System.Windows.Forms.TextBox()
        Me.lblCategory = New System.Windows.Forms.Label()
        Me.cboCategory = New System.Windows.Forms.ComboBox()
        Me.btnAdd = New System.Windows.Forms.Button()
        Me.btnUpdate = New System.Windows.Forms.Button()
        Me.btnDelete = New System.Windows.Forms.Button()
        Me.btnClear = New System.Windows.Forms.Button()
        Me.lblVersion = New System.Windows.Forms.Label()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        CType(Me.dgvFoodItems, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblHeading
        '
        Me.lblHeading.AutoSize = True
        Me.lblHeading.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Bold)
        Me.lblHeading.Location = New System.Drawing.Point(16, 12)
        Me.lblHeading.Name = "lblHeading"
        Me.lblHeading.Size = New System.Drawing.Size(180, 25)
        Me.lblHeading.TabIndex = 0
        Me.lblHeading.Text = "Food and drink"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.lblFilter)
        Me.GroupBox1.Controls.Add(Me.cboFilter)
        Me.GroupBox1.Controls.Add(Me.lblGridCount)
        Me.GroupBox1.Controls.Add(Me.dgvFoodItems)
        Me.GroupBox1.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.GroupBox1.Location = New System.Drawing.Point(16, 46)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(868, 380)
        Me.GroupBox1.TabIndex = 1
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "What is on sale at the counter"
        '
        'lblFilter
        '
        Me.lblFilter.AutoSize = True
        Me.lblFilter.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lblFilter.Location = New System.Drawing.Point(16, 28)
        Me.lblFilter.Name = "lblFilter"
        Me.lblFilter.Size = New System.Drawing.Size(55, 15)
        Me.lblFilter.TabIndex = 0
        Me.lblFilter.Text = "Category"
        '
        'cboFilter
        '
        Me.cboFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboFilter.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.cboFilter.Location = New System.Drawing.Point(85, 25)
        Me.cboFilter.Name = "cboFilter"
        Me.cboFilter.Size = New System.Drawing.Size(180, 23)
        Me.cboFilter.TabIndex = 1
        '
        'lblGridCount
        '
        Me.lblGridCount.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lblGridCount.Location = New System.Drawing.Point(560, 28)
        Me.lblGridCount.Name = "lblGridCount"
        Me.lblGridCount.Size = New System.Drawing.Size(292, 17)
        Me.lblGridCount.TabIndex = 2
        Me.lblGridCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'dgvFoodItems
        '
        Me.dgvFoodItems.AllowUserToAddRows = False
        Me.dgvFoodItems.AllowUserToDeleteRows = False
        Me.dgvFoodItems.AllowUserToResizeRows = False
        Me.dgvFoodItems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvFoodItems.Location = New System.Drawing.Point(16, 58)
        Me.dgvFoodItems.MultiSelect = False
        Me.dgvFoodItems.Name = "dgvFoodItems"
        Me.dgvFoodItems.ReadOnly = True
        Me.dgvFoodItems.RowHeadersVisible = False
        Me.dgvFoodItems.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvFoodItems.Size = New System.Drawing.Size(836, 310)
        Me.dgvFoodItems.TabIndex = 3
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.lblStatus)
        Me.GroupBox2.Controls.Add(Me.lblName)
        Me.GroupBox2.Controls.Add(Me.txtName)
        Me.GroupBox2.Controls.Add(Me.lblPrice)
        Me.GroupBox2.Controls.Add(Me.txtPrice)
        Me.GroupBox2.Controls.Add(Me.lblCategory)
        Me.GroupBox2.Controls.Add(Me.cboCategory)
        Me.GroupBox2.Controls.Add(Me.btnAdd)
        Me.GroupBox2.Controls.Add(Me.btnUpdate)
        Me.GroupBox2.Controls.Add(Me.btnDelete)
        Me.GroupBox2.Controls.Add(Me.btnClear)
        Me.GroupBox2.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.GroupBox2.Location = New System.Drawing.Point(16, 436)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(868, 180)
        Me.GroupBox2.TabIndex = 2
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Put something new on the menu or change what is there"
        '
        'lblStatus
        '
        Me.lblStatus.AutoSize = True
        Me.lblStatus.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lblStatus.Location = New System.Drawing.Point(16, 24)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(140, 19)
        Me.lblStatus.TabIndex = 0
        Me.lblStatus.Text = "Adding a new item"
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
        Me.txtName.Location = New System.Drawing.Point(90, 58)
        Me.txtName.Name = "txtName"
        Me.txtName.Size = New System.Drawing.Size(250, 23)
        Me.txtName.TabIndex = 2
        '
        'lblPrice
        '
        Me.lblPrice.AutoSize = True
        Me.lblPrice.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lblPrice.Location = New System.Drawing.Point(360, 61)
        Me.lblPrice.Name = "lblPrice"
        Me.lblPrice.Size = New System.Drawing.Size(50, 15)
        Me.lblPrice.TabIndex = 3
        Me.lblPrice.Text = "Price (£)"
        '
        'txtPrice
        '
        Me.txtPrice.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.txtPrice.Location = New System.Drawing.Point(415, 58)
        Me.txtPrice.Name = "txtPrice"
        Me.txtPrice.Size = New System.Drawing.Size(90, 23)
        Me.txtPrice.TabIndex = 4
        Me.txtPrice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblCategory
        '
        Me.lblCategory.AutoSize = True
        Me.lblCategory.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lblCategory.Location = New System.Drawing.Point(16, 97)
        Me.lblCategory.Name = "lblCategory"
        Me.lblCategory.Size = New System.Drawing.Size(55, 15)
        Me.lblCategory.TabIndex = 5
        Me.lblCategory.Text = "Category"
        '
        'cboCategory
        '
        Me.cboCategory.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.cboCategory.Location = New System.Drawing.Point(90, 94)
        Me.cboCategory.Name = "cboCategory"
        Me.cboCategory.Size = New System.Drawing.Size(250, 23)
        Me.cboCategory.TabIndex = 6
        '
        'btnAdd
        '
        Me.btnAdd.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me.btnAdd.Location = New System.Drawing.Point(600, 55)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(252, 42)
        Me.btnAdd.TabIndex = 7
        Me.btnAdd.Text = "ADD TO THE MENU"
        Me.btnAdd.UseVisualStyleBackColor = True
        '
        'btnUpdate
        '
        Me.btnUpdate.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnUpdate.Location = New System.Drawing.Point(600, 103)
        Me.btnUpdate.Name = "btnUpdate"
        Me.btnUpdate.Size = New System.Drawing.Size(121, 32)
        Me.btnUpdate.TabIndex = 8
        Me.btnUpdate.Text = "Save changes"
        Me.btnUpdate.UseVisualStyleBackColor = True
        '
        'btnDelete
        '
        Me.btnDelete.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnDelete.Location = New System.Drawing.Point(731, 103)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(121, 32)
        Me.btnDelete.TabIndex = 9
        Me.btnDelete.Text = "Delete"
        Me.btnDelete.UseVisualStyleBackColor = True
        '
        'btnClear
        '
        Me.btnClear.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnClear.Location = New System.Drawing.Point(600, 141)
        Me.btnClear.Name = "btnClear"
        Me.btnClear.Size = New System.Drawing.Size(252, 28)
        Me.btnClear.TabIndex = 10
        Me.btnClear.Text = "Clear the boxes"
        Me.btnClear.UseVisualStyleBackColor = True
        '
        'lblVersion
        '
        Me.lblVersion.AutoSize = True
        Me.lblVersion.ForeColor = System.Drawing.Color.Gray
        Me.lblVersion.Location = New System.Drawing.Point(16, 626)
        Me.lblVersion.Name = "lblVersion"
        Me.lblVersion.Size = New System.Drawing.Size(271, 16)
        Me.lblVersion.TabIndex = 3
        Me.lblVersion.Text = "Filmtopia Cinema Management System  v1.0"
        '
        'frmFoodItems
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(900, 655)
        Me.Controls.Add(Me.lblVersion)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.lblHeading)
        Me.Name = "frmFoodItems"
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Text = "Food and Drink"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        CType(Me.dgvFoodItems, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblHeading As Label
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents lblFilter As Label
    Friend WithEvents cboFilter As ComboBox
    Friend WithEvents lblGridCount As Label
    Friend WithEvents dgvFoodItems As DataGridView
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents lblStatus As Label
    Friend WithEvents lblName As Label
    Friend WithEvents txtName As TextBox
    Friend WithEvents lblPrice As Label
    Friend WithEvents txtPrice As TextBox
    Friend WithEvents lblCategory As Label
    Friend WithEvents cboCategory As ComboBox
    Friend WithEvents btnAdd As Button
    Friend WithEvents btnUpdate As Button
    Friend WithEvents btnDelete As Button
    Friend WithEvents btnClear As Button
    Friend WithEvents lblVersion As Label
End Class
