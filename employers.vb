Public Class employers
    Private Sub btnDeployments_Click(sender As Object, e As EventArgs) Handles btnDeployments.Click
        Dim newForm As New deployments()
        newForm.Show()     ' Show the new form
        Me.Hide()          ' Hide current form (or use Me.Close() to fully close it)
    End Sub

    Private Sub btnDashboard_Click(sender As Object, e As EventArgs) Handles btnDashboard.Click
        Dim newForm As New dashboard()
        newForm.Show()     ' Show the new form
        Me.Hide()          ' Hide current form (or use Me.Close() to fully close it)
    End Sub
    Private Sub btnOfw_Click(sender As Object, e As EventArgs) Handles btnOfw.Click
        Dim newForm As New ofws()
        newForm.Show()     ' Show the new form
        Me.Hide()          ' Hide current form (or use Me.Close() to fully close it)
    End Sub

    Private Sub btnAgencies_Click(sender As Object, e As EventArgs) Handles btnAgencies.Click
        Dim newForm As New agencies()
        newForm.Show()     ' Show the new form
        Me.Hide()          ' Hide current form (or use Me.Close() to fully close it)
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Dim dlg As New addEmployer()
        dlg.ShowDialog() ' Opens as a modal dialog
    End Sub

    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        Dim dlg As New editEmployer()
        dlg.ShowDialog() ' Opens as a modal dialog
    End Sub
End Class