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

Public Class GenerateAgencyJobReportData
    Public Function GenerateReport() As Boolean
        Try
            Dim agencyId As Integer = Session.CurrentReferenceID
            
            Dim query As String = "
                SELECT 
                    jp.JobPlacementID,
                    jp.JobTitle,
                    jp.JobDescription,
                    jp.CountryOfEmployment,
                    jp.SalaryRange,
                    jp.EmploymentContractDuration,
                    jp.RequiredSkills,
                    jp.JobType,
                    jp.VisaType,
                    jp.NumOfVacancies,
                    jp.Conditions,
                    jp.Benefits,
                    jp.ApplicationDeadline,
                    jp.JobStatus,
                    jp.PostingDate,
                    e.CompanyName AS EmployerCompany,
                    e.Industry AS EmployerIndustry,
                    a.AgencyName,
                    COUNT(DISTINCT app.ApplicationID) AS TotalApplications,
                    COUNT(DISTINCT dr.DeploymentID) AS TotalDeployments
                FROM jobplacement jp
                JOIN employer e ON jp.EmployerID = e.EmployerID
                JOIN agencypartneremployer ape ON ape.EmployerID = e.EmployerID
                JOIN agency a ON ape.AgencyID = a.AgencyID
                LEFT JOIN application app ON jp.JobPlacementID = app.JobPlacementID
                LEFT JOIN deploymentrecord dr ON app.ApplicationID = dr.ApplicationID
                WHERE ape.AgencyID = " & agencyId & "
                GROUP BY jp.JobPlacementID
                ORDER BY jp.PostingDate DESC"

            readQuery(query)
            Dim dt As New DataTable()
            dt.Load(cmdRead)

            Dim fileName As String = "AgencyJobReport-" & DateTime.Now.ToString("yyyyMMdd") & ".pdf"
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

            Dim agencyName As String = If(dt.Rows.Count > 0, dt.Rows(0)("AgencyName").ToString(), "Agency")
            Dim titleParagraph2 As New Paragraph($"{agencyName} - Job Placement Report", titleFont)
            titleParagraph2.Alignment = Element.ALIGN_CENTER
            doc.Add(titleParagraph2)

            doc.Add(New Paragraph(Environment.NewLine))

            ' Job Data Table
            AddJobDataTable(doc, dt)
            doc.NewPage()

            ' Summary section
            doc.Add(New Paragraph("Summary", titleFont))
            doc.Add(New Paragraph(Environment.NewLine))

            ' Job Status Distribution (One Page)
            Dim statusHeader As New Paragraph("Job Status Distribution", titleFont)
            statusHeader.Alignment = Element.ALIGN_CENTER
            statusHeader.SpacingBefore = 20
            doc.Add(statusHeader)
            AddChartToPdf(doc, dt, "JobStatus")
            doc.NewPage()

            ' Country Distribution (One Page)
            Dim countryHeader As New Paragraph("Country Employment Distribution", titleFont)
            countryHeader.Alignment = Element.ALIGN_CENTER
            countryHeader.SpacingBefore = 20
            doc.Add(countryHeader)
            AddChartToPdf(doc, dt, "CountryOfEmployment")
            doc.NewPage()

            ' Job Type and Industry Analysis (Combined Page - Side by Side)
            Dim combinedAnalysisHeader As New Paragraph("Job Type & Industry Analysis", titleFont)
            combinedAnalysisHeader.Alignment = Element.ALIGN_CENTER
            combinedAnalysisHeader.SpacingBefore = 20
            doc.Add(combinedAnalysisHeader)
            doc.Add(New Paragraph(Environment.NewLine))

            ' Create a table to hold both analysis side by side
            Dim analysisTable As New PdfPTable(2)
            analysisTable.WidthPercentage = 100
            analysisTable.SetWidths({50, 50})

            ' Left column - Job Type Distribution
            Dim jobTypeCell As New PdfPCell()
            jobTypeCell.Border = Rectangle.NO_BORDER
            jobTypeCell.AddElement(New Paragraph("Job Types", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 12)))
            jobTypeCell.AddElement(GenerateJobTypeDistributionTable(dt))
            analysisTable.AddCell(jobTypeCell)

            ' Right column - Industry Analysis
            Dim industryCell As New PdfPCell()
            industryCell.Border = Rectangle.NO_BORDER
            industryCell.AddElement(New Paragraph("Top Industries", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 12)))
            industryCell.AddElement(GenerateIndustryAnalysisTable(dt))
            analysisTable.AddCell(industryCell)

            doc.Add(analysisTable)
            doc.NewPage()

            ' Salary and Vacancy Analysis (Combined Page - Side by Side)
            Dim salaryVacancyHeader As New Paragraph("Salary & Vacancy Analysis", titleFont)
            salaryVacancyHeader.Alignment = Element.ALIGN_CENTER
            salaryVacancyHeader.SpacingBefore = 20
            doc.Add(salaryVacancyHeader)
            doc.Add(New Paragraph(Environment.NewLine))

            ' Create a table to hold both analysis side by side
            Dim salaryTable As New PdfPTable(2)
            salaryTable.WidthPercentage = 100
            salaryTable.SetWidths({50, 50})

            ' Left column - Salary Range Distribution
            Dim salaryCell As New PdfPCell()
            salaryCell.Border = Rectangle.NO_BORDER
            salaryCell.AddElement(New Paragraph("Salary Ranges", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 12)))
            salaryCell.AddElement(GenerateSalaryDistributionTable(dt))
            salaryTable.AddCell(salaryCell)

            ' Right column - Vacancy Activity
            Dim vacancyCell As New PdfPCell()
            vacancyCell.Border = Rectangle.NO_BORDER
            vacancyCell.AddElement(New Paragraph("Vacancy Levels", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 12)))
            vacancyCell.AddElement(GenerateVacancyActivityTable(dt))
            salaryTable.AddCell(vacancyCell)

            doc.Add(salaryTable)

            doc.Close()
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Sub AddJobDataTable(doc As Document, dt As DataTable)
        If dt.Rows.Count = 0 Then
            doc.Add(New Paragraph("No job data available for this agency.", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA, 12)))
            Return
        End If

        ' Create table with selected columns for better readability
        Dim selectedColumns As String() = {"JobPlacementID", "JobTitle", "EmployerCompany", "CountryOfEmployment", "SalaryRange", "JobType", "NumOfVacancies", "JobStatus", "TotalApplications", "TotalDeployments"}
        Dim columnHeaders As String() = {"ID", "Job Title", "Employer", "Country", "Salary", "Type", "Vacancies", "Status", "Apps", "Deploy."}

        Dim table As New PdfPTable(selectedColumns.Length)
        table.WidthPercentage = 100

        ' Add headers
        For Each header As String In columnHeaders
            table.AddCell(New PdfPCell(New Phrase(header, iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 8))))
        Next

        ' Add data rows
        For Each row As DataRow In dt.Rows
            For Each col As String In selectedColumns
                Dim cellValue As String = If(row(col) IsNot Nothing, row(col).ToString(), "")
                
                ' Truncate long job titles
                If col = "JobTitle" AndAlso cellValue.Length > 25 Then
                    cellValue = cellValue.Substring(0, 22) & "..."
                End If
                
                ' Truncate company names
                If col = "EmployerCompany" AndAlso cellValue.Length > 20 Then
                    cellValue = cellValue.Substring(0, 17) & "..."
                End If
                
                table.AddCell(New PdfPCell(New Phrase(cellValue, iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA, 7))))
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

    Private Function GenerateJobTypeDistributionTable(dt As DataTable) As PdfPTable
        Dim jobTypeTable As New PdfPTable(2)
        jobTypeTable.WidthPercentage = 90
        jobTypeTable.SpacingBefore = 10
        jobTypeTable.SpacingAfter = 10

        jobTypeTable.AddCell(New PdfPCell(New Phrase("Job Type", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 10))))
        jobTypeTable.AddCell(New PdfPCell(New Phrase("Count", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 10))))

        ' Count job types
        Dim jobTypeCounts As New Dictionary(Of String, Integer)()
        For Each row As DataRow In dt.Rows
            If Not row.IsNull("JobType") Then
                Dim jobType = row("JobType").ToString().Trim()
                If Not String.IsNullOrEmpty(jobType) Then
                    If jobTypeCounts.ContainsKey(jobType) Then
                        jobTypeCounts(jobType) += 1
                    Else
                        jobTypeCounts(jobType) = 1
                    End If
                End If
            End If
        Next

        Dim topJobTypes = jobTypeCounts.OrderByDescending(Function(x) x.Value).Take(10).ToList()

        For Each jobType In topJobTypes
            jobTypeTable.AddCell(jobType.Key)
            jobTypeTable.AddCell(jobType.Value.ToString())
        Next

        Return jobTypeTable
    End Function

    Private Function GenerateIndustryAnalysisTable(dt As DataTable) As PdfPTable
        Dim industryTable As New PdfPTable(2)
        industryTable.WidthPercentage = 90
        industryTable.SpacingBefore = 10
        industryTable.SpacingAfter = 10

        industryTable.AddCell(New PdfPCell(New Phrase("Industry", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 10))))
        industryTable.AddCell(New PdfPCell(New Phrase("Jobs", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 10))))

        ' Count industries
        Dim industryCounts As New Dictionary(Of String, Integer)()
        For Each row As DataRow In dt.Rows
            If Not row.IsNull("EmployerIndustry") Then
                Dim industry = row("EmployerIndustry").ToString().Trim()
                If Not String.IsNullOrEmpty(industry) Then
                    If industryCounts.ContainsKey(industry) Then
                        industryCounts(industry) += 1
                    Else
                        industryCounts(industry) = 1
                    End If
                End If
            End If
        Next

        Dim topIndustries = industryCounts.OrderByDescending(Function(x) x.Value).Take(10).ToList()

        For Each industry In topIndustries
            industryTable.AddCell(industry.Key)
            industryTable.AddCell(industry.Value.ToString())
        Next

        Return industryTable
    End Function

    Private Function GenerateSalaryDistributionTable(dt As DataTable) As PdfPTable
        Dim salaryTable As New PdfPTable(3)
        salaryTable.WidthPercentage = 90
        salaryTable.SpacingBefore = 10
        salaryTable.SpacingAfter = 10
        salaryTable.SetWidths({50, 20, 30})

        ' Table headers
        salaryTable.AddCell(New PdfPCell(New Phrase("Salary Range", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 10))))
        salaryTable.AddCell(New PdfPCell(New Phrase("Jobs", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 10))))
        salaryTable.AddCell(New PdfPCell(New Phrase("Percentage", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 10))))

        ' Count salary ranges
        Dim salaryRanges As New Dictionary(Of String, Integer)()
        For Each row As DataRow In dt.Rows
            If Not row.IsNull("SalaryRange") Then
                Dim salaryRange = row("SalaryRange").ToString().Trim()
                If Not String.IsNullOrEmpty(salaryRange) Then
                    If salaryRanges.ContainsKey(salaryRange) Then
                        salaryRanges(salaryRange) += 1
                    Else
                        salaryRanges(salaryRange) = 1
                    End If
                End If
            End If
        Next

        ' Calculate total for percentage
        Dim totalJobs As Integer = salaryRanges.Values.Sum()

        ' Add data rows (top 10)
        Dim topSalaryRanges = salaryRanges.OrderByDescending(Function(x) x.Value).Take(10).ToList()
        For Each salaryRange In topSalaryRanges
            Dim percentage As Double = If(totalJobs > 0, (salaryRange.Value / totalJobs) * 100, 0)
            salaryTable.AddCell(salaryRange.Key)
            salaryTable.AddCell(salaryRange.Value.ToString())
            salaryTable.AddCell(percentage.ToString("F1") & "%")
        Next

        Return salaryTable
    End Function

    Private Function GenerateVacancyActivityTable(dt As DataTable) As PdfPTable
        Dim vacancyTable As New PdfPTable(3)
        vacancyTable.WidthPercentage = 90
        vacancyTable.SpacingBefore = 10
        vacancyTable.SpacingAfter = 10
        vacancyTable.SetWidths({50, 20, 30})

        ' Table headers
        vacancyTable.AddCell(New PdfPCell(New Phrase("Vacancy Level", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 10))))
        vacancyTable.AddCell(New PdfPCell(New Phrase("Jobs", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 10))))
        vacancyTable.AddCell(New PdfPCell(New Phrase("Percentage", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 10))))

        ' Calculate vacancy levels
        Dim vacancyLevels As New Dictionary(Of String, Integer) From {
            {"High Demand (20+ positions)", 0},
            {"Medium Demand (10-19 positions)", 0},
            {"Low Demand (5-9 positions)", 0},
            {"Limited Demand (1-4 positions)", 0},
            {"No Vacancies (0)", 0}
        }

        For Each row As DataRow In dt.Rows
            Dim vacancies = If(row.IsNull("NumOfVacancies"), 0, CInt(row("NumOfVacancies")))
            
            If vacancies >= 20 Then
                vacancyLevels("High Demand (20+ positions)") += 1
            ElseIf vacancies >= 10 Then
                vacancyLevels("Medium Demand (10-19 positions)") += 1
            ElseIf vacancies >= 5 Then
                vacancyLevels("Low Demand (5-9 positions)") += 1
            ElseIf vacancies >= 1 Then
                vacancyLevels("Limited Demand (1-4 positions)") += 1
            Else
                vacancyLevels("No Vacancies (0)") += 1
            End If
        Next

        ' Calculate total for percentage
        Dim totalJobs As Integer = vacancyLevels.Values.Sum()

        ' Add data rows
        For Each vacancy In vacancyLevels
            Dim percentage As Double = If(totalJobs > 0, (vacancy.Value / totalJobs) * 100, 0)
            vacancyTable.AddCell(vacancy.Key)
            vacancyTable.AddCell(vacancy.Value.ToString())
            vacancyTable.AddCell(percentage.ToString("F1") & "%")
        Next

        Return vacancyTable
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
