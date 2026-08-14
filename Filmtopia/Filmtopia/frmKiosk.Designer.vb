<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmKiosk
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
        Me.pnlHeader = New System.Windows.Forms.Panel()
        Me.lblKioskTitle = New System.Windows.Forms.Label()
        Me.lblStep = New System.Windows.Forms.Label()
        Me.btnExitKiosk = New System.Windows.Forms.Button()
        Me.lblVersion = New System.Windows.Forms.Label()
        Me.pnlHeader.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnlHeader
        '
        Me.pnlHeader.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.pnlHeader.Controls.Add(Me.btnExitKiosk)
        Me.pnlHeader.Controls.Add(Me.lblStep)
        Me.pnlHeader.Controls.Add(Me.lblKioskTitle)
        Me.pnlHeader.Location = New System.Drawing.Point(0, 0)
        Me.pnlHeader.Name = "pnlHeader"
        Me.pnlHeader.Size = New System.Drawing.Size(1280, 96)
        Me.pnlHeader.TabIndex = 0
        '
        'lblKioskTitle
        '
        Me.lblKioskTitle.AutoSize = True
        Me.lblKioskTitle.Font = New System.Drawing.Font("Segoe UI", 22.0!, System.Drawing.FontStyle.Bold)
        Me.lblKioskTitle.ForeColor = System.Drawing.Color.White
        Me.lblKioskTitle.Location = New System.Drawing.Point(32, 16)
        Me.lblKioskTitle.Name = "lblKioskTitle"
        Me.lblKioskTitle.Size = New System.Drawing.Size(180, 41)
        Me.lblKioskTitle.TabIndex = 0
        Me.lblKioskTitle.Text = "Filmtopia"
        '
        'lblStep
        '
        Me.lblStep.AutoSize = True
        Me.lblStep.Font = New System.Drawing.Font("Segoe UI", 12.0!)
        Me.lblStep.ForeColor = System.Drawing.Color.FromArgb(CType(CType(231, Byte), Integer), CType(CType(169, Byte), Integer), CType(CType(231, Byte), Integer))
        Me.lblStep.Location = New System.Drawing.Point(36, 60)
        Me.lblStep.Name = "lblStep"
        Me.lblStep.Size = New System.Drawing.Size(120, 28)
        Me.lblStep.TabIndex = 1
        Me.lblStep.Text = "Self service"
        '
        'btnExitKiosk
        '
        Me.btnExitKiosk.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(120, Byte), Integer), CType(CType(60, Byte), Integer), CType(CType(120, Byte), Integer))
        Me.btnExitKiosk.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnExitKiosk.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.btnExitKiosk.ForeColor = System.Drawing.Color.FromArgb(CType(CType(190, Byte), Integer), CType(CType(150, Byte), Integer), CType(CType(190, Byte), Integer))
        Me.btnExitKiosk.Location = New System.Drawing.Point(1130, 28)
        Me.btnExitKiosk.Name = "btnExitKiosk"
        Me.btnExitKiosk.Size = New System.Drawing.Size(120, 44)
        Me.btnExitKiosk.TabIndex = 2
        Me.btnExitKiosk.Text = "Staff exit"
        Me.btnExitKiosk.UseVisualStyleBackColor = True
        '
        'lblVersion
        '
        Me.lblVersion.AutoSize = True
        Me.lblVersion.ForeColor = System.Drawing.Color.Gray
        Me.lblVersion.Location = New System.Drawing.Point(32, 772)
        Me.lblVersion.Name = "lblVersion"
        Me.lblVersion.Size = New System.Drawing.Size(271, 16)
        Me.lblVersion.TabIndex = 1
        Me.lblVersion.Text = "Filmtopia Cinema Management System  v1.0"
        '
        'frmKiosk
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1280, 800)
        Me.Controls.Add(Me.lblVersion)
        Me.Controls.Add(Me.pnlHeader)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "frmKiosk"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Filmtopia Kiosk"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.pnlHeader.ResumeLayout(False)
        Me.pnlHeader.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents pnlHeader As Panel
    Friend WithEvents lblKioskTitle As Label
    Friend WithEvents lblStep As Label
    Friend WithEvents btnExitKiosk As Button
    Friend WithEvents lblVersion As Label
End Class
