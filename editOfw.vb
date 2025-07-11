﻿Imports System
Imports System.IO
Imports Microsoft.VisualBasic
Imports MySql.Data.MySqlClient

Public Class editOfw
    Private selectedImagePath As String = ""
    Private currentImageBytes() As Byte
    Private loadedOFWId As Integer

    ' Constructor for logged-in OFW
    Public Sub New()
        InitializeComponent()
        loadedOFWId = Session.CurrentReferenceID
    End Sub

    ' Constructor for Employer/Agency viewing a selected OFW
    Public Sub New(ofwId As Integer)
        InitializeComponent()
        loadedOFWId = ofwId
    End Sub

    Private Sub editOfw_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Select Case Session.CurrentLoggedUser.userType
            Case "OFW"
                Label1.Text = "Edit Profile"
                btnSave.Visible = True
                btnCancel.Text = "Cancel"

            Case "Agency"
                Label1.Text = "OFW PROFILE"
                btnSave.Visible = False
                btnCancel.Text = "Close"

            Case "Employer"
                Label1.Text = "OFW PROFILE (View Only)"
                btnSave.Visible = False
                btnCancel.Text = "Close"
                SetControlsReadOnly()

            Case "Admin"
                Label1.Text = "Edit OFW Profile"
                btnSave.Visible = True
                btnCancel.Text = "Close"

            Case Else
                MessageBox.Show("Access denied.", "Unauthorized", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Me.Close()
                Return
        End Select

        If cbxSex.Items.Count = 0 Then cbxSex.Items.AddRange({"Male", "Female", "Other"})
        If cbxCivStat.Items.Count = 0 Then cbxCivStat.Items.AddRange({"Single", "Married", "Widowed", "Separated"})
        If cbxEducLevel.Items.Count = 0 Then cbxEducLevel.Items.AddRange({"High School", "Vocational", "College", "Postgraduate"})

        LoadOFWData()
    End Sub

    Private Sub SetControlsReadOnly()
        For Each ctrl As Control In Me.Controls
            If TypeOf ctrl Is TextBox Then CType(ctrl, TextBox).ReadOnly = True
        Next
        cbxSex.Enabled = False
        cbxCivStat.Enabled = False
        cbxEducLevel.Enabled = False
        dateDOB.Enabled = False
        btnAddImg.Enabled = False
        PictureBox2.Enabled = False
    End Sub

    Private Sub LoadOFWData()
        Try
            openConn(db_name)
            Dim query As String = $"SELECT * FROM ofw WHERE OFWId = {loadedOFWId}"
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
        If Not IsNumeric(txtbxZipcode.Text.Trim()) OrElse
           Not IsNumeric(txtbxContactNum.Text.Trim()) OrElse
           Not IsNumeric(txtbxEContactNum.Text.Trim()) Then
            MsgBox("Zip code and contact numbers must be numeric.", MsgBoxStyle.Exclamation)
            Return
        End If

        If String.IsNullOrWhiteSpace(txtbxFName.Text) OrElse
           cbxSex.SelectedItem Is Nothing OrElse
           cbxCivStat.SelectedItem Is Nothing OrElse
           cbxEducLevel.SelectedItem Is Nothing Then
            MsgBox("Please complete all required fields.", MsgBoxStyle.Exclamation)
            Return
        End If

        Dim EscapeStr As Func(Of String, String) = Function(s) s.Replace("'", "''")

        Dim fName = EscapeStr(txtbxFName.Text.Trim())
        Dim mName = EscapeStr(txtbxMName.Text.Trim())
        Dim lName = EscapeStr(txtbxLName.Text.Trim())
        Dim dob = dateDOB.Value.ToString("yyyy-MM-dd")
        Dim sex = EscapeStr(cbxSex.SelectedItem.ToString())
        Dim civilStat = EscapeStr(cbxCivStat.SelectedItem.ToString())
        Dim street = EscapeStr(txtbxStreet.Text.Trim())
        Dim brgy = EscapeStr(txtbxBrgy.Text.Trim())
        Dim city = EscapeStr(txtbxCity.Text.Trim())
        Dim prov = EscapeStr(txtbxProv.Text.Trim())
        Dim zip = txtbxZipcode.Text.Trim()
        Dim contact = txtbxContactNum.Text.Trim()
        Dim eContact = txtbxEContactNum.Text.Trim()
        Dim passport = EscapeStr(txtbxPassport.Text.Trim())
        Dim educ = EscapeStr(cbxEducLevel.SelectedItem.ToString())
        Dim skills = EscapeStr(txtbxSkills.Text.Trim())
        Dim visa = EscapeStr(txtbxVisa.Text.Trim())
        Dim oec = EscapeStr(txtbxOec.Text.Trim())

        Try
            openConn(db_name)

            If Not String.IsNullOrEmpty(selectedImagePath) Then
                currentImageBytes = File.ReadAllBytes(selectedImagePath)
            End If

            Dim query As String = $"
                UPDATE ofw SET 
                    FirstName = @fName, MiddleName = @mName, LastName = @lName, 
                    DOB = @dob, Sex = @sex, CivilStatus = @civil, Street = @street, 
                    Barangay = @brgy, City = @city, Province = @prov, Zipcode = @zip, 
                    ContactNum = @contact, EmergencyContactNum = @eContact, 
                    PassportNum = @passport, EducationalLevel = @educ, Skills = @skills, 
                    VISANum = @visa, OECNum = @oec, PictureFace = @img
                WHERE OFWId = {loadedOFWId}"

            Using cmd As New MySqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@fName", fName)
                cmd.Parameters.AddWithValue("@mName", mName)
                cmd.Parameters.AddWithValue("@lName", lName)
                cmd.Parameters.AddWithValue("@dob", dob)
                cmd.Parameters.AddWithValue("@sex", sex)
                cmd.Parameters.AddWithValue("@civil", civilStat)
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
                cmd.Parameters.AddWithValue("@img", currentImageBytes)
                cmd.ExecuteNonQuery()
            End Using

            MsgBox("Profile updated successfully!", MsgBoxStyle.Information)

            Me.DialogResult = DialogResult.OK
            Me.Close()
        Catch ex As Exception
            MsgBox("Error while updating: " & ex.Message, MsgBoxStyle.Critical)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub txtbxZipcode_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtbxZipcode.KeyPress
        If Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsDigit(e.KeyChar) Then e.Handled = True
    End Sub

    Private Sub txtbxContactNum_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtbxContactNum.KeyPress
        If Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsDigit(e.KeyChar) Then e.Handled = True
    End Sub

    Private Sub txtbxEContactNum_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtbxEContactNum.KeyPress
        If Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsDigit(e.KeyChar) Then e.Handled = True
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
        If btnAddImg.Enabled Then btnAddImg.PerformClick()
    End Sub
End Class
