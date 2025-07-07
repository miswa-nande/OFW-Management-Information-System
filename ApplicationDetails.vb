Imports MySql.Data.MySqlClient

Public Class ApplicationDetails
    Private ReadOnly applicationId As Integer

    ' Constructor that accepts ApplicationID
    Public Sub New(appId As Integer)
        InitializeComponent()
        applicationId = appId
    End Sub

    Private Sub ApplicationDetails_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadApplicationDetails()
    End Sub

    Private Sub LoadApplicationDetails()
        Dim query As String = "
    SELECT 
        a.ApplicationStatus, a.ApplicationDate,
        jp.JobTitle, jp.JobDescription, jp.CountryOfEmployment, jp.SalaryRange, 
        jp.EmploymentContractDuration, jp.JobType, jp.VisaType, jp.Conditions, jp.Benefits,
        o.OFWID, o.FirstName, o.MiddleName, o.LastName, o.DOB, o.Sex, o.CivilStatus, 
        o.ContactNum, CONCAT(o.Street, ', ', o.City, ', ', o.Province, ' ', o.Zipcode) AS address,
        o.EducationalLevel, o.PassportNum, o.VISANum, o.OECNum, o.PictureFace,
        GROUP_CONCAT(s.SkillName SEPARATOR ', ') AS skills
    FROM application a
    LEFT JOIN jobplacement jp ON a.JobPlacementID = jp.JobPlacementID
    LEFT JOIN ofw o ON a.OFWID = o.OFWID
    LEFT JOIN ofwskill os ON o.OFWID = os.OFWId
    LEFT JOIN skill s ON os.SkillID = s.SkillID
    WHERE a.ApplicationID = " & applicationId & "
    GROUP BY a.ApplicationID
"


        Try
            readQuery(query)

            If cmdRead.Read() Then
                ' Job Info
                txtbxJobTitle.Text = cmdRead("JobTitle").ToString()
                txtbxJobDescription.Text = cmdRead("JobDescription").ToString()
                txtbxCountry.Text = cmdRead("CountryOfEmployment").ToString()
                txtbxSalaryRange.Text = cmdRead("SalaryRange").ToString()
                txtbxContractDuration.Text = cmdRead("EmploymentContractDuration").ToString()
                txtbxJobType.Text = cmdRead("JobType").ToString()
                txtbxVisaType.Text = cmdRead("VisaType").ToString()
                txtbxConditions.Text = cmdRead("Conditions").ToString()
                txtbxBenefits.Text = cmdRead("Benefits").ToString()

                ' OFW Info
                Label4.Text = cmdRead("OFWID").ToString()
                lblFullName.Text = $"{cmdRead("FirstName")} {cmdRead("MiddleName")} {cmdRead("LastName")}"
                lblDOB.Text = If(IsDBNull(cmdRead("DOB")), "", Convert.ToDateTime(cmdRead("DOB")).ToString("yyyy-MM-dd"))
                lblSex.Text = cmdRead("Sex").ToString()
                lblCivilStat.Text = cmdRead("CivilStatus").ToString()
                lblContactNum.Text = cmdRead("ContactNum").ToString()
                lblFullAddress.Text = cmdRead("address").ToString()
                lblEducLevel.Text = cmdRead("EducationalLevel").ToString()
                lblPassportNum.Text = cmdRead("PassportNum").ToString()
                lblVisaNum.Text = cmdRead("VISANum").ToString()
                lblOecNum.Text = cmdRead("OECNum").ToString()
                lblSkills.Text = cmdRead("skills").ToString()

                ' Profile image (if base64 string is valid)
                If Not IsDBNull(cmdRead("PictureFace")) Then
                    If Not IsDBNull(cmdRead("PictureFace")) Then
                        Dim imgBytes As Byte() = CType(cmdRead("PictureFace"), Byte())
                        If imgBytes IsNot Nothing AndAlso imgBytes.Length > 0 Then
                            Using ms As New IO.MemoryStream(imgBytes)
                                picProfile.Image = Image.FromStream(ms)
                            End Using
                        Else
                            picProfile.Image = Nothing
                        End If
                    Else
                        picProfile.Image = Nothing
                    End If

                Else
                    picProfile.Image = Nothing
                End If
            Else
                MessageBox.Show("Application not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Me.Close()
            End If

            cmdRead.Close()
        Catch ex As Exception
            MessageBox.Show("Error loading details: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnApprove_Click(sender As Object, e As EventArgs) Handles btnApprove.Click
        UpdateApplicationStatus("Accepted")
    End Sub

    Private Sub btnDeny_Click(sender As Object, e As EventArgs) Handles btnDeny.Click
        UpdateApplicationStatus("Rejected")
    End Sub

    Private Sub UpdateApplicationStatus(status As String)
        Dim confirm = MessageBox.Show("Mark this application as " & status & "?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If confirm = DialogResult.Yes Then
            Try
                Dim updateQuery As String = "UPDATE application SET ApplicationStatus = '" & status & "' WHERE ApplicationID = " & applicationId
                readQuery(updateQuery)
                MessageBox.Show("Application marked as " & status, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Me.Close()
            Catch ex As Exception
                MessageBox.Show("Failed to update application: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub
End Class
