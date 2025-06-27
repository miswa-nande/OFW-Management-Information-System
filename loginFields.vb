Public Class loginFields
    Private selectedUserType As String

    ' Constructor to accept user type
    Public Sub New(userType As String)
        InitializeComponent()
        selectedUserType = userType
    End Sub

    Private Sub loginFields_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Session.CurrentLoggedUser.userType = selectedUserType
        txtbxId.Focus()
    End Sub

    Private Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        Dim userId As String = txtbxId.Text.Trim()
        Dim password As String = txtbxPassword.Text.Trim()
        Dim userType As String = selectedUserType

        If userId = "" OrElse password = "" Then
            MsgBox("Please enter both ID and password.", MsgBoxStyle.Exclamation)
            Exit Sub
        End If

        Dim query As String = String.Format("
            SELECT * FROM users 
            WHERE (username = '{0}' OR email = '{0}') 
            AND password = '{1}' 
            AND user_type = '{2}' 
            AND status = 'Active'",
            userId.Replace("'", "''"),
            password.Replace("'", "''"),
            userType.Replace("'", "''"))

        readQuery(query)

        If cmdRead IsNot Nothing AndAlso cmdRead.HasRows Then
            cmdRead.Read()

            ' Save session info
            Session.CurrentLoggedUser.id = CInt(cmdRead("user_id"))
            Session.CurrentLoggedUser.userType = userType

            Dim refID As Integer = CInt(cmdRead("reference_id"))

            ' Initialize full name variable
            Dim fullName As String = ""

            ' Determine full name based on user type and reference_id
            Dim nameQuery As String = ""
            Select Case userType
                Case "OFW"
                    nameQuery = $"SELECT CONCAT(FirstName, ' ', LastName) AS FullName FROM ofw WHERE OFWId = {refID}"
                    Session.LoggedInOfwID = refID
                Case "Agency"
                    nameQuery = $"SELECT AgencyName AS FullName FROM agency WHERE AgencyID = {refID}"
                    Session.LoggedInAgencyID = refID
                Case "Employer"
                    nameQuery = $"SELECT CONCAT(EmployerFirstName, ' ', EmployerLastName) AS FullName FROM employer WHERE EmployerID = {refID}"
                    Session.LoggedInEmployerID = refID
                Case "Admin"
                    Session.LoggedInAdminID = refID
                    fullName = cmdRead("username").ToString()
            End Select

            cmdRead.Close()

            ' Fetch full name if not admin
            If userType <> "Admin" Then
                readQuery(nameQuery)
                If cmdRead IsNot Nothing AndAlso cmdRead.Read() Then
                    fullName = cmdRead("FullName").ToString()
                End If
                cmdRead.Close()
            End If

            Session.CurrentLoggedUser.fullName = fullName

            ' Log the login event
            Session.Logs("User logged in.")

            ' Redirect to the correct dashboard
            Select Case userType
                Case "Admin"
                    Dim form As New dashboard()
                    form.Show()
                Case "Agency"
                    Dim form As New agencyDashboard()
                    form.Show()
                Case "Employer"
                    Dim form As New empDashboard()
                    form.Show()
                Case "OFW"
                    Dim form As New ofwProfile()
                    form.Show()
            End Select

            Me.Hide()
        Else
            cmdRead?.Close()
            MsgBox("Invalid ID or password for " & userType & ".", MsgBoxStyle.Critical)
        End If
    End Sub
End Class
