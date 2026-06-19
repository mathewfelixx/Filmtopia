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

    'the three seat colours, made with FromArgb so the colour checks match properly
    Private availableColour As Color = Color.FromArgb(220, 220, 220)
    Private selectedColour As Color = Color.Fuchsia
    Private takenColour As Color = Color.FromArgb(255, 192, 255)

    Private Sub frmBookings_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CommonFormStartup()
        LoadScreenings()
        LoadCustomers()
        WriteLog("BOOKING", "Bookings form opened")
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
        lastBookingID = 0
        btnOrderFood.Enabled = False

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
            'join booking to screening, then to film, so we can show the film title and date
            SQLCmd.CommandText = "SELECT tblBooking.BookingID, FilmTitle & ' (' & ScreeningDate & ')' AS Info " &
                                 "FROM (tblBooking INNER JOIN tblScreening ON tblBooking.ScreeningID = tblScreening.ScreeningID) " &
                                 "INNER JOIN tblFilm ON tblScreening.FilmID = tblFilm.FilmID " &
                                 "WHERE tblBooking.CustomerID = @CustomerID"
            SQLCmd.Parameters.AddWithValue("@CustomerID", CInt(customerID))
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
    Private Sub BuildSeatMap()
        pnlSeatMap.Controls.Clear()

        Dim dtSeats As New DataTable
        Dim dtTaken As New DataTable

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn

            'all the seats that belong to this screen
            SQLCmd.CommandText = "SELECT SeatID, SeatRow, SeatNumber " &
                                 "FROM tblSeat " &
                                 "WHERE ScreenID = @ScreenID " &
                                 "ORDER BY SeatRow, SeatNumber"
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
                b.ForeColor = Color.White
                b.Enabled = False
            Else
                b.BackColor = availableColour
                AddHandler b.Click, AddressOf Seat_Click
            End If

            pnlSeatMap.Controls.Add(b)
        Next

        UpdateTotal()
    End Sub

    'toggles a seat between selected and available when its clicked
    Private Sub Seat_Click(sender As Object, e As EventArgs)
        Dim b As Button = CType(sender, Button)
        If b.BackColor = selectedColour Then
            b.BackColor = availableColour
        Else
            b.BackColor = selectedColour
        End If
        UpdateTotal()
    End Sub

    'counts how many seats are selected on the map
    Private Function CountSelectedSeats() As Integer
        Dim count As Integer = 0
        For Each ctrl As Control In pnlSeatMap.Controls
            If TypeOf ctrl Is Button Then
                Dim b As Button = CType(ctrl, Button)
                If b.BackColor = selectedColour Then
                    count = count + 1
                End If
            End If
        Next
        Return count
    End Function

    'shows the running total of selected seats and their cost
    Private Sub UpdateTotal()
        Dim count As Integer = CountSelectedSeats()
        lblTotal.Text = count & " seats selected - " & FormatCurrency(count * currentTicketPrice)
    End Sub

    'creates a booking from the picked screening, customer and selected seats
    Private Sub btnCreateBooking_Click(sender As Object, e As EventArgs) Handles btnCreateBooking.Click
        'make sure everything needed has been picked first
        If currentScreeningID = 0 Then
            MessageBox.Show("Pick a screening first")
            Exit Sub
        End If
        If Not chkWalkIn.Checked And cboCustomer.SelectedIndex = -1 Then
            MessageBox.Show("Pick a customer first, or tick Walk-in")
            Exit Sub
        End If

        Dim seatCount As Integer = CountSelectedSeats()
        If seatCount = 0 Then
            MessageBox.Show("Select at least one seat")
            Exit Sub
        End If

        'safety check in case a selected seat got booked since the map loaded
        If AnySelectedSeatTaken() Then
            MessageBox.Show("One of your seats has just been booked, please reselect")
            BuildSeatMap()
            Exit Sub
        End If

        Dim newBookingID As Long = 0
        Dim totalCost As Double = seatCount * currentTicketPrice

        'if its a walk-in, make a quick customer record so the booking still has someone to belong to
        Dim bookingCustomerID As Long
        If chkWalkIn.Checked Then
            bookingCustomerID = CreateWalkInCustomer()
        Else
            bookingCustomerID = CLng(cboCustomer.SelectedValue)
        End If

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "INSERT INTO tblBooking (CustomerID, ScreeningID, BookingDate, TotalCost) " &
                                 "VALUES (@CustomerID, @ScreeningID, @BookingDate, @TotalCost)"
            SQLCmd.Parameters.AddWithValue("@CustomerID", CInt(bookingCustomerID))
            SQLCmd.Parameters.AddWithValue("@ScreeningID", CInt(currentScreeningID))
            SQLCmd.Parameters.AddWithValue("@BookingDate", Date.Now.Date)
            SQLCmd.Parameters.AddWithValue("@TotalCost", totalCost)
            SQLCmd.ExecuteNonQuery()

            'grab the id just given to the new booking so we can link its seats
            SQLCmd.CommandText = "SELECT @@IDENTITY"
            SQLCmd.Parameters.Clear()
            newBookingID = CLng(SQLCmd.ExecuteScalar())
            cn.Close()
        End If

        SaveBookingSeats(newBookingID)

        WriteLog("BOOKING", "Booking " & newBookingID & " created with " & seatCount & " seats")
        MessageBox.Show("Booking created")

        'remember this booking so food can be ordered for it
        lastBookingID = newBookingID
        btnOrderFood.Enabled = True

        BuildSeatMap()

        'walk-ins dont have a customer picked in the combo, so theres no list to refresh
        If Not chkWalkIn.Checked Then
            LoadCustomerBookings(CLng(cboCustomer.SelectedValue))
        End If
    End Sub

    'makes a zero-seat booking for someone who just wants to buy food, then opens food ordering for it
    Private Sub btnFoodOnly_Click(sender As Object, e As EventArgs) Handles btnFoodOnly.Click
        If currentScreeningID = 0 Then
            MessageBox.Show("Pick a screening first")
            Exit Sub
        End If

        Dim newCustomerID As Long = CreateWalkInCustomer()
        Dim newBookingID As Long = 0

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "INSERT INTO tblBooking (CustomerID, ScreeningID, BookingDate, TotalCost) " &
                                 "VALUES (@CustomerID, @ScreeningID, @BookingDate, @TotalCost)"
            SQLCmd.Parameters.AddWithValue("@CustomerID", CInt(newCustomerID))
            SQLCmd.Parameters.AddWithValue("@ScreeningID", CInt(currentScreeningID))
            SQLCmd.Parameters.AddWithValue("@BookingDate", Date.Now.Date)
            SQLCmd.Parameters.AddWithValue("@TotalCost", 0)
            SQLCmd.ExecuteNonQuery()

            SQLCmd.CommandText = "SELECT @@IDENTITY"
            SQLCmd.Parameters.Clear()
            newBookingID = CLng(SQLCmd.ExecuteScalar())
            cn.Close()
        End If

        WriteLog("BOOKING", "Food-only booking " & newBookingID & " created with no seats")

        frmFoodOrder.currentBookingID = newBookingID
        frmFoodOrder.ShowDialog()
    End Sub

    'opens the food ordering form for the booking just created
    Private Sub btnOrderFood_Click(sender As Object, e As EventArgs) Handles btnOrderFood.Click
        If lastBookingID = 0 Then
            MessageBox.Show("Create a booking first")
            Exit Sub
        End If

        frmFoodOrder.currentBookingID = lastBookingID
        frmFoodOrder.ShowDialog()
    End Sub

    'inserts a tblBookingSeat row for each selected seat on the map
    Private Sub SaveBookingSeats(bookingID As Long)
        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            For Each ctrl As Control In pnlSeatMap.Controls
                If TypeOf ctrl Is Button Then
                    Dim b As Button = CType(ctrl, Button)
                    If b.BackColor = selectedColour Then
                        SQLCmd.CommandText = "INSERT INTO tblBookingSeat (BookingID, SeatID) " &
                                             "VALUES (@BookingID, @SeatID)"
                        SQLCmd.Parameters.Clear()
                        SQLCmd.Parameters.AddWithValue("@BookingID", CInt(bookingID))
                        SQLCmd.Parameters.AddWithValue("@SeatID", CInt(b.Tag))
                        SQLCmd.ExecuteNonQuery()
                    End If
                End If
            Next
            cn.Close()
        End If
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

        For Each ctrl As Control In pnlSeatMap.Controls
            If TypeOf ctrl Is Button Then
                Dim b As Button = CType(ctrl, Button)
                If b.BackColor = selectedColour Then
                    If dtTaken.Select("SeatID = " & CLng(b.Tag)).Length > 0 Then
                        Return True
                    End If
                End If
            End If
        Next
        Return False
    End Function

    'clears any seat selection on the map
    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        For Each ctrl As Control In pnlSeatMap.Controls
            If TypeOf ctrl Is Button Then
                Dim b As Button = CType(ctrl, Button)
                If b.BackColor = selectedColour Then
                    b.BackColor = availableColour
                End If
            End If
        Next
        UpdateTotal()
        WriteLog("BOOKING", "Seat selection cleared")
    End Sub

End Class
