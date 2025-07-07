Imports System.IO
Imports MySql.Data.MySqlClient

Public Class loginPage
    Private configInitialized As Boolean = False

    Private Sub loginPage_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Automatically create config file if missing
        If Not configInitialized Then
            EnsureSQLConfigExists()
            configInitialized = True
        End If
    End Sub


    Private Sub btnAdmin_Click(sender As Object, e As EventArgs) Handles btnAdmin.Click
        Session.CurrentLoggedUser.userType = "Admin"
        CheckConnectionAndOpenLogin()
    End Sub

    Private Sub btnAgency_Click(sender As Object, e As EventArgs) Handles btnAgency.Click
        Session.CurrentLoggedUser.userType = "Agency"
        CheckConnectionAndOpenLogin()
    End Sub

    Private Sub btnOfw_Click(sender As Object, e As EventArgs) Handles btnOfw.Click
        Session.CurrentLoggedUser.userType = "OFW"
        CheckConnectionAndOpenLogin()
    End Sub

    Private Sub btnEmployer_Click(sender As Object, e As EventArgs) Handles btnEmployer.Click
        Session.CurrentLoggedUser.userType = "Employer"
        CheckConnectionAndOpenLogin()
    End Sub

    Private Sub CheckConnectionAndOpenLogin()
        Try
            EnsureSQLConfigExists()
            UpdateConnectionString()
            TestDatabaseConnection()

            MsgBox("Connected to the database. Welcome!", MsgBoxStyle.Information)

            ' If successful, open login form
            Dim login As New loginFields(Session.CurrentLoggedUser.userType)
            login.Show()
            Me.Hide()

        Catch ex As Exception
            MsgBox("Cannot connect to the database. Please contact the administrator." & vbCrLf & ex.Message,
                   MsgBoxStyle.Critical)
            Application.Exit()
        End Try
    End Sub

    Private Sub EnsureSQLConfigExists()
        Dim configDir As String = Path.Combine(Application.StartupPath, "config")
        Dim configPath As String = Path.Combine(configDir, "sql_config.txt")

        If Not Directory.Exists(configDir) Then
            Directory.CreateDirectory(configDir)
        End If

        If Not File.Exists(configPath) Then
            Dim defaultConfig As String() = {
                "Localhost=localhost",
                "Root=root",
                "Password=",
                "DB_Name=ofw_mis"
            }
            File.WriteAllLines(configPath, defaultConfig)
            MsgBox("SQL configuration file created with default values." & vbCrLf &
                   "Please edit the file before restarting." & vbCrLf & configPath,
                   MsgBoxStyle.Information)
            Application.Exit()
        End If
    End Sub

    Private Sub TestDatabaseConnection()
        Using testConn As New MySqlConnection(strConnection)
            testConn.Open()
        End Using
    End Sub

    ' === SHIFT+C to Open Config File ===
    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean
        If keyData = (Keys.Shift Or Keys.C) Then
            Dim configPath As String = Path.Combine(Application.StartupPath, "config", "sql_config.txt")
            Try
                If File.Exists(configPath) Then
                    Process.Start(New ProcessStartInfo("notepad.exe", $"""{configPath}""") With {.UseShellExecute = True})
                Else
                    MsgBox("Config file not found: " & configPath, MsgBoxStyle.Exclamation)
                End If
            Catch ex As Exception
                MsgBox("Failed to open config file: " & ex.Message, MsgBoxStyle.Critical)
            End Try
            Return True
        End If
        Return MyBase.ProcessCmdKey(msg, keyData)
    End Function

End Class
