Imports System.IO
Imports System.Drawing
Imports System.Windows.Forms.DataVisualization.Charting

Public Class dashboard

    Private Sub btnOfw_Click(sender As Object, e As EventArgs) Handles btnOfw.Click
        Dim newForm As New ofws()
        newForm.Show()
        Me.Hide()
    End Sub

    Private Sub btnDeployments_Click(sender As Object, e As EventArgs) Handles btnDeployments.Click
        Dim newForm As New deployments()
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

    Private Sub dashboard_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadDeploymentsToDGV(Deployed)
        FormatDGVUniformly(Deployed)
        LoadDashboardCounts()
        LoadChartsByYear()
        LoadTopAgenciesChart()
        Me.WindowState = FormWindowState.Maximized
    End Sub

    Private Sub LoadDashboardCounts(Optional fromDate As Date = Nothing, Optional toDate As Date = Nothing)
        Dim filter As String = If(fromDate <> Nothing AndAlso toDate <> Nothing,
            $" WHERE DateAdded BETWEEN '{fromDate:yyyy-MM-dd}' AND '{toDate:yyyy-MM-dd}'", "")

        readQuery($"SELECT COUNT(*) FROM ofw{filter}")
        lblNumOfw.Text = If(cmdRead.Read(), cmdRead.GetInt32(0).ToString(), "0")
        cmdRead.Close()

        readQuery($"SELECT COUNT(*) FROM agency{filter}")
        lblNumAgencies.Text = If(cmdRead.Read(), cmdRead.GetInt32(0).ToString(), "0")
        cmdRead.Close()

        readQuery($"SELECT COUNT(*) FROM employer{filter}")
        lblNumEmployers.Text = If(cmdRead.Read(), cmdRead.GetInt32(0).ToString(), "0")
        cmdRead.Close()

        Dim deploymentFilter As String = If(fromDate <> Nothing AndAlso toDate <> Nothing,
            $" WHERE DeploymentDate BETWEEN '{fromDate:yyyy-MM-dd}' AND '{toDate:yyyy-MM-dd}'", "")
        readQuery($"SELECT COUNT(*) FROM deploymentrecord{deploymentFilter}")
        lblNumDeployed.Text = If(cmdRead.Read(), cmdRead.GetInt32(0).ToString(), "0")
        cmdRead.Close()
    End Sub

    Private Sub LoadChartsByYear()
        LoadChartYearly("ofw", "OFW Registrations", ChartOFW)
        LoadChartYearly("agency", "Agency Registrations", ChartAgencies)
        LoadChartYearly("employer", "Employer Registrations", ChartEmployers)
    End Sub

    Private Sub LoadChartYearly(tableName As String, title As String, chart As Chart)
        chart.Series.Clear()
        chart.Titles.Clear()
        chart.Titles.Add(title)
        chart.Titles(0).Font = New Font("Segoe UI", 14, FontStyle.Bold)
        chart.Titles(0).ForeColor = Color.DarkSlateGray

        Dim series As New Series("Total")
        series.ChartType = SeriesChartType.SplineArea
        series.Color = Color.FromArgb(120, 72, 180, 255)
        series.BorderColor = Color.MediumBlue
        series.BorderWidth = 2
        series.IsValueShownAsLabel = True
        series.Font = New Font("Segoe UI", 10)
        series.LabelForeColor = Color.Black

        readQuery($"SELECT YEAR(DateAdded) AS RegYear, COUNT(*) AS Total FROM {tableName} GROUP BY RegYear ORDER BY RegYear")
        While cmdRead.Read()
            series.Points.AddXY(cmdRead("RegYear").ToString(), Convert.ToInt32(cmdRead("Total")))
        End While
        cmdRead.Close()

        chart.Series.Add(series)
        StyleChart(chart)
    End Sub

    Private Sub LoadChartRangeComparison(tableName As String, title As String, chart As Chart,
                                         label1 As String, from1 As Date, to1 As Date,
                                         label2 As String, from2 As Date, to2 As Date)

        chart.Series.Clear()
        chart.Titles.Clear()
        chart.Titles.Add(title)
        chart.Titles(0).Font = New Font("Segoe UI", 14, FontStyle.Bold)
        chart.Titles(0).ForeColor = Color.DarkSlateGray

        Dim series1 As New Series(label1)
        Dim series2 As New Series(label2)
        series1.ChartType = SeriesChartType.Line
        series2.ChartType = SeriesChartType.Line

        series1.BorderWidth = 3
        series2.BorderWidth = 3
        series1.Color = Color.FromArgb(120, 50, 200, 100)
        series2.Color = Color.FromArgb(120, 200, 100, 50)
        series1.MarkerStyle = MarkerStyle.Circle
        series2.MarkerStyle = MarkerStyle.Circle
        series1.MarkerSize = 8
        series2.MarkerSize = 8

        series1.IsValueShownAsLabel = True
        series2.IsValueShownAsLabel = True
        series1.Font = New Font("Segoe UI", 10)
        series2.Font = New Font("Segoe UI", 10)

        readQuery($"SELECT COUNT(*) AS Total FROM {tableName} WHERE DateAdded BETWEEN '{from1:yyyy-MM-dd}' AND '{to1:yyyy-MM-dd}'")
        series1.Points.AddXY(label1, If(cmdRead.Read(), Convert.ToInt32(cmdRead("Total")), 0))
        cmdRead.Close()

        readQuery($"SELECT COUNT(*) AS Total FROM {tableName} WHERE DateAdded BETWEEN '{from2:yyyy-MM-dd}' AND '{to2:yyyy-MM-dd}'")
        series2.Points.AddXY(label2, If(cmdRead.Read(), Convert.ToInt32(cmdRead("Total")), 0))
        cmdRead.Close()

        chart.Series.Add(series1)
        chart.Series.Add(series2)
        StyleChart(chart)
    End Sub

    Private Sub LoadTopAgenciesComparison(fromDate As Date, toDate As Date)
        ChartTopAgencies.Series.Clear()
        ChartTopAgencies.Titles.Clear()
        ChartTopAgencies.Titles.Add($"Top Agencies: {fromDate:yyyy-MM-dd} to {toDate:yyyy-MM-dd}")
        ChartTopAgencies.Titles(0).Font = New Font("Segoe UI", 14, FontStyle.Bold)

        Dim series As New Series("Top Agencies")
        series.ChartType = SeriesChartType.Pie
        series.Palette = ChartColorPalette.SeaGreen
        series.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        series.LabelForeColor = Color.White
        series.IsValueShownAsLabel = True
        series("PieLabelStyle") = "Outside"
        series.BorderWidth = 1
        series.BorderColor = Color.White

        readQuery($"SELECT a.AgencyName, COUNT(*) AS TotalDeployed FROM deploymentrecord d JOIN agency a ON d.AgencyID = a.AgencyID WHERE d.DeploymentDate BETWEEN '{fromDate:yyyy-MM-dd}' AND '{toDate:yyyy-MM-dd}' GROUP BY a.AgencyName ORDER BY TotalDeployed DESC LIMIT 10")
        While cmdRead.Read()
            series.Points.AddXY(cmdRead("AgencyName").ToString(), Convert.ToInt32(cmdRead("TotalDeployed")))
        End While
        cmdRead.Close()

        ChartTopAgencies.Series.Add(series)
        ChartTopAgencies.ChartAreas(0).Area3DStyle.Enable3D = True
        StyleChart(ChartTopAgencies)
    End Sub

    Private Sub LoadTopAgenciesChart()
        ChartTopAgencies.Series.Clear()
        ChartTopAgencies.Titles.Clear()
        ChartTopAgencies.Titles.Add("Top 10 Agencies by Deployed Workers")
        ChartTopAgencies.Titles(0).Font = New Font("Segoe UI", 14, FontStyle.Bold)

        Dim series As New Series("Agencies")
        series.ChartType = SeriesChartType.Pie
        series.Palette = ChartColorPalette.SeaGreen
        series.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        series.LabelForeColor = Color.White
        series.IsValueShownAsLabel = True
        series("PieLabelStyle") = "Outside"
        series.BorderWidth = 1
        series.BorderColor = Color.White

        readQuery("SELECT a.AgencyName, COUNT(*) AS TotalDeployed FROM deploymentrecord d JOIN agency a ON d.AgencyID = a.AgencyID GROUP BY a.AgencyName ORDER BY TotalDeployed DESC LIMIT 10")
        While cmdRead.Read()
            series.Points.AddXY(cmdRead("AgencyName").ToString(), Convert.ToInt32(cmdRead("TotalDeployed")))
        End While
        cmdRead.Close()

        ChartTopAgencies.Series.Add(series)
        ChartTopAgencies.ChartAreas(0).Area3DStyle.Enable3D = True
        StyleChart(ChartTopAgencies)
    End Sub

    Private Sub StyleChart(chart As Chart)
        With chart.ChartAreas(0)
            .BackColor = Color.White
            .AxisX.LabelStyle.Font = New Font("Segoe UI", 9)
            .AxisY.LabelStyle.Font = New Font("Segoe UI", 9)
            .AxisX.MajorGrid.LineColor = Color.LightGray
            .AxisY.MajorGrid.LineColor = Color.LightGray
            .AxisX.LineColor = Color.Gray
            .AxisY.LineColor = Color.Gray
        End With
        chart.AntiAliasing = AntiAliasingStyles.All
    End Sub

    ' === Filter Buttons ===
    Private Sub TodayBTN_Click(sender As Object, e As EventArgs) Handles TodayBTN.Click
        Dim d As Date = Date.Today
        LoadDashboardCounts(d.AddDays(-1), d)
        LoadChartRangeComparison("ofw", "OFW Registrations", ChartOFW, "Yesterday", d.AddDays(-1), d.AddDays(-1), "Today", d, d)
        LoadChartRangeComparison("agency", "Agency Registrations", ChartAgencies, "Yesterday", d.AddDays(-1), d.AddDays(-1), "Today", d, d)
        LoadChartRangeComparison("employer", "Employer Registrations", ChartEmployers, "Yesterday", d.AddDays(-1), d.AddDays(-1), "Today", d, d)
        LoadTopAgenciesComparison(d.AddDays(-1), d)
    End Sub

    Private Sub SvnDaysBTN_Click(sender As Object, e As EventArgs) Handles SvnDaysBTN.Click
        Dim d As Date = Date.Today
        LoadDashboardCounts(d.AddDays(-7), d)
        LoadChartRangeComparison("ofw", "OFW Registrations", ChartOFW, "Last 7 Days", d.AddDays(-7), d.AddDays(-1), "Today", d, d)
        LoadChartRangeComparison("agency", "Agency Registrations", ChartAgencies, "Last 7 Days", d.AddDays(-7), d.AddDays(-1), "Today", d, d)
        LoadChartRangeComparison("employer", "Employer Registrations", ChartEmployers, "Last 7 Days", d.AddDays(-7), d.AddDays(-1), "Today", d, d)
        LoadTopAgenciesComparison(d.AddDays(-7), d)
    End Sub

    Private Sub OneMonthBTN_Click(sender As Object, e As EventArgs) Handles OneMonthBTN.Click
        Dim today As Date = Date.Today
        Dim startCurrentMonth As New Date(today.Year, today.Month, 1)
        Dim startLastMonth As Date = startCurrentMonth.AddMonths(-1)
        Dim endLastMonth As Date = startCurrentMonth.AddDays(-1)

        LoadDashboardCounts(startLastMonth, today)
        LoadChartRangeComparison("ofw", "OFW Registrations", ChartOFW, "Last Month", startLastMonth, endLastMonth, "Current Month", startCurrentMonth, today)
        LoadChartRangeComparison("agency", "Agency Registrations", ChartAgencies, "Last Month", startLastMonth, endLastMonth, "Current Month", startCurrentMonth, today)
        LoadChartRangeComparison("employer", "Employer Registrations", ChartEmployers, "Last Month", startLastMonth, endLastMonth, "Current Month", startCurrentMonth, today)
        LoadTopAgenciesComparison(startLastMonth, today)
    End Sub

    Private Sub OneYearBTN_Click(sender As Object, e As EventArgs) Handles OneYearBTN.Click
        Dim today As Date = Date.Today
        Dim startCurrentYear As New Date(today.Year, 1, 1)
        Dim startLastYear As Date = startCurrentYear.AddYears(-1)
        Dim endLastYear As Date = startCurrentYear.AddDays(-1)

        LoadDashboardCounts(startLastYear, today)
        LoadChartRangeComparison("ofw", "OFW Registrations", ChartOFW, "Last Year", startLastYear, endLastYear, "Current Year", startCurrentYear, today)
        LoadChartRangeComparison("agency", "Agency Registrations", ChartAgencies, "Last Year", startLastYear, endLastYear, "Current Year", startCurrentYear, today)
        LoadChartRangeComparison("employer", "Employer Registrations", ChartEmployers, "Last Year", startLastYear, endLastYear, "Current Year", startCurrentYear, today)
        LoadTopAgenciesComparison(startLastYear, today)
    End Sub

    Private Sub refreshBtn_Click(sender As Object, e As EventArgs) Handles refreshBtn.Click
        LoadDashboardCounts()
        LoadChartsByYear()
        LoadTopAgenciesChart()
    End Sub

    Private Sub CustomDate_ValueChanged(sender As Object, e As EventArgs) Handles CustomDate.ValueChanged
        Dim fromDate As Date = CustomDate.Value
        Dim toDate As Date = Date.Today
        LoadDashboardCounts(fromDate, toDate)
        LoadChartRangeComparison("ofw", "OFW Registrations", ChartOFW, "Custom", fromDate, toDate.AddDays(-1), "Today", toDate, toDate)
        LoadChartRangeComparison("agency", "Agency Registrations", ChartAgencies, "Custom", fromDate, toDate.AddDays(-1), "Today", toDate, toDate)
        LoadChartRangeComparison("employer", "Employer Registrations", ChartEmployers, "Custom", fromDate, toDate.AddDays(-1), "Today", toDate, toDate)
        LoadTopAgenciesComparison(fromDate, toDate)
    End Sub

    Private Sub btnConfig_Click(sender As Object, e As EventArgs) Handles btnConfig.Click
        Dim newForm As New AdminConfiguration()
        newForm.Show()
        Me.Hide()
    End Sub

End Class
