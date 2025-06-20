Public Class joboffers
    Private Sub btnProfile_Click(sender As Object, e As EventArgs) Handles btnProfile.Click
        Dim newForm As New ofwProfile()
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

    Private Sub btnViewDetails_Click(sender As Object, e As EventArgs) Handles btnViewDetails.Click
        Dim dlg As New jobdetails()
        dlg.ShowDialog() ' Opens as a modal dialog
    End Sub
End Class