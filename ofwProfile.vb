Imports MySql.Data.MySqlClient
Imports System.IO

Public Class ofwProfile
    Public Shared Instance As ofwProfile

    Private Sub ofwProfile_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Instance = Me
        LoadOFWProfile()
    End Sub

    Public Sub LoadOFWProfile()
        Dim query As String = $"SELECT * FROM ofw WHERE OFWId = {Session.CurrentReferenceID}"
        readQuery(query)

        If cmdRead IsNot Nothing AndAlso cmdRead.Read() Then
            ' Load text labels
            Label10.Text = cmdRead("OFWId").ToString()
            lblFullName.Text = $"{cmdRead("FirstName")} {cmdRead("LastName")}"
            lblDOB.Text = Convert.ToDateTime(cmdRead("DOB")).ToString("MMMM dd, yyyy")
            lblCivStat.Text = cmdRead("CivilStatus").ToString()
            lblSex.Text = cmdRead("Sex").ToString()
            lblContactNum.Text = cmdRead("ContactNum").ToString()
            lblEducLvl.Text = cmdRead("EducationalLevel").ToString()
            lblPassportNum.Text = cmdRead("PassportNum").ToString()
            lblSkills.Text = cmdRead("Skills").ToString()
            lblVisaNum.Text = cmdRead("VISANum").ToString()
            lblRegisteredAgency.Text = cmdRead("EmploymentStatus").ToString()
            lblOemNum.Text = cmdRead("OECNum").ToString()

            ' Combine address fields
            Dim fullAddress As String = $"{cmdRead("Street")}, {cmdRead("Barangay")}, {cmdRead("City")}, {cmdRead("Province")} {cmdRead("Zipcode")}"
            lblFullAddress.Text = fullAddress

            ' Load image from BLOB
            If Not IsDBNull(cmdRead("PictureFace")) Then
                Dim imageData As Byte() = CType(cmdRead("PictureFace"), Byte())
                Using ms As New MemoryStream(imageData)
                    picProfile.Image = Image.FromStream(ms)
                End Using
            Else
                picProfile.Image = Nothing ' or load a default image
            End If
        End If

        cmdRead?.Close()
    End Sub

    ' Navigation Buttons
    Private Sub btnJobOffers_Click(sender As Object, e As EventArgs) Handles btnJobOffers.Click
        Dim newForm As New joboffers()
        newForm.Show()
        Me.Hide()
    End Sub

    Private Sub btnDeployment_Click(sender As Object, e As EventArgs) Handles btnDeployment.Click
        Dim newForm As New deploymentrecords()
        newForm.Show()
        Me.Hide()
    End Sub

    Private Sub btnApplications_Click(sender As Object, e As EventArgs) Handles btnApplications.Click
        Dim newForm As New applications()
        newForm.Show()
        Me.Hide()
    End Sub

    Private Sub EditProfile_Click(sender As Object, e As EventArgs) Handles EditProfile.Click
        Dim editForm As New editOfw()
        editForm.ShowDialog()

    End Sub

    Private Sub LogOut_Click(sender As Object, e As EventArgs) Handles LogOut.Click
        Dim loginForm As New loginPage()
        loginForm.Show()
        Me.Close()
    End Sub

    Private Sub ApplicationLetter_Click(sender As Object, e As EventArgs) Handles ApplicationLetter.Click
        Dim newForm As New ApplicationLetter()
        newForm.ShowDialog()
    End Sub

    Private Sub ChangeAgencyBTN_Click(sender As Object, e As EventArgs) Handles ChangeAgencyBTN.Click
        Dim newForm As New registeredAgency()
        newForm.ShowDialog()
    End Sub
End Class
