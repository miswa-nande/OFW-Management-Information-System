Imports MySql.Data.MySqlClient

Public Class agcDeployment
    Private Sub agcDeployment_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadDeployments()
        FormatDGVUniformly(DataGridView1)
        PopulateAllComboboxes()
    End Sub

    Private Sub LoadDeployments()
        Dim agencyId As Integer = Session.CurrentReferenceID
        Dim query As String = "
            SELECT dr.*, CONCAT(o.FirstName, ' ', o.LastName) AS OFWName
            FROM deploymentrecord dr
            LEFT JOIN ofw o ON dr.ApplicationID = o.OFWID
            WHERE dr.AgencyID = " & agencyId

        readQuery(query)
        Dim dt As New DataTable()
        dt.Load(cmdRead)
        cmdRead.Close()
        DataGridView1.DataSource = dt
    End Sub

    Private Sub PopulateAllComboboxes()
        Dim agencyId As Integer = Session.CurrentReferenceID

        ' Salary
        cbxSalary.Items.Clear()
        readQuery($"SELECT DISTINCT Salary FROM deploymentrecord WHERE AgencyID = {agencyId} AND Salary IS NOT NULL ORDER BY Salary")
        While cmdRead.Read()
            cbxSalary.Items.Add(cmdRead("Salary").ToString())
        End While
        cmdRead.Close()

        ' Deployment Status
        cbxDeploymentStatus.Items.Clear()
        readQuery($"SELECT DISTINCT DeploymentStatus FROM deploymentrecord WHERE AgencyID = {agencyId} AND DeploymentStatus IS NOT NULL")
        While cmdRead.Read()
            cbxDeploymentStatus.Items.Add(cmdRead("DeploymentStatus").ToString())
        End While
        cmdRead.Close()

        ' Repatriation Status
        cbxRepatriationStat.Items.Clear()
        readQuery($"SELECT DISTINCT RepatriationStatus FROM deploymentrecord WHERE AgencyID = {agencyId} AND RepatriationStatus IS NOT NULL")
        While cmdRead.Read()
            cbxRepatriationStat.Items.Add(cmdRead("RepatriationStatus").ToString())
        End While
        cmdRead.Close()

        ' Reason for Return
        cbxReasonforReturn.Items.Clear()
        readQuery($"SELECT DISTINCT ReasonForReturn FROM deploymentrecord WHERE AgencyID = {agencyId} AND ReasonForReturn IS NOT NULL")
        While cmdRead.Read()
            cbxReasonforReturn.Items.Add(cmdRead("ReasonForReturn").ToString())
        End While
        cmdRead.Close()

        ' Reset
        cbxSalary.SelectedIndex = -1
        cbxDeploymentStatus.SelectedIndex = -1
        cbxRepatriationStat.SelectedIndex = -1
        cbxReasonforReturn.SelectedIndex = -1
    End Sub

    Private Sub ApplyDeploymentFilters()
        Dim agencyId As Integer = Session.CurrentReferenceID
        Dim allCleared As Boolean =
            txtbxIdNum.Text.Trim() = "" AndAlso
            txtbxJobTitle.Text.Trim() = "" AndAlso
            txtbxCountryOfDep.Text.Trim() = "" AndAlso
            txtbxContractNum.Text.Trim() = "" AndAlso
            cbxSalary.SelectedIndex = -1 AndAlso
            cbxDeploymentStatus.SelectedIndex = -1 AndAlso
            cbxRepatriationStat.SelectedIndex = -1 AndAlso
            cbxReasonforReturn.SelectedIndex = -1 AndAlso
            Not dateContractStart.Checked AndAlso
            Not dateContractEnd.Checked

        If allCleared Then
            LoadDeployments()
            FormatDGVUniformly(DataGridView1)
            Return
        End If

        Dim query As String = "
            SELECT dr.*, CONCAT(o.FirstName, ' ', o.LastName) AS OFWName
            FROM deploymentrecord dr
            LEFT JOIN ofw o ON dr.ApplicationID = o.OFWID
            WHERE dr.AgencyID = " & agencyId

        If txtbxIdNum.Text.Trim() <> "" Then
            query &= " AND dr.DeploymentID LIKE '%" & txtbxIdNum.Text.Trim() & "%'"
        End If
        If txtbxJobTitle.Text.Trim() <> "" Then
            query &= " AND dr.JobPlacementID IN (SELECT JobPlacementID FROM jobplacement WHERE JobTitle LIKE '%" & txtbxJobTitle.Text.Trim() & "%')"
        End If
        If txtbxCountryOfDep.Text.Trim() <> "" Then
            query &= " AND dr.CountryOfDeployment LIKE '%" & txtbxCountryOfDep.Text.Trim() & "%'"
        End If
        If txtbxContractNum.Text.Trim() <> "" Then
            query &= " AND dr.ContractNumber LIKE '%" & txtbxContractNum.Text.Trim() & "%'"
        End If
        If cbxSalary.SelectedIndex <> -1 Then
            query &= " AND dr.Salary = '" & cbxSalary.SelectedItem.ToString() & "'"
        End If
        If cbxDeploymentStatus.SelectedIndex <> -1 Then
            query &= " AND dr.DeploymentStatus = '" & cbxDeploymentStatus.SelectedItem.ToString() & "'"
        End If
        If cbxRepatriationStat.SelectedIndex <> -1 Then
            query &= " AND dr.RepatriationStatus = '" & cbxRepatriationStat.SelectedItem.ToString() & "'"
        End If
        If cbxReasonforReturn.SelectedIndex <> -1 Then
            query &= " AND dr.ReasonForReturn = '" & cbxReasonforReturn.SelectedItem.ToString() & "'"
        End If
        If dateContractStart.Checked Then
            query &= " AND dr.ContractStartDate >= '" & dateContractStart.Value.ToString("yyyy-MM-dd") & "'"
        End If
        If dateContractEnd.Checked Then
            query &= " AND dr.ContractEndDate <= '" & dateContractEnd.Value.ToString("yyyy-MM-dd") & "'"
        End If

        readQuery(query)
        Dim dt As New DataTable()
        dt.Load(cmdRead)
        cmdRead.Close()
        DataGridView1.DataSource = dt
        FormatDGVUniformly(DataGridView1)
    End Sub

    ' Filter Events
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
    Private Sub cbxSalary_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxSalary.SelectedIndexChanged
        ApplyDeploymentFilters()
    End Sub
    Private Sub cbxDeploymentStatus_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxDeploymentStatus.SelectedIndexChanged
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

    ' Navigation
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

    ' Add Deployment
    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Dim dlg As New addDeployment()
        dlg.ShowDialog()
        LoadDeployments()
        FormatDGVUniformly(DataGridView1)
        PopulateAllComboboxes()
    End Sub

    ' Edit Deployment
    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        If DataGridView1.SelectedRows.Count > 0 Then
            Dim selectedRow As DataGridViewRow = DataGridView1.SelectedRows(0)
            Dim deploymentId As Integer = Convert.ToInt32(selectedRow.Cells("DeploymentID").Value)
            Session.CurrentReferenceID = deploymentId
            Dim dlg As New editDeployment()
            dlg.ShowDialog()
            LoadDeployments()
            FormatDGVUniformly(DataGridView1)
            PopulateAllComboboxes()
        Else
            MessageBox.Show("Please select a deployment record to edit.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    ' Clear Filters
    Private Sub ClearBTN_Click(sender As Object, e As EventArgs) Handles ClearBTN.Click
        txtbxIdNum.Clear()
        txtbxJobTitle.Clear()
        txtbxCountryOfDep.Clear()
        txtbxContractNum.Clear()
        cbxSalary.SelectedIndex = -1
        cbxDeploymentStatus.SelectedIndex = -1
        cbxRepatriationStat.SelectedIndex = -1
        cbxReasonforReturn.SelectedIndex = -1
        dateContractStart.Checked = False
        dateContractEnd.Checked = False
        dateContractStart.Value = Date.Today
        dateContractEnd.Value = Date.Today

        LoadDeployments()
        FormatDGVUniformly(DataGridView1)
        PopulateAllComboboxes()
    End Sub

    ' Deployment Tracking Form
    Private Sub OFWDeploymentTarcking_Click(sender As Object, e As EventArgs) Handles OFWDeploymentTarcking.Click
        Dim trackingForm As New agcDeploymentTracking()
        trackingForm.Show()
        Me.Hide()
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If DataGridView1.SelectedRows.Count > 0 Then
            Dim selectedRow As DataGridViewRow = DataGridView1.SelectedRows(0)
            Dim deploymentId As Integer = Convert.ToInt32(selectedRow.Cells("DeploymentID").Value)

            Dim result As DialogResult = MessageBox.Show(
                "Are you sure you want to delete this deployment record? This action cannot be undone.",
                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)

            If result = DialogResult.Yes Then
                Try
                    Using conn As New MySqlConnection(strConnection)
                        conn.Open()
                        Dim query As String = "DELETE FROM deploymentrecord WHERE DeploymentID = @id"
                        Using cmd As New MySqlCommand(query, conn)
                            cmd.Parameters.AddWithValue("@id", deploymentId)
                            cmd.ExecuteNonQuery()
                        End Using
                    End Using

                    MessageBox.Show("Deployment record deleted successfully!", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information)

                    ' Refresh the data
                    LoadDeployments()
                    FormatDGVUniformly(DataGridView1)
                    PopulateAllComboboxes()
                Catch ex As Exception
                    MessageBox.Show("Error deleting deployment record: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End If
        Else
            MessageBox.Show("Please select a deployment record to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

End Class
