<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class loginPage
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(loginPage))
        Me.logo = New System.Windows.Forms.PictureBox()
        Me.btnAdmin = New System.Windows.Forms.Button()
        Me.btnAgency = New System.Windows.Forms.Button()
        Me.btnOfw = New System.Windows.Forms.Button()
        Me.btnEmployer = New System.Windows.Forms.Button()
        CType(Me.logo, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'logo
        '
        Me.logo.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.logo.Image = CType(resources.GetObject("logo.Image"), System.Drawing.Image)
        Me.logo.Location = New System.Drawing.Point(702, 189)
        Me.logo.Name = "logo"
        Me.logo.Size = New System.Drawing.Size(500, 387)
        Me.logo.TabIndex = 0
        Me.logo.TabStop = False
        '
        'btnAdmin
        '
        Me.btnAdmin.BackColor = System.Drawing.SystemColors.Highlight
        Me.btnAdmin.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnAdmin.Font = New System.Drawing.Font("Microsoft YaHei", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAdmin.ForeColor = System.Drawing.Color.Transparent
        Me.btnAdmin.Location = New System.Drawing.Point(631, 619)
        Me.btnAdmin.Name = "btnAdmin"
        Me.btnAdmin.Size = New System.Drawing.Size(156, 57)
        Me.btnAdmin.TabIndex = 1
        Me.btnAdmin.Text = "Admin"
        Me.btnAdmin.UseVisualStyleBackColor = False
        '
        'btnAgency
        '
        Me.btnAgency.BackColor = System.Drawing.SystemColors.Highlight
        Me.btnAgency.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnAgency.Font = New System.Drawing.Font("Microsoft YaHei", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAgency.ForeColor = System.Drawing.Color.Transparent
        Me.btnAgency.Location = New System.Drawing.Point(793, 619)
        Me.btnAgency.Name = "btnAgency"
        Me.btnAgency.Size = New System.Drawing.Size(156, 57)
        Me.btnAgency.TabIndex = 2
        Me.btnAgency.Text = "Agency"
        Me.btnAgency.UseVisualStyleBackColor = False
        '
        'btnOfw
        '
        Me.btnOfw.BackColor = System.Drawing.SystemColors.Highlight
        Me.btnOfw.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnOfw.Font = New System.Drawing.Font("Microsoft YaHei", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnOfw.ForeColor = System.Drawing.Color.Transparent
        Me.btnOfw.Location = New System.Drawing.Point(955, 619)
        Me.btnOfw.Name = "btnOfw"
        Me.btnOfw.Size = New System.Drawing.Size(156, 57)
        Me.btnOfw.TabIndex = 3
        Me.btnOfw.Text = "OFW"
        Me.btnOfw.UseVisualStyleBackColor = False
        '
        'btnEmployer
        '
        Me.btnEmployer.BackColor = System.Drawing.SystemColors.Highlight
        Me.btnEmployer.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnEmployer.Font = New System.Drawing.Font("Microsoft YaHei", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnEmployer.ForeColor = System.Drawing.Color.Transparent
        Me.btnEmployer.Location = New System.Drawing.Point(1117, 619)
        Me.btnEmployer.Name = "btnEmployer"
        Me.btnEmployer.Size = New System.Drawing.Size(156, 57)
        Me.btnEmployer.TabIndex = 4
        Me.btnEmployer.Text = "Employer"
        Me.btnEmployer.UseVisualStyleBackColor = False
        '
        'loginPage
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1904, 1041)
        Me.Controls.Add(Me.btnEmployer)
        Me.Controls.Add(Me.btnOfw)
        Me.Controls.Add(Me.btnAgency)
        Me.Controls.Add(Me.btnAdmin)
        Me.Controls.Add(Me.logo)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Name = "loginPage"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "OFW Management Information System | LOGIN"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.logo, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents logo As PictureBox
    Friend WithEvents btnAdmin As Button
    Friend WithEvents btnAgency As Button
    Friend WithEvents btnOfw As Button
    Friend WithEvents btnEmployer As Button
End Class
