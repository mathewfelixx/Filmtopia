Imports System.Data.OleDb

Public Class frmScreens
    Private Const SeatsPerRow As Integer = 10

    Private Sub frmScreens_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadScreens()
    End Sub

    Private Sub LoadScreens()
        Dim SQLCmd As New OleDbCommand("SELECT ScreenID, ScreenName, ScreenCapacity " &
                                       "FROM tblScreen ORDER BY ScreenName")
        Dim dt As New DataTable
        If FillTable(SQLCmd, dt) Then
            dgvScreens.DataSource = dt
            dgvScreens.Columns("ScreenID").Visible = False
            dgvScreens.Columns("ScreenName").HeaderText = "Name"
            dgvScreens.Columns("ScreenCapacity").HeaderText = "Seats"
            dgvScreens.Columns("ScreenName").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        End If
    End Sub

    Private Function DetailsAreValid() As Boolean
        Dim valid As Boolean = False
        Dim seats As Integer

        If txtName.Text.Trim() = "" Then
            MessageBox.Show("Please enter the screen's name.", "Missing Details", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtName.Focus()

        ElseIf Not Integer.TryParse(txtSeats.Text.Trim(), seats) Then
            MessageBox.Show("The number of seats must be a whole number.", "Invalid Number of Seats", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtSeats.Focus()

        ElseIf seats < 10 Or seats > 260 Or seats Mod SeatsPerRow <> 0 Then
            MessageBox.Show("Seats are set out in rows of 10 lettered A to Z, so the number of seats must be a multiple of 10 from 10 to 260.", "Invalid Number of Seats", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtSeats.Focus()

        Else
            valid = True
        End If

        Return valid
    End Function

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        If DetailsAreValid() Then
            Dim screenName As String = txtName.Text.Trim()
            Dim seats As Integer = CInt(txtSeats.Text.Trim())
            Dim SQLCmd As New OleDbCommand("INSERT INTO tblScreen (ScreenName, ScreenCapacity) VALUES (@ScreenName, @ScreenCapacity)")
            SQLCmd.Parameters.AddWithValue("@ScreenName", screenName)
            SQLCmd.Parameters.AddWithValue("@ScreenCapacity", seats)
            If RunCommand(SQLCmd) Then
                WriteLog(LogScreen, "Added screen " & screenName & " with " & seats & " seats")
                LoadScreens()
                ClearFields()
            End If
        End If
    End Sub

    Private Sub ClearFields()
        txtName.Text = ""
        txtSeats.Text = ""
        txtName.Focus()
    End Sub
End Class
