Imports System
Imports System.IO
Imports Microsoft.VisualBasic
Imports MySql.Data.MySqlClient

Public Class editOfw
    Private selectedImagePath As String = ""
    Private currentImageBytes() As Byte

    Private Sub editOfw_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Session.CurrentLoggedUser.userType = "OFW" Then
            Label1.Text = "Edit Profile"
        ElseIf Session.CurrentLoggedUser.userType = "Agency" Then
            Label1.Text = "OFW PROFILE"
        Else
            MessageBox.Show("Access denied.", "Unauthorized", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Me.Close()
            Return
        End If

        cbxSex.Items.AddRange({"Male", "Female", "Other"})
        cbxCivStat.Items.AddRange({"Single", "Married", "Widowed", "Separated"})
        cbxEducLevel.Items.AddRange({"High School", "Vocational", "College", "Postgraduate"})

        LoadOFWData()
    End Sub

    Private Sub LoadOFWData()
        Try
            openConn(db_name)
            Dim query As String = $"SELECT * FROM ofw WHERE OFWId = {Session.CurrentReferenceID}"
            Using cmd As New MySqlCommand(query, conn)
                Using reader As MySqlDataReader = cmd.ExecuteReader()
                    If reader.Read() Then
                        txtbxFName.Text = reader("FirstName").ToString()
                        txtbxMName.Text = reader("MiddleName").ToString()
                        txtbxLName.Text = reader("LastName").ToString()
                        dateDOB.Value = Convert.ToDateTime(reader("DOB"))
                        cbxSex.SelectedItem = reader("Sex").ToString()
                        cbxCivStat.SelectedItem = reader("CivilStatus").ToString()
                        txtbxStreet.Text = reader("Street").ToString()
                        txtbxBrgy.Text = reader("Barangay").ToString()
                        txtbxCity.Text = reader("City").ToString()
                        txtbxProv.Text = reader("Province").ToString()
                        txtbxZipcode.Text = reader("Zipcode").ToString()
                        txtbxContactNum.Text = reader("ContactNum").ToString()
                        txtbxEContactNum.Text = reader("EmergencyContactNum").ToString()
                        txtbxPassport.Text = reader("PassportNum").ToString()
                        cbxEducLevel.SelectedItem = reader("EducationalLevel").ToString()
                        txtbxSkills.Text = reader("Skills").ToString()
                        txtbxVisa.Text = reader("VISANum").ToString()
                        txtbxOec.Text = reader("OECNum").ToString()

                        If Not IsDBNull(reader("PictureFace")) Then
                            currentImageBytes = DirectCast(reader("PictureFace"), Byte())
                            Using ms As New MemoryStream(currentImageBytes)
                                PictureBox2.Image = Image.FromStream(ms)
                            End Using
                        End If
                    End If
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Failed to load OFW data: " & ex.Message, MsgBoxStyle.Critical)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        ' Field validation
        If Not IsNumeric(txtbxZipcode.Text.Trim()) OrElse
           Not IsNumeric(txtbxContactNum.Text.Trim()) OrElse
           Not IsNumeric(txtbxEContactNum.Text.Trim()) Then
            MsgBox("Zip code and contact numbers must be numeric.", MsgBoxStyle.Exclamation)
            Return
        End If

        ' Ensure all required fields are filled
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
            MsgBox("Please complete all required fields.", MsgBoxStyle.Exclamation)
            Return
        End If

        ' Save changes
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

        Dim newImgBytes() As Byte = currentImageBytes
        If Not String.IsNullOrEmpty(selectedImagePath) AndAlso File.Exists(selectedImagePath) Then
            newImgBytes = File.ReadAllBytes(selectedImagePath)
        End If

        Try
            openConn(db_name)
            Dim query As String = "
                UPDATE ofw SET 
                    FirstName = @fName, MiddleName = @mName, LastName = @lName, DOB = @dob, Sex = @sex, 
                    CivilStatus = @civilStat, Street = @street, Barangay = @brgy, City = @city, Province = @prov, 
                    Zipcode = @zip, ContactNum = @contact, EmergencyContactNum = @eContact, PassportNum = @passport, 
                    EducationalLevel = @educ, Skills = @skills, VISANum = @visa, OECNum = @oec, PictureFace = @pic 
                WHERE OFWId = @ofwId"

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
                cmd.Parameters.AddWithValue("@contact", contact)
                cmd.Parameters.AddWithValue("@eContact", eContact)
                cmd.Parameters.AddWithValue("@passport", passport)
                cmd.Parameters.AddWithValue("@educ", educ)
                cmd.Parameters.AddWithValue("@skills", skills)
                cmd.Parameters.AddWithValue("@visa", visa)
                cmd.Parameters.AddWithValue("@oec", oec)
                cmd.Parameters.AddWithValue("@pic", If(newImgBytes IsNot Nothing, newImgBytes, DBNull.Value))
                cmd.Parameters.AddWithValue("@ofwId", Session.CurrentReferenceID)

                cmd.ExecuteNonQuery()
                MsgBox("Profile updated successfully!", MsgBoxStyle.Information)

                If Session.CurrentLoggedUser.userType = "OFW" Then
                    Dim profileForm As New ofwProfile()
                    profileForm.Show()
                End If

                Me.Close()
            End Using
        Catch ex As Exception
            MsgBox("Error while updating: " & ex.Message, MsgBoxStyle.Critical)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        If Session.CurrentLoggedUser.userType = "OFW" Then
            Dim profileForm As New ofwProfile()
            profileForm.Show()
        End If
        Me.Close()
    End Sub

    ' Restrictions
    Private Sub txtbxZipcode_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtbxZipcode.KeyPress
        If Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsDigit(e.KeyChar) Then e.Handled = True
    End Sub

    Private Sub txtbxContactNum_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtbxContactNum.KeyPress
        If Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsDigit(e.KeyChar) Then e.Handled = True
    End Sub

    Private Sub txtbxEContactNum_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtbxEContactNum.KeyPress
        If Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsDigit(e.KeyChar) Then e.Handled = True
    End Sub

    ' Image
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
