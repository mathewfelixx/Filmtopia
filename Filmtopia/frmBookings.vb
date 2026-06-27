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
    Private seatBooked(-1) As Boolean

    Private orderFoodIDs(-1) As Long
    Private orderNames(-1) As String
    Private orderPrices(-1) As Decimal
    Private orderQuantities(-1) As Integer

    Private Sub frmBookings_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadScreenings()
        LoadFoodMenu()
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

    Private Sub LoadFoodMenu()
        Dim SQLCmd As New OleDbCommand("SELECT FoodItemID, FoodCategory, FoodName, FoodPrice FROM tblFoodItem ORDER BY FoodCategory, FoodName")
        Dim dt As New DataTable
        If FillTable(SQLCmd, dt) Then
            dgvFood.DataSource = dt
            dgvFood.Columns("FoodItemID").Visible = False
            dgvFood.Columns("FoodCategory").HeaderText = "Category"
            dgvFood.Columns("FoodName").HeaderText = "Item"
            dgvFood.Columns("FoodPrice").HeaderText = "Price"
            dgvFood.Columns("FoodName").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            dgvFood.Columns("FoodPrice").DefaultCellStyle.Format = "C2"
        End If
    End Sub

    Private Sub frmBookings_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        dgvScreenings.ClearSelection()
    End Sub

    Private Sub dgvScreenings_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs) Handles dgvScreenings.DataBindingComplete
        dgvScreenings.ClearSelection()
    End Sub

    Private Sub dgvFood_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs) Handles dgvFood.DataBindingComplete
        dgvFood.ClearSelection()
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
        Dim dtBooked As DataTable = GetBookedSeats(selectedScreeningID)
        pnlSeats.Controls.Clear()
        ReDim seatIDs(dt.Rows.Count - 1)
        ReDim seatNames(dt.Rows.Count - 1)
        ReDim seatChosen(dt.Rows.Count - 1)
        ReDim seatBooked(dt.Rows.Count - 1)

        For i As Integer = 0 To dt.Rows.Count - 1
            Dim seatRow As String = dt.Rows(i)("SeatRow")
            Dim seatNumber As Integer = dt.Rows(i)("SeatNumber")
            seatIDs(i) = dt.Rows(i)("SeatID")
            seatNames(i) = seatRow & seatNumber
            seatBooked(i) = IsBooked(seatIDs(i), dtBooked)

            Dim seatButton As New Button
            seatButton.Text = seatNames(i)
            seatButton.Size = New Size(SeatSize, SeatSize)
            seatButton.Left = SeatGap + (seatNumber - 1) * (SeatSize + SeatGap)
            seatButton.Top = SeatGap + (Asc(seatRow) - Asc("A")) * (SeatSize + SeatGap)
            If seatBooked(i) Then
                seatButton.BackColor = BookedSeatColour
            Else
                seatButton.BackColor = FreeSeatColour
            End If
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

        If seatBooked(seatIndex) Then
            MessageBox.Show("Seat " & seatNames(seatIndex) & " is already booked for this screening, so it cannot be chosen.", "Seat Taken", MessageBoxButtons.OK, MessageBoxIcon.Information)
        ElseIf seatChosen(seatIndex) Then
            seatChosen(seatIndex) = False
            seatButton.BackColor = FreeSeatColour
        Else
            seatChosen(seatIndex) = True
            seatButton.BackColor = ChosenSeatColour
        End If
        UpdateSummary()
    End Sub

    Private Sub btnAddFood_Click(sender As Object, e As EventArgs) Handles btnAddFood.Click
        If dgvFood.SelectedRows.Count = 0 Then
            MessageBox.Show("Please pick an item from the menu first.", "No Item Picked", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            Dim item As DataGridViewRow = dgvFood.SelectedRows(0)
            Dim foodItemID As Long = item.Cells("FoodItemID").Value

            Dim alreadyInOrder As Boolean = False
            For i As Integer = 0 To orderFoodIDs.Length - 1
                If orderFoodIDs(i) = foodItemID Then
                    orderQuantities(i) = orderQuantities(i) + 1
                    alreadyInOrder = True
                End If
            Next

            If Not alreadyInOrder Then
                Dim newIndex As Integer = orderFoodIDs.Length
                ReDim Preserve orderFoodIDs(newIndex)
                ReDim Preserve orderNames(newIndex)
                ReDim Preserve orderPrices(newIndex)
                ReDim Preserve orderQuantities(newIndex)
                orderFoodIDs(newIndex) = foodItemID
                orderNames(newIndex) = item.Cells("FoodName").Value
                orderPrices(newIndex) = item.Cells("FoodPrice").Value
                orderQuantities(newIndex) = 1
            End If

            UpdateSummary()
        End If
    End Sub

    Private Sub btnRemoveFood_Click(sender As Object, e As EventArgs) Handles btnRemoveFood.Click
        Dim lineIndex As Integer = lstOrder.SelectedIndex

        If lineIndex = -1 Then
            MessageBox.Show("Please pick an item in the order first.", "No Item Picked", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            orderQuantities(lineIndex) = orderQuantities(lineIndex) - 1
            If orderQuantities(lineIndex) = 0 Then
                RemoveOrderLine(lineIndex)
            End If
            UpdateSummary()
        End If
    End Sub

    Private Sub RemoveOrderLine(lineIndex As Integer)
        Dim lastIndex As Integer = orderFoodIDs.Length - 1
        For i As Integer = lineIndex To lastIndex - 1
            orderFoodIDs(i) = orderFoodIDs(i + 1)
            orderNames(i) = orderNames(i + 1)
            orderPrices(i) = orderPrices(i + 1)
            orderQuantities(i) = orderQuantities(i + 1)
        Next
        ReDim Preserve orderFoodIDs(lastIndex - 1)
        ReDim Preserve orderNames(lastIndex - 1)
        ReDim Preserve orderPrices(lastIndex - 1)
        ReDim Preserve orderQuantities(lastIndex - 1)
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

        lstOrder.Items.Clear()
        For i As Integer = 0 To orderFoodIDs.Length - 1
            Dim lineTotal As Decimal = orderPrices(i) * orderQuantities(i)
            lstOrder.Items.Add(orderQuantities(i) & " x " & orderNames(i) & "   " & lineTotal.ToString("C2"))
            total = total + lineTotal
        Next

        If seatList = "" Then
            seatList = "None"
        End If
        lblChosenSeats.Text = seatList
        lblTickets.Text = tickets
        lblTotal.Text = total.ToString("C2")
    End Sub

    Private Sub btnClearBooking_Click(sender As Object, e As EventArgs) Handles btnClearBooking.Click
        ClearBooking()
    End Sub

    Private Sub ClearBooking()
        txtCustomerName.Text = ""
        ReDim orderFoodIDs(-1)
        ReDim orderNames(-1)
        ReDim orderPrices(-1)
        ReDim orderQuantities(-1)
        If selectedScreeningID <> 0 Then
            LoadSeats()
        End If
        UpdateSummary()
        txtCustomerName.Focus()
    End Sub

    Private Sub btnBook_Click(sender As Object, e As EventArgs) Handles btnBook.Click
        Dim customerName As String = txtCustomerName.Text.Trim()
        Dim tickets As Integer = 0
        For i As Integer = 0 To seatChosen.Length - 1
            If seatChosen(i) Then
                tickets = tickets + 1
            End If
        Next

        If selectedScreeningID = 0 Then
            MessageBox.Show("Please pick a screening first.", "Missing Details", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        ElseIf customerName = "" Then
            MessageBox.Show("Please enter the customer's name.", "Missing Details", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtCustomerName.Focus()
        ElseIf tickets = 0 Then
            MessageBox.Show("Please choose at least one seat.", "Missing Details", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Else
            Dim details As String = tickets & " ticket(s) (" & lblChosenSeats.Text & ") for " & lblSeatMap.Text
            If SeatsAreFree() Then
                Dim bookingID As Long = SaveBooking(customerName)
                If bookingID <> 0 Then
                    WriteLog(LogBooking, "Booking number " & bookingID & " for " & customerName & ": " & details & ", total " & lblTotal.Text)
                    MessageBox.Show("Booking number " & bookingID & " saved for " & customerName & ": " & details & ", total " & lblTotal.Text & ".", "Booking Saved", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    ClearBooking()
                End If
            Else
                MessageBox.Show("Sorry, one or more of those seats were just booked by someone else, so nothing was booked. The seat map has been updated to show them as booked; please choose again.", "Seat Taken", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                LoadSeats()
            End If
        End If
    End Sub

    Private Function SeatsAreFree() As Boolean
        Dim free As Boolean = True
        Dim dtBooked As DataTable = GetBookedSeats(selectedScreeningID)
        For i As Integer = 0 To seatIDs.Length - 1
            If seatChosen(i) And IsBooked(seatIDs(i), dtBooked) Then
                free = False
            End If
        Next
        Return free
    End Function

    Private Function SaveBooking(customerName As String) As Long
        Dim bookingID As Long = 0

        If DbConnect() Then
            Dim trans As OleDbTransaction = cn.BeginTransaction()
            Try
                Dim SQLCmd As New OleDbCommand
                SQLCmd.Connection = cn
                SQLCmd.Transaction = trans

                SQLCmd.CommandText = "INSERT INTO tblBooking (ScreeningID, CustomerName, BookingDateTime) VALUES (@ScreeningID, @CustomerName, Now())"
                SQLCmd.Parameters.AddWithValue("@ScreeningID", selectedScreeningID)
                SQLCmd.Parameters.AddWithValue("@CustomerName", customerName)
                SQLCmd.ExecuteNonQuery()

                SQLCmd.Parameters.Clear()
                SQLCmd.CommandText = "SELECT @@IDENTITY"
                bookingID = SQLCmd.ExecuteScalar()

                SQLCmd.CommandText = "INSERT INTO tblBookingSeat (BookingID, SeatID, SeatPricePaid) VALUES (@BookingID, @SeatID, @SeatPricePaid)"
                For i As Integer = 0 To seatIDs.Length - 1
                    If seatChosen(i) Then
                        SQLCmd.Parameters.Clear()
                        SQLCmd.Parameters.AddWithValue("@BookingID", bookingID)
                        SQLCmd.Parameters.AddWithValue("@SeatID", seatIDs(i))
                        SQLCmd.Parameters.AddWithValue("@SeatPricePaid", selectedTicketPrice)
                        SQLCmd.ExecuteNonQuery()
                    End If
                Next

                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                bookingID = 0
                MessageBox.Show("The booking could not be saved, so nothing was booked. " & ex.Message, "Booking Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
            cn.Close()
        End If

        Return bookingID
    End Function
End Class
