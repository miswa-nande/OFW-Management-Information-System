Imports MySql.Data.MySqlClient

Public Class registeredAgency
    Private currentAgencyID As Integer = -1
    Private currentAgencyName As String = ""

    Private Sub registeredAgency_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadCurrentAgency()
        LoadAvailableAgencies()
    End Sub

    ' 1. Load the current agency name using AgencyID foreign key
    Private Sub LoadCurrentAgency()
        Try
            Dim query As String = "
                SELECT o.AgencyID, a.AgencyName
                FROM ofw o
                LEFT JOIN agency a ON o.AgencyID = a.AgencyID
                WHERE o.OFWID = " & Session.CurrentReferenceID

            readQuery(query)

            If cmdRead.Read() Then
                If Not IsDBNull(cmdRead("AgencyID")) Then
                    currentAgencyID = Convert.ToInt32(cmdRead("AgencyID"))
                End If

                currentAgencyName = If(IsDBNull(cmdRead("AgencyName")), "None", cmdRead("AgencyName").ToString())
                lblAgencyName.Text = currentAgencyName
            Else
                lblAgencyName.Text = "None"
            End If

            cmdRead.Close()
        Catch ex As Exception
            MsgBox("Error loading current agency: " & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    ' 2. Load agencies not already assigned (excluding current agency)
    Private Sub LoadAvailableAgencies()
        Try
            Dim query As String = "SELECT AgencyID, AgencyName FROM agency"

            If currentAgencyID <> -1 Then
                query &= $" WHERE AgencyID <> {currentAgencyID}"
            End If

            readQuery(query)

            Dim dt As New DataTable()
            dt.Load(cmdRead)
            cmdRead.Close()

            dgvAvailableAgency.DataSource = dt

            ' Column settings
            dgvAvailableAgency.Columns("AgencyID").Visible = False
            dgvAvailableAgency.Columns("AgencyName").HeaderText = "Available Agencies"

            ' Appearance settings
            With dgvAvailableAgency
                .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
                .SelectionMode = DataGridViewSelectionMode.FullRowSelect
                .ReadOnly = True
                .AllowUserToAddRows = False
                .AllowUserToDeleteRows = False
                .AllowUserToResizeRows = False
                .RowHeadersVisible = False
                .EnableHeadersVisualStyles = False

                ' Header styling
                .ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 66, 155)
                .ColumnHeadersDefaultCellStyle.ForeColor = Color.White
                .ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 10, FontStyle.Bold)
                .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

                ' Cell styling
                .DefaultCellStyle.Font = New Font("Segoe UI", 10)
                .DefaultCellStyle.BackColor = Color.White
                .DefaultCellStyle.ForeColor = Color.Black
                .DefaultCellStyle.SelectionBackColor = Color.FromArgb(173, 216, 230)
                .DefaultCellStyle.SelectionForeColor = Color.Black
                .RowTemplate.Height = 30
            End With

        Catch ex As Exception
            MsgBox("Error loading available agencies: " & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub


    ' 3. Update OFW's AgencyID when selecting new agency
    Private Sub btnChange_Click(sender As Object, e As EventArgs) Handles btnChange.Click
        If dgvAvailableAgency.CurrentRow IsNot Nothing Then
            Dim selectedAgencyID As Integer = Convert.ToInt32(dgvAvailableAgency.CurrentRow.Cells("AgencyID").Value)
            Dim selectedAgencyName As String = dgvAvailableAgency.CurrentRow.Cells("AgencyName").Value.ToString()

            Dim updateQuery As String = $"UPDATE ofw SET AgencyID = {selectedAgencyID} WHERE OFWID = {Session.CurrentReferenceID}"
            readQuery(updateQuery)
            cmdRead?.Close()

            MessageBox.Show("Agency updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

            ' Refresh OFW profile if open
            If ofwProfile.Instance IsNot Nothing Then
                ofwProfile.Instance.LoadOFWProfile()
            End If

            Me.Close()
        Else
            MessageBox.Show("Please select an agency first.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub
End Class
