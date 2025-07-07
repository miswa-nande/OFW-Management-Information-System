Imports MySql.Data.MySqlClient
Imports System.Data

Public Class agcEmployers

    Private Sub agcEmployers_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadEmployers()
    End Sub

    Private Sub LoadEmployers()
        ApplyEmployerFilters()
    End Sub

    Private Sub ApplyEmployerFilters()
        Dim agencyId As Integer = Session.CurrentReferenceID
        Dim query As String = ""
        query &= "SELECT " & vbCrLf
        query &= "  e.EmployerID, " & vbCrLf
        query &= "  CONCAT(e.EmployerFirstName, ' ', e.EmployerMiddleName, ' ', e.EmployerLastName) AS FullName, " & vbCrLf
        query &= "  e.EmployerEmail, " & vbCrLf
        query &= "  e.EmployerContactNum, " & vbCrLf
        query &= "  e.CompanyName, " & vbCrLf
        query &= "  e.Industry, " & vbCrLf
        query &= "  e.NumOfOFWHired, " & vbCrLf
        query &= "  e.ActiveJobPlacement, " & vbCrLf
        query &= "  CONCAT(e.CompanyCity, ', ', e.CompanyState, ', ', e.CompanyCountry) AS FullAddress, " & vbCrLf
        query &= "  CASE WHEN ape.EmployerID IS NOT NULL THEN 'Partnered' ELSE NULL END AS Partnered, " & vbCrLf
        query &= "  CASE WHEN pr.EmployerID IS NOT NULL THEN 'Yes' ELSE 'No' END AS SentRequest " & vbCrLf
        query &= "FROM employer e " & vbCrLf
        query &= "LEFT JOIN agencypartneremployer ape ON ape.EmployerID = e.EmployerID AND ape.AgencyID = " & agencyId & vbCrLf
        query &= "LEFT JOIN partnershiprequest pr ON pr.EmployerID = e.EmployerID AND pr.AgencyID = " & agencyId & " AND pr.DateResponded IS NULL " & vbCrLf
        query &= "WHERE 1 = 1 "

        If txtbxIdNum.Text.Trim() <> "" Then
            query &= " AND e.EmployerID LIKE '%" & txtbxIdNum.Text.Trim().Replace("'", "''") & "%'"
        End If
        If txtbxFName.Text.Trim() <> "" Then
            query &= " AND e.EmployerFirstName LIKE '%" & txtbxFName.Text.Trim().Replace("'", "''") & "%'"
        End If
        If txtbxMName.Text.Trim() <> "" Then
            query &= " AND e.EmployerMiddleName LIKE '%" & txtbxMName.Text.Trim().Replace("'", "''") & "%'"
        End If
        If txtbxLName.Text.Trim() <> "" Then
            query &= " AND e.EmployerLastName LIKE '%" & txtbxLName.Text.Trim().Replace("'", "''") & "%'"
        End If
        If txtbxEmail.Text.Trim() <> "" Then
            query &= " AND e.EmployerEmail LIKE '%" & txtbxEmail.Text.Trim().Replace("'", "''") & "%'"
        End If
        If txtbxContactNum.Text.Trim() <> "" Then
            query &= " AND e.EmployerContactNum LIKE '%" & txtbxContactNum.Text.Trim().Replace("'", "''") & "%'"
        End If
        If txtbxCompanyName.Text.Trim() <> "" Then
            query &= " AND e.CompanyName LIKE '%" & txtbxCompanyName.Text.Trim().Replace("'", "''") & "%'"
        End If
        If txtbxIndustry.Text.Trim() <> "" Then
            query &= " AND e.Industry LIKE '%" & txtbxIndustry.Text.Trim().Replace("'", "''") & "%'"
        End If
        If txtbxNumOfOFW.Text.Trim() <> "" Then
            query &= " AND e.NumOfOFWHired LIKE '%" & txtbxNumOfOFW.Text.Trim().Replace("'", "''") & "%'"
        End If
        If txtbxActiveJobs.Text.Trim() <> "" Then
            query &= " AND e.ActiveJobPlacement LIKE '%" & txtbxActiveJobs.Text.Trim().Replace("'", "''") & "%'"
        End If

        readQuery(query)
        Dim dt As New DataTable()
        dt.Load(cmdRead)
        cmdRead.Close()

        DataGridView1.DataSource = dt
        FormatDGVUniformly(DataGridView1)
        HighlightRows()
    End Sub

    Private Sub FormatDGVUniformly(dgv As DataGridView)
        With dgv
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
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
            .DefaultCellStyle.WrapMode = DataGridViewTriState.True ' Allow multi-line if needed
            .DefaultCellStyle.SelectionBackColor = Color.FromArgb(100, 150, 200)
            .DefaultCellStyle.SelectionForeColor = Color.Black

            ' Automatically resize height based on content
            .AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells

            ' Enable tooltip for truncated cells
            For Each column As DataGridViewColumn In .Columns
                column.DefaultCellStyle.WrapMode = DataGridViewTriState.True
                column.ToolTipText = column.HeaderText
            Next
        End With
    End Sub


    Private Sub HighlightRows()
        For Each row As DataGridViewRow In DataGridView1.Rows
            If row.Cells("Partnered").Value.ToString() = "Partnered" Then
                row.DefaultCellStyle.BackColor = Color.LightGreen
            ElseIf row.Cells("SentRequest").Value.ToString() = "Yes" Then
                row.DefaultCellStyle.BackColor = Color.LightYellow
            End If
        Next
    End Sub

    Private Sub txtbx_TextChanged(sender As Object, e As EventArgs) Handles _
        txtbxIdNum.TextChanged, txtbxFName.TextChanged, txtbxMName.TextChanged, txtbxLName.TextChanged,
        txtbxEmail.TextChanged, txtbxContactNum.TextChanged, txtbxCompanyName.TextChanged,
        txtbxIndustry.TextChanged, txtbxNumOfOFW.TextChanged, txtbxActiveJobs.TextChanged

        ApplyEmployerFilters()
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
    End Sub

    Private Sub btnDashboard_Click(sender As Object, e As EventArgs) Handles btnDashboard.Click
        agcDashboard.Show()
        Me.Hide()
    End Sub

    Private Sub btnJobs_Click(sender As Object, e As EventArgs) Handles btnJobs.Click
        agcJobs.Show()
        Me.Hide()
    End Sub

    Private Sub btnApplications_Click(sender As Object, e As EventArgs) Handles btnApplications.Click
        Dim appForm As New agcApplications() ' Uses default optional parameter
        appForm.Show()
        Me.Hide()
    End Sub


    Private Sub btnDeployments_Click(sender As Object, e As EventArgs) Handles btnDeployments.Click
        agcDeploymentTracking.Show()
        Me.Hide()
    End Sub

    Private Sub btnOfws_Click(sender As Object, e As EventArgs) Handles btnOfws.Click
        agcOfws.Show()
        Me.Hide()
    End Sub

    Private Sub btnEmployers_Click(sender As Object, e As EventArgs) Handles btnEmployers.Click
        LoadEmployers()
    End Sub

    Private Sub SendPartnershipReqBTN_Click(sender As Object, e As EventArgs) Handles SendPartnershipReqBTN.Click
        If DataGridView1.SelectedRows.Count > 0 Then
            Dim empId As Integer = DataGridView1.SelectedRows(0).Cells("EmployerID").Value
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

    Private Sub CancelReqBTN_Click(sender As Object, e As EventArgs) Handles CancelReqBTN.Click
        If DataGridView1.SelectedRows.Count > 0 Then
            Dim empId As Integer = DataGridView1.SelectedRows(0).Cells("EmployerID").Value
            Dim deleteQuery As String = $"DELETE FROM partnershiprequest WHERE AgencyID = {Session.CurrentReferenceID} AND EmployerID = {empId} AND DateResponded IS NULL"
            readQuery(deleteQuery)
            MessageBox.Show("Request canceled.", "Canceled", MessageBoxButtons.OK, MessageBoxIcon.Information)
            LoadEmployers()
        Else
            MessageBox.Show("Please select an employer first.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub

    Private Sub RemovePartnershipBTN_Click(sender As Object, e As EventArgs) Handles RemovePartnershipBTN.Click
        If DataGridView1.SelectedRows.Count > 0 Then
            Dim empId As Integer = DataGridView1.SelectedRows(0).Cells("EmployerID").Value
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

    Private Sub ACCEPT_Click(sender As Object, e As EventArgs) Handles ACCEPT.Click
        If DataGridView1.SelectedRows.Count > 0 Then
            Dim empId As Integer = DataGridView1.SelectedRows(0).Cells("EmployerID").Value
            Dim agencyId As Integer = Session.CurrentReferenceID

            Dim checkQuery As String = $"SELECT * FROM partnershiprequest WHERE AgencyID = {agencyId} AND EmployerID = {empId} AND DateResponded IS NULL"
            readQuery(checkQuery)

            If cmdRead.HasRows Then
                cmdRead.Close()
                Dim insertQuery As String = $"INSERT INTO agencypartneremployer (AgencyID, EmployerID, Status) VALUES ({agencyId}, {empId}, 'Accepted')"
                readQuery(insertQuery)
                Dim updateQuery As String = $"UPDATE partnershiprequest SET DateResponded = CURRENT_DATE, Status = 'Accepted' WHERE AgencyID = {agencyId} AND EmployerID = {empId} AND DateResponded IS NULL"
                readQuery(updateQuery)
                MessageBox.Show("Partnership request accepted.", "Accepted", MessageBoxButtons.OK, MessageBoxIcon.Information)
                LoadEmployers()
            Else
                MessageBox.Show("No pending request from this employer.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Else
            MessageBox.Show("Please select an employer first.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub

    Private Sub REJECT_Click(sender As Object, e As EventArgs) Handles REJECT.Click
        If DataGridView1.SelectedRows.Count > 0 Then
            Dim empId As Integer = DataGridView1.SelectedRows(0).Cells("EmployerID").Value
            Dim agencyId As Integer = Session.CurrentReferenceID

            Dim checkQuery As String = $"SELECT * FROM partnershiprequest WHERE AgencyID = {agencyId} AND EmployerID = {empId} AND DateResponded IS NULL"
            readQuery(checkQuery)

            If cmdRead.HasRows Then
                cmdRead.Close()
                Dim updateQuery As String = $"UPDATE partnershiprequest SET DateResponded = CURRENT_DATE, Status = 'Rejected' WHERE AgencyID = {agencyId} AND EmployerID = {empId} AND DateResponded IS NULL"
                readQuery(updateQuery)
                MessageBox.Show("Partnership request rejected.", "Rejected", MessageBoxButtons.OK, MessageBoxIcon.Information)
                LoadEmployers()
            Else
                MessageBox.Show("No pending request from this employer.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Else
            MessageBox.Show("Please select an employer first.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub

End Class