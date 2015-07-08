//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================


//==============================================================================
function WorldEditor::init(%this) {
	// add objclasses which we do not want to collide with
	%this.ignoreObjClass(Sky);
	// editing modes
	WEditorPlugin.numEditModes = 3;
	WEditorPlugin.editMode[0]    = "move";
	WEditorPlugin.editMode[1]    = "rotate";
	WEditorPlugin.editMode[2]    = "scale";
	// context menu
	new GuiControl(WEContextPopupDlg, EditorGuiGroup) {
		profile = "ToolsGuiModelessDialogProfile";
		horizSizing = "width";
		vertSizing = "height";
		position = "0 0";
		extent = "640 480";
		minExtent = "8 8";
		visible = "1";
		setFirstResponder = "0";
		modal = "1";
		new GuiPopUpMenuCtrl(WEContextPopup) {
			profile = "ToolsScrollProfile";
			position = "0 0";
			extent = "0 0";
			minExtent = "0 0";
			maxPopupHeight = "200";
			command = "canvas.popDialog(WEContextPopupDlg);";
		};
	};
	WEContextPopup.setVisible(false);

	// Make sure we have an active selection set.
	if( !%this.getActiveSelection() )
		%this.setActiveSelection( new WorldEditorSelection( EWorldEditorSelection ) );
}

//------------------------------------------------------------------------------

//==============================================================================

function WorldEditor::export(%this) {
	getSaveFilename("~/editor/*.mac|mac file", %this @ ".doExport", "selection.mac");
}
//------------------------------------------------------------------------------
//==============================================================================
function WorldEditor::doExport(%this, %file) {
	missionGroup.save("~/editor/" @ %file, true);
}
//------------------------------------------------------------------------------
//==============================================================================
function WorldEditor::import(%this) {
	getLoadFilename("~/editor/*.mac|mac file", %this @ ".doImport");
}
//------------------------------------------------------------------------------
//==============================================================================
function WorldEditor::doImport(%this, %file) {
	exec("~/editor/" @ %file);
}
//------------------------------------------------------------------------------
//==============================================================================
function WorldEditor::onGuiUpdate(%this, %text) {
}
//------------------------------------------------------------------------------


//==============================================================================

function WorldEditorToolbarDlg::init(%this) {
	WorldEditorInspectorCheckBox.setValue( WorldEditorToolFrameSet.isMember( "EditorToolInspectorGui" ) );
	WorldEditorMissionAreaCheckBox.setValue( WorldEditorToolFrameSet.isMember( "EditorToolMissionAreaGui" ) );
	WorldEditorTreeCheckBox.setValue( WorldEditorToolFrameSet.isMember( "EditorToolTreeViewGui" ) );
	WorldEditorCreatorCheckBox.setValue( WorldEditorToolFrameSet.isMember( "EditorToolCreatorGui" ) );
}
//------------------------------------------------------------------------------
//==============================================================================
function WorldEditor::onAddSelected(%this,%obj) {
	SceneEditorTree.addSelection(%obj);
}
//------------------------------------------------------------------------------
//==============================================================================
function WorldEditor::onWorldEditorUndo( %this ) {
	SceneInspector.refresh();
}
//------------------------------------------------------------------------------
//==============================================================================