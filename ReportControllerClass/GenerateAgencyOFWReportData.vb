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

Public Class GenerateAgencyOFWReportData
    Public Function GenerateReport() As Boolean
        Try
            Dim agencyId As Integer = Session.CurrentReferenceID
            
            Dim query As String = "
                SELECT 
                    o.OFWId,
                    CONCAT(o.FirstName, ' ', o.MiddleName, ' ', o.LastName) AS FullName,
                    o.FirstName,
                    o.MiddleName,
                    o.LastName,
                    o.Sex,
                    TIMESTAMPDIFF(YEAR, o.DOB, CURDATE()) AS Age,
                    o.CivilStatus,
                    CONCAT(o.Street, ', ', o.Barangay, ', ', o.City, ', ', o.Province, ' ', o.Zipcode) AS Address,
                    o.City,
                    o.Province,
                    o.ContactNum,
                    o.Skills,
                    o.VISANum,
                    o.OECNum,
                    o.PassportNum,
                    COUNT(DISTINCT dr.DeploymentID) AS TotalDeployments,
                    COUNT(DISTINCT app.ApplicationID) AS TotalApplications,
                    a.AgencyName,
                    o.DateAdded
                FROM ofw o
                LEFT JOIN agency a ON o.AgencyID = a.AgencyID
                LEFT JOIN application app ON o.OFWId = app.OFWID
                LEFT JOIN deploymentrecord dr ON app.ApplicationID = dr.ApplicationID
                WHERE o.AgencyID = " & agencyId & "
                GROUP BY o.OFWId
                ORDER BY o.DateAdded DESC"

            readQuery(query)
            Dim dt As New DataTable()
            dt.Load(cmdRead)

            Dim fileName As String = "AgencyOFWReport-" & DateTime.Now.ToString("yyyyMMdd") & ".pdf"
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
            Dim titleParagraph2 As New Paragraph($"{agencyName} - OFW Report", titleFont)
            titleParagraph2.Alignment = Element.ALIGN_CENTER
            doc.Add(titleParagraph2)

            doc.Add(New Paragraph(Environment.NewLine))

            ' OFW Data Table
            AddOFWDataTable(doc, dt)
            doc.NewPage()

            ' Summary section
            doc.Add(New Paragraph("Summary", titleFont))
            doc.Add(New Paragraph(Environment.NewLine))

            ' Sex Distribution (One Page)
            Dim sexHeader As New Paragraph("Sex Distribution", titleFont)
            sexHeader.Alignment = Element.ALIGN_CENTER
            sexHeader.SpacingBefore = 20
            doc.Add(sexHeader)
            AddChartToPdf(doc, dt, "Sex")
            doc.NewPage()

            ' Civil Status Distribution (One Page)
            Dim civilHeader As New Paragraph("Civil Status Distribution", titleFont)
            civilHeader.Alignment = Element.ALIGN_CENTER
            civilHeader.SpacingBefore = 20
            doc.Add(civilHeader)
            AddChartToPdf(doc, dt, "CivilStatus")
            doc.NewPage()

            ' Geographic and Skills Analysis (Combined Page - Side by Side)
            Dim combinedAnalysisHeader As New Paragraph("Geographic & Skills Analysis", titleFont)
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
            cityCell.AddElement(New Paragraph("Top Cities by OFWs", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 12)))
            cityCell.AddElement(GenerateCityDistributionTable(dt))
            analysisTable.AddCell(cityCell)

            ' Right column - Skills Analysis
            Dim skillsCell As New PdfPCell()
            skillsCell.Border = Rectangle.NO_BORDER
            skillsCell.AddElement(New Paragraph("Top Skills", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 12)))
            skillsCell.AddElement(GenerateSkillsAnalysisTable(dt))
            analysisTable.AddCell(skillsCell)

            doc.Add(analysisTable)
            doc.NewPage()

            ' Age and Performance Analysis (Combined Page - Side by Side)
            Dim agePerformanceHeader As New Paragraph("Age & Performance Analysis", titleFont)
            agePerformanceHeader.Alignment = Element.ALIGN_CENTER
            agePerformanceHeader.SpacingBefore = 20
            doc.Add(agePerformanceHeader)
            doc.Add(New Paragraph(Environment.NewLine))

            ' Create a table to hold both analysis side by side
            Dim ageTable As New PdfPTable(2)
            ageTable.WidthPercentage = 100
            ageTable.SetWidths({50, 50})

            ' Left column - Age Distribution
            Dim ageCell As New PdfPCell()
            ageCell.Border = Rectangle.NO_BORDER
            ageCell.AddElement(New Paragraph("Age Distribution", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 12)))
            ageCell.AddElement(GenerateAgeDistributionTable(dt))
            ageTable.AddCell(ageCell)

            ' Right column - Deployment Activity
            Dim deploymentCell As New PdfPCell()
            deploymentCell.Border = Rectangle.NO_BORDER
            deploymentCell.AddElement(New Paragraph("Deployment Activity", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 12)))
            deploymentCell.AddElement(GenerateDeploymentActivityTable(dt))
            ageTable.AddCell(deploymentCell)

            doc.Add(ageTable)

            doc.Close()
            Return True
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
            Return False
        End Try
    End Function

    Private Sub AddOFWDataTable(doc As Document, dt As DataTable)
        If dt.Rows.Count = 0 Then
            doc.Add(New Paragraph("No OFW data available for this agency.", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA, 12)))
            Return
        End If

        ' Create table with selected columns for better readability
        Dim selectedColumns As String() = {"OFWId", "FullName", "Sex", "Age", "CivilStatus", "City", "ContactNum", "Skills", "TotalDeployments", "TotalApplications"}
        Dim columnHeaders As String() = {"ID", "Full Name", "Sex", "Age", "Civil Status", "City", "Contact", "Skills", "Deploy.", "Apps"}

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
                
                ' Truncate skills if too long
                If col = "Skills" AndAlso cellValue.Length > 20 Then
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

    Private Function GenerateSkillsAnalysisTable(dt As DataTable) As PdfPTable
        Dim skillsTable As New PdfPTable(2)
        skillsTable.WidthPercentage = 90
        skillsTable.SpacingBefore = 10
        skillsTable.SpacingAfter = 10

        skillsTable.AddCell(New PdfPCell(New Phrase("Skill", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 10))))
        skillsTable.AddCell(New PdfPCell(New Phrase("Count", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 10))))

        ' Parse and count skills
        Dim skillCounts As New Dictionary(Of String, Integer)()
        For Each row As DataRow In dt.Rows
            If Not row.IsNull("Skills") Then
                Dim skillsString = row("Skills").ToString().Trim()
                If Not String.IsNullOrEmpty(skillsString) Then
                    ' Split by common delimiters
                    Dim skills = skillsString.Split({","c, ";"c, "|"c}, StringSplitOptions.RemoveEmptyEntries)
                    For Each skill In skills
                        Dim cleanSkill = skill.Trim()
                        If Not String.IsNullOrEmpty(cleanSkill) Then
                            If skillCounts.ContainsKey(cleanSkill) Then
                                skillCounts(cleanSkill) += 1
                            Else
                                skillCounts(cleanSkill) = 1
                            End If
                        End If
                    Next
                End If
            End If
        Next

        Dim topSkills = skillCounts.OrderByDescending(Function(x) x.Value).Take(10).ToList()

        For Each skill In topSkills
            skillsTable.AddCell(skill.Key)
            skillsTable.AddCell(skill.Value.ToString())
        Next

        Return skillsTable
    End Function

    Private Function GenerateAgeDistributionTable(dt As DataTable) As PdfPTable
        Dim ageTable As New PdfPTable(3)
        ageTable.WidthPercentage = 90
        ageTable.SpacingBefore = 10
        ageTable.SpacingAfter = 10
        ageTable.SetWidths({50, 20, 30})

        ' Table headers
        ageTable.AddCell(New PdfPCell(New Phrase("Age Range", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 10))))
        ageTable.AddCell(New PdfPCell(New Phrase("Count", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 10))))
        ageTable.AddCell(New PdfPCell(New Phrase("Percentage", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 10))))

        ' Create age groups
        Dim ageRanges As New Dictionary(Of String, Integer) From {
            {"18-25 years", 0},
            {"26-35 years", 0},
            {"36-45 years", 0},
            {"46-55 years", 0},
            {"55+ years", 0}
        }

        For Each row As DataRow In dt.Rows
            If Not row.IsNull("Age") AndAlso IsNumeric(row("Age")) Then
                Dim age = CInt(row("Age"))
                If age <= 25 Then
                    ageRanges("18-25 years") += 1
                ElseIf age <= 35 Then
                    ageRanges("26-35 years") += 1
                ElseIf age <= 45 Then
                    ageRanges("36-45 years") += 1
                ElseIf age <= 55 Then
                    ageRanges("46-55 years") += 1
                Else
                    ageRanges("55+ years") += 1
                End If
            End If
        Next

        ' Calculate total for percentage
        Dim totalOFWs As Integer = ageRanges.Values.Sum()

        ' Add data rows
        For Each ageRange In ageRanges
            Dim percentage As Double = If(totalOFWs > 0, (ageRange.Value / totalOFWs) * 100, 0)
            ageTable.AddCell(ageRange.Key)
            ageTable.AddCell(ageRange.Value.ToString())
            ageTable.AddCell(percentage.ToString("F1") & "%")
        Next

        Return ageTable
    End Function

    Private Function GenerateDeploymentActivityTable(dt As DataTable) As PdfPTable
        Dim deployTable As New PdfPTable(3)
        deployTable.WidthPercentage = 90
        deployTable.SpacingBefore = 10
        deployTable.SpacingAfter = 10
        deployTable.SetWidths({50, 20, 30})

        ' Table headers
        deployTable.AddCell(New PdfPCell(New Phrase("Activity Level", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 10))))
        deployTable.AddCell(New PdfPCell(New Phrase("Count", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 10))))
        deployTable.AddCell(New PdfPCell(New Phrase("Percentage", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 10))))

        ' Calculate deployment activity levels
        Dim activityLevels As New Dictionary(Of String, Integer) From {
            {"Highly Active (3+ Deployments)", 0},
            {"Active (2 Deployments)", 0},
            {"Limited Activity (1 Deployment)", 0},
            {"New/No Deployments (0)", 0}
        }

        For Each row As DataRow In dt.Rows
            Dim deployments = If(row.IsNull("TotalDeployments"), 0, CInt(row("TotalDeployments")))
            
            If deployments >= 3 Then
                activityLevels("Highly Active (3+ Deployments)") += 1
            ElseIf deployments = 2 Then
                activityLevels("Active (2 Deployments)") += 1
            ElseIf deployments = 1 Then
                activityLevels("Limited Activity (1 Deployment)") += 1
            Else
                activityLevels("New/No Deployments (0)") += 1
            End If
        Next

        ' Calculate total for percentage
        Dim totalOFWs As Integer = activityLevels.Values.Sum()

        ' Add data rows
        For Each activity In activityLevels
            Dim percentage As Double = If(totalOFWs > 0, (activity.Value / totalOFWs) * 100, 0)
            deployTable.AddCell(activity.Key)
            deployTable.AddCell(activity.Value.ToString())
            deployTable.AddCell(percentage.ToString("F1") & "%")
        Next

        Return deployTable
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
