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
function ShapeEditorPlugin::initParamsArray( %this,%array ) {
	$SceneEdCfg = newScriptObject("ShapeEditorCfg");
	%array.group[%gId++] = "General settings";
	%array.setVal("BackgroundColor",       "0 0 0 100" TAB "BackgroundColor" TAB "TextEdit" TAB "" TAB "ShapeEdPreviewGui-->previewBackground.color" TAB %gId);
	%array.setVal("HighlightMaterial",   "1" TAB "HighlightMaterial" TAB "TextEdit" TAB "" TAB "" TAB %gId);
	%array.setVal("ShowNodes","1" TAB "ShowNodes" TAB "TextEdit" TAB "" TAB "" TAB %gId);
	%array.setVal("ShowBounds",       "0" TAB "ShowBounds" TAB "TextEdit" TAB "" TAB "" TAB %gId);
	%array.setVal("ShowObjBox",       "1" TAB "ShowObjBox" TAB "TextEdit" TAB "" TAB "" TAB %gId);
	%array.setVal("RenderMounts",       "1" TAB "RenderMounts" TAB "TextEdit" TAB "" TAB "" TAB %gId);
	%array.setVal("RenderCollision",       "0" TAB "RenderCollision" TAB "TextEdit" TAB "" TAB "" TAB %gId);
	%array.setVal("AdvancedWindowVisible",       "1" TAB "RenderCollision" TAB "TextEdit" TAB "" TAB "" TAB %gId);
	%array.setVal("AnimationBarVisible",       "1" TAB "RenderCollision" TAB "TextEdit" TAB "" TAB "" TAB %gId);
	
	%array.group[%gId++] = "Grid settings";
	%array.setVal("ShowGrid",       "1" TAB "ShowGrid" TAB "TextEdit" TAB "" TAB "ShapeEdShapeView" TAB %gId);
	%array.setVal("GridSize",       "0.1" TAB "GridSize" TAB "TextEdit" TAB "" TAB "ShapeEdShapeView" TAB %gId);
	%array.setVal("GridDimension",       "40 40" TAB "GridDimension" TAB "TextEdit" TAB "" TAB "ShapeEdShapeView" TAB %gId);
	
	%array.group[%gId++] = "Sun settings";
	%array.setVal("SunDiffuseColor",       "255 255 255 255" TAB "SunDiffuseColor" TAB "TextEdit" TAB "" TAB "ShapeEdShapeView" TAB %gId);
	%array.setVal("SunAmbientColor",       "180 180 180 255" TAB "SunAmbientColor" TAB "TextEdit" TAB "" TAB "ShapeEdShapeView" TAB %gId);
	%array.setVal("SunAngleX",       "45" TAB "SunAngleX" TAB "TextEdit" TAB "" TAB "ShapeEdShapeView" TAB %gId);
	%array.setVal("SunAngleZ",       "135" TAB "SunAngleZ" TAB "TextEdit" TAB "" TAB "ShapeEdShapeView" TAB %gId);
}
//------------------------------------------------------------------------------

//==============================================================================
// Plugin Object Callbacks - Called from TLab plugin management scripts
//==============================================================================

//==============================================================================
// Called when TorqueLab is launched for first time
function ShapeEditorPlugin::onWorldEditorStartup(%this) {
	Parent::onWorldEditorStartup( %this );
	// Add ourselves to the Editor Settings window
	ShapeEdAnimWindow.resize( -1, 526, 593, 53 );
	// Initialise gui
	ShapeEdSeqNodeTabBook.selectPage(0);
	ShapeEdAdvancedWindow-->tabBook.selectPage(0);
	ShapeEdSelectWindow-->tabBook.selectPage(0);
	ShapeEdSelectWindow.navigate("");
	SetToggleButtonValue( ShapeEditorToolbar-->orbitNodeBtn, 0 );
	SetToggleButtonValue( ShapeEditorToolbar-->ghostMode, 0 );
	// Initialise hints menu
	ShapeEdHintMenu.clear();
	%count = ShapeHintGroup.getCount();

	for (%i = 0; %i < %count; %i++) {
		%hint = ShapeHintGroup.getObject(%i);
		ShapeEdHintMenu.add(%hint.objectType, %hint);
	}
}
//------------------------------------------------------------------------------
//==============================================================================
// Called when the Plugin is activated (Active TorqueLab plugin)
function ShapeEditorPlugin::onActivated(%this) {
	if ( !%this.isActivated ) {
		// Activate the Shape Editor
		// Lab.setEditor( %this, true );
		// Get editor settings (note the sun angle is not configured in the settings
		// dialog, so apply the settings here instead of in readSettings)
		ShapeEdPreviewGui.fitIntoParents();
		ShapeEdPreviewGui-->previewBackground.fitIntoParents();
		ShapeEdShapeView.fitIntoParents();
		$wasInWireFrameMode = $gfx::wireframe;
		ShapeEditorToolbar-->wireframeMode.setStateOn($gfx::wireframe);

		if ( GlobalGizmoProfile.getFieldValue(alignment) $= "Object" )
			ShapeEdNodes-->objectTransform.setStateOn(1);
		else
			ShapeEdNodes-->worldTransform.setStateOn(1);

		// Initialise and show the shape editor
		ShapeEdShapeTreeView.open(MissionGroup);
		ShapeEdShapeTreeView.buildVisibleTree(true);
		EditorGui.bringToFront(ShapeEdPreviewGui);
		EWToolsPaletteArray->WorldEditorMove.performClick();
		
		// Switch to the ShapeEditor UndoManager
		%this.oldUndoMgr = Editor.getUndoManager();
		Editor.setUndoManager( ShapeEdUndoManager );
		ShapeEdShapeView.setDisplayType( Lab.cameraDisplayType );
		%this.initStatusBar();
		// Customise menu bar
		%this.oldCamFitCmd = %this.replaceMenuCmd( "Camera", 8, "ShapeEdShapeView.fitToShape();" );
		%this.oldCamFitOrbitCmd = %this.replaceMenuCmd( "Camera", 9, "ShapeEdShapeView.fitToShape();" );
		Parent::onActivated(%this);
	}
	show(ShapeEdSelectWindow);
		show(ShapeEdPropWindow);
	//Assign the Camera fit to the GuiShapeEdPreview
	Lab.fitCameraGui = ShapeEdShapeView;
	// Try to start with the shape selected in the world editor
	ShapeEditor.selectWorldEditorShape();	
	
	ShapeEditorPlugin.updateAnimBar();
}
//------------------------------------------------------------------------------
//==============================================================================
// Called when the Plugin is deactivated (active to inactive transition)
function ShapeEditorPlugin::onDeactivated(%this) {
	// Notify game objects if shape has been modified
	if ( ShapeEditor.isDirty() )
		ShapeEditor.shape.notifyShapeChanged();

	$gfx::wireframe = $wasInWireFrameMode;
	ShapeEdMaterials.updateSelectedMaterial(false);

	if( EditorGui-->MatEdPropertiesWindow.visible ) {
		ShapeEdMaterials.editSelectedMaterialEnd( true );
	}


	// Restore the original undo manager
	Editor.setUndoManager( %this.oldUndoMgr );
	// Restore menu bar
	%this.replaceMenuCmd( "Camera", 8, %this.oldCamFitCmd );
	%this.replaceMenuCmd( "Camera", 9, %this.oldCamFitOrbitCmd );
	Parent::onDeactivated(%this);
}
//------------------------------------------------------------------------------
//==============================================================================
// Called from TorqueLab after plugin is initialize to set needed settings
function SceneEditorPlugin::onPluginCreated( %this ) {
	EWorldEditor.dropType = SceneEditorPlugin.getCfg("DropType");
}
//------------------------------------------------------------------------------
//==============================================================================
// Called from TorqueLab when exitting the mission
function ShapeEditorPlugin::onExitMission( %this ) {
	// unselect the current shape
	ShapeEdShapeView.setModel( "" );

	if (ShapeEditor.shape != -1)
		delObj(ShapeEditor.shape);

	ShapeEditor.shape = 0;
	ShapeEdUndoManager.clearAll();
	ShapeEditor.setDirty( false );
	ShapeEdSequenceList.clear();
	ShapeEdNodeTreeView.removeItem( 0 );
	ShapeEdPropWindow.update_onNodeSelectionChanged( -1 );
	ShapeEdDetailTree.removeItem( 0 );
	ShapeEdMaterialList.clear();
	ShapeEdMountWindow-->mountList.clear();
	ShapeEdThreadWindow-->seqList.clear();
	ShapeEdThreadList.clear();
}
//------------------------------------------------------------------------------
//==============================================================================




function ShapeEditorPlugin::onPreSave( %this ) {
	ShapeEdShapeView.selectedNode = "-1";
	ShapeEdShapeView.selectedObject = "-1";
	ShapeEdShapeView.selectedObjDetail = "-1";
	ShapeEdShapeView.activeThread = "-1";
}


// Replace the command field in an Editor PopupMenu item (returns the original value)
function ShapeEditorPlugin::replaceMenuCmd(%this, %menuTitle, %id, %newCmd) {
	if (!$Cfg_UseCoreMenubar) return;

	%menu = Lab.findMenu( %menuTitle );
	%cmd = getField( %menu.item[%id], 2 );
	%menu.setItemCommand( %id, %newCmd );
	return %cmd;
}
