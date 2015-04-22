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
Imports System.Drawing
Imports System.Resources
Imports System.Reflection
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports Crownwood.Magic.Common
Imports Crownwood.Magic.Controls

Public Class SampleTabControl
    Inherits System.Windows.Forms.Form

    Private Shared _count As Integer = 0

    Private _internalImages As ImageList
    Private _update As Boolean = False
    Private _startForeColor As Color
    Private _startBackColor As Color
    Private _startButtonActive As Color
    Private _startButtonInactive As Color
    Private _startTextInactiveColor As Color
    Private _startHotTextColor As Color
    Private _strings As String() = New String() {"P&roperties", _
                                                 "Solution Explo&rer", _
                                                 "&Task List", _
                                                 "&Command Window", _
                                                 "Callstack", _
                                                 "B&reakpoints", _
                                                 "Output"}

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Create a strip of images by loading an embedded bitmap resource
        _internalImages = ResourceHelper.LoadBitmapStrip(Me.GetType(), _
                                                         "SampleTabControl.SampleImages.bmp", _
                                                         new Size(16,16), _
                                                         new Point(0,0))

        tabControl.ImageList = _internalImages

        ' Hook into the close button
        AddHandler tabControl.ClosePressed, AddressOf OnRemovePage

        ' Remember initial colors
        _startForeColor = tabControl.ForeColor
        _startBackColor = tabControl.BackColor
        _startButtonActive = tabControl.ButtonActiveColor
        _startButtonInactive = tabControl.ButtonInactiveColor
        _startTextInactiveColor = tabControl.TextInactiveColor
        _startHotTextColor = tabControl.HotTextColor

        normal.Checked = True

        UpdateControls()
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
    Private WithEvents addPage As System.Windows.Forms.Button
    Private WithEvents removePage As System.Windows.Forms.Button
    Private WithEvents clearAll As System.Windows.Forms.Button
    Private WithEvents StyleGroup As System.Windows.Forms.GroupBox
    Private WithEvents AppearanceGroup As System.Windows.Forms.GroupBox
    Private WithEvents exampleColors As System.Windows.Forms.GroupBox
    Private WithEvents positionAtTop As System.Windows.Forms.CheckBox
    Private WithEvents hotTrack As System.Windows.Forms.CheckBox
    Private WithEvents shrinkPages As System.Windows.Forms.CheckBox
    Private WithEvents showClose As System.Windows.Forms.CheckBox
    Private WithEvents showArrows As System.Windows.Forms.CheckBox
    Private WithEvents insetPlain As System.Windows.Forms.CheckBox
    Private WithEvents insetPagesOnly As System.Windows.Forms.CheckBox
    Private WithEvents selectedTextOnly As System.Windows.Forms.CheckBox
    Private WithEvents idePixelBorder As System.Windows.Forms.CheckBox
    Private WithEvents label1 As System.Windows.Forms.Label
    Private WithEvents label2 As System.Windows.Forms.Label
    Private WithEvents label3 As System.Windows.Forms.Label
    Private WithEvents label4 As System.Windows.Forms.Label
    Private WithEvents numericUpDown1 As System.Windows.Forms.NumericUpDown
    Private WithEvents numericUpDown2 As System.Windows.Forms.NumericUpDown
    Private WithEvents numericUpDown3 As System.Windows.Forms.NumericUpDown
    Private WithEvents numericUpDown4 As System.Windows.Forms.NumericUpDown
    Private WithEvents radioIDE As System.Windows.Forms.RadioButton
    Private WithEvents radioPlain As System.Windows.Forms.RadioButton
    Private WithEvents radioMultiBox As System.Windows.Forms.RadioButton
    Private WithEvents radioMultiForm As System.Windows.Forms.RadioButton
    Private WithEvents radioMultiDocument As System.Windows.Forms.RadioButton
    Private WithEvents red As System.Windows.Forms.RadioButton
    Private WithEvents blue As System.Windows.Forms.RadioButton
    Private WithEvents normal As System.Windows.Forms.RadioButton
    Private WithEvents tabControl As Crownwood.Magic.Controls.TabControl
    Friend WithEvents idePixelArea As System.Windows.Forms.CheckBox
    Friend WithEvents multiline As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents tabHideUsingLogic As System.Windows.Forms.RadioButton
    Friend WithEvents tabHideAlways As System.Windows.Forms.RadioButton
    Friend WithEvents tabShowAlways As System.Windows.Forms.RadioButton
    Friend WithEvents tabHideWithoutMouse As System.Windows.Forms.RadioButton
    Private WithEvents hoverSelect As System.Windows.Forms.CheckBox
    Friend WithEvents multilineFullWidth As System.Windows.Forms.CheckBox

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.selectedTextOnly = New System.Windows.Forms.CheckBox()
        Me.positionAtTop = New System.Windows.Forms.CheckBox()
        Me.radioMultiBox = New System.Windows.Forms.RadioButton()
        Me.tabControl = New Crownwood.Magic.Controls.TabControl()
        Me.removePage = New System.Windows.Forms.Button()
        Me.hotTrack = New System.Windows.Forms.CheckBox()
        Me.numericUpDown3 = New System.Windows.Forms.NumericUpDown()
        Me.numericUpDown1 = New System.Windows.Forms.NumericUpDown()
        Me.numericUpDown4 = New System.Windows.Forms.NumericUpDown()
        Me.numericUpDown2 = New System.Windows.Forms.NumericUpDown()
        Me.radioMultiForm = New System.Windows.Forms.RadioButton()
        Me.showClose = New System.Windows.Forms.CheckBox()
        Me.shrinkPages = New System.Windows.Forms.CheckBox()
        Me.addPage = New System.Windows.Forms.Button()
        Me.clearAll = New System.Windows.Forms.Button()
        Me.StyleGroup = New System.Windows.Forms.GroupBox()
        Me.radioPlain = New System.Windows.Forms.RadioButton()
        Me.radioIDE = New System.Windows.Forms.RadioButton()
        Me.label4 = New System.Windows.Forms.Label()
        Me.label1 = New System.Windows.Forms.Label()
        Me.label2 = New System.Windows.Forms.Label()
        Me.label3 = New System.Windows.Forms.Label()
        Me.insetPlain = New System.Windows.Forms.CheckBox()
        Me.insetPagesOnly = New System.Windows.Forms.CheckBox()
        Me.showArrows = New System.Windows.Forms.CheckBox()
        Me.radioMultiDocument = New System.Windows.Forms.RadioButton()
        Me.AppearanceGroup = New System.Windows.Forms.GroupBox()
        Me.exampleColors = New System.Windows.Forms.GroupBox()
        Me.red = New System.Windows.Forms.RadioButton()
        Me.blue = New System.Windows.Forms.RadioButton()
        Me.normal = New System.Windows.Forms.RadioButton()
        Me.idePixelBorder = New System.Windows.Forms.CheckBox()
        Me.idePixelArea = New System.Windows.Forms.CheckBox()
        Me.multiline = New System.Windows.Forms.CheckBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.tabHideWithoutMouse = New System.Windows.Forms.RadioButton()
        Me.tabHideUsingLogic = New System.Windows.Forms.RadioButton()
        Me.tabHideAlways = New System.Windows.Forms.RadioButton()
        Me.tabShowAlways = New System.Windows.Forms.RadioButton()
        Me.hoverSelect = New System.Windows.Forms.CheckBox()
        Me.multilineFullWidth = New System.Windows.Forms.CheckBox()
        CType(Me.numericUpDown3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numericUpDown1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numericUpDown4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numericUpDown2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.StyleGroup.SuspendLayout()
        Me.AppearanceGroup.SuspendLayout()
        Me.exampleColors.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'selectedTextOnly
        '
        Me.selectedTextOnly.Location = New System.Drawing.Point(176, 224)
        Me.selectedTextOnly.Name = "selectedTextOnly"
        Me.selectedTextOnly.Size = New System.Drawing.Size(120, 24)
        Me.selectedTextOnly.TabIndex = 1
        Me.selectedTextOnly.Text = "Selected Text Only"
        '
        'positionAtTop
        '
        Me.positionAtTop.Location = New System.Drawing.Point(176, 8)
        Me.positionAtTop.Name = "positionAtTop"
        Me.positionAtTop.TabIndex = 1
        Me.positionAtTop.Text = "Position At Top"
        '
        'radioMultiBox
        '
        Me.radioMultiBox.Location = New System.Drawing.Point(16, 64)
        Me.radioMultiBox.Name = "radioMultiBox"
        Me.radioMultiBox.Size = New System.Drawing.Size(88, 24)
        Me.radioMultiBox.TabIndex = 0
        Me.radioMultiBox.Text = "MultiBox"
        '
        'tabControl
        '
        Me.tabControl.Font = New System.Drawing.Font("Tahoma", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World)
        Me.tabControl.ForeColor = System.Drawing.SystemColors.MenuText
        Me.tabControl.HideTabsMode = Crownwood.Magic.Controls.TabControl.HideTabsModes.ShowAlways
        Me.tabControl.Location = New System.Drawing.Point(312, 24)
        Me.tabControl.Name = "tabControl"
        Me.tabControl.Size = New System.Drawing.Size(312, 248)
        Me.tabControl.TabIndex = 0
        Me.tabControl.TextColor = System.Drawing.SystemColors.MenuText
        '
        'removePage
        '
        Me.removePage.Location = New System.Drawing.Point(424, 384)
        Me.removePage.Name = "removePage"
        Me.removePage.Size = New System.Drawing.Size(88, 24)
        Me.removePage.TabIndex = 0
        Me.removePage.Text = "RemovePage"
        '
        'hotTrack
        '
        Me.hotTrack.Location = New System.Drawing.Point(176, 32)
        Me.hotTrack.Name = "hotTrack"
        Me.hotTrack.TabIndex = 1
        Me.hotTrack.Text = "HotTrack"
        '
        'numericUpDown3
        '
        Me.numericUpDown3.Location = New System.Drawing.Point(384, 344)
        Me.numericUpDown3.Name = "numericUpDown3"
        Me.numericUpDown3.Size = New System.Drawing.Size(56, 20)
        Me.numericUpDown3.TabIndex = 2
        '
        'numericUpDown1
        '
        Me.numericUpDown1.Location = New System.Drawing.Point(384, 312)
        Me.numericUpDown1.Name = "numericUpDown1"
        Me.numericUpDown1.Size = New System.Drawing.Size(56, 20)
        Me.numericUpDown1.TabIndex = 2
        '
        'numericUpDown4
        '
        Me.numericUpDown4.Location = New System.Drawing.Point(544, 344)
        Me.numericUpDown4.Name = "numericUpDown4"
        Me.numericUpDown4.Size = New System.Drawing.Size(56, 20)
        Me.numericUpDown4.TabIndex = 2
        '
        'numericUpDown2
        '
        Me.numericUpDown2.Location = New System.Drawing.Point(544, 312)
        Me.numericUpDown2.Name = "numericUpDown2"
        Me.numericUpDown2.Size = New System.Drawing.Size(56, 20)
        Me.numericUpDown2.TabIndex = 2
        '
        'radioMultiForm
        '
        Me.radioMultiForm.Location = New System.Drawing.Point(16, 40)
        Me.radioMultiForm.Name = "radioMultiForm"
        Me.radioMultiForm.Size = New System.Drawing.Size(88, 24)
        Me.radioMultiForm.TabIndex = 0
        Me.radioMultiForm.Text = "MultiForm"
        '
        'showClose
        '
        Me.showClose.Location = New System.Drawing.Point(176, 80)
        Me.showClose.Name = "showClose"
        Me.showClose.TabIndex = 1
        Me.showClose.Text = "Show Close"
        '
        'shrinkPages
        '
        Me.shrinkPages.Location = New System.Drawing.Point(176, 56)
        Me.shrinkPages.Name = "shrinkPages"
        Me.shrinkPages.TabIndex = 1
        Me.shrinkPages.Text = "Shrink pages"
        '
        'addPage
        '
        Me.addPage.Location = New System.Drawing.Point(312, 384)
        Me.addPage.Name = "addPage"
        Me.addPage.Size = New System.Drawing.Size(87, 24)
        Me.addPage.TabIndex = 0
        Me.addPage.Text = "AddPage"
        '
        'clearAll
        '
        Me.clearAll.Location = New System.Drawing.Point(536, 384)
        Me.clearAll.Name = "clearAll"
        Me.clearAll.Size = New System.Drawing.Size(88, 24)
        Me.clearAll.TabIndex = 0
        Me.clearAll.Text = "RemoveAll"
        '
        'StyleGroup
        '
        Me.StyleGroup.Controls.AddRange(New System.Windows.Forms.Control() {Me.radioPlain, Me.radioIDE})
        Me.StyleGroup.Location = New System.Drawing.Point(8, 16)
        Me.StyleGroup.Name = "StyleGroup"
        Me.StyleGroup.Size = New System.Drawing.Size(144, 72)
        Me.StyleGroup.TabIndex = 0
        Me.StyleGroup.TabStop = False
        Me.StyleGroup.Text = "Style"
        '
        'radioPlain
        '
        Me.radioPlain.Location = New System.Drawing.Point(16, 40)
        Me.radioPlain.Name = "radioPlain"
        Me.radioPlain.Size = New System.Drawing.Size(56, 24)
        Me.radioPlain.TabIndex = 0
        Me.radioPlain.Text = "Plain"
        '
        'radioIDE
        '
        Me.radioIDE.Location = New System.Drawing.Point(16, 16)
        Me.radioIDE.Name = "radioIDE"
        Me.radioIDE.Size = New System.Drawing.Size(56, 24)
        Me.radioIDE.TabIndex = 0
        Me.radioIDE.Text = "IDE"
        '
        'label4
        '
        Me.label4.Location = New System.Drawing.Point(456, 344)
        Me.label4.Name = "label4"
        Me.label4.Size = New System.Drawing.Size(80, 23)
        Me.label4.TabIndex = 3
        Me.label4.Text = "Bottom Offset"
        Me.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'label1
        '
        Me.label1.Location = New System.Drawing.Point(312, 312)
        Me.label1.Name = "label1"
        Me.label1.Size = New System.Drawing.Size(64, 23)
        Me.label1.TabIndex = 3
        Me.label1.Text = "Left Offset"
        Me.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'label2
        '
        Me.label2.Location = New System.Drawing.Point(472, 312)
        Me.label2.Name = "label2"
        Me.label2.Size = New System.Drawing.Size(64, 23)
        Me.label2.TabIndex = 3
        Me.label2.Text = "Right Offset"
        Me.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'label3
        '
        Me.label3.Location = New System.Drawing.Point(312, 344)
        Me.label3.Name = "label3"
        Me.label3.Size = New System.Drawing.Size(64, 23)
        Me.label3.TabIndex = 3
        Me.label3.Text = "Top Offset"
        Me.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'insetPlain
        '
        Me.insetPlain.Location = New System.Drawing.Point(176, 152)
        Me.insetPlain.Name = "insetPlain"
        Me.insetPlain.TabIndex = 1
        Me.insetPlain.Text = "Inset Plain"
        '
        'insetPagesOnly
        '
        Me.insetPagesOnly.Location = New System.Drawing.Point(176, 128)
        Me.insetPagesOnly.Name = "insetPagesOnly"
        Me.insetPagesOnly.Size = New System.Drawing.Size(120, 24)
        Me.insetPagesOnly.TabIndex = 4
        Me.insetPagesOnly.Text = "Inset Pages Only"
        '
        'showArrows
        '
        Me.showArrows.Location = New System.Drawing.Point(176, 104)
        Me.showArrows.Name = "showArrows"
        Me.showArrows.TabIndex = 1
        Me.showArrows.Text = "Show Arrows"
        '
        'radioMultiDocument
        '
        Me.radioMultiDocument.Location = New System.Drawing.Point(16, 16)
        Me.radioMultiDocument.Name = "radioMultiDocument"
        Me.radioMultiDocument.TabIndex = 0
        Me.radioMultiDocument.Text = "MultiDocument"
        '
        'AppearanceGroup
        '
        Me.AppearanceGroup.Controls.AddRange(New System.Windows.Forms.Control() {Me.radioMultiBox, Me.radioMultiForm, Me.radioMultiDocument})
        Me.AppearanceGroup.Location = New System.Drawing.Point(8, 96)
        Me.AppearanceGroup.Name = "AppearanceGroup"
        Me.AppearanceGroup.Size = New System.Drawing.Size(144, 96)
        Me.AppearanceGroup.TabIndex = 0
        Me.AppearanceGroup.TabStop = False
        Me.AppearanceGroup.Text = "Appearance"
        '
        'exampleColors
        '
        Me.exampleColors.Controls.AddRange(New System.Windows.Forms.Control() {Me.red, Me.blue, Me.normal})
        Me.exampleColors.Location = New System.Drawing.Point(8, 328)
        Me.exampleColors.Name = "exampleColors"
        Me.exampleColors.Size = New System.Drawing.Size(232, 64)
        Me.exampleColors.TabIndex = 1
        Me.exampleColors.TabStop = False
        Me.exampleColors.Text = "Example Colors"
        '
        'red
        '
        Me.red.Location = New System.Drawing.Point(160, 24)
        Me.red.Name = "red"
        Me.red.Size = New System.Drawing.Size(56, 24)
        Me.red.TabIndex = 0
        Me.red.Text = "Red"
        '
        'blue
        '
        Me.blue.Location = New System.Drawing.Point(88, 24)
        Me.blue.Name = "blue"
        Me.blue.Size = New System.Drawing.Size(88, 24)
        Me.blue.TabIndex = 0
        Me.blue.Text = "Blue"
        '
        'normal
        '
        Me.normal.Location = New System.Drawing.Point(16, 24)
        Me.normal.Name = "normal"
        Me.normal.Size = New System.Drawing.Size(88, 24)
        Me.normal.TabIndex = 0
        Me.normal.Text = "Normal"
        '
        'idePixelBorder
        '
        Me.idePixelBorder.Location = New System.Drawing.Point(176, 176)
        Me.idePixelBorder.Name = "idePixelBorder"
        Me.idePixelBorder.Size = New System.Drawing.Size(112, 24)
        Me.idePixelBorder.TabIndex = 5
        Me.idePixelBorder.Text = "IDE Pixel Border"
        '
        'idePixelArea
        '
        Me.idePixelArea.Location = New System.Drawing.Point(176, 200)
        Me.idePixelArea.Name = "idePixelArea"
        Me.idePixelArea.Size = New System.Drawing.Size(112, 24)
        Me.idePixelArea.TabIndex = 6
        Me.idePixelArea.Text = "IDE Pixel Area"
        '
        'multiline
        '
        Me.multiline.Location = New System.Drawing.Point(176, 272)
        Me.multiline.Name = "multiline"
        Me.multiline.Size = New System.Drawing.Size(112, 24)
        Me.multiline.TabIndex = 7
        Me.multiline.Text = "Multiline"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.AddRange(New System.Windows.Forms.Control() {Me.tabHideWithoutMouse, Me.tabHideUsingLogic, Me.tabHideAlways, Me.tabShowAlways})
        Me.GroupBox1.Location = New System.Drawing.Point(8, 200)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(144, 120)
        Me.GroupBox1.TabIndex = 2
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "HideTabsMode"
        '
        'tabHideWithoutMouse
        '
        Me.tabHideWithoutMouse.Location = New System.Drawing.Point(16, 88)
        Me.tabHideWithoutMouse.Name = "tabHideWithoutMouse"
        Me.tabHideWithoutMouse.Size = New System.Drawing.Size(120, 24)
        Me.tabHideWithoutMouse.TabIndex = 1
        Me.tabHideWithoutMouse.Text = "HideWithoutMouse"
        '
        'tabHideUsingLogic
        '
        Me.tabHideUsingLogic.Location = New System.Drawing.Point(16, 64)
        Me.tabHideUsingLogic.Name = "tabHideUsingLogic"
        Me.tabHideUsingLogic.TabIndex = 0
        Me.tabHideUsingLogic.Text = "HideUsingLogic"
        '
        'tabHideAlways
        '
        Me.tabHideAlways.Location = New System.Drawing.Point(16, 40)
        Me.tabHideAlways.Name = "tabHideAlways"
        Me.tabHideAlways.Size = New System.Drawing.Size(88, 24)
        Me.tabHideAlways.TabIndex = 0
        Me.tabHideAlways.Text = "HideAlways"
        '
        'tabShowAlways
        '
        Me.tabShowAlways.Location = New System.Drawing.Point(16, 16)
        Me.tabShowAlways.Name = "tabShowAlways"
        Me.tabShowAlways.TabIndex = 0
        Me.tabShowAlways.Text = "ShowAlways"
        '
        'hoverSelect
        '
        Me.hoverSelect.Location = New System.Drawing.Point(176, 248)
        Me.hoverSelect.Name = "hoverSelect"
        Me.hoverSelect.Size = New System.Drawing.Size(112, 24)
        Me.hoverSelect.TabIndex = 1
        Me.hoverSelect.Text = "Hover Select"
        '
        'multilineFullWidth
        '
        Me.multilineFullWidth.Location = New System.Drawing.Point(176, 296)
        Me.multilineFullWidth.Name = "multilineFullWidth"
        Me.multilineFullWidth.Size = New System.Drawing.Size(128, 24)
        Me.multilineFullWidth.TabIndex = 8
        Me.multilineFullWidth.Text = "Multiline Full Width"
        '
        'SampleTabControl
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(640, 429)
        Me.Controls.AddRange(New System.Windows.Forms.Control() {Me.multilineFullWidth, Me.multiline, Me.idePixelArea, Me.idePixelBorder, Me.insetPagesOnly, Me.hoverSelect, Me.selectedTextOnly, Me.numericUpDown3, Me.label3, Me.numericUpDown4, Me.label4, Me.numericUpDown2, Me.label2, Me.label1, Me.numericUpDown1, Me.insetPlain, Me.showArrows, Me.showClose, Me.tabControl, Me.shrinkPages, Me.addPage, Me.removePage, Me.clearAll, Me.hotTrack, Me.positionAtTop, Me.StyleGroup, Me.AppearanceGroup, Me.exampleColors, Me.GroupBox1})
        Me.Name = "SampleTabControl"
        Me.Text = "SampleTabControl"
        CType(Me.numericUpDown3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numericUpDown1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numericUpDown4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numericUpDown2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.StyleGroup.ResumeLayout(False)
        Me.AppearanceGroup.ResumeLayout(False)
        Me.exampleColors.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Protected Sub UpdateControls()
        Select Case tabControl.Appearance
            Case Crownwood.Magic.Controls.TabControl.VisualAppearance.MultiDocument
                _update = True
                radioMultiDocument.Select()
            Case Crownwood.Magic.Controls.TabControl.VisualAppearance.MultiForm
                _update = True
                radioMultiForm.Select()
            Case Crownwood.Magic.Controls.TabControl.VisualAppearance.MultiBox
                _update = True
                radioMultiBox.Select()
        End Select

        Select Case tabControl.Style
            Case VisualStyle.IDE
                _update = True
                radioIDE.Select()
            Case VisualStyle.Plain
                _update = True
                radioPlain.Select()
        End Select

        Select Case tabControl.HideTabsMode
            Case tabControl.HideTabsModes.ShowAlways
                _update = True
                tabShowAlways.Checked = True
            Case tabControl.HideTabsModes.HideAlways
                _update = True
                tabHideAlways.Checked = True
            Case tabControl.HideTabsModes.HideUsingLogic
                _update = True
                tabHideUsingLogic.Checked = True
            Case tabControl.HideTabsModes.HideWithoutMouse
                _update = True
                tabHideWithoutMouse.Checked = True
        End Select

        hotTrack.Checked = tabControl.HotTrack
        positionAtTop.Checked = tabControl.PositionTop
        shrinkPages.Checked = tabControl.ShrinkPagesToFit
        showClose.Checked = tabControl.ShowClose
        showArrows.Checked = tabControl.ShowArrows
        insetPlain.Checked = tabControl.InsetPlain
        idePixelBorder.Checked = tabControl.IDEPixelBorder
        idePixelArea.Checked = tabControl.IDEPixelArea
        insetPagesOnly.Checked = tabControl.InsetBorderPagesOnly
        selectedTextOnly.Checked = tabControl.SelectedTextOnly
        hoverSelect.Checked = tabControl.HoverSelect
        multiline.Checked = tabControl.Multiline
        multilineFullWidth.Checked = tabControl.MultilineFullWidth
        numericUpDown1.Value = tabControl.ControlLeftOffset
        numericUpDown2.Value = tabControl.ControlRightOffset
        numericUpDown3.Value = tabControl.ControlTopOffset
        numericUpDown4.Value = tabControl.ControlBottomOffset
    End Sub

    Protected Sub OnAddPage(ByVal sender As Object, ByVal e As EventArgs) Handles addPage.Click
        Dim controlToAdd As Control = Nothing

        Select Case _count
            Case 0, 2, 4, 6
                controlToAdd = New DummyForm()
                controlToAdd.BackColor = Color.White
            Case 1, 5
                Dim rtb As New RichTextBox()
                rtb.Text = "The quick brown fox jumped over the lazy dog."
                controlToAdd = rtb
            Case 3
                controlToAdd = New DummyPanel()
                controlToAdd.BackColor = Color.DarkSlateBlue
        End Select

        ' Define color that match the tabControl
        controlToAdd.ForeColor = tabControl.ForeColor
        controlToAdd.BackColor = tabControl.BackColor

        Dim page As Crownwood.Magic.Controls.TabPage

        ' Create a new page with the appropriate control for display, title text and image
        page = New Crownwood.Magic.Controls.TabPage(_strings(_count), controlToAdd, Nothing, _count)

        ' Make this page become selected when added
        page.Selected = True

        tabControl.TabPages.Add(page)

        ' Update the count for creating new pages
        _count += 1
        If (_count > 6) Then
            _count = 0
        End If
    End Sub

    Protected Sub OnRemovePage(ByVal sender As Object, ByVal e As EventArgs) Handles removePage.Click
        If (tabControl.TabPages.Count > 0) Then
            tabControl.TabPages.RemoveAt(0)
        End If
    End Sub

    Protected Sub OnClearAll(ByVal sender As Object, ByVal e As EventArgs) Handles clearAll.Click
        tabControl.TabPages.Clear()
    End Sub

    Protected Sub OnStyleIDE(ByVal sender As Object, ByVal e As EventArgs) Handles radioIDE.Click
        tabControl.Style = VisualStyle.IDE
        If Not _update Then
            UpdateControls()
        Else
            _update = False
        End If
    End Sub

    Protected Sub OnStylePlain(ByVal sender As Object, ByVal e As EventArgs) Handles radioPlain.Click
        tabControl.Style = VisualStyle.Plain
        If Not _update Then
            UpdateControls()
        Else
            _update = False
        End If
    End Sub

    Protected Sub OnAppearanceMultiBox(ByVal sender As Object, ByVal e As EventArgs) Handles radioMultiBox.Click
        tabControl.Appearance = Crownwood.Magic.Controls.TabControl.VisualAppearance.MultiBox
        If Not _update Then
            UpdateControls()
        Else
            _update = False
        End If
    End Sub

    Protected Sub OnAppearanceMultiForm(ByVal sender As Object, ByVal e As EventArgs) Handles radioMultiForm.Click
        tabControl.Appearance = Crownwood.Magic.Controls.TabControl.VisualAppearance.MultiForm
        If Not _update Then
            UpdateControls()
        Else
            _update = False
        End If
    End Sub

    Protected Sub OnAppearanceMultiDocument(ByVal sender As Object, ByVal e As EventArgs) Handles radioMultiDocument.Click
        tabControl.Appearance = Crownwood.Magic.Controls.TabControl.VisualAppearance.MultiDocument
        If Not _update Then
            UpdateControls()
        Else
            _update = False
        End If
    End Sub

    Protected Sub positionAtTop_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles positionAtTop.Click
        tabControl.PositionTop = positionAtTop.Checked
        UpdateControls()
    End Sub

    Protected Sub Highlight_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles hotTrack.Click
        tabControl.HotTrack = hotTrack.Checked
        UpdateControls()
    End Sub

    Protected Sub shrinkPages_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles shrinkPages.Click
        tabControl.ShrinkPagesToFit = shrinkPages.Checked
        UpdateControls()
    End Sub

    Protected Sub showClose_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles showClose.Click
        tabControl.ShowClose = showClose.Checked
        UpdateControls()
    End Sub

    Protected Sub showArrows_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles showArrows.Click
        tabControl.ShowArrows = showArrows.Checked
        UpdateControls()
    End Sub

    Protected Sub insetPlain_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles insetPlain.Click
        tabControl.InsetPlain = insetPlain.Checked
        UpdateControls()
    End Sub

    Protected Sub idePixelBorder_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles idePixelBorder.Click
        tabControl.IDEPixelBorder = idePixelBorder.Checked
        UpdateControls()
    End Sub

    Private Sub idePixelArea_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles idePixelArea.CheckedChanged
        tabControl.IDEPixelArea = idePixelArea.Checked
        UpdateControls()
    End Sub

    Protected Sub insetPagesOnly_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles insetPagesOnly.Click
        tabControl.InsetBorderPagesOnly = insetPagesOnly.Checked
        UpdateControls()
    End Sub

    Protected Sub selectedTextOnly_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles selectedTextOnly.Click
        tabControl.SelectedTextOnly = selectedTextOnly.Checked
        UpdateControls()
    End Sub

    Protected Sub numericUpDown1_ValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles numericUpDown1.Click
        tabControl.ControlLeftOffset = Val(numericUpDown1.Value)
        UpdateControls()
    End Sub

    Protected Sub numericUpDown3_ValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles numericUpDown3.Click
        tabControl.ControlTopOffset = Val(numericUpDown3.Value)
        UpdateControls()
    End Sub

    Protected Sub numericUpDown2_ValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles numericUpDown2.Click
        tabControl.ControlRightOffset = Val(numericUpDown2.Value)
        UpdateControls()
    End Sub

    Protected Sub numericUpDown4_ValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles numericUpDown4.Click
        tabControl.ControlBottomOffset = Val(numericUpDown4.Value)
        UpdateControls()
    End Sub

    Private Sub tabShowAlways_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tabShowAlways.CheckedChanged
        tabControl.HideTabsMode = Crownwood.Magic.Controls.TabControl.HideTabsModes.ShowAlways
        If Not _update Then
            UpdateControls()
        Else
            _update = False
        End If
    End Sub

    Private Sub tabHideAlways_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tabHideAlways.CheckedChanged
        tabControl.HideTabsMode = Crownwood.Magic.Controls.TabControl.HideTabsModes.HideAlways
        If Not _update Then
            UpdateControls()
        Else
            _update = False
        End If
    End Sub

    Private Sub tabHideUsingLogic_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tabHideUsingLogic.CheckedChanged
        tabControl.HideTabsMode = Crownwood.Magic.Controls.TabControl.HideTabsModes.HideUsingLogic
        If Not _update Then
            UpdateControls()
        Else
            _update = False
        End If
    End Sub

    Private Sub tabHideWithoutMouse_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tabHideWithoutMouse.CheckedChanged
        tabControl.HideTabsMode = Crownwood.Magic.Controls.TabControl.HideTabsModes.HideWithoutMouse
        If Not _update Then
            UpdateControls()
        Else
            _update = False
        End If
    End Sub

    Protected Sub hoverSelect_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles hoverSelect.Click
        tabControl.HoverSelect = hoverSelect.Checked
        UpdateControls()
    End Sub

    Private Sub multiline_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles multiline.CheckedChanged
        tabControl.Multiline = multiline.Checked
        UpdateControls()
    End Sub

    Private Sub multilineFullWidth_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles multilineFullWidth.CheckedChanged
        tabControl.MultilineFullWidth = multilineFullWidth.Checked
        UpdateControls()
    End Sub

    Protected Sub normal_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles normal.Click
        ' Give the tabControl a blue appearance
        tabControl.BackColor = _startBackColor
        tabControl.ForeColor = _startForeColor
        tabControl.ButtonActiveColor = _startButtonActive
        tabControl.ButtonInactiveColor = _startButtonInactive
        tabControl.TextInactiveColor = _startTextInactiveColor
        tabControl.HotTextColor = _startHotTextColor

        DefinePageColors(_startBackColor, _startForeColor)
    End Sub

    Protected Sub blue_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles blue.Click
        ' Give the tabControl a blue appearance
        tabControl.BackColor = Color.DarkBlue
        tabControl.ForeColor = Color.White
        tabControl.ButtonActiveColor = Color.White
        tabControl.ButtonInactiveColor = Color.LightBlue
        tabControl.TextInactiveColor = Color.Yellow
        tabControl.HotTextColor = Color.Orange

        DefinePageColors(Color.DarkBlue, Color.White)
    End Sub

    Protected Sub red_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles red.Click
        ' Give the tabControl a red appearance
        tabControl.BackColor = Color.DarkRed
        tabControl.ForeColor = Color.White
        tabControl.ButtonActiveColor = Color.White
        tabControl.ButtonInactiveColor = Color.Red
        tabControl.TextInactiveColor = Color.White
        tabControl.HotTextColor = Color.Cyan

        DefinePageColors(Color.DarkRed, Color.White)
    End Sub

    Protected Sub DefinePageColors(ByVal newBack As Color, ByVal newFore As Color)
        Dim page As Crownwood.Magic.Controls.TabPage
        For Each page In tabControl.TabPages
            If Not (page.Control Is Nothing) Then
                page.Control.ForeColor = newFore
                page.Control.BackColor = newBack
            End If
        Next
    End Sub
End Class

Public Class DummyForm
    Inherits Form

    Private _dummy1 As New Button()
    Private _dummy2 As New Button()
    Private _dummyBox As New GroupBox()
    Private _dummy3 As New RadioButton()
    Private _dummy4 As New RadioButton()

    Public Sub New()
        _dummy1.Text = "Dummy 1"
        _dummy1.Size = New Size(90, 25)
        _dummy1.Location = New Point(10, 10)

        _dummy2.Text = "Dummy 2"
        _dummy2.Size = New Size(90, 25)
        _dummy2.Location = New Point(110, 10)

        _dummyBox.Text = "Form GroupBox"
        _dummyBox.Size = New Size(190, 67)
        _dummyBox.Location = New Point(10, 45)

        _dummy3.Text = "Dummy 3"
        _dummy3.Size = New Size(100, 22)
        _dummy3.Location = New Point(10, 20)

        _dummy4.Text = "Dummy 4"
        _dummy4.Size = New Size(100, 22)
        _dummy4.Location = New Point(10, 42)
        _dummy4.Checked = True

        _dummyBox.Controls.AddRange(New Control() {_dummy3, _dummy4})

        Controls.AddRange(New Control() {_dummy1, _dummy2, _dummyBox})

        ' Define then control to be selected when first is activated for first time
        Me.ActiveControl = _dummy4
    End Sub
End Class

Public Class DummyPanel
    Inherits Panel

    Private _dummyBox As New GroupBox()
    Private _dummy3 As New RadioButton()
    Private _dummy4 As New RadioButton()

    Public Sub New()
        _dummyBox.Text = "Panel GroupBox"
        _dummyBox.Size = New Size(190, 67)
        _dummyBox.Location = New Point(10, 10)

        _dummy3.Text = "RadioButton 3"
        _dummy3.Size = New Size(100, 22)
        _dummy3.Location = New Point(10, 20)

        _dummy4.Text = "RadioButton 4"
        _dummy4.Size = New Size(100, 22)
        _dummy4.Location = New Point(10, 42)
        _dummy4.Checked = True

        _dummyBox.Controls.AddRange(New Control() {_dummy3, _dummy4})

        Controls.AddRange(New Control() {_dummyBox})
    End Sub
End Class