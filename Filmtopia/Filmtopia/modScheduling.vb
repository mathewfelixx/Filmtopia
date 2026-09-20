Imports System.Data.OleDb

Module modScheduling

    Public Const OpeningMinutes As Integer = 600
    Public Const ClosingMinutes As Integer = 1440

    Public Function TimeToMinutes(hhmm As String) As Integer
        Dim hourPart As Integer = CInt(hhmm.Substring(0, 2))
        Dim minutePart As Integer = CInt(hhmm.Substring(3, 2))
        Return hourPart * 60 + minutePart
    End Function

    Public Function FilmDurationMinutes(filmID As Integer) As Integer
        Dim duration As Integer = 0
        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT FilmDuration FROM tblFilm WHERE FilmID = @FilmID"
            SQLCmd.Parameters.AddWithValue("@FilmID", filmID)
            Dim result As Object = SQLCmd.ExecuteScalar()
            If result IsNot Nothing Then
                duration = CInt(result)
            End If
            cn.Close()
        End If
        Return duration
    End Function

    Public Function ScreeningLengthMinutes(filmDurationMinutes As Integer) As Integer
        Return filmDurationMinutes + GetSettingInt("AdvertMinutes") + GetSettingInt("TurnaroundMinutes")
    End Function

    Public Function ScreeningClashes(screenID As Integer, screeningDate As Date, startMinutes As Integer, lengthMinutes As Integer, excludeScreeningID As Integer) As Boolean
        Dim newEnd As Integer = startMinutes + lengthMinutes

        Dim times(-1) As String
        Dim durations(-1) As Integer
        Dim count As Integer = 0

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT ScreeningTime, FilmDuration " &
                                 "FROM tblScreening INNER JOIN tblFilm ON tblScreening.FilmID = tblFilm.FilmID " &
                                 "WHERE ScreenID = @ScreenID AND ScreeningDate = @ScreeningDate AND ScreeningID <> @ExcludeID"
            SQLCmd.Parameters.AddWithValue("@ScreenID", screenID)
            SQLCmd.Parameters.AddWithValue("@ScreeningDate", screeningDate)
            SQLCmd.Parameters.AddWithValue("@ExcludeID", excludeScreeningID)
            Dim rs As OleDbDataReader = SQLCmd.ExecuteReader()
            Do While rs.Read()
                ReDim Preserve times(count)
                ReDim Preserve durations(count)
                times(count) = rs("ScreeningTime").ToString()
                durations(count) = CInt(rs("FilmDuration"))
                count = count + 1
            Loop
            rs.Close()
            cn.Close()
        End If

        Dim clash As Boolean = False
        For i As Integer = 0 To count - 1
            Dim existStart As Integer = TimeToMinutes(times(i))
            Dim existEnd As Integer = existStart + ScreeningLengthMinutes(durations(i))
            If startMinutes < existEnd And existStart < newEnd Then
                clash = True
            End If
        Next

        Return clash
    End Function

    Public Function RoundUpToInterval(minutes As Integer) As Integer
        Dim interval As Integer = GetSettingInt("RoundingIntervalMinutes")
        If interval <= 1 Then
            Return minutes
        End If

        Dim remainder As Integer = minutes Mod interval
        If remainder = 0 Then
            Return minutes
        End If

        Return minutes + (interval - remainder)
    End Function

    Public Function MinutesToTime(minutes As Integer) As String
        Dim hours As Integer = (minutes \ 60) Mod 24
        Dim mins As Integer = minutes Mod 60
        Return Format(hours, "00") & ":" & Format(mins, "00")
    End Function

    Public Function SuggestNextStartTime(screenID As Integer, screeningDate As Date, filmID As Integer, ByRef suggestedMinutes As Integer) As Boolean
        Dim length As Integer = ScreeningLengthMinutes(FilmDurationMinutes(filmID))
        Dim interval As Integer = GetSettingInt("RoundingIntervalMinutes")
        If interval < 1 Then
            interval = 1
        End If

        Dim times(-1) As String
        Dim durations(-1) As Integer
        Dim count As Integer = 0

        If DbConnect() Then
            Dim SQLCmd As New OleDbCommand
            SQLCmd.Connection = cn
            SQLCmd.CommandText = "SELECT ScreeningTime, FilmDuration " &
                                 "FROM tblScreening INNER JOIN tblFilm ON tblScreening.FilmID = tblFilm.FilmID " &
                                 "WHERE ScreenID = @ScreenID AND ScreeningDate = @ScreeningDate"
            SQLCmd.Parameters.AddWithValue("@ScreenID", screenID)
            SQLCmd.Parameters.AddWithValue("@ScreeningDate", screeningDate)
            Dim rs As OleDbDataReader = SQLCmd.ExecuteReader()
            Do While rs.Read()
                ReDim Preserve times(count)
                ReDim Preserve durations(count)
                times(count) = rs("ScreeningTime").ToString()
                durations(count) = CInt(rs("FilmDuration"))
                count = count + 1
            Loop
            rs.Close()
            cn.Close()
        End If

        Dim starts(-1) As Integer
        Dim ends(-1) As Integer
        If count > 0 Then
            ReDim starts(count - 1)
            ReDim ends(count - 1)
        End If
        For i As Integer = 0 To count - 1
            starts(i) = TimeToMinutes(times(i))
            ends(i) = starts(i) + ScreeningLengthMinutes(durations(i))
        Next

        Dim candidate As Integer = RoundUpToInterval(OpeningMinutes)
        Do While candidate + length <= ClosingMinutes
            Dim fits As Boolean = True
            For i As Integer = 0 To count - 1
                If candidate < ends(i) And starts(i) < candidate + length Then
                    fits = False
                End If
            Next
            If fits Then
                suggestedMinutes = candidate
                Return True
            End If
            candidate = candidate + interval
        Loop

        Return False
    End Function

End Module
