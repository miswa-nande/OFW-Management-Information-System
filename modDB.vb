Imports System.IO
Imports System.Security.Cryptography
Imports System.Text
Imports MySql.Data.MySqlClient


Module modDB
    Public myadocon, conn As New MySqlConnection
    Public cmd As New MySqlCommand
    Public cmdRead As MySqlDataReader
    Public db_server As String = ""
    Public db_uid As String = ""
    Public db_pwd As String = ""
    Public db_name As String = ""
    Public strConnection As String = "server=" & db_server & ";uid=" & db_uid & ";password=" & db_pwd & ";database=" & db_name & ";" & "allowuservariables='True';"

    Public Structure LoggedUser
        Dim id As Integer
        Dim name As String
        Dim position As String
        Dim username As String
        Dim password As String
        Dim type As Integer
    End Structure

    Public Sub UpdateConnectionString()
        Try
            Dim configPath As String = Path.Combine(Application.StartupPath, "config", "sql_config.txt")

            If Not File.Exists(configPath) Then
                Throw New Exception("SQL config file not found at: " & configPath)
            End If

            Dim lines() As String = File.ReadAllLines(configPath)
            Dim configDict As New Dictionary(Of String, String)(StringComparer.OrdinalIgnoreCase)

            For Each line As String In lines
                If line.Contains("=") Then
                    Dim parts() As String = line.Split(New String() {"="}, 2, StringSplitOptions.None)
                    Dim key As String = parts(0).Trim()
                    Dim value As String = parts(1).Trim()
                    configDict(key) = value
                End If
            Next

            ' Validate required keys (Password can be empty but must exist)
            Dim requiredKeys() As String = {"Localhost", "Root", "Password", "DB_Name"}
            For Each key As String In requiredKeys
                If Not configDict.ContainsKey(key) Then
                    Throw New Exception($"Missing config key: {key}")
                End If

                If key <> "Password" AndAlso String.IsNullOrWhiteSpace(configDict(key)) Then
                    Throw New Exception($"Config key '{key}' cannot be empty.")
                End If
            Next

            ' Update global DB connection variables
            db_server = configDict("Localhost")
            db_uid = configDict("Root")
            db_pwd = configDict("Password")
            db_name = configDict("DB_Name")

            ' Build the connection string using the updated variables
            strConnection = $"server={db_server};uid={db_uid};password={db_pwd};database={db_name};allowuservariables=True;"

        Catch ex As Exception
            MsgBox("Error reading or parsing SQL config file:" & vbCrLf & ex.Message, MsgBoxStyle.Critical)
            Application.Exit()
        End Try
    End Sub

    Public CurrentLoggedUser As LoggedUser = Nothing
    Public Sub openConn(ByVal db_name As String)
        Try
            With conn
                If .State = ConnectionState.Open Then .Close()
                .ConnectionString = strConnection
                .Open()
            End With
        Catch EX As Exception
            MsgBox(EX.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Public Sub readQuery(ByVal sql As String)
        Try
            openConn(db_name)
            With cmd
                .Connection = conn
                .CommandText = sql
                cmdRead = .ExecuteReader
            End With
        Catch EX As Exception
            MsgBox(EX.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Public Function isConnectedToLocalServer() As Boolean
        Dim result As Boolean = False
        Try
            myadocon = New MySqlConnection
            myadocon.ConnectionString = strConnection
            Try
                myadocon.Open()
                If myadocon.State = ConnectionState.Open Then
                    result = True
                Else
                    result = False
                End If
            Catch ex As Exception
                Return False
            End Try
            If myadocon.State = ConnectionState.Open Then
                myadocon.Close()
            End If
        Catch
            Return False
        End Try
        Return result
    End Function

    Function LoadToDGV(ByVal query As String, ByVal dgv As DataGridView) As Integer
        Try
            readQuery(query)
            Dim dt As DataTable = New DataTable
            dt.Load(cmdRead)
            dgv.DataSource = dt
            dgv.Refresh()
            Return dgv.Rows.Count
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
        Return 0
    End Function

    Function LoadToDGVForDisplay(ByVal query As String, ByVal dgv As DataGridView) As Integer
        Try
            readQuery(query)
            Dim dt As DataTable = New DataTable
            dt.Load(cmdRead)
            dgv.DataSource = dt
            dgv.Refresh()
            If dgv.ColumnCount > 1 Then
                dgv.Columns(0).Visible = False
            End If
            Return dgv.Rows.Count
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
        Return 0
    End Function

    Public Function Encrypt(ByVal clearText As String) As String

        Dim EncryptionKey As String = "MAKV2SPBNI99212"
        Dim clearBytes As Byte() = Encoding.Unicode.GetBytes(clearText)
        Using encryptor As Aes = Aes.Create()
            Dim pdb As New Rfc2898DeriveBytes(EncryptionKey, New Byte() {&H49, &H76, &H61, &H6E, &H20, &H4D,
             &H65, &H64, &H76, &H65, &H64, &H65,
             &H76})
            encryptor.Key = pdb.GetBytes(32)
            encryptor.IV = pdb.GetBytes(16)
            Using ms As New MemoryStream()
                Using cs As New CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write)
                    cs.Write(clearBytes, 0, clearBytes.Length)
                    cs.Close()
                End Using
                clearText = Convert.ToBase64String(ms.ToArray())
            End Using
        End Using
        Return clearText
    End Function
    Public Function Decrypt(ByVal cipherText As String) As String
        Dim EncryptionKey As String = "MAKV2SPBNI99212"
        Dim cipherBytes As Byte() = Convert.FromBase64String(cipherText)
        Using encryptor As Aes = Aes.Create()
            Dim pdb As New Rfc2898DeriveBytes(EncryptionKey, New Byte() {&H49, &H76, &H61, &H6E, &H20, &H4D,
             &H65, &H64, &H76, &H65, &H64, &H65,
             &H76})
            encryptor.Key = pdb.GetBytes(32)
            encryptor.IV = pdb.GetBytes(16)
            Using ms As New MemoryStream()
                Using cs As New CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write)
                    cs.Write(cipherBytes, 0, cipherBytes.Length)
                    cs.Close()
                End Using
                cipherText = Encoding.Unicode.GetString(ms.ToArray())
            End Using
        End Using
        Return cipherText
    End Function
    Sub Logs(ByVal transaction As String, Optional ByVal events As String = "*_Click")
        Try
            readQuery(String.Format("INSERT INTO logs`(dt`, user_accounts_id, event, transactions) VALUES ({0},{1},'{2}','{3}')", "now()",
                                    CurrentLoggedUser.id,
                                    events,
                                    transaction))
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    ' Loads all OFWs into the provided DataGridView
    Public Sub LoadOFWsToDGV(dgv As DataGridView)
        Dim query As String = "SELECT OFWId, FirstName, MiddleName, LastName, DOB, Sex, CivilStatus, Street, Barangay, City, Province, Zipcode, EducationalLevel, Skills, ContactNum, EmergencyContactNum, PassportNum, VISANum, OECNum, EmploymentStatus, DateAdded, AgencyID FROM ofw"
        LoadToDGV(query, dgv)
    End Sub

    ' Loads all Employers into the provided DataGridView
    Public Sub LoadEmployersToDGV(dgv As DataGridView)
        Dim query As String = "SELECT EmployerID, EmployerFirstName, EmployerMiddleName, EmployerLastName, CompanyName, CompanyStreet, CompanyCity, CompanyState, CompanyCountry, CompanyZipcode, EmployerContactNum, EmployerEmail, Industry, NumOfOFWHired, ActiveJobPlacement, DateAdded FROM employer"
        LoadToDGV(query, dgv)
    End Sub

    ' Loads all Deployments into the provided DataGridView
    Public Sub LoadDeploymentsToDGV(dgv As DataGridView)
        Dim query As String = "SELECT DeploymentID, DeploymentDate, CountryOfDeployment, Salary, MedicalCleared, VisaIssued, POEACleared, PDOSCompleted, FlightNumber, Airport, DeploymentRemarks, ContractDuration, ContractNumber, DeploymentStatus, ContractStartDate, ContractEndDate, RepatriationStatus, ReasonForReturn, ReturnDate, Remarks, ApplicationID, JobPlacementID, AgencyID FROM DeploymentRecord"
        LoadToDGV(query, dgv)
    End Sub

    ' Loads all Agencies into the provided DataGridView
    Public Sub LoadAgenciesToDGV(dgv As DataGridView)
        Dim query As String = "SELECT AgencyID, AgencyName, AgencyLicenseNumber, Street, City, State, Zipcode, ContactNum, Email, WebsiteUrl, Specialization, YearsOfOperation, NumOfDeployedWorkers, GovAccreditationStat, NumActiveJobOrders, LicenseExpDate, Notes, DateAdded FROM agency"
        LoadToDGV(query, dgv)
    End Sub

    ' Loads all Job Placements into the provided DataGridView
    Public Sub LoadJobPlacementsToDGV(dgv As DataGridView)
        Dim query As String = "SELECT JobPlacementID, JobTitle, JobDescription, CountryOfEmployment, SalaryRange, EmploymentContractDuration, RequiredSkills, JobType, VisaType, NumOfVacancies, Conditions, PostingDate, Benefits, ApplicationDeadline, JobStatus, EmployerID, AgencyID FROM jobplacement"
        LoadToDGV(query, dgv)
    End Sub

    ' Formats a DataGridView for uniform appearance and text visibility
    Public Sub FormatDGVUniformly(dgv As DataGridView)
        With dgv
            ' Display Settings
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells
            .AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None
            .DefaultCellStyle.WrapMode = DataGridViewTriState.False
            .ScrollBars = ScrollBars.Both
            .RowHeadersVisible = False

            ' Read-only and layout behavior
            .ReadOnly = True
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .MultiSelect = False
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
            .AllowUserToResizeRows = False
            .AllowUserToResizeColumns = True
            .BorderStyle = BorderStyle.FixedSingle
            .BackgroundColor = Color.White
            .EnableHeadersVisualStyles = False

            ' Header styling
            .ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 66, 155)
            .ColumnHeadersDefaultCellStyle.ForeColor = Color.White
            .ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 11, FontStyle.Bold)
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            ' Cell styling
            .DefaultCellStyle.Font = New Font("Segoe UI", 10)
            .DefaultCellStyle.BackColor = Color.White
            .DefaultCellStyle.ForeColor = Color.Black
            .DefaultCellStyle.SelectionBackColor = Color.FromArgb(100, 150, 200)
            .DefaultCellStyle.SelectionForeColor = Color.Black
            .RowTemplate.Height = 30

            ' Minimum column width (optional)
            For Each col As DataGridViewColumn In .Columns
                col.MinimumWidth = 100
            Next
        End With
    End Sub




    Public Sub DeleteRecord(tableName As String, primaryKeyColumn As String, id As Integer)
        Try
            openConn(db_name)
            Dim query As String = $"DELETE FROM {tableName} WHERE {primaryKeyColumn} = @id"
            Using cmd As New MySqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@id", id)
                Dim result = cmd.ExecuteNonQuery()
                If result > 0 Then
                    MsgBox("Record deleted successfully.", MsgBoxStyle.Information)
                Else
                    MsgBox("Record not found or could not be deleted.", MsgBoxStyle.Exclamation)
                End If
            End Using
        Catch ex As Exception
            MsgBox("An error occurred: " & ex.Message, MsgBoxStyle.Critical)
        Finally
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try
    End Sub
End Module