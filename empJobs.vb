Imports System.Data

Public Class empJobs
    Private Sub empJobs_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadEmployerJobsSummary()
    End Sub

    Private Sub LoadEmployerJobsSummary()
        ApplyJobFilters()
    End Sub

    Private Sub ApplyJobFilters()
        Dim employerId As Integer = Session.CurrentReferenceID
        Dim query As String = """
            SELECT 
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
        ' Add filters if controls exist
        If txtbxJobTitle IsNot Nothing AndAlso txtbxJobTitle.Text.Trim() <> "" Then
            query &= " AND jp.JobTitle LIKE '%" & txtbxJobTitle.Text.Trim().Replace("'", "''") & "%'"
        End If
        If cbxCountry IsNot Nothing AndAlso cbxCountry.Text.Trim() <> "" Then
            query &= " AND jp.CountryOfEmployment LIKE '%" & cbxCountry.Text.Trim().Replace("'", "''") & "%'"
        End If
        If txtbxAgencyIdNum IsNot Nothing AndAlso txtbxAgencyIdNum.Text.Trim() <> "" Then
            query &= " AND a.AgencyName LIKE '%" & txtbxAgencyIdNum.Text.Trim().Replace("'", "''") & "%'"
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
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
            .DefaultCellStyle.Font = New Font("Segoe UI", 12)
            .ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 12, FontStyle.Bold)
            .RowTemplate.Height = 30
            .DefaultCellStyle.WrapMode = DataGridViewTriState.False
            .AllowUserToResizeRows = False
            .AllowUserToResizeColumns = True
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .MultiSelect = False
            .ReadOnly = True
        End With
    End Sub

    ' Event handlers for filter controls
    Private Sub txtbxJobTitle_TextChanged(sender As Object, e As EventArgs) Handles txtbxJobTitle.TextChanged
        ApplyJobFilters()
    End Sub
    Private Sub txtbxCountry_TextChanged(sender As Object, e As EventArgs) Handles cbxCountry.TextChanged
        ApplyJobFilters()
    End Sub
    Private Sub txtbxAgency_TextChanged(sender As Object, e As EventArgs) Handles txtbxAgencyIdNum.TextChanged
        ApplyJobFilters()
    End Sub

    ' Navigation button handlers
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

    ' Add Job
    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Dim dlg As New addJob()
        dlg.ShowDialog()
        ApplyJobFilters() ' Refresh after adding
    End Sub

    ' Edit Job
    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        If DGVJobOffers.SelectedRows.Count = 0 Then
            MsgBox("Please select a job to edit.", MsgBoxStyle.Information)
            Return
        End If
        ' Assuming the JobPlacementID is available but hidden in DGVJobOffers
        Dim jobTitle As String = DGVJobOffers.SelectedRows(0).Cells("Job Title").Value.ToString()
        Dim query As String = "SELECT JobPlacementID FROM jobplacement WHERE JobTitle = '" & jobTitle.Replace("'", "''") & "' AND EmployerID = " & Session.CurrentReferenceID
        readQuery(query)
        Dim jobId As Integer = -1
        If cmdRead.Read() Then
            jobId = Convert.ToInt32(cmdRead("JobPlacementID"))
        End If
        cmdRead.Close()
        If jobId = -1 Then
            MsgBox("Could not find JobPlacementID for the selected job.", MsgBoxStyle.Critical)
            Return
        End If
        Dim dlg As New editJob(jobId)
        dlg.ShowDialog()
        ApplyJobFilters() ' Refresh after editing
    End Sub
End Class