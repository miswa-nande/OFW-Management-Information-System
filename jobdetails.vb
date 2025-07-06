Public Class jobdetails
    Private jobID As Integer

    ' Constructor to accept selected JobPlacementID
    Public Sub New(selectedJobId As Integer)
        InitializeComponent()
        Me.jobID = selectedJobId
    End Sub

    Private Sub jobdetails_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Allow everyone to view, but only OFW can apply
        If Session.CurrentLoggedUser.userType <> "OFW" Then
            btnApply.Visible = False
            btnClose.Text = "Close"
            SetReadOnlyMode()
        End If

        LoadJobDetails()
    End Sub

    Private Sub LoadJobDetails()
        Try
            Dim query As String = "SELECT JobTitle, JobDescription, SalaryRange, CountryOfEmployment, " &
                                  "EmploymentContractDuration, RequiredSkills, JobType, VisaType, " &
                                  "Conditions, Benefits, ApplicationDeadline " &
                                  "FROM jobplacement WHERE JobPlacementID = " & jobID

            readQuery(query)

            If cmdRead.Read() Then
                txtbxJobTitle.Text = cmdRead("JobTitle").ToString()
                txtbxJobDescription.Text = cmdRead("JobDescription").ToString()
                txtbxSalaryRange.Text = cmdRead("SalaryRange").ToString()
                txtbxCountry.Text = cmdRead("CountryOfEmployment").ToString()
                txtbxContractDuration.Text = cmdRead("EmploymentContractDuration").ToString()
                txtbxReqSkill.Text = cmdRead("RequiredSkills").ToString()
                txtbxJobType.Text = cmdRead("JobType").ToString()
                txtbxVisaType.Text = cmdRead("VisaType").ToString()
                txtbxConditions.Text = cmdRead("Conditions").ToString()
                txtbxBenefits.Text = cmdRead("Benefits").ToString()

                If Not IsDBNull(cmdRead("ApplicationDeadline")) Then
                    TextBox5.Text = Convert.ToDateTime(cmdRead("ApplicationDeadline")).ToString("yyyy-MM-dd")
                Else
                    TextBox5.Text = "N/A"
                End If
            Else
                MsgBox("Job not found.", MsgBoxStyle.Exclamation)
                Me.Close()
            End If

            cmdRead.Close()
        Catch ex As Exception
            MsgBox("Error loading job details: " & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub btnApply_Click(sender As Object, e As EventArgs) Handles btnApply.Click
        Try
            Dim checkQuery As String = "SELECT 1 FROM application " &
                                       "WHERE OFWId = " & Session.CurrentReferenceID & " " &
                                       "AND JobPlacementID = " & jobID
            readQuery(checkQuery)

            If cmdRead.HasRows Then
                cmdRead.Close()
                MsgBox("You have already applied to this job.", MsgBoxStyle.Information)
                Return
            End If
            cmdRead.Close()

            Dim dlg As New applicationForm(jobID)
            dlg.Text = "Apply to Job"
            dlg.ShowDialog()
            Me.Close()

            readQuery(checkQuery)
            If cmdRead.HasRows Then
                MsgBox("Application successfully submitted!", MsgBoxStyle.Information)
            End If
            cmdRead.Close()
        Catch ex As Exception
            MsgBox("Error preparing application: " & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub SetReadOnlyMode()
        txtbxJobTitle.ReadOnly = True
        txtbxJobDescription.ReadOnly = True
        txtbxSalaryRange.ReadOnly = True
        txtbxCountry.ReadOnly = True
        txtbxContractDuration.ReadOnly = True
        txtbxReqSkill.ReadOnly = True
        txtbxJobType.ReadOnly = True
        txtbxVisaType.ReadOnly = True
        txtbxConditions.ReadOnly = True
        txtbxBenefits.ReadOnly = True
        TextBox5.ReadOnly = True
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub
End Class
