﻿' R- Instat
' Copyright (C) 2015-2017
'
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY; without even the implied warranty of
' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
' GNU General Public License for more details.
'
' You should have received a copy of the GNU General Public License 
' along with this program.  If not, see <http://www.gnu.org/licenses/>.

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class dlgSort
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.grpOrder = New System.Windows.Forms.GroupBox()
        Me.rdoDescending = New System.Windows.Forms.RadioButton()
        Me.rdoAscending = New System.Windows.Forms.RadioButton()
        Me.ucrPnlOrder = New instat.UcrPanel()
        Me.lblColumnsToSort = New System.Windows.Forms.Label()
        Me.grpMissingValues = New System.Windows.Forms.GroupBox()
        Me.rdoLast = New System.Windows.Forms.RadioButton()
        Me.rdoFirst = New System.Windows.Forms.RadioButton()
        Me.ucrPnlMissingValues = New instat.UcrPanel()
        Me.ucrSelectForSort = New instat.ucrSelectorByDataFrameAddRemove()
        Me.ucrReceiverSort = New instat.ucrReceiverMultiple()
        Me.ucrBase = New instat.ucrButtons()
        Me.grpOrder.SuspendLayout()
        Me.grpMissingValues.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpOrder
        '
        Me.grpOrder.Controls.Add(Me.rdoDescending)
        Me.grpOrder.Controls.Add(Me.rdoAscending)
        Me.grpOrder.Controls.Add(Me.ucrPnlOrder)
        Me.grpOrder.Location = New System.Drawing.Point(382, 244)
        Me.grpOrder.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.grpOrder.Name = "grpOrder"
        Me.grpOrder.Padding = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.grpOrder.Size = New System.Drawing.Size(180, 104)
        Me.grpOrder.TabIndex = 3
        Me.grpOrder.TabStop = False
        Me.grpOrder.Tag = "Order"
        Me.grpOrder.Text = "Order"
        '
        'rdoDescending
        '
        Me.rdoDescending.AutoSize = True
        Me.rdoDescending.Location = New System.Drawing.Point(15, 63)
        Me.rdoDescending.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.rdoDescending.Name = "rdoDescending"
        Me.rdoDescending.Size = New System.Drawing.Size(119, 24)
        Me.rdoDescending.TabIndex = 2
        Me.rdoDescending.TabStop = True
        Me.rdoDescending.Tag = "Descending"
        Me.rdoDescending.Text = "Descending"
        Me.rdoDescending.UseVisualStyleBackColor = True
        '
        'rdoAscending
        '
        Me.rdoAscending.AutoSize = True
        Me.rdoAscending.Location = New System.Drawing.Point(15, 28)
        Me.rdoAscending.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.rdoAscending.Name = "rdoAscending"
        Me.rdoAscending.Size = New System.Drawing.Size(109, 24)
        Me.rdoAscending.TabIndex = 1
        Me.rdoAscending.TabStop = True
        Me.rdoAscending.Tag = "Ascending"
        Me.rdoAscending.Text = "Ascending"
        Me.rdoAscending.UseVisualStyleBackColor = True
        '
        'ucrPnlOrder
        '
        Me.ucrPnlOrder.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.ucrPnlOrder.Location = New System.Drawing.Point(9, 18)
        Me.ucrPnlOrder.Margin = New System.Windows.Forms.Padding(9, 9, 9, 9)
        Me.ucrPnlOrder.Name = "ucrPnlOrder"
        Me.ucrPnlOrder.Size = New System.Drawing.Size(162, 76)
        Me.ucrPnlOrder.TabIndex = 0
        '
        'lblColumnsToSort
        '
        Me.lblColumnsToSort.AutoSize = True
        Me.lblColumnsToSort.Location = New System.Drawing.Point(381, 68)
        Me.lblColumnsToSort.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblColumnsToSort.Name = "lblColumnsToSort"
        Me.lblColumnsToSort.Size = New System.Drawing.Size(147, 20)
        Me.lblColumnsToSort.TabIndex = 1
        Me.lblColumnsToSort.Tag = ""
        Me.lblColumnsToSort.Text = "Columns to Sort by:"
        '
        'grpMissingValues
        '
        Me.grpMissingValues.Controls.Add(Me.rdoLast)
        Me.grpMissingValues.Controls.Add(Me.rdoFirst)
        Me.grpMissingValues.Controls.Add(Me.ucrPnlMissingValues)
        Me.grpMissingValues.Location = New System.Drawing.Point(382, 357)
        Me.grpMissingValues.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.grpMissingValues.Name = "grpMissingValues"
        Me.grpMissingValues.Padding = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.grpMissingValues.Size = New System.Drawing.Size(180, 104)
        Me.grpMissingValues.TabIndex = 4
        Me.grpMissingValues.TabStop = False
        Me.grpMissingValues.Tag = "Missing_Values"
        Me.grpMissingValues.Text = "Missing Values"
        '
        'rdoLast
        '
        Me.rdoLast.AutoSize = True
        Me.rdoLast.Location = New System.Drawing.Point(15, 63)
        Me.rdoLast.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.rdoLast.Name = "rdoLast"
        Me.rdoLast.Size = New System.Drawing.Size(65, 24)
        Me.rdoLast.TabIndex = 2
        Me.rdoLast.TabStop = True
        Me.rdoLast.Tag = "Last"
        Me.rdoLast.Text = "Last"
        Me.rdoLast.UseVisualStyleBackColor = True
        '
        'rdoFirst
        '
        Me.rdoFirst.AutoSize = True
        Me.rdoFirst.Location = New System.Drawing.Point(15, 28)
        Me.rdoFirst.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.rdoFirst.Name = "rdoFirst"
        Me.rdoFirst.Size = New System.Drawing.Size(65, 24)
        Me.rdoFirst.TabIndex = 1
        Me.rdoFirst.TabStop = True
        Me.rdoFirst.Tag = "First"
        Me.rdoFirst.Text = "First"
        Me.rdoFirst.UseVisualStyleBackColor = True
        '
        'ucrPnlMissingValues
        '
        Me.ucrPnlMissingValues.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.ucrPnlMissingValues.Location = New System.Drawing.Point(8, 24)
        Me.ucrPnlMissingValues.Margin = New System.Windows.Forms.Padding(9, 9, 9, 9)
        Me.ucrPnlMissingValues.Name = "ucrPnlMissingValues"
        Me.ucrPnlMissingValues.Size = New System.Drawing.Size(164, 64)
        Me.ucrPnlMissingValues.TabIndex = 0
        '
        'ucrSelectForSort
        '
        Me.ucrSelectForSort.AutoSize = True
        Me.ucrSelectForSort.bDropUnusedFilterLevels = False
        Me.ucrSelectForSort.bShowHiddenColumns = False
        Me.ucrSelectForSort.bUseCurrentFilter = True
        Me.ucrSelectForSort.Location = New System.Drawing.Point(15, 15)
        Me.ucrSelectForSort.Margin = New System.Windows.Forms.Padding(0)
        Me.ucrSelectForSort.Name = "ucrSelectForSort"
        Me.ucrSelectForSort.Size = New System.Drawing.Size(320, 274)
        Me.ucrSelectForSort.TabIndex = 0
        '
        'ucrReceiverSort
        '
        Me.ucrReceiverSort.AutoSize = True
        Me.ucrReceiverSort.frmParent = Me
        Me.ucrReceiverSort.Location = New System.Drawing.Point(382, 90)
        Me.ucrReceiverSort.Margin = New System.Windows.Forms.Padding(0)
        Me.ucrReceiverSort.Name = "ucrReceiverSort"
        Me.ucrReceiverSort.Selector = Nothing
        Me.ucrReceiverSort.Size = New System.Drawing.Size(180, 150)
        Me.ucrReceiverSort.strNcFilePath = ""
        Me.ucrReceiverSort.TabIndex = 2
        Me.ucrReceiverSort.ucrSelector = Nothing
        '
        'ucrBase
        '
        Me.ucrBase.AutoSize = True
        Me.ucrBase.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.ucrBase.Location = New System.Drawing.Point(15, 470)
        Me.ucrBase.Margin = New System.Windows.Forms.Padding(9, 9, 9, 9)
        Me.ucrBase.Name = "ucrBase"
        Me.ucrBase.Size = New System.Drawing.Size(611, 77)
        Me.ucrBase.TabIndex = 5
        '
        'dlgSort
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(144.0!, 144.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoSize = True
        Me.ClientSize = New System.Drawing.Size(622, 558)
        Me.Controls.Add(Me.ucrSelectForSort)
        Me.Controls.Add(Me.grpMissingValues)
        Me.Controls.Add(Me.lblColumnsToSort)
        Me.Controls.Add(Me.ucrReceiverSort)
        Me.Controls.Add(Me.grpOrder)
        Me.Controls.Add(Me.ucrBase)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "dlgSort"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Tag = "Sort"
        Me.Text = "Sort"
        Me.grpOrder.ResumeLayout(False)
        Me.grpOrder.PerformLayout()
        Me.grpMissingValues.ResumeLayout(False)
        Me.grpMissingValues.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents ucrBase As ucrButtons
    Friend WithEvents grpOrder As GroupBox
    Friend WithEvents rdoDescending As RadioButton
    Friend WithEvents rdoAscending As RadioButton
    Friend WithEvents ucrReceiverSort As ucrReceiverMultiple
    Friend WithEvents lblColumnsToSort As Label
    Friend WithEvents grpMissingValues As GroupBox
    Friend WithEvents ucrSelectForSort As ucrSelectorByDataFrameAddRemove
    Friend WithEvents ucrPnlOrder As UcrPanel
    Friend WithEvents rdoLast As RadioButton
    Friend WithEvents rdoFirst As RadioButton
    Friend WithEvents ucrPnlMissingValues As UcrPanel
End Class
