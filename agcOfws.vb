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
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            .AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None
            .DefaultCellStyle.Font = New Font("Segoe UI", 10)
            .ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 11, FontStyle.Bold)
            .RowTemplate.Height = 30
            .DefaultCellStyle.WrapMode = DataGridViewTriState.False
            .AllowUserToResizeRows = False
            .AllowUserToResizeColumns = True
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .MultiSelect = False
            .ReadOnly = True
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
            .RowHeadersVisible = False
            .BorderStyle = BorderStyle.None
            .EnableHeadersVisualStyles = False
            .BackgroundColor = Color.White

            ' Header style
            .ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 66, 125)
            .ColumnHeadersDefaultCellStyle.ForeColor = Color.White
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            ' Row style
            .DefaultCellStyle.ForeColor = Color.Black
            .DefaultCellStyle.SelectionBackColor = Color.FromArgb(173, 216, 230)
            .DefaultCellStyle.SelectionForeColor = Color.Black

            .AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 248, 255)
            .AlternatingRowsDefaultCellStyle.ForeColor = Color.Black


            ' Adjust all column minimum widths
            For Each col As DataGridViewColumn In .Columns
                col.MinimumWidth = 100
            Next
        End With

        ' Optional: Rename important headers for clarity
        If dgv.Columns.Contains("OFWId") Then dgv.Columns("OFWId").HeaderText = "OFW ID"
        If dgv.Columns.Contains("FirstName") Then dgv.Columns("FirstName").HeaderText = "First Name"
        If dgv.Columns.Contains("MiddleName") Then dgv.Columns("MiddleName").HeaderText = "Middle Name"
        If dgv.Columns.Contains("LastName") Then dgv.Columns("LastName").HeaderText = "Last Name"
        If dgv.Columns.Contains("Sex") Then dgv.Columns("Sex").HeaderText = "Gender"
        If dgv.Columns.Contains("CivilStatus") Then dgv.Columns("CivilStatus").HeaderText = "Civil Status"
        If dgv.Columns.Contains("Zipcode") Then dgv.Columns("Zipcode").HeaderText = "ZIP Code"
        If dgv.Columns.Contains("VISANum") Then dgv.Columns("VISANum").HeaderText = "Visa Number"
        If dgv.Columns.Contains("OECNum") Then dgv.Columns("OECNum").HeaderText = "OEC Number"
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

    ' Add OFW
    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        If Session.CurrentLoggedUser.userType = "Agency" Then
            ' Count rows before showing the form
            Dim rowCountBefore As Integer = DataGridView1.Rows.Count

            Dim dlg As New addOfw()
            dlg.ShowDialog()

            ' Reload if number of rows increased (meaning a new OFW was added)
            LoadAgencyOFWs()

            Dim rowCountAfter As Integer = DataGridView1.Rows.Count
            If rowCountAfter > rowCountBefore Then
                FormatDGVUniformly(DataGridView1)
            End If
        Else
            MessageBox.Show("Only agencies can add OFWs.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub

    ' Edit OFW
    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        If DataGridView1.SelectedRows.Count > 0 Then
            Dim selectedRow As DataGridViewRow = DataGridView1.SelectedRows(0)
            Dim ofwId As Integer = Convert.ToInt32(selectedRow.Cells("OFWId").Value)

            Dim dlg As New editOfw(ofwId)
            Dim result As DialogResult = dlg.ShowDialog()

            If result = DialogResult.OK Then
                ' Reload only if Save was clicked (DialogResult.OK)
                LoadAgencyOFWs()
                FormatDGVUniformly(DataGridView1)
            End If
        Else
            MessageBox.Show("Please select an OFW to edit.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub



    ' Delete OFW
    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If DataGridView1.SelectedRows.Count > 0 Then
            Dim selectedRow As DataGridViewRow = DataGridView1.SelectedRows(0)
            Dim ofwId As Integer = Convert.ToInt32(selectedRow.Cells("OFWId").Value)

            Dim result As DialogResult = MessageBox.Show("Are you sure you want to delete this OFW record? This action cannot be undone.", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
            If result = DialogResult.Yes Then
                DeleteRecord("ofw", "OFWId", ofwId)
                LoadAgencyOFWs()
                FormatDGVUniformly(DataGridView1)
            End If
        Else
            MessageBox.Show("Please select an OFW to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub btnGenerate_Click(sender As Object, e As EventArgs) Handles btnGenerate.Click

    End Sub
End Class