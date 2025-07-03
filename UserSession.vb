Public Class UserSession
    Public Property id As Integer            ' Maps to users.user_id
    Public Property userType As String       ' "OFW", "Employer", "Agency", etc.
    Public Property username As String       ' Username instead of fullName

    Public Sub New()
        id = 0
        userType = ""
        username = ""
    End Sub

    Public Sub New(userId As Integer, userType As String, username As String)
        Me.id = userId
        Me.userType = userType
        Me.username = username
    End Sub
End Class
