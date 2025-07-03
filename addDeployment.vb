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
        If txtbxOfwId.Text.Trim() = "" OrElse txtbxEmployerId.Text.Trim() = "" OrElse
           txtbxAgencyId.Text.Trim() = "" OrElse cbxCountry.SelectedIndex = -1 OrElse
           txtbxSalary.Text.Trim() = "" OrElse txtbxContractNum.Text.Trim() = "" Then
            MessageBox.Show("Please fill in all required fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Try
            Using conn As New MySqlConnection(strConnection)
                conn.Open()

                Dim query As String = "INSERT INTO deploymentrecord (ofw_id, employer_id, agency_id, CountryOfDeployment, Salary, ContractNumber, ContractDuration, DeploymentStatus, ContractStartDate, ContractEndDate, RepatriationStatus, ReasonForReturn, DeploymentRemarks, FlightNumber, Airport) " &
                                      "VALUES (@ofw, @employer, @agency, @country, @salary, @contractNum, @contractDuration, @depStatus, @start, @end, @repatriation, @reason, @remarks, @flight, @airport)"

                Using cmd As New MySqlCommand(query, conn)
                    ' Required fields
                    cmd.Parameters.AddWithValue("@ofw", txtbxOfwId.Text.Trim())
                    cmd.Parameters.AddWithValue("@employer", txtbxEmployerId.Text.Trim())
                    cmd.Parameters.AddWithValue("@agency", txtbxAgencyId.Text.Trim())
                    cmd.Parameters.AddWithValue("@country", cbxCountry.SelectedItem.ToString())
                    cmd.Parameters.AddWithValue("@salary", txtbxSalary.Text.Trim())
                    cmd.Parameters.AddWithValue("@contractNum", txtbxContractNum.Text.Trim())
                    cmd.Parameters.AddWithValue("@contractDuration", txtbxContractDuration.Text.Trim())

                    ' Optional ComboBoxes
                    cmd.Parameters.AddWithValue("@depStatus", If(cbxDepStat.SelectedItem IsNot Nothing, cbxDepStat.SelectedItem.ToString(), DBNull.Value))
                    cmd.Parameters.AddWithValue("@repatriation", If(cbxRepatriationStat.SelectedItem IsNot Nothing, cbxRepatriationStat.SelectedItem.ToString(), DBNull.Value))
                    cmd.Parameters.AddWithValue("@reason", If(cbxReason.SelectedItem IsNot Nothing, cbxReason.SelectedItem.ToString(), DBNull.Value))

                    ' Optional Dates
                    If dateContractStart.Checked Then
                        cmd.Parameters.AddWithValue("@start", dateContractStart.Value.ToString("yyyy-MM-dd"))
                    Else
                        cmd.Parameters.AddWithValue("@start", DBNull.Value)
                    End If
                    If dateContractEnd.Checked Then
                        cmd.Parameters.AddWithValue("@end", dateContractEnd.Value.ToString("yyyy-MM-dd"))
                    Else
                        cmd.Parameters.AddWithValue("@end", DBNull.Value)
                    End If

                    ' Optional Remarks, Flight Number, Airport
                    cmd.Parameters.AddWithValue("@remarks", If(String.IsNullOrWhiteSpace(txtbxRemarks.Text), DBNull.Value, txtbxRemarks.Text.Trim()))

                    cmd.ExecuteNonQuery()
                End Using
            End Using

            MessageBox.Show("Deployment record added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.Close()

        Catch ex As Exception
            MessageBox.Show("Error adding deployment record: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub dateContractEnd_ValueChanged(sender As Object, e As EventArgs) Handles dateContractEnd.ValueChanged

    End Sub
End Class
