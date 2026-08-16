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
        Me.lblHeading = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.lblSearch = New System.Windows.Forms.Label()
        Me.txtSearch = New System.Windows.Forms.TextBox()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.btnShowAll = New System.Windows.Forms.Button()
        Me.lblResultCount = New System.Windows.Forms.Label()
        Me.dgvBookings = New System.Windows.Forms.DataGridView()
        Me.lblSelectedBooking = New System.Windows.Forms.Label()
        Me.btnCancelBooking = New System.Windows.Forms.Button()
        Me.btnViewBooking = New System.Windows.Forms.Button()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.lblDoorListScreening = New System.Windows.Forms.Label()
        Me.cboDoorListScreening = New System.Windows.Forms.ComboBox()
        Me.btnLoadDoorList = New System.Windows.Forms.Button()
        Me.btnExportDoorList = New System.Windows.Forms.Button()
        Me.dgvDoorList = New System.Windows.Forms.DataGridView()
        Me.lblVersion = New System.Windows.Forms.Label()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        CType(Me.dgvBookings, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvDoorList, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblHeading
        '
        Me.lblHeading.AutoSize = True
        Me.lblHeading.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Bold)
        Me.lblHeading.Location = New System.Drawing.Point(16, 12)
        Me.lblHeading.Name = "lblHeading"
        Me.lblHeading.Size = New System.Drawing.Size(300, 25)
        Me.lblHeading.TabIndex = 0
        Me.lblHeading.Text = "Find and manage bookings"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.lblSearch)
        Me.GroupBox1.Controls.Add(Me.txtSearch)
        Me.GroupBox1.Controls.Add(Me.btnSearch)
        Me.GroupBox1.Controls.Add(Me.btnShowAll)
        Me.GroupBox1.Controls.Add(Me.lblResultCount)
        Me.GroupBox1.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.GroupBox1.Location = New System.Drawing.Point(16, 48)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(1103, 76)
        Me.GroupBox1.TabIndex = 1
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Find a booking"
        '
        'lblSearch
        '
        Me.lblSearch.AutoSize = True
        Me.lblSearch.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.lblSearch.Location = New System.Drawing.Point(16, 33)
        Me.lblSearch.Name = "lblSearch"
        Me.lblSearch.Size = New System.Drawing.Size(200, 17)
        Me.lblSearch.TabIndex = 0
        Me.lblSearch.Text = "Booking ID or customer name"
        '
        'txtSearch
        '
        Me.txtSearch.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.txtSearch.Location = New System.Drawing.Point(240, 30)
        Me.txtSearch.Name = "txtSearch"
        Me.txtSearch.Size = New System.Drawing.Size(300, 25)
        Me.txtSearch.TabIndex = 1
        '
        'btnSearch
        '
        Me.btnSearch.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnSearch.Location = New System.Drawing.Point(545, 29)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(110, 30)
        Me.btnSearch.TabIndex = 2
        Me.btnSearch.Text = "Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'btnShowAll
        '
        Me.btnShowAll.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnShowAll.Location = New System.Drawing.Point(665, 29)
        Me.btnShowAll.Name = "btnShowAll"
        Me.btnShowAll.Size = New System.Drawing.Size(110, 30)
        Me.btnShowAll.TabIndex = 3
        Me.btnShowAll.Text = "Show all"
        Me.btnShowAll.UseVisualStyleBackColor = True
        '
        'lblResultCount
        '
        Me.lblResultCount.AutoSize = True
        Me.lblResultCount.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.lblResultCount.Location = New System.Drawing.Point(795, 35)
        Me.lblResultCount.Name = "lblResultCount"
        Me.lblResultCount.Size = New System.Drawing.Size(100, 17)
        Me.lblResultCount.TabIndex = 4
        Me.lblResultCount.Text = ""
        '
        'dgvBookings
        '
        Me.dgvBookings.AllowUserToAddRows = False
        Me.dgvBookings.AllowUserToDeleteRows = False
        Me.dgvBookings.AllowUserToResizeRows = False
        Me.dgvBookings.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dgvBookings.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvBookings.Location = New System.Drawing.Point(16, 136)
        Me.dgvBookings.Name = "dgvBookings"
        Me.dgvBookings.ReadOnly = True
        Me.dgvBookings.RowHeadersVisible = False
        Me.dgvBookings.RowTemplate.Height = 30
        Me.dgvBookings.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvBookings.Size = New System.Drawing.Size(1103, 300)
        Me.dgvBookings.TabIndex = 2
        '
        'lblSelectedBooking
        '
        Me.lblSelectedBooking.AutoSize = True
        Me.lblSelectedBooking.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.lblSelectedBooking.Location = New System.Drawing.Point(16, 450)
        Me.lblSelectedBooking.Name = "lblSelectedBooking"
        Me.lblSelectedBooking.Size = New System.Drawing.Size(300, 17)
        Me.lblSelectedBooking.TabIndex = 3
        Me.lblSelectedBooking.Text = "No booking selected"
        '
        'btnViewBooking
        '
        Me.btnViewBooking.Enabled = False
        Me.btnViewBooking.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnViewBooking.Location = New System.Drawing.Point(769, 442)
        Me.btnViewBooking.Name = "btnViewBooking"
        Me.btnViewBooking.Size = New System.Drawing.Size(170, 32)
        Me.btnViewBooking.TabIndex = 5
        Me.btnViewBooking.Text = "Open booking"
        Me.btnViewBooking.UseVisualStyleBackColor = True
        '
        'btnCancelBooking
        '
        Me.btnCancelBooking.Enabled = False
        Me.btnCancelBooking.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me.btnCancelBooking.Location = New System.Drawing.Point(949, 442)
        Me.btnCancelBooking.Name = "btnCancelBooking"
        Me.btnCancelBooking.Size = New System.Drawing.Size(170, 32)
        Me.btnCancelBooking.TabIndex = 4
        Me.btnCancelBooking.Text = "Cancel booking"
        Me.btnCancelBooking.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.lblDoorListScreening)
        Me.GroupBox2.Controls.Add(Me.cboDoorListScreening)
        Me.GroupBox2.Controls.Add(Me.btnLoadDoorList)
        Me.GroupBox2.Controls.Add(Me.btnExportDoorList)
        Me.GroupBox2.Controls.Add(Me.dgvDoorList)
        Me.GroupBox2.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.GroupBox2.Location = New System.Drawing.Point(16, 486)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(1103, 252)
        Me.GroupBox2.TabIndex = 5
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Door list - who is coming to a screening"
        '
        'lblDoorListScreening
        '
        Me.lblDoorListScreening.AutoSize = True
        Me.lblDoorListScreening.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.lblDoorListScreening.Location = New System.Drawing.Point(16, 33)
        Me.lblDoorListScreening.Name = "lblDoorListScreening"
        Me.lblDoorListScreening.Size = New System.Drawing.Size(70, 17)
        Me.lblDoorListScreening.TabIndex = 0
        Me.lblDoorListScreening.Text = "Screening"
        '
        'cboDoorListScreening
        '
        Me.cboDoorListScreening.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboDoorListScreening.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.cboDoorListScreening.Location = New System.Drawing.Point(110, 30)
        Me.cboDoorListScreening.Name = "cboDoorListScreening"
        Me.cboDoorListScreening.Size = New System.Drawing.Size(470, 25)
        Me.cboDoorListScreening.TabIndex = 1
        '
        'btnLoadDoorList
        '
        Me.btnLoadDoorList.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnLoadDoorList.Location = New System.Drawing.Point(596, 29)
        Me.btnLoadDoorList.Name = "btnLoadDoorList"
        Me.btnLoadDoorList.Size = New System.Drawing.Size(130, 30)
        Me.btnLoadDoorList.TabIndex = 2
        Me.btnLoadDoorList.Text = "Load door list"
        Me.btnLoadDoorList.UseVisualStyleBackColor = True
        '
        'btnExportDoorList
        '
        Me.btnExportDoorList.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnExportDoorList.Location = New System.Drawing.Point(736, 29)
        Me.btnExportDoorList.Name = "btnExportDoorList"
        Me.btnExportDoorList.Size = New System.Drawing.Size(130, 30)
        Me.btnExportDoorList.TabIndex = 3
        Me.btnExportDoorList.Text = "Export CSV"
        Me.btnExportDoorList.UseVisualStyleBackColor = True
        '
        'dgvDoorList
        '
        Me.dgvDoorList.AllowUserToAddRows = False
        Me.dgvDoorList.AllowUserToDeleteRows = False
        Me.dgvDoorList.AllowUserToResizeRows = False
        Me.dgvDoorList.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dgvDoorList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvDoorList.Location = New System.Drawing.Point(16, 70)
        Me.dgvDoorList.Name = "dgvDoorList"
        Me.dgvDoorList.ReadOnly = True
        Me.dgvDoorList.RowHeadersVisible = False
        Me.dgvDoorList.RowTemplate.Height = 28
        Me.dgvDoorList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvDoorList.Size = New System.Drawing.Size(1071, 168)
        Me.dgvDoorList.TabIndex = 4
        '
        'lblVersion
        '
        Me.lblVersion.AutoSize = True
        Me.lblVersion.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.lblVersion.Location = New System.Drawing.Point(16, 748)
        Me.lblVersion.Name = "lblVersion"
        Me.lblVersion.Size = New System.Drawing.Size(240, 13)
        Me.lblVersion.TabIndex = 6
        Me.lblVersion.Text = "Filmtopia Cinema Management System  v1.0"
        '
        'frmBookingSearch
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1135, 775)
        Me.Controls.Add(Me.lblVersion)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.btnViewBooking)
        Me.Controls.Add(Me.btnCancelBooking)
        Me.Controls.Add(Me.lblSelectedBooking)
        Me.Controls.Add(Me.dgvBookings)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.lblHeading)
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "frmBookingSearch"
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Text = "Booking Search"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        CType(Me.dgvBookings, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvDoorList, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblHeading As Label
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents lblSearch As Label
    Friend WithEvents txtSearch As TextBox
    Friend WithEvents btnSearch As Button
    Friend WithEvents btnShowAll As Button
    Friend WithEvents lblResultCount As Label
    Friend WithEvents dgvBookings As DataGridView
    Friend WithEvents lblSelectedBooking As Label
    Friend WithEvents btnCancelBooking As Button
    Friend WithEvents btnViewBooking As Button
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents lblDoorListScreening As Label
    Friend WithEvents cboDoorListScreening As ComboBox
    Friend WithEvents btnLoadDoorList As Button
    Friend WithEvents btnExportDoorList As Button
    Friend WithEvents dgvDoorList As DataGridView
    Friend WithEvents lblVersion As Label
End Class
