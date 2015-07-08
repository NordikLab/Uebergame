//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

function PaintBrushSizeSliderCtrlContainer::onWake(%this) {
	%this-->slider.range = "1" SPC getWord(ETerrainEditor.maxBrushSize, 0);
	%this-->slider.setValue(PaintBrushSizeTextEditContainer-->textEdit.getValue());
}

function PaintBrushPressureSliderCtrlContainer::onWake(%this) {
	%this-->slider.setValue(PaintBrushPressureTextEditContainer-->textEdit.getValue() / 100);
}

function PaintBrushSoftnessSliderCtrlContainer::onWake(%this) {
	%this-->slider.setValue(PaintBrushSoftnessTextEditContainer-->textEdit.getValue() / 100);
}

//------------------------------------------------------------------------------------

function TerrainBrushSizeSliderCtrlContainer::onWake(%this) {
	%this-->slider.range = "1" SPC getWord(ETerrainEditor.maxBrushSize, 0);
	%this-->slider.setValue(TerrainBrushSizeTextEditContainer-->textEdit.getValue());
}

function TerrainBrushPressureSliderCtrlContainer::onWake(%this) {
	%this-->slider.setValue(TerrainBrushPressureTextEditContainer-->textEdit.getValue() / 100.0);
}

function TerrainBrushSoftnessSliderCtrlContainer::onWake(%this) {
	%this-->slider.setValue(TerrainBrushSoftnessTextEditContainer-->textEdit.getValue() / 100.0);
}

function TerrainSetHeightSliderCtrlContainer::onWake(%this) {
	%this-->slider.setValue(TerrainSetHeightTextEditContainer-->textEdit.getValue());
}