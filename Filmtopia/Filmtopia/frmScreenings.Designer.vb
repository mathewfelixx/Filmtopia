<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmScreenings
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
        Me.dgvScreenings = New System.Windows.Forms.DataGridView()
        Me.lblFilm = New System.Windows.Forms.Label()
        Me.cboFilm = New System.Windows.Forms.ComboBox()
        Me.lblScreen = New System.Windows.Forms.Label()
        Me.cboScreen = New System.Windows.Forms.ComboBox()
        Me.lblScreeningDate = New System.Windows.Forms.Label()
        Me.dtpScreeningDate = New System.Windows.Forms.DateTimePicker()
        Me.lblScreeningTime = New System.Windows.Forms.Label()
        Me.txtScreeningTime = New System.Windows.Forms.TextBox()
        Me.lblTicketPrice = New System.Windows.Forms.Label()
        Me.txtTicketPrice = New System.Windows.Forms.TextBox()
        Me.btnAdd = New System.Windows.Forms.Button()
        Me.btnUpdate = New System.Windows.Forms.Button()
        Me.btnDelete = New System.Windows.Forms.Button()
        Me.btnClear = New System.Windows.Forms.Button()
        Me.lblVersion = New System.Windows.Forms.Label()
        CType(Me.dgvScreenings, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'dgvScreenings
        '
        Me.dgvScreenings.AllowUserToAddRows = False
        Me.dgvScreenings.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvScreenings.Location = New System.Drawing.Point(16, 15)
        Me.dgvScreenings.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.dgvScreenings.MultiSelect = False
        Me.dgvScreenings.Name = "dgvScreenings"
        Me.dgvScreenings.RowHeadersWidth = 51
        Me.dgvScreenings.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvScreenings.Size = New System.Drawing.Size(1038, 369)
        Me.dgvScreenings.TabIndex = 0
        '
        'lblFilm
        '
        Me.lblFilm.AutoSize = True
        Me.lblFilm.Location = New System.Drawing.Point(16, 406)
        Me.lblFilm.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblFilm.Name = "lblFilm"
        Me.lblFilm.Size = New System.Drawing.Size(32, 16)
        Me.lblFilm.TabIndex = 1
        Me.lblFilm.Text = "Film"
        '
        'cboFilm
        '
        Me.cboFilm.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboFilm.FormattingEnabled = True
        Me.cboFilm.Location = New System.Drawing.Point(160, 402)
        Me.cboFilm.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.cboFilm.Name = "cboFilm"
        Me.cboFilm.Size = New System.Drawing.Size(332, 24)
        Me.cboFilm.TabIndex = 2
        '
        'lblScreen
        '
        Me.lblScreen.AutoSize = True
        Me.lblScreen.Location = New System.Drawing.Point(16, 443)
        Me.lblScreen.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblScreen.Name = "lblScreen"
        Me.lblScreen.Size = New System.Drawing.Size(48, 16)
        Me.lblScreen.TabIndex = 3
        Me.lblScreen.Text = "Screen"
        '
        'cboScreen
        '
        Me.cboScreen.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboScreen.FormattingEnabled = True
        Me.cboScreen.Location = New System.Drawing.Point(160, 439)
        Me.cboScreen.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.cboScreen.Name = "cboScreen"
        Me.cboScreen.Size = New System.Drawing.Size(200, 24)
        Me.cboScreen.TabIndex = 4
        '
        'lblScreeningDate
        '
        Me.lblScreeningDate.AutoSize = True
        Me.lblScreeningDate.Location = New System.Drawing.Point(16, 480)
        Me.lblScreeningDate.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblScreeningDate.Name = "lblScreeningDate"
        Me.lblScreeningDate.Size = New System.Drawing.Size(46, 16)
        Me.lblScreeningDate.TabIndex = 5
        Me.lblScreeningDate.Text = "Date"
        '
        'dtpScreeningDate
        '
        Me.dtpScreeningDate.Format = System.Windows.Forms.DateTimePickerFormat.Short
        Me.dtpScreeningDate.Location = New System.Drawing.Point(160, 476)
        Me.dtpScreeningDate.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.dtpScreeningDate.Name = "dtpScreeningDate"
        Me.dtpScreeningDate.Size = New System.Drawing.Size(200, 22)
        Me.dtpScreeningDate.TabIndex = 6
        '
        'lblScreeningTime
        '
        Me.lblScreeningTime.AutoSize = True
        Me.lblScreeningTime.Location = New System.Drawing.Point(16, 517)
        Me.lblScreeningTime.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblScreeningTime.Name = "lblScreeningTime"
        Me.lblScreeningTime.Size = New System.Drawing.Size(122, 16)
        Me.lblScreeningTime.TabIndex = 7
        Me.lblScreeningTime.Text = "Time (HH:MM)"
        '
        'txtScreeningTime
        '
        Me.txtScreeningTime.Location = New System.Drawing.Point(160, 513)
        Me.txtScreeningTime.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.txtScreeningTime.Name = "txtScreeningTime"
        Me.txtScreeningTime.Size = New System.Drawing.Size(132, 22)
        Me.txtScreeningTime.TabIndex = 8
        '
        'lblTicketPrice
        '
        Me.lblTicketPrice.AutoSize = True
        Me.lblTicketPrice.Location = New System.Drawing.Point(16, 554)
        Me.lblTicketPrice.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblTicketPrice.Name = "lblTicketPrice"
        Me.lblTicketPrice.Size = New System.Drawing.Size(76, 16)
        Me.lblTicketPrice.TabIndex = 9
        Me.lblTicketPrice.Text = "Ticket Price"
        '
        'txtTicketPrice
        '
        Me.txtTicketPrice.Location = New System.Drawing.Point(160, 550)
        Me.txtTicketPrice.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.txtTicketPrice.Name = "txtTicketPrice"
        Me.txtTicketPrice.Size = New System.Drawing.Size(132, 22)
        Me.txtTicketPrice.TabIndex = 10
        '
        'btnAdd
        '
        Me.btnAdd.Location = New System.Drawing.Point(600, 402)
        Me.btnAdd.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(120, 37)
        Me.btnAdd.TabIndex = 11
        Me.btnAdd.Text = "Add"
        Me.btnAdd.UseVisualStyleBackColor = True
        '
        'btnUpdate
        '
        Me.btnUpdate.Location = New System.Drawing.Point(728, 402)
        Me.btnUpdate.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnUpdate.Name = "btnUpdate"
        Me.btnUpdate.Size = New System.Drawing.Size(120, 37)
        Me.btnUpdate.TabIndex = 12
        Me.btnUpdate.Text = "Update"
        Me.btnUpdate.UseVisualStyleBackColor = True
        '
        'btnDelete
        '
        Me.btnDelete.Location = New System.Drawing.Point(600, 447)
        Me.btnDelete.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(120, 37)
        Me.btnDelete.TabIndex = 13
        Me.btnDelete.Text = "Delete"
        Me.btnDelete.UseVisualStyleBackColor = True
        '
        'btnClear
        '
        Me.btnClear.Location = New System.Drawing.Point(728, 447)
        Me.btnClear.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnClear.Name = "btnClear"
        Me.btnClear.Size = New System.Drawing.Size(120, 37)
        Me.btnClear.TabIndex = 14
        Me.btnClear.Text = "Clear"
        Me.btnClear.UseVisualStyleBackColor = True
        '
        'lblVersion
        '
        Me.lblVersion.AutoSize = True
        Me.lblVersion.ForeColor = System.Drawing.Color.Gray
        Me.lblVersion.Location = New System.Drawing.Point(733, 603)
        Me.lblVersion.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblVersion.Name = "lblVersion"
        Me.lblVersion.Size = New System.Drawing.Size(271, 16)
        Me.lblVersion.TabIndex = 15
        Me.lblVersion.Text = "Filmtopia Cinema Management System  v1.0"
        '
        'frmScreenings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1067, 640)
        Me.Controls.Add(Me.lblVersion)
        Me.Controls.Add(Me.btnClear)
        Me.Controls.Add(Me.btnDelete)
        Me.Controls.Add(Me.btnUpdate)
        Me.Controls.Add(Me.btnAdd)
        Me.Controls.Add(Me.txtTicketPrice)
        Me.Controls.Add(Me.lblTicketPrice)
        Me.Controls.Add(Me.txtScreeningTime)
        Me.Controls.Add(Me.lblScreeningTime)
        Me.Controls.Add(Me.dtpScreeningDate)
        Me.Controls.Add(Me.lblScreeningDate)
        Me.Controls.Add(Me.cboScreen)
        Me.Controls.Add(Me.lblScreen)
        Me.Controls.Add(Me.cboFilm)
        Me.Controls.Add(Me.lblFilm)
        Me.Controls.Add(Me.dgvScreenings)
        Me.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.Name = "frmScreenings"
        Me.Text = "Screenings"
        CType(Me.dgvScreenings, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents dgvScreenings As DataGridView
    Friend WithEvents lblFilm As Label
    Friend WithEvents cboFilm As ComboBox
    Friend WithEvents lblScreen As Label
    Friend WithEvents cboScreen As ComboBox
    Friend WithEvents lblScreeningDate As Label
    Friend WithEvents dtpScreeningDate As DateTimePicker
    Friend WithEvents lblScreeningTime As Label
    Friend WithEvents txtScreeningTime As TextBox
    Friend WithEvents lblTicketPrice As Label
    Friend WithEvents txtTicketPrice As TextBox
    Friend WithEvents btnAdd As Button
    Friend WithEvents btnUpdate As Button
    Friend WithEvents btnDelete As Button
    Friend WithEvents btnClear As Button
    Friend WithEvents lblVersion As Label
End Class
