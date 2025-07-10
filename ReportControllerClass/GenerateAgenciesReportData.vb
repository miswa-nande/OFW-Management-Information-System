Imports System.IO
Imports System.Data
Imports System.Windows.Forms.DataVisualization.Charting
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports DrawingFont = System.Drawing.Font
Imports DrawingImage = System.Drawing.Image
Imports DrawingBrushes = System.Drawing.Brushes
Imports DrawingGraphics = System.Drawing.Graphics
Imports DrawingBitmap = System.Drawing.Bitmap
Imports DrawingColor = System.Drawing.Color

Public Class GenerateAgenciesReportData
    Public Function GenerateReport() As Boolean
        Try
            Dim query As String = "
                SELECT 
                    a.AgencyID,
                    a.AgencyName,
                    a.AgencyLicenseNumber,
                    CONCAT(a.Street, ', ', a.City, ', ', a.State, ' ', a.Zipcode) AS Address,
                    a.ContactNum,
                    a.Email,
                    a.Specialization,
                    a.YearsOfOperation,
                    a.GovAccreditationStat,
                    a.LicenseExpDate,
                    a.City,
                    a.State,
                    COUNT(DISTINCT ape.EmployerID) AS Partnerships,
                    COUNT(DISTINCT dr.DeploymentID) AS TotalDeployments,
                    COUNT(DISTINCT jp.JobPlacementID) AS ActiveJobs,
                    a.DateAdded
                FROM agency a
                LEFT JOIN agencypartneremployer ape ON a.AgencyID = ape.AgencyID
                LEFT JOIN deploymentrecord dr ON a.AgencyID = dr.AgencyID
                LEFT JOIN jobplacement jp ON a.AgencyID = jp.AgencyID AND jp.JobStatus = 'Open'
                GROUP BY a.AgencyID
                ORDER BY a.DateAdded DESC"

            readQuery(query)
            Dim dt As New DataTable()
            dt.Load(cmdRead)

            Dim fileName As String = "AgenciesReport-" & DateTime.Now.ToString("yyyyMMdd") & ".pdf"
            Dim desktopPath As String = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
            Dim filePath As String = Path.Combine(desktopPath, fileName)

            Dim doc As New Document(PageSize.A4.Rotate(), 25, 25, 40, 40)
            Dim fs As New FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None)
            Dim writer As PdfWriter = PdfWriter.GetInstance(doc, fs)
            writer.PageEvent = New FooterEvent()

            doc.Open()

            ' Header with Logo
            Try
                Dim logoPath As String = Path.Combine(Application.StartupPath, "Resources", "logoM.png")
                If File.Exists(logoPath) Then
                    Dim logo = iTextSharp.text.Image.GetInstance(logoPath)
                    logo.Alignment = Element.ALIGN_CENTER
                    logo.ScaleToFit(80.0F, 80.0F)
                    doc.Add(logo)
                    doc.Add(New Paragraph(Environment.NewLine))
                End If
            Catch ex As Exception
                ' If logo loading fails, continue without it
            End Try

            ' Title
            Dim titleFont = iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 14)

            Dim titleParagraph1 As New Paragraph("OFW Management Information System", titleFont)
            titleParagraph1.Alignment = Element.ALIGN_CENTER
            doc.Add(titleParagraph1)

            Dim titleParagraph2 As New Paragraph("Agencies Comprehensive Report", titleFont)
            titleParagraph2.Alignment = Element.ALIGN_CENTER
            doc.Add(titleParagraph2)

            doc.Add(New Paragraph(Environment.NewLine))

            ' Agencies Table
            AddAgencyDataTable(doc, dt)
            doc.NewPage()

            ' Summary section
            doc.Add(New Paragraph("Summary", titleFont))
            doc.Add(New Paragraph(Environment.NewLine))

            ' Accreditation Status Distribution (One Page)
            Dim accredHeader As New Paragraph("Accreditation Status Distribution", titleFont)
            accredHeader.Alignment = Element.ALIGN_CENTER
            accredHeader.SpacingBefore = 20
            doc.Add(accredHeader)
            AddChartToPdf(doc, dt, "GovAccreditationStat")
            doc.NewPage()

            ' Specialization Distribution (One Page)
            Dim specializationHeader As New Paragraph("Top Specializations Distribution", titleFont)
            specializationHeader.Alignment = Element.ALIGN_CENTER
            specializationHeader.SpacingBefore = 20
            doc.Add(specializationHeader)
            AddSpecializationChartToPdf(doc, dt)
            doc.NewPage()

            ' Geographic Distribution and Performance Analysis (Combined Page - Side by Side)
            Dim combinedAnalysisHeader As New Paragraph("Geographic & Performance Analysis", titleFont)
            combinedAnalysisHeader.Alignment = Element.ALIGN_CENTER
            combinedAnalysisHeader.SpacingBefore = 20
            doc.Add(combinedAnalysisHeader)
            doc.Add(New Paragraph(Environment.NewLine))

            ' Create a table to hold both analysis side by side
            Dim analysisTable As New PdfPTable(2)
            analysisTable.WidthPercentage = 100
            analysisTable.SetWidths({50, 50})

            ' Left column - City Distribution
            Dim cityCell As New PdfPCell()
            cityCell.Border = Rectangle.NO_BORDER
            cityCell.AddElement(New Paragraph("Top Cities by Agencies", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 12)))
            cityCell.AddElement(GenerateCityDistributionTable(dt))
            analysisTable.AddCell(cityCell)

            ' Right column - Performance Analysis
            Dim performanceCell As New PdfPCell()
            performanceCell.Border = Rectangle.NO_BORDER
            performanceCell.AddElement(New Paragraph("Agency Performance Levels", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 12)))
            performanceCell.AddElement(GeneratePerformanceAnalysisTable(dt))
            analysisTable.AddCell(performanceCell)

            doc.Add(analysisTable)
            doc.NewPage()

            ' Years of Operation Analysis (One Page)
            Dim yearsHeader As New Paragraph("Years of Operation Analysis", titleFont)
            yearsHeader.Alignment = Element.ALIGN_CENTER
            yearsHeader.SpacingBefore = 20
            doc.Add(yearsHeader)
            doc.Add(New Paragraph(Environment.NewLine))
            doc.Add(GenerateYearsOperationTable(dt))

            doc.Close()
            Return True
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
            Return False
        End Try
    End Function

    Private Sub AddAgencyDataTable(doc As Document, dt As DataTable)
        If dt.Rows.Count = 0 Then
            doc.Add(New Paragraph("No agency data available.", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA, 12)))
            Return
        End If

        ' Create table with selected columns for better readability
        Dim selectedColumns As String() = {"AgencyID", "AgencyName", "AgencyLicenseNumber", "Address", "ContactNum", "Specialization", "GovAccreditationStat", "Partnerships", "TotalDeployments", "ActiveJobs"}
        Dim columnHeaders As String() = {"ID", "Agency Name", "License #", "Address", "Contact", "Specialization", "Accreditation", "Partners", "Deploy.", "Jobs"}

        Dim table As New PdfPTable(selectedColumns.Length)
        table.WidthPercentage = 100

        ' Add headers
        For Each header As String In columnHeaders
            table.AddCell(New PdfPCell(New Phrase(header, iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 7))))
        Next

        ' Add data rows
        For Each row As DataRow In dt.Rows
            For Each col As String In selectedColumns
                Dim cellValue As String = If(row(col) IsNot Nothing, row(col).ToString(), "")
                Dim cell As New PdfPCell(New Phrase(cellValue, iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA, 6)))

                ' Color code accreditation status
                If col = "GovAccreditationStat" Then
                    If cellValue = "Accredited" Then
                        cell.BackgroundColor = New BaseColor(200, 255, 200)
                    ElseIf cellValue = "Pending" Then
                        cell.BackgroundColor = New BaseColor(255, 255, 200)
                    Else
                        cell.BackgroundColor = New BaseColor(255, 200, 200)
                    End If
                End If

                table.AddCell(cell)
            Next
        Next

        doc.Add(table)
    End Sub

    Private Sub AddChartToPdf(doc As Document, dt As DataTable, columnName As String)
        Dim chart As New Chart()
        chart.Width = 600
        chart.Height = 400
        chart.BackColor = DrawingColor.White
        chart.ChartAreas.Add(New ChartArea())

        Dim series As New Series()
        series.ChartType = SeriesChartType.Pie
        series.CustomProperties = "PieLabelStyle=Outside"
        series.BorderColor = DrawingColor.Black
        series.BorderWidth = 1

        Dim grouped = From row In dt.AsEnumerable()
                      Where Not row.IsNull(columnName) AndAlso Not String.IsNullOrEmpty(row.Field(Of String)(columnName))
                      Group row By key = row.Field(Of String)(columnName) Into Group
                      Select key, Count = Group.Count()

        For Each item In grouped
            series.Points.AddXY(item.key, item.Count)
            series.Points(series.Points.Count - 1).Label = item.key & " (" & item.Count & ")"
        Next

        chart.Series.Add(series)

        Dim bmp As New DrawingBitmap(chart.Width, chart.Height)
        chart.DrawToBitmap(bmp, New System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height))

        Using ms As New MemoryStream()
            bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png)
            Dim img = iTextSharp.text.Image.GetInstance(ms.ToArray())
            img.Alignment = Element.ALIGN_CENTER
            img.ScaleToFit(400.0F, 300.0F)
            doc.Add(img)
        End Using
    End Sub

    Private Sub AddSpecializationChartToPdf(doc As Document, dt As DataTable)
        Dim chart As New Chart()
        chart.Width = 600
        chart.Height = 400
        chart.BackColor = DrawingColor.White
        chart.ChartAreas.Add(New ChartArea())

        Dim series As New Series()
        series.ChartType = SeriesChartType.Pie
        series.CustomProperties = "PieLabelStyle=Outside"
        series.BorderColor = DrawingColor.Black
        series.BorderWidth = 1

        ' Count specializations and get top 8
        Dim specializationCounts As New Dictionary(Of String, Integer)()
        For Each row As DataRow In dt.Rows
            If Not row.IsNull("Specialization") Then
                Dim spec = row("Specialization").ToString().Trim()
                If Not String.IsNullOrEmpty(spec) Then
                    If specializationCounts.ContainsKey(spec) Then
                        specializationCounts(spec) += 1
                    Else
                        specializationCounts(spec) = 1
                    End If
                End If
            End If
        Next

        Dim topSpecs = specializationCounts.OrderByDescending(Function(x) x.Value).Take(8).ToList()

        For Each item In topSpecs
            series.Points.AddXY(item.Key, item.Value)
            series.Points(series.Points.Count - 1).Label = item.Key & " (" & item.Value & ")"
        Next

        chart.Series.Add(series)

        Dim bmp As New DrawingBitmap(chart.Width, chart.Height)
        chart.DrawToBitmap(bmp, New System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height))

        Using ms As New MemoryStream()
            bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png)
            Dim img = iTextSharp.text.Image.GetInstance(ms.ToArray())
            img.Alignment = Element.ALIGN_CENTER
            img.ScaleToFit(400.0F, 300.0F)
            doc.Add(img)
        End Using
    End Sub

    Private Function GenerateCityDistributionTable(dt As DataTable) As PdfPTable
        Dim cityTable As New PdfPTable(2)
        cityTable.WidthPercentage = 90
        cityTable.SpacingBefore = 10
        cityTable.SpacingAfter = 10

        cityTable.AddCell(New PdfPCell(New Phrase("City", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 10))))
        cityTable.AddCell(New PdfPCell(New Phrase("Count", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 10))))

        ' Count cities
        Dim cityCounts As New Dictionary(Of String, Integer)()
        For Each row As DataRow In dt.Rows
            If Not row.IsNull("City") Then
                Dim city = row("City").ToString().Trim()
                If Not String.IsNullOrEmpty(city) Then
                    If cityCounts.ContainsKey(city) Then
                        cityCounts(city) += 1
                    Else
                        cityCounts(city) = 1
                    End If
                End If
            End If
        Next

        Dim topCities = cityCounts.OrderByDescending(Function(x) x.Value).Take(10).ToList()

        For Each city In topCities
            cityTable.AddCell(city.Key)
            cityTable.AddCell(city.Value.ToString())
        Next

        Return cityTable
    End Function

    Private Function GeneratePerformanceAnalysisTable(dt As DataTable) As PdfPTable
        Dim perfTable As New PdfPTable(2)
        perfTable.WidthPercentage = 90
        perfTable.SpacingBefore = 10
        perfTable.SpacingAfter = 10

        perfTable.AddCell(New PdfPCell(New Phrase("Performance Level", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 10))))
        perfTable.AddCell(New PdfPCell(New Phrase("Count", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 10))))

        ' Calculate performance levels based on deployments
        Dim performanceLevels As New Dictionary(Of String, Integer) From {
            {"High Performance (10+ Deployments)", 0},
            {"Medium Performance (5-9 Deployments)", 0},
            {"Low Performance (1-4 Deployments)", 0},
            {"New/Inactive (0 Deployments)", 0}
        }

        For Each row As DataRow In dt.Rows
            Dim deployments = If(row.IsNull("TotalDeployments"), 0, CInt(row("TotalDeployments")))

            If deployments >= 10 Then
                performanceLevels("High Performance (10+ Deployments)") += 1
            ElseIf deployments >= 5 Then
                performanceLevels("Medium Performance (5-9 Deployments)") += 1
            ElseIf deployments >= 1 Then
                performanceLevels("Low Performance (1-4 Deployments)") += 1
            Else
                performanceLevels("New/Inactive (0 Deployments)") += 1
            End If
        Next

        For Each perf In performanceLevels
            perfTable.AddCell(perf.Key)
            perfTable.AddCell(perf.Value.ToString())
        Next

        Return perfTable
    End Function

    Private Function GenerateYearsOperationTable(dt As DataTable) As PdfPTable
        Dim yearsTable As New PdfPTable(3)
        yearsTable.WidthPercentage = 80
        yearsTable.SetWidths({50, 20, 30})
        yearsTable.SpacingBefore = 10
        yearsTable.SpacingAfter = 10
        yearsTable.HorizontalAlignment = Element.ALIGN_CENTER

        ' Table headers
        yearsTable.AddCell(New PdfPCell(New Phrase("Years of Operation", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 12))))
        yearsTable.AddCell(New PdfPCell(New Phrase("Count", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 12))))
        yearsTable.AddCell(New PdfPCell(New Phrase("Percentage", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 12))))

        ' Create years groups
        Dim yearRanges As New Dictionary(Of String, Integer) From {
            {"0-2 years", 0},
            {"3-5 years", 0},
            {"6-10 years", 0},
            {"11-15 years", 0},
            {"16-20 years", 0},
            {"20+ years", 0}
        }

        For Each row As DataRow In dt.Rows
            If Not row.IsNull("YearsOfOperation") AndAlso IsNumeric(row("YearsOfOperation")) Then
                Dim years = CInt(row("YearsOfOperation"))
                If years <= 2 Then
                    yearRanges("0-2 years") += 1
                ElseIf years <= 5 Then
                    yearRanges("3-5 years") += 1
                ElseIf years <= 10 Then
                    yearRanges("6-10 years") += 1
                ElseIf years <= 15 Then
                    yearRanges("11-15 years") += 1
                ElseIf years <= 20 Then
                    yearRanges("16-20 years") += 1
                Else
                    yearRanges("20+ years") += 1
                End If
            End If
        Next

        ' Calculate total for percentage
        Dim totalAgencies As Integer = yearRanges.Values.Sum()

        ' Add data rows
        For Each yearRange In yearRanges
            Dim percentage As Double = If(totalAgencies > 0, (yearRange.Value / totalAgencies) * 100, 0)
            yearsTable.AddCell(yearRange.Key)
            yearsTable.AddCell(yearRange.Value.ToString())
            yearsTable.AddCell(percentage.ToString("F1") & "%")
        Next

        ' Add total row
        Dim totalCell As New PdfPCell(New Phrase("Total", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 10)))
        totalCell.BackgroundColor = BaseColor.LIGHT_GRAY
        yearsTable.AddCell(totalCell)

        Dim totalCountCell As New PdfPCell(New Phrase(totalAgencies.ToString(), iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 10)))
        totalCountCell.BackgroundColor = BaseColor.LIGHT_GRAY
        yearsTable.AddCell(totalCountCell)

        Dim totalPercentCell As New PdfPCell(New Phrase("100.0%", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 10)))
        totalPercentCell.BackgroundColor = BaseColor.LIGHT_GRAY
        yearsTable.AddCell(totalPercentCell)

        Return yearsTable
    End Function

    Private Class FooterEvent
        Inherits PdfPageEventHelper

        Public Overrides Sub OnEndPage(writer As PdfWriter, document As Document)
            Dim cb As PdfContentByte = writer.DirectContent
            Dim footerText As String = "Generated on " & DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            Dim font = iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_OBLIQUE, 8, BaseColor.GRAY)
            ColumnText.ShowTextAligned(cb, Element.ALIGN_CENTER, New Phrase(footerText, font),
                                       (document.Left + document.Right) / 2, document.Bottom - 10, 0)
        End Sub
    End Class
End Class
