Imports MySql.Data.MySqlClient

Public Class agcEmployers
    Private Sub agcEmployers_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadEmployers()
        FormatDGVUniformly(DataGridView1)
    End Sub

    Private Sub LoadEmployers()
        Dim agencyId As Integer = Session.CurrentReferenceID
        Dim query As String = "SELECT * FROM employer WHERE agency_id = " & agencyId
        readQuery(query)
        Dim dt As New DataTable()
        dt.Load(cmdRead)
        cmdRead.Close()
        DataGridView1.DataSource = dt
    End Sub

    Private Sub ApplyEmployerFilters()
        Dim agencyId As Integer = Session.CurrentReferenceID
        Dim allCleared As Boolean =
            txtbxIdNum.Text.Trim() = "" AndAlso
            txtbxFName.Text.Trim() = "" AndAlso
            txtbxMName.Text.Trim() = "" AndAlso
            txtbxLName.Text.Trim() = "" AndAlso
            txtbxEmail.Text.Trim() = "" AndAlso
            txtbxContactNum.Text.Trim() = "" AndAlso
            txtbxCompanyName.Text.Trim() = "" AndAlso
            txtbxIndustry.Text.Trim() = "" AndAlso
            txtbxNumOfOFW.Text.Trim() = "" AndAlso
            txtbxActiveJobs.Text.Trim() = ""

        If allCleared Then
            LoadEmployers()
            FormatDGVUniformly(DataGridView1)
            Return
        End If

        Dim query As String = "SELECT * FROM employer WHERE agency_id = " & agencyId
        If txtbxIdNum.Text.Trim() <> "" Then
            query &= " AND employer_id LIKE '%" & txtbxIdNum.Text.Trim() & "%'"
        End If
        If txtbxFName.Text.Trim() <> "" Then
            query &= " AND first_name LIKE '%" & txtbxFName.Text.Trim() & "%'"
        End If
        If txtbxMName.Text.Trim() <> "" Then
            query &= " AND middle_name LIKE '%" & txtbxMName.Text.Trim() & "%'"
        End If
        If txtbxLName.Text.Trim() <> "" Then
            query &= " AND last_name LIKE '%" & txtbxLName.Text.Trim() & "%'"
        End If
        If txtbxEmail.Text.Trim() <> "" Then
            query &= " AND email LIKE '%" & txtbxEmail.Text.Trim() & "%'"
        End If
        If txtbxContactNum.Text.Trim() <> "" Then
            query &= " AND contact_number LIKE '%" & txtbxContactNum.Text.Trim() & "%'"
        End If
        If txtbxCompanyName.Text.Trim() <> "" Then
            query &= " AND company_name LIKE '%" & txtbxCompanyName.Text.Trim() & "%'"
        End If
        If txtbxIndustry.Text.Trim() <> "" Then
            query &= " AND industry LIKE '%" & txtbxIndustry.Text.Trim() & "%'"
        End If
        If txtbxNumOfOFW.Text.Trim() <> "" Then
            query &= " AND num_of_ofw LIKE '%" & txtbxNumOfOFW.Text.Trim() & "%'"
        End If
        If txtbxActiveJobs.Text.Trim() <> "" Then
            query &= " AND active_jobs LIKE '%" & txtbxActiveJobs.Text.Trim() & "%'"
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

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

    End Sub
End Class