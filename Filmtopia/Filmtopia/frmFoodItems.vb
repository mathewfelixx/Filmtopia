Imports System.Data.OleDb

Public Class frmFoodItems

    Private selectedFoodItemID As Long = 0

    Private selectedStatus As String = FoodOnSale

    Private stillLoading As Boolean = True

    Private boxesChanged As Boolean = False

    Private fillingBoxes As Boolean = False

    Private imageFileName As String = ""
    Private imageOriginalName As String = ""
    Private imageSourceFile As String = ""

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

        dgvFoodItems.RowTemplate.MinimumHeight = ThumbSize + 8

        stillLoading = False

        Me.KeyPreview = True

        LoadFoodItems()
        ClearFields()
        txtName.Focus()
        WriteLog("FOOD", "Food items form opened")
    End Sub

    Private Sub frmFoodItems_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        DisposeGridPictures()

        If picFood.Image IsNot Nothing Then
            picFood.Image.Dispose()
            picFood.Image = Nothing
        End If
    End Sub

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

    Private Sub LoadFoodItems()
        DisposeGridPictures()

        Dim dt As New DataTable

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn

            Dim baseQuery As String = "SELECT FoodItemID, FoodItemName, FoodItemCategory, FoodItemPrice, " &
                                      "FoodItemImage, FoodItemStatus " &
                                      "FROM tblFoodItem"

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

    Private Sub TidyGridColumns()
        dgvFoodItems.Columns("FoodItemID").HeaderText = "ID"
        dgvFoodItems.Columns("FoodItemName").HeaderText = "Item"
        dgvFoodItems.Columns("FoodItemCategory").HeaderText = "Category"
        dgvFoodItems.Columns("FoodItemPrice").HeaderText = "Price"
        dgvFoodItems.Columns("FoodItemStatus").HeaderText = "Status"
        dgvFoodItems.Columns("Picture").HeaderText = ""

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

        Dim pictureColumn As DataGridViewImageColumn = CType(dgvFoodItems.Columns("Picture"), DataGridViewImageColumn)
        pictureColumn.ImageLayout = DataGridViewImageCellLayout.Zoom

        dgvFoodItems.Columns("FoodItemPrice").DefaultCellStyle.Format = "C"
        dgvFoodItems.Columns("FoodItemPrice").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
    End Sub

    Private Function StatusOfRow(row As DataGridViewRow) As String
        If row.Cells("FoodItemStatus").Value Is Nothing OrElse IsDBNull(row.Cells("FoodItemStatus").Value) Then
            Return FoodOnSale
        End If

        If row.Cells("FoodItemStatus").Value.ToString().Trim() = "" Then
            Return FoodOnSale
        End If

        Return row.Cells("FoodItemStatus").Value.ToString()
    End Function

    Private Sub MarkWithdrawnRows()
        For Each row As DataGridViewRow In dgvFoodItems.Rows
            If StatusOfRow(row) = FoodWithdrawn Then
                row.DefaultCellStyle.ForeColor = PastFore
            End If
        Next
    End Sub

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

    Private Function CategoryPicked() As String
        If cboFilter.SelectedIndex <= 0 Then
            Return ""
        End If

        Return cboFilter.Text
    End Function

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

            If StatusOfRow(row) = FoodWithdrawn Then
                withdrawn = withdrawn + 1
            End If
        Next

        Dim message As String = shown & " item(s) " & narrowedBy.Trim()
        message = message.Trim() & ", " & FormatCurrency(total / shown) & " on average"

        If withdrawn > 0 Then
            message = message & ", " & withdrawn & " off the menu"
        End If

        lblGridCount.Text = message
    End Sub

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

        lblNoRows.BackColor = InputBack
        dgvFoodItems.Visible = False
        lblNoRows.Visible = True
    End Sub

    Private Sub cboFilter_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboFilter.SelectedIndexChanged
        If stillLoading Then
            Exit Sub
        End If

        LoadFoodItems()
    End Sub

    Private Sub chkShowWithdrawn_CheckedChanged(sender As Object, e As EventArgs) Handles chkShowWithdrawn.CheckedChanged
        If stillLoading Then
            Exit Sub
        End If

        LoadFoodItems()
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
        LoadFoodItems()
    End Sub

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

    Private Function PriceForDatabase() As Double
        Return Math.Round(Val(txtPrice.Text), 2)
    End Function

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

            SQLCmd.CommandText = "SELECT @@IDENTITY"
            SQLCmd.Parameters.Clear()
            newFoodItemID = CLng(SQLCmd.ExecuteScalar())

            cn.Close()
        End If

        If newFoodItemID = 0 Then
            Exit Sub
        End If

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

    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        If selectedFoodItemID = 0 Then
            MessageBox.Show("Select an item in the grid first", "Food Items", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If Not DetailsAreOk() Then
            Exit Sub
        End If

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

        If Not saved Then
            Exit Sub
        End If

        If imageOriginalName <> "" AndAlso imageOriginalName <> imageFileName Then
            DeleteFoodImageFile(imageOriginalName)
        End If

        Dim savedName As String = txtName.Text.Trim()
        WriteLog("FOOD", "Food item updated: " & savedName, LogChange)
        ReloadEverything()
        SayDone(lblSaved, "Saved changes to '" & savedName & "'")
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If selectedFoodItemID = 0 Then
            MessageBox.Show("Select an item in the grid first", "Food Items", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Dim ordered As Integer = TimesOrdered(selectedFoodItemID)

        If ordered > 0 Then
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

        If MessageBox.Show("Delete '" & txtName.Text & "' for good?" & vbCrLf &
                           "Nothing has ever been ordered off it, so it can go completely." & vbCrLf & vbCrLf &
                           "This cannot be undone. To keep it but stop selling it, use Take off the menu instead.",
                           "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) = DialogResult.No Then
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

        If Not saved Then
            Exit Sub
        End If

        DeleteFoodImageFile(gonePicture)

        WriteLog("FOOD", "Food item deleted: " & goneName, LogChange)
        ReloadEverything()
        SayDone(lblSaved, "Deleted '" & goneName & "'")
    End Sub

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

    Private Sub ReloadEverything()
        Dim keepFilter As String = cboFilter.Text

        stillLoading = True
        LoadCategories()

        If cboFilter.Items.Contains(keepFilter) Then
            cboFilter.SelectedItem = keepFilter
        End If
        stillLoading = False

        LoadFoodItems()
        ClearFields()
    End Sub

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

    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        ClearFields()
    End Sub

    Private Sub Details_Changed(sender As Object, e As EventArgs) Handles txtName.TextChanged, txtPrice.TextChanged,
        cboCategory.TextChanged
        If fillingBoxes Then
            Exit Sub
        End If

        boxesChanged = True
        ShowPreview()
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

    Private Sub ShowWhatIsBeingEdited()
        If selectedFoodItemID = 0 Then
            lblStatus.Text = "Adding a new item"
            btnUpdate.Enabled = False
            btnDelete.Enabled = False
            btnWithdraw.Enabled = False
            btnAdd.Enabled = True

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

    Private Sub ShowPicture()
        If picFood.Image IsNot Nothing Then
            picFood.Image.Dispose()
            picFood.Image = Nothing
        End If

        If imageSourceFile <> "" Then
            picFood.Image = PictureFromFile(imageSourceFile)
        Else
            picFood.Image = FoodImage(imageFileName)
        End If

        lblNoPicture.Visible = (picFood.Image Is Nothing)
    End Sub

    Private Sub btnChoosePicture_Click(sender As Object, e As EventArgs) Handles btnChoosePicture.Click
        Dim openDialog As New OpenFileDialog
        openDialog.Filter = "Pictures (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png"
        openDialog.Title = "Choose a picture"
        openDialog.RestoreDirectory = True

        If openDialog.ShowDialog() <> DialogResult.OK Then
            Exit Sub
        End If

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

    Private Sub btnRemovePicture_Click(sender As Object, e As EventArgs) Handles btnRemovePicture.Click
        If imageSourceFile = "" AndAlso imageFileName = "" Then
            Exit Sub
        End If

        imageSourceFile = ""
        imageFileName = ""
        boxesChanged = True
        ShowPicture()
    End Sub

    Private Sub dgvFoodItems_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvFoodItems.CellClick
        If e.RowIndex < 0 Then Exit Sub

        If Not ChangesCanBeLost() Then
            Exit Sub
        End If

        fillingBoxes = True

        Dim row As DataGridViewRow = dgvFoodItems.Rows(e.RowIndex)
        selectedFoodItemID = CLng(row.Cells("FoodItemID").Value)
        selectedStatus = StatusOfRow(row)
        txtName.Text = row.Cells("FoodItemName").Value.ToString()
        cboCategory.Text = row.Cells("FoodItemCategory").Value.ToString()

        txtPrice.Text = Format(CDbl(row.Cells("FoodItemPrice").Value), "0.00")

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
