Public Class addEmployer
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

        ' SQL INSERT
        Dim query As String = $"
            INSERT INTO employer (FirstName, MiddleName, LastName, ContactNumber, Email, CompanyName, Industry, Street, City, State, Country, ZipCode)
            VALUES ('{fName}', '{mName}', '{lName}', '{contact}', '{email}', '{company}', '{industry}', '{street}', '{city}', '{state}', '{country}', '{zipcode}')"

        readQuery(query)
        MsgBox("Employer added successfully!", MsgBoxStyle.Information)
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    ' Input restrictions for numeric fields
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
