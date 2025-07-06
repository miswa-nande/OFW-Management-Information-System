Imports MySql.Data.MySqlClient

Public Class addJob
    Private Sub addJob_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        PopulateCountries()
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

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        ' Validate required fields
        If txtbxJobTitle.Text.Trim() = "" OrElse txtbxJobDescription.Text.Trim() = "" OrElse
            cbxCountryOfEmployment.SelectedIndex = -1 OrElse txtbxSalaryRange.Text.Trim() = "" OrElse
            txtbxJobType.Text.Trim() = "" OrElse cbxVisaType.SelectedIndex = -1 OrElse
            txtbxContractDuration.Text.Trim() = "" OrElse Not Integer.TryParse(txtbxContractDuration.Text.Trim(), Nothing) Then

            MessageBox.Show("Please fill in all required fields correctly.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Try
            Dim query As String = "INSERT INTO jobplacement (JobTitle, JobDescription, CountryOfEmployment, SalaryRange, EmploymentContractDuration, JobType, VisaType, ApplicationDeadline, EmployerID, AgencyID, NumOfVacancies, Conditions, Benefits, RequiredSkills) " &
                                  "VALUES (@title, @desc, @loc, @salary, @contract, @type, @visa, @deadline, @employer, @agency, @vacancies, @conditions, @benefits, @skills)"

            Using conn As New MySqlConnection(strConnection)
                conn.Open()

                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@title", txtbxJobTitle.Text.Trim())
                    cmd.Parameters.AddWithValue("@desc", txtbxJobDescription.Text.Trim())
                    cmd.Parameters.AddWithValue("@loc", cbxCountryOfEmployment.SelectedItem.ToString())
                    cmd.Parameters.AddWithValue("@salary", Convert.ToInt32(txtbxSalaryRange.Text.Trim()))
                    cmd.Parameters.AddWithValue("@contract", Convert.ToInt32(txtbxContractDuration.Text.Trim()))
                    cmd.Parameters.AddWithValue("@type", txtbxJobType.Text.Trim())
                    cmd.Parameters.AddWithValue("@visa", cbxVisaType.SelectedItem.ToString())
                    cmd.Parameters.AddWithValue("@deadline", dateApplicationDeadline.Value.ToString("yyyy-MM-dd"))
                    cmd.Parameters.AddWithValue("@employer", Convert.ToInt32(txtbxEmployerIdNum.Text.Trim()))
                    cmd.Parameters.AddWithValue("@agency", Session.CurrentReferenceID)
                    cmd.Parameters.AddWithValue("@vacancies", Convert.ToInt32(txtbxNumOfVacancies.Text.Trim()))
                    cmd.Parameters.AddWithValue("@conditions", txtbxConditions.Text.Trim())
                    cmd.Parameters.AddWithValue("@benefits", txtbxBenefits.Text.Trim())
                    cmd.Parameters.AddWithValue("@skills", txtbxReqSkill.Text.Trim())
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

    Private Sub cbxVisaType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxVisaType.SelectedIndexChanged
    End Sub
End Class
