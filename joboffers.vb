Imports MySql.Data.MySqlClient

Public Class joboffers

    Private Sub joboffers_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Session.CurrentLoggedUser.userType <> "OFW" Then
            MsgBox("Access denied. This section is for OFWs only.", MsgBoxStyle.Critical)
            Me.Close()
            Exit Sub
        End If

        LoadMatchingJobOffers()
    End Sub

    Private Sub LoadMatchingJobOffers()
        Dim query As String = "
        SELECT jp.JobPlacementID, jp.JobTitle, jp.SalaryRange, jp.CountryOfEmployment, 
               jp.JobType, jp.VisaType, jp.ApplicationDeadline, jp.RequiredSkills, e.CompanyName
        FROM jobplacement jp
        JOIN employer e ON jp.EmployerID = e.EmployerID
        JOIN agencypartneremployer ae ON ae.EmployerID = e.EmployerID
        JOIN ofw o ON ae.AgencyID = o.AgencyID
        JOIN ofwskill os ON o.OFWID = os.OFWID
        JOIN skill s ON os.SkillID = s.SkillID
        WHERE jp.RequiredSkills LIKE CONCAT('%', s.SkillName, '%')
          AND o.OFWID = " & Session.CurrentReferenceID

        ' Append filters
        If Not String.IsNullOrWhiteSpace(txtbxJobTitle.Text) Then
            query &= $" AND jp.JobTitle LIKE '%{txtbxJobTitle.Text.Trim()}%'"
        End If

        If Not String.IsNullOrWhiteSpace(txtbxJobType.Text) Then
            query &= $" AND jp.JobType LIKE '%{txtbxJobType.Text.Trim()}%'"
        End If

        If Not String.IsNullOrWhiteSpace(txtbxReqSkill.Text) Then
            query &= $" AND jp.RequiredSkills LIKE '%{txtbxReqSkill.Text.Trim()}%'"
        End If

        Dim salary As Integer
        If Integer.TryParse(txtbxSalaryRange.Text.Trim(), salary) Then
            query &= $" AND jp.SalaryRange >= {salary}"
        End If

        If cbxCountry.SelectedIndex >= 0 Then
            query &= $" AND jp.CountryOfEmployment = '{cbxCountry.SelectedItem.ToString()}'"
        End If

        If cbxVisaType.SelectedIndex >= 0 Then
            query &= $" AND jp.VisaType = '{cbxVisaType.SelectedItem.ToString()}'"
        End If

        If dateApplicationDeadline.Checked Then
            query &= $" AND jp.ApplicationDeadline = '{dateApplicationDeadline.Value.ToString("yyyy-MM-dd")}'"
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

            ' Set font sizes
            .ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 11, FontStyle.Bold)
            .DefaultCellStyle.Font = New Font("Segoe UI", 10, FontStyle.Regular)

            ' Header styling with new color
            .ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 66, 155)
            .ColumnHeadersDefaultCellStyle.ForeColor = Color.White
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            ' Cell selection styling
            .DefaultCellStyle.SelectionBackColor = Color.FromArgb(100, 150, 200)
            .DefaultCellStyle.SelectionForeColor = Color.Black

            ' Hide internal ID
            If .Columns.Contains("JobPlacementID") Then .Columns("JobPlacementID").Visible = False

            ' Rename headers
            If .Columns.Contains("JobTitle") Then .Columns("JobTitle").HeaderText = "Job Title"
            If .Columns.Contains("SalaryRange") Then .Columns("SalaryRange").HeaderText = "Salary"
            If .Columns.Contains("CountryOfEmployment") Then .Columns("CountryOfEmployment").HeaderText = "Country"
            If .Columns.Contains("JobType") Then .Columns("JobType").HeaderText = "Job Type"
            If .Columns.Contains("VisaType") Then .Columns("VisaType").HeaderText = "Visa Type"
            If .Columns.Contains("ApplicationDeadline") Then .Columns("ApplicationDeadline").HeaderText = "Deadline"
            If .Columns.Contains("RequiredSkills") Then .Columns("RequiredSkills").HeaderText = "Required Skills"
            If .Columns.Contains("CompanyName") Then .Columns("CompanyName").HeaderText = "Employer"
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
            Dim jobID As Integer = Convert.ToInt32(DGVJobOffers.SelectedRows(0).Cells("JobPlacementID").Value)
            Dim dlg As New jobdetails(jobID)
            dlg.ShowDialog()
        Catch ex As Exception
            MsgBox("Error opening job details: " & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

End Class
