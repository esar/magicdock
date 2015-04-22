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
using System.Xml;
using System.Data;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Crownwood.Magic.Common;
using Crownwood.Magic.Controls;
using Crownwood.Magic.Docking;
using Crownwood.Magic.Menus;

public class SampleDocking : Form
{
    protected byte[] _slot1;
    protected byte[] _slot2;
    protected int _count = 0;
    protected int _ignoreClose = 0;
    protected int _colorIndex = 0;
    protected bool _allowContextMenu = true;
    protected bool _customContextMenu = false;
	protected bool _tabsBottom = true;
	protected bool _captionBars = true;
    protected bool _closeButtons = true;
    protected VisualStyle _style;
    protected MenuCommand _placeHolder;
    protected DockingManager _manager;
    protected ImageList _internalImages;
    protected StatusBar _statusBar;
    protected Crownwood.Magic.Menus.MenuControl _topMenu;
    protected Crownwood.Magic.Controls.TabControl _filler;
    protected Crownwood.Magic.Controls.TabControl.VisualAppearance _tabAppearance = Crownwood.Magic.Controls.TabControl.VisualAppearance.MultiForm;

    [STAThread]
    public static void Main()
    {
        Application.Run(new SampleDocking());
    }

    public SampleDocking()
    {
        // Discover which docking manager to use
        if (DialogResult.Yes == MessageBox.Show("Press 'Yes' to select IDE appearance\n\n" +
                                                "Press 'No' to select Plain appearance\n",
                                                "Select Visual Style", MessageBoxButtons.YesNo))
            _style = VisualStyle.IDE;
        else
            _style = VisualStyle.Plain;
               
        InitializeComponent();

        // Create a strip of images by loading an embedded bitmap resource
        _internalImages = ResourceHelper.LoadBitmapStrip(this.GetType(),
                                                         "SampleDocking.SampleImages.bmp",
                                                         new Size(16,16),
                                                         new Point(0,0));
		
        BorderStyle bs = (_style == VisualStyle.Plain) ? BorderStyle.None : BorderStyle.FixedSingle;

        RichTextBox rtb1 = new RichTextBox();
        RichTextBox rtb2 = new RichTextBox();
        RichTextBox rtb3 = new RichTextBox();
        
        rtb1.BorderStyle = bs;
        rtb2.BorderStyle = bs;
        rtb3.BorderStyle = bs;
		
        _filler = new Crownwood.Magic.Controls.TabControl();
		_filler.TabPages.Add(new Crownwood.Magic.Controls.TabPage("First", rtb1));
		_filler.TabPages.Add(new Crownwood.Magic.Controls.TabPage("Second", rtb2));
		_filler.TabPages.Add(new Crownwood.Magic.Controls.TabPage("Third", rtb3));
		_filler.Appearance = Crownwood.Magic.Controls.TabControl.VisualAppearance.MultiDocument;
		_filler.Dock = DockStyle.Fill;
		_filler.Style = _style;
		_filler.IDEPixelBorder = true;
        Controls.Add(_filler);
 
        // Reduce the amount of flicker that occurs when windows are redocked within
        // the container. As this prevents unsightly backcolors being drawn in the
        // WM_ERASEBACKGROUND that seems to occur.
        SetStyle(ControlStyles.DoubleBuffer, true);
        SetStyle(ControlStyles.AllPaintingInWmPaint, true);

        // Create the object that manages the docking state
        _manager = new DockingManager(this, _style);

        // Notifications
        _manager.ContextMenu += new DockingManager.ContextMenuHandler(OnContextMenu);
        _manager.ContentHiding += new DockingManager.ContentHidingHandler(OnContentHiding);
		_manager.TabControlCreated += new DockingManager.TabControlCreatedHandler(OnTabControlCreated);

        // Ensure that the RichTextBox is always the innermost control
        _manager.InnerControl = _filler;

        // Create and setup the StatusBar object
        _statusBar = new StatusBar();
        _statusBar.Dock = DockStyle.Bottom;
        _statusBar.ShowPanels = true;

        // Create and setup a single panel for the StatusBar
        StatusBarPanel statusBarPanel = new StatusBarPanel();
        statusBarPanel.AutoSize = StatusBarPanelAutoSize.Spring;
        _statusBar.Panels.Add(statusBarPanel);
        Controls.Add(_statusBar);

        _topMenu = CreateMenus();

        // Ensure that docking occurs after the menu control and status bar controls
        _manager.OuterControl = _statusBar;
        
        // Setup custom config handling
        _manager.SaveCustomConfig += new DockingManager.SaveCustomConfigHandler(OnSaveConfig);
        _manager.LoadCustomConfig += new DockingManager.LoadCustomConfigHandler(OnLoadConfig);
    }	

    protected Crownwood.Magic.Menus.MenuControl CreateMenus()
    {
        Crownwood.Magic.Menus.MenuControl topMenu = new Crownwood.Magic.Menus.MenuControl();
        topMenu.Style = _style;
        topMenu.MultiLine = false;
		
        MenuCommand topManager = new MenuCommand("Manager");
        MenuCommand topConfig = new MenuCommand("Config");
        MenuCommand topSettings = new MenuCommand("Settings");
        MenuCommand topColors = new MenuCommand("Colors");
        MenuCommand topTabControls = new MenuCommand("TabControls");
        topMenu.MenuCommands.AddRange(new MenuCommand[]{topManager, topConfig, topSettings,topColors,topTabControls});

        // Manager 
        MenuCommand managerC1 = new MenuCommand("Create Form", new EventHandler(OnCreateC1));
        MenuCommand managerC2 = new MenuCommand("Create Panel", new EventHandler(OnCreateC2));
        MenuCommand managerC3 = new MenuCommand("Create RichTextBox", new EventHandler(OnCreateC3));
        MenuCommand managerSep1 = new MenuCommand("-");
        MenuCommand managerC31 = new MenuCommand("Create 3 in Row", new EventHandler(OnCreateC31));
        MenuCommand managerC32 = new MenuCommand("Create 3 in Column", new EventHandler(OnCreateC32));
        MenuCommand managerC33 = new MenuCommand("Create 3 in Window", new EventHandler(OnCreateC33));
		MenuCommand managerC34 = new MenuCommand("Create 3 in Window (autohide)", new EventHandler(OnCreateC34));
		MenuCommand managerSep2 = new MenuCommand("-");
        MenuCommand managerFF = new MenuCommand("Create 3 in Floating Form", new EventHandler(OnCreateFF));
        MenuCommand managerSep3 = new MenuCommand("-");
        MenuCommand managerClear = new MenuCommand("Delete Contents", new EventHandler(OnDeleteAll));
        MenuCommand managerSep4 = new MenuCommand("-");
        MenuCommand managerShowAll = new MenuCommand("Show All", new EventHandler(OnShowAll));
        MenuCommand managerHideAll = new MenuCommand("Hide All", new EventHandler(OnHideAll));
        MenuCommand managerSep5 = new MenuCommand("-");
        MenuCommand managerInsideFill = new MenuCommand("Inside Fill", new EventHandler(OnInsideFill));
        MenuCommand managerSep6 = new MenuCommand("-");
        MenuCommand managerExit = new MenuCommand("Exit", new EventHandler(OnExit));

        managerInsideFill.Update += new EventHandler(OnUpdateInsideFill);

        topManager.MenuCommands.AddRange(new MenuCommand[]{managerC1,managerC2,managerC3,managerSep1,
                                                           managerC31,managerC32,managerC33,managerC34,managerSep2,
                                                           managerFF,managerSep3,
                                                           managerClear,managerSep4,managerShowAll,
                                                           managerHideAll,managerSep5,managerInsideFill,
                                                           managerSep5,managerExit});

        // Config
        MenuCommand configSF1 = new MenuCommand("Save as Config1.xml", new EventHandler(OnSaveFile1));
        MenuCommand configSF2 = new MenuCommand("Save as Config2.xml", new EventHandler(OnSaveFile2));
        MenuCommand configSep1 = new MenuCommand("-");
        MenuCommand configLF1 = new MenuCommand("Load from Config1.xml", new EventHandler(OnLoadFile1));
        MenuCommand configLF2 = new MenuCommand("Load from Config2.xml", new EventHandler(OnLoadFile2));
        MenuCommand configSep2 = new MenuCommand("-");
        MenuCommand configSA1 = new MenuCommand("Save to byte array1", new EventHandler(OnSaveArray1));
        MenuCommand configSA2 = new MenuCommand("Save to byte array2", new EventHandler(OnSaveArray2));
        MenuCommand configSep3 = new MenuCommand("-");
        MenuCommand configLA1 = new MenuCommand("Load from byte array1", new EventHandler(OnLoadArray1));
        MenuCommand configLA2 = new MenuCommand("Load from byte array2", new EventHandler(OnLoadArray2));
        topConfig.MenuCommands.AddRange(new MenuCommand[]{configSF1,configSF2,configSep1,
                                                          configLF1,configLF2,configSep2,
                                                          configSA1,configSA2,configSep3,
                                                          configLA1,configLA2});

        // Settings
        MenuCommand settingsShow = new MenuCommand("Allow Context Menu", new EventHandler(OnContextMenuAllow));
        MenuCommand settingsCustom = new MenuCommand("Customize Context Menu", new EventHandler(OnContextMenuCustomize));
        MenuCommand settingsSep1 = new MenuCommand("-");
        MenuCommand settingsRSBD = new MenuCommand("Default ResizeBarVector", new EventHandler(OnResizeBarDefault));
        MenuCommand settingsRSB1 = new MenuCommand("1 Pixel ResizeBarVector", new EventHandler(OnResizeBar1));
        MenuCommand settingsRSB5 = new MenuCommand("5 Pixel ResizeBarVector", new EventHandler(OnResizeBar5));
        MenuCommand settingsRSB7 = new MenuCommand("7 Pixel ResizeBarVector", new EventHandler(OnResizeBar7));
        MenuCommand settingsSep2 = new MenuCommand("-");
        MenuCommand settingsCaptionBars = new MenuCommand("Show all caption bars", new EventHandler(OnCaptionBars));
        MenuCommand settingsCloseButtons = new MenuCommand("Show all close buttons", new EventHandler(OnCloseButtons));
        MenuCommand settingsSep3 = new MenuCommand("-");
        MenuCommand settingsAllow = new MenuCommand("Allow all close buttons", new EventHandler(OnAllowAllCloseButton));
        MenuCommand settingsIgnoreAll = new MenuCommand("Ignore all close buttons", new EventHandler(OnIgnoreAllCloseButton));
        MenuCommand settingsIgnorePanel = new MenuCommand("Ignore all Panel close buttons", new EventHandler(OnIgnoreAllPanelButton));
        MenuCommand settingsIgnoreForm = new MenuCommand("Ignore all Form close buttons", new EventHandler(OnIgnoreAllFormButton));
        MenuCommand settingsIgnoreTextBox = new MenuCommand("Ignore all RichTextBox close buttons", new EventHandler(OnIgnoreAllTextboxButton));
        MenuCommand settingsSep4 = new MenuCommand("-");
        MenuCommand settingsAllowMinMax = new MenuCommand("Enable Min/Max in Columns/Rows", new EventHandler(OnAllowMinMax));
        MenuCommand settingsSep5 = new MenuCommand("-");
        MenuCommand settingsPlainTabBorder = new MenuCommand("Show Plain Tab Border", new EventHandler(OnPlainTabBorder));

        settingsShow.Update += new EventHandler(OnUpdateAllow);
        settingsCustom.Update += new EventHandler(OnUpdateCustomize);
        settingsRSBD.Update += new EventHandler(OnUpdateRSBD);
        settingsRSB1.Update += new EventHandler(OnUpdateRSB1);
        settingsRSB5.Update += new EventHandler(OnUpdateRSB5);
        settingsRSB7.Update += new EventHandler(OnUpdateRSB7);
        settingsCaptionBars.Update += new EventHandler(OnUpdateCaptionBars);
        settingsCloseButtons.Update += new EventHandler(OnUpdateCloseButtons);
        settingsAllow.Update += new EventHandler(OnUpdateAllowAll);
        settingsIgnoreAll.Update += new EventHandler(OnUpdateIgnoreAll);
        settingsIgnorePanel.Update += new EventHandler(OnUpdateIgnorePanel);
        settingsIgnoreForm.Update += new EventHandler(OnUpdateIgnoreForm);
        settingsIgnoreTextBox.Update += new EventHandler(OnUpdateIgnoreTextBox);
        settingsAllowMinMax.Update += new EventHandler(OnUpdateAllowMinMax);
        settingsPlainTabBorder.Update += new EventHandler(OnUpdatePlainTabBorder);

        topSettings.MenuCommands.AddRange(new MenuCommand[]{settingsShow,settingsCustom,settingsSep1,settingsRSBD,
                                                            settingsRSB1,settingsRSB5,settingsRSB7,settingsSep2,
                                                            settingsCaptionBars,settingsCloseButtons,settingsSep3,
                                                            settingsAllow,settingsIgnoreAll,settingsIgnorePanel,
                                                            settingsIgnoreForm,settingsIgnoreTextBox,settingsSep4,
                                                            settingsAllowMinMax,settingsSep5,settingsPlainTabBorder});
 

        // Colors
        MenuCommand colorDefault = new MenuCommand("System Default", new EventHandler(OnColorDefault));
        MenuCommand colorSlateBlue = new MenuCommand("Custom - Slate Blue", new EventHandler(OnColorSlateBlue));
        MenuCommand colorFirebrick = new MenuCommand("Custom - Firebrick", new EventHandler(OnColorFirebrick));
        MenuCommand colorGreen = new MenuCommand("Custom - Green", new EventHandler(OnColorGreen));

        colorDefault.Update += new EventHandler(OnUpdateDefault);
        colorSlateBlue.Update += new EventHandler(OnUpdateSlateBlue);
        colorFirebrick.Update += new EventHandler(OnUpdateFirebrick);
        colorGreen.Update += new EventHandler(OnUpdateGreen);

        topColors.MenuCommands.AddRange(new MenuCommand[]{colorDefault,colorSlateBlue,colorFirebrick,colorGreen});
 

        // TabControls
        MenuCommand tabsBox = new MenuCommand("MultiBox", new EventHandler(OnTabsMultiBox));
        MenuCommand tabsForm = new MenuCommand("MultiForm", new EventHandler(OnTabsMultiForm));
        MenuCommand tabsDocument = new MenuCommand("MultiDocument", new EventHandler(OnTabsMultiDocument));
        MenuCommand tabsSep1 = new MenuCommand("-");
        MenuCommand tabsTop = new MenuCommand("Tabs at top", new EventHandler(OnTabsTop));
        MenuCommand tabsBottom = new MenuCommand("Tabs at bottom", new EventHandler(OnTabsBottom));

        tabsBox.Update += new EventHandler(OnUpdateTabsBox);
        tabsForm.Update += new EventHandler(OnUpdateTabsForm);
        tabsDocument.Update += new EventHandler(OnUpdateTabsDocument);
        tabsTop.Update += new EventHandler(OnUpdateTabsTop);
        tabsBottom.Update += new EventHandler(OnUpdateTabsBottom);

        topTabControls.MenuCommands.AddRange(new MenuCommand[]{tabsBox,tabsForm,tabsDocument,
															   tabsSep1,tabsTop,tabsBottom});

        topMenu.Dock = DockStyle.Top;
        Controls.Add(topMenu);

        return topMenu;
    }

    protected void OnContextMenu(PopupMenu pm, CancelEventArgs cea)
    {
        // Show the PopupMenu be cancelled and not shown?
        if (!_allowContextMenu)
            cea.Cancel = true;
        else
        {        
            if (_customContextMenu)
            {
                // Remove the Show All and Hide All commands
                pm.MenuCommands.Remove(pm.MenuCommands["Show All"]);
                pm.MenuCommands.Remove(pm.MenuCommands["Hide All"]);
                
                // Add a custom item at the start
                pm.MenuCommands.Insert(0, (new MenuCommand("Custom 1")));
                pm.MenuCommands.Insert(1, (new MenuCommand("-")));
                
                // Add a couple of custom commands at the end
                pm.MenuCommands.Add(new MenuCommand("Custom 2"));
                pm.MenuCommands.Add(new MenuCommand("Custom 3"));
            }
        }
    }
    
    protected void OnContentHiding(Content c, CancelEventArgs cea)
    {
        switch(_ignoreClose)
        {
            case 0:
                // Allow all, do nothing
                break;
            case 1:
                // Ignore all, cancel
                cea.Cancel = true;
                break;
            case 2:
                // Ignore Panels
                cea.Cancel = c.Control is Panel;
                break;
            case 3:
                // Ignore Forms
                cea.Cancel = c.Control is Form;
                break;
            case 4:
                // Ignore RichTextBox
                cea.Cancel = c.Control is RichTextBox;
                break;
        }
    }

	protected void OnTabControlCreated(Crownwood.Magic.Controls.TabControl tabControl)
	{	
		tabControl.PositionTop = !_tabsBottom;
		tabControl.Appearance = _tabAppearance;
	}

    protected void OnContextMenuAllow(object sender, EventArgs e)
    {
        // Toggle the display of the docking windows context menu
        _allowContextMenu = (_allowContextMenu == false);
    }
    
    protected void OnUpdateAllow(object sender, EventArgs e)
    {
        MenuCommand allowCommand = sender as MenuCommand;
        allowCommand.Checked = _allowContextMenu;
    }

    protected void OnContextMenuCustomize(object sender, EventArgs e)
    {
        // Toggle the customization of the displayed context menu
        _customContextMenu = (_customContextMenu == false);
    }

    protected void OnUpdateCustomize(object sender, EventArgs e)
    {
        MenuCommand customizeCommand = sender as MenuCommand;
        customizeCommand.Checked = _customContextMenu;
    }

    protected void OnInsideFill(object sender, EventArgs e)
    {
        if (_manager.InsideFill)
        {
            Controls.Add(_filler);
            Controls.SetChildIndex(_filler, 0);
            _manager.InnerControl = _filler;
            _manager.InsideFill = false;
        }
        else
        {
            Controls.Remove(_filler);
            _manager.InnerControl = null;
            _manager.InsideFill = true;
        } 
    }

    protected void OnUpdateInsideFill(object sender, EventArgs e)
    {
        MenuCommand fillCommand = sender as MenuCommand;
        fillCommand.Checked = _manager.InsideFill;
    }

    protected void OnCreateC1(object sender, EventArgs e)
    {
        // Create Content which contains a RichTextBox
        Content c = _manager.Contents.Add(new DummyForm(), "Form " + _count++, _internalImages, _count % 6);
        
        // Setup initial state to match menu selections
        DefineContentState(c);
        
        // Setup the correct starting colors to match the menu selections
        DefineControlColors(c);
    
        // Request a new Docking window be created for the above Content on the right edge
        _manager.AddContentWithState(c, State.DockRight);
    }

    protected void OnCreateC2(object sender, EventArgs e)
    {
        // Create Content which contains a RichTextBox
        Content c = _manager.Contents.Add(new DummyPanel(), "Panel " + _count++, _internalImages, _count % 6);
        
        // Setup initial state to match menu selections
        DefineContentState(c);

        // Setup the correct starting colors to match the menu selections
        DefineControlColors(c);

        // Request a new Docking window be created for the above Content on the bottom edge
        _manager.AddContentWithState(c, State.DockBottom);
    }
    
    protected void OnCreateC3(object sender, EventArgs e)
    {
        // Create Content which contains a RichTextBox
        Content c = _manager.Contents.Add(new RichTextBox(), "RichTextBox " + _count++, _internalImages, _count % 6);
        
        c.Control.Disposed += new EventHandler(OnRTBDisposed);
        
        // Setup initial state to match menu selections
        DefineContentState(c);

        // Setup the correct starting colors to match the menu selections
        DefineControlColors(c);

        // Request a new Docking window be created for the above Content on the left edge
        _manager.AddContentWithState(c, State.DockLeft);
    }

    protected void OnCreateC31(object sender, EventArgs e)
    {
        // Create three Content objects, one of each type
        Content cA = _manager.Contents.Add(new DummyForm(), "Form " + _count++, _internalImages, _count % 6);
        Content cB = _manager.Contents.Add(new DummyPanel(), "Panel " + _count++, _internalImages, _count % 6);
        Content cC = _manager.Contents.Add(new RichTextBox(), "RichTextBox " + _count++, _internalImages, _count % 6);
        
        // Setup initial state to match menu selections
        DefineContentState(cA);
        DefineContentState(cB);
        DefineContentState(cC);

        // Setup the correct starting colors to match the menu selections
        DefineControlColors(cA);
        DefineControlColors(cB);
        DefineControlColors(cC);

        // Request a new Docking window be created for the first content on the bottom edge
        WindowContent wc = _manager.AddContentWithState(cA, State.DockBottom) as WindowContent;
        
        // Add two other content into the same Zone
        _manager.AddContentToZone(cB, wc.ParentZone, 1);
        _manager.AddContentToZone(cC, wc.ParentZone, 2);
    }

    protected void OnCreateC32(object sender, EventArgs e)
    {
        // Create three Content objects, one of each type
        Content cA = _manager.Contents.Add(new DummyForm(), "Form " + _count++, _internalImages, _count % 6);
        Content cB = _manager.Contents.Add(new DummyPanel(), "Panel " + _count++, _internalImages, _count % 6);
        Content cC = _manager.Contents.Add(new RichTextBox(), "RichTextBox " + _count++, _internalImages, _count % 6);
        
        // Setup initial state to match menu selections
        DefineContentState(cA);
        DefineContentState(cB);
        DefineContentState(cC);

        // Setup the correct starting colors to match the menu selections
        DefineControlColors(cA);
        DefineControlColors(cB);
        DefineControlColors(cC);

        // Request a new Docking window be created for the first content on the left edge
        WindowContent wc = _manager.AddContentWithState(cA, State.DockLeft) as WindowContent;
        
        // Add two other content into the same Zone
        _manager.AddContentToZone(cB, wc.ParentZone, 1);
        _manager.AddContentToZone(cC, wc.ParentZone, 2);
    }

    protected void OnCreateC33(object sender, EventArgs e)
    {
        // Create three Content objects, one of each type
        Content cA = _manager.Contents.Add(new DummyForm(), "Form " + _count++, _internalImages, _count % 6);
        Content cB = _manager.Contents.Add(new DummyPanel(), "Panel " + _count++, _internalImages, _count % 6);
        Content cC = _manager.Contents.Add(new RichTextBox(), "RichTextBox " + _count++, _internalImages, _count % 6);

        // Setup initial state to match menu selections
        DefineContentState(cA);
        DefineContentState(cB);
        DefineContentState(cC);

        // Setup the correct starting colors to match the menu selections
        DefineControlColors(cA);
        DefineControlColors(cB);
        DefineControlColors(cC);

        // Request a new Docking window be created for the first content on the bottom edge
        WindowContent wc = _manager.AddContentWithState(cA, State.DockBottom) as WindowContent;
        
        // Add two other content into the same Window
        _manager.AddContentToWindowContent(cB, wc);
        _manager.AddContentToWindowContent(cC, wc);
    }
    
	protected void OnCreateC34(object sender, EventArgs e)
	{
		// Create three Content objects, one of each type
		Content cA = _manager.Contents.Add(new DummyForm(), "Form " + _count++, _internalImages, _count % 6);
		Content cB = _manager.Contents.Add(new DummyPanel(), "Panel " + _count++, _internalImages, _count % 6);
		Content cC = _manager.Contents.Add(new RichTextBox(), "RichTextBox " + _count++, _internalImages, _count % 6);

		// Setup initial state to match menu selections
		DefineContentState(cA);
		DefineContentState(cB);
		DefineContentState(cC);

		// Setup the correct starting colors to match the menu selections
		DefineControlColors(cA);
		DefineControlColors(cB);
		DefineControlColors(cC);

		// Prevent flicker where the contents are added and display and then a fraction of a 
		// second later they become auto hidden. By suspending and then resuming the layout this
		// small flicker can be avoided.
		this.SuspendLayout();

		// Request a new Docking window be created for the first content on the bottom edge
		WindowContent wc = _manager.AddContentWithState(cA, State.DockRight) as WindowContent;
        
		// Add two other content into the same Window
		_manager.AddContentToWindowContent(cB, wc);
		_manager.AddContentToWindowContent(cC, wc);

		// Move all contents in the same window as cA into autohide mode
		_manager.ToggleContentAutoHide(cA);

		this.ResumeLayout();
	}
	
	protected void OnCreateFF(object sender, EventArgs e)
    {
        // Create three Content objects, one of each type
        Content cA = _manager.Contents.Add(new DummyForm(), "Form " + _count++, _internalImages, _count % 6);
        Content cB = _manager.Contents.Add(new DummyPanel(), "Panel " + _count++, _internalImages, _count % 6);
        Content cC = _manager.Contents.Add(new RichTextBox(), "RichTextBox " + _count++, _internalImages, _count % 6);

        // Define the initial floating form size
        cA.FloatingSize = new Size(250,450);

        // Setup initial state to match menu selections
        DefineContentState(cA);
        DefineContentState(cB);
        DefineContentState(cC);

        // Setup the correct starting colors to match the menu selections
        DefineControlColors(cA);
        DefineControlColors(cB);
        DefineControlColors(cC);

        // Request a new Docking window be created for the first content on the bottom edge
        WindowContent wc = _manager.AddContentWithState(cA, State.Floating) as WindowContent;
        
        // Add second content into the same Window
        _manager.AddContentToWindowContent(cB, wc);
        
        // Add third into same Zone
        _manager.AddContentToZone(cC, wc.ParentZone, 1);
    }
        
    protected void DefineContentState(Content c)
    {
        c.CaptionBar = _captionBars;
        c.CloseButton = _closeButtons;
    }
    
    protected void DefineControlColors(Content c)
    {
        // Only interested in Forms and Panels
        if ((c.Control is Form) || (c.Control is Panel))
        {
            // Starting color depends on select menu option
            switch(_colorIndex)
            {
                case 0:
                    c.Control.BackColor = SystemColors.Control;
                    c.Control.ForeColor = SystemColors.ControlText;
                    break;
                case 1:
                    c.Control.BackColor = Color.DarkSlateBlue;
                    c.Control.ForeColor = Color.White;
                    break;
                case 2:
                    c.Control.BackColor = Color.Firebrick;
                    c.Control.ForeColor = Color.White;
                    break;
                case 3:
                    c.Control.BackColor = Color.PaleGreen;
                    c.Control.ForeColor = Color.Black;
                    break;
            }
        }
    }

    protected void OnDeleteAll(object sender, EventArgs e)
    {
        _manager.Contents.Clear();
    }

    protected void OnShowAll(object sender, EventArgs e)
    {	
        _manager.ShowAllContents();
    }

    protected void OnHideAll(object sender, EventArgs e)
    {	
        _manager.HideAllContents();
    }

    protected void OnExit(object sender, EventArgs e)
    {	
        Close();
    }

    protected void OnSaveFile1(object sender, EventArgs e)
    {
        _manager.SaveConfigToFile("config1.xml");
    }

    protected void OnSaveFile2(object sender, EventArgs e)
    {
        _manager.SaveConfigToFile("config2.xml");
    }

    protected void OnLoadFile1(object sender, EventArgs e)
    {
        _manager.LoadConfigFromFile("config1.xml");
    }

    protected void OnLoadFile2(object sender, EventArgs e)
    {
        _manager.LoadConfigFromFile("config2.xml");
    }

    protected void OnSaveArray1(object sender, EventArgs e)
    {
        _slot1 = _manager.SaveConfigToArray();
    }

    protected void OnSaveArray2(object sender, EventArgs e)
    {
        _slot2 = _manager.SaveConfigToArray();
    }

    protected void OnLoadArray1(object sender, EventArgs e)
    {
        if (_slot1 != null)
            _manager.LoadConfigFromArray(_slot1);
    }

    protected void OnLoadArray2(object sender, EventArgs e)
    {
        if (_slot2 != null)
            _manager.LoadConfigFromArray(_slot2);
    }

    protected void OnResizeBarDefault(object sender, EventArgs e)
    {
        _manager.ResizeBarVector = -1;
    }

    protected void OnUpdateRSBD(object sender, EventArgs e)
    {
        MenuCommand updateCommand = sender as MenuCommand;
        updateCommand.Checked = (_manager.ResizeBarVector == -1);
    }

    protected void OnResizeBar1(object sender, EventArgs e)
    {
        _manager.ResizeBarVector = 1;
    }

    protected void OnUpdateRSB1(object sender, EventArgs e)
    {
        MenuCommand updateCommand = sender as MenuCommand;
        updateCommand.Checked = (_manager.ResizeBarVector == 1);
    }

    protected void OnResizeBar5(object sender, EventArgs e)
    {
        _manager.ResizeBarVector = 5;
    }

    protected void OnUpdateRSB5(object sender, EventArgs e)
    {
        MenuCommand updateCommand = sender as MenuCommand;
        updateCommand.Checked = (_manager.ResizeBarVector == 5);
    }

    protected void OnResizeBar7(object sender, EventArgs e)
    {
        _manager.ResizeBarVector = 7;
    }

    protected void OnUpdateRSB7(object sender, EventArgs e)
    {
        MenuCommand updateCommand = sender as MenuCommand;
        updateCommand.Checked = (_manager.ResizeBarVector == 7);
    }
    
    protected void OnCaptionBars(object sender, EventArgs e)
    {
        _captionBars = (_captionBars == false);
        
        foreach(Content c in _manager.Contents)
            c.CaptionBar = _captionBars;
    }

    protected void OnUpdateCaptionBars(object sender, EventArgs e)
    {
        MenuCommand updateCommand = sender as MenuCommand;
        updateCommand.Checked = _captionBars;
    }
    
    protected void OnCloseButtons(object sender, EventArgs e)
    {
        _closeButtons = (_closeButtons == false);
        
        foreach(Content c in _manager.Contents)
            c.CloseButton = _closeButtons;
    }

    protected void OnUpdateCloseButtons(object sender, EventArgs e)
    {
        MenuCommand updateCommand = sender as MenuCommand;
        updateCommand.Checked = _closeButtons;
    }

    protected void OnAllowAllCloseButton(object sender, EventArgs e)
    {
        _ignoreClose = 0;
    }

    protected void OnUpdateAllowAll(object sender, EventArgs e)
    {
        MenuCommand updateCommand = sender as MenuCommand;
        updateCommand.Checked = (_ignoreClose == 0);
    }

    protected void OnIgnoreAllCloseButton(object sender, EventArgs e)
    {
        _ignoreClose = 1;
    }

    protected void OnUpdateIgnoreAll(object sender, EventArgs e)
    {
        MenuCommand updateCommand = sender as MenuCommand;
        updateCommand.Checked = (_ignoreClose == 1);
    }

    protected void OnIgnoreAllPanelButton(object sender, EventArgs e)
    {
        _ignoreClose = 2;
    }

    protected void OnUpdateIgnorePanel(object sender, EventArgs e)
    {
        MenuCommand updateCommand = sender as MenuCommand;
        updateCommand.Checked = (_ignoreClose == 2);
    }

    protected void OnIgnoreAllFormButton(object sender, EventArgs e)
    {
        _ignoreClose = 3;
    }

    protected void OnUpdateIgnoreForm(object sender, EventArgs e)
    {
        MenuCommand updateCommand = sender as MenuCommand;
        updateCommand.Checked = (_ignoreClose == 3);
    }

    protected void OnIgnoreAllTextboxButton(object sender, EventArgs e)
    {
        _ignoreClose = 4;
    }

    protected void OnUpdateIgnoreTextBox(object sender, EventArgs e)
    {
        MenuCommand updateCommand = sender as MenuCommand;
        updateCommand.Checked = (_ignoreClose == 4);
    }
    
    protected void OnAllowMinMax(object sender, EventArgs e)
    {
        _manager.ZoneMinMax = (_manager.ZoneMinMax == false);
    }

    protected void OnUpdateAllowMinMax(object sender, EventArgs e)
    {
        MenuCommand updateCommand = sender as MenuCommand;
        updateCommand.Checked = _manager.ZoneMinMax;
    }

    protected void OnPlainTabBorder(object sender, EventArgs e)
    {
        _manager.PlainTabBorder = (_manager.PlainTabBorder == false);
    }

    protected void OnUpdatePlainTabBorder(object sender, EventArgs e)
    {
        MenuCommand updateCommand = sender as MenuCommand;
        updateCommand.Checked = _manager.PlainTabBorder;
    }

    protected void OnColorDefault(object sender, EventArgs e)
    {
        // Remember the new color scheme selected
        _colorIndex = 0;

        // Define main Form back color
        _filler.TextColor = SystemColors.MenuText;
        _filler.BackColor = SystemColors.Control;
        this.BackColor = SystemColors.Control;
        
        // Define the menu/status bars
        _topMenu.ResetColors();
        _topMenu.Font = SystemInformation.MenuFont;
        _filler.Font = SystemInformation.MenuFont;

        _manager.ResetColors();
        _manager.CaptionFont = SystemInformation.MenuFont;
        _manager.TabControlFont = SystemInformation.MenuFont;

        // Need to manually update the colors of created Panels/Forms
        foreach(Content c in _manager.Contents)
            DefineControlColors(c);
    }

    protected void OnUpdateDefault(object sender, EventArgs e)
    {
        MenuCommand updateCommand = sender as MenuCommand;
        updateCommand.Checked = (_colorIndex == 0);
    }

    protected void OnColorSlateBlue(object sender, EventArgs e)
    {
        // Remember the new color scheme selected
        _colorIndex = 1;

        // Define main Form back color
        this.BackColor = Color.DarkSlateBlue;

        // Define the menu/status bars
        _topMenu.TextColor = Color.White;
        _topMenu.BackColor = Color.DarkSlateBlue;
        _topMenu.SelectedTextColor = Color.White;
		_topMenu.SelectedBackColor = Color.SteelBlue;
        _topMenu.Font = new Font("Garamond", 10f);

        // Define the TabControl appearance
        _filler.TextColor = Color.White;
        _filler.BackColor = Color.DarkSlateBlue;
        _filler.Font = new Font("Garamond", 10f);

		if (_style == VisualStyle.IDE)
		{
			_topMenu.HighlightTextColor = Color.Black;
			_topMenu.HighlightBackColor = Color.SlateGray;
		}
		else
		{
			_topMenu.HighlightTextColor = Color.White;
			_topMenu.HighlightBackColor = Color.DarkSlateBlue;
		}

        // Define docking window colors
        _manager.BackColor = Color.DarkSlateBlue;
        _manager.InactiveTextColor = Color.White;
        _manager.ActiveColor = Color.SteelBlue;
        _manager.ActiveTextColor = Color.White;
        _manager.ResizeBarColor = Color.DarkSlateBlue;
        _manager.CaptionFont = new Font("Garamond", 10f);
        _manager.TabControlFont = new Font("Garamond", 10f);

        // Need to manually update the colors of created Panels/Forms
        foreach(Content c in _manager.Contents)
            DefineControlColors(c);
    }

    protected void OnUpdateSlateBlue(object sender, EventArgs e)
    {
        MenuCommand updateCommand = sender as MenuCommand;
        updateCommand.Checked = (_colorIndex == 1);
    }

    protected void OnColorFirebrick(object sender, EventArgs e)
    {
        // Remember the new color scheme selected
        _colorIndex = 2;

        this.BackColor = Color.Firebrick;

        // Define the menu/status bars
        _topMenu.TextColor = Color.White;
        _topMenu.BackColor = Color.Firebrick;
	    _topMenu.HighlightTextColor = Color.White;
	    _topMenu.HighlightBackColor = Color.Firebrick;
        _topMenu.SelectedBackColor = Color.Orange;
        _topMenu.Font = new Font("Sans Serif", 12f);

        // Define the TabControl appearance
        _filler.TextColor = Color.White;
        _filler.BackColor = Color.Firebrick;
        _filler.Font = new Font("Sans Serif", 12f);

		if (_style == VisualStyle.IDE)
	        _topMenu.SelectedTextColor = Color.Black;
		else
	        _topMenu.SelectedTextColor = Color.White;

        // Define docking window colors
        _manager.BackColor = Color.Firebrick;
        _manager.InactiveTextColor = Color.White;
        _manager.ActiveColor = Color.Orange;
        _manager.ActiveTextColor = Color.Black;
        _manager.ResizeBarColor = Color.Firebrick;
        _manager.CaptionFont = new Font("Sans Serif", 12f);
        _manager.TabControlFont = new Font("Sans Serif", 10f);
        
        // Need to manually update the colors of created Panels/Forms
        foreach(Content c in _manager.Contents)
            DefineControlColors(c);
    }

    protected void OnUpdateFirebrick(object sender, EventArgs e)
    {
        MenuCommand updateCommand = sender as MenuCommand;
        updateCommand.Checked = (_colorIndex == 2);
    }
    
    protected void OnColorGreen(object sender, EventArgs e)
    {
        // Remember the new color scheme selected
        _colorIndex = 3;

        // Define main Form back color
        this.BackColor = Color.PaleGreen;

        // Define the menu/status bars
        _topMenu.TextColor = Color.Black;
        _topMenu.BackColor = Color.PaleGreen;
        _topMenu.SelectedBackColor = Color.DarkSlateGray;
        _topMenu.Font = new Font("Arial", 9f);

        // Define the TabControl appearance
        _filler.TextColor = Color.Black;
        _filler.BackColor = Color.PaleGreen;
        _filler.Font = new Font("Arial", 9f);
        
        if (_style == VisualStyle.IDE)
		{
			_topMenu.HighlightTextColor = Color.Black;
		    _topMenu.HighlightBackColor = Color.PaleGreen;
	        _topMenu.SelectedTextColor = Color.White;
		}
		else
		{
		    _topMenu.HighlightTextColor = Color.White;
		    _topMenu.HighlightBackColor = Color.DarkSlateGray;
		    _topMenu.SelectedBackColor = Color.PaleGreen;
		}

        // Define docking window colors
        _manager.BackColor = Color.PaleGreen;
        _manager.InactiveTextColor = Color.Black;
        _manager.ActiveColor = Color.DarkSlateGray;
        _manager.ActiveTextColor = Color.White;
        _manager.ResizeBarColor = Color.DarkSeaGreen;
        _manager.CaptionFont = new Font("Arial", 9f);
        _manager.TabControlFont = new Font("Arial", 9f);

        // Need to manually update the colors of created Panels/Forms
        foreach(Content c in _manager.Contents)
            DefineControlColors(c);
    }

    protected void OnUpdateGreen(object sender, EventArgs e)
    {
        MenuCommand updateCommand = sender as MenuCommand;
        updateCommand.Checked = (_colorIndex == 3);
    }

    protected void OnTabsMultiBox(object sender, EventArgs e)
    {
		_tabAppearance = Crownwood.Magic.Controls.TabControl.VisualAppearance.MultiBox;
		UpdateAllTabControls();
    }

    protected void OnUpdateTabsBox(object sender, EventArgs e)
    {
        MenuCommand updateCommand = sender as MenuCommand;
        updateCommand.Checked = (_tabAppearance == Crownwood.Magic.Controls.TabControl.VisualAppearance.MultiBox);
    }

    protected void OnTabsMultiForm(object sender, EventArgs e)
    {
		_tabAppearance = Crownwood.Magic.Controls.TabControl.VisualAppearance.MultiForm;
		UpdateAllTabControls();
    }

    protected void OnUpdateTabsForm(object sender, EventArgs e)
    {
        MenuCommand updateCommand = sender as MenuCommand;
        updateCommand.Checked = (_tabAppearance == Crownwood.Magic.Controls.TabControl.VisualAppearance.MultiForm);
    }

    protected void OnTabsMultiDocument(object sender, EventArgs e)
    {
		_tabAppearance = Crownwood.Magic.Controls.TabControl.VisualAppearance.MultiDocument;
		UpdateAllTabControls();
    }

    protected void OnUpdateTabsDocument(object sender, EventArgs e)
    {
        MenuCommand updateCommand = sender as MenuCommand;
        updateCommand.Checked = (_tabAppearance == Crownwood.Magic.Controls.TabControl.VisualAppearance.MultiDocument);
    }

    protected void OnTabsTop(object sender, EventArgs e)
    {
		_tabsBottom = false;
		UpdateAllTabControls();
    }

    protected void OnUpdateTabsTop(object sender, EventArgs e)
    {
        MenuCommand updateCommand = sender as MenuCommand;
        updateCommand.Checked = !_tabsBottom;
    }

    protected void OnTabsBottom(object sender, EventArgs e)
    {
		_tabsBottom = true;
		UpdateAllTabControls();
    }

	protected void UpdateAllTabControls()
	{
		foreach(Content c in _manager.Contents)
		{
			if (c.ParentWindowContent != null)
			{
				WindowContentTabbed wct = c.ParentWindowContent as WindowContentTabbed;

				if (wct != null)
				{
					wct.TabControl.Appearance = _tabAppearance;
					wct.TabControl.PositionTop = !_tabsBottom;
				}
			}
		}
	}

    protected void OnUpdateTabsBottom(object sender, EventArgs e)
    {
        MenuCommand updateCommand = sender as MenuCommand;
        updateCommand.Checked = _tabsBottom;
    }

    protected void OnRTBDisposed(object sender, EventArgs e)
    {   
        Console.WriteLine("OnRTBDisposed");
    }
    
    protected void OnSaveConfig(XmlTextWriter xmlOut)
    {
        // Add an extra node into the config to store some useless information
        xmlOut.WriteStartElement("MyCustomElement");
        xmlOut.WriteAttributeString("UselessData1", "Hello");
        xmlOut.WriteAttributeString("UselessData2", "World!");
        xmlOut.WriteEndElement();
    }

    protected void OnLoadConfig(XmlTextReader xmlIn)
    {
        // We are expecting our custom element to be the current one
        if (xmlIn.Name == "MyCustomElement")
        {
            // Read in both the expected attributes
            string attr1 = xmlIn.GetAttribute(0);
            string attr2 = xmlIn.GetAttribute(1);
            
            // Must move past our element
            xmlIn.Read();
        }
    }

    private void InitializeComponent()
    {
        this.BackColor = Color.SlateGray;
        this.ClientSize = new System.Drawing.Size(500, 500);
        this.Text = "SampleDocking";
		this.MinimumSize = new Size(250,250);
    }
}

public class DummyForm : Form
{
    private Button _dummy1 = new Button();
    private Button _dummy2 = new Button();
    private GroupBox _dummyBox = new GroupBox();
    private RadioButton _dummy3 = new RadioButton();
    private RadioButton _dummy4 = new RadioButton();

    public DummyForm()
    {
        _dummy1.Text = "Button 1";
        _dummy1.Size = new Size(90,25);
        _dummy1.Location = new Point(10,10);

        _dummy2.Text = "Button 2";
        _dummy2.Size = new Size(90,25);
        _dummy2.Location = new Point(110,10);

        _dummyBox.Text = "Form GroupBox";
        _dummyBox.Size = new Size(125, 67);
        _dummyBox.Location = new Point(10, 45);

        _dummy3.Text = "RadioButton 3";
        _dummy3.Size = new Size(110,22);
        _dummy3.Location = new Point(10, 20);

        _dummy4.Text = "RadioButton 4";
        _dummy4.Size = new Size(110,22);
        _dummy4.Location = new Point(10, 42);
        _dummy4.Checked = true;

        _dummyBox.Controls.AddRange(new Control[]{_dummy3, _dummy4});

        Controls.AddRange(new Control[]{_dummy1, _dummy2, _dummyBox});

        this.Disposed += new EventHandler(OnFormDisposed);

        // Define then control to be selected when activated for first time
        this.ActiveControl = _dummy4;
    }

    protected void OnFormDisposed(object sender, EventArgs e)
    {   
        Console.WriteLine("OnFormDisposed");
    }
}

public class DummyPanel : Panel
{
    private GroupBox _dummyBox = new GroupBox();
    private RadioButton _dummy3 = new RadioButton();
    private RadioButton _dummy4 = new RadioButton();

    public DummyPanel()
    {
        _dummyBox.Text = "Panel GroupBox";
        _dummyBox.Size = new Size(125, 67);
        _dummyBox.Location = new Point(10, 10);

        _dummy3.Text = "RadioButton 3";
        _dummy3.Size = new Size(110,22);
        _dummy3.Location = new Point(10, 20);

        _dummy4.Text = "RadioButton 4";
        _dummy4.Size = new Size(110,22);
        _dummy4.Location = new Point(10, 42);
        _dummy4.Checked = true;

        _dummyBox.Controls.AddRange(new Control[]{_dummy3, _dummy4});

        this.Disposed += new EventHandler(OnPanelDisposed);

        Controls.AddRange(new Control[]{_dummyBox});
    }
    
    protected void OnPanelDisposed(object sender, EventArgs e)
    {   
        Console.WriteLine("OnPanelDisposed");
    }
}
