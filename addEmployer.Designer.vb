<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class addEmployer
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.txtbxFName = New System.Windows.Forms.TextBox()
        Me.txtbxLName = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.txtbxMName = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.btnAdd = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.txtbxContactNum = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtbxCompanyName = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtbxEmail = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtbxStreet = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtbxIndustry = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtbxCity = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.txtbxZipcode = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txtbxState = New System.Windows.Forms.TextBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.txtbxCountry = New System.Windows.Forms.TextBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'txtbxFName
        '
        Me.txtbxFName.Font = New System.Drawing.Font("Century Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtbxFName.Location = New System.Drawing.Point(27, 174)
        Me.txtbxFName.Name = "txtbxFName"
        Me.txtbxFName.Size = New System.Drawing.Size(246, 31)
        Me.txtbxFName.TabIndex = 83
        '
        'txtbxLName
        '
        Me.txtbxLName.Font = New System.Drawing.Font("Century Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtbxLName.Location = New System.Drawing.Point(531, 174)
        Me.txtbxLName.Name = "txtbxLName"
        Me.txtbxLName.Size = New System.Drawing.Size(246, 31)
        Me.txtbxLName.TabIndex = 82
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Century Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.ForeColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(66, Byte), Integer), CType(CType(155, Byte), Integer))
        Me.Label11.Location = New System.Drawing.Point(527, 146)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(107, 22)
        Me.Label11.TabIndex = 81
        Me.Label11.Text = "Last Name"
        '
        'txtbxMName
        '
        Me.txtbxMName.Font = New System.Drawing.Font("Century Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtbxMName.Location = New System.Drawing.Point(279, 174)
        Me.txtbxMName.Name = "txtbxMName"
        Me.txtbxMName.Size = New System.Drawing.Size(246, 31)
        Me.txtbxMName.TabIndex = 80
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Century Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.ForeColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(66, Byte), Integer), CType(CType(155, Byte), Integer))
        Me.Label9.Location = New System.Drawing.Point(275, 146)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(132, 22)
        Me.Label9.TabIndex = 79
        Me.Label9.Text = "Middle Name"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Century Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.ForeColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(66, Byte), Integer), CType(CType(155, Byte), Integer))
        Me.Label10.Location = New System.Drawing.Point(22, 146)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(103, 22)
        Me.Label10.TabIndex = 78
        Me.Label10.Text = "First Name"
        '
        'btnAdd
        '
        Me.btnAdd.BackColor = System.Drawing.Color.LightGreen
        Me.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnAdd.Font = New System.Drawing.Font("Century Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAdd.Location = New System.Drawing.Point(429, 646)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(168, 56)
        Me.btnAdd.TabIndex = 77
        Me.btnAdd.Text = "ADD"
        Me.btnAdd.UseVisualStyleBackColor = False
        '
        'btnCancel
        '
        Me.btnCancel.BackColor = System.Drawing.Color.Tomato
        Me.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnCancel.Font = New System.Drawing.Font("Century Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCancel.Location = New System.Drawing.Point(609, 646)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(168, 56)
        Me.btnCancel.TabIndex = 76
        Me.btnCancel.Text = "CANCEL"
        Me.btnCancel.UseVisualStyleBackColor = False
        '
        'txtbxContactNum
        '
        Me.txtbxContactNum.Font = New System.Drawing.Font("Century Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtbxContactNum.Location = New System.Drawing.Point(28, 238)
        Me.txtbxContactNum.Name = "txtbxContactNum"
        Me.txtbxContactNum.Size = New System.Drawing.Size(224, 31)
        Me.txtbxContactNum.TabIndex = 65
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Century Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(66, Byte), Integer), CType(CType(155, Byte), Integer))
        Me.Label6.Location = New System.Drawing.Point(24, 210)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(167, 22)
        Me.Label6.TabIndex = 64
        Me.Label6.Text = "Contact Number"
        '
        'txtbxCompanyName
        '
        Me.txtbxCompanyName.Font = New System.Drawing.Font("Century Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtbxCompanyName.Location = New System.Drawing.Point(28, 331)
        Me.txtbxCompanyName.Name = "txtbxCompanyName"
        Me.txtbxCompanyName.Size = New System.Drawing.Size(497, 31)
        Me.txtbxCompanyName.TabIndex = 59
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Century Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(66, Byte), Integer), CType(CType(155, Byte), Integer))
        Me.Label4.Location = New System.Drawing.Point(24, 303)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(164, 22)
        Me.Label4.TabIndex = 58
        Me.Label4.Text = "Company Name"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Century Gothic", 48.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(66, Byte), Integer), CType(CType(155, Byte), Integer))
        Me.Label1.Location = New System.Drawing.Point(135, 36)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(520, 78)
        Me.Label1.TabIndex = 55
        Me.Label1.Text = "ADD EMPLOYER"
        '
        'txtbxEmail
        '
        Me.txtbxEmail.Font = New System.Drawing.Font("Century Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtbxEmail.Location = New System.Drawing.Point(279, 238)
        Me.txtbxEmail.Name = "txtbxEmail"
        Me.txtbxEmail.Size = New System.Drawing.Size(224, 31)
        Me.txtbxEmail.TabIndex = 85
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Century Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(66, Byte), Integer), CType(CType(155, Byte), Integer))
        Me.Label2.Location = New System.Drawing.Point(275, 210)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(56, 22)
        Me.Label2.TabIndex = 84
        Me.Label2.Text = "Email"
        '
        'txtbxStreet
        '
        Me.txtbxStreet.Font = New System.Drawing.Font("Century Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtbxStreet.Location = New System.Drawing.Point(28, 392)
        Me.txtbxStreet.Name = "txtbxStreet"
        Me.txtbxStreet.Size = New System.Drawing.Size(497, 31)
        Me.txtbxStreet.TabIndex = 87
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Century Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(66, Byte), Integer), CType(CType(155, Byte), Integer))
        Me.Label3.Location = New System.Drawing.Point(24, 366)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(63, 22)
        Me.Label3.TabIndex = 86
        Me.Label3.Text = "Street"
        '
        'txtbxIndustry
        '
        Me.txtbxIndustry.Font = New System.Drawing.Font("Century Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtbxIndustry.Location = New System.Drawing.Point(531, 331)
        Me.txtbxIndustry.Name = "txtbxIndustry"
        Me.txtbxIndustry.Size = New System.Drawing.Size(246, 31)
        Me.txtbxIndustry.TabIndex = 89
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Century Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(66, Byte), Integer), CType(CType(155, Byte), Integer))
        Me.Label5.Location = New System.Drawing.Point(527, 303)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(83, 22)
        Me.Label5.TabIndex = 88
        Me.Label5.Text = "Industry"
        '
        'txtbxCity
        '
        Me.txtbxCity.Font = New System.Drawing.Font("Century Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtbxCity.Location = New System.Drawing.Point(28, 453)
        Me.txtbxCity.Name = "txtbxCity"
        Me.txtbxCity.Size = New System.Drawing.Size(497, 31)
        Me.txtbxCity.TabIndex = 91
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Century Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(66, Byte), Integer), CType(CType(155, Byte), Integer))
        Me.Label7.Location = New System.Drawing.Point(24, 427)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(46, 22)
        Me.Label7.TabIndex = 90
        Me.Label7.Text = "City"
        '
        'txtbxZipcode
        '
        Me.txtbxZipcode.Font = New System.Drawing.Font("Century Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtbxZipcode.Location = New System.Drawing.Point(531, 575)
        Me.txtbxZipcode.Name = "txtbxZipcode"
        Me.txtbxZipcode.Size = New System.Drawing.Size(246, 31)
        Me.txtbxZipcode.TabIndex = 93
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Century Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.ForeColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(66, Byte), Integer), CType(CType(155, Byte), Integer))
        Me.Label8.Location = New System.Drawing.Point(527, 547)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(84, 22)
        Me.Label8.TabIndex = 92
        Me.Label8.Text = "Zipcode"
        '
        'txtbxState
        '
        Me.txtbxState.Font = New System.Drawing.Font("Century Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtbxState.Location = New System.Drawing.Point(28, 514)
        Me.txtbxState.Name = "txtbxState"
        Me.txtbxState.Size = New System.Drawing.Size(497, 31)
        Me.txtbxState.TabIndex = 95
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Century Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.ForeColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(66, Byte), Integer), CType(CType(155, Byte), Integer))
        Me.Label12.Location = New System.Drawing.Point(24, 488)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(58, 22)
        Me.Label12.TabIndex = 94
        Me.Label12.Text = "State"
        '
        'txtbxCountry
        '
        Me.txtbxCountry.Font = New System.Drawing.Font("Century Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtbxCountry.Location = New System.Drawing.Point(28, 575)
        Me.txtbxCountry.Name = "txtbxCountry"
        Me.txtbxCountry.Size = New System.Drawing.Size(497, 31)
        Me.txtbxCountry.TabIndex = 97
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Century Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.ForeColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(66, Byte), Integer), CType(CType(155, Byte), Integer))
        Me.Label13.Location = New System.Drawing.Point(24, 549)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(85, 22)
        Me.Label13.TabIndex = 96
        Me.Label13.Text = "Country"
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = Global.OFW_Management_Information_System.My.Resources.Resources.add_ic
        Me.PictureBox1.Location = New System.Drawing.Point(28, 26)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(101, 99)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 54
        Me.PictureBox1.TabStop = False
        '
        'addEmployer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(11.0!, 22.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.ClientSize = New System.Drawing.Size(799, 731)
        Me.Controls.Add(Me.txtbxCountry)
        Me.Controls.Add(Me.Label13)
        Me.Controls.Add(Me.txtbxState)
        Me.Controls.Add(Me.Label12)
        Me.Controls.Add(Me.txtbxZipcode)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.txtbxCity)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.txtbxIndustry)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.txtbxStreet)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.txtbxEmail)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtbxFName)
        Me.Controls.Add(Me.txtbxLName)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.txtbxMName)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.btnAdd)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.txtbxContactNum)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.txtbxCompanyName)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.PictureBox1)
        Me.Font = New System.Drawing.Font("Century Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.Name = "addEmployer"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "ADMIN | Add Employer"
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents txtbxFName As TextBox
    Friend WithEvents txtbxLName As TextBox
    Friend WithEvents Label11 As Label
    Friend WithEvents txtbxMName As TextBox
    Friend WithEvents Label9 As Label
    Friend WithEvents Label10 As Label
    Friend WithEvents btnAdd As Button
    Friend WithEvents btnCancel As Button
    Friend WithEvents txtbxContactNum As TextBox
    Friend WithEvents Label6 As Label
    Friend WithEvents txtbxCompanyName As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents txtbxEmail As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents txtbxStreet As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents txtbxIndustry As TextBox
    Friend WithEvents Label5 As Label
    Friend WithEvents txtbxCity As TextBox
    Friend WithEvents Label7 As Label
    Friend WithEvents txtbxZipcode As TextBox
    Friend WithEvents Label8 As Label
    Friend WithEvents txtbxState As TextBox
    Friend WithEvents Label12 As Label
    Friend WithEvents txtbxCountry As TextBox
    Friend WithEvents Label13 As Label
End Class
