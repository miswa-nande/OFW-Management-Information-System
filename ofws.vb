Imports MySql.Data.MySqlClient

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

        Me.WindowState = FormWindowState.Maximized
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

    Private Sub btnConfig_Click(sender As Object, e As EventArgs) Handles btnConfig.Click
        Dim newForm As New AdminConfiguration()
        newForm.Show()
        Me.Hide()
    End Sub

    ' FILTER LOGIC
    Private Sub ApplyOFWFilters()
        ' Check if all filters are empty
        If txtbxIdNum.Text.Trim() = "" AndAlso
           txtbxFName.Text.Trim() = "" AndAlso
           txtbxMName.Text.Trim() = "" AndAlso
           txtbxLName.Text.Trim() = "" AndAlso
           cbxSex.SelectedIndex = -1 AndAlso
           cbxCivStat.SelectedIndex = -1 AndAlso
           txtbxZipcode.Text.Trim() = "" AndAlso
           txtbxVisaNum.Text.Trim() = "" AndAlso
           txtbxOecNum.Text.Trim() = "" Then

            LoadOFWsToDGV(DataGridView1)
            FormatDGVUniformly(DataGridView1)
            Return
        End If

        ' Base query
        Dim query As String = "SELECT OFWId, FirstName, MiddleName, LastName, DOB, Sex, CivilStatus, Street, Barangay, City, Province, Zipcode, EducationalLevel, Skills, ContactNum, EmergencyContactNum, PassportNum, VISANum, OECNum, EmploymentStatus, DateAdded, AgencyID FROM ofw WHERE 1=1"
        Dim params As New List(Of MySqlParameter)

        ' Add filters
        If txtbxIdNum.Text.Trim() <> "" Then
            query &= " AND OFWId LIKE @ofwId"
            params.Add(New MySqlParameter("@ofwId", "%" & txtbxIdNum.Text.Trim() & "%"))
        End If

        If txtbxFName.Text.Trim() <> "" Then
            query &= " AND FirstName LIKE @fName"
            params.Add(New MySqlParameter("@fName", "%" & txtbxFName.Text.Trim() & "%"))
        End If

        If txtbxMName.Text.Trim() <> "" Then
            query &= " AND MiddleName LIKE @mName"
            params.Add(New MySqlParameter("@mName", "%" & txtbxMName.Text.Trim() & "%"))
        End If

        If txtbxLName.Text.Trim() <> "" Then
            query &= " AND LastName LIKE @lName"
            params.Add(New MySqlParameter("@lName", "%" & txtbxLName.Text.Trim() & "%"))
        End If

        If cbxSex.SelectedIndex <> -1 Then
            query &= " AND Sex = @sex"
            params.Add(New MySqlParameter("@sex", cbxSex.SelectedItem.ToString()))
        End If

        If cbxCivStat.SelectedIndex <> -1 Then
            query &= " AND CivilStatus = @civilStat"
            params.Add(New MySqlParameter("@civilStat", cbxCivStat.SelectedItem.ToString()))
        End If

        If txtbxZipcode.Text.Trim() <> "" Then
            query &= " AND ZipCode LIKE @zip"
            params.Add(New MySqlParameter("@zip", "%" & txtbxZipcode.Text.Trim() & "%"))
        End If

        If txtbxVisaNum.Text.Trim() <> "" Then
            query &= " AND VISANum LIKE @visa"
            params.Add(New MySqlParameter("@visa", "%" & txtbxVisaNum.Text.Trim() & "%"))
        End If

        If txtbxOecNum.Text.Trim() <> "" Then
            query &= " AND OECNum LIKE @oec"
            params.Add(New MySqlParameter("@oec", "%" & txtbxOecNum.Text.Trim() & "%"))
        End If

        If chkbxEmployed.CheckState <> CheckState.Indeterminate Then
            If chkbxEmployed.Checked Then
                query &= " AND EXISTS (SELECT 1 FROM DeploymentRecord dr JOIN Application a ON dr.ApplicationID = a.ApplicationID WHERE a.OFWId = ofw.OFWId)"
            Else
                query &= " AND NOT EXISTS (SELECT 1 FROM DeploymentRecord dr JOIN Application a ON dr.ApplicationID = a.ApplicationID WHERE a.OFWId = ofw.OFWId)"
            End If
        End If

        ' Execute
        Try
            openConn(db_name)
            Using cmd As New MySqlCommand(query, conn)
                cmd.Parameters.AddRange(params.ToArray())
                Dim dt As New DataTable()
                Using reader As MySqlDataReader = cmd.ExecuteReader()
                    dt.Load(reader)
                End Using
                DataGridView1.DataSource = dt
                FormatDGVUniformly(DataGridView1)
            End Using
        Catch ex As Exception
            MsgBox("An error occurred while filtering: " & ex.Message, MsgBoxStyle.Critical)
        Finally
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try
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

    ' CLEAR BUTTON
    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        txtbxIdNum.Clear()
        txtbxFName.Clear()
        txtbxMName.Clear()
        txtbxLName.Clear()
        txtbxZipcode.Clear()
        txtbxVisaNum.Clear()
        txtbxOecNum.Clear()
        cbxSex.SelectedIndex = -1
        cbxCivStat.SelectedIndex = -1

        LoadOFWsToDGV(DataGridView1)
        FormatDGVUniformly(DataGridView1)
    End Sub

    ' REPORT GENERATION
    Private Sub btnGenerate_Click(sender As Object, e As EventArgs) Handles btnGenerate.Click
        Dim dt As DataTable = CType(DataGridView1.DataSource, DataTable)
        Dim previewForm As New ReportPreviewForm(dt)
        previewForm.ShowDialog()
    End Sub

    ' DELETE
    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If DataGridView1.SelectedRows.Count > 0 Then
            Dim selectedRow As DataGridViewRow = DataGridView1.SelectedRows(0)
            Dim ofwId As Integer = Convert.ToInt32(selectedRow.Cells("OFWId").Value)

            Dim result As DialogResult = MessageBox.Show("Are you sure you want to delete this OFW record? This action cannot be undone.", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)

            If result = DialogResult.Yes Then
                DeleteRecord("ofw", "OFWId", ofwId)
                LoadOFWsToDGV(DataGridView1)
                FormatDGVUniformly(DataGridView1)
            End If
        Else
            MessageBox.Show("Please select an OFW to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub
End Class
