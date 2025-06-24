Public Class jobplacement
    Private Sub jobplacement_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadJobPlacementsToDGV(DataGridView1)
        FormatDGVUniformly(DataGridView1)
        cbxCountry.Items.AddRange({"Japan", "Saudi Arabia", "Qatar", "UAE", "Canada", "Australia", "New Zealand", "United Kingdom", "South Korea", "Singapore", "Hong Kong", "Taiwan", "Germany", "Italy", "Norway"})
        cbxVisaType.Items.AddRange({"Work Visa", "Tourist Visa", "Permanent Residency"})
    End Sub

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
            ' Get the ID of the selected job placement
            Dim selectedRow As DataGridViewRow = DataGridView1.SelectedRows(0)
            Dim jobPlacementId As Integer = Convert.ToInt32(selectedRow.Cells("JobPlacementID").Value)

            ' Ask for confirmation
            Dim result As DialogResult = MessageBox.Show("Are you sure you want to delete this job placement? This action cannot be undone.", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)

            If result = DialogResult.Yes Then
                ' Call the generic delete method
                DeleteRecord("JobPlacement", "JobPlacementID", jobPlacementId)

                ' Refresh the DataGridView
                LoadJobPlacementsToDGV(DataGridView1)
                FormatDGVUniformly(DataGridView1)
            End If
        Else
            MessageBox.Show("Please select a job placement to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub btnEmployers_Click(sender As Object, e As EventArgs) Handles btnEmployers.Click
        Dim newForm As New employers()
        newForm.Show()
        Me.Hide()
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
            cbxVisaType.SelectedIndex = -1

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
            query &= " AND Country = '" & cbxCountry.SelectedItem.ToString() & "'"
        End If

        If txtbxNuOfVacancies.Text.Trim() <> "" Then
            query &= " AND NumVacancies LIKE '%" & txtbxNuOfVacancies.Text.Trim() & "%'"
        End If

        If cbxVisaType.SelectedIndex <> -1 Then
            query &= " AND VisaType = '" & cbxVisaType.SelectedItem.ToString() & "'"
        End If

        readQuery(query)
        Dim dt As New DataTable()
        dt.Load(cmdRead)
        cmdRead.Close()

        DataGridView1.DataSource = dt
        FormatDGVUniformly(DataGridView1)
    End Sub

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

    Private Sub btnFilter_Click(sender As Object, e As EventArgs) Handles btnFilter.Click
        ApplyJobFilters()
    End Sub

    Private Sub datePostingDate_ValueChanged(sender As Object, e As EventArgs) Handles datePostingDate.ValueChanged
        ' Optional: Implement if filtering by date is needed
    End Sub

    Private Sub dateApplicationDeadline_ValueChanged(sender As Object, e As EventArgs) Handles dateApplicationDeadline.ValueChanged
        ' Optional: Implement if filtering by date is needed
    End Sub

    Private Sub dateLicExp_ValueChanged(sender As Object, e As EventArgs) Handles dateLicExp.ValueChanged
        ' Optional: May not apply to job placements
    End Sub

End Class
