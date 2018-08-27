Public Class PawFile
    Public Property path As String = ""
    Public Property dt As Date
    Public Property ftpPath As String = ""
    Public Property size As Long = 0
    Public Property type As Integer
    Public Property locked As Boolean = False
    Public Property name As String = ""

    Public Enum Types
        Folder = 0
        File = 1
    End Enum

    Public Enum LockState
        Locked = True
        Unlocked = False
    End Enum
End Class
