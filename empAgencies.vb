Imports System.Data

Public Class empAgencies
    Private Sub empAgencies_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadAgencyList()
    End Sub

    Private Sub LoadAgencyList()
        ApplyAgencyFilters()
    End Sub

    Private Sub ApplyAgencyFilters()
        Dim query As String = """
            SELECT 
                AgencyName AS 'Agency Name',
                AgencyLicenseNumber AS 'License Number',
                CASE WHEN GovAccreditationStat = 'Accredited' THEN 'Active' ELSE 'Inactive' END AS 'Status',
                Specialization
            FROM agency
            WHERE 1=1
        """
        If txtbxAgencyName IsNot Nothing AndAlso txtbxAgencyName.Text.Trim() <> "" Then
            query &= " AND AgencyName LIKE '%" & txtbxAgencyName.Text.Trim().Replace("'", "''") & "%'"
        End If
        If txtbxAgencyLicNum IsNot Nothing AndAlso txtbxAgencyLicNum.Text.Trim() <> "" Then
            query &= " AND AgencyLicenseNumber LIKE '%" & txtbxAgencyLicNum.Text.Trim().Replace("'", "''") & "%'"
        End If
        If cbxGovtAccredStat IsNot Nothing AndAlso cbxGovtAccredStat.SelectedIndex <> -1 Then
            If cbxGovtAccredStat.Text = "Active" Then
                query &= " AND GovAccreditationStat = 'Accredited'"
            ElseIf cbxGovtAccredStat.Text = "Inactive" Then
                query &= " AND GovAccreditationStat <> 'Accredited'"
            End If
        End If
        If txtbxSpecialization IsNot Nothing AndAlso txtbxSpecialization.Text.Trim() <> "" Then
            query &= " AND Specialization LIKE '%" & txtbxSpecialization.Text.Trim().Replace("'", "''") & "%'"
        End If
        readQuery(query)
        Dim dt As New DataTable()
        dt.Load(cmdRead)
        cmdRead.Close()
        DataGridView1.DataSource = dt
        FormatDGVUniformly(DataGridView1)
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
    Private Sub txtbxAgencyName_TextChanged(sender As Object, e As EventArgs) Handles txtbxAgencyName.TextChanged
        ApplyAgencyFilters()
    End Sub
    Private Sub txtbxLicenseNumber_TextChanged(sender As Object, e As EventArgs) Handles txtbxAgencyLicNum.TextChanged
        ApplyAgencyFilters()
    End Sub
    Private Sub cbxStatus_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxGovtAccredStat.SelectedIndexChanged
        ApplyAgencyFilters()
    End Sub
    Private Sub txtbxSpecialization_TextChanged(sender As Object, e As EventArgs) Handles txtbxSpecialization.TextChanged
        ApplyAgencyFilters()
    End Sub

    ' Navigation button handlers
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