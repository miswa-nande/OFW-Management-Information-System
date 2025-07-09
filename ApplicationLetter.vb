Imports MySql.Data.MySqlClient

Public Class ApplicationLetter
    Private Sub ApplicationLetter_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadExistingLetter()
    End Sub

    Private Sub LoadExistingLetter()
        ' Get the latest application for the current OFW
        Dim query As String = $"SELECT ApplicationID, LetterBody FROM application WHERE OFWID = {Session.CurrentReferenceID} ORDER BY ApplicationDate DESC LIMIT 1"
        readQuery(query)

        If cmdRead IsNot Nothing AndAlso cmdRead.Read() Then
            txtbxLetterContainer.Text = cmdRead("LetterBody").ToString()
        Else
            txtbxLetterContainer.Text = "" ' No letter yet
        End If

        cmdRead?.Close()
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Dim letterBody As String = txtbxLetterContainer.Text.Replace("'", "''")

        ' Get the latest application ID for this OFW
        Dim checkQuery As String = $"SELECT ApplicationID FROM application WHERE OFWID = {Session.CurrentReferenceID} ORDER BY ApplicationDate DESC LIMIT 1"
        readQuery(checkQuery)

        Dim applicationId As Integer = -1
        If cmdRead IsNot Nothing AndAlso cmdRead.Read() Then
            applicationId = Convert.ToInt32(cmdRead("ApplicationID"))
        End If
        cmdRead?.Close()

        If applicationId <> -1 Then
            ' Save or update the LetterBody field
            Dim updateQuery As String = $"UPDATE application SET LetterBody = '{letterBody}' WHERE ApplicationID = {applicationId}"
            readQuery(updateQuery)
            cmdRead?.Close()

            MessageBox.Show("Application letter saved successfully!", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.Close() ' ✅ Close after saving
        Else
            MessageBox.Show("No application found. Please apply to a job before writing a letter.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub


    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub
End Class
