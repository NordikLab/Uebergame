//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
function LabMissionSettingsDlg::onWake( %this ) {
	hide(%this-->customFieldsDlg);
	%this-->LevelName.setValue(theLevelInfo.levelName);
	%this-->LevelDescription.setText(theLevelInfo.levelDescription);
}
//------------------------------------------------------------------------------

//==============================================================================
function LabMissionSettingsDlg::saveAndClose( %this ) {
	theLevelInfo.levelName = %this-->LevelName.getValue();
	theLevelInfo.levelDescription = %this-->LevelDescription.getText();
	%inspect = SceneInspector.getInspectObject();

	if(%inspect.getClassName() $= "LevelInfo")
		SceneInspector.inspect(%inspect);

	popDlg(%this);
}
//------------------------------------------------------------------------------

//==============================================================================
function LabMissionSettingsDlg::cancelChanges( %this ) {
}
//------------------------------------------------------------------------------