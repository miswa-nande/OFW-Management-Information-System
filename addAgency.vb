Imports MySql.Data.MySqlClient

Public Class addAgency
    Private Sub addAgency_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        cbxGovtAccredStat.Items.AddRange({"Accredited", "Not Accredited", "Pending"})

        If Session.CurrentLoggedUser.userType = "Agency" Then
            btnAdd.Text = "Save"
            Label1.Text = "Agency Profile"
        End If
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        ' Check if all required fields are filled
        If txtbxAgencyName.Text.Trim() = "" OrElse
           txtbxLicenseNum.Text.Trim() = "" OrElse
           txtbxCity.Text.Trim() = "" OrElse
           txtbxState.Text.Trim() = "" OrElse
           txtbxStreet.Text.Trim() = "" OrElse
           txtbxZipcode.Text.Trim() = "" OrElse
           txtbxContactNum.Text.Trim() = "" OrElse
           txtbxEmail.Text.Trim() = "" OrElse
           txtbxUrl.Text.Trim() = "" OrElse
           txtbxSpecialization.Text.Trim() = "" OrElse
           txtbxYearsOfOperation.Text.Trim() = "" OrElse
           cbxGovtAccredStat.SelectedIndex = -1 Then

            MsgBox("Please fill in all fields.", MsgBoxStyle.Exclamation)
            Return
        End If

        ' Numeric validations
        If Not IsNumeric(txtbxZipcode.Text.Trim()) Then
            MsgBox("Zip Code must be numeric.", MsgBoxStyle.Exclamation)
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

        ' Sanitize and assign
        Dim name = txtbxAgencyName.Text.Trim().Replace("'", "''")
        Dim license = txtbxLicenseNum.Text.Trim().Replace("'", "''")
        Dim city = txtbxCity.Text.Trim().Replace("'", "''")
        Dim state = txtbxState.Text.Trim().Replace("'", "''")
        Dim street = txtbxStreet.Text.Trim().Replace("'", "''")
        Dim zip = txtbxZipcode.Text.Trim()
        Dim contact = txtbxContactNum.Text.Trim()
        Dim email = txtbxEmail.Text.Trim().Replace("'", "''")
        Dim url = txtbxUrl.Text.Trim().Replace("'", "''")
        Dim spec = txtbxSpecialization.Text.Trim().Replace("'", "''")
        Dim years = txtbxYearsOfOperation.Text.Trim()
        Dim govtAccred = cbxGovtAccredStat.SelectedItem.ToString()
        Dim licenseExp = dateLicenseExpDate.Value.ToString("yyyy-MM-dd")
        Dim notes As String = If(String.IsNullOrWhiteSpace(txtbxNotes.Text), "NULL", $"'{txtbxNotes.Text.Trim().Replace("'", "''")}'")


        ' Query (uses readQuery)
        Dim insertQuery As String = $"
    INSERT INTO agency (
        AgencyName, AgencyLicenseNumber, City, State, Street, Zipcode, ContactNum,
        Email, WebsiteUrl, Specialization, YearsOfOperation,
        GovAccreditationStat, LicenseExpDate, Notes
    ) VALUES (
        '{name}', '{license}', '{city}', '{state}', '{street}', '{zip}', '{contact}',
        '{email}', '{url}', '{spec}', '{years}', '{govtAccred}', '{licenseExp}', {notes}
    )"


        Try
            readQuery(insertQuery)
            Dim getIdQuery As String = "SELECT LAST_INSERT_ID()"
            readQuery(getIdQuery)

            Dim insertedId As Integer = 0
            If cmdRead.Read() Then
                insertedId = cmdRead.GetInt32(0)
            End If
            cmdRead.Close()

            ' Link to user if coming from registration
            If Session.CurrentLoggedUser.userType = "Agency" Then
                Dim updateRefQuery As String = $"
                    UPDATE users SET reference_id = {insertedId} 
                    WHERE user_id = {Session.CurrentLoggedUser.id}"
                readQuery(updateRefQuery)

                MsgBox("Profile saved. Redirecting to agency dashboard...", MsgBoxStyle.Information)
                Dim agencyForm As New agcDashboard()
                agencyForm.Show()
                Me.Close()
            Else
                MsgBox("Agency added successfully.", MsgBoxStyle.Information)
                Me.Close()
            End If

        Catch ex As Exception
            MsgBox("Error: " & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        If Session.CurrentLoggedUser.userType = "Agency" Then
            Dim loginForm As New loginPage()
            loginForm.Show()
        End If
        Me.Close()
    End Sub

    ' Restrict input to digits
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
