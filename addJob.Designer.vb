<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class addJob
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
        Me.txtbxBenefits = New System.Windows.Forms.TextBox()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.cbxCountryOfEmployment = New System.Windows.Forms.ComboBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.txtbxSalaryRange = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.txtbxJobTitle = New System.Windows.Forms.TextBox()
        Me.txtbxEmployerIdNum = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.btnAdd = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.txtbxJobDescription = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtbxReqSkill = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.txtbxJobType = New System.Windows.Forms.TextBox()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.cbxVisaType = New System.Windows.Forms.ComboBox()
        Me.txtbxConditions = New System.Windows.Forms.TextBox()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.txtbxNumOfVacancies = New System.Windows.Forms.TextBox()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.cbxJobStatus = New System.Windows.Forms.ComboBox()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.dateApplicationDeadline = New System.Windows.Forms.DateTimePicker()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.txtbxContractDuration = New System.Windows.Forms.TextBox()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'txtbxBenefits
        '
        Me.txtbxBenefits.Font = New System.Drawing.Font("Century Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtbxBenefits.Location = New System.Drawing.Point(32, 700)
        Me.txtbxBenefits.Multiline = True
        Me.txtbxBenefits.Name = "txtbxBenefits"
        Me.txtbxBenefits.Size = New System.Drawing.Size(666, 112)
        Me.txtbxBenefits.TabIndex = 167
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Font = New System.Drawing.Font("Century Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.ForeColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(66, Byte), Integer), CType(CType(155, Byte), Integer))
        Me.Label15.Location = New System.Drawing.Point(28, 674)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(90, 22)
        Me.Label15.TabIndex = 166
        Me.Label15.Text = "Benefits :"
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Font = New System.Drawing.Font("Century Gothic", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.ForeColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(66, Byte), Integer), CType(CType(155, Byte), Integer))
        Me.Label14.Location = New System.Drawing.Point(495, 349)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(141, 20)
        Me.Label14.TabIndex = 164
        Me.Label14.Text = "Contract Duration"
        '
        'cbxCountryOfEmployment
        '
        Me.cbxCountryOfEmployment.Font = New System.Drawing.Font("Century Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbxCountryOfEmployment.FormattingEnabled = True
        Me.cbxCountryOfEmployment.Location = New System.Drawing.Point(33, 373)
        Me.cbxCountryOfEmployment.Name = "cbxCountryOfEmployment"
        Me.cbxCountryOfEmployment.Size = New System.Drawing.Size(212, 30)
        Me.cbxCountryOfEmployment.TabIndex = 163
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Century Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.ForeColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(66, Byte), Integer), CType(CType(155, Byte), Integer))
        Me.Label13.Location = New System.Drawing.Point(29, 347)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(227, 22)
        Me.Label13.TabIndex = 162
        Me.Label13.Text = "Country of Employment"
        '
        'txtbxSalaryRange
        '
        Me.txtbxSalaryRange.Font = New System.Drawing.Font("Century Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtbxSalaryRange.Location = New System.Drawing.Point(266, 373)
        Me.txtbxSalaryRange.Name = "txtbxSalaryRange"
        Me.txtbxSalaryRange.Size = New System.Drawing.Size(212, 31)
        Me.txtbxSalaryRange.TabIndex = 159
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Century Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.ForeColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(66, Byte), Integer), CType(CType(155, Byte), Integer))
        Me.Label11.Location = New System.Drawing.Point(262, 347)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(132, 22)
        Me.Label11.TabIndex = 158
        Me.Label11.Text = "Salary Range"
        '
        'txtbxJobTitle
        '
        Me.txtbxJobTitle.Font = New System.Drawing.Font("Century Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtbxJobTitle.Location = New System.Drawing.Point(31, 173)
        Me.txtbxJobTitle.Name = "txtbxJobTitle"
        Me.txtbxJobTitle.Size = New System.Drawing.Size(440, 31)
        Me.txtbxJobTitle.TabIndex = 147
        '
        'txtbxEmployerIdNum
        '
        Me.txtbxEmployerIdNum.Font = New System.Drawing.Font("Century Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtbxEmployerIdNum.Location = New System.Drawing.Point(499, 173)
        Me.txtbxEmployerIdNum.Name = "txtbxEmployerIdNum"
        Me.txtbxEmployerIdNum.Size = New System.Drawing.Size(199, 31)
        Me.txtbxEmployerIdNum.TabIndex = 146
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Century Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.ForeColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(66, Byte), Integer), CType(CType(155, Byte), Integer))
        Me.Label9.Location = New System.Drawing.Point(495, 145)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(169, 21)
        Me.Label9.TabIndex = 145
        Me.Label9.Text = "Employer ID Number"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Century Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.ForeColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(66, Byte), Integer), CType(CType(155, Byte), Integer))
        Me.Label10.Location = New System.Drawing.Point(26, 147)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(82, 22)
        Me.Label10.TabIndex = 144
        Me.Label10.Text = "Job Title"
        '
        'btnAdd
        '
        Me.btnAdd.BackColor = System.Drawing.Color.LightGreen
        Me.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnAdd.Font = New System.Drawing.Font("Century Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAdd.Location = New System.Drawing.Point(350, 840)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(168, 56)
        Me.btnAdd.TabIndex = 143
        Me.btnAdd.Text = "ADD"
        Me.btnAdd.UseVisualStyleBackColor = False
        '
        'btnCancel
        '
        Me.btnCancel.BackColor = System.Drawing.Color.Tomato
        Me.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnCancel.Font = New System.Drawing.Font("Century Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCancel.Location = New System.Drawing.Point(530, 840)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(168, 56)
        Me.btnCancel.TabIndex = 142
        Me.btnCancel.Text = "CANCEL"
        Me.btnCancel.UseVisualStyleBackColor = False
        '
        'txtbxJobDescription
        '
        Me.txtbxJobDescription.Font = New System.Drawing.Font("Century Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtbxJobDescription.Location = New System.Drawing.Point(32, 234)
        Me.txtbxJobDescription.Multiline = True
        Me.txtbxJobDescription.Name = "txtbxJobDescription"
        Me.txtbxJobDescription.Size = New System.Drawing.Size(666, 109)
        Me.txtbxJobDescription.TabIndex = 139
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Century Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(66, Byte), Integer), CType(CType(155, Byte), Integer))
        Me.Label5.Location = New System.Drawing.Point(25, 208)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(150, 22)
        Me.Label5.TabIndex = 138
        Me.Label5.Text = "Job Description"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Century Gothic", 48.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(66, Byte), Integer), CType(CType(155, Byte), Integer))
        Me.Label1.Location = New System.Drawing.Point(139, 37)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(316, 78)
        Me.Label1.TabIndex = 137
        Me.Label1.Text = "ADD JOB"
        '
        'txtbxReqSkill
        '
        Me.txtbxReqSkill.Font = New System.Drawing.Font("Century Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtbxReqSkill.Location = New System.Drawing.Point(33, 463)
        Me.txtbxReqSkill.Name = "txtbxReqSkill"
        Me.txtbxReqSkill.Size = New System.Drawing.Size(212, 31)
        Me.txtbxReqSkill.TabIndex = 169
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Century Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(66, Byte), Integer), CType(CType(155, Byte), Integer))
        Me.Label2.Location = New System.Drawing.Point(29, 437)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(126, 22)
        Me.Label2.TabIndex = 168
        Me.Label2.Text = "Required Skill"
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Font = New System.Drawing.Font("Century Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.ForeColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(66, Byte), Integer), CType(CType(155, Byte), Integer))
        Me.Label16.Location = New System.Drawing.Point(489, 437)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(97, 22)
        Me.Label16.TabIndex = 172
        Me.Label16.Text = "Visa Type"
        '
        'txtbxJobType
        '
        Me.txtbxJobType.Font = New System.Drawing.Font("Century Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtbxJobType.Location = New System.Drawing.Point(266, 463)
        Me.txtbxJobType.Name = "txtbxJobType"
        Me.txtbxJobType.Size = New System.Drawing.Size(212, 31)
        Me.txtbxJobType.TabIndex = 171
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Font = New System.Drawing.Font("Century Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label17.ForeColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(66, Byte), Integer), CType(CType(155, Byte), Integer))
        Me.Label17.Location = New System.Drawing.Point(262, 437)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(93, 22)
        Me.Label17.TabIndex = 170
        Me.Label17.Text = "Job Type"
        '
        'cbxVisaType
        '
        Me.cbxVisaType.Font = New System.Drawing.Font("Century Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbxVisaType.FormattingEnabled = True
        Me.cbxVisaType.Location = New System.Drawing.Point(499, 464)
        Me.cbxVisaType.Name = "cbxVisaType"
        Me.cbxVisaType.Size = New System.Drawing.Size(199, 30)
        Me.cbxVisaType.TabIndex = 173
        '
        'txtbxConditions
        '
        Me.txtbxConditions.Font = New System.Drawing.Font("Century Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtbxConditions.Location = New System.Drawing.Point(32, 592)
        Me.txtbxConditions.Multiline = True
        Me.txtbxConditions.Name = "txtbxConditions"
        Me.txtbxConditions.Size = New System.Drawing.Size(666, 81)
        Me.txtbxConditions.TabIndex = 175
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Font = New System.Drawing.Font("Century Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label18.ForeColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(66, Byte), Integer), CType(CType(155, Byte), Integer))
        Me.Label18.Location = New System.Drawing.Point(25, 566)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(116, 22)
        Me.Label18.TabIndex = 174
        Me.Label18.Text = "Conditions :"
        '
        'txtbxNumOfVacancies
        '
        Me.txtbxNumOfVacancies.Font = New System.Drawing.Font("Century Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtbxNumOfVacancies.Location = New System.Drawing.Point(33, 526)
        Me.txtbxNumOfVacancies.Name = "txtbxNumOfVacancies"
        Me.txtbxNumOfVacancies.Size = New System.Drawing.Size(212, 31)
        Me.txtbxNumOfVacancies.TabIndex = 177
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Font = New System.Drawing.Font("Century Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label19.ForeColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(66, Byte), Integer), CType(CType(155, Byte), Integer))
        Me.Label19.Location = New System.Drawing.Point(29, 500)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(211, 22)
        Me.Label19.TabIndex = 176
        Me.Label19.Text = "Number of Vacancies"
        '
        'cbxJobStatus
        '
        Me.cbxJobStatus.Font = New System.Drawing.Font("Century Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbxJobStatus.FormattingEnabled = True
        Me.cbxJobStatus.Location = New System.Drawing.Point(499, 526)
        Me.cbxJobStatus.Name = "cbxJobStatus"
        Me.cbxJobStatus.Size = New System.Drawing.Size(199, 30)
        Me.cbxJobStatus.TabIndex = 181
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Font = New System.Drawing.Font("Century Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label20.ForeColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(66, Byte), Integer), CType(CType(155, Byte), Integer))
        Me.Label20.Location = New System.Drawing.Point(489, 499)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(104, 22)
        Me.Label20.TabIndex = 180
        Me.Label20.Text = "Job Status"
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.Font = New System.Drawing.Font("Century Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label21.ForeColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(66, Byte), Integer), CType(CType(155, Byte), Integer))
        Me.Label21.Location = New System.Drawing.Point(262, 499)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(203, 22)
        Me.Label21.TabIndex = 178
        Me.Label21.Text = "Application Deadline"
        '
        'dateApplicationDeadline
        '
        Me.dateApplicationDeadline.Font = New System.Drawing.Font("Century Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dateApplicationDeadline.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dateApplicationDeadline.Location = New System.Drawing.Point(266, 526)
        Me.dateApplicationDeadline.Name = "dateApplicationDeadline"
        Me.dateApplicationDeadline.Size = New System.Drawing.Size(212, 31)
        Me.dateApplicationDeadline.TabIndex = 182
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = Global.OFW_Management_Information_System.My.Resources.Resources.addjob_ic
        Me.PictureBox1.Location = New System.Drawing.Point(32, 27)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(101, 99)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 136
        Me.PictureBox1.TabStop = False
        '
        'txtbxContractDuration
        '
        Me.txtbxContractDuration.Font = New System.Drawing.Font("Century Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtbxContractDuration.Location = New System.Drawing.Point(499, 373)
        Me.txtbxContractDuration.Name = "txtbxContractDuration"
        Me.txtbxContractDuration.Size = New System.Drawing.Size(199, 31)
        Me.txtbxContractDuration.TabIndex = 205
        '
        'addJob
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.ClientSize = New System.Drawing.Size(732, 925)
        Me.Controls.Add(Me.txtbxContractDuration)
        Me.Controls.Add(Me.dateApplicationDeadline)
        Me.Controls.Add(Me.cbxJobStatus)
        Me.Controls.Add(Me.Label20)
        Me.Controls.Add(Me.Label21)
        Me.Controls.Add(Me.txtbxNumOfVacancies)
        Me.Controls.Add(Me.Label19)
        Me.Controls.Add(Me.txtbxConditions)
        Me.Controls.Add(Me.Label18)
        Me.Controls.Add(Me.cbxVisaType)
        Me.Controls.Add(Me.Label16)
        Me.Controls.Add(Me.txtbxJobType)
        Me.Controls.Add(Me.Label17)
        Me.Controls.Add(Me.txtbxReqSkill)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtbxBenefits)
        Me.Controls.Add(Me.Label15)
        Me.Controls.Add(Me.Label14)
        Me.Controls.Add(Me.cbxCountryOfEmployment)
        Me.Controls.Add(Me.Label13)
        Me.Controls.Add(Me.txtbxSalaryRange)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.txtbxJobTitle)
        Me.Controls.Add(Me.txtbxEmployerIdNum)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.btnAdd)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.txtbxJobDescription)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.PictureBox1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "addJob"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Add Job Placement"
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents txtbxBenefits As TextBox
    Friend WithEvents Label15 As Label
    Friend WithEvents Label14 As Label
    Friend WithEvents cbxCountryOfEmployment As ComboBox
    Friend WithEvents Label13 As Label
    Friend WithEvents txtbxSalaryRange As TextBox
    Friend WithEvents Label11 As Label
    Friend WithEvents txtbxJobTitle As TextBox
    Friend WithEvents txtbxEmployerIdNum As TextBox
    Friend WithEvents Label9 As Label
    Friend WithEvents Label10 As Label
    Friend WithEvents btnAdd As Button
    Friend WithEvents btnCancel As Button
    Friend WithEvents txtbxJobDescription As TextBox
    Friend WithEvents Label5 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents txtbxReqSkill As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents Label16 As Label
    Friend WithEvents txtbxJobType As TextBox
    Friend WithEvents Label17 As Label
    Friend WithEvents cbxVisaType As ComboBox
    Friend WithEvents txtbxConditions As TextBox
    Friend WithEvents Label18 As Label
    Friend WithEvents txtbxNumOfVacancies As TextBox
    Friend WithEvents Label19 As Label
    Friend WithEvents cbxJobStatus As ComboBox
    Friend WithEvents Label20 As Label
    Friend WithEvents Label21 As Label
    Friend WithEvents dateApplicationDeadline As DateTimePicker
    Friend WithEvents txtbxContractDuration As TextBox
End Class
