Imports System.Data.OleDb

Public Class frmCustomers

    'tracks the CustomerID of the row currently selected in the grid, 0 means nothing selected
    Private selectedCustomerID As Long = 0

    Private Sub frmCustomers_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CommonFormStartup()
        LoadCustomers()
        WriteLog("CUSTOMER", "Customers form opened")
    End Sub

    'loads all customers from tblCustomer into the grid
    Private Sub LoadCustomers()
        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT CustomerID, CustomerForename, CustomerSurname, CustomerEmail, CustomerPhone " &
                                 "FROM tblCustomer"
            Dim da As New OleDbDataAdapter(SQLCmd)
            Dim dt As New DataTable
            da.Fill(dt)
            dgvCustomers.DataSource = dt
            cn.Close()
        End If

        'let the email column stretch out and wrap so its all readable
        dgvCustomers.Columns("CustomerEmail").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        dgvCustomers.DefaultCellStyle.WrapMode = DataGridViewTriState.True
        dgvCustomers.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells

        WriteLog("CUSTOMER", "Customer list loaded")
    End Sub

    'adds a new customer using the values typed into the textboxes
    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        If txtForename.Text = "" Or txtSurname.Text = "" Then
            MessageBox.Show("Enter a forename and surname")
            Exit Sub
        End If

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "INSERT INTO tblCustomer (CustomerForename, CustomerSurname, CustomerEmail, CustomerPhone) " &
                                 "VALUES (@CustomerForename, @CustomerSurname, @CustomerEmail, @CustomerPhone)"
            SQLCmd.Parameters.AddWithValue("@CustomerForename", txtForename.Text)
            SQLCmd.Parameters.AddWithValue("@CustomerSurname", txtSurname.Text)
            SQLCmd.Parameters.AddWithValue("@CustomerEmail", txtEmail.Text)
            SQLCmd.Parameters.AddWithValue("@CustomerPhone", txtPhone.Text)
            SQLCmd.ExecuteNonQuery()
            cn.Close()
        End If

        WriteLog("CUSTOMER", "Customer added: " & txtForename.Text & " " & txtSurname.Text)
        LoadCustomers()
        ClearFields()
    End Sub

    'updates the currently selected customer with the values in the textboxes
    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        If selectedCustomerID = 0 Then
            MessageBox.Show("Select a customer in the grid first")
            Exit Sub
        End If

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "UPDATE tblCustomer " &
                                 "SET CustomerForename = @CustomerForename, CustomerSurname = @CustomerSurname, CustomerEmail = @CustomerEmail, CustomerPhone = @CustomerPhone " &
                                 "WHERE CustomerID = @CustomerID"
            SQLCmd.Parameters.AddWithValue("@CustomerForename", txtForename.Text)
            SQLCmd.Parameters.AddWithValue("@CustomerSurname", txtSurname.Text)
            SQLCmd.Parameters.AddWithValue("@CustomerEmail", txtEmail.Text)
            SQLCmd.Parameters.AddWithValue("@CustomerPhone", txtPhone.Text)
            SQLCmd.Parameters.AddWithValue("@CustomerID", CInt(selectedCustomerID))
            SQLCmd.ExecuteNonQuery()
            cn.Close()
        End If

        WriteLog("CUSTOMER", "Customer updated: " & txtForename.Text & " " & txtSurname.Text)
        LoadCustomers()
        ClearFields()
    End Sub

    'deletes the currently selected customer
    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If selectedCustomerID = 0 Then
            MessageBox.Show("Select a customer in the grid first")
            Exit Sub
        End If

        If MessageBox.Show("Delete this customer?", "Confirm", MessageBoxButtons.YesNo) = DialogResult.No Then
            Exit Sub
        End If

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "DELETE FROM tblCustomer " &
                                 "WHERE CustomerID = @CustomerID"
            SQLCmd.Parameters.AddWithValue("@CustomerID", CInt(selectedCustomerID))
            SQLCmd.ExecuteNonQuery()
            cn.Close()
        End If

        WriteLog("CUSTOMER", "Customer deleted: " & txtForename.Text & " " & txtSurname.Text)
        LoadCustomers()
        ClearFields()
    End Sub

    'clears the textboxes and the selection
    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        ClearFields()
        WriteLog("CUSTOMER", "Customer fields cleared")
    End Sub

    Private Sub ClearFields()
        selectedCustomerID = 0
        txtForename.Text = ""
        txtSurname.Text = ""
        txtEmail.Text = ""
        txtPhone.Text = ""
        dgvCustomers.ClearSelection()
    End Sub

    'when a row is clicked, load its values into the textboxes for editing
    Private Sub dgvCustomers_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvCustomers.CellClick
        If e.RowIndex < 0 Then Exit Sub

        Dim row As DataGridViewRow = dgvCustomers.Rows(e.RowIndex)
        selectedCustomerID = CLng(row.Cells("CustomerID").Value)
        txtForename.Text = row.Cells("CustomerForename").Value.ToString()
        txtSurname.Text = row.Cells("CustomerSurname").Value.ToString()
        txtEmail.Text = row.Cells("CustomerEmail").Value.ToString()
        txtPhone.Text = row.Cells("CustomerPhone").Value.ToString()
        WriteLog("CUSTOMER", "Customer selected: " & txtForename.Text & " " & txtSurname.Text)
    End Sub

End Class
