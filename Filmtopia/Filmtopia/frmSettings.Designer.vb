<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmSettings
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
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.grpSettings = New System.Windows.Forms.GroupBox()
        Me.lblTurnaround = New System.Windows.Forms.Label()
        Me.nudTurnaround = New System.Windows.Forms.NumericUpDown()
        Me.lblTurnaroundUnit = New System.Windows.Forms.Label()
        Me.lblAdvert = New System.Windows.Forms.Label()
        Me.nudAdvert = New System.Windows.Forms.NumericUpDown()
        Me.lblAdvertUnit = New System.Windows.Forms.Label()
        Me.lblRounding = New System.Windows.Forms.Label()
        Me.nudRounding = New System.Windows.Forms.NumericUpDown()
        Me.lblRoundingUnit = New System.Windows.Forms.Label()
        Me.lblPrice = New System.Windows.Forms.Label()
        Me.lblPound = New System.Windows.Forms.Label()
        Me.nudPrice = New System.Windows.Forms.NumericUpDown()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.lblVersion = New System.Windows.Forms.Label()
        Me.grpSettings.SuspendLayout()
        CType(Me.nudTurnaround, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudAdvert, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudRounding, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudPrice, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblTitle
        '
        Me.lblTitle.AutoSize = True
        Me.lblTitle.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold)
        Me.lblTitle.Location = New System.Drawing.Point(12, 9)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(150, 20)
        Me.lblTitle.TabIndex = 0
        Me.lblTitle.Text = "System Settings"
        '
        'grpSettings
        '
        Me.grpSettings.Controls.Add(Me.lblTurnaround)
        Me.grpSettings.Controls.Add(Me.nudTurnaround)
        Me.grpSettings.Controls.Add(Me.lblTurnaroundUnit)
        Me.grpSettings.Controls.Add(Me.lblAdvert)
        Me.grpSettings.Controls.Add(Me.nudAdvert)
        Me.grpSettings.Controls.Add(Me.lblAdvertUnit)
        Me.grpSettings.Controls.Add(Me.lblRounding)
        Me.grpSettings.Controls.Add(Me.nudRounding)
        Me.grpSettings.Controls.Add(Me.lblRoundingUnit)
        Me.grpSettings.Controls.Add(Me.lblPrice)
        Me.grpSettings.Controls.Add(Me.lblPound)
        Me.grpSettings.Controls.Add(Me.nudPrice)
        Me.grpSettings.Location = New System.Drawing.Point(12, 40)
        Me.grpSettings.Name = "grpSettings"
        Me.grpSettings.Size = New System.Drawing.Size(446, 205)
        Me.grpSettings.TabIndex = 1
        Me.grpSettings.TabStop = False
        Me.grpSettings.Text = "Scheduling and pricing"
        '
        'lblTurnaround
        '
        Me.lblTurnaround.AutoSize = True
        Me.lblTurnaround.Location = New System.Drawing.Point(15, 37)
        Me.lblTurnaround.Name = "lblTurnaround"
        Me.lblTurnaround.Size = New System.Drawing.Size(141, 13)
        Me.lblTurnaround.TabIndex = 0
        Me.lblTurnaround.Text = "Cleaning / turnaround gap"
        '
        'nudTurnaround
        '
        Me.nudTurnaround.Location = New System.Drawing.Point(250, 35)
        Me.nudTurnaround.Maximum = New Decimal(New Integer() {240, 0, 0, 0})
        Me.nudTurnaround.Name = "nudTurnaround"
        Me.nudTurnaround.Size = New System.Drawing.Size(60, 20)
        Me.nudTurnaround.TabIndex = 1
        Me.nudTurnaround.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblTurnaroundUnit
        '
        Me.lblTurnaroundUnit.AutoSize = True
        Me.lblTurnaroundUnit.Location = New System.Drawing.Point(318, 37)
        Me.lblTurnaroundUnit.Name = "lblTurnaroundUnit"
        Me.lblTurnaroundUnit.Size = New System.Drawing.Size(44, 13)
        Me.lblTurnaroundUnit.TabIndex = 2
        Me.lblTurnaroundUnit.Text = "minutes"
        '
        'lblAdvert
        '
        Me.lblAdvert.AutoSize = True
        Me.lblAdvert.Location = New System.Drawing.Point(15, 77)
        Me.lblAdvert.Name = "lblAdvert"
        Me.lblAdvert.Size = New System.Drawing.Size(148, 13)
        Me.lblAdvert.TabIndex = 3
        Me.lblAdvert.Text = "Adverts and trailers before film"
        '
        'nudAdvert
        '
        Me.nudAdvert.Location = New System.Drawing.Point(250, 75)
        Me.nudAdvert.Maximum = New Decimal(New Integer() {120, 0, 0, 0})
        Me.nudAdvert.Name = "nudAdvert"
        Me.nudAdvert.Size = New System.Drawing.Size(60, 20)
        Me.nudAdvert.TabIndex = 4
        Me.nudAdvert.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblAdvertUnit
        '
        Me.lblAdvertUnit.AutoSize = True
        Me.lblAdvertUnit.Location = New System.Drawing.Point(318, 77)
        Me.lblAdvertUnit.Name = "lblAdvertUnit"
        Me.lblAdvertUnit.Size = New System.Drawing.Size(44, 13)
        Me.lblAdvertUnit.TabIndex = 5
        Me.lblAdvertUnit.Text = "minutes"
        '
        'lblRounding
        '
        Me.lblRounding.AutoSize = True
        Me.lblRounding.Location = New System.Drawing.Point(15, 117)
        Me.lblRounding.Name = "lblRounding"
        Me.lblRounding.Size = New System.Drawing.Size(126, 13)
        Me.lblRounding.TabIndex = 6
        Me.lblRounding.Text = "Round start times up to"
        '
        'nudRounding
        '
        Me.nudRounding.Location = New System.Drawing.Point(250, 115)
        Me.nudRounding.Maximum = New Decimal(New Integer() {60, 0, 0, 0})
        Me.nudRounding.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.nudRounding.Name = "nudRounding"
        Me.nudRounding.Size = New System.Drawing.Size(60, 20)
        Me.nudRounding.TabIndex = 7
        Me.nudRounding.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.nudRounding.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'lblRoundingUnit
        '
        Me.lblRoundingUnit.AutoSize = True
        Me.lblRoundingUnit.Location = New System.Drawing.Point(318, 117)
        Me.lblRoundingUnit.Name = "lblRoundingUnit"
        Me.lblRoundingUnit.Size = New System.Drawing.Size(44, 13)
        Me.lblRoundingUnit.TabIndex = 8
        Me.lblRoundingUnit.Text = "minutes"
        '
        'lblPrice
        '
        Me.lblPrice.AutoSize = True
        Me.lblPrice.Location = New System.Drawing.Point(15, 157)
        Me.lblPrice.Name = "lblPrice"
        Me.lblPrice.Size = New System.Drawing.Size(99, 13)
        Me.lblPrice.TabIndex = 9
        Me.lblPrice.Text = "Default ticket price"
        '
        'lblPound
        '
        Me.lblPound.AutoSize = True
        Me.lblPound.Location = New System.Drawing.Point(232, 157)
        Me.lblPound.Name = "lblPound"
        Me.lblPound.Size = New System.Drawing.Size(13, 13)
        Me.lblPound.TabIndex = 10
        Me.lblPound.Text = "£"
        '
        'nudPrice
        '
        Me.nudPrice.DecimalPlaces = 2
        Me.nudPrice.Increment = New Decimal(New Integer() {5, 0, 0, 65536})
        Me.nudPrice.Location = New System.Drawing.Point(250, 155)
        Me.nudPrice.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.nudPrice.Name = "nudPrice"
        Me.nudPrice.Size = New System.Drawing.Size(70, 20)
        Me.nudPrice.TabIndex = 11
        Me.nudPrice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(368, 262)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(90, 30)
        Me.btnSave.TabIndex = 2
        Me.btnSave.Text = "Save"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'lblVersion
        '
        Me.lblVersion.AutoSize = True
        Me.lblVersion.ForeColor = System.Drawing.Color.Gray
        Me.lblVersion.Location = New System.Drawing.Point(243, 305)
        Me.lblVersion.Name = "lblVersion"
        Me.lblVersion.Size = New System.Drawing.Size(215, 13)
        Me.lblVersion.TabIndex = 3
        Me.lblVersion.Text = "Filmtopia Cinema Management System  v1.0"
        '
        'frmSettings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(470, 330)
        Me.Controls.Add(Me.lblVersion)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.grpSettings)
        Me.Controls.Add(Me.lblTitle)
        Me.Name = "frmSettings"
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Text = "Settings"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.grpSettings.ResumeLayout(False)
        Me.grpSettings.PerformLayout()
        CType(Me.nudTurnaround, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudAdvert, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudRounding, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudPrice, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblTitle As Label
    Friend WithEvents grpSettings As GroupBox
    Friend WithEvents lblTurnaround As Label
    Friend WithEvents nudTurnaround As NumericUpDown
    Friend WithEvents lblTurnaroundUnit As Label
    Friend WithEvents lblAdvert As Label
    Friend WithEvents nudAdvert As NumericUpDown
    Friend WithEvents lblAdvertUnit As Label
    Friend WithEvents lblRounding As Label
    Friend WithEvents nudRounding As NumericUpDown
    Friend WithEvents lblRoundingUnit As Label
    Friend WithEvents lblPrice As Label
    Friend WithEvents lblPound As Label
    Friend WithEvents nudPrice As NumericUpDown
    Friend WithEvents btnSave As Button
    Friend WithEvents lblVersion As Label
End Class
