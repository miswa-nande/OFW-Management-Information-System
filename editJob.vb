Imports MySql.Data.MySqlClient

Public Class editJob
    Private jobId As Integer

    Public Sub New(jobId As Integer)
        InitializeComponent()
        Me.jobId = jobId
    End Sub

    Private Sub editJob_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        PopulateCountries()
        LoadJobDetails()
    End Sub

    Private Sub PopulateCountries()
        cbxCountryOfEmployment.Items.Clear()

        Dim countries As String() = {
            "Saudi Arabia", "United Arab Emirates", "Qatar", "Kuwait", "Bahrain", "Oman",
            "Japan", "Taiwan", "South Korea", "Hong Kong", "Singapore", "Macau", "Malaysia",
            "Australia", "New Zealand", "Canada", "United Kingdom", "Germany", "Italy",
            "USA", "Cyprus", "Israel", "Brunei", "Malta", "Czech Republic", "Poland",
            "Romania", "Finland", "Norway", "Sweden", "Denmark"
        }

        cbxCountryOfEmployment.Items.AddRange(countries)
    End Sub

    Private Sub LoadJobDetails()
        Try
            Dim query As String = "SELECT * FROM jobplacement WHERE JobPlacementID = @jobId"
            Using conn As New MySqlConnection(strConnection)
                conn.Open()
                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@jobId", jobId)
                    Using reader As MySqlDataReader = cmd.ExecuteReader()
                        If reader.Read() Then
                            txtbxJobTitle.Text = reader("JobTitle").ToString()
                            txtbxJobDescription.Text = reader("JobDescription").ToString()
                            cbxCountryOfEmployment.Text = reader("CountryOfEmployment").ToString()
                            txtbxSalaryRange.Text = reader("SalaryRange").ToString()
                            txtbxContractDuration.Text = reader("EmploymentContractDuration").ToString()
                            txtbxReqSkill.Text = reader("RequiredSkills").ToString()
                            txtbxJobType.Text = reader("JobType").ToString()
                            cbxVisaType.Text = reader("VisaType").ToString()
                            txtbxNumOfVacancies.Text = reader("NumOfVacancies").ToString()
                            txtbxConditions.Text = reader("Conditions").ToString()
                            txtbxBenefits.Text = reader("Benefits").ToString()

                            ' ApplicationDeadline
                            If Not IsDBNull(reader("ApplicationDeadline")) Then
                                Dim deadline As Date
                                If Date.TryParse(reader("ApplicationDeadline").ToString(), deadline) Then
                                    dateApplicationDeadline.Value = deadline
                                Else
                                    dateApplicationDeadline.Value = Date.Today
                                End If
                            Else
                                dateApplicationDeadline.Value = Date.Today
                            End If

                            txtbxEmployerIdNum.Text = reader("EmployerID").ToString()
                        End If
                    End Using
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading job details: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        ' Validation
        If txtbxJobTitle.Text.Trim() = "" OrElse
           txtbxJobDescription.Text.Trim() = "" OrElse
           cbxCountryOfEmployment.Text.Trim() = "" OrElse
           txtbxSalaryRange.Text.Trim() = "" OrElse
           txtbxContractDuration.Text.Trim() = "" OrElse
           txtbxReqSkill.Text.Trim() = "" OrElse
           txtbxJobType.Text.Trim() = "" OrElse
           cbxVisaType.Text.Trim() = "" OrElse
           txtbxEmployerIdNum.Text.Trim() = "" Then

            MessageBox.Show("Please fill in all required fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Try
            Using conn As New MySqlConnection(strConnection)
                conn.Open()

                Dim updateQuery As String = "
                    UPDATE jobplacement SET 
                        JobTitle = @title,
                        JobDescription = @desc,
                        CountryOfEmployment = @country,
                        SalaryRange = @salary,
                        EmploymentContractDuration = @duration,
                        RequiredSkills = @skills,
                        JobType = @type,
                        VisaType = @visa,
                        NumOfVacancies = @vacancies,
                        Conditions = @conditions,
                        Benefits = @benefits,
                        ApplicationDeadline = @deadline,
                        EmployerID = @employer
                    WHERE JobPlacementID = @jobId
                "

                Using cmd As New MySqlCommand(updateQuery, conn)
                    cmd.Parameters.AddWithValue("@title", txtbxJobTitle.Text.Trim())
                    cmd.Parameters.AddWithValue("@desc", txtbxJobDescription.Text.Trim())
                    cmd.Parameters.AddWithValue("@country", cbxCountryOfEmployment.Text.Trim())
                    cmd.Parameters.AddWithValue("@salary", Convert.ToInt32(txtbxSalaryRange.Text.Trim()))
                    cmd.Parameters.AddWithValue("@duration", Convert.ToInt32(txtbxContractDuration.Text.Trim()))
                    cmd.Parameters.AddWithValue("@skills", txtbxReqSkill.Text.Trim())
                    cmd.Parameters.AddWithValue("@type", txtbxJobType.Text.Trim())
                    cmd.Parameters.AddWithValue("@visa", cbxVisaType.Text.Trim())
                    cmd.Parameters.AddWithValue("@vacancies", Convert.ToInt32(txtbxNumOfVacancies.Text.Trim()))
                    cmd.Parameters.AddWithValue("@conditions", txtbxConditions.Text.Trim())
                    cmd.Parameters.AddWithValue("@benefits", txtbxBenefits.Text.Trim())
                    cmd.Parameters.AddWithValue("@deadline", dateApplicationDeadline.Value.ToString("yyyy-MM-dd"))
                    cmd.Parameters.AddWithValue("@employer", Convert.ToInt32(txtbxEmployerIdNum.Text.Trim()))
                    cmd.Parameters.AddWithValue("@jobId", jobId)

                    cmd.ExecuteNonQuery()
                End Using
            End Using

            MessageBox.Show("Job updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.Close()

        Catch ex As Exception
            MessageBox.Show("Error updating job: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub
End Class
