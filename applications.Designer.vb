<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class applications
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
        Me.Label11 = New System.Windows.Forms.Label()
        Me.txtbxJobTitle = New System.Windows.Forms.TextBox()
        Me.cbxContractStat = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.dateContractStart = New System.Windows.Forms.DateTimePicker()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.txtbxContractNum = New System.Windows.Forms.TextBox()
        Me.btnApplications = New System.Windows.Forms.Button()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtbxIdNum = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.btnDeployment = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnJobOffers = New System.Windows.Forms.Button()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.PictureBox6 = New System.Windows.Forms.PictureBox()
        Me.PictureBox4 = New System.Windows.Forms.PictureBox()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.btnProfile = New System.Windows.Forms.Button()
        Me.logo = New System.Windows.Forms.PictureBox()
        Me.PictureBox5 = New System.Windows.Forms.PictureBox()
        Me.btnClear = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        CType(Me.PictureBox6, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.logo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox5, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Century Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.ForeColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(66, Byte), Integer), CType(CType(155, Byte), Integer))
        Me.Label11.Location = New System.Drawing.Point(2199, 288)
        Me.Label11.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(89, 23)
        Me.Label11.TabIndex = 182
        Me.Label11.Text = "Job Title"
        '
        'txtbxJobTitle
        '
        Me.txtbxJobTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtbxJobTitle.Font = New System.Drawing.Font("Century Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtbxJobTitle.Location = New System.Drawing.Point(2204, 320)
        Me.txtbxJobTitle.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.txtbxJobTitle.Name = "txtbxJobTitle"
        Me.txtbxJobTitle.Size = New System.Drawing.Size(258, 32)
        Me.txtbxJobTitle.TabIndex = 181
        '
        'cbxContractStat
        '
        Me.cbxContractStat.Font = New System.Drawing.Font("Century Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbxContractStat.FormattingEnabled = True
        Me.cbxContractStat.Location = New System.Drawing.Point(2243, 393)
        Me.cbxContractStat.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.cbxContractStat.Name = "cbxContractStat"
        Me.cbxContractStat.Size = New System.Drawing.Size(219, 31)
        Me.cbxContractStat.TabIndex = 176
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Century Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(66, Byte), Integer), CType(CType(155, Byte), Integer))
        Me.Label4.Location = New System.Drawing.Point(2237, 363)
        Me.Label4.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(187, 23)
        Me.Label4.TabIndex = 175
        Me.Label4.Text = "Application Status"
        '
        'dateContractStart
        '
        Me.dateContractStart.Font = New System.Drawing.Font("Century Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dateContractStart.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dateContractStart.Location = New System.Drawing.Point(2005, 395)
        Me.dateContractStart.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.dateContractStart.Name = "dateContractStart"
        Me.dateContractStart.Size = New System.Drawing.Size(219, 32)
        Me.dateContractStart.TabIndex = 174
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Font = New System.Drawing.Font("Century Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label17.ForeColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(66, Byte), Integer), CType(CType(155, Byte), Integer))
        Me.Label17.Location = New System.Drawing.Point(2000, 366)
        Me.Label17.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(164, 23)
        Me.Label17.TabIndex = 173
        Me.Label17.Text = "Date Submitted"
        '
        'txtbxContractNum
        '
        Me.txtbxContractNum.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtbxContractNum.Font = New System.Drawing.Font("Century Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtbxContractNum.Location = New System.Drawing.Point(2005, 466)
        Me.txtbxContractNum.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.txtbxContractNum.Name = "txtbxContractNum"
        Me.txtbxContractNum.Size = New System.Drawing.Size(457, 32)
        Me.txtbxContractNum.TabIndex = 170
        '
        'btnApplications
        '
        Me.btnApplications.BackColor = System.Drawing.SystemColors.ControlLightLight
        Me.btnApplications.Enabled = False
        Me.btnApplications.FlatAppearance.BorderSize = 0
        Me.btnApplications.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnApplications.Font = New System.Drawing.Font("Century Gothic", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnApplications.ForeColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(66, Byte), Integer), CType(CType(155, Byte), Integer))
        Me.btnApplications.Location = New System.Drawing.Point(17, 638)
        Me.btnApplications.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnApplications.Name = "btnApplications"
        Me.btnApplications.Size = New System.Drawing.Size(509, 123)
        Me.btnApplications.TabIndex = 4
        Me.btnApplications.Text = "APPLICATIONS"
        Me.btnApplications.UseVisualStyleBackColor = False
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Century Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(66, Byte), Integer), CType(CType(155, Byte), Integer))
        Me.Label7.Location = New System.Drawing.Point(2000, 432)
        Me.Label7.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(87, 23)
        Me.Label7.TabIndex = 165
        Me.Label7.Text = "Agency"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Century Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(66, Byte), Integer), CType(CType(155, Byte), Integer))
        Me.Label3.Location = New System.Drawing.Point(2000, 288)
        Me.Label3.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(158, 23)
        Me.Label3.TabIndex = 163
        Me.Label3.Text = "Job ID Number"
        '
        'txtbxIdNum
        '
        Me.txtbxIdNum.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtbxIdNum.Font = New System.Drawing.Font("Century Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtbxIdNum.Location = New System.Drawing.Point(2005, 320)
        Me.txtbxIdNum.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.txtbxIdNum.Name = "txtbxIdNum"
        Me.txtbxIdNum.Size = New System.Drawing.Size(133, 32)
        Me.txtbxIdNum.TabIndex = 162
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Century Gothic", 27.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(66, Byte), Integer), CType(CType(155, Byte), Integer))
        Me.Label2.Location = New System.Drawing.Point(1995, 225)
        Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(299, 56)
        Me.Label2.TabIndex = 161
        Me.Label2.Text = "Filter Search"
        '
        'DataGridView1
        '
        Me.DataGridView1.AllowUserToAddRows = False
        Me.DataGridView1.AllowUserToDeleteRows = False
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Location = New System.Drawing.Point(545, 225)
        Me.DataGridView1.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.ReadOnly = True
        Me.DataGridView1.RowHeadersWidth = 51
        Me.DataGridView1.Size = New System.Drawing.Size(1397, 1023)
        Me.DataGridView1.TabIndex = 160
        '
        'btnDeployment
        '
        Me.btnDeployment.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.btnDeployment.FlatAppearance.BorderSize = 0
        Me.btnDeployment.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnDeployment.Font = New System.Drawing.Font("Century Gothic", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDeployment.ForeColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(66, Byte), Integer), CType(CType(155, Byte), Integer))
        Me.btnDeployment.Location = New System.Drawing.Point(17, 507)
        Me.btnDeployment.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnDeployment.Name = "btnDeployment"
        Me.btnDeployment.Size = New System.Drawing.Size(549, 123)
        Me.btnDeployment.TabIndex = 10
        Me.btnDeployment.Text = "DEPLOYMENT"
        Me.btnDeployment.UseVisualStyleBackColor = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Century Gothic", 72.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(66, Byte), Integer), CType(CType(155, Byte), Integer))
        Me.Label1.Location = New System.Drawing.Point(719, 48)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(788, 141)
        Me.Label1.TabIndex = 159
        Me.Label1.Text = "Applications"
        '
        'btnJobOffers
        '
        Me.btnJobOffers.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.btnJobOffers.FlatAppearance.BorderSize = 0
        Me.btnJobOffers.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnJobOffers.Font = New System.Drawing.Font("Century Gothic", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnJobOffers.ForeColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(66, Byte), Integer), CType(CType(155, Byte), Integer))
        Me.btnJobOffers.Location = New System.Drawing.Point(17, 377)
        Me.btnJobOffers.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnJobOffers.Name = "btnJobOffers"
        Me.btnJobOffers.Size = New System.Drawing.Size(509, 123)
        Me.btnJobOffers.TabIndex = 3
        Me.btnJobOffers.Text = "JOB OFFERS"
        Me.btnJobOffers.UseVisualStyleBackColor = False
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.Panel1.Controls.Add(Me.PictureBox6)
        Me.Panel1.Controls.Add(Me.btnDeployment)
        Me.Panel1.Controls.Add(Me.PictureBox4)
        Me.Panel1.Controls.Add(Me.PictureBox2)
        Me.Panel1.Controls.Add(Me.PictureBox1)
        Me.Panel1.Controls.Add(Me.btnApplications)
        Me.Panel1.Controls.Add(Me.btnJobOffers)
        Me.Panel1.Controls.Add(Me.btnProfile)
        Me.Panel1.Controls.Add(Me.logo)
        Me.Panel1.Location = New System.Drawing.Point(-1, 0)
        Me.Panel1.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(509, 1346)
        Me.Panel1.TabIndex = 157
        '
        'PictureBox6
        '
        Me.PictureBox6.BackColor = System.Drawing.Color.Transparent
        Me.PictureBox6.Image = Global.OFW_Management_Information_System.My.Resources.Resources.deplo_ic
        Me.PictureBox6.Location = New System.Drawing.Point(43, 542)
        Me.PictureBox6.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.PictureBox6.Name = "PictureBox6"
        Me.PictureBox6.Size = New System.Drawing.Size(68, 59)
        Me.PictureBox6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox6.TabIndex = 11
        Me.PictureBox6.TabStop = False
        '
        'PictureBox4
        '
        Me.PictureBox4.Image = Global.OFW_Management_Information_System.My.Resources.Resources.ofwProfile_ic
        Me.PictureBox4.Location = New System.Drawing.Point(43, 279)
        Me.PictureBox4.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.PictureBox4.Name = "PictureBox4"
        Me.PictureBox4.Size = New System.Drawing.Size(68, 59)
        Me.PictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox4.TabIndex = 9
        Me.PictureBox4.TabStop = False
        '
        'PictureBox2
        '
        Me.PictureBox2.BackColor = System.Drawing.Color.Transparent
        Me.PictureBox2.Image = Global.OFW_Management_Information_System.My.Resources.Resources.job_ic
        Me.PictureBox2.Location = New System.Drawing.Point(43, 672)
        Me.PictureBox2.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(68, 59)
        Me.PictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox2.TabIndex = 7
        Me.PictureBox2.TabStop = False
        '
        'PictureBox1
        '
        Me.PictureBox1.BackColor = System.Drawing.Color.Transparent
        Me.PictureBox1.Image = Global.OFW_Management_Information_System.My.Resources.Resources.addjob_ic
        Me.PictureBox1.Location = New System.Drawing.Point(43, 411)
        Me.PictureBox1.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(68, 59)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 6
        Me.PictureBox1.TabStop = False
        '
        'btnProfile
        '
        Me.btnProfile.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.btnProfile.FlatAppearance.BorderSize = 0
        Me.btnProfile.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnProfile.Font = New System.Drawing.Font("Century Gothic", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnProfile.ForeColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(66, Byte), Integer), CType(CType(155, Byte), Integer))
        Me.btnProfile.Location = New System.Drawing.Point(17, 245)
        Me.btnProfile.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnProfile.Name = "btnProfile"
        Me.btnProfile.Size = New System.Drawing.Size(509, 123)
        Me.btnProfile.TabIndex = 2
        Me.btnProfile.Text = "PROFILE"
        Me.btnProfile.UseVisualStyleBackColor = False
        '
        'logo
        '
        Me.logo.Image = Global.OFW_Management_Information_System.My.Resources.Resources.logoM
        Me.logo.Location = New System.Drawing.Point(43, 34)
        Me.logo.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.logo.Name = "logo"
        Me.logo.Size = New System.Drawing.Size(409, 151)
        Me.logo.TabIndex = 1
        Me.logo.TabStop = False
        '
        'PictureBox5
        '
        Me.PictureBox5.BackColor = System.Drawing.Color.Transparent
        Me.PictureBox5.Image = Global.OFW_Management_Information_System.My.Resources.Resources.job_ic
        Me.PictureBox5.Location = New System.Drawing.Point(544, 34)
        Me.PictureBox5.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.PictureBox5.Name = "PictureBox5"
        Me.PictureBox5.Size = New System.Drawing.Size(167, 151)
        Me.PictureBox5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox5.TabIndex = 158
        Me.PictureBox5.TabStop = False
        '
        'btnClear
        '
        Me.btnClear.BackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(66, Byte), Integer), CType(CType(155, Byte), Integer))
        Me.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnClear.Font = New System.Drawing.Font("Century Gothic", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClear.ForeColor = System.Drawing.SystemColors.ControlLightLight
        Me.btnClear.Location = New System.Drawing.Point(2005, 529)
        Me.btnClear.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnClear.Name = "btnClear"
        Me.btnClear.Size = New System.Drawing.Size(457, 50)
        Me.btnClear.TabIndex = 172
        Me.btnClear.Text = "C L E A R"
        Me.btnClear.UseVisualStyleBackColor = False
        '
        'Button2
        '
        Me.Button2.BackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(66, Byte), Integer), CType(CType(155, Byte), Integer))
        Me.Button2.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.Button2.Font = New System.Drawing.Font("Century Gothic", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button2.ForeColor = System.Drawing.SystemColors.ControlLightLight
        Me.Button2.Location = New System.Drawing.Point(2005, 1160)
        Me.Button2.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(457, 50)
        Me.Button2.TabIndex = 183
        Me.Button2.Text = "VIEW APPLICATION"
        Me.Button2.UseVisualStyleBackColor = False
        '
        'applications
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.ControlLightLight
        Me.ClientSize = New System.Drawing.Size(2404, 1281)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.txtbxJobTitle)
        Me.Controls.Add(Me.cbxContractStat)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.dateContractStart)
        Me.Controls.Add(Me.Label17)
        Me.Controls.Add(Me.btnClear)
        Me.Controls.Add(Me.txtbxContractNum)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.txtbxIdNum)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.DataGridView1)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.PictureBox5)
        Me.Controls.Add(Me.Panel1)
        Me.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.Name = "applications"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "OFW | Applications"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        CType(Me.PictureBox6, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.logo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox5, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label11 As Label
    Friend WithEvents txtbxJobTitle As TextBox
    Friend WithEvents cbxContractStat As ComboBox
    Friend WithEvents Label4 As Label
    Friend WithEvents dateContractStart As DateTimePicker
    Friend WithEvents Label17 As Label
    Friend WithEvents txtbxContractNum As TextBox
    Friend WithEvents PictureBox6 As PictureBox
    Friend WithEvents btnApplications As Button
    Friend WithEvents Label7 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents txtbxIdNum As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents DataGridView1 As DataGridView
    Friend WithEvents btnDeployment As Button
    Friend WithEvents PictureBox4 As PictureBox
    Friend WithEvents PictureBox2 As PictureBox
    Friend WithEvents Label1 As Label
    Friend WithEvents PictureBox5 As PictureBox
    Friend WithEvents btnJobOffers As Button
    Friend WithEvents logo As PictureBox
    Friend WithEvents Panel1 As Panel
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents btnProfile As Button
    Friend WithEvents btnClear As Button
    Friend WithEvents Button2 As Button
End Class
