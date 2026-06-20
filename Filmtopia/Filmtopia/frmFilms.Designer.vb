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
        Me.dgvFilms = New System.Windows.Forms.DataGridView()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.txtTitle = New System.Windows.Forms.TextBox()
        Me.lblAgeRating = New System.Windows.Forms.Label()
        Me.txtAgeRating = New System.Windows.Forms.TextBox()
        Me.lblDuration = New System.Windows.Forms.Label()
        Me.txtDuration = New System.Windows.Forms.TextBox()
        Me.lblDescription = New System.Windows.Forms.Label()
        Me.txtDescription = New System.Windows.Forms.TextBox()
        Me.btnAdd = New System.Windows.Forms.Button()
        Me.btnUpdate = New System.Windows.Forms.Button()
        Me.btnDelete = New System.Windows.Forms.Button()
        Me.btnClear = New System.Windows.Forms.Button()
        Me.lblVersion = New System.Windows.Forms.Label()
        CType(Me.dgvFilms, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'dgvFilms
        '
        Me.dgvFilms.AllowUserToAddRows = False
        Me.dgvFilms.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvFilms.Location = New System.Drawing.Point(16, 15)
        Me.dgvFilms.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.dgvFilms.MultiSelect = False
        Me.dgvFilms.Name = "dgvFilms"
        Me.dgvFilms.RowHeadersWidth = 51
        Me.dgvFilms.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvFilms.Size = New System.Drawing.Size(1038, 369)
        Me.dgvFilms.TabIndex = 0
        '
        'lblTitle
        '
        Me.lblTitle.AutoSize = True
        Me.lblTitle.Location = New System.Drawing.Point(16, 406)
        Me.lblTitle.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(33, 16)
        Me.lblTitle.TabIndex = 1
        Me.lblTitle.Text = "Title"
        '
        'txtTitle
        '
        Me.txtTitle.Location = New System.Drawing.Point(160, 402)
        Me.txtTitle.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.txtTitle.Name = "txtTitle"
        Me.txtTitle.Size = New System.Drawing.Size(332, 22)
        Me.txtTitle.TabIndex = 2
        '
        'lblAgeRating
        '
        Me.lblAgeRating.AutoSize = True
        Me.lblAgeRating.Location = New System.Drawing.Point(16, 443)
        Me.lblAgeRating.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblAgeRating.Name = "lblAgeRating"
        Me.lblAgeRating.Size = New System.Drawing.Size(74, 16)
        Me.lblAgeRating.TabIndex = 3
        Me.lblAgeRating.Text = "Age Rating"
        '
        'txtAgeRating
        '
        Me.txtAgeRating.Location = New System.Drawing.Point(160, 439)
        Me.txtAgeRating.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.txtAgeRating.Name = "txtAgeRating"
        Me.txtAgeRating.Size = New System.Drawing.Size(132, 22)
        Me.txtAgeRating.TabIndex = 4
        '
        'lblDuration
        '
        Me.lblDuration.AutoSize = True
        Me.lblDuration.Location = New System.Drawing.Point(16, 480)
        Me.lblDuration.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblDuration.Name = "lblDuration"
        Me.lblDuration.Size = New System.Drawing.Size(96, 16)
        Me.lblDuration.TabIndex = 5
        Me.lblDuration.Text = "Duration (mins)"
        '
        'txtDuration
        '
        Me.txtDuration.Location = New System.Drawing.Point(160, 476)
        Me.txtDuration.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.txtDuration.Name = "txtDuration"
        Me.txtDuration.Size = New System.Drawing.Size(132, 22)
        Me.txtDuration.TabIndex = 6
        '
        'lblDescription
        '
        Me.lblDescription.AutoSize = True
        Me.lblDescription.Location = New System.Drawing.Point(16, 517)
        Me.lblDescription.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblDescription.Name = "lblDescription"
        Me.lblDescription.Size = New System.Drawing.Size(75, 16)
        Me.lblDescription.TabIndex = 7
        Me.lblDescription.Text = "Description"
        '
        'txtDescription
        '
        Me.txtDescription.Location = New System.Drawing.Point(160, 513)
        Me.txtDescription.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.txtDescription.Multiline = True
        Me.txtDescription.Name = "txtDescription"
        Me.txtDescription.Size = New System.Drawing.Size(532, 73)
        Me.txtDescription.TabIndex = 8
        '
        'btnAdd
        '
        Me.btnAdd.Location = New System.Drawing.Point(600, 402)
        Me.btnAdd.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(120, 37)
        Me.btnAdd.TabIndex = 9
        Me.btnAdd.Text = "Add"
        Me.btnAdd.UseVisualStyleBackColor = True
        '
        'btnUpdate
        '
        Me.btnUpdate.Location = New System.Drawing.Point(728, 402)
        Me.btnUpdate.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnUpdate.Name = "btnUpdate"
        Me.btnUpdate.Size = New System.Drawing.Size(120, 37)
        Me.btnUpdate.TabIndex = 10
        Me.btnUpdate.Text = "Update"
        Me.btnUpdate.UseVisualStyleBackColor = True
        '
        'btnDelete
        '
        Me.btnDelete.Location = New System.Drawing.Point(600, 447)
        Me.btnDelete.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(120, 37)
        Me.btnDelete.TabIndex = 11
        Me.btnDelete.Text = "Delete"
        Me.btnDelete.UseVisualStyleBackColor = True
        '
        'btnClear
        '
        Me.btnClear.Location = New System.Drawing.Point(728, 447)
        Me.btnClear.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnClear.Name = "btnClear"
        Me.btnClear.Size = New System.Drawing.Size(120, 37)
        Me.btnClear.TabIndex = 12
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
        Me.lblVersion.TabIndex = 13
        Me.lblVersion.Text = "Filmtopia Cinema Management System  v1.0"
        '
        'frmFilms
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1067, 640)
        Me.Controls.Add(Me.lblVersion)
        Me.Controls.Add(Me.btnClear)
        Me.Controls.Add(Me.btnDelete)
        Me.Controls.Add(Me.btnUpdate)
        Me.Controls.Add(Me.btnAdd)
        Me.Controls.Add(Me.txtDescription)
        Me.Controls.Add(Me.lblDescription)
        Me.Controls.Add(Me.txtDuration)
        Me.Controls.Add(Me.lblDuration)
        Me.Controls.Add(Me.txtAgeRating)
        Me.Controls.Add(Me.lblAgeRating)
        Me.Controls.Add(Me.txtTitle)
        Me.Controls.Add(Me.lblTitle)
        Me.Controls.Add(Me.dgvFilms)
        Me.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.Name = "frmFilms"
        Me.Text = "Films"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        CType(Me.dgvFilms, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents dgvFilms As DataGridView
    Friend WithEvents lblTitle As Label
    Friend WithEvents txtTitle As TextBox
    Friend WithEvents lblAgeRating As Label
    Friend WithEvents txtAgeRating As TextBox
    Friend WithEvents lblDuration As Label
    Friend WithEvents txtDuration As TextBox
    Friend WithEvents lblDescription As Label
    Friend WithEvents txtDescription As TextBox
    Friend WithEvents btnAdd As Button
    Friend WithEvents btnUpdate As Button
    Friend WithEvents btnDelete As Button
    Friend WithEvents btnClear As Button
    Friend WithEvents lblVersion As Label
End Class
