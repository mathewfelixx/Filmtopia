Module modScreenings
    Public Const MinutesInADay As Integer = 1440

    Public Const OpeningTime As String = "10:00"
    Public Const ClosingTime As String = "23:30"
    Public Const RoundingMinutes As Integer = 5

    Public Function TimeToMinutes(timeText As String) As Integer
        Dim hours As Integer = CInt(timeText.Substring(0, 2))
        Dim minutes As Integer = CInt(timeText.Substring(3, 2))
        Return hours * 60 + minutes
    End Function

    Public Function MinutesToTime(minutes As Integer) As String
        Dim minutesIntoDay As Integer = minutes Mod MinutesInADay
        Dim hours As Integer = minutesIntoDay \ 60
        Dim minutesPastHour As Integer = minutesIntoDay Mod 60
        Return hours.ToString("00") & ":" & minutesPastHour.ToString("00")
    End Function

    Public Function RoundUpTime(minutes As Integer) As Integer
        While minutes Mod RoundingMinutes <> 0
            minutes = minutes + 1
        End While
        Return minutes
    End Function
End Module
