Imports System.IO

Public Class loginPage

    Private Sub btnAdmin_Click(sender As Object, e As EventArgs) Handles btnAdmin.Click
        OpenLogin("Admin")
    End Sub

    Private Sub btnAgency_Click(sender As Object, e As EventArgs) Handles btnAgency.Click
        OpenLogin("Agency")
    End Sub

    Private Sub btnOfw_Click(sender As Object, e As EventArgs) Handles btnOfw.Click
        OpenLogin("OFW")
    End Sub

    Private Sub btnEmployer_Click(sender As Object, e As EventArgs) Handles btnEmployer.Click
        OpenLogin("Employer")
    End Sub

    Private Sub OpenLogin(userType As String)
        ' Set the current user type for session tracking
        Session.CurrentLoggedUser.userType = userType

        ' Pass the userType to the login form
        Dim login As New loginFields(userType)
        login.Show()
        Me.Hide()
    End Sub

    Private Sub CheckSQLConnection()
        Try
            ' Load SQL config from the txt file
            Dim configPath As String = Path.Combine(Application.StartupPath, "config", "sql_config.txt")

            If Not File.Exists(configPath) Then
                MsgBox("SQL configuration file not found.", MsgBoxStyle.Critical)
                Exit Sub
            End If

            Dim host As String = "", user As String = "", pass As String = "", dbname As String = ""
            Dim lines = File.ReadAllLines(configPath)

            For Each line In lines
                If line.StartsWith("Localhost:") Then host = line.Substring("Localhost:".Length).Trim()
                If line.StartsWith("Root:") Then user = line.Substring("Root:".Length).Trim()
                If line.StartsWith("Password:") Then pass = line.Substring("Password:".Length).Trim()
                If line.StartsWith("DB_Name:") Then dbname = line.Substring("DB_Name:".Length).Trim()
            Next

            ' Create a connection string from config
            Dim connectionString As String = $"server={host};user id={user};password={pass};database={dbname};"

            ' Try to open connection
            Using testConn As New MySql.Data.MySqlClient.MySqlConnection(connectionString)
                testConn.Open()
                MsgBox("Connected to the database. Welcome!", MsgBoxStyle.Information)
            End Using

        Catch ex As Exception
            MsgBox("Unable to connect to the database. Please check the configuration." & vbCrLf & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub loginPage_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CheckSQLConnection()
    End Sub


End Class
