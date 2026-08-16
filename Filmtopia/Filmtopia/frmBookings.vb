Imports System.Data.OleDb

Public Class frmBookings

    Private currentScreeningID As Long = 0
    Private currentScreenID As Long = 0
    Private currentTicketPrice As Double = 0
    Private lastBookingID As Long = 0

    Private pendingFood As DataTable

    Private pendingSeats As DataTable

    Private currentSeats As DataTable

    Private seatTips As New ToolTip

    Private availableColour As Color
    Private selectedColour As Color
    Private takenColour As Color

    Private Sub ApplySeatColours()
        availableColour = SeatAvailable
        selectedColour = SeatSelected
        takenColour = SeatTaken

        lblSwatchAvailable.BackColor = availableColour
        lblSwatchSelected.BackColor = selectedColour
        lblSwatchTaken.BackColor = takenColour

        lblSwatchPremium.BackColor = availableColour
        lblSwatchAccessible.BackColor = availableColour
        lblSwatchSaver.BackColor = availableColour
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

        Dim edgePen As New Pen(edge, 2)
        e.Graphics.DrawRectangle(edgePen, 1, 1, swatch.Width - 3, swatch.Height - 3)
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

    Public Sub SelectScreening(screeningID As Long)
        cboScreening.SelectedValue = screeningID

        If cboScreening.SelectedIndex = -1 Then
            MessageBox.Show("That screening cannot be sold at the moment. Either it has been cancelled or " &
                            "the screen it is in is out of service." & vbCrLf & vbCrLf &
                            "Pick another screening from the list.",
                            "Not on sale", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub LoadScreenings()
        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT tblScreening.ScreeningID, " &
                                 "FilmTitle & ' - ' & Format(ScreeningDate, 'dd/mm/yyyy') & ' ' & ScreeningTime & " &
                                 "' - ' & ScreenName AS Info " &
                                 "FROM (tblScreening INNER JOIN tblFilm ON tblScreening.FilmID = tblFilm.FilmID) " &
                                 "INNER JOIN tblScreen ON tblScreening.ScreenID = tblScreen.ScreenID " &
                                 "WHERE (ScreeningStatus IS NULL OR ScreeningStatus <> 'Cancelled') " &
                                 "AND (ScreenStatus IS NULL OR ScreenStatus <> @OutOfService) " &
                                 "ORDER BY ScreeningDate DESC, ScreeningTime DESC"
            SQLCmd.Parameters.AddWithValue("@OutOfService", ScreenOutOfService)
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

    Private Sub LoadCustomers()
        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT CustomerID, CustomerForename & ' ' & CustomerSurname AS CustomerName " &
                                 "FROM tblCustomer " &
                                 "WHERE CustomerForename <> @WalkInForename OR CustomerSurname <> @WalkInSurname " &
                                 "ORDER BY CustomerSurname, CustomerForename, CustomerID"
            SQLCmd.Parameters.AddWithValue("@WalkInForename", WalkInForename)
            SQLCmd.Parameters.AddWithValue("@WalkInSurname", WalkInSurname)
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

    Private Sub cboScreening_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboScreening.SelectedIndexChanged
        If cboScreening.SelectedIndex = -1 Then
            Exit Sub
        End If

        If Not IsNumeric(cboScreening.SelectedValue) Then
            Exit Sub
        End If

        currentScreeningID = CLng(cboScreening.SelectedValue)
        LoadScreeningDetails()
        BuildSeatMap()
    End Sub

    Private Sub cboCustomer_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboCustomer.SelectedIndexChanged
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

    Private Sub LoadCustomerBookings(customerID As Long)
        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT tblBooking.BookingID, IIf(IsNull(tblFilm.FilmTitle), 'Counter sale', tblFilm.FilmTitle & ' (' & ScreeningDate & ')') AS Info " &
                                 "FROM (tblBooking LEFT JOIN tblScreening ON tblBooking.ScreeningID = tblScreening.ScreeningID) " &
                                 "LEFT JOIN tblFilm ON tblScreening.FilmID = tblFilm.FilmID " &
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

        If dgvCustomerBookings.Columns.Count > 0 Then
            dgvCustomerBookings.Columns("BookingID").Width = 40
            dgvCustomerBookings.Columns("Info").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        End If
    End Sub

    Private Sub dgvCustomerBookings_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvCustomerBookings.CellClick
        If e.RowIndex < 0 Then Exit Sub

        Dim row As DataGridViewRow = dgvCustomerBookings.Rows(e.RowIndex)
        lastBookingID = CLng(row.Cells("BookingID").Value)
        btnOrderFood.Enabled = True
        lblCustomerBookings.Text = "Food will be added to booking " & lastBookingID
    End Sub

    Private Sub chkWalkIn_CheckedChanged(sender As Object, e As EventArgs) Handles chkWalkIn.CheckedChanged
        cboCustomer.Enabled = Not chkWalkIn.Checked

        If chkWalkIn.Checked Then
            cboCustomer.SelectedIndex = -1
            dgvCustomerBookings.DataSource = Nothing
            lastBookingID = 0
            btnOrderFood.Enabled = False
        End If
    End Sub

    Private Sub LoadScreeningDetails()
        currentScreenID = 0
        currentTicketPrice = 0

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

    Private Sub SetUpPendingSeats()
        pendingSeats = New DataTable
        pendingSeats.Columns.Add("SeatID", GetType(Integer))
        pendingSeats.Columns.Add("Multiplier", GetType(Double))
    End Sub

    Private Function IsSeatSelected(seatID As Long) As Boolean
        Return pendingSeats.Select("SeatID = " & seatID).Length > 0
    End Function

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

    Private Sub LoadFoodItems()
        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT FoodItemID, FoodItemName, FoodItemPrice " &
                                 "FROM tblFoodItem " &
                                 "WHERE (FoodItemStatus IS NULL OR FoodItemStatus <> @Withdrawn) " &
                                 "ORDER BY FoodItemName"
            SQLCmd.Parameters.AddWithValue("@Withdrawn", FoodWithdrawn)
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

    Private Sub btnAddFood_Click(sender As Object, e As EventArgs) Handles btnAddFood.Click
        If cboFoodItem.SelectedIndex = -1 Then
            MessageBox.Show("Pick a food or drink item first", "Booking", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Dim quantity As Integer = SafeInt(txtQuantity.Text)
        If quantity < 1 Then
            MessageBox.Show("Enter a quantity of 1 or more", "Booking", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Dim chosen As DataRowView = CType(cboFoodItem.SelectedItem, DataRowView)
        Dim itemID As Integer = CInt(chosen("FoodItemID"))
        Dim price As Double = CDbl(chosen("FoodItemPrice"))

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

    Private Sub btnRemoveFood_Click(sender As Object, e As EventArgs) Handles btnRemoveFood.Click
        If dgvPendingFood.CurrentRow Is Nothing Then
            MessageBox.Show("Pick a line in the food list first", "Booking", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        pendingFood.Rows.RemoveAt(dgvPendingFood.CurrentRow.Index)
        UpdateTotal()
    End Sub

    Private Function FoodTotal() As Double
        Dim total As Double = 0
        Dim i As Integer

        For i = 0 To pendingFood.Rows.Count - 1
            total = total + CDbl(pendingFood.Rows(i)("Subtotal"))
        Next

        Return total
    End Function

    Private Sub BuildSeatMap()
        ApplySeatColours()
        seatTips.RemoveAll()
        ClearPanel(pnlSeatMap)

        pendingSeats.Rows.Clear()

        currentSeats = New DataTable
        Dim dtSeats As DataTable = currentSeats
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
            da.Fill(dtSeats)

            SQLCmd.CommandText = "SELECT SeatID FROM tblBookingSeat " &
                                 "WHERE ScreeningID = @ScreeningID"
            SQLCmd.Parameters.Clear()
            SQLCmd.Parameters.AddWithValue("@ScreeningID", CInt(currentScreeningID))
            Dim da2 As New OleDbDataAdapter(SQLCmd)
            da2.Fill(dtTaken)

            cn.Close()
        End If

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

            Dim rowIndex As Integer = Asc(seatRow) - 65
            b.Location = New Point((seatNumber - 1) * 45 + 10, rowIndex * 45 + 10)

            If seatType = SeatPremium Then
                b.FlatStyle = FlatStyle.Flat
                b.FlatAppearance.BorderSize = 2
                b.FlatAppearance.BorderColor = SeatPremiumEdge
            ElseIf seatType = SeatAccessible Then
                b.FlatStyle = FlatStyle.Flat
                b.FlatAppearance.BorderSize = 2
                b.FlatAppearance.BorderColor = SeatAccessibleEdge
            ElseIf seatType = SeatSaver Then
                b.FlatStyle = FlatStyle.Flat
                b.FlatAppearance.BorderSize = 2
                b.FlatAppearance.BorderColor = SeatSaverEdge
            End If

            seatTips.SetToolTip(b, seatRow & seatNumber & " - " & seatType & " - " &
                                   FormatCurrency(SeatPrice(currentTicketPrice, multiplier)))

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

    Private Function MultiplierForSeat(seatID As Long) As Double
        Dim rows() As DataRow = currentSeats.Select("SeatID = " & seatID)

        If rows.Length > 0 Then
            Return CDbl(rows(0)("PriceMultiplier"))
        End If

        Return 1
    End Function

    Private Function CountSelectedSeats() As Integer
        Return pendingSeats.Rows.Count
    End Function

    Private Function TicketsTotal() As Double
        Dim total As Double = 0
        Dim i As Integer

        For i = 0 To pendingSeats.Rows.Count - 1
            total = total + SeatPrice(currentTicketPrice, CDbl(pendingSeats.Rows(i)("Multiplier")))
        Next

        Return total
    End Function

    Private Sub UpdateTotal()
        Dim seatCount As Integer = CountSelectedSeats()
        Dim ticketsCost As Double = TicketsTotal()
        Dim foodCost As Double = FoodTotal()

        lblTickets.Text = "Tickets (" & seatCount & ")  -  " & FormatCurrency(ticketsCost)
        lblFoodTotal.Text = "Food and drink  -  " & FormatCurrency(foodCost)
        lblTotal.Text = "TOTAL  " & FormatCurrency(ticketsCost + foodCost)

        btnCreateBooking.Enabled = (seatCount > 0 Or pendingFood.Rows.Count > 0)
    End Sub

    Private Sub btnCreateBooking_Click(sender As Object, e As EventArgs) Handles btnCreateBooking.Click
        Dim seatCount As Integer = CountSelectedSeats()

        If seatCount = 0 And pendingFood.Rows.Count = 0 Then
            MessageBox.Show("Pick some seats, or add something from the food and drink list", "Booking", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If
        If seatCount > 0 And currentScreeningID = 0 Then
            MessageBox.Show("Pick a screening first", "Booking", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If
        If Not chkWalkIn.Checked And cboCustomer.SelectedIndex = -1 Then
            MessageBox.Show("Pick a customer first, or tick Walk-in", "Booking", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If AnySelectedSeatTaken() Then
            MessageBox.Show("One of your seats has just been booked, please reselect", "Booking", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            BuildSeatMap()
            Exit Sub
        End If

        Dim ticketsCost As Double = TicketsTotal()
        Dim totalCost As Double = ticketsCost + FoodTotal()

        Dim customerID As Long = 0
        If Not chkWalkIn.Checked Then
            customerID = CLng(cboCustomer.SelectedValue)
        End If

        Dim newBookingID As Long = CompleteSale(customerID, chkWalkIn.Checked, currentScreeningID,
                                                GetSelectedSeatIDs(), pendingFood, totalCost,
                                                CurrentLoginID)

        If newBookingID = 0 Then
            Exit Sub
        End If

        WriteLog("BOOKING", "Sale " & newBookingID & " completed, " & seatCount & " seat(s) and " &
                            pendingFood.Rows.Count & " food line(s), " & FormatCurrency(totalCost))

        MessageBox.Show("Sale complete." & vbNewLine & vbNewLine &
                        "Booking " & newBookingID & vbNewLine &
                        seatCount & " seat(s) and " & pendingFood.Rows.Count & " food item line(s)" & vbNewLine &
                        "Total " & FormatCurrency(totalCost),
                        "Sale Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)

        lastBookingID = newBookingID
        btnOrderFood.Enabled = True
        BuildSeatMap()
        ClearSaleInputs()
        lblCustomerBookings.Text = "Food can still be added to booking " & newBookingID

        If Not chkWalkIn.Checked Then
            LoadCustomerBookings(CLng(cboCustomer.SelectedValue))
        End If
    End Sub

    Private Function GetSelectedSeatIDs() As Long()
        Dim seatIDs(pendingSeats.Rows.Count - 1) As Long
        Dim i As Integer

        For i = 0 To pendingSeats.Rows.Count - 1
            seatIDs(i) = CLng(pendingSeats.Rows(i)("SeatID"))
        Next

        Return seatIDs
    End Function

    Private Sub OpenFoodOrder(bookingID As Long)
        frmFoodOrder.currentBookingID = bookingID
        frmFoodOrder.ShowDialog()

        If Not chkWalkIn.Checked And cboCustomer.SelectedIndex <> -1 Then
            LoadCustomerBookings(CLng(cboCustomer.SelectedValue))
        End If
    End Sub

    Private Sub btnOrderFood_Click(sender As Object, e As EventArgs) Handles btnOrderFood.Click
        If lastBookingID = 0 Then
            MessageBox.Show("Create a booking first", "Booking", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        OpenFoodOrder(lastBookingID)
    End Sub

    Private Function AnySelectedSeatTaken() As Boolean
        Dim dtTaken As New DataTable
        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT SeatID FROM tblBookingSeat " &
                                 "WHERE ScreeningID = @ScreeningID"
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

    Private Sub ClearSaleInputs()
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

    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        ClearSaleInputs()

        lastBookingID = 0
        btnOrderFood.Enabled = False
        lblCustomerBookings.Text = "Pick a booking to add food to it"
        dgvCustomerBookings.ClearSelection()
    End Sub

    Private Sub txtQuantity_KeyDown(sender As Object, e As KeyEventArgs) Handles txtQuantity.KeyDown
        If e.KeyCode = Keys.Enter Then
            btnAddFood.PerformClick()
            e.SuppressKeyPress = True
        End If
    End Sub

    Private Sub txtQuantity_Enter(sender As Object, e As EventArgs) Handles txtQuantity.Enter
        txtQuantity.SelectAll()
    End Sub

End Class
