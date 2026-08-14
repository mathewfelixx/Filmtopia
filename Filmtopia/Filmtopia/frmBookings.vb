Imports System.Data.OleDb

Public Class frmBookings

    'the screening currently picked in the combo, 0 means none
    Private currentScreeningID As Long = 0
    'the screen that screening runs in, used to load the right seats
    Private currentScreenID As Long = 0
    'ticket price for the picked screening, used to work out the total
    Private currentTicketPrice As Double = 0
    'the booking id of the booking just created, used to open food ordering
    Private lastBookingID As Long = 0

    'the food the customer has asked for so far. it is only held here in memory while the sale
    'is being built up, and it is not written to the database until COMPLETE SALE is pressed
    Private pendingFood As DataTable

    'the seats picked so far, kept the same way the food is. before this the form worked out which
    'seats were picked by looking at what colour each button had gone, which meant the sale
    'depended on the theme. if a theme ever gave two seat states the same colour the wrong seats
    'would have been sold and nothing would have said so. the colour is now only how a seat is
    'drawn, and this table is what is actually being bought
    Private pendingSeats As DataTable

    'every seat in the screen being shown, with what sort it is and its price multiplier. it is
    'read once when the map is drawn so that clicking a seat does not need another look at the
    'database just to find out what that seat costs
    Private currentSeats As DataTable

    'shows what a seat is and what it costs when the mouse rests on it
    Private seatTips As New ToolTip

    'the three seat colours, these get set from the theme so they work in dark mode too
    Private availableColour As Color
    Private selectedColour As Color
    Private takenColour As Color

    'takes the seat colours from whichever theme is on and puts them on the little key labels
    Private Sub ApplySeatColours()
        availableColour = SeatAvailable
        selectedColour = SeatSelected
        takenColour = SeatTaken

        lblSwatchAvailable.BackColor = availableColour
        lblSwatchSelected.BackColor = selectedColour
        lblSwatchTaken.BackColor = takenColour
    End Sub

    Private Sub frmBookings_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CommonFormStartup(Me)
        ApplySeatColours()
        SetUpPendingSeats()
        SetUpPendingFood()
        LoadFoodItems()
        LoadScreenings()
        LoadCustomers()
        WriteLog("BOOKING", "Bookings form opened")
    End Sub

    'picks a screening in the combo from outside the form, used when a screening is double clicked
    'on the main menu, setting the value fires the combo's changed event which builds the seat map
    Public Sub SelectScreening(screeningID As Long)
        cboScreening.SelectedValue = screeningID
    End Sub

    'fills the screening combo with each screening and its film, date and time
    Private Sub LoadScreenings()
        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            'join screening to film so the combo can show the film title alongside the date and time
            SQLCmd.CommandText = "SELECT ScreeningID, FilmTitle & ' - ' & ScreeningDate & ' ' & ScreeningTime AS Info " &
                                 "FROM tblScreening INNER JOIN tblFilm ON tblScreening.FilmID = tblFilm.FilmID"
            Dim da As New OleDbDataAdapter(SQLCmd)
            Dim dt As New DataTable
            da.Fill(dt)
            cboScreening.DataSource = dt
            cboScreening.DisplayMember = "Info"
            cboScreening.ValueMember = "ScreeningID"
            cboScreening.SelectedIndex = -1
            cn.Close()
        End If
    End Sub

    'fills the customer combo with each customers full name
    Private Sub LoadCustomers()
        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT CustomerID, CustomerForename & ' ' & CustomerSurname AS CustomerName " &
                                 "FROM tblCustomer"
            Dim da As New OleDbDataAdapter(SQLCmd)
            Dim dt As New DataTable
            da.Fill(dt)
            cboCustomer.DataSource = dt
            cboCustomer.DisplayMember = "CustomerName"
            cboCustomer.ValueMember = "CustomerID"
            cboCustomer.SelectedIndex = -1
            cn.Close()
        End If
    End Sub

    'when a screening is picked, get its details and draw the seat map
    Private Sub cboScreening_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboScreening.SelectedIndexChanged
        If cboScreening.SelectedIndex = -1 Then
            Exit Sub
        End If

        'while the combo is still binding the value isnt a number yet, so skip
        If Not IsNumeric(cboScreening.SelectedValue) Then
            Exit Sub
        End If

        currentScreeningID = CLng(cboScreening.SelectedValue)
        LoadScreeningDetails()
        BuildSeatMap()
    End Sub

    'when a customer is picked, show their existing bookings in the small grid
    Private Sub cboCustomer_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboCustomer.SelectedIndexChanged
        'a different customer means the booking that was picked before is no longer relevant
        lastBookingID = 0
        btnOrderFood.Enabled = False
        lblCustomerBookings.Text = "Pick a booking to add food to it"

        If cboCustomer.SelectedIndex = -1 Then
            dgvCustomerBookings.DataSource = Nothing
            Exit Sub
        End If

        If Not IsNumeric(cboCustomer.SelectedValue) Then
            Exit Sub
        End If

        LoadCustomerBookings(CLng(cboCustomer.SelectedValue))
    End Sub

    'loads every booking made by this customer into the small grid
    Private Sub LoadCustomerBookings(customerID As Long)
        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            'join booking to screening, then to film, so we can show the film title and date.
            'cancelled bookings are left out because this list is what food gets added to, and
            'adding food to a sale that has already been refunded makes no sense
            SQLCmd.CommandText = "SELECT tblBooking.BookingID, FilmTitle & ' (' & ScreeningDate & ')' AS Info " &
                                 "FROM (tblBooking INNER JOIN tblScreening ON tblBooking.ScreeningID = tblScreening.ScreeningID) " &
                                 "INNER JOIN tblFilm ON tblScreening.FilmID = tblFilm.FilmID " &
                                 "WHERE tblBooking.CustomerID = @CustomerID " &
                                 "AND tblBooking.BookingStatus <> @Cancelled"
            SQLCmd.Parameters.AddWithValue("@CustomerID", CInt(customerID))
            SQLCmd.Parameters.AddWithValue("@Cancelled", BookingCancelled)
            Dim da As New OleDbDataAdapter(SQLCmd)
            Dim dt As New DataTable
            da.Fill(dt)
            dgvCustomerBookings.DataSource = dt
            cn.Close()
        End If

        'keep the small grid tidy, one line per booking
        If dgvCustomerBookings.Columns.Count > 0 Then
            dgvCustomerBookings.Columns("BookingID").Width = 40
            dgvCustomerBookings.Columns("Info").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        End If
    End Sub

    'when a booking is picked from the customer's list, allow food to be ordered for it
    Private Sub dgvCustomerBookings_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvCustomerBookings.CellClick
        If e.RowIndex < 0 Then Exit Sub

        Dim row As DataGridViewRow = dgvCustomerBookings.Rows(e.RowIndex)
        lastBookingID = CLng(row.Cells("BookingID").Value)
        btnOrderFood.Enabled = True
        lblCustomerBookings.Text = "Food will be added to booking " & lastBookingID
    End Sub

    'when walk-in is ticked, the customer combo isnt needed so grey it out
    Private Sub chkWalkIn_CheckedChanged(sender As Object, e As EventArgs) Handles chkWalkIn.CheckedChanged
        cboCustomer.Enabled = Not chkWalkIn.Checked

        If chkWalkIn.Checked Then
            cboCustomer.SelectedIndex = -1
            dgvCustomerBookings.DataSource = Nothing
            lastBookingID = 0
            btnOrderFood.Enabled = False
        End If
    End Sub

    'makes a quick customer record for someone who walks in without giving their details
    Private Function CreateWalkInCustomer() As Long
        Dim newCustomerID As Long = 0

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "INSERT INTO tblCustomer (CustomerForename, CustomerSurname, CustomerEmail, CustomerPhone) " &
                                 "VALUES (@CustomerForename, @CustomerSurname, @CustomerEmail, @CustomerPhone)"
            SQLCmd.Parameters.AddWithValue("@CustomerForename", "Walk-in")
            SQLCmd.Parameters.AddWithValue("@CustomerSurname", "Customer")
            SQLCmd.Parameters.AddWithValue("@CustomerEmail", "")
            SQLCmd.Parameters.AddWithValue("@CustomerPhone", "")
            SQLCmd.ExecuteNonQuery()

            SQLCmd.CommandText = "SELECT @@IDENTITY"
            SQLCmd.Parameters.Clear()
            newCustomerID = CLng(SQLCmd.ExecuteScalar())
            cn.Close()
        End If

        Return newCustomerID
    End Function

    'gets the screen and ticket price for the picked screening
    Private Sub LoadScreeningDetails()
        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT ScreenID, TicketPrice " &
                                 "FROM tblScreening " &
                                 "WHERE ScreeningID = @ScreeningID"
            SQLCmd.Parameters.AddWithValue("@ScreeningID", CInt(currentScreeningID))
            Dim rs As OleDbDataReader = SQLCmd.ExecuteReader()
            If rs.Read() Then
                currentScreenID = CLng(rs("ScreenID"))
                currentTicketPrice = CDbl(rs("TicketPrice"))
            End If
            rs.Close()
            cn.Close()
        End If
    End Sub

    'draws a button for every seat in the screens layout and greys out the taken ones
    'makes the empty table that holds the seats picked for the sale being built up
    Private Sub SetUpPendingSeats()
        pendingSeats = New DataTable
        pendingSeats.Columns.Add("SeatID", GetType(Integer))
        'the multiplier is kept with the seat so the running total can be worked out without going
        'back to the database every time a seat is clicked
        pendingSeats.Columns.Add("Multiplier", GetType(Double))
    End Sub

    'says whether a seat has been picked for this sale
    Private Function IsSeatSelected(seatID As Long) As Boolean
        Return pendingSeats.Select("SeatID = " & seatID).Length > 0
    End Function

    'makes the empty table that holds the food for the sale being built up
    Private Sub SetUpPendingFood()
        pendingFood = New DataTable
        pendingFood.Columns.Add("FoodItemID", GetType(Integer))
        pendingFood.Columns.Add("Item", GetType(String))
        pendingFood.Columns.Add("Price", GetType(Double))
        pendingFood.Columns.Add("Quantity", GetType(Integer))
        pendingFood.Columns.Add("Subtotal", GetType(Double))

        dgvPendingFood.DataSource = pendingFood
        TidyFoodGrid()
    End Sub

    'fills the food and drink combo
    Private Sub LoadFoodItems()
        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT FoodItemID, FoodItemName, FoodItemPrice " &
                                 "FROM tblFoodItem ORDER BY FoodItemName"
            Dim da As New OleDbDataAdapter(SQLCmd)
            Dim dt As New DataTable
            da.Fill(dt)
            cboFoodItem.DataSource = dt
            cboFoodItem.DisplayMember = "FoodItemName"
            cboFoodItem.ValueMember = "FoodItemID"
            cboFoodItem.SelectedIndex = -1
            cn.Close()
        End If
    End Sub

    Private Sub TidyFoodGrid()
        If dgvPendingFood.Columns.Count = 0 Then Exit Sub

        dgvPendingFood.Columns("FoodItemID").Visible = False
        dgvPendingFood.Columns("Item").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        dgvPendingFood.Columns("Price").Width = 90
        dgvPendingFood.Columns("Quantity").HeaderText = "Qty"
        dgvPendingFood.Columns("Quantity").Width = 60
        dgvPendingFood.Columns("Subtotal").Width = 100
        dgvPendingFood.Columns("Price").DefaultCellStyle.Format = "C"
        dgvPendingFood.Columns("Subtotal").DefaultCellStyle.Format = "C"
    End Sub

    'adds the picked item to the order being built up. nothing goes in the database yet
    Private Sub btnAddFood_Click(sender As Object, e As EventArgs) Handles btnAddFood.Click
        If cboFoodItem.SelectedIndex = -1 Then
            MessageBox.Show("Pick a food or drink item first")
            Exit Sub
        End If

        Dim quantity As Integer = CInt(Val(txtQuantity.Text))
        If quantity < 1 Then
            MessageBox.Show("Enter a quantity of 1 or more")
            Exit Sub
        End If

        Dim chosen As DataRowView = CType(cboFoodItem.SelectedItem, DataRowView)
        Dim itemID As Integer = CInt(chosen("FoodItemID"))
        Dim price As Double = CDbl(chosen("FoodItemPrice"))

        'if that item is already on the order just add to how many, rather than a second line
        Dim i As Integer
        For i = 0 To pendingFood.Rows.Count - 1
            If CInt(pendingFood.Rows(i)("FoodItemID")) = itemID Then
                pendingFood.Rows(i)("Quantity") = CInt(pendingFood.Rows(i)("Quantity")) + quantity
                pendingFood.Rows(i)("Subtotal") = CInt(pendingFood.Rows(i)("Quantity")) * price
                UpdateTotal()
                txtQuantity.Text = "1"
                Exit Sub
            End If
        Next

        Dim newRow As DataRow = pendingFood.NewRow()
        newRow("FoodItemID") = itemID
        newRow("Item") = chosen("FoodItemName").ToString()
        newRow("Price") = price
        newRow("Quantity") = quantity
        newRow("Subtotal") = price * quantity
        pendingFood.Rows.Add(newRow)

        TidyFoodGrid()
        UpdateTotal()
        txtQuantity.Text = "1"
    End Sub

    'takes a line back off the order
    Private Sub btnRemoveFood_Click(sender As Object, e As EventArgs) Handles btnRemoveFood.Click
        If dgvPendingFood.CurrentRow Is Nothing Then
            MessageBox.Show("Pick a line in the food list first")
            Exit Sub
        End If

        pendingFood.Rows.RemoveAt(dgvPendingFood.CurrentRow.Index)
        UpdateTotal()
    End Sub

    'adds up everything on the order so far
    Private Function FoodTotal() As Double
        Dim total As Double = 0
        Dim i As Integer

        For i = 0 To pendingFood.Rows.Count - 1
            total = total + CDbl(pendingFood.Rows(i)("Subtotal"))
        Next

        Return total
    End Function

    Private Sub BuildSeatMap()
        'make sure the colours match the theme in case it was changed since the form opened
        ApplySeatColours()
        pnlSeatMap.Controls.Clear()

        'the map is about to be drawn again, so anything picked on the old one is forgotten.
        'this used to happen by itself because the buttons were thrown away and the new ones came
        'back the available colour. now the picked seats are kept in a table they have to be
        'emptied on purpose, otherwise changing screening would carry the old seats across
        pendingSeats.Rows.Clear()

        currentSeats = New DataTable
        Dim dtSeats As DataTable = currentSeats
        Dim dtTaken As New DataTable

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn

            'all the seats that belong to this screen, with what sort of seat each one is and what
            'that does to its price
            SQLCmd.CommandText = "SELECT tblSeat.SeatID, tblSeat.SeatRow, tblSeat.SeatNumber, " &
                                 "tblSeatType.SeatTypeName, tblSeatType.PriceMultiplier " &
                                 "FROM tblSeat INNER JOIN tblSeatType ON tblSeat.SeatTypeID = tblSeatType.SeatTypeID " &
                                 "WHERE tblSeat.ScreenID = @ScreenID " &
                                 "ORDER BY tblSeat.SeatRow, tblSeat.SeatNumber"
            SQLCmd.Parameters.AddWithValue("@ScreenID", CInt(currentScreenID))
            Dim da As New OleDbDataAdapter(SQLCmd)
            da.Fill(dtSeats)

            'the seats already booked for this screening - join bookingseat to booking so we can filter by screening
            SQLCmd.CommandText = "SELECT tblBookingSeat.SeatID " &
                                 "FROM tblBookingSeat INNER JOIN tblBooking ON tblBookingSeat.BookingID = tblBooking.BookingID " &
                                 "WHERE tblBooking.ScreeningID = @ScreeningID"
            SQLCmd.Parameters.Clear()
            SQLCmd.Parameters.AddWithValue("@ScreeningID", CInt(currentScreeningID))
            Dim da2 As New OleDbDataAdapter(SQLCmd)
            da2.Fill(dtTaken)

            cn.Close()
        End If

        'make one button per seat, positioned by its row letter and seat number
        For i As Integer = 0 To dtSeats.Rows.Count - 1
            Dim seatID As Long = CLng(dtSeats.Rows(i)("SeatID"))
            Dim seatRow As String = dtSeats.Rows(i)("SeatRow").ToString()
            Dim seatNumber As Integer = CInt(dtSeats.Rows(i)("SeatNumber"))
            Dim seatType As String = dtSeats.Rows(i)("SeatTypeName").ToString()
            Dim multiplier As Double = CDbl(dtSeats.Rows(i)("PriceMultiplier"))

            Dim b As New Button
            b.Tag = seatID
            b.Text = seatRow & seatNumber
            b.Size = New Size(40, 35)
            b.Font = New Font("Segoe UI", 7)

            'the row letter A,B,C sets how far down, the seat number sets how far across
            Dim rowIndex As Integer = Asc(seatRow) - 65
            b.Location = New Point((seatNumber - 1) * 45 + 10, rowIndex * 45 + 10)

            'if this seat is already taken grey it out, otherwise let it be clicked
            If dtTaken.Select("SeatID = " & seatID).Length > 0 Then
                b.BackColor = takenColour
                b.ForeColor = SeatTakenFore
                b.Enabled = False
            Else
                b.BackColor = availableColour
                b.ForeColor = SeatFore
                AddHandler b.Click, AddressOf Seat_Click
            End If

            pnlSeatMap.Controls.Add(b)
        Next

        UpdateTotal()
    End Sub

    'toggles a seat between selected and available when its clicked. the table is what changes,
    'the colour is just put on afterwards to show what the table now says
    Private Sub Seat_Click(sender As Object, e As EventArgs)
        Dim b As Button = CType(sender, Button)
        Dim seatID As Long = CLng(b.Tag)

        If IsSeatSelected(seatID) Then
            Dim rows() As DataRow = pendingSeats.Select("SeatID = " & seatID)
            pendingSeats.Rows.Remove(rows(0))
            b.BackColor = availableColour
        Else
            pendingSeats.Rows.Add(CInt(seatID), MultiplierForSeat(seatID))
            b.BackColor = selectedColour
        End If

        UpdateTotal()
    End Sub

    'looks up what a seat does to the price, from the seats that were read when the map was drawn
    Private Function MultiplierForSeat(seatID As Long) As Double
        Dim rows() As DataRow = currentSeats.Select("SeatID = " & seatID)

        If rows.Length > 0 Then
            Return CDbl(rows(0)("PriceMultiplier"))
        End If

        'if it cannot be found the seat is charged as a standard one, which is the safe way round
        Return 1
    End Function

    'counts how many seats are picked for this sale
    Private Function CountSelectedSeats() As Integer
        Return pendingSeats.Rows.Count
    End Function

    'adds up what the picked seats come to. they are added one at a time rather than counted and
    'multiplied, because a premium seat is worth more than a standard one
    Private Function TicketsTotal() As Double
        Dim total As Double = 0
        Dim i As Integer

        For i = 0 To pendingSeats.Rows.Count - 1
            total = total + SeatPrice(currentTicketPrice, CDbl(pendingSeats.Rows(i)("Multiplier")))
        Next

        Return total
    End Function

    'shows the running total of selected seats and their cost
    'shows what the sale comes to as it is being built up, tickets and food kept separate so the
    'customer can be told what they are paying for
    Private Sub UpdateTotal()
        Dim seatCount As Integer = CountSelectedSeats()
        Dim ticketsCost As Double = seatCount * currentTicketPrice
        Dim foodCost As Double = FoodTotal()

        'a tab does not line up in a label because the font is not fixed width, so a plain
        'separator is used instead
        lblTickets.Text = "Tickets (" & seatCount & ")  -  " & FormatCurrency(ticketsCost)
        lblFoodTotal.Text = "Food and drink  -  " & FormatCurrency(foodCost)
        lblTotal.Text = "TOTAL  " & FormatCurrency(ticketsCost + foodCost)

        'there is nothing to sell until they have picked either a seat or something to eat
        btnCreateBooking.Enabled = (seatCount > 0 Or pendingFood.Rows.Count > 0)
    End Sub

    'creates a booking from the picked screening, customer and selected seats
    'saves the whole sale in one go. nothing has been written to the database up to this point,
    'so if anything is wrong the user is simply told and the sale stays on screen to be fixed
    Private Sub btnCreateBooking_Click(sender As Object, e As EventArgs) Handles btnCreateBooking.Click
        If currentScreeningID = 0 Then
            MessageBox.Show("Pick a screening first")
            Exit Sub
        End If
        If Not chkWalkIn.Checked And cboCustomer.SelectedIndex = -1 Then
            MessageBox.Show("Pick a customer first, or tick Walk-in")
            Exit Sub
        End If

        Dim seatCount As Integer = CountSelectedSeats()

        'a sale has to be for something, but it can be seats only, food only, or both
        If seatCount = 0 And pendingFood.Rows.Count = 0 Then
            MessageBox.Show("Pick some seats, or add something from the food and drink list")
            Exit Sub
        End If

        'safety check in case a selected seat got booked since the map loaded
        If AnySelectedSeatTaken() Then
            MessageBox.Show("One of your seats has just been booked, please reselect")
            BuildSeatMap()
            Exit Sub
        End If

        Dim ticketsCost As Double = seatCount * currentTicketPrice
        Dim totalCost As Double = ticketsCost + FoodTotal()

        Dim customerID As Long = 0
        If Not chkWalkIn.Checked Then
            customerID = CLng(cboCustomer.SelectedValue)
        End If

        'one transaction writes the customer, the booking, the seats and the food together
        Dim newBookingID As Long = CompleteSale(customerID, chkWalkIn.Checked, currentScreeningID,
                                                GetSelectedSeatIDs(), pendingFood, totalCost)

        If newBookingID = 0 Then
            'nothing was saved, the message has already been shown, so leave the sale on screen
            Exit Sub
        End If

        WriteLog("BOOKING", "Sale " & newBookingID & " completed, " & seatCount & " seat(s) and " &
                            pendingFood.Rows.Count & " food line(s), " & FormatCurrency(totalCost))

        MessageBox.Show("Sale complete." & vbNewLine & vbNewLine &
                        "Booking " & newBookingID & vbNewLine &
                        seatCount & " seat(s) and " & pendingFood.Rows.Count & " food item line(s)" & vbNewLine &
                        "Total " & FormatCurrency(totalCost),
                        "Sale Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)

        'start a fresh sale
        'the sale is finished, so clear it down ready for the next customer. the booking just
        'made is kept selected so food can still be added to it if they change their mind
        lastBookingID = newBookingID
        btnOrderFood.Enabled = True
        BuildSeatMap()
        ClearSaleInputs()
        lblCustomerBookings.Text = "Food can still be added to booking " & newBookingID

        'walk-ins dont have a customer picked in the combo, so theres no list to refresh
        If Not chkWalkIn.Checked Then
            LoadCustomerBookings(CLng(cboCustomer.SelectedValue))
        End If
    End Sub

    'collects the SeatID of every seat the user has picked, ready to be saved
    Private Function GetSelectedSeatIDs() As Long()
        Dim seatIDs(pendingSeats.Rows.Count - 1) As Long
        Dim i As Integer

        For i = 0 To pendingSeats.Rows.Count - 1
            seatIDs(i) = CLng(pendingSeats.Rows(i)("SeatID"))
        Next

        Return seatIDs
    End Function

    'reads back what a booking came to, so the message on screen matches what was saved
    Private Function GetBookingTotal(bookingID As Long) As Double
        Dim total As Double = 0

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT TotalCost FROM tblBooking " &
                                 "WHERE BookingID = @BookingID"
            SQLCmd.Parameters.AddWithValue("@BookingID", CInt(bookingID))
            Dim result = SQLCmd.ExecuteScalar()
            If result IsNot Nothing AndAlso Not IsDBNull(result) Then
                total = CDbl(result)
            End If
            cn.Close()
        End If

        Return total
    End Function

    'opens the food order form for a booking and tidies up afterwards. the food order changes
    'the booking total, so the customer's list of bookings is reloaded once it closes
    Private Sub OpenFoodOrder(bookingID As Long)
        frmFoodOrder.currentBookingID = bookingID
        frmFoodOrder.ShowDialog()

        If Not chkWalkIn.Checked And cboCustomer.SelectedIndex <> -1 Then
            LoadCustomerBookings(CLng(cboCustomer.SelectedValue))
        End If
    End Sub

    'opens the food ordering form for the booking just created
    Private Sub btnOrderFood_Click(sender As Object, e As EventArgs) Handles btnOrderFood.Click
        If lastBookingID = 0 Then
            MessageBox.Show("Create a booking first")
            Exit Sub
        End If

        OpenFoodOrder(lastBookingID)
    End Sub

    'rechecks the database to see if any selected seat has just been taken
    Private Function AnySelectedSeatTaken() As Boolean
        Dim dtTaken As New DataTable
        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            'join bookingseat to booking again so we can filter by screening
            SQLCmd.CommandText = "SELECT tblBookingSeat.SeatID " &
                                 "FROM tblBookingSeat INNER JOIN tblBooking ON tblBookingSeat.BookingID = tblBooking.BookingID " &
                                 "WHERE tblBooking.ScreeningID = @ScreeningID"
            SQLCmd.Parameters.AddWithValue("@ScreeningID", CInt(currentScreeningID))
            Dim da As New OleDbDataAdapter(SQLCmd)
            da.Fill(dtTaken)
            cn.Close()
        End If

        Dim i As Integer
        For i = 0 To pendingSeats.Rows.Count - 1
            If dtTaken.Select("SeatID = " & CLng(pendingSeats.Rows(i)("SeatID"))).Length > 0 Then
                Return True
            End If
        Next

        Return False
    End Function

    'clears any seat selection on the map
    'takes every seat back off the map and empties the food order. this is everything that makes
    'up the sale being built, so after this the form is ready to start a fresh one
    Private Sub ClearSaleInputs()
        'the picked seats go first, then every button that is still clickable is put back to the
        'available colour to match. a taken seat is disabled so it is left alone
        pendingSeats.Rows.Clear()

        For Each ctrl As Control In pnlSeatMap.Controls
            If TypeOf ctrl Is Button Then
                Dim b As Button = CType(ctrl, Button)
                If b.Enabled Then
                    b.BackColor = availableColour
                End If
            End If
        Next

        pendingFood.Rows.Clear()
        cboFoodItem.SelectedIndex = -1
        txtQuantity.Text = "1"

        UpdateTotal()
    End Sub

    'the button clears the sale and also forgets which past booking was being pointed at, so
    'nothing at all is left over from what the user was doing before
    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        ClearSaleInputs()

        lastBookingID = 0
        btnOrderFood.Enabled = False
        lblCustomerBookings.Text = "Pick a booking to add food to it"
        dgvCustomerBookings.ClearSelection()

        WriteLog("BOOKING", "Sale cleared before it was completed")
    End Sub

    'pressing enter in the quantity box adds the item, rather than reaching for the Add button
    Private Sub txtQuantity_KeyDown(sender As Object, e As KeyEventArgs) Handles txtQuantity.KeyDown
        If e.KeyCode = Keys.Enter Then
            btnAddFood.PerformClick()
            e.SuppressKeyPress = True
        End If
    End Sub

    'selects what is in the quantity box when it is clicked into, so a new number just replaces it
    Private Sub txtQuantity_Enter(sender As Object, e As EventArgs) Handles txtQuantity.Enter
        txtQuantity.SelectAll()
    End Sub

End Class
