Imports System.Data.OleDb

Public Class frmBookings
    Private Const SeatSize As Integer = 40
    Private Const SeatGap As Integer = 4

    Private selectedScreeningID As Long = 0
    Private selectedScreenID As Long = 0
    Private selectedTicketPrice As Decimal = 0
    Private seatIDs(-1) As Long
    Private seatNames(-1) As String
    Private seatChosen(-1) As Boolean

    Private Sub frmBookings_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadScreenings()
    End Sub

    Private Sub LoadScreenings()
        Dim SQLCmd As New OleDbCommand("SELECT ScreeningID, tblScreening.ScreenID, FilmDuration, FilmTitle, ScreenName, ScreeningDate, ScreeningTime, TicketPrice " &
                                       "FROM (tblScreening INNER JOIN tblFilm ON tblScreening.FilmID = tblFilm.FilmID) " &
                                       "INNER JOIN tblScreen ON tblScreening.ScreenID = tblScreen.ScreenID " &
                                       "WHERE ScreeningDate >= @Today " &
                                       "ORDER BY ScreeningDate, ScreeningTime, ScreenName")
        SQLCmd.Parameters.AddWithValue("@Today", Date.Today)
        Dim dt As New DataTable
        If FillTable(SQLCmd, dt) Then
            Dim timeNow As Integer = TimeToMinutes(Date.Now.ToString("HH:mm"))
            For i As Integer = dt.Rows.Count - 1 To 0 Step -1
                Dim screening As DataRow = dt.Rows(i)
                Dim filmEndTime As Integer = TimeToMinutes(screening("ScreeningTime")) + screening("FilmDuration")
                If screening("ScreeningDate") = Date.Today And filmEndTime <= timeNow Then
                    dt.Rows.Remove(screening)
                End If
            Next

            dgvScreenings.DataSource = dt
            dgvScreenings.Columns("ScreeningID").Visible = False
            dgvScreenings.Columns("ScreenID").Visible = False
            dgvScreenings.Columns("FilmDuration").Visible = False
            dgvScreenings.Columns("FilmTitle").HeaderText = "Film"
            dgvScreenings.Columns("ScreenName").HeaderText = "Screen"
            dgvScreenings.Columns("ScreeningDate").HeaderText = "Date"
            dgvScreenings.Columns("ScreeningTime").HeaderText = "Time"
            dgvScreenings.Columns("TicketPrice").HeaderText = "Ticket Price"
            dgvScreenings.Columns("FilmTitle").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            dgvScreenings.Columns("ScreeningDate").DefaultCellStyle.Format = "dd/MM/yyyy"
            dgvScreenings.Columns("TicketPrice").DefaultCellStyle.Format = "C2"
        End If
    End Sub

    Private Sub frmBookings_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        dgvScreenings.ClearSelection()
    End Sub

    Private Sub dgvScreenings_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs) Handles dgvScreenings.DataBindingComplete
        dgvScreenings.ClearSelection()
    End Sub

    Private Sub dgvScreenings_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvScreenings.CellClick
        If e.RowIndex >= 0 Then
            Dim row As DataGridViewRow = dgvScreenings.Rows(e.RowIndex)
            selectedScreeningID = row.Cells("ScreeningID").Value
            selectedScreenID = row.Cells("ScreenID").Value
            selectedTicketPrice = row.Cells("TicketPrice").Value
            lblSeatMap.Text = row.Cells("FilmTitle").Value & " in " & row.Cells("ScreenName").Value & " on " & CDate(row.Cells("ScreeningDate").Value).ToString("dd/MM/yyyy") & " at " & row.Cells("ScreeningTime").Value
            LoadSeats()
        End If
    End Sub

    Private Sub LoadSeats()
        Dim dt As DataTable = GetSeats(selectedScreenID)
        pnlSeats.Controls.Clear()
        ReDim seatIDs(dt.Rows.Count - 1)
        ReDim seatNames(dt.Rows.Count - 1)
        ReDim seatChosen(dt.Rows.Count - 1)

        For i As Integer = 0 To dt.Rows.Count - 1
            Dim seatRow As String = dt.Rows(i)("SeatRow")
            Dim seatNumber As Integer = dt.Rows(i)("SeatNumber")
            seatIDs(i) = dt.Rows(i)("SeatID")
            seatNames(i) = seatRow & seatNumber

            Dim seatButton As New Button
            seatButton.Text = seatNames(i)
            seatButton.Size = New Size(SeatSize, SeatSize)
            seatButton.Left = SeatGap + (seatNumber - 1) * (SeatSize + SeatGap)
            seatButton.Top = SeatGap + (Asc(seatRow) - Asc("A")) * (SeatSize + SeatGap)
            seatButton.BackColor = FreeSeatColour
            seatButton.FlatStyle = FlatStyle.Flat
            seatButton.Font = New Font(seatButton.Font.FontFamily, 7)
            seatButton.Tag = i
            AddHandler seatButton.Click, AddressOf SeatClicked
            pnlSeats.Controls.Add(seatButton)
        Next
        UpdateSummary()
    End Sub

    Private Sub SeatClicked(sender As Object, e As EventArgs)
        Dim seatButton As Button = sender
        Dim seatIndex As Integer = seatButton.Tag

        If seatChosen(seatIndex) Then
            seatChosen(seatIndex) = False
            seatButton.BackColor = FreeSeatColour
        Else
            seatChosen(seatIndex) = True
            seatButton.BackColor = ChosenSeatColour
        End If
        UpdateSummary()
    End Sub

    Private Sub UpdateSummary()
        Dim seatList As String = ""
        Dim tickets As Integer = 0
        Dim total As Decimal = 0

        For i As Integer = 0 To seatIDs.Length - 1
            If seatChosen(i) Then
                If seatList <> "" Then
                    seatList = seatList & ", "
                End If
                seatList = seatList & seatNames(i)
                tickets = tickets + 1
                total = total + selectedTicketPrice
            End If
        Next

        If seatList = "" Then
            seatList = "None"
        End If
        lblChosenSeats.Text = seatList
        lblTickets.Text = tickets
        lblTotal.Text = total.ToString("C2")
    End Sub
End Class
