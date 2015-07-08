//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// Scene Editor Params - Used set default settings and build plugins options GUI
//==============================================================================
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SceneEditorPlugin::initParamsArray( %this,%array ) {
	$SceneEdCfg = newScriptObject("SceneEditorCfg");
	%array.group[%groupId++] = "General settings";

	%array.setVal("DropLocation",       "10" TAB "Drop object location" TAB "Dropdown"  TAB "itemList>>$TLab_Object_DropTypes" TAB "SceneEditorCfg" TAB %groupId);
	%array.setVal("AutoCreatePrefab",       "1" TAB "Create prefab automatically" TAB "Checkbox"  TAB "" TAB "SceneEditorCfg" TAB %groupId);
	%array.group[%groupId++] = "MissionGroup Organizer";
	%array.setVal("CoreGroup",       "mgCore" TAB "Core Objects Group" TAB "TextEdit"  TAB "" TAB "SceneEditorCfg" TAB %groupId);
	%array.setVal("AmbientGroup",       "mgAmbient" TAB "Ambient Group" TAB "TextEdit"  TAB "" TAB "SceneEditorCfg" TAB %groupId);
	%array.setVal("EnvironmentGroup",       "mgEnvironment" TAB "Environment Group" TAB "TextEdit"  TAB "" TAB "SceneEditorCfg" TAB %groupId);
	%array.setVal("TSStaticGroup",       "mgMapModels" TAB "TSStatic Group" TAB "TextEdit"  TAB "" TAB "SceneEditorCfg" TAB %groupId);
	%array.setVal("SpawnGroup",       "PlayerDropPoints" TAB "Spawn Group" TAB "TextEdit"  TAB "" TAB "SceneEditorCfg" TAB %groupId);
	%array.setVal("MiscObjectGroup",       "mgMiscObject" TAB "Misc. Objects Group" TAB "TextEdit"  TAB "" TAB "SceneEditorCfg" TAB %groupId);
}
//------------------------------------------------------------------------------

//==============================================================================
// Plugin Object Callbacks - Called from TLab plugin management scripts
//==============================================================================

//==============================================================================
// Called when TorqueLab is launched for first time
function SceneEditorPlugin::onWorldEditorStartup( %this ) {
	Parent::onWorldEditorStartup( %this );
	%map = new ActionMap();
	%map.bindCmd( keyboard, "1", "EWorldEditorNoneModeBtn.performClick();", "" );  // Select
	%map.bindCmd( keyboard, "2", "EWorldEditorMoveModeBtn.performClick();", "" );  // Move
	%map.bindCmd( keyboard, "3", "EWorldEditorRotateModeBtn.performClick();", "" );  // Rotate
	%map.bindCmd( keyboard, "4", "EWorldEditorScaleModeBtn.performClick();", "" );  // Scale
	%map.bindCmd( keyboard, "f", "FitToSelectionBtn.performClick();", "" );// Fit Camera to Selection
	%map.bindCmd( keyboard, "z", "EditorGuiStatusBar.setCamera(\"Standard Camera\");", "" );// Free camera
	%map.bindCmd( keyboard, "n", "ToggleNodeBar->renderHandleBtn.performClick();", "" );// Render Node
	%map.bindCmd( keyboard, "shift n", "ToggleNodeBar->renderTextBtn.performClick();", "" );// Render Node Text
	%map.bindCmd( keyboard, "g", "ESnapOptions-->GridSnapButton.performClick();" ); // Grid Snappping
	%map.bindCmd( keyboard, "t", "SnapToBar->objectSnapDownBtn.performClick();", "" );// Terrain Snapping
	%map.bindCmd( keyboard, "b", "SnapToBar-->objectSnapBtn.performClick();" ); // Soft Snappping
	%map.bindCmd( keyboard, "v", "SceneEditorToolbar->boundingBoxColBtn.performClick();", "" );// Bounds Selection
	%map.bindCmd( keyboard, "o", "EToolbarObjectCenterDropdown->objectBoxBtn.performClick(); objectCenterDropdown.toggle();", "" );// Object Center
	%map.bindCmd( keyboard, "p", "EToolbarObjectCenterDropdown->objectBoundsBtn.performClick(); objectCenterDropdown.toggle();", "" );// Bounds Center
	%map.bindCmd( keyboard, "k", "EToolbarObjectTransformDropdown->objectTransformBtn.performClick(); EToolbarObjectTransformDropdown.toggle();", "" );// Object Transform
	%map.bindCmd( keyboard, "l", "EToolbarObjectTransformDropdown->worldTransformBtn.performClick(); EToolbarObjectTransformDropdown.toggle();", "" );// World Transform
	SceneEditorPlugin.map = %map;

	if (SceneEditorPlugin.getCfg("DropType") !$= "")
		EWorldEditor.dropType = %this.getCfg("DropType");

	%this.initAssets();
	SEP_GroundCover.buildLayerSettingGui();
	SEP_CreatorPage.initArrayCfg();
	
	ETransformBox.deactivate();
}
//------------------------------------------------------------------------------
//==============================================================================
// Called when the Plugin is activated (Active TorqueLab plugin)
function SceneEditorPlugin::onActivated( %this ) {
	Parent::onActivated( %this );
	SceneEditorToolbar-->groundCoverToolbar.visible = 0;
	%this.initToolBar();
	SceneEditorTreeFilter.extent.x = SceneEditorTreeTabBook.extent.x -  56;
	SceneEditorTreeTabBook.selectPage(0);
	SceneEditorModeTab.selectPage(0);
	
	SEP_GroupPage.updateContent();

	if (SceneEditorPlugin.getCfg("DropType") !$= "")
		EWorldEditor.dropType = %this.getCfg("DropType");
}
//------------------------------------------------------------------------------
//==============================================================================
// Called when the Plugin is deactivated (active to inactive transition)
function SceneEditorPlugin::onDeactivated( %this ) {
	Parent::onDeactivated( %this );

}
//------------------------------------------------------------------------------
//==============================================================================
// Called from TorqueLab after plugin is initialize to set needed settings
function SceneEditorPlugin::onPluginCreated( %this ) {
	EWorldEditor.dropType = SceneEditorPlugin.getCfg("DropType");
}
//------------------------------------------------------------------------------

//==============================================================================
// Called when the mission file has been saved
function SceneEditorPlugin::onSaveMission( %this, %file ) {
	SEP_GroundCover.setNotDirty();
}
//------------------------------------------------------------------------------
//==============================================================================
// Called when TorqueLab is closed
function SceneEditorPlugin::onEditorSleep( %this ) {
}
//------------------------------------------------------------------------------
//==============================================================================
//Called when editor is selected from menu
function SceneEditorPlugin::onEditMenuSelect( %this, %editMenu ) {
	%canCutCopy = EWorldEditor.getSelectionSize() > 0;
	%editMenu.enableItem( 3, %canCutCopy ); // Cut
	%editMenu.enableItem( 4, %canCutCopy ); // Copy
	%editMenu.enableItem( 5, EWorldEditor.canPasteSelection() ); // Paste
	%selSize = EWorldEditor.getSelectionSize();
	%lockCount = EWorldEditor.getSelectionLockCount();
	%hideCount = EWorldEditor.getSelectionHiddenCount();
	%editMenu.enableItem( 6, %selSize > 0 && %lockCount != %selSize ); // Delete Selection
	%editMenu.enableItem( 8, %canCutCopy ); // Deselect
}
//------------------------------------------------------------------------------

//==============================================================================
// Callbacks Handlers - Called on specific editor actions
//==============================================================================

//==============================================================================
//
function SceneEditorPlugin::handleDelete( %this ) {
	devLog(" SceneEditorPlugin::handleDelete( %this ) ");
	// The tree handles deletion and notifies the
	// world editor to clear its selection.
	//
	// This is because non-SceneObject elements like
	// SimGroups also need to be destroyed.
	//
	// See EditorTree::onObjectDeleteCompleted().
	%selSize = EWorldEditor.getSelectionSize();

	if( %selSize > 0 )
		SceneEditorTree.deleteSelection();
}
//------------------------------------------------------------------------------
//==============================================================================
function SceneEditorPlugin::handleDeselect() {
	EWorldEditor.clearSelection();
}
//------------------------------------------------------------------------------
//==============================================================================
function SceneEditorPlugin::handleCut() {
	EWorldEditor.cutSelection();
}
//------------------------------------------------------------------------------
//==============================================================================
function SceneEditorPlugin::handleCopy() {
	EWorldEditor.copySelection();
}
//------------------------------------------------------------------------------
//==============================================================================
function SceneEditorPlugin::handlePaste() {
	EWorldEditor.pasteSelection();
}
//------------------------------------------------------------------------------
//==============================================================================
$SceneEditorPluginToolModes = "Inspector Builder";
//SceneEditorPlugin.toggleToolMode();
function SceneEditorPlugin::toggleToolMode( %this ) {
	%lastTool = SceneEditorTools.getObject(SceneEditorTools.getCount() -1);
	%currentTool = SceneEditorTools.getObject(1);
	SceneEditorTools.reorderChild(%lastTool,%currentTool);
	SceneEditorTools.updateSizes();
}
//------------------------------------------------------------------------------
//==============================================================================
//SceneEditorPlugin.setToolMode("Builder");
function SceneEditorPlugin::setToolMode( %this,%mode ) {
	%toolCtrl = "SceneEditor"@%mode@"Gui";
	%currentTool = SceneEditorTools.getObject(1);
	SceneEditorTools.reorderChild(%toolCtrl,%currentTool);
	SceneEditorTools.updateSizes();
}
//------------------------------------------------------------------------------
