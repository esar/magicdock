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
Imports System.Xml
Imports System.Data
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports Crownwood.Magic.Common
Imports Crownwood.Magic.Controls
Imports Crownwood.Magic.Docking
Imports Crownwood.Magic.Menus

Public Class Form1
    Inherits System.Windows.Forms.Form

    Protected _slot1 As Byte()
    Protected _slot2 As Byte()
    Protected _count As Integer = 0
    Protected _ignoreClose As Integer = 0
    Protected _colorIndex As Integer = 0
    Protected _allowContextMenu As Boolean = True
    Protected _customContextMenu As Boolean = False
    Protected _tabsBottom As Boolean = True
    Protected _captionBars As Boolean = True
    Protected _closeButtons As Boolean = True
    Protected _style As VisualStyle
    Protected _placeHolder As MenuCommand
    Protected _manager As DockingManager
    Protected _internalImages As ImageList
    Protected _statusBar As StatusBar
    Protected _topMenu As Crownwood.Magic.Menus.MenuControl
    Protected _filler As Crownwood.Magic.Controls.TabControl
    Protected _tabAppearance As Crownwood.Magic.Controls.TabControl.VisualAppearance = Crownwood.Magic.Controls.TabControl.VisualAppearance.MultiForm

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        ' Discover which docking manager to use
        If DialogResult.Yes = MessageBox.Show("Press 'Yes' to select IDE appearance" & vbCrLf & vbCrLf & _
                                              "Press 'No' to select Plain appearance" & vbCrLf, _
                                              "Select Visual Style", MessageBoxButtons.YesNo) Then
            _style = VisualStyle.IDE
        Else
            _style = VisualStyle.Plain
        End If

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Create a strip of images by loading an embedded bitmap resource
        _internalImages = ResourceHelper.LoadBitmapStrip(Me.GetType(), _
                                                         "SampleDocking.SampleImages.bmp", _
                                                         New Size(16, 16), _
                                                         New Point(0, 0))

        Dim bs As BorderStyle

        If _style = VisualStyle.Plain Then
            bs = BorderStyle.None
        Else
            bs = BorderStyle.FixedSingle
        End If

        Dim rtb1 As RichTextBox = New RichTextBox()
        Dim rtb2 As RichTextBox = New RichTextBox()
        Dim rtb3 As RichTextBox = New RichTextBox()

        rtb1.BorderStyle = bs
        rtb2.BorderStyle = bs
        rtb3.BorderStyle = bs

        _filler = New Crownwood.Magic.Controls.TabControl()
        _filler.TabPages.Add(New Crownwood.Magic.Controls.TabPage("First", rtb1))
        _filler.TabPages.Add(New Crownwood.Magic.Controls.TabPage("Second", rtb2))
        _filler.TabPages.Add(New Crownwood.Magic.Controls.TabPage("Third", rtb3))
        _filler.Appearance = Crownwood.Magic.Controls.TabControl.VisualAppearance.MultiDocument
        _filler.Dock = DockStyle.Fill
        _filler.Style = _style
        _filler.IDEPixelBorder = True
        Controls.Add(_filler)

        ' Reduce the amount of flicker that occurs when windows are redocked within
        ' the container. As this prevents unsightly backcolors being drawn in the
        ' WM_ERASEBACKGROUND that seems to occur.
        SetStyle(ControlStyles.DoubleBuffer, True)
        SetStyle(ControlStyles.AllPaintingInWmPaint, True)

        ' Create the object that manages the docking state
        _manager = New DockingManager(Me, _style)

        ' Notifications
        AddHandler _manager.ContextMenu, AddressOf OnContextMenu
        AddHandler _manager.ContentHiding, AddressOf OnContentHiding
        AddHandler _manager.TabControlCreated, AddressOf OnTabControlCreated

        ' Ensure that the RichTextBox is always the innermost control
        _manager.InnerControl = _filler

        ' Create and setup the StatusBar object
        _statusBar = New StatusBar()
        _statusBar.Dock = DockStyle.Bottom
        _statusBar.ShowPanels = True

        ' Create and setup a single panel for the StatusBar
        Dim statusBarPanel As StatusBarPanel = New StatusBarPanel()
        statusBarPanel.AutoSize = StatusBarPanelAutoSize.Spring
        _statusBar.Panels.Add(statusBarPanel)
        Controls.Add(_statusBar)

        _topMenu = CreateMenus()

        ' Ensure that docking occurs after the menu control and status bar controls
        _manager.OuterControl = _statusBar

        ' Hook into ability to customize configuration persistence
        AddHandler _manager.SaveCustomConfig, AddressOf OnSaveConfig
        AddHandler _manager.LoadCustomConfig, AddressOf OnLoadConfig
    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.SuspendLayout()
        '
        'Form1
        '
        Me.ClientSize = New System.Drawing.Size(500, 500)
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Protected Function CreateMenus() As Crownwood.Magic.Menus.MenuControl
        Dim topMenu As Crownwood.Magic.Menus.MenuControl = New Crownwood.Magic.Menus.MenuControl()
        topMenu.Style = _style
        topMenu.MultiLine = False

        Dim topManager As MenuCommand = New MenuCommand("Manager")
        Dim topConfig As MenuCommand = New MenuCommand("Config")
        Dim topSettings As MenuCommand = New MenuCommand("Settings")
        Dim topColors As MenuCommand = New MenuCommand("Colors")
        Dim topTabControls As MenuCommand = New MenuCommand("TabControls")
        topMenu.MenuCommands.AddRange(New MenuCommand() {topManager, topConfig, topSettings, topColors, topTabControls})

        ' Manager 
        Dim managerC1 As MenuCommand = New MenuCommand("Create Form", New EventHandler(AddressOf OnCreateC1))
        Dim managerC2 As MenuCommand = New MenuCommand("Create Panel", New EventHandler(AddressOf OnCreateC2))
        Dim managerC3 As MenuCommand = New MenuCommand("Create RichTextBox", New EventHandler(AddressOf OnCreateC3))
        Dim managerSep1 As MenuCommand = New MenuCommand("-")
        Dim managerC31 As MenuCommand = New MenuCommand("Create 3 in Row", New EventHandler(AddressOf OnCreateC31))
        Dim managerC32 As MenuCommand = New MenuCommand("Create 3 in Column", New EventHandler(AddressOf OnCreateC32))
        Dim managerC33 As MenuCommand = New MenuCommand("Create 3 in Window", New EventHandler(AddressOf OnCreateC33))
        Dim managerSep2 As MenuCommand = New MenuCommand("-")
        Dim managerFF As MenuCommand = New MenuCommand("Create 3 in Floating Form", New EventHandler(AddressOf OnCreateFF))
        Dim managerSep3 As MenuCommand = New MenuCommand("-")
        Dim managerClear As MenuCommand = New MenuCommand("Delete Contents", New EventHandler(AddressOf OnDeleteAll))
        Dim managerSep4 As MenuCommand = New MenuCommand("-")
        Dim managerShowAll As MenuCommand = New MenuCommand("Show All", New EventHandler(AddressOf OnShowAll))
        Dim managerHideAll As MenuCommand = New MenuCommand("Hide All", New EventHandler(AddressOf OnHideAll))
        Dim managerSep5 As MenuCommand = New MenuCommand("-")
        Dim managerInsideFill As MenuCommand = New MenuCommand("Inside Fill", New EventHandler(AddressOf OnInsideFill))
        Dim managerSep6 As MenuCommand = New MenuCommand("-")
        Dim managerExit As MenuCommand = New MenuCommand("Exit", New EventHandler(AddressOf OnExit))

        AddHandler managerInsideFill.Update, AddressOf OnUpdateInsideFill

        topManager.MenuCommands.AddRange(New MenuCommand() {managerC1, managerC2, managerC3, managerSep1, _
                                                           managerC31, managerC32, managerC33, managerSep2, _
                                                           managerFF, managerSep3, _
                                                           managerClear, managerSep4, managerShowAll, _
                                                           managerHideAll, managerSep5, managerInsideFill, _
                                                           managerSep5, managerExit})

        ' Config
        Dim configSF1 As MenuCommand = New MenuCommand("Save as Config1.xml", New EventHandler(AddressOf OnSaveFile1))
        Dim configSF2 As MenuCommand = New MenuCommand("Save as Config2.xml", New EventHandler(AddressOf OnSaveFile2))
        Dim configSep1 As MenuCommand = New MenuCommand("-")
        Dim configLF1 As MenuCommand = New MenuCommand("Load from Config1.xml", New EventHandler(AddressOf OnLoadFile1))
        Dim configLF2 As MenuCommand = New MenuCommand("Load from Config2.xml", New EventHandler(AddressOf OnLoadFile2))
        Dim configSep2 As MenuCommand = New MenuCommand("-")
        Dim configSA1 As MenuCommand = New MenuCommand("Save to byte array1", New EventHandler(AddressOf OnSaveArray1))
        Dim configSA2 As MenuCommand = New MenuCommand("Save to byte array2", New EventHandler(AddressOf OnSaveArray2))
        Dim configSep3 As MenuCommand = New MenuCommand("-")
        Dim configLA1 As MenuCommand = New MenuCommand("Load from byte array1", New EventHandler(AddressOf OnLoadArray1))
        Dim configLA2 As MenuCommand = New MenuCommand("Load from byte array2", New EventHandler(AddressOf OnLoadArray2))
        topConfig.MenuCommands.AddRange(New MenuCommand() {configSF1, configSF2, configSep1, _
                                                          configLF1, configLF2, configSep2, _
                                                          configSA1, configSA2, configSep3, _
                                                          configLA1, configLA2})

        ' Settings
        Dim settingsShow As MenuCommand = New MenuCommand("Allow Context Menu", New EventHandler(AddressOf OnContextMenuAllow))
        Dim settingsCustom As MenuCommand = New MenuCommand("Customize Context Menu", New EventHandler(AddressOf OnContextMenuCustomize))
        Dim settingsSep1 As MenuCommand = New MenuCommand("-")
        Dim settingsRSBD As MenuCommand = New MenuCommand("Default ResizeBarVector", New EventHandler(AddressOf OnResizeBarDefault))
        Dim settingsRSB1 As MenuCommand = New MenuCommand("1 Pixel ResizeBarVector", New EventHandler(AddressOf OnResizeBar1))
        Dim settingsRSB5 As MenuCommand = New MenuCommand("5 Pixel ResizeBarVector", New EventHandler(AddressOf OnResizeBar5))
        Dim settingsRSB7 As MenuCommand = New MenuCommand("7 Pixel ResizeBarVector", New EventHandler(AddressOf OnResizeBar7))
        Dim settingsSep2 As MenuCommand = New MenuCommand("-")
        Dim settingsCaptionBars As MenuCommand = New MenuCommand("Show all caption bars", New EventHandler(AddressOf OnCaptionBars))
        Dim settingsCloseButtons As MenuCommand = New MenuCommand("Show all close buttons", New EventHandler(AddressOf OnCloseButtons))
        Dim settingsSep3 As MenuCommand = New MenuCommand("-")
        Dim settingsAllow As MenuCommand = New MenuCommand("Allow all close buttons", New EventHandler(AddressOf OnAllowAllCloseButton))
        Dim settingsIgnoreAll As MenuCommand = New MenuCommand("Ignore all close buttons", New EventHandler(AddressOf OnIgnoreAllCloseButton))
        Dim settingsIgnorePanel As MenuCommand = New MenuCommand("Ignore all Panel close buttons", New EventHandler(AddressOf OnIgnoreAllPanelButton))
        Dim settingsIgnoreForm As MenuCommand = New MenuCommand("Ignore all Form close buttons", New EventHandler(AddressOf OnIgnoreAllFormButton))
        Dim settingsIgnoreTextBox As MenuCommand = New MenuCommand("Ignore all RichTextBox close buttons", New EventHandler(AddressOf OnIgnoreAllTextboxButton))
        Dim settingsSep4 As MenuCommand = New MenuCommand("-")
        Dim settingsAllowMinMax As MenuCommand = New MenuCommand("Enable Min/Max in Columns/Rows", New EventHandler(AddressOf OnAllowMinMax))
        Dim settingsSep5 As MenuCommand = New MenuCommand("-")
        Dim settingsPlainTabBorder As MenuCommand = New MenuCommand("Show Plain Tab Border", New EventHandler(AddressOf OnPlainTabBorder))

        AddHandler settingsShow.Update, AddressOf OnUpdateAllow
        AddHandler settingsCustom.Update, AddressOf OnUpdateCustomize
        AddHandler settingsRSBD.Update, AddressOf OnUpdateRSBD
        AddHandler settingsRSB1.Update, AddressOf OnUpdateRSB1
        AddHandler settingsRSB5.Update, AddressOf OnUpdateRSB5
        AddHandler settingsRSB7.Update, AddressOf OnUpdateRSB7
        AddHandler settingsCaptionBars.Update, AddressOf OnUpdateCaptionBars
        AddHandler settingsCloseButtons.Update, AddressOf OnUpdateCloseButtons
        AddHandler settingsAllow.Update, AddressOf OnUpdateAllowAll
        AddHandler settingsIgnoreAll.Update, AddressOf OnUpdateIgnoreAll
        AddHandler settingsIgnorePanel.Update, AddressOf OnUpdateIgnorePanel
        AddHandler settingsIgnoreForm.Update, AddressOf OnUpdateIgnoreForm
        AddHandler settingsIgnoreTextBox.Update, AddressOf OnUpdateIgnoreTextBox
        AddHandler settingsAllowMinMax.Update, AddressOf OnUpdateAllowMinMax
        AddHandler settingsPlainTabBorder.Update, AddressOf OnUpdatePlainTabBorder

        topSettings.MenuCommands.AddRange(new MenuCommand(){settingsShow,settingsCustom,settingsSep1,settingsRSBD, _
                                                            settingsRSB1,settingsRSB5,settingsRSB7,settingsSep2, _
                                                            settingsCaptionBars,settingsCloseButtons,settingsSep3, _
                                                            settingsAllow,settingsIgnoreAll,settingsIgnorePanel, _
                                                            settingsIgnoreForm,settingsIgnoreTextBox,settingsSep4, _
                                                            settingsAllowMinMax,settingsSep5,settingsPlainTabBorder})


        ' Colors
        Dim colorDefault As MenuCommand = New MenuCommand("System Default", New EventHandler(AddressOf OnColorDefault))
        Dim colorSlateBlue As MenuCommand = New MenuCommand("Custom - Slate Blue", New EventHandler(AddressOf OnColorSlateBlue))
        Dim colorFirebrick As MenuCommand = New MenuCommand("Custom - Firebrick", New EventHandler(AddressOf OnColorFirebrick))
        Dim colorGreen As MenuCommand = New MenuCommand("Custom - Green", New EventHandler(AddressOf OnColorGreen))

        AddHandler colorDefault.Update, AddressOf OnUpdateDefault
        AddHandler colorSlateBlue.Update, AddressOf OnUpdateSlateBlue
        AddHandler colorFirebrick.Update, AddressOf OnUpdateFirebrick
        AddHandler colorGreen.Update, AddressOf OnUpdateGreen

        topColors.MenuCommands.AddRange(New MenuCommand() {colorDefault, colorSlateBlue, colorFirebrick, colorGreen})


        ' TabControls
        Dim tabsBox As MenuCommand = New MenuCommand("MultiBox", New EventHandler(AddressOf OnTabsMultiBox))
        Dim tabsForm As MenuCommand = New MenuCommand("MultiForm", New EventHandler(AddressOf OnTabsMultiForm))
        Dim tabsDocument As MenuCommand = New MenuCommand("MultiDocument", New EventHandler(AddressOf OnTabsMultiDocument))
        Dim tabsSep1 As MenuCommand = New MenuCommand("-")
        Dim tabsTop As MenuCommand = New MenuCommand("Tabs at top", New EventHandler(AddressOf OnTabsTop))
        Dim tabsBottom As MenuCommand = New MenuCommand("Tabs at bottom", New EventHandler(AddressOf OnTabsBottom))

        AddHandler tabsBox.Update, AddressOf OnUpdateTabsBox
        AddHandler tabsForm.Update, AddressOf OnUpdateTabsForm
        AddHandler tabsDocument.Update, AddressOf OnUpdateTabsDocument
        AddHandler tabsTop.Update, AddressOf OnUpdateTabsTop
        AddHandler tabsBottom.Update, AddressOf OnUpdateTabsBottom

        topTabControls.MenuCommands.AddRange(new MenuCommand(){tabsBox,tabsForm,tabsDocument, _
															   tabsSep1,tabsTop,tabsBottom})

        topMenu.Dock = DockStyle.Top
        Controls.Add(topMenu)

        Return topMenu
    End Function

    Protected Sub OnContextMenu(ByVal pm As PopupMenu, ByVal cea As CancelEventArgs)
        ' Show the PopupMenu be cancelled and not shown?
        If Not _allowContextMenu Then
            cea.Cancel = True
        Else
            If _customContextMenu Then
                ' Remove the Show All and Hide All commands
                pm.MenuCommands.Remove(pm.MenuCommands("Show All"))
                pm.MenuCommands.Remove(pm.MenuCommands("Hide All"))

                ' Add a custom item at the start
                pm.MenuCommands.Insert(0, (New MenuCommand("Custom 1")))
                pm.MenuCommands.Insert(1, (New MenuCommand("-")))

                ' Add a couple of custom commands at the end
                pm.MenuCommands.Add(New MenuCommand("Custom 2"))
                pm.MenuCommands.Add(New MenuCommand("Custom 3"))
            End If
        End If
    End Sub

    Protected Sub OnContentHiding(ByVal c As Content, ByVal cea As CancelEventArgs)
        Select Case _ignoreClose
            Case 0
                ' Allow all, do nothing
            Case 1
                ' Ignore all, cancel
                cea.Cancel = True
            Case 2
                ' Ignore Panels
                Dim p As Panel = c.Control
                cea.Cancel = Not (p Is Nothing)
            Case 3
                ' Ignore Forms
                Dim f As Form = c.Control
                cea.Cancel = Not (f Is Nothing)
            Case 4
                ' Ignore RichTextBox
                Dim rtb As RichTextBox = c.Control
                cea.Cancel = Not (rtb Is Nothing)
        End Select
    End Sub

    Protected Sub OnTabControlCreated(ByVal tabControl As Crownwood.Magic.Controls.TabControl)
        tabControl.PositionTop = Not _tabsBottom
        tabControl.Appearance = _tabAppearance
    End Sub

    Protected Sub OnContextMenuAllow(ByVal sender As Object, ByVal e As EventArgs)
        ' Toggle the display of the docking windows context menu
        _allowContextMenu = (_allowContextMenu = False)
    End Sub

    Protected Sub OnUpdateAllow(ByVal sender As Object, ByVal e As EventArgs)
        Dim allowCommand As MenuCommand = sender
        allowCommand.Checked = _allowContextMenu
    End Sub

    Protected Sub OnContextMenuCustomize(ByVal sender As Object, ByVal e As EventArgs)
        ' Toggle the customization of the displayed context menu
        _customContextMenu = (_customContextMenu = False)
    End Sub

    Protected Sub OnUpdateCustomize(ByVal sender As Object, ByVal e As EventArgs)
        Dim customizeCommand As MenuCommand = sender
        customizeCommand.Checked = _customContextMenu
    End Sub

    Protected Sub OnInsideFill(ByVal sender As Object, ByVal e As EventArgs)
        If _manager.InsideFill Then
            Controls.Add(_filler)
            Controls.SetChildIndex(_filler, 0)
            _manager.InnerControl = _filler
            _manager.InsideFill = False
        Else
            Controls.Remove(_filler)
            _manager.InnerControl = Nothing
            _manager.InsideFill = True
        End If
    End Sub

    Protected Sub OnUpdateInsideFill(ByVal sender As Object, ByVal e As EventArgs)
        Dim fillCommand As MenuCommand = sender
        fillCommand.Checked = _manager.InsideFill
    End Sub

    Protected Sub OnCreateC1(ByVal sender As Object, ByVal e As EventArgs)
        ' Create Content which contains a RichTextBox
        Dim c As Content = _manager.Contents.Add(New DummyForm(), "Form " & _count, _internalImages, _count Mod 6)

        ' Increment count of created contents
        _count += 1

        ' Setup initial state to match menu selections
        DefineContentState(c)

        ' Setup the correct starting colors to match the menu selections
        DefineControlColors(c)

        ' Request a new Docking window be created for the above Content on the right edge
        _manager.AddContentWithState(c, State.DockRight)
    End Sub

    Protected Sub OnCreateC2(ByVal sender As Object, ByVal e As EventArgs)
        ' Create Content which contains a RichTextBox
        Dim c As Content = _manager.Contents.Add(New DummyPanel(), "Panel " & _count, _internalImages, _count Mod 6)

        ' Increment count of created contents
        _count += 1

        ' Setup initial state to match menu selections
        DefineContentState(c)

        ' Setup the correct starting colors to match the menu selections
        DefineControlColors(c)

        ' Request a new Docking window be created for the above Content on the bottom edge
        _manager.AddContentWithState(c, State.DockBottom)
    End Sub

    Protected Sub OnCreateC3(ByVal sender As Object, ByVal e As EventArgs)
        ' Create Content which contains a RichTextBox
        Dim c As Content = _manager.Contents.Add(New RichTextBox(), "RichTextBox " & _count, _internalImages, _count Mod 6)

        ' Increment count of created contents
        _count += 1

        AddHandler c.Control.Disposed, AddressOf OnRTBDisposed

        ' Setup initial state to match menu selections
        DefineContentState(c)

        ' Setup the correct starting colors to match the menu selections
        DefineControlColors(c)

        ' Request a new Docking window be created for the above Content on the left edge
        _manager.AddContentWithState(c, State.DockLeft)
    End Sub

    Protected Sub OnCreateC31(ByVal sender As Object, ByVal e As EventArgs)
        ' Create three Content objects, one of each type
        Dim cA As Content = _manager.Contents.Add(New DummyForm(), "Form " & _count, _internalImages, _count Mod 6)
        Dim cB As Content = _manager.Contents.Add(New DummyPanel(), "Panel " & (_count + 1), _internalImages, _count Mod 6)
        Dim cC As Content = _manager.Contents.Add(New RichTextBox(), "RichTextBox " & (_count + 2), _internalImages, _count Mod 6)

        ' Increment count of created contents
        _count += 3

        ' Setup initial state to match menu selections
        DefineContentState(cA)
        DefineContentState(cB)
        DefineContentState(cC)

        ' Setup the correct starting colors to match the menu selections
        DefineControlColors(cA)
        DefineControlColors(cB)
        DefineControlColors(cC)

        ' Request a new Docking window be created for the first content on the bottom edge
        Dim wc As WindowContent = _manager.AddContentWithState(cA, State.DockBottom)

        ' Add two other content into the same Zone
        _manager.AddContentToZone(cB, wc.ParentZone, 1)
        _manager.AddContentToZone(cC, wc.ParentZone, 2)
    End Sub

    Protected Sub OnCreateC32(ByVal sender As Object, ByVal e As EventArgs)
        ' Create three Content objects, one of each type
        Dim cA As Content = _manager.Contents.Add(New DummyForm(), "Form " & _count, _internalImages, _count Mod 6)
        Dim cB As Content = _manager.Contents.Add(New DummyPanel(), "Panel " & (_count + 1), _internalImages, _count Mod 6)
        Dim cC As Content = _manager.Contents.Add(New RichTextBox(), "RichTextBox " & (_count + 2), _internalImages, _count Mod 6)

        ' Increment count of created contents
        _count += 3

        ' Setup initial state to match menu selections
        DefineContentState(cA)
        DefineContentState(cB)
        DefineContentState(cC)

        ' Setup the correct starting colors to match the menu selections
        DefineControlColors(cA)
        DefineControlColors(cB)
        DefineControlColors(cC)

        ' Request a new Docking window be created for the first content on the left edge
        Dim wc As WindowContent = _manager.AddContentWithState(cA, State.DockLeft)

        ' Add two other content into the same Zone
        _manager.AddContentToZone(cB, wc.ParentZone, 1)
        _manager.AddContentToZone(cC, wc.ParentZone, 2)
    End Sub

    Protected Sub OnCreateC33(ByVal sender As Object, ByVal e As EventArgs)
        ' Create three Content objects, one of each type
        Dim cA As Content = _manager.Contents.Add(New DummyForm(), "Form " & _count, _internalImages, _count Mod 6)
        Dim cB As Content = _manager.Contents.Add(New DummyPanel(), "Panel " & (_count + 1), _internalImages, _count Mod 6)
        Dim cC As Content = _manager.Contents.Add(New RichTextBox(), "RichTextBox " & (_count + 2), _internalImages, _count Mod 6)

        ' Increment count of created contents
        _count += 3

        ' Setup initial state to match menu selections
        DefineContentState(cA)
        DefineContentState(cB)
        DefineContentState(cC)

        ' Setup the correct starting colors to match the menu selections
        DefineControlColors(cA)
        DefineControlColors(cB)
        DefineControlColors(cC)

        ' Request a new Docking window be created for the first content on the bottom edge
        Dim wc As WindowContent = _manager.AddContentWithState(cA, State.DockBottom)

        ' Add two other content into the same Window
        _manager.AddContentToWindowContent(cB, wc)
        _manager.AddContentToWindowContent(cC, wc)
    End Sub

    Protected Sub OnCreateFF(ByVal sender As Object, ByVal e As EventArgs)
        ' Create three Content objects, one of each type
        Dim cA As Content = _manager.Contents.Add(New DummyForm(), "Form " & _count, _internalImages, _count Mod 6)
        Dim cB As Content = _manager.Contents.Add(New DummyPanel(), "Panel " & (_count + 1), _internalImages, _count Mod 6)
        Dim cC As Content = _manager.Contents.Add(New RichTextBox(), "RichTextBox " & (_count + 2), _internalImages, _count Mod 6)

        ' Increment count of created contents
        _count += 3

        ' Define the initial floating form size
        cA.FloatingSize = New Size(250, 450)

        ' Setup initial state to match menu selections
        DefineContentState(cA)
        DefineContentState(cB)
        DefineContentState(cC)

        ' Setup the correct starting colors to match the menu selections
        DefineControlColors(cA)
        DefineControlColors(cB)
        DefineControlColors(cC)

        ' Request a new Docking window be created for the first content on the bottom edge
        Dim wc As WindowContent = _manager.AddContentWithState(cA, State.Floating)

        ' Add second content into the same Window
        _manager.AddContentToWindowContent(cB, wc)

        ' Add third into same Zone
        _manager.AddContentToZone(cC, wc.ParentZone, 1)
    End Sub

    Protected Sub DefineContentState(ByVal c As Content)
        c.CaptionBar = _captionBars
        c.CloseButton = _closeButtons
    End Sub

    Protected Sub DefineControlColors(ByVal c As Content)
        ' Only interested in Forms and Panels
        If TypeOf c Is Form Or TypeOf c Is Panel Then
            ' Starting color depends on select menu option
            Select Case _colorIndex
                Case 0
                    c.Control.BackColor = SystemColors.Control
                    c.Control.ForeColor = SystemColors.ControlText
                Case 1
                    c.Control.BackColor = Color.DarkSlateBlue
                    c.Control.ForeColor = Color.White
                Case 2
                    c.Control.BackColor = Color.Firebrick
                    c.Control.ForeColor = Color.White
                Case 3
                    c.Control.BackColor = Color.PaleGreen
                    c.Control.ForeColor = Color.Black
            End Select
        End If
    End Sub

    Protected Sub OnDeleteAll(ByVal sender As Object, ByVal e As EventArgs)
        _manager.Contents.Clear()
    End Sub

    Protected Sub OnShowAll(ByVal sender As Object, ByVal e As EventArgs)
        _manager.ShowAllContents()
    End Sub

    Protected Sub OnHideAll(ByVal sender As Object, ByVal e As EventArgs)
        _manager.HideAllContents()
    End Sub

    Protected Sub OnExit(ByVal sender As Object, ByVal e As EventArgs)
        Close()
    End Sub

    Protected Sub OnSaveFile1(ByVal sender As Object, ByVal e As EventArgs)
        _manager.SaveConfigToFile("config1.xml")
    End Sub

    Protected Sub OnSaveFile2(ByVal sender As Object, ByVal e As EventArgs)
        _manager.SaveConfigToFile("config2.xml")
    End Sub

    Protected Sub OnLoadFile1(ByVal sender As Object, ByVal e As EventArgs)
        _manager.LoadConfigFromFile("config1.xml")
    End Sub

    Protected Sub OnLoadFile2(ByVal sender As Object, ByVal e As EventArgs)
        _manager.LoadConfigFromFile("config2.xml")
    End Sub

    Protected Sub OnSaveArray1(ByVal sender As Object, ByVal e As EventArgs)
        _slot1 = _manager.SaveConfigToArray()
    End Sub

    Protected Sub OnSaveArray2(ByVal sender As Object, ByVal e As EventArgs)
        _slot2 = _manager.SaveConfigToArray()
    End Sub

    Protected Sub OnLoadArray1(ByVal sender As Object, ByVal e As EventArgs)
        If Not (_slot1 Is Nothing) Then
            _manager.LoadConfigFromArray(_slot1)
        End If
    End Sub

    Protected Sub OnLoadArray2(ByVal sender As Object, ByVal e As EventArgs)
        If Not (_slot2 Is Nothing) Then
            _manager.LoadConfigFromArray(_slot2)
        End If
    End Sub

    Protected Sub OnResizeBarDefault(ByVal sender As Object, ByVal e As EventArgs)
        _manager.ResizeBarVector = -1
    End Sub

    Protected Sub OnUpdateRSBD(ByVal sender As Object, ByVal e As EventArgs)
        Dim updateCommand As MenuCommand = sender
        updateCommand.Checked = (_manager.ResizeBarVector = -1)
    End Sub

    Protected Sub OnResizeBar1(ByVal sender As Object, ByVal e As EventArgs)
        _manager.ResizeBarVector = 1
    End Sub

    Protected Sub OnUpdateRSB1(ByVal sender As Object, ByVal e As EventArgs)
        Dim updateCommand As MenuCommand = sender
        updateCommand.Checked = (_manager.ResizeBarVector = 1)
    End Sub

    Protected Sub OnResizeBar5(ByVal sender As Object, ByVal e As EventArgs)
        _manager.ResizeBarVector = 5
    End Sub

    Protected Sub OnUpdateRSB5(ByVal sender As Object, ByVal e As EventArgs)
        Dim updateCommand As MenuCommand = sender
        updateCommand.Checked = (_manager.ResizeBarVector = 5)
    End Sub

    Protected Sub OnResizeBar7(ByVal sender As Object, ByVal e As EventArgs)
        _manager.ResizeBarVector = 7
    End Sub

    Protected Sub OnUpdateRSB7(ByVal sender As Object, ByVal e As EventArgs)
        Dim updateCommand As MenuCommand = sender
        updateCommand.Checked = (_manager.ResizeBarVector = 7)
    End Sub

    Protected Sub OnCaptionBars(ByVal sender As Object, ByVal e As EventArgs)
        _captionBars = (_captionBars = False)

        Dim c As Content
        For Each c In _manager.Contents
            c.CaptionBar = _captionBars
        Next
    End Sub

    Protected Sub OnUpdateCaptionBars(ByVal sender As Object, ByVal e As EventArgs)
        Dim updateCommand As MenuCommand = sender
        updateCommand.Checked = _captionBars
    End Sub

    Protected Sub OnCloseButtons(ByVal sender As Object, ByVal e As EventArgs)
        _closeButtons = (_closeButtons = False)

        Dim c As Content
        For Each c In _manager.Contents
            c.CloseButton = _closeButtons
        Next
    End Sub

    Protected Sub OnUpdateCloseButtons(ByVal sender As Object, ByVal e As EventArgs)
        Dim updateCommand As MenuCommand = sender
        updateCommand.Checked = _closeButtons
    End Sub

    Protected Sub OnAllowAllCloseButton(ByVal sender As Object, ByVal e As EventArgs)
        _ignoreClose = 0
    End Sub

    Protected Sub OnUpdateAllowAll(ByVal sender As Object, ByVal e As EventArgs)
        Dim updateCommand As MenuCommand = sender
        updateCommand.Checked = (_ignoreClose = 0)
    End Sub

    Protected Sub OnIgnoreAllCloseButton(ByVal sender As Object, ByVal e As EventArgs)
        _ignoreClose = 1
    End Sub

    Protected Sub OnUpdateIgnoreAll(ByVal sender As Object, ByVal e As EventArgs)
        Dim updateCommand As MenuCommand = sender
        updateCommand.Checked = (_ignoreClose = 1)
    End Sub

    Protected Sub OnIgnoreAllPanelButton(ByVal sender As Object, ByVal e As EventArgs)
        _ignoreClose = 2
    End Sub

    Protected Sub OnUpdateIgnorePanel(ByVal sender As Object, ByVal e As EventArgs)
        Dim updateCommand As MenuCommand = sender
        updateCommand.Checked = (_ignoreClose = 2)
    End Sub

    Protected Sub OnIgnoreAllFormButton(ByVal sender As Object, ByVal e As EventArgs)
        _ignoreClose = 3
    End Sub

    Protected Sub OnUpdateIgnoreForm(ByVal sender As Object, ByVal e As EventArgs)
        Dim updateCommand As MenuCommand = sender
        updateCommand.Checked = (_ignoreClose = 3)
    End Sub

    Protected Sub OnIgnoreAllTextboxButton(ByVal sender As Object, ByVal e As EventArgs)
        _ignoreClose = 4
    End Sub

    Protected Sub OnUpdateIgnoreTextBox(ByVal sender As Object, ByVal e As EventArgs)
        Dim updateCommand As MenuCommand = sender
        updateCommand.Checked = (_ignoreClose = 4)
    End Sub

    Protected Sub OnAllowMinMax(ByVal sender As Object, ByVal e As EventArgs)
        _manager.ZoneMinMax = (_manager.ZoneMinMax = False)
    End Sub

    Protected Sub OnUpdateAllowMinMax(ByVal sender As Object, ByVal e As EventArgs)
        Dim updateCommand As MenuCommand = sender
        updateCommand.Checked = _manager.ZoneMinMax
    End Sub

    Protected Sub OnPlainTabBorder(ByVal sender As Object, ByVal e As EventArgs)
        _manager.PlainTabBorder = (_manager.PlainTabBorder = False)
    End Sub

    Protected Sub OnUpdatePlainTabBorder(ByVal sender As Object, ByVal e As EventArgs)
        Dim updateCommand As MenuCommand = sender
        updateCommand.Checked = _manager.PlainTabBorder
    End Sub

    Protected Sub OnColorDefault(ByVal sender As Object, ByVal e As EventArgs)
        ' Remember the new color scheme selected
        _colorIndex = 0

        ' Define main Form back color
        _filler.TextColor = SystemColors.MenuText
        _filler.BackColor = SystemColors.Control
        Me.BackColor = SystemColors.Control

        ' Define the menu/status bars
        _topMenu.ResetColors()
        _topMenu.Font = SystemInformation.MenuFont
        _filler.Font = SystemInformation.MenuFont

        _manager.ResetColors()
        _manager.CaptionFont = SystemInformation.MenuFont
        _manager.TabControlFont = SystemInformation.MenuFont

        ' Need to manually update the colors of created Panels/Forms
        Dim c As Content
        For Each c In _manager.Contents
            DefineControlColors(c)
        Next
    End Sub

    Protected Sub OnUpdateDefault(ByVal sender As Object, ByVal e As EventArgs)
        Dim updateCommand As MenuCommand = sender
        updateCommand.Checked = (_colorIndex = 0)
    End Sub

    Protected Sub OnColorSlateBlue(ByVal sender As Object, ByVal e As EventArgs)
        ' Remember the new color scheme selected
        _colorIndex = 1

        ' Define main Form back color
        Me.BackColor = Color.DarkSlateBlue

        ' Define the menu/status bars
        _topMenu.TextColor = Color.White
        _topMenu.BackColor = Color.DarkSlateBlue
        _topMenu.SelectedTextColor = Color.White
        _topMenu.SelectedBackColor = Color.SteelBlue
        _topMenu.Font = New Font("Garamond", 10.0F)

        ' Define the TabControl appearance
        _filler.TextColor = Color.White
        _filler.BackColor = Color.DarkSlateBlue
        _filler.Font = New Font("Garamond", 10.0F)

        If _style = VisualStyle.IDE Then
            _topMenu.HighlightTextColor = Color.Black
            _topMenu.HighlightBackColor = Color.SlateGray
        Else
            _topMenu.HighlightTextColor = Color.White
            _topMenu.HighlightBackColor = Color.DarkSlateBlue
        End If

        ' Define docking window colors
        _manager.BackColor = Color.DarkSlateBlue
        _manager.InactiveTextColor = Color.White
        _manager.ActiveColor = Color.SteelBlue
        _manager.ActiveTextColor = Color.White
        _manager.ResizeBarColor = Color.DarkSlateBlue
        _manager.CaptionFont = New Font("Garamond", 10.0F)
        _manager.TabControlFont = New Font("Garamond", 10.0F)

        ' Need to manually update the colors of created Panels/Forms
        Dim c As Content
        For Each c In _manager.Contents
            DefineControlColors(c)
        Next
    End Sub

    Protected Sub OnUpdateSlateBlue(ByVal sender As Object, ByVal e As EventArgs)
        Dim updateCommand As MenuCommand = sender
        updateCommand.Checked = (_colorIndex = 1)
    End Sub

    Protected Sub OnColorFirebrick(ByVal sender As Object, ByVal e As EventArgs)
        ' Remember the new color scheme selected
        _colorIndex = 2

        Me.BackColor = Color.Firebrick

        ' Define the menu/status bars
        _topMenu.TextColor = Color.White
        _topMenu.BackColor = Color.Firebrick
        _topMenu.HighlightTextColor = Color.White
        _topMenu.HighlightBackColor = Color.Firebrick
        _topMenu.SelectedBackColor = Color.Orange
        _topMenu.Font = New Font("Sans Serif", 12.0F)

        ' Define the TabControl appearance
        _filler.TextColor = Color.White
        _filler.BackColor = Color.Firebrick
        _filler.Font = New Font("Sans Serif", 12.0F)

        If _style = VisualStyle.IDE Then
            _topMenu.SelectedTextColor = Color.Black
        Else
            _topMenu.SelectedTextColor = Color.White
        End If

        ' Define docking window colors
        _manager.BackColor = Color.Firebrick
        _manager.InactiveTextColor = Color.White
        _manager.ActiveColor = Color.Orange
        _manager.ActiveTextColor = Color.Black
        _manager.ResizeBarColor = Color.Firebrick
        _manager.CaptionFont = New Font("Sans Serif", 12.0F)
        _manager.TabControlFont = New Font("Sans Serif", 10.0F)

        ' Need to manually update the colors of created Panels/Forms
        Dim c As Content
        For Each c In _manager.Contents
            DefineControlColors(c)
        Next
    End Sub

    Protected Sub OnUpdateFirebrick(ByVal sender As Object, ByVal e As EventArgs)
        Dim updateCommand As MenuCommand = sender
        updateCommand.Checked = (_colorIndex = 2)
    End Sub

    Protected Sub OnColorGreen(ByVal sender As Object, ByVal e As EventArgs)
        ' Remember the new color scheme selected
        _colorIndex = 3

        ' Define main Form back color
        Me.BackColor = Color.PaleGreen

        ' Define the menu/status bars
        _topMenu.TextColor = Color.Black
        _topMenu.BackColor = Color.PaleGreen
        _topMenu.SelectedBackColor = Color.DarkSlateGray
        _topMenu.Font = New Font("Arial", 9.0F)

        ' Define the TabControl appearance
        _filler.TextColor = Color.Black
        _filler.BackColor = Color.PaleGreen
        _filler.Font = New Font("Arial", 9.0F)

        If _style = VisualStyle.IDE Then
            _topMenu.HighlightTextColor = Color.Black
            _topMenu.HighlightBackColor = Color.PaleGreen
            _topMenu.SelectedTextColor = Color.White
        Else
            _topMenu.HighlightTextColor = Color.White
            _topMenu.HighlightBackColor = Color.DarkSlateGray
            _topMenu.SelectedBackColor = Color.PaleGreen
        End If

        ' Define docking window colors
        _manager.BackColor = Color.PaleGreen
        _manager.InactiveTextColor = Color.Black
        _manager.ActiveColor = Color.DarkSlateGray
        _manager.ActiveTextColor = Color.White
        _manager.ResizeBarColor = Color.DarkSeaGreen
        _manager.CaptionFont = New Font("Arial", 9.0F)
        _manager.TabControlFont = New Font("Arial", 9.0F)

        ' Need to manually update the colors of created Panels/Forms
        Dim c As Content
        For Each c In _manager.Contents
            DefineControlColors(c)
        Next
    End Sub

    Protected Sub OnUpdateGreen(ByVal sender As Object, ByVal e As EventArgs)
        Dim updateCommand As MenuCommand = sender
        updateCommand.Checked = (_colorIndex = 3)
    End Sub

    Protected Sub OnTabsMultiBox(ByVal sender As Object, ByVal e As EventArgs)
        _tabAppearance = Crownwood.Magic.Controls.TabControl.VisualAppearance.MultiBox
        UpdateAllTabControls()
    End Sub

    Protected Sub OnUpdateTabsBox(ByVal sender As Object, ByVal e As EventArgs)
        Dim updateCommand As MenuCommand = sender
        updateCommand.Checked = (_tabAppearance = Crownwood.Magic.Controls.TabControl.VisualAppearance.MultiBox)
    End Sub

    Protected Sub OnTabsMultiForm(ByVal sender As Object, ByVal e As EventArgs)
        _tabAppearance = Crownwood.Magic.Controls.TabControl.VisualAppearance.MultiForm
        UpdateAllTabControls()
    End Sub

    Protected Sub OnUpdateTabsForm(ByVal sender As Object, ByVal e As EventArgs)
        Dim updateCommand As MenuCommand = sender
        updateCommand.Checked = (_tabAppearance = Crownwood.Magic.Controls.TabControl.VisualAppearance.MultiForm)
    End Sub

    Protected Sub OnTabsMultiDocument(ByVal sender As Object, ByVal e As EventArgs)
        _tabAppearance = Crownwood.Magic.Controls.TabControl.VisualAppearance.MultiDocument
        UpdateAllTabControls()
    End Sub

    Protected Sub OnUpdateTabsDocument(ByVal sender As Object, ByVal e As EventArgs)
        Dim updateCommand As MenuCommand = sender
        updateCommand.Checked = (_tabAppearance = Crownwood.Magic.Controls.TabControl.VisualAppearance.MultiDocument)
    End Sub

    Protected Sub OnTabsTop(ByVal sender As Object, ByVal e As EventArgs)
        _tabsBottom = False
        UpdateAllTabControls()
    End Sub

    Protected Sub OnUpdateTabsTop(ByVal sender As Object, ByVal e As EventArgs)
        Dim updateCommand As MenuCommand = sender
        updateCommand.Checked = Not _tabsBottom
    End Sub

    Protected Sub OnTabsBottom(ByVal sender As Object, ByVal e As EventArgs)
        _tabsBottom = True
        UpdateAllTabControls()
    End Sub

    Protected Sub UpdateAllTabControls()
        Dim c As Content
        For Each c In _manager.Contents
            If Not (c.ParentWindowContent Is Nothing) Then
                Dim wct As WindowContentTabbed = c.ParentWindowContent

                If Not (wct Is Nothing) Then
                    wct.TabControl.Appearance = _tabAppearance
                    wct.TabControl.PositionTop = Not _tabsBottom
                End If
            End If
        Next
    End Sub

    Protected Sub OnUpdateTabsBottom(ByVal sender As Object, ByVal e As EventArgs)
        Dim updateCommand As MenuCommand = sender
        updateCommand.Checked = _tabsBottom
    End Sub

    Protected Sub OnRTBDisposed(ByVal sender As Object, ByVal e As EventArgs)
        Console.WriteLine("OnRTBDisposed")
    End Sub

    Protected Sub OnSaveConfig(ByVal xmlOut As XmlTextWriter)
        ' Add an extra node into the config to store some useless information
        xmlOut.WriteStartElement("MyCustomElement")
        xmlOut.WriteAttributeString("UselessData1", "Hello")
        xmlOut.WriteAttributeString("UselessData2", "World!")
        xmlOut.WriteEndElement()
    End Sub

    Protected Sub OnLoadConfig(ByVal xmlIn As XmlTextReader)
        ' We are expecting our custom element to be the current one
        If xmlIn.Name = "MyCustomElement" Then
            ' Read in both the expected attributes
            Dim attr1 As String = xmlIn.GetAttribute(0)
            Dim attr2 As String = xmlIn.GetAttribute(1)

            ' Must move past our element
            xmlIn.Read()
        End If
    End Sub

End Class

Public Class DummyForm
    Inherits Form

    Private _dummy1 As Button = New Button()
    Private _dummy2 As Button = New Button()
    Private _dummyBox As GroupBox = New GroupBox()
    Private _dummy3 As RadioButton = New RadioButton()
    Private _dummy4 As RadioButton = New RadioButton()

    Sub New()
        _dummy1.Text = "Button 1"
        _dummy1.Size = New Size(90, 25)
        _dummy1.Location = New Point(10, 10)

        _dummy2.Text = "Button 2"
        _dummy2.Size = New Size(90, 25)
        _dummy2.Location = New Point(110, 10)

        _dummyBox.Text = "Form GroupBox"
        _dummyBox.Size = New Size(125, 67)
        _dummyBox.Location = New Point(10, 45)

        _dummy3.Text = "RadioButton 3"
        _dummy3.Size = New Size(110, 22)
        _dummy3.Location = New Point(10, 20)

        _dummy4.Text = "RadioButton 4"
        _dummy4.Size = New Size(110, 22)
        _dummy4.Location = New Point(10, 42)
        _dummy4.Checked = True

        _dummyBox.Controls.AddRange(New Control() {_dummy3, _dummy4})

        Controls.AddRange(New Control() {_dummy1, _dummy2, _dummyBox})

        Dim c As Component = Me
        AddHandler c.Disposed, AddressOf OnFormDisposed

        ' Define then control to be selected when activated for first time
        Me.ActiveControl = _dummy4
    End Sub

    Protected Sub OnFormDisposed(ByVal sender As Object, ByVal e As EventArgs)
        Console.WriteLine("OnFormDisposed")
    End Sub

End Class

Public Class DummyPanel
    Inherits Panel

    Private _dummyBox As GroupBox = New GroupBox()
    Private _dummy3 As RadioButton = New RadioButton()
    Private _dummy4 As RadioButton = New RadioButton()

    Sub New()
        _dummyBox.Text = "Panel GroupBox"
        _dummyBox.Size = New Size(125, 67)
        _dummyBox.Location = New Point(10, 10)

        _dummy3.Text = "RadioButton 3"
        _dummy3.Size = New Size(110, 22)
        _dummy3.Location = New Point(10, 20)

        _dummy4.Text = "RadioButton 4"
        _dummy4.Size = New Size(110, 22)
        _dummy4.Location = New Point(10, 42)
        _dummy4.Checked = True

        _dummyBox.Controls.AddRange(New Control() {_dummy3, _dummy4})

        Dim c As Component = Me
        AddHandler c.Disposed, AddressOf OnPanelDisposed

        Controls.AddRange(New Control() {_dummyBox})
    End Sub

    Protected Sub OnPanelDisposed(ByVal sender As Object, ByVal e As EventArgs)
        Console.WriteLine("OnPanelDisposed")
    End Sub
End Class
