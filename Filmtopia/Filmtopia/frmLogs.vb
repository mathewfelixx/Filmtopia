Imports System.Data.OleDb

Public Class frmLogs

    'the most rows the grid will ever load at once. there are already hundreds of entries and the
    'log only grows, so loading the lot would get slower and slower and nobody reads a thousand
    'rows anyway. the newest ones are loaded and the label underneath says if any were left off
    Private Const MaxRows As Integer = 500

    'true while the form is setting itself up, so filling the filter boxes does not run a search
    'for every single item that gets added to them
    Private stillLoading As Boolean = True

    'how many entries matched the filters altogether, which is not the same as how many are on
    'screen once the cap above has been applied
    Private matchingRows As Integer = 0

    Private Sub frmLogs_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CommonFormStartup(Me)

        'a week is a sensible amount to be looking at, the date boxes can be widened if more is wanted
        dtpFrom.Value = Date.Today.AddDays(-7)
        dtpTo.Value = Date.Today

        LoadTypeFilter()
        LoadUserFilter()
        LoadSeverityFilter()

        stillLoading = False

        LoadLogs()
    End Sub

    'fills the area box with the log types that are actually in the table, so it can never offer
    'a type that has never been used
    Private Sub LoadTypeFilter()
        cboType.Items.Clear()
        cboType.Items.Add("All areas")

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT DISTINCT LogType FROM tblLogs ORDER BY LogType"
            Dim rs As OleDbDataReader = SQLCmd.ExecuteReader()
            While rs.Read()
                cboType.Items.Add(rs("LogType").ToString())
            End While
            rs.Close()
            cn.Close()
        End If

        cboType.SelectedIndex = 0
    End Sub

    'same idea for the user box, so you can pick a person and see everything they did
    Private Sub LoadUserFilter()
        cboUser.Items.Clear()
        cboUser.Items.Add("All users")

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT DISTINCT LogUser FROM tblLogs ORDER BY LogUser"
            Dim rs As OleDbDataReader = SQLCmd.ExecuteReader()
            While rs.Read()
                'older entries were written before the user was being recorded so they are empty
                If rs("LogUser").ToString() <> "" Then
                    cboUser.Items.Add(rs("LogUser").ToString())
                End If
            End While
            rs.Close()
            cn.Close()
        End If

        cboUser.SelectedIndex = 0
    End Sub

    'the levels are fixed so this one is just typed in. anything but routine is the one that gets
    'used most, it drops all the ordinary looking about and leaves the things that actually matter
    Private Sub LoadSeverityFilter()
        cboSeverity.Items.Clear()
        cboSeverity.Items.Add("All levels")
        cboSeverity.Items.Add("Anything but routine")
        cboSeverity.Items.Add(LogInfo)
        cboSeverity.Items.Add(LogChange)
        cboSeverity.Items.Add(LogWarning)
        cboSeverity.Items.Add(LogSecurity)
        cboSeverity.SelectedIndex = 0
    End Sub

    Private Sub btnApply_Click(sender As Object, e As EventArgs) Handles btnApply.Click
        LoadLogs()
    End Sub

    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        'the type and user boxes are filled again as well, in case somebody has logged in or used
        'part of the system for the first time since this form was opened
        Dim keepType As String = cboType.Text
        Dim keepUser As String = cboUser.Text

        stillLoading = True
        LoadTypeFilter()
        LoadUserFilter()
        stillLoading = False

        'put the boxes back where they were if those choices still exist
        If cboType.Items.Contains(keepType) Then
            cboType.SelectedItem = keepType
        End If
        If cboUser.Items.Contains(keepUser) Then
            cboUser.SelectedItem = keepUser
        End If

        LoadLogs()
    End Sub

    'puts every filter back to how it starts and shows the last week again
    Private Sub btnClearFilters_Click(sender As Object, e As EventArgs) Handles btnClearFilters.Click
        stillLoading = True
        dtpFrom.Value = Date.Today.AddDays(-7)
        dtpTo.Value = Date.Today
        cboType.SelectedIndex = 0
        cboUser.SelectedIndex = 0
        cboSeverity.SelectedIndex = 0
        txtSearch.Text = ""
        stillLoading = False

        LoadLogs()
    End Sub

    'changing any of the drop downs searches again straight away
    Private Sub Filter_Changed(sender As Object, e As EventArgs) Handles cboType.SelectedIndexChanged,
        cboSeverity.SelectedIndexChanged, cboUser.SelectedIndexChanged, dtpFrom.ValueChanged, dtpTo.ValueChanged

        If stillLoading Then
            Exit Sub
        End If

        LoadLogs()
    End Sub

    'pressing enter in the search box searches, rather than having to reach for the button
    Private Sub txtSearch_KeyDown(sender As Object, e As KeyEventArgs) Handles txtSearch.KeyDown
        If e.KeyCode = Keys.Enter Then
            LoadLogs()
            e.SuppressKeyPress = True
        End If
    End Sub

    'builds the WHERE part of the query out of whichever filters have been set. it is built as a
    'string on its own because the same conditions are needed twice, once to get the rows and once
    'to count how many there were altogether
    Private Function BuildWhere() As String
        Dim where As String = "WHERE LogDateTime >= @FromDate AND LogDateTime < @ToDate"

        If cboType.SelectedIndex > 0 Then
            where = where & " AND LogType = @Type"
        End If

        If cboSeverity.Text = "Anything but routine" Then
            where = where & " AND LogSeverity <> @Severity"
        ElseIf cboSeverity.SelectedIndex > 0 Then
            where = where & " AND LogSeverity = @Severity"
        End If

        If cboUser.SelectedIndex > 0 Then
            where = where & " AND LogUser = @User"
        End If

        If txtSearch.Text.Trim() <> "" Then
            where = where & " AND LogMessage LIKE @Search"
        End If

        Return where
    End Function

    'adds the values for whatever BuildWhere put in. OleDb does not go by the name, it goes by the
    'order they were added, so these have to be added in exactly the same order as above
    Private Sub AddWhereParams(SQLCmd As OleDbCommand)
        SQLCmd.Parameters.AddWithValue("@FromDate", dtpFrom.Value.Date)
        'the to date is moved on a day and the query uses less than, otherwise anything logged
        'during the last day would be missed because its time of day is after midnight
        SQLCmd.Parameters.AddWithValue("@ToDate", dtpTo.Value.Date.AddDays(1))

        If cboType.SelectedIndex > 0 Then
            SQLCmd.Parameters.AddWithValue("@Type", cboType.Text)
        End If

        If cboSeverity.Text = "Anything but routine" Then
            SQLCmd.Parameters.AddWithValue("@Severity", LogInfo)
        ElseIf cboSeverity.SelectedIndex > 0 Then
            SQLCmd.Parameters.AddWithValue("@Severity", cboSeverity.Text)
        End If

        If cboUser.SelectedIndex > 0 Then
            SQLCmd.Parameters.AddWithValue("@User", cboUser.Text)
        End If

        If txtSearch.Text.Trim() <> "" Then
            SQLCmd.Parameters.AddWithValue("@Search", "%" & txtSearch.Text.Trim() & "%")
        End If
    End Sub

    'loads the entries that match the filters, newest first
    Private Sub LoadLogs()
        Dim dt As New DataTable
        matchingRows = 0

        If DbConnect() Then
            Dim where As String = BuildWhere()

            'how many there are altogether before the cap is applied
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT COUNT(*) FROM tblLogs " & where
            AddWhereParams(SQLCmd)
            matchingRows = CInt(SQLCmd.ExecuteScalar())

            'then the rows themselves, newest first so the cap keeps the most recent ones
            Dim SQLCmd2 As New OleDbCommand
            SQLCmd2.Connection = cn
            SQLCmd2.CommandText = "SELECT TOP " & MaxRows & " LogDateTime, LogSeverity, LogType, LogUser, LogMessage " &
                                  "FROM tblLogs " & where & " " &
                                  "ORDER BY LogDateTime DESC, LogID DESC"
            AddWhereParams(SQLCmd2)
            Dim da As New OleDbDataAdapter(SQLCmd2)
            da.Fill(dt)
            cn.Close()
        End If

        dgvLogs.DataSource = dt

        If dgvLogs.Columns.Count > 0 Then
            dgvLogs.Columns("LogDateTime").HeaderText = "When"
            dgvLogs.Columns("LogSeverity").HeaderText = "Level"
            dgvLogs.Columns("LogType").HeaderText = "Area"
            dgvLogs.Columns("LogUser").HeaderText = "User"
            dgvLogs.Columns("LogMessage").HeaderText = "What happened"

            dgvLogs.Columns("LogDateTime").Width = 150
            dgvLogs.Columns("LogSeverity").Width = 90
            dgvLogs.Columns("LogType").Width = 100
            dgvLogs.Columns("LogUser").Width = 110
            dgvLogs.Columns("LogMessage").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill

            dgvLogs.Columns("LogDateTime").DefaultCellStyle.Format = "dd/MM/yyyy HH:mm:ss"
        End If

        ColourRows()
        ShowCount()
        dgvLogs.ClearSelection()
    End Sub

    'colours each row by its level so the important ones are obvious without reading every line.
    'the colours are picked to suit whichever theme is on, otherwise dark mode would end up with
    'pale backgrounds and white writing on top of them
    Private Sub ColourRows()
        For Each row As DataGridViewRow In dgvLogs.Rows
            Dim level As String = CellText(row, "LogSeverity")

            If level = LogSecurity Then
                If DarkModeOn Then
                    row.DefaultCellStyle.BackColor = Color.FromArgb(74, 20, 40)
                    row.DefaultCellStyle.ForeColor = Color.White
                Else
                    row.DefaultCellStyle.BackColor = Color.FromArgb(255, 228, 232)
                    row.DefaultCellStyle.ForeColor = Color.FromArgb(120, 20, 40)
                End If
                row.DefaultCellStyle.Font = New Font("Segoe UI", 9.0!, FontStyle.Bold)

            ElseIf level = LogWarning Then
                If DarkModeOn Then
                    row.DefaultCellStyle.BackColor = Color.FromArgb(82, 62, 12)
                    row.DefaultCellStyle.ForeColor = Color.White
                Else
                    row.DefaultCellStyle.BackColor = Color.FromArgb(255, 244, 205)
                    row.DefaultCellStyle.ForeColor = Color.FromArgb(110, 80, 0)
                End If
                row.DefaultCellStyle.Font = New Font("Segoe UI", 9.0!, FontStyle.Bold)

            ElseIf level = LogChange Then
                If DarkModeOn Then
                    row.DefaultCellStyle.BackColor = Color.FromArgb(24, 60, 34)
                    row.DefaultCellStyle.ForeColor = Color.White
                Else
                    row.DefaultCellStyle.BackColor = Color.FromArgb(232, 245, 233)
                    row.DefaultCellStyle.ForeColor = Color.FromArgb(27, 94, 32)
                End If

            Else
                'routine entries are left alone apart from being greyed off a bit so they sit back
                If DarkModeOn Then
                    row.DefaultCellStyle.ForeColor = Color.FromArgb(160, 160, 160)
                Else
                    row.DefaultCellStyle.ForeColor = Color.Gray
                End If
            End If
        Next
    End Sub

    'says how much is on screen, and owns up when the cap has left some out
    Private Sub ShowCount()
        Dim showing As Integer = dgvLogs.Rows.Count

        If matchingRows = 0 Then
            lblCount.Text = "Nothing matches those filters"
        ElseIf showing < matchingRows Then
            lblCount.Text = "Showing the newest " & showing & " of " & matchingRows &
                            " entries - narrow the dates or the filters to see the rest"
        ElseIf matchingRows = 1 Then
            lblCount.Text = "1 entry"
        Else
            lblCount.Text = showing & " entries"
        End If
    End Sub

    'saves whatever is on screen to a csv file, so a manager can keep a copy of what was looked at
    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        If dgvLogs.Rows.Count = 0 Then
            MessageBox.Show("There is nothing on screen to export")
            Exit Sub
        End If

        Dim saveDialog As New SaveFileDialog
        saveDialog.Filter = "CSV files (*.csv)|*.csv"
        saveDialog.FileName = "AuditLog.csv"

        If saveDialog.ShowDialog() = DialogResult.OK Then
            Dim writer As New System.IO.StreamWriter(saveDialog.FileName)

            writer.WriteLine("When,Level,Area,User,What happened")

            For Each row As DataGridViewRow In dgvLogs.Rows
                writer.WriteLine(CsvField(CellText(row, "LogDateTime")) & "," &
                                 CsvField(CellText(row, "LogSeverity")) & "," &
                                 CsvField(CellText(row, "LogType")) & "," &
                                 CsvField(CellText(row, "LogUser")) & "," &
                                 CsvField(CellText(row, "LogMessage")))
            Next

            writer.Close()

            'the export itself is worth logging, somebody taking a copy of the audit trail out of
            'the system is exactly the sort of thing an audit trail should be recording
            WriteLog("LOGS", "Audit log exported, " & dgvLogs.Rows.Count & " entries", LogSecurity)
            MessageBox.Show("Audit log exported")
        End If
    End Sub

    'gets a cell as text, an empty cell would crash ToString on its own
    Private Function CellText(row As DataGridViewRow, columnName As String) As String
        If row.Cells(columnName).Value Is Nothing Then
            Return ""
        End If

        Return row.Cells(columnName).Value.ToString()
    End Function

End Class
