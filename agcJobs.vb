Imports MySql.Data.MySqlClient

Public Class agcJobs
    Private Sub agcJobs_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadAgencyJobs()
        PopulateFilterComboboxes()
    End Sub

    Private Sub LoadAgencyJobs()
        Dim agencyId As Integer = Session.CurrentReferenceID
        Dim query As String = "
            SELECT jp.JobPlacementID, jp.JobTitle, e.CompanyName AS Employer,
                   jp.CountryOfEmployment AS Country, jp.JobStatus AS Status,
                   jp.NumOfVacancies AS Vacancies, jp.ApplicationDeadline
            FROM jobplacement jp
            JOIN employer e ON jp.EmployerID = e.EmployerID
            JOIN agencypartneremployer ape ON ape.EmployerID = e.EmployerID
            WHERE ape.AgencyID = " & agencyId

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
            cbxVisaType.SelectedIndex = -1

        If allCleared Then
            LoadAgencyJobs()
            Return
        End If

        Dim query As String = "
            SELECT jp.JobPlacementID, jp.JobTitle, e.CompanyName AS Employer,
                   jp.CountryOfEmployment AS Country, jp.JobStatus AS Status,
                   jp.NumOfVacancies AS Vacancies, jp.ApplicationDeadline
            FROM jobplacement jp
            JOIN employer e ON jp.EmployerID = e.EmployerID
            JOIN agencypartneremployer ape ON ape.EmployerID = e.EmployerID
            WHERE ape.AgencyID = " & agencyId

        If txtbxJobIdNum.Text.Trim() <> "" Then
            query &= " AND jp.JobPlacementID LIKE '%" & txtbxJobIdNum.Text.Trim().Replace("'", "''") & "%'"
        End If
        If txtbxJobTitle.Text.Trim() <> "" Then
            query &= " AND jp.JobTitle LIKE '%" & txtbxJobTitle.Text.Trim().Replace("'", "''") & "%'"
        End If
        If txtbxJobType.Text.Trim() <> "" Then
            query &= " AND jp.JobType LIKE '%" & txtbxJobType.Text.Trim().Replace("'", "''") & "%'"
        End If
        If txtbxReqSkill.Text.Trim() <> "" Then
            query &= " AND jp.RequiredSkills LIKE '%" & txtbxReqSkill.Text.Trim().Replace("'", "''") & "%'"
        End If
        If txtbxSalaryRange.Text.Trim() <> "" Then
            query &= " AND jp.SalaryRange LIKE '%" & txtbxSalaryRange.Text.Trim().Replace("'", "''") & "%'"
        End If
        If cbxCountry.SelectedIndex <> -1 Then
            query &= " AND jp.CountryOfEmployment = '" & cbxCountry.SelectedItem.ToString().Replace("'", "''") & "'"
        End If
        If cbxVisaType.SelectedIndex <> -1 Then
            query &= " AND jp.VisaType = '" & cbxVisaType.SelectedItem.ToString().Replace("'", "''") & "'"
        End If

        readQuery(query)
        Dim dt As New DataTable()
        dt.Load(cmdRead)
        cmdRead.Close()
        DGVJobOffers.DataSource = dt
        FormatDGVUniformly(DGVJobOffers)
    End Sub

    Private Sub FormatDGVUniformly(dgv As DataGridView)
        With dgv
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .MultiSelect = False
            .ReadOnly = True
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
            .AllowUserToResizeRows = False
            .RowHeadersVisible = False
            .BorderStyle = BorderStyle.None
            .EnableHeadersVisualStyles = False
            .BackgroundColor = Color.White

            .ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 66, 155)
            .ColumnHeadersDefaultCellStyle.ForeColor = Color.White
            .ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 11, FontStyle.Bold)
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            .DefaultCellStyle.Font = New Font("Segoe UI", 10)
            .DefaultCellStyle.SelectionBackColor = Color.FromArgb(100, 150, 200)
            .DefaultCellStyle.SelectionForeColor = Color.Black

            .AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 248, 255)
        End With

        If dgv.Columns.Contains("JobTitle") Then dgv.Columns("JobTitle").HeaderText = "Job Title"
        If dgv.Columns.Contains("Employer") Then dgv.Columns("Employer").HeaderText = "Employer"
        If dgv.Columns.Contains("Country") Then dgv.Columns("Country").HeaderText = "Country"
        If dgv.Columns.Contains("Status") Then dgv.Columns("Status").HeaderText = "Status"
        If dgv.Columns.Contains("Vacancies") Then dgv.Columns("Vacancies").HeaderText = "Vacancies"
        If dgv.Columns.Contains("ApplicationDeadline") Then dgv.Columns("ApplicationDeadline").HeaderText = "Deadline"
    End Sub

    Private Sub PopulateFilterComboboxes()
        cbxCountry.Items.Clear()
        Dim agencyId As Integer = Session.CurrentReferenceID
        Dim query As String = $"SELECT DISTINCT CountryOfEmployment FROM jobplacement jp JOIN employer e ON jp.EmployerID = e.EmployerID JOIN agencypartneremployer ape ON ape.EmployerID = e.EmployerID WHERE ape.AgencyID = {agencyId} AND CountryOfEmployment IS NOT NULL ORDER BY CountryOfEmployment"
        readQuery(query)
        While cmdRead.Read()
            cbxCountry.Items.Add(cmdRead("CountryOfEmployment").ToString())
        End While
        cmdRead.Close()
        cbxCountry.SelectedIndex = -1

        cbxVisaType.Items.Clear()
        cbxVisaType.Items.AddRange(New String() {"Tourist", "Work", "Student"})
        cbxVisaType.SelectedIndex = -1
    End Sub

    ' Filter Events
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

    ' Navigation
    Private Sub btnDashboard_Click(sender As Object, e As EventArgs) Handles btnDashboard.Click
        agcDashboard.Show()
        Me.Hide()
    End Sub
    Private Sub btnOfws_Click(sender As Object, e As EventArgs) Handles btnOfws.Click
        agcOfws.Show()
        Me.Hide()
    End Sub
    Private Sub btnEmployers_Click(sender As Object, e As EventArgs) Handles btnEmployers.Click
        agcEmployers.Show()
        Me.Hide()
    End Sub
    Private Sub btnApplications_Click(sender As Object, e As EventArgs) Handles btnApplications.Click
        Dim appForm As New agcApplications() ' Uses default optional parameter
        appForm.Show()
        Me.Hide()
    End Sub

    Private Sub btnDeployments_Click(sender As Object, e As EventArgs) Handles btnDeployments.Click
        agcDeployment.Show()
        Me.Hide()
    End Sub

    ' Add/Edit/Close
    Private Sub btnAdd_Click(sender As Object, e As EventArgs)
        Dim dlg As New addJob()
        dlg.ShowDialog()
        LoadAgencyJobs()
    End Sub

    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        If DGVJobOffers.SelectedRows.Count > 0 Then
            Dim jobId As Integer = CInt(DGVJobOffers.SelectedRows(0).Cells("JobPlacementID").Value)
            Dim dlg As New editJob(jobId)
            dlg.ShowDialog()
            LoadAgencyJobs()
        Else
            MessageBox.Show("Please select a job to edit.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If DGVJobOffers.SelectedRows.Count > 0 Then
            Dim jobId As Integer = CInt(DGVJobOffers.SelectedRows(0).Cells("JobPlacementID").Value)
            Dim status As String = DGVJobOffers.SelectedRows(0).Cells("Status").Value.ToString()

            If status = "Closed" Then
                MessageBox.Show("This job is already closed.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            If MessageBox.Show("Are you sure you want to close this job?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                Dim query As String = "UPDATE jobplacement SET JobStatus = 'Closed' WHERE JobPlacementID = @jobId"
                Using conn As New MySqlConnection(strConnection)
                    conn.Open()
                    Using cmd As New MySqlCommand(query, conn)
                        cmd.Parameters.AddWithValue("@jobId", jobId)
                        cmd.ExecuteNonQuery()
                    End Using
                End Using
                MessageBox.Show("Job successfully closed.", "Closed", MessageBoxButtons.OK, MessageBoxIcon.Information)
                LoadAgencyJobs()
            End If
        Else
            MessageBox.Show("Please select a job to close.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning)
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
        LoadAgencyJobs()
    End Sub

    Private Sub btnJobDetatils_Click(sender As Object, e As EventArgs) Handles btnJobDetatils.Click
        If DGVJobOffers.SelectedRows.Count > 0 Then
            Dim jobId As Integer = CInt(DGVJobOffers.SelectedRows(0).Cells("JobPlacementID").Value)
            Dim detailsForm As New jobdetails(jobId)
            detailsForm.ShowDialog()
        Else
            MessageBox.Show("Please select a job to view details.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub


    Private Sub btnViewApplicants_Click(sender As Object, e As EventArgs) Handles btnViewApplicants.Click
        If DGVJobOffers.SelectedRows.Count > 0 Then
            Dim jobId As Integer = CInt(DGVJobOffers.SelectedRows(0).Cells("JobPlacementID").Value)
            Dim applicantsForm As New agcApplications(jobId) 'Use overloaded constructor
            applicantsForm.Show()
        Else
            MessageBox.Show("Please select a job to view applicants.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub btnGenerate_Click(sender As Object, e As EventArgs) Handles btnGenerate.Click

    End Sub
End Class
