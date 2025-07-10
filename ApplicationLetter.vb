Imports MySql.Data.MySqlClient

Public Class ApplicationLetter
    Private Sub ApplicationLetter_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadExistingLetter()
    End Sub

    Private Sub LoadExistingLetter()
        Try
            ' Get the letter body directly from the OFW table
            Dim query As String = $"SELECT LetterBody FROM ofw WHERE OFWID = {Session.CurrentReferenceID}"
            readQuery(query)

            If cmdRead IsNot Nothing AndAlso cmdRead.Read() Then
                txtbxLetterContainer.Text = cmdRead("LetterBody").ToString()
            Else
                txtbxLetterContainer.Text = "" ' No letter yet
            End If

            cmdRead?.Close()
        Catch ex As Exception
            MessageBox.Show("Failed to load letter: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Try
            Dim letterBody As String = txtbxLetterContainer.Text.Replace("'", "''")

            ' Update the OFW table directly
            Dim updateQuery As String = $"UPDATE ofw SET LetterBody = '{letterBody}' WHERE OFWID = {Session.CurrentReferenceID}"
            readQuery(updateQuery)
            cmdRead?.Close()

            MessageBox.Show("Application letter saved successfully!", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.Close()
        Catch ex As Exception
            MessageBox.Show("Error saving letter: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub
End Class
