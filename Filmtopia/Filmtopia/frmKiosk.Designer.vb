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
        Me.pnlFooter = New System.Windows.Forms.Panel()
        Me.btnBack = New System.Windows.Forms.Button()
        Me.btnNext = New System.Windows.Forms.Button()
        Me.lblRunningTotal = New System.Windows.Forms.Label()
        Me.pnlWelcome = New System.Windows.Forms.Panel()
        Me.lblWelcomeTitle = New System.Windows.Forms.Label()
        Me.lblWelcomeSub = New System.Windows.Forms.Label()
        Me.btnStart = New System.Windows.Forms.Button()
        Me.pnlFilms = New System.Windows.Forms.Panel()
        Me.lblFilmsHeading = New System.Windows.Forms.Label()
        Me.lblNoFilms = New System.Windows.Forms.Label()
        Me.pnlFilmList = New System.Windows.Forms.Panel()
        Me.pnlTimes = New System.Windows.Forms.Panel()
        Me.lblTimesHeading = New System.Windows.Forms.Label()
        Me.lblTimesFilm = New System.Windows.Forms.Label()
        Me.pnlTimeList = New System.Windows.Forms.Panel()
        Me.pnlHeader.SuspendLayout()
        Me.pnlFooter.SuspendLayout()
        Me.pnlWelcome.SuspendLayout()
        Me.pnlFilms.SuspendLayout()
        Me.pnlTimes.SuspendLayout()
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
        Me.lblVersion.Location = New System.Drawing.Point(989, 74)
        Me.lblVersion.Name = "lblVersion"
        Me.lblVersion.Size = New System.Drawing.Size(271, 16)
        Me.lblVersion.TabIndex = 1
        Me.lblVersion.Text = "Filmtopia Cinema Management System  v1.0"
        '
        'pnlFooter
        '
        Me.pnlFooter.Controls.Add(Me.lblVersion)
        Me.pnlFooter.Controls.Add(Me.lblRunningTotal)
        Me.pnlFooter.Controls.Add(Me.btnNext)
        Me.pnlFooter.Controls.Add(Me.btnBack)
        Me.pnlFooter.Location = New System.Drawing.Point(0, 692)
        Me.pnlFooter.Name = "pnlFooter"
        Me.pnlFooter.Size = New System.Drawing.Size(1280, 100)
        Me.pnlFooter.TabIndex = 2
        '
        'btnBack
        '
        Me.btnBack.Font = New System.Drawing.Font("Segoe UI", 15.0!)
        Me.btnBack.Location = New System.Drawing.Point(32, 18)
        Me.btnBack.Name = "btnBack"
        Me.btnBack.Size = New System.Drawing.Size(220, 68)
        Me.btnBack.TabIndex = 0
        Me.btnBack.Text = "Back"
        Me.btnBack.UseVisualStyleBackColor = True
        '
        'btnNext
        '
        Me.btnNext.Font = New System.Drawing.Font("Segoe UI", 15.0!, System.Drawing.FontStyle.Bold)
        Me.btnNext.Location = New System.Drawing.Point(1028, 18)
        Me.btnNext.Name = "btnNext"
        Me.btnNext.Size = New System.Drawing.Size(220, 68)
        Me.btnNext.TabIndex = 1
        Me.btnNext.Text = "Continue"
        Me.btnNext.UseVisualStyleBackColor = True
        Me.btnNext.Visible = False
        '
        'lblRunningTotal
        '
        Me.lblRunningTotal.AutoSize = True
        Me.lblRunningTotal.Font = New System.Drawing.Font("Segoe UI", 16.0!, System.Drawing.FontStyle.Bold)
        Me.lblRunningTotal.Location = New System.Drawing.Point(800, 36)
        Me.lblRunningTotal.Name = "lblRunningTotal"
        Me.lblRunningTotal.Size = New System.Drawing.Size(140, 30)
        Me.lblRunningTotal.TabIndex = 2
        Me.lblRunningTotal.Text = "Total  £0.00"
        Me.lblRunningTotal.Visible = False
        '
        'pnlWelcome
        '
        Me.pnlWelcome.Controls.Add(Me.btnStart)
        Me.pnlWelcome.Controls.Add(Me.lblWelcomeSub)
        Me.pnlWelcome.Controls.Add(Me.lblWelcomeTitle)
        Me.pnlWelcome.Location = New System.Drawing.Point(0, 96)
        Me.pnlWelcome.Name = "pnlWelcome"
        Me.pnlWelcome.Size = New System.Drawing.Size(1280, 596)
        Me.pnlWelcome.TabIndex = 3
        '
        'lblWelcomeTitle
        '
        Me.lblWelcomeTitle.AutoSize = True
        Me.lblWelcomeTitle.Font = New System.Drawing.Font("Segoe UI", 34.0!, System.Drawing.FontStyle.Bold)
        Me.lblWelcomeTitle.Location = New System.Drawing.Point(340, 150)
        Me.lblWelcomeTitle.Name = "lblWelcomeTitle"
        Me.lblWelcomeTitle.Size = New System.Drawing.Size(560, 61)
        Me.lblWelcomeTitle.TabIndex = 0
        Me.lblWelcomeTitle.Text = "Buy your tickets here"
        '
        'lblWelcomeSub
        '
        Me.lblWelcomeSub.AutoSize = True
        Me.lblWelcomeSub.Font = New System.Drawing.Font("Segoe UI", 16.0!)
        Me.lblWelcomeSub.Location = New System.Drawing.Point(400, 230)
        Me.lblWelcomeSub.Name = "lblWelcomeSub"
        Me.lblWelcomeSub.Size = New System.Drawing.Size(420, 30)
        Me.lblWelcomeSub.TabIndex = 1
        Me.lblWelcomeSub.Text = "Pick a film, pick your seats, pay at the machine"
        '
        'btnStart
        '
        Me.btnStart.Font = New System.Drawing.Font("Segoe UI", 22.0!, System.Drawing.FontStyle.Bold)
        Me.btnStart.Location = New System.Drawing.Point(410, 320)
        Me.btnStart.Name = "btnStart"
        Me.btnStart.Size = New System.Drawing.Size(460, 130)
        Me.btnStart.TabIndex = 2
        Me.btnStart.Text = "Touch to start"
        Me.btnStart.UseVisualStyleBackColor = True
        '
        'pnlFilms
        '
        Me.pnlFilms.Controls.Add(Me.pnlFilmList)
        Me.pnlFilms.Controls.Add(Me.lblNoFilms)
        Me.pnlFilms.Controls.Add(Me.lblFilmsHeading)
        Me.pnlFilms.Location = New System.Drawing.Point(0, 96)
        Me.pnlFilms.Name = "pnlFilms"
        Me.pnlFilms.Size = New System.Drawing.Size(1280, 596)
        Me.pnlFilms.TabIndex = 4
        Me.pnlFilms.Visible = False
        '
        'lblFilmsHeading
        '
        Me.lblFilmsHeading.AutoSize = True
        Me.lblFilmsHeading.Font = New System.Drawing.Font("Segoe UI", 20.0!, System.Drawing.FontStyle.Bold)
        Me.lblFilmsHeading.Location = New System.Drawing.Point(32, 24)
        Me.lblFilmsHeading.Name = "lblFilmsHeading"
        Me.lblFilmsHeading.Size = New System.Drawing.Size(300, 37)
        Me.lblFilmsHeading.TabIndex = 0
        Me.lblFilmsHeading.Text = "What's on today"
        '
        'lblNoFilms
        '
        Me.lblNoFilms.AutoSize = True
        Me.lblNoFilms.Font = New System.Drawing.Font("Segoe UI", 14.0!)
        Me.lblNoFilms.ForeColor = System.Drawing.Color.Gray
        Me.lblNoFilms.Location = New System.Drawing.Point(36, 76)
        Me.lblNoFilms.Name = "lblNoFilms"
        Me.lblNoFilms.Size = New System.Drawing.Size(420, 25)
        Me.lblNoFilms.TabIndex = 1
        Me.lblNoFilms.Text = "There is nothing left on today, please ask at the desk"
        Me.lblNoFilms.Visible = False
        '
        'pnlFilmList
        '
        Me.pnlFilmList.AutoScroll = True
        Me.pnlFilmList.Location = New System.Drawing.Point(20, 76)
        Me.pnlFilmList.Name = "pnlFilmList"
        Me.pnlFilmList.Size = New System.Drawing.Size(1240, 500)
        Me.pnlFilmList.TabIndex = 2
        '
        'pnlTimes
        '
        Me.pnlTimes.Controls.Add(Me.pnlTimeList)
        Me.pnlTimes.Controls.Add(Me.lblTimesFilm)
        Me.pnlTimes.Controls.Add(Me.lblTimesHeading)
        Me.pnlTimes.Location = New System.Drawing.Point(0, 96)
        Me.pnlTimes.Name = "pnlTimes"
        Me.pnlTimes.Size = New System.Drawing.Size(1280, 596)
        Me.pnlTimes.TabIndex = 5
        Me.pnlTimes.Visible = False
        '
        'lblTimesHeading
        '
        Me.lblTimesHeading.AutoSize = True
        Me.lblTimesHeading.Font = New System.Drawing.Font("Segoe UI", 20.0!, System.Drawing.FontStyle.Bold)
        Me.lblTimesHeading.Location = New System.Drawing.Point(32, 24)
        Me.lblTimesHeading.Name = "lblTimesHeading"
        Me.lblTimesHeading.Size = New System.Drawing.Size(240, 37)
        Me.lblTimesHeading.TabIndex = 0
        Me.lblTimesHeading.Text = "Pick a showing"
        '
        'lblTimesFilm
        '
        Me.lblTimesFilm.AutoSize = True
        Me.lblTimesFilm.Font = New System.Drawing.Font("Segoe UI", 13.0!)
        Me.lblTimesFilm.Location = New System.Drawing.Point(36, 68)
        Me.lblTimesFilm.Name = "lblTimesFilm"
        Me.lblTimesFilm.Size = New System.Drawing.Size(160, 30)
        Me.lblTimesFilm.TabIndex = 1
        Me.lblTimesFilm.Text = "Film"
        '
        'pnlTimeList
        '
        Me.pnlTimeList.AutoScroll = True
        Me.pnlTimeList.Location = New System.Drawing.Point(20, 116)
        Me.pnlTimeList.Name = "pnlTimeList"
        Me.pnlTimeList.Size = New System.Drawing.Size(1240, 460)
        Me.pnlTimeList.TabIndex = 2
        '
        'frmKiosk
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1280, 800)
        Me.Controls.Add(Me.pnlTimes)
        Me.Controls.Add(Me.pnlFilms)
        Me.Controls.Add(Me.pnlWelcome)
        Me.Controls.Add(Me.pnlFooter)
        Me.Controls.Add(Me.pnlHeader)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "frmKiosk"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Filmtopia Kiosk"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.pnlHeader.ResumeLayout(False)
        Me.pnlHeader.PerformLayout()
        Me.pnlFooter.ResumeLayout(False)
        Me.pnlFooter.PerformLayout()
        Me.pnlWelcome.ResumeLayout(False)
        Me.pnlWelcome.PerformLayout()
        Me.pnlFilms.ResumeLayout(False)
        Me.pnlFilms.PerformLayout()
        Me.pnlTimes.ResumeLayout(False)
        Me.pnlTimes.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents pnlHeader As Panel
    Friend WithEvents lblKioskTitle As Label
    Friend WithEvents lblStep As Label
    Friend WithEvents btnExitKiosk As Button
    Friend WithEvents lblVersion As Label
    Friend WithEvents pnlFooter As Panel
    Friend WithEvents btnBack As Button
    Friend WithEvents btnNext As Button
    Friend WithEvents lblRunningTotal As Label
    Friend WithEvents pnlWelcome As Panel
    Friend WithEvents lblWelcomeTitle As Label
    Friend WithEvents lblWelcomeSub As Label
    Friend WithEvents btnStart As Button
    Friend WithEvents pnlFilms As Panel
    Friend WithEvents lblFilmsHeading As Label
    Friend WithEvents lblNoFilms As Label
    Friend WithEvents pnlFilmList As Panel
    Friend WithEvents pnlTimes As Panel
    Friend WithEvents lblTimesHeading As Label
    Friend WithEvents lblTimesFilm As Label
    Friend WithEvents pnlTimeList As Panel
End Class
