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

Public Class GenerateAgencyDeploymentReportData
    Public Function GenerateReport() As Boolean
        Try
            Dim agencyId As Integer = Session.CurrentReferenceID
            
            Dim query As String = "
                SELECT 
                    dr.DeploymentID,
                    CONCAT(o.FirstName, ' ', o.MiddleName, ' ', o.LastName) AS OFWName,
                    jp.JobTitle,
                    e.CompanyName AS EmployerCompany,
                    dr.CountryOfDeployment,
                    dr.Salary,
                    dr.ContractNumber,
                    dr.ContractDuration,
                    dr.DeploymentStatus,
                    dr.ContractStartDate,
                    dr.ContractEndDate,
                    dr.RepatriationStatus,
                    dr.ReasonForReturn,
                    dr.DeploymentRemarks,
                    a.AgencyName,
                    dr.DeploymentDate,
                    DATEDIFF(IFNULL(dr.ContractEndDate, CURDATE()), dr.ContractStartDate) AS ContractDaysTotal,
                    CASE 
                        WHEN dr.DeploymentStatus = 'Completed' THEN 1
                        WHEN dr.DeploymentStatus = 'Active' THEN 0
                        ELSE 0
                    END AS IsCompleted
                FROM deploymentrecord dr
                LEFT JOIN application app ON dr.ApplicationID = app.ApplicationID
                LEFT JOIN ofw o ON app.OFWID = o.OFWID
                LEFT JOIN jobplacement jp ON dr.JobPlacementID = jp.JobPlacementID
                LEFT JOIN employer e ON jp.EmployerID = e.EmployerID
                LEFT JOIN agency a ON dr.AgencyID = a.AgencyID
                WHERE dr.AgencyID = " & agencyId & "
                ORDER BY dr.DeploymentDate DESC"

            readQuery(query)
            Dim dt As New DataTable()
            dt.Load(cmdRead)

            Dim fileName As String = "AgencyDeploymentReport-" & DateTime.Now.ToString("yyyyMMdd") & ".pdf"
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
            Dim titleParagraph2 As New Paragraph($"{agencyName} - Deployment Report", titleFont)
            titleParagraph2.Alignment = Element.ALIGN_CENTER
            doc.Add(titleParagraph2)

            doc.Add(New Paragraph(Environment.NewLine))

            ' Deployment Data Table
            AddDeploymentDataTable(doc, dt)
            doc.NewPage()

            ' Summary section
            doc.Add(New Paragraph("Summary", titleFont))
            doc.Add(New Paragraph(Environment.NewLine))

            ' Deployment Status Distribution (One Page)
            Dim statusHeader As New Paragraph("Deployment Status Distribution", titleFont)
            statusHeader.Alignment = Element.ALIGN_CENTER
            statusHeader.SpacingBefore = 20
            doc.Add(statusHeader)
            AddChartToPdf(doc, dt, "DeploymentStatus")
            doc.NewPage()

            ' Country Distribution (One Page)
            Dim countryHeader As New Paragraph("Country Deployment Distribution", titleFont)
            countryHeader.Alignment = Element.ALIGN_CENTER
            countryHeader.SpacingBefore = 20
            doc.Add(countryHeader)
            AddChartToPdf(doc, dt, "CountryOfDeployment")
            doc.NewPage()

            ' Salary and Contract Analysis (Combined Page - Side by Side)
            Dim combinedAnalysisHeader As New Paragraph("Salary & Contract Analysis", titleFont)
            combinedAnalysisHeader.Alignment = Element.ALIGN_CENTER
            combinedAnalysisHeader.SpacingBefore = 20
            doc.Add(combinedAnalysisHeader)
            doc.Add(New Paragraph(Environment.NewLine))

            ' Create a table to hold both analysis side by side
            Dim analysisTable As New PdfPTable(2)
            analysisTable.WidthPercentage = 100
            analysisTable.SetWidths({50, 50})

            ' Left column - Salary Distribution
            Dim salaryCell As New PdfPCell()
            salaryCell.Border = Rectangle.NO_BORDER
            salaryCell.AddElement(New Paragraph("Salary Ranges", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 12)))
            salaryCell.AddElement(GenerateSalaryDistributionTable(dt))
            analysisTable.AddCell(salaryCell)

            ' Right column - Contract Duration Analysis
            Dim contractCell As New PdfPCell()
            contractCell.Border = Rectangle.NO_BORDER
            contractCell.AddElement(New Paragraph("Contract Durations", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 12)))
            contractCell.AddElement(GenerateContractDurationTable(dt))
            analysisTable.AddCell(contractCell)

            doc.Add(analysisTable)
            doc.NewPage()

            ' Repatriation and Performance Analysis (Combined Page - Side by Side)
            Dim repatriationHeader As New Paragraph("Repatriation & Performance Analysis", titleFont)
            repatriationHeader.Alignment = Element.ALIGN_CENTER
            repatriationHeader.SpacingBefore = 20
            doc.Add(repatriationHeader)
            doc.Add(New Paragraph(Environment.NewLine))

            ' Create a table to hold both analysis side by side
            Dim repatTable As New PdfPTable(2)
            repatTable.WidthPercentage = 100
            repatTable.SetWidths({50, 50})

            ' Left column - Repatriation Status
            Dim repatCell As New PdfPCell()
            repatCell.Border = Rectangle.NO_BORDER
            repatCell.AddElement(New Paragraph("Repatriation Status", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 12)))
            repatCell.AddElement(GenerateRepatriationStatusTable(dt))
            repatTable.AddCell(repatCell)

            ' Right column - Top Employers
            Dim employerCell As New PdfPCell()
            employerCell.Border = Rectangle.NO_BORDER
            employerCell.AddElement(New Paragraph("Top Employers", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 12)))
            employerCell.AddElement(GenerateTopEmployersTable(dt))
            repatTable.AddCell(employerCell)

            doc.Add(repatTable)

            doc.Close()
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Sub AddDeploymentDataTable(doc As Document, dt As DataTable)
        If dt.Rows.Count = 0 Then
            doc.Add(New Paragraph("No deployment data available for this agency.", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA, 12)))
            Return
        End If

        ' Create table with selected columns for better readability
        Dim selectedColumns As String() = {"DeploymentID", "OFWName", "JobTitle", "EmployerCompany", "CountryOfDeployment", "Salary", "DeploymentStatus", "ContractDuration", "RepatriationStatus"}
        Dim columnHeaders As String() = {"ID", "OFW Name", "Job Title", "Employer", "Country", "Salary", "Status", "Duration", "Repatriation"}

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
                
                ' Truncate long names and titles
                If (col = "OFWName" OrElse col = "JobTitle" OrElse col = "EmployerCompany") AndAlso cellValue.Length > 20 Then
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

    Private Function GenerateSalaryDistributionTable(dt As DataTable) As PdfPTable
        Dim salaryTable As New PdfPTable(3)
        salaryTable.WidthPercentage = 90
        salaryTable.SpacingBefore = 10
        salaryTable.SpacingAfter = 10
        salaryTable.SetWidths({50, 20, 30})

        ' Table headers
        salaryTable.AddCell(New PdfPCell(New Phrase("Salary Range", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 10))))
        salaryTable.AddCell(New PdfPCell(New Phrase("Count", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 10))))
        salaryTable.AddCell(New PdfPCell(New Phrase("Percentage", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 10))))

        ' Create salary ranges based on actual salary values
        Dim salaryRanges As New Dictionary(Of String, Integer) From {
            {"Below $1,000", 0},
            {"$1,000 - $2,000", 0},
            {"$2,001 - $3,000", 0},
            {"$3,001 - $4,000", 0},
            {"$4,001 - $5,000", 0},
            {"Above $5,000", 0}
        }

        For Each row As DataRow In dt.Rows
            If Not row.IsNull("Salary") AndAlso IsNumeric(row("Salary")) Then
                Dim salary = CDbl(row("Salary"))
                If salary < 1000 Then
                    salaryRanges("Below $1,000") += 1
                ElseIf salary <= 2000 Then
                    salaryRanges("$1,000 - $2,000") += 1
                ElseIf salary <= 3000 Then
                    salaryRanges("$2,001 - $3,000") += 1
                ElseIf salary <= 4000 Then
                    salaryRanges("$3,001 - $4,000") += 1
                ElseIf salary <= 5000 Then
                    salaryRanges("$4,001 - $5,000") += 1
                Else
                    salaryRanges("Above $5,000") += 1
                End If
            End If
        Next

        ' Calculate total for percentage
        Dim totalDeployments As Integer = salaryRanges.Values.Sum()

        ' Add data rows
        For Each salaryRange In salaryRanges
            Dim percentage As Double = If(totalDeployments > 0, (salaryRange.Value / totalDeployments) * 100, 0)
            salaryTable.AddCell(salaryRange.Key)
            salaryTable.AddCell(salaryRange.Value.ToString())
            salaryTable.AddCell(percentage.ToString("F1") & "%")
        Next

        Return salaryTable
    End Function

    Private Function GenerateContractDurationTable(dt As DataTable) As PdfPTable
        Dim durationTable As New PdfPTable(3)
        durationTable.WidthPercentage = 90
        durationTable.SpacingBefore = 10
        durationTable.SpacingAfter = 10
        durationTable.SetWidths({50, 20, 30})

        ' Table headers
        durationTable.AddCell(New PdfPCell(New Phrase("Duration", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 10))))
        durationTable.AddCell(New PdfPCell(New Phrase("Count", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 10))))
        durationTable.AddCell(New PdfPCell(New Phrase("Percentage", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 10))))

        ' Count contract durations
        Dim durationCounts As New Dictionary(Of String, Integer)()
        For Each row As DataRow In dt.Rows
            If Not row.IsNull("ContractDuration") Then
                Dim duration = row("ContractDuration").ToString().Trim()
                If Not String.IsNullOrEmpty(duration) Then
                    If durationCounts.ContainsKey(duration) Then
                        durationCounts(duration) += 1
                    Else
                        durationCounts(duration) = 1
                    End If
                End If
            End If
        Next

        ' Calculate total for percentage
        Dim totalDeployments As Integer = durationCounts.Values.Sum()

        ' Add data rows (top 10)
        Dim topDurations = durationCounts.OrderByDescending(Function(x) x.Value).Take(10).ToList()
        For Each duration In topDurations
            Dim percentage As Double = If(totalDeployments > 0, (duration.Value / totalDeployments) * 100, 0)
            durationTable.AddCell(duration.Key)
            durationTable.AddCell(duration.Value.ToString())
            durationTable.AddCell(percentage.ToString("F1") & "%")
        Next

        Return durationTable
    End Function

    Private Function GenerateRepatriationStatusTable(dt As DataTable) As PdfPTable
        Dim repatTable As New PdfPTable(3)
        repatTable.WidthPercentage = 90
        repatTable.SpacingBefore = 10
        repatTable.SpacingAfter = 10
        repatTable.SetWidths({50, 20, 30})

        ' Table headers
        repatTable.AddCell(New PdfPCell(New Phrase("Status", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 10))))
        repatTable.AddCell(New PdfPCell(New Phrase("Count", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 10))))
        repatTable.AddCell(New PdfPCell(New Phrase("Percentage", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 10))))

        ' Count repatriation statuses
        Dim repatCounts As New Dictionary(Of String, Integer)()
        For Each row As DataRow In dt.Rows
            Dim status As String = If(row.IsNull("RepatriationStatus") OrElse String.IsNullOrEmpty(row("RepatriationStatus").ToString()), "Not Specified", row("RepatriationStatus").ToString().Trim())
            
            If repatCounts.ContainsKey(status) Then
                repatCounts(status) += 1
            Else
                repatCounts(status) = 1
            End If
        Next

        ' Calculate total for percentage
        Dim totalDeployments As Integer = repatCounts.Values.Sum()

        ' Add data rows
        For Each repat In repatCounts.OrderByDescending(Function(x) x.Value)
            Dim percentage As Double = If(totalDeployments > 0, (repat.Value / totalDeployments) * 100, 0)
            repatTable.AddCell(repat.Key)
            repatTable.AddCell(repat.Value.ToString())
            repatTable.AddCell(percentage.ToString("F1") & "%")
        Next

        Return repatTable
    End Function

    Private Function GenerateTopEmployersTable(dt As DataTable) As PdfPTable
        Dim employerTable As New PdfPTable(2)
        employerTable.WidthPercentage = 90
        employerTable.SpacingBefore = 10
        employerTable.SpacingAfter = 10

        employerTable.AddCell(New PdfPCell(New Phrase("Employer", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 10))))
        employerTable.AddCell(New PdfPCell(New Phrase("Deployments", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 10))))

        ' Count employers
        Dim employerCounts As New Dictionary(Of String, Integer)()
        For Each row As DataRow In dt.Rows
            If Not row.IsNull("EmployerCompany") Then
                Dim employer = row("EmployerCompany").ToString().Trim()
                If Not String.IsNullOrEmpty(employer) Then
                    If employerCounts.ContainsKey(employer) Then
                        employerCounts(employer) += 1
                    Else
                        employerCounts(employer) = 1
                    End If
                End If
            End If
        Next

        Dim topEmployers = employerCounts.OrderByDescending(Function(x) x.Value).Take(10).ToList()

        For Each employer In topEmployers
            employerTable.AddCell(employer.Key)
            employerTable.AddCell(employer.Value.ToString())
        Next

        Return employerTable
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
