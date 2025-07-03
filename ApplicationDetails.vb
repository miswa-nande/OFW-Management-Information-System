Imports MySql.Data.MySqlClient

Public Class ApplicationDetails
    Private applicationId As Integer

    Private Sub ApplicationDetails_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        applicationId = Session.CurrentReferenceID
        LoadApplicationDetails()
    End Sub

    Private Sub LoadApplicationDetails()
        Dim query As String = "
            SELECT a.status, a.date_applied,
                   jp.job_title, jp.job_description, jp.location, jp.salary, jp.contract_duration,
                   jp.job_type, jp.visa_type, jp.conditions, jp.benefits,
                   o.firstname, o.lastname, o.dob, o.sex, o.civil_status, o.contact_number, o.address,
                   o.educ_level, o.passport_no, o.visa_no, o.oec_no,
                   GROUP_CONCAT(s.skill_name SEPARATOR ', ') AS skills,
                   o.profile_image
            FROM application a
            JOIN jobplacement jp ON a.job_id = jp.job_id
            JOIN ofw o ON a.ofw_id = o.ofw_id
            LEFT JOIN ofwskills os ON o.ofw_id = os.ofw_id
            LEFT JOIN skill s ON os.skill_id = s.skill_id
            WHERE a.application_id = @appId
            GROUP BY a.application_id
        "

        Using conn As New MySqlConnection(strConnection)
            conn.Open()
            Using cmd As New MySqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@appId", applicationId)
                Using reader As MySqlDataReader = cmd.ExecuteReader()
                    If reader.Read() Then
                        ' JOB DETAILS
                        txtbxJobTitle.Text = reader("job_title").ToString()
                        txtbxJobDescription.Text = reader("job_description").ToString()
                        txtbxCountry.Text = reader("location").ToString()
                        txtbxSalaryRange.Text = reader("salary").ToString()
                        txtbxContractDuration.Text = reader("contract_duration").ToString()
                        txtbxJobType.Text = reader("job_type").ToString()
                        txtbxVisaType.Text = reader("visa_type").ToString()
                        txtbxConditions.Text = reader("conditions").ToString()
                        txtbxBenefits.Text = reader("benefits").ToString()

                        ' OFW DETAILS
                        lblFullName.Text = reader("firstname") & " " & reader("lastname")
                        lblDOB.Text = Convert.ToDateTime(reader("dob")).ToString("yyyy-MM-dd")
                        lblSex.Text = reader("sex").ToString()
                        lblCivilStat.Text = reader("civil_status").ToString()
                        lblContactNum.Text = reader("contact_number").ToString()
                        lblFullAddress.Text = reader("address").ToString()
                        lblEducLevel.Text = reader("educ_level").ToString()
                        lblPassportNum.Text = reader("passport_no").ToString()
                        lblVisaNum.Text = reader("visa_no").ToString()
                        lblOecNum.Text = reader("oec_no").ToString()
                        lblSkills.Text = reader("skills").ToString()

                        ' Image (if stored as Base64 in DB)
                        If Not IsDBNull(reader("profile_image")) Then
                            Dim base64Str As String = reader("profile_image").ToString()
                            Dim bytes As Byte() = Convert.FromBase64String(base64Str)
                            Using ms As New IO.MemoryStream(bytes)
                                picProfile.Image = Image.FromStream(ms)
                            End Using
                        End If
                    Else
                        MessageBox.Show("No application details found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Me.Close()
                    End If
                End Using
            End Using
        End Using
    End Sub

    Private Sub btnApprove_Click(sender As Object, e As EventArgs) Handles btnApprove.Click
        UpdateApplicationStatus("Accepted")
    End Sub

    Private Sub btnDeny_Click(sender As Object, e As EventArgs) Handles btnDeny.Click
        UpdateApplicationStatus("Rejected")
    End Sub

    Private Sub UpdateApplicationStatus(status As String)
        Dim result As DialogResult = MessageBox.Show($"Are you sure you want to mark this application as {status}?",
                                                     "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If result = DialogResult.Yes Then
            Try
                Using conn As New MySqlConnection(strConnection)
                    conn.Open()
                    Dim query As String = "UPDATE application SET status = @status WHERE application_id = @id"
                    Using cmd As New MySqlCommand(query, conn)
                        cmd.Parameters.AddWithValue("@status", status)
                        cmd.Parameters.AddWithValue("@id", applicationId)
                        cmd.ExecuteNonQuery()
                    End Using
                End Using
                MessageBox.Show("Application status updated.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Me.Close()
            Catch ex As Exception
                MessageBox.Show("Failed to update status: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub
End Class
