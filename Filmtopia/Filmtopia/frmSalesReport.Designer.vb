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
        Me.lblFind = New System.Windows.Forms.Label()
        Me.txtFind = New System.Windows.Forms.TextBox()
        Me.btnFind = New System.Windows.Forms.Button()
        Me.lblQuick = New System.Windows.Forms.Label()
        Me.cboQuickRange = New System.Windows.Forms.ComboBox()
        Me.pnlCard1 = New System.Windows.Forms.Panel()
        Me.lblCardTitle1 = New System.Windows.Forms.Label()
        Me.lblStat1 = New System.Windows.Forms.Label()
        Me.lblCardSub1 = New System.Windows.Forms.Label()
        Me.pnlCard2 = New System.Windows.Forms.Panel()
        Me.lblCardTitle2 = New System.Windows.Forms.Label()
        Me.lblStat2 = New System.Windows.Forms.Label()
        Me.lblCardSub2 = New System.Windows.Forms.Label()
        Me.pnlCard3 = New System.Windows.Forms.Panel()
        Me.lblCardTitle3 = New System.Windows.Forms.Label()
        Me.lblStat3 = New System.Windows.Forms.Label()
        Me.lblCardSub3 = New System.Windows.Forms.Label()
        Me.pnlCard4 = New System.Windows.Forms.Panel()
        Me.lblCardTitle4 = New System.Windows.Forms.Label()
        Me.lblStat4 = New System.Windows.Forms.Label()
        Me.lblCardSub4 = New System.Windows.Forms.Label()
        Me.pnlCard5 = New System.Windows.Forms.Panel()
        Me.lblCardTitle5 = New System.Windows.Forms.Label()
        Me.lblStat5 = New System.Windows.Forms.Label()
        Me.lblCardSub5 = New System.Windows.Forms.Label()
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
        Me.dgvSalesByFilm.RowHeadersVisible = False
        Me.dgvSalesByFilm.RowHeadersWidth = 51
        Me.dgvSalesByFilm.Size = New System.Drawing.Size(660, 250)
        Me.dgvSalesByFilm.TabIndex = 7
        '
        'lblMeasureBy
        '
        Me.lblMeasureBy.AutoSize = True
        Me.lblMeasureBy.Location = New System.Drawing.Point(360, 60)
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
        Me.cboMeasureBy.Location = New System.Drawing.Point(455, 56)
        Me.cboMeasureBy.Margin = New System.Windows.Forms.Padding(4)
        Me.cboMeasureBy.Name = "cboMeasureBy"
        Me.cboMeasureBy.Size = New System.Drawing.Size(200, 24)
        Me.cboMeasureBy.TabIndex = 14
        '
        'pnlCard1
        '
        Me.pnlCard1.Controls.Add(Me.lblCardTitle1)
        Me.pnlCard1.Controls.Add(Me.lblStat1)
        Me.pnlCard1.Controls.Add(Me.lblCardSub1)
        Me.pnlCard1.Location = New System.Drawing.Point(16, 136)
        Me.pnlCard1.Margin = New System.Windows.Forms.Padding(4)
        Me.pnlCard1.Name = "pnlCard1"
        Me.pnlCard1.Size = New System.Drawing.Size(200, 112)
        Me.pnlCard1.TabIndex = 21
        '
        'lblCardTitle1
        '
        Me.lblCardTitle1.AutoSize = True
        Me.lblCardTitle1.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.lblCardTitle1.Location = New System.Drawing.Point(12, 8)
        Me.lblCardTitle1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblCardTitle1.Name = "lblCardTitle1"
        Me.lblCardTitle1.Size = New System.Drawing.Size(80, 17)
        Me.lblCardTitle1.TabIndex = 0
        Me.lblCardTitle1.Text = "Money taken"
        '
        'lblStat1
        '
        Me.lblStat1.AutoSize = True
        Me.lblStat1.Font = New System.Drawing.Font("Segoe UI", 14.0!, System.Drawing.FontStyle.Bold)
        Me.lblStat1.Location = New System.Drawing.Point(12, 40)
        Me.lblStat1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblStat1.Name = "lblStat1"
        Me.lblStat1.Size = New System.Drawing.Size(30, 30)
        Me.lblStat1.TabIndex = 1
        Me.lblStat1.Text = "0"
        '
        'lblCardSub1
        '
        Me.lblCardSub1.AutoSize = True
        Me.lblCardSub1.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.lblCardSub1.Location = New System.Drawing.Point(12, 84)
        Me.lblCardSub1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblCardSub1.Name = "lblCardSub1"
        Me.lblCardSub1.Size = New System.Drawing.Size(60, 13)
        Me.lblCardSub1.TabIndex = 2
        Me.lblCardSub1.Text = "tickets £0.00, snacks £0.00"
        '
        'pnlCard2
        '
        Me.pnlCard2.Controls.Add(Me.lblCardTitle2)
        Me.pnlCard2.Controls.Add(Me.lblStat2)
        Me.pnlCard2.Controls.Add(Me.lblCardSub2)
        Me.pnlCard2.Location = New System.Drawing.Point(228, 136)
        Me.pnlCard2.Margin = New System.Windows.Forms.Padding(4)
        Me.pnlCard2.Name = "pnlCard2"
        Me.pnlCard2.Size = New System.Drawing.Size(200, 112)
        Me.pnlCard2.TabIndex = 22
        '
        'lblCardTitle2
        '
        Me.lblCardTitle2.AutoSize = True
        Me.lblCardTitle2.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.lblCardTitle2.Location = New System.Drawing.Point(12, 8)
        Me.lblCardTitle2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblCardTitle2.Name = "lblCardTitle2"
        Me.lblCardTitle2.Size = New System.Drawing.Size(80, 17)
        Me.lblCardTitle2.TabIndex = 0
        Me.lblCardTitle2.Text = "Tickets sold"
        '
        'lblStat2
        '
        Me.lblStat2.AutoSize = True
        Me.lblStat2.Font = New System.Drawing.Font("Segoe UI", 14.0!, System.Drawing.FontStyle.Bold)
        Me.lblStat2.Location = New System.Drawing.Point(12, 40)
        Me.lblStat2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblStat2.Name = "lblStat2"
        Me.lblStat2.Size = New System.Drawing.Size(30, 30)
        Me.lblStat2.TabIndex = 1
        Me.lblStat2.Text = "0"
        '
        'lblCardSub2
        '
        Me.lblCardSub2.AutoSize = True
        Me.lblCardSub2.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.lblCardSub2.Location = New System.Drawing.Point(12, 84)
        Me.lblCardSub2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblCardSub2.Name = "lblCardSub2"
        Me.lblCardSub2.Size = New System.Drawing.Size(60, 13)
        Me.lblCardSub2.TabIndex = 2
        Me.lblCardSub2.Text = "on 0 bookings"
        '
        'pnlCard3
        '
        Me.pnlCard3.Controls.Add(Me.lblCardTitle3)
        Me.pnlCard3.Controls.Add(Me.lblStat3)
        Me.pnlCard3.Controls.Add(Me.lblCardSub3)
        Me.pnlCard3.Location = New System.Drawing.Point(440, 136)
        Me.pnlCard3.Margin = New System.Windows.Forms.Padding(4)
        Me.pnlCard3.Name = "pnlCard3"
        Me.pnlCard3.Size = New System.Drawing.Size(200, 112)
        Me.pnlCard3.TabIndex = 23
        '
        'lblCardTitle3
        '
        Me.lblCardTitle3.AutoSize = True
        Me.lblCardTitle3.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.lblCardTitle3.Location = New System.Drawing.Point(12, 8)
        Me.lblCardTitle3.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblCardTitle3.Name = "lblCardTitle3"
        Me.lblCardTitle3.Size = New System.Drawing.Size(80, 17)
        Me.lblCardTitle3.TabIndex = 0
        Me.lblCardTitle3.Text = "Average ticket"
        '
        'lblStat3
        '
        Me.lblStat3.AutoSize = True
        Me.lblStat3.Font = New System.Drawing.Font("Segoe UI", 14.0!, System.Drawing.FontStyle.Bold)
        Me.lblStat3.Location = New System.Drawing.Point(12, 40)
        Me.lblStat3.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblStat3.Name = "lblStat3"
        Me.lblStat3.Size = New System.Drawing.Size(30, 30)
        Me.lblStat3.TabIndex = 1
        Me.lblStat3.Text = "0"
        '
        'lblCardSub3
        '
        Me.lblCardSub3.AutoSize = True
        Me.lblCardSub3.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.lblCardSub3.Location = New System.Drawing.Point(12, 84)
        Me.lblCardSub3.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblCardSub3.Name = "lblCardSub3"
        Me.lblCardSub3.Size = New System.Drawing.Size(60, 13)
        Me.lblCardSub3.TabIndex = 2
        Me.lblCardSub3.Text = "per seat sold"
        '
        'pnlCard4
        '
        Me.pnlCard4.Controls.Add(Me.lblCardTitle4)
        Me.pnlCard4.Controls.Add(Me.lblStat4)
        Me.pnlCard4.Controls.Add(Me.lblCardSub4)
        Me.pnlCard4.Location = New System.Drawing.Point(652, 136)
        Me.pnlCard4.Margin = New System.Windows.Forms.Padding(4)
        Me.pnlCard4.Name = "pnlCard4"
        Me.pnlCard4.Size = New System.Drawing.Size(200, 112)
        Me.pnlCard4.TabIndex = 24
        '
        'lblCardTitle4
        '
        Me.lblCardTitle4.AutoSize = True
        Me.lblCardTitle4.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.lblCardTitle4.Location = New System.Drawing.Point(12, 8)
        Me.lblCardTitle4.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblCardTitle4.Name = "lblCardTitle4"
        Me.lblCardTitle4.Size = New System.Drawing.Size(80, 17)
        Me.lblCardTitle4.TabIndex = 0
        Me.lblCardTitle4.Text = "How full"
        '
        'lblStat4
        '
        Me.lblStat4.AutoSize = True
        Me.lblStat4.Font = New System.Drawing.Font("Segoe UI", 14.0!, System.Drawing.FontStyle.Bold)
        Me.lblStat4.Location = New System.Drawing.Point(12, 40)
        Me.lblStat4.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblStat4.Name = "lblStat4"
        Me.lblStat4.Size = New System.Drawing.Size(30, 30)
        Me.lblStat4.TabIndex = 1
        Me.lblStat4.Text = "0"
        '
        'lblCardSub4
        '
        Me.lblCardSub4.AutoSize = True
        Me.lblCardSub4.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.lblCardSub4.Location = New System.Drawing.Point(12, 84)
        Me.lblCardSub4.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblCardSub4.Name = "lblCardSub4"
        Me.lblCardSub4.Size = New System.Drawing.Size(60, 13)
        Me.lblCardSub4.TabIndex = 2
        Me.lblCardSub4.Text = "0 of 0 seats"
        '
        'pnlCard5
        '
        Me.pnlCard5.Controls.Add(Me.lblCardTitle5)
        Me.pnlCard5.Controls.Add(Me.lblStat5)
        Me.pnlCard5.Controls.Add(Me.lblCardSub5)
        Me.pnlCard5.Location = New System.Drawing.Point(864, 136)
        Me.pnlCard5.Margin = New System.Windows.Forms.Padding(4)
        Me.pnlCard5.Name = "pnlCard5"
        Me.pnlCard5.Size = New System.Drawing.Size(200, 112)
        Me.pnlCard5.TabIndex = 25
        '
        'lblCardTitle5
        '
        Me.lblCardTitle5.AutoSize = True
        Me.lblCardTitle5.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.lblCardTitle5.Location = New System.Drawing.Point(12, 8)
        Me.lblCardTitle5.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblCardTitle5.Name = "lblCardTitle5"
        Me.lblCardTitle5.Size = New System.Drawing.Size(80, 17)
        Me.lblCardTitle5.TabIndex = 0
        Me.lblCardTitle5.Text = "Snacks per head"
        '
        'lblStat5
        '
        Me.lblStat5.AutoSize = True
        Me.lblStat5.Font = New System.Drawing.Font("Segoe UI", 14.0!, System.Drawing.FontStyle.Bold)
        Me.lblStat5.Location = New System.Drawing.Point(12, 40)
        Me.lblStat5.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblStat5.Name = "lblStat5"
        Me.lblStat5.Size = New System.Drawing.Size(30, 30)
        Me.lblStat5.TabIndex = 1
        Me.lblStat5.Text = "0"
        '
        'lblCardSub5
        '
        Me.lblCardSub5.AutoSize = True
        Me.lblCardSub5.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.lblCardSub5.Location = New System.Drawing.Point(12, 84)
        Me.lblCardSub5.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblCardSub5.Name = "lblCardSub5"
        Me.lblCardSub5.Size = New System.Drawing.Size(60, 13)
        Me.lblCardSub5.TabIndex = 2
        Me.lblCardSub5.Text = "for every ticket"
        '
        'lblQuick
        '
        Me.lblQuick.AutoSize = True
        Me.lblQuick.Location = New System.Drawing.Point(470, 18)
        Me.lblQuick.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblQuick.Name = "lblQuick"
        Me.lblQuick.Size = New System.Drawing.Size(45, 16)
        Me.lblQuick.TabIndex = 18
        Me.lblQuick.Text = "Quick"
        '
        'cboQuickRange
        '
        Me.cboQuickRange.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboQuickRange.FormattingEnabled = True
        Me.cboQuickRange.Location = New System.Drawing.Point(540, 14)
        Me.cboQuickRange.Margin = New System.Windows.Forms.Padding(4)
        Me.cboQuickRange.Name = "cboQuickRange"
        Me.cboQuickRange.Size = New System.Drawing.Size(190, 24)
        Me.cboQuickRange.TabIndex = 19
        '
        'lblFind
        '
        Me.lblFind.AutoSize = True
        Me.lblFind.Location = New System.Drawing.Point(16, 102)
        Me.lblFind.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblFind.Name = "lblFind"
        Me.lblFind.Size = New System.Drawing.Size(35, 16)
        Me.lblFind.TabIndex = 15
        Me.lblFind.Text = "Find"
        '
        'txtFind
        '
        Me.txtFind.Location = New System.Drawing.Point(85, 98)
        Me.txtFind.Margin = New System.Windows.Forms.Padding(4)
        Me.txtFind.Name = "txtFind"
        Me.txtFind.Size = New System.Drawing.Size(220, 22)
        Me.txtFind.TabIndex = 16
        '
        'btnFind
        '
        Me.btnFind.Location = New System.Drawing.Point(315, 96)
        Me.btnFind.Margin = New System.Windows.Forms.Padding(4)
        Me.btnFind.Name = "btnFind"
        Me.btnFind.Size = New System.Drawing.Size(90, 26)
        Me.btnFind.TabIndex = 17
        Me.btnFind.Text = "Find"
        Me.btnFind.UseVisualStyleBackColor = True
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
        Me.ClientSize = New System.Drawing.Size(1100, 720)
        Me.Controls.Add(Me.pnlCard1)
        Me.Controls.Add(Me.pnlCard2)
        Me.Controls.Add(Me.pnlCard3)
        Me.Controls.Add(Me.pnlCard4)
        Me.Controls.Add(Me.pnlCard5)
        Me.Controls.Add(Me.cboQuickRange)
        Me.Controls.Add(Me.lblQuick)
        Me.Controls.Add(Me.btnFind)
        Me.Controls.Add(Me.txtFind)
        Me.Controls.Add(Me.lblFind)
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
        Me.MinimumSize = New System.Drawing.Size(920, 640)
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
    Friend WithEvents lblFind As Label
    Friend WithEvents txtFind As TextBox
    Friend WithEvents btnFind As Button
    Friend WithEvents lblQuick As Label
    Friend WithEvents cboQuickRange As ComboBox
    Friend WithEvents pnlCard1 As Panel
    Friend WithEvents lblCardTitle1 As Label
    Friend WithEvents lblStat1 As Label
    Friend WithEvents lblCardSub1 As Label
    Friend WithEvents pnlCard2 As Panel
    Friend WithEvents lblCardTitle2 As Label
    Friend WithEvents lblStat2 As Label
    Friend WithEvents lblCardSub2 As Label
    Friend WithEvents pnlCard3 As Panel
    Friend WithEvents lblCardTitle3 As Label
    Friend WithEvents lblStat3 As Label
    Friend WithEvents lblCardSub3 As Label
    Friend WithEvents pnlCard4 As Panel
    Friend WithEvents lblCardTitle4 As Label
    Friend WithEvents lblStat4 As Label
    Friend WithEvents lblCardSub4 As Label
    Friend WithEvents pnlCard5 As Panel
    Friend WithEvents lblCardTitle5 As Label
    Friend WithEvents lblStat5 As Label
    Friend WithEvents lblCardSub5 As Label
End Class
