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
           txtbxContractDuration.Text.Trim() = "" OrElse txtbxEmployerIdNum.Text.Trim() = "" OrElse
           txtbxNumOfVacancies.Text.Trim() = "" OrElse
           Not Integer.TryParse(txtbxSalaryRange.Text.Trim(), Nothing) OrElse
           Not Integer.TryParse(txtbxContractDuration.Text.Trim(), Nothing) OrElse
           Not Integer.TryParse(txtbxEmployerIdNum.Text.Trim(), Nothing) OrElse
           Not Integer.TryParse(txtbxNumOfVacancies.Text.Trim(), Nothing) Then

            MessageBox.Show("Please fill in all required fields correctly.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Prepare values
        Dim title = txtbxJobTitle.Text.Trim().Replace("'", "''")
        Dim desc = txtbxJobDescription.Text.Trim().Replace("'", "''")
        Dim country = cbxCountryOfEmployment.SelectedItem.ToString().Replace("'", "''")
        Dim salary = Convert.ToInt32(txtbxSalaryRange.Text.Trim())
        Dim contract = Convert.ToInt32(txtbxContractDuration.Text.Trim())
        Dim jobType = txtbxJobType.Text.Trim().Replace("'", "''")
        Dim visa = cbxVisaType.SelectedItem.ToString().Replace("'", "''")
        Dim deadline = dateApplicationDeadline.Value.ToString("yyyy-MM-dd")
        Dim employerId = Convert.ToInt32(txtbxEmployerIdNum.Text.Trim())
        Dim agencyId = Session.CurrentReferenceID
        Dim vacancies = Convert.ToInt32(txtbxNumOfVacancies.Text.Trim())
        Dim conditions = txtbxConditions.Text.Trim().Replace("'", "''")
        Dim benefits = txtbxBenefits.Text.Trim().Replace("'", "''")
        Dim skills = txtbxReqSkill.Text.Trim().Replace("'", "''")

        ' SQL Insert using readQuery
        Dim insertQuery As String = $"
            INSERT INTO jobplacement 
            (JobTitle, JobDescription, CountryOfEmployment, SalaryRange, EmploymentContractDuration, JobType, VisaType, 
             ApplicationDeadline, EmployerID, AgencyID, NumOfVacancies, Conditions, Benefits, RequiredSkills) 
            VALUES 
            ('{title}', '{desc}', '{country}', {salary}, {contract}, '{jobType}', '{visa}', 
             '{deadline}', {employerId}, {agencyId}, {vacancies}, '{conditions}', '{benefits}', '{skills}');
        "

        Try
            readQuery(insertQuery)
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
