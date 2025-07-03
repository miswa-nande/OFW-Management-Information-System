Imports MySql.Data.MySqlClient

Public Class agcApplications
    Private Sub agcApplications_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadAgencyApplications()
        FormatDGVUniformly(DataGridView1)
    End Sub

    Private Sub LoadAgencyApplications()
        Dim agencyId As Integer = Session.CurrentReferenceID
        Dim query As String = $"SELECT * FROM application WHERE AgencyID = {agencyId}"
        readQuery(query)
        Dim dt As New DataTable()
        dt.Load(cmdRead)
        cmdRead.Close()
        DataGridView1.DataSource = dt
    End Sub

    Private Sub ApplyApplicationFilters()
        Dim agencyId As Integer = Session.CurrentReferenceID
        Dim allCleared As Boolean =
            txtbxIdNum.Text.Trim() = "" AndAlso
            txtbxJobTitle.Text.Trim() = "" AndAlso
            txtbxContractNum.Text.Trim() = "" AndAlso
            cbxContractStat.SelectedIndex = -1 AndAlso
            Not dateContractStart.Checked

        If allCleared Then
            LoadAgencyApplications()
            FormatDGVUniformly(DataGridView1)
            Return
        End If

        Dim query As String = $"SELECT * FROM application WHERE AgencyID = {agencyId}"
        If txtbxIdNum.Text.Trim() <> "" Then
            query &= " AND ApplicationID LIKE '%" & txtbxIdNum.Text.Trim() & "%'"
        End If
        If txtbxJobTitle.Text.Trim() <> "" Then
            query &= " AND job_title LIKE '%" & txtbxJobTitle.Text.Trim() & "%'"
        End If
        If txtbxContractNum.Text.Trim() <> "" Then
            query &= " AND contract_number LIKE '%" & txtbxContractNum.Text.Trim() & "%'"
        End If
        If cbxContractStat.SelectedIndex <> -1 Then
            query &= " AND contract_status = '" & cbxContractStat.SelectedItem.ToString() & "'"
        End If
        If dateContractStart.Checked Then
            query &= " AND contract_start >= '" & dateContractStart.Value.ToString("yyyy-MM-dd") & "'"
        End If

        readQuery(query)
        Dim dt As New DataTable()
        dt.Load(cmdRead)
        cmdRead.Close()
        DataGridView1.DataSource = dt
        FormatDGVUniformly(DataGridView1)
    End Sub

    ' Live filter events
    Private Sub txtbxIdNum_TextChanged(sender As Object, e As EventArgs) Handles txtbxIdNum.TextChanged
        ApplyApplicationFilters()
    End Sub
    Private Sub txtbxJobTitle_TextChanged(sender As Object, e As EventArgs) Handles txtbxJobTitle.TextChanged
        ApplyApplicationFilters()
    End Sub
    Private Sub txtbxContractNum_TextChanged(sender As Object, e As EventArgs) Handles txtbxContractNum.TextChanged
        ApplyApplicationFilters()
    End Sub
    Private Sub cbxContractStat_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxContractStat.SelectedIndexChanged
        ApplyApplicationFilters()
    End Sub
    Private Sub dateContractStart_ValueChanged(sender As Object, e As EventArgs) Handles dateContractStart.ValueChanged
        ApplyApplicationFilters()
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
    Private Sub btnDeployments_Click(sender As Object, e As EventArgs) Handles btnDeployments.Click
        Dim depForm As New agcDeployment()
        depForm.Show()
        Me.Hide()
    End Sub
    Private Sub btnJobs_Click(sender As Object, e As EventArgs) Handles btnJobs.Click
        Dim jobsForm As New agcJobs()
        jobsForm.Show()
        Me.Hide()
    End Sub

    Private Sub ViewApplication_Click(sender As Object, e As EventArgs) Handles ViewApplication.Click
        If DataGridView1.SelectedRows.Count > 0 Then
            Dim selectedRow As DataGridViewRow = DataGridView1.SelectedRows(0)
            Dim applicationId As Integer = Convert.ToInt32(selectedRow.Cells("ApplicationID").Value)
            Dim agencyId As Integer = Session.CurrentReferenceID

            ' Validate ownership
            Dim query As String = $"SELECT * FROM application WHERE ApplicationID = {applicationId} AND AgencyID = {agencyId}"
            readQuery(query)

            If cmdRead.HasRows Then
                cmdRead.Close()
                Session.CurrentReferenceID = applicationId ' Pass ApplicationID to detail form
                Dim viewForm As New ApplicationDetails()
                viewForm.ShowDialog()
            Else
                cmdRead.Close()
                MessageBox.Show("Unauthorized access or invalid selection.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
        Else
            MessageBox.Show("Please select an application from the list.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub ClearBTN_Click(sender As Object, e As EventArgs) Handles ClearBTN.Click
        txtbxIdNum.Clear()
        txtbxJobTitle.Clear()
        txtbxContractNum.Clear()
        cbxContractStat.SelectedIndex = -1
        dateContractStart.Checked = False
        dateContractStart.Value = Date.Today

        LoadAgencyApplications()
        FormatDGVUniformly(DataGridView1)
    End Sub
End Class
