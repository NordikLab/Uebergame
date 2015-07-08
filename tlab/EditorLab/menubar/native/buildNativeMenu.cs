//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// Build native menu using defined globals
function Lab::buildNativeMenus(%this) {
	%menuId = 1;
	while($LabMenu[%menuId] !$= ""){
		%menu = new PopupMenu() {
			superClass = "MenuBuilder";
			class = "EditorFileMenu";
			barTitle = $LabMenu[%menuId];
		};
		devLog("---------",$LabMenu[%menuId]);			
		%itemId = 1;
		while($LabMenuItem[%menuId,%itemId] !$= ""){			
			%subId = 1;
			devLog("---------",$LabMenuItem[%menuId,%itemId]);		
			if ($LabMenuSubMenuItem[%menuId,%itemId,%subId] $= ""){
				%menu.appendItem($LabMenuItem[%menuId,%itemId]);				
			}
			else {			
				%subMenu = new PopupMenu() {
					superClass = "MenuBuilder";
					class = "EditorFileMenu";				
				};	
				while($LabMenuSubMenuItem[%menuId,%itemId,%subId] !$= ""){
					%subMenu.appendItem($LabMenuSubMenuItem[%menuId,%itemId,%subId]);
					devLog("--------- --------",$LabMenuSubMenuItem[%menuId,%itemId,%subId]);		
					%subId++;
				}
				%menu.appendItem($LabMenuItem[%menuId,%itemId] TAB %subMenu);	
						
			}
			%itemId++;
		}		
		%menuId++;
	}	
}
//------------------------------------------------------------------------------
function Lab::buildMenus(%this) {
	if(isObject(%this.menuBar)) {
		warnLog("Menu is already created, skipping rest");
		return;
	}

	//set up %cmdctrl variable so that it matches OS standards
	if( $platform $= "macos" ) {
		%cmdCtrl = "Cmd";
		%menuCmdCtrl = "Cmd";
		%quitShortcut = "Cmd Q";
		%redoShortcut = "Cmd-Shift Z";
	} else {
		%cmdCtrl = "Ctrl";
		%menuCmdCtrl = "Alt";
		%quitShortcut = "Alt F4";
		%redoShortcut = "Ctrl Y";
	}

	// Sub menus (temporary, until MenuBuilder gets updated)
	// The speed increments located here are overwritten in EditorCameraSpeedMenu::setupDefaultState.
	// The new min/max for the editor camera speed range can be set in each level's levelInfo object.
	%this.menu["CameraSpeed"] = new PopupMenu(EditorCameraSpeedOptions) {
		superClass = "MenuBuilder";
		class = "EditorCameraSpeedMenu";
	};
	%this.menuCameraSpeed.appendItem("Slowest" TAB %cmdCtrl @ "-Shift 1" TAB "5");
	%this.menuCameraSpeed.appendItem("Slow" TAB %cmdCtrl @ "-Shift 2" TAB "35");
	%this.menuCameraSpeed.appendItem("Slower" TAB %cmdCtrl @ "-Shift 3" TAB "70");
	%this.menuCameraSpeed.appendItem("Normal" TAB %cmdCtrl @ "-Shift 4" TAB "100");
	%this.menuCameraSpeed.appendItem("Faster" TAB %cmdCtrl @ "-Shift 5" TAB "130");
	%this.menuCameraSpeed.appendItem("Fast" TAB %cmdCtrl @ "-Shift 6" TAB "165");
	%this.menuCameraSpeed.appendItem("Fastest" TAB %cmdCtrl @ "-Shift 7" TAB "200");
	%this.freeCameraTypeMenu = new PopupMenu(EditorFreeCameraTypeOptions) {
		superClass = "MenuBuilder";
		class = "EditorFreeCameraTypeMenu";
		item[0] = "Standard" TAB "Ctrl 1" TAB "EditorGuiStatusBar.setCamera(\"Standard Camera\");";
		item[1] = "Orbit Camera" TAB "Ctrl 2" TAB "EditorGuiStatusBar.setCamera(\"Orbit Camera\");";
		Item[2] = "-";
		item[3] = "Smoothed" TAB "" TAB "EditorGuiStatusBar.setCamera(\"Smooth Camera\");";
		item[4] = "Smoothed Rotate" TAB "" TAB "EditorGuiStatusBar.setCamera(\"Smooth Rot Camera\");";
	};
	%this.playerCameraTypeMenu = new PopupMenu(EditorPlayerCameraTypeOptions) {
		superClass = "MenuBuilder";
		class = "EditorPlayerCameraTypeMenu";
		Item[0] = "First Person" TAB "" TAB "EditorGuiStatusBar.setCamera(\"1st Person Camera\");";
		Item[1] = "Third Person" TAB "" TAB "EditorGuiStatusBar.setCamera(\"3rd Person Camera\");";
	};
	%this.cameraBookmarksMenu = new PopupMenu(EditorCameraBookmarks) {
		superClass = "MenuBuilder";
		class = "EditorCameraBookmarksMenu";
		//item[0] = "None";
	};
	%this.viewTypeMenu = new PopupMenu() {
		superClass = "MenuBuilder";
		item[ 0 ] = "Top" TAB "Alt 2" TAB "EditorGuiStatusBar.setCamera(\"Top View\");";
		item[ 1 ] = "Bottom" TAB "Alt 5" TAB "EditorGuiStatusBar.setCamera(\"Bottom View\");";
		item[ 2 ] = "Front" TAB "Alt 3" TAB "EditorGuiStatusBar.setCamera(\"Front View\");";
		item[ 3 ] = "Back" TAB "Alt 6" TAB "EditorGuiStatusBar.setCamera(\"Back View\");";
		item[ 4 ] = "Left" TAB "Alt 4" TAB "EditorGuiStatusBar.setCamera(\"Left View\");";
		item[ 5 ] = "Right" TAB "Alt 7" TAB "EditorGuiStatusBar.setCamera(\"Right View\");";
		item[ 6 ] = "Perspective" TAB "Alt 1" TAB "EditorGuiStatusBar.setCamera(\"Standard Camera\");";
		item[ 7 ] = "Isometric" TAB "Alt 8" TAB "EditorGuiStatusBar.setCamera(\"Isometric View\");";
	};
	// Menu bar
	%this.menuBar = new MenuBar() {
		dynamicItemInsertPos = 3;
	};
//-----------------------------------------------------
//=====================================================
// MainMenu -> File Menu
	%fileMenu = new PopupMenu() {
		superClass = "MenuBuilder";
		class = "EditorFileMenu";
		barTitle = "File";
	};

	if(!isWebDemo()) {
		%fileMenu.appendItem("New Level" TAB "" TAB "schedule( 1, 0, \"EditorNewLevel\" );");
		%fileMenu.appendItem("Open Level..." TAB %cmdCtrl SPC "O" TAB "schedule( 1, 0, \"EditorOpenMission\" );");
		%fileMenu.appendItem("Save Level" TAB %cmdCtrl SPC "S" TAB "Lab.SaveCurrentMission();");
		%fileMenu.appendItem("Save Level As..." TAB "" TAB "Lab.SaveCurrentMission(true);");
		%fileMenu.appendItem("-");

		if( $platform $= "windows" ) {
			%fileMenu.appendItem( "Open Project in Torsion" TAB "" TAB "EditorOpenTorsionProject();" );
			%fileMenu.appendItem( "Open Level File in Torsion" TAB "" TAB "EditorOpenFileInTorsion();" );
			%fileMenu.appendItem( "-" );
		}
	}

	%fileMenu.appendItem("Create Blank Terrain" TAB "" TAB "Canvas.pushDialog( CreateNewTerrainGui );");
	%fileMenu.appendItem("Import Terrain Heightmap" TAB "" TAB "Canvas.pushDialog( TerrainImportGui );");

	if(!isWebDemo()) {
		%fileMenu.appendItem("Export Terrain Heightmap" TAB "" TAB "Canvas.pushDialog( TerrainExportGui );");
		%fileMenu.appendItem("-");
		%fileMenu.appendItem("Export To COLLADA..." TAB "" TAB "EditorExportToCollada();");
	}

	%fileMenu.appendItem( "-" );
	%fileMenu.appendItem( "Add FMOD Designer Audio..." TAB "" TAB "AddFMODProjectDlg.show();" );
	%fileMenu.appendItem("-");
	%fileMenu.appendItem("Play Level" TAB "F11" TAB "Editor.close($HudCtrl);");

	if(!isWebDemo()) {
		%fileMenu.appendItem("Exit Level" TAB "" TAB "EditorExitMission();");
		%fileMenu.appendItem("Quit" TAB %quitShortcut TAB "EditorQuitGame();");
	}

	%this.menuBar.insert(%fileMenu, %this.menuBar.getCount());
//-----------------------------------------------------
//=====================================================
// MainMenu -> Edit Menu
	%editMenu = new PopupMenu() {
		superClass = "MenuBuilder";
		class = "EditorEditMenu";
		internalName = "EditMenu";
		barTitle = "Edit";
	};
	%editMenu.appendItem("Undo" TAB %cmdCtrl SPC "Z" TAB "Editor.getUndoManager().undo();");
	%editMenu.appendItem("Redo" TAB %redoShortcut TAB "Editor.getUndoManager().redo();");
	%editMenu.appendItem("-");
	%editMenu.appendItem("Cut" TAB %cmdCtrl SPC "X" TAB "EditorMenuEditCut();");
	%editMenu.appendItem("Copy" TAB %cmdCtrl SPC "C" TAB "EditorMenuEditCopy();");
	%editMenu.appendItem("Paste" TAB %cmdCtrl SPC "V" TAB "EditorMenuEditPaste();");
	%editMenu.appendItem("Delete" TAB "Delete" TAB "EditorMenuEditDelete();");
	%editMenu.appendItem("-");
	%editMenu.appendItem("Deselect" TAB "X" TAB "EditorMenuEditDeselect();");
	%editMenu.appendItem("Select..." TAB "" TAB "ESelectObjects.toggleVisibility();");
	%editMenu.appendItem("-");
	%editMenu.appendItem("Audio Parameters..." TAB "" TAB "EManageSFXParameters.ToggleVisibility();");
	%editMenu.appendItem("LabEditor Settings..." TAB "" TAB "toggleDlg(LabSettingsDlg);");
	%editMenu.appendItem("Snap Options..." TAB "" TAB "ESnapOptions.ToggleVisibility();");
	%editMenu.appendItem("-");
	%editMenu.appendItem("Game Options..." TAB "" TAB "Canvas.pushDialog(DlgOptions);");
	%editMenu.appendItem("PostEffect Manager" TAB "" TAB "Canvas.pushDialog(PostFXManager);");
	%editMenu.appendItem("Copy Tool" TAB "" TAB "toggleDlg(ToolObjectCopyDlg);");
	%this.menuBar.insert(%editMenu, %this.menuBar.getCount());
//-----------------------------------------------------
//=====================================================
// MainMenu -> View Menu
	%viewMenu = new PopupMenu() {
		superClass = "MenuBuilder";
		class = "EditorViewMenu";
		internalName = "viewMenu";
		barTitle = "View";
	};
	%viewMenu.appendItem("Visibility Layers" TAB "Alt V" TAB "VisibilityDropdownToggle();");
	%viewMenu.appendItem("Show Grid in Ortho Views" TAB %cmdCtrl @ "-Shift-Alt G" TAB "EWorldEditor.renderOrthoGrid = !EWorldEditor.renderOrthoGrid;");
	%this.menuBar.insert(%viewMenu, %this.menuBar.getCount());
//-----------------------------------------------------
//=====================================================
// MainMenu -> View Menu
	%utilityMenu = new PopupMenu() {
		superClass = "MenuBuilder";
		class = "EditorUtilityMenu";
		internalName = "utilityMenu";
		barTitle = "Utility";
	};
	%utilityMenu.appendItem("Resize Guis" TAB "" TAB "Lab.resizeEditorGui();");
	%this.menuBar.insert(%utilityMenu, %this.menuBar.getCount());
//-----------------------------------------------------
//=====================================================
// MainMenu -> Camera Menu
	%cameraMenu = new PopupMenu() {
		superClass = "MenuBuilder";
		class = "EditorCameraMenu";
		barTitle = "Camera";
	};
	%cameraMenu.appendItem("World Camera" TAB %this.freeCameraTypeMenu);
	%cameraMenu.appendItem("Player Camera" TAB %this.playerCameraTypeMenu);
	%cameraMenu.appendItem("-");
	%cameraMenu.appendItem("Toggle Camera" TAB %menuCmdCtrl SPC "C" TAB "cmdServer('ToggleCamera');");
	%cameraMenu.appendItem("Place Camera at Selection" TAB "Ctrl Q" TAB "EWorldEditor.dropCameraToSelection();");
	%cameraMenu.appendItem("Place Camera at Player" TAB "Alt Q" TAB "commandToServer('dropCameraAtPlayer');");
	%cameraMenu.appendItem("Place Player at Camera" TAB "Alt W" TAB "commandToServer('DropPlayerAtCamera');");
	%cameraMenu.appendItem("-");
	%cameraMenu.appendItem("Fit View to Selection" TAB "F" TAB "commandToServer('EditorCameraAutoFit', EWorldEditor.getSelectionRadius()+1);");
	%cameraMenu.appendItem("Fit View To Selection and Orbit" TAB "Alt F" TAB "EditorGuiStatusBar.setCamera(\"Orbit Camera\"); commandToServer('EditorCameraAutoFit', EWorldEditor.getSelectionRadius()+1);");
	%cameraMenu.appendItem("-");
	%cameraMenu.appendItem("Speed" TAB %this.menuCameraSpeed);
	%cameraMenu.appendItem("View" TAB %this.viewTypeMenu);
	%cameraMenu.appendItem("-");
	%cameraMenu.appendItem("Add Bookmark..." TAB "Ctrl B" TAB "EManageBookmarks.addCameraBookmarkByGui();");
	%cameraMenu.appendItem("Manage Bookmarks..." TAB "Ctrl-Shift B" TAB "EManageBookmarks.ToggleVisibility();");
	%cameraMenu.appendItem("Jump to Bookmark" TAB %this.cameraBookmarksMenu);
	%this.menuBar.insert(%cameraMenu, %this.menuBar.getCount());
//-----------------------------------------------------
//=====================================================
// MainMenu -> Editors Menu
	%editorsMenu = new PopupMenu() {
		superClass = "MenuBuilder";
		class = "EditorToolsMenu";
		barTitle = "Editors";
	};
	%this.menuBar.insert(%editorsMenu, %this.menuBar.getCount());
//-----------------------------------------------------
//=====================================================
// MainMenu -> Lighting Menu
	%lightingMenu = new PopupMenu() {
		superClass = "MenuBuilder";
		class = "EditorLightingMenu";
		barTitle = "Lighting";
		item[0] = "Full Relight" TAB "Alt L" TAB "Editor.lightScene(\"\", forceAlways);";
		item[1] = "Toggle ShadowViz" TAB "" TAB "toggleShadowViz();";
		item[2] = "-";
		// NOTE: The light managers will be inserted as the
		// last menu items in EditorLightingMenu::onAdd().
	};
	%this.menuBar.insert(%lightingMenu, %this.menuBar.getCount());
//-----------------------------------------------------
//=====================================================
// MainMenu -> Help Menu
	%helpMenu = new PopupMenu() {
		superClass = "MenuBuilder";
		class = "EditorHelpMenu";
		barTitle = "Help";
		item[0] = "Online Documentation..." TAB "Alt F1" TAB "gotoWebPage(EWorldEditor.documentationURL);";
		item[1] = "Offline User Guide..." TAB "" TAB "gotoWebPage(EWorldEditor.documentationLocal);";
		item[2] = "Offline Reference Guide..." TAB "" TAB "shellexecute(EWorldEditor.documentationReference);";
		item[3] = "Torque 3D Forums..." TAB "" TAB "gotoWebPage(EWorldEditor.forumURL);";
	};
	%this.menuBar.insert(%helpMenu, %this.menuBar.getCount());

	// Menus that are added/removed dynamically (temporary)

//-----------------------------------------------------
//=====================================================
// MainMenu -> World Menu
	if(! isObject(%this.worldMenu)) {
		%this.dropTypeMenu = new PopupMenu() {
			superClass = "MenuBuilder";
			class = "EditorDropTypeMenu";
			// The onSelectItem() callback for this menu re-purposes the command field
			// as the MenuBuilder version is not used.
			item[0] = "at Origin" TAB "" TAB "atOrigin";
			item[1] = "at Camera" TAB "" TAB "atCamera";
			item[2] = "at Camera w/Rotation" TAB "" TAB "atCameraRot";
			item[3] = "Below Camera" TAB "" TAB "belowCamera";
			item[4] = "Screen Center" TAB "" TAB "screenCenter";
			item[5] = "at Centroid" TAB "" TAB "atCentroid";
			item[6] = "to Terrain" TAB "" TAB "toTerrain";
			item[7] = "Below Selection" TAB "" TAB "belowSelection";
		};
		%this.alignBoundsMenu = new PopupMenu() {
			superClass = "MenuBuilder";
			class = "EditorAlignBoundsMenu";
			// The onSelectItem() callback for this menu re-purposes the command field
			// as the MenuBuilder version is not used.
			item[0] = "+X Axis" TAB "" TAB "0";
			item[1] = "+Y Axis" TAB "" TAB "1";
			item[2] = "+Z Axis" TAB "" TAB "2";
			item[3] = "-X Axis" TAB "" TAB "3";
			item[4] = "-Y Axis" TAB "" TAB "4";
			item[5] = "-Z Axis" TAB "" TAB "5";
		};
		%this.alignCenterMenu = new PopupMenu() {
			superClass = "MenuBuilder";
			class = "EditorAlignCenterMenu";
			// The onSelectItem() callback for this menu re-purposes the command field
			// as the MenuBuilder version is not used.
			item[0] = "X Axis" TAB "" TAB "0";
			item[1] = "Y Axis" TAB "" TAB "1";
			item[2] = "Z Axis" TAB "" TAB "2";
		};
//-----------------------------------------------------
//=====================================================
// MainMenu -> Object Menu
		%objectMenu = new PopupMenu() {
			superClass = "MenuBuilder";
			class = "EditorWorldMenu";
			barTitle = "Object";
		};
		%objectMenu.appendItem("Lock Selection" TAB %cmdCtrl @ " L" TAB "EWorldEditor.lockSelection(true); EWorldEditor.syncGui();");
		%objectMenu.appendItem("Unlock Selection" TAB %cmdCtrl @ "-Shift L" TAB "EWorldEditor.lockSelection(false); EWorldEditor.syncGui();");
		%objectMenu.appendItem("-");
		%objectMenu.appendItem("Hide Selection" TAB %cmdCtrl @ " H" TAB "EWorldEditor.hideSelection(true); EWorldEditor.syncGui();");
		%objectMenu.appendItem("Show Selection" TAB %cmdCtrl @ "-Shift H" TAB "EWorldEditor.hideSelection(false); EWorldEditor.syncGui();");
		%objectMenu.appendItem("-");
		%objectMenu.appendItem("Group Selection" TAB %cmdCtrl @ " G" TAB "Lab.groupSelectedObjects();");
		%objectMenu.appendItem("Group Selection" TAB %cmdCtrl @ "-Shift G" TAB "Lab.ungroupSelectedObjects();");
		%objectMenu.appendItem("-");
		%objectMenu.appendItem("Align Bounds" TAB %this.alignBoundsMenu);
		%objectMenu.appendItem("Align Center" TAB %this.alignCenterMenu);
		%objectMenu.appendItem("-");
		%objectMenu.appendItem("Reset Transforms" TAB "Ctrl R" TAB "EWorldEditor.resetTransforms();");
		%objectMenu.appendItem("Reset Selected Rotation" TAB "" TAB "EWorldEditor.resetSelectedRotation();");
		%objectMenu.appendItem("Reset Selected Scale" TAB "" TAB "EWorldEditor.resetSelectedScale();");
		%objectMenu.appendItem("Transform Selection..." TAB "Ctrl T" TAB "ETransformSelection.ToggleVisibility();");
		%objectMenu.appendItem("-");
		//item[13] = "Drop Camera to Selection" TAB "Ctrl Q" TAB "EWorldEditor.dropCameraToSelection();";
		//item[14] = "Add Selection to Instant Group" TAB "" TAB "EWorldEditor.addSelectionToAddGroup();";
		%objectMenu.appendItem("Drop Selection" TAB "Ctrl D" TAB "EWorldEditor.dropSelection();");
		//item[15] = "-";
		%objectMenu.appendItem("Drop Location" TAB %this.dropTypeMenu);
		%objectMenu.appendItem("-");
		%objectMenu.appendItem("Make Selection Prefab" TAB "" TAB "Lab.CreatePrefab();");
		%objectMenu.appendItem("Explode Selected Prefab" TAB "" TAB "Lab.ExplodePrefab();");
		%objectMenu.appendItem("-");
		%objectMenu.appendItem("Mount Selection A to B" TAB "" TAB "EditorMount();");
		%objectMenu.appendItem("Unmount Selected Object" TAB "" TAB "EditorUnmount();");
		%objectMenu.appendItem("-");
		%objectMenu.appendItem("Objects Manager" TAB  %cmdCtrl @ " M" TAB "ESelectObjects.toggleVisibility();");
		%this.worldMenu = %objectMenu;
	}
}


//////////////////////////////////////////////////////////////////////////



//////////////////////////////////////////////////////////////////////////

function Lab::findMenu(%this, %name) {
	if(! isObject(%this.menuBar))
		return 0;

	for(%i = 0; %i < %this.menuBar.getCount(); %i++) {
		%menu = %this.menuBar.getObject(%i);

		if(%name $= %menu.barTitle)
			return %menu;
	}

	return 0;
}
