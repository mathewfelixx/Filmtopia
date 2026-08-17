Imports System.Data.OleDb

Public Class frmKiosk

    Private Const TileWidth As Integer = 380
    Private Const TileHeight As Integer = 170
    Private Const TileGap As Integer = 20

    Private Const PosterWidth As Integer = 92
    Private Const PosterHeight As Integer = 142

    Private Const TextLeftWithPoster As Integer = 126
    Private Const TextLeftNoPoster As Integer = 26

    Private Const FoodTileWidth As Integer = 280
    Private Const FoodTileHeight As Integer = 130

    Private Const FoodPictureSize As Integer = 56

    Private Const TextLeftWithPicture As Integer = 84
    Private Const TextLeftNoPicture As Integer = 24

    Private Const TimeTileWidth As Integer = 240
    Private Const TimeTileHeight As Integer = 140

    Private Const SeatWidth As Integer = 54
    Private Const SeatHeight As Integer = 46
    Private Const SeatGap As Integer = 8

    Private Const SmallestSeatWidth As Integer = 34

    Private Const SeatMapMargin As Integer = 24

    Private Const LeftColumnWidth As Integer = 330

    Private Const FooterHeight As Integer = 104
    Private Const FooterButtonHeight As Integer = 72

    Private Const DaysAhead As Integer = 7

    Private Const StepWelcome As String = "WELCOME"
    Private Const StepFilms As String = "FILMS"
    Private Const StepTimes As String = "TIMES"
    Private Const StepSeats As String = "SEATS"
    Private Const StepFood As String = "FOOD"
    Private Const StepConfirm As String = "CONFIRM"
    Private Const StepDone As String = "DONE"

    Private currentStep As String = StepWelcome

    Private secondsIdle As Integer = 0

    Private currentDay As Date = Date.Today

    Private currentFilmID As Long = 0
    Private currentFilmTitle As String = ""
    Private currentFilmRating As String = ""
    Private currentFilmDuration As Integer = 0
    Private currentFilmYear As String = ""
    Private currentFilmGenres As String = ""
    Private currentFilmSynopsis As String = ""
    Private currentFilmPoster As String = ""

    Private currentScreeningID As Long = 0
    Private currentScreenID As Long = 0
    Private currentTicketPrice As Double = 0
    Private currentShowingText As String = ""

    Private currentSeats As DataTable

    Private pickedSeats As DataTable

    Private pendingFood As DataTable

    Private foodOnSale As DataTable

    Private seatDrawWidth As Integer = SeatWidth
    Private seatDrawHeight As Integer = SeatHeight

    Private Sub frmKiosk_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CommonFormStartup(Me)
        SetUpPickedSeats()
        SetUpPendingFood()
        LayoutKiosk()
        ShowStep(StepWelcome)
        timerIdle.Start()
        WriteLog("KIOSK", "Kiosk opened")
    End Sub

    Private Sub LayoutKiosk()
        LayoutHeader()
        LayoutFooter()

        Dim contentTop As Integer = pnlHeader.Height
        Dim contentHeight As Integer = pnlFooter.Top - contentTop

        SizeStepPanel(pnlWelcome, contentTop, contentHeight)
        SizeStepPanel(pnlFilms, contentTop, contentHeight)
        SizeStepPanel(pnlTimes, contentTop, contentHeight)
        SizeStepPanel(pnlSeats, contentTop, contentHeight)
        SizeStepPanel(pnlFood, contentTop, contentHeight)
        SizeStepPanel(pnlConfirm, contentTop, contentHeight)
        SizeStepPanel(pnlDone, contentTop, contentHeight)

        CentreWelcome()
        LayoutFilmsStep()
        LayoutTimesStep()
        LayoutSeatsStep()
        LayoutFoodStep()
        LayoutConfirmStep()
        LayoutDoneStep()
    End Sub

    Private Sub LayoutHeader()
        pnlHeader.Width = Me.ClientSize.Width

        lblKioskTitle.Top = 12
        lblStep.Top = lblKioskTitle.Bottom + 4
        pnlHeader.Height = lblStep.Bottom + 14

        btnExitKiosk.Left = pnlHeader.Width - btnExitKiosk.Width - 30
        btnExitKiosk.Top = (pnlHeader.Height - btnExitKiosk.Height) \ 2
    End Sub

    Private Sub LayoutFooter()
        pnlFooter.Height = FooterHeight
        pnlFooter.Width = Me.ClientSize.Width
        pnlFooter.Top = Me.ClientSize.Height - pnlFooter.Height

        btnBack.Size = New Size(220, FooterButtonHeight)
        btnNext.Size = New Size(240, FooterButtonHeight)

        btnBack.Left = 32
        btnBack.Top = (pnlFooter.Height - btnBack.Height) \ 2

        btnNext.Left = pnlFooter.Width - btnNext.Width - 32
        btnNext.Top = btnBack.Top
        lblRunningTotal.Left = btnNext.Left - lblRunningTotal.Width - 40
        lblRunningTotal.Top = (pnlFooter.Height - lblRunningTotal.Height) \ 2

        lblVersion.Left = btnBack.Right + 40
        lblVersion.Top = pnlFooter.Height - lblVersion.Height - 12
    End Sub

    Private Sub LayoutFilmsStep()
        pnlDayPicker.Top = lblFilmsHeading.Bottom + 16
        pnlDayPicker.Width = pnlFilms.Width - 40
        ArrangeDayButtons()

        lblNoFilms.Top = pnlDayPicker.Bottom + 30

        pnlFilmList.Top = pnlDayPicker.Bottom + 20
        pnlFilmList.Width = pnlFilms.Width - 40
        pnlFilmList.Height = pnlFilms.Height - pnlFilmList.Top - 20
        ArrangeFilmTiles()
    End Sub

    Private Sub LayoutTimesStep()
        lblTimesFilm.Top = lblTimesHeading.Bottom + 6

        pnlTimeList.Top = lblTimesFilm.Bottom + 20
        pnlTimeList.Width = pnlTimes.Width - 40
        pnlTimeList.Height = pnlTimes.Height - pnlTimeList.Top - 20
        ArrangeTimeTiles()
    End Sub

    Private Sub LayoutFoodStep()
        lblFoodSub.Top = lblFoodHeading.Bottom + 6

        lblFoodOrder.Top = lblFoodSub.Bottom + 24
        lblFoodOrder.Height = pnlFood.Height - lblFoodOrder.Top - 20

        pnlFoodList.Left = LeftColumnWidth
        pnlFoodList.Top = lblFoodOrder.Top
        pnlFoodList.Width = pnlFood.Width - LeftColumnWidth - 30
        pnlFoodList.Height = pnlFood.Height - pnlFoodList.Top - 20
        ArrangeTiles(pnlFoodList, FoodTileWidth, FoodTileHeight)
    End Sub

    Private Sub LayoutConfirmStep()
        lblConfirmDetail.Top = lblConfirmHeading.Bottom + 20
        lblConfirmDetail.Height = pnlConfirm.Height - lblConfirmDetail.Top - 130

        lblConfirmTotal.Top = lblConfirmDetail.Bottom + 10
        lblConfirmNote.Top = lblConfirmTotal.Bottom + 8
    End Sub

    Private Sub LayoutDoneStep()
        lblDoneHeading.Left = (pnlDone.Width - lblDoneHeading.Width) \ 2
        lblDoneHeading.Top = (pnlDone.Height \ 2) - 190

        lblDoneRef.Left = (pnlDone.Width - lblDoneRef.Width) \ 2
        lblDoneRef.Top = lblDoneHeading.Bottom + 24

        lblDoneDetail.Left = (pnlDone.Width - lblDoneDetail.Width) \ 2
        lblDoneDetail.Top = lblDoneRef.Bottom + 30

        lblDoneNote.Left = (pnlDone.Width - lblDoneNote.Width) \ 2
        lblDoneNote.Top = lblDoneDetail.Bottom + 50
    End Sub

    Private Sub LayoutSeatsStep()
        lblSeatsShowing.Top = lblSeatsHeading.Bottom + 6

        lblSeatsPicked.Top = lblSeatsShowing.Bottom + 24
        lblSeatsPicked.Height = pnlSeats.Height - lblSeatsPicked.Top - 30

        If currentSeats IsNot Nothing Then
            CentreSeatMap()
        End If

        lblSwatchAvailable.Left = pnlSeatMap.Left
        lblKeyAvailable.Left = lblSwatchAvailable.Right + 12
        lblSwatchSelected.Left = lblKeyAvailable.Right + 40
        lblKeySelected.Left = lblSwatchSelected.Right + 12
        lblSwatchTaken.Left = lblKeySelected.Right + 40
        lblKeyTaken.Left = lblSwatchTaken.Right + 12

        lblSwatchAvailable.Top = pnlSeatMap.Bottom + 18
        lblSwatchSelected.Top = lblSwatchAvailable.Top
        lblSwatchTaken.Top = lblSwatchAvailable.Top

        lblKeyAvailable.Top = lblSwatchAvailable.Top + 2
        lblKeySelected.Top = lblKeyAvailable.Top
        lblKeyTaken.Top = lblKeyAvailable.Top

        lblSwatchPremium.Left = pnlSeatMap.Left
        lblKeyPremium.Left = lblSwatchPremium.Right + 12
        lblSwatchAccessible.Left = lblKeyPremium.Right + 40
        lblKeyAccessible.Left = lblSwatchAccessible.Right + 12
        lblSwatchSaver.Left = lblKeyAccessible.Right + 40
        lblKeySaver.Left = lblSwatchSaver.Right + 12

        lblSwatchPremium.Top = lblSwatchAvailable.Bottom + 12
        lblSwatchAccessible.Top = lblSwatchPremium.Top
        lblSwatchSaver.Top = lblSwatchPremium.Top

        lblKeyPremium.Top = lblSwatchPremium.Top + 2
        lblKeyAccessible.Top = lblKeyPremium.Top
        lblKeySaver.Top = lblKeyPremium.Top

        lblSeatKeyTypes.Left = pnlSeatMap.Left
        lblSeatKeyTypes.Top = lblKeyPremium.Bottom + 10
    End Sub

    Private Sub SizeStepPanel(pnl As Panel, contentTop As Integer, contentHeight As Integer)
        pnl.Left = 0
        pnl.Top = contentTop
        pnl.Width = Me.ClientSize.Width
        pnl.Height = contentHeight
    End Sub

    Private Sub CentreWelcome()
        lblWelcomeTitle.Left = (pnlWelcome.Width - lblWelcomeTitle.Width) \ 2
        lblWelcomeTitle.Top = (pnlWelcome.Height \ 2) - 170

        lblWelcomeSub.Left = (pnlWelcome.Width - lblWelcomeSub.Width) \ 2
        lblWelcomeSub.Top = lblWelcomeTitle.Bottom + 16

        btnStart.Size = New Size(460, 140)
        btnStart.Left = (pnlWelcome.Width - btnStart.Width) \ 2
        btnStart.Top = lblWelcomeSub.Bottom + 60
    End Sub

    Private Sub frmKiosk_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        StyleKioskButtons()

        LayoutKiosk()
    End Sub

    Private Sub ShowStep(stepName As String)
        currentStep = stepName
        Touched()

        pnlWelcome.Visible = (stepName = StepWelcome)
        pnlFilms.Visible = (stepName = StepFilms)
        pnlTimes.Visible = (stepName = StepTimes)
        pnlSeats.Visible = (stepName = StepSeats)
        pnlFood.Visible = (stepName = StepFood)
        pnlConfirm.Visible = (stepName = StepConfirm)
        pnlDone.Visible = (stepName = StepDone)

        If stepName = StepWelcome Then
            lblStep.Text = "Self service"
        ElseIf stepName = StepFilms Then
            lblStep.Text = "Step 1 of 5  -  choose a film"
        ElseIf stepName = StepTimes Then
            lblStep.Text = "Step 2 of 5  -  choose a showing"
        ElseIf stepName = StepSeats Then
            lblStep.Text = "Step 3 of 5  -  choose your seats"
        ElseIf stepName = StepFood Then
            lblStep.Text = "Step 4 of 5  -  food and drink"
        ElseIf stepName = StepConfirm Then
            lblStep.Text = "Step 5 of 5  -  pay"
        ElseIf stepName = StepDone Then
            lblStep.Text = "Self service"
        End If

        btnBack.Visible = (stepName <> StepWelcome And stepName <> StepDone)

        btnNext.Visible = (stepName = StepSeats Or stepName = StepFood Or
                          stepName = StepConfirm Or stepName = StepDone)
        lblRunningTotal.Visible = (stepName = StepSeats Or stepName = StepFood)

        If stepName = StepConfirm Then
            btnNext.Text = "Pay now"
            btnNext.Enabled = True
        ElseIf stepName = StepDone Then
            btnNext.Text = "Finish"
            btnNext.Enabled = True
        ElseIf stepName = StepFood Then
            btnNext.Text = NextTextForFood()
            btnNext.Enabled = True
        Else
            btnNext.Text = "Continue"
        End If

        LayoutKiosk()
    End Sub

    Private Sub LoadFilmsForDay()
        Dim dt As New DataTable

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT DISTINCT tblFilm.FilmID, FilmTitle, FilmAgeRating, FilmDuration, FilmPoster " &
                                 "FROM (tblFilm INNER JOIN tblScreening ON tblFilm.FilmID = tblScreening.FilmID) " &
                                 "INNER JOIN tblScreen ON tblScreening.ScreenID = tblScreen.ScreenID " &
                                 "WHERE ScreeningDate = @Day AND ScreeningTime >= @EarliestTime " &
                                 "AND (ScreeningStatus IS NULL OR ScreeningStatus <> 'Cancelled') " &
                                 "AND (ScreenStatus IS NULL OR ScreenStatus <> @OutOfService) " &
                                 "ORDER BY FilmTitle"
            SQLCmd.Parameters.AddWithValue("@Day", currentDay)
            SQLCmd.Parameters.AddWithValue("@EarliestTime", EarliestTimeForDay())
            SQLCmd.Parameters.AddWithValue("@OutOfService", ScreenOutOfService)
            Dim da As New OleDbDataAdapter(SQLCmd)
            da.Fill(dt)
            cn.Close()
        End If

        lblFilmsHeading.Text = HeadingForDay()
        BuildFilmTiles(dt)

        lblNoFilms.Visible = (dt.Rows.Count = 0)

        If currentDay = Date.Today Then
            lblNoFilms.Text = "There is nothing left on today, try another day"
        Else
            lblNoFilms.Text = "There is nothing on that day, try another one"
        End If
    End Sub

    Private Function HeadingForDay() As String
        If currentDay = Date.Today Then
            Return "What's on today"
        ElseIf currentDay = Date.Today.AddDays(1) Then
            Return "What's on tomorrow"
        End If

        Return "What's on " & Format(currentDay, "dddd d MMMM")
    End Function

    Private Function EarliestTimeForDay() As String
        If currentDay = Date.Today Then
            Return Format(Now, "HH:mm")
        End If

        Return "00:00"
    End Function

    Private Function DayName(theDay As Date) As String
        If theDay = Date.Today Then
            Return "Today"
        ElseIf theDay = Date.Today.AddDays(1) Then
            Return "Tomorrow"
        End If

        Return Format(theDay, "ddd d MMM")
    End Function

    Private Sub BuildDayPicker()
        ClearPanel(pnlDayPicker)

        Dim i As Integer

        For i = 0 To DaysAhead - 1
            Dim theDay As Date = Date.Today.AddDays(i)

            Dim b As New Button
            b.Tag = theDay
            b.Text = DayName(theDay)
            b.Font = New Font("Segoe UI", 11)
            b.FlatStyle = FlatStyle.Flat
            b.FlatAppearance.BorderColor = BorderCol

            If theDay = currentDay Then
                b.BackColor = HighlightBack
                b.ForeColor = HighlightFore
                b.Font = New Font("Segoe UI", 11, FontStyle.Bold)
            Else
                b.BackColor = CardBack
                b.ForeColor = TextFore
            End If

            AddHandler b.Click, AddressOf DayButton_Click
            pnlDayPicker.Controls.Add(b)
        Next

        ArrangeDayButtons()
    End Sub

    Private Sub ArrangeDayButtons()
        If pnlDayPicker.Controls.Count = 0 Then
            Exit Sub
        End If

        Dim gap As Integer = 8
        Dim buttonWidth As Integer = (pnlDayPicker.Width - (gap * (DaysAhead - 1))) \ DaysAhead

        Dim i As Integer
        For i = 0 To pnlDayPicker.Controls.Count - 1
            pnlDayPicker.Controls(i).Size = New Size(buttonWidth, 62)
            pnlDayPicker.Controls(i).Left = i * (buttonWidth + gap)
            pnlDayPicker.Controls(i).Top = 0
        Next
    End Sub

    Private Sub DayButton_Click(sender As Object, e As EventArgs)
        Touched()

        Dim b As Button = CType(sender, Button)
        currentDay = CDate(b.Tag)

        BuildDayPicker()
        LoadFilmsForDay()
        LayoutFilmsStep()
    End Sub

    Private Sub BuildFilmTiles(dtFilms As DataTable)
        ClearFilmTiles()

        Dim i As Integer
        For i = 0 To dtFilms.Rows.Count - 1
            Dim filmID As Long = CLng(dtFilms.Rows(i)("FilmID"))
            Dim title As String = dtFilms.Rows(i)("FilmTitle").ToString()
            Dim rating As String = dtFilms.Rows(i)("FilmAgeRating").ToString()
            Dim duration As Integer = CInt(dtFilms.Rows(i)("FilmDuration"))

            Dim poster As Image = SmallPicture("Posters", dtFilms.Rows(i)("FilmPoster").ToString(), PosterWidth, PosterHeight)
            Dim textLeft As Integer = TextLeftNoPoster

            If poster IsNot Nothing Then
                textLeft = TextLeftWithPoster
            End If

            Dim tile As New Panel
            tile.Name = "pnlCardFilm" & filmID
            tile.Size = New Size(TileWidth, TileHeight)
            tile.BackColor = CardBack
            tile.Cursor = Cursors.Hand
            tile.Tag = filmID

            Dim strip As New Panel
            strip.Name = "pnlAccentFilm" & filmID
            strip.Location = New Point(0, 0)
            strip.Size = New Size(8, TileHeight)
            strip.BackColor = HighlightBack
            tile.Controls.Add(strip)

            If poster IsNot Nothing Then
                Dim picFilm As New PictureBox
                picFilm.Name = "picFilm" & filmID
                picFilm.Location = New Point(20, (TileHeight - PosterHeight) \ 2)
                picFilm.Size = New Size(PosterWidth, PosterHeight)
                picFilm.SizeMode = PictureBoxSizeMode.Zoom
                picFilm.Image = poster
                picFilm.Cursor = Cursors.Hand
                picFilm.Tag = filmID
                AddHandler picFilm.Click, AddressOf FilmTile_Click
                tile.Controls.Add(picFilm)
            End If

            Dim lblTitle As New Label
            lblTitle.AutoSize = False
            lblTitle.Location = New Point(textLeft, 20)
            lblTitle.Size = New Size(TileWidth - textLeft - 24, 84)
            lblTitle.Font = New Font("Segoe UI", 15, FontStyle.Bold)
            lblTitle.ForeColor = TextFore
            lblTitle.Text = title
            lblTitle.Tag = filmID
            tile.Controls.Add(lblTitle)

            Dim lblMeta As New Label
            lblMeta.AutoSize = True
            lblMeta.Location = New Point(textLeft, TileHeight - 44)
            lblMeta.Font = New Font("Segoe UI", 11)
            lblMeta.ForeColor = SubtleFore
            lblMeta.Text = rating & "   -   " & RunningTime(duration)
            lblMeta.Tag = filmID
            tile.Controls.Add(lblMeta)

            AddHandler tile.Click, AddressOf FilmTile_Click
            AddHandler lblTitle.Click, AddressOf FilmTile_Click
            AddHandler lblMeta.Click, AddressOf FilmTile_Click

            pnlFilmList.Controls.Add(tile)
        Next

        ArrangeFilmTiles()
    End Sub

    Private Sub ClearFoodTiles()
        Dim tile As Control

        For Each tile In pnlFoodList.Controls
            Dim inner As Control

            For Each inner In tile.Controls
                If TypeOf inner Is PictureBox Then
                    Dim picFood As PictureBox = CType(inner, PictureBox)

                    If picFood.Image IsNot Nothing Then
                        picFood.Image.Dispose()
                        picFood.Image = Nothing
                    End If
                End If
            Next
        Next

        ClearPanel(pnlFoodList)
    End Sub

    Private Sub ClearFilmTiles()
        Dim tile As Control

        For Each tile In pnlFilmList.Controls
            Dim inner As Control

            For Each inner In tile.Controls
                If TypeOf inner Is PictureBox Then
                    Dim picFilm As PictureBox = CType(inner, PictureBox)

                    If picFilm.Image IsNot Nothing Then
                        picFilm.Image.Dispose()
                        picFilm.Image = Nothing
                    End If
                End If
            Next
        Next

        ClearPanel(pnlFilmList)
    End Sub

    Private Sub ArrangeTiles(pnl As Panel, tileWidth As Integer, tileHeight As Integer)
        Dim perRow As Integer = (pnl.Width - TileGap) \ (tileWidth + TileGap)

        If perRow < 1 Then
            perRow = 1
        End If

        Dim i As Integer
        For i = 0 To pnl.Controls.Count - 1
            Dim column As Integer = i Mod perRow
            Dim row As Integer = i \ perRow

            pnl.Controls(i).Left = column * (tileWidth + TileGap)
            pnl.Controls(i).Top = row * (tileHeight + TileGap)
        Next
    End Sub

    Private Sub ArrangeFilmTiles()
        ArrangeTiles(pnlFilmList, TileWidth, TileHeight)
    End Sub

    Private Sub ArrangeTimeTiles()
        ArrangeTiles(pnlTimeList, TimeTileWidth, TimeTileHeight)
    End Sub

    Private Function RunningTime(minutes As Integer) As String
        Return (minutes \ 60) & "h " & Format(minutes Mod 60, "00") & "m"
    End Function

    Private Sub FilmTile_Click(sender As Object, e As EventArgs)
        Touched()

        Dim ctrl As Control = CType(sender, Control)
        currentFilmID = CLng(ctrl.Tag)

        LoadFilmDetails()

        LoadShowingsForFilm()
        ShowStep(StepTimes)
    End Sub

    Private Sub LoadFilmDetails()
        currentFilmTitle = ""
        currentFilmRating = ""
        currentFilmDuration = 0
        currentFilmYear = ""
        currentFilmGenres = ""
        currentFilmSynopsis = ""
        currentFilmPoster = ""

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT FilmTitle, FilmAgeRating, FilmDuration, FilmYear, FilmGenres, " &
                                 "FilmDescription, FilmPoster FROM tblFilm WHERE FilmID = @FilmID"
            SQLCmd.Parameters.AddWithValue("@FilmID", CInt(currentFilmID))
            Dim rs As OleDbDataReader = SQLCmd.ExecuteReader()
            If rs.Read() Then
                currentFilmTitle = rs("FilmTitle").ToString()
                currentFilmRating = rs("FilmAgeRating").ToString()
                currentFilmGenres = rs("FilmGenres").ToString()
                currentFilmSynopsis = rs("FilmDescription").ToString()
                currentFilmPoster = rs("FilmPoster").ToString()
                currentFilmYear = rs("FilmYear").ToString()

                If Not IsDBNull(rs("FilmDuration")) Then
                    currentFilmDuration = CInt(rs("FilmDuration"))
                End If
            End If
            rs.Close()
            cn.Close()
        End If
    End Sub

    Private Sub LoadShowingsForFilm()
        Dim dt As New DataTable

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT tblScreening.ScreeningID, ScreeningTime, TicketPrice, " &
                                 "tblScreening.ScreenID, ScreenName, ScreenCapacity " &
                                 "FROM tblScreening INNER JOIN tblScreen ON tblScreening.ScreenID = tblScreen.ScreenID " &
                                 "WHERE FilmID = @FilmID AND ScreeningDate = @Day AND ScreeningTime >= @EarliestTime " &
                                 "AND (ScreeningStatus IS NULL OR ScreeningStatus <> 'Cancelled') " &
                                 "AND (ScreenStatus IS NULL OR ScreenStatus <> @OutOfService) " &
                                 "ORDER BY ScreeningTime"
            SQLCmd.Parameters.AddWithValue("@FilmID", CInt(currentFilmID))
            SQLCmd.Parameters.AddWithValue("@Day", currentDay)
            SQLCmd.Parameters.AddWithValue("@EarliestTime", EarliestTimeForDay())
            SQLCmd.Parameters.AddWithValue("@OutOfService", ScreenOutOfService)
            Dim da As New OleDbDataAdapter(SQLCmd)
            da.Fill(dt)
            cn.Close()
        End If

        lblTimesFilm.Text = currentFilmTitle
        BuildTimeTiles(dt)
    End Sub

    Private Function SeatsSold(screeningID As Long) As Integer
        Dim sold As Integer = 0

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT COUNT(*) FROM tblBookingSeat WHERE ScreeningID = @ScreeningID"
            SQLCmd.Parameters.AddWithValue("@ScreeningID", CInt(screeningID))
            sold = CInt(SQLCmd.ExecuteScalar())
            cn.Close()
        End If

        Return sold
    End Function

    Private Sub BuildTimeTiles(dtShowings As DataTable)
        ClearPanel(pnlTimeList)

        Dim i As Integer
        For i = 0 To dtShowings.Rows.Count - 1
            Dim screeningID As Long = CLng(dtShowings.Rows(i)("ScreeningID"))
            Dim showTime As String = dtShowings.Rows(i)("ScreeningTime").ToString()
            Dim screenName As String = dtShowings.Rows(i)("ScreenName").ToString()
            Dim capacity As Integer = CInt(dtShowings.Rows(i)("ScreenCapacity"))
            Dim price As Double = CDbl(dtShowings.Rows(i)("TicketPrice"))
            Dim seatsLeft As Integer = capacity - SeatsSold(screeningID)

            Dim tile As New Panel
            tile.Name = "pnlCardTime" & screeningID
            tile.Size = New Size(TimeTileWidth, TimeTileHeight)
            tile.BackColor = CardBack
            tile.Tag = screeningID

            Dim strip As New Panel
            strip.Name = "pnlAccentTime" & screeningID
            strip.Location = New Point(0, 0)
            strip.Size = New Size(8, TimeTileHeight)
            tile.Controls.Add(strip)

            Dim lblTime As New Label
            lblTime.AutoSize = True
            lblTime.Location = New Point(26, 18)
            lblTime.Font = New Font("Segoe UI", 26, FontStyle.Bold)
            lblTime.Text = showTime
            lblTime.Tag = screeningID
            tile.Controls.Add(lblTime)

            Dim lblMeta As New Label
            lblMeta.AutoSize = True
            lblMeta.Location = New Point(28, 78)
            lblMeta.Font = New Font("Segoe UI", 11)
            lblMeta.ForeColor = SubtleFore
            lblMeta.Tag = screeningID
            tile.Controls.Add(lblMeta)

            If seatsLeft > 0 Then
                strip.BackColor = HighlightBack
                lblTime.ForeColor = TextFore
                lblMeta.Text = screenName & vbNewLine & seatsLeft & " seats left  -  from " & FormatCurrency(price)
                tile.Cursor = Cursors.Hand

                AddHandler tile.Click, AddressOf TimeTile_Click
                AddHandler lblTime.Click, AddressOf TimeTile_Click
                AddHandler lblMeta.Click, AddressOf TimeTile_Click
            Else
                strip.BackColor = SubtleFore
                lblTime.ForeColor = SubtleFore
                lblMeta.Text = screenName & vbNewLine & "Sold out"
            End If

            pnlTimeList.Controls.Add(tile)
        Next

        ArrangeTimeTiles()
    End Sub

    Private Sub TimeTile_Click(sender As Object, e As EventArgs)
        Touched()

        Dim ctrl As Control = CType(sender, Control)
        currentScreeningID = CLng(ctrl.Tag)

        LoadShowingDetails()
        WriteLog("KIOSK", "Customer picked screening " & currentScreeningID & " (" & currentShowingText & ")")

        BuildSeatMap()
        ShowStep(StepSeats)
    End Sub

    Private Sub SetUpPickedSeats()
        pickedSeats = New DataTable
        pickedSeats.Columns.Add("SeatID", GetType(Integer))
        pickedSeats.Columns.Add("SeatName", GetType(String))
        pickedSeats.Columns.Add("SeatType", GetType(String))
        pickedSeats.Columns.Add("Multiplier", GetType(Double))
    End Sub

    Private Function IsSeatPicked(seatID As Long) As Boolean
        Return pickedSeats.Select("SeatID = " & seatID).Length > 0
    End Function

    Private Sub ApplySeatColours()
        lblSwatchAvailable.BackColor = SeatAvailable
        lblSwatchSelected.BackColor = SeatSelected
        lblSwatchTaken.BackColor = SeatTaken

        lblSwatchPremium.BackColor = SeatAvailable
        lblSwatchAccessible.BackColor = SeatAvailable
        lblSwatchSaver.BackColor = SeatAvailable
        lblSwatchPremium.Invalidate()
        lblSwatchAccessible.Invalidate()
        lblSwatchSaver.Invalidate()
    End Sub

    Private Sub SeatTypeSwatch_Paint(sender As Object, e As PaintEventArgs) Handles lblSwatchPremium.Paint,
        lblSwatchAccessible.Paint, lblSwatchSaver.Paint
        Dim swatch As Label = CType(sender, Label)
        Dim edge As Color = SeatPremiumEdge

        If swatch Is lblSwatchAccessible Then
            edge = SeatAccessibleEdge
        End If

        If swatch Is lblSwatchSaver Then
            edge = SeatSaverEdge
        End If

        Dim edgePen As New Pen(edge, 3)
        e.Graphics.DrawRectangle(edgePen, 1, 1, swatch.Width - 3, swatch.Height - 3)
    End Sub

    Private Sub BuildSeatMap()
        ApplySeatColours()
        ClearPanel(pnlSeatMap)
        lblSeatsShowing.Text = currentShowingText

        SetUpPickedSeats()

        currentSeats = New DataTable
        Dim dtTaken As New DataTable

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn

            SQLCmd.CommandText = "SELECT tblSeat.SeatID, tblSeat.SeatRow, tblSeat.SeatNumber, " &
                                 "tblSeatType.SeatTypeName, tblSeatType.PriceMultiplier " &
                                 "FROM tblSeat INNER JOIN tblSeatType ON tblSeat.SeatTypeID = tblSeatType.SeatTypeID " &
                                 "WHERE tblSeat.ScreenID = @ScreenID " &
                                 "ORDER BY tblSeat.SeatRow, tblSeat.SeatNumber"
            SQLCmd.Parameters.AddWithValue("@ScreenID", CInt(currentScreenID))
            Dim da As New OleDbDataAdapter(SQLCmd)
            da.Fill(currentSeats)

            SQLCmd.CommandText = "SELECT SeatID FROM tblBookingSeat WHERE ScreeningID = @ScreeningID"
            SQLCmd.Parameters.Clear()
            SQLCmd.Parameters.AddWithValue("@ScreeningID", CInt(currentScreeningID))
            Dim da2 As New OleDbDataAdapter(SQLCmd)
            da2.Fill(dtTaken)

            cn.Close()
        End If

        Dim i As Integer
        For i = 0 To currentSeats.Rows.Count - 1
            Dim seatID As Long = CLng(currentSeats.Rows(i)("SeatID"))
            Dim seatRow As String = currentSeats.Rows(i)("SeatRow").ToString()
            Dim seatNumber As Integer = CInt(currentSeats.Rows(i)("SeatNumber"))
            Dim multiplier As Double = CDbl(currentSeats.Rows(i)("PriceMultiplier"))
            Dim seatType As String = currentSeats.Rows(i)("SeatTypeName").ToString()

            Dim b As New Button
            b.Tag = seatID
            b.Text = seatRow & seatNumber
            b.FlatStyle = FlatStyle.Flat
            b.FlatAppearance.BorderSize = 0

            If seatType = SeatPremium Then
                b.FlatAppearance.BorderSize = 3
                b.FlatAppearance.BorderColor = SeatPremiumEdge
            ElseIf seatType = SeatAccessible Then
                b.FlatAppearance.BorderSize = 3
                b.FlatAppearance.BorderColor = SeatAccessibleEdge
            ElseIf seatType = SeatSaver Then
                b.FlatAppearance.BorderSize = 3
                b.FlatAppearance.BorderColor = SeatSaverEdge
            End If

            If dtTaken.Select("SeatID = " & seatID).Length > 0 Then
                b.BackColor = SeatTaken
                b.ForeColor = SeatTakenFore
                b.Enabled = False
            Else
                b.BackColor = SeatAvailable
                b.ForeColor = SeatFore
                AddHandler b.Click, AddressOf Seat_Click
            End If

            pnlSeatMap.Controls.Add(b)
        Next

        CentreSeatMap()
        UpdateSeatSummary()
    End Sub

    Private Sub CentreSeatMap()
        Dim seatsAcross As Integer = WidestRow()
        Dim rowsDown As Integer = DeepestRow()

        Dim spaceAcross As Integer = pnlSeats.Width - LeftColumnWidth - 30

        lblScreen.Top = lblSeatsShowing.Bottom + 24
        Dim mapTop As Integer = lblScreen.Bottom + 16
        Dim spaceDown As Integer = pnlSeats.Height - mapTop - 155

        seatDrawWidth = LargestSeatThatFits(seatsAcross, rowsDown, spaceAcross, spaceDown)

        seatDrawHeight = (seatDrawWidth * SeatHeight) \ SeatWidth

        Dim mapWidth As Integer = (seatsAcross * (seatDrawWidth + SeatGap)) - SeatGap + SeatMapMargin

        If mapWidth > spaceAcross Then
            mapWidth = spaceAcross
        End If

        Dim mapLeft As Integer = LeftColumnWidth + ((spaceAcross - mapWidth) \ 2)

        lblScreen.Left = mapLeft
        lblScreen.Width = mapWidth

        pnlSeatMap.Left = mapLeft
        pnlSeatMap.Width = mapWidth
        pnlSeatMap.Top = mapTop

        Dim mapHeight As Integer = (rowsDown * (seatDrawHeight + SeatGap)) - SeatGap

        If mapHeight > spaceDown Then
            mapHeight = spaceDown
        End If

        pnlSeatMap.Height = mapHeight

        ArrangeSeatMap()
    End Sub

    Private Function LargestSeatThatFits(seatsAcross As Integer, rowsDown As Integer,
                                         spaceAcross As Integer, spaceDown As Integer) As Integer
        Dim biggest As Integer = SeatWidth

        If seatsAcross > 0 Then
            Dim fitsAcross As Integer = ((spaceAcross - SeatMapMargin) \ seatsAcross) - SeatGap
            If fitsAcross < biggest Then
                biggest = fitsAcross
            End If
        End If

        If rowsDown > 0 Then
            Dim heightThatFits As Integer = ((spaceDown - SeatGap) \ rowsDown) - SeatGap
            Dim fitsDown As Integer = (heightThatFits * SeatWidth) \ SeatHeight

            If fitsDown < biggest Then
                biggest = fitsDown
            End If
        End If

        If biggest < SmallestSeatWidth Then
            biggest = SmallestSeatWidth
        End If

        Return biggest
    End Function

    Private Function DeepestRow() As Integer
        Dim deepest As Integer = 0
        Dim i As Integer

        For i = 0 To currentSeats.Rows.Count - 1
            Dim rowIndex As Integer = Asc(currentSeats.Rows(i)("SeatRow").ToString()) - 65
            If rowIndex + 1 > deepest Then
                deepest = rowIndex + 1
            End If
        Next

        Return deepest
    End Function

    Private Sub ArrangeSeatMap()
        If currentSeats Is Nothing Then
            Exit Sub
        End If

        Dim i As Integer
        For i = 0 To pnlSeatMap.Controls.Count - 1
            Dim seatRow As String = currentSeats.Rows(i)("SeatRow").ToString()
            Dim seatNumber As Integer = CInt(currentSeats.Rows(i)("SeatNumber"))

            Dim rowIndex As Integer = Asc(seatRow) - 65

            pnlSeatMap.Controls(i).Size = New Size(seatDrawWidth, seatDrawHeight)
            pnlSeatMap.Controls(i).Left = (seatNumber - 1) * (seatDrawWidth + SeatGap)
            pnlSeatMap.Controls(i).Top = rowIndex * (seatDrawHeight + SeatGap)

            pnlSeatMap.Controls(i).Font = New Font("Segoe UI", SeatFontSize())
        Next
    End Sub

    Private Function SeatFontSize() As Single
        If seatDrawWidth >= 50 Then
            Return 10
        ElseIf seatDrawWidth >= 42 Then
            Return 9
        End If

        Return 7.5
    End Function

    Private Function WidestRow() As Integer
        Dim widest As Integer = 0
        Dim i As Integer

        For i = 0 To currentSeats.Rows.Count - 1
            Dim seatNumber As Integer = CInt(currentSeats.Rows(i)("SeatNumber"))
            If seatNumber > widest Then
                widest = seatNumber
            End If
        Next

        Return widest
    End Function

    Private Sub Seat_Click(sender As Object, e As EventArgs)
        Touched()

        Dim b As Button = CType(sender, Button)
        Dim seatID As Long = CLng(b.Tag)

        If IsSeatPicked(seatID) Then
            Dim rows() As DataRow = pickedSeats.Select("SeatID = " & seatID)
            pickedSeats.Rows.Remove(rows(0))
            b.BackColor = SeatAvailable
        Else
            If pickedSeats.Rows.Count >= MaxSeatsPerSale Then
                MessageBox.Show("You can buy up to " & MaxSeatsPerSale & " tickets at the machine." & vbNewLine &
                                "For a bigger group please ask at the desk.",
                                "Too many seats", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If

            pickedSeats.Rows.Add(CInt(seatID), b.Text, TypeOfSeat(seatID), MultiplierForSeat(seatID))
            b.BackColor = SeatSelected
        End If

        UpdateSeatSummary()
    End Sub

    Private Function TypeOfSeat(seatID As Long) As String
        Dim rows() As DataRow = currentSeats.Select("SeatID = " & seatID)

        If rows.Length > 0 Then
            Return rows(0)("SeatTypeName").ToString()
        End If

        Return SeatStandard
    End Function

    Private Function MultiplierForSeat(seatID As Long) As Double
        Dim rows() As DataRow = currentSeats.Select("SeatID = " & seatID)

        If rows.Length > 0 Then
            Return CDbl(rows(0)("PriceMultiplier"))
        End If

        Return 1
    End Function

    Private Function TicketsTotal() As Double
        Dim total As Double = 0
        Dim i As Integer

        For i = 0 To pickedSeats.Rows.Count - 1
            total = total + SeatPrice(currentTicketPrice, CDbl(pickedSeats.Rows(i)("Multiplier")))
        Next

        Return total
    End Function

    Private Sub UpdateSeatSummary()
        Dim listing As String = ""
        Dim i As Integer

        For i = 0 To pickedSeats.Rows.Count - 1
            Dim seatName As String = pickedSeats.Rows(i)("SeatName").ToString()
            Dim price As Double = SeatPrice(currentTicketPrice, CDbl(pickedSeats.Rows(i)("Multiplier")))
            listing = listing & "Seat " & seatName & "   " & FormatCurrency(price) & vbNewLine
        Next

        If pickedSeats.Rows.Count = 0 Then
            lblSeatsPicked.Text = "Touch the seats you want" & vbNewLine & vbNewLine &
                                  "Up to " & MaxSeatsPerSale & " at a time"
        Else
            lblSeatsPicked.Text = pickedSeats.Rows.Count & " seat(s)" & vbNewLine & vbNewLine & listing
        End If

        lblRunningTotal.Text = "Total  " & FormatCurrency(OrderTotal())
        btnNext.Enabled = (pickedSeats.Rows.Count > 0)
        PaintKioskButton(btnNext, True)
    End Sub

    Private Sub LoadShowingDetails()
        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT ScreenID, TicketPrice, ScreeningTime " &
                                 "FROM tblScreening WHERE ScreeningID = @ScreeningID"
            SQLCmd.Parameters.AddWithValue("@ScreeningID", CInt(currentScreeningID))
            Dim rs As OleDbDataReader = SQLCmd.ExecuteReader()
            If rs.Read() Then
                currentScreenID = CLng(rs("ScreenID"))
                currentTicketPrice = CDbl(rs("TicketPrice"))
                currentShowingText = currentFilmTitle & " at " & rs("ScreeningTime").ToString()
            End If
            rs.Close()
            cn.Close()
        End If
    End Sub

    Private Sub Welcome_Click(sender As Object, e As EventArgs) Handles btnStart.Click, pnlWelcome.Click,
        lblWelcomeTitle.Click, lblWelcomeSub.Click

        BuildDayPicker()
        LoadFilmsForDay()
        ShowStep(StepFilms)
    End Sub

    Private Sub SetUpPendingFood()
        pendingFood = New DataTable
        pendingFood.Columns.Add("FoodItemID", GetType(Integer))
        pendingFood.Columns.Add("Item", GetType(String))
        pendingFood.Columns.Add("Price", GetType(Double))
        pendingFood.Columns.Add("Quantity", GetType(Integer))
    End Sub

    Private Sub LoadFoodItems()
        foodOnSale = New DataTable
        Dim dt As DataTable = foodOnSale

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT FoodItemID, FoodItemName, FoodItemPrice, FoodItemImage " &
                                 "FROM tblFoodItem " &
                                 "WHERE (FoodItemStatus IS NULL OR FoodItemStatus <> @Withdrawn) " &
                                 "ORDER BY FoodItemCategory, FoodItemName"
            SQLCmd.Parameters.AddWithValue("@Withdrawn", FoodWithdrawn)
            Dim da As New OleDbDataAdapter(SQLCmd)
            da.Fill(dt)
            cn.Close()
        End If

        BuildFoodTiles(dt)
        UpdateFoodOrder()
    End Sub

    Private Sub BuildFoodTiles(dtFood As DataTable)
        ClearFoodTiles()

        Dim i As Integer
        For i = 0 To dtFood.Rows.Count - 1
            Dim foodID As Long = CLng(dtFood.Rows(i)("FoodItemID"))
            Dim itemName As String = dtFood.Rows(i)("FoodItemName").ToString()
            Dim price As Double = CDbl(dtFood.Rows(i)("FoodItemPrice"))

            Dim tile As New Panel
            tile.Name = "pnlCardFood" & foodID
            tile.Size = New Size(FoodTileWidth, FoodTileHeight)
            tile.BackColor = CardBack
            tile.Cursor = Cursors.Hand
            tile.Tag = foodID

            Dim strip As New Panel
            strip.Name = "pnlAccentFood" & foodID
            strip.Location = New Point(0, 0)
            strip.Size = New Size(8, FoodTileHeight)
            strip.BackColor = HighlightBack
            tile.Controls.Add(strip)

            Dim picture As Image = FoodImage(dtFood.Rows(i)("FoodItemImage").ToString())
            Dim textLeft As Integer = TextLeftNoPicture

            If picture IsNot Nothing Then
                textLeft = TextLeftWithPicture

                Dim picFood As New PictureBox
                picFood.Name = "picFood" & foodID
                picFood.Location = New Point(16, (FoodTileHeight - FoodPictureSize) \ 2)
                picFood.Size = New Size(FoodPictureSize, FoodPictureSize)
                picFood.SizeMode = PictureBoxSizeMode.Zoom
                picFood.Image = picture
                picFood.Cursor = Cursors.Hand
                picFood.Tag = foodID

                AddHandler picFood.Click, AddressOf FoodTile_Click
                tile.Controls.Add(picFood)
            End If

            Dim lblName As New Label
            lblName.AutoSize = False
            lblName.Location = New Point(textLeft, 14)
            lblName.Size = New Size(FoodTileWidth - textLeft - 20, 46)
            lblName.Font = New Font("Segoe UI", 12, FontStyle.Bold)
            lblName.ForeColor = TextFore
            lblName.Text = itemName
            lblName.Tag = foodID
            tile.Controls.Add(lblName)

            Dim lblPrice As New Label
            lblPrice.AutoSize = True
            lblPrice.Location = New Point(textLeft, FoodTileHeight - 40)
            lblPrice.Font = New Font("Segoe UI", 12)
            lblPrice.ForeColor = SubtleFore
            lblPrice.Text = FormatCurrency(price)
            lblPrice.Tag = foodID
            tile.Controls.Add(lblPrice)

            Dim lblCount As New Label
            lblCount.Name = "lblFoodCount" & foodID
            lblCount.AutoSize = False
            lblCount.TextAlign = ContentAlignment.MiddleRight
            lblCount.Location = New Point(FoodTileWidth - 130, FoodTileHeight - 42)
            lblCount.Size = New Size(56, 30)
            lblCount.Font = New Font("Segoe UI", 14, FontStyle.Bold)
            lblCount.ForeColor = HighlightBack
            lblCount.Text = ""
            lblCount.Tag = foodID
            tile.Controls.Add(lblCount)

            Dim btnLess As New Button
            btnLess.Name = "btnFoodLess" & foodID
            btnLess.Text = "-"
            btnLess.Font = New Font("Segoe UI", 15, FontStyle.Bold)
            btnLess.Size = New Size(54, 40)
            btnLess.Location = New Point(FoodTileWidth - 66, FoodTileHeight - 50)
            btnLess.FlatStyle = FlatStyle.Flat
            btnLess.FlatAppearance.BorderSize = 1
            btnLess.FlatAppearance.BorderColor = BorderCol
            btnLess.BackColor = FormBack
            btnLess.ForeColor = TextFore
            btnLess.Visible = False
            btnLess.Tag = foodID
            AddHandler btnLess.Click, AddressOf FoodLess_Click
            tile.Controls.Add(btnLess)
            btnLess.BringToFront()

            AddHandler tile.Click, AddressOf FoodTile_Click
            AddHandler lblName.Click, AddressOf FoodTile_Click
            AddHandler lblPrice.Click, AddressOf FoodTile_Click
            AddHandler lblCount.Click, AddressOf FoodTile_Click

            pnlFoodList.Controls.Add(tile)
        Next

        ArrangeTiles(pnlFoodList, FoodTileWidth, FoodTileHeight)
    End Sub

    Private Sub FoodTile_Click(sender As Object, e As EventArgs)
        Touched()

        Dim ctrl As Control = CType(sender, Control)
        Dim foodID As Long = CLng(ctrl.Tag)

        Dim rows() As DataRow = pendingFood.Select("FoodItemID = " & foodID)

        If rows.Length > 0 Then
            rows(0)("Quantity") = CInt(rows(0)("Quantity")) + 1
        Else
            AddFoodLine(foodID)
        End If

        UpdateFoodOrder()
    End Sub

    Private Sub FoodLess_Click(sender As Object, e As EventArgs)
        Touched()

        Dim btn As Button = CType(sender, Button)
        Dim foodID As Long = CLng(btn.Tag)

        Dim rows() As DataRow = pendingFood.Select("FoodItemID = " & foodID)

        If rows.Length > 0 Then
            Dim quantity As Integer = CInt(rows(0)("Quantity")) - 1

            If quantity > 0 Then
                rows(0)("Quantity") = quantity
            Else
                pendingFood.Rows.Remove(rows(0))
            End If
        End If

        UpdateFoodOrder()
    End Sub

    Private Sub AddFoodLine(foodID As Long)
        Dim rows() As DataRow = foodOnSale.Select("FoodItemID = " & foodID)

        If rows.Length = 0 Then
            Exit Sub
        End If

        pendingFood.Rows.Add(CInt(foodID), rows(0)("FoodItemName").ToString(),
                             CDbl(rows(0)("FoodItemPrice")), 1)
    End Sub

    Private Sub UpdateFoodOrder()
        Dim listing As String = ""
        Dim i As Integer

        For i = 0 To pendingFood.Rows.Count - 1
            Dim quantity As Integer = CInt(pendingFood.Rows(i)("Quantity"))
            Dim itemName As String = pendingFood.Rows(i)("Item").ToString()
            Dim lineCost As Double = CDbl(pendingFood.Rows(i)("Price")) * quantity

            listing = listing & quantity & " x " & itemName & "   " & FormatCurrency(lineCost) & vbNewLine
        Next

        If pendingFood.Rows.Count = 0 Then
            lblFoodOrder.Text = "Nothing added yet"
        Else
            lblFoodOrder.Text = "Your order" & vbNewLine & vbNewLine & listing
        End If

        ShowFoodCounts()

        lblRunningTotal.Text = "Total  " & FormatCurrency(OrderTotal())
        btnNext.Text = NextTextForFood()
        PaintKioskButton(btnNext, True)
    End Sub

    Private Sub ShowFoodCounts()
        Dim i As Integer

        For i = 0 To pnlFoodList.Controls.Count - 1
            Dim foodID As Long = CLng(pnlFoodList.Controls(i).Tag)
            Dim lblCount As Control = pnlFoodList.Controls(i).Controls("lblFoodCount" & foodID)

            If lblCount IsNot Nothing Then
                Dim rows() As DataRow = pendingFood.Select("FoodItemID = " & foodID)

                If rows.Length > 0 Then
                    lblCount.Text = "x" & rows(0)("Quantity").ToString()
                Else
                    lblCount.Text = ""
                End If
            End If

            Dim btnLess As Control = pnlFoodList.Controls(i).Controls("btnFoodLess" & foodID)

            If btnLess IsNot Nothing Then
                btnLess.Visible = (pendingFood.Select("FoodItemID = " & foodID).Length > 0)
            End If
        Next
    End Sub

    Private Function NextTextForFood() As String
        If pendingFood Is Nothing OrElse pendingFood.Rows.Count = 0 Then
            Return "No thanks"
        End If

        Return "Continue"
    End Function

    Private Function FoodTotal() As Double
        Dim total As Double = 0
        Dim i As Integer

        For i = 0 To pendingFood.Rows.Count - 1
            total = total + (CDbl(pendingFood.Rows(i)("Price")) * CInt(pendingFood.Rows(i)("Quantity")))
        Next

        Return total
    End Function

    Private Function OrderTotal() As Double
        Return TicketsTotal() + FoodTotal()
    End Function

    Private Sub BuildConfirmation()
        Dim detail As String = currentShowingText & vbNewLine & vbNewLine

        Dim i As Integer
        For i = 0 To pickedSeats.Rows.Count - 1
            Dim seatName As String = pickedSeats.Rows(i)("SeatName").ToString()
            Dim seatType As String = pickedSeats.Rows(i)("SeatType").ToString()
            Dim price As Double = SeatPrice(currentTicketPrice, CDbl(pickedSeats.Rows(i)("Multiplier")))

            detail = detail & "Seat " & seatName

            If seatType <> SeatStandard Then
                detail = detail & "  (" & seatType.ToLower() & ")"
            End If

            detail = detail & "   " & FormatCurrency(price) & vbNewLine
        Next

        Dim f As Integer
        For f = 0 To pendingFood.Rows.Count - 1
            Dim quantity As Integer = CInt(pendingFood.Rows(f)("Quantity"))
            Dim itemName As String = pendingFood.Rows(f)("Item").ToString()
            Dim lineCost As Double = CDbl(pendingFood.Rows(f)("Price")) * quantity

            If f = 0 Then
                detail = detail & vbNewLine
            End If

            detail = detail & quantity & " x " & itemName & "   " & FormatCurrency(lineCost) & vbNewLine
        Next

        lblConfirmDetail.Text = detail
        lblConfirmTotal.Text = "To pay  " & FormatCurrency(OrderTotal())
    End Sub

    Private Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        If currentStep = StepFilms Then
            ShowStep(StepWelcome)
        ElseIf currentStep = StepTimes Then
            LoadFilmsForDay()
            ShowStep(StepFilms)
        ElseIf currentStep = StepSeats Then
            LoadShowingsForFilm()
            ShowStep(StepTimes)
        ElseIf currentStep = StepFood Then
            BuildSeatMap()
            ShowStep(StepSeats)
        ElseIf currentStep = StepConfirm Then
            ShowStep(StepFood)
        End If
    End Sub

    Private Sub btnNext_Click(sender As Object, e As EventArgs) Handles btnNext.Click
        If currentStep = StepSeats Then
            LoadFoodItems()
            ShowStep(StepFood)
        ElseIf currentStep = StepFood Then
            BuildConfirmation()
            ShowStep(StepConfirm)
        ElseIf currentStep = StepConfirm Then
            TakePayment()
        ElseIf currentStep = StepDone Then
            StartAgain()
        End If
    End Sub

    Private Sub TakePayment()
        Dim seatCount As Integer = pickedSeats.Rows.Count

        If seatCount = 0 Then
            Exit Sub
        End If

        Dim total As Double = OrderTotal()
        Dim newBookingID As Long = CompleteSale(0, True, currentScreeningID, PickedSeatIDs(), pendingFood, total, 0)

        If newBookingID = 0 Then
            BuildSeatMap()
            ShowStep(StepSeats)
            Exit Sub
        End If

        WriteLog("KIOSK", "Kiosk sale " & newBookingID & ", " & seatCount & " seat(s) and " &
                          pendingFood.Rows.Count & " food line(s), " & FormatCurrency(total))

        BuildReceipt(newBookingID, seatCount, total)
        ShowStep(StepDone)
    End Sub

    Private Function PickedSeatIDs() As Long()
        Dim seatIDs(pickedSeats.Rows.Count - 1) As Long
        Dim i As Integer

        For i = 0 To pickedSeats.Rows.Count - 1
            seatIDs(i) = CLng(pickedSeats.Rows(i)("SeatID"))
        Next

        Return seatIDs
    End Function

    Private Sub BuildReceipt(bookingID As Long, seatCount As Integer, total As Double)
        Dim seatList As String = ""
        Dim i As Integer

        For i = 0 To pickedSeats.Rows.Count - 1
            If seatList <> "" Then
                seatList = seatList & ", "
            End If
            seatList = seatList & pickedSeats.Rows(i)("SeatName").ToString()
        Next

        lblDoneRef.Text = "Booking " & bookingID
        lblDoneDetail.Text = currentShowingText & vbNewLine &
                             seatCount & " ticket(s), seat " & seatList & vbNewLine &
                             "Paid " & FormatCurrency(total)
    End Sub

    Private Sub StartAgain()
        currentDay = Date.Today
        currentFilmID = 0
        currentFilmTitle = ""
        currentFilmRating = ""
        currentFilmDuration = 0
        currentFilmYear = ""
        currentFilmGenres = ""
        currentFilmSynopsis = ""
        currentFilmPoster = ""
        currentScreeningID = 0
        currentScreenID = 0
        currentTicketPrice = 0
        currentShowingText = ""

        SetUpPickedSeats()
        SetUpPendingFood()
        ClearPanel(pnlSeatMap)
        ClearFoodTiles()
        currentSeats = Nothing

        ShowStep(StepWelcome)
    End Sub

    Private Sub StyleKioskButtons()
        PaintKioskButton(btnStart, True)
        PaintKioskButton(btnNext, True)
        PaintKioskButton(btnBack, False)
    End Sub

    Private Sub PaintKioskButton(btn As Button, isMainAction As Boolean)
        btn.FlatStyle = FlatStyle.Flat
        btn.UseVisualStyleBackColor = False

        If isMainAction Then
            btn.BackColor = HighlightBack
            btn.ForeColor = HighlightFore
            btn.FlatAppearance.BorderSize = 0
        Else
            btn.BackColor = CardBack
            btn.ForeColor = TextFore
            btn.FlatAppearance.BorderSize = 1
            btn.FlatAppearance.BorderColor = BorderCol
        End If

        If Not btn.Enabled Then
            btn.BackColor = ReadOnlyBack
            btn.ForeColor = SubtleFore
        End If
    End Sub

    Private Sub Touched()
        secondsIdle = 0
    End Sub

    Private Sub timerIdle_Tick(sender As Object, e As EventArgs) Handles timerIdle.Tick
        If currentStep = StepWelcome Then
            Exit Sub
        End If

        secondsIdle = secondsIdle + 1

        Dim allowed As Integer = IdleSecondsAllowed
        If currentStep = StepDone Then
            allowed = IdleSecondsOnThankYou
        End If

        If secondsIdle >= allowed Then
            If currentStep <> StepDone Then
                WriteLog("KIOSK", "Order left unfinished on the " & currentStep & " step, kiosk reset itself")
            End If

            StartAgain()
        End If
    End Sub

    Private Sub btnExitKiosk_Click(sender As Object, e As EventArgs) Handles btnExitKiosk.Click
        Dim answer As DialogResult = MessageBox.Show("Close the kiosk and go back to the main menu?",
                                                     "Staff Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        If answer = DialogResult.Yes Then
            WriteLog("KIOSK", "Kiosk closed by staff")
            Me.Close()
        End If
    End Sub

End Class
