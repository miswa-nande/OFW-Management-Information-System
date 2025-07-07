Imports System
Imports System.IO
Imports Microsoft.VisualBasic
Imports MySql.Data.MySqlClient


Public Class addOfw
    Private selectedImagePath As String = ""

    Private Sub addOfw_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        cbxSex.Items.AddRange({"Male", "Female", "Other"})
        cbxCivStat.Items.AddRange({"Single", "Married", "Widowed", "Separated"})
        cbxEducLevel.Items.AddRange({"High School", "Vocational", "College", "Postgraduate"})

        If Session.CurrentLoggedUser.userType = "OFW" Then
            btnAdd.Text = "Save"
            Label1.Text = "OFW Profile"
        End If
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

        Dim imgBytes() As Byte = Nothing
        If Not String.IsNullOrEmpty(selectedImagePath) AndAlso File.Exists(selectedImagePath) Then
            imgBytes = File.ReadAllBytes(selectedImagePath)
        End If

        Dim employmentStatus As String = "Active"
        Dim agencyId As Object = DBNull.Value

        Try
            openConn(db_name)
            Dim query As String = "INSERT INTO ofw (FirstName, MiddleName, LastName, DOB, Sex, CivilStatus, Street, Barangay, City, Province, Zipcode, EducationalLevel, Skills, ContactNum, EmergencyContactNum, PassportNum, VISANum, OECNum, EmploymentStatus, PictureFace, AgencyID) VALUES (@fName, @mName, @lName, @dob, @sex, @civilStat, @street, @brgy, @city, @prov, @zip, @educ, @skills, @contact, @eContact, @passport, @visa, @oec, @status, @pic, @agency)"
            Using cmd As New MySqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@fName", fName)
                cmd.Parameters.AddWithValue("@mName", mName)
                cmd.Parameters.AddWithValue("@lName", lName)
                cmd.Parameters.AddWithValue("@dob", dob)
                cmd.Parameters.AddWithValue("@sex", sex)
                cmd.Parameters.AddWithValue("@civilStat", civilStat)
                cmd.Parameters.AddWithValue("@street", street)
                cmd.Parameters.AddWithValue("@brgy", brgy)
                cmd.Parameters.AddWithValue("@city", city)
                cmd.Parameters.AddWithValue("@prov", prov)
                cmd.Parameters.AddWithValue("@zip", zip)
                cmd.Parameters.AddWithValue("@educ", educ)
                cmd.Parameters.AddWithValue("@skills", skills)
                cmd.Parameters.AddWithValue("@contact", contact)
                cmd.Parameters.AddWithValue("@eContact", eContact)
                cmd.Parameters.AddWithValue("@passport", passport)
                cmd.Parameters.AddWithValue("@visa", visa)
                cmd.Parameters.AddWithValue("@oec", oec)
                cmd.Parameters.AddWithValue("@status", employmentStatus)
                If imgBytes IsNot Nothing Then
                    cmd.Parameters.Add("@pic", MySqlDbType.Blob).Value = imgBytes
                Else
                    cmd.Parameters.Add("@pic", MySqlDbType.Blob).Value = DBNull.Value
                End If
                cmd.Parameters.AddWithValue("@agency", agencyId)

                cmd.ExecuteNonQuery()

                If Session.CurrentLoggedUser.userType = "OFW" Then
                    Dim insertedId As Integer = CInt(cmd.LastInsertedId)

                    ' Update the user table with reference ID
                    Dim updateQuery As String = $"UPDATE users SET reference_id = {insertedId} WHERE user_id = {Session.CurrentLoggedUser.id}"
                    readQuery(updateQuery)

                    ' Update session
                    Session.CurrentReferenceID = insertedId

                    MsgBox("Profile saved. Redirecting to OFW profile...", MsgBoxStyle.Information)
                    Dim profileForm As New ofwProfile()
                    profileForm.Show()
                    Me.Close()
                Else
                    MsgBox("OFW record added successfully!", MsgBoxStyle.Information)
                    Me.Close()
                End If
            End Using
        Catch ex As Exception
            MsgBox("An error occurred: " & ex.Message, MsgBoxStyle.Critical)
        Finally
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try

        ' Validate required fields
        If String.IsNullOrWhiteSpace(txtbxFName.Text) OrElse
           String.IsNullOrWhiteSpace(txtbxMName.Text) OrElse
           String.IsNullOrWhiteSpace(txtbxLName.Text) OrElse
           cbxSex.SelectedItem Is Nothing OrElse
           cbxCivStat.SelectedItem Is Nothing OrElse
           String.IsNullOrWhiteSpace(txtbxStreet.Text) OrElse
           String.IsNullOrWhiteSpace(txtbxBrgy.Text) OrElse
           String.IsNullOrWhiteSpace(txtbxCity.Text) OrElse
           String.IsNullOrWhiteSpace(txtbxProv.Text) OrElse
           String.IsNullOrWhiteSpace(txtbxZipcode.Text) OrElse
           cbxEducLevel.SelectedItem Is Nothing OrElse
           String.IsNullOrWhiteSpace(txtbxSkills.Text) OrElse
           String.IsNullOrWhiteSpace(txtbxContactNum.Text) OrElse
           String.IsNullOrWhiteSpace(txtbxEContactNum.Text) OrElse
           String.IsNullOrWhiteSpace(txtbxPassport.Text) OrElse
           String.IsNullOrWhiteSpace(txtbxVisa.Text) OrElse
           String.IsNullOrWhiteSpace(txtbxOec.Text) Then

            MsgBox("Please fill in all fields before saving.", MsgBoxStyle.Exclamation)
            Return
        End If

        If String.IsNullOrEmpty(selectedImagePath) OrElse Not File.Exists(selectedImagePath) Then
            MsgBox("Please upload a profile picture.", MsgBoxStyle.Exclamation)
            Return
        End If

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
        btnAddImg.PerformClick()
    End Sub
End Class
