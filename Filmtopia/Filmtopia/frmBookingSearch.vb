Imports System.Data.OleDb

Public Class frmBookingSearch

    'the booking currently selected in the grid, 0 means nothing selected
    Private selectedBookingID As Integer = 0
    'a description of that booking, kept so it can go in the log after the booking is deleted
    Private selectedBookingText As String = ""

    Private Sub frmBookingSearch_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CommonFormStartup(Me)
        LoadBookings("")
        LoadScreeningsCombo()
        WriteLog("BOOKING", "Booking search form opened")
    End Sub

    'fills the screening combo for the door list section
    Private Sub LoadScreeningsCombo()
        Dim dt As New DataTable

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT ScreeningID, FilmTitle & ' - ' & ScreeningDate & ' ' & ScreeningTime AS Info " &
                                 "FROM tblScreening INNER JOIN tblFilm ON tblScreening.FilmID = tblFilm.FilmID"
            Dim da As New OleDbDataAdapter(SQLCmd)
            da.Fill(dt)
            cn.Close()
        End If

        cboDoorListScreening.DataSource = dt
        cboDoorListScreening.DisplayMember = "Info"
        cboDoorListScreening.ValueMember = "ScreeningID"
        cboDoorListScreening.SelectedIndex = -1
    End Sub

    'searches by booking id if a number was typed, otherwise by customer name
    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        LoadBookings(txtSearch.Text.Trim())
        WriteLog("BOOKING", "Booking search run for '" & txtSearch.Text.Trim() & "'")
    End Sub

    'clears the search box and shows every booking
    Private Sub btnShowAll_Click(sender As Object, e As EventArgs) Handles btnShowAll.Click
        txtSearch.Text = ""
        LoadBookings("")
    End Sub

    'loads the bookings grid, filtered by booking id or customer name if given
    Private Sub LoadBookings(searchText As String)
        Dim dt As New DataTable

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn

            'join booking to customer (for the name), then to screening, then to film (for the title)
            Dim baseQuery As String = "SELECT tblBooking.BookingID, CustomerForename & ' ' & CustomerSurname AS CustomerName, FilmTitle, ScreeningDate, ScreeningTime, TotalCost " &
                                      "FROM ((tblBooking INNER JOIN tblCustomer ON tblBooking.CustomerID = tblCustomer.CustomerID) " &
                                      "INNER JOIN tblScreening ON tblBooking.ScreeningID = tblScreening.ScreeningID) " &
                                      "INNER JOIN tblFilm ON tblScreening.FilmID = tblFilm.FilmID"

            If searchText = "" Then
                SQLCmd.CommandText = baseQuery
            ElseIf IsNumeric(searchText) Then
                SQLCmd.CommandText = baseQuery & " WHERE tblBooking.BookingID = @SearchID"
                SQLCmd.Parameters.AddWithValue("@SearchID", CInt(searchText))
            Else
                'search the customer name and the film title, so typing a film finds everyone
                'booked onto it. the two parameters are added in the order they appear in the SQL
                SQLCmd.CommandText = baseQuery & " WHERE CustomerForename & ' ' & CustomerSurname LIKE @SearchName " &
                                     "OR FilmTitle LIKE @SearchFilm"
                SQLCmd.Parameters.AddWithValue("@SearchName", "%" & searchText & "%")
                SQLCmd.Parameters.AddWithValue("@SearchFilm", "%" & searchText & "%")
            End If

            Dim da As New OleDbDataAdapter(SQLCmd)
            da.Fill(dt)
            cn.Close()
        End If

        dgvBookings.DataSource = dt

        If dgvBookings.Columns.Count > 0 Then
            dgvBookings.Columns("BookingID").HeaderText = "Booking"
            dgvBookings.Columns("CustomerName").HeaderText = "Customer"
            dgvBookings.Columns("FilmTitle").HeaderText = "Film"
            dgvBookings.Columns("ScreeningDate").HeaderText = "Date"
            dgvBookings.Columns("ScreeningTime").HeaderText = "Time"
            dgvBookings.Columns("TotalCost").HeaderText = "Total"

            dgvBookings.Columns("BookingID").Width = 80
            dgvBookings.Columns("CustomerName").Width = 200
            dgvBookings.Columns("FilmTitle").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            dgvBookings.Columns("ScreeningDate").Width = 110
            dgvBookings.Columns("ScreeningTime").Width = 80
            dgvBookings.Columns("TotalCost").Width = 100

            dgvBookings.Columns("ScreeningDate").DefaultCellStyle.Format = "dd/MM/yyyy"
            dgvBookings.Columns("TotalCost").DefaultCellStyle.Format = "C"
            dgvBookings.ClearSelection()
        End If

        'say how many were found so an empty grid does not look like something went wrong
        If dt.Rows.Count = 1 Then
            lblResultCount.Text = "1 booking found"
        Else
            lblResultCount.Text = dt.Rows.Count & " bookings found"
        End If

        ClearSelectedBooking()
    End Sub

    'nothing is picked, so there is nothing to cancel
    Private Sub ClearSelectedBooking()
        selectedBookingID = 0
        selectedBookingText = ""
        lblSelectedBooking.Text = "No booking selected"
        btnCancelBooking.Enabled = False
    End Sub

    'pressing enter in the search box searches, rather than having to reach for the button
    Private Sub txtSearch_KeyDown(sender As Object, e As KeyEventArgs) Handles txtSearch.KeyDown
        If e.KeyCode = Keys.Enter Then
            LoadBookings(txtSearch.Text.Trim())
            e.SuppressKeyPress = True
        End If
    End Sub

    'remembers which booking was clicked and says on screen which one it is, so the user can see
    'exactly what they are about to cancel before they press the button
    Private Sub dgvBookings_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvBookings.CellClick
        If e.RowIndex < 0 Then Exit Sub

        Dim row As DataGridViewRow = dgvBookings.Rows(e.RowIndex)
        selectedBookingID = CInt(row.Cells("BookingID").Value)

        selectedBookingText = row.Cells("CustomerName").Value.ToString() & " - " &
                              row.Cells("FilmTitle").Value.ToString()

        lblSelectedBooking.Text = "Selected: booking " & selectedBookingID & " for " & selectedBookingText
        btnCancelBooking.Enabled = True
    End Sub

    'cancels the selected booking, removing its seats, food order and the booking itself
    Private Sub btnCancelBooking_Click(sender As Object, e As EventArgs) Handles btnCancelBooking.Click
        If selectedBookingID = 0 Then
            MessageBox.Show("Select a booking in the grid first")
            Exit Sub
        End If

        If MessageBox.Show("Cancel this booking? This will free its seats and remove any food order.", "Confirm", MessageBoxButtons.YesNo) = DialogResult.No Then
            Exit Sub
        End If

        Dim cancelledID As Integer = selectedBookingID
        Dim cancelledWhat As String = selectedBookingText
        Dim seatsFreed As Integer = 0
        Dim worked As Boolean = False

        If DbConnect() Then
            'the seats going back on sale and the booking being marked cancelled are done as one
            'transaction. without this, if something went wrong partway through, the seats could be
            'freed on a booking that still says it is active. this way either both happen or neither
            Dim trans As OleDbTransaction = cn.BeginTransaction()

            Try
                Dim SQLCmd As New OleDbCommand
                SQLCmd.Connection = cn
                SQLCmd.Transaction = trans

                'the seats really are removed rather than just marked. they have to be, because the
                'database will not let the same seat be sold twice on a screening, so leaving the
                'rows behind would keep the seat off sale for good. the food order is left alone so
                'there is still a record of what was bought and refunded
                SQLCmd.CommandText = "DELETE FROM tblBookingSeat " &
                                     "WHERE BookingID = @BookingID"
                SQLCmd.Parameters.AddWithValue("@BookingID", cancelledID)
                seatsFreed = SQLCmd.ExecuteNonQuery()

                'the booking is kept and marked instead of being deleted. before this it was thrown
                'away completely, which meant a cancelled sale vanished out of the history and the
                'sales report could never show what had been refunded
                SQLCmd.CommandText = "UPDATE tblBooking " &
                                     "SET BookingStatus = @BookingStatus, CancelledDate = @CancelledDate " &
                                     "WHERE BookingID = @BookingID AND BookingStatus <> @AlreadyCancelled"
                SQLCmd.Parameters.Clear()
                SQLCmd.Parameters.AddWithValue("@BookingStatus", BookingCancelled)
                SQLCmd.Parameters.AddWithValue("@CancelledDate", Date.Now)
                SQLCmd.Parameters.AddWithValue("@BookingID", cancelledID)
                SQLCmd.Parameters.AddWithValue("@AlreadyCancelled", BookingCancelled)

                'if this is zero the booking has gone or was already cancelled, so nothing is committed
                If SQLCmd.ExecuteNonQuery() = 0 Then
                    trans.Rollback()
                    MessageBox.Show("That booking could not be cancelled. It may have already been cancelled.", "Cancel Booking", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Else
                    trans.Commit()
                    worked = True
                End If

            Catch ex As Exception
                trans.Rollback()
                MessageBox.Show("The booking could not be cancelled, so nothing was changed. " & ex.Message, "Cancel Booking", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try

            cn.Close()
        End If

        'the log is written after the connection is closed because WriteLog opens it again
        If worked Then
            WriteLog("BOOKING", "Booking " & cancelledID & " cancelled (" & cancelledWhat & "), " & seatsFreed & " seat(s) freed", LogChange)
            MessageBox.Show("Booking cancelled and " & seatsFreed & " seat(s) put back on sale." & vbCrLf &
                            "The booking is still in the list, marked as cancelled.",
                            "Cancel Booking", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If

        LoadBookings(txtSearch.Text.Trim())
    End Sub

    'loads the door list grid: every seat booked on the selected screening, in seat order, with
    'the booking number so a ticket on the door can be matched to the list
    Private Sub btnLoadDoorList_Click(sender As Object, e As EventArgs) Handles btnLoadDoorList.Click
        If cboDoorListScreening.SelectedIndex = -1 Then
            MessageBox.Show("Select a screening first")
            Exit Sub
        End If

        Dim dt As New DataTable

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            'join booking to customer (name), then to bookingseat and seat to get the seat itself
            SQLCmd.CommandText = "SELECT tblBooking.BookingID, CustomerForename & ' ' & CustomerSurname AS CustomerName, tblSeat.SeatRow & tblSeat.SeatNumber AS SeatNumber " &
                                 "FROM ((tblBooking INNER JOIN tblCustomer ON tblBooking.CustomerID = tblCustomer.CustomerID) " &
                                 "INNER JOIN tblBookingSeat ON tblBooking.BookingID = tblBookingSeat.BookingID) " &
                                 "INNER JOIN tblSeat ON tblBookingSeat.SeatID = tblSeat.SeatID " &
                                 "WHERE tblBooking.ScreeningID = @ScreeningID " &
                                 "ORDER BY tblSeat.SeatRow, tblSeat.SeatNumber"
            SQLCmd.Parameters.AddWithValue("@ScreeningID", CInt(cboDoorListScreening.SelectedValue))
            Dim da As New OleDbDataAdapter(SQLCmd)
            da.Fill(dt)
            cn.Close()
        End If

        dgvDoorList.DataSource = dt

        If dgvDoorList.Columns.Count > 0 Then
            dgvDoorList.Columns("BookingID").HeaderText = "Booking"
            dgvDoorList.Columns("CustomerName").HeaderText = "Customer"
            dgvDoorList.Columns("SeatNumber").HeaderText = "Seat"
            dgvDoorList.Columns("BookingID").Width = 90
            dgvDoorList.Columns("CustomerName").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            dgvDoorList.Columns("SeatNumber").Width = 120
        End If

        'put the number of seats on the group box heading so it is obvious how busy it is
        If dt.Rows.Count = 0 Then
            GroupBox2.Text = "Door list - nobody has booked this one yet"
        Else
            GroupBox2.Text = "Door list - " & dt.Rows.Count & " seat(s) booked"
        End If

        WriteLog("BOOKING", "Door list loaded for ScreeningID " & cboDoorListScreening.SelectedValue.ToString())
    End Sub

    'saves the door list grid to a csv file so it can be printed and taken to the door
    Private Sub btnExportDoorList_Click(sender As Object, e As EventArgs) Handles btnExportDoorList.Click
        If dgvDoorList.Rows.Count = 0 Then
            MessageBox.Show("Load a door list first")
            Exit Sub
        End If

        Dim saveDialog As New SaveFileDialog
        saveDialog.Filter = "CSV files (*.csv)|*.csv"
        saveDialog.FileName = "DoorList.csv"

        If saveDialog.ShowDialog() = DialogResult.OK Then
            Dim writer As New System.IO.StreamWriter(saveDialog.FileName)

            'write the header row
            writer.WriteLine("Booking,Customer Name,Seat")

            'write each row of the door list
            For Each row As DataGridViewRow In dgvDoorList.Rows
                writer.WriteLine(CsvField(row.Cells("BookingID").Value.ToString()) & "," &
                                 CsvField(row.Cells("CustomerName").Value.ToString()) & "," &
                                 CsvField(row.Cells("SeatNumber").Value.ToString()))
            Next

            writer.Close()

            WriteLog("BOOKING", "Door list exported for ScreeningID " & cboDoorListScreening.SelectedValue.ToString())
            MessageBox.Show("Door list exported")
        End If
    End Sub

End Class
