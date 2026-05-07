Public Class frmMessageBox
    Public Enum MsgType
        Success
        Warning
        ErrorMsg
        Confirmation
    End Enum

    Private _messagetype As MsgType
    Public Sub Setup(type As MsgType, message As String)
        _messagetype = type
        lblMessage.Text = message
        Select Case type
            Case MsgType.Success

            Case MsgType.Warning

            Case MsgType.ErrorMsg

            Case MsgType.Confirmation

        End Select

    End Sub

    Private Sub ConfigureButtons()
        Select Case _messagetype
            Case MsgType.Success, MsgType.Warning, MsgType.ErrorMsg

            Case MsgType.Confirmation

        End Select
    End Sub

    Private Sub btnNo_Click(sender As Object, e As EventArgs) Handles btnNo.Click
        Me.DialogResult = DialogResult.No
        Me.Hide()
    End Sub

    Private Sub btnYes_Click(sender As Object, e As EventArgs) Handles btnYes.Click
        Me.DialogResult = DialogResult.Yes
        Me.Hide()
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Hide()
    End Sub

    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        Me.DialogResult = DialogResult.OK
        Me.Hide()
    End Sub
End Class