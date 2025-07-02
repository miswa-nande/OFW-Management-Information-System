Imports MySql.Data.MySqlClient

Public Class addJob
    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxVisaType.SelectedIndexChanged

    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        ' Validate required fields
        If txtbxJobTitle.Text.Trim() = "" OrElse txtbxJobDescription.Text.Trim() = "" OrElse cbxCountryOfEmployment.SelectedIndex = -1 OrElse txtbxSalaryRange.Text.Trim() = "" OrElse txtbxJobType.Text.Trim() = "" OrElse cbxVisaType.SelectedIndex = -1 OrElse dateApplicationDeadline.Value = Nothing Then
            MessageBox.Show("Please fill in all required fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Try
            Dim query As String = "INSERT INTO jobplacement (job_title, job_description, location, salary, contract_duration, job_type, visa_type, application_deadline, employer_id, agency_id, num_vacancies, conditions, benefits, skill_id) VALUES (@title, @desc, @loc, @salary, @contract, @type, @visa, @deadline, @employer, @agency, @vacancies, @conditions, @benefits, @skill)"
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
                    cmd.ExecuteNonQuery()
                End Using
            End Using
            MessageBox.Show("Job added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.Close()
        Catch ex As Exception
            MessageBox.Show("Error adding job: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub
End Class