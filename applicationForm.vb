Imports MySql.Data.MySqlClient
Imports System.IO

Public Class applicationForm
    Private jobID As Integer
    Private isViewMode As Boolean = False

    ' Constructor
    Public Sub New(jobId As Integer, Optional viewMode As Boolean = False)
        InitializeComponent()
        Me.jobID = jobId
        Me.isViewMode = viewMode
    End Sub

    Private Sub applicationForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadJobDetails()
        LoadOfwProfile()

        If isViewMode Then
            btnApply.Visible = False
            btnCancel.Text = "Close"
            txtbxJobTitle.ReadOnly = True
            txtbxJobDescription.ReadOnly = True
            txtbxSalaryRange.ReadOnly = True
            txtbxCountry.ReadOnly = True
            txtbxContractDuration.ReadOnly = True
            txtbxJobType.ReadOnly = True
            txtbxVisaType.ReadOnly = True
            txtbxConditions.ReadOnly = True
            txtbxBenefits.ReadOnly = True
            txtbxReqSkill.ReadOnly = True
        End If
    End Sub

    Private Sub LoadJobDetails()
        Try
            Dim query As String = "
                SELECT JobTitle, JobDescription, SalaryRange, CountryOfEmployment, 
                       EmploymentContractDuration, JobType, VisaType, Conditions, Benefits, 
                       RequiredSkills, ApplicationDeadline 
                FROM jobplacement 
                WHERE JobPlacementID = " & jobID

            readQuery(query)

            If cmdRead.Read() Then
                txtbxJobTitle.Text = cmdRead("JobTitle").ToString()
                txtbxJobDescription.Text = cmdRead("JobDescription").ToString()
                txtbxSalaryRange.Text = cmdRead("SalaryRange").ToString()
                txtbxCountry.Text = cmdRead("CountryOfEmployment").ToString()
                txtbxContractDuration.Text = cmdRead("EmploymentContractDuration").ToString()
                txtbxJobType.Text = cmdRead("JobType").ToString()
                txtbxVisaType.Text = cmdRead("VisaType").ToString()
                txtbxConditions.Text = cmdRead("Conditions").ToString()
                txtbxBenefits.Text = cmdRead("Benefits").ToString()
                txtbxReqSkill.Text = cmdRead("RequiredSkills").ToString()

                If Not IsDBNull(cmdRead("ApplicationDeadline")) Then
                    TextBox5.Text = Convert.ToDateTime(cmdRead("ApplicationDeadline")).ToString("yyyy-MM-dd")
                Else
                    TextBox5.Text = "N/A"
                End If
            End If
            cmdRead.Close()
        Catch ex As Exception
            MsgBox("Error loading job details: " & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub LoadOfwProfile()
        Try
            Dim query As String = "
                SELECT o.OFWID,
                       CONCAT(o.FirstName, ' ', o.MiddleName, ' ', o.LastName) AS full_name,
                       o.DOB, o.CivilStatus, o.Sex, o.ContactNum,
                       o.EducationalLevel, o.PassportNum, o.VISANum, o.OECNum,
                       CONCAT(o.Street, ', ', o.City, ', ', o.Province, ' ', o.Zipcode) AS full_address,
                       o.PictureFace
                FROM ofw o
                WHERE o.OFWID = " & Session.CurrentReferenceID

            readQuery(query)

            If cmdRead.Read() Then
                Label4.Text = cmdRead("OFWID").ToString()
                lblFullName.Text = cmdRead("full_name").ToString()
                lblDOB.Text = Convert.ToDateTime(cmdRead("DOB")).ToString("yyyy-MM-dd")
                lblCivilStat.Text = cmdRead("CivilStatus").ToString()
                lblSex.Text = cmdRead("Sex").ToString()
                lblContactNum.Text = cmdRead("ContactNum").ToString()
                lblEducLevel.Text = cmdRead("EducationalLevel").ToString()
                lblPassportNum.Text = cmdRead("PassportNum").ToString()
                lblVisaNum.Text = cmdRead("VISANum").ToString()
                lblOecNum.Text = cmdRead("OECNum").ToString()
                lblFullAddress.Text = cmdRead("full_address").ToString()

                ' Load image
                If Not IsDBNull(cmdRead("PictureFace")) Then
                    Dim imgBytes As Byte() = CType(cmdRead("PictureFace"), Byte())
                    If imgBytes IsNot Nothing AndAlso imgBytes.Length > 0 Then
                        Using ms As New MemoryStream(imgBytes)
                            picProfile.Image = Image.FromStream(ms)
                        End Using
                    Else
                        picProfile.Image = Nothing
                    End If
                Else
                    picProfile.Image = Nothing
                End If
            End If
            cmdRead.Close()

            ' Load skills
            Dim skillQuery As String = "
                SELECT GROUP_CONCAT(s.SkillName SEPARATOR ', ') AS skills
                FROM ofwskill os
                JOIN skill s ON os.SkillID = s.SkillID
                WHERE os.OFWID = " & Session.CurrentReferenceID

            readQuery(skillQuery)
            If cmdRead.Read() Then
                lblSkills.Text = cmdRead("skills").ToString()
            Else
                lblSkills.Text = "No skills listed"
            End If
            cmdRead.Close()
        Catch ex As Exception
            MsgBox("Error loading OFW profile: " & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub btnApply_Click(sender As Object, e As EventArgs) Handles btnApply.Click
        Try
            Dim checkQuery As String = "
                SELECT 1 FROM application 
                WHERE OFWID = " & Session.CurrentReferenceID & " 
                  AND JobPlacementID = " & jobID

            readQuery(checkQuery)
            If cmdRead.HasRows Then
                cmdRead.Close()
                MsgBox("You have already applied to this job.", MsgBoxStyle.Information)
                Exit Sub
            End If
            cmdRead.Close()

            Dim getAgencyQuery As String = "SELECT AgencyID FROM jobplacement WHERE JobPlacementID = " & jobID
            readQuery(getAgencyQuery)

            Dim agencyID As Integer = -1
            If cmdRead.Read() Then
                agencyID = Convert.ToInt32(cmdRead("AgencyID"))
            Else
                cmdRead.Close()
                MsgBox("Agency not found for this job.", MsgBoxStyle.Critical)
                Exit Sub
            End If
            cmdRead.Close()

            Dim insertQuery As String = "
                INSERT INTO application (OFWID, AgencyID, JobPlacementID, ApplicationStatus)
                VALUES (" & Session.CurrentReferenceID & ", " & agencyID & ", " & jobID & ", 'Pending')"

            readQuery(insertQuery)
            MsgBox("Application submitted successfully!", MsgBoxStyle.Information)
            Me.Close()
        Catch ex As Exception
            MsgBox("Error submitting application: " & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    ' Reusable function to get the latest application ID for this job
    Private Function GetLatestApplicationId() As Integer
        Try
            Dim query As String = "
                SELECT ApplicationID 
                FROM application 
                WHERE OFWID = " & Session.CurrentReferenceID & " 
                  AND JobPlacementID = " & jobID & " 
                ORDER BY ApplicationDate DESC 
                LIMIT 1"
            readQuery(query)
            If cmdRead.Read() Then
                Return Convert.ToInt32(cmdRead("ApplicationID"))
            End If
            cmdRead?.Close()
        Catch ex As Exception
            MsgBox("Warning: Could not check existing applications. " & ex.Message, MsgBoxStyle.Exclamation)
        End Try
        Return -1
    End Function

    ' View Application Letter
    Private Sub ApplicationLetterViewBTN_Click(sender As Object, e As EventArgs) Handles ApplicationLetterViewBTN.Click
        Dim applicationId As Integer = GetLatestApplicationId()

        Dim viewForm As ApplicationLetterView
        If applicationId = -1 Then
            ' Preview mode – letter not yet submitted
            viewForm = New ApplicationLetterView() ' Calls constructor with default -1
        Else
            ' Load submitted letter
            viewForm = New ApplicationLetterView(applicationId)
        End If

        viewForm.ShowDialog()
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub
End Class
