Imports MySql.Data.MySqlClient

Public Class agcDeploymentTracking
    Private selectedDeploymentId As Integer = -1

    Private Sub agcDeploymentTracking_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DisableAllFields()
        PopulateComboBoxes()
        LoadDeployments()
    End Sub

    Private Sub PopulateComboBoxes()
        ' Populate Yes/No combo boxes
        cbxMedical.Items.Clear()
        cbxVisa.Items.Clear()
        cbxPOEA.Items.Clear()
        cbxPDOS.Items.Clear()

        Dim yesNoOptions = {"Yes", "No"}
        cbxMedical.Items.AddRange(yesNoOptions)
        cbxVisa.Items.AddRange(yesNoOptions)
        cbxPOEA.Items.AddRange(yesNoOptions)
        cbxPDOS.Items.AddRange(yesNoOptions)

        ' Deployment status options
        cbxDepStat.Items.Clear()
        cbxDepStat.Items.AddRange({"Scheduled", "Deployed", "Completed", "Returned"})
    End Sub

    Private Sub DisableAllFields()
        cbxMedical.Enabled = False
        cbxVisa.Enabled = False
        cbxPOEA.Enabled = False
        cbxPDOS.Enabled = False
        txtbxFlightNumber.Enabled = False
        txtbxAirport.Enabled = False
        cbxDepStat.Enabled = False
        txtbxRemarks.Enabled = False
    End Sub

    Private Sub EnableRequirementFields()
        cbxMedical.Enabled = True
        cbxVisa.Enabled = True
        cbxPOEA.Enabled = True
        cbxPDOS.Enabled = True
    End Sub

    Private Sub Requirement_Changed(sender As Object, e As EventArgs) Handles cbxMedical.SelectedIndexChanged, cbxVisa.SelectedIndexChanged, cbxPOEA.SelectedIndexChanged, cbxPDOS.SelectedIndexChanged
        If selectedDeploymentId = -1 Then Exit Sub

        Dim query As String = $"
            UPDATE deploymentrecord SET
                MedicalCleared = {(cbxMedical.Text = "Yes")},
                VisaIssued = {(cbxVisa.Text = "Yes")},
                POEACleared = {(cbxPOEA.Text = "Yes")},
                PDOSCompleted = {(cbxPDOS.Text = "Yes")}
            WHERE DeploymentID = {selectedDeploymentId}"
        executeQuery(query)

        Dim allYes As Boolean = cbxMedical.Text = "Yes" AndAlso cbxVisa.Text = "Yes" AndAlso cbxPOEA.Text = "Yes" AndAlso cbxPDOS.Text = "Yes"
        txtbxFlightNumber.Enabled = allYes
        txtbxAirport.Enabled = allYes
    End Sub

    Private Sub txtbxFlightNumber_TextChanged(sender As Object, e As EventArgs) Handles txtbxFlightNumber.TextChanged
        SaveFlightInfo()
        CheckFlightCompletion()
    End Sub

    Private Sub txtbxAirport_TextChanged(sender As Object, e As EventArgs) Handles txtbxAirport.TextChanged
        SaveFlightInfo()
        CheckFlightCompletion()
    End Sub

    Private Sub SaveFlightInfo()
        If selectedDeploymentId = -1 Then Exit Sub
        Dim query As String = $"
            UPDATE deploymentrecord SET
                FlightNumber = '{txtbxFlightNumber.Text.Trim()}',
                Airport = '{txtbxAirport.Text.Trim()}'
            WHERE DeploymentID = {selectedDeploymentId}"
        executeQuery(query)
    End Sub

    Private Sub CheckFlightCompletion()
        Dim allFilled As Boolean = txtbxFlightNumber.Text.Trim() <> "" AndAlso txtbxAirport.Text.Trim() <> ""
        cbxDepStat.Enabled = allFilled
        txtbxRemarks.Enabled = allFilled
    End Sub

    Private Sub cbxDepStat_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxDepStat.SelectedIndexChanged
        If selectedDeploymentId = -1 Then Exit Sub
        Dim query As String = $"UPDATE deploymentrecord SET DeploymentStatus = '{cbxDepStat.Text}' WHERE DeploymentID = {selectedDeploymentId}"
        executeQuery(query)
    End Sub

    Private Sub txtbxRemarks_TextChanged(sender As Object, e As EventArgs) Handles txtbxRemarks.TextChanged
        If selectedDeploymentId = -1 Then Exit Sub
        Dim query As String = $"UPDATE deploymentrecord SET DeploymentRemarks = '{txtbxRemarks.Text.Trim()}' WHERE DeploymentID = {selectedDeploymentId}"
        executeQuery(query)
    End Sub

    Private Sub UpdateRequirementsBTN_Click(sender As Object, e As EventArgs) Handles UpdateRequirementsBTN.Click
        If DataGridView1.SelectedRows.Count > 0 Then
            Dim row As DataGridViewRow = DataGridView1.SelectedRows(0)
            selectedDeploymentId = Convert.ToInt32(row.Cells("DeploymentID").Value)

            ' Enable editing
            EnableRequirementFields()

            ' Pre-fill fields from selected row
            cbxMedical.SelectedItem = If(Convert.ToBoolean(row.Cells("MedicalCleared").Value), "Yes", "No")
            cbxVisa.SelectedItem = If(Convert.ToBoolean(row.Cells("VisaIssued").Value), "Yes", "No")
            cbxPOEA.SelectedItem = If(Convert.ToBoolean(row.Cells("POEACleared").Value), "Yes", "No")
            cbxPDOS.SelectedItem = If(Convert.ToBoolean(row.Cells("PDOSCompleted").Value), "Yes", "No")
            txtbxFlightNumber.Text = row.Cells("FlightNumber").Value.ToString()
            txtbxAirport.Text = row.Cells("Airport").Value.ToString()
            cbxDepStat.SelectedItem = row.Cells("DeploymentStatus").Value.ToString()
            txtbxRemarks.Text = row.Cells("DeploymentRemarks").Value.ToString()

            CheckFlightCompletion()
        Else
            MessageBox.Show("Please select a deployment record first.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub

    Private Sub ClearBTN_Click(sender As Object, e As EventArgs) Handles ClearBTN.Click
        txtbxOFWName.Clear()
        txtbxApplicationID.Clear()
        txtbxCountryOfDeployment.Clear()
        cbxDeploymentStatus.SelectedIndex = -1
        DisableAllFields()
        LoadDeployments()
    End Sub

    Private Sub executeQuery(query As String)
        Try
            readQuery(query)
            cmdRead.Close()
        Catch ex As Exception
            MessageBox.Show("Database error: " & ex.Message)
        End Try
    End Sub

    Private Sub LoadDeployments()
        Dim query As String = "SELECT * FROM deploymentrecord"
        readQuery(query)
        Dim dt As New DataTable()
        dt.Load(cmdRead)
        cmdRead.Close()
        DataGridView1.DataSource = dt
    End Sub

    ' Filter logic
    Private Sub ApplyFilters()
        Dim conditions As New List(Of String)()

        If txtbxOFWName.Text.Trim() <> "" Then
            conditions.Add($"CONCAT_WS(' ', FirstName, LastName) LIKE '%{txtbxOFWName.Text.Trim()}%'")
        End If
        If txtbxApplicationID.Text.Trim() <> "" Then
            conditions.Add($"ApplicationID LIKE '%{txtbxApplicationID.Text.Trim()}%'")
        End If
        If txtbxCountryOfDeployment.Text.Trim() <> "" Then
            conditions.Add($"CountryOfDeployment LIKE '%{txtbxCountryOfDeployment.Text.Trim()}%'")
        End If
        If cbxDeploymentStatus.SelectedIndex <> -1 Then
            conditions.Add($"DeploymentStatus = '{cbxDeploymentStatus.SelectedItem.ToString()}'")
        End If

        Dim query As String = "SELECT * FROM deploymentrecord"
        If conditions.Count > 0 Then
            query &= " WHERE " & String.Join(" AND ", conditions)
        End If

        readQuery(query)
        Dim dt As New DataTable()
        dt.Load(cmdRead)
        cmdRead.Close()
        DataGridView1.DataSource = dt
    End Sub

    Private Sub cbxDeploymentStatus_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxDeploymentStatus.SelectedIndexChanged
        ApplyFilters()
    End Sub

    Private Sub txtbxOFWName_TextChanged(sender As Object, e As EventArgs) Handles txtbxOFWName.TextChanged
        ApplyFilters()
    End Sub

    Private Sub txtbxApplicationID_TextChanged(sender As Object, e As EventArgs) Handles txtbxApplicationID.TextChanged
        ApplyFilters()
    End Sub

    Private Sub txtbxCountryOfDeployment_TextChanged(sender As Object, e As EventArgs) Handles txtbxCountryOfDeployment.TextChanged
        ApplyFilters()
    End Sub

    Private Sub MarkasdeployedBTN_Click(sender As Object, e As EventArgs) Handles MarkasdeployedBTN.Click
        If selectedDeploymentId = -1 Then
            MessageBox.Show("Please select a record to mark as deployed.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        Dim query As String = $"UPDATE deploymentrecord SET DeploymentStatus = 'Deployed' WHERE DeploymentID = {selectedDeploymentId}"
        executeQuery(query)
        MessageBox.Show("Deployment status updated to 'Deployed'.")
        LoadDeployments()
    End Sub

    ' Navigation buttons
    Private Sub btnEmployers_Click(sender As Object, e As EventArgs) Handles btnEmployers.Click
        Dim f As New agcEmployers()
        f.Show()
        Me.Hide()
    End Sub

    Private Sub btnOfws_Click(sender As Object, e As EventArgs) Handles btnOfws.Click
        Dim f As New agcOfws()
        f.Show()
        Me.Hide()
    End Sub

    Private Sub OFWDeploymentTarcking_Click(sender As Object, e As EventArgs) Handles OFWDeploymentTarcking.Click
        Dim f As New agcDeploymentTracking()
        f.Show()
        Me.Hide()
    End Sub

    Private Sub btnDeployments_Click(sender As Object, e As EventArgs) Handles btnDeployments.Click
        Dim f As New agcDeployment()
        f.Show()
        Me.Hide()
    End Sub

    Private Sub btnApplications_Click(sender As Object, e As EventArgs) Handles btnApplications.Click
        Dim f As New agcApplications()
        f.Show()
        Me.Hide()
    End Sub

    Private Sub btnJobs_Click(sender As Object, e As EventArgs) Handles btnJobs.Click
        Dim f As New agcJobs()
        f.Show()
        Me.Hide()
    End Sub

    Private Sub btnDashboard_Click(sender As Object, e As EventArgs) Handles btnDashboard.Click
        Dim f As New agcDashboard()
        f.Show()
        Me.Hide()
    End Sub

    Private Sub SaveBTN_Click(sender As Object, e As EventArgs) Handles SaveBTN.Click
        MessageBox.Show("All changes are saved automatically when you make updates.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Dim addForm As New addDeployment()
        addForm.ShowDialog()
        LoadDeployments()
    End Sub

    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        If DataGridView1.SelectedRows.Count = 0 Then
            MessageBox.Show("Please select a deployment to edit.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim selectedRow As DataGridViewRow = DataGridView1.SelectedRows(0)
        Dim deploymentId As Integer = Convert.ToInt32(selectedRow.Cells("DeploymentID").Value)

        Dim editForm As New editDeployment(deploymentId)
        editForm.ShowDialog()

        LoadDeployments()
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If DataGridView1.SelectedRows.Count = 0 Then
            MessageBox.Show("Please select a deployment to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim selectedRow As DataGridViewRow = DataGridView1.SelectedRows(0)
        Dim deploymentId As Integer = Convert.ToInt32(selectedRow.Cells("DeploymentID").Value)

        Dim confirmResult As DialogResult = MessageBox.Show(
            "Are you sure you want to delete this deployment record? This action cannot be undone.",
            "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)

        If confirmResult = DialogResult.Yes Then
            Try
                Dim query As String = $"DELETE FROM deploymentrecord WHERE DeploymentID = {deploymentId}"
                executeQuery(query)
                MessageBox.Show("Deployment record deleted successfully!", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information)
                LoadDeployments()
            Catch ex As Exception
                MessageBox.Show("Error deleting deployment: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub
End Class
