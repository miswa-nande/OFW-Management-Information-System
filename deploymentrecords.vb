Public Class deploymentrecords

    Private Sub deploymentrecords_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Restrict to OFW users only
        If Session.CurrentLoggedUser.userType <> "OFW" Then
            MsgBox("Access denied. This section is for OFWs only.", MsgBoxStyle.Critical)
            Me.Close()
            Exit Sub
        End If

        LoadDeploymentRecords()
        FormatDGV()
    End Sub

    ' Load all deployment records for the currently logged-in OFW
    Private Sub LoadDeploymentRecords()
        Try
            Dim query As String = $"
                SELECT d.deployment_id, d.contract_number, jp.job_title, d.country_of_deployment,
                       d.salary, d.contract_status, d.contract_start, d.contract_end,
                       d.repatriation_status, d.reason_for_return
                FROM deploymentrecord d
                JOIN jobplacement jp ON d.job_id = jp.job_id
                WHERE d.ofw_id = {Session.CurrentReferenceID}"

            LoadToDGV(query, DataGridView1)

        Catch ex As Exception
            MsgBox("Error loading deployment records: " & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub FormatDGV()
        With DataGridView1
            If .Columns.Contains("deployment_id") Then .Columns("deployment_id").Visible = False
            If .Columns.Contains("contract_number") Then .Columns("contract_number").HeaderText = "Contract #"
            If .Columns.Contains("job_title") Then .Columns("job_title").HeaderText = "Job Title"
            If .Columns.Contains("country_of_deployment") Then .Columns("country_of_deployment").HeaderText = "Country"
            If .Columns.Contains("salary") Then .Columns("salary").HeaderText = "Salary"
            If .Columns.Contains("contract_status") Then .Columns("contract_status").HeaderText = "Contract Status"
            If .Columns.Contains("contract_start") Then .Columns("contract_start").HeaderText = "Start Date"
            If .Columns.Contains("contract_end") Then .Columns("contract_end").HeaderText = "End Date"
            If .Columns.Contains("repatriation_status") Then .Columns("repatriation_status").HeaderText = "Repatriated?"
            If .Columns.Contains("reason_for_return") Then .Columns("reason_for_return").HeaderText = "Reason for Return"
        End With
    End Sub

    ' Navigation buttons
    Private Sub btnProfile_Click(sender As Object, e As EventArgs) Handles btnProfile.Click
        Dim newForm As New ofwProfile()
        newForm.Show()
        Me.Hide()
    End Sub

    Private Sub btnJobOffers_Click(sender As Object, e As EventArgs) Handles btnJobOffers.Click
        Dim newForm As New joboffers()
        newForm.Show()
        Me.Hide()
    End Sub

    Private Sub btnApplications_Click(sender As Object, e As EventArgs) Handles btnApplications.Click
        Dim newForm As New applications()
        newForm.Show()
        Me.Hide()
    End Sub

    ' Optional: If you want to allow clearing or refreshing
    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        LoadDeploymentRecords()
    End Sub

End Class
