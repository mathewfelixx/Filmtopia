<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTesting
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.btnVersionControl = New System.Windows.Forms.Button()
        Me.btnLogin = New System.Windows.Forms.Button()
        Me.btnMainOLD = New System.Windows.Forms.Button()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.btnMainStaff = New System.Windows.Forms.Button()
        Me.btnSettings = New System.Windows.Forms.Button()
        Me.btnLogs = New System.Windows.Forms.Button()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnVersionControl
        '
        Me.btnVersionControl.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnVersionControl.Location = New System.Drawing.Point(205, 182)
        Me.btnVersionControl.Margin = New System.Windows.Forms.Padding(2)
        Me.btnVersionControl.Name = "btnVersionControl"
        Me.btnVersionControl.Size = New System.Drawing.Size(199, 177)
        Me.btnVersionControl.TabIndex = 0
        Me.btnVersionControl.Text = "Version Control"
        Me.btnVersionControl.UseVisualStyleBackColor = True
        '
        'btnLogin
        '
        Me.btnLogin.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnLogin.Location = New System.Drawing.Point(2, 2)
        Me.btnLogin.Margin = New System.Windows.Forms.Padding(2)
        Me.btnLogin.Name = "btnLogin"
        Me.btnLogin.Size = New System.Drawing.Size(199, 176)
        Me.btnLogin.TabIndex = 1
        Me.btnLogin.Text = "Login"
        Me.btnLogin.UseVisualStyleBackColor = True
        '
        'btnMainOLD
        '
        Me.btnMainOLD.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnMainOLD.Location = New System.Drawing.Point(205, 2)
        Me.btnMainOLD.Margin = New System.Windows.Forms.Padding(2)
        Me.btnMainOLD.Name = "btnMainOLD"
        Me.btnMainOLD.Size = New System.Drawing.Size(199, 176)
        Me.btnMainOLD.TabIndex = 3
        Me.btnMainOLD.Text = "MainOLD"
        Me.btnMainOLD.UseVisualStyleBackColor = True
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 3
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 190.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.btnMainStaff, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.btnMainOLD, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.btnVersionControl, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.btnLogin, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.btnSettings, 2, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.btnLogs, 2, 1)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(2, 3)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 2
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(597, 361)
        Me.TableLayoutPanel1.TabIndex = 4
        '
        'btnMainStaff
        '
        Me.btnMainStaff.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnMainStaff.Location = New System.Drawing.Point(2, 182)
        Me.btnMainStaff.Margin = New System.Windows.Forms.Padding(2)
        Me.btnMainStaff.Name = "btnMainStaff"
        Me.btnMainStaff.Size = New System.Drawing.Size(199, 177)
        Me.btnMainStaff.TabIndex = 4
        Me.btnMainStaff.Text = "MainStaff"
        Me.btnMainStaff.UseVisualStyleBackColor = True
        '
        'btnSettings
        '
        Me.btnSettings.Location = New System.Drawing.Point(409, 3)
        Me.btnSettings.Name = "btnSettings"
        Me.btnSettings.Size = New System.Drawing.Size(185, 174)
        Me.btnSettings.TabIndex = 5
        Me.btnSettings.Text = "Settings"
        Me.btnSettings.UseVisualStyleBackColor = True
        '
        'btnLogs
        '
        Me.btnLogs.Location = New System.Drawing.Point(409, 183)
        Me.btnLogs.Name = "btnLogs"
        Me.btnLogs.Size = New System.Drawing.Size(185, 175)
        Me.btnLogs.TabIndex = 6
        Me.btnLogs.Text = "Logs"
        Me.btnLogs.UseVisualStyleBackColor = True
        '
        'frmTesting
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(600, 366)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Margin = New System.Windows.Forms.Padding(2)
        Me.Name = "frmTesting"
        Me.Text = "frmTesting"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents btnVersionControl As Button
    Friend WithEvents btnLogin As Button
    Friend WithEvents btnMainOLD As Button
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents btnMainStaff As Button
    Friend WithEvents btnSettings As Button
    Friend WithEvents btnLogs As Button
End Class
