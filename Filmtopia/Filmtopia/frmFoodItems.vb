Imports System.Data.OleDb

Public Class frmFoodItems

    'tracks the FoodItemID of the row currently selected in the grid, 0 means nothing selected
    Private selectedFoodItemID As Long = 0

    'true while the form is setting itself up, so filling the category box does not load the grid
    'before everything is ready
    Private stillLoading As Boolean = True

    Private Sub frmFoodItems_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If UserAccessLevel <> 1 Then
            MessageBox.Show("Only a manager can change what is on the menu.", "Food Items", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            WriteLog("FOOD", "Food items screen refused, access level " & UserAccessLevel, LogSecurity)
            Me.Close()
            Exit Sub
        End If

        CommonFormStartup(Me)

        LoadCategories()

        stillLoading = False

        'lets the form see escape before the box that has focus does
        Me.KeyPreview = True

        LoadFoodItems()
        ClearFields()
        txtName.Focus()
        WriteLog("FOOD", "Food items form opened")
    End Sub

    'escape shuts the form, same as the close button on the ones that have one
    Private Sub frmFoodItems_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.F5 Then
            LoadFoodItems()
        ElseIf e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub

    'fills the two category boxes. the filter at the top is built from the categories that are
    'actually in use, so it can never offer one that nothing is in. the one at the bottom starts
    'with the usual counter categories but can still be typed in, in case a new one is wanted
    Private Sub LoadCategories()
        cboFilter.Items.Clear()
        cboFilter.Items.Add("All categories")

        cboCategory.Items.Clear()
        cboCategory.Items.Add("Snacks")
        cboCategory.Items.Add("Drinks")
        cboCategory.Items.Add("Sweets")
        cboCategory.Items.Add("Meals")

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT DISTINCT FoodItemCategory FROM tblFoodItem ORDER BY FoodItemCategory"
            Dim rs As OleDbDataReader = SQLCmd.ExecuteReader()
            While rs.Read()
                Dim category As String = rs("FoodItemCategory").ToString()

                If category <> "" Then
                    cboFilter.Items.Add(category)

                    'a category somebody has already used but that is not one of the usual ones
                    'still needs to be pickable at the bottom
                    If Not cboCategory.Items.Contains(category) Then
                        cboCategory.Items.Add(category)
                    End If
                End If
            End While
            rs.Close()
            cn.Close()
        End If

        cboFilter.SelectedIndex = 0
    End Sub

    'loads the menu into the grid, only the picked category if one has been chosen
    Private Sub LoadFoodItems()
        Dim dt As New DataTable

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn

            Dim baseQuery As String = "SELECT FoodItemID, FoodItemName, FoodItemCategory, FoodItemPrice " &
                                      "FROM tblFoodItem"

            If cboFilter.SelectedIndex <= 0 Then
                SQLCmd.CommandText = baseQuery & " ORDER BY FoodItemCategory, FoodItemName, FoodItemID"
            Else
                SQLCmd.CommandText = baseQuery & " WHERE FoodItemCategory = @Category " &
                                     "ORDER BY FoodItemName, FoodItemID"
                SQLCmd.Parameters.AddWithValue("@Category", cboFilter.Text)
            End If

            Dim da As New OleDbDataAdapter(SQLCmd)
            da.Fill(dt)
            cn.Close()
        End If

        dgvFoodItems.DataSource = dt

        If dgvFoodItems.Columns.Count > 0 Then
            dgvFoodItems.Columns("FoodItemID").HeaderText = "ID"
            dgvFoodItems.Columns("FoodItemName").HeaderText = "Item"
            dgvFoodItems.Columns("FoodItemCategory").HeaderText = "Category"
            dgvFoodItems.Columns("FoodItemPrice").HeaderText = "Price"

            dgvFoodItems.Columns("FoodItemID").Width = 50
            dgvFoodItems.Columns("FoodItemName").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            dgvFoodItems.Columns("FoodItemCategory").Width = 160
            dgvFoodItems.Columns("FoodItemPrice").Width = 110

            'money is right aligned and shown with a pound sign, the same as everywhere else
            dgvFoodItems.Columns("FoodItemPrice").DefaultCellStyle.Format = "C"
            dgvFoodItems.Columns("FoodItemPrice").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        End If

        ShowCount(dt.Rows.Count)
        dgvFoodItems.ClearSelection()

        WriteLog("FOOD", "Food item list loaded")
    End Sub

    'says how many items are showing and what the counter charges on average, which is a quick way
    'of spotting a price that has been typed in wrong
    Private Sub ShowCount(shown As Integer)
        If shown = 0 Then
            lblGridCount.Text = "Nothing in this category"
            Exit Sub
        End If

        Dim total As Double = 0
        For Each row As DataGridViewRow In dgvFoodItems.Rows
            total = total + CDbl(row.Cells("FoodItemPrice").Value)
        Next

        lblGridCount.Text = shown & " item(s), " & FormatCurrency(total / shown) & " on average"
    End Sub

    'changing the category at the top loads the grid again
    Private Sub cboFilter_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboFilter.SelectedIndexChanged
        If stillLoading Then
            Exit Sub
        End If

        LoadFoodItems()
    End Sub

    'checks what has been typed in before it goes anywhere near the database. it is in one place
    'because adding an item and changing one both need exactly the same checks doing
    Private Function DetailsAreOk() As Boolean
        If txtName.Text.Trim() = "" Then
            MessageBox.Show("Enter a name for the item", "Food Items", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtName.Focus()
            Return False
        End If

        If txtPrice.Text.Trim() = "" Then
            MessageBox.Show("Enter a price", "Food Items", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtPrice.Focus()
            Return False
        End If

        If Not IsNumeric(txtPrice.Text) Then
            MessageBox.Show("The price has to be a number", "Food Items", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtPrice.Focus()
            Return False
        End If

        If Val(txtPrice.Text) <= 0 Then
            MessageBox.Show("The price has to be more than nothing", "Food Items", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtPrice.Focus()
            Return False
        End If

        'nothing behind a cinema counter costs fifty pounds, so a number that big is a slip, most
        'likely the price being typed in pence instead of pounds
        If Val(txtPrice.Text) > 50 Then
            MessageBox.Show("That price looks too high, it should be in pounds", "Food Items", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtPrice.Focus()
            Return False
        End If

        If cboCategory.Text.Trim() = "" Then
            MessageBox.Show("Pick a category", "Food Items", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            cboCategory.Focus()
            Return False
        End If

        If NameAlreadyUsed() Then
            MessageBox.Show("There is already an item called '" & txtName.Text.Trim() & "' on the menu." & vbCrLf &
                            "Two items with the same name are impossible to tell apart at the till.",
                            "Already on the menu", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtName.Focus()
            Return False
        End If

        Return True
    End Function

    'says whether another item on the menu already has this name. the item being edited is left out
    'of the count so saving one without renaming it does not trip over itself, and nothing being
    'selected leaves selectedFoodItemID as 0, which no real item has
    Private Function NameAlreadyUsed() As Boolean
        Dim total As Integer = 0

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT COUNT(*) FROM tblFoodItem " &
                                 "WHERE FoodItemName = @FoodItemName AND FoodItemID <> @FoodItemID"
            SQLCmd.Parameters.AddWithValue("@FoodItemName", txtName.Text.Trim())
            SQLCmd.Parameters.AddWithValue("@FoodItemID", CInt(selectedFoodItemID))
            total = CInt(SQLCmd.ExecuteScalar())
            cn.Close()
        End If

        Return total > 0
    End Function

    'adds a new item to the menu using the values typed into the boxes
    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        If Not DetailsAreOk() Then
            Exit Sub
        End If

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "INSERT INTO tblFoodItem (FoodItemName, FoodItemPrice, FoodItemCategory) " &
                                 "VALUES (@FoodItemName, @FoodItemPrice, @FoodItemCategory)"
            SQLCmd.Parameters.AddWithValue("@FoodItemName", txtName.Text.Trim())
            SQLCmd.Parameters.AddWithValue("@FoodItemPrice", Val(txtPrice.Text))
            SQLCmd.Parameters.AddWithValue("@FoodItemCategory", cboCategory.Text.Trim())
            SQLCmd.ExecuteNonQuery()
            cn.Close()
        End If

        Dim savedName As String = txtName.Text.Trim()
        WriteLog("FOOD", "Food item added: " & savedName, LogChange)
        ReloadEverything()
        SayDone(lblSaved, "Added '" & savedName & "'")
    End Sub

    'saves the changes made to the item selected in the grid
    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        If selectedFoodItemID = 0 Then
            MessageBox.Show("Select an item in the grid first", "Food Items", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If Not DetailsAreOk() Then
            Exit Sub
        End If

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "UPDATE tblFoodItem " &
                                 "SET FoodItemName = @FoodItemName, FoodItemPrice = @FoodItemPrice, FoodItemCategory = @FoodItemCategory " &
                                 "WHERE FoodItemID = @FoodItemID"
            SQLCmd.Parameters.AddWithValue("@FoodItemName", txtName.Text.Trim())
            SQLCmd.Parameters.AddWithValue("@FoodItemPrice", Val(txtPrice.Text))
            SQLCmd.Parameters.AddWithValue("@FoodItemCategory", cboCategory.Text.Trim())
            SQLCmd.Parameters.AddWithValue("@FoodItemID", CInt(selectedFoodItemID))
            SQLCmd.ExecuteNonQuery()
            cn.Close()
        End If

        Dim savedName As String = txtName.Text.Trim()
        WriteLog("FOOD", "Food item updated: " & savedName, LogChange)
        ReloadEverything()
        SayDone(lblSaved, "Saved changes to '" & savedName & "'")
    End Sub

    'takes an item off the menu
    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If selectedFoodItemID = 0 Then
            MessageBox.Show("Select an item in the grid first", "Food Items", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        'an item somebody has already bought cannot just be removed, the order lines for it would
        'be left pointing at nothing and the sales report would stop adding up
        Dim ordered As Integer = TimesOrdered(selectedFoodItemID)

        If ordered > 0 Then
            MessageBox.Show("'" & txtName.Text & "' is on " & ordered & " order(s) that have already been taken." & vbCrLf &
                            "It cannot be removed without those sales stopping adding up.",
                            "Cannot delete", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            WriteLog("FOOD", "Delete refused for '" & txtName.Text & "', it is on " & ordered & " order(s)", LogWarning)
            Exit Sub
        End If

        If MessageBox.Show("Take '" & txtName.Text & "' off the menu?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
            Exit Sub
        End If

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "DELETE FROM tblFoodItem " &
                                 "WHERE FoodItemID = @FoodItemID"
            SQLCmd.Parameters.AddWithValue("@FoodItemID", CInt(selectedFoodItemID))
            SQLCmd.ExecuteNonQuery()
            cn.Close()
        End If

        Dim savedName As String = txtName.Text.Trim()
        WriteLog("FOOD", "Food item deleted: " & savedName, LogChange)
        ReloadEverything()
        SayDone(lblSaved, "Took '" & savedName & "' off the menu")
    End Sub

    'counts how many order lines an item is on, used to stop it being deleted once it has sold
    Private Function TimesOrdered(foodItemID As Long) As Integer
        Dim total As Integer = 0

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT COUNT(*) FROM tblOrderItem WHERE FoodItemID = @FoodItemID"
            SQLCmd.Parameters.AddWithValue("@FoodItemID", CInt(foodItemID))
            total = CInt(SQLCmd.ExecuteScalar())
            cn.Close()
        End If

        Return total
    End Function

    'after anything is added, changed or removed the category boxes might need to change as well,
    'so everything is loaded again rather than just the grid
    Private Sub ReloadEverything()
        Dim keepFilter As String = cboFilter.Text

        stillLoading = True
        LoadCategories()

        'put the filter back where it was if that category still has something in it
        If cboFilter.Items.Contains(keepFilter) Then
            cboFilter.SelectedItem = keepFilter
        End If
        stillLoading = False

        LoadFoodItems()
        ClearFields()
    End Sub

    'clears the boxes and the selection
    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        ClearFields()
        WriteLog("FOOD", "Food item fields cleared")
    End Sub

    Private Sub ClearFields()
        'the confirmation only lasts until the next thing is started
        lblSaved.Text = ""
        selectedFoodItemID = 0
        txtName.Text = ""
        txtPrice.Text = ""
        cboCategory.SelectedIndex = -1
        cboCategory.Text = ""
        dgvFoodItems.ClearSelection()
        ShowWhatIsBeingEdited()
    End Sub

    'the heading over the boxes says whether a new item is being typed in or an existing one is
    'being changed. save and delete are switched off until something is picked, rather than
    'letting them be pressed and then telling the user off with a message box
    Private Sub ShowWhatIsBeingEdited()
        If selectedFoodItemID = 0 Then
            lblStatus.Text = "Adding a new item"
            btnUpdate.Enabled = False
            btnDelete.Enabled = False
            btnAdd.Enabled = True
        Else
            lblStatus.Text = "Editing: " & txtName.Text
            btnUpdate.Enabled = True
            btnDelete.Enabled = True
            btnAdd.Enabled = False
        End If
    End Sub

    'when a row is clicked, load its values into the boxes for editing
    Private Sub dgvFoodItems_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvFoodItems.CellClick
        If e.RowIndex < 0 Then Exit Sub

        Dim row As DataGridViewRow = dgvFoodItems.Rows(e.RowIndex)
        selectedFoodItemID = CLng(row.Cells("FoodItemID").Value)
        txtName.Text = row.Cells("FoodItemName").Value.ToString()
        cboCategory.Text = row.Cells("FoodItemCategory").Value.ToString()

        'the grid shows the price with a pound sign in front of it, the box wants the plain number
        txtPrice.Text = Format(CDbl(row.Cells("FoodItemPrice").Value), "0.00")

        ShowWhatIsBeingEdited()
        WriteLog("FOOD", "Food item selected: " & txtName.Text)
    End Sub

End Class
