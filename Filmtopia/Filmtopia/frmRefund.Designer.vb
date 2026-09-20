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
        Me.lblSeatsHeader = New System.Windows.Forms.Label()
        Me.dgvRefundSeats = New System.Windows.Forms.DataGridView()
        Me.lblFoodHeader = New System.Windows.Forms.Label()
        Me.dgvRefundFood = New System.Windows.Forms.DataGridView()
        Me.lblReason = New System.Windows.Forms.Label()
        Me.txtReason = New System.Windows.Forms.TextBox()
        Me.btnConfirmRefund = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.lblVersion = New System.Windows.Forms.Label()
        CType(Me.dgvRefundSeats, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvRefundFood, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblBookingInfo
        '
        Me.lblBookingInfo.AutoSize = True
        Me.lblBookingInfo.Location = New System.Drawing.Point(12, 12)
        Me.lblBookingInfo.Name = "lblBookingInfo"
        Me.lblBookingInfo.Size = New System.Drawing.Size(70, 13)
        Me.lblBookingInfo.TabIndex = 0
        Me.lblBookingInfo.Text = "Booking info"
        '
        'lblSeatsHeader
        '
        Me.lblSeatsHeader.AutoSize = True
        Me.lblSeatsHeader.Location = New System.Drawing.Point(12, 44)
        Me.lblSeatsHeader.Name = "lblSeatsHeader"
        Me.lblSeatsHeader.Size = New System.Drawing.Size(83, 13)
        Me.lblSeatsHeader.TabIndex = 1
        Me.lblSeatsHeader.Text = "Seats to refund"
        '
        'dgvRefundSeats
        '
        Me.dgvRefundSeats.AllowUserToAddRows = False
        Me.dgvRefundSeats.AllowUserToDeleteRows = False
        Me.dgvRefundSeats.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvRefundSeats.Location = New System.Drawing.Point(12, 62)
        Me.dgvRefundSeats.Name = "dgvRefundSeats"
        Me.dgvRefundSeats.RowHeadersVisible = False
        Me.dgvRefundSeats.Size = New System.Drawing.Size(290, 360)
        Me.dgvRefundSeats.TabIndex = 2
        '
        'lblFoodHeader
        '
        Me.lblFoodHeader.AutoSize = True
        Me.lblFoodHeader.Location = New System.Drawing.Point(315, 44)
        Me.lblFoodHeader.Name = "lblFoodHeader"
        Me.lblFoodHeader.Size = New System.Drawing.Size(79, 13)
        Me.lblFoodHeader.TabIndex = 3
        Me.lblFoodHeader.Text = "Food to refund"
        '
        'dgvRefundFood
        '
        Me.dgvRefundFood.AllowUserToAddRows = False
        Me.dgvRefundFood.AllowUserToDeleteRows = False
        Me.dgvRefundFood.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvRefundFood.Location = New System.Drawing.Point(315, 62)
        Me.dgvRefundFood.Name = "dgvRefundFood"
        Me.dgvRefundFood.RowHeadersVisible = False
        Me.dgvRefundFood.Size = New System.Drawing.Size(320, 360)
        Me.dgvRefundFood.TabIndex = 4
        '
        'lblReason
        '
        Me.lblReason.AutoSize = True
        Me.lblReason.Location = New System.Drawing.Point(12, 438)
        Me.lblReason.Name = "lblReason"
        Me.lblReason.Size = New System.Drawing.Size(43, 13)
        Me.lblReason.TabIndex = 5
        Me.lblReason.Text = "Reason"
        '
        'txtReason
        '
        Me.txtReason.Location = New System.Drawing.Point(70, 435)
        Me.txtReason.Name = "txtReason"
        Me.txtReason.Size = New System.Drawing.Size(400, 20)
        Me.txtReason.TabIndex = 6
        '
        'btnConfirmRefund
        '
        Me.btnConfirmRefund.Location = New System.Drawing.Point(12, 470)
        Me.btnConfirmRefund.Name = "btnConfirmRefund"
        Me.btnConfirmRefund.Size = New System.Drawing.Size(160, 32)
        Me.btnConfirmRefund.TabIndex = 7
        Me.btnConfirmRefund.Text = "Confirm refund"
        Me.btnConfirmRefund.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(185, 470)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(100, 32)
        Me.btnCancel.TabIndex = 8
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'lblVersion
        '
        Me.lblVersion.AutoSize = True
        Me.lblVersion.ForeColor = System.Drawing.Color.Gray
        Me.lblVersion.Location = New System.Drawing.Point(210, 525)
        Me.lblVersion.Name = "lblVersion"
        Me.lblVersion.Size = New System.Drawing.Size(215, 13)
        Me.lblVersion.TabIndex = 9
        Me.lblVersion.Text = "Filmtopia Cinema Management System  v1.0"
        '
        'frmRefund
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(650, 560)
        Me.Controls.Add(Me.lblVersion)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnConfirmRefund)
        Me.Controls.Add(Me.txtReason)
        Me.Controls.Add(Me.lblReason)
        Me.Controls.Add(Me.dgvRefundFood)
        Me.Controls.Add(Me.lblFoodHeader)
        Me.Controls.Add(Me.dgvRefundSeats)
        Me.Controls.Add(Me.lblSeatsHeader)
        Me.Controls.Add(Me.lblBookingInfo)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "frmRefund"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Refund"
        CType(Me.dgvRefundSeats, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvRefundFood, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblBookingInfo As Label
    Friend WithEvents lblSeatsHeader As Label
    Friend WithEvents dgvRefundSeats As DataGridView
    Friend WithEvents lblFoodHeader As Label
    Friend WithEvents dgvRefundFood As DataGridView
    Friend WithEvents lblReason As Label
    Friend WithEvents txtReason As TextBox
    Friend WithEvents btnConfirmRefund As Button
    Friend WithEvents btnCancel As Button
    Friend WithEvents lblVersion As Label
End Class
