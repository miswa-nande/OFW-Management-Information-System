Public Class joboffers

    Private Sub joboffers_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadMatchingJobOffers()
        FormatDGV()
    End Sub

    ' Load jobs matching the OFW's skills and agency's partnered employers
    Private Sub LoadMatchingJobOffers()
        Dim query As String = "
            SELECT jp.job_id, jp.job_title, jp.salary, jp.location, e.employer_name
            FROM jobplacement jp
            JOIN employer e ON jp.employer_id = e.employer_id
            JOIN agencyemployer ae ON ae.employer_id = e.employer_id
            JOIN ofw o ON ae.agency_id = o.agency_id
            JOIN ofwskills os ON o.ofw_id = os.ofw_id
            WHERE os.skill_id = jp.skill_id AND o.ofw_id = " & LoggedInOfwID

        LoadToDGV(query, DGVJobOffers)
    End Sub

    ' Format DGV column headers and hide internal IDs
    Private Sub FormatDGV()
        With DGVJobOffers
            If .Columns.Contains("job_id") Then .Columns("job_id").Visible = False
            If .Columns.Contains("job_title") Then .Columns("job_title").HeaderText = "Job Title"
            If .Columns.Contains("salary") Then .Columns("salary").HeaderText = "Salary"
            If .Columns.Contains("location") Then .Columns("location").HeaderText = "Location"
            If .Columns.Contains("employer_name") Then .Columns("employer_name").HeaderText = "Employer"
        End With
    End Sub

    ' Navigation buttons
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
        If DGVJobOffers.SelectedRows.Count = 0 Then
            MsgBox("Please select a job offer to view details.", MsgBoxStyle.Information)
            Return
        End If

        ' You can pass selected job_id if needed
        Dim jobID As Integer = Convert.ToInt32(DGVJobOffers.SelectedRows(0).Cells("job_id").Value)
        Dim dlg As New jobdetails(jobID)
        dlg.ShowDialog()
    End Sub

    Private Sub DGVJobOffers_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DGVJobOffers.CellContentClick
        ' Optional: Handle click events on specific columns if needed
    End Sub

End Class
