<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmVersionControlUTIL
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
        Me.txtVersionControl = New System.Windows.Forms.TextBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lblVersion = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'txtVersionControl
        '
        Me.txtVersionControl.Location = New System.Drawing.Point(32, 194)
        Me.txtVersionControl.Name = "txtVersionControl"
        Me.txtVersionControl.Size = New System.Drawing.Size(314, 22)
        Me.txtVersionControl.TabIndex = 0
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(148, 238)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 1
        Me.Button1.Text = "Submit"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(119, 160)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(149, 16)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Enter a version ; ""v1.0.0"""
        '
        'lblVersion
        '
        Me.lblVersion.AutoSize = True
        Me.lblVersion.Location = New System.Drawing.Point(12, 414)
        Me.lblVersion.Name = "lblVersion"
        Me.lblVersion.Size = New System.Drawing.Size(149, 16)
        Me.lblVersion.TabIndex = 3
        Me.lblVersion.Text = "Enter a version ; ""v1.0.0"""
        '
        'frmVersionControlUTIL
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.ControlDarkDark
        Me.ClientSize = New System.Drawing.Size(382, 450)
        Me.Controls.Add(Me.lblVersion)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.txtVersionControl)
        Me.Name = "frmVersionControlUTIL"
        Me.Text = "frmVersionControlUTIL"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents txtVersionControl As TextBox
    Friend WithEvents Button1 As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents lblVersion As Label
End Class
