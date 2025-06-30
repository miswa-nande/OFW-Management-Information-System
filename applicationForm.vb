Imports MySql.Data.MySqlClient

Public Class applicationForm
    Private jobID As Integer
    Private isViewMode As Boolean = False

    ' Updated constructor with view mode support
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

    ' ✅ Load job placement details
    Private Sub LoadJobDetails()
        Try
            Dim query As String = $"
                SELECT jp.job_title, jp.job_description, jp.salary, jp.location,
                       jp.contract_duration, jp.job_type, jp.visa_type,
                       jp.conditions, jp.benefits, s.skill_name
                FROM jobplacement jp
                LEFT JOIN skill s ON jp.skill_id = s.skill_id
                WHERE jp.job_id = {jobID}"

            readQuery(query)

            If cmdRead.Read() Then
                txtbxJobTitle.Text = cmdRead("job_title").ToString()
                txtbxJobDescription.Text = cmdRead("job_description").ToString()
                txtbxSalaryRange.Text = cmdRead("salary").ToString()
                txtbxCountry.Text = cmdRead("location").ToString()
                txtbxContractDuration.Text = cmdRead("contract_duration").ToString()
                txtbxJobType.Text = cmdRead("job_type").ToString()
                txtbxVisaType.Text = cmdRead("visa_type").ToString()
                txtbxConditions.Text = cmdRead("conditions").ToString()
                txtbxBenefits.Text = cmdRead("benefits").ToString()
                txtbxReqSkill.Text = cmdRead("skill_name").ToString()
            End If

            cmdRead.Close()
        Catch ex As Exception
            MsgBox("Error loading job details: " & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    ' ✅ Load OFW's own profile using Session
    Private Sub LoadOfwProfile()
        Try
            Dim query As String = $"
                SELECT CONCAT(FirstName, ' ', MiddleName, ' ', LastName) AS full_name, 
                       DOB, CivilStatus, Sex, ContactNum,
                       EducationalLevel, PassportNum, VISANum, OECNum
                FROM ofw
                WHERE ofw_id = {Session.CurrentReferenceID}"

            readQuery(query)

            If cmdRead.Read() Then
                lblFullName.Text = cmdRead("full_name").ToString()
                lblDOB.Text = CDate(cmdRead("DOB")).ToShortDateString()
                lblCivilStat.Text = cmdRead("CivilStatus").ToString()
                lblSex.Text = cmdRead("Sex").ToString()
                lblContactNum.Text = cmdRead("ContactNum").ToString()
                lblEducLevel.Text = cmdRead("EducationalLevel").ToString()
                lblPassportNum.Text = cmdRead("PassportNum").ToString()
                lblVisaNum.Text = cmdRead("VISANum").ToString()
                lblOecNum.Text = cmdRead("OECNum").ToString()
            End If

            cmdRead.Close()
        Catch ex As Exception
            MsgBox("Error loading OFW profile: " & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    ' ✅ Apply to job
    Private Sub btnApply_Click(sender As Object, e As EventArgs) Handles btnApply.Click
        Try
            ' Check if already applied
            Dim checkQuery As String = $"
                SELECT COUNT(*) FROM application 
                WHERE job_id = {jobID} AND ofw_id = {Session.CurrentReferenceID}"

            readQuery(checkQuery)

            If cmdRead.Read() AndAlso Convert.ToInt32(cmdRead(0)) > 0 Then
                cmdRead.Close()
                MsgBox("You have already applied to this job.", MsgBoxStyle.Exclamation)
                Exit Sub
            End If
            cmdRead.Close()

            ' Insert application
            Dim insertQuery As String = $"
                INSERT INTO application (job_id, ofw_id, status, date_applied)
                VALUES ({jobID}, {Session.CurrentReferenceID}, 'Pending', NOW())"

            readQuery(insertQuery)

            MsgBox("Application submitted successfully!", MsgBoxStyle.Information)
            Me.Close()
        Catch ex As Exception
            MsgBox("Error submitting application: " & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    ' Cancel
    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub
End Class
