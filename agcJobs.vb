Imports MySql.Data.MySqlClient

Public Class agcJobs
    Private Sub agcJobs_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadAgencyJobs()
        FormatDGVUniformly(DGVJobOffers)
        PopulateFilterComboboxes()
    End Sub

    Private Sub LoadAgencyJobs()
        Dim agencyId As Integer = Session.CurrentReferenceID
        Dim query As String = "
            SELECT jp.job_id, jp.job_title, e.employer_name AS employer, 
                   jp.location AS country, jp.status, jp.vacancies, jp.application_deadline
            FROM jobplacement jp
            JOIN employer e ON jp.employer_id = e.employer_id
            WHERE jp.agency_id = " & agencyId

        readQuery(query)
        Dim dt As New DataTable()
        dt.Load(cmdRead)
        cmdRead.Close()
        DGVJobOffers.DataSource = dt
        FormatDGVUniformly(DGVJobOffers)
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
            Return
        End If

        Dim query As String = "
            SELECT jp.job_id, jp.job_title, e.employer_name AS employer, 
                   jp.location AS country, jp.status, jp.vacancies, jp.application_deadline
            FROM jobplacement jp
            JOIN employer e ON jp.employer_id = e.employer_id
            WHERE jp.agency_id = " & agencyId

        If txtbxJobIdNum.Text.Trim() <> "" Then
            query &= " AND jp.job_id LIKE '%" & txtbxJobIdNum.Text.Trim() & "%'"
        End If
        If txtbxJobTitle.Text.Trim() <> "" Then
            query &= " AND jp.job_title LIKE '%" & txtbxJobTitle.Text.Trim() & "%'"
        End If
        If txtbxJobType.Text.Trim() <> "" Then
            query &= " AND jp.job_type LIKE '%" & txtbxJobType.Text.Trim() & "%'"
        End If
        If txtbxReqSkill.Text.Trim() <> "" Then
            query &= " AND jp.skill_id IN (SELECT skill_id FROM skill WHERE skill_name LIKE '%" & txtbxReqSkill.Text.Trim() & "%')"
        End If
        If txtbxSalaryRange.Text.Trim() <> "" Then
            query &= " AND jp.salary LIKE '%" & txtbxSalaryRange.Text.Trim() & "%'"
        End If
        If cbxCountry.SelectedIndex <> -1 Then
            query &= " AND jp.location = '" & cbxCountry.SelectedItem.ToString() & "'"
        End If
        If cbxVisaType.SelectedIndex <> -1 Then
            query &= " AND jp.visa_type = '" & cbxVisaType.SelectedItem.ToString() & "'"
        End If
        If dateApplicationDeadline.Checked Then
            query &= " AND jp.application_deadline = '" & dateApplicationDeadline.Value.ToString("yyyy-MM-dd") & "'"
        End If

        readQuery(query)
        Dim dt As New DataTable()
        dt.Load(cmdRead)
        cmdRead.Close()
        DGVJobOffers.DataSource = dt
        FormatDGVUniformly(DGVJobOffers)
    End Sub

    Private Sub PopulateFilterComboboxes()
        cbxCountry.Items.Clear()
        Dim agencyId As Integer = Session.CurrentReferenceID
        Dim query As String = $"SELECT DISTINCT location FROM jobplacement WHERE agency_id = {agencyId} AND location IS NOT NULL AND location <> '' ORDER BY location ASC"
        readQuery(query)
        While cmdRead.Read()
            cbxCountry.Items.Add(cmdRead("location").ToString())
        End While
        cmdRead.Close()
        cbxCountry.SelectedIndex = -1

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

    ' Add and Edit buttons
    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Dim dlg As New addJob()
        dlg.ShowDialog()
        LoadAgencyJobs()
    End Sub

    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        If DGVJobOffers.SelectedRows.Count > 0 Then
            Dim selectedRow As DataGridViewRow = DGVJobOffers.SelectedRows(0)
            Dim jobId As Integer = Convert.ToInt32(selectedRow.Cells("job_id").Value)
            Session.CurrentReferenceID = jobId
            Dim dlg As New editJob()
            dlg.ShowDialog()
            LoadAgencyJobs()
        Else
            MessageBox.Show("Please select a job to edit.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    ' Close Job button (replaces delete)
    Private Sub btnCloseJob_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If DGVJobOffers.SelectedRows.Count > 0 Then
            Dim selectedRow As DataGridViewRow = DGVJobOffers.SelectedRows(0)
            Dim jobId As Integer = Convert.ToInt32(selectedRow.Cells("job_id").Value)
            Dim status As String = selectedRow.Cells("status").Value.ToString()

            If status = "Closed" Then
                MessageBox.Show("This job is already closed.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            Dim result As DialogResult = MessageBox.Show("Are you sure you want to close this job?", "Confirm Close", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If result = DialogResult.Yes Then
                Try
                    Using conn As New MySqlConnection(strConnection)
                        conn.Open()
                        Dim query As String = "UPDATE jobplacement SET status = 'Closed' WHERE job_id = @jobId"
                        Using cmd As New MySqlCommand(query, conn)
                            cmd.Parameters.AddWithValue("@jobId", jobId)
                            cmd.ExecuteNonQuery()
                        End Using
                    End Using
                    MessageBox.Show("Job successfully closed.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    LoadAgencyJobs()
                Catch ex As Exception
                    MessageBox.Show("Error closing job: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End If
        Else
            MessageBox.Show("Please select a job to close.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        txtbxJobIdNum.Clear()
        txtbxJobTitle.Clear()
        txtbxJobType.Clear()
        txtbxReqSkill.Clear()
        txtbxSalaryRange.Clear()
        cbxCountry.SelectedIndex = -1
        cbxVisaType.SelectedIndex = -1
        dateApplicationDeadline.Value = Date.Today
        dateApplicationDeadline.Checked = False
        LoadAgencyJobs()
    End Sub

    Private Sub btnGenerate_Click(sender As Object, e As EventArgs) Handles btnGenerate.Click
        ' Future report generation logic here
    End Sub

    Private Sub btnViewApplicants_Click(sender As Object, e As EventArgs) Handles btnViewApplicants.Click
        ' Open applicants form here
    End Sub

    Private Sub btnJobDetatils_Click(sender As Object, e As EventArgs) Handles btnJobDetatils.Click
        ' Open job details window
    End Sub

    Private Sub FormatDGVUniformly(dgv As DataGridView)
        With dgv
            .Columns("job_title").HeaderText = "Job Title"
            .Columns("employer").HeaderText = "Employer"
            .Columns("country").HeaderText = "Country"
            .Columns("status").HeaderText = "Status"
            .Columns("vacancies").HeaderText = "Vacancies"
            .Columns("application_deadline").HeaderText = "Deadline"
        End With
    End Sub
End Class
