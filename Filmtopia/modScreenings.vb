Module modScreenings
    Public Const MinutesInADay As Integer = 1440

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
End Module
