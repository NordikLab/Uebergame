//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

function EMissionArea::activateEditor( %this ) {
	SceneEditorDialogs.showDlg("MissionArea");
	
	%ma = getMissionAreaServerObject();
	EMissionArea.theArea = %ma;

	if (!isObject(%ma)) {
		warnLog("Can't find a valid MissionArea object, please make sure there's one in the mission and retry");
		return;
	}

	MissionAreaTerrain.setMissionArea(%ma);
	MissionAreaTerrain.updateTerrain();

	if ( MissionAreaTerrain.bitmap $= "") {
		LabMsgOk("Mission have no terrain","There's no terrain found in the mission and you need one to use the visual area editting tool. You can edit the area bounds manually in the inspector");
		%this.setVisible(false);
	}
}
function EMissionArea::deactivateEditor( %this ) {
	devLog("Unselected=",%this);
	%this.setVisible(false);
}
function MissionArea::onEditorUnselect( %this ) {
	EMissionArea.setVisible(false);
}
function MissionArea::onEditorSelect( %this,%selectSize ) {
	EMissionArea.activateEditor();
}

function MissionAreaTerrain::onMissionAreaModified( %this ) {
	SceneInspector.refresh();
}

function MissionAreaTerrain::onUndo( %this ) {
	SceneInspector.refresh();
}

function EMissionArea::fitToTerrain( %this ) {
	%this.theArea.area = "-256 -256 512 512";
	SceneInspector.refresh();
}
