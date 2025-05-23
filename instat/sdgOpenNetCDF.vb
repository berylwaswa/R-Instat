﻿' R-Instat
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

Imports System.ComponentModel
Imports instat
Imports instat.Translations
Imports RDotNet

Public Class sdgOpenNetCDF
    Private clsImportNetcdfFunction, clsNcOpenFunction As New RFunction
    Private clsBoundaryListFunction, clsYLimitsFunction, clsXLimitsFunction, clsZLimitsFunction, clsSLimitsFunction, clsTLimitsFunction As New RFunction
    Private clsAsDateMin, clsAsDateMax As New RFunction
    Public bControlsInitialised As Boolean = False

    'We need to initialise the lists to be empty lists.
    ' If we don't do this then, when we build the translations database, a null reference exception 
    ' is triggered in `sdgOpenNetCDF_Closing()`.
    Private lstMinTextBoxes = New List(Of ucrInputTextBox)
    Private lstMaxTextBoxes = New List(Of ucrInputTextBox)
    Private lstAxesLabels = New List(Of Label)
    Private lstMinLabels = New List(Of Label)
    Private lstMaxLabels = New List(Of Label)
    Private lstDims = New List(Of String)
    Private lstAxesDetected = New List(Of Boolean)
    Private lstFunctions = New List(Of RFunction)

    Private strFilePath As String
    Private dctAxesNames As New Dictionary(Of String, String)
    Private bUpdating As Boolean = False
    Public bOKEnabled As Boolean = True
    Private bMultiImport As Boolean = False

    Private Sub sdgOpenNetCDF_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetHelpOptions()
        autoTranslate(Me)
    End Sub

    Public Sub InitialiseControls()

        clsAsDateMin.SetRCommand("as.Date")
        clsAsDateMax.SetRCommand("as.Date")

        ucrBase.iHelpTopicID = 117

        ucrInputFileDetails.txtInput.ScrollBars = ScrollBars.Vertical

        ucrInputMinX.SetParameter(New RParameter("min", 0, bNewIncludeArgumentName:=False))
        ucrInputMinX.SetValidationTypeAsNumeric()
        ucrInputMinX.AddQuotesIfUnrecognised = False
        ucrInputMinX.Focus()
        ucrInputMinX.SetLinkedDisplayControl(lblMinX)

        ucrInputMaxX.SetParameter(New RParameter("max", 1, bNewIncludeArgumentName:=False))
        ucrInputMaxX.SetValidationTypeAsNumeric()
        ucrInputMaxX.AddQuotesIfUnrecognised = False
        ucrInputMaxX.SetLinkedDisplayControl(lblMaxX)

        ucrInputMinY.SetParameter(New RParameter("min", 0, bNewIncludeArgumentName:=False))
        ucrInputMinY.SetValidationTypeAsNumeric()
        ucrInputMinY.AddQuotesIfUnrecognised = False
        ucrInputMinY.SetLinkedDisplayControl(lblMinY)

        ucrInputMaxY.SetParameter(New RParameter("max", 1, bNewIncludeArgumentName:=False))
        ucrInputMaxY.SetValidationTypeAsNumeric()
        ucrInputMaxY.AddQuotesIfUnrecognised = False
        ucrInputMaxY.SetLinkedDisplayControl(lblMaxY)

        ucrInputMinZ.SetParameter(New RParameter("min", 0, bNewIncludeArgumentName:=False))
        ucrInputMinZ.SetValidationTypeAsNumeric()
        ucrInputMinZ.AddQuotesIfUnrecognised = False
        ucrInputMinZ.SetLinkedDisplayControl(lblMinZ)

        ucrInputMaxZ.SetParameter(New RParameter("max", 1, bNewIncludeArgumentName:=False))
        ucrInputMaxZ.SetValidationTypeAsNumeric()
        ucrInputMaxZ.AddQuotesIfUnrecognised = False
        ucrInputMaxZ.SetLinkedDisplayControl(lblMaxZ)

        ucrInputMinS.SetParameter(New RParameter("min", 0, bNewIncludeArgumentName:=False))
        ucrInputMinS.SetValidationTypeAsNumeric()
        ucrInputMinS.AddQuotesIfUnrecognised = False
        ucrInputMinS.SetLinkedDisplayControl(lblMinS)

        ucrInputMaxS.SetParameter(New RParameter("max", 1, bNewIncludeArgumentName:=False))
        ucrInputMaxS.SetValidationTypeAsNumeric()
        ucrInputMaxS.AddQuotesIfUnrecognised = False
        ucrInputMaxS.SetLinkedDisplayControl(lblMaxS)

        'Temp disabled until code is stable with only_data_vars = False
        ucrChkOnlyDataVariables.Enabled = False

        ucrChkOnlyDataVariables.SetParameter(New RParameter("only_data_vars", 2))
        ucrChkOnlyDataVariables.SetText("Only Include Data Variables")
        ucrChkOnlyDataVariables.SetRDefault("TRUE")

        ucrChkKeepRawTime.SetParameter(New RParameter("keep_raw_time", 3))
        ucrChkKeepRawTime.SetText("Keep Raw Time")
        ucrChkKeepRawTime.SetRDefault("TRUE")

        ucrChkIncludeMetadata.SetParameter(New RParameter("include_metadata", 4))
        ucrChkIncludeMetadata.SetText("Include Metadata")
        ucrChkIncludeMetadata.SetRDefault("TRUE")

        ucrChkIncludeRequestedPoints.SetParameter(New RParameter("show_requested_points", 5))
        ucrChkIncludeRequestedPoints.SetText("Include requested points in data (if specified)")
        ucrChkIncludeRequestedPoints.SetRDefault("TRUE")

        ucrChkGreatCircleDist.SetParameter(New RParameter("great_circle_dist", 10))
        ucrChkGreatCircleDist.SetText("Use Great Circle (WGS84 ellipsoid) distance for nearest points")
        ucrChkGreatCircleDist.SetRDefault("FALSE")

        ucrReceiverPointsX.Selector = ucrSelectorPoints
        ucrReceiverPointsY.Selector = ucrSelectorPoints
        ucrReceiverPointsID.Selector = ucrSelectorPoints
        ucrReceiverPointsID.SetLinkedDisplayControl(lblPointsID)

        ucrPnlLocation.AddRadioButton(rdoRange)
        ucrPnlLocation.AddRadioButton(rdoPoints)
        ucrPnlLocation.AddRadioButton(rdoSinglePoint)

        ucrPnlLocation.AddParameterPresentCondition(rdoRange, "lon_points", False)
        ucrPnlLocation.AddParameterPresentCondition(rdoPoints, "lon_points", True)
        ucrPnlLocation.AddParameterIsRFunctionCondition(rdoPoints, "lon_points", True)
        ucrPnlLocation.AddParameterPresentCondition(rdoSinglePoint, "lon_points", True)
        ucrPnlLocation.AddParameterIsRFunctionCondition(rdoSinglePoint, "lon_points", False)

        lstDims = New List(Of String)(New String() {"X", "Y", "Z", "S", "T"})
        lstAxesDetected = New List(Of Boolean)(New Boolean() {False, False, False, False, False})
        lstMinTextBoxes = New List(Of ucrInputTextBox)(New ucrInputTextBox() {ucrInputMinX, ucrInputMinY, ucrInputMinZ, ucrInputMinS})
        lstMaxTextBoxes = New List(Of ucrInputTextBox)(New ucrInputTextBox() {ucrInputMaxX, ucrInputMaxY, ucrInputMaxZ, ucrInputMaxS})
        lstAxesLabels = New List(Of Label)(New Label() {lblX, lblY, lblZ, lblS, lblT})
        lstMinLabels = New List(Of Label)(New Label() {lblMinX, lblMinY, lblMinZ, lblMinS, lblMinT})
        lstMaxLabels = New List(Of Label)(New Label() {lblMaxX, lblMaxY, lblMaxZ, lblMaxS, lblMaxT})
        InitialiseTabs()
        bControlsInitialised = True
    End Sub

    Public Sub SetRFunction(clsNewImportNetcdfFunction As RFunction, clsNewNcOpenFunction As RFunction, clsNewBoundaryListFunction As RFunction, clsNewXLimitsFunction As RFunction, clsNewYLimitsFunction As RFunction, clsNewZLimitsFunction As RFunction, clsNewSLimitsFunction As RFunction, clsNewTLimitsFunction As RFunction, strNewShortDescription As String, bNewMultiImport As Boolean, Optional bReset As Boolean = False)
        Dim numMinMax As NumericVector
        Dim chrMinMaxDates As CharacterVector
        Dim dcmMin As Nullable(Of Decimal)
        Dim dcmMax As Nullable(Of Decimal)
        Dim strMinDate As String
        Dim strMaxDate As String
        Dim strMinDateSplit() As String
        Dim strMaxDateSplit() As String
        Dim iIndex As Integer
        Dim clsGetDimNames As New RFunction
        Dim clsGetBoundsFunction As New RFunction
        Dim expTemp As SymbolicExpression
        Dim bShowDimension As Boolean
        Dim dtMin As DateTime
        Dim dtMax As DateTime
        Dim strTemp As String
        Dim strScript As String = ""
        Dim lstDimNames As List(Of String)
        Dim lstDimAxes As List(Of String)
        Dim iTIndex As Integer

        bUpdating = True
        If Not bControlsInitialised Then
            InitialiseControls()
        End If
        clsImportNetcdfFunction = clsNewImportNetcdfFunction
        clsNcOpenFunction = clsNewNcOpenFunction
        clsBoundaryListFunction = clsNewBoundaryListFunction
        clsXLimitsFunction = clsNewXLimitsFunction
        clsYLimitsFunction = clsNewYLimitsFunction
        clsZLimitsFunction = clsNewZLimitsFunction
        clsSLimitsFunction = clsNewSLimitsFunction
        clsTLimitsFunction = clsNewTLimitsFunction

        clsTLimitsFunction.AddParameter("min", clsRFunctionParameter:=clsAsDateMin)
        clsTLimitsFunction.AddParameter("max", clsRFunctionParameter:=clsAsDateMax)
        lstFunctions = New List(Of RFunction)(New RFunction() {clsXLimitsFunction, clsYLimitsFunction, clsZLimitsFunction, clsSLimitsFunction, clsTLimitsFunction})

        ucrInputFileDetails.SetName(strNewShortDescription)

        bMultiImport = bNewMultiImport

        clsGetDimNames.SetPackageName("ncdf4.helpers")
        clsGetDimNames.SetRCommand("nc.get.dim.axes")
        clsGetDimNames.AddParameter("f", clsRFunctionParameter:=clsNcOpenFunction)
        strTemp = clsGetDimNames.ToScript(strScript)
        expTemp = frmMain.clsRLink.RunInternalScriptGetValue(strTemp, bSilent:=True)
        If expTemp IsNot Nothing AndAlso expTemp.Type <> Internals.SymbolicExpressionType.Null Then
            lstDimNames = expTemp.AsCharacter.Names.ToList
            lstDimAxes = expTemp.AsCharacter.ToList
            If bMultiImport Then
                If lstDimAxes.Contains("T") Then
                    iTIndex = lstDimAxes.IndexOf("T")
                    lstDimAxes.RemoveAt(iTIndex)
                    lstDimNames.RemoveAt(iTIndex)
                End If
            End If
            dctAxesNames = New Dictionary(Of String, String)
            For i As Integer = 0 To lstDimNames.Count - 1
                If lstDimAxes(i) IsNot Nothing Then
                    dctAxesNames.Add(lstDimAxes(i), lstDimNames(i))
                End If
            Next
        Else
            lstDimNames = Nothing
            lstDimAxes = Nothing
        End If
        If lstDimAxes IsNot Nothing Then
            clsGetBoundsFunction.SetPackageName("instatExtras")
            clsGetBoundsFunction.SetRCommand("nc_get_dim_min_max")
            clsGetBoundsFunction.AddParameter("nc", clsRFunctionParameter:=clsNcOpenFunction)
            For i As Integer = 0 To lstDims.Count - 1
                dcmMin = Nothing
                dcmMax = Nothing
                strMinDate = ""
                strMaxDate = ""
                If lstDimAxes.Contains(lstDims(i)) Then
                    iIndex = lstDimAxes.IndexOf(lstDims(i))
                    clsGetBoundsFunction.AddParameter("dimension", Chr(34) & lstDimNames(iIndex) & Chr(34))
                    expTemp = frmMain.clsRLink.RunInternalScriptGetValue(clsGetBoundsFunction.ToScript, bSilent:=True)
                    If expTemp IsNot Nothing AndAlso expTemp.Type <> Internals.SymbolicExpressionType.Null Then
                        If lstDims(i) = "T" Then
                            chrMinMaxDates = expTemp.AsCharacter
                            If chrMinMaxDates.Count = 2 Then
                                strMinDate = chrMinMaxDates(0)
                                strMaxDate = chrMinMaxDates(1)
                            End If
                        Else
                            numMinMax = expTemp.AsNumeric
                            If numMinMax.Count = 2 Then
                                dcmMin = numMinMax(0)
                                dcmMax = numMinMax(1)
                            End If
                        End If
                    End If
                End If
                If lstDims(i) = "T" Then
                    bShowDimension = True
                    If strMinDate <> "" AndAlso strMaxDate <> "" Then
                        strMinDateSplit = strMinDate.Split("-")
                        strMaxDateSplit = strMaxDate.Split("-")
                        If strMinDateSplit.Count = 3 AndAlso strMaxDateSplit.Count = 3 Then
                            Try
                                dtMin = New DateTime(strMinDateSplit(0), strMinDateSplit(1), strMinDateSplit(2))
                                dtMax = New DateTime(strMaxDateSplit(0), strMaxDateSplit(1), strMaxDateSplit(2))
                                dtpMinT.MinDate = dtMin
                                dtpMinT.MaxDate = dtMax
                                dtpMaxT.MinDate = dtMin
                                dtpMaxT.MaxDate = dtMax
                                dtpMinT.Value = dtMin
                                dtpMaxT.Value = dtMax
                            Catch ex As Exception
                                bShowDimension = False
                                lstAxesLabels(i).Text = GetTranslation("Could not read time dimension dates from file.")
                                lstAxesDetected(i) = False
                            End Try
                            If bShowDimension Then
                                lstAxesLabels(i).Text = lstDimNames(iIndex) & ":"
                                lstAxesDetected(i) = True
                            End If
                        Else
                            bShowDimension = False
                            lstAxesLabels(i).Text = "No " & lstDims(i) & " dimension."
                            lstAxesDetected(i) = False
                        End If
                    Else
                        bShowDimension = False
                        lstAxesLabels(i).Text = "No " & lstDims(i) & " dimension."
                        lstAxesDetected(i) = False
                    End If
                    If bShowDimension Then
                        dtpMinT.Visible = True
                        dtpMaxT.Visible = True
                        lstMinLabels(i).Visible = True
                        lstMaxLabels(i).Visible = True
                        clsBoundaryListFunction.AddParameter(lstDimNames(iIndex), clsRFunctionParameter:=lstFunctions(i))
                    Else
                        dtpMinT.Visible = False
                        dtpMaxT.Visible = False
                        lstMinLabels(i).Visible = False
                        lstMaxLabels(i).Visible = False
                    End If
                Else
                    If dcmMin.HasValue AndAlso dcmMax.HasValue Then
                        lstMinTextBoxes(i).Visible = True
                        lstMaxTextBoxes(i).Visible = True
                        lstMinLabels(i).Visible = True
                        lstMaxLabels(i).Visible = True
                        lstMinTextBoxes(i).SetValidationTypeAsNumeric(dcmMin:=dcmMin, dcmMax:=dcmMax)
                        lstMaxTextBoxes(i).SetValidationTypeAsNumeric(dcmMin:=dcmMin, dcmMax:=dcmMax)
                        lstFunctions(i).AddParameter("min", dcmMin, bIncludeArgumentName:=False)
                        lstFunctions(i).AddParameter("max", dcmMax, bIncludeArgumentName:=False)
                        clsBoundaryListFunction.AddParameter(lstDimNames(iIndex), clsRFunctionParameter:=lstFunctions(i))
                        If lstDimNames(iIndex).ToLower = "x" Then
                            lstAxesLabels(i).Text = lstDimNames(iIndex) & GetTranslation(" (lon) ") & ":"
                        ElseIf lstDimNames(iIndex).ToLower = "y" Then
                            lstAxesLabels(i).Text = lstDimNames(iIndex) & GetTranslation(" (lat) ") & ":"
                        Else
                            lstAxesLabels(i).Text = lstDimNames(iIndex) & ":"
                        End If
                        lstAxesDetected(i) = True
                    Else
                        lstMinTextBoxes(i).Visible = False
                        lstMaxTextBoxes(i).Visible = False
                        lstMinLabels(i).Visible = False
                        lstMaxLabels(i).Visible = False
                        lstAxesLabels(i).Text = "No " & lstDims(i) & " dimension detected."
                        lstAxesDetected(i) = False
                    End If
                End If
            Next
            If clsBoundaryListFunction.clsParameters.Count > 0 Then
                clsImportNetcdfFunction.AddParameter("boundary", clsRFunctionParameter:=clsBoundaryListFunction)
            End If
        End If

        ucrInputMinX.SetRCode(clsXLimitsFunction, bReset, bCloneIfNeeded:=True)
        ucrInputMaxX.SetRCode(clsXLimitsFunction, bReset, bCloneIfNeeded:=True)
        ucrInputMinY.SetRCode(clsYLimitsFunction, bReset, bCloneIfNeeded:=True)
        ucrInputMaxY.SetRCode(clsYLimitsFunction, bReset, bCloneIfNeeded:=True)
        ucrInputMinZ.SetRCode(clsZLimitsFunction, bReset, bCloneIfNeeded:=True)
        ucrInputMaxZ.SetRCode(clsZLimitsFunction, bReset, bCloneIfNeeded:=True)
        ucrInputMinS.SetRCode(clsSLimitsFunction, bReset, bCloneIfNeeded:=True)
        ucrInputMaxS.SetRCode(clsSLimitsFunction, bReset, bCloneIfNeeded:=True)

        ucrChkOnlyDataVariables.SetRCode(clsImportNetcdfFunction, bReset, bCloneIfNeeded:=True)
        ucrChkKeepRawTime.SetRCode(clsImportNetcdfFunction, bReset, bCloneIfNeeded:=True)
        ucrChkIncludeMetadata.SetRCode(clsImportNetcdfFunction, bReset, bCloneIfNeeded:=True)
        ucrChkIncludeRequestedPoints.SetRCode(clsImportNetcdfFunction, bReset, bCloneIfNeeded:=True)
        ucrChkGreatCircleDist.SetRCode(clsImportNetcdfFunction, bReset, bCloneIfNeeded:=True)

        ucrPnlLocation.SetRCode(clsImportNetcdfFunction, bReset, bCloneIfNeeded:=True)

        If bReset Then
            ucrSelectorPoints.Reset()
            ucrReceiverPointsX.SetMeAsReceiver()
        End If
        bUpdating = False
        SetLocationParameters()
    End Sub

    Private Sub InitialiseTabs()
        For i = 0 To tbNetCDF.TabCount - 1
            tbNetCDF.SelectedIndex = i
        Next
        tbNetCDF.SelectedIndex = 0
    End Sub

    Private Sub dtpMinT_ValueChanged(sender As Object, e As EventArgs) Handles dtpMinT.ValueChanged
        clsAsDateMin.AddParameter("x", Chr(34) & dtpMinT.Value.Year & "-" & dtpMinT.Value.Month & "-" & dtpMinT.Value.Day & Chr(34), iPosition:=0)
        clsAsDateMin.AddParameter("format", Chr(34) & "%Y-%m-%d" & Chr(34), iPosition:=1)
    End Sub

    Private Sub dtpMaxT_ValueChanged(sender As Object, e As EventArgs) Handles dtpMaxT.ValueChanged
        clsAsDateMax.AddParameter("x", Chr(34) & dtpMaxT.Value.Year & "-" & dtpMaxT.Value.Month & "-" & dtpMaxT.Value.Day & Chr(34), iPosition:=0)
        clsAsDateMax.AddParameter("format", Chr(34) & "%Y-%m-%d" & Chr(34), iPosition:=1)
    End Sub

    Private Sub ucrPnlLocation_ControlValueChanged(ucrChangedControl As ucrCore) Handles ucrPnlLocation.ControlValueChanged, ucrInputPointX.ControlValueChanged, ucrInputPointY.ControlValueChanged, ucrReceiverPointsX.ControlValueChanged, ucrReceiverPointsY.ControlValueChanged, ucrReceiverPointsID.ControlValueChanged
        SetLocationParameters()
    End Sub

    Private Sub SetLocationParameters()
        'TODO If x or y are not detected 
        If Not bUpdating Then
            clsImportNetcdfFunction.RemoveParameterByName("id_points")
            If rdoRange.Checked Then
                ucrReceiverPointsX.Visible = False
                ucrReceiverPointsY.Visible = False
                ucrReceiverPointsID.Visible = False
                ucrInputPointX.Visible = False
                ucrInputPointY.Visible = False
                If dctAxesNames.ContainsKey("X") AndAlso lstAxesDetected(0) Then
                    ucrInputMinX.Visible = True
                    ucrInputMaxX.Visible = True
                    clsBoundaryListFunction.AddParameter(dctAxesNames("X"), clsRFunctionParameter:=clsXLimitsFunction)
                Else
                    ucrInputMinX.Visible = False
                    ucrInputMaxX.Visible = False
                End If
                If dctAxesNames.ContainsKey("Y") AndAlso lstAxesDetected(1) Then
                    ucrInputMinY.Visible = True
                    ucrInputMaxY.Visible = True
                    clsBoundaryListFunction.AddParameter(dctAxesNames("Y"), clsRFunctionParameter:=clsYLimitsFunction)
                Else
                    ucrInputMinY.Visible = False
                    ucrInputMaxY.Visible = False
                End If
                clsImportNetcdfFunction.RemoveParameterByName("lon_points")
                clsImportNetcdfFunction.RemoveParameterByName("lat_points")
            ElseIf rdoPoints.Checked OrElse rdoSinglePoint.Checked Then
                ucrInputMinX.Visible = False
                ucrInputMaxX.Visible = False
                ucrInputMinY.Visible = False
                ucrInputMaxY.Visible = False
                If dctAxesNames.ContainsKey("X") Then
                    clsBoundaryListFunction.RemoveParameterByName(dctAxesNames("X"))
                End If
                If dctAxesNames.ContainsKey("Y") Then
                    clsBoundaryListFunction.RemoveParameterByName(dctAxesNames("Y"))
                End If
                If rdoPoints.Checked Then
                    ucrReceiverPointsX.Visible = lstAxesDetected(0)
                    ucrReceiverPointsY.Visible = lstAxesDetected(1)
                    ucrReceiverPointsID.Visible = lstAxesDetected(0) OrElse lstAxesDetected(1)
                    ucrInputPointX.Visible = False
                    ucrInputPointY.Visible = False
                    If lstAxesDetected(0) Then
                        clsImportNetcdfFunction.AddParameter("lon_points", clsRFunctionParameter:=ucrReceiverPointsX.GetVariables())
                    End If
                    If lstAxesDetected(1) Then
                        clsImportNetcdfFunction.AddParameter("lat_points", clsRFunctionParameter:=ucrReceiverPointsY.GetVariables())
                    End If
                    If (lstAxesDetected(0) OrElse lstAxesDetected(1)) AndAlso Not ucrReceiverPointsID.IsEmpty Then
                        clsImportNetcdfFunction.AddParameter("id_points", clsRFunctionParameter:=ucrReceiverPointsID.GetVariables())
                    End If
                Else
                    ucrReceiverPointsX.Visible = False
                    ucrReceiverPointsY.Visible = False
                    ucrReceiverPointsID.Visible = False
                    ucrInputPointX.Visible = lstAxesDetected(0)
                    ucrInputPointY.Visible = lstAxesDetected(1)
                    If lstAxesDetected(0) Then
                        clsImportNetcdfFunction.AddParameter("lon_points", ucrInputPointX.GetText())
                    End If
                    If lstAxesDetected(1) Then
                        clsImportNetcdfFunction.AddParameter("lat_points", ucrInputPointY.GetText())
                    End If
                End If
            End If
            ucrSelectorPoints.Visible = (rdoPoints.Checked)
        End If
    End Sub

    Private Sub sdgOpenNetCDF_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        Dim strMessage As String = ""
        Dim bWarning As Boolean = False
        Dim result As MsgBoxResult

        For i As Integer = 0 To lstMinTextBoxes.Count - 1
            If lstAxesDetected(i) AndAlso lstMinTextBoxes(i).Visible AndAlso lstMinTextBoxes(i).IsEmpty() Then
                strMessage = strMessage & Environment.NewLine & "Min " & lstAxesLabels(i).Text & " is missing."
                bWarning = True
            End If
        Next
        For i As Integer = 0 To lstMaxTextBoxes.Count - 1
            If lstAxesDetected(i) AndAlso lstMaxTextBoxes(i).Visible AndAlso lstMaxTextBoxes(i).IsEmpty() Then
                strMessage = strMessage & Environment.NewLine & "Max " & lstAxesLabels(i).Text & " is missing."
                bWarning = True
            End If
        Next
        If rdoPoints.Checked Then
            If lstAxesDetected(0) AndAlso ucrReceiverPointsX.IsEmpty() Then
                strMessage = strMessage & Environment.NewLine & "X (lon) points is missing."
                bWarning = True
            End If
            If lstAxesDetected(1) AndAlso ucrReceiverPointsY.IsEmpty() Then
                strMessage = strMessage & Environment.NewLine & "Y (lat) points is missing."
                bWarning = True
            End If
        ElseIf rdoSinglePoint.Checked Then
            If lstAxesDetected(0) AndAlso ucrInputPointX.IsEmpty() Then
                strMessage = strMessage & Environment.NewLine & "X (lon) point is missing."
                bWarning = True
            End If
            If lstAxesDetected(1) AndAlso ucrInputPointY.IsEmpty() Then
                strMessage = strMessage & Environment.NewLine & "Y (lat) point is missing."
                bWarning = True
            End If
        End If
        If bWarning Then
            result = MessageBox.Show(text:="Information missing. See details below. OK will not be enabled on the main dialog untill resolved." & Environment.NewLine & "Are you sure you want to return to the main dialog?" & Environment.NewLine & strMessage, caption:="Missing information", buttons:=MessageBoxButtons.YesNo, icon:=MessageBoxIcon.Information)
            If result = MsgBoxResult.No Then
                e.Cancel = True
            End If
        End If
        bOKEnabled = Not bWarning
    End Sub

    Private Sub SetHelpOptions()
        Select Case dlgOpenNetCDF.enumNetCDFMode
            Case dlgOpenNetCDF.NetCDFMode.File
                ucrBase.iHelpTopicID = 119
            Case dlgOpenNetCDF.NetCDFMode.Climatic
                ucrBase.iHelpTopicID = 117
        End Select
    End Sub
End Class