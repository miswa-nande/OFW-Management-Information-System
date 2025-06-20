Public Class dashboard

    Private Sub btnOfw_Click(sender As Object, e As EventArgs) Handles btnOfw.Click
        Dim newForm As New ofws()
        newForm.Show()     ' Show the new form
        Me.Hide()          ' Hide current form (or use Me.Close() to fully close it)
    End Sub

    Private Sub btnDeployments_Click(sender As Object, e As EventArgs) Handles btnDeployments.Click
        Dim newForm As New deployments()
        newForm.Show()     ' Show the new form
        Me.Hide()          ' Hide current form (or use Me.Close() to fully close it)
    End Sub

    Private Sub btnAgencies_Click(sender As Object, e As EventArgs) Handles btnAgencies.Click
        Dim newForm As New agencies()
        newForm.Show()     ' Show the new form
        Me.Hide()          ' Hide current form (or use Me.Close() to fully close it)
    End Sub

    Private Sub btnEmployers_Click(sender As Object, e As EventArgs) Handles btnEmployers.Click
        Dim newForm As New employers()
        newForm.Show()     ' Show the new form
        Me.Hide()          ' Hide current form (or use Me.Close() to fully close it)
    End Sub
End Class