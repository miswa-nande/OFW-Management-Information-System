Public Class AdminConfiguration
    Private isEditing As Boolean = False
    Private selectedUserId As Integer = -1

    Private Sub AdminConfiguration_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        cmbUserType.Items.Clear()
        cmbUserType.Items.Add("Admin")
        cmbUserType.SelectedIndex = 0

        cmbStatus.Items.Clear()
        cmbStatus.Items.AddRange({"Active", "Inactive"})
        cmbStatus.SelectedIndex = 0

        LoadUsersToDGV()
    End Sub

    Private Sub LoadUsersToDGV()
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
    End Sub

    Private Sub clearFields()
        txtUsername.Clear()
        txtEmail.Clear()
        txtPword.Clear()
        cmbStatus.SelectedIndex = 0
        btnAdduser.Text = "Add User"
        isEditing = False
        selectedUserId = -1
    End Sub

    Private Sub btnAdduser_Click(sender As Object, e As EventArgs) Handles btnAdduser.Click
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
            ' Save updated admin
            Dim updateQuery As String = $"UPDATE users SET username = '{username}', email = '{email}', status = '{status}' WHERE user_id = {selectedUserId}"
            readQuery(updateQuery)
            MsgBox("Admin updated successfully!", MsgBoxStyle.Information)
        Else
            ' Add new admin
            If rawPassword = "" Then
                MsgBox("Password is required.", MsgBoxStyle.Exclamation)
                Return
            End If
            Dim encryptedPassword As String = Encrypt(rawPassword)

            Dim insertQuery As String = String.Format(
                "INSERT INTO users (username, email, password, user_type, status) " &
                "VALUES ('{0}', '{1}', '{2}', '{3}', '{4}')",
                username, email, encryptedPassword, userType, status)

            readQuery(insertQuery)
            MsgBox("New admin user added successfully!", MsgBoxStyle.Information)
        End If

        LoadUsersToDGV()
        clearFields()
    End Sub

    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
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
                MsgBox("Unable to decrypt password. It may have been stored incorrectly." & vbCrLf & ex.Message,
                   MsgBoxStyle.Critical)
                txtPword.Text = ""
            End Try

            cmbStatus.SelectedItem = cmdRead("status").ToString()
        End If
        cmdRead.Close()

        btnAdduser.Text = "Save"
        isEditing = True
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
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
    End Sub

    ' NAVIGATION
    Private Sub btnDashboard_Click(sender As Object, e As EventArgs) Handles btnDashboard.Click
        Dim newForm As New dashboard()
        newForm.Show()
        Me.Hide()
    End Sub

    Private Sub btnOfw_Click(sender As Object, e As EventArgs) Handles btnOfw.Click
        Dim newForm As New ofws()
        newForm.Show()
        Me.Hide()
    End Sub

    Private Sub btnDeployments_Click(sender As Object, e As EventArgs) Handles btnDeployments.Click
        Dim newForm As New deployments()
        newForm.Show()
        Me.Hide()
    End Sub

    Private Sub btnAgencies_Click(sender As Object, e As EventArgs) Handles btnAgencies.Click
        Dim newForm As New agencies()
        newForm.Show()
        Me.Hide()
    End Sub

    Private Sub btnEmployers_Click(sender As Object, e As EventArgs) Handles btnEmployers.Click
        Dim newForm As New employers()
        newForm.Show()
        Me.Hide()
    End Sub

    Private Sub btnConfig_Click(sender As Object, e As EventArgs) Handles btnConfig.Click
        Dim newForm As New AdminConfiguration()
        newForm.Show()
        Me.Hide()
    End Sub
End Class
