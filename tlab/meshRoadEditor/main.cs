//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

function initializeMeshRoadEditor() {
	echo(" % - Initializing Mesh Road Editor");
	exec( "./meshRoadEditor.cs" );
	exec( "tlab/meshRoadEditor/gui/meshRoadEditorGui.gui" );
	exec( "./gui/MeshRoadEditorTools.gui" );
	exec( "./gui/meshRoadEditorToolbar.gui");
	exec( "tlab/meshRoadEditor/gui/meshRoadEditorPaletteGui.gui");
	exec( "tlab/meshRoadEditor/meshRoadEditorGui.cs" );
	exec( "tlab/meshRoadEditor/MeshRoadEditorPlugin.cs" );
	exec( "tlab/meshRoadEditor/MeshRoadEditorParams.cs" );
	Lab.createPlugin("MeshRoadEditor","Mesh Road Editor");
	Lab.addPluginEditor("MeshRoadEditor",MeshRoadEditorGui);
	Lab.addPluginGui("MeshRoadEditor",   MeshRoadEditorTools);
	Lab.addPluginToolbar("MeshRoadEditor",MeshRoadEditorToolbar);
	Lab.addPluginPalette("MeshRoadEditor",   MeshRoadEditorPalette);	
	MeshRoadEditorPlugin.editorGui = MeshRoadEditorGui;
	%map = new ActionMap();
	%map.bindCmd( keyboard, "backspace", "MeshRoadEditorGui.deleteNode();", "" );
	%map.bindCmd( keyboard, "1", "MeshRoadEditorGui.prepSelectionMode();", "" );
	%map.bindCmd( keyboard, "2", "EWToolsPaletteArray->MeshRoadEditorMoveMode.performClick();", "" );
	%map.bindCmd( keyboard, "3", "EWToolsPaletteArray->MeshRoadEditorRotateMode.performClick();", "" );
	%map.bindCmd( keyboard, "4", "EWToolsPaletteArray->MeshRoadEditorScaleMode.performClick();", "" );
	%map.bindCmd( keyboard, "5", "EWToolsPaletteArray->MeshRoadEditorAddRoadMode.performClick();", "" );
	%map.bindCmd( keyboard, "=", "EWToolsPaletteArray->MeshRoadEditorInsertPointMode.performClick();", "" );
	%map.bindCmd( keyboard, "numpadadd", "EWToolsPaletteArray->MeshRoadEditorInsertPointMode.performClick();", "" );
	%map.bindCmd( keyboard, "-", "EWToolsPaletteArray->MeshRoadEditorRemovePointMode.performClick();", "" );
	%map.bindCmd( keyboard, "numpadminus", "EWToolsPaletteArray->MeshRoadEditorRemovePointMode.performClick();", "" );
	%map.bindCmd( keyboard, "z", "MeshRoadEditorShowSplineBtn.performClick();", "" );
	%map.bindCmd( keyboard, "x", "MeshRoadEditorWireframeBtn.performClick();", "" );
	%map.bindCmd( keyboard, "v", "MeshRoadEditorShowRoadBtn.performClick();", "" );
	MeshRoadEditorPlugin.map = %map;
}

function destroyMeshRoadEditor() {
}
