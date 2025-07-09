Imports MySql.Data.MySqlClient

Public Class ApplicationLetterView
    Private applicationId As Integer

    Public Sub New(appId As Integer)
        InitializeComponent()
        Me.applicationId = appId
    End Sub

    Private Sub ApplicationLetterView_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If applicationId = -1 Then
            ' No application yet — show default placeholders
            lblOFWName.Text = "[Your Name]"
            OFWEmail.Text = "[email@example.com]"
            lblDateApplicationSubmitted.Text = "Not yet submitted"
            lblAgencyName.Text = "[Agency Name]"
            LoadLetterHTML("You have not submitted an application for this job yet.")
        Else
            LoadApplicationDetails()
        End If
    End Sub

    Private Sub LoadApplicationDetails()
        Try
            Dim query As String = "
                SELECT 
                    o.FirstName, o.LastName,
                    u.email,
                    a.ApplicationDate,
                    a.LetterBody,
                    ag.AgencyName
                FROM application a
                LEFT JOIN ofw o ON a.OFWID = o.OFWID
                LEFT JOIN users u ON u.reference_id = o.OFWID AND u.user_type = 'OFW'
                LEFT JOIN agency ag ON a.AgencyID = ag.AgencyID
                WHERE a.ApplicationID = " & applicationId

            readQuery(query)

            If cmdRead IsNot Nothing AndAlso cmdRead.Read() Then
                ' OFW Name
                lblOFWName.Text = $"{cmdRead("FirstName")} {cmdRead("LastName")}"

                ' Email from users
                OFWEmail.Text = cmdRead("email").ToString()

                ' Date Submitted
                lblDateApplicationSubmitted.Text = Convert.ToDateTime(cmdRead("ApplicationDate")).ToString("MMMM dd, yyyy")

                ' Agency
                lblAgencyName.Text = cmdRead("AgencyName").ToString()

                ' Letter Content
                Dim letterBody As String = cmdRead("LetterBody").ToString()
                LoadLetterHTML(letterBody)
            Else
                lblOFWName.Text = "N/A"
                OFWEmail.Text = "N/A"
                lblDateApplicationSubmitted.Text = "N/A"
                lblAgencyName.Text = "N/A"
                LoadLetterHTML("No application letter found.")
            End If

            cmdRead?.Close()
        Catch ex As Exception
            MsgBox("Error loading application letter: " & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub LoadLetterHTML(bodyText As String)
        ' Simple HTML template for the WebBrowser
        Dim html As String = $"
            <html>
            <head>
                <style>
                    body {{
                        font-family: 'Segoe UI', sans-serif;
                        padding: 20px;
                        line-height: 1.6;
                    }}
                    .letter-body {{
                        white-space: pre-wrap;
                    }}
                </style>
            </head>
            <body>
                <div class='letter-body'>
                    {bodyText.Replace(vbCrLf, "<br>")}
                </div>
            </body>
            </html>"

        LetterContentWebBrowser.DocumentText = html
    End Sub

    Private Sub CloseBTN_Click(sender As Object, e As EventArgs) Handles CloseBTN.Click
        Me.Close()
    End Sub
End Class
