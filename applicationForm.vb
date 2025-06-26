Imports MySql.Data.MySqlClient

Public Class applicationForm
    Private jobID As Integer

    ' Constructor accepting job ID from jobdetails form
    Public Sub New(jobId As Integer)
        InitializeComponent()
        Me.jobID = jobId
    End Sub

    Private Sub applicationForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadJobDetails()
        LoadOfwProfile()
    End Sub

    ' Load job placement details
    Private Sub LoadJobDetails()
        Try
            Dim query As String = "
                SELECT jp.job_title, jp.job_description, jp.salary, jp.location,
                       jp.contract_duration, jp.job_type, jp.visa_type,
                       jp.conditions, jp.benefits, s.skill_name
                FROM jobplacement jp
                LEFT JOIN skill s ON jp.skill_id = s.skill_id
                WHERE jp.job_id = " & jobID

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
            conn.Close()
        Catch ex As Exception
            MsgBox("Error loading job details: " & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    ' Load OFW personal profile details
    Private Sub LoadOfwProfile()
        Try
            Dim query As String = "
                SELECT full_name, dob, civil_status, sex, contact_number,
                       educ_level, passport_no, visa_no, oec_no
                FROM ofw
                WHERE ofw_id = " & LoggedInOfwID

            readQuery(query)

            If cmdRead.Read() Then
                lblFullName.Text = cmdRead("full_name").ToString()
                lblDOB.Text = cmdRead("dob").ToString()
                lblCivilStat.Text = cmdRead("civil_status").ToString()
                lblSex.Text = cmdRead("sex").ToString()
                lblContactNum.Text = cmdRead("contact_number").ToString()
                lblEducLevel.Text = cmdRead("educ_level").ToString()
                lblPassportNum.Text = cmdRead("passport_no").ToString()
                lblVisaNum.Text = cmdRead("visa_no").ToString()
                lblOecNum.Text = cmdRead("oec_no").ToString()
            End If
            conn.Close()
        Catch ex As Exception
            MsgBox("Error loading OFW profile: " & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    ' Apply button - inserts application if not already applied
    Private Sub btnApply_Click(sender As Object, e As EventArgs) Handles btnApply.Click
        Try
            ' Check if already applied
            Dim checkQuery As String = "
                SELECT COUNT(*) FROM application 
                WHERE job_id = " & jobID & " AND ofw_id = " & LoggedInOfwID
            readQuery(checkQuery)

            If cmdRead.Read() AndAlso Convert.ToInt32(cmdRead(0)) > 0 Then
                conn.Close()
                MsgBox("You have already applied to this job.", MsgBoxStyle.Exclamation)
                Exit Sub
            End If
            conn.Close()

            ' Insert new application
            Dim insertQuery As String = "
                INSERT INTO application (job_id, ofw_id, status, date_applied)
                VALUES (" & jobID & ", " & LoggedInOfwID & ", 'Pending', NOW())"
            readQuery(insertQuery)

            MsgBox("Application submitted successfully!", MsgBoxStyle.Information)
            Me.Close()
        Catch ex As Exception
            MsgBox("Error submitting application: " & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    ' Cancel button - closes the form
    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    ' (Optional empty handlers below – can be removed if not needed)

    Private Sub txtbxJobTitle_TextChanged(sender As Object, e As EventArgs) Handles txtbxJobTitle.TextChanged
    End Sub

    Private Sub txtbxJobDescription_TextChanged(sender As Object, e As EventArgs) Handles txtbxJobDescription.TextChanged
    End Sub

    Private Sub txtbxCountry_TextChanged(sender As Object, e As EventArgs) Handles txtbxCountry.TextChanged
    End Sub

    Private Sub txtbxSalaryRange_TextChanged(sender As Object, e As EventArgs) Handles txtbxSalaryRange.TextChanged
    End Sub

    Private Sub txtbxContractDuration_TextChanged(sender As Object, e As EventArgs) Handles txtbxContractDuration.TextChanged
    End Sub

    Private Sub txtbxReqSkill_TextChanged(sender As Object, e As EventArgs) Handles txtbxReqSkill.TextChanged
    End Sub

    Private Sub txtbxJobType_TextChanged(sender As Object, e As EventArgs) Handles txtbxJobType.TextChanged
    End Sub

    Private Sub txtbxVisaType_TextChanged(sender As Object, e As EventArgs) Handles txtbxVisaType.TextChanged
    End Sub

    Private Sub txtbxConditions_TextChanged(sender As Object, e As EventArgs) Handles txtbxConditions.TextChanged
    End Sub

    Private Sub txtbxBenefits_TextChanged(sender As Object, e As EventArgs) Handles txtbxBenefits.TextChanged
    End Sub

    Private Sub picProfile_Click(sender As Object, e As EventArgs) Handles picProfile.Click
    End Sub

    Private Sub Label4_Click(sender As Object, e As EventArgs) Handles Label4.Click
    End Sub

    Private Sub lblFullName_Click(sender As Object, e As EventArgs) Handles lblFullName.Click
    End Sub

    Private Sub lblDOB_Click(sender As Object, e As EventArgs) Handles lblDOB.Click
    End Sub

    Private Sub lblCivilStat_Click(sender As Object, e As EventArgs) Handles lblCivilStat.Click
    End Sub

    Private Sub lblSex_Click(sender As Object, e As EventArgs) Handles lblSex.Click
    End Sub

    Private Sub lblContactNum_Click(sender As Object, e As EventArgs) Handles lblContactNum.Click
    End Sub

    Private Sub lblEducLevel_Click(sender As Object, e As EventArgs) Handles lblEducLevel.Click
    End Sub

    Private Sub lblSkills_Click(sender As Object, e As EventArgs) Handles lblSkills.Click
    End Sub

    Private Sub lblPassportNum_Click(sender As Object, e As EventArgs) Handles lblPassportNum.Click
    End Sub

    Private Sub lblVisaNum_Click(sender As Object, e As EventArgs) Handles lblVisaNum.Click
    End Sub

    Private Sub lblOecNum_Click(sender As Object, e As EventArgs) Handles lblOecNum.Click
    End Sub
End Class
