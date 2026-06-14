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
        Me.lblVersion = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'lblScreening
        '
        Me.lblScreening.AutoSize = True
        Me.lblScreening.Location = New System.Drawing.Point(16, 18)
        Me.lblScreening.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblScreening.Name = "lblScreening"
        Me.lblScreening.Size = New System.Drawing.Size(68, 16)
        Me.lblScreening.TabIndex = 0
        Me.lblScreening.Text = "Screening"
        '
        'cboScreening
        '
        Me.cboScreening.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboScreening.FormattingEnabled = True
        Me.cboScreening.Location = New System.Drawing.Point(110, 14)
        Me.cboScreening.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.cboScreening.Name = "cboScreening"
        Me.cboScreening.Size = New System.Drawing.Size(430, 24)
        Me.cboScreening.TabIndex = 1
        '
        'lblCustomer
        '
        Me.lblCustomer.AutoSize = True
        Me.lblCustomer.Location = New System.Drawing.Point(16, 54)
        Me.lblCustomer.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblCustomer.Name = "lblCustomer"
        Me.lblCustomer.Size = New System.Drawing.Size(67, 16)
        Me.lblCustomer.TabIndex = 2
        Me.lblCustomer.Text = "Customer"
        '
        'cboCustomer
        '
        Me.cboCustomer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboCustomer.FormattingEnabled = True
        Me.cboCustomer.Location = New System.Drawing.Point(110, 50)
        Me.cboCustomer.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.cboCustomer.Name = "cboCustomer"
        Me.cboCustomer.Size = New System.Drawing.Size(300, 24)
        Me.cboCustomer.TabIndex = 3
        '
        'lblScreen
        '
        Me.lblScreen.Location = New System.Drawing.Point(16, 92)
        Me.lblScreen.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblScreen.Name = "lblScreen"
        Me.lblScreen.Size = New System.Drawing.Size(710, 20)
        Me.lblScreen.TabIndex = 4
        Me.lblScreen.Text = "----------------------------------  SCREEN  ----------------------------------"
        Me.lblScreen.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'pnlSeatMap
        '
        Me.pnlSeatMap.AutoScroll = True
        Me.pnlSeatMap.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlSeatMap.Location = New System.Drawing.Point(16, 120)
        Me.pnlSeatMap.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.pnlSeatMap.Name = "pnlSeatMap"
        Me.pnlSeatMap.Size = New System.Drawing.Size(710, 430)
        Me.pnlSeatMap.TabIndex = 5
        '
        'lblSwatchAvailable
        '
        Me.lblSwatchAvailable.BackColor = System.Drawing.Color.FromArgb(CType(CType(220, Byte), Integer), CType(CType(220, Byte), Integer), CType(CType(220, Byte), Integer))
        Me.lblSwatchAvailable.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblSwatchAvailable.Location = New System.Drawing.Point(745, 124)
        Me.lblSwatchAvailable.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblSwatchAvailable.Name = "lblSwatchAvailable"
        Me.lblSwatchAvailable.Size = New System.Drawing.Size(18, 18)
        Me.lblSwatchAvailable.TabIndex = 6
        '
        'lblLegendAvailable
        '
        Me.lblLegendAvailable.AutoSize = True
        Me.lblLegendAvailable.Location = New System.Drawing.Point(772, 125)
        Me.lblLegendAvailable.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblLegendAvailable.Name = "lblLegendAvailable"
        Me.lblLegendAvailable.Size = New System.Drawing.Size(66, 16)
        Me.lblLegendAvailable.TabIndex = 7
        Me.lblLegendAvailable.Text = "Available"
        '
        'lblSwatchSelected
        '
        Me.lblSwatchSelected.BackColor = System.Drawing.Color.FromArgb(CType(CType(173, Byte), Integer), CType(CType(20, Byte), Integer), CType(CType(87, Byte), Integer))
        Me.lblSwatchSelected.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblSwatchSelected.Location = New System.Drawing.Point(745, 149)
        Me.lblSwatchSelected.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblSwatchSelected.Name = "lblSwatchSelected"
        Me.lblSwatchSelected.Size = New System.Drawing.Size(18, 18)
        Me.lblSwatchSelected.TabIndex = 8
        '
        'lblLegendSelected
        '
        Me.lblLegendSelected.AutoSize = True
        Me.lblLegendSelected.Location = New System.Drawing.Point(772, 150)
        Me.lblLegendSelected.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblLegendSelected.Name = "lblLegendSelected"
        Me.lblLegendSelected.Size = New System.Drawing.Size(63, 16)
        Me.lblLegendSelected.TabIndex = 9
        Me.lblLegendSelected.Text = "Selected"
        '
        'lblSwatchTaken
        '
        Me.lblSwatchTaken.BackColor = System.Drawing.Color.FromArgb(CType(CType(150, Byte), Integer), CType(CType(40, Byte), Integer), CType(CType(40, Byte), Integer))
        Me.lblSwatchTaken.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblSwatchTaken.Location = New System.Drawing.Point(745, 174)
        Me.lblSwatchTaken.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblSwatchTaken.Name = "lblSwatchTaken"
        Me.lblSwatchTaken.Size = New System.Drawing.Size(18, 18)
        Me.lblSwatchTaken.TabIndex = 10
        '
        'lblLegendTaken
        '
        Me.lblLegendTaken.AutoSize = True
        Me.lblLegendTaken.Location = New System.Drawing.Point(772, 175)
        Me.lblLegendTaken.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblLegendTaken.Name = "lblLegendTaken"
        Me.lblLegendTaken.Size = New System.Drawing.Size(48, 16)
        Me.lblLegendTaken.TabIndex = 11
        Me.lblLegendTaken.Text = "Taken"
        '
        'lblTotal
        '
        Me.lblTotal.AutoSize = True
        Me.lblTotal.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lblTotal.Location = New System.Drawing.Point(742, 215)
        Me.lblTotal.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblTotal.Name = "lblTotal"
        Me.lblTotal.Size = New System.Drawing.Size(176, 19)
        Me.lblTotal.TabIndex = 12
        Me.lblTotal.Text = "0 seats selected"
        '
        'btnCreateBooking
        '
        Me.btnCreateBooking.Location = New System.Drawing.Point(745, 260)
        Me.btnCreateBooking.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnCreateBooking.Name = "btnCreateBooking"
        Me.btnCreateBooking.Size = New System.Drawing.Size(170, 40)
        Me.btnCreateBooking.TabIndex = 13
        Me.btnCreateBooking.Text = "Create Booking"
        Me.btnCreateBooking.UseVisualStyleBackColor = True
        '
        'btnClear
        '
        Me.btnClear.Location = New System.Drawing.Point(745, 310)
        Me.btnClear.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnClear.Name = "btnClear"
        Me.btnClear.Size = New System.Drawing.Size(170, 37)
        Me.btnClear.TabIndex = 14
        Me.btnClear.Text = "Clear Selection"
        Me.btnClear.UseVisualStyleBackColor = True
        '
        'lblVersion
        '
        Me.lblVersion.AutoSize = True
        Me.lblVersion.ForeColor = System.Drawing.Color.Gray
        Me.lblVersion.Location = New System.Drawing.Point(660, 590)
        Me.lblVersion.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblVersion.Name = "lblVersion"
        Me.lblVersion.Size = New System.Drawing.Size(271, 16)
        Me.lblVersion.TabIndex = 15
        Me.lblVersion.Text = "Filmtopia Cinema Management System  v1.0"
        '
        'frmBookings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(945, 625)
        Me.Controls.Add(Me.lblVersion)
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
        Me.Controls.Add(Me.cboCustomer)
        Me.Controls.Add(Me.lblCustomer)
        Me.Controls.Add(Me.cboScreening)
        Me.Controls.Add(Me.lblScreening)
        Me.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.Name = "frmBookings"
        Me.Text = "Bookings"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblScreening As Label
    Friend WithEvents cboScreening As ComboBox
    Friend WithEvents lblCustomer As Label
    Friend WithEvents cboCustomer As ComboBox
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
    Friend WithEvents lblVersion As Label
End Class
