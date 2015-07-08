//==============================================================================
// TorqueLab -> RoadEditorPlugin Initialization
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
function initializeRoadEditor() {
	echo( " - Initializing Road and Path Editor" );
	execRoadEd(true);
	
	Lab.createPlugin("RoadEditor","Road Editor");
	Lab.addPluginEditor("RoadEditor",RoadEditorGui);
	Lab.addPluginGui("RoadEditor",RoadEditorTools);
	//Lab.addPluginGui("RoadEditor",RoadEditorOptionsWindow);
	//Lab.addPluginGui("RoadEditor",RoadEditorTreeWindow);
	Lab.addPluginToolbar("RoadEditor",RoadEditorToolbar);
	Lab.addPluginPalette("RoadEditor",   RoadEditorPalette);
	
	RoadEditorPlugin.editorGui = RoadEditorGui;
	$REP = newScriptObject("REP");
	%map = new ActionMap();
	%map.bindCmd( keyboard, "backspace", "RoadEditorGui.onDeleteKey();", "" );
	%map.bindCmd( keyboard, "1", "RoadEditorGui.prepSelectionMode();", "" );
	%map.bindCmd( keyboard, "2", "EWToolsPaletteArray->RoadEditorMoveMode.performClick();", "" );
	%map.bindCmd( keyboard, "4", "EWToolsPaletteArray->RoadEditorScaleMode.performClick();", "" );
	%map.bindCmd( keyboard, "5", "EWToolsPaletteArray->RoadEditorAddRoadMode.performClick();", "" );
	%map.bindCmd( keyboard, "=", "EWToolsPaletteArray->RoadEditorInsertPointMode.performClick();", "" );
	%map.bindCmd( keyboard, "numpadadd", "EWToolsPaletteArray->RoadEditorInsertPointMode.performClick();", "" );
	%map.bindCmd( keyboard, "-", "EWToolsPaletteArray->RoadEditorRemovePointMode.performClick();", "" );
	%map.bindCmd( keyboard, "numpadminus", "EWToolsPaletteArray->RoadEditorRemovePointMode.performClick();", "" );
	%map.bindCmd( keyboard, "z", "RoadEditorShowSplineBtn.performClick();", "" );
	%map.bindCmd( keyboard, "x", "RoadEditorWireframeBtn.performClick();", "" );
	%map.bindCmd( keyboard, "v", "RoadEditorShowRoadBtn.performClick();", "" );
	RoadEditorPlugin.map = %map;
	//RoadEditorPlugin.initSettings();
}
//------------------------------------------------------------------------------
//==============================================================================
// Load all the Scripts and GUIs (if specified)
function execRoadEd(%loadGui) {
	if (%loadGui) {
		exec( "tlab/roadEditor/gui/guiProfiles.cs" );
		exec( "tlab/roadEditor/gui/roadEditorGui.gui" );
		exec( "tlab/roadEditor/gui/RoadEditorTools.gui" );
		exec( "tlab/roadEditor/gui/roadEditorToolbar.gui");
		exec( "tlab/roadEditor/gui/RoadEditorPaletteGui.gui");
	}

	exec( "tlab/roadEditor/roadEditorGui.cs" );
	exec( "tlab/roadEditor/RoadEditorPlugin.cs" );
	execPattern("tlab/roadEditor/editor/*.cs");
	execPattern("tlab/roadEditor/nodeManager/*.cs");
}
//------------------------------------------------------------------------------
//==============================================================================
function destroyRoadEditor() {
}
//------------------------------------------------------------------------------