Imports System.Data.OleDb

Public Class frmFoodItems

    'tracks the FoodItemID of the row currently selected in the grid, 0 means nothing selected
    Private selectedFoodItemID As Long = 0

    'whether the selected item is still on sale or has been taken off the menu. it decides what
    'the withdraw button says and what pressing it does
    Private selectedStatus As String = FoodOnSale

    'true while the form is setting itself up, so filling the category box does not load the grid
    'before everything is ready
    Private stillLoading As Boolean = True

    'true once something has been typed into the boxes that has not been saved yet. it is what
    'the warning before another row replaces it is based on
    Private boxesChanged As Boolean = False

    'true while a row is being copied into the boxes, so filling them in does not count as typing
    Private fillingBoxes As Boolean = False

    'the three bits of picture state, the same three the films form keeps. the name in the
    'database, what that name was when the row was picked so the old file can be tidied up, and
    'the full path of one that has been chosen but not saved yet
    Private imageFileName As String = ""
    Private imageOriginalName As String = ""
    Private imageSourceFile As String = ""

    'how big the little pictures down the side of the grid are drawn
    Private Const ThumbSize As Integer = 40

    Private Sub frmFoodItems_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If UserAccessLevel <> 1 Then
            MessageBox.Show("Only a manager can change what is on the menu.", "Food Items", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            WriteLog("FOOD", "Food items screen refused, access level " & UserAccessLevel, LogSecurity)
            Me.Close()
            Exit Sub
        End If

        CommonFormStartup(Me)

        LoadCategories()

        'the rows have to be tall enough for a picture to sit in. it is set here rather than in the
        'designer because opening the form in the designer wipes the row template settings
        dgvFoodItems.RowTemplate.MinimumHeight = ThumbSize + 8

        stillLoading = False

        'lets the form see escape before the box that has focus does
        Me.KeyPreview = True

        LoadFoodItems()
        ClearFields()
        txtName.Focus()
        WriteLog("FOOD", "Food items form opened")
    End Sub

    'the pictures on the grid and the one in the preview are all held open until they are given
    'back, so they are dropped on the way out. this is before the access check on purpose, the
    'same as the films form, so closing on the refusal path does not leave them behind either
    Private Sub frmFoodItems_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        DisposeGridPictures()

        If picFood.Image IsNot Nothing Then
            picFood.Image.Dispose()
            picFood.Image = Nothing
        End If
    End Sub

    'escape empties the search box, or shuts the form if there is nothing to empty. doing both off
    'the one key means it never has to be explained, you press it until you are out
    Private Sub frmFoodItems_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.F5 Then
            LoadFoodItems()
        ElseIf e.KeyCode = Keys.Escape Then
            If txtSearch.Text <> "" Then
                txtSearch.Text = ""
            Else
                Me.Close()
            End If
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

    'loads the menu into the grid. the search box, the category box and the withdrawn tick box can
    'each narrow it, so the conditions are built up into one string and only put after a WHERE if
    'anything actually asked for one
    Private Sub LoadFoodItems()
        'the pictures on the rows that are about to be thrown away have to be given back first,
        'otherwise every reload quietly loses a handle for every row on screen
        DisposeGridPictures()

        Dim dt As New DataTable

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn

            Dim baseQuery As String = "SELECT FoodItemID, FoodItemName, FoodItemCategory, FoodItemPrice, " &
                                      "FoodItemImage, FoodItemStatus " &
                                      "FROM tblFoodItem"

            'the values are added in the same order as the conditions, because OleDb goes by the
            'order the parameters were added and not by their names. the search term is put in
            'twice under two names for exactly that reason
            Dim conditions As String = ""

            If txtSearch.Text.Trim() <> "" Then
                conditions = "(FoodItemName LIKE @Search OR FoodItemCategory LIKE @Search2)"
            End If

            If CategoryPicked() <> "" Then
                If conditions <> "" Then conditions = conditions & " AND "
                conditions = conditions & "FoodItemCategory = @Category"
            End If

            If Not chkShowWithdrawn.Checked Then
                If conditions <> "" Then conditions = conditions & " AND "
                conditions = conditions & "(FoodItemStatus IS NULL OR FoodItemStatus <> @Withdrawn)"
            End If

            If conditions = "" Then
                SQLCmd.CommandText = baseQuery & " ORDER BY FoodItemCategory, FoodItemName, FoodItemID"
            Else
                SQLCmd.CommandText = baseQuery & " WHERE " & conditions &
                                     " ORDER BY FoodItemCategory, FoodItemName, FoodItemID"
            End If

            If txtSearch.Text.Trim() <> "" Then
                SQLCmd.Parameters.AddWithValue("@Search", "%" & txtSearch.Text.Trim() & "%")
                SQLCmd.Parameters.AddWithValue("@Search2", "%" & txtSearch.Text.Trim() & "%")
            End If

            If CategoryPicked() <> "" Then
                SQLCmd.Parameters.AddWithValue("@Category", CategoryPicked())
            End If

            If Not chkShowWithdrawn.Checked Then
                SQLCmd.Parameters.AddWithValue("@Withdrawn", FoodWithdrawn)
            End If

            Dim da As New OleDbDataAdapter(SQLCmd)
            da.Fill(dt)
            cn.Close()
        End If

        'the picture column is made here rather than coming out of the database, because all the
        'database keeps is the file name. a small copy is drawn for each row at the size it is
        'actually shown, so the grid is not holding a full sized jpg per row
        dt.Columns.Add("Picture", GetType(Image))
        For Each row As DataRow In dt.Rows
            row("Picture") = SmallPicture("FoodPictures", row("FoodItemImage").ToString(), ThumbSize, ThumbSize)
        Next

        dgvFoodItems.DataSource = dt

        If dgvFoodItems.Columns.Contains("FoodItemID") Then
            TidyGridColumns()
            MarkWithdrawnRows()
        End If

        ShowCount(dt.Rows.Count)
        ShowEmptyMessage(dt)
        dgvFoodItems.ClearSelection()
    End Sub

    'sets the headers, the widths and the order the columns come in. the picture column is made in
    'code so it lands on the end, which is why every column is given a position rather than only
    'the new one
    Private Sub TidyGridColumns()
        dgvFoodItems.Columns("FoodItemID").HeaderText = "ID"
        dgvFoodItems.Columns("FoodItemName").HeaderText = "Item"
        dgvFoodItems.Columns("FoodItemCategory").HeaderText = "Category"
        dgvFoodItems.Columns("FoodItemPrice").HeaderText = "Price"
        dgvFoodItems.Columns("FoodItemStatus").HeaderText = "Status"
        dgvFoodItems.Columns("Picture").HeaderText = ""

        'the file name is only along for the ride so clicking a row can pick it up
        dgvFoodItems.Columns("FoodItemImage").Visible = False

        dgvFoodItems.Columns("Picture").DisplayIndex = 0
        dgvFoodItems.Columns("FoodItemID").DisplayIndex = 1
        dgvFoodItems.Columns("FoodItemName").DisplayIndex = 2
        dgvFoodItems.Columns("FoodItemCategory").DisplayIndex = 3
        dgvFoodItems.Columns("FoodItemPrice").DisplayIndex = 4
        dgvFoodItems.Columns("FoodItemStatus").DisplayIndex = 5

        dgvFoodItems.Columns("Picture").Width = ThumbSize + 20
        dgvFoodItems.Columns("FoodItemID").Width = 50
        dgvFoodItems.Columns("FoodItemName").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        dgvFoodItems.Columns("FoodItemCategory").Width = 160
        dgvFoodItems.Columns("FoodItemPrice").Width = 110
        dgvFoodItems.Columns("FoodItemStatus").Width = 110

        'the picture is fitted into the cell keeping its shape instead of being stretched to fill it
        Dim pictureColumn As DataGridViewImageColumn = CType(dgvFoodItems.Columns("Picture"), DataGridViewImageColumn)
        pictureColumn.ImageLayout = DataGridViewImageCellLayout.Zoom

        'money is right aligned and shown with a pound sign, the same as everywhere else
        dgvFoodItems.Columns("FoodItemPrice").DefaultCellStyle.Format = "C"
        dgvFoodItems.Columns("FoodItemPrice").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
    End Sub

    'greys out anything that has been taken off the menu, so a withdrawn item is obvious at a
    'glance rather than only being findable by reading the status column
    Private Sub MarkWithdrawnRows()
        For Each row As DataGridViewRow In dgvFoodItems.Rows
            If row.Cells("FoodItemStatus").Value.ToString() = FoodWithdrawn Then
                row.DefaultCellStyle.ForeColor = PastFore
            End If
        Next
    End Sub

    'gives back every picture the grid is holding. a picture read off disk stays in memory until
    'it is disposed, and the grid is rebuilt on every search key press, so without this typing a
    'few letters into the search box leaks a picture for every row it drew on the way
    Private Sub DisposeGridPictures()
        Dim current As DataTable = TryCast(dgvFoodItems.DataSource, DataTable)

        If current Is Nothing OrElse Not current.Columns.Contains("Picture") Then
            Exit Sub
        End If

        For Each row As DataRow In current.Rows
            If Not IsDBNull(row("Picture")) AndAlso row("Picture") IsNot Nothing Then
                CType(row("Picture"), Image).Dispose()
            End If
        Next
    End Sub

    'the category picked at the top, or an empty string for all of them. it is a function because
    'both the query and the count label underneath need to ask the same question
    Private Function CategoryPicked() As String
        If cboFilter.SelectedIndex <= 0 Then
            Return ""
        End If

        Return cboFilter.Text
    End Function

    'says how many items are showing and what the counter charges on average, which is a quick way
    'of spotting a price that has been typed in wrong. it also says how it has been narrowed down,
    'so an empty looking grid explains itself instead of looking broken
    Private Sub ShowCount(shown As Integer)
        Dim narrowedBy As String = ""

        If txtSearch.Text.Trim() <> "" Then
            narrowedBy = "matching '" & txtSearch.Text.Trim() & "'"
        End If

        If CategoryPicked() <> "" Then
            If narrowedBy <> "" Then narrowedBy = narrowedBy & " and"
            narrowedBy = narrowedBy & " in " & CategoryPicked()
        End If

        If shown = 0 Then
            If narrowedBy = "" Then
                lblGridCount.Text = "Nothing on the menu yet"
            Else
                lblGridCount.Text = "Nothing " & narrowedBy.Trim()
            End If
            Exit Sub
        End If

        Dim total As Double = 0
        Dim withdrawn As Integer = 0

        For Each row As DataGridViewRow In dgvFoodItems.Rows
            total = total + CDbl(row.Cells("FoodItemPrice").Value)

            If row.Cells("FoodItemStatus").Value.ToString() = FoodWithdrawn Then
                withdrawn = withdrawn + 1
            End If
        Next

        Dim message As String = shown & " item(s) " & narrowedBy.Trim()
        message = message.Trim() & ", " & FormatCurrency(total / shown) & " on average"

        'only worth saying when there is one, otherwise it is noise on every single load
        If withdrawn > 0 Then
            message = message & ", " & withdrawn & " off the menu"
        End If

        lblGridCount.Text = message
    End Sub

    'an empty grid on its own looks like something has gone wrong, so when there is nothing to
    'list the grid is hidden and a message is put in exactly the same space explaining why
    Private Sub ShowEmptyMessage(dt As DataTable)
        If dt.Rows.Count > 0 Then
            lblNoRows.Visible = False
            dgvFoodItems.Visible = True
            Exit Sub
        End If

        If txtSearch.Text.Trim() <> "" Then
            lblNoRows.Text = "Nothing on the menu matches " & Chr(34) & txtSearch.Text.Trim() & Chr(34) & "."
        ElseIf CategoryPicked() <> "" Then
            lblNoRows.Text = "There is nothing in " & CategoryPicked() & "."
        Else
            lblNoRows.Text = "There is nothing on the menu yet."
        End If

        'the label takes the grids place so it is painted the same colour as the grid would be
        lblNoRows.BackColor = InputBack
        dgvFoodItems.Visible = False
        lblNoRows.Visible = True
    End Sub

    'changing the category at the top loads the grid again
    Private Sub cboFilter_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboFilter.SelectedIndexChanged
        If stillLoading Then
            Exit Sub
        End If

        LoadFoodItems()
    End Sub

    'so does ticking or unticking the withdrawn box
    Private Sub chkShowWithdrawn_CheckedChanged(sender As Object, e As EventArgs) Handles chkShowWithdrawn.CheckedChanged
        If stillLoading Then
            Exit Sub
        End If

        LoadFoodItems()
    End Sub

    'the list narrows as it is typed in, there is no need for a search button
    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        If stillLoading Then
            Exit Sub
        End If

        'reloading on every key press would be a trip to the database and a rebuild of the grid
        'for every letter, and this grid draws a picture on every row while it does it. the timer
        'is restarted instead, so typing straight through only searches once at the end
        timerSearch.Stop()
        timerSearch.Start()
    End Sub

    'runs a fraction of a second after the last key press in the search box
    Private Sub timerSearch_Tick(sender As Object, e As EventArgs) Handles timerSearch.Tick
        timerSearch.Stop()
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

    'the price as it should be stored. a price is money and money has two decimal places, but the
    'box will happily take 3.999, and Access would store that as typed and then show it rounded,
    'so the grid and the box would disagree about what an item costs
    Private Function PriceForDatabase() As Double
        Return Math.Round(Val(txtPrice.Text), 2)
    End Function

    'matches what has been typed in the category box against the ones already in use and hands
    'back the spelling that is already there. without this, typing snacks when there is already a
    'Snacks makes a second category that is the same thing, and the menu splits in half
    Private Function TidyCategory(typed As String) As String
        Dim tidied As String = typed.Trim()
        Dim i As Integer

        For i = 0 To cboCategory.Items.Count - 1
            If cboCategory.Items(i).ToString().ToUpper() = tidied.ToUpper() Then
                Return cboCategory.Items(i).ToString()
            End If
        Next

        Return tidied
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

        Dim newFoodItemID As Long = 0

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "INSERT INTO tblFoodItem (FoodItemName, FoodItemPrice, FoodItemCategory, FoodItemStatus) " &
                                 "VALUES (@FoodItemName, @FoodItemPrice, @FoodItemCategory, @FoodItemStatus)"
            SQLCmd.Parameters.AddWithValue("@FoodItemName", txtName.Text.Trim())
            SQLCmd.Parameters.AddWithValue("@FoodItemPrice", PriceForDatabase())
            SQLCmd.Parameters.AddWithValue("@FoodItemCategory", TidyCategory(cboCategory.Text))
            SQLCmd.Parameters.AddWithValue("@FoodItemStatus", FoodOnSale)
            SQLCmd.ExecuteNonQuery()

            'the picture file is named after the item, so the item has to exist before its picture
            'can be saved. the id is asked for while the connection is still open
            SQLCmd.CommandText = "SELECT @@IDENTITY"
            SQLCmd.Parameters.Clear()
            newFoodItemID = CLng(SQLCmd.ExecuteScalar())

            cn.Close()
        End If

        'nothing was written if the database could not be opened, so it must not be
        'logged or announced as though it had been
        If newFoodItemID = 0 Then
            Exit Sub
        End If

        'the picture is copied in as a second write rather than being part of the insert. a file
        'copy cannot be rolled back the way a row can, and an item that saved without its picture
        'is a far better outcome than one that did not save at all
        If imageSourceFile <> "" Then
            Dim addedPicture As String = SaveFoodImageFile(imageSourceFile, newFoodItemID)
            If addedPicture <> "" Then
                SaveImageName(newFoodItemID, addedPicture)
            End If
        End If

        Dim savedName As String = txtName.Text.Trim()
        WriteLog("FOOD", "Food item added: " & savedName, LogChange)
        ReloadEverything()
        SayDone(lblSaved, "Added '" & savedName & "'")
    End Sub

    'saves the changes made to the item selected in the grid
    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        'this cannot normally happen, the button is switched off until a row is picked.
        'it stays in so the sub can never run without an id, whatever calls it
        If selectedFoodItemID = 0 Then
            MessageBox.Show("Select an item in the grid first", "Food Items", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If Not DetailsAreOk() Then
            Exit Sub
        End If

        'the file is copied in before the update runs, so the name that gets written is a name
        'that really exists on disk
        If imageSourceFile <> "" Then
            Dim newPicture As String = SaveFoodImageFile(imageSourceFile, selectedFoodItemID)
            If newPicture <> "" Then
                imageFileName = newPicture
            End If
        End If

        Dim saved As Boolean = False

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "UPDATE tblFoodItem " &
                                 "SET FoodItemName = @FoodItemName, FoodItemPrice = @FoodItemPrice, FoodItemCategory = @FoodItemCategory, FoodItemImage = @FoodItemImage " &
                                 "WHERE FoodItemID = @FoodItemID"
            SQLCmd.Parameters.AddWithValue("@FoodItemName", txtName.Text.Trim())
            SQLCmd.Parameters.AddWithValue("@FoodItemPrice", PriceForDatabase())
            SQLCmd.Parameters.AddWithValue("@FoodItemCategory", TidyCategory(cboCategory.Text))
            SQLCmd.Parameters.AddWithValue("@FoodItemImage", imageFileName)
            SQLCmd.Parameters.AddWithValue("@FoodItemID", CInt(selectedFoodItemID))
            SQLCmd.ExecuteNonQuery()
            cn.Close()
            saved = True
        End If

        'nothing was written if the database could not be opened, so it must not be
        'logged or announced as though it had been
        If Not saved Then
            Exit Sub
        End If

        'the old file is only thrown away once the new name is safely in the database. an item
        'keeps its id so a new jpg usually writes straight over the old one, this is really only
        'for when the picture comes off altogether or a png replaces a jpg
        If imageOriginalName <> "" AndAlso imageOriginalName <> imageFileName Then
            DeleteFoodImageFile(imageOriginalName)
        End If

        Dim savedName As String = txtName.Text.Trim()
        WriteLog("FOOD", "Food item updated: " & savedName, LogChange)
        ReloadEverything()
        SayDone(lblSaved, "Saved changes to '" & savedName & "'")
    End Sub

    'takes an item off the menu, or offers to withdraw it instead when it cannot be deleted
    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        'this cannot normally happen, the button is switched off until a row is picked.
        'it stays in so the sub can never run without an id, whatever calls it
        If selectedFoodItemID = 0 Then
            MessageBox.Show("Select an item in the grid first", "Food Items", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        'an item somebody has already bought cannot just be removed, the order lines for it would
        'be left pointing at nothing and the sales report would stop adding up
        Dim ordered As Integer = TimesOrdered(selectedFoodItemID)

        If ordered > 0 Then
            'it used to just say no here, which left no way at all of retiring something that had
            'ever sold. now it says what to do instead and offers to do it
            Dim answer As DialogResult =
                MessageBox.Show("'" & txtName.Text & "' is on " & ordered & " order(s) that have already been taken." & vbCrLf &
                                "It cannot be removed without those sales stopping adding up." & vbCrLf & vbCrLf &
                                "Take it off the menu instead? It stops being sold but the old orders keep it.",
                                "Cannot delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

            WriteLog("FOOD", "Delete refused for '" & txtName.Text & "', it is on " & ordered & " order(s)", LogWarning)

            If answer = DialogResult.Yes Then
                SetItemStatus(FoodWithdrawn)
            End If

            Exit Sub
        End If

        If MessageBox.Show("Take '" & txtName.Text & "' off the menu?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = DialogResult.No Then
            Exit Sub
        End If

        Dim goneName As String = txtName.Text.Trim()
        Dim gonePicture As String = imageFileName
        Dim saved As Boolean = False

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "DELETE FROM tblFoodItem " &
                                 "WHERE FoodItemID = @FoodItemID"
            SQLCmd.Parameters.AddWithValue("@FoodItemID", CInt(selectedFoodItemID))
            SQLCmd.ExecuteNonQuery()
            cn.Close()
            saved = True
        End If

        'nothing was written if the database could not be opened, so it must not be
        'logged or announced as though it had been
        If Not saved Then
            Exit Sub
        End If

        'the row has gone, so nothing points at the picture any more
        DeleteFoodImageFile(gonePicture)

        WriteLog("FOOD", "Food item deleted: " & goneName, LogChange)
        ReloadEverything()
        SayDone(lblSaved, "Took '" & goneName & "' off the menu")
    End Sub

    'takes an item off the menu or puts it back, whichever way round it currently is
    Private Sub btnWithdraw_Click(sender As Object, e As EventArgs) Handles btnWithdraw.Click
        If selectedFoodItemID = 0 Then
            MessageBox.Show("Select an item in the grid first", "Food Items", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Dim itemName As String = txtName.Text.Trim()

        If selectedStatus = FoodWithdrawn Then
            If MessageBox.Show("Put '" & itemName & "' back on the menu?", "Confirm",
                               MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
                Exit Sub
            End If

            SetItemStatus(FoodOnSale)
        Else
            If MessageBox.Show("Take '" & itemName & "' off the menu?" & vbCrLf &
                               "It stops being offered at the till and on the kiosk, but every order it is already on keeps it.",
                               "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = DialogResult.No Then
                Exit Sub
            End If

            SetItemStatus(FoodWithdrawn)
        End If
    End Sub

    'writes the new status onto the selected item. it is its own sub because both the withdraw
    'button and the offer made when a delete is refused end up doing exactly the same thing
    Private Sub SetItemStatus(newStatus As String)
        Dim itemName As String = txtName.Text.Trim()
        Dim saved As Boolean = False

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "UPDATE tblFoodItem SET FoodItemStatus = @FoodItemStatus " &
                                 "WHERE FoodItemID = @FoodItemID"
            SQLCmd.Parameters.AddWithValue("@FoodItemStatus", newStatus)
            SQLCmd.Parameters.AddWithValue("@FoodItemID", CInt(selectedFoodItemID))
            SQLCmd.ExecuteNonQuery()
            cn.Close()
            saved = True
        End If

        If Not saved Then
            Exit Sub
        End If

        If newStatus = FoodWithdrawn Then
            WriteLog("FOOD", "Food item taken off the menu: " & itemName, LogChange)
            ReloadEverything()
            SayDone(lblSaved, "Took '" & itemName & "' off the menu")
        Else
            WriteLog("FOOD", "Food item put back on the menu: " & itemName, LogChange)
            ReloadEverything()
            SayDone(lblSaved, "Put '" & itemName & "' back on the menu")
        End If
    End Sub

    'writes which picture belongs to an item. it is its own sub because adding an item has to do
    'it as a second step, once Access has given the new row an id to name the file after
    Private Sub SaveImageName(foodItemID As Long, fileName As String)
        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "UPDATE tblFoodItem SET FoodItemImage = @FoodItemImage WHERE FoodItemID = @FoodItemID"
            SQLCmd.Parameters.AddWithValue("@FoodItemImage", fileName)
            SQLCmd.Parameters.AddWithValue("@FoodItemID", CInt(foodItemID))
            SQLCmd.ExecuteNonQuery()
            cn.Close()
        End If
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

    'sends whatever the grid is showing to a csv file, filters and all
    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        If dgvFoodItems.Rows.Count = 0 Then
            MessageBox.Show("There is nothing showing to export", "Food Items", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        If ExportGridToCsv(dgvFoodItems, "food items.csv", "Food Items") Then
            WriteLog("FOOD", "Food items exported to CSV")
            SayDone(lblSaved, "Exported " & dgvFoodItems.Rows.Count & " item(s)")
        End If
    End Sub

    'clears the boxes and the selection
    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        ClearFields()
    End Sub

    'anything changed in the boxes by hand counts as an unsaved change
    Private Sub Details_Changed(sender As Object, e As EventArgs) Handles txtName.TextChanged, txtPrice.TextChanged,
        cboCategory.TextChanged
        If fillingBoxes Then
            Exit Sub
        End If

        boxesChanged = True
        ShowPreview()
    End Sub

    'asks before typing that has not been saved gets thrown away. it only asks when something has
    'actually been changed, so clicking down a list of rows to read them never interrupts
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

        'the confirmation only lasts until the next thing is started
        lblSaved.Text = ""
        selectedFoodItemID = 0
        selectedStatus = FoodOnSale
        txtName.Text = ""
        txtPrice.Text = ""
        cboCategory.SelectedIndex = -1
        cboCategory.Text = ""

        imageFileName = ""
        imageOriginalName = ""
        imageSourceFile = ""

        fillingBoxes = False
        boxesChanged = False

        dgvFoodItems.ClearSelection()
        ShowPicture()
        ShowWhatIsBeingEdited()
    End Sub

    'the heading over the boxes says whether a new item is being typed in or an existing one is
    'being changed. save, delete and the withdraw button are switched off until something is
    'picked, rather than letting them be pressed and then telling the user off with a message box
    Private Sub ShowWhatIsBeingEdited()
        If selectedFoodItemID = 0 Then
            lblStatus.Text = "Adding a new item"
            btnUpdate.Enabled = False
            btnDelete.Enabled = False
            btnWithdraw.Enabled = False
            btnAdd.Enabled = True

            'it has to say something even while it is switched off, an empty button looks broken
            btnWithdraw.Text = "Take off the menu"
        Else
            If selectedStatus = FoodWithdrawn Then
                lblStatus.Text = "Editing: " & txtName.Text & "  (off the menu)"
                btnWithdraw.Text = "Put back on the menu"
            Else
                lblStatus.Text = "Editing: " & txtName.Text
                btnWithdraw.Text = "Take off the menu"
            End If

            btnUpdate.Enabled = True
            btnDelete.Enabled = True
            btnWithdraw.Enabled = True
            btnAdd.Enabled = False
        End If

        ShowPreview()
    End Sub

    'the little card under the picture, so what is being typed in can be read the way it will look
    'on the menu rather than only as boxes
    Private Sub ShowPreview()
        If txtName.Text.Trim() = "" Then
            lblPreviewName.Text = "New item"
        Else
            lblPreviewName.Text = txtName.Text.Trim()
        End If

        Dim meta As String = cboCategory.Text.Trim()

        If IsNumeric(txtPrice.Text) AndAlso Val(txtPrice.Text) > 0 Then
            If meta <> "" Then meta = meta & "  ·  "
            meta = meta & FormatCurrency(PriceForDatabase())
        End If

        If selectedStatus = FoodWithdrawn AndAlso selectedFoodItemID <> 0 Then
            If meta <> "" Then meta = meta & "  ·  "
            meta = meta & "off the menu"
        End If

        lblPreviewMeta.Text = meta
    End Sub

    'puts the item's picture on screen. whatever was showing is thrown away first, because a
    'picture box left holding the old one keeps a handle that is not given back until the program
    'is shut, and clicking down a list of items would lose one every time
    Private Sub ShowPicture()
        If picFood.Image IsNot Nothing Then
            picFood.Image.Dispose()
            picFood.Image = Nothing
        End If

        'a picture that has been picked but not saved yet is still sat wherever it was chosen
        'from, so it is read from there. once it has been saved it comes out of the food folder
        If imageSourceFile <> "" Then
            picFood.Image = PictureFromFile(imageSourceFile)
        Else
            picFood.Image = FoodImage(imageFileName)
        End If

        'the words only show when there is no picture in front of them
        lblNoPicture.Visible = (picFood.Image Is Nothing)
    End Sub

    'picks a picture off the computer for this item. nothing is copied anywhere yet, it is only
    'remembered and shown, so choosing one and then not saving leaves nothing behind
    Private Sub btnChoosePicture_Click(sender As Object, e As EventArgs) Handles btnChoosePicture.Click
        Dim openDialog As New OpenFileDialog
        openDialog.Filter = "Pictures (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png"
        openDialog.Title = "Choose a picture"
        'without this the whole program is left sat in whatever folder was last opened
        openDialog.RestoreDirectory = True

        If openDialog.ShowDialog() <> DialogResult.OK Then
            Exit Sub
        End If

        'a picture that cannot be read is refused here rather than being copied in and then showing
        'as an empty box afterwards with no telling why
        Dim check As Image = PictureFromFile(openDialog.FileName)
        If check Is Nothing Then
            MessageBox.Show("That file could not be read as a picture.", "Picture", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If
        check.Dispose()

        imageSourceFile = openDialog.FileName
        boxesChanged = True
        ShowPicture()
    End Sub

    'takes the picture off the item. the file itself is not deleted until the change is saved, so
    'pressing this and then not saving changes nothing
    Private Sub btnRemovePicture_Click(sender As Object, e As EventArgs) Handles btnRemovePicture.Click
        If imageSourceFile = "" AndAlso imageFileName = "" Then
            Exit Sub
        End If

        imageSourceFile = ""
        imageFileName = ""
        boxesChanged = True
        ShowPicture()
    End Sub

    'when a row is clicked, load its values into the boxes for editing
    Private Sub dgvFoodItems_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvFoodItems.CellClick
        If e.RowIndex < 0 Then Exit Sub

        'clicking a row replaces whatever is in the boxes, so anything typed and not saved
        'would have gone without a word. the selection is left alone if the answer is no
        If Not ChangesCanBeLost() Then
            Exit Sub
        End If

        fillingBoxes = True

        Dim row As DataGridViewRow = dgvFoodItems.Rows(e.RowIndex)
        selectedFoodItemID = CLng(row.Cells("FoodItemID").Value)
        selectedStatus = row.Cells("FoodItemStatus").Value.ToString()
        txtName.Text = row.Cells("FoodItemName").Value.ToString()
        cboCategory.Text = row.Cells("FoodItemCategory").Value.ToString()

        'the grid shows the price with a pound sign in front of it, the box wants the plain number
        txtPrice.Text = Format(CDbl(row.Cells("FoodItemPrice").Value), "0.00")

        'a blank status is an item written before the column existed, and those are all on sale
        If selectedStatus = "" Then
            selectedStatus = FoodOnSale
        End If

        imageFileName = row.Cells("FoodItemImage").Value.ToString()
        imageOriginalName = imageFileName
        imageSourceFile = ""

        fillingBoxes = False
        boxesChanged = False

        ShowPicture()
        ShowWhatIsBeingEdited()
    End Sub

End Class
