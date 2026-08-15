Imports System.Data.OleDb

Public Class frmLogs

    Private Const MaxRows As Integer = 500

    Private stillLoading As Boolean = True

    Private matchingRows As Integer = 0

    Private Sub frmLogs_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If UserAccessLevel <> 1 Then
            MessageBox.Show("Only a manager can see the audit log.", "Audit Log", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            WriteLog("LOGS", "Audit log refused, access level " & UserAccessLevel, LogSecurity)
            Me.Close()
            Exit Sub
        End If

        CommonFormStartup(Me)

        dtpFrom.Value = Date.Today.AddDays(-7)
        dtpTo.Value = Date.Today

        LoadTypeFilter()
        LoadUserFilter()
        LoadSeverityFilter()

        stillLoading = False

        Me.KeyPreview = True

        LoadLogs()
        txtSearch.Focus()
        WriteLog("LOGS", "Audit log form opened")
    End Sub

    Private Sub frmLogs_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.F5 Then
            LoadLogs()
        ElseIf e.KeyCode = Keys.Escape Then
            If txtSearch.Text <> "" Then
                txtSearch.Text = ""
            Else
                Me.Close()
            End If
        End If
    End Sub

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

    Private Sub LoadUserFilter()
        cboUser.Items.Clear()
        cboUser.Items.Add("All users")

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT DISTINCT LogUser FROM tblLogs ORDER BY LogUser"
            Dim rs As OleDbDataReader = SQLCmd.ExecuteReader()
            While rs.Read()
                If rs("LogUser").ToString() <> "" Then
                    cboUser.Items.Add(rs("LogUser").ToString())
                End If
            End While
            rs.Close()
            cn.Close()
        End If

        cboUser.SelectedIndex = 0
    End Sub

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
        Dim keepType As String = cboType.Text
        Dim keepUser As String = cboUser.Text

        stillLoading = True
        LoadTypeFilter()
        LoadUserFilter()
        stillLoading = False

        If cboType.Items.Contains(keepType) Then
            cboType.SelectedItem = keepType
        End If
        If cboUser.Items.Contains(keepUser) Then
            cboUser.SelectedItem = keepUser
        End If

        LoadLogs()
    End Sub

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

    Private Sub Filter_Changed(sender As Object, e As EventArgs) Handles cboType.SelectedIndexChanged,
        cboSeverity.SelectedIndexChanged, cboUser.SelectedIndexChanged, dtpFrom.ValueChanged, dtpTo.ValueChanged

        If stillLoading Then
            Exit Sub
        End If

        LoadLogs()
    End Sub

    Private Sub txtSearch_KeyDown(sender As Object, e As KeyEventArgs) Handles txtSearch.KeyDown
        If e.KeyCode = Keys.Enter Then
            LoadLogs()
            e.SuppressKeyPress = True
        End If
    End Sub

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

    Private Sub AddWhereParams(SQLCmd As OleDbCommand)
        SQLCmd.Parameters.AddWithValue("@FromDate", dtpFrom.Value.Date)
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

    Private Sub LoadLogs()
        Dim dt As New DataTable
        matchingRows = 0

        If DbConnect() Then
            Dim where As String = BuildWhere()

            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT COUNT(*) FROM tblLogs " & where
            AddWhereParams(SQLCmd)
            matchingRows = CInt(SQLCmd.ExecuteScalar())

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
                If DarkModeOn Then
                    row.DefaultCellStyle.ForeColor = Color.FromArgb(160, 160, 160)
                Else
                    row.DefaultCellStyle.ForeColor = Color.Gray
                End If
            End If
        Next
    End Sub

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

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        If dgvLogs.Rows.Count = 0 Then
            MessageBox.Show("There is nothing on screen to export", "Audit Log", MessageBoxButtons.OK, MessageBoxIcon.Warning)
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

            WriteLog("LOGS", "Audit log exported, " & dgvLogs.Rows.Count & " entries", LogSecurity)
            MessageBox.Show("Audit log exported", "Audit Log", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Function CellText(row As DataGridViewRow, columnName As String) As String
        If row.Cells(columnName).Value Is Nothing Then
            Return ""
        End If

        Return row.Cells(columnName).Value.ToString()
    End Function

End Class
