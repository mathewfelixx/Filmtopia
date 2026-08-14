<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmFilms
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
        Me.lblGridCount = New System.Windows.Forms.Label()
        Me.dgvFilms = New System.Windows.Forms.DataGridView()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.txtTitle = New System.Windows.Forms.TextBox()
        Me.lblAgeRating = New System.Windows.Forms.Label()
        Me.cboAgeRating = New System.Windows.Forms.ComboBox()
        Me.lblDuration = New System.Windows.Forms.Label()
        Me.txtDuration = New System.Windows.Forms.TextBox()
        Me.lblYear = New System.Windows.Forms.Label()
        Me.txtYear = New System.Windows.Forms.TextBox()
        Me.lblDescription = New System.Windows.Forms.Label()
        Me.txtDescription = New System.Windows.Forms.TextBox()
        Me.lblGenres = New System.Windows.Forms.Label()
        Me.txtGenres = New System.Windows.Forms.TextBox()
        Me.btnAdd = New System.Windows.Forms.Button()
        Me.btnUpdate = New System.Windows.Forms.Button()
        Me.btnDelete = New System.Windows.Forms.Button()
        Me.btnClear = New System.Windows.Forms.Button()
        Me.lblVersion = New System.Windows.Forms.Label()
        Me.GroupBox1.SuspendLayout()
        CType(Me.dgvFilms, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblHeading
        '
        Me.lblHeading.AutoSize = True
        Me.lblHeading.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Bold)
        Me.lblHeading.Location = New System.Drawing.Point(16, 12)
        Me.lblHeading.Name = "lblHeading"
        Me.lblHeading.Size = New System.Drawing.Size(60, 25)
        Me.lblHeading.TabIndex = 0
        Me.lblHeading.Text = "Films"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.lblSearch)
        Me.GroupBox1.Controls.Add(Me.txtSearch)
        Me.GroupBox1.Controls.Add(Me.lblGridCount)
        Me.GroupBox1.Controls.Add(Me.dgvFilms)
        Me.GroupBox1.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.GroupBox1.Location = New System.Drawing.Point(16, 46)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(1036, 400)
        Me.GroupBox1.TabIndex = 1
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "The films on the system"
        '
        'lblSearch
        '
        Me.lblSearch.AutoSize = True
        Me.lblSearch.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lblSearch.Location = New System.Drawing.Point(16, 28)
        Me.lblSearch.Name = "lblSearch"
        Me.lblSearch.Size = New System.Drawing.Size(42, 15)
        Me.lblSearch.TabIndex = 0
        Me.lblSearch.Text = "Search"
        '
        'txtSearch
        '
        Me.txtSearch.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.txtSearch.Location = New System.Drawing.Point(70, 25)
        Me.txtSearch.Name = "txtSearch"
        Me.txtSearch.Size = New System.Drawing.Size(300, 23)
        Me.txtSearch.TabIndex = 1
        '
        'lblGridCount
        '
        Me.lblGridCount.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lblGridCount.Location = New System.Drawing.Point(700, 28)
        Me.lblGridCount.Name = "lblGridCount"
        Me.lblGridCount.Size = New System.Drawing.Size(320, 17)
        Me.lblGridCount.TabIndex = 2
        Me.lblGridCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'dgvFilms
        '
        Me.dgvFilms.AllowUserToAddRows = False
        Me.dgvFilms.AllowUserToDeleteRows = False
        Me.dgvFilms.AllowUserToResizeRows = False
        Me.dgvFilms.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvFilms.Location = New System.Drawing.Point(16, 58)
        Me.dgvFilms.MultiSelect = False
        Me.dgvFilms.Name = "dgvFilms"
        Me.dgvFilms.ReadOnly = True
        Me.dgvFilms.RowHeadersVisible = False
        Me.dgvFilms.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvFilms.Size = New System.Drawing.Size(1004, 330)
        Me.dgvFilms.TabIndex = 3
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.lblStatus)
        Me.GroupBox2.Controls.Add(Me.lblTitle)
        Me.GroupBox2.Controls.Add(Me.txtTitle)
        Me.GroupBox2.Controls.Add(Me.lblAgeRating)
        Me.GroupBox2.Controls.Add(Me.cboAgeRating)
        Me.GroupBox2.Controls.Add(Me.lblDuration)
        Me.GroupBox2.Controls.Add(Me.txtDuration)
        Me.GroupBox2.Controls.Add(Me.lblDescription)
        Me.GroupBox2.Controls.Add(Me.txtDescription)
        Me.GroupBox2.Controls.Add(Me.btnAdd)
        Me.GroupBox2.Controls.Add(Me.btnUpdate)
        Me.GroupBox2.Controls.Add(Me.btnDelete)
        Me.GroupBox2.Controls.Add(Me.btnClear)
        Me.GroupBox2.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.GroupBox2.Location = New System.Drawing.Point(16, 456)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(1036, 220)
        Me.GroupBox2.TabIndex = 2
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Add a film or change one that is already there"
        '
        'lblStatus
        '
        Me.lblStatus.AutoSize = True
        Me.lblStatus.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lblStatus.Location = New System.Drawing.Point(16, 24)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(160, 23)
        Me.lblStatus.TabIndex = 0
        Me.lblStatus.Text = "Adding a new film"
        '
        'lblTitle
        '
        Me.lblTitle.AutoSize = True
        Me.lblTitle.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lblTitle.Location = New System.Drawing.Point(16, 58)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(38, 20)
        Me.lblTitle.TabIndex = 1
        Me.lblTitle.Text = "Title"
        '
        'txtTitle
        '
        Me.txtTitle.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.txtTitle.Location = New System.Drawing.Point(125, 55)
        Me.txtTitle.Name = "txtTitle"
        Me.txtTitle.Size = New System.Drawing.Size(285, 27)
        Me.txtTitle.TabIndex = 2
        '
        'lblAgeRating
        '
        Me.lblAgeRating.AutoSize = True
        Me.lblAgeRating.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lblAgeRating.Location = New System.Drawing.Point(432, 58)
        Me.lblAgeRating.Name = "lblAgeRating"
        Me.lblAgeRating.Size = New System.Drawing.Size(79, 20)
        Me.lblAgeRating.TabIndex = 3
        Me.lblAgeRating.Text = "Age rating"
        '
        'cboAgeRating
        '
        Me.cboAgeRating.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.cboAgeRating.Location = New System.Drawing.Point(526, 55)
        Me.cboAgeRating.Name = "cboAgeRating"
        Me.cboAgeRating.Size = New System.Drawing.Size(90, 28)
        Me.cboAgeRating.TabIndex = 4
        '
        'lblDuration
        '
        Me.lblDuration.AutoSize = True
        Me.lblDuration.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lblDuration.Location = New System.Drawing.Point(638, 58)
        Me.lblDuration.Name = "lblDuration"
        Me.lblDuration.Size = New System.Drawing.Size(112, 20)
        Me.lblDuration.TabIndex = 5
        Me.lblDuration.Text = "Duration (mins)"
        '
        'txtDuration
        '
        Me.txtDuration.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.txtDuration.Location = New System.Drawing.Point(768, 55)
        Me.txtDuration.Name = "txtDuration"
        Me.txtDuration.Size = New System.Drawing.Size(70, 27)
        Me.txtDuration.TabIndex = 6
        '
        'lblYear
        '
        Me.lblYear.AutoSize = True
        Me.lblYear.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lblYear.Location = New System.Drawing.Point(858, 58)
        Me.lblYear.Name = "lblYear"
        Me.lblYear.Size = New System.Drawing.Size(37, 20)
        Me.lblYear.TabIndex = 7
        Me.lblYear.Text = "Year"
        '
        'txtYear
        '
        Me.txtYear.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.txtYear.Location = New System.Drawing.Point(910, 55)
        Me.txtYear.Name = "txtYear"
        Me.txtYear.Size = New System.Drawing.Size(70, 27)
        Me.txtYear.TabIndex = 8
        '
        'lblDescription
        '
        Me.lblDescription.AutoSize = True
        Me.lblDescription.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lblDescription.Location = New System.Drawing.Point(16, 92)
        Me.lblDescription.Name = "lblDescription"
        Me.lblDescription.Size = New System.Drawing.Size(85, 20)
        Me.lblDescription.TabIndex = 7
        Me.lblDescription.Text = "Description"
        '
        'txtDescription
        '
        Me.txtDescription.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.txtDescription.Location = New System.Drawing.Point(125, 89)
        Me.txtDescription.Multiline = True
        Me.txtDescription.Name = "txtDescription"
        Me.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtDescription.Size = New System.Drawing.Size(560, 70)
        Me.txtDescription.TabIndex = 10
        '
        'lblGenres
        '
        Me.lblGenres.AutoSize = True
        Me.lblGenres.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lblGenres.Location = New System.Drawing.Point(16, 170)
        Me.lblGenres.Name = "lblGenres"
        Me.lblGenres.Size = New System.Drawing.Size(54, 20)
        Me.lblGenres.TabIndex = 11
        Me.lblGenres.Text = "Genres"
        '
        'txtGenres
        '
        Me.txtGenres.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.txtGenres.Location = New System.Drawing.Point(125, 167)
        Me.txtGenres.Name = "txtGenres"
        Me.txtGenres.Size = New System.Drawing.Size(560, 27)
        Me.txtGenres.TabIndex = 12
        '
        'btnAdd
        '
        Me.btnAdd.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me.btnAdd.Location = New System.Drawing.Point(700, 89)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(320, 42)
        Me.btnAdd.TabIndex = 13
        Me.btnAdd.Text = "ADD THIS FILM"
        Me.btnAdd.UseVisualStyleBackColor = True
        '
        'btnUpdate
        '
        Me.btnUpdate.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnUpdate.Location = New System.Drawing.Point(700, 137)
        Me.btnUpdate.Name = "btnUpdate"
        Me.btnUpdate.Size = New System.Drawing.Size(155, 34)
        Me.btnUpdate.TabIndex = 14
        Me.btnUpdate.Text = "Save changes"
        Me.btnUpdate.UseVisualStyleBackColor = True
        '
        'btnDelete
        '
        Me.btnDelete.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnDelete.Location = New System.Drawing.Point(865, 137)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(155, 34)
        Me.btnDelete.TabIndex = 15
        Me.btnDelete.Text = "Delete film"
        Me.btnDelete.UseVisualStyleBackColor = True
        '
        'btnClear
        '
        Me.btnClear.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnClear.Location = New System.Drawing.Point(700, 177)
        Me.btnClear.Name = "btnClear"
        Me.btnClear.Size = New System.Drawing.Size(320, 30)
        Me.btnClear.TabIndex = 16
        Me.btnClear.Text = "Clear the boxes"
        Me.btnClear.UseVisualStyleBackColor = True
        '
        'lblVersion
        '
        Me.lblVersion.AutoSize = True
        Me.lblVersion.ForeColor = System.Drawing.Color.Gray
        Me.lblVersion.Location = New System.Drawing.Point(16, 684)
        Me.lblVersion.Name = "lblVersion"
        Me.lblVersion.Size = New System.Drawing.Size(271, 16)
        Me.lblVersion.TabIndex = 3
        Me.lblVersion.Text = "Filmtopia Cinema Management System  v1.0"
        '
        'frmFilms
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1068, 710)
        Me.Controls.Add(Me.lblVersion)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.btnImportFromFile)
        Me.Controls.Add(Me.lblHeading)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "frmFilms"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Films"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.dgvFilms, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblHeading As Label
    Friend WithEvents btnImportFromFile As Button
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents lblSearch As Label
    Friend WithEvents txtSearch As TextBox
    Friend WithEvents lblGenreFilter As Label
    Friend WithEvents cboGenreFilter As ComboBox
    Friend WithEvents chkNeedsDescription As CheckBox
    Friend WithEvents lblGridCount As Label
    Friend WithEvents dgvFilms As DataGridView
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents lblStatus As Label
    Friend WithEvents lblTitle As Label
    Friend WithEvents txtTitle As TextBox
    Friend WithEvents lblAgeRating As Label
    Friend WithEvents cboAgeRating As ComboBox
    Friend WithEvents lblDuration As Label
    Friend WithEvents txtDuration As TextBox
    Friend WithEvents lblYear As Label
    Friend WithEvents txtYear As TextBox
    Friend WithEvents lblDescription As Label
    Friend WithEvents txtDescription As TextBox
    Friend WithEvents lblGenres As Label
    Friend WithEvents txtGenres As TextBox
    Friend WithEvents btnAdd As Button
    Friend WithEvents btnUpdate As Button
    Friend WithEvents btnDelete As Button
    Friend WithEvents btnClear As Button
    Friend WithEvents lblVersion As Label
End Class
