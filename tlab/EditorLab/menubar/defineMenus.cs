//==============================================================================
// TorqueLab -> Initialize editor plugins
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
function Lab::initLabMenuData(%this,%buildAfter) {
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

	%id = -1;
	%itemId = -1;
	$LabMenu[%id++] = "File";
	$LabMenuItem[%id,%itemId++] = "Open Level..." TAB "numpad0" TAB "schedule( 1, 0, \"EditorOpenMission\");";
	$LabMenuItem[%id,%itemId++] = "Save Level" TAB "Ctrl S" TAB "Lab.SaveCurrentMission();";
	$LabMenuItem[%id,%itemId++] = "Save Level As..." TAB "" TAB "Lab.SaveCurrentMission(true);";
	$LabMenuItem[%id,%itemId++] = "-";
	$LabMenuItem[%id,%itemId++] =  "Open Project in Torsion" TAB "" TAB "EditorOpenTorsionProject();";
	$LabMenuItem[%id,%itemId++] =  "Open Level File in Torsion" TAB "" TAB "EditorOpenFileInTorsion();";
	$LabMenuItem[%id,%itemId++] =  "-";
	$LabMenuItem[%id,%itemId++] = "Create Blank Terrain" TAB "" TAB "Canvas.pushDialog( CreateNewTerrainGui );";
	$LabMenuItem[%id,%itemId++] = "Import Terrain Heightmap" TAB "" TAB "Canvas.pushDialog( TerrainImportGui );";
	$LabMenuItem[%id,%itemId++] = "Export Terrain Heightmap" TAB "" TAB "Canvas.pushDialog( TerrainExportGui );";
	$LabMenuItem[%id,%itemId++] = "-";
	$LabMenuItem[%id,%itemId++] = "Export To COLLADA..." TAB "" TAB "EditorExportToCollada();";
	$LabMenuItem[%id,%itemId++] =  "-";
	$LabMenuItem[%id,%itemId++] =  "Add FMOD Designer Audio..." TAB "" TAB "AddFMODProjectDlg.show();";
	$LabMenuItem[%id,%itemId++] = "-";
	$LabMenuItem[%id,%itemId++] = "Play Level" TAB "F11" TAB "Editor.close($HudCtrl);";
	$LabMenuItem[%id,%itemId++] = "Exit Level" TAB "" TAB "EditorExitMission();";
	$LabMenuItem[%id,%itemId++] = "Quit" TAB %quitShortcut TAB "EditorQuitGame();";
	%itemId = -1;
	$LabMenu[%id++] = "Edit";
	$LabMenuItem[%id,%itemId++] = "Undo" TAB %cmdCtrl SPC "Z" TAB "Editor.getUndoManager().undo();";
	$LabMenuItem[%id,%itemId++] = "Redo" TAB %redoShortcut TAB "Editor.getUndoManager().redo();";
	$LabMenuItem[%id,%itemId++] = "-";
	$LabMenuItem[%id,%itemId++] = "Cut" TAB %cmdCtrl SPC "X" TAB "EditorMenuEditCut();";
	$LabMenuItem[%id,%itemId++] = "Copy" TAB %cmdCtrl SPC "C" TAB "EditorMenuEditCopy();";
	$LabMenuItem[%id,%itemId++] = "Paste" TAB %cmdCtrl SPC "V" TAB "EditorMenuEditPaste();";
	$LabMenuItem[%id,%itemId++] = "Delete" TAB "Delete" TAB "EditorMenuEditDelete();";
	$LabMenuItem[%id,%itemId++] = "-";
	$LabMenuItem[%id,%itemId++] = "Deselect" TAB "X" TAB "EditorMenuEditDeselect();";
	$LabMenuItem[%id,%itemId++] = "Select..." TAB "" TAB "ESelectObjects.toggleVisibility();";
	$LabMenuItem[%id,%itemId++] = "-";
	$LabMenuItem[%id,%itemId++] = "Audio Parameters..." TAB "" TAB "EManageSFXParameters.ToggleVisibility();";
	$LabMenuItem[%id,%itemId++] = "LabEditor Settings..." TAB "" TAB "toggleDlg(LabSettingsDlg);";
	$LabMenuItem[%id,%itemId++] = "Snap Options..." TAB "" TAB "ESnapOptions.ToggleVisibility();";
	$LabMenuItem[%id,%itemId++] = "-";
	$LabMenuItem[%id,%itemId++] = "Game Options..." TAB "" TAB "Canvas.pushDialog(DlgOptions);";
	$LabMenuItem[%id,%itemId++] = "PostEffect Manager" TAB "" TAB "Canvas.pushDialog(PostFXManager);";
	$LabMenuItem[%id,%itemId++] = "Copy Tool" TAB "" TAB "toggleDlg(ToolObjectCopyDlg);";
	$LabMenuItem[%id,%itemId++] = "Toggle transform box" TAB "" TAB "ETransformBox.toggleBox();";
	%itemId = -1;
	$LabMenu[%id++] = "View";
	$LabMenuItem[%id,%itemId++] = "Visibility Layers" TAB "Alt V" TAB "EVisibilityLayers.toggleVisibility();";
	$LabMenuItem[%id,%itemId++] = "Show Grid in Ortho Views" TAB %cmdCtrl @ "-Shift-Alt G" TAB "EWorldEditor.renderOrthoGrid = !EWorldEditor.renderOrthoGrid;";
	%itemId = -1;
	$LabMenu[%id++] = "Object";
	$LabMenuItem[%id,%itemId++] = "Lock Selection" TAB %cmdCtrl @ " L" TAB "EWorldEditor.lockSelection(true); EWorldEditor.syncGui();";
	$LabMenuItem[%id,%itemId++] = "Unlock Selection" TAB %cmdCtrl @ "-Shift L" TAB "EWorldEditor.lockSelection(false); EWorldEditor.syncGui();";
	$LabMenuItem[%id,%itemId++] = "-";
	$LabMenuItem[%id,%itemId++] = "Hide Selection" TAB %cmdCtrl @ " H" TAB "EWorldEditor.hideSelection(true); EWorldEditor.syncGui();";
	$LabMenuItem[%id,%itemId++] = "Show Selection" TAB %cmdCtrl @ "-Shift H" TAB "EWorldEditor.hideSelection(false); EWorldEditor.syncGui();";
	$LabMenuItem[%id,%itemId++] = "-";
	$LabMenuItem[%id,%itemId++] = "Group Selection" TAB %cmdCtrl @ " G" TAB "Lab.groupSelectedObjects();";
	$LabMenuItem[%id,%itemId++] = "Group Selection" TAB %cmdCtrl @ "-Shift G" TAB "Lab.ungroupSelectedObjects();";
	$LabMenuItem[%id,%itemId++] = "-";
	$LabMenuItem[%id,%itemId++] = "Align Bounds";
	$LabMenuItem[%id,%itemId++] = "Align Center";
	$LabMenuItem[%id,%itemId++] = "-";
	$LabMenuItem[%id,%itemId++] = "Reset Transforms" TAB "Ctrl R" TAB "EWorldEditor.resetTransforms();";
	$LabMenuItem[%id,%itemId++] = "Reset Selected Rotation" TAB "" TAB "EWorldEditor.resetSelectedRotation();";
	$LabMenuItem[%id,%itemId++] = "Reset Selected Scale" TAB "" TAB "EWorldEditor.resetSelectedScale();";
	$LabMenuItem[%id,%itemId++] = "Transform Selection..." TAB "Ctrl T" TAB "ETransformSelection.ToggleVisibility();";
	$LabMenuItem[%id,%itemId++] = "-";
	//item[13] = "Drop Camera to Selection" TAB "Ctrl Q" TAB "EWorldEditor.dropCameraToSelection();";
	//item[14] = "Add Selection to Instant Group" TAB "" TAB "EWorldEditor.addSelectionToAddGroup();";
	$LabMenuItem[%id,%itemId++] = "Drop Selection" TAB "Ctrl D" TAB "EWorldEditor.dropSelection();";
	//item[15] = "-";
	$LabMenuItem[%id,%itemId++] = "Drop Location";
	%subId=-1;
	$LabMenuSubMenuItem[%id,%itemId,%subId++] = "at Origin" TAB "" TAB "EWorldEditor.droptype = \"atOrigin\"";
	$LabMenuSubMenuItem[%id,%itemId,%subId++] = "at Camera" TAB "" TAB "EWorldEditor.droptype = \"atCamera";
	$LabMenuSubMenuItem[%id,%itemId,%subId++] = "at Camera w/Rotation" TAB "" TAB "EWorldEditor.droptype = \"atCameraRot\"";
	$LabMenuSubMenuItem[%id,%itemId,%subId++] = "Below Camera" TAB "" TAB "EWorldEditor.droptype = \"belowCamera\"";
	$LabMenuSubMenuItem[%id,%itemId,%subId++] = "Screen Center" TAB "" TAB "EWorldEditor.droptype = \"screenCenter\"";
	$LabMenuSubMenuItem[%id,%itemId,%subId++] = "at Centroid" TAB "" TAB "EWorldEditor.droptype = \"atCentroid\"";
	$LabMenuSubMenuItem[%id,%itemId,%subId++] = "to Terrain" TAB "" TAB "EWorldEditor.droptype = \"toTerrain\"";
	$LabMenuSubMenuItem[%id,%itemId,%subId++] = "Below Selection" TAB "" TAB "EWorldEditor.droptype = \"belowSelection\"";
	$LabMenuItem[%id,%itemId++] = "-";
	$LabMenuItem[%id,%itemId++] = "Make Selection Prefab" TAB "" TAB "Lab.CreatePrefab();";
	$LabMenuItem[%id,%itemId++] = "Explode Selected Prefab" TAB "" TAB "Lab.ExplodePrefab();";
	$LabMenuItem[%id,%itemId++] = "-";
	$LabMenuItem[%id,%itemId++] = "Mount Selection A to B" TAB "" TAB "EditorMount();";
	$LabMenuItem[%id,%itemId++] = "Unmount Selected Object" TAB "" TAB "EditorUnmount();";
	$LabMenuItem[%id,%itemId++] = "-";
	$LabMenuItem[%id,%itemId++] = "Objects Manager" TAB  %cmdCtrl @ " M" TAB "ESelectObjects.toggleVisibility();";
	%itemId = -1;
	$LabMenu[%id++] = "Utility";
	$LabMenuItem[%id,%itemId++] = "Full Relight" TAB "Alt L" TAB "Editor.lightScene(\"\", forceAlways);";
	$LabMenuItem[%id,%itemId++] = "Toggle ShadowViz" TAB "" TAB "toggleShadowViz();";
	$LabMenuItem[%id,%itemId++] = "-------------";
	$LabMenuItem[%id,%itemId++] = "Open disabled plugins bin" TAB "" TAB "Lab.openDisabledPluginsBin();";
	$LabMenuItem[%id,%itemId++] = "Customize Interface" TAB "" TAB "ETools.toggleTool(\"GuiCustomizer\");";
	
	%itemId = -1;
	$LabMenu[%id++] = "Camera";
	$LabMenuItem[%id,%itemId++] = "World Camera";
	$LabMenuItem[%id,%itemId++] = "Player Camera";
	$LabMenuItem[%id,%itemId++] = "-";
	$LabMenuItem[%id,%itemId++] = "Toggle Control Object" TAB %menuCmdCtrl SPC "M" TAB "cmdServer(\"Lab.toggleControlObject\");";
	$LabMenuItem[%id,%itemId++] = "Toggle Camera" TAB %menuCmdCtrl SPC "C" TAB "cmdServer(\"Game.ToggleClientCamera\");";
	$LabMenuItem[%id,%itemId++] = "Place Camera at Selection" TAB "Ctrl Q" TAB "EWorldEditor.dropCameraToSelection();";
	$LabMenuItem[%id,%itemId++] = "Place Camera at Player" TAB "Alt Q" TAB "commandToServer('dropCameraAtPlayer');";
	$LabMenuItem[%id,%itemId++] = "Place Player at Camera" TAB "Alt W" TAB "commandToServer('DropPlayerAtCamera');";
	$LabMenuItem[%id,%itemId++] = "-";
	$LabMenuItem[%id,%itemId++] = "Fit View to Selection" TAB "F" TAB "commandToServer('EditorCameraAutoFit', EWorldEditor.getSelectionRadius()+1);";
	$LabMenuItem[%id,%itemId++] = "Fit View To Selection and Orbit" TAB "Alt F" TAB "EditorGuiStatusBar.setCamera(\"Orbit Camera\"; commandToServer('EditorCameraAutoFit', EWorldEditor.getSelectionRadius()+1);";
	$LabMenuItem[%id,%itemId++] = "-";
	$LabMenuItem[%id,%itemId++] = "Speed";
	%subId=-1;
	$LabMenuSubMenuItem[%id,%itemId,%subId++] = "Slowest" TAB %cmdCtrl @ "-Shift 1" TAB "5";
	$LabMenuSubMenuItem[%id,%itemId,%subId++] = "Slow" TAB %cmdCtrl @ "-Shift 2" TAB "35";
	$LabMenuSubMenuItem[%id,%itemId,%subId++] = "Slower" TAB %cmdCtrl @ "-Shift 3" TAB "70";
	$LabMenuSubMenuItem[%id,%itemId,%subId++] = "Normal" TAB %cmdCtrl @ "-Shift 4" TAB "100";
	$LabMenuSubMenuItem[%id,%itemId,%subId++] = "Faster" TAB %cmdCtrl @ "-Shift 5" TAB "130";
	$LabMenuSubMenuItem[%id,%itemId,%subId++] = "Fast" TAB %cmdCtrl @ "-Shift 6" TAB "165";
	$LabMenuSubMenuItem[%id,%itemId,%subId++] = "Fastest" TAB %cmdCtrl @ "-Shift 7" TAB "200";
	$LabMenuItem[%id,%itemId++] = "View";
	%subId=-1;
	$LabMenuSubMenuItem[%id,%itemId,%subId++] = "Top" TAB "Alt 2" TAB "Lab.setCameraViewType(\"Top View\");";
	$LabMenuSubMenuItem[%id,%itemId,%subId++] = "Bottom" TAB "Alt 5" TAB "Lab.setCameraViewType(\"Bottom View\");";
	$LabMenuSubMenuItem[%id,%itemId,%subId++] = "Front" TAB "Alt 3" TAB "Lab.setCameraViewType(\"Front View\");";
	$LabMenuSubMenuItem[%id,%itemId,%subId++] = "Back" TAB "Alt 6" TAB "Lab.setCameraViewType(\"Back View\");";
	$LabMenuSubMenuItem[%id,%itemId,%subId++] = "Left" TAB "Alt 4" TAB "Lab.setCameraViewType(\"Left View\");";
	$LabMenuSubMenuItem[%id,%itemId,%subId++]= "Right" TAB "Alt 7" TAB "Lab.setCameraViewType(\"Right View\");";
	$LabMenuSubMenuItem[%id,%itemId,%subId++] = "Perspective" TAB "Alt 1" TAB "Lab.setCameraViewType(\"Standard Camera\");";
	$LabMenuSubMenuItem[%id,%itemId,%subId++] = "Isometric" TAB "Alt 8" TAB "Lab.setCameraViewType(\"Isometric View\");";
	$LabMenuItem[%id,%itemId++] = "-";
	$LabMenuItem[%id,%itemId++] = "Add Bookmark..." TAB "Ctrl B" TAB "EManageBookmarks.addCameraBookmarkByGui();";
	$LabMenuItem[%id,%itemId++] = "Manage Bookmarks..." TAB "Ctrl-Shift B" TAB "EManageBookmarks.ToggleVisibility();";
	$LabMenuItem[%id,%itemId++] = "Jump to Bookmark";
	%itemId = -1;
	$LabMenu[%id++] = "Mission";
	$LabMenuItem[%id,%itemId++] = "Mission settings" TAB "" TAB "toggleDlg(LabMissionSettingsDlg);";
	$LabMenuItem[%id,%itemId++] = "Set next screenshot as preview" TAB "" TAB "Lab.setNextScreenShotPreview();";
	%itemId = -1;
	$LabMenu[%id++] = "Tool";
	$LabMenuItem[%id,%itemId++] = "Editors";
	$LabMenuEditorSubMenu = %id SPC %itemId;
	$LabMenuEditorNextId = -1;
	$LabMenuItem[%id,%itemId++] = "Capture current view as level preview" TAB "" TAB "Lab.setCurrentViewAsPreview();";
	$LabMenuItem[%id,%itemId++] = "Set next screenshot as preview" TAB "" TAB "Lab.setNextScreenShotPreview();";
	$LabMenuItem[%id,%itemId++] = "----------------------";
	$LabMenuItem[%id,%itemId++] = "Toggle GroundCover Manager" TAB "" TAB "SceneEditorDialogs.toggleDlg(\"GroundCover\");";
	$LabMenuItem[%id,%itemId++] = "Toggle Ambient Manager" TAB "" TAB "SceneEditorDialogs.toggleDlg(\"AmbientManager\");";
	$LabMenuItem[%id,%itemId++] = "----------------------";
	$LabMenuItem[%id,%itemId++] = "Auto arrange MissionGroup Root" TAB "" TAB "SEP_ScenePage.organizeMissionGroup();";
	$LabMenuItem[%id,%itemId++] = "Auto arrange MissionGroup" TAB "" TAB "SEP_ScenePage.organizeMissionGroup(5);";
	%itemId = -1; 
	$LabMenu[%id++] = "Help";
	$LabMenuItem[%id,%itemId++] = "Online Documentation..." TAB "Alt F1" TAB "gotoWebPage(EWorldEditor.documentationURL);";
	$LabMenuItem[%id,%itemId++] = "Offline User Guide..." TAB "" TAB "gotoWebPage(EWorldEditor.documentationLocal);";
	$LabMenuItem[%id,%itemId++] = "Offline Reference Guide..." TAB "" TAB "shellexecute(EWorldEditor.documentationReference);";
	$LabMenuItem[%id,%itemId++] = "Torque 3D Forums..." TAB "" TAB "gotoWebPage(EWorldEditor.forumURL);";

	if (%buildAfter)
		Lab.buildMenu();
}
//------------------------------------------------------------------------------