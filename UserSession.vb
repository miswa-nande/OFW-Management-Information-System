Public Class UserSession
    Public Property id As Integer      ' This maps to user_accounts.user_accounts_id
    Public Property userType As String ' Can be "OFW", "Employer", "Agency", "Admin"
    Public Property fullName As String
End Class
