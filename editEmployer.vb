Imports System
Imports Microsoft.VisualBasic
Imports MySql.Data.MySqlClient

Public Class editEmployer
    Private currentEmployerId As Integer

    ' Constructor for admin or selection from employer list
    Public Sub New(employerId As Integer)
        InitializeComponent()
        currentEmployerId = employerId
    End Sub

    ' Default constructor (used when current logged in employer is editing)
    Public Sub New()
        InitializeComponent()
        currentEmployerId = Session.CurrentReferenceID
    End Sub

    Private Sub editEmployer_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Session.CurrentLoggedUser.userType = "Employer" Then
            Label1.Text = "Edit Profile"
        End If

        LoadEmployerData()
    End Sub

    Private Sub LoadEmployerData()
        Try
            Dim query As String = $"SELECT * FROM employer WHERE EmployerID = {currentEmployerId}"
            readQuery(query)

            If cmdRead.Read() Then
                txtbxFName.Text = cmdRead("EmployerFirstName").ToString()
                txtbxMName.Text = cmdRead("EmployerMiddleName").ToString()
                txtbxLName.Text = cmdRead("EmployerLastName").ToString()
                txtbxContactNum.Text = cmdRead("EmployerContactNum").ToString()
                txtbxEmail.Text = cmdRead("EmployerEmail").ToString()
                txtbxCompanyName.Text = cmdRead("CompanyName").ToString()
                txtbxIndustry.Text = cmdRead("Industry").ToString()
                txtbxStreet.Text = cmdRead("CompanyStreet").ToString()
                txtbxCity.Text = cmdRead("CompanyCity").ToString()
                txtbxState.Text = cmdRead("CompanyState").ToString()
                txtbxCountry.Text = cmdRead("CompanyCountry").ToString()
                txtbxZipcode.Text = cmdRead("CompanyZipcode").ToString()
            End If

            cmdRead.Close()
        Catch ex As Exception
            MsgBox("Failed to load employer data: " & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If Not IsNumeric(txtbxContactNum.Text.Trim()) Then
            MsgBox("Contact Number must be numeric.", MsgBoxStyle.Exclamation)
            Return
        End If

        If Not IsNumeric(txtbxZipcode.Text.Trim()) Then
            MsgBox("Zipcode must be numeric.", MsgBoxStyle.Exclamation)
            Return
        End If

        Dim query As String = "
            UPDATE employer SET
                EmployerFirstName = @fName,
                EmployerMiddleName = @mName,
                EmployerLastName = @lName,
                EmployerContactNum = @contact,
                EmployerEmail = @email,
                CompanyName = @company,
                Industry = @industry,
                CompanyStreet = @street,
                CompanyCity = @city,
                CompanyState = @state,
                CompanyCountry = @country,
                CompanyZipcode = @zip
            WHERE EmployerID = @id
        "

        Try
            openConn(db_name)
            Using cmd As New MySqlCommand(query, conn)
                With cmd.Parameters
                    .AddWithValue("@fName", txtbxFName.Text.Trim())
                    .AddWithValue("@mName", txtbxMName.Text.Trim())
                    .AddWithValue("@lName", txtbxLName.Text.Trim())
                    .AddWithValue("@contact", txtbxContactNum.Text.Trim())
                    .AddWithValue("@email", txtbxEmail.Text.Trim())
                    .AddWithValue("@company", txtbxCompanyName.Text.Trim())
                    .AddWithValue("@industry", txtbxIndustry.Text.Trim())
                    .AddWithValue("@street", txtbxStreet.Text.Trim())
                    .AddWithValue("@city", txtbxCity.Text.Trim())
                    .AddWithValue("@state", txtbxState.Text.Trim())
                    .AddWithValue("@country", txtbxCountry.Text.Trim())
                    .AddWithValue("@zip", txtbxZipcode.Text.Trim())
                    .AddWithValue("@id", currentEmployerId)
                End With

                cmd.ExecuteNonQuery()
            End Using

            MsgBox("Employer profile updated successfully!", MsgBoxStyle.Information)
            Me.Close()

        Catch ex As Exception
            MsgBox("Error while saving: " & ex.Message, MsgBoxStyle.Critical)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub txtbxContactNum_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtbxContactNum.KeyPress
        If Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsDigit(e.KeyChar) Then e.Handled = True
    End Sub

    Private Sub txtbxZipcode_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtbxZipcode.KeyPress
        If Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsDigit(e.KeyChar) Then e.Handled = True
    End Sub
End Class
