Public Class deploymentrecords
    Private selectedOfwId As Integer

    ' Constructor for OFW (no argument)
    Public Sub New()
        InitializeComponent()
        selectedOfwId = Session.CurrentReferenceID
    End Sub

    ' Constructor for Employer/Agency viewing a specific OFW
    Public Sub New(ofwId As Integer)
        InitializeComponent()
        selectedOfwId = ofwId
    End Sub

    Private Sub deploymentrecords_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Only OFWs can view their own deployment record directly
        If Session.CurrentLoggedUser.userType = "OFW" Then
            If selectedOfwId <> Session.CurrentReferenceID Then
                MsgBox("Access denied. You can only view your own records.", MsgBoxStyle.Critical)
                Me.Close()
                Exit Sub
            End If
        End If

        If Session.CurrentLoggedUser.userType = "Employer" Then
            ' Disable navigation for employer
            btnProfile.Enabled = False
            btnApplications.Enabled = False
            btnJobOffers.Enabled = False
        End If

        LoadDeploymentRecords()
        FormatDGV()
    End Sub

    ' Load all deployment records for the selected OFW
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
            )"

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
            If .Columns.Contains("RepatriationStatus") Then .Columns("RepatriationStatus").HeaderText = "Repatriated?"
            If .Columns.Contains("ReasonForReturn") Then .Columns("ReasonForReturn").HeaderText = "Reason for Return"
        End With
    End Sub

    ' Navigation buttons
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

    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        LoadDeploymentRecords()
    End Sub
End Class
