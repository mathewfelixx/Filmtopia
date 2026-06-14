<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmBookingSearch
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
        Me.lblSearch = New System.Windows.Forms.Label()
        Me.txtSearch = New System.Windows.Forms.TextBox()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.btnShowAll = New System.Windows.Forms.Button()
        Me.dgvBookings = New System.Windows.Forms.DataGridView()
        Me.btnCancelBooking = New System.Windows.Forms.Button()
        Me.lblVersion = New System.Windows.Forms.Label()
        CType(Me.dgvBookings, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblSearch
        '
        Me.lblSearch.AutoSize = True
        Me.lblSearch.Location = New System.Drawing.Point(33, 19)
        Me.lblSearch.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblSearch.Name = "lblSearch"
        Me.lblSearch.Size = New System.Drawing.Size(188, 16)
        Me.lblSearch.TabIndex = 0
        Me.lblSearch.Text = "Booking ID or Customer Name"
        '
        'txtSearch
        '
        Me.txtSearch.Location = New System.Drawing.Point(229, 17)
        Me.txtSearch.Margin = New System.Windows.Forms.Padding(4)
        Me.txtSearch.Name = "txtSearch"
        Me.txtSearch.Size = New System.Drawing.Size(316, 22)
        Me.txtSearch.TabIndex = 1
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(553, 13)
        Me.btnSearch.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(263, 30)
        Me.btnSearch.TabIndex = 2
        Me.btnSearch.Text = "Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'btnShowAll
        '
        Me.btnShowAll.Location = New System.Drawing.Point(824, 13)
        Me.btnShowAll.Margin = New System.Windows.Forms.Padding(4)
        Me.btnShowAll.Name = "btnShowAll"
        Me.btnShowAll.Size = New System.Drawing.Size(298, 30)
        Me.btnShowAll.TabIndex = 3
        Me.btnShowAll.Text = "Show All"
        Me.btnShowAll.UseVisualStyleBackColor = True
        '
        'dgvBookings
        '
        Me.dgvBookings.AllowUserToAddRows = False
        Me.dgvBookings.AllowUserToDeleteRows = False
        Me.dgvBookings.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvBookings.Location = New System.Drawing.Point(16, 56)
        Me.dgvBookings.Margin = New System.Windows.Forms.Padding(4)
        Me.dgvBookings.Name = "dgvBookings"
        Me.dgvBookings.ReadOnly = True
        Me.dgvBookings.RowHeadersWidth = 51
        Me.dgvBookings.Size = New System.Drawing.Size(1106, 320)
        Me.dgvBookings.TabIndex = 4
        '
        'btnCancelBooking
        '
        Me.btnCancelBooking.Location = New System.Drawing.Point(16, 390)
        Me.btnCancelBooking.Margin = New System.Windows.Forms.Padding(4)
        Me.btnCancelBooking.Name = "btnCancelBooking"
        Me.btnCancelBooking.Size = New System.Drawing.Size(357, 40)
        Me.btnCancelBooking.TabIndex = 5
        Me.btnCancelBooking.Text = "Cancel Booking"
        Me.btnCancelBooking.UseVisualStyleBackColor = True
        '
        'lblVersion
        '
        Me.lblVersion.AutoSize = True
        Me.lblVersion.ForeColor = System.Drawing.Color.Gray
        Me.lblVersion.Location = New System.Drawing.Point(836, 402)
        Me.lblVersion.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblVersion.Name = "lblVersion"
        Me.lblVersion.Size = New System.Drawing.Size(271, 16)
        Me.lblVersion.TabIndex = 6
        Me.lblVersion.Text = "Filmtopia Cinema Management System  v1.0"
        '
        'frmBookingSearch
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1135, 440)
        Me.Controls.Add(Me.lblVersion)
        Me.Controls.Add(Me.btnCancelBooking)
        Me.Controls.Add(Me.dgvBookings)
        Me.Controls.Add(Me.btnShowAll)
        Me.Controls.Add(Me.btnSearch)
        Me.Controls.Add(Me.txtSearch)
        Me.Controls.Add(Me.lblSearch)
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "frmBookingSearch"
        Me.Text = "Booking Search"
        CType(Me.dgvBookings, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblSearch As Label
    Friend WithEvents txtSearch As TextBox
    Friend WithEvents btnSearch As Button
    Friend WithEvents btnShowAll As Button
    Friend WithEvents dgvBookings As DataGridView
    Friend WithEvents btnCancelBooking As Button
    Friend WithEvents lblVersion As Label
End Class
