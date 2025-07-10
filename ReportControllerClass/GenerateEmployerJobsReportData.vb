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

Public Class GenerateEmployerJobsReportData
    Public Function GenerateReport() As Boolean
        Try
            Dim query As String = "
                SELECT  jp.JobTitle, jp.JobDescription, jp.CountryOfEmployment,
                       jp.SalaryRange, jp.EmploymentContractDuration, jp.RequiredSkills, jp.JobType,
                       jp.VisaType, jp.NumOfVacancies, jp.Conditions, jp.PostingDate, jp.Benefits,
                       jp.ApplicationDeadline, jp.JobStatus, e.CompanyName AS EmployerCompany,
                       a.AgencyName
                FROM jobplacement jp
                LEFT JOIN employer e ON jp.EmployerID = e.EmployerID
                LEFT JOIN agency a ON jp.AgencyID = a.AgencyID"

            readQuery(query)
            Dim dt As New DataTable()
            dt.Load(cmdRead)

            Dim fileName As String = "JobPlacementsGeneralReport-" & DateTime.Now.ToString("yyyyMMdd") & ".pdf"
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
            
            Dim titleParagraph2 As New Paragraph("Job Placements General Report Data", titleFont)
            titleParagraph2.Alignment = Element.ALIGN_CENTER
            doc.Add(titleParagraph2)
            
            doc.Add(New Paragraph(Environment.NewLine))

            ' Job Placements Table
            Dim table As New PdfPTable(dt.Columns.Count)
            table.WidthPercentage = 100

            For Each col As DataColumn In dt.Columns
                table.AddCell(New PdfPCell(New Phrase(col.ColumnName, iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 8))))
            Next

            For Each row As DataRow In dt.Rows
                For Each col As DataColumn In dt.Columns
                    table.AddCell(New PdfPCell(New Phrase(row(col.ColumnName).ToString(), iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA, 7))))
                Next
            Next

            doc.Add(table)
            doc.NewPage()

            ' Summary section
            doc.Add(New Paragraph("Summary", titleFont))
            doc.Add(New Paragraph(Environment.NewLine))

            ' Job Type Distribution (One Page)
            Dim jobTypeHeader As New Paragraph("Job Type Distribution", titleFont)
            jobTypeHeader.Alignment = Element.ALIGN_CENTER
            jobTypeHeader.SpacingBefore = 20
            doc.Add(jobTypeHeader)
            AddChartToPdf(doc, dt, "JobType")
            doc.NewPage()

            ' Country Distribution (One Page)
            Dim countryHeader As New Paragraph("Country of Employment Distribution", titleFont)
            countryHeader.Alignment = Element.ALIGN_CENTER
            countryHeader.SpacingBefore = 20
            doc.Add(countryHeader)
            AddChartToPdf(doc, dt, "CountryOfEmployment")
            doc.NewPage()

            ' Visa Type and Job Status Distribution (Combined Page - Side by Side)
            Dim combinedChartsHeader As New Paragraph("Visa Type & Job Status Distribution", titleFont)
            combinedChartsHeader.Alignment = Element.ALIGN_CENTER
            combinedChartsHeader.SpacingBefore = 20
            doc.Add(combinedChartsHeader)
            doc.Add(New Paragraph(Environment.NewLine))

            ' Create a table to hold both charts side by side
            Dim chartsTable As New PdfPTable(2)
            chartsTable.WidthPercentage = 100
            chartsTable.SetWidths({50, 50})

            ' Left column - Visa Type Distribution
            Dim visaCell As New PdfPCell()
            visaCell.Border = Rectangle.NO_BORDER
            visaCell.AddElement(New Paragraph("Visa Type Distribution", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 12)))
            AddChartToPdfCell(visaCell, dt, "VisaType")
            chartsTable.AddCell(visaCell)

            ' Right column - Job Status Distribution
            Dim statusCell As New PdfPCell()
            statusCell.Border = Rectangle.NO_BORDER
            statusCell.AddElement(New Paragraph("Job Status Distribution", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 12)))
            AddChartToPdfCell(statusCell, dt, "JobStatus")
            chartsTable.AddCell(statusCell)

            doc.Add(chartsTable)
            doc.NewPage()

            ' Required Skills and Salary Range Analysis (Combined Page - Side by Side)
            Dim combinedTablesHeader As New Paragraph("Skills & Salary Analysis", titleFont)
            combinedTablesHeader.Alignment = Element.ALIGN_CENTER
            combinedTablesHeader.SpacingBefore = 20
            doc.Add(combinedTablesHeader)
            doc.Add(New Paragraph(Environment.NewLine))

            ' Create a table to hold both summary tables side by side
            Dim mainTable As New PdfPTable(2)
            mainTable.WidthPercentage = 100
            mainTable.SetWidths({50, 50})

            ' Left column - Required Skills Distribution
            Dim skillsCell As New PdfPCell()
            skillsCell.Border = Rectangle.NO_BORDER
            skillsCell.AddElement(New Paragraph("Top Required Skills", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 12)))
            skillsCell.AddElement(GenerateSkillsTable(dt))
            mainTable.AddCell(skillsCell)

            ' Right column - Salary Range Distribution
            Dim salaryCell As New PdfPCell()
            salaryCell.Border = Rectangle.NO_BORDER
            salaryCell.AddElement(New Paragraph("Salary Range Distribution", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 12)))
            salaryCell.AddElement(GenerateSalaryRangeTable(dt))
            mainTable.AddCell(salaryCell)

            doc.Add(mainTable)
            doc.NewPage()

            ' Job Activity Analysis (One Page)
            Dim activityHeader As New Paragraph("Job Vacancy Analysis", titleFont)
            activityHeader.Alignment = Element.ALIGN_CENTER
            activityHeader.SpacingBefore = 20
            doc.Add(activityHeader)
            doc.Add(New Paragraph(Environment.NewLine))
            doc.Add(GenerateVacancyAnalysisTable(dt))

            doc.Close()
            Return True
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
            Return False
        End Try
    End Function

    Private Sub AddChartToPdfCell(cell As PdfPCell, dt As DataTable, columnName As String)
        Dim chart As New Chart()
        chart.Width = 300
        chart.Height = 200
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
            img.ScaleToFit(280.0F, 180.0F)
            cell.AddElement(img)
        End Using
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

    Private Function GenerateSkillsTable(dt As DataTable) As PdfPTable
        Dim skillsTable As New PdfPTable(2)
        skillsTable.WidthPercentage = 90
        skillsTable.SpacingBefore = 10
        skillsTable.SpacingAfter = 10

        skillsTable.AddCell(New PdfPCell(New Phrase("Required Skill", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 10))))
        skillsTable.AddCell(New PdfPCell(New Phrase("Count", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 10))))

        ' Parse skills and count occurrences
        Dim skillCounts As New Dictionary(Of String, Integer)
        
        For Each row As DataRow In dt.Rows
            If Not row.IsNull("RequiredSkills") AndAlso Not String.IsNullOrEmpty(row.Field(Of String)("RequiredSkills")) Then
                Dim skills() As String = row.Field(Of String)("RequiredSkills").Split(","c)
                For Each skill In skills
                    Dim cleanSkill As String = skill.Trim().ToLower()
                    If Not String.IsNullOrEmpty(cleanSkill) Then
                        If skillCounts.ContainsKey(cleanSkill) Then
                            skillCounts(cleanSkill) += 1
                        Else
                            skillCounts(cleanSkill) = 1
                        End If
                    End If
                Next
            End If
        Next

        ' Sort by count and take top 10
        Dim sortedSkills = skillCounts.OrderByDescending(Function(x) x.Value).Take(10)

        For Each skill In sortedSkills
            skillsTable.AddCell(skill.Key)
            skillsTable.AddCell(skill.Value.ToString())
        Next

        Return skillsTable
    End Function

    Private Function GenerateSalaryRangeTable(dt As DataTable) As PdfPTable
        Dim salaryTable As New PdfPTable(2)
        salaryTable.WidthPercentage = 90
        salaryTable.SpacingBefore = 10
        salaryTable.SpacingAfter = 10

        salaryTable.AddCell(New PdfPCell(New Phrase("Salary Range", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 10))))
        salaryTable.AddCell(New PdfPCell(New Phrase("Count", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 10))))

        ' Create salary range groups
        Dim salaryGroups As New Dictionary(Of String, Integer) From {
            {"$0 - $500", 0},
            {"$501 - $1,000", 0},
            {"$1,001 - $2,000", 0},
            {"$2,001 - $5,000", 0},
            {"$5,000+", 0}
        }

        For Each row As DataRow In dt.Rows
            If Not row.IsNull("SalaryRange") Then
                Dim salary As Integer = 0
                If Integer.TryParse(row("SalaryRange").ToString(), salary) Then
                    If salary <= 500 Then
                        salaryGroups("$0 - $500") += 1
                    ElseIf salary <= 1000 Then
                        salaryGroups("$501 - $1,000") += 1
                    ElseIf salary <= 2000 Then
                        salaryGroups("$1,001 - $2,000") += 1
                    ElseIf salary <= 5000 Then
                        salaryGroups("$2,001 - $5,000") += 1
                    Else
                        salaryGroups("$5,000+") += 1
                    End If
                End If
            End If
        Next

        For Each group In salaryGroups
            salaryTable.AddCell(group.Key)
            salaryTable.AddCell(group.Value.ToString())
        Next

        Return salaryTable
    End Function

    Private Function GenerateVacancyAnalysisTable(dt As DataTable) As PdfPTable
        Dim vacancyTable As New PdfPTable(3)
        vacancyTable.WidthPercentage = 80
        vacancyTable.SetWidths({50, 20, 30})
        vacancyTable.SpacingBefore = 10
        vacancyTable.SpacingAfter = 10
        vacancyTable.HorizontalAlignment = Element.ALIGN_CENTER

        ' Table headers
        vacancyTable.AddCell(New PdfPCell(New Phrase("Vacancy Range", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 12))))
        vacancyTable.AddCell(New PdfPCell(New Phrase("Count", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 12))))
        vacancyTable.AddCell(New PdfPCell(New Phrase("Percentage", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 12))))

        ' Create vacancy groups
        Dim vacancyGroups As New Dictionary(Of String, Integer) From {
            {"1-5 Vacancies", 0},
            {"6-10 Vacancies", 0},
            {"11-20 Vacancies", 0},
            {"21+ Vacancies", 0}
        }

        For Each row As DataRow In dt.Rows
            If Not row.IsNull("NumOfVacancies") Then
                Dim vacancies As Integer = 0
                If Integer.TryParse(row("NumOfVacancies").ToString(), vacancies) Then
                    If vacancies <= 5 Then
                        vacancyGroups("1-5 Vacancies") += 1
                    ElseIf vacancies <= 10 Then
                        vacancyGroups("6-10 Vacancies") += 1
                    ElseIf vacancies <= 20 Then
                        vacancyGroups("11-20 Vacancies") += 1
                    Else
                        vacancyGroups("21+ Vacancies") += 1
                    End If
                End If
            End If
        Next

        ' Calculate total for percentage
        Dim totalJobs As Integer = vacancyGroups.Values.Sum()

        ' Add data rows
        For Each group In vacancyGroups
            Dim percentage As Double = If(totalJobs > 0, (group.Value / totalJobs) * 100, 0)
            vacancyTable.AddCell(group.Key)
            vacancyTable.AddCell(group.Value.ToString())
            vacancyTable.AddCell(percentage.ToString("F1") & "%")
        Next

        ' Add total row
        Dim totalCell As New PdfPCell(New Phrase("Total", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 10)))
        totalCell.BackgroundColor = BaseColor.LIGHT_GRAY
        vacancyTable.AddCell(totalCell)
        
        Dim totalCountCell As New PdfPCell(New Phrase(totalJobs.ToString(), iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 10)))
        totalCountCell.BackgroundColor = BaseColor.LIGHT_GRAY
        vacancyTable.AddCell(totalCountCell)
        
        Dim totalPercentCell As New PdfPCell(New Phrase("100.0%", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 10)))
        totalPercentCell.BackgroundColor = BaseColor.LIGHT_GRAY
        vacancyTable.AddCell(totalPercentCell)

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
