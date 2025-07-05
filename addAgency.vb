Imports System
Imports System.Reflection.Emit
Imports Microsoft.VisualBasic
Imports MySql.Data.MySqlClient

Public Class addAgency
    Private Sub addAgency_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        cbxGovtAccredStat.Items.AddRange({"Accredited", "Not Accredited", "Pending"})

        ' Change button to "Save" if this is profile creation after registration
        If Session.CurrentLoggedUser.userType = "Agency" Then
            btnAdd.Text = "Save"
            Label1.Text = "Agency Profile"
        End If
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        ' Input validation
        If Not IsNumeric(txtbxZipcode.Text.Trim()) Then
            MsgBox("Zip Code must be a number.", MsgBoxStyle.Exclamation)
            Return
        End If

        If Not IsNumeric(txtbxContactNum.Text.Trim()) Then
            MsgBox("Contact Number must be numeric.", MsgBoxStyle.Exclamation)
            Return
        End If

        If Not IsNumeric(txtbxYearsOfOperation.Text.Trim()) Then
            MsgBox("Years of Operation must be numeric.", MsgBoxStyle.Exclamation)
            Return
        End If

        ' Collect inputs
        Dim name As String = txtbxAgencyName.Text.Trim()
        Dim license As String = txtbxLicenseNum.Text.Trim()
        Dim city As String = txtbxCity.Text.Trim()
        Dim state As String = txtbxState.Text.Trim()
        Dim street As String = txtbxStreet.Text.Trim()
        Dim zip As String = txtbxZipcode.Text.Trim()
        Dim contact As String = txtbxContactNum.Text.Trim()
        Dim email As String = txtbxEmail.Text.Trim()
        Dim url As String = txtbxUrl.Text.Trim()
        Dim spec As String = txtbxSpecialization.Text.Trim()
        Dim years As String = txtbxYearsOfOperation.Text.Trim()
        Dim govtAccred As String = cbxGovtAccredStat.SelectedItem?.ToString()
        Dim licenseExp As Date = dateLicenseExpDate.Value
        Dim notes As String = txtbxNotes.Text.Trim()

        Try
            openConn(db_name)

            ' Insert agency
            Dim query As String = "
                INSERT INTO agency 
                (AgencyName, AgencyLicenseNumber, City, State, Street, Zipcode, ContactNum, Email, WebsiteUrl, Specialization, YearsOfOperation, GovAccreditationStat, LicenseExpDate, Notes)
                VALUES
                (@name, @license, @city, @state, @street, @zip, @contact, @email, @url, @spec, @years, @govtAccred, @licenseExp, @notes)"

            Using cmd As New MySqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@name", name)
                cmd.Parameters.AddWithValue("@license", license)
                cmd.Parameters.AddWithValue("@city", city)
                cmd.Parameters.AddWithValue("@state", state)
                cmd.Parameters.AddWithValue("@street", street)
                cmd.Parameters.AddWithValue("@zip", zip)
                cmd.Parameters.AddWithValue("@contact", contact)
                cmd.Parameters.AddWithValue("@email", email)
                cmd.Parameters.AddWithValue("@url", url)
                cmd.Parameters.AddWithValue("@spec", spec)
                cmd.Parameters.AddWithValue("@years", years)
                cmd.Parameters.AddWithValue("@govtAccred", govtAccred)
                cmd.Parameters.AddWithValue("@licenseExp", licenseExp.ToString("yyyy-MM-dd"))
                cmd.Parameters.AddWithValue("@notes", notes)

                cmd.ExecuteNonQuery()

                If Session.CurrentLoggedUser.userType = "Agency" Then
                    Dim insertedId As Integer = CInt(cmd.LastInsertedId)
                    Dim updateQuery As String = $"UPDATE users SET reference_id = {insertedId} WHERE user_id = {Session.CurrentLoggedUser.id}"
                    readQuery(updateQuery)
                    MsgBox("Profile saved. Redirecting to agency dashboard...", MsgBoxStyle.Information)

                    ' Optionally redirect to agency profile
                    Dim agencyForm As New agcDashboard() ' 
                    agencyForm.Show()
                    Me.Close()
                Else
                    MsgBox("Agency record added successfully!", MsgBoxStyle.Information)
                    Me.Close()
                End If
            End Using
        Catch ex As Exception
            MsgBox("Error saving agency profile: " & ex.Message, MsgBoxStyle.Critical)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        If Session.CurrentLoggedUser.userType = "Agency" Then
            Dim loginForm As New loginPage()
            loginForm.Show()
        End If
        Me.Close()
    End Sub

    ' Input restrictions
    Private Sub txtbxZipcode_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtbxZipcode.KeyPress
        If Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsDigit(e.KeyChar) Then e.Handled = True
    End Sub

    Private Sub txtbxContactNum_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtbxContactNum.KeyPress
        If Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsDigit(e.KeyChar) Then e.Handled = True
    End Sub

    Private Sub txtbxYearsOfOperation_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtbxYearsOfOperation.KeyPress
        If Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsDigit(e.KeyChar) Then e.Handled = True
    End Sub
End Class
