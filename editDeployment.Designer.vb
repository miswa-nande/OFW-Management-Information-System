<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class editDeployment
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
        Me.txtbxOfwId = New System.Windows.Forms.TextBox()
        Me.txtbxEmployerId = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.txtbxAgencyId = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.txtbxRemarks = New System.Windows.Forms.TextBox()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.cbxReason = New System.Windows.Forms.ComboBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.cbxRepatriationStat = New System.Windows.Forms.ComboBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.dateContractStart = New System.Windows.Forms.DateTimePicker()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.cbxDepStat = New System.Windows.Forms.ComboBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.txtbxContractNum = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtbxContractDuration = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.cbxCountry = New System.Windows.Forms.ComboBox()
        Me.dateDepDate = New System.Windows.Forms.DateTimePicker()
        Me.txtbxSalary = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'txtbxOfwId
        '
        Me.txtbxOfwId.Font = New System.Drawing.Font("Century Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtbxOfwId.Location = New System.Drawing.Point(27, 174)
        Me.txtbxOfwId.Name = "txtbxOfwId"
        Me.txtbxOfwId.Size = New System.Drawing.Size(224, 31)
        Me.txtbxOfwId.TabIndex = 83
        '
        'txtbxEmployerId
        '
        Me.txtbxEmployerId.Font = New System.Drawing.Font("Century Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtbxEmployerId.Location = New System.Drawing.Point(567, 174)
        Me.txtbxEmployerId.Name = "txtbxEmployerId"
        Me.txtbxEmployerId.Size = New System.Drawing.Size(224, 31)
        Me.txtbxEmployerId.TabIndex = 82
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Century Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.ForeColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(66, Byte), Integer), CType(CType(155, Byte), Integer))
        Me.Label11.Location = New System.Drawing.Point(563, 146)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(197, 22)
        Me.Label11.TabIndex = 81
        Me.Label11.Text = "Employer ID Number"
        '
        'txtbxAgencyId
        '
        Me.txtbxAgencyId.Font = New System.Drawing.Font("Century Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtbxAgencyId.Location = New System.Drawing.Point(295, 174)
        Me.txtbxAgencyId.Name = "txtbxAgencyId"
        Me.txtbxAgencyId.Size = New System.Drawing.Size(224, 31)
        Me.txtbxAgencyId.TabIndex = 80
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Century Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.ForeColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(66, Byte), Integer), CType(CType(155, Byte), Integer))
        Me.Label9.Location = New System.Drawing.Point(291, 146)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(188, 22)
        Me.Label9.TabIndex = 79
        Me.Label9.Text = "Agency ID Number"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Century Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.ForeColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(66, Byte), Integer), CType(CType(155, Byte), Integer))
        Me.Label10.Location = New System.Drawing.Point(22, 146)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(156, 22)
        Me.Label10.TabIndex = 78
        Me.Label10.Text = "OFW ID Number"
        '
        'btnSave
        '
        Me.btnSave.BackColor = System.Drawing.Color.LightGreen
        Me.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnSave.Location = New System.Drawing.Point(444, 773)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(168, 56)
        Me.btnSave.TabIndex = 77
        Me.btnSave.Text = "SAVE"
        Me.btnSave.UseVisualStyleBackColor = False
        '
        'btnCancel
        '
        Me.btnCancel.BackColor = System.Drawing.Color.Tomato
        Me.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnCancel.Location = New System.Drawing.Point(624, 773)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(168, 56)
        Me.btnCancel.TabIndex = 76
        Me.btnCancel.Text = "CANCEL"
        Me.btnCancel.UseVisualStyleBackColor = False
        '
        'txtbxRemarks
        '
        Me.txtbxRemarks.Font = New System.Drawing.Font("Century Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtbxRemarks.Location = New System.Drawing.Point(28, 595)
        Me.txtbxRemarks.Multiline = True
        Me.txtbxRemarks.Name = "txtbxRemarks"
        Me.txtbxRemarks.Size = New System.Drawing.Size(764, 160)
        Me.txtbxRemarks.TabIndex = 75
        Me.txtbxRemarks.Text = "" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Font = New System.Drawing.Font("Century Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.ForeColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(66, Byte), Integer), CType(CType(155, Byte), Integer))
        Me.Label14.Location = New System.Drawing.Point(24, 568)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(97, 22)
        Me.Label14.TabIndex = 74
        Me.Label14.Text = "Remarks :"
        '
        'cbxReason
        '
        Me.cbxReason.Font = New System.Drawing.Font("Century Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbxReason.FormattingEnabled = True
        Me.cbxReason.Location = New System.Drawing.Point(296, 532)
        Me.cbxReason.Name = "cbxReason"
        Me.cbxReason.Size = New System.Drawing.Size(221, 30)
        Me.cbxReason.TabIndex = 73
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Century Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.ForeColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(66, Byte), Integer), CType(CType(155, Byte), Integer))
        Me.Label13.Location = New System.Drawing.Point(291, 504)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(280, 22)
        Me.Label13.TabIndex = 72
        Me.Label13.Text = "Reason of Return (if returned)"
        '
        'cbxRepatriationStat
        '
        Me.cbxRepatriationStat.Font = New System.Drawing.Font("Century Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbxRepatriationStat.FormattingEnabled = True
        Me.cbxRepatriationStat.Location = New System.Drawing.Point(28, 532)
        Me.cbxRepatriationStat.Name = "cbxRepatriationStat"
        Me.cbxRepatriationStat.Size = New System.Drawing.Size(221, 30)
        Me.cbxRepatriationStat.TabIndex = 71
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Century Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.ForeColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(66, Byte), Integer), CType(CType(155, Byte), Integer))
        Me.Label12.Location = New System.Drawing.Point(23, 504)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(183, 22)
        Me.Label12.TabIndex = 70
        Me.Label12.Text = "Repatriation Status"
        '
        'dateContractStart
        '
        Me.dateContractStart.Font = New System.Drawing.Font("Century Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dateContractStart.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dateContractStart.Location = New System.Drawing.Point(567, 289)
        Me.dateContractStart.Name = "dateContractStart"
        Me.dateContractStart.Size = New System.Drawing.Size(222, 31)
        Me.dateContractStart.TabIndex = 69
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Century Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.ForeColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(66, Byte), Integer), CType(CType(155, Byte), Integer))
        Me.Label8.Location = New System.Drawing.Point(564, 261)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(192, 22)
        Me.Label8.TabIndex = 68
        Me.Label8.Text = "Contract Start Date"
        '
        'cbxDepStat
        '
        Me.cbxDepStat.Font = New System.Drawing.Font("Century Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbxDepStat.FormattingEnabled = True
        Me.cbxDepStat.Location = New System.Drawing.Point(297, 417)
        Me.cbxDepStat.Name = "cbxDepStat"
        Me.cbxDepStat.Size = New System.Drawing.Size(221, 30)
        Me.cbxDepStat.TabIndex = 67
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Century Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(66, Byte), Integer), CType(CType(155, Byte), Integer))
        Me.Label7.Location = New System.Drawing.Point(292, 389)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(183, 22)
        Me.Label7.TabIndex = 66
        Me.Label7.Text = "Deployment Status"
        '
        'txtbxContractNum
        '
        Me.txtbxContractNum.Font = New System.Drawing.Font("Century Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtbxContractNum.Location = New System.Drawing.Point(296, 353)
        Me.txtbxContractNum.Name = "txtbxContractNum"
        Me.txtbxContractNum.Size = New System.Drawing.Size(224, 31)
        Me.txtbxContractNum.TabIndex = 65
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Century Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(66, Byte), Integer), CType(CType(155, Byte), Integer))
        Me.Label6.Location = New System.Drawing.Point(292, 325)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(173, 22)
        Me.Label6.TabIndex = 64
        Me.Label6.Text = "Contract Number"
        '
        'txtbxContractDuration
        '
        Me.txtbxContractDuration.Font = New System.Drawing.Font("Century Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtbxContractDuration.Location = New System.Drawing.Point(296, 289)
        Me.txtbxContractDuration.Name = "txtbxContractDuration"
        Me.txtbxContractDuration.Size = New System.Drawing.Size(224, 31)
        Me.txtbxContractDuration.TabIndex = 63
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Century Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(66, Byte), Integer), CType(CType(155, Byte), Integer))
        Me.Label5.Location = New System.Drawing.Point(289, 261)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(258, 22)
        Me.Label5.TabIndex = 62
        Me.Label5.Text = "Contract Duration (in mos.)"
        '
        'cbxCountry
        '
        Me.cbxCountry.Font = New System.Drawing.Font("Century Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbxCountry.FormattingEnabled = True
        Me.cbxCountry.Location = New System.Drawing.Point(28, 353)
        Me.cbxCountry.Name = "cbxCountry"
        Me.cbxCountry.Size = New System.Drawing.Size(221, 30)
        Me.cbxCountry.TabIndex = 61
        '
        'dateDepDate
        '
        Me.dateDepDate.Font = New System.Drawing.Font("Century Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dateDepDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dateDepDate.Location = New System.Drawing.Point(27, 289)
        Me.dateDepDate.Name = "dateDepDate"
        Me.dateDepDate.Size = New System.Drawing.Size(222, 31)
        Me.dateDepDate.TabIndex = 60
        '
        'txtbxSalary
        '
        Me.txtbxSalary.Font = New System.Drawing.Font("Century Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtbxSalary.Location = New System.Drawing.Point(27, 417)
        Me.txtbxSalary.Name = "txtbxSalary"
        Me.txtbxSalary.Size = New System.Drawing.Size(224, 31)
        Me.txtbxSalary.TabIndex = 59
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Century Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(66, Byte), Integer), CType(CType(155, Byte), Integer))
        Me.Label4.Location = New System.Drawing.Point(23, 389)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(142, 22)
        Me.Label4.TabIndex = 58
        Me.Label4.Text = "Salary Offered"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Century Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(66, Byte), Integer), CType(CType(155, Byte), Integer))
        Me.Label3.Location = New System.Drawing.Point(23, 325)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(226, 22)
        Me.Label3.TabIndex = 57
        Me.Label3.Text = "Country of Deployment"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Century Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(66, Byte), Integer), CType(CType(155, Byte), Integer))
        Me.Label2.Location = New System.Drawing.Point(23, 261)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(174, 22)
        Me.Label2.TabIndex = 56
        Me.Label2.Text = "Deployment Date"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Century Gothic", 48.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(66, Byte), Integer), CType(CType(155, Byte), Integer))
        Me.Label1.Location = New System.Drawing.Point(135, 36)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(585, 78)
        Me.Label1.TabIndex = 55
        Me.Label1.Text = "EDIT DEPLOYMENT"
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = Global.OFW_Management_Information_System.My.Resources.Resources.edit_ic
        Me.PictureBox1.Location = New System.Drawing.Point(28, 26)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(101, 99)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 54
        Me.PictureBox1.TabStop = False
        '
        'editDeployment
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(11.0!, 22.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.ClientSize = New System.Drawing.Size(815, 854)
        Me.Controls.Add(Me.txtbxOfwId)
        Me.Controls.Add(Me.txtbxEmployerId)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.txtbxAgencyId)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.txtbxRemarks)
        Me.Controls.Add(Me.Label14)
        Me.Controls.Add(Me.cbxReason)
        Me.Controls.Add(Me.Label13)
        Me.Controls.Add(Me.cbxRepatriationStat)
        Me.Controls.Add(Me.Label12)
        Me.Controls.Add(Me.dateContractStart)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.cbxDepStat)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.txtbxContractNum)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.txtbxContractDuration)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.cbxCountry)
        Me.Controls.Add(Me.dateDepDate)
        Me.Controls.Add(Me.txtbxSalary)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.PictureBox1)
        Me.Font = New System.Drawing.Font("Century Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.Name = "editDeployment"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "ADMIN | Edit Deployment"
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents txtbxOfwId As TextBox
    Friend WithEvents txtbxEmployerId As TextBox
    Friend WithEvents Label11 As Label
    Friend WithEvents txtbxAgencyId As TextBox
    Friend WithEvents Label9 As Label
    Friend WithEvents Label10 As Label
    Friend WithEvents btnSave As Button
    Friend WithEvents btnCancel As Button
    Friend WithEvents txtbxRemarks As TextBox
    Friend WithEvents Label14 As Label
    Friend WithEvents cbxReason As ComboBox
    Friend WithEvents Label13 As Label
    Friend WithEvents cbxRepatriationStat As ComboBox
    Friend WithEvents Label12 As Label
    Friend WithEvents dateContractStart As DateTimePicker
    Friend WithEvents Label8 As Label
    Friend WithEvents cbxDepStat As ComboBox
    Friend WithEvents Label7 As Label
    Friend WithEvents txtbxContractNum As TextBox
    Friend WithEvents Label6 As Label
    Friend WithEvents txtbxContractDuration As TextBox
    Friend WithEvents Label5 As Label
    Friend WithEvents cbxCountry As ComboBox
    Friend WithEvents dateDepDate As DateTimePicker
    Friend WithEvents txtbxSalary As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents PictureBox1 As PictureBox
End Class
