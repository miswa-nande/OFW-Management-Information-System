Imports System.Drawing
Imports System.Windows.Forms.DataVisualization.Charting

Public Class agcDashboard
    Private Sub agcDashboard_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadSummary()
        LoadDeploymentChart()
        LoadDeploymentStatusPie()
        Me.WindowState = FormWindowState.Maximized
    End Sub

    Private Sub LoadSummary()
        ' Load agency profile details
        readQuery($"SELECT * FROM agency WHERE agency_id = {Session.CurrentReferenceID}")
        If cmdRead.Read() Then
            lblIDNum.Text = Session.CurrentReferenceID.ToString()
            lblAgencyName.Text = cmdRead("AgencyName").ToString()
            lblLicenseNum.Text = cmdRead("LicenseNumber").ToString()
            lblEmail.Text = cmdRead("Email").ToString()
            lblContactNum.Text = cmdRead("ContactNumber").ToString()
            lblSpecialization.Text = cmdRead("Specialization").ToString()
            lblFullAddress.Text = $"{cmdRead("Street")}, {cmdRead("City")}, {cmdRead("Province")}, {cmdRead("ZipCode")}"
        End If
        cmdRead.Close()

        ' Load summary statistics
        lblNumJobPosted.Text = LoadToDGV($"SELECT * FROM jobplacement WHERE agency_id = {Session.CurrentReferenceID}", New DataGridView())
        TotalApplicationsReceived.Text = LoadToDGV($"SELECT * FROM application a JOIN jobplacement jp ON a.job_id = jp.job_id WHERE jp.agency_id = {Session.CurrentReferenceID}", New DataGridView())
        lblNumOfw.Text = LoadToDGV($"SELECT * FROM ofw WHERE agencyID = {Session.CurrentReferenceID}", New DataGridView())
        lblNumEmployers.Text = LoadToDGV($"SELECT DISTINCT e.* FROM employer e JOIN agencypartners ap ON e.employer_id = ap.employer_id WHERE ap.agency_id = {Session.CurrentReferenceID}", New DataGridView())

        LoadToDGV($"SELECT e.employer_id, e.CompanyName, e.Email, e.ContactNumber FROM employer e JOIN agencypartners ap ON e.employer_id = ap.employer_id WHERE ap.agency_id = {Session.CurrentReferenceID}", dgvPartnerEmployers)
    End Sub

    Private Sub LoadDeploymentStatusPie(Optional fromDate As Date = Nothing, Optional toDate As Date = Nothing)
        Dim filter As String = If(fromDate <> Nothing AndAlso toDate <> Nothing, $" AND deployment_date BETWEEN '{fromDate:yyyy-MM-dd}' AND '{toDate:yyyy-MM-dd}'", "")
        Dim query As String = $"
            SELECT status, COUNT(*) AS count
            FROM deploymentrecord d
            JOIN jobplacement jp ON d.job_id = jp.job_id
            WHERE jp.agency_id = {Session.CurrentReferenceID} {filter}
            GROUP BY status"
        readQuery(query)

        With DeploymentStatus
            .Series.Clear()
            Dim pieSeries As New Series("Status")
            pieSeries.ChartType = SeriesChartType.Pie
            pieSeries.Font = New Font("Segoe UI", 10, FontStyle.Bold)
            pieSeries.LabelForeColor = Color.White
            pieSeries.IsValueShownAsLabel = True
            pieSeries("PieLabelStyle") = "Outside"
            pieSeries.BorderWidth = 1
            pieSeries.BorderColor = Color.White

            While cmdRead.Read()
                pieSeries.Points.AddXY(cmdRead("status").ToString(), Convert.ToInt32(cmdRead("count")))
            End While
            .Series.Add(pieSeries)
            .ChartAreas(0).Area3DStyle.Enable3D = True
            .Titles.Clear()
            .Titles.Add("Deployment Status")
            .Titles(0).Font = New Font("Segoe UI", 14, FontStyle.Bold)
        End With
        cmdRead.Close()
    End Sub

    Private Sub LoadDeploymentChart(Optional fromDate As Date = Nothing)
        Dim filter As String = If(fromDate <> Nothing, $" AND deployment_date >= '{fromDate:yyyy-MM-dd}'", "")
        Dim query As String = $"
            SELECT DATE_FORMAT(deployment_date, '%Y-%m') AS Month, COUNT(*) AS Count
            FROM deploymentrecord d
            JOIN jobplacement jp ON d.job_id = jp.job_id
            WHERE jp.agency_id = {Session.CurrentReferenceID} {filter}
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
            series.LabelForeColor = Color.Black

            While cmdRead.Read()
                series.Points.AddXY(cmdRead("Month").ToString(), Convert.ToInt32(cmdRead("Count")))
            End While
            .Series.Add(series)
            .Titles.Clear()
            .Titles.Add("Deployments per Month")
            .Titles(0).Font = New Font("Segoe UI", 14, FontStyle.Bold)
            .ChartAreas(0).BackColor = Color.White
            .ChartAreas(0).AxisX.LabelStyle.Font = New Font("Segoe UI", 9)
            .ChartAreas(0).AxisY.LabelStyle.Font = New Font("Segoe UI", 9)
            .ChartAreas(0).AxisX.MajorGrid.LineColor = Color.LightGray
            .ChartAreas(0).AxisY.MajorGrid.LineColor = Color.LightGray
            .ChartAreas(0).AxisX.LineColor = Color.Gray
            .ChartAreas(0).AxisY.LineColor = Color.Gray
        End With
        cmdRead.Close()
    End Sub

    Private Sub EditProfile_Click(sender As Object, e As EventArgs) Handles EditProfile.Click
        Dim form As New editAgency()
        form.Show()
        Me.Hide()
    End Sub

    Private Sub Logout_Click(sender As Object, e As EventArgs) Handles Logout.Click
        Dim loginForm As New loginPage()
        loginForm.Show()
        Me.Close()
    End Sub

    Private Sub btnDashboard_Click(sender As Object, e As EventArgs) Handles btnDashboard.Click
        LoadSummary()
        LoadDeploymentChart()
        LoadDeploymentStatusPie()
    End Sub

    Private Sub btnJobs_Click(sender As Object, e As EventArgs) Handles btnJobs.Click
        Dim form As New agcJobs()
        form.Show()
        Me.Hide()
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

    Private Sub TodayBTN_Click(sender As Object, e As EventArgs) Handles TodayBTN.Click
        Dim d As Date = Date.Today
        LoadSummary()
        LoadDeploymentChart(d)
        LoadDeploymentStatusPie(d, d)
    End Sub

    Private Sub LastSvnDaysBTN_Click(sender As Object, e As EventArgs) Handles LastSvnDaysBTN.Click
        Dim d As Date = Date.Today
        LoadSummary()
        LoadDeploymentChart(d.AddDays(-7))
        LoadDeploymentStatusPie(d.AddDays(-7), d)
    End Sub

    Private Sub LastMonthBTN_Click(sender As Object, e As EventArgs) Handles LastMonthBTN.Click
        Dim today As Date = Date.Today
        Dim startCurrentMonth As New Date(today.Year, today.Month, 1)
        Dim startLastMonth As Date = startCurrentMonth.AddMonths(-1)
        Dim endLastMonth As Date = startCurrentMonth.AddDays(-1)

        LoadSummary()
        LoadDeploymentChart(startLastMonth)
        LoadDeploymentStatusPie(startLastMonth, endLastMonth)
    End Sub

    Private Sub LastYearBTN_Click(sender As Object, e As EventArgs) Handles LastYearBTN.Click
        Dim today As Date = Date.Today
        Dim startCurrentYear As New Date(today.Year, 1, 1)
        Dim startLastYear As Date = startCurrentYear.AddYears(-1)
        Dim endLastYear As Date = startCurrentYear.AddDays(-1)

        LoadSummary()
        LoadDeploymentChart(startLastYear)
        LoadDeploymentStatusPie(startLastYear, endLastYear)
    End Sub

    Private Sub refreshBtn_Click(sender As Object, e As EventArgs) Handles refreshBtn.Click
        LoadSummary()
        LoadDeploymentChart()
        LoadDeploymentStatusPie()
    End Sub
End Class
