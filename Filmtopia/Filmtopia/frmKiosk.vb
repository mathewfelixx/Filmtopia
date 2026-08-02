'the self service screen customers use themselves, so nothing on here is meant for a member of
'staff. it is one window with no border that fills the whole screen, and everything on it is made
'big enough to be pressed with a finger rather than clicked with a mouse
Imports System.Data.OleDb

Public Class frmKiosk

    'how big one film tile is drawn. a finger is a lot less accurate than a mouse pointer, so these
    'are deliberately much bigger than anything on the staff screens
    Private Const TileWidth As Integer = 300
    Private Const TileHeight As Integer = 170
    Private Const TileGap As Integer = 20

    'a food tile only has a name, a price and how many have been added
    Private Const FoodTileWidth As Integer = 220
    Private Const FoodTileHeight As Integer = 130

    'a showing tile is shorter and narrower than a film one, there is far less on it
    Private Const TimeTileWidth As Integer = 240
    Private Const TimeTileHeight As Integer = 140

    'a seat on the kiosk map. the same map on the staff booking form uses 40 by 35 buttons 45 apart,
    'which is fine with a mouse but far too small for a finger, so everything here is bigger
    Private Const SeatWidth As Integer = 54
    Private Const SeatHeight As Integer = 46
    Private Const SeatGap As Integer = 8

    'a seat is never drawn smaller than this. under it two seats are close enough together that a
    'finger catches both, and a map nobody can press is worse than one that scrolls
    Private Const SmallestSeatWidth As Integer = 34

    'a bit of room kept on the right of the map for the scroll bar that appears when a screen has
    'more rows than fit. without it the bar sits over the last seat in every row
    Private Const SeatMapMargin As Integer = 24

    'the most tickets one person can buy at the machine in one go. anybody wanting more than this
    'is a party booking and is better off talking to somebody at the desk
    Private Const MaxSeatsPerSale As Integer = 8

    'the edge put round a seat that is not a standard one. blue is used for accessible rather than
    'a theme colour because it has to mean the same thing in light mode and dark mode, and because
    'it is the colour that already means accessible everywhere else people see it
    Private ReadOnly AccessibleEdge As Color = Color.FromArgb(45, 125, 210)

    'how much of the seat step is taken up by the list of what has been picked so far, the map
    'gets what is left over
    Private Const LeftColumnWidth As Integer = 330

    'how big the buttons at the bottom are. windows draws a normal button about 50 high once this
    'machine has scaled the designer down, which is under what a finger reliably hits, so they are
    'sized here in code instead where the scaling cannot get at them
    Private Const FooterHeight As Integer = 104
    Private Const FooterButtonHeight As Integer = 72

    'how long the machine sits untouched before it gives up and goes back to the welcome screen.
    'part way through an order it waits a while, because somebody may well be stood there deciding.
    'on the thank you screen it is much shorter, since that one is finished with and the next
    'person in the queue should not walk up to somebody else's booking number
    Private Const IdleSecondsAllowed As Integer = 90
    Private Const IdleSecondsOnThankYou As Integer = 25

    'how many days ahead the machine will sell. a week is what the posters in the foyer show, and
    'anything further out than that is somebody planning rather than somebody walking in
    Private Const DaysAhead As Integer = 7

    'the steps a customer goes through. they are only ever compared as text so they are kept as
    'constants the same way the log severities are, which means a typo is a compile error instead
    'of a step that quietly never shows up
    Private Const StepWelcome As String = "WELCOME"
    Private Const StepFilms As String = "FILMS"
    Private Const StepTimes As String = "TIMES"
    Private Const StepSeats As String = "SEATS"
    Private Const StepFood As String = "FOOD"
    Private Const StepConfirm As String = "CONFIRM"
    Private Const StepDone As String = "DONE"

    'which step is on the screen at the moment
    Private currentStep As String = StepWelcome

    'how many seconds it has been since anybody touched anything
    Private secondsIdle As Integer = 0

    'the day being looked at. it starts on today and the customer can move it along the week
    Private currentDay As Date = Date.Today

    'the film the customer has picked, 0 means they have not picked one yet
    Private currentFilmID As Long = 0
    Private currentFilmTitle As String = ""

    'the showing they picked off that film, and the bits of it the later steps need
    Private currentScreeningID As Long = 0
    Private currentScreenID As Long = 0
    Private currentTicketPrice As Double = 0
    Private currentShowingText As String = ""

    'the films drawn on the first step, kept so the title can be looked up again when a tile is
    'touched without going back to the database for something that has already been read
    Private filmsOnDay As DataTable

    'every seat in the screen the picked showing is in, with what sort of seat it is and what that
    'does to the price. read once when the map is drawn so touching a seat does not need another
    'look at the database just to find out what that one costs
    Private currentSeats As DataTable

    'the seats picked so far. the buttons change colour to match what is in here, the colour is
    'never what decides anything, the same way round as the staff booking form does it
    Private pickedSeats As DataTable

    'the food and drink added so far. same as the seats, it is only held here while the order is
    'being built up and nothing reaches the database until the customer pays
    Private pendingFood As DataTable

    'everything on sale at the counter, kept so the name and price can be looked up again when a
    'tile is touched without going back to the database for something already read
    Private foodOnSale As DataTable

    'the size the seats are actually being drawn at, which is not always the size above because a
    'wide screen full of seats has to be squashed to fit
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

    'the header and footer stretch across whatever screen this ends up running on, and the panel
    'holding the current step fills everything left in between. it is done here rather than in the
    'designer because a kiosk screen is not the same size as the one this was drawn on
    Private Sub LayoutKiosk()
        LayoutHeader()
        LayoutFooter()

        'every step panel gets the same rectangle, since only one of them is ever on show
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

    'the purple bar. the two lines of writing in it are AutoSize labels, so how tall they really
    'are depends on the font windows ends up using and not on the numbers in the designer. the bar
    'is made to fit round them instead of the other way about, which is why the second line used to
    'sit on top of the Filmtopia name
    Private Sub LayoutHeader()
        pnlHeader.Width = Me.ClientSize.Width

        lblKioskTitle.Top = 12
        lblStep.Top = lblKioskTitle.Bottom + 4
        pnlHeader.Height = lblStep.Bottom + 14

        btnExitKiosk.Left = pnlHeader.Width - btnExitKiosk.Width - 30
        btnExitKiosk.Top = (pnlHeader.Height - btnExitKiosk.Height) \ 2
    End Sub

    'back is always bottom left and continue is always bottom right, whatever step is on the
    'screen. a customer should not have to look for the way on each time the screen changes
    Private Sub LayoutFooter()
        pnlFooter.Height = FooterHeight
        pnlFooter.Width = Me.ClientSize.Width
        pnlFooter.Top = Me.ClientSize.Height - pnlFooter.Height

        btnBack.Size = New Size(220, FooterButtonHeight)
        btnNext.Size = New Size(240, FooterButtonHeight)

        btnBack.Left = 32
        btnBack.Top = (pnlFooter.Height - btnBack.Height) \ 2

        'the total sits just inside continue. it is a label that grows with its own text so where
        'it starts has to be worked back from its right hand edge
        btnNext.Left = pnlFooter.Width - btnNext.Width - 32
        btnNext.Top = btnBack.Top
        lblRunningTotal.Left = btnNext.Left - lblRunningTotal.Width - 40
        lblRunningTotal.Top = (pnlFooter.Height - lblRunningTotal.Height) \ 2

        'the version goes in the empty middle of the footer, out of the way of both buttons
        lblVersion.Left = btnBack.Right + 40
        lblVersion.Top = pnlFooter.Height - lblVersion.Height - 12
    End Sub

    'the list of films fills its step, starting under the heading rather than at a number typed
    'into the designer, because how tall the heading draws depends on its font
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

    'the food step is the same two columns as the seats one, what has been added down the left and
    'the things that can be added filling the rest
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

    'the order goes down the left and the total sits underneath it, big enough that nobody presses
    'pay without having seen it. the note about the seats not being held goes under that
    Private Sub LayoutConfirmStep()
        lblConfirmDetail.Top = lblConfirmHeading.Bottom + 20
        lblConfirmDetail.Height = pnlConfirm.Height - lblConfirmDetail.Top - 130

        lblConfirmTotal.Top = lblConfirmDetail.Bottom + 10
        lblConfirmNote.Top = lblConfirmTotal.Bottom + 8
    End Sub

    'the thank you screen is all one column down the middle. it is centred rather than lined up on
    'the left because there is nothing to compare it against, it is just being read
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

    'the seat step is in two columns, the seats picked so far down the left and the map itself in
    'the middle with its key underneath. the key belongs to the map so it lines up with the map,
    'putting it in the left column left it sat on top of the list
    Private Sub LayoutSeatsStep()
        lblSeatsShowing.Top = lblSeatsHeading.Bottom + 6

        lblSeatsPicked.Top = lblSeatsShowing.Bottom + 24
        lblSeatsPicked.Height = pnlSeats.Height - lblSeatsPicked.Top - 30

        'the map is only worth moving about once there is one drawn
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

        lblSeatKeyTypes.Left = pnlSeatMap.Left
        lblSeatKeyTypes.Top = lblKeyAvailable.Bottom + 10
    End Sub

    'gives one step panel the whole of the space between the header and the footer
    Private Sub SizeStepPanel(pnl As Panel, contentTop As Integer, contentHeight As Integer)
        pnl.Left = 0
        pnl.Top = contentTop
        pnl.Width = Me.ClientSize.Width
        pnl.Height = contentHeight
    End Sub

    'puts the welcome wording and the start button in the middle of the screen. the labels grow
    'and shrink with their own text so where they start has to be worked out, it cannot just be
    'typed into the designer
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
        'the buttons are painted last, after this has decided which of them show, what they say and
        'whether they can be pressed. doing it first left Finish greyed off on the thank you screen,
        'because at that point it was still switched off from the seats step
        StyleKioskButtons()

        LayoutKiosk()
    End Sub

    'puts one step on the screen and hides the rest of them. everything about what the customer
    'can see and do belongs in here, so there is one place that decides what a step looks like
    'rather than every button doing its own showing and hiding
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

        'the wording under the Filmtopia name says where the customer is up to
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

        'there is nothing to go back to from the welcome screen, and once the sale is made going
        'back would only mean paying for the same seats twice
        btnBack.Visible = (stepName <> StepWelcome And stepName <> StepDone)

        'the first two steps are answered by touching a tile, so a continue button on them would
        'only be something else to press. it appears when there is a running total to carry on with
        btnNext.Visible = (stepName = StepSeats Or stepName = StepFood Or
                          stepName = StepConfirm Or stepName = StepDone)
        lblRunningTotal.Visible = (stepName = StepSeats Or stepName = StepFood)

        'the button says what pressing it is about to do. carrying on, paying and finishing are
        'three different things and the middle one wants saying out loud
        If stepName = StepConfirm Then
            btnNext.Text = "Pay now"
            btnNext.Enabled = True
        ElseIf stepName = StepDone Then
            btnNext.Text = "Finish"
            btnNext.Enabled = True
        ElseIf stepName = StepFood Then
            'nothing has to be bought on this step so it is always pressable, and it says no thanks
            'rather than continue because that is the answer most people are giving it
            btnNext.Text = NextTextForFood()
            btnNext.Enabled = True
        Else
            btnNext.Text = "Continue"
        End If

        LayoutKiosk()
    End Sub

    'the films that still have a showing left today. a film with nothing but screenings that have
    'already started is no use to somebody stood at the machine, so those are left out.
    'ScreeningTime is text in HH:MM with the zero always on the front, so comparing it against the
    'time now as text puts them in the right order without having to turn every row into a number
    Private Sub LoadFilmsForDay()
        filmsOnDay = New DataTable
        Dim dt As DataTable = filmsOnDay

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            'DISTINCT because a film on three times in a day should still only be one tile
            SQLCmd.CommandText = "SELECT DISTINCT tblFilm.FilmID, FilmTitle, FilmAgeRating, FilmDuration " &
                                 "FROM tblFilm INNER JOIN tblScreening ON tblFilm.FilmID = tblScreening.FilmID " &
                                 "WHERE ScreeningDate = @Day AND ScreeningTime >= @EarliestTime " &
                                 "ORDER BY FilmTitle"
            SQLCmd.Parameters.AddWithValue("@Day", currentDay)
            SQLCmd.Parameters.AddWithValue("@EarliestTime", EarliestTimeForDay())
            Dim da As New OleDbDataAdapter(SQLCmd)
            da.Fill(dt)
            cn.Close()
        End If

        lblFilmsHeading.Text = HeadingForDay()
        BuildFilmTiles(dt)

        'if there is genuinely nothing on that day, say so rather than leaving a blank screen that
        'looks like the machine has gone wrong
        lblNoFilms.Visible = (dt.Rows.Count = 0)

        If currentDay = Date.Today Then
            lblNoFilms.Text = "There is nothing left on today, try another day"
        Else
            lblNoFilms.Text = "There is nothing on that day, try another one"
        End If
    End Sub

    'the heading over the list. today and tomorrow get the word because that is what people say,
    'any other day gets its date
    Private Function HeadingForDay() As String
        If currentDay = Date.Today Then
            Return "What's on today"
        ElseIf currentDay = Date.Today.AddDays(1) Then
            Return "What's on tomorrow"
        End If

        Return "What's on " & Format(currentDay, "dddd d MMMM")
    End Function

    'the earliest showing worth offering on the day being looked at. on today that is the time now,
    'because a showing that has already started is no use to somebody stood at the machine. on any
    'other day the whole day is still to come, so it is midnight
    Private Function EarliestTimeForDay() As String
        If currentDay = Date.Today Then
            Return Format(Now, "HH:mm")
        End If

        Return "00:00"
    End Function

    'what a day is called on the buttons and in the heading. the first two get words because that
    'is what people say, the rest get the date
    Private Function DayName(theDay As Date) As String
        If theDay = Date.Today Then
            Return "Today"
        ElseIf theDay = Date.Today.AddDays(1) Then
            Return "Tomorrow"
        End If

        Return Format(theDay, "ddd d MMM")
    End Function

    'makes the row of day buttons across the top of the film list, today first and then the rest
    'of the week. they are built here because which days they are depends on what day it is
    Private Sub BuildDayPicker()
        pnlDayPicker.Controls.Clear()

        Dim i As Integer

        For i = 0 To DaysAhead - 1
            Dim theDay As Date = Date.Today.AddDays(i)

            Dim b As New Button
            b.Tag = theDay
            b.Text = DayName(theDay)
            b.Font = New Font("Segoe UI", 11)
            b.FlatStyle = FlatStyle.Flat
            b.FlatAppearance.BorderColor = BorderCol

            'the day being looked at is the pink one, so it is obvious which list is on screen
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

    'shares the width out between the day buttons rather than giving them a fixed size. seven
    'buttons at a size that looked right on one screen ran off the edge of a narrower one, and a
    'day the customer cannot see is a day they cannot buy for
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

    'a different day was picked, so the whole list is read again for it
    Private Sub DayButton_Click(sender As Object, e As EventArgs)
        Touched()

        Dim b As Button = CType(sender, Button)
        currentDay = CDate(b.Tag)

        BuildDayPicker()
        LoadFilmsForDay()
        LayoutFilmsStep()
    End Sub

    'makes one tile per film. they are built here rather than being put in the designer because
    'how many there are depends on what is on that day
    Private Sub BuildFilmTiles(dtFilms As DataTable)
        pnlFilmList.Controls.Clear()

        Dim i As Integer
        For i = 0 To dtFilms.Rows.Count - 1
            Dim filmID As Long = CLng(dtFilms.Rows(i)("FilmID"))
            Dim title As String = dtFilms.Rows(i)("FilmTitle").ToString()
            Dim rating As String = dtFilms.Rows(i)("FilmAgeRating").ToString()
            Dim duration As Integer = CInt(dtFilms.Rows(i)("FilmDuration"))

            'the tile itself. it is called pnlCard... so the theme treats it the same way it treats
            'the cards on the main menu, which means it changes with dark mode without extra code
            Dim tile As New Panel
            tile.Name = "pnlCardFilm" & filmID
            tile.Size = New Size(TileWidth, TileHeight)
            tile.BackColor = CardBack
            tile.Cursor = Cursors.Hand
            tile.Tag = filmID

            'the pink strip down the side, the same one the main menu cards have
            Dim strip As New Panel
            strip.Name = "pnlAccentFilm" & filmID
            strip.Location = New Point(0, 0)
            strip.Size = New Size(8, TileHeight)
            strip.BackColor = HighlightBack
            tile.Controls.Add(strip)

            'the title is allowed two or three lines, film titles get long and cutting one off
            'halfway is no help to somebody deciding what to watch
            Dim lblTitle As New Label
            lblTitle.AutoSize = False
            lblTitle.Location = New Point(26, 20)
            lblTitle.Size = New Size(TileWidth - 50, 84)
            lblTitle.Font = New Font("Segoe UI", 15, FontStyle.Bold)
            lblTitle.ForeColor = TextFore
            lblTitle.Text = title
            lblTitle.Tag = filmID
            tile.Controls.Add(lblTitle)

            Dim lblMeta As New Label
            lblMeta.AutoSize = True
            lblMeta.Location = New Point(26, TileHeight - 44)
            lblMeta.Font = New Font("Segoe UI", 11)
            lblMeta.ForeColor = SubtleFore
            lblMeta.Text = rating & "   -   " & RunningTime(duration)
            lblMeta.Tag = filmID
            tile.Controls.Add(lblMeta)

            'the whole tile answers to a touch, not just the middle of it. the labels sit on top of
            'the panel so a finger landing on the title would otherwise do nothing at all
            AddHandler tile.Click, AddressOf FilmTile_Click
            AddHandler lblTitle.Click, AddressOf FilmTile_Click
            AddHandler lblMeta.Click, AddressOf FilmTile_Click

            pnlFilmList.Controls.Add(tile)
        Next

        ArrangeFilmTiles()
    End Sub

    'works out where each tile in a list goes. how many fit on a row depends on how wide the screen
    'is, so it is worked out again whenever the window changes size rather than being fixed.
    'the film list and the showings list are both laid out by this, they only differ in tile size
    Private Sub ArrangeTiles(pnl As Panel, tileWidth As Integer, tileHeight As Integer)
        Dim perRow As Integer = (pnl.Width - TileGap) \ (tileWidth + TileGap)

        'a very narrow screen still has to show one tile per row rather than none at all
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

    'turns a length in minutes into something a customer reads, so 118 comes out as 1h 58m
    Private Function RunningTime(minutes As Integer) As String
        Return (minutes \ 60) & "h " & Format(minutes Mod 60, "00") & "m"
    End Function

    'a film has been picked. the FilmID is kept in Tag on the tile and on each of its labels, so
    'whichever part of it got touched the answer is the same
    Private Sub FilmTile_Click(sender As Object, e As EventArgs)
        Touched()

        Dim ctrl As Control = CType(sender, Control)
        currentFilmID = CLng(ctrl.Tag)
        currentFilmTitle = TitleOfFilm(currentFilmID)

        LoadShowingsForFilm()
        ShowStep(StepTimes)
    End Sub

    'the title of a film that has already been read onto the first step
    Private Function TitleOfFilm(filmID As Long) As String
        Dim rows() As DataRow = filmsOnDay.Select("FilmID = " & filmID)

        If rows.Length > 0 Then
            Return rows(0)("FilmTitle").ToString()
        End If

        Return ""
    End Function

    'every showing of the picked film that has not started yet today, with the screen it is in and
    'what a standard ticket costs. how full each one is comes afterwards, one showing at a time,
    'because counting the seats sold inside this query would mean a join inside a subquery and Jet
    'refuses to run that once the tables have keys on them
    Private Sub LoadShowingsForFilm()
        Dim dt As New DataTable

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT tblScreening.ScreeningID, ScreeningTime, TicketPrice, " &
                                 "tblScreening.ScreenID, ScreenName, ScreenCapacity " &
                                 "FROM tblScreening INNER JOIN tblScreen ON tblScreening.ScreenID = tblScreen.ScreenID " &
                                 "WHERE FilmID = @FilmID AND ScreeningDate = @Day AND ScreeningTime >= @EarliestTime " &
                                 "ORDER BY ScreeningTime"
            SQLCmd.Parameters.AddWithValue("@FilmID", CInt(currentFilmID))
            SQLCmd.Parameters.AddWithValue("@Day", currentDay)
            SQLCmd.Parameters.AddWithValue("@EarliestTime", EarliestTimeForDay())
            Dim da As New OleDbDataAdapter(SQLCmd)
            da.Fill(dt)
            cn.Close()
        End If

        lblTimesFilm.Text = currentFilmTitle
        BuildTimeTiles(dt)
    End Sub

    'how many seats have gone on a screening. the screening is written on the seat row itself, so
    'this is one table and needs no join
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

    'makes one tile per showing, with the time in big writing and the screen and how many seats are
    'left underneath it. a showing with nothing left is still drawn, greyed out and saying sold out,
    'because leaving it off the screen would just make the customer wonder where it had gone
    Private Sub BuildTimeTiles(dtShowings As DataTable)
        pnlTimeList.Controls.Clear()

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
                'nothing left, so it is shown but it does not answer to a touch
                strip.BackColor = SubtleFore
                lblTime.ForeColor = SubtleFore
                lblMeta.Text = screenName & vbNewLine & "Sold out"
            End If

            pnlTimeList.Controls.Add(tile)
        Next

        ArrangeTimeTiles()
    End Sub

    'a showing has been picked, so everything the later steps need about it is kept
    Private Sub TimeTile_Click(sender As Object, e As EventArgs)
        Touched()

        Dim ctrl As Control = CType(sender, Control)
        currentScreeningID = CLng(ctrl.Tag)

        LoadShowingDetails()
        WriteLog("KIOSK", "Customer picked screening " & currentScreeningID & " (" & currentShowingText & ")")

        BuildSeatMap()
        ShowStep(StepSeats)
    End Sub

    'makes the empty table that holds the seats picked for this sale
    Private Sub SetUpPickedSeats()
        pickedSeats = New DataTable
        pickedSeats.Columns.Add("SeatID", GetType(Integer))
        pickedSeats.Columns.Add("SeatName", GetType(String))
        pickedSeats.Columns.Add("SeatType", GetType(String))
        'the multiplier travels with the seat so the running total can be added up without going
        'back to the database every time somebody touches one
        pickedSeats.Columns.Add("Multiplier", GetType(Double))
    End Sub

    'says whether a seat has been picked for this sale
    Private Function IsSeatPicked(seatID As Long) As Boolean
        Return pickedSeats.Select("SeatID = " & seatID).Length > 0
    End Function

    'takes the seat colours from whichever theme is on so the map works in dark mode too
    Private Sub ApplySeatColours()
        lblSwatchAvailable.BackColor = SeatAvailable
        lblSwatchSelected.BackColor = SeatSelected
        lblSwatchTaken.BackColor = SeatTaken
    End Sub

    'draws a button for every seat in the screen this showing is in and greys out the ones that
    'have already gone
    Private Sub BuildSeatMap()
        ApplySeatColours()
        pnlSeatMap.Controls.Clear()
        lblSeatsShowing.Text = currentShowingText

        'a fresh showing means nothing carries over from the last one somebody looked at
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

            'the seats already sold on this showing. the screening is written on the seat row
            'itself so this is one table and needs no join
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
            'where it goes and how big it is are decided by ArrangeSeatMap, because both depend on
            'how much room the screen has and that is not known until the map is being laid out

            'anything that is not a plain standard seat gets an edge round it, so the difference can
            'be seen without the background having to say it. the background is busy saying whether
            'the seat is free, picked or gone and the two would only fight over the same colour.
            'accessible seats used to get nothing at all because they are the same price as a
            'standard one, and the marking was going off the price rather than what sort of seat it
            'is, which is exactly the wrong thing to hang it on
            If seatType = SeatPremium Then
                b.FlatAppearance.BorderSize = 3
                b.FlatAppearance.BorderColor = AccentFore
            ElseIf seatType = SeatAccessible Then
                b.FlatAppearance.BorderSize = 3
                b.FlatAppearance.BorderColor = AccessibleEdge
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

    'puts the map and the SCREEN bar above it in the middle of the space to the right of the list
    'of picked seats. how wide the map is depends on how many seats are in a row, so it cannot be
    'a number typed into the designer
    Private Sub CentreSeatMap()
        Dim seatsAcross As Integer = WidestRow()
        Dim rowsDown As Integer = DeepestRow()

        'what the map has to play with once the list down the left and the key underneath have had
        'their share
        Dim spaceAcross As Integer = pnlSeats.Width - LeftColumnWidth - 30

        lblScreen.Top = lblSeatsShowing.Bottom + 24
        Dim mapTop As Integer = lblScreen.Bottom + 16
        'the key and the line about the seat edges both sit under the map, so that much is kept back
        Dim spaceDown As Integer = pnlSeats.Height - mapTop - 115

        'the whole room has to fit on the screen at once. a seat map you have to scroll around is
        'no use to somebody choosing where to sit, they need to see the shape of it, so when there
        'is not enough room the seats get smaller rather than the map getting scroll bars
        seatDrawWidth = LargestSeatThatFits(seatsAcross, rowsDown, spaceAcross, spaceDown)

        'the seats keep the shape they were drawn at rather than turning into squares
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

        'the panel is only made as tall as the seats actually need. if it was given the whole of
        'the space left the key underneath it would end up floating a long way below the back row
        Dim mapHeight As Integer = (rowsDown * (seatDrawHeight + SeatGap)) - SeatGap

        If mapHeight > spaceDown Then
            mapHeight = spaceDown
        End If

        pnlSeatMap.Height = mapHeight

        ArrangeSeatMap()
    End Sub

    'the biggest a seat can be drawn and still have the whole room fit in the space it has been
    'given, both ways. it works out what would fit across and what would fit down and takes
    'whichever is the smaller of the two, since a seat has to satisfy both at once
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
            'worked out in height first then turned back into a width, because the two are tied
            'together and it is the width everything else is measured from
            'a gap is taken off the height first so there is always a little slack. without it the
            'sums came out exactly the right size and the panel put a scroll bar up anyway
            Dim heightThatFits As Integer = ((spaceDown - SeatGap) \ rowsDown) - SeatGap
            Dim fitsDown As Integer = (heightThatFits * SeatWidth) \ SeatHeight

            If fitsDown < biggest Then
                biggest = fitsDown
            End If
        End If

        'below this a finger cannot land on one seat without catching the one next to it, so it is
        'better to let it scroll than to draw something nobody can press
        If biggest < SmallestSeatWidth Then
            biggest = SmallestSeatWidth
        End If

        Return biggest
    End Function

    'how many rows of seats there are, which is what decides how tall the map has to be
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

    'puts every seat button where it belongs at whatever size was just worked out. the buttons were
    'made in the same order the seats were read, so row i of the table is button i on the map
    Private Sub ArrangeSeatMap()
        If currentSeats Is Nothing Then
            Exit Sub
        End If

        Dim i As Integer
        For i = 0 To pnlSeatMap.Controls.Count - 1
            Dim seatRow As String = currentSeats.Rows(i)("SeatRow").ToString()
            Dim seatNumber As Integer = CInt(currentSeats.Rows(i)("SeatNumber"))

            'the row letter A,B,C says how far down and the seat number says how far across
            Dim rowIndex As Integer = Asc(seatRow) - 65

            pnlSeatMap.Controls(i).Size = New Size(seatDrawWidth, seatDrawHeight)
            pnlSeatMap.Controls(i).Left = (seatNumber - 1) * (seatDrawWidth + SeatGap)
            pnlSeatMap.Controls(i).Top = rowIndex * (seatDrawHeight + SeatGap)

            'the writing on a seat has to shrink with the seat or it stops fitting on it
            pnlSeatMap.Controls(i).Font = New Font("Segoe UI", SeatFontSize())
        Next
    End Sub

    'how big the seat letter and number is drawn, worked out from the seat rather than fixed, so a
    'squashed up map does not end up with A10 hanging out over the edge of its own button
    Private Function SeatFontSize() As Single
        If seatDrawWidth >= 50 Then
            Return 10
        ElseIf seatDrawWidth >= 42 Then
            Return 9
        End If

        Return 7.5
    End Function

    'how many seats are in the longest row, which is what decides how wide the map has to be
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

    'turns a seat on or off. the table is what changes, the colour is only put on afterwards to
    'show what the table now says
    Private Sub Seat_Click(sender As Object, e As EventArgs)
        Touched()

        Dim b As Button = CType(sender, Button)
        Dim seatID As Long = CLng(b.Tag)

        If IsSeatPicked(seatID) Then
            Dim rows() As DataRow = pickedSeats.Select("SeatID = " & seatID)
            pickedSeats.Rows.Remove(rows(0))
            b.BackColor = SeatAvailable
        Else
            'a machine in a foyer is not the place to sell a party twenty tickets, and letting
            'somebody fill a whole screen by leaning on it would be worse
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

    'what sort of seat it is, out of the seats that were read when the map was drawn
    Private Function TypeOfSeat(seatID As Long) As String
        Dim rows() As DataRow = currentSeats.Select("SeatID = " & seatID)

        If rows.Length > 0 Then
            Return rows(0)("SeatTypeName").ToString()
        End If

        Return SeatStandard
    End Function

    'what a seat does to the price, out of the seats that were read when the map was drawn
    Private Function MultiplierForSeat(seatID As Long) As Double
        Dim rows() As DataRow = currentSeats.Select("SeatID = " & seatID)

        If rows.Length > 0 Then
            Return CDbl(rows(0)("PriceMultiplier"))
        End If

        'if it cannot be found the seat is charged as a standard one, which is the safe way round
        Return 1
    End Function

    'adds up what the picked seats come to. they go on one at a time rather than being counted and
    'multiplied, because a premium seat is worth more than a standard one
    Private Function TicketsTotal() As Double
        Dim total As Double = 0
        Dim i As Integer

        For i = 0 To pickedSeats.Rows.Count - 1
            total = total + SeatPrice(currentTicketPrice, CDbl(pickedSeats.Rows(i)("Multiplier")))
        Next

        Return total
    End Function

    'writes out what has been picked so far down the left hand side, and puts the total in the
    'footer. continue only becomes pressable once there is at least one seat
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

    'the screen and the ticket price of the picked showing, read once here so the seat map and the
    'running total do not have to keep asking for them
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

    'the start button is the size it is on purpose, but the whole welcome screen answers to a touch
    'as well. somebody walking up to a machine should not have to aim at anything
    Private Sub Welcome_Click(sender As Object, e As EventArgs) Handles btnStart.Click, pnlWelcome.Click,
        lblWelcomeTitle.Click, lblWelcomeSub.Click

        'both are read again every time rather than once when the form opens, otherwise a machine
        'left on all day would still be offering this morning's showings and yesterday's dates
        BuildDayPicker()
        LoadFilmsForDay()
        ShowStep(StepFilms)
    End Sub

    'makes the empty table that holds the food added to this order. the columns match what
    'CompleteSale expects, so the same table goes straight into the sale without being copied
    Private Sub SetUpPendingFood()
        pendingFood = New DataTable
        pendingFood.Columns.Add("FoodItemID", GetType(Integer))
        pendingFood.Columns.Add("Item", GetType(String))
        pendingFood.Columns.Add("Price", GetType(Double))
        pendingFood.Columns.Add("Quantity", GetType(Integer))
    End Sub

    'everything on sale at the counter, drawn as tiles the same way the films are
    Private Sub LoadFoodItems()
        foodOnSale = New DataTable
        Dim dt As DataTable = foodOnSale

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT FoodItemID, FoodItemName, FoodItemPrice " &
                                 "FROM tblFoodItem ORDER BY FoodItemName"
            Dim da As New OleDbDataAdapter(SQLCmd)
            da.Fill(dt)
            cn.Close()
        End If

        BuildFoodTiles(dt)
        UpdateFoodOrder()
    End Sub

    'one tile per thing on sale. touching it adds one, and once there is at least one on the order
    'the tile says how many so the customer can see what they have done without reading the list
    Private Sub BuildFoodTiles(dtFood As DataTable)
        pnlFoodList.Controls.Clear()

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

            Dim lblName As New Label
            lblName.AutoSize = False
            'the name gets the top of the tile to itself, all the way across. it used to be tall
            'enough to reach the take one off button and was drawing over the top of it
            lblName.Location = New Point(24, 14)
            lblName.Size = New Size(FoodTileWidth - 44, 46)
            lblName.Font = New Font("Segoe UI", 12, FontStyle.Bold)
            lblName.ForeColor = TextFore
            lblName.Text = itemName
            lblName.Tag = foodID
            tile.Controls.Add(lblName)

            Dim lblPrice As New Label
            lblPrice.AutoSize = True
            lblPrice.Location = New Point(24, FoodTileHeight - 40)
            lblPrice.Font = New Font("Segoe UI", 12)
            lblPrice.ForeColor = SubtleFore
            lblPrice.Text = FormatCurrency(price)
            lblPrice.Tag = foodID
            tile.Controls.Add(lblPrice)

            'how many of this one are on the order. it is made now and left empty, so that adding
            'one only has to change the writing on it rather than build the whole tile again
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

            'taking one back off. it only shows up once there is something to take off. it is in the
            'bottom corner on its own, well away from the name, so that the big easy thing to hit
            'is still adding one and taking one off has to be aimed at
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

            'the whole tile answers to a touch, the labels sit on top of it so a finger landing on
            'the name would otherwise do nothing
            AddHandler tile.Click, AddressOf FoodTile_Click
            AddHandler lblName.Click, AddressOf FoodTile_Click
            AddHandler lblPrice.Click, AddressOf FoodTile_Click
            AddHandler lblCount.Click, AddressOf FoodTile_Click

            pnlFoodList.Controls.Add(tile)
        Next

        ArrangeTiles(pnlFoodList, FoodTileWidth, FoodTileHeight)
    End Sub

    'adds one of whatever was touched. if it is already on the order it just goes up by one rather
    'than appearing twice, the same way the till screen does it
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

    'takes one of something back off the order. a tile is easy to catch by accident on a screen
    'people prod at, so there has to be a way back that is not starting the whole order again
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
                'the last one has gone so the line comes off altogether rather than sitting there
                'saying nought
                pendingFood.Rows.Remove(rows(0))
            End If
        End If

        UpdateFoodOrder()
    End Sub

    'puts a new line on the order. the name and the price come out of what was read when the tiles
    'were drawn, the same way the film title is looked up, rather than asking the database again
    'for something that is already on the screen
    Private Sub AddFoodLine(foodID As Long)
        Dim rows() As DataRow = foodOnSale.Select("FoodItemID = " & foodID)

        If rows.Length = 0 Then
            Exit Sub
        End If

        pendingFood.Rows.Add(CInt(foodID), rows(0)("FoodItemName").ToString(),
                             CDbl(rows(0)("FoodItemPrice")), 1)
    End Sub

    'writes out what has been added down the left, puts the count on each tile and works the
    'running total out again
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

    'puts the number added onto each tile, or clears it off again when there are none
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

            'the take one off button is only any use once there is something on the order
            Dim btnLess As Control = pnlFoodList.Controls(i).Controls("btnFoodLess" & foodID)

            If btnLess IsNot Nothing Then
                btnLess.Visible = (pendingFood.Select("FoodItemID = " & foodID).Length > 0)
            End If
        Next
    End Sub

    'the button says no thanks until something has been added, because carrying on with an empty
    'order is what most people want and it should not look like they have missed a step
    Private Function NextTextForFood() As String
        If pendingFood Is Nothing OrElse pendingFood.Rows.Count = 0 Then
            Return "No thanks"
        End If

        Return "Continue"
    End Function

    'what the food comes to
    Private Function FoodTotal() As Double
        Dim total As Double = 0
        Dim i As Integer

        For i = 0 To pendingFood.Rows.Count - 1
            total = total + (CDbl(pendingFood.Rows(i)("Price")) * CInt(pendingFood.Rows(i)("Quantity")))
        Next

        Return total
    End Function

    'the whole order, tickets and food. everything that shows a total goes through here so the
    'running total, the confirmation and what actually gets saved cannot disagree
    Private Function OrderTotal() As Double
        Return TicketsTotal() + FoodTotal()
    End Function

    'writes out the whole order in plain english before any money is taken. everything on here has
    'already been worked out on the step before, it is not added up again, so what the customer is
    'shown to agree to is exactly what they were shown while they were picking
    Private Sub BuildConfirmation()
        Dim detail As String = currentShowingText & vbNewLine & vbNewLine

        Dim i As Integer
        For i = 0 To pickedSeats.Rows.Count - 1
            Dim seatName As String = pickedSeats.Rows(i)("SeatName").ToString()
            Dim seatType As String = pickedSeats.Rows(i)("SeatType").ToString()
            Dim price As Double = SeatPrice(currentTicketPrice, CDbl(pickedSeats.Rows(i)("Multiplier")))

            detail = detail & "Seat " & seatName

            'anything that is not a plain seat is said out loud, so nobody gets to the total and
            'wonders why it is more than the price on the poster, and so anybody who has picked an
            'accessible seat can see that is what they have got
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

    'goes back a step. the welcome screen is the one place it cannot be pressed
    Private Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        If currentStep = StepFilms Then
            ShowStep(StepWelcome)
        ElseIf currentStep = StepTimes Then
            'going back to the list of films reads it again, because a showing could have started
            'while the customer was stood there deciding
            LoadFilmsForDay()
            ShowStep(StepFilms)
        ElseIf currentStep = StepSeats Then
            'the seats picked are thrown away on the way back, and the list of showings is read
            'again so how many are left is right rather than however full it was a minute ago
            LoadShowingsForFilm()
            ShowStep(StepTimes)
        ElseIf currentStep = StepFood Then
            'the map is drawn again on the way back rather than being left as it was, because
            'somebody else could have bought one of those seats while this order sat on screen.
            'that does mean the seats picked are lost, which is annoying but a lot less annoying
            'than being shown a seat as free that has already gone
            BuildSeatMap()
            ShowStep(StepSeats)
        ElseIf currentStep = StepConfirm Then
            ShowStep(StepFood)
        End If
    End Sub

    'the button in the bottom right. what it does depends on which step is on the screen
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

    'makes the sale. nothing at all has been written to the database up to this point, so a
    'customer who walks away part way through leaves nothing behind.
    'the whole sale goes through CompleteSale, the same routine the till uses, rather than the
    'kiosk having its own way of writing a booking. that routine already checks the seats are
    'still free inside its own transaction and works the seat prices out, and having a second
    'version of all that on here is exactly how the two screens would end up disagreeing
    Private Sub TakePayment()
        Dim seatCount As Integer = pickedSeats.Rows.Count

        If seatCount = 0 Then
            Exit Sub
        End If

        'the kiosk does not know who anybody is, so every sale it makes is a walk-in. the food goes
        'in with it, and if nothing was added the table is simply empty
        Dim total As Double = OrderTotal()
        Dim newBookingID As Long = CompleteSale(0, True, currentScreeningID, PickedSeatIDs(), pendingFood, total)

        If newBookingID = 0 Then
            'nothing was saved and CompleteSale has already said why. the most likely reason is
            'somebody else took one of these seats, so the map is drawn again and they start the
            'seat picking over rather than being left looking at an order that cannot happen
            BuildSeatMap()
            ShowStep(StepSeats)
            Exit Sub
        End If

        WriteLog("KIOSK", "Kiosk sale " & newBookingID & ", " & seatCount & " seat(s) and " &
                          pendingFood.Rows.Count & " food line(s), " & FormatCurrency(total))

        BuildReceipt(newBookingID, seatCount, total)
        ShowStep(StepDone)
    End Sub

    'collects the SeatID of every seat picked, ready to be saved
    Private Function PickedSeatIDs() As Long()
        Dim seatIDs(pickedSeats.Rows.Count - 1) As Long
        Dim i As Integer

        For i = 0 To pickedSeats.Rows.Count - 1
            seatIDs(i) = CLng(pickedSeats.Rows(i)("SeatID"))
        Next

        Return seatIDs
    End Function

    'what the customer is left looking at once they have paid. the booking number is the biggest
    'thing on it because that is what they will be asked for at the door
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

    'clears everything down ready for whoever walks up next. it is deliberately a full reset, a
    'kiosk that remembers the last person's order is a kiosk that sells somebody the wrong thing
    Private Sub StartAgain()
        currentDay = Date.Today
        currentFilmID = 0
        currentFilmTitle = ""
        currentScreeningID = 0
        currentScreenID = 0
        currentTicketPrice = 0
        currentShowingText = ""

        SetUpPickedSeats()
        SetUpPendingFood()
        pnlSeatMap.Controls.Clear()
        pnlFoodList.Controls.Clear()
        currentSeats = Nothing

        ShowStep(StepWelcome)
    End Sub

    'the buttons a customer presses are painted here rather than being left as windows draws them.
    'the one that carries on is the pink one and it is the only pink thing on the screen, so on a
    'machine somebody walks up to and uses without being shown how, where to press is obvious.
    'it is called from ShowStep because the theme repaints every button when the form is coloured
    'and would otherwise put them all back to grey
    Private Sub StyleKioskButtons()
        PaintKioskButton(btnStart, True)
        PaintKioskButton(btnNext, True)
        PaintKioskButton(btnBack, False)
    End Sub

    'one button. the main one is filled in, the ones that only go backwards are left plain with a
    'border, so they can still be found without competing with the way forward
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

        'a button that cannot be pressed yet is greyed off, otherwise a bright pink Continue that
        'does nothing when you press it just looks like the machine has stopped working
        If Not btn.Enabled Then
            btn.BackColor = ReadOnlyBack
            btn.ForeColor = SubtleFore
        End If
    End Sub

    'anything the customer does calls this, so the machine knows it is still being used. every
    'handler on the form goes through it rather than trying to listen for the mouse itself, because
    'a click on a seat button never reaches the form underneath it
    Private Sub Touched()
        secondsIdle = 0
    End Sub

    'ticks once a second and counts how long the machine has been left alone. an order somebody has
    'walked away from is cleared down, both so the next person starts fresh and so a half picked
    'order is not left on a screen in a public foyer
    Private Sub timerIdle_Tick(sender As Object, e As EventArgs) Handles timerIdle.Tick
        'nothing to time out on the welcome screen, it is already where it would go back to
        If currentStep = StepWelcome Then
            Exit Sub
        End If

        secondsIdle = secondsIdle + 1

        Dim allowed As Integer = IdleSecondsAllowed
        If currentStep = StepDone Then
            allowed = IdleSecondsOnThankYou
        End If

        If secondsIdle >= allowed Then
            'a sale that has already gone through is finished with, anything before that is being
            'abandoned and is worth a line in the log so it can be seen how often it happens
            If currentStep <> StepDone Then
                WriteLog("KIOSK", "Order left unfinished on the " & currentStep & " step, kiosk reset itself")
            End If

            StartAgain()
        End If
    End Sub

    'a real kiosk has no way out for a customer, so this is the way a member of staff gets back to
    'the rest of the program. it asks first because pressing it by accident in front of a queue
    'would put the till screen up on a public display
    Private Sub btnExitKiosk_Click(sender As Object, e As EventArgs) Handles btnExitKiosk.Click
        Dim answer As DialogResult = MessageBox.Show("Close the kiosk and go back to the main menu?",
                                                     "Staff Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        If answer = DialogResult.Yes Then
            WriteLog("KIOSK", "Kiosk closed by staff")
            Me.Close()
        End If
    End Sub

End Class
