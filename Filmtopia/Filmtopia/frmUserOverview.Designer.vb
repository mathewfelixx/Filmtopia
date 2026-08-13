<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmUserOverview
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
        Me.lblHeading = New System.Windows.Forms.Label()
        Me.lblWelcome = New System.Windows.Forms.Label()
        Me.lblRole = New System.Windows.Forms.Label()
        Me.lblSubLastLogin = New System.Windows.Forms.Label()
        Me.lblSubSession = New System.Windows.Forms.Label()
        Me.pnlCard1 = New System.Windows.Forms.Panel()
        Me.lblCardTitle1 = New System.Windows.Forms.Label()
        Me.lblStat1 = New System.Windows.Forms.Label()
        Me.lblCardSub1 = New System.Windows.Forms.Label()
        Me.pnlAccent1 = New System.Windows.Forms.Panel()
        Me.pnlCard2 = New System.Windows.Forms.Panel()
        Me.lblCardTitle2 = New System.Windows.Forms.Label()
        Me.lblStat2 = New System.Windows.Forms.Label()
        Me.lblCardSub2 = New System.Windows.Forms.Label()
        Me.pnlAccent2 = New System.Windows.Forms.Panel()
        Me.pnlCard3 = New System.Windows.Forms.Panel()
        Me.lblCardTitle3 = New System.Windows.Forms.Label()
        Me.lblStat3 = New System.Windows.Forms.Label()
        Me.lblCardSub3 = New System.Windows.Forms.Label()
        Me.pnlAccent3 = New System.Windows.Forms.Panel()
        Me.pnlCard4 = New System.Windows.Forms.Panel()
        Me.lblCardTitle4 = New System.Windows.Forms.Label()
        Me.lblStat4 = New System.Windows.Forms.Label()
        Me.lblCardSub4 = New System.Windows.Forms.Label()
        Me.pnlAccent4 = New System.Windows.Forms.Panel()
        Me.tabMe = New System.Windows.Forms.TabControl()
        Me.tabActivity = New System.Windows.Forms.TabPage()
        Me.tabSales = New System.Windows.Forms.TabPage()
        Me.tabPattern = New System.Windows.Forms.TabPage()
        Me.tabSettings = New System.Windows.Forms.TabPage()
        Me.lblVersion = New System.Windows.Forms.Label()
        Me.pnlCard1.SuspendLayout()
        Me.pnlCard2.SuspendLayout()
        Me.pnlCard3.SuspendLayout()
        Me.pnlCard4.SuspendLayout()
        Me.tabMe.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblHeading
        '
        Me.lblHeading.AutoSize = True
        Me.lblHeading.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Bold)
        Me.lblHeading.Location = New System.Drawing.Point(16, 12)
        Me.lblHeading.Name = "lblHeading"
        Me.lblHeading.Size = New System.Drawing.Size(126, 25)
        Me.lblHeading.TabIndex = 0
        Me.lblHeading.Text = "My Account"
        '
        'lblWelcome
        '
        Me.lblWelcome.AutoSize = True
        Me.lblWelcome.Font = New System.Drawing.Font("Segoe UI", 12.0!)
        Me.lblWelcome.Location = New System.Drawing.Point(16, 48)
        Me.lblWelcome.Name = "lblWelcome"
        Me.lblWelcome.Size = New System.Drawing.Size(70, 21)
        Me.lblWelcome.TabIndex = 1
        Me.lblWelcome.Text = "Welcome"
        '
        'lblRole
        '
        Me.lblRole.AutoSize = True
        Me.lblRole.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.lblRole.Location = New System.Drawing.Point(16, 80)
        Me.lblRole.Name = "lblRole"
        Me.lblRole.Size = New System.Drawing.Size(0, 17)
        Me.lblRole.TabIndex = 2
        '
        'lblSubLastLogin
        '
        Me.lblSubLastLogin.AutoSize = True
        Me.lblSubLastLogin.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.lblSubLastLogin.Location = New System.Drawing.Point(16, 106)
        Me.lblSubLastLogin.Name = "lblSubLastLogin"
        Me.lblSubLastLogin.Size = New System.Drawing.Size(0, 13)
        Me.lblSubLastLogin.TabIndex = 3
        '
        'lblSubSession
        '
        Me.lblSubSession.AutoSize = True
        Me.lblSubSession.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.lblSubSession.Location = New System.Drawing.Point(16, 126)
        Me.lblSubSession.Name = "lblSubSession"
        Me.lblSubSession.Size = New System.Drawing.Size(0, 13)
        Me.lblSubSession.TabIndex = 4
        '
        'pnlCard1
        '
        Me.pnlCard1.Controls.Add(Me.lblCardTitle1)
        Me.pnlCard1.Controls.Add(Me.lblStat1)
        Me.pnlCard1.Controls.Add(Me.lblCardSub1)
        Me.pnlCard1.Controls.Add(Me.pnlAccent1)
        Me.pnlCard1.Location = New System.Drawing.Point(16, 158)
        Me.pnlCard1.Name = "pnlCard1"
        Me.pnlCard1.Size = New System.Drawing.Size(263, 126)
        Me.pnlCard1.TabIndex = 5
        '
        'lblCardTitle1
        '
        Me.lblCardTitle1.AutoSize = True
        Me.lblCardTitle1.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.lblCardTitle1.Location = New System.Drawing.Point(25, 14)
        Me.lblCardTitle1.Name = "lblCardTitle1"
        Me.lblCardTitle1.Size = New System.Drawing.Size(80, 17)
        Me.lblCardTitle1.TabIndex = 0
        Me.lblCardTitle1.Text = "Sales taken"
        '
        'lblStat1
        '
        Me.lblStat1.AutoSize = True
        Me.lblStat1.Font = New System.Drawing.Font("Segoe UI", 24.0!, System.Drawing.FontStyle.Bold)
        Me.lblStat1.Location = New System.Drawing.Point(21, 38)
        Me.lblStat1.Name = "lblStat1"
        Me.lblStat1.Size = New System.Drawing.Size(37, 45)
        Me.lblStat1.TabIndex = 1
        Me.lblStat1.Text = "0"
        '
        'lblCardSub1
        '
        Me.lblCardSub1.AutoSize = True
        Me.lblCardSub1.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.lblCardSub1.Location = New System.Drawing.Point(25, 95)
        Me.lblCardSub1.Name = "lblCardSub1"
        Me.lblCardSub1.Size = New System.Drawing.Size(0, 13)
        Me.lblCardSub1.TabIndex = 2
        '
        'pnlAccent1
        '
        Me.pnlAccent1.BackColor = System.Drawing.Color.FromArgb(CType(CType(216, Byte), Integer), CType(CType(27, Byte), Integer), CType(CType(96, Byte), Integer))
        Me.pnlAccent1.Dock = System.Windows.Forms.DockStyle.Left
        Me.pnlAccent1.Location = New System.Drawing.Point(0, 0)
        Me.pnlAccent1.Name = "pnlAccent1"
        Me.pnlAccent1.Size = New System.Drawing.Size(7, 126)
        Me.pnlAccent1.TabIndex = 3
        '
        'pnlCard2
        '
        Me.pnlCard2.Controls.Add(Me.lblCardTitle2)
        Me.pnlCard2.Controls.Add(Me.lblStat2)
        Me.pnlCard2.Controls.Add(Me.lblCardSub2)
        Me.pnlCard2.Controls.Add(Me.pnlAccent2)
        Me.pnlCard2.Location = New System.Drawing.Point(296, 158)
        Me.pnlCard2.Name = "pnlCard2"
        Me.pnlCard2.Size = New System.Drawing.Size(263, 126)
        Me.pnlCard2.TabIndex = 6
        '
        'lblCardTitle2
        '
        Me.lblCardTitle2.AutoSize = True
        Me.lblCardTitle2.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.lblCardTitle2.Location = New System.Drawing.Point(25, 14)
        Me.lblCardTitle2.Name = "lblCardTitle2"
        Me.lblCardTitle2.Size = New System.Drawing.Size(80, 17)
        Me.lblCardTitle2.TabIndex = 0
        Me.lblCardTitle2.Text = "Seats sold"
        '
        'lblStat2
        '
        Me.lblStat2.AutoSize = True
        Me.lblStat2.Font = New System.Drawing.Font("Segoe UI", 24.0!, System.Drawing.FontStyle.Bold)
        Me.lblStat2.Location = New System.Drawing.Point(21, 38)
        Me.lblStat2.Name = "lblStat2"
        Me.lblStat2.Size = New System.Drawing.Size(37, 45)
        Me.lblStat2.TabIndex = 1
        Me.lblStat2.Text = "0"
        '
        'lblCardSub2
        '
        Me.lblCardSub2.AutoSize = True
        Me.lblCardSub2.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.lblCardSub2.Location = New System.Drawing.Point(25, 95)
        Me.lblCardSub2.Name = "lblCardSub2"
        Me.lblCardSub2.Size = New System.Drawing.Size(0, 13)
        Me.lblCardSub2.TabIndex = 2
        '
        'pnlAccent2
        '
        Me.pnlAccent2.BackColor = System.Drawing.Color.FromArgb(CType(CType(142, Byte), Integer), CType(CType(36, Byte), Integer), CType(CType(170, Byte), Integer))
        Me.pnlAccent2.Dock = System.Windows.Forms.DockStyle.Left
        Me.pnlAccent2.Location = New System.Drawing.Point(0, 0)
        Me.pnlAccent2.Name = "pnlAccent2"
        Me.pnlAccent2.Size = New System.Drawing.Size(7, 126)
        Me.pnlAccent2.TabIndex = 3
        '
        'pnlCard3
        '
        Me.pnlCard3.Controls.Add(Me.lblCardTitle3)
        Me.pnlCard3.Controls.Add(Me.lblStat3)
        Me.pnlCard3.Controls.Add(Me.lblCardSub3)
        Me.pnlCard3.Controls.Add(Me.pnlAccent3)
        Me.pnlCard3.Location = New System.Drawing.Point(576, 158)
        Me.pnlCard3.Name = "pnlCard3"
        Me.pnlCard3.Size = New System.Drawing.Size(263, 126)
        Me.pnlCard3.TabIndex = 7
        '
        'lblCardTitle3
        '
        Me.lblCardTitle3.AutoSize = True
        Me.lblCardTitle3.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.lblCardTitle3.Location = New System.Drawing.Point(25, 14)
        Me.lblCardTitle3.Name = "lblCardTitle3"
        Me.lblCardTitle3.Size = New System.Drawing.Size(80, 17)
        Me.lblCardTitle3.TabIndex = 0
        Me.lblCardTitle3.Text = "Things done"
        '
        'lblStat3
        '
        Me.lblStat3.AutoSize = True
        Me.lblStat3.Font = New System.Drawing.Font("Segoe UI", 24.0!, System.Drawing.FontStyle.Bold)
        Me.lblStat3.Location = New System.Drawing.Point(21, 38)
        Me.lblStat3.Name = "lblStat3"
        Me.lblStat3.Size = New System.Drawing.Size(37, 45)
        Me.lblStat3.TabIndex = 1
        Me.lblStat3.Text = "0"
        '
        'lblCardSub3
        '
        Me.lblCardSub3.AutoSize = True
        Me.lblCardSub3.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.lblCardSub3.Location = New System.Drawing.Point(25, 95)
        Me.lblCardSub3.Name = "lblCardSub3"
        Me.lblCardSub3.Size = New System.Drawing.Size(0, 13)
        Me.lblCardSub3.TabIndex = 2
        '
        'pnlAccent3
        '
        Me.pnlAccent3.BackColor = System.Drawing.Color.FromArgb(CType(CType(2, Byte), Integer), CType(CType(136, Byte), Integer), CType(CType(209, Byte), Integer))
        Me.pnlAccent3.Dock = System.Windows.Forms.DockStyle.Left
        Me.pnlAccent3.Location = New System.Drawing.Point(0, 0)
        Me.pnlAccent3.Name = "pnlAccent3"
        Me.pnlAccent3.Size = New System.Drawing.Size(7, 126)
        Me.pnlAccent3.TabIndex = 3
        '
        'pnlCard4
        '
        Me.pnlCard4.Controls.Add(Me.lblCardTitle4)
        Me.pnlCard4.Controls.Add(Me.lblStat4)
        Me.pnlCard4.Controls.Add(Me.lblCardSub4)
        Me.pnlCard4.Controls.Add(Me.pnlAccent4)
        Me.pnlCard4.Location = New System.Drawing.Point(856, 158)
        Me.pnlCard4.Name = "pnlCard4"
        Me.pnlCard4.Size = New System.Drawing.Size(263, 126)
        Me.pnlCard4.TabIndex = 8
        '
        'lblCardTitle4
        '
        Me.lblCardTitle4.AutoSize = True
        Me.lblCardTitle4.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.lblCardTitle4.Location = New System.Drawing.Point(25, 14)
        Me.lblCardTitle4.Name = "lblCardTitle4"
        Me.lblCardTitle4.Size = New System.Drawing.Size(80, 17)
        Me.lblCardTitle4.TabIndex = 0
        Me.lblCardTitle4.Text = "Takings"
        '
        'lblStat4
        '
        Me.lblStat4.AutoSize = True
        Me.lblStat4.Font = New System.Drawing.Font("Segoe UI", 24.0!, System.Drawing.FontStyle.Bold)
        Me.lblStat4.Location = New System.Drawing.Point(21, 38)
        Me.lblStat4.Name = "lblStat4"
        Me.lblStat4.Size = New System.Drawing.Size(37, 45)
        Me.lblStat4.TabIndex = 1
        Me.lblStat4.Text = "0"
        '
        'lblCardSub4
        '
        Me.lblCardSub4.AutoSize = True
        Me.lblCardSub4.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.lblCardSub4.Location = New System.Drawing.Point(25, 95)
        Me.lblCardSub4.Name = "lblCardSub4"
        Me.lblCardSub4.Size = New System.Drawing.Size(0, 13)
        Me.lblCardSub4.TabIndex = 2
        '
        'pnlAccent4
        '
        Me.pnlAccent4.BackColor = System.Drawing.Color.FromArgb(CType(CType(56, Byte), Integer), CType(CType(142, Byte), Integer), CType(CType(60, Byte), Integer))
        Me.pnlAccent4.Dock = System.Windows.Forms.DockStyle.Left
        Me.pnlAccent4.Location = New System.Drawing.Point(0, 0)
        Me.pnlAccent4.Name = "pnlAccent4"
        Me.pnlAccent4.Size = New System.Drawing.Size(7, 126)
        Me.pnlAccent4.TabIndex = 3
        '
        'tabMe
        '
        Me.tabMe.Controls.Add(Me.tabActivity)
        Me.tabMe.Controls.Add(Me.tabSales)
        Me.tabMe.Controls.Add(Me.tabPattern)
        Me.tabMe.Controls.Add(Me.tabSettings)
        Me.tabMe.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.tabMe.Location = New System.Drawing.Point(16, 298)
        Me.tabMe.Name = "tabMe"
        Me.tabMe.SelectedIndex = 0
        Me.tabMe.Size = New System.Drawing.Size(1103, 442)
        Me.tabMe.TabIndex = 9
        '
        'tabActivity
        '
        Me.tabActivity.Location = New System.Drawing.Point(4, 24)
        Me.tabActivity.Name = "tabActivity"
        Me.tabActivity.Padding = New System.Windows.Forms.Padding(3)
        Me.tabActivity.Size = New System.Drawing.Size(1095, 414)
        Me.tabActivity.TabIndex = 0
        Me.tabActivity.Text = "What I have done"
        Me.tabActivity.UseVisualStyleBackColor = True
        '
        'tabSales
        '
        Me.tabSales.Location = New System.Drawing.Point(4, 24)
        Me.tabSales.Name = "tabSales"
        Me.tabSales.Padding = New System.Windows.Forms.Padding(3)
        Me.tabSales.Size = New System.Drawing.Size(1095, 414)
        Me.tabSales.TabIndex = 1
        Me.tabSales.Text = "Sales I have taken"
        Me.tabSales.UseVisualStyleBackColor = True
        '
        'tabPattern
        '
        Me.tabPattern.Location = New System.Drawing.Point(4, 24)
        Me.tabPattern.Name = "tabPattern"
        Me.tabPattern.Padding = New System.Windows.Forms.Padding(3)
        Me.tabPattern.Size = New System.Drawing.Size(1095, 414)
        Me.tabPattern.TabIndex = 2
        Me.tabPattern.Text = "When I work"
        Me.tabPattern.UseVisualStyleBackColor = True
        '
        'tabSettings
        '
        Me.tabSettings.Location = New System.Drawing.Point(4, 24)
        Me.tabSettings.Name = "tabSettings"
        Me.tabSettings.Padding = New System.Windows.Forms.Padding(3)
        Me.tabSettings.Size = New System.Drawing.Size(1095, 414)
        Me.tabSettings.TabIndex = 3
        Me.tabSettings.Text = "My settings"
        Me.tabSettings.UseVisualStyleBackColor = True
        '
        'lblVersion
        '
        Me.lblVersion.AutoSize = True
        Me.lblVersion.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.lblVersion.Location = New System.Drawing.Point(16, 748)
        Me.lblVersion.Name = "lblVersion"
        Me.lblVersion.Size = New System.Drawing.Size(240, 13)
        Me.lblVersion.TabIndex = 10
        Me.lblVersion.Text = "Filmtopia Cinema Management System  v1.0"
        '
        'frmUserOverview
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1135, 775)
        Me.Controls.Add(Me.lblVersion)
        Me.Controls.Add(Me.tabMe)
        Me.Controls.Add(Me.pnlCard4)
        Me.Controls.Add(Me.pnlCard3)
        Me.Controls.Add(Me.pnlCard2)
        Me.Controls.Add(Me.pnlCard1)
        Me.Controls.Add(Me.lblSubSession)
        Me.Controls.Add(Me.lblSubLastLogin)
        Me.Controls.Add(Me.lblRole)
        Me.Controls.Add(Me.lblWelcome)
        Me.Controls.Add(Me.lblHeading)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.MaximizeBox = False
        Me.Name = "frmUserOverview"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "My Account"
        Me.pnlCard1.ResumeLayout(False)
        Me.pnlCard1.PerformLayout()
        Me.pnlCard2.ResumeLayout(False)
        Me.pnlCard2.PerformLayout()
        Me.pnlCard3.ResumeLayout(False)
        Me.pnlCard3.PerformLayout()
        Me.pnlCard4.ResumeLayout(False)
        Me.pnlCard4.PerformLayout()
        Me.tabMe.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblHeading As Label
    Friend WithEvents lblWelcome As Label
    Friend WithEvents lblRole As Label
    Friend WithEvents lblSubLastLogin As Label
    Friend WithEvents lblSubSession As Label
    Friend WithEvents pnlCard1 As Panel
    Friend WithEvents lblCardTitle1 As Label
    Friend WithEvents lblStat1 As Label
    Friend WithEvents lblCardSub1 As Label
    Friend WithEvents pnlAccent1 As Panel
    Friend WithEvents pnlCard2 As Panel
    Friend WithEvents lblCardTitle2 As Label
    Friend WithEvents lblStat2 As Label
    Friend WithEvents lblCardSub2 As Label
    Friend WithEvents pnlAccent2 As Panel
    Friend WithEvents pnlCard3 As Panel
    Friend WithEvents lblCardTitle3 As Label
    Friend WithEvents lblStat3 As Label
    Friend WithEvents lblCardSub3 As Label
    Friend WithEvents pnlAccent3 As Panel
    Friend WithEvents pnlCard4 As Panel
    Friend WithEvents lblCardTitle4 As Label
    Friend WithEvents lblStat4 As Label
    Friend WithEvents lblCardSub4 As Label
    Friend WithEvents pnlAccent4 As Panel
    Friend WithEvents tabMe As TabControl
    Friend WithEvents tabActivity As TabPage
    Friend WithEvents tabSales As TabPage
    Friend WithEvents tabPattern As TabPage
    Friend WithEvents tabSettings As TabPage
    Friend WithEvents lblVersion As Label
End Class
