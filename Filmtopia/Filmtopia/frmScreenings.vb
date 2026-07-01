Imports System.Data.OleDb

Public Class frmScreenings

    'tracks the ScreeningID of the row currently selected in the grid, 0 means nothing selected
    Private selectedScreeningID As Integer = 0

    Private Sub frmScreenings_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CommonFormStartup()
        LoadFilmsCombo()
        LoadScreensCombo()
        LoadScreenings()
        WriteLog("SCREENING", "Screenings form opened")
    End Sub

    'fills the film combo with every film title
    Private Sub LoadFilmsCombo()
        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT FilmID, FilmTitle " &
                                 "FROM tblFilm"
            Dim da As New OleDbDataAdapter(SQLCmd)
            Dim dt As New DataTable
            da.Fill(dt)
            cboFilm.DataSource = dt
            cboFilm.DisplayMember = "FilmTitle"
            cboFilm.ValueMember = "FilmID"
            cboFilm.SelectedIndex = -1
            cn.Close()
        End If
    End Sub

    'fills the screen combo with every screen name
    Private Sub LoadScreensCombo()
        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT ScreenID, ScreenName " &
                                 "FROM tblScreen"
            Dim da As New OleDbDataAdapter(SQLCmd)
            Dim dt As New DataTable
            da.Fill(dt)
            cboScreen.DataSource = dt
            cboScreen.DisplayMember = "ScreenName"
            cboScreen.ValueMember = "ScreenID"
            cboScreen.SelectedIndex = -1
            cn.Close()
        End If
    End Sub

    'loads all screenings, joined with film and screen names, into the grid
    Private Sub LoadScreenings()
        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            'join screening to film (for the title) and to screen (for the name)
            SQLCmd.CommandText = "SELECT tblScreening.ScreeningID, FilmTitle, ScreenName, ScreeningDate, ScreeningTime, TicketPrice, tblScreening.FilmID, tblScreening.ScreenID " &
                                 "FROM (tblScreening INNER JOIN tblFilm ON tblScreening.FilmID = tblFilm.FilmID) " &
                                 "INNER JOIN tblScreen ON tblScreening.ScreenID = tblScreen.ScreenID"
            Dim da As New OleDbDataAdapter(SQLCmd)
            Dim dt As New DataTable
            da.Fill(dt)
            dgvScreenings.DataSource = dt
            cn.Close()
        End If

        'hide the raw FilmID and ScreenID columns, the names are shown instead
        dgvScreenings.Columns("FilmID").Visible = False
        dgvScreenings.Columns("ScreenID").Visible = False

        'let the film title column stretch out and wrap so its all readable
        dgvScreenings.Columns("FilmTitle").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        dgvScreenings.DefaultCellStyle.WrapMode = DataGridViewTriState.True
        dgvScreenings.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells

        WriteLog("SCREENING", "Screening list loaded")
    End Sub

    'checks the screening time is in HH:MM format, e.g. 14:30
    Private Function IsValidScreeningTime(timeText As String) As Boolean
        If timeText.Length <> 5 Then
            Return False
        End If
        If timeText.Chars(2) <> ":" Then
            Return False
        End If

        Dim hourPart As String = timeText.Substring(0, 2)
        Dim minutePart As String = timeText.Substring(3, 2)

        If Not IsNumeric(hourPart) Or Not IsNumeric(minutePart) Then
            Return False
        End If

        Dim hour As Integer = CInt(hourPart)
        Dim minute As Integer = CInt(minutePart)

        If hour < 0 Or hour > 23 Or minute < 0 Or minute > 59 Then
            Return False
        End If

        Return True
    End Function

    'adds a new screening using the values picked and typed in
    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        If cboFilm.SelectedIndex = -1 Or cboScreen.SelectedIndex = -1 Then
            MessageBox.Show("Pick a film and a screen")
            Exit Sub
        End If
        If dtpScreeningDate.Value.Date < Date.Now.Date Then
            MessageBox.Show("Screening date cant be in the past")
            Exit Sub
        End If
        If txtScreeningTime.Text = "" Then
            MessageBox.Show("Enter a screening time (HH:MM)")
            Exit Sub
        End If
        If Not IsValidScreeningTime(txtScreeningTime.Text) Then
            MessageBox.Show("Screening time must be in HH:MM format, e.g. 14:30")
            Exit Sub
        End If
        If txtTicketPrice.Text = "" Then
            MessageBox.Show("Enter a ticket price")
            Exit Sub
        End If
        If Not IsNumeric(txtTicketPrice.Text) Then
            MessageBox.Show("Ticket price must be a number")
            Exit Sub
        End If
        If Val(txtTicketPrice.Text) <= 0 Then
            MessageBox.Show("Ticket price must be greater than 0")
            Exit Sub
        End If

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "INSERT INTO tblScreening (FilmID, ScreenID, ScreeningDate, ScreeningTime, TicketPrice) " &
                                 "VALUES (@FilmID, @ScreenID, @ScreeningDate, @ScreeningTime, @TicketPrice)"
            SQLCmd.Parameters.AddWithValue("@FilmID", CInt(cboFilm.SelectedValue))
            SQLCmd.Parameters.AddWithValue("@ScreenID", CInt(cboScreen.SelectedValue))
            SQLCmd.Parameters.AddWithValue("@ScreeningDate", dtpScreeningDate.Value.Date)
            SQLCmd.Parameters.AddWithValue("@ScreeningTime", txtScreeningTime.Text)
            SQLCmd.Parameters.AddWithValue("@TicketPrice", Val(txtTicketPrice.Text))
            SQLCmd.ExecuteNonQuery()
            cn.Close()
        End If

        WriteLog("SCREENING", "Screening added: " & cboFilm.Text & " on " & cboScreen.Text)
        LoadScreenings()
        ClearFields()
    End Sub

    'updates the currently selected screening with the values picked and typed in
    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        If selectedScreeningID = 0 Then
            MessageBox.Show("Select a screening in the grid first")
            Exit Sub
        End If
        If cboFilm.SelectedIndex = -1 Or cboScreen.SelectedIndex = -1 Then
            MessageBox.Show("Pick a film and a screen")
            Exit Sub
        End If
        If txtScreeningTime.Text = "" Then
            MessageBox.Show("Enter a screening time (HH:MM)")
            Exit Sub
        End If
        If Not IsValidScreeningTime(txtScreeningTime.Text) Then
            MessageBox.Show("Screening time must be in HH:MM format, e.g. 14:30")
            Exit Sub
        End If
        If txtTicketPrice.Text = "" Then
            MessageBox.Show("Enter a ticket price")
            Exit Sub
        End If
        If Not IsNumeric(txtTicketPrice.Text) Then
            MessageBox.Show("Ticket price must be a number")
            Exit Sub
        End If
        If Val(txtTicketPrice.Text) <= 0 Then
            MessageBox.Show("Ticket price must be greater than 0")
            Exit Sub
        End If

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "UPDATE tblScreening " &
                                 "SET FilmID = @FilmID, ScreenID = @ScreenID, ScreeningDate = @ScreeningDate, ScreeningTime = @ScreeningTime, TicketPrice = @TicketPrice " &
                                 "WHERE ScreeningID = @ScreeningID"
            SQLCmd.Parameters.AddWithValue("@FilmID", CInt(cboFilm.SelectedValue))
            SQLCmd.Parameters.AddWithValue("@ScreenID", CInt(cboScreen.SelectedValue))
            SQLCmd.Parameters.AddWithValue("@ScreeningDate", dtpScreeningDate.Value.Date)
            SQLCmd.Parameters.AddWithValue("@ScreeningTime", txtScreeningTime.Text)
            SQLCmd.Parameters.AddWithValue("@TicketPrice", Val(txtTicketPrice.Text))
            SQLCmd.Parameters.AddWithValue("@ScreeningID", selectedScreeningID)
            SQLCmd.ExecuteNonQuery()
            cn.Close()
        End If

        WriteLog("SCREENING", "Screening updated: " & cboFilm.Text & " on " & cboScreen.Text)
        LoadScreenings()
        ClearFields()
    End Sub

    'deletes the currently selected screening
    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If selectedScreeningID = 0 Then
            MessageBox.Show("Select a screening in the grid first")
            Exit Sub
        End If

        If MessageBox.Show("Delete this screening?", "Confirm", MessageBoxButtons.YesNo) = DialogResult.No Then
            Exit Sub
        End If

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "DELETE FROM tblScreening " &
                                 "WHERE ScreeningID = @ScreeningID"
            SQLCmd.Parameters.AddWithValue("@ScreeningID", selectedScreeningID)
            SQLCmd.ExecuteNonQuery()
            cn.Close()
        End If

        WriteLog("SCREENING", "Screening deleted: ScreeningID " & selectedScreeningID)
        LoadScreenings()
        ClearFields()
    End Sub

    'clears the fields and the selection
    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        ClearFields()
        WriteLog("SCREENING", "Screening fields cleared")
    End Sub

    Private Sub ClearFields()
        selectedScreeningID = 0
        cboFilm.SelectedIndex = -1
        cboScreen.SelectedIndex = -1
        dtpScreeningDate.Value = Date.Now
        txtScreeningTime.Text = ""
        txtTicketPrice.Text = ""
        dgvScreenings.ClearSelection()
    End Sub

    'when a row is clicked, load its values into the fields for editing
    Private Sub dgvScreenings_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvScreenings.CellClick
        If e.RowIndex < 0 Then Exit Sub

        Dim row As DataGridViewRow = dgvScreenings.Rows(e.RowIndex)
        selectedScreeningID = CInt(row.Cells("ScreeningID").Value)
        cboFilm.SelectedValue = CInt(row.Cells("FilmID").Value)
        cboScreen.SelectedValue = CInt(row.Cells("ScreenID").Value)
        dtpScreeningDate.Value = CDate(row.Cells("ScreeningDate").Value)
        txtScreeningTime.Text = row.Cells("ScreeningTime").Value.ToString()
        txtTicketPrice.Text = row.Cells("TicketPrice").Value.ToString()
        WriteLog("SCREENING", "Screening selected: " & cboFilm.Text & " on " & cboScreen.Text)
    End Sub

End Class
