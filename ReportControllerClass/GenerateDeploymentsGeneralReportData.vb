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

Public Class GenerateDeploymentsGeneralReportData
    Public Function GenerateReport() As Boolean
        Try
            Dim query As String = "
                SELECT dr.DeploymentDate, dr.CountryOfDeployment, dr.Salary,
                       dr.MedicalCleared, dr.VisaIssued, dr.POEACleared, dr.PDOSCompleted,
                       dr.FlightNumber, dr.Airport, dr.DeploymentRemarks, dr.ContractDuration,
                       dr.ContractNumber, dr.DeploymentStatus, dr.ContractStartDate, dr.ContractEndDate,
                       dr.RepatriationStatus, dr.ReasonForReturn, dr.ReturnDate,
                       CONCAT(o.FirstName, ' ', o.MiddleName, ' ', o.LastName) AS OFWName,
                       jp.JobTitle, e.CompanyName AS EmployerCompany, a.AgencyName
                FROM deploymentrecord dr
                LEFT JOIN application app ON dr.ApplicationID = app.ApplicationID
                LEFT JOIN ofw o ON app.OFWID = o.OFWID
                LEFT JOIN jobplacement jp ON dr.JobPlacementID = jp.JobPlacementID
                LEFT JOIN employer e ON jp.EmployerID = e.EmployerID
                LEFT JOIN agency a ON dr.AgencyID = a.AgencyID"

            readQuery(query)
            Dim dt As New DataTable()
            dt.Load(cmdRead)

            Dim fileName As String = "DeploymentsGeneralReport-" & DateTime.Now.ToString("yyyyMMdd") & ".pdf"
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

            Dim titleParagraph2 As New Paragraph("Deployments General Report Data", titleFont)
            titleParagraph2.Alignment = Element.ALIGN_CENTER
            doc.Add(titleParagraph2)

            doc.Add(New Paragraph(Environment.NewLine))

            ' Deployments Table
            Dim table As New PdfPTable(dt.Columns.Count)
            table.WidthPercentage = 100

            For Each col As DataColumn In dt.Columns
                table.AddCell(New PdfPCell(New Phrase(col.ColumnName, iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 7))))
            Next

            For Each row As DataRow In dt.Rows
                For Each col As DataColumn In dt.Columns
                    table.AddCell(New PdfPCell(New Phrase(row(col.ColumnName).ToString(), iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA, 6))))
                Next
            Next

            doc.Add(table)
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
            Dim countryHeader As New Paragraph("Country of Deployment Distribution", titleFont)
            countryHeader.Alignment = Element.ALIGN_CENTER
            countryHeader.SpacingBefore = 20
            doc.Add(countryHeader)
            AddChartToPdf(doc, dt, "CountryOfDeployment")
            doc.NewPage()

            ' Repatriation Status and Reason for Return (Combined Page - Side by Side)
            Dim combinedChartsHeader As New Paragraph("Repatriation Analysis", titleFont)
            combinedChartsHeader.Alignment = Element.ALIGN_CENTER
            combinedChartsHeader.SpacingBefore = 20
            doc.Add(combinedChartsHeader)
            doc.Add(New Paragraph(Environment.NewLine))

            ' Create a table to hold both charts side by side
            Dim chartsTable As New PdfPTable(2)
            chartsTable.WidthPercentage = 100
            chartsTable.SetWidths({50, 50})

            ' Left column - Repatriation Status
            Dim repatCell As New PdfPCell()
            repatCell.Border = Rectangle.NO_BORDER
            repatCell.AddElement(New Paragraph("Repatriation Status", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 12)))
            AddChartToPdfCell(repatCell, dt, "RepatriationStatus")
            chartsTable.AddCell(repatCell)

            ' Right column - Reason for Return
            Dim reasonCell As New PdfPCell()
            reasonCell.Border = Rectangle.NO_BORDER
            reasonCell.AddElement(New Paragraph("Reason for Return", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 12)))
            AddChartToPdfCell(reasonCell, dt, "ReasonForReturn")
            chartsTable.AddCell(reasonCell)

            doc.Add(chartsTable)
            doc.NewPage()

            ' Compliance Requirements Analysis (Combined Page - Side by Side)
            Dim complianceTablesHeader As New Paragraph("Compliance Requirements Analysis", titleFont)
            complianceTablesHeader.Alignment = Element.ALIGN_CENTER
            complianceTablesHeader.SpacingBefore = 20
            doc.Add(complianceTablesHeader)
            doc.Add(New Paragraph(Environment.NewLine))

            ' Create a table to hold both summary tables side by side
            Dim mainTable As New PdfPTable(2)
            mainTable.WidthPercentage = 100
            mainTable.SetWidths({50, 50})

            ' Left column - Medical/Visa Compliance
            Dim complianceCell As New PdfPCell()
            complianceCell.Border = Rectangle.NO_BORDER
            complianceCell.AddElement(New Paragraph("Compliance Requirements", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 12)))
            complianceCell.AddElement(GenerateComplianceTable(dt))
            mainTable.AddCell(complianceCell)

            ' Right column - Salary Range Distribution
            Dim salaryCell As New PdfPCell()
            salaryCell.Border = Rectangle.NO_BORDER
            salaryCell.AddElement(New Paragraph("Salary Range Distribution", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 12)))
            salaryCell.AddElement(GenerateSalaryRangeTable(dt))
            mainTable.AddCell(salaryCell)

            doc.Add(mainTable)
            doc.NewPage()

            ' Contract Duration Analysis (One Page)
            Dim durationHeader As New Paragraph("Contract Duration Analysis", titleFont)
            durationHeader.Alignment = Element.ALIGN_CENTER
            durationHeader.SpacingBefore = 20
            doc.Add(durationHeader)
            doc.Add(New Paragraph(Environment.NewLine))
            doc.Add(GenerateContractDurationTable(dt))

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

    Private Function GenerateComplianceTable(dt As DataTable) As PdfPTable
        Dim complianceTable As New PdfPTable(2)
        complianceTable.WidthPercentage = 90
        complianceTable.SpacingBefore = 10
        complianceTable.SpacingAfter = 10

        complianceTable.AddCell(New PdfPCell(New Phrase("Requirement", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 10))))
        complianceTable.AddCell(New PdfPCell(New Phrase("Compliance Rate", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 10))))

        ' Calculate compliance rates
        Dim requirements As String() = {"MedicalCleared", "VisaIssued", "POEACleared", "PDOSCompleted"}
        Dim requirementNames As String() = {"Medical Clearance", "Visa Issued", "POEA Clearance", "PDOS Completed"}

        For i As Integer = 0 To requirements.Length - 1
            Dim totalRecords As Integer = dt.Rows.Count
            Dim compliantRecords As Integer = 0

            For Each row As DataRow In dt.Rows
                If Not row.IsNull(requirements(i)) Then
                    Dim value As String = row(requirements(i)).ToString().ToLower()
                    If value = "true" Or value = "1" Or value = "yes" Then
                        compliantRecords += 1
                    End If
                End If
            Next

            Dim complianceRate As Double = If(totalRecords > 0, (compliantRecords / totalRecords) * 100, 0)
            complianceTable.AddCell(requirementNames(i))
            complianceTable.AddCell(complianceRate.ToString("F1") & "%")
        Next

        Return complianceTable
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
            If Not row.IsNull("Salary") Then
                Dim salary As Integer = 0
                If Integer.TryParse(row("Salary").ToString(), salary) Then
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

    Private Function GenerateContractDurationTable(dt As DataTable) As PdfPTable
        Dim durationTable As New PdfPTable(3)
        durationTable.WidthPercentage = 80
        durationTable.SetWidths({50, 20, 30})
        durationTable.SpacingBefore = 10
        durationTable.SpacingAfter = 10
        durationTable.HorizontalAlignment = Element.ALIGN_CENTER

        ' Table headers
        durationTable.AddCell(New PdfPCell(New Phrase("Contract Duration", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 12))))
        durationTable.AddCell(New PdfPCell(New Phrase("Count", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 12))))
        durationTable.AddCell(New PdfPCell(New Phrase("Percentage", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 12))))

        ' Create duration groups
        Dim durationGroups As New Dictionary(Of String, Integer) From {
            {"Less than 1 year", 0},
            {"1-2 years", 0},
            {"2-3 years", 0},
            {"3+ years", 0}
        }

        For Each row As DataRow In dt.Rows
            If Not row.IsNull("ContractDuration") Then
                Dim duration As Integer = 0
                If Integer.TryParse(row("ContractDuration").ToString(), duration) Then
                    If duration < 12 Then
                        durationGroups("Less than 1 year") += 1
                    ElseIf duration <= 24 Then
                        durationGroups("1-2 years") += 1
                    ElseIf duration <= 36 Then
                        durationGroups("2-3 years") += 1
                    Else
                        durationGroups("3+ years") += 1
                    End If
                End If
            End If
        Next

        ' Calculate total for percentage
        Dim totalDeployments As Integer = durationGroups.Values.Sum()

        ' Add data rows
        For Each group In durationGroups
            Dim percentage As Double = If(totalDeployments > 0, (group.Value / totalDeployments) * 100, 0)
            durationTable.AddCell(group.Key)
            durationTable.AddCell(group.Value.ToString())
            durationTable.AddCell(percentage.ToString("F1") & "%")
        Next

        ' Add total row
        Dim totalCell As New PdfPCell(New Phrase("Total", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 10)))
        totalCell.BackgroundColor = BaseColor.LIGHT_GRAY
        durationTable.AddCell(totalCell)

        Dim totalCountCell As New PdfPCell(New Phrase(totalDeployments.ToString(), iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 10)))
        totalCountCell.BackgroundColor = BaseColor.LIGHT_GRAY
        durationTable.AddCell(totalCountCell)

        Dim totalPercentCell As New PdfPCell(New Phrase("100.0%", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 10)))
        totalPercentCell.BackgroundColor = BaseColor.LIGHT_GRAY
        durationTable.AddCell(totalPercentCell)

        Return durationTable
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
