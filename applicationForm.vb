Imports MySql.Data.MySqlClient

Public Class applicationForm
    Private jobID As Integer
    Private isViewMode As Boolean = False

    ' Constructor with optional View Mode
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

            ' Make fields read-only
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

    'Load job details from jobplacement table
    Private Sub LoadJobDetails()
        Try
            Dim query As String = "SELECT JobTitle, JobDescription, SalaryRange, CountryOfEmployment, " &
                                  "EmploymentContractDuration, JobType, VisaType, Conditions, Benefits, RequiredSkills, ApplicationDeadline " &
                                  "FROM jobplacement WHERE JobPlacementID = " & jobID

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
            ' Query to get profile and address
            Dim query As String = "
            SELECT 
                o.OFWID,
                CONCAT(o.FirstName, ' ', o.MiddleName, ' ', o.LastName) AS full_name,
                o.DOB, o.CivilStatus, o.Sex, o.ContactNum,
                o.EducationalLevel, o.PassportNum, o.VISANum, o.OECNum,
                CONCAT(o.Street, ', ', o.City, ', ', o.Province, ' ', o.Zipcode) AS full_address
            FROM ofw o
            WHERE o.OFWID = " & Session.CurrentReferenceID

            readQuery(query)

            If cmdRead.Read() Then
                Label4.Text = cmdRead("OFWId").ToString()
                lblFullName.Text = cmdRead("full_name").ToString()
                lblDOB.Text = CDate(cmdRead("DOB")).ToShortDateString()
                lblCivilStat.Text = cmdRead("CivilStatus").ToString()
                lblSex.Text = cmdRead("Sex").ToString()
                lblContactNum.Text = cmdRead("ContactNum").ToString()
                lblEducLevel.Text = cmdRead("EducationalLevel").ToString()
                lblPassportNum.Text = cmdRead("PassportNum").ToString()
                lblVisaNum.Text = cmdRead("VISANum").ToString()
                lblOecNum.Text = cmdRead("OECNum").ToString()
                lblFullAddress.Text = cmdRead("full_address").ToString()
            End If
            cmdRead.Close()

            ' Get skills (separate query)
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


    'Handle job application submission
    Private Sub btnApply_Click(sender As Object, e As EventArgs) Handles btnApply.Click
        Try
            ' Check if already applied to this specific job
            Dim checkQuery As String = "SELECT 1 FROM application " &
                                   "WHERE OFWId = " & Session.CurrentReferenceID & " " &
                                   "AND JobPlacementID = " & jobID

            readQuery(checkQuery)

            If cmdRead.HasRows Then
                cmdRead.Close()
                MsgBox("You have already applied to this job.", MsgBoxStyle.Information)
                Exit Sub
            End If
            cmdRead.Close()

            ' Get the agency ID from the jobplacement table
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

            ' Insert new application
            Dim insertQuery As String = "INSERT INTO application (OFWId, AgencyID, JobPlacementID, ApplicationStatus) " &
                                    "VALUES (" & Session.CurrentReferenceID & ", " & agencyID & ", " & jobID & ", 'Pending')"

            readQuery(insertQuery)
            MsgBox("Application submitted successfully!", MsgBoxStyle.Information)
            Me.Close()
        Catch ex As Exception
            MsgBox("Error submitting application: " & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub



    ' Cancel and close form
    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub ApplicationLetterViewBTN_Click(sender As Object, e As EventArgs) Handles ApplicationLetterViewBTN.Click
        ' Still try to load existing application if it exists, but don’t block the form
        Dim applicationId As Integer = -1

        Try
            Dim query As String = "SELECT ApplicationID FROM application " &
                                  "WHERE OFWID = " & Session.CurrentReferenceID & " " &
                                  "AND JobPlacementID = " & jobID & " " &
                                  "ORDER BY ApplicationDate DESC LIMIT 1"
            readQuery(query)

            If cmdRead IsNot Nothing AndAlso cmdRead.Read() Then
                applicationId = Convert.ToInt32(cmdRead("ApplicationID"))
            End If
            cmdRead?.Close()
        Catch ex As Exception
            MsgBox("Warning: Could not check existing applications. Proceeding anyway.", MsgBoxStyle.Exclamation)
        End Try

        ' Open the view form regardless
        Dim viewForm As New ApplicationLetterView(applicationId)
        viewForm.ShowDialog()
    End Sub

    Private Sub lblSkills_Click(sender As Object, e As EventArgs) Handles lblSkills.Click

    End Sub

    Private Sub lblFullAddress_Click(sender As Object, e As EventArgs) Handles lblFullAddress.Click

    End Sub
End Class
