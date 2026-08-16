<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmRefund
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
        Me.lblSoldBy = New System.Windows.Forms.Label()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.dgvSeats = New System.Windows.Forms.DataGridView()
        Me.lblSeatCount = New System.Windows.Forms.Label()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.dgvFood = New System.Windows.Forms.DataGridView()
        Me.lblFoodCount = New System.Windows.Forms.Label()
        Me.lblReason = New System.Windows.Forms.Label()
        Me.txtReason = New System.Windows.Forms.TextBox()
        Me.lblRefundTotal = New System.Windows.Forms.Label()
        Me.lblAlreadyRefunded = New System.Windows.Forms.Label()
        Me.lblNotAllowed = New System.Windows.Forms.Label()
        Me.btnRefundSelected = New System.Windows.Forms.Button()
        Me.btnRefundEverything = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.lblVersion = New System.Windows.Forms.Label()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        CType(Me.dgvSeats, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvFood, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblBookingInfo
        '
        Me.lblBookingInfo.AutoSize = True
        Me.lblBookingInfo.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold)
        Me.lblBookingInfo.Location = New System.Drawing.Point(16, 14)
        Me.lblBookingInfo.Name = "lblBookingInfo"
        Me.lblBookingInfo.Size = New System.Drawing.Size(160, 28)
        Me.lblBookingInfo.TabIndex = 0
        Me.lblBookingInfo.Text = "Booking details"
        '
        'lblSoldBy
        '
        Me.lblSoldBy.AutoSize = True
        Me.lblSoldBy.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.lblSoldBy.Location = New System.Drawing.Point(18, 48)
        Me.lblSoldBy.Name = "lblSoldBy"
        Me.lblSoldBy.Size = New System.Drawing.Size(120, 17)
        Me.lblSoldBy.TabIndex = 1
        Me.lblSoldBy.Text = "Sold by"
        '
        'lblStatus
        '
        Me.lblStatus.AutoSize = True
        Me.lblStatus.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me.lblStatus.Location = New System.Drawing.Point(18, 78)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(60, 17)
        Me.lblStatus.TabIndex = 2
        Me.lblStatus.Text = "Status"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.dgvSeats)
        Me.GroupBox1.Controls.Add(Me.lblSeatCount)
        Me.GroupBox1.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.GroupBox1.Location = New System.Drawing.Point(16, 112)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(880, 290)
        Me.GroupBox1.TabIndex = 3
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Seats sold on this booking"
        '
        'dgvSeats
        '
        Me.dgvSeats.AllowUserToAddRows = False
        Me.dgvSeats.AllowUserToDeleteRows = False
        Me.dgvSeats.AllowUserToResizeRows = False
        Me.dgvSeats.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dgvSeats.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvSeats.Location = New System.Drawing.Point(16, 28)
        Me.dgvSeats.Name = "dgvSeats"
        Me.dgvSeats.RowHeadersVisible = False
        Me.dgvSeats.RowTemplate.Height = 28
        Me.dgvSeats.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvSeats.Size = New System.Drawing.Size(848, 230)
        Me.dgvSeats.TabIndex = 0
        '
        'lblSeatCount
        '
        Me.lblSeatCount.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lblSeatCount.Location = New System.Drawing.Point(16, 264)
        Me.lblSeatCount.Name = "lblSeatCount"
        Me.lblSeatCount.Size = New System.Drawing.Size(500, 18)
        Me.lblSeatCount.TabIndex = 1
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.dgvFood)
        Me.GroupBox2.Controls.Add(Me.lblFoodCount)
        Me.GroupBox2.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.GroupBox2.Location = New System.Drawing.Point(16, 412)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(880, 304)
        Me.GroupBox2.TabIndex = 4
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Concessions on this booking"
        '
        'dgvFood
        '
        Me.dgvFood.AllowUserToAddRows = False
        Me.dgvFood.AllowUserToDeleteRows = False
        Me.dgvFood.AllowUserToResizeRows = False
        Me.dgvFood.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dgvFood.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvFood.Location = New System.Drawing.Point(16, 28)
        Me.dgvFood.Name = "dgvFood"
        Me.dgvFood.RowHeadersVisible = False
        Me.dgvFood.RowTemplate.Height = 28
        Me.dgvFood.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvFood.Size = New System.Drawing.Size(848, 244)
        Me.dgvFood.TabIndex = 0
        '
        'lblFoodCount
        '
        Me.lblFoodCount.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lblFoodCount.Location = New System.Drawing.Point(16, 278)
        Me.lblFoodCount.Name = "lblFoodCount"
        Me.lblFoodCount.Size = New System.Drawing.Size(500, 18)
        Me.lblFoodCount.TabIndex = 1
        '
        'lblReason
        '
        Me.lblReason.AutoSize = True
        Me.lblReason.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.lblReason.Location = New System.Drawing.Point(16, 732)
        Me.lblReason.Name = "lblReason"
        Me.lblReason.Size = New System.Drawing.Size(120, 17)
        Me.lblReason.TabIndex = 5
        Me.lblReason.Text = "Reason for refund"
        '
        'txtReason
        '
        Me.txtReason.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.txtReason.Location = New System.Drawing.Point(180, 728)
        Me.txtReason.MaxLength = 100
        Me.txtReason.Name = "txtReason"
        Me.txtReason.Size = New System.Drawing.Size(400, 25)
        Me.txtReason.TabIndex = 6
        '
        'lblRefundTotal
        '
        Me.lblRefundTotal.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold)
        Me.lblRefundTotal.Location = New System.Drawing.Point(600, 726)
        Me.lblRefundTotal.Name = "lblRefundTotal"
        Me.lblRefundTotal.Size = New System.Drawing.Size(296, 28)
        Me.lblRefundTotal.TabIndex = 7
        Me.lblRefundTotal.Text = "Refund total: 0.00"
        Me.lblRefundTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblAlreadyRefunded
        '
        Me.lblAlreadyRefunded.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lblAlreadyRefunded.Location = New System.Drawing.Point(460, 766)
        Me.lblAlreadyRefunded.Name = "lblAlreadyRefunded"
        Me.lblAlreadyRefunded.Size = New System.Drawing.Size(436, 18)
        Me.lblAlreadyRefunded.TabIndex = 8
        Me.lblAlreadyRefunded.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblNotAllowed
        '
        Me.lblNotAllowed.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lblNotAllowed.Location = New System.Drawing.Point(16, 768)
        Me.lblNotAllowed.Name = "lblNotAllowed"
        Me.lblNotAllowed.Size = New System.Drawing.Size(430, 18)
        Me.lblNotAllowed.TabIndex = 9
        '
        'btnRefundSelected
        '
        Me.btnRefundSelected.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me.btnRefundSelected.Location = New System.Drawing.Point(536, 798)
        Me.btnRefundSelected.Name = "btnRefundSelected"
        Me.btnRefundSelected.Size = New System.Drawing.Size(170, 34)
        Me.btnRefundSelected.TabIndex = 10
        Me.btnRefundSelected.Text = "Refund selected"
        Me.btnRefundSelected.UseVisualStyleBackColor = True
        '
        'btnRefundEverything
        '
        Me.btnRefundEverything.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnRefundEverything.Location = New System.Drawing.Point(716, 798)
        Me.btnRefundEverything.Name = "btnRefundEverything"
        Me.btnRefundEverything.Size = New System.Drawing.Size(180, 34)
        Me.btnRefundEverything.TabIndex = 11
        Me.btnRefundEverything.Text = "Refund everything"
        Me.btnRefundEverything.UseVisualStyleBackColor = True
        '
        'btnClose
        '
        Me.btnClose.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnClose.Location = New System.Drawing.Point(416, 798)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(110, 34)
        Me.btnClose.TabIndex = 12
        Me.btnClose.Text = "Close"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'lblVersion
        '
        Me.lblVersion.AutoSize = True
        Me.lblVersion.ForeColor = System.Drawing.Color.Gray
        Me.lblVersion.Location = New System.Drawing.Point(16, 814)
        Me.lblVersion.Name = "lblVersion"
        Me.lblVersion.Size = New System.Drawing.Size(271, 16)
        Me.lblVersion.TabIndex = 13
        Me.lblVersion.Text = "Filmtopia Cinema Management System  v1.0"
        '
        'frmRefund
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(912, 864)
        Me.Controls.Add(Me.lblVersion)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnRefundEverything)
        Me.Controls.Add(Me.btnRefundSelected)
        Me.Controls.Add(Me.lblNotAllowed)
        Me.Controls.Add(Me.lblAlreadyRefunded)
        Me.Controls.Add(Me.lblRefundTotal)
        Me.Controls.Add(Me.txtReason)
        Me.Controls.Add(Me.lblReason)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.lblStatus)
        Me.Controls.Add(Me.lblSoldBy)
        Me.Controls.Add(Me.lblBookingInfo)
        Me.CancelButton = Me.btnClose
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "frmRefund"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Booking detail and refund"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        CType(Me.dgvSeats, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvFood, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblBookingInfo As Label
    Friend WithEvents lblSoldBy As Label
    Friend WithEvents lblStatus As Label
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents dgvSeats As DataGridView
    Friend WithEvents lblSeatCount As Label
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents dgvFood As DataGridView
    Friend WithEvents lblFoodCount As Label
    Friend WithEvents lblReason As Label
    Friend WithEvents txtReason As TextBox
    Friend WithEvents lblRefundTotal As Label
    Friend WithEvents lblAlreadyRefunded As Label
    Friend WithEvents lblNotAllowed As Label
    Friend WithEvents btnRefundSelected As Button
    Friend WithEvents btnRefundEverything As Button
    Friend WithEvents btnClose As Button
    Friend WithEvents lblVersion As Label
End Class
