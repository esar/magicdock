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
using System.Data;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Crownwood.Magic.Menus;
using Crownwood.Magic.Common;
using Crownwood.Magic.Controls;
using Crownwood.Magic.Docking;
using Crownwood.Magic.Win32;


namespace SampleTabbedGroups
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class SampleForm : System.Windows.Forms.Form
	{
	    private int _count = 1;
        private int _image = -1;
        private RichTextBox _global;
        private DockingManager _manager;
	
        private Crownwood.Magic.Menus.MenuControl menuControl1;
        private System.Windows.Forms.StatusBar statusBar1;
        private Crownwood.Magic.Controls.TabControl tabControl1;
        private Crownwood.Magic.Controls.TabPage tabPage1;
        private Crownwood.Magic.Controls.TabPage tabPage2;
        private Crownwood.Magic.Controls.TabPage tabPage3;
        private Crownwood.Magic.Controls.TabbedGroups tabbedGroups1;
        private Crownwood.Magic.Controls.TabbedGroups tabbedGroups2;
        private Crownwood.Magic.Controls.TabbedGroups tabbedGroups3;
        private System.Windows.Forms.ImageList mainTabs;
        private System.Windows.Forms.ImageList groupTabs;
        private System.ComponentModel.IContainer components;

		public SampleForm()
		{
			// Required for Windows Form Designer support
			InitializeComponent();
			
			// Create menu options
			CreateMenu();
			
            // Define the docking windows
            CreateDockingWindows();
            
            // Create some initial tab pages inside each group
			CreateInitialPages();
		}
		
		protected void CreateMenu()
		{
		    // Create top level commands
            MenuCommand pages = CreatePages();
            MenuCommand persist = CreatePersistence();
            MenuCommand tabsMode = CreateTabsMode();
            
            // Add top level commands
            menuControl1.MenuCommands.AddRange(new MenuCommand[] {pages, persist, tabsMode});	    
		}

        protected MenuCommand CreatePages()
        {
            MenuCommand pages = new MenuCommand("Pages");
            
            // Create pages sub commands
            MenuCommand add = new MenuCommand("Add", new EventHandler(OnAddPage));
            MenuCommand remove = new MenuCommand("Remove", new EventHandler(OnRemovePage));

            pages.MenuCommands.AddRange(new MenuCommand[] {add, remove});
                                                                
            // Enable/disable the remove option as appropriate
            pages.PopupStart += new CommandHandler(OnPages);

            return pages;                                                                
        }

        protected MenuCommand CreatePersistence()
        {
            MenuCommand persist = new MenuCommand("Persistence");
            
            // Create Persistence sub commands
            MenuCommand saveG1 = new MenuCommand("Save Group1", new EventHandler(OnSaveG1));
            MenuCommand loadG1 = new MenuCommand("Load Group1", new EventHandler(OnLoadG1));
            MenuCommand sep1 = new MenuCommand("-");
            MenuCommand saveG2 = new MenuCommand("Save Group2", new EventHandler(OnSaveG2));
            MenuCommand loadG2 = new MenuCommand("Load Group2", new EventHandler(OnLoadG2));
            MenuCommand sep2 = new MenuCommand("-");
            MenuCommand saveG3 = new MenuCommand("Save Group3", new EventHandler(OnSaveG3));
            MenuCommand loadG3 = new MenuCommand("Load Group3", new EventHandler(OnLoadG3));
            
            persist.MenuCommands.AddRange(new MenuCommand[] {saveG1, loadG1, sep1,
                                                             saveG2, loadG2, sep2,
                                                             saveG3, loadG3});
                                                                
            return persist;                                                                
        }
		
        protected MenuCommand CreateTabsMode()
        {
            MenuCommand tabsMode = new MenuCommand("DisplayMode");
            
            // Create modes sub commands
            MenuCommand hideAll = new MenuCommand("Hide All", new EventHandler(OnHideAll));
            MenuCommand showAll = new MenuCommand("Show All", new EventHandler(OnShowAll));
            MenuCommand showActiveLeaf = new MenuCommand("Show Active Leaf", new EventHandler(OnShowActiveLeaf));
            MenuCommand showMouseOver = new MenuCommand("Show Mouse Over", new EventHandler(OnShowMouseOver));
            MenuCommand showActiveAndMouseOver = new MenuCommand("Show Active And Mouse Over", new EventHandler(OnShowActiveAndMouseOver));

            
            tabsMode.MenuCommands.AddRange(new MenuCommand[] {hideAll, showAll, showActiveLeaf,
                                                              showMouseOver, showActiveAndMouseOver});

            // Set correct check mark when menu opened
            tabsMode.PopupStart += new CommandHandler(OnDisplayMode);
                                                                
            return tabsMode;                                                                
        }
        
        protected void CreateDockingWindows()
		{
		    // Create the docking manager instance
		    _manager = new DockingManager(this, VisualStyle.IDE);

            // Define innner/outer controls for correct docking operation
            _manager.InnerControl = tabControl1;
            _manager.OuterControl = menuControl1;
		    
		    // Create the tree control
		    TreeView tv = new DragTree();
		    tv.Nodes.Add(new TreeNode("First"));
            tv.Nodes.Add(new TreeNode("Second"));
            tv.Nodes.Add(new TreeNode("Third"));
            tv.Nodes.Add(new TreeNode("Fourth"));
            
            // Create a rich text box for the second content
            _global = new RichTextBox();
            
            // Create content instances
            Content c1 = _manager.Contents.Add(tv, "TreeView");
            Content c2 = _manager.Contents.Add(_global, "Another Window");
            
            // Add to the display on the left hand side
            WindowContent wc = _manager.AddContentWithState(c1, State.DockLeft);
            
            // Add at the bottom of the same column
            _manager.AddContentToZone(c2, wc.ParentZone, 1);
            
        }

        protected void CreateInitialPages()
        {
            CreateInitialPagesGroup1();
            CreateInitialPagesGroup2();
            CreateInitialPagesGroup3();
        }
        
        protected void CreateInitialPagesGroup1()
        {        
            // Access the default leaf group
            TabGroupLeaf tgl = tabbedGroups1.RootSequence[0] as TabGroupLeaf;
            
            // Create two pages for the leaf
            Crownwood.Magic.Controls.TabPage tp1 = new Crownwood.Magic.Controls.TabPage("Page" + _count++, new RichTextBox(), NextImage());
            Crownwood.Magic.Controls.TabPage tp2 = new Crownwood.Magic.Controls.TabPage("Page" + _count++, new RichTextBox(), NextImage());
            
            // Add a two pages to the leaf
            tgl.TabPages.Add(tp1);
            tgl.TabPages.Add(tp2);
        }

        protected void CreateInitialPagesGroup2()
        {        
            // Access the default leaf group
            TabGroupLeaf tgl1 = tabbedGroups2.RootSequence[0] as TabGroupLeaf;
            
            // Add a new leaf group in the same sequence
            TabGroupLeaf tgl2 = tabbedGroups2.RootSequence.AddNewLeaf();
            
            // Add a two pages to the leaf
            tgl1.TabPages.Add(NewTabPage());
            tgl2.TabPages.Add(NewTabPage());
        }

        protected void CreateInitialPagesGroup3()
        {        
            // Change direction to opposite
            tabbedGroups3.RootSequence.Direction = Direction.Vertical;
        
            // Access the default leaf group
            TabGroupLeaf tgl1 = tabbedGroups3.RootSequence[0] as TabGroupLeaf;
            
            // Add a new leaf group in the same sequence
            TabGroupLeaf tgl2 = tabbedGroups3.RootSequence.AddNewLeaf();
            
            // Add a two pages to the leaf
            tgl1.TabPages.Add(NewTabPage());
            tgl2.TabPages.Add(NewTabPage());
        }

        protected int NextImage()
        {
            _image = ++_image % 8;
            return _image;
        }
        
        protected Crownwood.Magic.Controls.TabPage NewTabPage()
        {
            return new Crownwood.Magic.Controls.TabPage("Page" + _count++, new RichTextBox(), NextImage());       
        }

        protected void OnPages(MenuCommand pages)
        {
            Crownwood.Magic.Controls.TabControl tc = null;

            // Find the active tab control in the selected group 
            if (tabControl1.SelectedIndex == 0)
            {
                if (tabbedGroups1.ActiveLeaf != null)
                    tc = tabbedGroups1.ActiveLeaf.GroupControl as Crownwood.Magic.Controls.TabControl;
            }
            else
            {
                if (tabControl1.SelectedIndex == 1)
                {
                    if (tabbedGroups2.ActiveLeaf != null)
                        tc = tabbedGroups2.ActiveLeaf.GroupControl as Crownwood.Magic.Controls.TabControl;
                }
                else
                {
                    if (tabbedGroups3.ActiveLeaf != null)
                        tc = tabbedGroups3.ActiveLeaf.GroupControl as Crownwood.Magic.Controls.TabControl;
                }
            }
            
            // Did we find a current tab control?
            if ((tc != null) && (tc.SelectedTab != null))
                pages.MenuCommands[1].Enabled = true;
            else
                pages.MenuCommands[1].Enabled = false;
        }

        protected void OnAddPage(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                if (tabbedGroups1.ActiveLeaf != null)
                    tabbedGroups1.ActiveLeaf.TabPages.Add(NewTabPage());
            }
            else
            {
                if (tabControl1.SelectedIndex == 1)
                {
                    if (tabbedGroups2.ActiveLeaf != null)
                        tabbedGroups2.ActiveLeaf.TabPages.Add(NewTabPage());
                }
                else
                {
                    if (tabbedGroups3.ActiveLeaf != null)
                        tabbedGroups3.ActiveLeaf.TabPages.Add(NewTabPage());
                }
            }
        }

        protected void OnRemovePage(object sender, EventArgs e)
        {
            Crownwood.Magic.Controls.TabControl tc = null;

            // Find the active tab control in the selected group 
            if (tabControl1.SelectedIndex == 0)
            {
                if (tabbedGroups1.ActiveLeaf != null)
                    tc = tabbedGroups1.ActiveLeaf.GroupControl as Crownwood.Magic.Controls.TabControl;
            }
            else
            {
                if (tabControl1.SelectedIndex == 1)
                {
                    if (tabbedGroups2.ActiveLeaf != null)
                        tc = tabbedGroups2.ActiveLeaf.GroupControl as Crownwood.Magic.Controls.TabControl;
                }
                else
                {
                    if (tabbedGroups3.ActiveLeaf != null)
                        tc = tabbedGroups3.ActiveLeaf.GroupControl as Crownwood.Magic.Controls.TabControl;
                }
            }
            
            // Did we find a current tab control?
            if (tc != null)
            {
                // Does it have a selected tab?
                if (tc.SelectedTab != null)
                {
                    // Remove the page
                    tc.TabPages.Remove(tc.SelectedTab);
                }
            }
        }    
            
        protected void OnSaveG1(object sender, EventArgs e)
        {
            tabbedGroups1.SaveConfigToFile(@"Group1.xml");
        }

        protected void OnLoadG1(object sender, EventArgs e)
        {
            try
            {
                tabbedGroups1.LoadConfigFromFile(@"Group1.xml");
            }
            finally
            {
            }
        }

        protected void OnSaveG2(object sender, EventArgs e)
        {
            tabbedGroups2.SaveConfigToFile(@"Group2.xml");
        }

        protected void OnLoadG2(object sender, EventArgs e)
        {
            try
            {
                tabbedGroups2.LoadConfigFromFile(@"Group2.xml");
            }
            finally
            {
            }
        }

        protected void OnSaveG3(object sender, EventArgs e)
        {
            tabbedGroups3.SaveConfigToFile(@"Group3.xml");
        }

        protected void OnLoadG3(object sender, EventArgs e)
        {
            try
            {
                tabbedGroups3.LoadConfigFromFile(@"Group3.xml");
            }
            finally
            {
            }
        }

        protected void OnDisplayMode(MenuCommand tabsMode)
        {
            // Default all the commands to not being checked
            foreach(MenuCommand mc in tabsMode.MenuCommands)
                mc.Checked = false;
            
            switch(tabbedGroups1.DisplayTabMode)
            {
                case TabbedGroups.DisplayTabModes.HideAll:
                    tabsMode.MenuCommands[0].Checked = true;
                    break;
                case TabbedGroups.DisplayTabModes.ShowAll:
                    tabsMode.MenuCommands[1].Checked = true;
                    break;
                case TabbedGroups.DisplayTabModes.ShowActiveLeaf:
                    tabsMode.MenuCommands[2].Checked = true;
                    break;
                case TabbedGroups.DisplayTabModes.ShowMouseOver:
                    tabsMode.MenuCommands[3].Checked = true;
                    break;
                case TabbedGroups.DisplayTabModes.ShowActiveAndMouseOver:
                    tabsMode.MenuCommands[4].Checked = true;
                    break;
            }
        }

        protected void OnHideAll(object sender, EventArgs e)
        {
            tabbedGroups1.DisplayTabMode = TabbedGroups.DisplayTabModes.HideAll;
            tabbedGroups2.DisplayTabMode = TabbedGroups.DisplayTabModes.HideAll;
            tabbedGroups3.DisplayTabMode = TabbedGroups.DisplayTabModes.HideAll;
        }
        
        protected void OnShowAll(object sender, EventArgs e)
        {
            tabbedGroups1.DisplayTabMode = TabbedGroups.DisplayTabModes.ShowAll;
            tabbedGroups2.DisplayTabMode = TabbedGroups.DisplayTabModes.ShowAll;
            tabbedGroups3.DisplayTabMode = TabbedGroups.DisplayTabModes.ShowAll;
        }

        protected void OnShowActiveLeaf(object sender, EventArgs e)
        {
            tabbedGroups1.DisplayTabMode = TabbedGroups.DisplayTabModes.ShowActiveLeaf;
            tabbedGroups2.DisplayTabMode = TabbedGroups.DisplayTabModes.ShowActiveLeaf;
            tabbedGroups3.DisplayTabMode = TabbedGroups.DisplayTabModes.ShowActiveLeaf;
        }

        protected void OnShowMouseOver(object sender, EventArgs e)
        {
            tabbedGroups1.DisplayTabMode = TabbedGroups.DisplayTabModes.ShowMouseOver;
            tabbedGroups2.DisplayTabMode = TabbedGroups.DisplayTabModes.ShowMouseOver;
            tabbedGroups3.DisplayTabMode = TabbedGroups.DisplayTabModes.ShowMouseOver;
        }

        protected void OnShowActiveAndMouseOver(object sender, EventArgs e)
        {
            tabbedGroups1.DisplayTabMode = TabbedGroups.DisplayTabModes.ShowActiveAndMouseOver;
            tabbedGroups2.DisplayTabMode = TabbedGroups.DisplayTabModes.ShowActiveAndMouseOver;
            tabbedGroups3.DisplayTabMode = TabbedGroups.DisplayTabModes.ShowActiveAndMouseOver;
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(SampleForm));
            this.menuControl1 = new Crownwood.Magic.Menus.MenuControl();
            this.statusBar1 = new System.Windows.Forms.StatusBar();
            this.tabControl1 = new Crownwood.Magic.Controls.TabControl();
            this.mainTabs = new System.Windows.Forms.ImageList(this.components);
            this.tabPage2 = new Crownwood.Magic.Controls.TabPage();
            this.tabbedGroups2 = new Crownwood.Magic.Controls.TabbedGroups();
            this.groupTabs = new System.Windows.Forms.ImageList(this.components);
            this.tabPage1 = new Crownwood.Magic.Controls.TabPage();
            this.tabbedGroups1 = new Crownwood.Magic.Controls.TabbedGroups();
            this.tabPage3 = new Crownwood.Magic.Controls.TabPage();
            this.tabbedGroups3 = new Crownwood.Magic.Controls.TabbedGroups();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabbedGroups2)).BeginInit();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabbedGroups1)).BeginInit();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabbedGroups3)).BeginInit();
            this.SuspendLayout();
            // 
            // menuControl1
            // 
            this.menuControl1.AnimateStyle = Crownwood.Magic.Menus.Animation.System;
            this.menuControl1.AnimateTime = 100;
            this.menuControl1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.menuControl1.Direction = Crownwood.Magic.Common.Direction.Horizontal;
            this.menuControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.menuControl1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
            this.menuControl1.HighlightTextColor = System.Drawing.SystemColors.MenuText;
            this.menuControl1.Name = "menuControl1";
            this.menuControl1.Size = new System.Drawing.Size(512, 25);
            this.menuControl1.Style = Crownwood.Magic.Common.VisualStyle.IDE;
            this.menuControl1.TabIndex = 0;
            this.menuControl1.TabStop = false;
            this.menuControl1.Text = "menuControl1";
            // 
            // statusBar1
            // 
            this.statusBar1.Location = new System.Drawing.Point(0, 455);
            this.statusBar1.Name = "statusBar1";
            this.statusBar1.Size = new System.Drawing.Size(512, 22);
            this.statusBar1.TabIndex = 1;
            this.statusBar1.Text = "statusBar1";
            // 
            // tabControl1
            // 
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.HideTabsMode = Crownwood.Magic.Controls.TabControl.HideTabsModes.ShowAlways;
            this.tabControl1.ImageList = this.mainTabs;
            this.tabControl1.Location = new System.Drawing.Point(0, 25);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.SelectedTab = this.tabPage1;
            this.tabControl1.Size = new System.Drawing.Size(512, 430);
            this.tabControl1.TabIndex = 2;
            this.tabControl1.TabPages.AddRange(new Crownwood.Magic.Controls.TabPage[] {
                                                                                          this.tabPage1,
                                                                                          this.tabPage2,
                                                                                          this.tabPage3});
            // 
            // mainTabs
            // 
            this.mainTabs.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.mainTabs.ImageSize = new System.Drawing.Size(16, 16);
            this.mainTabs.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("mainTabs.ImageStream")));
            this.mainTabs.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                                   this.tabbedGroups2});
            this.tabPage2.ImageIndex = 1;
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Selected = false;
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Title = "Group2";
            // 
            // tabbedGroups2
            // 
            this.tabbedGroups2.ActiveLeaf = null;
            this.tabbedGroups2.AllowDrop = true;
            this.tabbedGroups2.AtLeastOneLeaf = true;
            this.tabbedGroups2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabbedGroups2.ImageList = this.groupTabs;
            this.tabbedGroups2.Name = "tabbedGroups2";
            this.tabbedGroups2.ProminentLeaf = null;
            this.tabbedGroups2.ResizeBarColor = System.Drawing.SystemColors.Control;
            this.tabbedGroups2.Size = new System.Drawing.Size(384, 301);
            this.tabbedGroups2.TabIndex = 0;
            this.tabbedGroups2.TabControlCreated += new Crownwood.Magic.Controls.TabbedGroups.TabControlCreatedHandler(this.TabControlCreated);
            this.tabbedGroups2.PageSaving += new Crownwood.Magic.Controls.TabbedGroups.PageSavingHandler(this.PageSaving);
            this.tabbedGroups2.ExternalDrop += new Crownwood.Magic.Controls.TabbedGroups.ExternalDropHandler(this.ExternalDrop);
            this.tabbedGroups2.PageLoading += new Crownwood.Magic.Controls.TabbedGroups.PageLoadingHandler(this.PageLoading);
            this.tabbedGroups2.GlobalLoading += new Crownwood.Magic.Controls.TabbedGroups.GlobalLoadingHandler(this.GlobalLoading);
            this.tabbedGroups2.GlobalSaving += new Crownwood.Magic.Controls.TabbedGroups.GlobalSavingHandler(this.GlobalSaving);
            // 
            // groupTabs
            // 
            this.groupTabs.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.groupTabs.ImageSize = new System.Drawing.Size(16, 16);
            this.groupTabs.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("groupTabs.ImageStream")));
            this.groupTabs.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                                   this.tabbedGroups1});
            this.tabPage1.ImageIndex = 0;
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Title = "Group1";
            // 
            // tabbedGroups1
            // 
            this.tabbedGroups1.ActiveLeaf = null;
            this.tabbedGroups1.AllowDrop = true;
            this.tabbedGroups1.AtLeastOneLeaf = true;
            this.tabbedGroups1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabbedGroups1.ImageList = this.groupTabs;
            this.tabbedGroups1.Name = "tabbedGroups1";
            this.tabbedGroups1.ProminentLeaf = null;
            this.tabbedGroups1.ResizeBarColor = System.Drawing.SystemColors.Control;
            this.tabbedGroups1.Size = new System.Drawing.Size(512, 405);
            this.tabbedGroups1.TabIndex = 0;
            this.tabbedGroups1.TabControlCreated += new Crownwood.Magic.Controls.TabbedGroups.TabControlCreatedHandler(this.TabControlCreated);
            this.tabbedGroups1.PageSaving += new Crownwood.Magic.Controls.TabbedGroups.PageSavingHandler(this.PageSaving);
            this.tabbedGroups1.ExternalDrop += new Crownwood.Magic.Controls.TabbedGroups.ExternalDropHandler(this.ExternalDrop);
            this.tabbedGroups1.PageLoading += new Crownwood.Magic.Controls.TabbedGroups.PageLoadingHandler(this.PageLoading);
            this.tabbedGroups1.GlobalLoading += new Crownwood.Magic.Controls.TabbedGroups.GlobalLoadingHandler(this.GlobalLoading);
            this.tabbedGroups1.GlobalSaving += new Crownwood.Magic.Controls.TabbedGroups.GlobalSavingHandler(this.GlobalSaving);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                                   this.tabbedGroups3});
            this.tabPage3.ImageIndex = 2;
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Selected = false;
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Title = "Group3";
            // 
            // tabbedGroups3
            // 
            this.tabbedGroups3.ActiveLeaf = null;
            this.tabbedGroups3.AllowDrop = true;
            this.tabbedGroups3.AtLeastOneLeaf = true;
            this.tabbedGroups3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabbedGroups3.ImageList = this.groupTabs;
            this.tabbedGroups3.Name = "tabbedGroups3";
            this.tabbedGroups3.ProminentLeaf = null;
            this.tabbedGroups3.ResizeBarColor = System.Drawing.SystemColors.Control;
            this.tabbedGroups3.Size = new System.Drawing.Size(384, 301);
            this.tabbedGroups3.TabIndex = 0;
            this.tabbedGroups3.PageSaving += new Crownwood.Magic.Controls.TabbedGroups.PageSavingHandler(this.PageSaving);
            this.tabbedGroups3.ExternalDrop += new Crownwood.Magic.Controls.TabbedGroups.ExternalDropHandler(this.ExternalDrop);
            this.tabbedGroups3.PageLoading += new Crownwood.Magic.Controls.TabbedGroups.PageLoadingHandler(this.PageLoading);
            this.tabbedGroups3.GlobalLoading += new Crownwood.Magic.Controls.TabbedGroups.GlobalLoadingHandler(this.GlobalLoading);
            this.tabbedGroups3.GlobalSaving += new Crownwood.Magic.Controls.TabbedGroups.GlobalSavingHandler(this.GlobalSaving);
            // 
            // SampleForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(512, 477);
            this.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                          this.tabControl1,
                                                                          this.menuControl1,
                                                                          this.statusBar1});
            this.Name = "SampleForm";
            this.Text = "Form1";
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tabbedGroups2)).EndInit();
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tabbedGroups1)).EndInit();
            this.tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tabbedGroups3)).EndInit();
            this.ResumeLayout(false);

        }
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new SampleForm());
		}

        private void PageSaving(Crownwood.Magic.Controls.TabbedGroups tg, Crownwood.Magic.Controls.TGPageSavingEventArgs e)
        {
            // Persist the text box contents
            e.XmlOut.WriteCData((e.TabPage.Control as RichTextBox).Text);
        }

        private void PageLoading(Crownwood.Magic.Controls.TabbedGroups tg, Crownwood.Magic.Controls.TGPageLoadingEventArgs e)
        {
            // Read back the text box contents
            (e.TabPage.Control as RichTextBox).Text = e.XmlIn.ReadString();        
        }

        private void GlobalSaving(Crownwood.Magic.Controls.TabbedGroups tg, System.Xml.XmlTextWriter xmlOut)
        {
            // Persist the global text box contents
            xmlOut.WriteCData(_global.Text);
        }

        private void GlobalLoading(Crownwood.Magic.Controls.TabbedGroups tg, System.Xml.XmlTextReader xmlIn)
        {
            // Read back the global text box contents
            _global.Text = xmlIn.ReadString();        
        }

        private void TabControlCreated(Crownwood.Magic.Controls.TabbedGroups tg, Crownwood.Magic.Controls.TabControl tc)
        {
            // This is where you change the tab control defaults when a new tab control is created
        }
        
        private void ExternalDrop(Crownwood.Magic.Controls.TabbedGroups tg, 
                                  Crownwood.Magic.Controls.TabGroupLeaf tgl,
                                  Crownwood.Magic.Controls.TabControl tc,
                                  Crownwood.Magic.Controls.TabbedGroups.DragProvider dp)
        {
            // Create a new tab page
            Crownwood.Magic.Controls.TabPage tp = NewTabPage();
            
            // Define the text in this control
            (tp.Control as RichTextBox).Text = "Dragged from node '" + (string)dp.Tag + "'";
            
            // We want the new page to become selected
            tp.Selected = true;
            
            // Add new page into the destination tab control
            tgl.TabPages.Add(tp);
        }
	}

    public class DragTree : TreeView
    {
        protected bool _leftDown;
        protected Point _leftPoint;
        protected TreeNode _leftNode;
    
        public DragTree()
            : base()
        {
            _leftDown = false;
        }
        
        protected override void OnMouseDown(MouseEventArgs e)
        {
            // Only interested in the left button
            if (e.Button == MouseButtons.Left)
            {
                TreeNode n = this.GetNodeAt(new Point(e.X, e.Y));
                
                // Are we selecting a valid node?
                if (n != null)
                {
                    // Might be start of a drag, so remember details
                    _leftNode = n;
                    _leftDown = true;
                    _leftPoint = new Point(e.X, e.Y);
                    
                    // Must capture the mouse
                    this.Capture = true;
                    this.Focus();
                }
            }

            base.OnMouseDown(e);
        }
            
        protected override void OnMouseMove(MouseEventArgs e)
        {
            // Are we monitoring for a drag operation?
            if (_leftDown)
            {
                Rectangle dragRect = new Rectangle(_leftPoint, new Size(0,0));
                
                // Create rectangle for drag start
                dragRect.Inflate(SystemInformation.DoubleClickSize);
                
                // Has mouse been dragged outside of rectangle?
                if (!dragRect.Contains(new Point(e.X, e.Y)))
                {
                    // Create an object the TabbedGroups control understands
                    TabbedGroups.DragProvider dp = new TabbedGroups.DragProvider();
                    
                    // Box the node name as the parameter for passing across
                    dp.Tag = (object)_leftNode.Text;
                
                    // Must start a drag operation
                    DoDragDrop(dp, DragDropEffects.Copy);
                    
                    // Cancel any further drag events until mouse is pressed again
                    _leftDown = false;
                    _leftNode = null;
                }
            }
        
            base.OnMouseMove(e);
        }
        
        protected override void WndProc(ref Message m)
        {
            switch(m.Msg)
            {
                case (int)Crownwood.Magic.Win32.Msgs.WM_LBUTTONUP:
                    // Remembering drag info?
                    if (_leftDown)
                    {
                        // Cancel any drag attempt
                        _leftDown = false;
                        _leftNode = null;
                    }   
                    break;
            }
        
            base.WndProc(ref m);
        }
    }
}
