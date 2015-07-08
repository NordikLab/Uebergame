//==============================================================================
// TorqueLab -> Editor Gui Open and Closing States
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// Initial Editor launch call from EditorManager
function Editor::open(%this) {
	// prevent the mission editor from opening while the GuiEditor is open.
	if(Canvas.getContent() == GuiEditorGui.getId())
		return;

	Lab.attachEditorGuis();
	Lab.closeDisabledPluginsBin();

	if( !LabEditor.isInitialized )
		Lab.initializeEditorGui();

	%this.editorEnabled();

	//Set the wanted plugin activated
	if (!isObject(Lab.currentEditor))
		Lab.currentEditor = Lab.defaultPlugin;

	Lab.setEditor( Lab.currentEditor, true );
	EditorGui.previousGui = Canvas.getContent();
	//Lab.SyncEditor();
	Canvas.setContent(EditorGui);

	//The default menu seem to be create somewhere between setCOntent and here
	if(!$Cfg_UseCoreMenubar && isObject(Lab.menuBar))
		Lab.menuBar.removeFromCanvas();

	show(EWToolsToolbar);
	show(EWToolsPaletteContainer);
	ToolsToolbarArray.reorderChild( ToolsToolbarArray-->SceneEditorPlugin,ToolsToolbarArray.getObject(0));
	ToolsToolbarArray.refresh();
	Lab.updateActivePlugins();
	//Callbacks used by plugins
	Lab.OnEditorOpen();
	EditorFrameWorld.pushToBack(EditorFrameTools);
}
//------------------------------------------------------------------------------
//==============================================================================
// EditorGui OnWake -> When the EditorGui is rendered
function EditorGui::onWake( %this ) {
	Lab.setInitialCamera();
	//EHWorldEditor.setStateOn( 1 );
	startFileChangeNotifications();
	// Notify the editor plugins that the editor has started.

	foreach( %plugin in EditorPluginSet ){
		%plugin.onEditorWake();
	}
	
	//Reset the GameLabGui to default state
	GameLabGui.reset();

	// Push the ActionMaps in the order that we want to have them
	// before activating an editor plugin, so that if the plugin
	// installs an ActionMap, it will be highest on the stack.
	MoveMap.push();
	EditorMap.push();

	// Active the current editor plugin.

	if( !Lab.currentEditor.isActivated )
		Lab.currentEditor.onActivated();

	%slashPos = 0;

	while( strpos( $Server::MissionFile, "/", %slashPos ) != -1 ) {
		%slashPos = strpos( $Server::MissionFile, "/", %slashPos ) + 1;
	}

	%levelName = getSubStr( $Server::MissionFile , %slashPos , 99 );

	if( %levelName !$= Lab.levelName )
		%this.onNewLevelLoaded( %levelName );
}
//------------------------------------------------------------------------------
//==============================================================================
// Called when we have been set as the content and onWake has been called
function EditorGui::onSetContent(%this, %oldContent) {
	Lab.attachMenus();
}
//------------------------------------------------------------------------------
//==============================================================================
//EditManager Functions
//==============================================================================


//==============================================================================
function Lab::initializeEditorGui( %this ) {
	EWorldEditor.isDirty = false;
	ETerrainEditor.isDirty = false;
	ETerrainEditor.isMissionDirty = false;
	//Get up-to-date config
	Lab.readAllConfigArray();
	//Store some dimension for future session update
	Lab.toolbarHeight = EditorGuiToolbar.y;
	Lab.pluginBarHeight = EWToolsToolbar.y;
	Lab.paletteBarWidth = EWToolsPaletteContainer.x;

	if( LabEditor.isInitialized )
		return;

	$SelectedOperation = -1;
	$NextOperationId   = 1;
	$HeightfieldDirtyRow = -1;
	//Lab.buildMenus(); //Menu are built in initLabEditor
	Lab.loadPluginsPalettes();
	// Visibility Layer Window
	//Lab.addGui( EVisibilityLayers,"Dialog");
	EVisibilityLayers-->TabBook.selectPage(0);
	//-----------------------------------------------------
	// Old Editor Settings Window (For temporary references)
	//-----------------------------------------------------
	// Object Snap Options Window
	//Lab.addGui( ESnapOptions ,"Dialog");
	ESnapOptions-->TabBook.selectPage(0);
	// Transform Selection Window
	Lab.addGui( EToolOverlayGui ,"Overlay");
	Lab.addGui( ETools ,"Dialog");
	//Lab.addGui( ETransformSelection ,"Dialog");
	//Lab.addGui( ECloneTool ,"Dialog");
	//Lab.addGui( ECloneDrag ,"Dialog");
	Lab.addGui( ESceneManager ,"Dialog");
	// Manage Bookmarks Window
	Lab.addGui( EManageBookmarks ,"Dialog");
	Lab.addGui( EManageSFXParameters ,"Dialog");
	Lab.addGui( ESelectObjects ,"Dialog");
	
	EWorldEditor.init();
	EWorldEditor.setDisplayType($EditTsCtrl::DisplayTypePerspective);
	ETerrainEditor.init();
	//Creator.init();
	SceneCreatorWindow.init();
	ObjectBuilderGui.init();
	Lab.setMenuDefaultState();
	//EditorGuiToolbar-->cameraTypes.setBitmap("tlab/gui/icons/default/toolbar/player");
	/*
	EWorldEditorCameraSpeed.clear();
	EWorldEditorCameraSpeed.add("Slowest - Camera 1",0);
	EWorldEditorCameraSpeed.add("Slow - Camera 2",1);
	EWorldEditorCameraSpeed.add("Slower - Camera 3",2);
	EWorldEditorCameraSpeed.add("Normal - Camera 4",3);
	EWorldEditorCameraSpeed.add("Faster - Camera 5",4);
	EWorldEditorCameraSpeed.add("Fast - Camera 6",5);
	EWorldEditorCameraSpeed.add("Fastest - Camera 7",6);
	EWorldEditorCameraSpeed.setSelected(3);
	*/
	EWorldEditorAlignPopup.clear();
	EWorldEditorAlignPopup.add("World",0);
	EWorldEditorAlignPopup.add("Object",1);
	EWorldEditorAlignPopup.setSelected(0);
	Lab.initEditorCamera();
	// sync camera gui
	Lab.syncCameraGui();
	// this will brind EToolDlgCameraTypes to front so that it goes over the menubar
	EditorGui.pushToBack(EToolDlgCameraTypes);
	EditorGui.pushToBack(VisibilityDropdown);
	// dropdowns out so that they display correctly in editor gui
	// make sure to show the default world editor guis
	EditorGui.bringToFront( EWorldEditor );
	EWorldEditor.setVisible( false );

	// Call the startup callback on the editor plugins.
	for ( %i = 0; %i < EditorPluginSet.getCount(); %i++ ) {
		%obj = EditorPluginSet.getObject( %i );
		%obj.onWorldEditorStartup();
	}

	Lab.AddSelectionCallback("ETransformBox.updateSource","Transform");
	//Lab.setCameraViewMode("Standard Camera");
	// With everything loaded, start up the settings window
	// Start up initial editor plugin.
	// Done.
	LabEditor.isInitialized = true;
	Lab.initCoreGuis();
	Lab.resizeEditorGui();
	//Lab.initObjectConfigArray(EWorldEditor,"WorldEditor","General");
	
}
//------------------------------------------------------------------------------


//-----------------------------------------------------------------------------




//-----------------------------------------------------------------------------


//------------------------------------------------------------------------------

