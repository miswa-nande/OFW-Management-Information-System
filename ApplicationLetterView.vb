Imports MySql.Data.MySqlClient

Public Class ApplicationLetterView
    Private applicationId As Integer

    Public Sub New(Optional appId As Integer = -1)
        InitializeComponent()
        Me.applicationId = appId
    End Sub

    Private Sub ApplicationLetterView_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If applicationId = -1 Then
            LoadPreviewLetter()
        Else
            LoadApplicationDetails()
        End If
    End Sub

    ' Load letter based on application ID (normal mode)
    Private Sub LoadApplicationDetails()
        Try
            Dim query As String = "
                SELECT 
                    o.FirstName, o.LastName,
                    u.email,
                    a.ApplicationDate,
                    o.LetterBody,
                    ag.AgencyName
                FROM application a
                LEFT JOIN ofw o ON a.OFWID = o.OFWID
                LEFT JOIN users u ON u.reference_id = o.OFWID AND u.user_type = 'OFW'
                LEFT JOIN agency ag ON a.AgencyID = ag.AgencyID
                WHERE a.ApplicationID = " & applicationId

            readQuery(query)

            If cmdRead IsNot Nothing AndAlso cmdRead.Read() Then
                lblOFWName.Text = $"{cmdRead("FirstName")} {cmdRead("LastName")}"
                OFWEmail.Text = cmdRead("email").ToString()
                lblDateApplicationSubmitted.Text = Convert.ToDateTime(cmdRead("ApplicationDate")).ToString("MMMM dd, yyyy")
                lblAgencyName.Text = cmdRead("AgencyName").ToString()

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

    ' Load letter directly from OFW profile (preview mode)
    Private Sub LoadPreviewLetter()
        Try
            Dim query As String = "
                SELECT 
                    o.FirstName, o.LastName,
                    u.email,
                    o.LetterBody
                FROM ofw o
                LEFT JOIN users u ON u.reference_id = o.OFWID AND u.user_type = 'OFW'
                WHERE o.OFWID = " & Session.CurrentReferenceID

            readQuery(query)

            If cmdRead IsNot Nothing AndAlso cmdRead.Read() Then
                lblOFWName.Text = $"{cmdRead("FirstName")} {cmdRead("LastName")}"
                OFWEmail.Text = cmdRead("email").ToString()
                lblDateApplicationSubmitted.Text = "(Preview Mode)"
                lblAgencyName.Text = "(Not Yet Applied)"
                LoadLetterHTML(cmdRead("LetterBody").ToString())
            Else
                lblOFWName.Text = "N/A"
                OFWEmail.Text = "N/A"
                lblDateApplicationSubmitted.Text = "N/A"
                lblAgencyName.Text = "N/A"
                LoadLetterHTML("You have not written your letter yet.")
            End If

            cmdRead?.Close()
        Catch ex As Exception
            MsgBox("Error loading preview: " & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    ' Render the letter as styled HTML inside WebBrowser control
    Private Sub LoadLetterHTML(bodyText As String)
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
