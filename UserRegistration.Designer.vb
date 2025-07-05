<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class UserRegistration
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(UserRegistration))
        Me.TxtbxUserName = New System.Windows.Forms.TextBox()
        Me.CreateAccBTN = New System.Windows.Forms.Button()
        Me.UsernameLBL = New System.Windows.Forms.Label()
        Me.LblOR = New System.Windows.Forms.Label()
        Me.TxtbxEmail = New System.Windows.Forms.TextBox()
        Me.LblEmail = New System.Windows.Forms.Label()
        Me.txtbxPass = New System.Windows.Forms.TextBox()
        Me.lblPassword = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lblLogin = New System.Windows.Forms.Label()
        Me.logo = New System.Windows.Forms.PictureBox()
        CType(Me.logo, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TxtbxUserName
        '
        Me.TxtbxUserName.Font = New System.Drawing.Font("Century Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtbxUserName.Location = New System.Drawing.Point(1052, 549)
        Me.TxtbxUserName.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.TxtbxUserName.Name = "TxtbxUserName"
        Me.TxtbxUserName.Size = New System.Drawing.Size(433, 32)
        Me.TxtbxUserName.TabIndex = 0
        '
        'CreateAccBTN
        '
        Me.CreateAccBTN.Font = New System.Drawing.Font("Century Gothic", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CreateAccBTN.Location = New System.Drawing.Point(1053, 814)
        Me.CreateAccBTN.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.CreateAccBTN.Name = "CreateAccBTN"
        Me.CreateAccBTN.Size = New System.Drawing.Size(433, 46)
        Me.CreateAccBTN.TabIndex = 3
        Me.CreateAccBTN.Text = "Create Account"
        Me.CreateAccBTN.UseVisualStyleBackColor = True
        '
        'UsernameLBL
        '
        Me.UsernameLBL.AutoSize = True
        Me.UsernameLBL.Font = New System.Drawing.Font("Century Gothic", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.UsernameLBL.Location = New System.Drawing.Point(1047, 521)
        Me.UsernameLBL.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.UsernameLBL.Name = "UsernameLBL"
        Me.UsernameLBL.Size = New System.Drawing.Size(103, 23)
        Me.UsernameLBL.TabIndex = 4
        Me.UsernameLBL.Text = "Username"
        '
        'LblOR
        '
        Me.LblOR.AutoSize = True
        Me.LblOR.Font = New System.Drawing.Font("Century Gothic", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblOR.Location = New System.Drawing.Point(1248, 597)
        Me.LblOR.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.LblOR.Name = "LblOR"
        Me.LblOR.Size = New System.Drawing.Size(28, 23)
        Me.LblOR.TabIndex = 5
        Me.LblOR.Text = "or"
        '
        'TxtbxEmail
        '
        Me.TxtbxEmail.Font = New System.Drawing.Font("Century Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtbxEmail.Location = New System.Drawing.Point(1052, 659)
        Me.TxtbxEmail.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.TxtbxEmail.Name = "TxtbxEmail"
        Me.TxtbxEmail.Size = New System.Drawing.Size(433, 32)
        Me.TxtbxEmail.TabIndex = 6
        '
        'LblEmail
        '
        Me.LblEmail.AutoSize = True
        Me.LblEmail.Font = New System.Drawing.Font("Century Gothic", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblEmail.Location = New System.Drawing.Point(1048, 630)
        Me.LblEmail.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.LblEmail.Name = "LblEmail"
        Me.LblEmail.Size = New System.Drawing.Size(61, 23)
        Me.LblEmail.TabIndex = 7
        Me.LblEmail.Text = "Email"
        '
        'txtbxPass
        '
        Me.txtbxPass.Font = New System.Drawing.Font("Century Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtbxPass.Location = New System.Drawing.Point(1053, 733)
        Me.txtbxPass.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.txtbxPass.Name = "txtbxPass"
        Me.txtbxPass.PasswordChar = Global.Microsoft.VisualBasic.ChrW(9679)
        Me.txtbxPass.Size = New System.Drawing.Size(432, 32)
        Me.txtbxPass.TabIndex = 8
        '
        'lblPassword
        '
        Me.lblPassword.AutoSize = True
        Me.lblPassword.Font = New System.Drawing.Font("Century Gothic", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPassword.Location = New System.Drawing.Point(1048, 704)
        Me.lblPassword.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblPassword.Name = "lblPassword"
        Me.lblPassword.Size = New System.Drawing.Size(96, 23)
        Me.lblPassword.TabIndex = 9
        Me.lblPassword.Text = "Password"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Century Gothic", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(1105, 916)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(210, 20)
        Me.Label1.TabIndex = 10
        Me.Label1.Text = "Already Have an Account?"
        '
        'lblLogin
        '
        Me.lblLogin.AutoSize = True
        Me.lblLogin.Font = New System.Drawing.Font("Century Gothic", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLogin.Location = New System.Drawing.Point(1336, 916)
        Me.lblLogin.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblLogin.Name = "lblLogin"
        Me.lblLogin.Size = New System.Drawing.Size(52, 18)
        Me.lblLogin.TabIndex = 11
        Me.lblLogin.Text = "Log In"
        '
        'logo
        '
        Me.logo.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.logo.Image = CType(resources.GetObject("logo.Image"), System.Drawing.Image)
        Me.logo.Location = New System.Drawing.Point(993, 107)
        Me.logo.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.logo.Name = "logo"
        Me.logo.Size = New System.Drawing.Size(710, 474)
        Me.logo.TabIndex = 12
        Me.logo.TabStop = False
        '
        'UserRegistration
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(2382, 1303)
        Me.Controls.Add(Me.lblLogin)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.lblPassword)
        Me.Controls.Add(Me.txtbxPass)
        Me.Controls.Add(Me.LblEmail)
        Me.Controls.Add(Me.TxtbxEmail)
        Me.Controls.Add(Me.LblOR)
        Me.Controls.Add(Me.UsernameLBL)
        Me.Controls.Add(Me.CreateAccBTN)
        Me.Controls.Add(Me.TxtbxUserName)
        Me.Controls.Add(Me.logo)
        Me.ForeColor = System.Drawing.Color.Black
        Me.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.Name = "UserRegistration"
        Me.Text = "User Registration"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.logo, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents TxtbxUserName As TextBox
    Friend WithEvents CreateAccBTN As Button
    Friend WithEvents UsernameLBL As Label
    Friend WithEvents LblOR As Label
    Friend WithEvents TxtbxEmail As TextBox
    Friend WithEvents LblEmail As Label
    Friend WithEvents txtbxPass As TextBox
    Friend WithEvents lblPassword As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents lblLogin As Label
    Friend WithEvents logo As PictureBox
End Class
