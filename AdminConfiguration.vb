Imports System.IO

Public Class AdminConfiguration
    Private isEditing As Boolean = False
    Private selectedUserId As Integer = -1
    Private configFilePath As String = Path.Combine(Application.StartupPath, "config", "sql_config.txt")

    Private Sub AdminConfiguration_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            UpdateConnectionString()

            cmbUserType.Items.Clear()
            cmbUserType.Items.Add("Admin")
            cmbUserType.SelectedIndex = 0

            cmbStatus.Items.Clear()
            cmbStatus.Items.AddRange({"Active", "Inactive"})
            cmbStatus.SelectedIndex = 0

            LoadUsersToDGV()
            LoadSQLFieldsToForm()
            SaveSQLConnectionIfChanged()
            LoadConnectionConfigs()
        Catch ex As Exception
            MsgBox("Error loading form: " & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    ' Load Admin Users
    Private Sub LoadUsersToDGV()
        Try
            UpdateConnectionString()
            Dim query As String = "SELECT user_id, username, email, password, user_type, status, created_at FROM users WHERE user_type = 'Admin'"
            readQuery(query)
            Dim dt As New DataTable()
            dt.Load(cmdRead)

            With DataGridView1
                .DataSource = dt
                .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
                .AllowUserToAddRows = False
                .ReadOnly = True
            End With

            If DataGridView1.Columns.Contains("user_id") Then
                DataGridView1.Columns("user_id").Visible = False
            End If
        Catch ex As Exception
            MsgBox("Error loading users: " & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    ' Clear Text Fields
    Private Sub clearFields()
        txtUsername.Clear()
        txtEmail.Clear()
        txtPword.Clear()
        cmbStatus.SelectedIndex = 0
        btnAdduser.Text = "Add User"
        isEditing = False
        selectedUserId = -1
    End Sub

    ' Add or Update Admin
    Private Sub btnAdduser_Click(sender As Object, e As EventArgs) Handles btnAdduser.Click
        Try
            UpdateConnectionString()
            Dim username As String = txtUsername.Text.Trim()
            Dim email As String = txtEmail.Text.Trim()
            Dim rawPassword As String = txtPword.Text.Trim()
            Dim userType As String = "Admin"
            Dim status As String = cmbStatus.SelectedItem.ToString()

            If username = "" Or email = "" Then
                MsgBox("Username and Email are required.", MsgBoxStyle.Exclamation)
                Return
            End If

            If isEditing Then
                Dim updateQuery As String = $"UPDATE users SET username = '{username}', email = '{email}', status = '{status}' WHERE user_id = {selectedUserId}"
                readQuery(updateQuery)
                MsgBox("Admin updated successfully!", MsgBoxStyle.Information)
            Else
                If rawPassword = "" Then
                    MsgBox("Password is required.", MsgBoxStyle.Exclamation)
                    Return
                End If
                Dim encryptedPassword As String = Encrypt(rawPassword)
                Dim insertQuery As String = $"INSERT INTO users (username, email, password, user_type, status) VALUES ('{username}', '{email}', '{encryptedPassword}', '{userType}', '{status}')"
                readQuery(insertQuery)
                MsgBox("New admin user added successfully!", MsgBoxStyle.Information)
            End If

            LoadUsersToDGV()
            clearFields()
        Catch ex As Exception
            MsgBox("Error saving user: " & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    ' Edit Admin
    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        Try
            UpdateConnectionString()
            If DataGridView1.SelectedRows.Count = 0 Then
                MsgBox("Please select an admin from the table first.", MsgBoxStyle.Exclamation)
                Return
            End If

            Dim row As DataGridViewRow = DataGridView1.SelectedRows(0)
            selectedUserId = CInt(row.Cells("user_id").Value)

            Dim query As String = $"SELECT username, email, password, status FROM users WHERE user_id = {selectedUserId}"
            readQuery(query)

            If cmdRead.Read() Then
                txtUsername.Text = cmdRead("username").ToString()
                txtEmail.Text = cmdRead("email").ToString()
                Try
                    txtPword.Text = Decrypt(cmdRead("password").ToString())
                Catch ex As Exception
                    MsgBox("Unable to decrypt password: " & ex.Message, MsgBoxStyle.Critical)
                    txtPword.Text = ""
                End Try
                cmbStatus.SelectedItem = cmdRead("status").ToString()
            End If
            cmdRead.Close()

            btnAdduser.Text = "Save"
            isEditing = True
        Catch ex As Exception
            MsgBox("Error editing user: " & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    ' Delete Admin
    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        Try
            UpdateConnectionString()
            If DataGridView1.SelectedRows.Count = 0 Then
                MsgBox("Please select an admin to delete.", MsgBoxStyle.Exclamation)
                Return
            End If

            Dim row As DataGridViewRow = DataGridView1.SelectedRows(0)
            Dim userIdToDelete As Integer = CInt(row.Cells("user_id").Value)

            Dim confirm = MsgBox("Are you sure you want to delete this admin?", MsgBoxStyle.YesNo + MsgBoxStyle.Question)
            If confirm = MsgBoxResult.Yes Then
                Dim deleteQuery As String = $"DELETE FROM users WHERE user_id = {userIdToDelete}"
                readQuery(deleteQuery)
                MsgBox("Admin deleted successfully.", MsgBoxStyle.Information)
                LoadUsersToDGV()
                clearFields()
            End If
        Catch ex As Exception
            MsgBox("Error deleting user: " & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    ' Save SQL Config to Text File and check DB
    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Try
            Dim folder As String = Path.GetDirectoryName(configFilePath)
            If Not Directory.Exists(folder) Then Directory.CreateDirectory(folder)

            Using writer As New StreamWriter(configFilePath, False)
                writer.WriteLine("Localhost:" & txtLocalhost.Text.Trim())
                writer.WriteLine("Root:" & txtRoot.Text.Trim())
                writer.WriteLine("Password:" & txtPassword.Text.Trim())
                writer.WriteLine("DB_Name:" & txtDBname.Text.Trim())
            End Using

            MsgBox("SQL configuration saved to file successfully!", MsgBoxStyle.Information)

            SaveSQLConnectionIfChanged()
            LoadConnectionConfigs()

        Catch ex As Exception
            MsgBox("Error saving SQL config to file: " & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    ' Load SQL Config into Fields
    Private Sub LoadSQLFieldsToForm()
        Try
            If Not File.Exists(configFilePath) Then Exit Sub
            Dim lines = File.ReadAllLines(configFilePath)

            For Each line As String In lines
                If line.StartsWith("Localhost:") Then txtLocalhost.Text = line.Substring("Localhost:".Length)
                If line.StartsWith("Root:") Then txtRoot.Text = line.Substring("Root:".Length)
                If line.StartsWith("Password:") Then txtPassword.Text = line.Substring("Password:".Length)
                If line.StartsWith("DB_Name:") Then txtDBname.Text = line.Substring("DB_Name:".Length)
            Next
        Catch ex As Exception
            MsgBox("Error loading SQL config from file: " & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    ' Save to DB only if config changed
    Private Sub SaveSQLConnectionIfChanged()
        Try
            UpdateConnectionString()
            Dim currentHost As String = txtLocalhost.Text.Trim()
            Dim currentRoot As String = txtRoot.Text.Trim()
            Dim currentPass As String = txtPassword.Text.Trim()
            Dim currentDb As String = txtDBname.Text.Trim()

            Dim checkQuery As String = "SELECT hostname, root, password, database_name FROM sql_connections ORDER BY date_connected DESC LIMIT 1"
            readQuery(checkQuery)

            Dim needsSave As Boolean = True

            If cmdRead.Read() Then
                If cmdRead("hostname").ToString() = currentHost AndAlso
                   cmdRead("root").ToString() = currentRoot AndAlso
                   cmdRead("password").ToString() = currentPass AndAlso
                   cmdRead("database_name").ToString() = currentDb Then
                    needsSave = False
                End If
            End If
            cmdRead.Close()

            If needsSave Then
                Dim insertSql As String = "INSERT INTO sql_connections (hostname, root, password, database_name) VALUES " &
                                          $"('{currentHost}', '{currentRoot}', '{currentPass}', '{currentDb}')"
                readQuery(insertSql)
            End If
        Catch ex As Exception
            MsgBox("Error saving SQL config to DB: " & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    ' Load SQL Configs from DB
    Private Sub LoadConnectionConfigs()
        Try
            UpdateConnectionString()
            Dim dt As New DataTable()
            Dim query As String = "SELECT hostname, root, password, database_name, date_connected FROM sql_connections ORDER BY date_connected DESC"
            readQuery(query)
            dt.Load(cmdRead)

            DataGridView2.DataSource = dt
            DataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            DataGridView2.ReadOnly = True
            DataGridView2.AllowUserToAddRows = False

            DataGridView2.EnableHeadersVisualStyles = False
            DataGridView2.ColumnHeadersDefaultCellStyle.BackColor = Color.Black
            DataGridView2.ColumnHeadersDefaultCellStyle.ForeColor = Color.White

            For i As Integer = 0 To DataGridView2.Rows.Count - 1
                DataGridView2.Rows(i).DefaultCellStyle.BackColor = If(i = 0, Color.LightGreen, Color.LightCoral)
            Next
        Catch ex As Exception
            MsgBox("Error loading SQL connection configs: " & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    ' Navigation
    Private Sub btnDashboard_Click(sender As Object, e As EventArgs) Handles btnDashboard.Click
        Me.Hide()
        Dim newForm As New dashboard()
        newForm.ShowDialog()
        Me.Show()
    End Sub

    Private Sub btnOfw_Click(sender As Object, e As EventArgs) Handles btnOfw.Click
        Me.Hide()
        Dim newForm As New ofws()
        newForm.ShowDialog()
        Me.Show()
    End Sub

    Private Sub btnDeployments_Click(sender As Object, e As EventArgs) Handles btnDeployments.Click
        Me.Hide()
        Dim newForm As New deployments()
        newForm.ShowDialog()
        Me.Show()
    End Sub

    Private Sub btnAgencies_Click(sender As Object, e As EventArgs) Handles btnAgencies.Click
        Me.Hide()
        Dim newForm As New agencies()
        newForm.ShowDialog()
        Me.Show()
    End Sub

    Private Sub btnEmployers_Click(sender As Object, e As EventArgs) Handles btnEmployers.Click
        Me.Hide()
        Dim newForm As New employers()
        newForm.ShowDialog()
        Me.Show()
    End Sub

    Private Sub btnConfig_Click(sender As Object, e As EventArgs) Handles btnConfig.Click
        MsgBox("You are already in the Configuration form.", MsgBoxStyle.Information)
    End Sub
End Class