Imports MySql.Data.MySqlClient

Public Class deploymentrecords
    Private selectedOfwId As Integer

    ' Constructor for OFW
    Public Sub New()
        InitializeComponent()
        selectedOfwId = Session.CurrentReferenceID
    End Sub

    ' Constructor for Employer/Agency
    Public Sub New(ofwId As Integer)
        InitializeComponent()
        selectedOfwId = ofwId
    End Sub

    Private Sub deploymentrecords_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Session.CurrentLoggedUser.userType = "OFW" AndAlso selectedOfwId <> Session.CurrentReferenceID Then
            MsgBox("Access denied. You can only view your own records.", MsgBoxStyle.Critical)
            Me.Close()
            Exit Sub
        End If

        If Session.CurrentLoggedUser.userType = "Employer" Then
            btnProfile.Enabled = False
            btnApplications.Enabled = False
            btnJobOffers.Enabled = False
        End If

        ' Uncheck date filters on load
        dateContractStart.Checked = False
        dateContractEnd.Checked = False

        PopulateFilters()
        LoadDeploymentRecords()
        FormatDGV()
    End Sub

    Private Sub PopulateFilters()
        cbxContractStat.Items.Clear()
        cbxRepatriationStat.Items.Clear()
        cbxReasonforReturn.Items.Clear()

        Dim fields As String() = {"DeploymentStatus", "RepatriationStatus", "ReasonForReturn"}

        For Each field In fields
            Dim query = $"SELECT DISTINCT {field} FROM deploymentrecord WHERE {field} IS NOT NULL AND {field} <> ''"
            readQuery(query)
            While cmdRead.Read()
                Dim value = cmdRead(0).ToString()
                If field = "DeploymentStatus" Then cbxContractStat.Items.Add(value)
                If field = "RepatriationStatus" Then cbxRepatriationStat.Items.Add(value)
                If field = "ReasonForReturn" Then cbxReasonforReturn.Items.Add(value)
            End While
            cmdRead.Close()
        Next
    End Sub

    Private Sub LoadDeploymentRecords()
        Try
            Dim query As String = $"
                SELECT d.DeploymentID, d.ContractNumber, jp.JobTitle, d.CountryOfDeployment,
                       d.Salary, d.DeploymentStatus, d.ContractStartDate, d.ContractEndDate,
                       d.RepatriationStatus, d.ReasonForReturn
                FROM deploymentrecord d
                JOIN jobplacement jp ON d.JobPlacementID = jp.JobPlacementID
                WHERE d.ApplicationID IN (
                    SELECT a.ApplicationID FROM application a WHERE a.OFWID = {selectedOfwId}
                )
            "

            ' === Apply filters ===
            If txtbxIdNum.Text.Trim() <> "" AndAlso IsNumeric(txtbxIdNum.Text.Trim()) Then
                query &= $" AND d.DeploymentID = {txtbxIdNum.Text.Trim()}"
            End If

            If txtbxJobTitle.Text.Trim() <> "" Then
                query &= $" AND jp.JobTitle LIKE '%{txtbxJobTitle.Text.Trim().Replace("'", "''")}%'"
            End If

            If txtbxCountryOfDep.Text.Trim() <> "" Then
                query &= $" AND d.CountryOfDeployment LIKE '%{txtbxCountryOfDep.Text.Trim().Replace("'", "''")}%'"
            End If

            If txtbxSalary.Text.Trim() <> "" AndAlso IsNumeric(txtbxSalary.Text.Trim()) Then
                query &= $" AND d.Salary >= {txtbxSalary.Text.Trim()}"
            End If

            If cbxContractStat.SelectedIndex <> -1 Then
                query &= $" AND d.DeploymentStatus = '{cbxContractStat.Text.Replace("'", "''")}'"
            End If

            If cbxRepatriationStat.SelectedIndex <> -1 Then
                query &= $" AND d.RepatriationStatus = '{cbxRepatriationStat.Text.Replace("'", "''")}'"
            End If

            If cbxReasonforReturn.SelectedIndex <> -1 Then
                query &= $" AND d.ReasonForReturn = '{cbxReasonforReturn.Text.Replace("'", "''")}'"
            End If

            If dateContractStart.Checked Then
                query &= $" AND DATE(d.ContractStartDate) >= '{dateContractStart.Value:yyyy-MM-dd}'"
            End If

            If dateContractEnd.Checked Then
                query &= $" AND DATE(d.ContractEndDate) <= '{dateContractEnd.Value:yyyy-MM-dd}'"
            End If

            LoadToDGV(query, DataGridView1)

        Catch ex As Exception
            MsgBox("Error loading deployment records: " & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub FormatDGV()
        With DataGridView1
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
            .DefaultCellStyle.SelectionBackColor = Color.FromArgb(100, 150, 200)
            .DefaultCellStyle.SelectionForeColor = Color.Black

            If .Columns.Contains("DeploymentID") Then .Columns("DeploymentID").Visible = False
            If .Columns.Contains("ContractNumber") Then .Columns("ContractNumber").HeaderText = "Contract #"
            If .Columns.Contains("JobTitle") Then .Columns("JobTitle").HeaderText = "Job Title"
            If .Columns.Contains("CountryOfDeployment") Then .Columns("CountryOfDeployment").HeaderText = "Country"
            If .Columns.Contains("Salary") Then .Columns("Salary").HeaderText = "Salary"
            If .Columns.Contains("DeploymentStatus") Then .Columns("DeploymentStatus").HeaderText = "Status"
            If .Columns.Contains("ContractStartDate") Then .Columns("ContractStartDate").HeaderText = "Start Date"
            If .Columns.Contains("ContractEndDate") Then .Columns("ContractEndDate").HeaderText = "End Date"
            If .Columns.Contains("RepatriationStatus") Then .Columns("RepatriationStatus").HeaderText = "Repatriated"
            If .Columns.Contains("ReasonForReturn") Then .Columns("ReasonForReturn").HeaderText = "Reason for Return"
        End With
    End Sub

    ' === Filter Events ===
    Private Sub txtbxIdNum_TextChanged(sender As Object, e As EventArgs) Handles txtbxIdNum.TextChanged
        LoadDeploymentRecords()
    End Sub

    Private Sub txtbxJobTitle_TextChanged(sender As Object, e As EventArgs) Handles txtbxJobTitle.TextChanged
        LoadDeploymentRecords()
    End Sub

    Private Sub txtbxCountryOfDep_TextChanged(sender As Object, e As EventArgs) Handles txtbxCountryOfDep.TextChanged
        LoadDeploymentRecords()
    End Sub

    Private Sub txtbxSalary_TextChanged(sender As Object, e As EventArgs) Handles txtbxSalary.TextChanged
        LoadDeploymentRecords()
    End Sub

    Private Sub cbxContractStat_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxContractStat.SelectedIndexChanged
        LoadDeploymentRecords()
    End Sub

    Private Sub cbxRepatriationStat_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxRepatriationStat.SelectedIndexChanged
        LoadDeploymentRecords()
    End Sub

    Private Sub cbxReasonforReturn_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxReasonforReturn.SelectedIndexChanged
        LoadDeploymentRecords()
    End Sub

    Private Sub dateContractStart_ValueChanged(sender As Object, e As EventArgs) Handles dateContractStart.ValueChanged
        LoadDeploymentRecords()
    End Sub

    Private Sub dateContractEnd_ValueChanged(sender As Object, e As EventArgs) Handles dateContractEnd.ValueChanged
        LoadDeploymentRecords()
    End Sub

    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        txtbxIdNum.Clear()
        txtbxJobTitle.Clear()
        txtbxCountryOfDep.Clear()
        txtbxSalary.Clear()
        cbxContractStat.SelectedIndex = -1
        cbxRepatriationStat.SelectedIndex = -1
        cbxReasonforReturn.SelectedIndex = -1
        dateContractStart.Value = Date.Today
        dateContractStart.Checked = False
        dateContractEnd.Value = Date.Today
        dateContractEnd.Checked = False
        LoadDeploymentRecords()
    End Sub

    ' === Navigation Buttons ===
    Private Sub btnProfile_Click(sender As Object, e As EventArgs) Handles btnProfile.Click
        Dim newForm As New ofwProfile()
        newForm.Show()
        Me.Hide()
    End Sub

    Private Sub btnJobOffers_Click(sender As Object, e As EventArgs) Handles btnJobOffers.Click
        Dim newForm As New joboffers()
        newForm.Show()
        Me.Hide()
    End Sub

    Private Sub btnApplications_Click(sender As Object, e As EventArgs) Handles btnApplications.Click
        Dim newForm As New applications()
        newForm.Show()
        Me.Hide()
    End Sub
End Class
