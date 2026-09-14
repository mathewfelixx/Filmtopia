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
        nudTurnaround.Value = GetSettingInt("TurnaroundMinutes")
        nudAdvert.Value = GetSettingInt("AdvertMinutes")
        nudRounding.Value = GetSettingInt("RoundingIntervalMinutes")
        nudPrice.Value = GetSettingCurrency("DefaultTicketPrice")
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        SetSettingValue("TurnaroundMinutes", CStr(CInt(nudTurnaround.Value)))
        SetSettingValue("AdvertMinutes", CStr(CInt(nudAdvert.Value)))
        SetSettingValue("RoundingIntervalMinutes", CStr(CInt(nudRounding.Value)))
        SetSettingValue("DefaultTicketPrice", Format(nudPrice.Value, "0.00"))

        MessageBox.Show("Settings saved.")
    End Sub

End Class
