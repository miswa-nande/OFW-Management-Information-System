Imports System.Data

Public Class empOfws
    Private Sub empOfws_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadEmployerOfws()
    End Sub

    Private Sub LoadEmployerOfws()
        Dim employerId As Integer = Session.CurrentReferenceID
        Dim query As String = """
            SELECT 
                CONCAT(o.FirstName, ' ', o.MiddleName, ' ', o.LastName) AS 'Name',
                jp.JobTitle AS 'Job Title',
                dr.DeploymentStatus AS 'Deployment Status',
                dr.DeploymentDate AS 'Date Hired',
                a.AgencyName AS 'Agency',
                o.OFWId
            FROM ofw o
            JOIN deploymentrecord dr ON o.OFWId = dr.ApplicationID
            JOIN jobplacement jp ON dr.JobPlacementID = jp.JobPlacementID
            JOIN agency a ON dr.AgencyID = a.AgencyID
            WHERE jp.EmployerID = " & employerId
        readQuery(query)
        Dim dt As New DataTable()
        dt.Load(cmdRead)
        cmdRead.Close()
        DataGridView1.DataSource = dt
        FormatDGVUniformly(DataGridView1)
        ' Add button columns if not already present
        If DataGridView1.Columns("ViewDetails") Is Nothing Then
            Dim btnDetails As New DataGridViewButtonColumn()
            btnDetails.Name = "ViewDetails"
            btnDetails.HeaderText = ""
            btnDetails.Text = "View Details"
            btnDetails.UseColumnTextForButtonValue = True
            DataGridView1.Columns.Add(btnDetails)
        End If
        If DataGridView1.Columns("ViewDeployment") Is Nothing Then
            Dim btnDeployment As New DataGridViewButtonColumn()
            btnDeployment.Name = "ViewDeployment"
            btnDeployment.HeaderText = ""
            btnDeployment.Text = "View Deployment Record"
            btnDeployment.UseColumnTextForButtonValue = True
            DataGridView1.Columns.Add(btnDeployment)
        End If
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
        End With
    End Sub

    ' Navigation button handlers
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
End Class
