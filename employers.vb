Public Class employers

    Private Sub employers_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadEmployersToDGV(DataGridView1)
        FormatDGVUniformly(DataGridView1)

        Me.WindowState = FormWindowState.Maximized
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

    Private Sub btnOfw_Click(sender As Object, e As EventArgs) Handles btnOfw.Click
        Dim newForm As New ofws()
        newForm.Show()
        Me.Hide()
    End Sub

    Private Sub btnAgencies_Click(sender As Object, e As EventArgs) Handles btnAgencies.Click
        Dim newForm As New agencies()
        newForm.Show()
        Me.Hide()
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Dim dlg As New addEmployer()
        dlg.ShowDialog()
        LoadEmployersToDGV(DataGridView1)
        FormatDGVUniformly(DataGridView1)
    End Sub

    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        If DataGridView1.SelectedRows.Count > 0 Then
            Dim selectedRow As DataGridViewRow = DataGridView1.SelectedRows(0)
            Dim employerId As Integer = Convert.ToInt32(selectedRow.Cells("EmployerID").Value)

            Dim dlg As New editEmployer(employerId) ' Pass ID to constructor
            dlg.ShowDialog()
            LoadEmployersToDGV(DataGridView1)
            FormatDGVUniformly(DataGridView1)
        Else
            MsgBox("Please select an employer to edit.", MsgBoxStyle.Information)
        End If
    End Sub


    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If DataGridView1.SelectedRows.Count > 0 Then
            Dim selectedRow As DataGridViewRow = DataGridView1.SelectedRows(0)
            Dim employerId As Integer = Convert.ToInt32(selectedRow.Cells("EmployerID").Value)

            Dim result As DialogResult = MessageBox.Show("Are you sure you want to delete this employer? This action cannot be undone.", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)

            If result = DialogResult.Yes Then
                DeleteRecord("employer", "EmployerID", employerId)
                LoadEmployersToDGV(DataGridView1)
                FormatDGVUniformly(DataGridView1)
            End If
        Else
            MessageBox.Show("Please select an employer to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub ApplyEmployerFilters()
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
            LoadEmployersToDGV(DataGridView1)
            FormatDGVUniformly(DataGridView1)
            Return
        End If

        Dim query As String = "SELECT * FROM employer WHERE 1=1"

        If txtbxIdNum.Text.Trim() <> "" Then
            query &= " AND EmployerID LIKE '%" & txtbxIdNum.Text.Trim() & "%'"
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

        If txtbxEmail.Text.Trim() <> "" Then
            query &= " AND Email LIKE '%" & txtbxEmail.Text.Trim() & "%'"
        End If

        If txtbxContactNum.Text.Trim() <> "" Then
            query &= " AND ContactNumber LIKE '%" & txtbxContactNum.Text.Trim() & "%'"
        End If

        If txtbxCompanyName.Text.Trim() <> "" Then
            query &= " AND CompanyName LIKE '%" & txtbxCompanyName.Text.Trim() & "%'"
        End If

        If txtbxIndustry.Text.Trim() <> "" Then
            query &= " AND Industry LIKE '%" & txtbxIndustry.Text.Trim() & "%'"
        End If

        If txtbxNumOfOFW.Text.Trim() <> "" Then
            query &= " AND NumOfOFW LIKE '%" & txtbxNumOfOFW.Text.Trim() & "%'"
        End If

        If txtbxActiveJobs.Text.Trim() <> "" Then
            query &= " AND ActiveJobPostings LIKE '%" & txtbxActiveJobs.Text.Trim() & "%'"
        End If

        readQuery(query)
        Dim dt As New DataTable()
        dt.Load(cmdRead)
        cmdRead.Close()

        DataGridView1.DataSource = dt
        FormatDGVUniformly(DataGridView1)
    End Sub

    ' Live filters
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

    ' Clear filter fields and reload all data
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

        LoadEmployersToDGV(DataGridView1)
        FormatDGVUniformly(DataGridView1)
    End Sub

    Private Sub btnJobPlacements_Click_1(sender As Object, e As EventArgs) Handles btnJobPlacements.Click
        Dim newForm As New jobplacement()
        newForm.Show()
        Me.Hide()
    End Sub

    Private Sub btnConfig_Click(sender As Object, e As EventArgs) Handles btnConfig.Click
        Dim newForm As New AdminConfiguration()
        newForm.Show()
        Me.Hide()
    End Sub

    Private Sub btnGenerate_Click(sender As Object, e As EventArgs) Handles btnGenerate.Click

    End Sub
End Class
