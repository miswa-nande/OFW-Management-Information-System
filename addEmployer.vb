Imports MySql.Data.MySqlClient

Public Class addEmployer
    Private Sub addEmployer_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Change button to "Save" if this is after registration
        If Session.CurrentLoggedUser.userType = "Employer" Then
            btnAdd.Text = "Save"
        End If
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        ' Input validation
        If Not IsNumeric(txtbxContactNum.Text.Trim()) Then
            MsgBox("Contact Number must be numeric.", MsgBoxStyle.Exclamation)
            Return
        End If

        If Not IsNumeric(txtbxZipcode.Text.Trim()) Then
            MsgBox("Zipcode must be numeric.", MsgBoxStyle.Exclamation)
            Return
        End If

        ' Collect values
        Dim fName As String = txtbxFName.Text.Trim()
        Dim mName As String = txtbxMName.Text.Trim()
        Dim lName As String = txtbxLName.Text.Trim()
        Dim contact As String = txtbxContactNum.Text.Trim()
        Dim email As String = txtbxEmail.Text.Trim()
        Dim company As String = txtbxCompanyName.Text.Trim()
        Dim industry As String = txtbxIndustry.Text.Trim()
        Dim street As String = txtbxStreet.Text.Trim()
        Dim city As String = txtbxCity.Text.Trim()
        Dim state As String = txtbxState.Text.Trim()
        Dim country As String = txtbxCountry.Text.Trim()
        Dim zipcode As String = txtbxZipcode.Text.Trim()

        Try
            openConn(db_name)

            ' Insert employer record
            Dim query As String = "
                INSERT INTO employer 
                (FirstName, MiddleName, LastName, ContactNumber, Email, CompanyName, Industry, Street, City, State, Country, ZipCode)
                VALUES 
                (@fName, @mName, @lName, @contact, @email, @company, @industry, @street, @city, @state, @country, @zipcode)"

            Using cmd As New MySqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@fName", fName)
                cmd.Parameters.AddWithValue("@mName", mName)
                cmd.Parameters.AddWithValue("@lName", lName)
                cmd.Parameters.AddWithValue("@contact", contact)
                cmd.Parameters.AddWithValue("@email", email)
                cmd.Parameters.AddWithValue("@company", company)
                cmd.Parameters.AddWithValue("@industry", industry)
                cmd.Parameters.AddWithValue("@street", street)
                cmd.Parameters.AddWithValue("@city", city)
                cmd.Parameters.AddWithValue("@state", state)
                cmd.Parameters.AddWithValue("@country", country)
                cmd.Parameters.AddWithValue("@zipcode", zipcode)

                cmd.ExecuteNonQuery()

                If Session.CurrentLoggedUser.userType = "Employer" Then
                    Dim insertedId As Integer = CInt(cmd.LastInsertedId)
                    Dim updateQuery As String = $"UPDATE users SET reference_id = {insertedId} WHERE user_id = {Session.CurrentLoggedUser.id}"
                    readQuery(updateQuery)
                    MsgBox("Profile saved. Redirecting to employer dashboard...", MsgBoxStyle.Information)

                    ' Optional: Show employer dashboard
                    Dim empForm As New empDashboard() ' ← change if your form is named differently
                    empForm.Show()
                    Me.Close()
                Else
                    MsgBox("Employer added successfully!", MsgBoxStyle.Information)
                    Me.Close()
                End If
            End Using
        Catch ex As Exception
            MsgBox("Error saving employer profile: " & ex.Message, MsgBoxStyle.Critical)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        If Session.CurrentLoggedUser.userType = "Employer" Then
            Dim loginForm As New loginPage()
            loginForm.Show()
        End If
        Me.Close()
    End Sub

    ' Input restrictions
    Private Sub txtbxContactNum_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtbxContactNum.KeyPress
        If Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsDigit(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtbxZipcode_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtbxZipcode.KeyPress
        If Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsDigit(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub
End Class
