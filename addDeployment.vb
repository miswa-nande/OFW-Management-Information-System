Imports System.Globalization
Imports MySql.Data.MySqlClient

Public Class addDeployment
    Private Sub addDeployment_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Populate country ComboBox
        Dim countrySet As New HashSet(Of String)()
        For Each culture As CultureInfo In CultureInfo.GetCultures(CultureTypes.SpecificCultures)
            Dim region As New RegionInfo(culture.Name)
            countrySet.Add(region.EnglishName)
        Next
        Dim countryList = countrySet.ToList()
        countryList.Sort()
        cbxCountry.Items.AddRange(countryList.ToArray())

        ' Restrict access to Agency users only
        If Session.CurrentLoggedUser.userType = "Agency" Then
            txtbxAgencyId.Text = Session.CurrentReferenceID.ToString()
            txtbxAgencyId.ReadOnly = True
        Else
            MessageBox.Show("Access denied. Only agencies can add deployments.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Me.Close()
        End If
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        ' Validate required fields (example: OFW ID, Employer ID, Agency ID, Country, Salary, Contract Number, etc.)
        If txtbxOfwId.Text.Trim() = "" OrElse txtbxEmployerId.Text.Trim() = "" OrElse txtbxAgencyId.Text.Trim() = "" OrElse cbxCountry.SelectedIndex = -1 OrElse txtbxSalary.Text.Trim() = "" OrElse txtbxContractNum.Text.Trim() = "" Then
            MessageBox.Show("Please fill in all required fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Try
            Using conn As New MySqlConnection(strConnection)
                conn.Open()
                Dim query As String = "INSERT INTO deploymentrecord (ofw_id, employer_id, agency_id, country_of_deployment, salary, contract_number, contract_duration, deployment_status, contract_start, contract_end, repatriation_status, reason_for_return, remarks) VALUES (@ofw, @employer, @agency, @country, @salary, @contractNum, @contractDuration, @status, @start, @end, @repatriation, @reason, @remarks)"
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
End Class