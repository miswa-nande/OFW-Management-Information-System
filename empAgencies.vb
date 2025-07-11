Imports System.Data
Imports MySql.Data.MySqlClient

Public Class empAgencies
    Private Sub empAgencies_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        PopulateGovtAccredStatCombo()
        LoadAgencyList()
    End Sub
    Private Sub PopulateGovtAccredStatCombo()
        Try
            Dim query As String = "SELECT DISTINCT GovAccreditationStat FROM agency WHERE GovAccreditationStat IS NOT NULL ORDER BY GovAccreditationStat"
            readQuery(query)

            cbxGovtAccredStat.Items.Clear()
            cbxGovtAccredStat.Items.Add("All") ' Optional: for no filtering

            While cmdRead.Read()
                cbxGovtAccredStat.Items.Add(cmdRead("GovAccreditationStat").ToString())
            End While
            cmdRead.Close()

            cbxGovtAccredStat.SelectedIndex = -1 ' Start unselected
        Catch ex As Exception
            MsgBox("Error loading accreditation statuses: " & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub


    Private Sub LoadAgencyList()
        ApplyAgencyFilters()
    End Sub

    Private Sub ApplyAgencyFilters()
        Dim empId = Session.CurrentReferenceID

        Dim query As String = $"
        SELECT 
            a.AgencyID,
            a.AgencyName AS 'Agency Name',
            a.AgencyLicenseNumber AS 'License Number',
            a.ContactNum AS 'Contact Number',
            CASE WHEN a.GovAccreditationStat = 'Accredited' THEN 'Active' ELSE 'Inactive' END AS 'Status',
            a.Specialization,
            CASE WHEN ape.EmployerID IS NOT NULL THEN 'Yes' ELSE 'No' END AS 'Partnered',
            CASE WHEN pr.EmployerID IS NOT NULL THEN 'Yes' ELSE 'No' END AS 'SentRequest',
            COUNT(DISTINCT dr.DeploymentID) AS 'DeployedWorkers'
        FROM agency a
        LEFT JOIN agencypartneremployer ape ON a.AgencyID = ape.AgencyID AND ape.EmployerID = {empId}
        LEFT JOIN partnershiprequest pr ON a.AgencyID = pr.AgencyID AND pr.EmployerID = {empId}
        LEFT JOIN deploymentrecord dr ON a.AgencyID = dr.AgencyID
        WHERE 1=1
        "

        If txtbxAgencyID.Text.Trim() <> "" Then
            query &= " AND a.AgencyID LIKE '%" & txtbxAgencyID.Text.Trim().Replace("'", "''") & "%'"
        End If
        If txtbxAgencyName.Text.Trim() <> "" Then
            query &= " AND a.AgencyName LIKE '%" & txtbxAgencyName.Text.Trim().Replace("'", "''") & "%'"
        End If
        If txtbxAgencyLicNum.Text.Trim() <> "" Then
            query &= " AND a.AgencyLicenseNumber LIKE '%" & txtbxAgencyLicNum.Text.Trim().Replace("'", "''") & "%'"
        End If
        If txtbxContact.Text.Trim() <> "" Then
            query &= " AND a.ContactNum LIKE '%" & txtbxContact.Text.Trim().Replace("'", "''") & "%'"
        End If
        If cbxGovtAccredStat.SelectedIndex <> -1 AndAlso cbxGovtAccredStat.Text <> "All" Then
            query &= " AND a.GovAccreditationStat = '" & cbxGovtAccredStat.Text.Replace("'", "''") & "'"
        End If
        If txtbxSpecialization.Text.Trim() <> "" Then
            query &= " AND a.Specialization LIKE '%" & txtbxSpecialization.Text.Trim().Replace("'", "''") & "%'"
        End If

        query &= " GROUP BY a.AgencyID"

        Dim havingConditions As New List(Of String)

        If txtbxNumDepWorkers.Text.Trim() <> "" Then
            If IsNumeric(txtbxNumDepWorkers.Text.Trim()) Then
                havingConditions.Add("DeployedWorkers = " & txtbxNumDepWorkers.Text.Trim())
            Else
                MsgBox("Please enter a valid number for Deployed Workers.", MsgBoxStyle.Exclamation)
                Return
            End If
        End If

        If havingConditions.Count > 0 Then
            query &= " HAVING " & String.Join(" AND ", havingConditions)
        End If

        readQuery(query)
        Dim dt As New DataTable()
        dt.Load(cmdRead)
        cmdRead.Close()

        DataGridView1.DataSource = dt
        FormatDGVUniformly(DataGridView1)

        If DataGridView1.Columns.Contains("AgencyID") Then
            DataGridView1.Columns("AgencyID").Visible = False
        End If

        HighlightAgencyRows()
    End Sub

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
        End With
    End Sub

    Private Sub HighlightAgencyRows()
        For Each row As DataGridViewRow In DataGridView1.Rows
            Dim isPartnered = row.Cells("Partnered").Value.ToString() = "Yes"
            Dim sentRequest = row.Cells("SentRequest").Value.ToString() = "Yes"

            If isPartnered Then
                row.DefaultCellStyle.BackColor = Color.LightGreen
            ElseIf sentRequest Then
                row.DefaultCellStyle.BackColor = Color.LightYellow
            Else
                row.DefaultCellStyle.BackColor = Color.White
            End If
        Next
    End Sub

    ' === Filter Events ===
    Private Sub txtbxAgencyID_TextChanged(sender As Object, e As EventArgs) Handles txtbxAgencyID.TextChanged
        ApplyAgencyFilters()
    End Sub

    Private Sub txtbxAgencyName_TextChanged(sender As Object, e As EventArgs) Handles txtbxAgencyName.TextChanged
        ApplyAgencyFilters()
    End Sub

    Private Sub txtbxLicenseNumber_TextChanged(sender As Object, e As EventArgs) Handles txtbxAgencyLicNum.TextChanged
        ApplyAgencyFilters()
    End Sub

    Private Sub txtbxContact_TextChanged(sender As Object, e As EventArgs) Handles txtbxContact.TextChanged
        ApplyAgencyFilters()
    End Sub

    Private Sub cbxGovtAccredStat_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxGovtAccredStat.SelectedIndexChanged
        ApplyAgencyFilters()
    End Sub

    Private Sub txtbxSpecialization_TextChanged(sender As Object, e As EventArgs) Handles txtbxSpecialization.TextChanged
        ApplyAgencyFilters()
    End Sub

    Private Sub txtbxNumDepWorkers_TextChanged(sender As Object, e As EventArgs) Handles txtbxNumDepWorkers.TextChanged
        ApplyAgencyFilters()
    End Sub

    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        txtbxAgencyID.Clear()
        txtbxAgencyName.Clear()
        txtbxAgencyLicNum.Clear()
        txtbxContact.Clear()
        cbxGovtAccredStat.SelectedIndex = -1
        txtbxSpecialization.Clear()
        txtbxNumDepWorkers.Clear()
        LoadAgencyList()
    End Sub

    ' === Partnership Buttons ===
    Private Sub sendpartnership_Click(sender As Object, e As EventArgs) Handles sendpartnership.Click
        If DataGridView1.SelectedRows.Count = 0 Then
            MsgBox("Please select an agency to send a partnership request.", MsgBoxStyle.Information)
            Return
        End If

        Dim agencyId = CInt(DataGridView1.SelectedRows(0).Cells("AgencyID").Value)
        Dim checkQuery = $"SELECT * FROM partnershiprequest WHERE AgencyID = {agencyId} AND EmployerID = {Session.CurrentReferenceID}"
        readQuery(checkQuery)

        If cmdRead.HasRows Then
            MsgBox("Request already exists.")
            cmdRead.Close()
            Return
        End If
        cmdRead.Close()

        Dim insertQuery = $"INSERT INTO partnershiprequest (AgencyID, EmployerID) VALUES ({agencyId}, {Session.CurrentReferenceID})"
        readQuery(insertQuery)

        MsgBox("Partnership request sent.")
        LoadAgencyList()
    End Sub

    Private Sub cancelrequest_Click(sender As Object, e As EventArgs) Handles cancelrequest.Click
        If DataGridView1.SelectedRows.Count = 0 Then
            MsgBox("Select a request to cancel.", MsgBoxStyle.Information)
            Return
        End If

        Dim agencyId = CInt(DataGridView1.SelectedRows(0).Cells("AgencyID").Value)
        Dim query = $"DELETE FROM partnershiprequest WHERE AgencyID = {agencyId} AND EmployerID = {Session.CurrentReferenceID}"
        readQuery(query)
        MsgBox("Partnership request cancelled.")
        LoadAgencyList()
    End Sub

    Private Sub removepartnership_Click(sender As Object, e As EventArgs) Handles removepartnership.Click
        If DataGridView1.SelectedRows.Count = 0 Then
            MsgBox("Select a partnership to remove.", MsgBoxStyle.Information)
            Return
        End If

        Dim agencyId = CInt(DataGridView1.SelectedRows(0).Cells("AgencyID").Value)
        Dim result = MessageBox.Show("Are you sure you want to remove this partnership?", "Confirm", MessageBoxButtons.YesNo)
        If result = DialogResult.Yes Then
            Dim query = $"DELETE FROM agencypartneremployer WHERE AgencyID = {agencyId} AND EmployerID = {Session.CurrentReferenceID}"
            readQuery(query)
            MsgBox("Partnership removed.")
            LoadAgencyList()
        End If
    End Sub

    Private Sub ACCEPT_Click(sender As Object, e As EventArgs) Handles ACCEPT.Click
        If DataGridView1.SelectedRows.Count = 0 Then
            MsgBox("Please select an agency.", MsgBoxStyle.Information)
            Return
        End If

        Dim row = DataGridView1.SelectedRows(0)
        If row.Cells("SentRequest").Value.ToString() <> "Yes" Then
            MsgBox("This agency did not send a partnership request.", MsgBoxStyle.Exclamation)
            Return
        End If

        Dim agencyId = CInt(row.Cells("AgencyID").Value)
        Dim insert = $"INSERT INTO agencypartneremployer (AgencyID, EmployerID) VALUES ({agencyId}, {Session.CurrentReferenceID})"
        Dim delete = $"DELETE FROM partnershiprequest WHERE AgencyID = {agencyId} AND EmployerID = {Session.CurrentReferenceID}"
        readQuery(insert)
        readQuery(delete)

        MsgBox("Partnership accepted.")
        LoadAgencyList()
    End Sub

    Private Sub REJECT_Click(sender As Object, e As EventArgs) Handles REJECT.Click
        If DataGridView1.SelectedRows.Count = 0 Then
            MsgBox("Please select an agency.", MsgBoxStyle.Information)
            Return
        End If

        Dim row = DataGridView1.SelectedRows(0)
        If row.Cells("SentRequest").Value.ToString() <> "Yes" Then
            MsgBox("No request to reject.", MsgBoxStyle.Exclamation)
            Return
        End If

        Dim agencyId = CInt(row.Cells("AgencyID").Value)
        Dim delete = $"DELETE FROM partnershiprequest WHERE AgencyID = {agencyId} AND EmployerID = {Session.CurrentReferenceID}"
        readQuery(delete)

        MsgBox("Request rejected.")
        LoadAgencyList()
    End Sub

    ' === Navigation ===
    Private Sub btnDashboard_Click(sender As Object, e As EventArgs) Handles btnDashboard.Click
        Dim form As New empDashboard()
        form.Show()
        Me.Hide()
    End Sub

    Private Sub btnJobs_Click(sender As Object, e As EventArgs) Handles btnJobs.Click
        Dim form As New empJobs()
        form.Show()
        Me.Hide()
    End Sub

    Private Sub btnOfws_Click(sender As Object, e As EventArgs) Handles btnOfws.Click
        Dim form As New empOfws()
        form.Show()
        Me.Hide()
    End Sub
End Class
