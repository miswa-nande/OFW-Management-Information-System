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

        ' Only check for admin signup visibility if Admin is selected
        If selectedUserType = "Admin" Then
            Dim query As String = "SELECT COUNT(*) FROM users WHERE user_type = 'Admin'"
            readQuery(query)

            If cmdRead IsNot Nothing AndAlso cmdRead.Read() Then
                Dim adminCount As Integer = CInt(cmdRead(0))
                If adminCount > 0 Then
                    Label4.Visible = False ' Sign Up
                    Label3.Visible = False ' Any other admin-only label
                Else
                    Label4.Visible = True
                    Label3.Visible = True
                End If
            End If

            cmdRead?.Close()
        End If
    End Sub


    Private Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        Dim userId As String = txtbxId.Text.Trim()
        Dim password As String = txtbxPassword.Text.Trim()
        Dim userType As String = selectedUserType

        If userId = "" OrElse password = "" Then
            MsgBox("Please enter both ID and password.", MsgBoxStyle.Exclamation)
            Exit Sub
        End If

        Dim encryptedPass As String = Encrypt(password)

        Dim query As String = String.Format(
            "SELECT * FROM users WHERE (username = '{0}' OR email = '{0}') AND password = '{1}' AND user_type = '{2}' AND status = 'Active'",
            userId.Replace("'", "''"),
            encryptedPass.Replace("'", "''"),
            userType.Replace("'", "''"))

        readQuery(query)

        If cmdRead IsNot Nothing AndAlso cmdRead.HasRows Then
            cmdRead.Read()

            Session.CurrentLoggedUser.id = CInt(cmdRead("user_id"))
            Session.CurrentLoggedUser.userType = userType
            Session.CurrentReferenceID = CInt(cmdRead("reference_id"))

            Dim refID As Integer = Session.CurrentReferenceID
            Dim fullName As String = ""
            Dim nameQuery As String = ""

            Select Case userType
                Case "OFW"
                    Session.LoggedInOfwID = refID
                    nameQuery = $"SELECT CONCAT(FirstName, ' ', LastName) AS FullName FROM ofw WHERE OFWId = {refID}"
                Case "Agency"
                    Session.LoggedInAgencyID = refID
                    nameQuery = $"SELECT AgencyName AS FullName FROM agency WHERE AgencyID = {refID}"
                Case "Employer"
                    Session.LoggedInEmployerID = refID
                    nameQuery = $"SELECT CONCAT(EmployerFirstName, ' ', EmployerLastName) AS FullName FROM employer WHERE EmployerID = {refID}"
                Case "Admin"
                    Session.LoggedInAdminID = refID
                    fullName = cmdRead("username").ToString()
            End Select

            cmdRead.Close()

            If userType <> "Admin" Then
                readQuery(nameQuery)
                If cmdRead IsNot Nothing AndAlso cmdRead.Read() Then
                    fullName = cmdRead("FullName").ToString()
                End If
                cmdRead.Close()
            End If

            Session.CurrentLoggedUser.username = fullName
            Session.Logs("User logged in.")

            Select Case userType
                Case "Admin"
                    Dim form As New dashboard()
                    form.Show()

                Case "Agency"
                    If refID > 0 Then
                        readQuery($"SELECT * FROM agency WHERE AgencyID = {refID}")
                        If cmdRead.HasRows Then
                            cmdRead.Close()
                            Dim form As New agcDashboard()
                            form.Show()
                        Else
                            cmdRead.Close()
                            Dim form As New addAgency()
                            form.ShowDialog()
                        End If
                    Else
                        Dim form As New addAgency()
                        form.ShowDialog()
                    End If

                Case "Employer"
                    If refID > 0 Then
                        readQuery($"SELECT * FROM employer WHERE EmployerID = {refID}")
                        If cmdRead.HasRows Then
                            cmdRead.Close()
                            Dim form As New empDashboard()
                            form.Show()
                        Else
                            cmdRead.Close()
                            Dim form As New addEmployer()
                            form.ShowDialog()
                        End If
                    Else
                        Dim form As New addEmployer()
                        form.ShowDialog()
                    End If

                Case "OFW"
                    If refID > 0 Then
                        readQuery($"SELECT * FROM ofw WHERE OFWId = {refID}")
                        If cmdRead.HasRows Then
                            cmdRead.Close()
                            Dim form As New ofwProfile()
                            form.Show()
                        Else
                            cmdRead.Close()
                            Dim form As New addOfw()
                            form.ShowDialog()
                        End If
                    Else
                        Dim form As New addOfw()
                        form.ShowDialog()
                    End If
            End Select

            Me.Close()
        Else
            cmdRead?.Close()
            MsgBox("Invalid ID or password for " & userType & ".", MsgBoxStyle.Critical)
        End If
    End Sub

    Private Sub LabelSignUp_Click(sender As Object, e As EventArgs) Handles Label4.Click
        Dim RegForm As New UserRegistration()
        RegForm.Show()
        Me.Hide()
    End Sub
End Class
