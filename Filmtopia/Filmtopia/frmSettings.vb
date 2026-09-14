Imports System.Data.OleDb

Public Class frmSettings

    Private Sub frmSettings_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If UserAccessLevel <> 1 Then
            MessageBox.Show("Only a manager can change the system settings.")
            Me.Close()
            Exit Sub
        End If

        CommonFormStartup()
        LoadSettings()
        WriteLog("SETTING", "Settings form opened")
    End Sub

    Private Sub LoadSettings()
        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT SettingName, SettingValue, SettingDescription " &
                                 "FROM tblSetting ORDER BY SettingID"
            Dim da As New OleDbDataAdapter(SQLCmd)
            Dim dt As New DataTable
            da.Fill(dt)
            dgvSettings.DataSource = dt
            cn.Close()
        End If

        dgvSettings.Columns("SettingName").HeaderText = "Setting"
        dgvSettings.Columns("SettingName").ReadOnly = True
        dgvSettings.Columns("SettingName").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        dgvSettings.Columns("SettingValue").HeaderText = "Value"
        dgvSettings.Columns("SettingDescription").HeaderText = "Description"
        dgvSettings.Columns("SettingDescription").ReadOnly = True
        dgvSettings.Columns("SettingDescription").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        dgvSettings.DefaultCellStyle.WrapMode = DataGridViewTriState.True
        dgvSettings.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells

        WriteLog("SETTING", "Settings loaded")
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If Not SettingsAreValid() Then Exit Sub

        For Each row As DataGridViewRow In dgvSettings.Rows
            If row.IsNewRow Then Continue For
            Dim settingName As String = row.Cells("SettingName").Value.ToString()
            Dim settingValue As String = row.Cells("SettingValue").Value.ToString()
            SetSettingValue(settingName, settingValue)
        Next

        MessageBox.Show("Settings saved.")
        LoadSettings()
    End Sub

    Private Function SettingsAreValid() As Boolean
        For Each row As DataGridViewRow In dgvSettings.Rows
            If row.IsNewRow Then Continue For

            Dim settingName As String = row.Cells("SettingName").Value.ToString()
            Dim settingValue As String = ""
            If row.Cells("SettingValue").Value IsNot Nothing Then
                settingValue = row.Cells("SettingValue").Value.ToString()
            End If

            If settingName = "DefaultTicketPrice" Then
                If Not IsNumeric(settingValue) OrElse Val(settingValue) <= 0 Then
                    MessageBox.Show("The default ticket price must be a number greater than 0.")
                    Return False
                End If
            ElseIf settingName = "TurnaroundMinutes" Or settingName = "AdvertMinutes" Or settingName = "RoundingIntervalMinutes" Then
                If Not IsNumeric(settingValue) OrElse Val(settingValue) < 0 Then
                    MessageBox.Show(settingName & " must be a number of 0 or more.")
                    Return False
                End If
            End If
        Next

        Return True
    End Function

End Class
