Public Class jobdetails
    Private jobID As Integer

    ' Constructor to accept job_id
    Public Sub New(selectedJobId As Integer)
        InitializeComponent()
        Me.jobID = selectedJobId
    End Sub

    Private Sub jobdetails_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadJobDetails()
    End Sub

    Private Sub LoadJobDetails()
        Try
            Dim query As String = "
                SELECT jp.job_title, jp.job_description, jp.salary, jp.location,
                       jp.contract_duration, jp.skill_id, jp.job_type,
                       jp.visa_type, jp.conditions, jp.benefits,
                       s.skill_name
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
                txtbxReqSkill.Text = cmdRead("skill_name").ToString()
                txtbxJobType.Text = cmdRead("job_type").ToString()
                txtbxVisaType.Text = cmdRead("visa_type").ToString()
                txtbxConditions.Text = cmdRead("conditions").ToString()
                txtbxBenefits.Text = cmdRead("benefits").ToString()
            Else
                MsgBox("Job not found.", MsgBoxStyle.Exclamation)
                Me.Close()
            End If
        Catch ex As Exception
            MsgBox("Error loading job details: " & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub btnApply_Click(sender As Object, e As EventArgs) Handles btnApply.Click
        ' Pass the job ID to the application form
        Dim dlg As New applicationForm(jobID)
        dlg.ShowDialog()
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    ' The other TextBox TextChanged events can remain empty unless you need live validation
End Class
