Imports MySql.Data.MySqlClient

Public Class editJob
    Private jobId As Integer

    Private Sub editJob_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        jobId = Session.CurrentReferenceID
        LoadJobDetails()
    End Sub

    Private Sub LoadJobDetails()
        Try
            Dim query As String = "SELECT * FROM jobplacement WHERE job_id = @jobId"
            Using conn As New MySqlConnection(strConnection)
                conn.Open()
                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@jobId", jobId)
                    Using reader As MySqlDataReader = cmd.ExecuteReader()
                        If reader.Read() Then
                            txtbxJobTitle.Text = reader("job_title").ToString()
                            txtbxJobDescription.Text = reader("job_description").ToString()
                            cbxCountryOfEmployment.SelectedItem = reader("location").ToString()
                            txtbxSalaryRange.Text = reader("salary").ToString()
                            dateContractDuration.Value = If(IsDBNull(reader("contract_duration")), Date.Now, Convert.ToDateTime(reader("contract_duration")))
                            txtbxJobType.Text = reader("job_type").ToString()
                            cbxVisaType.SelectedItem = reader("visa_type").ToString()
                            dateApplicationDeadline.Value = If(IsDBNull(reader("application_deadline")), Date.Now, Convert.ToDateTime(reader("application_deadline")))
                            txtbxEmployerIdNum.Text = reader("employer_id").ToString()
                            txtbxNumOfVacancies.Text = reader("num_vacancies").ToString()
                            txtbxConditions.Text = reader("conditions").ToString()
                            txtbxBenefits.Text = reader("benefits").ToString()
                            ' Skill: look up skill name
                            If Not IsDBNull(reader("skill_id")) Then
                                Dim skillId = Convert.ToInt32(reader("skill_id"))
                                txtbxReqSkill.Text = GetSkillName(skillId)
                            End If
                        End If
                    End Using
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading job details: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Function GetSkillName(skillId As Integer) As String
        Try
            Using conn As New MySqlConnection(strConnection)
                conn.Open()
                Dim query As String = "SELECT skill_name FROM skill WHERE skill_id = @id"
                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@id", skillId)
                    Dim result = cmd.ExecuteScalar()
                    If result IsNot Nothing Then
                        Return result.ToString()
                    End If
                End Using
            End Using
        Catch
        End Try
        Return ""
    End Function

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        ' Validate required fields
        If txtbxJobTitle.Text.Trim() = "" OrElse txtbxJobDescription.Text.Trim() = "" OrElse cbxCountryOfEmployment.SelectedIndex = -1 OrElse txtbxSalaryRange.Text.Trim() = "" OrElse txtbxJobType.Text.Trim() = "" OrElse cbxVisaType.SelectedIndex = -1 OrElse dateApplicationDeadline.Value = Nothing Then
            MessageBox.Show("Please fill in all required fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        Try
            Dim query As String = "UPDATE jobplacement SET job_title=@title, job_description=@desc, location=@loc, salary=@salary, contract_duration=@contract, job_type=@type, visa_type=@visa, application_deadline=@deadline, employer_id=@employer, agency_id=@agency, num_vacancies=@vacancies, conditions=@conditions, benefits=@benefits, skill_id=@skill WHERE job_id=@jobId"
            Using conn As New MySqlConnection(strConnection)
                conn.Open()
                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@title", txtbxJobTitle.Text.Trim())
                    cmd.Parameters.AddWithValue("@desc", txtbxJobDescription.Text.Trim())
                    cmd.Parameters.AddWithValue("@loc", cbxCountryOfEmployment.SelectedItem.ToString())
                    cmd.Parameters.AddWithValue("@salary", txtbxSalaryRange.Text.Trim())
                    cmd.Parameters.AddWithValue("@contract", dateContractDuration.Value.ToString("yyyy-MM-dd"))
                    cmd.Parameters.AddWithValue("@type", txtbxJobType.Text.Trim())
                    cmd.Parameters.AddWithValue("@visa", cbxVisaType.SelectedItem.ToString())
                    cmd.Parameters.AddWithValue("@deadline", dateApplicationDeadline.Value.ToString("yyyy-MM-dd"))
                    cmd.Parameters.AddWithValue("@employer", txtbxEmployerIdNum.Text.Trim())
                    cmd.Parameters.AddWithValue("@agency", Session.CurrentReferenceID)
                    cmd.Parameters.AddWithValue("@vacancies", txtbxNumOfVacancies.Text.Trim())
                    cmd.Parameters.AddWithValue("@conditions", txtbxConditions.Text.Trim())
                    cmd.Parameters.AddWithValue("@benefits", txtbxBenefits.Text.Trim())
                    ' Skill: look up or create skill_id from skill name
                    Dim skillId As Object = DBNull.Value
                    If txtbxReqSkill.Text.Trim() <> "" Then
                        Dim skillQuery As String = "SELECT skill_id FROM skill WHERE skill_name = @skillName"
                        Using skillCmd As New MySqlCommand(skillQuery, conn)
                            skillCmd.Parameters.AddWithValue("@skillName", txtbxReqSkill.Text.Trim())
                            Dim result = skillCmd.ExecuteScalar()
                            If result IsNot Nothing Then
                                skillId = result
                            Else
                                ' Insert new skill if not found
                                Dim insertSkill As String = "INSERT INTO skill (skill_name) VALUES (@skillName)" 
                                Using insertCmd As New MySqlCommand(insertSkill, conn)
                                    insertCmd.Parameters.AddWithValue("@skillName", txtbxReqSkill.Text.Trim())
                                    insertCmd.ExecuteNonQuery()
                                End Using
                                skillId = skillCmd.LastInsertedId
                            End If
                        End Using
                    End If
                    cmd.Parameters.AddWithValue("@skill", skillId)
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