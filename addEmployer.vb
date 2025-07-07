Imports System
Imports Microsoft.VisualBasic
Imports MySql.Data.MySqlClient

Public Class addEmployer
    Private Sub addEmployer_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Session.CurrentLoggedUser.userType = "Employer" Then
            btnAdd.Text = "Save"
            Label1.Text = "Employer Profile"
        End If
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        ' Validate required fields
        If txtbxFName.Text.Trim() = "" OrElse txtbxLName.Text.Trim() = "" OrElse
           txtbxContactNum.Text.Trim() = "" OrElse txtbxEmail.Text.Trim() = "" OrElse
           txtbxCompanyName.Text.Trim() = "" OrElse txtbxIndustry.Text.Trim() = "" OrElse
           txtbxStreet.Text.Trim() = "" OrElse txtbxCity.Text.Trim() = "" OrElse
           txtbxState.Text.Trim() = "" OrElse txtbxCountry.Text.Trim() = "" OrElse
           txtbxZipcode.Text.Trim() = "" Then
            MsgBox("Please fill in all required fields.", MsgBoxStyle.Exclamation)
            Return
        End If

        If Not IsNumeric(txtbxContactNum.Text.Trim()) Then
            MsgBox("Contact Number must be numeric.", MsgBoxStyle.Exclamation)
            Return
        End If

        If Not IsNumeric(txtbxZipcode.Text.Trim()) Then
            MsgBox("Zip Code must be numeric.", MsgBoxStyle.Exclamation)
            Return
        End If

        ' Prepare values with escaping
        Dim fName = txtbxFName.Text.Trim().Replace("'", "''")
        Dim mName = txtbxMName.Text.Trim().Replace("'", "''")
        Dim lName = txtbxLName.Text.Trim().Replace("'", "''")
        Dim contact = txtbxContactNum.Text.Trim()
        Dim email = txtbxEmail.Text.Trim().Replace("'", "''")
        Dim company = txtbxCompanyName.Text.Trim().Replace("'", "''")
        Dim industry = txtbxIndustry.Text.Trim().Replace("'", "''")
        Dim street = txtbxStreet.Text.Trim().Replace("'", "''")
        Dim city = txtbxCity.Text.Trim().Replace("'", "''")
        Dim state = txtbxState.Text.Trim().Replace("'", "''")
        Dim country = txtbxCountry.Text.Trim().Replace("'", "''")
        Dim zipcode = txtbxZipcode.Text.Trim()

        ' Compose INSERT SQL
        Dim insertQuery As String = $"
            INSERT INTO employer 
            (FirstName, MiddleName, LastName, ContactNumber, Email, CompanyName, Industry, Street, City, State, Country, ZipCode)
            VALUES 
            ('{fName}', '{mName}', '{lName}', '{contact}', '{email}', 
             '{company}', '{industry}', '{street}', '{city}', '{state}', '{country}', '{zipcode}');
        "

        Try
            readQuery(insertQuery)

            ' Get last inserted ID
            Dim getIdQuery As String = "SELECT LAST_INSERT_ID()"
            readQuery(getIdQuery)
            Dim insertedId As Integer = 0
            If cmdRead.Read() Then
                insertedId = Convert.ToInt32(cmdRead(0))
            End If
            cmdRead.Close()

            ' If employer is self-registering
            If Session.CurrentLoggedUser.userType = "Employer" Then
                Dim updateUserQuery As String = $"UPDATE users SET reference_id = {insertedId} WHERE user_id = {Session.CurrentLoggedUser.id}"
                readQuery(updateUserQuery)

                MsgBox("Profile saved. Redirecting to employer dashboard...", MsgBoxStyle.Information)
                Dim empForm As New empDashboard() ' Update if your form has a different name
                empForm.Show()
                Me.Close()
            Else
                MsgBox("Employer added successfully!", MsgBoxStyle.Information)
                Me.Close()
            End If
        Catch ex As Exception
            MsgBox("Error saving employer profile: " & ex.Message, MsgBoxStyle.Critical)
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
