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
        Me.dgvSalesByFilm = New System.Windows.Forms.DataGridView()
        Me.lblTicketRevenue = New System.Windows.Forms.Label()
        Me.lblFoodRevenue = New System.Windows.Forms.Label()
        Me.lblGrandTotal = New System.Windows.Forms.Label()
        Me.lblVersion = New System.Windows.Forms.Label()
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
        Me.dtpFrom.Location = New System.Drawing.Point(70, 14)
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
        Me.dtpTo.Location = New System.Drawing.Point(290, 14)
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
        'dgvSalesByFilm
        '
        Me.dgvSalesByFilm.AllowUserToAddRows = False
        Me.dgvSalesByFilm.AllowUserToDeleteRows = False
        Me.dgvSalesByFilm.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvSalesByFilm.Location = New System.Drawing.Point(16, 56)
        Me.dgvSalesByFilm.Margin = New System.Windows.Forms.Padding(4)
        Me.dgvSalesByFilm.Name = "dgvSalesByFilm"
        Me.dgvSalesByFilm.ReadOnly = True
        Me.dgvSalesByFilm.RowHeadersWidth = 51
        Me.dgvSalesByFilm.Size = New System.Drawing.Size(660, 250)
        Me.dgvSalesByFilm.TabIndex = 5
        '
        'lblTicketRevenue
        '
        Me.lblTicketRevenue.AutoSize = True
        Me.lblTicketRevenue.Location = New System.Drawing.Point(16, 322)
        Me.lblTicketRevenue.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblTicketRevenue.Name = "lblTicketRevenue"
        Me.lblTicketRevenue.Size = New System.Drawing.Size(120, 16)
        Me.lblTicketRevenue.TabIndex = 6
        Me.lblTicketRevenue.Text = "Ticket revenue: £0.00"
        '
        'lblFoodRevenue
        '
        Me.lblFoodRevenue.AutoSize = True
        Me.lblFoodRevenue.Location = New System.Drawing.Point(16, 350)
        Me.lblFoodRevenue.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblFoodRevenue.Name = "lblFoodRevenue"
        Me.lblFoodRevenue.Size = New System.Drawing.Size(112, 16)
        Me.lblFoodRevenue.TabIndex = 7
        Me.lblFoodRevenue.Text = "Food revenue: £0.00"
        '
        'lblGrandTotal
        '
        Me.lblGrandTotal.AutoSize = True
        Me.lblGrandTotal.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold)
        Me.lblGrandTotal.Location = New System.Drawing.Point(16, 380)
        Me.lblGrandTotal.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblGrandTotal.Name = "lblGrandTotal"
        Me.lblGrandTotal.Size = New System.Drawing.Size(140, 28)
        Me.lblGrandTotal.TabIndex = 8
        Me.lblGrandTotal.Text = "Grand total: £0.00"
        '
        'lblVersion
        '
        Me.lblVersion.AutoSize = True
        Me.lblVersion.ForeColor = System.Drawing.Color.Gray
        Me.lblVersion.Location = New System.Drawing.Point(16, 450)
        Me.lblVersion.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblVersion.Name = "lblVersion"
        Me.lblVersion.Size = New System.Drawing.Size(271, 16)
        Me.lblVersion.TabIndex = 9
        Me.lblVersion.Text = "Filmtopia Cinema Management System  v1.0"
        '
        'frmSalesReport
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(694, 490)
        Me.Controls.Add(Me.lblVersion)
        Me.Controls.Add(Me.lblGrandTotal)
        Me.Controls.Add(Me.lblFoodRevenue)
        Me.Controls.Add(Me.lblTicketRevenue)
        Me.Controls.Add(Me.dgvSalesByFilm)
        Me.Controls.Add(Me.btnRunReport)
        Me.Controls.Add(Me.dtpTo)
        Me.Controls.Add(Me.lblTo)
        Me.Controls.Add(Me.dtpFrom)
        Me.Controls.Add(Me.lblFrom)
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "frmSalesReport"
        Me.Text = "Sales Report"
        CType(Me.dgvSalesByFilm, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblFrom As Label
    Friend WithEvents dtpFrom As DateTimePicker
    Friend WithEvents lblTo As Label
    Friend WithEvents dtpTo As DateTimePicker
    Friend WithEvents btnRunReport As Button
    Friend WithEvents dgvSalesByFilm As DataGridView
    Friend WithEvents lblTicketRevenue As Label
    Friend WithEvents lblFoodRevenue As Label
    Friend WithEvents lblGrandTotal As Label
    Friend WithEvents lblVersion As Label
End Class
