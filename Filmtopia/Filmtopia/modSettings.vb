Imports System.Data.OleDb

Module modSettings
    Public Sub DarkMode()

    End Sub
    Public Sub LightMode()

    End Sub
    Public Sub ColourScheme()

    End Sub
    Public Sub ChangePassword(Username As String, OldPassword As String)

    End Sub
    Public Sub CreateBackup()

    End Sub

    Public Function GetSettingValue(settingName As String) As String
        Dim value As String = ""
        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT SettingValue FROM tblSetting WHERE SettingName = @SettingName"
            SQLCmd.Parameters.AddWithValue("@SettingName", settingName)
            Dim result As Object = SQLCmd.ExecuteScalar()
            If result IsNot Nothing Then
                value = result.ToString()
            End If
            cn.Close()
        End If
        Return value
    End Function

    Public Function GetSettingInt(settingName As String) As Integer
        Return CInt(Val(GetSettingValue(settingName)))
    End Function

    Public Function GetSettingCurrency(settingName As String) As Decimal
        Return CDec(Val(GetSettingValue(settingName)))
    End Function

    Public Sub SetSettingValue(settingName As String, settingValue As String)
        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "UPDATE tblSetting SET SettingValue = @SettingValue WHERE SettingName = @SettingName"
            SQLCmd.Parameters.AddWithValue("@SettingValue", settingValue)
            SQLCmd.Parameters.AddWithValue("@SettingName", settingName)
            SQLCmd.ExecuteNonQuery()
            cn.Close()
        End If
        WriteLog("SETTING", "Setting changed: " & settingName & " = " & settingValue)
    End Sub

End Module
