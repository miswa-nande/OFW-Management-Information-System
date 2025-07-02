Imports MySql.Data.MySqlClient

Public Class agcJobs
    Private Sub agcJobs_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadAgencyJobs()
        FormatDGVUniformly(DGVJobOffers)
        PopulateFilterComboboxes()
    End Sub

    Private Sub LoadAgencyJobs()
        Dim agencyId As Integer = Session.CurrentReferenceID
        Dim query As String = $"SELECT * FROM jobplacement WHERE agency_id = {agencyId}"
        readQuery(query)
        Dim dt As New DataTable()
        dt.Load(cmdRead)
        cmdRead.Close()
        DGVJobOffers.DataSource = dt
    End Sub

    Private Sub ApplyJobFilters()
        Dim agencyId As Integer = Session.CurrentReferenceID
        Dim allCleared As Boolean =
            txtbxJobIdNum.Text.Trim() = "" AndAlso
            txtbxJobTitle.Text.Trim() = "" AndAlso
            txtbxJobType.Text.Trim() = "" AndAlso
            txtbxReqSkill.Text.Trim() = "" AndAlso
            txtbxSalaryRange.Text.Trim() = "" AndAlso
            cbxCountry.SelectedIndex = -1 AndAlso
            cbxVisaType.SelectedIndex = -1 AndAlso
            Not dateApplicationDeadline.Checked

        If allCleared Then
            LoadAgencyJobs()
            FormatDGVUniformly(DGVJobOffers)
            Return
        End If

        Dim query As String = $"SELECT * FROM jobplacement WHERE agency_id = {agencyId}"

        If txtbxJobIdNum.Text.Trim() <> "" Then
            query &= " AND job_id LIKE '%" & txtbxJobIdNum.Text.Trim() & "%'"
        End If
        If txtbxJobTitle.Text.Trim() <> "" Then
            query &= " AND job_title LIKE '%" & txtbxJobTitle.Text.Trim() & "%'"
        End If
        If txtbxJobType.Text.Trim() <> "" Then
            query &= " AND job_type LIKE '%" & txtbxJobType.Text.Trim() & "%'"
        End If
        If txtbxReqSkill.Text.Trim() <> "" Then
            query &= " AND skill_id IN (SELECT skill_id FROM skill WHERE skill_name LIKE '%" & txtbxReqSkill.Text.Trim() & "%')"
        End If
        If txtbxSalaryRange.Text.Trim() <> "" Then
            query &= " AND salary LIKE '%" & txtbxSalaryRange.Text.Trim() & "%'"
        End If
        If cbxCountry.SelectedIndex <> -1 Then
            query &= " AND location = '" & cbxCountry.SelectedItem.ToString() & "'"
        End If
        If cbxVisaType.SelectedIndex <> -1 Then
            query &= " AND visa_type = '" & cbxVisaType.SelectedItem.ToString() & "'"
        End If
        If dateApplicationDeadline.Checked Then
            query &= " AND application_deadline = '" & dateApplicationDeadline.Value.ToString("yyyy-MM-dd") & "'"
        End If

        readQuery(query)
        Dim dt As New DataTable()
        dt.Load(cmdRead)
        cmdRead.Close()
        DGVJobOffers.DataSource = dt
        FormatDGVUniformly(DGVJobOffers)
    End Sub

    Private Sub PopulateFilterComboboxes()
        ' Populate cbxCountry with unique locations
        cbxCountry.Items.Clear()
        Dim agencyId As Integer = Session.CurrentReferenceID
        Dim query As String = $"SELECT DISTINCT location FROM jobplacement WHERE agency_id = {agencyId} AND location IS NOT NULL AND location <> '' ORDER BY location ASC"
        readQuery(query)
        While cmdRead.Read()
            cbxCountry.Items.Add(cmdRead("location").ToString())
        End While
        cmdRead.Close()
        cbxCountry.SelectedIndex = -1

        ' Populate cbxVisaType
        cbxVisaType.Items.Clear()
        cbxVisaType.Items.AddRange(New String() {"Work Visa", "Tourist Visa", "Permanent Residency"})
        cbxVisaType.SelectedIndex = -1
    End Sub

    ' Live filter events
    Private Sub txtbxJobIdNum_TextChanged(sender As Object, e As EventArgs) Handles txtbxJobIdNum.TextChanged
        ApplyJobFilters()
    End Sub
    Private Sub txtbxJobTitle_TextChanged(sender As Object, e As EventArgs) Handles txtbxJobTitle.TextChanged
        ApplyJobFilters()
    End Sub
    Private Sub txtbxJobType_TextChanged(sender As Object, e As EventArgs) Handles txtbxJobType.TextChanged
        ApplyJobFilters()
    End Sub
    Private Sub txtbxReqSkill_TextChanged(sender As Object, e As EventArgs) Handles txtbxReqSkill.TextChanged
        ApplyJobFilters()
    End Sub
    Private Sub txtbxSalaryRange_TextChanged(sender As Object, e As EventArgs) Handles txtbxSalaryRange.TextChanged
        ApplyJobFilters()
    End Sub
    Private Sub cbxCountry_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxCountry.SelectedIndexChanged
        ApplyJobFilters()
    End Sub
    Private Sub cbxVisaType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxVisaType.SelectedIndexChanged
        ApplyJobFilters()
    End Sub
    Private Sub dateApplicationDeadline_ValueChanged(sender As Object, e As EventArgs) Handles dateApplicationDeadline.ValueChanged
        dateApplicationDeadline.CustomFormat = "yyyy-MM-dd"
        ApplyJobFilters()
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

    Private Sub btnDeployments_Click(sender As Object, e As EventArgs) Handles btnDeployments.Click
        Dim depForm As New agcDeployment()
        depForm.Show()
        Me.Hide()
    End Sub

    ' Add Job button handler
    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Dim dlg As New addJob()
        dlg.ShowDialog()
        LoadAgencyJobs()
        FormatDGVUniformly(DGVJobOffers)
    End Sub

    ' Edit Job button handler
    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        If DGVJobOffers.SelectedRows.Count > 0 Then
            Dim selectedRow As DataGridViewRow = DGVJobOffers.SelectedRows(0)
            Dim jobId As Integer = Convert.ToInt32(selectedRow.Cells("job_id").Value)
            Session.CurrentReferenceID = jobId ' Pass job_id to editJob via session
            Dim dlg As New editJob()
            dlg.ShowDialog()
            LoadAgencyJobs()
            FormatDGVUniformly(DGVJobOffers)
        Else
            MessageBox.Show("Please select a job to edit.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    ' Delete Job button handler
    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If DGVJobOffers.SelectedRows.Count > 0 Then
            Dim selectedRow As DataGridViewRow = DGVJobOffers.SelectedRows(0)
            Dim jobId As Integer = Convert.ToInt32(selectedRow.Cells("job_id").Value)
            Dim result As DialogResult = MessageBox.Show("Are you sure you want to delete this job? This action cannot be undone.", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
            If result = DialogResult.Yes Then
                Try
                    Using conn As New MySqlConnection(strConnection)
                        conn.Open()
                        Dim query As String = "DELETE FROM jobplacement WHERE job_id = @jobId"
                        Using cmd As New MySqlCommand(query, conn)
                            cmd.Parameters.AddWithValue("@jobId", jobId)
                            cmd.ExecuteNonQuery()
                        End Using
                    End Using
                    MessageBox.Show("Job deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    LoadAgencyJobs()
                    FormatDGVUniformly(DGVJobOffers)
                Catch ex As Exception
                    MessageBox.Show("Error deleting job: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End If
        Else
            MessageBox.Show("Please select a job to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub DGVJobOffers_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DGVJobOffers.CellContentClick

    End Sub
End Class