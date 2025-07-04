﻿Imports System.Drawing
Imports System.Windows.Forms.DataVisualization.Charting

Public Class empDashboard
    Private Sub empDashboard_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadSummary()
        LoadApplicationsPerJobChart()
        LoadDeploymentStatusChart()
        Me.WindowState = FormWindowState.Maximized
    End Sub

    Private Sub LoadSummary()
        Dim query As String = $"SELECT * FROM employer WHERE EmployerID = {Session.CurrentReferenceID}"
        readQuery(query)

        If cmdRead.Read() Then
            lblIDNUM.Text = cmdRead("EmployerID").ToString()
            lblFullName.Text = cmdRead("EmployerFirstName").ToString() & " " & cmdRead("EmployerLastName").ToString()
            lblCompanyName.Text = cmdRead("CompanyName").ToString()
            lblIndustry.Text = cmdRead("Industry").ToString()
            lblEmail.Text = cmdRead("EmployerEmail").ToString()
            lblContactNum.Text = cmdRead("EmployerContactNum").ToString()
            lblFullAddress.Text = cmdRead("CompanyStreet") & ", " & cmdRead("CompanyCity") & ", " & cmdRead("CompanyState") & ", " & cmdRead("CompanyCountry") & " " & cmdRead("CompanyZipcode")
        End If
        cmdRead.Close()

        lblNumJobPosted.Text = LoadToDGV($"SELECT * FROM jobplacement WHERE EmployerID = {Session.CurrentReferenceID}", New DataGridView())
        ' There is no EmployerID in ofw table, so this query is not possible:
        ' lblNumOfw.Text = LoadToDGV($"SELECT * FROM ofw WHERE EmployerID = {Session.CurrentReferenceID}", New DataGridView())
        lblNumEmployers.Text = LoadToDGV($"SELECT DISTINCT a.* FROM agency a JOIN agencypartneremployer ap ON a.AgencyID = ap.AgencyID WHERE ap.EmployerID = {Session.CurrentReferenceID}", New DataGridView())
    End Sub

    Private Sub LoadApplicationsPerJobChart(Optional fromDate As Date = Nothing)
        Dim filter As String = If(fromDate <> Nothing, $" AND jp.PostingDate >= '{fromDate:yyyy-MM-dd}'", "")
        Dim query As String = $"SELECT jp.JobTitle, 0 AS TotalApplications FROM jobplacement jp WHERE jp.EmployerID = {Session.CurrentReferenceID} {filter} GROUP BY jp.JobTitle"
        readQuery(query)

        With ChartTopJobApplications
            .Series.Clear()
            Dim series As New Series("Applications")
            series.ChartType = SeriesChartType.Column
            series.Color = Color.MediumSeaGreen
            series.IsValueShownAsLabel = True
            series.Font = New Font("Segoe UI", 10)

            While cmdRead.Read()
                series.Points.AddXY(cmdRead("JobTitle").ToString(), Convert.ToInt32(cmdRead("TotalApplications")))
            End While
            .Series.Add(series)
            .Titles.Clear()
            .Titles.Add("Applications per Job Post")
            .Titles(0).Font = New Font("Segoe UI", 14, FontStyle.Bold)
        End With
        cmdRead.Close()
    End Sub

    Private Sub LoadDeploymentStatusChart(Optional fromDate As Date = Nothing, Optional toDate As Date = Nothing)
        Dim filter As String = If(fromDate <> Nothing AndAlso toDate <> Nothing, $" AND d.DeploymentDate BETWEEN '{fromDate:yyyy-MM-dd}' AND '{toDate:yyyy-MM-dd}'", "")
        Dim query As String = $"
            SELECT d.DeploymentStatus, COUNT(*) AS count
            FROM deploymentrecord d
            JOIN jobplacement jp ON d.JobPlacementID = jp.JobPlacementID
            WHERE jp.EmployerID = {Session.CurrentReferenceID} {filter}
            GROUP BY d.DeploymentStatus"
        readQuery(query)

        With DeploymentStatusChart
            .Series.Clear()
            Dim series As New Series("Status")
            series.ChartType = SeriesChartType.Pie
            series.IsValueShownAsLabel = True
            series.Font = New Font("Segoe UI", 10, FontStyle.Bold)
            series.LabelForeColor = Color.White
            series("PieLabelStyle") = "Outside"
            series.BorderWidth = 1
            series.BorderColor = Color.White

            While cmdRead.Read()
                series.Points.AddXY(cmdRead("DeploymentStatus").ToString(), Convert.ToInt32(cmdRead("count")))
            End While
            .Series.Add(series)
            .ChartAreas(0).Area3DStyle.Enable3D = True
            .Titles.Clear()
            .Titles.Add("Deployment Status")
            .Titles(0).Font = New Font("Segoe UI", 14, FontStyle.Bold)
        End With
        cmdRead.Close()
    End Sub

    ' === Filter Buttons ===
    Private Sub TodayBTN_Click(sender As Object, e As EventArgs) Handles TodayBTN.Click
        Dim d As Date = Date.Today
        LoadSummary()
        LoadApplicationsPerJobChart(d)
        LoadDeploymentStatusChart(d, d)
    End Sub

    Private Sub LastSvnDaysBTN_Click(sender As Object, e As EventArgs) Handles LastSvnDaysBTN.Click
        Dim d As Date = Date.Today
        LoadSummary()
        LoadApplicationsPerJobChart(d.AddDays(-7))
        LoadDeploymentStatusChart(d.AddDays(-7), d)
    End Sub

    Private Sub LastMonthBTN_Click(sender As Object, e As EventArgs) Handles LastMonthBTN.Click
        Dim today As Date = Date.Today
        Dim startCurrentMonth As New Date(today.Year, today.Month, 1)
        Dim startLastMonth As Date = startCurrentMonth.AddMonths(-1)
        Dim endLastMonth As Date = startCurrentMonth.AddDays(-1)

        LoadSummary()
        LoadApplicationsPerJobChart(startLastMonth)
        LoadDeploymentStatusChart(startLastMonth, endLastMonth)
    End Sub

    Private Sub LastYearButton_Click(sender As Object, e As EventArgs) Handles LastYearButton.Click
        Dim today As Date = Date.Today
        Dim startCurrentYear As New Date(today.Year, 1, 1)
        Dim startLastYear As Date = startCurrentYear.AddYears(-1)
        Dim endLastYear As Date = startCurrentYear.AddDays(-1)

        LoadSummary()
        LoadApplicationsPerJobChart(startLastYear)
        LoadDeploymentStatusChart(startLastYear, endLastYear)
    End Sub

    Private Sub refreshBtn_Click(sender As Object, e As EventArgs) Handles refreshBtn.Click
        LoadSummary()
        LoadApplicationsPerJobChart()
        LoadDeploymentStatusChart()
    End Sub

    ' === Navigation ===
    Private Sub EditProfile_Click(sender As Object, e As EventArgs) Handles EditProfile.Click
        Dim form As New editEmployer()
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
        LoadApplicationsPerJobChart()
        LoadDeploymentStatusChart()
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

    Private Sub btnOfws_Click(sender As Object, e As EventArgs) Handles btnOfws.Click
        Dim form As New empOfws()
        form.Show()
        Me.Hide()
    End Sub
End Class
