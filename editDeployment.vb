Imports System.Globalization
Imports MySql.Data.MySqlClient

Public Class editDeployment
    Private deploymentId As Integer

    Private Sub editDeployment_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Only agencies can access this form
        If Session.CurrentLoggedUser.userType <> "Agency" Then
            MessageBox.Show("Access denied. Only agencies can edit deployments.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Me.Close()
            Return
        End If

        ' Use the existing session reference ID as DeploymentID
        deploymentId = Session.CurrentReferenceID

        ' Populate dropdowns
        PopulateComboBoxes()

        ' Load data for editing
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

        ' Static lists
        cbxDepStat.Items.AddRange({"Scheduled", "Deployed", "Completed", "Returned"})
        cbxRepatriationStat.Items.AddRange({"Yes", "No"})
        cbxReason.Items.AddRange({"Completed", "Terminated", "Emergency"})
    End Sub

    Private Sub LoadDeploymentDetails()
        Try
            Using conn As New MySqlConnection(strConnection)
                conn.Open()
                Dim query As String = "SELECT * FROM deploymentrecord WHERE DeploymentID = @id"
                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@id", deploymentId)
                    Using reader As MySqlDataReader = cmd.ExecuteReader()
                        If reader.Read() Then
                            txtbxOfwId.Text = reader("ofw_id").ToString()
                            txtbxEmployerId.Text = reader("employer_id").ToString()
                            txtbxAgencyId.Text = reader("agency_id").ToString()
                            cbxCountry.SelectedItem = reader("CountryOfDeployment").ToString()
                            txtbxSalary.Text = reader("Salary").ToString()
                            txtbxContractNum.Text = reader("ContractNumber").ToString()
                            txtbxContractDuration.Text = reader("ContractDuration").ToString()
                            cbxDepStat.SelectedItem = reader("DeploymentStatus").ToString()

                            If Not IsDBNull(reader("ContractStartDate")) Then
                                dateContractStart.Value = Convert.ToDateTime(reader("ContractStartDate"))
                            End If
                            If Not IsDBNull(reader("ContractEndDate")) Then
                                dateContractEnd.Value = Convert.ToDateTime(reader("ContractEndDate"))
                            End If

                            cbxRepatriationStat.SelectedItem = reader("RepatriationStatus").ToString()
                            cbxReason.SelectedItem = reader("ReasonForReturn").ToString()
                            txtbxRemarks.Text = reader("DeploymentRemarks").ToString()
                        End If
                    End Using
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading deployment details: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        ' Validation
        If txtbxOfwId.Text.Trim() = "" OrElse txtbxEmployerId.Text.Trim() = "" OrElse
           txtbxAgencyId.Text.Trim() = "" OrElse cbxCountry.SelectedIndex = -1 OrElse
           txtbxSalary.Text.Trim() = "" OrElse txtbxContractNum.Text.Trim() = "" Then
            MessageBox.Show("Please fill in all required fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Try
            Using conn As New MySqlConnection(strConnection)
                conn.Open()
                Dim query As String = "UPDATE deploymentrecord SET " &
                    "ofw_id=@ofw, employer_id=@employer, agency_id=@agency, CountryOfDeployment=@country, " &
                    "Salary=@salary, ContractNumber=@contractNum, ContractDuration=@contractDuration, " &
                    "DeploymentStatus=@status, ContractStartDate=@start, ContractEndDate=@end, " &
                    "RepatriationStatus=@repatriation, ReasonForReturn=@reason, DeploymentRemarks=@remarks " &
                    "WHERE DeploymentID=@id"

                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@ofw", txtbxOfwId.Text.Trim())
                    cmd.Parameters.AddWithValue("@employer", txtbxEmployerId.Text.Trim())
                    cmd.Parameters.AddWithValue("@agency", txtbxAgencyId.Text.Trim())
                    cmd.Parameters.AddWithValue("@country", cbxCountry.SelectedItem.ToString())
                    cmd.Parameters.AddWithValue("@salary", txtbxSalary.Text.Trim())
                    cmd.Parameters.AddWithValue("@contractNum", txtbxContractNum.Text.Trim())
                    cmd.Parameters.AddWithValue("@contractDuration", txtbxContractDuration.Text.Trim())
                    cmd.Parameters.AddWithValue("@status", If(cbxDepStat.SelectedItem IsNot Nothing, cbxDepStat.SelectedItem.ToString(), DBNull.Value))
                    cmd.Parameters.AddWithValue("@start", If(dateContractStart.Checked, dateContractStart.Value.ToString("yyyy-MM-dd"), DBNull.Value))
                    cmd.Parameters.AddWithValue("@end", If(dateContractEnd.Checked, dateContractEnd.Value.ToString("yyyy-MM-dd"), DBNull.Value))
                    cmd.Parameters.AddWithValue("@repatriation", If(cbxRepatriationStat.SelectedItem IsNot Nothing, cbxRepatriationStat.SelectedItem.ToString(), DBNull.Value))
                    cmd.Parameters.AddWithValue("@reason", If(cbxReason.SelectedItem IsNot Nothing, cbxReason.SelectedItem.ToString(), DBNull.Value))
                    cmd.Parameters.AddWithValue("@remarks", If(String.IsNullOrWhiteSpace(txtbxRemarks.Text), DBNull.Value, txtbxRemarks.Text.Trim()))
                    cmd.Parameters.AddWithValue("@id", deploymentId)

                    cmd.ExecuteNonQuery()
                End Using
            End Using

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
