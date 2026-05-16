<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmScreenings
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.dgvScreenings = New System.Windows.Forms.DataGridView()
        Me.lblFilm = New System.Windows.Forms.Label()
        Me.cboFilm = New System.Windows.Forms.ComboBox()
        Me.lblScreen = New System.Windows.Forms.Label()
        Me.cboScreen = New System.Windows.Forms.ComboBox()
        Me.lblDate = New System.Windows.Forms.Label()
        Me.dtpDate = New System.Windows.Forms.DateTimePicker()
        Me.lblTime = New System.Windows.Forms.Label()
        Me.txtTime = New System.Windows.Forms.TextBox()
        Me.lblTicketPrice = New System.Windows.Forms.Label()
        Me.txtTicketPrice = New System.Windows.Forms.TextBox()
        Me.btnAdd = New System.Windows.Forms.Button()
        Me.btnUpdate = New System.Windows.Forms.Button()
        Me.btnDelete = New System.Windows.Forms.Button()
        Me.btnClear = New System.Windows.Forms.Button()
        CType(Me.dgvScreenings, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'dgvScreenings
        '
        Me.dgvScreenings.AllowUserToAddRows = False
        Me.dgvScreenings.AllowUserToDeleteRows = False
        Me.dgvScreenings.AllowUserToResizeRows = False
        Me.dgvScreenings.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.dgvScreenings.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvScreenings.Location = New System.Drawing.Point(16, 15)
        Me.dgvScreenings.MultiSelect = False
        Me.dgvScreenings.Name = "dgvScreenings"
        Me.dgvScreenings.ReadOnly = True
        Me.dgvScreenings.RowHeadersVisible = False
        Me.dgvScreenings.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvScreenings.Size = New System.Drawing.Size(1038, 369)
        Me.dgvScreenings.TabIndex = 0
        '
        'lblFilm
        '
        Me.lblFilm.AutoSize = True
        Me.lblFilm.Location = New System.Drawing.Point(16, 406)
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
        Me.cboFilm.Name = "cboFilm"
        Me.cboFilm.Size = New System.Drawing.Size(332, 24)
        Me.cboFilm.TabIndex = 2
        '
        'lblScreen
        '
        Me.lblScreen.AutoSize = True
        Me.lblScreen.Location = New System.Drawing.Point(16, 443)
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
        Me.cboScreen.Name = "cboScreen"
        Me.cboScreen.Size = New System.Drawing.Size(200, 24)
        Me.cboScreen.TabIndex = 4
        '
        'lblDate
        '
        Me.lblDate.AutoSize = True
        Me.lblDate.Location = New System.Drawing.Point(16, 480)
        Me.lblDate.Name = "lblDate"
        Me.lblDate.Size = New System.Drawing.Size(36, 16)
        Me.lblDate.TabIndex = 5
        Me.lblDate.Text = "Date"
        '
        'dtpDate
        '
        Me.dtpDate.Location = New System.Drawing.Point(160, 476)
        Me.dtpDate.Name = "dtpDate"
        Me.dtpDate.Size = New System.Drawing.Size(260, 22)
        Me.dtpDate.TabIndex = 6
        '
        'lblTime
        '
        Me.lblTime.AutoSize = True
        Me.lblTime.Location = New System.Drawing.Point(16, 517)
        Me.lblTime.Name = "lblTime"
        Me.lblTime.Size = New System.Drawing.Size(91, 16)
        Me.lblTime.TabIndex = 7
        Me.lblTime.Text = "Time (HH:MM)"
        '
        'txtTime
        '
        Me.txtTime.Location = New System.Drawing.Point(160, 513)
        Me.txtTime.Name = "txtTime"
        Me.txtTime.Size = New System.Drawing.Size(132, 22)
        Me.txtTime.TabIndex = 8
        '
        'lblTicketPrice
        '
        Me.lblTicketPrice.AutoSize = True
        Me.lblTicketPrice.Location = New System.Drawing.Point(16, 554)
        Me.lblTicketPrice.Name = "lblTicketPrice"
        Me.lblTicketPrice.Size = New System.Drawing.Size(98, 16)
        Me.lblTicketPrice.TabIndex = 9
        Me.lblTicketPrice.Text = "Ticket Price (£)"
        '
        'txtTicketPrice
        '
        Me.txtTicketPrice.Location = New System.Drawing.Point(160, 550)
        Me.txtTicketPrice.Name = "txtTicketPrice"
        Me.txtTicketPrice.Size = New System.Drawing.Size(132, 22)
        Me.txtTicketPrice.TabIndex = 10
        '
        'btnAdd
        '
        Me.btnAdd.Location = New System.Drawing.Point(728, 402)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(120, 37)
        Me.btnAdd.TabIndex = 11
        Me.btnAdd.Text = "Add"
        Me.btnAdd.UseVisualStyleBackColor = True
        '
        'btnUpdate
        '
        Me.btnUpdate.Location = New System.Drawing.Point(856, 402)
        Me.btnUpdate.Name = "btnUpdate"
        Me.btnUpdate.Size = New System.Drawing.Size(120, 37)
        Me.btnUpdate.TabIndex = 12
        Me.btnUpdate.Text = "Update"
        Me.btnUpdate.UseVisualStyleBackColor = True
        '
        'btnDelete
        '
        Me.btnDelete.Location = New System.Drawing.Point(728, 447)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(120, 37)
        Me.btnDelete.TabIndex = 13
        Me.btnDelete.Text = "Delete"
        Me.btnDelete.UseVisualStyleBackColor = True
        '
        'btnClear
        '
        Me.btnClear.Location = New System.Drawing.Point(856, 447)
        Me.btnClear.Name = "btnClear"
        Me.btnClear.Size = New System.Drawing.Size(120, 37)
        Me.btnClear.TabIndex = 14
        Me.btnClear.Text = "Clear"
        Me.btnClear.UseVisualStyleBackColor = True
        '
        'frmScreenings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1067, 605)
        Me.Controls.Add(Me.btnClear)
        Me.Controls.Add(Me.btnDelete)
        Me.Controls.Add(Me.btnUpdate)
        Me.Controls.Add(Me.btnAdd)
        Me.Controls.Add(Me.txtTicketPrice)
        Me.Controls.Add(Me.lblTicketPrice)
        Me.Controls.Add(Me.txtTime)
        Me.Controls.Add(Me.lblTime)
        Me.Controls.Add(Me.dtpDate)
        Me.Controls.Add(Me.lblDate)
        Me.Controls.Add(Me.cboScreen)
        Me.Controls.Add(Me.lblScreen)
        Me.Controls.Add(Me.cboFilm)
        Me.Controls.Add(Me.lblFilm)
        Me.Controls.Add(Me.dgvScreenings)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "frmScreenings"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Filmtopia - Screenings"
        CType(Me.dgvScreenings, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents dgvScreenings As System.Windows.Forms.DataGridView
    Friend WithEvents lblFilm As System.Windows.Forms.Label
    Friend WithEvents cboFilm As System.Windows.Forms.ComboBox
    Friend WithEvents lblScreen As System.Windows.Forms.Label
    Friend WithEvents cboScreen As System.Windows.Forms.ComboBox
    Friend WithEvents lblDate As System.Windows.Forms.Label
    Friend WithEvents dtpDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents lblTime As System.Windows.Forms.Label
    Friend WithEvents txtTime As System.Windows.Forms.TextBox
    Friend WithEvents lblTicketPrice As System.Windows.Forms.Label
    Friend WithEvents txtTicketPrice As System.Windows.Forms.TextBox
    Friend WithEvents btnAdd As System.Windows.Forms.Button
    Friend WithEvents btnUpdate As System.Windows.Forms.Button
    Friend WithEvents btnDelete As System.Windows.Forms.Button
    Friend WithEvents btnClear As System.Windows.Forms.Button
End Class
