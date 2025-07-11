Imports System.Data

Public Class empJobs
    Private Sub empJobs_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        PopulateComboBoxes()
        ClearJobFilters()          ' Clear filters first
        LoadEmployerJobsSummary() ' Then apply filters (which are now empty)
    End Sub

    Private Sub LoadEmployerJobsSummary()
        ApplyJobFilters()
    End Sub

    Private Sub ClearJobFilters()
        txtbxJobIdNum.Clear()
        txtbxJobTitle.Clear()
        txtbxJobType.Clear()
        txtbxReqSkill.Clear()
        txtbxSalaryRange.Clear()
        cbxCountry.SelectedIndex = -1
        cbxVisaType.SelectedIndex = -1
        txtbxAgencyIdNum.Clear()
        dateApplicationDeadline.Value = DateTime.Now
        dateApplicationDeadline.Checked = False
    End Sub

    Private Sub ApplyJobFilters()
        Dim employerId As Integer = Session.CurrentReferenceID
        Dim query As String = "
            SELECT 
                jp.JobPlacementID,
                jp.JobTitle AS 'Job Title',
                jp.CountryOfEmployment AS 'Country',
                a.AgencyName AS 'Agency',
                jp.JobStatus AS 'Status',
                (
                    SELECT COUNT(*) 
                    FROM application app
                    WHERE app.JobPlacementID = jp.JobPlacementID
                ) AS 'No. of Applicants'
            FROM jobplacement jp
            JOIN agency a ON jp.AgencyID = a.AgencyID
            WHERE jp.EmployerID = " & employerId

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
        If cbxCountry.Text.Trim() <> "" Then
            query &= " AND jp.CountryOfEmployment LIKE '%" & cbxCountry.Text.Trim().Replace("'", "''") & "%'"
        End If
        If cbxVisaType.Text.Trim() <> "" Then
            query &= " AND jp.VisaType LIKE '%" & cbxVisaType.Text.Trim().Replace("'", "''") & "%'"
        End If
        If txtbxAgencyIdNum.Text.Trim() <> "" Then
            query &= " AND a.AgencyName LIKE '%" & txtbxAgencyIdNum.Text.Trim().Replace("'", "''") & "%'"
        End If
        If dateApplicationDeadline.Checked Then
            query &= " AND jp.ApplicationDeadline = '" & dateApplicationDeadline.Value.ToString("yyyy-MM-dd") & "'"
        End If

        readQuery(query)
        Dim dt As New DataTable()
        dt.Load(cmdRead)
        cmdRead.Close()

        DGVJobOffers.DataSource = dt
        FormatDGVUniformly(DGVJobOffers)

        If DGVJobOffers.Columns.Contains("JobPlacementID") Then
            DGVJobOffers.Columns("JobPlacementID").Visible = False
        End If
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
        End With
    End Sub

    ' === Live Filter Events ===
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

    Private Sub cbxCountry_TextChanged(sender As Object, e As EventArgs) Handles cbxCountry.TextChanged
        ApplyJobFilters()
    End Sub

    Private Sub cbxVisaType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxVisaType.SelectedIndexChanged
        ApplyJobFilters()
    End Sub

    Private Sub txtbxAgencyIdNum_TextChanged(sender As Object, e As EventArgs) Handles txtbxAgencyIdNum.TextChanged
        ApplyJobFilters()
    End Sub

    Private Sub dateApplicationDeadline_ValueChanged(sender As Object, e As EventArgs) Handles dateApplicationDeadline.ValueChanged
        ApplyJobFilters()
    End Sub

    ' === Action Buttons ===
    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        ClearJobFilters()
        ApplyJobFilters()
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Dim dlg As New addJob()
        dlg.ShowDialog()
        ApplyJobFilters()
    End Sub

    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        If DGVJobOffers.SelectedRows.Count = 0 Then
            MsgBox("Please select a job to edit.", MsgBoxStyle.Information)
            Return
        End If
        Dim jobId As Integer = Convert.ToInt32(DGVJobOffers.SelectedRows(0).Cells("JobPlacementID").Value)
        Dim dlg As New editJob(jobId)
        dlg.ShowDialog()
        ApplyJobFilters()
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If DGVJobOffers.SelectedRows.Count = 0 Then
            MsgBox("Please select a job to delete.", MsgBoxStyle.Information)
            Return
        End If
        Dim jobId As Integer = Convert.ToInt32(DGVJobOffers.SelectedRows(0).Cells("JobPlacementID").Value)
        Dim result = MessageBox.Show("Are you sure you want to delete this job?", "Confirm Deletion", MessageBoxButtons.YesNo)
        If result = DialogResult.Yes Then
            Dim deleteQuery = $"DELETE FROM jobplacement WHERE JobPlacementID = {jobId}"
            readQuery(deleteQuery)
            MsgBox("Job deleted.")
            ApplyJobFilters()
        End If
    End Sub

    Private Sub btnViewDetails_Click(sender As Object, e As EventArgs) Handles btnViewDetails.Click
        If DGVJobOffers.SelectedRows.Count = 0 Then
            MsgBox("Please select a job to view.", MsgBoxStyle.Information)
            Return
        End If
        Dim jobId As Integer = Convert.ToInt32(DGVJobOffers.SelectedRows(0).Cells("JobPlacementID").Value)
        Dim form As New jobdetails(jobId)
        form.ShowDialog()
    End Sub

    ' === Navigation Buttons ===
    Private Sub btnDashboard_Click(sender As Object, e As EventArgs) Handles btnDashboard.Click
        Dim form As New empDashboard()
        form.Show()
        Me.Hide()
    End Sub

    Private Sub btnAgencies_Click(sender As Object, e As EventArgs) Handles btnAgencies.Click
        Dim form As New empAgencies()
        form.Show()
        Me.Hide()
    End Sub

    Private Sub btnOfws_Click(sender As Object, e As EventArgs) Handles btnOfws.Click
        Dim form As New empOfws()
        form.Show()
        Me.Hide()
    End Sub

    Private Sub PopulateComboBoxes()
        ' === Populate Country ComboBox ===
        Try
            Dim countryQuery As String = "SELECT DISTINCT CountryOfEmployment FROM jobplacement WHERE CountryOfEmployment IS NOT NULL ORDER BY CountryOfEmployment"
            readQuery(countryQuery)
            cbxCountry.Items.Clear()
            While cmdRead.Read()
                cbxCountry.Items.Add(cmdRead("CountryOfEmployment").ToString())
            End While
            cmdRead.Close()
        Catch ex As Exception
            MsgBox("Error loading countries: " & ex.Message, MsgBoxStyle.Critical)
        End Try

        ' === Populate Visa Type ComboBox ===
        Try
            Dim visaQuery As String = "SELECT DISTINCT VisaType FROM jobplacement WHERE VisaType IS NOT NULL ORDER BY VisaType"
            readQuery(visaQuery)
            cbxVisaType.Items.Clear()
            While cmdRead.Read()
                cbxVisaType.Items.Add(cmdRead("VisaType").ToString())
            End While
            cmdRead.Close()
        Catch ex As Exception
            MsgBox("Error loading visa types: " & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub btnGenerate_Click(sender As Object, e As EventArgs) Handles btnGenerate.Click
        Try
            Dim reportGenerator As New GenerateEmployerJobsReportData()
            If reportGenerator.GenerateReport() Then
                MessageBox.Show("Job placements report generated successfully! Check your Desktop for the PDF file.", "Report Generated", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                MessageBox.Show("Failed to generate job placements report.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Catch ex As Exception
            MessageBox.Show("Error generating report: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
End Class
