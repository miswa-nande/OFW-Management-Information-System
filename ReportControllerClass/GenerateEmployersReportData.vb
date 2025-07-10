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

Public Class GenerateEmployersReportData
    Public Function GenerateReport() As Boolean
        Try
            Dim query As String = "
                SELECT EmployerID, EmployerFirstName, EmployerMiddleName, EmployerLastName,
                       EmployerEmail, EmployerContactNum, CompanyName, Industry,
                       CompanyCity, CompanyState, CompanyCountry, NumOfOFWHired,
                       ActiveJobPlacement, DateAdded
                FROM employer"

            readQuery(query)
            Dim dt As New DataTable()
            dt.Load(cmdRead)

            Dim fileName As String = "EmployersGeneralReport-" & DateTime.Now.ToString("yyyyMMdd") & ".pdf"
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

            Dim titleParagraph2 As New Paragraph("Employers General Report Data", titleFont)
            titleParagraph2.Alignment = Element.ALIGN_CENTER
            doc.Add(titleParagraph2)

            doc.Add(New Paragraph(Environment.NewLine))

            ' Employers Table
            Dim table As New PdfPTable(dt.Columns.Count)
            table.WidthPercentage = 100

            For Each col As DataColumn In dt.Columns
                table.AddCell(New PdfPCell(New Phrase(col.ColumnName, iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 9))))
            Next

            For Each row As DataRow In dt.Rows
                For Each col As DataColumn In dt.Columns
                    table.AddCell(row(col.ColumnName).ToString())
                Next
            Next

            doc.Add(table)
            doc.NewPage()

            ' Summary section
            doc.Add(New Paragraph("Summary", titleFont))
            doc.Add(New Paragraph(Environment.NewLine))

            ' Industry Distribution (One Page)
            Dim industryHeader As New Paragraph("Industry Distribution", titleFont)
            industryHeader.Alignment = Element.ALIGN_CENTER
            industryHeader.SpacingBefore = 20
            doc.Add(industryHeader)
            AddChartToPdf(doc, dt, "Industry")
            doc.NewPage()

            ' Country Distribution (One Page)
            Dim countryHeader As New Paragraph("Country Distribution", titleFont)
            countryHeader.Alignment = Element.ALIGN_CENTER
            countryHeader.SpacingBefore = 20
            doc.Add(countryHeader)
            AddChartToPdf(doc, dt, "CompanyCountry")
            doc.NewPage()

            ' City and Employer Status Distribution (Combined Page - Side by Side)
            Dim combinedHeader As New Paragraph("Distribution Tables", titleFont)
            combinedHeader.Alignment = Element.ALIGN_CENTER
            combinedHeader.SpacingBefore = 20
            doc.Add(combinedHeader)
            doc.Add(New Paragraph(Environment.NewLine))

            ' Create a table to hold both summary tables side by side
            Dim mainTable As New PdfPTable(2)
            mainTable.WidthPercentage = 100
            mainTable.SetWidths({50, 50})

            ' Left column - City Distribution
            Dim cityCell As New PdfPCell()
            cityCell.Border = Rectangle.NO_BORDER
            cityCell.AddElement(New Paragraph("City Distribution", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 12)))
            cityCell.AddElement(GenerateSummaryTable(dt, "CompanyCity"))
            mainTable.AddCell(cityCell)

            ' Right column - State Distribution  
            Dim stateCell As New PdfPCell()
            stateCell.Border = Rectangle.NO_BORDER
            stateCell.AddElement(New Paragraph("State Distribution", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 12)))
            stateCell.AddElement(GenerateSummaryTable(dt, "CompanyState"))
            mainTable.AddCell(stateCell)

            doc.Add(mainTable)
            doc.NewPage()

            ' Employer Activity Distribution (One Page)
            Dim activityHeader As New Paragraph("Employer Activity Distribution", titleFont)
            activityHeader.Alignment = Element.ALIGN_CENTER
            activityHeader.SpacingBefore = 20
            doc.Add(activityHeader)
            doc.Add(New Paragraph(Environment.NewLine))
            doc.Add(GenerateEmployerActivityTable(dt))

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

    Private Function GenerateEmployerActivityTable(dt As DataTable) As PdfPTable
        ' Create activity groups based on number of OFW hired
        Dim activityGroups As New Dictionary(Of String, Integer) From {
            {"Low Activity (0-5 OFWs)", 0},
            {"Medium Activity (6-20 OFWs)", 0},
            {"High Activity (21-50 OFWs)", 0},
            {"Very High Activity (50+ OFWs)", 0}
        }

        For Each row As DataRow In dt.Rows
            If Not row.IsNull("NumOfOFWHired") Then
                Dim numOfws As Integer = 0
                If Integer.TryParse(row("NumOfOFWHired").ToString(), numOfws) Then
                    If numOfws <= 5 Then
                        activityGroups("Low Activity (0-5 OFWs)") += 1
                    ElseIf numOfws <= 20 Then
                        activityGroups("Medium Activity (6-20 OFWs)") += 1
                    ElseIf numOfws <= 50 Then
                        activityGroups("High Activity (21-50 OFWs)") += 1
                    Else
                        activityGroups("Very High Activity (50+ OFWs)") += 1
                    End If
                End If
            End If
        Next

        Dim activityTable As New PdfPTable(3)
        activityTable.WidthPercentage = 80
        activityTable.SetWidths({50, 20, 30})
        activityTable.SpacingBefore = 10
        activityTable.SpacingAfter = 10
        activityTable.HorizontalAlignment = Element.ALIGN_CENTER

        ' Table headers
        activityTable.AddCell(New PdfPCell(New Phrase("Activity Level", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 12))))
        activityTable.AddCell(New PdfPCell(New Phrase("Count", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 12))))
        activityTable.AddCell(New PdfPCell(New Phrase("Percentage", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 12))))

        ' Calculate total for percentage
        Dim totalEmployers As Integer = activityGroups.Values.Sum()

        ' Add data rows
        For Each group In activityGroups
            Dim percentage As Double = If(totalEmployers > 0, (group.Value / totalEmployers) * 100, 0)
            activityTable.AddCell(group.Key)
            activityTable.AddCell(group.Value.ToString())
            activityTable.AddCell(percentage.ToString("F1") & "%")
        Next

        ' Add total row
        Dim totalCell As New PdfPCell(New Phrase("Total", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 10)))
        totalCell.BackgroundColor = BaseColor.LIGHT_GRAY
        activityTable.AddCell(totalCell)

        Dim totalCountCell As New PdfPCell(New Phrase(totalEmployers.ToString(), iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 10)))
        totalCountCell.BackgroundColor = BaseColor.LIGHT_GRAY
        activityTable.AddCell(totalCountCell)

        Dim totalPercentCell As New PdfPCell(New Phrase("100.0%", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 10)))
        totalPercentCell.BackgroundColor = BaseColor.LIGHT_GRAY
        activityTable.AddCell(totalPercentCell)

        Return activityTable
    End Function

    Private Function GenerateSummaryTable(dt As DataTable, columnName As String) As PdfPTable
        Dim summaryTable As New PdfPTable(2)
        summaryTable.WidthPercentage = 50
        summaryTable.SpacingBefore = 10
        summaryTable.SpacingAfter = 10

        summaryTable.AddCell(New PdfPCell(New Phrase(columnName, iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 10))))
        summaryTable.AddCell(New PdfPCell(New Phrase("Count", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 10))))

        Dim grouped = From row In dt.AsEnumerable()
                      Where Not row.IsNull(columnName) AndAlso Not String.IsNullOrEmpty(row.Field(Of String)(columnName))
                      Group row By key = row.Field(Of String)(columnName) Into Group
                      Select key, Count = Group.Count()

        For Each item In grouped
            summaryTable.AddCell(item.key)
            summaryTable.AddCell(item.Count.ToString())
        Next

        Return summaryTable
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
