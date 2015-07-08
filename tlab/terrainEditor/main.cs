//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
function initializeTerrainEditor() {
	echo( " - Initializing Terrain Editor" );
	execTerrainEd(true);
	
	
	//Add the plugin GUI elements
	//----------------------------------------------
	// Terrain Editor Plugin
	Lab.createPlugin("TerrainEditor","Terrain Editor");
	Lab.addPluginToolbar("TerrainEditor",EWTerrainEditToolbar);
	Lab.addPluginPalette("TerrainEditor",TerrainEditorPalette);
	Lab.addPluginDlg("TerrainEditor",TerrainEditorDialogs);
	TerrainEditorPlugin.PM = new PersistenceManager();
	TerrainEditorPlugin.setEditorMode("Terrain");	
	
	
	
	//----------------------------------------------
	// Terrain Painter Plugin
	Lab.createPlugin("TerrainPainter","Terrain Painter");
	Lab.addPluginGui("TerrainPainter",TerrainPainterTools);
	Lab.addPluginToolbar("TerrainPainter",EWTerrainPainterToolbar);
	Lab.addPluginPalette("TerrainPainter",TerrainPainterPalette);
	Lab.addPluginDlg("TerrainPainter",TerrainPainterDialogs);
	TerrainPainterPlugin.PM = new PersistenceManager();
	TerrainPainterPlugin.setEditorMode("Terrain");	
	
	
	%map = new ActionMap();
	newSimSet("FilteredTerrainMaterialsSet");
	TerrainMaterialDlg-->materialFilter.setText("");
	//Create scriptobject for paint generator
	$TPG = newScriptObject("TPG");
	
	
	/*  %map.bindCmd( keyboard, "1", "LabSceneNoneModeBtn.performClick();", "" ); // Select
	  %map.bindCmd( keyboard, "2", "LabSceneMoveModeBtn.performClick();", "" );   // Move
	  %map.bindCmd( keyboard, "3", "LabSceneRotateModeBtn.performClick();", "" ); // Rotate
	  %map.bindCmd( keyboard, "4", "LabSceneScaleModeBtn.performClick();", "" );  // Scale
	  %map.bindCmd( keyboard, "backspace", "LabScenePlugin.onDeleteKey();", "" );
	  %map.bindCmd( keyboard, "delete", "LabScenePlugin.onDeleteKey();", "" );




	  LabScenePlugin.map = %map;
	  */
	// TerrainEditorPlugin.initSettings();
	// TerrainPainterPlugin.initSettings();
}

function execTerrainEd(%loadGui) {
	//----------------------------------------------
	// Terrain Editor GUIs
	if (%loadGui) {
		exec("tlab/terrainEditor/gui/TerrainCreatorGui.gui" );
		exec("tlab/terrainEditor/gui/TerrainImportGui.gui" );
		exec("tlab/terrainEditor/gui/TerrainExportGui.gui" );
		exec("tlab/terrainEditor/gui/TerrainEditorVSettingsGui.gui");
		exec("tlab/terrainEditor/gui/TerrainEditorPalette.gui");
		exec("tlab/terrainEditor/gui/TerrainEditorToolbar.gui");
		exec("tlab/terrainEditor/gui/TerrainEditorDialogs.gui");
	}

	exec("tlab/terrainEditor/gui/TerrainImportGui.cs");
	exec("tlab/terrainEditor/gui/TerrainExportGui.cs");
	exec("tlab/terrainEditor/gui/TerrainCreatorGui.cs" );


	//----------------------------------------------
	// Terrain Painter GUIs
	if (%loadGui) {
		exec("tlab/terrainEditor/gui/ProceduralTerrainPainterGui.gui" );
		exec("tlab/terrainEditor/gui/TerrainPaintGeneratorGui.gui");
		exec("tlab/terrainEditor/gui/TerrainPainterTools.gui");
		exec("tlab/terrainEditor/gui/guiTerrainMaterialDlg.gui");
		exec("tlab/terrainEditor/gui/TerrainBrushSoftnessCurveDlg.gui");
		exec("tlab/terrainEditor/gui/TerrainPainterToolbar.gui");
		exec("tlab/terrainEditor/gui/TerrainPainterPalette.gui");
		exec("tlab/terrainEditor/gui/TerrainPainterDialogs.gui");
	}

	exec("tlab/terrainEditor/terrainPainterPlugin.cs");
	exec("tlab/terrainEditor/terrainEditorPlugin.cs");

	exec("tlab/terrainEditor/gui/TerrainPaintGeneratorGui.cs");

	execPattern("tlab/terrainEditor/scripts/*.cs");
	execPattern("tlab/terrainEditor/painter/*.cs");
	execPattern("tlab/terrainEditor/editor/*.cs");
	exec("tlab/terrainEditor/terrainMaterials/terrainMaterialDlg.cs");
	exec("tlab/terrainEditor/terrainMaterials/terrainMaterialFilters.cs");
	exec("tlab/terrainEditor/terrainMaterials/terrainMaterialSetup.cs");
}


function destroyLabScene() {
}
