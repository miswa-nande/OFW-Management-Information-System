Public Class loginPage

    Private Sub btnAdmin_Click(sender As Object, e As EventArgs) Handles btnAdmin.Click
        OpenLogin("Admin")
    End Sub

    Private Sub btnAgency_Click(sender As Object, e As EventArgs) Handles btnAgency.Click
        OpenLogin("Agency")
    End Sub

    Private Sub btnOfw_Click(sender As Object, e As EventArgs) Handles btnOfw.Click
        OpenLogin("OFW")
    End Sub

    Private Sub btnEmployer_Click(sender As Object, e As EventArgs) Handles btnEmployer.Click
        OpenLogin("Employer")
    End Sub

    Private Sub OpenLogin(userType As String)
        ' Set the current user type for session tracking
        Session.CurrentLoggedUser.userType = userType

        ' Pass the userType to the login form
        Dim login As New loginFields(userType)
        login.Show()
        Me.Hide()
    End Sub

End Class
