<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmSalesReport
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
        Me.lblFrom = New System.Windows.Forms.Label()
        Me.dtpFrom = New System.Windows.Forms.DateTimePicker()
        Me.lblTo = New System.Windows.Forms.Label()
        Me.dtpTo = New System.Windows.Forms.DateTimePicker()
        Me.btnRunReport = New System.Windows.Forms.Button()
        Me.lblShow = New System.Windows.Forms.Label()
        Me.cboReportType = New System.Windows.Forms.ComboBox()
        Me.dgvSalesByFilm = New System.Windows.Forms.DataGridView()
        Me.lblTicketRevenue = New System.Windows.Forms.Label()
        Me.lblFoodRevenue = New System.Windows.Forms.Label()
        Me.lblGrandTotal = New System.Windows.Forms.Label()
        Me.btnExport = New System.Windows.Forms.Button()
        Me.lblVersion = New System.Windows.Forms.Label()
        Me.lblGridCount = New System.Windows.Forms.Label()
        Me.lblMeasureBy = New System.Windows.Forms.Label()
        Me.cboMeasureBy = New System.Windows.Forms.ComboBox()
        CType(Me.dgvSalesByFilm, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblFrom
        '
        Me.lblFrom.AutoSize = True
        Me.lblFrom.Location = New System.Drawing.Point(16, 18)
        Me.lblFrom.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblFrom.Name = "lblFrom"
        Me.lblFrom.Size = New System.Drawing.Size(38, 16)
        Me.lblFrom.TabIndex = 0
        Me.lblFrom.Text = "From"
        '
        'dtpFrom
        '
        Me.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short
        Me.dtpFrom.Location = New System.Drawing.Point(82, 14)
        Me.dtpFrom.Margin = New System.Windows.Forms.Padding(4)
        Me.dtpFrom.Name = "dtpFrom"
        Me.dtpFrom.Size = New System.Drawing.Size(150, 22)
        Me.dtpFrom.TabIndex = 1
        '
        'lblTo
        '
        Me.lblTo.AutoSize = True
        Me.lblTo.Location = New System.Drawing.Point(250, 18)
        Me.lblTo.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblTo.Name = "lblTo"
        Me.lblTo.Size = New System.Drawing.Size(20, 16)
        Me.lblTo.TabIndex = 2
        Me.lblTo.Text = "To"
        '
        'dtpTo
        '
        Me.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Short
        Me.dtpTo.Location = New System.Drawing.Point(296, 14)
        Me.dtpTo.Margin = New System.Windows.Forms.Padding(4)
        Me.dtpTo.Name = "dtpTo"
        Me.dtpTo.Size = New System.Drawing.Size(150, 22)
        Me.dtpTo.TabIndex = 3
        '
        'btnRunReport
        '
        Me.btnRunReport.Location = New System.Drawing.Point(470, 12)
        Me.btnRunReport.Margin = New System.Windows.Forms.Padding(4)
        Me.btnRunReport.Name = "btnRunReport"
        Me.btnRunReport.Size = New System.Drawing.Size(140, 30)
        Me.btnRunReport.TabIndex = 4
        Me.btnRunReport.Text = "Run Report"
        Me.btnRunReport.UseVisualStyleBackColor = True
        '
        'lblShow
        '
        Me.lblShow.AutoSize = True
        Me.lblShow.Location = New System.Drawing.Point(16, 60)
        Me.lblShow.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblShow.Name = "lblShow"
        Me.lblShow.Size = New System.Drawing.Size(40, 16)
        Me.lblShow.TabIndex = 5
        Me.lblShow.Text = "Show"
        '
        'cboReportType
        '
        Me.cboReportType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboReportType.Location = New System.Drawing.Point(85, 56)
        Me.cboReportType.Margin = New System.Windows.Forms.Padding(4)
        Me.cboReportType.Name = "cboReportType"
        Me.cboReportType.Size = New System.Drawing.Size(250, 24)
        Me.cboReportType.TabIndex = 6
        '
        'dgvSalesByFilm
        '
        Me.dgvSalesByFilm.AllowUserToAddRows = False
        Me.dgvSalesByFilm.AllowUserToDeleteRows = False
        Me.dgvSalesByFilm.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvSalesByFilm.Location = New System.Drawing.Point(16, 133)
        Me.dgvSalesByFilm.Margin = New System.Windows.Forms.Padding(4)
        Me.dgvSalesByFilm.Name = "dgvSalesByFilm"
        Me.dgvSalesByFilm.ReadOnly = True
        Me.dgvSalesByFilm.RowHeadersWidth = 51
        Me.dgvSalesByFilm.Size = New System.Drawing.Size(660, 250)
        Me.dgvSalesByFilm.TabIndex = 7
        '
        'lblMeasureBy
        '
        Me.lblMeasureBy.AutoSize = True
        Me.lblMeasureBy.Location = New System.Drawing.Point(16, 99)
        Me.lblMeasureBy.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblMeasureBy.Name = "lblMeasureBy"
        Me.lblMeasureBy.Size = New System.Drawing.Size(80, 16)
        Me.lblMeasureBy.TabIndex = 13
        Me.lblMeasureBy.Text = "Measure by"
        '
        'cboMeasureBy
        '
        Me.cboMeasureBy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboMeasureBy.FormattingEnabled = True
        Me.cboMeasureBy.Location = New System.Drawing.Point(110, 95)
        Me.cboMeasureBy.Margin = New System.Windows.Forms.Padding(4)
        Me.cboMeasureBy.Name = "cboMeasureBy"
        Me.cboMeasureBy.Size = New System.Drawing.Size(225, 24)
        Me.cboMeasureBy.TabIndex = 14
        '
        'lblGridCount
        '
        Me.lblGridCount.AutoSize = True
        Me.lblGridCount.Location = New System.Drawing.Point(500, 389)
        Me.lblGridCount.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblGridCount.Name = "lblGridCount"
        Me.lblGridCount.Size = New System.Drawing.Size(50, 16)
        Me.lblGridCount.TabIndex = 12
        Me.lblGridCount.Text = "0 rows"
        '
        'lblTicketRevenue
        '
        Me.lblTicketRevenue.AutoSize = True
        Me.lblTicketRevenue.Location = New System.Drawing.Point(16, 398)
        Me.lblTicketRevenue.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblTicketRevenue.Name = "lblTicketRevenue"
        Me.lblTicketRevenue.Size = New System.Drawing.Size(120, 16)
        Me.lblTicketRevenue.TabIndex = 8
        Me.lblTicketRevenue.Text = "Ticket revenue: £0.00"
        '
        'lblFoodRevenue
        '
        Me.lblFoodRevenue.AutoSize = True
        Me.lblFoodRevenue.Location = New System.Drawing.Point(16, 426)
        Me.lblFoodRevenue.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblFoodRevenue.Name = "lblFoodRevenue"
        Me.lblFoodRevenue.Size = New System.Drawing.Size(112, 16)
        Me.lblFoodRevenue.TabIndex = 9
        Me.lblFoodRevenue.Text = "Food revenue: £0.00"
        '
        'lblGrandTotal
        '
        Me.lblGrandTotal.AutoSize = True
        Me.lblGrandTotal.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold)
        Me.lblGrandTotal.Location = New System.Drawing.Point(16, 456)
        Me.lblGrandTotal.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblGrandTotal.Name = "lblGrandTotal"
        Me.lblGrandTotal.Size = New System.Drawing.Size(140, 28)
        Me.lblGrandTotal.TabIndex = 10
        Me.lblGrandTotal.Text = "Grand total: £0.00"
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(470, 54)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(140, 30)
        Me.btnExport.Text = "Export to CSV"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'lblVersion
        '
        Me.lblVersion.AutoSize = True
        Me.lblVersion.ForeColor = System.Drawing.Color.Gray
        Me.lblVersion.Location = New System.Drawing.Point(16, 528)
        Me.lblVersion.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblVersion.Name = "lblVersion"
        Me.lblVersion.Size = New System.Drawing.Size(271, 16)
        Me.lblVersion.TabIndex = 11
        Me.lblVersion.Text = "Filmtopia Cinema Management System  v1.0"
        '
        'frmSalesReport
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(694, 568)
        Me.Controls.Add(Me.cboMeasureBy)
        Me.Controls.Add(Me.lblMeasureBy)
        Me.Controls.Add(Me.lblGridCount)
        Me.Controls.Add(Me.btnExport)
        Me.Controls.Add(Me.lblVersion)
        Me.Controls.Add(Me.lblGrandTotal)
        Me.Controls.Add(Me.lblFoodRevenue)
        Me.Controls.Add(Me.lblTicketRevenue)
        Me.Controls.Add(Me.dgvSalesByFilm)
        Me.Controls.Add(Me.cboReportType)
        Me.Controls.Add(Me.lblShow)
        Me.Controls.Add(Me.btnRunReport)
        Me.Controls.Add(Me.dtpTo)
        Me.Controls.Add(Me.lblTo)
        Me.Controls.Add(Me.dtpFrom)
        Me.Controls.Add(Me.lblFrom)
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "frmSalesReport"
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Text = "Sales Report"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        CType(Me.dgvSalesByFilm, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblFrom As Label
    Friend WithEvents dtpFrom As DateTimePicker
    Friend WithEvents lblTo As Label
    Friend WithEvents dtpTo As DateTimePicker
    Friend WithEvents btnRunReport As Button
    Friend WithEvents lblShow As Label
    Friend WithEvents cboReportType As ComboBox
    Friend WithEvents dgvSalesByFilm As DataGridView
    Friend WithEvents lblTicketRevenue As Label
    Friend WithEvents lblFoodRevenue As Label
    Friend WithEvents lblGrandTotal As Label
    Friend WithEvents btnExport As Button
    Friend WithEvents lblVersion As Label
    Friend WithEvents lblGridCount As Label
    Friend WithEvents lblMeasureBy As Label
    Friend WithEvents cboMeasureBy As ComboBox
End Class
