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
        Me.components = New System.ComponentModel.Container()
        Me.lblHeading = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.lblSearch = New System.Windows.Forms.Label()
        Me.txtSearch = New System.Windows.Forms.TextBox()
        Me.lblFilter = New System.Windows.Forms.Label()
        Me.cboFilter = New System.Windows.Forms.ComboBox()
        Me.chkShowWithdrawn = New System.Windows.Forms.CheckBox()
        Me.btnExport = New System.Windows.Forms.Button()
        Me.lblGridCount = New System.Windows.Forms.Label()
        Me.lblNoRows = New System.Windows.Forms.Label()
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
        Me.btnWithdraw = New System.Windows.Forms.Button()
        Me.btnClear = New System.Windows.Forms.Button()
        Me.lblPicture = New System.Windows.Forms.Label()
        Me.lblNoPicture = New System.Windows.Forms.Label()
        Me.picFood = New System.Windows.Forms.PictureBox()
        Me.btnChoosePicture = New System.Windows.Forms.Button()
        Me.btnRemovePicture = New System.Windows.Forms.Button()
        Me.lblPreviewName = New System.Windows.Forms.Label()
        Me.lblPreviewMeta = New System.Windows.Forms.Label()
        Me.lblSaved = New System.Windows.Forms.Label()
        Me.lblVersion = New System.Windows.Forms.Label()
        Me.timerSearch = New System.Windows.Forms.Timer(Me.components)
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        CType(Me.dgvFoodItems, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picFood, System.ComponentModel.ISupportInitialize).BeginInit()
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
        Me.GroupBox1.Controls.Add(Me.lblSearch)
        Me.GroupBox1.Controls.Add(Me.txtSearch)
        Me.GroupBox1.Controls.Add(Me.lblFilter)
        Me.GroupBox1.Controls.Add(Me.cboFilter)
        Me.GroupBox1.Controls.Add(Me.chkShowWithdrawn)
        Me.GroupBox1.Controls.Add(Me.btnExport)
        Me.GroupBox1.Controls.Add(Me.lblGridCount)
        Me.GroupBox1.Controls.Add(Me.lblNoRows)
        Me.GroupBox1.Controls.Add(Me.dgvFoodItems)
        Me.GroupBox1.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.GroupBox1.Location = New System.Drawing.Point(16, 46)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(1148, 390)
        Me.GroupBox1.TabIndex = 1
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "What is on sale at the counter"
        '
        'lblSearch
        '
        Me.lblSearch.AutoSize = True
        Me.lblSearch.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lblSearch.Location = New System.Drawing.Point(16, 28)
        Me.lblSearch.Name = "lblSearch"
        Me.lblSearch.Size = New System.Drawing.Size(45, 15)
        Me.lblSearch.TabIndex = 0
        Me.lblSearch.Text = "Search"
        '
        'txtSearch
        '
        Me.txtSearch.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.txtSearch.Location = New System.Drawing.Point(90, 25)
        Me.txtSearch.Name = "txtSearch"
        Me.txtSearch.Size = New System.Drawing.Size(220, 23)
        Me.txtSearch.TabIndex = 1
        '
        'lblFilter
        '
        Me.lblFilter.AutoSize = True
        Me.lblFilter.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lblFilter.Location = New System.Drawing.Point(325, 28)
        Me.lblFilter.Name = "lblFilter"
        Me.lblFilter.Size = New System.Drawing.Size(55, 15)
        Me.lblFilter.TabIndex = 2
        Me.lblFilter.Text = "Category"
        '
        'cboFilter
        '
        Me.cboFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboFilter.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.cboFilter.Location = New System.Drawing.Point(405, 25)
        Me.cboFilter.Name = "cboFilter"
        Me.cboFilter.Size = New System.Drawing.Size(170, 23)
        Me.cboFilter.TabIndex = 3
        '
        'chkShowWithdrawn
        '
        Me.chkShowWithdrawn.AutoSize = True
        Me.chkShowWithdrawn.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.chkShowWithdrawn.Location = New System.Drawing.Point(590, 27)
        Me.chkShowWithdrawn.Name = "chkShowWithdrawn"
        Me.chkShowWithdrawn.Size = New System.Drawing.Size(160, 19)
        Me.chkShowWithdrawn.TabIndex = 4
        Me.chkShowWithdrawn.Text = "Include withdrawn items"
        Me.chkShowWithdrawn.UseVisualStyleBackColor = True
        '
        'btnExport
        '
        Me.btnExport.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnExport.Location = New System.Drawing.Point(810, 22)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(120, 28)
        Me.btnExport.TabIndex = 5
        Me.btnExport.Text = "Export to CSV"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'lblGridCount
        '
        Me.lblGridCount.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lblGridCount.Location = New System.Drawing.Point(940, 28)
        Me.lblGridCount.Name = "lblGridCount"
        Me.lblGridCount.Size = New System.Drawing.Size(192, 17)
        Me.lblGridCount.TabIndex = 6
        Me.lblGridCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblNoRows
        '
        Me.lblNoRows.Font = New System.Drawing.Font("Segoe UI", 11.0!)
        Me.lblNoRows.Location = New System.Drawing.Point(16, 58)
        Me.lblNoRows.Name = "lblNoRows"
        Me.lblNoRows.Size = New System.Drawing.Size(1116, 316)
        Me.lblNoRows.TabIndex = 7
        Me.lblNoRows.Text = "Nothing to show"
        Me.lblNoRows.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.lblNoRows.Visible = False
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
        Me.dgvFoodItems.Size = New System.Drawing.Size(1116, 316)
        Me.dgvFoodItems.TabIndex = 8
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
        Me.GroupBox2.Controls.Add(Me.btnWithdraw)
        Me.GroupBox2.Controls.Add(Me.btnClear)
        Me.GroupBox2.Controls.Add(Me.lblPicture)
        Me.GroupBox2.Controls.Add(Me.lblNoPicture)
        Me.GroupBox2.Controls.Add(Me.picFood)
        Me.GroupBox2.Controls.Add(Me.btnChoosePicture)
        Me.GroupBox2.Controls.Add(Me.btnRemovePicture)
        Me.GroupBox2.Controls.Add(Me.lblPreviewName)
        Me.GroupBox2.Controls.Add(Me.lblPreviewMeta)
        Me.GroupBox2.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.GroupBox2.Location = New System.Drawing.Point(16, 446)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(1148, 240)
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
        Me.txtName.MaxLength = 50
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
        Me.txtPrice.Location = New System.Drawing.Point(445, 58)
        Me.txtPrice.MaxLength = 6
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
        Me.cboCategory.Location = New System.Drawing.Point(110, 94)
        Me.cboCategory.MaxLength = 30
        Me.cboCategory.Name = "cboCategory"
        Me.cboCategory.Size = New System.Drawing.Size(250, 23)
        Me.cboCategory.TabIndex = 6
        '
        'btnAdd
        '
        Me.btnAdd.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me.btnAdd.Location = New System.Drawing.Point(600, 30)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(252, 42)
        Me.btnAdd.TabIndex = 7
        Me.btnAdd.Text = "ADD TO THE MENU"
        Me.btnAdd.UseVisualStyleBackColor = True
        '
        'btnUpdate
        '
        Me.btnUpdate.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnUpdate.Location = New System.Drawing.Point(600, 78)
        Me.btnUpdate.Name = "btnUpdate"
        Me.btnUpdate.Size = New System.Drawing.Size(121, 32)
        Me.btnUpdate.TabIndex = 8
        Me.btnUpdate.Text = "Save changes"
        Me.btnUpdate.UseVisualStyleBackColor = True
        '
        'btnDelete
        '
        Me.btnDelete.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnDelete.Location = New System.Drawing.Point(731, 78)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(121, 32)
        Me.btnDelete.TabIndex = 9
        Me.btnDelete.Text = "Delete"
        Me.btnDelete.UseVisualStyleBackColor = True
        '
        'btnWithdraw
        '
        Me.btnWithdraw.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnWithdraw.Location = New System.Drawing.Point(600, 116)
        Me.btnWithdraw.Name = "btnWithdraw"
        Me.btnWithdraw.Size = New System.Drawing.Size(252, 30)
        Me.btnWithdraw.TabIndex = 10
        Me.btnWithdraw.Text = "Take off the menu"
        Me.btnWithdraw.UseVisualStyleBackColor = True
        '
        'btnClear
        '
        Me.btnClear.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnClear.Location = New System.Drawing.Point(600, 152)
        Me.btnClear.Name = "btnClear"
        Me.btnClear.Size = New System.Drawing.Size(252, 28)
        Me.btnClear.TabIndex = 11
        Me.btnClear.Text = "Clear the boxes"
        Me.btnClear.UseVisualStyleBackColor = True
        '
        'lblPicture
        '
        Me.lblPicture.AutoSize = True
        Me.lblPicture.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lblPicture.Location = New System.Drawing.Point(880, 20)
        Me.lblPicture.Name = "lblPicture"
        Me.lblPicture.Size = New System.Drawing.Size(48, 15)
        Me.lblPicture.TabIndex = 12
        Me.lblPicture.Text = "Picture"
        '
        'lblNoPicture
        '
        Me.lblNoPicture.Location = New System.Drawing.Point(882, 110)
        Me.lblNoPicture.Name = "lblNoPicture"
        Me.lblNoPicture.Size = New System.Drawing.Size(136, 20)
        Me.lblNoPicture.TabIndex = 13
        Me.lblNoPicture.Text = "No picture"
        Me.lblNoPicture.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'picFood
        '
        Me.picFood.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.picFood.Location = New System.Drawing.Point(880, 54)
        Me.picFood.Name = "picFood"
        Me.picFood.Size = New System.Drawing.Size(140, 132)
        Me.picFood.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.picFood.TabIndex = 14
        Me.picFood.TabStop = False
        '
        'btnChoosePicture
        '
        Me.btnChoosePicture.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnChoosePicture.Location = New System.Drawing.Point(1026, 54)
        Me.btnChoosePicture.Name = "btnChoosePicture"
        Me.btnChoosePicture.Size = New System.Drawing.Size(106, 26)
        Me.btnChoosePicture.TabIndex = 15
        Me.btnChoosePicture.Text = "Choose..."
        Me.btnChoosePicture.UseVisualStyleBackColor = True
        '
        'btnRemovePicture
        '
        Me.btnRemovePicture.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnRemovePicture.Location = New System.Drawing.Point(1026, 86)
        Me.btnRemovePicture.Name = "btnRemovePicture"
        Me.btnRemovePicture.Size = New System.Drawing.Size(106, 26)
        Me.btnRemovePicture.TabIndex = 16
        Me.btnRemovePicture.Text = "Remove"
        Me.btnRemovePicture.UseVisualStyleBackColor = True
        '
        'lblPreviewName
        '
        Me.lblPreviewName.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lblPreviewName.Location = New System.Drawing.Point(880, 192)
        Me.lblPreviewName.Name = "lblPreviewName"
        Me.lblPreviewName.Size = New System.Drawing.Size(252, 20)
        Me.lblPreviewName.TabIndex = 17
        '
        'lblPreviewMeta
        '
        Me.lblPreviewMeta.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lblPreviewMeta.Location = New System.Drawing.Point(880, 212)
        Me.lblPreviewMeta.Name = "lblPreviewMeta"
        Me.lblPreviewMeta.Size = New System.Drawing.Size(252, 18)
        Me.lblPreviewMeta.TabIndex = 18
        '
        'lblSaved
        '
        Me.lblSaved.Location = New System.Drawing.Point(360, 696)
        Me.lblSaved.Name = "lblSaved"
        Me.lblSaved.Size = New System.Drawing.Size(804, 16)
        Me.lblSaved.Text = ""
        Me.lblSaved.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblVersion
        '
        Me.lblVersion.AutoSize = True
        Me.lblVersion.ForeColor = System.Drawing.Color.Gray
        Me.lblVersion.Location = New System.Drawing.Point(16, 696)
        Me.lblVersion.Name = "lblVersion"
        Me.lblVersion.Size = New System.Drawing.Size(271, 16)
        Me.lblVersion.TabIndex = 3
        Me.lblVersion.Text = "Filmtopia Cinema Management System  v1.0"
        '
        'timerSearch
        '
        Me.timerSearch.Interval = 300
        '
        'frmFoodItems
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1180, 730)
        Me.Controls.Add(Me.lblSaved)
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
        CType(Me.picFood, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblHeading As Label
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents lblSearch As Label
    Friend WithEvents txtSearch As TextBox
    Friend WithEvents lblFilter As Label
    Friend WithEvents cboFilter As ComboBox
    Friend WithEvents chkShowWithdrawn As CheckBox
    Friend WithEvents btnExport As Button
    Friend WithEvents lblGridCount As Label
    Friend WithEvents lblNoRows As Label
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
    Friend WithEvents btnWithdraw As Button
    Friend WithEvents btnClear As Button
    Friend WithEvents lblPicture As Label
    Friend WithEvents lblNoPicture As Label
    Friend WithEvents picFood As PictureBox
    Friend WithEvents btnChoosePicture As Button
    Friend WithEvents btnRemovePicture As Button
    Friend WithEvents lblPreviewName As Label
    Friend WithEvents lblPreviewMeta As Label
    Friend WithEvents lblSaved As Label
    Friend WithEvents lblVersion As Label
    Friend WithEvents timerSearch As Timer
End Class
