Imports MySql.Data.MySqlClient

Public Class joboffers

    Private Sub joboffers_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Session.CurrentLoggedUser.userType <> "OFW" Then
            MsgBox("Access denied. This section is for OFWs only.", MsgBoxStyle.Critical)
            Me.Close()
            Exit Sub
        End If

        LoadMatchingJobOffers()
        FormatDGV()
    End Sub

    Private Sub LoadMatchingJobOffers()
        Dim query As String = "
            SELECT jp.job_id, jp.job_title, jp.salary, jp.location, jp.job_type, 
                   jp.visa_type, jp.application_deadline, s.skill_name, e.employer_name
            FROM jobplacement jp
            JOIN employer e ON jp.employer_id = e.employer_id
            JOIN agencyemployer ae ON ae.employer_id = e.employer_id
            JOIN ofw o ON ae.agency_id = o.agency_id
            JOIN ofwskills os ON o.ofw_id = os.ofw_id
            LEFT JOIN skill s ON jp.skill_id = s.skill_id
            WHERE os.skill_id = jp.skill_id AND o.ofw_id = " & Session.CurrentReferenceID

        ' Append filters from input controls
        If Not String.IsNullOrWhiteSpace(txtbxJobTitle.Text) Then
            query &= $" AND jp.job_title LIKE '%{txtbxJobTitle.Text.Trim()}%'"
        End If

        If Not String.IsNullOrWhiteSpace(txtbxJobType.Text) Then
            query &= $" AND jp.job_type LIKE '%{txtbxJobType.Text.Trim()}%'"
        End If

        If Not String.IsNullOrWhiteSpace(txtbxReqSkill.Text) Then
            query &= $" AND s.skill_name LIKE '%{txtbxReqSkill.Text.Trim()}%'"
        End If

        If Not String.IsNullOrWhiteSpace(txtbxSalaryRange.Text) Then
            query &= $" AND jp.salary LIKE '%{txtbxSalaryRange.Text.Trim()}%'"
        End If

        If cbxCountry.SelectedIndex >= 0 Then
            query &= $" AND jp.location = '{cbxCountry.SelectedItem.ToString()}'"
        End If

        If cbxVisaType.SelectedIndex >= 0 Then
            query &= $" AND jp.visa_type = '{cbxVisaType.SelectedItem.ToString()}'"
        End If

        If dateApplicationDeadline.Checked Then
            query &= $" AND jp.application_deadline = '{dateApplicationDeadline.Value.ToString("yyyy-MM-dd")}'"
        End If

        Try
            LoadToDGV(query, DGVJobOffers)
            FormatDGV()
        Catch ex As Exception
            MsgBox("Failed to load job offers: " & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub FormatDGV()
        With DGVJobOffers
            If .Columns.Contains("job_id") Then .Columns("job_id").Visible = False
            If .Columns.Contains("job_title") Then .Columns("job_title").HeaderText = "Job Title"
            If .Columns.Contains("salary") Then .Columns("salary").HeaderText = "Salary"
            If .Columns.Contains("location") Then .Columns("location").HeaderText = "Location"
            If .Columns.Contains("employer_name") Then .Columns("employer_name").HeaderText = "Employer"
            If .Columns.Contains("job_type") Then .Columns("job_type").HeaderText = "Job Type"
            If .Columns.Contains("visa_type") Then .Columns("visa_type").HeaderText = "Visa Type"
            If .Columns.Contains("application_deadline") Then .Columns("application_deadline").HeaderText = "Deadline"
            If .Columns.Contains("skill_name") Then .Columns("skill_name").HeaderText = "Skill Required"
        End With
    End Sub

    ' Filtering triggers
    Private Sub txtbxJobTitle_TextChanged(sender As Object, e As EventArgs) Handles txtbxJobTitle.TextChanged
        LoadMatchingJobOffers()
    End Sub

    Private Sub txtbxJobType_TextChanged(sender As Object, e As EventArgs) Handles txtbxJobType.TextChanged
        LoadMatchingJobOffers()
    End Sub

    Private Sub txtbxReqSkill_TextChanged(sender As Object, e As EventArgs) Handles txtbxReqSkill.TextChanged
        LoadMatchingJobOffers()
    End Sub

    Private Sub txtbxSalaryRange_TextChanged(sender As Object, e As EventArgs) Handles txtbxSalaryRange.TextChanged
        LoadMatchingJobOffers()
    End Sub

    Private Sub cbxCountry_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxCountry.SelectedIndexChanged
        LoadMatchingJobOffers()
    End Sub

    Private Sub cbxVisaType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxVisaType.SelectedIndexChanged
        LoadMatchingJobOffers()
    End Sub

    Private Sub dateApplicationDeadline_ValueChanged(sender As Object, e As EventArgs) Handles dateApplicationDeadline.ValueChanged
        LoadMatchingJobOffers()
    End Sub

    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        txtbxJobTitle.Clear()
        txtbxJobType.Clear()
        txtbxReqSkill.Clear()
        txtbxSalaryRange.Clear()
        cbxCountry.SelectedIndex = -1
        cbxVisaType.SelectedIndex = -1
        dateApplicationDeadline.Value = Date.Today
        dateApplicationDeadline.Checked = False
        LoadMatchingJobOffers()
    End Sub

    ' Navigation
    Private Sub btnProfile_Click(sender As Object, e As EventArgs) Handles btnProfile.Click
        Dim newForm As New ofwProfile()
        newForm.Show()
        Me.Hide()
    End Sub

    Private Sub btnDeployment_Click(sender As Object, e As EventArgs) Handles btnDeployment.Click
        Dim newForm As New deploymentrecords()
        newForm.Show()
        Me.Hide()
    End Sub

    Private Sub btnApplications_Click(sender As Object, e As EventArgs) Handles btnApplications.Click
        Dim newForm As New applications()
        newForm.Show()
        Me.Hide()
    End Sub

    Private Sub btnViewDetails_Click(sender As Object, e As EventArgs) Handles btnViewDetails.Click
        OpenSelectedJobDetails()
    End Sub

    Private Sub DGVJobOffers_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DGVJobOffers.CellContentClick
        If e.RowIndex >= 0 Then
            DGVJobOffers.Rows(e.RowIndex).Selected = True
            OpenSelectedJobDetails()
        End If
    End Sub

    Private Sub OpenSelectedJobDetails()
        If DGVJobOffers.SelectedRows.Count = 0 Then
            MsgBox("Please select a job offer to view details.", MsgBoxStyle.Information)
            Return
        End If

        Try
            Dim jobID As Integer = Convert.ToInt32(DGVJobOffers.SelectedRows(0).Cells("job_id").Value)
            Dim dlg As New jobdetails(jobID)
            dlg.ShowDialog()
        Catch ex As Exception
            MsgBox("Error opening job details: " & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

End Class
