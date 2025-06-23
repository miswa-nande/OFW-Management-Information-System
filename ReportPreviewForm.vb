Imports Microsoft.Reporting.WinForms

Public Class ReportPreviewForm
    Private reportData As DataTable

    Public Sub New(dt As DataTable)
        InitializeComponent()
        reportData = dt
    End Sub

    Private Sub ReportPreviewForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Set the processing mode for the ReportViewer to Local
        ReportViewer1.ProcessingMode = ProcessingMode.Local
        ' Set the path to your RDLC file
        ReportViewer1.LocalReport.ReportPath = "OFWReport.rdlc"

        ' Clear any existing data sources
        ReportViewer1.LocalReport.DataSources.Clear()

        ' Add the new data source
        Dim rds As New ReportDataSource("OFWData", reportData)
        ReportViewer1.LocalReport.DataSources.Add(rds)

        ' Refresh the report
        ReportViewer1.RefreshReport()
    End Sub
    ' Export to PDF button
    Private Sub btnExportPDF_Click(sender As Object, e As EventArgs) Handles btnExportPDF.Click
        Try
            Dim saveFileDialog As New SaveFileDialog()
            saveFileDialog.Filter = "PDF files (*.pdf)|*.pdf"
            saveFileDialog.Title = "Export Report as PDF"
            saveFileDialog.FileName = "OFW_Report.pdf"
            If saveFileDialog.ShowDialog() = DialogResult.OK Then
                Dim bytes As Byte() = ReportViewer1.LocalReport.Render("PDF")
                System.IO.File.WriteAllBytes(saveFileDialog.FileName, bytes)
                MessageBox.Show("Report exported successfully!", "Export to PDF", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Catch ex As Exception
            MessageBox.Show("Error exporting PDF: " & ex.Message, "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

End Class