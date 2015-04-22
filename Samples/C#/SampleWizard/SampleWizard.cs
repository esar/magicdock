// *****************************************************************************
// 
//  (c) Crownwood Consulting Limited 2002-2003
//  All rights reserved. The software and associated documentation 
//  supplied hereunder are the proprietary information of Crownwood Consulting 
//	Limited, Crownwood, Bracknell, Berkshire, England and are supplied subject 
//  to licence terms.
// 
//  Magic Version 1.7.4.0 	www.dotnetmagic.com
// *****************************************************************************

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Crownwood.Magic.Controls;

namespace SampleWizard
{
	public class SampleWizard : Crownwood.Magic.Forms.WizardDialog
	{
	    private Timer installTimer;
	    private int installCount;
        private Crownwood.Magic.Controls.WizardPage wizardStartPage;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private Crownwood.Magic.Controls.WizardPage wizardInfo1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton3;
        private Crownwood.Magic.Controls.WizardPage wizardLegal;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RadioButton radioButton4;
        private System.Windows.Forms.RadioButton radioButton5;
        private Crownwood.Magic.Controls.WizardPage wizardInstall;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label7;
        private Crownwood.Magic.Controls.WizardPage wizardFinish;
        private Crownwood.Magic.Controls.WizardPage wizardWarn;
        private Crownwood.Magic.Controls.WizardPage wizardInfo2;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
		private System.ComponentModel.IContainer components = null;

		public SampleWizard()
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.wizardStartPage = new Crownwood.Magic.Controls.WizardPage();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.wizardInfo1 = new Crownwood.Magic.Controls.WizardPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.wizardLegal = new Crownwood.Magic.Controls.WizardPage();
            this.radioButton5 = new System.Windows.Forms.RadioButton();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.wizardWarn = new Crownwood.Magic.Controls.WizardPage();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.wizardInstall = new Crownwood.Magic.Controls.WizardPage();
            this.label7 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.wizardFinish = new Crownwood.Magic.Controls.WizardPage();
            this.label10 = new System.Windows.Forms.Label();
            this.wizardInfo2 = new Crownwood.Magic.Controls.WizardPage();
            this.label9 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.wizardStartPage.SuspendLayout();
            this.wizardInfo1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.wizardLegal.SuspendLayout();
            this.wizardWarn.SuspendLayout();
            this.wizardInstall.SuspendLayout();
            this.wizardFinish.SuspendLayout();
            this.wizardInfo2.SuspendLayout();
            this.SuspendLayout();
            // 
            // wizardControl
            // 
            this.wizardControl.AssignDefaultButton = true;
            this.wizardControl.Profile = Crownwood.Magic.Controls.WizardControl.Profiles.Install;
            this.wizardControl.SelectedIndex = 0;
            this.wizardControl.Size = new System.Drawing.Size(410, 343);
            this.wizardControl.Title = "Sample Wizard using Install Profile";
            this.wizardControl.Visible = true;
            this.wizardControl.WizardPages.AddRange(new Crownwood.Magic.Controls.WizardPage[] {
                                                                                                  this.wizardStartPage,
                                                                                                  this.wizardLegal,
                                                                                                  this.wizardInfo1,
                                                                                                  this.wizardInfo2,
                                                                                                  this.wizardWarn,
                                                                                                  this.wizardInstall,
                                                                                                  this.wizardFinish});
            // 
            // wizardStartPage
            // 
            this.wizardStartPage.CaptionTitle = "Sample Description";
            this.wizardStartPage.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                                          this.label3,
                                                                                          this.label2,
                                                                                          this.label1});
            this.wizardStartPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardStartPage.FullPage = false;
            this.wizardStartPage.Name = "wizardStartPage";
            this.wizardStartPage.Size = new System.Drawing.Size(410, 189);
            this.wizardStartPage.SubTitle = "Start page explaining what this sample demonstrates";
            this.wizardStartPage.TabIndex = 3;
            this.wizardStartPage.Title = "Start";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(8, 136);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(400, 64);
            this.label3.TabIndex = 3;
            this.label3.Text = @"Therefore on the second to last page only the 'Cancel' button is available and used to abort the installation. On the last page only the 'Close' button is available as the only possible action left is to exit the dialog.  On preceding pages the 'Next' and 'Back' buttons are displayed when appropriate as well as the 'Cancel' button.";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(8, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(400, 64);
            this.label2.TabIndex = 2;
            this.label2.Text = @"In this profile the last two pages have special significance. The second to last is used for conducting the actual installation. The last page is used to report the success or failure of the install. All other preceding pages are for information gathering prior to the install attempt.";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(8, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(400, 56);
            this.label1.TabIndex = 1;
            this.label1.Text = @"The WizardControl and associated WizardDialog can be used in three different profiles depending on the type of Wizard required. Available options are 'Install', 'Configure' and 'Controller'. This sample is intended to demonstrate use of the 'Install' profile.";
            // 
            // wizardInfo1
            // 
            this.wizardInfo1.CaptionTitle = "Gather Info 1";
            this.wizardInfo1.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                                      this.groupBox1});
            this.wizardInfo1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardInfo1.FullPage = false;
            this.wizardInfo1.Name = "wizardInfo1";
            this.wizardInfo1.Selected = false;
            this.wizardInfo1.Size = new System.Drawing.Size(410, 269);
            this.wizardInfo1.SubTitle = "This is the first of two pages for gathering input";
            this.wizardInfo1.TabIndex = 4;
            this.wizardInfo1.Title = "Info1";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                                    this.radioButton3,
                                                                                    this.radioButton2,
                                                                                    this.radioButton1});
            this.groupBox1.Location = new System.Drawing.Point(24, 16);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(160, 152);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Example of Selection";
            // 
            // radioButton3
            // 
            this.radioButton3.Location = new System.Drawing.Point(32, 112);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.TabIndex = 2;
            this.radioButton3.Text = "Debug Install";
            // 
            // radioButton2
            // 
            this.radioButton2.Location = new System.Drawing.Point(32, 72);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.TabIndex = 1;
            this.radioButton2.Text = "Server Install";
            // 
            // radioButton1
            // 
            this.radioButton1.Location = new System.Drawing.Point(32, 32);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.TabIndex = 0;
            this.radioButton1.Text = "Client Install";
            // 
            // wizardLegal
            // 
            this.wizardLegal.CaptionTitle = "Standard Legal Notice";
            this.wizardLegal.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                                      this.radioButton5,
                                                                                      this.radioButton4,
                                                                                      this.label4,
                                                                                      this.textBox1});
            this.wizardLegal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardLegal.FullPage = false;
            this.wizardLegal.Name = "wizardLegal";
            this.wizardLegal.Selected = false;
            this.wizardLegal.Size = new System.Drawing.Size(410, 269);
            this.wizardLegal.SubTitle = "Force the user to agree a license agreement for product";
            this.wizardLegal.TabIndex = 5;
            this.wizardLegal.Title = "Legal";
            // 
            // radioButton5
            // 
            this.radioButton5.Checked = true;
            this.radioButton5.Location = new System.Drawing.Point(216, 160);
            this.radioButton5.Name = "radioButton5";
            this.radioButton5.Size = new System.Drawing.Size(88, 24);
            this.radioButton5.TabIndex = 3;
            this.radioButton5.TabStop = true;
            this.radioButton5.Text = "I Disagree";
            this.radioButton5.CheckedChanged += new System.EventHandler(this.radioButton5_CheckedChanged);
            // 
            // radioButton4
            // 
            this.radioButton4.Location = new System.Drawing.Point(120, 160);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(72, 24);
            this.radioButton4.TabIndex = 2;
            this.radioButton4.Text = "I Agree";
            this.radioButton4.CheckedChanged += new System.EventHandler(this.radioButton4_CheckedChanged);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(32, 8);
            this.label4.Name = "label4";
            this.label4.TabIndex = 1;
            this.label4.Text = "Must Agree Terms";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(32, 32);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(352, 120);
            this.textBox1.TabIndex = 0;
            this.textBox1.Text = "You must select \'I Agree\' before the \'Next\' button will enable itself. This custo" +
                "m action is not part of the WizardControl, see the sample code which is trivial." +
                "";
            // 
            // wizardWarn
            // 
            this.wizardWarn.CaptionTitle = "Warning, about to install";
            this.wizardWarn.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                                     this.label6,
                                                                                     this.label5});
            this.wizardWarn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardWarn.FullPage = false;
            this.wizardWarn.Name = "wizardWarn";
            this.wizardWarn.Selected = false;
            this.wizardWarn.Size = new System.Drawing.Size(410, 269);
            this.wizardWarn.SubTitle = "This warns user that installation is about to begin";
            this.wizardWarn.TabIndex = 6;
            this.wizardWarn.Title = "Warn";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(40, 56);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(360, 64);
            this.label6.TabIndex = 1;
            this.label6.Text = "Warn user that pressing \'Next\' will begin installation process.";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(40, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(280, 24);
            this.label5.TabIndex = 0;
            this.label5.Text = "Last page before installation.";
            // 
            // wizardInstall
            // 
            this.wizardInstall.CaptionTitle = "Installing";
            this.wizardInstall.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                                        this.label7,
                                                                                        this.progressBar1});
            this.wizardInstall.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardInstall.FullPage = false;
            this.wizardInstall.Name = "wizardInstall";
            this.wizardInstall.Selected = false;
            this.wizardInstall.Size = new System.Drawing.Size(410, 269);
            this.wizardInstall.SubTitle = "Perform some fake installation process";
            this.wizardInstall.TabIndex = 7;
            this.wizardInstall.Title = "Install";
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(40, 16);
            this.label7.Name = "label7";
            this.label7.TabIndex = 1;
            this.label7.Text = "Fake Installation";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(40, 48);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(328, 24);
            this.progressBar1.TabIndex = 0;
            // 
            // wizardFinish
            // 
            this.wizardFinish.CaptionTitle = "Intall Complete";
            this.wizardFinish.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                                       this.label10});
            this.wizardFinish.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardFinish.FullPage = false;
            this.wizardFinish.Name = "wizardFinish";
            this.wizardFinish.Selected = false;
            this.wizardFinish.Size = new System.Drawing.Size(410, 269);
            this.wizardFinish.SubTitle = "This page gives the success or failure of attempting the previous install process" +
                "";
            this.wizardFinish.TabIndex = 8;
            this.wizardFinish.Title = "Finished";
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(32, 16);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(304, 104);
            this.label10.TabIndex = 0;
            this.label10.Text = "Installation has completed with success.";
            // 
            // wizardInfo2
            // 
            this.wizardInfo2.CaptionTitle = "Gather Info 2";
            this.wizardInfo2.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                                      this.label9,
                                                                                      this.textBox3,
                                                                                      this.label8,
                                                                                      this.textBox2});
            this.wizardInfo2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardInfo2.FullPage = false;
            this.wizardInfo2.Name = "wizardInfo2";
            this.wizardInfo2.Selected = false;
            this.wizardInfo2.Size = new System.Drawing.Size(410, 269);
            this.wizardInfo2.SubTitle = "This is the second of two pages for gathering input";
            this.wizardInfo2.TabIndex = 9;
            this.wizardInfo2.Title = "Info2";
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(48, 80);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(136, 23);
            this.label9.TabIndex = 3;
            this.label9.Text = "Enter Company Name";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(48, 104);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(160, 21);
            this.textBox3.TabIndex = 2;
            this.textBox3.Text = "ACNE Corp.";
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(48, 16);
            this.label8.Name = "label8";
            this.label8.TabIndex = 1;
            this.label8.Text = "Enter Username";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(48, 40);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(160, 21);
            this.textBox2.TabIndex = 0;
            this.textBox2.Text = "Anon";
            // 
            // SampleWizard
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(410, 343);
            this.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                          this.wizardControl});
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "SampleWizard";
            this.TitleMode = Crownwood.Magic.Forms.WizardDialog.TitleModes.Steps;
            this.wizardStartPage.ResumeLayout(false);
            this.wizardInfo1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.wizardLegal.ResumeLayout(false);
            this.wizardWarn.ResumeLayout(false);
            this.wizardInstall.ResumeLayout(false);
            this.wizardFinish.ResumeLayout(false);
            this.wizardInfo2.ResumeLayout(false);
            this.ResumeLayout(false);

        }
		#endregion

        private void radioButton4_CheckedChanged(object sender, System.EventArgs e)
        {
            if (this.radioButton4.Checked)
            {
               this.wizardControl.EnableNextButton = WizardControl.Status.Default;
               radioButton5.Checked = false;
            }
            else                    
            {
                this.wizardControl.EnableNextButton = WizardControl.Status.No;
               radioButton5.Checked = true;
            }
        }

        private void radioButton5_CheckedChanged(object sender, System.EventArgs e)
        {
            if (this.radioButton5.Checked)
            {
                this.wizardControl.EnableNextButton = WizardControl.Status.No;
                radioButton4.Checked = false;
            }
            else                    
            {
                this.wizardControl.EnableNextButton = WizardControl.Status.Default;
                radioButton4.Checked = true;
            }
        }
    
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() 
        {
            Application.Run(new SampleWizard());
        }

        protected override void OnWizardPageEnter(Crownwood.Magic.Controls.WizardPage wp, 
                                                  Crownwood.Magic.Controls.WizardControl wc)
        {
            // Asking for licence terms by entering page?
            if (wp.Name == "wizardLegal")
            {
                if (this.radioButton4.Checked)
                    wc.EnableNextButton = WizardControl.Status.Default;
                else
                    wc.EnableNextButton = WizardControl.Status.No;
            }
            
            // Started the installation process by entering page 5?
            if (wp.Name == "wizardInstall")
            {
                // Kick off a timer to represent progress
                installCount = 0;
                installTimer = new Timer();
                installTimer.Interval = 250;
                installTimer.Tick += new EventHandler(OnProgressTick);
                installTimer.Start();
            }
        }
        
        protected override void OnWizardPageLeave(Crownwood.Magic.Controls.WizardPage wp, 
                                                  Crownwood.Magic.Controls.WizardControl wc)
        {
            // Leaving page means we have to restore default status of next button
            if (wp.Name == "wizardLegal")
            {
                // Default the next button to disable
                wc.EnableNextButton = WizardControl.Status.Default;
            }
        }
            
        protected override void OnCancelClick(object sender, System.EventArgs e)
        {
            // Suspend any installation process if happening
            if (installTimer != null)
                installTimer.Stop();
        
            if (MessageBox.Show(this, "Sure you want to exit?", "Cancel Pressed", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                // Let base class close the form
                base.OnCancelClick(sender, e);
            }
            else
            {
                // Resume any installation process if happening
                if (installTimer != null)
                    installTimer.Start();
            }
        }

        private void OnProgressTick(object sender, EventArgs e)
        {
            installCount++;
            
            // Finished yet?
            if (installCount >= 20)
            {
                // No longer need to simulate actions
                installTimer.Stop();
                
                // Move to last page
                base.wizardControl.SelectedIndex = base.wizardControl.WizardPages.Count - 1;
            }
            else
            {
                // Update percentage completed
                progressBar1.Value = 100 / 20 * installCount;   
            }
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            this.wizardControl.WizardPages.Add(new WizardPage());
        }
    }
}

