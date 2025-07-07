Imports System.Data

Public Class empOfws
    Private Sub empOfws_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        PopulateSexCombo()
        PopulateCivilStatusCombo()
        ClearFilters()
        LoadEmployerOfws()
    End Sub

    Private Sub LoadEmployerOfws()
        Dim employerId As Integer = Session.CurrentReferenceID

        Dim query As String = "
    SELECT 
        o.OFWId,
        CONCAT(o.FirstName, ' ', o.MiddleName, ' ', o.LastName) AS 'Name',
        o.Sex,
        o.CivilStatus,
        o.Zipcode,
        jp.JobTitle AS 'Job Title',
        dr.DeploymentStatus AS 'Deployment Status',
        dr.DeploymentDate AS 'Date Hired',
        a.AgencyName AS 'Agency',
        o.VISANum AS 'Visa Number',
        o.OECNum AS 'OEC Number'
    FROM ofw o
    JOIN application app ON o.OFWId = app.OFWID
    JOIN deploymentrecord dr ON app.ApplicationID = dr.ApplicationID
    JOIN jobplacement jp ON dr.JobPlacementID = jp.JobPlacementID
    JOIN agency a ON dr.AgencyID = a.AgencyID
    WHERE jp.EmployerID = " & employerId


        If txtbxIdNum.Text.Trim() <> "" Then
            query &= " AND o.OFWId LIKE '%" & txtbxIdNum.Text.Trim().Replace("'", "''") & "%'"
        End If
        If txtbxFName.Text.Trim() <> "" Then
            query &= " AND o.FirstName LIKE '%" & txtbxFName.Text.Trim().Replace("'", "''") & "%'"
        End If
        If txtbxMName.Text.Trim() <> "" Then
            query &= " AND o.MiddleName LIKE '%" & txtbxMName.Text.Trim().Replace("'", "''") & "%'"
        End If
        If txtbxLName.Text.Trim() <> "" Then
            query &= " AND o.LastName LIKE '%" & txtbxLName.Text.Trim().Replace("'", "''") & "%'"
        End If
        If txtbxZipcode.Text.Trim() <> "" Then
            query &= " AND o.Zipcode LIKE '%" & txtbxZipcode.Text.Trim().Replace("'", "''") & "%'"
        End If
        If cbxSex.Text.Trim() <> "" Then
            query &= " AND o.Sex = '" & cbxSex.Text.Trim().Replace("'", "''") & "'"
        End If
        If cbxCivStat.Text.Trim() <> "" Then
            query &= " AND o.CivilStatus = '" & cbxCivStat.Text.Trim().Replace("'", "''") & "'"
        End If
        If txtbxVisaNum.Text.Trim() <> "" Then
            query &= " AND o.VISANum LIKE '%" & txtbxVisaNum.Text.Trim().Replace("'", "''") & "%'"
        End If
        If txtbxOecNum.Text.Trim() <> "" Then
            query &= " AND o.OECNum LIKE '%" & txtbxOecNum.Text.Trim().Replace("'", "''") & "%'"
        End If



        readQuery(query)
        Dim dt As New DataTable()
        dt.Load(cmdRead)
        cmdRead.Close()

        DataGridView1.DataSource = dt
        FormatDGV(DataGridView1)

        If DataGridView1.Columns.Contains("OFWId") Then
            DataGridView1.Columns("OFWId").Visible = False
        End If
    End Sub

    Private Sub FormatDGV(dgv As DataGridView)
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
            .DefaultCellStyle.ForeColor = Color.Black
            .DefaultCellStyle.BackColor = Color.White
            .DefaultCellStyle.SelectionBackColor = Color.FromArgb(100, 150, 200)
            .DefaultCellStyle.SelectionForeColor = Color.Black
        End With
    End Sub

    ' === ComboBox Fillers ===
    Private Sub PopulateSexCombo()
        cbxSex.Items.Clear()
        cbxSex.Items.AddRange({"Male", "Female", "Other"})
    End Sub

    Private Sub PopulateCivilStatusCombo()
        cbxCivStat.Items.Clear()
        cbxCivStat.Items.AddRange({"Single", "Married", "Widowed", "Divorced", "Separated"})
    End Sub

    ' === Clear Filters Method ===
    Private Sub ClearFilters()
        txtbxIdNum.Clear()
        txtbxFName.Clear()
        txtbxMName.Clear()
        txtbxLName.Clear()
        txtbxZipcode.Clear()
        txtbxVisaNum.Clear()
        txtbxOecNum.Clear()
        cbxSex.SelectedIndex = -1
        cbxCivStat.SelectedIndex = -1
    End Sub

    ' === Filter Events ===
    Private Sub txtbxIdNum_TextChanged(sender As Object, e As EventArgs) Handles txtbxIdNum.TextChanged
        LoadEmployerOfws()
    End Sub

    Private Sub txtbxFName_TextChanged(sender As Object, e As EventArgs) Handles txtbxFName.TextChanged
        LoadEmployerOfws()
    End Sub

    Private Sub txtbxMName_TextChanged(sender As Object, e As EventArgs) Handles txtbxMName.TextChanged
        LoadEmployerOfws()
    End Sub

    Private Sub txtbxLName_TextChanged(sender As Object, e As EventArgs) Handles txtbxLName.TextChanged
        LoadEmployerOfws()
    End Sub

    Private Sub txtbxZipcode_TextChanged(sender As Object, e As EventArgs) Handles txtbxZipcode.TextChanged
        LoadEmployerOfws()
    End Sub

    Private Sub cbxSex_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxSex.SelectedIndexChanged
        LoadEmployerOfws()
    End Sub

    Private Sub cbxCivStat_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxCivStat.SelectedIndexChanged
        LoadEmployerOfws()
    End Sub

    Private Sub txtbxVisaNum_TextChanged(sender As Object, e As EventArgs) Handles txtbxVisaNum.TextChanged
        LoadEmployerOfws()
    End Sub

    Private Sub txtbxOecNum_TextChanged(sender As Object, e As EventArgs) Handles txtbxOecNum.TextChanged
        LoadEmployerOfws()
    End Sub

    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        ClearFilters()
        LoadEmployerOfws()
    End Sub

    ' === Navigation Buttons ===
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

    Private Sub btnAgencies_Click(sender As Object, e As EventArgs) Handles btnAgencies.Click
        Dim form As New empAgencies()
        form.Show()
        Me.Hide()
    End Sub

    ' === View Actions ===
    Private Sub viewDetailsOfw_Click(sender As Object, e As EventArgs) Handles viewDetailsOfw.Click
        If DataGridView1.SelectedRows.Count > 0 Then
            Dim ofwId As Integer = Convert.ToInt32(DataGridView1.SelectedRows(0).Cells("OFWId").Value)
            Dim viewForm As New editOfw(ofwId)
            viewForm.ShowDialog()
        Else
            MsgBox("Please select an OFW from the list first.", MsgBoxStyle.Exclamation)
        End If
    End Sub

    Private Sub viewDeploymentRecordOfw_Click(sender As Object, e As EventArgs) Handles viewDeploymentRecordOfw.Click
        If DataGridView1.SelectedRows.Count > 0 Then
            Dim ofwId As Integer = Convert.ToInt32(DataGridView1.SelectedRows(0).Cells("OFWId").Value)
            Dim viewDepForm As New deploymentrecords(ofwId)

            ' Set minimum size
            viewDepForm.MinimumSize = New Size(1000, 1000)

            viewDepForm.ShowDialog()
        Else
            MsgBox("Please select an OFW from the list first.", MsgBoxStyle.Exclamation)
        End If
    End Sub

End Class
