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
        Me.btnSettings = New System.Windows.Forms.Button()
        Me.btnLogs = New System.Windows.Forms.Button()
        Me.btnLogin = New System.Windows.Forms.Button()
        Me.btnVersionControl = New System.Windows.Forms.Button()
        Me.btnMainOLD = New System.Windows.Forms.Button()
        Me.btnMainStaff = New System.Windows.Forms.Button()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.btnAdminMode = New System.Windows.Forms.Button()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnSettings
        '
        Me.btnSettings.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnSettings.Location = New System.Drawing.Point(236, 229)
        Me.btnSettings.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSettings.Name = "btnSettings"
        Me.btnSettings.Size = New System.Drawing.Size(91, 217)
        Me.btnSettings.TabIndex = 5
        Me.btnSettings.Text = "Settings"
        Me.btnSettings.UseVisualStyleBackColor = True
        '
        'btnLogs
        '
        Me.btnLogs.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnLogs.Location = New System.Drawing.Point(236, 4)
        Me.btnLogs.Margin = New System.Windows.Forms.Padding(4)
        Me.btnLogs.Name = "btnLogs"
        Me.btnLogs.Size = New System.Drawing.Size(91, 217)
        Me.btnLogs.TabIndex = 6
        Me.btnLogs.Text = "Logs"
        Me.btnLogs.UseVisualStyleBackColor = True
        '
        'btnLogin
        '
        Me.btnLogin.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnLogin.Location = New System.Drawing.Point(3, 2)
        Me.btnLogin.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.btnLogin.Name = "btnLogin"
        Me.btnLogin.Size = New System.Drawing.Size(110, 221)
        Me.btnLogin.TabIndex = 1
        Me.btnLogin.Text = "Login"
        Me.btnLogin.UseVisualStyleBackColor = True
        '
        'btnVersionControl
        '
        Me.btnVersionControl.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnVersionControl.Location = New System.Drawing.Point(119, 227)
        Me.btnVersionControl.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.btnVersionControl.Name = "btnVersionControl"
        Me.btnVersionControl.Size = New System.Drawing.Size(110, 221)
        Me.btnVersionControl.TabIndex = 0
        Me.btnVersionControl.Text = "Version Control"
        Me.btnVersionControl.UseVisualStyleBackColor = True
        '
        'btnMainOLD
        '
        Me.btnMainOLD.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnMainOLD.Location = New System.Drawing.Point(119, 2)
        Me.btnMainOLD.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.btnMainOLD.Name = "btnMainOLD"
        Me.btnMainOLD.Size = New System.Drawing.Size(110, 221)
        Me.btnMainOLD.TabIndex = 3
        Me.btnMainOLD.Text = "MainOLD"
        Me.btnMainOLD.UseVisualStyleBackColor = True
        '
        'btnMainStaff
        '
        Me.btnMainStaff.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnMainStaff.Location = New System.Drawing.Point(3, 227)
        Me.btnMainStaff.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.btnMainStaff.Name = "btnMainStaff"
        Me.btnMainStaff.Size = New System.Drawing.Size(110, 221)
        Me.btnMainStaff.TabIndex = 4
        Me.btnMainStaff.Text = "MainNew"
        Me.btnMainStaff.UseVisualStyleBackColor = True
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 6
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 99.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 131.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 317.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.btnAdminMode, 3, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.btnSettings, 2, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.btnLogs, 2, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.btnMainStaff, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.btnMainOLD, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.btnVersionControl, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.btnLogin, 0, 0)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Margin = New System.Windows.Forms.Padding(4)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 2
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(800, 450)
        Me.TableLayoutPanel1.TabIndex = 4
        '
        'btnAdminMode
        '
        Me.btnAdminMode.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnAdminMode.Location = New System.Drawing.Point(335, 4)
        Me.btnAdminMode.Margin = New System.Windows.Forms.Padding(4)
        Me.btnAdminMode.Name = "btnAdminMode"
        Me.btnAdminMode.Size = New System.Drawing.Size(123, 217)
        Me.btnAdminMode.TabIndex = 7
        Me.btnAdminMode.Text = "MainFormAdminMode"
        Me.btnAdminMode.UseVisualStyleBackColor = True
        '
        'frmTesting
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Name = "frmTesting"
        Me.Text = "frmTesting"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnSettings As Button
    Friend WithEvents btnLogs As Button
    Friend WithEvents btnLogin As Button
    Friend WithEvents btnVersionControl As Button
    Friend WithEvents btnMainOLD As Button
    Friend WithEvents btnMainStaff As Button
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents btnAdminMode As Button
End Class
