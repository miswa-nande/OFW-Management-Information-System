Imports System.Drawing
Imports System.Windows.Forms.DataVisualization.Charting

Public Class empDashboard
    Private Sub empDashboard_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.WindowState = FormWindowState.Maximized
        LoadSummary()
        LoadTopJobApplicationsPie()
        LoadDeploymentStatusChart()
        LoadPartneredAgencies() ' <<< Load agency list
    End Sub

    ' === Load Employer Summary Info ===
    Private Sub LoadSummary()
        ' Employer Info
        Dim query As String = $"SELECT * FROM employer WHERE EmployerID = {Session.CurrentReferenceID}"
        readQuery(query)
        If cmdRead.Read() Then
            lblIDNUM.Text = cmdRead("EmployerID").ToString()
            lblFullName.Text = $"{cmdRead("EmployerFirstName")} {cmdRead("EmployerLastName")}"
            lblCompanyName.Text = cmdRead("CompanyName").ToString()
            lblIndustry.Text = cmdRead("Industry").ToString()
            lblEmail.Text = cmdRead("EmployerEmail").ToString()
            lblContactNum.Text = cmdRead("EmployerContactNum").ToString()
            lblFullAddress.Text = $"{cmdRead("CompanyStreet")}, {cmdRead("CompanyCity")}, {cmdRead("CompanyState")}, {cmdRead("CompanyCountry")} {cmdRead("CompanyZipcode")}"
        End If
        cmdRead.Close()

        ' Total Jobs
        readQuery($"SELECT COUNT(*) FROM jobplacement WHERE EmployerID = {Session.CurrentReferenceID}")
        If cmdRead.Read() Then lblNumJobPosted.Text = cmdRead(0).ToString()
        cmdRead.Close()

        ' Total Agencies
        readQuery($"SELECT COUNT(DISTINCT AgencyID) FROM agencypartneremployer WHERE EmployerID = {Session.CurrentReferenceID}")
        If cmdRead.Read() Then lblNumAgencies.Text = cmdRead(0).ToString()
        cmdRead.Close()

        ' Total OFWs
        Dim ofwQuery As String = $"
            SELECT COUNT(DISTINCT a.OFWId)
            FROM application a
            JOIN jobplacement jp ON a.JobPlacementID = jp.JobPlacementID
            WHERE jp.EmployerID = {Session.CurrentReferenceID}"
        readQuery(ofwQuery)
        If cmdRead.Read() Then lblNumOfw.Text = cmdRead(0).ToString()
        cmdRead.Close()
    End Sub

    ' === CHART 2: Top 5 Jobs by Applications (Pie) ===
    Private Sub LoadTopJobApplicationsPie(Optional fromDate As Date = Nothing)
        Dim filter As String = If(fromDate <> Nothing, $" AND jp.PostingDate >= '{fromDate:yyyy-MM-dd}'", "")
        Dim query As String = $"
            SELECT jp.JobTitle, COUNT(app.ApplicationID) AS TotalApplications
            FROM jobplacement jp
            LEFT JOIN application app ON jp.JobPlacementID = app.JobPlacementID
            WHERE jp.EmployerID = {Session.CurrentReferenceID} {filter}
            GROUP BY jp.JobTitle
            ORDER BY TotalApplications DESC
            LIMIT 5"
        readQuery(query)

        With ChartTopJobApplications
            .Series.Clear()
            Dim series As New Series("Top Jobs")
            series.ChartType = SeriesChartType.Pie
            series.IsValueShownAsLabel = True
            series.Font = New Font("Segoe UI", 10)
            series("PieLabelStyle") = "Outside"
            series.BorderColor = Color.White
            series.BorderWidth = 1

            While cmdRead.Read()
                series.Points.AddXY(cmdRead("JobTitle").ToString(), Convert.ToInt32(cmdRead("TotalApplications")))
            End While

            .Series.Add(series)
            .ChartAreas(0).Area3DStyle.Enable3D = True
            .Titles.Clear()
            .Titles.Add("Top 5 Jobs by Application Volume")
            .Titles(0).Font = New Font("Segoe UI", 14, FontStyle.Bold)
        End With
        cmdRead.Close()
    End Sub

    ' === CHART 3: Deployment Status (Line Chart) ===
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
            .ChartAreas(0).Area3DStyle.Enable3D = False ' Disable 3D for line chart

            Dim series As New Series("Deployment Status")
            series.ChartType = SeriesChartType.Line
            series.BorderWidth = 3
            series.Color = Color.MediumBlue
            series.MarkerStyle = MarkerStyle.Circle
            series.MarkerSize = 8
            series.Font = New Font("Segoe UI", 10)

            While cmdRead.Read()
                series.Points.AddXY(cmdRead("DeploymentStatus").ToString(), Convert.ToInt32(cmdRead("count")))
            End While

            .Series.Add(series)
            .Titles.Clear()
            .Titles.Add("Deployment Status by Category")
            .Titles(0).Font = New Font("Segoe UI", 14, FontStyle.Bold)

            ' Optional: Format Axis
            With .ChartAreas(0).AxisX
                .Title = "Deployment Status"
                .TitleFont = New Font("Segoe UI", 12, FontStyle.Bold)
                .LabelStyle.Font = New Font("Segoe UI", 10)
            End With

            With .ChartAreas(0).AxisY
                .Title = "No. of Records"
                .TitleFont = New Font("Segoe UI", 12, FontStyle.Bold)
                .LabelStyle.Font = New Font("Segoe UI", 10)
            End With
        End With
        cmdRead.Close()
    End Sub


    ' === NEW: Partnered Agencies DGV ===
    Private Sub LoadPartneredAgencies()
        Dim query As String = $"
    SELECT 
        a.AgencyName AS 'Agency Name',
        ape.Status AS 'Partnership Status',
        ape.DateStablished AS 'Date Partnered'
    FROM agencypartneremployer ape
    JOIN agency a ON ape.AgencyID = a.AgencyID
    WHERE ape.EmployerID = {Session.CurrentReferenceID}
    ORDER BY ape.DateStablished DESC"
        readQuery(query)


        Dim dt As New DataTable()
        dt.Load(cmdRead)
        cmdRead.Close()

        PartneredAgencies.DataSource = dt
        FormatDGVUniformly(PartneredAgencies)

        ' Optional: Adjust width
        If PartneredAgencies.Columns.Contains("Agency Name") Then
            PartneredAgencies.Columns("Agency Name").Width = 300
        End If
        If PartneredAgencies.Columns.Contains("Date Partnered") Then
            PartneredAgencies.Columns("Date Partnered").Width = 150
        End If
    End Sub

    ' === Format Helper ===
    Private Sub FormatDGVUniformly(dgv As DataGridView)
        With dgv
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
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
        End With
    End Sub

    ' === Filter Buttons ===
    Private Sub TodayBTN_Click(sender As Object, e As EventArgs) Handles TodayBTN.Click
        Dim d As Date = Date.Today
        LoadSummary()
        LoadTopJobApplicationsPie(d)
        LoadDeploymentStatusChart(d, d)
        LoadPartneredAgencies()
    End Sub

    Private Sub LastSvnDaysBTN_Click(sender As Object, e As EventArgs) Handles LastSvnDaysBTN.Click
        Dim d As Date = Date.Today
        LoadSummary()
        LoadTopJobApplicationsPie(d.AddDays(-7))
        LoadDeploymentStatusChart(d.AddDays(-7), d)
        LoadPartneredAgencies()
    End Sub

    Private Sub LastMonthBTN_Click(sender As Object, e As EventArgs) Handles LastMonthBTN.Click
        Dim today As Date = Date.Today
        Dim startCurrentMonth As New Date(today.Year, today.Month, 1)
        Dim startLastMonth As Date = startCurrentMonth.AddMonths(-1)
        Dim endLastMonth As Date = startCurrentMonth.AddDays(-1)

        LoadSummary()
        LoadTopJobApplicationsPie(startLastMonth)
        LoadDeploymentStatusChart(startLastMonth, endLastMonth)
        LoadPartneredAgencies()
    End Sub

    Private Sub LastYearButton_Click(sender As Object, e As EventArgs) Handles LastYearButton.Click
        Dim today As Date = Date.Today
        Dim startCurrentYear As New Date(today.Year, 1, 1)
        Dim startLastYear As Date = startCurrentYear.AddYears(-1)
        Dim endLastYear As Date = startCurrentYear.AddDays(-1)

        LoadSummary()
        LoadTopJobApplicationsPie(startLastYear)
        LoadDeploymentStatusChart(startLastYear, endLastYear)
        LoadPartneredAgencies()
    End Sub

    Private Sub refreshBtn_Click(sender As Object, e As EventArgs) Handles refreshBtn.Click
        LoadSummary()
        LoadTopJobApplicationsPie()
        LoadDeploymentStatusChart()
        LoadPartneredAgencies()
    End Sub

    ' === Navigation ===
    Private Sub EditProfile_Click(sender As Object, e As EventArgs) Handles EditProfile.Click
        Dim form As New editEmployer()
        form.ShowDialog()
    End Sub

    Private Sub Logout_Click(sender As Object, e As EventArgs) Handles Logout.Click
        Dim loginForm As New loginPage()
        loginForm.Show()
        Me.Close()
    End Sub

    Private Sub btnDashboard_Click(sender As Object, e As EventArgs) Handles btnDashboard.Click
        LoadSummary()
        LoadTopJobApplicationsPie()
        LoadDeploymentStatusChart()
        LoadPartneredAgencies()
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

    Private Sub PartneredAgencies_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles PartneredAgencies.CellContentClick
        If e.RowIndex >= 0 Then
            Dim agencyForm As New empAgencies()
            agencyForm.Show()
            Me.Hide()
        End If
    End Sub
End Class
