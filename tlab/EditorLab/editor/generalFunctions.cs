//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================

$RelightCallback = "";


//------------------------------------------------------------------------------
//==============================================================================

$RelightCallback = "";

function EditorLightingComplete() {
	$lightingMission = false;
	RelightStatus.visible = false;

	if ($RelightCallback !$= "") {
		eval($RelightCallback);
	}

	$RelightCallback = "";
}
//------------------------------------------------------------------------------
//==============================================================================
function updateEditorLightingProgress() {
	RelightProgress.setValue(($SceneLighting::lightingProgress));

	if ($lightingMission)
		$lightingProgressThread = schedule(1, 0, "updateEditorLightingProgress");
}
//------------------------------------------------------------------------------
//==============================================================================
function Editor::lightScene(%this, %callback, %forceAlways) {
	if ($lightingMission)
		return;

	$lightingMission = true;
	$RelightCallback = %callback;
	RelightStatus.visible = true;
	RelightProgress.setValue(0);
	Canvas.repaint();
	lightScene("EditorLightingComplete", %forceAlways);
	updateEditorLightingProgress();
}
//------------------------------------------------------------------------------
