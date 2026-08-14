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

    'the steps a customer goes through. they are only ever compared as text so they are kept as
    'constants the same way the log severities are, which means a typo is a compile error instead
    'of a step that quietly never shows up
    Private Const StepWelcome As String = "WELCOME"
    Private Const StepFilms As String = "FILMS"

    'which step is on the screen at the moment
    Private currentStep As String = StepWelcome

    'the film the customer has picked, 0 means they have not picked one yet
    Private currentFilmID As Long = 0

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
        pnlHeader.Width = Me.ClientSize.Width
        btnExitKiosk.Left = pnlHeader.Width - btnExitKiosk.Width - 30

        pnlFooter.Width = Me.ClientSize.Width
        pnlFooter.Top = Me.ClientSize.Height - pnlFooter.Height

        'every step panel gets the same rectangle, since only one of them is ever on show
        Dim contentTop As Integer = pnlHeader.Height
        Dim contentHeight As Integer = pnlFooter.Top - contentTop

        SizeStepPanel(pnlWelcome, contentTop, contentHeight)
        SizeStepPanel(pnlFilms, contentTop, contentHeight)

        'the list of films fills its step, leaving room for the heading above it
        pnlFilmList.Width = pnlFilms.Width - 40
        pnlFilmList.Height = pnlFilms.Height - pnlFilmList.Top - 20
        ArrangeFilmTiles()

        CentreWelcome()

        lblVersion.Top = pnlFooter.Height - lblVersion.Height - 10
        lblVersion.Left = pnlFooter.Width - lblVersion.Width - 20
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

        'the wording under the Filmtopia name says where the customer is up to
        If stepName = StepWelcome Then
            lblStep.Text = "Self service"
        ElseIf stepName = StepFilms Then
            lblStep.Text = "Step 1 of 4  -  choose a film"
        End If

        'there is nothing to go back to from the welcome screen
        btnBack.Visible = (stepName <> StepWelcome)
    End Sub

    'the films that still have a showing left today. a film with nothing but screenings that have
    'already started is no use to somebody stood at the machine, so those are left out.
    'ScreeningTime is text in HH:MM with the zero always on the front, so comparing it against the
    'time now as text puts them in the right order without having to turn every row into a number
    Private Sub LoadFilmsForToday()
        Dim dt As New DataTable

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

    'works out where each tile goes. how many fit on a row depends on how wide the screen is, so
    'this is worked out again whenever the window changes size rather than being fixed
    Private Sub ArrangeFilmTiles()
        Dim perRow As Integer = (pnlFilmList.Width - TileGap) \ (TileWidth + TileGap)

        'a very narrow screen still has to show one tile per row rather than none at all
        If perRow < 1 Then
            perRow = 1
        End If

        Dim i As Integer
        For i = 0 To pnlFilmList.Controls.Count - 1
            Dim column As Integer = i Mod perRow
            Dim row As Integer = i \ perRow

            pnlFilmList.Controls(i).Left = column * (TileWidth + TileGap)
            pnlFilmList.Controls(i).Top = row * (TileHeight + TileGap)
        Next
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

        WriteLog("KIOSK", "Customer picked film " & currentFilmID)
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
