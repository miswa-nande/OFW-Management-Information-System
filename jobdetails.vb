Public Class jobdetails
    Private Sub btnApply_Click(sender As Object, e As EventArgs) Handles btnApply.Click
        Dim dlg As New applicationForm()
        dlg.ShowDialog() ' Opens as a modal dialog
    End Sub


End Class