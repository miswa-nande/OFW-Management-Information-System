Imports System.Globalization
Imports MySql.Data.MySqlClient

Public Class addDeployment
    Private Sub addDeployment_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Populate country ComboBox
        Dim countrySet As New HashSet(Of String)()
        For Each culture As CultureInfo In CultureInfo.GetCultures(CultureTypes.SpecificCultures)
            Try
                Dim region As New RegionInfo(culture.Name)
                countrySet.Add(region.EnglishName)
            Catch
            End Try
        Next
        Dim countryList = countrySet.ToList()
        countryList.Sort()
        cbxCountry.Items.AddRange(countryList.ToArray())

        ' Populate static ComboBoxes
        cbxDepStat.Items.AddRange({"Scheduled", "Deployed", "Completed", "Returned"})
        cbxRepatriationStat.Items.AddRange({"Yes", "No"})
        cbxReason.Items.AddRange({"Completed", "Terminated", "Emergency"})

        ' Restrict to Agency users
        If Session.CurrentLoggedUser.userType = "Agency" Then
            txtbxAgencyId.Text = Session.CurrentReferenceID.ToString()
            txtbxAgencyId.ReadOnly = True
        Else
            MessageBox.Show("Access denied. Only agencies can add deployments.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Me.Close()
        End If
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        ' Validate required fields
        If txtbxOfwId.Text.Trim() = "" OrElse
           txtbxEmployerId.Text.Trim() = "" OrElse
           txtbxAgencyId.Text.Trim() = "" OrElse
           cbxCountry.SelectedIndex = -1 OrElse
           txtbxSalary.Text.Trim() = "" OrElse
           txtbxContractNum.Text.Trim() = "" Then

            MessageBox.Show("Please fill in all required fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Build INSERT query manually
        Dim query As String = "
            INSERT INTO deploymentrecord (
                ApplicationID, EmployerID, AgencyID, CountryOfDeployment, Salary,
                ContractNumber, ContractDuration, DeploymentStatus, ContractStartDate,
                ContractEndDate, RepatriationStatus, ReasonForReturn, DeploymentRemarks,
                FlightNumber, Airport
            ) VALUES (
                {0}, {1}, {2}, '{3}', '{4}',
                '{5}', '{6}', {7}, {8},
                {9}, {10}, {11}, {12},
                {13}, {14}
            )"

        ' Sanitize and inject values
        Dim ofwId = txtbxOfwId.Text.Trim()
        Dim employerId = txtbxEmployerId.Text.Trim()
        Dim agencyId = txtbxAgencyId.Text.Trim()
        Dim country = cbxCountry.SelectedItem.ToString().Replace("'", "''")
        Dim salary = txtbxSalary.Text.Trim().Replace("'", "''")
        Dim contractNum = txtbxContractNum.Text.Trim().Replace("'", "''")
        Dim contractDuration = txtbxContractDuration.Text.Trim().Replace("'", "''")
        Dim depStatus = If(cbxDepStat.SelectedItem IsNot Nothing, $"'{cbxDepStat.SelectedItem.ToString().Replace("'", "''")}'", "NULL")
        Dim repatriation = If(cbxRepatriationStat.SelectedItem IsNot Nothing, $"'{cbxRepatriationStat.SelectedItem.ToString().Replace("'", "''")}'", "NULL")
        Dim reason = If(cbxReason.SelectedItem IsNot Nothing, $"'{cbxReason.SelectedItem.ToString().Replace("'", "''")}'", "NULL")
        Dim remarks = If(String.IsNullOrWhiteSpace(txtbxRemarks.Text), "NULL", $"'{txtbxRemarks.Text.Trim().Replace("'", "''")}'")
        Dim startDate = If(dateContractStart.Checked, $"'{dateContractStart.Value.ToString("yyyy-MM-dd")}'", "NULL")
        Dim endDate = If(dateContractEnd.Checked, $"'{dateContractEnd.Value.ToString("yyyy-MM-dd")}'", "NULL")

        ' Final formatted query
        Dim finalQuery = String.Format(query,
                                       ofwId, employerId, agencyId, country, salary,
                                       contractNum, contractDuration, depStatus, startDate,
                                       endDate, repatriation, reason, remarks)

        Try
            readQuery(finalQuery)
            MessageBox.Show("Deployment record added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.Close()
        Catch ex As Exception
            MessageBox.Show("Error adding deployment record: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub
End Class
