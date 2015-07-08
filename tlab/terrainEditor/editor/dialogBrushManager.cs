//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$TEP_BrushManager_SetHeight_Range_Min = 0;
$TEP_BrushManager_SetHeight_Range_Max = 200;
$TEP_BrushManager_BrushSize_Range_Min = 0;
$TEP_BrushManager_BrushSize_Range_Max = 200;
$TEP_BrushManager_Softness_Range_Min = 0;
$TEP_BrushManager_Softness_Range_Max = 200;
$TEP_BrushManager_Pressure_Range_Min = 0;
$TEP_BrushManager_Pressure_Range_Max = 200;



$TEP_BrushManager_SetHeight = 100;

$TEP_BrushManager_OptionFields = "heightRangeMin heightRangeMax";
function TEP_BrushManager::onWake( %this ) {
}
//------------------------------------------------------------------------------
//==============================================================================
// Brush Size update and validation
//==============================================================================
//==============================================================================
// Set the size of the brush (in game unit)
function TEP_BrushManager::updateBrushSize( %this,%ctrl ) {
	%validValue = %this.validateBrushSize(%ctrl.getValue());
	%maxBrushSize = getWord(ETerrainEditor.maxBrushSize, 0);

	//Check the slider range and fix in case settings have changed
	if(%ctrl.isMemberOfClass("GuiSliderCtrl")) {
		%latestRange = "1" SPC %maxBrushSize;

		if (%ctrl.range !$= %latestRange)
			%ctrl.range = %latestRange;
	}

	TerrainEditorPlugin.setParam("BrushSize",%validValue);
}
//------------------------------------------------------------------------------
//==============================================================================
// Set the size of the brush (in game unit)
function TEP_BrushManager::validateBrushSize( %this,%value ) {
	%minBrushSize = 1;
	%maxBrushSize = getWord(ETerrainEditor.maxBrushSize, 0);
	//Convert float to closest integer
	%brushSize = mCeil(%value);
	%brushSize = mClamp(%brushSize,%minBrushSize,%maxBrushSize);
	ETerrainEditor.setBrushSize(%brushSize);

	//Set the validated value to control and update friends if there's any
	foreach(%slider in $GuiGroup_TEP_BrushSizeSlider) {
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
function TEP_BrushManager::updateBrushPressure( %this,%ctrl ) {
	//Convert float to closest integer
	%brushPressure = %ctrl.getValue();
	%validValue = %this.validateBrushPressure(%brushPressure);
	TerrainEditorPlugin.setParam("BrushPressure",%validValue);
}
//------------------------------------------------------------------------------
//==============================================================================
// Set the pressure of the brush
function TEP_BrushManager::validateBrushPressure( %this,%brushPressure ) {
	//Convert float to closest integer
	%convPressure = %brushPressure/100;
	%clampPressure = mClamp(%convPressure,"0.0","1.0");
	ETerrainEditor.setBrushPressure(%clampPressure);
	%editorPressure = ETerrainEditor.getBrushPressure();
	%newPressure = %editorPressure * 100;
	%formatPressure = mFloatLength(%newPressure,1);

	//Set the validated value to control and update friends if there's any
	foreach(%slider in $GuiGroup_TEP_PressureSlider) {
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
function TEP_BrushManager::updateBrushSoftness( %this,%ctrl ) {
	//Convert float to closest integer
	%brushSoftness = %ctrl.getValue();
	%validValue = %this.validateBrushSoftness(%brushSoftness);
	TerrainEditorPlugin.setParam("BrushSoftness",%validValue);
}
//------------------------------------------------------------------------------
//==============================================================================
// Set the softness of the brush - (Lower = Less effects)
function TEP_BrushManager::validateBrushSoftness( %this,%value ) {
	//Convert float to closest integer
	%brushSoftness = %value;
	%convSoftness = %brushSoftness/100;
	%clampSoftness = mClamp(%convSoftness,"0","1");
	ETerrainEditor.setBrushSoftness(%clampSoftness);
	%editorSoftness = ETerrainEditor.getBrushSoftness();
	%newSoftness = %editorSoftness * 100;
	%formatSoftness = mFloatLength(%newSoftness,1);

	//Set the validated value to control and update friends if there's any
	foreach(%slider in $GuiGroup_TEP_SoftnessSlider) {
		%slider.setValue(%formatSoftness);
		%slider.updateFriends();
	}

	return %formatSoftness;
}
//------------------------------------------------------------------------------

//==============================================================================
// Brush Softness update and validation
//==============================================================================
//==============================================================================
// Set the softness of the brush - (Lower = Less effects)
function TEP_BrushManager::updateSetHeightValue( %this,%ctrl ) {
	//Convert float to closest integer
	%validValue = %this.validateBrushSetHeight(%ctrl.getValue());

	if (%validValue $= "")
		return;

	TerrainEditorPlugin.setParam("BrushSetHeight",%validValue);
}
//------------------------------------------------------------------------------
//==============================================================================
// Set the softness of the brush - (Lower = Less effects)
function TEP_BrushManager::validateBrushSetHeight( %this,%value ) {
	//Convert float to closest integer
	if (!strIsNumeric(%value)) {
		warnLog("Invalid non-numeric value specified:",%value);
		return;
	}

	%value = mFloatLength(%value,2);
	ETerrainEditor.setHeightVal = %value;

	foreach(%slider in $GuiGroup_TEP_SetHeightSlider) {
		%slider.setValue(%value);
		%slider.updateFriends();
	}

	return %value;
}
//------------------------------------------------------------------------------
//==============================================================================
// Brush Options Setting Functions
//==============================================================================

//==============================================================================
function TEP_BrushManagerOptionEdit::onValidate( %this ) {
	%infoWords = strreplace(%this.internalName,"_"," ");
	%group = getWord(%infoWords,0);
	%type = getWord(%infoWords,1);
	%field = getWord(%infoWords,2);
	%value = %this.getText();
	TEP_BrushManager.doBrushOption(%group,%type,%field,%value);
}
//------------------------------------------------------------------------------

//==============================================================================
function TEP_BrushManager::doBrushOption( %this,%group,%type,%field,%value ) {
	%fullField = %group@"_"@%type@"_"@%field;
	TEP_BrushManager.setFieldValue(%fullField,%value);
	$TEP_BrushManager_[%fullField] = %value;

	if (%group $= "BrushSize" && %type $= "Range" && %field $= "Max")
		ETerrainEditor.maxBrushSize = %value;

	switch$(%type) {
	case "range":
		%guiGroup = "TEP_"@%group@"Slider";
		%rangeFull = $TEP_BrushManager_[%group,"Range","Min"] SPC $TEP_BrushManager_[%group,"Range","Max"];
		eval("%group = $GuiGroup_TEP_"@%group@"Slider;");

		foreach(%ctrl in %group) {
			%ctrl.range = %rangeFull;
			%ctrl.setValue(%value);
		}
	}
}
//------------------------------------------------------------------------------

//==============================================================================
function TEP_BrushManager::brushSetHeightRange( %this,%range ) {
	$TEP_BrushManager_["SetHeight","Range","Min"] = %range.x;
	$TEP_BrushManager_["SetHeight","Range","Max"] = %range.y;

	foreach(%ctrl in $GuiGroup_TEP_SetHeightSlider) {
		%ctrl.range = %range;
		%value = %ctrl.getValue();
		%ctrl.setValue(%value);
	}
}
//------------------------------------------------------------------------------