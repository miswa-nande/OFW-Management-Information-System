Public Class ofws
    Private Sub ofws_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Load OFW data
        LoadOFWsToDGV(DataGridView1)
        FormatDGVUniformly(DataGridView1)

        ' Populate Sex ComboBox
        cbxSex.Items.Clear()
        cbxSex.Items.Add("Male")
        cbxSex.Items.Add("Female")
        cbxSex.Items.Add("Other")

        ' Populate Civil Status ComboBox
        cbxCivStat.Items.Clear()
        cbxCivStat.Items.Add("Single")
        cbxCivStat.Items.Add("Married")
        cbxCivStat.Items.Add("Widowed")
        cbxCivStat.Items.Add("Separated")
        cbxCivStat.Items.Add("Divorced")

        ' Optional: No default selection
        cbxSex.SelectedIndex = -1
        cbxCivStat.SelectedIndex = -1
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Dim dlg As New addOfw()
        dlg.ShowDialog()
        LoadOFWsToDGV(DataGridView1)
        FormatDGVUniformly(DataGridView1)
    End Sub

    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        Dim dlg As New editOfw()
        dlg.ShowDialog()
        LoadOFWsToDGV(DataGridView1)
        FormatDGVUniformly(DataGridView1)
    End Sub

    Private Sub btnDeployments_Click(sender As Object, e As EventArgs) Handles btnDeployments.Click
        Dim newForm As New deployments()
        newForm.Show()
        Me.Hide()
    End Sub

    Private Sub btnDashboard_Click(sender As Object, e As EventArgs) Handles btnDashboard.Click
        Dim newForm As New dashboard()
        newForm.Show()
        Me.Hide()
    End Sub

    Private Sub btnAgencies_Click(sender As Object, e As EventArgs) Handles btnAgencies.Click
        Dim newForm As New agencies()
        newForm.Show()
        Me.Hide()
    End Sub

    Private Sub btnEmployers_Click(sender As Object, e As EventArgs) Handles btnEmployers.Click
        Dim newForm As New employers()
        newForm.Show()
        Me.Hide()
    End Sub

    ' FILTER LOGIC
    Private Sub ApplyOFWFilters()
        ' Check if all filters are empty/default
        Dim allFiltersCleared As Boolean =
            txtbxIdNum.Text.Trim() = "" AndAlso
            txtbxFName.Text.Trim() = "" AndAlso
            txtbxMName.Text.Trim() = "" AndAlso
            txtbxLName.Text.Trim() = "" AndAlso
            txtbxZipcode.Text.Trim() = "" AndAlso
            txtbxVisaNum.Text.Trim() = "" AndAlso
            txtbxOecNum.Text.Trim() = "" AndAlso
            cbxSex.SelectedIndex = -1 AndAlso
            cbxCivStat.SelectedIndex = -1

        ' If all filters are cleared, load full data
        If allFiltersCleared Then
            LoadOFWsToDGV(DataGridView1)
            FormatDGVUniformly(DataGridView1)
            Return
        End If

        ' Otherwise, apply filtered query
        Dim query As String = "SELECT * FROM ofw WHERE 1=1"

        If txtbxIdNum.Text.Trim() <> "" Then
            query &= " AND OFWId LIKE '%" & txtbxIdNum.Text.Trim() & "%'"
        End If

        If txtbxFName.Text.Trim() <> "" Then
            query &= " AND FirstName LIKE '%" & txtbxFName.Text.Trim() & "%'"
        End If

        If txtbxMName.Text.Trim() <> "" Then
            query &= " AND MiddleName LIKE '%" & txtbxMName.Text.Trim() & "%'"
        End If

        If txtbxLName.Text.Trim() <> "" Then
            query &= " AND LastName LIKE '%" & txtbxLName.Text.Trim() & "%'"
        End If

        If cbxSex.SelectedIndex <> -1 Then
            query &= " AND Sex = '" & cbxSex.SelectedItem.ToString() & "'"
        End If

        If cbxCivStat.SelectedIndex <> -1 Then
            query &= " AND CivilStatus = '" & cbxCivStat.SelectedItem.ToString() & "'"
        End If

        If txtbxZipcode.Text.Trim() <> "" Then
            query &= " AND ZipCode LIKE '%" & txtbxZipcode.Text.Trim() & "%'"
        End If

        If txtbxVisaNum.Text.Trim() <> "" Then
            query &= " AND VisaNumber LIKE '%" & txtbxVisaNum.Text.Trim() & "%'"
        End If

        If txtbxOecNum.Text.Trim() <> "" Then
            query &= " AND OECNumber LIKE '%" & txtbxOecNum.Text.Trim() & "%'"
        End If

        If chkbxEmployed.CheckState <> CheckState.Indeterminate Then
            If chkbxEmployed.Checked Then
                query &= " AND EXISTS (SELECT 1 FROM deploymentrecord dr WHERE dr.ofwID = ofw.ofwID)"
            Else
                query &= " AND NOT EXISTS (SELECT 1 FROM deploymentrecord dr WHERE dr.ofwID = ofw.ofwID)"
            End If
        End If

        readQuery(query)
        Dim dt As New DataTable()
        dt.Load(cmdRead)
        cmdRead.Close()

        DataGridView1.DataSource = dt
        FormatDGVUniformly(DataGridView1)
    End Sub

    ' LIVE FILTER EVENTS
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

    Private Sub chkbxEmployed_CheckedChanged(sender As Object, e As EventArgs) Handles chkbxEmployed.CheckedChanged
        ApplyOFWFilters()
    End Sub
    ' REPORT GENERATION
    Private Sub btnGenerate_Click(sender As Object, e As EventArgs) Handles btnGenerate.Click
        Dim dt As DataTable = CType(DataGridView1.DataSource, DataTable)
        Dim previewForm As New ReportPreviewForm(dt)
        previewForm.ShowDialog()
    End Sub


End Class
