Imports MySql.Data.MySqlClient
Imports System.Security.Cryptography
Imports System.Text
Imports System.IO

Public Class UserRegistration
    Inherits Form

    ' Declare controls

    Private showPassBtn As CheckBox
    Private cmbUserType As ComboBox
    Private btnCreateAccount As Button
    Private loginLink As LinkLabel

    Public Sub New()
        ' Basic form properties
        Me.Text = "User Registration"
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.AutoSize = True
        Me.AutoSizeMode = AutoSizeMode.GrowAndShrink

        ' Initialize controls
        TxtbxUserName = New TextBox()
        TxtbxEmail = New TextBox()
        txtbxPass = New TextBox() With {.PasswordChar = "●"c}
        showPassBtn = New CheckBox() With {.Text = "Show Password"}
        cmbUserType = New ComboBox() With {.DropDownStyle = ComboBoxStyle.DropDownList}
        btnCreateAccount = New Button() With {.Text = "Create Account"}
        loginLink = New LinkLabel() With {.Text = "Already have an account? Log In", .AutoSize = True}

        cmbUserType.Items.AddRange({"OFW", "Agency", "Employer"})

        ' Create layout panel
        Dim layout As New TableLayoutPanel() With {
            .Dock = DockStyle.Fill,
            .AutoSize = True,
            .ColumnCount = 1,
            .Padding = New Padding(20),
            .AutoScroll = True
        }

        ' Add controls to layout
        layout.Controls.Add(New Label() With {.Text = "Username"})
        layout.Controls.Add(TxtbxUserName)

        layout.Controls.Add(New Label() With {.Text = "Email"})
        layout.Controls.Add(TxtbxEmail)

        layout.Controls.Add(New Label() With {.Text = "Password"})
        layout.Controls.Add(txtbxPass)

        layout.Controls.Add(showPassBtn)

        layout.Controls.Add(New Label() With {.Text = "Account Type"})
        layout.Controls.Add(cmbUserType)

        layout.Controls.Add(btnCreateAccount)
        layout.Controls.Add(loginLink)

        ' Add layout to form
        Me.Controls.Add(layout)

        ' Events
        AddHandler showPassBtn.CheckedChanged, AddressOf TogglePasswordVisibility
        AddHandler btnCreateAccount.Click, AddressOf btnCreateAccount_Click
        AddHandler loginLink.Click, AddressOf loginLink_Click
    End Sub

    Private Sub TogglePasswordVisibility(sender As Object, e As EventArgs)
        txtbxPass.PasswordChar = If(showPassBtn.Checked, ControlChars.NullChar, "●"c)
    End Sub

    Private Sub btnCreateAccount_Click(sender As Object, e As EventArgs)
        Dim username As String = TxtbxUserName.Text.Trim()
        Dim email As String = TxtbxEmail.Text.Trim()
        Dim password As String = txtbxPass.Text.Trim()
        Dim userType As String = cmbUserType.SelectedItem?.ToString()

        If String.IsNullOrEmpty(username) OrElse String.IsNullOrEmpty(email) OrElse String.IsNullOrEmpty(password) OrElse String.IsNullOrEmpty(userType) Then
            MessageBox.Show("All fields are required.", "Missing Info", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        readQuery($"SELECT * FROM users WHERE username = '{username.Replace("'", "''")}' OR email = '{email.Replace("'", "''")}'")
        If cmdRead.HasRows Then
            MessageBox.Show("Username or Email already exists.", "Duplicate", MessageBoxButtons.OK, MessageBoxIcon.Error)
            cmdRead.Close()
            Return
        End If
        cmdRead.Close()

        Dim encryptedPass As String = Encrypt(password)

        Dim insertQuery As String = $"
            INSERT INTO users (username, email, password, user_type, reference_id, status)
            VALUES ('{username.Replace("'", "''")}', '{email.Replace("'", "''")}', '{encryptedPass}', '{userType}', 0, 'Active')"
        readQuery(insertQuery)

        readQuery("SELECT LAST_INSERT_ID()")
        Dim newUserId As Integer = If(cmdRead.Read(), Convert.ToInt32(cmdRead(0)), 0)
        cmdRead.Close()

        Session.CurrentLoggedUser.id = newUserId
        Session.CurrentLoggedUser.username = username
        Session.CurrentLoggedUser.userType = userType
        Session.CurrentReferenceID = 0

        Select Case userType
            Case "OFW"
                Session.LoggedInOfwID = 0
                Dim f As New addOfw() : f.Show()
            Case "Agency"
                Session.LoggedInAgencyID = 0
                Dim f As New addAgency() : f.Show()
            Case "Employer"
                Session.LoggedInEmployerID = 0
                Dim f As New addEmployer() : f.Show()
        End Select

        Me.Hide()
    End Sub

    Private Sub loginLink_Click(sender As Object, e As EventArgs)
        Dim loginForm As New loginPage()
        loginForm.Show()
        Me.Hide()
    End Sub

    ' Encryption Function
    Public Function Encrypt(clearText As String) As String
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
