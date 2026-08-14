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

    'a showing tile is shorter and narrower than a film one, there is far less on it
    Private Const TimeTileWidth As Integer = 240
    Private Const TimeTileHeight As Integer = 140

    'a seat on the kiosk map. the same map on the staff booking form uses 40 by 35 buttons 45 apart,
    'which is fine with a mouse but far too small for a finger, so everything here is bigger
    Private Const SeatWidth As Integer = 54
    Private Const SeatHeight As Integer = 46
    Private Const SeatGap As Integer = 8

    'the most tickets one person can buy at the machine in one go. anybody wanting more than this
    'is a party booking and is better off talking to somebody at the desk
    Private Const MaxSeatsPerSale As Integer = 8

    'how much of the seat step is taken up by the list of what has been picked so far, the map
    'gets what is left over
    Private Const LeftColumnWidth As Integer = 330

    'the steps a customer goes through. they are only ever compared as text so they are kept as
    'constants the same way the log severities are, which means a typo is a compile error instead
    'of a step that quietly never shows up
    Private Const StepWelcome As String = "WELCOME"
    Private Const StepFilms As String = "FILMS"
    Private Const StepTimes As String = "TIMES"
    Private Const StepSeats As String = "SEATS"

    'which step is on the screen at the moment
    Private currentStep As String = StepWelcome

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
    Private filmsOnToday As DataTable

    'every seat in the screen the picked showing is in, with what sort of seat it is and what that
    'does to the price. read once when the map is drawn so touching a seat does not need another
    'look at the database just to find out what that one costs
    Private currentSeats As DataTable

    'the seats picked so far. the buttons change colour to match what is in here, the colour is
    'never what decides anything, the same way round as the staff booking form does it
    Private pickedSeats As DataTable

    Private Sub frmKiosk_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CommonFormStartup(Me)
        LayoutKiosk()
        ShowStep(StepWelcome)
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

        CentreWelcome()
        LayoutFilmsStep()
        LayoutTimesStep()
        LayoutSeatsStep()
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
        pnlFooter.Width = Me.ClientSize.Width
        pnlFooter.Top = Me.ClientSize.Height - pnlFooter.Height

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
        lblNoFilms.Top = lblFilmsHeading.Bottom + 24

        pnlFilmList.Top = lblFilmsHeading.Bottom + 20
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

        btnStart.Left = (pnlWelcome.Width - btnStart.Width) \ 2
        btnStart.Top = lblWelcomeSub.Bottom + 60
    End Sub

    Private Sub frmKiosk_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        LayoutKiosk()
    End Sub

    'puts one step on the screen and hides the rest of them. everything about what the customer
    'can see and do belongs in here, so there is one place that decides what a step looks like
    'rather than every button doing its own showing and hiding
    Private Sub ShowStep(stepName As String)
        currentStep = stepName

        pnlWelcome.Visible = (stepName = StepWelcome)
        pnlFilms.Visible = (stepName = StepFilms)
        pnlTimes.Visible = (stepName = StepTimes)
        pnlSeats.Visible = (stepName = StepSeats)

        'the wording under the Filmtopia name says where the customer is up to
        If stepName = StepWelcome Then
            lblStep.Text = "Self service"
        ElseIf stepName = StepFilms Then
            lblStep.Text = "Step 1 of 4  -  choose a film"
        ElseIf stepName = StepTimes Then
            lblStep.Text = "Step 2 of 4  -  choose a showing"
        ElseIf stepName = StepSeats Then
            lblStep.Text = "Step 3 of 4  -  choose your seats"
        End If

        'there is nothing to go back to from the welcome screen
        btnBack.Visible = (stepName <> StepWelcome)

        'the first two steps are answered by touching a tile, so a continue button on them would
        'only be something else to press. it appears when there is a running total to carry on with
        btnNext.Visible = (stepName = StepSeats)
        lblRunningTotal.Visible = (stepName = StepSeats)

        LayoutKiosk()
    End Sub

    'the films that still have a showing left today. a film with nothing but screenings that have
    'already started is no use to somebody stood at the machine, so those are left out.
    'ScreeningTime is text in HH:MM with the zero always on the front, so comparing it against the
    'time now as text puts them in the right order without having to turn every row into a number
    Private Sub LoadFilmsForToday()
        filmsOnToday = New DataTable
        Dim dt As DataTable = filmsOnToday

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            'DISTINCT because a film with three showings today should still only be one tile
            SQLCmd.CommandText = "SELECT DISTINCT tblFilm.FilmID, FilmTitle, FilmAgeRating, FilmDuration " &
                                 "FROM tblFilm INNER JOIN tblScreening ON tblFilm.FilmID = tblScreening.FilmID " &
                                 "WHERE ScreeningDate = @Today AND ScreeningTime >= @TimeNow " &
                                 "ORDER BY FilmTitle"
            SQLCmd.Parameters.AddWithValue("@Today", Date.Today)
            SQLCmd.Parameters.AddWithValue("@TimeNow", Format(Now, "HH:mm"))
            Dim da As New OleDbDataAdapter(SQLCmd)
            da.Fill(dt)
            cn.Close()
        End If

        BuildFilmTiles(dt)

        'if there is genuinely nothing left on, say so rather than leaving a blank screen that
        'looks like the machine has gone wrong
        lblNoFilms.Visible = (dt.Rows.Count = 0)
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
        Dim ctrl As Control = CType(sender, Control)
        currentFilmID = CLng(ctrl.Tag)
        currentFilmTitle = TitleOfFilm(currentFilmID)

        LoadShowingsForFilm()
        ShowStep(StepTimes)
    End Sub

    'the title of a film that has already been read onto the first step
    Private Function TitleOfFilm(filmID As Long) As String
        Dim rows() As DataRow = filmsOnToday.Select("FilmID = " & filmID)

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
                                 "WHERE FilmID = @FilmID AND ScreeningDate = @Today AND ScreeningTime >= @TimeNow " &
                                 "ORDER BY ScreeningTime"
            SQLCmd.Parameters.AddWithValue("@FilmID", CInt(currentFilmID))
            SQLCmd.Parameters.AddWithValue("@Today", Date.Today)
            SQLCmd.Parameters.AddWithValue("@TimeNow", Format(Now, "HH:mm"))
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

            Dim b As New Button
            b.Tag = seatID
            b.Text = seatRow & seatNumber
            b.Size = New Size(SeatWidth, SeatHeight)
            b.Font = New Font("Segoe UI", 10)
            b.FlatStyle = FlatStyle.Flat
            b.FlatAppearance.BorderSize = 0

            'the row letter A,B,C says how far down and the seat number says how far across
            Dim rowIndex As Integer = Asc(seatRow) - 65
            b.Location = New Point((seatNumber - 1) * (SeatWidth + SeatGap),
                                   rowIndex * (SeatHeight + SeatGap))

            'a seat that costs more than a standard one gets a border round it so the difference
            'can be seen. the background is left to say whether it is free, picked or gone, so the
            'two things are not fighting over the same colour
            If multiplier <> 1 Then
                b.FlatAppearance.BorderSize = 3
                b.FlatAppearance.BorderColor = AccentFore
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
        Dim mapWidth As Integer = (seatsAcross * (SeatWidth + SeatGap)) - SeatGap

        'the space left over once the list down the left has had its share
        Dim spaceLeft As Integer = pnlSeats.Width - LeftColumnWidth - 30

        'the map never gets wider than the room it has, otherwise a small screen would leave it
        'hanging off the side. if it does not fit the panel scrolls instead
        If mapWidth > spaceLeft Then
            mapWidth = spaceLeft
        End If

        Dim mapLeft As Integer = LeftColumnWidth + ((spaceLeft - mapWidth) \ 2)

        lblScreen.Left = mapLeft
        lblScreen.Width = mapWidth
        lblScreen.Top = lblSeatsShowing.Bottom + 24

        pnlSeatMap.Left = mapLeft
        pnlSeatMap.Width = mapWidth
        pnlSeatMap.Top = lblScreen.Bottom + 16
        'the key sits under the map so the map stops short of the bottom to leave room for it
        pnlSeatMap.Height = pnlSeats.Height - pnlSeatMap.Top - 80
    End Sub

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

            pickedSeats.Rows.Add(CInt(seatID), b.Text, MultiplierForSeat(seatID))
            b.BackColor = SeatSelected
        End If

        UpdateSeatSummary()
    End Sub

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

        lblRunningTotal.Text = "Total  " & FormatCurrency(TicketsTotal())
        btnNext.Enabled = (pickedSeats.Rows.Count > 0)
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

        'the list is read again every time rather than once when the form opens, otherwise a
        'machine left on all day would still be offering this morning's showings
        LoadFilmsForToday()
        ShowStep(StepFilms)
    End Sub

    'goes back a step. the welcome screen is the one place it cannot be pressed
    Private Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        If currentStep = StepFilms Then
            ShowStep(StepWelcome)
        ElseIf currentStep = StepTimes Then
            'going back to the list of films reads it again, because a showing could have started
            'while the customer was stood there deciding
            LoadFilmsForToday()
            ShowStep(StepFilms)
        ElseIf currentStep = StepSeats Then
            'the seats picked are thrown away on the way back, and the list of showings is read
            'again so how many are left is right rather than however full it was a minute ago
            LoadShowingsForFilm()
            ShowStep(StepTimes)
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
