Imports System
Imports Microsoft.VisualBasic
Imports MySql.Data.MySqlClient

Public Class editAgency

    Private Sub editAgency_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        cbxGovtAccredStat.Items.AddRange({"Accredited", "Not Accredited", "Pending"})

        If Session.CurrentLoggedUser.userType = "Agency" Then
            Label1.Text = "Edit Profile"
        End If

        LoadAgencyDetails()
    End Sub

    Private Sub LoadAgencyDetails()
        Try
            Dim query As String = $"SELECT * FROM agency WHERE AgencyID = {Session.CurrentReferenceID}"
            readQuery(query)

            If cmdRead.Read() Then
                txtbxAgencyName.Text = cmdRead("AgencyName").ToString()
                txtbxLicenseNum.Text = cmdRead("LicenseNumber").ToString()
                txtbxCity.Text = cmdRead("City").ToString()
                txtbxZipcode.Text = cmdRead("ZipCode").ToString()
                txtbxState.Text = cmdRead("State").ToString()
                txtbxStreet.Text = cmdRead("Street").ToString()
                txtbxContactNum.Text = cmdRead("ContactNumber").ToString()
                txtbxEmail.Text = cmdRead("Email").ToString()
                txtbxUrl.Text = cmdRead("WebsiteURL").ToString()
                txtbxSpecialization.Text = cmdRead("Specialization").ToString()
                txtbxYearsOfOperation.Text = cmdRead("YearsOfOperation").ToString()
                cbxGovtAccredStat.SelectedItem = cmdRead("GovtAccreditation").ToString()
                dateLicenseExpDate.Value = CDate(cmdRead("LicenseExpiryDate"))
                txtbxNotes.Text = cmdRead("Notes").ToString()
            End If

            cmdRead.Close()
        Catch ex As Exception
            MsgBox("Error loading agency profile: " & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        ' Validation
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
        Dim govtAccred As String = cbxGovtAccredStat.Text
        Dim licenseExp As Date = dateLicenseExpDate.Value
        Dim notes As String = txtbxNotes.Text.Trim()

        ' SQL UPDATE
        Dim updateQuery As String = $"
            UPDATE agency
            SET AgencyName = '{name}', LicenseNumber = '{license}', City = '{city}',
                State = '{state}', Street = '{street}', ZipCode = '{zip}',
                ContactNumber = '{contact}', Email = '{email}', WebsiteURL = '{url}',
                Specialization = '{spec}', YearsOfOperation = '{years}',
                GovtAccreditation = '{govtAccred}', LicenseExpiryDate = '{licenseExp:yyyy-MM-dd}',
                Notes = '{notes}'
            WHERE agency_id = {Session.CurrentReferenceID}"

        Try
            readQuery(updateQuery)
            MsgBox("Agency profile updated successfully!", MsgBoxStyle.Information)
            Me.Close()
        Catch ex As Exception
            MsgBox("Error updating agency: " & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    ' Input Restrictions
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
