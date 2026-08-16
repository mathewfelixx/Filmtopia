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
        Me.components = New System.ComponentModel.Container()
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
        Me.pnlDayPicker = New System.Windows.Forms.Panel()
        Me.pnlTimes = New System.Windows.Forms.Panel()
        Me.lblTimesHeading = New System.Windows.Forms.Label()
        Me.lblTimesFilm = New System.Windows.Forms.Label()
        Me.pnlTimeList = New System.Windows.Forms.Panel()
        Me.pnlSeats = New System.Windows.Forms.Panel()
        Me.lblSeatsHeading = New System.Windows.Forms.Label()
        Me.lblSeatsShowing = New System.Windows.Forms.Label()
        Me.lblScreen = New System.Windows.Forms.Label()
        Me.pnlSeatMap = New System.Windows.Forms.Panel()
        Me.lblSwatchAvailable = New System.Windows.Forms.Label()
        Me.lblKeyAvailable = New System.Windows.Forms.Label()
        Me.lblSwatchSelected = New System.Windows.Forms.Label()
        Me.lblKeySelected = New System.Windows.Forms.Label()
        Me.lblSwatchTaken = New System.Windows.Forms.Label()
        Me.lblKeyTaken = New System.Windows.Forms.Label()
        Me.lblSwatchPremium = New System.Windows.Forms.Label()
        Me.lblKeyPremium = New System.Windows.Forms.Label()
        Me.lblSwatchAccessible = New System.Windows.Forms.Label()
        Me.lblKeyAccessible = New System.Windows.Forms.Label()
        Me.lblSwatchSaver = New System.Windows.Forms.Label()
        Me.lblKeySaver = New System.Windows.Forms.Label()
        Me.lblSeatsPicked = New System.Windows.Forms.Label()
        Me.lblSeatKeyTypes = New System.Windows.Forms.Label()
        Me.pnlConfirm = New System.Windows.Forms.Panel()
        Me.lblConfirmHeading = New System.Windows.Forms.Label()
        Me.lblConfirmDetail = New System.Windows.Forms.Label()
        Me.lblConfirmTotal = New System.Windows.Forms.Label()
        Me.lblConfirmNote = New System.Windows.Forms.Label()
        Me.pnlDone = New System.Windows.Forms.Panel()
        Me.lblDoneHeading = New System.Windows.Forms.Label()
        Me.lblDoneRef = New System.Windows.Forms.Label()
        Me.lblDoneDetail = New System.Windows.Forms.Label()
        Me.lblDoneNote = New System.Windows.Forms.Label()
        Me.timerIdle = New System.Windows.Forms.Timer(Me.components)
        Me.pnlFood = New System.Windows.Forms.Panel()
        Me.lblFoodHeading = New System.Windows.Forms.Label()
        Me.lblFoodSub = New System.Windows.Forms.Label()
        Me.pnlFoodList = New System.Windows.Forms.Panel()
        Me.lblFoodOrder = New System.Windows.Forms.Label()
        Me.pnlHeader.SuspendLayout()
        Me.pnlFooter.SuspendLayout()
        Me.pnlWelcome.SuspendLayout()
        Me.pnlFilms.SuspendLayout()
        Me.pnlTimes.SuspendLayout()
        Me.pnlSeats.SuspendLayout()
        Me.pnlConfirm.SuspendLayout()
        Me.pnlDone.SuspendLayout()
        Me.pnlFood.SuspendLayout()
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
        Me.pnlFilms.Controls.Add(Me.pnlDayPicker)
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
        'pnlDayPicker
        '
        Me.pnlDayPicker.Location = New System.Drawing.Point(20, 70)
        Me.pnlDayPicker.Name = "pnlDayPicker"
        Me.pnlDayPicker.Size = New System.Drawing.Size(1240, 76)
        Me.pnlDayPicker.TabIndex = 3
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
        'pnlSeats
        '
        Me.pnlSeats.Controls.Add(Me.lblSeatKeyTypes)
        Me.pnlSeats.Controls.Add(Me.lblSeatsPicked)
        Me.pnlSeats.Controls.Add(Me.lblKeySaver)
        Me.pnlSeats.Controls.Add(Me.lblSwatchSaver)
        Me.pnlSeats.Controls.Add(Me.lblKeyAccessible)
        Me.pnlSeats.Controls.Add(Me.lblSwatchAccessible)
        Me.pnlSeats.Controls.Add(Me.lblKeyPremium)
        Me.pnlSeats.Controls.Add(Me.lblSwatchPremium)
        Me.pnlSeats.Controls.Add(Me.lblKeyTaken)
        Me.pnlSeats.Controls.Add(Me.lblSwatchTaken)
        Me.pnlSeats.Controls.Add(Me.lblKeySelected)
        Me.pnlSeats.Controls.Add(Me.lblSwatchSelected)
        Me.pnlSeats.Controls.Add(Me.lblKeyAvailable)
        Me.pnlSeats.Controls.Add(Me.lblSwatchAvailable)
        Me.pnlSeats.Controls.Add(Me.pnlSeatMap)
        Me.pnlSeats.Controls.Add(Me.lblScreen)
        Me.pnlSeats.Controls.Add(Me.lblSeatsShowing)
        Me.pnlSeats.Controls.Add(Me.lblSeatsHeading)
        Me.pnlSeats.Location = New System.Drawing.Point(0, 96)
        Me.pnlSeats.Name = "pnlSeats"
        Me.pnlSeats.Size = New System.Drawing.Size(1280, 596)
        Me.pnlSeats.TabIndex = 6
        Me.pnlSeats.Visible = False
        '
        'lblSeatsHeading
        '
        Me.lblSeatsHeading.AutoSize = True
        Me.lblSeatsHeading.Font = New System.Drawing.Font("Segoe UI", 20.0!, System.Drawing.FontStyle.Bold)
        Me.lblSeatsHeading.Location = New System.Drawing.Point(32, 20)
        Me.lblSeatsHeading.Name = "lblSeatsHeading"
        Me.lblSeatsHeading.Size = New System.Drawing.Size(280, 37)
        Me.lblSeatsHeading.TabIndex = 0
        Me.lblSeatsHeading.Text = "Choose your seats"
        '
        'lblSeatsShowing
        '
        Me.lblSeatsShowing.AutoSize = True
        Me.lblSeatsShowing.Font = New System.Drawing.Font("Segoe UI", 13.0!)
        Me.lblSeatsShowing.Location = New System.Drawing.Point(36, 62)
        Me.lblSeatsShowing.Name = "lblSeatsShowing"
        Me.lblSeatsShowing.Size = New System.Drawing.Size(200, 30)
        Me.lblSeatsShowing.TabIndex = 1
        Me.lblSeatsShowing.Text = "Showing"
        '
        'lblScreen
        '
        Me.lblScreen.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.lblScreen.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold)
        Me.lblScreen.ForeColor = System.Drawing.Color.White
        Me.lblScreen.Location = New System.Drawing.Point(330, 106)
        Me.lblScreen.Name = "lblScreen"
        Me.lblScreen.Size = New System.Drawing.Size(620, 34)
        Me.lblScreen.TabIndex = 2
        Me.lblScreen.Text = "SCREEN"
        Me.lblScreen.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'pnlSeatMap
        '
        Me.pnlSeatMap.AutoScroll = True
        Me.pnlSeatMap.Location = New System.Drawing.Point(330, 156)
        Me.pnlSeatMap.Name = "pnlSeatMap"
        Me.pnlSeatMap.Size = New System.Drawing.Size(620, 380)
        Me.pnlSeatMap.TabIndex = 3
        '
        'lblSwatchAvailable
        '
        Me.lblSwatchAvailable.Location = New System.Drawing.Point(36, 500)
        Me.lblSwatchAvailable.Name = "lblSwatchAvailable"
        Me.lblSwatchAvailable.Size = New System.Drawing.Size(28, 28)
        Me.lblSwatchAvailable.TabIndex = 4
        '
        'lblKeyAvailable
        '
        Me.lblKeyAvailable.AutoSize = True
        Me.lblKeyAvailable.Font = New System.Drawing.Font("Segoe UI", 11.0!)
        Me.lblKeyAvailable.Location = New System.Drawing.Point(74, 503)
        Me.lblKeyAvailable.Name = "lblKeyAvailable"
        Me.lblKeyAvailable.Size = New System.Drawing.Size(70, 25)
        Me.lblKeyAvailable.TabIndex = 5
        Me.lblKeyAvailable.Text = "Free"
        '
        'lblSwatchSelected
        '
        Me.lblSwatchSelected.Location = New System.Drawing.Point(160, 500)
        Me.lblSwatchSelected.Name = "lblSwatchSelected"
        Me.lblSwatchSelected.Size = New System.Drawing.Size(28, 28)
        Me.lblSwatchSelected.TabIndex = 6
        '
        'lblKeySelected
        '
        Me.lblKeySelected.AutoSize = True
        Me.lblKeySelected.Font = New System.Drawing.Font("Segoe UI", 11.0!)
        Me.lblKeySelected.Location = New System.Drawing.Point(198, 503)
        Me.lblKeySelected.Name = "lblKeySelected"
        Me.lblKeySelected.Size = New System.Drawing.Size(80, 25)
        Me.lblKeySelected.TabIndex = 7
        Me.lblKeySelected.Text = "Yours"
        '
        'lblSwatchTaken
        '
        Me.lblSwatchTaken.Location = New System.Drawing.Point(36, 544)
        Me.lblSwatchTaken.Name = "lblSwatchTaken"
        Me.lblSwatchTaken.Size = New System.Drawing.Size(28, 28)
        Me.lblSwatchTaken.TabIndex = 8
        '
        'lblKeyTaken
        '
        Me.lblKeyTaken.AutoSize = True
        Me.lblKeyTaken.Font = New System.Drawing.Font("Segoe UI", 11.0!)
        Me.lblKeyTaken.Location = New System.Drawing.Point(74, 547)
        Me.lblKeyTaken.Name = "lblKeyTaken"
        Me.lblKeyTaken.Size = New System.Drawing.Size(80, 25)
        Me.lblKeyTaken.TabIndex = 9
        Me.lblKeyTaken.Text = "Taken"
        '
        'lblSwatchPremium
        '
        Me.lblSwatchPremium.Location = New System.Drawing.Point(160, 544)
        Me.lblSwatchPremium.Name = "lblSwatchPremium"
        Me.lblSwatchPremium.Size = New System.Drawing.Size(28, 28)
        Me.lblSwatchPremium.TabIndex = 12
        '
        'lblKeyPremium
        '
        Me.lblKeyPremium.AutoSize = True
        Me.lblKeyPremium.Font = New System.Drawing.Font("Segoe UI", 11.0!)
        Me.lblKeyPremium.Location = New System.Drawing.Point(198, 547)
        Me.lblKeyPremium.Name = "lblKeyPremium"
        Me.lblKeyPremium.Size = New System.Drawing.Size(90, 25)
        Me.lblKeyPremium.TabIndex = 13
        Me.lblKeyPremium.Text = "Premium"
        '
        'lblSwatchAccessible
        '
        Me.lblSwatchAccessible.Location = New System.Drawing.Point(320, 544)
        Me.lblSwatchAccessible.Name = "lblSwatchAccessible"
        Me.lblSwatchAccessible.Size = New System.Drawing.Size(28, 28)
        Me.lblSwatchAccessible.TabIndex = 14
        '
        'lblKeyAccessible
        '
        Me.lblKeyAccessible.AutoSize = True
        Me.lblKeyAccessible.Font = New System.Drawing.Font("Segoe UI", 11.0!)
        Me.lblKeyAccessible.Location = New System.Drawing.Point(358, 547)
        Me.lblKeyAccessible.Name = "lblKeyAccessible"
        Me.lblKeyAccessible.Size = New System.Drawing.Size(100, 25)
        Me.lblKeyAccessible.TabIndex = 15
        Me.lblKeyAccessible.Text = "Accessible"
        '
        'lblSwatchSaver
        '
        Me.lblSwatchSaver.Location = New System.Drawing.Point(490, 544)
        Me.lblSwatchSaver.Name = "lblSwatchSaver"
        Me.lblSwatchSaver.Size = New System.Drawing.Size(28, 28)
        Me.lblSwatchSaver.TabIndex = 16
        '
        'lblKeySaver
        '
        Me.lblKeySaver.AutoSize = True
        Me.lblKeySaver.Font = New System.Drawing.Font("Segoe UI", 11.0!)
        Me.lblKeySaver.Location = New System.Drawing.Point(528, 547)
        Me.lblKeySaver.Name = "lblKeySaver"
        Me.lblKeySaver.Size = New System.Drawing.Size(65, 25)
        Me.lblKeySaver.TabIndex = 17
        Me.lblKeySaver.Text = "Saver"
        '
        'lblSeatsPicked
        '
        Me.lblSeatsPicked.Font = New System.Drawing.Font("Segoe UI", 13.0!)
        Me.lblSeatsPicked.Location = New System.Drawing.Point(36, 120)
        Me.lblSeatsPicked.Name = "lblSeatsPicked"
        Me.lblSeatsPicked.Size = New System.Drawing.Size(270, 340)
        Me.lblSeatsPicked.TabIndex = 10
        Me.lblSeatsPicked.Text = "No seats picked yet"
        '
        'lblSeatKeyTypes
        '
        Me.lblSeatKeyTypes.AutoSize = True
        Me.lblSeatKeyTypes.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.lblSeatKeyTypes.ForeColor = System.Drawing.Color.Gray
        Me.lblSeatKeyTypes.Location = New System.Drawing.Point(330, 570)
        Me.lblSeatKeyTypes.Name = "lblSeatKeyTypes"
        Me.lblSeatKeyTypes.Size = New System.Drawing.Size(560, 23)
        Me.lblSeatKeyTypes.TabIndex = 11
        Me.lblSeatKeyTypes.Text = "A premium seat costs more than a standard one. An accessible seat is the standard price."
        '
        'pnlConfirm
        '
        Me.pnlConfirm.Controls.Add(Me.lblConfirmNote)
        Me.pnlConfirm.Controls.Add(Me.lblConfirmTotal)
        Me.pnlConfirm.Controls.Add(Me.lblConfirmDetail)
        Me.pnlConfirm.Controls.Add(Me.lblConfirmHeading)
        Me.pnlConfirm.Location = New System.Drawing.Point(0, 96)
        Me.pnlConfirm.Name = "pnlConfirm"
        Me.pnlConfirm.Size = New System.Drawing.Size(1280, 596)
        Me.pnlConfirm.TabIndex = 7
        Me.pnlConfirm.Visible = False
        '
        'lblConfirmHeading
        '
        Me.lblConfirmHeading.AutoSize = True
        Me.lblConfirmHeading.Font = New System.Drawing.Font("Segoe UI", 20.0!, System.Drawing.FontStyle.Bold)
        Me.lblConfirmHeading.Location = New System.Drawing.Point(32, 20)
        Me.lblConfirmHeading.Name = "lblConfirmHeading"
        Me.lblConfirmHeading.Size = New System.Drawing.Size(280, 37)
        Me.lblConfirmHeading.TabIndex = 0
        Me.lblConfirmHeading.Text = "Check your order"
        '
        'lblConfirmDetail
        '
        Me.lblConfirmDetail.Font = New System.Drawing.Font("Segoe UI", 14.0!)
        Me.lblConfirmDetail.Location = New System.Drawing.Point(36, 80)
        Me.lblConfirmDetail.Name = "lblConfirmDetail"
        Me.lblConfirmDetail.Size = New System.Drawing.Size(700, 380)
        Me.lblConfirmDetail.TabIndex = 1
        Me.lblConfirmDetail.Text = "Order"
        '
        'lblConfirmTotal
        '
        Me.lblConfirmTotal.AutoSize = True
        Me.lblConfirmTotal.Font = New System.Drawing.Font("Segoe UI", 28.0!, System.Drawing.FontStyle.Bold)
        Me.lblConfirmTotal.Location = New System.Drawing.Point(36, 470)
        Me.lblConfirmTotal.Name = "lblConfirmTotal"
        Me.lblConfirmTotal.Size = New System.Drawing.Size(300, 50)
        Me.lblConfirmTotal.TabIndex = 2
        Me.lblConfirmTotal.Text = "To pay  £0.00"
        '
        'lblConfirmNote
        '
        Me.lblConfirmNote.AutoSize = True
        Me.lblConfirmNote.Font = New System.Drawing.Font("Segoe UI", 12.0!)
        Me.lblConfirmNote.ForeColor = System.Drawing.Color.Gray
        Me.lblConfirmNote.Location = New System.Drawing.Point(38, 530)
        Me.lblConfirmNote.Name = "lblConfirmNote"
        Me.lblConfirmNote.Size = New System.Drawing.Size(520, 28)
        Me.lblConfirmNote.TabIndex = 3
        Me.lblConfirmNote.Text = "Your seats are not held until you have paid"
        '
        'pnlDone
        '
        Me.pnlDone.Controls.Add(Me.lblDoneNote)
        Me.pnlDone.Controls.Add(Me.lblDoneDetail)
        Me.pnlDone.Controls.Add(Me.lblDoneRef)
        Me.pnlDone.Controls.Add(Me.lblDoneHeading)
        Me.pnlDone.Location = New System.Drawing.Point(0, 96)
        Me.pnlDone.Name = "pnlDone"
        Me.pnlDone.Size = New System.Drawing.Size(1280, 596)
        Me.pnlDone.TabIndex = 8
        Me.pnlDone.Visible = False
        '
        'lblDoneHeading
        '
        Me.lblDoneHeading.AutoSize = True
        Me.lblDoneHeading.Font = New System.Drawing.Font("Segoe UI", 34.0!, System.Drawing.FontStyle.Bold)
        Me.lblDoneHeading.Location = New System.Drawing.Point(400, 90)
        Me.lblDoneHeading.Name = "lblDoneHeading"
        Me.lblDoneHeading.Size = New System.Drawing.Size(320, 61)
        Me.lblDoneHeading.TabIndex = 0
        Me.lblDoneHeading.Text = "Thank you"
        '
        'lblDoneRef
        '
        Me.lblDoneRef.AutoSize = True
        Me.lblDoneRef.Font = New System.Drawing.Font("Segoe UI", 22.0!, System.Drawing.FontStyle.Bold)
        Me.lblDoneRef.Location = New System.Drawing.Point(400, 170)
        Me.lblDoneRef.Name = "lblDoneRef"
        Me.lblDoneRef.Size = New System.Drawing.Size(300, 41)
        Me.lblDoneRef.TabIndex = 1
        Me.lblDoneRef.Text = "Booking"
        '
        'lblDoneDetail
        '
        Me.lblDoneDetail.AutoSize = True
        Me.lblDoneDetail.Font = New System.Drawing.Font("Segoe UI", 15.0!)
        Me.lblDoneDetail.Location = New System.Drawing.Point(400, 230)
        Me.lblDoneDetail.Name = "lblDoneDetail"
        Me.lblDoneDetail.Size = New System.Drawing.Size(400, 34)
        Me.lblDoneDetail.TabIndex = 2
        Me.lblDoneDetail.Text = "Details"
        '
        'lblDoneNote
        '
        Me.lblDoneNote.AutoSize = True
        Me.lblDoneNote.Font = New System.Drawing.Font("Segoe UI", 14.0!)
        Me.lblDoneNote.ForeColor = System.Drawing.Color.Gray
        Me.lblDoneNote.Location = New System.Drawing.Point(400, 380)
        Me.lblDoneNote.Name = "lblDoneNote"
        Me.lblDoneNote.Size = New System.Drawing.Size(500, 32)
        Me.lblDoneNote.TabIndex = 3
        Me.lblDoneNote.Text = "Please take your tickets from the slot below"
        '
        'timerIdle
        '
        Me.timerIdle.Interval = 1000
        '
        'pnlFood
        '
        Me.pnlFood.Controls.Add(Me.lblFoodOrder)
        Me.pnlFood.Controls.Add(Me.pnlFoodList)
        Me.pnlFood.Controls.Add(Me.lblFoodSub)
        Me.pnlFood.Controls.Add(Me.lblFoodHeading)
        Me.pnlFood.Location = New System.Drawing.Point(0, 96)
        Me.pnlFood.Name = "pnlFood"
        Me.pnlFood.Size = New System.Drawing.Size(1280, 596)
        Me.pnlFood.TabIndex = 9
        Me.pnlFood.Visible = False
        '
        'lblFoodHeading
        '
        Me.lblFoodHeading.AutoSize = True
        Me.lblFoodHeading.Font = New System.Drawing.Font("Segoe UI", 20.0!, System.Drawing.FontStyle.Bold)
        Me.lblFoodHeading.Location = New System.Drawing.Point(32, 20)
        Me.lblFoodHeading.Name = "lblFoodHeading"
        Me.lblFoodHeading.Size = New System.Drawing.Size(320, 37)
        Me.lblFoodHeading.TabIndex = 0
        Me.lblFoodHeading.Text = "Anything to eat or drink?"
        '
        'lblFoodSub
        '
        Me.lblFoodSub.AutoSize = True
        Me.lblFoodSub.Font = New System.Drawing.Font("Segoe UI", 13.0!)
        Me.lblFoodSub.ForeColor = System.Drawing.Color.Gray
        Me.lblFoodSub.Location = New System.Drawing.Point(36, 62)
        Me.lblFoodSub.Name = "lblFoodSub"
        Me.lblFoodSub.Size = New System.Drawing.Size(400, 30)
        Me.lblFoodSub.TabIndex = 1
        Me.lblFoodSub.Text = "Touch an item to add one, or just carry on"
        '
        'pnlFoodList
        '
        Me.pnlFoodList.AutoScroll = True
        Me.pnlFoodList.Location = New System.Drawing.Point(330, 116)
        Me.pnlFoodList.Name = "pnlFoodList"
        Me.pnlFoodList.Size = New System.Drawing.Size(920, 440)
        Me.pnlFoodList.TabIndex = 2
        '
        'lblFoodOrder
        '
        Me.lblFoodOrder.Font = New System.Drawing.Font("Segoe UI", 13.0!)
        Me.lblFoodOrder.Location = New System.Drawing.Point(36, 116)
        Me.lblFoodOrder.Name = "lblFoodOrder"
        Me.lblFoodOrder.Size = New System.Drawing.Size(270, 440)
        Me.lblFoodOrder.TabIndex = 3
        Me.lblFoodOrder.Text = "Nothing added yet"
        '
        'frmKiosk
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1280, 800)
        Me.Controls.Add(Me.pnlFood)
        Me.Controls.Add(Me.pnlDone)
        Me.Controls.Add(Me.pnlConfirm)
        Me.Controls.Add(Me.pnlSeats)
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
        Me.pnlSeats.ResumeLayout(False)
        Me.pnlSeats.PerformLayout()
        Me.pnlConfirm.ResumeLayout(False)
        Me.pnlConfirm.PerformLayout()
        Me.pnlDone.ResumeLayout(False)
        Me.pnlDone.PerformLayout()
        Me.pnlFood.ResumeLayout(False)
        Me.pnlFood.PerformLayout()
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
    Friend WithEvents pnlDayPicker As Panel
    Friend WithEvents pnlTimes As Panel
    Friend WithEvents lblTimesHeading As Label
    Friend WithEvents lblTimesFilm As Label
    Friend WithEvents pnlTimeList As Panel
    Friend WithEvents pnlSeats As Panel
    Friend WithEvents lblSeatsHeading As Label
    Friend WithEvents lblSeatsShowing As Label
    Friend WithEvents lblScreen As Label
    Friend WithEvents pnlSeatMap As Panel
    Friend WithEvents lblSwatchAvailable As Label
    Friend WithEvents lblKeyAvailable As Label
    Friend WithEvents lblSwatchSelected As Label
    Friend WithEvents lblKeySelected As Label
    Friend WithEvents lblSwatchTaken As Label
    Friend WithEvents lblKeyTaken As Label
    Friend WithEvents lblSwatchPremium As Label
    Friend WithEvents lblKeyPremium As Label
    Friend WithEvents lblSwatchAccessible As Label
    Friend WithEvents lblKeyAccessible As Label
    Friend WithEvents lblSwatchSaver As Label
    Friend WithEvents lblKeySaver As Label
    Friend WithEvents lblSeatsPicked As Label
    Friend WithEvents lblSeatKeyTypes As Label
    Friend WithEvents pnlConfirm As Panel
    Friend WithEvents lblConfirmHeading As Label
    Friend WithEvents lblConfirmDetail As Label
    Friend WithEvents lblConfirmTotal As Label
    Friend WithEvents lblConfirmNote As Label
    Friend WithEvents pnlDone As Panel
    Friend WithEvents lblDoneHeading As Label
    Friend WithEvents lblDoneRef As Label
    Friend WithEvents lblDoneDetail As Label
    Friend WithEvents lblDoneNote As Label
    Friend WithEvents timerIdle As Timer
    Friend WithEvents pnlFood As Panel
    Friend WithEvents lblFoodHeading As Label
    Friend WithEvents lblFoodSub As Label
    Friend WithEvents pnlFoodList As Panel
    Friend WithEvents lblFoodOrder As Label
End Class
