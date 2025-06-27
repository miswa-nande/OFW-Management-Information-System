Public Class UserSession
    Public Property id As Integer      ' Maps to users.user_id
    Public Property userType As String ' Possible values: "OFW", "Employer", "Agency", "Admin"
    Public Property fullName As String

    Public Sub New()
        id = 0
        userType = ""
        fullName = ""
    End Sub

    Public Sub New(userId As Integer, userType As String, fullName As String)
        Me.id = userId
        Me.userType = userType
        Me.fullName = fullName
    End Sub
End Class
