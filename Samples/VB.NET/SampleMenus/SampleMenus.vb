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
Imports System.IO
Imports System.Data
Imports System.Drawing
Imports System.Resources
Imports System.Reflection
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Drawing.Drawing2D
Imports Crownwood.Magic.Menus
Imports Crownwood.Magic.Common
Imports Crownwood.Magic.Controls

Public Class MDIContainer
    Inherits System.Windows.Forms.Form

    Private _count As Integer = 1
    Private _images As ImageList = Nothing
    Private _status As StatusBar = Nothing
    Private _statusBarPanel As StatusBarPanel = Nothing
    Private _topMenu As Crownwood.Magic.Menus.MenuControl = Nothing

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        LoadResources()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        SetupMenus()
        SetupStatusBar()
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
    'It can be modified Imports the Windows Form Designer.  
    'Do not modify it Imports the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        components = New System.ComponentModel.Container()
        Me.BackColor = System.Drawing.SystemColors.ControlLight
        Me.ClientSize = New System.Drawing.Size(250, 270)
        Me.Name = "MDIContainer"
        Me.Text = "SampleMenus"
        Me.IsMdiContainer = True
    End Sub

#End Region

    Shared Sub Main()
        Application.Run(New MDIContainer())
    End Sub

    Protected Sub LoadResources()
        ' Create a strip of images by loading an embedded bitmap resource
        _images = ResourceHelper.LoadBitmapStrip(Me.GetType(), _
                                                 "SampleMenus.MenuImages.bmp", _
                                                 New Size(16, 16), _
                                                 New Point(0, 0))
    End Sub

    Protected Sub SetupMenus()
        ' Create the MenuControl
        _topMenu = New Crownwood.Magic.Menus.MenuControl()

        ' We want the control to handle the MDI pendant
        _topMenu.MdiContainer = Me

        ' Create the top level Menu
        Dim top1 As MenuCommand = New MenuCommand("&Appearance")
        Dim top2 As MenuCommand = New MenuCommand("&Windows")
        Dim top3 As MenuCommand = New MenuCommand("A&nimation")
        Dim top4 As MenuCommand = New MenuCommand("&Cities1")
        Dim top5 As MenuCommand = New MenuCommand("&Movies1")
        Dim top6 As MenuCommand = New MenuCommand("Ca&rs1")
        Dim top7 As MenuCommand = New MenuCommand("C&ities2")
        Dim top8 As MenuCommand = New MenuCommand("Mo&vies2")
        Dim top9 As MenuCommand = New MenuCommand("Car&s2")
        _topMenu.MenuCommands.AddRange(New MenuCommand() {top1, top2, top3, top4, top5, top6, top7, top8, top9})

        ' Create the submenus
        CreateAppearanceMenu(top1)
        CreateWindowsMenu(top2)
        CreateAnimationMenu(top3)
        CreateCityMenus(top4, top7)
        CreateMovieMenus(top5, top8)
        CreateCarMenus(top6, top9)

        ' Add to the display
        _topMenu.Dock = DockStyle.Top

        AddHandler _topMenu.Selected, AddressOf OnSelected
        AddHandler _topMenu.Deselected, AddressOf OnDeselected

        Controls.Add(_topMenu)

        ' Create an initial MDI child window
        OnNewWindowSelected(Nothing, EventArgs.Empty)
    End Sub

    Protected Sub CreateAppearanceMenu(ByVal mc As MenuCommand)
        ' Create menu commands
        Dim style1 As MenuCommand = New MenuCommand("&IDE", New EventHandler(AddressOf OnIDESelected))
        Dim style2 As MenuCommand = New MenuCommand("&Plain", New EventHandler(AddressOf OnPlainSelected))
        Dim style3 As MenuCommand = New MenuCommand("-")
        Dim style4 As MenuCommand = New MenuCommand("PlainAsBlock", New EventHandler(AddressOf OnPlainAsBlockSelected))
        Dim style5 As MenuCommand = New MenuCommand("-")
        Dim style6 As MenuCommand = New MenuCommand("Dock Left", New EventHandler(AddressOf OnDockLeftSelected))
        Dim style7 As MenuCommand = New MenuCommand("Dock Top", New EventHandler(AddressOf OnDockTopSelected))
        Dim style8 As MenuCommand = New MenuCommand("Dock Right", New EventHandler(AddressOf OnDockRightSelected))
        Dim style9 As MenuCommand = New MenuCommand("Dock Bottom", New EventHandler(AddressOf OnDockBottomSelected))
        Dim styleA As MenuCommand = New MenuCommand("-")
        Dim styleB As MenuCommand = New MenuCommand("MultiLine", New EventHandler(AddressOf OnMultiLineSelected))
        Dim styleC As MenuCommand = New MenuCommand("-")
        Dim styleD As MenuCommand = New MenuCommand("E&xit", New EventHandler(AddressOf OnExit))

        ' Setup event handlers
        AddHandler style1.Update, AddressOf OnIDEUpdate
        AddHandler style2.Update, AddressOf OnPlainUpdate
        AddHandler style4.Update, AddressOf OnPlainAsBlockUpdate
        AddHandler style6.Update, AddressOf OnDockLeftUpdate
        AddHandler style7.Update, AddressOf OnDockTopUpdate
        AddHandler style8.Update, AddressOf OnDockRightUpdate
        AddHandler style9.Update, AddressOf OnDockBottomUpdate
        AddHandler styleB.Update, AddressOf OnMultiLineUpdate

        mc.MenuCommands.AddRange(New MenuCommand() {style1, style2, style3, style4, style5, style6, _
                                                   style7, style8, style9, styleA, styleB, styleC, styleD})
    End Sub

    Protected Sub CreateWindowsMenu(ByVal mc As MenuCommand)
        ' Create menu commands
        Dim window1 As MenuCommand = New MenuCommand("&New Window", _images, 0, New EventHandler(AddressOf OnNewWindowSelected))
        Dim window2 As MenuCommand = New MenuCommand("Cl&ose", _images, 1, New EventHandler(AddressOf OnCloseWindowSelected))
        Dim window3 As MenuCommand = New MenuCommand("Close A&ll", New EventHandler(AddressOf OnCloseAllSelected))
        Dim window4 As MenuCommand = New MenuCommand("-")
        Dim window5 As MenuCommand = New MenuCommand("Ne&xt", _images, 2, New EventHandler(AddressOf OnNextSelected))
        Dim window6 As MenuCommand = New MenuCommand("Pre&vious", _images, 3, New EventHandler(AddressOf OnPreviousSelected))
        Dim window7 As MenuCommand = New MenuCommand("-")
        Dim window8 As MenuCommand = New MenuCommand("&Cascade", _images, 4, New EventHandler(AddressOf OnCascadeSelected))
        Dim window9 As MenuCommand = New MenuCommand("Tile &Horizontally", _images, 5, New EventHandler(AddressOf OnTileHSelected))
        Dim windowA As MenuCommand = New MenuCommand("&Tile Vertically", _images, 6, New EventHandler(AddressOf OnTileVSelected))

        ' Setup event handlers
        AddHandler window2.Update, AddressOf OnCloseWindowUpdate
        AddHandler window2.Update, AddressOf OnCloseAllUpdate
        AddHandler window5.Update, AddressOf OnNextPreviousUpdate
        AddHandler window6.Update, AddressOf OnNextPreviousUpdate
        AddHandler window8.Update, AddressOf OnLayoutUpdate
        AddHandler window9.Update, AddressOf OnLayoutUpdate
        AddHandler windowA.Update, AddressOf OnLayoutUpdate

        mc.MenuCommands.AddRange(New MenuCommand() {window1, window2, window3, window4, _
                                                   window5, window6, window7, window8, _
                                                   window9, windowA})

        ' Want to know when MenuControl shows/hide PopupMenu
        AddHandler mc.PopupStart, AddressOf OnWindowMenuStart
        AddHandler mc.PopupEnd, AddressOf OnWindowMenuEnd
    End Sub

    Protected Sub CreateAnimationMenu(ByVal mc As MenuCommand)
        ' Create menu commands
        Dim animate1 As MenuCommand = New MenuCommand("Yes - Always animate", New EventHandler(AddressOf OnYesAnimateSelected))
        Dim animate2 As MenuCommand = New MenuCommand("No  - Never animate", New EventHandler(AddressOf OnNoAnimateSelected))
        Dim animate3 As MenuCommand = New MenuCommand("System - Ask O/S", New EventHandler(AddressOf OnSystemAnimateSelected))
        Dim animate4 As MenuCommand = New MenuCommand("-")
        Dim animate5 As MenuCommand = New MenuCommand("100ms", New EventHandler(AddressOf On100Selected))
        Dim animate6 As MenuCommand = New MenuCommand("250ms", New EventHandler(AddressOf On250Selected))
        Dim animate7 As MenuCommand = New MenuCommand("1000ms", New EventHandler(AddressOf On1000Selected))
        Dim animate8 As MenuCommand = New MenuCommand("-")
        Dim animate9 As MenuCommand = New MenuCommand("Blend", New EventHandler(AddressOf OnBlendSelected))
        Dim animateA As MenuCommand = New MenuCommand("Center", New EventHandler(AddressOf OnCenterSelected))
        Dim animateB As MenuCommand = New MenuCommand("+Hor +Ver", New EventHandler(AddressOf OnPPSelected))
        Dim animateC As MenuCommand = New MenuCommand("-Hor -Ver", New EventHandler(AddressOf OnNNSelected))
        Dim animateD As MenuCommand = New MenuCommand("+Hor -Ver", New EventHandler(AddressOf OnPNSelected))
        Dim animateE As MenuCommand = New MenuCommand("-Hor +Ver", New EventHandler(AddressOf OnNPSelected))
        Dim animateF As MenuCommand = New MenuCommand("System", New EventHandler(AddressOf OnSystemSelected))

        ' Setup event handlers
        AddHandler animate1.Update, AddressOf OnYesAnimateUpdate
        AddHandler animate2.Update, AddressOf OnNoAnimateUpdate
        AddHandler animate3.Update, AddressOf OnSystemAnimateUpdate
        AddHandler animate5.Update, AddressOf On100Update
        AddHandler animate6.Update, AddressOf On250Update
        AddHandler animate7.Update, AddressOf On1000Update
        AddHandler animate9.Update, AddressOf OnBlendUpdate
        AddHandler animateA.Update, AddressOf OnCenterUpdate
        AddHandler animateB.Update, AddressOf OnPPUpdate
        AddHandler animateC.Update, AddressOf OnNNUpdate
        AddHandler animateD.Update, AddressOf OnPNUpdate
        AddHandler animateE.Update, AddressOf OnNPUpdate
        AddHandler animateF.Update, AddressOf OnSystemUpdate

        mc.MenuCommands.AddRange(New MenuCommand() {animate1, animate2, animate3, animate4, _
                                                   animate5, animate6, animate7, animate8, _
                                                   animate9, animateA, animateB, animateC, _
                                                   animateD, animateE, animateF})
    End Sub

    Protected Sub CreateCarMenus(ByVal mc1 As MenuCommand, ByVal mc2 As MenuCommand)
        ' Create menu commands
        Dim car1 As MenuCommand = New MenuCommand("Ford", _images, 0)
        Dim car2 As MenuCommand = New MenuCommand("Vauxhall", _images, 1)
        Dim car3 As MenuCommand = New MenuCommand("Opel", _images, 2)
        Dim car4 As MenuCommand = New MenuCommand("Volvo", _images, 5)
        Dim car5 As MenuCommand = New MenuCommand("Lotus", _images, 6, Shortcut.Alt0)
        Dim car6 As MenuCommand = New MenuCommand("Aston Martin", _images, 0, Shortcut.ShiftF1)
        Dim car7 As MenuCommand = New MenuCommand("Ferrari", _images, 1, Shortcut.CtrlShift0)
        Dim car8 As MenuCommand = New MenuCommand("Jaguar", _images, 2, Shortcut.ShiftIns)

        ' Change default properties of some items
        car2.Enabled = False
        car3.Enabled = False
        car4.Break = True
        car6.Infrequent = True
        car5.Infrequent = True

        mc1.MenuCommands.AddRange(New MenuCommand() {car1, car2, car3, car4, car5, car6, car7, car8})
        mc2.MenuCommands.AddRange(New MenuCommand() {car1, car2, car3, car4, car5, car6, car7, car8})
    End Sub

    Protected Sub CreateCityMenus(ByVal mc1 As MenuCommand, ByVal mc2 As MenuCommand)
        ' Create menu commands
        Dim s0 As MenuCommand = New MenuCommand("&Italy", _images, 0, New EventHandler(AddressOf OnGenericSelect))
        Dim s1 As MenuCommand = New MenuCommand("&Spain", _images, 1, New EventHandler(AddressOf OnGenericSelect))
        Dim s2 As MenuCommand = New MenuCommand("&Canada", _images, 2, New EventHandler(AddressOf OnGenericSelect))
        Dim s3 As MenuCommand = New MenuCommand("&France", _images, 3, New EventHandler(AddressOf OnGenericSelect))
        Dim s4 As MenuCommand = New MenuCommand("&Belgium", _images, 4, New EventHandler(AddressOf OnGenericSelect))
        Dim spain0 As MenuCommand = New MenuCommand("&Nerja", _images, 5, New EventHandler(AddressOf OnGenericSelect))
        Dim spain1 As MenuCommand = New MenuCommand("&Madrid", _images, 6, New EventHandler(AddressOf OnGenericSelect))
        Dim spain2 As MenuCommand = New MenuCommand("&Barcelona", _images, 0, New EventHandler(AddressOf OnGenericSelect))
        Dim canada0 As MenuCommand = New MenuCommand("Toronto", _images, 5, New EventHandler(AddressOf OnGenericSelect))
        Dim canada1 As MenuCommand = New MenuCommand("&Montreal", _images, 6, New EventHandler(AddressOf OnGenericSelect))
        Dim canada2 As MenuCommand = New MenuCommand("&Belleville", _images, 0, New EventHandler(AddressOf OnGenericSelect))
        Dim england As MenuCommand = New MenuCommand("England", _images, 2, New EventHandler(AddressOf OnGenericSelect))
        Dim england1 As MenuCommand = New MenuCommand("London", _images, 5, New EventHandler(AddressOf OnGenericSelect))
        Dim england2 As MenuCommand = New MenuCommand("&Birmingham", _images, 6, New EventHandler(AddressOf OnGenericSelect))
        Dim england3 As MenuCommand = New MenuCommand("&Nottingham", _images, 0, New EventHandler(AddressOf OnGenericSelect))

        ' Define hierarchy
        england.MenuCommands.AddRange(New MenuCommand() {england1, england2, england3})
        s1.MenuCommands.AddRange(New MenuCommand() {spain0, spain1, spain2})
        s2.MenuCommands.AddRange(New MenuCommand() {canada0, canada1, canada2, england})
        mc1.MenuCommands.AddRange(New MenuCommand() {s0, s1, s2, s3, s4})
        mc2.MenuCommands.AddRange(New MenuCommand() {s0, s1, s2, s3, s4})

        ' Change default properties of some items
        spain0.Infrequent = True
        spain1.Infrequent = True

        Dim fs As FontStyle = FontStyle.Bold + FontStyle.Italic

        ' Setup the left column details
        england.MenuCommands.ExtraText = "English"
        england.MenuCommands.ExtraTextColor = Color.White
        england.MenuCommands.ExtraBackColor = Color.DarkBlue
        england.MenuCommands.ExtraFont = New Font("Times New Roman", 12.0F, fs)
        s1.MenuCommands.ExtraText = "Spanish"
        s1.MenuCommands.ExtraTextColor = Color.DarkRed
        s1.MenuCommands.ExtraBackColor = Color.Orange
        s1.MenuCommands.ExtraFont = New Font("Times New Roman", 12.0F, fs)
        s2.MenuCommands.ExtraText = "Canadian"
        s2.MenuCommands.ExtraTextColor = Color.White
        s2.MenuCommands.ExtraBackColor = Color.DarkRed
        s2.MenuCommands.ExtraFont = New Font("Times New Roman", 12.0F, fs)
        mc1.MenuCommands.ExtraText = "Countries"
        mc1.MenuCommands.ExtraTextColor = Color.White
        mc1.MenuCommands.ExtraBackColor = Color.SlateGray
        mc1.MenuCommands.ExtraFont = New Font("Times New Roman", 12.0F, fs)
    End Sub

    Protected Sub CreateMovieMenus(ByVal mc1 As MenuCommand, ByVal mc2 As MenuCommand)
        ' Create menu commands
        Dim movie0 As MenuCommand = New MenuCommand("Dr No", _images, 0, New EventHandler(AddressOf OnGenericSelect))
        Dim movie1 As MenuCommand = New MenuCommand("Goldfinger", _images, 1, New EventHandler(AddressOf OnGenericSelect))
        Dim movie2 As MenuCommand = New MenuCommand("Goldeneye", _images, 2, New EventHandler(AddressOf OnGenericSelect))
        Dim movie3 As MenuCommand = New MenuCommand("-")
        Dim movie4 As MenuCommand = New MenuCommand("Live and Let Die", _images, 3, New EventHandler(AddressOf OnGenericSelect))
        Dim movie5 As MenuCommand = New MenuCommand("Man with the Golden Gun", _images, 4, New EventHandler(AddressOf OnGenericSelect))
        Dim movie6 As MenuCommand = New MenuCommand("License Revoked", _images, 5, New EventHandler(AddressOf OnGenericSelect))
        Dim movie7 As MenuCommand = New MenuCommand("Diamonds are Forever", _images, 6, New EventHandler(AddressOf OnGenericSelect))
        Dim movie8 As MenuCommand = New MenuCommand("From Russia with Love", _images, 0, New EventHandler(AddressOf OnGenericSelect))

        ' Change default properties of some items
        movie0.Infrequent = True
        movie1.Infrequent = True
        movie5.Infrequent = True
        movie7.Infrequent = True
        movie8.Infrequent = True

        mc1.MenuCommands.AddRange(New MenuCommand() {movie0, movie1, movie2, movie3, movie4, movie5, movie6, movie7, movie8})
        mc2.MenuCommands.AddRange(New MenuCommand() {movie0, movie1, movie2, movie3, movie4, movie5, movie6, movie7, movie8})

        ' Setup the left column details
        mc1.MenuCommands.ExtraText = "Bond Films"
        mc1.MenuCommands.ExtraFont = New Font("Garamond", 12.0F, FontStyle.Bold)
        mc1.MenuCommands.ExtraBackBrush = New LinearGradientBrush(New Point(0, 0), New Point(100, 100), _
                                                                  Color.LightGreen, Color.DarkGreen)
    End Sub

    Protected Sub SetupStatusBar()
        ' Create and setup the StatusBar object
        _status = New StatusBar()
        _status.Dock = DockStyle.Bottom
        _status.ShowPanels = True

        ' Create and setup a single panel for the StatusBar
        _statusBarPanel = New StatusBarPanel()
        _statusBarPanel.AutoSize = StatusBarPanelAutoSize.Spring
        _status.Panels.Add(_statusBarPanel)

        Controls.Add(_status)
    End Sub

    Public Sub SetStatusBarText(ByVal text As String)
        _statusBarPanel.Text = text
    End Sub

    Public ReadOnly Property Images() As ImageList
        Get
            Images = _images
        End Get
    End Property

    Public ReadOnly Property Style() As VisualStyle
        Get
            Style = _topMenu.Style
        End Get
    End Property

    Protected Sub OnSelected(ByVal mc As MenuCommand)
        SetStatusBarText("Selection over " & mc.Description)
    End Sub

    Protected Sub OnDeselected(ByVal mc As MenuCommand)
        SetStatusBarText("")
    End Sub

    Protected Sub OnMenuItemSelected(ByVal name As String)
        Dim child As MDIChild = Me.ActiveMdiChild

        If Not (child Is Nothing) Then
            child.AppendText(name)
        End If
    End Sub

    Protected Sub OnGenericSelect(ByVal sender As Object, ByVal e As EventArgs)
        Dim mc As MenuCommand = sender
        OnMenuItemSelected(mc.Text)
    End Sub

    Protected Sub OnIDEUpdate(ByVal sender As Object, ByVal e As EventArgs)
        Dim mc As MenuCommand = sender
        mc.Checked = (_topMenu.Style = VisualStyle.IDE)
    End Sub

    Protected Sub OnIDESelected(ByVal sender As Object, ByVal e As EventArgs)
        _topMenu.Style = VisualStyle.IDE
        OnMenuItemSelected("IDE")
    End Sub

    Protected Sub OnPlainUpdate(ByVal sender As Object, ByVal e As EventArgs)
        Dim mc As MenuCommand = sender
        mc.Checked = (_topMenu.Style = VisualStyle.Plain)
    End Sub

    Protected Sub OnPlainSelected(ByVal sender As Object, ByVal e As EventArgs)
        _topMenu.Style = VisualStyle.Plain
        OnMenuItemSelected("Plain")
    End Sub

    Protected Sub OnPlainAsBlockUpdate(ByVal sender As Object, ByVal e As EventArgs)
        Dim mc As MenuCommand = sender
        mc.Checked = _topMenu.PlainAsBlock
    End Sub

    Protected Sub OnPlainAsBlockSelected(ByVal sender As Object, ByVal e As EventArgs)
        _topMenu.PlainAsBlock = Not _topMenu.PlainAsBlock
        OnMenuItemSelected("PlainAsBlock")
    End Sub

    Protected Sub OnMultiLineUpdate(ByVal sender As Object, ByVal e As EventArgs)
        Dim mc As MenuCommand = sender
        mc.Checked = _topMenu.MultiLine
    End Sub

    Protected Sub OnMultiLineSelected(ByVal sender As Object, ByVal e As EventArgs)
        _topMenu.MultiLine = Not _topMenu.MultiLine
        OnMenuItemSelected("MultiLine")
    End Sub

    Protected Sub OnDockLeftSelected(ByVal sender As Object, ByVal e As EventArgs)
        _topMenu.Dock = DockStyle.Left
        OnMenuItemSelected("DockLeft")
    End Sub

    Protected Sub OnDockLeftUpdate(ByVal sender As Object, ByVal e As EventArgs)
        Dim mc As MenuCommand = sender
        mc.Checked = (_topMenu.Dock = DockStyle.Left)
    End Sub

    Protected Sub OnDockTopSelected(ByVal sender As Object, ByVal e As EventArgs)
        _topMenu.Dock = DockStyle.Top
        OnMenuItemSelected("DockTop")
    End Sub

    Protected Sub OnDockTopUpdate(ByVal sender As Object, ByVal e As EventArgs)
        Dim mc As MenuCommand = sender
        mc.Checked = (_topMenu.Dock = DockStyle.Top)
    End Sub

    Protected Sub OnDockRightSelected(ByVal sender As Object, ByVal e As EventArgs)
        _topMenu.Dock = DockStyle.Right
        OnMenuItemSelected("DockRight")
    End Sub

    Protected Sub OnDockRightUpdate(ByVal sender As Object, ByVal e As EventArgs)
        Dim mc As MenuCommand = sender
        mc.Checked = (_topMenu.Dock = DockStyle.Right)
    End Sub

    Protected Sub OnDockBottomSelected(ByVal sender As Object, ByVal e As EventArgs)
        _topMenu.Dock = DockStyle.Bottom
        OnMenuItemSelected("DockBottom")
    End Sub

    Protected Sub OnDockBottomUpdate(ByVal sender As Object, ByVal e As EventArgs)
        Dim mc As MenuCommand = sender
        mc.Checked = (_topMenu.Dock = DockStyle.Bottom)
    End Sub

    Protected Sub OnExit(ByVal sender As Object, ByVal e As EventArgs)
        Close()
    End Sub

    Protected Sub OnNewWindowSelected(ByVal sender As Object, ByVal e As EventArgs)
        Dim child As MDIChild = New MDIChild(Me)

        child.MdiParent = Me
        child.Size = New Size(130, 130)
        child.Text = "Child" & _count
        child.Show()

        _count += 1

        OnMenuItemSelected("NewWindow")
    End Sub

    Protected Sub OnCloseWindowUpdate(ByVal sender As Object, ByVal e As EventArgs)
        Dim mc As MenuCommand = sender
        mc.Enabled = Not (Me.ActiveMdiChild Is Nothing)
    End Sub

    Protected Sub OnCloseWindowSelected(ByVal sender As Object, ByVal e As EventArgs)
        ' Close just the curren mdi child window
        Me.ActiveMdiChild.Close()
        OnMenuItemSelected("CloseWindow")
    End Sub

    Protected Sub OnCloseAllUpdate(ByVal sender As Object, ByVal e As EventArgs)
        Dim mc As MenuCommand = sender
        mc.Enabled = Not (Me.ActiveMdiChild Is Nothing)
    End Sub

    Protected Sub OnCloseAllSelected(ByVal sender As Object, ByVal e As EventArgs)
        Dim mdiCommand As MenuCommand = sender

        Dim child As MDIChild
        For Each child In Controls
            child.Close()
        Next

        OnMenuItemSelected("CloseAll")
    End Sub

    Protected Sub OnNextSelected(ByVal sender As Object, ByVal e As EventArgs)
        Dim current As Form = Me.ActiveMdiChild

        If Not (current Is Nothing) Then
            ' Get collectiom of Mdi child windows
            Dim children As Form() = Me.MdiChildren

            ' Find position of the window after the current one
            Dim newPos As Integer = Array.LastIndexOf(children, current) + 1

            ' Check for moving off the end of list, wrap back to start
            If (newPos = children.Length) Then
                newPos = 0
            End If

            children(newPos).Activate()
        End If

        OnMenuItemSelected("Next")
    End Sub

    Protected Sub OnPreviousSelected(ByVal sender As Object, ByVal e As EventArgs)
        Dim current As Form = Me.ActiveMdiChild

        If Not (current Is Nothing) Then
            ' Get collectiom of Mdi child windows
            Dim children As Form() = Me.MdiChildren

            ' Find position of the window after the current one
            Dim newPos As Integer = Array.LastIndexOf(children, current) - 1

            ' Check for moving off the start of list, wrap back to end
            If (newPos < 0) Then
                newPos = children.Length - 1
            End If

            children(newPos).Activate()
        End If

        OnMenuItemSelected("Previous")
    End Sub

    Protected Sub OnNextPreviousUpdate(ByVal sender As Object, ByVal e As EventArgs)
        Dim mc As MenuCommand = sender
        mc.Enabled = (Me.MdiChildren.Length > 1)
    End Sub

    Protected Sub OnCascadeSelected(ByVal sender As Object, ByVal e As EventArgs)
        Me.LayoutMdi(MdiLayout.Cascade)
        OnMenuItemSelected("Cascade")
    End Sub

    Protected Sub OnTileHSelected(ByVal sender As Object, ByVal e As EventArgs)
        Me.LayoutMdi(MdiLayout.TileHorizontal)
        OnMenuItemSelected("TileH")
    End Sub

    Protected Sub OnTileVSelected(ByVal sender As Object, ByVal e As EventArgs)
        Me.LayoutMdi(MdiLayout.TileVertical)
        OnMenuItemSelected("TileV")
    End Sub

    Protected Sub OnLayoutUpdate(ByVal sender As Object, ByVal e As EventArgs)
        Dim mc As MenuCommand = sender
        mc.Enabled = (Me.MdiChildren.Length > 0)
    End Sub

    Protected Sub OnWindowMenuStart(ByVal mc As MenuCommand)
        Dim current As Form = Me.ActiveMdiChild

        ' Get collectiom of Mdi child windows
        Dim children As Form() = Me.MdiChildren

        If (children.Length > 0) Then
            ' Add a separator to the menu
            mc.MenuCommands.Add(New MenuCommand("-"))

            Dim f As Form
            For Each f In children
                Dim newMC As MenuCommand = New MenuCommand(f.Text)

                ' Is this the currently selected child?
                newMC.Checked = (current Is f)

                AddHandler newMC.Click, AddressOf OnChildSelect

                ' Add a command for this active MDI Child
                mc.MenuCommands.Add(newMC)
            Next
        End If
    End Sub

    Protected Sub OnWindowMenuEnd(ByVal mc As MenuCommand)
        Dim count As Integer = mc.MenuCommands.Count

        ' Did the OnTopMenuStart add any entries?
        If (count >= 10) Then
            ' Remove all the extras
            Dim index As Integer
            For index = 11 To count
                mc.MenuCommands.RemoveAt(10)
            Next index
        End If
    End Sub

    Protected Sub OnChildSelect(ByVal sender As Object, ByVal e As EventArgs)
        Dim childCommand As MenuCommand = sender

        ' Get name of the window to activate
        Dim name As String = childCommand.Text

        ' Get collectiom of Mdi child windows
        Dim children As Form() = Me.MdiChildren

        Dim f As Form
        For Each f In children
            ' Aha...found it
            If (f.Text = name) Then
                f.Activate()
                Exit For
            End If
        Next

        OnMenuItemSelected("ChildSelected")
    End Sub

    Protected Sub OnYesAnimateSelected(ByVal sender As Object, ByVal e As EventArgs)
        _topMenu.Animate = Animate.Yes
        OnMenuItemSelected("Yes - Animate")
    End Sub

    Protected Sub OnYesAnimateUpdate(ByVal sender As Object, ByVal e As EventArgs)
        Dim mc As MenuCommand = sender
        mc.Checked = (_topMenu.Animate = Animate.Yes)
    End Sub

    Protected Sub OnNoAnimateSelected(ByVal sender As Object, ByVal e As EventArgs)
        _topMenu.Animate = Animate.No
        OnMenuItemSelected("No - Animate")
    End Sub

    Protected Sub OnNoAnimateUpdate(ByVal sender As Object, ByVal e As EventArgs)
        Dim mc As MenuCommand = sender
        mc.Checked = (_topMenu.Animate = Animate.No)
    End Sub

    Protected Sub OnSystemAnimateSelected(ByVal sender As Object, ByVal e As EventArgs)
        _topMenu.Animate = Animate.System
        OnMenuItemSelected("System - Animate")
    End Sub

    Protected Sub OnSystemAnimateUpdate(ByVal sender As Object, ByVal e As EventArgs)
        Dim mc As MenuCommand = sender
        mc.Checked = (_topMenu.Animate = Animate.System)
    End Sub

    Protected Sub On100Selected(ByVal sender As Object, ByVal e As EventArgs)
        _topMenu.AnimateTime = 100
        OnMenuItemSelected("100ms - AnimateTime")
    End Sub

    Protected Sub On100Update(ByVal sender As Object, ByVal e As EventArgs)
        Dim mc As MenuCommand = sender
        mc.Checked = (_topMenu.AnimateTime = 100)
    End Sub

    Protected Sub On250Selected(ByVal sender As Object, ByVal e As EventArgs)
        _topMenu.AnimateTime = 250
        OnMenuItemSelected("250ms - AnimateTime")
    End Sub

    Protected Sub On250Update(ByVal sender As Object, ByVal e As EventArgs)
        Dim mc As MenuCommand = sender
        mc.Checked = (_topMenu.AnimateTime = 250)
    End Sub

    Protected Sub On1000Selected(ByVal sender As Object, ByVal e As EventArgs)
        _topMenu.AnimateTime = 1000
        OnMenuItemSelected("1000ms - AnimateTime")
    End Sub

    Protected Sub On1000Update(ByVal sender As Object, ByVal e As EventArgs)
        Dim mc As MenuCommand = sender
        mc.Checked = (_topMenu.AnimateTime = 1000)
    End Sub

    Protected Sub OnBlendSelected(ByVal sender As Object, ByVal e As EventArgs)
        _topMenu.AnimateStyle = Animation.Blend
        OnMenuItemSelected("Blend - Animation")
    End Sub

    Protected Sub OnBlendUpdate(ByVal sender As Object, ByVal e As EventArgs)
        Dim mc As MenuCommand = sender
        mc.Checked = (_topMenu.AnimateStyle = Animation.Blend)
    End Sub

    Protected Sub OnCenterSelected(ByVal sender As Object, ByVal e As EventArgs)
        _topMenu.AnimateStyle = Animation.SlideCenter
        OnMenuItemSelected("Center - Animation")
    End Sub

    Protected Sub OnCenterUpdate(ByVal sender As Object, ByVal e As EventArgs)
        Dim mc As MenuCommand = sender
        mc.Checked = (_topMenu.AnimateStyle = Animation.SlideCenter)
    End Sub

    Protected Sub OnPPSelected(ByVal sender As Object, ByVal e As EventArgs)
        _topMenu.AnimateStyle = Animation.SlideHorVerPositive
        OnMenuItemSelected("+Hor +Ver - Animation")
    End Sub

    Protected Sub OnPPUpdate(ByVal sender As Object, ByVal e As EventArgs)
        Dim mc As MenuCommand = sender
        mc.Checked = (_topMenu.AnimateStyle = Animation.SlideHorVerPositive)
    End Sub

    Protected Sub OnNNSelected(ByVal sender As Object, ByVal e As EventArgs)
        _topMenu.AnimateStyle = Animation.SlideHorVerNegative
        OnMenuItemSelected("-Hor -Ver - Animation")
    End Sub

    Protected Sub OnNNUpdate(ByVal sender As Object, ByVal e As EventArgs)
        Dim mc As MenuCommand = sender
        mc.Checked = (_topMenu.AnimateStyle = Animation.SlideHorVerNegative)
    End Sub

    Protected Sub OnPNSelected(ByVal sender As Object, ByVal e As EventArgs)
        _topMenu.AnimateStyle = Animation.SlideHorPosVerNegative
        OnMenuItemSelected("+Hor -Ver - Animation")
    End Sub

    Protected Sub OnPNUpdate(ByVal sender As Object, ByVal e As EventArgs)
        Dim mc As MenuCommand = sender
        mc.Checked = (_topMenu.AnimateStyle = Animation.SlideHorPosVerNegative)
    End Sub

    Protected Sub OnNPSelected(ByVal sender As Object, ByVal e As EventArgs)
        _topMenu.AnimateStyle = Animation.SlideHorNegVerPositive
        OnMenuItemSelected("-Hor +Ver - Animation")
    End Sub

    Protected Sub OnNPUpdate(ByVal sender As Object, ByVal e As EventArgs)
        Dim mc As MenuCommand = sender
        mc.Checked = (_topMenu.AnimateStyle = Animation.SlideHorNegVerPositive)
    End Sub

    Protected Sub OnSystemSelected(ByVal sender As Object, ByVal e As EventArgs)
        _topMenu.AnimateStyle = Animation.System
        OnMenuItemSelected("System - Animation")
    End Sub

    Protected Sub OnSystemUpdate(ByVal sender As Object, ByVal e As EventArgs)
        Dim mc As MenuCommand = sender
        mc.Checked = (_topMenu.AnimateStyle = Animation.System)
    End Sub
End Class


Public Class MDIChild
    Inherits Form

    Protected _mdiContainer As MDIContainer
    Protected _box As RichTextBox

    Sub New(ByVal mdiContainer As MDIContainer)
        ' Remember parent Form
        _mdiContainer = mdiContainer

        ' Create a RichTextBox to fill entire client area
        _box = New RichTextBox()
        _box.Text = "Right click inside this window to show a Popup menu."
        _box.Dock = DockStyle.Fill
        _box.BorderStyle = BorderStyle.None
        AddHandler _box.MouseUp, AddressOf OnRichTextMouseUp
        Controls.Add(_box)
    End Sub

    Public Sub AppendText(ByVal text As String)
        _box.Text = _box.Text & vbCrLf & text
    End Sub

    Protected Sub OnRichTextMouseUp(ByVal sender As Object, ByVal e As MouseEventArgs)
        If e.Button = MouseButtons.Right Then
            Dim box As RichTextBox = sender

            Dim s0 As MenuCommand = New MenuCommand("Italy", _mdiContainer.Images, 0)
            Dim s1 As MenuCommand = New MenuCommand("Spain", _mdiContainer.Images, 1)
            Dim s2 As MenuCommand = New MenuCommand("Canada", _mdiContainer.Images, 2)
            Dim s3 As MenuCommand = New MenuCommand("France", _mdiContainer.Images, 3)
            Dim s4 As MenuCommand = New MenuCommand("Belgium", _mdiContainer.Images, 4)
            Dim spain0 As MenuCommand = New MenuCommand("Nerja", _mdiContainer.Images, 5)
            Dim spain1 As MenuCommand = New MenuCommand("Madrid", _mdiContainer.Images, 6)
            Dim spain2 As MenuCommand = New MenuCommand("Barcelona", _mdiContainer.Images, 0)
            Dim canada0 As MenuCommand = New MenuCommand("Toronto", _mdiContainer.Images, 5)
            Dim canada1 As MenuCommand = New MenuCommand("Montreal", _mdiContainer.Images, 6)
            Dim canada2 As MenuCommand = New MenuCommand("Belleville", _mdiContainer.Images, 0)
            Dim england As MenuCommand = New MenuCommand("England", _mdiContainer.Images, 2)
            Dim england1 As MenuCommand = New MenuCommand("London", _mdiContainer.Images, 5)
            Dim england2 As MenuCommand = New MenuCommand("Birmingham", _mdiContainer.Images, 6)
            Dim england3 As MenuCommand = New MenuCommand("Nottingham", _mdiContainer.Images, 0)

            england.MenuCommands.AddRange(New MenuCommand() {england1, england2, england3})
            s1.MenuCommands.AddRange(New MenuCommand() {spain0, spain1, spain2})
            s2.MenuCommands.AddRange(New MenuCommand() {canada0, canada1, canada2, england})

            ' Create the popup menu object
            Dim popup As PopupMenu = New PopupMenu()

            ' Define the list of menu commands
            popup.MenuCommands.AddRange(New MenuCommand() {s0, s1, s2, s3, s4})

            ' Define the properties to get appearance to match MenuControl
            popup.Style = _mdiContainer.Style

            AddHandler popup.Selected, AddressOf OnSelected
            AddHandler popup.Deselected, AddressOf OnDeselected

            ' Show it!
            popup.TrackPopup(box.PointToScreen(New Point(e.X, e.Y)))
        End If
    End Sub

    Protected Sub OnSelected(ByVal mc As MenuCommand)
        _mdiContainer.SetStatusBarText("Selection over " & mc.Description)
    End Sub

    Protected Sub OnDeselected(ByVal mc As MenuCommand)
        _mdiContainer.SetStatusBarText("")
    End Sub
End Class


