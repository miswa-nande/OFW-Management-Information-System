Public Module Session
    ' OFW Session
    Public LoggedInOfwID As Integer

    ' Employer Session
    Public LoggedInEmployerID As Integer

    ' Agency Session
    Public LoggedInAgencyID As Integer

    ' Admin Session
    Public LoggedInAdminID As Integer

    ' General User Info for Logging
    Public CurrentLoggedUser As New UserSession

    ' Store the detailed reference ID from users.reference_id column
    Public CurrentReferenceID As Integer

    ' Logs transactions with optional event name (default is "*_Click")
    Public Sub Logs(ByVal transaction As String, Optional ByVal events As String = "*_Click")
        Try
            ' Use parameterized queries if possible for security; 
            ' if not, at least sanitize inputs as you do here.
            readQuery(String.Format("INSERT INTO logs (dt, user_id, event, transactions) " &
                "VALUES (NOW(), {0}, '{1}', '{2}')",
                CurrentLoggedUser.id,
                events.Replace("'", "''"),
                transaction.Replace("'", "''")))
        Catch ex As Exception
            MsgBox("Log Error: " & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub
End Module
