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

        ' Load all-time data
        LoadChartOFWWithFilter()
        LoadAgencyChartWithFilter()
        LoadEmployerChartWithFilter()
        LoadTopAgenciesChart()
    End Sub

    Private Sub LoadDashboardCounts()
        readQuery("SELECT COUNT(*) FROM ofw")
        If cmdRead.Read() Then lblNumOfw.Text = cmdRead.GetInt32(0).ToString()
        cmdRead.Close()

        readQuery("SELECT COUNT(*) FROM agency")
        If cmdRead.Read() Then lblNumAgencies.Text = cmdRead.GetInt32(0).ToString()
        cmdRead.Close()

        readQuery("SELECT COUNT(*) FROM employer")
        If cmdRead.Read() Then lblNumEmployers.Text = cmdRead.GetInt32(0).ToString()
        cmdRead.Close()

        readQuery("SELECT COUNT(*) FROM deploymentrecord")
        If cmdRead.Read() Then lblNumDeployed.Text = cmdRead.GetInt32(0).ToString()
        cmdRead.Close()
    End Sub

    Private Sub LoadChartOFWWithFilter(Optional startDate As DateTime = Nothing, Optional endDate As DateTime = Nothing)
        ChartOFW.Series.Clear()
        ChartOFW.Titles.Clear()
        ChartOFW.Titles.Add("OFW Registrations Over Time")

        Dim series As New DataVisualization.Charting.Series("OFWs Registered")
        series.ChartType = DataVisualization.Charting.SeriesChartType.SplineArea
        series.IsValueShownAsLabel = True

        Dim query As String = "SELECT DATE_FORMAT(DateAdded, '%Y-%m-%d') AS RegDay, COUNT(*) AS Total FROM ofw"

        If startDate <> DateTime.MinValue AndAlso endDate <> DateTime.MinValue Then
            query &= " WHERE DATE(DateAdded) BETWEEN '" & startDate.ToString("yyyy-MM-dd") & "' AND '" & endDate.ToString("yyyy-MM-dd") & "'"
        End If

        query &= " GROUP BY RegDay ORDER BY RegDay ASC"

        readQuery(query)
        While cmdRead.Read()
            series.Points.AddXY(cmdRead("RegDay").ToString(), Convert.ToInt32(cmdRead("Total")))
        End While
        cmdRead.Close()

        ChartOFW.Series.Add(series)
        With ChartOFW.ChartAreas(0)
            .AxisX.Title = "Date Registered"
            .AxisY.Title = "Number of OFWs"
            .AxisX.Interval = 1
            .Area3DStyle.Enable3D = False
        End With
    End Sub

    Private Sub LoadAgencyChartWithFilter(Optional startDate As DateTime = Nothing, Optional endDate As DateTime = Nothing)
        ChartAgencies.Series.Clear()
        ChartAgencies.Titles.Clear()
        ChartAgencies.Titles.Add("Agency Registrations Over Time")

        Dim series As New DataVisualization.Charting.Series("Agencies Registered")
        series.ChartType = DataVisualization.Charting.SeriesChartType.SplineArea
        series.IsValueShownAsLabel = True

        Dim query As String = "SELECT DATE_FORMAT(DateAdded, '%Y-%m-%d') AS RegDay, COUNT(*) AS Total FROM agency"

        If startDate <> DateTime.MinValue AndAlso endDate <> DateTime.MinValue Then
            query &= " WHERE DATE(DateAdded) BETWEEN '" & startDate.ToString("yyyy-MM-dd") & "' AND '" & endDate.ToString("yyyy-MM-dd") & "'"
        End If

        query &= " GROUP BY RegDay ORDER BY RegDay ASC"

        readQuery(query)
        While cmdRead.Read()
            series.Points.AddXY(cmdRead("RegDay").ToString(), Convert.ToInt32(cmdRead("Total")))
        End While
        cmdRead.Close()

        ChartAgencies.Series.Add(series)
        With ChartAgencies.ChartAreas(0)
            .AxisX.Title = "Date Registered"
            .AxisY.Title = "Number of Agencies"
            .AxisX.Interval = 1
            .AxisX.LabelStyle.Angle = -45
            .Area3DStyle.Enable3D = False
        End With
    End Sub

    Private Sub LoadEmployerChartWithFilter(Optional startDate As DateTime = Nothing, Optional endDate As DateTime = Nothing)
        ChartEmployers.Series.Clear()
        ChartEmployers.Titles.Clear()
        ChartEmployers.Titles.Add("Employer Registrations Over Time")

        Dim series As New DataVisualization.Charting.Series("Employers Registered")
        series.ChartType = DataVisualization.Charting.SeriesChartType.SplineArea
        series.IsValueShownAsLabel = True

        Dim query As String = "SELECT DATE_FORMAT(DateAdded, '%Y-%m-%d') AS RegDay, COUNT(*) AS Total FROM employer"

        If startDate <> DateTime.MinValue AndAlso endDate <> DateTime.MinValue Then
            query &= " WHERE DATE(DateAdded) BETWEEN '" & startDate.ToString("yyyy-MM-dd") & "' AND '" & endDate.ToString("yyyy-MM-dd") & "'"
        End If

        query &= " GROUP BY RegDay ORDER BY RegDay ASC"

        readQuery(query)
        While cmdRead.Read()
            series.Points.AddXY(cmdRead("RegDay").ToString(), Convert.ToInt32(cmdRead("Total")))
        End While
        cmdRead.Close()

        ChartEmployers.Series.Add(series)
        With ChartEmployers.ChartAreas(0)
            .AxisX.Title = "Date Registered"
            .AxisY.Title = "Number of Employers"
            .AxisX.Interval = 1
            .Area3DStyle.Enable3D = False
        End With
    End Sub

    Private Sub LoadTopAgenciesChart(Optional startDate As DateTime = Nothing, Optional endDate As DateTime = Nothing)
        ChartTopAgencies.Series.Clear()
        ChartTopAgencies.Titles.Clear()
        ChartTopAgencies.Titles.Add("Top 10 Agencies by Deployed Workers")

        Dim series As New DataVisualization.Charting.Series("Agencies")
        series.ChartType = DataVisualization.Charting.SeriesChartType.Pie
        series.IsValueShownAsLabel = True

        Dim query As String = "SELECT a.AgencyName, COUNT(*) AS TotalDeployed FROM deploymentrecord d JOIN agency a ON d.AgencyID = a.AgencyID"

        If startDate <> DateTime.MinValue AndAlso endDate <> DateTime.MinValue Then
            query &= " WHERE d.DeploymentDate BETWEEN '" & startDate.ToString("yyyy-MM-dd") & "' AND '" & endDate.ToString("yyyy-MM-dd") & "'"
        End If

        query &= " GROUP BY a.AgencyName ORDER BY TotalDeployed DESC LIMIT 10"

        readQuery(query)
        While cmdRead.Read()
            series.Points.AddXY(cmdRead("AgencyName").ToString(), Convert.ToInt32(cmdRead("TotalDeployed")))
        End While
        cmdRead.Close()

        ChartTopAgencies.Series.Add(series)
        With ChartTopAgencies.ChartAreas(0)
            .Area3DStyle.Enable3D = True
        End With
    End Sub

    ' Filter Buttons
    Private Sub TodayBTN_Click(sender As Object, e As EventArgs) Handles TodayBTN.Click
        Dim d As Date = DateTime.Today
        LoadChartOFWWithFilter(d, d)
        LoadAgencyChartWithFilter(d, d)
        LoadEmployerChartWithFilter(d, d)
        LoadTopAgenciesChart(d, d)
    End Sub

    Private Sub SvnDaysBTN_Click(sender As Object, e As EventArgs) Handles SvnDaysBTN.Click
        Dim endDate As Date = DateTime.Today
        Dim startDate As Date = endDate.AddDays(-6)
        LoadChartOFWWithFilter(startDate, endDate)
        LoadAgencyChartWithFilter(startDate, endDate)
        LoadEmployerChartWithFilter(startDate, endDate)
        LoadTopAgenciesChart(startDate, endDate)
    End Sub

    Private Sub OneMonthBTN_Click(sender As Object, e As EventArgs) Handles OneMonthBTN.Click
        Dim endDate As Date = DateTime.Today
        Dim startDate As Date = endDate.AddMonths(-1).AddDays(1)
        LoadChartOFWWithFilter(startDate, endDate)
        LoadAgencyChartWithFilter(startDate, endDate)
        LoadEmployerChartWithFilter(startDate, endDate)
        LoadTopAgenciesChart(startDate, endDate)
    End Sub

    Private Sub ThreeMonthBTN_Click(sender As Object, e As EventArgs) Handles ThreeMonthBTN.Click
        Dim endDate As Date = DateTime.Today
        Dim startDate As Date = endDate.AddMonths(-3).AddDays(1)
        LoadChartOFWWithFilter(startDate, endDate)
        LoadAgencyChartWithFilter(startDate, endDate)
        LoadEmployerChartWithFilter(startDate, endDate)
        LoadTopAgenciesChart(startDate, endDate)
    End Sub

    Private Sub refreshBtn_Click(sender As Object, e As EventArgs) Handles refreshBtn.Click
        LoadChartOFWWithFilter()
        LoadAgencyChartWithFilter()
        LoadEmployerChartWithFilter()
        LoadTopAgenciesChart()
    End Sub

    Private Sub CustomDate_ValueChanged(sender As Object, e As EventArgs) Handles CustomDate.ValueChanged
        Dim d As Date = CustomDate.Value.Date
        LoadChartOFWWithFilter(d, d)
        LoadAgencyChartWithFilter(d, d)
        LoadEmployerChartWithFilter(d, d)
        LoadTopAgenciesChart(d, d)
    End Sub

End Class