Imports MySql.Data.MySqlClient

Public Class agcDeployment
    Private Sub agcDeployment_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadDeployments()
        FormatDGVUniformly(DataGridView1)
    End Sub

    Private Sub LoadDeployments()
        Dim agencyId As Integer = Session.CurrentReferenceID
        Dim query As String = "SELECT * FROM deploymentrecord WHERE agency_id = " & agencyId
        readQuery(query)
        Dim dt As New DataTable()
        dt.Load(cmdRead)
        cmdRead.Close()
        DataGridView1.DataSource = dt
    End Sub

    Private Sub ApplyDeploymentFilters()
        Dim agencyId As Integer = Session.CurrentReferenceID
        Dim allCleared As Boolean =
            txtbxIdNum.Text.Trim() = "" AndAlso
            txtbxJobTitle.Text.Trim() = "" AndAlso
            txtbxCountryOfDep.Text.Trim() = "" AndAlso
            txtbxContractNum.Text.Trim() = "" AndAlso
            txtbxSalary.Text.Trim() = "" AndAlso
            cbxContractStat.SelectedIndex = -1 AndAlso
            cbxRepatriationStat.SelectedIndex = -1 AndAlso
            cbxReasonforReturn.SelectedIndex = -1 AndAlso
            Not dateContractStart.Checked AndAlso
            Not dateContractEnd.Checked

        If allCleared Then
            LoadDeployments()
            FormatDGVUniformly(DataGridView1)
            Return
        End If

        Dim query As String = "SELECT * FROM deploymentrecord WHERE agency_id = " & agencyId
        If txtbxIdNum.Text.Trim() <> "" Then
            query &= " AND deployment_id LIKE '%" & txtbxIdNum.Text.Trim() & "%'"
        End If
        If txtbxJobTitle.Text.Trim() <> "" Then
            query &= " AND job_title LIKE '%" & txtbxJobTitle.Text.Trim() & "%'"
        End If
        If txtbxCountryOfDep.Text.Trim() <> "" Then
            query &= " AND country_of_deployment LIKE '%" & txtbxCountryOfDep.Text.Trim() & "%'"
        End If
        If txtbxContractNum.Text.Trim() <> "" Then
            query &= " AND contract_number LIKE '%" & txtbxContractNum.Text.Trim() & "%'"
        End If
        If txtbxSalary.Text.Trim() <> "" Then
            query &= " AND salary LIKE '%" & txtbxSalary.Text.Trim() & "%'"
        End If
        If cbxContractStat.SelectedIndex <> -1 Then
            query &= " AND contract_status = '" & cbxContractStat.SelectedItem.ToString() & "'"
        End If
        If cbxRepatriationStat.SelectedIndex <> -1 Then
            query &= " AND repatriation_status = '" & cbxRepatriationStat.SelectedItem.ToString() & "'"
        End If
        If cbxReasonforReturn.SelectedIndex <> -1 Then
            query &= " AND reason_for_return = '" & cbxReasonforReturn.SelectedItem.ToString() & "'"
        End If
        If dateContractStart.Checked Then
            query &= " AND contract_start >= '" & dateContractStart.Value.ToString("yyyy-MM-dd") & "'"
        End If
        If dateContractEnd.Checked Then
            query &= " AND contract_end <= '" & dateContractEnd.Value.ToString("yyyy-MM-dd") & "'"
        End If
        readQuery(query)
        Dim dt As New DataTable()
        dt.Load(cmdRead)
        cmdRead.Close()
        DataGridView1.DataSource = dt
        FormatDGVUniformly(DataGridView1)
    End Sub

    ' Live filter events
    Private Sub txtbxIdNum_TextChanged(sender As Object, e As EventArgs) Handles txtbxIdNum.TextChanged
        ApplyDeploymentFilters()
    End Sub
    Private Sub txtbxJobTitle_TextChanged(sender As Object, e As EventArgs) Handles txtbxJobTitle.TextChanged
        ApplyDeploymentFilters()
    End Sub
    Private Sub txtbxCountryOfDep_TextChanged(sender As Object, e As EventArgs) Handles txtbxCountryOfDep.TextChanged
        ApplyDeploymentFilters()
    End Sub
    Private Sub txtbxContractNum_TextChanged(sender As Object, e As EventArgs) Handles txtbxContractNum.TextChanged
        ApplyDeploymentFilters()
    End Sub
    Private Sub txtbxSalary_TextChanged(sender As Object, e As EventArgs) Handles txtbxSalary.TextChanged
        ApplyDeploymentFilters()
    End Sub
    Private Sub cbxContractStat_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxContractStat.SelectedIndexChanged
        ApplyDeploymentFilters()
    End Sub
    Private Sub cbxRepatriationStat_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxRepatriationStat.SelectedIndexChanged
        ApplyDeploymentFilters()
    End Sub
    Private Sub cbxReasonforReturn_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxReasonforReturn.SelectedIndexChanged
        ApplyDeploymentFilters()
    End Sub
    Private Sub dateContractStart_ValueChanged(sender As Object, e As EventArgs) Handles dateContractStart.ValueChanged
        ApplyDeploymentFilters()
    End Sub
    Private Sub dateContractEnd_ValueChanged(sender As Object, e As EventArgs) Handles dateContractEnd.ValueChanged
        ApplyDeploymentFilters()
    End Sub

    ' Navigation button handlers
    Private Sub btnDashboard_Click(sender As Object, e As EventArgs) Handles btnDashboard.Click
        Dim dash As New agcDashboard()
        dash.Show()
        Me.Hide()
    End Sub
    Private Sub btnOfws_Click(sender As Object, e As EventArgs) Handles btnOfws.Click
        Dim ofwForm As New agcOfws()
        ofwForm.Show()
        Me.Hide()
    End Sub
    Private Sub btnEmployers_Click(sender As Object, e As EventArgs) Handles btnEmployers.Click
        Dim empForm As New agcEmployers()
        empForm.Show()
        Me.Hide()
    End Sub
    Private Sub btnApplications_Click(sender As Object, e As EventArgs) Handles btnApplications.Click
        Dim appForm As New agcApplications()
        appForm.Show()
        Me.Hide()
    End Sub
    Private Sub btnJobs_Click(sender As Object, e As EventArgs) Handles btnJobs.Click
        Dim jobsForm As New agcJobs()
        jobsForm.Show()
        Me.Hide()
    End Sub

    ' Add Deployment button handler
    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Dim dlg As New addDeployment()
        dlg.ShowDialog()
        LoadDeployments()
        FormatDGVUniformly(DataGridView1)
    End Sub

    ' Edit Deployment button handler
    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        If DataGridView1.SelectedRows.Count > 0 Then
            Dim selectedRow As DataGridViewRow = DataGridView1.SelectedRows(0)
            Dim deploymentId As Integer = Convert.ToInt32(selectedRow.Cells("deployment_id").Value)
            Session.CurrentReferenceID = deploymentId ' Pass deployment_id to editDeployment via session
            Dim dlg As New editDeployment()
            dlg.ShowDialog()
            LoadDeployments()
            FormatDGVUniformly(DataGridView1)
        Else
            MessageBox.Show("Please select a deployment record to edit.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

    End Sub
End Class