Imports System.Data.OleDb
Imports System.Text
Module modMain
    Private Const DatabasePath As String = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source='Filmtopia.mdb';
                                           Persist Security Info=false;"
    Public cn As OleDbConnection
    Public LogedIn As Boolean = False
    Public UserAccessLevel As Integer = 99  ' indicates the prvilage level the user has 99 for customers
    Public CurrentLoginID As Long = 0  ' the LoginID of whoever is logged in, 0 means nobody is

    'the two states a screen can be in. a screen that is having work done to it is not deleted,
    'it is marked, the same way a cancelled booking is marked instead of being thrown away. that
    'matters because a deleted screen would take its seats and its history with it, and the
    'screenings that have already been on in it still have to add up in the sales report
    Public Const ScreenInService As String = "In service"
    Public Const ScreenOutOfService As String = "Out of service"

    'says whether a screen is open for business. it is asked before a screening is scheduled, so
    'the marking on the screens form actually stops something rather than just being a colour
    Public Function ScreenIsInService(screenID As Long) As Boolean
        Dim status As String = ScreenInService

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT ScreenStatus FROM tblScreen WHERE ScreenID = @ScreenID"
            SQLCmd.Parameters.AddWithValue("@ScreenID", CInt(screenID))
            Dim answer As Object = SQLCmd.ExecuteScalar()

            'a screen made before the status column existed has nothing in it, and an empty one
            'is treated as open rather than shut so nothing that used to work suddenly stops
            If answer IsNot Nothing AndAlso Not IsDBNull(answer) Then
                status = answer.ToString()
            End If

            cn.Close()
        End If

        Return status <> ScreenOutOfService
    End Function

    Public Function DbConnect() As Boolean
        Try
            cn = New OleDbConnection(DatabasePath)
            cn.Open()
            '   MessageBox.Show("Opend Successfully ")
            Return True
        Catch ex As Exception
            MessageBox.Show("Unable to open the database. " & ex.Message, "Database", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function
    'same idea as DbConnect but it does not show a message if it cannot get in, and it closes
    'the connection again straight away. it is for things that run on a timer, where a popup
    'appearing every minute would make the program impossible to use
    Public Function DbConnectQuiet() As Boolean
        Try
            cn = New OleDbConnection(DatabasePath)
            cn.Open()
            cn.Close()
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function Encrypt(PlainText As String) As String
        'Function takes plain text concerts it into Unitext8 (UTF8) and uses VB built encrption function (basically it
        'XORS with key called UTF8) 
        ' it will return encrypted string called cipertext 

        Dim CipherText As String = PlainText

        If PlainText.Length = 0 Then
            MessageBox.Show("There is nothing to encrypt", "Encryption", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Else
            Dim dat As Byte() = New Byte(PlainText.Length - 1) {}
            dat = System.Text.Encoding.UTF8.GetBytes(PlainText)
            CipherText = Convert.ToBase64String(dat)
        End If
        Return CipherText
    End Function

    Public Function Decrypt(CipherText As String) As String
        Dim PlainText As String = CipherText

        If CipherText.Length = 0 Then
            MessageBox.Show("There is nothing to decrypt", "Encryption", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Else

            Dim strDecrypt As New System.Text.UTF8Encoding
            Dim UTF_Decrypt As System.Text.Decoder = strDecrypt.GetDecoder
            Dim uData As Byte() = Convert.FromBase64String(CipherText)
            Dim CharNum As Integer = UTF_Decrypt.GetCharCount(uData, 0, uData.Length) ' number of char in ciphertext
            Dim Decrypt_Char As Char() = New Char(CharNum - 1) {}
            UTF_Decrypt.GetChars(uData, 0, uData.Length, Decrypt_Char, 0)
            PlainText = New String(Decrypt_Char)
        End If

        Return PlainText
    End Function

    Public Sub CommonFormStartup(frm As Form)
        LoadVersion()
        UpdateAllVersionLabels()
        ColourScheme(frm)

    End Sub

    'wraps a value in quotes for a csv file, and doubles up any quotes already in it.
    'without this a name or a film title containing a comma would be split into two columns
    'when the file was opened in Excel. it lives here because more than one form exports now
    Public Function CsvField(value As String) As String
        Return """" & value.Replace("""", """""") & """"
    End Function

    'puts a short line on the form saying what just happened. adding, changing and deleting a record
    'only ever wrote a log entry before, so on screen nothing moved except the grid and there was no
    'telling a save that worked from a button that had not been pressed properly. the colour comes
    'from the theme so it still reads in dark mode. the label is emptied again by the form's
    'ClearFields, so the message only lasts until the next thing is started
    Public Sub SayDone(lbl As Label, message As String)
        lbl.Text = message
        lbl.ForeColor = AccentFore
    End Sub

    'turns what somebody typed into a whole number. Val is used rather than CInt on its own because
    'it copes with the box being empty or having letters in it, but Val hands back a Double, and
    'CInt on a Double too big for an Integer throws instead of coming back with anything. pasting a
    'long number into the quantity box was enough to bring the program down, so anything out of
    'range comes back as 0 and gets refused by the validation like any other silly value would
    Public Function SafeInt(text As String) As Integer
        Dim number As Double = Val(text)

        If number > Integer.MaxValue Or number < Integer.MinValue Then
            Return 0
        End If

        Return CInt(number)
    End Function

    'gets a cell as text. an empty cell holds Nothing rather than an empty string, so calling
    'ToString on it straight off falls over
    Public Function CellAsText(row As DataGridViewRow, columnIndex As Integer) As String
        If row.Cells(columnIndex).Value Is Nothing Then
            Return ""
        End If

        Return row.Cells(columnIndex).Value.ToString()
    End Function

    'saves whatever is showing in a grid out to a csv file. it walks the grid's own columns rather
    'than having the column names typed in, so a screen that gains a column gains it in the export
    'too instead of quietly leaving it out. hidden columns are left out because they are not part
    'of what the user is looking at. returns True only if a file was actually written, so the form
    'that called it knows whether there is anything worth logging
    Public Function ExportGridToCsv(dgv As DataGridView, defaultFileName As String, title As String) As Boolean
        If dgv.Rows.Count = 0 Then
            MessageBox.Show("There is nothing on screen to export", title, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If

        Dim saveDialog As New SaveFileDialog
        saveDialog.Filter = "CSV files (*.csv)|*.csv"
        saveDialog.FileName = defaultFileName
        'without this the whole program is left sat in whatever folder was last saved to
        saveDialog.RestoreDirectory = True

        If saveDialog.ShowDialog() <> DialogResult.OK Then
            Return False
        End If

        Dim writer As New System.IO.StreamWriter(saveDialog.FileName)
        Dim line As String = ""

        'the headings come off the grid so the file says the same as the screen does
        For Each col As DataGridViewColumn In dgv.Columns
            If col.Visible Then
                If line <> "" Then
                    line = line & ","
                End If
                line = line & CsvField(col.HeaderText)
            End If
        Next
        writer.WriteLine(line)

        For Each row As DataGridViewRow In dgv.Rows
            line = ""
            For Each col As DataGridViewColumn In dgv.Columns
                If col.Visible Then
                    If line <> "" Then
                        line = line & ","
                    End If
                    line = line & CsvField(CellAsText(row, col.Index))
                End If
            Next
            writer.WriteLine(line)
        Next

        writer.Close()
        Return True
    End Function
End Module
