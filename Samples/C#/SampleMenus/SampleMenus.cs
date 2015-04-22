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
using System.IO;
using System.Data;
using System.Drawing;
using System.Resources;
using System.Reflection;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using Crownwood.Magic.Menus;
using Crownwood.Magic.Win32;
using Crownwood.Magic.Common;
using Crownwood.Magic.Controls;

namespace SampleMenus
{
    public class MDIContainer : System.Windows.Forms.Form
    {
		private int _count = 1;
        private ImageList _images = null;
        private StatusBar _status = null;
        private StatusBarPanel _statusBarPanel = null;
        private Crownwood.Magic.Menus.MenuControl _topMenu = null;
        private System.ComponentModel.Container components = null;

        public MDIContainer()
        {
            LoadResources();

            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            SetupMenus();
            SetupStatusBar();
        }

        protected void LoadResources()
        {
            // Create a strip of images by loading an embedded bitmap resource
            _images = ResourceHelper.LoadBitmapStrip(this.GetType(),
                                                     "SampleMenus.MenuImages.bmp",
                                                     new Size(16,16),
                                                     new Point(0,0));
        }

        protected void SetupMenus()
        {
            // Create the MenuControl
            _topMenu = new Crownwood.Magic.Menus.MenuControl();

            // We want the control to handle the MDI pendant
            _topMenu.MdiContainer = this;

            // Create the top level Menu
            MenuCommand top1 = new MenuCommand("&Appearance");
            MenuCommand top2 = new MenuCommand("&Windows");
            MenuCommand top3 = new MenuCommand("A&nimation");
            MenuCommand top4 = new MenuCommand("&Cities1");
            MenuCommand top5 = new MenuCommand("&Movies1");
            MenuCommand top6 = new MenuCommand("Ca&rs1");
            MenuCommand top7 = new MenuCommand("C&ities2");
            MenuCommand top8 = new MenuCommand("Mo&vies2");
            MenuCommand top9 = new MenuCommand("Car&s2");
            _topMenu.MenuCommands.AddRange(new MenuCommand[]{top1,top2,top3,top4,top5,top6,top7,top8,top9});

            // Create the submenus
            CreateAppearanceMenu(top1);
            CreateWindowsMenu(top2);
            CreateAnimationMenu(top3);
            CreateCityMenus(top4, top7);
            CreateMovieMenus(top5, top8);
            CreateCarMenus(top6, top9);

            // Add to the display
            _topMenu.Dock = DockStyle.Top;
            _topMenu.Selected += new CommandHandler(OnSelected);
            _topMenu.Deselected += new CommandHandler(OnDeselected);
            Controls.Add(_topMenu);

            // Create an initial MDI child window
            OnNewWindowSelected(null, EventArgs.Empty);
        }

        protected void CreateAppearanceMenu(MenuCommand mc)
        {
            // Create menu commands
            MenuCommand style1 = new MenuCommand("&IDE", new EventHandler(OnIDESelected));
            MenuCommand style2 = new MenuCommand("&Plain", new EventHandler(OnPlainSelected));
            MenuCommand style3 = new MenuCommand("-");
            MenuCommand style4 = new MenuCommand("PlainAsBlock", new EventHandler(OnPlainAsBlockSelected));
            MenuCommand style5 = new MenuCommand("-");
            MenuCommand style6 = new MenuCommand("Dock Left", new EventHandler(OnDockLeftSelected));
            MenuCommand style7 = new MenuCommand("Dock Top", new EventHandler(OnDockTopSelected));
            MenuCommand style8 = new MenuCommand("Dock Right", new EventHandler(OnDockRightSelected));
            MenuCommand style9 = new MenuCommand("Dock Bottom", new EventHandler(OnDockBottomSelected));
            MenuCommand styleA = new MenuCommand("-");
            MenuCommand styleB = new MenuCommand("MultiLine", new EventHandler(OnMultiLineSelected));
            MenuCommand styleC = new MenuCommand("-");
            MenuCommand styleD = new MenuCommand("E&xit", new EventHandler(OnExit));

            // Setup event handlers
            style1.Update += new EventHandler(OnIDEUpdate);
            style2.Update += new EventHandler(OnPlainUpdate);
            style4.Update += new EventHandler(OnPlainAsBlockUpdate);
            style6.Update += new EventHandler(OnDockLeftUpdate);
            style7.Update += new EventHandler(OnDockTopUpdate);
            style8.Update += new EventHandler(OnDockRightUpdate);
            style9.Update += new EventHandler(OnDockBottomUpdate);
            styleB.Update += new EventHandler(OnMultiLineUpdate);

            mc.MenuCommands.AddRange(new MenuCommand[]{style1,style2,style3,style4,style5,style6,
                                                       style7,style8,style9,styleA,styleB,styleC,styleD});
			
        }

        protected void CreateWindowsMenu(MenuCommand mc)
        {
            // Create menu commands
            MenuCommand window1 = new MenuCommand("&New Window", _images, 0, new EventHandler(OnNewWindowSelected));
            MenuCommand window2 = new MenuCommand("Cl&ose", _images, 1, new EventHandler(OnCloseWindowSelected));
            MenuCommand window3 = new MenuCommand("Close A&ll", new EventHandler(OnCloseAllSelected));
            MenuCommand window4 = new MenuCommand("-");
            MenuCommand window5 = new MenuCommand("Ne&xt", _images, 2, new EventHandler(OnNextSelected));
            MenuCommand window6 = new MenuCommand("Pre&vious", _images, 3, new EventHandler(OnPreviousSelected));
            MenuCommand window7 = new MenuCommand("-");
            MenuCommand window8 = new MenuCommand("&Cascade", _images, 4, new EventHandler(OnCascadeSelected));
            MenuCommand window9 = new MenuCommand("Tile &Horizontally", _images, 5, new EventHandler(OnTileHSelected));
            MenuCommand windowA = new MenuCommand("&Tile Vertically", _images, 6, new EventHandler(OnTileVSelected));

            window1.Shortcut = Shortcut.Ctrl0;

            // Setup event handlers
            window2.Update += new EventHandler(OnCloseWindowUpdate);
            window2.Update += new EventHandler(OnCloseAllUpdate);
            window5.Update += new EventHandler(OnNextPreviousUpdate);
            window6.Update += new EventHandler(OnNextPreviousUpdate);
            window8.Update += new EventHandler(OnLayoutUpdate);
            window9.Update += new EventHandler(OnLayoutUpdate);
            windowA.Update += new EventHandler(OnLayoutUpdate);
						
            mc.MenuCommands.AddRange(new MenuCommand[]{window1,window2,window3,window4,
                                                       window5,window6,window7,window8,
                                                       window9,windowA});

            // Want to know when MenuControl shows/hide PopupMenu
            mc.PopupStart += new CommandHandler(OnWindowMenuStart);
            mc.PopupEnd += new CommandHandler(OnWindowMenuEnd);
        }

        protected void CreateAnimationMenu(MenuCommand mc)
        {
            // Create menu commands
            MenuCommand animate1 = new MenuCommand("Yes - Always animate", new EventHandler(OnYesAnimateSelected));
            MenuCommand animate2 = new MenuCommand("No  - Never animate", new EventHandler(OnNoAnimateSelected));
            MenuCommand animate3 = new MenuCommand("System - Ask O/S", new EventHandler(OnSystemAnimateSelected));
            MenuCommand animate4 = new MenuCommand("-");
            MenuCommand animate5 = new MenuCommand("100ms", new EventHandler(On100Selected));
            MenuCommand animate6 = new MenuCommand("250ms", new EventHandler(On250Selected));
            MenuCommand animate7 = new MenuCommand("1000ms", new EventHandler(On1000Selected));
            MenuCommand animate8 = new MenuCommand("-");
            MenuCommand animate9 = new MenuCommand("Blend", new EventHandler(OnBlendSelected));
            MenuCommand animateA = new MenuCommand("Center", new EventHandler(OnCenterSelected));
            MenuCommand animateB = new MenuCommand("+Hor +Ver", new EventHandler(OnPPSelected));
            MenuCommand animateC = new MenuCommand("-Hor -Ver", new EventHandler(OnNNSelected));
            MenuCommand animateD = new MenuCommand("+Hor -Ver", new EventHandler(OnPNSelected));
            MenuCommand animateE = new MenuCommand("-Hor +Ver", new EventHandler(OnNPSelected));
            MenuCommand animateF = new MenuCommand("System", new EventHandler(OnSystemSelected));
			
            // Setup event handlers
            animate1.Update += new EventHandler(OnYesAnimateUpdate);
            animate2.Update += new EventHandler(OnNoAnimateUpdate);
            animate3.Update += new EventHandler(OnSystemAnimateUpdate);
            animate5.Update += new EventHandler(On100Update);
            animate6.Update += new EventHandler(On250Update);
            animate7.Update += new EventHandler(On1000Update);
            animate9.Update += new EventHandler(OnBlendUpdate);
            animateA.Update += new EventHandler(OnCenterUpdate);
            animateB.Update += new EventHandler(OnPPUpdate);
            animateC.Update += new EventHandler(OnNNUpdate);
            animateD.Update += new EventHandler(OnPNUpdate);
            animateE.Update += new EventHandler(OnNPUpdate);
            animateF.Update += new EventHandler(OnSystemUpdate);
						
            mc.MenuCommands.AddRange(new MenuCommand[]{animate1,animate2,animate3,animate4,
                                                       animate5,animate6,animate7,animate8,
                                                       animate9,animateA,animateB,animateC,
                                                       animateD,animateE,animateF});
        }

        protected void CreateCarMenus(MenuCommand mc1, MenuCommand mc2)
        {
            // Create menu commands
            MenuCommand car1 = new MenuCommand("Ford", _images, 0);
            MenuCommand car2 = new MenuCommand("Vauxhall", _images, 1);
            MenuCommand car3 = new MenuCommand("Opel", _images, 2);
            MenuCommand car4 = new MenuCommand("Volvo", _images, 5);
            MenuCommand car5 = new MenuCommand("Lotus", _images, 6, Shortcut.Alt0);
            MenuCommand car6 = new MenuCommand("Aston Martin", _images, 0, Shortcut.ShiftF1);
            MenuCommand car7 = new MenuCommand("Ferrari", _images, 1, Shortcut.CtrlShift0);
            MenuCommand car8 = new MenuCommand("Jaguar", _images, 2, Shortcut.ShiftIns);

            // Change default properties of some items
            car2.Enabled = false;
            car3.Enabled = false;
            car4.Break = true;
            car6.Infrequent = true;
            car5.Infrequent = true;

            mc1.MenuCommands.AddRange(new MenuCommand[]{car1,car2,car3,car4,car5,car6,car7,car8});
            mc2.MenuCommands.AddRange(new MenuCommand[]{car1,car2,car3,car4,car5,car6,car7,car8});
        }

        protected void CreateCityMenus(MenuCommand mc1, MenuCommand mc2)
        {
            // Create menu commands
            MenuCommand s0 = new MenuCommand("&Italy", _images, 0, new EventHandler(OnGenericSelect));
            MenuCommand s1 = new MenuCommand("&Spain", _images, 1, new EventHandler(OnGenericSelect));
            MenuCommand s2 = new MenuCommand("&Canada", _images, 2, new EventHandler(OnGenericSelect));
            MenuCommand s3 = new MenuCommand("&France", _images, 3, new EventHandler(OnGenericSelect));
            MenuCommand s4 = new MenuCommand("&Belgium", _images, 4, new EventHandler(OnGenericSelect));
            MenuCommand spain0 = new MenuCommand("&Nerja", _images, 5, new EventHandler(OnGenericSelect));
            MenuCommand spain1 = new MenuCommand("&Madrid", _images, 6, new EventHandler(OnGenericSelect));
            MenuCommand spain2 = new MenuCommand("&Barcelona", _images, 0, new EventHandler(OnGenericSelect));
            MenuCommand canada0 = new MenuCommand("Toronto", _images, 5, new EventHandler(OnGenericSelect));
            MenuCommand canada1 = new MenuCommand("&Montreal", _images, 6, new EventHandler(OnGenericSelect));
            MenuCommand canada2 = new MenuCommand("&Belleville", _images, 0, new EventHandler(OnGenericSelect));
            MenuCommand england = new MenuCommand("England", _images, 2, new EventHandler(OnGenericSelect));
            MenuCommand england1 = new MenuCommand("London", _images, 5, new EventHandler(OnGenericSelect));
            MenuCommand england2 = new MenuCommand("&Birmingham", _images, 6, new EventHandler(OnGenericSelect));
            MenuCommand england3 = new MenuCommand("&Nottingham", _images, 0, new EventHandler(OnGenericSelect));

            // Define hierarchy
            england.MenuCommands.AddRange(new MenuCommand[]{england1,england2,england3});
            s1.MenuCommands.AddRange(new MenuCommand[]{spain0, spain1, spain2});
            s2.MenuCommands.AddRange(new MenuCommand[]{canada0, canada1, canada2, england});
            mc1.MenuCommands.AddRange(new MenuCommand[]{s0, s1, s2, s3, s4});
            mc2.MenuCommands.AddRange(new MenuCommand[]{s0, s1, s2, s3, s4});
            
            // Change default properties of some items
            spain0.Infrequent = true;
            spain1.Infrequent = true;
			
            // Setup the left column details
            england.MenuCommands.ExtraText = "English";
            england.MenuCommands.ExtraTextColor = Color.White;
            england.MenuCommands.ExtraBackColor = Color.DarkBlue;
            england.MenuCommands.ExtraFont = new Font("Times New Roman", 12f, FontStyle.Bold | FontStyle.Italic);
            s1.MenuCommands.ExtraText = "Spanish";
            s1.MenuCommands.ExtraTextColor = Color.DarkRed;
            s1.MenuCommands.ExtraBackColor = Color.Orange;
            s1.MenuCommands.ExtraFont = new Font("Times New Roman", 12f, FontStyle.Bold | FontStyle.Italic);
            s2.MenuCommands.ExtraText = "Canadian";
            s2.MenuCommands.ExtraTextColor = Color.White;
            s2.MenuCommands.ExtraBackColor = Color.DarkRed;
            s2.MenuCommands.ExtraFont = new Font("Times New Roman", 12f, FontStyle.Bold | FontStyle.Italic);
            mc1.MenuCommands.ExtraText = "Countries";
            mc1.MenuCommands.ExtraTextColor = Color.White;
            mc1.MenuCommands.ExtraBackColor = Color.SlateGray;
            mc1.MenuCommands.ExtraFont = new Font("Times New Roman", 12f, FontStyle.Bold | FontStyle.Italic);            
        }
        
        protected void CreateMovieMenus(MenuCommand mc1, MenuCommand mc2)
        {
            // Create menu commands
            MenuCommand movie0 = new MenuCommand("Dr No", _images, 0, new EventHandler(OnGenericSelect));
            MenuCommand movie1 = new MenuCommand("Goldfinger", _images, 1, new EventHandler(OnGenericSelect));
            MenuCommand movie2 = new MenuCommand("Goldeneye", _images, 2, new EventHandler(OnGenericSelect));
            MenuCommand movie3 = new MenuCommand("-");
            MenuCommand movie4 = new MenuCommand("Live and Let Die", _images, 3, new EventHandler(OnGenericSelect));
            MenuCommand movie5 = new MenuCommand("Man with the Golden Gun", _images, 4, new EventHandler(OnGenericSelect));
            MenuCommand movie6 = new MenuCommand("License Revoked", _images, 5, new EventHandler(OnGenericSelect));
            MenuCommand movie7 = new MenuCommand("Diamonds are Forever", _images, 6, new EventHandler(OnGenericSelect));
            MenuCommand movie8 = new MenuCommand("From Russia with Love", _images, 0, new EventHandler(OnGenericSelect));

            // Change default properties of some items
            movie0.Infrequent = true;
            movie1.Infrequent = true;
            movie5.Infrequent = true;
            movie7.Infrequent = true;
            movie8.Infrequent = true;

            mc1.MenuCommands.AddRange(new MenuCommand[]{movie0, movie1, movie2, movie3, movie4, movie5, movie6, movie7, movie8});
            mc2.MenuCommands.AddRange(new MenuCommand[]{movie0, movie1, movie2, movie3, movie4, movie5, movie6, movie7, movie8});
			
            // Setup the left column details
            mc1.MenuCommands.ExtraText = "Bond Films";
            mc1.MenuCommands.ExtraFont = new Font("Garamond", 12f, FontStyle.Bold);
            mc1.MenuCommands.ExtraBackBrush = new LinearGradientBrush(new Point(0,0), new Point(100,100), 
                                                                      Color.LightGreen, Color.DarkGreen);
        }

        protected void SetupStatusBar()
        {
            // Create and setup the StatusBar object
            _status = new StatusBar();
            _status.Dock = DockStyle.Bottom;
            _status.ShowPanels = true;

            // Create and setup a single panel for the StatusBar
            _statusBarPanel = new StatusBarPanel();
            _statusBarPanel.AutoSize = StatusBarPanelAutoSize.Spring;
            _status.Panels.Add(_statusBarPanel);

            Controls.Add(_status);
        }

        public void SetStatusBarText(string text)
        {
            _statusBarPanel.Text = text;
        }

        public ImageList Images
        {
            get { return _images; }
        }

        public VisualStyle Style
        {
            get { return _topMenu.Style; }
        }

        protected void OnSelected(MenuCommand mc)
        {
            SetStatusBarText("Selection over " + mc.Description);
        }

        protected void OnDeselected(MenuCommand mc)
        {
            SetStatusBarText("");
        }

		protected void OnMenuItemSelected(string name)
		{
			MDIChild child = this.ActiveMdiChild as MDIChild;

			if (child != null)
				child.AppendText(name);
		}

        protected void OnGenericSelect(object sender, EventArgs e)
        {
			MenuCommand mc = sender as MenuCommand;
			OnMenuItemSelected(mc.Text);
        }

        protected void OnIDEUpdate(object sender, EventArgs e)
        {
            MenuCommand mc = sender as MenuCommand;
            mc.Checked = (_topMenu.Style == VisualStyle.IDE);
        }

        protected void OnIDESelected(object sender, EventArgs e)
        {
            _topMenu.Style = VisualStyle.IDE;
			OnMenuItemSelected("IDE");
        }

        protected void OnPlainUpdate(object sender, EventArgs e)
        {
            MenuCommand mc = sender as MenuCommand;
            mc.Checked = (_topMenu.Style == VisualStyle.Plain);
        }

        protected void OnPlainSelected(object sender, EventArgs e)
        {
            _topMenu.Style = VisualStyle.Plain;
			OnMenuItemSelected("Plain");
        }

        protected void OnPlainAsBlockUpdate(object sender, EventArgs e)
        {
            MenuCommand mc = sender as MenuCommand;
            mc.Checked = _topMenu.PlainAsBlock;
        }

        protected void OnPlainAsBlockSelected(object sender, EventArgs e)
        {
            _topMenu.PlainAsBlock = !_topMenu.PlainAsBlock;
			OnMenuItemSelected("PlainAsBlock");
        }

        protected void OnMultiLineUpdate(object sender, EventArgs e)
        {
            MenuCommand mc = sender as MenuCommand;
            mc.Checked = _topMenu.MultiLine;
        }

        protected void OnMultiLineSelected(object sender, EventArgs e)
        {
            _topMenu.MultiLine = !_topMenu.MultiLine;
			OnMenuItemSelected("MultiLine");
        }

        protected void OnDockLeftSelected(object sender, EventArgs e)
        {
            _topMenu.Dock = DockStyle.Left;
			OnMenuItemSelected("DockLeft");
        }

        protected void OnDockLeftUpdate(object sender, EventArgs e)
        {
            MenuCommand mc = sender as MenuCommand;
            mc.Checked = (_topMenu.Dock == DockStyle.Left);
        }

        protected void OnDockTopSelected(object sender, EventArgs e)
        {
            _topMenu.Dock = DockStyle.Top;
			OnMenuItemSelected("DockTop");
        }

        protected void OnDockTopUpdate(object sender, EventArgs e)
        {
            MenuCommand mc = sender as MenuCommand;
            mc.Checked = (_topMenu.Dock == DockStyle.Top);
        }

        protected void OnDockRightSelected(object sender, EventArgs e)
        {
            _topMenu.Dock = DockStyle.Right;
			OnMenuItemSelected("DockRight");
        }

        protected void OnDockRightUpdate(object sender, EventArgs e)
        {
            MenuCommand mc = sender as MenuCommand;
            mc.Checked = (_topMenu.Dock == DockStyle.Right);
        }

        protected void OnDockBottomSelected(object sender, EventArgs e)
        {
            _topMenu.Dock = DockStyle.Bottom;
			OnMenuItemSelected("DockBottom");
        }

        protected void OnDockBottomUpdate(object sender, EventArgs e)
        {
            MenuCommand mc = sender as MenuCommand;
            mc.Checked = (_topMenu.Dock == DockStyle.Bottom);
        }

        protected void OnExit(object sender, EventArgs e)
        {
			Close();
        }

        protected void OnNewWindowSelected(object sender, EventArgs e)
        {
            MDIChild child = new MDIChild(this);
			
            child.MdiParent = this;
            child.Size = new Size(130,130);
			child.Text = "Child" + _count++;
            child.Show();

			OnMenuItemSelected("NewWindow");
        }

        protected void OnCloseWindowUpdate(object sender, EventArgs e)
        {
            MenuCommand mc = sender as MenuCommand;
            mc.Enabled = (this.ActiveMdiChild != null);
        }

        protected void OnCloseWindowSelected(object sender, EventArgs e)
        {
            // Close just the curren mdi child window
            this.ActiveMdiChild.Close();
			OnMenuItemSelected("CloseWindow");
        }

        protected void OnCloseAllUpdate(object sender, EventArgs e)
        {
            MenuCommand mc = sender as MenuCommand;
            mc.Enabled = (this.ActiveMdiChild != null);
        }

        protected void OnCloseAllSelected(object sender, EventArgs e)
        {
            MenuCommand mdiCommand = sender as MenuCommand;

			foreach(MDIChild child in Controls)
				child.Close();

			OnMenuItemSelected("CloseAll");
        }

        protected void OnNextSelected(object sender, EventArgs e)
        {
            Form current = this.ActiveMdiChild;
			
            if (current != null)
            {
                // Get collectiom of Mdi child windows
                Form[] children = this.MdiChildren;

                // Find position of the window after the current one
                int newPos = Array.LastIndexOf(children, current) + 1;

                // Check for moving off the end of list, wrap back to start
                if (newPos == children.Length)
                    newPos = 0;

                children[newPos].Activate();
            }

			OnMenuItemSelected("Next");
        }

        protected void OnPreviousSelected(object sender, EventArgs e)
        {
            Form current = this.ActiveMdiChild;
			
            if (current != null)
            {
                // Get collectiom of Mdi child windows
                Form[] children = this.MdiChildren;

                // Find position of the window after the current one
                int newPos = Array.LastIndexOf(children, current) - 1;

                // Check for moving off the start of list, wrap back to end
                if (newPos < 0)
                    newPos = children.Length - 1;

                children[newPos].Activate();
            }

			OnMenuItemSelected("Previous");
        }

        protected void OnNextPreviousUpdate(object sender, EventArgs e)
        {
            MenuCommand mc = sender as MenuCommand;
            mc.Enabled = (this.MdiChildren.Length > 1);
        }

        protected void OnCascadeSelected(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.Cascade);
			OnMenuItemSelected("Cascade");
        }

        protected void OnTileHSelected(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileHorizontal);
			OnMenuItemSelected("TileH");
        }

        protected void OnTileVSelected(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileVertical);
			OnMenuItemSelected("TileV");
        }

        protected void OnLayoutUpdate(object sender, EventArgs e)
        {
            MenuCommand mc = sender as MenuCommand;
            mc.Enabled = (this.MdiChildren.Length > 0);
        }

		protected void OnWindowMenuStart(MenuCommand mc)
		{
			Form current = this.ActiveMdiChild;
			
            // Get collectiom of Mdi child windows
            Form[] children = this.MdiChildren;

			if (children.Length > 0)
			{
				// Add a separator to the menu
				mc.MenuCommands.Add(new MenuCommand("-"));

				foreach(Form f in children)
				{
					MenuCommand newMC = new MenuCommand(f.Text);

					// Is this the currently selected child?
					newMC.Checked = (current == f);
					newMC.Click += new EventHandler(OnChildSelect);

					// Add a command for this active MDI Child
					mc.MenuCommands.Add(newMC);
				}
			}
		}

		protected void OnWindowMenuEnd(MenuCommand mc)
		{
			int count = mc.MenuCommands.Count;

			// Did the OnTopMenuStart add any entries?
			if (count >= 10)
			{
				// Remove all the extras
				for(int index = 10; index < count; index++)
					mc.MenuCommands.RemoveAt(10);
			}
		}

        protected void OnChildSelect(object sender, EventArgs e)
        {
            MenuCommand childCommand = sender as MenuCommand;

			// Get name of the window to activate
			string name = childCommand.Text;

            // Get collectiom of Mdi child windows
            Form[] children = this.MdiChildren;

			foreach(Form f in children)
			{
				// Aha...found it
				if (f.Text == name)
				{
					f.Activate();
					break;
				}
			}

			OnMenuItemSelected("ChildSelected");
		}

        protected void OnYesAnimateSelected(object sender, EventArgs e)
        {
            _topMenu.Animate = Animate.Yes;
            OnMenuItemSelected("Yes - Animate");
        }

        protected void OnYesAnimateUpdate(object sender, EventArgs e)
        {
            MenuCommand mc = sender as MenuCommand;
            mc.Checked = (_topMenu.Animate == Animate.Yes);
        }
        
        protected void OnNoAnimateSelected(object sender, EventArgs e)
        {
            _topMenu.Animate = Animate.No;
            OnMenuItemSelected("No - Animate");
        }

        protected void OnNoAnimateUpdate(object sender, EventArgs e)
        {
            MenuCommand mc = sender as MenuCommand;
            mc.Checked = (_topMenu.Animate == Animate.No);
        }

        protected void OnSystemAnimateSelected(object sender, EventArgs e)
        {
            _topMenu.Animate = Animate.System;
            OnMenuItemSelected("System - Animate");
        }

        protected void OnSystemAnimateUpdate(object sender, EventArgs e)
        {
            MenuCommand mc = sender as MenuCommand;
            mc.Checked = (_topMenu.Animate == Animate.System);
        }

        protected void On100Selected(object sender, EventArgs e)
        {
            _topMenu.AnimateTime = 100;
            OnMenuItemSelected("100ms - AnimateTime");
        }

        protected void On100Update(object sender, EventArgs e)
        {
            MenuCommand mc = sender as MenuCommand;
            mc.Checked = (_topMenu.AnimateTime == 100);
        }

        protected void On250Selected(object sender, EventArgs e)
        {
            _topMenu.AnimateTime = 250;
            OnMenuItemSelected("250ms - AnimateTime");
        }

        protected void On250Update(object sender, EventArgs e)
        {
            MenuCommand mc = sender as MenuCommand;
            mc.Checked = (_topMenu.AnimateTime == 250);
        }

        protected void On1000Selected(object sender, EventArgs e)
        {
            _topMenu.AnimateTime = 1000;
            OnMenuItemSelected("1000ms - AnimateTime");
        }

        protected void On1000Update(object sender, EventArgs e)
        {
            MenuCommand mc = sender as MenuCommand;
            mc.Checked = (_topMenu.AnimateTime == 1000);
        }

        protected void OnBlendSelected(object sender, EventArgs e)
        {
            _topMenu.AnimateStyle = Animation.Blend;
            OnMenuItemSelected("Blend - Animation");
        }

        protected void OnBlendUpdate(object sender, EventArgs e)
        {
            MenuCommand mc = sender as MenuCommand;
            mc.Checked = (_topMenu.AnimateStyle == Animation.Blend);
        }

        protected void OnCenterSelected(object sender, EventArgs e)
        {
            _topMenu.AnimateStyle = Animation.SlideCenter;
            OnMenuItemSelected("Center - Animation");
        }

        protected void OnCenterUpdate(object sender, EventArgs e)
        {
            MenuCommand mc = sender as MenuCommand;
            mc.Checked = (_topMenu.AnimateStyle == Animation.SlideCenter);
        }

        protected void OnPPSelected(object sender, EventArgs e)
        {
            _topMenu.AnimateStyle = Animation.SlideHorVerPositive;
            OnMenuItemSelected("+Hor +Ver - Animation");
        }

        protected void OnPPUpdate(object sender, EventArgs e)
        {
            MenuCommand mc = sender as MenuCommand;
            mc.Checked = (_topMenu.AnimateStyle == Animation.SlideHorVerPositive);
        }

        protected void OnNNSelected(object sender, EventArgs e)
        {
            _topMenu.AnimateStyle = Animation.SlideHorVerNegative;
            OnMenuItemSelected("-Hor -Ver - Animation");
        }

        protected void OnNNUpdate(object sender, EventArgs e)
        {
            MenuCommand mc = sender as MenuCommand;
            mc.Checked = (_topMenu.AnimateStyle == Animation.SlideHorVerNegative);
        }

        protected void OnPNSelected(object sender, EventArgs e)
        {
            _topMenu.AnimateStyle = Animation.SlideHorPosVerNegative;
            OnMenuItemSelected("+Hor -Ver - Animation");
        }

        protected void OnPNUpdate(object sender, EventArgs e)
        {
            MenuCommand mc = sender as MenuCommand;
            mc.Checked = (_topMenu.AnimateStyle == Animation.SlideHorPosVerNegative);
        }

        protected void OnNPSelected(object sender, EventArgs e)
        {
            _topMenu.AnimateStyle = Animation.SlideHorNegVerPositive;
            OnMenuItemSelected("-Hor +Ver - Animation");
        }

        protected void OnNPUpdate(object sender, EventArgs e)
        {
            MenuCommand mc = sender as MenuCommand;
            mc.Checked = (_topMenu.AnimateStyle == Animation.SlideHorNegVerPositive);
        }

        protected void OnSystemSelected(object sender, EventArgs e)
        {
            _topMenu.AnimateStyle = Animation.System;
            OnMenuItemSelected("System - Animation");
        }

        protected void OnSystemUpdate(object sender, EventArgs e)
        {
            MenuCommand mc = sender as MenuCommand;
            mc.Checked = (_topMenu.AnimateStyle == Animation.System);
        }

        protected override void Dispose( bool disposing )
        {
            if( disposing )
            {
                if (components != null) 
                    components.Dispose();
            }
            base.Dispose( disposing );
        }

		#region Windows Form Designer generated code
        private void InitializeComponent()
        {
            // 
            // MDIContainer
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ClientSize = new System.Drawing.Size(250, 270);
            this.Name = "MDIContainer";
            this.Text = "SampleMenus";
            this.IsMdiContainer = true;
        }
		#endregion

        [STAThread]
        static void Main() 
        {
            Application.Run(new MDIContainer());
        }
    }
    
    class MDIChild : Form
    {
        protected MDIContainer _mdiContainer;
		protected RichTextBox _box;

        public MDIChild(MDIContainer mdiContainer)
        {
            // Remember parent Form
            _mdiContainer = mdiContainer;

            // Create a RichTextBox to fill entire client area
            _box = new RichTextBox();
            _box.Text = "Right click inside this window to show a Popup menu.";
            _box.Dock = DockStyle.Fill;
            _box.BorderStyle = BorderStyle.None;
            _box.MouseUp += new MouseEventHandler(OnRichTextMouseUp);
            Controls.Add(_box);
        }

		public void AppendText(string text)
		{
			_box.Text = _box.Text + "\n" + text;
		}

        protected void OnRichTextMouseUp(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Right)
            {
                RichTextBox box = sender as RichTextBox;

	            MenuCommand s0 = new MenuCommand("Italy", _mdiContainer.Images, 0);
                MenuCommand s1 = new MenuCommand("Spain", _mdiContainer.Images, 1);
                MenuCommand s2 = new MenuCommand("Canada", _mdiContainer.Images, 2);
                MenuCommand s3 = new MenuCommand("France", _mdiContainer.Images, 3);
                MenuCommand s4 = new MenuCommand("Belgium", _mdiContainer.Images, 4);
                MenuCommand spain0 = new MenuCommand("Nerja", _mdiContainer.Images, 5);
                MenuCommand spain1 = new MenuCommand("Madrid", _mdiContainer.Images, 6);
                MenuCommand spain2 = new MenuCommand("Barcelona", _mdiContainer.Images, 0);
                MenuCommand canada0 = new MenuCommand("Toronto", _mdiContainer.Images, 5);
                MenuCommand canada1 = new MenuCommand("Montreal", _mdiContainer.Images, 6);
                MenuCommand canada2 = new MenuCommand("Belleville", _mdiContainer.Images, 0);
                MenuCommand england = new MenuCommand("England", _mdiContainer.Images, 2);
                MenuCommand england1 = new MenuCommand("London", _mdiContainer.Images, 5);
                MenuCommand england2 = new MenuCommand("Birmingham", _mdiContainer.Images, 6);
                MenuCommand england3 = new MenuCommand("Nottingham", _mdiContainer.Images, 0);
                england.MenuCommands.AddRange(new MenuCommand[]{england1,england2,england3});
                s1.MenuCommands.AddRange(new MenuCommand[]{spain0, spain1, spain2});
                s2.MenuCommands.AddRange(new MenuCommand[]{canada0, canada1, canada2, england});

                // Create the popup menu object
                PopupMenu popup = new PopupMenu();

                // Define the list of menu commands
                popup.MenuCommands.AddRange(new MenuCommand[]{s0, s1, s2, s3, s4});

                // Define the properties to get appearance to match MenuControl
                popup.Style = _mdiContainer.Style;

                popup.Selected += new CommandHandler(OnSelected);
                popup.Deselected += new CommandHandler(OnDeselected);

                // Show it!
                popup.TrackPopup(box.PointToScreen(new Point(e.X, e.Y)));
            }
        }

        protected void OnSelected(MenuCommand mc)
        {
            _mdiContainer.SetStatusBarText("Selection over " + mc.Description);
        }

        protected void OnDeselected(MenuCommand mc)
        {
            _mdiContainer.SetStatusBarText("");
        }
    }
}