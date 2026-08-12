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
        Me.lblHeading = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.lblShow = New System.Windows.Forms.Label()
        Me.cboShow = New System.Windows.Forms.ComboBox()
        Me.lblGridCount = New System.Windows.Forms.Label()
        Me.dgvScreenings = New System.Windows.Forms.DataGridView()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.lblFilm = New System.Windows.Forms.Label()
        Me.cboFilm = New System.Windows.Forms.ComboBox()
        Me.lblScreen = New System.Windows.Forms.Label()
        Me.cboScreen = New System.Windows.Forms.ComboBox()
        Me.lblDate = New System.Windows.Forms.Label()
        Me.dtpScreeningDate = New System.Windows.Forms.DateTimePicker()
        Me.lblTime = New System.Windows.Forms.Label()
        Me.txtScreeningTime = New System.Windows.Forms.TextBox()
        Me.lblPrice = New System.Windows.Forms.Label()
        Me.txtTicketPrice = New System.Windows.Forms.TextBox()
        Me.lblEndsAt = New System.Windows.Forms.Label()
        Me.btnAdd = New System.Windows.Forms.Button()
        Me.btnUpdate = New System.Windows.Forms.Button()
        Me.btnDelete = New System.Windows.Forms.Button()
        Me.btnClear = New System.Windows.Forms.Button()
        Me.btnSuggest = New System.Windows.Forms.Button()
        Me.btnExport = New System.Windows.Forms.Button()
        Me.lblSaved = New System.Windows.Forms.Label()
        Me.lblVersion = New System.Windows.Forms.Label()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        CType(Me.dgvScreenings, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblHeading
        '
        Me.lblHeading.AutoSize = True
        Me.lblHeading.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Bold)
        Me.lblHeading.Location = New System.Drawing.Point(16, 12)
        Me.lblHeading.Name = "lblHeading"
        Me.lblHeading.Size = New System.Drawing.Size(130, 25)
        Me.lblHeading.TabIndex = 0
        Me.lblHeading.Text = "Screenings"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.lblShow)
        Me.GroupBox1.Controls.Add(Me.cboShow)
        Me.GroupBox1.Controls.Add(Me.lblGridCount)
        Me.GroupBox1.Controls.Add(Me.dgvScreenings)
        Me.GroupBox1.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.GroupBox1.Location = New System.Drawing.Point(16, 46)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(1036, 400)
        Me.GroupBox1.TabIndex = 1
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "What is scheduled"
        '
        'lblShow
        '
        Me.lblShow.AutoSize = True
        Me.lblShow.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lblShow.Location = New System.Drawing.Point(16, 28)
        Me.lblShow.Name = "lblShow"
        Me.lblShow.Size = New System.Drawing.Size(35, 15)
        Me.lblShow.TabIndex = 0
        Me.lblShow.Text = "Show"
        '
        'cboShow
        '
        Me.cboShow.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboShow.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.cboShow.Location = New System.Drawing.Point(78, 25)
        Me.cboShow.Name = "cboShow"
        Me.cboShow.Size = New System.Drawing.Size(180, 23)
        Me.cboShow.TabIndex = 1
        '
        'lblGridCount
        '
        Me.lblGridCount.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lblGridCount.Location = New System.Drawing.Point(660, 28)
        Me.lblGridCount.Name = "lblGridCount"
        Me.lblGridCount.Size = New System.Drawing.Size(360, 17)
        Me.lblGridCount.TabIndex = 2
        Me.lblGridCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'dgvScreenings
        '
        Me.dgvScreenings.AllowUserToAddRows = False
        Me.dgvScreenings.AllowUserToDeleteRows = False
        Me.dgvScreenings.AllowUserToResizeRows = False
        Me.dgvScreenings.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvScreenings.Location = New System.Drawing.Point(16, 58)
        Me.dgvScreenings.MultiSelect = False
        Me.dgvScreenings.Name = "dgvScreenings"
        Me.dgvScreenings.ReadOnly = True
        Me.dgvScreenings.RowHeadersVisible = False
        Me.dgvScreenings.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvScreenings.Size = New System.Drawing.Size(1004, 330)
        Me.dgvScreenings.TabIndex = 3
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.lblStatus)
        Me.GroupBox2.Controls.Add(Me.lblFilm)
        Me.GroupBox2.Controls.Add(Me.cboFilm)
        Me.GroupBox2.Controls.Add(Me.lblScreen)
        Me.GroupBox2.Controls.Add(Me.cboScreen)
        Me.GroupBox2.Controls.Add(Me.lblDate)
        Me.GroupBox2.Controls.Add(Me.dtpScreeningDate)
        Me.GroupBox2.Controls.Add(Me.lblTime)
        Me.GroupBox2.Controls.Add(Me.txtScreeningTime)
        Me.GroupBox2.Controls.Add(Me.lblPrice)
        Me.GroupBox2.Controls.Add(Me.txtTicketPrice)
        Me.GroupBox2.Controls.Add(Me.lblEndsAt)
        Me.GroupBox2.Controls.Add(Me.btnSuggest)
        Me.GroupBox2.Controls.Add(Me.btnAdd)
        Me.GroupBox2.Controls.Add(Me.btnUpdate)
        Me.GroupBox2.Controls.Add(Me.btnDelete)
        Me.GroupBox2.Controls.Add(Me.btnClear)
        Me.GroupBox2.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.GroupBox2.Location = New System.Drawing.Point(16, 456)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(1036, 200)
        Me.GroupBox2.TabIndex = 2
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Put a film on, or change a screening that is already there"
        '
        'lblStatus
        '
        Me.lblStatus.AutoSize = True
        Me.lblStatus.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lblStatus.Location = New System.Drawing.Point(16, 24)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(170, 19)
        Me.lblStatus.TabIndex = 0
        Me.lblStatus.Text = "Adding a new screening"
        '
        'lblFilm
        '
        Me.lblFilm.AutoSize = True
        Me.lblFilm.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lblFilm.Location = New System.Drawing.Point(16, 61)
        Me.lblFilm.Name = "lblFilm"
        Me.lblFilm.Size = New System.Drawing.Size(28, 15)
        Me.lblFilm.TabIndex = 1
        Me.lblFilm.Text = "Film"
        '
        'cboFilm
        '
        Me.cboFilm.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboFilm.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.cboFilm.Location = New System.Drawing.Point(90, 58)
        Me.cboFilm.Name = "cboFilm"
        Me.cboFilm.Size = New System.Drawing.Size(300, 23)
        Me.cboFilm.TabIndex = 2
        '
        'lblScreen
        '
        Me.lblScreen.AutoSize = True
        Me.lblScreen.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lblScreen.Location = New System.Drawing.Point(410, 61)
        Me.lblScreen.Name = "lblScreen"
        Me.lblScreen.Size = New System.Drawing.Size(45, 15)
        Me.lblScreen.TabIndex = 3
        Me.lblScreen.Text = "Screen"
        '
        'cboScreen
        '
        Me.cboScreen.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboScreen.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.cboScreen.Location = New System.Drawing.Point(490, 58)
        Me.cboScreen.Name = "cboScreen"
        Me.cboScreen.Size = New System.Drawing.Size(180, 23)
        Me.cboScreen.TabIndex = 4
        '
        'lblDate
        '
        Me.lblDate.AutoSize = True
        Me.lblDate.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lblDate.Location = New System.Drawing.Point(16, 99)
        Me.lblDate.Name = "lblDate"
        Me.lblDate.Size = New System.Drawing.Size(31, 15)
        Me.lblDate.TabIndex = 5
        Me.lblDate.Text = "Date"
        '
        'dtpScreeningDate
        '
        Me.dtpScreeningDate.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.dtpScreeningDate.Format = System.Windows.Forms.DateTimePickerFormat.Short
        Me.dtpScreeningDate.Location = New System.Drawing.Point(90, 95)
        Me.dtpScreeningDate.Name = "dtpScreeningDate"
        Me.dtpScreeningDate.Size = New System.Drawing.Size(140, 23)
        Me.dtpScreeningDate.TabIndex = 6
        '
        'lblTime
        '
        Me.lblTime.AutoSize = True
        Me.lblTime.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lblTime.Location = New System.Drawing.Point(250, 99)
        Me.lblTime.Name = "lblTime"
        Me.lblTime.Size = New System.Drawing.Size(80, 15)
        Me.lblTime.TabIndex = 7
        Me.lblTime.Text = "Starts (HH:MM)"
        '
        'txtScreeningTime
        '
        Me.txtScreeningTime.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.txtScreeningTime.Location = New System.Drawing.Point(385, 95)
        Me.txtScreeningTime.MaxLength = 5
        Me.txtScreeningTime.Name = "txtScreeningTime"
        Me.txtScreeningTime.Size = New System.Drawing.Size(70, 23)
        Me.txtScreeningTime.TabIndex = 8
        Me.txtScreeningTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lblPrice
        '
        Me.lblPrice.AutoSize = True
        Me.lblPrice.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lblPrice.Location = New System.Drawing.Point(475, 99)
        Me.lblPrice.Name = "lblPrice"
        Me.lblPrice.Size = New System.Drawing.Size(75, 15)
        Me.lblPrice.TabIndex = 9
        Me.lblPrice.Text = "Ticket price (£)"
        '
        'txtTicketPrice
        '
        Me.txtTicketPrice.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.txtTicketPrice.Location = New System.Drawing.Point(605, 95)
        Me.txtTicketPrice.MaxLength = 6
        Me.txtTicketPrice.Name = "txtTicketPrice"
        Me.txtTicketPrice.Size = New System.Drawing.Size(75, 23)
        Me.txtTicketPrice.TabIndex = 10
        Me.txtTicketPrice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblEndsAt
        '
        Me.lblEndsAt.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lblEndsAt.Location = New System.Drawing.Point(16, 131)
        Me.lblEndsAt.Name = "lblEndsAt"
        Me.lblEndsAt.Size = New System.Drawing.Size(500, 62)
        Me.lblEndsAt.TabIndex = 11
        Me.lblEndsAt.Text = "Pick a film and a start time to see when the screen would be free again"
        '
        'btnSuggest
        '
        Me.btnSuggest.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnSuggest.Location = New System.Drawing.Point(540, 136)
        Me.btnSuggest.Name = "btnSuggest"
        Me.btnSuggest.Size = New System.Drawing.Size(140, 32)
        Me.btnSuggest.TabIndex = 12
        Me.btnSuggest.Text = "Find me a free time"
        Me.btnSuggest.UseVisualStyleBackColor = True
        '
        'btnAdd
        '
        Me.btnAdd.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me.btnAdd.Location = New System.Drawing.Point(700, 58)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(320, 42)
        Me.btnAdd.TabIndex = 12
        Me.btnAdd.Text = "PUT THIS FILM ON"
        Me.btnAdd.UseVisualStyleBackColor = True
        '
        'btnUpdate
        '
        Me.btnUpdate.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnUpdate.Location = New System.Drawing.Point(700, 106)
        Me.btnUpdate.Name = "btnUpdate"
        Me.btnUpdate.Size = New System.Drawing.Size(155, 34)
        Me.btnUpdate.TabIndex = 13
        Me.btnUpdate.Text = "Save changes"
        Me.btnUpdate.UseVisualStyleBackColor = True
        '
        'btnDelete
        '
        Me.btnDelete.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnDelete.Location = New System.Drawing.Point(865, 106)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(155, 34)
        Me.btnDelete.TabIndex = 14
        Me.btnDelete.Text = "Delete screening"
        Me.btnDelete.UseVisualStyleBackColor = True
        '
        'btnClear
        '
        Me.btnClear.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnClear.Location = New System.Drawing.Point(700, 146)
        Me.btnClear.Name = "btnClear"
        Me.btnClear.Size = New System.Drawing.Size(320, 30)
        Me.btnClear.TabIndex = 15
        Me.btnClear.Text = "Clear the boxes"
        Me.btnClear.UseVisualStyleBackColor = True
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(910, 10)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(140, 28)
        Me.btnExport.Text = "Export to CSV"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'lblSaved
        '
        Me.lblSaved.AutoSize = False
        Me.lblSaved.Location = New System.Drawing.Point(360, 666)
        Me.lblSaved.Name = "lblSaved"
        Me.lblSaved.Size = New System.Drawing.Size(692, 16)
        Me.lblSaved.Text = ""
        '
        'lblVersion
        '
        Me.lblVersion.AutoSize = True
        Me.lblVersion.ForeColor = System.Drawing.Color.Gray
        Me.lblVersion.Location = New System.Drawing.Point(16, 666)
        Me.lblVersion.Name = "lblVersion"
        Me.lblVersion.Size = New System.Drawing.Size(271, 16)
        Me.lblVersion.TabIndex = 3
        Me.lblVersion.Text = "Filmtopia Cinema Management System  v1.0"
        '
        'frmScreenings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1068, 695)
        Me.Controls.Add(Me.btnExport)
        Me.Controls.Add(Me.lblSaved)
        Me.Controls.Add(Me.lblVersion)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.lblHeading)
        Me.Name = "frmScreenings"
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Text = "Screenings"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        CType(Me.dgvScreenings, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblHeading As Label
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents lblShow As Label
    Friend WithEvents cboShow As ComboBox
    Friend WithEvents lblGridCount As Label
    Friend WithEvents dgvScreenings As DataGridView
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents lblStatus As Label
    Friend WithEvents lblFilm As Label
    Friend WithEvents cboFilm As ComboBox
    Friend WithEvents lblScreen As Label
    Friend WithEvents cboScreen As ComboBox
    Friend WithEvents lblDate As Label
    Friend WithEvents dtpScreeningDate As DateTimePicker
    Friend WithEvents lblTime As Label
    Friend WithEvents txtScreeningTime As TextBox
    Friend WithEvents lblPrice As Label
    Friend WithEvents txtTicketPrice As TextBox
    Friend WithEvents lblEndsAt As Label
    Friend WithEvents btnSuggest As Button
    Friend WithEvents btnAdd As Button
    Friend WithEvents btnUpdate As Button
    Friend WithEvents btnDelete As Button
    Friend WithEvents btnClear As Button
    Friend WithEvents btnExport As Button
    Friend WithEvents lblSaved As Label
    Friend WithEvents lblVersion As Label
End Class
