Public Class ofwProfile
    Private Sub btnJobOffers_Click(sender As Object, e As EventArgs) Handles btnJobOffers.Click
        Dim newForm As New joboffers()
        newForm.Show()     ' Show the new form
        Me.Hide()          ' Hide current form (or use Me.Close() to fully close it)
    End Sub

    Private Sub btnDeployment_Click(sender As Object, e As EventArgs) Handles btnDeployment.Click
        Dim newForm As New deploymentrecords()
        newForm.Show()     ' Show the new form
        Me.Hide()          ' Hide current form (or use Me.Close() to fully close it)
    End Sub

    Private Sub btnApplications_Click(sender As Object, e As EventArgs) Handles btnApplications.Click
        Dim newForm As New applications()
        newForm.Show()     ' Show the new form
        Me.Hide()          ' Hide current form (or use Me.Close() to fully close it)
    End Sub
End Class