Imports System.Globalization
Imports MySql.Data.MySqlClient

Public Class editDeployment
    Private deploymentId As Integer
    Private jobPlacementId As Integer

    Public Sub New(depId As Integer)
        InitializeComponent()
        deploymentId = depId
    End Sub

    Private Sub editDeployment_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Session.CurrentLoggedUser.userType <> "Agency" Then
            MessageBox.Show("Access denied. Only agencies can edit deployments.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Me.Close()
            Return
        End If

        PopulateComboBoxes()
        LoadDeploymentDetails()
    End Sub

    Private Sub PopulateComboBoxes()
        ' Country list
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

        ' Static values
        cbxDepStat.Items.AddRange({"Scheduled", "Deployed", "Completed", "Returned"})
        cbxRepatriationStat.Items.AddRange({"Yes", "No"})
        cbxReason.Items.AddRange({"Completed", "Terminated", "Emergency"})
    End Sub

    Private Sub LoadDeploymentDetails()
        Dim query As String = "
            SELECT dr.*, jp.EmployerID 
            FROM deploymentrecord dr
            LEFT JOIN jobplacement jp ON dr.JobPlacementID = jp.JobPlacementID
            WHERE dr.DeploymentID = " & deploymentId

        Try
            readQuery(query)

            If cmdRead.Read() Then
                txtbxOfwId.Text = cmdRead("ApplicationID").ToString()
                txtbxAgencyId.Text = cmdRead("AgencyID").ToString()
                txtbxEmployerId.Text = cmdRead("EmployerID").ToString() ' From join
                jobPlacementId = Convert.ToInt32(cmdRead("JobPlacementID"))

                cbxCountry.SelectedItem = cmdRead("CountryOfDeployment").ToString()
                txtbxSalary.Text = cmdRead("Salary").ToString()
                txtbxContractNum.Text = cmdRead("ContractNumber").ToString()
                txtbxContractDuration.Text = cmdRead("ContractDuration").ToString()
                cbxDepStat.SelectedItem = cmdRead("DeploymentStatus").ToString()

                If Not IsDBNull(cmdRead("ContractStartDate")) Then
                    dateContractStart.Value = Convert.ToDateTime(cmdRead("ContractStartDate"))
                    dateContractStart.Checked = True
                Else
                    dateContractStart.Checked = False
                End If

                If Not IsDBNull(cmdRead("ContractEndDate")) Then
                    dateContractEnd.Value = Convert.ToDateTime(cmdRead("ContractEndDate"))
                    dateContractEnd.Checked = True
                Else
                    dateContractEnd.Checked = False
                End If

                cbxRepatriationStat.SelectedItem = cmdRead("RepatriationStatus").ToString()
                cbxReason.SelectedItem = cmdRead("ReasonForReturn").ToString()
                txtbxRemarks.Text = cmdRead("DeploymentRemarks").ToString()
            End If

            cmdRead.Close()
        Catch ex As Exception
            MessageBox.Show("Error loading deployment details: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        ' Validation
        If txtbxOfwId.Text.Trim() = "" OrElse txtbxAgencyId.Text.Trim() = "" OrElse
           txtbxEmployerId.Text.Trim() = "" OrElse cbxCountry.SelectedIndex = -1 OrElse
           txtbxSalary.Text.Trim() = "" OrElse txtbxContractNum.Text.Trim() = "" Then
            MessageBox.Show("Please fill in all required fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Safe values
        Dim startDate As String = If(dateContractStart.Checked, $"'{dateContractStart.Value:yyyy-MM-dd}'", "NULL")
        Dim endDate As String = If(dateContractEnd.Checked, $"'{dateContractEnd.Value:yyyy-MM-dd}'", "NULL")
        Dim depStat As String = If(cbxDepStat.SelectedItem IsNot Nothing, $"'{cbxDepStat.SelectedItem}'", "NULL")
        Dim repatriation As String = If(cbxRepatriationStat.SelectedItem IsNot Nothing, $"'{cbxRepatriationStat.SelectedItem}'", "NULL")
        Dim reason As String = If(cbxReason.SelectedItem IsNot Nothing, $"'{cbxReason.SelectedItem}'", "NULL")
        Dim remarks As String = If(String.IsNullOrWhiteSpace(txtbxRemarks.Text), "NULL", $"'{txtbxRemarks.Text.Replace("'", "''")}'")

        ' Update query
        Dim updateQuery As String = $"
            UPDATE deploymentrecord SET
                ApplicationID = {txtbxOfwId.Text.Trim()},
                JobPlacementID = {jobPlacementId},
                AgencyID = {txtbxAgencyId.Text.Trim()},
                CountryOfDeployment = '{cbxCountry.SelectedItem.ToString().Replace("'", "''")}',
                Salary = '{txtbxSalary.Text.Trim().Replace("'", "''")}',
                ContractNumber = '{txtbxContractNum.Text.Trim().Replace("'", "''")}',
                ContractDuration = '{txtbxContractDuration.Text.Trim().Replace("'", "''")}',
                DeploymentStatus = {depStat},
                ContractStartDate = {startDate},
                ContractEndDate = {endDate},
                RepatriationStatus = {repatriation},
                ReasonForReturn = {reason},
                DeploymentRemarks = {remarks}
            WHERE DeploymentID = {deploymentId}
        "

        Try
            readQuery(updateQuery)
            MessageBox.Show("Deployment record updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.Close()
        Catch ex As Exception
            MessageBox.Show("Error updating deployment record: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub
End Class
