<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmBookings
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
        Me.lblScreening = New System.Windows.Forms.Label()
        Me.cboScreening = New System.Windows.Forms.ComboBox()
        Me.lblCustomer = New System.Windows.Forms.Label()
        Me.cboCustomer = New System.Windows.Forms.ComboBox()
        Me.chkWalkIn = New System.Windows.Forms.CheckBox()
        Me.lblScreen = New System.Windows.Forms.Label()
        Me.pnlSeatMap = New System.Windows.Forms.Panel()
        Me.lblSwatchAvailable = New System.Windows.Forms.Label()
        Me.lblLegendAvailable = New System.Windows.Forms.Label()
        Me.lblSwatchSelected = New System.Windows.Forms.Label()
        Me.lblLegendSelected = New System.Windows.Forms.Label()
        Me.lblSwatchTaken = New System.Windows.Forms.Label()
        Me.lblLegendTaken = New System.Windows.Forms.Label()
        Me.lblTotal = New System.Windows.Forms.Label()
        Me.btnCreateBooking = New System.Windows.Forms.Button()
        Me.btnClear = New System.Windows.Forms.Button()
        Me.btnFoodOnly = New System.Windows.Forms.Button()
        Me.btnOrderFood = New System.Windows.Forms.Button()
        Me.lblCustomerBookings = New System.Windows.Forms.Label()
        Me.dgvCustomerBookings = New System.Windows.Forms.DataGridView()
        Me.lblVersion = New System.Windows.Forms.Label()
        CType(Me.dgvCustomerBookings, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblScreening
        '
        Me.lblScreening.AutoSize = True
        Me.lblScreening.Location = New System.Drawing.Point(12, 15)
        Me.lblScreening.Name = "lblScreening"
        Me.lblScreening.Size = New System.Drawing.Size(55, 13)
        Me.lblScreening.TabIndex = 0
        Me.lblScreening.Text = "Screening"
        '
        'cboScreening
        '
        Me.cboScreening.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboScreening.FormattingEnabled = True
        Me.cboScreening.Location = New System.Drawing.Point(82, 11)
        Me.cboScreening.Name = "cboScreening"
        Me.cboScreening.Size = New System.Drawing.Size(324, 21)
        Me.cboScreening.TabIndex = 1
        '
        'lblCustomer
        '
        Me.lblCustomer.AutoSize = True
        Me.lblCustomer.Location = New System.Drawing.Point(12, 44)
        Me.lblCustomer.Name = "lblCustomer"
        Me.lblCustomer.Size = New System.Drawing.Size(51, 13)
        Me.lblCustomer.TabIndex = 2
        Me.lblCustomer.Text = "Customer"
        '
        'cboCustomer
        '
        Me.cboCustomer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboCustomer.FormattingEnabled = True
        Me.cboCustomer.Location = New System.Drawing.Point(82, 41)
        Me.cboCustomer.Name = "cboCustomer"
        Me.cboCustomer.Size = New System.Drawing.Size(195, 21)
        Me.cboCustomer.TabIndex = 3
        '
        'chkWalkIn
        '
        Me.chkWalkIn.AutoSize = True
        Me.chkWalkIn.Location = New System.Drawing.Point(285, 43)
        Me.chkWalkIn.Margin = New System.Windows.Forms.Padding(3, 0, 3, 0)
        Me.chkWalkIn.Name = "chkWalkIn"
        Me.chkWalkIn.Size = New System.Drawing.Size(116, 17)
        Me.chkWalkIn.TabIndex = 19
        Me.chkWalkIn.Text = "Walk-in (no details)"
        Me.chkWalkIn.UseVisualStyleBackColor = True
        '
        'lblScreen
        '
        Me.lblScreen.Location = New System.Drawing.Point(12, 75)
        Me.lblScreen.Name = "lblScreen"
        Me.lblScreen.Size = New System.Drawing.Size(474, 16)
        Me.lblScreen.TabIndex = 4
        Me.lblScreen.Text = "----------------------------------  SCREEN  ----------------------------------"
        Me.lblScreen.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'pnlSeatMap
        '
        Me.pnlSeatMap.AutoScroll = True
        Me.pnlSeatMap.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlSeatMap.Location = New System.Drawing.Point(12, 98)
        Me.pnlSeatMap.Name = "pnlSeatMap"
        Me.pnlSeatMap.Size = New System.Drawing.Size(474, 368)
        Me.pnlSeatMap.TabIndex = 5
        '
        'lblSwatchAvailable
        '
        Me.lblSwatchAvailable.BackColor = System.Drawing.Color.FromArgb(CType(CType(220, Byte), Integer), CType(CType(220, Byte), Integer), CType(CType(220, Byte), Integer))
        Me.lblSwatchAvailable.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblSwatchAvailable.Location = New System.Drawing.Point(497, 101)
        Me.lblSwatchAvailable.Name = "lblSwatchAvailable"
        Me.lblSwatchAvailable.Size = New System.Drawing.Size(14, 15)
        Me.lblSwatchAvailable.TabIndex = 6
        '
        'lblLegendAvailable
        '
        Me.lblLegendAvailable.AutoSize = True
        Me.lblLegendAvailable.Location = New System.Drawing.Point(518, 102)
        Me.lblLegendAvailable.Name = "lblLegendAvailable"
        Me.lblLegendAvailable.Size = New System.Drawing.Size(50, 13)
        Me.lblLegendAvailable.TabIndex = 7
        Me.lblLegendAvailable.Text = "Available"
        '
        'lblSwatchSelected
        '
        Me.lblSwatchSelected.BackColor = System.Drawing.Color.Fuchsia
        Me.lblSwatchSelected.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblSwatchSelected.Location = New System.Drawing.Point(497, 121)
        Me.lblSwatchSelected.Name = "lblSwatchSelected"
        Me.lblSwatchSelected.Size = New System.Drawing.Size(14, 15)
        Me.lblSwatchSelected.TabIndex = 8
        '
        'lblLegendSelected
        '
        Me.lblLegendSelected.AutoSize = True
        Me.lblLegendSelected.Location = New System.Drawing.Point(518, 122)
        Me.lblLegendSelected.Name = "lblLegendSelected"
        Me.lblLegendSelected.Size = New System.Drawing.Size(49, 13)
        Me.lblLegendSelected.TabIndex = 9
        Me.lblLegendSelected.Text = "Selected"
        '
        'lblSwatchTaken
        '
        Me.lblSwatchTaken.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.lblSwatchTaken.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblSwatchTaken.Location = New System.Drawing.Point(497, 141)
        Me.lblSwatchTaken.Name = "lblSwatchTaken"
        Me.lblSwatchTaken.Size = New System.Drawing.Size(14, 15)
        Me.lblSwatchTaken.TabIndex = 10
        '
        'lblLegendTaken
        '
        Me.lblLegendTaken.AutoSize = True
        Me.lblLegendTaken.Location = New System.Drawing.Point(518, 142)
        Me.lblLegendTaken.Name = "lblLegendTaken"
        Me.lblLegendTaken.Size = New System.Drawing.Size(38, 13)
        Me.lblLegendTaken.TabIndex = 11
        Me.lblLegendTaken.Text = "Taken"
        '
        'lblTotal
        '
        Me.lblTotal.AutoSize = True
        Me.lblTotal.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lblTotal.Location = New System.Drawing.Point(495, 175)
        Me.lblTotal.Name = "lblTotal"
        Me.lblTotal.Size = New System.Drawing.Size(113, 19)
        Me.lblTotal.TabIndex = 12
        Me.lblTotal.Text = "0 seats selected"
        '
        'btnCreateBooking
        '
        Me.btnCreateBooking.Location = New System.Drawing.Point(497, 211)
        Me.btnCreateBooking.Name = "btnCreateBooking"
        Me.btnCreateBooking.Size = New System.Drawing.Size(250, 32)
        Me.btnCreateBooking.TabIndex = 13
        Me.btnCreateBooking.Text = "Create Booking"
        Me.btnCreateBooking.UseVisualStyleBackColor = True
        '
        'btnClear
        '
        Me.btnClear.Location = New System.Drawing.Point(497, 252)
        Me.btnClear.Name = "btnClear"
        Me.btnClear.Size = New System.Drawing.Size(250, 30)
        Me.btnClear.TabIndex = 14
        Me.btnClear.Text = "Clear Selection"
        Me.btnClear.UseVisualStyleBackColor = True
        '
        'btnFoodOnly
        '
        Me.btnFoodOnly.Location = New System.Drawing.Point(497, 290)
        Me.btnFoodOnly.Name = "btnFoodOnly"
        Me.btnFoodOnly.Size = New System.Drawing.Size(250, 30)
        Me.btnFoodOnly.TabIndex = 20
        Me.btnFoodOnly.Text = "Sell Food Only (No Seat)"
        Me.btnFoodOnly.UseVisualStyleBackColor = True
        '
        'btnOrderFood
        '
        Me.btnOrderFood.Enabled = False
        Me.btnOrderFood.Location = New System.Drawing.Point(497, 328)
        Me.btnOrderFood.Name = "btnOrderFood"
        Me.btnOrderFood.Size = New System.Drawing.Size(250, 30)
        Me.btnOrderFood.TabIndex = 16
        Me.btnOrderFood.Text = "Order Food"
        Me.btnOrderFood.UseVisualStyleBackColor = True
        '
        'lblCustomerBookings
        '
        Me.lblCustomerBookings.AutoSize = True
        Me.lblCustomerBookings.Location = New System.Drawing.Point(497, 365)
        Me.lblCustomerBookings.Name = "lblCustomerBookings"
        Me.lblCustomerBookings.Size = New System.Drawing.Size(105, 13)
        Me.lblCustomerBookings.TabIndex = 17
        Me.lblCustomerBookings.Text = "Customer's Bookings"
        '
        'dgvCustomerBookings
        '
        Me.dgvCustomerBookings.AllowUserToAddRows = False
        Me.dgvCustomerBookings.AllowUserToDeleteRows = False
        Me.dgvCustomerBookings.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvCustomerBookings.Font = New System.Drawing.Font("Segoe UI", 7.5!)
        Me.dgvCustomerBookings.Location = New System.Drawing.Point(497, 381)
        Me.dgvCustomerBookings.Name = "dgvCustomerBookings"
        Me.dgvCustomerBookings.ReadOnly = True
        Me.dgvCustomerBookings.RowHeadersVisible = False
        Me.dgvCustomerBookings.RowHeadersWidth = 51
        Me.dgvCustomerBookings.Size = New System.Drawing.Size(253, 89)
        Me.dgvCustomerBookings.TabIndex = 18
        '
        'lblVersion
        '
        Me.lblVersion.AutoSize = True
        Me.lblVersion.ForeColor = System.Drawing.Color.Gray
        Me.lblVersion.Location = New System.Drawing.Point(495, 486)
        Me.lblVersion.Name = "lblVersion"
        Me.lblVersion.Size = New System.Drawing.Size(215, 13)
        Me.lblVersion.TabIndex = 15
        Me.lblVersion.Text = "Filmtopia Cinema Management System  v1.0"
        '
        'frmBookings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(760, 508)
        Me.Controls.Add(Me.lblVersion)
        Me.Controls.Add(Me.dgvCustomerBookings)
        Me.Controls.Add(Me.lblCustomerBookings)
        Me.Controls.Add(Me.btnOrderFood)
        Me.Controls.Add(Me.btnFoodOnly)
        Me.Controls.Add(Me.btnClear)
        Me.Controls.Add(Me.btnCreateBooking)
        Me.Controls.Add(Me.lblTotal)
        Me.Controls.Add(Me.lblLegendTaken)
        Me.Controls.Add(Me.lblSwatchTaken)
        Me.Controls.Add(Me.lblLegendSelected)
        Me.Controls.Add(Me.lblSwatchSelected)
        Me.Controls.Add(Me.lblLegendAvailable)
        Me.Controls.Add(Me.lblSwatchAvailable)
        Me.Controls.Add(Me.pnlSeatMap)
        Me.Controls.Add(Me.lblScreen)
        Me.Controls.Add(Me.chkWalkIn)
        Me.Controls.Add(Me.cboCustomer)
        Me.Controls.Add(Me.lblCustomer)
        Me.Controls.Add(Me.cboScreening)
        Me.Controls.Add(Me.lblScreening)
        Me.Name = "frmBookings"
        Me.Text = "Bookings"
        CType(Me.dgvCustomerBookings, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblScreening As Label
    Friend WithEvents cboScreening As ComboBox
    Friend WithEvents lblCustomer As Label
    Friend WithEvents cboCustomer As ComboBox
    Friend WithEvents chkWalkIn As CheckBox
    Friend WithEvents lblScreen As Label
    Friend WithEvents pnlSeatMap As Panel
    Friend WithEvents lblSwatchAvailable As Label
    Friend WithEvents lblLegendAvailable As Label
    Friend WithEvents lblSwatchSelected As Label
    Friend WithEvents lblLegendSelected As Label
    Friend WithEvents lblSwatchTaken As Label
    Friend WithEvents lblLegendTaken As Label
    Friend WithEvents lblTotal As Label
    Friend WithEvents btnCreateBooking As Button
    Friend WithEvents btnClear As Button
    Friend WithEvents btnOrderFood As Button
    Friend WithEvents btnFoodOnly As Button
    Friend WithEvents lblCustomerBookings As Label
    Friend WithEvents dgvCustomerBookings As DataGridView
    Friend WithEvents lblVersion As Label
End Class
