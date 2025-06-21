Public Class addAgency
    Private Sub addAgency_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        cbxGovtAccredStat.Items.AddRange({"Accredited", "Not Accredited", "Pending"})
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        ' Validation
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

        ' Collect data
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
        Dim govtAccred As String = cbxGovtAccredStat.SelectedItem.ToString()
        Dim licenseExp As Date = dateLicenseExpDate.Value
        Dim notes As String = txtbxNotes.Text.Trim()

        ' SQL Insert
        Dim query As String = $"
            INSERT INTO agency (AgencyName, LicenseNumber, City, State, Street, ZipCode, ContactNumber, Email, WebsiteURL, Specialization, YearsOfOperation, GovtAccreditation, LicenseExpiryDate, Notes)
            VALUES ('{name}', '{license}', '{city}', '{state}', '{street}', '{zip}', '{contact}', '{email}', '{url}', '{spec}', '{years}', '{govtAccred}', '{licenseExp.ToString("yyyy-MM-dd")}', '{notes}')"

        readQuery(query)
        MsgBox("Agency record added successfully!", MsgBoxStyle.Information)
        Me.Close()
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
