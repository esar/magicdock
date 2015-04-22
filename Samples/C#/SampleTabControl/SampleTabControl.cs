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
using System.Drawing;
using System.Resources;
using System.Reflection;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Crownwood.Magic.Common;
using Crownwood.Magic.Menus;
using Crownwood.Magic.Controls;
using Crownwood.Magic.Win32;

namespace SampleTabControl
{
    public class SampleTabControl : System.Windows.Forms.Form
    {
        private static int _count = 0;
        
        private PopupMenu _popupMenu;
        private ImageList _internalImages;
        private bool _update = false;
        private Color _startForeColor;
        private Color _startBackColor;
        private Color _startButtonActive;
        private Color _startButtonInactive;
        private Color _startTextInactiveColor;
        private Color _startHotTextColor;
        private string[] _strings = new string[]{"P&roperties", 
                                                 "Solution Explo&rer", 
                                                 "&Task List", 
                                                 "&Command Window", 
                                                 "Callstack", 
                                                 "B&reakpoints", 
                                                 "Output"};

        private System.Windows.Forms.Button addPage;
        private System.Windows.Forms.Button removePage;
        private System.Windows.Forms.Button clearAll;
        private System.Windows.Forms.GroupBox StyleGroup;
        private System.Windows.Forms.GroupBox AppearanceGroup;
        private System.Windows.Forms.GroupBox exampleColors;
        private System.Windows.Forms.CheckBox positionAtTop;
        private System.Windows.Forms.CheckBox hotTrack;
        private System.Windows.Forms.CheckBox shrinkPages;
        private System.Windows.Forms.CheckBox showClose;
        private System.Windows.Forms.CheckBox showArrows;
        private System.Windows.Forms.CheckBox insetPlain;
        private System.Windows.Forms.CheckBox insetPagesOnly;
        private System.Windows.Forms.CheckBox selectedTextOnly;
        private System.Windows.Forms.CheckBox hoverSelect;
        private System.Windows.Forms.CheckBox idePixelBorder;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
        private System.Windows.Forms.NumericUpDown numericUpDown3;
        private System.Windows.Forms.NumericUpDown numericUpDown4;
        private System.Windows.Forms.RadioButton radioIDE;
        private System.Windows.Forms.RadioButton radioPlain;
        private System.Windows.Forms.RadioButton radioMultiBox;
        private System.Windows.Forms.RadioButton radioMultiForm;
        private System.Windows.Forms.RadioButton radioMultiDocument;
        private System.Windows.Forms.RadioButton red;
        private System.Windows.Forms.RadioButton blue;
        private System.Windows.Forms.RadioButton normal;
        private System.Windows.Forms.CheckBox idePixelArea;
        private System.Windows.Forms.CheckBox multiLine;
        private Crownwood.Magic.Controls.TabControl tabControl;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton tabHideUsingLogic;
        private System.Windows.Forms.RadioButton tabHideAlways;
        private System.Windows.Forms.RadioButton tabShowAlways;
        private System.Windows.Forms.RadioButton tabHideWithoutMouse;
        private System.Windows.Forms.CheckBox multilineFullWidth;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public SampleTabControl()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            
            // Create a strip of images by loading an embedded bitmap resource
            _internalImages = ResourceHelper.LoadBitmapStrip(this.GetType(),
                                                             "SampleTabControl.SampleImages.bmp",
                                                             new Size(16,16),
                                                             new Point(0,0));

            _popupMenu = new PopupMenu();
            _popupMenu.MenuCommands.Add(new MenuCommand("Example1", _internalImages, 0));
            _popupMenu.MenuCommands.Add(new MenuCommand("Example2", _internalImages, 1));
            _popupMenu.MenuCommands.Add(new MenuCommand("Example3", _internalImages, 2));
            tabControl.ContextPopupMenu = _popupMenu;

            tabControl.ImageList = _internalImages;
            
            // Hook into the close button
            tabControl.ClosePressed += new EventHandler(OnRemovePage);
	
            // Remember initial colors
            _startForeColor = tabControl.ForeColor;
            _startBackColor = tabControl.BackColor;
            _startButtonActive = tabControl.ButtonActiveColor;
            _startButtonInactive = tabControl.ButtonInactiveColor;
            _startTextInactiveColor = tabControl.TextInactiveColor;
			_startHotTextColor = tabControl.HotTextColor;
            normal.Checked = true;

            UpdateControls();
        }

        protected override void Dispose( bool disposing )
        {
            if( disposing )
            {
                if(components != null)
                    components.Dispose();
            }
            base.Dispose( disposing );
        }

		#region Windows Form Designer generated code
        private void InitializeComponent()
        {
            this.selectedTextOnly = new System.Windows.Forms.CheckBox();
            this.positionAtTop = new System.Windows.Forms.CheckBox();
            this.radioMultiBox = new System.Windows.Forms.RadioButton();
            this.removePage = new System.Windows.Forms.Button();
            this.hotTrack = new System.Windows.Forms.CheckBox();
            this.numericUpDown3 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown4 = new System.Windows.Forms.NumericUpDown();
            this.hoverSelect = new System.Windows.Forms.CheckBox();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.radioMultiForm = new System.Windows.Forms.RadioButton();
            this.showClose = new System.Windows.Forms.CheckBox();
            this.shrinkPages = new System.Windows.Forms.CheckBox();
            this.addPage = new System.Windows.Forms.Button();
            this.clearAll = new System.Windows.Forms.Button();
            this.StyleGroup = new System.Windows.Forms.GroupBox();
            this.radioPlain = new System.Windows.Forms.RadioButton();
            this.radioIDE = new System.Windows.Forms.RadioButton();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.insetPlain = new System.Windows.Forms.CheckBox();
            this.insetPagesOnly = new System.Windows.Forms.CheckBox();
            this.showArrows = new System.Windows.Forms.CheckBox();
            this.radioMultiDocument = new System.Windows.Forms.RadioButton();
            this.AppearanceGroup = new System.Windows.Forms.GroupBox();
            this.exampleColors = new System.Windows.Forms.GroupBox();
            this.red = new System.Windows.Forms.RadioButton();
            this.blue = new System.Windows.Forms.RadioButton();
            this.normal = new System.Windows.Forms.RadioButton();
            this.idePixelBorder = new System.Windows.Forms.CheckBox();
            this.idePixelArea = new System.Windows.Forms.CheckBox();
            this.multiLine = new System.Windows.Forms.CheckBox();
            this.tabControl = new Crownwood.Magic.Controls.TabControl();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tabHideWithoutMouse = new System.Windows.Forms.RadioButton();
            this.tabHideUsingLogic = new System.Windows.Forms.RadioButton();
            this.tabHideAlways = new System.Windows.Forms.RadioButton();
            this.tabShowAlways = new System.Windows.Forms.RadioButton();
            this.multilineFullWidth = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            this.StyleGroup.SuspendLayout();
            this.AppearanceGroup.SuspendLayout();
            this.exampleColors.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // selectedTextOnly
            // 
            this.selectedTextOnly.Location = new System.Drawing.Point(176, 224);
            this.selectedTextOnly.Name = "selectedTextOnly";
            this.selectedTextOnly.Size = new System.Drawing.Size(120, 24);
            this.selectedTextOnly.TabIndex = 1;
            this.selectedTextOnly.Text = "Selected Text Only";
            this.selectedTextOnly.CheckedChanged += new System.EventHandler(this.selectedTextOnly_CheckedChanged);
            // 
            // positionAtTop
            // 
            this.positionAtTop.Location = new System.Drawing.Point(176, 8);
            this.positionAtTop.Name = "positionAtTop";
            this.positionAtTop.TabIndex = 1;
            this.positionAtTop.Text = "Position At Top";
            this.positionAtTop.CheckedChanged += new System.EventHandler(this.positionAtTop_CheckedChanged);
            // 
            // radioMultiBox
            // 
            this.radioMultiBox.Location = new System.Drawing.Point(16, 64);
            this.radioMultiBox.Name = "radioMultiBox";
            this.radioMultiBox.Size = new System.Drawing.Size(88, 24);
            this.radioMultiBox.TabIndex = 0;
            this.radioMultiBox.Text = "MultiBox";
            this.radioMultiBox.Click += new System.EventHandler(this.OnAppearanceMultiBox);
            // 
            // removePage
            // 
            this.removePage.Location = new System.Drawing.Point(424, 376);
            this.removePage.Name = "removePage";
            this.removePage.Size = new System.Drawing.Size(88, 24);
            this.removePage.TabIndex = 0;
            this.removePage.Text = "RemovePage";
            this.removePage.Click += new System.EventHandler(this.OnRemovePage);
            // 
            // hotTrack
            // 
            this.hotTrack.Location = new System.Drawing.Point(176, 32);
            this.hotTrack.Name = "hotTrack";
            this.hotTrack.TabIndex = 1;
            this.hotTrack.Text = "HotTrack";
            this.hotTrack.CheckedChanged += new System.EventHandler(this.Highlight_CheckedChanged);
            // 
            // numericUpDown3
            // 
            this.numericUpDown3.Location = new System.Drawing.Point(384, 336);
            this.numericUpDown3.Name = "numericUpDown3";
            this.numericUpDown3.Size = new System.Drawing.Size(56, 20);
            this.numericUpDown3.TabIndex = 2;
            this.numericUpDown3.ValueChanged += new System.EventHandler(this.numericUpDown3_ValueChanged);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(384, 304);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(56, 20);
            this.numericUpDown1.TabIndex = 2;
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // numericUpDown4
            // 
            this.numericUpDown4.Location = new System.Drawing.Point(544, 336);
            this.numericUpDown4.Name = "numericUpDown4";
            this.numericUpDown4.Size = new System.Drawing.Size(56, 20);
            this.numericUpDown4.TabIndex = 2;
            this.numericUpDown4.ValueChanged += new System.EventHandler(this.numericUpDown4_ValueChanged);
            // 
            // hoverSelect
            // 
            this.hoverSelect.Location = new System.Drawing.Point(176, 248);
            this.hoverSelect.Name = "hoverSelect";
            this.hoverSelect.Size = new System.Drawing.Size(112, 24);
            this.hoverSelect.TabIndex = 1;
            this.hoverSelect.Text = "Hover Select";
            this.hoverSelect.CheckedChanged += new System.EventHandler(this.hoverSelect_CheckedChanged);
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.Location = new System.Drawing.Point(544, 304);
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(56, 20);
            this.numericUpDown2.TabIndex = 2;
            this.numericUpDown2.ValueChanged += new System.EventHandler(this.numericUpDown2_ValueChanged);
            // 
            // radioMultiForm
            // 
            this.radioMultiForm.Location = new System.Drawing.Point(16, 40);
            this.radioMultiForm.Name = "radioMultiForm";
            this.radioMultiForm.Size = new System.Drawing.Size(88, 24);
            this.radioMultiForm.TabIndex = 0;
            this.radioMultiForm.Text = "MultiForm";
            this.radioMultiForm.Click += new System.EventHandler(this.OnAppearanceMultiForm);
            // 
            // showClose
            // 
            this.showClose.Location = new System.Drawing.Point(176, 80);
            this.showClose.Name = "showClose";
            this.showClose.TabIndex = 1;
            this.showClose.Text = "Show Close";
            this.showClose.CheckedChanged += new System.EventHandler(this.showClose_CheckedChanged);
            // 
            // shrinkPages
            // 
            this.shrinkPages.Location = new System.Drawing.Point(176, 56);
            this.shrinkPages.Name = "shrinkPages";
            this.shrinkPages.TabIndex = 1;
            this.shrinkPages.Text = "Shrink pages";
            this.shrinkPages.CheckedChanged += new System.EventHandler(this.shrinkPages_CheckedChanged);
            // 
            // addPage
            // 
            this.addPage.Location = new System.Drawing.Point(312, 376);
            this.addPage.Name = "addPage";
            this.addPage.Size = new System.Drawing.Size(87, 24);
            this.addPage.TabIndex = 0;
            this.addPage.Text = "AddPage";
            this.addPage.Click += new System.EventHandler(this.OnAddPage);
            // 
            // clearAll
            // 
            this.clearAll.Location = new System.Drawing.Point(536, 376);
            this.clearAll.Name = "clearAll";
            this.clearAll.Size = new System.Drawing.Size(88, 24);
            this.clearAll.TabIndex = 0;
            this.clearAll.Text = "RemoveAll";
            this.clearAll.Click += new System.EventHandler(this.OnClearAll);
            // 
            // StyleGroup
            // 
            this.StyleGroup.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                                     this.radioPlain,
                                                                                     this.radioIDE});
            this.StyleGroup.Location = new System.Drawing.Point(8, 8);
            this.StyleGroup.Name = "StyleGroup";
            this.StyleGroup.Size = new System.Drawing.Size(144, 72);
            this.StyleGroup.TabIndex = 0;
            this.StyleGroup.TabStop = false;
            this.StyleGroup.Text = "Style";
            // 
            // radioPlain
            // 
            this.radioPlain.Location = new System.Drawing.Point(16, 40);
            this.radioPlain.Name = "radioPlain";
            this.radioPlain.Size = new System.Drawing.Size(56, 24);
            this.radioPlain.TabIndex = 0;
            this.radioPlain.Text = "Plain";
            this.radioPlain.Click += new System.EventHandler(this.OnStylePlain);
            // 
            // radioIDE
            // 
            this.radioIDE.Location = new System.Drawing.Point(16, 16);
            this.radioIDE.Name = "radioIDE";
            this.radioIDE.Size = new System.Drawing.Size(56, 24);
            this.radioIDE.TabIndex = 0;
            this.radioIDE.Text = "IDE";
            this.radioIDE.Click += new System.EventHandler(this.OnStyleIDE);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(456, 336);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 23);
            this.label4.TabIndex = 3;
            this.label4.Text = "Bottom Offset";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(312, 304);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 23);
            this.label1.TabIndex = 3;
            this.label1.Text = "Left Offset";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(472, 304);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 23);
            this.label2.TabIndex = 3;
            this.label2.Text = "Right Offset";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(312, 336);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 23);
            this.label3.TabIndex = 3;
            this.label3.Text = "Top Offset";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // insetPlain
            // 
            this.insetPlain.Location = new System.Drawing.Point(176, 152);
            this.insetPlain.Name = "insetPlain";
            this.insetPlain.TabIndex = 1;
            this.insetPlain.Text = "Inset Plain";
            this.insetPlain.CheckedChanged += new System.EventHandler(this.insetPlain_CheckedChanged);
            // 
            // insetPagesOnly
            // 
            this.insetPagesOnly.Location = new System.Drawing.Point(176, 128);
            this.insetPagesOnly.Name = "insetPagesOnly";
            this.insetPagesOnly.Size = new System.Drawing.Size(120, 24);
            this.insetPagesOnly.TabIndex = 4;
            this.insetPagesOnly.Text = "Inset Pages Only";
            this.insetPagesOnly.CheckedChanged += new System.EventHandler(this.insetPagesOnly_CheckedChanged);
            // 
            // showArrows
            // 
            this.showArrows.Location = new System.Drawing.Point(176, 104);
            this.showArrows.Name = "showArrows";
            this.showArrows.TabIndex = 1;
            this.showArrows.Text = "Show Arrows";
            this.showArrows.CheckedChanged += new System.EventHandler(this.showArrows_CheckedChanged);
            // 
            // radioMultiDocument
            // 
            this.radioMultiDocument.Location = new System.Drawing.Point(16, 16);
            this.radioMultiDocument.Name = "radioMultiDocument";
            this.radioMultiDocument.TabIndex = 0;
            this.radioMultiDocument.Text = "MultiDocument";
            this.radioMultiDocument.Click += new System.EventHandler(this.OnAppearanceMultiDocument);
            // 
            // AppearanceGroup
            // 
            this.AppearanceGroup.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                                          this.radioMultiBox,
                                                                                          this.radioMultiForm,
                                                                                          this.radioMultiDocument});
            this.AppearanceGroup.Location = new System.Drawing.Point(8, 88);
            this.AppearanceGroup.Name = "AppearanceGroup";
            this.AppearanceGroup.Size = new System.Drawing.Size(144, 96);
            this.AppearanceGroup.TabIndex = 0;
            this.AppearanceGroup.TabStop = false;
            this.AppearanceGroup.Text = "Appearance";
            // 
            // exampleColors
            // 
            this.exampleColors.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                                        this.red,
                                                                                        this.blue,
                                                                                        this.normal});
            this.exampleColors.Location = new System.Drawing.Point(8, 336);
            this.exampleColors.Name = "exampleColors";
            this.exampleColors.Size = new System.Drawing.Size(232, 64);
            this.exampleColors.TabIndex = 1;
            this.exampleColors.TabStop = false;
            this.exampleColors.Text = "Example Colors";
            // 
            // red
            // 
            this.red.Location = new System.Drawing.Point(136, 24);
            this.red.Name = "red";
            this.red.Size = new System.Drawing.Size(88, 24);
            this.red.TabIndex = 0;
            this.red.Text = "Red";
            this.red.CheckedChanged += new System.EventHandler(this.red_CheckedChanged);
            // 
            // blue
            // 
            this.blue.Location = new System.Drawing.Point(80, 24);
            this.blue.Name = "blue";
            this.blue.Size = new System.Drawing.Size(88, 24);
            this.blue.TabIndex = 0;
            this.blue.Text = "Blue";
            this.blue.CheckedChanged += new System.EventHandler(this.blue_CheckedChanged);
            // 
            // normal
            // 
            this.normal.Location = new System.Drawing.Point(16, 24);
            this.normal.Name = "normal";
            this.normal.Size = new System.Drawing.Size(88, 24);
            this.normal.TabIndex = 0;
            this.normal.Text = "Normal";
            this.normal.CheckedChanged += new System.EventHandler(this.normal_CheckedChanged);
            // 
            // idePixelBorder
            // 
            this.idePixelBorder.Location = new System.Drawing.Point(176, 176);
            this.idePixelBorder.Name = "idePixelBorder";
            this.idePixelBorder.Size = new System.Drawing.Size(112, 24);
            this.idePixelBorder.TabIndex = 5;
            this.idePixelBorder.Text = "IDE Pixel Border";
            this.idePixelBorder.CheckedChanged += new System.EventHandler(this.idePixelBorder_CheckedChanged);
            // 
            // idePixelArea
            // 
            this.idePixelArea.Location = new System.Drawing.Point(176, 200);
            this.idePixelArea.Name = "idePixelArea";
            this.idePixelArea.Size = new System.Drawing.Size(112, 24);
            this.idePixelArea.TabIndex = 6;
            this.idePixelArea.Text = "IDE Pixel Area";
            this.idePixelArea.CheckedChanged += new System.EventHandler(this.idePixelArea_CheckedChanged);
            // 
            // multiLine
            // 
            this.multiLine.Location = new System.Drawing.Point(176, 272);
            this.multiLine.Name = "multiLine";
            this.multiLine.TabIndex = 7;
            this.multiLine.Text = "MultiLine";
            this.multiLine.CheckedChanged += new System.EventHandler(this.multiLine_CheckedChanged);
            // 
            // tabControl
            // 
            this.tabControl.HideTabsMode = Crownwood.Magic.Controls.TabControl.HideTabsModes.ShowAlways;
            this.tabControl.Location = new System.Drawing.Point(304, 16);
            this.tabControl.Name = "tabControl";
            this.tabControl.Size = new System.Drawing.Size(320, 272);
            this.tabControl.TabIndex = 8;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                                    this.tabHideWithoutMouse,
                                                                                    this.tabHideUsingLogic,
                                                                                    this.tabHideAlways,
                                                                                    this.tabShowAlways});
            this.groupBox1.Location = new System.Drawing.Point(8, 192);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(144, 120);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "HideTabsMode";
            // 
            // tabHideWithoutMouse
            // 
            this.tabHideWithoutMouse.Location = new System.Drawing.Point(16, 88);
            this.tabHideWithoutMouse.Name = "tabHideWithoutMouse";
            this.tabHideWithoutMouse.Size = new System.Drawing.Size(120, 24);
            this.tabHideWithoutMouse.TabIndex = 1;
            this.tabHideWithoutMouse.Text = "HideWithoutMouse";
            this.tabHideWithoutMouse.CheckedChanged += new System.EventHandler(this.tabHideWithoutMouse_CheckedChanged);
            // 
            // tabHideUsingLogic
            // 
            this.tabHideUsingLogic.Location = new System.Drawing.Point(16, 64);
            this.tabHideUsingLogic.Name = "tabHideUsingLogic";
            this.tabHideUsingLogic.TabIndex = 0;
            this.tabHideUsingLogic.Text = "HideUsingLogic";
            this.tabHideUsingLogic.CheckedChanged += new System.EventHandler(this.tabHideUsingLogic_CheckedChanged);
            // 
            // tabHideAlways
            // 
            this.tabHideAlways.Location = new System.Drawing.Point(16, 40);
            this.tabHideAlways.Name = "tabHideAlways";
            this.tabHideAlways.Size = new System.Drawing.Size(88, 24);
            this.tabHideAlways.TabIndex = 0;
            this.tabHideAlways.Text = "HideAlways";
            this.tabHideAlways.CheckedChanged += new System.EventHandler(this.tabHideAlways_CheckedChanged);
            // 
            // tabShowAlways
            // 
            this.tabShowAlways.Location = new System.Drawing.Point(16, 16);
            this.tabShowAlways.Name = "tabShowAlways";
            this.tabShowAlways.TabIndex = 2;
            // 
            // multilineFullWidth
            // 
            this.multilineFullWidth.Location = new System.Drawing.Point(176, 296);
            this.multilineFullWidth.Name = "multilineFullWidth";
            this.multilineFullWidth.Size = new System.Drawing.Size(136, 24);
            this.multilineFullWidth.TabIndex = 9;
            this.multilineFullWidth.Text = "MultiLine Full Width";
            this.multilineFullWidth.CheckedChanged += new System.EventHandler(this.multilineFullWidth_CheckedChanged);
            // 
            // SampleTabControl
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(648, 413);
            this.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                          this.multilineFullWidth,
                                                                          this.tabControl,
                                                                          this.multiLine,
                                                                          this.idePixelArea,
                                                                          this.idePixelBorder,
                                                                          this.insetPagesOnly,
                                                                          this.hoverSelect,
                                                                          this.selectedTextOnly,
                                                                          this.numericUpDown3,
                                                                          this.label3,
                                                                          this.numericUpDown4,
                                                                          this.label4,
                                                                          this.numericUpDown2,
                                                                          this.label2,
                                                                          this.label1,
                                                                          this.numericUpDown1,
                                                                          this.insetPlain,
                                                                          this.showArrows,
                                                                          this.showClose,
                                                                          this.shrinkPages,
                                                                          this.addPage,
                                                                          this.removePage,
                                                                          this.clearAll,
                                                                          this.hotTrack,
                                                                          this.positionAtTop,
                                                                          this.StyleGroup,
                                                                          this.AppearanceGroup,
                                                                          this.exampleColors,
                                                                          this.groupBox1});
            this.Name = "SampleTabControl";
            this.Text = "SampleTabControl";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            this.StyleGroup.ResumeLayout(false);
            this.AppearanceGroup.ResumeLayout(false);
            this.exampleColors.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }
		#endregion

        protected void UpdateControls()
        {
            switch(tabControl.Appearance)
            {
                case Crownwood.Magic.Controls.TabControl.VisualAppearance.MultiDocument:
                    _update = true;
                    radioMultiDocument.Select();
                    break;
                case Crownwood.Magic.Controls.TabControl.VisualAppearance.MultiForm:
                    _update = true;
                    radioMultiForm.Select();
                    break;
                case Crownwood.Magic.Controls.TabControl.VisualAppearance.MultiBox:
                    _update = true;
                    radioMultiBox.Select();
                    break;
            }

            switch(tabControl.Style)
            {
                case VisualStyle.IDE:
                    _update = true;
                    radioIDE.Select();
                    break;
                case VisualStyle.Plain:
                    _update = true;
                    radioPlain.Select();
                    break;
            }
            
            switch(tabControl.HideTabsMode)
            {
                case Crownwood.Magic.Controls.TabControl.HideTabsModes.ShowAlways:
                    _update = true;
                    tabShowAlways.Checked = true;
                    break;
                case Crownwood.Magic.Controls.TabControl.HideTabsModes.HideAlways:
                    _update = true;
                    tabHideAlways.Checked = true;
                    break;
                case Crownwood.Magic.Controls.TabControl.HideTabsModes.HideUsingLogic:
                    _update = true;
                    tabHideUsingLogic.Checked = true;
                    break;
                case Crownwood.Magic.Controls.TabControl.HideTabsModes.HideWithoutMouse:
                    _update = true;
                    tabHideWithoutMouse.Checked = true;
                    break;
            }

            hotTrack.Checked = tabControl.HotTrack;
            positionAtTop.Checked = tabControl.PositionTop;
            shrinkPages.Checked = tabControl.ShrinkPagesToFit;
            showClose.Checked = tabControl.ShowClose;
            showArrows.Checked = tabControl.ShowArrows;
            insetPlain.Checked = tabControl.InsetPlain;
            idePixelBorder.Checked = tabControl.IDEPixelBorder;
            idePixelArea.Checked = tabControl.IDEPixelArea;
            insetPagesOnly.Checked = tabControl.InsetBorderPagesOnly;
            selectedTextOnly.Checked = tabControl.SelectedTextOnly;
            hoverSelect.Checked = tabControl.HoverSelect;
            multiLine.Checked = tabControl.Multiline;
            multilineFullWidth.Checked = tabControl.MultilineFullWidth;
            numericUpDown1.Value = tabControl.ControlLeftOffset;
            numericUpDown2.Value = tabControl.ControlRightOffset;
            numericUpDown3.Value = tabControl.ControlTopOffset;
            numericUpDown4.Value = tabControl.ControlBottomOffset;
        }

        protected void OnAddPage(object sender, EventArgs e)
        {
            Control controlToAdd = null;

            switch(_count)
            {
                case 0:
                case 2:
                case 4:
                case 6:
                    controlToAdd = new DummyForm();
                    controlToAdd.BackColor = Color.White;
                    break;

                case 1:
                case 5:
                    RichTextBox rtb = new RichTextBox();
                    rtb.Text = "The quick brown fox jumped over the lazy dog.";
                    controlToAdd = rtb;
                    break;

                case 3:
                    controlToAdd = new DummyPanel();
                    controlToAdd.BackColor = Color.DarkSlateBlue;
                    break;
            }

            // Define color that match the tabControl
            controlToAdd.ForeColor = tabControl.ForeColor;
            controlToAdd.BackColor = tabControl.BackColor;

            Crownwood.Magic.Controls.TabPage page;

            // Create a new page with the appropriate control for display, title text and image
            page = new Crownwood.Magic.Controls.TabPage(_strings[_count], controlToAdd, null, _count);

            // Make this page become selected when added
            page.Selected = true;

            tabControl.TabPages.Add(page);
			
            // Update the count for creating new pages
            _count++;
            if (_count > 6)
                _count = 0;
        }

        protected void OnRemovePage(object sender, EventArgs e)
        {
            if (tabControl.TabPages.Count > 0)
                tabControl.TabPages.RemoveAt(0);
        }

        protected void OnClearAll(object sender, EventArgs e)
        {
            tabControl.TabPages.Clear();
        }

        protected void OnStyleIDE(object sender, EventArgs e)
        {
            tabControl.Style = VisualStyle.IDE;
            if (!_update)
                UpdateControls();
            else
                _update = false;
        }

        protected void OnStylePlain(object sender, EventArgs e)
        {
            tabControl.Style = VisualStyle.Plain;
            if (!_update)
                UpdateControls();
            else
                _update = false;
        }

        protected void OnAppearanceMultiBox(object sender, EventArgs e)
        {
            tabControl.Appearance = Crownwood.Magic.Controls.TabControl.VisualAppearance.MultiBox;
            if (!_update)
                UpdateControls();
            else
                _update = false;
        }

        protected void OnAppearanceMultiForm(object sender, EventArgs e)
        {
            tabControl.Appearance = Crownwood.Magic.Controls.TabControl.VisualAppearance.MultiForm;
            if (!_update)
                UpdateControls();
            else
                _update = false;
        }

        protected void OnAppearanceMultiDocument(object sender, EventArgs e)
        {
            tabControl.Appearance = Crownwood.Magic.Controls.TabControl.VisualAppearance.MultiDocument;
            if (!_update)
                UpdateControls();
            else
                _update = false;
        }

        protected void positionAtTop_CheckedChanged(object sender, System.EventArgs e)
        {
            tabControl.PositionTop = positionAtTop.Checked;
            UpdateControls();
        }

        protected void Highlight_CheckedChanged(object sender, System.EventArgs e)
        {
            tabControl.HotTrack = hotTrack.Checked;
            UpdateControls();
        }

        protected void shrinkPages_CheckedChanged(object sender, System.EventArgs e)
        {
            tabControl.ShrinkPagesToFit = shrinkPages.Checked;
            UpdateControls();
        }

        protected void showClose_CheckedChanged(object sender, System.EventArgs e)
        {
            tabControl.ShowClose = showClose.Checked;
            UpdateControls();
        }

        protected void showArrows_CheckedChanged(object sender, System.EventArgs e)
        {
            tabControl.ShowArrows = showArrows.Checked;
            UpdateControls();
        }

        protected void insetPlain_CheckedChanged(object sender, System.EventArgs e)
        {
            tabControl.InsetPlain = insetPlain.Checked;
            UpdateControls();
        }

        protected void idePixelBorder_CheckedChanged(object sender, System.EventArgs e)
        {
            tabControl.IDEPixelBorder = idePixelBorder.Checked;
            UpdateControls();
        }

        protected void idePixelArea_CheckedChanged(object sender, System.EventArgs e)
        {
            tabControl.IDEPixelArea = idePixelArea.Checked;
            UpdateControls();
        }

        protected void insetPagesOnly_CheckedChanged(object sender, System.EventArgs e)
        {
            tabControl.InsetBorderPagesOnly = insetPagesOnly.Checked;
            UpdateControls();
        }

        protected void selectedTextOnly_CheckedChanged(object sender, System.EventArgs e)
        {
            tabControl.SelectedTextOnly = selectedTextOnly.Checked;
            UpdateControls();
        }

        protected void numericUpDown1_ValueChanged(object sender, System.EventArgs e)
        {
            tabControl.ControlLeftOffset = (int)numericUpDown1.Value;
            UpdateControls();
        }

        protected void numericUpDown3_ValueChanged(object sender, System.EventArgs e)
        {
            tabControl.ControlTopOffset = (int)numericUpDown3.Value;
            UpdateControls();
        }

        protected void numericUpDown2_ValueChanged(object sender, System.EventArgs e)
        {
            tabControl.ControlRightOffset = (int)numericUpDown2.Value;
            UpdateControls();
        }

        protected void numericUpDown4_ValueChanged(object sender, System.EventArgs e)
        {
            tabControl.ControlBottomOffset = (int)numericUpDown4.Value;
            UpdateControls();
        }

        private void tabShowAlways_CheckedChanged(object sender, System.EventArgs e)
        {
            tabControl.HideTabsMode = Crownwood.Magic.Controls.TabControl.HideTabsModes.ShowAlways;
            if (!_update)
                UpdateControls();
            else
                _update = false;
        }

        private void tabHideAlways_CheckedChanged(object sender, System.EventArgs e)
        {
            tabControl.HideTabsMode = Crownwood.Magic.Controls.TabControl.HideTabsModes.HideAlways;
            if (!_update)
                UpdateControls();
            else
                _update = false;
        }

        private void tabHideUsingLogic_CheckedChanged(object sender, System.EventArgs e)
        {        
            tabControl.HideTabsMode = Crownwood.Magic.Controls.TabControl.HideTabsModes.HideUsingLogic;
            if (!_update)
                UpdateControls();
            else
                _update = false;
        }

        private void tabHideWithoutMouse_CheckedChanged(object sender, System.EventArgs e)
        {
            tabControl.HideTabsMode = Crownwood.Magic.Controls.TabControl.HideTabsModes.HideWithoutMouse;
            if (!_update)
                UpdateControls();
            else
                _update = false;
        }

        protected void hoverSelect_CheckedChanged(object sender, System.EventArgs e)
        {
            tabControl.HoverSelect = hoverSelect.Checked;
            UpdateControls();
        }

        private void multiLine_CheckedChanged(object sender, System.EventArgs e)
        {
            tabControl.Multiline = multiLine.Checked;
            UpdateControls();
        }
        

        private void multilineFullWidth_CheckedChanged(object sender, System.EventArgs e)
        {
            tabControl.MultilineFullWidth = multilineFullWidth.Checked;
            UpdateControls();
        }

        protected void normal_CheckedChanged(object sender, System.EventArgs e)
        {
            // Give the tabControl a blue appearance
            tabControl.BackColor = _startBackColor;
            tabControl.ForeColor = _startForeColor;
            tabControl.ButtonActiveColor = _startButtonActive;
            tabControl.ButtonInactiveColor = _startButtonInactive;
            tabControl.TextInactiveColor = _startTextInactiveColor;
			tabControl.HotTextColor = _startHotTextColor;

            DefinePageColors(_startBackColor, _startForeColor);
        }

        protected void blue_CheckedChanged(object sender, System.EventArgs e)
        {
            // Give the tabControl a blue appearance
            tabControl.BackColor = Color.DarkBlue;
            tabControl.ForeColor = Color.White;
            tabControl.ButtonActiveColor = Color.White;
            tabControl.ButtonInactiveColor = Color.LightBlue;
            tabControl.TextInactiveColor = Color.Yellow;
			tabControl.HotTextColor = Color.Orange;

            DefinePageColors(Color.DarkBlue, Color.White);
        }

        protected void red_CheckedChanged(object sender, System.EventArgs e)
        {
            // Give the tabControl a red appearance
            tabControl.BackColor = Color.DarkRed;
            tabControl.ForeColor = Color.White;
            tabControl.ButtonActiveColor = Color.White;
            tabControl.ButtonInactiveColor = Color.Red;
            tabControl.TextInactiveColor = Color.White;
			tabControl.HotTextColor = Color.Cyan;

            DefinePageColors(Color.DarkRed, Color.White);
        }

        protected void DefinePageColors(Color newBack, Color newFore)
        {
            foreach(Crownwood.Magic.Controls.TabPage page in tabControl.TabPages)
            {
                if (page.Control != null)
                {
                    page.Control.ForeColor = newFore;
                    page.Control.BackColor = newBack;
                }
            }
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() 
        {
            Application.Run(new SampleTabControl());
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
            _dummy1.Text = "Dummy 1";
            _dummy1.Size = new Size(90,25);
            _dummy1.Location = new Point(10,10);

            _dummy2.Text = "Dummy 2";
            _dummy2.Size = new Size(90,25);
            _dummy2.Location = new Point(110,10);

            _dummyBox.Text = "Form GroupBox";
            _dummyBox.Size = new Size(190, 67);
            _dummyBox.Location = new Point(10, 45);

            _dummy3.Text = "Dummy 3";
            _dummy3.Size = new Size(100,22);
            _dummy3.Location = new Point(10, 20);

            _dummy4.Text = "Dummy 4";
            _dummy4.Size = new Size(100,22);
            _dummy4.Location = new Point(10, 42);
            _dummy4.Checked = true;

            _dummyBox.Controls.AddRange(new Control[]{_dummy3, _dummy4});

            Controls.AddRange(new Control[]{_dummy1, _dummy2, _dummyBox});

            // Define then control to be selected when first is activated for first time
            this.ActiveControl = _dummy4;
        }
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
        _dummyBox.Size = new Size(190, 67);
        _dummyBox.Location = new Point(10, 10);

        _dummy3.Text = "RadioButton 3";
        _dummy3.Size = new Size(100,22);
        _dummy3.Location = new Point(10, 20);

        _dummy4.Text = "RadioButton 4";
        _dummy4.Size = new Size(100,22);
        _dummy4.Location = new Point(10, 42);
        _dummy4.Checked = true;

        _dummyBox.Controls.AddRange(new Control[]{_dummy3, _dummy4});

        Controls.AddRange(new Control[]{_dummyBox});
    }
}
