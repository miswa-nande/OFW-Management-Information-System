Public Class jobplacement

    Private Sub jobplacement_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadJobPlacementsToDGV(DataGridView1)
        FormatDGVUniformly(DataGridView1)

        LoadUniqueCountriesFromEmployment()

        cbxVisaType.Items.Clear()
        cbxVisaType.Items.AddRange({"Work Visa", "Tourist Visa", "Permanent Residency"})

        InitDatePicker(datePostingDate)
        InitDatePicker(dateApplicationDeadline)

        Me.WindowState = FormWindowState.Maximized
    End Sub

    Private Sub InitDatePicker(dp As DateTimePicker)
        dp.Format = DateTimePickerFormat.Custom
        dp.CustomFormat = " "
        dp.Checked = False
    End Sub

    Private Sub LoadUniqueCountriesFromEmployment()
        Dim query As String = "SELECT DISTINCT CountryOfEmployment FROM jobplacement WHERE CountryOfEmployment IS NOT NULL AND CountryOfEmployment <> '' ORDER BY CountryOfEmployment ASC"
        Try
            readQuery(query)
            cbxCountry.Items.Clear()
            While cmdRead.Read()
                cbxCountry.Items.Add(cmdRead("CountryOfEmployment").ToString())
            End While
            cmdRead.Close()
        Catch ex As Exception
            MsgBox("Error loading countries: " & ex.Message)
        End Try
    End Sub

    Private Sub ApplyJobFilters()
        Dim allCleared As Boolean =
            txtbxJobIdNum.Text.Trim() = "" AndAlso
            txtbxEmployerIdNum.Text.Trim() = "" AndAlso
            txtbxJobTitle.Text.Trim() = "" AndAlso
            txtbxJobType.Text.Trim() = "" AndAlso
            txtbxReqSkill.Text.Trim() = "" AndAlso
            txtbxSalaryRange.Text.Trim() = "" AndAlso
            cbxCountry.SelectedIndex = -1 AndAlso
            txtbxNuOfVacancies.Text.Trim() = "" AndAlso
            cbxVisaType.SelectedIndex = -1 AndAlso
            Not datePostingDate.Checked AndAlso
            Not dateApplicationDeadline.Checked

        If allCleared Then
            LoadJobPlacementsToDGV(DataGridView1)
            FormatDGVUniformly(DataGridView1)
            Return
        End If

        Dim query As String = "SELECT * FROM jobplacement WHERE 1=1"

        If txtbxJobIdNum.Text.Trim() <> "" Then
            query &= " AND JobID LIKE '%" & txtbxJobIdNum.Text.Trim() & "%'"
        End If

        If txtbxEmployerIdNum.Text.Trim() <> "" Then
            query &= " AND EmployerID LIKE '%" & txtbxEmployerIdNum.Text.Trim() & "%'"
        End If

        If txtbxJobTitle.Text.Trim() <> "" Then
            query &= " AND JobTitle LIKE '%" & txtbxJobTitle.Text.Trim() & "%'"
        End If

        If txtbxJobType.Text.Trim() <> "" Then
            query &= " AND JobType LIKE '%" & txtbxJobType.Text.Trim() & "%'"
        End If

        If txtbxReqSkill.Text.Trim() <> "" Then
            query &= " AND RequiredSkill LIKE '%" & txtbxReqSkill.Text.Trim() & "%'"
        End If

        If txtbxSalaryRange.Text.Trim() <> "" Then
            query &= " AND SalaryRange LIKE '%" & txtbxSalaryRange.Text.Trim() & "%'"
        End If

        If cbxCountry.SelectedIndex <> -1 Then
            query &= " AND CountryOfEmployment = '" & cbxCountry.SelectedItem.ToString() & "'"
        End If

        If txtbxNuOfVacancies.Text.Trim() <> "" Then
            query &= " AND NumVacancies LIKE '%" & txtbxNuOfVacancies.Text.Trim() & "%'"
        End If

        If cbxVisaType.SelectedIndex <> -1 Then
            query &= " AND VisaType = '" & cbxVisaType.SelectedItem.ToString() & "'"
        End If

        If datePostingDate.Checked Then
            query &= " AND PostingDate = '" & datePostingDate.Value.ToString("yyyy-MM-dd") & "'"
        End If

        If dateApplicationDeadline.Checked Then
            query &= " AND ApplicationDeadline = '" & dateApplicationDeadline.Value.ToString("yyyy-MM-dd") & "'"
        End If

        readQuery(query)
        Dim dt As New DataTable()
        dt.Load(cmdRead)
        cmdRead.Close()

        DataGridView1.DataSource = dt
        FormatDGVUniformly(DataGridView1)
    End Sub

    ' CRUD
    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Dim dlg As New addJob()
        dlg.ShowDialog()
        LoadJobPlacementsToDGV(DataGridView1)
        FormatDGVUniformly(DataGridView1)
    End Sub

    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        Dim dlg As New editJob()
        dlg.ShowDialog()
        LoadJobPlacementsToDGV(DataGridView1)
        FormatDGVUniformly(DataGridView1)
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If DataGridView1.SelectedRows.Count > 0 Then
            Dim selectedRow As DataGridViewRow = DataGridView1.SelectedRows(0)
            Dim jobPlacementId As Integer = Convert.ToInt32(selectedRow.Cells("JobPlacementID").Value)

            Dim result As DialogResult = MessageBox.Show("Are you sure you want to delete this job placement?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)

            If result = DialogResult.Yes Then
                DeleteRecord("JobPlacement", "JobPlacementID", jobPlacementId)
                LoadJobPlacementsToDGV(DataGridView1)
                FormatDGVUniformly(DataGridView1)
            End If
        Else
            MessageBox.Show("Please select a job placement to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    ' CLEAR FILTERS
    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        txtbxJobIdNum.Clear()
        txtbxEmployerIdNum.Clear()
        txtbxJobTitle.Clear()
        txtbxJobType.Clear()
        txtbxReqSkill.Clear()
        txtbxSalaryRange.Clear()
        cbxCountry.SelectedIndex = -1
        txtbxNuOfVacancies.Clear()
        cbxVisaType.SelectedIndex = -1
        InitDatePicker(datePostingDate)
        InitDatePicker(dateApplicationDeadline)
        ApplyJobFilters()
    End Sub

    ' LIVE FILTER EVENTS
    Private Sub txtbxJobIdNum_TextChanged(sender As Object, e As EventArgs) Handles txtbxJobIdNum.TextChanged
        ApplyJobFilters()
    End Sub

    Private Sub txtbxEmployerIdNum_TextChanged(sender As Object, e As EventArgs) Handles txtbxEmployerIdNum.TextChanged
        ApplyJobFilters()
    End Sub

    Private Sub txtbxJobTitle_TextChanged(sender As Object, e As EventArgs) Handles txtbxJobTitle.TextChanged
        ApplyJobFilters()
    End Sub

    Private Sub txtbxJobType_TextChanged(sender As Object, e As EventArgs) Handles txtbxJobType.TextChanged
        ApplyJobFilters()
    End Sub

    Private Sub txtbxReqSkill_TextChanged(sender As Object, e As EventArgs) Handles txtbxReqSkill.TextChanged
        ApplyJobFilters()
    End Sub

    Private Sub txtbxSalaryRange_TextChanged(sender As Object, e As EventArgs) Handles txtbxSalaryRange.TextChanged
        ApplyJobFilters()
    End Sub

    Private Sub cbxCountry_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxCountry.SelectedIndexChanged
        ApplyJobFilters()
    End Sub

    Private Sub txtbxNuOfVacancies_TextChanged(sender As Object, e As EventArgs) Handles txtbxNuOfVacancies.TextChanged
        ApplyJobFilters()
    End Sub

    Private Sub cbxVisaType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxVisaType.SelectedIndexChanged
        ApplyJobFilters()
    End Sub

    Private Sub datePostingDate_ValueChanged(sender As Object, e As EventArgs) Handles datePostingDate.ValueChanged
        datePostingDate.CustomFormat = "yyyy-MM-dd"
        ApplyJobFilters()
    End Sub

    Private Sub dateApplicationDeadline_ValueChanged(sender As Object, e As EventArgs) Handles dateApplicationDeadline.ValueChanged
        dateApplicationDeadline.CustomFormat = "yyyy-MM-dd"
        ApplyJobFilters()
    End Sub

    ' NAVIGATION
    Private Sub btnDashboard_Click(sender As Object, e As EventArgs) Handles btnDashboard.Click
        Dim dash As New dashboard()
        dash.Show()
        Me.Hide()
    End Sub

    Private Sub btnAgencies_Click(sender As Object, e As EventArgs) Handles btnAgencies.Click
        Dim agc As New agencies()
        agc.Show()
        Me.Hide()
    End Sub

    Private Sub btnEmployers_Click(sender As Object, e As EventArgs) Handles btnEmployers.Click
        Dim emp As New employers()
        emp.Show()
        Me.Hide()
    End Sub

    Private Sub btnOfws_Click(sender As Object, e As EventArgs) Handles btnOfw.Click
        Dim ofwForm As New ofws()
        ofwForm.Show()
        Me.Hide()
    End Sub

    Private Sub btnDeployments_Click(sender As Object, e As EventArgs) Handles btnDeployments.Click
        Dim dep As New deployments()
        dep.Show()
        Me.Hide()
    End Sub

    Private Sub btnConfig_Click(sender As Object, e As EventArgs) Handles btnConfig.Click
        Dim newForm As New AdminConfiguration()
        newForm.Show()
        Me.Hide()
    End Sub
End Class
