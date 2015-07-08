//==============================================================================
// LabEditor ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// Set and validate paint brush settings
//==============================================================================
//==============================================================================

//==============================================================================
// Set the pressure of the brush
function ForestEditorPlugin::setBrushPressure( %this,%ctrl ) {
	//Convert float to closest integer
	%brushPressure = %ctrl.getValue();
	%convPressure = %brushPressure/100;
	%clampPressure = mClamp(%convPressure,"0.0","1.0");
	ForestToolBrush.pressure = %clampPressure;
	%newPressure = %clampPressure * 100;
	%formatPressure = mFloatLength(%newPressure,1);
	
	//Set the validated value to control and update friends if there's any
	%ctrl.setValue(%formatPressure);
	%ctrl.updateFriends();
}
//------------------------------------------------------------------------------
//==============================================================================
// Set the pressure of the brush
function ForestEditorPlugin::setBrushHardness( %this,%ctrl ) {
	//Convert float to closest integer
	%brushHardness = %ctrl.getValue();
	%convHardness = %brushHardness/100;
	%clampHardness = mClamp(%convHardness,"0.0","1.0");
	ForestToolBrush.Hardness = %clampHardness;
	%newHardness = %clampHardness * 100;
	%formatHardness = mFloatLength(%newHardness,1);
	
	//Set the validated value to control and update friends if there's any
	%ctrl.setValue(%formatHardness);
	%ctrl.updateFriends();
}
//------------------------------------------------------------------------------
//==============================================================================
function ForestEditorPlugin::setGlobalScale( %this,%ctrl ) {
	%minScaleSize = "0.1";
	%maxScaleSize = "10";

	if (%ctrl $= "")
		%ctrl = $ThisControl;

	%val = %ctrl.getValue();
	%val = mClamp(%val,%minScaleSize,%maxScaleSize);
	%ctrl.setValue(%val);
	$Forest::GlobalScale = %val;
	%ctrl.updateFriends();
	%syncWith = %ctrl.getParent().internalName;

	if (!isObject(%syncWith))
		return;

	%syncWith-->globalScaleSlider.setValue(%val);
	%syncWith-->globalScaleEdit.setValue(%val);
}
//------------------------------------------------------------------------------
//==============================================================================
function ForestEditorGui::validateBrushSize( %this ) {
	%minBrushSize = 1;
	%maxBrushSize = getWord(ETerrainEditor.maxBrushSize, 0);
	%val = $ThisControl.getText();

	if(%val < %minBrushSize)
		$ThisControl.setValue(%minBrushSize);
	else if(%val > %maxBrushSize)
		$ThisControl.setValue(%maxBrushSize);
}
//------------------------------------------------------------------------------
