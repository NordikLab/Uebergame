//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$TPP_BrushManager_Slope_Range_Min = 0;
$TPP_BrushManager_Slope_Range_Max = 90;
$TPP_BrushManager_BrushSize_Range_Min = 0;
$TPP_BrushManager_BrushSize_Range_Max = 200;
$TPP_BrushManager_Softness_Range_Min = 0;
$TPP_BrushManager_Softness_Range_Max = 200;
$TPP_BrushManager_Pressure_Range_Min = 0;
$TPP_BrushManager_Pressure_Range_Max = 200;




$TPP_BrushManager_OptionFields = "heightRangeMin heightRangeMax";
function TPP_BrushManager::onWake( %this ) {
}
//------------------------------------------------------------------------------

//==============================================================================
// Brush Updates Setting Functions
//==============================================================================

//==============================================================================
// Brush Size update and validation
//==============================================================================
//==============================================================================
// Set the size of the brush (in game unit)
function TPP_BrushManager::updateBrushSize( %this,%ctrl ) {
	%validValue = %this.validateBrushSize(%ctrl.getValue());
	%maxBrushSize = getWord(ETerrainEditor.maxBrushSize, 0);

	//Check the slider range and fix in case settings have changed
	if(%ctrl.isMemberOfClass("GuiSliderCtrl")) {
		%latestRange = "1" SPC %maxBrushSize;

		if (%ctrl.range !$= %latestRange)
			%ctrl.range = %latestRange;
	}

	TerrainPainterPlugin.setParam("BrushSize",%validValue);
}
//------------------------------------------------------------------------------
//==============================================================================
// Set the size of the brush (in game unit)
function TPP_BrushManager::validateBrushSize( %this,%value ) {
	%minBrushSize = 1;
	%maxBrushSize = getWord(ETerrainEditor.maxBrushSize, 0);
	//Convert float to closest integer
	%brushSize = mCeil(%value);
	%brushSize = mClamp(%brushSize,%minBrushSize,%maxBrushSize);
	ETerrainEditor.setBrushSize(%brushSize);

	//Set the validated value to control and update friends if there's any
	foreach(%slider in $GuiGroup_TPP_Slider_BrushSize) {
		%slider.setValue(%brushSize);
		%slider.updateFriends();
	}

	return %brushSize;
}
//------------------------------------------------------------------------------
//==============================================================================
// Brush Pressure update and validation
//==============================================================================
//==============================================================================
// Set the pressure of the brush
function TPP_BrushManager::updateBrushPressure( %this,%ctrl ) {
	//Convert float to closest integer
	%brushPressure = %ctrl.getValue();
	%validValue = %this.validateBrushPressure(%brushPressure);
	TerrainPainterPlugin.setParam("BrushPressure",%validValue);
}
//------------------------------------------------------------------------------
//==============================================================================
// Set the pressure of the brush
function TPP_BrushManager::validateBrushPressure( %this,%brushPressure ) {
	//Convert float to closest integer
	%convPressure = %brushPressure/100;
	%clampPressure = mClamp(%convPressure,"0.0","1.0");
	ETerrainEditor.setBrushPressure(%clampPressure);
	%editorPressure = ETerrainEditor.getBrushPressure();
	%newPressure = %editorPressure * 100;
	%formatPressure = mFloatLength(%newPressure,1);

	//Set the validated value to control and update friends if there's any
	foreach(%slider in $GuiGroup_TPP_Slider_Pressure) {
		%slider.setValue(%formatPressure);
		%slider.updateFriends();
	}

	return %formatPressure;
}
//------------------------------------------------------------------------------
//==============================================================================
// Brush Softness update and validation
//==============================================================================

//==============================================================================
// Set the softness of the brush - (Lower = Less effects)
function TPP_BrushManager::updateBrushSoftness( %this,%ctrl ) {
	//Convert float to closest integer
	%brushSoftness = %ctrl.getValue();
	%validValue = %this.validateBrushSoftness(%brushSoftness);
	TerrainPainterPlugin.setParam("BrushSoftness",%validValue);
}
//------------------------------------------------------------------------------
//==============================================================================
// Set the softness of the brush - (Lower = Less effects)
function TPP_BrushManager::validateBrushSoftness( %this,%value ) {
	//Convert float to closest integer
	%brushSoftness = %value;
	%convSoftness = %brushSoftness/100;
	%clampSoftness = mClamp(%convSoftness,"0","1");
	ETerrainEditor.setBrushSoftness(%clampSoftness);
	%editorSoftness = ETerrainEditor.getBrushSoftness();
	%newSoftness = %editorSoftness * 100;
	%formatSoftness = mFloatLength(%newSoftness,1);

	//Set the validated value to control and update friends if there's any
	foreach(%slider in $GuiGroup_TPP_Slider_Softness) {
		%slider.setValue(%formatSoftness);
		%slider.updateFriends();
	}

	return %formatSoftness;
}
//------------------------------------------------------------------------------
//==============================================================================
// Brush SlopeMin update and validation
//==============================================================================

//==============================================================================
// Set Slope Angle Min. - Brush have no effect on terrain with lower angle
function TPP_BrushManager::setSlopeMin( %this,%ctrl ) {
	%validValue = %this.validateBrushSlopeMin(%ctrl.getValue());
	TerrainPainterPlugin.setParam("BrushSlopeMin",%validValue);
}
//------------------------------------------------------------------------------
//==============================================================================
// Set Slope Angle Min. - Brush have no effect on terrain with lower angle
function TPP_BrushManager::validateBrushSlopeMin( %this,%value ) {
	//Force the value into the TerrainEditor code and it will be returned validated
	%val = ETerrainEditor.setSlopeLimitMinAngle(%value);
	//Set precision to 1 for gui display
	%formatVal = mFloatLength(%val,1);

	//Set the validated value to control and update friends if there's any
	foreach(%slider in $GuiGroup_TPP_Slider_SlopeMin) {
		%slider.setValue(%formatVal);
		%slider.updateFriends();
	}

	return %val;
}
//------------------------------------------------------------------------------
//==============================================================================
// Brush SlopeMax update and validation
//==============================================================================

//==============================================================================
// Set Slope Angle Min. - Brush have no effect on terrain with lower angle
function TPP_BrushManager::setSlopeMax( %this,%ctrl ) {
	%validValue = %this.validateBrushSlopeMax(%ctrl.getValue());
	TerrainPainterPlugin.setParam("BrushSlopeMax",%validValue);
}
//------------------------------------------------------------------------------
//==============================================================================
// Set Slope Angle Min. - Brush have no effect on terrain with lower angle
function TPP_BrushManager::validateBrushSlopeMax( %this,%value ) {
	//Force the value into the TerrainEditor code and it will be returned validated
	%val = ETerrainEditor.setSlopeLimitMaxAngle(%value);
	//Set precision to 1 for gui display
	%formatVal = mFloatLength(%val,1);

	//Set the validated value to control and update friends if there's any
	foreach(%slider in $GuiGroup_TPP_Slider_SlopeMax) {
		%slider.setValue(%formatVal);
		%slider.updateFriends();
	}

	return %val;
}
//------------------------------------------------------------------------------

//==============================================================================
// Brush Options Setting Functions
//==============================================================================
//==============================================================================
function TPP_BrushManagerSlopeRange::onValidate( %this ) {
	%infoWords = strreplace(%this.internalName,"_"," ");
	%group = getWord(%infoWords,0);
	%type = getWord(%infoWords,1);
	%field = getWord(%infoWords,2);
	%value = %this.getText();
	%rangeFull = $TPP_BrushManager_[%group,"Range","Min"] SPC $TPP_BrushManager_[%group,"Range","Max"];
	%min = ETerrainEditor.getSlopeLimitMinAngle();

	foreach(%ctrl in $GuiGroup_TPP_Slider_SlopeMin) {
		%ctrl.range = %rangeFull;
		%ctrl.setValue(%min);
	}

	%max = ETerrainEditor.getSlopeLimitMaxAngle();

	foreach(%ctrl in $GuiGroup_TPP_Slider_SlopeMax) {
		%ctrl.range = %rangeFull;
		%ctrl.setValue(%max);
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function TPP_BrushManagerOptionEdit::onValidate( %this ) {
	%infoWords = strreplace(%this.internalName,"_"," ");
	%group = getWord(%infoWords,0);
	%type = getWord(%infoWords,1);
	%field = getWord(%infoWords,2);
	%value = %this.getText();
	TPP_BrushManager.doBrushOption(%group,%type,%field,%value);
}
//------------------------------------------------------------------------------

//==============================================================================
function TPP_BrushManager::doBrushOption( %this,%group,%type,%field,%value ) {
	%fullField = %group@"_"@%type@"_"@%field;
	TPP_BrushManager.setFieldValue(%fullField,%value);
	$TPP_BrushManager_[%fullField] = %value;

	if (%group $= "BrushSize" && %type $= "Range" && %field $= "Max")
		ETerrainEditor.maxBrushSize = %value;

	switch$(%type) {
	case "range":
		%guiGroup = "TPP_"@%group@"Slider";
		%rangeFull = $TPP_BrushManager_[%group,"Range","Min"] SPC $TPP_BrushManager_[%group,"Range","Max"];

		foreach(%ctrl in $GuiGroup_TPP_Slider_[%group]) {
			%ctrl.range = %rangeFull;
			%ctrl.setValue(%value);
		}
	}
}
//------------------------------------------------------------------------------