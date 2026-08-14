<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmImportFilms
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
        Me.lblFileHint = New System.Windows.Forms.Label()
        Me.txtFilePath = New System.Windows.Forms.TextBox()
        Me.btnBrowse = New System.Windows.Forms.Button()
        Me.lblFileInfo = New System.Windows.Forms.Label()
        Me.lblDescHint = New System.Windows.Forms.Label()
        Me.txtDescFilePath = New System.Windows.Forms.TextBox()
        Me.btnBrowseDesc = New System.Windows.Forms.Button()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.lblSearchTitle = New System.Windows.Forms.Label()
        Me.txtSearchTitle = New System.Windows.Forms.TextBox()
        Me.lblYearFrom = New System.Windows.Forms.Label()
        Me.txtYearFrom = New System.Windows.Forms.TextBox()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.lblSearchInfo = New System.Windows.Forms.Label()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.dgvMatches = New System.Windows.Forms.DataGridView()
        Me.lblMatchCount = New System.Windows.Forms.Label()
        Me.btnImport = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.lblVersion = New System.Windows.Forms.Label()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        CType(Me.dgvMatches, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblHeading
        '
        Me.lblHeading.AutoSize = True
        Me.lblHeading.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Bold)
        Me.lblHeading.Location = New System.Drawing.Point(16, 12)
        Me.lblHeading.Name = "lblHeading"
        Me.lblHeading.Size = New System.Drawing.Size(340, 25)
        Me.lblHeading.TabIndex = 0
        Me.lblHeading.Text = "Import films from an IMDb data file"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.lblFileHint)
        Me.GroupBox1.Controls.Add(Me.txtFilePath)
        Me.GroupBox1.Controls.Add(Me.btnBrowse)
        Me.GroupBox1.Controls.Add(Me.lblFileInfo)
        Me.GroupBox1.Controls.Add(Me.lblDescHint)
        Me.GroupBox1.Controls.Add(Me.txtDescFilePath)
        Me.GroupBox1.Controls.Add(Me.btnBrowseDesc)
        Me.GroupBox1.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.GroupBox1.Location = New System.Drawing.Point(16, 46)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(1036, 180)
        Me.GroupBox1.TabIndex = 1
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "The data file"
        '
        'lblFileHint
        '
        Me.lblFileHint.AutoSize = True
        Me.lblFileHint.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lblFileHint.Location = New System.Drawing.Point(16, 24)
        Me.lblFileHint.Name = "lblFileHint"
        Me.lblFileHint.Size = New System.Drawing.Size(420, 15)
        Me.lblFileHint.TabIndex = 0
        Me.lblFileHint.Text = "Choose the title.basics.tsv file that was downloaded from IMDb"
        '
        'txtFilePath
        '
        Me.txtFilePath.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.txtFilePath.Location = New System.Drawing.Point(16, 50)
        Me.txtFilePath.Name = "txtFilePath"
        Me.txtFilePath.ReadOnly = True
        Me.txtFilePath.Size = New System.Drawing.Size(800, 23)
        Me.txtFilePath.TabIndex = 1
        '
        'btnBrowse
        '
        Me.btnBrowse.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnBrowse.Location = New System.Drawing.Point(830, 49)
        Me.btnBrowse.Name = "btnBrowse"
        Me.btnBrowse.Size = New System.Drawing.Size(190, 26)
        Me.btnBrowse.TabIndex = 2
        Me.btnBrowse.Text = "Choose file..."
        Me.btnBrowse.UseVisualStyleBackColor = True
        '
        'lblFileInfo
        '
        Me.lblFileInfo.AutoSize = True
        Me.lblFileInfo.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lblFileInfo.ForeColor = System.Drawing.Color.Gray
        Me.lblFileInfo.Location = New System.Drawing.Point(16, 84)
        Me.lblFileInfo.Name = "lblFileInfo"
        Me.lblFileInfo.Size = New System.Drawing.Size(150, 15)
        Me.lblFileInfo.TabIndex = 3
        Me.lblFileInfo.Text = "No file chosen yet"
        '
        'lblDescHint
        '
        Me.lblDescHint.AutoSize = True
        Me.lblDescHint.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lblDescHint.Location = New System.Drawing.Point(16, 112)
        Me.lblDescHint.Name = "lblDescHint"
        Me.lblDescHint.Size = New System.Drawing.Size(500, 15)
        Me.lblDescHint.TabIndex = 4
        Me.lblDescHint.Text = "Descriptions file (optional) - fills in what each film is about. Leave it empty to import without descriptions"
        '
        'txtDescFilePath
        '
        Me.txtDescFilePath.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.txtDescFilePath.Location = New System.Drawing.Point(16, 138)
        Me.txtDescFilePath.Name = "txtDescFilePath"
        Me.txtDescFilePath.ReadOnly = True
        Me.txtDescFilePath.Size = New System.Drawing.Size(800, 23)
        Me.txtDescFilePath.TabIndex = 5
        '
        'btnBrowseDesc
        '
        Me.btnBrowseDesc.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnBrowseDesc.Location = New System.Drawing.Point(830, 137)
        Me.btnBrowseDesc.Name = "btnBrowseDesc"
        Me.btnBrowseDesc.Size = New System.Drawing.Size(190, 26)
        Me.btnBrowseDesc.TabIndex = 6
        Me.btnBrowseDesc.Text = "Choose descriptions..."
        Me.btnBrowseDesc.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.lblSearchTitle)
        Me.GroupBox2.Controls.Add(Me.txtSearchTitle)
        Me.GroupBox2.Controls.Add(Me.lblYearFrom)
        Me.GroupBox2.Controls.Add(Me.txtYearFrom)
        Me.GroupBox2.Controls.Add(Me.btnSearch)
        Me.GroupBox2.Controls.Add(Me.lblSearchInfo)
        Me.GroupBox2.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.GroupBox2.Location = New System.Drawing.Point(16, 236)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(1036, 90)
        Me.GroupBox2.TabIndex = 2
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Search the file"
        '
        'lblSearchTitle
        '
        Me.lblSearchTitle.AutoSize = True
        Me.lblSearchTitle.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lblSearchTitle.Location = New System.Drawing.Point(16, 28)
        Me.lblSearchTitle.Name = "lblSearchTitle"
        Me.lblSearchTitle.Size = New System.Drawing.Size(85, 15)
        Me.lblSearchTitle.TabIndex = 0
        Me.lblSearchTitle.Text = "Title contains"
        '
        'txtSearchTitle
        '
        Me.txtSearchTitle.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.txtSearchTitle.Location = New System.Drawing.Point(135, 25)
        Me.txtSearchTitle.Name = "txtSearchTitle"
        Me.txtSearchTitle.Size = New System.Drawing.Size(300, 23)
        Me.txtSearchTitle.TabIndex = 1
        '
        'lblYearFrom
        '
        Me.lblYearFrom.AutoSize = True
        Me.lblYearFrom.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lblYearFrom.Location = New System.Drawing.Point(462, 28)
        Me.lblYearFrom.Name = "lblYearFrom"
        Me.lblYearFrom.Size = New System.Drawing.Size(62, 15)
        Me.lblYearFrom.TabIndex = 2
        Me.lblYearFrom.Text = "Year from"
        '
        'txtYearFrom
        '
        Me.txtYearFrom.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.txtYearFrom.Location = New System.Drawing.Point(556, 25)
        Me.txtYearFrom.Name = "txtYearFrom"
        Me.txtYearFrom.Size = New System.Drawing.Size(70, 23)
        Me.txtYearFrom.TabIndex = 3
        '
        'btnSearch
        '
        Me.btnSearch.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnSearch.Location = New System.Drawing.Point(660, 24)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(180, 26)
        Me.btnSearch.TabIndex = 4
        Me.btnSearch.Text = "Search the file"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'lblSearchInfo
        '
        Me.lblSearchInfo.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lblSearchInfo.ForeColor = System.Drawing.Color.Gray
        Me.lblSearchInfo.Location = New System.Drawing.Point(16, 60)
        Me.lblSearchInfo.Name = "lblSearchInfo"
        Me.lblSearchInfo.Size = New System.Drawing.Size(1004, 17)
        Me.lblSearchInfo.TabIndex = 5
        Me.lblSearchInfo.Text = "The file is very big, so only films matching the search are read out of it"
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.dgvMatches)
        Me.GroupBox3.Controls.Add(Me.lblMatchCount)
        Me.GroupBox3.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.GroupBox3.Location = New System.Drawing.Point(16, 336)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(1036, 330)
        Me.GroupBox3.TabIndex = 3
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Films found in the file, tick the ones to bring in"
        '
        'dgvMatches
        '
        Me.dgvMatches.AllowUserToAddRows = False
        Me.dgvMatches.AllowUserToDeleteRows = False
        Me.dgvMatches.AllowUserToResizeRows = False
        Me.dgvMatches.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvMatches.Location = New System.Drawing.Point(16, 25)
        Me.dgvMatches.MultiSelect = False
        Me.dgvMatches.Name = "dgvMatches"
        Me.dgvMatches.RowHeadersVisible = False
        Me.dgvMatches.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.dgvMatches.Size = New System.Drawing.Size(1004, 268)
        Me.dgvMatches.TabIndex = 0
        '
        'lblMatchCount
        '
        Me.lblMatchCount.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lblMatchCount.Location = New System.Drawing.Point(16, 300)
        Me.lblMatchCount.Name = "lblMatchCount"
        Me.lblMatchCount.Size = New System.Drawing.Size(1004, 17)
        Me.lblMatchCount.TabIndex = 1
        Me.lblMatchCount.Text = "Nothing searched for yet"
        '
        'btnImport
        '
        Me.btnImport.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me.btnImport.Location = New System.Drawing.Point(16, 678)
        Me.btnImport.Name = "btnImport"
        Me.btnImport.Size = New System.Drawing.Size(520, 42)
        Me.btnImport.TabIndex = 4
        Me.btnImport.Text = "IMPORT TICKED FILMS"
        Me.btnImport.UseVisualStyleBackColor = True
        '
        'btnClose
        '
        Me.btnClose.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnClose.Location = New System.Drawing.Point(546, 678)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(250, 42)
        Me.btnClose.TabIndex = 5
        Me.btnClose.Text = "Close"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'lblVersion
        '
        Me.lblVersion.AutoSize = True
        Me.lblVersion.ForeColor = System.Drawing.Color.Gray
        Me.lblVersion.Location = New System.Drawing.Point(16, 774)
        Me.lblVersion.Name = "lblVersion"
        Me.lblVersion.Size = New System.Drawing.Size(271, 16)
        Me.lblVersion.TabIndex = 6
        Me.lblVersion.Text = "Filmtopia Cinema Management System  v1.0"
        '
        'frmImportFilms
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1068, 800)
        Me.Controls.Add(Me.lblVersion)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnImport)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.lblHeading)
        Me.AcceptButton = Me.btnSearch
        Me.CancelButton = Me.btnClose
        Me.Name = "frmImportFilms"
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Text = "Import films"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        CType(Me.dgvMatches, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblHeading As Label
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents lblFileHint As Label
    Friend WithEvents txtFilePath As TextBox
    Friend WithEvents btnBrowse As Button
    Friend WithEvents lblFileInfo As Label
    Friend WithEvents lblDescHint As Label
    Friend WithEvents txtDescFilePath As TextBox
    Friend WithEvents btnBrowseDesc As Button
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents lblSearchTitle As Label
    Friend WithEvents txtSearchTitle As TextBox
    Friend WithEvents lblYearFrom As Label
    Friend WithEvents txtYearFrom As TextBox
    Friend WithEvents btnSearch As Button
    Friend WithEvents lblSearchInfo As Label
    Friend WithEvents GroupBox3 As GroupBox
    Friend WithEvents dgvMatches As DataGridView
    Friend WithEvents lblMatchCount As Label
    Friend WithEvents btnImport As Button
    Friend WithEvents btnClose As Button
    Friend WithEvents lblVersion As Label
End Class
