<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmFoodOrder
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
        Me.lblBookingInfo = New System.Windows.Forms.Label()
        Me.lblFoodItem = New System.Windows.Forms.Label()
        Me.cboFoodItem = New System.Windows.Forms.ComboBox()
        Me.lblPrice = New System.Windows.Forms.Label()
        Me.lblQuantity = New System.Windows.Forms.Label()
        Me.txtQuantity = New System.Windows.Forms.TextBox()
        Me.btnAddItem = New System.Windows.Forms.Button()
        Me.dgvOrderItems = New System.Windows.Forms.DataGridView()
        Me.btnRemoveItem = New System.Windows.Forms.Button()
        Me.lblTotal = New System.Windows.Forms.Label()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.lblVersion = New System.Windows.Forms.Label()
        CType(Me.dgvOrderItems, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblBookingInfo
        '
        Me.lblBookingInfo.AutoSize = True
        Me.lblBookingInfo.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lblBookingInfo.Location = New System.Drawing.Point(16, 16)
        Me.lblBookingInfo.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblBookingInfo.Name = "lblBookingInfo"
        Me.lblBookingInfo.Size = New System.Drawing.Size(124, 23)
        Me.lblBookingInfo.TabIndex = 0
        Me.lblBookingInfo.Text = "Booking details"
        '
        'lblFoodItem
        '
        Me.lblFoodItem.AutoSize = True
        Me.lblFoodItem.Location = New System.Drawing.Point(16, 56)
        Me.lblFoodItem.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblFoodItem.Name = "lblFoodItem"
        Me.lblFoodItem.Size = New System.Drawing.Size(67, 16)
        Me.lblFoodItem.TabIndex = 1
        Me.lblFoodItem.Text = "Food Item"
        '
        'cboFoodItem
        '
        Me.cboFoodItem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboFoodItem.FormattingEnabled = True
        Me.cboFoodItem.Location = New System.Drawing.Point(110, 52)
        Me.cboFoodItem.Margin = New System.Windows.Forms.Padding(4)
        Me.cboFoodItem.Name = "cboFoodItem"
        Me.cboFoodItem.Size = New System.Drawing.Size(300, 24)
        Me.cboFoodItem.TabIndex = 2
        '
        'lblPrice
        '
        Me.lblPrice.AutoSize = True
        Me.lblPrice.Location = New System.Drawing.Point(420, 56)
        Me.lblPrice.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblPrice.Name = "lblPrice"
        Me.lblPrice.Size = New System.Drawing.Size(14, 16)
        Me.lblPrice.TabIndex = 3
        Me.lblPrice.Text = " "
        '
        'lblQuantity
        '
        Me.lblQuantity.AutoSize = True
        Me.lblQuantity.Location = New System.Drawing.Point(16, 92)
        Me.lblQuantity.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblQuantity.Name = "lblQuantity"
        Me.lblQuantity.Size = New System.Drawing.Size(60, 16)
        Me.lblQuantity.TabIndex = 4
        Me.lblQuantity.Text = "Quantity"
        '
        'txtQuantity
        '
        Me.txtQuantity.Location = New System.Drawing.Point(110, 88)
        Me.txtQuantity.Margin = New System.Windows.Forms.Padding(4)
        Me.txtQuantity.Name = "txtQuantity"
        Me.txtQuantity.Size = New System.Drawing.Size(60, 22)
        Me.txtQuantity.TabIndex = 5
        Me.txtQuantity.Text = "1"
        '
        'btnAddItem
        '
        Me.btnAddItem.Location = New System.Drawing.Point(184, 86)
        Me.btnAddItem.Margin = New System.Windows.Forms.Padding(4)
        Me.btnAddItem.Name = "btnAddItem"
        Me.btnAddItem.Size = New System.Drawing.Size(140, 30)
        Me.btnAddItem.TabIndex = 6
        Me.btnAddItem.Text = "Add To Order"
        Me.btnAddItem.UseVisualStyleBackColor = True
        '
        'dgvOrderItems
        '
        Me.dgvOrderItems.AllowUserToAddRows = False
        Me.dgvOrderItems.AllowUserToDeleteRows = False
        Me.dgvOrderItems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvOrderItems.Location = New System.Drawing.Point(16, 140)
        Me.dgvOrderItems.Margin = New System.Windows.Forms.Padding(4)
        Me.dgvOrderItems.Name = "dgvOrderItems"
        Me.dgvOrderItems.ReadOnly = True
        Me.dgvOrderItems.RowHeadersWidth = 51
        Me.dgvOrderItems.Size = New System.Drawing.Size(560, 250)
        Me.dgvOrderItems.TabIndex = 7
        '
        'btnRemoveItem
        '
        Me.btnRemoveItem.Location = New System.Drawing.Point(16, 404)
        Me.btnRemoveItem.Margin = New System.Windows.Forms.Padding(4)
        Me.btnRemoveItem.Name = "btnRemoveItem"
        Me.btnRemoveItem.Size = New System.Drawing.Size(140, 37)
        Me.btnRemoveItem.TabIndex = 8
        Me.btnRemoveItem.Text = "Remove Selected"
        Me.btnRemoveItem.UseVisualStyleBackColor = True
        '
        'lblTotal
        '
        Me.lblTotal.AutoSize = True
        Me.lblTotal.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lblTotal.Location = New System.Drawing.Point(380, 410)
        Me.lblTotal.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblTotal.Name = "lblTotal"
        Me.lblTotal.Size = New System.Drawing.Size(82, 23)
        Me.lblTotal.TabIndex = 9
        Me.lblTotal.Text = "Total: £0.00"
        '
        'btnClose
        '
        Me.btnClose.Location = New System.Drawing.Point(460, 450)
        Me.btnClose.Margin = New System.Windows.Forms.Padding(4)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(116, 37)
        Me.btnClose.TabIndex = 10
        Me.btnClose.Text = "Close"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'lblVersion
        '
        Me.lblVersion.AutoSize = True
        Me.lblVersion.ForeColor = System.Drawing.Color.Gray
        Me.lblVersion.Location = New System.Drawing.Point(16, 460)
        Me.lblVersion.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblVersion.Name = "lblVersion"
        Me.lblVersion.Size = New System.Drawing.Size(271, 16)
        Me.lblVersion.TabIndex = 11
        Me.lblVersion.Text = "Filmtopia Cinema Management System  v1.0"
        '
        'frmFoodOrder
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(594, 500)
        Me.Controls.Add(Me.lblVersion)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.lblTotal)
        Me.Controls.Add(Me.btnRemoveItem)
        Me.Controls.Add(Me.dgvOrderItems)
        Me.Controls.Add(Me.btnAddItem)
        Me.Controls.Add(Me.txtQuantity)
        Me.Controls.Add(Me.lblQuantity)
        Me.Controls.Add(Me.lblPrice)
        Me.Controls.Add(Me.cboFoodItem)
        Me.Controls.Add(Me.lblFoodItem)
        Me.Controls.Add(Me.lblBookingInfo)
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "frmFoodOrder"
        Me.Text = "Food Order"
        CType(Me.dgvOrderItems, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblBookingInfo As Label
    Friend WithEvents lblFoodItem As Label
    Friend WithEvents cboFoodItem As ComboBox
    Friend WithEvents lblPrice As Label
    Friend WithEvents lblQuantity As Label
    Friend WithEvents txtQuantity As TextBox
    Friend WithEvents btnAddItem As Button
    Friend WithEvents dgvOrderItems As DataGridView
    Friend WithEvents btnRemoveItem As Button
    Friend WithEvents lblTotal As Label
    Friend WithEvents btnClose As Button
    Friend WithEvents lblVersion As Label
End Class
