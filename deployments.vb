Imports System

Public Class deployments

    Private Sub deployments_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadCustomDeploymentsQuery()
        FormatDGVUniformly(DataGridView1)

        cbxSex.Items.Clear()
        cbxSex.Items.AddRange({"Male", "Female", "Other"})
        cbxSex.SelectedIndex = -1

        cbxDepStat.Items.Clear()
        cbxDepStat.Items.AddRange({"Scheduled", "Deployed", "Completed", "Returned"})
        cbxDepStat.SelectedIndex = -1

        dateDepDate.Format = DateTimePickerFormat.Custom
        dateDepDate.CustomFormat = " "
        dateDepDate.Checked = False

        Me.WindowState = FormWindowState.Maximized
    End Sub

    Private Sub LoadCustomDeploymentsQuery()
        Dim query As String = "
            SELECT 
                CONCAT(o.LastName, ', ', o.FirstName, ' ', o.MiddleName) AS `OFW Name`,
                d.DeploymentID,
                d.DeploymentDate,
                d.CountryOfDeployment,
                d.Salary,
                d.MedicalCleared,
                d.VisaIssued,
                d.POEACleared,
                d.PDOSCompleted,
                d.FlightNumber,
                d.Airport,
                d.DeploymentRemarks,
                d.ContractDuration,
                d.ContractNumber,
                d.DeploymentStatus,
                d.ContractStartDate,
                d.ContractEndDate,
                d.RepatriationStatus,
                d.ReasonForReturn,
                d.ReturnDate,
                d.Remarks
            FROM deploymentrecord d
            LEFT JOIN application a ON d.ApplicationID = a.ApplicationID
            LEFT JOIN ofw o ON a.OFWId = o.OFWId
        "

        LoadToDGV(query, DataGridView1)
    End Sub

    Private Sub ApplyDeploymentFilters()
        Dim allCleared As Boolean =
            txtbxIdNum.Text.Trim() = "" AndAlso
            txtbxFName.Text.Trim() = "" AndAlso
            txtbxMName.Text.Trim() = "" AndAlso
            txtbxLName.Text.Trim() = "" AndAlso
            cbxSex.SelectedIndex = -1 AndAlso
            txtbxCountryOfDep.Text.Trim() = "" AndAlso
            txtbxVisaNum.Text.Trim() = "" AndAlso
            txtbxOecNum.Text.Trim() = "" AndAlso
            cbxDepStat.SelectedIndex = -1 AndAlso
            Not dateDepDate.Checked

        If allCleared Then
            LoadCustomDeploymentsQuery()
            FormatDGVUniformly(DataGridView1)
            Return
        End If

        Dim query As String = "
            SELECT 
                CONCAT(o.LastName, ', ', o.FirstName, ' ', o.MiddleName) AS `OFW Name`,
                d.DeploymentID,
                d.DeploymentDate,
                d.CountryOfDeployment,
                d.Salary,
                d.MedicalCleared,
                d.VisaIssued,
                d.POEACleared,
                d.PDOSCompleted,
                d.FlightNumber,
                d.Airport,
                d.DeploymentRemarks,
                d.ContractDuration,
                d.ContractNumber,
                d.DeploymentStatus,
                d.ContractStartDate,
                d.ContractEndDate,
                d.RepatriationStatus,
                d.ReasonForReturn,
                d.ReturnDate,
                d.Remarks
            FROM deploymentrecord d
            LEFT JOIN application a ON d.ApplicationID = a.ApplicationID
            LEFT JOIN ofw o ON a.OFWId = o.OFWId
            WHERE 1=1
        "

        If txtbxIdNum.Text.Trim() <> "" Then
            query &= " AND o.OFWId LIKE '%" & txtbxIdNum.Text.Trim() & "%'"
        End If

        If txtbxFName.Text.Trim() <> "" Then
            query &= " AND o.FirstName LIKE '%" & txtbxFName.Text.Trim() & "%'"
        End If

        If txtbxMName.Text.Trim() <> "" Then
            query &= " AND o.MiddleName LIKE '%" & txtbxMName.Text.Trim() & "%'"
        End If

        If txtbxLName.Text.Trim() <> "" Then
            query &= " AND o.LastName LIKE '%" & txtbxLName.Text.Trim() & "%'"
        End If

        If cbxSex.SelectedIndex <> -1 Then
            query &= " AND o.Sex = '" & cbxSex.SelectedItem.ToString() & "'"
        End If

        If txtbxCountryOfDep.Text.Trim() <> "" Then
            query &= " AND d.CountryOfDeployment LIKE '%" & txtbxCountryOfDep.Text.Trim() & "%'"
        End If

        If txtbxVisaNum.Text.Trim() <> "" Then
            query &= " AND o.VISANum LIKE '%" & txtbxVisaNum.Text.Trim() & "%'"
        End If

        If txtbxOecNum.Text.Trim() <> "" Then
            query &= " AND o.OECNum LIKE '%" & txtbxOecNum.Text.Trim() & "%'"
        End If

        If cbxDepStat.SelectedIndex <> -1 Then
            query &= " AND d.DeploymentStatus = '" & cbxDepStat.SelectedItem.ToString() & "'"
        End If

        If dateDepDate.Checked Then
            query &= " AND d.DeploymentDate = '" & dateDepDate.Value.ToString("yyyy-MM-dd") & "'"
        End If

        LoadToDGV(query, DataGridView1)
        FormatDGVUniformly(DataGridView1)
    End Sub

    ' Live filter events
    Private Sub txtbxIdNum_TextChanged(sender As Object, e As EventArgs) Handles txtbxIdNum.TextChanged
        ApplyDeploymentFilters()
    End Sub

    Private Sub txtbxFName_TextChanged(sender As Object, e As EventArgs) Handles txtbxFName.TextChanged
        ApplyDeploymentFilters()
    End Sub

    Private Sub txtbxMName_TextChanged(sender As Object, e As EventArgs) Handles txtbxMName.TextChanged
        ApplyDeploymentFilters()
    End Sub

    Private Sub txtbxLName_TextChanged(sender As Object, e As EventArgs) Handles txtbxLName.TextChanged
        ApplyDeploymentFilters()
    End Sub

    Private Sub cbxSex_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxSex.SelectedIndexChanged
        ApplyDeploymentFilters()
    End Sub

    Private Sub txtbxCountryOfDep_TextChanged(sender As Object, e As EventArgs) Handles txtbxCountryOfDep.TextChanged
        ApplyDeploymentFilters()
    End Sub

    Private Sub txtbxVisaNum_TextChanged(sender As Object, e As EventArgs) Handles txtbxVisaNum.TextChanged
        ApplyDeploymentFilters()
    End Sub

    Private Sub txtbxOecNum_TextChanged(sender As Object, e As EventArgs) Handles txtbxOecNum.TextChanged
        ApplyDeploymentFilters()
    End Sub

    Private Sub cbxDepStat_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxDepStat.SelectedIndexChanged
        ApplyDeploymentFilters()
    End Sub

    Private Sub dateDepDate_ValueChanged(sender As Object, e As EventArgs) Handles dateDepDate.ValueChanged
        ApplyDeploymentFilters()
    End Sub

    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        txtbxIdNum.Clear()
        txtbxFName.Clear()
        txtbxMName.Clear()
        txtbxLName.Clear()
        cbxSex.SelectedIndex = -1
        txtbxCountryOfDep.Clear()
        txtbxVisaNum.Clear()
        txtbxOecNum.Clear()
        cbxDepStat.SelectedIndex = -1
        dateDepDate.Checked = False

        LoadCustomDeploymentsQuery()
        FormatDGVUniformly(DataGridView1)
    End Sub

    ' CRUD Buttons
    Private Sub btnAdd_Click(sender As Object, e As EventArgs)
        Dim dlg As New addDeployment()
        dlg.ShowDialog()
        LoadCustomDeploymentsQuery()
        FormatDGVUniformly(DataGridView1)
    End Sub

    ' REPORT GENERATION
    Private Sub btnGenerate_Click(sender As Object, e As EventArgs) Handles btnGenerate.Click
        Try
            Dim reportGenerator As New GenerateDeploymentsGeneralReportData()
            If reportGenerator.GenerateReport() Then
                MessageBox.Show("Deployments report generated successfully! Check your Desktop for the PDF file.", "Report Generated", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                MessageBox.Show("Failed to generate deployments report.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Catch ex As Exception
            MessageBox.Show("Error generating report: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnEdit_Click(sender As Object, e As EventArgs)
        If DataGridView1.SelectedRows.Count > 0 Then
            Dim selectedRow As DataGridViewRow = DataGridView1.SelectedRows(0)
            Dim deploymentId As Integer = Convert.ToInt32(selectedRow.Cells("DeploymentID").Value)

            Dim dlg As New editDeployment(deploymentId)
            dlg.ShowDialog()

            LoadCustomDeploymentsQuery()
            FormatDGVUniformly(DataGridView1)
        Else
            MessageBox.Show("Please select a deployment record to edit.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub


    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If DataGridView1.SelectedRows.Count > 0 Then
            Dim selectedRow As DataGridViewRow = DataGridView1.SelectedRows(0)
            Dim deploymentId As Integer = Convert.ToInt32(selectedRow.Cells("DeploymentID").Value)
            Dim result As DialogResult = MessageBox.Show("Are you sure you want to delete this deployment record?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)

            If result = DialogResult.Yes Then
                DeleteRecord("DeploymentRecord", "DeploymentID", deploymentId)
                LoadCustomDeploymentsQuery()
                FormatDGVUniformly(DataGridView1)
            End If
        Else
            MessageBox.Show("Please select a deployment record to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    ' Navigation
    Private Sub btnOfw_Click(sender As Object, e As EventArgs) Handles btnOfw.Click
        Dim newForm As New ofws()
        newForm.Show()
        Me.Close()
    End Sub

    Private Sub btnDashboard_Click(sender As Object, e As EventArgs) Handles btnDashboard.Click
        Dim newForm As New dashboard()
        newForm.Show()
        Me.Hide()
    End Sub

    Private Sub btnAgencies_Click(sender As Object, e As EventArgs) Handles btnAgencies.Click
        Dim newForm As New agencies()
        newForm.Show()
        Me.Hide()
    End Sub

    Private Sub btnEmployers_Click(sender As Object, e As EventArgs) Handles btnEmployers.Click
        Dim newForm As New employers()
        newForm.Show()
        Me.Hide()
    End Sub

    Private Sub btnConfig_Click(sender As Object, e As EventArgs) Handles btnConfig.Click
        Dim newForm As New AdminConfiguration()
        newForm.Show()
        Me.Hide()
    End Sub
End Class
