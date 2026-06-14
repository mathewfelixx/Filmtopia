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
        Me.lblRegisterTitle = New System.Windows.Forms.Label()
        Me.lblRegisterScreening = New System.Windows.Forms.Label()
        Me.cboRegisterScreening = New System.Windows.Forms.ComboBox()
        Me.btnLoadRegister = New System.Windows.Forms.Button()
        Me.btnExportRegister = New System.Windows.Forms.Button()
        Me.dgvRegister = New System.Windows.Forms.DataGridView()
        Me.lblVersion = New System.Windows.Forms.Label()
        CType(Me.dgvBookings, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvRegister, System.ComponentModel.ISupportInitialize).BeginInit()
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
        'lblRegisterTitle
        '
        Me.lblRegisterTitle.AutoSize = True
        Me.lblRegisterTitle.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lblRegisterTitle.Location = New System.Drawing.Point(16, 444)
        Me.lblRegisterTitle.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblRegisterTitle.Name = "lblRegisterTitle"
        Me.lblRegisterTitle.Size = New System.Drawing.Size(155, 23)
        Me.lblRegisterTitle.TabIndex = 7
        Me.lblRegisterTitle.Text = "Screening Register"
        '
        'lblRegisterScreening
        '
        Me.lblRegisterScreening.AutoSize = True
        Me.lblRegisterScreening.Location = New System.Drawing.Point(16, 480)
        Me.lblRegisterScreening.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblRegisterScreening.Name = "lblRegisterScreening"
        Me.lblRegisterScreening.Size = New System.Drawing.Size(68, 16)
        Me.lblRegisterScreening.TabIndex = 8
        Me.lblRegisterScreening.Text = "Screening"
        '
        'cboRegisterScreening
        '
        Me.cboRegisterScreening.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboRegisterScreening.FormattingEnabled = True
        Me.cboRegisterScreening.Location = New System.Drawing.Point(110, 476)
        Me.cboRegisterScreening.Margin = New System.Windows.Forms.Padding(4)
        Me.cboRegisterScreening.Name = "cboRegisterScreening"
        Me.cboRegisterScreening.Size = New System.Drawing.Size(500, 24)
        Me.cboRegisterScreening.TabIndex = 9
        '
        'btnLoadRegister
        '
        Me.btnLoadRegister.Location = New System.Drawing.Point(630, 474)
        Me.btnLoadRegister.Margin = New System.Windows.Forms.Padding(4)
        Me.btnLoadRegister.Name = "btnLoadRegister"
        Me.btnLoadRegister.Size = New System.Drawing.Size(140, 30)
        Me.btnLoadRegister.TabIndex = 10
        Me.btnLoadRegister.Text = "Load Register"
        Me.btnLoadRegister.UseVisualStyleBackColor = True
        '
        'btnExportRegister
        '
        Me.btnExportRegister.Location = New System.Drawing.Point(780, 474)
        Me.btnExportRegister.Margin = New System.Windows.Forms.Padding(4)
        Me.btnExportRegister.Name = "btnExportRegister"
        Me.btnExportRegister.Size = New System.Drawing.Size(140, 30)
        Me.btnExportRegister.TabIndex = 11
        Me.btnExportRegister.Text = "Export CSV"
        Me.btnExportRegister.UseVisualStyleBackColor = True
        '
        'dgvRegister
        '
        Me.dgvRegister.AllowUserToAddRows = False
        Me.dgvRegister.AllowUserToDeleteRows = False
        Me.dgvRegister.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvRegister.Location = New System.Drawing.Point(16, 514)
        Me.dgvRegister.Margin = New System.Windows.Forms.Padding(4)
        Me.dgvRegister.Name = "dgvRegister"
        Me.dgvRegister.ReadOnly = True
        Me.dgvRegister.RowHeadersWidth = 51
        Me.dgvRegister.Size = New System.Drawing.Size(1106, 180)
        Me.dgvRegister.TabIndex = 12
        '
        'lblVersion
        '
        Me.lblVersion.AutoSize = True
        Me.lblVersion.ForeColor = System.Drawing.Color.Gray
        Me.lblVersion.Location = New System.Drawing.Point(16, 706)
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
        Me.ClientSize = New System.Drawing.Size(1135, 730)
        Me.Controls.Add(Me.lblVersion)
        Me.Controls.Add(Me.dgvRegister)
        Me.Controls.Add(Me.btnExportRegister)
        Me.Controls.Add(Me.btnLoadRegister)
        Me.Controls.Add(Me.cboRegisterScreening)
        Me.Controls.Add(Me.lblRegisterScreening)
        Me.Controls.Add(Me.lblRegisterTitle)
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
        CType(Me.dgvRegister, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblSearch As Label
    Friend WithEvents txtSearch As TextBox
    Friend WithEvents btnSearch As Button
    Friend WithEvents btnShowAll As Button
    Friend WithEvents dgvBookings As DataGridView
    Friend WithEvents btnCancelBooking As Button
    Friend WithEvents lblRegisterTitle As Label
    Friend WithEvents lblRegisterScreening As Label
    Friend WithEvents cboRegisterScreening As ComboBox
    Friend WithEvents btnLoadRegister As Button
    Friend WithEvents btnExportRegister As Button
    Friend WithEvents dgvRegister As DataGridView
    Friend WithEvents lblVersion As Label
End Class
