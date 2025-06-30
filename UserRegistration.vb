Imports MySql.Data.MySqlClient
Imports System.Security.Cryptography
Imports System.Text
Imports System.IO

Public Class UserRegistration
    Inherits Form

    ' Controls
    Private EmailLBL As Label
    Private PasswordLBL As Label
    Private showPassBtn As CheckBox

    Private UserTypeLBL As Label
    Private cmbUserType As ComboBox

    Private btnCreateAccount As Button
    Private loginLink As LinkLabel

    Public Sub New()
        MyBase.New()
        SetupForm()
    End Sub

    Private Sub SetupForm()
        ' Form UI
        Me.Text = "User Registration"
        Me.Size = New Size(400, 520)
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.BackColor = Color.WhiteSmoke
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.MaximizeBox = False

        ' Username
        UsernameLBL = New Label() With {.Text = "Username", .Font = New Font("Segoe UI", 10, FontStyle.Bold), .Location = New Point(60, 40), .Size = New Size(100, 25)}
        TxtbxUserName = New TextBox() With {.Font = New Font("Segoe UI", 10), .Size = New Size(250, 25), .Location = New Point(60, 65)}

        ' OR
        LblOR = New Label() With {.Text = "or", .Font = New Font("Segoe UI", 9, FontStyle.Italic), .Location = New Point(185, 100), .Size = New Size(30, 20)}

        ' Email
        EmailLBL = New Label() With {.Text = "Email", .Font = New Font("Segoe UI", 10, FontStyle.Bold), .Location = New Point(60, 130), .Size = New Size(100, 25)}
        TxtbxEmail = New TextBox() With {.Font = New Font("Segoe UI", 10), .Size = New Size(250, 25), .Location = New Point(60, 155)}

        ' Password
        PasswordLBL = New Label() With {.Text = "Password", .Font = New Font("Segoe UI", 10, FontStyle.Bold), .Location = New Point(60, 190), .Size = New Size(100, 25)}
        txtbxPass = New TextBox() With {.Font = New Font("Segoe UI", 10), .Size = New Size(250, 25), .Location = New Point(60, 215), .PasswordChar = "●"}
        showPassBtn = New CheckBox() With {.Text = "Show Password", .Location = New Point(60, 245), .AutoSize = True}
        AddHandler showPassBtn.CheckedChanged, Sub()
                                                   txtbxPass.PasswordChar = If(showPassBtn.Checked, ControlChars.NullChar, "●"c)
                                               End Sub

        ' User type
        UserTypeLBL = New Label() With {.Text = "Account Type", .Font = New Font("Segoe UI", 10, FontStyle.Bold), .Location = New Point(60, 270)}
        cmbUserType = New ComboBox() With {.DropDownStyle = ComboBoxStyle.DropDownList, .Location = New Point(60, 295), .Size = New Size(250, 25)}
        cmbUserType.Items.AddRange({"OFW", "Agency", "Employer"})

        ' Create Account
        btnCreateAccount = New Button() With {
            .Text = "Create Account",
            .Font = New Font("Segoe UI", 10, FontStyle.Bold),
            .BackColor = Color.FromArgb(52, 152, 219),
            .ForeColor = Color.White,
            .FlatStyle = FlatStyle.Flat,
            .Size = New Size(250, 35),
            .Location = New Point(60, 340)
        }

        ' Login Link
        loginLink = New LinkLabel() With {
            .Text = "Already have an account? Log In",
            .Font = New Font("Segoe UI", 9, FontStyle.Regular),
            .Location = New Point(100, 390),
            .AutoSize = True,
            .LinkColor = Color.Blue
        }

        ' Add to Form
        Me.Controls.AddRange({UsernameLBL, TxtbxUserName, LblOR, EmailLBL, TxtbxEmail, PasswordLBL, txtbxPass, showPassBtn, UserTypeLBL, cmbUserType, btnCreateAccount, loginLink})

        ' Events
        AddHandler btnCreateAccount.Click, AddressOf Me.btnCreateAccount_Click
        AddHandler loginLink.Click, AddressOf Me.loginLink_Click
    End Sub

    Private Sub btnCreateAccount_Click(sender As Object, e As EventArgs)
        Dim username As String = TxtbxUserName.Text.Trim()
        Dim email As String = TxtbxEmail.Text.Trim()
        Dim password As String = txtbxPass.Text.Trim()
        Dim userType As String = cmbUserType.SelectedItem?.ToString()

        If String.IsNullOrEmpty(username) OrElse String.IsNullOrEmpty(email) OrElse String.IsNullOrEmpty(password) OrElse String.IsNullOrEmpty(userType) Then
            MsgBox("All fields are required.", MsgBoxStyle.Exclamation)
            Return
        End If

        ' Check for duplicates
        readQuery($"SELECT * FROM users WHERE username = '{username.Replace("'", "''")}' OR email = '{email.Replace("'", "''")}'")
        If cmdRead.HasRows Then
            MsgBox("Username or Email already exists.", MsgBoxStyle.Critical)
            cmdRead.Close()
            Return
        End If
        cmdRead.Close()

        ' Encrypt password
        Dim encryptedPass As String = Encrypt(password)

        ' Insert account
        Dim insertQuery As String = $"
            INSERT INTO users (username, email, password, user_type, reference_id, status)
            VALUES ('{username.Replace("'", "''")}', '{email.Replace("'", "''")}', '{encryptedPass}', '{userType}', 0, 'Active')"

        Try
            readQuery(insertQuery)
            MsgBox("Account created successfully!", MsgBoxStyle.Information)

            ' Redirect to corresponding profile page
            Select Case userType
                Case "OFW"
                    Dim ofwProfile As New addOfw() ' Load empty OFW profile form
                    ofwProfile.Show()
                Case "Agency"
                    Dim agencyProfile As New addAgency()
                    agencyProfile.Show()
                Case "Employer"
                    Dim employerProfile As New addEmployer()
                    employerProfile.Show()
            End Select

            Me.Hide()
        Catch ex As Exception
            MsgBox("Error: " & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub loginLink_Click(sender As Object, e As EventArgs)
        Dim loginForm As New loginPage()
        loginForm.Show()
        Me.Hide()
    End Sub

    ' AES Encryption
    Public Function Encrypt(ByVal clearText As String) As String
        Dim EncryptionKey As String = "MAKV2SPBNI99212"
        Dim clearBytes As Byte() = Encoding.Unicode.GetBytes(clearText)
        Using encryptor As Aes = Aes.Create()
            Dim pdb As New Rfc2898DeriveBytes(EncryptionKey, New Byte() {&H49, &H76, &H61, &H6E, &H20, &H4D, &H65, &H64, &H76, &H65, &H64, &H65, &H76})
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
