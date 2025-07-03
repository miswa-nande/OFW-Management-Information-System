Imports MySql.Data.MySqlClient

Public Class agcEmployers
    Private Sub agcEmployers_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadEmployers()
        FormatDGVUniformly(DataGridView1)
    End Sub

    Private Sub LoadEmployers()
        Dim agencyId As Integer = Session.CurrentReferenceID
        Dim query As String = "SELECT e.*, 
            CASE 
                WHEN ape.Status IS NOT NULL THEN ape.Status 
                WHEN pr.RequestID IS NOT NULL THEN 'Pending' 
                ELSE 'Not Partnered' 
            END AS PartnershipStatus
            FROM employer e
            LEFT JOIN agencypartneremployer ape ON e.employer_id = ape.EmployerID AND ape.AgencyID = " & agencyId & "
            LEFT JOIN partnershiprequest pr ON e.employer_id = pr.EmployerID AND pr.AgencyID = " & agencyId & " AND pr.DateResponded IS NULL"

        readQuery(query)
        Dim dt As New DataTable()
        dt.Load(cmdRead)
        cmdRead.Close()
        DataGridView1.DataSource = dt
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

    Private Sub ApplyEmployerFilters()
        Dim agencyId As Integer = Session.CurrentReferenceID
        Dim query As String = "SELECT e.*, 
            CASE 
                WHEN ape.Status IS NOT NULL THEN ape.Status 
                WHEN pr.RequestID IS NOT NULL THEN 'Pending' 
                ELSE 'Not Partnered' 
            END AS PartnershipStatus
            FROM employer e
            LEFT JOIN agencypartneremployer ape ON e.employer_id = ape.EmployerID AND ape.AgencyID = " & agencyId & "
            LEFT JOIN partnershiprequest pr ON e.employer_id = pr.EmployerID AND pr.AgencyID = " & agencyId & " AND pr.DateResponded IS NULL
            WHERE 1 = 1"

        If txtbxIdNum.Text.Trim() <> "" Then query &= " AND e.employer_id LIKE '%" & txtbxIdNum.Text.Trim() & "%'"
        If txtbxFName.Text.Trim() <> "" Then query &= " AND e.first_name LIKE '%" & txtbxFName.Text.Trim() & "%'"
        If txtbxMName.Text.Trim() <> "" Then query &= " AND e.middle_name LIKE '%" & txtbxMName.Text.Trim() & "%'"
        If txtbxLName.Text.Trim() <> "" Then query &= " AND e.last_name LIKE '%" & txtbxLName.Text.Trim() & "%'"
        If txtbxEmail.Text.Trim() <> "" Then query &= " AND e.email LIKE '%" & txtbxEmail.Text.Trim() & "%'"
        If txtbxContactNum.Text.Trim() <> "" Then query &= " AND e.contact_number LIKE '%" & txtbxContactNum.Text.Trim() & "%'"
        If txtbxCompanyName.Text.Trim() <> "" Then query &= " AND e.company_name LIKE '%" & txtbxCompanyName.Text.Trim() & "%'"
        If txtbxIndustry.Text.Trim() <> "" Then query &= " AND e.industry LIKE '%" & txtbxIndustry.Text.Trim() & "%'"

        readQuery(query)
        Dim dt As New DataTable()
        dt.Load(cmdRead)
        cmdRead.Close()
        DataGridView1.DataSource = dt
        FormatDGVUniformly(DataGridView1)
    End Sub

    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        txtbxIdNum.Clear()
        txtbxFName.Clear()
        txtbxMName.Clear()
        txtbxLName.Clear()
        txtbxEmail.Clear()
        txtbxContactNum.Clear()
        txtbxCompanyName.Clear()
        txtbxIndustry.Clear()
        txtbxNumOfOFW.Clear()
        txtbxActiveJobs.Clear()
        LoadEmployers()
        FormatDGVUniformly(DataGridView1)
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
    Private Sub btnApplications_Click(sender As Object, e As EventArgs) Handles btnApplications.Click
        Dim appForm As New agcApplications()
        appForm.Show()
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

    Private Sub SendPartnershipReqBTN_Click(sender As Object, e As EventArgs) Handles SendPartnershipReqBTN.Click
        If DataGridView1.SelectedRows.Count > 0 Then
            Dim empId As Integer = DataGridView1.SelectedRows(0).Cells("employer_id").Value
            Dim checkQuery As String = $"SELECT * FROM partnershiprequest WHERE AgencyID = {Session.CurrentReferenceID} AND EmployerID = {empId} AND DateResponded IS NULL"
            readQuery(checkQuery)

            If cmdRead.HasRows Then
                MessageBox.Show("A partnership request is already pending.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                Dim insertQuery As String = $"INSERT INTO partnershiprequest (AgencyID, EmployerID) VALUES ({Session.CurrentReferenceID}, {empId})"
                readQuery(insertQuery)
                MessageBox.Show("Partnership request sent.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                LoadEmployers()
            End If
        Else
            MessageBox.Show("Please select an employer first.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub

    Private Sub RemovePartnershipBTN_Click(sender As Object, e As EventArgs) Handles RemovePartnershipBTN.Click
        If DataGridView1.SelectedRows.Count > 0 Then
            Dim empId As Integer = DataGridView1.SelectedRows(0).Cells("employer_id").Value
            Dim result = MessageBox.Show("Are you sure you want to remove this partnership?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If result = DialogResult.Yes Then
                Dim deleteQuery As String = $"DELETE FROM agencypartneremployer WHERE AgencyID = {Session.CurrentReferenceID} AND EmployerID = {empId}"
                readQuery(deleteQuery)
                MessageBox.Show("Partnership removed.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information)
                LoadEmployers()
            End If
        Else
            MessageBox.Show("Please select an employer first.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub

    Private Sub CancelReqBTN_Click(sender As Object, e As EventArgs) Handles CancelReqBTN.Click
        If DataGridView1.SelectedRows.Count > 0 Then
            Dim empId As Integer = DataGridView1.SelectedRows(0).Cells("employer_id").Value
            Dim deleteQuery As String = $"DELETE FROM partnershiprequest WHERE AgencyID = {Session.CurrentReferenceID} AND EmployerID = {empId} AND DateResponded IS NULL"
            readQuery(deleteQuery)
            MessageBox.Show("Request canceled.", "Canceled", MessageBoxButtons.OK, MessageBoxIcon.Information)
            LoadEmployers()
        Else
            MessageBox.Show("Please select an employer first.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub

    ' Live filter handlers
    Private Sub txtbxIdNum_TextChanged(sender As Object, e As EventArgs) Handles txtbxIdNum.TextChanged
        ApplyEmployerFilters()
    End Sub
    Private Sub txtbxFName_TextChanged(sender As Object, e As EventArgs) Handles txtbxFName.TextChanged
        ApplyEmployerFilters()
    End Sub
    Private Sub txtbxMName_TextChanged(sender As Object, e As EventArgs) Handles txtbxMName.TextChanged
        ApplyEmployerFilters()
    End Sub
    Private Sub txtbxLName_TextChanged(sender As Object, e As EventArgs) Handles txtbxLName.TextChanged
        ApplyEmployerFilters()
    End Sub
    Private Sub txtbxEmail_TextChanged(sender As Object, e As EventArgs) Handles txtbxEmail.TextChanged
        ApplyEmployerFilters()
    End Sub
    Private Sub txtbxContactNum_TextChanged(sender As Object, e As EventArgs) Handles txtbxContactNum.TextChanged
        ApplyEmployerFilters()
    End Sub
    Private Sub txtbxCompanyName_TextChanged(sender As Object, e As EventArgs) Handles txtbxCompanyName.TextChanged
        ApplyEmployerFilters()
    End Sub
    Private Sub txtbxIndustry_TextChanged(sender As Object, e As EventArgs) Handles txtbxIndustry.TextChanged
        ApplyEmployerFilters()
    End Sub
    Private Sub txtbxNumOfOFW_TextChanged(sender As Object, e As EventArgs) Handles txtbxNumOfOFW.TextChanged
        ApplyEmployerFilters()
    End Sub
    Private Sub txtbxActiveJobs_TextChanged(sender As Object, e As EventArgs) Handles txtbxActiveJobs.TextChanged
        ApplyEmployerFilters()
    End Sub
End Class