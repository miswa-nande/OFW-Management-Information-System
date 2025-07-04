﻿Imports System
Imports Microsoft.VisualBasic
Imports MySql.Data.MySqlClient

Public Class editEmployer
    Private Sub editEmployer_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Change label to "Edit Profile" if logged-in user is Employer
        If Session.CurrentLoggedUser.userType = "Employer" Then
            Label1.Text = "Edit Profile"
        End If

        LoadEmployerData()
    End Sub

    Private Sub LoadEmployerData()
        Try
            openConn(db_name)
            Dim query As String = $"SELECT * FROM employer WHERE EmployerID = {Session.CurrentReferenceID}"
            Using cmd As New MySqlCommand(query, conn)
                Using reader As MySqlDataReader = cmd.ExecuteReader()
                    If reader.Read() Then
                        txtbxFName.Text = reader("FirstName").ToString()
                        txtbxMName.Text = reader("MiddleName").ToString()
                        txtbxLName.Text = reader("LastName").ToString()
                        txtbxContactNum.Text = reader("ContactNumber").ToString()
                        txtbxEmail.Text = reader("Email").ToString()
                        txtbxCompanyName.Text = reader("CompanyName").ToString()
                        txtbxIndustry.Text = reader("Industry").ToString()
                        txtbxStreet.Text = reader("Street").ToString()
                        txtbxCity.Text = reader("City").ToString()
                        txtbxState.Text = reader("State").ToString()
                        txtbxCountry.Text = reader("Country").ToString()
                        txtbxZipcode.Text = reader("ZipCode").ToString()
                    End If
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Failed to load employer data: " & ex.Message, MsgBoxStyle.Critical)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
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
                FirstName = @fName,
                MiddleName = @mName,
                LastName = @lName,
                ContactNumber = @contact,
                Email = @email,
                CompanyName = @company,
                Industry = @industry,
                Street = @street,
                City = @city,
                State = @state,
                Country = @country,
                ZipCode = @zip
            WHERE employer_id = @id"

        Try
            openConn(db_name)
            Using cmd As New MySqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@fName", txtbxFName.Text.Trim())
                cmd.Parameters.AddWithValue("@mName", txtbxMName.Text.Trim())
                cmd.Parameters.AddWithValue("@lName", txtbxLName.Text.Trim())
                cmd.Parameters.AddWithValue("@contact", txtbxContactNum.Text.Trim())
                cmd.Parameters.AddWithValue("@email", txtbxEmail.Text.Trim())
                cmd.Parameters.AddWithValue("@company", txtbxCompanyName.Text.Trim())
                cmd.Parameters.AddWithValue("@industry", txtbxIndustry.Text.Trim())
                cmd.Parameters.AddWithValue("@street", txtbxStreet.Text.Trim())
                cmd.Parameters.AddWithValue("@city", txtbxCity.Text.Trim())
                cmd.Parameters.AddWithValue("@state", txtbxState.Text.Trim())
                cmd.Parameters.AddWithValue("@country", txtbxCountry.Text.Trim())
                cmd.Parameters.AddWithValue("@zip", txtbxZipcode.Text.Trim())
                cmd.Parameters.AddWithValue("@id", Session.CurrentReferenceID)

                cmd.ExecuteNonQuery()

                MsgBox("Employer profile updated successfully!", MsgBoxStyle.Information)
                Me.Close()
            End Using
        Catch ex As Exception
            MsgBox("Error while saving: " & ex.Message, MsgBoxStyle.Critical)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    ' Input restrictions
    Private Sub txtbxContactNum_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtbxContactNum.KeyPress
        If Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsDigit(e.KeyChar) Then e.Handled = True
    End Sub

    Private Sub txtbxZipcode_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtbxZipcode.KeyPress
        If Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsDigit(e.KeyChar) Then e.Handled = True
    End Sub
End Class
