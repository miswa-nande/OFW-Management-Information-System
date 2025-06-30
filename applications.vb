Imports MySql.Data.MySqlClient

Public Class applications

    Private Sub applications_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Session.CurrentLoggedUser.userType <> "OFW" Then
            MsgBox("Access denied. This section is for OFWs only.", MsgBoxStyle.Critical)
            Me.Close()
            Exit Sub
        End If

        LoadApplications()
    End Sub

    ' Load all applications for logged-in OFW with filters
    Private Sub LoadApplications()
        Dim query As String = $"
            SELECT a.application_id, a.job_id, jp.job_title, jp.location, jp.salary,
                   a.status, a.date_applied
            FROM application a
            JOIN jobplacement jp ON a.job_id = jp.job_id
            WHERE a.ofw_id = {Session.CurrentReferenceID}"

        If Not String.IsNullOrWhiteSpace(txtbxJobTitle.Text) Then
            query &= $" AND jp.job_title LIKE '%{txtbxJobTitle.Text.Replace("'", "''")}%'"
        End If

        If Not String.IsNullOrWhiteSpace(cbxContractStat.Text) AndAlso cbxContractStat.Text <> "All" Then
            query &= $" AND a.status = '{cbxContractStat.Text.Replace("'", "''")}'"
        End If

        If Not String.IsNullOrWhiteSpace(txtbxIdNum.Text) Then
            query &= $" AND a.application_id LIKE '%{txtbxIdNum.Text.Trim()}%'"
        End If

        If Not String.IsNullOrWhiteSpace(txtbxContractNum.Text) Then
            query &= $" AND a.job_id LIKE '%{txtbxContractNum.Text.Trim()}%'"
        End If

        If dateContractStart.Checked Then
            query &= $" AND DATE(a.date_applied) = '{dateContractStart.Value.ToString("yyyy-MM-dd")}'"
        End If

        query &= " ORDER BY a.date_applied DESC"

        Try
            LoadToDGV(query, DataGridView1)
        Catch ex As Exception
            MsgBox("Failed to load applications: " & ex.Message, MsgBoxStyle.Critical)
        End Try
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

    Private Sub btnDeployment_Click(sender As Object, e As EventArgs) Handles btnDeployment.Click
        Dim newForm As New deploymentrecords()
        newForm.Show()
        Me.Hide()
    End Sub

    ' 🔍 Live filter handlers
    Private Sub txtbxJobTitle_TextChanged(sender As Object, e As EventArgs) Handles txtbxJobTitle.TextChanged
        LoadApplications()
    End Sub

    Private Sub cbxContractStat_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxContractStat.SelectedIndexChanged
        LoadApplications()
    End Sub

    Private Sub txtbxIdNum_TextChanged(sender As Object, e As EventArgs) Handles txtbxIdNum.TextChanged
        LoadApplications()
    End Sub

    Private Sub txtbxContractNum_TextChanged(sender As Object, e As EventArgs) Handles txtbxContractNum.TextChanged
        LoadApplications()
    End Sub

    Private Sub dateContractStart_ValueChanged(sender As Object, e As EventArgs) Handles dateContractStart.ValueChanged
        LoadApplications()
    End Sub

    ' 🧼 Clear all filters
    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        txtbxJobTitle.Clear()
        cbxContractStat.SelectedIndex = -1
        txtbxIdNum.Clear()
        txtbxContractNum.Clear()
        dateContractStart.Value = Date.Today
        dateContractStart.Checked = False
        LoadApplications()
    End Sub

    ' 👁️ View selected application
    Private Sub BtnViewApplication_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If DataGridView1.SelectedRows.Count = 0 Then
            MsgBox("Please select an application to view.", MsgBoxStyle.Information)
            Return
        End If

        Dim jobId As Integer = Convert.ToInt32(DataGridView1.SelectedRows(0).Cells("job_id").Value)
        Dim form As New applicationForm(jobId, True)
        form.Text = "View Application"
        form.ShowDialog()
    End Sub

End Class
