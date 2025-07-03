Imports MySql.Data.MySqlClient
Imports System.Security.Cryptography
Imports System.Text
Imports System.IO

Public Class UserRegistration

    Private userType As String

    Public Sub New(selectedUserType As String)
        InitializeComponent()
        userType = selectedUserType ' e.g. "OFW", "Agency", "Employer"
    End Sub

    Private Sub UserRegistration_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txtbxPass.PasswordChar = "●"c ' Hide password characters
    End Sub

    Private Sub btnCreateAccount_Click(sender As Object, e As EventArgs) Handles CreateAccBTN.Click
        Dim username As String = TxtbxUserName.Text.Trim()
        Dim email As String = TxtbxEmail.Text.Trim()
        Dim password As String = txtbxPass.Text.Trim()

        ' Validate input
        If String.IsNullOrEmpty(password) Then
            MessageBox.Show("Password is required.", "Missing Info", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If String.IsNullOrEmpty(username) AndAlso String.IsNullOrEmpty(email) Then
            MessageBox.Show("Enter either Username or Email.", "Missing Info", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If Not String.IsNullOrEmpty(username) AndAlso Not String.IsNullOrEmpty(email) Then
            MessageBox.Show("Enter only one: either Username or Email — not both.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Check if username or email exists
        Dim checkQuery As String = ""
        If Not String.IsNullOrEmpty(username) Then
            checkQuery = $"SELECT * FROM users WHERE username = '{username.Replace("'", "''")}'"
        Else
            checkQuery = $"SELECT * FROM users WHERE email = '{email.Replace("'", "''")}'"
        End If

        readQuery(checkQuery)
        If cmdRead.HasRows Then
            MessageBox.Show("Username or Email already exists.", "Duplicate", MessageBoxButtons.OK, MessageBoxIcon.Error)
            cmdRead.Close()
            conn.Close()
            Return
        End If
        cmdRead.Close()
        conn.Close()

        ' Encrypt password
        Dim encryptedPass As String = Encrypt(password)

        ' Insert user
        Dim insertQuery As String = $"INSERT INTO users (username, email, password, user_type, reference_id, status) VALUES (" &
            $"'{username.Replace("'", "''")}', '{email.Replace("'", "''")}', '{encryptedPass}', '{userType}', 0, 'Active')"
        readQuery(insertQuery)
        conn.Close()

        ' Get last inserted ID
        readQuery("SELECT LAST_INSERT_ID()")
        Dim newUserId As Integer = If(cmdRead.Read(), Convert.ToInt32(cmdRead(0)), 0)
        cmdRead.Close()
        conn.Close()

        ' Set session
        Session.CurrentLoggedUser.id = newUserId
        Session.CurrentLoggedUser.username = If(String.IsNullOrEmpty(username), email, username)
        Session.CurrentLoggedUser.userType = userType
        Session.CurrentReferenceID = 0

        ' Navigate to profile form
        Select Case userType
            Case "OFW"
                Session.LoggedInOfwID = 0
                Dim f As New addOfw()
                f.Show()
            Case "Agency"
                Session.LoggedInAgencyID = 0
                Dim f As New addAgency()
                f.Show()
            Case "Employer"
                Session.LoggedInEmployerID = 0
                Dim f As New addEmployer()
                f.Show()
            Case Else
                MessageBox.Show("Invalid user type.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
        End Select

        Me.Hide()
    End Sub

    Private Sub loginLink_Click(sender As Object, e As EventArgs) Handles lblLogin.Click
        Dim loginForm As New loginPage()
        loginForm.Show()
        Me.Hide()
    End Sub

    Public Function Encrypt(clearText As String) As String
        Dim EncryptionKey As String = "MAKV2SPBNI99212"
        Dim clearBytes As Byte() = Encoding.Unicode.GetBytes(clearText)
        Using encryptor As Aes = Aes.Create()
            Dim pdb As New Rfc2898DeriveBytes(EncryptionKey, New Byte() {
                &H49, &H76, &H61, &H6E, &H20, &H4D, &H65, &H64,
                &H76, &H65, &H64, &H65, &H76})
            encryptor.Key = pdb.GetBytes(32)
            encryptor.IV = pdb.GetBytes(16)
            Using ms As New MemoryStream()
                Using cs As New CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write)
                    cs.Write(clearBytes, 0, clearBytes.Length)
                    cs.Close()
                End Using
                Return Convert.ToBase64String(ms.ToArray())
            End Using
        End Using
    End Function
End Class
