Public Class agencies

    Private Sub agencies_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadAgenciesToDGV(DataGridView1)
        FormatDGVUniformly(DataGridView1)
        cbxGovtAccredStat.Items.AddRange({"Accredited", "Not Accredited", "Pending"})
    End Sub

    Private Sub btnDeployments_Click(sender As Object, e As EventArgs) Handles btnDeployments.Click
        Dim newForm As New deployments()
        newForm.Show()
        Me.Hide()
    End Sub

    Private Sub btnDashboard_Click(sender As Object, e As EventArgs) Handles btnDashboard.Click
        Dim newForm As New dashboard()
        newForm.Show()
        Me.Hide()
    End Sub

    Private Sub btnOfw_Click(sender As Object, e As EventArgs) Handles btnOfw.Click
        Dim newForm As New ofws()
        newForm.Show()
        Me.Hide()
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Dim dlg As New addAgency()
        dlg.ShowDialog()
        LoadAgenciesToDGV(DataGridView1)
        FormatDGVUniformly(DataGridView1)
    End Sub

    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        Dim dlg As New editAgency()
        dlg.ShowDialog()
        LoadAgenciesToDGV(DataGridView1)
        FormatDGVUniformly(DataGridView1)
    End Sub

    Private Sub btnJobPlacements_Click(sender As Object, e As EventArgs) Handles btnJobPlacements.Click
        Dim newForm As New jobplacement()
        newForm.Show()
        Me.Hide()
    End Sub

    Private Sub btnEmployers_Click(sender As Object, e As EventArgs) Handles btnEmployers.Click
        Dim newForm As New employers()
        newForm.Show()
        Me.Hide()
    End Sub

    Private Sub ApplyAgencyFilters()
        Dim allCleared As Boolean =
            txtbxIdNum.Text.Trim() = "" AndAlso
            txtbxAgencyName.Text.Trim() = "" AndAlso
            txtbxAgencyLicNum.Text.Trim() = "" AndAlso
            txtbxContactNum.Text.Trim() = "" AndAlso
            txtbxSpecialization.Text.Trim() = "" AndAlso
            cbxGovtAccredStat.SelectedIndex = -1 AndAlso
            txtbxNumDepWorkers.Text.Trim() = "" AndAlso
            txtbxNumActiveJobs.Text.Trim() = ""

        If allCleared Then
            LoadAgenciesToDGV(DataGridView1)
            FormatDGVUniformly(DataGridView1)
            Return
        End If

        Dim query As String = "SELECT * FROM agency WHERE 1=1"

        If txtbxIdNum.Text.Trim() <> "" Then
            query &= " AND AgencyID LIKE '%" & txtbxIdNum.Text.Trim() & "%'"
        End If

        If txtbxAgencyName.Text.Trim() <> "" Then
            query &= " AND AgencyName LIKE '%" & txtbxAgencyName.Text.Trim() & "%'"
        End If

        If txtbxAgencyLicNum.Text.Trim() <> "" Then
            query &= " AND LicenseNumber LIKE '%" & txtbxAgencyLicNum.Text.Trim() & "%'"
        End If

        If txtbxContactNum.Text.Trim() <> "" Then
            query &= " AND ContactNumber LIKE '%" & txtbxContactNum.Text.Trim() & "%'"
        End If

        If txtbxSpecialization.Text.Trim() <> "" Then
            query &= " AND Specialization LIKE '%" & txtbxSpecialization.Text.Trim() & "%'"
        End If

        If cbxGovtAccredStat.SelectedIndex <> -1 Then
            query &= " AND AccreditationStatus = '" & cbxGovtAccredStat.SelectedItem.ToString() & "'"
        End If

        If txtbxNumDepWorkers.Text.Trim() <> "" Then
            query &= " AND NumDeployedWorkers LIKE '%" & txtbxNumDepWorkers.Text.Trim() & "%'"
        End If

        If txtbxNumActiveJobs.Text.Trim() <> "" Then
            query &= " AND NumActiveJobOrders LIKE '%" & txtbxNumActiveJobs.Text.Trim() & "%'"
        End If

        readQuery(query)
        Dim dt As New DataTable()
        dt.Load(cmdRead)
        cmdRead.Close()

        DataGridView1.DataSource = dt
        FormatDGVUniformly(DataGridView1)
    End Sub

    Private Sub txtbxIdNum_TextChanged(sender As Object, e As EventArgs) Handles txtbxIdNum.TextChanged
        ApplyAgencyFilters()
    End Sub

    Private Sub txtbxAgencyName_TextChanged(sender As Object, e As EventArgs) Handles txtbxAgencyName.TextChanged
        ApplyAgencyFilters()
    End Sub

    Private Sub txtbxAgencyLicNum_TextChanged(sender As Object, e As EventArgs) Handles txtbxAgencyLicNum.TextChanged
        ApplyAgencyFilters()
    End Sub

    Private Sub txtbxContactNum_TextChanged(sender As Object, e As EventArgs) Handles txtbxContactNum.TextChanged
        ApplyAgencyFilters()
    End Sub

    Private Sub txtbxSpecialization_TextChanged(sender As Object, e As EventArgs) Handles txtbxSpecialization.TextChanged
        ApplyAgencyFilters()
    End Sub

    Private Sub cbxGovtAccredStat_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxGovtAccredStat.SelectedIndexChanged
        ApplyAgencyFilters()
    End Sub

    Private Sub txtbxNumDepWorkers_TextChanged(sender As Object, e As EventArgs) Handles txtbxNumDepWorkers.TextChanged
        ApplyAgencyFilters()
    End Sub

    Private Sub txtbxNumActiveJobs_TextChanged(sender As Object, e As EventArgs) Handles txtbxNumActiveJobs.TextChanged
        ApplyAgencyFilters()
    End Sub

    Private Sub btnFilter_Click(sender As Object, e As EventArgs) Handles btnFilter.Click
        ApplyAgencyFilters()
    End Sub

End Class