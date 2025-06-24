Public Class deployments
    Private Sub deployments_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadDeploymentsToDGV(DataGridView1)
        FormatDGVUniformly(DataGridView1)
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Dim dlg As New addDeployment()
        dlg.ShowDialog() ' Opens as a modal dialog
        LoadDeploymentsToDGV(DataGridView1)
        FormatDGVUniformly(DataGridView1)
    End Sub

    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        Dim dlg As New editDeployment()
        dlg.ShowDialog() ' Opens as a modal dialog
        LoadDeploymentsToDGV(DataGridView1)
        FormatDGVUniformly(DataGridView1)
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If DataGridView1.SelectedRows.Count > 0 Then
            ' Get the ID of the selected deployment
            Dim selectedRow As DataGridViewRow = DataGridView1.SelectedRows(0)
            Dim deploymentId As Integer = Convert.ToInt32(selectedRow.Cells("DeploymentID").Value)

            ' Ask for confirmation
            Dim result As DialogResult = MessageBox.Show("Are you sure you want to delete this deployment record? This action cannot be undone.", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)

            If result = DialogResult.Yes Then
                ' Call the generic delete method
                DeleteRecord("DeploymentRecord", "DeploymentID", deploymentId)

                ' Refresh the DataGridView
                LoadDeploymentsToDGV(DataGridView1)
                FormatDGVUniformly(DataGridView1)
            End If
        Else
            MessageBox.Show("Please select a deployment record to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub btnOfw_Click(sender As Object, e As EventArgs) Handles btnOfw.Click
        Dim newForm As New ofws()
        newForm.Show()     ' Show the new form
        Me.Close()         ' Hide current form (or use Me.Close() to fully close it)
    End Sub

    Private Sub btnDashboard_Click(sender As Object, e As EventArgs) Handles btnDashboard.Click
        Dim newForm As New dashboard()
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

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick
        ' Add logic here if needed
    End Sub
End Class