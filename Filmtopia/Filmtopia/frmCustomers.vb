Imports System.Data.OleDb

Public Class frmCustomers

    Private selectedCustomerID As Long = 0

    Private stillLoading As Boolean = True

    Private boxesChanged As Boolean = False

    Private fillingBoxes As Boolean = False

    Private Sub frmCustomers_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CommonFormStartup(Me)

        stillLoading = False

        Me.KeyPreview = True

        LoadCustomers()
        ClearFields()

        txtSearch.Focus()
        WriteLog("CUSTOMER", "Customers form opened")
    End Sub

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        If ExportGridToCsv(dgvCustomers, "Customers.csv", "Customers") Then
            WriteLog("CUSTOMER", "Customer list exported, " & dgvCustomers.Rows.Count & " customers", LogSecurity)
        End If
    End Sub

    Private Sub frmCustomers_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.F5 Then
            LoadCustomers()
        ElseIf e.KeyCode = Keys.Escape Then
            If txtSearch.Text <> "" Then
                txtSearch.Text = ""
            Else
                Me.Close()
            End If
        End If
    End Sub

    Private Function IsDigitsOnly(phoneText As String) As Boolean
        For Each ch As Char In phoneText
            If Not Char.IsDigit(ch) Then
                Return False
            End If
        Next
        Return True
    End Function

    Private Sub LoadCustomers()
        Dim dt As New DataTable

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn

            Dim baseQuery As String = "SELECT tblCustomer.CustomerID, CustomerForename, CustomerSurname, CustomerEmail, CustomerPhone, " &
                                      "COUNT(tblBooking.BookingID) AS Bookings " &
                                      "FROM tblCustomer LEFT JOIN tblBooking ON tblCustomer.CustomerID = tblBooking.CustomerID"

            Dim grouping As String = " GROUP BY tblCustomer.CustomerID, CustomerForename, CustomerSurname, CustomerEmail, CustomerPhone " &
                                     "ORDER BY CustomerSurname, CustomerForename, tblCustomer.CustomerID"

            If txtSearch.Text.Trim() = "" Then
                SQLCmd.CommandText = baseQuery & grouping
            Else
                SQLCmd.CommandText = baseQuery &
                                     " WHERE CustomerForename & ' ' & CustomerSurname LIKE @SearchName " &
                                     "OR CustomerEmail LIKE @SearchEmail " &
                                     "OR CustomerPhone LIKE @SearchPhone" & grouping
                SQLCmd.Parameters.AddWithValue("@SearchName", "%" & txtSearch.Text.Trim() & "%")
                SQLCmd.Parameters.AddWithValue("@SearchEmail", "%" & txtSearch.Text.Trim() & "%")
                SQLCmd.Parameters.AddWithValue("@SearchPhone", "%" & txtSearch.Text.Trim() & "%")
            End If

            Dim da As New OleDbDataAdapter(SQLCmd)
            da.Fill(dt)
            cn.Close()
        End If

        dgvCustomers.DataSource = dt

        If dgvCustomers.Columns.Count > 0 Then
            dgvCustomers.Columns("CustomerID").HeaderText = "ID"
            dgvCustomers.Columns("CustomerForename").HeaderText = "Forename"
            dgvCustomers.Columns("CustomerSurname").HeaderText = "Surname"
            dgvCustomers.Columns("CustomerEmail").HeaderText = "Email"
            dgvCustomers.Columns("CustomerPhone").HeaderText = "Phone"
            dgvCustomers.Columns("Bookings").HeaderText = "Bookings"

            dgvCustomers.Columns("CustomerID").Width = 50
            dgvCustomers.Columns("CustomerForename").Width = 160
            dgvCustomers.Columns("CustomerSurname").Width = 160
            dgvCustomers.Columns("CustomerEmail").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            dgvCustomers.Columns("CustomerPhone").Width = 130
            dgvCustomers.Columns("Bookings").Width = 90

            dgvCustomers.Columns("Bookings").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        End If

        ShowCount(dt.Rows.Count)
        dgvCustomers.ClearSelection()
    End Sub

    Private Sub ShowCount(shown As Integer)
        If txtSearch.Text.Trim() = "" Then
            If shown = 1 Then
                lblGridCount.Text = "1 customer"
            Else
                lblGridCount.Text = shown & " customers"
            End If
        ElseIf shown = 0 Then
            lblGridCount.Text = "Nobody matches '" & txtSearch.Text.Trim() & "'"
        Else
            lblGridCount.Text = shown & " match(es) for '" & txtSearch.Text.Trim() & "'"
        End If
    End Sub

    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        If stillLoading Then
            Exit Sub
        End If

        timerSearch.Stop()
        timerSearch.Start()
    End Sub

    Private Sub timerSearch_Tick(sender As Object, e As EventArgs) Handles timerSearch.Tick
        timerSearch.Stop()
        LoadCustomers()
    End Sub

    Private Function DetailsAreOk() As Boolean
        Dim forename As String = txtForename.Text.Trim()
        Dim surname As String = txtSurname.Text.Trim()
        Dim email As String = txtEmail.Text.Trim()
        Dim phone As String = txtPhone.Text.Trim()

        If forename = "" Then
            MessageBox.Show("Enter a forename", "Customers", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtForename.Focus()
            Return False
        End If

        If surname = "" Then
            MessageBox.Show("Enter a surname", "Customers", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtSurname.Focus()
            Return False
        End If

        If email = "" Then
            MessageBox.Show("Enter an email address", "Customers", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtEmail.Focus()
            Return False
        End If

        Dim atPos As Integer = email.IndexOf("@")
        Dim dotPos As Integer = email.LastIndexOf(".")

        If atPos < 1 Or dotPos < atPos + 2 Or dotPos = email.Length - 1 Then
            MessageBox.Show("Enter a valid email address, like name@example.com", "Customers", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtEmail.Focus()
            Return False
        End If

        If phone = "" Then
            MessageBox.Show("Enter a phone number", "Customers", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtPhone.Focus()
            Return False
        End If

        If phone.Length < 10 Or phone.Length > 11 Then
            MessageBox.Show("Phone number must be 10 or 11 digits long", "Customers", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtPhone.Focus()
            Return False
        End If

        If Not IsDigitsOnly(phone) Then
            MessageBox.Show("Phone number must contain digits only", "Customers", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtPhone.Focus()
            Return False
        End If

        Return True
    End Function

    Private Function SameNameCount() As Integer
        Dim total As Integer = 0

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT COUNT(*) FROM tblCustomer " &
                                 "WHERE CustomerForename = @CustomerForename AND CustomerSurname = @CustomerSurname " &
                                 "AND CustomerID <> @CustomerID"
            SQLCmd.Parameters.AddWithValue("@CustomerForename", txtForename.Text.Trim())
            SQLCmd.Parameters.AddWithValue("@CustomerSurname", txtSurname.Text.Trim())
            SQLCmd.Parameters.AddWithValue("@CustomerID", CInt(selectedCustomerID))
            total = CInt(SQLCmd.ExecuteScalar())
            cn.Close()
        End If

        Return total
    End Function

    Private Function DuplicateNameIsOk() As Boolean
        If SameNameCount() = 0 Then
            Return True
        End If

        Dim fullName As String = txtForename.Text.Trim() & " " & txtSurname.Text.Trim()

        Return MessageBox.Show("There is already somebody called " & fullName & " on the system." & vbCrLf &
                               "Two people can genuinely have the same name, so this is only a warning." & vbCrLf & vbCrLf &
                               "Save this one as well?",
                               "Name already used", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes
    End Function

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        If Not DetailsAreOk() Then
            Exit Sub
        End If

        If Not DuplicateNameIsOk() Then
            Exit Sub
        End If

        Dim saved As Boolean = False

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "INSERT INTO tblCustomer (CustomerForename, CustomerSurname, CustomerEmail, CustomerPhone) " &
                                 "VALUES (@CustomerForename, @CustomerSurname, @CustomerEmail, @CustomerPhone)"
            SQLCmd.Parameters.AddWithValue("@CustomerForename", txtForename.Text.Trim())
            SQLCmd.Parameters.AddWithValue("@CustomerSurname", txtSurname.Text.Trim())
            SQLCmd.Parameters.AddWithValue("@CustomerEmail", txtEmail.Text.Trim())
            SQLCmd.Parameters.AddWithValue("@CustomerPhone", txtPhone.Text.Trim())
            SQLCmd.ExecuteNonQuery()
            cn.Close()
            saved = True
        End If

        If Not saved Then
            Exit Sub
        End If

        Dim savedName As String = txtForename.Text.Trim() & " " & txtSurname.Text.Trim()
        WriteLog("CUSTOMER", "Customer added: " & txtForename.Text.Trim() & " " & txtSurname.Text.Trim(), LogChange)
        LoadCustomers()
        ClearFields()
        SayDone(lblSaved, "Added '" & savedName & "'")
    End Sub

    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        If selectedCustomerID = 0 Then
            MessageBox.Show("Select a customer in the grid first", "Customers", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If Not DetailsAreOk() Then
            Exit Sub
        End If

        If Not DuplicateNameIsOk() Then
            Exit Sub
        End If

        Dim saved As Boolean = False

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "UPDATE tblCustomer " &
                                 "SET CustomerForename = @CustomerForename, CustomerSurname = @CustomerSurname, CustomerEmail = @CustomerEmail, CustomerPhone = @CustomerPhone " &
                                 "WHERE CustomerID = @CustomerID"
            SQLCmd.Parameters.AddWithValue("@CustomerForename", txtForename.Text.Trim())
            SQLCmd.Parameters.AddWithValue("@CustomerSurname", txtSurname.Text.Trim())
            SQLCmd.Parameters.AddWithValue("@CustomerEmail", txtEmail.Text.Trim())
            SQLCmd.Parameters.AddWithValue("@CustomerPhone", txtPhone.Text.Trim())
            SQLCmd.Parameters.AddWithValue("@CustomerID", CInt(selectedCustomerID))
            SQLCmd.ExecuteNonQuery()
            cn.Close()
            saved = True
        End If

        If Not saved Then
            Exit Sub
        End If

        Dim savedName As String = txtForename.Text.Trim() & " " & txtSurname.Text.Trim()
        WriteLog("CUSTOMER", "Customer updated: " & txtForename.Text.Trim() & " " & txtSurname.Text.Trim(), LogChange)
        LoadCustomers()
        ClearFields()
        SayDone(lblSaved, "Saved changes to '" & savedName & "'")
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If selectedCustomerID = 0 Then
            MessageBox.Show("Select a customer in the grid first", "Customers", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Dim bookings As Integer = BookingsFor(selectedCustomerID)

        If bookings > 0 Then
            MessageBox.Show(txtForename.Text & " " & txtSurname.Text & " has " & bookings & " booking(s) on the system." & vbCrLf & vbCrLf &
                            "A booking is kept even after it is cancelled, so that the sale stays in the takings " &
                            "and the refund is on record. That means somebody who has ever booked cannot be " &
                            "removed, and cancelling their bookings will not change that.",
                            "Cannot delete", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            WriteLog("CUSTOMER", "Delete refused for " & txtForename.Text & " " & txtSurname.Text & ", they have " & bookings & " booking(s)", LogWarning)
            Exit Sub
        End If

        If MessageBox.Show("Delete " & txtForename.Text & " " & txtSurname.Text & "?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = DialogResult.No Then
            Exit Sub
        End If

        Dim saved As Boolean = False

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "DELETE FROM tblCustomer " &
                                 "WHERE CustomerID = @CustomerID"
            SQLCmd.Parameters.AddWithValue("@CustomerID", CInt(selectedCustomerID))
            SQLCmd.ExecuteNonQuery()
            cn.Close()
            saved = True
        End If

        If Not saved Then
            Exit Sub
        End If

        Dim savedName As String = txtForename.Text.Trim() & " " & txtSurname.Text.Trim()
        WriteLog("CUSTOMER", "Customer deleted: " & txtForename.Text & " " & txtSurname.Text, LogChange)
        LoadCustomers()
        ClearFields()
        SayDone(lblSaved, "Deleted '" & savedName & "'")
    End Sub

    Private Function BookingsFor(customerID As Long) As Integer
        Dim total As Integer = 0

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT COUNT(*) FROM tblBooking WHERE CustomerID = @CustomerID"
            SQLCmd.Parameters.AddWithValue("@CustomerID", CInt(customerID))
            total = CInt(SQLCmd.ExecuteScalar())
            cn.Close()
        End If

        Return total
    End Function

    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        ClearFields()
    End Sub

    Private Sub Details_Changed(sender As Object, e As EventArgs) Handles txtForename.TextChanged, txtSurname.TextChanged,
        txtEmail.TextChanged, txtPhone.TextChanged
        If fillingBoxes Then
            Exit Sub
        End If

        boxesChanged = True
    End Sub

    Private Function ChangesCanBeLost() As Boolean
        If Not boxesChanged Then
            Return True
        End If

        Return MessageBox.Show("There are changes in the boxes that have not been saved." & vbCrLf &
                               "Throw them away?", "Unsaved changes",
                               MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                               MessageBoxDefaultButton.Button2) = DialogResult.Yes
    End Function

    Private Sub ClearFields()
        fillingBoxes = True

        lblSaved.Text = ""
        selectedCustomerID = 0
        txtForename.Text = ""
        txtSurname.Text = ""
        txtEmail.Text = ""
        txtPhone.Text = ""
        fillingBoxes = False
        boxesChanged = False

        dgvCustomers.ClearSelection()
        ShowWhatIsBeingEdited()
    End Sub

    Private Sub ShowWhatIsBeingEdited()
        If selectedCustomerID = 0 Then
            lblStatus.Text = "Adding a new customer"
            btnUpdate.Enabled = False
            btnDelete.Enabled = False
            btnAdd.Enabled = True
        Else
            lblStatus.Text = "Editing: " & txtForename.Text & " " & txtSurname.Text
            btnUpdate.Enabled = True
            btnDelete.Enabled = True
            btnAdd.Enabled = False
        End If
    End Sub

    Private Sub dgvCustomers_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvCustomers.CellClick
        If e.RowIndex < 0 Then Exit Sub

        If Not ChangesCanBeLost() Then
            Exit Sub
        End If

        fillingBoxes = True

        Dim row As DataGridViewRow = dgvCustomers.Rows(e.RowIndex)
        selectedCustomerID = CLng(row.Cells("CustomerID").Value)
        txtForename.Text = row.Cells("CustomerForename").Value.ToString()
        txtSurname.Text = row.Cells("CustomerSurname").Value.ToString()
        txtEmail.Text = row.Cells("CustomerEmail").Value.ToString()
        txtPhone.Text = row.Cells("CustomerPhone").Value.ToString()

        fillingBoxes = False
        boxesChanged = False

        ShowWhatIsBeingEdited()
    End Sub

End Class
