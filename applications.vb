Imports MySql.Data.MySqlClient

Public Class applications

    Private Sub applications_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Session.CurrentLoggedUser.userType <> "OFW" Then
            MsgBox("Access denied. This section is for OFWs only.", MsgBoxStyle.Critical)
            Me.Close()
            Exit Sub
        End If

        ' === Populate application status filter ===
        cbxContractStat.Items.Clear()
        cbxContractStat.Items.AddRange(New String() {"All", "Pending", "Accepted", "Rejected"})
        cbxContractStat.SelectedIndex = 0 ' Default to "All"

        ' === Clear all filters ===
        txtbxJobTitle.Clear()
        txtbxIdNum.Clear()
        txtbxContractNum.Clear()
        dateContractStart.Value = Date.Today
        dateContractStart.Checked = False

        ' === Load applications ===
        LoadApplications()
        FormatDGV()
    End Sub



    ' Load all applications for logged-in OFW with filters
    Private Sub LoadApplications()
        Dim query As String = $"
        SELECT a.ApplicationID, a.JobPlacementID, jp.JobTitle, jp.CountryOfEmployment, jp.SalaryRange,
               a.ApplicationStatus, a.ApplicationDate
        FROM application a
        JOIN jobplacement jp ON a.JobPlacementID = jp.JobPlacementID
        WHERE a.OFWId = {Session.CurrentReferenceID}"

        If Not String.IsNullOrWhiteSpace(txtbxJobTitle.Text) Then
            query &= $" AND jp.JobTitle LIKE '%{txtbxJobTitle.Text.Replace("'", "''")}%'"
        End If

        If Not String.IsNullOrWhiteSpace(cbxContractStat.Text) AndAlso cbxContractStat.Text <> "All" Then
            query &= $" AND a.ApplicationStatus = '{cbxContractStat.Text.Replace("'", "''")}'"
        End If

        If Not String.IsNullOrWhiteSpace(txtbxIdNum.Text) Then
            query &= $" AND a.ApplicationID LIKE '%{txtbxIdNum.Text.Trim()}%'"
        End If

        If Not String.IsNullOrWhiteSpace(txtbxContractNum.Text) Then
            query &= $" AND a.JobPlacementID LIKE '%{txtbxContractNum.Text.Trim()}%'"
        End If

        If dateContractStart.Checked Then
            query &= $" AND DATE(a.ApplicationDate) = '{dateContractStart.Value.ToString("yyyy-MM-dd")}'"
        End If

        query &= " ORDER BY a.ApplicationDate DESC"

        Try
            LoadToDGV(query, DataGridView1)
            FormatDGV()
        Catch ex As Exception
            MsgBox("Failed to load applications: " & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub


    ' Format DataGridView headers
    Private Sub FormatDGV()
        With DataGridView1
            ' Layout and behavior
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .MultiSelect = False
            .ReadOnly = True
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
            .AllowUserToResizeRows = False
            .RowHeadersVisible = False
            .BorderStyle = BorderStyle.None
            .EnableHeadersVisualStyles = False
            .BackgroundColor = Color.White

            ' Header style
            .ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 66, 155)
            .ColumnHeadersDefaultCellStyle.ForeColor = Color.White
            .ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 11, FontStyle.Bold)
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            ' Row style
            .DefaultCellStyle.Font = New Font("Segoe UI", 10)
            .DefaultCellStyle.SelectionBackColor = Color.FromArgb(100, 150, 200)
            .DefaultCellStyle.SelectionForeColor = Color.Black

            ' Header text renaming
            If .Columns.Contains("ApplicationID") Then .Columns("ApplicationID").HeaderText = "App ID"
            If .Columns.Contains("JobPlacementID") Then .Columns("JobPlacementID").HeaderText = "Job ID"
            If .Columns.Contains("JobTitle") Then .Columns("JobTitle").HeaderText = "Job Title"
            If .Columns.Contains("CountryOfEmployment") Then .Columns("CountryOfEmployment").HeaderText = "Country"
            If .Columns.Contains("SalaryRange") Then .Columns("SalaryRange").HeaderText = "Salary"
            If .Columns.Contains("ApplicationStatus") Then .Columns("ApplicationStatus").HeaderText = "Status"
            If .Columns.Contains("ApplicationDate") Then .Columns("ApplicationDate").HeaderText = "Date Applied"

            ' Status column color
            For Each row As DataGridViewRow In .Rows
                If Not row.IsNewRow AndAlso Not IsDBNull(row.Cells("ApplicationStatus").Value) Then
                    Dim status = row.Cells("ApplicationStatus").Value.ToString()
                    Select Case status
                        Case "Pending"
                            row.Cells("ApplicationStatus").Style.ForeColor = Color.DarkOrange
                        Case "Accepted"
                            row.Cells("ApplicationStatus").Style.ForeColor = Color.Green
                        Case "Rejected"
                            row.Cells("ApplicationStatus").Style.ForeColor = Color.Red
                    End Select
                End If
            Next
        End With
    End Sub



    ' Conditional formatting for status
    Private Sub HighlightStatusColumn()
        For Each row As DataGridViewRow In DataGridView1.Rows
            If row.Cells("ApplicationStatus").Value IsNot Nothing Then
                Dim status As String = row.Cells("ApplicationStatus").Value.ToString()

                Select Case status
                    Case "Accepted"
                        row.Cells("ApplicationStatus").Style.BackColor = Color.LightGreen
                        row.Cells("ApplicationStatus").Style.ForeColor = Color.Black

                    Case "Rejected"
                        row.Cells("ApplicationStatus").Style.BackColor = Color.IndianRed
                        row.Cells("ApplicationStatus").Style.ForeColor = Color.White

                    Case "Pending"
                        row.Cells("ApplicationStatus").Style.BackColor = Color.Gold
                        row.Cells("ApplicationStatus").Style.ForeColor = Color.Black
                End Select
            End If
        Next
    End Sub

    ' Live filter handlers
    Private Sub txtbxJobTitle_TextChanged(sender As Object, e As EventArgs) Handles txtbxJobTitle.TextChanged
        LoadApplications()
    End Sub

    Private Sub cbxContractStat_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxContractStat.SelectedIndexChanged
        LoadApplications()
    End Sub

    Private Sub txtbxIdNum_TextChanged(sender As Object, e As EventArgs) Handles txtbxIdNum.TextChanged
        LoadApplications()
    End Sub

    Private Sub txtbxContractNum_TextChanged(sender As Object, e As EventArgs) Handles txtbxContractNum.TextChanged
        LoadApplications()
    End Sub

    Private Sub dateContractStart_ValueChanged(sender As Object, e As EventArgs) Handles dateContractStart.ValueChanged
        LoadApplications()
    End Sub

    ' 🧼 Clear all filters
    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        txtbxJobTitle.Clear()
        cbxContractStat.SelectedIndex = -1
        txtbxIdNum.Clear()
        txtbxContractNum.Clear()
        dateContractStart.Value = Date.Today
        dateContractStart.Checked = False
        LoadApplications()
    End Sub

    ' View selected application
    Private Sub BtnViewApplication_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If DataGridView1.SelectedRows.Count = 0 Then
            MsgBox("Please select an application to view.", MsgBoxStyle.Information)
            Return
        End If

        Dim jobId As Integer = Convert.ToInt32(DataGridView1.SelectedRows(0).Cells("JobPlacementID").Value)
        Dim form As New applicationForm(jobId, True)
        form.Text = "View Application"
        form.ShowDialog()
    End Sub

    '  Navigation buttons
    Private Sub btnProfile_Click(sender As Object, e As EventArgs) Handles btnProfile.Click
        Dim newForm As New ofwProfile()
        newForm.Show()
        Me.Hide()
    End Sub

    Private Sub btnJobOffers_Click(sender As Object, e As EventArgs) Handles btnJobOffers.Click
        Dim newForm As New joboffers()
        newForm.Show()
        Me.Hide()
    End Sub

    Private Sub btnDeployment_Click(sender As Object, e As EventArgs) Handles btnDeployment.Click
        Dim newForm As New deploymentrecords()
        newForm.Show()
        Me.Hide()
    End Sub

End Class
