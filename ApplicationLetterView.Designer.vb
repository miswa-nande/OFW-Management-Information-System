<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ApplicationLetterView
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
        Me.lblOFWName = New System.Windows.Forms.Label()
        Me.CloseBTN = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.OFWEmail = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.lblDateApplicationSubmitted = New System.Windows.Forms.Label()
        Me.lblAgencyName = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.LetterContentWebBrowser = New System.Windows.Forms.WebBrowser()
        Me.SuspendLayout()
        '
        'lblOFWName
        '
        Me.lblOFWName.AutoSize = True
        Me.lblOFWName.Font = New System.Drawing.Font("Arial Rounded MT Bold", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOFWName.Location = New System.Drawing.Point(44, 48)
        Me.lblOFWName.Name = "lblOFWName"
        Me.lblOFWName.Size = New System.Drawing.Size(112, 22)
        Me.lblOFWName.TabIndex = 0
        Me.lblOFWName.Text = "OFW Name"
        '
        'CloseBTN
        '
        Me.CloseBTN.BackColor = System.Drawing.Color.Firebrick
        Me.CloseBTN.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.CloseBTN.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CloseBTN.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CloseBTN.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.CloseBTN.Location = New System.Drawing.Point(627, 861)
        Me.CloseBTN.Name = "CloseBTN"
        Me.CloseBTN.Size = New System.Drawing.Size(101, 43)
        Me.CloseBTN.TabIndex = 1
        Me.CloseBTN.Text = "Close"
        Me.CloseBTN.UseVisualStyleBackColor = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(46, 96)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(0, 13)
        Me.Label2.TabIndex = 2
        '
        'OFWEmail
        '
        Me.OFWEmail.AutoSize = True
        Me.OFWEmail.Font = New System.Drawing.Font("Arial Rounded MT Bold", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OFWEmail.Location = New System.Drawing.Point(46, 79)
        Me.OFWEmail.Name = "OFWEmail"
        Me.OFWEmail.Size = New System.Drawing.Size(44, 15)
        Me.OFWEmail.TabIndex = 3
        Me.OFWEmail.Text = "Email"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Arial Rounded MT Bold", 21.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(258, 169)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(268, 33)
        Me.Label4.TabIndex = 4
        Me.Label4.Text = "Application Letter"
        '
        'lblDateApplicationSubmitted
        '
        Me.lblDateApplicationSubmitted.AutoSize = True
        Me.lblDateApplicationSubmitted.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDateApplicationSubmitted.Location = New System.Drawing.Point(46, 233)
        Me.lblDateApplicationSubmitted.Name = "lblDateApplicationSubmitted"
        Me.lblDateApplicationSubmitted.Size = New System.Drawing.Size(96, 16)
        Me.lblDateApplicationSubmitted.TabIndex = 5
        Me.lblDateApplicationSubmitted.Text = "Submition Date"
        '
        'lblAgencyName
        '
        Me.lblAgencyName.AutoSize = True
        Me.lblAgencyName.Font = New System.Drawing.Font("Arial Rounded MT Bold", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAgencyName.Location = New System.Drawing.Point(46, 263)
        Me.lblAgencyName.Name = "lblAgencyName"
        Me.lblAgencyName.Size = New System.Drawing.Size(96, 15)
        Me.lblAgencyName.TabIndex = 6
        Me.lblAgencyName.Text = "Agency Name"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(46, 293)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(94, 16)
        Me.Label7.TabIndex = 7
        Me.Label7.Text = "Hiring Manager"
        '
        'LetterContentWebBrowser
        '
        Me.LetterContentWebBrowser.Location = New System.Drawing.Point(49, 336)
        Me.LetterContentWebBrowser.MinimumSize = New System.Drawing.Size(20, 20)
        Me.LetterContentWebBrowser.Name = "LetterContentWebBrowser"
        Me.LetterContentWebBrowser.Size = New System.Drawing.Size(679, 519)
        Me.LetterContentWebBrowser.TabIndex = 11
        '
        'ApplicationLetterView
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(775, 951)
        Me.Controls.Add(Me.LetterContentWebBrowser)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.lblAgencyName)
        Me.Controls.Add(Me.lblDateApplicationSubmitted)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.OFWEmail)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.CloseBTN)
        Me.Controls.Add(Me.lblOFWName)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "ApplicationLetterView"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "ApplicationLetterView"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblOFWName As Label
    Friend WithEvents CloseBTN As Button
    Friend WithEvents Label2 As Label
    Friend WithEvents OFWEmail As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents lblDateApplicationSubmitted As Label
    Friend WithEvents lblAgencyName As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents LetterContentWebBrowser As WebBrowser
End Class
