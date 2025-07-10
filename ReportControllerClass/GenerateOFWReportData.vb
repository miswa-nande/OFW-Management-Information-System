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

Public Class GenerateOFWReportData
    Public Function GenerateReport() As Boolean
        Try
            Dim query As String = "
                SELECT FirstName, MiddleName, LastName, DOB, Sex, CivilStatus,
                       City, EducationalLevel, Skills
                FROM ofw"

            readQuery(query)
            Dim dt As New DataTable()
            dt.Load(cmdRead)

            Dim fileName As String = "OFWGeneralReport-" & DateTime.Now.ToString("yyyyMMdd") & ".pdf"
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

            Dim titleParagraph2 As New Paragraph("OFW General Report Data", titleFont)
            titleParagraph2.Alignment = Element.ALIGN_CENTER
            doc.Add(titleParagraph2)

            doc.Add(New Paragraph(Environment.NewLine))

            ' OFW Table
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
            doc.Add(New Paragraph(Environment.NewLine))

            ' Summary section
            doc.Add(New Paragraph("Summary", titleFont))
            doc.Add(New Paragraph(Environment.NewLine))

            ' Sex and Civil Status Distribution (Combined Page - Side by Side)
            Dim chartsHeader As New Paragraph("Distribution Charts", titleFont)
            chartsHeader.Alignment = Element.ALIGN_CENTER
            chartsHeader.SpacingBefore = 20
            doc.Add(chartsHeader)
            doc.Add(New Paragraph(Environment.NewLine))

            ' Create a table to hold both charts side by side
            Dim chartsTable As New PdfPTable(2)
            chartsTable.WidthPercentage = 100
            chartsTable.SetWidths({50, 50})

            ' Left column - Sex Distribution
            Dim sexCell As New PdfPCell()
            sexCell.Border = Rectangle.NO_BORDER
            sexCell.AddElement(New Paragraph("Sex Distribution", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 12)))
            AddChartToPdfCell(sexCell, dt, "Sex")
            chartsTable.AddCell(sexCell)

            ' Right column - Civil Status Distribution
            Dim civilStatusCell As New PdfPCell()
            civilStatusCell.Border = Rectangle.NO_BORDER
            civilStatusCell.AddElement(New Paragraph("Civil Status Distribution", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 12)))
            AddChartToPdfCell(civilStatusCell, dt, "CivilStatus")
            chartsTable.AddCell(civilStatusCell)

            doc.Add(chartsTable)
            doc.NewPage()

            ' City and Skills Distribution (Combined Page - Side by Side)
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
            cityCell.AddElement(GenerateSummaryTable(dt, "City"))
            mainTable.AddCell(cityCell)

            ' Right column - Skills Distribution  
            Dim skillsCell As New PdfPCell()
            skillsCell.Border = Rectangle.NO_BORDER
            skillsCell.AddElement(New Paragraph("Skills Distribution", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 12)))
            skillsCell.AddElement(GenerateSummaryTable(dt, "Skills"))
            mainTable.AddCell(skillsCell)

            doc.Add(mainTable)
            doc.NewPage()

            ' Age Distribution (One Page)
            Dim ageHeader As New Paragraph("Age Distribution", titleFont)
            ageHeader.Alignment = Element.ALIGN_CENTER
            ageHeader.SpacingBefore = 20
            doc.Add(ageHeader)
            AddAgeChartToPdf(doc, dt)

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
                      Where Not row.IsNull(columnName)
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
                      Where Not row.IsNull(columnName)
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

    Private Sub AddAgeChartToPdf(doc As Document, dt As DataTable)
        Dim now As DateTime = DateTime.Today
        Dim ageGroups As New Dictionary(Of String, Integer) From {
            {"18-30", 0}, {"31-45", 0}, {"46-60", 0}, {"61+", 0}
        }

        For Each row As DataRow In dt.Rows
            If Not row.IsNull("DOB") Then
                Dim dob As Date = Convert.ToDateTime(row("DOB"))
                Dim age As Integer = now.Year - dob.Year
                If dob > now.AddYears(-age) Then age -= 1

                If age <= 30 Then
                    ageGroups("18-30") += 1
                ElseIf age <= 45 Then
                    ageGroups("31-45") += 1
                ElseIf age <= 60 Then
                    ageGroups("46-60") += 1
                Else
                    ageGroups("61+") += 1
                End If
            End If
        Next

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

        For Each group In ageGroups
            series.Points.AddXY(group.Key, group.Value)
            series.Points(series.Points.Count - 1).Label = group.Key & " (" & group.Value & ")"
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

    Private Function GenerateSummaryTable(dt As DataTable, columnName As String) As PdfPTable
        Dim summaryTable As New PdfPTable(2)
        summaryTable.WidthPercentage = 50
        summaryTable.SpacingBefore = 10
        summaryTable.SpacingAfter = 10

        summaryTable.AddCell(New PdfPCell(New Phrase(columnName, iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 10))))
        summaryTable.AddCell(New PdfPCell(New Phrase("Count", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 10))))

        Dim grouped = From row In dt.AsEnumerable()
                      Where Not row.IsNull(columnName)
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
