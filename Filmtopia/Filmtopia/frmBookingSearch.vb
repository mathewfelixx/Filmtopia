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
            SQLCmd.CommandText = "SELECT ScreeningID, FilmTitle & ' - ' & Format(ScreeningDate, 'dd/mm/yyyy') & ' ' & ScreeningTime AS Info " &
                                 "FROM tblScreening INNER JOIN tblFilm ON tblScreening.FilmID = tblFilm.FilmID " &
                                 "ORDER BY ScreeningDate DESC, ScreeningTime DESC"
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

            Dim baseQuery As String = "SELECT tblBooking.BookingID, CustomerForename & ' ' & CustomerSurname AS CustomerName, IIf(IsNull(tblFilm.FilmTitle), 'Counter sale', tblFilm.FilmTitle) AS FilmTitle, ScreeningDate, ScreeningTime, TotalCost, BookingStatus " &
                                      "FROM ((tblBooking LEFT JOIN tblCustomer ON tblBooking.CustomerID = tblCustomer.CustomerID) " &
                                      "LEFT JOIN tblScreening ON tblBooking.ScreeningID = tblScreening.ScreeningID) " &
                                      "LEFT JOIN tblFilm ON tblScreening.FilmID = tblFilm.FilmID"

            If searchText = "" Then
                SQLCmd.CommandText = baseQuery & " ORDER BY tblBooking.BookingID DESC"
            ElseIf IsNumeric(searchText) Then
                SQLCmd.CommandText = baseQuery & " WHERE tblBooking.BookingID = @SearchID " &
                                     "ORDER BY tblBooking.BookingID DESC"
                SQLCmd.Parameters.AddWithValue("@SearchID", SafeInt(searchText))
            Else
                SQLCmd.CommandText = baseQuery & " WHERE CustomerForename & ' ' & CustomerSurname LIKE @SearchName " &
                                     "OR tblFilm.FilmTitle LIKE @SearchFilm " &
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
        btnViewBooking.Enabled = False
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

        btnCancelBooking.Enabled = (Not isCancelled) AndAlso (UserAccessLevel = 1)
        btnViewBooking.Enabled = True
    End Sub

    Private Sub dgvBookings_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvBookings.CellDoubleClick
        If e.RowIndex < 0 Then Exit Sub

        OpenSelectedBooking()
    End Sub

    Private Sub btnViewBooking_Click(sender As Object, e As EventArgs) Handles btnViewBooking.Click
        OpenSelectedBooking()
    End Sub

    Private Sub OpenSelectedBooking()
        If selectedBookingID = 0 Then
            MessageBox.Show("Select a booking in the grid first", "Booking Search", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        frmRefund.currentBookingID = selectedBookingID
        frmRefund.ShowDialog()
        LoadBookings(txtSearch.Text.Trim())
    End Sub

    Private Sub btnCancelBooking_Click(sender As Object, e As EventArgs) Handles btnCancelBooking.Click
        If selectedBookingID = 0 Then
            MessageBox.Show("Select a booking in the grid first", "Booking Search", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If UserAccessLevel <> 1 Then
            MessageBox.Show("Only a manager can give money back.", "Cancel Booking", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If MessageBox.Show("Refund the whole of booking " & selectedBookingID & "?" & vbCrLf &
                           "Its seats go back on sale and everything not already refunded is paid back." & vbCrLf &
                           "To refund only part of it, or to write down a reason of your own, use Open booking instead.",
                           "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
            Exit Sub
        End If

        Dim cancelledID As Integer = selectedBookingID
        Dim cancelledWhat As String = selectedBookingText

        Dim refundID As Long = RefundWholeBooking(cancelledID, "Whole booking refunded from booking search", CurrentLoginID)

        If refundID > 0 Then
            MessageBox.Show("Booking " & cancelledID & " for " & cancelledWhat & " has been refunded in full." & vbCrLf &
                            "Its seats are back on sale and the refund is written down as refund " & refundID & ".",
                            "Cancel Booking", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            MessageBox.Show("There was nothing left to refund on that booking.", "Cancel Booking", MessageBoxButtons.OK, MessageBoxIcon.Warning)
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
            Dim writer As System.IO.StreamWriter

            Try
                writer = New System.IO.StreamWriter(saveDialog.FileName)
            Catch ex As Exception
                MessageBox.Show("That file could not be written to. If it is open in another program, close it and try again." & vbCrLf & vbCrLf & ex.Message,
                                "Booking Search", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End Try

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
