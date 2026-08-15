Imports System.Data.OleDb

Public Class frmBookingSearch

    Private selectedBookingID As Integer = 0
    Private selectedBookingText As String = ""

    Private Sub frmBookingSearch_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CommonFormStartup(Me)

        Me.KeyPreview = True

        LoadBookings("")
        LoadScreeningsCombo()
        txtSearch.Focus()
        WriteLog("BOOKING", "Booking search form opened")
    End Sub

    Private Sub frmBookingSearch_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.F5 Then
            LoadBookings(txtSearch.Text.Trim())
        ElseIf e.KeyCode = Keys.Escape Then
            If txtSearch.Text <> "" Then
                txtSearch.Text = ""
                LoadBookings("")
            Else
                Me.Close()
            End If
        End If
    End Sub

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

    Public Sub SelectBooking(bookingID As Long)
        txtSearch.Text = bookingID.ToString()
        LoadBookings(txtSearch.Text.Trim())
    End Sub

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        LoadBookings(txtSearch.Text.Trim())
        WriteLog("BOOKING", "Booking search run for '" & txtSearch.Text.Trim() & "'")
    End Sub

    Private Sub btnShowAll_Click(sender As Object, e As EventArgs) Handles btnShowAll.Click
        txtSearch.Text = ""
        LoadBookings("")
    End Sub

    Private Sub LoadBookings(searchText As String)
        Dim dt As New DataTable

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn

            Dim baseQuery As String = "SELECT tblBooking.BookingID, CustomerForename & ' ' & CustomerSurname AS CustomerName, FilmTitle, ScreeningDate, ScreeningTime, TotalCost, BookingStatus " &
                                      "FROM ((tblBooking INNER JOIN tblCustomer ON tblBooking.CustomerID = tblCustomer.CustomerID) " &
                                      "INNER JOIN tblScreening ON tblBooking.ScreeningID = tblScreening.ScreeningID) " &
                                      "INNER JOIN tblFilm ON tblScreening.FilmID = tblFilm.FilmID"

            If searchText = "" Then
                SQLCmd.CommandText = baseQuery & " ORDER BY tblBooking.BookingID DESC"
            ElseIf IsNumeric(searchText) Then
                SQLCmd.CommandText = baseQuery & " WHERE tblBooking.BookingID = @SearchID " &
                                     "ORDER BY tblBooking.BookingID DESC"
                SQLCmd.Parameters.AddWithValue("@SearchID", CInt(searchText))
            Else
                SQLCmd.CommandText = baseQuery & " WHERE CustomerForename & ' ' & CustomerSurname LIKE @SearchName " &
                                     "OR FilmTitle LIKE @SearchFilm " &
                                     "ORDER BY tblBooking.BookingID DESC"
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
            dgvBookings.Columns("BookingStatus").HeaderText = "Status"

            dgvBookings.Columns("BookingID").Width = 80
            dgvBookings.Columns("CustomerName").Width = 200
            dgvBookings.Columns("FilmTitle").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            dgvBookings.Columns("ScreeningDate").Width = 110
            dgvBookings.Columns("ScreeningTime").Width = 80
            dgvBookings.Columns("TotalCost").Width = 100
            dgvBookings.Columns("BookingStatus").Width = 90

            dgvBookings.Columns("ScreeningDate").DefaultCellStyle.Format = "dd/MM/yyyy"
            dgvBookings.Columns("TotalCost").DefaultCellStyle.Format = "C"
            GreyOutCancelled()
            dgvBookings.ClearSelection()
        End If

        If dt.Rows.Count = 1 Then
            lblResultCount.Text = "1 booking found"
        Else
            lblResultCount.Text = dt.Rows.Count & " bookings found"
        End If

        ClearSelectedBooking()
    End Sub

    Private Sub GreyOutCancelled()
        For Each row As DataGridViewRow In dgvBookings.Rows
            If row.Cells("BookingStatus").Value IsNot Nothing Then
                If row.Cells("BookingStatus").Value.ToString() = BookingCancelled Then
                    row.DefaultCellStyle.ForeColor = PastFore
                End If
            End If
        Next
    End Sub

    Private Sub ClearSelectedBooking()
        selectedBookingID = 0
        selectedBookingText = ""
        lblSelectedBooking.Text = "No booking selected"
        btnCancelBooking.Enabled = False
    End Sub

    Private Sub txtSearch_KeyDown(sender As Object, e As KeyEventArgs) Handles txtSearch.KeyDown
        If e.KeyCode = Keys.Enter Then
            LoadBookings(txtSearch.Text.Trim())
            e.SuppressKeyPress = True
        End If
    End Sub

    Private Sub dgvBookings_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvBookings.CellClick
        If e.RowIndex < 0 Then Exit Sub

        Dim row As DataGridViewRow = dgvBookings.Rows(e.RowIndex)
        selectedBookingID = CInt(row.Cells("BookingID").Value)

        selectedBookingText = row.Cells("CustomerName").Value.ToString() & " - " &
                              row.Cells("FilmTitle").Value.ToString()

        Dim isCancelled As Boolean = False
        If row.Cells("BookingStatus").Value IsNot Nothing Then
            isCancelled = (row.Cells("BookingStatus").Value.ToString() = BookingCancelled)
        End If

        If isCancelled Then
            lblSelectedBooking.Text = "Booking " & selectedBookingID & " for " & selectedBookingText & " is already cancelled"
        Else
            lblSelectedBooking.Text = "Selected: booking " & selectedBookingID & " for " & selectedBookingText
        End If

        btnCancelBooking.Enabled = Not isCancelled
    End Sub

    Private Sub btnCancelBooking_Click(sender As Object, e As EventArgs) Handles btnCancelBooking.Click
        If selectedBookingID = 0 Then
            MessageBox.Show("Select a booking in the grid first", "Booking Search", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If MessageBox.Show("Cancel this booking? Its seats go back on sale." & vbCrLf &
                           "The booking itself is kept and marked as cancelled, so the sale is still in the history.",
                           "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
            Exit Sub
        End If

        Dim cancelledID As Integer = selectedBookingID
        Dim cancelledWhat As String = selectedBookingText
        Dim seatsFreed As Integer = 0
        Dim worked As Boolean = False

        If DbConnect() Then
            Dim trans As OleDbTransaction = cn.BeginTransaction()

            Try
                Dim SQLCmd As New OleDbCommand
                SQLCmd.Connection = cn
                SQLCmd.Transaction = trans

                SQLCmd.CommandText = "DELETE FROM tblBookingSeat " &
                                     "WHERE BookingID = @BookingID"
                SQLCmd.Parameters.AddWithValue("@BookingID", cancelledID)
                seatsFreed = SQLCmd.ExecuteNonQuery()

                SQLCmd.CommandText = "UPDATE tblBooking " &
                                     "SET BookingStatus = @BookingStatus, CancelledDate = Now() " &
                                     "WHERE BookingID = @BookingID AND BookingStatus <> @AlreadyCancelled"
                SQLCmd.Parameters.Clear()
                SQLCmd.Parameters.AddWithValue("@BookingStatus", BookingCancelled)
                SQLCmd.Parameters.AddWithValue("@BookingID", cancelledID)
                SQLCmd.Parameters.AddWithValue("@AlreadyCancelled", BookingCancelled)

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

        If worked Then
            WriteLog("BOOKING", "Booking " & cancelledID & " cancelled (" & cancelledWhat & "), " & seatsFreed & " seat(s) freed", LogChange)
            MessageBox.Show("Booking cancelled and " & seatsFreed & " seat(s) put back on sale." & vbCrLf &
                            "The booking is still in the list, marked as cancelled.",
                            "Cancel Booking", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If

        LoadBookings(txtSearch.Text.Trim())
    End Sub

    Private Sub btnLoadDoorList_Click(sender As Object, e As EventArgs) Handles btnLoadDoorList.Click
        If cboDoorListScreening.SelectedIndex = -1 Then
            MessageBox.Show("Pick a screening first", "Booking Search", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Dim dt As New DataTable

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
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

        If dt.Rows.Count = 0 Then
            GroupBox2.Text = "Door list - nobody has booked this one yet"
        Else
            GroupBox2.Text = "Door list - " & dt.Rows.Count & " seat(s) booked"
        End If

        WriteLog("BOOKING", "Door list loaded for ScreeningID " & cboDoorListScreening.SelectedValue.ToString())
    End Sub

    Private Sub btnExportDoorList_Click(sender As Object, e As EventArgs) Handles btnExportDoorList.Click
        If dgvDoorList.Rows.Count = 0 Then
            MessageBox.Show("Load a door list first", "Booking Search", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Dim saveDialog As New SaveFileDialog
        saveDialog.Filter = "CSV files (*.csv)|*.csv"
        saveDialog.FileName = "DoorList.csv"

        If saveDialog.ShowDialog() = DialogResult.OK Then
            Dim writer As New System.IO.StreamWriter(saveDialog.FileName)

            writer.WriteLine("Booking,Customer Name,Seat")

            For Each row As DataGridViewRow In dgvDoorList.Rows
                writer.WriteLine(CsvField(row.Cells("BookingID").Value.ToString()) & "," &
                                 CsvField(row.Cells("CustomerName").Value.ToString()) & "," &
                                 CsvField(row.Cells("SeatNumber").Value.ToString()))
            Next

            writer.Close()

            WriteLog("BOOKING", "Door list exported for ScreeningID " & cboDoorListScreening.SelectedValue.ToString())
            MessageBox.Show("Door list exported", "Booking Search", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

End Class
