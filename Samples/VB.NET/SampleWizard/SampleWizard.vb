' *****************************************************************************
' 
'  (c) Crownwood Consulting Limited 2002-2003
'  All rights reserved. The software and associated documentation 
'  supplied hereunder are the proprietary information of Crownwood Consulting 
'	Limited, Crownwood, Bracknell, Berkshire, England and are supplied subject 
'  to licence terms.
' 
'  Magic Version 1.7.4.0 	www.dotnetmagic.com
' *****************************************************************************

Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports Crownwood.Magic.Controls

Namespace SampleWizard
	Public Class SampleWizard
		Inherits Crownwood.Magic.Forms.WizardDialog

		Private installTimer As Timer
		Private installCount As Int32
		Private wizardStartPage As Crownwood.Magic.Controls.WizardPage
		Private label1 As System.Windows.Forms.Label
		Private label2 As System.Windows.Forms.Label
		Private label3 As System.Windows.Forms.Label
		Private wizardInfo1 As Crownwood.Magic.Controls.WizardPage
		Private groupBox1 As System.Windows.Forms.GroupBox
		Private radioButton1 As System.Windows.Forms.RadioButton
		Private radioButton2 As System.Windows.Forms.RadioButton
		Private radioButton3 As System.Windows.Forms.RadioButton
		Private wizardLegal As Crownwood.Magic.Controls.WizardPage
		Private textBox1 As System.Windows.Forms.TextBox
		Private label4 As System.Windows.Forms.Label
		Private radioButton4 As System.Windows.Forms.RadioButton
		Private radioButton5 As System.Windows.Forms.RadioButton
		Private wizardInstall As Crownwood.Magic.Controls.WizardPage
		Private label5 As System.Windows.Forms.Label
		Private label6 As System.Windows.Forms.Label
		Private progressBar1 As System.Windows.Forms.ProgressBar
		Private label7 As System.Windows.Forms.Label
		Private wizardFinish As Crownwood.Magic.Controls.WizardPage
		Private wizardWarn As Crownwood.Magic.Controls.WizardPage
		Private wizardInfo2 As Crownwood.Magic.Controls.WizardPage
		Private textBox2 As System.Windows.Forms.TextBox
		Private label8 As System.Windows.Forms.Label
		Private textBox3 As System.Windows.Forms.TextBox
		Private label9 As System.Windows.Forms.Label
		Private label10 As System.Windows.Forms.Label
		Private components As System.ComponentModel.IContainer = Nothing

		Sub New()
			' Me call is required by the Windows Form Designer.
			InitializeComponent()

			AddHandler radioButton5.CheckedChanged, AddressOf radioButton5_CheckedChanged
			AddHandler radioButton4.CheckedChanged, AddressOf radioButton4_CheckedChanged
		End Sub

		Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
			If (disposing) Then
				If Not (components Is Nothing) Then
					components.Dispose()
				End If
			End If
			MyBase.Dispose(disposing)
		End Sub

#Region "Designer generated code"
		Private Sub InitializeComponent()
			Me.wizardStartPage = New Crownwood.Magic.Controls.WizardPage()
			Me.label3 = New System.Windows.Forms.Label()
			Me.label2 = New System.Windows.Forms.Label()
			Me.label1 = New System.Windows.Forms.Label()
			Me.wizardInfo1 = New Crownwood.Magic.Controls.WizardPage()
			Me.groupBox1 = New System.Windows.Forms.GroupBox()
			Me.radioButton3 = New System.Windows.Forms.RadioButton()
			Me.radioButton2 = New System.Windows.Forms.RadioButton()
			Me.radioButton1 = New System.Windows.Forms.RadioButton()
			Me.wizardLegal = New Crownwood.Magic.Controls.WizardPage()
			Me.radioButton5 = New System.Windows.Forms.RadioButton()
			Me.radioButton4 = New System.Windows.Forms.RadioButton()
			Me.label4 = New System.Windows.Forms.Label()
			Me.textBox1 = New System.Windows.Forms.TextBox()
			Me.wizardWarn = New Crownwood.Magic.Controls.WizardPage()
			Me.label6 = New System.Windows.Forms.Label()
			Me.label5 = New System.Windows.Forms.Label()
			Me.wizardInstall = New Crownwood.Magic.Controls.WizardPage()
			Me.label7 = New System.Windows.Forms.Label()
			Me.progressBar1 = New System.Windows.Forms.ProgressBar()
			Me.wizardFinish = New Crownwood.Magic.Controls.WizardPage()
			Me.label10 = New System.Windows.Forms.Label()
			Me.wizardInfo2 = New Crownwood.Magic.Controls.WizardPage()
			Me.label9 = New System.Windows.Forms.Label()
			Me.textBox3 = New System.Windows.Forms.TextBox()
			Me.label8 = New System.Windows.Forms.Label()
			Me.textBox2 = New System.Windows.Forms.TextBox()
			Me.wizardStartPage.SuspendLayout()
			Me.wizardInfo1.SuspendLayout()
			Me.groupBox1.SuspendLayout()
			Me.wizardLegal.SuspendLayout()
			Me.wizardWarn.SuspendLayout()
			Me.wizardInstall.SuspendLayout()
			Me.wizardFinish.SuspendLayout()
			Me.wizardInfo2.SuspendLayout()
			Me.SuspendLayout()
			'
			'wizardControl
			'
			Me.wizardControl.Profile = Crownwood.Magic.Controls.WizardControl.Profiles.Install
			Me.wizardControl.SelectedIndex = 0
			Me.wizardControl.Size = New System.Drawing.Size(410, 351)
			Me.wizardControl.Title = "Sample Wizard using Install Profile"
			Me.wizardControl.Visible = True
			Me.wizardControl.WizardPages.AddRange(New Crownwood.Magic.Controls.WizardPage() {Me.wizardStartPage, Me.wizardLegal, Me.wizardInfo1, Me.wizardInfo2, Me.wizardWarn, Me.wizardInstall, Me.wizardFinish})
			'
			'wizardStartPage
			'
			Me.wizardStartPage.Controls.AddRange(New System.Windows.Forms.Control() {Me.label3, Me.label2, Me.label1})
			Me.wizardStartPage.FullPage = False
			Me.wizardStartPage.Name = "wizardStartPage"
			Me.wizardStartPage.Size = New System.Drawing.Size(410, 222)
			Me.wizardStartPage.SubTitle = "Start page explaining what Me sample demonstrates"
			Me.wizardStartPage.TabIndex = 3
			Me.wizardStartPage.Title = "Start"
			'
			'label3
			'
			Me.label3.Location = New System.Drawing.Point(8, 136)
			Me.label3.Name = "label3"
			Me.label3.Size = New System.Drawing.Size(400, 64)
			Me.label3.TabIndex = 3
			Me.label3.Text = "Therefore on the second to last page only the 'Cancel' button is available and us" & _
			"ed to abort the installation. On the last page only the 'Close' button is availa" & _
			"ble as the only possible action left is to exit the dialog.  On preceding pages " & _
			"the 'Next' and 'Back' buttons are displayed when appropriate as well as the 'Can" & _
			"cel' button."
			'
			'label2
			'
			Me.label2.Location = New System.Drawing.Point(8, 72)
			Me.label2.Name = "label2"
			Me.label2.Size = New System.Drawing.Size(400, 64)
			Me.label2.TabIndex = 2
			Me.label2.Text = "In Me profile the last two pages have special significance. The second to last is" & _
			" used for conducting the actual installation. The last page is used to report th" & _
			"e success or failure of the install. All other preceding pages are for informati" & _
			"on gathering prior to the install attempt."
			'
			'label1
			'
			Me.label1.Location = New System.Drawing.Point(8, 8)
			Me.label1.Name = "label1"
			Me.label1.Size = New System.Drawing.Size(400, 56)
			Me.label1.TabIndex = 1
			Me.label1.Text = "The WizardControl and associated WizardDialog can be used in three different prof" & _
			"iles depending on the type of Wizard required. Available options are 'Install', " & _
			"'Configure' and 'Controller'. Me sample is intended to demonstrate use of the 'I" & _
			"nstall' profile."
			'
			'wizardInfo1
			'
			Me.wizardInfo1.Controls.AddRange(New System.Windows.Forms.Control() {Me.groupBox1})
			Me.wizardInfo1.FullPage = False
			Me.wizardInfo1.Name = "wizardInfo1"
			Me.wizardInfo1.Selected = False
			Me.wizardInfo1.Size = New System.Drawing.Size(410, 222)
			Me.wizardInfo1.SubTitle = "Me is the first of two pages for gathering input"
			Me.wizardInfo1.TabIndex = 4
			Me.wizardInfo1.Title = "Info1"
			'
			'groupBox1
			'
			Me.groupBox1.Controls.AddRange(New System.Windows.Forms.Control() {Me.radioButton3, Me.radioButton2, Me.radioButton1})
			Me.groupBox1.Location = New System.Drawing.Point(24, 16)
			Me.groupBox1.Name = "groupBox1"
			Me.groupBox1.Size = New System.Drawing.Size(160, 152)
			Me.groupBox1.TabIndex = 0
			Me.groupBox1.TabStop = False
			Me.groupBox1.Text = "Example of Selection"
			'
			'radioButton3
			'
			Me.radioButton3.Location = New System.Drawing.Point(32, 112)
			Me.radioButton3.Name = "radioButton3"
			Me.radioButton3.TabIndex = 2
			Me.radioButton3.Text = "Debug Install"
			'
			'radioButton2
			'
			Me.radioButton2.Location = New System.Drawing.Point(32, 72)
			Me.radioButton2.Name = "radioButton2"
			Me.radioButton2.TabIndex = 1
			Me.radioButton2.Text = "Server Install"
			'
			'radioButton1
			'
			Me.radioButton1.Location = New System.Drawing.Point(32, 32)
			Me.radioButton1.Name = "radioButton1"
			Me.radioButton1.TabIndex = 0
			Me.radioButton1.Text = "Client Install"
			'
			'wizardLegal
			'
			Me.wizardLegal.Controls.AddRange(New System.Windows.Forms.Control() {Me.radioButton5, Me.radioButton4, Me.label4, Me.textBox1})
			Me.wizardLegal.FullPage = False
			Me.wizardLegal.Name = "wizardLegal"
			Me.wizardLegal.Selected = False
			Me.wizardLegal.Size = New System.Drawing.Size(410, 222)
			Me.wizardLegal.SubTitle = "Force the user to agree a license agreement for product"
			Me.wizardLegal.TabIndex = 5
			Me.wizardLegal.Title = "Legal"
			'
			'radioButton5
			'
			Me.radioButton5.Checked = True
			Me.radioButton5.Location = New System.Drawing.Point(216, 160)
			Me.radioButton5.Name = "radioButton5"
			Me.radioButton5.Size = New System.Drawing.Size(88, 24)
			Me.radioButton5.TabIndex = 3
			Me.radioButton5.TabStop = True
			Me.radioButton5.Text = "I Disagree"
			'
			'radioButton4
			'
			Me.radioButton4.Location = New System.Drawing.Point(120, 160)
			Me.radioButton4.Name = "radioButton4"
			Me.radioButton4.Size = New System.Drawing.Size(72, 24)
			Me.radioButton4.TabIndex = 2
			Me.radioButton4.Text = "I Agree"
			'
			'label4
			'
			Me.label4.Location = New System.Drawing.Point(32, 8)
			Me.label4.Name = "label4"
			Me.label4.TabIndex = 1
			Me.label4.Text = "Must Agree Terms"
			'
			'textBox1
			'
			Me.textBox1.Location = New System.Drawing.Point(32, 32)
			Me.textBox1.Multiline = True
			Me.textBox1.Name = "textBox1"
			Me.textBox1.Size = New System.Drawing.Size(352, 120)
			Me.textBox1.TabIndex = 0
			Me.textBox1.Text = "You must select 'I Agree' before the 'Next' button will enable itself. Me custom " & _
			"action is not part of the WizardControl, see the sample code which is trivial."
			'
			'wizardWarn
			'
			Me.wizardWarn.Controls.AddRange(New System.Windows.Forms.Control() {Me.label6, Me.label5})
			Me.wizardWarn.FullPage = False
			Me.wizardWarn.Name = "wizardWarn"
			Me.wizardWarn.Selected = False
			Me.wizardWarn.Size = New System.Drawing.Size(410, 222)
			Me.wizardWarn.SubTitle = "Me warns user that installation is about to begin"
			Me.wizardWarn.TabIndex = 6
			Me.wizardWarn.Title = "Warn"
			'
			'label6
			'
			Me.label6.Location = New System.Drawing.Point(40, 56)
			Me.label6.Name = "label6"
			Me.label6.Size = New System.Drawing.Size(360, 64)
			Me.label6.TabIndex = 1
			Me.label6.Text = "Warn user that pressing 'Next' will begin installation process."
			'
			'label5
			'
			Me.label5.Location = New System.Drawing.Point(40, 16)
			Me.label5.Name = "label5"
			Me.label5.Size = New System.Drawing.Size(280, 24)
			Me.label5.TabIndex = 0
			Me.label5.Text = "Last page before installation."
			'
			'wizardInstall
			'
			Me.wizardInstall.Controls.AddRange(New System.Windows.Forms.Control() {Me.label7, Me.progressBar1})
			Me.wizardInstall.FullPage = False
			Me.wizardInstall.Name = "wizardInstall"
			Me.wizardInstall.Selected = False
			Me.wizardInstall.Size = New System.Drawing.Size(410, 222)
			Me.wizardInstall.SubTitle = "Perform some fake installation process"
			Me.wizardInstall.TabIndex = 7
			Me.wizardInstall.Title = "Install"
			'
			'label7
			'
			Me.label7.Location = New System.Drawing.Point(40, 16)
			Me.label7.Name = "label7"
			Me.label7.TabIndex = 1
			Me.label7.Text = "Fake Installation"
			'
			'progressBar1
			'
			Me.progressBar1.Location = New System.Drawing.Point(40, 48)
			Me.progressBar1.Name = "progressBar1"
			Me.progressBar1.Size = New System.Drawing.Size(328, 24)
			Me.progressBar1.TabIndex = 0
			'
			'wizardFinish
			'
			Me.wizardFinish.Controls.AddRange(New System.Windows.Forms.Control() {Me.label10})
			Me.wizardFinish.FullPage = False
			Me.wizardFinish.Name = "wizardFinish"
			Me.wizardFinish.Selected = False
			Me.wizardFinish.Size = New System.Drawing.Size(410, 222)
			Me.wizardFinish.SubTitle = "Me page gives the success or failure of attempting the previous install process"
			Me.wizardFinish.TabIndex = 8
			Me.wizardFinish.Title = "Finished"
			'
			'label10
			'
			Me.label10.Location = New System.Drawing.Point(32, 16)
			Me.label10.Name = "label10"
			Me.label10.Size = New System.Drawing.Size(304, 104)
			Me.label10.TabIndex = 0
			Me.label10.Text = "Installation has completed with success."
			'
			'wizardInfo2
			'
			Me.wizardInfo2.Controls.AddRange(New System.Windows.Forms.Control() {Me.label9, Me.textBox3, Me.label8, Me.textBox2})
			Me.wizardInfo2.FullPage = False
			Me.wizardInfo2.Name = "wizardInfo2"
			Me.wizardInfo2.Selected = False
			Me.wizardInfo2.Size = New System.Drawing.Size(410, 222)
			Me.wizardInfo2.SubTitle = "Me is the second of two pages for gathering input"
			Me.wizardInfo2.TabIndex = 9
			Me.wizardInfo2.Title = "Info2"
			'
			'label9
			'
			Me.label9.Location = New System.Drawing.Point(48, 80)
			Me.label9.Name = "label9"
			Me.label9.Size = New System.Drawing.Size(136, 23)
			Me.label9.TabIndex = 3
			Me.label9.Text = "Enter Company Name"
			'
			'textBox3
			'
			Me.textBox3.Location = New System.Drawing.Point(48, 104)
			Me.textBox3.Name = "textBox3"
			Me.textBox3.Size = New System.Drawing.Size(160, 21)
			Me.textBox3.TabIndex = 2
			Me.textBox3.Text = "ACNE Corp."
			'
			'label8
			'
			Me.label8.Location = New System.Drawing.Point(48, 16)
			Me.label8.Name = "label8"
			Me.label8.TabIndex = 1
			Me.label8.Text = "Enter Username"
			'
			'textBox2
			'
			Me.textBox2.Location = New System.Drawing.Point(48, 40)
			Me.textBox2.Name = "textBox2"
			Me.textBox2.Size = New System.Drawing.Size(160, 21)
			Me.textBox2.TabIndex = 0
			Me.textBox2.Text = "Anon"
			'
			'SampleWizard
			'
			Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
			Me.ClientSize = New System.Drawing.Size(410, 351)
			Me.Controls.AddRange(New System.Windows.Forms.Control() {Me.wizardControl})
			Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
			Me.Name = "SampleWizard"
			Me.wizardStartPage.ResumeLayout(False)
			Me.wizardInfo1.ResumeLayout(False)
			Me.groupBox1.ResumeLayout(False)
			Me.wizardLegal.ResumeLayout(False)
			Me.wizardWarn.ResumeLayout(False)
			Me.wizardInstall.ResumeLayout(False)
			Me.wizardFinish.ResumeLayout(False)
			Me.wizardInfo2.ResumeLayout(False)
			Me.ResumeLayout(False)

		End Sub
#End Region

		Private Sub radioButton4_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
			If (Me.radioButton4.Checked) Then
				Me.wizardControl.EnableNextButton = WizardControl.Status.Default
				radioButton5.Checked = False
			Else
				Me.wizardControl.EnableNextButton = WizardControl.Status.No
				radioButton5.Checked = True
			End If
		End Sub

		Private Sub radioButton5_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
			If (Me.radioButton5.Checked) Then
				Me.wizardControl.EnableNextButton = WizardControl.Status.No
				radioButton4.Checked = False
			Else
				Me.wizardControl.EnableNextButton = WizardControl.Status.Default
				radioButton4.Checked = True
			End If
		End Sub

		Protected Overrides Sub OnWizardPageEnter(ByVal wp As Crownwood.Magic.Controls.WizardPage, _
												  ByVal wc As Crownwood.Magic.Controls.WizardControl)
			' Asking for licence terms by entering page?
			If (wp.Name = "wizardLegal") Then
				If (Me.radioButton4.Checked) Then
					wc.EnableNextButton = WizardControl.Status.Default
				Else
					wc.EnableNextButton = WizardControl.Status.No
				End If
			End If

			' Started the installation process by entering page 5?
			If (wp.Name = "wizardInstall") Then
				' Kick off a timer to represent progress
				installCount = 0
				installTimer = New Timer()
				installTimer.Interval = 250
				AddHandler installTimer.Tick, AddressOf OnProgressTick
				installTimer.Start()
			End If
		End Sub

		Protected Overrides Sub OnWizardPageLeave(ByVal wp As Crownwood.Magic.Controls.WizardPage, _
												  ByVal wc As Crownwood.Magic.Controls.WizardControl)
			' Leaving page means we have to restore default status of next button
			If (wp.Name = "wizardLegal") Then
				' Default the next button to disable
				wc.EnableNextButton = WizardControl.Status.Default
			End If
		End Sub

		Protected Overrides Sub OnCancelClick(ByVal sender As Object, ByVal e As System.EventArgs)
			' Suspend any installation process if happening
			If Not (installTimer Is Nothing) Then installTimer.Stop()

			If (MessageBox.Show(Me, "Sure you want to exit?", "Cancel Pressed", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.Yes) Then
				' Let base class close the form
				MyBase.OnCancelClick(sender, e)
			Else
				' Resume any installation process if happening
				If Not (installTimer Is Nothing) Then installTimer.Start()
			End If
		End Sub

		Private Sub OnProgressTick(ByVal sender As Object, ByVal e As System.EventArgs)
			installCount += 1

			' Finished yet?
			If (installCount = 20) Then
				' No longer need to simulate actions
				installTimer.Stop()

				' Move to last page
				MyBase.wizardControl.SelectedIndex = MyBase.wizardControl.WizardPages.Count - 1
			Else
				' Update percentage completed
				progressBar1.Value = 100 / 20 * installCount
			End If
		End Sub
	End Class
End Namespace

