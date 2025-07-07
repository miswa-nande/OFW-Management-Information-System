Imports MySql.Data.MySqlClient

Public Class agcApplications
    Private Sub agcApplications_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Initialize the DateTimePicker to be cleared
        dateDateSubmitted.Format = DateTimePickerFormat.Custom
        dateDateSubmitted.CustomFormat = " "
        dateDateSubmitted.ShowCheckBox = True

        LoadAgencyApplications()
        FormatDGVUniformly(DataGridView1)
    End Sub

    Private Sub LoadAgencyApplications()
        Dim agencyId As Integer = Session.CurrentReferenceID
        Dim query As String = $"
            SELECT a.ApplicationID, ag.AgencyName, a.ApplicationStatus, a.ApplicationDate, a.LastUpdate, a.Remarks
            FROM application a
            JOIN agency ag ON a.AgencyID = ag.AgencyID
            WHERE a.AgencyID = {agencyId}
        "
        readQuery(query)
        Dim dt As New DataTable()
        dt.Load(cmdRead)
        cmdRead.Close()
        DataGridView1.DataSource = dt
    End Sub

    Private Sub ApplyApplicationFilters()
        Dim agencyId As Integer = Session.CurrentReferenceID
        Dim allCleared As Boolean =
            txtbxJobIdNum.Text.Trim() = "" AndAlso
            txtbxAgencyName.Text.Trim() = "" AndAlso
            cbxApplicationStatus.SelectedIndex = -1 AndAlso
            Not dateDateSubmitted.Checked

        If allCleared Then
            LoadAgencyApplications()
            Return
        End If

        Dim query As String = $"
            SELECT a.ApplicationID, ag.AgencyName, a.ApplicationStatus, a.ApplicationDate, a.LastUpdate, a.Remarks
            FROM application a
            JOIN agency ag ON a.AgencyID = ag.AgencyID
            WHERE a.AgencyID = {agencyId}
        "

        If txtbxJobIdNum.Text.Trim() <> "" Then
            query &= " AND a.ApplicationID LIKE '%" & txtbxJobIdNum.Text.Trim().Replace("'", "''") & "%'"
        End If
        If txtbxAgencyName.Text.Trim() <> "" Then
            query &= " AND ag.AgencyName LIKE '%" & txtbxAgencyName.Text.Trim().Replace("'", "''") & "%'"
        End If
        If cbxApplicationStatus.SelectedIndex <> -1 Then
            query &= " AND a.ApplicationStatus = '" & cbxApplicationStatus.SelectedItem.ToString() & "'"
        End If
        If dateDateSubmitted.Checked Then
            query &= " AND DATE(a.ApplicationDate) = '" & dateDateSubmitted.Value.ToString("yyyy-MM-dd") & "'"
        End If

        readQuery(query)
        Dim dt As New DataTable()
        dt.Load(cmdRead)
        cmdRead.Close()
        DataGridView1.DataSource = dt
        FormatDGVUniformly(DataGridView1)
    End Sub

    ' Live Filter Events
    Private Sub txtbxJobIdNum_TextChanged(sender As Object, e As EventArgs) Handles txtbxJobIdNum.TextChanged
        ApplyApplicationFilters()
    End Sub

    Private Sub txtbxAgencyName_TextChanged(sender As Object, e As EventArgs) Handles txtbxAgencyName.TextChanged
        ApplyApplicationFilters()
    End Sub

    Private Sub cbxApplicationStatus_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxApplicationStatus.SelectedIndexChanged
        ApplyApplicationFilters()
    End Sub

    Private Sub dateDateSubmitted_ValueChanged(sender As Object, e As EventArgs) Handles dateDateSubmitted.ValueChanged
        If dateDateSubmitted.Checked Then
            dateDateSubmitted.CustomFormat = "yyyy-MM-dd"
            ApplyApplicationFilters()
        Else
            dateDateSubmitted.CustomFormat = " " ' Visually clear
            ApplyApplicationFilters()
        End If
    End Sub

    Private Sub ClearBTN_Click(sender As Object, e As EventArgs) Handles ClearBTN.Click
        txtbxJobIdNum.Clear()
        txtbxAgencyName.Clear()
        cbxApplicationStatus.SelectedIndex = -1

        ' Reset and clear the date picker
        dateDateSubmitted.Checked = False
        dateDateSubmitted.Value = Date.Today
        dateDateSubmitted.CustomFormat = " "

        LoadAgencyApplications()
    End Sub

    ' DGV Styling
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

        If dgv.Columns.Contains("ApplicationID") Then dgv.Columns("ApplicationID").HeaderText = "Application ID"
        If dgv.Columns.Contains("AgencyName") Then dgv.Columns("AgencyName").HeaderText = "Agency"
        If dgv.Columns.Contains("ApplicationStatus") Then dgv.Columns("ApplicationStatus").HeaderText = "Status"
        If dgv.Columns.Contains("ApplicationDate") Then dgv.Columns("ApplicationDate").HeaderText = "Date Submitted"
        If dgv.Columns.Contains("LastUpdate") Then dgv.Columns("LastUpdate").HeaderText = "Last Update"
        If dgv.Columns.Contains("Remarks") Then dgv.Columns("Remarks").HeaderText = "Remarks"
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
    Private Sub btnDeployments_Click(sender As Object, e As EventArgs) Handles btnDeployments.Click
        agcDeployment.Show()
        Me.Hide()
    End Sub
    Private Sub btnJobs_Click(sender As Object, e As EventArgs) Handles btnJobs.Click
        agcJobs.Show()
        Me.Hide()
    End Sub

    ' View Application Details
    Private Sub ViewApplication_Click(sender As Object, e As EventArgs) Handles ViewApplication.Click
        If DataGridView1.SelectedRows.Count > 0 Then
            Dim selectedRow As DataGridViewRow = DataGridView1.SelectedRows(0)
            Dim applicationId As Integer = Convert.ToInt32(selectedRow.Cells("ApplicationID").Value)
            Dim agencyId As Integer = Session.CurrentReferenceID

            Dim query As String = $"SELECT * FROM application WHERE ApplicationID = {applicationId} AND AgencyID = {agencyId}"
            readQuery(query)

            If cmdRead.HasRows Then
                cmdRead.Close()
                ' ✅ Just view the application, no need to store ID in session
                Dim viewForm As New ApplicationDetails(applicationId)
                viewForm.ShowDialog()
            Else
                cmdRead.Close()
                MessageBox.Show("Unauthorized access or invalid selection.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
        Else
            MessageBox.Show("Please select an application from the list.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

End Class
