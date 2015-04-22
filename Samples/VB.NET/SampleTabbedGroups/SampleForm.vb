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

Imports Crownwood.Magic.Menus
Imports Crownwood.Magic.Common
Imports Crownwood.Magic.Controls
Imports Crownwood.Magic.Docking

Public Class Form1
	Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

	Private _count As Int32 = 1
	Private _image As Int32 = -1
	Private _global As RichTextBox
	Private _manager As DockingManager

	Public Sub New()
		MyBase.New()

		'This call is required by the Windows Form Designer.
		InitializeComponent()

		' Create menu options
		CreateMenu()

		' Define the docking windows
		CreateDockingWindows()

		' Create some initial tab pages inside each group
		CreateInitialPages()
	End Sub

	Protected Sub CreateMenu()
		' Create top level commands
		Dim pages As MenuCommand = CreatePages()
		Dim persist As MenuCommand = CreatePersistence()
		Dim tabsMode As MenuCommand = CreateTabsMode()

		' Add top level commands
		MenuControl1.MenuCommands.AddRange(New MenuCommand() {pages, persist, tabsMode})
	End Sub

	Protected Function CreatePages() As MenuCommand
		Dim pages As New MenuCommand("Pages")

		' Create pages sub commands
		Dim add As New MenuCommand("Add")
		Dim remove As New MenuCommand("Remove")

		AddHandler add.Click, AddressOf OnAddPage
		AddHandler remove.Click, AddressOf OnRemovePage

		pages.MenuCommands.AddRange(New MenuCommand() {add, remove})

		' Enable/disable the remove option as appropriate
		AddHandler pages.PopupStart, AddressOf OnPages

		Return pages
	End Function

	Protected Function CreatePersistence() As MenuCommand
		Dim persist As New MenuCommand("Persistence")

		' Create Persistence sub commands
		Dim saveG1 As New MenuCommand("Save Group1")
		Dim loadG1 As New MenuCommand("Load Group1")
		Dim sep1 As New MenuCommand("-")
		Dim saveG2 As New MenuCommand("Save Group2")
		Dim loadG2 As New MenuCommand("Load Group2")
		Dim sep2 As New MenuCommand("-")
		Dim saveG3 As New MenuCommand("Save Group3")
		Dim loadG3 As New MenuCommand("Load Group3")

		AddHandler saveG1.Click, AddressOf OnSaveG1
		AddHandler loadG1.Click, AddressOf OnLoadG1
		AddHandler saveG2.Click, AddressOf OnSaveG2
		AddHandler loadG2.Click, AddressOf OnLoadG2
		AddHandler saveG3.Click, AddressOf OnSaveG3
		AddHandler loadG3.Click, AddressOf OnLoadG3

		persist.MenuCommands.AddRange(New MenuCommand() {saveG1, loadG1, sep1, _
														 saveG2, loadG2, sep2, _
														 saveG3, loadG3})

		Return persist
	End Function

	Protected Function CreateTabsMode() As MenuCommand
		Dim tabsMode As New MenuCommand("DisplayMode")

		' Create modes sub commands
		Dim hideAll As New MenuCommand("Hide All")
		Dim showAll As New MenuCommand("Show All")
		Dim showActiveLeaf As New MenuCommand("Show Active Leaf")
		Dim showMouseOver As New MenuCommand("Show Mouse Over")
		Dim showActiveAndMouseOver As New MenuCommand("Show Active And Mouse Over")

		AddHandler hideAll.Click, AddressOf OnHideAll
		AddHandler showAll.Click, AddressOf OnShowAll
		AddHandler showActiveLeaf.Click, AddressOf OnShowActiveLeaf
		AddHandler showMouseOver.Click, AddressOf OnShowMouseOver
		AddHandler showActiveAndMouseOver.Click, AddressOf OnShowActiveAndMouseOver

		tabsMode.MenuCommands.AddRange(New MenuCommand() {hideAll, showAll, showActiveLeaf, _
														  showMouseOver, showActiveAndMouseOver})

		' Set correct check mark when menu opened
		AddHandler tabsMode.PopupStart, AddressOf OnDisplayMode

		Return tabsMode
	End Function

	Protected Sub CreateDockingWindows()
		' Create the docking manager instance
		_manager = New DockingManager(Me, VisualStyle.IDE)

		' Define innner/outer controls for correct docking operation
		_manager.InnerControl = TabControl1
		_manager.OuterControl = StatusBar1

		' Create the tree control
		Dim tv As TreeView = New DragTree()
		tv.Nodes.Add(New TreeNode("First"))
		tv.Nodes.Add(New TreeNode("Second"))
		tv.Nodes.Add(New TreeNode("Third"))
		tv.Nodes.Add(New TreeNode("Fourth"))

		' Create a rich text box for the second content
		_global = New RichTextBox()

		' Create content instances
		Dim c1 As Content = _manager.Contents.Add(tv, "TreeView")
		Dim c2 As Content = _manager.Contents.Add(_global, "Another Window")

		' Add to the display on the left hand side
		Dim wc As WindowContent = _manager.AddContentWithState(c1, State.DockLeft)

		' Add at the bottom of the same column
		_manager.AddContentToZone(c2, wc.ParentZone, 1)
	End Sub

	Protected Sub CreateInitialPages()
		CreateInitialPagesGroup1()
		CreateInitialPagesGroup2()
		CreateInitialPagesGroup3()
	End Sub

	Protected Sub CreateInitialPagesGroup1()
		' Access the default leaf group
		Dim tgl As TabGroupLeaf = CType(TabbedGroups1.RootSequence(0), TabGroupLeaf)

		' Create two pages for the leaf
		Dim tp1 As Crownwood.Magic.Controls.TabPage = New Crownwood.Magic.Controls.TabPage("Page" & _count, New RichTextBox(), NextImage())
		Dim tp2 As Crownwood.Magic.Controls.TabPage = New Crownwood.Magic.Controls.TabPage("Page" & (_count + 1), New RichTextBox(), NextImage())
		_count += 2

		' Add a two pages to the leaf
		tgl.TabPages.Add(tp1)
		tgl.TabPages.Add(tp2)
	End Sub

	Protected Sub CreateInitialPagesGroup2()
		' Access the default leaf group
		Dim tgl1 As TabGroupLeaf = CType(TabbedGroups2.RootSequence(0), TabGroupLeaf)

		' Add a new leaf group in the same sequence
		Dim tgl2 As TabGroupLeaf = TabbedGroups2.RootSequence.AddNewLeaf()

		' Add a two pages to the leaf
		tgl1.TabPages.Add(NewTabPage())
		tgl2.TabPages.Add(NewTabPage())
	End Sub

	Protected Sub CreateInitialPagesGroup3()
		' Change direction to opposite
		TabbedGroups3.RootSequence.Direction = Direction.Vertical

		' Access the default leaf group
		Dim tgl1 As TabGroupLeaf = CType(TabbedGroups3.RootSequence(0), TabGroupLeaf)

		' Add a new leaf group in the same sequence
		Dim tgl2 As TabGroupLeaf = TabbedGroups3.RootSequence.AddNewLeaf()

		' Add a two pages to the leaf
		tgl1.TabPages.Add(NewTabPage())
		tgl2.TabPages.Add(NewTabPage())
	End Sub

	Protected Function NextImage() As Int32
		_image += 1
		If _image > 8 Then _image = 0
		Return _image
	End Function

	Protected Function NewTabPage() As Crownwood.Magic.Controls.TabPage
		Dim newPage As Crownwood.Magic.Controls.TabPage = New Crownwood.Magic.Controls.TabPage("Page" & _count, New RichTextBox(), NextImage())
		_count += 1
		Return newPage
	End Function

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
	Private WithEvents groupTabs As System.Windows.Forms.ImageList
	Private WithEvents mainTabs As System.Windows.Forms.ImageList
	Friend WithEvents MenuControl1 As Crownwood.Magic.Menus.MenuControl
	Friend WithEvents TabControl1 As Crownwood.Magic.Controls.TabControl
	Friend WithEvents StatusBar1 As System.Windows.Forms.StatusBar
	Friend WithEvents TabPage1 As Crownwood.Magic.Controls.TabPage
	Friend WithEvents TabPage2 As Crownwood.Magic.Controls.TabPage
	Friend WithEvents TabPage3 As Crownwood.Magic.Controls.TabPage
	Friend WithEvents TabbedGroups1 As Crownwood.Magic.Controls.TabbedGroups
	Friend WithEvents TabbedGroups2 As Crownwood.Magic.Controls.TabbedGroups
	Friend WithEvents TabbedGroups3 As Crownwood.Magic.Controls.TabbedGroups
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(Form1))
		Me.groupTabs = New System.Windows.Forms.ImageList(Me.components)
		Me.mainTabs = New System.Windows.Forms.ImageList(Me.components)
		Me.MenuControl1 = New Crownwood.Magic.Menus.MenuControl()
		Me.TabControl1 = New Crownwood.Magic.Controls.TabControl()
		Me.TabPage1 = New Crownwood.Magic.Controls.TabPage()
		Me.TabbedGroups1 = New Crownwood.Magic.Controls.TabbedGroups()
		Me.TabPage2 = New Crownwood.Magic.Controls.TabPage()
		Me.TabbedGroups2 = New Crownwood.Magic.Controls.TabbedGroups()
		Me.TabPage3 = New Crownwood.Magic.Controls.TabPage()
		Me.TabbedGroups3 = New Crownwood.Magic.Controls.TabbedGroups()
		Me.StatusBar1 = New System.Windows.Forms.StatusBar()
		Me.TabPage1.SuspendLayout()
		CType(Me.TabbedGroups1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.TabPage2.SuspendLayout()
		CType(Me.TabbedGroups2, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.TabPage3.SuspendLayout()
		CType(Me.TabbedGroups3, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'groupTabs
		'
		Me.groupTabs.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit
		Me.groupTabs.ImageSize = New System.Drawing.Size(16, 16)
		Me.groupTabs.ImageStream = CType(resources.GetObject("groupTabs.ImageStream"), System.Windows.Forms.ImageListStreamer)
		Me.groupTabs.TransparentColor = System.Drawing.Color.Transparent
		'
		'mainTabs
		'
		Me.mainTabs.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit
		Me.mainTabs.ImageSize = New System.Drawing.Size(16, 16)
		Me.mainTabs.ImageStream = CType(resources.GetObject("mainTabs.ImageStream"), System.Windows.Forms.ImageListStreamer)
		Me.mainTabs.TransparentColor = System.Drawing.Color.Transparent
		'
		'MenuControl1
		'
		Me.MenuControl1.AnimateStyle = Crownwood.Magic.Menus.Animation.System
		Me.MenuControl1.AnimateTime = 100
		Me.MenuControl1.Cursor = System.Windows.Forms.Cursors.Arrow
		Me.MenuControl1.Direction = Crownwood.Magic.Common.Direction.Horizontal
		Me.MenuControl1.Dock = System.Windows.Forms.DockStyle.Top
		Me.MenuControl1.Font = New System.Drawing.Font("Tahoma", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World)
		Me.MenuControl1.HighlightTextColor = System.Drawing.SystemColors.MenuText
		Me.MenuControl1.Name = "MenuControl1"
		Me.MenuControl1.Size = New System.Drawing.Size(440, 25)
		Me.MenuControl1.Style = Crownwood.Magic.Common.VisualStyle.IDE
		Me.MenuControl1.TabIndex = 0
		Me.MenuControl1.TabStop = False
		Me.MenuControl1.Text = "MenuControl1"
		'
		'TabControl1
		'
		Me.TabControl1.Dock = System.Windows.Forms.DockStyle.Fill
		Me.TabControl1.HideTabsMode = Crownwood.Magic.Controls.TabControl.HideTabsModes.ShowAlways
		Me.TabControl1.ImageList = Me.mainTabs
		Me.TabControl1.Location = New System.Drawing.Point(0, 25)
		Me.TabControl1.Name = "TabControl1"
		Me.TabControl1.SelectedIndex = 0
		Me.TabControl1.SelectedTab = Me.TabPage1
		Me.TabControl1.Size = New System.Drawing.Size(440, 366)
		Me.TabControl1.TabIndex = 1
		Me.TabControl1.TabPages.AddRange(New Crownwood.Magic.Controls.TabPage() {Me.TabPage1, Me.TabPage2, Me.TabPage3})
		'
		'TabPage1
		'
		Me.TabPage1.Controls.AddRange(New System.Windows.Forms.Control() {Me.TabbedGroups1})
		Me.TabPage1.ImageIndex = 0
		Me.TabPage1.Name = "TabPage1"
		Me.TabPage1.Size = New System.Drawing.Size(440, 341)
		Me.TabPage1.TabIndex = 0
		Me.TabPage1.Title = "Group1"
		'
		'TabbedGroups1
		'
		Me.TabbedGroups1.ActiveLeaf = Nothing
		Me.TabbedGroups1.AllowDrop = True
		Me.TabbedGroups1.AtLeastOneLeaf = True
		Me.TabbedGroups1.Dock = System.Windows.Forms.DockStyle.Fill
		Me.TabbedGroups1.ImageList = Me.groupTabs
		Me.TabbedGroups1.Name = "TabbedGroups1"
		Me.TabbedGroups1.ProminentLeaf = Nothing
		Me.TabbedGroups1.ResizeBarColor = System.Drawing.SystemColors.Control
		Me.TabbedGroups1.Size = New System.Drawing.Size(440, 341)
		Me.TabbedGroups1.TabIndex = 0
		'
		'TabPage2
		'
		Me.TabPage2.Controls.AddRange(New System.Windows.Forms.Control() {Me.TabbedGroups2})
		Me.TabPage2.ImageIndex = 1
		Me.TabPage2.Name = "TabPage2"
		Me.TabPage2.Selected = False
		Me.TabPage2.Size = New System.Drawing.Size(440, 341)
		Me.TabPage2.TabIndex = 1
		Me.TabPage2.Title = "Group2"
		'
		'TabbedGroups2
		'
		Me.TabbedGroups2.ActiveLeaf = Nothing
		Me.TabbedGroups2.AllowDrop = True
		Me.TabbedGroups2.AtLeastOneLeaf = True
		Me.TabbedGroups2.Dock = System.Windows.Forms.DockStyle.Fill
		Me.TabbedGroups2.ImageList = Me.groupTabs
		Me.TabbedGroups2.Name = "TabbedGroups2"
		Me.TabbedGroups2.ProminentLeaf = Nothing
		Me.TabbedGroups2.ResizeBarColor = System.Drawing.SystemColors.Control
		Me.TabbedGroups2.Size = New System.Drawing.Size(440, 341)
		Me.TabbedGroups2.TabIndex = 0
		'
		'TabPage3
		'
		Me.TabPage3.Controls.AddRange(New System.Windows.Forms.Control() {Me.TabbedGroups3})
		Me.TabPage3.ImageIndex = 2
		Me.TabPage3.Name = "TabPage3"
		Me.TabPage3.Selected = False
		Me.TabPage3.Size = New System.Drawing.Size(440, 341)
		Me.TabPage3.TabIndex = 2
		Me.TabPage3.Title = "Group3"
		'
		'TabbedGroups3
		'
		Me.TabbedGroups3.ActiveLeaf = Nothing
		Me.TabbedGroups3.AllowDrop = True
		Me.TabbedGroups3.AtLeastOneLeaf = True
		Me.TabbedGroups3.Dock = System.Windows.Forms.DockStyle.Fill
		Me.TabbedGroups3.ImageList = Me.groupTabs
		Me.TabbedGroups3.Name = "TabbedGroups3"
		Me.TabbedGroups3.ProminentLeaf = Nothing
		Me.TabbedGroups3.ResizeBarColor = System.Drawing.SystemColors.Control
		Me.TabbedGroups3.Size = New System.Drawing.Size(440, 341)
		Me.TabbedGroups3.TabIndex = 0
		'
		'StatusBar1
		'
		Me.StatusBar1.Location = New System.Drawing.Point(0, 391)
		Me.StatusBar1.Name = "StatusBar1"
		Me.StatusBar1.Size = New System.Drawing.Size(440, 22)
		Me.StatusBar1.TabIndex = 2
		Me.StatusBar1.Text = "StatusBar1"
		'
		'Form1
		'
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.ClientSize = New System.Drawing.Size(440, 413)
		Me.Controls.AddRange(New System.Windows.Forms.Control() {Me.TabControl1, Me.MenuControl1, Me.StatusBar1})
		Me.Name = "Form1"
		Me.Text = "Form1"
		Me.TabPage1.ResumeLayout(False)
		CType(Me.TabbedGroups1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.TabPage2.ResumeLayout(False)
		CType(Me.TabbedGroups2, System.ComponentModel.ISupportInitialize).EndInit()
		Me.TabPage3.ResumeLayout(False)
		CType(Me.TabbedGroups3, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)

	End Sub

	Protected Sub OnPages(ByVal pages As MenuCommand)
		Dim tc As Crownwood.Magic.Controls.TabControl

		' Find the active tab control in the selected group 
		If (TabControl1.SelectedIndex = 0) Then
			If Not (TabbedGroups1.ActiveLeaf Is Nothing) Then
				tc = CType(TabbedGroups1.ActiveLeaf.GroupControl, Crownwood.Magic.Controls.TabControl)
			End If
		Else
			If (TabControl1.SelectedIndex = 1) Then
				If Not (TabbedGroups2.ActiveLeaf Is Nothing) Then
					tc = CType(TabbedGroups2.ActiveLeaf.GroupControl, Crownwood.Magic.Controls.TabControl)
				End If
			Else
				If Not (TabbedGroups3.ActiveLeaf Is Nothing) Then
					tc = CType(TabbedGroups3.ActiveLeaf.GroupControl, Crownwood.Magic.Controls.TabControl)
				End If
			End If
		End If

		' Did we find a current tab control?
		If (Not (tc Is Nothing) And Not (tc.SelectedTab Is Nothing)) Then
			pages.MenuCommands(1).Enabled = True
		Else
			pages.MenuCommands(1).Enabled = False
		End If
	End Sub

	Protected Sub OnAddPage(ByVal sender As Object, ByVal e As EventArgs)
		If (TabControl1.SelectedIndex = 0) Then
			If Not (TabbedGroups1.ActiveLeaf Is Nothing) Then
				TabbedGroups1.ActiveLeaf.TabPages.Add(NewTabPage())
			End If
		Else
			If (TabControl1.SelectedIndex = 1) Then
				If Not (TabbedGroups2.ActiveLeaf Is Nothing) Then
					TabbedGroups2.ActiveLeaf.TabPages.Add(NewTabPage())
				End If
			Else
				If Not (TabbedGroups3.ActiveLeaf Is Nothing) Then
					TabbedGroups3.ActiveLeaf.TabPages.Add(NewTabPage())
				End If
			End If
		End If
	End Sub

	Protected Sub OnRemovePage(ByVal sender As Object, ByVal e As EventArgs)
		Dim tc As Crownwood.Magic.Controls.TabControl

		' Find the active tab control in the selected group 
		If (TabControl1.SelectedIndex = 0) Then
			If Not (TabbedGroups1.ActiveLeaf Is Nothing) Then
				tc = CType(TabbedGroups1.ActiveLeaf.GroupControl, Crownwood.Magic.Controls.TabControl)
			End If
		Else
			If (TabControl1.SelectedIndex = 1) Then
				If Not (TabbedGroups2.ActiveLeaf Is Nothing) Then
					tc = CType(TabbedGroups2.ActiveLeaf.GroupControl, Crownwood.Magic.Controls.TabControl)
				End If
			Else
				If Not (TabbedGroups3.ActiveLeaf Is Nothing) Then
					tc = CType(TabbedGroups3.ActiveLeaf.GroupControl, Crownwood.Magic.Controls.TabControl)
				End If
			End If
		End If

		' Did we find a current tab control?
		If Not (tc Is Nothing) Then
			' Does it have a selected tab?
			If Not (tc.SelectedTab Is Nothing) Then
				' Remove the page
				tc.TabPages.Remove(tc.SelectedTab)
			End If
		End If
	End Sub

	Protected Sub OnSaveG1(ByVal sender As Object, ByVal e As EventArgs)
		TabbedGroups1.SaveConfigToFile("Group1.xml")
	End Sub

	Protected Sub OnLoadG1(ByVal sender As Object, ByVal e As EventArgs)
		Try
			TabbedGroups1.LoadConfigFromFile("Group1.xml")
		Finally
		End Try
	End Sub

	Protected Sub OnSaveG2(ByVal sender As Object, ByVal e As EventArgs)
		TabbedGroups2.SaveConfigToFile("Group2.xml")
	End Sub

	Protected Sub OnLoadG2(ByVal sender As Object, ByVal e As EventArgs)
		Try
			TabbedGroups2.LoadConfigFromFile("Group2.xml")
		Finally
		End Try
	End Sub

	Protected Sub OnSaveG3(ByVal sender As Object, ByVal e As EventArgs)
		TabbedGroups3.SaveConfigToFile("Group3.xml")
	End Sub

	Protected Sub OnLoadG3(ByVal sender As Object, ByVal e As EventArgs)
		Try
			TabbedGroups3.LoadConfigFromFile("Group3.xml")
		Finally
		End Try
	End Sub

	Protected Sub OnDisplayMode(ByVal tabsMode As MenuCommand)
		' Default all the commands to not being checked
		Dim mc As MenuCommand
		For Each mc In tabsMode.MenuCommands
			mc.Checked = False
		Next

		Select Case TabbedGroups1.DisplayTabMode
			Case TabbedGroups.DisplayTabModes.HideAll
				tabsMode.MenuCommands(0).Checked = True
			Case TabbedGroups.DisplayTabModes.ShowAll
				tabsMode.MenuCommands(1).Checked = True
			Case TabbedGroups.DisplayTabModes.ShowActiveLeaf
				tabsMode.MenuCommands(2).Checked = True
			Case TabbedGroups.DisplayTabModes.ShowMouseOver
				tabsMode.MenuCommands(3).Checked = True
			Case TabbedGroups.DisplayTabModes.ShowActiveAndMouseOver
				tabsMode.MenuCommands(4).Checked = True
		End Select
	End Sub

	Protected Sub OnHideAll(ByVal sender As Object, ByVal e As EventArgs)
		TabbedGroups1.DisplayTabMode = TabbedGroups.DisplayTabModes.HideAll
		TabbedGroups2.DisplayTabMode = TabbedGroups.DisplayTabModes.HideAll
		TabbedGroups3.DisplayTabMode = TabbedGroups.DisplayTabModes.HideAll
	End Sub

	Protected Sub OnShowAll(ByVal sender As Object, ByVal e As EventArgs)
		TabbedGroups1.DisplayTabMode = TabbedGroups.DisplayTabModes.ShowAll
		TabbedGroups2.DisplayTabMode = TabbedGroups.DisplayTabModes.ShowAll
		TabbedGroups3.DisplayTabMode = TabbedGroups.DisplayTabModes.ShowAll
	End Sub

	Protected Sub OnShowActiveLeaf(ByVal sender As Object, ByVal e As EventArgs)
		TabbedGroups1.DisplayTabMode = TabbedGroups.DisplayTabModes.ShowActiveLeaf
		TabbedGroups2.DisplayTabMode = TabbedGroups.DisplayTabModes.ShowActiveLeaf
		TabbedGroups3.DisplayTabMode = TabbedGroups.DisplayTabModes.ShowActiveLeaf
	End Sub

	Protected Sub OnShowMouseOver(ByVal sender As Object, ByVal e As EventArgs)
		TabbedGroups1.DisplayTabMode = TabbedGroups.DisplayTabModes.ShowMouseOver
		TabbedGroups2.DisplayTabMode = TabbedGroups.DisplayTabModes.ShowMouseOver
		TabbedGroups3.DisplayTabMode = TabbedGroups.DisplayTabModes.ShowMouseOver
	End Sub

	Protected Sub OnShowActiveAndMouseOver(ByVal sender As Object, ByVal e As EventArgs)
		TabbedGroups1.DisplayTabMode = TabbedGroups.DisplayTabModes.ShowActiveAndMouseOver
		TabbedGroups2.DisplayTabMode = TabbedGroups.DisplayTabModes.ShowActiveAndMouseOver
		TabbedGroups3.DisplayTabMode = TabbedGroups.DisplayTabModes.ShowActiveAndMouseOver
	End Sub

	Private Sub PageSaving(ByVal tg As Crownwood.Magic.Controls.TabbedGroups, _
						   ByVal e As Crownwood.Magic.Controls.TGPageSavingEventArgs) _
		Handles TabbedGroups1.PageSaving, TabbedGroups2.PageSaving, TabbedGroups3.PageSaving
		' Persist the text box contents
		e.XmlOut.WriteCData(CType(e.TabPage.Control, RichTextBox).Text)
	End Sub

	Private Sub PageLoading(ByVal tg As Crownwood.Magic.Controls.TabbedGroups, _
							ByVal e As Crownwood.Magic.Controls.TGPageLoadingEventArgs) _
		Handles TabbedGroups1.PageLoading, TabbedGroups2.PageLoading, TabbedGroups3.PageLoading
		' Read back the text box contents
		CType(e.TabPage.Control, RichTextBox).Text = e.XmlIn.ReadString()
	End Sub

	Private Sub GlobalSaving(ByVal tg As Crownwood.Magic.Controls.TabbedGroups, _
							 ByVal xmlOut As System.Xml.XmlTextWriter) _
		Handles TabbedGroups1.GlobalSaving, TabbedGroups2.GlobalSaving, TabbedGroups3.GlobalSaving
		' Persist the global text box contents
		xmlOut.WriteCData(_global.Text)
	End Sub

	Private Sub GlobalLoading(ByVal tg As Crownwood.Magic.Controls.TabbedGroups, _
							  ByVal xmlIn As System.Xml.XmlTextReader) _
		Handles TabbedGroups1.GlobalLoading, TabbedGroups2.GlobalLoading, TabbedGroups3.GlobalLoading
		' Read back the global text box contents
		_global.Text = xmlIn.ReadString()
	End Sub

	Private Sub TabControlCreated(ByVal tg As Crownwood.Magic.Controls.TabbedGroups, _
								  ByVal tc As Crownwood.Magic.Controls.TabControl) _
		Handles TabbedGroups1.TabControlCreated, TabbedGroups2.TabControlCreated, TabbedGroups3.TabControlCreated
		' This is where you change the tab control defaults when a new tab control is created
	End Sub

	Private Sub ExternalDrop(ByVal tg As Crownwood.Magic.Controls.TabbedGroups, _
							 ByVal tgl As Crownwood.Magic.Controls.TabGroupLeaf, _
							 ByVal tc As Crownwood.Magic.Controls.TabControl, _
							 ByVal dp As Crownwood.Magic.Controls.TabbedGroups.DragProvider) _
		Handles TabbedGroups1.ExternalDrop, TabbedGroups2.ExternalDrop, TabbedGroups3.ExternalDrop
		' Create a new tab page
		Dim tp As Crownwood.Magic.Controls.TabPage = NewTabPage()

		' Define the text in this control
		CType(tp.Control, RichTextBox).Text = "Dragged from node '" & CType(dp.Tag, String) & "'"

		' We want the new page to become selected
		tp.Selected = True

		' Add new page into the destination tab control
		tgl.TabPages.Add(tp)
	End Sub

#End Region

	Private Sub TabControl1_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TabControl1.SelectionChanged

	End Sub
End Class

Public Class DragTree
	Inherits TreeView
	Protected _leftDown As Boolean
	Protected _leftPoint As System.Drawing.Point
	Protected _leftNode As TreeNode

	Sub New()
		_leftDown = False
	End Sub

	Protected Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)
		' Only interested in the left button
		If (e.Button = MouseButtons.Left) Then
			Dim n As TreeNode = Me.GetNodeAt(New Point(e.X, e.Y))

			' Are we selecting a valid node?
			If Not (n Is Nothing) Then
				' Might be start of a drag, so remember details
				_leftNode = n
				_leftDown = True
				_leftPoint = New Point(e.X, e.Y)

				' Must capture the mouse
				Me.Capture = True
				Me.Focus()
			End If
		End If

		MyBase.OnMouseDown(e)
	End Sub

	Protected Overrides Sub OnMouseMove(ByVal e As MouseEventArgs)
		' Are we monitoring for a drag operation?
		If (_leftDown) Then
			Dim dragRect As Rectangle = New Rectangle(_leftPoint, New Size(0, 0))

			' Create rectangle for drag start
			dragRect.Inflate(SystemInformation.DoubleClickSize)

			' Has mouse been dragged outside of rectangle?
			If (Not dragRect.Contains(New Point(e.X, e.Y))) Then
				' Create an object the TabbedGroups control understands
				Dim dp As TabbedGroups.DragProvider = New TabbedGroups.DragProvider()

				' Box the node name as the parameter for passing across
				dp.Tag = CType(_leftNode.Text, Object)

				' Must start a drag operation
				DoDragDrop(dp, DragDropEffects.Copy)

				' Cancel any further drag events until mouse is pressed again
				_leftDown = False
				_leftNode = Nothing
			End If
		End If

		MyBase.OnMouseMove(e)
	End Sub

	Protected Overrides Sub WndProc(ByRef m As Message)
		Select Case m.Msg
			Case CType(Crownwood.Magic.Win32.Msgs.WM_LBUTTONUP, Int32)
				' Remembering drag info?
				If (_leftDown) Then
					' Cancel any drag attempt
					_leftDown = False
					_leftNode = Nothing
				End If
		End Select

		MyBase.WndProc(m)
	End Sub
End Class
