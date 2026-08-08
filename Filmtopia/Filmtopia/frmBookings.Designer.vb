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
        Me.lblHeading = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.lblScreening = New System.Windows.Forms.Label()
        Me.cboScreening = New System.Windows.Forms.ComboBox()
        Me.lblCustomer = New System.Windows.Forms.Label()
        Me.cboCustomer = New System.Windows.Forms.ComboBox()
        Me.chkWalkIn = New System.Windows.Forms.CheckBox()
        Me.lblScreen = New System.Windows.Forms.Label()
        Me.pnlSeatMap = New System.Windows.Forms.Panel()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.lblSwatchAvailable = New System.Windows.Forms.Label()
        Me.lblLegendAvailable = New System.Windows.Forms.Label()
        Me.lblSwatchSelected = New System.Windows.Forms.Label()
        Me.lblLegendSelected = New System.Windows.Forms.Label()
        Me.lblSwatchTaken = New System.Windows.Forms.Label()
        Me.lblLegendTaken = New System.Windows.Forms.Label()
        Me.lblTotal = New System.Windows.Forms.Label()
        Me.btnCreateBooking = New System.Windows.Forms.Button()
        Me.btnClear = New System.Windows.Forms.Button()
        Me.btnOrderFood = New System.Windows.Forms.Button()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.lblFoodItem = New System.Windows.Forms.Label()
        Me.cboFoodItem = New System.Windows.Forms.ComboBox()
        Me.lblQty = New System.Windows.Forms.Label()
        Me.txtQuantity = New System.Windows.Forms.TextBox()
        Me.btnAddFood = New System.Windows.Forms.Button()
        Me.btnRemoveFood = New System.Windows.Forms.Button()
        Me.dgvPendingFood = New System.Windows.Forms.DataGridView()
        Me.lblTickets = New System.Windows.Forms.Label()
        Me.lblFoodTotal = New System.Windows.Forms.Label()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.lblCustomerBookings = New System.Windows.Forms.Label()
        Me.dgvCustomerBookings = New System.Windows.Forms.DataGridView()
        Me.lblVersion = New System.Windows.Forms.Label()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        CType(Me.dgvPendingFood, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox3.SuspendLayout()
        CType(Me.dgvCustomerBookings, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblHeading
        '
        Me.lblHeading.AutoSize = True
        Me.lblHeading.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Bold)
        Me.lblHeading.Location = New System.Drawing.Point(16, 12)
        Me.lblHeading.Name = "lblHeading"
        Me.lblHeading.Size = New System.Drawing.Size(160, 25)
        Me.lblHeading.TabIndex = 0
        Me.lblHeading.Text = "Make a booking"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.lblScreening)
        Me.GroupBox1.Controls.Add(Me.cboScreening)
        Me.GroupBox1.Controls.Add(Me.lblCustomer)
        Me.GroupBox1.Controls.Add(Me.cboCustomer)
        Me.GroupBox1.Controls.Add(Me.chkWalkIn)
        Me.GroupBox1.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.GroupBox1.Location = New System.Drawing.Point(16, 46)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(1148, 76)
        Me.GroupBox1.TabIndex = 1
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Who is it for and what are they seeing"
        '
        'lblScreening
        '
        Me.lblScreening.AutoSize = True
        Me.lblScreening.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lblScreening.Location = New System.Drawing.Point(16, 33)
        Me.lblScreening.Name = "lblScreening"
        Me.lblScreening.Size = New System.Drawing.Size(60, 15)
        Me.lblScreening.TabIndex = 0
        Me.lblScreening.Text = "Screening"
        '
        'cboScreening
        '
        Me.cboScreening.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboScreening.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.cboScreening.Location = New System.Drawing.Point(90, 30)
        Me.cboScreening.Name = "cboScreening"
        Me.cboScreening.Size = New System.Drawing.Size(360, 23)
        Me.cboScreening.TabIndex = 1
        '
        'lblCustomer
        '
        Me.lblCustomer.AutoSize = True
        Me.lblCustomer.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lblCustomer.Location = New System.Drawing.Point(480, 33)
        Me.lblCustomer.Name = "lblCustomer"
        Me.lblCustomer.Size = New System.Drawing.Size(60, 15)
        Me.lblCustomer.TabIndex = 2
        Me.lblCustomer.Text = "Customer"
        '
        'cboCustomer
        '
        Me.cboCustomer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboCustomer.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.cboCustomer.Location = New System.Drawing.Point(555, 30)
        Me.cboCustomer.Name = "cboCustomer"
        Me.cboCustomer.Size = New System.Drawing.Size(240, 23)
        Me.cboCustomer.TabIndex = 3
        '
        'chkWalkIn
        '
        Me.chkWalkIn.AutoSize = True
        Me.chkWalkIn.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.chkWalkIn.Location = New System.Drawing.Point(812, 32)
        Me.chkWalkIn.Margin = New System.Windows.Forms.Padding(3, 0, 3, 0)
        Me.chkWalkIn.Name = "chkWalkIn"
        Me.chkWalkIn.Size = New System.Drawing.Size(140, 19)
        Me.chkWalkIn.TabIndex = 4
        Me.chkWalkIn.Text = "Walk-in (no details)"
        Me.chkWalkIn.UseVisualStyleBackColor = True
        '
        'lblScreen
        '
        Me.lblScreen.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.lblScreen.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblScreen.ForeColor = System.Drawing.Color.White
        Me.lblScreen.Location = New System.Drawing.Point(16, 136)
        Me.lblScreen.Name = "lblScreen"
        Me.lblScreen.Size = New System.Drawing.Size(490, 26)
        Me.lblScreen.TabIndex = 2
        Me.lblScreen.Text = "S C R E E N"
        Me.lblScreen.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'pnlSeatMap
        '
        Me.pnlSeatMap.AutoScroll = True
        Me.pnlSeatMap.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlSeatMap.Location = New System.Drawing.Point(16, 170)
        Me.pnlSeatMap.Name = "pnlSeatMap"
        Me.pnlSeatMap.Size = New System.Drawing.Size(490, 382)
        Me.pnlSeatMap.TabIndex = 3
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.lblSwatchAvailable)
        Me.GroupBox2.Controls.Add(Me.lblLegendAvailable)
        Me.GroupBox2.Controls.Add(Me.lblSwatchSelected)
        Me.GroupBox2.Controls.Add(Me.lblLegendSelected)
        Me.GroupBox2.Controls.Add(Me.lblSwatchTaken)
        Me.GroupBox2.Controls.Add(Me.lblLegendTaken)
        Me.GroupBox2.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.GroupBox2.Location = New System.Drawing.Point(522, 136)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(642, 62)
        Me.GroupBox2.TabIndex = 4
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "What the seat colours mean"
        '
        'lblSwatchAvailable
        '
        Me.lblSwatchAvailable.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblSwatchAvailable.Location = New System.Drawing.Point(16, 30)
        Me.lblSwatchAvailable.Name = "lblSwatchAvailable"
        Me.lblSwatchAvailable.Size = New System.Drawing.Size(16, 16)
        Me.lblSwatchAvailable.TabIndex = 0
        '
        'lblLegendAvailable
        '
        Me.lblLegendAvailable.AutoSize = True
        Me.lblLegendAvailable.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lblLegendAvailable.Location = New System.Drawing.Point(40, 31)
        Me.lblLegendAvailable.Name = "lblLegendAvailable"
        Me.lblLegendAvailable.Size = New System.Drawing.Size(55, 15)
        Me.lblLegendAvailable.TabIndex = 1
        Me.lblLegendAvailable.Text = "Available"
        '
        'lblSwatchSelected
        '
        Me.lblSwatchSelected.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblSwatchSelected.Location = New System.Drawing.Point(170, 30)
        Me.lblSwatchSelected.Name = "lblSwatchSelected"
        Me.lblSwatchSelected.Size = New System.Drawing.Size(16, 16)
        Me.lblSwatchSelected.TabIndex = 2
        '
        'lblLegendSelected
        '
        Me.lblLegendSelected.AutoSize = True
        Me.lblLegendSelected.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lblLegendSelected.Location = New System.Drawing.Point(194, 31)
        Me.lblLegendSelected.Name = "lblLegendSelected"
        Me.lblLegendSelected.Size = New System.Drawing.Size(55, 15)
        Me.lblLegendSelected.TabIndex = 3
        Me.lblLegendSelected.Text = "Selected"
        '
        'lblSwatchTaken
        '
        Me.lblSwatchTaken.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblSwatchTaken.Location = New System.Drawing.Point(320, 30)
        Me.lblSwatchTaken.Name = "lblSwatchTaken"
        Me.lblSwatchTaken.Size = New System.Drawing.Size(16, 16)
        Me.lblSwatchTaken.TabIndex = 4
        '
        'lblLegendTaken
        '
        Me.lblLegendTaken.AutoSize = True
        Me.lblLegendTaken.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lblLegendTaken.Location = New System.Drawing.Point(344, 31)
        Me.lblLegendTaken.Name = "lblLegendTaken"
        Me.lblLegendTaken.Size = New System.Drawing.Size(80, 15)
        Me.lblLegendTaken.TabIndex = 5
        Me.lblLegendTaken.Text = "Already taken"
        '
        'lblTotal
        '
        Me.lblTotal.AutoSize = True
        Me.lblTotal.Font = New System.Drawing.Font("Segoe UI", 13.0!, System.Drawing.FontStyle.Bold)
        Me.lblTotal.Location = New System.Drawing.Point(526, 498)
        Me.lblTotal.Name = "lblTotal"
        Me.lblTotal.Size = New System.Drawing.Size(160, 23)
        Me.lblTotal.TabIndex = 5
        Me.lblTotal.Text = "0 seats selected"
        '
        'btnCreateBooking
        '
        Me.btnCreateBooking.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me.btnCreateBooking.Location = New System.Drawing.Point(522, 538)
        Me.btnCreateBooking.Name = "btnCreateBooking"
        Me.btnCreateBooking.Size = New System.Drawing.Size(642, 46)
        Me.btnCreateBooking.TabIndex = 6
        Me.btnCreateBooking.Text = "COMPLETE SALE"
        Me.btnCreateBooking.UseVisualStyleBackColor = True
        '
        'btnClear
        '
        Me.btnClear.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnClear.Location = New System.Drawing.Point(522, 592)
        Me.btnClear.Name = "btnClear"
        Me.btnClear.Size = New System.Drawing.Size(315, 32)
        Me.btnClear.TabIndex = 7
        Me.btnClear.Text = "Clear Sale"
        Me.btnClear.UseVisualStyleBackColor = True
        '
        '
        'btnOrderFood
        '
        Me.btnOrderFood.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnOrderFood.Location = New System.Drawing.Point(849, 592)
        Me.btnOrderFood.Name = "btnOrderFood"
        Me.btnOrderFood.Size = New System.Drawing.Size(315, 32)
        Me.btnOrderFood.TabIndex = 9
        Me.btnOrderFood.Text = "Edit Food On A Past Booking"
        Me.btnOrderFood.UseVisualStyleBackColor = True
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.lblFoodItem)
        Me.GroupBox4.Controls.Add(Me.cboFoodItem)
        Me.GroupBox4.Controls.Add(Me.lblQty)
        Me.GroupBox4.Controls.Add(Me.txtQuantity)
        Me.GroupBox4.Controls.Add(Me.btnAddFood)
        Me.GroupBox4.Controls.Add(Me.btnRemoveFood)
        Me.GroupBox4.Controls.Add(Me.dgvPendingFood)
        Me.GroupBox4.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.GroupBox4.Location = New System.Drawing.Point(522, 206)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(642, 236)
        Me.GroupBox4.TabIndex = 11
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Food and drink for this sale"
        '
        'lblFoodItem
        '
        Me.lblFoodItem.AutoSize = True
        Me.lblFoodItem.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lblFoodItem.Location = New System.Drawing.Point(16, 33)
        Me.lblFoodItem.Name = "lblFoodItem"
        Me.lblFoodItem.Size = New System.Drawing.Size(30, 15)
        Me.lblFoodItem.TabIndex = 0
        Me.lblFoodItem.Text = "Item"
        '
        'cboFoodItem
        '
        Me.cboFoodItem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboFoodItem.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.cboFoodItem.Location = New System.Drawing.Point(56, 30)
        Me.cboFoodItem.Name = "cboFoodItem"
        Me.cboFoodItem.Size = New System.Drawing.Size(240, 23)
        Me.cboFoodItem.TabIndex = 1
        '
        'lblQty
        '
        Me.lblQty.AutoSize = True
        Me.lblQty.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lblQty.Location = New System.Drawing.Point(310, 33)
        Me.lblQty.Name = "lblQty"
        Me.lblQty.Size = New System.Drawing.Size(25, 15)
        Me.lblQty.TabIndex = 2
        Me.lblQty.Text = "Qty"
        '
        'txtQuantity
        '
        Me.txtQuantity.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.txtQuantity.Location = New System.Drawing.Point(340, 30)
        Me.txtQuantity.Name = "txtQuantity"
        Me.txtQuantity.Size = New System.Drawing.Size(50, 23)
        Me.txtQuantity.TabIndex = 3
        Me.txtQuantity.Text = "1"
        Me.txtQuantity.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'btnAddFood
        '
        Me.btnAddFood.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnAddFood.Location = New System.Drawing.Point(402, 29)
        Me.btnAddFood.Name = "btnAddFood"
        Me.btnAddFood.Size = New System.Drawing.Size(90, 26)
        Me.btnAddFood.TabIndex = 4
        Me.btnAddFood.Text = "Add"
        Me.btnAddFood.UseVisualStyleBackColor = True
        '
        'btnRemoveFood
        '
        Me.btnRemoveFood.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnRemoveFood.Location = New System.Drawing.Point(500, 29)
        Me.btnRemoveFood.Name = "btnRemoveFood"
        Me.btnRemoveFood.Size = New System.Drawing.Size(110, 26)
        Me.btnRemoveFood.TabIndex = 5
        Me.btnRemoveFood.Text = "Remove"
        Me.btnRemoveFood.UseVisualStyleBackColor = True
        '
        'dgvPendingFood
        '
        Me.dgvPendingFood.AllowUserToAddRows = False
        Me.dgvPendingFood.AllowUserToDeleteRows = False
        Me.dgvPendingFood.AllowUserToResizeRows = False
        Me.dgvPendingFood.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dgvPendingFood.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvPendingFood.Location = New System.Drawing.Point(16, 66)
        Me.dgvPendingFood.Name = "dgvPendingFood"
        Me.dgvPendingFood.ReadOnly = True
        Me.dgvPendingFood.RowHeadersVisible = False
        Me.dgvPendingFood.RowTemplate.Height = 26
        Me.dgvPendingFood.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvPendingFood.Size = New System.Drawing.Size(610, 156)
        Me.dgvPendingFood.TabIndex = 6
        '
        'lblTickets
        '
        Me.lblTickets.AutoSize = True
        Me.lblTickets.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.lblTickets.Location = New System.Drawing.Point(526, 452)
        Me.lblTickets.Name = "lblTickets"
        Me.lblTickets.Size = New System.Drawing.Size(120, 17)
        Me.lblTickets.TabIndex = 12
        Me.lblTickets.Text = "Tickets (0)"
        '
        'lblFoodTotal
        '
        Me.lblFoodTotal.AutoSize = True
        Me.lblFoodTotal.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.lblFoodTotal.Location = New System.Drawing.Point(526, 474)
        Me.lblFoodTotal.Name = "lblFoodTotal"
        Me.lblFoodTotal.Size = New System.Drawing.Size(120, 17)
        Me.lblFoodTotal.TabIndex = 13
        Me.lblFoodTotal.Text = "Food"
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.lblCustomerBookings)
        Me.GroupBox3.Controls.Add(Me.dgvCustomerBookings)
        Me.GroupBox3.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.GroupBox3.Location = New System.Drawing.Point(16, 560)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(490, 110)
        Me.GroupBox3.TabIndex = 10
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Other bookings this customer has"
        '
        'lblCustomerBookings
        '
        Me.lblCustomerBookings.AutoSize = True
        Me.lblCustomerBookings.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.lblCustomerBookings.Location = New System.Drawing.Point(14, 22)
        Me.lblCustomerBookings.Name = "lblCustomerBookings"
        Me.lblCustomerBookings.Size = New System.Drawing.Size(200, 13)
        Me.lblCustomerBookings.TabIndex = 0
        Me.lblCustomerBookings.Text = "Pick a booking to add food to it"
        '
        'dgvCustomerBookings
        '
        Me.dgvCustomerBookings.AllowUserToAddRows = False
        Me.dgvCustomerBookings.AllowUserToDeleteRows = False
        Me.dgvCustomerBookings.AllowUserToResizeRows = False
        Me.dgvCustomerBookings.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dgvCustomerBookings.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvCustomerBookings.Location = New System.Drawing.Point(14, 40)
        Me.dgvCustomerBookings.Name = "dgvCustomerBookings"
        Me.dgvCustomerBookings.ReadOnly = True
        Me.dgvCustomerBookings.RowHeadersVisible = False
        Me.dgvCustomerBookings.RowTemplate.Height = 26
        Me.dgvCustomerBookings.Size = New System.Drawing.Size(462, 60)
        Me.dgvCustomerBookings.TabIndex = 1
        '
        'lblVersion
        '
        Me.lblVersion.AutoSize = True
        Me.lblVersion.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.lblVersion.Location = New System.Drawing.Point(16, 678)
        Me.lblVersion.Name = "lblVersion"
        Me.lblVersion.Size = New System.Drawing.Size(215, 13)
        Me.lblVersion.TabIndex = 11
        Me.lblVersion.Text = "Filmtopia Cinema Management System  v1.0"
        '
        'frmBookings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1180, 700)
        Me.Controls.Add(Me.lblVersion)
        Me.Controls.Add(Me.GroupBox4)
        Me.Controls.Add(Me.lblTickets)
        Me.Controls.Add(Me.lblFoodTotal)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.btnOrderFood)
        Me.Controls.Add(Me.btnClear)
        Me.Controls.Add(Me.btnCreateBooking)
        Me.Controls.Add(Me.lblTotal)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.pnlSeatMap)
        Me.Controls.Add(Me.lblScreen)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.lblHeading)
        Me.Name = "frmBookings"
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Text = "Bookings"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        CType(Me.dgvPendingFood, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        CType(Me.dgvCustomerBookings, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblHeading As Label
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents lblScreening As Label
    Friend WithEvents cboScreening As ComboBox
    Friend WithEvents lblCustomer As Label
    Friend WithEvents cboCustomer As ComboBox
    Friend WithEvents chkWalkIn As CheckBox
    Friend WithEvents lblScreen As Label
    Friend WithEvents pnlSeatMap As Panel
    Friend WithEvents GroupBox2 As GroupBox
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
    Friend WithEvents GroupBox4 As GroupBox
    Friend WithEvents lblFoodItem As Label
    Friend WithEvents cboFoodItem As ComboBox
    Friend WithEvents lblQty As Label
    Friend WithEvents txtQuantity As TextBox
    Friend WithEvents btnAddFood As Button
    Friend WithEvents btnRemoveFood As Button
    Friend WithEvents dgvPendingFood As DataGridView
    Friend WithEvents lblTickets As Label
    Friend WithEvents lblFoodTotal As Label
    Friend WithEvents GroupBox3 As GroupBox
    Friend WithEvents lblCustomerBookings As Label
    Friend WithEvents dgvCustomerBookings As DataGridView
    Friend WithEvents lblVersion As Label
End Class
