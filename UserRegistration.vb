Imports MySql.Data.MySqlClient
Imports System.Security.Cryptography
Imports System.Text
Imports System.IO

Public Class UserRegistration

    Private userType As String

    Private Sub UserRegistration_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txtbxPass.PasswordChar = "●"c
        userType = Session.CurrentLoggedUser.userType ' Use session variable
    End Sub

    Private Sub CreateAccBTN_Click(sender As Object, e As EventArgs) Handles CreateAccBTN.Click
        Dim username As String = TxtbxUserName.Text.Trim()
        Dim email As String = TxtbxEmail.Text.Trim()
        Dim password As String = txtbxPass.Text.Trim()

        ' 1. Validate input
        If String.IsNullOrEmpty(password) Then
            MessageBox.Show("Password is required.", "Missing Info", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If String.IsNullOrEmpty(username) OrElse String.IsNullOrEmpty(email) Then
            MessageBox.Show("Both Username and Email are required.", "Missing Info", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' 2. Check if username OR email already exists
        Dim checkQuery As String = $"
            SELECT * FROM users 
            WHERE username = '{username.Replace("'", "''")}' 
               OR email = '{email.Replace("'", "''")}'"
        readQuery(checkQuery)

        If cmdRead.HasRows Then
            MessageBox.Show("Username or Email already exists.", "Duplicate", MessageBoxButtons.OK, MessageBoxIcon.Error)
            cmdRead.Close()
            conn.Close()
            Return
        End If

        cmdRead.Close()
        conn.Close()



        ' 3. Encrypt password
        Dim encryptedPass As String = Encrypt(password)

        ' 4. Prepare username/email values (insert NULL if empty)
        Dim usernameValue As String = If(String.IsNullOrEmpty(username), "NULL", $"'{username.Replace("'", "''")}'")
        Dim emailValue As String = If(String.IsNullOrEmpty(email), "NULL", $"'{email.Replace("'", "''")}'")

        ' 5. Insert into users
        Dim insertQuery As String = $"
    INSERT INTO users (username, email, password, user_type, reference_id, status)
    VALUES ({usernameValue}, {emailValue}, '{encryptedPass}', '{userType}', 0, 'Active')"
        readQuery(insertQuery)
        conn.Close()

        ' 6. Get user_id
        readQuery("SELECT LAST_INSERT_ID()")
        Dim newUserId As Integer = If(cmdRead.Read(), Convert.ToInt32(cmdRead(0)), 0)
        cmdRead.Close()
        conn.Close()


        ' 7. Redirect to profile form
        ' 7. Redirect to profile form or dashboard
        Select Case userType
            Case "OFW"
                Session.LoggedInOfwID = 0
                Dim f As New addOfw()
                f.ShowDialog()
            Case "Agency"
                Session.LoggedInAgencyID = 0
                Dim f As New addAgency()
                f.ShowDialog()
            Case "Employer"
                Session.LoggedInEmployerID = 0
                Dim f As New addEmployer()
                f.ShowDialog()
            Case "Admin"
                Session.LoggedInAdminID = newUserId
                Session.CurrentLoggedUser.id = newUserId
                Session.CurrentLoggedUser.username = If(Not String.IsNullOrEmpty(username), username, email)
                Session.CurrentLoggedUser.userType = "Admin"
                Session.CurrentReferenceID = 0
                Dim f As New dashboard()
                f.Show()
            Case Else
                MessageBox.Show("Unknown user type.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
        End Select

        Me.Hide()


        Me.Hide()
    End Sub

    Private Sub lblLogin_Click(sender As Object, e As EventArgs) Handles lblLogin.Click
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
