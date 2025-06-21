Imports System.IO

Public Class addOfw
    Private selectedImagePath As String = ""

    Private Sub addOfw_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        cbxSex.Items.AddRange({"Male", "Female", "Other"})
        cbxCivStat.Items.AddRange({"Single", "Married", "Widowed", "Separated"})
        cbxEducLevel.Items.AddRange({"High School", "Vocational", "College", "Postgraduate"})
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        ' Validation
        If Not IsNumeric(txtbxZipcode.Text.Trim()) Then
            MsgBox("Zip Code must be a number.", MsgBoxStyle.Exclamation)
            Return
        End If

        If Not IsNumeric(txtbxContactNum.Text.Trim()) Or Not IsNumeric(txtbxEContactNum.Text.Trim()) Then
            MsgBox("Contact numbers must be numeric.", MsgBoxStyle.Exclamation)
            Return
        End If

        ' Save logic
        Dim fName As String = txtbxFName.Text.Trim()
        Dim mName As String = txtbxMName.Text.Trim()
        Dim lName As String = txtbxLName.Text.Trim()
        Dim dob As Date = dateDOB.Value
        Dim sex As String = cbxSex.SelectedItem.ToString()
        Dim civilStat As String = cbxCivStat.SelectedItem.ToString()
        Dim street As String = txtbxStreet.Text.Trim()
        Dim brgy As String = txtbxBrgy.Text.Trim()
        Dim city As String = txtbxCity.Text.Trim()
        Dim prov As String = txtbxProv.Text.Trim()
        Dim zip As String = txtbxZipcode.Text.Trim()
        Dim contact As String = txtbxContactNum.Text.Trim()
        Dim eContact As String = txtbxEContactNum.Text.Trim()
        Dim passport As String = txtbxPassport.Text.Trim()
        Dim educ As String = cbxEducLevel.SelectedItem.ToString()
        Dim skills As String = txtbxSkills.Text.Trim()
        Dim visa As String = txtbxVisa.Text.Trim()
        Dim oec As String = txtbxOec.Text.Trim()

        ' Convert image to Base64
        Dim base64Image As String = ""
        If Not String.IsNullOrEmpty(selectedImagePath) Then
            Dim imgBytes() As Byte = File.ReadAllBytes(selectedImagePath)
            base64Image = Convert.ToBase64String(imgBytes)
        End If

        Dim query As String = $"
            INSERT INTO ofw (FirstName, MiddleName, LastName, DOB, Sex, CivilStatus, Street, Barangay, City, Province, ZipCode, ContactNumber, EmergencyContactNumber, PassportNumber, EducationLevel, Skills, VisaNumber, OECNumber, ProfileImage)
            VALUES ('{fName}', '{mName}', '{lName}', '{dob:yyyy-MM-dd}', '{sex}', '{civilStat}', '{street}', '{brgy}', '{city}', '{prov}', '{zip}', '{contact}', '{eContact}', '{passport}', '{educ}', '{skills}', '{visa}', '{oec}', '{base64Image}')"

        readQuery(query)
        MsgBox("OFW record added successfully!", MsgBoxStyle.Information)
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub txtbxZipcode_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtbxZipcode.KeyPress
        If Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsDigit(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtbxContactNum_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtbxContactNum.KeyPress
        If Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsDigit(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtbxEContactNum_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtbxEContactNum.KeyPress
        If Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsDigit(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    Private Sub btnAddImg_Click(sender As Object, e As EventArgs) Handles btnAddImg.Click
        Dim ofd As New OpenFileDialog()
        ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp"

        If ofd.ShowDialog() = DialogResult.OK Then
            selectedImagePath = ofd.FileName
            PictureBox2.Image = Image.FromFile(selectedImagePath)
        End If
    End Sub

    Private Sub PictureBox2_Click(sender As Object, e As EventArgs) Handles PictureBox2.Click
        ' Optional: Click to change image
        btnAddImg.PerformClick()
    End Sub

End Class