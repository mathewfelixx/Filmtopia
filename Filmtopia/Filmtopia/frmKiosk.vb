'the self service screen customers use themselves, so nothing on here is meant for a member of
'staff. it is one window with no border that fills the whole screen, and everything on it is made
'big enough to be pressed with a finger rather than clicked with a mouse
Public Class frmKiosk

    Private Sub frmKiosk_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CommonFormStartup(Me)
        LayoutKiosk()
        WriteLog("KIOSK", "Kiosk opened")
    End Sub

    'the header stretches across whatever screen this ends up running on, and the staff exit stays
    'tucked against the right hand edge. it is done here rather than in the designer because a
    'kiosk screen is not the same size as the one this was drawn on
    Private Sub LayoutKiosk()
        pnlHeader.Width = Me.ClientSize.Width
        btnExitKiosk.Left = pnlHeader.Width - btnExitKiosk.Width - 30
        lblVersion.Top = Me.ClientSize.Height - lblVersion.Height - 12
    End Sub

    Private Sub frmKiosk_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        LayoutKiosk()
    End Sub

    'a real kiosk has no way out for a customer, so this is the way a member of staff gets back to
    'the rest of the program. it asks first because pressing it by accident in front of a queue
    'would put the till screen up on a public display
    Private Sub btnExitKiosk_Click(sender As Object, e As EventArgs) Handles btnExitKiosk.Click
        Dim answer As DialogResult = MessageBox.Show("Close the kiosk and go back to the main menu?",
                                                     "Staff Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        If answer = DialogResult.Yes Then
            WriteLog("KIOSK", "Kiosk closed by staff")
            Me.Close()
        End If
    End Sub

End Class
