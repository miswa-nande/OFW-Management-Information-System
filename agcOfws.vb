Imports MySql.Data.MySqlClient

Public Class agcOfws
    Private Sub agcOfws_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadAgencyOFWs()
        FormatDGVUniformly(DataGridView1)

        ' Populate filter comboboxes
        cbxSex.Items.Clear()
        cbxSex.Items.AddRange(New String() {"Male", "Female", "Other"})
        cbxSex.SelectedIndex = -1

        cbxCivStat.Items.Clear()
        cbxCivStat.Items.AddRange(New String() {"Single", "Married", "Widowed", "Separated"})
        cbxCivStat.SelectedIndex = -1
    End Sub

    Private Sub LoadAgencyOFWs()
        Dim agencyId As Integer = Session.CurrentReferenceID ' Or however you store the logged-in agency's ID
        Dim query As String = $"SELECT * FROM ofw WHERE AgencyID = {agencyId}"
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
            For Each col As DataGridViewColumn In .Columns
                col.MinimumWidth = 100
            Next
        End With
    End Sub

    Private Sub ApplyOFWFilters()
        Dim agencyId As Integer = Session.CurrentReferenceID
        Dim query As String = $"SELECT * FROM ofw WHERE AgencyID = {agencyId}"

        If txtbxIdNum.Text.Trim() <> "" Then
            query &= $" AND OFWId LIKE '%{txtbxIdNum.Text.Trim()}%'"
        End If
        If txtbxFName.Text.Trim() <> "" Then
            query &= $" AND FirstName LIKE '%{txtbxFName.Text.Trim()}%'"
        End If
        If txtbxMName.Text.Trim() <> "" Then
            query &= $" AND MiddleName LIKE '%{txtbxMName.Text.Trim()}%'"
        End If
        If txtbxLName.Text.Trim() <> "" Then
            query &= $" AND LastName LIKE '%{txtbxLName.Text.Trim()}%'"
        End If
        If cbxSex.SelectedIndex <> -1 Then
            query &= $" AND Sex = '{cbxSex.SelectedItem.ToString()}'"
        End If
        If cbxCivStat.SelectedIndex <> -1 Then
            query &= $" AND CivilStatus = '{cbxCivStat.SelectedItem.ToString()}'"
        End If
        If txtbxZipcode.Text.Trim() <> "" Then
            query &= $" AND Zipcode LIKE '%{txtbxZipcode.Text.Trim()}%'"
        End If
        If txtbxVisaNum.Text.Trim() <> "" Then
            query &= $" AND VISANum LIKE '%{txtbxVisaNum.Text.Trim()}%'"
        End If
        If txtbxOecNum.Text.Trim() <> "" Then
            query &= $" AND OECNum LIKE '%{txtbxOecNum.Text.Trim()}%'"
        End If

        readQuery(query)
        Dim dt As New DataTable()
        dt.Load(cmdRead)
        cmdRead.Close()
        DataGridView1.DataSource = dt
    End Sub

    ' Filter event handlers
    Private Sub txtbxIdNum_TextChanged(sender As Object, e As EventArgs) Handles txtbxIdNum.TextChanged
        ApplyOFWFilters()
    End Sub
    Private Sub txtbxFName_TextChanged(sender As Object, e As EventArgs) Handles txtbxFName.TextChanged
        ApplyOFWFilters()
    End Sub
    Private Sub txtbxMName_TextChanged(sender As Object, e As EventArgs) Handles txtbxMName.TextChanged
        ApplyOFWFilters()
    End Sub
    Private Sub txtbxLName_TextChanged(sender As Object, e As EventArgs) Handles txtbxLName.TextChanged
        ApplyOFWFilters()
    End Sub
    Private Sub cbxSex_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxSex.SelectedIndexChanged
        ApplyOFWFilters()
    End Sub
    Private Sub cbxCivStat_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxCivStat.SelectedIndexChanged
        ApplyOFWFilters()
    End Sub
    Private Sub txtbxZipcode_TextChanged(sender As Object, e As EventArgs) Handles txtbxZipcode.TextChanged
        ApplyOFWFilters()
    End Sub
    Private Sub txtbxVisaNum_TextChanged(sender As Object, e As EventArgs) Handles txtbxVisaNum.TextChanged
        ApplyOFWFilters()
    End Sub
    Private Sub txtbxOecNum_TextChanged(sender As Object, e As EventArgs) Handles txtbxOecNum.TextChanged
        ApplyOFWFilters()
    End Sub

    ' Clear button
    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        txtbxIdNum.Clear()
        txtbxFName.Clear()
        txtbxMName.Clear()
        txtbxLName.Clear()
        cbxSex.SelectedIndex = -1
        cbxCivStat.SelectedIndex = -1
        txtbxZipcode.Clear()
        txtbxVisaNum.Clear()
        txtbxOecNum.Clear()
        LoadAgencyOFWs()
    End Sub

    ' Navigation buttons
    Private Sub btnDashboard_Click(sender As Object, e As EventArgs) Handles btnDashboard.Click
        Dim newForm As New agcDashboard()
        newForm.Show()
        Me.Hide()
    End Sub
    Private Sub btnEmployers_Click(sender As Object, e As EventArgs) Handles btnEmployers.Click
        Dim newForm As New agcEmployers()
        newForm.Show()
        Me.Hide()
    End Sub
    Private Sub btnApplications_Click(sender As Object, e As EventArgs) Handles btnApplications.Click
        Dim newForm As New agcApplications()
        newForm.Show()
        Me.Hide()
    End Sub
    Private Sub btnDeployments_Click(sender As Object, e As EventArgs) Handles btnDeployments.Click
        Dim newForm As New agcDeployment()
        newForm.Show()
        Me.Hide()
    End Sub
    Private Sub btnJobs_Click(sender As Object, e As EventArgs) Handles btnJobs.Click
        Dim newForm As New agcJobs()
        newForm.Show()
        Me.Hide()
    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

    End Sub
End Class