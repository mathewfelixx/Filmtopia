Imports System.Data.OleDb

Module modVersionControl
    Public appversion As String
    Public Function GetVersion() As String
        Dim fullstring As String = "Filmtopia Cinema Management System v" & appversion
        Return fullstring
    End Function

    Public Sub LoadVersion()
        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT Version " &
                                 "FROM tblVersionControl"
            Dim result = SQLCmd.ExecuteScalar()
            If result IsNot Nothing AndAlso Not IsDBNull(result) Then
                appversion = CStr(result)
            Else
                appversion = "v1.0.0 [VC ERROR]"
            End If
            cn.Close()
        End If
    End Sub

    Public Sub SaveVersion()
        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "UPDATE tblVersionControl " &
                                 "SET Version = @Version"
            SQLCmd.Parameters.AddWithValue("@Version", appversion)
            SQLCmd.ExecuteNonQuery()
            cn.Close()
        End If
    End Sub

    Public Sub UpdateAllVersionLabels()
        frmLogin.lblVersion.Text = GetVersion()
        frmVersionControlUTIL.lblVersion.Text = GetVersion()
        frmMainForm.lblVersion.Text = GetVersion()
        frmFilms.lblVersion.Text = GetVersion()
        frmImportFilms.lblVersion.Text = GetVersion()
        frmScreens.lblVersion.Text = GetVersion()
        frmCustomers.lblVersion.Text = GetVersion()
        frmFoodItems.lblVersion.Text = GetVersion()
        frmBookings.lblVersion.Text = GetVersion()
        frmScreenings.lblVersion.Text = GetVersion()
        frmFoodOrder.lblVersion.Text = GetVersion()
        frmRefund.lblVersion.Text = GetVersion()
        frmSalesReport.lblVersion.Text = GetVersion()
        frmBookingSearch.lblVersion.Text = GetVersion()
        frmSettings.lblVersion.Text = GetVersion()
        frmMainMenuV2.lblVersion.Text = GetVersion()
        frmLogs.lblVersion.Text = GetVersion()
        frmKiosk.lblVersion.Text = GetVersion()
        frmUserOverview.lblVersion.Text = GetVersion()

    End Sub


End Module
