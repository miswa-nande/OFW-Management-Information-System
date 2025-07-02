Imports System.Globalization
Imports MySql.Data.MySqlClient

Public Class editDeployment
    Private deploymentId As Integer

    Private Sub editDeployment_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Restrict access to Agency users only
        If Session.CurrentLoggedUser.userType <> "Agency" Then
            MessageBox.Show("Access denied. Only agencies can edit deployments.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Me.Close()
            Return
        End If
        deploymentId = Session.CurrentReferenceID
        ' Populate country ComboBox
        Dim countrySet As New HashSet(Of String)()
        For Each culture As CultureInfo In CultureInfo.GetCultures(CultureTypes.SpecificCultures)
            Dim region As New RegionInfo(culture.Name)
            countrySet.Add(region.EnglishName)
        Next
        Dim countryList = countrySet.ToList()
        countryList.Sort()
        cbxCountry.Items.AddRange(countryList.ToArray())
        LoadDeploymentDetails()
    End Sub

    Private Sub LoadDeploymentDetails()
        Try
            Using conn As New MySqlConnection(strConnection)
                conn.Open()
                Dim query As String = "SELECT * FROM deploymentrecord WHERE deployment_id = @id"
                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@id", deploymentId)
                    Using reader As MySqlDataReader = cmd.ExecuteReader()
                        If reader.Read() Then
                            txtbxOfwId.Text = reader("ofw_id").ToString()
                            txtbxEmployerId.Text = reader("employer_id").ToString()
                            txtbxAgencyId.Text = reader("agency_id").ToString()
                            cbxCountry.SelectedItem = reader("country_of_deployment").ToString()
                            txtbxSalary.Text = reader("salary").ToString()
                            txtbxContractNum.Text = reader("contract_number").ToString()
                            txtbxContractDuration.Text = reader("contract_duration").ToString()
                            cbxDepStat.SelectedItem = reader("deployment_status").ToString()
                            dateContractStart.Value = DateTime.Parse(reader("contract_start").ToString())
                            cbxRepatriationStat.SelectedItem = reader("repatriation_status").ToString()
                            cbxReason.SelectedItem = reader("reason_for_return").ToString()
                            txtbxRemarks.Text = reader("remarks").ToString()
                        End If
                    End Using
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading deployment details: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        ' Validate required fields
        If txtbxOfwId.Text.Trim() = "" OrElse txtbxEmployerId.Text.Trim() = "" OrElse txtbxAgencyId.Text.Trim() = "" OrElse cbxCountry.SelectedIndex = -1 OrElse txtbxSalary.Text.Trim() = "" OrElse txtbxContractNum.Text.Trim() = "" Then
            MessageBox.Show("Please fill in all required fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        Try
            Using conn As New MySqlConnection(strConnection)
                conn.Open()
                Dim query As String = "UPDATE deploymentrecord SET ofw_id=@ofw, employer_id=@employer, agency_id=@agency, country_of_deployment=@country, salary=@salary, contract_number=@contractNum, contract_duration=@contractDuration, deployment_status=@status, contract_start=@start, contract_end=@end, repatriation_status=@repatriation, reason_for_return=@reason, remarks=@remarks WHERE deployment_id=@id"
                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@ofw", txtbxOfwId.Text.Trim())
                    cmd.Parameters.AddWithValue("@employer", txtbxEmployerId.Text.Trim())
                    cmd.Parameters.AddWithValue("@agency", txtbxAgencyId.Text.Trim())
                    cmd.Parameters.AddWithValue("@country", cbxCountry.SelectedItem.ToString())
                    cmd.Parameters.AddWithValue("@salary", txtbxSalary.Text.Trim())
                    cmd.Parameters.AddWithValue("@contractNum", txtbxContractNum.Text.Trim())
                    cmd.Parameters.AddWithValue("@contractDuration", txtbxContractDuration.Text.Trim())
                    ' Handle ComboBox nulls
                    If cbxDepStat.SelectedItem IsNot Nothing Then
                        cmd.Parameters.AddWithValue("@status", cbxDepStat.SelectedItem.ToString())
                    Else
                        cmd.Parameters.AddWithValue("@status", "")
                    End If
                    ' Handle date values
                    If dateContractStart.Checked Then
                        cmd.Parameters.AddWithValue("@start", dateContractStart.Value.ToString("yyyy-MM-dd"))
                    Else
                        cmd.Parameters.AddWithValue("@start", DBNull.Value)
                    End If
                    If cbxRepatriationStat.SelectedItem IsNot Nothing Then
                        cmd.Parameters.AddWithValue("@repatriation", cbxRepatriationStat.SelectedItem.ToString())
                    Else
                        cmd.Parameters.AddWithValue("@repatriation", "")
                    End If
                    If cbxReason.SelectedItem IsNot Nothing Then
                        cmd.Parameters.AddWithValue("@reason", cbxReason.SelectedItem.ToString())
                    Else
                        cmd.Parameters.AddWithValue("@reason", "")
                    End If
                    cmd.Parameters.AddWithValue("@remarks", txtbxRemarks.Text.Trim())
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