<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmTesting
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
        Me.btnOpenLogin = New System.Windows.Forms.Button()
        Me.btnMainStaff = New System.Windows.Forms.Button()
        Me.btnMainAdmin = New System.Windows.Forms.Button()
        Me.btnOpenLogs = New System.Windows.Forms.Button()
        Me.btnOpenVersionControl = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'btnOpenLogin
        '
        Me.btnOpenLogin.Location = New System.Drawing.Point(30, 30)
        Me.btnOpenLogin.Name = "btnOpenLogin"
        Me.btnOpenLogin.Size = New System.Drawing.Size(180, 30)
        Me.btnOpenLogin.TabIndex = 0
        Me.btnOpenLogin.Text = "Open Login"
        Me.btnOpenLogin.UseVisualStyleBackColor = True
        '
        'btnMainStaff
        '
        Me.btnMainStaff.Location = New System.Drawing.Point(30, 70)
        Me.btnMainStaff.Name = "btnMainStaff"
        Me.btnMainStaff.Size = New System.Drawing.Size(180, 30)
        Me.btnMainStaff.TabIndex = 1
        Me.btnMainStaff.Text = "Open Main Menu Staff"
        Me.btnMainStaff.UseVisualStyleBackColor = True
        '
        'btnMainAdmin
        '
        Me.btnMainAdmin.Location = New System.Drawing.Point(30, 110)
        Me.btnMainAdmin.Name = "btnMainAdmin"
        Me.btnMainAdmin.Size = New System.Drawing.Size(180, 30)
        Me.btnMainAdmin.TabIndex = 2
        Me.btnMainAdmin.Text = "Open Main Menu Admin"
        Me.btnMainAdmin.UseVisualStyleBackColor = True
        '
        'btnOpenLogs
        '
        Me.btnOpenLogs.Location = New System.Drawing.Point(30, 150)
        Me.btnOpenLogs.Name = "btnOpenLogs"
        Me.btnOpenLogs.Size = New System.Drawing.Size(180, 30)
        Me.btnOpenLogs.TabIndex = 3
        Me.btnOpenLogs.Text = "Open Logs"
        Me.btnOpenLogs.UseVisualStyleBackColor = True
        '
        'btnOpenVersionControl
        '
        Me.btnOpenVersionControl.Location = New System.Drawing.Point(30, 190)
        Me.btnOpenVersionControl.Name = "btnOpenVersionControl"
        Me.btnOpenVersionControl.Size = New System.Drawing.Size(180, 30)
        Me.btnOpenVersionControl.TabIndex = 4
        Me.btnOpenVersionControl.Text = "Open Version Control"
        Me.btnOpenVersionControl.UseVisualStyleBackColor = True
        '
        'frmTesting
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.btnOpenLogin)
        Me.Controls.Add(Me.btnMainStaff)
        Me.Controls.Add(Me.btnMainAdmin)
        Me.Controls.Add(Me.btnOpenLogs)
        Me.Controls.Add(Me.btnOpenVersionControl)
        Me.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Name = "frmTesting"
        Me.Text = "frmTesting"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents btnOpenLogin As Button
    Friend WithEvents btnMainStaff As Button
    Friend WithEvents btnMainAdmin As Button
    Friend WithEvents btnOpenLogs As Button
    Friend WithEvents btnOpenVersionControl As Button
End Class
