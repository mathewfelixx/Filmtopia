Imports System.Data.OleDb

Public Class frmCustomers

    'tracks the CustomerID of the row currently selected in the grid, 0 means nothing selected
    Private selectedCustomerID As Long = 0

    'true while the form is setting itself up, so filling the search box does not load the grid
    'before everything is ready
    Private stillLoading As Boolean = True

    Private Sub frmCustomers_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CommonFormStartup(Me)

        stillLoading = False

        'lets the form see escape before the box that has focus does
        Me.KeyPreview = True

        LoadCustomers()
        ClearFields()

        'looking somebody up is the usual reason for opening this, so the cursor starts in the search
        txtSearch.Focus()
        WriteLog("CUSTOMER", "Customers form opened")
    End Sub

    'saves the customer list as it is on screen. this one is worth logging as a security entry,
    'somebody taking peoples names and phone numbers out of the system should leave a trace
    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        If ExportGridToCsv(dgvCustomers, "Customers.csv", "Customers") Then
            WriteLog("CUSTOMER", "Customer list exported, " & dgvCustomers.Rows.Count & " customers", LogSecurity)
        End If
    End Sub

    'escape empties the search box, or shuts the form if there is nothing to empty
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

    'checks a phone number is made up of digits only, no spaces or dashes
    Private Function IsDigitsOnly(phoneText As String) As Boolean
        For Each ch As Char In phoneText
            If Not Char.IsDigit(ch) Then
                Return False
            End If
        Next
        Return True
    End Function

    'loads the customers into the grid, only the ones matching the search box if anything is
    'typed in it. how many bookings each person has made is counted at the same time
    Private Sub LoadCustomers()
        Dim dt As New DataTable

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn

            'a LEFT JOIN is used rather than an ordinary one so that somebody who has never booked
            'anything still appears in the list, with a count of nothing next to them
            Dim baseQuery As String = "SELECT tblCustomer.CustomerID, CustomerForename, CustomerSurname, CustomerEmail, CustomerPhone, " &
                                      "COUNT(tblBooking.BookingID) AS Bookings " &
                                      "FROM tblCustomer LEFT JOIN tblBooking ON tblCustomer.CustomerID = tblBooking.CustomerID"

            'the id on the end of the ORDER BY has to say which table it comes from. tblBooking has
            'a CustomerID on it as well, that is what the join is on, so an unqualified one leaves
            'Access with two columns of that name to choose between and it refuses the whole query
            Dim grouping As String = " GROUP BY tblCustomer.CustomerID, CustomerForename, CustomerSurname, CustomerEmail, CustomerPhone " &
                                     "ORDER BY CustomerSurname, CustomerForename, tblCustomer.CustomerID"

            If txtSearch.Text.Trim() = "" Then
                SQLCmd.CommandText = baseQuery & grouping
            Else
                'the name, the email and the phone number are all searched, because whoever is on
                'the desk might only have one of the three to go on
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

    'says how many customers are showing, and whether the search is hiding any
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

    'the list narrows as it is typed in, there is no need for a search button
    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        If stillLoading Then
            Exit Sub
        End If

        LoadCustomers()
    End Sub

    'checks what has been typed in before it goes anywhere near the database. it is in one place
    'because adding a customer and changing one both need exactly the same checks doing, and they
    'were written out twice before which meant remembering to change both of them
    Private Function DetailsAreOk() As Boolean
        If txtForename.Text.Trim() = "" Then
            MessageBox.Show("Enter a forename", "Customers", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtForename.Focus()
            Return False
        End If

        If txtSurname.Text.Trim() = "" Then
            MessageBox.Show("Enter a surname", "Customers", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtSurname.Focus()
            Return False
        End If

        If txtEmail.Text.Trim() = "" Then
            MessageBox.Show("Enter an email address", "Customers", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtEmail.Focus()
            Return False
        End If

        If Not txtEmail.Text.Contains("@") Or Not txtEmail.Text.Contains(".") Then
            MessageBox.Show("Enter a valid email address", "Customers", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtEmail.Focus()
            Return False
        End If

        If txtPhone.Text.Trim() = "" Then
            MessageBox.Show("Enter a phone number", "Customers", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtPhone.Focus()
            Return False
        End If

        If txtPhone.Text.Length < 10 Or txtPhone.Text.Length > 11 Then
            MessageBox.Show("Phone number must be 10 or 11 digits long", "Customers", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtPhone.Focus()
            Return False
        End If

        If Not IsDigitsOnly(txtPhone.Text) Then
            MessageBox.Show("Phone number must contain digits only", "Customers", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtPhone.Focus()
            Return False
        End If

        Return True
    End Function

    'adds a new customer using the values typed into the boxes
    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        If Not DetailsAreOk() Then
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

        'nothing was written if the database could not be opened, so it must not be
        'logged or announced as though it had been
        If Not saved Then
            Exit Sub
        End If

        Dim savedName As String = txtForename.Text.Trim() & " " & txtSurname.Text.Trim()
        WriteLog("CUSTOMER", "Customer added: " & txtForename.Text.Trim() & " " & txtSurname.Text.Trim(), LogChange)
        LoadCustomers()
        ClearFields()
        SayDone(lblSaved, "Added '" & savedName & "'")
    End Sub

    'saves the changes made to the customer selected in the grid
    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        If selectedCustomerID = 0 Then
            MessageBox.Show("Select a customer in the grid first", "Customers", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If Not DetailsAreOk() Then
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

        'nothing was written if the database could not be opened, so it must not be
        'logged or announced as though it had been
        If Not saved Then
            Exit Sub
        End If

        Dim savedName As String = txtForename.Text.Trim() & " " & txtSurname.Text.Trim()
        WriteLog("CUSTOMER", "Customer updated: " & txtForename.Text.Trim() & " " & txtSurname.Text.Trim(), LogChange)
        LoadCustomers()
        ClearFields()
        SayDone(lblSaved, "Saved changes to '" & savedName & "'")
    End Sub

    'deletes the customer selected in the grid
    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If selectedCustomerID = 0 Then
            MessageBox.Show("Select a customer in the grid first", "Customers", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        'somebody who has booked cannot just be removed, their bookings would be left pointing at
        'a customer who is not there and the booking list would show blanks where the name goes
        Dim bookings As Integer = BookingsFor(selectedCustomerID)

        If bookings > 0 Then
            MessageBox.Show(txtForename.Text & " " & txtSurname.Text & " has " & bookings & " booking(s)." & vbCrLf &
                            "Cancel those bookings first, then this customer can be removed.",
                            "Cannot delete", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            WriteLog("CUSTOMER", "Delete refused for " & txtForename.Text & " " & txtSurname.Text & ", they have " & bookings & " booking(s)", LogWarning)
            Exit Sub
        End If

        If MessageBox.Show("Delete " & txtForename.Text & " " & txtSurname.Text & "?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
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

        'nothing was written if the database could not be opened, so it must not be
        'logged or announced as though it had been
        If Not saved Then
            Exit Sub
        End If

        Dim savedName As String = txtForename.Text.Trim() & " " & txtSurname.Text.Trim()
        WriteLog("CUSTOMER", "Customer deleted: " & txtForename.Text & " " & txtSurname.Text, LogChange)
        LoadCustomers()
        ClearFields()
        SayDone(lblSaved, "Deleted '" & savedName & "'")
    End Sub

    'counts how many bookings somebody has, used to stop them being deleted while they have some
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

    'clears the boxes and the selection
    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        ClearFields()
    End Sub

    Private Sub ClearFields()
        'the confirmation only lasts until the next thing is started
        lblSaved.Text = ""
        selectedCustomerID = 0
        txtForename.Text = ""
        txtSurname.Text = ""
        txtEmail.Text = ""
        txtPhone.Text = ""
        dgvCustomers.ClearSelection()
        ShowWhatIsBeingEdited()
    End Sub

    'the heading over the boxes says whether a new customer is being typed in or an existing one
    'is being changed. save and delete are switched off until somebody is picked, rather than
    'letting them be pressed and then telling the user off with a message box
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

    'when a row is clicked, load its values into the boxes for editing
    Private Sub dgvCustomers_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvCustomers.CellClick
        If e.RowIndex < 0 Then Exit Sub

        Dim row As DataGridViewRow = dgvCustomers.Rows(e.RowIndex)
        selectedCustomerID = CLng(row.Cells("CustomerID").Value)
        txtForename.Text = row.Cells("CustomerForename").Value.ToString()
        txtSurname.Text = row.Cells("CustomerSurname").Value.ToString()
        txtEmail.Text = row.Cells("CustomerEmail").Value.ToString()
        txtPhone.Text = row.Cells("CustomerPhone").Value.ToString()

        ShowWhatIsBeingEdited()
    End Sub

End Class
