Imports System.Drawing
Imports System.Windows.Forms.DataVisualization.Charting
Imports MySql.Data.MySqlClient

Public Class agcDashboard
    Private Sub agcDashboard_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadSummary()
        LoadDeploymentChart()
        LoadDeploymentStatusDonut()
        Me.WindowState = FormWindowState.Maximized
    End Sub

    Private Sub LoadSummary()
        readQuery($"SELECT * FROM agency WHERE AgencyID = {Session.CurrentReferenceID}")
        If cmdRead.Read() Then
            lblIDNum.Text = Session.CurrentReferenceID.ToString()
            lblAccreditationStatus.Text = cmdRead("GovAccreditationStat").ToString()
            lblAgencyName.Text = cmdRead("AgencyName").ToString()
            lblLicenseNum.Text = cmdRead("AgencyLicenseNumber").ToString()
            lblEmail.Text = cmdRead("Email").ToString()
            lblContactNum.Text = cmdRead("ContactNum").ToString()
            lblSpecialization.Text = cmdRead("Specialization").ToString()
            lblFullAddress.Text = $"{cmdRead("Street")}, {cmdRead("City")}, {cmdRead("State")}, {cmdRead("Zipcode")}"
        End If
        cmdRead.Close()

        lblNumJobPosted.Text = CountRows($"SELECT JobPlacementID FROM jobplacement WHERE AgencyID = {Session.CurrentReferenceID}")
        TotalApplicationsReceived.Text = CountRows($"SELECT a.ApplicationID FROM application a JOIN jobplacement jp ON a.JobPlacementID = jp.JobPlacementID WHERE jp.AgencyID = {Session.CurrentReferenceID}")
        lblNumOfw.Text = CountRows($"SELECT OFWID FROM ofw WHERE AgencyID = {Session.CurrentReferenceID}")
        lblNumEmployers.Text = CountRows($"SELECT DISTINCT e.EmployerID FROM employer e JOIN agencypartneremployer ap ON e.EmployerID = ap.EmployerID WHERE ap.AgencyID = {Session.CurrentReferenceID}")

        LoadToDGV($"
            SELECT 
                e.EmployerID AS 'Employer ID', 
                e.CompanyName AS 'Company Name', 
                e.EmployerEmail AS 'Email', 
                e.EmployerContactNum AS 'Contact Number', 
                ap.DateStablished AS 'Date Partnered'
            FROM employer e 
            JOIN agencypartneremployer ap ON e.EmployerID = ap.EmployerID 
            WHERE ap.AgencyID = {Session.CurrentReferenceID}
            ORDER BY ap.DateStablished DESC", dgvPartnerEmployers)
        FormatDGV(dgvPartnerEmployers)

        LoadApplicationsSummary()
    End Sub

    Private Sub LoadApplicationsSummary()
        LoadToDGV($"
            SELECT 
                a.ApplicationID AS 'App ID',
                o.FirstName AS 'OFW First Name',
                o.LastName AS 'OFW Last Name',
                jp.JobTitle AS 'Job Title',
                a.ApplicationStatus AS 'Status',
                a.ApplicationDate AS 'Date Applied'
            FROM application a
            JOIN jobplacement jp ON a.JobPlacementID = jp.JobPlacementID
            JOIN ofw o ON a.OFWID = o.OFWID
            WHERE jp.AgencyID = {Session.CurrentReferenceID}
            ORDER BY a.ApplicationDate DESC", ApplicationsDGV)
        FormatDGV(ApplicationsDGV)
    End Sub

    Private Function CountRows(query As String) As Integer
        Try
            Dim dt As New DataTable()
            Using adapter As New MySqlDataAdapter(query, conn)
                adapter.Fill(dt)
            End Using
            Return dt.Rows.Count
        Catch ex As Exception
            MessageBox.Show("Error counting rows: " & ex.Message)
            Return 0
        End Try
    End Function

    Private Sub FormatDGV(dgv As DataGridView)
        With dgv
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells
            .AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .MultiSelect = False
            .ReadOnly = True
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
            .RowHeadersVisible = False
            .BorderStyle = BorderStyle.None
            .EnableHeadersVisualStyles = False
            .BackgroundColor = Color.White

            ' Header formatting
            .ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 66, 155)
            .ColumnHeadersDefaultCellStyle.ForeColor = Color.White
            .ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 10, FontStyle.Bold)
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            ' Cell formatting
            .DefaultCellStyle.Font = New Font("Segoe UI", 10)
            .DefaultCellStyle.WrapMode = DataGridViewTriState.True
            .DefaultCellStyle.SelectionBackColor = Color.LightBlue
            .DefaultCellStyle.SelectionForeColor = Color.Black

            ' Set minimum width for specific columns if needed
            For Each col As DataGridViewColumn In .Columns
                If col.HeaderText = "Email" OrElse col.HeaderText = "Company Name" Then
                    col.MinimumWidth = 150
                End If
            Next
        End With
    End Sub


    Private Sub LoadDeploymentStatusDonut(Optional fromDate As Date = Nothing, Optional toDate As Date = Nothing)
        Dim filter As String = If(fromDate <> Nothing AndAlso toDate <> Nothing, $" AND d.DeploymentDate BETWEEN '{fromDate:yyyy-MM-dd}' AND '{toDate:yyyy-MM-dd}'", "")
        Dim query As String = $"
            SELECT d.DeploymentStatus, COUNT(*) AS Count
            FROM deploymentrecord d
            JOIN jobplacement jp ON d.JobPlacementID = jp.JobPlacementID
            WHERE jp.AgencyID = {Session.CurrentReferenceID} {filter}
            GROUP BY d.DeploymentStatus"
        readQuery(query)

        With DeploymentStatus
            .Series.Clear()
            Dim series As New Series("Deployment Status")
            series.ChartType = SeriesChartType.Doughnut
            series.IsValueShownAsLabel = True
            series.Font = New Font("Segoe UI", 10)
            series.LabelForeColor = Color.Black

            While cmdRead.Read()
                series.Points.AddXY(cmdRead("DeploymentStatus").ToString(), Convert.ToInt32(cmdRead("Count")))
            End While

            .Series.Add(series)
            .ChartAreas(0).BackColor = Color.White
            .ChartAreas(0).Area3DStyle.Enable3D = True
            .Titles.Clear()
            .Titles.Add("Deployment Status Summary")
            .Titles(0).Font = New Font("Segoe UI", 14, FontStyle.Bold)
        End With
        cmdRead.Close()
    End Sub

    Private Sub LoadDeploymentChart(Optional fromDate As Date = Nothing)
        Dim filter As String = If(fromDate <> Nothing, $" AND d.DeploymentDate >= '{fromDate:yyyy-MM-dd}'", "")
        Dim query As String = $"
            SELECT DATE_FORMAT(d.DeploymentDate, '%Y-%m') AS Month, COUNT(*) AS Count
            FROM deploymentrecord d
            JOIN jobplacement jp ON d.JobPlacementID = jp.JobPlacementID
            WHERE jp.AgencyID = {Session.CurrentReferenceID} {filter}
            GROUP BY Month
            ORDER BY Month"
        readQuery(query)

        With DeploymentPerMonth
            .Series.Clear()
            Dim series As New Series("Deployments")
            series.ChartType = SeriesChartType.SplineArea
            series.Color = Color.FromArgb(120, 72, 180, 255)
            series.BorderColor = Color.MediumBlue
            series.BorderWidth = 2
            series.IsValueShownAsLabel = True
            series.Font = New Font("Segoe UI", 10)

            While cmdRead.Read()
                series.Points.AddXY(cmdRead("Month").ToString(), Convert.ToInt32(cmdRead("Count")))
            End While

            .Series.Add(series)
            .ChartAreas(0).BackColor = Color.White
            .ChartAreas(0).AxisX.LabelStyle.Font = New Font("Segoe UI", 9)
            .ChartAreas(0).AxisY.LabelStyle.Font = New Font("Segoe UI", 9)
            .Titles.Clear()
            .Titles.Add("Deployments per Month")
            .Titles(0).Font = New Font("Segoe UI", 14, FontStyle.Bold)
        End With
        cmdRead.Close()
    End Sub

    ' === NAVIGATION ===
    Private Sub EditProfile_Click(sender As Object, e As EventArgs) Handles EditProfile.Click
        Dim form As New editAgency()
        form.ShowDialog()
    End Sub

    Private Sub Logout_Click(sender As Object, e As EventArgs) Handles Logout.Click
        Dim loginForm As New loginPage()
        loginForm.Show()
        Me.Close()
    End Sub

    Private Sub btnDashboard_Click(sender As Object, e As EventArgs) Handles btnDashboard.Click
        LoadSummary()
        LoadDeploymentChart()
        LoadDeploymentStatusDonut()
    End Sub

    Private Sub btnJobs_Click(sender As Object, e As EventArgs) Handles btnJobs.Click
        Dim form As New agcJobs()
        form.Show()
    End Sub

    Private Sub btnApplications_Click(sender As Object, e As EventArgs) Handles btnApplications.Click
        Dim form As New agcApplications()
        form.Show()
        Me.Hide()
    End Sub

    Private Sub btnDeployments_Click(sender As Object, e As EventArgs) Handles btnDeployments.Click
        Dim form As New agcDeployment()
        form.Show()
        Me.Hide()
    End Sub

    Private Sub btnOfws_Click(sender As Object, e As EventArgs) Handles btnOfws.Click
        Dim form As New agcOfws()
        form.Show()
        Me.Hide()
    End Sub

    Private Sub btnEmployers_Click(sender As Object, e As EventArgs) Handles btnEmployers.Click
        Dim form As New agcEmployers()
        form.Show()
        Me.Hide()
    End Sub

    ' === FILTERS ===
    Private Sub TodayBTN_Click(sender As Object, e As EventArgs) Handles TodayBTN.Click
        Dim d As Date = Date.Today
        LoadSummary()
        LoadDeploymentChart(d)
        LoadDeploymentStatusDonut(d, d)
    End Sub

    Private Sub LastSvnDaysBTN_Click(sender As Object, e As EventArgs) Handles LastSvnDaysBTN.Click
        Dim d As Date = Date.Today
        LoadSummary()
        LoadDeploymentChart(d.AddDays(-7))
        LoadDeploymentStatusDonut(d.AddDays(-7), d)
    End Sub

    Private Sub LastMonthBTN_Click(sender As Object, e As EventArgs) Handles LastMonthBTN.Click
        Dim today As Date = Date.Today
        Dim startCurrentMonth As New Date(today.Year, today.Month, 1)
        Dim startLastMonth As Date = startCurrentMonth.AddMonths(-1)
        Dim endLastMonth As Date = startCurrentMonth.AddDays(-1)

        LoadSummary()
        LoadDeploymentChart(startLastMonth)
        LoadDeploymentStatusDonut(startLastMonth, endLastMonth)
    End Sub

    Private Sub LastYearBTN_Click(sender As Object, e As EventArgs) Handles LastYearBTN.Click
        Dim today As Date = Date.Today
        Dim startCurrentYear As New Date(today.Year, 1, 1)
        Dim startLastYear As Date = startCurrentYear.AddYears(-1)
        Dim endLastYear As Date = startCurrentYear.AddDays(-1)

        LoadSummary()
        LoadDeploymentChart(startLastYear)
        LoadDeploymentStatusDonut(startLastYear, endLastYear)
    End Sub

    Private Sub refreshBtn_Click(sender As Object, e As EventArgs) Handles refreshBtn.Click
        LoadSummary()
        LoadDeploymentChart()
        LoadDeploymentStatusDonut()
    End Sub

    Private Sub dgvPartnerEmployers_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvPartnerEmployers.CellContentClick
        If e.RowIndex >= 0 Then
            Dim form As New agcEmployers()
            form.Show()
            Me.Hide()
        End If
    End Sub
End Class
