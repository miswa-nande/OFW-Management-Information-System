Imports System.Globalization
Imports System.Windows.Forms.VisualStyles.VisualStyleElement

Public Class addDeployment
    Private Sub addDeployment_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Use a HashSet to avoid duplicate country names
        Dim countrySet As New HashSet(Of String)()

        ' Loop through all specific cultures
        For Each culture As CultureInfo In CultureInfo.GetCultures(CultureTypes.SpecificCultures)
            Dim region As New RegionInfo(culture.Name)
            countrySet.Add(region.EnglishName)
        Next

        ' Convert to a sorted list
        Dim countryList = countrySet.ToList()
        countryList.Sort()

        ' Add countries to ComboBox
        cbxCountry.Items.AddRange(countryList.ToArray())
    End Sub

End Class