<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class empDashboard
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
        Dim ChartArea1 As System.Windows.Forms.DataVisualization.Charting.ChartArea = New System.Windows.Forms.DataVisualization.Charting.ChartArea()
        Dim Legend1 As System.Windows.Forms.DataVisualization.Charting.Legend = New System.Windows.Forms.DataVisualization.Charting.Legend()
        Dim Series1 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
        Me.Deployed = New System.Windows.Forms.DataGridView()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.btnAgencies = New System.Windows.Forms.Button()
        Me.btnProfile = New System.Windows.Forms.Button()
        Me.btnOfws = New System.Windows.Forms.Button()
        Me.btnJobs = New System.Windows.Forms.Button()
        Me.btnDashboard = New System.Windows.Forms.Button()
        Me.refreshBtn = New System.Windows.Forms.Button()
        Me.ThreeMonthBTN = New System.Windows.Forms.Button()
        Me.OneMonthBTN = New System.Windows.Forms.Button()
        Me.SvnDaysBTN = New System.Windows.Forms.Button()
        Me.TodayBTN = New System.Windows.Forms.Button()
        Me.CustomDate = New System.Windows.Forms.DateTimePicker()
        Me.lblNumOfw = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Panel6 = New System.Windows.Forms.Panel()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.lblNumEmployers = New System.Windows.Forms.Label()
        Me.Panel5 = New System.Windows.Forms.Panel()
        Me.ChartTopAgencies = New System.Windows.Forms.DataVisualization.Charting.Chart()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.lblNumJobPosted = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.PictureBox6 = New System.Windows.Forms.PictureBox()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.PictureBox4 = New System.Windows.Forms.PictureBox()
        Me.PictureBox3 = New System.Windows.Forms.PictureBox()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.PictureBox10 = New System.Windows.Forms.PictureBox()
        Me.logo = New System.Windows.Forms.PictureBox()
        Me.PictureBox5 = New System.Windows.Forms.PictureBox()
        Me.PictureBox9 = New System.Windows.Forms.PictureBox()
        Me.PictureBox8 = New System.Windows.Forms.PictureBox()
        Me.PictureBox7 = New System.Windows.Forms.PictureBox()
        CType(Me.Deployed, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.Panel6.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.Panel5.SuspendLayout()
        CType(Me.ChartTopAgencies, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel3.SuspendLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox6, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox10, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.logo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox5, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox9, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox8, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox7, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Deployed
        '
        Me.Deployed.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.Deployed.Location = New System.Drawing.Point(18, 109)
        Me.Deployed.Name = "Deployed"
        Me.Deployed.Size = New System.Drawing.Size(959, 633)
        Me.Deployed.TabIndex = 12
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.Panel1.Controls.Add(Me.PictureBox1)
        Me.Panel1.Controls.Add(Me.btnAgencies)
        Me.Panel1.Controls.Add(Me.PictureBox4)
        Me.Panel1.Controls.Add(Me.PictureBox3)
        Me.Panel1.Controls.Add(Me.PictureBox2)
        Me.Panel1.Controls.Add(Me.PictureBox10)
        Me.Panel1.Controls.Add(Me.btnProfile)
        Me.Panel1.Controls.Add(Me.btnOfws)
        Me.Panel1.Controls.Add(Me.btnJobs)
        Me.Panel1.Controls.Add(Me.btnDashboard)
        Me.Panel1.Controls.Add(Me.logo)
        Me.Panel1.Location = New System.Drawing.Point(-1, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(382, 1094)
        Me.Panel1.TabIndex = 60
        '
        'btnAgencies
        '
        Me.btnAgencies.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.btnAgencies.FlatAppearance.BorderSize = 0
        Me.btnAgencies.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnAgencies.Font = New System.Drawing.Font("Century Gothic", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAgencies.ForeColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(66, Byte), Integer), CType(CType(155, Byte), Integer))
        Me.btnAgencies.Location = New System.Drawing.Point(13, 412)
        Me.btnAgencies.Name = "btnAgencies"
        Me.btnAgencies.Size = New System.Drawing.Size(412, 100)
        Me.btnAgencies.TabIndex = 10
        Me.btnAgencies.Text = "AGENCIES"
        Me.btnAgencies.UseVisualStyleBackColor = False
        '
        'btnProfile
        '
        Me.btnProfile.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.btnProfile.FlatAppearance.BorderSize = 0
        Me.btnProfile.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnProfile.Font = New System.Drawing.Font("Century Gothic", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnProfile.ForeColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(66, Byte), Integer), CType(CType(155, Byte), Integer))
        Me.btnProfile.Location = New System.Drawing.Point(13, 625)
        Me.btnProfile.Name = "btnProfile"
        Me.btnProfile.Size = New System.Drawing.Size(382, 100)
        Me.btnProfile.TabIndex = 5
        Me.btnProfile.Text = "PROFILE"
        Me.btnProfile.UseVisualStyleBackColor = False
        '
        'btnOfws
        '
        Me.btnOfws.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.btnOfws.FlatAppearance.BorderSize = 0
        Me.btnOfws.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnOfws.Font = New System.Drawing.Font("Century Gothic", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnOfws.ForeColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(66, Byte), Integer), CType(CType(155, Byte), Integer))
        Me.btnOfws.Location = New System.Drawing.Point(13, 518)
        Me.btnOfws.Name = "btnOfws"
        Me.btnOfws.Size = New System.Drawing.Size(382, 100)
        Me.btnOfws.TabIndex = 4
        Me.btnOfws.Text = "HIRED OFWs"
        Me.btnOfws.UseVisualStyleBackColor = False
        '
        'btnJobs
        '
        Me.btnJobs.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.btnJobs.FlatAppearance.BorderSize = 0
        Me.btnJobs.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnJobs.Font = New System.Drawing.Font("Century Gothic", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnJobs.ForeColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(66, Byte), Integer), CType(CType(155, Byte), Integer))
        Me.btnJobs.Location = New System.Drawing.Point(13, 306)
        Me.btnJobs.Name = "btnJobs"
        Me.btnJobs.Size = New System.Drawing.Size(382, 100)
        Me.btnJobs.TabIndex = 3
        Me.btnJobs.Text = "JOBS"
        Me.btnJobs.UseVisualStyleBackColor = False
        '
        'btnDashboard
        '
        Me.btnDashboard.BackColor = System.Drawing.SystemColors.ControlLightLight
        Me.btnDashboard.Enabled = False
        Me.btnDashboard.FlatAppearance.BorderSize = 0
        Me.btnDashboard.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnDashboard.Font = New System.Drawing.Font("Century Gothic", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDashboard.ForeColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(66, Byte), Integer), CType(CType(155, Byte), Integer))
        Me.btnDashboard.Location = New System.Drawing.Point(13, 199)
        Me.btnDashboard.Name = "btnDashboard"
        Me.btnDashboard.Size = New System.Drawing.Size(382, 100)
        Me.btnDashboard.TabIndex = 2
        Me.btnDashboard.Text = "DASHBOARD"
        Me.btnDashboard.UseVisualStyleBackColor = False
        '
        'refreshBtn
        '
        Me.refreshBtn.Font = New System.Drawing.Font("Century Gothic", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.refreshBtn.Location = New System.Drawing.Point(506, 17)
        Me.refreshBtn.Name = "refreshBtn"
        Me.refreshBtn.Size = New System.Drawing.Size(149, 47)
        Me.refreshBtn.TabIndex = 65
        Me.refreshBtn.Text = "Refresh"
        Me.refreshBtn.UseVisualStyleBackColor = True
        '
        'ThreeMonthBTN
        '
        Me.ThreeMonthBTN.Font = New System.Drawing.Font("Century Gothic", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ThreeMonthBTN.Location = New System.Drawing.Point(1687, 17)
        Me.ThreeMonthBTN.Name = "ThreeMonthBTN"
        Me.ThreeMonthBTN.Size = New System.Drawing.Size(149, 47)
        Me.ThreeMonthBTN.TabIndex = 64
        Me.ThreeMonthBTN.Text = "1 Year"
        Me.ThreeMonthBTN.UseVisualStyleBackColor = True
        '
        'OneMonthBTN
        '
        Me.OneMonthBTN.Font = New System.Drawing.Font("Century Gothic", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OneMonthBTN.Location = New System.Drawing.Point(1533, 17)
        Me.OneMonthBTN.Name = "OneMonthBTN"
        Me.OneMonthBTN.Size = New System.Drawing.Size(149, 47)
        Me.OneMonthBTN.TabIndex = 63
        Me.OneMonthBTN.Text = "1 Month"
        Me.OneMonthBTN.UseVisualStyleBackColor = True
        '
        'SvnDaysBTN
        '
        Me.SvnDaysBTN.Font = New System.Drawing.Font("Century Gothic", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SvnDaysBTN.Location = New System.Drawing.Point(1378, 17)
        Me.SvnDaysBTN.Name = "SvnDaysBTN"
        Me.SvnDaysBTN.Size = New System.Drawing.Size(149, 47)
        Me.SvnDaysBTN.TabIndex = 62
        Me.SvnDaysBTN.Text = "7 Days"
        Me.SvnDaysBTN.UseVisualStyleBackColor = True
        '
        'TodayBTN
        '
        Me.TodayBTN.Font = New System.Drawing.Font("Century Gothic", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TodayBTN.Location = New System.Drawing.Point(1223, 17)
        Me.TodayBTN.Name = "TodayBTN"
        Me.TodayBTN.Size = New System.Drawing.Size(149, 47)
        Me.TodayBTN.TabIndex = 61
        Me.TodayBTN.Text = "Today"
        Me.TodayBTN.UseVisualStyleBackColor = True
        '
        'CustomDate
        '
        Me.CustomDate.Font = New System.Drawing.Font("Century Gothic", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CustomDate.Location = New System.Drawing.Point(901, 26)
        Me.CustomDate.Name = "CustomDate"
        Me.CustomDate.Size = New System.Drawing.Size(310, 26)
        Me.CustomDate.TabIndex = 66
        '
        'lblNumOfw
        '
        Me.lblNumOfw.AutoSize = True
        Me.lblNumOfw.Font = New System.Drawing.Font("Century Gothic", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNumOfw.ForeColor = System.Drawing.SystemColors.MenuText
        Me.lblNumOfw.Location = New System.Drawing.Point(113, 88)
        Me.lblNumOfw.Name = "lblNumOfw"
        Me.lblNumOfw.Size = New System.Drawing.Size(140, 38)
        Me.lblNumOfw.TabIndex = 11
        Me.lblNumOfw.Text = "###,###"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Century Gothic", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label1.Location = New System.Drawing.Point(102, 18)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(151, 48)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Total number " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "of OFWs :"
        '
        'Panel2
        '
        Me.Panel2.BackColor = System.Drawing.SystemColors.GradientActiveCaption
        Me.Panel2.Controls.Add(Me.Label7)
        Me.Panel2.Controls.Add(Me.lblNumOfw)
        Me.Panel2.Controls.Add(Me.PictureBox5)
        Me.Panel2.Controls.Add(Me.Label1)
        Me.Panel2.Font = New System.Drawing.Font("Century Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Panel2.Location = New System.Drawing.Point(504, 73)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(313, 147)
        Me.Panel2.TabIndex = 55
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Century Gothic", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label7.Location = New System.Drawing.Point(121, 224)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(0, 38)
        Me.Label7.TabIndex = 12
        '
        'Panel6
        '
        Me.Panel6.BackColor = System.Drawing.SystemColors.GradientActiveCaption
        Me.Panel6.Controls.Add(Me.Deployed)
        Me.Panel6.Controls.Add(Me.PictureBox9)
        Me.Panel6.Controls.Add(Me.Label13)
        Me.Panel6.Font = New System.Drawing.Font("Century Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Panel6.Location = New System.Drawing.Point(498, 247)
        Me.Panel6.Name = "Panel6"
        Me.Panel6.Size = New System.Drawing.Size(992, 760)
        Me.Panel6.TabIndex = 57
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Century Gothic", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label13.Location = New System.Drawing.Point(102, 18)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(139, 48)
        Me.Label13.TabIndex = 0
        Me.Label13.Text = "Deployment" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Status :"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Century Gothic", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label6.Location = New System.Drawing.Point(102, 18)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(152, 48)
        Me.Label6.TabIndex = 0
        Me.Label6.Text = "Total number " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "of Employers :"
        '
        'Panel4
        '
        Me.Panel4.BackColor = System.Drawing.SystemColors.GradientActiveCaption
        Me.Panel4.Controls.Add(Me.PictureBox8)
        Me.Panel4.Controls.Add(Me.lblNumEmployers)
        Me.Panel4.Controls.Add(Me.Label6)
        Me.Panel4.Font = New System.Drawing.Font("Century Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Panel4.Location = New System.Drawing.Point(838, 73)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(313, 147)
        Me.Panel4.TabIndex = 58
        '
        'lblNumEmployers
        '
        Me.lblNumEmployers.AutoSize = True
        Me.lblNumEmployers.Font = New System.Drawing.Font("Century Gothic", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNumEmployers.ForeColor = System.Drawing.SystemColors.MenuText
        Me.lblNumEmployers.Location = New System.Drawing.Point(113, 80)
        Me.lblNumEmployers.Name = "lblNumEmployers"
        Me.lblNumEmployers.Size = New System.Drawing.Size(140, 38)
        Me.lblNumEmployers.TabIndex = 11
        Me.lblNumEmployers.Text = "###,###"
        '
        'Panel5
        '
        Me.Panel5.BackColor = System.Drawing.SystemColors.GradientActiveCaption
        Me.Panel5.Controls.Add(Me.DataGridView1)
        Me.Panel5.Controls.Add(Me.ChartTopAgencies)
        Me.Panel5.Controls.Add(Me.PictureBox7)
        Me.Panel5.Controls.Add(Me.Label12)
        Me.Panel5.Font = New System.Drawing.Font("Century Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Panel5.Location = New System.Drawing.Point(1514, 73)
        Me.Panel5.Name = "Panel5"
        Me.Panel5.Size = New System.Drawing.Size(322, 934)
        Me.Panel5.TabIndex = 59
        '
        'ChartTopAgencies
        '
        Me.ChartTopAgencies.BackColor = System.Drawing.SystemColors.GradientActiveCaption
        ChartArea1.BackSecondaryColor = System.Drawing.SystemColors.GradientActiveCaption
        ChartArea1.Name = "ChartArea1"
        Me.ChartTopAgencies.ChartAreas.Add(ChartArea1)
        Legend1.Name = "Legend1"
        Me.ChartTopAgencies.Legends.Add(Legend1)
        Me.ChartTopAgencies.Location = New System.Drawing.Point(19, 109)
        Me.ChartTopAgencies.Name = "ChartTopAgencies"
        Series1.ChartArea = "ChartArea1"
        Series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie
        Series1.Legend = "Legend1"
        Series1.Name = "Series1"
        Me.ChartTopAgencies.Series.Add(Series1)
        Me.ChartTopAgencies.Size = New System.Drawing.Size(291, 224)
        Me.ChartTopAgencies.TabIndex = 16
        Me.ChartTopAgencies.Text = "Chart4"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Century Gothic", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label12.Location = New System.Drawing.Point(105, 18)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(174, 48)
        Me.Label12.TabIndex = 0
        Me.Label12.Text = "Applications per" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Job Post :"
        '
        'Panel3
        '
        Me.Panel3.BackColor = System.Drawing.SystemColors.GradientActiveCaption
        Me.Panel3.Controls.Add(Me.PictureBox6)
        Me.Panel3.Controls.Add(Me.lblNumJobPosted)
        Me.Panel3.Controls.Add(Me.Label3)
        Me.Panel3.Font = New System.Drawing.Font("Century Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Panel3.Location = New System.Drawing.Point(1177, 73)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(313, 147)
        Me.Panel3.TabIndex = 59
        '
        'lblNumJobPosted
        '
        Me.lblNumJobPosted.AutoSize = True
        Me.lblNumJobPosted.Font = New System.Drawing.Font("Century Gothic", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNumJobPosted.ForeColor = System.Drawing.SystemColors.MenuText
        Me.lblNumJobPosted.Location = New System.Drawing.Point(113, 80)
        Me.lblNumJobPosted.Name = "lblNumJobPosted"
        Me.lblNumJobPosted.Size = New System.Drawing.Size(140, 38)
        Me.lblNumJobPosted.TabIndex = 11
        Me.lblNumJobPosted.Text = "###,###"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Century Gothic", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label3.Location = New System.Drawing.Point(102, 18)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(169, 48)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "Total number " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "of Jobs Posted :"
        '
        'DataGridView1
        '
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Location = New System.Drawing.Point(19, 349)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.Size = New System.Drawing.Size(282, 567)
        Me.DataGridView1.TabIndex = 17
        '
        'PictureBox6
        '
        Me.PictureBox6.BackColor = System.Drawing.Color.Transparent
        Me.PictureBox6.Image = Global.OFW_Management_Information_System.My.Resources.Resources.job_ic
        Me.PictureBox6.Location = New System.Drawing.Point(16, 18)
        Me.PictureBox6.Name = "PictureBox6"
        Me.PictureBox6.Size = New System.Drawing.Size(80, 76)
        Me.PictureBox6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox6.TabIndex = 10
        Me.PictureBox6.TabStop = False
        '
        'PictureBox1
        '
        Me.PictureBox1.BackColor = System.Drawing.Color.Transparent
        Me.PictureBox1.Image = Global.OFW_Management_Information_System.My.Resources.Resources.employer_ic
        Me.PictureBox1.Location = New System.Drawing.Point(32, 440)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(51, 48)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 11
        Me.PictureBox1.TabStop = False
        '
        'PictureBox4
        '
        Me.PictureBox4.Image = Global.OFW_Management_Information_System.My.Resources.Resources.admin_ic
        Me.PictureBox4.Location = New System.Drawing.Point(32, 227)
        Me.PictureBox4.Name = "PictureBox4"
        Me.PictureBox4.Size = New System.Drawing.Size(51, 48)
        Me.PictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox4.TabIndex = 9
        Me.PictureBox4.TabStop = False
        '
        'PictureBox3
        '
        Me.PictureBox3.BackColor = System.Drawing.Color.Transparent
        Me.PictureBox3.Image = Global.OFW_Management_Information_System.My.Resources.Resources.profile_ic
        Me.PictureBox3.Location = New System.Drawing.Point(32, 653)
        Me.PictureBox3.Name = "PictureBox3"
        Me.PictureBox3.Size = New System.Drawing.Size(51, 48)
        Me.PictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox3.TabIndex = 8
        Me.PictureBox3.TabStop = False
        '
        'PictureBox2
        '
        Me.PictureBox2.BackColor = System.Drawing.Color.Transparent
        Me.PictureBox2.Image = Global.OFW_Management_Information_System.My.Resources.Resources.joblist_ic
        Me.PictureBox2.Location = New System.Drawing.Point(32, 546)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(51, 48)
        Me.PictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox2.TabIndex = 7
        Me.PictureBox2.TabStop = False
        '
        'PictureBox10
        '
        Me.PictureBox10.BackColor = System.Drawing.Color.Transparent
        Me.PictureBox10.Image = Global.OFW_Management_Information_System.My.Resources.Resources.job_ic
        Me.PictureBox10.Location = New System.Drawing.Point(32, 334)
        Me.PictureBox10.Name = "PictureBox10"
        Me.PictureBox10.Size = New System.Drawing.Size(51, 48)
        Me.PictureBox10.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox10.TabIndex = 6
        Me.PictureBox10.TabStop = False
        '
        'logo
        '
        Me.logo.Image = Global.OFW_Management_Information_System.My.Resources.Resources.logoM
        Me.logo.Location = New System.Drawing.Point(32, 28)
        Me.logo.Name = "logo"
        Me.logo.Size = New System.Drawing.Size(307, 123)
        Me.logo.TabIndex = 1
        Me.logo.TabStop = False
        '
        'PictureBox5
        '
        Me.PictureBox5.BackColor = System.Drawing.Color.Transparent
        Me.PictureBox5.Image = Global.OFW_Management_Information_System.My.Resources.Resources.ofw_ic
        Me.PictureBox5.Location = New System.Drawing.Point(16, 18)
        Me.PictureBox5.Name = "PictureBox5"
        Me.PictureBox5.Size = New System.Drawing.Size(80, 76)
        Me.PictureBox5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox5.TabIndex = 10
        Me.PictureBox5.TabStop = False
        '
        'PictureBox9
        '
        Me.PictureBox9.BackColor = System.Drawing.Color.Transparent
        Me.PictureBox9.Image = Global.OFW_Management_Information_System.My.Resources.Resources.deployment_ic
        Me.PictureBox9.Location = New System.Drawing.Point(16, 18)
        Me.PictureBox9.Name = "PictureBox9"
        Me.PictureBox9.Size = New System.Drawing.Size(80, 76)
        Me.PictureBox9.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox9.TabIndex = 10
        Me.PictureBox9.TabStop = False
        '
        'PictureBox8
        '
        Me.PictureBox8.BackColor = System.Drawing.Color.Transparent
        Me.PictureBox8.Image = Global.OFW_Management_Information_System.My.Resources.Resources.employer_ic
        Me.PictureBox8.Location = New System.Drawing.Point(16, 18)
        Me.PictureBox8.Name = "PictureBox8"
        Me.PictureBox8.Size = New System.Drawing.Size(80, 76)
        Me.PictureBox8.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox8.TabIndex = 10
        Me.PictureBox8.TabStop = False
        '
        'PictureBox7
        '
        Me.PictureBox7.BackColor = System.Drawing.Color.Transparent
        Me.PictureBox7.Image = Global.OFW_Management_Information_System.My.Resources.Resources.joblist_ic
        Me.PictureBox7.Location = New System.Drawing.Point(19, 18)
        Me.PictureBox7.Name = "PictureBox7"
        Me.PictureBox7.Size = New System.Drawing.Size(80, 76)
        Me.PictureBox7.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox7.TabIndex = 10
        Me.PictureBox7.TabStop = False
        '
        'empDashboard
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.ControlLightLight
        Me.ClientSize = New System.Drawing.Size(1904, 1041)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.refreshBtn)
        Me.Controls.Add(Me.ThreeMonthBTN)
        Me.Controls.Add(Me.OneMonthBTN)
        Me.Controls.Add(Me.SvnDaysBTN)
        Me.Controls.Add(Me.TodayBTN)
        Me.Controls.Add(Me.CustomDate)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel6)
        Me.Controls.Add(Me.Panel4)
        Me.Controls.Add(Me.Panel5)
        Me.Name = "empDashboard"
        Me.Text = " EMPLOYER | Dashboard"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.Deployed, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.Panel6.ResumeLayout(False)
        Me.Panel6.PerformLayout()
        Me.Panel4.ResumeLayout(False)
        Me.Panel4.PerformLayout()
        Me.Panel5.ResumeLayout(False)
        Me.Panel5.PerformLayout()
        CType(Me.ChartTopAgencies, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox6, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox10, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.logo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox5, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox9, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox8, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox7, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Deployed As DataGridView
    Friend WithEvents Panel1 As Panel
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents btnAgencies As Button
    Friend WithEvents PictureBox4 As PictureBox
    Friend WithEvents PictureBox3 As PictureBox
    Friend WithEvents PictureBox2 As PictureBox
    Friend WithEvents PictureBox10 As PictureBox
    Friend WithEvents btnProfile As Button
    Friend WithEvents btnOfws As Button
    Friend WithEvents btnJobs As Button
    Friend WithEvents btnDashboard As Button
    Friend WithEvents logo As PictureBox
    Friend WithEvents refreshBtn As Button
    Friend WithEvents ThreeMonthBTN As Button
    Friend WithEvents OneMonthBTN As Button
    Friend WithEvents SvnDaysBTN As Button
    Friend WithEvents TodayBTN As Button
    Friend WithEvents CustomDate As DateTimePicker
    Friend WithEvents lblNumOfw As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents Panel2 As Panel
    Friend WithEvents Label7 As Label
    Friend WithEvents PictureBox5 As PictureBox
    Friend WithEvents Panel6 As Panel
    Friend WithEvents PictureBox9 As PictureBox
    Friend WithEvents Label13 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Panel4 As Panel
    Friend WithEvents PictureBox8 As PictureBox
    Friend WithEvents lblNumEmployers As Label
    Friend WithEvents Panel5 As Panel
    Friend WithEvents ChartTopAgencies As DataVisualization.Charting.Chart
    Friend WithEvents PictureBox7 As PictureBox
    Friend WithEvents Label12 As Label
    Friend WithEvents Panel3 As Panel
    Friend WithEvents PictureBox6 As PictureBox
    Friend WithEvents lblNumJobPosted As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents DataGridView1 As DataGridView
End Class
